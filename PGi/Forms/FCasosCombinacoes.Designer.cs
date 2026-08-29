namespace PG
{ 
    partial class FCasosCombinacoes
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(FCasosCombinacoes));
            this.imgTemaEscuro_18x18 = new System.Windows.Forms.ImageList(this.components);
            this.imgLigaDesliga19x19 = new System.Windows.Forms.ImageList(this.components);
            this.label13 = new System.Windows.Forms.Label();
            this.cbCasoCarga = new System.Windows.Forms.ComboBox();
            this.btVisualizarCargas = new System.Windows.Forms.Button();
            this.button2 = new System.Windows.Forms.Button();
            this.label21 = new System.Windows.Forms.Label();
            this.cbCombinacao = new System.Windows.Forms.ComboBox();
            this.SuspendLayout();
            // 
            // imgTemaEscuro_18x18
            // 
            this.imgTemaEscuro_18x18.ImageStream = ((System.Windows.Forms.ImageListStreamer)(resources.GetObject("imgTemaEscuro_18x18.ImageStream")));
            this.imgTemaEscuro_18x18.TransparentColor = System.Drawing.Color.Transparent;
            this.imgTemaEscuro_18x18.Images.SetKeyName(0, "redo.png");
            this.imgTemaEscuro_18x18.Images.SetKeyName(1, "undo.png");
            this.imgTemaEscuro_18x18.Images.SetKeyName(2, "copiar.png");
            this.imgTemaEscuro_18x18.Images.SetKeyName(3, "espelhar.png");
            this.imgTemaEscuro_18x18.Images.SetKeyName(4, "rotacionar.png");
            this.imgTemaEscuro_18x18.Images.SetKeyName(5, "mover.png");
            this.imgTemaEscuro_18x18.Images.SetKeyName(6, "mover extremo.png");
            this.imgTemaEscuro_18x18.Images.SetKeyName(7, "fator.bmp");
            this.imgTemaEscuro_18x18.Images.SetKeyName(8, "cota on.bmp");
            this.imgTemaEscuro_18x18.Images.SetKeyName(9, "cota on.bmp");
            this.imgTemaEscuro_18x18.Images.SetKeyName(10, "snap.bmp");
            this.imgTemaEscuro_18x18.Images.SetKeyName(11, "snap cfg.bmp");
            // 
            // imgLigaDesliga19x19
            // 
            this.imgLigaDesliga19x19.ImageStream = ((System.Windows.Forms.ImageListStreamer)(resources.GetObject("imgLigaDesliga19x19.ImageStream")));
            this.imgLigaDesliga19x19.TransparentColor = System.Drawing.Color.Transparent;
            this.imgLigaDesliga19x19.Images.SetKeyName(0, "nao mostra cargas.bmp");
            // 
            // label13
            // 
            this.label13.AutoSize = true;
            this.label13.ForeColor = System.Drawing.SystemColors.ActiveBorder;
            this.label13.Location = new System.Drawing.Point(9, 10);
            this.label13.Name = "label13";
            this.label13.Size = new System.Drawing.Size(81, 13);
            this.label13.TabIndex = 163;
            this.label13.Text = "Casos de carga";
            this.label13.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // cbCasoCarga
            // 
            this.cbCasoCarga.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cbCasoCarga.FormattingEnabled = true;
            this.cbCasoCarga.Location = new System.Drawing.Point(91, 2);
            this.cbCasoCarga.Name = "cbCasoCarga";
            this.cbCasoCarga.Size = new System.Drawing.Size(283, 21);
            this.cbCasoCarga.TabIndex = 162;
            // 
            // btVisualizarCargas
            // 
            this.btVisualizarCargas.AccessibleName = "Visualizar as cargas";
            this.btVisualizarCargas.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(64)))), ((int)(((byte)(64)))), ((int)(((byte)(64)))));
            this.btVisualizarCargas.FlatAppearance.BorderColor = System.Drawing.Color.Cyan;
            this.btVisualizarCargas.FlatAppearance.MouseDownBackColor = System.Drawing.Color.White;
            this.btVisualizarCargas.FlatAppearance.MouseOverBackColor = System.Drawing.Color.Silver;
            this.btVisualizarCargas.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btVisualizarCargas.ImageIndex = 0;
            this.btVisualizarCargas.ImageList = this.imgLigaDesliga19x19;
            this.btVisualizarCargas.Location = new System.Drawing.Point(380, 0);
            this.btVisualizarCargas.Name = "btVisualizarCargas";
            this.btVisualizarCargas.Size = new System.Drawing.Size(25, 25);
            this.btVisualizarCargas.TabIndex = 165;
            this.btVisualizarCargas.Tag = "0";
            this.btVisualizarCargas.UseVisualStyleBackColor = false;
            // 
            // button2
            // 
            this.button2.AccessibleName = "Escala das cargas";
            this.button2.FlatAppearance.BorderColor = System.Drawing.Color.FromArgb(((int)(((byte)(64)))), ((int)(((byte)(64)))), ((int)(((byte)(64)))));
            this.button2.FlatAppearance.MouseDownBackColor = System.Drawing.Color.Transparent;
            this.button2.FlatAppearance.MouseOverBackColor = System.Drawing.Color.Silver;
            this.button2.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.button2.ImageIndex = 7;
            this.button2.ImageList = this.imgTemaEscuro_18x18;
            this.button2.Location = new System.Drawing.Point(408, 0);
            this.button2.Name = "button2";
            this.button2.Size = new System.Drawing.Size(25, 25);
            this.button2.TabIndex = 164;
            this.button2.UseVisualStyleBackColor = true;
            // 
            // label21
            // 
            this.label21.AutoSize = true;
            this.label21.ForeColor = System.Drawing.SystemColors.ActiveBorder;
            this.label21.Location = new System.Drawing.Point(439, 10);
            this.label21.Name = "label21";
            this.label21.Size = new System.Drawing.Size(66, 13);
            this.label21.TabIndex = 167;
            this.label21.Text = "Combinação";
            this.label21.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // cbCombinacao
            // 
            this.cbCombinacao.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cbCombinacao.FormattingEnabled = true;
            this.cbCombinacao.Location = new System.Drawing.Point(507, 2);
            this.cbCombinacao.Name = "cbCombinacao";
            this.cbCombinacao.Size = new System.Drawing.Size(297, 21);
            this.cbCombinacao.TabIndex = 166;
            // 
            // FCasosCombinacoes
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(64)))), ((int)(((byte)(64)))), ((int)(((byte)(64)))));
            this.ClientSize = new System.Drawing.Size(437, 35);
            this.Controls.Add(this.label21);
            this.Controls.Add(this.cbCombinacao);
            this.Controls.Add(this.btVisualizarCargas);
            this.Controls.Add(this.button2);
            this.Controls.Add(this.label13);
            this.Controls.Add(this.cbCasoCarga);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.None;
            this.Name = "FCasosCombinacoes";
            this.Text = "FCasosCombinacoes";
            this.TopMost = true;
            this.Load += new System.EventHandler(this.FCasosCombinacoes_Load);
            this.Shown += new System.EventHandler(this.FCasosCombinacoes_Shown);
            this.Click += new System.EventHandler(this.FCasosCombinacoes_Click);
            this.MouseClick += new System.Windows.Forms.MouseEventHandler(this.FCasosCombinacoes_MouseClick);
            this.MouseDown += new System.Windows.Forms.MouseEventHandler(this.FCasosCombinacoes_MouseDown);
            this.MouseMove += new System.Windows.Forms.MouseEventHandler(this.FCasosCombinacoes_MouseMove);
            this.MouseUp += new System.Windows.Forms.MouseEventHandler(this.FCasosCombinacoes_MouseUp);
            this.Move += new System.EventHandler(this.FCasosCombinacoes_Move);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.ImageList imgTemaEscuro_18x18;
        private System.Windows.Forms.ImageList imgLigaDesliga19x19;
        private System.Windows.Forms.Label label13;
        public System.Windows.Forms.ComboBox cbCasoCarga;
        private System.Windows.Forms.Button btVisualizarCargas;
        private System.Windows.Forms.Button button2;
        private System.Windows.Forms.Label label21;
        public System.Windows.Forms.ComboBox cbCombinacao;
    }
}