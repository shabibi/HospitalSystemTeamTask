using HospitalSystemTeamTask.Models;

namespace HospitalSystemTeamTask.Repositories
{
    public interface IBookingRepo
    {
        IEnumerable<Booking> GetAllBooking();
        void AddBooking(Booking booking);
        void UpdateBooking(Booking booking);
        Booking GetBookingById(int bookingId);
        IEnumerable<Booking> GetBookingsByClinicAndDate(int clinicId, DateTime date);
        IEnumerable<Booking> GetBookingsByPatientId(int PatientId);
    }
}
