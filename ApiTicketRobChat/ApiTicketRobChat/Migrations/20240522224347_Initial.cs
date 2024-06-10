using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ApiTicketRobChat.Migrations
{
    /// <inheritdoc />
    public partial class Initial : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "TicketRobChat",
                columns: table => new
                {
                    fiIDTicketSolicitud = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    fiIDTicketSaris = table.Column<int>(type: "int", nullable: false),
                    fdFechaCreacion = table.Column<DateTime>(type: "datetime2", nullable: false),
                    fiIDTicketTobchat = table.Column<int>(type: "int", nullable: false),
                    fcTelefono = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    fcComentario = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    fcQueja = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    fcNombre = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    fcApellido = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_TicketRobChat", x => x.fiIDTicketSolicitud);
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "TicketRobChat");
        }
    }
}
