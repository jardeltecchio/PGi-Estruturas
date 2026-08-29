namespace PG
{
    partial class FGrelhaConfiguracao
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
            this.panel1 = new System.Windows.Forms.Panel();
            this.button3 = new System.Windows.Forms.Button();
            this.button1 = new System.Windows.Forms.Button();
            this.groupBox2 = new System.Windows.Forms.GroupBox();
            this.lbPavimentos = new System.Windows.Forms.ListBox();
            this.groupBox3 = new System.Windows.Forms.GroupBox();
            this.gbDiscretizacao = new System.Windows.Forms.GroupBox();
            this.button2 = new System.Windows.Forms.Button();
            this.label5 = new System.Windows.Forms.Label();
            this.edEspacamentoY = new System.Windows.Forms.NumericUpDown();
            this.label4 = new System.Windows.Forms.Label();
            this.label3 = new System.Windows.Forms.Label();
            this.edEspacamentoX = new System.Windows.Forms.NumericUpDown();
            this.label2 = new System.Windows.Forms.Label();
            this.label1 = new System.Windows.Forms.Label();
            this.edAnguloBarras = new System.Windows.Forms.NumericUpDown();
            this.labelPiso = new System.Windows.Forms.Label();
            this.rbGerarNovaGrelha = new System.Windows.Forms.RadioButton();
            this.rbConsiderarGrelhaEditada = new System.Windows.Forms.RadioButton();
            this.panel1.SuspendLayout();
            this.groupBox2.SuspendLayout();
            this.groupBox3.SuspendLayout();
            this.gbDiscretizacao.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.edEspacamentoY)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.edEspacamentoX)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.edAnguloBarras)).BeginInit();
            this.SuspendLayout();
            // 
            // panel1
            // 
            this.panel1.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.panel1.Controls.Add(this.button3);
            this.panel1.Controls.Add(this.button1);
            this.panel1.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.panel1.Location = new System.Drawing.Point(0, 401);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(462, 35);
            this.panel1.TabIndex = 6;
            // 
            // button3
            // 
            this.button3.Location = new System.Drawing.Point(338, 3);
            this.button3.Name = "button3";
            this.button3.Size = new System.Drawing.Size(115, 25);
            this.button3.TabIndex = 18;
            this.button3.Text = "Cancelar";
            this.button3.UseVisualStyleBackColor = true;
            this.button3.Click += new System.EventHandler(this.button3_Click);
            // 
            // button1
            // 
            this.button1.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.button1.Location = new System.Drawing.Point(4, 3);
            this.button1.Name = "button1";
            this.button1.Size = new System.Drawing.Size(115, 25);
            this.button1.TabIndex = 17;
            this.button1.Text = "Gravar";
            this.button1.UseVisualStyleBackColor = true;
            this.button1.Click += new System.EventHandler(this.button1_Click);
            // 
            // groupBox2
            // 
            this.groupBox2.Controls.Add(this.lbPavimentos);
            this.groupBox2.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.groupBox2.Location = new System.Drawing.Point(12, 12);
            this.groupBox2.Name = "groupBox2";
            this.groupBox2.Size = new System.Drawing.Size(150, 383);
            this.groupBox2.TabIndex = 32;
            this.groupBox2.TabStop = false;
            this.groupBox2.Text = "Pisos:";
            // 
            // lbPavimentos
            // 
            this.lbPavimentos.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.lbPavimentos.Dock = System.Windows.Forms.DockStyle.Fill;
            this.lbPavimentos.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lbPavimentos.FormattingEnabled = true;
            this.lbPavimentos.Location = new System.Drawing.Point(3, 16);
            this.lbPavimentos.Name = "lbPavimentos";
            this.lbPavimentos.Size = new System.Drawing.Size(144, 364);
            this.lbPavimentos.TabIndex = 28;
            this.lbPavimentos.Click += new System.EventHandler(this.lbPavimentos_Click);
            this.lbPavimentos.SelectedIndexChanged += new System.EventHandler(this.lbPavimentos_SelectedIndexChanged);
            // 
            // groupBox3
            // 
            this.groupBox3.Controls.Add(this.rbConsiderarGrelhaEditada);
            this.groupBox3.Controls.Add(this.rbGerarNovaGrelha);
            this.groupBox3.Location = new System.Drawing.Point(168, 48);
            this.groupBox3.Name = "groupBox3";
            this.groupBox3.Size = new System.Drawing.Size(284, 40);
            this.groupBox3.TabIndex = 34;
            this.groupBox3.TabStop = false;
            this.groupBox3.Text = "Grelha";
            // 
            // gbDiscretizacao
            // 
            this.gbDiscretizacao.Controls.Add(this.button2);
            this.gbDiscretizacao.Controls.Add(this.label5);
            this.gbDiscretizacao.Controls.Add(this.edEspacamentoY);
            this.gbDiscretizacao.Controls.Add(this.label4);
            this.gbDiscretizacao.Controls.Add(this.label3);
            this.gbDiscretizacao.Controls.Add(this.edEspacamentoX);
            this.gbDiscretizacao.Controls.Add(this.label2);
            this.gbDiscretizacao.Controls.Add(this.label1);
            this.gbDiscretizacao.Controls.Add(this.edAnguloBarras);
            this.gbDiscretizacao.Location = new System.Drawing.Point(168, 99);
            this.gbDiscretizacao.Name = "gbDiscretizacao";
            this.gbDiscretizacao.Size = new System.Drawing.Size(284, 129);
            this.gbDiscretizacao.TabIndex = 33;
            this.gbDiscretizacao.TabStop = false;
            this.gbDiscretizacao.Text = "Discretização das barras";
            // 
            // button2
            // 
            this.button2.Location = new System.Drawing.Point(7, 96);
            this.button2.Name = "button2";
            this.button2.Size = new System.Drawing.Size(241, 23);
            this.button2.TabIndex = 10;
            this.button2.Text = "Aplicar essa discretização em todos os pisos";
            this.button2.UseVisualStyleBackColor = true;
            this.button2.Click += new System.EventHandler(this.button2_Click);
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.Location = new System.Drawing.Point(218, 30);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(21, 13);
            this.label5.TabIndex = 9;
            this.label5.Text = "cm";
            // 
            // edEspacamentoY
            // 
            this.edEspacamentoY.Location = new System.Drawing.Point(168, 23);
            this.edEspacamentoY.Maximum = new decimal(new int[] {
            500,
            0,
            0,
            0});
            this.edEspacamentoY.Name = "edEspacamentoY";
            this.edEspacamentoY.Size = new System.Drawing.Size(44, 20);
            this.edEspacamentoY.TabIndex = 8;
            this.edEspacamentoY.Value = new decimal(new int[] {
            35,
            0,
            0,
            0});
            this.edEspacamentoY.ValueChanged += new System.EventHandler(this.edEspacamentoY_ValueChanged);
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(153, 30);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(12, 13);
            this.label4.TabIndex = 7;
            this.label4.Text = "x";
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(6, 30);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(72, 13);
            this.label3.TabIndex = 6;
            this.label3.Text = "Espaçamento";
            // 
            // edEspacamentoX
            // 
            this.edEspacamentoX.Location = new System.Drawing.Point(104, 23);
            this.edEspacamentoX.Maximum = new decimal(new int[] {
            500,
            0,
            0,
            0});
            this.edEspacamentoX.Name = "edEspacamentoX";
            this.edEspacamentoX.Size = new System.Drawing.Size(44, 20);
            this.edEspacamentoX.TabIndex = 5;
            this.edEspacamentoX.Value = new decimal(new int[] {
            35,
            0,
            0,
            0});
            this.edEspacamentoX.ValueChanged += new System.EventHandler(this.edEspacamentoX_ValueChanged);
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Font = new System.Drawing.Font("Microsoft Sans Serif", 4F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label2.Location = new System.Drawing.Point(154, 61);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(7, 7);
            this.label2.TabIndex = 4;
            this.label2.Text = "o";
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(6, 68);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(92, 13);
            this.label1.TabIndex = 3;
            this.label1.Text = "Ângulo das barras";
            // 
            // edAnguloBarras
            // 
            this.edAnguloBarras.Location = new System.Drawing.Point(104, 61);
            this.edAnguloBarras.Name = "edAnguloBarras";
            this.edAnguloBarras.Size = new System.Drawing.Size(44, 20);
            this.edAnguloBarras.TabIndex = 2;
            this.edAnguloBarras.ValueChanged += new System.EventHandler(this.edAnguloBarras_ValueChanged);
            // 
            // labelPiso
            // 
            this.labelPiso.AutoSize = true;
            this.labelPiso.Font = new System.Drawing.Font("Tahoma", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.labelPiso.Location = new System.Drawing.Point(172, 21);
            this.labelPiso.Name = "labelPiso";
            this.labelPiso.Size = new System.Drawing.Size(36, 14);
            this.labelPiso.TabIndex = 35;
            this.labelPiso.Text = "Piso:";
            // 
            // rbGerarNovaGrelha
            // 
            this.rbGerarNovaGrelha.AutoSize = true;
            this.rbGerarNovaGrelha.Checked = true;
            this.rbGerarNovaGrelha.Location = new System.Drawing.Point(9, 19);
            this.rbGerarNovaGrelha.Name = "rbGerarNovaGrelha";
            this.rbGerarNovaGrelha.Size = new System.Drawing.Size(78, 17);
            this.rbGerarNovaGrelha.TabIndex = 35;
            this.rbGerarNovaGrelha.TabStop = true;
            this.rbGerarNovaGrelha.Text = "Gerar nova";
            this.rbGerarNovaGrelha.UseVisualStyleBackColor = true;
            this.rbGerarNovaGrelha.CheckedChanged += new System.EventHandler(this.rbGerarNovaGrelha_CheckedChanged);
            // 
            // rbConsiderarGrelhaEditada
            // 
            this.rbConsiderarGrelhaEditada.AutoSize = true;
            this.rbConsiderarGrelhaEditada.Location = new System.Drawing.Point(133, 19);
            this.rbConsiderarGrelhaEditada.Name = "rbConsiderarGrelhaEditada";
            this.rbConsiderarGrelhaEditada.Size = new System.Drawing.Size(145, 17);
            this.rbConsiderarGrelhaEditada.TabIndex = 36;
            this.rbConsiderarGrelhaEditada.Text = "Considerar grelha editada";
            this.rbConsiderarGrelhaEditada.UseVisualStyleBackColor = true;
            this.rbConsiderarGrelhaEditada.CheckedChanged += new System.EventHandler(this.rbConsiderarGrelhaEditada_CheckedChanged);
            // 
            // FGrelhaConfiguracao
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(462, 436);
            this.Controls.Add(this.labelPiso);
            this.Controls.Add(this.groupBox3);
            this.Controls.Add(this.gbDiscretizacao);
            this.Controls.Add(this.groupBox2);
            this.Controls.Add(this.panel1);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedToolWindow;
            this.Name = "FGrelhaConfiguracao";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "PGi - Configuração de Grelha por Pisos";
            this.TopMost = true;
            this.Load += new System.EventHandler(this.FGrelhaConfiguracao_Load);
            this.panel1.ResumeLayout(false);
            this.groupBox2.ResumeLayout(false);
            this.groupBox3.ResumeLayout(false);
            this.groupBox3.PerformLayout();
            this.gbDiscretizacao.ResumeLayout(false);
            this.gbDiscretizacao.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.edEspacamentoY)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.edEspacamentoX)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.edAnguloBarras)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion
        private System.Windows.Forms.Panel panel1;
        private System.Windows.Forms.Button button3;
        private System.Windows.Forms.Button button1;
        private System.Windows.Forms.GroupBox groupBox2;
        private System.Windows.Forms.ListBox lbPavimentos;
        private System.Windows.Forms.GroupBox groupBox3;
        private System.Windows.Forms.GroupBox gbDiscretizacao;
        private System.Windows.Forms.Button button2;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.NumericUpDown edEspacamentoY;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.NumericUpDown edEspacamentoX;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.NumericUpDown edAnguloBarras;
        private System.Windows.Forms.Label labelPiso;
        private System.Windows.Forms.RadioButton rbConsiderarGrelhaEditada;
        private System.Windows.Forms.RadioButton rbGerarNovaGrelha;
    }
}