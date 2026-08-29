namespace PG
{
    partial class FCargaNodal
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(FCargaNodal));
            this.panel1 = new System.Windows.Forms.Panel();
            this.button7 = new System.Windows.Forms.Button();
            this.imageList3 = new System.Windows.Forms.ImageList(this.components);
            this.button8 = new System.Windows.Forms.Button();
            this.button1 = new System.Windows.Forms.Button();
            this.pnCorCaso = new System.Windows.Forms.Panel();
            this.label2 = new System.Windows.Forms.Label();
            this.cbCasos = new System.Windows.Forms.ComboBox();
            this.direcao = new System.Windows.Forms.GroupBox();
            this.rbZ = new System.Windows.Forms.RadioButton();
            this.rbY = new System.Windows.Forms.RadioButton();
            this.rbX = new System.Windows.Forms.RadioButton();
            this.lbUnidade = new System.Windows.Forms.Label();
            this.label17 = new System.Windows.Forms.Label();
            this.Valor = new System.Windows.Forms.TextBox();
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.rbMomento = new System.Windows.Forms.RadioButton();
            this.rbForca = new System.Windows.Forms.RadioButton();
            this.panel1.SuspendLayout();
            this.direcao.SuspendLayout();
            this.groupBox1.SuspendLayout();
            this.SuspendLayout();
            // 
            // panel1
            // 
            this.panel1.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.panel1.Controls.Add(this.button7);
            this.panel1.Controls.Add(this.button8);
            this.panel1.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.panel1.Location = new System.Drawing.Point(0, 220);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(190, 32);
            this.panel1.TabIndex = 41;
            // 
            // button7
            // 
            this.button7.BackColor = System.Drawing.Color.Transparent;
            this.button7.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.button7.ImageKey = "fechar.bmp";
            this.button7.ImageList = this.imageList3;
            this.button7.Location = new System.Drawing.Point(146, 3);
            this.button7.Name = "button7";
            this.button7.Size = new System.Drawing.Size(40, 25);
            this.button7.TabIndex = 7;
            this.button7.UseVisualStyleBackColor = false;
            this.button7.Click += new System.EventHandler(this.button7_Click);
            // 
            // imageList3
            // 
            this.imageList3.ImageStream = ((System.Windows.Forms.ImageListStreamer)(resources.GetObject("imageList3.ImageStream")));
            this.imageList3.TransparentColor = System.Drawing.Color.Transparent;
            this.imageList3.Images.SetKeyName(0, "confirmar.bmp");
            this.imageList3.Images.SetKeyName(1, "fechar.bmp");
            this.imageList3.Images.SetKeyName(2, "novo.png");
            this.imageList3.Images.SetKeyName(3, "excluir.bmp");
            // 
            // button8
            // 
            this.button8.BackColor = System.Drawing.Color.Transparent;
            this.button8.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.button8.ImageKey = "confirmar.bmp";
            this.button8.ImageList = this.imageList3;
            this.button8.Location = new System.Drawing.Point(3, 2);
            this.button8.Name = "button8";
            this.button8.Size = new System.Drawing.Size(40, 25);
            this.button8.TabIndex = 6;
            this.button8.UseVisualStyleBackColor = false;
            this.button8.Click += new System.EventHandler(this.button8_Click);
            // 
            // button1
            // 
            this.button1.Font = new System.Drawing.Font("Microsoft Sans Serif", 10F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.button1.Location = new System.Drawing.Point(166, 23);
            this.button1.Name = "button1";
            this.button1.Size = new System.Drawing.Size(21, 22);
            this.button1.TabIndex = 107;
            this.button1.Text = "+";
            this.button1.UseVisualStyleBackColor = true;
            this.button1.Click += new System.EventHandler(this.button1_Click);
            // 
            // pnCorCaso
            // 
            this.pnCorCaso.Location = new System.Drawing.Point(148, 26);
            this.pnCorCaso.Name = "pnCorCaso";
            this.pnCorCaso.Size = new System.Drawing.Size(15, 19);
            this.pnCorCaso.TabIndex = 106;
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(6, 9);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(76, 13);
            this.label2.TabIndex = 105;
            this.label2.Text = "Caso de carga";
            // 
            // cbCasos
            // 
            this.cbCasos.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cbCasos.FormattingEnabled = true;
            this.cbCasos.Location = new System.Drawing.Point(9, 24);
            this.cbCasos.Name = "cbCasos";
            this.cbCasos.Size = new System.Drawing.Size(134, 21);
            this.cbCasos.TabIndex = 104;
            this.cbCasos.SelectedIndexChanged += new System.EventHandler(this.cbCasos_SelectedIndexChanged);
            // 
            // direcao
            // 
            this.direcao.Controls.Add(this.rbZ);
            this.direcao.Controls.Add(this.rbY);
            this.direcao.Controls.Add(this.rbX);
            this.direcao.Location = new System.Drawing.Point(9, 113);
            this.direcao.Name = "direcao";
            this.direcao.Size = new System.Drawing.Size(115, 41);
            this.direcao.TabIndex = 101;
            this.direcao.TabStop = false;
            this.direcao.Text = "Direção";
            // 
            // rbZ
            // 
            this.rbZ.AutoSize = true;
            this.rbZ.Checked = true;
            this.rbZ.Location = new System.Drawing.Point(78, 19);
            this.rbZ.Name = "rbZ";
            this.rbZ.Size = new System.Drawing.Size(30, 17);
            this.rbZ.TabIndex = 97;
            this.rbZ.TabStop = true;
            this.rbZ.Text = "z";
            this.rbZ.UseVisualStyleBackColor = true;
            // 
            // rbY
            // 
            this.rbY.AutoSize = true;
            this.rbY.Location = new System.Drawing.Point(43, 18);
            this.rbY.Name = "rbY";
            this.rbY.Size = new System.Drawing.Size(30, 17);
            this.rbY.TabIndex = 96;
            this.rbY.Text = "y";
            this.rbY.UseVisualStyleBackColor = true;
            // 
            // rbX
            // 
            this.rbX.AutoSize = true;
            this.rbX.Location = new System.Drawing.Point(10, 18);
            this.rbX.Name = "rbX";
            this.rbX.Size = new System.Drawing.Size(30, 17);
            this.rbX.TabIndex = 95;
            this.rbX.Text = "x";
            this.rbX.UseVisualStyleBackColor = true;
            // 
            // lbUnidade
            // 
            this.lbUnidade.AutoSize = true;
            this.lbUnidade.Location = new System.Drawing.Point(125, 181);
            this.lbUnidade.Name = "lbUnidade";
            this.lbUnidade.Size = new System.Drawing.Size(13, 13);
            this.lbUnidade.TabIndex = 103;
            this.lbUnidade.Text = "tf";
            // 
            // label17
            // 
            this.label17.AutoSize = true;
            this.label17.Location = new System.Drawing.Point(14, 181);
            this.label17.Name = "label17";
            this.label17.Size = new System.Drawing.Size(30, 13);
            this.label17.TabIndex = 102;
            this.label17.Text = "valor";
            // 
            // Valor
            // 
            this.Valor.Location = new System.Drawing.Point(46, 174);
            this.Valor.Name = "Valor";
            this.Valor.Size = new System.Drawing.Size(78, 20);
            this.Valor.TabIndex = 0;
            this.Valor.Text = "0";
            this.Valor.KeyDown += new System.Windows.Forms.KeyEventHandler(this.Valor_KeyDown);
            this.Valor.KeyPress += new System.Windows.Forms.KeyPressEventHandler(this.Valor_KeyPress);
            // 
            // groupBox1
            // 
            this.groupBox1.Controls.Add(this.rbMomento);
            this.groupBox1.Controls.Add(this.rbForca);
            this.groupBox1.Location = new System.Drawing.Point(9, 52);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(178, 47);
            this.groupBox1.TabIndex = 110;
            this.groupBox1.TabStop = false;
            this.groupBox1.Text = "Tipo";
            // 
            // rbMomento
            // 
            this.rbMomento.AutoSize = true;
            this.rbMomento.Enabled = false;
            this.rbMomento.Location = new System.Drawing.Point(75, 19);
            this.rbMomento.Name = "rbMomento";
            this.rbMomento.Size = new System.Drawing.Size(69, 17);
            this.rbMomento.TabIndex = 111;
            this.rbMomento.Text = "Momento";
            this.rbMomento.UseVisualStyleBackColor = true;
            this.rbMomento.CheckedChanged += new System.EventHandler(this.rbMomento_CheckedChanged);
            // 
            // rbForca
            // 
            this.rbForca.AutoSize = true;
            this.rbForca.Checked = true;
            this.rbForca.Location = new System.Drawing.Point(10, 19);
            this.rbForca.Name = "rbForca";
            this.rbForca.Size = new System.Drawing.Size(52, 17);
            this.rbForca.TabIndex = 110;
            this.rbForca.TabStop = true;
            this.rbForca.Text = "Força";
            this.rbForca.UseVisualStyleBackColor = true;
            this.rbForca.CheckedChanged += new System.EventHandler(this.rbForca_CheckedChanged);
            // 
            // FCargaNodal
            // 
            this.AcceptButton = this.button8;
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(190, 252);
            this.Controls.Add(this.groupBox1);
            this.Controls.Add(this.button1);
            this.Controls.Add(this.pnCorCaso);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.cbCasos);
            this.Controls.Add(this.direcao);
            this.Controls.Add(this.lbUnidade);
            this.Controls.Add(this.label17);
            this.Controls.Add(this.Valor);
            this.Controls.Add(this.panel1);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedToolWindow;
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "FCargaNodal";
            this.Text = "Criar carga em um nó";
            this.FormClosed += new System.Windows.Forms.FormClosedEventHandler(this.FCargaNodal_FormClosed);
            this.Load += new System.EventHandler(this.FCargaNodal_Load);
            this.Move += new System.EventHandler(this.FCargaNodal_Move);
            this.panel1.ResumeLayout(false);
            this.direcao.ResumeLayout(false);
            this.direcao.PerformLayout();
            this.groupBox1.ResumeLayout(false);
            this.groupBox1.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion
        private System.Windows.Forms.Panel panel1;
        private System.Windows.Forms.Button button7;
        private System.Windows.Forms.Button button8;
        private System.Windows.Forms.Button button1;
        private System.Windows.Forms.Panel pnCorCaso;
        private System.Windows.Forms.Label label2;
        public System.Windows.Forms.ComboBox cbCasos;
        private System.Windows.Forms.GroupBox direcao;
        public System.Windows.Forms.RadioButton rbZ;
        public System.Windows.Forms.RadioButton rbY;
        public System.Windows.Forms.RadioButton rbX;
        private System.Windows.Forms.Label lbUnidade;
        private System.Windows.Forms.Label label17;
        public System.Windows.Forms.TextBox Valor;
        private System.Windows.Forms.GroupBox groupBox1;
        public System.Windows.Forms.RadioButton rbMomento;
        public System.Windows.Forms.RadioButton rbForca;
        public System.Windows.Forms.ImageList imageList3;
    }
}