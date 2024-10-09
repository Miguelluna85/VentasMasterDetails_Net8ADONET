using System.Data;
using System.Data.SqlClient;
using Ventas.DTOs;

namespace Ventas.Repositories;

public class SaleRepository : ISaleRepository
{
    private readonly string _connectionString;
    public SaleRepository(IConexionDBRepository conexionDBRepository)
    {
        _connectionString = conexionDBRepository.GetConnectionString();
    }

    public async Task<int> AddSaleAsync(SaleDTO saleDTO)
    {
        int saleID;
        var saleDetailsTable = new DataTable();
        saleDetailsTable.Columns.Add("ProductID", typeof(int));
        saleDetailsTable.Columns.Add("Quantity", typeof(int));
        saleDetailsTable.Columns.Add("Price", typeof(decimal));

        foreach (SaleDetailDTO detail in saleDTO.SaleDetails)
        {
            var ProductPrice = await GetProductPriceAsync(detail.ProductID);
            saleDetailsTable.Rows.Add(detail.ProductID, detail.Quantity, ProductPrice);
        }

        saleDTO.Total = saleDetailsTable.AsEnumerable()
                        .Sum(x => x.Field<decimal>("Price") * x.Field<int>("Quantity"));

        using (SqlConnection con = new(_connectionString))
        {
            using SqlCommand cmd = new("sp_InsertSale", con);

            cmd.CommandType = CommandType.StoredProcedure;
            
            cmd.Parameters.AddWithValue("@Date", saleDTO.Date);
            cmd.Parameters.AddWithValue("@Total", saleDTO.Total);
            
            var param = cmd.Parameters.AddWithValue("@SaleDetails", saleDetailsTable);
            param.SqlDbType = SqlDbType.Structured;
            param.TypeName = "saleDetailsType";

            await con.OpenAsync();
            saleID = Convert.ToInt32(await cmd.ExecuteScalarAsync());
        }
        return saleID;
    }

    private async Task<decimal> GetProductPriceAsync(int productID)
    {
        decimal price;
        using (SqlConnection con = new SqlConnection(_connectionString))
        {
            using SqlCommand cmd = new SqlCommand("sp_GetProductPrice", con);
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.Parameters.AddWithValue("@ProductID", productID);
            await con.OpenAsync();
            price = Convert.ToDecimal(await cmd.ExecuteScalarAsync());
        }

        return price;
    }

    public async Task DeleteSaleAsync(int id)
    {
        using SqlConnection con = new SqlConnection(_connectionString);
        using SqlCommand cmd = new SqlCommand("sp_DeleteSale", con);
        cmd.CommandType = CommandType.StoredProcedure;
        cmd.Parameters.AddWithValue("@SaleID", id);
        await con.OpenAsync();
        await cmd.ExecuteNonQueryAsync();
    }

    public async Task<IEnumerable<SaleDTO>> GetAllSalesAsync()
    {
        List<SaleDTO> lstSales = new();

        using SqlConnection con = new SqlConnection(_connectionString);
        using SqlCommand cmd = new("sp_GetAllSales", con);
        cmd.CommandType = CommandType.StoredProcedure;

        await con.OpenAsync();

        using SqlDataReader reader = await cmd.ExecuteReaderAsync();
        while (await reader.ReadAsync())
        {
            lstSales.Add(new SaleDTO
            {
                SaleID = reader.GetInt32(reader.GetOrdinal("SaleID")),
                Date = reader.GetDateTime(reader.GetOrdinal("Date")),
                Total = reader.GetDecimal(reader.GetOrdinal("Total")),
                SaleDetails = []
            });
        }

        if (await reader.NextResultAsync())
        {
            foreach (SaleDTO sale in lstSales)
            {
                while (await reader.ReadAsync() && sale.SaleID == reader.GetInt32(reader.GetOrdinal("SaleID")))
                {
                    sale.SaleDetails.Add(new SaleDetailDTO
                    {
                        SaleDetailID = reader.GetInt32(reader.GetOrdinal("SaleDetailsID")),
                        SaleID = reader.GetInt32(reader.GetOrdinal("SaleID")),
                        ProductID = reader.GetInt32(reader.GetOrdinal("ProductID")),
                        Quantity = reader.GetInt32(reader.GetOrdinal("Quantity")),
                        Price = reader.GetDecimal(reader.GetOrdinal("Price"))
                    });
                }
            }
        }

        return lstSales;
    }

