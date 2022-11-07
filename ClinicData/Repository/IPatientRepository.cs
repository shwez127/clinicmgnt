using ClinicEntity.Models;
using System;
using System.Collections.Generic;
using System.Text;

namespace ClinicData.Repository
{
    public interface IPatientRepository
    {
        #region All Method for Patients
        void AddPatient(Patient patient);
        void UpdatePatient(Patient patient);    
        void DeletePatient(int patientId);
        Patient GetPatientById(int patientId);
        IEnumerable<Patient> GetAllPatients();
        #endregion
    }
}
