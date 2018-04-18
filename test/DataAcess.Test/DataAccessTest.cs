// <copyright file="DataAccessTest.cs" company="PlaceholderCompany">
// Copyright (c) PlaceholderCompany. All rights reserved.
// </copyright>

namespace DataAcess.Test
{
    using System;
    using System.IO;
    using FluentAssertions;
    using Microsoft.VisualStudio.TestTools.UnitTesting;
    using File = DataAccess.File;

    [TestClass]
    public class DataAccessTest
    {
        [TestMethod]
        public void DataAccessStoreAndGetTest()
        {
            var data = new DataAccess.DataAccess(Path.Combine(Path.GetTempPath(), Guid.NewGuid().ToString()));
            data.StoreFile("www.sample.com", "file.txt", @"//Storage/file1.txt", 100, DateTime.Now);
            data.GetFiles(0).Should().NotBeNull().And.Should().BeAssignableTo<File>();
        }

        [TestMethod]
        public void DataAccessUpdateTest()
        {
            var data = new DataAccess.DataAccess(Path.Combine(Path.GetTempPath(), Guid.NewGuid().ToString()));
            data.StoreFile("www.sample.com", "file.txt", @"//Storage/file1.txt", 100, DateTime.Now);
            data.GetFiles(0).Should().NotBeNull().And.Should().BeAssignableTo<File>();
        }
    }
}