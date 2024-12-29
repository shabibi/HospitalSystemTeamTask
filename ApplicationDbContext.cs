using HospitalSystemTeamTask.Models;
using Microsoft.EntityFrameworkCore;

namespace HospitalSystemTeamTask
{
    public class ApplicationDbContext : DbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options)
        {
        }

        public DbSet<User> Users { get; set; }
        public DbSet<Patient> Patients { get; set; }
        public DbSet<PatientRecord> PatientRecords { get; set; }
        public DbSet<Doctor> Doctors { get; set; }
        public DbSet<Department> Departments { get; set; }
        public DbSet<Branch> Branches { get; set; }
        public DbSet<BranchDepartment> branchDepartments { get; set; }
        public DbSet<Clinic> Clinics { get; set; }
        public DbSet<Booking> Bookings { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<User>()
                        .HasIndex(u => u.Email)
                        .IsUnique();

            modelBuilder.Entity<Clinic>()
                       .HasIndex(u => u.ClincName)
                       .IsUnique();

            modelBuilder.Entity<Branch>()
                       .HasIndex(u => u.BranchName)
                       .IsUnique();
          
        }
       
    }
}
