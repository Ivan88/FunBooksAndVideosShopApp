using ShopModels;
using ShopModels.Exceptions;
using ShopReposInterfaces;

namespace ShopRepos;

public class CustomerRepo : ICustomerRepo
{
	//db table "Customer" simulation
	private readonly IEnumerable<Customer> _customers = [new() { Id = 1 }];
	
	public Customer GetCustomer(int id)
	{
		var customer = _customers.FirstOrDefault(x => x.Id == id);
		return customer ?? throw new EntityNotFoundException(id, nameof(Customer));
	}

	public IEnumerable<Customer> GetCustomers()
	{
		return _customers;
	}
}