using HospitalSystemTeamTask.Models;
using HospitalSystemTeamTask;
using HospitalSystemTeamTask.Repositories;
using Microsoft.EntityFrameworkCore;
using static System.Runtime.InteropServices.JavaScript.JSType;
namespace HospitalSystemTeamTask.Repositories
{
    public class BookingRepo : IBookingRepo
    {
        private readonly ApplicationDbContext _context;

        public BookingRepo(ApplicationDbContext context)
        {
            _context = context;
        }

        public IEnumerable<Booking> GetAllBooking()
        {
            try
            {
                return _context.Bookings.ToList();
            }
            catch (Exception ex)
            {
                throw new InvalidOperationException($"Database error: {ex.Message}");
            }
        }

        public void AddBooking(Booking booking)
        {
            try
            {
                _context.Bookings.Add(booking);
                _context.SaveChanges();
            }
            catch (Exception ex)
            {
                throw new InvalidOperationException($"Database error: {ex.Message}");
            }
        }

        public void UpdateBooking(Booking booking)
        {
            _context.Bookings.Update(booking);
            _context.SaveChanges();
        }
        public Booking GetBookingById(int bookingId)
        {
            try
            {
                return _context.Bookings.FirstOrDefault(u => u.BookingID == bookingId);
            }
            catch (Exception ex)
            {
                throw new InvalidOperationException($"Database error: {ex.Message}");
            }
        }

        public IEnumerable<Booking> GetBookingsByClinicAndDate(int clinicId, DateTime date)
        {
            return _context.Bookings
                .Where(b => b.CID == clinicId && b.Date.Date == date.Date)
                .ToList();
        }

        public IEnumerable<Booking> GetBookingsByPatientId(int PatientId)
        {
            try
            {
                return _context.Bookings
                .Where(b => b.PID == PatientId)
                .ToList();
            }
            catch (Exception ex)
            {
                throw new InvalidOperationException($"Database error: {ex.Message}");
            }
        }

        public void DeleteBooking(int bookingId)
        {
            var booking = GetBookingById(bookingId);
            _context.Bookings.Remove(booking);
            _context.SaveChanges();
        }

    }
}
       