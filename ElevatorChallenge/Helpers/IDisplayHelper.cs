using ElevatorChallenge.Models;

namespace ElevatorChallenge.Helpers
{
    public interface IDisplayHelper
    {
        void LogErrorToConsole(string input);
        void UpdateOutputSection(IEnumerable<ElevatorStatus> elevatorStatusList);
    }
}
