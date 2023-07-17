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
        private readonly FootballDbContext _dbContext;

        public TeamRepository(FootballDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public IEnumerable<string> GetAllTeams()
        {
            return _dbContext.Teams.Select(team => team.Name).ToList();
        }

        public void AddTeam(string team)
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
            return team?.Name;
        }
    }
}
