#region var
var pathFiles = Directory.GetFiles("C:\\Users\\mebau\\OneDrive\\HTL\\Programming\\Project_063_SchnitzelHunt\\MenuCards");
string restaurantName = "";
#endregion

#region program
Console.OutputEncoding = System.Text.Encoding.Default;
GetSearchOption(out bool cheap, out string searchText, out int itemPrice);
foreach(string pathFile in pathFiles)
{
    var lines = File.ReadAllLines(pathFile);
    FindSchnitzel(lines, searchText, pathFile,ref itemPrice, cheap, ref restaurantName);
}
Console.WriteLine($"{restaurantName}, {itemPrice}€");
#endregion

#region methods
void FindSchnitzel(string[] lines, string searchText, string pathFile, ref int itemPrice, bool cheap, ref string restaurantName)
{
    foreach(string line in lines)
    {
        if(line.Contains(searchText, StringComparison.CurrentCultureIgnoreCase))
        {
            int price = GetDishPrice(line);
            if (cheap && price < itemPrice || !cheap && price > itemPrice)
            {
                restaurantName = Path.GetFileNameWithoutExtension(pathFile);
                itemPrice = price;
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
void GetSearchOption(out bool cheap, out string searchText, out int itemPrice)
{   
    searchText = "Seitan Schnitzel";
    Console.WriteLine("Please enter what you want to search for and if you want to most expensive or the cheapest dish.");
    string input = Console.ReadLine()!;
    cheap = input.Contains("cheap", StringComparison.CurrentCultureIgnoreCase);
    if(cheap)
    {
        itemPrice = int.MaxValue;
    }
    else
    {
        itemPrice = int.MinValue;
    }
}
#endregion