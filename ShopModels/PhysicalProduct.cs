namespace ShopModels;

public abstract record PhysicalProduct : Product
{
	public required float Weight { get; init; }
}