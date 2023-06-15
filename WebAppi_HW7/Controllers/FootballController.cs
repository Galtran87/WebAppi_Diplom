using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;

namespace WebAppi_HW7.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class FootballController : ControllerBase
    {
        private static List<string> _teams = new List<string> { "Real Madrid", "Barcelona", "Atletico Madrid", "Sevilla", "Valencia" };

        // ендпоїнт для отримання всіх команд
        [HttpGet]
        public ActionResult<IEnumerable<string>> GetAllTeams()
        {
            return Ok(_teams);
        }

        // ендпоїнт для додавання нової команди
        [HttpPost]
        public ActionResult AddTeam(string team)
        {
            _teams.Add(team);
            return Ok();
        }

        // ендпоїнт для зміни команди
        [HttpPut("{id}")]
        public ActionResult UpdateTeam(int id, string updatedTeam)
        {
            if (id >= 0 && id < _teams.Count)
            {
                _teams[id] = updatedTeam;
                return Ok();
            }
            else
            {
                return NotFound();
            }
        }

        // ендпоїнт для видалення команди
        [HttpDelete("{id}")]
        public ActionResult DeleteTeam(int id)
        {
            if (id >= 0 && id < _teams.Count)
            {
                _teams.RemoveAt(id);
                return Ok();
            }
            else
            {
                return NotFound();
            }
        }

        // ендпоїнт для отримання конкретної команди
        [HttpGet("{id}")]
        public ActionResult<string> GetTeam(int id)
        {
            if (id >= 0 && id < _teams.Count)
            {
                return Ok(_teams[id]);
            }
            else
            {
                return NotFound();
            }
        }
    }
}
