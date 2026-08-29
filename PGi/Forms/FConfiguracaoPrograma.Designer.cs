namespace PG
{
    partial class FConfiguracaoPrograma
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(FConfiguracaoPrograma));
            this.tabControl1 = new System.Windows.Forms.TabControl();
            this.tabPage1 = new System.Windows.Forms.TabPage();
            this.groupBox4 = new System.Windows.Forms.GroupBox();
            this.chAbrirUltimoProjeto = new System.Windows.Forms.CheckBox();
            this.groupBox3 = new System.Windows.Forms.GroupBox();
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.chSuavizacao = new System.Windows.Forms.CheckBox();
            this.groupBox2 = new System.Windows.Forms.GroupBox();
            this.cbFecharJanelaMensagensAposCalculo = new System.Windows.Forms.CheckBox();
            this.cbIrParaResultados = new System.Windows.Forms.CheckBox();
            this.panel1 = new System.Windows.Forms.Panel();
            this.button3 = new System.Windows.Forms.Button();
            this.imageList2 = new System.Windows.Forms.ImageList(this.components);
            this.button1 = new System.Windows.Forms.Button();
            this.tabControl1.SuspendLayout();
            this.tabPage1.SuspendLayout();
            this.groupBox4.SuspendLayout();
            this.groupBox3.SuspendLayout();
            this.groupBox1.SuspendLayout();
            this.groupBox2.SuspendLayout();
            this.panel1.SuspendLayout();
            this.SuspendLayout();
            // 
            // tabControl1
            // 
            this.tabControl1.Controls.Add(this.tabPage1);
            this.tabControl1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tabControl1.Location = new System.Drawing.Point(0, 0);
            this.tabControl1.Name = "tabControl1";
            this.tabControl1.SelectedIndex = 0;
            this.tabControl1.Size = new System.Drawing.Size(419, 407);
            this.tabControl1.TabIndex = 73;
            // 
            // tabPage1
            // 
            this.tabPage1.Controls.Add(this.groupBox4);
            this.tabPage1.Controls.Add(this.groupBox3);
            this.tabPage1.Controls.Add(this.groupBox2);
            this.tabPage1.Location = new System.Drawing.Point(4, 22);
            this.tabPage1.Name = "tabPage1";
            this.tabPage1.Size = new System.Drawing.Size(411, 381);
            this.tabPage1.TabIndex = 4;
            this.tabPage1.Text = "Geral";
            this.tabPage1.UseVisualStyleBackColor = true;
            // 
            // groupBox4
            // 
            this.groupBox4.Controls.Add(this.chAbrirUltimoProjeto);
            this.groupBox4.Location = new System.Drawing.Point(8, 214);
            this.groupBox4.Name = "groupBox4";
            this.groupBox4.Size = new System.Drawing.Size(290, 100);
            this.groupBox4.TabIndex = 100;
            this.groupBox4.TabStop = false;
            this.groupBox4.Text = "Geral";
            // 
            // chAbrirUltimoProjeto
            // 
            this.chAbrirUltimoProjeto.AutoSize = true;
            this.chAbrirUltimoProjeto.Location = new System.Drawing.Point(41, 28);
            this.chAbrirUltimoProjeto.Name = "chAbrirUltimoProjeto";
            this.chAbrirUltimoProjeto.Size = new System.Drawing.Size(213, 17);
            this.chAbrirUltimoProjeto.TabIndex = 96;
            this.chAbrirUltimoProjeto.Text = "Abrir último projeto ao iniciar o programa";
            this.chAbrirUltimoProjeto.UseVisualStyleBackColor = true;
            // 
            // groupBox3
            // 
            this.groupBox3.Controls.Add(this.groupBox1);
            this.groupBox3.Location = new System.Drawing.Point(8, 108);
            this.groupBox3.Name = "groupBox3";
            this.groupBox3.Size = new System.Drawing.Size(281, 100);
            this.groupBox3.TabIndex = 99;
            this.groupBox3.TabStop = false;
            this.groupBox3.Text = "Gráficos";
            // 
            // groupBox1
            // 
            this.groupBox1.Controls.Add(this.chSuavizacao);
            this.groupBox1.Location = new System.Drawing.Point(41, 19);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(231, 46);
            this.groupBox1.TabIndex = 1;
            this.groupBox1.TabStop = false;
            this.groupBox1.Text = "OpenGL";
            // 
            // chSuavizacao
            // 
            this.chSuavizacao.AutoSize = true;
            this.chSuavizacao.Location = new System.Drawing.Point(6, 19);
            this.chSuavizacao.Name = "chSuavizacao";
            this.chSuavizacao.Size = new System.Drawing.Size(146, 17);
            this.chSuavizacao.TabIndex = 95;
            this.chSuavizacao.Text = "Suavização (anti-aliasing)";
            this.chSuavizacao.UseVisualStyleBackColor = true;
            // 
            // groupBox2
            // 
            this.groupBox2.Controls.Add(this.cbFecharJanelaMensagensAposCalculo);
            this.groupBox2.Controls.Add(this.cbIrParaResultados);
            this.groupBox2.Location = new System.Drawing.Point(8, 16);
            this.groupBox2.Name = "groupBox2";
            this.groupBox2.Size = new System.Drawing.Size(281, 86);
            this.groupBox2.TabIndex = 98;
            this.groupBox2.TabStop = false;
            this.groupBox2.Text = "Cálculo";
            // 
            // cbFecharJanelaMensagensAposCalculo
            // 
            this.cbFecharJanelaMensagensAposCalculo.AutoSize = true;
            this.cbFecharJanelaMensagensAposCalculo.Location = new System.Drawing.Point(41, 46);
            this.cbFecharJanelaMensagensAposCalculo.Name = "cbFecharJanelaMensagensAposCalculo";
            this.cbFecharJanelaMensagensAposCalculo.Size = new System.Drawing.Size(234, 17);
            this.cbFecharJanelaMensagensAposCalculo.TabIndex = 99;
            this.cbFecharJanelaMensagensAposCalculo.Text = "Fechar janela de mensagens após o cálculo";
            this.cbFecharJanelaMensagensAposCalculo.UseVisualStyleBackColor = true;
            // 
            // cbIrParaResultados
            // 
            this.cbIrParaResultados.AutoSize = true;
            this.cbIrParaResultados.Location = new System.Drawing.Point(41, 23);
            this.cbIrParaResultados.Name = "cbIrParaResultados";
            this.cbIrParaResultados.Size = new System.Drawing.Size(237, 17);
            this.cbIrParaResultados.TabIndex = 98;
            this.cbIrParaResultados.Text = "Ir para a página de resultados após a análise";
            this.cbIrParaResultados.UseVisualStyleBackColor = true;
            // 
            // panel1
            // 
            this.panel1.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.panel1.Controls.Add(this.button3);
            this.panel1.Controls.Add(this.button1);
            this.panel1.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.panel1.Location = new System.Drawing.Point(0, 372);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(419, 35);
            this.panel1.TabIndex = 74;
            // 
            // button3
            // 
            this.button3.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.button3.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.button3.ImageIndex = 1;
            this.button3.ImageList = this.imageList2;
            this.button3.Location = new System.Drawing.Point(334, 3);
            this.button3.Name = "button3";
            this.button3.Size = new System.Drawing.Size(80, 25);
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
            this.button1.Size = new System.Drawing.Size(80, 25);
            this.button1.TabIndex = 17;
            this.button1.UseVisualStyleBackColor = true;
            this.button1.Click += new System.EventHandler(this.button1_Click);
            // 
            // FConfiguracaoPrograma
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.CancelButton = this.button3;
            this.ClientSize = new System.Drawing.Size(419, 407);
            this.Controls.Add(this.panel1);
            this.Controls.Add(this.tabControl1);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedToolWindow;
            this.Name = "FConfiguracaoPrograma";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "Preferências do programa";
            this.FormClosed += new System.Windows.Forms.FormClosedEventHandler(this.FConfiguracaoPrograma_FormClosed);
            this.Load += new System.EventHandler(this.FConfiguracaoPrograma_Load);
            this.Move += new System.EventHandler(this.FConfiguracaoPrograma_Move);
            this.tabControl1.ResumeLayout(false);
            this.tabPage1.ResumeLayout(false);
            this.groupBox4.ResumeLayout(false);
            this.groupBox4.PerformLayout();
            this.groupBox3.ResumeLayout(false);
            this.groupBox1.ResumeLayout(false);
            this.groupBox1.PerformLayout();
            this.groupBox2.ResumeLayout(false);
            this.groupBox2.PerformLayout();
            this.panel1.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.TabControl tabControl1;
        private System.Windows.Forms.Panel panel1;
        private System.Windows.Forms.Button button3;
        private System.Windows.Forms.Button button1;
        private System.Windows.Forms.TabPage tabPage1;
        public System.Windows.Forms.ImageList imageList2;
        private System.Windows.Forms.GroupBox groupBox3;
        private System.Windows.Forms.GroupBox groupBox1;
        private System.Windows.Forms.CheckBox chSuavizacao;
        private System.Windows.Forms.GroupBox groupBox2;
        private System.Windows.Forms.CheckBox cbFecharJanelaMensagensAposCalculo;
        private System.Windows.Forms.CheckBox cbIrParaResultados;
        private System.Windows.Forms.GroupBox groupBox4;
        private System.Windows.Forms.CheckBox chAbrirUltimoProjeto;
    }
}