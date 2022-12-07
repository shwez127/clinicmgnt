using ClinicBusiness;
using ClinicEntity.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;

namespace ClinicAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class PatientController : ControllerBase
    {
        //creating a constructor for accessing PatientService class to access Business Layer
        //Private member
        PatientService _patientService;

        //Constructor
        public PatientController(PatientService patientService)
        {
            _patientService = patientService;
        }

        [HttpPost("AddPatient")]
        public IActionResult AddPatient(Patient patient)
        {
            #region Patient adding action
            _patientService.AddPatient(patient);
            return Ok("Patient Added Successfully");
            #endregion
        }

        [HttpPut("UpdatePatient")]
        public IActionResult UpdatePatient([FromBody] Patient patient)
        {
            #region Patient updation action
            _patientService.UpdatePatient(patient);
            return Ok("Patient Updated successfully!!");
            #endregion
        }

        [HttpDelete("DeletePatient")]
        public IActionResult DeletePatient(int patientId)
        {
            #region Patient deletion action
            _patientService.DeletePatient(patientId);
            return Ok("Patient Deleted successfully!!");
            #endregion
        }

        [HttpGet("GetPatientById")]
        public Patient GetPatientById(int patientId)
        {
            #region Search patient by patient ID
            return _patientService.GetPatientById(patientId);
            #endregion
        }

        [HttpGet("GetAllPatients")]
        public IEnumerable<Patient> GetAllPatients()
        {
            #region Listing all patients
            return _patientService.GetAllPatients();
            #endregion
        }

    }
}
