
namespace Ventas.Repositories;

public class ConexionDBRepository : IConexionDBRepository
{
    private readonly IConfiguration _configuration;
    public ConexionDBRepository(IConfiguration configuration)
    {
        _configuration = configuration;
    }
    public string GetConnectionString()
    {
        var cadena = _configuration.GetConnectionString("DefaultConnection");

        if (cadena != null)
            return cadena;
        else
            return "";
    }
}
