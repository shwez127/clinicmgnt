using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace ClinicEntity.Models
{
    [Table("appointments")]
    public class Appointment
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int AppointID { get; set; }

        [ForeignKey("Doctor")]
        public int DoctorID { get; set; }
        public Doctor Doctor { get; set; }

        [ForeignKey("Patient")]
        public int PatientID { get; set; }
        public Patient Patient { get; set; }

        public DateTime Date { get; set; }

        public int Appointment_Status { get; set; }

        public float Bill_Amount { get; set; }

        public int Bill_Status { get; set; }

        [Column(TypeName = "varchar(30)")]
        public string Disease { get; set; }

        [Column(TypeName = "varchar(50)")]
        public string Progress { get; set; }

        [Column(TypeName = "varchar(60)")]
        public string Prescription { get; set; }
    }
}
