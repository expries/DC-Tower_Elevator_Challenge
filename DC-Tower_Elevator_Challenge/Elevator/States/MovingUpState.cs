using System;
using System.Collections.Generic;
using System.Linq;

namespace DC_Tower_Elevator_Challenge.Elevator.States
{
    public class MovingUpState : IElevatorState
    {
        public readonly Elevator Elevator;
        
        public Dictionary<int, int?> Stops { get; } = new Dictionary<int, int?>();
        
        public MovingUpState(Elevator elevator)
        {
            this.Elevator = elevator;
        }
        
        public void OnNewRequest(ElevatorRequest request)
        {
            this.Stops.Add(request.CurrentFloor, null);
            this.Stops.Add(request.DestinationFloor, request.CurrentFloor);
        }

        public void OnWorkOffRequests()
        {
            // collect passengers from lower floors that want to get up
            while (this.Stops.Any(kv => this.Elevator.Floor > kv.Key))
            {
                int destinationFloor = this.Stops.Select(kv => kv.Key).Max();
                this.Elevator.MoveToFloor(destinationFloor);
            }
            
            // move passengers up (and stop on the way if requested)
            while (this.Stops.Any(kv => this.Elevator.Floor <= kv.Key))
            {
                var stop = this.Stops.FirstOrDefault(kv => this.Elevator.Floor <= kv.Key);
                int floor = stop.Key;
                this.Elevator.MoveToFloor(floor);
            }

            this.Elevator.State = this.Elevator.States.Stopped;
        }

        public void OnFloorReached(int floor)
        {
            // check if floor is on stop list
            if (!this.Stops.ContainsKey(floor))
            {
                return;
            }

            // stop if passengers are collected or are getting out
            int? firstStop = this.Stops[floor];

            if (firstStop != null && this.Stops.ContainsKey(firstStop.Value))
            {
                return;
            }

            // collect passengers and remove stop from stop list
            this.Elevator.State = this.Elevator.States.Stopped;
            this.Elevator.Stop();
            this.Stops.Remove(floor);
        }
        
        public void OnStop()
        {
            throw new InvalidOperationException("Elevator should not be stop while moving up.");
        }

        public void OnDoorsOpen()
        {
            this.Elevator.CloseDoors();
        }
        
        public void OnDoorsClosed()
        {
            this.Elevator.WorkOffRequests();
        }
    }
}