using System.Collections.Generic;
using System.Threading.Tasks;
using ALevelSample.Data;
using ALevelSample.Data.Entities;
using ALevelSample.Models;
using ALevelSample.Repositories;
using ALevelSample.Repositories.Abstractions;
using ALevelSample.Services.Abstractions;
using Microsoft.Extensions.Logging;
using static Microsoft.EntityFrameworkCore.DbLoggerCategory;

namespace ALevelSample.Services;

public class ProductService : BaseDataService<ApplicationDbContext>, IProductService
{
    private readonly IProductRepository _productRepository;
    private readonly ILogger<UserService> _loggerService;

    public ProductService(
        IDbContextWrapper<ApplicationDbContext> dbContextWrapper,
        ILogger<BaseDataService<ApplicationDbContext>> logger,
        IProductRepository productRepository,
        ILogger<UserService> loggerService)
        : base(dbContextWrapper, logger)
    {
        _productRepository = productRepository;
        _loggerService = loggerService;
    }

    public async Task<int> AddProductAsync(string name, double price)
    {
        var id = await _productRepository.AddProductAsync(name, price);
        _loggerService.LogInformation($"Created product with Id = {id}");
        return id;
    }

    public async Task<int> UpdateProductAsync(int id, string name, double price)
    {
        var updatedId = await _productRepository.UpdateProductAsync(id, name, price);
        _loggerService.LogInformation($"Updated product with Id = {id}");
        return id;
    }

    public async Task DeleteProductAsync(int id)
    {
        await _productRepository.DeleteProductAsync(id);
        _loggerService.LogInformation($"Deleted product with Id = {id}");
    }

    public async Task<List<ProductEntity>> GetProductsAsync(int page, string? filterByName, double? filterByPrice)
    {
        return await _productRepository.GetProductsAsync(page, filterByName, filterByPrice);
    }

    public async Task<Product> GetProductAsync(int id)
    {
        var result = await _productRepository.GetProductAsync(id);

        if (result == null)
        {
            _loggerService.LogWarning($"Not founded product with Id = {id}");
            return null!;
        }

        return new Product()
        {
            Id = result.Id,
            Name = result.Name,
            Price = result.Price
        };
    }
}