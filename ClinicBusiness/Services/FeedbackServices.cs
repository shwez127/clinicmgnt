using ClinicData.Repository;
using ClinicEntity.Models;
using System;
using System.Collections.Generic;
using System.Text;

namespace ClinicBusiness.Services
{
    public class FeedbackServices
    {
        IFeedbackRepository _feedback;
        public FeedbackServices(IFeedbackRepository feedback)
        {
            _feedback = feedback;
        }
        public void AddFeedback(Pending_Feedback pending_Feedback)
        {
            _feedback.AddFeedback(pending_Feedback);
        }
        public void DeleteFeedBack(int feedbackId)
        {
            _feedback.DeleteFeedback(feedbackId);
        }
        public void UpdateFeedback(Pending_Feedback pending_Feedback)
        {
            _feedback.UpdateFeedback(pending_Feedback);
        }
        public Pending_Feedback GetFeedbackById(int feedbackId)
        {
            return _feedback.GetFeedbackById(feedbackId);
        }
        public IEnumerable<Pending_Feedback> GetAllFeedbacks()
        {
            return _feedback.GetAllFeedbacks();
        }
    }
}
