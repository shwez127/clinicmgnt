﻿// <auto-generated />
using System;
using ClinicData.Data;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;

namespace ClinicData.Migrations
{
    [DbContext(typeof(ClinicDbContext))]
    partial class ClinicDbContextModelSnapshot : ModelSnapshot
    {
        protected override void BuildModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .UseIdentityColumns()
                .HasAnnotation("Relational:MaxIdentifierLength", 128)
                .HasAnnotation("ProductVersion", "5.0.0");

            modelBuilder.Entity("ClinicEntity.Models.Appointment", b =>
                {
                    b.Property<int>("AppointID")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int")
                        .UseIdentityColumn();

                    b.Property<int>("Appointment_Status")
                        .HasColumnType("int");

                    b.Property<float>("Bill_Amount")
                        .HasColumnType("real");

                    b.Property<int>("Bill_Status")
                        .HasColumnType("int");

                    b.Property<DateTime>("Date")
                        .HasColumnType("datetime2");

                    b.Property<string>("Disease")
                        .HasColumnType("varchar(30)");

                    b.Property<int>("DoctorID")
                        .HasColumnType("int");

                    b.Property<int>("PatientID")
                        .HasColumnType("int");

                    b.Property<string>("Prescription")
                        .HasColumnType("varchar(60)");

                    b.Property<string>("Progress")
                        .HasColumnType("varchar(50)");

                    b.HasKey("AppointID");

                    b.HasIndex("DoctorID");

                    b.HasIndex("PatientID");

                    b.ToTable("appointments");
                });

            modelBuilder.Entity("ClinicEntity.Models.Department", b =>
                {
                    b.Property<int>("DeptNo")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int")
                        .UseIdentityColumn();

                    b.Property<string>("DeptName")
                        .IsRequired()
                        .HasColumnType("varchar(20)");

                    b.Property<string>("Description")
                        .HasColumnType("varchar(100)");

                    b.HasKey("DeptNo");

                    b.ToTable("departments");
                });

            modelBuilder.Entity("ClinicEntity.Models.Doctor", b =>
                {
                    b.Property<int>("DoctorID")
                        .HasColumnType("int");

                    b.Property<string>("Address")
                        .HasColumnType("varchar(40)");

                    b.Property<DateTime>("BirthDate")
                        .HasColumnType("datetime2");

                    b.Property<float>("Charges_Per_Visit")
                        .HasColumnType("real");

                    b.Property<int>("Deptno")
                        .HasColumnType("int");

                    b.Property<string>("Gender")
                        .IsRequired()
                        .HasColumnType("char(1)");

                    b.Property<float>("MonthlySalary")
                        .HasColumnType("real");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasColumnType("varchar(20)");

                    b.Property<int>("Patient_Treated")
                        .HasColumnType("int");

                    b.Property<string>("Phone")
                        .HasColumnType("char(12)");

                    b.Property<string>("Qualification")
                        .IsRequired()
                        .HasColumnType("varchar(50)");

                    b.Property<float>("ReputeIndex")
                        .HasColumnType("real");

                    b.Property<string>("Specialization")
                        .HasColumnType("varchar(50)");

                    b.Property<int>("Status")
                        .HasColumnType("int");

                    b.Property<int>("Work_Experience")
                        .HasColumnType("int");

                    b.HasKey("DoctorID");

                    b.HasIndex("Deptno");

                    b.ToTable("doctors");
                });

            modelBuilder.Entity("ClinicEntity.Models.LoginTable", b =>
                {
                    b.Property<int>("ID")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int")
                        .UseIdentityColumn();

                    b.Property<string>("Email")
                        .IsRequired()
                        .HasColumnType("varchar(30)");

                    b.Property<string>("Password")
                        .IsRequired()
                        .HasColumnType("varchar(20)");

                    b.Property<int>("Type")
                        .HasColumnType("int");

                    b.HasKey("ID");

                    b.ToTable("LoginTable");
                });

            modelBuilder.Entity("ClinicEntity.Models.OtherStaff", b =>
                {
                    b.Property<int>("StaffID")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int")
                        .UseIdentityColumn();

                    b.Property<string>("Address")
                        .HasColumnType("varchar(30)");

                    b.Property<DateTime>("BirthDate")
                        .HasColumnType("datetime2");

                    b.Property<string>("Designation")
                        .IsRequired()
                        .HasColumnType("varchar(15)");

                    b.Property<string>("Gender")
                        .IsRequired()
                        .HasColumnType("char(1)");

                    b.Property<string>("Highest_Qualification")
                        .HasColumnType("varchar(20)");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasColumnType("varchar(20)");

                    b.Property<string>("Phone")
                        .HasColumnType("char(12)");

                    b.Property<float>("Salary")
                        .HasColumnType("real");

                    b.HasKey("StaffID");

                    b.ToTable("otherStaffs");
                });

            modelBuilder.Entity("ClinicEntity.Models.Patient", b =>
                {
                    b.Property<int>("PatientID")
                        .HasColumnType("int");

                    b.Property<string>("Address")
                        .HasColumnType("varchar(40)");

                    b.Property<DateTime>("BirthDate")
                        .HasColumnType("datetime2");

                    b.Property<string>("Gender")
                        .IsRequired()
                        .HasColumnType("char(1)");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasColumnType("varchar(20)");

                    b.Property<string>("Phone")
                        .HasColumnType("char(12)");

                    b.HasKey("PatientID");

                    b.ToTable("patients");
                });

            modelBuilder.Entity("ClinicEntity.Models.Pending_Feedback", b =>
                {
                    b.Property<int>("AppointID")
                        .HasColumnType("int");

                    b.Property<int>("DoctorID")
                        .HasColumnType("int");

                    b.Property<int>("PatientID")
                        .HasColumnType("int");

                    b.HasKey("AppointID");

                    b.HasIndex("DoctorID");

                    b.HasIndex("PatientID");

                    b.ToTable("pending_Feedbacks");
                });

            modelBuilder.Entity("ClinicEntity.Models.Appointment", b =>
                {
                    b.HasOne("ClinicEntity.Models.Doctor", "Doctor")
                        .WithMany()
                        .HasForeignKey("DoctorID")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("ClinicEntity.Models.Patient", "Patient")
                        .WithMany()
                        .HasForeignKey("PatientID")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Doctor");

                    b.Navigation("Patient");
                });

            modelBuilder.Entity("ClinicEntity.Models.Doctor", b =>
                {
                    b.HasOne("ClinicEntity.Models.Department", "Department")
                        .WithMany()
                        .HasForeignKey("Deptno")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("ClinicEntity.Models.LoginTable", "LoginTable")
                        .WithMany()
                        .HasForeignKey("DoctorID")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Department");

                    b.Navigation("LoginTable");
                });

            modelBuilder.Entity("ClinicEntity.Models.Patient", b =>
                {
                    b.HasOne("ClinicEntity.Models.LoginTable", "LoginTable")
                        .WithMany()
                        .HasForeignKey("PatientID")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("LoginTable");
                });

            modelBuilder.Entity("ClinicEntity.Models.Pending_Feedback", b =>
                {
                    b.HasOne("ClinicEntity.Models.Appointment", "Appointment")
                        .WithMany()
                        .HasForeignKey("AppointID")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("ClinicEntity.Models.Doctor", "Doctor")
                        .WithMany()
                        .HasForeignKey("DoctorID")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("ClinicEntity.Models.Patient", "Patient")
                        .WithMany()
                        .HasForeignKey("PatientID")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Appointment");

                    b.Navigation("Doctor");

                    b.Navigation("Patient");
                });
#pragma warning restore 612, 618
        }
    }
}
