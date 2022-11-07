using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace ClinicEntity.Models
{
    [Table("doctors")]
    public class Doctor
    {
        [Key]
        [ForeignKey("LoginTable")]
        
        public int DoctorID { get; set; }
        public LoginTable LoginTable { get; set; }

        [Required]
        [Column(TypeName = "varchar(20)")]
        public string Name { get; set; }

        [Column(TypeName = "char(12)")]
        public string Phone { get; set; }

        [Column(TypeName = "varchar(40)")]
        public string Address { get; set; }

        [Required]
        public DateTime BirthDate { get; set; }

        [Required]
        [Column(TypeName = "char(1)")]
        public string Gender { get; set; }

        [Required]
        [ForeignKey("Department")]
        public int Deptno { get; set; }
        public Department Department { get; set; }

        [Required]
        public float Charges_Per_Visit { get; set; }
        public float MonthlySalary { get; set; }
        public float ReputeIndex { get; set; }

        [Required]
        public int Patient_Treated { get; set; }

        [Required]
        [Column(TypeName = "varchar(50)")]
        public string Qualification { get; set; }

        [Column(TypeName = "varchar(50)")]
        public string Specialization { get; set; }
        public int Work_Experience { get; set; }

        [Required]
        public int Status { get; set; }
    }
}
