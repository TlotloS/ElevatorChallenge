using ElevatorChallenge.Enums;

namespace ElevatorChallenge.Models
{
    public class ElevatorTravelDetails
    {
        /// <summary>
        /// The current direction of the elevator
        /// </summary>
        public ElevatorDirection Direction { get; set; }
        /// <summary>
        /// list of integers indication which floor the elevator should stop 
        /// </summary>
        public IEnumerable<int> FloorsToStop { get; set; }
    }
}
