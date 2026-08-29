using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Threading;
namespace PG
{
    public partial class FDadosMaterial : Form
    {
        public FDadosMaterial()
        {
            InitializeComponent();
        }
        public FDadosMaterial(Gerenciador ger)
        {
            gerenciador = ger;
            InitializeComponent();
            ShowInTaskbar = false;
        }

        private void cbMaterial_SelectedIndexChanged(object sender, EventArgs e)
        {
            imAco.Visible = false;
            imConcreto.Visible = false;
            cbAco.Visible = false;
            cbConcreto.Visible = false;
            gbConcreto.Visible = false;
            gbAco.Visible = false;
            lbE2.Visible = false;
            edEcs.Visible = false;
            lbMpaECS.Visible = false;
            if (cbMaterial.SelectedIndex == 0)
            {
            //    cbAco.Visible = true;
                cbAco.SelectedIndex = 1;
                imAco.Visible = true;
                imAco.Left = 240;
                gbAco.Visible = true;
                edEcs.Text = "0";
                lbE1.Text = "Módulo elast. (E)";
            }

            if (cbMaterial.SelectedIndex == 1)
            {
                imConcreto.Left = 240;
                lbMpaECS.Visible = true;
                edEcs.Visible = true;
                gbConcreto.Visible = true;
                imConcreto.Visible = true;
         //       cbConcreto.Visible = true;
                cbConcreto.SelectedIndex = 1;
                gbAco.Visible = false;
                lbE2.Visible = true;
                lbE1.Text = "Módulo elast. inicial (Eci)";
                lbE2.Text = "Módulo elast. secante (Ecs)";
            }

            Nome.Focus();
            Nome.SelectAll();
        }

        private void cbAco_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (!carregando)
            {
                Nome.Text = cbAco.Text;
                edE.Text = Dados_Materiais_Aco[cbAco.SelectedIndex].e.ToString("n2");
                edG.Text = Dados_Materiais_Aco[cbAco.SelectedIndex].g.ToString("n2");
                edPoisson.Text = Dados_Materiais_Aco[cbAco.SelectedIndex].poisson.ToString("n2");
                edPesoEsp.Text = Dados_Materiais_Aco[cbAco.SelectedIndex].pesoespe.ToString("n2");
                edCoefTermico.Text = Dados_Materiais_Aco[cbAco.SelectedIndex].coeftermico.ToString("n5");
                edFu.Text = Dados_Materiais_Aco[cbAco.SelectedIndex].fu.ToString("n2");
                edFy.Text = Dados_Materiais_Aco[cbAco.SelectedIndex].fy.ToString("n2");
            }
        }
        public bool alterando;

