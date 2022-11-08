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
            #region Adding feedback details to database
            _clinicDbContext.pending_feedbacks.Add(pending_Feedback);
            _clinicDbContext.SaveChanges();
            #endregion
        }

        public void DeleteFeedback(int feedbackId)
        {
            #region Deleting feedback details from database
            var feedback = _clinicDbContext.pending_feedbacks.Find(feedbackId);
            _clinicDbContext.pending_feedbacks.Remove(feedback);
            _clinicDbContext.SaveChanges();
            #endregion
        }

        public IEnumerable<Pending_Feedback> GetAllFeedbacks()
        {
            #region Show all existing feedbacks
            return _clinicDbContext.pending_feedbacks.ToList();
            #endregion
        }

        public Pending_Feedback GetFeedbackById(int feedbackId)
        {
            #region Search feedback details in database through feedbackID
            return _clinicDbContext.pending_feedbacks.Find(feedbackId);
            #endregion
        }

        public void UpdateFeedback(Pending_Feedback pending_Feedback)
        {
            #region Updating feedback details in database
            _clinicDbContext.Entry(pending_Feedback).State = EntityState.Modified;
            _clinicDbContext.SaveChanges();
            #endregion
        }
    }
}
