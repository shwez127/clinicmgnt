using ClinicData.Repository;
using ClinicEntity.Models;
using System;
using System.Collections.Generic;
using System.Text;

namespace ClinicBusiness.Services
{
    public class DepartmentService
    {
        IDepartmentRepository _departmentRepository;
        public DepartmentService(IDepartmentRepository departmentRepository)
        {
            _departmentRepository = departmentRepository;
        }

        public void AddDepartment(Department department)
        {
            _departmentRepository.AddDepartment(department);
        }
        public void UpdateDepartment(Department department)
        {
            _departmentRepository.UpdateDepartment(department);
        }
        public void DeleteDepartment(int departmentId)
        {
            _departmentRepository.DeleteDepartment(departmentId);
        }
        public Department GetDepartmentById(int departmentId)
        {
            return _departmentRepository.getDepartmentById(departmentId);
        }
        public IEnumerable<Department> GetDepartments()
        {
            return _departmentRepository.GetDepartments();
        }
    }
}
