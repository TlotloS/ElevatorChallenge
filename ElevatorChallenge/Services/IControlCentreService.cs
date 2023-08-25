using ElevatorChallenge.Models;

namespace ElevatorChallenge.Services
{
    /// <summary>
    /// Control Centre Interface
    /// </summary>
    public interface IControlCentreService
    {
        /// <summary>
        /// Method for creating a new passenger request
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        Task AddPickUpRequest(PassengerRequest request);
        /// <summary>
        /// Return the awaiting passenger list
        /// </summary>
        /// <returns></returns>
        IEnumerable<PassengerRequest> GetPendingPassengerRequests();
        ElevatorSystemConfig GetConfigDetails();
        Task<IEnumerable<ElevatorStatus>> GetElevatorStatuses();
        Task<IEnumerable<IElevator>> GetElevators();
    }
}
