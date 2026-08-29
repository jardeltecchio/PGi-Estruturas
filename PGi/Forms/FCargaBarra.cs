using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using static Microsoft.WindowsAPICodePack.Shell.PropertySystem.SystemProperties;

namespace PG
{
    public partial class FCarga : Form
    {
        TCargaPontual cargaPontual;
        public TCargaLinear cargaLinear;
        public bool alterando;
        string tipo;
        public FCarga()
        {
            InitializeComponent();
        }

        Gerenciador gerenciador;
        public FCarga(Gerenciador ger, string _tipo, TCargaPontual carP, TCargaLinear carL, bool alteracao)
        {
            this.gerenciador = ger;
            InitializeComponent();

            this.cargaPontual = carP;
            this.cargaLinear = carL;
            this.tipo = _tipo;

            //lbUnidade.Text = gerenciador.cbUnForca.Text + "/" + gerenciador.cbUnComp.Text;
            rbGlobal.Checked = true;

            if (alteracao)
            {
                this.Text = "Alterar carregamento";
                alterando = true;
            }
            ShowInTaskbar = false;
        }

        void Carregar()
        {
            if (this.cargaLinear != null)
            {

                if (cargaLinear.Dados.DirecaoProjecao == 0) rbX.Checked = true;
                if (cargaLinear.Dados.DirecaoProjecao == 1) rbY.Checked = true;
                if (cargaLinear.Dados.DirecaoProjecao == 2) rbZ.Checked = true;

                if (cargaLinear.Dados.ProjecaoGlobal == 0)
                    rbGlobal.Checked = true;
                else
                    rbLocal.Checked = true;

                edD.Text = cargaLinear.Dados.d.ToString("n2");
                chPosRel.Checked = cargaLinear.Dados.posicaoRelativa;

                idCasoSelecionado = cargaLinear.Dados.idCaso;

                for (int i = 0; i < gerenciador.formDesenho.CasosCarga.Count; i++)
                {
                    if (gerenciador.formDesenho.CasosCarga[i].ID == cargaLinear.Dados.idCaso)
                        cbCasos.SelectedIndex = i-1;
                }

                if (cargaLinear.Dados.distribuida)
                {
                    distribuida();
                    double forca = conv.forca(cargaLinear.Dados.valor, und.kN, gerenciador.ConfiguracoesPGi.CfgProjeto.unidadesProjeto.res_un_forca.ToString());
                    forca = conv.comp(forca, gerenciador.ConfiguracoesPGi.CfgProjeto.unidadesProjeto.res_un_comprimento.ToString(), und.m);
                    Valor.Text = forca.ToString("n"+ gerenciador.ConfiguracoesPGi.CfgProjeto.unidadesProjeto.res_casas.ToString());
                }
                else
                if (cargaLinear.Dados.concentrada)
                {
                    concentrada();
                    double forca = conv.forca(cargaLinear.Dados.valor, und.kN, gerenciador.ConfiguracoesPGi.CfgProjeto.unidadesProjeto.res_un_forca.ToString());
                    Valor.Text = forca.ToString("n" + gerenciador.ConfiguracoesPGi.CfgProjeto.unidadesProjeto.res_casas.ToString());
                }
            }

        }
        public void GravaDados()
        {
            if (this.cargaLinear != null)
            {
                this.cargaLinear.Dados.valor = System.Convert.ToDouble(Valor.Text);
            }
        }

