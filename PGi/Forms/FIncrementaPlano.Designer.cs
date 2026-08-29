namespace PG
{
    partial class FIncrementaPlano
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(FIncrementaPlano));
            this.lbCoordPlano = new System.Windows.Forms.Label();
            this.edIncPlano = new System.Windows.Forms.TextBox();
            this.btPisoCima = new System.Windows.Forms.Button();
            this.btPisoBaixo = new System.Windows.Forms.Button();
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.rbyz = new System.Windows.Forms.RadioButton();
            this.rbxz = new System.Windows.Forms.RadioButton();
            this.rbxy = new System.Windows.Forms.RadioButton();
            this.panel1 = new System.Windows.Forms.Panel();
            this.button2 = new System.Windows.Forms.Button();
            this.imageList2 = new System.Windows.Forms.ImageList(this.components);
            this.button1 = new System.Windows.Forms.Button();
            this.groupBox1.SuspendLayout();
            this.panel1.SuspendLayout();
            this.SuspendLayout();
            // 
            // lbCoordPlano
            // 
            this.lbCoordPlano.AutoSize = true;
            this.lbCoordPlano.Location = new System.Drawing.Point(10, 68);
            this.lbCoordPlano.Name = "lbCoordPlano";
            this.lbCoordPlano.Size = new System.Drawing.Size(34, 13);
            this.lbCoordPlano.TabIndex = 149;
            this.lbCoordPlano.Text = "Z (m):";
            // 
            // edIncPlano
            // 
            this.edIncPlano.BackColor = System.Drawing.SystemColors.ButtonHighlight;
            this.edIncPlano.Location = new System.Drawing.Point(53, 63);
            this.edIncPlano.Name = "edIncPlano";
            this.edIncPlano.Size = new System.Drawing.Size(50, 20);
            this.edIncPlano.TabIndex = 148;
            this.edIncPlano.Text = "0";
            this.edIncPlano.KeyDown += new System.Windows.Forms.KeyEventHandler(this.edIncPlano_KeyDown);
            this.edIncPlano.KeyPress += new System.Windows.Forms.KeyPressEventHandler(this.edIncPlano_KeyPress);
            this.edIncPlano.KeyUp += new System.Windows.Forms.KeyEventHandler(this.edIncPlano_KeyUp);
            // 
            // btPisoCima
            // 
            this.btPisoCima.BackColor = System.Drawing.SystemColors.ScrollBar;
            this.btPisoCima.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btPisoCima.Image = global::PGi.Properties.Resources.pra_cima1;
            this.btPisoCima.Location = new System.Drawing.Point(109, 64);
            this.btPisoCima.Name = "btPisoCima";
            this.btPisoCima.Size = new System.Drawing.Size(20, 19);
            this.btPisoCima.TabIndex = 154;
            this.btPisoCima.UseVisualStyleBackColor = false;
            this.btPisoCima.Click += new System.EventHandler(this.btPisoCima_Click);
            // 
            // btPisoBaixo
            // 
            this.btPisoBaixo.BackColor = System.Drawing.SystemColors.ScrollBar;
            this.btPisoBaixo.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btPisoBaixo.Image = global::PGi.Properties.Resources.pra_baixo1;
            this.btPisoBaixo.Location = new System.Drawing.Point(132, 64);
            this.btPisoBaixo.Name = "btPisoBaixo";
            this.btPisoBaixo.Size = new System.Drawing.Size(19, 19);
            this.btPisoBaixo.TabIndex = 153;
            this.btPisoBaixo.UseVisualStyleBackColor = false;
            this.btPisoBaixo.Click += new System.EventHandler(this.btPisoBaixo_Click);
            // 
            // groupBox1
            // 
            this.groupBox1.Controls.Add(this.rbyz);
            this.groupBox1.Controls.Add(this.rbxz);
            this.groupBox1.Controls.Add(this.rbxy);
            this.groupBox1.Location = new System.Drawing.Point(5, 4);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(145, 42);
            this.groupBox1.TabIndex = 155;
            this.groupBox1.TabStop = false;
            this.groupBox1.Text = "Plano";
            // 
            // rbyz
            // 
            this.rbyz.AutoSize = true;
            this.rbyz.Location = new System.Drawing.Point(53, 19);
            this.rbyz.Name = "rbyz";
            this.rbyz.Size = new System.Drawing.Size(39, 17);
            this.rbyz.TabIndex = 2;
            this.rbyz.TabStop = true;
            this.rbyz.Text = "YZ";
            this.rbyz.UseVisualStyleBackColor = true;
            this.rbyz.CheckedChanged += new System.EventHandler(this.rbyz_CheckedChanged);
            // 
            // rbxz
            // 
            this.rbxz.AutoSize = true;
            this.rbxz.Location = new System.Drawing.Point(97, 19);
            this.rbxz.Name = "rbxz";
            this.rbxz.Size = new System.Drawing.Size(39, 17);
            this.rbxz.TabIndex = 1;
            this.rbxz.TabStop = true;
            this.rbxz.Text = "XZ";
            this.rbxz.UseVisualStyleBackColor = true;
            this.rbxz.CheckedChanged += new System.EventHandler(this.rbxz_CheckedChanged);
            // 
            // rbxy
            // 
            this.rbxy.AutoSize = true;
            this.rbxy.Location = new System.Drawing.Point(8, 19);
            this.rbxy.Name = "rbxy";
            this.rbxy.Size = new System.Drawing.Size(39, 17);
            this.rbxy.TabIndex = 0;
            this.rbxy.TabStop = true;
            this.rbxy.Text = "XY";
            this.rbxy.UseVisualStyleBackColor = true;
            this.rbxy.CheckedChanged += new System.EventHandler(this.rbxy_CheckedChanged);
            // 
            // panel1
            // 
            this.panel1.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.panel1.Controls.Add(this.button2);
            this.panel1.Controls.Add(this.button1);
            this.panel1.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.panel1.Location = new System.Drawing.Point(0, 98);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(186, 33);
            this.panel1.TabIndex = 156;
            // 
            // button2
            // 
            this.button2.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.button2.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.button2.ImageIndex = 1;
            this.button2.ImageList = this.imageList2;
            this.button2.Location = new System.Drawing.Point(133, 4);
            this.button2.Name = "button2";
            this.button2.Size = new System.Drawing.Size(48, 24);
            this.button2.TabIndex = 154;
            this.button2.UseVisualStyleBackColor = true;
            this.button2.Click += new System.EventHandler(this.button2_Click);
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
            this.button1.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.button1.ImageIndex = 0;
            this.button1.ImageList = this.imageList2;
            this.button1.Location = new System.Drawing.Point(4, 3);
            this.button1.Name = "button1";
            this.button1.Size = new System.Drawing.Size(48, 24);
            this.button1.TabIndex = 153;
            this.button1.UseVisualStyleBackColor = true;
            this.button1.Click += new System.EventHandler(this.button1_Click);
            // 
            // FIncrementaPlano
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.CancelButton = this.button2;
            this.ClientSize = new System.Drawing.Size(186, 131);
            this.Controls.Add(this.panel1);
            this.Controls.Add(this.groupBox1);
            this.Controls.Add(this.btPisoCima);
            this.Controls.Add(this.btPisoBaixo);
            this.Controls.Add(this.lbCoordPlano);
            this.Controls.Add(this.edIncPlano);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedToolWindow;
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "FIncrementaPlano";
            this.Text = "Plano";
            this.TopMost = true;
            this.FormClosed += new System.Windows.Forms.FormClosedEventHandler(this.FIncrementaPlano_FormClosed);
            this.Load += new System.EventHandler(this.FIncrementaPlano_Load);
            this.Move += new System.EventHandler(this.FIncrementaPlano_Move);
            this.groupBox1.ResumeLayout(false);
            this.groupBox1.PerformLayout();
            this.panel1.ResumeLayout(false);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        public System.Windows.Forms.TextBox edIncPlano;
        public System.Windows.Forms.Label lbCoordPlano;
        private System.Windows.Forms.Button btPisoCima;
        private System.Windows.Forms.Button btPisoBaixo;
        private System.Windows.Forms.GroupBox groupBox1;
        private System.Windows.Forms.Panel panel1;
        private System.Windows.Forms.Button button2;
        private System.Windows.Forms.Button button1;
        public System.Windows.Forms.RadioButton rbyz;
        public System.Windows.Forms.RadioButton rbxz;
        public System.Windows.Forms.RadioButton rbxy;
        public System.Windows.Forms.ImageList imageList2;
    }
}