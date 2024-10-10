using System.Data;
using System.Data.SqlClient;
using Ventas.DTOs;

namespace Ventas.Repositories;

public class VentaRepository(IConexionDBRepository ConexionDBRepository) : IVentaRepository
{
    string coneccionString = ConexionDBRepository.GetConnectionString();

    public async Task<int> AddVentaAsync(VentaDTO ventaDTO)
    {
        int idVenta = 0;
        int idVentaDetalle = 1;
        DataTable dt = HelperDataTable.TBVentaDetalle();

        foreach (VentaDetalleDTO producto in ventaDTO.ListVentaDEtalle)
        {
            dt.Rows.Add(idVentaDetalle, producto.IDVenta, producto.NombreProducto,
                        producto.Cantidad, producto.PrecioUnitario);
            idVentaDetalle++;
        }
        using (SqlConnection conn = new SqlConnection(coneccionString))
        {
            using (SqlCommand cmd = new SqlCommand("sp_GuardaVenta", conn))
            {
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@fecha", ventaDTO.fecha);
                cmd.Parameters.AddWithValue("@total", ventaDTO.Total);

                SqlParameter paramDetalle = new SqlParameter("@tbDetalleVenta",SqlDbType.Structured);
                paramDetalle.TypeName = "VentaDetalleType";
                paramDetalle.Value = dt;
                cmd.Parameters.Add(paramDetalle);
                
                await conn.OpenAsync();
                idVenta = Convert.ToInt32(await cmd.ExecuteScalarAsync());
                await conn.CloseAsync();
            }

        }
        return idVenta;
    }

    public Task<int> DeleteSaleAsync(int id)
    {
        throw new NotImplementedException();
    }

    public Task<IEnumerable<VentaDTO>> GetAllVentasAsync()
    {
        throw new NotImplementedException();
    }

    public Task<VentaDTO> GetVentaAsync(int id)
    {
        throw new NotImplementedException();
    }

    public Task<int> UpdateProductAsync(VentaDTO ventaDTO)
    {
        throw new NotImplementedException();
    }
}
