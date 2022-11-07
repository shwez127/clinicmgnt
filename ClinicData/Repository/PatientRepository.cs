using ClinicData.Data;
using ClinicEntity.Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ClinicData.Repository
{
    public class PatientRepository : IPatientRepository
    {
        //creating a constructor for accessing ClinicDbContext class to access/fetch database
        ClinicDbContext _clinicDb;
        public PatientRepository(ClinicDbContext clinicDb)
        {
            _clinicDb = clinicDb;
        }

        public void AddPatient(Patient patient)
        {
            #region Adding Patient Details in Database
            _clinicDb.Add(patient);
            _clinicDb.SaveChanges();
            #endregion
        }
        public void UpdatePatient(Patient patient)
        {
            #region Updating Patient Details in Database
            _clinicDb.Entry(patient).State = EntityState.Modified;
            _clinicDb.SaveChanges();
            #endregion
        }
        public void DeletePatient(int patientId)
        {
            #region Deleting Patient Details in Database through PatientID
            var patient = _clinicDb.patients.Find(patientId);
            _clinicDb.patients.Remove(patient);
            _clinicDb.SaveChanges();
            #endregion
        }
        public Patient GetPatientById(int patientId)
        {
            #region Search Patient Details in Database through PatientID
            return _clinicDb.patients.Find(patientId);
            #endregion
        }
        public IEnumerable<Patient> GetAllPatients()
        {
            #region Show All Existing Patient Details in Database
            return _clinicDb.patients.ToList();
            #endregion
        }
    }
}
