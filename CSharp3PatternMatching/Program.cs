#region Pattern List

using System.Globalization;
using System.Net;
using System.Net.Http.Headers;
using System.Reflection;
using System.Security.Cryptography;
using CSharp3PatternMatching;

var lst = new [] {1, 2, 3, 4};

if (lst is [1, 2, 3, 4])
{

}

var names = new[] {"Петя", "Вася", "Олег", "Анна", "Алевтина", "Алиса"};

foreach (var name in names)
{
    // Начинается с 'А', потом что-угодно, заканчивается на 'а'
    if (name is ['А', .., 'а'])
        Console.WriteLine(name);
}

var res = ToCapitalFirstLetter("бабушка");
Console.WriteLine(res);

string ToCapitalFirstLetter(string text) =>
    text is [var first, .. var rest] ? $"{first.ToString().ToUpper()}{rest}" : text;
    
    
var str = "sfdsfsdfdsfsdfsf";
// .. только один раз в паттерне
//if (str is ['f', .., 'd', .., 'f'])

var item1 = new Item(Name: "Вася", "Очень хороши мальчик");
var item2 = new Item(Name: "Вася", "Очень хороши мальчик");

var resStr = item1 == item2 ? "== Equals" : "== Not equals";
Console.WriteLine(resStr);

resStr = item1 is ("Вася", "Очень хороши мальчик") ? "is Equals" : "is Not equals";
Console.WriteLine(resStr);

#endregion

#region LINQ

/*
LINQ (SELECT, UPDATE, INSERT, DELETE)

SELECT * FROM Table1;

UPDATE Table1 SET Name = 'sadsadsa';
WHERE ID = 1;

INSERT INTO Table1 (Name, Description)
VALUES ("dsadsa", "dsadsadsa");

DELETE FROM Tabl1
WHERE Id = 1;

*/

var names2 = new[] {"Петя", "Вася", "Олег", "Анна", "Алевтина", "Алиса"};

IEnumerable<string> SelectNamesWithMoreThan4Letters(IEnumerable<string> names)
{
    var resList = new List<string>();
    foreach (var name in names)
    {
        if (name.Length >= 4)
            resList.Add(name);
    }

    return resList;
}

IEnumerable<string> SelectNamesWithMoreThan4LettersLINQ(IEnumerable<string> names)
{
    return (from name in names
        where name.Length >= 4
        select name);
}

IEnumerable<string> SelectNamesWithMoreThan4LettersLINQExt(IEnumerable<string> names)
{
    return names.Where(x => x.Length >= 4)
        //.Select(x => x)
        ;
}

void PrintArray(IEnumerable<string> names) => Console.WriteLine($"[{string.Join(',', names)}]");

PrintArray(SelectNamesWithMoreThan4Letters(names2));
PrintArray(SelectNamesWithMoreThan4LettersLINQ(names2));
PrintArray(SelectNamesWithMoreThan4LettersLINQExt(names2));

var manufacturers = new[]
{
    new Manufacturer(Id: 1, Name: "Apple", Country: "USA"),
    new Manufacturer(Id: 2, Name: "Samsung", Country: "Korea"),
    new Manufacturer(Id: 3, Name: "Xiaomi", Country: "China"),
    new Manufacturer(Id: 4, Name: "Meizu", Country: "China"),
    new Manufacturer(Id: 5, Name: "LG", Country: "Korea"),
    new Manufacturer(Id: 6, Name: "Nokia", Country: "Denmark"),
};

var products = new[]
{
    new Product(Id: 1, ManufacturerId: 1, Name: "IPhone 10", Price: 12000, 
        Amount: 200, Weight: 300, Description: "The best phone in the world"),
    new Product(Id: 2, ManufacturerId: 1, Name: "IPad 10", Price: 32000, 
        Amount: 100, Weight: 600, Description: "The best tablet in the world"),
    new Product(Id: 3, ManufacturerId: 4, Name: "9 Pro Max", Price: 16000, 
        Amount: 10, Weight: 220, Description: String.Empty),
    new Product(Id: 4, ManufacturerId: 6, Name: "3310", Price: 3000, 
            Amount: 1, Weight: 300, Description: "I will always remember you"),
    new Product(Id: 5, ManufacturerId: 5, Name: "343243", Price: 10000, 
        Amount: 10, Weight: 300, Description: "The best phone in the world"),
};

var resPrices = (from product in products
    select new
    {
        Name = product.Name,
        Price = product.Price
    });

foreach(var item in resPrices)
    Console.WriteLine($"{item.Name}: {item.Price}");

