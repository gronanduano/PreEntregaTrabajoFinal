using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data.SqlClient;
using System.Data;

namespace ClaseUsuario
{
    //Definición de la Clase Usuario
    public class Usuario
    {
        public int Id { get; set; }
        public string Nombre { get; set; }
        public string Apellido { get; set; }
        public string Contraseña { get; set; }
        public string NombreUsuario { get; set; }
        public string Mail { get; set; }
    }

    //Definición de los Métodos de la Clase Usuario
    public class MetodosUsuario
    {
        //Este método va a buscar un usuario en la BD y los devuelve en una tabla
        public List<Usuario> BuscarDatosUsuarioxNombre(string DBConn, string username)
        {
            //Tabla temporal para almacenarar el resultado de la consulta
            DataTable temp_table = new DataTable();

            List<Usuario> Lista_usuario = new List<Usuario>();
            using (SqlConnection sqlConnection = new SqlConnection(DBConn))
            {
                using (SqlCommand sqlCommand = new SqlCommand("SELECT * FROM Usuario WHERE NombreUsuario = @username", sqlConnection))
                {
                    sqlConnection.Open();

                    sqlCommand.Parameters.AddWithValue("@username", username);
                    SqlDataAdapter SqlAdapter = new SqlDataAdapter();
                    SqlAdapter.SelectCommand = sqlCommand;
                    SqlAdapter.Fill(temp_table);

                    foreach (DataRow line in temp_table.Rows)
                    {
                        Usuario usuario = new Usuario();
                        usuario.Id = Convert.ToInt32(line["Id"]);
                        usuario.Nombre = line["Nombre"].ToString();
                        usuario.Apellido = line["Apellido"].ToString();
                        usuario.Contraseña = line["Contraseña"].ToString();
                        usuario.NombreUsuario = line["NombreUsuario"].ToString();
                        usuario.Mail = line["Mail"].ToString();
                        Lista_usuario.Add(usuario);
                    }
                    sqlConnection.Close();
                }
            }
            return Lista_usuario;
        }

        //Este método va a validar los datos de acceso de un usuario
        public List<Usuario> ValidarAccesoUsuario(string DBConn, string user, string psw)
        {
            //Tabla temporal para almacenarar el resultado de la consulta
            DataTable temp_table = new DataTable();

            List<Usuario> Lista_usuario = new List<Usuario>();

            using (SqlConnection sqlConnection = new SqlConnection(DBConn))
            {
                using (SqlCommand sqlCommand = new SqlCommand("SELECT * FROM Usuario WHERE NombreUsuario = @user and Contraseña = @psw", sqlConnection))
                {
                    sqlConnection.Open();

                    sqlCommand.Parameters.AddWithValue("@user", user);
                    sqlCommand.Parameters.AddWithValue("@psw", psw);
                    SqlDataAdapter SqlAdapter = new SqlDataAdapter();
                    SqlAdapter.SelectCommand = sqlCommand;
                    SqlAdapter.Fill(temp_table);

                    if (temp_table.Rows.Count == 0)
                    {
                        //Lista_usuario.Rows[0].;

                    }
                    else
                    {
                        foreach (DataRow line in temp_table.Rows)
                        {
                            Usuario usuario = new Usuario();
                            usuario.Id = Convert.ToInt32(line["Id"]);
                            usuario.Nombre = line["Nombre"].ToString();
                            usuario.Apellido = line["Apellido"].ToString();
                            usuario.Mail = line["Mail"].ToString();
                            Lista_usuario.Add(usuario);
                        }
                    }
                    sqlConnection.Close();
                }
            }
            return Lista_usuario;
        }

        //Este método va a mostrar una lista con los datos del Usuario consultado
        public void MostrarDatosUsuario(List<Usuario> Lista_usuario)
        {
            Console.WriteLine("\n----------------------------------------------------------------------------------------");
            Console.WriteLine("{0,5}{1,10}{2,5}{3,10}{4,15}{5,22}", "ID".PadRight(5), "Nombre".PadRight(12), "Apellido".PadRight(15), "NombreUsuario".PadRight(15), "Contraseña".PadRight(15), "Mail".PadRight(20));
            Console.WriteLine("----------------------------------------------------------------------------------------");
            foreach (var linea in Lista_usuario)
            {
                //Console.WriteLine(linea.Id + "\t" + linea.Nombre + "\t" + linea.Apellido + "\t" + linea.NombreUsuario + "\t" + linea.Contraseña + "\t" + linea.Mail);
                Console.WriteLine("{0,5}{1,10}{2,5}{3,10}{4,15}{5,22}", linea.Id.ToString().PadRight(5), linea.Nombre.PadRight(12), linea.Apellido.PadRight(15), linea.NombreUsuario.PadRight(15), linea.Contraseña.PadRight(15), linea.Mail.PadRight(20));
            }
            Console.WriteLine("\n");
        }

    }
}
