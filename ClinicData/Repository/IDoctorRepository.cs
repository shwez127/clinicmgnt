using ClinicEntity.Models;
using System;
using System.Collections.Generic;
using System.Text;

namespace ClinicData.Repository
{
    public interface IDoctorRepository
    {
        void AddDoctor(Doctor doctor);
        void UpdateDoctor(Doctor doctor);
        void DeleteDoctor(int doctorId);
        Doctor GetDoctorById(int doctorId);
        IEnumerable<Doctor> GetAllDoctors();
    }
}
