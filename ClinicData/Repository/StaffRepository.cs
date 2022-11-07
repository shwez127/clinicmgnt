using ClinicData.Data;
using ClinicEntity.Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ClinicData.Repository
{
    public class StaffRepository: IStaffRepository
    {
        ClinicDbContext _clinicDbContext;
        public StaffRepository(ClinicDbContext clinicDbContext)
        {
            _clinicDbContext = clinicDbContext;
        }

        public void AddStaff(OtherStaff otherStaff)
        {
            _clinicDbContext.otherStaffs.Add(otherStaff);
            _clinicDbContext.SaveChanges();
        }

        public void DeleteStaff(int staffId)
        {
            var staff = _clinicDbContext.otherStaffs.Find(staffId);
            _clinicDbContext.otherStaffs.Remove(staff);
            _clinicDbContext.SaveChanges();
        }

        public IEnumerable<OtherStaff> GetAllStaffs()
        {
            return _clinicDbContext.otherStaffs.ToList();
        }

        public OtherStaff GetStaffbyId(int staffId)
        {
            return _clinicDbContext.otherStaffs.Find(staffId);
        }

        public void UpdateStaff(OtherStaff otherStaff)
        {
            _clinicDbContext.Entry(otherStaff).State = EntityState.Modified;
            _clinicDbContext.SaveChanges();
        }
    }
}
