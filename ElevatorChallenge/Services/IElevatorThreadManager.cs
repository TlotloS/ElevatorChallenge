namespace ElevatorChallenge.Services
{
    public interface IElevatorThreadManager
    {
        void StartElevatorThreadsAsync(IEnumerable<IElevator> elevators);
    }
}
