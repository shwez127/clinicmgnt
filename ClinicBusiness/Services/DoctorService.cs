using ClinicData.Repository;
using ClinicEntity.Models;
using System;
using System.Collections.Generic;
using System.Text;

namespace ClinicBusiness.Services
{
    public class DoctorService
    {
        IDoctorRepository _doctorRepository;
        public DoctorService(IDoctorRepository doctorRepository)
        {
            _doctorRepository = doctorRepository;
        }
        public void AddDoctor(Doctor doctor)
        {
            _doctorRepository.AddDoctor(doctor);
        }
        public void DeleteDoctor(int doctorId)
        {
            _doctorRepository.DeleteDoctor(doctorId);
        }
        public void UpdateDoctor(Doctor doctor)
        {
            _doctorRepository.UpdateDoctor(doctor);
        }
        public Doctor GetDoctorById(int doctorId)
        {
            return _doctorRepository.GetDoctorById(doctorId);
        }
        public IEnumerable<Doctor> GetDoctors()

        {
            return _doctorRepository.GetAllDoctors();
        }
    }
}
