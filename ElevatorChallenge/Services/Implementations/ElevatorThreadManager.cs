using ElevatorChallenge.Services.Interfaces;

public class ElevatorThreadManager : IElevatorThreadManager
{
    private List<Task> _elevatorTasks = new List<Task>();
    public ElevatorThreadManager()
    {
    }

    public void StartElevatorThreadsAsync(IEnumerable<IElevator> elevators)
    {
        // Start each elevator in its own thread
        foreach (var elevator in elevators)
        {
            StartElevatorThread(elevator);
        }
    }

    private void StartElevatorThread(IElevator elevator)
    {
        Task.Run(async () =>
        {
            while (true)
            {
                if (elevator.HasPendingRequests())
                {
                    await elevator.MoveToNextLevelAsync();
                    await Task.Delay(TimeSpan.FromSeconds(2));
                }
            }
        });
    }
}
