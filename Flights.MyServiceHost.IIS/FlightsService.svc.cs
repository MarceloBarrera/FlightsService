using System;
using System.Net;
using System.ServiceModel;
using System.ServiceModel.Activation;
using System.ServiceModel.Web;
using Flights.Model;
using Flights.ThirdPartyService;

namespace Flights.MyServiceHost.IIS
{
    [ServiceContract(Namespace = "")]
    [AspNetCompatibilityRequirements(RequirementsMode = AspNetCompatibilityRequirementsMode.Allowed)]
    public class FlightsService
    {
        private readonly IBookingGateway _bookingGateway;
        readonly Validator _validator = new Validator();
        public FlightsService()
        {
            _bookingGateway = new BookingGateway();
        }

        public FlightsService(IBookingGateway bookingGateway)
        {
            _bookingGateway = bookingGateway;
        }

        [WebGet(UriTemplate ="/Search?origin={origin}&destination={destination}&departureDate={departureDate}&returnDate={returnDate}")]
        public FlightsResponse Get(string origin, string destination, DateTime departureDate, DateTime returnDate)
        {
            _validator.ValidateServiceParameters(origin, destination, departureDate, returnDate);
            try
            {
                return _bookingGateway.GetFlights(origin, destination, departureDate, returnDate);
            }
            catch (Exception e)
            {
                Console.WriteLine("Log Exception: {0}", e.GetType());//on a real app I would log the error somewhere
                throw new WebFaultException<string>("Could you try again in few moments please?.",HttpStatusCode.InternalServerError);
            }
        }

       
    }
}
