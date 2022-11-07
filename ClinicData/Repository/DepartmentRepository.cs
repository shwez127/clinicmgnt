using ClinicData.Data;
using ClinicEntity.Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ClinicData.Repository
{
    public class DepartmentRepository: IDepartmentRepository
    {
        ClinicDbContext _clinicDb;
        public DepartmentRepository(ClinicDbContext clinicDb)
        {
            _clinicDb = clinicDb;
        }
        public void AddDepartment(Department department)
        {
            _clinicDb.departments.Add(department);
            _clinicDb.SaveChanges();
        }
        public void UpdateDepartment(Department department)
        {
            _clinicDb.Entry(department).State = EntityState.Modified;
            _clinicDb.SaveChanges();
        }
        public void DeleteDepartment(int departmentId)
        {
            var department = _clinicDb.departments.Find(departmentId);
            _clinicDb.departments.Remove(department);
            _clinicDb.SaveChanges();
        }
        public Department getDepartmentById(int departmentId)
        {
            return _clinicDb.departments.Find(departmentId);
        }
        public IEnumerable<Department> GetDepartments()
        {
            return _clinicDb.departments.ToList();
        }

    }
}
