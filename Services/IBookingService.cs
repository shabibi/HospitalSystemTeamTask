using HospitalSystemTeamTask.DTO_s;
using HospitalSystemTeamTask.Models;

namespace HospitalSystemTeamTask.Services
{
    public interface IBookingService
    {
        IEnumerable<Booking> GetAllBooking();
        void AddBooking(BookingInputDTO input);
        Booking GetBookingById(int bookingId);
    }
}
