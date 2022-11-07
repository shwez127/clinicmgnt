using ClinicEntity.Models;
using System;
using System.Collections.Generic;
using System.Text;

namespace ClinicData.Repository
{
    public interface IFeedbackRepository
    {
        void AddFeedback(Pending_Feedback pending_Feedback);
        void UpdateFeedback(Pending_Feedback pending_Feedback);
        void DeleteFeedback(int feedbackId);
        Pending_Feedback GetFeedbackById(int feedbackId);
        IEnumerable<Pending_Feedback> GetAllFeedbacks();
    }
}
