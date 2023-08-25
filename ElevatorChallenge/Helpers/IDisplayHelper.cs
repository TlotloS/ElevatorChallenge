using ElevatorChallenge.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ElevatorChallenge.Helpers
{
    public interface IDisplayHelper
    {
        void LogErrorToConsole(string input);
        void UpdateOutputSection(IEnumerable<ElevatorStatus> elevatorStatusList);
    }
}
