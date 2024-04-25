using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ApiHistorias.Migrations
{
    /// <inheritdoc />
    public partial class pfinal : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "usuarioId",
                table: "Historias",
                type: "nvarchar(450)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.CreateIndex(
                name: "IX_Historias_usuarioId",
                table: "Historias",
                column: "usuarioId");

            migrationBuilder.AddForeignKey(
                name: "FK_Historias_AspNetUsers_usuarioId",
                table: "Historias",
                column: "usuarioId",
                principalTable: "AspNetUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Historias_AspNetUsers_usuarioId",
                table: "Historias");

            migrationBuilder.DropIndex(
                name: "IX_Historias_usuarioId",
                table: "Historias");

            migrationBuilder.DropColumn(
                name: "usuarioId",
                table: "Historias");
        }
    }
}
