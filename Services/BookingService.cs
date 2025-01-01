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




    }
}
