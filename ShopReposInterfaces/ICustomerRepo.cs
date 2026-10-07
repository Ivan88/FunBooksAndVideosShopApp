using ShopModels;

namespace ShopReposInterfaces;

public interface ICustomerRepo
{
	Customer GetCustomer(int id);
	IEnumerable<Customer> GetCustomers();
}