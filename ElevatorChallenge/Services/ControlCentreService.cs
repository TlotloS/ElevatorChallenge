using ElevatorChallenge.Enums;
using ElevatorChallenge.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ElevatorChallenge.Services
{
    public class ControlCentreService : IControlCentreService
    {
        private readonly List<IElevator> _elevators = new List<IElevator>();
        private readonly ElevatorSystemConfig _elevatorSystemConfig;
        private readonly List<PassengerRequest> _pendingPassengerRequests = new();

        /// <summary>
        /// Default constructor
        /// </summary>
        /// <param name="totalFloors"></param>
        /// <param name="totalElevators"></param>
        /// <param name="maxElevatorWeight"></param>
        public ControlCentreService(int totalFloors, int totalElevators, int maxElevatorWeight)
        {
            _elevatorSystemConfig = new ElevatorSystemConfig(totalFloors, totalElevators,maxElevatorWeight);
            for (int i = 0; i < totalElevators; i++)
            {
                _elevators.Add(new Elevator());
            }
        }

        public void AddPickUpRequest(PassengerRequest request)
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
        }

        public IEnumerable<PassengerRequest> GetPendingPassengerRequests() => _pendingPassengerRequests;
        public ElevatorSystemConfig GetConfigDetails() => _elevatorSystemConfig;
        public IEnumerable<IElevator> GetElevators() => _elevators;

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
