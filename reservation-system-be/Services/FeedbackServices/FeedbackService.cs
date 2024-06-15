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

            if (feedbackRequest == null)
            {
                throw new ArgumentNullException(nameof(feedbackRequest));
            }

            try
            {
                var serviceFeedback = new Feedback
                {
                    Designation = feedbackRequest.Designation,
                    Type = "service",
                    Vehicle_Review = feedbackRequest.Vehicle_Review,
                    Service_Review = feedbackRequest.Service_Review,
                    RatingNo = feedbackRequest.RatingNo,
                    Feedback_Date = DateTime.Now,
                    Feedback_Time = DateTime.Now,
                    CustomerReservationId = feedbackRequest.CustomerReservationId,
                    CustomerReservation = null,
                };

                if (feedbackRequest.CustomerReservationId != null)
                {
                    var customerReservation = await _context.CustomerReservations.FindAsync(feedbackRequest.CustomerReservationId);

                    if (customerReservation == null)
                    {
                        throw new ArgumentException("Invalid ReservationId");
                    }

                    serviceFeedback.CustomerReservationId = feedbackRequest.CustomerReservationId;
                    serviceFeedback.CustomerReservation = customerReservation;

                }

                _context.Feedbacks.AddRange(serviceFeedback);
                await _context.SaveChangesAsync();

                return new List<Feedback> { serviceFeedback };
            }
            catch (Exception ex)
            {
                Console.WriteLine("An error occurred while adding feedbacks:");
                Console.WriteLine(ex.ToString());

                // Print inner exception details if available
                if (ex.InnerException != null)
                {
                    Console.WriteLine("Inner Exception:");
                    Console.WriteLine(ex.InnerException.ToString());
                }

                throw new Exception("Errors adding feedbacks", ex);
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

        public async Task<List<FeedbackResponse>> GetFeedbacksForVehicle(int vehicleId)
        {
            try
            {
                var feedbackResponses = await _context.Feedbacks
                    .Join(_context.CustomerReservations,
                          feedback => feedback.CustomerReservationId,
                          customerReservation => customerReservation.Id,
                          (feedback, customerReservation) => new { feedback, customerReservation })
                    .Join(_context.Customers,
                           frc => frc.customerReservation.CustomerId,
                           customer => customer.Id,
                           (frc, customer) => new { frc.feedback, frc.customerReservation, customer })
                    .Where(frc => frc.customerReservation.VehicleId == vehicleId)
                    .Select(frc => new FeedbackResponse
                    {
                        Feedback = frc.feedback,
                        CustomerName = frc.customer.Name
                    })
                    .ToListAsync();

                return feedbackResponses;
            }
            catch (Exception ex)
            {
                throw new Exception("Error getting feedbacks for vehicle.", ex);
            }
        }

    }

    public class FeedbackResponse
    {
        public Feedback Feedback { get; set; }
        public string CustomerName { get; set; }
    }

    public class FeedbackRequest
    {
        public string Designation { get; set; } = string.Empty;
        public int RatingNo { get; set; }
        public string Service_Review { get; set; } = string.Empty;
        public string Vehicle_Review { get; set; } = string.Empty;
        public int CustomerReservationId { get; set; }

    }
}
