namespace CongestionChargeProblem;

public class Input
{
    public string Name { get; init; }
    public DateTime StartTime { get; set; }
    public DateTime EndTime { get; set; }
    public VehicleType VehicleType { get; init; }

    public Input(string name, DateTime startTime, DateTime endTime)
    {
        Name = name;
        StartTime = startTime;
        EndTime = endTime;

        if (Name.ToLower() == "motorbike")
        {
            VehicleType = VehicleType.Motorbike;
        }
        else
        {
            VehicleType = VehicleType.Car;
        }
    }
}