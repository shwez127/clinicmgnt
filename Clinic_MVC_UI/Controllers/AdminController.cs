using ClinicEntity.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.Extensions.Configuration;
using Newtonsoft.Json;
using System.Collections.Generic;
using System.IO;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

namespace Clinic_MVC_UI.Controllers
{
    public class AdminController : Controller
    {
        private IConfiguration _configuration;
        public AdminController(IConfiguration configuration)
        {
            _configuration = configuration;
        }
        public IActionResult Index()
        {
            return View();
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

        #region Doctor View-Edit-Delete Actions
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

        [HttpGet]
        public async Task<IActionResult> GetAllDoctors()
        {
            IEnumerable<Doctor> doctorresult = null;
            using (HttpClient client = new HttpClient())
            {
                string endPoint = _configuration["WebApiBaseUrl"] + "Doctor/GetDoctors";
                using (var response = await client.GetAsync(endPoint))
                {
                    if (response.StatusCode == System.Net.HttpStatusCode.OK)
                    {
                        var result = await response.Content.ReadAsStringAsync();
                        doctorresult = JsonConvert.DeserializeObject<IEnumerable<Doctor>>(result);
                    }
                }
            }
            return View(doctorresult);
        }

        public async Task<IActionResult> EditDoctor(int DoctorId)
        {
            Doctor doctor = null;
            using (HttpClient client = new HttpClient())
            {
                string endPoint = _configuration["WebApiBaseUrl"] + "Doctor/GetDoctorById?doctorId=" + DoctorId;
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
            Doctor doctor = null;
            using (HttpClient client = new HttpClient())
            {
                string endPoint = _configuration["WebApiBaseUrl"] + "Doctor/GetDoctorById?doctorId=" + DoctorId;
                using (var response = await client.GetAsync(endPoint))
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

        [HttpPost]
        public async Task<IActionResult> DeleteDoctor(Doctor doctor)
        {
            ViewBag.status = "";
            using (HttpClient client = new HttpClient())
            {
                string endPoint = _configuration["WebApiBaseUrl"] + "Doctor/DeleteDoctor?doctorId=" + doctor.DoctorID;
                using (var response = await client.DeleteAsync(endPoint))
                {
                    if (response.StatusCode == System.Net.HttpStatusCode.OK)
                    {
                        ViewBag.status = "Ok";
                        ViewBag.message = "Doctor Details Deleted Successfully!";
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

        #region Patient View-Edit-Delete Actions
        [HttpGet]
        public async Task<IActionResult> GetAllPatients()
        {
            IEnumerable<Patient> patientresult = null;
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
            return View(patient);

        }

        [HttpPost]
        public async Task<IActionResult> DeletePatient(Patient patient)
        {
            ViewBag.status = "";
            using (HttpClient client = new HttpClient())
            {
                string endPoint = _configuration["WebApiBaseUrl"] + "Patient/DeletePatient?patientId=" + patient.PatientID;
                using (var response = await client.DeleteAsync(endPoint))
                {
                    if (response.StatusCode == System.Net.HttpStatusCode.OK)
                    {
                        ViewBag.status = "Ok";
                        ViewBag.message = "Patient Details Deleted Successfully!";
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

        #region OtherStaffs Create-View-Edit-Delete Actions
        public async Task<IActionResult> GetAllOtherStaffs()
        {
            IEnumerable<OtherStaff> otherstaffresult = null;
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
            OtherStaff otherStaff = null;
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
            return View(otherStaff);

        }

        [HttpPost]
        public async Task<IActionResult> DeleteOtherStaff(OtherStaff otherStaff)
        {
            ViewBag.status = "";
            using (HttpClient client = new HttpClient())
            {
                string endPoint = _configuration["WebApiBaseUrl"] + "Staff/DeleteStaff?staffId=" + otherStaff.StaffID;
                using (var response = await client.DeleteAsync(endPoint))
                {
                    if (response.StatusCode == System.Net.HttpStatusCode.OK)
                    {
                        ViewBag.status = "Ok";
                        ViewBag.message = "Staff Details Deleted Successfully!";
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