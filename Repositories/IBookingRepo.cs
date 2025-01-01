using HospitalSystemTeamTask.Models;

namespace HospitalSystemTeamTask.Repositories
{
    public interface IBookingRepo
    {
        IEnumerable<Booking> GetAllBooking();

    }
}
