using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace DataAcess.Test
{
    using System;
    using System.IO;
    using DataAccess;

    [TestClass]
    public class DataAccessTest
    {
        [TestInitialize]
        void Init()
        {
            
        }

        [TestMethod]
        public void StoreFileTest()
        {
            var data =  new DataAccess();
            data.StoreFile("www.sample.com", "file.txt", @"//Storage/file1.txt", 100, DateTime.Now);
        }
    }
}
