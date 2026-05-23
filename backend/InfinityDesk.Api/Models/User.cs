using System.Collections.Generic;

namespace InfinityDesk.Api.Models
{
    public class User
    {
        public int Id { get; set; }

        public string Username { get; set; } = string.Empty;

        public string PasswordHash { get; set; } = string.Empty;

        public List<Note> Notes { get; set; } = new List<Note>();

        public List<TaskItem> Tasks { get; set; } = new List<TaskItem>();

        public List<Document> Documents { get; set; } = new List<Document>();
    }
}