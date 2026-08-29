namespace PG
{
    partial class FNovoNo
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
            this.edz = new System.Windows.Forms.TextBox();
            this.edy = new System.Windows.Forms.TextBox();
            this.edx = new System.Windows.Forms.TextBox();
            this.label1 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.label3 = new System.Windows.Forms.Label();
            this.panel1 = new System.Windows.Forms.Panel();
            this.button3 = new System.Windows.Forms.Button();
            this.btSalvar = new System.Windows.Forms.Button();
            this.Engaste = new System.Windows.Forms.RadioButton();
            this.Apoio = new System.Windows.Forms.RadioButton();
            this.radioButton1 = new System.Windows.Forms.RadioButton();
            this.panel1.SuspendLayout();
            this.SuspendLayout();
            // 
            // edz
            // 
            this.edz.Location = new System.Drawing.Point(26, 75);
            this.edz.Name = "edz";
            this.edz.Size = new System.Drawing.Size(68, 20);
            this.edz.TabIndex = 36;
            this.edz.Text = "0";
            this.edz.KeyPress += new System.Windows.Forms.KeyPressEventHandler(this.edb1_KeyPress);
            // 
            // edy
            // 
            this.edy.Location = new System.Drawing.Point(26, 49);
            this.edy.Name = "edy";
            this.edy.Size = new System.Drawing.Size(68, 20);
            this.edy.TabIndex = 35;
            this.edy.Text = "0";
            this.edy.KeyPress += new System.Windows.Forms.KeyPressEventHandler(this.edb1_KeyPress);
            // 
            // edx
            // 
            this.edx.Location = new System.Drawing.Point(26, 23);
            this.edx.Name = "edx";
            this.edx.Size = new System.Drawing.Size(68, 20);
            this.edx.TabIndex = 34;
            this.edx.Text = "0";
            this.edx.KeyPress += new System.Windows.Forms.KeyPressEventHandler(this.edb1_KeyPress);
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(8, 30);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(14, 13);
            this.label1.TabIndex = 37;
            this.label1.Text = "X";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(8, 56);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(14, 13);
            this.label2.TabIndex = 38;
            this.label2.Text = "Y";
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(8, 78);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(14, 13);
            this.label3.TabIndex = 39;
            this.label3.Text = "Z";
            // 
            // panel1
            // 
            this.panel1.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.panel1.Controls.Add(this.button3);
            this.panel1.Controls.Add(this.btSalvar);
            this.panel1.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.panel1.Location = new System.Drawing.Point(0, 151);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(207, 34);
            this.panel1.TabIndex = 40;
            // 
            // button3
            // 
            this.button3.BackColor = System.Drawing.Color.Transparent;
            this.button3.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.button3.Location = new System.Drawing.Point(138, 3);
            this.button3.Name = "button3";
            this.button3.Size = new System.Drawing.Size(61, 25);
            this.button3.TabIndex = 16;
            this.button3.Text = "Fechar";
            this.button3.UseVisualStyleBackColor = false;
            this.button3.Click += new System.EventHandler(this.button3_Click);
            // 
            // btSalvar
            // 
            this.btSalvar.BackColor = System.Drawing.Color.Transparent;
            this.btSalvar.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btSalvar.Location = new System.Drawing.Point(3, 4);
            this.btSalvar.Name = "btSalvar";
            this.btSalvar.Size = new System.Drawing.Size(67, 25);
            this.btSalvar.TabIndex = 16;
            this.btSalvar.Text = "Ok";
            this.btSalvar.UseVisualStyleBackColor = false;
            this.btSalvar.Click += new System.EventHandler(this.btSalvar_Click);
            // 
            // Engaste
            // 
            this.Engaste.AutoSize = true;
            this.Engaste.Location = new System.Drawing.Point(67, 101);
            this.Engaste.Name = "Engaste";
            this.Engaste.Size = new System.Drawing.Size(64, 17);
            this.Engaste.TabIndex = 43;
            this.Engaste.Text = "Engaste";
            this.Engaste.UseVisualStyleBackColor = true;
            // 
            // Apoio
            // 
            this.Apoio.AutoSize = true;
            this.Apoio.Location = new System.Drawing.Point(136, 101);
            this.Apoio.Name = "Apoio";
            this.Apoio.Size = new System.Drawing.Size(52, 17);
            this.Apoio.TabIndex = 44;
            this.Apoio.Text = "Apoio";
            this.Apoio.UseVisualStyleBackColor = true;
            // 
            // radioButton1
            // 
            this.radioButton1.AutoSize = true;
            this.radioButton1.Checked = true;
            this.radioButton1.Location = new System.Drawing.Point(12, 101);
            this.radioButton1.Name = "radioButton1";
            this.radioButton1.Size = new System.Drawing.Size(48, 17);
            this.radioButton1.TabIndex = 45;
            this.radioButton1.TabStop = true;
            this.radioButton1.Text = "Livre";
            this.radioButton1.UseVisualStyleBackColor = true;
            // 
            // FNovoNo
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(207, 185);
            this.Controls.Add(this.radioButton1);
            this.Controls.Add(this.Apoio);
            this.Controls.Add(this.Engaste);
            this.Controls.Add(this.panel1);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.edz);
            this.Controls.Add(this.edy);
            this.Controls.Add(this.edx);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedToolWindow;
            this.Name = "FNovoNo";
            this.Text = "PGi - Novo nó";
            this.TopMost = true;
            this.FormClosed += new System.Windows.Forms.FormClosedEventHandler(this.FNovoNo_FormClosed);
            this.Load += new System.EventHandler(this.FNovoNo_Load);
            this.Shown += new System.EventHandler(this.FNovoNo_Shown);
            this.panel1.ResumeLayout(false);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        public System.Windows.Forms.TextBox edz;
        public System.Windows.Forms.TextBox edy;
        public System.Windows.Forms.TextBox edx;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.Panel panel1;
        private System.Windows.Forms.Button button3;
        public System.Windows.Forms.Button btSalvar;
        private System.Windows.Forms.RadioButton Engaste;
        private System.Windows.Forms.RadioButton Apoio;
        private System.Windows.Forms.RadioButton radioButton1;
    }
}