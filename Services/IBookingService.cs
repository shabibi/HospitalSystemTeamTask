using HospitalSystemTeamTask.DTO_s;
using HospitalSystemTeamTask.Models;
using System.Collections.Generic;

namespace HospitalSystemTeamTask.Services
{
    public interface IBookingService
    {
        IEnumerable<BookingOutputDTO> GetAllBooking(int pageNumber, int pageSize);
        Booking GetBookingById(int bookingId);
        IEnumerable<Booking> ScheduledAppointments(int cid, DateTime appointmentDate);
       void BookAppointment(BookingInputDTO input, int patientId);
        IEnumerable<BookingInputDTO> GetAvailableAppointmentsBy(int? clinicId, int? departmentId);
        IEnumerable<BookingOutputDTO> GetBookedAppointments(int? patientId, int? clinicId, int? departmentId, DateTime? date);
        void CancelAppointment(BookingInputDTO bookingInputDTO);
        IEnumerable<Booking> GetBookingsByClinicAndDate(int clinicId, DateTime date);
    }
}
