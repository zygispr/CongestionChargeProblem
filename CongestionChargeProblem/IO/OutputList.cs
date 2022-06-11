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
            // "timeVar" is the main variable that is used in calculations below.
            // It is equal to the input's StartTime at the beginning.
            // The while cycle continues until timeVar doesn't reach the input's EndTime.
            DateTime timeVar = input.StartTime; 
            
            TimeSpan timeSpanAM = TimeSpan.Zero; // Represents the time span that passed while the AM rate was active
            TimeSpan timeSpanPM = TimeSpan.Zero; // Represents the time span that passed while the PM rate was active
            TimeSpan timeDifference = new TimeSpan(); // A variable time span that is used in various calculations below

            // These variables represent the hour marks when the congestion charge rate changes
            // (it is possible to change the hour marks):
            TimeOnly morningHourMark = new TimeOnly(7, 0);
            TimeOnly afternoonHourMark = new TimeOnly(12, 0);
            TimeOnly eveningHourMark = new TimeOnly(19, 0);

            // Console.WriteLine("Start:");
            // Console.WriteLine(timeVar.ToString("dd/MM/yyyy HH:mm"));
            // Console.WriteLine($"timeSpanAM: {timeSpanAM}");
            // Console.WriteLine($"timeSpanPM: {timeSpanPM}");
            // Console.WriteLine();
            
            int addHours;
            int addMinutes;

            while(timeVar < input.EndTime)
                // for (int i = 0; i < 2; i++)
            {
                if (timeVar.DayOfWeek == DayOfWeek.Saturday || timeVar.DayOfWeek == DayOfWeek.Sunday)
                {
                    // Console.WriteLine("weekend");
                    
                    if (timeVar.Hour < morningHourMark.Hour)
                    {
                        addHours = morningHourMark.Hour - timeVar.Hour;
                    }
                    else
                    {
                        addHours = 24 + morningHourMark.Hour - timeVar.Hour;
                    }
                    
                    addMinutes = 60 - timeVar.Minute;
                    if (addMinutes > 0)
                    {
                        addHours--;
                    }

                    timeVar = timeVar.Add(new TimeSpan(addHours, addMinutes, 0));
                } else if (timeVar.Hour >= eveningHourMark.Hour)
                {
                    // Console.WriteLine($">={eveningHourMark.Hour}");
                    
                    addHours = 24 + morningHourMark.Hour - timeVar.Hour;
                    addMinutes = 60 - timeVar.Minute;
                    if (addMinutes > 0)
                    {
                        addHours--;
                    }

                    timeVar = timeVar.Add(new TimeSpan(addHours, addMinutes, 0));
                } else if (timeVar.Hour >= afternoonHourMark.Hour)
                {
                    // Console.WriteLine($">={afternoonHourMark.Hour}");

                    if ((input.EndTime.DayOfYear > timeVar.DayOfYear && input.EndTime.Year == timeVar.Year) ||  TimeOnly.FromDateTime(input.EndTime) > eveningHourMark || input.EndTime.Year > timeVar.Year)
                    {
                        timeDifference = eveningHourMark - TimeOnly.FromDateTime(timeVar);
                    }
                    else
                    {
                        timeDifference = input.EndTime - timeVar;
                    }
                    
                    timeSpanPM += timeDifference;
                    timeVar = timeVar.Add(timeDifference);
                } else if (timeVar.Hour >= morningHourMark.Hour)
                {
                    // Console.WriteLine($">={morningHourMark.Hour}");
                    
                    if ((input.EndTime.DayOfYear > timeVar.DayOfYear && input.EndTime.Year == timeVar.Year) || TimeOnly.FromDateTime(input.EndTime) > afternoonHourMark || input.EndTime.Year > timeVar.Year)
                    {
                        timeDifference = afternoonHourMark - TimeOnly.FromDateTime(timeVar);
                    }
                    else
                    {
                        timeDifference = input.EndTime - timeVar;
                    }
                    
                    timeSpanAM += timeDifference;
                    timeVar = timeVar.Add(timeDifference);
                } else if (timeVar.Hour >= 0)
                {
                    // Console.WriteLine(">=0");

                    timeDifference = morningHourMark - TimeOnly.FromDateTime(timeVar);
                    timeVar = timeVar.Add(timeDifference);
                }
                
                // Console.WriteLine(timeVar.ToString("dd/MM/yyyy HH:mm"));
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