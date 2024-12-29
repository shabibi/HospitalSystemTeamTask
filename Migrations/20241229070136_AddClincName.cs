using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace HospitalSystemTeamTask.Migrations
{
    /// <inheritdoc />
    public partial class AddClincName : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "ClincName",
                table: "Clinics",
                type: "nvarchar(450)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AlterColumn<string>(
                name: "BranchName",
                table: "Branches",
                type: "nvarchar(450)",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(max )");

            migrationBuilder.CreateIndex(
                name: "IX_Clinics_ClincName",
                table: "Clinics",
                column: "ClincName",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Branches_BranchName",
                table: "Branches",
                column: "BranchName",
                unique: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_Clinics_ClincName",
                table: "Clinics");

            migrationBuilder.DropIndex(
                name: "IX_Branches_BranchName",
                table: "Branches");

            migrationBuilder.DropColumn(
                name: "ClincName",
                table: "Clinics");

            migrationBuilder.AlterColumn<string>(
                name: "BranchName",
                table: "Branches",
                type: "nvarchar(max)",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(450)");
        }
    }
}
