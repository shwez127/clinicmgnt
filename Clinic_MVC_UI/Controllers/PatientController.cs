using ClinicEntity.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.AspNetCore.Mvc.ViewFeatures;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using Microsoft.Extensions.Configuration;
using Microsoft.VisualBasic;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Linq;
using System.Net.Http;
using System.Numerics;
using System.Text;
using System.Threading.Tasks;
using static Microsoft.AspNetCore.Razor.Language.TagHelperMetadata;

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
            #region Patient profile
            //storing the profile Id
            int PatientProfileId = Convert.ToInt32(TempData["ProfileID"]);
            TempData.Keep();

            Patient patient = null;
            using (HttpClient client = new HttpClient())
            {
                string endpoint = _configuration["WebApiBaseUrl"] + "Patient/GetPatientById?patientId=" + PatientProfileId;
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
            #endregion
        }

        public async Task<IActionResult> SelectingDepartment()
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
                //if appointment is in progress it will block to create a new appointment
                if (PatientAppointmentId == item.PatientID  && item.Bill_Status != 1 && item.Appointment_Status!=4) 
                {
                    //it will goes to AppointmentProcessing Page
                    return RedirectToAction("AppointmentProcessing", "Patient");
                }
            }
            #endregion
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

            //fetching the departments and adding to the Viewbag for selecting appointment
            department.Add(new SelectListItem { Value = "Select Department", Text = "Select Department" });
            foreach (var item in departments)
            {
                department.Add(new SelectListItem { Value = item.DeptNo.ToString(), Text = item.DeptName });
            }

            ViewBag.Departmentlist = department;
           
            return View();
            #endregion
        }
        public async Task<IActionResult> AppointmentProcessing()
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
                //it will check the Login patient appointments by using PatientId
                if (PatientAppointmentId == item.PatientID  )
                {

                    #region checking the date of appointment it is less than today it will automatically reject
                    //booking appointment is less than today but appointment in requested mode. it will automatically reject
                    if (item.Date < DateTime.Today && (item.Appointment_Status==0 || item.Appointment_Status==1))
                    {

                        item.Appointment_Status = 4;
                        #region Upadting the appointment
                        ViewBag.status = "";
                        using (HttpClient client = new HttpClient())
                        {
                            StringContent content = new StringContent(JsonConvert.SerializeObject(item), Encoding.UTF8, "application/json");
                            string endPoint = _configuration["WebApiBaseUrl"] + "Appointment/UpdateAppointment";
                            using (var response = await client.PutAsync(endPoint, content))
                            {
                                if (response.StatusCode == System.Net.HttpStatusCode.OK)
                                {
                                    ViewBag.status = "Ok";
                                    ViewBag.message = "Appointment Details Updated Successfully!";
                                    TempData["Notification"] =  1;
                                }
                                else
                                {
                                    ViewBag.status = "Error";
                                    ViewBag.message = "appointment wrong Entries!";
                                }
                            }
                        }
                        #endregion
                    }
                    else { 
                    #endregion
                    PatientAppointments.Add(item);
                    }
                }
            }
            Appointment appointment = null;
            foreach(var doctor in PatientAppointments) { 
            if (doctor.Bill_Status != 1 && doctor.Appointment_Status != 4)
            {
                    appointment=doctor;
            }
            }
            //for purpose of notification it is storing the appointment Id
            TempData["AppointmentidNotify"] = appointment.AppointID;
            return View(PatientAppointments);
            #endregion
        }

        [HttpPost]
        public async Task<IActionResult> SelectingDepartment(Department department)
        {
            // We are storing the DeptNo and By using that DeptNo we will search the doctors
            TempData["DepartmentId"] = department.DeptNo;
            TempData.Keep();
            return RedirectToAction("addAppointment", "Patient");
            return View();
        }
        public async  Task<IActionResult> addAppointment()
        {
  
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
            List<Doctor> Doctors = new List<Doctor>();

            foreach (var item in doctors)
            {
                //it will search the doctor based on patient selected department
                if (item.Deptno == Convert.ToInt32(TempData["DepartmentId"]))
                {
                    Doctors.Add(item);
                }
            }

           
            return View(Doctors);
            #endregion
        }
        public async Task<IActionResult> After_Selecting_doctor (int SelectingDoctorId)
        {
            #region it shows the full details of doctor
            Appointment appointment = new Appointment();
            Doctor doctor = new Doctor();
            using (HttpClient client = new HttpClient())
            {
                string endPoint = _configuration["WebApiBaseUrl"] + "Doctor/GetDoctorById?doctorId=" + SelectingDoctorId;
                using (var response = await client.GetAsync(endPoint))
                {
                    if (response.StatusCode == System.Net.HttpStatusCode.OK)
                    {
                        var result = await response.Content.ReadAsStringAsync();
                        doctor = JsonConvert.DeserializeObject<Doctor>(result);
                    }
                }
            }
            doctor.BirthDate = DateTime.Today;
          
            return View(doctor);
            #endregion region

        }
        [HttpPost]
        public async Task<IActionResult> After_Selecting_doctor(Doctor doctor, int DoctorIdd)
        {
            #region We are Avoiding the null referance error
            Doctor doctors = new Doctor();
            using (HttpClient client = new HttpClient())
            {
                string endPoint = _configuration["WebApiBaseUrl"] + "Doctor/GetDoctorById?doctorId=" + DoctorIdd;
                using (var response = await client.GetAsync(endPoint))
                {
                    if (response.StatusCode == System.Net.HttpStatusCode.OK)
                    {
                        var result = await response.Content.ReadAsStringAsync();
                        doctors = JsonConvert.DeserializeObject<Doctor>(result);
                    }
                }
            }
            #endregion


            Appointment appointment = new Appointment();
            #region Adding of Appointment
            appointment.DoctorID=DoctorIdd;
            appointment.Date = doctor.BirthDate;
            appointment.PatientID = Convert.ToInt32(TempData["ProfileID"]);
            TempData.Keep();
            ViewBag.Status = "";
            //the appointment date must be today onwords
            if (appointment.Date < DateTime.Today)
            {
                ViewBag.status = "Error";
                ViewBag.message = "Ensure your Appointment date must be Today Onwards";
            }
            else
            {
                using (HttpClient client = new HttpClient())
                {
                    StringContent content = new StringContent(JsonConvert.SerializeObject(appointment), Encoding.UTF8, "application/json");
                    string endPoint = _configuration["WebApiBaseUrl"] + "Appointment/AddAppointment";
                    using (var response = await client.PostAsync(endPoint, content))
                    {
                        if (response.StatusCode == System.Net.HttpStatusCode.OK)
                        {
                            ViewBag.status = "Ok";
                            ViewBag.message = "Appointment booked Successfully";
                            TempData["Notification"] =  1;
                            TempData.Keep();
                            return RedirectToAction("SelectingDepartment", "Patient");
                            

                        }
                        else
                        {
                            ViewBag.status = "Error";
                            ViewBag.message = "Wrong Entries";
                        }
                    }

                }
            }
            
            #endregion
            return View(doctors);
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
                //when Appointment is completed and he need to pay bill
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
            //after Bill paid the bill status need to change
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
                        ViewBag.message = "Bill Paid Successfully!";
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
 return View();
            #endregion

        }
        [HttpGet]
        public async Task<IActionResult> Notification()
        {
            #region Patient can see Notifications
            //after clicking on the notification the notification flag status is 0
            TempData["Notification"] = 0;
            TempData.Keep();
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
                //only login patient can see his appointments only
                if (PatientAppointmentId == item.PatientID  )
                {
                    PatientAppointments.Add(item);
                }
            }
          //checking his appointments count
            TempData["oldone"] = PatientAppointments.Count();
            return View(PatientAppointments);
            #endregion
        }
        [HttpGet]
        public async Task<IActionResult> TreatmentHistory()
        {
            #region Patient can see his Treatment history
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
                //In Treatment History Patient Can view His History when he paid
                if (PatientAppointmentId == item.PatientID && item.Bill_Status == 1)
                {
                    PatientAppointments.Add(item);
                }
            }
            return View(PatientAppointments);
            #endregion

        }
        public async Task<IActionResult> PatientFeedback()
        {

            #region Patient can give Feedback
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
                //it will show the all feedbacks of patient
                if (PatientAppointmentId == item.PatientID && item.Bill_Status == 1)
                {
                    PatientAppointments.Add(item);
                }
            }
            return View(PatientAppointments);
            #endregion
        }

        public IActionResult Create(int AppointmentId)
            
        {
            #region Feedback rating select list

            TempData["AppointmentId"] = AppointmentId;
            //it is feedback rating list

            List<SelectListItem> Rating = new List<SelectListItem>()
            {
                new SelectListItem{Value="Select",Text="Select"},
                new SelectListItem{Value="5",Text="Very good"},
                new SelectListItem{Value="4",Text="Good"},
                new SelectListItem{Value="3",Text="Mediocre"},
                new SelectListItem{Value="2",Text="Bad"},
                 new SelectListItem{Value="1",Text="Very Bad"}
            };
            ViewBag.Ratinglist = Rating;
            return View();
            #endregion
        }
        [HttpPost]
        public async Task<ActionResult> Create(Pending_Feedback pending_Feedback)

        {
            #region Feedback
            
            pending_Feedback.AppointID = Convert.ToInt32(TempData["AppointmentId"]);
            TempData.Keep();

            ViewBag.status = "";
            
            using (HttpClient client = new HttpClient())
            {

                StringContent content = new StringContent(JsonConvert.SerializeObject(pending_Feedback), Encoding.UTF8, "application/json");
                string endPoint = _configuration["WebApiBaseUrl"] + "Feedback/AddFeedback";
                using (var response = await client.PostAsync(endPoint, content))
                {
                    if (response.StatusCode == System.Net.HttpStatusCode.OK)
                    {
                        ViewBag.status = "Ok";
                        ViewBag.message = "Feedback Saved Successfully!";
                    }
                    else
                    {
                        ViewBag.status = "Error";
                        ViewBag.message = "Your Feedback is already submitted";
                    }
                }

            }

            #region Fetching the appointment details
            Appointment appointment = null;
            using (HttpClient client = new HttpClient())
            {
                string endpoint = _configuration["WebApiBaseUrl"] + "Appointment/GetAppointmentById?AppointmentId=" + pending_Feedback.AppointID;
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
            //Updating the ratings of doctor
            Doctor doctor = new Doctor();
            using (HttpClient client = new HttpClient())
            {
                string endPoint = _configuration["WebApiBaseUrl"] + "Doctor/GetDoctorById?doctorId=" + appointment.DoctorID;
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
            //it will calculate the repute index of doctor
            doctor.ReputeIndex = ((doctor.ReputeIndex * (doctor.Patient_Treated - 1)) + pending_Feedback.Rating) / doctor.Patient_Treated;
            using (HttpClient client = new HttpClient())
            {
                StringContent content = new StringContent(JsonConvert.SerializeObject(doctor), Encoding.UTF8, "application/json");
                string endPoint = _configuration["WebApiBaseUrl"] + "Doctor/UpdateDoctor";
                using (var response = await client.PutAsync(endPoint, content))
                {
                    if (response.StatusCode == System.Net.HttpStatusCode.OK)
                    {
                        int l= 1;

                    }
                    else
                    {
                        ViewBag.status = "Error";
                        ViewBag.message = "Wrong Entries!";
                    }
                }
            }
          return View();
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
            //it will reject the appointment 
            appointment.Appointment_Status = Appointment_Status;

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
                        //it will show the notification after feedback
                        TempData["Notification"] =  1;
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
      
    }
}
