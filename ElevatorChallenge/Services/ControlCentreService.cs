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

        public void AddPassengerRequest(PassengerRequest request)
        {
            // Assume that the passenger will reconduct a valid request if they exceed the limit
            if(request.PassengerCount >= _elevatorSystemConfig.MaxWeight) {
                throw new Exception("Weight limit exceeded, please try again");
            }
            _pendingPassengerRequests.Add(request);
        }

        public IEnumerable<PassengerRequest> GetPendingPassengerRequests() => _pendingPassengerRequests;
        public ElevatorSystemConfig GetConfigDetails() => _elevatorSystemConfig;
        public IEnumerable<IElevator> GetElevators() => _elevators;

    }
}
