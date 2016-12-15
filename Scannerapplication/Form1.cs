using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Drawing.Imaging;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using WIATest;
using Tesseract;
using System.Diagnostics;
using System.Text.RegularExpressions;
using System.IO;
using System.Globalization;
using System.Threading;
using System.Timers;
using Oracle.DataAccess.Client;

namespace Scannerapplication
{
    public partial class Form1 : Form
    {
        List<Image> images;
        Image imagenReal;
        private string nombreImagen;
        private int random; // es para generar un numero aleatorio cuando son varias imagenes por reservacion
        private int tipoMov; // tipo de movimiento 1. escaneo y guardado 2. buscar reserva
        public static string apellido; // informacion que se va a pasar al otro form
        public Thread th1;
        static frmSplash splash;
        const int kSplashUpdateInterval_ms = 1;
        private int randomID, cuentaPasa;

        public Form1(string[] args)
        {
            InitializeComponent();

            nombreImagen = args[0]; // el nombre como identificador de la/s imagen/es hotel + año + numero de reserva
            Program.Reserva = args[1]; // numero de reserva hotelera
            Program.Sesion = args[2]; // sesion del sistema oracle forms
            tipoMov = Convert.ToInt16(args[3]);

            // Consulto el archivo txt que contiene cada unidad de negocio para traer los parametros de conexión
            StringBuilder ip = new StringBuilder();
            StringBuilder server = new StringBuilder();
            StringBuilder user = new StringBuilder();
            StringBuilder pass = new StringBuilder();
            StringBuilder uneg = new StringBuilder();
            StringBuilder ambiente = new StringBuilder();

            string archivo = "C:\\\\componentePagos\\BDParam.ini";

            if (File.Exists(archivo))
            {
                Util.GetPrivateProfileString("BDUNeg", "ip", "", ip, ip.Capacity, archivo);
                Program.ipBD = ip.ToString();
                Util.GetPrivateProfileString("BDUNeg", "server", "", server, server.Capacity, archivo);
                Program.serverBD = server.ToString();
                Util.GetPrivateProfileString("BDUNeg", "user", "", user, user.Capacity, archivo);
                Program.usuarioBD = user.ToString();
                Util.GetPrivateProfileString("BDUNeg", "pass", "", pass, pass.Capacity, archivo);
                Program.passBD = pass.ToString();
            }
            ////////////Fin consulta txt//////////////
        }

        static public void StartSplash()
        {
            // Instance a splash form given the image names
            splash = new frmSplash(kSplashUpdateInterval_ms);
            splash.StartPosition = FormStartPosition.CenterScreen;
            // Run the form
            Application.Run(splash);
        }

        private void CloseSplash()
        {
            if (splash == null)
                return;

            // Shut down the splash screen
            splash.Invoke(new EventHandler(splash.KillMe));
            splash.Dispose();
            splash = null;
        }

        private void btn_scan_Click(object sender, EventArgs e)
        {
            try
            {
                // obtengo la lista de dispositivos disponibles
                List<string> devices = WIAScanner.GetDevices();

                foreach (string device in devices)
                {
                    lbDevices.Items.Add(device);
                }
                // si no se encuentra disponiblee
                if (lbDevices.Items.Count == 0)
                {
                    MessageBox.Show("No se encuentra conectado ningun dispositivo para escanear.");
                    this.Close();
                }
                else
                {
                    lbDevices.SelectedIndex = 0;
                }
                // obtiene la imagen del escaner
                images = WIAScanner.Scan((string)lbDevices.SelectedItem);

                foreach (Image image in images)
                {
                    pic_scan.Image = image;
                    pic_scan.Image.RotateFlip(RotateFlipType.Rotate270FlipNone);
                    pic_scan.Show();
                    pic_scan.SizeMode = PictureBoxSizeMode.StretchImage;
                    imagenReal = image;
                }

                if (tipoMov == 1)
                {
                    btnLeer.Enabled = true;
                } else
                {
                    btnBuscar.Enabled = true;
                }
            }
            catch (Exception exc)
            {
                MessageBox.Show(exc.Message);
            }
        }

        private void Home_SizeChanged(object sender, EventArgs e)
        {
            int pheight = this.Size.Height - 153;
            pic_scan.Size = new Size(pheight - 150, pheight);
        }

