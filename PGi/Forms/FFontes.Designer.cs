namespace PG
{
    partial class FFontes
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
            this.fontDialog1 = new System.Windows.Forms.FontDialog();
            this.tabControl1 = new System.Windows.Forms.TabControl();
            this.tabPage1 = new System.Windows.Forms.TabPage();
            this.tabPage2 = new System.Windows.Forms.TabPage();
            this.label1 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.label3 = new System.Windows.Forms.Label();
            this.label4 = new System.Windows.Forms.Label();
            this.button1 = new System.Windows.Forms.Button();
            this.button2 = new System.Windows.Forms.Button();
            this.button3 = new System.Windows.Forms.Button();
            this.button4 = new System.Windows.Forms.Button();
            this.NomeViga = new System.Windows.Forms.Label();
            this.SecaoViga = new System.Windows.Forms.Label();
            this.NomePilar = new System.Windows.Forms.Label();
            this.SecaoPilar = new System.Windows.Forms.Label();
            this.panel1 = new System.Windows.Forms.Panel();
            this.button7 = new System.Windows.Forms.Button();
            this.button8 = new System.Windows.Forms.Button();
            this.tabControl1.SuspendLayout();
            this.tabPage1.SuspendLayout();
            this.panel1.SuspendLayout();
            this.SuspendLayout();
            // 
            // tabControl1
            // 
            this.tabControl1.Controls.Add(this.tabPage1);
            this.tabControl1.Controls.Add(this.tabPage2);
            this.tabControl1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tabControl1.Location = new System.Drawing.Point(0, 0);
            this.tabControl1.Name = "tabControl1";
            this.tabControl1.SelectedIndex = 0;
            this.tabControl1.Size = new System.Drawing.Size(431, 387);
            this.tabControl1.TabIndex = 0;
            // 
            // tabPage1
            // 
            this.tabPage1.Controls.Add(this.SecaoPilar);
            this.tabPage1.Controls.Add(this.NomePilar);
            this.tabPage1.Controls.Add(this.SecaoViga);
            this.tabPage1.Controls.Add(this.NomeViga);
            this.tabPage1.Controls.Add(this.button4);
            this.tabPage1.Controls.Add(this.button3);
            this.tabPage1.Controls.Add(this.button2);
            this.tabPage1.Controls.Add(this.button1);
            this.tabPage1.Controls.Add(this.label3);
            this.tabPage1.Controls.Add(this.label4);
            this.tabPage1.Controls.Add(this.label2);
            this.tabPage1.Controls.Add(this.label1);
            this.tabPage1.Location = new System.Drawing.Point(4, 22);
            this.tabPage1.Name = "tabPage1";
            this.tabPage1.Padding = new System.Windows.Forms.Padding(3);
            this.tabPage1.Size = new System.Drawing.Size(423, 361);
            this.tabPage1.TabIndex = 0;
            this.tabPage1.Text = "Editor Gráfico";
            this.tabPage1.UseVisualStyleBackColor = true;
            // 
            // tabPage2
            // 
            this.tabPage2.Location = new System.Drawing.Point(4, 22);
            this.tabPage2.Name = "tabPage2";
            this.tabPage2.Padding = new System.Windows.Forms.Padding(3);
            this.tabPage2.Size = new System.Drawing.Size(423, 361);
            this.tabPage2.TabIndex = 1;
            this.tabPage2.Text = "Planta de Formas";
            this.tabPage2.UseVisualStyleBackColor = true;
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(8, 28);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(78, 13);
            this.label1.TabIndex = 0;
            this.label1.Text = "Nome de vigas";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(8, 53);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(81, 13);
            this.label2.TabIndex = 1;
            this.label2.Text = "Seção de vigas";
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(9, 101);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(86, 13);
            this.label3.TabIndex = 3;
            this.label3.Text = "Seção de pilares";
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(8, 77);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(83, 13);
            this.label4.TabIndex = 2;
            this.label4.Text = "Nome de pilares";
            // 
            // button1
            // 
            this.button1.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.button1.Location = new System.Drawing.Point(234, 23);
            this.button1.Name = "button1";
            this.button1.Size = new System.Drawing.Size(24, 21);
            this.button1.TabIndex = 42;
            this.button1.Text = "...";
            this.button1.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            this.button1.UseVisualStyleBackColor = true;
            this.button1.Click += new System.EventHandler(this.button1_Click);
            // 
            // button2
            // 
            this.button2.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.button2.Location = new System.Drawing.Point(234, 47);
            this.button2.Name = "button2";
            this.button2.Size = new System.Drawing.Size(24, 21);
            this.button2.TabIndex = 43;
            this.button2.Text = "...";
            this.button2.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            this.button2.UseVisualStyleBackColor = true;
            // 
            // button3
            // 
            this.button3.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.button3.Location = new System.Drawing.Point(234, 71);
            this.button3.Name = "button3";
            this.button3.Size = new System.Drawing.Size(24, 21);
            this.button3.TabIndex = 44;
            this.button3.Text = "...";
            this.button3.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            this.button3.UseVisualStyleBackColor = true;
            // 
            // button4
            // 
            this.button4.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.button4.Location = new System.Drawing.Point(234, 95);
            this.button4.Name = "button4";
            this.button4.Size = new System.Drawing.Size(24, 21);
            this.button4.TabIndex = 45;
            this.button4.Text = "...";
            this.button4.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            this.button4.UseVisualStyleBackColor = true;
            // 
            // NomeViga
            // 
            this.NomeViga.AutoSize = true;
            this.NomeViga.Location = new System.Drawing.Point(101, 28);
            this.NomeViga.Name = "NomeViga";
            this.NomeViga.Size = new System.Drawing.Size(78, 13);
            this.NomeViga.TabIndex = 46;
            this.NomeViga.Text = "Nome de vigas";
            // 
            // SecaoViga
            // 
            this.SecaoViga.AutoSize = true;
            this.SecaoViga.Location = new System.Drawing.Point(101, 53);
            this.SecaoViga.Name = "SecaoViga";
            this.SecaoViga.Size = new System.Drawing.Size(81, 13);
            this.SecaoViga.TabIndex = 47;
            this.SecaoViga.Text = "Seção de vigas";
            // 
            // NomePilar
            // 
            this.NomePilar.AutoSize = true;
            this.NomePilar.Location = new System.Drawing.Point(101, 77);
            this.NomePilar.Name = "NomePilar";
            this.NomePilar.Size = new System.Drawing.Size(83, 13);
            this.NomePilar.TabIndex = 48;
            this.NomePilar.Text = "Nome de pilares";
            // 
            // SecaoPilar
            // 
            this.SecaoPilar.AutoSize = true;
            this.SecaoPilar.Location = new System.Drawing.Point(101, 101);
            this.SecaoPilar.Name = "SecaoPilar";
            this.SecaoPilar.Size = new System.Drawing.Size(86, 13);
            this.SecaoPilar.TabIndex = 49;
            this.SecaoPilar.Text = "Seção de pilares";
            // 
            // panel1
            // 
            this.panel1.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.panel1.Controls.Add(this.button7);
            this.panel1.Controls.Add(this.button8);
            this.panel1.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.panel1.Location = new System.Drawing.Point(0, 355);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(431, 32);
            this.panel1.TabIndex = 42;
            // 
            // button7
            // 
            this.button7.BackColor = System.Drawing.Color.Transparent;
            this.button7.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.button7.Location = new System.Drawing.Point(323, 3);
            this.button7.Name = "button7";
            this.button7.Size = new System.Drawing.Size(100, 23);
            this.button7.TabIndex = 7;
            this.button7.Text = "Cancelar";
            this.button7.UseVisualStyleBackColor = false;
            this.button7.Click += new System.EventHandler(this.button7_Click_1);
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
            // 
            // FFontes
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(431, 387);
            this.Controls.Add(this.panel1);
            this.Controls.Add(this.tabControl1);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedToolWindow;
            this.Name = "FFontes";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "PGi - Alterar Fontes";
            this.TopMost = true;
            this.tabControl1.ResumeLayout(false);
            this.tabPage1.ResumeLayout(false);
            this.tabPage1.PerformLayout();
            this.panel1.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.FontDialog fontDialog1;
        private System.Windows.Forms.TabControl tabControl1;
        private System.Windows.Forms.TabPage tabPage1;
        private System.Windows.Forms.TabPage tabPage2;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.Button button4;
        private System.Windows.Forms.Button button3;
        private System.Windows.Forms.Button button2;
        private System.Windows.Forms.Button button1;
        private System.Windows.Forms.Label NomeViga;
        private System.Windows.Forms.Label SecaoPilar;
        private System.Windows.Forms.Label NomePilar;
        private System.Windows.Forms.Label SecaoViga;
        private System.Windows.Forms.Panel panel1;
        private System.Windows.Forms.Button button7;
        private System.Windows.Forms.Button button8;
    }
}