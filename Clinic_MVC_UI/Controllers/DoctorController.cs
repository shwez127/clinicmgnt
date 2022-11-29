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
            #region Doctor profile
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
            #endregion
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
                if (doctorappointId == item.DoctorID && item.Appointment_Status==0)
                {
                    DoctorAppointments.Add(item);
                }
            }
            return View(DoctorAppointments);
            #endregion
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
            #region He can see today only
            List<Appointment> TodayAppointments = new List<Appointment>();
            foreach (var item in DoctorAppointments)
            {


                if (item.Date == DateTime.Today)
                {
                    TodayAppointments.Add(item);

                }
            }
            return View(TodayAppointments);
            #endregion

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
                        TempData["Notification"] = Convert.ToInt32(TempData["Notification"]) + 1;
                        TempData.Keep();
                    }
                    else
                    {
                        ViewBag.status = "Error";
                        ViewBag.message = "appointment wrong Entries!";
                    }
                }
            }
            return View(appointment);
            #endregion
        }

        public async Task<IActionResult> AddPrescription(int AppointmentId)
        {
            #region Fecthing the appointment details
            TempData["AppointmentIDFor"]= AppointmentId;
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
            return View(appointment);
            #endregion

        }
        [HttpPost]
        public async Task<IActionResult> AddPrescription(Appointment appointment)
        {
            appointment.AppointID = Convert.ToInt32(TempData["AppointmentIDFor"]);
            #region Again Feching the appointment details
            Appointment appointmentNew = new Appointment();
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
            #region Updating the appointment by using prescription, disease and Progress 

            appointmentNew.Appointment_Status = 1;
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
                        ViewBag.message = "Prescription Updated Successfully!";
                        TempData["Notification"] = Convert.ToInt32(TempData["Notification"]) + 1;
                        TempData.Keep();
                    }
                    else
                    {
                        ViewBag.status = "Error";
                        ViewBag.message = "Prescription wrong Entries!";
                    }
                }
            }
            return View();
            #endregion
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
                if (doctorappointId == item.DoctorID && item.Appointment_Status==1 && item.Bill_Amount==0)
                {
                    DoctorAppointments.Add(item);
                }
            }
            return View(DoctorAppointments);
            #endregion
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
            appointmentNew.Appointment_Status = 3;

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
                        TempData["Notification"] = Convert.ToInt32(TempData["Notification"]) + 1;
                        TempData.Keep();
                    }
                    else
                    {
                        ViewBag.status = "Error";
                        ViewBag.message = "appointment wrong Entries!";
                    }
                }
            }
            Doctor doctor = new Doctor();
            using (HttpClient client = new HttpClient())
            {
                string endPoint = _configuration["WebApiBaseUrl"] + "Doctor/GetDoctorById?doctorId=" + appointmentNew.DoctorID;
                TempData.Keep();
                using (var response = await client.GetAsync(endPoint))
                {
                    if (response.StatusCode == System.Net.HttpStatusCode.OK)
                    {
                        var result = await response.Content.ReadAsStringAsync();
                        doctor = JsonConvert.DeserializeObject<Doctor>(result);
                    }
                }
            }
            doctor.Patient_Treated += 1;
            using (HttpClient client = new HttpClient())
            {
                StringContent content = new StringContent(JsonConvert.SerializeObject(doctor), Encoding.UTF8, "application/json");
                string endPoint = _configuration["WebApiBaseUrl"] + "Doctor/UpdateDoctor";
                using (var response = await client.PutAsync(endPoint, content))
                {
                    if (response.StatusCode == System.Net.HttpStatusCode.OK)
                    {
                        ViewBag.status = "Ok";
                        ViewBag.message = "Doctor Details Updated Successfully!";
                       
                    }
                    else
                    {
                        ViewBag.status = "Error";
                        ViewBag.message = "Wrong Entries!";
                    }
                }
            }
            
            return View(appointmentNew);

            #endregion
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
            return View(DoctorAppointments);
            #endregion

        }








    }
}
