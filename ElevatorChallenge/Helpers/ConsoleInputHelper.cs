using ElevatorChallenge.Models;
using ElevatorChallenge.Services;

namespace ElevatorChallenge.Helpers
{
    public class ConsoleInputHelper : IConsoleInputHelper
    {
        private readonly IControlCentreService _controlCentreService;
        private readonly IElevatorThreadManager _elevatorThreadManager;
        private readonly IConsoleOutputHelper _diplayHelper;
        private List<ElevatorStatus> _elevatorStatuses = new List<ElevatorStatus>();
        public ConsoleInputHelper(IControlCentreService controlCentreService,
            IElevatorThreadManager elevatorThreadManager,
            IConsoleOutputHelper displayHelper)
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
            Console.SetCursorPosition(0, 1); // Move cursor to a specific position1
            Console.WriteLine("Input Section (originFloor; destinationFloor; passengers):");
            Console.SetCursorPosition(0, 2);
            await Task.Run(async () =>
            {
                await StartElevatorThreads();
                while (true)
                {
                    Console.Clear();
                    var elevatorStatusList = await _controlCentreService.GetElevatorStatuses();
                    _elevatorStatuses = elevatorStatusList.ToList();
                    _diplayHelper.UpdateOutputSection(_elevatorStatuses);

                    // Delay before updating again
                    await Task.Delay(TimeSpan.FromSeconds(0.25));

                    // Check for user input without blocking
                    if (Console.KeyAvailable)
                    {
                        string input = Console.ReadLine();
                        await ConvertInputToAndSubmitRequestAsync(input);
                    }
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
                await Task.Delay(TimeSpan.FromSeconds(0.5));
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
