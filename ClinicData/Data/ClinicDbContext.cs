using ClinicEntity.Models;
using Microsoft.EntityFrameworkCore;
using System;

namespace ClinicData.Data
{
    public class ClinicDbContext:DbContext
    {
        /*public ClinicDbContext()
        {

        }
        public ClinicDbContext(DbContextOptions<ClinicDbContext> options) : base(options)
        {

        }*/

        public DbSet<Patient> patients { get; set; }
        public DbSet<Doctor> doctors { get; set; }
        public DbSet<OtherStaff> otherStaffs { get; set; }
        public DbSet<Pending_Feedback> pending_feedbacks { get; set; }
        public DbSet<Department> departments { get; set; }
        public DbSet<Appointment> appointments { get; set; }


        protected override void OnConfiguring(DbContextOptionsBuilder dbContextOptionsBuilder)
        {
            dbContextOptionsBuilder.UseSqlServer("Data Source=LAPTOP-62EGJFDT\\SQLEXPRESS;Initial Catalog=ClinicMngt_2; Integrated Security=true;");
        }
    }
}
