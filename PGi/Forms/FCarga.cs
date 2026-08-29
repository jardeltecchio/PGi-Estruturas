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
    public partial class FCarga : Form
    {
        TCargaPontual cargaPontual;
        TCargaLinear cargaLinear;
        public bool alterando;
        string tipo;
        public FCarga()
        {
            InitializeComponent();
        }

        Gerenciador gerenciador;
        public FCarga(Gerenciador ger, string _tipo, TCargaPontual carP, TCargaLinear carL)
        {
            this.gerenciador = ger;
            InitializeComponent();

            this.cargaPontual = carP;
            this.cargaLinear = carL;
            this.tipo = _tipo;

            //lbUnidade.Text = gerenciador.cbUnForca.Text + "/" + gerenciador.cbUnComp.Text;
            cbDirecao.SelectedIndex = 0;
        }

        void Carregar()
        {
            if (this.cargaLinear != null)
            {
                Valor.Text = cargaLinear.Dados.valor.ToString();
                if (cargaLinear.Dados.DirecaoProjecao == 0) rbX.Checked = true;
                if (cargaLinear.Dados.DirecaoProjecao == 1) rbY.Checked = true;
                if (cargaLinear.Dados.DirecaoProjecao == 2) rbZ.Checked = true;
                cbDirecao.SelectedIndex = cargaLinear.Dados.ProjecaoGlobal;
                edD.Text = cargaLinear.Dados.d.ToString("n2");
                chPosRel.Checked = cargaLinear.Dados.posicaoRelativa;

                for (int i = 0; i < gerenciador.formDesenho.CasosCarga.Count; i++)
                {
                    if (gerenciador.formDesenho.CasosCarga[i].Id == cargaLinear.Dados.idCaso)
                        cbCasos.SelectedIndex = i;
                }

                if (cargaLinear.Dados.distribuida)
                    btDistribuida_Click(btDistribuida,null);
                else                    
                if (cargaLinear.Dados.concentrada)
                    btConcentrada_Click(btConcentrada, null);
            }

        }
        public void GravaDados()
        {
            if (this.cargaLinear != null)
                this.cargaLinear.Dados.valor = System.Convert.ToDouble(Valor.Text);
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

                if (cbCasos.SelectedIndex == -1)
                {
                    MessageBox.Show("Nenhum caso de carga cadastrado", "Atenção", MessageBoxButtons.OK, MessageBoxIcon.Stop);
                    return;
                }

                if (alterando)
                {
                    this.cargaLinear.Dados.valor = System.Convert.ToDouble(Valor.Text);
                    this.cargaLinear.Dados.d = System.Convert.ToDouble(edD.Text);
                    this.cargaLinear.Dados.idCaso = gerenciador.formDesenho.CasosCarga[cbCasos.SelectedIndex].Id;

                    this.cargaLinear.Dados.ProjecaoGlobal = cbDirecao.SelectedIndex;
                    this.cargaLinear.Dados.concentrada = pnCargaConcentrada.Visible;
                    this.cargaLinear.Dados.distribuida = !pnCargaConcentrada.Visible;
                    this.cargaLinear.Dados.posicaoRelativa = chPosRel.Checked;
                    int dp = 2;
                    if (rbX.Checked) dp = 0;
                    if (rbY.Checked) dp = 1;
                    if (rbZ.Checked) dp = 2;
                    this.cargaLinear.Dados.DirecaoProjecao = dp;
                    this.cargaLinear.Dados.cor = gerenciador.formDesenho.CasosCarga[cbCasos.SelectedIndex].Cor_;
                    this.cargaLinear.RotacionaDiagramaCarga(true,0,0);

                    this.DialogResult = System.Windows.Forms.DialogResult.Yes;
                }
                else
                    gerenciador.NovosDadosDeCarga(this.tipo);

                gerenciador.formDesenho.MostraCargasPeloCaso();

            }
            catch(Exception er )
            {
                MessageBox.Show("Inserção inválida em algum campo da tela!  " + er.Message);
            }
        }

        private void RigidezGJ_Leave(object sender, EventArgs e)
        {
            if ((sender as TextBox).Text.Trim() != "")
                (sender as TextBox).Text = double.Parse((sender as TextBox).Text).ToString("n2");
        }

        private void RigidezGJ_KeyPress(object sender, KeyPressEventArgs e)
        {
         //   e.Handled = (!char.IsDigit(e.KeyChar) && !char.IsControl(e.KeyChar));
        }
        public void AtualizaCasos()
        {
            cbCasos.Items.Clear();
            gerenciador.cbCasoCarga.Items.Clear();
            gerenciador.cbCasoCarga.Items.Add("<Nenhum>");
            foreach (TCasosCarga c in gerenciador.formDesenho.CasosCarga)
            {
                cbCasos.Items.Add(c.Nome);
                gerenciador.cbCasoCarga.Items.Add(c.Nome);
            }
        }

        private void FCarga_Load(object sender, EventArgs e)
        {
            cbCasos.Items.Clear();

            foreach (TCasosCarga c in gerenciador.formDesenho.CasosCarga)
               cbCasos.Items.Add(c.Nome);

            if (cbCasos.Items.Count > 0)
              cbCasos.SelectedIndex = 0;

            if (alterando)
                Carregar();
            else
            {
                btDistribuida_Click(btDistribuida, null);
            }

            this.Left = 25;
            Valor.Focus();
        }

        private void FCarga_Move(object sender, EventArgs e)
        {
            gerenciador.AtualizaDesenho();
        
        }

        private void btConcentrada_Click(object sender, EventArgs e)
        {
            pnCargaConcentrada.Visible = true;
            imConcentrada.Top = 61;
            imConcentrada.Visible = true;
            imDistribuida.Visible = false;
            lbUnidade.Text = "tf";
            gbTipo.Text = "Tipo: Concentrada";
            chPosRel.Visible = true;
            Valor.Focus();
        }

        private void btDistribuida_Click(object sender, EventArgs e)
        {
            pnCargaConcentrada.Visible = false;
            chPosRel.Visible = false;
            imConcentrada.Visible = false;
            imDistribuida.Top = 61;
            imDistribuida.Visible = true;
    //        lbUnidade.Text = gerenciador.cbUnForca.Text + "/" + gerenciador.cbUnComp.Text;
            lbUnidade.Text = "tf/m";
            gbTipo.Text = "Tipo: Distribuída";
            Valor.Focus();
        }

        private void Valor_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (!char.IsControl(e.KeyChar) && !char.IsDigit(e.KeyChar) && (e.KeyChar != '.') && (e.KeyChar != '-'))
            {
                e.Handled = true;
            }
            if (!char.IsControl(e.KeyChar) && !char.IsDigit(e.KeyChar) && (e.KeyChar != '.') && (e.KeyChar != '-'))
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
        }

        private void FCarga_FormClosed(object sender, FormClosedEventArgs e)
        {
                gerenciador.CargaBarra = null;
                gerenciador.formDesenho.CancelaInsercoes();
        }

        private void edD_Leave(object sender, EventArgs e)
        {
            if ((sender as TextBox).Text.Trim() != "")
                (sender as TextBox).Text = double.Parse((sender as TextBox).Text).ToString("n2");
        }
        public int idCaso;
        private void cbCasos_SelectedIndexChanged(object sender, EventArgs e)
        {
            pnCorCaso.BackColor = gerenciador.formDesenho.CasosCarga[cbCasos.SelectedIndex].Cor_;
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
    }
}
