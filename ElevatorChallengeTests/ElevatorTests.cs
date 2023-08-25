using ElevatorChallenge.Enums;
using ElevatorChallenge.Models;
using ElevatorChallenge.Services;

namespace ElevatorChallenge.Tests
{
    public class ElevatorTests
    {
        [Fact]
        public async Task TestMoveToNextLevelAsync_NoPendingRequests_ReturnsCurrentStatus()
        {
            // Arrange
            var elevator = new Elevator(new ElevatorStatus
            {
                Direction = ElevatorDirection.None,
                CurrentFloor = 0
            });

            // Act
            var result = await elevator.MoveToNextLevelAsync();

            // Assert
            Assert.Equal(ElevatorDirection.None, result.Direction);
            Assert.Equal(0, result.CurrentFloor);
        }


        [Fact]
        public void Elevator_InitaliseElevator_VerifyInitialStatus()
        {
            // arrange
            var elevator = new Elevator();
            // assert
            var status = elevator.CurrentStatus;
            Assert.Equal(0, status.CurrentFloor);
            Assert.Equal(ElevatorDirection.None, status.Direction);
        }

        [Fact]
        public async Task TestMoveToNextLevelAsync_InitialStateIsStationAndHasPendingRequests_ExpectDirectionSetAndFloorIncremented()
        {
            // Arrange
            var elevator = new Elevator(new ElevatorStatus
            {
                Direction = ElevatorDirection.None,
                CurrentFloor = 0
            });

            var passengerRequest = new PassengerRequest { OriginFloorLevel = 2, DestinationFloorLevel = 5 };
            await elevator.QueuePassengerRequest(passengerRequest);

            // Act
            var result = await elevator.MoveToNextLevelAsync();

            // Assert
            Assert.Equal(ElevatorDirection.Up, result.Direction);
            Assert.Equal(1, result.CurrentFloor); // Current floor incremented
        }

        [Fact]
        public async Task TestHandlePassengersAsync_DirectionIsSetToUpWithPendingRequestInOppositeDirection_ExpectThatDirectionWillBeRevertedToDown()
        {
            // Arrange
            var elevator = new Elevator(new ElevatorStatus
            {
                Direction = ElevatorDirection.Up,
                CurrentFloor = 3
            });

            var passengerRequest = new PassengerRequest { OriginFloorLevel = 2, DestinationFloorLevel = 5 };
            await elevator.QueuePassengerRequest(passengerRequest);
            // Act
            var result = await elevator.MoveToNextLevelAsync();

            // Assert
            Assert.Equal(ElevatorDirection.Down, result.Direction);
            Assert.Equal(2, result.CurrentFloor); // Current floor incremented
        }

        [Fact]
        public async Task TestHandlePassengersAsync_ElevatorHasPendingPickups_ExpectTheLoadToIncrease()
        {
            // Arrange
            var elevator = new Elevator(new ElevatorStatus
            {
                Direction = ElevatorDirection.Up,
                CurrentFloor = 1,
                Load = 0,
            });

            var passengerRequest1 = new PassengerRequest { OriginFloorLevel = 2, DestinationFloorLevel = 8, PassengerCount = 2 };
            var passengerRequest2 = new PassengerRequest { OriginFloorLevel = 3, DestinationFloorLevel = 8, PassengerCount = 3 };
            await elevator.QueuePassengerRequest(passengerRequest1);
            await elevator.QueuePassengerRequest(passengerRequest2);
            // Act
            var firstMove = await elevator.MoveToNextLevelAsync();
            Assert.Equal(2, firstMove.Load);

            var secondMove = await elevator.MoveToNextLevelAsync();
            Assert.Equal(5, secondMove.Load);
        }
    }
}
