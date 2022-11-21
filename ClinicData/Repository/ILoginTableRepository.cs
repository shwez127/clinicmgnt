using ClinicEntity.Models;
using System;
using System.Collections.Generic;
using System.Text;
using System.Xml;

namespace ClinicData.Repository
{
    public interface ILoginTableRepository
    {
        public int[] Login(LoginTable loginTable);
        public int AddLoginTable(LoginTable loginTable);

        public LoginTable ForgetPassword(LoginTable loginTable);

        public void UpdatePassword(LoginTable login);
    }
}
