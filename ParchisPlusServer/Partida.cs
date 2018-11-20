using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ParchisPlusServer
{
    enum eFicha{
        Amarillo,
        AZUL,
        ROJO,
        VERDE,
    }
    enum tablero
    {
        Casa = 0,
        Pasillo = 200,
        Meta = 208
    }

    class Partida
    {
        public int partidaID { get; set; }
        public List<Cliente> participantes { get; set; }
        private int[,] posicionPiezas;
        public int JugadorActivo = 0;
        private int numTiradas = 0;
        private int dado = -1;
        public int piezaSeleccionada = -1;

        public Partida(int partidaID, List<Cliente> participantes)
        {
            this.partidaID = partidaID;
            this.participantes = participantes;
            this.posicionPiezas = new int[4,4];
        }

        private void iniciarJuego()
        {

        }

        private int TirarDado()
        {
            Random random = new Random();
            dado = random.Next(7);
            Console.WriteLine(dado);

            numTiradas++;
            if (numTiradas >= 3 && dado == 6)
            {
                siguienteJugador();

            }
            else
            {
                if (TodasEnCasa())
                {

                    if (dado != 5)
                    {
                        siguienteJugador();
                    }

                }
                else
                {
                    if (puedeMover())
                    {
                        numTiradas++;
                        if (dado == 6 && numTiradas >= 3)
                        {
                            siguienteJugador();
                        }
                    }
                    else
                    {
                        siguienteJugador();
                    }
                }

            }

            return dado;
        }

        private void siguienteJugador()
        {
            numTiradas = 0;
            dado = 0;

            JugadorActivo = (JugadorActivo + 1) % 4;
        }

        private bool puedeMover()
        {
            bool puede = false;


            for (int pieza = 0; pieza < 4; pieza++)
            {

                bool piezaEnCasa = posicionPiezas[JugadorActivo, pieza] == (int) tablero.Casa;

                if ( dado == 5 && piezaEnCasa && !casillaBloqueada( 5 + JugadorActivo * 17)) //Salida jugador actual
                {
                    return true;
                }
                else
                {
                   
                }

            }
            return puede;
        }


        private bool casillaBloqueada(int casilla)
        {
            int cont = 0;
            
            foreach (int c in posicionPiezas)
            {
                if (c == casilla)
                {
                    cont++;
                    if (cont >= 2)
                    {
                        return true;
                    }
                }
            }

            return false;
        }

        private bool recorridoBloqueado(int origen, int destino)
        {
            int cont = 0;
            int casilla = 0;

            for (int i = origen + 1; i<=destino; i++)
            {
                foreach(int c in posicionPiezas)
                {
                    if (c == casilla)
                    {
                        cont++;
                        if (cont >= 2)
                        {
                            return true;
                        }
                    }
                }

            }

            return false;
        }


        private bool recorridoComer(int origen, int destino)
        {

            for (int jugador=0; jugador < 4; jugador++)
            {
                if (jugador != JugadorActivo)
                {
                    for (int pieza = 0; pieza < 4; pieza++)
                    {
                        if (posicionPiezas[jugador,pieza] > origen && posicionPiezas[jugador, pieza] <= destino)
                        {
                            posicionPiezas[jugador, pieza] = (int)tablero.Casa;
                        }
                    }
                }
                
            }

            return false;
        }


        public bool moverPieza(int jugador, int origen)
        {
            bool mover = false;

            if (dado == 5 && origen == 0 && !casillaBloqueada(5 + JugadorActivo * 17))
            {
                posicionPiezas[jugador, piezaSeleccionada] = 5 + JugadorActivo * 17;//salida jugador actual
                recorridoComer((5 + JugadorActivo * 17) -1 ,(5 + JugadorActivo * 17));//comer casa jugador actual
                mover = true;
            }
            else
            {
                if (!recorridoBloqueado(origen, origen + dado))
                {
                    if (origen + dado <= (17 + JugadorActivo * 17))//Antes de llegar al pasillo del jugador actual
                    {
                        posicionPiezas[jugador, piezaSeleccionada] = origen + dado;
                        recorridoComer(origen, origen + dado);//comer recorrido
                        mover = true;
                    }
                    else
                    {
                        int n = dado - ( (17 + jugador * 17) - origen); //numero de casillas restantes al llegar al pasillo
                        int pasillo = (int)tablero.Pasillo + (JugadorActivo * 10) + n ; // posicion en el pasillo

                        posicionPiezas[jugador, piezaSeleccionada] = pasillo;
                        mover = true;
                    }
                }
                
                
            }

            if (mover)
            {
                dado = 0;
                piezaSeleccionada = -1;
            }

            return mover;
        }

        private bool TodasEnCasa()
        {
            for (int i=0;i<4;i++)
            {
                if(posicionPiezas[JugadorActivo,i] != 0)
                {
                    return false;
                }
            }

            return true;
        }

        
    }

}
