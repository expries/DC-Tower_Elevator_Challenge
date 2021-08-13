using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using DC_Tower_Elevator_Challenge.Elevator.States;

namespace DC_Tower_Elevator_Challenge.Elevator
{
    public class Elevator : IElevator
    {
        public int Floor { get; private set; }
        
        public ElevatorDirection Direction { get; set; }

        public IElevatorState State { get; set; }

        public readonly ElevatorStates States;

        private readonly BlockingCollection<ElevatorRequest> _requests;

        public Elevator()
        {
            this.States = new ElevatorStates
            {
                Stopped = new StoppedState(this),
                MovingDown = new MovingDownState(this),
                MovingUp = new MovingUpState(this)
            };

            this.State = this.States.Stopped;
            this._requests = new BlockingCollection<ElevatorRequest>(new ConcurrentQueue<ElevatorRequest>(), 1000);
        }

        public List<int> GetScheduledStops()
        {
            var stops = this.States.MovingDown.Stops.Keys.Union(this.States.MovingUp.Stops.Keys);
            return stops.ToList();
        }

        public void Launch()
        {
            while (true)
            {
                this._requests.Take();
                WorkOffRequests();
            }
        }

        public void Dispatch(ElevatorRequest request)
        {
            if (!this._requests.Contains(request))
            {
                this.State.OnNewRequest(request);
                this._requests.Add(request);
            }
        }

        public void WorkOffRequests()
        {
            this.State.OnWorkOffRequests();
        }

        public void OpenDoors()
        {
            this.State.OnDoorsOpen();
        }

        public void CloseDoors()
        {
            this.State.OnDoorsClosed();
        }

        public void Stop()
        {
            this.State.OnStop();
        }

        public void MoveToFloor(int floor)
        {
            if (floor < this.Floor)
            {
                MoveDownToFloor(floor);
            }
            else if (floor > this.Floor)
            {
                MoveUpToFloor(floor);
            }
            else
            {
                this.State.OnFloorReached(floor);
            }
        }

        private void MoveUpToFloor(int floor)
        {
            while (this.Floor < floor)
            {
                Thread.Sleep(250);
                this.Floor++;
                this.State.OnFloorReached(this.Floor);
            }
        }

        private void MoveDownToFloor(int floor)
        {
            while (this.Floor > floor)
            {
                Thread.Sleep(250);
                this.Floor--;
                this.State.OnFloorReached(this.Floor);
            }
        }
    }
}