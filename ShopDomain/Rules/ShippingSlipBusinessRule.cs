using DomainInterfaces;
using ShopModels;
using ShopReposInterfaces;

namespace ShopServices.Rules;

public class ShippingSlipBusinessRule(IShippingSlipRepo shippingSlipRepo) : IBusinessRule
{
	public void Apply(Order order)
	{
		var products = order.Items.OfType<PhysicalProduct>().ToArray();
		
		if (products.Length > 0)
			shippingSlipRepo.GenerateShippingSlip(products.Select(x => x.Id));
	}
}