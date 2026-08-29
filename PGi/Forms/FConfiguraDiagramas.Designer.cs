namespace PG
{
    partial class FConfiguraDiagramas
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(FConfiguraDiagramas));
            this.panel1 = new System.Windows.Forms.Panel();
            this.button3 = new System.Windows.Forms.Button();
            this.imageList2 = new System.Windows.Forms.ImageList(this.components);
            this.button1 = new System.Windows.Forms.Button();
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.rbGradiente = new System.Windows.Forms.RadioButton();
            this.rbLinhas = new System.Windows.Forms.RadioButton();
            this.colorDialog1 = new System.Windows.Forms.ColorDialog();
            this.groupBox2 = new System.Windows.Forms.GroupBox();
            this.rbExibirTudo = new System.Windows.Forms.RadioButton();
            this.rbExibirPerc = new System.Windows.Forms.RadioButton();
            this.percentualValor = new System.Windows.Forms.TextBox();
            this.label8 = new System.Windows.Forms.Label();
            this.groupBox3 = new System.Windows.Forms.GroupBox();
            this.chLegenda = new System.Windows.Forms.CheckBox();
            this.button2 = new System.Windows.Forms.Button();
            this.chContorno = new System.Windows.Forms.CheckBox();
            this.groupBox4 = new System.Windows.Forms.GroupBox();
            this.chLinhaGradiente = new System.Windows.Forms.CheckBox();
            this.btCorLinha = new System.Windows.Forms.Button();
            this.groupBox5 = new System.Windows.Forms.GroupBox();
            this.cbAlinhamentoDiagrama = new System.Windows.Forms.ComboBox();
            this.label1 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.btCorValor = new System.Windows.Forms.Button();
            this.panel1.SuspendLayout();
            this.groupBox1.SuspendLayout();
            this.groupBox2.SuspendLayout();
            this.groupBox3.SuspendLayout();
            this.groupBox4.SuspendLayout();
            this.groupBox5.SuspendLayout();
            this.SuspendLayout();
            // 
            // panel1
            // 
            this.panel1.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.panel1.Controls.Add(this.button3);
            this.panel1.Controls.Add(this.button1);
            this.panel1.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.panel1.Location = new System.Drawing.Point(0, 221);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(265, 35);
            this.panel1.TabIndex = 3;
            this.panel1.Paint += new System.Windows.Forms.PaintEventHandler(this.panel1_Paint);
            // 
            // button3
            // 
            this.button3.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.button3.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.button3.ImageIndex = 1;
            this.button3.ImageList = this.imageList2;
            this.button3.Location = new System.Drawing.Point(186, 3);
            this.button3.Name = "button3";
            this.button3.Size = new System.Drawing.Size(73, 25);
            this.button3.TabIndex = 18;
            this.button3.UseVisualStyleBackColor = true;
            this.button3.Click += new System.EventHandler(this.button3_Click);
            // 
            // imageList2
            // 
            this.imageList2.ImageStream = ((System.Windows.Forms.ImageListStreamer)(resources.GetObject("imageList2.ImageStream")));
            this.imageList2.TransparentColor = System.Drawing.Color.Transparent;
            this.imageList2.Images.SetKeyName(0, "confirmar.bmp");
            this.imageList2.Images.SetKeyName(1, "fechar.bmp");
            // 
            // button1
            // 
            this.button1.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.button1.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.button1.ImageIndex = 0;
            this.button1.ImageList = this.imageList2;
            this.button1.Location = new System.Drawing.Point(4, 3);
            this.button1.Name = "button1";
            this.button1.Size = new System.Drawing.Size(73, 25);
            this.button1.TabIndex = 17;
            this.button1.UseVisualStyleBackColor = true;
            this.button1.Click += new System.EventHandler(this.button1_Click);
            // 
            // groupBox1
            // 
            this.groupBox1.Controls.Add(this.rbGradiente);
            this.groupBox1.Controls.Add(this.rbLinhas);
            this.groupBox1.Location = new System.Drawing.Point(6, 8);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(254, 49);
            this.groupBox1.TabIndex = 4;
            this.groupBox1.TabStop = false;
            this.groupBox1.Text = "Representação";
            // 
            // rbGradiente
            // 
            this.rbGradiente.AutoSize = true;
            this.rbGradiente.Location = new System.Drawing.Point(135, 22);
            this.rbGradiente.Name = "rbGradiente";
            this.rbGradiente.Size = new System.Drawing.Size(115, 17);
            this.rbGradiente.TabIndex = 1;
            this.rbGradiente.TabStop = true;
            this.rbGradiente.Text = "Gradiente de cores";
            this.rbGradiente.UseVisualStyleBackColor = true;
            this.rbGradiente.CheckedChanged += new System.EventHandler(this.rbGradiente_CheckedChanged);
            // 
            // rbLinhas
            // 
            this.rbLinhas.AutoSize = true;
            this.rbLinhas.Location = new System.Drawing.Point(6, 22);
            this.rbLinhas.Name = "rbLinhas";
            this.rbLinhas.Size = new System.Drawing.Size(97, 17);
            this.rbLinhas.TabIndex = 0;
            this.rbLinhas.TabStop = true;
            this.rbLinhas.Text = "Somente linhas";
            this.rbLinhas.UseVisualStyleBackColor = true;
            this.rbLinhas.CheckedChanged += new System.EventHandler(this.rbLinhas_CheckedChanged);
            // 
            // groupBox2
            // 
            this.groupBox2.Controls.Add(this.rbExibirTudo);
            this.groupBox2.Controls.Add(this.rbExibirPerc);
            this.groupBox2.Controls.Add(this.percentualValor);
            this.groupBox2.Controls.Add(this.label8);
            this.groupBox2.Location = new System.Drawing.Point(428, 205);
            this.groupBox2.Name = "groupBox2";
            this.groupBox2.Size = new System.Drawing.Size(369, 78);
            this.groupBox2.TabIndex = 73;
            this.groupBox2.TabStop = false;
            this.groupBox2.Text = "Alinhamento Valores";
            // 
            // rbExibirTudo
            // 
            this.rbExibirTudo.AutoSize = true;
            this.rbExibirTudo.Location = new System.Drawing.Point(6, 55);
            this.rbExibirTudo.Name = "rbExibirTudo";
            this.rbExibirTudo.Size = new System.Drawing.Size(74, 17);
            this.rbExibirTudo.TabIndex = 37;
            this.rbExibirTudo.TabStop = true;
            this.rbExibirTudo.Text = "Exibir tudo";
            this.rbExibirTudo.UseVisualStyleBackColor = true;
            this.rbExibirTudo.CheckedChanged += new System.EventHandler(this.rbExibirMaximo_CheckedChanged);
            // 
            // rbExibirPerc
            // 
            this.rbExibirPerc.AutoSize = true;
            this.rbExibirPerc.Location = new System.Drawing.Point(6, 27);
            this.rbExibirPerc.Name = "rbExibirPerc";
            this.rbExibirPerc.Size = new System.Drawing.Size(162, 17);
            this.rbExibirPerc.TabIndex = 36;
            this.rbExibirPerc.TabStop = true;
            this.rbExibirPerc.Text = "Exibir valores maiores do que";
            this.rbExibirPerc.UseVisualStyleBackColor = true;
            this.rbExibirPerc.CheckedChanged += new System.EventHandler(this.rbExibirPerc_CheckedChanged);
            // 
            // percentualValor
            // 
            this.percentualValor.Location = new System.Drawing.Point(263, 47);
            this.percentualValor.Name = "percentualValor";
            this.percentualValor.Size = new System.Drawing.Size(49, 20);
            this.percentualValor.TabIndex = 32;
            this.percentualValor.Text = "0";
            this.percentualValor.Visible = false;
            this.percentualValor.KeyPress += new System.Windows.Forms.KeyPressEventHandler(this.percentualValor_KeyPress);
            // 
            // label8
            // 
            this.label8.AutoSize = true;
            this.label8.Location = new System.Drawing.Point(223, 31);
            this.label8.Name = "label8";
            this.label8.Size = new System.Drawing.Size(117, 13);
            this.label8.TabIndex = 31;
            this.label8.Text = "%  do resultado máximo";
            this.label8.Visible = false;
            // 
            // groupBox3
            // 
            this.groupBox3.Controls.Add(this.chLegenda);
            this.groupBox3.Controls.Add(this.button2);
            this.groupBox3.Controls.Add(this.chContorno);
            this.groupBox3.Location = new System.Drawing.Point(404, 38);
            this.groupBox3.Name = "groupBox3";
            this.groupBox3.Size = new System.Drawing.Size(256, 111);
            this.groupBox3.TabIndex = 84;
            this.groupBox3.TabStop = false;
            this.groupBox3.Text = "Opções de visualização";
            this.groupBox3.Visible = false;
            // 
            // chLegenda
            // 
            this.chLegenda.AutoSize = true;
            this.chLegenda.Location = new System.Drawing.Point(6, 23);
            this.chLegenda.Name = "chLegenda";
            this.chLegenda.Size = new System.Drawing.Size(68, 17);
            this.chLegenda.TabIndex = 88;
            this.chLegenda.Text = "Legenda";
            this.chLegenda.UseVisualStyleBackColor = true;
            // 
            // button2
            // 
            this.button2.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.button2.Image = ((System.Drawing.Image)(resources.GetObject("button2.Image")));
            this.button2.Location = new System.Drawing.Point(167, 16);
            this.button2.Name = "button2";
            this.button2.Size = new System.Drawing.Size(26, 24);
            this.button2.TabIndex = 85;
            this.button2.Text = "...";
            this.button2.UseVisualStyleBackColor = true;
            this.button2.Click += new System.EventHandler(this.button2_Click);
            // 
            // chContorno
            // 
            this.chContorno.AutoSize = true;
            this.chContorno.Location = new System.Drawing.Point(99, 23);
            this.chContorno.Name = "chContorno";
            this.chContorno.Size = new System.Drawing.Size(69, 17);
            this.chContorno.TabIndex = 84;
            this.chContorno.Text = "Contorno";
            this.chContorno.UseVisualStyleBackColor = true;
            // 
            // groupBox4
            // 
            this.groupBox4.Controls.Add(this.chLinhaGradiente);
            this.groupBox4.Controls.Add(this.btCorLinha);
            this.groupBox4.Location = new System.Drawing.Point(6, 63);
            this.groupBox4.Name = "groupBox4";
            this.groupBox4.Size = new System.Drawing.Size(254, 49);
            this.groupBox4.TabIndex = 97;
            this.groupBox4.TabStop = false;
            this.groupBox4.Text = "Cor das linhas";
            // 
            // chLinhaGradiente
            // 
            this.chLinhaGradiente.AutoSize = true;
            this.chLinhaGradiente.Location = new System.Drawing.Point(82, 26);
            this.chLinhaGradiente.Name = "chLinhaGradiente";
            this.chLinhaGradiente.Size = new System.Drawing.Size(75, 17);
            this.chLinhaGradiente.TabIndex = 98;
            this.chLinhaGradiente.Text = "Gradiente ";
            this.chLinhaGradiente.UseVisualStyleBackColor = true;
            // 
            // btCorLinha
            // 
            this.btCorLinha.BackColor = System.Drawing.Color.Lime;
            this.btCorLinha.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btCorLinha.Location = new System.Drawing.Point(6, 24);
            this.btCorLinha.Name = "btCorLinha";
            this.btCorLinha.Size = new System.Drawing.Size(19, 19);
            this.btCorLinha.TabIndex = 97;
            this.btCorLinha.UseVisualStyleBackColor = false;
            this.btCorLinha.Click += new System.EventHandler(this.btCorLinha_Click_1);
            // 
            // groupBox5
            // 
            this.groupBox5.Controls.Add(this.cbAlinhamentoDiagrama);
            this.groupBox5.Controls.Add(this.label1);
            this.groupBox5.Controls.Add(this.label2);
            this.groupBox5.Controls.Add(this.btCorValor);
            this.groupBox5.Location = new System.Drawing.Point(6, 118);
            this.groupBox5.Name = "groupBox5";
            this.groupBox5.Size = new System.Drawing.Size(254, 88);
            this.groupBox5.TabIndex = 98;
            this.groupBox5.TabStop = false;
            this.groupBox5.Text = "Valores";
            // 
            // cbAlinhamentoDiagrama
            // 
            this.cbAlinhamentoDiagrama.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cbAlinhamentoDiagrama.FormattingEnabled = true;
            this.cbAlinhamentoDiagrama.Items.AddRange(new object[] {
            "Mesmo ângulo do diagrama",
            "Horizontal",
            "Vertical"});
            this.cbAlinhamentoDiagrama.Location = new System.Drawing.Point(96, 54);
            this.cbAlinhamentoDiagrama.Name = "cbAlinhamentoDiagrama";
            this.cbAlinhamentoDiagrama.Size = new System.Drawing.Size(153, 21);
            this.cbAlinhamentoDiagrama.TabIndex = 97;
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(9, 62);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(65, 13);
            this.label1.TabIndex = 96;
            this.label1.Text = "Alinhamento";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(9, 31);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(80, 13);
            this.label2.TabIndex = 95;
            this.label2.Text = "Cor dos valores";
            // 
            // btCorValor
            // 
            this.btCorValor.BackColor = System.Drawing.Color.Maroon;
            this.btCorValor.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btCorValor.Location = new System.Drawing.Point(96, 25);
            this.btCorValor.Name = "btCorValor";
            this.btCorValor.Size = new System.Drawing.Size(19, 19);
            this.btCorValor.TabIndex = 94;
            this.btCorValor.UseVisualStyleBackColor = false;
            this.btCorValor.Click += new System.EventHandler(this.btCorValor_Click);
            // 
            // FConfiguraDiagramas
            // 
            this.AcceptButton = this.button1;
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.CancelButton = this.button3;
            this.ClientSize = new System.Drawing.Size(265, 256);
            this.Controls.Add(this.groupBox5);
            this.Controls.Add(this.groupBox4);
            this.Controls.Add(this.groupBox3);
            this.Controls.Add(this.groupBox2);
            this.Controls.Add(this.groupBox1);
            this.Controls.Add(this.panel1);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedToolWindow;
            this.Name = "FConfiguraDiagramas";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "Configuração de visualização dos diagramas";
            this.FormClosed += new System.Windows.Forms.FormClosedEventHandler(this.FConfiguraDiagramas_FormClosed);
            this.Load += new System.EventHandler(this.FConfiguraDiagramas_Load);
            this.Move += new System.EventHandler(this.FConfiguraDiagramas_Move);
            this.panel1.ResumeLayout(false);
            this.groupBox1.ResumeLayout(false);
            this.groupBox1.PerformLayout();
            this.groupBox2.ResumeLayout(false);
            this.groupBox2.PerformLayout();
            this.groupBox3.ResumeLayout(false);
            this.groupBox3.PerformLayout();
            this.groupBox4.ResumeLayout(false);
            this.groupBox4.PerformLayout();
            this.groupBox5.ResumeLayout(false);
            this.groupBox5.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion
        private System.Windows.Forms.Panel panel1;
        private System.Windows.Forms.Button button3;
        private System.Windows.Forms.Button button1;
        private System.Windows.Forms.GroupBox groupBox1;
        private System.Windows.Forms.RadioButton rbGradiente;
        private System.Windows.Forms.RadioButton rbLinhas;
        private System.Windows.Forms.ColorDialog colorDialog1;
        private System.Windows.Forms.GroupBox groupBox2;
        public System.Windows.Forms.TextBox percentualValor;
        private System.Windows.Forms.Label label8;
        private System.Windows.Forms.GroupBox groupBox3;
        public System.Windows.Forms.ImageList imageList2;
        private System.Windows.Forms.RadioButton rbExibirPerc;
        private System.Windows.Forms.CheckBox chLegenda;
        private System.Windows.Forms.Button button2;
        private System.Windows.Forms.CheckBox chContorno;
        private System.Windows.Forms.RadioButton rbExibirTudo;
        private System.Windows.Forms.GroupBox groupBox4;
        private System.Windows.Forms.Button btCorLinha;
        private System.Windows.Forms.CheckBox chLinhaGradiente;
        private System.Windows.Forms.GroupBox groupBox5;
        private System.Windows.Forms.ComboBox cbAlinhamentoDiagrama;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Button btCorValor;
    }
}