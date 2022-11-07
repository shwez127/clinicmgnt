using ClinicData.Repository;
using ClinicEntity.Models;
using System;
using System.Collections.Generic;

namespace ClinicBusiness
{
    public class PatientService
    {
        //creating a constructor for accessing IPatientRepository interface to access Data Layer
        IPatientRepository _patientRepository;
        public PatientService(IPatientRepository patientRepository)
        {
            _patientRepository = patientRepository;
        }

        public void AddPatient(Patient patient)
        {
            _patientRepository.AddPatient(patient);
        }
        public void UpdatePatient(Patient patient)
        {
            _patientRepository.UpdatePatient(patient);
        }
        public void DeletePatient(int patientId)
        {
            _patientRepository.DeletePatient(patientId);
        }
        public Patient GetPatientById(int patientId)
        {
            return _patientRepository.GetPatientById(patientId);
        }
        public IEnumerable<Patient> GetAllPatients()
        {
            return _patientRepository.GetAllPatients();
        }
    }
}
