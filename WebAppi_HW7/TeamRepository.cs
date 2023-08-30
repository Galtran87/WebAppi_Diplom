using WebAppi_Diplom;
using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;
using System.Collections.Generic;
using WebApi_HW7;
using Microsoft.AspNetCore.Mvc;

namespace WebAppi_Diplom
{
    public class TeamRepository : ITeamRepository
    {
        public TeamRepository()
        {
            // Додав Безпараметричний конструктор бо без нього не працює тест
        }
        
        public FootballDbContext _dbContext;

        public TeamRepository(FootballDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public IEnumerable<string> GetAllTeams()
        {
            return _dbContext.Teams.Select(team => team.Name).ToList();
        }

        public virtual void AddTeam(string team)
        {
            var newTeam = new Team { Name = team };
            _dbContext.Teams.Add(newTeam);
            _dbContext.SaveChanges();
        }

        public void UpdateTeam(int id, string updatedTeam)
        {
            var team = _dbContext.Teams.Find(id);
            if (team != null)
            {
                team.Name = updatedTeam;
                _dbContext.SaveChanges();
            }
        }
        [HttpDelete]
        public void DeleteTeam(int id)
        {
            var team = _dbContext.Teams.Find(id);
            if (team != null)
            {
                _dbContext.Teams.Remove(team);
                _dbContext.SaveChanges();
            }
        }

        public string GetTeam(int id)
        {
            var team = _dbContext.Teams.Find(id);
            if (team != null)
            {
                return team.Name;
            }
            return null;
        }


        public Team GetTeamWithPlayers(int id)
        {
            var team = _dbContext.Teams.Include(t => t.Players).FirstOrDefault(t => t.Id == id);
            return team;
        }

        public void AddPlayerToTeam(int teamId, string playerName)
        {
            var team = _dbContext.Teams
                .Include(t => t.Players)
                .FirstOrDefault(t => t.Id == teamId);

            if (team != null)
            {
                var newPlayer = new Player
                {
                    PlayerName = playerName,
                    TeamNumber = teamId
                };

                team.Players.Add(newPlayer);
                _dbContext.SaveChanges();
            }
        }
    }
}
