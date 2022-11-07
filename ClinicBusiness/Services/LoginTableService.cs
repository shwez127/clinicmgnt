using ClinicData.Repository;
using ClinicEntity.Models;
using System;
using System.Collections.Generic;
using System.Text;

namespace ClinicBusiness.Services
{
    public class LoginTableService
    {
        ILoginTableRepository _logintableRepository;
        public LoginTableService(ILoginTableRepository logintableRepository)
        {
            _logintableRepository = logintableRepository;
        }

        public int Login(LoginTable loginTable)
        {
            return _logintableRepository.Login(loginTable);
        }
    }
}
