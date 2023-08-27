using ElevatorChallenge.Models;

namespace ElevatorChallenge.Helpers
{
    public interface IConsoleOutputHelper
    {
        void LogErrorToConsole(string input);
        void UpdateOutputSection(IEnumerable<ElevatorStatus> elevatorStatusList);
    }
}
