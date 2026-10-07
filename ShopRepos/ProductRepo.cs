using ShopModels;
using ShopModels.Exceptions;
using ShopReposInterfaces;

namespace ShopRepos;

public class ProductRepo : IProductRepo
{
	//db table "Product" simulation (test purposes)
	private readonly IEnumerable<Product> _products = [
		new Book { Id = 1, Author = "a1", Price = 10.98m, Title = "t1", Weight = 90.02f },
		new Video { Id = 2, Title = "v1", Duration = 100, Price = 99.21m, DownloadUrl = "url"},
		new Membership { Id = 3, MembershipType = MembershipType.BookClub, Title = "m1", Price = 100.00m },
		new Membership { Id = 4, MembershipType = MembershipType.VideoClub, Title = "m2", Price = 200.00m },
		new Membership { Id = 5, MembershipType = MembershipType.Premium, Title = "m3", Price = 300.00m }
	];
	
	public Product GetProduct(int id)
	{
		var product = _products.FirstOrDefault(x => x.Id == id);
		return product ?? throw new EntityNotFoundException(id, nameof(Product));
	}

	public IEnumerable<Product> GetProducts()
	{
		return  _products;
	}
}