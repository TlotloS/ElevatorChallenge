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
            .AddScoped<IConsoleInputHelper, ConsoleInputHelper>()
            .AddSingleton<IDisplayHelper, DisplayHelper>()
            .AddSingleton<IElevatorThreadManager, ElevatorThreadManager>()
            .AddSingleton<IControlCentreService, ControlCentreService>()
            .AddSingleton<ElevatorSystemConfig>(provider =>
            {
                // Create and return an instance of ElevatorSystemConfig here
                return new ElevatorSystemConfig(
                    // Initialize properties as needed
                    8,          // floors count 
                    4,         // elevators count
                    8      // max weight/peopel
                 );
            })
            .BuildServiceProvider();
            try
            {
                // Get an instance of your ConsoleInputHelper to start the printing task
                var consoleInputHelper = serviceProvider.GetRequiredService<IConsoleInputHelper>();
                await consoleInputHelper.StartPrintingTaskAsync();
                await consoleInputHelper.StartElevatorThreads();
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
    }
}
