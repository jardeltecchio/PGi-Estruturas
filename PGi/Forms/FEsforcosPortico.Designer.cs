namespace PG
{
    partial class FEsforcosPortico
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
            this.components = new System.ComponentModel.Container();
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(FEsforcosPortico));
            this.cbCombinacao = new System.Windows.Forms.ComboBox();
            this.label1 = new System.Windows.Forms.Label();
            this.imageList1 = new System.Windows.Forms.ImageList(this.components);
            this.lbDica = new System.Windows.Forms.Label();
            this.trackEscalaDiagrama = new System.Windows.Forms.TrackBar();
            this.panel1 = new System.Windows.Forms.Panel();
            this.btPorticoFX = new System.Windows.Forms.Button();
            this.btPorticoMX = new System.Windows.Forms.Button();
            this.btPorticoMZ = new System.Windows.Forms.Button();
            this.btPorticoMY = new System.Windows.Forms.Button();
            this.btPorticoFZ = new System.Windows.Forms.Button();
            this.btPorticoFY = new System.Windows.Forms.Button();
            this.btPorticoDz = new System.Windows.Forms.Button();
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.trackEscalaCarga = new System.Windows.Forms.TrackBar();
            this.chMostrarCargasPontuais = new System.Windows.Forms.CheckBox();
            this.chMostrarCargasLineares = new System.Windows.Forms.CheckBox();
            this.label2 = new System.Windows.Forms.Label();
            ((System.ComponentModel.ISupportInitialize)(this.trackEscalaDiagrama)).BeginInit();
            this.groupBox1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.trackEscalaCarga)).BeginInit();
            this.SuspendLayout();
            // 
            // cbCombinacao
            // 
            this.cbCombinacao.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cbCombinacao.Font = new System.Drawing.Font("Tahoma", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.cbCombinacao.FormattingEnabled = true;
            this.cbCombinacao.Items.AddRange(new object[] {
            "1",
            "2",
            "3"});
            this.cbCombinacao.Location = new System.Drawing.Point(6, 22);
            this.cbCombinacao.Name = "cbCombinacao";
            this.cbCombinacao.Size = new System.Drawing.Size(326, 21);
            this.cbCombinacao.TabIndex = 34;
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Font = new System.Drawing.Font("Tahoma", 8.5F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label1.ForeColor = System.Drawing.SystemColors.MenuText;
            this.label1.Location = new System.Drawing.Point(3, 6);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(136, 14);
            this.label1.TabIndex = 35;
            this.label1.Text = "Hipóteses/Combinações";
            // 
            // imageList1
            // 
            this.imageList1.ImageStream = ((System.Windows.Forms.ImageListStreamer)(resources.GetObject("imageList1.ImageStream")));
            this.imageList1.TransparentColor = System.Drawing.Color.Transparent;
            this.imageList1.Images.SetKeyName(0, "Axial.bmp");
            this.imageList1.Images.SetKeyName(1, "Cortante y.bmp");
            this.imageList1.Images.SetKeyName(2, "Cortante z.bmp");
            this.imageList1.Images.SetKeyName(3, "Deslocamento.bmp");
            this.imageList1.Images.SetKeyName(4, "Fletor y.bmp");
            this.imageList1.Images.SetKeyName(5, "Fletor z.bmp");
            this.imageList1.Images.SetKeyName(6, "Torcor.bmp");
            // 
            // lbDica
            // 
            this.lbDica.AutoSize = true;
            this.lbDica.Location = new System.Drawing.Point(146, 86);
            this.lbDica.Name = "lbDica";
            this.lbDica.Size = new System.Drawing.Size(10, 13);
            this.lbDica.TabIndex = 43;
            this.lbDica.Text = " ";
            // 
            // trackEscalaDiagrama
            // 
            this.trackEscalaDiagrama.AutoSize = false;
            this.trackEscalaDiagrama.LargeChange = 1;
            this.trackEscalaDiagrama.Location = new System.Drawing.Point(257, 49);
            this.trackEscalaDiagrama.Maximum = 20;
            this.trackEscalaDiagrama.Name = "trackEscalaDiagrama";
            this.trackEscalaDiagrama.Size = new System.Drawing.Size(81, 23);
            this.trackEscalaDiagrama.TabIndex = 69;
            this.trackEscalaDiagrama.Value = 10;
            this.trackEscalaDiagrama.Scroll += new System.EventHandler(this.trackEscalaDiagrama_Scroll);
            // 
            // panel1
            // 
            this.panel1.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.panel1.Location = new System.Drawing.Point(147, 47);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(2, 24);
            this.panel1.TabIndex = 71;
            // 
            // btPorticoFX
            // 
            this.btPorticoFX.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.btPorticoFX.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
          //  this.btPorticoFX.Image = global::PGi.Properties.Resources.Axial;
            this.btPorticoFX.Location = new System.Drawing.Point(111, 47);
            this.btPorticoFX.Name = "btPorticoFX";
            this.btPorticoFX.Size = new System.Drawing.Size(29, 23);
            this.btPorticoFX.TabIndex = 39;
            this.btPorticoFX.Tag = "0";
            this.btPorticoFX.UseVisualStyleBackColor = true;
            this.btPorticoFX.Click += new System.EventHandler(this.button1_Click);
            this.btPorticoFX.MouseEnter += new System.EventHandler(this.btPorticoFX_MouseEnter);
            this.btPorticoFX.MouseLeave += new System.EventHandler(this.btPorticoDz_MouseLeave);
            // 
            // btPorticoMX
            // 
            this.btPorticoMX.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
          //  this.btPorticoMX.Image = global::PGi.Properties.Resources.Torcor;
            this.btPorticoMX.Location = new System.Drawing.Point(224, 47);
            this.btPorticoMX.Name = "btPorticoMX";
            this.btPorticoMX.Size = new System.Drawing.Size(29, 23);
            this.btPorticoMX.TabIndex = 42;
            this.btPorticoMX.Tag = "0";
            this.btPorticoMX.UseVisualStyleBackColor = true;
            this.btPorticoMX.Click += new System.EventHandler(this.button1_Click);
            this.btPorticoMX.MouseEnter += new System.EventHandler(this.btPorticoMX_MouseEnter);
            this.btPorticoMX.MouseLeave += new System.EventHandler(this.btPorticoDz_MouseLeave);
            // 
            // btPorticoMZ
            // 
            this.btPorticoMZ.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
          //  this.btPorticoMZ.Image = global::PGi.Properties.Resources.Fletor_z;
            this.btPorticoMZ.Location = new System.Drawing.Point(190, 47);
            this.btPorticoMZ.Name = "btPorticoMZ";
            this.btPorticoMZ.Size = new System.Drawing.Size(29, 23);
            this.btPorticoMZ.TabIndex = 41;
            this.btPorticoMZ.Tag = "0";
            this.btPorticoMZ.UseVisualStyleBackColor = true;
            this.btPorticoMZ.Click += new System.EventHandler(this.button1_Click);
            this.btPorticoMZ.MouseEnter += new System.EventHandler(this.btPorticoMZ_MouseEnter);
            this.btPorticoMZ.MouseLeave += new System.EventHandler(this.btPorticoDz_MouseLeave);
            // 
            // btPorticoMY
            // 
            this.btPorticoMY.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
          //  this.btPorticoMY.Image = global::PGi.Properties.Resources.Fletor_y;
            this.btPorticoMY.Location = new System.Drawing.Point(156, 47);
            this.btPorticoMY.Name = "btPorticoMY";
            this.btPorticoMY.Size = new System.Drawing.Size(29, 23);
            this.btPorticoMY.TabIndex = 40;
            this.btPorticoMY.Tag = "0";
            this.btPorticoMY.UseVisualStyleBackColor = true;
            this.btPorticoMY.Click += new System.EventHandler(this.button1_Click);
            this.btPorticoMY.MouseEnter += new System.EventHandler(this.btPorticoMY_MouseEnter);
            this.btPorticoMY.MouseLeave += new System.EventHandler(this.btPorticoDz_MouseLeave);
            // 
            // btPorticoFZ
            // 
            this.btPorticoFZ.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
         //   this.btPorticoFZ.Image = global::PGi.Properties.Resources.Cortante_z;
            this.btPorticoFZ.Location = new System.Drawing.Point(76, 47);
            this.btPorticoFZ.Name = "btPorticoFZ";
            this.btPorticoFZ.Size = new System.Drawing.Size(29, 23);
            this.btPorticoFZ.TabIndex = 38;
            this.btPorticoFZ.Tag = "0";
            this.btPorticoFZ.UseVisualStyleBackColor = true;
            this.btPorticoFZ.Click += new System.EventHandler(this.button1_Click);
            this.btPorticoFZ.MouseEnter += new System.EventHandler(this.btPorticoFZ_MouseEnter);
            this.btPorticoFZ.MouseLeave += new System.EventHandler(this.btPorticoDz_MouseLeave);
            // 
            // btPorticoFY
            // 
            this.btPorticoFY.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
           // this.btPorticoFY.Image = global::PGi.Properties.Resources.Cortante_y;
            this.btPorticoFY.Location = new System.Drawing.Point(41, 47);
            this.btPorticoFY.Name = "btPorticoFY";
            this.btPorticoFY.Size = new System.Drawing.Size(29, 23);
            this.btPorticoFY.TabIndex = 37;
            this.btPorticoFY.Tag = "0";
            this.btPorticoFY.UseVisualStyleBackColor = true;
            this.btPorticoFY.Click += new System.EventHandler(this.button1_Click);
            this.btPorticoFY.MouseEnter += new System.EventHandler(this.btPorticoFY_MouseEnter);
            this.btPorticoFY.MouseLeave += new System.EventHandler(this.btPorticoDz_MouseLeave);
            // 
            // btPorticoDz
            // 
            this.btPorticoDz.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
          //  this.btPorticoDz.Image = global::PGi.Properties.Resources.Deslocamento1;
            this.btPorticoDz.Location = new System.Drawing.Point(6, 47);
            this.btPorticoDz.Name = "btPorticoDz";
            this.btPorticoDz.Size = new System.Drawing.Size(29, 23);
            this.btPorticoDz.TabIndex = 36;
            this.btPorticoDz.Tag = "0";
            this.btPorticoDz.UseVisualStyleBackColor = true;
            this.btPorticoDz.Click += new System.EventHandler(this.button1_Click);
            this.btPorticoDz.MouseEnter += new System.EventHandler(this.btPorticoDz_MouseEnter);
            this.btPorticoDz.MouseLeave += new System.EventHandler(this.btPorticoDz_MouseLeave);
            // 
            // groupBox1
            // 
            this.groupBox1.Controls.Add(this.trackEscalaCarga);
            this.groupBox1.Controls.Add(this.chMostrarCargasPontuais);
            this.groupBox1.Controls.Add(this.chMostrarCargasLineares);
            this.groupBox1.Location = new System.Drawing.Point(6, 81);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(232, 41);
            this.groupBox1.TabIndex = 76;
            this.groupBox1.TabStop = false;
            this.groupBox1.Text = "Mostrar cargas";
            // 
            // trackEscalaCarga
            // 
            this.trackEscalaCarga.AutoSize = false;
            this.trackEscalaCarga.LargeChange = 1;
            this.trackEscalaCarga.Location = new System.Drawing.Point(150, 13);
            this.trackEscalaCarga.Maximum = 40;
            this.trackEscalaCarga.Name = "trackEscalaCarga";
            this.trackEscalaCarga.Size = new System.Drawing.Size(81, 23);
            this.trackEscalaCarga.TabIndex = 76;
            this.trackEscalaCarga.Value = 10;
            this.trackEscalaCarga.Scroll += new System.EventHandler(this.trackEscalaCarga_Scroll);
            // 
            // chMostrarCargasPontuais
            // 
            this.chMostrarCargasPontuais.AutoSize = true;
            this.chMostrarCargasPontuais.Location = new System.Drawing.Point(78, 19);
            this.chMostrarCargasPontuais.Name = "chMostrarCargasPontuais";
            this.chMostrarCargasPontuais.Size = new System.Drawing.Size(67, 17);
            this.chMostrarCargasPontuais.TabIndex = 75;
            this.chMostrarCargasPontuais.Text = "Pontuais";
            this.chMostrarCargasPontuais.UseVisualStyleBackColor = true;
            this.chMostrarCargasPontuais.CheckedChanged += new System.EventHandler(this.chMostrarCargasPontuais_CheckedChanged);
            // 
            // chMostrarCargasLineares
            // 
            this.chMostrarCargasLineares.AutoSize = true;
            this.chMostrarCargasLineares.Location = new System.Drawing.Point(6, 19);
            this.chMostrarCargasLineares.Name = "chMostrarCargasLineares";
            this.chMostrarCargasLineares.Size = new System.Drawing.Size(66, 17);
            this.chMostrarCargasLineares.TabIndex = 74;
            this.chMostrarCargasLineares.Text = "Lineares";
            this.chMostrarCargasLineares.UseVisualStyleBackColor = true;
            this.chMostrarCargasLineares.CheckedChanged += new System.EventHandler(this.chMostrarCargasLineares_CheckedChanged);
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Font = new System.Drawing.Font("Tahoma", 8F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label2.ForeColor = System.Drawing.SystemColors.MenuText;
            this.label2.Location = new System.Drawing.Point(329, 57);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(25, 13);
            this.label2.TabIndex = 77;
            this.label2.Text = "x10";
            // 
            // FEsforcosPortico
            // 
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.None;
            this.CancelButton = this.btPorticoFX;
            this.ClientSize = new System.Drawing.Size(361, 152);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.groupBox1);
            this.Controls.Add(this.panel1);
            this.Controls.Add(this.trackEscalaDiagrama);
            this.Controls.Add(this.lbDica);
            this.Controls.Add(this.btPorticoMX);
            this.Controls.Add(this.btPorticoMZ);
            this.Controls.Add(this.btPorticoMY);
            this.Controls.Add(this.btPorticoFX);
            this.Controls.Add(this.btPorticoFZ);
            this.Controls.Add(this.btPorticoFY);
            this.Controls.Add(this.btPorticoDz);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.cbCombinacao);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedToolWindow;
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "FEsforcosPortico";
            this.SizeGripStyle = System.Windows.Forms.SizeGripStyle.Hide;
            this.Text = "Esforços - Pórtico";
            this.TopMost = true;
            this.Shown += new System.EventHandler(this.FEsforcosPortico_Shown);
            this.Move += new System.EventHandler(this.FEsforcosPortico_Move);
            ((System.ComponentModel.ISupportInitialize)(this.trackEscalaDiagrama)).EndInit();
            this.groupBox1.ResumeLayout(false);
            this.groupBox1.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.trackEscalaCarga)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        public System.Windows.Forms.ComboBox cbCombinacao;
        public System.Windows.Forms.Label label1;
        private System.Windows.Forms.Button btPorticoDz;
        private System.Windows.Forms.Button btPorticoFY;
        private System.Windows.Forms.Button btPorticoFZ;
        private System.Windows.Forms.Button btPorticoFX;
        private System.Windows.Forms.Button btPorticoMY;
        private System.Windows.Forms.Button btPorticoMZ;
        private System.Windows.Forms.Button btPorticoMX;
        private System.Windows.Forms.ImageList imageList1;
        private System.Windows.Forms.Label lbDica;
        public System.Windows.Forms.TrackBar trackEscalaDiagrama;
        private System.Windows.Forms.Panel panel1;
        private System.Windows.Forms.GroupBox groupBox1;
        public System.Windows.Forms.TrackBar trackEscalaCarga;
        private System.Windows.Forms.CheckBox chMostrarCargasPontuais;
        private System.Windows.Forms.CheckBox chMostrarCargasLineares;
        public System.Windows.Forms.Label label2;
    }
}