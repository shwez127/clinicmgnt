using ClinicEntity.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.Extensions.Configuration;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Net.Http;
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

        public IActionResult PatientRegister1()
        {
            List<SelectListItem> gender = new List<SelectListItem>()
            {
                new SelectListItem{Value="Select",Text="select"},
                new SelectListItem{Value="0",Text="Are You Patient?"},
                new SelectListItem{Value="1",Text="Are You Doctor?"},
               
            };
            ViewBag.genderlist = gender;

            return View();
        }
        [HttpPost]
        public async Task<IActionResult> PatientRegister1(LoginTable loginTable)
        {
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
                            TempData["LoginID"]=loginID.ToString();
                            TempData.Keep();
                        }
                        if (loginTable.Type == 0)
                        {
                            ViewBag.status = "Ok";
                            ViewBag.message = "Patient Registered Successfully";
                            return RedirectToAction("PatientRegister2", "Register");
                        }
                        else if(loginTable.Type == 1)
                        {
                            ViewBag.status = "Ok";
                            ViewBag.message = "Patient Registered Successfully";
                            return RedirectToAction("DoctorRegister2", "Register");
                        }
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
        public IActionResult PatientRegister2()
        {
            return View();
        }
        [HttpPost]
        public async Task<IActionResult> PatientRegister2(Patient patient)
        {
            patient.PatientID = Convert.ToInt32( TempData["LoginID"]);
            ViewBag.Status = "";
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
            return View();
        }
       /* public IActionResult DoctorRegister1()
        {
            return View();
        }
        [HttpPost]
        public async Task<IActionResult> DoctorRegister1(LoginTable loginTable)
        {
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
                        ViewBag.status = "Ok";
                        ViewBag.message = "Patient Registered Successfully";
                        return RedirectToAction("DoctorRegister2", "Register");
                    }
                    else
                    {
                        ViewBag.status = "Error";
                        ViewBag.message = "Wrong Entries";
                    }
                }
            }
            return View();
        }*/
        public IActionResult DoctorRegister2()
        {
            return View();
        }
        [HttpPost]
        public async Task<IActionResult> DoctorRegister2(Doctor doctor)
        {
            doctor.DoctorID = Convert.ToInt32(TempData["LoginID"]);
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
            return View();
        }
    }
}
