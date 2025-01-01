using HospitalSystemTeamTask.DTO_s;
using HospitalSystemTeamTask.Models;
using HospitalSystemTeamTask.Repositories;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace HospitalSystemTeamTask.Services
{
    public class BookingService : IBookingService
    {
        private readonly IBookingRepo _bookingRepo;
        private readonly IConfiguration _configuration;

        public BookingService(IBookingRepo bookingRepo, IConfiguration configuration)
        {
            _bookingRepo = bookingRepo;
            _configuration = configuration;
        }
        public IEnumerable<Booking> GetAllBooking()
        {
            return _bookingRepo.GetAllBooking();
        }

        public void AddBooking(BookingInputDTO input)
        {
            if (input == null)
            {
                throw new ArgumentException("Booking details are required.");
            }

            var booking = new Booking
            {
                StartTime = input.StartTime,
                EndTime = input.EndTime,
          
                Staus = input.Staus,
                Date = input.Date,
                CID = input.CID,
                PID = input.PID,
                BookingDate = input.BookingDate
            };
            _bookingRepo.AddBooking(booking);
        }

        public Booking GetBookingById(int bookingId)
        {
            // Validate input
            if (bookingId <= 0)
            {
                throw new ArgumentException("Invalid booking ID. booking ID must be greater than 0.");
            }

            
            var booking = _bookingRepo.GetBookingById(bookingId);

           
            if (booking == null)
            {
                throw new KeyNotFoundException($"booking with ID {bookingId} not found.");
            }

            return booking;
        }

        public void UpdateBookingDetails(int BookingID, BookingInputDTO input)
        {
            var existingBooking = _bookingRepo.GetBookingById(BookingID);



            // Map updated properties
            existingBooking.BookingDate = input.BookingDate;
            existingBooking.CID = input.CID;
            existingBooking.StartTime = input.StartTime;
            existingBooking.EndTime = input.EndTime;
            existingBooking.Staus = input.Staus;
            existingBooking.PID = input.PID;
            existingBooking.Date = input.Date;
            

            // Persist changes
            _bookingRepo.UpdateBooking(existingBooking);
        }



    }
}
