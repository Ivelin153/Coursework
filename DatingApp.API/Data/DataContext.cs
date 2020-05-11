using AprioriApp.API.Model;
using Microsoft.EntityFrameworkCore;

namespace AprioriApp.API.Data
{
    public class DataContext : DbContext
    {
        public DataContext(DbContextOptions<DataContext> options) : base(options) { }

        public DbSet<User> Users { get; set; }
        public DbSet<File> Files { get; set; }
    }
}