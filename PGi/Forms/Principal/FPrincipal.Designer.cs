namespace PG
{
    partial class FPrincipal
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>fg
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(FPrincipal));
            this.backgroundWorker1 = new System.ComponentModel.BackgroundWorker();
            this.toolTip1 = new System.Windows.Forms.ToolTip(this.components);
            this.btCoordRelativa = new System.Windows.Forms.Button();
            this.imageList1 = new System.Windows.Forms.ImageList(this.components);
            this.toolTip2 = new System.Windows.Forms.ToolTip(this.components);
            this.printPreviewDialog1 = new System.Windows.Forms.PrintPreviewDialog();
            this.timer1 = new System.Windows.Forms.Timer(this.components);
            this.printDocument2 = new System.Drawing.Printing.PrintDocument();
            this.menuPrincipal = new System.Windows.Forms.ContextMenuStrip(this.components);
            this.editarBarras = new System.Windows.Forms.ToolStripMenuItem();
            this.editarApoios = new System.Windows.Forms.ToolStripMenuItem();
            this.editarCargaPontual = new System.Windows.Forms.ToolStripMenuItem();
            this.editarCargaDistribuida = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripMenuItem3 = new System.Windows.Forms.ToolStripSeparator();
            this.Elemento_rotacionar45 = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripMenuItem5 = new System.Windows.Forms.ToolStripSeparator();
            this.copiarToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.colarCtrlVToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.procurarCtrlFToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripMenuItem1 = new System.Windows.Forms.ToolStripSeparator();
            this.esconderSomenteToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.mostrarSomenteToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.mostrarTudoToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripMenuItem2 = new System.Windows.Forms.ToolStripSeparator();
            this.toolStripMenuItem4 = new System.Windows.Forms.ToolStripSeparator();
            this.cancelarToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.textBox1 = new System.Windows.Forms.TextBox();
            this.textBox2 = new System.Windows.Forms.TextBox();
            this.button1 = new System.Windows.Forms.Button();
            this.button2 = new System.Windows.Forms.Button();
            this.textBox4 = new System.Windows.Forms.TextBox();
            this.textBox3 = new System.Windows.Forms.TextBox();
            this.textBox5 = new System.Windows.Forms.TextBox();
            this.textBox6 = new System.Windows.Forms.TextBox();
            this.button3 = new System.Windows.Forms.Button();
            this.button4 = new System.Windows.Forms.Button();
            this.button5 = new System.Windows.Forms.Button();
            this.button6 = new System.Windows.Forms.Button();
            this.button7 = new System.Windows.Forms.Button();
            this.checkBox1 = new System.Windows.Forms.CheckBox();
            this.label1 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.checkBox2 = new System.Windows.Forms.CheckBox();
            this.checkBox3 = new System.Windows.Forms.CheckBox();
            this.button8 = new System.Windows.Forms.Button();
            this.checkBox4 = new System.Windows.Forms.CheckBox();
        //    this.glControl = new OpenTK.GLControl();
            this.edZ = new System.Windows.Forms.TextBox();
            this.edY = new System.Windows.Forms.TextBox();
            this.edX = new System.Windows.Forms.TextBox();
            this.btConfirmaCoord = new System.Windows.Forms.Button();
            this.pnDivBarras = new System.Windows.Forms.Panel();
            this.button10 = new System.Windows.Forms.Button();
            this.edCota = new System.Windows.Forms.TextBox();
            this.btConfirmaCota = new System.Windows.Forms.Button();
            this.lbUnidadeCota = new System.Windows.Forms.Label();
            this.timerSnap = new System.Windows.Forms.Timer(this.components);
            this.numericUpDown1 = new System.Windows.Forms.NumericUpDown();
            this.pnProgresso = new System.Windows.Forms.Panel();
            this.Progresso = new System.Windows.Forms.ProgressBar();
            this.pnRepeticoesCopia = new System.Windows.Forms.Panel();
            this.label5 = new System.Windows.Forms.Label();
            this.chCopiarCargas = new System.Windows.Forms.CheckBox();
            this.lbNumRepeticoes = new System.Windows.Forms.Label();
            this.edNumRepeticoes = new System.Windows.Forms.NumericUpDown();
            this.numericUpDown2 = new System.Windows.Forms.NumericUpDown();
            this.numericUpDown3 = new System.Windows.Forms.NumericUpDown();
            this.numericUpDown4 = new System.Windows.Forms.NumericUpDown();
            this.lbprogresso = new System.Windows.Forms.Label();
            this.timerZoom = new System.Windows.Forms.Timer(this.components);
            this.textBox7 = new System.Windows.Forms.TextBox();
            this.contextMenuStrip2 = new System.Windows.Forms.ContextMenuStrip(this.components);
            this.textBox8 = new System.Windows.Forms.TextBox();
            this.Comando = new System.Windows.Forms.Label();
            this.pnEspelhar = new System.Windows.Forms.Panel();
            this.label3 = new System.Windows.Forms.Label();
            this.chCopiarCargas_Espelhar = new System.Windows.Forms.CheckBox();
            this.pnMover = new System.Windows.Forms.Panel();
            this.label4 = new System.Windows.Forms.Label();
            this.chMoverManterConexoes = new System.Windows.Forms.CheckBox();
            this.pnRotacionar = new System.Windows.Forms.Panel();
            this.button9 = new System.Windows.Forms.Button();
            this.label7 = new System.Windows.Forms.Label();
            this.label9 = new System.Windows.Forms.Label();
            this.edAnguloRotacao = new System.Windows.Forms.TextBox();
            this.chApagarOriginalRotacao = new System.Windows.Forms.CheckBox();
            this.chCopiarCargasRotacao = new System.Windows.Forms.CheckBox();
            this.label8 = new System.Windows.Forms.Label();
            this.qtdRepeticoesRotacao = new System.Windows.Forms.NumericUpDown();
            this.label6 = new System.Windows.Forms.Label();
            this.menuPrincipal.SuspendLayout();
            this.pnDivBarras.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.numericUpDown1)).BeginInit();
            this.pnProgresso.SuspendLayout();
            this.pnRepeticoesCopia.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.edNumRepeticoes)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.numericUpDown2)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.numericUpDown3)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.numericUpDown4)).BeginInit();
            this.pnEspelhar.SuspendLayout();
            this.pnMover.SuspendLayout();
            this.pnRotacionar.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.qtdRepeticoesRotacao)).BeginInit();
            this.SuspendLayout();
            // 
            // toolTip1
            // 
            this.toolTip1.BackColor = System.Drawing.SystemColors.Desktop;
            this.toolTip1.ForeColor = System.Drawing.SystemColors.Highlight;
            this.toolTip1.IsBalloon = true;
            this.toolTip1.UseAnimation = false;
            this.toolTip1.UseFading = false;
            // 
            // btCoordRelativa
            // 
            this.btCoordRelativa.BackColor = System.Drawing.Color.Transparent;
            this.btCoordRelativa.FlatStyle = System.Windows.Forms.FlatStyle.Popup;
            this.btCoordRelativa.Font = new System.Drawing.Font("Tahoma", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btCoordRelativa.ForeColor = System.Drawing.Color.Black;
            this.btCoordRelativa.ImageIndex = 0;
            this.btCoordRelativa.ImageList = this.imageList1;
            this.btCoordRelativa.Location = new System.Drawing.Point(192, 393);
            this.btCoordRelativa.Name = "btCoordRelativa";
            this.btCoordRelativa.Size = new System.Drawing.Size(24, 21);
            this.btCoordRelativa.TabIndex = 73;
            this.toolTip1.SetToolTip(this.btCoordRelativa, "Coordenada relativa");
            this.btCoordRelativa.UseVisualStyleBackColor = false;
            this.btCoordRelativa.Visible = false;
            this.btCoordRelativa.Click += new System.EventHandler(this.btCoordRelativa_Click);
            // 
            // imageList1
            // 
            this.imageList1.ImageStream = ((System.Windows.Forms.ImageListStreamer)(resources.GetObject("imageList1.ImageStream")));
            this.imageList1.TransparentColor = System.Drawing.Color.Transparent;
            this.imageList1.Images.SetKeyName(0, "coordenada relativa - desabilitada.bmp");
            this.imageList1.Images.SetKeyName(1, "coordenada relativa.bmp");
            // 
            // printPreviewDialog1
            // 
            this.printPreviewDialog1.AutoScrollMargin = new System.Drawing.Size(0, 0);
            this.printPreviewDialog1.AutoScrollMinSize = new System.Drawing.Size(0, 0);
            this.printPreviewDialog1.ClientSize = new System.Drawing.Size(400, 300);
            this.printPreviewDialog1.Enabled = true;
            this.printPreviewDialog1.Icon = ((System.Drawing.Icon)(resources.GetObject("printPreviewDialog1.Icon")));
            this.printPreviewDialog1.Name = "printPreviewDialog1";
            this.printPreviewDialog1.Visible = false;
            // 
            // timer1
            // 
            this.timer1.Interval = 50;
            this.timer1.Tick += new System.EventHandler(this.timer1_Tick);
            // 
            // menuPrincipal
            // 
            this.menuPrincipal.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.editarBarras,
            this.editarApoios,
            this.editarCargaPontual,
            this.editarCargaDistribuida,
            this.toolStripMenuItem3,
            this.Elemento_rotacionar45,
            this.toolStripMenuItem5,
            this.copiarToolStripMenuItem,
            this.colarCtrlVToolStripMenuItem,
            this.procurarCtrlFToolStripMenuItem,
            this.toolStripMenuItem1,
            this.esconderSomenteToolStripMenuItem,
            this.mostrarSomenteToolStripMenuItem,
            this.mostrarTudoToolStripMenuItem,
            this.toolStripMenuItem2,
            this.toolStripMenuItem4,
            this.cancelarToolStripMenuItem});
            this.menuPrincipal.Name = "contextMenuStrip1";
            this.menuPrincipal.Size = new System.Drawing.Size(207, 298);
            this.menuPrincipal.Opening += new System.ComponentModel.CancelEventHandler(this.menuPrincipal_Opening);
            // 
            // editarBarras
            // 
            this.editarBarras.ImageScaling = System.Windows.Forms.ToolStripItemImageScaling.None;
            this.editarBarras.Name = "editarBarras";
            this.editarBarras.Size = new System.Drawing.Size(206, 22);
            this.editarBarras.Text = "Editar membro";
            this.editarBarras.Visible = false;
            this.editarBarras.Click += new System.EventHandler(this.editarBarras_Click);
            // 
            // editarApoios
            // 
            this.editarApoios.ImageScaling = System.Windows.Forms.ToolStripItemImageScaling.None;
            this.editarApoios.Name = "editarApoios";
            this.editarApoios.Size = new System.Drawing.Size(206, 22);
            this.editarApoios.Text = "Editar apoios";
            this.editarApoios.Visible = false;
            this.editarApoios.Click += new System.EventHandler(this.editarApoios_Click);
            // 
            // editarCargaPontual
            // 
            this.editarCargaPontual.ImageScaling = System.Windows.Forms.ToolStripItemImageScaling.None;
            this.editarCargaPontual.Name = "editarCargaPontual";
            this.editarCargaPontual.Size = new System.Drawing.Size(206, 22);
            this.editarCargaPontual.Text = "Editar carga nodal";
            this.editarCargaPontual.Click += new System.EventHandler(this.editarCargaPontual_Click);
            // 
            // editarCargaDistribuida
            // 
            this.editarCargaDistribuida.ImageScaling = System.Windows.Forms.ToolStripItemImageScaling.None;
            this.editarCargaDistribuida.Name = "editarCargaDistribuida";
            this.editarCargaDistribuida.Size = new System.Drawing.Size(206, 22);
            this.editarCargaDistribuida.Text = "Editar carga no elemento";
            this.editarCargaDistribuida.Click += new System.EventHandler(this.editarCargaDistribuida_Click);
            // 
            // toolStripMenuItem3
            // 
            this.toolStripMenuItem3.Name = "toolStripMenuItem3";
            this.toolStripMenuItem3.Size = new System.Drawing.Size(203, 6);
            // 
            // Elemento_rotacionar45
            // 
            this.Elemento_rotacionar45.Name = "Elemento_rotacionar45";
            this.Elemento_rotacionar45.Size = new System.Drawing.Size(206, 22);
            this.Elemento_rotacionar45.Tag = "Elemento";
            this.Elemento_rotacionar45.Text = "Rotacionar 45° (Ctrl + q)";
            this.Elemento_rotacionar45.Click += new System.EventHandler(this.rotacionar45CtrlQToolStripMenuItem_Click);
            // 
            // toolStripMenuItem5
            // 
            this.toolStripMenuItem5.Name = "toolStripMenuItem5";
            this.toolStripMenuItem5.Size = new System.Drawing.Size(203, 6);
            // 
            // copiarToolStripMenuItem
            // 
            this.copiarToolStripMenuItem.Name = "copiarToolStripMenuItem";
            this.copiarToolStripMenuItem.Size = new System.Drawing.Size(206, 22);
            this.copiarToolStripMenuItem.Text = "Copiar (Ctrl + c)";
            this.copiarToolStripMenuItem.Visible = false;
            this.copiarToolStripMenuItem.Click += new System.EventHandler(this.copiarToolStripMenuItem_Click);
            // 
            // colarCtrlVToolStripMenuItem
            // 
            this.colarCtrlVToolStripMenuItem.Name = "colarCtrlVToolStripMenuItem";
            this.colarCtrlVToolStripMenuItem.Size = new System.Drawing.Size(206, 22);
            this.colarCtrlVToolStripMenuItem.Text = "Colar (Ctrl + v)";
            this.colarCtrlVToolStripMenuItem.Visible = false;
            this.colarCtrlVToolStripMenuItem.Click += new System.EventHandler(this.colarCtrlVToolStripMenuItem_Click);
            // 
            // procurarCtrlFToolStripMenuItem
            // 
            this.procurarCtrlFToolStripMenuItem.Name = "procurarCtrlFToolStripMenuItem";
            this.procurarCtrlFToolStripMenuItem.Size = new System.Drawing.Size(206, 22);
            this.procurarCtrlFToolStripMenuItem.Text = "Localizar (Ctrl + F)";
            // 
            // toolStripMenuItem1
            // 
            this.toolStripMenuItem1.Name = "toolStripMenuItem1";
            this.toolStripMenuItem1.Size = new System.Drawing.Size(203, 6);
            // 
            // esconderSomenteToolStripMenuItem
            // 
            this.esconderSomenteToolStripMenuItem.Name = "esconderSomenteToolStripMenuItem";
            this.esconderSomenteToolStripMenuItem.Size = new System.Drawing.Size(206, 22);
            this.esconderSomenteToolStripMenuItem.Text = "Esconder";
            this.esconderSomenteToolStripMenuItem.Click += new System.EventHandler(this.esconderSomenteToolStripMenuItem_Click);
            // 
            // mostrarSomenteToolStripMenuItem
            // 
            this.mostrarSomenteToolStripMenuItem.Name = "mostrarSomenteToolStripMenuItem";
            this.mostrarSomenteToolStripMenuItem.Size = new System.Drawing.Size(206, 22);
            this.mostrarSomenteToolStripMenuItem.Text = "Mostrar somente";
            this.mostrarSomenteToolStripMenuItem.Click += new System.EventHandler(this.mostrarSomenteToolStripMenuItem_Click);
            // 
            // mostrarTudoToolStripMenuItem
            // 
            this.mostrarTudoToolStripMenuItem.Name = "mostrarTudoToolStripMenuItem";
            this.mostrarTudoToolStripMenuItem.Size = new System.Drawing.Size(206, 22);
            this.mostrarTudoToolStripMenuItem.Text = "Mostrar tudo";
            this.mostrarTudoToolStripMenuItem.Click += new System.EventHandler(this.mostrarTudoToolStripMenuItem_Click);
            // 
            // toolStripMenuItem2
            // 
            this.toolStripMenuItem2.Name = "toolStripMenuItem2";
            this.toolStripMenuItem2.Size = new System.Drawing.Size(203, 6);
            // 
            // toolStripMenuItem4
            // 
            this.toolStripMenuItem4.Name = "toolStripMenuItem4";
            this.toolStripMenuItem4.Size = new System.Drawing.Size(203, 6);
            // 
            // cancelarToolStripMenuItem
            // 
            this.cancelarToolStripMenuItem.ImageScaling = System.Windows.Forms.ToolStripItemImageScaling.None;
            this.cancelarToolStripMenuItem.Name = "cancelarToolStripMenuItem";
            this.cancelarToolStripMenuItem.Size = new System.Drawing.Size(206, 22);
            this.cancelarToolStripMenuItem.Text = "Cancelar";
            this.cancelarToolStripMenuItem.Click += new System.EventHandler(this.cancelarToolStripMenuItem_Click);
            // 
            // textBox1
            // 
            this.textBox1.Location = new System.Drawing.Point(12, 66);
            this.textBox1.Name = "textBox1";
            this.textBox1.Size = new System.Drawing.Size(342, 20);
            this.textBox1.TabIndex = 46;
            this.textBox1.Visible = false;
            // 
            // textBox2
            // 
            this.textBox2.ForeColor = System.Drawing.Color.DimGray;
            this.textBox2.Location = new System.Drawing.Point(340, 76);
            this.textBox2.Name = "textBox2";
            this.textBox2.Size = new System.Drawing.Size(100, 20);
            this.textBox2.TabIndex = 47;
            this.textBox2.Visible = false;
            // 
            // button1
            // 
            this.button1.Location = new System.Drawing.Point(228, 163);
            this.button1.Name = "button1";
            this.button1.Size = new System.Drawing.Size(75, 23);
            this.button1.TabIndex = 48;
            this.button1.Text = "button1";
            this.button1.UseVisualStyleBackColor = true;
            this.button1.Visible = false;
            this.button1.Click += new System.EventHandler(this.button1_Click);
            // 
            // button2
            // 
            this.button2.Location = new System.Drawing.Point(0, 0);
            this.button2.Name = "button2";
            this.button2.Size = new System.Drawing.Size(75, 23);
            this.button2.TabIndex = 51;
            this.button2.Text = "button2";
            this.button2.UseVisualStyleBackColor = true;
            this.button2.Visible = false;
            this.button2.Click += new System.EventHandler(this.button2_Click);
            // 
            // textBox4
            // 
            this.textBox4.Location = new System.Drawing.Point(12, 92);
            this.textBox4.Multiline = true;
            this.textBox4.Name = "textBox4";
            this.textBox4.Size = new System.Drawing.Size(342, 56);
            this.textBox4.TabIndex = 49;
            this.textBox4.Visible = false;
            // 
            // textBox3
            // 
            this.textBox3.Location = new System.Drawing.Point(470, 76);
            this.textBox3.Name = "textBox3";
            this.textBox3.Size = new System.Drawing.Size(100, 20);
            this.textBox3.TabIndex = 50;
            this.textBox3.Visible = false;
            // 
            // textBox5
            // 
            this.textBox5.Location = new System.Drawing.Point(351, 140);
            this.textBox5.Name = "textBox5";
            this.textBox5.Size = new System.Drawing.Size(100, 20);
            this.textBox5.TabIndex = 53;
            this.textBox5.Visible = false;
            // 
            // textBox6
            // 
            this.textBox6.Location = new System.Drawing.Point(467, 140);
            this.textBox6.Name = "textBox6";
            this.textBox6.Size = new System.Drawing.Size(100, 20);
            this.textBox6.TabIndex = 52;
            this.textBox6.Visible = false;
            // 
            // button3
            // 
            this.button3.Location = new System.Drawing.Point(0, 140);
            this.button3.Name = "button3";
            this.button3.Size = new System.Drawing.Size(75, 23);
            this.button3.TabIndex = 54;
            this.button3.Text = "dif x";
            this.button3.UseVisualStyleBackColor = true;
            this.button3.Visible = false;
            this.button3.Click += new System.EventHandler(this.button3_Click_1);
            // 
            // button4
            // 
            this.button4.Location = new System.Drawing.Point(116, 140);
            this.button4.Name = "button4";
            this.button4.Size = new System.Drawing.Size(75, 23);
            this.button4.TabIndex = 55;
            this.button4.Text = "dif y";
            this.button4.UseVisualStyleBackColor = true;
            this.button4.Visible = false;
            this.button4.Click += new System.EventHandler(this.button4_Click);
            // 
            // button5
            // 
            this.button5.Location = new System.Drawing.Point(131, 173);
            this.button5.Name = "button5";
            this.button5.Size = new System.Drawing.Size(75, 23);
            this.button5.TabIndex = 56;
            this.button5.Text = "button5";
            this.button5.UseVisualStyleBackColor = true;
            this.button5.Visible = false;
            this.button5.Click += new System.EventHandler(this.button5_Click);
            // 
            // button6
            // 
            this.button6.BackColor = System.Drawing.Color.Black;
            this.button6.Location = new System.Drawing.Point(357, 173);
            this.button6.Name = "button6";
            this.button6.Size = new System.Drawing.Size(75, 23);
            this.button6.TabIndex = 57;
            this.button6.Text = "roda Y";
            this.button6.UseVisualStyleBackColor = false;
            this.button6.Visible = false;
            this.button6.Click += new System.EventHandler(this.button6_Click);
            // 
            // button7
            // 
            this.button7.BackColor = System.Drawing.Color.Black;
            this.button7.Location = new System.Drawing.Point(467, 173);
            this.button7.Name = "button7";
            this.button7.Size = new System.Drawing.Size(75, 23);
            this.button7.TabIndex = 58;
            this.button7.Text = "roda Z";
            this.button7.UseVisualStyleBackColor = false;
            this.button7.Visible = false;
            this.button7.Click += new System.EventHandler(this.button7_Click);
            // 
            // checkBox1
            // 
            this.checkBox1.AutoSize = true;
            this.checkBox1.BackColor = System.Drawing.Color.Black;
            this.checkBox1.Location = new System.Drawing.Point(357, 79);
            this.checkBox1.Name = "checkBox1";
            this.checkBox1.Size = new System.Drawing.Size(50, 17);
            this.checkBox1.TabIndex = 59;
            this.checkBox1.Text = "trava";
            this.checkBox1.UseVisualStyleBackColor = false;
            this.checkBox1.Visible = false;
            this.checkBox1.CheckedChanged += new System.EventHandler(this.checkBox1_CheckedChanged);
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.ForeColor = System.Drawing.Color.Black;
            this.label1.Location = new System.Drawing.Point(348, 121);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(21, 13);
            this.label1.TabIndex = 60;
            this.label1.Text = "XY";
            this.label1.Visible = false;
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.ForeColor = System.Drawing.Color.Black;
            this.label2.Location = new System.Drawing.Point(470, 121);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(21, 13);
            this.label2.TabIndex = 61;
            this.label2.Text = "XZ";
            this.label2.Visible = false;
            // 
            // checkBox2
            // 
            this.checkBox2.AutoSize = true;
            this.checkBox2.BackColor = System.Drawing.Color.Black;
            this.checkBox2.Location = new System.Drawing.Point(487, 79);
            this.checkBox2.Name = "checkBox2";
            this.checkBox2.Size = new System.Drawing.Size(65, 17);
            this.checkBox2.TabIndex = 62;
            this.checkBox2.Text = "RODAR";
            this.checkBox2.UseVisualStyleBackColor = false;
            this.checkBox2.Visible = false;
            // 
            // checkBox3
            // 
            this.checkBox3.AutoSize = true;
            this.checkBox3.BackColor = System.Drawing.Color.Black;
            this.checkBox3.Location = new System.Drawing.Point(371, 217);
            this.checkBox3.Name = "checkBox3";
            this.checkBox3.Size = new System.Drawing.Size(69, 17);
            this.checkBox3.TabIndex = 63;
            this.checkBox3.Text = "translada";
            this.checkBox3.UseVisualStyleBackColor = false;
            this.checkBox3.Visible = false;
            // 
            // button8
            // 
            this.button8.ForeColor = System.Drawing.Color.Black;
            this.button8.Location = new System.Drawing.Point(393, 266);
            this.button8.Name = "button8";
            this.button8.Size = new System.Drawing.Size(75, 23);
            this.button8.TabIndex = 64;
            this.button8.Text = "pan!";
            this.button8.UseVisualStyleBackColor = true;
            this.button8.Visible = false;
            this.button8.Click += new System.EventHandler(this.button8_Click);
            // 
            // checkBox4
            // 
            this.checkBox4.AutoSize = true;
            this.checkBox4.Location = new System.Drawing.Point(490, 207);
            this.checkBox4.Name = "checkBox4";
            this.checkBox4.Size = new System.Drawing.Size(80, 17);
            this.checkBox4.TabIndex = 65;
            this.checkBox4.Text = "checkBox4";
            this.checkBox4.UseVisualStyleBackColor = true;
            this.checkBox4.Visible = false;
            // 
            // glControl
            // 
            this.glControl.BackColor = System.Drawing.Color.Black;
            this.glControl.Cursor = System.Windows.Forms.Cursors.Arrow;
            this.glControl.Dock = System.Windows.Forms.DockStyle.Fill;
            this.glControl.Location = new System.Drawing.Point(0, 0);
            this.glControl.Name = "glControl";
            this.glControl.Size = new System.Drawing.Size(934, 519);
            this.glControl.TabIndex = 66;
            this.glControl.VSync = false;
            this.glControl.Load += new System.EventHandler(this.glControl_Load);
            this.glControl.Click += new System.EventHandler(this.glControl_Click);
            this.glControl.KeyDown += new System.Windows.Forms.KeyEventHandler(this.glControl_KeyDown);
            this.glControl.MouseDoubleClick += new System.Windows.Forms.MouseEventHandler(this.glControl_MouseDoubleClick);
            this.glControl.MouseDown += new System.Windows.Forms.MouseEventHandler(this.glControl_MouseDown);
            this.glControl.MouseEnter += new System.EventHandler(this.glControl_MouseEnter);
            this.glControl.MouseMove += new System.Windows.Forms.MouseEventHandler(this.glControl_MouseMove);
            this.glControl.MouseUp += new System.Windows.Forms.MouseEventHandler(this.glControl_MouseUp);
            this.glControl.Resize += new System.EventHandler(this.glControl_Resize);
            // 
            // edZ
            // 
            this.edZ.BackColor = System.Drawing.SystemColors.InactiveBorder;
            this.edZ.Location = new System.Drawing.Point(339, 393);
            this.edZ.Name = "edZ";
            this.edZ.Size = new System.Drawing.Size(50, 20);
            this.edZ.TabIndex = 70;
            this.edZ.Visible = false;
            this.edZ.KeyPress += new System.Windows.Forms.KeyPressEventHandler(this.edX_KeyPress);
            this.edZ.KeyUp += new System.Windows.Forms.KeyEventHandler(this.edZ_KeyUp);
            // 
            // edY
            // 
            this.edY.BackColor = System.Drawing.SystemColors.InactiveBorder;
            this.edY.Location = new System.Drawing.Point(283, 393);
            this.edY.Name = "edY";
            this.edY.Size = new System.Drawing.Size(50, 20);
            this.edY.TabIndex = 69;
            this.edY.Visible = false;
            this.edY.KeyPress += new System.Windows.Forms.KeyPressEventHandler(this.edX_KeyPress);
            this.edY.KeyUp += new System.Windows.Forms.KeyEventHandler(this.edY_KeyUp);
            // 
            // edX
            // 
            this.edX.BackColor = System.Drawing.SystemColors.InactiveBorder;
            this.edX.Location = new System.Drawing.Point(222, 393);
            this.edX.Name = "edX";
            this.edX.Size = new System.Drawing.Size(50, 20);
            this.edX.TabIndex = 68;
            this.edX.Visible = false;
            this.edX.KeyPress += new System.Windows.Forms.KeyPressEventHandler(this.edX_KeyPress);
            this.edX.KeyUp += new System.Windows.Forms.KeyEventHandler(this.edX_KeyUp);
            // 
            // btConfirmaCoord
            // 
            this.btConfirmaCoord.BackColor = System.Drawing.Color.Transparent;
            this.btConfirmaCoord.FlatStyle = System.Windows.Forms.FlatStyle.Popup;
            this.btConfirmaCoord.ForeColor = System.Drawing.Color.Transparent;
            this.btConfirmaCoord.Image = global::PGi.Properties.Resources.conf_cota1;
            this.btConfirmaCoord.Location = new System.Drawing.Point(427, 363);
            this.btConfirmaCoord.Name = "btConfirmaCoord";
            this.btConfirmaCoord.Size = new System.Drawing.Size(24, 21);
            this.btConfirmaCoord.TabIndex = 72;
            this.btConfirmaCoord.UseVisualStyleBackColor = false;
            this.btConfirmaCoord.Visible = false;
            this.btConfirmaCoord.Click += new System.EventHandler(this.btConfirmaCoord_Click);
            // 
            // pnDivBarras
            // 
            this.pnDivBarras.BackColor = System.Drawing.SystemColors.HighlightText;
            this.pnDivBarras.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.pnDivBarras.Controls.Add(this.button10);
            this.pnDivBarras.Controls.Add(this.edCota);
            this.pnDivBarras.Controls.Add(this.btConfirmaCota);
            this.pnDivBarras.Controls.Add(this.lbUnidadeCota);
            this.pnDivBarras.Font = new System.Drawing.Font("Microsoft Sans Serif", 10F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Pixel, ((byte)(0)));
            this.pnDivBarras.Location = new System.Drawing.Point(271, 207);
            this.pnDivBarras.Name = "pnDivBarras";
            this.pnDivBarras.Size = new System.Drawing.Size(134, 32);
            this.pnDivBarras.TabIndex = 74;
            // 
            // button10
            // 
            this.button10.FlatStyle = System.Windows.Forms.FlatStyle.Popup;
            this.button10.Font = new System.Drawing.Font("Microsoft Sans Serif", 15F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Pixel, ((byte)(0)));
            this.button10.Image = ((System.Drawing.Image)(resources.GetObject("button10.Image")));
            this.button10.Location = new System.Drawing.Point(108, 2);
            this.button10.Name = "button10";
            this.button10.Size = new System.Drawing.Size(22, 22);
            this.button10.TabIndex = 158;
            this.button10.TextAlign = System.Drawing.ContentAlignment.TopCenter;
            this.button10.UseVisualStyleBackColor = true;
            this.button10.Click += new System.EventHandler(this.button10_Click);
            // 
            // edCota
            // 
            this.edCota.BackColor = System.Drawing.SystemColors.InactiveBorder;
            this.edCota.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.edCota.Location = new System.Drawing.Point(1, 4);
            this.edCota.Name = "edCota";
            this.edCota.Size = new System.Drawing.Size(50, 19);
            this.edCota.TabIndex = 156;
            this.edCota.KeyPress += new System.Windows.Forms.KeyPressEventHandler(this.edX_KeyPress);
            this.edCota.KeyUp += new System.Windows.Forms.KeyEventHandler(this.edCota_KeyUp);
            // 
            // btConfirmaCota
            // 
            this.btConfirmaCota.FlatStyle = System.Windows.Forms.FlatStyle.Popup;
            this.btConfirmaCota.Font = new System.Drawing.Font("Microsoft Sans Serif", 15F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Pixel, ((byte)(0)));
            this.btConfirmaCota.Image = ((System.Drawing.Image)(resources.GetObject("btConfirmaCota.Image")));
            this.btConfirmaCota.Location = new System.Drawing.Point(84, 2);
            this.btConfirmaCota.Name = "btConfirmaCota";
            this.btConfirmaCota.Size = new System.Drawing.Size(22, 22);
            this.btConfirmaCota.TabIndex = 157;
            this.btConfirmaCota.TextAlign = System.Drawing.ContentAlignment.TopCenter;
            this.btConfirmaCota.UseVisualStyleBackColor = true;
            this.btConfirmaCota.Click += new System.EventHandler(this.button9_Click);
            // 
            // lbUnidadeCota
            // 
            this.lbUnidadeCota.AutoSize = true;
            this.lbUnidadeCota.Font = new System.Drawing.Font("Tahoma", 10F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Pixel, ((byte)(0)));
            this.lbUnidadeCota.ForeColor = System.Drawing.Color.Black;
            this.lbUnidadeCota.Location = new System.Drawing.Point(51, 11);
            this.lbUnidadeCota.Name = "lbUnidadeCota";
            this.lbUnidadeCota.Size = new System.Drawing.Size(13, 12);
            this.lbUnidadeCota.TabIndex = 154;
            this.lbUnidadeCota.Text = "m";
            // 
            // timerSnap
            // 
            this.timerSnap.Interval = 1500;
            this.timerSnap.Tick += new System.EventHandler(this.timerSnap_Tick);
            // 
            // numericUpDown1
            // 
            this.numericUpDown1.Location = new System.Drawing.Point(485, 266);
            this.numericUpDown1.Minimum = new decimal(new int[] {
            1,
            0,
            0,
            0});
            this.numericUpDown1.Name = "numericUpDown1";
            this.numericUpDown1.Size = new System.Drawing.Size(57, 20);
            this.numericUpDown1.TabIndex = 75;
            this.numericUpDown1.Value = new decimal(new int[] {
            1,
            0,
            0,
            0});
            this.numericUpDown1.Visible = false;
            this.numericUpDown1.ValueChanged += new System.EventHandler(this.numericUpDown1_ValueChanged);
            // 
            // pnProgresso
            // 
            this.pnProgresso.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.pnProgresso.Controls.Add(this.Progresso);
            this.pnProgresso.Location = new System.Drawing.Point(14, 363);
            this.pnProgresso.Name = "pnProgresso";
            this.pnProgresso.Size = new System.Drawing.Size(258, 32);
            this.pnProgresso.TabIndex = 76;
            this.pnProgresso.Visible = false;
            // 
            // Progresso
            // 
            this.Progresso.Location = new System.Drawing.Point(150, 14);
            this.Progresso.Name = "Progresso";
            this.Progresso.Size = new System.Drawing.Size(266, 13);
            this.Progresso.Step = 1;
            this.Progresso.TabIndex = 0;
            this.Progresso.Visible = false;
            // 
            // pnRepeticoesCopia
            // 
            this.pnRepeticoesCopia.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(64)))), ((int)(((byte)(64)))), ((int)(((byte)(64)))));
            this.pnRepeticoesCopia.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.pnRepeticoesCopia.Controls.Add(this.label5);
            this.pnRepeticoesCopia.Controls.Add(this.chCopiarCargas);
            this.pnRepeticoesCopia.Controls.Add(this.lbNumRepeticoes);
            this.pnRepeticoesCopia.Controls.Add(this.edNumRepeticoes);
            this.pnRepeticoesCopia.Cursor = System.Windows.Forms.Cursors.Arrow;
            this.pnRepeticoesCopia.Location = new System.Drawing.Point(228, 279);
            this.pnRepeticoesCopia.Name = "pnRepeticoesCopia";
            this.pnRepeticoesCopia.Size = new System.Drawing.Size(240, 46);
            this.pnRepeticoesCopia.TabIndex = 80;
            this.pnRepeticoesCopia.Visible = false;
            this.pnRepeticoesCopia.Validated += new System.EventHandler(this.pnRepeticoesCopia_Validated);
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(64)))), ((int)(((byte)(64)))), ((int)(((byte)(64)))));
            this.label5.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.label5.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label5.ForeColor = System.Drawing.Color.Silver;
            this.label5.Location = new System.Drawing.Point(1, 2);
            this.label5.Name = "label5";
            this.label5.RightToLeft = System.Windows.Forms.RightToLeft.No;
            this.label5.Size = new System.Drawing.Size(47, 13);
            this.label5.TabIndex = 84;
            this.label5.Text = "COPIAR";
            this.label5.TextAlign = System.Drawing.ContentAlignment.TopCenter;
            // 
            // chCopiarCargas
            // 
            this.chCopiarCargas.AutoSize = true;
            this.chCopiarCargas.BackColor = System.Drawing.Color.Transparent;
            this.chCopiarCargas.Checked = true;
            this.chCopiarCargas.CheckState = System.Windows.Forms.CheckState.Checked;
            this.chCopiarCargas.ForeColor = System.Drawing.Color.White;
            this.chCopiarCargas.Location = new System.Drawing.Point(144, 29);
            this.chCopiarCargas.Name = "chCopiarCargas";
            this.chCopiarCargas.Size = new System.Drawing.Size(91, 17);
            this.chCopiarCargas.TabIndex = 82;
            this.chCopiarCargas.Text = "Copiar cargas";
            this.chCopiarCargas.UseVisualStyleBackColor = false;
            // 
            // lbNumRepeticoes
            // 
            this.lbNumRepeticoes.AutoSize = true;
            this.lbNumRepeticoes.ForeColor = System.Drawing.Color.White;
            this.lbNumRepeticoes.Location = new System.Drawing.Point(-1, 29);
            this.lbNumRepeticoes.Name = "lbNumRepeticoes";
            this.lbNumRepeticoes.RightToLeft = System.Windows.Forms.RightToLeft.No;
            this.lbNumRepeticoes.Size = new System.Drawing.Size(93, 13);
            this.lbNumRepeticoes.TabIndex = 81;
            this.lbNumRepeticoes.Text = "Número de cópias";
            // 
            // edNumRepeticoes
            // 
            this.edNumRepeticoes.Location = new System.Drawing.Point(93, 22);
            this.edNumRepeticoes.Minimum = new decimal(new int[] {
            1,
            0,
            0,
            0});
            this.edNumRepeticoes.Name = "edNumRepeticoes";
            this.edNumRepeticoes.Size = new System.Drawing.Size(41, 20);
            this.edNumRepeticoes.TabIndex = 80;
            this.edNumRepeticoes.Value = new decimal(new int[] {
            1,
            0,
            0,
            0});
            this.edNumRepeticoes.ValueChanged += new System.EventHandler(this.edNumRepeticoes_ValueChanged);
            // 
            // numericUpDown2
            // 
            this.numericUpDown2.Location = new System.Drawing.Point(34, 228);
            this.numericUpDown2.Maximum = new decimal(new int[] {
            10000,
            0,
            0,
            0});
            this.numericUpDown2.Minimum = new decimal(new int[] {
            10000,
            0,
            0,
            -2147483648});
            this.numericUpDown2.Name = "numericUpDown2";
            this.numericUpDown2.Size = new System.Drawing.Size(57, 20);
            this.numericUpDown2.TabIndex = 81;
            this.numericUpDown2.Visible = false;
            this.numericUpDown2.ValueChanged += new System.EventHandler(this.numericUpDown2_ValueChanged);
            // 
            // numericUpDown3
            // 
            this.numericUpDown3.Location = new System.Drawing.Point(34, 254);
            this.numericUpDown3.Maximum = new decimal(new int[] {
            10000,
            0,
            0,
            0});
            this.numericUpDown3.Minimum = new decimal(new int[] {
            10000,
            0,
            0,
            -2147483648});
            this.numericUpDown3.Name = "numericUpDown3";
            this.numericUpDown3.Size = new System.Drawing.Size(57, 20);
            this.numericUpDown3.TabIndex = 82;
            this.numericUpDown3.Visible = false;
            this.numericUpDown3.ValueChanged += new System.EventHandler(this.numericUpDown2_ValueChanged);
            // 
            // numericUpDown4
            // 
            this.numericUpDown4.Location = new System.Drawing.Point(34, 279);
            this.numericUpDown4.Maximum = new decimal(new int[] {
            10000,
            0,
            0,
            0});
            this.numericUpDown4.Minimum = new decimal(new int[] {
            10000,
            0,
            0,
            -2147483648});
            this.numericUpDown4.Name = "numericUpDown4";
            this.numericUpDown4.Size = new System.Drawing.Size(57, 20);
            this.numericUpDown4.TabIndex = 83;
            this.numericUpDown4.Visible = false;
            this.numericUpDown4.ValueChanged += new System.EventHandler(this.numericUpDown2_ValueChanged);
            // 
            // lbprogresso
            // 
            this.lbprogresso.AutoSize = true;
            this.lbprogresso.BackColor = System.Drawing.Color.Transparent;
            this.lbprogresso.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.lbprogresso.Font = new System.Drawing.Font("Arial", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(5)));
            this.lbprogresso.ForeColor = System.Drawing.Color.Black;
            this.lbprogresso.Location = new System.Drawing.Point(262, 256);
            this.lbprogresso.Name = "lbprogresso";
            this.lbprogresso.Size = new System.Drawing.Size(83, 20);
            this.lbprogresso.TabIndex = 84;
            this.lbprogresso.Text = "Progresso";
            this.lbprogresso.Visible = false;
            // 
            // timerZoom
            // 
            this.timerZoom.Interval = 1000;
            // 
            // textBox7
            // 
            this.textBox7.Dock = System.Windows.Forms.DockStyle.Top;
            this.textBox7.Location = new System.Drawing.Point(0, 0);
            this.textBox7.Name = "textBox7";
            this.textBox7.Size = new System.Drawing.Size(934, 20);
            this.textBox7.TabIndex = 85;
            this.textBox7.Visible = false;
            // 
            // contextMenuStrip2
            // 
            this.contextMenuStrip2.Name = "contextMenuStrip2";
            this.contextMenuStrip2.Size = new System.Drawing.Size(61, 4);
            // 
            // textBox8
            // 
            this.textBox8.Dock = System.Windows.Forms.DockStyle.Top;
            this.textBox8.Location = new System.Drawing.Point(0, 20);
            this.textBox8.Name = "textBox8";
            this.textBox8.Size = new System.Drawing.Size(934, 20);
            this.textBox8.TabIndex = 86;
            this.textBox8.Visible = false;
            // 
            // Comando
            // 
            this.Comando.AutoSize = true;
            this.Comando.BackColor = System.Drawing.Color.Maroon;
            this.Comando.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.Comando.Font = new System.Drawing.Font("Tahoma", 8F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.Comando.ForeColor = System.Drawing.Color.WhiteSmoke;
            this.Comando.Location = new System.Drawing.Point(0, 506);
            this.Comando.Name = "Comando";
            this.Comando.Size = new System.Drawing.Size(60, 13);
            this.Comando.TabIndex = 155;
            this.Comando.Text = "Comando";
            // 
            // pnEspelhar
            // 
            this.pnEspelhar.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(64)))), ((int)(((byte)(64)))), ((int)(((byte)(64)))));
            this.pnEspelhar.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.pnEspelhar.Controls.Add(this.label3);
            this.pnEspelhar.Controls.Add(this.chCopiarCargas_Espelhar);
            this.pnEspelhar.Cursor = System.Windows.Forms.Cursors.Arrow;
            this.pnEspelhar.Location = new System.Drawing.Point(485, 335);
            this.pnEspelhar.Name = "pnEspelhar";
            this.pnEspelhar.Size = new System.Drawing.Size(138, 41);
            this.pnEspelhar.TabIndex = 156;
            this.pnEspelhar.Visible = false;
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(64)))), ((int)(((byte)(64)))), ((int)(((byte)(64)))));
            this.label3.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label3.ForeColor = System.Drawing.Color.Silver;
            this.label3.Location = new System.Drawing.Point(0, 0);
            this.label3.Name = "label3";
            this.label3.RightToLeft = System.Windows.Forms.RightToLeft.No;
            this.label3.Size = new System.Drawing.Size(64, 13);
            this.label3.TabIndex = 83;
            this.label3.Text = "ESPELHAR";
            this.label3.TextAlign = System.Drawing.ContentAlignment.TopCenter;
            // 
            // chCopiarCargas_Espelhar
            // 
            this.chCopiarCargas_Espelhar.AutoSize = true;
            this.chCopiarCargas_Espelhar.BackColor = System.Drawing.Color.Transparent;
            this.chCopiarCargas_Espelhar.Checked = true;
            this.chCopiarCargas_Espelhar.CheckState = System.Windows.Forms.CheckState.Checked;
            this.chCopiarCargas_Espelhar.ForeColor = System.Drawing.Color.White;
            this.chCopiarCargas_Espelhar.Location = new System.Drawing.Point(26, 23);
            this.chCopiarCargas_Espelhar.Name = "chCopiarCargas_Espelhar";
            this.chCopiarCargas_Espelhar.Size = new System.Drawing.Size(91, 17);
            this.chCopiarCargas_Espelhar.TabIndex = 82;
            this.chCopiarCargas_Espelhar.Text = "Copiar cargas";
            this.chCopiarCargas_Espelhar.UseVisualStyleBackColor = false;
            // 
            // pnMover
            // 
            this.pnMover.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(64)))), ((int)(((byte)(64)))), ((int)(((byte)(64)))));
            this.pnMover.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.pnMover.Controls.Add(this.label4);
            this.pnMover.Controls.Add(this.chMoverManterConexoes);
            this.pnMover.Cursor = System.Windows.Forms.Cursors.Arrow;
            this.pnMover.Location = new System.Drawing.Point(425, 390);
            this.pnMover.Name = "pnMover";
            this.pnMover.Size = new System.Drawing.Size(232, 38);
            this.pnMover.TabIndex = 157;
            this.pnMover.Visible = false;
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(64)))), ((int)(((byte)(64)))), ((int)(((byte)(64)))));
            this.label4.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label4.ForeColor = System.Drawing.Color.White;
            this.label4.Location = new System.Drawing.Point(0, 0);
            this.label4.Name = "label4";
            this.label4.RightToLeft = System.Windows.Forms.RightToLeft.No;
            this.label4.Size = new System.Drawing.Size(46, 13);
            this.label4.TabIndex = 83;
            this.label4.Text = "MOVER";
            this.label4.TextAlign = System.Drawing.ContentAlignment.TopCenter;
            // 
            // chMoverManterConexoes
            // 
            this.chMoverManterConexoes.AutoSize = true;
            this.chMoverManterConexoes.BackColor = System.Drawing.Color.Transparent;
            this.chMoverManterConexoes.ForeColor = System.Drawing.Color.White;
            this.chMoverManterConexoes.Location = new System.Drawing.Point(3, 20);
            this.chMoverManterConexoes.Name = "chMoverManterConexoes";
            this.chMoverManterConexoes.Size = new System.Drawing.Size(222, 17);
            this.chMoverManterConexoes.TabIndex = 82;
            this.chMoverManterConexoes.Text = "Manter todos as barras e nós conectados";
            this.chMoverManterConexoes.UseVisualStyleBackColor = false;
            // 
            // pnRotacionar
            // 
            this.pnRotacionar.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(64)))), ((int)(((byte)(64)))), ((int)(((byte)(64)))));
            this.pnRotacionar.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.pnRotacionar.Controls.Add(this.button9);
            this.pnRotacionar.Controls.Add(this.label7);
            this.pnRotacionar.Controls.Add(this.label9);
            this.pnRotacionar.Controls.Add(this.edAnguloRotacao);
            this.pnRotacionar.Controls.Add(this.chApagarOriginalRotacao);
            this.pnRotacionar.Controls.Add(this.chCopiarCargasRotacao);
            this.pnRotacionar.Controls.Add(this.label8);
            this.pnRotacionar.Controls.Add(this.qtdRepeticoesRotacao);
            this.pnRotacionar.Controls.Add(this.label6);
            this.pnRotacionar.Cursor = System.Windows.Forms.Cursors.Arrow;
            this.pnRotacionar.Location = new System.Drawing.Point(586, 177);
            this.pnRotacionar.Name = "pnRotacionar";
            this.pnRotacionar.Size = new System.Drawing.Size(232, 99);
            this.pnRotacionar.TabIndex = 158;
            this.pnRotacionar.Visible = false;
            this.pnRotacionar.MouseDown += new System.Windows.Forms.MouseEventHandler(this.pnRotacionar_MouseDown);
            this.pnRotacionar.MouseMove += new System.Windows.Forms.MouseEventHandler(this.pnRotacionar_MouseMove);
            this.pnRotacionar.MouseUp += new System.Windows.Forms.MouseEventHandler(this.pnRotacionar_MouseUp);
            // 
            // button9
            // 
            this.button9.BackColor = System.Drawing.Color.DimGray;
            this.button9.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.button9.ForeColor = System.Drawing.Color.Azure;
            this.button9.Location = new System.Drawing.Point(204, 0);
            this.button9.Name = "button9";
            this.button9.Size = new System.Drawing.Size(25, 21);
            this.button9.TabIndex = 129;
            this.button9.Text = "X";
            this.button9.UseVisualStyleBackColor = false;
            this.button9.Click += new System.EventHandler(this.button9_Click_1);
            // 
            // label7
            // 
            this.label7.AutoSize = true;
            this.label7.ForeColor = System.Drawing.Color.White;
            this.label7.Location = new System.Drawing.Point(8, 56);
            this.label7.Name = "label7";
            this.label7.Size = new System.Drawing.Size(40, 13);
            this.label7.TabIndex = 128;
            this.label7.Text = "Ângulo";
            // 
            // label9
            // 
            this.label9.AutoSize = true;
            this.label9.ForeColor = System.Drawing.Color.White;
            this.label9.Location = new System.Drawing.Point(153, 54);
            this.label9.Name = "label9";
            this.label9.Size = new System.Drawing.Size(33, 13);
            this.label9.TabIndex = 127;
            this.label9.Text = "graus";
            // 
            // edAnguloRotacao
            // 
            this.edAnguloRotacao.Location = new System.Drawing.Point(107, 49);
            this.edAnguloRotacao.Name = "edAnguloRotacao";
            this.edAnguloRotacao.Size = new System.Drawing.Size(44, 20);
            this.edAnguloRotacao.TabIndex = 126;
            this.edAnguloRotacao.Text = "90";
            this.edAnguloRotacao.KeyPress += new System.Windows.Forms.KeyPressEventHandler(this.edX_KeyPress);
            // 
            // chApagarOriginalRotacao
            // 
            this.chApagarOriginalRotacao.AutoSize = true;
            this.chApagarOriginalRotacao.Enabled = false;
            this.chApagarOriginalRotacao.ForeColor = System.Drawing.Color.White;
            this.chApagarOriginalRotacao.Location = new System.Drawing.Point(11, 78);
            this.chApagarOriginalRotacao.Name = "chApagarOriginalRotacao";
            this.chApagarOriginalRotacao.Size = new System.Drawing.Size(105, 17);
            this.chApagarOriginalRotacao.TabIndex = 125;
            this.chApagarOriginalRotacao.Text = "Apagar o original";
            this.chApagarOriginalRotacao.UseVisualStyleBackColor = true;
            // 
            // chCopiarCargasRotacao
            // 
            this.chCopiarCargasRotacao.AutoSize = true;
            this.chCopiarCargasRotacao.BackColor = System.Drawing.Color.Transparent;
            this.chCopiarCargasRotacao.Checked = true;
            this.chCopiarCargasRotacao.CheckState = System.Windows.Forms.CheckState.Checked;
            this.chCopiarCargasRotacao.ForeColor = System.Drawing.Color.White;
            this.chCopiarCargasRotacao.Location = new System.Drawing.Point(122, 78);
            this.chCopiarCargasRotacao.Name = "chCopiarCargasRotacao";
            this.chCopiarCargasRotacao.Size = new System.Drawing.Size(91, 17);
            this.chCopiarCargasRotacao.TabIndex = 124;
            this.chCopiarCargasRotacao.Text = "Copiar cargas";
            this.chCopiarCargasRotacao.UseVisualStyleBackColor = false;
            // 
            // label8
            // 
            this.label8.AutoSize = true;
            this.label8.Location = new System.Drawing.Point(8, 31);
            this.label8.Name = "label8";
            this.label8.Size = new System.Drawing.Size(93, 13);
            this.label8.TabIndex = 123;
            this.label8.Text = "Número de cópias";
            // 
            // qtdRepeticoesRotacao
            // 
            this.qtdRepeticoesRotacao.Location = new System.Drawing.Point(107, 24);
            this.qtdRepeticoesRotacao.Name = "qtdRepeticoesRotacao";
            this.qtdRepeticoesRotacao.Size = new System.Drawing.Size(44, 20);
            this.qtdRepeticoesRotacao.TabIndex = 122;
            this.qtdRepeticoesRotacao.ValueChanged += new System.EventHandler(this.qtdRepeticoesRotacao_ValueChanged);
            // 
            // label6
            // 
            this.label6.AutoSize = true;
            this.label6.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(64)))), ((int)(((byte)(64)))), ((int)(((byte)(64)))));
            this.label6.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label6.ForeColor = System.Drawing.Color.White;
            this.label6.Location = new System.Drawing.Point(0, 0);
            this.label6.Name = "label6";
            this.label6.RightToLeft = System.Windows.Forms.RightToLeft.No;
            this.label6.Size = new System.Drawing.Size(78, 13);
            this.label6.TabIndex = 83;
            this.label6.Text = "ROTACIONAR";
            this.label6.TextAlign = System.Drawing.ContentAlignment.TopCenter;
            // 
            // FPrincipal
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.White;
            this.ClientSize = new System.Drawing.Size(934, 519);
            this.ContextMenuStrip = this.menuPrincipal;
            this.Controls.Add(this.pnRotacionar);
            this.Controls.Add(this.pnMover);
            this.Controls.Add(this.pnEspelhar);
            this.Controls.Add(this.Comando);
            this.Controls.Add(this.textBox8);
            this.Controls.Add(this.textBox7);
            this.Controls.Add(this.lbprogresso);
            this.Controls.Add(this.numericUpDown4);
            this.Controls.Add(this.numericUpDown3);
            this.Controls.Add(this.numericUpDown2);
            this.Controls.Add(this.pnRepeticoesCopia);
            this.Controls.Add(this.pnProgresso);
            this.Controls.Add(this.numericUpDown1);
            this.Controls.Add(this.pnDivBarras);
            this.Controls.Add(this.btCoordRelativa);
            this.Controls.Add(this.btConfirmaCoord);
            this.Controls.Add(this.edZ);
            this.Controls.Add(this.edY);
            this.Controls.Add(this.edX);
            this.Controls.Add(this.button5);
            this.Controls.Add(this.checkBox4);
            this.Controls.Add(this.button8);
            this.Controls.Add(this.checkBox3);
            this.Controls.Add(this.checkBox2);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.checkBox1);
            this.Controls.Add(this.button7);
            this.Controls.Add(this.button6);
            this.Controls.Add(this.button4);
            this.Controls.Add(this.button3);
            this.Controls.Add(this.textBox5);
            this.Controls.Add(this.textBox6);
            this.Controls.Add(this.button2);
            this.Controls.Add(this.textBox3);
            this.Controls.Add(this.textBox4);
            this.Controls.Add(this.button1);
            this.Controls.Add(this.textBox2);
            this.Controls.Add(this.textBox1);
            this.Controls.Add(this.glControl);
            this.Cursor = System.Windows.Forms.Cursors.Cross;
            this.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.ForeColor = System.Drawing.Color.Snow;
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedToolWindow;
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.Name = "FPrincipal";
            this.Text = "3D";
            this.Activated += new System.EventHandler(this.Desenho_Activated);
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.FPrincipal_FormClosing);
            this.FormClosed += new System.Windows.Forms.FormClosedEventHandler(this.Desenho_FormClosed);
            this.Load += new System.EventHandler(this.Desenho_Load);
            this.Shown += new System.EventHandler(this.Desenho_Shown);
            this.Click += new System.EventHandler(this.Desenho_Click);
            this.DoubleClick += new System.EventHandler(this.Desenho_DoubleClick);
            this.KeyDown += new System.Windows.Forms.KeyEventHandler(this.Desenho_KeyDown);
            this.KeyPress += new System.Windows.Forms.KeyPressEventHandler(this.Desenho_KeyPress);
            this.MouseDoubleClick += new System.Windows.Forms.MouseEventHandler(this.Desenho_MouseDoubleClick);
            this.MouseDown += new System.Windows.Forms.MouseEventHandler(this.Desenho_MouseDown);
            this.MouseEnter += new System.EventHandler(this.Desenho_MouseEnter);
            this.MouseUp += new System.Windows.Forms.MouseEventHandler(this.Desenho_MouseUp);
            this.MouseWheel += new System.Windows.Forms.MouseEventHandler(this.Desenho_MouseWheel);
            this.Resize += new System.EventHandler(this.Desenho_Resize);
            this.menuPrincipal.ResumeLayout(false);
            this.pnDivBarras.ResumeLayout(false);
            this.pnDivBarras.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.numericUpDown1)).EndInit();
            this.pnProgresso.ResumeLayout(false);
            this.pnRepeticoesCopia.ResumeLayout(false);
            this.pnRepeticoesCopia.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.edNumRepeticoes)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.numericUpDown2)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.numericUpDown3)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.numericUpDown4)).EndInit();
            this.pnEspelhar.ResumeLayout(false);
            this.pnEspelhar.PerformLayout();
            this.pnMover.ResumeLayout(false);
            this.pnMover.PerformLayout();
            this.pnRotacionar.ResumeLayout(false);
            this.pnRotacionar.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.qtdRepeticoesRotacao)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion
        private System.ComponentModel.BackgroundWorker backgroundWorker1;
        private System.Windows.Forms.ToolTip toolTip1;
        private System.Windows.Forms.ToolTip toolTip2;
        private System.Windows.Forms.PrintPreviewDialog printPreviewDialog1;
        private System.Windows.Forms.Timer timer1;
        private System.Drawing.Printing.PrintDocument printDocument2;
        private System.Windows.Forms.ContextMenuStrip menuPrincipal;
        private System.Windows.Forms.ToolStripMenuItem copiarToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem colarCtrlVToolStripMenuItem;
        private System.Windows.Forms.ToolStripSeparator toolStripMenuItem1;
        private System.Windows.Forms.ToolStripMenuItem procurarCtrlFToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem cancelarToolStripMenuItem;
        private System.Windows.Forms.TextBox textBox2;
        private System.Windows.Forms.Button button1;
        private System.Windows.Forms.Button button2;
        private System.Windows.Forms.TextBox textBox3;
        private System.Windows.Forms.TextBox textBox5;
        private System.Windows.Forms.TextBox textBox6;
        private System.Windows.Forms.Button button3;
        private System.Windows.Forms.Button button4;
        private System.Windows.Forms.Button button5;
        private System.Windows.Forms.Button button6;
        private System.Windows.Forms.Button button7;
        private System.Windows.Forms.CheckBox checkBox1;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.CheckBox checkBox3;
        private System.Windows.Forms.Button button8;
        private System.Windows.Forms.CheckBox checkBox4;
        public OpenTK.GLControl glControl;
        private System.Windows.Forms.TextBox edZ;
        private System.Windows.Forms.TextBox edY;
        private System.Windows.Forms.TextBox edX;
        private System.Windows.Forms.Button btConfirmaCoord;
        private System.Windows.Forms.Button btCoordRelativa;
        private System.Windows.Forms.ImageList imageList1;
        private System.Windows.Forms.Button btConfirmaCota;
        public System.Windows.Forms.Label lbUnidadeCota;
        private System.Windows.Forms.Button button10;
        public System.Windows.Forms.CheckBox checkBox2;
        private System.Windows.Forms.Timer timerSnap;
        private System.Windows.Forms.NumericUpDown numericUpDown1;
        public System.Windows.Forms.Panel pnDivBarras;
        public System.Windows.Forms.ProgressBar Progresso;
        public System.Windows.Forms.Panel pnProgresso;
        private System.Windows.Forms.Panel pnRepeticoesCopia;
        private System.Windows.Forms.Label lbNumRepeticoes;
        public System.Windows.Forms.NumericUpDown edNumRepeticoes;
        private System.Windows.Forms.NumericUpDown numericUpDown2;
        private System.Windows.Forms.NumericUpDown numericUpDown3;
        private System.Windows.Forms.NumericUpDown numericUpDown4;
        private System.Windows.Forms.ToolStripMenuItem mostrarSomenteToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem esconderSomenteToolStripMenuItem;
        private System.Windows.Forms.ToolStripSeparator toolStripMenuItem2;
        private System.Windows.Forms.ToolStripMenuItem mostrarTudoToolStripMenuItem;
        private System.Windows.Forms.ToolStripSeparator toolStripMenuItem4;
        private System.Windows.Forms.ToolStripMenuItem editarBarras;
        private System.Windows.Forms.ToolStripSeparator toolStripMenuItem3;
        private System.Windows.Forms.ToolStripMenuItem editarApoios;
        public System.Windows.Forms.TextBox edCota;
        private System.Windows.Forms.Label lbprogresso;
        private System.Windows.Forms.ToolStripMenuItem editarCargaPontual;
        private System.Windows.Forms.ToolStripMenuItem editarCargaDistribuida;
        private System.Windows.Forms.Timer timerZoom;
        private System.Windows.Forms.TextBox textBox7;
        private System.Windows.Forms.ContextMenuStrip contextMenuStrip2;
        private System.Windows.Forms.TextBox textBox8;
        private System.Windows.Forms.ToolStripMenuItem Elemento_rotacionar45;
        private System.Windows.Forms.ToolStripSeparator toolStripMenuItem5;
        public System.Windows.Forms.Label Comando;
        public System.Windows.Forms.TextBox textBox1;
        public System.Windows.Forms.TextBox textBox4;
        public System.Windows.Forms.CheckBox chCopiarCargas;
        private System.Windows.Forms.Panel pnEspelhar;
        public System.Windows.Forms.CheckBox chCopiarCargas_Espelhar;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.Panel pnMover;
        private System.Windows.Forms.Label label4;
        public System.Windows.Forms.CheckBox chMoverManterConexoes;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.Label label6;
        private System.Windows.Forms.Label label7;
        private System.Windows.Forms.Label label9;
        public System.Windows.Forms.TextBox edAnguloRotacao;
        public System.Windows.Forms.CheckBox chApagarOriginalRotacao;
        public System.Windows.Forms.CheckBox chCopiarCargasRotacao;
        private System.Windows.Forms.Label label8;
        public System.Windows.Forms.NumericUpDown qtdRepeticoesRotacao;
        public System.Windows.Forms.Panel pnRotacionar;
        private System.Windows.Forms.Button button9;
    }
}