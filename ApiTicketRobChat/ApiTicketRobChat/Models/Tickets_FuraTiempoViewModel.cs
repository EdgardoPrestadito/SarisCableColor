namespace ApiTicketRobChat.Models
{
    public class Tickets_FuraTiempoViewModel
    {
        public int fiIDRequerimiento { get; set; }
        public string? fcTituloRequerimiento { get; set; }
        public string? fcDescripcionRequerimiento { get; set; }
        public DateTime fdFechaCreacion { get; set; }
        public int fiIDAreaSolicitante { get; set; }
        public int fcAreaSolicitante { get; set; }
        public int fiIDUsuarioSolicitante { get; set; }
        public string? fcNombreCorto { get; set; }
        public int fiIDEstadoRequerimiento { get; set; }
        public string? fcDescripcionEstado { get; set; }
        public string? fcCorreoUsuarioAsignado { get; set; }
        public string? fcCorreoUsuarioSolicitante { get; set; }
        public string? fcCorreoJefeArea { get; set; }
        public string? fcDescripcionCategoria { get; set; }
        public string? fcTipoRequerimiento { get; set; }
        public int minutos { get; set; }

    }
}
