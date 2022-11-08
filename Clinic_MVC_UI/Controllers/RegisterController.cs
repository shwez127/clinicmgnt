using ClinicEntity.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Newtonsoft.Json;
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
            return View();
        }
        [HttpPost]
        public async Task<IActionResult> PatientRegister1(LoginTable loginTable)
        {
            ViewBag.Status = "";
            using (HttpClient client = new HttpClient())
            {
                StringContent content = new StringContent(JsonConvert.SerializeObject(loginTable), Encoding.UTF8, "application/json");
                string endPoint = _configuration["WebApiBaseUrl"] + "Logintable/AddPatient";
                using (var response = await client.PostAsync(endPoint, content))
                {
                    if (response.StatusCode == System.Net.HttpStatusCode.OK)
                    {
                        ViewBag.status = "Ok";
                        ViewBag.message = "Patient Registered Successfully";
                        return RedirectToAction("PatientRegister2", "Register");
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
        public async Task<IActionResult> PatientRegister2(LoginTable loginTable)
        {
            ViewBag.Status = "";
            using (HttpClient client = new HttpClient())
            {
                StringContent content = new StringContent(JsonConvert.SerializeObject(loginTable), Encoding.UTF8, "application/json");
                string endPoint = _configuration["WebApiBaseUrl"] + "Patient/AddPatient";
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
    }
}
