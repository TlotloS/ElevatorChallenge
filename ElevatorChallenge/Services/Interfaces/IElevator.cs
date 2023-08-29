using ElevatorChallenge.Models;

namespace ElevatorChallenge.Services.Interfaces
{
    public interface IElevator
    {
        ElevatorStatus CurrentStatus { get; }
        /// <summary>
        /// Moves to the next floor - this includes a delay
        /// <list type="number">
        /// <item>Check passenger requests</item>
        /// <item>Adjust status i.e. change direction, handle passengers</item>
        /// </list>
        /// </summary>
        /// <returns></returns>
        Task<ElevatorStatus> MoveToNextLevelAsync();
        Task QueuePassengerRequest(PassengerRequest passengerRequest);
        bool HasPendingRequests();
    }
}
