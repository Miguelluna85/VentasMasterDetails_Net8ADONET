namespace Ventas.DTOs;

public class ProductDTO
{
    //no es VM para usar dataannotacion o logica de negocio

    public int ProductID { get; set; }
    public string? Name { get; set; } = null;
    public decimal Price { get; set; }
    public decimal Quantity { get; set; }

}
