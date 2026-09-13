namespace PG
{
    partial class FCombinacoes
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(FCombinacoes));
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle6 = new System.Windows.Forms.DataGridViewCellStyle();
            this.panel1 = new System.Windows.Forms.Panel();
            this.button3 = new System.Windows.Forms.Button();
            this.imageList3 = new System.Windows.Forms.ImageList(this.components);
            this.button6 = new System.Windows.Forms.Button();
            this.button7 = new System.Windows.Forms.Button();
            this.bindingSource1 = new System.Windows.Forms.BindingSource(this.components);
            this.panel2 = new System.Windows.Forms.Panel();
            this.lbComb = new System.Windows.Forms.ListBox();
            this.panel3 = new System.Windows.Forms.Panel();
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.groupBox2 = new System.Windows.Forms.GroupBox();
            this.button1 = new System.Windows.Forms.Button();
            this.button2 = new System.Windows.Forms.Button();
            this.GridPrincipal = new System.Windows.Forms.DataGridView();
            this.Coeficiente = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.Caso = new System.Windows.Forms.DataGridViewComboBoxColumn();
            this.pg = new System.Windows.Forms.PropertyGrid();
            this.button4 = new System.Windows.Forms.Button();
            this.tbTipoCombinacao = new System.Windows.Forms.TabControl();
            this.tabPage1 = new System.Windows.Forms.TabPage();
            this.tabPage2 = new System.Windows.Forms.TabPage();
            this.panel1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.bindingSource1)).BeginInit();
            this.panel2.SuspendLayout();
            this.panel3.SuspendLayout();
            this.groupBox1.SuspendLayout();
            this.groupBox2.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.GridPrincipal)).BeginInit();
            this.tbTipoCombinacao.SuspendLayout();
            this.SuspendLayout();
            // 
            // panel1
            // 
            this.panel1.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.panel1.Controls.Add(this.button3);
            this.panel1.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.panel1.Location = new System.Drawing.Point(0, 366);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(872, 34);
            this.panel1.TabIndex = 15;
            // 
            // button3
            // 
            this.button3.BackColor = System.Drawing.Color.Transparent;
            this.button3.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.button3.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.button3.ImageKey = "fechar.bmp";
            this.button3.ImageList = this.imageList3;
            this.button3.Location = new System.Drawing.Point(790, 3);
            this.button3.Name = "button3";
            this.button3.Size = new System.Drawing.Size(77, 25);
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
            this.imageList3.Images.SetKeyName(4, "clonar.png");
            // 
            // button6
            // 
            this.button6.BackColor = System.Drawing.Color.Transparent;
            this.button6.FlatAppearance.MouseDownBackColor = System.Drawing.Color.Transparent;
            this.button6.FlatAppearance.MouseOverBackColor = System.Drawing.Color.Transparent;
            this.button6.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.button6.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.button6.ImageIndex = 2;
            this.button6.ImageList = this.imageList3;
            this.button6.Location = new System.Drawing.Point(4, 339);
            this.button6.Name = "button6";
            this.button6.Size = new System.Drawing.Size(70, 24);
            this.button6.TabIndex = 139;
            this.button6.Text = "Novo";
            this.button6.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            this.button6.UseVisualStyleBackColor = false;
            this.button6.Click += new System.EventHandler(this.button6_Click);
            // 
            // button7
            // 
            this.button7.BackColor = System.Drawing.Color.Transparent;
            this.button7.FlatAppearance.MouseDownBackColor = System.Drawing.Color.Transparent;
            this.button7.FlatAppearance.MouseOverBackColor = System.Drawing.Color.Transparent;
            this.button7.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.button7.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.button7.ImageIndex = 3;
            this.button7.ImageList = this.imageList3;
            this.button7.Location = new System.Drawing.Point(76, 339);
            this.button7.Name = "button7";
            this.button7.Size = new System.Drawing.Size(70, 24);
            this.button7.TabIndex = 140;
            this.button7.Text = "Excluir";
            this.button7.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            this.button7.UseVisualStyleBackColor = false;
            this.button7.Click += new System.EventHandler(this.button7_Click);
            // 
            // panel2
            // 
            this.panel2.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.panel2.Controls.Add(this.lbComb);
            this.panel2.Location = new System.Drawing.Point(4, 29);
            this.panel2.Name = "panel2";
            this.panel2.Size = new System.Drawing.Size(364, 305);
            this.panel2.TabIndex = 148;
            // 
            // lbComb
            // 
            this.lbComb.BackColor = System.Drawing.Color.White;
            this.lbComb.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.lbComb.Dock = System.Windows.Forms.DockStyle.Fill;
            this.lbComb.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lbComb.FormattingEnabled = true;
            this.lbComb.HorizontalScrollbar = true;
            this.lbComb.Location = new System.Drawing.Point(0, 0);
            this.lbComb.Name = "lbComb";
            this.lbComb.Size = new System.Drawing.Size(362, 303);
            this.lbComb.TabIndex = 142;
            this.lbComb.Click += new System.EventHandler(this.lbComb_Click);
            this.lbComb.SelectedIndexChanged += new System.EventHandler(this.lbComb_SelectedIndexChanged);
            // 
            // panel3
            // 
            this.panel3.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.panel3.Controls.Add(this.groupBox1);
            this.panel3.Location = new System.Drawing.Point(371, 5);
            this.panel3.Name = "panel3";
            this.panel3.Size = new System.Drawing.Size(498, 328);
            this.panel3.TabIndex = 149;
            // 
            // groupBox1
            // 
            this.groupBox1.Controls.Add(this.groupBox2);
            this.groupBox1.Controls.Add(this.pg);
            this.groupBox1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.groupBox1.Location = new System.Drawing.Point(0, 0);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(496, 326);
            this.groupBox1.TabIndex = 144;
            this.groupBox1.TabStop = false;
            this.groupBox1.Text = "Dados da combinação";
            // 
            // groupBox2
            // 
            this.groupBox2.Controls.Add(this.button1);
            this.groupBox2.Controls.Add(this.button2);
            this.groupBox2.Controls.Add(this.GridPrincipal);
            this.groupBox2.ForeColor = System.Drawing.SystemColors.Desktop;
            this.groupBox2.Location = new System.Drawing.Point(6, 112);
            this.groupBox2.Name = "groupBox2";
            this.groupBox2.Size = new System.Drawing.Size(340, 180);
            this.groupBox2.TabIndex = 146;
            this.groupBox2.TabStop = false;
            this.groupBox2.Text = "Coeficientes";
            // 
            // button1
            // 
            this.button1.BackColor = System.Drawing.Color.Transparent;
            this.button1.FlatAppearance.BorderColor = System.Drawing.Color.Black;
            this.button1.FlatAppearance.MouseDownBackColor = System.Drawing.Color.Transparent;
            this.button1.FlatAppearance.MouseOverBackColor = System.Drawing.Color.Transparent;
            this.button1.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.button1.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.button1.ImageIndex = 2;
            this.button1.ImageList = this.imageList3;
            this.button1.Location = new System.Drawing.Point(261, 21);
            this.button1.Name = "button1";
            this.button1.Size = new System.Drawing.Size(69, 22);
            this.button1.TabIndex = 149;
            this.button1.Text = "Novo";
            this.button1.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            this.button1.UseVisualStyleBackColor = false;
            this.button1.Click += new System.EventHandler(this.button1_Click);
            // 
            // button2
            // 
            this.button2.BackColor = System.Drawing.Color.Transparent;
            this.button2.FlatAppearance.MouseDownBackColor = System.Drawing.Color.Transparent;
            this.button2.FlatAppearance.MouseOverBackColor = System.Drawing.Color.Transparent;
            this.button2.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.button2.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.button2.ImageIndex = 3;
            this.button2.ImageList = this.imageList3;
            this.button2.Location = new System.Drawing.Point(261, 49);
            this.button2.Name = "button2";
            this.button2.Size = new System.Drawing.Size(69, 22);
            this.button2.TabIndex = 150;
            this.button2.Text = "Excluir";
            this.button2.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            this.button2.UseVisualStyleBackColor = false;
            this.button2.Click += new System.EventHandler(this.button2_Click);
            // 
            // GridPrincipal
            // 
            this.GridPrincipal.AllowUserToResizeRows = false;
            this.GridPrincipal.AutoSizeColumnsMode = System.Windows.Forms.DataGridViewAutoSizeColumnsMode.AllCells;
            this.GridPrincipal.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.GridPrincipal.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.Coeficiente,
            this.Caso});
            dataGridViewCellStyle6.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleLeft;
            dataGridViewCellStyle6.BackColor = System.Drawing.SystemColors.ControlLightLight;
            dataGridViewCellStyle6.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            dataGridViewCellStyle6.ForeColor = System.Drawing.SystemColors.Desktop;
            dataGridViewCellStyle6.SelectionBackColor = System.Drawing.SystemColors.Highlight;
            dataGridViewCellStyle6.SelectionForeColor = System.Drawing.SystemColors.ControlText;
            dataGridViewCellStyle6.WrapMode = System.Windows.Forms.DataGridViewTriState.False;
            this.GridPrincipal.DefaultCellStyle = dataGridViewCellStyle6;
            this.GridPrincipal.EnableHeadersVisualStyles = false;
            this.GridPrincipal.GridColor = System.Drawing.SystemColors.ActiveCaption;
            this.GridPrincipal.Location = new System.Drawing.Point(6, 19);
            this.GridPrincipal.MultiSelect = false;
            this.GridPrincipal.Name = "GridPrincipal";
            this.GridPrincipal.RowHeadersVisible = false;
            this.GridPrincipal.RowHeadersWidth = 10;
            this.GridPrincipal.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect;
            this.GridPrincipal.ShowEditingIcon = false;
            this.GridPrincipal.Size = new System.Drawing.Size(249, 155);
            this.GridPrincipal.TabIndex = 148;
            this.GridPrincipal.CellEndEdit += new System.Windows.Forms.DataGridViewCellEventHandler(this.GridPrincipal_CellEndEdit);
            // 
            // Coeficiente
            // 
            this.Coeficiente.DataPropertyName = "coef";
            this.Coeficiente.HeaderText = "Coeficiente";
            this.Coeficiente.Name = "Coeficiente";
            this.Coeficiente.Width = 85;
            // 
            // Caso
            // 
            this.Caso.DataPropertyName = "caso";
            this.Caso.HeaderText = "Caso";
            this.Caso.Items.AddRange(new object[] {
            "-1",
            "0"});
            this.Caso.Name = "Caso";
            this.Caso.Width = 39;
            // 
            // pg
            // 
            this.pg.BackColor = System.Drawing.SystemColors.ActiveCaption;
            this.pg.CategoryForeColor = System.Drawing.Color.CornflowerBlue;
            this.pg.HelpVisible = false;
            this.pg.LineColor = System.Drawing.SystemColors.ScrollBar;
            this.pg.Location = new System.Drawing.Point(6, 19);
            this.pg.Name = "pg";
            this.pg.PropertySort = System.Windows.Forms.PropertySort.Categorized;
            this.pg.Size = new System.Drawing.Size(485, 86);
            this.pg.TabIndex = 143;
            this.pg.ToolbarVisible = false;
            this.pg.PropertyValueChanged += new System.Windows.Forms.PropertyValueChangedEventHandler(this.pg_PropertyValueChanged);
            // 
            // button4
            // 
            this.button4.BackColor = System.Drawing.Color.Transparent;
            this.button4.FlatAppearance.MouseDownBackColor = System.Drawing.Color.Transparent;
            this.button4.FlatAppearance.MouseOverBackColor = System.Drawing.Color.Transparent;
            this.button4.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.button4.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.button4.ImageIndex = 4;
            this.button4.ImageList = this.imageList3;
            this.button4.Location = new System.Drawing.Point(149, 339);
            this.button4.Name = "button4";
            this.button4.Size = new System.Drawing.Size(70, 24);
            this.button4.TabIndex = 150;
            this.button4.Text = "Clonar";
            this.button4.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            this.button4.UseVisualStyleBackColor = false;
            this.button4.Click += new System.EventHandler(this.button4_Click_1);
            // 
            // tbTipoCombinacao
            // 
            this.tbTipoCombinacao.Controls.Add(this.tabPage1);
            this.tbTipoCombinacao.Controls.Add(this.tabPage2);
            this.tbTipoCombinacao.Location = new System.Drawing.Point(4, 5);
            this.tbTipoCombinacao.Name = "tbTipoCombinacao";
            this.tbTipoCombinacao.SelectedIndex = 0;
            this.tbTipoCombinacao.Size = new System.Drawing.Size(361, 23);
            this.tbTipoCombinacao.TabIndex = 151;
            this.tbTipoCombinacao.SelectedIndexChanged += new System.EventHandler(this.tbTipoCombinacao_SelectedIndexChanged);
            // 
            // tabPage1
            // 
            this.tabPage1.Location = new System.Drawing.Point(4, 22);
            this.tabPage1.Name = "tabPage1";
            this.tabPage1.Padding = new System.Windows.Forms.Padding(3);
            this.tabPage1.Size = new System.Drawing.Size(353, 0);
            this.tabPage1.TabIndex = 0;
            this.tabPage1.Text = "Combinações lineares";
            this.tabPage1.UseVisualStyleBackColor = true;
            // 
            // tabPage2
            // 
            this.tabPage2.Location = new System.Drawing.Point(4, 22);
            this.tabPage2.Name = "tabPage2";
            this.tabPage2.Padding = new System.Windows.Forms.Padding(3);
            this.tabPage2.Size = new System.Drawing.Size(353, 0);
            this.tabPage2.TabIndex = 1;
            this.tabPage2.Text = "Combinações de estabilidade";
            this.tabPage2.UseVisualStyleBackColor = true;
            // 
            // FCombinacoes
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.CancelButton = this.button3;
            this.ClientSize = new System.Drawing.Size(872, 400);
            this.Controls.Add(this.tbTipoCombinacao);
            this.Controls.Add(this.button4);
            this.Controls.Add(this.panel3);
            this.Controls.Add(this.panel2);
            this.Controls.Add(this.button6);
            this.Controls.Add(this.button7);
            this.Controls.Add(this.panel1);
            this.Cursor = System.Windows.Forms.Cursors.Default;
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedToolWindow;
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "FCombinacoes";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "Combinações";
            this.FormClosed += new System.Windows.Forms.FormClosedEventHandler(this.FCombinacoes_FormClosed);
            this.Load += new System.EventHandler(this.FCombinacoes_Load);
            this.Move += new System.EventHandler(this.FCombinacoes_Move);
            this.panel1.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.bindingSource1)).EndInit();
            this.panel2.ResumeLayout(false);
            this.panel3.ResumeLayout(false);
            this.groupBox1.ResumeLayout(false);
            this.groupBox2.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.GridPrincipal)).EndInit();
            this.tbTipoCombinacao.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion
        private System.Windows.Forms.Panel panel1;
        private System.Windows.Forms.Button button3;
        public System.Windows.Forms.ImageList imageList3;
        private System.Windows.Forms.Button button6;
        private System.Windows.Forms.Button button7;
        private System.Windows.Forms.BindingSource bindingSource1;
        private System.Windows.Forms.Panel panel2;
        private System.Windows.Forms.ListBox lbComb;
        private System.Windows.Forms.Panel panel3;
        private System.Windows.Forms.GroupBox groupBox1;
        private System.Windows.Forms.GroupBox groupBox2;
        private System.Windows.Forms.Button button1;
        private System.Windows.Forms.Button button2;
        private System.Windows.Forms.DataGridView GridPrincipal;
        internal System.Windows.Forms.PropertyGrid pg;
        private System.Windows.Forms.DataGridViewTextBoxColumn Coeficiente;
        private System.Windows.Forms.DataGridViewComboBoxColumn Caso;
        private System.Windows.Forms.Button button4;
        private System.Windows.Forms.TabControl tbTipoCombinacao;
        private System.Windows.Forms.TabPage tabPage1;
        private System.Windows.Forms.TabPage tabPage2;
    }
}