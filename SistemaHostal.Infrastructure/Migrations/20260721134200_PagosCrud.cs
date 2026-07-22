using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace SistemaHostal.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class PagosCrud : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_MetodosDePago_Tipo",
                table: "MetodosDePago");

            migrationBuilder.RenameColumn(
                name: "Tipo",
                table: "MetodosDePago",
                newName: "Estado");

            migrationBuilder.AddColumn<DateTime>(
                name: "FechaCreacion",
                table: "MetodosDePago",
                type: "timestamp with time zone",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.AddColumn<DateTime>(
                name: "FechaModificacion",
                table: "MetodosDePago",
                type: "timestamp with time zone",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Nombre",
                table: "MetodosDePago",
                type: "character varying(50)",
                maxLength: 50,
                nullable: false,
                defaultValue: "");

            migrationBuilder.CreateIndex(
                name: "IX_MetodosDePago_Nombre",
                table: "MetodosDePago",
                column: "Nombre",
                unique: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_MetodosDePago_Nombre",
                table: "MetodosDePago");

            migrationBuilder.DropColumn(
                name: "FechaCreacion",
                table: "MetodosDePago");

            migrationBuilder.DropColumn(
                name: "FechaModificacion",
                table: "MetodosDePago");

            migrationBuilder.DropColumn(
                name: "Nombre",
                table: "MetodosDePago");

            migrationBuilder.RenameColumn(
                name: "Estado",
                table: "MetodosDePago",
                newName: "Tipo");

            migrationBuilder.CreateIndex(
                name: "IX_MetodosDePago_Tipo",
                table: "MetodosDePago",
                column: "Tipo",
                unique: true);
        }
    }
}
