using ShopModels;

namespace ShopReposInterfaces;

public interface IProductRepo
{
	Product GetProduct(int id);
	IEnumerable<Product> GetProducts();
}