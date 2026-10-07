using ShopModels;

namespace DomainInterfaces;

public interface IOrderService
{
	Order GetOrder(int orderId);
	int CreateOrder(int customerId);
	void AddProductToOrder(int orderId, int productId);
	void ProcessOrder(int orderId);
}