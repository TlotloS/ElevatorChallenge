using ElevatorChallenge.Enums;
using ElevatorChallenge.Models;

namespace ElevatorChallenge.Helpers
{
    public class ConsoleOutputHelper : IConsoleOutputHelper
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

            // Display the output matrix
            Console.SetCursorPosition(0, 10);
            Console.WriteLine("Output Section:");
            PrintElevatorGraph(elevatorStatusList.ToList(), 5);
            PrintElevatorStatusMatrix(elevatorStatusList);

            Console.SetCursorPosition(0, 2);
        }

        static void PrintElevatorGraph(List<ElevatorStatus> elevatorStatuses, int floorsCount)
        {
            int[] floorNumbers = Enumerable.Range(0, floorsCount).ToArray();

            Console.WriteLine(new string('-', (elevatorStatuses.Count + 1) * 20));
            for (int floor = floorNumbers.Length - 1; floor >= 0; floor--)
            {
                Console.Write(floorNumbers[floor]);
                foreach (var elevatorStatus in elevatorStatuses)
                {
                    string position = elevatorStatus.CurrentFloor == floorNumbers[floor] ? $"==={elevatorStatus.Load}===" : "       ";
                    Console.Write($"|{position}");
                }
                Console.WriteLine('|');
            }
            Console.WriteLine(new string('-', (elevatorStatuses.Count + 1) * 20));
            Console.Write(" |");
            foreach (var elevatorStatus in elevatorStatuses)
            {
                Console.Write($"   " +
                    $"{ElevatorNamesHelper.GetElevatorName(elevatorStatus.ElevatorNumber)}" +
                    $"   |");
            }
            Console.WriteLine();
        }
        private void PrintElevatorStatusMatrix(IEnumerable<ElevatorStatus> elevatorStatusList)
        {
            // Determine the highest floor among all elevators
            Console.WriteLine();
            Console.WriteLine("Elevator stats:");
            int highestFloor = elevatorStatusList.Max(e => e.CurrentFloor);

            // Print header
            Console.WriteLine("Elevator\tDirection\tFloor\t#Passengers\t");

            // Print elevator status information
            foreach (var elevatorStatus in elevatorStatusList)
            {
                Console.WriteLine($"{ElevatorNamesHelper.GetElevatorName(elevatorStatus.ElevatorNumber)}" +
                    $"\t\t{GetDirectionSymbol(elevatorStatus.Direction)}" +
                    $"\t\t{elevatorStatus.CurrentFloor}" +
                    $"\t\t{elevatorStatus.Load}");
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
    }
}
