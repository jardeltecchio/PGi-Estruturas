using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Windows.Ink;
using System.Windows.Media;
using System.Windows.Shapes;
using Win32Interop.Enums;
using Win32Interop.Structs;
using static System.Windows.Forms.VisualStyles.VisualStyleElement.TextBox;

namespace PG
{
    public partial class FEscolheSecao : Form
    {
        FDadosBarra dadosBarra;
        Gerenciador gerenciador;
        public FEscolheSecao()
        {
            InitializeComponent();
        }       
        List<TPropriedades_Perfil_W_Gerdau> Dados_Perfil_W_Gerdau;
        List<TPropriedades_Perfil_i_Gerdau> Dados_Perfil_i_Gerdau;
        List<TPropriedades_Perfil_u_Gerdau> Dados_Perfil_u_Gerdau;
        List<TPropriedades_Perfil_t_Gerdau> Dados_Perfil_t_Gerdau;

        List<TPropriedades_Cantoneiras_Gerdau> Dados_Cantoneira_Gerdau;

        TPropriedades_Perfil_Retangulo Dados_Perfil_Retangulo;

        List<string> tipos_secao;

        public int id = -1;
        public void Carrega(string _tipo)
        {
            alterando = true;
          /*  tipo = _tipo;
            if (_tipo == Const.SECAO_W_I_LAMINADO)
            {
                cbSecoesW.SelectedIndex = secao.idSecaoLaminada;          
            }else
            if (tipo == Const.SECAO_SOLIDO_RET)
            {
                edb1.Text = (secao.b1 * 100).ToString("n2");
                edh1.Text = (secao.h1 * 100).ToString("n2");
            }else
            if (tipo == Const.SECAO_SOLIDO_CIRC)
            {
                edD.Text = (secao.d1 * 100).ToString("n2");
            }

            for (int k = 0; k < tbForm.TabPages.Count; k++)
            {
                for (int i = 0; i < tbForm.TabPages[k].Controls.Count; i++)
                {
                    if (tbForm.TabPages[k].Controls[i] is TabControl)
                    {
                        TabControl tc = (TabControl)(tbForm.TabPages[k].Controls[i]);

                        for (int j = 0; j < tc.TabPages.Count; j++)
                        {
                            if (tc.TabPages[j].AccessibleName == _tipo)
                            {
                                tbForm.SelectedIndex = k;
                                tc.SelectedIndex = j;
                            }
                        }
                    }
                }
            }

            if (cbMaterial.Items.Count == 0)
                return;
            if (secao == null) 
                return;

            btCor.BackColor             = System.Drawing.Color.FromArgb(secao.Rgb[0], secao.Rgb[1], secao.Rgb[2]);
            cbMaterial.SelectedIndex    = secao.idMaterial;
            NomeSecao.Text              = secao.descricao;
            chPropriedadeManual.Checked = secao.PropriedadesManuais;
            chEditarProp_CheckedChanged(chPropriedadeManual,null);

            edA.Text = (secao.area*(10000)).ToString("n4");
            edIx.Text = (secao.inercia_flexao_z * (100000000)).ToString("n4");
            edIy.Text = (secao.inercia_flexao_y * (100000000)).ToString("n4");
            edIt.Text = (secao.inercia_torcao * (100000000)).ToString("n4");
            edb1_Validated(edA, null);
            edb1_Validated(edIx, null);
            edb1_Validated(edIy, null);
            edb1_Validated(edIt, null);*/

            this.Text = "Alterar seção transversal"; 

        }
        string textoAtual;
        void GetPropriedade(ref string line, ref int soma, ref string item, ref double prop)
        {
            textoAtual = line.Substring(soma + 1, line.Length - soma - 1);
            if (separador_decimal_windows == ".")
              item = textoAtual.Substring(0, textoAtual.IndexOf(" ")).Replace(",", ".");
            else
              item = textoAtual.Substring(0, textoAtual.IndexOf(" "));
           // item = Convert.ToString(item, System.Globalization.CultureInfo.InvariantCulture);
            //item = item.ToString(System.Globalization.CultureInfo.InvariantCulture);
            prop = System.Convert.ToDouble(item);
            soma += item.Length + 1;
        }
        string separador_decimal_windows;

        public FEscolheSecao(FDadosBarra dados, Gerenciador ow)
        {
            InitializeComponent();
            dadosBarra  = dados;
            gerenciador = ow;
            ShowInTaskbar = false;
            //  Carrega_W_Laminados();
        }
        private void btSalvar_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void button3_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        CoordenadaD[] coordsPoligono;
        public TPoligono poligono;
        public TSecao secao;
        string tipo;
        TGeraCoodenadasLaminado CoordenadaSecao_Laminado;

        public bool alterando;

