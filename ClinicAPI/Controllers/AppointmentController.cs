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
        //Private member
        private AppointmentService _appointmentService;

        //Constructor
        public AppointmentController(AppointmentService appointmentService)
        {
            _appointmentService = appointmentService;
        }

        [HttpPost("AddAppointment")]
        public IActionResult AddAppointment(Appointment appointment)
        {
            #region Appointment adding action
            _appointmentService.AddAppointment(appointment);
            return Ok("Appointment Added Successfully");
            #endregion
        }

        [HttpDelete("DeleteAppointment")]
        public IActionResult DeleteAppointment(int AppointmentID)
        {
            #region Appointment deletion action
            _appointmentService.DeleteAppointment(AppointmentID);
            return Ok("Appointment deleted succesfully");
            #endregion
        }
        [HttpPut("UpdateAppointment")]
        public IActionResult UpdateAppointment([FromBody] Appointment appointment)
        {
            #region Appointment updation action
            _appointmentService.UpdateAppointment(appointment);
            return Ok("Appointment updated succesfully");
            #endregion
        }

        [HttpGet("GetAppointment")]
        public IEnumerable<Appointment> GetAppointment()
        {
            #region Listing all appointments
            return _appointmentService.GetAppointment();
            #endregion
        }
        [HttpGet("GetAppointmentById")]
        public Appointment GetAppointmentById(int AppointmentId)
        {
            #region Search appointment by Appointment ID
            return _appointmentService.GetByAppointmentId(AppointmentId);
            #endregion
        }
    }
}
