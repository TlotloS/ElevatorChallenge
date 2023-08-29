using ElevatorChallenge.Models;
using ElevatorChallenge.Services.Implementations;
using ElevatorChallenge.Services.Interfaces;
using Microsoft.Extensions.Options;
using Moq;

namespace ElevatorChallenge.Tests
{
    public class ControlCentreServiceTests
    {
        /// <summary>
        /// Declare instance for the default control centre object
        /// </summary>
        private readonly IControlCentreService _controlCentreService;
        private readonly Mock<IOptions<ElevatorConfiguration>> _elevatorConfigMock;
        private readonly Mock<IElevatorThreadManager> _threadManager;
        public ControlCentreServiceTests()
        {
            // assign the defaul control centre service object
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

            _threadManager = new Mock<IElevatorThreadManager>();
            _elevatorConfigMock = elevatorConfigOptionsMock;
            _controlCentreService = new ControlCentreService(elevatorConfigOptionsMock.Object);
        }

        [Fact]
        public async Task ControlCentreService_ConstructNewObject_VerifyConfigDetailRequestsAndElevators()
        {
            // arrange 
            var totalFloors = 50;
            var totalElevators = 2;
            var maxElevatorWeight = 8;
            _elevatorConfigMock.Setup(x => x.Value).Returns(new ElevatorConfiguration
            {
                ElevatorMaximumWeight = maxElevatorWeight,
                TotalElevators = totalElevators,
                TotalFloors = totalFloors,
                DelayInSeconds = new DelayConfiguration
                {
                    HandlingPassengers = 5,
                    MovingToNextLevel = 3,
                }
            });
            // act
            var controlCentreService = new ControlCentreService(_elevatorConfigMock.Object);

            // assert
            var config = controlCentreService.GetConfigDetails();
            Assert.NotNull(config);
            Assert.NotNull(_controlCentreService);
            Assert.Equal(totalFloors, config.TotalFloors);
            Assert.Equal(totalElevators, config.TotalElevators);
            Assert.Equal(maxElevatorWeight, config.ElevatorMaximumWeight);

            // test inital state of arrays 
            Assert.Empty(controlCentreService.GetPendingPassengerRequests());
            var elevators = await controlCentreService.GetElevators();
            Assert.Equal(totalElevators, elevators.Count());
        }

        [Fact]
        public void AddPassengerRequest_ValidRequest_ShouldAddRequestToList()
        {
            // Arrange
            var passengerRequest = new PassengerRequest { PassengerCount = 2, DestinationFloorLevel = 1, OriginFloorLevel = 2 };

            // Act
            _controlCentreService.AddPickUpRequest(passengerRequest);

            // Assert
            var pendingRequests = _controlCentreService.GetPendingPassengerRequests();
            Assert.Single(pendingRequests);
            Assert.Contains(passengerRequest, pendingRequests);
        }

        [Fact]
        public void AddPassengerRequest_PassengerCountExceedWeighLimit_ThrowsException()
        {
            // Arrange
            var passengerRequest = new PassengerRequest
            {
                PassengerCount = _elevatorConfigMock.Object.Value.ElevatorMaximumWeight + 1,
                DestinationFloorLevel = 10,
                OriginFloorLevel = 18
            };

            // Act & Assert
            Assert.ThrowsAnyAsync<Exception>(async () => await _controlCentreService.AddPickUpRequest(passengerRequest));
        }

        [Fact]
        public void AddPassengerRequest_InvalidOriginFloorLevel_ThrowsInvalidOperationException()
        {
            // Arrange
            var passengerRequest = new PassengerRequest
            {
                PassengerCount = 1,
                DestinationFloorLevel = _elevatorConfigMock.Object.Value.TotalFloors + 5,
                OriginFloorLevel = 1,
            };

            // Act & Assert
            Assert.ThrowsAnyAsync<InvalidOperationException>(async () => await _controlCentreService.AddPickUpRequest(passengerRequest));
        }

        [Fact]
        public void AddPassengerRequest_InvalidDestinaionFloorLevel_ThrowsInvalidOperationException()
        {
            // Arrange
            var passengerRequest = new PassengerRequest
            {
                PassengerCount = 1,
                DestinationFloorLevel = 1,
                OriginFloorLevel = _elevatorConfigMock.Object.Value.TotalFloors + 5,
            };

            // Act & Assert
            Assert.ThrowsAnyAsync<InvalidOperationException>(async () => await _controlCentreService.AddPickUpRequest(passengerRequest));
        }


        [Fact]
        public void AddPassengerRequest_PDestinationSameAsOrigin_PassengerQueRemainsTheSame()
        {
            // Arrange
            var passengerRequest = new PassengerRequest
            {
                PassengerCount = _elevatorConfigMock.Object.Value.ElevatorMaximumWeight - 1,
                DestinationFloorLevel = 2,
                OriginFloorLevel = 2
            };
            var previousPendingRequests = _controlCentreService.GetPendingPassengerRequests();

            // Act 
            _controlCentreService.AddPickUpRequest(passengerRequest);
            var updatedPendingRequest = _controlCentreService.GetPendingPassengerRequests();
            // Assert
            Assert.Empty(previousPendingRequests);
            Assert.Equal(previousPendingRequests.Count(), updatedPendingRequest.Count());

        }
    }
}