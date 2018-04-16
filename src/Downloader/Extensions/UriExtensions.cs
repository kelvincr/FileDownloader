namespace Downloader.Extensions
{
    using System;
    using System.IO;
    using System.Linq;
    using System.Net;
    using System.Security.Cryptography;
    using System.Text;
    using System.Text.RegularExpressions;

    public static class UriExtensions
    {
        public static string GetMd5(this Uri uri)
        {
            var md5 = new MD5CryptoServiceProvider();

            var originalBytes = Encoding.Default.GetBytes(uri.ToString());
            var encodedBytes = md5.ComputeHash(originalBytes);

            return BitConverter.ToString(encodedBytes).Replace("-", string.Empty);
        }

        public static Uri CleanUri(this Uri uri)
        {
            var newUri = Regex.Replace(uri.ToString(), @"\/\/(.+)@+", @"//");
            return new Uri(newUri);
        }

        public static string GetFileName(this Uri uri)
        {
            return Path.GetInvalidFileNameChars().Aggregate(uri.ToString(), (current, c) => current.Replace(c.ToString(), "_"));
        }

        public static ICredentials GetCredentials(this Uri uri)
        {
            var userInfo = uri.UserInfo.Split(':');
            return userInfo.Length == 2 ? new NetworkCredential(userInfo[0], userInfo[1]) : null;
        }
    }
}
