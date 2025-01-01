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

        //public void UpdateBookingDetails(int BookingID, BookingInputDTO input)
        //{
        //    var existingClinic = _bookingRepo.GetBookingById(bookingID);
        //    TimeSpan totalDuration = input.EndTime - input.StartTime;
        //    if (input.Capacity <= 0)
        //    {
        //        throw new ArgumentException("Capacity must be greater than 0.");
        //    }

        //    int slotTime = (int)(totalDuration.TotalMinutes / input.Capacity);

        //    // Map updated properties
        //    existingClinic.ClincName = input.ClincName;
        //    existingClinic.Capacity = input.Capacity;
        //    existingClinic.StartTime = input.StartTime;
        //    existingClinic.EndTime = input.EndTime;
        //    existingClinic.SlotDuration = slotTime;
        //    existingClinic.Cost = input.Cost;
        //    existingClinic.IsActive = input.IsActive;
        //    existingClinic.DepID = input.DepID;
        //    existingClinic.BID = input.BID;
        //    existingClinic.AssignDoctor = input.AssignDoctor;

        //    // Persist changes
        //    _clinicRepo.UpdateClinic(existingClinic);
        //}



    }
}
