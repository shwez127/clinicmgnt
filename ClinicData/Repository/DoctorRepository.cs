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
            #region Adding doctor details in the database
            _clinicDbContext.doctors.Add(doctor);
            _clinicDbContext.SaveChanges();
            #endregion
        }

        public void DeleteDoctor(int doctorId)
        {
            #region Deleting doctor details from database
            var doctor = _clinicDbContext.doctors.Find(doctorId);
            _clinicDbContext.doctors.Remove(doctor);
            _clinicDbContext.SaveChanges(); 
            #endregion
        }

        public IEnumerable<Doctor> GetAllDoctors()
        {
            #region Show all exixting details in database
            return _clinicDbContext.doctors.Include(obj=>obj.Department).Include(obj=>obj.LoginTable).ToList();
            #endregion
        }

        public Doctor GetDoctorById(int doctorId)
        {
            #region Search Doctor Details in Database through DoctorID
            Doctor doctor1=new Doctor();
            List<Doctor> doctors= _clinicDbContext.doctors.Include(obj => obj.Department).Include(obj => obj.LoginTable).ToList();
            foreach(var doctor in doctors)
            {
                if(doctor.DoctorID == doctorId)
                {
                    doctor1 = doctor;
                }
            }

            return doctor1;
            #endregion
        }

        public void UpdateDoctor(Doctor doctor)
        {
            #region Updating Doctor details in database
            _clinicDbContext.Entry(doctor).State = EntityState.Modified;
            _clinicDbContext.SaveChanges();
            #endregion
        }
    }
}