        private void button7_Click(object sender, EventArgs e)
        {
            gerenciador.CargaBarra = null;
            
            this.Close();
        }
        public bool cargaPortico;
        private void button8_Click(object sender, EventArgs e)
        {
            try
            {
                if (Valor.Text == "0.00" || Valor.Text == "0" || Valor.Text.Trim() == "")
                {
                    MessageBox.Show("Valor da carga não foi informado.", "Atenção", MessageBoxButtons.OK, MessageBoxIcon.Stop);
                    Valor.Select();
                    return;
                }
                if (Valor.Text == "0.00" || Valor.Text == "0" || Valor.Text.Trim() == "")
                {
                    MessageBox.Show("Valor da carga não foi informado.", "Atenção", MessageBoxButtons.OK, MessageBoxIcon.Stop);
                    Valor.Select();
                    return;
                }

                if (cbCasos.SelectedIndex == -1)
                {
                    MessageBox.Show("Nenhum caso de carga foi informado.", "Atenção", MessageBoxButtons.OK, MessageBoxIcon.Stop);
                    return;
                }

                double valor = 0;
                if (tabControl1.SelectedIndex == 0) //distribuida
                {
                    valor = conv.forca(System.Convert.ToDouble(Valor.Text), gerenciador.ConfiguracoesPGi.CfgProjeto.unidadesProjeto.res_un_forca.ToString(), und.kN);
                    valor = conv.comp(valor, und.m, gerenciador.ConfiguracoesPGi.CfgProjeto.unidadesProjeto.res_un_comprimento.ToString());
                }
                else
                    valor = conv.forca(System.Convert.ToDouble(Valor.Text), gerenciador.ConfiguracoesPGi.CfgProjeto.unidadesProjeto.res_un_forca.ToString(), und.kN);


                if (alterando)
                {
                    int dp = 2;
                    if (rbX.Checked) dp = 0;
                    if (rbY.Checked) dp = 1;
                    if (rbZ.Checked) dp = 2;
                    List<TCargaLinear> cl = gerenciador.formDesenho.CargasLineares.FindAll(
                     o => o.Dados.valor == valor
                    && o.Dados.idCaso == idCasoSelecionado
                    && o.Dados.ProjecaoGlobal == (rbGlobal.Checked ? 0 : 1)
                    && o.Dados.concentrada == false
                    && o.Dados.distribuida == (tabControl1.SelectedIndex == 0)
                    && o.Dados.posicaoRelativa == chPosRel.Checked
                    && o.Dados.DirecaoProjecao == dp);

                    if (tabControl1.SelectedIndex == 1)
                    {
                        cl = gerenciador.formDesenho.CargasLineares.FindAll(
                            o => o.Dados.valor == valor
                            && o.Dados.idCaso == idCasoSelecionado
                            && o.Dados.ProjecaoGlobal == (rbGlobal.Checked ? 0 : 1)
                            && o.Dados.concentrada == true
                            && o.Dados.distribuida == false
                            && o.Dados.d == System.Convert.ToDouble(edD.Text)
                            && o.Dados.posicaoRelativa == chPosRel.Checked
                            && o.Dados.DirecaoProjecao == dp);

                        if (cl.Exists(o => o.idBarra == cargaLinear.idBarra && o.ID != cargaLinear.ID))
                        {
                            MessageBox.Show("Já existe a carga concentrada de " +valor.ToString("n2")+" kN na mesma posição para esse mesmo caso de carga nesse mesmo elemento!", "Atenção", MessageBoxButtons.OK, MessageBoxIcon.Stop);
                            return;
                        }
                    }
                    else
                    if (cl.Exists(o => o.idBarra == cargaLinear.idBarra && o.ID != cargaLinear.ID))
                    {
                        MessageBox.Show("Já existe a carga distribuída de " + valor.ToString("n2") + " kN para esse mesmo caso de carga nesse mesmo elemento!", "Atenção", MessageBoxButtons.OK, MessageBoxIcon.Stop);
                        return;
                    }

                    if (tabControl1.SelectedIndex == 1) // concentrada
                    {
                        TBarraGenerica bar = gerenciador.formDesenho.Barras.Find(o => o.IDBarra == cargaLinear.idBarra);
                        double posicaoFinal = chPosRel.Checked ? bar.comprimento * System.Convert.ToDouble(edD.Text) : System.Convert.ToDouble(edD.Text);
                        if (posicaoFinal > bar.comprimento)
                        {
                            MessageBox.Show("Com a distância de "+posicaoFinal.ToString("n2")+" m, carga concentrada não ficará posicionada no interior do elemento, portanto ela não pode ser alterada!", "Erro",MessageBoxButtons.OK, MessageBoxIcon.Error);
                            return;
                        }
                    }

                    this.cargaLinear.Dados.valor = valor;
                    this.cargaLinear.Dados.d = System.Convert.ToDouble(edD.Text);
                    this.cargaLinear.Dados.idCaso = idCasoSelecionado;

                    this.cargaLinear.Dados.ProjecaoGlobal = (rbGlobal.Checked ? 0 : 1);
                    this.cargaLinear.Dados.concentrada = tabControl1.SelectedIndex == 1;
                    this.cargaLinear.Dados.distribuida = tabControl1.SelectedIndex == 0;
                    this.cargaLinear.Dados.posicaoRelativa = chPosRel.Checked;

                    this.cargaLinear.Dados.DirecaoProjecao = dp;
                    this.cargaLinear.Dados.cor = gerenciador.formDesenho.CasosCarga.Find(o=>o.ID == idCasoSelecionado).Cor;
                    this.cargaLinear.CriaSetas(true,0,0, gerenciador.formDesenho.EscalaCargas);

                    this.DialogResult = System.Windows.Forms.DialogResult.Yes;
                }
                else
                    gerenciador.NovosDadosDeCargaBarra(this.tipo);

                gerenciador.formDesenho.Alterou(true);
                gerenciador.formDesenho.MostrarCargasPeloCaso();

            }
            catch(Exception er )
            {
                MessageBox.Show("Inserção inválida em algum campo da tela!  " + er.Message);
            }
        }

