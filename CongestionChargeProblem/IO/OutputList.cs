using System.Text;

namespace CongestionChargeProblem;

public class OutputList
{
    public List<Output> Outputs { get; set; }

    public OutputList(InputList inputList)
    {
        Outputs = new List<Output>();
        
        int index = 1;

        foreach (var input in inputList.Inputs)
        {
            TimeSpan timeSpanAM = TimeSpan.Zero;
            TimeSpan timeSpanPM = TimeSpan.Zero;
            TimeSpan timeDifference = new TimeSpan();

            TimeOnly sevenAm = new TimeOnly(7, 0);
            TimeOnly twelvePm = new TimeOnly(12, 0);
            TimeOnly sevenPm = new TimeOnly(19, 0);

            // Console.WriteLine("Start:");
            // Console.WriteLine(input.StartTime.ToString("dd/MM/yyyy HH:mm"));
            // Console.WriteLine($"timeSpanAM: {timeSpanAM}");
            // Console.WriteLine($"timeSpanPM: {timeSpanPM}");
            // Console.WriteLine();

            for (int i = 0; i < 2; i++)
            {
                if (input.StartTime.DayOfWeek == DayOfWeek.Saturday || input.StartTime.DayOfWeek == DayOfWeek.Sunday)
                {
                    Console.WriteLine("weekend");
                } else if (input.StartTime.Hour >= 19)
                {
                    Console.WriteLine(">=19");
                    input.StartTime = input.StartTime.AddHours(12);

                } else if (input.StartTime.Hour >= 12)
                {
                    Console.WriteLine(">=12");

                    if (TimeOnly.FromDateTime(input.EndTime) > sevenPm)
                    {
                        timeDifference = sevenPm - TimeOnly.FromDateTime(input.StartTime);
                    }
                    else
                    {
                        timeDifference = input.EndTime - input.StartTime;
                    }
                    
                    timeSpanPM += timeDifference;
                    input.StartTime = input.StartTime.Add(timeDifference);
                } else if (input.StartTime.Hour >= 7)
                {
                    Console.WriteLine(">=7");

                    if (TimeOnly.FromDateTime(input.EndTime) > twelvePm)
                    {
                        timeDifference = twelvePm - TimeOnly.FromDateTime(input.StartTime);
                    }
                    else
                    {
                        timeDifference = input.EndTime - input.StartTime;
                    }
                    
                    timeSpanAM += timeDifference;
                    input.StartTime = input.StartTime.Add(timeDifference);
                }
                
                // Console.WriteLine(input.StartTime.ToString("dd/MM/yyyy HH:mm"));
                // Console.WriteLine($"timeSpanAM: {timeSpanAM}");
                // Console.WriteLine($"timeSpanPM: {timeSpanPM}");
                // Console.WriteLine();
            }

            Outputs.Add(new Output(timeSpanAM, timeSpanPM, input.VehicleType));
        }
    }

    public override string ToString()
    {
        int index = 1;
        StringBuilder sb = new StringBuilder();

        foreach (var output in Outputs)
        {
            sb.Append($"OUTPUT {index}\n");
            sb.Append($"Charge for {output.TimeSpanAM.Hours}h {output.TimeSpanAM.Minutes}m (AM rate): £{output.AMCharge.ToString("0.00")}\n");
            sb.Append($"Charge for {output.TimeSpanPM.Hours}h {output.TimeSpanPM.Minutes}m (PM rate): £{output.PMCharge.ToString("0.00")}\n");
            sb.Append($"Total Charge: £{output.TotalCharge.ToString("0.00")}\n");
            index++;
        }

        return sb.ToString();
    }
}