using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace ClinicEntity.Models
{
    [Table("departments")]
    public class Department
    {
        [Key]
        public int DeptNo { get; set; }
        [Required]
        [Column(TypeName = "varchar(20)")]
        public string DeptName { get; set; }
        [Column(TypeName = "varchar(100)")]
        public string Description { get; set; }
    }
}
