namespace InfinityDesk.Api.Models
{
    public class Document
    {
        public int Id { get; set; }

        public string FileName { get; set; } = string.Empty;

        public string StoredFileName { get; set; } = string.Empty;

        public int UserId { get; set; }

        public User? User { get; set; }
    }
}