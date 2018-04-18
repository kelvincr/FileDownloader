// <copyright file="DataAccess.cs" company="Corp">
// Copyright (c) Corp. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.
// </copyright>

namespace DataAccess
{
    using System;
    using System.Collections.Generic;
    using System.IO;
    using System.Linq;
    using Extensibility;
    using LiteDB;

    /// <summary>
    ///     Database Access.
    /// </summary>
    /// <seealso cref="Extensibility.IDataAccess" />
    /// <inheritdoc />
    public class DataAccess : IDataAccess
    {
        private readonly string databasePath;

        /// <summary>
        ///     Initializes a new instance of the <see cref="DataAccess" /> class.
        /// </summary>
        /// <param name="databasePath">The database path.</param>
        public DataAccess(string databasePath)
        {
            this.databasePath = databasePath;
            if (!Directory.Exists(this.databasePath))
            {
                Directory.CreateDirectory(this.databasePath);
            }
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="DataAccess"/> class.
        /// </summary>
        public DataAccess()
        : this(Path.Combine(Path.GetTempPath(), "DownloaderFilesDB"))
        {
        }

        /// <inheritdoc />
        public IEnumerable<File> GetFiles(int index, int count = 5)
        {
            using (var db = new LiteDatabase(this.databasePath))
            {
                var col = db.GetCollection<FullFile>("files");
                return col.Find(Query.Between("Id", index, index + count));
            }
        }

        /// <inheritdoc />
        public int StoreFile(string server, string name, string filePath, long size, DateTime date)
        {
            using (var db = new LiteDatabase(this.databasePath))
            {
                var col = db.GetCollection<FullFile>("files");

                var file = new FullFile
                {
                    Server = server,
                    Name = name,
                    LocalFilePath = filePath,
                    Size = size,
                    Date = date.ToShortDateString(),
                    Status = "Pending to process"
                };
                col.EnsureIndex(x => x.Id, true);
                return col.Insert(file);
            }
        }

        /// <inheritdoc />
        public bool UpDateFile(int id, string status)
        {
            using (var db = new LiteDatabase(this.databasePath))
            {
                var col = db.GetCollection<FullFile>("files");
                var file = col.Find(Query.EQ("Id", id), limit: 1).FirstOrDefault();
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
                var file = col.Find(Query.EQ("Id", index), limit: 1).FirstOrDefault();
                return file;
            }
        }
    }
}