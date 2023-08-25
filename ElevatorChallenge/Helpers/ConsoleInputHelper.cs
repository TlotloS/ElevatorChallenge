using ElevatorChallenge.Enums;
using ElevatorChallenge.Models;
using ElevatorChallenge.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ElevatorChallenge.Helpers
{
    public class ConsoleInputHelper : IConsoleInputHelper
    {
        private readonly IControlCentreService _controlCentreService;
        private readonly IElevatorThreadManager _elevatorThreadManager;
        public ConsoleInputHelper(IControlCentreService controlCentreService, IElevatorThreadManager elevatorThreadManager)
        {
            _controlCentreService = controlCentreService;
            _elevatorThreadManager = elevatorThreadManager;
        }
        public async Task StartElevatorThreads()
        {
            var elevators = await _controlCentreService.GetElevators();
            _elevatorThreadManager.StartElevatorThreadsAsync(elevators);
        }
        public async Task StartPrintingTaskAsync()
        {
            await Task.Run(async () =>
            {
                await StartElevatorThreads();
                while (true)
                {
                    // Clear the console and print elevator statuses
                    Console.Clear();
                    var elevatorStatusList = await _controlCentreService.GetElevatorStatuses(); // Implement this async method to get the status list
                    UpdateOutputSection(elevatorStatusList);
                    await UpdateInputSection();

                    // Delay before updating again
                    await Task.Delay(TimeSpan.FromSeconds(2));
                }
            });
        }

        private async Task ConvertInputToAndSubmitRequestAsync(string input)
        {
            string[] parts = input.Split(';');

            if (parts.Length != 3)
            {
                throw new ArgumentException("Invalid input format");
            }

            int originFloor = int.Parse(parts[0]);
            int destinationFloor = int.Parse(parts[1]);
            int passengers = int.Parse(parts[2]);

            await _controlCentreService.AddPickUpRequest(new PassengerRequest
            {
                OriginFloorLevel = originFloor,
                DestinationFloorLevel = destinationFloor,
                PassengerCount = passengers
            });
        }
        private async Task UpdateInputSection()
        {
            Console.SetCursorPosition(0, 1); // Move cursor to a specific position1
            Console.WriteLine("Input Section (originFloor; destinationFloor; passengers):");
            Console.SetCursorPosition(0, 2);
            string input = Console.ReadLine();
            // Process input or perform actions based on input
            await ConvertInputToAndSubmitRequestAsync(input);
        }

        private void UpdateOutputSection(IEnumerable<ElevatorStatus> elevatorStatusList)
        {
            Console.SetCursorPosition(0, 10);
            Console.WriteLine("Output Section:");
            Console.SetCursorPosition(0, 11);
            // Perform output logic or display information
            PrintElevatorStatusMatrix(elevatorStatusList);
        }


        private void PrintElevatorStatusMatrix(IEnumerable<ElevatorStatus> elevatorStatusList)
        {
                // Print header
                Console.WriteLine("Elevator\tFloor\tDirection\tPassenger Count");

                // Print elevator status information
                foreach (var elevatorStatus in elevatorStatusList)
                {
                    Console.WriteLine($"{elevatorStatus.ElevatorNumber}\t\t{elevatorStatus.CurrentFloor}\t{GetDirectionSymbol(elevatorStatus.Direction)}\t\t{elevatorStatus.Load}");
                }
        }

        private string GetDirectionSymbol(ElevatorDirection direction)
        {
            switch (direction)
            {
                case ElevatorDirection.Up:
                    return "↑";
                case ElevatorDirection.Down:
                    return "↓";
                default:
                    return "-";
            }
        }
    }
}
