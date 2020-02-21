using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using EAM.DAL.Context;
using EAM.DAL.Entity;
using EAM.API.Model;

namespace EAM.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AttendancesController : ControllerBase
    {
        private readonly EamContext _context;

        public AttendancesController(EamContext context)
        {
            _context = context;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<DAL.Entity.Attendance>>> GetAttendances()
        {
            return await _context.Attendances.Include(a => a.Card).ToListAsync();
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<DAL.Entity.Attendance>> GetAttendance(int id)
        {
            var attendance = await _context.Attendances.FindAsync(id);

            if (attendance == null)
            {
                return NotFound();
            }

            return attendance;
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> PutAttendance(int id, DAL.Entity.Attendance attendance)
        {
            if (id != attendance.AttendanceID)
            {
                return BadRequest();
            }

            _context.Entry(attendance).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!AttendanceExists(id))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }

            return NoContent();
        }

        [HttpPost]
        public async Task<ActionResult<RFID>> PostAttendance([FromBody]RFID RFID)
        {
            var card = _context.Cards.FirstOrDefault(c => c.RFID == RFID.UniqueIdentification);
            DAL.Entity.Attendance attendance = new DAL.Entity.Attendance();
            attendance.AttendanceID = 0;
            attendance.CardID = card.CardID;
            attendance.DateTime = new DateTimeOffset(DateTime.Now.ToUniversalTime());
            var markAttendance = _context.Attendances.Add(attendance);
            await _context.SaveChangesAsync();
            return Ok();
        }

        [HttpDelete("{id}")]
        public async Task<ActionResult<DAL.Entity.Attendance>> DeleteAttendance(int id)
        {
            var attendance = await _context.Attendances.FindAsync(id);
            if (attendance == null)
            {
                return NotFound();
            }

            _context.Attendances.Remove(attendance);
            await _context.SaveChangesAsync();

            return attendance;
        }

        private bool AttendanceExists(int id)
        {
            return _context.Attendances.Any(e => e.AttendanceID == id);
        }
    }
}
