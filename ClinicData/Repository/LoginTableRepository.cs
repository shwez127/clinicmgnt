using ClinicData.Data;
using ClinicEntity.Models;
using System;
using System.Collections.Generic;
using System.Text;

namespace ClinicData.Repository
{
    public class LoginTableRepository: ILoginTableRepository
    {
        ClinicDbContext _clinicDb;
        public LoginTableRepository(ClinicDbContext clinicDb)
        {
            _clinicDb = clinicDb;
        }
        public int Login(LoginTable loginTable)
        {
            if (loginTable.Email == "Prabhu@gmail.com" && loginTable.Password == "prabhu123")
            {
                return 1;
            }
            return 0;

        }
    }
}
