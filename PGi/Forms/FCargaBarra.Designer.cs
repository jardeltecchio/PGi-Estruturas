namespace PG
{
    partial class FCarga
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(FCarga));
            this.panel1 = new System.Windows.Forms.Panel();
            this.button7 = new System.Windows.Forms.Button();
            this.imageList1 = new System.Windows.Forms.ImageList(this.components);
            this.button8 = new System.Windows.Forms.Button();
            this.lbUnidade = new System.Windows.Forms.Label();
            this.label17 = new System.Windows.Forms.Label();
            this.Valor = new System.Windows.Forms.TextBox();
            this.direcao = new System.Windows.Forms.GroupBox();
            this.rbZ = new System.Windows.Forms.RadioButton();
            this.rbY = new System.Windows.Forms.RadioButton();
            this.rbX = new System.Windows.Forms.RadioButton();
            this.imageList2 = new System.Windows.Forms.ImageList(this.components);
            this.gbTipo = new System.Windows.Forms.GroupBox();
            this.tabControl1 = new System.Windows.Forms.TabControl();
            this.tabPage1 = new System.Windows.Forms.TabPage();
            this.panel3 = new System.Windows.Forms.Panel();
            this.tabPage2 = new System.Windows.Forms.TabPage();
            this.panel2 = new System.Windows.Forms.Panel();
            this.imConcentrada = new System.Windows.Forms.Panel();
            this.pnCargaConcentrada = new System.Windows.Forms.Panel();
            this.label1 = new System.Windows.Forms.Label();
            this.d = new System.Windows.Forms.Label();
            this.edD = new System.Windows.Forms.TextBox();
            this.cbCasos = new System.Windows.Forms.ComboBox();
            this.label2 = new System.Windows.Forms.Label();
            this.pnCorCaso = new System.Windows.Forms.Panel();
            this.chPosRel = new System.Windows.Forms.CheckBox();
            this.button1 = new System.Windows.Forms.Button();
            this.imDistribuida = new System.Windows.Forms.Panel();
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.rbLocal = new System.Windows.Forms.RadioButton();
            this.rbGlobal = new System.Windows.Forms.RadioButton();
            this.panel1.SuspendLayout();
            this.direcao.SuspendLayout();
            this.gbTipo.SuspendLayout();
            this.tabControl1.SuspendLayout();
            this.tabPage1.SuspendLayout();
            this.tabPage2.SuspendLayout();
            this.pnCargaConcentrada.SuspendLayout();
            this.groupBox1.SuspendLayout();
            this.SuspendLayout();
            // 
            // panel1
            // 
            this.panel1.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.panel1.Controls.Add(this.button7);
            this.panel1.Controls.Add(this.button8);
            this.panel1.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.panel1.Location = new System.Drawing.Point(0, 337);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(192, 32);
            this.panel1.TabIndex = 40;
            // 
            // button7
            // 
            this.button7.BackColor = System.Drawing.Color.Transparent;
            this.button7.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.button7.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.button7.ImageIndex = 1;
            this.button7.ImageList = this.imageList1;
            this.button7.Location = new System.Drawing.Point(146, 3);
            this.button7.Name = "button7";
            this.button7.Size = new System.Drawing.Size(40, 25);
            this.button7.TabIndex = 7;
            this.button7.UseVisualStyleBackColor = false;
            this.button7.Click += new System.EventHandler(this.button7_Click);
            // 
            // imageList1
            // 
            this.imageList1.ImageStream = ((System.Windows.Forms.ImageListStreamer)(resources.GetObject("imageList1.ImageStream")));
            this.imageList1.TransparentColor = System.Drawing.Color.Transparent;
            this.imageList1.Images.SetKeyName(0, "confirmar.bmp");
            this.imageList1.Images.SetKeyName(1, "fechar.bmp");
            // 
            // button8
            // 
            this.button8.BackColor = System.Drawing.Color.Transparent;
            this.button8.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.button8.ImageKey = "confirmar.bmp";
            this.button8.ImageList = this.imageList1;
            this.button8.Location = new System.Drawing.Point(3, 2);
            this.button8.Name = "button8";
            this.button8.Size = new System.Drawing.Size(40, 25);
            this.button8.TabIndex = 6;
            this.button8.UseVisualStyleBackColor = false;
            this.button8.Click += new System.EventHandler(this.button8_Click);
            // 
            // lbUnidade
            // 
            this.lbUnidade.AutoSize = true;
            this.lbUnidade.Location = new System.Drawing.Point(139, 313);
            this.lbUnidade.Name = "lbUnidade";
            this.lbUnidade.Size = new System.Drawing.Size(35, 13);
            this.lbUnidade.TabIndex = 87;
            this.lbUnidade.Text = "kN.m²";
            // 
            // label17
            // 
            this.label17.AutoSize = true;
            this.label17.Location = new System.Drawing.Point(20, 313);
            this.label17.Name = "label17";
            this.label17.Size = new System.Drawing.Size(30, 13);
            this.label17.TabIndex = 86;
            this.label17.Text = "valor";
            // 
            // Valor
            // 
            this.Valor.Location = new System.Drawing.Point(68, 306);
            this.Valor.Name = "Valor";
            this.Valor.Size = new System.Drawing.Size(69, 20);
            this.Valor.TabIndex = 1;
            this.Valor.Text = "0";
            this.Valor.KeyDown += new System.Windows.Forms.KeyEventHandler(this.Valor_KeyDown);
            this.Valor.KeyPress += new System.Windows.Forms.KeyPressEventHandler(this.Valor_KeyPress);
            this.Valor.KeyUp += new System.Windows.Forms.KeyEventHandler(this.Valor_KeyUp);
            // 
            // direcao
            // 
            this.direcao.Controls.Add(this.rbZ);
            this.direcao.Controls.Add(this.rbY);
            this.direcao.Controls.Add(this.rbX);
            this.direcao.Location = new System.Drawing.Point(7, 198);
            this.direcao.Name = "direcao";
            this.direcao.Size = new System.Drawing.Size(183, 42);
            this.direcao.TabIndex = 4;
            this.direcao.TabStop = false;
            this.direcao.Text = "Direção";
            this.direcao.Enter += new System.EventHandler(this.direcao_Enter);
            // 
            // rbZ
            // 
            this.rbZ.AutoSize = true;
            this.rbZ.Checked = true;
            this.rbZ.Font = new System.Drawing.Font("Microsoft Sans Serif", 10F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.rbZ.ForeColor = System.Drawing.Color.Blue;
            this.rbZ.Location = new System.Drawing.Point(100, 19);
            this.rbZ.Name = "rbZ";
            this.rbZ.Size = new System.Drawing.Size(34, 21);
            this.rbZ.TabIndex = 97;
            this.rbZ.TabStop = true;
            this.rbZ.Text = "z";
            this.rbZ.UseVisualStyleBackColor = true;
            // 
            // rbY
            // 
            this.rbY.AutoSize = true;
            this.rbY.Font = new System.Drawing.Font("Microsoft Sans Serif", 10F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.rbY.ForeColor = System.Drawing.Color.Green;
            this.rbY.Location = new System.Drawing.Point(54, 19);
            this.rbY.Name = "rbY";
            this.rbY.Size = new System.Drawing.Size(34, 21);
            this.rbY.TabIndex = 96;
            this.rbY.Text = "y";
            this.rbY.UseVisualStyleBackColor = true;
            // 
            // rbX
            // 
            this.rbX.AutoSize = true;
            this.rbX.Font = new System.Drawing.Font("Microsoft Sans Serif", 10F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.rbX.ForeColor = System.Drawing.Color.Red;
            this.rbX.Location = new System.Drawing.Point(9, 19);
            this.rbX.Name = "rbX";
            this.rbX.Size = new System.Drawing.Size(33, 21);
            this.rbX.TabIndex = 95;
            this.rbX.Text = "x";
            this.rbX.UseVisualStyleBackColor = true;
            // 
            // imageList2
            // 
            this.imageList2.ImageStream = ((System.Windows.Forms.ImageListStreamer)(resources.GetObject("imageList2.ImageStream")));
            this.imageList2.TransparentColor = System.Drawing.Color.Transparent;
            this.imageList2.Images.SetKeyName(0, "apoio simples.bmp");
            this.imageList2.Images.SetKeyName(1, "engaste.bmp");
            // 
            // gbTipo
            // 
            this.gbTipo.Controls.Add(this.tabControl1);
            this.gbTipo.Location = new System.Drawing.Point(4, 47);
            this.gbTipo.Name = "gbTipo";
            this.gbTipo.Size = new System.Drawing.Size(183, 101);
            this.gbTipo.TabIndex = 88;
            this.gbTipo.TabStop = false;
            this.gbTipo.Text = "Tipo";
            // 
            // tabControl1
            // 
            this.tabControl1.Controls.Add(this.tabPage1);
            this.tabControl1.Controls.Add(this.tabPage2);
            this.tabControl1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tabControl1.Location = new System.Drawing.Point(3, 16);
            this.tabControl1.Name = "tabControl1";
            this.tabControl1.SelectedIndex = 0;
            this.tabControl1.Size = new System.Drawing.Size(177, 82);
            this.tabControl1.TabIndex = 101;
            this.tabControl1.SelectedIndexChanged += new System.EventHandler(this.tabControl1_SelectedIndexChanged);
            // 
            // tabPage1
            // 
            this.tabPage1.Controls.Add(this.panel3);
            this.tabPage1.Location = new System.Drawing.Point(4, 22);
            this.tabPage1.Name = "tabPage1";
            this.tabPage1.Padding = new System.Windows.Forms.Padding(3);
            this.tabPage1.Size = new System.Drawing.Size(169, 56);
            this.tabPage1.TabIndex = 0;
            this.tabPage1.Text = "Distribuída";
            this.tabPage1.UseVisualStyleBackColor = true;
            // 
            // panel3
            // 
            this.panel3.BackgroundImage = ((System.Drawing.Image)(resources.GetObject("panel3.BackgroundImage")));
            this.panel3.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Center;
            this.panel3.Dock = System.Windows.Forms.DockStyle.Fill;
            this.panel3.Location = new System.Drawing.Point(3, 3);
            this.panel3.Name = "panel3";
            this.panel3.Size = new System.Drawing.Size(163, 50);
            this.panel3.TabIndex = 0;
            // 
            // tabPage2
            // 
            this.tabPage2.Controls.Add(this.panel2);
            this.tabPage2.Location = new System.Drawing.Point(4, 22);
            this.tabPage2.Name = "tabPage2";
            this.tabPage2.Padding = new System.Windows.Forms.Padding(3);
            this.tabPage2.Size = new System.Drawing.Size(169, 56);
            this.tabPage2.TabIndex = 1;
            this.tabPage2.Text = "Concentrada";
            this.tabPage2.UseVisualStyleBackColor = true;
            // 
            // panel2
            // 
            this.panel2.BackgroundImage = ((System.Drawing.Image)(resources.GetObject("panel2.BackgroundImage")));
            this.panel2.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Center;
            this.panel2.Dock = System.Windows.Forms.DockStyle.Fill;
            this.panel2.Location = new System.Drawing.Point(3, 3);
            this.panel2.Name = "panel2";
            this.panel2.Size = new System.Drawing.Size(163, 50);
            this.panel2.TabIndex = 0;
            // 
            // imConcentrada
            // 
            this.imConcentrada.BackgroundImageLayout = System.Windows.Forms.ImageLayout.None;
            this.imConcentrada.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.imConcentrada.Location = new System.Drawing.Point(298, 112);
            this.imConcentrada.Name = "imConcentrada";
            this.imConcentrada.Size = new System.Drawing.Size(99, 52);
            this.imConcentrada.TabIndex = 89;
            // 
            // pnCargaConcentrada
            // 
            this.pnCargaConcentrada.Controls.Add(this.label1);
            this.pnCargaConcentrada.Controls.Add(this.d);
            this.pnCargaConcentrada.Controls.Add(this.edD);
            this.pnCargaConcentrada.Location = new System.Drawing.Point(37, 274);
            this.pnCargaConcentrada.Name = "pnCargaConcentrada";
            this.pnCargaConcentrada.Size = new System.Drawing.Size(109, 26);
            this.pnCargaConcentrada.TabIndex = 0;
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(85, 10);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(15, 13);
            this.label1.TabIndex = 95;
            this.label1.Text = "m";
            // 
            // d
            // 
            this.d.AutoSize = true;
            this.d.Location = new System.Drawing.Point(8, 10);
            this.d.Name = "d";
            this.d.Size = new System.Drawing.Size(13, 13);
            this.d.TabIndex = 94;
            this.d.Text = "d";
            // 
            // edD
            // 
            this.edD.Location = new System.Drawing.Point(30, 3);
            this.edD.Name = "edD";
            this.edD.Size = new System.Drawing.Size(49, 20);
            this.edD.TabIndex = 0;
            this.edD.Text = "0";
            this.edD.TextChanged += new System.EventHandler(this.edD_TextChanged);
            this.edD.KeyDown += new System.Windows.Forms.KeyEventHandler(this.edD_KeyDown);
            this.edD.KeyPress += new System.Windows.Forms.KeyPressEventHandler(this.Valor_KeyPress);
            this.edD.Leave += new System.EventHandler(this.edD_Leave);
            this.edD.Validated += new System.EventHandler(this.edD_Validated);
            // 
            // cbCasos
            // 
            this.cbCasos.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cbCasos.FormattingEnabled = true;
            this.cbCasos.Location = new System.Drawing.Point(8, 21);
            this.cbCasos.Name = "cbCasos";
            this.cbCasos.Size = new System.Drawing.Size(134, 21);
            this.cbCasos.TabIndex = 95;
            this.cbCasos.SelectedIndexChanged += new System.EventHandler(this.cbCasos_SelectedIndexChanged);
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(5, 6);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(76, 13);
            this.label2.TabIndex = 96;
            this.label2.Text = "Caso de carga";
            // 
            // pnCorCaso
            // 
            this.pnCorCaso.Location = new System.Drawing.Point(147, 23);
            this.pnCorCaso.Name = "pnCorCaso";
            this.pnCorCaso.Size = new System.Drawing.Size(15, 19);
            this.pnCorCaso.TabIndex = 97;
            // 
            // chPosRel
            // 
            this.chPosRel.AutoSize = true;
            this.chPosRel.Location = new System.Drawing.Point(27, 254);
            this.chPosRel.Name = "chPosRel";
            this.chPosRel.Size = new System.Drawing.Size(107, 17);
            this.chPosRel.TabIndex = 98;
            this.chPosRel.Text = "Distância relativa";
            this.chPosRel.UseVisualStyleBackColor = true;
            this.chPosRel.CheckedChanged += new System.EventHandler(this.checkBox1_CheckedChanged);
            // 
            // button1
            // 
            this.button1.Font = new System.Drawing.Font("Microsoft Sans Serif", 10F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.button1.Location = new System.Drawing.Point(165, 20);
            this.button1.Name = "button1";
            this.button1.Size = new System.Drawing.Size(21, 22);
            this.button1.TabIndex = 99;
            this.button1.Text = "+";
            this.button1.UseVisualStyleBackColor = true;
            this.button1.Click += new System.EventHandler(this.button1_Click);
            // 
            // imDistribuida
            // 
            this.imDistribuida.BackgroundImage = global::PGi.Properties.Resources.carga_distribuida3;
            this.imDistribuida.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.imDistribuida.Location = new System.Drawing.Point(308, 167);
            this.imDistribuida.Name = "imDistribuida";
            this.imDistribuida.Size = new System.Drawing.Size(99, 52);
            this.imDistribuida.TabIndex = 94;
            // 
            // groupBox1
            // 
            this.groupBox1.Controls.Add(this.rbLocal);
            this.groupBox1.Controls.Add(this.rbGlobal);
            this.groupBox1.Location = new System.Drawing.Point(5, 152);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(183, 38);
            this.groupBox1.TabIndex = 100;
            this.groupBox1.TabStop = false;
            this.groupBox1.Text = "Sistema de coordenadas";
            // 
            // rbLocal
            // 
            this.rbLocal.AutoSize = true;
            this.rbLocal.Location = new System.Drawing.Point(70, 19);
            this.rbLocal.Name = "rbLocal";
            this.rbLocal.Size = new System.Drawing.Size(51, 17);
            this.rbLocal.TabIndex = 96;
            this.rbLocal.Text = "Local";
            this.rbLocal.UseVisualStyleBackColor = true;
            // 
            // rbGlobal
            // 
            this.rbGlobal.AutoSize = true;
            this.rbGlobal.Checked = true;
            this.rbGlobal.Location = new System.Drawing.Point(9, 19);
            this.rbGlobal.Name = "rbGlobal";
            this.rbGlobal.Size = new System.Drawing.Size(55, 17);
            this.rbGlobal.TabIndex = 95;
            this.rbGlobal.TabStop = true;
            this.rbGlobal.Text = "Global";
            this.rbGlobal.UseVisualStyleBackColor = true;
            // 
            // FCarga
            // 
            this.AcceptButton = this.button8;
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.CancelButton = this.button7;
            this.ClientSize = new System.Drawing.Size(192, 369);
            this.Controls.Add(this.groupBox1);
            this.Controls.Add(this.button1);
            this.Controls.Add(this.direcao);
            this.Controls.Add(this.chPosRel);
            this.Controls.Add(this.pnCorCaso);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.cbCasos);
            this.Controls.Add(this.imDistribuida);
            this.Controls.Add(this.pnCargaConcentrada);
            this.Controls.Add(this.imConcentrada);
            this.Controls.Add(this.gbTipo);
            this.Controls.Add(this.lbUnidade);
            this.Controls.Add(this.label17);
            this.Controls.Add(this.Valor);
            this.Controls.Add(this.panel1);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedToolWindow;
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "FCarga";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "Criar carga em barra";
            this.FormClosed += new System.Windows.Forms.FormClosedEventHandler(this.FCarga_FormClosed);
            this.Load += new System.EventHandler(this.FCarga_Load);
            this.Move += new System.EventHandler(this.FCarga_Move);
            this.panel1.ResumeLayout(false);
            this.direcao.ResumeLayout(false);
            this.direcao.PerformLayout();
            this.gbTipo.ResumeLayout(false);
            this.tabControl1.ResumeLayout(false);
            this.tabPage1.ResumeLayout(false);
            this.tabPage2.ResumeLayout(false);
            this.pnCargaConcentrada.ResumeLayout(false);
            this.pnCargaConcentrada.PerformLayout();
            this.groupBox1.ResumeLayout(false);
            this.groupBox1.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Panel panel1;
        private System.Windows.Forms.Button button7;
        private System.Windows.Forms.Button button8;
        private System.Windows.Forms.Label lbUnidade;
        private System.Windows.Forms.Label label17;
        public System.Windows.Forms.TextBox Valor;
        private System.Windows.Forms.GroupBox direcao;
        public System.Windows.Forms.ImageList imageList1;
        private System.Windows.Forms.ImageList imageList2;
        public System.Windows.Forms.RadioButton rbZ;
        public System.Windows.Forms.RadioButton rbY;
        public System.Windows.Forms.RadioButton rbX;
        private System.Windows.Forms.GroupBox gbTipo;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label d;
        public System.Windows.Forms.TextBox edD;
        public System.Windows.Forms.ComboBox cbCasos;
        private System.Windows.Forms.Label label2;
        public System.Windows.Forms.Panel imConcentrada;
        public System.Windows.Forms.Panel imDistribuida;
        public System.Windows.Forms.Panel pnCargaConcentrada;
        private System.Windows.Forms.Panel pnCorCaso;
        public System.Windows.Forms.CheckBox chPosRel;
        private System.Windows.Forms.Button button1;
        private System.Windows.Forms.TabPage tabPage1;
        private System.Windows.Forms.TabPage tabPage2;
        private System.Windows.Forms.Panel panel3;
        private System.Windows.Forms.Panel panel2;
        private System.Windows.Forms.GroupBox groupBox1;
        public System.Windows.Forms.RadioButton rbLocal;
        public System.Windows.Forms.RadioButton rbGlobal;
        public System.Windows.Forms.TabControl tabControl1;
    }
}