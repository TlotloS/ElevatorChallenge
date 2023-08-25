using ElevatorChallenge.Enums;
using ElevatorChallenge.Models;

namespace ElevatorChallenge.Services
{
    public class Elevator : IElevator
    {
        /// </summary>
        public ElevatorStatus CurrentStatus { get; private set; }
        private List<PassengerRequest> _passengerRequestQueue;
        private List<PassengerRequest> _passengersInTransit;
        /// <summary>
        /// Default constructor
        /// </summary>
        public Elevator()
        {
            CurrentStatus = new ElevatorStatus
            {
                Direction = ElevatorDirection.None,
                CurrentFloor = 0,
            };
            _passengerRequestQueue = new List<PassengerRequest>();
            _passengersInTransit = new List<PassengerRequest>();
        }

        /// <summary>
        /// Secondary constructor - will be used for unit test for now
        /// </summary>
        /// <param name="initStatus"></param>
        public Elevator(ElevatorStatus initStatus) {
            CurrentStatus = initStatus;
            _passengersInTransit = new List<PassengerRequest>();
            _passengerRequestQueue = new List<PassengerRequest>();
        }

        public async Task QueuePassengerRequest(PassengerRequest passengerRequest)
        {
            _passengerRequestQueue.Add(passengerRequest);
            await Task.Delay(TimeSpan.FromMilliseconds(1)); // Simulated delay
        }
        public async Task<ElevatorStatus> MoveToNextLevelAsync()
        {
            // if there aren't any pending request return the current status
            if (!HasPendingRequests()) { 
                CurrentStatus.Direction = ElevatorDirection.None;
                return CurrentStatus;
            }

            var floorsInMyDirection = await GetFloorStoppingPointsWithinTravellingDirectionAsync();
            if(floorsInMyDirection.Any())


            // increment or decrement floor based on current floor and direction (moved)
            if (CurrentStatus.Direction == ElevatorDirection.Up)
                CurrentStatus.CurrentFloor += 1;
            else
                CurrentStatus.CurrentFloor -= 1;

            if (floorsInMyDirection.Any(floorLevel => CurrentStatus.CurrentFloor == floorLevel))
            {
               await HandlePassengersAsync();
            }
            else
            {
                await Task.Delay(TimeSpan.FromSeconds(0.5)); // Simulated delay
            }
            return CurrentStatus;
        }


        public async Task HandlePassengersAsync()
        {
            // Move passengers around here
            Console.WriteLine("Handling passengers async");
            await Task.Delay(TimeSpan.FromSeconds(2)); // Simulated delay
        }

        private async Task<IEnumerable<int>> GetFloorStoppingPointsWithinTravellingDirectionAsync()
        {
            var direction = CurrentStatus.Direction;
            var floorsToStopAtInDirection = new List<int>();

            if (direction == ElevatorDirection.Up)
            {
                var floorsForPickUps = _passengerRequestQueue
                    .Where(x => x.OriginFloorLevel > CurrentStatus.CurrentFloor)
                    .Select(x => x.OriginFloorLevel);

                var floorsForDropOffs = _passengersInTransit
                    .Where(x => x.DestinationFloorLevel > CurrentStatus.CurrentFloor)
                    .Select(x => x.DestinationFloorLevel);

                floorsToStopAtInDirection.AddRange(floorsForPickUps);
                floorsToStopAtInDirection.AddRange(floorsForDropOffs);
            }
            else if (direction == ElevatorDirection.Down)
            {
                var floorsForPickUps = _passengerRequestQueue
                    .Where(x => x.OriginFloorLevel < CurrentStatus.CurrentFloor)
                    .Select(x => x.OriginFloorLevel);

                var floorsForDropOffs = _passengersInTransit
                    .Where(x => x.DestinationFloorLevel < CurrentStatus.CurrentFloor)
                    .Select(x => x.DestinationFloorLevel);

                floorsToStopAtInDirection.AddRange(floorsForPickUps);
                floorsToStopAtInDirection.AddRange(floorsForDropOffs);
            }

            // If there are no floors to stop at in the current direction, change the direction
            if (!floorsToStopAtInDirection.Any())
            {
                CurrentStatus.Direction = (direction == ElevatorDirection.Up) ? ElevatorDirection.Down : ElevatorDirection.Up;
            }

            return floorsToStopAtInDirection;
        }


        private bool HasPendingRequests() => _passengerRequestQueue.Any() || _passengersInTransit.Any();
        private ElevatorDirection DetermineElevatorDirection()
        {
            if (_passengerRequestQueue.Any())
            {
                return _passengerRequestQueue.First().OriginFloorLevel > CurrentStatus.CurrentFloor ? ElevatorDirection.Up : ElevatorDirection.Down;
            }

            if (_passengersInTransit.Any())
            {
                return _passengersInTransit.First().DestinationFloorLevel > CurrentStatus.CurrentFloor ? ElevatorDirection.Up : ElevatorDirection.Down;
            }
            return ElevatorDirection.None;
        }
    }
}
