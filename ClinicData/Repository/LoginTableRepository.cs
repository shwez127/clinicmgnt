using ClinicData.Data;
using ClinicEntity.Models;
using System;
using System.Collections.Generic;
using System.Linq;
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


      
        public int AddLoginTable(LoginTable loginTable)
        {
            _clinicDb.logintables.Add(loginTable);
            _clinicDb.SaveChanges();
            List<LoginTable> list = new List<LoginTable>();
            list = _clinicDb.logintables.ToList();
            var loginTable1 = (from list1 in list
                               select list1).Last();
            return loginTable1.ID;
        }
        public int Login(LoginTable loginTable)
{
          List<LoginTable> LoginLists=_clinicDb.logintables.ToList();
            var TotalList = from v in LoginLists select v;
            if (loginTable.Email == "Prabhu@gmail.com" && loginTable.Password == "prabhu123" && loginTable.Type==0)
            {
                return 1;
            }else
            {
                foreach(var i in TotalList)
                {
                    if(i.Email==loginTable.Email && i.Password==loginTable.Password && i.Type == loginTable.Type)
                    {
                        return i.Type;
                    }
                }
            }
            return 0;

        }
    }
}
