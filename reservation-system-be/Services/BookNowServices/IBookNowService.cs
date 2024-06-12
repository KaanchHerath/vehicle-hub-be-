using reservation_system_be.DTOs;

namespace reservation_system_be.Services.CustomerVehicleServices
{
    public interface IBookNowService
    {
        Task<IEnumerable<BookNowDto>> GetAllCards();
    }
}
