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
        PatientService _patientService;
        public PatientController(PatientService patientService)
        {
            _patientService = patientService;
        }

        [HttpPost("AddPatient")]
        public IActionResult AddPatient(Patient patient)
        {
            _patientService.AddPatient(patient);
            return Ok("Patient Added Successfully");
        }

        [HttpPut("UpdatePatient")]
        public IActionResult UpdatePatient([FromBody] Patient patient)
        {
            _patientService.UpdatePatient(patient);
            return Ok("Patient Updated successfully!!");
        }

        [HttpDelete("DeletePatient")]
        public IActionResult DeletePatient(int patientId)
        {
            _patientService.DeletePatient(patientId);
            return Ok("Patient Deleted successfully!!");
        }

        [HttpGet("GetMovieById")]
        public Patient GetPatientById(int patientId)
        {
            return _patientService.GetPatientById(patientId);
        }

        [HttpGet("GetAllPatients")]
        public IEnumerable<Patient> GetAllPatients()
        {
            return _patientService.GetAllPatients();
        }
        
    }
}
