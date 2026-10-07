using DomainInterfaces;
using ShopModels;
using ShopModels.Exceptions;
using ShopReposInterfaces;

namespace ShopServices;

public class OrderService(IOrderRepo orderRepo, IProductRepo productRepo, IEnumerable<IBusinessRule> rules) : IOrderService
{
	public Order GetOrder(int orderId) => orderRepo.GetOrder(orderId);

	public int CreateOrder(int customerId)
	{
		//check if customer already has unprocessed order
		var orders = orderRepo.GetOrdersByCustomerId(customerId);
		if (orders.Any(x => !x.IsProcessed))
			throw new OrderAlreadyCreatedException(customerId);
		
		return orderRepo.CreateOrder(customerId);
	}

	public void AddProductToOrder(int orderId, int productId)
	{
		//check if product already added to order
		var order = orderRepo.GetOrder(orderId);
		if (order.Items.Any(x => x.Id == productId))
			throw new OrderAlreadyContainsProductException(orderId, productId);
		var product = productRepo.GetProduct(productId);
		
		order.AddProduct(product);
	}

	public void ProcessOrder(int orderId)
	{
		var order = orderRepo.GetOrder(orderId);
		order.MarkAsProcessed();
		
		foreach (var rule in rules)
		{
			rule.Apply(order);
		}
	}
}