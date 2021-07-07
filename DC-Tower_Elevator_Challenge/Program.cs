using System;
using System.Threading;
using System.Threading.Tasks;

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
            var manager = new ElevatorManager();
            Task.Run(() => manager.Launch());
            
            manager.AddRequest(new ElevatorRequest
            {
                Direction = ElevatorDirection.Up, CurrentFloor = 0, DestinationFloor = 5
            });
            
            manager.AddRequest(new ElevatorRequest
            {
                Direction = ElevatorDirection.Up, CurrentFloor = 0, DestinationFloor = 43
            });
            
            manager.AddRequest(new ElevatorRequest
            {
                Direction = ElevatorDirection.Up, CurrentFloor = 0, DestinationFloor = 3
            });
            
            manager.AddRequest(new ElevatorRequest
            {
                Direction = ElevatorDirection.Up, CurrentFloor = 0, DestinationFloor = 2
            });
            
            manager.AddRequest(new ElevatorRequest
            {
                Direction = ElevatorDirection.Up, CurrentFloor = 0, DestinationFloor = 6
            });
            
            manager.AddRequest(new ElevatorRequest
            {
                Direction = ElevatorDirection.Up, CurrentFloor = 0, DestinationFloor = 43
            });
            
            manager.AddRequest(new ElevatorRequest
            {
                Direction = ElevatorDirection.Up, CurrentFloor = 0, DestinationFloor = 43
            });

            manager.AddRequest(new ElevatorRequest
            {
                Direction = ElevatorDirection.Up, CurrentFloor = 0, DestinationFloor = 14
            });
            
            manager.AddRequest(new ElevatorRequest
            {
                Direction = ElevatorDirection.Up, CurrentFloor = 0, DestinationFloor = 14
            });
            
            manager.AddRequest(new ElevatorRequest
            {
                Direction = ElevatorDirection.Down, CurrentFloor = 12, DestinationFloor = 0
            });
            
            manager.AddRequest(new ElevatorRequest
            {
                Direction = ElevatorDirection.Up, CurrentFloor = 0, DestinationFloor = 2
            });
            
            manager.AddRequest(new ElevatorRequest
            {
                Direction = ElevatorDirection.Up, CurrentFloor = 0, DestinationFloor = 44
            });

            while (manager.Running)
            {
                manager.PrintElevators();
                Thread.Sleep(250);
                Console.Clear();
            }
        }
    }
}