using Microsoft.EntityFrameworkCore;

namespace RamTestDb
{
    class MyDbContext : DbContext
    {
        public MyDbContext(DbContextOptions<MyDbContext> options) : base(options) { }
        public DbSet<MyModel> Entities { get; set; }
    }
}
