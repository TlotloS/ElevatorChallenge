using ElevatorChallenge.Models;

namespace ElevatorChallenge.Services
{
    public interface IElevator
    {
        public ElevatorStatus CurrentStatus { get; }
        /// <summary>
        /// Moves to the next floor - this includes a delay
        /// <list type="number">
        /// <item>Check passenger requests</item>
        /// <item>Adjust status i.e. change direction, handle passengers</item>
        /// </list>
        /// </summary>
        /// <returns></returns>
        Task<ElevatorStatus> MoveToNextLevelAsync();
        /// <summary>
        /// Picking up and dropping off passengers.
        /// <list type="number">
        /// <item>Drop off/pickup passenger if on the correct level</item>
        /// <item></item>
        /// </list>
        /// </summary>
        /// <returns></returns>
        Task HandlePassengersAsync();
        Task AddPassengerRequest(int originFloor, int destinationFloor, int passengerCount);
    }
}
