using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace PG
{
    public partial class FCargaNodal : Form
    {
        public FCargaNodal()
        {
            InitializeComponent();
        }

        Gerenciador gerenciador;
        public FCargaNodal(Gerenciador ger, string _tipo, TCargaPontual carP, bool alteracao)
        {
            this.gerenciador = ger;
            InitializeComponent();

            this.cargaPontual = carP;
            this.tipo = _tipo;

            if (alteracao)
            {
                this.Text = "Alterar carregamento";
                alterando = true;
            }
        }
        public TCargaPontual cargaPontual;

        public bool alterando;
        string tipo;

        private void button7_Click(object sender, EventArgs e)
        {
            gerenciador.CargaNodal = null;

            this.Close();
        }
        public int idCasoSelecionado;
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

                if (cbCasos.SelectedIndex == -1)
                {
                    MessageBox.Show("Nenhum caso de carga foi informado.", "Atenção", MessageBoxButtons.OK, MessageBoxIcon.Stop);
                    return;
                }

                if (alterando)
                {
                    int dp = 2;
                    if (rbX.Checked) dp = 0;
                    if (rbY.Checked) dp = 1;
                    if (rbZ.Checked) dp = 2;

                    double valor = conv.forca(System.Convert.ToDouble(Valor.Text), gerenciador.ConfiguracoesPGi.CfgProjeto.unidadesProjeto.res_un_forca.ToString(), und.kN);

                    List<TCargaPontual> cargasRepetidas = gerenciador.formDesenho.CargasPontuais.FindAll(
                        c => c.Dados.valor == valor
                     && c.Dados.idCaso == idCasoSelecionado
                    && c.Dados.tipoForca == true
                    && c.Dados.DirecaoProjecao == dp
                    && Geom.Iguais(c.pIni.x, this.cargaPontual.pIni.x)
                    && Geom.Iguais(c.pIni.y, this.cargaPontual.pIni.y)
                    && Geom.Iguais(c.pIni.z, this.cargaPontual.pIni.z)
                    && c.ID != this.cargaPontual.ID);

                    if (cargasRepetidas.Count > 0)
                    {
                        MessageBox.Show("A carga pontual de " + cargasRepetidas[0].Dados.valor.ToString("n2") + " kN em (x: " + this.cargaPontual.pIni.x.ToString("n2") + " y: " + (this.cargaPontual.pIni.y * -1).ToString("n2") + " z: " + (this.cargaPontual.pIni.z * -1).ToString("n2") + ") \r já foi inserida para esse mesmo caso de carga!", "Erro", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        return;
                    }

                    this.cargaPontual.Dados.valor = valor;
                    this.cargaPontual.Dados.idCaso = idCasoSelecionado;


                    this.cargaPontual.Dados.DirecaoProjecao = dp;
                    this.cargaPontual.Dados.tipoForca = rbForca.Checked;
                    this.cargaPontual.Dados.tipoMomento = rbMomento.Checked;

                    this.cargaPontual.Dados.cor = gerenciador.formDesenho.CasosCarga.Find(o => o.ID == idCasoSelecionado).Cor;
                    this.cargaPontual.CriaSeta(true, gerenciador.formDesenho.EscalaCargas);

                    this.DialogResult = System.Windows.Forms.DialogResult.Yes;
                }
                else
                    gerenciador.NovosDadosDeCargaNodal(this.tipo);

                gerenciador.formDesenho.Alterou(true);
                gerenciador.formDesenho.MostrarCargasPeloCaso();

            }
            catch (Exception er)
            {
                MessageBox.Show("Inserção inválida em algum campo da tela!  " + er.Message);
            }
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

        private void button1_Click(object sender, EventArgs e)
        {
            if (gerenciador.CasosCarga == null)
                gerenciador.CasosCarga = new FCasosCarga(gerenciador);
            gerenciador.CasosCarga.Show();
        }
        void Carregar()
        {
            if (this.cargaPontual != null)
            {
                double forca = conv.forca(cargaPontual.Dados.valor, und.kN, gerenciador.ConfiguracoesPGi.CfgProjeto.unidadesProjeto.res_un_forca.ToString());

                Valor.Text = forca.ToString("n" + gerenciador.ConfiguracoesPGi.CfgProjeto.unidadesProjeto.res_casas.ToString());
                if (cargaPontual.Dados.DirecaoProjecao == 0) rbX.Checked = true;
                if (cargaPontual.Dados.DirecaoProjecao == 1) rbY.Checked = true;
                if (cargaPontual.Dados.DirecaoProjecao == 2) rbZ.Checked = true;
                idCasoSelecionado = cargaPontual.Dados.idCaso;
                if (cargaPontual.Dados.tipoForca) rbForca.Checked = true;
                if (cargaPontual.Dados.tipoMomento) rbMomento.Checked = true;

                for (int i = 0; i < gerenciador.formDesenho.CasosCarga.Count; i++)
                {
                    if (gerenciador.formDesenho.CasosCarga[i].ID == cargaPontual.Dados.idCaso)
                      cbCasos.SelectedIndex = i-1;
                }
            }
            gerenciador.AtualizaDesenho();
        }

        public void GravaDados()
        {
            if (this.cargaPontual != null)
                this.cargaPontual.Dados.valor = System.Convert.ToDouble(Valor.Text);
        }

        private void FCargaNodal_Load(object sender, EventArgs e)
        {
            cbCasos.Items.Clear();

            foreach (TCasosCarga c in gerenciador.formDesenho.CasosCarga)
                if (c.ID != 1)
                    cbCasos.Items.Add(c.Nome);

            if (cbCasos.Items.Count > 0)
            {
                if ((gerenciador.cbCasoCarga.SelectedIndex != 0) /*TODOS*/ &&
                    (gerenciador.cbCasoCarga.SelectedIndex != 1) /*peso proprio*/ &&
                    (gerenciador.cbCasoCarga.SelectedIndex != gerenciador.cbCasoCarga.Items.Count - 1) /*Nenhum*/)
                    cbCasos.SelectedIndex = gerenciador.cbCasoCarga.SelectedIndex - 2;
                else
                    cbCasos.SelectedIndex = -1;
            }

            if (alterando)
              Carregar();
            lbUnidade.Text = gerenciador.ConfiguracoesPGi.CfgProjeto.unidadesProjeto.res_un_forca.ToString();

            this.Left = 25;
            this.Top = 200;

            Valor.Focus();
        }

        private void FCargaNodal_Move(object sender, EventArgs e)
        {
            gerenciador.AtualizaDesenho();
        
        }

        private void cbCasos_SelectedIndexChanged(object sender, EventArgs e)
        {
              pnCorCaso.BackColor = gerenciador.formDesenho.CasosCarga[cbCasos.SelectedIndex+1].Cor;
              if (gerenciador.cbCasoCarga.SelectedIndex != gerenciador.cbCasoCarga.Items.Count - 1)
              {
                  gerenciador.cbCasoCarga.SelectedIndex = cbCasos.SelectedIndex + 2;
                  gerenciador.cbCargas_SelectedIndexChanged(gerenciador.cbCasoCarga, null);

                  idCasoSelecionado = gerenciador.formDesenho.CasosCarga[gerenciador.cbCasoCarga.SelectedIndex - 1].ID;
            }
            Valor.Focus();
        }

        private void Valor_KeyPress(object sender, KeyPressEventArgs e)
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

        private void Valor_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Escape)
                this.Close(); 
            
            if (e.KeyCode == Keys.Enter)
            {
                button8_Click(button8, null);
            }
        }

        private void FCargaNodal_FormClosed(object sender, FormClosedEventArgs e)
        {
            if (!alterando)
            {
                gerenciador.CargaNodal = null;
                gerenciador.formDesenho.CancelaInsercoes();
            }
        }

        private void rbMomento_CheckedChanged(object sender, EventArgs e)
        {
            //lbUnidade.Text = "tf.m";
            lbUnidade.Text = gerenciador.ConfiguracoesPGi.CfgProjeto.unidadesProjeto.res_un_forca.ToString()+"."+ gerenciador.ConfiguracoesPGi.CfgProjeto.unidadesProjeto.res_un_comprimento.ToString();
        }

        private void rbForca_CheckedChanged(object sender, EventArgs e)
        {
          //  lbUnidade.Text = "tf";
            lbUnidade.Text = gerenciador.ConfiguracoesPGi.CfgProjeto.unidadesProjeto.res_un_forca.ToString();
        }
    }
}
