using ElevatorChallenge.Models;
using ElevatorChallenge.Services;
using Moq;

namespace ElevatorChallengeTests
{
    public class ControlCentreServiceTests
    {
        /// <summary>
        /// Declare instance for the default control centre object
        /// </summary>
        private readonly IControlCentreService _controlCentreService;
        private readonly ElevatorSystemConfig _elevatorSystemConfig;
        public ControlCentreServiceTests()
        {
            // assign the defaul control centre service object
            _elevatorSystemConfig = new ElevatorSystemConfig(5, 1, 6);
            _controlCentreService = new ControlCentreService(
                _elevatorSystemConfig.FloorsCount, // 5
                _elevatorSystemConfig.ElevatorsCount, // 1
                _elevatorSystemConfig.MaxWeight); // 6
        }

        [Fact]
        public void ControlCentreService_ConstructNewObject_VerifyConfigDetailRequestsAndElevators()
        {
            // arrange 
            var totalFloors = 50;
            var totalElevators = 2;
            var maxElevatorWeight = 8;
            // act
            var controlCentreService = new ControlCentreService(totalFloors, totalElevators, maxElevatorWeight);

            // assert
            var config = controlCentreService.GetConfigDetails();
            Assert.NotNull(config);
            Assert.NotNull(_controlCentreService);
            Assert.Equal(totalFloors, config.FloorsCount);
            Assert.Equal(totalElevators, config.ElevatorsCount);
            Assert.Equal(maxElevatorWeight, config.MaxWeight);

            // test inital state of arrays 
            Assert.Empty(controlCentreService.GetPendingPassengerRequests());
            Assert.Equal(totalElevators, controlCentreService.GetElevators().Count());
        }

        [Fact]
        public void AddPassengerRequest_ValidRequest_ShouldAddRequestToList()
        {
            // Arrange
            var passengerRequest = new PassengerRequest { PassengerCount = 2, DestinationFloorLevel = 10, OriginFloorLevel = 18 };

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
                PassengerCount = _elevatorSystemConfig.MaxWeight + 1,
                DestinationFloorLevel = 10,
                OriginFloorLevel = 18
            };

            // Act & Assert
            Assert.ThrowsAny<Exception>(() => _controlCentreService.AddPickUpRequest(passengerRequest));
        }

        [Fact]
        public void AddPassengerRequest_PDestinationSameAsOrigin_PassengerQueRemainsTheSame()
        {
            // Arrange
            var passengerRequest = new PassengerRequest
            {
                PassengerCount = _elevatorSystemConfig.MaxWeight - 1,
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