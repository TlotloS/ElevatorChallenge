namespace ElevatorChallenge.Models
{
    public class PassengerRequest
    {
        /// <summary>
        /// The floor which the request is coming from
        /// </summary>
        public int OriginFloorLevel { get; set; }
        /// <summary>
        /// The floor which the passenger(s) are going to
        /// </summary>
        public int DestinationFloorLevel { get; set; }
        public int PassengerCount { get; set; }
    }
}
