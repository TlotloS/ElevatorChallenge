using ElevatorChallenge.Enums;
using ElevatorChallenge.Models;
using ElevatorChallenge.Services.Implementations;
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
                    HandlingPassengers = 0,
                    MovingToNextLevel = 0,
                }
            });
            _elevatorConfigMock = elevatorConfigOptionsMock;
        }

        [Fact]
        public async Task MoveToNextLevelAsync_NoPendingRequests_ReturnsCurrentStatus()
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
        public async Task MoveToNextLevelAsync_InitialStateIsStationAndHasPendingRequests_ExpectDirectionSetAndFloorIncremented()
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
        public async Task MoveToNextLevelAsync_NewPassengerRequestIsAddedOnTheSameFloorAsTheElevator_ElevatorShouldProcessPassengers()
        {
            // Arrange
            var elevator = new Elevator(new ElevatorStatus
            {
                Direction = ElevatorDirection.None,
                CurrentFloor = 0
            }, _elevatorConfigMock.Object.Value);

            var passengerRequest = new PassengerRequest { OriginFloorLevel = 0, DestinationFloorLevel = 5, PassengerCount = 2 };
            await elevator.QueuePassengerRequest(passengerRequest);

            // Act
            var result = await elevator.MoveToNextLevelAsync();

            // Assert
            Assert.Equal(2, result.Load);
            Assert.Equal(ElevatorDirection.Up, result.Direction);
        }

        [Fact]
        public async Task HandlePassengersAsync_DirectionIsSetToUpWithPendingRequestInOppositeDirection_ExpectThatDirectionWillBeRevertedToDown()
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
        public async Task HandlePassengersAsync_ElevatorHasPendingPickupsOn_ExpectTheLoadToIncrease()
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
            Assert.Equal(0, firstMove.Load);

            var secondMove = await elevator.MoveToNextLevelAsync();
            Assert.Equal(2, secondMove.Load);

            var thirdMove = await elevator.MoveToNextLevelAsync();
            Assert.Equal(5, thirdMove.Load);
        }

        [Fact]
        public async Task MoveToNextLevelAsync_RequestMadeAtTheElevatorsCurrentFloor_EnsureThatElevatorTravelAccording()
        {
            // Arrange
            var elevator = new Elevator(new ElevatorStatus
            {
                Direction = ElevatorDirection.Up,
                CurrentFloor = 0,
                Load = 0,
            }, _elevatorConfigMock.Object.Value);

            var passengerRequest = new PassengerRequest { OriginFloorLevel = 0, DestinationFloorLevel = 8, PassengerCount = 6 };
            await elevator.QueuePassengerRequest(passengerRequest);
            // Act
            var firstMove = await elevator.MoveToNextLevelAsync();
            Assert.Equal(6, firstMove.Load);
            Assert.Equal(1, firstMove.CurrentFloor);
            Assert.Equal(ElevatorDirection.Up, firstMove.Direction);

            var secondMove = await elevator.MoveToNextLevelAsync();
            Assert.Equal(6, secondMove.Load);
            Assert.Equal(2, secondMove.CurrentFloor);
            Assert.Equal(ElevatorDirection.Up, secondMove.Direction);

            var thirdMove = await elevator.MoveToNextLevelAsync();
            Assert.Equal(6, thirdMove.Load);
            Assert.Equal(3, thirdMove.CurrentFloor);
            Assert.Equal(ElevatorDirection.Up, thirdMove.Direction);
        }

        [Fact]
        public async Task MoveToNextLevelAsync_ElevatorArrivesAtRequestDestinationLevel_TheElevatorLoadMustDecrease()
        {
            // Arrange
            var elevator = new Elevator(new ElevatorStatus
            {
                Direction = ElevatorDirection.Up,
                CurrentFloor = 0,
                Load = 0,
            }, _elevatorConfigMock.Object.Value);

            var passengerRequest = new PassengerRequest { OriginFloorLevel = 0, DestinationFloorLevel = 1, PassengerCount = 6 };
            await elevator.QueuePassengerRequest(passengerRequest);
            // Act
            var firstMove = await elevator.MoveToNextLevelAsync(); // picks up passenger & moves to next floor
            Assert.Equal(6, firstMove.Load);
            Assert.Equal(1, firstMove.CurrentFloor);
            Assert.Equal(ElevatorDirection.Up, firstMove.Direction);

            var secondMove = await elevator.MoveToNextLevelAsync(); // drops of passengers & set direction to stationery
            Assert.Equal(0, secondMove.Load);
            Assert.Equal(ElevatorDirection.None, secondMove.Direction);
        }

        [Fact]
        public async Task MoveToNextLevelAsync_TestForElevatorGoingUpToPickupPassengerThatWantToComeDown_TheElevatorMustBeginToComeDownAfterPickup()
        {
            // Arrange
            var elevator = new Elevator(new ElevatorStatus
            {
                Direction = ElevatorDirection.Up,
                CurrentFloor = 0,
                Load = 0,
            }, _elevatorConfigMock.Object.Value);

            var passengerRequest = new PassengerRequest { OriginFloorLevel = 2, DestinationFloorLevel = 0, PassengerCount = 6 };
            await elevator.QueuePassengerRequest(passengerRequest);
            // Act
            var firstMove = await elevator.MoveToNextLevelAsync(); // moves to 1st floor
            Assert.Equal(0, firstMove.Load);
            Assert.Equal(1, firstMove.CurrentFloor);
            Assert.Equal(ElevatorDirection.Up, firstMove.Direction);

            var secondMove = await elevator.MoveToNextLevelAsync(); // moves to 2nd floor
            Assert.Equal(0, firstMove.Load);
            Assert.Equal(2, firstMove.CurrentFloor);
            Assert.Equal(ElevatorDirection.Up, firstMove.Direction);

            var third = await elevator.MoveToNextLevelAsync(); // picks up passenger and begins to down
            Assert.Equal(6, secondMove.Load);
            Assert.Equal(1, firstMove.CurrentFloor);
            Assert.Equal(ElevatorDirection.Down, firstMove.Direction);
        }
    }
}
