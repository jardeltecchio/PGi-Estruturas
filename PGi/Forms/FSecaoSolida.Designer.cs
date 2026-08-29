namespace PG
{
    partial class FSecaoSolida
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(FSecaoSolida));
            this.panel1 = new System.Windows.Forms.Panel();
            this.button3 = new System.Windows.Forms.Button();
            this.imageList2 = new System.Windows.Forms.ImageList(this.components);
            this.btSalvar = new System.Windows.Forms.Button();
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.button1 = new System.Windows.Forms.Button();
            this.label3 = new System.Windows.Forms.Label();
            this.cbMaterial = new System.Windows.Forms.ComboBox();
            this.label6 = new System.Windows.Forms.Label();
            this.NomeSecao = new System.Windows.Forms.TextBox();
            this.label7 = new System.Windows.Forms.Label();
            this.btCor = new System.Windows.Forms.Button();
            this.gbTipo = new System.Windows.Forms.GroupBox();
            this.pnCirculo = new System.Windows.Forms.Panel();
            this.label4 = new System.Windows.Forms.Label();
            this.label1 = new System.Windows.Forms.Label();
            this.edD = new System.Windows.Forms.TextBox();
            this.pnRetangulo = new System.Windows.Forms.Panel();
            this.label5 = new System.Windows.Forms.Label();
            this.label12 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.edh1 = new System.Windows.Forms.TextBox();
            this.edb1 = new System.Windows.Forms.TextBox();
            this.colorDialog1 = new System.Windows.Forms.ColorDialog();
            this.groupBox3 = new System.Windows.Forms.GroupBox();
            this.btCirc = new System.Windows.Forms.Button();
            this.btRet = new System.Windows.Forms.Button();
            this.btCalculaSecao = new System.Windows.Forms.Button();
            this.panel1.SuspendLayout();
            this.groupBox1.SuspendLayout();
            this.gbTipo.SuspendLayout();
            this.pnCirculo.SuspendLayout();
            this.pnRetangulo.SuspendLayout();
            this.groupBox3.SuspendLayout();
            this.SuspendLayout();
            // 
            // panel1
            // 
            this.panel1.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.panel1.Controls.Add(this.button3);
            this.panel1.Controls.Add(this.btSalvar);
            this.panel1.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.panel1.Location = new System.Drawing.Point(0, 404);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(577, 34);
            this.panel1.TabIndex = 63;
            this.panel1.Paint += new System.Windows.Forms.PaintEventHandler(this.panel1_Paint);
            // 
            // button3
            // 
            this.button3.BackColor = System.Drawing.Color.Transparent;
            this.button3.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.button3.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.button3.ImageIndex = 1;
            this.button3.ImageList = this.imageList2;
            this.button3.Location = new System.Drawing.Point(240, 3);
            this.button3.Name = "button3";
            this.button3.Size = new System.Drawing.Size(40, 25);
            this.button3.TabIndex = 16;
            this.button3.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            this.button3.UseVisualStyleBackColor = false;
            this.button3.Click += new System.EventHandler(this.button3_Click);
            // 
            // imageList2
            // 
            this.imageList2.ImageStream = ((System.Windows.Forms.ImageListStreamer)(resources.GetObject("imageList2.ImageStream")));
            this.imageList2.TransparentColor = System.Drawing.Color.Transparent;
            this.imageList2.Images.SetKeyName(0, "confirmar.bmp");
            this.imageList2.Images.SetKeyName(1, "fechar.bmp");
            // 
            // btSalvar
            // 
            this.btSalvar.BackColor = System.Drawing.Color.Transparent;
            this.btSalvar.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btSalvar.ImageKey = "confirmar.bmp";
            this.btSalvar.ImageList = this.imageList2;
            this.btSalvar.Location = new System.Drawing.Point(4, 3);
            this.btSalvar.Name = "btSalvar";
            this.btSalvar.Size = new System.Drawing.Size(40, 25);
            this.btSalvar.TabIndex = 16;
            this.btSalvar.TextAlign = System.Drawing.ContentAlignment.BottomCenter;
            this.btSalvar.UseVisualStyleBackColor = false;
            this.btSalvar.Click += new System.EventHandler(this.btSalvar_Click);
            // 
            // groupBox1
            // 
            this.groupBox1.BackColor = System.Drawing.SystemColors.ButtonFace;
            this.groupBox1.Controls.Add(this.button1);
            this.groupBox1.Controls.Add(this.label3);
            this.groupBox1.Controls.Add(this.cbMaterial);
            this.groupBox1.Controls.Add(this.label6);
            this.groupBox1.Controls.Add(this.NomeSecao);
            this.groupBox1.Controls.Add(this.label7);
            this.groupBox1.Controls.Add(this.btCor);
            this.groupBox1.Dock = System.Windows.Forms.DockStyle.Top;
            this.groupBox1.Location = new System.Drawing.Point(0, 0);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(577, 68);
            this.groupBox1.TabIndex = 73;
            this.groupBox1.TabStop = false;
            // 
            // button1
            // 
            this.button1.Font = new System.Drawing.Font("Microsoft Sans Serif", 10F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.button1.Location = new System.Drawing.Point(259, 9);
            this.button1.Name = "button1";
            this.button1.Size = new System.Drawing.Size(21, 22);
            this.button1.TabIndex = 108;
            this.button1.Text = "+";
            this.button1.UseVisualStyleBackColor = true;
            this.button1.Click += new System.EventHandler(this.button1_Click);
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.ForeColor = System.Drawing.SystemColors.Desktop;
            this.label3.Location = new System.Drawing.Point(90, 18);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(44, 13);
            this.label3.TabIndex = 88;
            this.label3.Text = "Material";
            // 
            // cbMaterial
            // 
            this.cbMaterial.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cbMaterial.FormattingEnabled = true;
            this.cbMaterial.Location = new System.Drawing.Point(134, 10);
            this.cbMaterial.Name = "cbMaterial";
            this.cbMaterial.Size = new System.Drawing.Size(121, 21);
            this.cbMaterial.TabIndex = 87;
            this.cbMaterial.KeyDown += new System.Windows.Forms.KeyEventHandler(this.btCor_KeyDown);
            // 
            // label6
            // 
            this.label6.AutoSize = true;
            this.label6.Location = new System.Drawing.Point(8, 45);
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size(35, 13);
            this.label6.TabIndex = 86;
            this.label6.Text = "Nome";
            // 
            // NomeSecao
            // 
            this.NomeSecao.Location = new System.Drawing.Point(44, 37);
            this.NomeSecao.Name = "NomeSecao";
            this.NomeSecao.Size = new System.Drawing.Size(235, 20);
            this.NomeSecao.TabIndex = 85;
            this.NomeSecao.KeyDown += new System.Windows.Forms.KeyEventHandler(this.btCor_KeyDown);
            // 
            // label7
            // 
            this.label7.AutoSize = true;
            this.label7.Location = new System.Drawing.Point(8, 19);
            this.label7.Name = "label7";
            this.label7.Size = new System.Drawing.Size(23, 13);
            this.label7.TabIndex = 84;
            this.label7.Text = "Cor";
            // 
            // btCor
            // 
            this.btCor.BackColor = System.Drawing.Color.DimGray;
            this.btCor.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btCor.Location = new System.Drawing.Point(44, 13);
            this.btCor.Name = "btCor";
            this.btCor.Size = new System.Drawing.Size(20, 19);
            this.btCor.TabIndex = 83;
            this.btCor.UseVisualStyleBackColor = false;
            this.btCor.Click += new System.EventHandler(this.btCorCima_Click);
            this.btCor.KeyDown += new System.Windows.Forms.KeyEventHandler(this.btCor_KeyDown);
            // 
            // gbTipo
            // 
            this.gbTipo.Controls.Add(this.pnCirculo);
            this.gbTipo.Controls.Add(this.pnRetangulo);
            this.gbTipo.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.gbTipo.ForeColor = System.Drawing.SystemColors.HotTrack;
            this.gbTipo.Location = new System.Drawing.Point(0, 298);
            this.gbTipo.Name = "gbTipo";
            this.gbTipo.Size = new System.Drawing.Size(577, 106);
            this.gbTipo.TabIndex = 0;
            this.gbTipo.TabStop = false;
            this.gbTipo.Text = "Tipo";
            // 
            // pnCirculo
            // 
            this.pnCirculo.Controls.Add(this.label4);
            this.pnCirculo.Controls.Add(this.label1);
            this.pnCirculo.Controls.Add(this.edD);
            this.pnCirculo.Location = new System.Drawing.Point(10, 45);
            this.pnCirculo.Name = "pnCirculo";
            this.pnCirculo.Size = new System.Drawing.Size(176, 33);
            this.pnCirculo.TabIndex = 74;
            this.pnCirculo.Tag = "2";
            this.pnCirculo.Visible = false;
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.ForeColor = System.Drawing.SystemColors.Desktop;
            this.label4.Location = new System.Drawing.Point(119, 14);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(21, 13);
            this.label4.TabIndex = 75;
            this.label4.Text = "cm";
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.ForeColor = System.Drawing.SystemColors.Desktop;
            this.label1.Location = new System.Drawing.Point(11, 14);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(47, 13);
            this.label1.TabIndex = 74;
            this.label1.Text = "diâmetro";
            // 
            // edD
            // 
            this.edD.Location = new System.Drawing.Point(64, 7);
            this.edD.Name = "edD";
            this.edD.Size = new System.Drawing.Size(49, 20);
            this.edD.TabIndex = 73;
            this.edD.Text = "14";
            this.edD.KeyDown += new System.Windows.Forms.KeyEventHandler(this.btCor_KeyDown);
            this.edD.KeyPress += new System.Windows.Forms.KeyPressEventHandler(this.edD_KeyPress);
            this.edD.Validated += new System.EventHandler(this.edD_Validated);
            // 
            // pnRetangulo
            // 
            this.pnRetangulo.Controls.Add(this.label5);
            this.pnRetangulo.Controls.Add(this.label12);
            this.pnRetangulo.Controls.Add(this.label2);
            this.pnRetangulo.Controls.Add(this.edh1);
            this.pnRetangulo.Controls.Add(this.edb1);
            this.pnRetangulo.Location = new System.Drawing.Point(44, 19);
            this.pnRetangulo.Name = "pnRetangulo";
            this.pnRetangulo.Size = new System.Drawing.Size(184, 35);
            this.pnRetangulo.TabIndex = 73;
            this.pnRetangulo.Tag = "1";
            this.pnRetangulo.Visible = false;
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.ForeColor = System.Drawing.SystemColors.Desktop;
            this.label5.Location = new System.Drawing.Point(144, 10);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(21, 13);
            this.label5.TabIndex = 77;
            this.label5.Text = "cm";
            // 
            // label12
            // 
            this.label12.AutoSize = true;
            this.label12.ForeColor = System.Drawing.SystemColors.Desktop;
            this.label12.Location = new System.Drawing.Point(77, 10);
            this.label12.Name = "label12";
            this.label12.Size = new System.Drawing.Size(13, 13);
            this.label12.TabIndex = 76;
            this.label12.Text = "h";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.ForeColor = System.Drawing.SystemColors.Desktop;
            this.label2.Location = new System.Drawing.Point(3, 10);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(13, 13);
            this.label2.TabIndex = 75;
            this.label2.Text = "b";
            // 
            // edh1
            // 
            this.edh1.Location = new System.Drawing.Point(93, 3);
            this.edh1.Name = "edh1";
            this.edh1.Size = new System.Drawing.Size(49, 20);
            this.edh1.TabIndex = 1;
            this.edh1.Text = "30";
            this.edh1.KeyDown += new System.Windows.Forms.KeyEventHandler(this.btCor_KeyDown);
            this.edh1.KeyPress += new System.Windows.Forms.KeyPressEventHandler(this.edb1_KeyPress);
            this.edh1.Validated += new System.EventHandler(this.edb1_Validated);
            // 
            // edb1
            // 
            this.edb1.Location = new System.Drawing.Point(20, 3);
            this.edb1.Name = "edb1";
            this.edb1.Size = new System.Drawing.Size(49, 20);
            this.edb1.TabIndex = 0;
            this.edb1.Text = "14";
            this.edb1.KeyDown += new System.Windows.Forms.KeyEventHandler(this.btCor_KeyDown);
            this.edb1.KeyPress += new System.Windows.Forms.KeyPressEventHandler(this.edb1_KeyPress);
            this.edb1.Validated += new System.EventHandler(this.edb1_Validated);
            // 
            // groupBox3
            // 
            this.groupBox3.Controls.Add(this.btCirc);
            this.groupBox3.Controls.Add(this.btRet);
            this.groupBox3.Dock = System.Windows.Forms.DockStyle.Top;
            this.groupBox3.ForeColor = System.Drawing.SystemColors.HotTrack;
            this.groupBox3.Location = new System.Drawing.Point(0, 68);
            this.groupBox3.Name = "groupBox3";
            this.groupBox3.Size = new System.Drawing.Size(577, 53);
            this.groupBox3.TabIndex = 10;
            this.groupBox3.TabStop = false;
            this.groupBox3.Text = "Tipo";
            // 
            // btCirc
            // 
            this.btCirc.BackColor = System.Drawing.Color.Transparent;
            this.btCirc.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btCirc.ImageAlign = System.Drawing.ContentAlignment.BottomLeft;
            this.btCirc.ImageKey = "PERFIL CIRUCLO.bmp";
            this.btCirc.Location = new System.Drawing.Point(44, 20);
            this.btCirc.Margin = new System.Windows.Forms.Padding(0);
            this.btCirc.Name = "btCirc";
            this.btCirc.Size = new System.Drawing.Size(29, 25);
            this.btCirc.TabIndex = 1;
            this.btCirc.Tag = "2";
            this.btCirc.TextAlign = System.Drawing.ContentAlignment.BottomCenter;
            this.btCirc.TextImageRelation = System.Windows.Forms.TextImageRelation.TextBeforeImage;
            this.btCirc.UseVisualStyleBackColor = false;
            this.btCirc.Click += new System.EventHandler(this.btRet_Click);
            this.btCirc.KeyDown += new System.Windows.Forms.KeyEventHandler(this.btCor_KeyDown);
            // 
            // btRet
            // 
            this.btRet.BackColor = System.Drawing.Color.Transparent;
            this.btRet.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btRet.ImageAlign = System.Drawing.ContentAlignment.BottomLeft;
            this.btRet.ImageKey = "PERFIL MACIÇO.bmp";
            this.btRet.Location = new System.Drawing.Point(10, 20);
            this.btRet.Margin = new System.Windows.Forms.Padding(0);
            this.btRet.Name = "btRet";
            this.btRet.Size = new System.Drawing.Size(29, 25);
            this.btRet.TabIndex = 0;
            this.btRet.Tag = "1";
            this.btRet.TextAlign = System.Drawing.ContentAlignment.BottomCenter;
            this.btRet.TextImageRelation = System.Windows.Forms.TextImageRelation.TextBeforeImage;
            this.btRet.UseVisualStyleBackColor = false;
            this.btRet.Click += new System.EventHandler(this.btRet_Click);
            this.btRet.KeyDown += new System.Windows.Forms.KeyEventHandler(this.btCor_KeyDown);
            // 
            // btCalculaSecao
            // 
            this.btCalculaSecao.Location = new System.Drawing.Point(369, 143);
            this.btCalculaSecao.Name = "btCalculaSecao";
            this.btCalculaSecao.Size = new System.Drawing.Size(168, 33);
            this.btCalculaSecao.TabIndex = 164;
            this.btCalculaSecao.Text = "Calculadora de seção - MEF";
            this.btCalculaSecao.UseVisualStyleBackColor = true;
            // 
            // FSecaoSolida
            // 
            this.AcceptButton = this.btSalvar;
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.CancelButton = this.button3;
            this.ClientSize = new System.Drawing.Size(577, 438);
            this.Controls.Add(this.btCalculaSecao);
            this.Controls.Add(this.groupBox3);
            this.Controls.Add(this.gbTipo);
            this.Controls.Add(this.groupBox1);
            this.Controls.Add(this.panel1);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedToolWindow;
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "FSecaoSolida";
            this.Tag = "Secoes";
            this.Text = "Seção - Maciça";
            this.TopMost = true;
            this.FormClosed += new System.Windows.Forms.FormClosedEventHandler(this.FSecaoSolida_FormClosed);
            this.Load += new System.EventHandler(this.FSecaoSolida_Load);
            this.KeyDown += new System.Windows.Forms.KeyEventHandler(this.FSecaoSolida_KeyDown);
            this.MouseMove += new System.Windows.Forms.MouseEventHandler(this.FSecaoSolida_MouseMove);
            this.Move += new System.EventHandler(this.FSecaoSolida_Move);
            this.Validated += new System.EventHandler(this.s);
            this.panel1.ResumeLayout(false);
            this.groupBox1.ResumeLayout(false);
            this.groupBox1.PerformLayout();
            this.gbTipo.ResumeLayout(false);
            this.pnCirculo.ResumeLayout(false);
            this.pnCirculo.PerformLayout();
            this.pnRetangulo.ResumeLayout(false);
            this.pnRetangulo.PerformLayout();
            this.groupBox3.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion
        private System.Windows.Forms.Panel panel1;
        private System.Windows.Forms.Button button3;
        public System.Windows.Forms.Button btSalvar;
        private System.Windows.Forms.GroupBox groupBox1;
        private System.Windows.Forms.GroupBox gbTipo;
        private System.Windows.Forms.Panel pnCirculo;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.Label label1;
        public System.Windows.Forms.TextBox edD;
        private System.Windows.Forms.Panel pnRetangulo;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.Label label12;
        private System.Windows.Forms.Label label2;
        public System.Windows.Forms.TextBox edh1;
        private System.Windows.Forms.ColorDialog colorDialog1;
        private System.Windows.Forms.Label label6;
        private System.Windows.Forms.TextBox NomeSecao;
        private System.Windows.Forms.Label label7;
        private System.Windows.Forms.Button btCor;
        private System.Windows.Forms.GroupBox groupBox3;
        public System.Windows.Forms.Button btCirc;
        public System.Windows.Forms.Button btRet;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.ComboBox cbMaterial;
        private System.Windows.Forms.Button button1;
        public System.Windows.Forms.TextBox edb1;
        public System.Windows.Forms.ImageList imageList2;
        private System.Windows.Forms.Button btCalculaSecao;
    }
}