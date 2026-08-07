using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace SistemaHostal.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class MetodoPagoEnListadoYTransferenciaHabitacion : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<DateTime>(
                name: "FechaHoraTransferencia",
                table: "Ventas",
                type: "timestamp with time zone",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "HabitacionAnterior",
                table: "Ventas",
                type: "text",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "MotivoTransferencia",
                table: "Ventas",
                type: "text",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "UsuarioTransferenciaId",
                table: "Ventas",
                type: "integer",
                nullable: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "FechaHoraTransferencia",
                table: "Ventas");

            migrationBuilder.DropColumn(
                name: "HabitacionAnterior",
                table: "Ventas");

            migrationBuilder.DropColumn(
                name: "MotivoTransferencia",
                table: "Ventas");

            migrationBuilder.DropColumn(
                name: "UsuarioTransferenciaId",
                table: "Ventas");
        }
    }
}
