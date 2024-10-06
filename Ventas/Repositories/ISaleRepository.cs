using Ventas.DTOs;

namespace Ventas.Repositories;

public interface ISaleRepository
{
    Task<IEnumerable<SaleDTO>> GetAllSalesAsync();
    Task<SaleDTO> GetSaleByIdAsync(int id);
    Task<int> AddSaleAsync(SaleDTO saleDTO);
    Task UpdateProductAsync(SaleDTO saleDTO);
    Task DeleteSaleAsync(int id);
}
