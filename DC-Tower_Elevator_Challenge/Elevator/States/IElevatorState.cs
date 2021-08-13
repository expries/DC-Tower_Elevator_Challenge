namespace DC_Tower_Elevator_Challenge.Elevator.States
{
    public interface IElevatorState
    {
        public void OnNewRequest(ElevatorRequest request);

        public void OnWorkOffRequests();

        public void OnFloorReached(int floor);
        
        public void OnStop();

        public void OnDoorsOpen();

        public void OnDoorsClosed();
    }
}