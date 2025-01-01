using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace HospitalSystemTeamTask.Migrations
{
    /// <inheritdoc />
    public partial class SetNullClincId : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Doctors_Clinics_CID",
                table: "Doctors");

            migrationBuilder.AlterColumn<int>(
                name: "CID",
                table: "Doctors",
                type: "int",
                nullable: true,
                oldClrType: typeof(int),
                oldType: "int");

            migrationBuilder.AddForeignKey(
                name: "FK_Doctors_Clinics_CID",
                table: "Doctors",
                column: "CID",
                principalTable: "Clinics",
                principalColumn: "CID");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Doctors_Clinics_CID",
                table: "Doctors");

            migrationBuilder.AlterColumn<int>(
                name: "CID",
                table: "Doctors",
                type: "int",
                nullable: false,
                defaultValue: 0,
                oldClrType: typeof(int),
                oldType: "int",
                oldNullable: true);

            migrationBuilder.AddForeignKey(
                name: "FK_Doctors_Clinics_CID",
                table: "Doctors",
                column: "CID",
                principalTable: "Clinics",
                principalColumn: "CID",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
