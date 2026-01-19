using System;
using webApi.Models;
using webApi.Repository.IRepository;

namespace webApi.Repository;

public class ProductRepository : IProductRepository
{
    private readonly ApplicationDbContext _db;

    public ProductRepository(ApplicationDbContext db)
    {
        _db = db;
    }
    public bool BuyProduct(string name, int quantity)
    {
        if(string.IsNullOrEmpty(name) || | quantity <= 0)
        {
            return false;
        }

        var product = _db.Products.FirstOrDefault(p => p.Name.ToLower().Trim() == name.ToLower().Trim());
        if(product == null || product.Stock < quantity)
        {
            return false;
        }   

        product.Stock -= quantity;
        _db.Products.Update(product);
        return Save();
    }

    public bool CreateProduct(Product product)
    {
        if(product == null)
        {
            return false;
        }

        product.CreationDate = DateTime.Now;
        product.UpdateDate = DateTime.Now;

        _db.Products.Add(product);
        return Save();
    }

    public bool DeleteProduct(Product product)
    {
        if(product == null)
        {
            return false;
        }

        _db.Products.Remove(product);
        return Save();
    }

    public Product? GetProduct(int productId)
    {
        if(productId <= 0)
        {
            return null;
        }
        return _db.Products.FirstOrDefault(p => p.ProductId == productId);
    }

    public ICollection<Product> GetProductForCategory(int categoryId)
    {
        return [.. _db.Products.OrderBy(p => p.Name)];
    }

    public ICollection<Product> GetProducts()
    {
        throw new NotImplementedException();
    }

    public bool ProductExists(int productId)
    {
        throw new NotImplementedException();
    }

    public bool ProductExists(string name)
    {
        throw new NotImplementedException();
    }

    public bool Save()
    {
        throw new NotImplementedException();
    }

    public ICollection<Product> SearchProduct(string name)
    {
        throw new NotImplementedException();
    }

    public bool UpdateProduct(Product product)
    {
        throw new NotImplementedException();
    }
}
