using ElevatorChallenge.Enums;
using ElevatorChallenge.Services;

namespace ElevatorChallengeTests
{
    public class ElevatorTests
    {
        public ElevatorTests()
        {
        }

        [Fact]
        public void Elevator_InitaliseElevator_VerifyInitialStatus()
        {
            // arrange
            var elevator = new Elevator();
            // assert
            var status = elevator.CurrentStatus;
            Assert.False(status.CurrentlyActive);
            Assert.Equal(0, status.CurrentFloor);
            Assert.Equal(ElevatorDirection.None, status.Direction);
        }

    }
}
