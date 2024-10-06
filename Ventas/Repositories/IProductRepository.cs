using Ventas.DTOs;

namespace Ventas.Repositories;

public interface IProductRepository
{
    Task<IEnumerable<ProductDTO>> GetAllProductsAsync();
    Task<ProductDTO> GetProductByIdAsync(int id);
    Task<int> AddProductAsync(ProductDTO productDTO);
    Task UpdateProductAsync(ProductDTO productDTO);
    Task DeleteProductAsync(int id);
}
