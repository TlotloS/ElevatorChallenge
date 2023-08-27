using ElevatorChallenge.Helpers;
using ElevatorChallenge.Models;
using ElevatorChallenge.Services;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using System;

namespace DividedConsoleApp
{
    class Program
    {
        static async Task Main(string[] args)
        {
            Console.WriteLine("Welcome to the Divided Console App!");
            // Configure the
            IConfiguration configuration = new ConfigurationBuilder()
            .SetBasePath(Directory.GetCurrentDirectory())
            .AddJsonFile("appsettings.json")
            .Build();

            // Configure Services and Build DI Container:
            // The ServiceCollection is used to configure and build the ServiceProvider,
            // which is the actual DI container responsible for managing the
            // dependencies in your application.
            var serviceProvider = new ServiceCollection()
            .AddSingleton(configuration)
            .AddScoped<IConsoleInputHelper, ConsoleInputHelper>()
            .AddSingleton<IDisplayHelper, DisplayHelper>()
            .AddSingleton<IElevatorThreadManager, ElevatorThreadManager>()
            .AddSingleton<IControlCentreService, ControlCentreService>()
            // Configure elevator configuration using the options pattern
            .Configure<ElevatorConfiguration>(configuration.GetSection("ElevatorConfiguration"))
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
