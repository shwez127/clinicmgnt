using ClinicEntity.Models;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using System.Net.Http;
using System.Threading.Tasks;
using System;
using Microsoft.Extensions.Configuration;
using System.Collections.Generic;
using System.Numerics;
using System.Text;

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
      
        public async Task<IActionResult> ApproveOrReject(int AppointmentId, int Appointment_Status)
        {
            #region Fetching the appointment details
            Appointment appointment = null;
            using (HttpClient client = new HttpClient())
            {
                string endpoint = _configuration["WebApiBaseUrl"] + "Appointment/GetAppointmentById?AppointmentId=" + AppointmentId;
                using (var response = await client.GetAsync(endpoint))
                {
                    if (response.StatusCode == System.Net.HttpStatusCode.OK)
                    {
                        var result = await response.Content.ReadAsStringAsync();
                        appointment = JsonConvert.DeserializeObject<Appointment>(result);
                    }
                }
            }
            #endregion
            #region Updating the Apppointment Status
            appointment.Appointment_Status= Appointment_Status;

            ViewBag.status = "";
            using (HttpClient client = new HttpClient())
            {
                StringContent content = new StringContent(JsonConvert.SerializeObject(appointment), Encoding.UTF8, "application/json");
                string endPoint = _configuration["WebApiBaseUrl"] + "Appointment/UpdateAppointment";
                using (var response = await client.PutAsync(endPoint, content))
                {
                    if (response.StatusCode == System.Net.HttpStatusCode.OK)
                    {
                        ViewBag.status = "Ok";
                        ViewBag.message = "Appointment Details Updated Successfully!";
                    }
                    else
                    {
                        ViewBag.status = "Error";
                        ViewBag.message = "appointment wrong Entries!";
                    }
                }
            }
            #endregion

            return View(appointment);
            
        }

        public async Task<IActionResult> AddPrescription(int AppointmentId)
        {
            #region Fecthing the appointment details
            Appointment appointment = null;
            using (HttpClient client = new HttpClient())
            {
                string endpoint = _configuration["WebApiBaseUrl"] + "Appointment/GetAppointmentById?AppointmentId=" + AppointmentId;
                using (var response = await client.GetAsync(endpoint))
                {
                    if (response.StatusCode == System.Net.HttpStatusCode.OK)
                    {
                        var result = await response.Content.ReadAsStringAsync();
                        appointment = JsonConvert.DeserializeObject<Appointment>(result);
                    }
                }
            }
            #endregion
            return View(appointment);
        }
        [HttpPost]
        public async Task<IActionResult> AddPrescription(Appointment appointment)
        {
            #region Again Feching the appointment details
            Appointment appointmentNew = null;
            using (HttpClient client = new HttpClient())
            {
                string endpoint = _configuration["WebApiBaseUrl"] + "Appointment/GetAppointmentById?AppointmentId=" + appointment.AppointID;
                using (var response = await client.GetAsync(endpoint))
                {
                    if (response.StatusCode == System.Net.HttpStatusCode.OK)
                    {
                        var result = await response.Content.ReadAsStringAsync();
                        appointmentNew = JsonConvert.DeserializeObject<Appointment>(result);
                    }
                }
            }
            #endregion
            #region Updateing the appointment by using prescription, disease and Progress 
            appointmentNew.Appointment_Status = 3;
            appointmentNew.Prescription = appointment.Prescription;
            appointmentNew.Disease = appointment.Disease;
            appointmentNew.Progress = appointment.Progress;
            appointmentNew.Bill_Amount = appointment.Bill_Amount;
            appointmentNew.Bill_Status = 0;

            ViewBag.status = "";
            using (HttpClient client = new HttpClient())
            {
                StringContent content = new StringContent(JsonConvert.SerializeObject(appointmentNew), Encoding.UTF8, "application/json");
                string endPoint = _configuration["WebApiBaseUrl"] + "Appointment/UpdateAppointment";
                using (var response = await client.PutAsync(endPoint, content))
                {
                    if (response.StatusCode == System.Net.HttpStatusCode.OK)
                    {
                        ViewBag.status = "Ok";
                        ViewBag.message = "Appointment Preception Updated Successfully!";
                    }
                    else
                    {
                        ViewBag.status = "Error";
                        ViewBag.message = "appointment wrong Entries!";
                    }
                }
            }
            #endregion

            return View();
        }
        [HttpGet]
        public async Task<IActionResult> GetAllCompleted()
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
                if (doctorappointId == item.DoctorID && item.Appointment_Status==3 && item.Bill_Amount==0)
                {
                    DoctorAppointments.Add(item);
                }
            }
            #endregion
            return View(DoctorAppointments);
        }
       
       
        public async Task<IActionResult> GenerateBill(int AppointmentId)
        {
            #region Again Feching the appointment details
            Appointment appointmentNew = null;
            using (HttpClient client = new HttpClient())
            {
                string endpoint = _configuration["WebApiBaseUrl"] + "Appointment/GetAppointmentById?AppointmentId=" + AppointmentId;
                using (var response = await client.GetAsync(endpoint))
                {
                    if (response.StatusCode == System.Net.HttpStatusCode.OK)
                    {
                        var result = await response.Content.ReadAsStringAsync();
                        appointmentNew = JsonConvert.DeserializeObject<Appointment>(result);
                    }
                }
            }
            #endregion
            #region Update the Bill
            appointmentNew.Bill_Amount=appointmentNew.Doctor.Charges_Per_Visit;

            ViewBag.status = "";
            using (HttpClient client = new HttpClient())
            {
                StringContent content = new StringContent(JsonConvert.SerializeObject(appointmentNew), Encoding.UTF8, "application/json");
                string endPoint = _configuration["WebApiBaseUrl"] + "Appointment/UpdateAppointment";
                using (var response = await client.PutAsync(endPoint, content))
                {
                    if (response.StatusCode == System.Net.HttpStatusCode.OK)
                    {
                        ViewBag.status = "Ok";
                        ViewBag.message = "Appointment Bill Generated!";
                    }
                    else
                    {
                        ViewBag.status = "Error";
                        ViewBag.message = "appointment wrong Entries!";
                    }
                }
            }
            #endregion
            return View(appointmentNew);
        }

        [HttpGet]
        public async Task<IActionResult> PatientHistory()
        {
            #region View the patient History
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
                if (doctorappointId == item.DoctorID && item.Appointment_Status==3 && item.Bill_Amount>0 )
                {
                    DoctorAppointments.Add(item);
                }
            }
            #endregion
            return View(DoctorAppointments);
        }








    }
}
