using ElevatorChallenge.Enums;
namespace ElevatorChallenge.Models
{
    public class ElevatorStatus
    {
        public int ElevatorNumber { get; set; }
        /// <summary>
        /// Indicates the current floor which the elevator is on
        /// </summary>
        public int CurrentFloor { get; set; }
        /// <summary>
        /// The diretion which the elevator is currently going
        /// </summary>
        public ElevatorDirection Direction { get; set; }
        /// <summary>
        /// Count of people inside the elevator
        /// </summary>
        public int Load { get; set; }
        /// <summary>
        /// Elevator name
        /// </summary>
        public string Name { get; set; }
    }
}
