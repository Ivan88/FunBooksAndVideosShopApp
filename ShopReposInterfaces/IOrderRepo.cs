using ShopModels;

namespace ShopReposInterfaces;

public interface IOrderRepo
{
	Order GetOrder(int id);
	IEnumerable<Order> GetOrdersByCustomerId(int customerId);
	int CreateOrder(int customerId);
}