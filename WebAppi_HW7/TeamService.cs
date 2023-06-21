using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;

namespace WebAppi_Diplom
{
    public class TeamService : ITeamService
    {
        private readonly TeamRepository _teamRepository;

        public TeamService(TeamRepository teamRepository)
        {
            _teamRepository = teamRepository;
        }

        public IEnumerable<string> GetAllTeams()
        {
            return _teamRepository.GetAllTeams();
        }

        public void AddTeam(string team)
        {
            _teamRepository.AddTeam(team);
        }

        public void UpdateTeam(int id, string updatedTeam)
        {
            _teamRepository.UpdateTeam(id, updatedTeam);
        }

        public void DeleteTeam(int id)
        {
            _teamRepository.DeleteTeam(id);
        }

        public string GetTeam(int id)
        {
            return _teamRepository.GetTeam(id);
        }
    }

}