    public async Task<SaleDTO> GetSaleByIdAsync(int id)
    {
        SaleDTO Sale = new();

        using SqlConnection con = new SqlConnection(_connectionString);
        using SqlCommand cmd = new("sp_GetSaleByID", con);
        cmd.CommandType = CommandType.StoredProcedure;
        cmd.Parameters.AddWithValue("@SaleID", id);
        await con.OpenAsync();

        using SqlDataReader reader = await cmd.ExecuteReaderAsync();
        if (await reader.ReadAsync())
        {
            Sale = new SaleDTO
            {
                SaleID = reader.GetInt32(reader.GetOrdinal("SaleID")),
                Date = reader.GetDateTime(reader.GetOrdinal("Date")),
                Total = reader.GetDecimal(reader.GetOrdinal("Total")),
                SaleDetails = []
            };
        }

        if (await reader.NextResultAsync())
        {
            while (await reader.ReadAsync())
            {
                Sale.SaleDetails.Add(new SaleDetailDTO
                {
                    SaleDetailID = reader.GetInt32(reader.GetOrdinal("SaleDetailsID")),
                    SaleID = reader.GetInt32(reader.GetOrdinal("SaleID")),
                    ProductID = reader.GetInt32(reader.GetOrdinal("ProductID")),
                    Quantity = reader.GetInt32(reader.GetOrdinal("Quantity")),
                    Price = reader.GetDecimal(reader.GetOrdinal("Price"))
                });
            }
        }

        return Sale;
    }

    public async Task UpdateProductAsync(SaleDTO saleDTO)
    {
        using SqlConnection con = new(_connectionString);
        await con.OpenAsync();

        using (SqlTransaction transaction = con.BeginTransaction())
        {
            try
            {
                SqlCommand deleteCommand = new SqlCommand("sp_DeleteSaleDetails", con,transaction);
                deleteCommand.CommandType = CommandType.StoredProcedure;
                deleteCommand.Parameters.AddWithValue("@SaleID", saleDTO.SaleID);
                await deleteCommand.ExecuteNonQueryAsync();

                foreach (SaleDetailDTO detail in saleDTO.SaleDetails)
                {
                    SqlCommand insertDetailCommand = new SqlCommand("sp_InsertSaleDetail", con, transaction);
                    insertDetailCommand.CommandType = CommandType.StoredProcedure;
                    insertDetailCommand.Parameters.AddWithValue("@SaleID", saleDTO.SaleID);
                    insertDetailCommand.Parameters.AddWithValue("@ProductID", detail.ProductID);
                    insertDetailCommand.Parameters.AddWithValue("@Quantity", detail.Quantity);
                    insertDetailCommand.Parameters.AddWithValue("@Price", detail.Price);
                    await insertDetailCommand.ExecuteNonQueryAsync();
                }
                SqlCommand UpdateCommand = new SqlCommand("sp_UpdateSale", con, transaction);
                UpdateCommand.CommandType = CommandType.StoredProcedure;
                UpdateCommand.Parameters.AddWithValue("@SaleID", saleDTO.SaleID);
                UpdateCommand.Parameters.AddWithValue("@Date", saleDTO.Date);
                UpdateCommand.Parameters.AddWithValue("@Total", saleDTO.Total);
                await UpdateCommand.ExecuteNonQueryAsync();

                transaction.Commit();

            }
            catch
            {
                transaction.Rollback();
                throw;
            }

        }
    }
}
