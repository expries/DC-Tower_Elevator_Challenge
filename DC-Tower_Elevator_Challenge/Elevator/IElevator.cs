using System.Collections.Generic;
using DC_Tower_Elevator_Challenge.Elevator.States;

namespace DC_Tower_Elevator_Challenge.Elevator
{
    public interface IElevator
    {
        public int Floor { get; }
        
        public ElevatorDirection Direction { get; set; }

        public IElevatorState State { get; set; }

        public List<int> GetScheduledStops();

        public void Launch();

        public void Dispatch(ElevatorRequest request);

        public void WorkOffRequests();

        public void MoveToFloor(int floor);

        public void OpenDoors();

        public void CloseDoors();

        public void Stop();
    }
}