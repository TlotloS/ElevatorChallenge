using ElevatorChallenge.Enums;
using ElevatorChallenge.Models;
using ElevatorChallenge.Services.Interfaces;
using Microsoft.Extensions.Options;

namespace ElevatorChallenge.Services.Implementations
{
    public class ControlCentreService : IControlCentreService
    {
        private readonly List<IElevator> _elevators = new List<IElevator>();
        private readonly ElevatorConfiguration _config;
        private readonly List<PassengerRequest> _pendingPassengerRequests = new();

        /// <summary>
        /// Default constructor
        /// </summary>
        /// <param name="config">Dependancy Injection - Injects the config service depedancy via constructor dependancy</param>
        public ControlCentreService(IOptions<ElevatorConfiguration> config)
        {
            _config = config.Value;
            for (int i = 0; i < _config.TotalElevators; i++)
            {
                _elevators.Add(new Elevator(i, _config));
            }
        }

        public async Task AddPickUpRequest(PassengerRequest request)
        {
            // Assume that the passenger will reconduct a valid request if they exceed the limit
            if (request.PassengerCount > _config.ElevatorMaximumWeight)
            {
                throw new Exception("Weight limit exceeded, please try again");
            }
            if (request.OriginFloorLevel >= _config.TotalFloors || request.OriginFloorLevel < 0)
            {
                throw new InvalidOperationException($"Invalid origin floor level {nameof(request.OriginFloorLevel)}");
            }
            if (request.DestinationFloorLevel >= _config.TotalFloors || request.DestinationFloorLevel < 0)
            {
                throw new InvalidOperationException($"Invalid destinatino floor level {nameof(request.DestinationFloorLevel)}");
            }
            // Ignore request to the same floor
            if (request.OriginFloorLevel == request.DestinationFloorLevel) { return; }
            _pendingPassengerRequests.Add(request);
            var elevator = await GetClosestElevator(request);
            await elevator.QueuePassengerRequest(request);
        }

        public IEnumerable<PassengerRequest> GetPendingPassengerRequests() => _pendingPassengerRequests;
        public ElevatorConfiguration GetConfigDetails() => _config;
        public Task<IEnumerable<ElevatorStatus>> GetElevatorStatuses() => Task.FromResult(_elevators.Select(x => x.CurrentStatus));

        #region Private method
        private Task<IElevator> GetClosestElevator(PassengerRequest request)
        {
            // determine direction of passenger request
            ElevatorDirection requestDirection = request.DestinationFloorLevel - request.OriginFloorLevel > 0 ?
                ElevatorDirection.Up :
                ElevatorDirection.Down;

            // 1) Consider elevator going in the same direction as the passenger request first
            // 2) Provided the elevator has not already passed the elevator
            var elevatorsTravellingTowardRequestLevel = _elevators.Where(x => x.CurrentStatus.Direction == requestDirection &&
                (
                    (requestDirection == ElevatorDirection.Up && x.CurrentStatus.CurrentFloor <= request.OriginFloorLevel)
                    ||
                    (requestDirection == ElevatorDirection.Down && x.CurrentStatus.CurrentFloor >= request.OriginFloorLevel)
                )
            );
            IElevator closestElevator;

            if (elevatorsTravellingTowardRequestLevel.Any())
            {
                closestElevator = elevatorsTravellingTowardRequestLevel
                // absolute delte will give us the distacne
                .OrderBy((x) => Math.Abs(x.CurrentStatus.CurrentFloor - request.OriginFloorLevel))
                .First();
            }
            else
            {
                closestElevator = _elevators
                                .OrderBy((x) => Math.Abs(x.CurrentStatus.CurrentFloor - request.OriginFloorLevel)) // absolute value ensure the delta is alway positive
                                .First();
            }
            return Task.FromResult(closestElevator);
        }

        public Task<IEnumerable<IElevator>> GetElevators()
        {
            return Task.FromResult<IEnumerable<IElevator>>(_elevators);
        }

        #endregion

    }
}
