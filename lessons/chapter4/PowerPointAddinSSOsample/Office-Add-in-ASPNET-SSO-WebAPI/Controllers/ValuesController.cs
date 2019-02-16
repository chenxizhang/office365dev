// Copyright (c) Microsoft. All rights reserved. Licensed under the MIT license. See full license at the root of the repo.

/* 
    This file provides controller methods to get data from MS Graph. 
*/

using Microsoft.Identity.Client;
using System.IdentityModel.Tokens;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using System.Web.Http;
using Office_Add_in_ASPNET_SSO_WebAPI.Helpers;
using Office_Add_in_ASPNET_SSO_WebAPI.Models;
using System;
using System.Net;
using System.Net.Http;

namespace Office_Add_in_ASPNET_SSO_WebAPI.Controllers
{
    [Authorize]
    public class ValuesController : ApiController
    {
        // GET api/values
        public async Task<HttpResponseMessage> Get()
        {
            // OWIN middleware validated the audience and issuer, but the scope must also be validated; must contain "access_as_user".
            string[] addinScopes = ClaimsPrincipal.Current.FindFirst("http://schemas.microsoft.com/identity/claims/scope").Value.Split(' ');
            if (addinScopes.Contains("access_as_user"))
            {
                // Get the raw token that the add-in page received from the Office host.
                var bootstrapContext = ClaimsPrincipal.Current.Identities.First().BootstrapContext
                    as BootstrapContext;
                UserAssertion userAssertion = new UserAssertion(bootstrapContext.Token);

                // Get the access token for MS Graph. 
                ClientCredential clientCred = new ClientCredential(ConfigurationManager.AppSettings["ida:Password"]);
                ConfidentialClientApplication cca =
                    new ConfidentialClientApplication(ConfigurationManager.AppSettings["ida:ClientID"],
                                                      "https://localhost:44355", clientCred, null, null);
                string[] graphScopes = { "Files.Read.All" };                
                AuthenticationResult result = null;
                try
                {
                    // The AcquireTokenOnBehalfOfAsync method will first look in the MSAL in memory cache for a
                    // matching access token. Only if there isn't one, does it initiate the "on behalf of" flow
                    // with the Azure AD V2 endpoint.
                    result = await cca.AcquireTokenOnBehalfOfAsync(graphScopes, userAssertion, "https://login.microsoftonline.com/common/oauth2/v2.0");
                }
                catch (MsalServiceException e)
                {
                    // If multi-factor authentication is required by the MS Graph resource and the user 
                    // has not yet provided it, AAD will return "400 Bad Request" with error AADSTS50076 
                    // and a Claims property. MSAL throws a MsalUiRequiredException (which inherits 
                    // from MsalServiceException) with this information. The Claims property value must 
                    // be passed to the client which should pass it to the Office host, which then 
                    // includes it in a request for a new token. AAD will prompt the user for all 
                    // required forms of authentication.
                    if (e.Message.StartsWith("AADSTS50076")) {

                        // The APIs that create HTTP Responses from exceptions don't know about the 
                        // Claims property, so they don't include it. We have to manually create a message
                        // that includes it. A custom Message property, however, blocks the creation of an 
                        // ExceptionMessage property, so the only way to get the error AADSTS50076 to the 
                        // client is to add it to the custom Message. JavaScript in the client will need 
                        // to discover if a response has a Message or ExceptionMessage, so it knows which 
                        // to read. 
                        string responseMessage = String.Format("{{\"AADError\":\"AADSTS50076\",\"Claims\":{0}}}", e.Claims);
                        return SendErrorToClient(HttpStatusCode.Forbidden, null, responseMessage);
                    }

                    // If the call to AAD contained at least one scope (permission) for which neither 
                    // the user nor a tenant administrator has consented (or consent was revoked. 
                    // AAD will return "400 Bad Request" with error AADSTS65001. MSAL throws a 
                    // MsalUiRequiredException with this information. The client should re-call 
                    // getAccessTokenAsync with the option { forceConsent: true }.
                    if ((e.Message.StartsWith("AADSTS65001"))

                    // If the call to AAD contained at least one scope that AAD does not recognize,
                    // AAD returns "400 Bad Request" with error AADSTS70011. MSAL throws a 
                    // MsalUiRequiredException (which inherits from MsalServiceException) with this 
                    // information. The client should inform the user.
                    || (e.Message.StartsWith("AADSTS70011: The provided value for the input parameter 'scope' is not valid.")))
                    {
                        return SendErrorToClient(HttpStatusCode.Forbidden, e, null);
                    }
                    else
                    {
                        // Rethrowing the MsalServiceException will not relay the original 
                        // "400 Bad Request" exception to the client. Instead a "500 Server Error"
                        // is sent.
                        throw e;
                    }                    
                }

                // Get the names of files and folders in OneDrive for Business by using the Microsoft Graph API. Select only properties needed.
                // Note that the parameter is hardcoded. If you reuse this code in a production add-in and any part of the query parameter comes 
                // from user input, be sure that it is sanitized so that it cannot be used in a Response header injection attack.
                var fullOneDriveItemsUrl = GraphApiHelper.GetOneDriveItemNamesUrl("?$select=name&$top=3");

                IEnumerable<OneDriveItem> filesResult;
                try
                {
                    filesResult = await ODataHelper.GetItems<OneDriveItem>(fullOneDriveItemsUrl, result.AccessToken);
                }

                // If the token is invalid, MS Graph sends a "401 Unauthorized" error with the code 
                // "InvalidAuthenticationToken". ASP.NET then throws a RuntimeBinderException. This
                // is also what happens when the token is expired, although MSAL should prevent that
                // from ever happening. In either case, the client should start the process over by 
                // re-calling getAccessTokenAsync. 
                catch (Microsoft.CSharp.RuntimeBinder.RuntimeBinderException e)
                {
                    return SendErrorToClient(HttpStatusCode.Unauthorized, e, null);                    
                }

                // The returned JSON includes OData metadata and eTags that the add-in does not use. 
                // Return to the client-side only the filenames.
                List<string> itemNames = new List<string>();
                foreach (OneDriveItem item in filesResult)
                {
                    itemNames.Add(item.Name);
                }

                var requestMessage = new HttpRequestMessage();
                requestMessage.SetConfiguration(new HttpConfiguration());
                var response = requestMessage.CreateResponse<List<string>>(HttpStatusCode.OK, itemNames); 
                return response;
            }
            // The token from the client does not have "access_as_user" permission.
            return SendErrorToClient(HttpStatusCode.Unauthorized, null, "Missing access_as_user.");
        }

        private HttpResponseMessage SendErrorToClient(HttpStatusCode statusCode, Exception e, string message)
        {
            HttpError error;

            if (e != null)
            {
                error = new HttpError(e, true);
            }
            else
            {
                error = new HttpError(message);
            }
            var requestMessage = new HttpRequestMessage();
            var errorMessage = requestMessage.CreateErrorResponse(statusCode, error);

            return errorMessage;
        }
        
        // GET api/values/5
        public string Get(int id)
        {
            return "value";
        }

        // POST api/values
        public void Post([FromBody]string value)
        {
        }

        // PUT api/values/5
        public void Put(int id, [FromBody]string value)
        {
        }

        // DELETE api/values/5
        public void Delete(int id)
        {
        }
    }
}
