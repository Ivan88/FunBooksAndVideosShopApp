namespace ShopModels.Exceptions;

public class OrderAlreadyCreatedException(int customerId) : Exception($"Order already created for customer {customerId}.")
{
	public int CustomerId => customerId; //for logging
}