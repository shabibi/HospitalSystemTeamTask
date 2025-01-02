using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace HospitalSystemTeamTask.Migrations
{
    /// <inheritdoc />
    public partial class AddDoctorClinicOneToOne : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "CID",
                table: "Doctors",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.CreateIndex(
                name: "IX_Doctors_CID",
                table: "Doctors",
                column: "CID");

            migrationBuilder.AddForeignKey(
                name: "FK_Doctors_Clinics_CID",
                table: "Doctors",
                column: "CID",
                principalTable: "Clinics",
                principalColumn: "CID",
                onDelete: ReferentialAction.NoAction);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Doctors_Clinics_CID",
                table: "Doctors");

            migrationBuilder.DropIndex(
                name: "IX_Doctors_CID",
                table: "Doctors");

            migrationBuilder.DropColumn(
                name: "CID",
                table: "Doctors");
        }
    }
}
