using System;
using reservation_system_be.Data;
using reservation_system_be.Models;
using Microsoft.EntityFrameworkCore;
using reservation_system_be.DTOs;

namespace reservation_system_be.Services.FeedbackReportService
{
    public class FeedbackReportService : IFeedbackReportService
    {
        private readonly DataContext _context;

        public FeedbackReportService(DataContext context)
        {
            _context = context;
        }

        public async Task<List<FeedBackReportDTO>> GetAllFeedBacks()
        {
            try
            {
                return await _context.Feedbacks
                 .Include(f => f.CustomerReservation)
                 .ThenInclude(cr => cr.Customer)
                 .Select(f => new FeedBackReportDTO
                 {
                     id = f.Id,
                     vehicle = f.Type,
                     content = f.Service_Review,
                     rating = f.RatingNo,
                     date = f.Feedback_Date,
                     customer = f.CustomerReservation.Customer.Name
                 })
                 .ToListAsync(); ;
            }
            catch (Exception ex)
            {
                throw new Exception("Error getting feedbacks", ex);
            }
        }
    }
}
