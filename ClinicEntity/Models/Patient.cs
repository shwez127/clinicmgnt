using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace ClinicEntity.Models
{
    [Table("patients")]
    public class Patient
    {
        [Key]
        [ForeignKey("LoginTable")]
        public int PatientID { get; set; }
        public LoginTable LoginTable { get; set; }

        [Required]
        [Column(TypeName ="varchar(20)")]
        
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

    }
}
