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
        public int[] Login(LoginTable loginTable)
{
            int[] arr = { -1, 4 };
          List<LoginTable> LoginLists=_clinicDb.logintables.ToList();
            var TotalList = from v in LoginLists select v;
            if (loginTable.Email == "Prabhu@gmail.com" && loginTable.Password == "prabhu123" )
            {
                arr[1] = 3;
                return arr;
            }else
            {
                foreach(var i in TotalList)
                {
                    if(i.Email==loginTable.Email && i.Password==loginTable.Password )
                    {
                        arr[0]=i.ID;
                        arr[1] = i.Type;
                    }
                }
            }
            return arr;

        }
    }
}
