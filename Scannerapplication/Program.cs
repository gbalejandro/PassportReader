using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Forms;

namespace Scannerapplication
{
    static class Program
    {
        private static string _reserva;
        private static string _sesion;
        private static string _ipBD;
        private static string _serverBD;
        private static string _usuarioBD;
        private static string _passBD;
        /// <summary>
        /// The main entry point for the application.
        /// </summary>
        [STAThread]
        static void Main(string[] args)
        {
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);
            Application.Run(new Form1(args));
        }

        public static string Reserva
        {
            get { return _reserva; }
            set { _reserva = value; }
        }

        public static string Sesion
        {
            get { return _sesion; }
            set { _sesion = value; }
        }

        public static string ipBD
        {
            get { return _ipBD; }
            set { _ipBD = value; }
        }

        public static string serverBD
        {
            get { return _serverBD; }
            set { _serverBD = value; }
        }

        public static string usuarioBD
        {
            get { return _usuarioBD; }
            set { _usuarioBD = value; }
        }

        public static string passBD
        {
            get { return _passBD; }
            set { _passBD = value; }
        }
    }
}
