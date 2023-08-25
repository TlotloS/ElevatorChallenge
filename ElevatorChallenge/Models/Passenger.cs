using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ElevatorChallenge.Models
{
    public class Passenger
    {
        public int OriginFloor { get; }
        public int DestinationFloor { get; }
        // creational pattern
        public Passenger(int originFloor, int destinationFloor)
        {
            OriginFloor = originFloor;
            DestinationFloor = destinationFloor;
        }
    }
}
