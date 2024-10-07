using System.Data.SqlClient;
using System.Data;
using Ventas.DTOs;

namespace Ventas.Repositories;

public class ProductRepository : IProductRepository
{
    private readonly string _conecctionString;

    public ProductRepository(IConexionDBRepository conexionDBRepository)
    {
        _conecctionString = conexionDBRepository.GetConnectionString();
    }
    public async Task<int> AddProductAsync(ProductDTO productDTO)
    {
        int productID;

        using SqlConnection con = new SqlConnection(_conecctionString);
        using SqlCommand cmd = new SqlCommand("sp_InsertProduct", con);

        cmd.CommandType = CommandType.StoredProcedure;
        cmd.Parameters.AddWithValue("@Name", productDTO.Name);
        cmd.Parameters.AddWithValue("@Price", productDTO.Price);
        cmd.Parameters.AddWithValue("@Quantity", productDTO.Quantity);
        await con.OpenAsync();
        productID = Convert.ToInt32(await cmd.ExecuteScalarAsync());

        return productID;
    }

    public async Task DeleteProductAsync(int id)
    {
        using SqlConnection con = new SqlConnection(_conecctionString);
        using SqlCommand cmd = new SqlCommand("sp_DeleteProduct", con);
        cmd.CommandType = CommandType.StoredProcedure;
        cmd.Parameters.AddWithValue("@ProductID", id);

        await con.OpenAsync();
        await cmd.ExecuteScalarAsync();
    }

    public async Task<IEnumerable<ProductDTO>> GetAllProductsAsync()
    {
        var lstProducts = new List<ProductDTO>();

        using (SqlConnection con = new SqlConnection(_conecctionString))
        {
            using SqlCommand cmd = new SqlCommand("sp_GetAllProducts", con);
            cmd.CommandType = CommandType.StoredProcedure;

            await con.OpenAsync();
            using SqlDataReader reader = await cmd.ExecuteReaderAsync();

            while (await reader.ReadAsync())
            {
                lstProducts.Add(new ProductDTO()
                {
                    ProductID = (int)reader["ProductID"],
                    Name = (string)reader["Name"],
                    Price = (decimal)reader["Price"],
                    Quantity = String.IsNullOrEmpty(reader["Quantity"].ToString()) ? 0 : Convert.ToInt32(reader["Quantity"])
                });
            }
        }
        return lstProducts;
    }

    public async Task<ProductDTO> GetProductByIdAsync(int id)
    {
        ProductDTO product = new();

        using (SqlConnection con = new SqlConnection(_conecctionString))
        {
            using SqlCommand cmd = new SqlCommand("sp_GetProductByID", con);
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.Parameters.AddWithValue("@ProductID", id);
            await con.OpenAsync();
            using SqlDataReader reader = await cmd.ExecuteReaderAsync();

            if (await reader.ReadAsync())
            {
                product = new ProductDTO()
                {
                    ProductID = (int)reader["ProductID"],
                    Name = (string)reader["Name"],
                    Price = (decimal)reader["Price"],
                    Quantity = String.IsNullOrEmpty(reader["Quantity"].ToString()) ? 0 : Convert.ToInt32(reader["Quantity"])
                };
            }
        }
        return product;
    }

    public async Task UpdateProductAsync(ProductDTO productDTO)
    {
        using SqlConnection con = new SqlConnection(_conecctionString);
        using SqlCommand cmd = new SqlCommand("sp_UpdateProduct", con);

        cmd.CommandType = CommandType.StoredProcedure;
        cmd.Parameters.AddWithValue("@ProductID", productDTO.ProductID);
        cmd.Parameters.AddWithValue("@Name", productDTO.Name);
        cmd.Parameters.AddWithValue("@Price", productDTO.Price);
        cmd.Parameters.AddWithValue("@Quantity", productDTO.Quantity);
        await con.OpenAsync();
        await cmd.ExecuteNonQueryAsync();

    }
}
