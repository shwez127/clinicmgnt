using ClinicBusiness.Services;
using ClinicEntity.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ClinicAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class DoctorController : ControllerBase
    {
        //creating a constructor for accessing DoctorService class to access Business Layer
        DoctorService _doctorService;

        //Constructor
        public DoctorController(DoctorService doctorService)
        {
            _doctorService = doctorService;
        }

        [HttpGet("GetDoctors")]
        public IEnumerable<Doctor> GetDoctors()
        {
            #region Listing all doctors
            return _doctorService.GetDoctors();
            #endregion
        }

        [HttpPost("AddDoctor")]
        public IActionResult AddDoctor(Doctor doctor)
        {
            #region Doctor adding action 
            _doctorService.AddDoctor(doctor);
            return Ok("Doctor Added Successfully");
            #endregion
        }

        [HttpPut("UpdateDoctor")]
        public IActionResult UpdateDoctor(Doctor doctor)
        {
            #region Doctor updation action
            _doctorService.UpdateDoctor(doctor);
            return Ok("Doctor updated Successfully");
            #endregion
        }

        [HttpDelete("DeleteDoctor")]
        public IActionResult DeleteDoctor(int doctorId)
        {
            #region Doctor deletion action
            _doctorService.DeleteDoctor(doctorId);
            return Ok("Doctor deleted Successfully");
            #endregion
        }

        [HttpGet("GetDoctorById")]
        public Doctor GetDoctorById(int doctorId)
        {
            #region Search doctor by doctor ID
            return _doctorService.GetDoctorById(doctorId);
            #endregion
        }
    }
}
