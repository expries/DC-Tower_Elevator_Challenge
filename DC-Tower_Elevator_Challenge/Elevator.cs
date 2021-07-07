using System;
using System.Collections.Concurrent;
using System.Linq;
using System.Threading;

namespace DC_Tower_Elevator_Challenge
{
    public class Elevator
    {
        public int CurrentFloor { get; private set; }
        
        public bool Busy { get; private set; }
        
        public bool Running { get; private set; }
        
        public ElevatorRequest CurrentRequest { get; private set; }

        protected BlockingCollection<ElevatorRequest> Requests { get; }

        protected int Pace { get; set; }

        public Elevator()
        {
            this.CurrentFloor = 0;
            this.Busy = false;
            this.Running = false;
            this.Requests = new BlockingCollection<ElevatorRequest>();
            this.Pace = 500;
        }

        public void Launch()
        {
            this.Running = true;
            
            while (this.Running)
            {
                var request = this.Requests.Take();
                this.CurrentRequest = request;
                ExecuteRequest(request);
                this.CurrentRequest = null;
            }
        }

        public void AddRequest(ElevatorRequest request)
        {
            if (!this.Requests.Contains(request))
            {
                this.Requests.Add(request);
            }
        }
        
        private void ExecuteRequest(ElevatorRequest request)
        {
            this.Busy = true;
            MoveToFloor(request.CurrentFloor);
            MoveToFloor(request.DestinationFloor);
            this.Busy = false;
        }
        
        public int DistanceToFloor(int floor)
        {
            return Math.Abs(this.CurrentFloor - floor);
        }
        
        public int CompareLoadTo(Elevator other)
        {
            if (this.Requests.Count < other.Requests.Count)
            {
                return -1;
            }

            if (this.Requests.Count > other.Requests.Count)
            {
                return 1;
            }
                
            return 0;
        }

        private void MoveToFloor(int floor)
        {
            while (this.CurrentFloor != floor)
            {
                Thread.Sleep(this.Pace);

                if (this.CurrentFloor > floor)
                {
                    this.CurrentFloor--;
                }
                else
                {
                    this.CurrentFloor++;
                }
            }
        }
    }
}