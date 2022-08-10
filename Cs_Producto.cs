using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data.SqlClient;
using System.Data;


namespace ClaseProducto
{
    //Definición de la Clase Producto
    public class Producto
    {
        public int Id { get; set; }
        public string Descripciones { get; set; }
        public double Costo { get; set; }
        public double PrecioVenta { get; set; }
        public int Stock { get; set; }
        public int IdUsuario { get; set; }
    }

    //Definición de la Clase Producto Vendido
    public class ProductoVendido
    {
        public int IdUsuario { get; set; }
        public int IdProducto { get; set; }
        public string Descripciones { get; set; }
        public int Stock { get; set; }
    }

    //Definición de la Clase Ventas Totales
    public class VentasTotales
    {
        public int IdUsuario { get; set; }
        public int IdProducto { get; set; }
        public string Descripciones { get; set; }
        public int Stock { get; set; }
        public double PrecioVenta { get; set; }
        public double Venta { get; set; }
    }

    //Definición de los Métodos de la Clase Producto
    public class MetodosProducto
    { 
        //Este método va a buscar todos los productos en la BD y los devuelve en una tabla
        public List<Producto> BuscarTodosLosProductos(string DBConn)  
        {
            List<Producto> Lista_productos = new List<Producto>();
            using (SqlConnection sqlConnection = new SqlConnection(DBConn))
            {
                using (SqlCommand sqlCommand = new SqlCommand( "SELECT * FROM Producto", sqlConnection))
                {
                    sqlConnection.Open();

                    using (SqlDataReader dataReader = sqlCommand.ExecuteReader())
                    {
                        if (dataReader.HasRows)
                        {
                            while (dataReader.Read())
                            {
                                Producto producto = new Producto();
                                producto.Id = Convert.ToInt32(dataReader["Id"]);
                                producto.Stock = Convert.ToInt32(dataReader["Stock"]);
                                producto.IdUsuario = Convert.ToInt32(dataReader["IdUsuario"]);
                                producto.Costo = Convert.ToInt32(dataReader["Costo"]);
                                producto.PrecioVenta = Convert.ToInt32(dataReader["PrecioVenta"]);
                                producto.Descripciones = dataReader["Descripciones"].ToString();
                                Lista_productos.Add(producto);
                            }
                        }
                    }
                    sqlConnection.Close();
                }
            }
            return Lista_productos;
        }

        //Este método va a buscar todos los productos en la BD según un IDUsuario y los devuelve en una tabla
        public List<Producto> BuscarProductosxIDUsuario(string DBConn, int ID)
        {
            //Tabla temporal para almacenarar el resultado de la consulta
            DataTable temp_table = new DataTable();

            List<Producto> Lista_productos = new List<Producto>();
            using (SqlConnection sqlConnection = new SqlConnection(DBConn))
            {
                using (SqlCommand sqlCommand = new SqlCommand("SELECT * FROM Producto WHERE IdUsuario = @ID", sqlConnection))
                {
                    sqlConnection.Open();

                    sqlCommand.Parameters.AddWithValue("@ID", ID);
                    SqlDataAdapter SqlAdapter = new SqlDataAdapter();
                    SqlAdapter.SelectCommand = sqlCommand;
                    SqlAdapter.Fill(temp_table);

                    foreach (DataRow line in temp_table.Rows)
                    {
                        Producto producto = new Producto();
                        producto.Id = Convert.ToInt32(line["Id"]);
                        producto.Descripciones = line["Descripciones"].ToString();
                        producto.IdUsuario = Convert.ToInt32(line["IdUsuario"]);
                        Lista_productos.Add(producto);
                    }
                    sqlConnection.Close();
                }
            }
            return Lista_productos;
        }

        //Este método va a buscar cantidades vendidas de un producto en la BD según un IDUsuario y los devuelve en una tabla
        public List<ProductoVendido> BuscarVentasxProducto(string DBConn, List<Producto> ListaProductos)
        {
            //Tabla temporal para almacenarar el resultado de la consulta
            DataTable temp_table = new DataTable();

            List<ProductoVendido> Lista_productos_Vendidos = new List<ProductoVendido>();
            using (SqlConnection sqlConnection = new SqlConnection(DBConn))
            {
                using (SqlCommand sqlCommand = new SqlCommand("select p.IdUsuario, pv.IdProducto, p.Descripciones, pv.Stock " +
                    "from ProductoVendido pv join Producto p on pv.IdProducto = p.Id " +
                    "where pv.IdProducto = @IdProducto and p.IdUsuario = @IdUsuario", sqlConnection))
                    
                {
                    sqlConnection.Open();

                    foreach (var row in ListaProductos)
                    {
                        sqlCommand.Parameters.Clear();
                        sqlCommand.Parameters.AddWithValue("@IdProducto", row.Id);
                        sqlCommand.Parameters.AddWithValue("@IdUsuario", row.IdUsuario);
                        SqlDataAdapter SqlAdapter = new SqlDataAdapter();
                        SqlAdapter.SelectCommand = sqlCommand;
                        SqlAdapter.Fill(temp_table);
                    }

                    foreach (DataRow line in temp_table.Rows)
                    {
                        ProductoVendido producto_vendido = new ProductoVendido();

                        producto_vendido.IdUsuario = Convert.ToInt32(line["IdUsuario"]);
                        producto_vendido.IdProducto = Convert.ToInt32(line["IdProducto"]);
                        producto_vendido.Descripciones = line["Descripciones"].ToString();
                        producto_vendido.Stock = Convert.ToInt32(line["Stock"]);
                        Lista_productos_Vendidos.Add(producto_vendido);
                    }
                    sqlConnection.Close();
                }
            }
            return Lista_productos_Vendidos;
        }

