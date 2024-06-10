using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;

namespace ApiTicketRobChat.Models
{
    public class TicketsRobChat
    {
        [Key]
        public int fiIDTicketSolicitud { get; set; }
        public int fiIDTicketSaris { get; set; }
        public DateTime fdFechaCreacion { get; set; }
        public int fiIDTicketTobchat { get; set; }
        public required string fcTelefono { get; set; }
        public required string fcComentario { get; set; }
        public required string fcQueja { get; set; }
        public required string fcNombre { get; set; }
        public required string fcApellido { get; set; }
    }
}
