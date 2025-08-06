using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace WebApplication1.Migrations
{
    /// <inheritdoc />
    public partial class DropAttendanceTable : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "DepartmentId",
                table: "Attendances",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.CreateIndex(
                name: "IX_Attendances_DepartmentId",
                table: "Attendances",
                column: "DepartmentId");

            migrationBuilder.AddForeignKey(
                name: "FK_Attendances_Departments_DepartmentId",
                table: "Attendances",
                column: "DepartmentId",
                principalTable: "Departments",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Attendances_Departments_DepartmentId",
                table: "Attendances");

            migrationBuilder.DropIndex(
                name: "IX_Attendances_DepartmentId",
                table: "Attendances");

            migrationBuilder.DropColumn(
                name: "DepartmentId",
                table: "Attendances");
        }
    }
}
