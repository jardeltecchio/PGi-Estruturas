using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using Win32Interop.Enums;

namespace PG
{
    public partial class FConfiguraProjeto : Form
    {
        public FConfiguraProjeto()
        {
            InitializeComponent();
        }
        Gerenciador gerenciador;
        SConfiguracoesProjeto Cfg;
        public FConfiguraProjeto(Gerenciador gerenciador)
        {
            this.gerenciador = gerenciador;
            Cfg = gerenciador.ConfiguracoesPGi.CfgProjeto;
             
            InitializeComponent();
            ShowInTaskbar = false;
        }

        private void button1_Click(object sender, EventArgs e)
        {
            GravaDados();
            Close();
        }


        #region Portico
        void CarregaPortico()
        {
            carregando = true;
            if (Cfg.portico != null)
            {
                if (Cfg.portico.qtdbarrasPortico == 0)
                    Cfg.portico.qtdbarrasPortico = 8;

                porQtdBarrasPortico.Value = Cfg.portico.qtdbarrasPortico;
                porRefinar.Checked = Cfg.portico.refinar;

            }
            carregando = false;
        }
        void GravaPortico()
        {
            if (!carregando)
            {
                if (Cfg.portico == null) 
                    Cfg.portico = new SConfiguracaoPortico(true);

                 Cfg.portico.qtdbarrasPortico = (int)porQtdBarrasPortico.Value;
                Cfg.portico.refinar = porRefinar.Checked;
            }
        }

        #endregion


        #region Sistema
        void CarregaSistema()
        {
            carregando = true;
            if (Cfg.sistema != null)
            {         
              sisSolver_cholesky.Checked = Cfg.sistema.Solver_cholesky_supernodal;
              sisSolver_Cholheskypadrao.Checked = Cfg.sistema.Solver_choleskypadrao;
              sisSolver_cg.Checked = Cfg.sistema.Solver_gradiente_conjugado;
              sisDll.Checked     = Cfg.sistema.UsarDll;

                if (Cfg.sistema.numeroModos == 0) Cfg.sistema.numeroModos = 1;

              numModos.Value = Cfg.sistema.numeroModos;
              chCalcularModos.Checked = Cfg.sistema.CalculaModosVibracao;
              InterromperCalculoConexoesPerdidas.Checked = Cfg.sistema.Interromper_calculo_conexao_perdida;
              InterromperCalculoElementosSobrepostos.Checked = Cfg.sistema.Interromper_calculo_elementos_sobrepostos;
                if (Cfg.sistema.toleranciaConexaoPerdida == 0)
                    Cfg.sistema.toleranciaConexaoPerdida = 20;

              toleranciaInterromperCalculo.Value = (int)Cfg.sistema.toleranciaConexaoPerdida;
            }
            carregando = false;
        }

        void GravaSistema()
        {
            if (!carregando)
            {
                if (Cfg.sistema == null) Cfg.sistema = new SSistema(true);
                Cfg.sistema.Solver_cholesky_supernodal = sisSolver_cholesky.Checked;
                Cfg.sistema.Solver_gradiente_conjugado = sisSolver_cg.Checked;
                Cfg.sistema.Solver_choleskypadrao = sisSolver_Cholheskypadrao.Checked;
                Cfg.sistema.UsarDll = sisDll.Checked;
                Cfg.sistema.CalculaModosVibracao= chCalcularModos.Checked;

                Cfg.sistema.Interromper_calculo_conexao_perdida = InterromperCalculoConexoesPerdidas.Checked;
                Cfg.sistema.Interromper_calculo_elementos_sobrepostos= InterromperCalculoElementosSobrepostos.Checked;
                Cfg.sistema.toleranciaConexaoPerdida = (int)toleranciaInterromperCalculo.Value;
                Cfg.sistema.numeroModos = (int)numModos.Value;
            }
        }

        #endregion

        
        #region Grelha
        void CarregaGrelha()
        {
            carregando = true;
            greedAnguloBarras.Value = gerenciador.Pavimentos[grepisos.SelectedIndex].ConfiguracaGrelha.AnguloBarras;
            greedEspacamentoX.Value = gerenciador.Pavimentos[grepisos.SelectedIndex].ConfiguracaGrelha.EspacamentoX;
            greedEspacamentoY.Value = gerenciador.Pavimentos[grepisos.SelectedIndex].ConfiguracaGrelha.EspacamentoY;
            grerbGerarNovaGrelha.Checked = gerenciador.Pavimentos[grepisos.SelectedIndex].ConfiguracaGrelha.GerarNovaGrelha;
            grerbConsiderarGrelhaEditada.Checked = gerenciador.Pavimentos[grepisos.SelectedIndex].ConfiguracaGrelha.ConsiderarGrelhaEditada;
            carregando = false;
        }

