using GestionPedidos.Domain.Products;

namespace GestionPedidos.UnitTests.Domain.Products;

public static class ProductMother
{
    private const string Name = "Keyboard";
    private const decimal Price = 100.5m;
    
    public static Product Default() => Product.Create(Name, Price);
}