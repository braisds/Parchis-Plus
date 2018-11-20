using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace ParchisPlusCliente
{
    public partial class Principal : Form
    {
        Cliente c;
        bool loginOk;

        public Principal()
        {
            InitializeComponent();
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            c = new Cliente();
            c.conectarServidor();

            loginOk = false;
            
            while (!loginOk)
            {
                Console.WriteLine("refre");
                Login l = new Login();
                DialogResult res = l.ShowDialog();
                
                if (res == DialogResult.OK)
                {
                    string nombre = l.txtUsuario.Text;
                    string contraseña = l.txtContraseña.Text;

                    if (nombre != null && contraseña != null)
                    {
                        
                        c.mandarMensaje("LOGIN:" + nombre + "," + contraseña);
                    }
                    else
                    {
                        l.lblError.Text = "Inserte el nombre y la contraseña";
                    }
                    
                }
                else
                {
                    this.Close();
                }
            }
            
        }

        

        private void loginRespuesta(String mensajeRespuesta)
        {

            if(mensajeRespuesta == "OK")
            {
                loginOk = true;
            }
            else
            {
                
            }


        }

        private void nuevoJuegoToolStripMenuItem_Click(object sender, EventArgs e)
        {
            c.mandarMensaje("NUEVOJUEGO:");
        }

        private void pictureBox1_MouseDown(object sender, MouseEventArgs e)
        {
            Console.WriteLine("X: " + e.X + " Y: " + e.Y);
        }
    }
}