        void GravaGrelha(int piso)
        {
            if (gerenciador.Pavimentos.Count > 0)
            {
                gerenciador.Pavimentos[piso].ConfiguracaGrelha.AnguloBarras = (int)greedAnguloBarras.Value;
                gerenciador.Pavimentos[piso].ConfiguracaGrelha.EspacamentoX = (int)greedEspacamentoX.Value;
                gerenciador.Pavimentos[piso].ConfiguracaGrelha.EspacamentoY = (int)greedEspacamentoY.Value;
                gerenciador.Pavimentos[piso].ConfiguracaGrelha.GerarNovaGrelha = grerbGerarNovaGrelha.Checked;
                gerenciador.Pavimentos[piso].ConfiguracaGrelha.ConsiderarGrelhaEditada = grerbConsiderarGrelhaEditada.Checked;
            }
        }

        private void grerbConsiderarGrelhaEditada_CheckedChanged(object sender, EventArgs e)
        {
            gbDiscretizacao.Enabled = false;
            if (!carregando)
                GravaGrelha(grepisos.SelectedIndex);
        }

        private void grerbGerarNovaGrelha_CheckedChanged(object sender, EventArgs e)
        {
            gbDiscretizacao.Enabled = true;
            if (!carregando)
                GravaGrelha(grepisos.SelectedIndex);
        }
        private void grepisos_Click(object sender, EventArgs e)
        {
            CarregaGrelha();
        }
        bool carregando;

        private void greedEspacamentoX_ValueChanged(object sender, EventArgs e)
        {
        //    if (!carregando)
         //       GravaGrelha(grepisos.SelectedIndex);
        }

        private void greedEspacamentoY_ValueChanged(object sender, EventArgs e)
        {
    //        if (!carregando)
     //           GravaGrelha(grepisos.SelectedIndex);
        }

        private void greedAnguloBarras_ValueChanged(object sender, EventArgs e)
        {
            if (!carregando)
                GravaGrelha(grepisos.SelectedIndex);
        }
        private void button2_Click(object sender, EventArgs e)
        {
            foreach (TPavimento pav in gerenciador.Pavimentos)
            {
                pav.ConfiguracaGrelha.EspacamentoX = (int)greedEspacamentoX.Value;
                pav.ConfiguracaGrelha.EspacamentoY = (int)greedEspacamentoY.Value;
                pav.ConfiguracaGrelha.AnguloBarras = (int)greedAnguloBarras.Value;
            }
        }
        #endregion

