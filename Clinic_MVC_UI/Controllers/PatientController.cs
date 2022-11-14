using ClinicEntity.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.Extensions.Configuration;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Numerics;
using System.Text;
using System.Threading.Tasks;

namespace Clinic_MVC_UI.Controllers
{
    public class PatientController : Controller
    {
        private IConfiguration _configuration;
        public PatientController(IConfiguration configuration)
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

            Patient patient = null;
            using (HttpClient client = new HttpClient())
            {
                string endpoint = _configuration["WebApiBaseUrl"] + "Patient/GetPatientById?patientId=" + doctorProfileId;
                using (var response = await client.GetAsync(endpoint))
                {
                    if (response.StatusCode == System.Net.HttpStatusCode.OK)
                    {
                        var result = await response.Content.ReadAsStringAsync();
                        patient = JsonConvert.DeserializeObject<Patient>(result);
                    }
                }
            }
            return View(patient);
        }

        public async  Task<IActionResult> addAppointment()

        {
            #region Selecting the department
            List<Department> departments = new List<Department>();
            using (HttpClient client = new HttpClient())
            {
                string endpoint = _configuration["WebApiBaseUrl"] + "Department/GetDepartments";
                using (var response = await client.GetAsync(endpoint))
                {
                    if (response.StatusCode == System.Net.HttpStatusCode.OK)
                    {
                        var result = await response.Content.ReadAsStringAsync();
                        departments = JsonConvert.DeserializeObject<List<Department>>(result);
                    }
                }
            }
            List<SelectListItem> department = new List<SelectListItem>();


            department.Add(new SelectListItem { Value = "select", Text = "select" });
            foreach (var item in departments)
            {
                department.Add(new SelectListItem { Value = item.DeptNo.ToString(), Text = item.DeptName });
            }

            ViewBag.Departmentlist = department;
            #endregion

            #region selecting the doctor
            List<Doctor> doctors = new List<Doctor>();
            using (HttpClient client = new HttpClient())
            {
                string endpoint = _configuration["WebApiBaseUrl"] + "Doctor/GetDoctors";
                using (var response = await client.GetAsync(endpoint))
                {
                    if (response.StatusCode == System.Net.HttpStatusCode.OK)
                    {
                        var result = await response.Content.ReadAsStringAsync();
                        doctors = JsonConvert.DeserializeObject<List<Doctor>>(result);
                    }
                }
            }
            List<SelectListItem> Doctorlist = new List<SelectListItem>();


            Doctorlist.Add(new SelectListItem { Value = "select", Text = "select" });
            foreach (var item in doctors)
            {
                Doctorlist.Add(new SelectListItem { Value = item.DoctorID.ToString(), Text = "Name: "+item.Name+"; Qulification:  "+item.Qualification+"; Charges Per Visit: "+item.Charges_Per_Visit });
            }

            ViewBag.Doctorlist = Doctorlist;
            #endregion

            return View();
        }

        [HttpPost]
        public async Task<IActionResult> addAppointment(Appointment appointment)
        {
            appointment.PatientID = Convert.ToInt32(TempData["ProfileID"]);
            TempData.Keep();
             
            
       
            ViewBag.Status = "";
            using (HttpClient client = new HttpClient())
            {
                StringContent content = new StringContent(JsonConvert.SerializeObject(appointment), Encoding.UTF8, "application/json");
                string endPoint = _configuration["WebApiBaseUrl"] + "Appointment/AddAppointment";
                using (var response = await client.PostAsync(endPoint, content))
                {
                    if (response.StatusCode == System.Net.HttpStatusCode.OK)
                    {
                        ViewBag.status = "Ok";
                        ViewBag.message = "Patient Registered Successfully";
                        
                    }
                    else
                    {
                        ViewBag.status = "Error";
                        ViewBag.message = "Wrong Entries";
                    }
                }
            }
            return View();
        }
        [HttpGet]
        public async Task<IActionResult> PendingAppointment()
        {
            #region Patient can see His Appointments
            int PatientAppointmentId = Convert.ToInt32(TempData["ProfileID"]);
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
            List<Appointment> PatientAppointments = new List<Appointment>();
            foreach (var item in Appointments)
            {
                if (PatientAppointmentId == item.PatientID)
                {
                    PatientAppointments.Add(item);
                }
            }
            #endregion

            return View(PatientAppointments);
        }
        [HttpGet]
        public async Task<IActionResult> BillHistory()
        {
            #region Patient can see His Appointments
            int PatientAppointmentId = Convert.ToInt32(TempData["ProfileID"]);
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
            List<Appointment> PatientAppointments = new List<Appointment>();
            foreach (var item in Appointments)
            {
                if (PatientAppointmentId == item.PatientID && item.Appointment_Status==3 && item.Bill_Status==0)
                {
                    PatientAppointments.Add(item);
                }
            }
            #endregion
            return View(PatientAppointments);
        }
    
        public async Task<IActionResult> UpdateBill(int AppointmentId)
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
            #region Updating the Bill Status
            appointment.Bill_Status = 1;

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
            return View();
        }
        [HttpGet]
        public async Task<IActionResult> TreatmentHistory()
        {
            #region Patient can see His Appointments
            int PatientAppointmentId = Convert.ToInt32(TempData["ProfileID"]);
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
            List<Appointment> PatientAppointments = new List<Appointment>();
            foreach (var item in Appointments)
            {
                if (PatientAppointmentId == item.PatientID && item.Appointment_Status == 3 && item.Bill_Status == 1)
                {
                    PatientAppointments.Add(item);
                }
            }
            #endregion
            return View(Appointments);
        }
    }
}
