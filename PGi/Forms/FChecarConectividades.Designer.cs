namespace PG
{
    partial class ChecarConectividades
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(ChecarConectividades));
            this.panel1 = new System.Windows.Forms.Panel();
            this.button3 = new System.Windows.Forms.Button();
            this.imageList2 = new System.Windows.Forms.ImageList(this.components);
            this.btSalvar = new System.Windows.Forms.Button();
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.checarConectividade = new System.Windows.Forms.CheckBox();
            this.label7 = new System.Windows.Forms.Label();
            this.groupBox2 = new System.Windows.Forms.GroupBox();
            this.conexoesPerdidas = new System.Windows.Forms.CheckBox();
            this.label1 = new System.Windows.Forms.Label();
            this.label54 = new System.Windows.Forms.Label();
            this.label55 = new System.Windows.Forms.Label();
            this.tolerancia = new System.Windows.Forms.TextBox();
            this.resultado = new System.Windows.Forms.Label();
            this.panel1.SuspendLayout();
            this.groupBox1.SuspendLayout();
            this.groupBox2.SuspendLayout();
            this.SuspendLayout();
            // 
            // panel1
            // 
            this.panel1.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.panel1.Controls.Add(this.button3);
            this.panel1.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.panel1.Location = new System.Drawing.Point(0, 340);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(237, 34);
            this.panel1.TabIndex = 64;
            // 
            // button3
            // 
            this.button3.BackColor = System.Drawing.Color.Transparent;
            this.button3.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.button3.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.button3.ImageIndex = 1;
            this.button3.ImageList = this.imageList2;
            this.button3.Location = new System.Drawing.Point(163, 3);
            this.button3.Name = "button3";
            this.button3.Size = new System.Drawing.Size(67, 25);
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
            this.btSalvar.FlatAppearance.MouseDownBackColor = System.Drawing.Color.Transparent;
            this.btSalvar.FlatAppearance.MouseOverBackColor = System.Drawing.Color.Transparent;
            this.btSalvar.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btSalvar.Font = new System.Drawing.Font("Microsoft Sans Serif", 10F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btSalvar.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.btSalvar.ImageIndex = 0;
            this.btSalvar.ImageList = this.imageList2;
            this.btSalvar.Location = new System.Drawing.Point(75, 253);
            this.btSalvar.Name = "btSalvar";
            this.btSalvar.Size = new System.Drawing.Size(89, 37);
            this.btSalvar.TabIndex = 86;
            this.btSalvar.Text = "Checar";
            this.btSalvar.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            this.btSalvar.UseVisualStyleBackColor = false;
            this.btSalvar.Click += new System.EventHandler(this.btSalvar_Click);
            // 
            // groupBox1
            // 
            this.groupBox1.Controls.Add(this.checarConectividade);
            this.groupBox1.Controls.Add(this.label7);
            this.groupBox1.Location = new System.Drawing.Point(12, 7);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(219, 89);
            this.groupBox1.TabIndex = 90;
            this.groupBox1.TabStop = false;
            this.groupBox1.Text = "Conexões";
            // 
            // checarConectividade
            // 
            this.checarConectividade.AutoSize = true;
            this.checarConectividade.Checked = true;
            this.checarConectividade.CheckState = System.Windows.Forms.CheckState.Checked;
            this.checarConectividade.Location = new System.Drawing.Point(6, 19);
            this.checarConectividade.Name = "checarConectividade";
            this.checarConectividade.Size = new System.Drawing.Size(130, 17);
            this.checarConectividade.TabIndex = 88;
            this.checarConectividade.Text = "Checar conectividade";
            this.checarConectividade.UseVisualStyleBackColor = true;
            this.checarConectividade.CheckedChanged += new System.EventHandler(this.checarConectividade_CheckedChanged);
            // 
            // label7
            // 
            this.label7.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label7.ForeColor = System.Drawing.Color.Blue;
            this.label7.Location = new System.Drawing.Point(6, 41);
            this.label7.Name = "label7";
            this.label7.Size = new System.Drawing.Size(209, 41);
            this.label7.TabIndex = 86;
            this.label7.Text = "Essa opção realça todos os elementos que estão conectados ao elemento selecionado" +
    ".";
            // 
            // groupBox2
            // 
            this.groupBox2.Controls.Add(this.conexoesPerdidas);
            this.groupBox2.Controls.Add(this.label1);
            this.groupBox2.Controls.Add(this.label54);
            this.groupBox2.Controls.Add(this.label55);
            this.groupBox2.Controls.Add(this.tolerancia);
            this.groupBox2.Location = new System.Drawing.Point(12, 115);
            this.groupBox2.Name = "groupBox2";
            this.groupBox2.Size = new System.Drawing.Size(219, 135);
            this.groupBox2.TabIndex = 91;
            this.groupBox2.TabStop = false;
            this.groupBox2.Text = "Conexões perdidas";
            // 
            // conexoesPerdidas
            // 
            this.conexoesPerdidas.AutoSize = true;
            this.conexoesPerdidas.Location = new System.Drawing.Point(6, 19);
            this.conexoesPerdidas.Name = "conexoesPerdidas";
            this.conexoesPerdidas.Size = new System.Drawing.Size(158, 17);
            this.conexoesPerdidas.TabIndex = 95;
            this.conexoesPerdidas.Text = "Procurar conexões perdidas";
            this.conexoesPerdidas.UseVisualStyleBackColor = true;
            this.conexoesPerdidas.CheckedChanged += new System.EventHandler(this.conexoesPerdidas_CheckedChanged_1);
            // 
            // label1
            // 
            this.label1.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label1.ForeColor = System.Drawing.Color.Blue;
            this.label1.Location = new System.Drawing.Point(7, 69);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(206, 61);
            this.label1.TabIndex = 94;
            this.label1.Text = "Essa opção realça todos os nós que se encontram muito próximos de outros nós ou e" +
    "lementos sem estarem conectados à eles.\r\n";
            // 
            // label54
            // 
            this.label54.AutoSize = true;
            this.label54.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label54.ForeColor = System.Drawing.Color.Black;
            this.label54.Location = new System.Drawing.Point(127, 49);
            this.label54.Name = "label54";
            this.label54.Size = new System.Drawing.Size(25, 13);
            this.label54.TabIndex = 92;
            this.label54.Text = "mm";
            // 
            // label55
            // 
            this.label55.AutoSize = true;
            this.label55.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label55.ForeColor = System.Drawing.Color.Black;
            this.label55.Location = new System.Drawing.Point(11, 49);
            this.label55.Name = "label55";
            this.label55.Size = new System.Drawing.Size(63, 13);
            this.label55.TabIndex = 91;
            this.label55.Text = "tolerância";
            // 
            // tolerancia
            // 
            this.tolerancia.Location = new System.Drawing.Point(76, 42);
            this.tolerancia.Name = "tolerancia";
            this.tolerancia.Size = new System.Drawing.Size(45, 20);
            this.tolerancia.TabIndex = 90;
            this.tolerancia.Text = "10";
            this.tolerancia.KeyPress += new System.Windows.Forms.KeyPressEventHandler(this.tolerancia_KeyPress);
            // 
            // resultado
            // 
            this.resultado.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.resultado.ForeColor = System.Drawing.Color.Black;
            this.resultado.Location = new System.Drawing.Point(0, 306);
            this.resultado.Name = "resultado";
            this.resultado.Size = new System.Drawing.Size(241, 31);
            this.resultado.TabIndex = 92;
            this.resultado.Text = "<>";
            // 
            // ChecarConectividades
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.CancelButton = this.button3;
            this.ClientSize = new System.Drawing.Size(237, 374);
            this.Controls.Add(this.resultado);
            this.Controls.Add(this.groupBox2);
            this.Controls.Add(this.groupBox1);
            this.Controls.Add(this.btSalvar);
            this.Controls.Add(this.panel1);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedToolWindow;
            this.Name = "ChecarConectividades";
            this.StartPosition = System.Windows.Forms.FormStartPosition.Manual;
            this.Text = "Checagem estrutural";
            this.FormClosed += new System.Windows.Forms.FormClosedEventHandler(this.ChecarConectividades_FormClosed);
            this.panel1.ResumeLayout(false);
            this.groupBox1.ResumeLayout(false);
            this.groupBox1.PerformLayout();
            this.groupBox2.ResumeLayout(false);
            this.groupBox2.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion
        private System.Windows.Forms.Panel panel1;
        private System.Windows.Forms.Button button3;
        public System.Windows.Forms.ImageList imageList2;
        public System.Windows.Forms.Button btSalvar;
        private System.Windows.Forms.GroupBox groupBox1;
        private System.Windows.Forms.Label label7;
        private System.Windows.Forms.GroupBox groupBox2;
        private System.Windows.Forms.Label label54;
        private System.Windows.Forms.Label label55;
        public System.Windows.Forms.TextBox tolerancia;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.CheckBox checarConectividade;
        private System.Windows.Forms.CheckBox conexoesPerdidas;
        private System.Windows.Forms.Label resultado;
    }
}