using ApiTicketRobChat.Models;
using Microsoft.EntityFrameworkCore;

namespace ApiTicketRobChat.Context
{
    public class AppDbContext: DbContext
    {
        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options)
        {

        }

        public DbSet<TicketsRobChat> TicketRobChat { get; set; }
        public DbSet<Tickets_FuraTiempoViewModel> Tickets_FuraTiempo { get; set; }

    }
}
