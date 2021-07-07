using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace DC_Tower_Elevator_Challenge
{
    public class ElevatorManager
    {
        public bool Running { get; set; }
        
        protected List<Elevator> Elevators;

        protected BlockingCollection<ElevatorRequest> Requests;

        public ElevatorManager()
        {
            this.Elevators = new List<Elevator>
            {
                new Elevator(),
                new Elevator(),
                new Elevator(),
                new Elevator(),
                new Elevator(),
            };
            
            this.Requests = new BlockingCollection<ElevatorRequest>(new ConcurrentQueue<ElevatorRequest>());
        }

        public void AddRequest(ElevatorRequest request)
        {
            if (!this.Requests.Contains(request))
            {
                this.Requests.Add(request);
            }
        }

        public void Launch()
        {
            this.Running = true;
            LaunchElevators();
            
            while (this.Running)
            {
                var request = this.Requests.Take();
                Task.Run(() => HandleRequest(request));
            }
        }
        
        public void PrintElevators()
        {
            for (int i = 0; i < this.Elevators.Count; i++)
            {
                var elevator = this.Elevators[i];
                Console.WriteLine($"Elevator {i + 1} | Floor: {elevator.CurrentFloor} | Busy: {elevator.Busy}");
            }
        }

        private void LaunchElevators()
        {
            foreach (var elevator in this.Elevators)
            {
                Task.Run(() => elevator.Launch());
            }
        }
        
        private void HandleRequest(ElevatorRequest request)
        {
            var elevators = this.Elevators;

            if (this.Elevators.Any(x => !x.Busy))
            {
                elevators = this.Elevators.Where(x => !x.Busy).ToList();
            }
            
            elevators.Sort((x, y) => x.CompareLoadTo(y));

            var elevator = GetClosestElevator(request, elevators);
            elevator.AddRequest(request);
        }

        private static Elevator GetClosestElevator(ElevatorRequest request, List<Elevator> elevators)
        {
            int minimum = elevators.Min(x => x.DistanceToFloor(request.DestinationFloor));
            return elevators.FirstOrDefault(x => x.DistanceToFloor(request.DestinationFloor) == minimum);
        }
    }
}