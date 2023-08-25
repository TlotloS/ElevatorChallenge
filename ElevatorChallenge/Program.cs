using ElevatorChallenge.Models;
using System;

namespace DividedConsoleApp
{
    class Program
    {
        
        public IEnumerable<PassengerRequest> Requests { get; set; }
        static void Main(string[] args)
        {
            Console.WriteLine("Welcome to the Divided Console App!");
            // Create a loop to continuously update sections
            while (true)
            {
                Console.Clear(); // Clear the entire console

                // Update and display different sections
                //UpdateInputSection();
                //UpdateOutputSection();

                // Sleep to control the refresh rate
                Thread.Sleep(1000);
            }
        }
    }
}
