namespace PG
{
    partial class FConfiguraDeformacao
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(FConfiguraDeformacao));
            this.panel1 = new System.Windows.Forms.Panel();
            this.button3 = new System.Windows.Forms.Button();
            this.imageList1 = new System.Windows.Forms.ImageList(this.components);
            this.button1 = new System.Windows.Forms.Button();
            this.groupBox7 = new System.Windows.Forms.GroupBox();
            this.label2 = new System.Windows.Forms.Label();
            this.label1 = new System.Windows.Forms.Label();
            this.rbCorDeslocamentoGradiente = new System.Windows.Forms.RadioButton();
            this.rbCorDeslocamentoNormal = new System.Windows.Forms.RadioButton();
            this.groupBox6 = new System.Windows.Forms.GroupBox();
            this.label5 = new System.Windows.Forms.Label();
            this.label4 = new System.Windows.Forms.Label();
            this.label3 = new System.Windows.Forms.Label();
            this.rbCorEsforcoDuasCores = new System.Windows.Forms.RadioButton();
            this.rbCorEsforcoGradiente = new System.Windows.Forms.RadioButton();
            this.rbCorEsforcoArestas = new System.Windows.Forms.RadioButton();
            this.groupBox5 = new System.Windows.Forms.GroupBox();
            this.chMostrarBarrasRigidas = new System.Windows.Forms.CheckBox();
            this.chMostrarBarrasPilar = new System.Windows.Forms.CheckBox();
            this.chMostrarBarrasViga = new System.Windows.Forms.CheckBox();
            this.chSuavizar = new System.Windows.Forms.CheckBox();
            this.chMostrarNos = new System.Windows.Forms.CheckBox();
            this.chMostrarCargasLineares = new System.Windows.Forms.CheckBox();
            this.chMostrarCargasPontuais = new System.Windows.Forms.CheckBox();
            this.chPerspectiva = new System.Windows.Forms.CheckBox();
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.rbExibirTudo = new System.Windows.Forms.RadioButton();
            this.rbExibirMaximo = new System.Windows.Forms.RadioButton();
            this.rbExibirPerc = new System.Windows.Forms.RadioButton();
            this.cbValores = new System.Windows.Forms.ComboBox();
            this.label9 = new System.Windows.Forms.Label();
            this.percentualValor = new System.Windows.Forms.TextBox();
            this.label8 = new System.Windows.Forms.Label();
            this.groupBox2 = new System.Windows.Forms.GroupBox();
            this.mostralinhasdecontorno = new System.Windows.Forms.CheckBox();
            this.label6 = new System.Windows.Forms.Label();
            this.btCorValor = new System.Windows.Forms.Button();
            this.colorDialog1 = new System.Windows.Forms.ColorDialog();
            this.panel1.SuspendLayout();
            this.groupBox7.SuspendLayout();
            this.groupBox6.SuspendLayout();
            this.groupBox5.SuspendLayout();
            this.groupBox1.SuspendLayout();
            this.groupBox2.SuspendLayout();
            this.SuspendLayout();
            // 
            // panel1
            // 
            this.panel1.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.panel1.Controls.Add(this.button3);
            this.panel1.Controls.Add(this.button1);
            this.panel1.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.panel1.Location = new System.Drawing.Point(0, 181);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(389, 35);
            this.panel1.TabIndex = 2;
            // 
            // button3
            // 
            this.button3.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.button3.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.button3.ImageIndex = 1;
            this.button3.ImageList = this.imageList1;
            this.button3.Location = new System.Drawing.Point(305, 4);
            this.button3.Name = "button3";
            this.button3.Size = new System.Drawing.Size(80, 25);
            this.button3.TabIndex = 18;
            this.button3.UseVisualStyleBackColor = true;
            this.button3.Click += new System.EventHandler(this.button3_Click);
            // 
            // imageList1
            // 
            this.imageList1.ImageStream = ((System.Windows.Forms.ImageListStreamer)(resources.GetObject("imageList1.ImageStream")));
            this.imageList1.TransparentColor = System.Drawing.Color.Transparent;
            this.imageList1.Images.SetKeyName(0, "confirmar.bmp");
            this.imageList1.Images.SetKeyName(1, "fechar.bmp");
            // 
            // button1
            // 
            this.button1.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.button1.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.button1.ImageIndex = 0;
            this.button1.ImageList = this.imageList1;
            this.button1.Location = new System.Drawing.Point(4, 4);
            this.button1.Name = "button1";
            this.button1.Size = new System.Drawing.Size(80, 25);
            this.button1.TabIndex = 17;
            this.button1.UseVisualStyleBackColor = true;
            this.button1.Click += new System.EventHandler(this.button1_Click);
            // 
            // groupBox7
            // 
            this.groupBox7.Controls.Add(this.label2);
            this.groupBox7.Controls.Add(this.label1);
            this.groupBox7.Controls.Add(this.rbCorDeslocamentoGradiente);
            this.groupBox7.Controls.Add(this.rbCorDeslocamentoNormal);
            this.groupBox7.Location = new System.Drawing.Point(569, 16);
            this.groupBox7.Name = "groupBox7";
            this.groupBox7.Size = new System.Drawing.Size(208, 58);
            this.groupBox7.TabIndex = 19;
            this.groupBox7.TabStop = false;
            this.groupBox7.Text = "Formato de cores para deslocamentos";
            this.groupBox7.Visible = false;
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(31, 40);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(53, 13);
            this.label2.TabIndex = 21;
            this.label2.Text = "Gradiente";
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(31, 21);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(47, 13);
            this.label1.TabIndex = 20;
            this.label1.Text = "Uma cor";
            // 
            // rbCorDeslocamentoGradiente
            // 
            this.rbCorDeslocamentoGradiente.AutoSize = true;
            this.rbCorDeslocamentoGradiente.Location = new System.Drawing.Point(10, 40);
            this.rbCorDeslocamentoGradiente.Name = "rbCorDeslocamentoGradiente";
            this.rbCorDeslocamentoGradiente.Size = new System.Drawing.Size(14, 13);
            this.rbCorDeslocamentoGradiente.TabIndex = 19;
            this.rbCorDeslocamentoGradiente.UseVisualStyleBackColor = true;
            // 
            // rbCorDeslocamentoNormal
            // 
            this.rbCorDeslocamentoNormal.AutoSize = true;
            this.rbCorDeslocamentoNormal.Checked = true;
            this.rbCorDeslocamentoNormal.Location = new System.Drawing.Point(10, 21);
            this.rbCorDeslocamentoNormal.Name = "rbCorDeslocamentoNormal";
            this.rbCorDeslocamentoNormal.Size = new System.Drawing.Size(14, 13);
            this.rbCorDeslocamentoNormal.TabIndex = 18;
            this.rbCorDeslocamentoNormal.TabStop = true;
            this.rbCorDeslocamentoNormal.UseVisualStyleBackColor = true;
            // 
            // groupBox6
            // 
            this.groupBox6.Controls.Add(this.label5);
            this.groupBox6.Controls.Add(this.label4);
            this.groupBox6.Controls.Add(this.label3);
            this.groupBox6.Controls.Add(this.rbCorEsforcoDuasCores);
            this.groupBox6.Controls.Add(this.rbCorEsforcoGradiente);
            this.groupBox6.Controls.Add(this.rbCorEsforcoArestas);
            this.groupBox6.Location = new System.Drawing.Point(392, 16);
            this.groupBox6.Name = "groupBox6";
            this.groupBox6.Size = new System.Drawing.Size(171, 74);
            this.groupBox6.TabIndex = 18;
            this.groupBox6.TabStop = false;
            this.groupBox6.Text = "Formato de cores para esforços";
            this.groupBox6.Visible = false;
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.Location = new System.Drawing.Point(29, 54);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(61, 13);
            this.label5.TabIndex = 23;
            this.label5.Text = "Duas cores";
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(29, 37);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(53, 13);
            this.label4.TabIndex = 22;
            this.label4.Text = "Gradiente";
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(29, 18);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(86, 13);
            this.label3.TabIndex = 21;
            this.label3.Text = "Somente arestas";
            // 
            // rbCorEsforcoDuasCores
            // 
            this.rbCorEsforcoDuasCores.AutoSize = true;
            this.rbCorEsforcoDuasCores.Location = new System.Drawing.Point(9, 54);
            this.rbCorEsforcoDuasCores.Name = "rbCorEsforcoDuasCores";
            this.rbCorEsforcoDuasCores.Size = new System.Drawing.Size(14, 13);
            this.rbCorEsforcoDuasCores.TabIndex = 20;
            this.rbCorEsforcoDuasCores.UseVisualStyleBackColor = true;
            // 
            // rbCorEsforcoGradiente
            // 
            this.rbCorEsforcoGradiente.AutoSize = true;
            this.rbCorEsforcoGradiente.Checked = true;
            this.rbCorEsforcoGradiente.Location = new System.Drawing.Point(9, 37);
            this.rbCorEsforcoGradiente.Name = "rbCorEsforcoGradiente";
            this.rbCorEsforcoGradiente.Size = new System.Drawing.Size(14, 13);
            this.rbCorEsforcoGradiente.TabIndex = 19;
            this.rbCorEsforcoGradiente.TabStop = true;
            this.rbCorEsforcoGradiente.UseVisualStyleBackColor = true;
            // 
            // rbCorEsforcoArestas
            // 
            this.rbCorEsforcoArestas.AutoSize = true;
            this.rbCorEsforcoArestas.Location = new System.Drawing.Point(9, 18);
            this.rbCorEsforcoArestas.Name = "rbCorEsforcoArestas";
            this.rbCorEsforcoArestas.Size = new System.Drawing.Size(14, 13);
            this.rbCorEsforcoArestas.TabIndex = 18;
            this.rbCorEsforcoArestas.UseVisualStyleBackColor = true;
            // 
            // groupBox5
            // 
            this.groupBox5.Controls.Add(this.chMostrarBarrasRigidas);
            this.groupBox5.Controls.Add(this.chMostrarBarrasPilar);
            this.groupBox5.Controls.Add(this.chMostrarBarrasViga);
            this.groupBox5.Location = new System.Drawing.Point(392, 96);
            this.groupBox5.Name = "groupBox5";
            this.groupBox5.Size = new System.Drawing.Size(219, 52);
            this.groupBox5.TabIndex = 17;
            this.groupBox5.TabStop = false;
            this.groupBox5.Text = "Mostrar diagramas em:";
            this.groupBox5.Visible = false;
            // 
            // chMostrarBarrasRigidas
            // 
            this.chMostrarBarrasRigidas.AutoSize = true;
            this.chMostrarBarrasRigidas.Location = new System.Drawing.Point(128, 24);
            this.chMostrarBarrasRigidas.Name = "chMostrarBarrasRigidas";
            this.chMostrarBarrasRigidas.Size = new System.Drawing.Size(91, 17);
            this.chMostrarBarrasRigidas.TabIndex = 10;
            this.chMostrarBarrasRigidas.Text = "Barras rígidas";
            this.chMostrarBarrasRigidas.UseVisualStyleBackColor = true;
            // 
            // chMostrarBarrasPilar
            // 
            this.chMostrarBarrasPilar.AutoSize = true;
            this.chMostrarBarrasPilar.Checked = true;
            this.chMostrarBarrasPilar.CheckState = System.Windows.Forms.CheckState.Checked;
            this.chMostrarBarrasPilar.Location = new System.Drawing.Point(69, 24);
            this.chMostrarBarrasPilar.Name = "chMostrarBarrasPilar";
            this.chMostrarBarrasPilar.Size = new System.Drawing.Size(57, 17);
            this.chMostrarBarrasPilar.TabIndex = 8;
            this.chMostrarBarrasPilar.Text = "Pilares";
            this.chMostrarBarrasPilar.UseVisualStyleBackColor = true;
            // 
            // chMostrarBarrasViga
            // 
            this.chMostrarBarrasViga.AutoSize = true;
            this.chMostrarBarrasViga.Checked = true;
            this.chMostrarBarrasViga.CheckState = System.Windows.Forms.CheckState.Checked;
            this.chMostrarBarrasViga.Location = new System.Drawing.Point(11, 24);
            this.chMostrarBarrasViga.Name = "chMostrarBarrasViga";
            this.chMostrarBarrasViga.Size = new System.Drawing.Size(52, 17);
            this.chMostrarBarrasViga.TabIndex = 9;
            this.chMostrarBarrasViga.Text = "Vigas";
            this.chMostrarBarrasViga.UseVisualStyleBackColor = true;
            // 
            // chSuavizar
            // 
            this.chSuavizar.AutoSize = true;
            this.chSuavizar.Checked = true;
            this.chSuavizar.CheckState = System.Windows.Forms.CheckState.Checked;
            this.chSuavizar.Location = new System.Drawing.Point(392, 154);
            this.chSuavizar.Name = "chSuavizar";
            this.chSuavizar.Size = new System.Drawing.Size(232, 17);
            this.chSuavizar.TabIndex = 21;
            this.chSuavizar.Text = "Habilitar suavização de arestas e polígonos";
            this.chSuavizar.UseVisualStyleBackColor = true;
            this.chSuavizar.Visible = false;
            // 
            // chMostrarNos
            // 
            this.chMostrarNos.AutoSize = true;
            this.chMostrarNos.Checked = true;
            this.chMostrarNos.CheckState = System.Windows.Forms.CheckState.Checked;
            this.chMostrarNos.Location = new System.Drawing.Point(392, 177);
            this.chMostrarNos.Name = "chMostrarNos";
            this.chMostrarNos.Size = new System.Drawing.Size(83, 17);
            this.chMostrarNos.TabIndex = 22;
            this.chMostrarNos.Text = "Mostrar Nós";
            this.chMostrarNos.UseVisualStyleBackColor = true;
            this.chMostrarNos.Visible = false;
            // 
            // chMostrarCargasLineares
            // 
            this.chMostrarCargasLineares.AutoSize = true;
            this.chMostrarCargasLineares.Checked = true;
            this.chMostrarCargasLineares.CheckState = System.Windows.Forms.CheckState.Checked;
            this.chMostrarCargasLineares.Location = new System.Drawing.Point(391, 200);
            this.chMostrarCargasLineares.Name = "chMostrarCargasLineares";
            this.chMostrarCargasLineares.Size = new System.Drawing.Size(140, 17);
            this.chMostrarCargasLineares.TabIndex = 23;
            this.chMostrarCargasLineares.Text = "Mostrar Cargas Lineares";
            this.chMostrarCargasLineares.UseVisualStyleBackColor = true;
            this.chMostrarCargasLineares.Visible = false;
            // 
            // chMostrarCargasPontuais
            // 
            this.chMostrarCargasPontuais.AutoSize = true;
            this.chMostrarCargasPontuais.Checked = true;
            this.chMostrarCargasPontuais.CheckState = System.Windows.Forms.CheckState.Checked;
            this.chMostrarCargasPontuais.Location = new System.Drawing.Point(377, 139);
            this.chMostrarCargasPontuais.Name = "chMostrarCargasPontuais";
            this.chMostrarCargasPontuais.Size = new System.Drawing.Size(141, 17);
            this.chMostrarCargasPontuais.TabIndex = 24;
            this.chMostrarCargasPontuais.Text = "Mostrar Cargas Pontuais";
            this.chMostrarCargasPontuais.UseVisualStyleBackColor = true;
            this.chMostrarCargasPontuais.Visible = false;
            // 
            // chPerspectiva
            // 
            this.chPerspectiva.AutoSize = true;
            this.chPerspectiva.Checked = true;
            this.chPerspectiva.CheckState = System.Windows.Forms.CheckState.Checked;
            this.chPerspectiva.Location = new System.Drawing.Point(683, 154);
            this.chPerspectiva.Name = "chPerspectiva";
            this.chPerspectiva.Size = new System.Drawing.Size(82, 17);
            this.chPerspectiva.TabIndex = 25;
            this.chPerspectiva.Text = "Perspectiva";
            this.chPerspectiva.UseVisualStyleBackColor = true;
            this.chPerspectiva.Visible = false;
            // 
            // groupBox1
            // 
            this.groupBox1.Controls.Add(this.rbExibirTudo);
            this.groupBox1.Controls.Add(this.rbExibirMaximo);
            this.groupBox1.Controls.Add(this.rbExibirPerc);
            this.groupBox1.Controls.Add(this.cbValores);
            this.groupBox1.Controls.Add(this.label9);
            this.groupBox1.Controls.Add(this.percentualValor);
            this.groupBox1.Controls.Add(this.label8);
            this.groupBox1.Location = new System.Drawing.Point(11, 5);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(375, 57);
            this.groupBox1.TabIndex = 28;
            this.groupBox1.TabStop = false;
            this.groupBox1.Text = "Valores";
            this.groupBox1.Enter += new System.EventHandler(this.groupBox1_Enter);
            // 
            // rbExibirTudo
            // 
            this.rbExibirTudo.AutoSize = true;
            this.rbExibirTudo.Location = new System.Drawing.Point(34, 110);
            this.rbExibirTudo.Name = "rbExibirTudo";
            this.rbExibirTudo.Size = new System.Drawing.Size(74, 17);
            this.rbExibirTudo.TabIndex = 37;
            this.rbExibirTudo.TabStop = true;
            this.rbExibirTudo.Text = "Exibir tudo";
            this.rbExibirTudo.UseVisualStyleBackColor = true;
            this.rbExibirTudo.Visible = false;
            // 
            // rbExibirMaximo
            // 
            this.rbExibirMaximo.AutoSize = true;
            this.rbExibirMaximo.Location = new System.Drawing.Point(34, 87);
            this.rbExibirMaximo.Name = "rbExibirMaximo";
            this.rbExibirMaximo.Size = new System.Drawing.Size(140, 17);
            this.rbExibirMaximo.TabIndex = 36;
            this.rbExibirMaximo.TabStop = true;
            this.rbExibirMaximo.Text = "Exibir somente o máximo";
            this.rbExibirMaximo.UseVisualStyleBackColor = true;
            this.rbExibirMaximo.Visible = false;
            this.rbExibirMaximo.CheckedChanged += new System.EventHandler(this.rbExibirMaximo_CheckedChanged);
            // 
            // rbExibirPerc
            // 
            this.rbExibirPerc.AutoSize = true;
            this.rbExibirPerc.Location = new System.Drawing.Point(34, 64);
            this.rbExibirPerc.Name = "rbExibirPerc";
            this.rbExibirPerc.Size = new System.Drawing.Size(162, 17);
            this.rbExibirPerc.TabIndex = 35;
            this.rbExibirPerc.TabStop = true;
            this.rbExibirPerc.Text = "Exibir valores maiores do que";
            this.rbExibirPerc.UseVisualStyleBackColor = true;
            this.rbExibirPerc.Visible = false;
            this.rbExibirPerc.CheckedChanged += new System.EventHandler(this.rbExibirPerc_CheckedChanged);
            // 
            // cbValores
            // 
            this.cbValores.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cbValores.FormattingEnabled = true;
            this.cbValores.Items.AddRange(new object[] {
            "Dx global",
            "Dy global",
            "Dz global",
            "Rx global",
            "Ry global",
            "Rz global",
            "D Total"});
            this.cbValores.Location = new System.Drawing.Point(51, 20);
            this.cbValores.Name = "cbValores";
            this.cbValores.Size = new System.Drawing.Size(84, 21);
            this.cbValores.TabIndex = 34;
            // 
            // label9
            // 
            this.label9.AutoSize = true;
            this.label9.Location = new System.Drawing.Point(3, 28);
            this.label9.Name = "label9";
            this.label9.Size = new System.Drawing.Size(42, 13);
            this.label9.TabIndex = 33;
            this.label9.Text = "Valores";
            // 
            // percentualValor
            // 
            this.percentualValor.Location = new System.Drawing.Point(198, 58);
            this.percentualValor.Name = "percentualValor";
            this.percentualValor.Size = new System.Drawing.Size(49, 20);
            this.percentualValor.TabIndex = 32;
            this.percentualValor.Text = "0";
            this.percentualValor.Visible = false;
            this.percentualValor.KeyPress += new System.Windows.Forms.KeyPressEventHandler(this.Valor_KeyPress);
            // 
            // label8
            // 
            this.label8.AutoSize = true;
            this.label8.Location = new System.Drawing.Point(250, 68);
            this.label8.Name = "label8";
            this.label8.Size = new System.Drawing.Size(117, 13);
            this.label8.TabIndex = 31;
            this.label8.Text = "%  do resultado máximo";
            this.label8.Visible = false;
            // 
            // groupBox2
            // 
            this.groupBox2.Controls.Add(this.mostralinhasdecontorno);
            this.groupBox2.Location = new System.Drawing.Point(11, 73);
            this.groupBox2.Name = "groupBox2";
            this.groupBox2.Size = new System.Drawing.Size(375, 50);
            this.groupBox2.TabIndex = 29;
            this.groupBox2.TabStop = false;
            this.groupBox2.Text = "Opções de visualização renderizada";
            // 
            // mostralinhasdecontorno
            // 
            this.mostralinhasdecontorno.AutoSize = true;
            this.mostralinhasdecontorno.Location = new System.Drawing.Point(9, 22);
            this.mostralinhasdecontorno.Name = "mostralinhasdecontorno";
            this.mostralinhasdecontorno.Size = new System.Drawing.Size(126, 17);
            this.mostralinhasdecontorno.TabIndex = 24;
            this.mostralinhasdecontorno.Text = "Mostrar discretização";
            this.mostralinhasdecontorno.UseVisualStyleBackColor = true;
            // 
            // label6
            // 
            this.label6.AutoSize = true;
            this.label6.Location = new System.Drawing.Point(14, 139);
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size(80, 13);
            this.label6.TabIndex = 95;
            this.label6.Text = "Cor dos valores";
            // 
            // btCorValor
            // 
            this.btCorValor.BackColor = System.Drawing.Color.Maroon;
            this.btCorValor.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btCorValor.Location = new System.Drawing.Point(94, 133);
            this.btCorValor.Name = "btCorValor";
            this.btCorValor.Size = new System.Drawing.Size(19, 19);
            this.btCorValor.TabIndex = 94;
            this.btCorValor.UseVisualStyleBackColor = false;
            this.btCorValor.Click += new System.EventHandler(this.btCorValor_Click);
            // 
            // FConfiguraDeformacao
            // 
            this.AcceptButton = this.button1;
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.CancelButton = this.button3;
            this.ClientSize = new System.Drawing.Size(389, 216);
            this.Controls.Add(this.label6);
            this.Controls.Add(this.btCorValor);
            this.Controls.Add(this.groupBox2);
            this.Controls.Add(this.groupBox1);
            this.Controls.Add(this.chPerspectiva);
            this.Controls.Add(this.chMostrarCargasPontuais);
            this.Controls.Add(this.chMostrarCargasLineares);
            this.Controls.Add(this.chMostrarNos);
            this.Controls.Add(this.chSuavizar);
            this.Controls.Add(this.groupBox7);
            this.Controls.Add(this.groupBox6);
            this.Controls.Add(this.groupBox5);
            this.Controls.Add(this.panel1);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedToolWindow;
            this.Name = "FConfiguraDeformacao";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "Configurações de visualização dos deslocamentos";
            this.Load += new System.EventHandler(this.FGrelhaOpcoesVisualizacao_Load);
            this.panel1.ResumeLayout(false);
            this.groupBox7.ResumeLayout(false);
            this.groupBox7.PerformLayout();
            this.groupBox6.ResumeLayout(false);
            this.groupBox6.PerformLayout();
            this.groupBox5.ResumeLayout(false);
            this.groupBox5.PerformLayout();
            this.groupBox1.ResumeLayout(false);
            this.groupBox1.PerformLayout();
            this.groupBox2.ResumeLayout(false);
            this.groupBox2.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Panel panel1;
        private System.Windows.Forms.Button button3;
        private System.Windows.Forms.Button button1;
        private System.Windows.Forms.GroupBox groupBox7;
        private System.Windows.Forms.RadioButton rbCorDeslocamentoGradiente;
        private System.Windows.Forms.GroupBox groupBox6;
        private System.Windows.Forms.RadioButton rbCorEsforcoDuasCores;
        private System.Windows.Forms.RadioButton rbCorEsforcoGradiente;
        private System.Windows.Forms.RadioButton rbCorEsforcoArestas;
        private System.Windows.Forms.GroupBox groupBox5;
        private System.Windows.Forms.CheckBox chMostrarBarrasPilar;
        private System.Windows.Forms.CheckBox chMostrarBarrasViga;
        private System.Windows.Forms.RadioButton rbCorDeslocamentoNormal;
        private System.Windows.Forms.CheckBox chSuavizar;
        private System.Windows.Forms.CheckBox chMostrarBarrasRigidas;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.CheckBox chMostrarNos;
        private System.Windows.Forms.CheckBox chMostrarCargasLineares;
        private System.Windows.Forms.CheckBox chMostrarCargasPontuais;
        private System.Windows.Forms.CheckBox chPerspectiva;
        private System.Windows.Forms.GroupBox groupBox1;
        private System.Windows.Forms.Label label8;
        public System.Windows.Forms.TextBox percentualValor;
        private System.Windows.Forms.GroupBox groupBox2;
        private System.Windows.Forms.CheckBox mostralinhasdecontorno;
        private System.Windows.Forms.ComboBox cbValores;
        private System.Windows.Forms.Label label9;
        public System.Windows.Forms.ImageList imageList1;
        private System.Windows.Forms.RadioButton rbExibirMaximo;
        private System.Windows.Forms.RadioButton rbExibirPerc;
        private System.Windows.Forms.RadioButton rbExibirTudo;
        private System.Windows.Forms.Label label6;
        private System.Windows.Forms.Button btCorValor;
        private System.Windows.Forms.ColorDialog colorDialog1;
    }
}