        private void FSecaoLaminada_FormClosed(object sender, FormClosedEventArgs e)
        {
            dadosBarra.SecaoLaminada = null;
            dadosBarra.Enquadrar();

          //  gerenciador.formDesenho.AtualizaShaders(true);
          //  gerenciador.formDesenho.glControl.MakeCurrent();
        }

        private void btCor_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Escape)
                this.Close();
        }

        private void FSecaoLaminada_Load(object sender, EventArgs e)
        {
          
            //     SetaTipoSecao();

            /*  for (int k = 0; k < tbForm.TabPages.Count; k++)
                  paginas.Add(tbForm.TabPages[k]);

               for (int k = 0; k < tbForm.TabPages.Count; k++)
               {
                   for (int i = 0; i < tbForm.TabPages[k].Controls.Count; i++)
                   {
                       if (tbForm.TabPages[k].Controls[i] is Button)
                       {
                           ((Button)(tbForm.TabPages[k].Controls[i])).AccessibleName = tbForm.TabPages[k].Text;
                       }
                   }
               }*/
            lbSecoes.Items.Clear();
            for (int i = 0; i < dadosBarra.cbSecoes.Items.Count; i++)
                lbSecoes.Items.Add(dadosBarra.cbSecoes.Items[i]);

            for (int i = 0; i < tbForm.TabPages.Count; i++)
            {
                for (int j = 0; j < tbForm.TabPages[i].Controls.Count; j++)
                {
                    if (tbForm.TabPages[i].Text != "Biblioteca")
                    {
                        if (tbForm.TabPages[i].Controls[j] is Button)
                        {
                            Button tc = (Button)(tbForm.TabPages[i].Controls[j]);
                            //   MessageBox.Show(tc.Name.ToString());
                            ((Button)(tbForm.TabPages[i].Controls[j])).MouseEnter += new EventHandler(btConcRetangulo_MouseEnter);
                            ((Button)(tbForm.TabPages[i].Controls[j])).Click += new EventHandler(ClickBotoes);

                            //forçando uma passsagem de mouse em cima de todos os botoes, pois senao parece que demora pra aparecer o tooltipo na primeira vez que o usuario pasar o mouse
                            btConcRetangulo_MouseEnter(((Button)(tbForm.TabPages[i].Controls[j])), null);
                        }
                    }
                    else
                    {
                        if (tbForm.TabPages[i].Controls[j] is Button)
                        {
                            Button tc = (Button)(tbForm.TabPages[i].Controls[j]);

                            ((Button)(tbForm.TabPages[i].Controls[j])).MouseEnter += new EventHandler(btConcRetangulo_MouseEnter);
                            //forçando uma passsagem de mouse em cima de todos os botoes, pois senao parece que demora pra aparecer o tooltipo na primeira vez que o usuario pasar o mouse
                            btConcRetangulo_MouseEnter(((Button)(tbForm.TabPages[i].Controls[j])), null);
                        }
                    }

                }
            }

            ReposicionaTabPagesConformeTreeView();

            while (tbForm.TabCount > 0)
              tbForm.TabPages.RemoveAt(0);
        }

        public FCalculaSecao calculasecao;
        public bool testasecao;

        List<TabPage> paginas = new List<TabPage>();

        void MostraPaginaSecao(Button botao)
        {
            TabPage p = null;

            for (int k = 0; k < paginas.Count; k++)
                if (paginas[k].Text == botao.Text)
                    p = paginas[k];

            tbForm.TabPages.Clear();

            tbForm.TabPages.Add(p);
        }
        private void NomeSecao_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Escape)
                this.Close();
        }

        private void edb1_KeyPress(object sender, KeyPressEventArgs e)
        {
            char a = Convert.ToChar(System.Threading.Thread.CurrentThread.CurrentCulture.NumberFormat.NumberDecimalSeparator);
            if (!char.IsControl(e.KeyChar) && !char.IsDigit(e.KeyChar) && (e.KeyChar != a) && (e.KeyChar != '-'))
            {
                e.Handled = true;
            }

            if (e.KeyChar == (char)Keys.Enter || e.KeyChar == (char)Keys.Escape)
            {
                e.Handled = true;   // stop annoying beep
            }
            base.OnKeyPress(e);
        }
        private void edb1_KeyPress_1(object sender, KeyPressEventArgs e)
        {
            char a = Convert.ToChar(System.Threading.Thread.CurrentThread.CurrentCulture.NumberFormat.NumberDecimalSeparator);
            if (!char.IsControl(e.KeyChar) && !char.IsDigit(e.KeyChar) && (e.KeyChar != a) && (e.KeyChar != '-'))
            {
                e.Handled = true;
            }

            if (e.KeyChar == (char)Keys.Enter || e.KeyChar == (char)Keys.Escape)
            {
                e.Handled = true;   // stop annoying beep
            }
            base.OnKeyPress(e);
        }

        private void tbForm_SelectedIndexChanged_1(object sender, EventArgs e)
        {
            /* tipo = tbSecoesConcreto.SelectedTab.AccessibleName;

             if (tipo == Const.SECAO_SOLIDO_CIRC)
                 edD_Validated(edD, null);
             if (tipo == Const.SECAO_SOLIDO_RET)
                 edb1_Validated(edb1, null);*/

            if (tbForm.SelectedTab == tbBiblioteca)
            {
                lbTiposLaminado.SelectedIndex = 0;
               // lbFabricantesLaminado_SelectedIndexChanged(lbTiposLaminado, null);

          //      tbSecoesBiblioteca.SelectedIndex = 0;
          //      tbSecoesBiblioteca_SelectedIndexChanged(tbSecoesBiblioteca, null);
            }
            if (tbForm.SelectedTab == tbDobrado)
            {

            }
            if (tbForm.SelectedTab == tbConcreto)
            {

            }
        }

        void ReposicionaTabPagesConformeTreeView()
        {
            List<TabPage> p = new List<TabPage>();

            for (int i = 0; i < tvSecoes.Nodes.Count; i++)
            {
                if (tvSecoes.Nodes[i].Nodes.Count == 0)
                {
                    for (int j = 0; j < tbForm.TabPages.Count; j++)
                        if (tbForm.TabPages[j].Text == tvSecoes.Nodes[i].Text)
                            p.Add(tbForm.TabPages[j]);
                }
                else
                {
                    for (int j = 0; j < tvSecoes.Nodes[i].Nodes.Count; j++)
                    {
                        for (int k = 0; k < tbForm.TabPages.Count; k++)
                            if (tbForm.TabPages[k].Text == tvSecoes.Nodes[i].Nodes[j].Text)
                                p.Add(tbForm.TabPages[k]);
                    }
                }
            }

            for (int i = 0; i < p.Count; i++)
                paginas.Add(p[i]);
        }

        private void tvSecoes_AfterSelect(object sender, TreeViewEventArgs e)
        {
            
            try
            {
                if (tvSecoes.SelectedNode.ToolTipText != string.Empty)
                {
                    for (int i = 0; i < paginas.Count; i++)
                        tbForm.TabPages.Remove(paginas[i]);

                    tbForm.TabPages.Add(paginas[(int)tvSecoes.SelectedNode.Tag]);

                    if (tbForm.TabPages[0].Controls[0] is Button)
                        tbForm.TabPages[0].Controls[0].Focus();

                    if (tbForm.TabPages[0].Name == "tbBiblioteca")
                    {
                        btLaminadoW_Click(btLaminadoW, null);
                    }
                    //  tbForm.SelectedIndex = tv.SelectedNode.Index;     
                }
            }

            catch (Exception ss)
            {
                MessageBox.Show(ss.Message);
            }
        }

        private void FSecaoLaminada_Shown(object sender, EventArgs e)
        {
            int cc = -1;
            for (int i = 0; i < tvSecoes.Nodes.Count; i++)
            {
               cc++;
               tvSecoes.Nodes[i].Tag = cc;
            }

         /*   for (int i = 0; i < tbForm.TabPages.Count; i++)
            {
                for (int j = 0; j < tbForm.TabPages[i].Controls.Count; j++)
                {
                    if (tbForm.TabPages[i].Text != "Biblioteca")
                    {
                        if (tbForm.TabPages[i].Controls[j] is Button)
                        {
                            Button tc = (Button)(tbForm.TabPages[i].Controls[j]);
                            //   MessageBox.Show(tc.Name.ToString());
                            ((Button)(tbForm.TabPages[i].Controls[j])).MouseEnter += new EventHandler(btConcRetangulo_MouseEnter);
                            ((Button)(tbForm.TabPages[i].Controls[j])).Click += new EventHandler(ClickBotoes);

                            //forçando uma passsagem de mouse em cima de todos os botoes, pois senao parece que demora pra aparecer o tooltipo na primeira vez que o usuario pasar o mouse
                            btConcRetangulo_MouseEnter(((Button)(tbForm.TabPages[i].Controls[j])), null);
                        }
                    }
                    else
                    {
                        if (tbForm.TabPages[i].Controls[j] is Button)
                        {
                            Button tc = (Button)(tbForm.TabPages[i].Controls[j]);
       
                            ((Button)(tbForm.TabPages[i].Controls[j])).MouseEnter += new EventHandler(btConcRetangulo_MouseEnter);
                            //forçando uma passsagem de mouse em cima de todos os botoes, pois senao parece que demora pra aparecer o tooltipo na primeira vez que o usuario pasar o mouse
                            btConcRetangulo_MouseEnter(((Button)(tbForm.TabPages[i].Controls[j])), null);
                        }
                    }

                }
            }*/

         //   ReposicionaTabPagesConformeTreeView();

            tvSecoes.SelectedNode = tvSecoes.Nodes[0];

            tvSecoes_AfterSelect(tvSecoes, null);
            tvSecoes.SelectedNode = tvSecoes.Nodes[0];
            tvSecoes.Focus();
        }

        
        ToolTip tipBotoes = new ToolTip();

        private void btConcRetangulo_MouseEnter(object sender, EventArgs e)
        {
            Button btn = (Button)sender;
            
            tipBotoes.SetToolTip(btn, btn.AccessibleName);
        }
        void Carrega_W_Laminados()
        {
            lbCatalogoLaminados.Items.Clear();
            lbCatalogoLaminados.AccessibleDescription = btLaminadoW.AccessibleDescription;

            separador_decimal_windows = Convert.ToString(System.Threading.Thread.CurrentThread.CurrentCulture.NumberFormat.NumberDecimalSeparator);

            Dados_Perfil_W_Gerdau = new List<TPropriedades_Perfil_W_Gerdau>();
            TPropriedades_Perfil_W_Gerdau dp;

            string arq = System.IO.Path.GetDirectoryName(Application.ExecutablePath) + "\\perfil_w_laminados.txt";
            List<string> linhas = new List<string>();
            string lin;
            linhas = System.IO.File.ReadLines(arq).ToList();
            if (separador_decimal_windows == ".")
            {
                for (int i = 0; i < linhas.Count(); i++)
                {
                    //lin = "1.088,77";//linhas[i];
                    lin = linhas[i];
                    linhas[i] = lin.Replace(".", "");
                    linhas[i] = linhas[i].Replace(",", ".");
                }
            }

            int soma, cc = 0;

            for (int i = 0; i < 88; i++)
            {
                dp = new TPropriedades_Perfil_W_Gerdau();
                cc = (88 + i);
                soma = 0;
                //  cbSecoesW.Items.Add(linhas[i]);
                lbCatalogoLaminados.Items.Add(linhas[i]);
                dp.nome = linhas[i];
                dp.MASSA = System.Convert.ToDouble(linhas[cc]);
                dp.D = System.Convert.ToDouble(linhas[cc + (88 * (++soma))]);
                dp.BF = System.Convert.ToDouble(linhas[cc + (88 * (++soma))]);
                dp.TW = System.Convert.ToDouble(linhas[cc + (88 * (++soma))]);
                dp.TF = System.Convert.ToDouble(linhas[cc + (88 * (++soma))]);
                dp.H = System.Convert.ToDouble(linhas[cc + (88 * (++soma))]);
                dp.dlinha = System.Convert.ToDouble(linhas[cc + (88 * (++soma))]);
                dp.AREA = System.Convert.ToDouble(linhas[cc + (88 * (++soma))]);

                dp.IX = System.Convert.ToDouble(linhas[cc + (88 * (++soma))]);
                dp.WX = System.Convert.ToDouble(linhas[cc + (88 * (++soma))]);
                dp.RX = System.Convert.ToDouble(linhas[cc + (88 * (++soma))]);
                dp.ZX = System.Convert.ToDouble(linhas[cc + (88 * (++soma))]);

                dp.IY = System.Convert.ToDouble(linhas[cc + (88 * (++soma))]);
                dp.WY = System.Convert.ToDouble(linhas[cc + (88 * (++soma))]);
                dp.RY = System.Convert.ToDouble(linhas[cc + (88 * (++soma))]);
                dp.ZY = System.Convert.ToDouble(linhas[cc + (88 * (++soma))]);

                dp.RT = System.Convert.ToDouble(linhas[cc + (88 * (++soma))]);
                dp.IT = System.Convert.ToDouble(linhas[cc + (88 * (++soma))]);

                dp.esbeltezmesa = System.Convert.ToDouble(linhas[cc + (88 * (++soma))]);
                dp.esbeltezalma = System.Convert.ToDouble(linhas[cc + (88 * (++soma))]);
                dp.CW = System.Convert.ToDouble(linhas[cc + (88 * (++soma))]);

                Dados_Perfil_W_Gerdau.Add(dp);
            }
        }
        void Carrega_Cantoneiras_Laminadas()
        {
            lbCatalogoLaminados.Items.Clear();
            lbCatalogoLaminados.AccessibleDescription = btLaminadoCantoneira.AccessibleDescription;

            separador_decimal_windows = Convert.ToString(System.Threading.Thread.CurrentThread.CurrentCulture.NumberFormat.NumberDecimalSeparator);

            Dados_Cantoneira_Gerdau = new List<TPropriedades_Cantoneiras_Gerdau>();
            TPropriedades_Cantoneiras_Gerdau dp;

            string arq = System.IO.Path.GetDirectoryName(Application.ExecutablePath) + "\\cantoneiras_laminadas.txt";
            List<string> linhas = new List<string>();
            string lin;
            linhas = System.IO.File.ReadLines(arq).ToList();
            if (separador_decimal_windows == ".")
            {
                for (int i = 0; i < linhas.Count(); i++)
                {
                    //lin = "1.088,77";//linhas[i];
                    lin = linhas[i];
                    linhas[i] = lin.Replace(".", "");
                    linhas[i] = linhas[i].Replace(",", ".");
                }
            }
            int tot_perfis = 52;
            int soma, cc = 0;

            for (int i = 0; i < tot_perfis; i++)
            {
                dp = new TPropriedades_Cantoneiras_Gerdau();
                cc = (tot_perfis + i);
                soma = 0;

                lbCatalogoLaminados.Items.Add(linhas[i]);
                dp.nome = linhas[i];                            
                dp.b = System.Convert.ToDouble(linhas[cc]);

                int lll = cc + (tot_perfis * (++soma));
                dp.t = System.Convert.ToDouble(linhas[lll]);

                Dados_Cantoneira_Gerdau.Add(dp);
            }

            lbCatalogoLaminados.SelectedIndex = 0;
        }
        void Carrega_u_Laminados()
        {
            lbCatalogoLaminados.Items.Clear();
            lbCatalogoLaminados.AccessibleDescription = btLaminadoU.AccessibleDescription;

            separador_decimal_windows = Convert.ToString(System.Threading.Thread.CurrentThread.CurrentCulture.NumberFormat.NumberDecimalSeparator);

            Dados_Perfil_u_Gerdau = new List<TPropriedades_Perfil_u_Gerdau>();
            TPropriedades_Perfil_u_Gerdau dp;

            string arq = System.IO.Path.GetDirectoryName(Application.ExecutablePath) + "\\perfil_u_laminados.txt";
            List<string> linhas = new List<string>();
            string lin;
            linhas = System.IO.File.ReadLines(arq).ToList();
            if (separador_decimal_windows == ".")
            {
                for (int i = 0; i < linhas.Count(); i++)
                {
                    //lin = "1.088,77";//linhas[i];
                    lin = linhas[i];
                    linhas[i] = lin.Replace(".", "");
                    linhas[i] = linhas[i].Replace(",", ".");
                }
            }
            int tot_perfis = 12;
            int soma, cc = 0;
            //massa, d, tw, bf, tf, area, Ix, Wx, rx, Iy, Wy, ry, x;
            for (int i = 0; i < tot_perfis; i++)
            {
                dp = new TPropriedades_Perfil_u_Gerdau();
                cc = (tot_perfis + i);
                soma = 0;

                lbCatalogoLaminados.Items.Add(linhas[i]);
                dp.nome = linhas[i];
                dp.massa = System.Convert.ToDouble(linhas[cc]);
                dp.d = System.Convert.ToDouble(linhas[cc + (tot_perfis * (++soma))]);
                dp.tw = System.Convert.ToDouble(linhas[cc + (tot_perfis * (++soma))]);
                dp.bf = System.Convert.ToDouble(linhas[cc + (tot_perfis * (++soma))]);
                dp.tf = System.Convert.ToDouble(linhas[cc + (tot_perfis * (++soma))]);
                dp.area = System.Convert.ToDouble(linhas[cc + (tot_perfis * (++soma))]);
                dp.Ix = System.Convert.ToDouble(linhas[cc + (tot_perfis * (++soma))]);
                dp.Wx = System.Convert.ToDouble(linhas[cc + (tot_perfis * (++soma))]);
                dp.rx = System.Convert.ToDouble(linhas[cc + (tot_perfis * (++soma))]);
                dp.Iy = System.Convert.ToDouble(linhas[cc + (tot_perfis * (++soma))]);
                dp.Wy = System.Convert.ToDouble(linhas[cc + (tot_perfis * (++soma))]);
                dp.ry = System.Convert.ToDouble(linhas[cc + (tot_perfis * (++soma))]);
                dp.x = System.Convert.ToDouble(linhas[cc + (tot_perfis * (++soma))]);

                Dados_Perfil_u_Gerdau.Add(dp);
            }

            lbCatalogoLaminados.SelectedIndex = 0;
        }
        void Carrega_t_Laminados()
        {
            lbCatalogoLaminados.Items.Clear();
            lbCatalogoLaminados.AccessibleDescription = btLaminadoT.AccessibleDescription;

            separador_decimal_windows = Convert.ToString(System.Threading.Thread.CurrentThread.CurrentCulture.NumberFormat.NumberDecimalSeparator);

            Dados_Perfil_t_Gerdau = new List<TPropriedades_Perfil_t_Gerdau>();
            TPropriedades_Perfil_t_Gerdau dp;

            string arq = System.IO.Path.GetDirectoryName(Application.ExecutablePath) + "\\perfil_t_laminados.txt";
            List<string> linhas = new List<string>();
            string lin;
            linhas = System.IO.File.ReadLines(arq).ToList();
            if (separador_decimal_windows == ".")
            {
                for (int i = 0; i < linhas.Count(); i++)
                {
                    //lin = "1.088,77";//linhas[i];
                    lin = linhas[i];
                    linhas[i] = lin.Replace(".", "");
                    linhas[i] = linhas[i].Replace(",", ".");
                }
            }
            int tot_perfis = 11;
            int soma, cc = 0;

            for (int i = 0; i < tot_perfis; i++)
            {
                dp = new TPropriedades_Perfil_t_Gerdau();
                cc = (tot_perfis + i);
                soma = 0;

                lbCatalogoLaminados.Items.Add(linhas[i]);
                dp.nome = linhas[i];
                dp.d = System.Convert.ToDouble(linhas[cc]);
                dp.tw = System.Convert.ToDouble(linhas[cc + (tot_perfis * (++soma))]);
                dp.massa = System.Convert.ToDouble(linhas[cc + (tot_perfis * (++soma))]);
                dp.area = System.Convert.ToDouble(linhas[cc + (tot_perfis * (++soma))]);
                dp.Ix = System.Convert.ToDouble(linhas[cc + (tot_perfis * (++soma))]);
                dp.Wx = System.Convert.ToDouble(linhas[cc + (tot_perfis * (++soma))]);
                dp.rx = System.Convert.ToDouble(linhas[cc + (tot_perfis * (++soma))]);
                dp.Iy = System.Convert.ToDouble(linhas[cc + (tot_perfis * (++soma))]);
                dp.Wy = System.Convert.ToDouble(linhas[cc + (tot_perfis * (++soma))]);
                dp.ry = System.Convert.ToDouble(linhas[cc + (tot_perfis * (++soma))]);
                dp.x = System.Convert.ToDouble(linhas[cc + (tot_perfis * (++soma))]);

                Dados_Perfil_t_Gerdau.Add(dp);
            }

            lbCatalogoLaminados.SelectedIndex = 0;
        }
        void Carrega_i_Laminados()
        {
            lbCatalogoLaminados.Items.Clear();
            lbCatalogoLaminados.AccessibleDescription = @"Biblioteca\sec2";

            separador_decimal_windows = Convert.ToString(System.Threading.Thread.CurrentThread.CurrentCulture.NumberFormat.NumberDecimalSeparator);

            Dados_Perfil_i_Gerdau = new List<TPropriedades_Perfil_i_Gerdau>();
            TPropriedades_Perfil_i_Gerdau dp;

            string arq = System.IO.Path.GetDirectoryName(Application.ExecutablePath) + "\\perfil_i_laminados.txt";
            List<string> linhas = new List<string>();
            string lin;
            linhas = System.IO.File.ReadLines(arq).ToList();
            if (separador_decimal_windows == ".")
            {
                for (int i = 0; i < linhas.Count(); i++)
                {
                    //lin = "1.088,77";//linhas[i];
                    lin = linhas[i];
                    linhas[i] = lin.Replace(".", "");
                    linhas[i] = linhas[i].Replace(",", ".");
                }
            }
            int tot_perfis = 8;
            int soma, cc = 0;
            //massa, d, tw, bf, tf, area, Ix, Wx, rx, Iy, Wy, ry, rt;
            for (int i = 0; i < tot_perfis; i++)
            {
                dp = new TPropriedades_Perfil_i_Gerdau();
                cc = (tot_perfis + i);
                soma = 0;

                lbCatalogoLaminados.Items.Add(linhas[i]);
                dp.nome = linhas[i];
                dp.massa = System.Convert.ToDouble(linhas[cc]);
                dp.d = System.Convert.ToDouble(linhas[cc + (tot_perfis * (++soma))]);
                dp.tw = System.Convert.ToDouble(linhas[cc + (tot_perfis * (++soma))]);
                dp.bf = System.Convert.ToDouble(linhas[cc + (tot_perfis * (++soma))]);
                dp.tf = System.Convert.ToDouble(linhas[cc + (tot_perfis * (++soma))]);
                dp.area = System.Convert.ToDouble(linhas[cc + (tot_perfis * (++soma))]);
                dp.Ix = System.Convert.ToDouble(linhas[cc + (tot_perfis * (++soma))]);
                dp.Wx = System.Convert.ToDouble(linhas[cc + (tot_perfis * (++soma))]);

                dp.rx = System.Convert.ToDouble(linhas[cc + (tot_perfis * (++soma))]);
                dp.Iy = System.Convert.ToDouble(linhas[cc + (tot_perfis * (++soma))]);
                dp.Wy = System.Convert.ToDouble(linhas[cc + (tot_perfis * (++soma))]);
                dp.ry = System.Convert.ToDouble(linhas[cc + (tot_perfis * (++soma))]);

                dp.rt = System.Convert.ToDouble(linhas[cc + (tot_perfis * (++soma))]);

                Dados_Perfil_i_Gerdau.Add(dp);
            }

            lbCatalogoLaminados.SelectedIndex = 0;
        }
        void AbreSecaoConformeBiblioteca(string tipo,string nome, int item, bool editar, string template, string caminho_template)
        {
            List<double> valorCotas = new List<double>();
            List<double> valorRaios = new List<double>();

            if (tipo == "perfil w")
            {
                //inserir na ordem que está inserido as cotas no template
                valorCotas.Add((Dados_Perfil_W_Gerdau[item].BF));
                valorCotas.Add((Dados_Perfil_W_Gerdau[item].D));
                valorCotas.Add((Dados_Perfil_W_Gerdau[item].TF));
                valorCotas.Add((Dados_Perfil_W_Gerdau[item].TW));

                valorRaios.Add((double.Parse(Dados_Perfil_W_Gerdau[item].raio)));
            }
            else
            if (tipo == "perfil i")
            {
                //inserir na ordem que está inserido as cotas no template
                valorCotas.Add((Dados_Perfil_i_Gerdau[item].tw));
                valorCotas.Add((Dados_Perfil_i_Gerdau[item].d));
                valorCotas.Add((Dados_Perfil_i_Gerdau[item].bf));
                valorCotas.Add((Dados_Perfil_i_Gerdau[item].tf));
                valorCotas.Add(Dados_Perfil_i_Gerdau[item].tf + 
                              (0.14*((Dados_Perfil_i_Gerdau[item].bf/2) - (Dados_Perfil_i_Gerdau[item].tw / 2))));

                valorRaios.Add((Dados_Perfil_i_Gerdau[item].tw));
                valorRaios.Add((0.4* Dados_Perfil_i_Gerdau[item].tf));

            }
            else
            if (tipo == "cantoneira")
            {
                //inserir na ordem que está inserido as cotas no template
                valorCotas.Add((Dados_Cantoneira_Gerdau[item].b));
                valorCotas.Add((Dados_Cantoneira_Gerdau[item].b));
                valorCotas.Add((Dados_Cantoneira_Gerdau[item].t));
                valorCotas.Add((Dados_Cantoneira_Gerdau[item].t));

                valorRaios.Add(1.2 * Dados_Cantoneira_Gerdau[item].t);
                valorRaios.Add(0.5 * Dados_Cantoneira_Gerdau[item].t);
                valorRaios.Add(0.5 * Dados_Cantoneira_Gerdau[item].t);
            }
            else
            if (tipo == "perfil u")
            {
                //inserir na ordem que está inserido as cotas no template
                valorCotas.Add((Dados_Perfil_u_Gerdau[item].d));
                valorCotas.Add((Dados_Perfil_u_Gerdau[item].bf));
                valorCotas.Add((Dados_Perfil_u_Gerdau[item].tf));
                valorCotas.Add((Dados_Perfil_u_Gerdau[item].tw));
                
                //inclinação : 17 por cento da metade da mesa
                valorCotas.Add(Dados_Perfil_u_Gerdau[item].tf +
                              (0.17 * (Dados_Perfil_u_Gerdau[item].bf - Dados_Perfil_u_Gerdau[item].tw)));

                valorRaios.Add(0.4 * Dados_Perfil_u_Gerdau[item].tf);
                valorRaios.Add(0.4 * Dados_Perfil_u_Gerdau[item].tf);
                valorRaios.Add(1.8 * Dados_Perfil_u_Gerdau[item].tf);
            }
            else
            if (tipo == "perfil t")
            {
                //inserir na ordem que está inserido as cotas no template
                valorCotas.Add((Dados_Perfil_t_Gerdau[item].d));
                valorCotas.Add((Dados_Perfil_t_Gerdau[item].d));
                valorCotas.Add(Dados_Perfil_t_Gerdau[item].tw);
                valorCotas.Add(Dados_Perfil_t_Gerdau[item].tw);

                valorRaios.Add(1.2 * Dados_Perfil_t_Gerdau[item].tw);
            }

            AbrirSecao(false, template, caminho_template, nome, ref valorCotas, ref valorRaios, true);
        }

        private void btLaminadoW_Click(object sender, EventArgs e)
        {
            lbTiposLaminado.Items.Clear();
            lbTiposLaminado.Items.Add("Perfil W / H - Gerdau");
            lbTiposLaminado.Items.Add("Perfil I - Gerdau");

            lbTiposLaminado.SelectedIndex = 0;
            Carrega_W_Laminados();
            labelTipoLaminado.Text = (sender as Button).AccessibleName;

        }

        private void lbTiposLaminado_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (labelTipoLaminado.Text == btLaminadoW.AccessibleName)
            {
                if (lbTiposLaminado.SelectedIndex == 0)
                    Carrega_W_Laminados();
                else
                if (lbTiposLaminado.SelectedIndex == 1)
                    Carrega_i_Laminados();
            }
            else
            if (labelTipoLaminado.Text == btLaminadoU.AccessibleName)
            {
                if (lbTiposLaminado.SelectedIndex == 0)
                    Carrega_u_Laminados();
            }
            else
            if (labelTipoLaminado.Text == btLaminadoT.AccessibleName)
            {
                if (lbTiposLaminado.SelectedIndex == 0)
                    Carrega_t_Laminados();
            }
            else
            if (labelTipoLaminado.Text == btLaminadoCantoneira.AccessibleName)
            {
                if (lbTiposLaminado.SelectedIndex == 0)
                    Carrega_Cantoneiras_Laminadas();
            }
        }

        private void btLaminadoCantoneira_Click(object sender, EventArgs e)
        {
            lbTiposLaminado.Items.Clear();
            lbTiposLaminado.Items.Add("Cant. abas iguais - Gerdau");

            lbTiposLaminado.SelectedIndex = 0;
            Carrega_Cantoneiras_Laminadas();
            labelTipoLaminado.Text = (sender as Button).AccessibleName;
        }

        private void btLaminadoU_Click(object sender, EventArgs e)
        {
            lbTiposLaminado.Items.Clear();
            lbTiposLaminado.Items.Add("Perfil U - Gerdau");
            lbTiposLaminado.SelectedIndex = 0;
            Carrega_u_Laminados();
            labelTipoLaminado.Text = (sender as Button).AccessibleName;
        }

        private void btLaminadoT_Click(object sender, EventArgs e)
        {
            lbTiposLaminado.Items.Clear();
            lbTiposLaminado.Items.Add("Perfil T - Gerdau");
            lbTiposLaminado.SelectedIndex = 0;
            Carrega_t_Laminados();
            labelTipoLaminado.Text = (sender as Button).AccessibleName;
        }
        
        void AbrirSecao(bool editar, string template, string caminho_template, string nome, ref List<double> valorCotas, ref List<double> valorRaios, bool biblioteca = false)
        {
            calculasecao = new FCalculaSecao(gerenciador,  template, caminho_template, nome, testasecao, ref valorCotas, ref valorRaios, biblioteca);
            calculasecao.dadosBarra = dadosBarra;
            calculasecao.escolheSecao = this;

            DialogResult result = calculasecao.ShowDialog();

            if (result == DialogResult.OK)
            {
                gerenciador.formDesenho.Alterou(true);
                
           //     gerenciador.formDesenho.AtualizaShaders(true);

                gerenciador.formDesenho.DesenhaObjetos();
                gerenciador.formDesenho.glControl.SwapBuffers();
        //        gerenciador.formDesenho.glControl.MakeCurrent();
            }
        }

        private void ClickBotoes(object sender, EventArgs e)
        {
            string template = (sender as Button).AccessibleDescription;
            string CaminhoTemplate = Directory.GetCurrentDirectory() + @"\TemplateSec\" + template;
          
            if (File.Exists(CaminhoTemplate+"_vec"))
            {
                List<double> valorCotas = new List<double>();
                List<double> valorRaios = new List<double>();

                AbrirSecao(false, template, CaminhoTemplate, (sender as Button).AccessibleName, ref valorCotas, ref valorRaios);
            }
            else
            {
                MessageBox.Show("O template da seção não existe! Contate o desenvolvedor.\r Template: " + CaminhoTemplate, "Atenção", MessageBoxButtons.OK, MessageBoxIcon.Information);

            }
        }

        private void lbCatalogoLaminados_DoubleClick(object sender, EventArgs e)
        {
            string caminho_template = Directory.GetCurrentDirectory() + @"\TemplateSec\" + (sender as ListBox).AccessibleDescription;

            string tipo = "";
            if (caminho_template.Contains("sec1"))
                tipo = "perfil w";
            else
            if (caminho_template.Contains("sec2"))
                tipo = "perfil i";
            else
            if (caminho_template.Contains("sec3"))
                tipo = "cantoneira";
            else
            if (caminho_template.Contains("sec4"))
                tipo = "perfil u";
            else
            if (caminho_template.Contains("sec5"))
                tipo = "perfil t";

            if (File.Exists(caminho_template + "_vec"))
            {
                AbreSecaoConformeBiblioteca(tipo, (string)(sender as ListBox).SelectedItem, lbCatalogoLaminados.SelectedIndex, false, (sender as ListBox).AccessibleDescription, caminho_template);
            }
            else
            {
                MessageBox.Show("O template da seção não existe! Contate o desenvolvedor.\r Template: " + caminho_template, "Atenção", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
        }

    }
}
