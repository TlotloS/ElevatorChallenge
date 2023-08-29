namespace ElevatorChallenge.Helpers
{
    /// <summary>
    /// This interface is responsible for the operation of the elevator system
    /// <list type="bullet">
    /// <item>Starting up the elevator system</item>
    /// <item>Recording the passenger requests ~ input</item>
    /// <item>Providing a meaning meaning full UI for the system status</item>
    /// </list>
    /// </summary>
    public interface IOperationService
    {
        /// <summary>
        /// Begin the elevator threads that managed the individual elevator instaces
        /// </summary>
        /// <returns></returns>
        Task StartElevatorThreads();
        /// <summary>
        /// Start the async method operation responsible for monitoring the 
        /// I/O of the the elevator system.
        /// <list type="number">
        /// <item>Keeps track of the user inputs - responsible for recording elevator request from the console</item>
        /// <item>Outputs the elevator statuses on the console</item>
        /// </list>
        /// </summary>
        /// <returns></returns>
        Task StartTheElevatorSystemIOMonitoringProcessAsync();
    }
}
