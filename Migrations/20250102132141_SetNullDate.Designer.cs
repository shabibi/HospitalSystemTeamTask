﻿// <auto-generated />
using System;
using HospitalSystemTeamTask;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;

#nullable disable

namespace HospitalSystemTeamTask.Migrations
{
    [DbContext(typeof(ApplicationDbContext))]
    [Migration("20250102132141_SetNullDate")]
    partial class SetNullDate
    {
        /// <inheritdoc />
        protected override void BuildTargetModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "9.0.0")
                .HasAnnotation("Relational:MaxIdentifierLength", 128);

            SqlServerModelBuilderExtensions.UseIdentityColumns(modelBuilder);

            modelBuilder.Entity("HospitalSystemTeamTask.Models.Booking", b =>
                {
                    b.Property<int>("BookingID")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("BookingID"));

                    b.Property<DateTime?>("BookingDate")
                        .HasColumnType("datetime2");

                    b.Property<int>("CID")
                        .HasColumnType("int");

                    b.Property<DateTime>("Date")
                        .HasColumnType("datetime2");

                    b.Property<TimeSpan>("EndTime")
                        .HasColumnType("time");

                    b.Property<int?>("PID")
                        .HasColumnType("int");

                    b.Property<TimeSpan>("StartTime")
                        .HasColumnType("time");

                    b.Property<bool>("Staus")
                        .HasColumnType("bit");

                    b.HasKey("BookingID");

                    b.HasIndex("CID");

                    b.HasIndex("PID");

                    b.ToTable("Bookings");
                });

            modelBuilder.Entity("HospitalSystemTeamTask.Models.Branch", b =>
                {
                    b.Property<int>("BID")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("BID"));

                    b.Property<string>("BranchName")
                        .IsRequired()
                        .HasColumnType("nvarchar(450)");

                    b.Property<bool>("IsActive")
                        .HasColumnType("bit");

                    b.Property<string>("Location")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.HasKey("BID");

                    b.HasIndex("BranchName")
                        .IsUnique();

                    b.ToTable("Branches");
                });

            modelBuilder.Entity("HospitalSystemTeamTask.Models.BranchDepartment", b =>
                {
                    b.Property<int>("BID")
                        .HasColumnType("int");

                    b.Property<int>("DepID")
                        .HasColumnType("int");

                    b.Property<int>("DepartmentCapacity")
                        .HasColumnType("int");

                    b.HasKey("BID", "DepID");

                    b.HasIndex("DepID");

                    b.ToTable("branchDepartments");
                });

            modelBuilder.Entity("HospitalSystemTeamTask.Models.Clinic", b =>
                {
                    b.Property<int>("CID")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("CID"));

                    b.Property<int>("AssignDoctor")
                        .HasColumnType("int");

                    b.Property<int>("BID")
                        .HasColumnType("int");

                    b.Property<int>("Capacity")
                        .HasColumnType("int");

                    b.Property<string>("ClincName")
                        .IsRequired()
                        .HasColumnType("nvarchar(450)");

                    b.Property<decimal>("Cost")
                        .HasColumnType("decimal(18,2)");

                    b.Property<int>("DepID")
                        .HasColumnType("int");

                    b.Property<TimeSpan>("EndTime")
                        .HasColumnType("time");

                    b.Property<bool>("IsActive")
                        .HasColumnType("bit");

                    b.Property<int>("SlotDuration")
                        .HasColumnType("int");

                    b.Property<TimeSpan>("StartTime")
                        .HasColumnType("time");

                    b.HasKey("CID");

                    b.HasIndex("AssignDoctor");

                    b.HasIndex("BID");

                    b.HasIndex("ClincName")
                        .IsUnique();

                    b.HasIndex("DepID");

                    b.ToTable("Clinics");
                });

            modelBuilder.Entity("HospitalSystemTeamTask.Models.Department", b =>
                {
                    b.Property<int>("DepID")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("DepID"));

                    b.Property<string>("DepartmentName")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Description")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<bool>("IsActive")
                        .HasColumnType("bit");

                    b.HasKey("DepID");

                    b.ToTable("Departments");
                });

            modelBuilder.Entity("HospitalSystemTeamTask.Models.Doctor", b =>
                {
                    b.Property<int>("DID")
                        .HasColumnType("int");

                    b.Property<int?>("CID")
                        .HasColumnType("int");

                    b.Property<int>("CurrentBrunch")
                        .HasColumnType("int");

                    b.Property<string>("Degree")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<int>("DepId")
                        .HasColumnType("int");

                    b.Property<DateOnly>("JoiningDate")
                        .HasColumnType("date");

                    b.Property<string>("Level")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<int>("WorkingYear")
                        .HasColumnType("int");

                    b.HasKey("DID");

                    b.HasIndex("CID");

                    b.HasIndex("CurrentBrunch");

                    b.HasIndex("DepId");

                    b.ToTable("Doctors");
                });

            modelBuilder.Entity("HospitalSystemTeamTask.Models.Patient", b =>
                {
                    b.Property<int>("PID")
                        .HasColumnType("int");

                    b.Property<int>("Age")
                        .HasColumnType("int");

                    b.Property<string>("Gender")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.HasKey("PID");

                    b.ToTable("Patients");
                });

            modelBuilder.Entity("HospitalSystemTeamTask.Models.PatientRecord", b =>
                {
                    b.Property<int>("RID")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("RID"));

                    b.Property<int>("BID")
                        .HasColumnType("int");

                    b.Property<decimal>("Cost")
                        .HasColumnType("decimal(18,2)");

                    b.Property<int>("DID")
                        .HasColumnType("int");

                    b.Property<string>("Inspection")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<int>("PID")
                        .HasColumnType("int");

                    b.Property<string>("Treatment")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<DateOnly>("VisitDate")
                        .HasColumnType("date");

                    b.Property<TimeSpan>("VisitTime")
                        .HasColumnType("time");

                    b.HasKey("RID");

                    b.HasIndex("BID");

                    b.HasIndex("DID");

                    b.HasIndex("PID");

                    b.ToTable("PatientRecords");
                });

            modelBuilder.Entity("HospitalSystemTeamTask.Models.User", b =>
                {
                    b.Property<int>("UID")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("UID"));

                    b.Property<string>("Email")
                        .IsRequired()
                        .HasColumnType("nvarchar(450)");

                    b.Property<bool>("IsActive")
                        .HasColumnType("bit");

                    b.Property<string>("Password")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Phone")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Role")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("UserName")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.HasKey("UID");

                    b.HasIndex("Email")
                        .IsUnique();

                    b.ToTable("Users");
                });

            modelBuilder.Entity("HospitalSystemTeamTask.Models.Booking", b =>
                {
                    b.HasOne("HospitalSystemTeamTask.Models.Clinic", "Clinic")
                        .WithMany("Bookings")
                        .HasForeignKey("CID")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("HospitalSystemTeamTask.Models.Patient", "Patient")
                        .WithMany("Bookings")
                        .HasForeignKey("PID");

                    b.Navigation("Clinic");

                    b.Navigation("Patient");
                });

            modelBuilder.Entity("HospitalSystemTeamTask.Models.BranchDepartment", b =>
                {
                    b.HasOne("HospitalSystemTeamTask.Models.Branch", "Branch")
                        .WithMany("BranchDepartments")
                        .HasForeignKey("BID")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("HospitalSystemTeamTask.Models.Department", "Department")
                        .WithMany("BranchDepartments")
                        .HasForeignKey("DepID")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Branch");

                    b.Navigation("Department");
                });

            modelBuilder.Entity("HospitalSystemTeamTask.Models.Clinic", b =>
                {
                    b.HasOne("HospitalSystemTeamTask.Models.Doctor", "Doctor")
                        .WithMany()
                        .HasForeignKey("AssignDoctor")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("HospitalSystemTeamTask.Models.Branch", "Branch")
                        .WithMany("Clinics")
                        .HasForeignKey("BID")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("HospitalSystemTeamTask.Models.Department", "Department")
                        .WithMany("Clinics")
                        .HasForeignKey("DepID")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Branch");

                    b.Navigation("Department");

                    b.Navigation("Doctor");
                });

            modelBuilder.Entity("HospitalSystemTeamTask.Models.Doctor", b =>
                {
                    b.HasOne("HospitalSystemTeamTask.Models.Clinic", "Clinic")
                        .WithMany()
                        .HasForeignKey("CID");

                    b.HasOne("HospitalSystemTeamTask.Models.Branch", "Branch")
                        .WithMany("Doctors")
                        .HasForeignKey("CurrentBrunch")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("HospitalSystemTeamTask.Models.User", "User")
                        .WithMany("Doctors")
                        .HasForeignKey("DID")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("HospitalSystemTeamTask.Models.Department", "Department")
                        .WithMany("Doctors")
                        .HasForeignKey("DepId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Branch");

                    b.Navigation("Clinic");

                    b.Navigation("Department");

                    b.Navigation("User");
                });

            modelBuilder.Entity("HospitalSystemTeamTask.Models.Patient", b =>
                {
                    b.HasOne("HospitalSystemTeamTask.Models.User", "User")
                        .WithMany("Patients")
                        .HasForeignKey("PID")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("User");
                });

            modelBuilder.Entity("HospitalSystemTeamTask.Models.PatientRecord", b =>
                {
                    b.HasOne("HospitalSystemTeamTask.Models.Branch", "Branch")
                        .WithMany("PatientRecords")
                        .HasForeignKey("BID")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("HospitalSystemTeamTask.Models.Doctor", "Doctor")
                        .WithMany("PatientRecords")
                        .HasForeignKey("DID")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("HospitalSystemTeamTask.Models.Patient", "Patient")
                        .WithMany("PatientRecords")
                        .HasForeignKey("PID")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Branch");

                    b.Navigation("Doctor");

                    b.Navigation("Patient");
                });

            modelBuilder.Entity("HospitalSystemTeamTask.Models.Branch", b =>
                {
                    b.Navigation("BranchDepartments");

                    b.Navigation("Clinics");

                    b.Navigation("Doctors");

                    b.Navigation("PatientRecords");
                });

            modelBuilder.Entity("HospitalSystemTeamTask.Models.Clinic", b =>
                {
                    b.Navigation("Bookings");
                });

            modelBuilder.Entity("HospitalSystemTeamTask.Models.Department", b =>
                {
                    b.Navigation("BranchDepartments");

                    b.Navigation("Clinics");

                    b.Navigation("Doctors");
                });

            modelBuilder.Entity("HospitalSystemTeamTask.Models.Doctor", b =>
                {
                    b.Navigation("PatientRecords");
                });

            modelBuilder.Entity("HospitalSystemTeamTask.Models.Patient", b =>
                {
                    b.Navigation("Bookings");

                    b.Navigation("PatientRecords");
                });

            modelBuilder.Entity("HospitalSystemTeamTask.Models.User", b =>
                {
                    b.Navigation("Doctors");

                    b.Navigation("Patients");
                });
#pragma warning restore 612, 618
        }
    }
}
