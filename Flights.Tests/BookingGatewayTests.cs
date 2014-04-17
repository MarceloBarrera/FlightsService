using System;
using Flights.ThirdPartyService;
using NUnit.Framework;

namespace Flights.Tests
{
    [TestFixture]
    public class BookingGatewayTests
    {
        // check the Gateway works correctly with the external component that it "wraps".
        [Test]
        public void GetFlights_ReturnFlightsJourneys()
        {
            var sut = new BookingGateway();
            
            var flights = sut.GetFlights("SYD", "MEL", new DateTime(2013, 10, 20), new DateTime(2013, 10, 23));
            
            Assert.That(flights.InboundJourneys, Is.Not.Empty);
            Assert.That(flights.OutboundJourneys, Is.Not.Empty);
        }
    }
}
