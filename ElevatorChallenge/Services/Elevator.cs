using ElevatorChallenge.Enums;
using ElevatorChallenge.Models;

namespace ElevatorChallenge.Services
{
    public class Elevator : IElevator
    {
        /// </summary>
        public ElevatorStatus CurrentStatus { get; private set; }
        private List<PassengerRequest> _passengerQueue;
        private List<int> _distinctFloors;
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
           _passengerQueue = new List<PassengerRequest>();
        }

        /// <summary>
        /// Secondary constructor - will be used for unit test for now
        /// </summary>
        /// <param name="initStatus"></param>
        public Elevator(ElevatorStatus initStatus) { CurrentStatus = initStatus; }

        public async Task QueuePassengerRequest(PassengerRequest passengerRequest)
        {
            _passengerQueue.Add(passengerRequest);
            await Task.Delay(TimeSpan.FromMilliseconds(1)); // Simulated delay
        }

        public async Task HandlePassengersAsync()
        {
            await Task.Delay(TimeSpan.FromSeconds(2)); // Simulated delay
        }

        public async Task<ElevatorStatus> MoveToNextLevelAsync()
        {
            // 1) Get direction
            // 2) Check form passenger

            // Asynchronously move to the next floor
            await Task.Delay(TimeSpan.FromSeconds(1)); // Simulated delay
            return new ElevatorStatus { };
        }
    }
}
