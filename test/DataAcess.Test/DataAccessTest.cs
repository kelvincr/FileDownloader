// <copyright file="DataAccessTest.cs" company="Corp">
// Copyright (c) Corp. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.
// </copyright>

using DataAccess;

namespace DataAcess.Test
{
    using System;
    using System.IO;
    using System.Linq;
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
            var files = data.GetFiles(0, 1).ToArray();
                files.Should().NotBeNull();
            files.Count().Should().Be(1);
            var file = files.First();
            file.Should().NotBeNull();
            file.Should().BeOfType<FullFile>();
            file.Name.Should().Be("file.txt");
        }

        /// <summary>
        /// Data access update test.
        /// </summary>
        [TestMethod]
        public void DataAccessUpdateTest()
        {
            var data = new DataAccess.DataBase(Path.Combine(Path.GetTempPath(), Guid.NewGuid().ToString()));
            var storedId = data.StoreFile("www.sample.com", "file.txt", @"//Storage/file1.txt", 100, string.Empty, string.Empty, DateTime.Now);
            var file = data.GetFullFile(storedId);
            file.Should().NotBeNull();
            file.Should().BeOfType<FullFile>();
        }
    }
}