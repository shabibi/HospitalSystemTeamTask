using HospitalSystemTeamTask.DTO_s;
using HospitalSystemTeamTask.Models;
using HospitalSystemTeamTask.Repositories;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using static Microsoft.EntityFrameworkCore.DbLoggerCategory;

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
        public IEnumerable<BookingOutputDTO> GetAllBooking(int pageNumber, int pageSize)
        {
            // Validate pagination parameters
            if (pageNumber <= 0 || pageSize <= 0)
                throw new ArgumentException("Page number and page size must be greater than zero.");

            // Get all bookings from the repository
            var appointments = _bookingRepo.GetAllBooking();

            // Map each booking to a BookingOutputDTO
            var bookingList = appointments.Select(appointment => new BookingOutputDTO
            {
                CID = appointment.CID,
                Date = appointment.Date,
                StartTime = appointment.StartTime,
                Staus = appointment.Staus,
                PID = appointment.PID,
                BookingDate = appointment.BookingDate
            });

            // Apply pagination using Skip and Take
            var pagedBookings = bookingList
                .Skip((pageNumber - 1) * pageSize) // Skip records for previous pages
                .Take(pageSize)                   // Take records for the current page
                .ToList();

            return pagedBookings;
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


        public IEnumerable<BookingInputDTO> GetAvailableAppointmentsBy( int? clinicId, int? departmentId)
        {
            // Retrieve all bookings from the repository
            var bookings = _bookingRepo.GetAllBooking();

            // Filter available bookings only
            var availableBookings = bookings.Where(b => !b.Staus);

            if (clinicId.HasValue)
            {
                availableBookings = availableBookings.Where(b => b.CID == clinicId.Value);
            }

            if (departmentId.HasValue)
            {
                // Assuming you have a way to filter by department, like a clinic-department mapping
                availableBookings = availableBookings.Where(b => _clinicService.GetClinicById(b.CID)?.DepID == departmentId.Value);
            }

            // Check if no available bookings were found
            if (!availableBookings.Any())
                throw new InvalidOperationException("No available appointments found for the given criteria.");

            // Map the filtered bookings to BookingOutputDTO
            var bookingOutput = availableBookings.Select(b => new BookingInputDTO
            {
                CID = b.CID,
                Date = b.Date,
                StartTime = b.StartTime
            });

            return bookingOutput;
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

        public IEnumerable<BookingOutputDTO> GetBookedAppointments(int ? patientId, int? clinicId, int? departmentId, DateTime ? date)
        {
            // Retrieve all bookings from the repository
            var bookings = _bookingRepo.GetAllBooking();

            // Filter only booked appointments (Staus == true)
            var bookedAppointments = bookings.Where(b => b.Staus == true);

            // Filter by clinicId if provided
            if (clinicId.HasValue)
            {
                bookedAppointments = bookedAppointments.Where(b => b.CID == clinicId.Value);
            }

            // Filter by departmentId if provided
            if (departmentId.HasValue)
            {
                bookedAppointments = bookedAppointments.Where(b => _clinicService.GetClinicById(b.CID)?.DepID == departmentId.Value);
            }

            // Filter by patientId if provided
            if (patientId.HasValue)
            {
                bookedAppointments = bookedAppointments.Where(b => b.PID == patientId.Value);
            }
            // Filter by date if provided
            if (date.HasValue)
            {
                bookedAppointments = bookedAppointments.Where(b => b.Date == date.Value);
            }


            // Check if no available bookings were found
            if (!bookedAppointments.Any())
                throw new InvalidOperationException("No booked appointments found for the given criteria");

            // Map the filtered bookings to BookingOutputDTO
            var bookingOutput = bookedAppointments.Select(b => new BookingOutputDTO
            {
                CID = b.CID,
                Date = b.Date,
                StartTime = b.StartTime,
                Staus = b.Staus,
                PID = b.PID,
                BookingDate = b.BookingDate
            });

            return bookingOutput;
        }

        public void CancelAppointment(BookingInputDTO bookingInputDTO)
        {
            // Retrieve the appointment based on clinic, date, and start time
            var appointment = _bookingRepo
                .GetBookingsByClinicAndDate(bookingInputDTO.CID, bookingInputDTO.Date)
                .FirstOrDefault(b => b.StartTime == bookingInputDTO.StartTime);

            // Check if the appointment exists and is currently booked
            if (appointment == null)
                throw new Exception("No appointment found for the provided details.");

            if (!appointment.Staus)
                throw new Exception("The appointment is not currently booked and cannot be canceled.");

            // Update the appointment to mark it as canceled
            appointment.Staus = false;
            appointment.BookingDate = null;
            appointment.PID = null;

            // Persist the updated appointment in the repository
            _bookingRepo.UpdateBooking(appointment);
        }
        public IEnumerable<Booking> GetBookingsByClinicAndDate(int clinicId, DateTime date)
        {
           return _bookingRepo.GetBookingsByClinicAndDate(clinicId, date);
        }
        public void UpdateBookedAppointment(BookingInputDTO previousAppointment, BookingInputDTO newAppointment, int patientId)
        {
            // Retrieve the existing (previous) appointment
            var previousBookedAppointment = _bookingRepo
                .GetBookingsByClinicAndDate(previousAppointment.CID, previousAppointment.Date)
                .FirstOrDefault(b => b.StartTime == previousAppointment.StartTime);

            // Validate if the previous appointment exists and is booked
            if (previousBookedAppointment == null)
                throw new Exception("No appointment found for the provided details.");

            if (!previousBookedAppointment.Staus)
                throw new Exception("The appointment is not currently booked and cannot be updated.");

            // Ensure the patient owns the appointment
            if (previousBookedAppointment.PID != patientId)
                throw new Exception("You cannot update an appointment booked by another patient.");

            // Retrieve the new appointment slot
            var newAppointmentSlot = _bookingRepo
                .GetBookingsByClinicAndDate(newAppointment.CID, newAppointment.Date)
                .FirstOrDefault(b => b.StartTime == newAppointment.StartTime);

            // Validate if the new appointment slot exists and is available
            if (newAppointmentSlot == null)
                throw new Exception("No appointment found for the new provided details.");

            if (newAppointmentSlot.Staus)
                throw new Exception("The new appointment slot is already booked.");

            // Update the previous appointment to be unbooked
            previousBookedAppointment.PID = null;
            previousBookedAppointment.BookingDate = null;
            previousBookedAppointment.Staus = false;

            _bookingRepo.UpdateBooking(previousBookedAppointment);

            // Update the new appointment slot to be booked by the patient
            newAppointmentSlot.Staus = true;
            newAppointmentSlot.PID = patientId;
            newAppointmentSlot.BookingDate = DateTime.Today;

            _bookingRepo.UpdateBooking(newAppointmentSlot);
        }

        public void DeleteAppointments(BookingInputDTO bookingInputDTO)
        {
            // Retrieve the appointment based on clinic, date, and start time
            var appointment = _bookingRepo
                .GetBookingsByClinicAndDate(bookingInputDTO.CID, bookingInputDTO.Date)
                .FirstOrDefault(b => b.StartTime == bookingInputDTO.StartTime);

            // Check if the appointment exists and is currently booked
            if (appointment == null)
                throw new Exception("No appointment found for the provided details.");

            if (appointment.Staus)
                throw new Exception("The appointment is currently booked and cannot be deleted.");

            _bookingRepo.DeleteBooking(appointment.BookingID);

        }

    }
}
