using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Do.Commission.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class ModifyEntities2 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "Deiscription",
                table: "Positions",
                newName: "Description");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "Description",
                table: "Positions",
                newName: "Deiscription");
        }
    }
}
