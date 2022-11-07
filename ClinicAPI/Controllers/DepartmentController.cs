using ClinicBusiness.Services;
using ClinicEntity.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ClinicAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class DepartmentController : ControllerBase
    {
        DepartmentService _departmentService;
        public DepartmentController(DepartmentService departmentService)
        {
            _departmentService = departmentService;
        }

        [HttpPost("AddDepartment")]
        public IActionResult AddDepartment(Department department)
        {
            _departmentService.AddDepartment(department);
            return Ok("Department Added Successfully");
        }

        [HttpPut("UpdateDepartment")]
        public IActionResult UpdateDepartment([FromBody] Department department)
        {
            _departmentService.UpdateDepartment(department);
            return Ok("Department updated successfully");
        }

        [HttpDelete("DeleteDepartment")]
        public IActionResult DeleteDepartment(int departmentId)
        {
            _departmentService.DeleteDepartment(departmentId);
            return Ok("Department deleted successfully");
        }

        [HttpGet("GetDepartments")]
        public IEnumerable<Department> GetDepartments()
        {
            return _departmentService.GetDepartments();
        }

        [HttpGet("GetDepartmentsById")]
        public Department GetDepartmentsById(int departmentId)
        {
            return _departmentService.GetDepartmentById(departmentId);
        }
    }
}
