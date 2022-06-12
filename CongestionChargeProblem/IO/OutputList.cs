using System.ComponentModel;
using System.Text;
using OpenQA.Selenium.DevTools.V100;

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
            // It is equal to the INPUT's StartTime at the beginning.
            // The while cycle below continues until timeVar doesn't reach the INPUT's EndTime.
            DateTime timeVar = input.StartTime;
            
            TimeSpan timeSpanAM = TimeSpan.Zero; // Represents the time span that passed while the AM rate was active
            TimeSpan timeSpanPM = TimeSpan.Zero; // Represents the time span that passed while the PM rate was active
            
            // These variables represent the hour marks when the congestion charge rate changes
            // (it is possible to change them):
            int morningHourMark = 7;
            int afternoonHourMark = 12;
            int eveningHourMark = 19;
            
            // These variables represent how many hours or minutes need to be added to timeSpanAM/timeSpanPM/timeVar:
            int addHours = 0;
            int addMinutes = 0;
            TimeSpan timeDifference = new TimeSpan(addHours, addMinutes, 0);

            Console.WriteLine("Start:");
            Console.WriteLine(timeVar.ToString("dd/MM/yyyy HH:mm"));
            Console.WriteLine($"timeSpanAM: {timeSpanAM}");
            Console.WriteLine($"timeSpanPM: {timeSpanPM}");
            Console.WriteLine();

            while(timeVar < input.EndTime)
            {
                if (timeVar.DayOfWeek == DayOfWeek.Saturday || timeVar.DayOfWeek == DayOfWeek.Sunday)
                {
                    Console.WriteLine("weekend");
                    
                    if (timeVar.Hour < morningHourMark)
                    {
                        addHours = morningHourMark - timeVar.Hour;
                    }
                    else
                    {
                        addHours = 24 + morningHourMark - timeVar.Hour;
                    }
                    
                    addMinutes = 60 - timeVar.Minute;
                    
                    if (addMinutes > 0)
                    {
                        addHours--;
                    }
                    
                    timeDifference = new TimeSpan(addHours, addMinutes, 0);
                    timeVar = timeVar.Add(timeDifference);
                } else if (timeVar.Hour >= eveningHourMark)
                {
                    Console.WriteLine($">={eveningHourMark}");
                    
                    addHours = 24 + morningHourMark - timeVar.Hour;
                    addMinutes = 60 - timeVar.Minute;
                    
                    if (addMinutes > 0)
                    {
                        addHours--;
                    }
                    
                    timeDifference = new TimeSpan(addHours, addMinutes, 0);
                    timeVar = timeVar.Add(timeDifference);
                } else if (timeVar.Hour >= afternoonHourMark)
                {
                    Console.WriteLine($">={afternoonHourMark}");

                    if ((input.EndTime.DayOfYear > timeVar.DayOfYear && input.EndTime.Year == timeVar.Year) ||  TimeOnly.FromDateTime(input.EndTime).Hour > eveningHourMark || input.EndTime.Year > timeVar.Year)
                    {
                        addHours = eveningHourMark - timeVar.Hour;
                        addMinutes = 60 - timeVar.Minute;
                        if (addMinutes > 0)
                        {
                            addHours--;
                        }
                    }
                    else
                    {
                        addHours = input.EndTime.Hour - timeVar.Hour;
                        addMinutes = input.EndTime.Minute - timeVar.Minute;
                    }
                    
                    timeDifference = new TimeSpan(addHours, addMinutes, 0);
                    timeSpanPM += timeDifference;
                    timeVar = timeVar.Add(timeDifference);
                } else if (timeVar.Hour >= morningHourMark)
                {
                    Console.WriteLine($">={morningHourMark}");

                    if ((input.EndTime.DayOfYear > timeVar.DayOfYear && input.EndTime.Year == timeVar.Year) || TimeOnly.FromDateTime(input.EndTime).Hour > afternoonHourMark || input.EndTime.Year > timeVar.Year)
                    {
                        addHours = afternoonHourMark - timeVar.Hour;
                        addMinutes = 60 - timeVar.Minute;
                        if (addMinutes > 0)
                        {
                            addHours--;
                        }
                    }
                    else
                    {
                        addHours = input.EndTime.Hour - timeVar.Hour;
                        addMinutes = input.EndTime.Minute - timeVar.Minute;
                    }

                    timeDifference = new TimeSpan(addHours, addMinutes, 0);

                    timeSpanAM += timeDifference;
                    timeVar = timeVar.Add(timeDifference);
                    
                } else if (timeVar.Hour >= 0)
                {
                    Console.WriteLine(">=0");

                    addHours = morningHourMark - timeVar.Hour;
                    addMinutes = 60 - timeVar.Minute;
                    if (addMinutes > 0)
                    {
                        addHours--;
                    }

                    timeVar = timeVar.Add(new TimeSpan(addHours, addMinutes, 0));
                }
                
                Console.WriteLine(timeVar.ToString("dd/MM/yyyy HH:mm"));
                Console.WriteLine($"timeSpanAM: {timeSpanAM}");
                Console.WriteLine($"timeSpanPM: {timeSpanPM}");
                Console.WriteLine();
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