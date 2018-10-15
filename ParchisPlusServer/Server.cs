using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Net.Sockets;
using System.Net;
using System.Threading;
using System.IO;

namespace ParchisPlusServer
{
    class Server
    {
        public static readonly Object l = new Object();
        public static Socket socketServer;
        public static ManejadorBaseDeDatos bd;
        public static List<Cliente> jugadores;
        public static List<Partida> partidas;

        public Server()
        {
            jugadores = new List<Cliente>();
            partidas = new List<Partida>();

            bd = new ManejadorBaseDeDatos();
            bd.conectar();
        }

        static void Main(string[] args)
        {
            Server server = new Server();
            IPEndPoint ie = new IPEndPoint(IPAddress.Any, 31416);
            socketServer = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
            socketServer.Bind(ie);
            socketServer.Listen(10);
            Console.WriteLine("Servidor a la espera en {0}", ie.Port);
            while (true)
            {
                Socket socketCliente = socketServer.Accept();
                HiloCliente hiloCliente = new HiloCliente(socketCliente);
                Thread hilo = new Thread(hiloCliente.hiloCliente);
                hilo.Start();
            }
        }

        
    }
}
