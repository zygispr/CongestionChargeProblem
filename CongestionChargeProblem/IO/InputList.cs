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
            // I am using Regex to easily get the wanted parts of the INPUT strings.
            
            // Here I get the vehicle Name:
            string pattern = @"\w+";
            MatchCollection matchCollection = Regex.Matches(inputString, pattern);
            string name = matchCollection[0].Value;
            
            // Here I get the StartTime and EndTime:
            pattern = @"\d+\/\d+\/\d+\s*\d+:\d+";
            matchCollection = Regex.Matches(inputString, pattern);
            DateTime startTime = DateTime.Parse(matchCollection[0].Value, new CultureInfo("en-GB"));
            DateTime endTime = DateTime.Parse(matchCollection[1].Value, new CultureInfo("en-GB"));
            
            Inputs.Add(new Input(name, startTime, endTime));
        }
    }
    
    public override string ToString()
    {
        int index = 1;
        StringBuilder sb = new StringBuilder();

        foreach (var input in Inputs)
        {
            sb.Append($"INPUT {index}\n");
            sb.Append($"{input.Name}: {input.StartTime.ToString("dd/MM/yyyy HH:mm")} - {input.EndTime.ToString("dd/MM/yyyy HH:mm")}\n\n");
            index++;
        }

        return sb.ToString();
    }
}