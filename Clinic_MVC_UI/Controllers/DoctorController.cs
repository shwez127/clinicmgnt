using ClinicEntity.Models;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using System.Net.Http;
using System.Threading.Tasks;
using System;
using Microsoft.Extensions.Configuration;
using System.Collections.Generic;

namespace Clinic_MVC_UI.Controllers
{
    public class DoctorController : Controller
    {

        private IConfiguration _configuration;
        public DoctorController(IConfiguration configuration)
        {
            _configuration = configuration;
        }
        public IActionResult Index()
        {
            return View();
        }
        public async Task<IActionResult> Profile()
        {
            int doctorProfileId = Convert.ToInt32(TempData["ProfileID"]);
            TempData.Keep();

            Doctor doctor = null;
            using (HttpClient client = new HttpClient())
            {
                string endpoint = _configuration["WebApiBaseUrl"] + "Doctor/GetDoctorById?doctorId=" + doctorProfileId;
                using (var response = await client.GetAsync(endpoint))
                {
                    if (response.StatusCode == System.Net.HttpStatusCode.OK)
                    {
                        var result = await response.Content.ReadAsStringAsync();
                        doctor = JsonConvert.DeserializeObject<Doctor>(result);
                    }



                }
            }
            return View(doctor);
        }
        [HttpGet]
        public async Task<IActionResult> PendingAppointment()
        {
            #region Doctor can see His Appointments
            int doctorappointId = Convert.ToInt32(TempData["ProfileID"]);
            TempData.Keep();
            IEnumerable<Appointment> Appointments = null;
            using (HttpClient client = new HttpClient())
            {
                string endPoint = _configuration["WebApiBaseUrl"] + "Appointment/GetAppointment";
                using (var response = await client.GetAsync(endPoint))
                {
                    if (response.StatusCode == System.Net.HttpStatusCode.OK)
                    {
                        var result = await response.Content.ReadAsStringAsync();
                        Appointments = JsonConvert.DeserializeObject<IEnumerable<Appointment>>(result);
                    }
                }
            }
             List<Appointment> DoctorAppointments= new List<Appointment>();
            foreach(var item in Appointments)
            {
                if (doctorappointId == item.DoctorID)
                {
                    DoctorAppointments.Add(item);
                }
            }
            #endregion

            return View(DoctorAppointments);
        }
        [HttpGet]
        public async Task<IActionResult> TodayAppointment()
        {
            #region Doctor can see His Appointments
            int doctorappointId = Convert.ToInt32(TempData["ProfileID"]);
            TempData.Keep();
            IEnumerable<Appointment> Appointments = null;
            using (HttpClient client = new HttpClient())
            {
                string endPoint = _configuration["WebApiBaseUrl"] + "Appointment/GetAppointment";
                using (var response = await client.GetAsync(endPoint))
                {
                    if (response.StatusCode == System.Net.HttpStatusCode.OK)
                    {
                        var result = await response.Content.ReadAsStringAsync();
                        Appointments = JsonConvert.DeserializeObject<IEnumerable<Appointment>>(result);
                    }
                }
            }
            List<Appointment> DoctorAppointments = new List<Appointment>();
            foreach (var item in Appointments)
            {
                if (doctorappointId == item.DoctorID)
                {
                    DoctorAppointments.Add(item);
                }
            }
            #endregion
            #region He acn see today only
            List<Appointment> TodayAppointments = new List<Appointment>();
            foreach (var item in DoctorAppointments)
            {


                if (item.Date == DateTime.Today)
                {
                    TodayAppointments.Add(item);

                }
            }
            #endregion

            return View(TodayAppointments);

        }
        public async Task<IActionResult> ApproveOrReject(int Apointment_Id)
        {
            IEnumerable<Appointment> Appointments = null;
            using (HttpClient client = new HttpClient())
            {
                string endPoint = _configuration["WebApiBaseUrl"] + "Appointment/GetAppointment";
                using (var response = await client.GetAsync(endPoint))
                {
                    if (response.StatusCode == System.Net.HttpStatusCode.OK)
                    {
                        var result = await response.Content.ReadAsStringAsync();
                        Appointments = JsonConvert.DeserializeObject<IEnumerable<Appointment>>(result);
                    }
                }
            }
            List<Appointment> DoctorAppointments = new List<Appointment>();
            foreach (var item in Appointments)
            {
                if (Apointment_Id == item.AppointID)
                {
                    DoctorAppointments.Add(item);
                }
            }
            return View(DoctorAppointments);
            
        }





    }
}
