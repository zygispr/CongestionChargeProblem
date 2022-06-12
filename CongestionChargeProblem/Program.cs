using CongestionChargeProblem;

// You can test multiple INPUTS at once by adding them to this array:
string[] inputStrings = 
{
    "Car: 24/04/2008 11:32 - 24/04/2008 14:42",
    "Motorbike: 24/04/2008 17:00 - 24/04/2008 22:11",
    "Van: 25/04/2008 10:23 - 28/04/2008 09:02",
    // "Car: 30/05/2010 22:49 - 02/06/2010 12:35",
    // "Truck: 29/12/2020 19:20 - 04/01/2021 10:14",
    // "Motorbike: 11/06/2022 04:13 - 13/06/2022 08:49",
    // "Car: 07/06/2022 19:46 - 08/06/2022 15:37",
    // "Car: 07/06/2022 00:00 - 08/06/2022 19:36",
    // "Van: 02/06/2022 03:48 - 06/06/2022 06:35",
    // "Motorbike: 07/06/2022 07:35 - 07/06/2022 18:34",
    // "Cra: 31/05/2022 12:48 - 01/06/2022 07:55"
};

var inputList = new InputList(inputStrings);
Console.WriteLine(inputList);

var outputList = new OutputList(inputList);
Console.WriteLine(outputList);