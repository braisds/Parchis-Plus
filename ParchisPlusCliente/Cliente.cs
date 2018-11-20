using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace ParchisPlusCliente
{
    
    class Cliente
    {
        IPEndPoint ipServidor;
        Socket servidor;
        NetworkStream ns=null;
        StreamReader sr=null;
        StreamWriter sw=null;
        public string mensaje;
        Thread hiloRecibir;

        public Cliente()
        {
            conectarServidor();
        }

        public void conectarServidor()
        {
            ipServidor = new IPEndPoint(IPAddress.Parse("127.0.0.1"), 31416);
            servidor = new Socket(AddressFamily.InterNetwork,
                SocketType.Stream, ProtocolType.Tcp);

            try
            {
                servidor.Connect(ipServidor);

                ns = new NetworkStream(servidor);
                sw = new StreamWriter(ns);
                sr = new StreamReader(ns);

                hiloRecibir = new Thread(new ThreadStart(HiloRecibir));
                hiloRecibir.Start();

                Principal p = new Principal();
                p.Show();
            }
            catch (SocketException)
            {
                cerrarConexion();
            }

        }

        public void HiloRecibir()
        {
            while (true)
            {
                mensaje = sr.ReadLine();

                if (mensaje != null)
                {
                    InterpretarMensaje(mensaje);
                }

            }

        }


        public void mandarMensaje(string mensaje)
        {
            sw.WriteLine(mensaje);
            Console.WriteLine(mensaje);
            sw.Flush();
        }

        private void InterpretarMensaje(string mensaje)
        {
            Console.WriteLine(mensaje);
            string[] array = mensaje.Split(':');
            string comando = array[0];
            string parametros = "";
            if (array.Length == 2 )
            {
                parametros = array[1];
            }
            
            switch (comando)
            {
                case "LOGIN":
                    ResultadoLogin(parametros);
                    break;
                case "TIRAR":
                    resultadoDado(parametros);
                    break;
                case "MOVER":
                    ResultadoMoverPieza(parametros);
                    break;
                case "NUEVOJUEGO":
                    NuevoJuego(parametros);
                    break;
                default:
                    Console.WriteLine("COMANDO NO VALIDO");
                    break;
            }

        }
        //usuario - contraseña
        private void ResultadoLogin(string mensaje)
        {
            Console.WriteLine("Login");

            if (mensaje == "OK")
            {

            }
            else
            {

            }

        }

        //
        private void resultadoDado(string mensaje)
        {
            Console.WriteLine("TIRAR");


        }

        //codgame - jugador - pieza - desde - hasta
        private void ResultadoMoverPieza(string mensaje)
        {
            Console.WriteLine("MOVER");

        }


        private void NuevoJuego(string mensaje)
        {
            Console.WriteLine("NUEVOJUEGO");

        }


        private void cerrarConexion()
        {
            if (sw != null)
            {
                sw.Close();
            }
            if (sr != null)
            {
                sr.Close();
            }
            if (ns != null)
            {
                ns.Close();
            }
            if (servidor != null)
            {
                servidor.Close();
            }

        }

    }
}
