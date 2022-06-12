using System.Text;

namespace CongestionChargeProblem;

public class OutputList
{
    public List<Output> Outputs { get; set; }

    // These variables represent the hour marks when the congestion charge rate changes
    // (it is possible to change them):
    private readonly int morningHourMark = 7;
    private readonly int afternoonHourMark = 12;
    private readonly int eveningHourMark = 19;

    public OutputList(InputList inputList)
    {
        Outputs = new List<Output>();

        foreach (var input in inputList.Inputs)
        {
            // "timeVar" is the main variable that is used in calculations below.
            // It is equal to the INPUT's StartTime at the beginning.
            // The while cycle below continues until timeVar doesn't reach the INPUT's EndTime.
            DateTime timeVar = input.StartTime;

            TimeSpan timeSpanAM = TimeSpan.Zero; // Represents the time span that passed while the AM rate was active
            TimeSpan timeSpanPM = TimeSpan.Zero; // Represents the time span that passed while the PM rate was active
            TimeSpan timeDifference = TimeSpan.Zero; // Represents how many hours and minutes need to be added to timeSpanAM/timeSpanPM/timeVar

            while (timeVar < input.EndTime)
            {
                timeDifference = CalculateTimeDifference(timeVar, input.EndTime);
                (timeSpanAM, timeSpanPM) = IncreaseTimeSpans(timeSpanAM, timeSpanPM, timeDifference, timeVar);
                timeVar = timeVar.Add(timeDifference);
            }

            Outputs.Add(new Output(timeSpanAM, timeSpanPM, input.VehicleType));
        }
    }

    private TimeSpan CalculateTimeDifference(DateTime timeVar, DateTime endTime)
    {
        // These variables represent how many hours or minutes need to be added to timeSpanAM/timeSpanPM/timeVar:
        int addHours = 0;
        int addMinutes = 0;
        TimeSpan timeDifference = new TimeSpan(addHours, addMinutes, 0);

        if (timeVar.DayOfWeek == DayOfWeek.Saturday || timeVar.DayOfWeek == DayOfWeek.Sunday)
        {
            if (timeVar.Hour < morningHourMark)
            {
                (addHours, addMinutes) = 
                    CalculateAddHoursAndAddMinutes(morningHourMark, timeVar.Hour, 60, timeVar.Minute, true);
            }
            else
            {
                (addHours, addMinutes) =
                    CalculateAddHoursAndAddMinutes(24 + morningHourMark, timeVar.Hour, 60, timeVar.Minute, true);
            }
        }
        else if (timeVar.Hour >= eveningHourMark)
        {
            (addHours, addMinutes) =
                CalculateAddHoursAndAddMinutes(24 + morningHourMark, timeVar.Hour, 60, timeVar.Minute, true);
        }
        else if (timeVar.Hour >= afternoonHourMark)
        {
            if (endTime.Year > timeVar.Year ||
                (endTime.DayOfYear > timeVar.DayOfYear && endTime.Year == timeVar.Year) ||
                TimeOnly.FromDateTime(endTime).Hour >= eveningHourMark)
            {
                (addHours, addMinutes) =
                    CalculateAddHoursAndAddMinutes(eveningHourMark, timeVar.Hour, 60, timeVar.Minute, true);
            }
            else
            {
                (addHours, addMinutes) =
                    CalculateAddHoursAndAddMinutes(endTime.Hour, timeVar.Hour, endTime.Minute, timeVar.Minute, false);
            }
        }
        else if (timeVar.Hour >= morningHourMark)
        {
            if (endTime.Year > timeVar.Year ||
                (endTime.DayOfYear > timeVar.DayOfYear && endTime.Year == timeVar.Year) ||
                TimeOnly.FromDateTime(endTime).Hour >= afternoonHourMark)
            {
                (addHours, addMinutes) =
                    CalculateAddHoursAndAddMinutes(afternoonHourMark, timeVar.Hour, 60, timeVar.Minute, true);
            }
            else
            {
                (addHours, addMinutes) =
                    CalculateAddHoursAndAddMinutes(endTime.Hour, timeVar.Hour, endTime.Minute, timeVar.Minute, false);
            }
        }
        else if (timeVar.Hour >= 0)
        {
            (addHours, addMinutes) =
                CalculateAddHoursAndAddMinutes(morningHourMark, timeVar.Hour, 60, timeVar.Minute, true);
        }
        timeDifference = new TimeSpan(addHours, addMinutes, 0);

        return timeDifference;
    }

    private (int, int) CalculateAddHoursAndAddMinutes(int endHour, int startHour, int endMinute, int startMinute, bool subtractHours)
    {
        int addHours = endHour - startHour;
        int addMinutes = endMinute - startMinute;

        if (subtractHours && addMinutes > 0)
        {
            addHours--;
        }

        return (addHours, addMinutes);
    }

    private (TimeSpan, TimeSpan) IncreaseTimeSpans(TimeSpan timeSpanAM, TimeSpan timeSpanPM, TimeSpan timeDifference, DateTime timeVar)
    {
        if (timeVar.Hour < eveningHourMark && timeVar.Hour >= afternoonHourMark &&
            timeVar.DayOfWeek != DayOfWeek.Saturday && timeVar.DayOfWeek != DayOfWeek.Sunday)
        {
            timeSpanPM += timeDifference;
        }
        else if (timeVar.Hour < afternoonHourMark && timeVar.Hour >= morningHourMark &&
                 timeVar.DayOfWeek != DayOfWeek.Saturday && timeVar.DayOfWeek != DayOfWeek.Sunday)
        {
            timeSpanAM += timeDifference;
        }

        return (timeSpanAM, timeSpanPM);
    }

    public override string ToString()
    {
        int index = 1;
        StringBuilder sb = new StringBuilder();

        foreach (var output in Outputs)
        {
            sb.Append($"OUTPUT {index}\n");
            sb.Append(
                $"Charge for {output.TimeSpanAM.Hours}h {output.TimeSpanAM.Minutes}m (AM rate): £{output.AMCharge.ToString("0.00")}\n");
            sb.Append(
                $"Charge for {output.TimeSpanPM.Hours}h {output.TimeSpanPM.Minutes}m (PM rate): £{output.PMCharge.ToString("0.00")}\n");
            sb.Append($"Total Charge: £{output.TotalCharge.ToString("0.00")}\n\n");
            index++;
        }

        return sb.ToString();
    }
}