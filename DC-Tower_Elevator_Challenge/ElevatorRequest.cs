using DC_Tower_Elevator_Challenge.Elevator;

namespace DC_Tower_Elevator_Challenge
{
    public class ElevatorRequest
    {
        public int CurrentFloor { get; set; }
        
        public int DestinationFloor { get; set; }
        
        public ElevatorDirection Direction { get; set; }
    }
}