        private void RigidezGJ_Leave(object sender, EventArgs e)
        {

        }

        private void RigidezGJ_KeyPress(object sender, KeyPressEventArgs e)
        {
         //   e.Handled = (!char.IsDigit(e.KeyChar) && !char.IsControl(e.KeyChar));
        }
        public void AtualizaCasos()
        {
            cbCasos.Items.Clear();
            foreach (TCasosCarga c in gerenciador.formDesenho.CasosCarga)
            {
                if (c.ID != 1)
                  cbCasos.Items.Add(c.Nome);
            }
        }

        private void FCarga_Load(object sender, EventArgs e)
        {
            cbCasos.Items.Clear();

            foreach (TCasosCarga c in gerenciador.formDesenho.CasosCarga)
                if (c.ID != 1)
                    cbCasos.Items.Add(c.Nome);

            if (cbCasos.Items.Count > 0)
            {
                if ((gerenciador.cbCasoCarga.SelectedIndex != 0) /*TODOS*/ &&
                    (gerenciador.cbCasoCarga.SelectedIndex != 1) /*peso proprio*/ &&
                    (gerenciador.cbCasoCarga.SelectedIndex != gerenciador.cbCasoCarga.Items.Count-1) /*Nenhum*/)
                  cbCasos.SelectedIndex = gerenciador.cbCasoCarga.SelectedIndex -2;
                else
                  cbCasos.SelectedIndex = -1;
            }

            if (alterando)
                Carregar();
            else
            {
                distribuida();
            }

            this.Left = 25;
            Valor.Focus();
        }

        private void FCarga_Move(object sender, EventArgs e)
        {
            gerenciador.AtualizaDesenho();
        
        }
        private void Valor_KeyPress(object sender, KeyPressEventArgs e)
        {
            char a = Convert.ToChar(System.Threading.Thread.CurrentThread.CurrentCulture.NumberFormat.NumberDecimalSeparator);
            if (!char.IsControl(e.KeyChar) && !char.IsDigit(e.KeyChar) && (e.KeyChar != a)  && (e.KeyChar != '-'))
            {
                e.Handled = true;
            }

            if (e.KeyChar == (char)Keys.Enter || e.KeyChar == (char)Keys.Escape)
            {
                e.Handled = true;   // stop annoying beep
            }
            base.OnKeyPress(e);

        }

        private void Valor_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
            {
                button8_Click(button8, null);
            }
            if (e.KeyCode == Keys.Escape)
                this.Close();
        }

