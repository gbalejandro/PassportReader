using System.Windows.Forms;
namespace Scannerapplication
{
    partial class Form1
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.showImages = new System.Windows.Forms.Button();
            this.splitter1 = new System.Windows.Forms.Splitter();
            this.pnl_capture = new System.Windows.Forms.Panel();
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.radioButton1 = new System.Windows.Forms.RadioButton();
            this.radioButton2 = new System.Windows.Forms.RadioButton();
            this.btnBuscar = new System.Windows.Forms.Button();
            this.btnLeer = new System.Windows.Forms.Button();
            this.pic_scan = new System.Windows.Forms.PictureBox();
            this.lbDevices = new System.Windows.Forms.ListBox();
            this.btn_scan = new System.Windows.Forms.Button();
            this.pnl_capture.SuspendLayout();
            this.groupBox1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.pic_scan)).BeginInit();
            this.SuspendLayout();
            // 
            // showImages
            // 
            this.showImages.Location = new System.Drawing.Point(242, 216);
            this.showImages.Name = "showImages";
            this.showImages.Size = new System.Drawing.Size(85, 23);
            this.showImages.TabIndex = 5;
            this.showImages.Text = "Show Images";
            this.showImages.UseVisualStyleBackColor = true;
            this.showImages.Visible = false;
            // 
            // splitter1
            // 
            this.splitter1.Dock = System.Windows.Forms.DockStyle.Top;
            this.splitter1.Location = new System.Drawing.Point(0, 0);
            this.splitter1.Name = "splitter1";
            this.splitter1.Size = new System.Drawing.Size(802, 3);
            this.splitter1.TabIndex = 38;
            this.splitter1.TabStop = false;
            // 
            // pnl_capture
            // 
            this.pnl_capture.BackColor = System.Drawing.Color.Transparent;
            this.pnl_capture.Controls.Add(this.groupBox1);
            this.pnl_capture.Controls.Add(this.btnBuscar);
            this.pnl_capture.Controls.Add(this.btnLeer);
            this.pnl_capture.Controls.Add(this.pic_scan);
            this.pnl_capture.Controls.Add(this.lbDevices);
            this.pnl_capture.Controls.Add(this.btn_scan);
            this.pnl_capture.Dock = System.Windows.Forms.DockStyle.Fill;
            this.pnl_capture.Location = new System.Drawing.Point(0, 3);
            this.pnl_capture.Name = "pnl_capture";
            this.pnl_capture.Size = new System.Drawing.Size(802, 399);
            this.pnl_capture.TabIndex = 39;
            // 
            // groupBox1
            // 
            this.groupBox1.Controls.Add(this.radioButton1);
            this.groupBox1.Controls.Add(this.radioButton2);
            this.groupBox1.Location = new System.Drawing.Point(12, 100);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(148, 100);
            this.groupBox1.TabIndex = 11;
            this.groupBox1.TabStop = false;
            this.groupBox1.Text = "Documento";
            // 
            // radioButton1
            // 
            this.radioButton1.AutoSize = true;
            this.radioButton1.Checked = true;
            this.radioButton1.ForeColor = System.Drawing.SystemColors.ActiveCaptionText;
            this.radioButton1.Location = new System.Drawing.Point(26, 29);
            this.radioButton1.Name = "radioButton1";
            this.radioButton1.Size = new System.Drawing.Size(73, 17);
            this.radioButton1.TabIndex = 9;
            this.radioButton1.TabStop = true;
            this.radioButton1.Text = "Pasaporte";
            this.radioButton1.UseVisualStyleBackColor = true;
            // 
            // radioButton2
            // 
            this.radioButton2.AutoSize = true;
            this.radioButton2.ForeColor = System.Drawing.SystemColors.ActiveCaptionText;
            this.radioButton2.Location = new System.Drawing.Point(26, 66);
            this.radioButton2.Name = "radioButton2";
            this.radioButton2.Size = new System.Drawing.Size(36, 17);
            this.radioButton2.TabIndex = 10;
            this.radioButton2.Text = "ID";
            this.radioButton2.UseVisualStyleBackColor = true;
            // 
            // btnBuscar
            // 
            this.btnBuscar.Enabled = false;
            this.btnBuscar.ForeColor = System.Drawing.SystemColors.ActiveCaptionText;
            this.btnBuscar.Location = new System.Drawing.Point(38, 64);
            this.btnBuscar.Name = "btnBuscar";
            this.btnBuscar.Size = new System.Drawing.Size(75, 30);
            this.btnBuscar.TabIndex = 8;
            this.btnBuscar.Text = "Buscar";
            this.btnBuscar.UseVisualStyleBackColor = true;
            this.btnBuscar.Click += new System.EventHandler(this.btnBuscar_Click);
            // 
            // btnLeer
            // 
            this.btnLeer.Enabled = false;
            this.btnLeer.ForeColor = System.Drawing.SystemColors.ActiveCaptionText;
            this.btnLeer.Location = new System.Drawing.Point(38, 212);
            this.btnLeer.Name = "btnLeer";
            this.btnLeer.Size = new System.Drawing.Size(75, 30);
            this.btnLeer.TabIndex = 7;
            this.btnLeer.Text = "Guardar";
            this.btnLeer.UseVisualStyleBackColor = true;
            this.btnLeer.Click += new System.EventHandler(this.btnGuardar_Click);
            // 
            // pic_scan
            // 
            this.pic_scan.BackColor = System.Drawing.Color.White;
            this.pic_scan.Location = new System.Drawing.Point(166, 21);
            this.pic_scan.Name = "pic_scan";
            this.pic_scan.Size = new System.Drawing.Size(609, 355);
            this.pic_scan.TabIndex = 6;
            this.pic_scan.TabStop = false;
            this.pic_scan.Visible = false;
            // 
            // lbDevices
            // 
            this.lbDevices.FormattingEnabled = true;
            this.lbDevices.Location = new System.Drawing.Point(12, 250);
            this.lbDevices.Name = "lbDevices";
            this.lbDevices.Size = new System.Drawing.Size(120, 30);
            this.lbDevices.TabIndex = 5;
            this.lbDevices.Visible = false;
            // 
            // btn_scan
            // 
            this.btn_scan.ForeColor = System.Drawing.SystemColors.ActiveCaptionText;
            this.btn_scan.Location = new System.Drawing.Point(38, 26);
            this.btn_scan.Name = "btn_scan";
            this.btn_scan.Size = new System.Drawing.Size(75, 30);
            this.btn_scan.TabIndex = 4;
            this.btn_scan.Text = "Escanear";
            this.btn_scan.UseVisualStyleBackColor = true;
            this.btn_scan.Click += new System.EventHandler(this.btn_scan_Click);
            // 
            // Form1
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch;
            this.ClientSize = new System.Drawing.Size(802, 402);
            this.Controls.Add(this.pnl_capture);
            this.Controls.Add(this.splitter1);
            this.ForeColor = System.Drawing.SystemColors.ControlLightLight;
            this.Name = "Form1";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "Passport Reader";
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.Form1_FormClosing);
            this.Load += new System.EventHandler(this.Form1_Load);
            this.SizeChanged += new System.EventHandler(this.Home_SizeChanged);
            this.pnl_capture.ResumeLayout(false);
            this.groupBox1.ResumeLayout(false);
            this.groupBox1.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.pic_scan)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion



        //camera
        private System.Windows.Forms.Button showImages;
        private Splitter splitter1;
        private Panel pnl_capture;
        private PictureBox pic_scan;
        private ListBox lbDevices;
        private Button btn_scan;
        private Button btnLeer;
        private Button btnBuscar;
        private GroupBox groupBox1;
        private RadioButton radioButton1;
        private RadioButton radioButton2;
    }
}

