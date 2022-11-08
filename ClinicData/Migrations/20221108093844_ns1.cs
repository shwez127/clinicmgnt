using Microsoft.EntityFrameworkCore.Migrations;

namespace ClinicData.Migrations
{
    public partial class ns1 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_doctors_LoginTable_DoctorID",
                table: "doctors");

            migrationBuilder.DropForeignKey(
                name: "FK_patients_LoginTable_PatientID",
                table: "patients");

            migrationBuilder.DropPrimaryKey(
                name: "PK_LoginTable",
                table: "LoginTable");

            migrationBuilder.RenameTable(
                name: "LoginTable",
                newName: "logintables");

            migrationBuilder.AddPrimaryKey(
                name: "PK_logintables",
                table: "logintables",
                column: "ID");

            migrationBuilder.AddForeignKey(
                name: "FK_doctors_logintables_DoctorID",
                table: "doctors",
                column: "DoctorID",
                principalTable: "logintables",
                principalColumn: "ID",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_patients_logintables_PatientID",
                table: "patients",
                column: "PatientID",
                principalTable: "logintables",
                principalColumn: "ID",
                onDelete: ReferentialAction.Cascade);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_doctors_logintables_DoctorID",
                table: "doctors");

            migrationBuilder.DropForeignKey(
                name: "FK_patients_logintables_PatientID",
                table: "patients");

            migrationBuilder.DropPrimaryKey(
                name: "PK_logintables",
                table: "logintables");

            migrationBuilder.RenameTable(
                name: "logintables",
                newName: "LoginTable");

            migrationBuilder.AddPrimaryKey(
                name: "PK_LoginTable",
                table: "LoginTable",
                column: "ID");

            migrationBuilder.AddForeignKey(
                name: "FK_doctors_LoginTable_DoctorID",
                table: "doctors",
                column: "DoctorID",
                principalTable: "LoginTable",
                principalColumn: "ID",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_patients_LoginTable_PatientID",
                table: "patients",
                column: "PatientID",
                principalTable: "LoginTable",
                principalColumn: "ID",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
