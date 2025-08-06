using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace WebApplication1.Migrations
{
    /// <inheritdoc />
    public partial class UpdateTrainingCompletionReportTable : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_GeneralDetails_TraineeId",
                table: "GeneralDetails");

            migrationBuilder.DropColumn(
                name: "StudentId",
                table: "TrainingCompletionReports");

            migrationBuilder.DropColumn(
                name: "WrittenTestScore",
                table: "TrainingCompletionReports");

            migrationBuilder.RenameColumn(
                name: "PerformanceRemarks",
                table: "TrainingCompletionReports",
                newName: "VTRId");

            migrationBuilder.RenameColumn(
                name: "CompletionStatus",
                table: "TrainingCompletionReports",
                newName: "Semester");

            migrationBuilder.AddColumn<string>(
                name: "Behaviour",
                table: "TrainingCompletionReports",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "CertificateNumber",
                table: "TrainingCompletionReports",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "College",
                table: "TrainingCompletionReports",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "Course",
                table: "TrainingCompletionReports",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "Department",
                table: "TrainingCompletionReports",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "FullName",
                table: "TrainingCompletionReports",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "Progress",
                table: "TrainingCompletionReports",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.CreateIndex(
                name: "IX_GeneralDetails_TraineeId",
                table: "GeneralDetails",
                column: "TraineeId",
                unique: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_GeneralDetails_TraineeId",
                table: "GeneralDetails");

            migrationBuilder.DropColumn(
                name: "Behaviour",
                table: "TrainingCompletionReports");

            migrationBuilder.DropColumn(
                name: "CertificateNumber",
                table: "TrainingCompletionReports");

            migrationBuilder.DropColumn(
                name: "College",
                table: "TrainingCompletionReports");

            migrationBuilder.DropColumn(
                name: "Course",
                table: "TrainingCompletionReports");

            migrationBuilder.DropColumn(
                name: "Department",
                table: "TrainingCompletionReports");

            migrationBuilder.DropColumn(
                name: "FullName",
                table: "TrainingCompletionReports");

            migrationBuilder.DropColumn(
                name: "Progress",
                table: "TrainingCompletionReports");

            migrationBuilder.RenameColumn(
                name: "VTRId",
                table: "TrainingCompletionReports",
                newName: "PerformanceRemarks");

            migrationBuilder.RenameColumn(
                name: "Semester",
                table: "TrainingCompletionReports",
                newName: "CompletionStatus");

            migrationBuilder.AddColumn<int>(
                name: "StudentId",
                table: "TrainingCompletionReports",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "WrittenTestScore",
                table: "TrainingCompletionReports",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.CreateIndex(
                name: "IX_GeneralDetails_TraineeId",
                table: "GeneralDetails",
                column: "TraineeId");
        }
    }
}
