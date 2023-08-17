using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using WebAppi_Diplom;
using WebApi_HW7;
using Microsoft.AspNetCore.Authorization;

namespace WebAppi_HW7.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    //[LogFilter] //изм.3
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
        public ActionResult AddTeam([FromBody] string team) //додано атрибут [FromBody] - Це дозволить правильно бандлити дані з тіла запиту
        {
            _teamService.AddTeam(team);
            return Ok();
        }

        // ендпоїнт для зміни команди
        [HttpPut("{id}")]
        [Authorize("Admin")]
        
        public ActionResult UpdateTeam(int id, string updatedTeam)
        {
            _teamService.UpdateTeam(id, updatedTeam);
            return Ok();
        }

        // ендпоїнт для видалення команди
        [HttpDelete("{id}")]
        [Authorize("Admin")]
        public ActionResult DeleteTeam(int id)
        {
            _teamService.DeleteTeam(id);
            return Ok();
        }

        // ендпоїнт для отримання конкретної команди
        [HttpGet("{id}")]
        public ActionResult<string> GetTeam([FromRoute] int id)
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
        [HttpGet("WithPlayers/{id}")]
        public ActionResult<Team> GetTeamWithPlayers([FromRoute(Name = "id")] int teamId)
        {
            var team = _teamService.GetTeamWithPlayers(teamId);
            if (team != null)
            {
                return Ok(team);
            }
            else
            {
                return NotFound();
            }
        }

        [HttpPost("AddPlayerToTeam")]
        public IActionResult AddPlayerToTeam(int teamId, string playerName)
        {
            try
            {
                _teamService.AddPlayerToTeam(teamId, playerName);
                return Ok("Гравця додано до команди.");
            }
            catch (Exception ex)
            {
                return BadRequest($"Помилка: {ex.Message}");
            }
        }


    }
}
