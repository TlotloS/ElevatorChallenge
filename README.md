# Elevator Control App

Welcome to the Elevator Control App! This cool app lets you play around with elevators using a simple console interface. You can boss them around, check their status, and send them on wild rides between floors.

## Table of Contents

- [Get Started](#get-started)
- [Features](#features)
- [Configuration](#configuration)
- [Installation](#installation)
- [How to Use](#how-to-use)
- [Tests](#tests)
- [Contribute](#contribute)
- [License](#license)

## Get Started

Ready to hop in the Elevator Control App? Here’s what to do:

1. Grab the code by cloning this repository.
2. Open up the solution in your favorite coding spot (could be Visual Studio, VS Code, or anything else).
3. Hit that build button to make sure everything’s set.
4. Fire up the app and have some elevator fun!

## Features

- Simulate elevators in motion.
- Watch elevator status updates in real-time.
- Give elevator tasks by sending passenger requests.
- Enjoy a tidy and straightforward console user interface.
- **Unit Tests**: We didn’t stop at just making things work – we made sure they keep working. Our app comes with a bunch of tests to prove it!

## Configuration

Want to fine-tune the Elevator Control App? You can do it using the `appsettings.json` file. Adjust things like the number of elevators, floors, weight limits, and even the time it takes to handle passengers and move to the next floor.

To customize the app:

1. Open `appsettings.json` in the app’s root folder.
2. Find the `"ElevatorConfiguration"` section:

   ```json
   "ElevatorConfiguration": {
     "TotalElevators": 5,
     "TotalFloors": 5,
     "ElevatorMaximumWeight": 10,
     "DelayInSeconds": {
       "HandlingPassengers": 5,
       "MovingToNextLevel": 5
     }
   }
