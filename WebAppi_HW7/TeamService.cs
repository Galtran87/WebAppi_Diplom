using Microsoft.EntityFrameworkCore;
using WebApi_HW7;

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
        public Team GetTeamWithPlayers(int id)
        {
            var team = _teamRepository._dbContext.Teams.Include(t => t.Players).FirstOrDefault(t => t.Id == id);
            return team;
        }
        public void AddPlayerToTeam(int teamId, string playerName)
        {
            var team = _teamRepository._dbContext.Teams
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
                _teamRepository._dbContext.SaveChanges(); 
            }
        }

    }
}