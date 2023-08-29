using ElevatorChallenge.Models;

namespace ElevatorChallenge.Services.Interfaces
{
    public interface IElevatorMotion
    {
        /// <summary>
        /// Get the travel elevator travel information i.e route
        /// <list type="number">
        /// <item>The direction which the elevator is/should travel in</item>
        /// <item>The floors which the elevator should stop on in the current direction</item>
        /// </list>
        /// </summary>
        /// <param name="elevator"></param>
        /// <param name="passengerRequestQueue"></param>
        /// <param name="passengersInTransit"></param>
        /// <returns>The travelling details model of type <see cref="ElevatorTravelDetails"/></returns>
        Task<ElevatorTravelDetails> GetElevatorTravelDetailsAsync(ElevatorStatus elevator,
            IEnumerable<PassengerRequest> passengerRequestQueue,
            IEnumerable<PassengerRequest> passengersInTransit);

        /// <summary>
        /// This lets you know if the elevator instance has eany pending passenger requests 
        /// (i.e. Pickups and/or DropOffs)
        /// which need to be actioned from the current floors.
        /// </summary>
        /// <param name="direction"></param>
        /// <param name="currentFloor"></param>
        /// <param name="passengerRequestQueue"></param>
        /// <param name="passengersInTransit"></param>
        /// <returns></returns>
        Task<bool> HasPendingRequestsFromCurrentFloor(int currentFloor,
            IEnumerable<PassengerRequest> passengerRequestQueue,
            IEnumerable<PassengerRequest> passengersInTransit);
    }
}
