// Copyright (c) Microsoft. All rights reserved. Licensed under the MIT license. See full license at the root of the project.

/* 
    This file provides URLs to help get Microsoft Graph data. 
*/

using System.Threading.Tasks;

namespace Office_Add_in_ASPNET_SSO_WebAPI.Helpers
{
    /// <summary>
    /// Provides methods and strings for Microsoft Graph-specific endpoints.
    /// </summary>
    internal static class GraphApiHelper
    {
        // Microsoft Graph-related base URLs
        internal static string GetFilesUrl = @"https://graph.microsoft.com/v1.0/me/drive/root/children";

        // The selectedProperties parameter is a query parameter that will be added
        // to the Microsoft Graph REST API URL. If any part of it comes from user input,
        // be sure that it is sanitized so that it cannot be used in
        // a Response header injection attack.
        internal static string GetOneDriveItemNamesUrl(string selectedProperties)
        {
            // Construct URL for the names of the folders and files.
            return GetFilesUrl + selectedProperties;
        }
    }
}
