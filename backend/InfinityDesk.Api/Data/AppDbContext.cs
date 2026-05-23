using Microsoft.EntityFrameworkCore;
using InfinityDesk.Api.Models;

namespace InfinityDesk.Api.Data
{
    public class AppDbContext : DbContext
    {
        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options)
        {
        }

        public DbSet<User> Users => Set<User>(); //Set se koristi za pristup DbSetu, ne treba da se inicijalizuje kao lista, EF Core ce se pobrinuti za to

        public DbSet<Note> Notes => Set<Note>();

        public DbSet<TaskItem> Tasks => Set<TaskItem>();

        public DbSet<Document> Documents => Set<Document>();
    }
}