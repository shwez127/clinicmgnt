using ClinicData.Data;
using ClinicEntity.Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ClinicData.Repository
{
    public class DoctorRepository: IDoctorRepository
    {
        ClinicDbContext _clinicDbContext;
        public DoctorRepository(ClinicDbContext clinicDbContext)
        {
            _clinicDbContext = clinicDbContext;
        }

        public void AddDoctor(Doctor doctor)
        {
            _clinicDbContext.doctors.Add(doctor);
            _clinicDbContext.SaveChanges();
        }

        public void DeleteDoctor(int doctorId)
        {
            var doctor = _clinicDbContext.doctors.Find(doctorId);
            _clinicDbContext.doctors.Remove(doctor);
            _clinicDbContext.SaveChanges();
        }

        public IEnumerable<Doctor> GetAllDoctors()
        {
            return _clinicDbContext.doctors.ToList();
        }

        public Doctor GetDoctorById(int doctorId)
        {
            return _clinicDbContext.doctors.Find(doctorId);
        }

        public void UpdateDoctor(Doctor doctor)
        {
            _clinicDbContext.Entry(doctor).State = EntityState.Modified;
            _clinicDbContext.SaveChanges();
        }
    }
}
