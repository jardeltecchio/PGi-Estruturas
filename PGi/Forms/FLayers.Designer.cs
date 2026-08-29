namespace PG
{
    partial class FLayers
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(FLayers));
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle1 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.TreeNode treeNode1 = new System.Windows.Forms.TreeNode("Tipo de elemento");
            System.Windows.Forms.TreeNode treeNode2 = new System.Windows.Forms.TreeNode("Cor de fundo");
            System.Windows.Forms.TreeNode treeNode3 = new System.Windows.Forms.TreeNode("Tela", new System.Windows.Forms.TreeNode[] {
            treeNode2});
            this.dataSet1 = new System.Data.DataSet();
            this.dataTable1 = new System.Data.DataTable();
            this.panel1 = new System.Windows.Forms.Panel();
            this.button7 = new System.Windows.Forms.Button();
            this.imageList2 = new System.Windows.Forms.ImageList(this.components);
            this.button8 = new System.Windows.Forms.Button();
            this.tbLayers = new System.Windows.Forms.TabControl();
            this.tabPage1 = new System.Windows.Forms.TabPage();
            this.pnCorDeFundo = new System.Windows.Forms.Panel();
            this.label2 = new System.Windows.Forms.Label();
            this.label1 = new System.Windows.Forms.Label();
            this.btCorBaixo = new System.Windows.Forms.Button();
            this.btCorCima = new System.Windows.Forms.Button();
            this.pnTipoElemento = new System.Windows.Forms.Panel();
            this.GridTipoElemento = new System.Windows.Forms.DataGridView();
            this.treeView1 = new System.Windows.Forms.TreeView();
            this.colorDialog1 = new System.Windows.Forms.ColorDialog();
            ((System.ComponentModel.ISupportInitialize)(this.dataSet1)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.dataTable1)).BeginInit();
            this.panel1.SuspendLayout();
            this.tbLayers.SuspendLayout();
            this.tabPage1.SuspendLayout();
            this.pnCorDeFundo.SuspendLayout();
            this.pnTipoElemento.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.GridTipoElemento)).BeginInit();
            this.SuspendLayout();
            // 
            // dataSet1
            // 
            this.dataSet1.DataSetName = "NewDataSet";
            this.dataSet1.Tables.AddRange(new System.Data.DataTable[] {
            this.dataTable1});
            // 
            // dataTable1
            // 
            this.dataTable1.TableName = "Table1";
            // 
            // panel1
            // 
            this.panel1.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.panel1.Controls.Add(this.button7);
            this.panel1.Controls.Add(this.button8);
            this.panel1.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.panel1.Location = new System.Drawing.Point(0, 484);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(678, 32);
            this.panel1.TabIndex = 39;
            // 
            // button7
            // 
            this.button7.BackColor = System.Drawing.Color.Transparent;
            this.button7.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.button7.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.button7.ImageIndex = 1;
            this.button7.ImageList = this.imageList2;
            this.button7.Location = new System.Drawing.Point(335, 2);
            this.button7.Name = "button7";
            this.button7.Size = new System.Drawing.Size(49, 25);
            this.button7.TabIndex = 7;
            this.button7.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            this.button7.UseVisualStyleBackColor = false;
            this.button7.Click += new System.EventHandler(this.button7_Click);
            // 
            // imageList2
            // 
            this.imageList2.ImageStream = ((System.Windows.Forms.ImageListStreamer)(resources.GetObject("imageList2.ImageStream")));
            this.imageList2.TransparentColor = System.Drawing.Color.Transparent;
            this.imageList2.Images.SetKeyName(0, "confirmar.bmp");
            this.imageList2.Images.SetKeyName(1, "fechar.bmp");
            // 
            // button8
            // 
            this.button8.BackColor = System.Drawing.Color.Transparent;
            this.button8.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.button8.ImageIndex = 0;
            this.button8.ImageList = this.imageList2;
            this.button8.Location = new System.Drawing.Point(3, 2);
            this.button8.Name = "button8";
            this.button8.Size = new System.Drawing.Size(51, 25);
            this.button8.TabIndex = 6;
            this.button8.TextAlign = System.Drawing.ContentAlignment.BottomRight;
            this.button8.UseVisualStyleBackColor = false;
            this.button8.Click += new System.EventHandler(this.button8_Click);
            // 
            // tbLayers
            // 
            this.tbLayers.Controls.Add(this.tabPage1);
            this.tbLayers.Dock = System.Windows.Forms.DockStyle.Top;
            this.tbLayers.Location = new System.Drawing.Point(0, 0);
            this.tbLayers.Name = "tbLayers";
            this.tbLayers.SelectedIndex = 0;
            this.tbLayers.Size = new System.Drawing.Size(678, 462);
            this.tbLayers.TabIndex = 43;
            // 
            // tabPage1
            // 
            this.tabPage1.Controls.Add(this.pnCorDeFundo);
            this.tabPage1.Controls.Add(this.pnTipoElemento);
            this.tabPage1.Controls.Add(this.treeView1);
            this.tabPage1.Location = new System.Drawing.Point(4, 22);
            this.tabPage1.Name = "tabPage1";
            this.tabPage1.Padding = new System.Windows.Forms.Padding(3);
            this.tabPage1.Size = new System.Drawing.Size(670, 436);
            this.tabPage1.TabIndex = 0;
            this.tabPage1.Text = "Cor";
            this.tabPage1.UseVisualStyleBackColor = true;
            // 
            // pnCorDeFundo
            // 
            this.pnCorDeFundo.Controls.Add(this.label2);
            this.pnCorDeFundo.Controls.Add(this.label1);
            this.pnCorDeFundo.Controls.Add(this.btCorBaixo);
            this.pnCorDeFundo.Controls.Add(this.btCorCima);
            this.pnCorDeFundo.Location = new System.Drawing.Point(158, 222);
            this.pnCorDeFundo.Name = "pnCorDeFundo";
            this.pnCorDeFundo.Size = new System.Drawing.Size(213, 59);
            this.pnCorDeFundo.TabIndex = 46;
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(5, 37);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(66, 13);
            this.label2.TabIndex = 7;
            this.label2.Text = "Cor de baixo";
            this.label2.Visible = false;
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(5, 13);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(68, 13);
            this.label1.TabIndex = 6;
            this.label1.Text = "Cor de fundo";
            this.label1.Click += new System.EventHandler(this.label1_Click);
            // 
            // btCorBaixo
            // 
            this.btCorBaixo.BackColor = System.Drawing.Color.DarkGray;
            this.btCorBaixo.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btCorBaixo.Location = new System.Drawing.Point(74, 27);
            this.btCorBaixo.Name = "btCorBaixo";
            this.btCorBaixo.Size = new System.Drawing.Size(40, 23);
            this.btCorBaixo.TabIndex = 5;
            this.btCorBaixo.UseVisualStyleBackColor = false;
            this.btCorBaixo.Visible = false;
            this.btCorBaixo.Click += new System.EventHandler(this.btCorBaixo_Click);
            // 
            // btCorCima
            // 
            this.btCorCima.BackColor = System.Drawing.Color.DodgerBlue;
            this.btCorCima.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btCorCima.Location = new System.Drawing.Point(74, 3);
            this.btCorCima.Name = "btCorCima";
            this.btCorCima.Size = new System.Drawing.Size(40, 23);
            this.btCorCima.TabIndex = 4;
            this.btCorCima.UseVisualStyleBackColor = false;
            this.btCorCima.Click += new System.EventHandler(this.btCorCima_Click);
            // 
            // pnTipoElemento
            // 
            this.pnTipoElemento.Controls.Add(this.GridTipoElemento);
            this.pnTipoElemento.Location = new System.Drawing.Point(219, 60);
            this.pnTipoElemento.Name = "pnTipoElemento";
            this.pnTipoElemento.Size = new System.Drawing.Size(115, 86);
            this.pnTipoElemento.TabIndex = 45;
            // 
            // GridTipoElemento
            // 
            this.GridTipoElemento.AllowUserToResizeRows = false;
            this.GridTipoElemento.AutoSizeColumnsMode = System.Windows.Forms.DataGridViewAutoSizeColumnsMode.AllCells;
            this.GridTipoElemento.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            dataGridViewCellStyle1.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleLeft;
            dataGridViewCellStyle1.BackColor = System.Drawing.SystemColors.ControlLightLight;
            dataGridViewCellStyle1.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            dataGridViewCellStyle1.ForeColor = System.Drawing.SystemColors.ControlText;
            dataGridViewCellStyle1.SelectionBackColor = System.Drawing.SystemColors.Highlight;
            dataGridViewCellStyle1.SelectionForeColor = System.Drawing.SystemColors.ControlText;
            dataGridViewCellStyle1.WrapMode = System.Windows.Forms.DataGridViewTriState.False;
            this.GridTipoElemento.DefaultCellStyle = dataGridViewCellStyle1;
            this.GridTipoElemento.Dock = System.Windows.Forms.DockStyle.Fill;
            this.GridTipoElemento.EnableHeadersVisualStyles = false;
            this.GridTipoElemento.GridColor = System.Drawing.SystemColors.ActiveCaption;
            this.GridTipoElemento.Location = new System.Drawing.Point(0, 0);
            this.GridTipoElemento.MultiSelect = false;
            this.GridTipoElemento.Name = "GridTipoElemento";
            this.GridTipoElemento.ReadOnly = true;
            this.GridTipoElemento.RowHeadersVisible = false;
            this.GridTipoElemento.RowHeadersWidth = 10;
            this.GridTipoElemento.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.CellSelect;
            this.GridTipoElemento.ShowEditingIcon = false;
            this.GridTipoElemento.Size = new System.Drawing.Size(115, 86);
            this.GridTipoElemento.TabIndex = 44;
            this.GridTipoElemento.CellDoubleClick += new System.Windows.Forms.DataGridViewCellEventHandler(this.GridLayers_CellDoubleClick);
            this.GridTipoElemento.DataBindingComplete += new System.Windows.Forms.DataGridViewBindingCompleteEventHandler(this.GridLayers_DataBindingComplete);
            // 
            // treeView1
            // 
            this.treeView1.Dock = System.Windows.Forms.DockStyle.Left;
            this.treeView1.Location = new System.Drawing.Point(3, 3);
            this.treeView1.Name = "treeView1";
            treeNode1.Name = "TipoElemento";
            treeNode1.Tag = "pnTipoElemento";
            treeNode1.Text = "Tipo de elemento";
            treeNode2.Name = "CorDeFundo";
            treeNode2.Tag = "pnCorDeFundo";
            treeNode2.Text = "Cor de fundo";
            treeNode3.Name = "Node3";
            treeNode3.Text = "Tela";
            this.treeView1.Nodes.AddRange(new System.Windows.Forms.TreeNode[] {
            treeNode1,
            treeNode3});
            this.treeView1.Size = new System.Drawing.Size(149, 430);
            this.treeView1.TabIndex = 44;
            this.treeView1.AfterSelect += new System.Windows.Forms.TreeViewEventHandler(this.treeView1_AfterSelect);
            // 
            // FLayers
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.CancelButton = this.button7;
            this.ClientSize = new System.Drawing.Size(678, 516);
            this.Controls.Add(this.tbLayers);
            this.Controls.Add(this.panel1);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedToolWindow;
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "FLayers";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "PGi - Opções de cores";
            this.FormClosed += new System.Windows.Forms.FormClosedEventHandler(this.FLayers_FormClosed);
            this.Shown += new System.EventHandler(this.FLayers_Shown);
            this.Move += new System.EventHandler(this.FLayers_Move);
            ((System.ComponentModel.ISupportInitialize)(this.dataSet1)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.dataTable1)).EndInit();
            this.panel1.ResumeLayout(false);
            this.tbLayers.ResumeLayout(false);
            this.tabPage1.ResumeLayout(false);
            this.pnCorDeFundo.ResumeLayout(false);
            this.pnCorDeFundo.PerformLayout();
            this.pnTipoElemento.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.GridTipoElemento)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Data.DataSet dataSet1;
        private System.Data.DataTable dataTable1;
        private System.Windows.Forms.Panel panel1;
        private System.Windows.Forms.Button button7;
        private System.Windows.Forms.Button button8;
        private System.Windows.Forms.TabControl tbLayers;
        private System.Windows.Forms.TabPage tabPage1;
        private System.Windows.Forms.TreeView treeView1;
        private System.Windows.Forms.ColorDialog colorDialog1;
        private System.Windows.Forms.Panel pnTipoElemento;
        private System.Windows.Forms.DataGridView GridTipoElemento;
        private System.Windows.Forms.Panel pnCorDeFundo;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Button btCorBaixo;
        private System.Windows.Forms.Button btCorCima;
        public System.Windows.Forms.ImageList imageList2;
    }
}