using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ALevelSample.Data;
using ALevelSample.Data.Entities;
using ALevelSample.Models;
using ALevelSample.Repositories.Abstractions;
using ALevelSample.Services.Abstractions;
using Microsoft.EntityFrameworkCore;

namespace ALevelSample.Repositories;

public class ProductRepository : IProductRepository
{
    private readonly ApplicationDbContext _dbContext;

    public ProductRepository(
        IDbContextWrapper<ApplicationDbContext> dbContextWrapper)
    {
        _dbContext = dbContextWrapper.DbContext;
    }

    public async Task<int> AddProductAsync(string name, double price)
    {
        var product = new ProductEntity()
        {
            Name = name,
            Price = price
        };

        var result = await _dbContext.Products.AddAsync(product);
        await _dbContext.SaveChangesAsync();

        return result.Entity.Id;
    }

    public async Task<int> UpdateProductAsync(int id, string name, double price)
    {
        var product = await _dbContext.Products.FindAsync(id);

        if (product == null)
        {
            throw new Exception("Product not found");
        }

        product.Name = name;
        product.Price = price;

        await _dbContext.SaveChangesAsync();

        return product.Id;
    }

    public async Task DeleteProductAsync(int id)
    {
        var product = await _dbContext.Products.FindAsync(id);

        if (product == null)
        {
            throw new Exception("Product not found");
        }

        _dbContext.Products.Remove(product);

        await _dbContext.SaveChangesAsync();
    }

    public async Task<List<ProductEntity>> GetProductsAsync(int page, string? filterByName, double? filterByPrice)
    {
        int pageSize = 20;
        int skip = (page - 1) * pageSize;

        var query = _dbContext.Products.AsQueryable();

        if (!string.IsNullOrEmpty(filterByName))
        {
            query = query.Where(p => p.Name.Contains(filterByName));
        }

        if (filterByPrice.HasValue)
        {
            query = query.Where(p => p.Price == filterByPrice.Value);
        }

        query = query.OrderByDescending(p => p.Price);

        query = query.Skip(skip).Take(pageSize);

        return await query.ToListAsync();
    }

    public async Task<ProductEntity?> GetProductAsync(int id)
    {
        return await _dbContext.Products.FirstOrDefaultAsync(f => f.Id == id);
    }
}