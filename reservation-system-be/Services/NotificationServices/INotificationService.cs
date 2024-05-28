using reservation_system_be.Models;

namespace reservation_system_be.Services.FeedbackServices
{
	public interface INotificationService
	{
		Task<List<Notification>> GetAllNotifications(int uid);
	}

}
