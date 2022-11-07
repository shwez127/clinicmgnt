using ClinicData.Repository;
using ClinicEntity.Models;
using System;
using System.Collections.Generic;
using System.Text;

namespace ClinicBusiness.Services
{
    public class StaffServices
    {
        IStaffRepository _staffRepository;
        public StaffServices(IStaffRepository staffRepository)
        {
            _staffRepository = staffRepository;
        }
        public void AddStaff(OtherStaff otherStaff)
        {
            _staffRepository.AddStaff(otherStaff);
        }
        public void DeleteStaff(int staffId)
        {
            _staffRepository.DeleteStaff(staffId);
        }
        public void UpdateStaff(OtherStaff otherStaff)
        {
            _staffRepository.UpdateStaff(otherStaff);
        }
        public OtherStaff GetStaffById(int staffId)
        {
            return _staffRepository.GetStaffbyId(staffId);
        }
        public IEnumerable<OtherStaff> GetAllStaffs()
        {
            return _staffRepository.GetAllStaffs();
        }
    }
}
