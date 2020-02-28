using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using EAM.DAL.Context;
using EAM.DAL.Entity;

namespace EAM.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CardsController : ControllerBase
    {
        private readonly EamContext _context;

        public CardsController(EamContext context)
        {
            _context = context;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<Card>>> GetCards()
        {
            return await _context.Cards.ToListAsync();
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<Card>> GetCard(string id)
        {
            var card = await _context.Cards.Include(c => c.User).FirstOrDefaultAsync(c => c.RFID == id);

            if (card == null)
            {
                return NotFound();
            }

            return card;
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> PutCard(string id, Card card)
        {
            var user = _context.Users.Find(card.UserID);
            if(user == null)
            {
                return Ok(new { errorMessage = "User does not Exist." });
            }
            var existingCard = _context.Cards.Any(c => c.RFID == card.RFID);
            if (existingCard)
            {
                return Ok(new { errorMessage = "RFID Card already Exist" });
            }
            var existingCardHolder = _context.Cards.Any(c => c.UserID == card.UserID);
            if (!existingCardHolder)
            {
                return Ok(new { errorMessage = "No RFID Card was alloted to this user before, Add new card" });
            }
            var updatedCard = await _context.Cards.FirstOrDefaultAsync(c => c.UserID == card.UserID);
            updatedCard.RFID = card.RFID;
            if (id != card.RFID || updatedCard == null)
            {
                return BadRequest();
            }

            _context.Entry(updatedCard).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!CardExists(id))
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
        public async Task<ActionResult<Card>> PostCard(Card card)
        {
            var findUser = _context.Users.Find(card.UserID);
            if (findUser == null)
            {
                return Ok(new { errorMessage = "User does not Exist." });
            }
            var existingCard = _context.Cards.Any(c => c.RFID == card.RFID || c.UserID == card.UserID);
            if (existingCard)
            {
                return Ok(new { errorMessage = "RFID Card already Exist" });
            }
            
            _context.Cards.Add(card);
            await _context.SaveChangesAsync();

            return CreatedAtAction("GetCard", new { id = card.CardID }, card);
        }

        [HttpDelete("{id}")]
        public async Task<ActionResult<Card>> DeleteCard(string id)
        {
            var card = await _context.Cards.FirstOrDefaultAsync(c => c.RFID == id);
            // var card = await _context.Cards.FindAsync(id);
            if (card == null)
            {
                return NotFound();
            }

            _context.Cards.Remove(card);
            await _context.SaveChangesAsync();

            return card;
        }

        private bool CardExists(string id)
        {
            return _context.Cards.Any(e => e.RFID == id);
        }
    }
}
