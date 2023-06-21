namespace WebAppi_Diplom
{
    public class TeamRepository
    {
        private readonly List<string> _teams = new List<string> {
        "Real Madrid",
        "Barcelona",
        "Atletico Madrid",
        "Sevilla",
        "Valencia"
    };

        public IEnumerable<string> GetAllTeams()
        {
            return _teams;
        }

        public void AddTeam(string team)
        {
            _teams.Add(team);
        }

        public void UpdateTeam(int id, string updatedTeam)
        {
            if (id >= 0 && id < _teams.Count)
            {
                _teams[id] = updatedTeam;
            }
        }

        public void DeleteTeam(int id)
        {
            if (id >= 0 && id < _teams.Count)
            {
                _teams.RemoveAt(id);
            }
        }

        public string GetTeam(int id)
        {
            if (id >= 0 && id < _teams.Count)
            {
                return _teams[id];
            }
            else
            {
                return null;
            }
        }
    }
}
