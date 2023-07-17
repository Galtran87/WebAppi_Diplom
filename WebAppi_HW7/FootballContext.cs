using Microsoft.EntityFrameworkCore;

namespace WebApi_HW7
{
    public class FootballDbContext : DbContext
    {
        public FootballDbContext(DbContextOptions<FootballDbContext> options) : base(options)
        {
        }

        public DbSet<Team> Teams { get; set; }
    }

    public class Team
    {
        public int Id { get; set; }
        public string Name { get; set; }
    }
}
