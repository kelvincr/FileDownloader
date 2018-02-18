// <copyright file="UriExtensionsTest.cs" company="PlaceholderCompany">
// Copyright (c) PlaceholderCompany. All rights reserved.
// </copyright>

namespace Downloader.Test
{
    using System;
    using Extensions;
    using FluentAssertions;
    using Microsoft.VisualStudio.TestTools.UnitTesting;

    /// <summary>
    /// Uri extensions tests.
    /// </summary>
    [TestClass]
    public class UriExtensionsTest
    {
        /// <summary>
        /// Gets the MD5 test.
        /// </summary>
        [TestMethod]
        public void GetMd5Test()
        {
            var uri = new Uri("http://sample.com:80/file?q=123");
            var md5 = uri.GetMd5();
            md5.Should().Be("DED868DA2BBCCC9D1BD97A74F0190976");
        }

        /// <summary>
        /// Cleans the URI test.
        /// </summary>
        [TestMethod]
        public void CleanUriTest()
        {
            var uri = new Uri("http://user:password@sample.com:80/file?q=123");
            var newUri = uri.CleanUri();
            newUri.Should().Be("http://sample.com:80/file?q=123");
        }

        /// <summary>
        /// Gets the file name test.
        /// </summary>
        [TestMethod]
        public void GetFileNameTest()
        {
            var uri = new Uri("http://sample.com:81/file?q=123");
            var fileName = uri.GetFileName();
            fileName.Should().Be("http___sample.com_81_file_q=123");
        }

        /// <summary>
        /// Gets the credentials test.
        /// </summary>
        [TestMethod]
        public void GetCredentialsTest()
        {
            var uri = new Uri("http://user:password@sample.com:80/file?q=123");
            var credentials = uri.GetCredentials();
            credentials.Should().NotBeNull();
        }

        /// <summary>
        /// Gets the no credentials test.
        /// </summary>
        [TestMethod]
        public void GetNoCredentialsTest()
        {
            var uri = new Uri("http://sample.com:80/file?q=123");
            var credentials = uri.GetCredentials();
            credentials.Should().BeNull();
        }
    }
}