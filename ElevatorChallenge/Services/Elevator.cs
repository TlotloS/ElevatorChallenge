using ElevatorChallenge.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ElevatorChallenge.Services
{
    public class Elevator : IElevator
    {
        public Task AddPassengerRequest(int originFloor, int destinationFloor, int passengerCount)
        {
            throw new NotImplementedException();
        }

        public Task<ElevatorStatus> GetStatus()
        {
            throw new NotImplementedException();
        }

        public async Task HandlePassengersAsync()
        {
            await Task.Delay(TimeSpan.FromSeconds(2)); // Simulated delay
        }

        public async Task<ElevatorStatus> MoveToNextLevelAsync()
        {
            // Asynchronously move to the next floor
            await Task.Delay(TimeSpan.FromSeconds(1)); // Simulated delay
            return new ElevatorStatus { };
        }
    }
}
