namespace CSharp3PatternMatching;

public record Product(int Id, int ManufacturerId, string Name, double Price, int Amount, 
    double Weight, string Description);

public record Manufacturer(int Id, string Name, string Country);