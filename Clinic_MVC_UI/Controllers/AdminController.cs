using ClinicData.Data;
using ClinicEntity.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.CodeAnalysis.Options;
using Microsoft.Extensions.Configuration;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;


namespace Clinic_MVC_UI.Controllers
{
    public class AdminController : Controller
    {
        //Private member
        private IConfiguration _configuration;

        //Constructor
        public AdminController(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        ClinicDbContext db =new ClinicDbContext();
        public IActionResult Index()
        {
 
            return View();
        }
        public List<SelectListItem> GetGender()
        {
            #region Getting gender list
            //it will show the Gender Dropdown
            List<SelectListItem> gender = new List<SelectListItem>()
            {
                new SelectListItem{Value="Select",Text="Select"},
                new SelectListItem{Value="M",Text="Male"},
                new SelectListItem{Value="F",Text="Female"},
                new SelectListItem{Value="O",Text="Others"},

            };
            return gender;
            #endregion
        }

        #region Doctor View-Edit-Delete Actions
        public List<SelectListItem> GetDoctorStatus()
        {
            //it will show the Doctor Status
            List<SelectListItem> doctorstatus = new List<SelectListItem>()
            {
                new SelectListItem{Value="Select",Text="select"},
                new SelectListItem{Value="1",Text="Present"},
                new SelectListItem{Value="0",Text="Left"},

            };
            return doctorstatus;
        }
        public ActionResult GetAllDoctors(string searchName)
        {
            var projects = from pr in db.doctors select pr;

            if (!String.IsNullOrEmpty(searchName))
            {
                //this function will search details from the database what we want to search
                projects = projects.Where(c => c.Name.Contains(searchName));
            }

            return View(projects);
        }

        #region It will Featch the all doctors details
        [HttpGet]
        public async Task<IActionResult> GetAllDoctors()
        {
            IEnumerable<Doctor> doctorresult = null;
            using (HttpClient client = new HttpClient())
            {
               // LocalHost Adress in endpoint
                string endPoint = _configuration["WebApiBaseUrl"] + "Doctor/GetDoctors";
                using (var response = await client.GetAsync(endPoint))
                {
                    if (response.StatusCode == System.Net.HttpStatusCode.OK)
                    {
                        var result = await response.Content.ReadAsStringAsync();
                        //It will deserilize the object in the form of JSON
                        doctorresult = JsonConvert.DeserializeObject<IEnumerable<Doctor>>(result);
                    }
                }
            }
            return View(doctorresult);
        }
        #endregion

        public async Task<IActionResult> EditDoctor(int DoctorId)
        {
            if(DoctorId != 0) 
            {
                //We are Storing Doctor Id  temporary to avoid the error. Now it will show the doctor details after the update also
                TempData["EditDoctorId"]=DoctorId;
                TempData.Keep();
            }
            Doctor doctor = null;
            //it will fetch the Doctor Details by using DoctorID
            using (HttpClient client = new HttpClient())
            {
                string endPoint = _configuration["WebApiBaseUrl"] + "Doctor/GetDoctorById?doctorId=" + Convert.ToInt32(TempData["EditDoctorId"]);
                TempData.Keep();
                using (var response = await client.GetAsync(endPoint))
                {
                    if (response.StatusCode == System.Net.HttpStatusCode.OK)
                    {
                        var result = await response.Content.ReadAsStringAsync();
                        //It will deserilize the object in the form of JSON
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
                        //It will deserilize the object in the form of JSON
                        departments = JsonConvert.DeserializeObject<List<Department>>(result);
                    }
                }
            }

            //it will store the list of department in the form of Dropdown
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
            //it will update the doctor details after Admin Changes
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
                        return RedirectToAction("GetAllDoctors", "Admin");
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
       
        public async Task<IActionResult> DeleteDoctor(int DoctorId)
        {
            ViewBag.status = "";
            //it will Delete the doctor Details by using doctor Id
            using (HttpClient client = new HttpClient())
            {
                string endPoint = _configuration["WebApiBaseUrl"] + "Doctor/DeleteDoctor?doctorId=" + DoctorId;
                using (var response = await client.DeleteAsync(endPoint))
                {
                    if (response.StatusCode == System.Net.HttpStatusCode.OK)
                    {
                        ViewBag.status = "Ok";
                        ViewBag.message = "Doctor Details Deleted Successfully!";
                        return RedirectToAction("GetAllDoctors", "Admin");
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
        public async Task<IActionResult> DoctorProfile(int doctorProfileId)
        {
            Doctor doctor = null;
            //It will fetch the doctor details By using the Doctor Id 
            using (HttpClient client = new HttpClient())
            {
                string endpoint = _configuration["WebApiBaseUrl"] + "Doctor/GetDoctorById?doctorId=" + doctorProfileId;
                using (var response = await client.GetAsync(endpoint))
                {
                    if (response.StatusCode == System.Net.HttpStatusCode.OK)
                    {
                        var result = await response.Content.ReadAsStringAsync();
                        //It will deserilize the object in the form of JSON
                        doctor = JsonConvert.DeserializeObject<Doctor>(result);
                    }
                }
            }
            return View(doctor);
        }
        #endregion

        #region Patient View-Edit-Delete Actions

        public ActionResult GetAllPatients(string option, string search)
        {
            if (option == "Subjects")
            {
                //GetAllPatients action method will return a view with a patient records based on what a user specify the value in textbox  
                return View(db.patients.Where(x => x.Address == search || search == null).ToList());
            }
            else if (option == "Gender")
            {
                return View(db.patients.Where(x => x.Gender == search || search == null).ToList());
            }
            else
            {
                return View(db.patients.Where(x => x.Name.StartsWith(search) || search == null).ToList());
            }
        }

        [HttpGet]
        public async Task<IActionResult> GetAllPatients()
        {
           
            IEnumerable<Patient> patientresult = null;
            //it will fetch all Patient Details from database
            using (HttpClient client = new HttpClient())
            {
                string endPoint = _configuration["WebApiBaseUrl"] + "Patient/GetAllPatients";
                using (var response = await client.GetAsync(endPoint))
                {
                    if (response.StatusCode == System.Net.HttpStatusCode.OK)
                    {
                        var result = await response.Content.ReadAsStringAsync();
                        patientresult = JsonConvert.DeserializeObject<IEnumerable<Patient>>(result);
                    }
                }
            }
            
            return View(patientresult);
        }
        public async Task<IActionResult> EditPatient(int PatientId)
        {
            Patient patient = null;
            //It will fetch One Patient Details by using Patient Id
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
            //it will update Patient Details in Database.
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
                        return RedirectToAction("GetAllPatients", "Admin");
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
        
        public async Task<IActionResult> DeletePatient(int PatientId)
        {
            ViewBag.status = "";
            //it will delete the patient details in database by using the patient Id
            using (HttpClient client = new HttpClient())
            {
                string endPoint = _configuration["WebApiBaseUrl"] + "Patient/DeletePatient?patientId=" + PatientId;
                using (var response = await client.DeleteAsync(endPoint))
                {
                    if (response.StatusCode == System.Net.HttpStatusCode.OK)
                    {
                        ViewBag.status = "Ok";
                        ViewBag.message = "Patient Details Deleted Successfully!";
                        return RedirectToAction("GetAllPatients", "Admin");
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
        public async Task<IActionResult> PatientProfile(int patientProfileId)
        {
            Patient patient = null;
            //it will fetch the patient details by Using PatientId. It will show the Patient Profile
            using (HttpClient client = new HttpClient())
            {
                string endpoint = _configuration["WebApiBaseUrl"] + "Patient/GetPatientById?patientId=" + patientProfileId;
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
        #endregion

        #region OtherStaffs Create-View-Edit-Delete Actions

        public ActionResult GetAllOtherStaffs(string searchName)
        {
            var projects = from pr in db.otherStaffs select pr;
            //this function is for searching purpose 

            if (!String.IsNullOrEmpty(searchName))
            {
                projects = projects.Where(c => c.Name.Contains(searchName));
            }

            return View(projects);
        }

        [HttpGet]
        public async Task<IActionResult> GetAllOtherStaffs()
        {
            IEnumerable<OtherStaff> otherstaffresult = null;
            //This function will fetch the all Staffs Details
            using (HttpClient client = new HttpClient())
            {
                string endPoint = _configuration["WebApiBaseUrl"] + "Staff/GetAllStaffs";
                using (var response = await client.GetAsync(endPoint))
                {
                    if (response.StatusCode == System.Net.HttpStatusCode.OK)
                    {
                        var result = await response.Content.ReadAsStringAsync();
                        otherstaffresult = JsonConvert.DeserializeObject<IEnumerable<OtherStaff>>(result);
                    }
                }
            }
            return View(otherstaffresult);
        }
        public IActionResult OtherStaffEntry()
        {
            ViewBag.genderlist = GetGender();
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> OtherStaffEntry(OtherStaff otherStaff)
        {
            if (ModelState.IsValid)
            {
                ViewBag.status = "";
                //this function will add the OtherStaffs to Database
                using (HttpClient client = new HttpClient())
                {

                    StringContent content = new StringContent(JsonConvert.SerializeObject(otherStaff), Encoding.UTF8, "application/json");
                    string endPoint = _configuration["WebApiBaseUrl"] + "Staff/AddStaff";
                    using (var response = await client.PostAsync(endPoint, content))
                    {
                        if (response.StatusCode == System.Net.HttpStatusCode.OK)
                        {
                            ViewBag.status = "Ok";
                            ViewBag.message = "Staff Details Saved Successfully!";
                        }
                        else
                        {
                            ViewBag.status = "Error";
                            ViewBag.message = "Wrong Entries!";
                        }
                    }
                }
            }
            return View();
        }

        public async Task<IActionResult> EditOtherStaff(int OtherStaffId)
        {
            OtherStaff otherStaff = null;
            //this function will fetch the deatils of other staff by using other staff Id 
            using (HttpClient client = new HttpClient())
            {
                string endPoint = _configuration["WebApiBaseUrl"] + "Staff/GetStaffById?staffId=" + OtherStaffId;
                using (var response = await client.GetAsync(endPoint))
                {
                    if (response.StatusCode == System.Net.HttpStatusCode.OK)
                    {
                        var result = await response.Content.ReadAsStringAsync();
                        otherStaff = JsonConvert.DeserializeObject<OtherStaff>(result);
                    }
                }
            }
            ViewBag.genderlist = GetGender();
            return View(otherStaff);
        }

        [HttpPost]
        public async Task<IActionResult> EditOtherStaff(OtherStaff otherStaff)
        {
            ViewBag.status = "";
            //it will update the database of other staff based on Admin Entry
            using (HttpClient client = new HttpClient())
            {
                StringContent content = new StringContent(JsonConvert.SerializeObject(otherStaff), Encoding.UTF8, "application/json");
                string endPoint = _configuration["WebApiBaseUrl"] + "Staff/UpdateStaff";
                using (var response = await client.PutAsync(endPoint, content))
                {
                    if (response.StatusCode == System.Net.HttpStatusCode.OK)
                    {
                        ViewBag.status = "Ok";
                        ViewBag.message = "Staff Details Updated Successfully!";
                        return RedirectToAction("GetAllOtherStaffs", "Admin");
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
        
        public async Task<IActionResult> DeleteOtherStaff(int OtherStaffId)
        {
            ViewBag.status = "";
            //it will delete  otherstaff details based on the otherstaffId
            using (HttpClient client = new HttpClient())
            {
                string endPoint = _configuration["WebApiBaseUrl"] + "Staff/DeleteStaff?staffId=" + OtherStaffId;
                using (var response = await client.DeleteAsync(endPoint))
                {
                    if (response.StatusCode == System.Net.HttpStatusCode.OK)
                    {
                        ViewBag.status = "Ok";
                        ViewBag.message = "Staff Details Deleted Successfully!";
                        return RedirectToAction("GetAllOtherStaffs", "Admin");
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
        #endregion

        #region Departments Create-View-Edit-Delete Actions
        public async Task<IActionResult> GetAllDepartments()
        {
            IEnumerable<Department> departmentresult = null;

            using (HttpClient client = new HttpClient())
            {
                string endPoint = _configuration["WebApiBaseUrl"] + "Department/GetDepartments";
                using (var response = await client.GetAsync(endPoint))
                {
                    if (response.StatusCode == System.Net.HttpStatusCode.OK)
                    {
                        var result = await response.Content.ReadAsStringAsync();
                        departmentresult = JsonConvert.DeserializeObject<IEnumerable<Department>>(result);
                    }
                }
            }
            return View(departmentresult);
        }
        public IActionResult DepartmentEntry()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> DepartmentEntry(Department department)
        {
            if (ModelState.IsValid)
            {
                ViewBag.status = "";
                using (HttpClient client = new HttpClient())
                {

                    StringContent content = new StringContent(JsonConvert.SerializeObject(department), Encoding.UTF8, "application/json");
                    string endPoint = _configuration["WebApiBaseUrl"] + "Department/AddDepartment";
                    using (var response = await client.PostAsync(endPoint, content))
                    {
                        if (response.StatusCode == System.Net.HttpStatusCode.OK)
                        {
                            ViewBag.status = "Ok";
                            ViewBag.message = "Department Details Saved Successfully!";
                        }
                        else
                        {
                            ViewBag.status = "Error";
                            ViewBag.message = "Wrong Entries!";
                        }
                    }
                }
            }
            return View();
        }

        public async Task<IActionResult> EditDepartment(int DepartmentId)
        {
            Department department = null;
            using (HttpClient client = new HttpClient())
            {
                string endPoint = _configuration["WebApiBaseUrl"] + "Department/GetDepartmentsById?departmentId=" + DepartmentId;
                using (var response = await client.GetAsync(endPoint))
                {
                    if (response.StatusCode == System.Net.HttpStatusCode.OK)
                    {
                        var result = await response.Content.ReadAsStringAsync();
                        department = JsonConvert.DeserializeObject<Department>(result);
                    }
                }
            }
            return View(department);
        }

        [HttpPost]
        public async Task<IActionResult> EditDepartment(Department department)
        {
            ViewBag.status = "";
            using (HttpClient client = new HttpClient())
            {
                StringContent content = new StringContent(JsonConvert.SerializeObject(department), Encoding.UTF8, "application/json");
                string endPoint = _configuration["WebApiBaseUrl"] + "Department/UpdateDepartment";
                using (var response = await client.PutAsync(endPoint, content))
                {
                    if (response.StatusCode == System.Net.HttpStatusCode.OK)
                    {
                        ViewBag.status = "Ok";
                        ViewBag.message = "Department Details Updated Successfully!";
                        return RedirectToAction("GetAllDepartments", "Admin");
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

        public async Task<IActionResult> DeleteDepartment(int DepartmentId)
        {
            ViewBag.status = "";
            using (HttpClient client = new HttpClient())
            {
                string endPoint = _configuration["WebApiBaseUrl"] + "Department/DeleteDepartment?departmentId=" + DepartmentId;
                using (var response = await client.DeleteAsync(endPoint))
                {
                    if (response.StatusCode == System.Net.HttpStatusCode.OK)
                    {
                        ViewBag.status = "Ok";
                        ViewBag.message = "Department Details Deleted Successfully!";
                        return RedirectToAction("GetAllDepartments", "Admin");
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
        #endregion

       


    }
}