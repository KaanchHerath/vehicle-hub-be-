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
                 .Include(f => f.Reservation)
                 .ThenInclude(r => r.CustomerReservation)
                 .ThenInclude(cr => cr.Customer)
                 .Select(f => new FeedBackReportDTO
                 {
                     Id = f.Id,
                     Type = f.Type,
                     Content = f.Content,
                     RatingNo = f.RatingNo,
                     Feedback_Date = f.Feedback_Date,
                     Customername = f.Reservation.CustomerReservation.Customer.Name
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

