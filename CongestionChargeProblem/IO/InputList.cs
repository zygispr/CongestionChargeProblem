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
            
            // Here I get the vehicle Name:
            string pattern = @"\w+";
            Regex rg = new Regex(pattern);
            MatchCollection matchCollection = rg.Matches(inputString);
            string name = matchCollection[0].Value;
            
            // Here I get the StartTime and EndTime:
            pattern = @"\d{2}\/\d{2}\/\d{4}\s\d{2}:\d{2}";
            rg = new Regex(pattern);
            matchCollection = rg.Matches(inputString);
            DateTime startDate = DateTime.Parse(matchCollection[0].Value, new CultureInfo("en-GB"));
            DateTime endDate = DateTime.Parse(matchCollection[1].Value, new CultureInfo("en-GB"));
            
            Inputs.Add(new Input(name, startDate, endDate));
        }
    }

    public override string ToString()
    {
        int index = 1;
        StringBuilder sb = new StringBuilder();

        foreach (var input in Inputs)
        {
            sb.Append($"INPUT {index}\n");
            sb.Append($"{input.Name}: {input.StartTime.ToString("dd/MM/yyyy HH:mm")} - {input.EndTime.ToString("dd/MM/yyyy HH:mm")}\n");
            index++;
        }

        return sb.ToString();
    }
}