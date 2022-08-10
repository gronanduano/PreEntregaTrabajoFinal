using System.Data.SqlClient;
using System.Data;
using ClaseProducto;
using ClaseUsuario;
using System;

namespace ClaseMain
{
    public class ClaseMain
    {
        static void Main(string[] args)
        {
            //Constante para definir los datos de la BD por única vez
            const string DBConn = "Server=ARASALP190583\\LOCALDB;Database=SistemaGestion;Trusted_Connection=True";
            
            //Variables para controlar las opciones del Menú
            int valor;
            string opcion;
            
            do
            {
                Console.ForegroundColor = ConsoleColor.Green;
                Console.WriteLine("**********************************************************");
                Console.WriteLine("                    MENU DE OPCIONES                      ");
                Console.WriteLine("**********************************************************");
                Console.WriteLine("\n" + "<1> Buscar todos los Productos" +
                                  "\n" + "<2> Buscar Producto por ID de Usuario" +
                                  "\n" + "<3> Buscar Producto y Cantidad vendidos por ID de Usuario" +
                                  "\n" + "<4> Buscar Ventas realizadas por ID de Usuario" +
                                  "\n" + "<5> Buscar Datos de Usuario por ID" +
                                  "\n" + "<6> Validar datos de acceso de un Usuario" +
                                  "\n" + "<0> Salir");
                
                //Leer la opción ingresada y validarla
                Console.Write("\nIngrese la opción: ");
                Console.ResetColor();
                opcion = Console.ReadLine();
                if (!int.TryParse(opcion, out valor))
                {
                    Console.WriteLine("\nOpción '" + opcion + "' no es válida\n"); continue;
                }

                switch (opcion)
                {
                    case "0": continue;
                    case "1": BuscarListaProductos(); break;
                    case "2": BuscarProductosxIDUsuario(); break;
                    case "3": BuscarCantyProductoVendidoxIDUsuario(); break;
                    case "4": BuscarVentaTotalxIDUsuario(); break;
                    case "5": BuscarDatosUsuarioxNombre(); break;
                    case "6": ValidarAccesoUsuario(); break;
                    default: Console.WriteLine("\nOpción '" + opcion + "' no es válida\n"); break;
                }
            } while (!opcion.Contains("0"));

            //Buscar todos los Productos disponibles
            void BuscarListaProductos()
            {
                MetodosProducto metodo_productos = new MetodosProducto();
                List<Producto> listadoproductos = new List<Producto>();
                listadoproductos = metodo_productos.BuscarTodosLosProductos(DBConn);
                if (listadoproductos.Count > 0)
                {
                    metodo_productos.MostrarProductos(listadoproductos);
                }
                else
                {
                    Console.Write("\nNo existen Productos.\n\n");
                }
            }

            //Buscar Productos x ID de Usuario
            void BuscarProductosxIDUsuario()
            {
                MetodosProducto metodo_productos = new MetodosProducto();
                List<Producto> listadoproductos = new List<Producto>();
                

                //Solicitamos que ingrese el ID de Usuario para buscar
                Console.Write("\nIngrese ID de Usuario: ");
                listadoproductos = metodo_productos.BuscarProductosxIDUsuario(DBConn, Convert.ToInt32(Console.ReadLine()));
                if (listadoproductos.Count > 0)
                {
                    metodo_productos.MostrarProductos(listadoproductos);
                }
                else
                {
                    Console.Write("\nNo existen Productos para el ID de Usuario ingresado.\n\n");
                }
            }

            //Buscar Cantidad y Productos Vendidos x ID de Usuario
            void BuscarCantyProductoVendidoxIDUsuario()
            {
                MetodosProducto metodo_productos = new MetodosProducto();
                List<ProductoVendido> Lista_productos_Vendidos = new List<ProductoVendido>();
                List<Producto> listadoproductos = new List<Producto>();

                //Solicitamos que ingrese el ID de Usuario para buscar
                Console.Write("\nIngrese ID de Usuario: ");

                //Se recupera la lista de productos (Idem anterior)
                listadoproductos = metodo_productos.BuscarProductosxIDUsuario(DBConn, Convert.ToInt32(Console.ReadLine()));

                //Con eso nos fijamos en las ventas de cada producto
                Lista_productos_Vendidos = metodo_productos.BuscarVentasxProducto(DBConn, listadoproductos);
                
                if (Lista_productos_Vendidos.Count > 0)
                {
                    metodo_productos.MostrarProductosVendidos(Lista_productos_Vendidos);
                }
                else
                {
                    Console.Write("\nNo existen Ventas para el ID de Usuario ingresado.\n\n");
                }
            }

            //Buscar Venta Total x ID de Usuario
            void BuscarVentaTotalxIDUsuario()
            {
                MetodosProducto metodo_productos = new MetodosProducto();
                List<VentasTotales> VentasTotales = new List<VentasTotales>();
                List<Producto> listadoproductos = new List<Producto>();

                //Solicitamos que ingrese el ID de Usuario para buscar
                Console.Write("\nIngrese ID de Usuario: ");

                //Se recupera la lista de productos (Idem anterior)
                listadoproductos = metodo_productos.BuscarProductosxIDUsuario(DBConn, Convert.ToInt32(Console.ReadLine()));

                //Con eso nos fijamos en las ventas de cada producto
                VentasTotales = metodo_productos.BuscarVentasTotales(DBConn, listadoproductos);

                if (VentasTotales.Count > 0)
                {
                    metodo_productos.MostrarVentasTotales(VentasTotales);
                }
                else
                {
                    Console.Write("\nNo existen Ventas para el ID de Usuario ingresado.\n\n");
                }
            }

            //Buscar Datos de Usuario por NombreUsuario
            void BuscarDatosUsuarioxNombre()
            {
                MetodosUsuario metodo_usuario = new MetodosUsuario();
                List<Usuario> Lista_usuario = new List<Usuario>();

                //Solicitamos que ingrese el Nombre de Usuario a buscar
                Console.Write("\nIngrese Nombre de Usuario: ");

                //Se recupera la lista de productos (Idem anterior)
                Lista_usuario = metodo_usuario.BuscarDatosUsuarioxNombre(DBConn, Console.ReadLine());

                if (Lista_usuario.Count > 0)
                {
                    metodo_usuario.MostrarDatosUsuario(Lista_usuario);
                }
                else
                {
                    Console.Write("\nNo existen datos para el Usuario ingresado.\n\n");
                }
            }
            //Validar datos de acceso de un Usuario
            void ValidarAccesoUsuario()
            {
                MetodosUsuario metodo_usuario = new MetodosUsuario();
                List<Usuario> usuario_ingresado = new List<Usuario>();

                //Variables para almacenar usuario y contraseña ingresados
                string user = String.Empty;
                string psw = String.Empty;

                //Solicitamos que ingrese el Nombre de Usuario a consultar
                Console.Write("\nIngrese Nombre de Usuario: ");
                user = Console.ReadLine();
                
                //Solicitamos que ingrese la Contraseña del Usuario a consultar
                Console.Write("Ingrese Contraseña: ");
                psw = Console.ReadLine();

                //Se recupera la lista de productos (Idem anterior)
                usuario_ingresado = metodo_usuario.ValidarAccesoUsuario(DBConn, user, psw);
                
                if (usuario_ingresado.Count > 0)
                {
                    Console.Write("\nLos datos de acceso del usuario: '"+ user + "' son correctos.\n\n");
                }
                else
                {
                    Console.Write("\nError - Los datos de acceso del usuario: '" + user + "' no son correctos.\n\n"); ;
                }
            }
        }
    }
}