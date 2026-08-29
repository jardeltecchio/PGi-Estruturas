namespace PG
{
    partial class FLayersEscolheCor
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
            this.colorDialog1 = new System.Windows.Forms.ColorDialog();
            this.btCor = new System.Windows.Forms.Button();
            this.button1 = new System.Windows.Forms.Button();
            this.label1 = new System.Windows.Forms.Label();
            this.panel1 = new System.Windows.Forms.Panel();
            this.button7 = new System.Windows.Forms.Button();
            this.button8 = new System.Windows.Forms.Button();
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.rbTravado = new System.Windows.Forms.RadioButton();
            this.rbCongelado = new System.Windows.Forms.RadioButton();
            this.rbLigado = new System.Windows.Forms.RadioButton();
            this.panel1.SuspendLayout();
            this.groupBox1.SuspendLayout();
            this.SuspendLayout();
            // 
            // btCor
            // 
            this.btCor.Enabled = false;
            this.btCor.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btCor.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btCor.Location = new System.Drawing.Point(35, 12);
            this.btCor.Name = "btCor";
            this.btCor.Size = new System.Drawing.Size(86, 18);
            this.btCor.TabIndex = 0;
            this.btCor.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            this.btCor.UseVisualStyleBackColor = true;
            // 
            // button1
            // 
            this.button1.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.button1.Location = new System.Drawing.Point(125, 10);
            this.button1.Name = "button1";
            this.button1.Size = new System.Drawing.Size(24, 21);
            this.button1.TabIndex = 1;
            this.button1.Text = "...";
            this.button1.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            this.button1.UseVisualStyleBackColor = true;
            this.button1.Click += new System.EventHandler(this.button1_Click);
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(6, 16);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(23, 13);
            this.label1.TabIndex = 2;
            this.label1.Text = "Cor";
            // 
            // panel1
            // 
            this.panel1.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.panel1.Controls.Add(this.groupBox1);
            this.panel1.Controls.Add(this.button7);
            this.panel1.Controls.Add(this.button8);
            this.panel1.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.panel1.Location = new System.Drawing.Point(0, 44);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(236, 32);
            this.panel1.TabIndex = 40;
            // 
            // button7
            // 
            this.button7.BackColor = System.Drawing.Color.Transparent;
            this.button7.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.button7.Location = new System.Drawing.Point(114, 4);
            this.button7.Name = "button7";
            this.button7.Size = new System.Drawing.Size(100, 23);
            this.button7.TabIndex = 7;
            this.button7.Text = "Cancelar";
            this.button7.UseVisualStyleBackColor = false;
            this.button7.Click += new System.EventHandler(this.button7_Click);
            // 
            // button8
            // 
            this.button8.BackColor = System.Drawing.Color.Transparent;
            this.button8.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.button8.Location = new System.Drawing.Point(3, 4);
            this.button8.Name = "button8";
            this.button8.Size = new System.Drawing.Size(100, 23);
            this.button8.TabIndex = 6;
            this.button8.Text = "Gravar";
            this.button8.UseVisualStyleBackColor = false;
            this.button8.Click += new System.EventHandler(this.button8_Click);
            // 
            // groupBox1
            // 
            this.groupBox1.Controls.Add(this.rbTravado);
            this.groupBox1.Controls.Add(this.rbCongelado);
            this.groupBox1.Controls.Add(this.rbLigado);
            this.groupBox1.Location = new System.Drawing.Point(220, 4);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(259, 45);
            this.groupBox1.TabIndex = 44;
            this.groupBox1.TabStop = false;
            this.groupBox1.Text = "Status";
            this.groupBox1.Visible = false;
            // 
            // rbTravado
            // 
            this.rbTravado.AutoSize = true;
            this.rbTravado.Location = new System.Drawing.Point(188, 19);
            this.rbTravado.Name = "rbTravado";
            this.rbTravado.Size = new System.Drawing.Size(65, 17);
            this.rbTravado.TabIndex = 46;
            this.rbTravado.TabStop = true;
            this.rbTravado.Text = "Travado";
            this.rbTravado.UseVisualStyleBackColor = true;
            // 
            // rbCongelado
            // 
            this.rbCongelado.AutoSize = true;
            this.rbCongelado.Location = new System.Drawing.Point(88, 19);
            this.rbCongelado.Name = "rbCongelado";
            this.rbCongelado.Size = new System.Drawing.Size(76, 17);
            this.rbCongelado.TabIndex = 45;
            this.rbCongelado.TabStop = true;
            this.rbCongelado.Text = "Congelado";
            this.rbCongelado.UseVisualStyleBackColor = true;
            // 
            // rbLigado
            // 
            this.rbLigado.AutoSize = true;
            this.rbLigado.Location = new System.Drawing.Point(6, 19);
            this.rbLigado.Name = "rbLigado";
            this.rbLigado.Size = new System.Drawing.Size(57, 17);
            this.rbLigado.TabIndex = 44;
            this.rbLigado.TabStop = true;
            this.rbLigado.Text = "Ligado";
            this.rbLigado.UseVisualStyleBackColor = true;
            // 
            // FLayersEscolheCor
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(236, 76);
            this.Controls.Add(this.panel1);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.button1);
            this.Controls.Add(this.btCor);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedToolWindow;
            this.Name = "FLayersEscolheCor";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "PGi - Layers";
            this.TopMost = true;
            this.panel1.ResumeLayout(false);
            this.groupBox1.ResumeLayout(false);
            this.groupBox1.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.ColorDialog colorDialog1;
        private System.Windows.Forms.Button button1;
        public System.Windows.Forms.Button btCor;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Panel panel1;
        private System.Windows.Forms.Button button7;
        private System.Windows.Forms.Button button8;
        private System.Windows.Forms.GroupBox groupBox1;
        public System.Windows.Forms.RadioButton rbTravado;
        public System.Windows.Forms.RadioButton rbCongelado;
        public System.Windows.Forms.RadioButton rbLigado;
    }
}