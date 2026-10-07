using DomainInterfaces;
using Moq;
using ShopModels;
using ShopModels.Exceptions;
using ShopReposInterfaces;
using ShopServices;

namespace FunBooksAndVideosShopTests;

public class OrderServiceUnitTests
{
	private readonly Mock<IOrderRepo> _orderRepoMock = new();
	private readonly Mock<IProductRepo> _productRepoMock = new();
	private readonly OrderService _service;

	public OrderServiceUnitTests()
	{
		_service = new OrderService(_orderRepoMock.Object, _productRepoMock.Object, []);
	}
	
	[Fact]
	public void CreateOrder_OrderNotExists_OrderCreated()
	{
		//Arrange
		var customerId = 1;
		var expectedOrderId = 1;
		_orderRepoMock.Setup(x => x.GetOrdersByCustomerId(customerId)).Returns([]);
		_orderRepoMock.Setup((x => x.CreateOrder(customerId))).Returns(expectedOrderId);
		
		//Act
		var actualOrderId = _service.CreateOrder(customerId);

		//Assert
		Assert.Equal(expectedOrderId, actualOrderId);
	}
	
	
	[Fact]
	public void CreateOrder_ProcessedOrderExists_OrderCreated()
	{
		//Arrange
		var customerId = 1;
		var expectedOrderId = 1;
		var existingOrder = new Order { Id = 2, CustomerId = customerId };
		existingOrder.MarkAsProcessed();
		_orderRepoMock.Setup(x => x.GetOrdersByCustomerId(customerId)).Returns([existingOrder]);
		_orderRepoMock.Setup((x => x.CreateOrder(customerId))).Returns(expectedOrderId);
		
		//Act
		var actualOrderId = _service.CreateOrder(customerId);

		//Assert
		Assert.Equal(expectedOrderId, actualOrderId);
	}

	[Fact]
	public void CreateOrder_OrderExists_ThrowsException()
	{
		//Arrange
		var customerId = 1;
		_orderRepoMock.Setup(x => x.GetOrdersByCustomerId(customerId)).Returns([new Order { Id = 1, CustomerId = 2 }]);
		
		//Act
		//Assert
		Assert.Throws<OrderAlreadyCreatedException>(() => _service.CreateOrder(customerId));
	}
	
	[Fact]
	public void GetOrder_OrderExists_ReturnsOrder()
	{
		//Arrange
		var expectedOrder = new Order { Id = 1, CustomerId = 2 };
		_orderRepoMock.Setup(x => x.GetOrder(expectedOrder.Id)).Returns(expectedOrder);
		
		//Act
		var actualOrder = _service.GetOrder(expectedOrder.Id);
		
		//Assert
		Assert.Equal(expectedOrder.Id, actualOrder.Id);
		Assert.Equal(expectedOrder.CustomerId, actualOrder.CustomerId);
	}

	[Fact]
	public void AddProductToOrder_OrderDoesNotContain_ProductAdded()
	{
		//Arrange
		var expectedOrder = new Order { Id = 1, CustomerId = 2};
		expectedOrder.AddProduct(new Book { Id = 2, Price = 1.00m, Title = "b1", Author = "a1", Weight = 100f});
		var expectedProduct = new Video()
			{ Id = 1, Duration = 100, Price = 90m, Title = "t1", DownloadUrl = "url" };
		_orderRepoMock.Setup(x => x.GetOrder(expectedOrder.Id)).Returns(expectedOrder);
		_productRepoMock.Setup(x => x.GetProduct(expectedProduct.Id)).Returns(expectedProduct);
		
		//Act
		_service.AddProductToOrder(expectedOrder.Id, expectedProduct.Id);
		
		//Assert
		Assert.Contains(expectedProduct, expectedOrder.Items);
	}

	[Fact]
	public void AddProductToOrder_OrderContains_ThrowsException()
	{
		//Arrange
		var expectedProductId = 1;
		var expectedOrder = new Order { Id = 1, CustomerId = 2};
		expectedOrder.AddProduct(new Book { Id = expectedProductId, Price = 1.00m, Title = "b1", Author = "a1", Weight = 100f});
		_orderRepoMock.Setup(x => x.GetOrder(expectedOrder.Id)).Returns(expectedOrder);
		
		//Act
		//Assert
		Assert.Throws<OrderAlreadyContainsProductException>(() => _service.AddProductToOrder(expectedOrder.Id, expectedProductId));
	}
	
	[Fact]
	public void ProcessOrder_RulesExists_OrderProcessed()
	{
		//Arrange
		var expectedOrder = new Order { Id = 1, CustomerId = 2};
		expectedOrder.AddProduct(new Book { Id = 2, Price = 1.00m, Title = "b1", Author = "a1", Weight = 100f});
		_orderRepoMock.Setup(x => x.GetOrder(expectedOrder.Id)).Returns(expectedOrder);
		var rule1 = new Mock<IBusinessRule>();
		var rule2 = new Mock<IBusinessRule>();
		var service = new OrderService(_orderRepoMock.Object, _productRepoMock.Object, [rule1.Object, rule2.Object]);
		
		//Act
		service.ProcessOrder(expectedOrder.Id);
		
		//Assert
		Assert.True(expectedOrder.IsProcessed);
		rule1.Verify(x => x.Apply(expectedOrder), Times.Once);
		rule2.Verify(x => x.Apply(expectedOrder), Times.Once);
	}
}