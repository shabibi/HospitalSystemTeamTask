using HospitalSystemTeamTask.DTO_s;
using HospitalSystemTeamTask.Models;

namespace HospitalSystemTeamTask.Services
{
    public interface IBookingService
    {
        IEnumerable<BookingOutputDTO> GetAllBooking(int pageNumber, int pageSize);
        Booking GetBookingById(int bookingId);
        IEnumerable<Booking> ScheduledAppointments(int cid, DateTime appointmentDate);
       void BookAppointment(BookingInputDTO input, int patientId);
    }
}
