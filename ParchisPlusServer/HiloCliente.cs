using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;

namespace ParchisPlusServer
{
    class HiloCliente
    {
        Socket socketCliente;
        IPEndPoint ieCliente;
        NetworkStream ns;
        StreamReader sr;
        StreamWriter sw;
        string mensaje;
        string mensajeRespuesta;


        public HiloCliente(Socket socket)
        {
            socketCliente = socket;
            ieCliente = (IPEndPoint)socketCliente.RemoteEndPoint;
            lock (Server.l)
            {
                Console.WriteLine("Conectado con el cliente {0} en el puerto {1}", ieCliente.Address, ieCliente.Port);
            }
            ns = new NetworkStream(socketCliente);
            sw = new StreamWriter(ns);
            sr = new StreamReader(ns);
        }

        public void hiloCliente()
        {
            while (true)
            {
                try
                {
                    mensaje = sr.ReadLine();//Recibir
                    if (mensaje != null)
                    {
                        lock (Server.l)
                        {
                            Console.WriteLine("Mensaje: " + mensaje);
                        }
                            
                        InterpretarMensaje();
                        if (mensajeRespuesta != null)
                        {
                            lock (Server.l) { 

                                Console.WriteLine("Mensaje Respuesta: " + mensaje);
                            }
                            sw.WriteLine(mensajeRespuesta);//Enviar
                            sw.Flush();
                            
                            
                        }
                    }

                }
                catch (IOException)
                {
                    //Salta al acceder al socket
                    //y no estar permitido
                    break;
                }
            }
            Console.WriteLine("Conexión finalizada con {0}:{1}", ieCliente.Address, ieCliente.Port);

            if (sw != null)
            {
                sw.Close();
            }
            if (sr !=null)
            {
                sr.Close();
            }
            if (ns!= null)
            {
                ns.Close();
            }
            if (socketCliente != null)
            {
                socketCliente.Close();
            }
            
        }

        private void InterpretarMensaje()
        {
            //COMANDO:PARAMENTO1,PARAMETRO2,...
            String[] array = mensaje.Split(':');
            String comando = array[0];
            String parametros = array[1];
            Console.WriteLine("cmado: " + comando + " param" + parametros);
            switch (comando)
            {
                case "LOGIN":
                    login(parametros);
                    break;
                case "CERRARSESION":
                    cerrarSesion(parametros);
                    break;
                case "TIRAR":
                    //tirarDado(parametros);
                    break;
                case "MOVER":
                    //moverPieza(parametros);
                    break;
                case "NUEVOJUEGO":
                    //NuevoJuego(parametros);
                    break;
                default:
                    Console.WriteLine("COMANDO NO VALIDO");
                    break;
            }

        }
        //usuario - contraseña
        private void login(string parametros)
        {
            mensajeRespuesta = "LOGIN:FALLO";
            string[] valores = parametros.Split(',');
            if (Server.bd.loginUsuario(valores[0], valores[1]))
            {
                mensajeRespuesta = "LOGIN:OK";
                lock (Server.l)
                {
                    Server.jugadores.Add(new Cliente(socketCliente, Server.bd.getUsuario(valores[0])));
                }

                foreach (Cliente a in Server.jugadores)
                {
                    Console.WriteLine("cliente "+a.Usuario.Nombre+" " + a.Usuario.CodUsuario);
                }
            }

        }

        //usuario - contraseña
        private String cerrarSesion(string mensaje)
        {
            Console.WriteLine("CERRARSESION");
            String mensajeRespuesta = "CERRARSESION:FALLO";
            string[] valores = mensaje.Split(',');
            //if (Server.bd.loginUsuario(valores[0], valores[1]))
            {
                mensajeRespuesta = "CERRARSESION:OK";
            }
            Console.WriteLine("mensajeRespuesta CERRARSESION: " + mensajeRespuesta);
            return mensajeRespuesta;

        }

        //
        private void tirarDado(string mensaje)
        {
            //sw.WriteLine("TIRAR");

        }

        //codgame - jugador - pieza - desde - hasta
        private void moverPieza(string mensaje)
        {
            //sw.WriteLine("MOVER");

        }


        private void NuevoJuego(string mensaje)
        {
            //sw.WriteLine("NUEVOJUEGO");

        }

    }
}
