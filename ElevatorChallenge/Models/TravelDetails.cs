using ElevatorChallenge.Enums;

namespace ElevatorChallenge.Models
{
    /// <summary>
    /// Elevator travel details
    /// </summary>
    public class TravelDetails
    {
        public ElevatorDirection ElevatorDirection { get; set; }
        public IEnumerable<int> FloorToStopAtWithinTravelDirection { get; set; } = Enumerable.Empty<int>();
    }
}
