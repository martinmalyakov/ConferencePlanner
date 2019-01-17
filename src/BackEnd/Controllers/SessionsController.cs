﻿using System.Linq;
using System.Threading.Tasks;
using BackEnd.Data;
using BackEnd.Infrastructure;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace BackEnd.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class SessionsController : ControllerBase
    {
        private readonly ApplicationDbContext _db;

        public SessionsController(ApplicationDbContext db)
        {
            _db = db;
        }

        // GET: api/Sessions
        [HttpGet]
        public async Task<IActionResult> GetSessions()
        {
            var sessions = await _db.Sessions
                .AsNoTracking()
                .Include(s => s.Track)
                .Include(s => s.SessionSpeakers)
                .ThenInclude(ss => ss.Speaker)
                .Include(s => s.SessionTags)
                .ThenInclude(st => st.Tag)
                .ToListAsync();

            var results = sessions.Select(s => s.MapSessionResponse());
            return Ok(results);
        }

        // GET: api/Sessions/5
        [HttpGet("{id:int}")]
        public async Task<IActionResult> GetSession([FromRoute]int id)
        {
            var session = await _db.Sessions
                .AsNoTracking()
                .Include(s => s.Track)
                .Include(s => s.SessionSpeakers)
                .ThenInclude(ss => ss.Speaker)
                .Include(s => s.SessionTags)
                .ThenInclude(st => st.Tag)
                .SingleOrDefaultAsync(s => s.ID == id);

            if (session == null)
                return NotFound();

            var result = session.MapSessionResponse();
            return Ok(result);
        }

        // POST: api/Sessions
        [HttpPost]
        public async Task<IActionResult> CreateSession([FromBody]ConferenceDTO.Session input)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var session = new Session
            {
                Title = input.Title,
                ConferenceID = input.ConferenceID,
                StartTime = input.StartTime,
                EndTime = input.EndTime,
                Abstract = input.Abstract,
                TrackId = input.TrackId
            };

            _db.Sessions.Add(session);
            await _db.SaveChangesAsync();

            var result = session.MapSessionResponse();
            return CreatedAtAction(nameof(GetSession), new { id = result.ID }, result);
        }

        // PUT: api/Sessions/5
        [HttpPut("{id:int}")]
        public async Task<IActionResult> UpdateSession([FromRoute]int id, [FromBody]ConferenceDTO.Session input)
        {
            var session = await _db.FindAsync<Session>(id);
            if (session == null)
                return NotFound();

            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            session.ID = input.ID;
            session.Title = input.Title;
            session.Abstract = input.Abstract;
            session.StartTime = input.StartTime;
            session.EndTime = input.EndTime;
            session.TrackId = input.TrackId;
            session.ConferenceID = input.ConferenceID;

            await _db.SaveChangesAsync();

            var result = session.MapSessionResponse();
            return Ok(result);
        }

        // DELETE: api/Sessions/5
        [HttpDelete("{id:int}")]
        public async Task<IActionResult> DeleteSession(int id)
        {
            var session = await _db.FindAsync<Session>(id);
            if (session == null)
                return NotFound();

            _db.Sessions.Remove(session);
            await _db.SaveChangesAsync();

            return NoContent();
        }
    }
}