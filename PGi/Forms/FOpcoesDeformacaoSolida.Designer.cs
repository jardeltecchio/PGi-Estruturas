namespace PG
{
    partial class FOpcoesDeformacaoSolida
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(FOpcoesDeformacaoSolida));
            this.imageList1 = new System.Windows.Forms.ImageList(this.components);
            this.colorDialog1 = new System.Windows.Forms.ColorDialog();
            this.panel1 = new System.Windows.Forms.Panel();
            this.button3 = new System.Windows.Forms.Button();
            this.button1 = new System.Windows.Forms.Button();
            this.checkBox1 = new System.Windows.Forms.CheckBox();
            this.panel1.SuspendLayout();
            this.SuspendLayout();
            // 
            // imageList1
            // 
            this.imageList1.ImageStream = ((System.Windows.Forms.ImageListStreamer)(resources.GetObject("imageList1.ImageStream")));
            this.imageList1.TransparentColor = System.Drawing.Color.Transparent;
            this.imageList1.Images.SetKeyName(0, "excluir.bmp");
            this.imageList1.Images.SetKeyName(1, "ok.bmp");
            this.imageList1.Images.SetKeyName(2, "arco mif.bmp");
            this.imageList1.Images.SetKeyName(3, "face insercao 1.png");
            this.imageList1.Images.SetKeyName(4, "face insercao 2.png");
            this.imageList1.Images.SetKeyName(5, "face insercao 3.png");
            this.imageList1.Images.SetKeyName(6, "insercao arco.png");
            this.imageList1.Images.SetKeyName(7, "SECAO CONCRETO.bmp");
            this.imageList1.Images.SetKeyName(8, "cancelar.bmp");
            this.imageList1.Images.SetKeyName(9, "CONFIRMAR.bmp");
            this.imageList1.Images.SetKeyName(10, "PERFIL LAMINADO.bmp");
            this.imageList1.Images.SetKeyName(11, "PERFIL MACIÇO.bmp");
            this.imageList1.Images.SetKeyName(12, "PERFIL USUARIO.bmp");
            this.imageList1.Images.SetKeyName(13, "Erase.bmp");
            this.imageList1.Images.SetKeyName(14, "PERFIL DOBRADO.bmp");
            // 
            // panel1
            // 
            this.panel1.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.panel1.Controls.Add(this.button3);
            this.panel1.Controls.Add(this.button1);
            this.panel1.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.panel1.Location = new System.Drawing.Point(0, 227);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(321, 35);
            this.panel1.TabIndex = 22;
            // 
            // button3
            // 
            this.button3.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.button3.ImageIndex = 9;
            this.button3.ImageList = this.imageList1;
            this.button3.Location = new System.Drawing.Point(273, 3);
            this.button3.Name = "button3";
            this.button3.Size = new System.Drawing.Size(38, 25);
            this.button3.TabIndex = 18;
            this.button3.UseVisualStyleBackColor = true;
            this.button3.Click += new System.EventHandler(this.button3_Click);
            // 
            // button1
            // 
            this.button1.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.button1.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.button1.ImageIndex = 8;
            this.button1.ImageList = this.imageList1;
            this.button1.Location = new System.Drawing.Point(4, 3);
            this.button1.Name = "button1";
            this.button1.Size = new System.Drawing.Size(38, 25);
            this.button1.TabIndex = 17;
            this.button1.UseVisualStyleBackColor = true;
            // 
            // checkBox1
            // 
            this.checkBox1.AutoSize = true;
            this.checkBox1.Location = new System.Drawing.Point(12, 12);
            this.checkBox1.Name = "checkBox1";
            this.checkBox1.Size = new System.Drawing.Size(151, 17);
            this.checkBox1.TabIndex = 23;
            this.checkBox1.Text = "Mostrar linhas de contorno";
            this.checkBox1.UseVisualStyleBackColor = true;
            // 
            // FOpcoesDeformacaoSolida
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(321, 262);
            this.Controls.Add(this.checkBox1);
            this.Controls.Add(this.panel1);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedToolWindow;
            this.Name = "FOpcoesDeformacaoSolida";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "Opções de deformação sólida";
            this.FormClosed += new System.Windows.Forms.FormClosedEventHandler(this.FOpcoesDeformacaoSolida_FormClosed);
            this.Move += new System.EventHandler(this.FOpcoesDeformacaoSolida_Move);
            this.panel1.ResumeLayout(false);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        public System.Windows.Forms.ImageList imageList1;
        private System.Windows.Forms.ColorDialog colorDialog1;
        private System.Windows.Forms.Panel panel1;
        private System.Windows.Forms.Button button3;
        private System.Windows.Forms.Button button1;
        private System.Windows.Forms.CheckBox checkBox1;
    }
}