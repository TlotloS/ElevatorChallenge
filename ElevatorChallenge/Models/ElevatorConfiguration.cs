namespace ElevatorChallenge.Models
{
    public class ElevatorConfiguration
    {
        public int TotalElevators { get; set; }
        public int TotalFloors { get; set; }
        public int ElevatorMaximumWeight { get; set; }
        public DelayConfiguration DelayInSeconds { get; set; }
    }

    public class DelayConfiguration
    {
        /// <summary>
        /// Delay to simulate to action in seconds
        /// </summary>
        public int HandlingPassengers { get; set; } = 5;
        /// <summary>
        /// Delay to simulate to action in seconds
        /// </summary>
        public int MovingToNextLevel { get; set; } = 1;
    }
}
