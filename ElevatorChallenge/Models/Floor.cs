using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ElevatorChallenge.Models
{
    public class Floor
    {
        public int FloorId { get; set; }
        public int Passenger { get; set; }
        public int RequestedDirection { get; set;}
    }
}
