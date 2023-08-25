namespace ElevatorChallenge.Services
{
    public interface IElevatorThreadManager
    {
        Task StartElevatorThreadsAsync(List<IElevator> elevators);
    }
}
