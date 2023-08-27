using ElevatorChallenge.Enums;

namespace ElevatorChallenge.Services
{
    public interface IElevatorMotion
    {
        Task<ElevatorDirection> GetElevatorDirectionAsync();
        Task<IEnumerable<int>> DetermineElevatorTravelRoute(ElevatorDirection direction);
        Task<IEnumerable<int>> GetFloorStoppingPointsWithinTravellingDirectionAsync();
    }
}
