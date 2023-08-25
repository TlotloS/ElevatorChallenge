using ElevatorChallenge.Services;

public class ElevatorThreadManager : IElevatorThreadManager
{
    private List<Task> _elevatorTasks = new List<Task>();
    public ElevatorThreadManager()
    {
    }

    public async Task StartElevatorThreadsAsync(List<IElevator> elevators)
    {
        foreach (var elevator in elevators)
        {
            var task = RunElevatorAsync(elevator);
            _elevatorTasks.Add(task);
        }

        await Task.WhenAll(_elevatorTasks);
    }

    private async Task RunElevatorAsync(IElevator elevator)
    {
        while (true)
        {
            await elevator.MoveToNextLevelAsync();
        }
    }
}
