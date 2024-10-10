namespace Ventas.DTOs;

public class VentaDetalleDTO
{
    public int ID { get; set; }
    public int IDVenta { get; set; }
    public string? NombreProducto { get; set; }
    public decimal Cantidad { get; set; }
    public decimal PrecioUnitario { get; set; }

}
