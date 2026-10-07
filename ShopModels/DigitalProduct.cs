namespace ShopModels;

public abstract record DigitalProduct : Product
{
	public required string DownloadUrl { get; init; }
}