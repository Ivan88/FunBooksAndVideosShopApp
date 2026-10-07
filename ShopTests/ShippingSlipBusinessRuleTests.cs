using Moq;
using ShopModels;
using ShopReposInterfaces;
using ShopServices.Rules;

namespace FunBooksAndVideosShopTests;

public class ShippingSlipBusinessRuleTests
{
	[Fact]
	public void Apply_NoPhysicalProduct_MembershipNotAssigned()
	{
		//Arrange
		var expectedOrder = new Order
		{
			Id = 1,
			CustomerId = 1
		};
		expectedOrder.AddProduct(new Video()
			{ Id = 11, Title = "b1", Price = 100.09m, DownloadUrl = "a1", Duration = 90 });
		var orderRepoMock = new Mock<IOrderRepo>();
		orderRepoMock.Setup(x => x.GetOrder(expectedOrder.Id)).Returns(expectedOrder);
		var shippingSlipRepoMock = new Mock<IShippingSlipRepo>();
		var rule = new ShippingSlipBusinessRule(shippingSlipRepoMock.Object);
		
		//Act
		rule.Apply(expectedOrder);
		
		//Assert
		shippingSlipRepoMock.Verify(x => x.GenerateShippingSlip(It.IsAny<IEnumerable<int>>()), Times.Never());
	}
	
	[Fact]
	public void Apply_PhysicalProductExists_MembershipNotAssigned()
	{
		//Arrange
		var expectedProductId = 11;
		var expectedOrder = new Order
		{
			Id = 1,
			CustomerId = 1
		};
		expectedOrder.AddProduct(new Book() { Id = expectedProductId, Title = "b1", Price = 100.09m, Author = "a1", Weight = 90.01f });
		var orderRepoMock = new Mock<IOrderRepo>();
		orderRepoMock.Setup(x => x.GetOrder(expectedOrder.Id)).Returns(expectedOrder);
		var shippingSlipRepoMock = new Mock<IShippingSlipRepo>();
		var rule = new ShippingSlipBusinessRule(shippingSlipRepoMock.Object);
		
		//Act
		rule.Apply(expectedOrder);
		
		//Assert
		shippingSlipRepoMock.Verify(x => x.GenerateShippingSlip(
			It.Is<IEnumerable<int>>(y => y.Single() == expectedProductId)), Times.Once());
	}
}