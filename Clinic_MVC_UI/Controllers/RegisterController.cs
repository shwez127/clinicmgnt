using ClinicEntity.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.Extensions.Configuration;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Dynamic;
using System.Net.Http;
using System.Numerics;
using System.Text;
using System.Threading.Tasks;

namespace Clinic_MVC_UI.Controllers
{
    public class RegisterController : Controller
    {
        private IConfiguration _configuration;
        public RegisterController(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        public IActionResult Index()
        {
            return View();

        }

        public async Task<IActionResult> PatientRegister1()   
        {
            #region Select list before registration
            TempData["Notification"] = 0;
            TempData.Keep();           
            List<SelectListItem> gender = new List<SelectListItem>()
            {
                new SelectListItem{Value="Gender",Text="Gender"},
                new SelectListItem{Value="M",Text="Male"},
                new SelectListItem{Value="F",Text="Female"},
                new SelectListItem{Value="O",Text="Others"},
            };
            ViewBag.genderlist = gender;

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
            department.Add(new SelectListItem { Value = "Department", Text = "Department" });
            foreach (var item in departments)
            {
                department.Add(new SelectListItem { Value = item.DeptNo.ToString(), Text = item.DeptName });
            }

            ViewBag.Departmentlist = department;

            dynamic d= new ExpandoObject();
            
           
            return View();
            #endregion
        }
        [HttpPost]
        public async Task<IActionResult> PatientRegister1(Appointment appointment, int Type)
        {
            #region Verifying as doctor/patient
            LoginTable loginTable = new LoginTable();
            if (Type == 1)
            {
                loginTable.Email = appointment.Doctor.LoginTable.Email;
                loginTable.Password = appointment.Doctor.LoginTable.Password;
                loginTable.Type= Type;
            }
            else if(Type==0)
            {
                loginTable.Email = appointment.Patient.LoginTable.Email;
                loginTable.Password = appointment.Patient.LoginTable.Password;
                loginTable.Type = Type;
            }
            #endregion
            #region Patient can register here only by email and password
            ViewBag.Status = "";
            int loginID = 0;
            using (HttpClient client = new HttpClient())
            {
                StringContent content = new StringContent(JsonConvert.SerializeObject(loginTable), Encoding.UTF8, "application/json");
                string endPoint = _configuration["WebApiBaseUrl"] + "LoginTable/AddLogin";
                using (var response = await client.PostAsync(endPoint, content))
                {
                    if (response.StatusCode == System.Net.HttpStatusCode.OK)
                    {
                        var result = await response.Content.ReadAsStringAsync();
                        loginID = JsonConvert.DeserializeObject<int>(result);
                        if (loginID != 0)
                        {
                            TempData["LoginID"] = loginID.ToString();
                            TempData.Keep();

                        }
                       
                    }
                    else
                    {
                        ViewBag.status = "Error";
                        ViewBag.message = "Wrong Entries";
                    }
                }
            }
            if (Type == 1)
            {
                Doctor doctor=new Doctor();
                doctor.DoctorID = loginID;
                doctor.Name = appointment.Doctor.Name;
                doctor.Address = appointment.Doctor.Address;
                doctor.BirthDate = appointment.Doctor.BirthDate;
                doctor.Gender = appointment.Doctor.Gender;  
                doctor.Phone = appointment.Doctor.Phone;
                doctor.Deptno = appointment.Doctor.Deptno;
                doctor.Specialization = appointment.Doctor.Specialization;
                doctor.Qualification= appointment.Doctor.Qualification;
                doctor.Charges_Per_Visit = appointment.Doctor.Charges_Per_Visit;
                doctor.Work_Experience = appointment.Doctor.Work_Experience;
                ViewBag.Status = "";
                using (HttpClient client = new HttpClient())
                {
                    StringContent content = new StringContent(JsonConvert.SerializeObject(doctor), Encoding.UTF8, "application/json");
                    string endPoint = _configuration["WebApiBaseUrl"] + "Doctor/AddDoctor";
                    using (var response = await client.PostAsync(endPoint, content))
                    {
                        if (response.StatusCode == System.Net.HttpStatusCode.OK)
                        {
                            ViewBag.status = "Ok";
                            ViewBag.message = "Doctor Registered Successfully";
                            return RedirectToAction("Index", "LoginTable");
                        }
                        else
                        {
                            ViewBag.status = "Error";
                            ViewBag.message = "Wrong Entries";
                        }
                    }
                }

            }
            else if (Type == 0){
                ViewBag.Status = "";
                Patient patient= new Patient();
                patient.PatientID = loginID;
                patient.Name = appointment.Patient.Name;
                patient.Address = appointment.Patient.Address;
                patient.BirthDate=appointment.Patient.BirthDate;
                patient.Gender = appointment.Patient.Gender;
                patient.Phone = appointment.Patient.Phone;
                using (HttpClient client = new HttpClient())
                {
                    StringContent content = new StringContent(JsonConvert.SerializeObject(patient), Encoding.UTF8, "application/json");
                    string endPoint = _configuration["WebApiBaseUrl"] + "Patient/AddPatient";
                    using (var response = await client.PostAsync(endPoint, content))
                    {
                        if (response.StatusCode == System.Net.HttpStatusCode.OK)
                        {
                            ViewBag.status = "Ok";
                            ViewBag.message = "Patient Registered Successfully";
                            return RedirectToAction("Index", "LoginTable");
                        }
                        else
                        {
                            ViewBag.status = "Error";
                            ViewBag.message = "Wrong Entries";
                        }
                    }
                }
            }
            return View();
            #endregion
        }
        
        public IActionResult ForgetPassword()
        {
            return View();
        }
        [HttpPost]
        public async Task<IActionResult> ForgetPassword(LoginTable loginTable)
        {
            #region Forget Password
            loginTable.Password ="hell";
            LoginTable loginTable1 = new LoginTable();  
            ViewBag.status = "";
            int[] arr = new int[2];
            TempData["EmailAdress"] = loginTable.Email;
            using (HttpClient client = new HttpClient())
            {
                StringContent content = new StringContent(JsonConvert.SerializeObject(loginTable), Encoding.UTF8, "application/json");
                string endpoint = _configuration["WebApiBaseUrl"] + "LoginTable/ForgetPassword";
                using (var response = await client.PostAsync(endpoint, content))
                {
                    var result = await response.Content.ReadAsStringAsync();
                    loginTable1 = JsonConvert.DeserializeObject<LoginTable>(result);


                    if (response.StatusCode == System.Net.HttpStatusCode.OK)
                    {
                        TempData["ForgetId"] = loginTable1.ID;
                        TempData.Keep();
                        TempData["TypeId"] = loginTable1.Type;
                        TempData.Keep();
                        ViewBag.status = "Ok";
                        ViewBag.message = "Login Succesfully";
                        if (loginTable1.Type == 0)
                        {

                            return RedirectToAction("UpdatePassword", "Register");
                        }
                        else if (loginTable1.Type == 1)
                        {
                            
                            TempData.Keep();
                            return RedirectToAction("UpdatePassword", "Register");
                        }


                    }
                    else
                    {
                        ViewBag.staus = "Error";
                        ViewBag.message = "Wrong Entries";
                    }
                }
            }

            return View();
            #endregion
        }
        public IActionResult UpdatePassword()
        {
            return View();
        }
        [HttpPost]
        public async Task<IActionResult> UpdatePassword(Doctor doctor)
        {
            #region Update password
            LoginTable loginTable = new LoginTable();
            loginTable.ID = Convert.ToInt32(TempData["ForgetId"]);
            loginTable.Type = Convert.ToInt32(TempData["TypeId"]);
            if (Convert.ToInt32(TempData["TypeId"]) == 1)
            {
                Doctor doctor1 = null;
                using (HttpClient client = new HttpClient())
                {
                    string endpoint = _configuration["WebApiBaseUrl"] + "Doctor/GetDoctorById?doctorId=" + Convert.ToInt32(TempData["ForgetId"]);
                    using (var response = await client.GetAsync(endpoint))
                    {
                        if (response.StatusCode == System.Net.HttpStatusCode.OK)
                        {
                            var result = await response.Content.ReadAsStringAsync();
                            doctor1 = JsonConvert.DeserializeObject<Doctor>(result);
                        }
                    }
                }
                //Checking birthdate to update password
                if (doctor1.BirthDate == doctor.BirthDate)
                {
                    loginTable.Password = doctor.LoginTable.Password;
                }
            }
            else if (Convert.ToInt32(TempData["TypeId"]) == 0)
            {
                Patient patient = null;
                using (HttpClient client = new HttpClient())
                {
                    string endpoint = _configuration["WebApiBaseUrl"] + "Patient/GetPatientById?patientId=" + Convert.ToInt32(TempData["ForgetId"]);
                    using (var response = await client.GetAsync(endpoint))
                    {
                        if (response.StatusCode == System.Net.HttpStatusCode.OK)
                        {
                            var result = await response.Content.ReadAsStringAsync();
                            patient = JsonConvert.DeserializeObject<Patient>(result);
                        }
                    }
                }
                if (patient.BirthDate == doctor.BirthDate)
                {
                    loginTable.Password = doctor.LoginTable.Password;
                }

            }
            loginTable.Email = TempData["EmailAdress"].ToString();

            ViewBag.status = "";
            using (HttpClient client = new HttpClient())
            {
                StringContent content = new StringContent(JsonConvert.SerializeObject(loginTable), Encoding.UTF8, "application/json");
                string endPoint = _configuration["WebApiBaseUrl"] + "LoginTable/UpdatePassword";
                using (var response = await client.PutAsync(endPoint, content))
                {
                    if (response.StatusCode == System.Net.HttpStatusCode.OK)
                    {
                        ViewBag.status = "Ok";
                        ViewBag.message = "Password Updated Successfully";
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
      
    }
}



