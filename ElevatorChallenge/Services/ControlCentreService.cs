using ElevatorChallenge.Enums;
using ElevatorChallenge.Models;

namespace ElevatorChallenge.Services
{
    public class ControlCentreService : IControlCentreService
    {
        private readonly List<IElevator> _elevators = new List<IElevator>();
        private readonly ElevatorSystemConfig _elevatorSystemConfig;
        private readonly List<PassengerRequest> _pendingPassengerRequests = new();
        private readonly IElevatorThreadManager _elevatorThreadManager;

        /// <summary>
        /// Default constructor
        /// </summary>
        /// <param name="totalFloors"></param>
        /// <param name="totalElevators"></param>
        /// <param name="maxElevatorWeight"></param>
        public ControlCentreService(IElevatorThreadManager elevatorThreadManager, ElevatorSystemConfig elevatorSystemConfig)
        {
            _elevatorThreadManager = elevatorThreadManager;
            _elevatorSystemConfig = elevatorSystemConfig;
            for (int i = 0; i < elevatorSystemConfig.ElevatorsCount; i++)
            {
                _elevators.Add(new Elevator(i));
            }

            // start elevator
            _elevatorThreadManager.StartElevatorThreadsAsync(_elevators);
        }

        public async Task AddPickUpRequest(PassengerRequest request)
        {
            // Assume that the passenger will reconduct a valid request if they exceed the limit
            if(request.PassengerCount >= _elevatorSystemConfig.MaxWeight) {
                throw new Exception("Weight limit exceeded, please try again");
            }
            if (request.OriginFloorLevel > _elevatorSystemConfig.FloorsCount || request.OriginFloorLevel <= 0)
            {
                throw new InvalidOperationException($"Invalid floor level {nameof(request.OriginFloorLevel)}");
            }
            if (request.DestinationFloorLevel > _elevatorSystemConfig.FloorsCount || request.DestinationFloorLevel <= 0)
            {
                throw new InvalidOperationException($"Invalid floor level {nameof(request.DestinationFloorLevel)}");
            }
            // Ignore request to the same floor
            if (request.OriginFloorLevel == request.DestinationFloorLevel) { return; }
            _pendingPassengerRequests.Add(request);
            var elevator = GetClosestElevator(request);
            await elevator.QueuePassengerRequest(request);
        }

        public IEnumerable<PassengerRequest> GetPendingPassengerRequests() => _pendingPassengerRequests;
        public ElevatorSystemConfig GetConfigDetails() => _elevatorSystemConfig;
        public IEnumerable<ElevatorStatus> GetElevatorStatuses() => _elevators.Select(x => x.CurrentStatus);

        #region Private method
        private IElevator GetClosestElevator(PassengerRequest request)
        {
            // determine direction of passenger request
            ElevatorDirection requestDirection = request.DestinationFloorLevel - request.DestinationFloorLevel > 0 ?
                ElevatorDirection.Up : 
                ElevatorDirection.Down;

            var closestElevator = _elevators
                .OrderBy((x) => Math.Abs(x.CurrentStatus.CurrentFloor - request.OriginFloorLevel)) // absolute value ensure the delta is alway positive
                .First();

            // To-do
            // 1) Consider elevator going in the same direction first as the passenger request
            // 2) Provided the elevator has not already passed the elevator

            return closestElevator;
        }
        #endregion

    }
}
