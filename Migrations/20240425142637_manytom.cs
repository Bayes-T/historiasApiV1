using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ApiHistorias.Migrations
{
    /// <inheritdoc />
    public partial class manytom : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Historias_AspNetUsers_usuarioId",
                table: "Historias");

            migrationBuilder.AlterColumn<string>(
                name: "usuarioId",
                table: "Historias",
                type: "nvarchar(450)",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(450)");

            migrationBuilder.AddForeignKey(
                name: "FK_Historias_AspNetUsers_usuarioId",
                table: "Historias",
                column: "usuarioId",
                principalTable: "AspNetUsers",
                principalColumn: "Id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Historias_AspNetUsers_usuarioId",
                table: "Historias");

            migrationBuilder.AlterColumn<string>(
                name: "usuarioId",
                table: "Historias",
                type: "nvarchar(450)",
                nullable: false,
                defaultValue: "",
                oldClrType: typeof(string),
                oldType: "nvarchar(450)",
                oldNullable: true);

            migrationBuilder.AddForeignKey(
                name: "FK_Historias_AspNetUsers_usuarioId",
                table: "Historias",
                column: "usuarioId",
                principalTable: "AspNetUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.NoAction);
        }
    }
}
