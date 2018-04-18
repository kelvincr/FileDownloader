using System;
using System.Collections.Generic;
using System.Text;

namespace DataAccess
{
    public class FullFile : File
    {
        public string LocalFilePath { get; set; }
        public string Extension { get; set; }
    }
}
