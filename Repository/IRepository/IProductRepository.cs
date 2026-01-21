using System;
using webApi.Models;

namespace webApi.Repository.IRepository;

public interface IProductRepository
{
    ICollection<Product> GetProducts();

    ICollection<Product> GetProductForCategory(int categoryId);
    ICollection<Product> SearchProducts(string searchTerm);

    Product? GetProduct(int productId);

    bool BuyProduct(string name, int quantity);

    bool ProductExists(int productId);

    bool ProductExists(string name);

    bool CreateProduct(Product product);

    bool UpdateProduct(Product product);

    bool DeleteProduct(Product product);

    bool Save();
}
