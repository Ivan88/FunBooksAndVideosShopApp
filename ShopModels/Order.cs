using ShopModels.Exceptions;

namespace ShopModels;

public class Order
{
	private readonly List<Product> _items = new();

	public required int Id { get; init; }
	public required int CustomerId { get; init; }
	public bool IsProcessed { get; private set; }
	public decimal TotalPrice => _items.Sum(x => x.Price);
	public IReadOnlyList<Product> Items => _items;

	private readonly Lock _lock = new();
	public void AddProduct(Product product)
	{
		ArgumentNullException.ThrowIfNull(product);

		if (IsProcessed)
			throw new InvalidOperationException("Cannot add product to a processed order.");
		
		lock (_lock)
		{
			if (_items.Any(p => p.Id == product.Id))
				throw new OrderAlreadyContainsProductException(Id, product.Id);
			_items.Add(product);
		}
	}

	public void MarkAsProcessed()
	{
		IsProcessed = true;
	}
}