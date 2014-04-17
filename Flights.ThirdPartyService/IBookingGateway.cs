using System;
using Flights.Model;

namespace Flights.ThirdPartyService
{
    public interface IBookingGateway
    {
        FlightsResponse GetFlights(string origin, string destination, DateTime departureDate, DateTime returnDate);
    }
}