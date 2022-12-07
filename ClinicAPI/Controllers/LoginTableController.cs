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
        //Private member
        LoginTableService _loginTableService;

        //Constructor
        public LoginTableController(LoginTableService loginTableService)
        {
            _loginTableService = loginTableService;
        }

        [HttpPost("Login")]
        public int[] Login(LoginTable loginTable)
        {
            #region Role based login as admin,patient and doctor
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
            #endregion

        }
        [HttpPost("AddLogin")]
        public int AddLogin(LoginTable loginTable)
        {
            #region Login adding action
            int flag = _loginTableService.AddLogin(loginTable);
            if (flag != 0)
                return flag;
            return 0;
            #endregion
        }
        [HttpPost("ForgetPassword")]
        public LoginTable ForgetPassword(LoginTable login)
        {
            #region Forget password action
            return _loginTableService.ForgetPassword(login);
            #endregion
        }
        [HttpPut("UpdatePassword")]
        public void UpdatePassword(LoginTable login)
        {
            #region Updating password in forget password action
            _loginTableService.UpdatePassword(login);
            #endregion
        }
    }
}