        private void btnGuardar_Click(object sender, EventArgs e)
        {
            //random = 1;
            var tessData = @"C:\tessdata";
            int count = 0;
            string linea1 = "", linea2 = "", identificadorDoc = "", nombre = "";
            bool ok = false;
            int edad = 0;
            string codPais = "", carpeta="";

            // obtengo la ruta real de las imagenes
            // le quito el año a las siglas del hotel
            string inputHotel = "";
            inputHotel = nombreImagen.Substring(0, nombreImagen.Length - 2);

            // consulto la fecha de llegada de la reserva
            UConnection DB = new UConnection(Program.ipBD, Program.serverBD, Program.usuarioBD, Program.passBD);
            try
            {
                string comandoSql4 = string.Format("select * from freserva where rv_reserva = '{0}'", Program.Reserva);
                if (DB.EjecutaSQL(comandoSql4))
                {
                    if (DB.ora_DataReader.HasRows)
                    {
                        while (DB.ora_DataReader.Read())
                        {
                            if (!DBNull.Value.Equals(DB.ora_DataReader["RV_LLEGADA"]))
                            {
                                DateTime fec = Convert.ToDateTime(DB.ora_DataReader["RV_LLEGADA"]);
                                string anio = fec.ToString("yyyy");
                                string mes = fec.ToString("MM");
                                string dia = fec.ToString("dd");
                                carpeta = @"\\192.168.1.243\Backups\ScannerOCR\" + inputHotel + "\\" + anio + "\\" + mes + "\\" + dia + "\\";
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("NO SE PUDO OBTENER LA FECHA DE LLEGADA DE LA RESERVACIÓN \n" + ex.Message);
            }
            finally
            {
                DB.Dispose();
            }

            // si no existe el directorio, lo creo
            if (!Directory.Exists(carpeta))
            {
                System.IO.Directory.CreateDirectory(carpeta);
            }

            var testImagePath = carpeta + nombreImagen + Program.Sesion + ".tif";

            if (pic_scan.Image != null)
            {
                try
                {
                    Thread splashThread = new Thread(new ThreadStart(StartSplash));
                    splashThread.Start();
                    //this.Visible = false;
                    imagenReal.Save(testImagePath, System.Drawing.Imaging.ImageFormat.Tiff);

                    try
                    {
                        using (var engine = new TesseractEngine(tessData, "eng", EngineMode.Default))
                        {
                            using (var img = Pix.LoadFromFile(testImagePath))
                            {
                                using (var page = engine.Process(img))
                                {
                                    var text = page.GetText();
                                    string[] words = text.Split('\n');
                                    for (int i = words.Length - 1; i > 0; --i)
                                    {
                                        var linea = words[i];
                                        if (linea != "" && linea != " ")
                                        {
                                            ++count;
                                            if (count == 1)
                                            {
                                                linea2 = linea;
                                            }
                                            else
                                            {
                                                linea1 = linea;
                                            }

                                            if (count == 2)
                                                i = 0;
                                        }
                                    }
                                    // verifico si es pasaporte o identificacion a escanear
                                    if (radioButton1.Checked)
                                    {
                                        identificadorDoc = linea1.Substring(0, 1);
                                        // primero verifico que contenga los caracteres < para que no truene
                                        if (identificadorDoc == "P")
                                        {
                                            int startIndex = 1;

                                            // apellidos
                                            int endIndex = linea1.IndexOf("<<", startIndex);
                                            string apellidos = linea1.Substring(startIndex, endIndex - startIndex);
                                            apellidos = apellidos.Replace("<", " ");
                                            apellidos = apellidos.TrimStart();
                                            apellidos = apellidos.Replace("0", "O");
                                            // obtengo el codigo del pais
                                            codPais = apellidos.Substring(0, 3);
                                            // para quitar el código de país
                                            apellidos = apellidos.Substring(3, apellidos.Length - 3);
                                            // verifico con esta clase si tiene dos apellidos
                                            if (apellidos.Contains(" "))
                                            {
                                                // para extraer un apellido
                                                int endIndexA = apellidos.IndexOf(" ", 0);
                                                apellido = apellidos.Substring(0, endIndexA);
                                            }
                                            else
                                            {
                                                apellido = apellidos;
                                            }
                                            //apellidos = Microsoft.VisualBasic.Interaction.InputBox(@"Si los Apellidos obtenidos del Pasaporte no son los correctos, " +
                                            //    "cambielos y ponga Aceptar\n\n" + apellidos, "Apellidos", apellidos);
                                            // fin apellidos
                                            // nombres
                                            int endIndexN = linea1.IndexOf("<<<", endIndex);
                                            string nombres = linea1.Substring(endIndex, endIndexN - endIndex);
                                            nombres = nombres.Replace("<", " ");
                                            nombres = nombres.TrimStart();
                                            // verifico si son dos apellidos
                                            if (nombres.Contains(" "))
                                            {
                                                // para extraer un nombre
                                                int endIndexNo = nombres.IndexOf(" ", 0);
                                                nombre = nombres.Substring(0, endIndexNo);
                                            }
                                            else
                                            {
                                                nombre = nombres;
                                            }
                                            // fin nombres
                                        

                                            // numero de pasaporte
                                            int endIndexP = linea2.IndexOf(codPais);
                                            string sPasaporte = linea2.Substring(0, endIndexP - 1);
                                            // si contiene el digito, se lo quito
                                            string numPasaporte = "";
                                            if (sPasaporte.Contains("<"))
                                            {
                                                numPasaporte = sPasaporte.Replace("<", "");
                                            }
                                            else
                                            {
                                                numPasaporte = sPasaporte;
                                            }
                                            // fin pasaporte
                                            // fecha nacimiento para la edad
                                            int startIndexCP = linea2.IndexOf(codPais);
                                            string fechaNac = linea2.Substring(startIndexCP + 3, 6);

                                            int f = 0;
                                            bool result = int.TryParse(fechaNac, out f);
                                            // es para extraer la edad por medio de la fecha de nacimiento
                                            if (result)
                                            {
                                                int dia = Convert.ToInt16(fechaNac.Substring(4, 2));
                                                int mes = Convert.ToInt16(fechaNac.Substring(2, 2));
                                                int anio = Convert.ToInt16(fechaNac.Substring(0, 2));
                                                // obtengo el año actual
                                                DateTime Hoy = DateTime.Today;
                                                string anioActual = Hoy.ToString("yy");
                                                int anioA = Convert.ToInt16(anioActual);
                                                if (anio > anioA)
                                                {
                                                    anio = anio + 1900;
                                                }
                                                else
                                                {
                                                    anio = anio + 2000;
                                                }
                                                DateTime dt = new DateTime(anio, mes, dia);
                                                //DateTime nacimiento = new DateTime(dt); //Fecha de nacimiento
                                                edad = DateTime.Today.AddTicks(-dt.Ticks).Year - 1;
                                                // fin fecha nacimiento
                                            }

                                            int filas = 0;

                                            // primero consulto si ya hay un huesped asignado a la reserva
                                            UConnection DB1 = new UConnection(Program.ipBD, Program.serverBD, Program.usuarioBD, Program.passBD);
                                            string sql = "select MAX(VN_SECUENCIA) VN_SECUENCIA from freserno where vn_reserva = '" + Program.Reserva + "' and vn_pasaporte != 'null'";
                                            ++cuentaPasa; // las veces que se escanea un pasaporte
                                            try
                                            {
                                                if (DB1.EjecutaSQL(sql))
                                                {
                                                    string comandoSqls = string.Empty;
                                                    // si existe, no elimino el primer registro porque ya tiene pasaporte
                                                    if (DB1.ora_DataReader.HasRows)
                                                    {
                                                        while (DB1.ora_DataReader.Read())
                                                        {
                                                            if (!DBNull.Value.Equals(DB1.ora_DataReader["VN_SECUENCIA"]))
                                                            {
                                                                random = Convert.ToInt16(DB1.ora_DataReader["VN_SECUENCIA"]);
                                                                // nada mas elimino los que no tengan pasaporte
                                                                comandoSqls = string.Format("delete from freserno where vn_reserva = '{0}' and vn_secuencia > " + random, Program.Reserva);
                                                                ok = DB1.EjecutaSQL(comandoSqls, ref filas);
                                                            }
                                                            else // si no, me plancho todos y llevo la secuencia del escaneo
                                                            {
                                                                comandoSqls = string.Format("delete from freserno where vn_reserva = '{0}'", Program.Reserva);
                                                                ok = DB1.EjecutaSQL(comandoSqls, ref filas);
                                                            }
                                                        }
                                                    }
                                                    ++random;
                                                }
                                                // si pasa la primera vez, inserto el pais
                                                if (cuentaPasa == 1)
                                                {
                                                    string comandoSql3 = string.Format("update freserva set rv_pais = '{0}' where rv_reserva = '{1}'", codPais, Program.Reserva);
                                                    ok = DB1.EjecutaSQL(comandoSql3, ref filas);
                                                    //++cuentaPasa;
                                                }
                                                // Inserto la respuesta en la base de datos
                                                string comandoSql = string.Format("insert into freserno (vn_reserva,vn_secuencia,vn_apellido,vn_nombre,vn_pase,vn_cupon,vn_edad,vn_edo_civil,vn_tipo_tarjeta,vn_extra_llegada,vn_extra_salida,vn_extra_status,vn_extra_importe,vn_extra_transa,vn_extra_cheque,vn_pasaporte,vn_en_casa,vn_en_casa_f) values ('{0}',{1},'{2}','{3}','{4}','{5}',{6},'{7}','{8}','{9}','{10}','{11}',{12},'{13}','{14}','{15}',{16},'{17}')",
                                                Program.Reserva, random, apellido, nombre, null, null, edad, null, null, null, null, null, 0.0, null, null, numPasaporte, 0, null);

                                                ok = DB1.EjecutaSQL(comandoSql, ref filas);
                                                // obtengo la ip del cliente
                                                //string name = System.Net.Dns.GetHostName();
                                                //string ip = System.Net.Dns.GetHostAddresses(name)[0].ToString();

                                                var testImagePathE = carpeta + nombreImagen + Program.Reserva + identificadorDoc + random + ".jpg";
                                                imagenReal.Save(testImagePathE, System.Drawing.Imaging.ImageFormat.Jpeg);

                                            }
                                            catch (Exception ex)
                                            {
                                                CloseSplash();
                                                MessageBox.Show("ERROR EN EL FRONT: " + ex.Message);
                                                // elimino el archivo ya guardado
                                                FileInfo file = new FileInfo(testImagePath);
                                                if (file.Exists)
                                                {
                                                    file.Delete();
                                                }
                                                pic_scan.Image = null;
                                            }
                                            finally
                                            {
                                                DB1.Dispose();
                                            }

                                            if (filas > 0)
                                            {
                                                CloseSplash();
                                                MessageBox.Show("SE GUARDARON SATISFACTORIAMENTE LA IMAGEN Y LOS DATOS.", "¡ÉXITO!");
                                                // elimino el archivo ya guardado
                                                FileInfo file = new FileInfo(testImagePath);
                                                if (file.Exists)
                                                {
                                                    file.Delete();
                                                }
                                                pic_scan.Image = null;
                                            }
                                            else
                                            {
                                                CloseSplash();
                                                MessageBox.Show("NO SE GUARDARON LOS DATOS SATISFACTORIAMENTE", "FAVOR DE REVISAR");
                                                // elimino el archivo ya guardado
                                                FileInfo file = new FileInfo(testImagePath);
                                                if (file.Exists)
                                                {
                                                    file.Delete();
                                                }
                                                pic_scan.Image = null;
                                            }
                                        }
                                        else
                                        {

                                                CloseSplash();
                                                // elimino el archivo ya guardado
                                                FileInfo file = new FileInfo(testImagePath);
                                                if (file.Exists)
                                                {
                                                    file.Delete();
                                                }
                                                pic_scan.Image = null;
                                                // y envio el mensaje de que no se escaneo satisfactoriamente
                                                MessageBox.Show("NO FUE POSIBLE OBTENER LOS DATOS DEL DOCUMENTO\nVUELVA A ESCANEARLO POR FAVOR.", "¡ERROR!");
                                        }
                                    }
                                    else
                                    {
                                        CloseSplash();
                                        // elimino el archivo ya guardado
                                        FileInfo file = new FileInfo(testImagePath);
                                        if (file.Exists)
                                        {
                                            file.Delete();
                                        }
                                        var testImagePathID = carpeta + nombreImagen + Program.Reserva + "ID" + ++randomID + ".jpg";
                                        // verifico el id, si ya existe le agrego otro numero
                                        FileInfo file2 = new FileInfo(testImagePathID);
                                        if (file2.Exists)
                                        {
                                            Random random = new Random();
                                            int randomNumber = random.Next(0, 1000);
                                            testImagePathID = carpeta + nombreImagen + Program.Reserva + "ID" + randomID + Convert.ToString(randomNumber) + ".jpg";
                                        }
                                        imagenReal.Save(testImagePathID, System.Drawing.Imaging.ImageFormat.Jpeg);
                                        MessageBox.Show("SE GUARDÓ SATISFACTORIAMENTE LA IDENTIFICACIÓN.", "INFORMACIÓN");
                                        pic_scan.Image = null;
                                    } // verifica el tipo de documento P o ID
                                }
                            }
                        }
                    }
                    catch (Exception ex)
                    {
                        CloseSplash();
                        MessageBox.Show("OCURRIÓ UN ERROR EN LA LECTURA DEL DOCUMENTO.\n\n" + ex.Message, "VUELVA A ESCANEAR");
                        pic_scan.Image = null;
                        --random;
                    }
                }
                catch (Exception ex)
                {
                    // Do some exception handling here
                }
                finally
                {
                    // After all is done, close your splash. Put it here, so that if your code throws an exception, the finally will close the splash form
                    CloseSplash();
                }
            }
            else
            {
                MessageBox.Show("DEBE ESCANEAR UNA IMAGEN PARA SER LEÍDA.", "ALERTA!");
            }
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            if (tipoMov == 1)
            {
                btnBuscar.Visible = false;
            }
            else
            {
                btnLeer.Visible = false;
                groupBox1.Visible = false;
            }
        }

        private void btnBuscar_Click(object sender, EventArgs e)
        {
            var testImagePath = @"\\192.168.1.243\Backups\ScannerOCR\" + nombreImagen + Program.Sesion + ".tif";
            var tessData = @"C:\tessdata";
            int count = 0;
            string linea1 = "", linea2 = "", identificadorDoc = "";
            int edad = 0;
            string carpeta = "";

            if (pic_scan.Image != null)
            {
                Thread splashThread = new Thread(new ThreadStart(StartSplash));
                splashThread.Start();
                imagenReal.Save(testImagePath, System.Drawing.Imaging.ImageFormat.Tiff);

                try
                {
                    using (var engine = new TesseractEngine(tessData, "eng", EngineMode.Default))
                    {
                        using (var img = Pix.LoadFromFile(testImagePath))
                        {
                            using (var page = engine.Process(img))
                            {
                                var text = page.GetText();
                                string[] words = text.Split('\n');
                                for (int i = words.Length - 1; i > 0; --i)
                                {
                                    var linea = words[i];
                                    if (linea != "" && linea != " ")
                                    {
                                        ++count;
                                        if (count == 1)
                                        {
                                            linea2 = linea;
                                        }
                                        else
                                        {
                                            linea1 = linea;
                                        }

                                        if (count == 2)
                                            i = 0;
                                    }
                                }

                                identificadorDoc = linea1.Substring(0, 1);
                                // primero verifico que contenga los caracteres < para que no truene
                                if (identificadorDoc == "P")
                                {
                                    // inicio desde la segunda posición
                                    int startIndex = 1;

                                    // apellido
                                    int endIndex = linea1.IndexOf("<<", startIndex);
                                    string apellidos = linea1.Substring(startIndex, endIndex - startIndex);
                                    apellidos = apellidos.Replace("<", " ");
                                    apellidos = apellidos.TrimStart();
                                    apellidos = apellidos.Replace("0", "O");
                                    // verifico si tiene dos apellidos para extraer uno
                                    if (apellidos.Contains(" "))
                                    {
                                        int endIndexDos = apellidos.IndexOf(" ", 0);
                                        apellidos = apellidos.Substring(0, endIndexDos);
                                    }
                                    // obtengo el codigo del pais
                                    string codPais = apellidos.Substring(0, 3);
                                    // para quitar el código de país
                                    apellido = apellidos.Substring(3, apellidos.Length - 3);
                                    // extrae el numero de pasaporte
                                    // numero de pasaporte
                                    int endIndexP = linea2.IndexOf(codPais);
                                    string sPasaporte = linea2.Substring(0, endIndexP - 1);
                                    // si contiene el digito, se lo quito
                                    string numPasaporte = "";
                                    if (sPasaporte.Contains("<"))
                                    {
                                        numPasaporte = sPasaporte.Replace("<", "");
                                    }
                                    else
                                    {
                                        numPasaporte = sPasaporte;
                                    }
                                    // fin pasaporte
                                    // fecha nacimiento para la edad
                                    int startIndexCP = linea2.IndexOf(codPais);
                                    string fechaNac = linea2.Substring(startIndexCP + 3, 6);

                                    int f = 0;
                                    bool result = int.TryParse(fechaNac, out f);
                                    // es para extraer la edad por medio de la fecha de nacimiento
                                    if (result)
                                    {
                                        int diaE = Convert.ToInt16(fechaNac.Substring(4, 2));
                                        int mesE = Convert.ToInt16(fechaNac.Substring(2, 2));
                                        int anioE = Convert.ToInt16(fechaNac.Substring(0, 2));
                                        // obtengo el año actual
                                        DateTime Hoy2 = DateTime.Today;
                                        string anioActual = Hoy2.ToString("yy");
                                        int anioA = Convert.ToInt16(anioActual);
                                        if (anioE > anioA)
                                        {
                                            anioE = anioE + 1900;
                                        }
                                        else
                                        {
                                            anioE = anioE + 2000;
                                        }
                                        DateTime dt = new DateTime(anioE, mesE, diaE);
                                        //DateTime nacimiento = new DateTime(dt); //Fecha de nacimiento
                                        edad = DateTime.Today.AddTicks(-dt.Ticks).Year - 1;
                                        // fin fecha nacimiento
                                    }
                                    // consulto la fecha de 
                                    DateTime Hoy = DateTime.Today;
                                    DateTime ayer = Hoy.AddDays(-1);
                                    string fecha_actual = Hoy.ToString("dd-MMM-yyyy", CultureInfo.CreateSpecificCulture("en-US"));
                                    string fecha_ayer = ayer.ToString("dd-MMM-yyyy", CultureInfo.CreateSpecificCulture("en-US"));

                                    UConnection DB2 = new UConnection(Program.ipBD, Program.serverBD, Program.usuarioBD, Program.passBD);
                                    string sql2 = "select a.vn_reserva, a.vn_apellido, a.vn_nombre, b.rv_agencia, b.rv_voucher, (b.rv_adulto+b.rv_menor+b.rv_junior) PAX " +
                                                    "from freserno a inner join freserva b on a.vn_reserva = b.rv_reserva " +
                                                    "where a.vn_secuencia = 1 and a.vn_apellido like '%" + apellido + "%' and b.rv_llegada between '" + fecha_ayer + "' and '" + fecha_actual + "' " +
                                                    "and b.rv_status = 'R'";

                                    try
                                    {
                                        var dt = new DataTable();

                                        if (DB2.EjecutaSQL(sql2))
                                        {
                                            if (DB2.ora_DataReader.HasRows)
                                            {
                                                CloseSplash();
                                                FrmReservaciones frm = new FrmReservaciones();
                                                frm.StartPosition = FormStartPosition.CenterScreen;
                                                frm.ShowDialog();
                                            }
                                            else
                                            {
                                                CloseSplash();
                                                apellido = Microsoft.VisualBasic.Interaction.InputBox(@"LA CONSULTA NO DEVOLVIÓ NINGUNA RESERVACIÓN CON ESE APELLIDO., " +
                                                    "CAMBIELO Y PONGA ACEPTAR\n\n" + apellido, "APELLIDO A BUSCAR", apellido);
                                                // si el apellido regresa vacío, no abro el grid de reservaciones
                                                if (apellido != "")
                                                {
                                                    FrmReservaciones frm = new FrmReservaciones();
                                                    frm.StartPosition = FormStartPosition.CenterScreen;
                                                    frm.ShowDialog();
                                                }
                                            }
                                        }
                                    }
                                    catch (Exception ex)
                                    {
                                        MessageBox.Show("OCURRIÓ UN ERROR EN LA CONSULTA A LA BASE DE DATOS\n\n" + ex.Message, "¡ERROR!");
                                    }
                                    finally
                                    {
                                        DB2.Dispose();
                                    }
                                    // si se encontró el numero de reservacion
                                    if (FrmReservaciones.reservacion != null)
                                    {
                                        int filas = 0;
                                        bool ok;
                                        UConnection DB = new UConnection(Program.ipBD, Program.serverBD, Program.usuarioBD, Program.passBD);
                                        string comandoSql3 = string.Format("update freserva set rv_pais = '{0}' where rv_reserva = '{1}'", codPais, FrmReservaciones.reservacion);
                                        ok = DB.EjecutaSQL(comandoSql3, ref filas);
                                        // inserto el numero de pasaporte en freserno
                                        string comandoSql2 = string.Format("update freserno set vn_pasaporte = '{0}', vn_edad = {1} where vn_reserva = '{2}' and vn_secuencia = 1",
                                            numPasaporte, edad, FrmReservaciones.reservacion);                                        
                                        ok = DB.EjecutaSQL(comandoSql2, ref filas);
                                        // inserto el mapeo de la sesion de oracle con el numero de reservacion
                                        string comandoSql = string.Format("insert into SCPASMAP (RESERVA,SESION) values ('{0}','{1}')",
                                        FrmReservaciones.reservacion, Program.Sesion);
                                        ok = DB.EjecutaSQL(comandoSql, ref filas);
                                        
                                        CloseSplash();
                                        if (filas > 0)
                                        {
                                            FileInfo file = new FileInfo(testImagePath);
                                            if (file.Exists)
                                            {
                                                file.Delete();
                                            }
                                            // le quito el año a las siglas del hotel
                                            string inputHotel = "";
                                            inputHotel = nombreImagen.Substring(0, nombreImagen.Length - 2);

                                            // consulto la fecha de llegada de la reserva
                                            string comandoSql4 = string.Format("select * from freserva where rv_reserva = '{0}'", FrmReservaciones.reservacion);
                                            if (DB.EjecutaSQL(comandoSql4))
                                            {
                                                if (DB.ora_DataReader.HasRows)
                                                {
                                                    while (DB.ora_DataReader.Read())
                                                    {
                                                        if (!DBNull.Value.Equals(DB.ora_DataReader["RV_LLEGADA"]))
                                                        {
                                                            DateTime fec = Convert.ToDateTime(DB.ora_DataReader["RV_LLEGADA"]);
                                                            string anio = fec.ToString("yyyy");
                                                            string mes = fec.ToString("MM");
                                                            string dia = fec.ToString("dd");
                                                            carpeta = @"\\192.168.1.243\Backups\ScannerOCR\" + inputHotel + "\\" + anio + "\\" + mes + "\\" + dia + "\\";
                                                        }
                                                    }
                                                }
                                            }                                            
                                            // si no existe el directorio, lo creo
                                            if (!Directory.Exists(carpeta))
                                            {
                                                System.IO.Directory.CreateDirectory(carpeta);
                                            }

                                            var testImagePathE = carpeta + nombreImagen + FrmReservaciones.reservacion + identificadorDoc + ++random + ".jpg";
                                            imagenReal.Save(testImagePathE, System.Drawing.Imaging.ImageFormat.Jpeg);
                                            this.Close();
                                        }
                                        // cierro la conexión
                                        DB.Dispose();
                                    }
                                    else
                                    {
                                        // elimino el archivo ya guardado
                                        FileInfo file = new FileInfo(testImagePath);
                                        if (file.Exists)
                                        {
                                            file.Delete();
                                        }
                                        pic_scan.Image = null;
                                    }
                                }
                                else
                                {
                                    CloseSplash();
                                    // y envio el mensaje de que no se escaneo satisfactoriamente
                                    MessageBox.Show("EL ESCANER NO RECONOCE EL DOCUMENTO\n\nCOMO UN PASAPORTE\n\nVUELVA A ESCANEARLO POR FAVOR.", "¡ERROR!");
                                    // elimino el archivo ya guardado
                                    FileInfo file = new FileInfo(testImagePath);
                                    if (file.Exists)
                                    {
                                        file.Delete();
                                    }
                                    pic_scan.Image = null;
                                }
                            }
                        }
                    }
                }
                catch (Exception ex)
                {
                    CloseSplash();
                    pic_scan.Image = null;
                    if (random != 0)
                    {
                        --random;
                    }
                    // elimino el archivo ya guardado
                    FileInfo file = new FileInfo(testImagePath);
                    if (file.Exists)
                    {
                        file.Delete();
                    }
                    MessageBox.Show("OCURRIÓ UN ERROR QUE NO PERMITIÓ OBTENER LA IMAGEN.\n\n" + ex.Message, "VUELVA A ESCANEAR");
                }
            }
            else
            {
                MessageBox.Show("DEBE ESCANEAR UNA IMAGEN PARA SER LEÍDA.", "ALERTA!");
            } // picturebox nulo
            //Cursor.Current = Cursors.Default;
        }

        private void Form1_FormClosing(object sender, FormClosingEventArgs e)
        {
            // Make sure the splash screen is closed
            CloseSplash();
            base.OnClosing(e);
        }
    }
}
