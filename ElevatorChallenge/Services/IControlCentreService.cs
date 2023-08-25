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
        public void AddPickUpRequest(PassengerRequest request);
        /// <summary>
        /// Return the awaiting passenger list
        /// </summary>
        /// <returns></returns>
        public IEnumerable<PassengerRequest> GetPendingPassengerRequests();
        public ElevatorSystemConfig GetConfigDetails();
        IEnumerable<IElevator> GetElevators();
    }
}
