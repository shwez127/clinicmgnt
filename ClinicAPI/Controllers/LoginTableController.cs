using ClinicBusiness.Services;
using ClinicEntity.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ClinicAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class LoginTableController : ControllerBase
    {
        LoginTableService _loginTableService;
        public LoginTableController(LoginTableService loginTableService)
        {
            _loginTableService = loginTableService;
        }

        [HttpPost("Login")]
        public IActionResult Login(LoginTable loginTable)
        {
            int flag = _loginTableService.Login(loginTable);
            if (flag == 1)
            {
                return Ok("Admin Login Suceesfully");
            }
            return BadRequest("Invalid user");
        }
    }
}
