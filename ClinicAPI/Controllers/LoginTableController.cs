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
        public int[] Login(LoginTable loginTable)
        {
            int[] flag = _loginTableService.Login(loginTable);
            if ( flag[1]==3)
            {
                return flag;
            }else
            if ( flag[1] == 0)
            {
                return flag;
            }else
            if ( flag[1] == 1)
            {
                return flag; ;
            }
            return null;

        }
        [HttpPost("AddLogin")]
        public int AddLogin(LoginTable loginTable)
        {
          int flag= _loginTableService.AddLogin(loginTable);
            if (flag != 0)
                return flag;
            return 0;
        }
        [HttpPost("ForgetPassword")]
        public LoginTable ForgetPassword(LoginTable login)
        {
            return _loginTableService.ForgetPassword(login);
        }
        [HttpPut("UpdatePassword")]
        public void UpdatePassword(LoginTable login)
        {
            _loginTableService.UpdatePassword(login);
        }
    }
}
