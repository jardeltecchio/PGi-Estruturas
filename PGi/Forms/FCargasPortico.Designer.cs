namespace PG
{
    partial class FCargasPortico
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
            this.trackEscalaDiagrama = new System.Windows.Forms.TrackBar();
            this.label1 = new System.Windows.Forms.Label();
            this.cbCombinacao = new System.Windows.Forms.ComboBox();
            ((System.ComponentModel.ISupportInitialize)(this.trackEscalaDiagrama)).BeginInit();
            this.SuspendLayout();
            // 
            // trackEscalaDiagrama
            // 
            this.trackEscalaDiagrama.AutoSize = false;
            this.trackEscalaDiagrama.LargeChange = 1;
            this.trackEscalaDiagrama.Location = new System.Drawing.Point(12, 68);
            this.trackEscalaDiagrama.Maximum = 40;
            this.trackEscalaDiagrama.Name = "trackEscalaDiagrama";
            this.trackEscalaDiagrama.Size = new System.Drawing.Size(81, 23);
            this.trackEscalaDiagrama.TabIndex = 72;
            this.trackEscalaDiagrama.Value = 10;
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Font = new System.Drawing.Font("Tahoma", 8.5F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label1.ForeColor = System.Drawing.SystemColors.MenuText;
            this.label1.Location = new System.Drawing.Point(9, 11);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(72, 14);
            this.label1.TabIndex = 71;
            this.label1.Text = "Combinação";
            // 
            // cbCombinacao
            // 
            this.cbCombinacao.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cbCombinacao.Font = new System.Drawing.Font("Tahoma", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.cbCombinacao.FormattingEnabled = true;
            this.cbCombinacao.Items.AddRange(new object[] {
            "1",
            "2",
            "3"});
            this.cbCombinacao.Location = new System.Drawing.Point(12, 27);
            this.cbCombinacao.Name = "cbCombinacao";
            this.cbCombinacao.Size = new System.Drawing.Size(326, 21);
            this.cbCombinacao.TabIndex = 70;
            // 
            // FCargasPortico
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(359, 119);
            this.Controls.Add(this.trackEscalaDiagrama);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.cbCombinacao);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedToolWindow;
            this.Name = "FCargasPortico";
            this.Text = "Cargas - Pórtico";
            this.TopMost = true;
            ((System.ComponentModel.ISupportInitialize)(this.trackEscalaDiagrama)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        public System.Windows.Forms.TrackBar trackEscalaDiagrama;
        public System.Windows.Forms.Label label1;
        public System.Windows.Forms.ComboBox cbCombinacao;
    }
}