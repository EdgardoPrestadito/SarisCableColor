using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using ApiTicketRobChat.Context;
using ApiTicketRobChat.Models;

namespace ApiTicketRobChat.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class TicketsRobChatsController : ControllerBase
    {
        private readonly AppDbContext _context;

        public TicketsRobChatsController(AppDbContext context)
        {
            _context = context;
        }

        // GET: api/TicketsRobChats
        [HttpGet]
        public async Task<ActionResult<IEnumerable<TicketsRobChat>>> GetTicketRobChat()
        {
            return await _context.TicketRobChat.ToListAsync();
        }

        // GET: api/TicketsRobChats/5
        [HttpGet("{id}")]
        public async Task<ActionResult<TicketsRobChat>> GetTicketsRobChat(int id)
        {
            var ticketsRobChat = await _context.TicketRobChat.FindAsync(id);

            if (ticketsRobChat == null)
            {
                return NotFound();
            }

            return ticketsRobChat;
        }

        // PUT: api/TicketsRobChats/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPut("{id}")]
        public async Task<IActionResult> PutTicketsRobChat(int id, TicketsRobChat ticketsRobChat)
        {
            if (id != ticketsRobChat.fiIDTicketSolicitud)
            {
                return BadRequest();
            }

            _context.Entry(ticketsRobChat).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!TicketsRobChatExists(id))
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

        // POST: api/TicketsRobChats
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost]
        public async Task<ActionResult<TicketsRobChat>> PostTicketsRobChat(TicketsRobChat ticketsRobChat)
        {
            _context.TicketRobChat.Add(ticketsRobChat);
            await _context.SaveChangesAsync();

            return CreatedAtAction("GetTicketsRobChat", new { id = ticketsRobChat.fiIDTicketSolicitud }, ticketsRobChat);
        }

        // DELETE: api/TicketsRobChats/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteTicketsRobChat(int id)
        {
            var ticketsRobChat = await _context.TicketRobChat.FindAsync(id);
            if (ticketsRobChat == null)
            {
                return NotFound();
            }

            _context.TicketRobChat.Remove(ticketsRobChat);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        private bool TicketsRobChatExists(int id)
        {
            return _context.TicketRobChat.Any(e => e.fiIDTicketSolicitud == id);
        }
    }
}
