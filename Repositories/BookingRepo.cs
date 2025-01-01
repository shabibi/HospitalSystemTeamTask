using HospitalSystemTeamTask.Models;
using HospitalSystemTeamTask;
using HospitalSystemTeamTask.Repositories;
using Microsoft.EntityFrameworkCore;
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





        } }
       