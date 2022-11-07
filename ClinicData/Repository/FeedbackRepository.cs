using ClinicData.Data;
using ClinicEntity.Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ClinicData.Repository
{
    public class FeedbackRepository: IFeedbackRepository
    {
        ClinicDbContext _clinicDbContext;
        public FeedbackRepository(ClinicDbContext clinicDbContext)
        {
            _clinicDbContext = clinicDbContext;
        }

        public void AddFeedback(Pending_Feedback pending_Feedback)
        {
            _clinicDbContext.pending_feedbacks.Add(pending_Feedback);
            _clinicDbContext.SaveChanges();
        }

        public void DeleteFeedback(int feedbackId)
        {
            var feedback = _clinicDbContext.pending_feedbacks.Find(feedbackId);
            _clinicDbContext.pending_feedbacks.Remove(feedback);
            _clinicDbContext.SaveChanges();
        }

        public IEnumerable<Pending_Feedback> GetAllFeedbacks()
        {
            return _clinicDbContext.pending_feedbacks.ToList();
        }

        public Pending_Feedback GetFeedbackById(int feedbackId)
        {
            return _clinicDbContext.pending_feedbacks.Find(feedbackId);
        }

        public void UpdateFeedback(Pending_Feedback pending_Feedback)
        {
            _clinicDbContext.Entry(pending_Feedback).State = EntityState.Modified;
            _clinicDbContext.SaveChanges();
        }
    }
}
