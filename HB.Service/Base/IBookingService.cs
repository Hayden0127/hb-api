using HB.Model;

namespace HB.Service
{
    public interface IBookingService
    {
        Task<BookingResponseModel> CreateNewBookingAsync(BookingRequestModel model);
        BookingDisplayListResponseModel GetAllBookingByUser(int id);
    }
}