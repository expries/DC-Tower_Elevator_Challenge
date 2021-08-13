using System;
using System.Threading;

namespace DC_Tower_Elevator_Challenge.Elevator.States
{
    public class StoppedState : IElevatorState
    {
        public readonly Elevator Elevator;

        public StoppedState(Elevator elevator)
        {
            this.Elevator = elevator;
        }
        
        public void OnNewRequest(ElevatorRequest request)
        {
            this.Elevator.State = GetMovingStateForDirection(request.Direction);
            this.Elevator.Direction = request.Direction;
            this.Elevator.Dispatch(request);
        }
        
        public void OnWorkOffRequests()
        {
            this.Elevator.State = this.Elevator.States.Stopped;
            this.Elevator.Stop();
        }

        public void OnFloorReached(int floor)
        {
            throw new InvalidOperationException("Elevator should not move while in stopped mode.");
        }
        
        public void OnStop()
        {
            this.Elevator.OpenDoors();
            this.Elevator.CloseDoors();
            this.Elevator.State = GetMovingStateForDirection(this.Elevator.Direction);
        }
        
        public void OnDoorsOpen()
        {
            Thread.Sleep(2500);
        }
        
        public void OnDoorsClosed()
        {
            Thread.Sleep(500);
        }

        private IElevatorState GetMovingStateForDirection(ElevatorDirection direction)
        {
            if (direction is ElevatorDirection.Down)
            {
                return this.Elevator.States.MovingDown;
            }

            return this.Elevator.States.MovingUp;
        }
    }
}