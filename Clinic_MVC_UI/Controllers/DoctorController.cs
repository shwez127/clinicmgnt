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
using Microsoft.AspNetCore.Mvc.Rendering;

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
            //Fetching the doctor details from database using DoctorId
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
                //in all list of appointments. we just show his(Doctor who is login) appointments and it must be today
                if (doctorappointId == item.DoctorID && item.Date == DateTime.Today)
                {
                    DoctorAppointments.Add(item);
                }
            }
            
            return View(DoctorAppointments);
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
            //updating the appointment status (it may be approve or reject)
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
                        TempData["Notification"] =  1;
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
            
            //we are storing the AppointmentId in tempdata
            TempData["AppointmentIDFor"]= AppointmentId;
           
            return View();
           

        }
        [HttpPost]
        public async Task<IActionResult> AddPrescription(Appointment appointment)
        {
            appointment.AppointID = Convert.ToInt32(TempData["AppointmentIDFor"]);
            #region  Feching the appointment details
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
            //Adding Prescription(Doctor added Prescription) in Database 
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
                        //it will show the notification
                        TempData["Notification"] =  1;
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
                //we will fetch the appointments by doctorId and the bill must be paid,bill amount grater than 0 and doctor must added Prescription or disease about Patient
                if (doctorappointId == item.DoctorID && item.Appointment_Status==1 && item.Bill_Amount==0 && (item.Prescription != null || item.Disease != null))
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
            //updating the bill Amount as per the doctor charges and appointment status completed
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
                        TempData["Notification"] =  1;
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
            //After Appointment Completed Patient Treated  will increases to 1
            doctor.Patient_Treated += 1;
            #region Updating the patient treated count
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
                //Doctor can see his patients . when Appointment status is Completed
                if (doctorappointId == item.DoctorID && item.Appointment_Status==3  )
                {
                    DoctorAppointments.Add(item);
                }
            }
            return View(DoctorAppointments);
            #endregion

        }



        public List<SelectListItem> GetGender()
        {
            List<SelectListItem> gender = new List<SelectListItem>()
            {
                new SelectListItem{Value="Select",Text="Select"},
                new SelectListItem{Value="M",Text="Male"},
                new SelectListItem{Value="F",Text="Female"},
                new SelectListItem{Value="O",Text="Others"},



           };
            return gender;
        }
        public List<SelectListItem> GetDoctorStatus()
        {
            List<SelectListItem> doctorstatus = new List<SelectListItem>()
            {
                new SelectListItem{Value="Select",Text="select"},
                new SelectListItem{Value="1",Text="Present"},
                new SelectListItem{Value="0",Text="Left"},



           };
            return doctorstatus;
        }
        public async Task<IActionResult> EditDoctor(int DoctorId)
        {
            if (DoctorId != 0)
            {
                TempData["EditDoctorId"] = DoctorId;
                TempData.Keep();
            }
            Doctor doctor = null;
            using (HttpClient client = new HttpClient())
            {
                string endPoint = _configuration["WebApiBaseUrl"] + "Doctor/GetDoctorById?doctorId=" + Convert.ToInt32(TempData["EditDoctorId"]);
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
            ViewBag.genderlist = GetGender();
            ViewBag.doctorstatuslist = GetDoctorStatus();



            //Department dropdown list
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
            return View(doctor);
        }

        [HttpPost]
        public async Task<IActionResult> EditDoctor(Doctor doctor)
        {
            ViewBag.status = "";
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
                        return RedirectToAction("Profile", "Doctor");
                    }
                    else
                    {
                        ViewBag.status = "Error";
                        ViewBag.message = "Wrong Entries!";
                    }
                }
            }
            return View();



        }




    }
}
