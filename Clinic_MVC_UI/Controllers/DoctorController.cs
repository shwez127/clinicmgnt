using ClinicEntity.Models;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using System.Net.Http;
using System.Threading.Tasks;
using System;
using Microsoft.Extensions.Configuration;

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
        }
        
    }
}
