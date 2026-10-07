namespace ShopModels;

public record Book : PhysicalProduct
{
	public required string Author { get; init; }
}