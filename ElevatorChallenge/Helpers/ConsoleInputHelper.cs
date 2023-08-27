using ElevatorChallenge.Models;
using ElevatorChallenge.Services;

namespace ElevatorChallenge.Helpers
{
    public class ConsoleInputHelper : IConsoleInputHelper
    {
        private readonly IControlCentreService _controlCentreService;
        private readonly IElevatorThreadManager _elevatorThreadManager;
        private readonly IDisplayHelper _diplayHelper;
        public ConsoleInputHelper(IControlCentreService controlCentreService,
            IElevatorThreadManager elevatorThreadManager,
            IDisplayHelper displayHelper)
        {
            _controlCentreService = controlCentreService;
            _elevatorThreadManager = elevatorThreadManager;
            _diplayHelper = displayHelper;
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
                    _diplayHelper.UpdateOutputSection(elevatorStatusList);
                    await UpdateInputSection();

                    // Delay before updating again
                    await Task.Delay(TimeSpan.FromSeconds(2.5));
                }
            });
        }

        private async Task ConvertInputToAndSubmitRequestAsync(string input)
        {
            try
            {
                string[] parts = input.Split(';');

                if (parts.Length != 3)
                {
                    _diplayHelper.LogErrorToConsole("Invalid input format");
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
            catch (Exception ex)
            {
                _diplayHelper.LogErrorToConsole(ex.Message);
            }
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
    }
}
