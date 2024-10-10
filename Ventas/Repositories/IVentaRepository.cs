using Ventas.DTOs;

namespace Ventas.Repositories;

public interface IVentaRepository
{
    Task<IEnumerable<VentaDTO>> GetAllVentasAsync();
    Task<VentaDTO> GetVentaAsync(int id);
    Task<int> AddVentaAsync(VentaDTO ventaDTO);
    Task<int> UpdateProductAsync(VentaDTO ventaDTO);
    Task<int> DeleteSaleAsync(int id);
}
