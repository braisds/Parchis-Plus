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
        public static readonly Object lockListaEspera = new object();
        Socket socketCliente;
        IPEndPoint ieCliente;
        NetworkStream ns;
        StreamReader sr;
        StreamWriter sw;
        string mensaje;
        string mensajeRespuesta;
        private Cliente cliente;
        private Partida partidaActual;


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
            string[] array = mensaje.Split(':');
            string comando = array[0];
            string parametros = "";
            if (array.Length == 2)
            {
                parametros = array[1];
            }
            
            Console.WriteLine("comado: " + comando + " param" + parametros);
            switch (comando)
            {
                case "LOGIN":
                    login(parametros);
                    break;
                case "CERRARSESION":
                    cerrarSesion(parametros);
                    break;
                case "REGISTRO":
                    registro(parametros);
                    break;
                case "TIRAR":
                    //tirarDado(parametros);
                    break;
                case "MOVER":
                    //moverPieza(parametros);
                    break;
                case "NUEVOJUEGO":
                    NuevoJuego();
                    break;
                case "NUEVOJUEGOCANCELAR":
                    //NuevoJuegoCancelar(parametros);
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
            //if (Server.bd.loginUsuario(valores[0], valores[1]))
            if (valores[0] == "abc" && valores[1] == "123")
            {
                mensajeRespuesta = "LOGIN:OK";
                lock (Server.l)
                {
                    cliente = new Cliente(socketCliente, new Usuario(1, "abc", 100)/*Server.bd.getUsuario(valores[0])*/);
                    Server.jugadores.Add(cliente);
                }

            }
            enviar(mensajeRespuesta);

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
        //usuario - contraseña
        private void registro(string mensaje)
        {
            Console.WriteLine("REGISTRO");
            string[] valores = mensaje.Split(',');
            if (Server.bd.getUsuario != null)
            {
                
            }
        }

        
        private void tirarDado(string mensaje)
        {
            //sw.WriteLine("TIRAR");


        }

        //codgame - jugador - pieza - desde - hasta
        private void moverPieza(string mensaje)
        {
            //sw.WriteLine("MOVER");

        }


        private void NuevoJuego()
        {
            //sw.WriteLine("NUEVOJUEGO");
            lock (lockListaEspera)
            {
                Server.listaEspera.Add(cliente);
                //TODO:Añadir a bd
                enviar("NUEVOJUEGO:listaEsperaOK");

                if (Server.listaEspera.Count == 4)
                {

                    //Todo: insert bd partida para consegir el id y quitar el 0
                    Server.partidas.Add(new Partida(0, Server.listaEspera));

                    partidaActual = Server.partidas[Server.partidas.Count - 1];

                    //NUEVOJUEGOOK:idPartida,jugador0,jugador1,jugador2,jugador3
                    string msj = string.Format("NUEVOJUEGOOK:{0},", partidaActual.partidaID);
                    foreach (Cliente p in partidaActual.participantes)
                    {
                        msj += p.Usuario.Nombre+",";
                    }

                    Server.enviarMensajeVarios(msj, Server.listaEspera);
                    Server.listaEspera.Clear();
                }
            }
        }

        private void enviar(string mensaje)
        {
            sw.WriteLine(mensaje);
            sw.Flush();
        }

    }
}
