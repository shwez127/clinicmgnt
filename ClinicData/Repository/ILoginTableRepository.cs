using ClinicEntity.Models;
using System;
using System.Collections.Generic;
using System.Text;

namespace ClinicData.Repository
{
    public interface ILoginTableRepository
    {
        public int Login(LoginTable loginTable);    
    }
}
