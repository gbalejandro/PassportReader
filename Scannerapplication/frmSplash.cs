using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace Scannerapplication
{
    public partial class frmSplash : Form
    {
        System.Threading.Timer splashTimer = null;
        int curAnimCell = 0;
        int numUpdates = 0;
        int timerInterval_ms = 0;

        public frmSplash(int timerInterval)
        {
            timerInterval_ms = timerInterval;
            InitializeComponent();
        }

        public int GetUpMilliseconds()
        {
            return numUpdates * timerInterval_ms;
        }

        public void KillMe(object o, EventArgs e)
        {
            //splashTimer.Dispose();

            this.Close();
        }

        private void frmSplash_Load(object sender, EventArgs e)
        {
            this.Text = "";
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.ControlBox = false;
            this.FormBorderStyle = FormBorderStyle.None;
            this.Menu = null;
        }
    }
}
