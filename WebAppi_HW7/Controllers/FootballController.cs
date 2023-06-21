using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using WebAppi_Diplom;

namespace WebAppi_HW7.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class FootballController : ControllerBase
    {
        private readonly ITeamService _teamService;

        public FootballController(ITeamService teamService)
        {
            _teamService = teamService;
        }

        // ендпоїнт для отримання всіх команд
        [HttpGet]
        public ActionResult<IEnumerable<string>> GetAllTeams()
        {
            var teams = _teamService.GetAllTeams();
            return Ok(teams);
        }

        // ендпоїнт для додавання нової команди
        [HttpPost]
        public ActionResult AddTeam(string team)
        {
            _teamService.AddTeam(team);
            return Ok();
        }

        // ендпоїнт для зміни команди
        [HttpPut("{id}")]
        public ActionResult UpdateTeam(int id, string updatedTeam)
        {
            _teamService.UpdateTeam(id, updatedTeam);
            return Ok();
        }

        // ендпоїнт для видалення команди
        [HttpDelete("{id}")]
        public ActionResult DeleteTeam(int id)
        {
            _teamService.DeleteTeam(id);
            return Ok();
        }

        // ендпоїнт для отримання конкретної команди
        [HttpGet("{id}")]
        public ActionResult<string> GetTeam(int id)
        {
            var team = _teamService.GetTeam(id);
            if (team != null)
            {
                return Ok(team);
            }
            else
            {
                return NotFound();
            }
        }
    }
}
