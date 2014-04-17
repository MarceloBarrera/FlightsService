using System;

namespace Flights.Model
{
    public class Journey
    {
        public string ArrivalStation { get; set; }
        public DateTime ArrivalTime { get; set; }
        public string DepartureStation { get; set; }
        public DateTime DepartureTime { get; set; }
        public string Carrier { get; set; }
        public string Number { get; set; }
    }
}
