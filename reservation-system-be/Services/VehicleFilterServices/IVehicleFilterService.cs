using reservation_system_be.Services.VehicleServices;

namespace reservation_system_be.Services.VehicleFilterServices
{
	public interface IVehicleFilterService
	{
		Task<List<VehicleResponse>> GetvailableVehiclesDetails(DateTime startDate, TimeOnly startTime, DateTime endDate, TimeOnly endTime);
	}
}
