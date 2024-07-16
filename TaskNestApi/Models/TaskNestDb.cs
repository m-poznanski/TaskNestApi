using Microsoft.EntityFrameworkCore;

namespace TaskNestApi.Models
{
    class TaskNestDb : DbContext
    {
        public TaskNestDb(DbContextOptions options) : base(options) { }
        public DbSet<Ticket> Tickets {get; set;} = null!;
        public DbSet<User> Users {get; set;} = null!;
        public DbSet<Change> Changes {get; set;} = null!;
    }
}