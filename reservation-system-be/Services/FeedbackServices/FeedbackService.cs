using Microsoft.EntityFrameworkCore;
using reservation_system_be.Data;
using reservation_system_be.Models;

namespace reservation_system_be.Services.FeedbackServices
{
    public class FeedbackService : IFeedbackService
    {
        private readonly DataContext _context;

        public FeedbackService(DataContext context)
        {
            _context = context;
        }

        public async Task<List<Feedback>> AddFeedbacks(FeedbackRequest feedbackRequest)
        {

            //if (feedbackRequest == null)
            //{
            //throw new ArgumentNullException(nameof(feedbackRequest));
            //}

            try
            {
                var serviceFeedback = new Feedback
                {
                    Designation = feedbackRequest.Designation,
                    Type = "service",
                    Content = feedbackRequest.Service_Review,
                    RatingNo = feedbackRequest.RatingNo,
                    Feedback_Date = DateTime.Now,
                    Feedback_Time = DateTime.Now,
                    ReservationId = feedbackRequest.ReservationId,
                    Reservation = null,
                };

                var vehicleFeedback = new Feedback
                {
                    Designation = feedbackRequest.Designation,
                    Type = "vehicle",
                    Content = feedbackRequest.Vehicle_Review,
                    RatingNo = feedbackRequest.RatingNo,
                    Feedback_Date = DateTime.Now,
                    Feedback_Time = DateTime.Now,
                    ReservationId = feedbackRequest.ReservationId,
                    Reservation = null,
                };

                /*This code block is commented out to avoid affecting the functionality of the application until the Reservation Model is populated with data*/
                //if (feedbackRequest.ReservationId != null)
                //{
                //var reservation = await _context.Reservations.FindAsync(feedbackRequest.ReservationId);

                //if (reservation == null)
                //{
                //throw new ArgumentException("Invalid ReservationId");
                //}

                //serviceFeedback.ReservationId = feedbackRequest.ReservationId;
                //serviceFeedback.Reservation = reservation;

                //vehicleFeedback.ReservationId = feedbackRequest.ReservationId;
                //vehicleFeedback.Reservation = reservation;

                //}


                //_context.Feedbacks.AddRange(serviceFeedback, vehicleFeedback);
                //await _context.SaveChangesAsync();

                return new List<Feedback> { serviceFeedback, vehicleFeedback };
            }
            catch (Exception ex)
            {
                throw new Exception("Error adding feedbacks", ex);
            }
        }

        public async Task<List<Feedback>> GetAllFeedbacks()
        {
            try
            {
                return await _context.Feedbacks.ToListAsync();
            }
            catch (Exception ex)
            {
                throw new Exception("Error getting feedbacks", ex);
            }
        }

        public async Task<List<Feedback>> GetFeedbacks(string type)
        {
            try
            {
                var feedbacks = await _context.Feedbacks
                    .Where(f => f.Type == type)
                    .ToListAsync();

                return feedbacks;
            }
            catch (Exception ex)
            {
                throw new Exception("Error getting feedbacks", ex);
            }
        }
    }

    public class FeedbackRequest
    {
        public string Designation { get; set; } = string.Empty;
        public int RatingNo { get; set; }
        public string Service_Review { get; set; } = string.Empty;
        public string Vehicle_Review { get; set; } = string.Empty;

        public int ReservationId { get; set; }

    }
}
