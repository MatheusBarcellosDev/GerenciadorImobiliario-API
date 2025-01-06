using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace GerenciadorImobiliario_API.Migrations
{
    /// <inheritdoc />
    public partial class AttIsActiveLead : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<bool>(
                name: "IsActive",
                table: "Lead",
                type: "tinyint(1)",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<DateTime>(
                name: "LastInteractionDate",
                table: "Lead",
                type: "datetime(6)",
                nullable: true);

            migrationBuilder.UpdateData(
                table: "PipelineStage",
                keyColumn: "Id",
                keyValue: 1L,
                column: "Name",
                value: "Esperando Atendimento");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "IsActive",
                table: "Lead");

            migrationBuilder.DropColumn(
                name: "LastInteractionDate",
                table: "Lead");

            migrationBuilder.UpdateData(
                table: "PipelineStage",
                keyColumn: "Id",
                keyValue: 1L,
                column: "Name",
                value: "Novo Lead");
        }
    }
}
