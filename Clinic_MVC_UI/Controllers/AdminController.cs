using ClinicEntity.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Newtonsoft.Json;
using System.Collections.Generic;
using System.Net.Http;
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
        [HttpGet]
        public async Task<IActionResult> GetAllDoctors()
        {
            IEnumerable<Doctor> bookingresult = null;
            using (HttpClient client = new HttpClient())
            {
                string endPoint = _configuration["WebApiBaseUrl"] + "Doctor/GetDoctors";
                using (var response = await client.GetAsync(endPoint))
                {
                    if (response.StatusCode == System.Net.HttpStatusCode.OK)
                    {
                        var result = await response.Content.ReadAsStringAsync();
                        bookingresult = JsonConvert.DeserializeObject<IEnumerable<Doctor>>(result);
                    }
                }
            }
            return View(bookingresult);
        }
        [HttpGet]
        public async Task<IActionResult> GetAllPatients()
        {
            IEnumerable<Patient> bookingresult = null;
            using (HttpClient client = new HttpClient())
            {
                string endPoint = _configuration["WebApiBaseUrl"] + "Patient/GetAllPatients";
                using (var response = await client.GetAsync(endPoint))
                {
                    if (response.StatusCode == System.Net.HttpStatusCode.OK)
                    {
                        var result = await response.Content.ReadAsStringAsync();
                        bookingresult = JsonConvert.DeserializeObject<IEnumerable<Patient>>(result);
                    }
                }
            }
            return View(bookingresult);
        }
    }
}
