using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace ClinicEntity.Models
{
    [Table("pending_Feedbacks")]
    public class Pending_Feedback
    {
        
        [ForeignKey("Appointment")]
        [Key]
        public int AppointID { get; set; }
        public Appointment Appointment { get; set; }

        [ForeignKey("Doctor")]
        public int DoctorID { get; set; }
        public Doctor Doctor { get; set; }

        [ForeignKey("Patient")]
        public int PatientID { get; set; }
        public Patient Patient { get; set; }
    }
}
