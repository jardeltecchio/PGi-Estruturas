namespace PG
{
    partial class FProcessoCalculo
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
            this.PanelCalculo = new System.Windows.Forms.Panel();
            this.panel4 = new System.Windows.Forms.Panel();
            this.LabelProcesso = new System.Windows.Forms.Label();
            this.PanelTitulo = new System.Windows.Forms.Panel();
            this.btInterromper = new System.Windows.Forms.Button();
            this.Progresso = new System.Windows.Forms.ProgressBar();
            this.lbErro = new System.Windows.Forms.Label();
            this.ListaCalculo = new System.Windows.Forms.ListBox();
            this.panel2 = new System.Windows.Forms.Panel();
            this.label1 = new System.Windows.Forms.Label();
            this.pnMatrizRigidez = new System.Windows.Forms.Panel();
            this.PanelCalculo.SuspendLayout();
            this.panel4.SuspendLayout();
            this.PanelTitulo.SuspendLayout();
            this.pnMatrizRigidez.SuspendLayout();
            this.SuspendLayout();
            // 
            // PanelCalculo
            // 
            this.PanelCalculo.BackColor = System.Drawing.SystemColors.Control;
            this.PanelCalculo.Controls.Add(this.panel4);
            this.PanelCalculo.Dock = System.Windows.Forms.DockStyle.Fill;
            this.PanelCalculo.Location = new System.Drawing.Point(0, 0);
            this.PanelCalculo.Name = "PanelCalculo";
            this.PanelCalculo.Size = new System.Drawing.Size(613, 290);
            this.PanelCalculo.TabIndex = 94;
            // 
            // panel4
            // 
            this.panel4.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(64)))), ((int)(((byte)(64)))), ((int)(((byte)(64)))));
            this.panel4.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.panel4.Controls.Add(this.LabelProcesso);
            this.panel4.Controls.Add(this.PanelTitulo);
            this.panel4.Controls.Add(this.ListaCalculo);
            this.panel4.Controls.Add(this.panel2);
            this.panel4.Dock = System.Windows.Forms.DockStyle.Fill;
            this.panel4.ForeColor = System.Drawing.SystemColors.ActiveCaptionText;
            this.panel4.Location = new System.Drawing.Point(0, 0);
            this.panel4.Name = "panel4";
            this.panel4.Size = new System.Drawing.Size(613, 290);
            this.panel4.TabIndex = 37;
            // 
            // LabelProcesso
            // 
            this.LabelProcesso.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(64)))), ((int)(((byte)(64)))), ((int)(((byte)(64)))));
            this.LabelProcesso.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.LabelProcesso.Font = new System.Drawing.Font("Tahoma", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.LabelProcesso.ForeColor = System.Drawing.Color.Red;
            this.LabelProcesso.Location = new System.Drawing.Point(0, 228);
            this.LabelProcesso.Name = "LabelProcesso";
            this.LabelProcesso.Size = new System.Drawing.Size(611, 35);
            this.LabelProcesso.TabIndex = 144;
            this.LabelProcesso.Text = "Erro";
            this.LabelProcesso.Visible = false;
            // 
            // PanelTitulo
            // 
            this.PanelTitulo.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(64)))), ((int)(((byte)(64)))), ((int)(((byte)(64)))));
            this.PanelTitulo.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.PanelTitulo.Controls.Add(this.btInterromper);
            this.PanelTitulo.Controls.Add(this.Progresso);
            this.PanelTitulo.Controls.Add(this.lbErro);
            this.PanelTitulo.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.PanelTitulo.Location = new System.Drawing.Point(0, 263);
            this.PanelTitulo.Name = "PanelTitulo";
            this.PanelTitulo.Size = new System.Drawing.Size(611, 25);
            this.PanelTitulo.TabIndex = 143;
            // 
            // btInterromper
            // 
            this.btInterromper.BackColor = System.Drawing.SystemColors.Control;
            this.btInterromper.Dock = System.Windows.Forms.DockStyle.Right;
            this.btInterromper.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btInterromper.Font = new System.Drawing.Font("Tahoma", 8F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btInterromper.ForeColor = System.Drawing.Color.Red;
            this.btInterromper.Location = new System.Drawing.Point(511, 0);
            this.btInterromper.Name = "btInterromper";
            this.btInterromper.Size = new System.Drawing.Size(98, 23);
            this.btInterromper.TabIndex = 102;
            this.btInterromper.Text = "Interromper cálculo";
            this.btInterromper.UseVisualStyleBackColor = false;
            this.btInterromper.Visible = false;
            // 
            // Progresso
            // 
            this.Progresso.Location = new System.Drawing.Point(4, 5);
            this.Progresso.Name = "Progresso";
            this.Progresso.Size = new System.Drawing.Size(503, 12);
            this.Progresso.Style = System.Windows.Forms.ProgressBarStyle.Continuous;
            this.Progresso.TabIndex = 100;
            // 
            // lbErro
            // 
            this.lbErro.AutoSize = true;
            this.lbErro.Font = new System.Drawing.Font("Tahoma", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lbErro.ForeColor = System.Drawing.Color.Blue;
            this.lbErro.Location = new System.Drawing.Point(25, 3);
            this.lbErro.Name = "lbErro";
            this.lbErro.Size = new System.Drawing.Size(32, 14);
            this.lbErro.TabIndex = 99;
            this.lbErro.Text = "Erro";
            this.lbErro.Visible = false;
            // 
            // ListaCalculo
            // 
            this.ListaCalculo.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(64)))), ((int)(((byte)(64)))), ((int)(((byte)(64)))));
            this.ListaCalculo.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this.ListaCalculo.Dock = System.Windows.Forms.DockStyle.Top;
            this.ListaCalculo.Font = new System.Drawing.Font("Tahoma", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.ListaCalculo.ForeColor = System.Drawing.Color.White;
            this.ListaCalculo.FormattingEnabled = true;
            this.ListaCalculo.ItemHeight = 14;
            this.ListaCalculo.Location = new System.Drawing.Point(0, 0);
            this.ListaCalculo.Name = "ListaCalculo";
            this.ListaCalculo.Size = new System.Drawing.Size(611, 224);
            this.ListaCalculo.TabIndex = 134;
            // 
            // panel2
            // 
            this.panel2.Location = new System.Drawing.Point(266, 109);
            this.panel2.Name = "panel2";
            this.panel2.Size = new System.Drawing.Size(403, 58);
            this.panel2.TabIndex = 133;
            this.panel2.Visible = false;
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(61, 0);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(129, 13);
            this.label1.TabIndex = 137;
            this.label1.Text = "Perfil da Matriz de Rigidez";
            // 
            // pnMatrizRigidez
            // 
            this.pnMatrizRigidez.BackColor = System.Drawing.Color.White;
            this.pnMatrizRigidez.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.pnMatrizRigidez.Controls.Add(this.label1);
            this.pnMatrizRigidez.Location = new System.Drawing.Point(613, 1);
            this.pnMatrizRigidez.Name = "pnMatrizRigidez";
            this.pnMatrizRigidez.Size = new System.Drawing.Size(148, 191);
            this.pnMatrizRigidez.TabIndex = 138;
            this.pnMatrizRigidez.Paint += new System.Windows.Forms.PaintEventHandler(this.pnMatrizRigidez_Paint);
            // 
            // FProcessoCalculo
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(613, 290);
            this.Controls.Add(this.pnMatrizRigidez);
            this.Controls.Add(this.PanelCalculo);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedToolWindow;
            this.Name = "FProcessoCalculo";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "Cálculo";
            this.TopMost = true;
            this.FormClosed += new System.Windows.Forms.FormClosedEventHandler(this.FProcessoCalculo_FormClosed);
            this.Load += new System.EventHandler(this.FProcessoCalculo_Load);
            this.Shown += new System.EventHandler(this.FProcessoCalculo_Shown);
            this.PanelCalculo.ResumeLayout(false);
            this.panel4.ResumeLayout(false);
            this.PanelTitulo.ResumeLayout(false);
            this.PanelTitulo.PerformLayout();
            this.pnMatrizRigidez.ResumeLayout(false);
            this.pnMatrizRigidez.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        public System.Windows.Forms.Panel PanelCalculo;
        private System.Windows.Forms.Panel panel4;
        public System.Windows.Forms.ListBox ListaCalculo;
        private System.Windows.Forms.Panel panel2;
        private System.Windows.Forms.Label label1;
        public System.Windows.Forms.Panel pnMatrizRigidez;
        private System.Windows.Forms.Panel PanelTitulo;
        public System.Windows.Forms.Button btInterromper;
        public System.Windows.Forms.ProgressBar Progresso;
        private System.Windows.Forms.Label lbErro;
        public System.Windows.Forms.Label LabelProcesso;
    }
}