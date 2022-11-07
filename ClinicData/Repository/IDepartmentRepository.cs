using ClinicEntity.Models;
using System;
using System.Collections.Generic;
using System.Text;

namespace ClinicData.Repository
{
    public interface IDepartmentRepository
    {
        void AddDepartment(Department department);
        void UpdateDepartment(Department department);
        void DeleteDepartment(int departmentId);
        Department getDepartmentById(int departmentId);
        IEnumerable<Department> GetDepartments();
    }
}
