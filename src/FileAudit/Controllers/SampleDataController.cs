using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;

namespace FileAudit.Controllers
{
    [Route("api/[controller]")]
    public class SampleDataController : Controller
    {
        [HttpGet("[action]")]
        public IEnumerable<File> Files(int startDateIndex)
        {
            var rng = new Random();
            return Enumerable.Range(1, 5).Select(index => new File
            {
                Id = rng.Next(),
                Server = "www.google.com",
                Name = "test.html",
                Size = 10,
                Date = DateTime.Now.AddDays(index + startDateIndex).ToString("d"),
                Status = "Ready to process"
            });
        }

        [HttpGet("[action]")]
        public FileResult File(int id)
        {
            var fileName = Path.GetFullPath(Path.Combine("wwwroot", "img", "1.jpg"));
            return PhysicalFile(fileName, "image/jpg");
        }



        
    }
}
