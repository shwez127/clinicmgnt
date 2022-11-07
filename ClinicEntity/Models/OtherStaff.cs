using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace ClinicEntity.Models
{
    [Table("otherStaffs")]
    public class OtherStaff
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int StaffID { get; set; }

        [Required]
        [Column(TypeName = "varchar(20)")]
        public string Name { get; set; }

        [Column(TypeName = "char(12)")]
        public string Phone  { get; set; }

        [Column(TypeName = "varchar(30)")]
        public string Address { get; set; }

        [Required]
        [Column(TypeName = "varchar(15)")]
        public string Designation { get; set; }

        [Required]
        [Column(TypeName = "char(1)")]
        public string Gender { get; set; }

        public DateTime BirthDate { get; set; }

        [Column(TypeName = "varchar(20)")]
        public string Highest_Qualification { get; set; }

        public float Salary { get; set; }
    }
}
