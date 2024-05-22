using System;

namespace BusStation
{
    public class BusRoute
    {
        public string BusNumber { get; set; }
        public string DeparturePoint { get; set; }
        public string DestinationPoint { get; set; }
        public DateTime DepartureTime { get; set; } // Изменено на DateTime
        public DateTime ArrivalTime { get; set; } // Изменено на DateTime
        public decimal TicketPrice { get; set; }
    }
}