using ShopModels;

namespace ShopReposInterfaces;

public interface IShippingSlipRepo
{
	void GenerateShippingSlip(IEnumerable<int> productIds);
	IEnumerable<ShippingSlip> GetShippingSlips();
}