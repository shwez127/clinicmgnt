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
        //creating a constructor for accessing FeedbackService class to access Business Layer
        FeedbackServices _feedbackService;

        //Constructor
        public FeedbackController(FeedbackServices feedbackService)
        {
            _feedbackService = feedbackService;
        }

        [HttpGet("GetAllFeedbacks")]
        public IEnumerable<Pending_Feedback> GetAllFeedbacks()
        {
            #region Listing all feedbacks
            return _feedbackService.GetAllFeedbacks();
            #endregion
        }

        [HttpPost("AddFeedback")]
        public IActionResult AddFeedback(Pending_Feedback pending_Feedback)
        {
            #region Feedback adding action
            _feedbackService.AddFeedback(pending_Feedback);
            return Ok("Feedback added successfully");
            #endregion
        }

        [HttpPut("DeleteFeedback")]
        public IActionResult DeleteFeedback(int feedbackId)
        {
            #region Feedback deletion action
            _feedbackService.DeleteFeedBack(feedbackId);
            return Ok("Feedback Deleted successfully");
            #endregion
        }

        [HttpPut("UpdateFeedback")]
        public IActionResult UpdateFeedback(Pending_Feedback pending_Feedback)
        {
            #region Feedback updation action
            _feedbackService.UpdateFeedback(pending_Feedback);
            return Ok("Feedback updated successfully");
            #endregion
        }

        [HttpGet("GetFeedbackById")]
        public Pending_Feedback GetFeedbackById(int feedbackId)
        {
            #region Search feedback by feedback ID
            return _feedbackService.GetFeedbackById(feedbackId);
            #endregion
        }
    }
}
