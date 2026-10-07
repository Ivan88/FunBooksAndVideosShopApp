namespace ShopModels;

public class Customer
{
	public required int Id { get; init; }
	public MembershipType? MembershipType { get; set; }
}