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
            //Arrange
            var department = _fixture.Create<Department>();
            moq.Setup(x => x.AddDepartment(department));
            departmentController = new DepartmentController(new DepartmentService(moq.Object));

            //Act
            var result = departmentController.AddDepartment(department);
            var Obj = result as ObjectResult;

            //Assert
            Assert.AreEqual(200, Obj.StatusCode);
        }

        [TestMethod()]
        public async Task AddDepartmentNegativeTest()
        {
            //Arrange
            var department = _fixture.Create<Department>();
            moq.Setup(x => x.AddDepartment(department)).Throws(new Exception());
            departmentController = new DepartmentController(new DepartmentService(moq.Object));

            //Act
            var result = departmentController.AddDepartment(department);
            var Obj = result as ObjectResult;

            //Assert
            Assert.AreEqual(Obj.StatusCode, 400);
        }

        [TestMethod()]
        public void UpdateDepartmentTest()
        {
            //Arrange
            var department = _fixture.Create<Department>();
            moq.Setup(x => x.UpdateDepartment(department));
            departmentController = new DepartmentController(new DepartmentService(moq.Object));

            //Act
            var result = departmentController.UpdateDepartment(department);
            var Obj = result as ObjectResult;

            //Assert
            Assert.AreEqual(200, Obj.StatusCode);
        }

        [TestMethod()]
        public void Update_ThrowsException_IfIdNotFound()
        {
            //Arrange
            var department = _fixture.Create<Department>();
            moq.Setup(x => x.UpdateDepartment(department)).Throws(new Exception());
            departmentController = new DepartmentController(new DepartmentService(moq.Object));

            //Act
            var result = departmentController.UpdateDepartment(department);
            var Obj = result as ObjectResult;

            //Assert
            Assert.AreEqual(Obj.StatusCode, 400);
        }

        [TestMethod()]
        public void DeleteDepartmentTest()
        {
            //Arrange
            var department = _fixture.Create<Department>();
            moq.Setup(x => x.DeleteDepartment(It.IsAny<int>()));
            departmentController = new DepartmentController(new DepartmentService(moq.Object));

            //Act
            var result = departmentController.DeleteDepartment(It.IsAny<int>());
            var Obj = result as ObjectResult;

            //Assert
            Assert.AreEqual(Obj.StatusCode, 200);
        }

        [TestMethod()]
        public void Delete_ThrowsException_IfIdNotFound()
        {
            //Arrange
            var department = _fixture.Create<Department>();
            moq.Setup(x => x.DeleteDepartment(It.IsAny<int>())).Throws(new Exception());
            departmentController = new DepartmentController(new DepartmentService(moq.Object));

            //Act
            var result = departmentController.DeleteDepartment(It.IsAny<int>());
            var Obj = result as ObjectResult;

            //Assert
            Assert.AreEqual(Obj.StatusCode, 400);
        }
        
        [TestMethod()]
        public void GetDepartmentsTest()
        {
            //Arrange
            var departmentlist = _fixture.CreateMany<Department>(3).ToList();
            moq.Setup(x => x.GetDepartments()).Returns(departmentlist);
            departmentController = new DepartmentController(new DepartmentService(moq.Object));

            //Act
            var result = departmentController.GetDepartments();

            //Assert
            Assert.AreEqual(result.Count(), 3);
        }

        [TestMethod()]
        public async Task GetDepartmentsNeagtiveTest()
        {
            //Arrange
            List<Department> departments = null;
            moq.Setup(x => x.GetDepartments()).Returns(departments);
            departmentController = new DepartmentController(new DepartmentService(moq.Object));

            //Assert
            Assert.IsNull(departmentController.GetDepartments());
        }

        [TestMethod()]
        public void GetDepartmentsByIdTest()
        {
            //Arrange
            var department = _fixture.Create<Department>();
            moq.Setup(x => x.getDepartmentById(1)).Returns(department);
            departmentController = new DepartmentController(new DepartmentService(moq.Object));

            //Assert
            Assert.AreEqual(departmentController.GetDepartmentsById(1), department);
        }
    }
}