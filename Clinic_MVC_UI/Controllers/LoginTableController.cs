using ClinicEntity.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

namespace Clinic_MVC_UI.Controllers
{
    public class LoginTableController : Controller
    {
        private IConfiguration _configuration;
        public LoginTableController(IConfiguration configuration)
        {
            _configuration = configuration;
        }
        public IActionResult Index()
        {
            return View();
        }
        [HttpPost]
        public async Task<IActionResult> Index(LoginTable loginTable)
        {
            ViewBag.status = "";
            int[] arr;
            using (HttpClient client = new HttpClient())
            {
                StringContent content = new StringContent(JsonConvert.SerializeObject(loginTable), Encoding.UTF8, "application/json");
                string endpoint = _configuration["WebApiBaseUrl"] + "LoginTable/Login";
                using (var response = await client.PostAsync(endpoint, content))
                {
                    var result = await response.Content.ReadAsStringAsync();
                    arr = JsonConvert.DeserializeObject<int[]>(result);
                    
                    TempData.Keep();
                    if (response.StatusCode == System.Net.HttpStatusCode.OK)
                    {
                        var x = TempData["ProfileId"] = arr[0].ToString();
                        TempData.Keep();
                        ViewBag.status = "Ok";
                        ViewBag.message = "Login Succesfully";
                        if (arr[1] == 0)
                        {
                           /* var x = TempData["PatientID"] = arr[0].ToString();
                            TempData.Keep();*/
                            return RedirectToAction("Index","Patient");
                        }else if (arr[1] == 1)
                        {
                           /* var x = TempData["DoctorID"] = arr[0].ToString();
                            TempData.Keep();*/
                            return RedirectToAction("Index","Doctor");
                        }else if(arr[1] == 3)
                        {
                            return RedirectToAction("Index", "Admin");
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
        }
        
    }
}
