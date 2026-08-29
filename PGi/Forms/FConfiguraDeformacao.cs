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
    public struct SPorticoOpcoesVisualizacao
    {
        public int textos_casadecimal, valores;
        public double percentual_maximo_deformacao;
        public bool Diagramas_MostrarBarrasPilar,
                    Diagramas_MostrarBarrasViga,
                    Diagramas_MostrarBarrasRigidas,
                    Diagramas_Gradiente,
                    Diagramas_Arestas,
                    exibir_percentual_maximo_deformacao,
                    exibir_somente_maximo, exibir_tudo,
                    Diagramas_ApenasBarras,
                    Diagramas_DuasCores,
                    Suavizar,
                    Diagramas_DeslocamentoGradiente,
                    MostrarNos,
                    Perspectiva,
                    MostrarCargasPontuais,
                    MostrarLinhasContorno,
                    
                    MostrarCargasLineares;
        public Color CorValor;

        public SPorticoOpcoesVisualizacao(bool padrao = true) 
        {
            percentual_maximo_deformacao = 90;
            exibir_percentual_maximo_deformacao = true;
            exibir_somente_maximo = false;
            textos_casadecimal = 2;
            exibir_tudo = false;
            Diagramas_MostrarBarrasPilar    = true;
            Diagramas_MostrarBarrasViga    = true;
            Diagramas_MostrarBarrasRigidas = false;
            Perspectiva = false;
            Diagramas_Gradiente    = true;
            Diagramas_Arestas      = true;
            Diagramas_ApenasBarras = false;
            Diagramas_DuasCores    = false;
            Suavizar               = false;
            MostrarNos             = false;
            MostrarCargasLineares = false;
            MostrarCargasPontuais = false;
            MostrarLinhasContorno = false;
            Diagramas_DeslocamentoGradiente = true;
            valores = 7;
            CorValor = Color.Gray;
        }
    }

    public partial class FConfiguraDeformacao : Form
    {
        Gerenciador gerenciador;

        void LerDados()
        {
            rbCorEsforcoArestas.Checked    = gerenciador.ConfiguracoesPGi.PorticoOpcoesVisualizacao.Diagramas_Arestas;
            rbCorEsforcoDuasCores.Checked  = gerenciador.ConfiguracoesPGi.PorticoOpcoesVisualizacao.Diagramas_DuasCores;
            rbCorEsforcoGradiente.Checked  = gerenciador.ConfiguracoesPGi.PorticoOpcoesVisualizacao.Diagramas_Gradiente;
            chMostrarNos.Checked = gerenciador.ConfiguracoesPGi.PorticoOpcoesVisualizacao.MostrarNos;
            chMostrarBarrasRigidas.Checked = gerenciador.ConfiguracoesPGi.PorticoOpcoesVisualizacao.Diagramas_MostrarBarrasRigidas;
            chMostrarBarrasPilar.Checked = gerenciador.ConfiguracoesPGi.PorticoOpcoesVisualizacao.Diagramas_MostrarBarrasPilar;
            chMostrarBarrasViga.Checked = gerenciador.ConfiguracoesPGi.PorticoOpcoesVisualizacao.Diagramas_MostrarBarrasViga;
            chSuavizar.Checked = gerenciador.ConfiguracoesPGi.PorticoOpcoesVisualizacao.Suavizar;
            chMostrarCargasLineares.Checked = gerenciador.ConfiguracoesPGi.PorticoOpcoesVisualizacao.MostrarCargasLineares;
            chMostrarCargasPontuais.Checked = gerenciador.ConfiguracoesPGi.PorticoOpcoesVisualizacao.MostrarCargasPontuais;
            chPerspectiva.Checked = gerenciador.ConfiguracoesPGi.PorticoOpcoesVisualizacao.Perspectiva;
            rbCorDeslocamentoGradiente.Checked = gerenciador.ConfiguracoesPGi.PorticoOpcoesVisualizacao.Diagramas_DeslocamentoGradiente;
            mostralinhasdecontorno.Checked = gerenciador.ConfiguracoesPGi.PorticoOpcoesVisualizacao.MostrarLinhasContorno;
            rbCorDeslocamentoNormal.Checked = !rbCorDeslocamentoGradiente.Checked;
            percentualValor.Text = gerenciador.ConfiguracoesPGi.PorticoOpcoesVisualizacao.percentual_maximo_deformacao.ToString("N2");
            rbExibirMaximo.Checked = gerenciador.ConfiguracoesPGi.PorticoOpcoesVisualizacao.exibir_somente_maximo;
            rbExibirPerc.Checked = gerenciador.ConfiguracoesPGi.PorticoOpcoesVisualizacao.exibir_percentual_maximo_deformacao;
            rbExibirTudo.Checked = gerenciador.ConfiguracoesPGi.PorticoOpcoesVisualizacao.exibir_tudo;
       
            btCorValor.BackColor = gerenciador.ConfiguracoesPGi.PorticoOpcoesVisualizacao.CorValor;

            if (gerenciador.ConfiguracoesPGi.PorticoOpcoesVisualizacao.valores == 1)
                cbValores.SelectedIndex = 0;
            if (gerenciador.ConfiguracoesPGi.PorticoOpcoesVisualizacao.valores == 3)
                cbValores.SelectedIndex = 1;
            if (gerenciador.ConfiguracoesPGi.PorticoOpcoesVisualizacao.valores == 2)
                cbValores.SelectedIndex = 2;

            if (gerenciador.ConfiguracoesPGi.PorticoOpcoesVisualizacao.valores == 4)
                cbValores.SelectedIndex = 3;
            if (gerenciador.ConfiguracoesPGi.PorticoOpcoesVisualizacao.valores == 6)
                cbValores.SelectedIndex = 4;
            if (gerenciador.ConfiguracoesPGi.PorticoOpcoesVisualizacao.valores == 5)
                cbValores.SelectedIndex = 5;

            if (gerenciador.ConfiguracoesPGi.PorticoOpcoesVisualizacao.valores == 7)
                cbValores.SelectedIndex = 6;
        }

        void GravarDados()
        {
            gerenciador.ConfiguracoesPGi.PorticoOpcoesVisualizacao.Diagramas_Arestas = rbCorEsforcoArestas.Checked;
            gerenciador.ConfiguracoesPGi.PorticoOpcoesVisualizacao.Diagramas_DuasCores = rbCorEsforcoDuasCores.Checked;
            gerenciador.ConfiguracoesPGi.PorticoOpcoesVisualizacao.Diagramas_Gradiente = rbCorEsforcoGradiente.Checked;
            gerenciador.ConfiguracoesPGi.PorticoOpcoesVisualizacao.Diagramas_DeslocamentoGradiente = rbCorDeslocamentoGradiente.Checked;
            gerenciador.ConfiguracoesPGi.PorticoOpcoesVisualizacao.Suavizar = chSuavizar.Checked;
            gerenciador.ConfiguracoesPGi.PorticoOpcoesVisualizacao.Diagramas_MostrarBarrasPilar = chMostrarBarrasPilar.Checked;
            gerenciador.ConfiguracoesPGi.PorticoOpcoesVisualizacao.Diagramas_MostrarBarrasViga = chMostrarBarrasViga.Checked;
            gerenciador.ConfiguracoesPGi.PorticoOpcoesVisualizacao.Diagramas_MostrarBarrasRigidas = chMostrarBarrasRigidas.Checked;
            gerenciador.ConfiguracoesPGi.PorticoOpcoesVisualizacao.MostrarNos                     = chMostrarNos.Checked;
            gerenciador.ConfiguracoesPGi.PorticoOpcoesVisualizacao.MostrarCargasLineares = chMostrarCargasLineares.Checked;
            gerenciador.ConfiguracoesPGi.PorticoOpcoesVisualizacao.MostrarCargasPontuais = chMostrarCargasPontuais.Checked;
            gerenciador.ConfiguracoesPGi.PorticoOpcoesVisualizacao.Perspectiva  = chPerspectiva.Checked;
            gerenciador.ConfiguracoesPGi.PorticoOpcoesVisualizacao.MostrarLinhasContorno = mostralinhasdecontorno.Checked;

            gerenciador.ConfiguracoesPGi.PorticoOpcoesVisualizacao.exibir_tudo = rbExibirTudo.Checked;
            gerenciador.ConfiguracoesPGi.PorticoOpcoesVisualizacao.exibir_somente_maximo = rbExibirMaximo.Checked;
            gerenciador.ConfiguracoesPGi.PorticoOpcoesVisualizacao.exibir_percentual_maximo_deformacao = rbExibirPerc.Checked;
            gerenciador.ConfiguracoesPGi.PorticoOpcoesVisualizacao.CorValor = btCorValor.BackColor;

/*Dx global
Dy global
Dz global
Rx global
Ry global
Rz global
U Total*/

            if (cbValores.SelectedIndex == 0)
              gerenciador.ConfiguracoesPGi.PorticoOpcoesVisualizacao.valores = 1;
            if (cbValores.SelectedIndex == 1)
                gerenciador.ConfiguracoesPGi.PorticoOpcoesVisualizacao.valores = 3;
            if (cbValores.SelectedIndex == 2)
                gerenciador.ConfiguracoesPGi.PorticoOpcoesVisualizacao.valores = 2;

            if (cbValores.SelectedIndex == 3)
                gerenciador.ConfiguracoesPGi.PorticoOpcoesVisualizacao.valores = 4;
            if (cbValores.SelectedIndex == 4)
                gerenciador.ConfiguracoesPGi.PorticoOpcoesVisualizacao.valores = 6;
            if (cbValores.SelectedIndex == 5)
                gerenciador.ConfiguracoesPGi.PorticoOpcoesVisualizacao.valores = 5;

            if (cbValores.SelectedIndex == 6)
                gerenciador.ConfiguracoesPGi.PorticoOpcoesVisualizacao.valores = 7;

            gerenciador.ConfiguracoesPGi.PorticoOpcoesVisualizacao.percentual_maximo_deformacao = System.Convert.ToDouble(percentualValor.Text);
        }

        public FConfiguraDeformacao()
        {
            InitializeComponent();
        }

        public FConfiguraDeformacao(Gerenciador frmPai)
        {
            InitializeComponent();
            gerenciador = frmPai;
            ShowInTaskbar = false;
        }


        private void button3_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            GravarDados();
            gerenciador.formDesenho.AtualizaConfiguracoes3D();
            gerenciador.Preenche_nos_valores_deformacao();
     //       gerenciador.FVisGrelha.AtualizaEsforcos();
            this.DialogResult = System.Windows.Forms.DialogResult.Yes;
        }

        private void FGrelhaOpcoesVisualizacao_Load(object sender, EventArgs e)
        {
            LerDados();
        }

        private void pictureBox4_MouseClick(object sender, MouseEventArgs e)
        {
            rbCorDeslocamentoNormal.Checked = true;
        }

        private void pictureBox3_MouseClick_1(object sender, MouseEventArgs e)
        {
            rbCorEsforcoArestas.Checked = true;
        }

        private void pictureBox1_MouseClick_1(object sender, MouseEventArgs e)
        {
            rbCorEsforcoGradiente.Checked = true;
        }

        private void pictureBox2_MouseClick_1(object sender, MouseEventArgs e)
        {
            rbCorEsforcoDuasCores.Checked = true;
        }

        private void pictureBox6_MouseClick(object sender, MouseEventArgs e)
        {
            rbCorDeslocamentoGradiente.Checked = true;
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

        private void groupBox1_Enter(object sender, EventArgs e)
        {

        }

        private void rbExibirMaximo_CheckedChanged(object sender, EventArgs e)
        {
            percentualValor.Enabled = false;
        }

        private void rbExibirPerc_CheckedChanged(object sender, EventArgs e)
        {
            percentualValor.Enabled = true;

        }

        private void btCorValor_Click(object sender, EventArgs e)
        {
            if (colorDialog1.ShowDialog() == DialogResult.OK)
            {
                btCorValor.BackColor = colorDialog1.Color;
            }
        }
    }
}