var resPrices2 = products.Select(x =>
    new
    {
        Name = x.Name,
        Price = x.Price
    });

foreach(var item in resPrices2)
    Console.WriteLine($"{item.Name}: {item.Price}");

List<(string, double)> resList3 = new();
foreach (var product in products)
{
    resList3.Add((product.Name, product.Price));
}
foreach(var item in resPrices2)
    Console.WriteLine($"{item.Name}: {item.Price}");

var resNames = 
    from product in products
    where product.Price > 15000
    orderby product.Price descending
    select product.Name;

Console.WriteLine("=================");

foreach(var name in resNames)
    Console.WriteLine(name);

List<(string, double)> names3 = new();
foreach (var product in products)
{
    if (product.Price > 15000)
        names3.Add((product.Name, product.Price));
}

names3.Sort(comparison: (v1, v2) =>
{
    var res = v2.Item2 - v1.Item2;
    if (res > 0) return 1;
    if (res == 0) return 0;
    if (res < 0) return -1;

    return 0;
});

List<string> names4 = new();
foreach(var name in names3)
    names4.Add(name.Item1);

Console.WriteLine("=================");

foreach(var name in names4)
    Console.WriteLine(name);

var products5 =
    from product in products
    let totalPrice = product.Amount * product.Price
    select new
    {
        Name = product.Name,
        Price = product.Price,
        TotalPrice = totalPrice,
        Amount = product.Amount
    };
foreach(var product in products5)
    Console.WriteLine($"{product.Name}: {product.Price}, Amount: {product.Amount}, Total; {product.TotalPrice}");

List<(string, string)> productsManufacturers = new();
foreach(var product in products)
foreach (var manufacturer in manufacturers)
    if (manufacturer.Id == product.ManufacturerId)
    {
        productsManufacturers.Add((product.Name, manufacturer.Name));
        break;
    }

foreach(var pm in productsManufacturers)
    Console.WriteLine($"{pm.Item1} {pm.Item2}");

var productsManufacturers2 =
    from product in products
    join manufacturer in manufacturers
        on product.ManufacturerId equals manufacturer.Id
    select new
    {
        ProductName = product.Name,
        ManufacturerName = manufacturer.Name
    };

Console.WriteLine("=================");

foreach(var pm in productsManufacturers2)
    Console.WriteLine($"{pm.ProductName} {pm.ManufacturerName}");

var productsManufacturers3 = 
    products.Join(manufacturers, p => p.ManufacturerId, m => m.Id, (p, m) =>
    new
    {
        ProductName = p.Name,
        ManufacturerName = m.Name
    });

Console.WriteLine("=================");

foreach(var pm in productsManufacturers3)
    Console.WriteLine($"{pm.ProductName} {pm.ManufacturerName}");

var numbers = new[] {1, 34, 55, 21, 32, 34, 34, 343, 324};
int maxValue = Int32.MinValue;
int minValue = Int32.MaxValue;
int total = 0;
foreach (var number in numbers)
{
    if (number < minValue)
        minValue = number;
    if (number > maxValue)
        maxValue = number;
    total += number;
}
var avg = ((double) total) / numbers.Length;

Console.WriteLine($"Min: {minValue}; Max: {maxValue}; Total: {total}; Avg: {avg}");

Console.WriteLine($"Min: {numbers.Min()}; Max: {numbers.Max()}; Total: {numbers.Sum()}; Avg: {numbers.Average()}");

var numbers1 = new[] {1, 3, 4, 8, 23, 11};
var numbers3 = new[] {1, 45, 67, 23, 11};

var numbers4 = numbers1.Intersect(numbers3).ToArray();

var results =
    from product in products
    join manufacturer in manufacturers
        on product.ManufacturerId equals manufacturer.Id
    group product by manufacturer.Name
    into g
    let avgPrice = g.Sum(x => x.Price * x.Amount) / g.Sum(x => x.Amount)
    where avgPrice > 8000
    orderby avgPrice descending 
    select new
    {
        ManufacturerName = g.Key,
        Amount = g.Sum(x => x.Amount),
        AvgPrice = avgPrice,
        Names = g.Select(x => x.Name).Aggregate((x, y) => $"{x}, {y}")
    };

Console.WriteLine("===============================================");

foreach(var group in results)
    Console.WriteLine($"{group.ManufacturerName}, Names: [{group.Names}], " +
                      $"Total amount: {group.Amount}, Avg. Price: {group.AvgPrice}");



Product? FindProductById(int id)
{
    return
        (from product in products
        where product.Id == id
        select product).FirstOrDefault();
}

#endregion