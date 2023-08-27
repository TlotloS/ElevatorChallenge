using ElevatorChallenge.Enums;
using ElevatorChallenge.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ElevatorChallenge.Services
{
    public class ElevatorMotion
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
            var allPassengerRequests = passengerRequestQueue.Concat(passengersInTransit);
            // Set the elevator status to Up by default : to do - use logic to change this
            var defaultDirection = elevator.Direction == ElevatorDirection.None ? ElevatorDirection.Down : elevator.Direction;
            //(1) If the elevator has 0 stopping points in the upward or downward direction
            if (!allPassengerRequests.Any())
            {
                return new ElevatorTravelDetails
                {
                    Direction = ElevatorDirection.None,
                    FloorsToStop = new List<int>(),
                };
            }


            //(2) if the elevator still has travel point in the direction then direction stays the same
            var floorToStopInCurrentDirection = await GetStoppingFloors(defaultDirection, 
                elevator.CurrentFloor, allPassengerRequests);
            if (floorToStopInCurrentDirection.Any())
            {
                return new ElevatorTravelDetails
                {
                    Direction = elevator.Direction,
                    FloorsToStop = floorToStopInCurrentDirection,
                };
            }


            // toggle direction ~ try the other direction
            var oppositeDirection = (elevator.Direction == ElevatorDirection.Up) ? ElevatorDirection.Down : ElevatorDirection.Up;

            //(3) if the elevator has travel points in the opposite direction  then direction is toggle
            var floorToStopInOppositeDirection = await GetStoppingFloors(oppositeDirection, 
                elevator.CurrentFloor, allPassengerRequests);
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

        /// <summary>
        /// Takes the current elevator direction and a list of PassengerRequest 
        /// objects and returns a list of integers representing the floors
        /// where the elevator will stop.
        /// </summary>
        /// <param name="direction"></param>
        /// <param name="passengerRequests"></param>
        /// <returns>a list of integers representing the floors</returns>
        public async Task<IEnumerable<int>> GetStoppingFloors(ElevatorDirection direction, int currentFloor, 
            IEnumerable<PassengerRequest> passengerRequests)
        {
            if (direction == ElevatorDirection.Up)
            {
                var floorsToStopOn = passengerRequests
                    .Where(x => x.OriginFloorLevel > currentFloor)
                    .Select(x => x.OriginFloorLevel);

                return await Task.FromResult(floorsToStopOn);
            }
            else if (direction == ElevatorDirection.Down)
            {
                var floorsToStopOn = passengerRequests
                    .Where(x => x.OriginFloorLevel < currentFloor)
                    .Select(x => x.OriginFloorLevel);

                return await Task.FromResult(floorsToStopOn);
            }
            return await Task.FromResult(new List<int>());
        }

    }
}
