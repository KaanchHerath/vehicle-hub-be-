using reservation_system_be.Models;

namespace reservation_system_be.Services.FeedbackServices
{
    public interface IFeedbackService
    {
        Task<Feedback> AddFeedbacks(FeedbackRequest feedbackRequest);

        Task<List<Feedback>> GetAllFeedbacks();

        Task<List<Feedback>> GetFeedbacks(String type);

        Task<List<FeedbackResponse>> GetFeedbacksForVehicle(int vehicleid);
    }

}
