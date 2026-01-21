using System;
using Microsoft.EntityFrameworkCore;
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
        if(string.IsNullOrEmpty(name) || quantity <= 0)
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
        return _db.Products.Include(p => p.category).FirstOrDefault(p => p.ProductId == productId);
    }

    public ICollection<Product> GetProductForCategory(int categoryId)
    {
        if(categoryId <= 0)
        {
            return new List<Product>();
        }

        return [.. _db.Products.Include(p => p.category).Where(p => p.CategoryId == categoryId).OrderBy(p => p.Name)];
    }

    public ICollection<Product> GetProducts()
    {
        return [.. _db.Products.Include(p => p.category).OrderBy(p => p.Name)];
    }

    public bool ProductExists(int productId)
    {
        if(productId <= 0)
        {
            return false;
        }

        return _db.Products.Any(p => p.ProductId == productId);
    }

    public bool ProductExists(string name)
    {
        if(string.IsNullOrEmpty(name))
        {
            return false;
        }

        return _db.Products.Any(p => p.Name.ToLower().Trim() == name.ToLower().Trim());

    }

    public bool Save()
    {
        return _db.SaveChanges() >= 0;
    }

    public ICollection<Product> SearchProducts(string searchTerm)
    {
        IQueryable<Product> query = _db.Products;
        var searchTermLower = searchTerm.ToLower().Trim();

        if(!string.IsNullOrEmpty(searchTerm))
        {
            query = query.Include(p => p.category).Where(p => p.Name.ToLower().Trim().Contains(searchTerm.ToLower().Trim())
            ||
            p.Description.ToLower().Contains(searchTermLower));
        }

        return [.. query.OrderBy(p => p.Name)];
    }

    public bool UpdateProduct(Product product)
    {
        if(product == null)
        {
            return false;
        }

        product.UpdateDate = DateTime.Now;
        _db.Products.Update(product);
        return Save();
    }
}
