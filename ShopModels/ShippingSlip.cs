namespace ShopModels;

public class ShippingSlip
{
	public Guid TrackingNumber { get; } = Guid.NewGuid();
	public IEnumerable<int> ProductIds { get; init; }
	public bool AreProductsDelivered { get; set; }
}