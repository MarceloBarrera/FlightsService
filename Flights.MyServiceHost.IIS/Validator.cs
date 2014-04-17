using System;
using System.Net;
using System.ServiceModel.Web;

namespace Flights.MyServiceHost.IIS
{
    //this class at the moment is doing a simple validation but could be more complex,
    //for example validate those city codes with 3 letters, etc.
    public class Validator
    {
        public void ValidateServiceParameters(string origin, string destination, DateTime departureDate, DateTime returnDate)
        {
            DateTime tempDate;
            DateTime tempDate2;
            bool isValid = DateTime.TryParse(departureDate.ToString(), out tempDate)
                      && DateTime.TryParse(returnDate.ToString(), out tempDate2)
                      && !string.IsNullOrEmpty(origin) && !string.IsNullOrEmpty(destination);
            if(!isValid)
                throw new WebFaultException<string>("Invalid parameters pass to the service", HttpStatusCode.BadRequest);
            
        }
    }
}