using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Downloader.Extensions;
using FluentAssertions;

namespace Downloader.Test
{
    [TestClass]
    public class UriExtensionsTest
    {
        [TestMethod]
        public void GetMd5Test()
        {
            var uri = new Uri("http://sample.com:80/file?q=123");
            var md5 = uri.GetMd5();
            md5.Should().Be("DED868DA2BBCCC9D1BD97A74F0190976");
        }

        [TestMethod]
        public void CleanUriTest()
        {
            var uri = new Uri("http://user:password@sample.com:80/file?q=123");
            var newUri = uri.CleanUri();
            newUri.Should().Be("http://sample.com:80/file?q=123");
        }

        [TestMethod]
        public void GetFileNameTest()
        {
            var uri = new Uri("http://sample.com:81/file?q=123");
            var fileName = uri.GetFileName();
            fileName.Should().Be("http___sample.com_81_file_q=123");

        }

        [TestMethod]
        public void GetCredentialsTest()
        {
            var uri = new Uri("http://user:password@sample.com:80/file?q=123");
            var credentials = uri.GetCredentials();
            credentials.Should().NotBeNull();
        }

        [TestMethod]
        public void GetNoCredentialsTest()
        {
            var uri = new Uri("http://sample.com:80/file?q=123");
            var credentials = uri.GetCredentials();
            credentials.Should().BeNull();
        }
    }
}
