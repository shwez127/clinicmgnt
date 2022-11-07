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
        DoctorService _doctorService;
        public DoctorController(DoctorService doctorService)
        {
            _doctorService = doctorService;
        }

        [HttpGet("GetDoctors")]
        public IEnumerable<Doctor> GetDoctors()
        {
            return _doctorService.GetDoctors();
        }

        [HttpPost("AddDoctor")]
        public IActionResult AddDoctor(Doctor doctor)
        {
            _doctorService.AddDoctor(doctor);
            return Ok("Doctor Added Successfully");
        }

        [HttpPut("UpdateDoctor")]
        public IActionResult UpdateDoctor(Doctor doctor)
        {
            _doctorService.UpdateDoctor(doctor);
            return Ok("Doctor updated Successfully");
        }

        [HttpDelete("DeleteDoctor")]
        public IActionResult DeleteDoctor(int doctorId)
        {
            _doctorService.DeleteDoctor(doctorId);
            return Ok("Doctor deleted Successfully");
        }

        [HttpGet("GetDoctorById")]
        public Doctor GetDoctorById(int doctorId)
        {
            return _doctorService.GetDoctorById(doctorId);
        }
    }
}
