using DomainInterfaces;
using ShopModels;
using ShopReposInterfaces;

namespace ShopServices.Rules;

public class MembershipBusinessRule(ICustomerRepo customerRepo) : IBusinessRule
{
	public void Apply(Order order)
	{
		var memberships = order.Items.OfType<Membership>().ToArray();

		if (memberships.Length == 0) return;
		
		var customer = customerRepo.GetCustomer(order.CustomerId);
		customer.MembershipType = 
			memberships.Length > 1 ? MembershipType.Premium : memberships[0].MembershipType;
	}
}