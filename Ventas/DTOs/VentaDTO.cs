namespace Ventas.DTOs;

public class VentaDTO
{
    public int ID { get; set; }
    public DateTime? fecha { get; set; }
    public decimal Total { get; set; }
    public List<VentaDetalleDTO> ListVentaDEtalle { get; set; } = [];
}
