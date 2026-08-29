namespace PG
{
    partial class FCopiarPavimento
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(FCopiarPavimento));
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.lbDe = new System.Windows.Forms.ListBox();
            this.groupBox2 = new System.Windows.Forms.GroupBox();
            this.lbPara = new System.Windows.Forms.ListBox();
            this.groupBox3 = new System.Windows.Forms.GroupBox();
            this.chCopiarDesenhos = new System.Windows.Forms.CheckBox();
            this.chCopiarLajes = new System.Windows.Forms.CheckBox();
            this.chCopiarPilares = new System.Windows.Forms.CheckBox();
            this.chCopiarVigas = new System.Windows.Forms.CheckBox();
            this.panel1 = new System.Windows.Forms.Panel();
            this.button7 = new System.Windows.Forms.Button();
            this.button6 = new System.Windows.Forms.Button();
            this.label6 = new System.Windows.Forms.Label();
            this.label1 = new System.Windows.Forms.Label();
            this.groupBox1.SuspendLayout();
            this.groupBox2.SuspendLayout();
            this.groupBox3.SuspendLayout();
            this.panel1.SuspendLayout();
            this.SuspendLayout();
            // 
            // groupBox1
            // 
            this.groupBox1.Controls.Add(this.lbDe);
            this.groupBox1.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.groupBox1.Location = new System.Drawing.Point(159, 12);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(145, 290);
            this.groupBox1.TabIndex = 31;
            this.groupBox1.TabStop = false;
            this.groupBox1.Text = "De:";
            // 
            // lbDe
            // 
            this.lbDe.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.lbDe.Dock = System.Windows.Forms.DockStyle.Fill;
            this.lbDe.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lbDe.FormattingEnabled = true;
            this.lbDe.HorizontalScrollbar = true;
            this.lbDe.Location = new System.Drawing.Point(3, 16);
            this.lbDe.Name = "lbDe";
            this.lbDe.ScrollAlwaysVisible = true;
            this.lbDe.Size = new System.Drawing.Size(139, 271);
            this.lbDe.TabIndex = 28;
            this.lbDe.SelectedIndexChanged += new System.EventHandler(this.lbDe_SelectedIndexChanged);
            // 
            // groupBox2
            // 
            this.groupBox2.Controls.Add(this.lbPara);
            this.groupBox2.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.groupBox2.Location = new System.Drawing.Point(327, 12);
            this.groupBox2.Name = "groupBox2";
            this.groupBox2.Size = new System.Drawing.Size(157, 290);
            this.groupBox2.TabIndex = 32;
            this.groupBox2.TabStop = false;
            this.groupBox2.Text = "Para:";
            // 
            // lbPara
            // 
            this.lbPara.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.lbPara.Dock = System.Windows.Forms.DockStyle.Fill;
            this.lbPara.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lbPara.FormattingEnabled = true;
            this.lbPara.HorizontalScrollbar = true;
            this.lbPara.Location = new System.Drawing.Point(3, 16);
            this.lbPara.Name = "lbPara";
            this.lbPara.ScrollAlwaysVisible = true;
            this.lbPara.SelectionMode = System.Windows.Forms.SelectionMode.MultiExtended;
            this.lbPara.Size = new System.Drawing.Size(151, 271);
            this.lbPara.TabIndex = 29;
            // 
            // groupBox3
            // 
            this.groupBox3.Controls.Add(this.chCopiarDesenhos);
            this.groupBox3.Controls.Add(this.chCopiarLajes);
            this.groupBox3.Controls.Add(this.chCopiarPilares);
            this.groupBox3.Controls.Add(this.chCopiarVigas);
            this.groupBox3.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.groupBox3.Location = new System.Drawing.Point(12, 12);
            this.groupBox3.Name = "groupBox3";
            this.groupBox3.Size = new System.Drawing.Size(126, 118);
            this.groupBox3.TabIndex = 37;
            this.groupBox3.TabStop = false;
            this.groupBox3.Text = "O que copiar";
            // 
            // chCopiarDesenhos
            // 
            this.chCopiarDesenhos.AutoSize = true;
            this.chCopiarDesenhos.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.chCopiarDesenhos.Location = new System.Drawing.Point(6, 94);
            this.chCopiarDesenhos.Name = "chCopiarDesenhos";
            this.chCopiarDesenhos.Size = new System.Drawing.Size(117, 17);
            this.chCopiarDesenhos.TabIndex = 40;
            this.chCopiarDesenhos.Text = "Desenhos externos";
            this.chCopiarDesenhos.UseVisualStyleBackColor = true;
            // 
            // chCopiarLajes
            // 
            this.chCopiarLajes.AutoSize = true;
            this.chCopiarLajes.Checked = true;
            this.chCopiarLajes.CheckState = System.Windows.Forms.CheckState.Checked;
            this.chCopiarLajes.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.chCopiarLajes.Location = new System.Drawing.Point(6, 71);
            this.chCopiarLajes.Name = "chCopiarLajes";
            this.chCopiarLajes.Size = new System.Drawing.Size(51, 17);
            this.chCopiarLajes.TabIndex = 39;
            this.chCopiarLajes.Text = "Lajes";
            this.chCopiarLajes.UseVisualStyleBackColor = true;
            // 
            // chCopiarPilares
            // 
            this.chCopiarPilares.AutoSize = true;
            this.chCopiarPilares.Checked = true;
            this.chCopiarPilares.CheckState = System.Windows.Forms.CheckState.Checked;
            this.chCopiarPilares.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.chCopiarPilares.Location = new System.Drawing.Point(6, 48);
            this.chCopiarPilares.Name = "chCopiarPilares";
            this.chCopiarPilares.Size = new System.Drawing.Size(57, 17);
            this.chCopiarPilares.TabIndex = 38;
            this.chCopiarPilares.Text = "Pilares";
            this.chCopiarPilares.UseVisualStyleBackColor = true;
            // 
            // chCopiarVigas
            // 
            this.chCopiarVigas.AutoSize = true;
            this.chCopiarVigas.Checked = true;
            this.chCopiarVigas.CheckState = System.Windows.Forms.CheckState.Checked;
            this.chCopiarVigas.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.chCopiarVigas.Location = new System.Drawing.Point(6, 25);
            this.chCopiarVigas.Name = "chCopiarVigas";
            this.chCopiarVigas.Size = new System.Drawing.Size(52, 17);
            this.chCopiarVigas.TabIndex = 37;
            this.chCopiarVigas.Text = "Vigas";
            this.chCopiarVigas.UseVisualStyleBackColor = true;
            this.chCopiarVigas.CheckedChanged += new System.EventHandler(this.chCopiarVigas_CheckedChanged);
            // 
            // panel1
            // 
            this.panel1.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.panel1.Controls.Add(this.button7);
            this.panel1.Controls.Add(this.button6);
            this.panel1.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.panel1.Location = new System.Drawing.Point(0, 422);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(493, 32);
            this.panel1.TabIndex = 38;
            // 
            // button7
            // 
            this.button7.BackColor = System.Drawing.SystemColors.ControlLightLight;
            this.button7.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            //this.button7.Image = global::PGi.Properties.Resources.cancelar;
            this.button7.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.button7.Location = new System.Drawing.Point(388, 3);
            this.button7.Name = "button7";
            this.button7.Size = new System.Drawing.Size(100, 23);
            this.button7.TabIndex = 7;
            this.button7.Text = "Cancelar";
            this.button7.UseVisualStyleBackColor = false;
            this.button7.Click += new System.EventHandler(this.button7_Click);
            // 
            // button6
            // 
            this.button6.BackColor = System.Drawing.SystemColors.Window;
            this.button6.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.button6.Image = ((System.Drawing.Image)(resources.GetObject("button6.Image")));
            this.button6.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.button6.Location = new System.Drawing.Point(3, 4);
            this.button6.Name = "button6";
            this.button6.Size = new System.Drawing.Size(100, 23);
            this.button6.TabIndex = 6;
            this.button6.Text = "Copiar";
            this.button6.UseVisualStyleBackColor = false;
            this.button6.Click += new System.EventHandler(this.button6_Click);
            // 
            // label6
            // 
            this.label6.AutoSize = true;
            this.label6.Font = new System.Drawing.Font("Tahoma", 8F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label6.ForeColor = System.Drawing.Color.Red;
            this.label6.Location = new System.Drawing.Point(9, 406);
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size(400, 13);
            this.label6.TabIndex = 39;
            this.label6.Text = "Esse recurso irá apagar os elementos selecionados do(s) pavimento(s) de destino ";
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Font = new System.Drawing.Font("Tahoma", 8F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label1.ForeColor = System.Drawing.Color.Red;
            this.label1.Location = new System.Drawing.Point(1, 393);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(101, 13);
            this.label1.TabIndex = 40;
            this.label1.Text = "**Importante**";
            // 
            // FCopiarPavimento
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(493, 454);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.label6);
            this.Controls.Add(this.panel1);
            this.Controls.Add(this.groupBox3);
            this.Controls.Add(this.groupBox2);
            this.Controls.Add(this.groupBox1);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedToolWindow;
            this.Name = "FCopiarPavimento";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "PGi - Copiar pavimento";
            this.TopMost = true;
            this.groupBox1.ResumeLayout(false);
            this.groupBox2.ResumeLayout(false);
            this.groupBox3.ResumeLayout(false);
            this.groupBox3.PerformLayout();
            this.panel1.ResumeLayout(false);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.GroupBox groupBox1;
        private System.Windows.Forms.ListBox lbDe;
        private System.Windows.Forms.GroupBox groupBox2;
        private System.Windows.Forms.ListBox lbPara;
        private System.Windows.Forms.GroupBox groupBox3;
        private System.Windows.Forms.CheckBox chCopiarDesenhos;
        private System.Windows.Forms.CheckBox chCopiarLajes;
        private System.Windows.Forms.CheckBox chCopiarPilares;
        private System.Windows.Forms.CheckBox chCopiarVigas;
        private System.Windows.Forms.Panel panel1;
        private System.Windows.Forms.Button button7;
        private System.Windows.Forms.Button button6;
        private System.Windows.Forms.Label label6;
        private System.Windows.Forms.Label label1;

    }
}