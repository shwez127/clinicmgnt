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
    public class AppointmentController : ControllerBase
    {
        private AppointmentService _appointmentService;

        public AppointmentController(AppointmentService appointmentService)
        {
            _appointmentService = appointmentService;
        }

        [HttpPost("AddAppointment")]
        public IActionResult AddAppointment(Appointment appointment)
        {
            _appointmentService.AddAppointment(appointment);
            return Ok("Appointment Added Successfully");
        }

        [HttpDelete("DeleteAppointment")]
        public IActionResult DeleteAppointment(int AppointmentID)
        {
            _appointmentService.DeleteAppointment(AppointmentID);
            return Ok("Appointment deleted succesfully");
        }
        [HttpPut("UpdateAppointment")]
        public IActionResult UpdateAppointment([FromBody] Appointment appointment)
        {
            _appointmentService.UpdateAppointment(appointment);
            return Ok("Appointment updated succesfully");
        }

        [HttpGet("GetAppointment")]
        public IEnumerable<Appointment> GetAppointment()
        {
            return _appointmentService.GetAppointment();
        }
    }
}
