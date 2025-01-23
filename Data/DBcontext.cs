using Microsoft.EntityFrameworkCore;
using sambackend.Models; 

namespace sambackend.Data
{
    public class DataContext : DbContext
    {
        public DataContext(DbContextOptions<DataContext> options) : base(options) { }

        public DbSet<User> Users { get; set; }
        public DbSet<StoreToken> StoreTokens { get; set; }
    }
}