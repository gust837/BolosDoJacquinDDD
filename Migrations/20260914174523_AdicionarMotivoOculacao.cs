using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace BolosDoJacquin.Migrations
{
    /// <inheritdoc />
    public partial class AdicionarMotivoOculacao : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "MotivoOcultacao",
                table: "Avaliacao",
                type: "nvarchar(max)",
                nullable: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "MotivoOcultacao",
                table: "Avaliacao");
        }
    }
}
