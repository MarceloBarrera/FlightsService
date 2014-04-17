using System.Collections.Generic;

namespace Flights.Model
{
    public class FlightsResponse
    {
        public IEnumerable<Journey> OutboundJourneys;
        public IEnumerable<Journey> InboundJourneys;
    }
}
