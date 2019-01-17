using System.Linq;
using System.Threading.Tasks;
using BackEnd.Data;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace BackEnd.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ConferencesController : ControllerBase
    {
        private readonly ApplicationDbContext _db;

        public ConferencesController(ApplicationDbContext db)
        {
            _db = db;
        }

        // GET: api/Conferences
        [HttpGet]
        public async Task<IActionResult> GetConferences()
        {
            var conferences = await _db.Conferences
                .AsNoTracking()
                .ToListAsync();

            var result = conferences.Select(s => new ConferenceDTO.ConferenceResponse
            {
                ID = s.ID,
                Name = s.Name,
                //Sessions = ??,
                //Tracks = ??
                //Sessions = ??
            });
            return Ok(result);
        }

        // GET: api/Conferences/5
        [HttpGet("{id:int}")]
        public async Task<IActionResult> GetConference([FromRoute] int id)
        {
            var conference = await _db.FindAsync<Conference>(id);
            if (conference == null)
                return NotFound();

            var result = new ConferenceDTO.ConferenceResponse
            {
                ID = conference.ID,
                Name = conference.Name,
                //Sessions = ??,
                //Tracks = ??
                //Sessions = ??
            };
            return Ok(result);
        }

        // POST: api/Conferences
        [HttpPost]
        public async Task<IActionResult> CreateConference([FromBody] ConferenceDTO.Conference input)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var conference = new Conference
            {
                Name = input.Name
            };

            _db.Conferences.Add(conference);
            await _db.SaveChangesAsync();

            var result = new ConferenceDTO.ConferenceResponse
            {
                ID = conference.ID,
                Name = conference.Name,
                //Sessions = ??,
                //Tracks = ??
                //Sessions = ??
            };

            return CreatedAtAction(nameof(GetConference), new { id = conference.ID }, result);
        }

        // PUT: api/Conferences/5
        [HttpPut("{id:int}")]
        public async Task<IActionResult> UpdateConference([FromRoute]int id, [FromBody]ConferenceDTO.Conference input)
        {
            var conference = await _db.FindAsync<Conference>(id);
            if (conference == null)
                return NotFound();

            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            conference.Name = input.Name;
            await _db.SaveChangesAsync();

            var result = new ConferenceDTO.ConferenceResponse
            {
                ID = conference.ID,
                Name = conference.Name,
                //Sessions = ??,
                //Tracks = ??
                //Sessions = ??
            };

            return Ok(result);
        }

        // DELETE: api/Conferences/5
        [HttpDelete("{id:int}")]
        public async Task<IActionResult> DeleteConference([FromRoute] int id)
        {
            var conference = await _db.FindAsync<Conference>(id);
            if (conference == null)
                return NotFound();

            _db.Remove(conference);
            await _db.SaveChangesAsync();

            return NoContent();
        }
    }
}