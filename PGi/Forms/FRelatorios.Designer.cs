namespace PG
{
    partial class FRelatorios
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(FRelatorios));
            this.edRelatorio = new System.Windows.Forms.TextBox();
            this.imageList3 = new System.Windows.Forms.ImageList(this.components);
            this.panel1 = new System.Windows.Forms.Panel();
            this.button3 = new System.Windows.Forms.Button();
            this.panel1.SuspendLayout();
            this.SuspendLayout();
            // 
            // edRelatorio
            // 
            this.edRelatorio.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.edRelatorio.Font = new System.Drawing.Font("Consolas", 10F);
            this.edRelatorio.Location = new System.Drawing.Point(2, 21);
            this.edRelatorio.Multiline = true;
            this.edRelatorio.Name = "edRelatorio";
            this.edRelatorio.ReadOnly = true;
            this.edRelatorio.ScrollBars = System.Windows.Forms.ScrollBars.Both;
            this.edRelatorio.Size = new System.Drawing.Size(895, 414);
            this.edRelatorio.TabIndex = 0;
            this.edRelatorio.WordWrap = false;
            // 
            // imageList3
            // 
            this.imageList3.ImageStream = ((System.Windows.Forms.ImageListStreamer)(resources.GetObject("imageList3.ImageStream")));
            this.imageList3.TransparentColor = System.Drawing.Color.Transparent;
            this.imageList3.Images.SetKeyName(0, "confirmar.bmp");
            this.imageList3.Images.SetKeyName(1, "fechar.bmp");
            this.imageList3.Images.SetKeyName(2, "novo.png");
            this.imageList3.Images.SetKeyName(3, "excluir.bmp");
            // 
            // panel1
            // 
            this.panel1.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.panel1.Controls.Add(this.button3);
            this.panel1.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.panel1.Location = new System.Drawing.Point(0, 442);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(899, 34);
            this.panel1.TabIndex = 14;
            // 
            // button3
            // 
            this.button3.BackColor = System.Drawing.Color.Transparent;
            this.button3.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.button3.Dock = System.Windows.Forms.DockStyle.Right;
            this.button3.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.button3.ImageKey = "fechar.bmp";
            this.button3.ImageList = this.imageList3;
            this.button3.Location = new System.Drawing.Point(783, 0);
            this.button3.Name = "button3";
            this.button3.Size = new System.Drawing.Size(114, 32);
            this.button3.TabIndex = 17;
            this.button3.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            this.button3.UseVisualStyleBackColor = false;
            this.button3.Click += new System.EventHandler(this.button3_Click);
            // 
            // FRelatorios
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(899, 476);
            this.Controls.Add(this.panel1);
            this.Controls.Add(this.edRelatorio);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.SizableToolWindow;
            this.Name = "FRelatorios";
            this.Text = "FRelatorios";
            this.panel1.ResumeLayout(false);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.TextBox edRelatorio;
        public System.Windows.Forms.ImageList imageList3;
        private System.Windows.Forms.Panel panel1;
        private System.Windows.Forms.Button button3;
    }
}
