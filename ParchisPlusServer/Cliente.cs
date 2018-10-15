using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;

namespace ParchisPlusServer
{
    class Cliente
    {
        public Socket SocketCliente { get; set; }
        public Usuario Usuario { get; set; }

        public Cliente(Socket socketCliente, Usuario usuario)
        {
            SocketCliente = socketCliente;
            Usuario = usuario;
        }
    }
}
