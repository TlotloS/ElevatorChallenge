using ElevatorChallenge.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ElevatorChallenge.Models
{
    public class ElevatorStatus
    {
        /// <summary>
        /// Indicates the current floor which the elevator is on
        /// </summary>
        public int CurrentFloor { get; set; }
        /// <summary>
        /// Indicates weather the elevator is currently in motion
        /// </summary>
        public bool InService { get; set; }
        /// <summary>
        /// The diretion which the elevator is currently going
        /// </summary>
        public ElevatorDirection Direction { get; set; }
    }
}
