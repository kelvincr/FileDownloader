// <copyright file="DataAccessTest.cs" company="PlaceholderCompany">
// Copyright (c) PlaceholderCompany. All rights reserved.
// </copyright>

using System.Linq;

namespace DataAcess.Test
{
    using System;
    using System.IO;
    using FluentAssertions;
    using Microsoft.VisualStudio.TestTools.UnitTesting;
    using File = DataAccess.File;

    /// <summary>
    /// Database tests.
    /// </summary>
    [TestClass]
    public class DataAccessTest
    {
        /// <summary>
        /// Data access store and get test.
        /// </summary>
        [TestMethod]
        public void DataAccessStoreAndGetTest()
        {
            var data = new DataAccess.DataBase(Path.Combine(Path.GetTempPath(), Guid.NewGuid().ToString()));
            data.StoreFile("www.sample.com", "file.txt", @"//Storage/file1.txt", 100, string.Empty, string.Empty, DateTime.Now);
            data.StoreFile("www.sample.com", "file2.txt", @"//Storage/file2.txt", 100, string.Empty, string.Empty, DateTime.Now);
            var files = data.GetFiles(0,1).ToArray();
                files.Should().NotBeNull();
            files.Count().Should().Be(1);
            var file = files.First();
                file.Should().NotBeNull().And.Should().BeAssignableTo<File>();
            file.Name.Should().Be("file.txt");
        }

        /// <summary>
        /// Data access update test.
        /// </summary>
        [TestMethod]
        public void DataAccessUpdateTest()
        {
            var data = new DataAccess.DataBase(Path.Combine(Path.GetTempPath(), Guid.NewGuid().ToString()));
            data.StoreFile("www.sample.com", "file.txt", @"//Storage/file1.txt", 100, string.Empty, string.Empty, DateTime.Now);
            data.GetFiles(0).Should().NotBeNull().And.Should().BeAssignableTo<File>();
        }
    }
}