        private void FCarga_FormClosed(object sender, FormClosedEventArgs e)
        {
            if (!alterando)
            {
                gerenciador.CargaBarra = null;
                gerenciador.formDesenho.CancelaInsercoes();
            }
        }
        private void edD_Leave(object sender, EventArgs e)
        {
            if ((sender as TextBox).Text.Trim() != "")
                (sender as TextBox).Text = double.Parse((sender as TextBox).Text).ToString("n2");
        }
        public int idCasoSelecionado;
        private void cbCasos_SelectedIndexChanged(object sender, EventArgs e)
        {
            pnCorCaso.BackColor = gerenciador.formDesenho.CasosCarga[cbCasos.SelectedIndex+1].Cor;
            if (gerenciador.cbCasoCarga.SelectedIndex != gerenciador.cbCasoCarga.Items.Count - 1)
            {
                gerenciador.cbCasoCarga.SelectedIndex = cbCasos.SelectedIndex + 2;

           //     idCasoSelecionado = gerenciador.formDesenho.CasosCarga[gerenciador.cbCasoCarga.SelectedIndex - 1].ID;

                idCasoSelecionado = gerenciador.formDesenho.CasosCarga[cbCasos.SelectedIndex + 1].ID;
                gerenciador.cbCargas_SelectedIndexChanged(gerenciador.cbCasoCarga, null);
            }

            Valor.Focus();
        }

        private void checkBox1_CheckedChanged(object sender, EventArgs e)
        {
            if (chPosRel.Checked)
              label1.Text = "0...1";
            else
                label1.Text = "m";
                
            edD.Focus();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            if (gerenciador.CasosCarga == null)
                gerenciador.CasosCarga = new FCasosCarga(gerenciador);
            gerenciador.CasosCarga.Show();
        }

        private void edD_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Escape)
                this.Close();
        }

        private void Valor_KeyUp(object sender, KeyEventArgs e)
        {
            if ((Valor.Text.Trim() == "-") && (Valor.Text.Trim() == ",") && (Valor.Text.Trim() == "."))
            {
                MessageBox.Show("Valor inválido");
            }
        }

        private void direcao_Enter(object sender, EventArgs e)
        {

        }

        void distribuida()
        {
            pnCargaConcentrada.Visible = false;
            tabControl1.SelectedIndex = 0;
            chPosRel.Visible = false;
            imConcentrada.Visible = false;
            imDistribuida.Top = 61;
            imDistribuida.Visible = true;
            //        lbUnidade.Text = gerenciador.cbUnForca.Text + "/" + gerenciador.cbUnComp.Text;
            lbUnidade.Text = gerenciador.ConfiguracoesPGi.CfgProjeto.unidadesProjeto.res_un_forca.ToString() + "/" + gerenciador.ConfiguracoesPGi.CfgProjeto.unidadesProjeto.res_un_comprimento.ToString();
            gbTipo.Text = "Tipo: Distribuída";
            groupBox1.Enabled = true;

            Valor.Focus();
        }
        void concentrada()
        {
            pnCargaConcentrada.Visible = true;
            tabControl1.SelectedIndex = 1;
            imConcentrada.Top = 61;
            imConcentrada.Visible = true;
            imDistribuida.Visible = false;
            lbUnidade.Text = gerenciador.ConfiguracoesPGi.CfgProjeto.unidadesProjeto.res_un_forca.ToString();
            gbTipo.Text = "Tipo: Concentrada";
            chPosRel.Visible = true;
            rbGlobal.Checked = true;
            groupBox1.Enabled = false;

            Valor.Focus();
        }
        private void tabControl1_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (tabControl1.SelectedIndex == 0)
            {
                distribuida();
            }
            else
            {
                concentrada();
            }
        }

        private void edD_TextChanged(object sender, EventArgs e)
        {

        }

        private void edD_Validated(object sender, EventArgs e)
        {
            double posrel = double.Parse(edD.Text);
            
            if (chPosRel.Checked) 
            if ( posrel > 1 || posrel <0 )
            {
                    edD.Text = "0";
            }
        }
    }
}
