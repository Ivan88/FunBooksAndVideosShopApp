namespace ShopModels;

public record Video : DigitalProduct
{
	public required int Duration { get; init; }
}