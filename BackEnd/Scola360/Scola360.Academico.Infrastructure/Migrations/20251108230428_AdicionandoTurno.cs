using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Scola360.Academico.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class AdicionandoTurno : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "Turno",
                table: "Turma",
                type: "integer",
                nullable: false,
                defaultValue: 0);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Turno",
                table: "Turma");
        }
    }
}
