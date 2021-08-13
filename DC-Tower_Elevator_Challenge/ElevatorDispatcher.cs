using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DC_Tower_Elevator_Challenge.Elevator;

namespace DC_Tower_Elevator_Challenge
{
    public class ElevatorDispatcher
    {
        public bool Running { get; set; }

        private readonly List<IElevator> _elevators;

        private readonly BlockingCollection<ElevatorRequest> _requests;

        public ElevatorDispatcher(int elevatorCount)
        {
            this._elevators = new List<IElevator>();
            
            for (int i = 0; i < elevatorCount; i++)
            {
                this._elevators.Add(new Elevator.Elevator());
            }

            this._requests = new BlockingCollection<ElevatorRequest>(new ConcurrentQueue<ElevatorRequest>(), 1000);
        }

        public void AddRequest(ElevatorRequest request)
        {
            if (!this._requests.Contains(request))
            {
                this._requests.Add(request);
            }
        }

        public void Launch()
        {
            this.Running = true;
            LaunchElevators();

            while (this.Running)
            {
                var request = this._requests.Take();
                DispatchElevator(request);
            }
        }
        
        public void PrintElevators()
        {
            for (int i = 0; i < this._elevators.Count; i++)
            {
                var elevator = this._elevators[i];
                
                var sb = new StringBuilder();
                sb.Append($"Elevator {i + 1} ");
                sb.Append($"| Floor: {elevator.Floor} ");
                sb.Append($"| Status: {elevator.State.GetType().Name} ");
                sb.Append($"| Direction: {elevator.Direction} ");
                sb.Append($"| Stops #: {elevator.GetScheduledStops().Count} ");
                sb.Append($"| Stops: {string.Join(",", elevator.GetScheduledStops())} ");
                
                string elevatorInfo = sb.ToString();
                Console.WriteLine(elevatorInfo);
            }
        }

        private void LaunchElevators()
        {
            foreach (var elevator in this._elevators)
            {
                Task.Run(() => elevator.Launch());
            }
        }
        
        private void DispatchElevator(ElevatorRequest request)
        {
            var loadComparison = GetElevatorLoadComparison(request);
            this._elevators.Sort(loadComparison);
            var elevator = this._elevators.First();
            elevator.Dispatch(request);
        }

        private static Comparison<IElevator> GetElevatorLoadComparison(ElevatorRequest request)
        {
            return (x, y) =>
            {
                // elevators which are idles are on top
                if (x.GetScheduledStops().Count == 0)
                {
                    return -1;
                }

                if (y.GetScheduledStops().Count == 0)
                {
                    return 1;
                }

                // next are elevators with lower workload which are also moving in the right direction
                int distanceX = Math.Abs(x.Floor - request.CurrentFloor) + x.GetScheduledStops().Count;
                int distanceY = Math.Abs(x.Floor - request.CurrentFloor) + y.GetScheduledStops().Count;

                if (distanceX < distanceY && x.Direction == request.Direction)
                {
                    return -1;
                }

                if (distanceY < distanceX && y.Direction == request.Direction)
                {
                    return 1;
                }
                
                // prefer elevators that are moving in the right direction
                if (x.Direction == request.Direction && y.Direction != request.Direction)
                {
                    return -1;
                }
                
                if (x.Direction != request.Direction && y.Direction == request.Direction)
                {
                    return 1;
                }
                
                // elevators with lower workload are better
                if (distanceX < distanceY)
                {
                    return -1;
                }

                if (distanceY < distanceX)
                {
                    return 1;
                }

                return 0;
            };
        }
    }
}