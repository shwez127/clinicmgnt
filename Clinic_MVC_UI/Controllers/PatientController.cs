using ClinicEntity.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.AspNetCore.Mvc.ViewFeatures;
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
                if (PatientAppointmentId == item.PatientID  && item.Bill_Status != 1 && item.Appointment_Status!=4) 
                {
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
                
                if (PatientAppointmentId == item.PatientID  )
                {
                    #region checking the adate of appointment it is less than today it will automatically reject
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
            if (doctor.Bill_Status != 1 )
            {
                    appointment=doctor;
            }
            }
            TempData["AppointmentidNotify"] = appointment.AppointID;
            return View(PatientAppointments);
            #endregion
        }

        [HttpPost]
        public async Task<IActionResult> SelectingDepartment(Department department)
        {
            TempData["DepartmentId"] = department.DeptNo;
            TempData.Keep();
            return RedirectToAction("addAppointment", "Patient");
            return View();
        }
        public async  Task<IActionResult> addAppointment()
        {
            #region Selecting the department
            Appointment appointment = new Appointment();
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
            


            
            foreach (var item in departments)
            {
                if(item.DeptNo == Convert.ToInt32(TempData["DepartmentId"]))
                {
                    appointment.Progress=item.DeptName;
                }
            }
        
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
            List<Doctor> Doctors = new List<Doctor>();

            foreach (var item in doctors)
            {
                if (item.Deptno == Convert.ToInt32(TempData["DepartmentId"]))
                {
                    Doctors.Add(item);
                }
            }

            /* List<SelectListItem> Doctorlist = new List<SelectListItem>();
             Doctorlist.Add(new SelectListItem { Value = "select", Text = "select" });
             foreach (var item in doctors)
             {
                 if (item.Deptno == Convert.ToInt32(TempData["DepartmentId"]))
                 {
                     Doctorlist.Add(new SelectListItem { Value = item.DoctorID.ToString(), Text = "Name: " + item.Name + "; Qulification:  " + item.Qualification + "; Charges Per Visit: " + item.Charges_Per_Visit });
                 }
             }

             ViewBag.Doctorlist = Doctorlist;*/
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
            /*var tupeluser = new Tuple<Doctor, Appointment>(doctor, appointment);*/
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
            if (appointment.Date < DateTime.Today)
            {
                ViewBag.status = "Error";
                ViewBag.message = "Ensure your appointment date must be today onwards";
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
                if (PatientAppointmentId == item.PatientID && (item.Appointment_Status!=3 && item.Appointment_Status !=4))
                {
                    PatientAppointments.Add(item);
                }
            }
            

            return View(PatientAppointments);
            #endregion
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

            Appointment appointment1 = null;
            using (HttpClient client = new HttpClient())
            {
                string endpoint = _configuration["WebApiBaseUrl"] + "Appointment/GetAppointmentById?AppointmentId=" + AppointmentId;
                using (var response = await client.GetAsync(endpoint))
                {
                    if (response.StatusCode == System.Net.HttpStatusCode.OK)
                    {
                        var result = await response.Content.ReadAsStringAsync();
                        appointment1 = JsonConvert.DeserializeObject<Appointment>(result);
                    }
                }
            }
                
                return View(appointment1);
            #endregion

        }
        [HttpGet]
        public async Task<IActionResult> Notification()
        {
            #region Patient can see Notifications
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
                if (PatientAppointmentId == item.PatientID  )
                {
                    PatientAppointments.Add(item);
                }
            }
          
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
            /* pending_Feedback.PatientID = Convert.ToInt32(TempData["ProfileID"]);

             TempData.Keep();*/
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
            doctor.ReputeIndex = ((doctor.ReputeIndex * (doctor.Patient_Treated - 1)) + pending_Feedback.Rating) / doctor.Patient_Treated;
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
            /*  pending_Feedback.DoctorID = Convert.ToInt32(TempData["DoctorId"]);
              TempData.Keep();
  */
            /*ViewBag.status = "";
            using (HttpClient client = new HttpClient())
            {

                StringContent content = new StringContent(JsonConvert.SerializeObject(pending_Feedback), Encoding.UTF8, "application/json");
                string endPoint = _configuration["WebApiBaseUrl"] + "Feedback/AddFeedback";
                using (var response = await client.PostAsync(endPoint, content))
                {
                    if (response.StatusCode == System.Net.HttpStatusCode.OK)
                    {
                        ViewBag.status = "Ok";
                        ViewBag.message = "Feedback Submited Successfully!";
                    }
                    else
                    {
                        ViewBag.status = "Error";
                        ViewBag.message = "Wrong Entries!";
                    }
                }
            }*/

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
                        TempData["Notification"] =  1;
                        return RedirectToAction("AppointmentProcessing", "Patient");
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
        public async Task<IActionResult> EditPatient(int PatientId)
        {
            Patient patient = null;
            using (HttpClient client = new HttpClient())
            {
                string endPoint = _configuration["WebApiBaseUrl"] + "Patient/GetPatientById?patientId=" + PatientId;
                using (var response = await client.GetAsync(endPoint))
                {
                    if (response.StatusCode == System.Net.HttpStatusCode.OK)
                    {
                        var result = await response.Content.ReadAsStringAsync();
                        patient = JsonConvert.DeserializeObject<Patient>(result);
                    }
                }
            }
            ViewBag.genderlist = GetGender();
            return View(patient);
        }

        [HttpPost]
        public async Task<IActionResult> EditPatient(Patient patient)
        {
            ViewBag.status = "";
            using (HttpClient client = new HttpClient())
            {
                StringContent content = new StringContent(JsonConvert.SerializeObject(patient), Encoding.UTF8, "application/json");
                string endPoint = _configuration["WebApiBaseUrl"] + "Patient/UpdatePatient";
                using (var response = await client.PutAsync(endPoint, content))
                {
                    if (response.StatusCode == System.Net.HttpStatusCode.OK)
                    {
                        ViewBag.status = "Ok";
                        ViewBag.message = "Patient Details Updated Successfully!";
                        return RedirectToAction("Profile", "Patient");
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

        public async Task<IActionResult> Prescription(int AppointmentId)
        {
            #region Patient prescription
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
    }
}
