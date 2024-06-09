using System.Collections.Generic;
using System.Threading.Tasks;
using ALevelSample.Data.Entities;
using ALevelSample.Models;

namespace ALevelSample.Services.Abstractions;

public interface IProductService
{
    Task<int> AddProductAsync(string name, double price);
    Task<int> UpdateProductAsync(int id, string name, double price);
    Task DeleteProductAsync(int id);
    Task<Product> GetProductAsync(int id);
    Task<List<ProductEntity>> GetProductsAsync(int page, string? filterByName, double? filterByPrice);
}