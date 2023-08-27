using ElevatorChallenge.Enums;
using ElevatorChallenge.Models;
using System.Text;

namespace ElevatorChallenge.Helpers
{
    public class DisplayHelper : IDisplayHelper
    {
        public void LogErrorToConsole(string input)
        {
            Console.SetCursorPosition(0, 3);
            Console.WriteLine($"Something went wrong: {input}");
            Console.SetCursorPosition(0, 2);
        }
        public void UpdateOutputSection(IEnumerable<ElevatorStatus> elevatorStatusList)
        {
            Console.SetCursorPosition(0, 1); // Move cursor to a specific position1
            Console.WriteLine("Input Section (originFloor; destinationFloor; passengers):");
            // Perform output logic or display information

            Console.SetCursorPosition(0, 10);
            Console.WriteLine("Output Section:");
            PrintElevatorStatusMatrix(elevatorStatusList);

            Console.SetCursorPosition(0, 2);
        }

        private void PrintElevatorStatusMatrix(IEnumerable<ElevatorStatus> elevatorStatusList)
        {
            // Determine the highest floor among all elevators
            int highestFloor = elevatorStatusList.Max(e => e.CurrentFloor);

            // Print header
            Console.WriteLine("Elevator\tDirection\tFloor\tPassenger Count\tStatus");

            // Print elevator status information
            foreach (var elevatorStatus in elevatorStatusList)
            {
                string positionIndicator = GeneratePositionIndicator(elevatorStatus.CurrentFloor, highestFloor);
                Console.WriteLine($"{elevatorStatus.ElevatorNumber}\t\t{GetDirectionSymbol(elevatorStatus.Direction)}\t\t{elevatorStatus.CurrentFloor}\t\t{elevatorStatus.Load}\t\t{positionIndicator}");
            }
        }

        private string GetDirectionSymbol(ElevatorDirection direction)
        {
            switch (direction)
            {
                case ElevatorDirection.Up:
                    return "UP";
                case ElevatorDirection.Down:
                    return "DOWN";
                default:
                    return "-";
            }
        }

        private string GeneratePositionIndicator(int currentFloor, int highestFloor)
        {
            StringBuilder indicatorBuilder = new StringBuilder();

            for (int floor = highestFloor; floor >= 1; floor--)
            {
                indicatorBuilder.Append(floor == currentFloor ? "E" : " ");
            }

            return indicatorBuilder.ToString();
        }
    }
}
