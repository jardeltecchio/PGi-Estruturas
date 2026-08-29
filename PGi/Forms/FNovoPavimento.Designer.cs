namespace PG
{
    partial class FNovoPavimento
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
            this.button7 = new System.Windows.Forms.Button();
            this.button6 = new System.Windows.Forms.Button();
            this.tbPavimentos = new System.Windows.Forms.TabControl();
            this.tabPage1 = new System.Windows.Forms.TabPage();
            this.edRepeticoes = new System.Windows.Forms.NumericUpDown();
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.label6 = new System.Windows.Forms.Label();
            this.edCotaFundacao = new System.Windows.Forms.TextBox();
            this.lbSequencia = new System.Windows.Forms.ListBox();
            this.label8 = new System.Windows.Forms.Label();
            this.Altura = new System.Windows.Forms.Label();
            this.label7 = new System.Windows.Forms.Label();
            this.lbNivel = new System.Windows.Forms.ListBox();
            this.lbPeDireito = new System.Windows.Forms.ListBox();
            this.edPeDireito = new System.Windows.Forms.TextBox();
            this.btGravar = new System.Windows.Forms.Button();
            this.label3 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.label1 = new System.Windows.Forms.Label();
            this.edNome = new System.Windows.Forms.TextBox();
            this.lb = new System.Windows.Forms.ListBox();
            this.btMoverBaixo = new System.Windows.Forms.Button();
            this.btMoverCima = new System.Windows.Forms.Button();
            this.btExcluir = new System.Windows.Forms.Button();
            this.btInsereAbaixo = new System.Windows.Forms.Button();
            this.btInsereAcima = new System.Windows.Forms.Button();
            this.panel2 = new System.Windows.Forms.Panel();
            this.label4 = new System.Windows.Forms.Label();
            this.edNomeProjeto = new System.Windows.Forms.TextBox();
            this.edDescricaoProjeto = new System.Windows.Forms.TextBox();
            this.label5 = new System.Windows.Forms.Label();
            this.panel1.SuspendLayout();
            this.tbPavimentos.SuspendLayout();
            this.tabPage1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.edRepeticoes)).BeginInit();
            this.groupBox1.SuspendLayout();
            this.SuspendLayout();
            // 
            // panel1
            // 
            this.panel1.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.panel1.Controls.Add(this.button7);
            this.panel1.Controls.Add(this.button6);
            this.panel1.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.panel1.Location = new System.Drawing.Point(0, 621);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(513, 32);
            this.panel1.TabIndex = 9;
            // 
            // button7
            // 
            this.button7.BackColor = System.Drawing.Color.Transparent;
            this.button7.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.button7.Location = new System.Drawing.Point(403, 4);
            this.button7.Name = "button7";
            this.button7.Size = new System.Drawing.Size(100, 23);
            this.button7.TabIndex = 7;
            this.button7.Text = "Cancelar";
            this.button7.UseVisualStyleBackColor = false;
            this.button7.Click += new System.EventHandler(this.button7_Click);
            // 
            // button6
            // 
            this.button6.BackColor = System.Drawing.Color.Transparent;
            this.button6.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.button6.Location = new System.Drawing.Point(3, 4);
            this.button6.Name = "button6";
            this.button6.Size = new System.Drawing.Size(100, 23);
            this.button6.TabIndex = 6;
            this.button6.Text = "Ok";
            this.button6.UseVisualStyleBackColor = false;
            this.button6.Click += new System.EventHandler(this.button6_Click);
            // 
            // tbPavimentos
            // 
            this.tbPavimentos.Controls.Add(this.tabPage1);
            this.tbPavimentos.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.tbPavimentos.Location = new System.Drawing.Point(0, 152);
            this.tbPavimentos.Name = "tbPavimentos";
            this.tbPavimentos.SelectedIndex = 0;
            this.tbPavimentos.Size = new System.Drawing.Size(513, 469);
            this.tbPavimentos.TabIndex = 2;
            // 
            // tabPage1
            // 
            this.tabPage1.Controls.Add(this.edRepeticoes);
            this.tabPage1.Controls.Add(this.groupBox1);
            this.tabPage1.Controls.Add(this.lbSequencia);
            this.tabPage1.Controls.Add(this.label8);
            this.tabPage1.Controls.Add(this.Altura);
            this.tabPage1.Controls.Add(this.label7);
            this.tabPage1.Controls.Add(this.lbNivel);
            this.tabPage1.Controls.Add(this.lbPeDireito);
            this.tabPage1.Controls.Add(this.edPeDireito);
            this.tabPage1.Controls.Add(this.btGravar);
            this.tabPage1.Controls.Add(this.label3);
            this.tabPage1.Controls.Add(this.label2);
            this.tabPage1.Controls.Add(this.label1);
            this.tabPage1.Controls.Add(this.edNome);
            this.tabPage1.Controls.Add(this.lb);
            this.tabPage1.Controls.Add(this.btMoverBaixo);
            this.tabPage1.Controls.Add(this.btMoverCima);
            this.tabPage1.Controls.Add(this.btExcluir);
            this.tabPage1.Controls.Add(this.btInsereAbaixo);
            this.tabPage1.Controls.Add(this.btInsereAcima);
            this.tabPage1.Controls.Add(this.panel2);
            this.tabPage1.Location = new System.Drawing.Point(4, 22);
            this.tabPage1.Name = "tabPage1";
            this.tabPage1.Padding = new System.Windows.Forms.Padding(3);
            this.tabPage1.Size = new System.Drawing.Size(505, 443);
            this.tabPage1.TabIndex = 0;
            this.tabPage1.Text = "Pavimentos";
            this.tabPage1.UseVisualStyleBackColor = true;
            // 
            // edRepeticoes
            // 
            this.edRepeticoes.Location = new System.Drawing.Point(111, 54);
            this.edRepeticoes.Maximum = new decimal(new int[] {
            10,
            0,
            0,
            0});
            this.edRepeticoes.Minimum = new decimal(new int[] {
            1,
            0,
            0,
            0});
            this.edRepeticoes.Name = "edRepeticoes";
            this.edRepeticoes.Size = new System.Drawing.Size(54, 20);
            this.edRepeticoes.TabIndex = 40;
            this.edRepeticoes.Value = new decimal(new int[] {
            1,
            0,
            0,
            0});
            // 
            // groupBox1
            // 
            this.groupBox1.Controls.Add(this.label6);
            this.groupBox1.Controls.Add(this.edCotaFundacao);
            this.groupBox1.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.groupBox1.Location = new System.Drawing.Point(111, 389);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(125, 48);
            this.groupBox1.TabIndex = 39;
            this.groupBox1.TabStop = false;
            this.groupBox1.Text = "Cota da fundação";
            this.groupBox1.Enter += new System.EventHandler(this.groupBox1_Enter);
            // 
            // label6
            // 
            this.label6.AutoSize = true;
            this.label6.Location = new System.Drawing.Point(67, 26);
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size(23, 13);
            this.label6.TabIndex = 40;
            this.label6.Text = "cm";
            // 
            // edCotaFundacao
            // 
            this.edCotaFundacao.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.edCotaFundacao.Location = new System.Drawing.Point(6, 19);
            this.edCotaFundacao.Name = "edCotaFundacao";
            this.edCotaFundacao.Size = new System.Drawing.Size(55, 20);
            this.edCotaFundacao.TabIndex = 39;
            this.edCotaFundacao.Text = "0";
            this.edCotaFundacao.KeyPress += new System.Windows.Forms.KeyPressEventHandler(this.edCotaFundacao_KeyPress);
            this.edCotaFundacao.Leave += new System.EventHandler(this.edCotaFundacao_Leave);
            // 
            // lbSequencia
            // 
            this.lbSequencia.FormattingEnabled = true;
            this.lbSequencia.Location = new System.Drawing.Point(46, 105);
            this.lbSequencia.Name = "lbSequencia";
            this.lbSequencia.Size = new System.Drawing.Size(47, 303);
            this.lbSequencia.TabIndex = 37;
            this.lbSequencia.Visible = false;
            this.lbSequencia.Click += new System.EventHandler(this.lbSequencia_Click);
            this.lbSequencia.SelectedIndexChanged += new System.EventHandler(this.lb_SelectedIndexChanged);
            // 
            // label8
            // 
            this.label8.AutoSize = true;
            this.label8.BackColor = System.Drawing.Color.Gainsboro;
            this.label8.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label8.Location = new System.Drawing.Point(334, 89);
            this.label8.Name = "label8";
            this.label8.Size = new System.Drawing.Size(33, 13);
            this.label8.TabIndex = 35;
            this.label8.Text = "Cota";
            // 
            // Altura
            // 
            this.Altura.AutoSize = true;
            this.Altura.BackColor = System.Drawing.Color.Gainsboro;
            this.Altura.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.Altura.Location = new System.Drawing.Point(268, 89);
            this.Altura.Name = "Altura";
            this.Altura.Size = new System.Drawing.Size(40, 13);
            this.Altura.TabIndex = 34;
            this.Altura.Text = "Altura";
            // 
            // label7
            // 
            this.label7.BackColor = System.Drawing.Color.Gainsboro;
            this.label7.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label7.Location = new System.Drawing.Point(112, 89);
            this.label7.Name = "label7";
            this.label7.Size = new System.Drawing.Size(280, 13);
            this.label7.TabIndex = 33;
            this.label7.Text = "Nome";
            // 
            // lbNivel
            // 
            this.lbNivel.FormattingEnabled = true;
            this.lbNivel.Location = new System.Drawing.Point(336, 105);
            this.lbNivel.Name = "lbNivel";
            this.lbNivel.Size = new System.Drawing.Size(56, 277);
            this.lbNivel.TabIndex = 32;
            this.lbNivel.Click += new System.EventHandler(this.lbNivel_Click);
            this.lbNivel.SelectedIndexChanged += new System.EventHandler(this.lb_SelectedIndexChanged);
            this.lbNivel.RightToLeftChanged += new System.EventHandler(this.lbNivel_RightToLeftChanged);
            this.lbNivel.MouseDoubleClick += new System.Windows.Forms.MouseEventHandler(this.lbNivel_MouseDoubleClick);
            // 
            // lbPeDireito
            // 
            this.lbPeDireito.FormattingEnabled = true;
            this.lbPeDireito.Location = new System.Drawing.Point(273, 105);
            this.lbPeDireito.Name = "lbPeDireito";
            this.lbPeDireito.Size = new System.Drawing.Size(62, 277);
            this.lbPeDireito.TabIndex = 31;
            this.lbPeDireito.Click += new System.EventHandler(this.lbPeDireito_Click);
            this.lbPeDireito.SelectedIndexChanged += new System.EventHandler(this.lb_SelectedIndexChanged);
            this.lbPeDireito.MouseDoubleClick += new System.Windows.Forms.MouseEventHandler(this.lbPeDireito_MouseDoubleClick);
            // 
            // edPeDireito
            // 
            this.edPeDireito.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.edPeDireito.Location = new System.Drawing.Point(111, 30);
            this.edPeDireito.Name = "edPeDireito";
            this.edPeDireito.Size = new System.Drawing.Size(55, 20);
            this.edPeDireito.TabIndex = 18;
            this.edPeDireito.Text = "300";
            this.edPeDireito.KeyDown += new System.Windows.Forms.KeyEventHandler(this.edPeDireito_KeyDown);
            this.edPeDireito.KeyPress += new System.Windows.Forms.KeyPressEventHandler(this.edPeDireito_KeyPress);
            // 
            // btGravar
            // 
            this.btGravar.BackColor = System.Drawing.Color.Transparent;
            this.btGravar.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.btGravar.Location = new System.Drawing.Point(170, 51);
            this.btGravar.Name = "btGravar";
            this.btGravar.Size = new System.Drawing.Size(83, 24);
            this.btGravar.TabIndex = 20;
            this.btGravar.Text = "Gravar";
            this.btGravar.UseVisualStyleBackColor = false;
            this.btGravar.Click += new System.EventHandler(this.btGravar_Click);
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(21, 61);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(84, 13);
            this.label3.TabIndex = 29;
            this.label3.Text = "Qtd. Repetições";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(17, 13);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(88, 13);
            this.label2.TabIndex = 28;
            this.label2.Text = "Nome Pavimento";
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(43, 37);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(57, 13);
            this.label1.TabIndex = 27;
            this.label1.Text = "Altura (cm)";
            // 
            // edNome
            // 
            this.edNome.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.edNome.Location = new System.Drawing.Point(111, 6);
            this.edNome.Name = "edNome";
            this.edNome.Size = new System.Drawing.Size(159, 20);
            this.edNome.TabIndex = 17;
            this.edNome.KeyDown += new System.Windows.Forms.KeyEventHandler(this.edNome_KeyDown);
            this.edNome.KeyPress += new System.Windows.Forms.KeyPressEventHandler(this.edNome_KeyPress);
            // 
            // lb
            // 
            this.lb.FormattingEnabled = true;
            this.lb.HorizontalScrollbar = true;
            this.lb.Location = new System.Drawing.Point(113, 105);
            this.lb.Name = "lb";
            this.lb.Size = new System.Drawing.Size(159, 277);
            this.lb.TabIndex = 26;
            this.lb.Click += new System.EventHandler(this.lb_Click);
            this.lb.SelectedIndexChanged += new System.EventHandler(this.lb_SelectedIndexChanged);
            this.lb.MouseDoubleClick += new System.Windows.Forms.MouseEventHandler(this.lb_MouseDoubleClick);
            // 
            // btMoverBaixo
            // 
            this.btMoverBaixo.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.btMoverBaixo.Location = new System.Drawing.Point(397, 212);
            this.btMoverBaixo.Name = "btMoverBaixo";
            this.btMoverBaixo.Size = new System.Drawing.Size(94, 25);
            this.btMoverBaixo.TabIndex = 25;
            this.btMoverBaixo.Text = "Mover p/ baixo";
            this.btMoverBaixo.UseVisualStyleBackColor = true;
            this.btMoverBaixo.Click += new System.EventHandler(this.btMoverBaixo_Click);
            // 
            // btMoverCima
            // 
            this.btMoverCima.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.btMoverCima.Location = new System.Drawing.Point(397, 182);
            this.btMoverCima.Name = "btMoverCima";
            this.btMoverCima.Size = new System.Drawing.Size(94, 25);
            this.btMoverCima.TabIndex = 24;
            this.btMoverCima.Text = "Mover p/ cima";
            this.btMoverCima.UseVisualStyleBackColor = true;
            this.btMoverCima.Click += new System.EventHandler(this.btMoverCima_Click);
            // 
            // btExcluir
            // 
            this.btExcluir.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.btExcluir.Location = new System.Drawing.Point(397, 256);
            this.btExcluir.Name = "btExcluir";
            this.btExcluir.Size = new System.Drawing.Size(94, 25);
            this.btExcluir.TabIndex = 27;
            this.btExcluir.Text = "Excluir";
            this.btExcluir.UseVisualStyleBackColor = true;
            this.btExcluir.Click += new System.EventHandler(this.btExcluir_Click);
            // 
            // btInsereAbaixo
            // 
            this.btInsereAbaixo.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.btInsereAbaixo.Location = new System.Drawing.Point(397, 135);
            this.btInsereAbaixo.Name = "btInsereAbaixo";
            this.btInsereAbaixo.Size = new System.Drawing.Size(94, 25);
            this.btInsereAbaixo.TabIndex = 22;
            this.btInsereAbaixo.Text = "Inserir abaixo";
            this.btInsereAbaixo.UseVisualStyleBackColor = true;
            this.btInsereAbaixo.Click += new System.EventHandler(this.btInsereAbaixo_Click);
            // 
            // btInsereAcima
            // 
            this.btInsereAcima.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.btInsereAcima.Location = new System.Drawing.Point(397, 105);
            this.btInsereAcima.Name = "btInsereAcima";
            this.btInsereAcima.Size = new System.Drawing.Size(94, 25);
            this.btInsereAcima.TabIndex = 0;
            this.btInsereAcima.Text = "Inserir acima";
            this.btInsereAcima.UseVisualStyleBackColor = true;
            this.btInsereAcima.Click += new System.EventHandler(this.btInsereAcima_Click);
            // 
            // panel2
            // 
            this.panel2.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.panel2.Location = new System.Drawing.Point(111, 87);
            this.panel2.Name = "panel2";
            this.panel2.Size = new System.Drawing.Size(283, 297);
            this.panel2.TabIndex = 36;
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Font = new System.Drawing.Font("Tahoma", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label4.Location = new System.Drawing.Point(4, 9);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(39, 14);
            this.label4.TabIndex = 33;
            this.label4.Text = "Nome";
            // 
            // edNomeProjeto
            // 
            this.edNomeProjeto.BackColor = System.Drawing.SystemColors.InactiveCaption;
            this.edNomeProjeto.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.edNomeProjeto.Font = new System.Drawing.Font("Tahoma", 10F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.edNomeProjeto.Location = new System.Drawing.Point(7, 26);
            this.edNomeProjeto.Name = "edNomeProjeto";
            this.edNomeProjeto.Size = new System.Drawing.Size(477, 24);
            this.edNomeProjeto.TabIndex = 0;
            this.edNomeProjeto.KeyDown += new System.Windows.Forms.KeyEventHandler(this.edNomeProjeto_KeyDown);
            this.edNomeProjeto.KeyPress += new System.Windows.Forms.KeyPressEventHandler(this.edNomeProjeto_KeyPress);
            // 
            // edDescricaoProjeto
            // 
            this.edDescricaoProjeto.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.edDescricaoProjeto.Font = new System.Drawing.Font("Tahoma", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.edDescricaoProjeto.Location = new System.Drawing.Point(7, 72);
            this.edDescricaoProjeto.Multiline = true;
            this.edDescricaoProjeto.Name = "edDescricaoProjeto";
            this.edDescricaoProjeto.ScrollBars = System.Windows.Forms.ScrollBars.Both;
            this.edDescricaoProjeto.Size = new System.Drawing.Size(494, 72);
            this.edDescricaoProjeto.TabIndex = 1;
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.Font = new System.Drawing.Font("Tahoma", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label5.Location = new System.Drawing.Point(4, 56);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(126, 14);
            this.label5.TabIndex = 35;
            this.label5.Text = "Descrição/Observação";
            // 
            // FNovoPavimento
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(513, 653);
            this.Controls.Add(this.label5);
            this.Controls.Add(this.edDescricaoProjeto);
            this.Controls.Add(this.label4);
            this.Controls.Add(this.edNomeProjeto);
            this.Controls.Add(this.tbPavimentos);
            this.Controls.Add(this.panel1);
            this.Cursor = System.Windows.Forms.Cursors.Arrow;
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedToolWindow;
            this.KeyPreview = true;
            this.Name = "FNovoPavimento";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "PGi - Arquivo";
            this.TopMost = true;
            this.Shown += new System.EventHandler(this.FNovoPavimento_Shown);
            this.Move += new System.EventHandler(this.FNovoPavimento_Move);
            this.panel1.ResumeLayout(false);
            this.tbPavimentos.ResumeLayout(false);
            this.tabPage1.ResumeLayout(false);
            this.tabPage1.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.edRepeticoes)).EndInit();
            this.groupBox1.ResumeLayout(false);
            this.groupBox1.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Panel panel1;
        private System.Windows.Forms.Button button7;
        private System.Windows.Forms.Button button6;
        private System.Windows.Forms.TabControl tbPavimentos;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.TextBox edNomeProjeto;
        private System.Windows.Forms.TextBox edDescricaoProjeto;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.TabPage tabPage1;
        private System.Windows.Forms.Label label8;
        private System.Windows.Forms.Label Altura;
        private System.Windows.Forms.Label label7;
        private System.Windows.Forms.ListBox lbNivel;
        private System.Windows.Forms.ListBox lbPeDireito;
        private System.Windows.Forms.TextBox edPeDireito;
        private System.Windows.Forms.Button btGravar;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.TextBox edNome;
        private System.Windows.Forms.ListBox lb;
        private System.Windows.Forms.Button btMoverBaixo;
        private System.Windows.Forms.Button btMoverCima;
        private System.Windows.Forms.Button btExcluir;
        private System.Windows.Forms.Button btInsereAbaixo;
        private System.Windows.Forms.Button btInsereAcima;
        private System.Windows.Forms.Panel panel2;
        private System.Windows.Forms.ListBox lbSequencia;
        private System.Windows.Forms.GroupBox groupBox1;
        private System.Windows.Forms.TextBox edCotaFundacao;
        private System.Windows.Forms.Label label6;
        private System.Windows.Forms.NumericUpDown edRepeticoes;

    }
}