using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace SistemaHostal.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class AnulacionVenta : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<DateTime>(
                name: "FechaHoraAnulacion",
                table: "Ventas",
                type: "timestamp with time zone",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "MotivoAnulacion",
                table: "Ventas",
                type: "text",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "UsuarioAnulacionId",
                table: "Ventas",
                type: "integer",
                nullable: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "FechaHoraAnulacion",
                table: "Ventas");

            migrationBuilder.DropColumn(
                name: "MotivoAnulacion",
                table: "Ventas");

            migrationBuilder.DropColumn(
                name: "UsuarioAnulacionId",
                table: "Ventas");
        }
    }
}
