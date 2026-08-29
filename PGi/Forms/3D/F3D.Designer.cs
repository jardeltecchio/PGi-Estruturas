namespace PG
{
    partial class F3D
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
            this.opçõesDeVisualizaçãoToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.checkBox1 = new System.Windows.Forms.CheckBox();
            this.button1 = new System.Windows.Forms.Button();
            this.edZoom = new System.Windows.Forms.NumericUpDown();
            this.numericUpDown2 = new System.Windows.Forms.NumericUpDown();
            this.numericUpDown3 = new System.Windows.Forms.NumericUpDown();
            this.numericUpDown1 = new System.Windows.Forms.NumericUpDown();
            this.numericUpDown4 = new System.Windows.Forms.NumericUpDown();
            this.textBox1 = new System.Windows.Forms.TextBox();
            this.textBox2 = new System.Windows.Forms.TextBox();
            this.textBox3 = new System.Windows.Forms.TextBox();
            this.Controle = new OpenTK.GLControl(/*new OpenTK.Graphics.GraphicsMode(32, 24, 0, 8)*/);
            this.checkBox2 = new System.Windows.Forms.CheckBox();
            this.textBox4 = new System.Windows.Forms.TextBox();
            this.textBox5 = new System.Windows.Forms.TextBox();
            this.textBox6 = new System.Windows.Forms.TextBox();
            this.button2 = new System.Windows.Forms.Button();
            this.button3 = new System.Windows.Forms.Button();
            this.textBox7 = new System.Windows.Forms.TextBox();
            this.textBox8 = new System.Windows.Forms.TextBox();
            this.label1 = new System.Windows.Forms.Label();
            this.contextMenuStrip1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.edZoom)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.numericUpDown2)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.numericUpDown3)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.numericUpDown1)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.numericUpDown4)).BeginInit();
            this.SuspendLayout();
            // 
            // contextMenuStrip1
            // 
            this.contextMenuStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.opçõesDeVisualizaçãoToolStripMenuItem});
            this.contextMenuStrip1.Name = "contextMenuStrip1";
            this.contextMenuStrip1.Size = new System.Drawing.Size(197, 26);
            // 
            // opçõesDeVisualizaçãoToolStripMenuItem
            // 
            this.opçõesDeVisualizaçãoToolStripMenuItem.Name = "opçõesDeVisualizaçãoToolStripMenuItem";
            this.opçõesDeVisualizaçãoToolStripMenuItem.Size = new System.Drawing.Size(196, 22);
            this.opçõesDeVisualizaçãoToolStripMenuItem.Text = "Opções de visualização";
            this.opçõesDeVisualizaçãoToolStripMenuItem.Click += new System.EventHandler(this.opçõesDeVisualizaçãoToolStripMenuItem_Click);
            // 
            // checkBox1
            // 
            this.checkBox1.AutoSize = true;
            this.checkBox1.Location = new System.Drawing.Point(343, 197);
            this.checkBox1.Name = "checkBox1";
            this.checkBox1.Size = new System.Drawing.Size(80, 17);
            this.checkBox1.TabIndex = 34;
            this.checkBox1.Text = "checkBox1";
            this.checkBox1.UseVisualStyleBackColor = true;
            this.checkBox1.Visible = false;
            // 
            // button1
            // 
            this.button1.Location = new System.Drawing.Point(166, 68);
            this.button1.Name = "button1";
            this.button1.Size = new System.Drawing.Size(106, 32);
            this.button1.TabIndex = 35;
            this.button1.Text = "button1";
            this.button1.UseVisualStyleBackColor = true;
            this.button1.Visible = false;
            this.button1.Click += new System.EventHandler(this.button1_Click);
            // 
            // edZoom
            // 
            this.edZoom.Location = new System.Drawing.Point(587, 175);
            this.edZoom.Maximum = new decimal(new int[] {
            10000,
            0,
            0,
            0});
            this.edZoom.Name = "edZoom";
            this.edZoom.Size = new System.Drawing.Size(67, 20);
            this.edZoom.TabIndex = 36;
            this.edZoom.Value = new decimal(new int[] {
            45,
            0,
            0,
            0});
            this.edZoom.Visible = false;
            this.edZoom.ValueChanged += new System.EventHandler(this.numericUpDown1_ValueChanged);
            // 
            // numericUpDown2
            // 
            this.numericUpDown2.Location = new System.Drawing.Point(587, 201);
            this.numericUpDown2.Maximum = new decimal(new int[] {
            100000,
            0,
            0,
            0});
            this.numericUpDown2.Name = "numericUpDown2";
            this.numericUpDown2.Size = new System.Drawing.Size(78, 20);
            this.numericUpDown2.TabIndex = 37;
            this.numericUpDown2.Visible = false;
            // 
            // numericUpDown3
            // 
            this.numericUpDown3.Location = new System.Drawing.Point(134, 12);
            this.numericUpDown3.Maximum = new decimal(new int[] {
            5000000,
            0,
            0,
            0});
            this.numericUpDown3.Name = "numericUpDown3";
            this.numericUpDown3.Size = new System.Drawing.Size(61, 20);
            this.numericUpDown3.TabIndex = 40;
            this.numericUpDown3.Visible = false;
            this.numericUpDown3.ValueChanged += new System.EventHandler(this.numericUpDown3_ValueChanged);
            // 
            // numericUpDown1
            // 
            this.numericUpDown1.Location = new System.Drawing.Point(67, 12);
            this.numericUpDown1.Maximum = new decimal(new int[] {
            5000000,
            0,
            0,
            0});
            this.numericUpDown1.Name = "numericUpDown1";
            this.numericUpDown1.Size = new System.Drawing.Size(61, 20);
            this.numericUpDown1.TabIndex = 39;
            this.numericUpDown1.Visible = false;
            this.numericUpDown1.ValueChanged += new System.EventHandler(this.numericUpDown1_ValueChanged_1);
            // 
            // numericUpDown4
            // 
            this.numericUpDown4.Location = new System.Drawing.Point(0, 12);
            this.numericUpDown4.Maximum = new decimal(new int[] {
            5000000,
            0,
            0,
            0});
            this.numericUpDown4.Name = "numericUpDown4";
            this.numericUpDown4.Size = new System.Drawing.Size(61, 20);
            this.numericUpDown4.TabIndex = 38;
            this.numericUpDown4.Visible = false;
            this.numericUpDown4.ValueChanged += new System.EventHandler(this.numericUpDown4_ValueChanged);
            // 
            // textBox1
            // 
            this.textBox1.Location = new System.Drawing.Point(40, 80);
            this.textBox1.Name = "textBox1";
            this.textBox1.Size = new System.Drawing.Size(33, 20);
            this.textBox1.TabIndex = 41;
            this.textBox1.Visible = false;
            // 
            // textBox2
            // 
            this.textBox2.Location = new System.Drawing.Point(79, 80);
            this.textBox2.Name = "textBox2";
            this.textBox2.Size = new System.Drawing.Size(39, 20);
            this.textBox2.TabIndex = 42;
            this.textBox2.Visible = false;
            // 
            // textBox3
            // 
            this.textBox3.Location = new System.Drawing.Point(124, 80);
            this.textBox3.Name = "textBox3";
            this.textBox3.Size = new System.Drawing.Size(39, 20);
            this.textBox3.TabIndex = 43;
            this.textBox3.Visible = false;
            // 
            // Controle
            // 
            this.Controle.BackColor = System.Drawing.Color.Black;
            this.Controle.Cursor = System.Windows.Forms.Cursors.Default;
            this.Controle.Dock = System.Windows.Forms.DockStyle.Fill;
            this.Controle.Location = new System.Drawing.Point(0, 0);
            this.Controle.Name = "Controle";
            this.Controle.Size = new System.Drawing.Size(794, 481);
            this.Controle.TabIndex = 44;
            this.Controle.VSync = false;
            this.Controle.Load += new System.EventHandler(this.Controle_Load);
            this.Controle.DoubleClick += new System.EventHandler(this.Controle_DoubleClick);
            this.Controle.MouseClick += new System.Windows.Forms.MouseEventHandler(this.Controle_MouseClick_1);
            this.Controle.MouseDown += new System.Windows.Forms.MouseEventHandler(this.Controle_MouseDown);
            this.Controle.MouseMove += new System.Windows.Forms.MouseEventHandler(this.Controle_MouseMove);
            this.Controle.MouseUp += new System.Windows.Forms.MouseEventHandler(this.Controle_MouseUp);
            this.Controle.MouseWheel += new System.Windows.Forms.MouseEventHandler(this.Controle_MouseWheel);
            this.Controle.Resize += new System.EventHandler(this.Controle_Resize_1);
            // 
            // checkBox2
            // 
            this.checkBox2.AutoSize = true;
            this.checkBox2.Location = new System.Drawing.Point(65, 24);
            this.checkBox2.Name = "checkBox2";
            this.checkBox2.Size = new System.Drawing.Size(80, 17);
            this.checkBox2.TabIndex = 45;
            this.checkBox2.Text = "checkBox2";
            this.checkBox2.UseVisualStyleBackColor = true;
            // 
            // textBox4
            // 
            this.textBox4.Location = new System.Drawing.Point(151, 11);
            this.textBox4.Name = "textBox4";
            this.textBox4.Size = new System.Drawing.Size(100, 20);
            this.textBox4.TabIndex = 46;
            this.textBox4.Visible = false;
            // 
            // textBox5
            // 
            this.textBox5.Location = new System.Drawing.Point(266, 12);
            this.textBox5.Name = "textBox5";
            this.textBox5.Size = new System.Drawing.Size(100, 20);
            this.textBox5.TabIndex = 47;
            this.textBox5.Visible = false;
            // 
            // textBox6
            // 
            this.textBox6.Location = new System.Drawing.Point(397, 11);
            this.textBox6.Name = "textBox6";
            this.textBox6.Size = new System.Drawing.Size(100, 20);
            this.textBox6.TabIndex = 48;
            this.textBox6.Visible = false;
            // 
            // button2
            // 
            this.button2.Location = new System.Drawing.Point(520, 20);
            this.button2.Name = "button2";
            this.button2.Size = new System.Drawing.Size(75, 23);
            this.button2.TabIndex = 49;
            this.button2.Text = "button2";
            this.button2.UseVisualStyleBackColor = true;
            this.button2.Visible = false;
            this.button2.Click += new System.EventHandler(this.button2_Click_1);
            // 
            // button3
            // 
            this.button3.Location = new System.Drawing.Point(88, 150);
            this.button3.Name = "button3";
            this.button3.Size = new System.Drawing.Size(75, 23);
            this.button3.TabIndex = 50;
            this.button3.Text = "button3";
            this.button3.UseVisualStyleBackColor = true;
            this.button3.Visible = false;
            this.button3.Click += new System.EventHandler(this.button3_Click);
            // 
            // textBox7
            // 
            this.textBox7.Location = new System.Drawing.Point(169, 68);
            this.textBox7.Name = "textBox7";
            this.textBox7.Size = new System.Drawing.Size(100, 20);
            this.textBox7.TabIndex = 52;
            this.textBox7.Visible = false;
            // 
            // textBox8
            // 
            this.textBox8.Location = new System.Drawing.Point(60, 68);
            this.textBox8.Name = "textBox8";
            this.textBox8.Size = new System.Drawing.Size(100, 20);
            this.textBox8.TabIndex = 51;
            this.textBox8.Visible = false;
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(57, 53);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(38, 13);
            this.label1.TabIndex = 53;
            this.label1.Text = "offsets";
            this.label1.Visible = false;
            // 
            // F3D
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(794, 481);
            this.ContextMenuStrip = this.contextMenuStrip1;
            this.Controls.Add(this.label1);
            this.Controls.Add(this.textBox7);
            this.Controls.Add(this.textBox8);
            this.Controls.Add(this.button3);
            this.Controls.Add(this.button2);
            this.Controls.Add(this.textBox6);
            this.Controls.Add(this.textBox5);
            this.Controls.Add(this.textBox4);
            this.Controls.Add(this.checkBox2);
            this.Controls.Add(this.Controle);
            this.Controls.Add(this.textBox3);
            this.Controls.Add(this.textBox2);
            this.Controls.Add(this.textBox1);
            this.Controls.Add(this.numericUpDown3);
            this.Controls.Add(this.numericUpDown1);
            this.Controls.Add(this.numericUpDown4);
            this.Controls.Add(this.numericUpDown2);
            this.Controls.Add(this.edZoom);
            this.Controls.Add(this.button1);
            this.Controls.Add(this.checkBox1);
            this.Cursor = System.Windows.Forms.Cursors.Cross;
            this.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.Name = "F3D";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "3D";
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.F3D_FormClosing);
            this.FormClosed += new System.Windows.Forms.FormClosedEventHandler(this.F3D_FormClosed);
            this.Load += new System.EventHandler(this.F3D_Load);
            this.Shown += new System.EventHandler(this.F3D_Shown);
            this.Resize += new System.EventHandler(this.F3D_Resize);
            this.contextMenuStrip1.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.edZoom)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.numericUpDown2)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.numericUpDown3)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.numericUpDown1)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.numericUpDown4)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.CheckBox checkBox1;
        private System.Windows.Forms.Button button1;
        private System.Windows.Forms.NumericUpDown edZoom;
        private System.Windows.Forms.NumericUpDown numericUpDown2;
        private System.Windows.Forms.ContextMenuStrip contextMenuStrip1;
        private System.Windows.Forms.ToolStripMenuItem opçõesDeVisualizaçãoToolStripMenuItem;
        private System.Windows.Forms.NumericUpDown numericUpDown3;
        private System.Windows.Forms.NumericUpDown numericUpDown1;
        private System.Windows.Forms.NumericUpDown numericUpDown4;
        private System.Windows.Forms.TextBox textBox1;
        private System.Windows.Forms.TextBox textBox2;
        private System.Windows.Forms.TextBox textBox3;
        public OpenTK.GLControl Controle;
        private System.Windows.Forms.CheckBox checkBox2;
        private System.Windows.Forms.TextBox textBox4;
        private System.Windows.Forms.TextBox textBox5;
        private System.Windows.Forms.TextBox textBox6;
        private System.Windows.Forms.Button button2;
        private System.Windows.Forms.Button button3;
        private System.Windows.Forms.TextBox textBox7;
        private System.Windows.Forms.TextBox textBox8;
        private System.Windows.Forms.Label label1;
    }
}