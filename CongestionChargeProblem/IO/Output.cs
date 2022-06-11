namespace CongestionChargeProblem;

public class Output
{
    public TimeSpan TimeSpanAM { get; set; }
    public TimeSpan TimeSpanPM { get; set; }
    public double AMRate { get; set; }
    public double PMRate { get; set; }
    public double AMCharge { get; set; }
    public double PMCharge { get; set; }
    public double TotalCharge { get; set; }

    public Output(TimeSpan timeSpanAM, TimeSpan timeSpanPM, VehicleType vehicleType)
    {
        TimeSpanAM = timeSpanAM;
        TimeSpanPM = timeSpanPM;

        if (vehicleType == VehicleType.Motorbike)
        {
            AMRate = 1;
            PMRate = 1;
        }
        else
        {
            AMRate = 2;
            PMRate = 2.5;
        }

        AMCharge = (timeSpanAM.Hours * 60 + timeSpanAM.Minutes) * AMRate / 60;
        AMCharge = Math.Floor(AMCharge * 10) / 10;
        
        PMCharge = (timeSpanPM.Hours * 60 + timeSpanPM.Minutes) * PMRate / 60;
        PMCharge = Math.Floor(PMCharge * 10) / 10;
        
        TotalCharge = AMCharge + PMCharge;
    }
}