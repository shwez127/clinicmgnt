using Microsoft.VisualStudio.TestTools.UnitTesting;
using ClinicAPI.Controllers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AutoFixture;
using ClinicData.Repository;
using Moq;
using ClinicBusiness.Services;
using ClinicEntity.Models;
using Microsoft.AspNetCore.Mvc;

namespace ClinicAPI.Controllers.Tests
{
    [TestClass()]
    public class StaffControllerTests
    {
        StaffController staffController;
        Fixture _fixture;
        Mock<IStaffRepository> moq;

        public StaffControllerTests()
        {
            _fixture = new Fixture();
            moq = new Mock<IStaffRepository>();
        }

        [TestInitialize]
        public void setup()
        {
            var moq = new Mock<StaffServices>();
        }

        [TestMethod()]
        public void GetAllStaffsTest()
        {
            var stafflist = _fixture.CreateMany<OtherStaff>(3).ToList();
            moq.Setup(x => x.GetAllStaffs()).Returns(stafflist);
            staffController = new StaffController(new StaffServices(moq.Object));
            var result = staffController.GetAllStaffs();
            Assert.AreEqual(result.Count(), 3);
        }

        [TestMethod()]
        public async Task GetAllStaffsNeagtiveTest()
        {
            var stafflist = _fixture.CreateMany<OtherStaff>(3).ToList();
            moq.Setup(x => x.GetAllStaffs()).Returns(stafflist);
            staffController = new StaffController(new StaffServices(moq.Object));
            var result = staffController.GetAllStaffs();
        }

        [TestMethod()]
        public void AddStaffTest()
        {
            var otherStaff = _fixture.Create<OtherStaff>();
            moq.Setup(x => x.AddStaff(otherStaff));
            staffController = new StaffController(new StaffServices(moq.Object));
            var result = staffController.AddStaff(otherStaff);
            var Obj = result as ObjectResult;
            Assert.AreEqual(200, Obj.StatusCode);
        }

        [TestMethod()]
        public async Task AddStaffNegativeTest()
        {
            var otherStaff = _fixture.Create<OtherStaff>();
            moq.Setup(x => x.AddStaff(otherStaff)).Throws(new Exception());
            staffController = new StaffController(new StaffServices(moq.Object));
            var result = staffController.AddStaff(otherStaff);
            var Obj = result as ObjectResult;
            Assert.AreEqual(Obj.StatusCode, 400);
        }

        [TestMethod()]
        public void UpdateStaffTest()
        {
            var otherStaff = _fixture.Create<OtherStaff>();
            moq.Setup(x => x.UpdateStaff(otherStaff));
            staffController = new StaffController(new StaffServices(moq.Object));
            var result = staffController.UpdateStaff(otherStaff);
            var Obj = result as ObjectResult;
            Assert.AreEqual(200, Obj.StatusCode);
        }

        [TestMethod()]
        public void Update_ThrowsException_IfIdNotFound()
        {
            var otherStaff = _fixture.Create<OtherStaff>();
            moq.Setup(x => x.UpdateStaff(otherStaff)).Throws(new Exception());
            staffController = new StaffController(new StaffServices(moq.Object));
            var result = staffController.UpdateStaff(otherStaff);
            var Obj = result as ObjectResult;
            Assert.AreEqual(Obj.StatusCode, 400);
        }

        [TestMethod()]
        public void DeleteStaffTest()
        {
            var otherStaff = _fixture.Create<OtherStaff>();
            moq.Setup(x => x.DeleteStaff(It.IsAny<int>()));
            staffController = new StaffController(new StaffServices(moq.Object));
            var result = staffController.DeleteStaff(It.IsAny<int>());
            var Obj = result as ObjectResult;
            Assert.AreEqual(200, Obj.StatusCode);
        }

        [TestMethod()]
        public void Delete_ThrowsException_IfIdNotFound()
        {
            var otherStaff = _fixture.Create<OtherStaff>();
            moq.Setup(x => x.DeleteStaff(It.IsAny<int>())).Throws(new Exception());
            staffController = new StaffController(new StaffServices(moq.Object));
            var result = staffController.DeleteStaff(It.IsAny<int>());
            var Obj = result as ObjectResult;
            Assert.AreEqual(Obj.StatusCode, 400);
        }

        [TestMethod()]
        public void GetStaffByIdTest()
        {
            var otherstaff = _fixture.Create<OtherStaff>();
            moq.Setup(x => x.GetStaffbyId(1)).Returns(otherstaff);
            staffController = new StaffController(new StaffServices(moq.Object));
            Assert.AreEqual(staffController.GetStaffById(1), otherstaff);
        }
    }
}