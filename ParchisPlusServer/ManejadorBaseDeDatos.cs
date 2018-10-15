using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ParchisPlusServer
{
    class ManejadorBaseDeDatos
    {
        MySqlConnection conexion;

        public ManejadorBaseDeDatos()
        {
            conexion = new MySqlConnection("Server=127.0.0.1;Database=parchisplusdb;Uid=root;Pwd=;SslMode=none");
        }
        public void conectar()
        {
            conexion.Open();
            Console.WriteLine("ok");
            
        }

        public void cerrar()
        {
            conexion.Close();
        }

        public Boolean loginUsuario(string nombre,string contraseña)
        {
            string consulta = "Select * from usuario where nombre like '"+nombre+"' and contraseña like '"+contraseña+"'";
            Console.WriteLine(consulta);
            MySqlCommand stmt = new MySqlCommand(consulta, conexion);
            Boolean resultado = false;

            MySqlDataReader myreader = stmt.ExecuteReader();
            if (myreader.Read())
            {
                resultado = true;
            }

            if (myreader != null)
            {
                myreader.Close();
            }

            return resultado;

        }
        public Boolean registrarUsuario(string nombre, string contraseña)
        {
            string consulta = "insert into usuario (nombre, contraseña) vaues ("+nombre+","+contraseña+")";

            MySqlCommand stmt = new MySqlCommand(consulta, conexion);

            

            return false;

        }

        public Usuario getUsuario(string nombre)
        {
            string consulta = "Select id, nombre, puntosTotales from usuario where nombre like '" + nombre + "'";
            Console.WriteLine(consulta);
            MySqlCommand stmt = new MySqlCommand(consulta, conexion);
            Usuario u = null;
            MySqlDataReader myreader = stmt.ExecuteReader();

            if (myreader.Read())
            {
                u = new Usuario();
                u.CodUsuario = myreader.GetInt32("id"); Console.WriteLine(u.CodUsuario);
                u.Nombre = myreader.GetString("nombre");
                u.PuntosTotales = myreader.GetInt32("puntosTotales");
            }
            

            if (myreader != null)
            {
                myreader.Close();
            }

            return u;

        }
    }
}
