using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ApiHistorias.Migrations
{
    /// <inheritdoc />
    public partial class userrelation2 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Historias_AspNetUsers_UsuarioId",
                table: "Historias");

            migrationBuilder.RenameColumn(
                name: "UsuarioId",
                table: "Historias",
                newName: "usuarioId");

            migrationBuilder.RenameIndex(
                name: "IX_Historias_UsuarioId",
                table: "Historias",
                newName: "IX_Historias_usuarioId");

            migrationBuilder.AddForeignKey(
                name: "FK_Historias_AspNetUsers_usuarioId",
                table: "Historias",
                column: "usuarioId",
                principalTable: "AspNetUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.NoAction);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Historias_AspNetUsers_usuarioId",
                table: "Historias");

            migrationBuilder.RenameColumn(
                name: "usuarioId",
                table: "Historias",
                newName: "UsuarioId");

            migrationBuilder.RenameIndex(
                name: "IX_Historias_usuarioId",
                table: "Historias",
                newName: "IX_Historias_UsuarioId");

            migrationBuilder.AddForeignKey(
                name: "FK_Historias_AspNetUsers_UsuarioId",
                table: "Historias",
                column: "UsuarioId",
                principalTable: "AspNetUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.NoAction);
        }
    }
}
