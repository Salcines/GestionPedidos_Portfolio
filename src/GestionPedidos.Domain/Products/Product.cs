namespace GestionPedidos.Domain.Products;

public sealed class Product
{
    private Product(string name, decimal price)
    {
        if (String.IsNullOrWhiteSpace(name))
        {
            throw new DomainException("Se debe ingresar un nombre de producto");
        }
        Name = name;
        Price = price;
    }

    public string Name { get; }

    public decimal Price { get; }

    public static Product Create(string name, decimal price) 
        => new(name, price);
}