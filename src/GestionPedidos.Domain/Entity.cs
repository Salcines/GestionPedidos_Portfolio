namespace GestionPedidos.Domain;

public abstract class Entity
{
    public Guid Id { get; } = Guid.CreateVersion7();
}