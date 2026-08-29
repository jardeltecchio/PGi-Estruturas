namespace PG
{
    partial class FUnidades
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
            this.panel1 = new System.Windows.Forms.Panel();
            this.button3 = new System.Windows.Forms.Button();
            this.button1 = new System.Windows.Forms.Button();
            this.cbForca = new System.Windows.Forms.ComboBox();
            this.Força = new System.Windows.Forms.Label();
            this.Comprimento = new System.Windows.Forms.Label();
            this.cbComp = new System.Windows.Forms.ComboBox();
            this.label1 = new System.Windows.Forms.Label();
            this.cbArmadura = new System.Windows.Forms.ComboBox();
            this.Seção = new System.Windows.Forms.Label();
            this.cbSecao = new System.Windows.Forms.ComboBox();
            this.panel1.SuspendLayout();
            this.SuspendLayout();
            // 
            // panel1
            // 
            this.panel1.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.panel1.Controls.Add(this.button3);
            this.panel1.Controls.Add(this.button1);
            this.panel1.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.panel1.Location = new System.Drawing.Point(0, 78);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(351, 35);
            this.panel1.TabIndex = 7;
            // 
            // button3
            // 
            this.button3.Location = new System.Drawing.Point(223, 3);
            this.button3.Name = "button3";
            this.button3.Size = new System.Drawing.Size(115, 25);
            this.button3.TabIndex = 18;
            this.button3.Text = "Cancelar";
            this.button3.UseVisualStyleBackColor = true;
            this.button3.Click += new System.EventHandler(this.button3_Click);
            // 
            // button1
            // 
            this.button1.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.button1.Location = new System.Drawing.Point(4, 3);
            this.button1.Name = "button1";
            this.button1.Size = new System.Drawing.Size(115, 25);
            this.button1.TabIndex = 17;
            this.button1.Text = "Gravar";
            this.button1.UseVisualStyleBackColor = true;
            this.button1.Click += new System.EventHandler(this.button1_Click);
            // 
            // cbForca
            // 
            this.cbForca.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cbForca.FormattingEnabled = true;
            this.cbForca.Items.AddRange(new object[] {
            "N",
            "kN",
            "kgf",
            "tf"});
            this.cbForca.Location = new System.Drawing.Point(78, 12);
            this.cbForca.Name = "cbForca";
            this.cbForca.Size = new System.Drawing.Size(86, 21);
            this.cbForca.TabIndex = 8;
            // 
            // Força
            // 
            this.Força.AutoSize = true;
            this.Força.Location = new System.Drawing.Point(43, 20);
            this.Força.Name = "Força";
            this.Força.Size = new System.Drawing.Size(34, 13);
            this.Força.TabIndex = 9;
            this.Força.Text = "Força";
            // 
            // Comprimento
            // 
            this.Comprimento.AutoSize = true;
            this.Comprimento.Location = new System.Drawing.Point(9, 47);
            this.Comprimento.Name = "Comprimento";
            this.Comprimento.Size = new System.Drawing.Size(68, 13);
            this.Comprimento.TabIndex = 11;
            this.Comprimento.Text = "Comprimento";
            // 
            // cbComp
            // 
            this.cbComp.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cbComp.FormattingEnabled = true;
            this.cbComp.Items.AddRange(new object[] {
            "cm",
            "m"});
            this.cbComp.Location = new System.Drawing.Point(78, 39);
            this.cbComp.Name = "cbComp";
            this.cbComp.Size = new System.Drawing.Size(86, 21);
            this.cbComp.TabIndex = 10;
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(195, 47);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(52, 13);
            this.label1.TabIndex = 15;
            this.label1.Text = "Armadura";
            // 
            // cbArmadura
            // 
            this.cbArmadura.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cbArmadura.FormattingEnabled = true;
            this.cbArmadura.Items.AddRange(new object[] {
            "mm"});
            this.cbArmadura.Location = new System.Drawing.Point(251, 39);
            this.cbArmadura.Name = "cbArmadura";
            this.cbArmadura.Size = new System.Drawing.Size(86, 21);
            this.cbArmadura.TabIndex = 14;
            // 
            // Seção
            // 
            this.Seção.AutoSize = true;
            this.Seção.Location = new System.Drawing.Point(209, 20);
            this.Seção.Name = "Seção";
            this.Seção.Size = new System.Drawing.Size(38, 13);
            this.Seção.TabIndex = 13;
            this.Seção.Text = "Seção";
            // 
            // cbSecao
            // 
            this.cbSecao.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cbSecao.FormattingEnabled = true;
            this.cbSecao.Items.AddRange(new object[] {
            "cm",
            "m"});
            this.cbSecao.Location = new System.Drawing.Point(251, 12);
            this.cbSecao.Name = "cbSecao";
            this.cbSecao.Size = new System.Drawing.Size(86, 21);
            this.cbSecao.TabIndex = 12;
            // 
            // FUnidades
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(351, 113);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.cbArmadura);
            this.Controls.Add(this.Seção);
            this.Controls.Add(this.cbSecao);
            this.Controls.Add(this.Comprimento);
            this.Controls.Add(this.cbComp);
            this.Controls.Add(this.Força);
            this.Controls.Add(this.cbForca);
            this.Controls.Add(this.panel1);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedToolWindow;
            this.Name = "FUnidades";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "PGi - Unidades";
            this.Shown += new System.EventHandler(this.FUnidades_Shown);
            this.SizeChanged += new System.EventHandler(this.FUnidades_SizeChanged);
            this.panel1.ResumeLayout(false);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Panel panel1;
        private System.Windows.Forms.Button button3;
        private System.Windows.Forms.Button button1;
        private System.Windows.Forms.ComboBox cbForca;
        private System.Windows.Forms.Label Força;
        private System.Windows.Forms.Label Comprimento;
        private System.Windows.Forms.ComboBox cbComp;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.ComboBox cbArmadura;
        private System.Windows.Forms.Label Seção;
        private System.Windows.Forms.ComboBox cbSecao;
    }
}