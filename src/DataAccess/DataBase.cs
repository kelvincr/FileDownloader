// <copyright file="DataBase.cs" company="Corp">
// Copyright (c) Corp. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.
// </copyright>

namespace DataAccess
{
    using System;
    using System.Collections.Generic;
    using System.Composition;
    using System.IO;
    using System.Linq;
    using Extensibility;
    using LiteDB;

    /// <summary>
    ///     Database Access.
    /// </summary>
    /// <seealso cref="Extensibility.IDataAccess" />
    /// <inheritdoc />
    [Export(typeof(IDataAccess))]
    public class DataBase : IDataAccess
    {
        private readonly string databasePath;

        /// <summary>
        ///     Initializes a new instance of the <see cref="DataBase" /> class.
        /// </summary>
        /// <param name="databasePath">The database path.</param>
        public DataBase(string databasePath)
        {
            this.databasePath = databasePath;
            var directoryName = Path.GetDirectoryName(this.databasePath);
            if (!string.IsNullOrEmpty(directoryName) && !Directory.Exists(directoryName))
            {
                Directory.CreateDirectory(directoryName);
            }
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="DataBase"/> class.
        /// </summary>
        public DataBase()
        : this(Path.Combine(Path.GetTempPath(), "DownloaderFilesDB", "Data.db"))
        {
        }

        /// <inheritdoc />
        public IEnumerable<File> GetFiles(int index, int count = 5)
        {
            using (var db = new LiteDatabase(this.databasePath))
            {
                var col = db.GetCollection<FullFile>("files");
                return col.Find(Query.All("Id"), index, count);
            }
        }

        /// <inheritdoc />
        public int StoreFile(string server, string name, string filePath, long size, string mime, string extension, DateTime date)
        {
            using (var db = new LiteDatabase(this.databasePath))
            {
                var col = db.GetCollection<FullFile>("files");
                col.EnsureIndex(x => x.Id, true);
                col.EnsureIndex(x => x.LocalFilePath, true);
                var existingFile = col.FindOne(Query.EQ("LocalFilePath", filePath));
                if (existingFile != null)
                {
                    existingFile.Size = size;
                    existingFile.Date = date.ToShortDateString();
                    existingFile.Status = "Ready to process";
                    existingFile.Extension = extension;
                    col.Update(existingFile);
                    return existingFile.Id;
                }
                else
                {
                    var file = new FullFile
                    {
                        Server = server,
                        Name = name,
                        LocalFilePath = filePath,
                        Size = size,
                        Date = date.ToShortDateString(),
                        Status = "Ready to process",
                        Mime = mime,
                        Extension = extension
                    };
                    return col.Insert(file);
                }
            }
        }

        /// <inheritdoc />
        public bool UpDateFile(int id, string status)
        {
            using (var db = new LiteDatabase(this.databasePath))
            {
                var col = db.GetCollection<FullFile>("files");
                var file = col.FindById(id);
                if (file == null)
                {
                    return false;
                }

                file.Status = status;
                return col.Update(id, file);
            }
        }

        /// <inheritdoc />
        public FullFile GetFullFile(int index)
        {
            using (var db = new LiteDatabase(this.databasePath))
            {
                var col = db.GetCollection<FullFile>("files");
                var file = col.FindById(index);
                return file;
            }
        }
    }
}