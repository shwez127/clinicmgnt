using ClinicEntity.Models;
using Microsoft.EntityFrameworkCore;
using System;


namespace ClinicData.Data
{
    public class ClinicDbContext:DbContext
    {
        public ClinicDbContext()
        {

        }
        public ClinicDbContext(DbContextOptions<ClinicDbContext> options) : base(options)
        {

        }

        public DbSet<Patient> patients { get; set; }
        public DbSet<Doctor> doctors { get; set; }
        public DbSet<OtherStaff> otherStaffs { get; set; }
        public DbSet<Pending_Feedback> pending_feedbacks { get; set; }
        public DbSet<Department> departments { get; set; }
        public DbSet<Appointment> appointments { get; set; }
        public DbSet<LoginTable> logintables { get; set; }


        protected override void OnConfiguring(DbContextOptionsBuilder dbContextOptionsBuilder)
        {

<<<<<<< HEAD
            dbContextOptionsBuilder.UseSqlServer("Data Source=VDC01LTC2106;Initial Catalog=ClinicMngt; Integrated Security=true;");

=======
            dbContextOptionsBuilder.UseSqlServer("Data Source=VDC01LTC2156;Initial Catalog=ClinicMngt; Integrated Security=true;");
>>>>>>> b24e7bdf9f704ea45faa2c8546df7470c458d0c6

        }

    }
}
