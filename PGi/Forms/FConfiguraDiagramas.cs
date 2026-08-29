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
    [Serializable]
    public struct SDiagramaOpcoesVisualizacao
    {
        public int textos_casadecimal, alinhamento;
        public double percentual_maximo;
        public bool exibir_tudo, exibir_percentual_maximo, linha_gradiente, linhas,gradiente,contorno, legenda;

        public Color CorLinha, CorValor;
        public SDiagramaOpcoesVisualizacao(bool padrao = true)
        {
            legenda = false;
            textos_casadecimal = 2;
            percentual_maximo = 90;
            gradiente = true;
            contorno = false;
            linhas = false;
            CorLinha = Color.Black;
            CorValor = Color.Gray;
            exibir_tudo = true;
            exibir_percentual_maximo = false;
            linha_gradiente = true;
            alinhamento = 0;
        }
    }
    public partial class FConfiguraDiagramas : Form
    {
        public FConfiguraDiagramas()
        {
            InitializeComponent();
        }
        void LerDados()
        {
            rbLinhas.Checked = gerenciador.ConfiguracoesPGi.DiagramaOpcoesVisualizacao.linhas;
            rbGradiente.Checked = gerenciador.ConfiguracoesPGi.DiagramaOpcoesVisualizacao.gradiente;
            chLegenda.Checked = gerenciador.ConfiguracoesPGi.DiagramaOpcoesVisualizacao.legenda;
            chLinhaGradiente.Checked = gerenciador.ConfiguracoesPGi.DiagramaOpcoesVisualizacao.linha_gradiente;
            chContorno.Checked = gerenciador.ConfiguracoesPGi.DiagramaOpcoesVisualizacao.contorno;
            btCorLinha.BackColor = gerenciador.ConfiguracoesPGi.DiagramaOpcoesVisualizacao.CorLinha;
            btCorValor.BackColor = gerenciador.ConfiguracoesPGi.DiagramaOpcoesVisualizacao.CorValor;
            percentualValor.Text = gerenciador.ConfiguracoesPGi.DiagramaOpcoesVisualizacao.percentual_maximo.ToString("N2");
            rbExibirTudo.Checked = gerenciador.ConfiguracoesPGi.DiagramaOpcoesVisualizacao.exibir_tudo;
            rbExibirPerc.Checked = gerenciador.ConfiguracoesPGi.DiagramaOpcoesVisualizacao.exibir_percentual_maximo;
            cbAlinhamentoDiagrama.SelectedIndex = gerenciador.ConfiguracoesPGi.DiagramaOpcoesVisualizacao.alinhamento;
        }

        void GravarDados()
        {
            gerenciador.ConfiguracoesPGi.DiagramaOpcoesVisualizacao.alinhamento = cbAlinhamentoDiagrama.SelectedIndex;
            gerenciador.ConfiguracoesPGi.DiagramaOpcoesVisualizacao.linhas = rbLinhas.Checked;
            gerenciador.ConfiguracoesPGi.DiagramaOpcoesVisualizacao.legenda = chLegenda.Checked;
            gerenciador.ConfiguracoesPGi.DiagramaOpcoesVisualizacao.gradiente = rbGradiente.Checked;
            gerenciador.ConfiguracoesPGi.DiagramaOpcoesVisualizacao.contorno  = chContorno.Checked;
            gerenciador.ConfiguracoesPGi.DiagramaOpcoesVisualizacao.CorLinha = btCorLinha.BackColor;
            gerenciador.ConfiguracoesPGi.DiagramaOpcoesVisualizacao.CorValor = btCorValor.BackColor;
            gerenciador.ConfiguracoesPGi.DiagramaOpcoesVisualizacao.linha_gradiente = chLinhaGradiente.Checked;

            gerenciador.ConfiguracoesPGi.DiagramaOpcoesVisualizacao.percentual_maximo = System.Convert.ToDouble(percentualValor.Text);
            gerenciador.ConfiguracoesPGi.DiagramaOpcoesVisualizacao.exibir_tudo = rbExibirTudo.Checked;
            gerenciador.ConfiguracoesPGi.DiagramaOpcoesVisualizacao.exibir_percentual_maximo = rbExibirPerc.Checked;
        }

        public FConfiguraDiagramas(Gerenciador _gerenciador)
        {
            this.gerenciador = _gerenciador;
            InitializeComponent();
            ShowInTaskbar = false;
        }
        Gerenciador gerenciador;
        private void button3_Click(object sender, EventArgs e)
        {
            Close();
        }

        private void FConfiguraDiagramas_Move(object sender, EventArgs e)
        {
            gerenciador.AtualizaDesenho();
        }

        private void FConfiguraDiagramas_FormClosed(object sender, FormClosedEventArgs e)
        {
            gerenciador.fConfiguraDiagramas = null;
        }

        private void button1_Click(object sender, EventArgs e)
        {
            GravarDados();
            gerenciador.Preenche_barras_valores_esforco();

            if (gerenciador.formDesenho.MostraTextoDiagramas)
            {
              //  if (gerenciador.formDesenho.fx || gerenciador.formDesenho.mx)
             //       gerenciador.formDesenho.CriarTextosAxial_Torcor();
             //   else
                    gerenciador.formDesenho.CriarTextosEsforco();
            }

            gerenciador.formDesenho.AtualizaConfiguracaoDiagramas();

            gerenciador.formDesenho.AtualizarDesenho();
            gerenciador.formDesenho.glControl.SwapBuffers();

            this.DialogResult = System.Windows.Forms.DialogResult.Yes;
        }

        private void percentualValor_KeyPress(object sender, KeyPressEventArgs e)
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

        private void FConfiguraDiagramas_Load(object sender, EventArgs e)
        {
            LerDados();
        }

        private void btCor_Click_1(object sender, EventArgs e)
        {

        }

        private void button2_Click(object sender, EventArgs e)
        {

        }

        private void rbExibirPerc_CheckedChanged(object sender, EventArgs e)
        {
            percentualValor.Enabled = true;
        }

        private void rbExibirMaximo_CheckedChanged(object sender, EventArgs e)
        {
            percentualValor.Enabled = false;
        }

        private void btCorLinha_Click(object sender, EventArgs e)
        {

        }

        private void btCorValor_Click(object sender, EventArgs e)
        {
            if (colorDialog1.ShowDialog() == DialogResult.OK)
            {
                btCorValor.BackColor = colorDialog1.Color;
            }
        }

        private void btCorLinha_Click_1(object sender, EventArgs e)
        {
            if (colorDialog1.ShowDialog() == DialogResult.OK)
            {
                btCorLinha.BackColor = colorDialog1.Color;
            }
        }

        private void rbGradiente_CheckedChanged(object sender, EventArgs e)
        {
            chLinhaGradiente.Enabled = true;
            
            chLinhaGradiente.Checked = true;
        }

        private void rbLinhas_CheckedChanged(object sender, EventArgs e)
        {

            chLinhaGradiente.Checked = false;
            chLinhaGradiente.Enabled = false;
        }

        private void panel1_Paint(object sender, PaintEventArgs e)
        {

        }
    }
}
