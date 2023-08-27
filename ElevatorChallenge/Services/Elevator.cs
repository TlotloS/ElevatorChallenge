using ElevatorChallenge.Enums;
using ElevatorChallenge.Helpers;
using ElevatorChallenge.Models;

namespace ElevatorChallenge.Services
{
    public class Elevator : ElevatorMotion, IElevator
    {
        /// </summary>
        public ElevatorStatus CurrentStatus { get; private set; }
        private List<PassengerRequest> _passengerRequestQueue;
        private List<PassengerRequest> _passengersInTransit;
        private ElevatorConfiguration _elevatorConfiguration;
        /// <summary>
        /// Default constructor
        /// </summary>
        public Elevator(int elevatorIndex, ElevatorConfiguration elevatorConfiguration)
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
            _elevatorConfiguration = elevatorConfiguration;
        }

        /// <summary>
        /// Secondary constructor - will be used for unit test for now
        /// </summary>
        /// <param name="initStatus"></param>
        public Elevator(ElevatorStatus initStatus, ElevatorConfiguration elevatorConfiguration)
        {
            CurrentStatus = initStatus;
            _passengersInTransit = new List<PassengerRequest>();
            _passengerRequestQueue = new List<PassengerRequest>();
            _elevatorConfiguration = elevatorConfiguration;
        }

        public async Task QueuePassengerRequest(PassengerRequest passengerRequest)
        {
            await Task.Delay(TimeSpan.FromSeconds(1)); // Simulated delay
            _passengerRequestQueue.Add(passengerRequest);
        }
        public async Task<ElevatorStatus> MoveToNextLevelAsync()
        {
            // if there aren't any pending request return the current status
            if (!HasPendingRequests())
            {
                CurrentStatus.Direction = ElevatorDirection.None;
                await Task.Delay(TimeSpan.FromSeconds(3)); // Simulated delay
                return CurrentStatus;
            }

            var elevatorTravelDetails = await GetElevatorTravelDetailsAsync(CurrentStatus,
                _passengerRequestQueue,
                _passengersInTransit);

            if (!elevatorTravelDetails.FloorsToStop.Any())
            {
                CurrentStatus.Direction = ElevatorDirection.None;
                return CurrentStatus;
            }
            else
            {
                CurrentStatus.Direction = elevatorTravelDetails.Direction;
            }

            // increment or decrement floor based on current floor and direction (moved)
            if (elevatorTravelDetails.Direction == ElevatorDirection.Up)
                CurrentStatus.CurrentFloor += 1;
            else if (elevatorTravelDetails.Direction == ElevatorDirection.Down)
                CurrentStatus.CurrentFloor -= 1;
            else return CurrentStatus;

            if (elevatorTravelDetails.FloorsToStop.Any(floorLevel => CurrentStatus.CurrentFloor == floorLevel))
            {
                await HandlePassengersAsync();
            }
            else
            {
                await Task.Delay(_elevatorConfiguration.DelayInSeconds.MovingToNextLevel); // Simulated delay
            }
            return CurrentStatus;
        }

        #region Private methods

        private async Task HandlePassengersAsync()
        {
            // Move passengers around here
            HandleDropOffs();
            HandlePickups();
            await Task.Delay(TimeSpan.FromSeconds(_elevatorConfiguration.DelayInSeconds.HandlingPassengers)); // Simulated delay
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
            if (pickUps.Any())
            {
                _passengersInTransit.AddRange(pickUps);
                CurrentStatus.Load += pickUps.Sum(x => x.PassengerCount);
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

        private bool HasPendingRequests() => _passengerRequestQueue.Any() || _passengersInTransit.Any();
        #endregion
    }
}
