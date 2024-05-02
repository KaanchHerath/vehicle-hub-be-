using System;
using reservation_system_be.DTOs;

namespace reservation_system_be.Services.FeedbackReportService
{
	public interface IFeedbackReportService
	{
		public Task<List<FeedBackReportDTO>> GetAllFeedBacks();
	}
}

