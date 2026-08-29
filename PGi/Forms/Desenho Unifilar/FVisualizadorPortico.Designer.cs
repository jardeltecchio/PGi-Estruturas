namespace PG
{
    partial class FVisualizadorPortico
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
            this.contextMenuStrip1 = new System.Windows.Forms.ContextMenuStrip(this.components);
            this.cancelarToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.animarToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.lbInfoBarra = new System.Windows.Forms.Label();
            this.lbInfoNo = new System.Windows.Forms.Label();
            this.PanelCores2 = new System.Windows.Forms.Panel();
            this.panel1 = new System.Windows.Forms.Panel();
            this.lbd0 = new System.Windows.Forms.Label();
            this.lbd10 = new System.Windows.Forms.Label();
            this.lbd9 = new System.Windows.Forms.Label();
            this.lbd8 = new System.Windows.Forms.Label();
            this.lbd7 = new System.Windows.Forms.Label();
            this.lbd6 = new System.Windows.Forms.Label();
            this.lbd5 = new System.Windows.Forms.Label();
            this.lbd4 = new System.Windows.Forms.Label();
            this.lbd3 = new System.Windows.Forms.Label();
            this.lbd2 = new System.Windows.Forms.Label();
            this.lbd1 = new System.Windows.Forms.Label();
            this.PanelCores = new System.Windows.Forms.Panel();
            this.lbNomeEsforco = new System.Windows.Forms.Label();
            this.pnd10 = new System.Windows.Forms.Button();
            this.pnd9 = new System.Windows.Forms.Button();
            this.pnd8 = new System.Windows.Forms.Button();
            this.pnd7 = new System.Windows.Forms.Button();
            this.pnd6 = new System.Windows.Forms.Button();
            this.pnd5 = new System.Windows.Forms.Button();
            this.pnd4 = new System.Windows.Forms.Button();
            this.pnd3 = new System.Windows.Forms.Button();
            this.pnd2 = new System.Windows.Forms.Button();
            this.pnd1 = new System.Windows.Forms.Button();
            this.timerVistas = new System.Windows.Forms.Timer(this.components);
            this.Controle = new OpenTK.GLControl();
            this.timer1 = new System.Windows.Forms.Timer(this.components);
            this.button1 = new System.Windows.Forms.Button();
            this.contextMenuStrip1.SuspendLayout();
            this.PanelCores2.SuspendLayout();
            this.panel1.SuspendLayout();
            this.PanelCores.SuspendLayout();
            this.SuspendLayout();
            // 
            // contextMenuStrip1
            // 
            this.contextMenuStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.cancelarToolStripMenuItem,
            this.animarToolStripMenuItem});
            this.contextMenuStrip1.Name = "contextMenuStrip1";
            this.contextMenuStrip1.Size = new System.Drawing.Size(123, 48);
            // 
            // cancelarToolStripMenuItem
            // 
            this.cancelarToolStripMenuItem.Name = "cancelarToolStripMenuItem";
            this.cancelarToolStripMenuItem.Size = new System.Drawing.Size(122, 22);
            this.cancelarToolStripMenuItem.Text = "Cancelar";
            this.cancelarToolStripMenuItem.Click += new System.EventHandler(this.cancelarToolStripMenuItem_Click);
            // 
            // animarToolStripMenuItem
            // 
            this.animarToolStripMenuItem.CheckOnClick = true;
            this.animarToolStripMenuItem.Name = "animarToolStripMenuItem";
            this.animarToolStripMenuItem.Size = new System.Drawing.Size(122, 22);
            this.animarToolStripMenuItem.Text = "Animar...";
            this.animarToolStripMenuItem.Click += new System.EventHandler(this.AnimarToolStripMenuItem_Click);
            // 
            // lbInfoBarra
            // 
            this.lbInfoBarra.AutoSize = true;
            this.lbInfoBarra.BackColor = System.Drawing.SystemColors.ActiveCaptionText;
            this.lbInfoBarra.Font = new System.Drawing.Font("Tahoma", 8F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lbInfoBarra.ForeColor = System.Drawing.SystemColors.ButtonHighlight;
            this.lbInfoBarra.Location = new System.Drawing.Point(288, 231);
            this.lbInfoBarra.Name = "lbInfoBarra";
            this.lbInfoBarra.Size = new System.Drawing.Size(51, 13);
            this.lbInfoBarra.TabIndex = 70;
            this.lbInfoBarra.Text = "infoBarra";
            this.lbInfoBarra.Visible = false;
            // 
            // lbInfoNo
            // 
            this.lbInfoNo.AutoSize = true;
            this.lbInfoNo.BackColor = System.Drawing.SystemColors.Info;
            this.lbInfoNo.Font = new System.Drawing.Font("Microsoft Sans Serif", 10F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lbInfoNo.Location = new System.Drawing.Point(546, 407);
            this.lbInfoNo.Name = "lbInfoNo";
            this.lbInfoNo.Size = new System.Drawing.Size(0, 17);
            this.lbInfoNo.TabIndex = 71;
            this.lbInfoNo.Visible = false;
            // 
            // PanelCores2
            // 
            this.PanelCores2.BackColor = System.Drawing.SystemColors.ActiveCaptionText;
            this.PanelCores2.Controls.Add(this.panel1);
            this.PanelCores2.Controls.Add(this.PanelCores);
            this.PanelCores2.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.PanelCores2.Location = new System.Drawing.Point(0, 438);
            this.PanelCores2.Name = "PanelCores2";
            this.PanelCores2.Size = new System.Drawing.Size(894, 36);
            this.PanelCores2.TabIndex = 72;
            this.PanelCores2.Visible = false;
            // 
            // panel1
            // 
            this.panel1.BackColor = System.Drawing.Color.Black;
            this.panel1.Controls.Add(this.lbd0);
            this.panel1.Controls.Add(this.lbd10);
            this.panel1.Controls.Add(this.lbd9);
            this.panel1.Controls.Add(this.lbd8);
            this.panel1.Controls.Add(this.lbd7);
            this.panel1.Controls.Add(this.lbd6);
            this.panel1.Controls.Add(this.lbd5);
            this.panel1.Controls.Add(this.lbd4);
            this.panel1.Controls.Add(this.lbd3);
            this.panel1.Controls.Add(this.lbd2);
            this.panel1.Controls.Add(this.lbd1);
            this.panel1.Location = new System.Drawing.Point(-27, 3);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(770, 15);
            this.panel1.TabIndex = 56;
            // 
            // lbd0
            // 
            this.lbd0.BackColor = System.Drawing.Color.Black;
            this.lbd0.Font = new System.Drawing.Font("Microsoft Sans Serif", 8F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lbd0.ForeColor = System.Drawing.SystemColors.ButtonFace;
            this.lbd0.Location = new System.Drawing.Point(31, 1);
            this.lbd0.Name = "lbd0";
            this.lbd0.Size = new System.Drawing.Size(60, 15);
            this.lbd0.TabIndex = 86;
            this.lbd0.Text = "-";
            this.lbd0.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // lbd10
            // 
            this.lbd10.BackColor = System.Drawing.Color.Black;
            this.lbd10.Font = new System.Drawing.Font("Microsoft Sans Serif", 8F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lbd10.ForeColor = System.Drawing.SystemColors.ButtonFace;
            this.lbd10.Location = new System.Drawing.Point(721, 0);
            this.lbd10.Name = "lbd10";
            this.lbd10.Size = new System.Drawing.Size(46, 15);
            this.lbd10.TabIndex = 85;
            this.lbd10.Text = "-";
            this.lbd10.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // lbd9
            // 
            this.lbd9.BackColor = System.Drawing.Color.Black;
            this.lbd9.Font = new System.Drawing.Font("Microsoft Sans Serif", 8F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lbd9.ForeColor = System.Drawing.SystemColors.ButtonFace;
            this.lbd9.Location = new System.Drawing.Point(655, 0);
            this.lbd9.Name = "lbd9";
            this.lbd9.Size = new System.Drawing.Size(60, 15);
            this.lbd9.TabIndex = 84;
            this.lbd9.Text = "-";
            this.lbd9.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // lbd8
            // 
            this.lbd8.BackColor = System.Drawing.Color.Black;
            this.lbd8.Font = new System.Drawing.Font("Microsoft Sans Serif", 8F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lbd8.ForeColor = System.Drawing.SystemColors.ButtonFace;
            this.lbd8.Location = new System.Drawing.Point(585, 0);
            this.lbd8.Name = "lbd8";
            this.lbd8.Size = new System.Drawing.Size(60, 15);
            this.lbd8.TabIndex = 83;
            this.lbd8.Text = "-";
            this.lbd8.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // lbd7
            // 
            this.lbd7.BackColor = System.Drawing.Color.Black;
            this.lbd7.Font = new System.Drawing.Font("Microsoft Sans Serif", 8F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lbd7.ForeColor = System.Drawing.SystemColors.ButtonFace;
            this.lbd7.Location = new System.Drawing.Point(515, 0);
            this.lbd7.Name = "lbd7";
            this.lbd7.Size = new System.Drawing.Size(60, 15);
            this.lbd7.TabIndex = 82;
            this.lbd7.Text = "-";
            this.lbd7.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // lbd6
            // 
            this.lbd6.BackColor = System.Drawing.Color.Black;
            this.lbd6.Font = new System.Drawing.Font("Microsoft Sans Serif", 8F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lbd6.ForeColor = System.Drawing.SystemColors.ButtonFace;
            this.lbd6.Location = new System.Drawing.Point(445, 0);
            this.lbd6.Name = "lbd6";
            this.lbd6.Size = new System.Drawing.Size(60, 15);
            this.lbd6.TabIndex = 81;
            this.lbd6.Text = "-";
            this.lbd6.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // lbd5
            // 
            this.lbd5.BackColor = System.Drawing.Color.Black;
            this.lbd5.Font = new System.Drawing.Font("Microsoft Sans Serif", 8F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lbd5.ForeColor = System.Drawing.SystemColors.ButtonFace;
            this.lbd5.Location = new System.Drawing.Point(375, 0);
            this.lbd5.Name = "lbd5";
            this.lbd5.Size = new System.Drawing.Size(60, 15);
            this.lbd5.TabIndex = 80;
            this.lbd5.Text = "-";
            this.lbd5.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // lbd4
            // 
            this.lbd4.BackColor = System.Drawing.Color.Black;
            this.lbd4.Font = new System.Drawing.Font("Microsoft Sans Serif", 8F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lbd4.ForeColor = System.Drawing.SystemColors.ButtonFace;
            this.lbd4.Location = new System.Drawing.Point(304, 0);
            this.lbd4.Name = "lbd4";
            this.lbd4.Size = new System.Drawing.Size(60, 15);
            this.lbd4.TabIndex = 79;
            this.lbd4.Text = "-";
            this.lbd4.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // lbd3
            // 
            this.lbd3.BackColor = System.Drawing.Color.Black;
            this.lbd3.Font = new System.Drawing.Font("Microsoft Sans Serif", 8F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lbd3.ForeColor = System.Drawing.SystemColors.ButtonFace;
            this.lbd3.Location = new System.Drawing.Point(236, 0);
            this.lbd3.Name = "lbd3";
            this.lbd3.Size = new System.Drawing.Size(60, 15);
            this.lbd3.TabIndex = 78;
            this.lbd3.Text = "-";
            this.lbd3.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // lbd2
            // 
            this.lbd2.BackColor = System.Drawing.Color.Black;
            this.lbd2.Font = new System.Drawing.Font("Microsoft Sans Serif", 8F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lbd2.ForeColor = System.Drawing.SystemColors.ButtonFace;
            this.lbd2.Location = new System.Drawing.Point(165, 0);
            this.lbd2.Name = "lbd2";
            this.lbd2.Size = new System.Drawing.Size(60, 15);
            this.lbd2.TabIndex = 77;
            this.lbd2.Text = "-";
            this.lbd2.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // lbd1
            // 
            this.lbd1.BackColor = System.Drawing.Color.Black;
            this.lbd1.Font = new System.Drawing.Font("Microsoft Sans Serif", 8F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lbd1.ForeColor = System.Drawing.SystemColors.ButtonFace;
            this.lbd1.Location = new System.Drawing.Point(94, 1);
            this.lbd1.Name = "lbd1";
            this.lbd1.Size = new System.Drawing.Size(60, 15);
            this.lbd1.TabIndex = 76;
            this.lbd1.Text = "-";
            this.lbd1.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // PanelCores
            // 
            this.PanelCores.BackColor = System.Drawing.SystemColors.ActiveCaptionText;
            this.PanelCores.Controls.Add(this.lbNomeEsforco);
            this.PanelCores.Controls.Add(this.pnd10);
            this.PanelCores.Controls.Add(this.pnd9);
            this.PanelCores.Controls.Add(this.pnd8);
            this.PanelCores.Controls.Add(this.pnd7);
            this.PanelCores.Controls.Add(this.pnd6);
            this.PanelCores.Controls.Add(this.pnd5);
            this.PanelCores.Controls.Add(this.pnd4);
            this.PanelCores.Controls.Add(this.pnd3);
            this.PanelCores.Controls.Add(this.pnd2);
            this.PanelCores.Controls.Add(this.pnd1);
            this.PanelCores.Location = new System.Drawing.Point(32, 17);
            this.PanelCores.Name = "PanelCores";
            this.PanelCores.Size = new System.Drawing.Size(871, 19);
            this.PanelCores.TabIndex = 52;
            // 
            // lbNomeEsforco
            // 
            this.lbNomeEsforco.AutoSize = true;
            this.lbNomeEsforco.Font = new System.Drawing.Font("Microsoft Sans Serif", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lbNomeEsforco.ForeColor = System.Drawing.SystemColors.ControlLightLight;
            this.lbNomeEsforco.Location = new System.Drawing.Point(748, 1);
            this.lbNomeEsforco.Name = "lbNomeEsforco";
            this.lbNomeEsforco.Size = new System.Drawing.Size(11, 15);
            this.lbNomeEsforco.TabIndex = 76;
            this.lbNomeEsforco.Text = "-";
            // 
            // pnd10
            // 
            this.pnd10.BackColor = System.Drawing.Color.Blue;
            this.pnd10.Dock = System.Windows.Forms.DockStyle.Left;
            this.pnd10.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.pnd10.Location = new System.Drawing.Point(629, 0);
            this.pnd10.Name = "pnd10";
            this.pnd10.Size = new System.Drawing.Size(70, 19);
            this.pnd10.TabIndex = 64;
            this.pnd10.UseVisualStyleBackColor = false;
            // 
            // pnd9
            // 
            this.pnd9.BackColor = System.Drawing.Color.RoyalBlue;
            this.pnd9.Dock = System.Windows.Forms.DockStyle.Left;
            this.pnd9.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.pnd9.Location = new System.Drawing.Point(559, 0);
            this.pnd9.Name = "pnd9";
            this.pnd9.Size = new System.Drawing.Size(70, 19);
            this.pnd9.TabIndex = 63;
            this.pnd9.UseVisualStyleBackColor = false;
            // 
            // pnd8
            // 
            this.pnd8.BackColor = System.Drawing.Color.DodgerBlue;
            this.pnd8.Dock = System.Windows.Forms.DockStyle.Left;
            this.pnd8.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.pnd8.Location = new System.Drawing.Point(489, 0);
            this.pnd8.Name = "pnd8";
            this.pnd8.Size = new System.Drawing.Size(70, 19);
            this.pnd8.TabIndex = 62;
            this.pnd8.UseVisualStyleBackColor = false;
            // 
            // pnd7
            // 
            this.pnd7.BackColor = System.Drawing.SystemColors.ActiveCaption;
            this.pnd7.Dock = System.Windows.Forms.DockStyle.Left;
            this.pnd7.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.pnd7.Location = new System.Drawing.Point(419, 0);
            this.pnd7.Name = "pnd7";
            this.pnd7.Size = new System.Drawing.Size(70, 19);
            this.pnd7.TabIndex = 61;
            this.pnd7.UseVisualStyleBackColor = false;
            // 
            // pnd6
            // 
            this.pnd6.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(224)))), ((int)(((byte)(224)))), ((int)(((byte)(224)))));
            this.pnd6.Dock = System.Windows.Forms.DockStyle.Left;
            this.pnd6.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.pnd6.Location = new System.Drawing.Point(349, 0);
            this.pnd6.Name = "pnd6";
            this.pnd6.Size = new System.Drawing.Size(70, 19);
            this.pnd6.TabIndex = 60;
            this.pnd6.UseVisualStyleBackColor = false;
            // 
            // pnd5
            // 
            this.pnd5.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(240)))), ((int)(((byte)(148)))));
            this.pnd5.Dock = System.Windows.Forms.DockStyle.Left;
            this.pnd5.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.pnd5.Location = new System.Drawing.Point(279, 0);
            this.pnd5.Name = "pnd5";
            this.pnd5.Size = new System.Drawing.Size(70, 19);
            this.pnd5.TabIndex = 59;
            this.pnd5.UseVisualStyleBackColor = false;
            // 
            // pnd4
            // 
            this.pnd4.BackColor = System.Drawing.Color.Yellow;
            this.pnd4.Dock = System.Windows.Forms.DockStyle.Left;
            this.pnd4.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.pnd4.Location = new System.Drawing.Point(209, 0);
            this.pnd4.Name = "pnd4";
            this.pnd4.Size = new System.Drawing.Size(70, 19);
            this.pnd4.TabIndex = 58;
            this.pnd4.UseVisualStyleBackColor = false;
            // 
            // pnd3
            // 
            this.pnd3.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(128)))), ((int)(((byte)(0)))));
            this.pnd3.Dock = System.Windows.Forms.DockStyle.Left;
            this.pnd3.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.pnd3.Location = new System.Drawing.Point(139, 0);
            this.pnd3.Name = "pnd3";
            this.pnd3.Size = new System.Drawing.Size(70, 19);
            this.pnd3.TabIndex = 57;
            this.pnd3.UseVisualStyleBackColor = false;
            // 
            // pnd2
            // 
            this.pnd2.BackColor = System.Drawing.Color.OrangeRed;
            this.pnd2.Dock = System.Windows.Forms.DockStyle.Left;
            this.pnd2.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.pnd2.Location = new System.Drawing.Point(69, 0);
            this.pnd2.Name = "pnd2";
            this.pnd2.Size = new System.Drawing.Size(70, 19);
            this.pnd2.TabIndex = 56;
            this.pnd2.UseVisualStyleBackColor = false;
            // 
            // pnd1
            // 
            this.pnd1.BackColor = System.Drawing.Color.Red;
            this.pnd1.Dock = System.Windows.Forms.DockStyle.Left;
            this.pnd1.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.pnd1.Location = new System.Drawing.Point(0, 0);
            this.pnd1.Name = "pnd1";
            this.pnd1.Size = new System.Drawing.Size(69, 19);
            this.pnd1.TabIndex = 55;
            this.pnd1.UseVisualStyleBackColor = false;
            // 
            // Controle
            // 
            this.Controle.BackColor = System.Drawing.Color.Black;
            this.Controle.ContextMenuStrip = this.contextMenuStrip1;
            this.Controle.Dock = System.Windows.Forms.DockStyle.Fill;
            this.Controle.Location = new System.Drawing.Point(0, 0);
            this.Controle.Name = "Controle";
            this.Controle.Size = new System.Drawing.Size(894, 438);
            this.Controle.TabIndex = 75;
            this.Controle.VSync = false;
            this.Controle.MouseClick += new System.Windows.Forms.MouseEventHandler(this.Controle_MouseClick);
            this.Controle.MouseDown += new System.Windows.Forms.MouseEventHandler(this.Controle_MouseDown);
            this.Controle.MouseEnter += new System.EventHandler(this.Controle_MouseEnter);
            this.Controle.MouseMove += new System.Windows.Forms.MouseEventHandler(this.Controle_MouseMove);
            this.Controle.MouseUp += new System.Windows.Forms.MouseEventHandler(this.Controle_MouseUp);
            this.Controle.MouseWheel += new System.Windows.Forms.MouseEventHandler(this.Controle_MouseWheel);
            this.Controle.Resize += new System.EventHandler(this.Controle_Resize);
            // 
            // timer1
            // 
            this.timer1.Interval = 200;
            this.timer1.Tick += new System.EventHandler(this.timer1_Tick);
            // 
            // button1
            // 
            this.button1.Location = new System.Drawing.Point(81, 59);
            this.button1.Name = "button1";
            this.button1.Size = new System.Drawing.Size(327, 23);
            this.button1.TabIndex = 76;
            this.button1.Text = "button1";
            this.button1.UseVisualStyleBackColor = true;
            this.button1.Click += new System.EventHandler(this.button1_Click);
            // 
            // FVisualizadorPortico
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(894, 474);
            this.Controls.Add(this.button1);
            this.Controls.Add(this.lbInfoNo);
            this.Controls.Add(this.lbInfoBarra);
            this.Controls.Add(this.Controle);
            this.Controls.Add(this.PanelCores2);
            this.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.Name = "FVisualizadorPortico";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "Pórtico";
            this.WindowState = System.Windows.Forms.FormWindowState.Maximized;
            this.Activated += new System.EventHandler(this.FVisualizadorPortico_Activated);
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.FVisualizadorPortico_FormClosing);
            this.Load += new System.EventHandler(this.FVisualizadorPortico_Load);
            this.Shown += new System.EventHandler(this.FVisualizadorPortico_Shown);
            this.DockChanged += new System.EventHandler(this.FVisualizadorPortico_DockChanged);
            this.SizeChanged += new System.EventHandler(this.FVisualizadorPortico_SizeChanged);
            this.Paint += new System.Windows.Forms.PaintEventHandler(this.FVisualizadorPortico_Paint);
            this.Resize += new System.EventHandler(this.FVisualizadorPortico_Resize);
            this.Validated += new System.EventHandler(this.FVisualizadorPortico_Validated);
            this.contextMenuStrip1.ResumeLayout(false);
            this.PanelCores2.ResumeLayout(false);
            this.panel1.ResumeLayout(false);
            this.PanelCores.ResumeLayout(false);
            this.PanelCores.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label lbInfoBarra;
        private System.Windows.Forms.Label lbInfoNo;
        public System.Windows.Forms.Panel PanelCores2;
        private System.Windows.Forms.Panel panel1;
        private System.Windows.Forms.Label lbd0;
        private System.Windows.Forms.Label lbd10;
        private System.Windows.Forms.Label lbd9;
        private System.Windows.Forms.Label lbd8;
        private System.Windows.Forms.Label lbd7;
        private System.Windows.Forms.Label lbd6;
        private System.Windows.Forms.Label lbd5;
        private System.Windows.Forms.Label lbd4;
        private System.Windows.Forms.Label lbd3;
        private System.Windows.Forms.Label lbd2;
        private System.Windows.Forms.Label lbd1;
        private System.Windows.Forms.Panel PanelCores;
        private System.Windows.Forms.Label lbNomeEsforco;
        private System.Windows.Forms.Button pnd10;
        private System.Windows.Forms.Button pnd9;
        private System.Windows.Forms.Button pnd8;
        private System.Windows.Forms.Button pnd7;
        private System.Windows.Forms.Button pnd6;
        private System.Windows.Forms.Button pnd5;
        private System.Windows.Forms.Button pnd4;
        private System.Windows.Forms.Button pnd3;
        private System.Windows.Forms.Button pnd2;
        private System.Windows.Forms.Button pnd1;
        private System.Windows.Forms.Timer timerVistas;
        private System.Windows.Forms.ContextMenuStrip contextMenuStrip1;
        private System.Windows.Forms.ToolStripMenuItem cancelarToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem animarToolStripMenuItem;
        public OpenTK.GLControl Controle;
        private System.Windows.Forms.Timer timer1;
        private System.Windows.Forms.Button button1;
    }
}