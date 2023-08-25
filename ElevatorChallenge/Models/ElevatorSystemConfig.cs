namespace ElevatorChallenge.Models
{
    public class ElevatorSystemConfig
    {
        public int FloorsCount { get; set; } = 1;
        public int ElevatorsCount { get; set; } = 2;
        public int MaxWeight { get; set; } = int.MaxValue;
        public ElevatorSystemConfig(int floorsCount, int elevatorsCount, int maxWeight)
        {
            FloorsCount = floorsCount;
            ElevatorsCount = elevatorsCount;
            MaxWeight = maxWeight;
        }
    }
}
