namespace DataAccess
{
    public class File
    {
        public int Id { get; set; }
        public string Server { get; set; }
        public string Name { get; set; }
        public long Size { get; set; }
        public string Date { get; set; }
        public Status Status { get; set; }
    }
}
