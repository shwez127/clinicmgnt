using Microsoft.VisualStudio.TestTools.UnitTesting;
using ClinicAPI.Controllers;
using Moq;
using AutoFixture;
using ClinicData.Repository;
using ClinicBusiness.Services;
using Microsoft.AspNetCore.Mvc;
using ClinicEntity.Models;



namespace ClinicAPI.Controllers.Tests
{
    [TestClass()]
    public class DepartmentControllerTests
    {
        DepartmentController departmentController;
        Fixture _fixture;
        Mock<IDepartmentRepository> moq;

        public DepartmentControllerTests()
        {
            _fixture = new Fixture();
            moq = new Mock<IDepartmentRepository>();
        }
        [TestInitialize]
        public void setup()
        {
            var moq = new Mock<DepartmentService>();
        }



        [TestMethod()]
        public void AddDepartmentTest()
        {
            var department = _fixture.Create<Department>();
            moq.Setup(x => x.AddDepartment(department));
            departmentController = new DepartmentController(new DepartmentService(moq.Object));
            var result = departmentController.AddDepartment(department);
            var Obj = result as ObjectResult;
            Assert.AreEqual(200, Obj.StatusCode);
        }

        [TestMethod()]
        public async Task AddDepartmentNegativeTest()
        {
            var department = _fixture.Create<Department>();
            moq.Setup(x => x.AddDepartment(department)).Throws(new Exception());
            departmentController = new DepartmentController(new DepartmentService(moq.Object));
            var result = departmentController.AddDepartment(department);
            var Obj = result as ObjectResult;
            Assert.AreEqual(Obj.StatusCode, 400);
        }

        [TestMethod()]
        public void UpdateDepartmentTest()
        {
            var department = _fixture.Create<Department>();
            moq.Setup(x => x.UpdateDepartment(department));
            departmentController = new DepartmentController(new DepartmentService(moq.Object));
            var result = departmentController.UpdateDepartment(department);
            var Obj = result as ObjectResult;
            Assert.AreEqual(200, Obj.StatusCode);
        }

        [TestMethod()]
        public void Update_ThrowsException_IfIdNotFound()
        {
            var department = _fixture.Create<Department>();
            moq.Setup(x => x.UpdateDepartment(department)).Throws(new Exception());
            departmentController = new DepartmentController(new DepartmentService(moq.Object));
            var result = departmentController.UpdateDepartment(department);
            var Obj = result as ObjectResult;
            Assert.AreEqual(Obj.StatusCode, 400);
        }

        [TestMethod()]
        public void DeleteDepartmentTest()
        {
            var department = _fixture.Create<Department>();
            moq.Setup(x => x.DeleteDepartment(It.IsAny<int>()));
            departmentController = new DepartmentController(new DepartmentService(moq.Object));
            var result = departmentController.DeleteDepartment(It.IsAny<int>());
            var Obj = result as ObjectResult;
            Assert.AreEqual(Obj.StatusCode, 200);
        }

        [TestMethod()]
        public void Delete_ThrowsException_IfIdNotFound()
        {
            var department = _fixture.Create<Department>();
            moq.Setup(x => x.DeleteDepartment(It.IsAny<int>())).Throws(new Exception());
            departmentController = new DepartmentController(new DepartmentService(moq.Object));
            var result = departmentController.DeleteDepartment(It.IsAny<int>());
            var Obj = result as ObjectResult;
            Assert.AreEqual(Obj.StatusCode, 400);
        }
        
        [TestMethod()]
        public void GetDepartmentsTest()
        {
            var departmentlist = _fixture.CreateMany<Department>(3).ToList();
            moq.Setup(x => x.GetDepartments()).Returns(departmentlist);
            departmentController = new DepartmentController(new DepartmentService(moq.Object));
            var result = departmentController.GetDepartments();
            Assert.AreEqual(result.Count(), 3);
        }

        [TestMethod()]
        public async Task GetDepartmentsNeagtiveTest()
        {
            var departmentlist = _fixture.CreateMany<Department>().ToList();
            moq.Setup(x => x.GetDepartments()).Returns(departmentlist);
            departmentController = new DepartmentController(new DepartmentService(moq.Object));
            var result = departmentController.GetDepartments();
        }

        [TestMethod()]
        public void GetDepartmentsByIdTest()
        {
            var department = _fixture.Create<Department>();
            moq.Setup(x => x.getDepartmentById(1)).Returns(department);
            departmentController = new DepartmentController(new DepartmentService(moq.Object));
            Assert.AreEqual(departmentController.GetDepartmentsById(1), department);
        }
    }
}