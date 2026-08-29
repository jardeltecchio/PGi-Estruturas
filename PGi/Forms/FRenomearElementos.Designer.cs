namespace PG
{
    partial class FRenomearElementos
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
            this.NovoPrefixo = new System.Windows.Forms.TextBox();
            this.panel1 = new System.Windows.Forms.Panel();
            this.button7 = new System.Windows.Forms.Button();
            this.button8 = new System.Windows.Forms.Button();
            this.NumeroInicial = new System.Windows.Forms.TextBox();
            this.label1 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.chManterPrefixo = new System.Windows.Forms.CheckBox();
            this.chManterNumero = new System.Windows.Forms.CheckBox();
            this.panel1.SuspendLayout();
            this.SuspendLayout();
            // 
            // NovoPrefixo
            // 
            this.NovoPrefixo.Enabled = false;
            this.NovoPrefixo.Location = new System.Drawing.Point(97, 20);
            this.NovoPrefixo.Name = "NovoPrefixo";
            this.NovoPrefixo.Size = new System.Drawing.Size(63, 20);
            this.NovoPrefixo.TabIndex = 0;
            // 
            // panel1
            // 
            this.panel1.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.panel1.Controls.Add(this.button7);
            this.panel1.Controls.Add(this.button8);
            this.panel1.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.panel1.Location = new System.Drawing.Point(0, 129);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(308, 32);
            this.panel1.TabIndex = 40;
            // 
            // button7
            // 
            this.button7.BackColor = System.Drawing.Color.Transparent;
            this.button7.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.button7.Location = new System.Drawing.Point(188, 2);
            this.button7.Name = "button7";
            this.button7.Size = new System.Drawing.Size(115, 25);
            this.button7.TabIndex = 7;
            this.button7.Text = "Cancelar";
            this.button7.UseVisualStyleBackColor = false;
            this.button7.Click += new System.EventHandler(this.button7_Click);
            // 
            // button8
            // 
            this.button8.BackColor = System.Drawing.Color.Transparent;
            this.button8.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.button8.Location = new System.Drawing.Point(3, 2);
            this.button8.Name = "button8";
            this.button8.Size = new System.Drawing.Size(115, 25);
            this.button8.TabIndex = 6;
            this.button8.Text = "Ok";
            this.button8.UseVisualStyleBackColor = false;
            this.button8.Click += new System.EventHandler(this.button8_Click);
            // 
            // NumeroInicial
            // 
            this.NumeroInicial.Location = new System.Drawing.Point(97, 59);
            this.NumeroInicial.Name = "NumeroInicial";
            this.NumeroInicial.Size = new System.Drawing.Size(63, 20);
            this.NumeroInicial.TabIndex = 41;
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(3, 27);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(67, 13);
            this.label1.TabIndex = 42;
            this.label1.Text = "Novo prefixo";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(3, 66);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(88, 13);
            this.label2.TabIndex = 43;
            this.label2.Text = "Número de início";
            // 
            // chManterPrefixo
            // 
            this.chManterPrefixo.AutoSize = true;
            this.chManterPrefixo.Checked = true;
            this.chManterPrefixo.CheckState = System.Windows.Forms.CheckState.Checked;
            this.chManterPrefixo.Location = new System.Drawing.Point(166, 25);
            this.chManterPrefixo.Name = "chManterPrefixo";
            this.chManterPrefixo.Size = new System.Drawing.Size(119, 17);
            this.chManterPrefixo.TabIndex = 44;
            this.chManterPrefixo.Text = "Manter prefixo atual";
            this.chManterPrefixo.UseVisualStyleBackColor = true;
            this.chManterPrefixo.CheckedChanged += new System.EventHandler(this.chManterPrefixo_CheckedChanged);
            // 
            // chManterNumero
            // 
            this.chManterNumero.AutoSize = true;
            this.chManterNumero.Location = new System.Drawing.Point(167, 64);
            this.chManterNumero.Name = "chManterNumero";
            this.chManterNumero.Size = new System.Drawing.Size(141, 17);
            this.chManterNumero.TabIndex = 45;
            this.chManterNumero.Text = "Manter numeração atual";
            this.chManterNumero.UseVisualStyleBackColor = true;
            this.chManterNumero.CheckedChanged += new System.EventHandler(this.chManterNumero_CheckedChanged);
            // 
            // FRenomearElementos
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(308, 161);
            this.Controls.Add(this.chManterNumero);
            this.Controls.Add(this.chManterPrefixo);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.NumeroInicial);
            this.Controls.Add(this.panel1);
            this.Controls.Add(this.NovoPrefixo);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedToolWindow;
            this.Name = "FRenomearElementos";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "PGi - Renomear";
            this.TopMost = true;
            this.panel1.ResumeLayout(false);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Panel panel1;
        private System.Windows.Forms.Button button7;
        private System.Windows.Forms.Button button8;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.CheckBox chManterPrefixo;
        private System.Windows.Forms.CheckBox chManterNumero;
        public System.Windows.Forms.TextBox NovoPrefixo;
        public System.Windows.Forms.TextBox NumeroInicial;
    }
}