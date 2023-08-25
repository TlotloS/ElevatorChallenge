using ElevatorChallenge.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ElevatorChallenge.Helpers
{
    internal interface IConsoleInputHelper 
    {
        public PassengerRequest ConvertInputToRequest(string input);
        public void UpdateInputSection();
        public void UpdateOutputSection();

    }
}