        //Este método va a calcular el total de ventas en la BD según un IDUsuario y los devuelve en una tabla
        public List<VentasTotales> BuscarVentasTotales(string DBConn, List<Producto> ListaProductos)
        {
            //Tabla temporal para almacenarar el resultado de la consulta
            DataTable temp_table = new DataTable();

            List<VentasTotales> VentasTotales = new List<VentasTotales>();
            using (SqlConnection sqlConnection = new SqlConnection(DBConn))
            {
                using (SqlCommand sqlCommand = new SqlCommand("select p.IdUsuario, pv.IdProducto, p.Descripciones, pv.Stock, p.PrecioVenta " +
                    "from ProductoVendido pv join Producto p on pv.IdProducto = p.Id " +
                    "where pv.IdProducto = @IdProducto and p.IdUsuario = @IdUsuario", sqlConnection))

                {
                    sqlConnection.Open();

                    foreach (var row in ListaProductos)
                    {
                        sqlCommand.Parameters.Clear();
                        sqlCommand.Parameters.AddWithValue("@IdProducto", row.Id);
                        sqlCommand.Parameters.AddWithValue("@IdUsuario", row.IdUsuario);
                        SqlDataAdapter SqlAdapter = new SqlDataAdapter();
                        SqlAdapter.SelectCommand = sqlCommand;
                        SqlAdapter.Fill(temp_table);
                    }

                    foreach (DataRow line in temp_table.Rows)
                    {
                        VentasTotales venta_total = new VentasTotales();

                        venta_total.IdUsuario = Convert.ToInt32(line["IdUsuario"]);
                        venta_total.IdProducto = Convert.ToInt32(line["IdProducto"]);
                        venta_total.Descripciones = line["Descripciones"].ToString();
                        venta_total.Stock = Convert.ToInt32(line["Stock"]);
                        venta_total.PrecioVenta = Convert.ToDouble(line["PrecioVenta"]);
                        venta_total.Venta = venta_total.Stock * venta_total.PrecioVenta;
                        VentasTotales.Add(venta_total);
                    }
                    sqlConnection.Close();
                }
            }
            return VentasTotales;
        }

        //Este método va a mostrar una lista de Productos por pantalla
        public void MostrarProductos(List<Producto> ListaProductos)
        {
            Console.WriteLine("\n---------------------------------------------");
            Console.WriteLine("{0,5}{1,30}{2,2}", "ID".PadRight(5), "Descripciones".PadRight(30), "ID Usuario");
            Console.WriteLine("---------------------------------------------");
            foreach (var linea in ListaProductos)
            {
                Console.WriteLine("{0,5}{1,30}{2,2}", linea.Id.ToString().PadRight(5) , linea.Descripciones.PadRight(30) , linea.IdUsuario);
            }
            Console.WriteLine("\n");
        }

        //Este método va a mostrar una lista por pantalla de Productos vendidos por Usuario
        public void MostrarProductosVendidos(List<ProductoVendido> ListaProductos)
        {
            Console.WriteLine("\n------------------------------------------------------------------");
            Console.WriteLine("{0,5}{1,12}{2,32}{3,7}", "IDUsuario".PadRight(5), "IDProducto".PadRight(1), "Descripciones".PadRight(30), "Cant.Vendida".PadRight(1));
            Console.WriteLine("------------------------------------------------------------------");
            foreach (var linea in ListaProductos)
            {
                Console.WriteLine("{0,5}{1,11}{2,37}{3,7}", linea.IdUsuario.ToString().PadRight(5), linea.IdProducto.ToString().PadRight(1), linea.Descripciones.PadRight(30), linea.Stock.ToString().PadRight(1));
            }
            Console.WriteLine("\n");
        }

        //Este método va a mostrar por pantalla una lista con las ventas totales por Usuario
        public void MostrarVentasTotales(List<VentasTotales> VentasTotales)
        {
            Console.WriteLine("\n-------------------------------------------------------------------------------------------");
            Console.WriteLine("{0,5}{1,12}{2,32}{3,7}{4,14}{5,12}", "IDUsuario".PadRight(5), "IDProducto".PadRight(1), "Descripción".PadRight(30),
                                "Cant.Vendida".PadRight(1), "Precio Unit.".PadRight(1), "Venta Total".PadRight(1));
            Console.WriteLine("-------------------------------------------------------------------------------------------");
            foreach (var linea in VentasTotales)
            {
                 Console.WriteLine("{0,5}{1,12}{2,36}{3,7}{4,15}{5,16}", linea.IdUsuario.ToString().PadRight(5), linea.IdProducto.ToString().PadRight(1), 
                     linea.Descripciones.PadRight(30), linea.Stock.ToString().PadRight(1), linea.PrecioVenta.ToString().PadRight(1), linea.Venta.ToString().PadRight(1));
            }
            Console.WriteLine("\n");
        }
    }
}