        public void Limpar()
        {
        //    cbMaterial.SelectedIndex = 0;
            edG.Text = "0";
            edE.Text = "0";
            edEcs.Text = "0";

            edFy.Text = "0";
            edFu.Text = "0";

            edPoisson.Text = "0";
            edCoefTermico.Text = "0";
            edPesoEsp.Text = "0";
            edFcd.Text = "0";
            edFck.Text = "0";
            Nome.Clear();
            Nome.Focus();
        }
        bool carregando;
        private void cbConcreto_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (!carregando)
            {
                Nome.Text = cbConcreto.Text;
                edPoisson.Text = Dados_Materiais_Concreto[cbConcreto.SelectedIndex].poisson.ToString("n2");
                edPesoEsp.Text = Dados_Materiais_Concreto[cbConcreto.SelectedIndex].pesoespe.ToString("n2");
                edCoefTermico.Text = Dados_Materiais_Concreto[cbConcreto.SelectedIndex].coeftermico.ToString("n5");
                edFck.Text = (Dados_Materiais_Concreto[cbConcreto.SelectedIndex].fck * 10).ToString("n2");
                cbAgregado.SelectedIndex = 0;
                cbCoefMinoracao.SelectedIndex = 0;
                CalculaModulos(double.Parse(edFck.Text), cbAgregado.SelectedIndex);
            }
          /*  if (!alterando)
                if (cbConcreto.SelectedIndex > 0)
                {

                    CarregaClasses(cbConcreto.SelectedIndex - 1);
                    clasfck.Text = Cfg.cla sses[cbConcreto.SelectedIndex - 1].fck.ToString("n2");
                    clasfcd.Text = Cfg.classes[cbConcreto.SelectedIndex - 1].fcd.ToString("n2");
                }*/
        }

        struct DadosMaterial
        {
            public double e, g, poisson, pesoespe, coeftermico, fy, fu, fck;
            public string nome;
        }
        List<DadosMaterial> Dados_Materiais_Aco, Dados_Materiais_Concreto;
        string separador_decimal_windows;
        void CarregaDadosAco()
        {
            Dados_Materiais_Aco = new List<DadosMaterial>();
            DadosMaterial dp;

            string arq = System.IO.Path.GetDirectoryName(Application.ExecutablePath) + "\\mat_aco.txt";
            string item, texto;
            int soma;
            foreach (string line in System.IO.File.ReadLines(arq))
            {
                //A36*200000 77220 0.3 7850 0.00005 250 400
                if (line.Contains("nome"))
                    continue;

                if (line.Trim() == string.Empty)
                    break;

                dp = new DadosMaterial();

                item = line.Substring(0, line.IndexOf("*"));
                dp.nome = item;
                cbAco.Items.Add(item);

                soma = item.Length;
                texto = line.Substring(soma + 1, line.Length - soma - 1);
                if (separador_decimal_windows == ",")
                    item = texto.Substring(0, texto.IndexOf(" ")).Replace(".", ",");
                else
                    item = texto.Substring(0, texto.IndexOf(" "));
                dp.e = System.Convert.ToDouble(item);
                soma += item.Length + 1;

                texto = line.Substring(soma + 1, line.Length - soma - 1);
                if (separador_decimal_windows == ",")
                    item = texto.Substring(0, texto.IndexOf(" ")).Replace(".", ",");
                else
                    item = texto.Substring(0, texto.IndexOf(" "));
                dp.g = System.Convert.ToDouble(item);
                soma += item.Length + 1;

                texto = line.Substring(soma + 1, line.Length - soma - 1);
                if (separador_decimal_windows == ",")
                    item = texto.Substring(0, texto.IndexOf(" ")).Replace(".", ",");
                else
                    item = texto.Substring(0, texto.IndexOf(" "));
                dp.poisson = System.Convert.ToDouble(item);
                soma += item.Length + 1;

                texto = line.Substring(soma + 1, line.Length - soma - 1);
                if (separador_decimal_windows == ",")
                    item = texto.Substring(0, texto.IndexOf(" ")).Replace(".", ",");
                else
                    item = texto.Substring(0, texto.IndexOf(" "));
                dp.pesoespe = System.Convert.ToDouble(item);
                soma += item.Length + 1;

                texto = line.Substring(soma + 1, line.Length - soma - 1);
                if (separador_decimal_windows == ",")
                    item = texto.Substring(0, texto.IndexOf(" ")).Replace(".", ",");
                else
                    item = texto.Substring(0, texto.IndexOf(" "));
                dp.coeftermico = System.Convert.ToDouble(item);
                soma += item.Length + 1;

                texto = line.Substring(soma + 1, line.Length - soma - 1);
                if (separador_decimal_windows == ",")
                    item = texto.Substring(0, texto.IndexOf(" ")).Replace(".", ",");
                else
                    item = texto.Substring(0, texto.IndexOf(" "));
                dp.fy = System.Convert.ToDouble(item);
                soma += item.Length + 1;

                texto = line.Substring(soma + 1, line.Length - soma - 1);
                if (separador_decimal_windows == ",")
                    item = texto.Substring(0, texto.IndexOf(" ")).Replace(".", ",");
                else
                    item = texto.Substring(0, texto.IndexOf(" "));
                dp.fu = System.Convert.ToDouble(item);
                soma += item.Length + 1;
                Dados_Materiais_Aco.Add(dp);
            }
            // HabilitaSecao(btW);

            //   cbTipo.SelectedIndex = 0;

        }
        void CarregaDadosConcreto()
        {
            Dados_Materiais_Concreto = new List<DadosMaterial>();
            DadosMaterial dp;

            string arq = System.IO.Path.GetDirectoryName(Application.ExecutablePath) + "\\mat_concreto.txt";
            string item, texto;
            int soma;
            foreach (string line in System.IO.File.ReadLines(arq))
            {
              //C-20*0.2 2500 0.00005 20
                if (line.Contains("nome"))
                    continue;

                if (line.Trim() == string.Empty)
                    break;

                dp = new DadosMaterial();

                item = line.Substring(0, line.IndexOf("*"));
                dp.nome = item;
                cbConcreto.Items.Add(item);

                soma = item.Length;
                texto = line.Substring(soma + 1, line.Length - soma - 1);

                //o formato desse txt é com ponto
                if (separador_decimal_windows == ",")
                    item = texto.Substring(0, texto.IndexOf(" ")).Replace(".", ",");
                else
                  item = texto.Substring(0, texto.IndexOf(" "));

                dp.poisson = System.Convert.ToDouble(item);
                soma += item.Length + 1;

                texto = line.Substring(soma + 1, line.Length - soma - 1);
                if (separador_decimal_windows == ",")
                    item = texto.Substring(0, texto.IndexOf(" ")).Replace(".", ",");
                else
                    item = texto.Substring(0, texto.IndexOf(" "));
                    
                dp.pesoespe = System.Convert.ToDouble(item);
                soma += item.Length + 1;

                texto = line.Substring(soma + 1, line.Length - soma - 1);
                if (separador_decimal_windows == ",")
                    item = texto.Substring(0, texto.IndexOf(" ")).Replace(".", ",");
                else
                    item = texto.Substring(0, texto.IndexOf(" "));
                dp.coeftermico = System.Convert.ToDouble(item);
                soma += item.Length + 1;

                texto = line.Substring(soma + 1, line.Length - soma - 1);
                if (separador_decimal_windows == ",")
                    item = texto.Substring(0, texto.IndexOf(" ")).Replace(".", ",");
                else
                    item = texto.Substring(0, texto.IndexOf(" "));
                dp.fck = System.Convert.ToDouble(item);
                soma += item.Length + 1;
                Dados_Materiais_Concreto.Add(dp);
            }
        }
        private void cbCoefMinoracao_SelectedIndexChanged(object sender, EventArgs e)
        {
            CalculaFcd();
        }

        private void cbAgregado_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (edFck.Text.Trim() != "")
                CalculaModulos(double.Parse(edFck.Text), cbAgregado.SelectedIndex);
        }
        void CalculaFcd()
        {
            double fcd,fck,min;

            if (!carregando)
            {
                if (cbCoefMinoracao.SelectedIndex > -1)
                {
                    min = System.Convert.ToDouble(cbCoefMinoracao.Text);
                    fck = System.Convert.ToDouble(edFck.Text);
                    fcd = fck / min;
                    edFcd.Text = (fcd).ToString("n2");
                }
            }
        }
        void CalculaModulos(double fck, int tipoAgregado)
        {
            if (cbMaterial.SelectedIndex == 1  && !carregando)
            {
                double ecs, gama1, coefAgregado;

                /*   fck em kgf/cm²   */

                /*
                granito
                calcário
                arenito
                basalto
                */
                fck /= 10;
                coefAgregado = 0;
                ecs = 0;
                switch (tipoAgregado)
                {
                    case 0: coefAgregado = 1; break;
                    case 1: coefAgregado = 0.9; break;
                    case 2: coefAgregado = 0.7; break;
                    case 3: coefAgregado = 1.2; break;
                };

                double eci = coefAgregado * 5600 * Math.Sqrt(fck);
                gama1 = 0.8 + (0.2 * (fck / 80));
                ecs = (gama1 * eci);

                edE.Text = eci.ToString("n2");  //CalculaModulos(fck, classe, clasagregado.SelectedIndex, ref eci).ToString("n2");
                edG.Text = (ecs * 0.4).ToString("n2");
                edEcs.Text = ecs.ToString("n2");
            }
        }
        int id;
        public SConfiguracoesProjeto Cfg;
        public void Carrega(int _ri,SConfiguracoesProjeto _Cfg)
        {
            carregando = true;
            this.Cfg = _Cfg;
            alterando = true;
            this.ri = _ri;
            id = _Cfg.materiais[_ri].Id;

            btCor.BackColor = _Cfg.materiais[_ri].Cor_;
            Nome.Text = _Cfg.materiais[_ri].Descricao;
            edFcd.Text = _Cfg.materiais[_ri].fcd.ToString("n2");
            edFck.Text = _Cfg.materiais[_ri].fck.ToString("n2");
            edFu.Text = _Cfg.materiais[_ri].fu.ToString("n2");
            edFy.Text = _Cfg.materiais[_ri].fy.ToString("n2");

            edE.Text = _Cfg.materiais[_ri].E.ToString("n2");
            edEcs.Text = _Cfg.materiais[_ri].Ecs.ToString("n2");

            edG.Text = _Cfg.materiais[_ri].G.ToString("n2");
            edPoisson.Text = _Cfg.materiais[_ri].V.ToString("n2");
            edPesoEsp.Text = _Cfg.materiais[_ri].PesoEspecifico.ToString("n2");
            edCoefTermico.Text = _Cfg.materiais[_ri].CoefTermico.ToString("n5");
            cbMaterial.SelectedIndex = _Cfg.materiais[_ri].Tipo;
            cbAco.SelectedIndex = _Cfg.materiais[_ri].tipoAco;
            cbConcreto.SelectedIndex = _Cfg.materiais[_ri].tipoConcreto;
            cbAgregado.SelectedIndex = _Cfg.materiais[_ri].Agregado;
            cbCoefMinoracao.SelectedIndex = _Cfg.materiais[_ri].coefMin;
            btSalvar.Enabled = true;
            Nome.Focus();
            carregando = false;
        }
        private void btNovo_Click(object sender, EventArgs e)
        {

        }

        private void FDadosMaterial_Load(object sender, EventArgs e)
        {
            imAco.Visible = false;
            imConcreto.Visible = false;
            gbConcreto.Visible = false;
            Nome.Focus();

            cbConcreto.Top = 50;
            cbConcreto.Left = 129;
            cbAco.Top = 50;
            cbAco.Left = 129;

            gbAco.Top = 84;
            gbAco.Left = 22;

            separador_decimal_windows = Convert.ToString(System.Threading.Thread.CurrentThread.CurrentCulture.NumberFormat.NumberDecimalSeparator);
            if (separador_decimal_windows == ",")
            {
                cbCoefMinoracao.Items.Clear();
                cbCoefMinoracao.Items.Add("1,3");
                cbCoefMinoracao.Items.Add("1,4");
                cbCoefMinoracao.Items.Add("1,5");
            }

            CarregaDadosAco();
            CarregaDadosConcreto();
            Limpar();

            Cfg = gerenciador.ConfiguracoesPGi.CfgProjeto;
            if (!alterando)
            {
           //     cbAco.SelectedIndex = 0;
            //    cbAco_SelectedIndexChanged(cbAco,null);
            }

            gbAco.Visible = false;

            if (alterando)
              Carrega(ri, Cfg);

        }
        Gerenciador gerenciador;
        private void FDadosMaterial_Move(object sender, EventArgs e)
        {
            gerenciador.AtualizaDesenho();
        }

        private void FDadosMaterial_FormClosed(object sender, FormClosedEventArgs e)
        {
            gerenciador.Materiais.dadosmaterial = null;

        }

        private void button1_Click(object sender, EventArgs e)
        {

        }

        private void button1_Click_1(object sender, EventArgs e)
        {
            if (edFck.Text.Trim() != "")
                CalculaModulos(double.Parse(edFck.Text), cbAgregado.SelectedIndex);
        }

        private void button3_Click(object sender, EventArgs e)
        {
            Close();
        }
        public int ri;
        private void btSalvar_Click(object sender, EventArgs e)
        {
            if (Nome.Text.Trim() == "")
            {
                MessageBox.Show("A descrição do material deve ser informada.", "Atenção", MessageBoxButtons.OK, MessageBoxIcon.Stop);
                Nome.Select();
                return;
            }

            if (!alterando)
            {
                if (Cfg.materiais == null)
                    Cfg.materiais = new List<TMateriais>();

                int id_;
                if (Cfg.materiais.Count > 0)
                    id_ = Cfg.materiais.Max(o => o.Id) + 1;
                else
                    id_ = 1;

                Cfg.materiais.Add(new TMateriais(id_, Nome.Text,
                    System.Convert.ToDouble(edE.Text),
                    System.Convert.ToDouble(edEcs.Text),
                    System.Convert.ToDouble(edG.Text),
                    System.Convert.ToDouble(edPoisson.Text),
                    System.Convert.ToDouble(edPesoEsp.Text),
                    System.Convert.ToDouble(edCoefTermico.Text),
                     btCor.BackColor,
                     cbMaterial.SelectedIndex,
                     cbAco.SelectedIndex,
                     cbConcreto.SelectedIndex,
                     cbCoefMinoracao.SelectedIndex,
                     cbAgregado.SelectedIndex,
                     cbMaterial.SelectedIndex == 0 ? 0 : System.Convert.ToDouble(edFck.Text),
                     cbMaterial.SelectedIndex == 0 ? 0 : System.Convert.ToDouble(edFcd.Text),
                     cbMaterial.SelectedIndex == 0 ? System.Convert.ToDouble(edFy.Text):0,
                     cbMaterial.SelectedIndex == 0 ? System.Convert.ToDouble(edFu.Text):0,
                     cbMaterial.SelectedIndex == 0 ? cbAco.SelectedIndex : cbConcreto.SelectedIndex));
            }
            else
            {
                Cfg.materiais[ri].Descricao = Nome.Text;
                Cfg.materiais[ri].E = System.Convert.ToDouble(edE.Text);
                Cfg.materiais[ri].Ecs = System.Convert.ToDouble(edEcs.Text);

                Cfg.materiais[ri].G = System.Convert.ToDouble(edG.Text);
                Cfg.materiais[ri].V = System.Convert.ToDouble(edPoisson.Text);
                Cfg.materiais[ri].PesoEspecifico = System.Convert.ToDouble(edPesoEsp.Text);
                Cfg.materiais[ri].CoefTermico = System.Convert.ToDouble(edCoefTermico.Text);
                Cfg.materiais[ri].Tipo = (int)cbMaterial.SelectedIndex;
                Cfg.materiais[ri].tipoAco = (int)cbAco.SelectedIndex;
                Cfg.materiais[ri].tipoConcreto = (int)cbConcreto.SelectedIndex;
                Cfg.materiais[ri].coefMin = (int)cbCoefMinoracao.SelectedIndex;
                Cfg.materiais[ri].Agregado = (int)cbAgregado.SelectedIndex;
                Cfg.materiais[ri].fck = System.Convert.ToDouble(edFck.Text);
                Cfg.materiais[ri].fcd = System.Convert.ToDouble(edFcd.Text);
                Cfg.materiais[ri].fcd = System.Convert.ToDouble(edFcd.Text);
                Cfg.materiais[ri].fu = System.Convert.ToDouble(edFu.Text);
                Cfg.materiais[ri].fy = System.Convert.ToDouble(edFy.Text);
                Cfg.materiais[ri].indiceTipo = (cbMaterial.SelectedIndex == 0 ? cbAco.SelectedIndex : cbConcreto.SelectedIndex);

                btCor.BackColor = btCor.BackColor;
                Cfg.materiais[ri].Cor_ = btCor.BackColor;

                AtualizaPesoProprioBarrasProjeto();
            }

            Nome.Clear();
            edG.Text = "0";
            edE.Text = "0";
            edEcs.Text = "0";

            edPoisson.Text = "0";
            edCoefTermico.Text = "0";
            edPesoEsp.Text = "0";
            edFcd.Text = "0";
            edFck.Text = "0";

            btSalvar.Enabled = false;
            gerenciador.Materiais.Carrega();
            gerenciador.formDesenho.Alterou(true);
            this.Close();
        }

        void AtualizaPesoProprioBarrasProjeto()
        {
            foreach (TSecao sec in gerenciador.formDesenho.Secoes)
            {
                double pp = sec.material.PesoEspecifico * sec.propriedades.area / 100; //kgf/m -> kn/m
                sec.PesoProprio = pp;

                foreach (TBarraGenerica b in gerenciador.formDesenho.Estrutura.barras)
                {
                    if (b.Dados.secao.id == sec.id)
                    {
                        b.Dados.secao.PesoProprio = pp;
                        b.CriaPesoProprio();
                    }
                }
            }
            gerenciador.formDesenho.AtualizaCargas();
            gerenciador.formDesenho.Alterou(true);
            //            foreach (TBarraGenerica b in gerenciador.formDesenho.Estrutura.barras)
            //               b.CriaPesoProprio();

        }

        private void FDadosMaterial_FormClosing(object sender, FormClosingEventArgs e)
        {
            gerenciador.AtualizaDesenho();
        }

        private void btCor_Click(object sender, EventArgs e)
        {
            if (colorDialog1.ShowDialog() == DialogResult.OK)
                btCor.BackColor = colorDialog1.Color;
        }

        private void edE_Leave(object sender, EventArgs e)
        {
            if ((sender as TextBox).Text.Trim() != "")
            {
                if ((sender as TextBox) == edCoefTermico)
                {
                    (sender as TextBox).Text = double.Parse((sender as TextBox).Text).ToString("n5");
                }
                else

                (sender as TextBox).Text = double.Parse((sender as TextBox).Text).ToString("n2");
            }
        }

        private void edE_KeyPress(object sender, KeyPressEventArgs e)
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
    }
}
