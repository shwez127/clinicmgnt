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
    public class FeedbackController : ControllerBase
    {
        FeedbackServices _feedbackService;
        public FeedbackController(FeedbackServices feedbackService)
        {
            _feedbackService = feedbackService;
        }

        [HttpGet("GetAllFeedbacks")]
        public IEnumerable<Pending_Feedback> GetAllFeedbacks()
        {
            return _feedbackService.GetAllFeedbacks();
        }

        [HttpDelete("AddFeedback")]
        public IActionResult AddFeedback(Pending_Feedback pending_Feedback)
        {
            _feedbackService.AddFeedback(pending_Feedback);
            return Ok("Feedback added successfully");
        }

        [HttpPut("DeleteFeedback")]
        public IActionResult DeleteFeedback(int feedbackId)
        {
            _feedbackService.DeleteFeedBack(feedbackId);
            return Ok("Feedback Deleted successfully");
        }

        [HttpPut("UpdateFeedback")]
        public IActionResult UpdateFeedback(Pending_Feedback pending_Feedback)
        {
            _feedbackService.UpdateFeedback(pending_Feedback);
            return Ok("Feedback updated successfully");
        }

        [HttpGet("GetFeedbackById")]
        public Pending_Feedback GetFeedbackById(int feedbackId)
        {
            return _feedbackService.GetFeedbackById(feedbackId);
        }
    }
}
