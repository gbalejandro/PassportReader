using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.Common;
using System.Drawing;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace Scannerapplication
{
    public partial class FrmReservaciones : Form
    {
        public static string reservacion;

        public FrmReservaciones()
        {
            InitializeComponent();
            //buscaReservaciones();
        }

        private void buscaReservaciones()
        {
            DateTime Hoy = DateTime.Today;
            DateTime ayer = Hoy.AddDays(-1);
            string fecha_ayer = ayer.ToString("dd-MMM-yyyy", CultureInfo.CreateSpecificCulture("en-US"));
            string fecha_actual = Hoy.ToString("dd-MMM-yyyy", CultureInfo.CreateSpecificCulture("en-US"));

            UConnection DB2 = new UConnection(Program.ipBD, Program.serverBD, Program.usuarioBD, Program.passBD);
            string sql2 = "select a.vn_reserva, a.vn_apellido, a.vn_nombre, b.rv_agencia, b.rv_voucher, (b.rv_adulto+b.rv_menor+b.rv_junior) PAX " + 
                            "from freserno a inner join freserva b on a.vn_reserva = b.rv_reserva " + 
                            "where a.vn_secuencia = 1 and a.vn_apellido like '%" + Form1.apellido + "%' and b.rv_llegada between '" + fecha_ayer + "' and '" + fecha_actual + "' " +
                                                    "and b.rv_status = 'R'";

            try
            {
                var dt = new DataTable();

                if (DB2.EjecutaSQL(sql2))
                {
                    if (DB2.ora_DataReader.HasRows)
                    {
                        dt.Load(DB2.ora_DataReader);
                        //dgvReservaciones.AutoGenerateColumns = true;
                        dgvReservaciones.DataSource = dt;
                    }
                    else
                    {
                        MessageBox.Show("LA CONSULTA NO DEVOLVIÓ NINGUNA RESERVACIÓN CON ESE APELLIDO.", "¡ALERTA!");
                        this.Close();
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("NO FUE POSIBLE OBTENER RESERVACIONES\n\n" + ex.Message);
            }
            finally
            {
                DB2.Dispose();
            }
        }

        private void FrmReservaciones_Load(object sender, EventArgs e)
        {
            buscaReservaciones();
        }

        private void btnAceptar_Click(object sender, EventArgs e)
        {
            DataGridViewRow row = dgvReservaciones.CurrentRow;

            if (row != null)
            {
                reservacion = row.Cells[0].Value.ToString();
                this.Close();
            }
        }
    }
}
