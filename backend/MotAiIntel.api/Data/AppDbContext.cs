namespace MotAiIntel.api.Data
{
    using Microsoft.EntityFrameworkCore;
    using MotAiIntel.api.Models;
    using System.Collections.Generic;

    public class AppDbContext : DbContext
    {
        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options) { }

        public DbSet<User> Users => Set<User>();
        public DbSet<SearchHistory> Searches => Set<SearchHistory>();
    }
}
