using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ApiHistorias.Migrations
{
    /// <inheritdoc />
    public partial class entidades : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "ProfesionalId",
                table: "Pacientes",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "PacienteId",
                table: "Historias",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.CreateIndex(
                name: "IX_Pacientes_ProfesionalId",
                table: "Pacientes",
                column: "ProfesionalId");

            migrationBuilder.CreateIndex(
                name: "IX_Historias_PacienteId",
                table: "Historias",
                column: "PacienteId");

            migrationBuilder.AddForeignKey(
                name: "FK_Historias_Pacientes_PacienteId",
                table: "Historias",
                column: "PacienteId",
                principalTable: "Pacientes",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Pacientes_Profesionales_ProfesionalId",
                table: "Pacientes",
                column: "ProfesionalId",
                principalTable: "Profesionales",
                principalColumn: "Id",
                onDelete: ReferentialAction.NoAction);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Historias_Pacientes_PacienteId",
                table: "Historias");

            migrationBuilder.DropForeignKey(
                name: "FK_Pacientes_Profesionales_ProfesionalId",
                table: "Pacientes");

            migrationBuilder.DropIndex(
                name: "IX_Pacientes_ProfesionalId",
                table: "Pacientes");

            migrationBuilder.DropIndex(
                name: "IX_Historias_PacienteId",
                table: "Historias");

            migrationBuilder.DropColumn(
                name: "ProfesionalId",
                table: "Pacientes");

            migrationBuilder.DropColumn(
                name: "PacienteId",
                table: "Historias");
        }
    }
}
