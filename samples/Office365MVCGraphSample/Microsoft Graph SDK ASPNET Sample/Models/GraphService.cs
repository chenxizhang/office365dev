/* 
*  Copyright (c) Microsoft. All rights reserved. Licensed under the MIT license. 
*  See LICENSE in the source repository root for complete license information. 
*/
#if MSGRAPH
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;
using Resources;
using Microsoft.Graph;
using System.IO;

namespace Microsoft_Graph_REST_ASPNET_Connect.Models
{
    public class GraphService
    {
        public async Task<string> GetMyEmailAddress(GraphServiceClient graphClient)
        {
            // Get the current user. 
            // This sample only needs the user's email address, so select the mail and userPrincipalName properties.
            // If the mail property isn't defined, userPrincipalName should map to the email for all account types. 
            User me = await graphClient.Me.Request().Select("mail,userPrincipalName").GetAsync();
            return me.Mail ?? me.UserPrincipalName;


        }

        // Send an email message from the current user.
        public async Task SendEmail(GraphServiceClient graphClient, Message message)
        {
            await graphClient.Me.SendMail(message, true).Request().PostAsync();
        }

        // Create the email message.
        public async Task<Message> BuildEmailMessage(GraphServiceClient graphClient, string recipients, string subject)
        {

            // Get current user photo
            Stream photoStream = await GetCurrentUserPhotoStream(graphClient);

            // If the user doesn't have a photo, we use a default photo
            if (photoStream == null)
            {
                photoStream = System.IO.File.OpenRead(System.Web.Hosting.HostingEnvironment.MapPath("/Content/puppies.jpg"));
            }

            MemoryStream photoStreamMS = new MemoryStream();
            // Copy stream to MemoryStream object so that it can be converted to byte array.
            photoStream.CopyTo(photoStreamMS);


            // Upload the Photo
            DriveItem photoFile = await UploadFileToOneDrive(graphClient, photoStreamMS.ToArray());

            // Add the sharing link to the email body.
            Permission sharingLink = await GetSharingLink(graphClient, photoFile.Id);
            string bodyContent = string.Format(Resource.Graph_SendMail_Body_Content, sharingLink.Link.WebUrl);

            //Attach Photo to message
            MessageAttachmentsCollectionPage attachments = new MessageAttachmentsCollectionPage();
            attachments.Add(new FileAttachment
            {
                ODataType = "#microsoft.graph.fileAttachment",
                ContentBytes = photoStreamMS.ToArray(),
                ContentType = "image/png",
                Name = "me.png"
            });

            
            // Prepare the recipient list.
            string[] splitter = { ";" };
            string[] splitRecipientsString = recipients.Split(splitter, StringSplitOptions.RemoveEmptyEntries);
            List<Recipient> recipientList = new List<Recipient>();
            foreach (string recipient in splitRecipientsString)
            {
                recipientList.Add(new Recipient
                {
                    EmailAddress = new EmailAddress
                    {
                        Address = recipient.Trim()
                    }
                });
            }

            // Build the email message.
            Message email = new Message
            {
                Body = new ItemBody
                {
                    Content = Resource.Graph_SendMail_Body_Content,
                    ContentType = BodyType.Html,
                },
                Subject = subject,
                ToRecipients = recipientList
            };
            return email;
        }

        // Gets the stream content of the signed-in user's photo. 
        public async Task<Stream> GetCurrentUserPhotoStream(GraphServiceClient graphClient)
        {
            Stream currentUserPhotoStream = null;

            try
            {
                //Uncomment: this line calls the Microsoft Graph SDK to get the user profile photo
                currentUserPhotoStream = await graphClient.Me.Photo.Content.Request().GetAsync();
            }
            catch (ServiceException se)
            {
                Console.WriteLine(se.Error.Message);
                return null;
            }

            return currentUserPhotoStream;

        }

        // Uploads the specified file to the user's root OneDrive directory.
        public async Task<DriveItem> UploadFileToOneDrive(GraphServiceClient graphClient, byte[] file)
        {
            DriveItem uploadedFile = null;

            try
            {
                //Uncomment: The following shows you the code to upload the file represented the byte area to the user's Root OneDrive folder and call it 'me.png'
                MemoryStream fileStream = new MemoryStream(file);
                uploadedFile = await graphClient.Me.Drive.Root.ItemWithPath("me.png").Content.Request().PutAsync<DriveItem>(fileStream);
            }
            catch (ServiceException ex)
            {
                Console.WriteLine(ex.Error.Message);
                return null;
            }

            return uploadedFile;
        }

        //Get a sharing link based on drive item id
        public async Task<Permission> GetSharingLink(GraphServiceClient graphClient, string Id)
        {
            Permission permission = null;

            try
            {
                //Uncomment: Uses the SDK to create a Sharing Link.
                permission = await graphClient.Me.Drive.Items[Id].CreateLink("view").Request().PostAsync();
            }
            catch (ServiceException se)
            {
                Console.WriteLine(se.Error.Message);
                return null;
            }

            return permission;
        }


    }
}
#endif