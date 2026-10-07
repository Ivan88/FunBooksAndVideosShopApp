using System.Collections.Concurrent;
using ShopModels;
using ShopModels.Exceptions;
using ShopReposInterfaces;

namespace ShopRepos;

public class OrderRepo() : IOrderRepo
{
	//simulate "Order" db table
	private readonly ConcurrentBag<Order> _orders = [];
	
	public Order GetOrder(int id)
	{
		var order = _orders.FirstOrDefault(x => x.Id == id);
		return order ?? throw new EntityNotFoundException(id, nameof(Order));
	}

	public IEnumerable<Order> GetOrdersByCustomerId(int customerId)
	{
		return _orders.Where(x => x.CustomerId == customerId);
	}

	private readonly Lock _lock = new();
	public int CreateOrder(int customerId)
	{
		lock (_lock)
		{
			var order = new Order
			{
				Id = (_orders.Count != 0 ? _orders.Max(x => x.Id) : 0) + 1,
				CustomerId = customerId
			};
			_orders.Add(order);
			return order.Id;
		}
	}
}