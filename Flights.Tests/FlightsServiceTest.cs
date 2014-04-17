using System;
using System.Net;
using System.ServiceModel.Web;
using System.Web.Script.Serialization;
using Flights.MyServiceHost.IIS;
using Flights.ThirdPartyService;
using Moq;
using NUnit.Framework;
using System.IO;

namespace Flights.Tests
{
    

    [TestFixture]
    class FlightsServiceTest
    {
        //before running this tests the service need to be running on IIS with the
        // following base address or change it accordingly please.
        private const string BaseAddress = "http://localhost:62245/FlightsService.svc";

        [Test]
        public void Get_ShouldCallBookingGatewayServiceOnce()
        {
            var fakeGateway = new Mock<IBookingGateway>();
            var sut = new FlightsService(fakeGateway.Object);

            sut.Get("SYD", "MEL", new DateTime(2013, 10, 20), new DateTime(2013, 10, 23));

            fakeGateway.Verify(a => a.GetFlights(It.IsAny<string>(), It.IsAny<string>(), It.IsAny<DateTime>(), It.IsAny<DateTime>())
                            ,Times.Once);
        }

        [Test]
        public void Get_ShouldReturnResponseWithInAndOutBoundsJourneys()
        {
            var url = BaseAddress + "/Search?origin=LON&destination=NYC&departureDate=2014-10-20&returnDate=2014-10-23";

            var request = (HttpWebRequest) WebRequest.Create(url);
            request.Method = "GET";
            request.ContentType = "application/json";
            var response =(HttpWebResponse) request.GetResponse();
            string jSonString;
            using (var sr= new StreamReader(response.GetResponseStream()))
            {
                jSonString = sr.ReadToEnd();
            }
            var jsSerializer = new JavaScriptSerializer();
            var jsonResponse = jsSerializer.Deserialize<dynamic>(jSonString);
            
            Assert.That(jsonResponse["InboundJourneys"], Is.Not.Empty);
            Assert.That(jsonResponse["OutboundJourneys"], Is.Not.Empty);
        }

        [Test]
        public void GEt_ShouldThrowWebFaultExceptionWhenBookingGatewayServiceThrowsExcpetion()
        {
            var fakeGateway = new Mock<IBookingGateway>();
            
            fakeGateway.Setup(b => b.GetFlights(It.IsAny<string>(), It.IsAny<string>(), It.IsAny<DateTime>(), It.IsAny<DateTime>()))
                    .Throws<Exception>();

            var sut = new FlightsService(fakeGateway.Object);
            
            Assert.Throws<WebFaultException<string>>(()=>sut.Get(It.IsAny<string>(), It.IsAny<string>(), It.IsAny<DateTime>(), It.IsAny<DateTime>()));

        }

        [Test]
        public void GEt_ShouldThrowWebFaultExceptionWhenInvalidParameters()
        {
            var fakeGateway = new Mock<IBookingGateway>();
            
            var sut = new FlightsService(fakeGateway.Object);

            Assert.Throws<WebFaultException<string>>(() => sut.Get("","",DateTime.MinValue,DateTime.MinValue));

        }

    }
}
