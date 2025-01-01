using HospitalSystemTeamTask.Models;

namespace HospitalSystemTeamTask.Services
{
    public interface IBookingService
    {
        IEnumerable<Booking> GetAllBooking();
    }
}
