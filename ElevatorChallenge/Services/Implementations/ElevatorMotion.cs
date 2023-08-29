using ElevatorChallenge.Enums;
using ElevatorChallenge.Models;
using ElevatorChallenge.Services.Interfaces;

namespace ElevatorChallenge.Services.Implementations
{
    public class ElevatorMotion : IElevatorMotion
    {
        /// <summary>
        /// Get the travel elevator travel information (Direction and floors to stop on)
        /// </summary>
        /// <param name="elevator"></param>
        /// <param name="passengerRequestQueue"></param>
        /// <param name="passengersInTransit"></param>
        /// <returns></returns>
        public async Task<ElevatorTravelDetails> GetElevatorTravelDetailsAsync(ElevatorStatus elevator,
            IEnumerable<PassengerRequest> passengerRequestQueue,
            IEnumerable<PassengerRequest> passengersInTransit)
        {
            var defaultDirection = elevator.Direction == ElevatorDirection.None ? ElevatorDirection.Up : elevator.Direction;
            //(1) If the elevator has 0 stopping points in the upward or downward direction
            if (passengerRequestQueue.Any() == false && passengersInTransit.Any() == false)
            {
                return new ElevatorTravelDetails
                {
                    Direction = ElevatorDirection.None,
                    FloorsToStop = new List<int>(),
                };
            }


            //(2) if the elevator still has travel point in the direction then direction stays the same
            var floorToStopInCurrentDirection = await GetStoppingFloors(defaultDirection,
                elevator.CurrentFloor, passengerRequestQueue, passengersInTransit);
            if (floorToStopInCurrentDirection.Any())
            {
                return new ElevatorTravelDetails
                {
                    Direction = defaultDirection,
                    FloorsToStop = floorToStopInCurrentDirection,
                };
            }


            // toggle direction ~ try the other direction
            var oppositeDirection = defaultDirection == ElevatorDirection.Up ? ElevatorDirection.Down : ElevatorDirection.Up;

            //(3) if the elevator has travel points in the opposite direction  then direction is toggle
            var floorToStopInOppositeDirection = await GetStoppingFloors(oppositeDirection,
                elevator.CurrentFloor, passengerRequestQueue, passengersInTransit);
            if (floorToStopInOppositeDirection.Any())
            {
                return new ElevatorTravelDetails
                {
                    Direction = oppositeDirection,
                    FloorsToStop = floorToStopInOppositeDirection,
                };
            }
            // default - this should never happen
            return new ElevatorTravelDetails
            {
                Direction = ElevatorDirection.None,
                FloorsToStop = new List<int>(),
            };
        }

        ///<inheritdoc/>
        public async Task<bool> HasPendingRequestsFromCurrentFloor(int currentFloor,
            IEnumerable<PassengerRequest> passengerRequestQueue,
            IEnumerable<PassengerRequest> passengersInTransit)
        {

            var floorsToStopOnPickups = passengerRequestQueue
                .Where(x => x.OriginFloorLevel == currentFloor)
                .Select(x => x.OriginFloorLevel);

            var floorsToStopOnDropOffs = passengersInTransit
                .Where(x => x.DestinationFloorLevel == currentFloor)
                .Select(x => x.DestinationFloorLevel);
            return await Task.FromResult(floorsToStopOnPickups.Concat(floorsToStopOnDropOffs).Any());
        }

        /// <summary>
        /// /// Determined which floors the elevator should stop in the current travelling direction
        /// </summary>
        /// <param name="direction">Direction which the elevator instance is moving in</param>
        /// <param name="currentFloor">Current floor of the elevator</param>
        /// <param name="passengerRequestQueue">Passenger that are awaiting pickup</param>
        /// <param name="passengersInTransit">Passengers that are in transit - pendinging dropoff</param>
        /// <returns></returns>
        private async Task<IEnumerable<int>> GetStoppingFloors(ElevatorDirection direction, int currentFloor,
            IEnumerable<PassengerRequest> passengerRequestQueue,
            IEnumerable<PassengerRequest> passengersInTransit)
        {
            if (direction == ElevatorDirection.Up)
            {
                var floorsToStopOnPickups = passengerRequestQueue
                    .Where(x => x.OriginFloorLevel >= currentFloor)
                    .Select(x => x.OriginFloorLevel);

                var floorsToStopOnDropOffs = passengersInTransit
                    .Where(x => x.DestinationFloorLevel >= currentFloor)
                    .Select(x => x.DestinationFloorLevel);
                return await Task.FromResult(floorsToStopOnPickups.Concat(floorsToStopOnDropOffs));
            }
            else if (direction == ElevatorDirection.Down)
            {
                var floorsToStopOnPickups = passengerRequestQueue
                    .Where(x => x.OriginFloorLevel <= currentFloor)
                    .Select(x => x.OriginFloorLevel);

                var floorsToStopOnDropOffs = passengersInTransit
                    .Where(x => x.DestinationFloorLevel <= currentFloor)
                    .Select(x => x.DestinationFloorLevel);
                return await Task.FromResult(floorsToStopOnPickups.Concat(floorsToStopOnDropOffs));
            }
            return await Task.FromResult(new List<int>());
        }
    }
}
