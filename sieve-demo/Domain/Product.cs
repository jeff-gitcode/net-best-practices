using Sieve.Attributes;

public class Product
{
    public int Id { get; set; }

    [Sieve(CanFilter = true, CanSort = true)]
    public string Name { get; set; }

    [Sieve(CanFilter = true, CanSort = true)]
    public decimal Price { get; set; }

    [Sieve(CanFilter = true, CanSort = true)]
    public int Stock { get; set; }
}