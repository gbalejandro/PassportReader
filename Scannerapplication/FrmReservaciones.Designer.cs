namespace Scannerapplication
{
    partial class FrmReservaciones
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
            this.dgvReservaciones = new System.Windows.Forms.DataGridView();
            this.label1 = new System.Windows.Forms.Label();
            this.btnAceptar = new System.Windows.Forms.Button();
            this.VN_RESERVA = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.VN_APELLIDO = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.VN_NOMBRE = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.RV_AGENCIA = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.RV_VOUCHER = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.PAX = new System.Windows.Forms.DataGridViewTextBoxColumn();
            ((System.ComponentModel.ISupportInitialize)(this.dgvReservaciones)).BeginInit();
            this.SuspendLayout();
            // 
            // dgvReservaciones
            // 
            this.dgvReservaciones.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dgvReservaciones.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.VN_RESERVA,
            this.VN_APELLIDO,
            this.VN_NOMBRE,
            this.RV_AGENCIA,
            this.RV_VOUCHER,
            this.PAX});
            this.dgvReservaciones.Location = new System.Drawing.Point(28, 53);
            this.dgvReservaciones.Name = "dgvReservaciones";
            this.dgvReservaciones.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect;
            this.dgvReservaciones.Size = new System.Drawing.Size(820, 187);
            this.dgvReservaciones.TabIndex = 0;
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(25, 20);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(302, 13);
            this.label1.TabIndex = 1;
            this.label1.Text = "SELECCIONE LA RESERVACIÓN Y DE CLICK EN ACEPTAR";
            // 
            // btnAceptar
            // 
            this.btnAceptar.Location = new System.Drawing.Point(54, 259);
            this.btnAceptar.Name = "btnAceptar";
            this.btnAceptar.Size = new System.Drawing.Size(75, 23);
            this.btnAceptar.TabIndex = 2;
            this.btnAceptar.Text = "Aceptar";
            this.btnAceptar.UseVisualStyleBackColor = true;
            this.btnAceptar.Click += new System.EventHandler(this.btnAceptar_Click);
            // 
            // VN_RESERVA
            // 
            this.VN_RESERVA.DataPropertyName = "VN_RESERVA";
            this.VN_RESERVA.HeaderText = "RESERVACIÓN";
            this.VN_RESERVA.Name = "VN_RESERVA";
            // 
            // VN_APELLIDO
            // 
            this.VN_APELLIDO.DataPropertyName = "VN_APELLIDO";
            this.VN_APELLIDO.HeaderText = "APELLIDO";
            this.VN_APELLIDO.Name = "VN_APELLIDO";
            this.VN_APELLIDO.Width = 150;
            // 
            // VN_NOMBRE
            // 
            this.VN_NOMBRE.DataPropertyName = "VN_NOMBRE";
            this.VN_NOMBRE.HeaderText = "NOMBRE";
            this.VN_NOMBRE.Name = "VN_NOMBRE";
            this.VN_NOMBRE.Width = 150;
            // 
            // RV_AGENCIA
            // 
            this.RV_AGENCIA.DataPropertyName = "RV_AGENCIA";
            this.RV_AGENCIA.HeaderText = "AGENCIA";
            this.RV_AGENCIA.Name = "RV_AGENCIA";
            this.RV_AGENCIA.Width = 150;
            // 
            // RV_VOUCHER
            // 
            this.RV_VOUCHER.DataPropertyName = "RV_VOUCHER";
            this.RV_VOUCHER.HeaderText = "VOUCHER";
            this.RV_VOUCHER.Name = "RV_VOUCHER";
            // 
            // PAX
            // 
            this.PAX.DataPropertyName = "PAX";
            this.PAX.HeaderText = "PAX";
            this.PAX.Name = "PAX";
            // 
            // FrmReservaciones
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(897, 311);
            this.Controls.Add(this.btnAceptar);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.dgvReservaciones);
            this.Name = "FrmReservaciones";
            this.Text = "Seleccion de Reservaciones";
            this.Load += new System.EventHandler(this.FrmReservaciones_Load);
            ((System.ComponentModel.ISupportInitialize)(this.dgvReservaciones)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.DataGridView dgvReservaciones;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Button btnAceptar;
        private System.Windows.Forms.DataGridViewTextBoxColumn VN_RESERVA;
        private System.Windows.Forms.DataGridViewTextBoxColumn VN_APELLIDO;
        private System.Windows.Forms.DataGridViewTextBoxColumn VN_NOMBRE;
        private System.Windows.Forms.DataGridViewTextBoxColumn RV_AGENCIA;
        private System.Windows.Forms.DataGridViewTextBoxColumn RV_VOUCHER;
        private System.Windows.Forms.DataGridViewTextBoxColumn PAX;
    }
}