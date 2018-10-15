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
        string mensajeEnviar;
        public string mensajeRespuesta;

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

                while (true)
                {
                    if (mensajeEnviar != null)
                    {
                        mandarMensaje(mensajeEnviar);

                        mensajeRespuesta = sr.ReadLine();
                        if (mensajeRespuesta != null)
                        {
                            InterpretarMensaje(mensajeRespuesta);
                        }
                    }
                    else
                    {

                    }

                    
                }


            }
            catch (SocketException e)
            {

            }

            finally
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



        public void mandarMensaje(string mensaje)
        {
            sw.WriteLine(mensaje);
            sw.Flush();
        }

        private void InterpretarMensaje(string mensaje)
        {
            Console.WriteLine(mensaje);
            String[] array = mensaje.Split(':');
            String comando = array[0];
            String parametros = array[1];

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


    }
}
