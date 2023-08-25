using ElevatorChallenge.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ElevatorChallenge.Helpers
{
    public class ConsoleInputHelper : IConsoleInputHelper
    {
        public PassengerRequest ConvertInputToRequest(string input)
        {
            string[] parts = input.Split(';');

            if (parts.Length != 3)
            {
                throw new ArgumentException("Invalid input format");
            }

            int originFloor = int.Parse(parts[0]);
            int destinationFloor = int.Parse(parts[1]);
            int passengers = int.Parse(parts[2]);

            return new PassengerRequest
            {
                OrginFloorLevel = originFloor,
                DestinationFloorLevel = destinationFloor,
                PassengerCount = passengers
            };
        }
        public void UpdateInputSection()
        {
            Console.SetCursorPosition(0, 1); // Move cursor to a specific position12
            Console.WriteLine("Input Section (originFloor; destinationFloor; passengers):");
            Console.SetCursorPosition(0, 2);
            string input = Console.ReadLine();
            Console.WriteLine(input);
            // Process input or perform actions based on input
        }

        public void UpdateOutputSection()
        {
            Console.SetCursorPosition(0, 10);
            Console.WriteLine("Output Section:");
            Console.SetCursorPosition(0, 11);
            // Perform output logic or display information
            Console.WriteLine("Output data goes here...");
        }
    }
}
