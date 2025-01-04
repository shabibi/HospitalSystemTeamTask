using HospitalSystemTeamTask.DTO_s;
using HospitalSystemTeamTask.Models;
using HospitalSystemTeamTask.Repositories;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
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

        public IEnumerable<Booking> ScheduledAppointments(int cid, DateTime appointmentDate)
        {
            // Validate that the appointment date is not in the past
            if (appointmentDate < DateTime.Today)
                throw new InvalidOperationException("Cannot schedule appointments for a past date.");

            // Retrieve the clinic from the service
            var clinic = _clinicService.GetClinicById(cid);

            // Validate clinic existence and activity status
            if (clinic == null || !clinic.IsActive)
                throw new InvalidOperationException("Invalid Clinic ID or the clinic is not active.");

            // Calculate total available minutes for scheduling
            var totalMinutes = (clinic.EndTime - clinic.StartTime).TotalMinutes;

            // Ensure the clinic has valid operating hours
            if (totalMinutes <= 0)
                throw new InvalidOperationException("Clinic operating hours are invalid.");

            // Retrieve existing bookings for the specified date
            var existingBookings = _bookingRepo.GetBookingsByClinicAndDate(cid, appointmentDate).ToList();

            // Ensure there are no existing schedules for this clinic on the same day
            if (existingBookings.Any())
                throw new InvalidOperationException("Clinic is already scheduled for this date.");

            // Generate appointment slots
            var appointmentSlots = new List<Booking>();
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
                    BookingDate = null, // Default to null for unbooked slots
                    Staus = false,     // Slot is initially unbooked
                    PID = null          // No patient assigned
                };

                appointmentSlots.Add(appointment);
            }

            // Add all generated slots to the repository in a batch
            foreach (var appointment in appointmentSlots)
            {
                _bookingRepo.AddBooking(appointment);
            }

            // Return all generated slots
            return appointmentSlots;
        }


        public void BookAppointment(BookingInputDTO input, int patientId)
        {
            // Fetch appointments for the clinic and date
            var bookedAppointments = _bookingRepo.GetBookingsByClinicAndDate(input.CID, input.Date);

            // Check if the patient already has a booking at the same clinic or time
            var patientBookings = _bookingRepo.GetBookingsByPatientId(patientId);

            foreach (var booking in patientBookings)
            {
                if ((booking.Date == input.Date && booking.StartTime == input.StartTime) || booking.CID == input.CID)
                {
                    throw new InvalidOperationException("You already have an appointment at this time or at this clinic.");
                }
            }

            // Check for time conflicts
            var conflictingAppointment = bookedAppointments
                .FirstOrDefault(b => b.StartTime == input.StartTime && b.Staus);

            if (conflictingAppointment != null)
            {
                throw new InvalidOperationException("The selected time slot is already booked.");
            }

            // Find an available appointment slot
            var availableAppointment = bookedAppointments
                .FirstOrDefault(b => b.StartTime == input.StartTime && !b.Staus);

            if (availableAppointment == null)
            {
                throw new InvalidOperationException("No available slot for the given time.");
            }

            // Book the appointment
            availableAppointment.Staus = true;
            availableAppointment.PID = patientId;
            availableAppointment.BookingDate = DateTime.Now;

            // Update the booking in the repository
            _bookingRepo.UpdateBooking(availableAppointment);
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
