namespace PG
{
    partial class FMateriais
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(FMateriais));
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle1 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle2 = new System.Windows.Forms.DataGridViewCellStyle();
            this.btNovo = new System.Windows.Forms.Button();
            this.imageList3 = new System.Windows.Forms.ImageList(this.components);
            this.btExcluir = new System.Windows.Forms.Button();
            this.GridMateriais = new System.Windows.Forms.DataGridView();
            this.colorDialog1 = new System.Windows.Forms.ColorDialog();
            this.panel1 = new System.Windows.Forms.Panel();
            this.button3 = new System.Windows.Forms.Button();
            ((System.ComponentModel.ISupportInitialize)(this.GridMateriais)).BeginInit();
            this.panel1.SuspendLayout();
            this.SuspendLayout();
            // 
            // btNovo
            // 
            this.btNovo.BackColor = System.Drawing.Color.Transparent;
            this.btNovo.FlatAppearance.MouseDownBackColor = System.Drawing.Color.Transparent;
            this.btNovo.FlatAppearance.MouseOverBackColor = System.Drawing.Color.Transparent;
            this.btNovo.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btNovo.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.btNovo.ImageIndex = 2;
            this.btNovo.ImageList = this.imageList3;
            this.btNovo.Location = new System.Drawing.Point(497, 12);
            this.btNovo.Name = "btNovo";
            this.btNovo.Size = new System.Drawing.Size(70, 22);
            this.btNovo.TabIndex = 91;
            this.btNovo.Text = "Novo";
            this.btNovo.TextAlign = System.Drawing.ContentAlignment.TopRight;
            this.btNovo.UseVisualStyleBackColor = false;
            this.btNovo.Click += new System.EventHandler(this.button6_Click);
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
            // btExcluir
            // 
            this.btExcluir.BackColor = System.Drawing.Color.Transparent;
            this.btExcluir.FlatAppearance.MouseDownBackColor = System.Drawing.Color.Transparent;
            this.btExcluir.FlatAppearance.MouseOverBackColor = System.Drawing.Color.Transparent;
            this.btExcluir.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btExcluir.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.btExcluir.ImageIndex = 3;
            this.btExcluir.ImageList = this.imageList3;
            this.btExcluir.Location = new System.Drawing.Point(497, 40);
            this.btExcluir.Name = "btExcluir";
            this.btExcluir.Size = new System.Drawing.Size(70, 21);
            this.btExcluir.TabIndex = 90;
            this.btExcluir.Text = "Excluir";
            this.btExcluir.TextAlign = System.Drawing.ContentAlignment.TopRight;
            this.btExcluir.UseVisualStyleBackColor = false;
            this.btExcluir.Click += new System.EventHandler(this.btExcluir_Click);
            // 
            // GridMateriais
            // 
            this.GridMateriais.AutoSizeColumnsMode = System.Windows.Forms.DataGridViewAutoSizeColumnsMode.AllCells;
            this.GridMateriais.AutoSizeRowsMode = System.Windows.Forms.DataGridViewAutoSizeRowsMode.AllCells;
            this.GridMateriais.CellBorderStyle = System.Windows.Forms.DataGridViewCellBorderStyle.SingleVertical;
            dataGridViewCellStyle1.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleLeft;
            dataGridViewCellStyle1.BackColor = System.Drawing.SystemColors.Control;
            dataGridViewCellStyle1.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            dataGridViewCellStyle1.ForeColor = System.Drawing.SystemColors.WindowText;
            dataGridViewCellStyle1.SelectionBackColor = System.Drawing.SystemColors.ScrollBar;
            dataGridViewCellStyle1.SelectionForeColor = System.Drawing.SystemColors.HighlightText;
            dataGridViewCellStyle1.WrapMode = System.Windows.Forms.DataGridViewTriState.True;
            this.GridMateriais.ColumnHeadersDefaultCellStyle = dataGridViewCellStyle1;
            this.GridMateriais.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            dataGridViewCellStyle2.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleLeft;
            dataGridViewCellStyle2.BackColor = System.Drawing.SystemColors.ControlLightLight;
            dataGridViewCellStyle2.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            dataGridViewCellStyle2.ForeColor = System.Drawing.SystemColors.ControlText;
            dataGridViewCellStyle2.SelectionBackColor = System.Drawing.SystemColors.Highlight;
            dataGridViewCellStyle2.SelectionForeColor = System.Drawing.SystemColors.ControlText;
            dataGridViewCellStyle2.WrapMode = System.Windows.Forms.DataGridViewTriState.False;
            this.GridMateriais.DefaultCellStyle = dataGridViewCellStyle2;
            this.GridMateriais.GridColor = System.Drawing.SystemColors.ActiveCaption;
            this.GridMateriais.Location = new System.Drawing.Point(2, 12);
            this.GridMateriais.MultiSelect = false;
            this.GridMateriais.Name = "GridMateriais";
            this.GridMateriais.ReadOnly = true;
            this.GridMateriais.RowHeadersVisible = false;
            this.GridMateriais.RowHeadersWidthSizeMode = System.Windows.Forms.DataGridViewRowHeadersWidthSizeMode.AutoSizeToAllHeaders;
            this.GridMateriais.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect;
            this.GridMateriais.ShowEditingIcon = false;
            this.GridMateriais.Size = new System.Drawing.Size(491, 117);
            this.GridMateriais.TabIndex = 85;
            this.GridMateriais.CellClick += new System.Windows.Forms.DataGridViewCellEventHandler(this.GridMateriais_CellClick);
            this.GridMateriais.CellDoubleClick += new System.Windows.Forms.DataGridViewCellEventHandler(this.GridMateriais_CellDoubleClick);
            this.GridMateriais.DataBindingComplete += new System.Windows.Forms.DataGridViewBindingCompleteEventHandler(this.GridMateriais_DataBindingComplete);
            // 
            // panel1
            // 
            this.panel1.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.panel1.Controls.Add(this.button3);
            this.panel1.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.panel1.Location = new System.Drawing.Point(0, 144);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(577, 34);
            this.panel1.TabIndex = 100;
            // 
            // button3
            // 
            this.button3.BackColor = System.Drawing.Color.Transparent;
            this.button3.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.button3.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.button3.ImageKey = "fechar.bmp";
            this.button3.ImageList = this.imageList3;
            this.button3.Location = new System.Drawing.Point(530, 3);
            this.button3.Name = "button3";
            this.button3.Size = new System.Drawing.Size(40, 25);
            this.button3.TabIndex = 17;
            this.button3.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            this.button3.UseVisualStyleBackColor = false;
            this.button3.Click += new System.EventHandler(this.button3_Click);
            // 
            // FMateriais
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.CancelButton = this.button3;
            this.ClientSize = new System.Drawing.Size(577, 178);
            this.Controls.Add(this.panel1);
            this.Controls.Add(this.btNovo);
            this.Controls.Add(this.btExcluir);
            this.Controls.Add(this.GridMateriais);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedToolWindow;
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "FMateriais";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "Materiais";
            this.FormClosed += new System.Windows.Forms.FormClosedEventHandler(this.FMateriais_FormClosed);
            this.Load += new System.EventHandler(this.FMateriais_Load);
            this.Move += new System.EventHandler(this.FMateriais_Move);
            ((System.ComponentModel.ISupportInitialize)(this.GridMateriais)).EndInit();
            this.panel1.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Button btNovo;
        private System.Windows.Forms.Button btExcluir;
        private System.Windows.Forms.DataGridView GridMateriais;
        private System.Windows.Forms.ColorDialog colorDialog1;
        private System.Windows.Forms.Panel panel1;
        private System.Windows.Forms.Button button3;
        public System.Windows.Forms.ImageList imageList3;
    }
}