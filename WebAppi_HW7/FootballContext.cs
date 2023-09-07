using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Migrations.Operations;
using System.ComponentModel.DataAnnotations.Schema;
using WebApi_HW7;

namespace WebApi_HW7
{
    public class FootballDbContext : DbContext

    {
        
        public FootballDbContext(DbContextOptions<FootballDbContext> options) : base(options)
        { }
          //  => Database.EnsureCreated();
        //public DbSet<Player> Players = Set<Player>();

        public DbSet<Team> Teams { get; set; }       
        public DbSet<Player> Players { get; set; }
        
    }

    public class Team
    {
        [Key]
        public int Id { get; set; }
        public string Name { get; set; }
        public ICollection<Player> Players { get; set; }
    }
    public class Player
    {
        [Key]
        public int Id { get; set; }
        public int TeamNumber { get; set; }
        public string PlayerName { get; set; }
        
        [ForeignKey(nameof(TeamID))]
        public int TeamID { get; set; }
        public Team Team { get; set; }
    }
}