        void GravaVisualizacao()
        {
            if (gerenciador.ConfiguracoesPGi.F3DOpcoesVisualizacao.RgbArestas == null)
                gerenciador.ConfiguracoesPGi.F3DOpcoesVisualizacao.RgbArestas = new byte[3];

            gerenciador.ConfiguracoesPGi.F3DOpcoesVisualizacao.MostrarDescricaoElementos = MostrarDescricaoElementos.Checked;
            gerenciador.ConfiguracoesPGi.F3DOpcoesVisualizacao.MostrarNumeroElementos = MostrarNumeroElementos.Checked;
            gerenciador.ConfiguracoesPGi.F3DOpcoesVisualizacao.UsarPlanoFundoGradiente = chUsarPlanoFundoGradiente.Checked;

            gerenciador.ConfiguracoesPGi.F3DOpcoesVisualizacao.Transparencia = (byte)(255 - Transparencia.Value);
            double tamno = (double)TamanhoNo.Value;
            gerenciador.ConfiguracoesPGi.F3DOpcoesVisualizacao.tamanhoNo = (double)tamno;
            gerenciador.ConfiguracoesPGi.F3DOpcoesVisualizacao.Perspectiva = chPerspectiva.Checked;

            gerenciador.ConfiguracoesPGi.F3DOpcoesVisualizacao.EixosCentrais = MostrarEixosCentrais.Checked;
            gerenciador.ConfiguracoesPGi.F3DOpcoesVisualizacao.TamApoios = (int)tamApoio.Value;
            gerenciador.ConfiguracoesPGi.F3DOpcoesVisualizacao.TamArt = (int)tamArt.Value;

            gerenciador.ConfiguracoesPGi.F3DOpcoesVisualizacao.Arestas = Arestas.Checked;
            gerenciador.ConfiguracoesPGi.F3DOpcoesVisualizacao.ArestasConformeObjetos = ArestaConfObjeto.Checked;
            gerenciador.ConfiguracoesPGi.F3DOpcoesVisualizacao.Nos = Nos.Checked;

            gerenciador.ConfiguracoesPGi.F3DOpcoesVisualizacao.MostrarChao = chMostrarChao.Checked;
            gerenciador.ConfiguracoesPGi.F3DOpcoesVisualizacao.MostrarEixosLocais = MostrarEixosLocais.Checked;
            gerenciador.ConfiguracoesPGi.F3DOpcoesVisualizacao.MostrarTexturas = chMostrarTextura.Checked;
            gerenciador.ConfiguracoesPGi.F3DOpcoesVisualizacao.Perspectiva = chPerspectiva.Checked;
            gerenciador.ConfiguracoesPGi.F3DOpcoesVisualizacao.CorPorTipoDeElemento = rbCorPorTipoDeElemento.Checked;
            gerenciador.ConfiguracoesPGi.F3DOpcoesVisualizacao.CorPorMaterial = rbCorPorMaterial.Checked;
            gerenciador.ConfiguracoesPGi.F3DOpcoesVisualizacao.CorPorSecao = rbCorPorSecao.Checked;
            gerenciador.ConfiguracoesPGi.F3DOpcoesVisualizacao.CorNo = CorNo.BackColor;
            gerenciador.ConfiguracoesPGi.F3DOpcoesVisualizacao.CorDescElementos = CorDescElementos.BackColor;

            gerenciador.ConfiguracoesPGi.F3DOpcoesVisualizacao.RgbArestas[0] = CorArestas.BackColor.R;
            gerenciador.ConfiguracoesPGi.F3DOpcoesVisualizacao.RgbArestas[1] = CorArestas.BackColor.G;
            gerenciador.ConfiguracoesPGi.F3DOpcoesVisualizacao.RgbArestas[2] = CorArestas.BackColor.B;

            gerenciador.formDesenho.Estrutura.Rgb_Barras[0] = btCorEstruturaGeral.BackColor.R;
            gerenciador.formDesenho.Estrutura.Rgb_Barras[1] = btCorEstruturaGeral.BackColor.G;
            gerenciador.formDesenho.Estrutura.Rgb_Barras[2] = btCorEstruturaGeral.BackColor.B;

            gerenciador.ConfiguracoesPGi.F3DOpcoesVisualizacao.CorFundoCima = btCorCima.BackColor;
            gerenciador.formDesenho.Alterou(true,false);
            gerenciador.formDesenho.AtualizaConfiguracoes3D();
        }
        void CarregaVisualizacao()
        {
            btCorCima.BackColor = gerenciador.ConfiguracoesPGi.F3DOpcoesVisualizacao.CorFundoCima;
            MostrarDescricaoElementos.Checked = gerenciador.ConfiguracoesPGi.F3DOpcoesVisualizacao.MostrarDescricaoElementos;
            MostrarNumeroElementos.Checked = gerenciador.ConfiguracoesPGi.F3DOpcoesVisualizacao.MostrarNumeroElementos;
            Nos.Checked = gerenciador.ConfiguracoesPGi.F3DOpcoesVisualizacao.Nos;
            MostrarEixosCentrais.Checked = gerenciador.ConfiguracoesPGi.F3DOpcoesVisualizacao.EixosCentrais;
            chPerspectiva.Checked = gerenciador.ConfiguracoesPGi.F3DOpcoesVisualizacao.Perspectiva;
            chUsarPlanoFundoGradiente.Checked = gerenciador.ConfiguracoesPGi.F3DOpcoesVisualizacao.UsarPlanoFundoGradiente;
            Transparencia.Value = 255 - gerenciador.ConfiguracoesPGi.F3DOpcoesVisualizacao.Transparencia;

            if (gerenciador.ConfiguracoesPGi.F3DOpcoesVisualizacao.TamApoios == 0)
              tamApoio.Value = 30;
            else
              tamApoio.Value = (int)((gerenciador.ConfiguracoesPGi.F3DOpcoesVisualizacao.TamApoios));

            if (gerenciador.ConfiguracoesPGi.F3DOpcoesVisualizacao.TamArt == 0)
                tamArt.Value = 8;
            else
                tamArt.Value = (int)((gerenciador.ConfiguracoesPGi.F3DOpcoesVisualizacao.TamArt));

            TamanhoNo.Value = (int)((double)(gerenciador.ConfiguracoesPGi.F3DOpcoesVisualizacao.tamanhoNo));

            MostrarEixosLocais.Checked = gerenciador.ConfiguracoesPGi.F3DOpcoesVisualizacao.MostrarEixosLocais;

            chMostrarChao.Checked = gerenciador.ConfiguracoesPGi.F3DOpcoesVisualizacao.MostrarChao;
            chMostrarTextura.Checked = gerenciador.ConfiguracoesPGi.F3DOpcoesVisualizacao.MostrarTexturas;
            Arestas.Checked = gerenciador.ConfiguracoesPGi.F3DOpcoesVisualizacao.Arestas;
            ArestaConfObjeto.Checked = gerenciador.ConfiguracoesPGi.F3DOpcoesVisualizacao.ArestasConformeObjetos;
            CorArestas.BackColor = Color.FromArgb(gerenciador.ConfiguracoesPGi.F3DOpcoesVisualizacao.RgbArestas[0],
            gerenciador.ConfiguracoesPGi.F3DOpcoesVisualizacao.RgbArestas[1],
            gerenciador.ConfiguracoesPGi.F3DOpcoesVisualizacao.RgbArestas[2]);
            CorNo.BackColor = gerenciador.ConfiguracoesPGi.F3DOpcoesVisualizacao.CorNo;
            CorDescElementos.BackColor = gerenciador.ConfiguracoesPGi.F3DOpcoesVisualizacao.CorDescElementos;
            rbCorPorTipoDeElemento.Checked = gerenciador.ConfiguracoesPGi.F3DOpcoesVisualizacao.CorPorTipoDeElemento;
            rbCorPorMaterial.Checked = gerenciador.ConfiguracoesPGi.F3DOpcoesVisualizacao.CorPorMaterial;
            rbCorPorSecao.Checked = gerenciador.ConfiguracoesPGi.F3DOpcoesVisualizacao.CorPorSecao;

            btCorEstruturaGeral.BackColor =
           Color.FromArgb(gerenciador.formDesenho.Estrutura.Rgb_Barras[0],
            gerenciador.formDesenho.Estrutura.Rgb_Barras[1]
           ,gerenciador.formDesenho.Estrutura.Rgb_Barras[2]);

            if (!Arestas.Checked)
                ArestaConfObjeto.Checked = false;

            ArestaConfObjeto.Enabled = Arestas.Checked;
        }

