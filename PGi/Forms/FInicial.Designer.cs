namespace PG
{
    partial class Inicial
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(Inicial));
            this._imageList = new System.Windows.Forms.ImageList(this.components);
            this.panel1 = new System.Windows.Forms.Panel();
            this.btNovo = new System.Windows.Forms.Button();
            this.panel2 = new System.Windows.Forms.Panel();
            this.button4 = new System.Windows.Forms.Button();
            this.label1 = new System.Windows.Forms.Label();
            this.button3 = new System.Windows.Forms.Button();
            this.button2 = new System.Windows.Forms.Button();
            this.textBox1 = new System.Windows.Forms.TextBox();
            this.tv = new System.Windows.Forms.TreeView();
            this.Diretorios = new System.Windows.Forms.TreeView();
            this.panel1.SuspendLayout();
            this.panel2.SuspendLayout();
            this.SuspendLayout();
            // 
            // _imageList
            // 
            this._imageList.ImageStream = ((System.Windows.Forms.ImageListStreamer)(resources.GetObject("_imageList.ImageStream")));
            this._imageList.TransparentColor = System.Drawing.Color.Transparent;
            this._imageList.Images.SetKeyName(0, "");
            this._imageList.Images.SetKeyName(1, "");
            this._imageList.Images.SetKeyName(2, "");
            this._imageList.Images.SetKeyName(3, "");
            this._imageList.Images.SetKeyName(4, "");
            this._imageList.Images.SetKeyName(5, "");
            this._imageList.Images.SetKeyName(6, "");
            this._imageList.Images.SetKeyName(7, "");
            this._imageList.Images.SetKeyName(8, "");
            this._imageList.Images.SetKeyName(9, "");
            this._imageList.Images.SetKeyName(10, "");
            this._imageList.Images.SetKeyName(11, "");
            this._imageList.Images.SetKeyName(12, "");
            this._imageList.Images.SetKeyName(13, "edificio.bmp");
            this._imageList.Images.SetKeyName(14, "pavimento.bmp");
            this._imageList.Images.SetKeyName(15, "pavimento_sel.bmp");
            // 
            // panel1
            // 
            this.panel1.Controls.Add(this.btNovo);
            this.panel1.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.panel1.Location = new System.Drawing.Point(0, 528);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(636, 35);
            this.panel1.TabIndex = 3;
            // 
            // btNovo
            // 
            this.btNovo.AutoSize = true;
            this.btNovo.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch;
            this.btNovo.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btNovo.ImageIndex = 7;
            this.btNovo.ImageList = this._imageList;
            this.btNovo.Location = new System.Drawing.Point(3, 3);
            this.btNovo.Name = "btNovo";
            this.btNovo.Size = new System.Drawing.Size(75, 26);
            this.btNovo.TabIndex = 2;
            this.btNovo.Text = "Novo";
            this.btNovo.TextAlign = System.Drawing.ContentAlignment.BottomCenter;
            this.btNovo.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageBeforeText;
            this.btNovo.UseCompatibleTextRendering = true;
            this.btNovo.UseVisualStyleBackColor = true;
            this.btNovo.Click += new System.EventHandler(this.btNovo_Click);
            // 
            // panel2
            // 
            this.panel2.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.panel2.Controls.Add(this.button4);
            this.panel2.Controls.Add(this.label1);
            this.panel2.Controls.Add(this.button3);
            this.panel2.Controls.Add(this.button2);
            this.panel2.Controls.Add(this.textBox1);
            this.panel2.Dock = System.Windows.Forms.DockStyle.Top;
            this.panel2.Location = new System.Drawing.Point(0, 0);
            this.panel2.Name = "panel2";
            this.panel2.Size = new System.Drawing.Size(636, 42);
            this.panel2.TabIndex = 5;
            // 
            // button4
            // 
            this.button4.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
      
            this.button4.Location = new System.Drawing.Point(348, 15);
            this.button4.Name = "button4";
            this.button4.Size = new System.Drawing.Size(26, 23);
            this.button4.TabIndex = 4;
            this.button4.Text = "+";
            this.button4.UseVisualStyleBackColor = true;
            this.button4.Click += new System.EventHandler(this.button4_Click);
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(3, 1);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(242, 13);
            this.label1.TabIndex = 3;
            this.label1.Text = "Adicionar nova pasta de projetos para esta árvore";
            // 
            // button3
            // 
            this.button3.Enabled = false;
            this.button3.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.button3.Location = new System.Drawing.Point(318, 15);
            this.button3.Name = "button3";
            this.button3.Size = new System.Drawing.Size(26, 23);
            this.button3.TabIndex = 2;
            this.button3.Text = "+";
            this.button3.UseVisualStyleBackColor = true;
            // 
            // button2
            // 
            this.button2.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.button2.Location = new System.Drawing.Point(290, 15);
            this.button2.Name = "button2";
            this.button2.Size = new System.Drawing.Size(26, 23);
            this.button2.TabIndex = 1;
            this.button2.Text = "...";
            this.button2.UseVisualStyleBackColor = true;
            // 
            // textBox1
            // 
            this.textBox1.Location = new System.Drawing.Point(3, 17);
            this.textBox1.Name = "textBox1";
            this.textBox1.Size = new System.Drawing.Size(284, 20);
            this.textBox1.TabIndex = 0;
            // 
            // tv
            // 
            this.tv.AccessibleRole = System.Windows.Forms.AccessibleRole.ScrollBar;
            this.tv.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.tv.Dock = System.Windows.Forms.DockStyle.Right;
            this.tv.ForeColor = System.Drawing.SystemColors.Desktop;
            this.tv.HideSelection = false;
            this.tv.ImageIndex = 0;
            this.tv.ImageList = this._imageList;
            this.tv.ImeMode = System.Windows.Forms.ImeMode.Disable;
            this.tv.Location = new System.Drawing.Point(336, 42);
            this.tv.Name = "tv";
            this.tv.SelectedImageIndex = 0;
            this.tv.ShowNodeToolTips = true;
            this.tv.ShowPlusMinus = false;
            this.tv.Size = new System.Drawing.Size(300, 486);
            this.tv.TabIndex = 6;
            this.tv.AfterSelect += new System.Windows.Forms.TreeViewEventHandler(this.tv_AfterSelect);
            this.tv.MouseDoubleClick += new System.Windows.Forms.MouseEventHandler(this.tv_MouseDoubleClick);
            // 
            // Diretorios
            // 
            this.Diretorios.AccessibleRole = System.Windows.Forms.AccessibleRole.ScrollBar;
            this.Diretorios.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.Diretorios.Dock = System.Windows.Forms.DockStyle.Left;
            this.Diretorios.ForeColor = System.Drawing.Color.Black;
            this.Diretorios.HideSelection = false;
            this.Diretorios.ImageIndex = 12;
            this.Diretorios.ImageList = this._imageList;
            this.Diretorios.ImeMode = System.Windows.Forms.ImeMode.Disable;
            this.Diretorios.Location = new System.Drawing.Point(0, 42);
            this.Diretorios.Name = "Diretorios";
            this.Diretorios.SelectedImageIndex = 12;
            this.Diretorios.ShowNodeToolTips = true;
            this.Diretorios.Size = new System.Drawing.Size(300, 486);
            this.Diretorios.TabIndex = 7;
            this.Diretorios.AfterSelect += new System.Windows.Forms.TreeViewEventHandler(this.Diretorios_AfterSelect);
            // 
            // Inicial
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(636, 563);
            this.Controls.Add(this.Diretorios);
            this.Controls.Add(this.tv);
            this.Controls.Add(this.panel2);
            this.Controls.Add(this.panel1);
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "Inicial";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "PG - Projeto";
            this.TopMost = true;
            this.panel1.ResumeLayout(false);
            this.panel1.PerformLayout();
            this.panel2.ResumeLayout(false);
            this.panel2.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.ImageList _imageList;
        private System.Windows.Forms.Panel panel1;
        private System.Windows.Forms.Button btNovo;
        private System.Windows.Forms.Panel panel2;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Button button3;
        private System.Windows.Forms.Button button2;
        private System.Windows.Forms.TextBox textBox1;
        private System.Windows.Forms.TreeView tv;
        private System.Windows.Forms.Button button4;
        private System.Windows.Forms.TreeView Diretorios;
    }
}