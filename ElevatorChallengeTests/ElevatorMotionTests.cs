using ElevatorChallenge.Enums;
using ElevatorChallenge.Models;
using ElevatorChallenge.Services.Implementations;

namespace ElevatorChallenge.Tests
{
    public class ElevatorMotionTests
    {
        [Fact]
        public async Task GetElevatorTravelDirectionAsync_ElevatorHasValidStopsInCurrentUpDirection_ShouldReturnUpDirection()
        {
            // Arrange
            var elevatorMotion = new ElevatorMotion();
            var elevator = new ElevatorStatus
            {
                CurrentFloor = 0,
                Direction = ElevatorDirection.Up,
            };
            var passengerRequests = new List<PassengerRequest>
            {
                new PassengerRequest { OriginFloorLevel = 2, DestinationFloorLevel = 5 },
                new PassengerRequest { OriginFloorLevel = 3, DestinationFloorLevel = 1 }
            };

            // Act
            var travelDetails = await elevatorMotion.GetElevatorTravelDetailsAsync(
                elevator, passengerRequests, Enumerable.Empty<PassengerRequest>());

            // Assert
            Assert.Equal(ElevatorDirection.Up, travelDetails.Direction);
        }

        [Fact]
        public async Task GetElevatorTravelDirectionAsync_NoStopsInCurrentDirection_ShouldReturnOppositeDirection_()
        {
            // Arrange
            var elevatorMotion = new ElevatorMotion();
            var elevator = new ElevatorStatus
            {
                CurrentFloor = 5,
                Direction = ElevatorDirection.Up,
            };
            var passengerRequests = new List<PassengerRequest>
            {
                new PassengerRequest { OriginFloorLevel = 2, DestinationFloorLevel = 1 }
            };

            // Act
            var travelDetails = await elevatorMotion.GetElevatorTravelDetailsAsync(
                elevator, passengerRequests, Enumerable.Empty<PassengerRequest>());

            // Assert
            Assert.Equal(ElevatorDirection.Down, travelDetails.Direction);
        }

        [Fact]
        public async Task GetElevatorTravelDirectionAsync_WhenThereNoPassengerRequests_ShouldReturnStationeryDirection()
        {
            // Arrange
            var elevatorMotion = new ElevatorMotion();
            var elevator = new ElevatorStatus
            {
                CurrentFloor = 5,
                Direction = ElevatorDirection.Up,
            };

            // Act
            var travelDetails = await elevatorMotion.GetElevatorTravelDetailsAsync(
                elevator, Enumerable.Empty<PassengerRequest>(), Enumerable.Empty<PassengerRequest>());

            // Assert
            Assert.Equal(ElevatorDirection.None, travelDetails.Direction);
        }
    }
}
