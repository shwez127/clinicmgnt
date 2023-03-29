using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace ClinicData.Migrations
{
    public partial class db : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "departments",
                columns: table => new
                {
                    DeptNo = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    DeptName = table.Column<string>(type: "varchar(20)", nullable: false),
                    Description = table.Column<string>(type: "varchar(500)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_departments", x => x.DeptNo);
                });

            migrationBuilder.CreateTable(
                name: "logintables",
                columns: table => new
                {
                    ID = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Password = table.Column<string>(type: "varchar(20)", nullable: false),
                    Email = table.Column<string>(type: "varchar(30)", nullable: false),
                    Type = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_logintables", x => x.ID);
                });

            migrationBuilder.CreateTable(
                name: "otherStaffs",
                columns: table => new
                {
                    StaffID = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(type: "varchar(20)", nullable: false),
                    Phone = table.Column<string>(type: "char(12)", nullable: true),
                    Address = table.Column<string>(type: "varchar(30)", nullable: true),
                    Designation = table.Column<string>(type: "varchar(15)", nullable: false),
                    Gender = table.Column<string>(type: "char(1)", nullable: false),
                    BirthDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    Highest_Qualification = table.Column<string>(type: "varchar(20)", nullable: true),
                    Salary = table.Column<float>(type: "real", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_otherStaffs", x => x.StaffID);
                });

            migrationBuilder.CreateTable(
                name: "doctors",
                columns: table => new
                {
                    DoctorID = table.Column<int>(type: "int", nullable: false),
                    Name = table.Column<string>(type: "varchar(20)", nullable: false),
                    Phone = table.Column<string>(type: "char(12)", nullable: true),
                    Address = table.Column<string>(type: "varchar(40)", nullable: true),
                    BirthDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    Gender = table.Column<string>(type: "char(1)", nullable: false),
                    Deptno = table.Column<int>(type: "int", nullable: false),
                    Charges_Per_Visit = table.Column<float>(type: "real", nullable: false),
                    MonthlySalary = table.Column<float>(type: "real", nullable: false),
                    ReputeIndex = table.Column<float>(type: "real", nullable: false),
                    Patient_Treated = table.Column<int>(type: "int", nullable: false),
                    Qualification = table.Column<string>(type: "varchar(50)", nullable: false),
                    Specialization = table.Column<string>(type: "varchar(50)", nullable: true),
                    Work_Experience = table.Column<int>(type: "int", nullable: false),
                    Status = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_doctors", x => x.DoctorID);
                    table.ForeignKey(
                        name: "FK_doctors_departments_Deptno",
                        column: x => x.Deptno,
                        principalTable: "departments",
                        principalColumn: "DeptNo",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_doctors_logintables_DoctorID",
                        column: x => x.DoctorID,
                        principalTable: "logintables",
                        principalColumn: "ID",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "patients",
                columns: table => new
                {
                    PatientID = table.Column<int>(type: "int", nullable: false),
                    Name = table.Column<string>(type: "varchar(20)", nullable: false),
                    Phone = table.Column<string>(type: "char(12)", nullable: true),
                    Address = table.Column<string>(type: "varchar(40)", nullable: true),
                    BirthDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    Gender = table.Column<string>(type: "char(1)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_patients", x => x.PatientID);
                    table.ForeignKey(
                        name: "FK_patients_logintables_PatientID",
                        column: x => x.PatientID,
                        principalTable: "logintables",
                        principalColumn: "ID",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "appointments",
                columns: table => new
                {
                    AppointID = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    DoctorID = table.Column<int>(type: "int", nullable: false),
                    PatientID = table.Column<int>(type: "int", nullable: false),
                    Date = table.Column<DateTime>(type: "datetime2", nullable: false),
                    Appointment_Status = table.Column<int>(type: "int", nullable: false),
                    Bill_Amount = table.Column<float>(type: "real", nullable: false),
                    Bill_Status = table.Column<int>(type: "int", nullable: false),
                    Disease = table.Column<string>(type: "varchar(30)", nullable: true),
                    Progress = table.Column<string>(type: "varchar(50)", nullable: true),
                    Prescription = table.Column<string>(type: "varchar(60)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_appointments", x => x.AppointID);
                    table.ForeignKey(
                        name: "FK_appointments_doctors_DoctorID",
                        column: x => x.DoctorID,
                        principalTable: "doctors",
                        principalColumn: "DoctorID",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_appointments_patients_PatientID",
                        column: x => x.PatientID,
                        principalTable: "patients",
                        principalColumn: "PatientID",
                        onDelete: ReferentialAction.NoAction);
                });

            migrationBuilder.CreateTable(
                name: "pending_Feedbacks",
                columns: table => new
                {
                    AppointID = table.Column<int>(type: "int", nullable: false),
                    Rating = table.Column<int>(type: "int", nullable: false),
                    Comment = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_pending_Feedbacks", x => x.AppointID);
                    table.ForeignKey(
                        name: "FK_pending_Feedbacks_appointments_AppointID",
                        column: x => x.AppointID,
                        principalTable: "appointments",
                        principalColumn: "AppointID",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_appointments_DoctorID",
                table: "appointments",
                column: "DoctorID");

            migrationBuilder.CreateIndex(
                name: "IX_appointments_PatientID",
                table: "appointments",
                column: "PatientID");

            migrationBuilder.CreateIndex(
                name: "IX_doctors_Deptno",
                table: "doctors",
                column: "Deptno");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "otherStaffs");

            migrationBuilder.DropTable(
                name: "pending_Feedbacks");

            migrationBuilder.DropTable(
                name: "appointments");

            migrationBuilder.DropTable(
                name: "doctors");

            migrationBuilder.DropTable(
                name: "patients");

            migrationBuilder.DropTable(
                name: "departments");

            migrationBuilder.DropTable(
                name: "logintables");
        }
    }
}
