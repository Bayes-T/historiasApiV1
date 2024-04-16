using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ApiHistorias.Migrations
{
    /// <inheritdoc />
    public partial class userrelation : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "UsuarioId",
                table: "Pacientes",
                type: "nvarchar(450)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "UsuarioId",
                table: "Historias",
                type: "nvarchar(450)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.CreateIndex(
                name: "IX_Pacientes_UsuarioId",
                table: "Pacientes",
                column: "UsuarioId");

            migrationBuilder.CreateIndex(
                name: "IX_Historias_UsuarioId",
                table: "Historias",
                column: "UsuarioId");

            migrationBuilder.AddForeignKey(
                name: "FK_Historias_AspNetUsers_UsuarioId",
                table: "Historias",
                column: "UsuarioId",
                principalTable: "AspNetUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.NoAction);

            migrationBuilder.AddForeignKey(
                name: "FK_Pacientes_AspNetUsers_UsuarioId",
                table: "Pacientes",
                column: "UsuarioId",
                principalTable: "AspNetUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.NoAction);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Historias_AspNetUsers_UsuarioId",
                table: "Historias");

            migrationBuilder.DropForeignKey(
                name: "FK_Pacientes_AspNetUsers_UsuarioId",
                table: "Pacientes");

            migrationBuilder.DropIndex(
                name: "IX_Pacientes_UsuarioId",
                table: "Pacientes");

            migrationBuilder.DropIndex(
                name: "IX_Historias_UsuarioId",
                table: "Historias");

            migrationBuilder.DropColumn(
                name: "UsuarioId",
                table: "Pacientes");

            migrationBuilder.DropColumn(
                name: "UsuarioId",
                table: "Historias");
        }
    }
}
