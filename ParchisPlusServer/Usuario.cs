using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ParchisPlusServer
{
    class Usuario
    {
        public int CodUsuario;
        public String Nombre;
        public int PuntosTotales;

        public Usuario(int codUsuario, string nombre, int puntosTotales)
        {
            CodUsuario = codUsuario;
            Nombre = nombre;
            PuntosTotales = puntosTotales;
        }

        public Usuario()
        {
        }
    }
}
