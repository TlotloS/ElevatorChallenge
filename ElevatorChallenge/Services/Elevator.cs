using ElevatorChallenge.Enums;
using ElevatorChallenge.Helpers;
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
        public Elevator(int elevatorIndex)
        {
            CurrentStatus = new ElevatorStatus
            {
                Direction = ElevatorDirection.None,
                CurrentFloor = 0,
                ElevatorNumber = elevatorIndex,
                Name = ElevatorNamesHelper.GetElevatorName(elevatorIndex),
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
            await Task.Delay(TimeSpan.FromSeconds(1)); // Simulated delay
            _passengerRequestQueue.Add(passengerRequest);
        }
        public async Task<ElevatorStatus> MoveToNextLevelAsync()
        {
            // if there aren't any pending request return the current status
            if (!HasPendingRequests()) { 
                CurrentStatus.Direction = ElevatorDirection.None;
                await Task.Delay(TimeSpan.FromSeconds(1)); // Simulated delay
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
                await Task.Delay(TimeSpan.FromSeconds(1)); // Simulated delay
            }
            return CurrentStatus;
        }


        private async Task HandlePassengersAsync()
        {
            // Move passengers around here
            HandleDropOffs();
            HandlePickups();
            await Task.Delay(TimeSpan.FromSeconds(2)); // Simulated delay
        }

        private void HandlePickups()
        {
            var pickUps = _passengerRequestQueue.Where(x => x.OriginFloorLevel == CurrentStatus.CurrentFloor).ToList();

            // remove from queue
            foreach (var pickup in pickUps)
            {
                _passengerRequestQueue.Remove(pickup);
            }

            // Handle passengers who were picked up on the current floor - move them to the _passengersInTransit list
            if (pickUps.Any()) { 
                _passengersInTransit.AddRange(pickUps);
                CurrentStatus.Load += pickUps.Sum(x => x.PassengerCount) ;
            }

        }

        private void HandleDropOffs()
        {
            var dropOffs = _passengersInTransit.Where(x => x.DestinationFloorLevel == CurrentStatus.CurrentFloor).ToList();
            CurrentStatus.Load -= dropOffs.Sum(x => x.PassengerCount);
            foreach (var dropoff in dropOffs)
            {
                _passengersInTransit.Remove(dropoff);
            }
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
                // toggle the direction
                CurrentStatus.Direction = (direction == ElevatorDirection.Up) ? ElevatorDirection.Down : ElevatorDirection.Up;
                // recursively call this method to get the new floors
                return await GetFloorStoppingPointsWithinTravellingDirectionAsync();
            }

            return floorsToStopAtInDirection;
        }


        private bool HasPendingRequests() => _passengerRequestQueue.Any() || _passengersInTransit.Any();
    }
}
