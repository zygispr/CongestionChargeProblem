using System.Globalization;
using System.Text;
using System.Text.RegularExpressions;

namespace CongestionChargeProblem;

public class InputList
{
    public List<Input> Inputs { get; init; }
    
    public InputList(string[] inputStrings)
    {
        Inputs = new List<Input>();
    
        foreach (string inputString in inputStrings)
        {
            // I am using Regex to easily get the wanted parts of the input strings.
            
            // Here I get the vehicle name:
            string pattern = @"\w+";
            Regex rg = new Regex(pattern);
            MatchCollection matchCollection = rg.Matches(inputString);
            string name = matchCollection[0].Value;
            
            // Here I get the start and end DateTimes:
            pattern = @"\d{2}\/\d{2}\/\d{4}\s\d{2}:\d{2}";
            rg = new Regex(pattern);
            matchCollection = rg.Matches(inputString);
            DateTime startDate = DateTime.Parse(matchCollection[0].Value, new CultureInfo("en-GB"));
            DateTime endDate = DateTime.Parse(matchCollection[1].Value, new CultureInfo("en-GB"));
            
            Inputs.Add(new Input(name, startDate, endDate));
        }
    }

    /*
    public void CalculateCharges(InputList inputList)
    {
        int index = 1;
        ChargeRate chargeRate;
        
        foreach (var input in inputList.Inputs)
        {
            Console.WriteLine(input.StartTime);
            input.StartTime = input.StartTime.AddMinutes(28);
            Console.WriteLine(input.StartTime);

            index++;
            if (index > 1)
            {
                break;
            }
        }
    }
    */

    public override string ToString()
    {
        int index = 1;
        StringBuilder sb = new StringBuilder();

        foreach (var input in Inputs)
        {
            sb.Append($"INPUT {index}\n");
            sb.Append($"{input.Name}: {input.StartTime} - {input.EndTime}\n");
            index++;
        }

        return sb.ToString();
    }
}