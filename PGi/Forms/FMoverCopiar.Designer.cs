namespace PG
{
    partial class FMoverCopiar
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
            this.edDx = new System.Windows.Forms.TextBox();
            this.lbCoordPlano = new System.Windows.Forms.Label();
            this.label1 = new System.Windows.Forms.Label();
            this.edDy = new System.Windows.Forms.TextBox();
            this.label2 = new System.Windows.Forms.Label();
            this.edDz = new System.Windows.Forms.TextBox();
            this.button2 = new System.Windows.Forms.Button();
            this.button1 = new System.Windows.Forms.Button();
            this.numericUpDown1 = new System.Windows.Forms.NumericUpDown();
            this.label3 = new System.Windows.Forms.Label();
            this.chManual = new System.Windows.Forms.CheckBox();
            ((System.ComponentModel.ISupportInitialize)(this.numericUpDown1)).BeginInit();
            this.SuspendLayout();
            // 
            // edDx
            // 
            this.edDx.BackColor = System.Drawing.SystemColors.ButtonHighlight;
            this.edDx.Location = new System.Drawing.Point(24, 27);
            this.edDx.Name = "edDx";
            this.edDx.Size = new System.Drawing.Size(50, 20);
            this.edDx.TabIndex = 150;
            this.edDx.Text = "0";
            this.edDx.TextChanged += new System.EventHandler(this.edDx_TextChanged);
            // 
            // lbCoordPlano
            // 
            this.lbCoordPlano.AutoSize = true;
            this.lbCoordPlano.Location = new System.Drawing.Point(7, 34);
            this.lbCoordPlano.Name = "lbCoordPlano";
            this.lbCoordPlano.Size = new System.Drawing.Size(18, 13);
            this.lbCoordPlano.TabIndex = 151;
            this.lbCoordPlano.Text = "dx";
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(87, 34);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(18, 13);
            this.label1.TabIndex = 153;
            this.label1.Text = "dy";
            // 
            // edDy
            // 
            this.edDy.BackColor = System.Drawing.SystemColors.ButtonHighlight;
            this.edDy.Location = new System.Drawing.Point(104, 27);
            this.edDy.Name = "edDy";
            this.edDy.Size = new System.Drawing.Size(50, 20);
            this.edDy.TabIndex = 152;
            this.edDy.Text = "0";
            this.edDy.TextChanged += new System.EventHandler(this.edDx_TextChanged);
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(166, 34);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(18, 13);
            this.label2.TabIndex = 155;
            this.label2.Text = "dz";
            // 
            // edDz
            // 
            this.edDz.BackColor = System.Drawing.SystemColors.ButtonHighlight;
            this.edDz.Location = new System.Drawing.Point(183, 27);
            this.edDz.Name = "edDz";
            this.edDz.Size = new System.Drawing.Size(50, 20);
            this.edDz.TabIndex = 154;
            this.edDz.Text = "0";
            this.edDz.TextChanged += new System.EventHandler(this.edDx_TextChanged);
            // 
            // button2
            // 
            this.button2.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
          //  this.button2.Image = global::PGi.Properties.Resources.CONFIRMAR1;
            this.button2.Location = new System.Drawing.Point(194, 116);
            this.button2.Name = "button2";
            this.button2.Size = new System.Drawing.Size(48, 24);
            this.button2.TabIndex = 157;
            this.button2.UseVisualStyleBackColor = true;
            this.button2.Click += new System.EventHandler(this.button2_Click);
            // 
            // button1
            // 
            this.button1.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.button1.Image = global::PGi.Properties.Resources.cancelar4;
            this.button1.Location = new System.Drawing.Point(9, 116);
            this.button1.Name = "button1";
            this.button1.Size = new System.Drawing.Size(48, 24);
            this.button1.TabIndex = 156;
            this.button1.UseVisualStyleBackColor = true;
            this.button1.Click += new System.EventHandler(this.button1_Click);
            // 
            // numericUpDown1
            // 
            this.numericUpDown1.Location = new System.Drawing.Point(103, 79);
            this.numericUpDown1.Name = "numericUpDown1";
            this.numericUpDown1.Size = new System.Drawing.Size(61, 20);
            this.numericUpDown1.TabIndex = 158;
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(6, 86);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(93, 13);
            this.label3.TabIndex = 159;
            this.label3.Text = "Número de cópias";
            // 
            // chManual
            // 
            this.chManual.AutoSize = true;
            this.chManual.Location = new System.Drawing.Point(10, 4);
            this.chManual.Name = "chManual";
            this.chManual.Size = new System.Drawing.Size(90, 17);
            this.chManual.TabIndex = 160;
            this.chManual.Text = "Manualmente";
            this.chManual.UseVisualStyleBackColor = true;
            this.chManual.CheckedChanged += new System.EventHandler(this.checkBox1_CheckedChanged);
            // 
            // FMoverCopiar
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(245, 147);
            this.Controls.Add(this.chManual);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.numericUpDown1);
            this.Controls.Add(this.button2);
            this.Controls.Add(this.button1);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.edDz);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.edDy);
            this.Controls.Add(this.lbCoordPlano);
            this.Controls.Add(this.edDx);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedToolWindow;
            this.Name = "FMoverCopiar";
            this.Text = "Transformar";
            this.TopMost = true;
            ((System.ComponentModel.ISupportInitialize)(this.numericUpDown1)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        public System.Windows.Forms.TextBox edDx;
        public System.Windows.Forms.Label lbCoordPlano;
        public System.Windows.Forms.Label label1;
        public System.Windows.Forms.TextBox edDy;
        public System.Windows.Forms.Label label2;
        public System.Windows.Forms.TextBox edDz;
        private System.Windows.Forms.Button button2;
        private System.Windows.Forms.Button button1;
        private System.Windows.Forms.NumericUpDown numericUpDown1;
        public System.Windows.Forms.Label label3;
        public System.Windows.Forms.CheckBox chManual;
    }
}