using ElevatorChallenge.Enums;
using ElevatorChallenge.Helpers;
using ElevatorChallenge.Models;
using ElevatorChallenge.Services;
using Microsoft.Extensions.Options;
using Moq;

namespace ElevatorChallenge.Tests
{
    public class ElevatorTests
    {

        private readonly Mock<IOptions<ElevatorConfiguration>> _elevatorConfigMock;
        public ElevatorTests()
        {
            var elevatorConfigOptionsMock = new Mock<IOptions<ElevatorConfiguration>>();
            elevatorConfigOptionsMock.Setup(x => x.Value).Returns(new ElevatorConfiguration
            {
                ElevatorMaximumWeight = 5,
                TotalElevators = 5,
                TotalFloors = 5,
                DelayInSeconds = new DelayConfiguration
                {
                    HandlingPassengers = 5,
                    MovingToNextLevel = 3,
                }
            });
            _elevatorConfigMock = elevatorConfigOptionsMock;
        }

        [Fact]
        public async Task TestMoveToNextLevelAsync_NoPendingRequests_ReturnsCurrentStatus()
        {
            // Arrange
            var elevator = new Elevator(new ElevatorStatus
            {
                Direction = ElevatorDirection.None,
                CurrentFloor = 0
            }, _elevatorConfigMock.Object.Value);

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
            var elevator = new Elevator(1, _elevatorConfigMock.Object.Value);
            // assert
            var status = elevator.CurrentStatus;
            Assert.Equal(0, status.CurrentFloor);
            Assert.Equal(ElevatorDirection.None, status.Direction);
            Assert.False(string.IsNullOrEmpty(status.Name));
        }

        [Fact]
        public async Task TestMoveToNextLevelAsync_InitialStateIsStationAndHasPendingRequests_ExpectDirectionSetAndFloorIncremented()
        {
            // Arrange
            var elevator = new Elevator(new ElevatorStatus
            {
                Direction = ElevatorDirection.None,
                CurrentFloor = 0
            }, _elevatorConfigMock.Object.Value);

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
            }, _elevatorConfigMock.Object.Value);

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
            }, _elevatorConfigMock.Object.Value);

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
