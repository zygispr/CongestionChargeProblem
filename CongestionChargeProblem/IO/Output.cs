using System.Text.RegularExpressions;

namespace CongestionChargeProblem;

public class Output
{
    public TimeSpan TimeSpanAM { get; set; }
    public double AMRate { get; set; }
    public TimeSpan TimeSpanPM { get; set; }
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
        AMCharge = Math.Round(AMCharge, 1);
        
        PMCharge = (timeSpanPM.Hours * 60 + timeSpanPM.Minutes) * PMRate / 60;
        PMCharge = Math.Round(PMCharge, 1);

        Console.WriteLine();
        Console.WriteLine(PMCharge);
        Console.WriteLine(Math.Truncate(PMCharge));
        Console.WriteLine();
        
        // Laikinas priskyrimas, kol sugalvosiu kaip skaiciuoti:
        TotalCharge = AMCharge + PMCharge;
    }
}