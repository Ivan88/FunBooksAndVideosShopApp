namespace ShopModels.Exceptions;

public class OrderAlreadyContainsProductException(int orderId, int productId) : Exception($"Order already contains product")
{
	public int OrderId => orderId; //for logging
	public int ProductId => productId; //for logging
}