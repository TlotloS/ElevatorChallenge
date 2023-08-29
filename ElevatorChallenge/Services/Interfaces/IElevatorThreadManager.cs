namespace ElevatorChallenge.Services.Interfaces
{
    public interface IElevatorThreadManager
    {
        void StartElevatorThreadsAsync(IEnumerable<IElevator> elevators);
    }
}