        private void button3_Click(object sender, EventArgs e)
        {
            Close();
        }
        void GravaDados()
        {
            GravaGrelha(grepisos.SelectedIndex);
            GravaSistema();
            GravaPortico();
            GravaVisualizacao();
            GravaUnidades();

            gerenciador.formDesenho.AtualizaConfiguracoes3D();
        }

        List<TabPage> paginas = new List<TabPage>();
        private void treeView1_AfterSelect(object sender, TreeViewEventArgs e)
        {
            try
            {
                if (tv.SelectedNode.Tag != null)
                {
                    for (int i = 0; i < paginas.Count; i++)
                        tbForm.TabPages.Remove(paginas[i]);

                    tbForm.TabPages.Add(paginas[(int)tv.SelectedNode.Tag]);
                    //  tbForm.SelectedIndex = tv.SelectedNode.Index;     
                }
            }
               
            catch(Exception ss)
            {
                MessageBox.Show(ss.Message);
            }
        }

        void ReposicionaTabPagesConformeTreeView()
        {
                List<TabPage> p = new List<TabPage>();

                for (int i = 0; i < tv.Nodes.Count; i++)
                {
                    if (tv.Nodes[i].Nodes.Count == 0)
                    {
                        for (int j = 0; j < tbForm.TabPages.Count; j++)
                           if (tbForm.TabPages[j].Text == tv.Nodes[i].Text)
                              p.Add(tbForm.TabPages[j]);      
                    }
                    else
                    {
                        for (int j = 0; j < tv.Nodes[i].Nodes.Count; j++)
                        {
                            for (int k = 0; k < tbForm.TabPages.Count; k++)
                                if (tbForm.TabPages[k].Text == tv.Nodes[i].Nodes[j].Text)
                                    p.Add(tbForm.TabPages[k]);  
                        }
                    }
                }

                for (int i = 0; i < p.Count; i++)
                    paginas.Add(p[i]);
        }

        private void FMateriais_Shown(object sender, EventArgs e)
        {
            tbForm.TabPages.RemoveAt(1);

            int cc = -1;
            for (int i = 0; i < tv.Nodes.Count; i++)
            {
                if (tv.Nodes[i].Nodes.Count == 0)
                {
                    cc++;
                    tv.Nodes[i].Tag = cc;
                }
                else
                {
                   for (int j = 0; j < tv.Nodes[i].Nodes.Count; j++)
                   {
                         cc++;
                         tv.Nodes[i].Nodes[j].Tag = cc;
                   }
                }
            }

            tv.ExpandAll();
            
            ReposicionaTabPagesConformeTreeView();

            Carrega();

            if (tv.Nodes[0].Nodes.Count > 0)
                tv.SelectedNode = tv.Nodes[0].Nodes[0];
            else
                tv.SelectedNode = tv.Nodes[0] ;
        
            treeView1_AfterSelect(tv, null);
            tv.SelectedNode = tv.Nodes[0];
            tv.Focus();

        }

