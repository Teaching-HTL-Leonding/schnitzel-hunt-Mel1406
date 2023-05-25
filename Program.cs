#region var
var pathFiles = Directory.GetFiles("C:\\Users\\mebau\\OneDrive\\HTL\\Programming\\Project_063_SchnitzelHunt\\MenuCards");
var cheapestRestaurants = new string[3];
var cheapestDishes = new string[3];
var cheapestPrices = new int[3] {int.MaxValue, int.MaxValue, int.MaxValue};
var searchText = "Schnitzel";
var minAppetizer = 0;
var minMainDish = 0;
var minDessert = 0;
var priceToDrive = 7;
int maxValue = 100;
#endregion

#region program
Console.OutputEncoding = System.Text.Encoding.Default;
foreach (string pathFile in pathFiles)
{
    var lines = File.ReadAllLines(pathFile);
    FindSchnitzel(lines,
                  searchText,
                  pathFile,
                  ref cheapestPrices,
                  ref cheapestRestaurants,
                  ref cheapestDishes);
}
var maxSum = cheapestPrices.Sum() + 2 * priceToDrive;
minAppetizer = cheapestPrices[0];
minMainDish = cheapestPrices[1];
minDessert = cheapestPrices[2];

for (int i = 0; i < 4; i++)
{
    var threeProducts = i == 3 ? true : false;
    var toFill = i switch
    {
        0 => minAppetizer,
        1 => minMainDish,
        2 => minDessert,
        _ => -1,
    };
    foreach (var pathFile in pathFiles)
    {

        var lines = File.ReadAllLines(pathFile);
        var prices = new int[3] { maxValue, maxValue, maxValue };
        var restaurants = new string[3];
        var dishes = new string[3];
        Get2ProductsOneRestaurant(lines,
                                  searchText,
                                  pathFile,
                                  ref prices,
                                  ref restaurants,
                                  ref dishes,
                                  ref maxSum,
                                  i,
                                  toFill,
                                  ref cheapestPrices,
                                  ref cheapestRestaurants,
                                  ref cheapestDishes,
                                  threeProducts);
    }
}
Console.WriteLine($"Appetizers: {cheapestRestaurants[0]}, {cheapestDishes[0]} {cheapestPrices[0]}€");
Console.WriteLine($"Main Dishes: {cheapestRestaurants[1]}, {cheapestDishes[1]} {cheapestPrices[1]}€");
Console.WriteLine($"Dessert: {cheapestRestaurants[2]}, {cheapestDishes[2]} {cheapestPrices[2]}€");
#endregion

#region methods
void FindSchnitzel(string[] lines,
                   string searchText,
                   string pathFile,
                   ref int[] prices,
                   ref string[] restaurants,
                   ref string[] dishes)
{
    var currentKindOfDish = 0;
    foreach (var line in lines)
    {
        currentKindOfDish = line switch
        {
            "APPETIZERS" => 0,
            "MAIN DISHES" => 1,
            "DESSERTS" => 2,
            _ => currentKindOfDish,
        };
        if (line.Contains(searchText, StringComparison.CurrentCultureIgnoreCase)
            && GetDishPrice(line) < prices[currentKindOfDish])
        {
            restaurants[currentKindOfDish] = Path.GetFileNameWithoutExtension(pathFile);
            prices[currentKindOfDish] = GetDishPrice(line);
            dishes[currentKindOfDish] = GetDishName(line);
        }
    }
}
int GetDishPrice(string line)
{
    var index = line.IndexOf(":");
    return int.Parse(line[(index + 2)..^1]);
}
string GetDishName(string line)
{
    var index = line.IndexOf(":");
    return line[0..index];
}
void Get2ProductsOneRestaurant(string[] lines,
                   string searchText,
                   string pathFile,
                   ref int[] prices,
                   ref string[] restaurants,
                   ref string[] dishes,
                   ref int maxSum,
                   int kindOfDishNotChanged,
                   int toFill,
                   ref int[] cheapestPrices,
                   ref string[] cheapestRestaurants,
                   ref string[] CheapestDishes,
                   bool threeProducts)
{
    var currentKindOfDish = 0;
    if (kindOfDishNotChanged != 3)
    {
        prices[kindOfDishNotChanged] = toFill;
    }
    foreach (var line in lines)
    {
        currentKindOfDish = line switch
        {
            "APPETIZERS" => 0,
            "MAIN DISHES" => 1,
            "DESSERTS" => 2,
            _ => currentKindOfDish,
        };
        if (line.Contains(searchText, StringComparison.CurrentCultureIgnoreCase) 
            && (threeProducts ? true : currentKindOfDish != kindOfDishNotChanged)
            && GetDishPrice(line) < prices[currentKindOfDish])
        {
            restaurants[currentKindOfDish] = Path.GetFileNameWithoutExtension(pathFile);
            prices[currentKindOfDish] = GetDishPrice(line);
            dishes[currentKindOfDish] = GetDishName(line);
        }
    }
    if (prices.Sum() + (threeProducts ? 0 : priceToDrive) < maxSum 
        && prices[0] != maxValue && prices[1] != maxValue && prices[2] != maxValue)
    {
        maxSum = prices.Sum() + (threeProducts ? 0 : priceToDrive);
        switch (kindOfDishNotChanged)
        {
            case 0:
                cheapestDishes[1] = dishes[1]; 
                cheapestDishes[2] = dishes[2];
                cheapestRestaurants[1] = restaurants[1];
                cheapestRestaurants[2] = restaurants[2];
                break;
            case 1:
                cheapestDishes[0] = dishes[0];
                cheapestDishes[2] = dishes[2];
                cheapestRestaurants[0] = restaurants[0];
                cheapestRestaurants[2] = restaurants[2];
                break;
            case 2:
                cheapestDishes[0] = dishes[0];
                cheapestDishes[1] = dishes[1];
                cheapestRestaurants[0] = restaurants[0];
                cheapestRestaurants[1] = restaurants[1];
                break;
            default:
                cheapestDishes = dishes;
                cheapestRestaurants = restaurants;
                break;
        }
        cheapestPrices = prices;
    }
}
#endregion