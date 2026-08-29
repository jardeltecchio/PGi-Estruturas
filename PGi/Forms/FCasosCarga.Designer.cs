namespace PG
{
    partial class FCasosCarga
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(FCasosCarga));
            this.panel1 = new System.Windows.Forms.Panel();
            this.button3 = new System.Windows.Forms.Button();
            this.imageList3 = new System.Windows.Forms.ImageList(this.components);
            this.button2 = new System.Windows.Forms.Button();
            this.button1 = new System.Windows.Forms.Button();
            this.colorDialog1 = new System.Windows.Forms.ColorDialog();
            this.pg = new System.Windows.Forms.PropertyGrid();
            this.lbCasos = new System.Windows.Forms.ListBox();
            this.panel1.SuspendLayout();
            this.SuspendLayout();
            // 
            // panel1
            // 
            this.panel1.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.panel1.Controls.Add(this.button3);
            this.panel1.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.panel1.Location = new System.Drawing.Point(0, 287);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(624, 34);
            this.panel1.TabIndex = 14;
            // 
            // button3
            // 
            this.button3.BackColor = System.Drawing.Color.Transparent;
            this.button3.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.button3.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.button3.ImageKey = "fechar.bmp";
            this.button3.ImageList = this.imageList3;
            this.button3.Location = new System.Drawing.Point(546, 3);
            this.button3.Name = "button3";
            this.button3.Size = new System.Drawing.Size(73, 25);
            this.button3.TabIndex = 17;
            this.button3.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            this.button3.UseVisualStyleBackColor = false;
            this.button3.Click += new System.EventHandler(this.button3_Click);
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
            // button2
            // 
            this.button2.BackColor = System.Drawing.Color.Transparent;
            this.button2.FlatAppearance.MouseDownBackColor = System.Drawing.Color.Transparent;
            this.button2.FlatAppearance.MouseOverBackColor = System.Drawing.Color.Transparent;
            this.button2.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.button2.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.button2.ImageIndex = 2;
            this.button2.ImageList = this.imageList3;
            this.button2.Location = new System.Drawing.Point(12, 255);
            this.button2.Name = "button2";
            this.button2.Size = new System.Drawing.Size(70, 24);
            this.button2.TabIndex = 1;
            this.button2.Text = "Novo";
            this.button2.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            this.button2.UseVisualStyleBackColor = false;
            this.button2.Click += new System.EventHandler(this.button2_Click);
            // 
            // button1
            // 
            this.button1.BackColor = System.Drawing.Color.Transparent;
            this.button1.FlatAppearance.MouseDownBackColor = System.Drawing.Color.Transparent;
            this.button1.FlatAppearance.MouseOverBackColor = System.Drawing.Color.Transparent;
            this.button1.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.button1.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.button1.ImageIndex = 3;
            this.button1.ImageList = this.imageList3;
            this.button1.Location = new System.Drawing.Point(90, 255);
            this.button1.Name = "button1";
            this.button1.Size = new System.Drawing.Size(70, 24);
            this.button1.TabIndex = 68;
            this.button1.Text = "Excluir";
            this.button1.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            this.button1.UseVisualStyleBackColor = false;
            this.button1.Click += new System.EventHandler(this.button1_Click);
            // 
            // pg
            // 
            this.pg.Anchor = System.Windows.Forms.AnchorStyles.None;
            this.pg.HelpVisible = false;
            this.pg.LineColor = System.Drawing.SystemColors.ScrollBar;
            this.pg.Location = new System.Drawing.Point(166, 13);
            this.pg.Name = "pg";
            this.pg.PropertySort = System.Windows.Forms.PropertySort.Categorized;
            this.pg.Size = new System.Drawing.Size(453, 236);
            this.pg.TabIndex = 69;
            this.pg.ToolbarVisible = false;
            this.pg.PropertyValueChanged += new System.Windows.Forms.PropertyValueChangedEventHandler(this.pg_PropertyValueChanged);
            this.pg.CausesValidationChanged += new System.EventHandler(this.pg_CausesValidationChanged);
            // 
            // lbCasos
            // 
            this.lbCasos.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.lbCasos.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lbCasos.FormattingEnabled = true;
            this.lbCasos.Location = new System.Drawing.Point(12, 13);
            this.lbCasos.Name = "lbCasos";
            this.lbCasos.Size = new System.Drawing.Size(148, 236);
            this.lbCasos.TabIndex = 70;
            this.lbCasos.Click += new System.EventHandler(this.lbCasos_Click);
            this.lbCasos.SelectedIndexChanged += new System.EventHandler(this.lbCasos_SelectedIndexChanged);
            // 
            // FCasosCarga
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.CancelButton = this.button3;
            this.ClientSize = new System.Drawing.Size(624, 321);
            this.Controls.Add(this.lbCasos);
            this.Controls.Add(this.pg);
            this.Controls.Add(this.button2);
            this.Controls.Add(this.button1);
            this.Controls.Add(this.panel1);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedToolWindow;
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "FCasosCarga";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "Casos de carga";
            this.FormClosed += new System.Windows.Forms.FormClosedEventHandler(this.FCasosCarga_FormClosed);
            this.Load += new System.EventHandler(this.FCasosCarga_Load);
            this.Move += new System.EventHandler(this.FCasosCarga_Move);
            this.panel1.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion
        private System.Windows.Forms.Panel panel1;
        private System.Windows.Forms.Button button3;
        private System.Windows.Forms.Button button1;
        private System.Windows.Forms.Button button2;
        private System.Windows.Forms.ColorDialog colorDialog1;
        public System.Windows.Forms.ImageList imageList3;
        internal System.Windows.Forms.PropertyGrid pg;
        private System.Windows.Forms.ListBox lbCasos;
    }
}