        void Carrega()
        {
            carregando = true;
            if (gerenciador.Pavimentos.Count > 0)
            {
                foreach (TPavimento pav in gerenciador.Pavimentos)
                {
                    grepisos.Items.Add(pav.Descricao);
                }
            }

            carregando = false;

            CarregaSistema();
            CarregaPortico();
            CarregaVisualizacao();
            CarregaUnidades();             
        }
        void CarregaUnidades()
        {

            carregando = true;
            if (Cfg.unidadesProjeto != null)
            {
                geoUndComp.Text = Cfg.unidadesProjeto.geo_un_comprimento;
                geoCasasDecimais.Value = Cfg.unidadesProjeto.geo_casas;
                resCasasDecimais.Value = Cfg.unidadesProjeto.res_casas;
                unidComp.Text = Cfg.unidadesProjeto.res_un_comprimento;
                unidForca.Text = Cfg.unidadesProjeto.res_un_forca;

                unidDeslocamento.Text = Cfg.unidadesProjeto.res_un_deformacao;
                unidTensao.Text = Cfg.unidadesProjeto.res_un_tensao;
            }
            carregando = false;
        }

        void GravaUnidades()
        {
            if (!carregando)
            {
                Cfg.unidadesProjeto.geo_un_comprimento = geoUndComp.Text;
                Cfg.unidadesProjeto.res_un_comprimento = unidComp.Text;

                Cfg.unidadesProjeto.geo_casas = (int)geoCasasDecimais.Value;
                Cfg.unidadesProjeto.res_casas = (int)resCasasDecimais.Value;

                Cfg.unidadesProjeto.res_un_forca = unidForca.Text;
                Cfg.unidadesProjeto.res_un_deformacao = unidDeslocamento.Text;
                Cfg.unidadesProjeto.res_un_tensao = unidTensao.Text;
            }
        }

        private void claspesoesp_KeyPress(object sender, KeyPressEventArgs e)
        {
            e.Handled = (!(e.KeyChar == 44) && !char.IsDigit(e.KeyChar) && !char.IsControl(e.KeyChar));
        }

        private void FConfiguraProjeto_Move(object sender, EventArgs e)
        {
            gerenciador.AtualizaDesenho();
        }

        private void FConfiguraProjeto_FormClosed(object sender, FormClosedEventArgs e)
        {
           gerenciador.CfgProjeto = null;
        }

        private void btCorCima_Click(object sender, EventArgs e)
        {
            if (colorDialog1.ShowDialog() == DialogResult.OK)
                btCorCima.BackColor = colorDialog1.Color;
        }

        private void CorNo_Click(object sender, EventArgs e)
        {
            if (colorDialog1.ShowDialog() == DialogResult.OK)
                CorNo.BackColor = colorDialog1.Color;
        }

        private void CorDescElementos_Click(object sender, EventArgs e)
        {
            if (colorDialog1.ShowDialog() == DialogResult.OK)
                CorDescElementos.BackColor = colorDialog1.Color;
        }

        private void Arestas_CheckedChanged(object sender, EventArgs e)
        {
            if (!Arestas.Checked)
                ArestaConfObjeto.Checked = false;

            ArestaConfObjeto.Enabled = Arestas.Checked;
        }

        private void button4_Click(object sender, EventArgs e)
        {
            btCorCima.BackColor = Color.FromArgb(64,64,64);

        }

        private void Nos_CheckedChanged(object sender, EventArgs e)
        {

        }

        private void MostrarDescricaoElementos_CheckedChanged(object sender, EventArgs e)
        {
            if (MostrarDescricaoElementos.Checked)
                MostrarNumeroElementos.Checked = false;
        }

        private void MostrarNumeroElementos_CheckedChanged(object sender, EventArgs e)
        {
            if (MostrarNumeroElementos.Checked)
                MostrarDescricaoElementos.Checked = false;
        }

        private void button5_Click(object sender, EventArgs e)
        {

        }

        private void btCorEstruturaGeral_Click(object sender, EventArgs e)
        {
            if (colorDialog1.ShowDialog() == DialogResult.OK)
                btCorEstruturaGeral.BackColor = colorDialog1.Color;
        }

        private void sisSolver_cholesky_CheckedChanged(object sender, EventArgs e)
        {

        }
    }
}
