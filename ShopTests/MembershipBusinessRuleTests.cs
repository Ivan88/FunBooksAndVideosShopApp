using Moq;
using ShopModels;
using ShopReposInterfaces;
using ShopServices.Rules;

namespace FunBooksAndVideosShopTests;

public class MembershipBusinessRuleTests
{
	private readonly Mock<IOrderRepo> _orderRepoMock = new();
	private readonly Mock<ICustomerRepo> _customerRepoMock = new();
	private readonly MembershipBusinessRule _rule;

	public MembershipBusinessRuleTests()
	{
		_rule = new MembershipBusinessRule(_customerRepoMock.Object);
	}
	
	[Fact]
	public void Apply_NoMemberships_MembershipNotAssigned()
	{
		//Arrange
		var customer = CreateCustomer();
		var order = CreateOrder(customer.Id);
		order.AddProduct(CreateBook());
		_orderRepoMock.Setup(x => x.GetOrder(order.Id)).Returns(order);
		_customerRepoMock.Setup(x => x.GetCustomer(order.CustomerId)).Returns(customer);
		
		//Act
		_rule.Apply(order);
		
		//Assert
		Assert.Null(customer.MembershipType);
	}
	
	[Fact]
	public void Apply_BookMembership_BookMembershipAssigned()
	{
		//Arrange
		var expectedMembership = MembershipType.BookClub;
		var customer = CreateCustomer();
		var order = CreateOrder(customer.Id);
		order.AddProduct(CreateBook());
		order.AddProduct(CreateMembership(expectedMembership));
		_orderRepoMock.Setup(x => x.GetOrder(order.Id)).Returns(order);
		_customerRepoMock.Setup(x => x.GetCustomer(order.CustomerId)).Returns(customer);
		
		//Act
		_rule.Apply(order);
		
		//Assert
		Assert.Equal(customer.MembershipType, expectedMembership);
	}
	
	[Fact]
	public void Apply_TwoMembershipsExists_PremiumAssigned()
	{
		//Arrange
		var customer = CreateCustomer();
		var order = CreateOrder(customer.Id);
		order.AddProduct(CreateBook());
		order.AddProduct(CreateMembership(MembershipType.BookClub));
		order.AddProduct(CreateMembership(MembershipType.VideoClub));
		_orderRepoMock.Setup(x => x.GetOrder(order.Id)).Returns(order);
		_customerRepoMock.Setup(x => x.GetCustomer(order.CustomerId)).Returns(customer);
		
		//Act
		_rule.Apply(order);
		
		//Assert
		Assert.Equal(customer.MembershipType, MembershipType.Premium);
	}
	
	[Fact]
	public void Apply_OnePremiumMembershipExists_PremiumAssigned()
	{
		//Arrange
		var customer = CreateCustomer();
		var order = CreateOrder(customer.Id);
		order.AddProduct(CreateBook());
		order.AddProduct(CreateMembership(MembershipType.Premium));
	
		_orderRepoMock.Setup(x => x.GetOrder(order.Id)).Returns(order);
		_customerRepoMock.Setup(x => x.GetCustomer(order.CustomerId)).Returns(customer);
		
		//Act
		_rule.Apply(order);
		
		//Assert
		Assert.Equal(customer.MembershipType, MembershipType.Premium);
	}
	
	[Fact]
	public void Apply_TwoMembershipIncludedPremiumExists_PremiumAssigned()
	{
		//Arrange
		var customer = CreateCustomer();
		var order = CreateOrder(customer.Id);
		order.AddProduct(CreateBook());
		order.AddProduct(CreateMembership(MembershipType.BookClub));
		order.AddProduct(CreateMembership(MembershipType.Premium));
		_orderRepoMock.Setup(x => x.GetOrder(order.Id)).Returns(order);
		_customerRepoMock.Setup(x => x.GetCustomer(order.CustomerId)).Returns(customer);
		
		//Act
		_rule.Apply(order);
		
		//Assert
		Assert.Equal(customer.MembershipType, MembershipType.Premium);
	}
	
	private Order CreateOrder(int customerId) => new() { Id = 1, CustomerId = customerId };
	private Customer CreateCustomer() => new() { Id = 1 };
	private Book CreateBook() => new() { Id = 11, Title = "b1", Price = 100.09m, Author = "a1", Weight = 90.01f };
	private Membership CreateMembership(MembershipType type) => new() { Id = Random.Shared.Next(), Title = "m1", MembershipType = type, Price = 1000.00m };
}