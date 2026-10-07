using ShopModels;
using ShopReposInterfaces;

namespace ShopRepos;

public class ShippingSlipRepo : IShippingSlipRepo
{
	//db table "ShippingSlip" simulation
	private readonly List<ShippingSlip> _shippingSlips = [];
	
	public void GenerateShippingSlip(IEnumerable<int> productIds)
	{
		_shippingSlips.Add(new ShippingSlip { ProductIds = productIds });
	}

	public IEnumerable<ShippingSlip> GetShippingSlips()
	{
		return _shippingSlips;
	}
}