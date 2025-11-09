using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Scola360.Academico.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class AdicionandoIdTurmaDisciplina : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            // 1. Remove a PK composta antiga
            migrationBuilder.DropPrimaryKey(
                name: "PK_TurmaDisciplina",
                table: "TurmaDisciplina");

            // 2. Adiciona a coluna Id (inicialmente nullable)
            migrationBuilder.AddColumn<Guid>(
                name: "Id",
                table: "TurmaDisciplina",
                type: "uuid",
                nullable: true);

            // 3. Gera GUIDs para todos os registros existentes
            migrationBuilder.Sql(@"
                UPDATE ""TurmaDisciplina""
                SET ""Id"" = gen_random_uuid()
                WHERE ""Id"" IS NULL;
            ");

            // 4. Altera a coluna Id para NOT NULL
            migrationBuilder.AlterColumn<Guid>(
                name: "Id",
                table: "TurmaDisciplina",
                type: "uuid",
                nullable: false,
                oldClrType: typeof(Guid),
                oldType: "uuid",
                oldNullable: true);

            // 5. Adiciona a nova PK no campo Id
            migrationBuilder.AddPrimaryKey(
                name: "PK_TurmaDisciplina",
                table: "TurmaDisciplina",
                column: "Id");

            // 6. Cria índice único na composição antiga (TurmaId, DisciplinaId, FuncionarioId)
            migrationBuilder.CreateIndex(
                name: "IX_TurmaDisciplina_TurmaId_DisciplinaId_FuncionarioId",
                table: "TurmaDisciplina",
                columns: new[] { "TurmaId", "DisciplinaId", "FuncionarioId" },
                unique: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropPrimaryKey(
                name: "PK_TurmaDisciplina",
                table: "TurmaDisciplina");

            migrationBuilder.DropIndex(
                name: "IX_TurmaDisciplina_TurmaId_DisciplinaId_FuncionarioId",
                table: "TurmaDisciplina");

            migrationBuilder.DropColumn(
                name: "Id",
                table: "TurmaDisciplina");

            migrationBuilder.AddPrimaryKey(
                name: "PK_TurmaDisciplina",
                table: "TurmaDisciplina",
                columns: new[] { "TurmaId", "DisciplinaId", "FuncionarioId" });
        }
    }
}
