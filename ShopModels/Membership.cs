namespace ShopModels;

public record Membership : Product
{
	public required MembershipType MembershipType { get; init; }
}