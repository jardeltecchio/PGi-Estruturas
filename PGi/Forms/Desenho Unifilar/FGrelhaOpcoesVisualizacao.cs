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
    public struct SGrelhaOpcoesVisualizacao
    {
        public bool Diagramas_MostrarBarrasLaje,
                    Diagramas_MostrarBarrasViga,
                    Diagramas_MostrarBarrasRigidas,
                    Diagramas_Gradiente,
                    Diagramas_Arestas,
                    Diagramas_ApenasBarras,
                    Diagramas_DuasCores,
                    Diagramas_DirecaoX,
                    Diagramas_DirecaoY,
                    BarrasLajeVisiveis,
                    Suavizar,
                    Diagramas_DeslocamentoGradiente;

        public SGrelhaOpcoesVisualizacao(bool padrao = true) 
        {
            Diagramas_MostrarBarrasLaje    = true;
            Diagramas_MostrarBarrasViga    = false;
            Diagramas_MostrarBarrasRigidas = false;
            BarrasLajeVisiveis = true;

            Diagramas_Gradiente    = true;
            Diagramas_Arestas      = true;
            Diagramas_ApenasBarras = false;
            Diagramas_DuasCores    = false;
            Diagramas_DirecaoX     = true;
            Diagramas_DirecaoY     = true;
            Suavizar               = false;
      
            Diagramas_DeslocamentoGradiente = true;
        }
    }

    public partial class FGrelhaOpcoesVisualizacao : Form
    {
        Gerenciador gerenciador;

        void LerDados()
        {
            rbCorEsforcoArestas.Checked    = gerenciador.ConfiguracoesPGi.GrelhaOpcoesVisualizacao.Diagramas_Arestas;
            rbCorEsforcoDuasCores.Checked  = gerenciador.ConfiguracoesPGi.GrelhaOpcoesVisualizacao.Diagramas_DuasCores;
            rbCorEsforcoGradiente.Checked  = gerenciador.ConfiguracoesPGi.GrelhaOpcoesVisualizacao.Diagramas_Gradiente;

            chMostrarBarrasRigidas.Checked    = gerenciador.ConfiguracoesPGi.GrelhaOpcoesVisualizacao.Diagramas_MostrarBarrasRigidas;
            chMostrarBarrasLaje.Checked       = gerenciador.ConfiguracoesPGi.GrelhaOpcoesVisualizacao.Diagramas_MostrarBarrasLaje;
            chBarrasLajeVisiveis.Checked      = gerenciador.ConfiguracoesPGi.GrelhaOpcoesVisualizacao.BarrasLajeVisiveis;
            chMostrarBarrasViga.Checked       = gerenciador.ConfiguracoesPGi.GrelhaOpcoesVisualizacao.Diagramas_MostrarBarrasViga;
            chMostrarDiagramaDirecaoX.Checked = gerenciador.ConfiguracoesPGi.GrelhaOpcoesVisualizacao.Diagramas_DirecaoX;
            chMostrarDiagramaDirecaoY.Checked = gerenciador.ConfiguracoesPGi.GrelhaOpcoesVisualizacao.Diagramas_DirecaoY;
            chSuavizar.Checked                = gerenciador.ConfiguracoesPGi.GrelhaOpcoesVisualizacao.Suavizar;

            rbCorDeslocamentoGradiente.Checked = gerenciador.ConfiguracoesPGi.GrelhaOpcoesVisualizacao.Diagramas_DeslocamentoGradiente;
            rbCorDeslocamentoNormal.Checked    = !rbCorDeslocamentoGradiente.Checked;
        }

        void GravarDados()
        {
            gerenciador.ConfiguracoesPGi.GrelhaOpcoesVisualizacao.Diagramas_Arestas               = rbCorEsforcoArestas.Checked;
            gerenciador.ConfiguracoesPGi.GrelhaOpcoesVisualizacao.Diagramas_DuasCores             = rbCorEsforcoDuasCores.Checked;
            gerenciador.ConfiguracoesPGi.GrelhaOpcoesVisualizacao.Diagramas_Gradiente             = rbCorEsforcoGradiente.Checked;
            gerenciador.ConfiguracoesPGi.GrelhaOpcoesVisualizacao.Diagramas_DeslocamentoGradiente = rbCorDeslocamentoGradiente.Checked;
            gerenciador.ConfiguracoesPGi.GrelhaOpcoesVisualizacao.Suavizar                        = chSuavizar.Checked;
            gerenciador.ConfiguracoesPGi.GrelhaOpcoesVisualizacao.Diagramas_MostrarBarrasLaje = chMostrarBarrasLaje.Checked;
            gerenciador.ConfiguracoesPGi.GrelhaOpcoesVisualizacao.BarrasLajeVisiveis = chBarrasLajeVisiveis.Checked;
            gerenciador.ConfiguracoesPGi.GrelhaOpcoesVisualizacao.Diagramas_MostrarBarrasViga = chMostrarBarrasViga.Checked;
            gerenciador.ConfiguracoesPGi.GrelhaOpcoesVisualizacao.Diagramas_MostrarBarrasRigidas = chMostrarBarrasRigidas.Checked;
            gerenciador.ConfiguracoesPGi.GrelhaOpcoesVisualizacao.Diagramas_DirecaoX = chMostrarDiagramaDirecaoX.Checked;
            gerenciador.ConfiguracoesPGi.GrelhaOpcoesVisualizacao.Diagramas_DirecaoY          = chMostrarDiagramaDirecaoY.Checked;
        }

        public FGrelhaOpcoesVisualizacao()
        {
            InitializeComponent();
        }

        public FGrelhaOpcoesVisualizacao(Gerenciador frmPai)
        {
            InitializeComponent();
            gerenciador = frmPai;
        }


        private void button3_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            GravarDados();
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
    }
}
