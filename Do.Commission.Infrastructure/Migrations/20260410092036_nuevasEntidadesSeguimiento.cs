using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Do.Commission.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class nuevasEntidadesSeguimiento : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "DepartmentId",
                table: "PositionHistory",
                type: "int",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "ProjectId",
                table: "PositionHistory",
                type: "int",
                nullable: true);

            migrationBuilder.CreateTable(
                name: "Departments",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    Name = table.Column<string>(type: "longtext", nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Departments", x => x.Id);
                })
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.CreateTable(
                name: "Projects",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    Name = table.Column<string>(type: "longtext", nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Projects", x => x.Id);
                })
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.CreateIndex(
                name: "IX_PositionHistory_DepartmentId",
                table: "PositionHistory",
                column: "DepartmentId");

            migrationBuilder.CreateIndex(
                name: "IX_PositionHistory_EmployeeId",
                table: "PositionHistory",
                column: "EmployeeId");

            migrationBuilder.CreateIndex(
                name: "IX_PositionHistory_PositionId",
                table: "PositionHistory",
                column: "PositionId");

            migrationBuilder.CreateIndex(
                name: "IX_PositionHistory_ProjectId",
                table: "PositionHistory",
                column: "ProjectId");

            migrationBuilder.AddForeignKey(
                name: "FK_PositionHistory_Departments_DepartmentId",
                table: "PositionHistory",
                column: "DepartmentId",
                principalTable: "Departments",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_PositionHistory_Employees_EmployeeId",
                table: "PositionHistory",
                column: "EmployeeId",
                principalTable: "Employees",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_PositionHistory_Positions_PositionId",
                table: "PositionHistory",
                column: "PositionId",
                principalTable: "Positions",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_PositionHistory_Projects_ProjectId",
                table: "PositionHistory",
                column: "ProjectId",
                principalTable: "Projects",
                principalColumn: "Id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_PositionHistory_Departments_DepartmentId",
                table: "PositionHistory");

            migrationBuilder.DropForeignKey(
                name: "FK_PositionHistory_Employees_EmployeeId",
                table: "PositionHistory");

            migrationBuilder.DropForeignKey(
                name: "FK_PositionHistory_Positions_PositionId",
                table: "PositionHistory");

            migrationBuilder.DropForeignKey(
                name: "FK_PositionHistory_Projects_ProjectId",
                table: "PositionHistory");

            migrationBuilder.DropTable(
                name: "Departments");

            migrationBuilder.DropTable(
                name: "Projects");

            migrationBuilder.DropIndex(
                name: "IX_PositionHistory_DepartmentId",
                table: "PositionHistory");

            migrationBuilder.DropIndex(
                name: "IX_PositionHistory_EmployeeId",
                table: "PositionHistory");

            migrationBuilder.DropIndex(
                name: "IX_PositionHistory_PositionId",
                table: "PositionHistory");

            migrationBuilder.DropIndex(
                name: "IX_PositionHistory_ProjectId",
                table: "PositionHistory");

            migrationBuilder.DropColumn(
                name: "DepartmentId",
                table: "PositionHistory");

            migrationBuilder.DropColumn(
                name: "ProjectId",
                table: "PositionHistory");
        }
    }
}
