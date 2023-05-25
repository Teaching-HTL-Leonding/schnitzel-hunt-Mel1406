#region var
string[] pathFiles = Directory.GetFiles("C:\\Users\\mebau\\OneDrive\\HTL\\Programming\\Project_063_SchnitzelHunt\\MenuCards");
string[] restaurants = new string[3];
string[] dishes = new string[3];
int[] prices = new int[3];
string searchText = "Schnitzel";
#endregion

#region program
Console.OutputEncoding = System.Text.Encoding.Default;
GetSearchOption(out bool cheap, prices);
foreach(string pathFile in pathFiles)
{
    var lines = File.ReadAllLines(pathFile);
    FindSchnitzel(lines, 
                  searchText, 
                  pathFile,
                  cheap, 
                  ref prices, 
                  ref restaurants,
                  ref dishes);
}
Console.WriteLine($"Appetizers: {restaurants[0]}, {dishes[0]} {prices[0]}€");
Console.WriteLine($"Main Dishes: {restaurants[1]}, {dishes[1]} {prices[1]}€");
Console.WriteLine($"Dessert: {restaurants[2]}, {dishes[2]} {prices[2]}€");
#endregion

#region methods
void FindSchnitzel(string[] lines, 
                   string searchText,
                   string pathFile,  
                   bool cheap, 
                   ref int[] prices,
                   ref string[] restaurants,
                   ref string[] dishes)
{
    string currentKindOfDish = "";
    foreach(string line in lines)
    {
        currentKindOfDish = line switch{
            "APPETIZERS" => "a",
            "MAIN DISHES" => "m",
            "DESSERTS" => "d",
            _ => currentKindOfDish,
        };
        if(line.Contains(searchText, StringComparison.CurrentCultureIgnoreCase))
        {
            int price = GetDishPrice(line);
            if ((cheap && price < prices[0] || !cheap && price > prices[0]) && currentKindOfDish == "a")
            {
                restaurants[0] = Path.GetFileNameWithoutExtension(pathFile);
                prices[0] = price;
                dishes[0] = GetDishName(line);
            }
            else if ((cheap && price < prices[1] || !cheap && price > prices[1]) && currentKindOfDish == "m")
            {
                restaurants[1] = Path.GetFileNameWithoutExtension(pathFile);
                prices[1] = price;
                dishes[1] = GetDishName(line);
            }
            else if ((cheap && price < prices[2] || !cheap && price > prices[2]) && currentKindOfDish == "d")
            {
                restaurants[2] = Path.GetFileNameWithoutExtension(pathFile);
                prices[2] = price;
                dishes[2] = GetDishName(line);
            }
        }
    }
}
int GetDishPrice(string line)
{
    int index = line.IndexOf(":");
    int index2 = line.IndexOf("€");
    return int.Parse(line.Substring(index + 2, index2 - index - 2));
}
string GetDishName(string line)
{
    int index = line.IndexOf(":");
    return line.Substring(0, index);
}
void GetSearchOption(out bool cheap, int[] prices)
{
    Console.WriteLine("Please enter if you want to most expensive or the cheapest dishes.");
    string input = Console.ReadLine()!;
    cheap = input.Contains("cheap", StringComparison.CurrentCultureIgnoreCase);
    if(cheap)
    {
        prices[0] = int.MaxValue;
        prices[1] = int.MaxValue; 
        prices[2] = int.MaxValue;
    }
    else
    {
        prices[0] = int.MinValue;
        prices[1] = int.MinValue; 
        prices[2] = int.MinValue;
    }
}
#endregion