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
        //Private member
        DepartmentService _departmentService;

        //Constructor
        public DepartmentController(DepartmentService departmentService)
        {
            _departmentService = departmentService;
        }

        [HttpPost("AddDepartment")]
        public IActionResult AddDepartment(Department department)
        {
            try
            {

                #region Department adding action
                _departmentService.AddDepartment(department);
                return Ok("Department Added Successfully");
                #endregion
            }
            catch
            {
                return BadRequest(400);
            }
        }

        [HttpPut("UpdateDepartment")]
        public IActionResult UpdateDepartment([FromBody] Department department)
        {
            try
            {
                #region Department updation action
                _departmentService.UpdateDepartment(department);
                return Ok("Department updated successfully");
                #endregion
            }
            catch
            {
                return BadRequest(400);
            }

        }

        [HttpDelete("DeleteDepartment")]
        public IActionResult DeleteDepartment(int departmentId)
        {
            try
            {
                #region Department deletion action
                _departmentService.DeleteDepartment(departmentId);
                return Ok("Department deleted successfully");
                #endregion
            }
            catch
            {
                return BadRequest(400);
            }
        }

        [HttpGet("GetDepartments")]
        public IEnumerable<Department> GetDepartments()
        {
            #region Listing all departments
            return _departmentService.GetDepartments();
            #endregion
        }

        [HttpGet("GetDepartmentsById")]
        public Department GetDepartmentsById(int departmentId)
        {
            #region Search department by department ID
            return _departmentService.GetDepartmentById(departmentId);
            #endregion
        }
    }
}
