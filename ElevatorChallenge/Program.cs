using ElevatorChallenge.Helpers;
using ElevatorChallenge.Models;
using ElevatorChallenge.Services;
using Microsoft.Extensions.DependencyInjection;
using System;

namespace DividedConsoleApp
{
    class Program
    {
        static async Task Main(string[] args)
        {
            Console.WriteLine("Welcome to the Divided Console App!");
            // Configure dependancy injection service provider
            var serviceProvider = new ServiceCollection()
            .AddScoped<IControlCentreService, ControlCentreService>()
            .AddScoped<IConsoleInputHelper, ConsoleInputHelper>()
            .AddSingleton<IElevatorThreadManager, ElevatorThreadManager>()
            .AddSingleton<ElevatorSystemConfig>(provider =>
            {
                // Create and return an instance of ElevatorSystemConfig here
                return new ElevatorSystemConfig(
                    // Initialize properties as needed
                    3,          // floors count 
                    4,         // elevators count
                    100      // max weight/peopel
                 );
            })
            .AddScoped<IElevator, Elevator>()
            .BuildServiceProvider();

            // Get an instance of your ConsoleInputHelper to start the printing task
            var consoleInputHelper = serviceProvider.GetRequiredService<IConsoleInputHelper>();
            await consoleInputHelper.StartPrintingTaskAsync(); 
            await consoleInputHelper.StartElevatorThreads();
        }
    }
}
