using System;
using System.Collections.Generic;
using System.Linq;
using System.ServiceModel;
using Flights.Model;
using Flights.ThirdPartyService.BookingService;


namespace Flights.ThirdPartyService
{
    //In a real application I would log the exceptions somewhere for example a file or Db.
    //I would send an email if necessary
    public class BookingGateway : IBookingGateway
    {
        public FlightsResponse GetFlights(string origin, string destination, DateTime departureDate, DateTime returnDate)
        {
            var availabilityRequest = CreateAvailabilityRequest(origin, destination, departureDate, returnDate);

            var bookingClient = new BookingClient();

            AvailabilityResponse availabilityResponse;
            try
            {
                availabilityResponse = bookingClient.GetAvailability(availabilityRequest);
            }

            catch (FaultException fe)
            {
                Console.WriteLine("FaultException Log: {0}", fe.GetType());
                throw;
            }
            catch (CommunicationException ce)
            {
                Console.WriteLine("CommunicationException Log: {0}", ce.GetType());
                throw;
            }
            catch (TimeoutException te)
            {
                Console.WriteLine("TimeoutException Log: {0}", te.GetType());
                throw;
            }
            finally
            {
                bookingClient.Abort();
            }

            List<Model.Journey> inBoundJourneys =
                availabilityResponse.InboundJourneys.Select(inBoundJourney => CreateJourney(inBoundJourney)).ToList();

            List<Model.Journey> outBoundJourneys =
                availabilityResponse.OutoundJourneys.Select(outBoundJourney => CreateJourney(outBoundJourney)).ToList();

            var fligtsResponse = new FlightsResponse
            {
                InboundJourneys = inBoundJourneys,
                OutboundJourneys = outBoundJourneys
            };

            bookingClient.Close();

            return fligtsResponse;

        }

        private AvailabilityRequest CreateAvailabilityRequest(string origin, string destination, DateTime departureDate,
            DateTime returnDate)
        {
            var availabilityRequest = new AvailabilityRequest
            {
                Origin = origin,
                DepartureDate = departureDate,
                Destination = destination,
                ReturnDate = returnDate
            };
            return availabilityRequest;
        }

        private Model.Journey CreateJourney(BookingService.Journey bookingJourney)
        {
            var journey = new Model.Journey
            {
                ArrivalStation = bookingJourney.ArrivalStation.Name,
                ArrivalTime = bookingJourney.ArrivalTime,
                Carrier = bookingJourney.Carrier,
                DepartureStation = bookingJourney.DepartureStation.Name,
                DepartureTime = bookingJourney.DepartureTime,
                Number= bookingJourney.Number
            };
            return journey;
        }
    }
}

