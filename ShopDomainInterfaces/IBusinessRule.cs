using ShopModels;

namespace DomainInterfaces;

public interface IBusinessRule
{
	void Apply(Order order);
}