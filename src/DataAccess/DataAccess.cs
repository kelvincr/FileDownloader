using System;

namespace DataAccess
{
    using System.Collections.Generic;
    using System.IO;
    using VelocityDb.Session;


    public class DataAccess
    {
        private string defaultDatabasePath = Path.Combine(Path.GetTempPath(), "Downloader");

        public IEnumerable<File> GetFiles(int index)
        {
            using (var session = new SessionNoServer(defaultDatabasePath))
            {
                session.BeginRead();
                var bugTracker = session.Open();
                session.Commit();
                return new File[0];
            }
        }

        public void StoreFile(String server, string name, string filePath, long size, DateTime date)
        {
            using (SessionNoServer session = new SessionNoServer(this.defaultDatabasePath))
            {
                session.BeginUpdate();
                FullFile file = new FullFile()
                {
                    Server = server,
                    Name = name,
                    LocalFilePath = filePath,
                    Size = size,
                    Date = date.ToShortDateString(),
                    Status = Status.PendingToProcess
                };
                session.Persist(file);
                session.Commit();
            }
        }

        public void UpDateFile(int id)
        {

        }
    }
}
