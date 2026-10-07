using System.Text.Json.Serialization;

namespace ShopModels;

[JsonPolymorphic(TypeDiscriminatorPropertyName = "type")]
[JsonDerivedType(typeof(Book), "book")]
[JsonDerivedType(typeof(Video), "video")]
[JsonDerivedType(typeof(Membership), "membership")]
public abstract record Product
{
	public required int Id { get; init; }
	public required decimal Price { get; init; }
	public required string Title { get; init; }
}