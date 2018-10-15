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
            loginOk = false;
            Login l = new Login();

            while (!loginOk)
            {
                DialogResult res = l.ShowDialog();

                if (res == DialogResult.OK)
                {
                    string nombre = l.txtUsuario.Text;
                    string contraseña = l.txtContraseña.Text;

                    c = new Cliente();
                    c.conectarServidor();
                    c.mandarMensaje("LOGIN:" + nombre + "," + contraseña);
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


        }
    }
}
