// <copyright file="SampleDataController.cs" company="Corp">
// Copyright (c) Corp. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.
// </copyright>

namespace FileAudit.Controllers
{
    using System.Collections.Generic;
    using DataAccess;
    using Microsoft.AspNetCore.Mvc;
    using MimeTypes;
    using File = DataAccess.File;

    /// <summary>
    /// Sample Data Controller.
    /// </summary>
    /// <seealso cref="Microsoft.AspNetCore.Mvc.Controller" />
    [Route("api/[controller]")]
    public class SampleDataController : Controller
    {
        /// <summary>
        /// Files meta data starting from specified start index.
        /// </summary>
        /// <param name="startIndex">The start index.</param>
        /// <returns>Files Meta-data</returns>
        [HttpGet("[action]")]
        public IEnumerable<File> Files(int startIndex)
        {
            var db = new DataBase();
            return db.GetFiles(startIndex);
        }

        /// <summary>
        /// Files the specified identifier.
        /// </summary>
        /// <param name="id">The identifier.</param>
        /// <returns>File</returns>
        [HttpGet("[action]")]
        public FileResult File(int id)
        {
            var db = new DataBase();
            var file = db.GetFullFile(id);
            return this.PhysicalFile(file.LocalFilePath, file.Mime ?? MimeTypeMap.GetMimeType(file.Extension) ?? "application/octet-stream");
        }
    }
}
