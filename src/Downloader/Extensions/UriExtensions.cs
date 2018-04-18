// <copyright file="UriExtensions.cs" company="Corp">
// Copyright (c) Corp. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.
// </copyright>

namespace Downloader.Extensions
{
    using System;
    using System.IO;
    using System.Linq;
    using System.Net;
    using System.Security.Cryptography;
    using System.Text;
    using System.Text.RegularExpressions;

    /// <summary>
    /// Uri extensions.
    /// </summary>
    public static class UriExtensions
    {
        /// <summary>
        /// Gets the MD5.
        /// </summary>
        /// <param name="uri">The URI.</param>
        /// <returns>get the Md5 from an Uri.</returns>
        public static string GetMd5(this Uri uri)
        {
            var md5 = new MD5CryptoServiceProvider();

            var originalBytes = Encoding.Default.GetBytes(uri.ToString());
            var encodedBytes = md5.ComputeHash(originalBytes);

            return BitConverter.ToString(encodedBytes).Replace("-", string.Empty);
        }

        /// <summary>
        /// Cleans the URI.
        /// </summary>
        /// <param name="uri">The URI.</param>
        /// <returns>A clean Uri without user and password.</returns>
        public static Uri CleanUri(this Uri uri)
        {
            var newUri = Regex.Replace(uri.ToString(), @"\/\/(.+)@+", @"//");
            return new Uri(newUri);
        }

        /// <summary>
        /// Gets the name of the file.
        /// </summary>
        /// <param name="uri">The URI.</param>
        /// <returns>A proper file Name from Uri to use as file name.</returns>
        public static string GetFileName(this Uri uri)
        {
            return Path.GetInvalidFileNameChars()
                .Aggregate(uri.ToString(), (current, c) => current.Replace(c.ToString(), "_"));
        }

        /// <summary>
        /// Gets the credentials.
        /// </summary>
        /// <param name="uri">The URI.</param>
        /// <returns>Tries to extract credentials from the Uri.</returns>
        public static ICredentials GetCredentials(this Uri uri)
        {
            var userInfo = uri.UserInfo.Split(':');
            return userInfo.Length == 2 ? new NetworkCredential(userInfo[0], userInfo[1]) : null;
        }
    }
}