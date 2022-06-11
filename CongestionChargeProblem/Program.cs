using CongestionChargeProblem;

string[] inputStrings = 
{
    "Car: 24/04/2008 11:32 - 24/04/2008 14:42",
    "Motorbike: 24/04/2008 17:00 - 24/04/2008 22:11",
    "Van: 25/04/2008 10:23 - 28/04/2008 09:02",
    // "Car: 30/05/2010 22:49 - 02/06/2010 12:35",
    // "Truck: 29/12/2020 19:20 - 04/01/2021 10:14",
    // "Motorbike: 11/06/2022 04:13 - 13/06/2022 08:49",
    // "Car: 07/06/2022 19:46 - 08/06/2022 15:37",
    // "Van: 02/06/2022 18:48 - 06/06/2022 06:35",
    // "Motorbike: 07/06/2022 03:45 - 07/06/2022 18:34"
};

var inputList = new InputList(inputStrings);
Console.WriteLine(inputList);

var outputList = new OutputList(inputList);
Console.WriteLine(outputList);