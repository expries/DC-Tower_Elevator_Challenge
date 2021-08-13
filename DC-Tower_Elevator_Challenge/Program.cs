using System;
using System.Threading;
using System.Threading.Tasks;
using DC_Tower_Elevator_Challenge.Elevator;

namespace DC_Tower_Elevator_Challenge
{
    class Program
    {
        static void Main(string[] args)
        {
            DoStuff();
        }

        public static async Task DoStuff()
        {
            var dispatcher = new ElevatorDispatcher(5);
            Task.Run(() => dispatcher.Launch());

            /*
            var requestA = new ElevatorRequest
            {
                Direction = ElevatorDirection.Up,
                CurrentFloor = 2,
                DestinationFloor = 4
            };
            
            var requestB = new ElevatorRequest
            {
                Direction = ElevatorDirection.Down,
                CurrentFloor = 10,
                DestinationFloor = 1
            };
            
            var requestC = new ElevatorRequest
            {
                Direction = ElevatorDirection.Down,
                CurrentFloor = 8,
                DestinationFloor = 5
            };
            
            dispatcher.AddRequest(requestA);
            dispatcher.AddRequest(requestB);
            dispatcher.AddRequest(requestC);
            */
            
            for (int i = 0; i < 100; i++)
            {
                int currentFloor = new Random().Next(0, 55);
                int destinationFloor = new Random().Next(0, 55);
                var direction = currentFloor > destinationFloor ? ElevatorDirection.Down : ElevatorDirection.Up;
                
                var request = new ElevatorRequest
                {
                    Direction = direction, 
                    CurrentFloor = currentFloor, 
                    DestinationFloor = destinationFloor
                };

                dispatcher.AddRequest(request);
            }

            while (dispatcher.Running)
            {
                dispatcher.PrintElevators();
                Thread.Sleep(200);
                Console.Clear();
            }
        }
    }
}