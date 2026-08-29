namespace PG
{
    partial class FSecaoGenerica
    {
        /// <summary>
        /// Required designer variable.
        /// </summary
        /// 
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(FSecaoGenerica));
            this.colorDialog1 = new System.Windows.Forms.ColorDialog();
            this.panel1 = new System.Windows.Forms.Panel();
            this.button3 = new System.Windows.Forms.Button();
            this.imageList2 = new System.Windows.Forms.ImageList(this.components);
            this.btSalvar = new System.Windows.Forms.Button();
            this.label6 = new System.Windows.Forms.Label();
            this.NomeSecao = new System.Windows.Forms.TextBox();
            this.label7 = new System.Windows.Forms.Label();
            this.btCor = new System.Windows.Forms.Button();
            this.label2 = new System.Windows.Forms.Label();
            this.edA = new System.Windows.Forms.TextBox();
            this.edE = new System.Windows.Forms.TextBox();
            this.label3 = new System.Windows.Forms.Label();
            this.label8 = new System.Windows.Forms.Label();
            this.edG = new System.Windows.Forms.TextBox();
            this.label10 = new System.Windows.Forms.Label();
            this.ix = new System.Windows.Forms.TextBox();
            this.label12 = new System.Windows.Forms.Label();
            this.iy = new System.Windows.Forms.TextBox();
            this.label14 = new System.Windows.Forms.Label();
            this.edJ = new System.Windows.Forms.TextBox();
            this.label1 = new System.Windows.Forms.Label();
            this.label9 = new System.Windows.Forms.Label();
            this.label11 = new System.Windows.Forms.Label();
            this.label15 = new System.Windows.Forms.Label();
            this.label16 = new System.Windows.Forms.Label();
            this.label17 = new System.Windows.Forms.Label();
            this.button1 = new System.Windows.Forms.Button();
            this.button2 = new System.Windows.Forms.Button();
            this.button4 = new System.Windows.Forms.Button();
            this.panel1.SuspendLayout();
            this.SuspendLayout();
            // 
            // panel1
            // 
            this.panel1.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.panel1.Controls.Add(this.button3);
            this.panel1.Controls.Add(this.btSalvar);
            this.panel1.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.panel1.Location = new System.Drawing.Point(0, 254);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(294, 34);
            this.panel1.TabIndex = 10;
            // 
            // button3
            // 
            this.button3.BackColor = System.Drawing.Color.Transparent;
            this.button3.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.button3.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.button3.ImageIndex = 1;
            this.button3.ImageList = this.imageList2;
            this.button3.Location = new System.Drawing.Point(249, 3);
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
            this.btSalvar.TabIndex = 8;
            this.btSalvar.TextAlign = System.Drawing.ContentAlignment.BottomCenter;
            this.btSalvar.UseVisualStyleBackColor = false;
            this.btSalvar.Click += new System.EventHandler(this.btSalvar_Click);
            // 
            // label6
            // 
            this.label6.AutoSize = true;
            this.label6.Location = new System.Drawing.Point(12, 47);
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size(35, 13);
            this.label6.TabIndex = 90;
            this.label6.Text = "Nome";
            // 
            // NomeSecao
            // 
            this.NomeSecao.Location = new System.Drawing.Point(47, 39);
            this.NomeSecao.Name = "NomeSecao";
            this.NomeSecao.Size = new System.Drawing.Size(160, 20);
            this.NomeSecao.TabIndex = 0;
            // 
            // label7
            // 
            this.label7.AutoSize = true;
            this.label7.Location = new System.Drawing.Point(12, 17);
            this.label7.Name = "label7";
            this.label7.Size = new System.Drawing.Size(23, 13);
            this.label7.TabIndex = 88;
            this.label7.Text = "Cor";
            // 
            // btCor
            // 
            this.btCor.BackColor = System.Drawing.Color.DimGray;
            this.btCor.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btCor.Location = new System.Drawing.Point(47, 14);
            this.btCor.Name = "btCor";
            this.btCor.Size = new System.Drawing.Size(20, 19);
            this.btCor.TabIndex = 87;
            this.btCor.UseVisualStyleBackColor = false;
            this.btCor.Click += new System.EventHandler(this.btCor_Click);
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.ForeColor = System.Drawing.SystemColors.Desktop;
            this.label2.Location = new System.Drawing.Point(12, 81);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(29, 13);
            this.label2.TabIndex = 92;
            this.label2.Text = "Área";
            // 
            // edA
            // 
            this.edA.Location = new System.Drawing.Point(47, 74);
            this.edA.Name = "edA";
            this.edA.Size = new System.Drawing.Size(49, 20);
            this.edA.TabIndex = 1;
            this.edA.KeyPress += new System.Windows.Forms.KeyPressEventHandler(this.textBox1_KeyPress);
            // 
            // edE
            // 
            this.edE.Location = new System.Drawing.Point(47, 181);
            this.edE.Name = "edE";
            this.edE.Size = new System.Drawing.Size(49, 20);
            this.edE.TabIndex = 5;
            this.edE.KeyPress += new System.Windows.Forms.KeyPressEventHandler(this.textBox1_KeyPress);
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.ForeColor = System.Drawing.SystemColors.Desktop;
            this.label3.Location = new System.Drawing.Point(12, 188);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(14, 13);
            this.label3.TabIndex = 95;
            this.label3.Text = "E";
            // 
            // label8
            // 
            this.label8.AutoSize = true;
            this.label8.ForeColor = System.Drawing.SystemColors.Desktop;
            this.label8.Location = new System.Drawing.Point(12, 214);
            this.label8.Name = "label8";
            this.label8.Size = new System.Drawing.Size(15, 13);
            this.label8.TabIndex = 98;
            this.label8.Text = "G";
            // 
            // edG
            // 
            this.edG.Location = new System.Drawing.Point(47, 207);
            this.edG.Name = "edG";
            this.edG.Size = new System.Drawing.Size(49, 20);
            this.edG.TabIndex = 6;
            this.edG.KeyDown += new System.Windows.Forms.KeyEventHandler(this.edG_KeyDown);
            // 
            // label10
            // 
            this.label10.AutoSize = true;
            this.label10.ForeColor = System.Drawing.SystemColors.Desktop;
            this.label10.Location = new System.Drawing.Point(12, 106);
            this.label10.Name = "label10";
            this.label10.Size = new System.Drawing.Size(15, 13);
            this.label10.TabIndex = 101;
            this.label10.Text = "Ix";
            // 
            // ix
            // 
            this.ix.Location = new System.Drawing.Point(47, 99);
            this.ix.Name = "ix";
            this.ix.Size = new System.Drawing.Size(49, 20);
            this.ix.TabIndex = 2;
            // 
            // label12
            // 
            this.label12.AutoSize = true;
            this.label12.ForeColor = System.Drawing.SystemColors.Desktop;
            this.label12.Location = new System.Drawing.Point(12, 132);
            this.label12.Name = "label12";
            this.label12.Size = new System.Drawing.Size(15, 13);
            this.label12.TabIndex = 104;
            this.label12.Text = "Iy";
            // 
            // iy
            // 
            this.iy.Location = new System.Drawing.Point(47, 125);
            this.iy.Name = "iy";
            this.iy.Size = new System.Drawing.Size(49, 20);
            this.iy.TabIndex = 3;
            // 
            // label14
            // 
            this.label14.AutoSize = true;
            this.label14.ForeColor = System.Drawing.SystemColors.Desktop;
            this.label14.Location = new System.Drawing.Point(12, 160);
            this.label14.Name = "label14";
            this.label14.Size = new System.Drawing.Size(13, 13);
            this.label14.TabIndex = 107;
            this.label14.Text = "It";
            // 
            // edJ
            // 
            this.edJ.Location = new System.Drawing.Point(47, 153);
            this.edJ.Name = "edJ";
            this.edJ.Size = new System.Drawing.Size(49, 20);
            this.edJ.TabIndex = 4;
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.ForeColor = System.Drawing.SystemColors.Desktop;
            this.label1.Location = new System.Drawing.Point(98, 81);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(24, 13);
            this.label1.TabIndex = 109;
            this.label1.Text = "cm²";
            // 
            // label9
            // 
            this.label9.AutoSize = true;
            this.label9.ForeColor = System.Drawing.SystemColors.Desktop;
            this.label9.Location = new System.Drawing.Point(98, 106);
            this.label9.Name = "label9";
            this.label9.Size = new System.Drawing.Size(27, 13);
            this.label9.TabIndex = 110;
            this.label9.Text = "cm4";
            // 
            // label11
            // 
            this.label11.AutoSize = true;
            this.label11.ForeColor = System.Drawing.SystemColors.Desktop;
            this.label11.Location = new System.Drawing.Point(98, 132);
            this.label11.Name = "label11";
            this.label11.Size = new System.Drawing.Size(27, 13);
            this.label11.TabIndex = 111;
            this.label11.Text = "cm4";
            // 
            // label15
            // 
            this.label15.AutoSize = true;
            this.label15.ForeColor = System.Drawing.SystemColors.Desktop;
            this.label15.Location = new System.Drawing.Point(98, 160);
            this.label15.Name = "label15";
            this.label15.Size = new System.Drawing.Size(27, 13);
            this.label15.TabIndex = 112;
            this.label15.Text = "cm4";
            // 
            // label16
            // 
            this.label16.AutoSize = true;
            this.label16.ForeColor = System.Drawing.SystemColors.Desktop;
            this.label16.Location = new System.Drawing.Point(98, 188);
            this.label16.Name = "label16";
            this.label16.Size = new System.Drawing.Size(29, 13);
            this.label16.TabIndex = 113;
            this.label16.Text = "MPa";
            // 
            // label17
            // 
            this.label17.AutoSize = true;
            this.label17.ForeColor = System.Drawing.SystemColors.Desktop;
            this.label17.Location = new System.Drawing.Point(98, 214);
            this.label17.Name = "label17";
            this.label17.Size = new System.Drawing.Size(29, 13);
            this.label17.TabIndex = 114;
            this.label17.Text = "MPa";
            // 
            // button1
            // 
            this.button1.Location = new System.Drawing.Point(157, 74);
            this.button1.Name = "button1";
            this.button1.Size = new System.Drawing.Size(112, 23);
            this.button1.TabIndex = 115;
            this.button1.Text = "<-  m2 para cm2";
            this.button1.UseVisualStyleBackColor = true;
            this.button1.Click += new System.EventHandler(this.button1_Click);
            // 
            // button2
            // 
            this.button2.Location = new System.Drawing.Point(157, 99);
            this.button2.Name = "button2";
            this.button2.Size = new System.Drawing.Size(112, 77);
            this.button2.TabIndex = 116;
            this.button2.Text = "<-  m4 para cm4";
            this.button2.UseVisualStyleBackColor = true;
            this.button2.Click += new System.EventHandler(this.button2_Click);
            // 
            // button4
            // 
            this.button4.Location = new System.Drawing.Point(157, 179);
            this.button4.Name = "button4";
            this.button4.Size = new System.Drawing.Size(112, 48);
            this.button4.TabIndex = 117;
            this.button4.Text = "<-  kn/m² para MPa";
            this.button4.UseVisualStyleBackColor = true;
            this.button4.Click += new System.EventHandler(this.button4_Click);
            // 
            // FSecaoGenerica
            // 
            this.AcceptButton = this.btSalvar;
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.CancelButton = this.button3;
            this.ClientSize = new System.Drawing.Size(294, 288);
            this.Controls.Add(this.button4);
            this.Controls.Add(this.button2);
            this.Controls.Add(this.button1);
            this.Controls.Add(this.label17);
            this.Controls.Add(this.label16);
            this.Controls.Add(this.label15);
            this.Controls.Add(this.label11);
            this.Controls.Add(this.label9);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.label14);
            this.Controls.Add(this.edJ);
            this.Controls.Add(this.label12);
            this.Controls.Add(this.iy);
            this.Controls.Add(this.label10);
            this.Controls.Add(this.ix);
            this.Controls.Add(this.label8);
            this.Controls.Add(this.edG);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.edE);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.edA);
            this.Controls.Add(this.label6);
            this.Controls.Add(this.NomeSecao);
            this.Controls.Add(this.label7);
            this.Controls.Add(this.btCor);
            this.Controls.Add(this.panel1);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedToolWindow;
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "FSecaoGenerica";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Text = "Seção - Genérica";
            this.TopMost = true;
            this.FormClosed += new System.Windows.Forms.FormClosedEventHandler(this.FSecaoGenerica_FormClosed);
            this.Load += new System.EventHandler(this.FSecaoGenerica_Load);
            this.Move += new System.EventHandler(this.FSecaoGenerica_Move);
            this.panel1.ResumeLayout(false);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion
        private System.Windows.Forms.ColorDialog colorDialog1;
        private System.Windows.Forms.Panel panel1;
        private System.Windows.Forms.Button button3;
        public System.Windows.Forms.Button btSalvar;
        private System.Windows.Forms.Label label6;
        private System.Windows.Forms.TextBox NomeSecao;
        private System.Windows.Forms.Label label7;
        private System.Windows.Forms.Button btCor;
        private System.Windows.Forms.Label label2;
        public System.Windows.Forms.TextBox edA;
        private System.Windows.Forms.TextBox edE;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.Label label8;
        private System.Windows.Forms.TextBox edG;
        private System.Windows.Forms.Label label10;
        public System.Windows.Forms.TextBox ix;
        private System.Windows.Forms.Label label12;
        public System.Windows.Forms.TextBox iy;
        private System.Windows.Forms.Label label14;
        public System.Windows.Forms.TextBox edJ;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label label9;
        private System.Windows.Forms.Label label11;
        private System.Windows.Forms.Label label15;
        private System.Windows.Forms.Label label16;
        private System.Windows.Forms.Label label17;
        private System.Windows.Forms.Button button1;
        private System.Windows.Forms.Button button2;
        private System.Windows.Forms.Button button4;
        public System.Windows.Forms.ImageList imageList2;
    }
}