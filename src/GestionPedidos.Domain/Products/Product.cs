namespace GestionPedidos.Domain.Products;

public sealed class Product
{
    private Product(string name, decimal price)
    {
        Name = name;
        Price = price;
    }

    public string Name { get; }

    public decimal Price { get; }

    public static Product Create(string name, decimal price) 
        => new(name, price);
}