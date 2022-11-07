using ClinicEntity.Models;
using System;
using System.Collections.Generic;
using System.Text;

namespace ClinicData.Repository
{
    public interface IStaffRepository
    {
        void AddStaff(OtherStaff otherStaff);
        void DeleteStaff(int staffId);
        void UpdateStaff(OtherStaff otherStaff);
        OtherStaff GetStaffbyId(int staffId);
        IEnumerable<OtherStaff> GetAllStaffs();
    }
}
