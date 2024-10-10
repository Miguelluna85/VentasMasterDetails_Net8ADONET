using System.Data;

namespace Ventas.DTOs;

public static class HelperDataTable
{

    public static DataTable TBVentaDetalle()
    {
        DataTable dt = new DataTable();
        dt.Columns.Add("id");
        dt.Columns.Add("idVenta");
        dt.Columns.Add("nombreProducto");
        dt.Columns.Add("cantidad");
        dt.Columns.Add("precio");

        return dt;
    
    }
}
