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
        private readonly IClinicService _clinicService;
        public BookingService(IBookingRepo bookingRepo, IClinicService clinicService)
        {
            _bookingRepo = bookingRepo;
            _clinicService = clinicService;
        }
        public IEnumerable<Booking> GetAllBooking()
        {
            return _bookingRepo.GetAllBooking();
        }

        public void ScheduledAppointments(int cid , DateTime appointmentDate)
        {
            // Retrieve the clinic from the service
            var clinic = _clinicService.GetClinicById(cid);

            // Validate clinic existence and activity status
            if (clinic == null || !clinic.IsActive)
                throw new ArgumentException("Invalid Clinic ID or the clinic is not active.");

            // Calculate total available minutes for scheduling
            var totalMinutes = (clinic.EndTime - clinic.StartTime).TotalMinutes;

            // Ensure the clinic has valid operating hours
            if (totalMinutes <= 0)
                throw new ArgumentException("Clinic operating hours are invalid.");

            // Generate appointment slots
            for (int i = 0; i < totalMinutes; i += clinic.SlotDuration)
            {
                var startTime = clinic.StartTime.Add(TimeSpan.FromMinutes(i));
                var endTime = startTime.Add(TimeSpan.FromMinutes(clinic.SlotDuration));

                // Create a new appointment slot
                var appointment = new Booking
                {
                    CID = clinic.CID,
                    StartTime = startTime,
                    EndTime = endTime,
                    Date = appointmentDate,
                    BookingDate = DateTime.Now, // Default to current date/time
                    Staus = false, // Default status: not booked
                    PID = null // No patient assigned initially
                };

                // Add the appointment to the repository
                _bookingRepo.AddBooking(appointment);
            }
        }
    
    //public void AddBooking(BookingInputDTO input, int patientId)
    //    {
    //        var bookedAppointments = _bookingRepo.GetBookingByClinicID(input.CID);
    //        foreach(var booked in bookedAppointments)
    //        {
    //            if(booked.StartTime == input.)
    //        }
    //        var booking = new Booking
    //        {
    //            StartTime = input.StartTime,
    //            EndTime = input.EndTime,
          
    //            Staus = input.Staus,
    //            Date = input.Date,
    //            CID = input.CID,
    //            PID = input.PID,
    //            BookingDate = input.BookingDate
    //        };
    //        _bookingRepo.AddBooking(booking);
    //    }

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
        //    var existingBooking = _bookingRepo.GetBookingById(BookingID);



        //    // Map updated properties
        //    existingBooking.BookingDate = input.BookingDate;
        //    existingBooking.CID = input.CID;
        //    existingBooking.StartTime = input.StartTime;
        //    existingBooking.EndTime = input.EndTime;
        //    existingBooking.Staus = input.Staus;
        //    existingBooking.PID = input.PID;
        //    existingBooking.Date = input.Date;
            

        //    // Persist changes
        //    _bookingRepo.UpdateBooking(existingBooking);
        }



    }
}
