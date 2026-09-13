 using System;
 using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Diagnostics;
using System.Threading;
using System.IO;
using rpaulo.toolbar;
using PG;
using ControlExtenders;

using System.Runtime.Serialization.Formatters.Binary;
using System.Runtime.Serialization;
using System.Xml.Serialization;
//using PGiSolver;
using OpenTK;
using OpenTK.Input;
using OpenTK.Graphics.OpenGL;
using OpenTK.Platform;
using PGi.Testes;
using PGi.Properties;
using System.Windows.Media.Animation;
using System.Web.UI.Design;
using System.Windows.Media;
using System.Security.Permissions;


namespace PG
{
    public partial class Gerenciador : Form
    {
        public static FPrincipal desenho, detalhamento;
        public FPrincipal formDesenho;
        public FUnidades Unidades;
        public FConfiguraProjeto CfgProjeto;
        public FCarga CargaBarra; 
        public FCargaNodal CargaNodal;
            public DadosViga FDadosViga;
        public FDadosBarra DadosBarra;
        public FApoio fApoio;
        public FCasosCarga CasosCarga;
        public FMateriais Materiais;
        public FConfiguracaoPrograma fConfiguracaoPrograma;
        public FEdicaoNos fEditaNos;
        public FConfiguraDiagramas fConfiguraDiagramas;
        public FOpcoesCaptura OpcoesCaptura;
        public FCombinacoes Combinacoes;
        public FFatorCargas FatorCarga;

        public DadosPilar FDadosPilar;
        public DadosLaje FDadosLaje;
        public FNovoPavimento NovoPavimento;
        public FGrelhaOpcoesVisualizacao FOpcVisGrelha;
        public FConfiguraDeformacao ConfiguraDeformacao;

        public FGrelhaConfiguracao FConfiguracaoGrelha;
        public FProjetosRecentes FProjetosRecentes;
        public FLayers fLayers;
        public FFontes FFontes;
        public FVisualizadorGrelha FVisGrelha;
        public FVisualizadorPortico FVisPortico;

        public F3D F3d;
        public FCopiarPavimento FCopiarPavimento;

        public ToolBarDockHolder holderProgresso;

        ToolBarManager ToolBarManager1,
                       ToolBarManager2;

        DockExtender dockExtender;
        public bool alterado, necessitaCalculo;

        public List<TPavimento> Pavimentos;

        public TConfiguracoesPGi ConfiguracoesPGi;

        public TProjeto Projeto;
        
        #region ToolbarButtons

        private ToolBarButton btLinha2;
        private ToolBarButton btArco2;
        private ToolBarButton btTexto2;
        private ToolBarButton btCirculo2;

     //   private ToolBarButton btCfgViga;
        private ToolBarButton btInsViga;
        private ToolBarButton btDivViga2;
        private ToolBarButton btUnirViga2;

     //   private ToolBarButton btCfgLaje;
        private ToolBarButton btInsLaje;

        private ToolBarButton btRodarPilar;
        private ToolBarButton btInsPilar;
        private ToolBarButton btCopiarPilar;
        private ToolBarButton btMudarPontoFixoPilar;
        private ToolBarButton btDeslocarPilar;

        private ToolBarButton btCargaLinear2;
        private ToolBarButton btCargaPontual2;
        private ToolBarButton btCargaArea;

        private ToolBarButton btCalcular2;
        private ToolBarButton separador1;
       
        private ToolBarButton btUndo2;
        private ToolBarButton btRedo2;
        private ToolBarButton btEnquadra2;
        private ToolBarButton btLayers2;
        private ToolBarButton btEditarGrelha2;
        private ToolBarButton btSalvarGrelha2;

        private ToolBarButton bt3DEstrutura2;
        private ToolBarButton bt3DPavimento2;
        private ToolBarButton bt3DOpcoesVisualizacao2;
        
        private ToolBarButton btEsforcoGrelha2;
        private ToolBarButton btIsovalorGrelha2;
        private ToolBarButton btEsforcoPortico2;


        private ToolBarButton btArvore2;
        private ToolBarButton btSalvar2;
        private ToolBarButton btAbrir2;
        private ToolBarButton btNovo2;
        private ToolBarButton btPavimentos2;
        private ToolBarButton btEditarPavimento2;

        private ToolBarButton btVisGrelhaFletor;
        private ToolBarButton btVisGrelhaTorcor;
        private ToolBarButton btVisGrelhaCortante;
        private ToolBarButton btVisGrelhaDeslocamento;
        private ToolBarButton btVisGrelhaOpc;
        private ToolBarButton btVisGrelhaAtualizar;
        private ToolBarButton btVisGrelhaVistaCima;
        private ToolBarButton btVisGrelhaVistaPerspectiva;

        private ToolBarButton btVisPorticoFletorZ, btVisPorticoFletorY, btVisPorticoTorcor,btVisPorticoAxiais, btVisPorticoCortanteZ, btVisPorticoCortanteY,
                              btVisPorticoDeslocamento,btVisPorticoOpc, btVisPorticoAtualizar, btVisPorticoVistaCima, btVisPorticoVistaPerspectiva;
        #endregion

        public void CriaConfiguracoesPadrao()
        {
           
            ConfiguracoesPGi = new TConfiguracoesPGi();

            MudaStatusBotao(btMostrarNos);
            MudaStatusBotao(btMostrarEixosLocais);
        }

        void CarregaConfiguracaoSnap()
        {
            PontoFinal.Checked = ConfiguracoesPGi.snap_PontoFinal;
            PontoMedio.Checked = ConfiguracoesPGi.snap_PontoMedio;
            Intersecao.Checked = ConfiguracoesPGi.snap_Intersecao;
            Perpendicular.Checked = ConfiguracoesPGi.snap_Perpendicular;

            formDesenho.snap_Intersecao = Intersecao.Checked;
            formDesenho.snap_Perpendicular = Perpendicular.Checked;
            formDesenho.snap_PontoFinal= PontoFinal.Checked;
            formDesenho.snap_PontoMeio = PontoMedio.Checked;
            formDesenho.divCotas = ConfiguracoesPGi.divisaoCotas;
        }
        public TConfiguracoesPrograma ConfiguracaoPrograma;
        public void CarregaConfiguracaoPrograma()
        {
            StreamReader sr;
            StringBuilder dados = new StringBuilder(); ;
            string arq = System.IO.Path.GetDirectoryName(Application.ExecutablePath) + "\\preferencias_programa.txt";
            string item, texto, linha;
            sr = File.OpenText(arq);
            
            try
            {
                ConfiguracaoPrograma = new TConfiguracoesPrograma();

                string s;
                int lin = 0;
                while ((s = sr.ReadLine()) != null)
                {
                    if (s.Trim() == string.Empty)
                        break;
                    lin++;

                    linha = s.Trim();
                    texto = linha.Substring(0, linha.IndexOf("=") + 1);
                    item = linha.Substring(linha.IndexOf("=") + 1, linha.Length - texto.Length);
                    dados.Append(item.Trim());
                }

                ConfiguracaoPrograma.SuavizacaoOpenGl = dados[0].ToString().Trim() == "S";
                ConfiguracaoPrograma.IrParaPaginaResultados = dados[1].ToString().Trim() == "S";
                ConfiguracaoPrograma.FecharJanelaResultadosAposCalculo = dados[2].ToString().Trim() == "S";
                ConfiguracaoPrograma.AbrirUltimoProjeto = dados[3].ToString().Trim() == "S";
            }
            catch (Exception ex)
            {
                MessageBox.Show("erro ao acessar preferências do programa: " +ex.Message);
            }

            sr.Dispose();
            sr.Close();
        }

        public void ShowMatriz(int barra, bool global, bool grelha)
        {
            formDesenho.ShowMatrizTela(global, barra,350, grelha);
        }

        private void CfgToolBarButton(ref ToolBarButton button, int imageindex, string tooltip, string nome, string tag, ref ToolBar toolbar, bool separador = false)
        {
            button = new System.Windows.Forms.ToolBarButton();
            button.Name = nome;
            button.ImageIndex = imageindex;
            button.ToolTipText = tooltip;
            button.Tag = tag;

            if (separador)
              button.Style = ToolBarButtonStyle.Separator;

            toolbar.Buttons.Add(button);
        }

        private int r(ref int v)
        {
            return v = 0;
        }

        private void SetMenuItens()
        {
        //    RenomearViga.Tag = Const.ID_RENOMEAR_VIGA;
        }

        void SetBotoes()
        {
            panelDeformacoes.Visible = false;
            panelDinamica1.Visible = false;
            panelModosFlambagem.Visible = false;
            panelDiagramas.Visible = false;
            panelTensoes.Visible = false;

            btBarra.Tag = Const.ID_BARRAGENERICA;


            ribbonTabGrelha.Visible = false;
           // ribbonTabPortico.Visible = false;
        }

        public void StartSplash()
        {
            Application.Run(new Splash());
        }

        private void ribbon2_OrbDropDown_Click(object sender, EventArgs e)
        {
          //  if (sender is RibbonOrbRecentItem)
          //      MessageBox.Show((sender as RibbonOrbRecentItem).Text);
        }
        private void ribbonOrbRecentItem4_Click(object sender, EventArgs e)
        {
            Abrir((sender as RibbonOrbRecentItem).Text);
        }
        void CarregaArquivosRecentes()
        {
            string NomeArquivo = Directory.GetCurrentDirectory() + @"\recentes";
            //MenuItemRecentes.MenuItems.Clear();
        
            /*Arquivos recentes*/

            if (File.Exists(NomeArquivo))
            {
                FileStream arquivo;
                BinaryFormatter arqBin = new BinaryFormatter();

                arquivo = new FileStream(NomeArquivo, FileMode.Open, FileAccess.Read);
                ArquivosRecentes = (List<string>)arqBin.Deserialize(arquivo);
                arquivo.Close();

                ArquivosRecentes.RemoveAll(obj => obj.ToString() == "");

                for (int i = ArquivosRecentes.Count-1; i >= 0; i--)
                {
                    if (lbRecentes.Items.Count > 10) break;

                   lbRecentes.Items.Add(ArquivosRecentes[i]);
                //    ribbon2.OrbDropDown.RecentItems[ribbon2.OrbDropDown.RecentItems.Count - 1].Click += new EventHandler(ribbonOrbRecentItem4_Click);

                 //   MenuItemRecentes.MenuItems.Add(ArquivosRecentes[i]);
                  ///  MenuItemRecentes.MenuItems[MenuItemRecentes.MenuItems.Count - 1].Click += new EventHandler(MenuItemRecente_Click);
                }//

                if (ArquivosRecentes.Count > 10)
                   ArquivosRecentes.RemoveRange(0, (ArquivosRecentes.Count - 10));         
            }
           // else
          //    MenuItemRecentes.Enabled = false;
        }

        void CarregaGeral()
        {
            CriaListasPadrao();
           // CarregaArquivosRecentes();
  

            stripPrincipal.ImageList = imgBotoesLigaDesliga;
         //   DivBarrasCotas.Value = 15;

            
        }

        void CriaListasPadrao()
        {
            Pavimentos = new List<TPavimento>();
            ElementosCopiados = new List<TObjetoDesenho>();
        }

        void MenuItemRecente_Click(object sender, EventArgs e)
        {

        }
        public bool Inicializando;
        public List<Panel> panelCores;
        public List<Label> labelCores;

        void CarregaEstadoPanels()
        {
            pnCorResultados.Visible = false;
            panelCores = new List<Panel>();
            panelCores.Add(grad1); panelCores.Add(grad2); panelCores.Add(grad3); panelCores.Add(grad4); panelCores.Add(grad5);
            panelCores.Add(grad6); panelCores.Add(grad7); panelCores.Add(grad8); panelCores.Add(grad9); panelCores.Add(grad10); 
            panelCores.Add(grad11);
            panelCores.Add(grad12); panelCores.Add(grad13); panelCores.Add(grad14);

            labelCores = new List<Label>();
            labelCores.Add(lbTipoResultado);
            labelCores.Add(lbVal1);
            labelCores.Add(lbVal2); labelCores.Add(lbVal3);
            labelCores.Add(lbVal4); labelCores.Add(lbVal5);
            labelCores.Add(lbVal6); labelCores.Add(lbVal7);
            labelCores.Add(lbVal8); labelCores.Add(lbVal9);
            labelCores.Add(lbVal10); labelCores.Add(lbVal11);
            labelCores.Add(lbVal12); labelCores.Add(lbVal13);
            labelCores.Add(lbVal14); labelCores.Add(lbVal15);

            string NomeArquivo = Directory.GetCurrentDirectory() + @"\estadopanels";
            if (File.Exists(NomeArquivo))
            {
                FileStream arquivo;
                BinaryFormatter arqBin = new BinaryFormatter();

                arquivo = new FileStream(NomeArquivo, FileMode.Open, FileAccess.Read);
                estadoPanels = (List<EstadoPanels>)arqBin.Deserialize(arquivo);
                arquivo.Close();

                pnCargasCombinacoes.Left = estadoPanels[0].left;
                pnCargasCombinacoes.Top = estadoPanels[0].top;

                pnCargasLeft = (pnCargasCombinacoes.Left * 100) / this.Width;
                pnCargasTop = (pnCargasCombinacoes.Top * 100) / this.Height;

                pnUtilitarios.Left = estadoPanels[1].left;
                pnUtilitarios.Top = estadoPanels[1].top;

                pnCorResultados.Left = estadoPanels[2].left;
                pnCorResultados.Top = estadoPanels[2].top;

                pnCargasLeft = (pnCargasCombinacoes.Left * 100) / this.Width;
                pnCargasTop = (pnCargasCombinacoes.Top * 100) / this.Height;
            }
            else
            {
                pnCargasCombinacoes.Left = this.Width/2 - pnCargasCombinacoes.Width/2;
                pnCargasCombinacoes.Top = 0;

                pnUtilitarios.Left = this.Width - (int)(pnUtilitarios.Width)-15;
                pnUtilitarios.Top = 0;

                pnCorResultados.Left = this.Width - (int)(pnCorResultados.Width);
                pnCorResultados.Top = this.Height/2;
            }

            pnUtilitariosLeftRel = (double)pnUtilitarios.Left / this.ClientSize.Width;
            pnUtilitariosTopRel = (double)pnUtilitarios.Top / this.ClientSize.Height;
            pnCargasLeftRel = (double)pnCargasCombinacoes.Left / this.ClientSize.Width;
            pnCargasTopRel = (double)pnCargasCombinacoes.Top / this.ClientSize.Height;
        }
        public Gerenciador()
        {
            try
            {               

            Thread t = new Thread(new ThreadStart(StartSplash));
            t.Start();
            InitializeComponent();
            Thread.CurrentThread.CurrentCulture = System.Globalization.CultureInfo.CreateSpecificCulture("en");
   
            PanelAguarde.Visible = false;
            pnCombinacoes.Visible = false;
            pnCasos.Visible = false;

            CarregaConfiguracaoPrograma();
            CarregaArquivosRecentes();
          
            SetDockPanels();
            SetBotoes();
                
            CriaFormDesenho();
            CarregaGeral();

            CriaConfiguracoesPadrao();
            CarregaConfiguracaoSnap();           

            //Comando.Text = "";
   
            lbInformacao.Text = "";

            t.Abort();

            this.BringToFront();
                this.SetStyle(ControlStyles.ResizeRedraw, true);
            }
           catch(Exception e )
           {
               MessageBox.Show("erro  - public Gerenciador() " + e.Message);
           }
        }

        public IFloaty floaty;
        public void SetDockPanels()
        {
            dockPanel.BringToFront();
            dockPanel.Dock = DockStyle.Fill;
            dockExtender        = new DockExtender(this);     

        //    floaty.Text = "Cálculo";
       //     floaty.Docking += new EventHandler(floaty_Docking);
        }

        void floaty_Docking(object sender, EventArgs e)
        {
            // make sure the ZOrder remains intact
            this.BringToFront();
        }

        private void Form_MouseWheel(object sender, System.Windows.Forms.MouseEventArgs e)
        {
 //          formDesenho.Desenho_MouseWheel(desenho, e);
        }

        private void CriaForms()
        {
            FOpcVisGrelha       = new FGrelhaOpcoesVisualizacao(this);
           // FConfiguracaoGrelha = new FGrelhaConfiguracao(this);
           // fLayers             = new FLayers();
        }

       public ToolBarDockHolder holderPrincipal, holderElementosEstruturais, holderElemBasicos,
                          holderPiso,
                          holderGrelhaEspacial, holderPorticoEspacial, holderCargas,
                          holder3D,
                          holderEsforcoEspacialEscala,
                          holderEsforcoEspacialCombinacao;

        public List<ToolBarDockHolder> HoldersDesenho;
        public List<ToolBarDockHolder> Lista_HoldersGrelhaEspacial, Lista_HoldersPorticoEspacial;

        List<ToolBarButton> BotoesEnable = new List<ToolBarButton>();

        TransparentPanel pnredonda;

        private void Gerenciador_Shown(object sender, EventArgs e)
        {
            try
            {

                /*  CriaFormDesenho();

                  CriaConfiguracoesPadrao();
                  CarregaConfiguracaoPadrao();
                  SetaEnableToolBarButtons(false);*/
                //      CriaConfiguracoesPadrao();
                //    CriaFormDesenho();
                Inicializando = true;
                NovoProjeto(true);
              //  this.TopMost = true;
                Inicializando = false;
                // PanelBaixo.Dock = DockStyle.Bottom;

                PanelBaixo.Visible = false;
                CarregaEstadoPanels();

                
             /*   this.pnredonda.Location = new System.Drawing.Point(243, 125);
                this.pnredonda.Name = "pnredonda";
                this.pnredonda.Size = new System.Drawing.Size(237, 32);
                this.pnredonda.TabIndex = 155;
                pnredonda.Height = 327;
                pnredonda.Width = 75;
                this.Controls.Add(this.pnredonda);
                pnredonda.BringToFront();*/
/*
                pnredonda = new CustomPanel();

                this.pnredonda.Location = new System.Drawing.Point(243, 125);
                this.pnredonda.Name = "pnredonda";
                this.pnredonda.Size = new System.Drawing.Size(237, 32);
                this.pnredonda.TabIndex = 155;
                pnredonda.BorderRadius = 5;
                pnredonda.BorderSize = 0;
                pnredonda.BorderStyle = BorderStyle.None;
                pnredonda.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(255)))), ((int)(((byte)(128)))));
                this.TransparencyKey = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(255)))), ((int)(((byte)(128)))));
                this.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(192)))), ((int)(((byte)(192)))));

                pnredonda.BorderColor = Color.FromArgb(64, 64, 64);
                pnredonda.Height = 327;
                pnredonda.Width = 75;

                pnredonda.BorderFocusColor = Color.Red;
                pnredonda.UnderlinedStyle = true;
                pnredonda.Controls.Add(pnCorResultados);
                pnCorResultados.Visible = true;
                this.Controls.Add(this.pnredonda);
                pnredonda.BringToFront();*/
                //   this.WindowState = System.Windows.Forms.FormWindowState.Maximized;

                //   formDesenho.x_rot_angle = 135;
                //    formDesenho.y_rot_angle = 230;
            }
            catch(Exception ex )
            {
                MessageBox.Show("erro gerenciador_onShown -  " + ex.Message);
            }
           // this.TopMost = false;
           // ChamaPorticoEspacial();
        }

        int PavAntes;
        void ChamaGrelhaEspacial()
        {
            if (formDesenho.Estrutura.vigas.Count == 0 && formDesenho.Estrutura.calculoOk)
            {
                MessageBox.Show(">Pavimento não possui grelha.","Aviso",MessageBoxButtons.OK,MessageBoxIcon.Information);
                return;
            }
            if (!formDesenho.Estrutura.calculoOk)
            {
                MessageBox.Show(">Pavimento não foi calculado.", "Atenção", MessageBoxButtons.OK, MessageBoxIcon.Information);
                return;
            }

            if (formDesenho.Estrutura.vigas.Count() == 0)
            {
                MessageBox.Show(">Pavimento não possui grelha.");
                return;
            }
            
            if (arquivoAtual != null && GrelhaPrimeiraVez)
              CarregaGrelhaPrimeiraVez();

            floaty.Hide();
            
            if (F3d != null)
                F3d.Close();
            if (FVisPortico != null)
                FVisPortico.Close();
            if (FVisGrelha != null)
              FVisGrelha = null;

            /*o usuário pode mudar o pavimento navegando na grelha, mas quando fechar a grelha, tem que voltar p pavimento que estava no editor*/
            PavAntes = cbPiso.SelectedIndex;

        //    FVisGrelha = new FVisualizadorGrelha(this, formDesenho.PavimentoAtual);

          //  formDesenho.Visible = false;
            FVisGrelha.MdiParent = this;
            FVisGrelha.WindowState = System.Windows.Forms.FormWindowState.Maximized;
            FVisGrelha.Show(dockPanel);
            FVisGrelha.Atualizar();
            // formDesenho.Visible = true;

        }

        public TPorticoEspacial PorticoEspacial;
        void ChamaPorticoEspacial(bool avulso = false)
        {
           // if (PorticoEspacial == null)
           // {
         ///       MessageBox.Show("Estrutura ainda não foi calculada.", "Aviso", MessageBoxButtons.OK, MessageBoxIcon.Information);
           //     return;
           // }

         //   if (!PorticoEspacial.calculoOk)
         //   {
         //       MessageBox.Show("Estrutura ainda não foi calculada.", "Aviso", MessageBoxButtons.OK, MessageBoxIcon.Information);
         //       return;
         //   }

            try
            {
                floaty.Hide();
                
                if (F3d != null)
                    F3d.Close();
                if (FVisGrelha != null)
                    FVisGrelha.Close();
          
                if (FVisPortico != null)
                    FVisPortico = null;

               // PorticoEspacial = null;
  
               // PorticoEspacial = new TPorticoEspacial(Pavimentos, this);

                abrindoPortico = true;
                FVisPortico = new FVisualizadorPortico(this, avulso, ref PorticoEspacial);
                FVisPortico.Deslocamento = true;
                FVisPortico.Text = "Pórtico";
           //     btVisPorticoDeslocamento.Pushed = true;
             //   btVisPorticoFletorY.Pushed = false;
             //   btVisPorticoFletorZ.Pushed = false;
              //  btVisPorticoTorcor.Pushed = false;
              //  btVisPorticoCortanteZ.Pushed = false;
              //  btVisPorticoCortanteY.Pushed = false;

              //colocar o windowstate sempre antes do show, pois senao vai criar os botoes de minimizar e fechar no canto superior da tela e fica esquisito
                FVisPortico.MdiParent = this;
                FVisPortico.WindowState = System.Windows.Forms.FormWindowState.Maximized;
                FVisPortico.Show(dockPanel);
        
                PavAntes = cbPiso.SelectedIndex;
                FVisPortico.Atualizar();

                if (!avulso)
                {
                    EsforcoPortico = new FEsforcosPortico(FVisPortico);
                    EsforcoPortico.Show();
                //    PanelCalcularPortico.Visible = false;
                }
                else
                {
          //          PanelCalcularPortico.Visible = true;
                }

                abrindoPortico = false;
            }
            catch(Exception e)
            {
                MessageBox.Show(e.Message);
            }
        }
        public bool abrindoPortico;


        void CarregaGrelhaPrimeiraVez()
        {
         /*   FileStream arquivo;
            BinaryFormatter arqBin = new BinaryFormatter(); 
            
            ChamaAguardar(this, "Carregando modelos calculados. Aguarde...");

            List<TLayer> Grelha = new List<TLayer>();
            arquivo = new FileStream(arquivoAtual + "\\@PgiGrelha", FileMode.Open, FileAccess.Read);
            Grelha  = (List<TLayer>)arqBin.Deserialize(arquivo);

            for (int i = 0; i < Grelha.Count; i++)
            {
                Pavimentos[i].LayersByIdPrincipal[Lay.GrelhaLajes] = Grelha[i];
                Pavimentos[i].layers[5] = Grelha[i];

                Pavimentos[i].barras = new TBarraGrelha[Pavimentos[i].max_sBarra+1];
                Pavimentos[i].nos    = new TNoGrelha[Pavimentos[i].max_sNo+1];
               
                int nNo = 0;
                for (int j = 1; j <= Pavimentos[i].LayersByIdPrincipal[Lay.GrelhaLajes].Obj.Count; j++)
                {
                    if (Pavimentos[i].LayersByIdPrincipal[Lay.GrelhaLajes].Obj[j - 1].Tipo == Const.ID_BARRAGRELHA)
                    {
                        Pavimentos[i].barras[j] = (TBarraGrelha)Pavimentos[i].LayersByIdPrincipal[Lay.GrelhaLajes].Obj[j - 1];
                        Pavimentos[i].barras[j].Pavimento = Pavimentos[i];

                        if (!Pavimentos[i].LocalizaNo(ref Pavimentos[i].barras[j].pIni))
                            Pavimentos[i].nos[++nNo] = Pavimentos[i].barras[j].pIni;

                        if (!Pavimentos[i].LocalizaNo(ref Pavimentos[i].barras[j].pFin))
                            Pavimentos[i].nos[++nNo] = Pavimentos[i].barras[j].pFin;
                    }
                }
            }

            arquivo.Close();
            FechaAguardar();
            GrelhaPrimeiraVez = false;*/
           // formDesenho.Controle.SwapBuffers();
        }

        public void VoltaParaPavimento()
        {           
/*if (FVisGrelha != null)
            {
                FVisGrelha.Close();
                FVisGrelha = null;
            }*/
            try
            {
              //  cbPiso.SelectedIndex = PavAntes;
                //  formDesenho.Controle.InitializeContexts();
                formDesenho.Visible = true;
                //   formDesenho.WindowState = System.Windows.Forms.FormWindowState.Maximized;
                //  formDesenho.Controle.SwapBuffers();

                //SetaVisibilidadeHolders(HoldersDesenho, true);

              //  SetaVisibilidadeHolders(Lista_HoldersGrelhaEspacial, false);
              //  SetaVisibilidadeHolders(Lista_HoldersPorticoEspacial, false);

             //   btEsforcoGrelha2.Enabled = true;
                //    btIsovalorGrelha.Enabled = true;
                btEditarGrelha.Enabled = true;
                btSalvarGrelha.Enabled = false;
                ribbonTabGrelha.Visible = false;
             //   ribbonTabPortico.Visible = false;
              
                Ribbon.SelectedTab = tabPrincipal;
     
            }
            catch(Exception e)
            {
                MessageBox.Show(e.Message);
            }
          //  formDesenho.Desenho_Resize(Gerenciador.desenho,null);
         //   formDesenho.DrawByLayer(ref formDesenho.ControleCAD);
        }

        public void CancelaTudo()
        {
         //   btLinha2.Pushed = false;
        //    btSalvar2.Pushed = false;
        //    btTexto2.Pushed = false;
        //    btLinha2.Pushed = false;
        }

        private void CriaFormDesenho()
        {
            try
            {
             //   if (desenho == null)
                desenho = new FPrincipal(this);

                formDesenho = desenho;

                formDesenho.WindowState = System.Windows.Forms.FormWindowState.Maximized;
                //     formDesenho.Show(dockPanel);
                formDesenho.camera = new Camera3D(formDesenho.Width, formDesenho.Height);
                formDesenho.camera.x_rot_angle = 135;
                formDesenho.camera.y_rot_angle = 225;
                formDesenho.pnDivBarras.Visible = false;
                this.Text = "PGi - [Não salvo]";

               // AtivaModo3D();            

                AddTools();

            }
            catch (Exception e)
            {
                MessageBox.Show(e.Message);
            }
        }

        void AddTools()
        {
            try
            {
                formDesenho.AdicionaFerramentasDesenho(Const.ID_LAJE, new TLaje());
                formDesenho.AdicionaFerramentasDesenho(Const.ID_TRECHOVIGA, new TTrechoViga(formDesenho.DadosTrecho));
                formDesenho.AdicionaFerramentasDesenho(Const.ID_BARRAGENERICA, new TBarraGenerica(formDesenho.DadosBarra));
                formDesenho.AdicionaFerramentasDesenho(Const.ID_APOIO, new TApoio(formDesenho.DadosApoio));
                formDesenho.AdicionaFerramentasDesenho(Const.ID_PILAR, new TPilar(formDesenho.DadosPilar));

                formDesenho.AdicionaFerramentasDesenho(Const.ID_LINHA, new TLinha());
                formDesenho.AdicionaFerramentasDesenho(Const.ID_ARCOIMF, new TArcoIMF());
                formDesenho.AdicionaFerramentasDesenho(Const.ID_TEXTO, new TTexto());
                formDesenho.AdicionaFerramentasDesenho(Const.ID_CIRCULO, new TCirculo());

                formDesenho.AdicionaFerramentasDesenho(Const.ID_CARGA_PONTUAL, new TCargaPontual());
                formDesenho.AdicionaFerramentasDesenho(Const.ID_CARGA_LINEAR, new TCargaLinear());

                /*Ferramentas de edição*/
                formDesenho.AddEditTool(Const.ID_DIVIDIR_VIGA, new TDivideTrechoViga());
                formDesenho.AddEditTool(Const.ID_COPIAR_ELEMENTOS, new TCopiarElementos());
                formDesenho.AddEditTool(Const.ID_ROTACIONAR_ELEMENTOS, new TRotacionarElementos());
                formDesenho.AddEditTool(Const.ID_ESPELHAR_ELEMENTOS, new TEspelharElementos());

                formDesenho.AddEditTool(Const.ID_GIRAR_ELEMENTOS, new TGirarElementos());
                formDesenho.AddEditTool(Const.ID_MOVER_ELEMENTOS, new TMoverElementos());
                formDesenho.AddEditTool(Const.ID_MOVER_EXTREMO_ELEMENTOS, new TMoverExtremoElemento());
                formDesenho.AddEditTool(Const.ID_COPIA_PADRAO, new TCopiaPadrao());
                formDesenho.AddEditTool(Const.ID_DIVIDIR_NAS_INTERSECOES, new TDividirNasInterseccoes());

                formDesenho.AddEditTool(Const.ID_ARTICULAR_ELEMENTO, new TArticularElemento());

                formDesenho.AddEditTool(Const.ID_RENOMEAR_VIGA, new TRenomeiaTrechosVigas());
                formDesenho.AddEditTool(Const.ID_ROTACIONAR_PILAR, new TDivideTrechoViga());
            }
            catch(Exception exx)
            {
                MessageBox.Show(exx.Message);
            }
        }
        public FEdicaoNos rotacaoElementos;
        private void salvarToolStripMenuItem_Click(object sender, EventArgs e)
        {
            string folder = Directory.GetCurrentDirectory();

            if (!Directory.Exists(folder))
                Directory.CreateDirectory(folder);

            StreamWriter ss = new StreamWriter(folder + "\\tses.txt");

            for (int g = 0; g < 20000; g++)
                ss.WriteLine("aa" + g.ToString());

            ss.Close();
        }

        private void menuItem11_Click(object sender, EventArgs e)
        {
            Application.Exit();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            if (cbPiso.SelectedIndex > 0)
            {
                cbPiso.SelectedIndex -= 1;

                AlternaPavimento();
            }
        }

        private void button2_Click(object sender, EventArgs e)
        {

        }
        private void TreeViewForm_DoubleClick(object sender, EventArgs e)
        {
            if (!EhRaiz)
            {
                if (ChamaGrelha)
                {
                    if (FVisGrelha == null)
                      ChamaGrelhaEspacial();
                }
                else
                {
                    if (FVisGrelha != null)
                      FVisGrelha.Close();
                }

                cbPiso.SelectedIndex = PavimentoDoTreeView;
                AlternaPavimento();
                //cbPiso_SelectionChangeCommitted(cbPiso, null);
            }
        }

        void AlternaPavimento()
        {
            CancelaEdicoes();
            if (this.ActiveMdiChild == FVisGrelha)
            {
                FVisGrelha.PavAntes  = cbPiso.SelectedIndex;

                if (Pavimentos[cbPiso.SelectedIndex].vigas.Count == 0 && Pavimentos[cbPiso.SelectedIndex].calculoOk)
                {
                    MessageBox.Show("Pavimento não possui grelha.", "Aviso", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    cbPiso.SelectedIndex = PavAntes;
                    FVisGrelha.VistaPerspectiva();
                    return;
                }
                else
                {
                    FVisGrelha.Pavimento = Pavimentos[cbPiso.SelectedIndex];

                    FVisGrelha.CarregaPavimentoSelecionado();
                    FVisGrelha.GerarModelo3D(true);
                }
            }
            else
         //   if (this.ActiveMdiChild == desenho)
            {
                PavAntes = cbPiso.SelectedIndex;
   

                formDesenho.PavimentoAtual = cbPiso.SelectedIndex;

                formDesenho.AtualizarPixels();
                SetaToolTipsPavimento();

                formDesenho.Atualiza_Segmentos_e_Snap(true);

                if (cbPiso.SelectedIndex > -1)
                {
                    edIncPlano.Text = (Pavimentos[cbPiso.SelectedIndex].Nivel / 100).ToString("n2");

                    formDesenho.PlanoTrabalho.Normal.x = 0;
                    formDesenho.PlanoTrabalho.Normal.y = 0;
                    formDesenho.PlanoTrabalho.Normal.z = 1;
                    formDesenho.PlanoTrabalho.Posicao.z = Pavimentos[cbPiso.SelectedIndex].Nivel;
                    formDesenho.PlanoTrabalho.Posicao.x = 0;
                    formDesenho.PlanoTrabalho.Posicao.y = 0;
                }
                formDesenho.DesenhaObjetos();
                formDesenho.glControl.SwapBuffers();
            }
        }

        void SetaToolTipsPavimento()
        {
         //   btEditarGrelha.ToolTip    = formDesenho.PavimentoAtual.Descricao + " - Editar a Grelha";
          //  btEsforcoGrelha2.ToolTipText   = formDesenho.PavimentoAtual.Descricao + " - Visualizar Esforços";
          //  btIsovalorGrelha.ToolTipText =  formDesenho.PavimentoAtual.Descricao + " - Visualizar Isovalores";

          //  btEditarPavimento2.ToolTipText = "Voltar para o piso " + formDesenho.PavimentoAtual.Descricao;
        }

        private void cbPiso_SelectionChangeCommitted(object sender, EventArgs e)
        {
            AlternaPavimento();
        }

        private void DigitarComando_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyData == Keys.Escape)
            {
                formDesenho.CancelaInsercoes();
                CancelaEdicoes();
             //   CancelaResultados();
                ElementosCopiados.Clear();
            }
            //  else
            //  if (e.KeyData == Keys.Enter && DigitarComando.Text == "" && formDesenho.drawObjectId == Const.ID_TRECHOVIGA)
            //  {
            //      formDesenho.NovaViga();
            //  }
            else
                if (e.KeyData == Keys.Enter)
                {
                    try
                    {
                        if ((formDesenho.tipoComando == eTipoComando.draw))
                        {
                            if (DigitarComando.Text.Contains("@") && !DigitarComando.Text.Contains("<")) // coordenada relativa simples
                            {
                                string x = DigitarComando.Text.Substring(1, DigitarComando.Text.IndexOf(",") - 1).Replace(".", ",");
                                string y = DigitarComando.Text.Substring(DigitarComando.Text.IndexOf(",") + 1, DigitarComando.Text.Length - (x.Length) - 2).Trim().Replace(".", ",");
                                formDesenho.MouseDrawing(System.Convert.ToSingle(x), System.Convert.ToSingle(y), 0);
                               
                            }
                            if (DigitarComando.Text.Contains("@") && DigitarComando.Text.Contains("<")) // coordenada relativa polar
                            {
                                string tamanho = DigitarComando.Text.Substring(1, DigitarComando.Text.IndexOf("<") - 1).Replace(".", ",");
                                string angulo = DigitarComando.Text.Substring(DigitarComando.Text.IndexOf("<") + 1, DigitarComando.Text.Length - (tamanho.Length) - 2).Trim().Replace(".", ",");

                                float ca = System.Convert.ToSingle(tamanho) * System.Convert.ToSingle(Math.Cos(System.Convert.ToDouble(angulo) * Const.PIDiv180));
                                float co = System.Convert.ToSingle(tamanho) * System.Convert.ToSingle(Math.Sin(System.Convert.ToDouble(angulo) * Const.PIDiv180));


                                formDesenho.MouseDrawing(System.Convert.ToSingle(ca),
                                                                         System.Convert.ToSingle(co),0);
                             
                            }
                            else  //coordenada simples
                            if (!DigitarComando.Text.Contains("@") && !DigitarComando.Text.Contains("<")) // coordenada simples
                            {
                                if (formDesenho.FerramentaEdicao != null && (formDesenho.IdFerramentaEdicao == Const.ID_GIRAR_ELEMENTOS))
                                {
                                    if ((formDesenho.FerramentaEdicao as TGirarElementos).Comando == (formDesenho.FerramentaEdicao as TGirarElementos).ListaComandos[2])
                                    {
                                        double ang;

                                        if (DigitarComando.TextLength == 0)
                                            ang = 0;
                                        else
                                            ang = Convert.ToDouble(DigitarComando.Text.Substring(0, DigitarComando.TextLength).Replace(".", ","));

                                        (formDesenho.FerramentaEdicao as TGirarElementos).angRotacao = ang;
                                    /*    double hip = 1 / Math.Sin(ang * Const.PIDiv180);
                                        double novoX = Math.Cos(ang * Const.PIDiv180) * hip;

                                        (formDesenho.NewObject as TPilar).pFin.x = xIni + novoX;
                                        (formDesenho.NewObject as TPilar).pFin.y = yIni + 1;

                                        formDesenho.HandleMouseDownDrawing(xIni + novoX, yIni + 1);*/
                                    }
                                }
                                else
                                    if (formDesenho.ObjetoNovo != null)
                                {
                                    if (formDesenho.IdObjetoDesenho == Const.ID_PILAR && (formDesenho.ObjetoNovo as TPilar).DefinindoAngulo)
                                    {
                                        double xIni = (formDesenho.ObjetoNovo as TPilar).pIni.x;
                                        double yIni = (formDesenho.ObjetoNovo as TPilar).pIni.y;

                                        double ang;

                                        if (DigitarComando.TextLength == 0)
                                            ang = 0;
                                        else
                                            ang = Convert.ToDouble(DigitarComando.Text.Substring(0, DigitarComando.TextLength).Replace(".", ","));

                                        double hip = 1 / Math.Sin(ang * Const.PIDiv180);
                                        double novoX = Math.Cos(ang * Const.PIDiv180) * hip;

                                        (formDesenho.ObjetoNovo as TPilar).pFin.x = xIni + novoX;
                                        (formDesenho.ObjetoNovo as TPilar).pFin.y = yIni + 1;

                                        formDesenho.MouseDrawing(xIni + novoX, yIni + 1, 0);
                                    }
                                    else
                                    {
                                        string x = DigitarComando.Text.Substring(0, DigitarComando.Text.IndexOf(",")).Replace(".", ",");
                                        string y = DigitarComando.Text.Substring(DigitarComando.Text.IndexOf(",") + 1, DigitarComando.Text.Length - (x.Length) - 1).Trim().Replace(".", ",");

                                        formDesenho.MouseDrawing(-System.Convert.ToSingle(x), System.Convert.ToSingle(y), 0);
                                    }

                                }
                                else
                                {
                                  /*  double x = System.Convert.ToSingle(edCoordX.Text);
                                    double y = System.Convert.ToSingle(edCoordY.Text);
                                    double z = System.Convert.ToSingle(edCoordZ.Text);

                                    formDesenho.MouseDrawing(x, y, z);*/
                                }
                            }
                        }
                    }

                    catch (System.ArgumentOutOfRangeException)
                    {
                        MessageBox.Show("Comando inválido!\n\n Possíveis motivos: Usou vírgula como separdor decimal ? Nesse caso, use ponto ''.'' " +
                                        "\n Usou comando alfanumérico? ");
                    }
                    catch (System.FormatException)
                    {
                        MessageBox.Show("Comando inválido!\n\n Possíveis motivos: \n Usou vírgula como separdor decimal ? Nesse caso, use ponto '.' " +
                                        "\n Usou comando alfanumérico? Nesse caso, use somente números. ");
                    }
                };
        }

        private void DigitarComando_KeyPress(object sender, System.Windows.Forms.KeyPressEventArgs e)
        {

            if (e.KeyChar == (char)Keys.Enter || e.KeyChar == (char)Keys.Escape)
            {
                e.Handled = true;   // stop annoying beep
            }

            // call base handler...
            base.OnKeyPress(e);

        }
        public static bool IsNumber(Keys key)
        {
            string num = key.ToString().Substring(key.ToString().Length - 1);
            Int64 i64;
            if (Int64.TryParse(num, out i64))
            {
                return true;
            }
            return false;
        }

        private void DigitarComando_KeyUp(object sender, KeyEventArgs e)
        {
            try
            {
                if (e.KeyData == Keys.Escape)
                {
                    formDesenho.CancelaInsercoes();
                    CancelaEdicoes();
                    ElementosCopiados.Clear();
                }
                else
                    if (formDesenho.FerramentaEdicao != null)
                    {
                        if (formDesenho.IdFerramentaEdicao == Const.ID_GIRAR_ELEMENTOS)
                        {
                            if (IsNumber(e.KeyCode) && (DigitarComando.TextLength > 0))
                               AnguloRotacao = Convert.ToDouble(DigitarComando.Text.Substring(0, DigitarComando.TextLength).Replace(".", ","));
                            else
                            if (DigitarComando.TextLength == 0)
                               AnguloRotacao = 0;
                        }
                    }
                    else
                        if (e.KeyData == Keys.Enter)
                        {
                            if (DigitarComando.Text.Contains("b="))
                            {
                                string tamanho = DigitarComando.Text.Substring(DigitarComando.Text.IndexOf("=") + 1, 10).Replace(".", ",");
                                double b1 = System.Convert.ToDouble(DigitarComando.Text.Substring(DigitarComando.Text.IndexOf("=") + 1, DigitarComando.Text.Length - (tamanho.Length) - 2).Trim().Replace(".", ","));
                                formDesenho.DadosTrecho.b1 = b1;
                            }
                        }
                        else
                        {
                            if ((Object)formDesenho.ObjetoNovo != null)
                            {
                                if (formDesenho.ObjetoNovo.Command(e.KeyData))
                                    DigitarComando.Clear();

                                AtualizaDesenho();
                            }
                        }
            }
            catch(Exception)
            {
                MessageBox.Show("Valor inadequado para o comando.");
                DigitarComando.Focus();
            }
            
          //     formDesenho.ProcuraComandoXObjeto(e.KeyData);
        }
        public void FechaForms()
        {
            if (FVisGrelha != null)
                FVisGrelha.Close();
            if (FVisPortico != null)
                FVisPortico.Close();
            if (F3d != null)
                F3d.Close();
        }

        public bool ProjetoNaoEcontrado = false;
        private void NovoProjeto(bool novo)
        {
            
            if (alterado)
            {
                DialogResult dlgresult = System.Windows.Forms.MessageBox.Show(new Form { TopMost = true }, "Existem modificações que não foram salvas.\r Deseja salvar?"
                                             , "Salvar", MessageBoxButtons.YesNoCancel, MessageBoxIcon.Question);

                if (dlgresult == DialogResult.Yes)
                {
                    if (arquivoAtual == "")
                    {
                        if (Salvar(true) == false)
                            return;
                    }
                    else
                    {
                      if (Salvar(false) == false)
                       return;
                    }
               //     return;
                }
                if (dlgresult == DialogResult.Cancel)
                  return;
            }

            try
            {
                FecharJanelas();
                CriaConfiguracoesPadrao();
                CarregaConfiguracaoSnap();
                ConfiguracoesPGi.CotaFundacao = 0;
        
                this.Text = ConfiguracoesPGi.NomeProjeto;

                Salvo = false;

                arquivoAtual = "";
                formDesenho.Show(dockPanel);
                formDesenho.MdiParent = this;

                AtivaModo3D();

                if ((Inicializando && !ConfiguracaoPrograma.AbrirUltimoProjeto) || ProjetoNaoEcontrado)
                {
                    if (ProjetoNaoEcontrado) ProjetoNaoEcontrado = false;

                    formDesenho.PreparaNovoProjeto();
                    AlternaCamera(true);
                }

                if (!Inicializando)
                {
                    formDesenho.PreparaNovoProjeto();
                    AlternaCamera(true);
                }

                AtualizaDesenho();
                AtualizaComboCombinacoes();
                AtualizaComboCarga();
                CancelaDiagramas();
                CancelaResultados();
                AtualizaDesenho();
                //TTestes testes = new TTestes(this);
               // testes.testaSecao();
            }
            catch(Exception e)
            {
                MessageBox.Show("erro ao criar novo projeto: " + e.Message);
            }
        }
        public bool abrindo3D;
        
        public void Chama3D(bool tresdpavimento, bool teladividida = false)
        {

                if (F3d != null)
                    F3d.Close();

                F3d = new F3D(this, tresdpavimento, Pavimentos, tresdpavimento ? cbPiso.SelectedIndex: -1);

                if (FVisPortico != null)
                    FVisPortico.Close();
                if (FVisGrelha != null)
                    FVisGrelha.Close();
       
               // if (!ConfiguracoesPGi.F3DOpcoesVisualizacao.TelaLiberada)
                  F3d.MdiParent = this;

                if (tresdpavimento)
                {
                    F3d.WindowState = System.Windows.Forms.FormWindowState.Maximized;
               //     F3d.WindowState = System.Windows.Forms.FormWindowState.Normal;
                }
                else
                    F3d.WindowState = System.Windows.Forms.FormWindowState.Maximized;
               
                abrindo3D = true;

                if (teladividida)
                {
     
                    F3d.Show(dockPanel, WeifenLuo.WinFormsUI.Docking.DockState.DockRight);
                 //   formDesenho.Pane.Width = 350;
                   // formDesenho.Height = this.ClientSize.Height - 80;
                   // formDesenho.Top = 0;
                   // F3d.PanelPane.Width = 500;
                   // F3d.Left = formDesenho.Width;
                   // F3d.Height = this.ClientSize.Height - 80;
                   // F3d.Top = 0;
                }
                else
                    F3d.Show(dockPanel);
           
             /*   if (!ConfiguracoesPGi.F3DOpcoesVisualizacao.TelaLiberada && teladividida)
                {
                    formDesenho.Width = this.Width / 2 + 100;
                    formDesenho.Height = this.ClientSize.Height - 80;
                    formDesenho.Top = 0;

                    F3d.Width = this.Width / 2 - 150;
                    F3d.Left = formDesenho.Width;
                    F3d.Height = this.ClientSize.Height - 80;
                    F3d.Top = 0;

                    F3d.SendToBack();
                    F3d.BringToFront();
                }
                else
                if (ConfiguracoesPGi.F3DOpcoesVisualizacao.TelaLiberada)
                {
                    F3d.Width = formDesenho.Width-60;
                    F3d.Left = formDesenho.Left+30;
                    F3d.Height = formDesenho.Height-100;
                    F3d.Top = formDesenho.Top + 150;
                }*/

                F3d.GerarModelo3D(true);
                abrindo3D = false;

        }

        public bool TresDPavimento;

        /*public void MsgCalculo(string titulo, string texto, int max)
        {
            PanelCalculo.Visible = true;
            Progresso.Visible = true;
            Progresso.Value = 0;
            Progresso.Maximum = System.Convert.ToInt32(max);

            //   lbTitulo.Text    = "Calculando...";
            lbProgresso.Text = texto;
            Refresh();
        }*/

        public void MsgCalculo(string titulo, string texto, int max, bool fim = false, bool erro = false)
        {
        //    BringToFront(); 
            
           // Progresso3.Value       = 0;
           // if (fim)
            //  Progresso3.Visible = false;
          //  Progresso3.Maximum   = System.Convert.ToInt32(max);
    
            if (erro)
            {
             //   lbErro.Visible = true;
              //  lbErro.Text = texto;
            }
            else
                lbProgresso.Text = texto;

            Application.DoEvents();
            Update();
        }

      /*  public void HistoricoCalculo(string texto, bool Edit = false)
        {
            if (Edit)
                ListaCalculo.Items[ListaCalculo.Items.Count - 1] = texto;
            else
                ListaCalculo.Items.Insert(ListaCalculo.Items.Count, texto);

            ListaCalculo.SelectedIndex = ListaCalculo.Items.Count - 1;
            Refresh();
        }*/

        public void HistoricoCalculo(string texto, bool Edit = false)
        {
            if (Edit)
                processo.ListaCalculo.Items[processo.ListaCalculo.Items.Count - 1] = texto;
            else
                processo.ListaCalculo.Items.Insert(processo.ListaCalculo.Items.Count, texto);

            processo.ListaCalculo.SelectedIndex = processo.ListaCalculo.Items.Count - 1;

            //Progresso3.Value = 0;
           // Progresso3.Visible = false;

           // Refresh();
        }

        private void menuItem7_Click(object sender, EventArgs e)
        {
            NovoProjeto(true);
        }


        string arg = string.Empty;
        DialogResult result = DialogResult.Yes;

        public bool Salvo;
        public string arquivoAtual;
        List<sObjSerializar> VigasSerializar_, PilaresSerializar_, BarrasSerializar_;
        List<TSecao> SecoesSerializar_;
        List<TLaje> LajesSerializar_;
       // sLayerSerializar LajesSerializar_;
        TEstrutura EstruturaSerializar_;
        public List<string> ArquivosRecentes = new List<string>();
        string nomePasta = string.Empty;
        public bool Salvar(bool SalvarComo = false)
        {         
            string filename = arquivoAtual;
            FileStream arquivo;
            BinaryFormatter arqBin = new BinaryFormatter();
            
            DialogResult result;
            
            result = DialogResult.OK;
            if (filename == null || filename == "" ||  SalvarComo)
            {
                using (SaveFileDialog fileChooser = new SaveFileDialog())
                {
                    fileChooser.CheckFileExists = false;
                    fileChooser.FileName        = ConfiguracoesPGi.NomeProjeto;
                    result                      = fileChooser.ShowDialog();
                    filename                    = fileChooser.FileName;
                }
            }

            if (result == DialogResult.OK)
            {
                if (filename == string.Empty)
                    MessageBox.Show("Arquivo inválido", "Erro", MessageBoxButtons.OK, MessageBoxIcon.Error);
                else
                {
                    try
                    {
                        try
                        {
                            if (filename != null)
                            {
                                if (!ArquivosRecentes.Exists(o => o == filename))
                                  ArquivosRecentes.Add(filename);
                                else
                                {
                                    int iii = ArquivosRecentes.FindIndex(o => o == filename);
                                    ArquivosRecentes.RemoveAt(iii);
                                    ArquivosRecentes.Add(filename);
                                }

                                arquivo = new FileStream(Directory.GetCurrentDirectory() + @"\recentes", FileMode.OpenOrCreate, FileAccess.Write);
                                arqBin.Serialize(arquivo, ArquivosRecentes);
                                arquivo.Close();
                            }
                            pbAguardar.Value = 0;
                            pbAguardar.Maximum = 11;

                            ChamaAguardar(this, "Salvando. Aguarde...");

                            Application.DoEvents();

                          /*  if (Directory.Exists(filename + "\\bkp"))
                            {
                                Directory.Delete(filename + "\\bkp");
                            }
                            Directory.CreateDirectory(filename + "\\bkp");

                            if (Directory.Exists(filename))
                            {
                                Directory.Delete(filename, true);
                            }*/

                           // Directory.CreateDirectory(filename);

                            arquivoAtual = filename;
                            BarrasSerializar_ = new List<sObjSerializar>();
                            SecoesSerializar_ = new List<TSecao>();
                            
                            formDesenho.SetaSelecionados(false,-1);

                            EstruturaSerializar_ = formDesenho.Estrutura;
                            EstruturaSerializar_.x_rot_angle = (float)formDesenho.camera.x_rot_angle;
                            EstruturaSerializar_.y_rot_angle = (float)formDesenho.camera.y_rot_angle;
                            EstruturaSerializar_.x_trans = (float)formDesenho.camera.x_trans;
                            EstruturaSerializar_.y_trans = (float)formDesenho.camera.y_trans;
                            EstruturaSerializar_.z_trans = (float)formDesenho.camera.z_trans;

                            EstruturaSerializar_.cameraOrto = formDesenho.CameraOrto;

                            pbAguardar.Value += 1;Application.DoEvents();

                            SecoesSerializar_.Clear();
                            foreach (TSecao o in formDesenho.Secoes)
                                SecoesSerializar_.Add(o);

                            nomePasta = filename;
                            TArquivo ArquivoFinal =new TArquivo( formDesenho.Estrutura.apoios,
                                                                 SecoesSerializar_,
                                                                 EstruturaSerializar_,
                                                                 formDesenho.Estrutura.cargaPontual,
                                                                 formDesenho.Estrutura.cargaLinear,
                                                                 formDesenho.CasosCarga,
                                                                 formDesenho.Estrutura.combinacoes,
                                                                 formDesenho.Estrutura.barras,
                                                                 ConfiguracoesPGi);
                          
                            arquivo = new FileStream(nomePasta, FileMode.OpenOrCreate, FileAccess.Write);
                            arqBin.Serialize(arquivo, ArquivoFinal);
                            arquivo.Close();

                            this.Text = "PGi - [" + arquivoAtual + "]";
                            alterado = false;
                            arquivo.Dispose();
                        }
                        catch (Exception e)
                        {
                            formDesenho.Comando.Text = "";
                            MessageBox.Show(e.Message, "Erro", System.Windows.Forms.MessageBoxButtons.OK, MessageBoxIcon.Error);
                        }

                    }
                    catch (SerializationException)
                    {
                        MessageBox.Show("Erro ao escrever o arquivo", "Erro", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    }
                    catch (IOException)
                    {
                        MessageBox.Show("Erro ao acessar o aqruivo", "Erro", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    }
                    
                    formDesenho.Comando.Text = "";
                    FechaAguardar();
                    AtualizaDesenho();
                }

                return true;
            }
         
            return false;
        }
        //    Poligono = new TPoligono(cbTipo.Text, edh1.Text, edb1.Text, edb2.Text, edh2.Text, "","",false);
        //formDesenho.CommandDraw(button.Tag.ToString(), arg);
        [Serializable]
        struct sObjSerializar
        {
            public ISettings dados;
            public CoordenadaD pi, pf;
            public TTexto t1, t2;
            public int cont;
            
            public sObjSerializar(ISettings d, CoordenadaD p1, CoordenadaD p2, TTexto tit1, TTexto tit2, int _cont)
            {
                this.dados = d;
                this.pi = p1;
                this.pf = p2;
                this.t1 = tit1;
                this.t2 = tit2;
                cont = _cont;
            }
        }
        [Serializable]
        struct sLayerSerializar
        {
            public TLayer main, textos;
            public sLayerSerializar(TLayer main, TLayer textos)
            {
                this.main = main;
                this.textos = textos;
            }
        }
        void FecharJanelas()
        {
            FormCollection fc = Application.OpenForms;
            List<Form> forms = new List<Form>();
  
            foreach (Form frm in fc)
                if (frm != null)
                    if ((string)frm.Tag != "Principal" && (string)frm.Tag != "Splash")
                        forms.Add(frm);
  
            foreach (Form frm in forms)
                frm.Close();
        }

        public bool GrelhaPrimeiraVez;
        public void Abrir(string caminho = "", bool abrindoPrograma = false)
        {            
            if (alterado)
            {
                DialogResult dlgresult = System.Windows.Forms.MessageBox.Show(new Form { TopMost = true }, "Existem modificações que não foram salvas.\r Deseja salvar?"
                                             , "Salvar", MessageBoxButtons.YesNoCancel, MessageBoxIcon.Question);

                if (dlgresult == DialogResult.Yes)
                {
                    if (arquivoAtual == "")
                    {
                        if (Salvar(true) == false)
                            return;
                    }
                    else
                    {
                        if (Salvar(false) == false)
                            return;
                    }
                   // return;
                }
                if (dlgresult == DialogResult.Cancel)
                    return;

            }

            string filename;

            DialogResult result = DialogResult.OK;
     
            if (caminho != "")
              filename = caminho;
            else
            {
              result   = openFileDialog1.ShowDialog();
              filename = openFileDialog1.FileName;
            }
  
            if (result == DialogResult.OK)
            {
                try
                {
                    FecharJanelas();

                    this.Cursor = Cursors.WaitCursor;
                    if (formDesenho != null) formDesenho.Update();
                    FileStream arquivo;
                    BinaryFormatter arqBin = new BinaryFormatter();
                    if (formDesenho != null) formDesenho.Cursor = System.Windows.Forms.Cursors.WaitCursor;

                    if (!abrindoPrograma)
                    {
                        if (formDesenho != null)
                            formDesenho.Close();

                        if (desenho != null)
                            formDesenho.Close();

                        CriaFormDesenho();

                        formDesenho.Show(dockPanel);
                        formDesenho.MdiParent = this;
                    }

                    //  GL.Clear(ClearBufferMask.ColorBufferBit | ClearBufferMask.DepthBufferBit);
                    //  formDesenho.glControl.SwapBuffers();
                    PanelAguarde.Visible = false;
                    Application.DoEvents();
                    this.Refresh();
                    pbAguardar.Value = 0;
                    pbAguardar.Maximum = 9;

                    ChamaAguardar(this, "Abrindo. Aguarde...");

                    //      File.SetAttributes(filename, FileAttributes.Normal);

                    //       File.Copy(filename, filename  + "_bkp");

                  
                    arquivoAtual = filename;

                    arquivo = new FileStream(filename , FileMode.Open, FileAccess.Read);

                    TArquivo arquivoProjeto = (TArquivo)arqBin.Deserialize(arquivo);

                    EstruturaSerializar_ = arquivoProjeto.estrutura;
                    EstruturaSerializar_.CriaListas(this);
                    formDesenho.Estrutura = EstruturaSerializar_;

                    ConfiguracoesPGi = arquivoProjeto.cfg;

                    ////

                 /*   arquivo = new FileStream(filename + "\\@ModEstrutura", FileMode.Open, FileAccess.Read);
                    EstruturaSerializar_ = (TEstrutura)arqBin.Deserialize(arquivo);
                    EstruturaSerializar_.CriaListas(this);
                    formDesenho.Estrutura = EstruturaSerializar_;
                    arquivo.Close();

                    arquivo = new FileStream(filename + "\\@PgiConfiguracoes", FileMode.Open, FileAccess.Read);
                    ConfiguracoesPGi = (TConfiguracoesPGi)arqBin.Deserialize(arquivo);
                    arquivo.Close();
                    */
                    BarrasSerializar_ = new List<sObjSerializar>();

                    formDesenho.AbrindoArquivo = true;

                    string arq;

                    SecoesSerializar_ = new List<TSecao>();
                    SecoesSerializar_ = arquivoProjeto.secoes;

                   /* arq = filename + "\\@ModSecoes";
                    if (File.Exists(arq))
                    {
                        arquivo = new FileStream(arq, FileMode.Open, FileAccess.Read);
                        SecoesSerializar_ = (List<TSecao>)arqBin.Deserialize(arquivo);
                        arquivo.Close();
                    }*/
                    pbAguardar.Value += 1; Application.DoEvents();

                    formDesenho.LimpaTela();
                    pbAguardar.Value += 1; Application.DoEvents();

                    try
                    {
                        formDesenho.CasosCarga = arquivoProjeto.casos;
                        /*arq = filename + "\\@ModCasos";
                        arquivo = new FileStream(arq, FileMode.Open, FileAccess.Read);
                        formDesenho.CasosCarga = (List<TCasosCarga>)arqBin.Deserialize(arquivo);
                        arquivo.Close();*/

                        formDesenho.Estrutura.barras = arquivoProjeto.barras;
                        pbAguardar.Value += 1; Application.DoEvents();
                      /*  arq = filename + "\\@ModBarras";
                        arquivo = new FileStream(arq, FileMode.Open, FileAccess.Read);
                        formDesenho.Estrutura.barras = (List<TBarraGenerica>)arqBin.Deserialize(arquivo);*/
     
                        // formDesenho.Estrutura.barras = formDesenho.Barras;
                  
                          foreach (TSecao s in SecoesSerializar_)
                          {
                              formDesenho.Secoes.Add(s);
                          }

                        foreach (var ponto in formDesenho.Estrutura.nos)
                            ponto.BarrasConectadas.Clear();
                        
                        foreach (TBarraGenerica b in formDesenho.Estrutura.barras)
                        {
                              b.DirtySelecao = true;
                              b.DirtyTriangulos = true;
                              formDesenho.Barras.Add(b);
                              formDesenho.AddLinha(b.Linha_Eixo, -1, false, false, false, null, -1, false);
                              formDesenho.Linhas[formDesenho.Linhas.Count - 1].IDBarra = b.IDBarra;
                              formDesenho.Linhas[formDesenho.Linhas.Count - 1].Barra = b;
                           
                              b.pIni.BarrasConectadas = new List<TBarraGenerica>();
                              b.pFin.BarrasConectadas = new List<TBarraGenerica>();

                              b.pIni.BarrasConectadas.Add(b);
                              b.pFin.BarrasConectadas.Add(b);
                              b.DirtySelecao = true;
                              b.DirtyTriangulos = true;
                              b.DirtyArestas = true;
                              b.Visivel = true;
                        }


                        /*   pbAguardar.Value += 1; pbAguardar.Refresh();
                           arq = filename + "\\@ModApoios";
                             arquivo = new FileStream(arq, FileMode.Open, FileAccess.Read);
                             formDesenho.Estrutura.apoios = (List<TApoio>)arqBin.Deserialize(arquivo);
                             arquivo.Close();*/
                        formDesenho.Estrutura.apoios = arquivoProjeto.apoios;
                          formDesenho.Estrutura.cargaLinear = arquivoProjeto.cargalinear;
                          formDesenho.Estrutura.combinacoes = arquivoProjeto.combinacoes;

                        /*arq = filename + "\\@ModCargasLineares";
                      arquivo = new FileStream(arq, FileMode.Open, FileAccess.Read);
                      formDesenho.Estrutura.cargaLinear = (List<TCargaLinear>)arqBin.Deserialize(arquivo);
                      arquivo.Close();*/

                        foreach (TCargaLinear c in formDesenho.Estrutura.cargaLinear)
                             formDesenho.CargasLineares.Add(c);           

                        pbAguardar.Value += 1; pbAguardar.Refresh();

                        /*arq = filename + "\\@ModCargasNodais";
                          arquivo = new FileStream(arq, FileMode.Open, FileAccess.Read);
                          formDesenho.Estrutura.cargaPontual = (List<TCargaPontual>)arqBin.Deserialize(arquivo);*/

                        formDesenho.Estrutura.cargaPontual = arquivoProjeto.cargapontual;


                        foreach (TCargaPontual c in formDesenho.Estrutura.cargaPontual)
                            formDesenho.CargasPontuais.Add(c);

                          arquivo.Close();
                      }
                      catch (Exception ee)
                      {
                          MessageBox.Show("Erro ao abrir vigas: " + ee.Message);
                      }

                    Application.DoEvents();
                    this.Refresh();
                    ChamaAguardar(this, "Atualizando elementos. Aguarde...");

                    // AtualizaPavimentos();

                    formDesenho.Estrutura.AtualizaListaObjetos();

                    formDesenho.InsereNos();//necessario inserir novamente os nos, pois eu nao quero salvar eles pra nao pesar o arquivo
                    
                    GrelhaPrimeiraVez = true;
                    

                    if (!ArquivosRecentes.Exists(o => o == openFileDialog1.FileName))
                      ArquivosRecentes.Add(openFileDialog1.FileName);

                    arquivo = new FileStream(Directory.GetCurrentDirectory() + @"\recentes", FileMode.OpenOrCreate, FileAccess.Write);
                    arqBin.Serialize(arquivo, ArquivosRecentes);
                    arquivo.Close();

                    pbAguardar.Value += 1;

                   /* formDesenho.Atualiza_pIni_pFin_das_Barras_e_Cargas();
                    formDesenho.AtualizaBarras(true);
                    formDesenho.MostrarTudo();
                    */
                    this.Text = "PGi - [" + arquivoAtual + "]";

                    formDesenho.camera.x_rot_angle = 135;
                    formDesenho.camera.y_rot_angle = 225;

                    formDesenho.camera.x_trans = EstruturaSerializar_.x_trans;
                    formDesenho.camera.y_trans = EstruturaSerializar_.y_trans;
                    formDesenho.camera.z_trans = EstruturaSerializar_.z_trans;
                    formDesenho.camera.x_ang_rad = (formDesenho.camera.x_rot_angle * (float)Math.PI) / 180;
                    formDesenho.camera.z_ang_rad = (formDesenho.camera.y_rot_angle * (float)Math.PI) / 180;
                    formDesenho.CameraOrto  = EstruturaSerializar_.cameraOrto;
                    formDesenho.Unifilar     = ConfiguracoesPGi.F3DOpcoesVisualizacao.Unifilar;

                    formDesenho.MostrarNos = ConfiguracoesPGi.F3DOpcoesVisualizacao.Nos;
                    MudaStatusBotao(btMostrarNos);

                    formDesenho.Arestas = ConfiguracoesPGi.F3DOpcoesVisualizacao.Arestas;
                    CarregaConfiguracaoSnap();

                    formDesenho.SetupCamera(false);

                    formDesenho.AtualizaConfiguracoes3D(false,false);
                    formDesenho.AtualizaConfiguracaoDiagramas();


                    formDesenho.Atualiza_pIni_pFin_das_Barras_e_Cargas();
                    formDesenho.AtualizaBarras(true);
                    formDesenho.MostrarTudo();

                    formDesenho.MostrarCargas = false;
                    MudaStatusBotao(btMostrarEixosLocais);
                    MudaStatusBotao(btVisualizarCargas);

                    AtualizaComboCarga();
                    AtualizaComboCombinacoes();

                    arquivo.Dispose();
                    if (!abrindoPrograma)
                        formDesenho.Enquadrar();
                    //formDesenho.AtualizaDisplayList_Nos();
                    alterado = false;

                    pbAguardar.Value += 1;
                    formDesenho.ApagaElementosDesnecessarios();

                    if ((string)btDeformacao.Tag == "1")
                        btDeformacao_Click(btDeformacao, null);

                    if ((string)btTensoes.Tag == "1")
                        btTensoes_Click(btTensoes, null); 
                    
                    if ((string)btDiagrama.Tag == "1")
                        btDiagrama_Click(btDiagrama, null);

                    if ((string)btDinamica1.Tag == "1")
                        btDinamica1_Click(btDinamica1, null);

                    cbModosVibracao.Items.Clear();
                    cbModosFlambagem.Items.Clear();

                    /*  if ((string)btVisualizarCargas.Tag == "1")
                      {
                          formDesenho.MostrarCargas = false;
                          btVisualizarCargas_Click(btVisualizarCargas, null);
                      }*/

                    Ribbon.SelectedTab = tabPrincipal;
                    formDesenho.AtualizaListaSnap();

                    FechaAguardar();
                   
                }
                catch ( Exception e)
                {
                    FechaAguardar();
                    MessageBox.Show("Erro ao abrir arquivo: " + e.Message);

                }
                formDesenho.AbrindoArquivo = false;
                this.Cursor = Cursors.Arrow;
                formDesenho.glControl.Focus();
                AtualizaDesenho();
            }
        }

        private void menuItem9_Click(object sender, EventArgs e)
        {
            Salvar();
        }

        private void menuItem8_Click(object sender, EventArgs e)
        {
            Abrir();
        }

        private void menuItem11_Click_1(object sender, EventArgs e)
        {
            Application.Exit();
        }

        private void menuItem41_Click(object sender, EventArgs e)
        {
            formDesenho.AtualizarPixels();
        }

        private void Gerenciador_FormClosing(object sender, FormClosingEventArgs e)
        {
            if (alterado)
            {
           //     DialogResult dlgresult =  MessageBox.Show("Existem modificações que não foram salvas.\r Deseja salvar?"
        //                                    , "Salvar", MessageBoxButtons.YesNoCancel, MessageBoxIcon.Question);

                DialogResult dlgresult = System.Windows.Forms.MessageBox.Show(new Form { TopMost = true }, "Existem modificações que não foram salvas.\r Deseja salvar?"
                                            , "Salvar", MessageBoxButtons.YesNoCancel, MessageBoxIcon.Question);

                if (dlgresult == DialogResult.Yes)
                {
                    if (arquivoAtual == "")
                    {
                        if (Salvar(true) == false)
                            e.Cancel = true;
                    }
                    else
                    {
                        if (Salvar(false) == false)
                            return;
                    }
                }

                if (dlgresult == DialogResult.Cancel)
                    e.Cancel = true;
            }
            SalvarPanels();
        }
        [Serializable]
        struct EstadoPanels
        {
            public string panel;
            public int left, top;
            public EstadoPanels(string p, int l, int t)
            {
                panel = p;
                left = l;
                top = t;
            }
        }
        List<EstadoPanels> estadoPanels;
        void SalvarPanels()
        {
            estadoPanels = new List<EstadoPanels>();
            estadoPanels.Add(new EstadoPanels(pnCargasCombinacoes.Name, pnCargasCombinacoes.Left, pnCargasCombinacoes.Top));
            estadoPanels.Add(new EstadoPanels(pnUtilitarios.Name, pnUtilitarios.Left, pnUtilitarios.Top));
            estadoPanels.Add(new EstadoPanels(pnCorResultados.Name, pnCorResultados.Left, pnCorResultados.Top));
            FileStream arquivo;
            BinaryFormatter arqBin = new BinaryFormatter();

            arquivo = new FileStream(Directory.GetCurrentDirectory() + @"\estadopanels", FileMode.OpenOrCreate, FileAccess.Write);

            arqBin.Serialize(arquivo, estadoPanels);
            arquivo.Close();
        }

        public void SetaVisibilidadeHolders(List<ToolBarDockHolder> lh, bool visible)
        {
            lh.ForEach(obj => obj.Visible = visible);
        }


        private void tbVisGrelha_ButtonClick(object sender, ToolBarButtonClickEventArgs e)
        {
            if (!FVisGrelha.Visible) return;
            
            if (e.Button == btVisGrelhaDeslocamento)
            {
                FVisGrelha.iEsforcoAtual       = FVisualizadorGrelha.iDeslocamento;
                FVisGrelha.PanelCores2.Visible = ConfiguracoesPGi.GrelhaOpcoesVisualizacao.Diagramas_DeslocamentoGradiente;
            }

            if (e.Button == btVisGrelhaFletor)
                FVisGrelha.iEsforcoAtual = FVisualizadorGrelha.iFletor;

            if (e.Button == btVisGrelhaTorcor)
                FVisGrelha.iEsforcoAtual = FVisualizadorGrelha.iTorcor;

            if (e.Button == btVisGrelhaCortante)
                FVisGrelha.iEsforcoAtual = FVisualizadorGrelha.iCortante;

            if (FVisGrelha.iEsforcoAtual != FVisualizadorGrelha.iDeslocamento)
                FVisGrelha.PanelCores2.Visible = ConfiguracoesPGi.GrelhaOpcoesVisualizacao.Diagramas_Gradiente;

            if (e.Button == btVisGrelhaOpc)
            {
                menuItem43_Click(sender, null);
            }
            if (e.Button == btVisGrelhaAtualizar)
            {
                FVisGrelha.Atualizar();
            }
            if (e.Button == btVisGrelhaVistaCima)
            {
                FVisGrelha.VistaCima();
            }
            if (e.Button == btVisGrelhaVistaPerspectiva)
            {
                FVisGrelha.VistaPerspectiva();
            }


            FVisGrelha.AtualizaFormCores();
            FVisGrelha.Render();
         //   TOpenGl.OGL.Refresh();
        }

        void DiagramasGrelha(object sender)
        {
            try
            {
                if (FVisGrelha == null) return;
                if (!FVisGrelha.Visible) return;

                if ((sender as RibbonButton) == btGrelhaDz)
                {
                    FVisGrelha.iEsforcoAtual = FVisualizadorGrelha.iDeslocamento;
                    FVisGrelha.PanelCores2.Visible = ConfiguracoesPGi.GrelhaOpcoesVisualizacao.Diagramas_DeslocamentoGradiente;
                }

                if ((sender as RibbonButton) == btGrelhaMY)
                    FVisGrelha.iEsforcoAtual = FVisualizadorGrelha.iFletor;

                if ((sender as RibbonButton) == btGrelhaMX)
                    FVisGrelha.iEsforcoAtual = FVisualizadorGrelha.iTorcor;

                if ((sender as RibbonButton) == btGrelhaFZ)
                    FVisGrelha.iEsforcoAtual = FVisualizadorGrelha.iCortante;

                if (FVisGrelha.iEsforcoAtual != FVisualizadorGrelha.iDeslocamento)
                    FVisGrelha.PanelCores2.Visible = ConfiguracoesPGi.GrelhaOpcoesVisualizacao.Diagramas_Gradiente;

                if ((sender as RibbonButton) == btGrelhaOpc)
                {
                    menuItem43_Click(sender, null);
                }
                if ((sender as RibbonButton) == btGrelhaAtualizar)
                {
                    FVisGrelha.Atualizar();
                }
                if ((sender as RibbonButton) == btGrelhaVistaCima)
                {
                    FVisGrelha.VistaCima();
                }
                if ((sender as RibbonButton) == btGrelhaVistaPerspectiva)
                {
                    FVisGrelha.VistaPerspectiva();
                }

                FVisGrelha.AtualizaFormCores();
                FVisGrelha.Render();
               // TOpenGl.OGL.Refresh();
            }
            catch (Exception e)
            {
                MessageBox.Show("Erro ao atualizar diagramas na grelha: " + e.Message);
            }
        }

        public void ChamaAguardar(Form Owner, string Texto, bool mostraprogresso = true)
        {
            Application.DoEvents();
          /*  if (!mostraprogresso)
                metroProgressSpinner1.Visible = false;
            else
                metroProgressSpinner1.Visible = true;*/
            Application.DoEvents();

           pbAguardar.Update();
           PanelAguarde.Refresh();
           formDesenho.Refresh();
            Application.DoEvents();
            this.Refresh();
           
           LabelAguarde.Text = Texto;
    
           PanelAguarde.Width = 300;
           PanelAguarde.Height = 40;
           PanelAguarde.Left = this.Width  / 2 - PanelAguarde.Width/2;
           PanelAguarde.Top = this.Height - PanelAguarde.Height-100;

           LabelAguarde.Left = PanelAguarde.Width / 2 - LabelAguarde.Width / 2;
           LabelAguarde.Top  = PanelAguarde.Height / 2 - LabelAguarde.Height/2;

            PanelAguarde.Visible = true;
            PanelAguarde.Refresh();
           formDesenho.Refresh();
           Application.DoEvents();
           this.Refresh();
        }

        public void FechaAguardar()
        {
            PanelAguarde.Visible = false;
            AtualizaDesenho();
        }

        private void menuItem43_Click(object sender, EventArgs e)
        {
         //   if ((string)mmItemVisualizacao.Tag == "GRELHA")
            {
                string arg = string.Empty;
      
                DialogResult result = DialogResult.Yes;
                {
                    if (!FOpcVisGrelha.Visible)
                        FOpcVisGrelha = new FGrelhaOpcoesVisualizacao(this);

                    result = FOpcVisGrelha.ShowDialog(this);
                }

                if (result == DialogResult.Yes)
                  FVisGrelha.AtualizaEsforcos("Atualizando grelha. Aguarde...");

                if (FVisGrelha.iEsforcoAtual == FVisualizadorGrelha.iDeslocamento)
                  FVisGrelha.PanelCores2.Visible = ConfiguracoesPGi.GrelhaOpcoesVisualizacao.Diagramas_DeslocamentoGradiente;
                else
                if (FVisGrelha.iEsforcoAtual != FVisualizadorGrelha.iDeslocamento)
                    FVisGrelha.PanelCores2.Visible = ConfiguracoesPGi.GrelhaOpcoesVisualizacao.Diagramas_Gradiente;
                
                /*    if (result == DialogResult.Yes)
                    {
                        formDesenho.CommandDraw(button.Tag.ToString(), arg);
                        formDesenho.Settings = formDesenho.DadosTrecho;
                        cbLayers.SelectedIndex = cbLayers.Items.IndexOf("Lajes");
                        pnCorLayer.BackColor = Color.Green;
                        formDesenho.ActiveLayer = formDesenho.LayersById[cbLayers.Text];
                    }
                    */
            }
        }

        private void btMostraPanelForm_MouseMove(object sender, System.Windows.Forms.MouseEventArgs e)
        {

        }

        public bool recalculou;
        DateTime HoraInicio;
        bool MateriaisOk()
        {
            foreach (TPavimento p in Pavimentos)
                if (p.classeLaje == null || p.classeViga == null || p.classePilar == null)
                    return false;

            return true;
        }

        bool VerificaCfg()
        {
            if (!MateriaisOk())
                return false;

            return true;
        }

        bool VerificacoesPreCalculo()
        {
            ChamaAguardar(this, "Verificando a estrutura. Aguarde...", false);

            if (formDesenho.Barras.Count == 0)
            {          
                MessageBox.Show("A estrutura não possui barras.", "Atenção", System.Windows.Forms.MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                return false;
            }
            
            if (formDesenho.Estrutura.apoios.Count == 0)
            {
                
                MessageBox.Show("A estrutura não possui nenhum apoio. \nA estrutura não pode ser calculada.", "Atenção", System.Windows.Forms.MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                return false;
            }
            Application.DoEvents();
            formDesenho.Estrutura.DeletaElementosComTamanhoZero();
            Application.DoEvents();
            string resposta = "";
           /* if (ConfiguracoesPGi.CfgProjeto.sistema.Interromper_calculo_conexao_perdida)
            {
                if (formDesenho.Estrutura.TemNosPerdidos(ref resposta, ConfiguracoesPGi.CfgProjeto.sistema.toleranciaConexaoPerdida))
                {
                  
                    MessageBox.Show(resposta, "Atenção", System.Windows.Forms.MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                    return false;
                }
            }*/
            Application.DoEvents();
            if (ConfiguracoesPGi.CfgProjeto.sistema.Interromper_calculo_elementos_sobrepostos)
            {
                if (formDesenho.Estrutura.TemElementoSobreposto(ref resposta))
                {
              
                    MessageBox.Show(resposta, "Atenção", System.Windows.Forms.MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                    return false;
                }
            }
            Application.DoEvents();
            if (formDesenho.Estrutura.TemElementoAvulso(ref resposta))
            {
              
                MessageBox.Show(resposta, "Atenção", System.Windows.Forms.MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                return false;
            }
            Application.DoEvents();
           
            if (formDesenho.Estrutura.TemElementosArticuladosEmBalanco(ref resposta))
            {
                MessageBox.Show(resposta, "Atenção", System.Windows.Forms.MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                return false;
            }

            FechaAguardar();
            return true;
          /*  if (!VerificaCfg())
            {
                MessageBox.Show("Os materiais não foram totalmente configurados. Verifique em 'Configurações->Projeto->Materiais'", "Atenção", System.Windows.Forms.MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                return;
            }*/


        }
        double escalaDeformacao;
        double escalaDiagrama, escalaModoVibracao,escalaFlambagem;
        public FProcessoCalculo processo;

        public void CalcularEscalaDeformacoes()
        {
            if ((cbTipoCargaResultado.SelectedIndex == 0 && cbResultadoCasos.SelectedIndex > -1)
                ||
               (cbTipoCargaResultado.SelectedIndex == 1 && cbResultadoCombinacoes.SelectedIndex > -1))
            {
                double maxDef = formDesenho.Estrutura.PorticoEspacial.MaximoDeslocamento(
                                                           cbTipoCargaResultado.SelectedIndex,
                                                           cbResultadoCasos.SelectedIndex,
                                                           cbResultadoCombinacoes.SelectedIndex);
                if (maxDef != 0)
                {
                    escalaDeformacao = (int)(0.5 / maxDef);
                    if (escalaDeformacao == 0)
                    {
                        formDesenho.fatorDeformacao = 0.5;
                        edEscalaDeformacao.Text = formDesenho.fatorDeformacao.ToString("n2");
                        edEscalaTensao.Text = formDesenho.fatorDeformacao.ToString("n2");

                    }
                    else
                    {
                        edEscalaDeformacao.Text = escalaDeformacao.ToString();
                        edEscalaTensao.Text = escalaDeformacao.ToString();
                        formDesenho.fatorDeformacao = escalaDeformacao;
                    }

                    escalaDef_anterior = 1;
                    escalaDiagrama_anterior = 1;
                }
            }
        }
        public void CalcularEscalaModosFlambagem(int id_combinacao)
        {
            var portico = formDesenho.Estrutura.PorticoEspacial;
            if (!ConfiguracoesPGi.CfgProjeto.sistema.CalculaModosFlambagem || portico == null)
                return;

            var resultado = portico.ResultadosFlambagem.Find(r => r.EhCombinacao && r.IdReferencia == id_combinacao);
            if (resultado?.Modos == null || resultado.Modos.GetLength(1) == 0)
                return;

            int modo = cbModosFlambagem.SelectedIndex;
            if (modo < 0 || modo >= resultado.Modos.GetLength(1))
            {
                modo = 0;
                if (cbModosFlambagem.Items.Count > 0)
                    cbModosFlambagem.SelectedIndex = modo;
            }

            double maximo = 0.0;
            for (int i = 1; i <= portico.nBarras; i++)
            {
                double[] maximos;
                var maximosPorCombinacao = portico.barras[i].MaximosDeslocamentosFlambagem;
                if (maximosPorCombinacao != null && maximosPorCombinacao.TryGetValue(id_combinacao, out maximos) && maximos != null && modo < maximos.Length)
                    maximo = Math.Max(maximo, maximos[modo]);
            }

            // Mesmo deslocamento visual alvo do desenho modal: 0,5 unidade.
            escalaFlambagem = maximo > 0.0 ? 0.5 / maximo : 1.0;
            if (double.IsNaN(escalaFlambagem) || double.IsInfinity(escalaFlambagem))
                escalaFlambagem = 1.0;
            vEscalaFlambagem = escalaFlambagem;
            formDesenho.fatorFlambagem = escalaFlambagem;
            formDesenho.EscalaFlambagem = escalaFlambagem;
            edEscalaFlambagem.Text = escalaFlambagem.ToString("n3");
            formDesenho.DirtyPortico();
        }

        public void CalcularEscalaModosVibracao()
        {
            var portico = formDesenho.Estrutura.PorticoEspacial;
            if (!ConfiguracoesPGi.CfgProjeto.sistema.CalculaModosVibracao ||
                portico?.ModosVibracao == null || portico.ModosVibracao.GetLength(1) == 0)
                return;

            int modo = cbModosVibracao.SelectedIndex;
            if (modo < 0 || modo >= portico.ModosVibracao.GetLength(1))
            {
                modo = 0;
                if (cbModosVibracao.Items.Count > 0)
                    cbModosVibracao.SelectedIndex = modo;
            }

            double maximo = 0.0;
            for (int i = 1; i <= portico.nBarras; i++)
            {
                double[] maximos = portico.barras[i].MaximosDeslocamentosModais;
                if (maximos != null && modo < maximos.Length)
                    maximo = Math.Max(maximo, maximos[modo]);
            }

            // Mesmo deslocamento visual alvo das deformações: 0,5 unidade.
            // Não truncar: modos normalizados pela massa podem exigir escala < 1.
            escalaModoVibracao = maximo > 0.0 ? 0.5 / maximo : 1.0;
            if (double.IsNaN(escalaModoVibracao) || double.IsInfinity(escalaModoVibracao))
                escalaModoVibracao = 1.0;
            vEscalaModoVibracao = escalaModoVibracao;
            formDesenho.fatorModoVibracao = escalaModoVibracao;
            formDesenho.EscalaModoVibracao = escalaModoVibracao;
            edEscalaModo.Text = escalaModoVibracao.ToString("n3");
            formDesenho.DirtyPortico();
        }

        void Calcular()
        {
            try
            {
                if (!VerificacoesPreCalculo())
                {
                    FechaAguardar();
                    return;
                }
                HoraInicio = DateTime.Now;

                bool RecriarGrelha = FVisGrelha != null;
                bool RecriarPortico = FVisPortico != null;

                GrelhaPrimeiraVez = false;
                CalculoCancelado = false;

             //   if (btDeformacoes.Checked)
              //    btDeformacoes_Click(btDeformacoes, null);

                CalculoCancelado = false;
                CancelaEdicoes();

                lbProgresso.Text = "";

                if (processo != null)
                    processo.Close();

                processo = new FProcessoCalculo(this, false);
                processo.Show();
                processo.btInterromper.Visible = true;

                lbProgresso.Text = "";
                for (int i = 0; i < this.Controls.Count; i++)
                    this.Controls[i].Enabled = false;
     

                 // stripBottom.Visible = false;
                stripPrincipal.Visible = false;


                //   stripBottom.Enabled = true;

                try
                {
                    // CalculaPavimentos();

                    CalculaPortico();


                    processo.pnMatrizRigidez.Refresh();
                 //   MsgCalculo("Cálculo", "CÁLCULO CONCLUÍDO DO SUCESSO - [" + DateTime.Now.Subtract(HoraInicio).ToString("mm") + ":" + DateTime.Now.Subtract(HoraInicio).ToString("ss") + "]", 0, true);
                    
                    double maxDef = formDesenho.Estrutura.PorticoEspacial.MaximoDeslocamento(cbTipoCargaResultado.SelectedIndex,
                                                                           cbResultadoCasos.SelectedIndex,
                                                                           cbResultadoCombinacoes.SelectedIndex);
                    if (maxDef != 0)
                    {
                        CalcularEscalaDeformacoes();

                        if (ConfiguracaoPrograma.IrParaPaginaResultados)
                        {                      
                            Ribbon.SelectedTab = tabResultados;
                            btDeformacao.Tag = "0";
                            btTensoes.Tag = "0";
                            btDeformacaoSolida.Tag = "0";
                            btDeformacaoColorida.Tag = "0";
                            formDesenho.DeformacaoSolida = false;

                            btDeformacao_Click(btDeformacao, null);

                            if ((string)btDiagrama.Tag == "1")
                            {
                                if (formDesenho.MostraTextoDiagramas)
                                {
                                    formDesenho.CriarTextosEsforco();
                                }
                            }
                        }
                        // if (btDeformacoes.Checked)
                        //  btDeformacoes.Checked = true;
                        //    btDeformacoes_Click(btDeformacoes, null);

                        cbUnComp.SelectedIndex = 0;

                        cbCasoCarga.SelectedIndex = cbResultadoCasos.SelectedIndex + 1;

                        cbCargas_SelectedIndexChanged(cbCasoCarga, null);
                    }

                    if (ConfiguracaoPrograma.FecharJanelaResultadosAposCalculo)
                    {
                        processo.Close();
                        processo = null;
                        AtualizaDesenho();
                    }
                /*    processo.Progresso.Visible = false;
                    processo.btInterromper.Visible = false;
                    processo.LabelProcesso.Text = "";*/
                }
                catch (TFaltaMemoria)
                {
                    HistoricoCalculo("ERRO: Falta de memória.");
                }
                catch (TErroPavimento)
                {
                    HistoricoCalculo("   >CÁLCULO INTERROMPIDO");
                }
                catch (TErroPortico)
                {
                    HistoricoCalculo("   >CÁLCULO INTERROMPIDO");
                }
                catch (TCalculoInterrompido)
                {
                    HistoricoCalculo("   >CÁLCULO INTERROMPIDO");
                }
                catch (Exception e)
                {
                    HistoricoCalculo("ERRO: " + e.Message);
                    //MessageBox.Show(e.Message);
                }

                for (int i = 0; i < this.Controls.Count; i++)
                  this.Controls[i].Enabled = true;

                // stripBottom.Visible = true;
            //    stripPrincipal.Visible = true;
               // ListaCalculo.Width = panel4.Width;
        
                System.GC.Collect();
            }
            catch(Exception)
            {
                for (int i = 0; i < this.Controls.Count; i++)
                    this.Controls[i].Enabled = true;
            }
            this.Focus();
        }

        void CalculaPortico()
        {

            HistoricoCalculo("INÍCIO DA ANÁLISE");
            HistoricoCalculo("");
            HistoricoCalculo("   > CÁLCULO DO PÓRTICO ");

            formDesenho.Estrutura.PorticoEspacial = new TPorticoEspacial(Pavimentos, formDesenho.Estrutura.barras, 
                formDesenho.Estrutura.apoios,
                ConfiguracoesPGi.CfgProjeto.materiais,
                formDesenho.CargasPontuais,
                formDesenho.CargasLineares,
                this,ConfiguracoesPGi.CfgProjeto.portico.qtdbarrasPortico);
            
            if (ConfiguracoesPGi.CfgProjeto.portico == null)
                throw new TErroPortico(PorticoEspacial, "Não há configurações de pórtico.");

            bool calculoOk = formDesenho.Estrutura.PorticoEspacial.Calcular(ConfiguracoesPGi.CfgProjeto.portico.calcularEsforcos, 
                                                                            ConfiguracoesPGi.CfgProjeto.sistema.UsarDll, 
                                                                            ConfiguracoesPGi.CfgProjeto.sistema.CalculaModosVibracao, 
                                                                            ConfiguracoesPGi.CfgProjeto.sistema.numeroModosVibracao,
                                                                            ConfiguracoesPGi.CfgProjeto.sistema.CalculaModosFlambagem,
                                                                            ConfiguracoesPGi.CfgProjeto.sistema.numeroModosFlambagem);

            HistoricoCalculo("");
            if (calculoOk)
            {
                necessitaCalculo = false;
                HistoricoCalculo("ANÁLISE CONCLUÍDA COM SUCESSO - TEMPO TOTAL: " + DateTime.Now.Subtract(HoraInicio).ToString("mm") + ":" + DateTime.Now.Subtract(HoraInicio).ToString("ss"));
                HistoricoCalculo("-------------------------------------------------------------------------");
                processo.ListaCalculo.SelectedIndex = processo.ListaCalculo.Items.Count - 2;

                if (ConfiguracoesPGi.CfgProjeto.sistema.CalculaModosVibracao)
                {
                    cbModosVibracao.Items.Clear();
                    for (int i = 1; i <= ConfiguracoesPGi.CfgProjeto.sistema.numeroModosVibracao; i++)
                        cbModosVibracao.Items.Add(i + " - freq: "+ formDesenho.Estrutura.PorticoEspacial.FrequenciasNaturais[i-1].ToString("n3") +" hz");

                    CalcularEscalaModosVibracao();
                }
            }
        }

        void CalculaPavimentos()
        {
            HistoricoCalculo("_____________________________________________________________________________________");
            HistoricoCalculo("");
            HistoricoCalculo("[Início cálculo dos pisos] - " + DateTime.Now.ToString());

            for (int i = Pavimentos.Count; i > 0; i--)
              Pavimentos[i-1].Calcula();

            //        MsgCalculo("Cálculo", "Processo concluído - [" + DateTime.Now.Subtract(span1).ToString("mm") + ":" + DateTime.Now.Subtract(span1).ToString("ss") + "]",0, true);
            HistoricoCalculo("[Cálculo dos pisos OK] - " + DateTime.Now.ToString());
            //    HistoricoCalculo("");
        }

        int PavimentoDoTreeView;
        bool ChamaGrelha, EhRaiz;

        private void TreeViewForm_AfterSelect(object sender, TreeViewEventArgs e)
        {
            EhRaiz = false;
            if (e.Node.Text == "Pavimentos" || e.Node.Text == "Modelos" || e.Node.Text == "Grelhas" || e.Node.Text == "Projeto")
            {
                EhRaiz = true;
                return;
            }
           
            PavimentoDoTreeView = int.Parse(e.Node.ToolTipText);

            ChamaGrelha = (e.Node.Parent.Text == "Grelhas");
        }

        private void menuItem44_Click(object sender, EventArgs e)
        {
            Salvar(true);
        }

        private void menuItem47_Click(object sender, EventArgs e)
        {
            FCopiarPavimento = new FCopiarPavimento(this);

            FCopiarPavimento.ShowDialog(this);
        }

        public List<TObjetoDesenho> ElementosCopiados;

        public void Copiar()
        {
            formDesenho.Estrutura.AtualizaListaObjetos();

            if ((Object)formDesenho.PavimentoAtual != null)
            {
                ElementosCopiados.Clear();

                foreach (TObjetoDesenho obj in formDesenho.Estrutura.objetos)
                       if (obj != null)
                           if (obj.layer != null)
                              if (obj.Selecionado && obj.layer.Grupo == GrupoLay.Principal)
                                  if (obj.ObjetoPrimario)
                                     ElementosCopiados.Add(obj);

                if (ElementosCopiados.Count == 1)
                   formDesenho.Comando.Text = "OK - 1 Elemento copiado";
                else
                if (ElementosCopiados.Count > 1)
                    formDesenho.Comando.Text = "OK - " + ElementosCopiados.Count + " Elementos copiados";

                formDesenho.SetaSelecionados(false, formDesenho.PavimentoAtual);
            }
        }
        [NonSerialized]
        public static double[,] RGB_EsforcosNegativos, RGB_EsforcosPositivos, RGB_Deslocamentos, RGB_TensoesNormaisNegativas, RGB_TensoesNormaisPositivas;
        public static System.Drawing.Color corSemTensao;
        public double AnguloRotacao = 0;
        
        private void Gerenciador_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.F5)
            {
                CancelaResultados();
                formDesenho.MostrarTudo();

                Calcular();
            }
            if (e.KeyCode == Keys.E)
            {
                CriaNovaBarra(); 
            }
            if ((e.KeyData == (Keys.Control | Keys.S)))
            {
                Salvar();
            }
            if ((e.KeyData == (Keys.Control | Keys.Q)))
            {
                formDesenho.RotacionarSecoesSelecionadas();
            }

            if ((e.KeyData == (Keys.Control | Keys.Z)))
            {
                formDesenho.Desfazer();
            }

            if (e.KeyCode == Keys.ControlKey)
            {
                formDesenho.clickCtrl = true;
            }
            if (e.KeyCode == Keys.ShiftKey)
            {
                formDesenho.clickShift = true;
                if (formDesenho.tipoComando == eTipoComando.draw || formDesenho.tipoComando == eTipoComando.edit)
                 
                    formDesenho.ProjetaLinhasNoPonto();
            }

            if ((e.KeyData == (Keys.Control | Keys.Q)) && (this.ActiveMdiChild == FVisPortico))
                FVisPortico.ChamaInfoBarra();

            if (e.KeyData == (Keys.Control | Keys.C))
            {
                FInfoBarraPortico infb = new FInfoBarraPortico(formDesenho.Estrutura.PorticoEspacial.barras[1], this);
                infb.Show();
            }

            if (e.KeyData == (Keys.Control | Keys.F))
            {
                formDesenho.Comando.Text = "Localizar texto - Digite o texto";
                return;
            }
            if (e.KeyData == (Keys.Control | Keys.N))
            {
                NovoProjeto(true);
            }

            if (e.KeyData == Keys.Enter)
            {
                if (formDesenho.FerramentaEdicao != null && !formDesenho.pnDivBarras.Visible)
                {
                    formDesenho.Estrutura.AtualizaListaObjetos();
                    if (formDesenho.FerramentaEdicao.editToolTipoSelecao > 0) // se for seleção para ferramenta de edição
                    {
                        foreach (TObjetoDesenho objeto in formDesenho.Estrutura.objetos)
                                if (((objeto.Selecionado && formDesenho.FerramentaEdicao.TipoObjetoParaSelecionar == objeto.Tipo) ||
                                     (objeto.Selecionado && formDesenho.FerramentaEdicao.TipoObjetoParaSelecionar == "TODOS")
                                    ) && formDesenho.FerramentaEdicao.editToolTipoSelecao == eEditToolTipoSelecao.multiSelecao)
                                {
                                    if (objeto.ObjetoPrimario)
                                        if (!formDesenho.ObjetoJaEstaEstaNaListaSelecionado(objeto))
                                        {
                                            objeto.SetaSelecao(true, false);

                                            formDesenho.ObjetosSelecionados.Add(objeto);
                                        }
                                }
                    }

                    //objetos que nao forem do tipo do editTool sao desselecionados
                    foreach (TObjetoDesenho objeto in formDesenho.Estrutura.objetos)
                                if (objeto.Tipo != formDesenho.FerramentaEdicao.TipoObjetoParaSelecionar && formDesenho.FerramentaEdicao.TipoObjetoParaSelecionar != "TODOS")
                                {
                                    objeto.Selecionado = false;
                                    objeto.SetaSelecao(false, false);
                                }

                        if (formDesenho.ObjetosSelecionados.Count > 0)
                        {
                            if (formDesenho.FerramentaEdicao != null)
                            {
                                formDesenho.tipoComando = eTipoComando.edit;
                                
                                if (DigitarComando.Text.Contains("@") && !DigitarComando.Text.Contains("<")) // coordenada relativa simples
                                {
                                    string x = DigitarComando.Text.Substring(1, DigitarComando.Text.IndexOf(",") - 1).Replace(".", ",");
                                    string y = DigitarComando.Text.Substring(DigitarComando.Text.IndexOf(",") + 1, DigitarComando.Text.Length - (x.Length) - 2).Trim().Replace(".", ",");

                                    if (formDesenho.IdFerramentaEdicao == Const.ID_COPIAR_ELEMENTOS)
                                    {
                                        formDesenho.MouseEdit((formDesenho.FerramentaEdicao as TCopiarElementos).ponto1.x + System.Convert.ToSingle(x),
                                                              (formDesenho.FerramentaEdicao as TCopiarElementos).ponto1.y + System.Convert.ToSingle(y),0, 0, "", 0);
                                    }
                                }
                                else
                                if (DigitarComando.TextLength == 0)
                                {
                             //       formDesenho.MouseEdit(0, 0, 0, 0, "", 0);


                                 //   if (formDesenho.ObjetosSelecionados.Count > 0)
                                 //      formDesenho.MouseEdit(0, 0, 0, 0, "", 0);
                                }

                                else
                                {
                                    if (formDesenho.IdFerramentaEdicao != Const.ID_GIRAR_ELEMENTOS)
                                    {
                                        string x = DigitarComando.Text.Substring(1, DigitarComando.Text.IndexOf(",") - 1).Replace(".", ",");
                                        string y = DigitarComando.Text.Substring(DigitarComando.Text.IndexOf(",") + 1, DigitarComando.Text.Length - (x.Length) - 2).Trim().Replace(".", ",");

                                        formDesenho.MouseEdit(System.Convert.ToSingle(x),
                                                                    System.Convert.ToSingle(y), 0,
                                                                    0, "", 0);
                                    }
                                }
                            }
                        }
                    }

                if (formDesenho.IdObjetoDesenho != null)
                {
             //       formDesenho.MultiMouseDraw();
                }
            }

            if (this.DigitarComando.Visible /*&& (dockPanel.ActiveDocument == desenho && desenho != null)*/)
            {
                this.DigitarComando.Focus();
            }

            if (e.KeyData == Keys.Delete && formDesenho.Estrutura.GetCountElementosSelecionados() > 0)
            {
                DialogResult result = MessageBox.Show("Deletar os objetos selecionados?"/* (" + formDesenho.Estrutura.GetCountElementosSelecionados() + ")"*/
                                                     , "Excluir", MessageBoxButtons.YesNo, MessageBoxIcon.Question);
                if (result == DialogResult.Yes)
                {
                    formDesenho.DeletaSelecionados(formDesenho.PavimentoAtual);
                    formDesenho.ApagaElementosDesnecessarios();
                }
            };
            
            if (F3d != null)
                if (e.KeyCode == Keys.ControlKey && F3d.Visible)
                  F3d.Focus();


            //Directory.CreateDirectory();
        }

        public PaintEventArgs ctrlCAD;


        public void CancelaEdicoes()
        {
 
            formDesenho.CancelaInsercoes();
        }

        bool EditandoGrelha;

        void AbreImportadorDWGDXF()
        {
            string CaminhoExe = Directory.GetCurrentDirectory() + "\\PGiDwgDxf.exe";
            //MessageBox.Show(CaminhoExe);
            System.Diagnostics.Process.Start(CaminhoExe);

            timerDXF.Start();
        }

        private void menuItem49_Click(object sender, EventArgs e)
        {
            File.Delete(Directory.GetCurrentDirectory() + "\\DWGDXF_Importando_CANCELOU.txt");
            File.Delete(Directory.GetCurrentDirectory() + "\\DWGDXF_Importando.txt");
            AbreImportadorDWGDXF();	
            
        }
        private void timerDXF_Tick(object sender, EventArgs e)
        {
                string inputFileTxt = "";

                if (File.Exists(Directory.GetCurrentDirectory() + "\\DWGDXF_Importando_FECHOU.txt"))
                {
                    File.Delete(Directory.GetCurrentDirectory() + "\\DWGDXF_Importando_FECHOU.txt");
                    timerDXF.Stop();
                }

                if (File.Exists(Directory.GetCurrentDirectory() + "\\DWGDXF_Importando.txt"))
                {
                    inputFileTxt = Directory.GetCurrentDirectory() + "\\DWGDXF_Importando.txt";

                    int ino = inputFileTxt.LastIndexOf("\\");	//index no of the last "\" (that is before the filename) is found here

                    if (inputFileTxt.Length > 0)
                    {
                        //    ChamaAguardar(this, "Importando arquivo. Aguarde...");
                            formDesenho.LerDXF(inputFileTxt);	//the filename is sent to the method for data extraction and interpretation...
 
                       // FechaAguardar();
                        formDesenho.Enquadrar();
                        timerDXF.Stop();                    
                    }

                    File.Delete(Directory.GetCurrentDirectory() + "\\DWGDXF_Importando.txt");
                }
            }

        private void menuItem38_Click(object sender, EventArgs e)
        {
            FFontes = new FFontes(this);
            FFontes.ShowDialog();
            this.Refresh();
        }

        private void cbTipoDiagrama_SelectedIndexChanged(object sender, EventArgs e)
        {

        }

        public bool CalculoCancelado;
        private void btCancelar_Click(object sender, EventArgs e)
        {
            CalculoCancelado = true;
        }
        void ChamaCfgCalculo()
        { 
           if (CfgProjeto == null)
                CfgProjeto = new FConfiguraProjeto(this);

            CfgProjeto.Show(this);
        }

        void DiagramasPortico(object sender)
        {
            if (FVisPortico == null)
                return;

      //      ribbonPanelPortico.Text = "Pórtico Espacial - ";
            FVisPortico.Text = "Pórtico";
            FVisPortico.Deslocamento = false;
            FVisPortico.CortanteY    = false;
            FVisPortico.CortanteZ    = false;
            FVisPortico.Torcor       = false;
            FVisPortico.Axial        = false;


            if (FVisPortico != null)
            {
                FVisPortico.AtualizaFormCores();
                FVisPortico.Render();
                FVisPortico.Controle.SwapBuffers();
            }
        }

        void ChamaOpcPortico()
        {
           string arg = string.Empty;

           
            DialogResult result = DialogResult.Yes;
           {
           //    if (!FOpcVisPortico.Visible)
             //      FOpcVisPortico = new FPorticoOpcoesVisualizacao(this);
               ConfiguraDeformacao = new FConfiguraDeformacao(this);

               result = ConfiguraDeformacao.ShowDialog(this);
           }

           if (result == DialogResult.Yes)
           {
               FVisPortico.Atualizar();
               if (FVisPortico != null)
               {
                   FVisPortico.AtualizaEsforcos("Atualizando pórtico. Aguarde...");
                   FVisPortico.SetupCamera(!ConfiguracoesPGi.PorticoOpcoesVisualizacao.Perspectiva);
               }
           }

           if (FVisPortico != null)
           {
               if (FVisPortico.iEsforcoAtual == FVisualizadorPortico.iDeslocamento)
                   FVisPortico.PanelCores2.Visible = ConfiguracoesPGi.PorticoOpcoesVisualizacao.Diagramas_DeslocamentoGradiente;
               else
                   if (FVisPortico.iEsforcoAtual != FVisualizadorPortico.iDeslocamento)
                       FVisPortico.PanelCores2.Visible = ConfiguracoesPGi.PorticoOpcoesVisualizacao.Diagramas_Gradiente;
           }
        }

        public int x_ult_formBarras = 0, y_ult_formBarras = 0;
        void CriaNovaBarra()
        {
            if (DadosBarra == null)
            {
                if (formDesenho.Barras.Count > 0)
                {
                    int tr = 0;
                    int max = 0;
                    for (int j = 0; j < formDesenho.Barras.Count; j++)
                    {
                        if (formDesenho.Barras[j].IDBarra > max)
                        {
                            tr = j;
                            max = formDesenho.Barras[j].IDBarra;
                        }
                    }

                    if (DadosBarra == null)
                    {
                        DadosBarra = new FDadosBarra(this, null);
                        DadosBarra.numero.Text = (formDesenho.Barras.Max(obj => obj.IDBarra) + 1).ToString();
                    }
                }
                else
                {
                    if (DadosBarra == null)
                        DadosBarra = new FDadosBarra(this, null);
                }

                DadosBarra.Show(this);
                DadosBarra.ShowInTaskbar = false;
            }
        }
        void CriaNovoApoio()
        {
            if (fApoio == null)
                fApoio = new FApoio(this, null);
           // fApoio.MdiParent = this;
            fApoio.Show(this);
        }

        public void ComandoNovaBarra()
        {
            formDesenho.ComandoDesenhar(btBarra.Tag.ToString(), formDesenho.DadosBarra, arg, Const.ID_INSERCAO_INDIVIDUAL);
            
            formDesenho.HabilitaCoords(true); 
        }
        public void ComandoNovoApoio(bool InsercaoIndividual)
        {
            if (InsercaoIndividual)
            {
                formDesenho.ComandoDesenhar(Const.ID_APOIO, formDesenho.DadosApoio, arg, Const.ID_INSERCAO_INDIVIDUAL); 
                formDesenho.MouseDrawing(0, 0, 0);
                formDesenho.HabilitaCoords(true);
            }
            else
            {
                formDesenho.ComandoDesenhar(Const.ID_APOIO, formDesenho.DadosApoio, arg, Const.ID_INSERCAO_SELECAO);
            }
        }

        void NovaViga()
        {
            if (formDesenho.TrechosVigas.Count > 0)
            {
                int tr = 0;
                int max = 0;
                for (int j = 0; j < formDesenho.TrechosVigas.Count; j++)
                {
                    if (formDesenho.TrechosVigas[j].Dados.numero > max)
                    {
                        tr = j;
                        max = formDesenho.TrechosVigas[j].Dados.numero;
                    }
                }

                if (FDadosViga == null)
                {
                    FDadosViga = new DadosViga(this, null, formDesenho.TrechosVigas[tr].Dados.indiceTipo);
                    FDadosViga.NumViga.Value = formDesenho.TrechosVigas.Max(obj => obj.Dados.numero) + 1;
                    FDadosViga.edh1.Text = formDesenho.TrechosVigas[tr].Dados.h1.ToString();
                    FDadosViga.edb1.Text = formDesenho.TrechosVigas[tr].Dados.b1.ToString();
                    FDadosViga.edh2.Text = formDesenho.TrechosVigas[tr].Dados.h2.ToString();
                    FDadosViga.edb2.Text = formDesenho.TrechosVigas[tr].Dados.b2.ToString();
                    FDadosViga.redTorcao.Value = formDesenho.TrechosVigas[tr].Dados.redTorcao;
                    FDadosViga.CargaExtra.Text = formDesenho.TrechosVigas[tr].Dados.CargaExtra.ToString();
                    FDadosViga.RigidezEI.Text = formDesenho.TrechosVigas[tr].Dados.RigidezEI.ToString();
                    FDadosViga.RigidezGJ.Text = formDesenho.TrechosVigas[tr].Dados.RigidezGJ.ToString();
                    FDadosViga.NaoUsarPP.Checked = formDesenho.TrechosVigas[tr].Dados.NaoUsarPP;
                  //  FDadosViga.matcbvigas.SelectedIndex   = formDesenho.TrechosVigas[tr].Estrutura.classeViga.classe;
                    // if (formDesenho.TrechosVigas[tr].Dados.CargaParede == 0)
                    //     FDadosViga.CargaParede.Text = "500";
                    // else
                    FDadosViga.CargaParede.Text = formDesenho.TrechosVigas[tr].Dados.CargaParede.ToString();

                    FDadosViga.edE.Text = formDesenho.TrechosVigas[tr].Dados.excentricidade.ToString();
                    FDadosViga.NomeViga.Text = "V";
                }
            }
            else
            {
                if (FDadosViga == null)
                  FDadosViga = new DadosViga(this, null, 0);
            }

            FDadosViga.Show();    
        }

        public void NovosDadosDeViga()
        {
            formDesenho.DadosTrecho =
                new TDadosViga(System.Convert.ToDouble(FDadosViga.edb1.Text),
                System.Convert.ToDouble(FDadosViga.edb2.Text),
                System.Convert.ToDouble(FDadosViga.edh1.Text),
                System.Convert.ToDouble(FDadosViga.edh2.Text),
                System.Convert.ToDouble(FDadosViga.edE.Text),
                FDadosViga.cbTipo.SelectedIndex,
                FDadosViga.Poligono,
                FDadosViga.NomeViga.Text,
                System.Convert.ToInt32(FDadosViga.NumViga.Text),
                FDadosViga.tbFaces.Buttons[0].Pushed,
                FDadosViga.tbFaces.Buttons[2].Pushed,
                FDadosViga.tbFaces.Buttons[1].Pushed,
                (int)FDadosViga.redTorcao.Value,
                System.Convert.ToDouble(FDadosViga.CargaParede.Text),
                System.Convert.ToDouble(FDadosViga.CargaExtra.Text));

            /*---------------------------------------------------------*/

            formDesenho.DadosTrecho.RigidezEI = System.Convert.ToDouble(FDadosViga.RigidezEI.Text);
            formDesenho.DadosTrecho.RigidezGJ = System.Convert.ToDouble(FDadosViga.RigidezGJ.Text);
            formDesenho.DadosTrecho.NaoUsarPP = FDadosViga.NaoUsarPP.Checked;
        //    formDesenho.ActiveLayer = formDesenho.PavimentoAtual.LayersByIdPrincipal[Lay.Vigas];
        }

        void NovoPilar()
        {
            if (formDesenho.Pilares.Count > 0)
            {
                int tr = 0;
                int max = 0;
                for (int j = 0; j < formDesenho.Pilares.Count; j++)
                {
                    if (formDesenho.Pilares[j].Dados.numero > max)
                    {
                        tr = j;
                        max = formDesenho.Pilares[j].Dados.numero;
                    }
                }

                FDadosPilar = new DadosPilar(this, null, formDesenho.Pilares[tr].Dados.indiceTipo);
                FDadosPilar.Num.Value = formDesenho.Pilares.Max(obj => obj.Dados.numero) + 1;
                FDadosPilar.edh1.Text = formDesenho.Pilares[tr].Dados.h1.ToString();
                FDadosPilar.edb1.Text = formDesenho.Pilares[tr].Dados.b1.ToString();

                FDadosPilar.edE.Text = formDesenho.Pilares[tr].Dados.excentricidade.ToString();
                FDadosPilar.Nome.Text = "P";
            }
            else
            if (FDadosPilar == null)
                FDadosPilar = new DadosPilar(this, null, 0);

          //  result = FDadosPilar.ShowDialog(this);
            FDadosPilar.Show();
        }

        public void NovosDadosDePilar()
        {
            formDesenho.DadosPilar =
                new TDadosPilar(System.Convert.ToDouble(FDadosPilar.edAltura.Text),
            System.Convert.ToDouble(FDadosPilar.edh1.Text),
            System.Convert.ToDouble(FDadosPilar.edb1.Text),0,0,
            FDadosPilar.cbTipo.SelectedIndex,
            System.Convert.ToDouble(FDadosPilar.edE.Text),
            FDadosPilar.Nome.Text,
            System.Convert.ToInt32(FDadosPilar.Num.Text),
            FDadosPilar.Poligono);
            /*---------------------------------------------------------*/
            formDesenho.ActiveLayer = formDesenho.Estrutura.LayersByIdPrincipal[Lay.Pilares];

            formDesenho.MouseDrawing(0, 0, 0);
        }

        void NovaLaje()
        {
            if (formDesenho.Lajes.Count > 0)
            {
                int tr = 0;
                int max = 0;
                for (int j = 0; j < formDesenho.Lajes.Count; j++)
                {
                    if (formDesenho.Lajes[j].Dados.numero > max)
                    {
                        tr = j;
                        max = formDesenho.Lajes[j].Dados.numero;
                    }
                }

                FDadosLaje = new DadosLaje(this, null, formDesenho.Lajes[tr].Dados.indiceTipo);
                FDadosLaje.NumLaje.Value = formDesenho.Lajes.Max(obj => obj.Dados.numero) + 1;
                FDadosLaje.edh1.Text = formDesenho.Lajes[tr].Dados.h.ToString();
                FDadosLaje.NomeLaje.Text = "L";
                FDadosLaje.chGerarGrelha.Checked = true;
                FDadosLaje.CargaAcidental.Text = formDesenho.Lajes[tr].Dados.CargaAcidental.ToString();
                FDadosLaje.CargaPermanente.Text = formDesenho.Lajes[tr].Dados.CargaPermanente.ToString();
                FDadosLaje.rbGrelhaPavimento.Checked = true;
            }
            else
            if (FDadosLaje == null)
                FDadosLaje = new DadosLaje(this, null, 0);

            FDadosLaje.Show();
        }

        public void NovosDadosDeLaje()
        {
            formDesenho.DadosLaje =
             new TDadosLaje(
            System.Convert.ToDouble(FDadosLaje.edh1.Text),
            FDadosLaje.NomeLaje.Text,
            System.Convert.ToInt32(FDadosLaje.NumLaje.Text),
            FDadosLaje.cbTipo.SelectedIndex,
            FDadosLaje.ConfigGrelha,
            FDadosLaje.chGerarGrelha.Checked,
            FDadosLaje.rbGrelhaEspecifica.Checked,
            System.Convert.ToDouble(FDadosLaje.CargaPermanente.Text),
            System.Convert.ToDouble(FDadosLaje.CargaAcidental.Text));

            formDesenho.ActiveLayer = formDesenho.Estrutura.LayersByIdPrincipal[Lay.Lajes];
        }

        private void ribbonButton7_Click(object sender, EventArgs e)
        {
            ChamaPorticoEspacial();
        }

        private void ribbonButton8_Click(object sender, EventArgs e)
        {
            ChamaGrelhaEspacial();
        }

        private void ribbonButton5_Click(object sender, EventArgs e)
        {
            if (cbPiso.SelectedIndex > 0)
            {
                cbPiso.SelectedIndex -= 1;

                AlternaPavimento();
            }
        }

        private void ribbonButton3_Click(object sender, EventArgs e)
        {
            
        }

        private void ribbonButton7_Click_1(object sender, EventArgs e)
        {
            ChamaGrelhaEspacial();
        }

        private void ribbonButton8_Click_1(object sender, EventArgs e)
        {
            ChamaPorticoEspacial();
        }

        private void ribbonButton27_Click(object sender, EventArgs e)
        {
            NovoProjeto(false);
        }

        private void ribbonButton35_Click_1(object sender, EventArgs e)
        {
            Chama3D(false,false);
        }

        private void ribbonButton33_Click(object sender, EventArgs e)
        {

        }

        int ultPisoSelecionado = -1;
        int[] ultPisosSelecionados = new int[20];

        private void label5_Click(object sender, EventArgs e)
        {
            cbPiso.SelectedIndex = (sender as Label).TabIndex;
         
            AlternaPavimento();
        }

        private void btViga_Click(object sender, EventArgs e)
        {
            //   NovaViga();

           formDesenho.ComandoDesenhar(Const.ID_ARCOIMF, null, arg, Const.ID_INSERCAO_INDIVIDUAL);

           formDesenho.HabilitaCoords(true);

        }

        private void btPilar_Click(object sender, EventArgs e)
        {
            NovoPilar();
        }

        private void ribbonButton6_Click(object sender, EventArgs e)
        {
            NovaLaje();
        }


        private void ribbonUpDown1_DownButtonClicked(object sender, System.Windows.Forms.MouseEventArgs e)
        {
            if (escalaGrelha.TextBoxText == "0") return;
            escalaGrelha.TextBoxText = System.Convert.ToString(int.Parse(escalaGrelha.TextBoxText.ToString()) - 1);
            FVisGrelha.iEscalaDiagrama = int.Parse(escalaGrelha.TextBoxText.ToString());
            FVisGrelha.Render();
        }

        private void ribbonUpDown1_UpButtonClicked(object sender, System.Windows.Forms.MouseEventArgs e)
        {
            escalaGrelha.TextBoxText = System.Convert.ToString(int.Parse(escalaGrelha.TextBoxText.ToString()) + 1);
            FVisGrelha.iEscalaDiagrama = int.Parse(escalaGrelha.TextBoxText.ToString());
            FVisGrelha.Render();
         //   TOpenGl.OGL.Refresh();
        }

        private void btGrelhaDeslocamento_Click(object sender, EventArgs e)
        {
            DiagramasGrelha(sender);
        }


        private void ribbonButton2_Click(object sender, EventArgs e)
        {
            this.Refresh();
        }
        public void NovosDadosDeCargaBarra(string tipo)
        {
            //dadoscarga -> uso a mesma classe para armazenar temporariamente o valor da carga
            int dp =2;
            if (CargaBarra.rbX.Checked) dp = 0;
            if (CargaBarra.rbY.Checked) dp = 1;
            if (CargaBarra.rbZ.Checked) dp = 2;
            double valor = 0;
            if (CargaBarra.tabControl1.SelectedIndex == 0) // distribuida
            {
                valor = conv.forca(System.Convert.ToDouble(CargaBarra.Valor.Text), ConfiguracoesPGi.CfgProjeto.unidadesProjeto.res_un_forca.ToString(), und.kN);
                valor = conv.comp(valor, und.m, ConfiguracoesPGi.CfgProjeto.unidadesProjeto.res_un_comprimento.ToString());
            }
            else
                valor = conv.forca(System.Convert.ToDouble(CargaBarra.Valor.Text), ConfiguracoesPGi.CfgProjeto.unidadesProjeto.res_un_forca.ToString(), und.kN);
             
              formDesenho.DadosCarga = new TDadosCarga(valor,
                System.Convert.ToDouble(CargaBarra.edD.Text),dp,
                (CargaBarra.rbGlobal.Checked ? 0 : 1),
                CargaBarra.tabControl1.SelectedIndex == 0,
                CargaBarra.tabControl1.SelectedIndex == 1,
                CargaBarra.idCasoSelecionado,
                formDesenho.CasosCarga.Find(o => o.ID == CargaBarra.idCasoSelecionado).Cor,
                CargaBarra.chPosRel.Checked,true,false);
    
            formDesenho.ComandoDesenhar(Const.ID_CARGA_LINEAR, formDesenho.DadosCarga, "", Const.ID_INSERCAO_SELECAO);
/*
            if (tipo == Const.ID_CARGA_PONTUAL)
                formDesenho.MouseDrawing(0, 0,0, false);*/
        }
        public void NovosDadosDeCargaNodal(string tipo)
        {
            //dadoscarga -> uso a mesma classe para armazenar temporariamente o valor da carga
            int dp = 2;
            if (CargaNodal.rbX.Checked) dp = 0;
            if (CargaNodal.rbY.Checked) dp = 1;
            if (CargaNodal.rbZ.Checked) dp = 2;

            double valor = conv.forca(System.Convert.ToDouble(CargaNodal.Valor.Text), ConfiguracoesPGi.CfgProjeto.unidadesProjeto.res_un_forca.ToString(), und.kN);

             formDesenho.DadosCarga = new TDadosCarga(valor,
                0, dp,
                -1,false,true,
                 CargaNodal.idCasoSelecionado,
                formDesenho.CasosCarga.Find(o => o.ID == CargaNodal.idCasoSelecionado).Cor,
                false,
                CargaNodal.rbForca.Checked, CargaNodal.rbMomento.Checked);

            formDesenho.ComandoDesenhar(Const.ID_CARGA_PONTUAL, formDesenho.DadosCarga, "", Const.ID_INSERCAO_SELECAO);
            /*
                        if (tipo == Const.ID_CARGA_PONTUAL)
                            formDesenho.MouseDrawing(0, 0,0, false);*/
        }
        private void ribbonButton8_Click_2(object sender, EventArgs e)
        {
            ChamaPorticoEspacial();
        }

        private void timerModelos_Tick(object sender, EventArgs e)
        {
            timerModelos.Stop();
        }

        private void button13_Click(object sender, EventArgs e)
        {
            ChamaPorticoEspacial();
        }

        private void button12_Click(object sender, EventArgs e)
        {
            ChamaGrelhaEspacial();
        }

        private void ribbonButton8_Click_3(object sender, EventArgs e)
        {
            NovoProjeto(true);
        }

        private void ribbonButton19_Click(object sender, EventArgs e)
        {
           // lbModeloGrelha.Text = "Grelha: " + formDesenho.PavimentoAtual.Descricao;
         //   timerModelos.Start();
        }

        private void CriaFormDetalhamento()
        {
            try
            {
                desenho = new FPrincipal(this);
                formDesenho.MdiParent = this;
                formDesenho.WindowState = System.Windows.Forms.FormWindowState.Maximized;

                formDesenho.Show();
             
            }
            catch (Exception e)
            {
                MessageBox.Show(e.Message);
            }
        }
        private void ribbonButton29_Click_1(object sender, EventArgs e)
        {
            CriaFormDetalhamento();
        }

        private void cbGrelhaTipoDiagrama_TextBoxTextChanged(object sender, EventArgs e)
        {
            if (FVisGrelha != null)
            {
                if (cbGrelhaTipoDiagrama.TextBoxText == "Diagramas")
                {
                    FVisGrelha.HabilitaIsovalores(false);
                    FVisGrelha.VistaPerspectiva();
                }
                else
                if (cbGrelhaTipoDiagrama.TextBoxText == "Isolinhas")
                {
                    FVisGrelha.HabilitaIsovalores(true);
                    FVisGrelha.VistaCima();
                }
            }
        }

        private void ribbonButton42_Click(object sender, EventArgs e)
        {
            Chama3D(false, true);
        }

        private void ribbonButton36_Click(object sender, EventArgs e)
        {
            Chama3D(true,true);
        }

        private void ribbonButton1_Click(object sender, EventArgs e)
        {
            Chama3D(false, true);
        }

        private void ribbonOrbMenuItem2_Click(object sender, EventArgs e)
        {
            NovoProjeto(true);
        }

        private void ribbonOrbMenuItem3_Click(object sender, EventArgs e)
        {
            Abrir();
        }

        private void ribbonOrbMenuItem4_Click(object sender, EventArgs e)
        {
            Salvar();
        }

        private void ribbonOrbMenuItem5_Click(object sender, EventArgs e)
        {
            Salvar(true);
        }

        private void ribbonButton6_Click_1(object sender, EventArgs e)
        {
            File.Delete(Directory.GetCurrentDirectory() + "\\DWGDXF_Importando_CANCELOU.txt");
            File.Delete(Directory.GetCurrentDirectory() + "\\DWGDXF_Importando.txt");
            AbreImportadorDWGDXF();	
        }

        void PisoAcima()
        {
            if (cbPiso.SelectedIndex > 0)
            {
                cbPiso.SelectedIndex -= 1;

                AlternaPavimento();
            }
        }
        void PisoAbaixo()
        {
            if (cbPiso.SelectedIndex < cbPiso.Items.Count - 1)
            {
                cbPiso.SelectedIndex += 1;

                AlternaPavimento();
            }
        }

        private void ribbonButton50_Click(object sender, EventArgs e)
        {
            formDesenho.ComandoEdicao(Const.ID_DIVIDIR_VIGA);
        }

        private void ribbonButton51_Click(object sender, EventArgs e)
        {
            formDesenho.ComandoEdicao(Const.ID_RENOMEAR_VIGA);
        }


        public FEsforcosPortico EsforcoPortico;
        public FCargasPortico CargasPortico;
        public FSobre sobre;
        private void ribbonButton16_Click(object sender, EventArgs e)
        {

        }

        private void ribbonButton52_Click(object sender, EventArgs e)
        {
            ChamaPorticoEspacial(true);
        }

        private void ribbonButton10_Click(object sender, EventArgs e)
        {
            File.Delete(Directory.GetCurrentDirectory() + "\\DWGDXF_Importando_CANCELOU.txt");
            File.Delete(Directory.GetCurrentDirectory() + "\\DWGDXF_Importando.txt");
            AbreImportadorDWGDXF();
        }

        private void Gerenciador_KeyUp(object sender, KeyEventArgs e)
        {
            if (FVisPortico != null)
                if (e.KeyCode == Keys.ControlKey && FVisPortico.Visible)
                {
                    if (FVisPortico.shift)
                        FVisPortico.shift = false;    
                }

            if (F3d != null)
                if (e.KeyCode == Keys.ControlKey && F3d.Visible)
                {
                    if (F3d.shift)
                        F3d.shift = false;
                }

            if (e.KeyCode == Keys.ControlKey)
            {
               formDesenho.clickCtrl = false;
            }
            if (e.KeyCode == Keys.ShiftKey)
            {
                formDesenho.clickShift = false;
            }
        }

        public  void AtivaModo3D()
        {
            formDesenho.PlanoCorte = false;
            formDesenho.MostraPlano = false;
            formDesenho.Cursor = Cursors.Cross;
            lbCoordPlano.Visible = false;
            btPisoBaixo.Visible = false;
            btPisoCima.Visible = false;
            cbPiso.Visible = false;
            btModo.Text = "Modelo 3D";
            formDesenho.Text = btModo.Text;

            edIncPlano.Visible = false;
            btSelecaoPiso.Visible = false;
            Modo3D = true;
            ModoPiso = false;
            ModoPlano = false;

            ultPisoSelecionado = cbPiso.SelectedIndex;
        }

        public bool Modo3D, ModoPiso, ModoPlano;
        public void AtualizaComboCombinacoes()
        {
            cbResultadoCombinacoes.Items.Clear();
            cbResultadoCasos.Items.Clear();

            cbTipoCargaResultado.SelectedIndex = 0;

            foreach (TCasosCarga c in formDesenho.CasosCarga)
              cbResultadoCasos.Items.Add(c.Nome);

            foreach (TCombinacoes c in formDesenho.Estrutura.combinacoes)
            {
                if (c.categoriaCombinacao == CategoriaCombinacao.Linear)
                    cbResultadoCombinacoes.Items.Add(c.EstadoLimite + "/" + c.Nome + " - " + c.Descricao);
                else
                if (c.categoriaCombinacao == CategoriaCombinacao.Estabilidade)
                    cbResultadoCombinacoes.Items.Add("Estabilidade/" + c.Nome + " - " + c.Descricao);
            }

            if (cbResultadoCombinacoes.Items.Count>0)
              cbResultadoCombinacoes.SelectedIndex = 0;
            
            cbResultadoCasos.SelectedIndex = 0;
        }

        public void AtualizaComboCarga()
        {
            cbCasoCarga.Items.Clear();
            cbCasoCarga.Items.Add("<NENHUM>");
            foreach (TCasosCarga c in formDesenho.CasosCarga)
                cbCasoCarga.Items.Add(c.Nome);
            cbCasoCarga.Items.Add("<TODOS>");
            cbCasoCarga.SelectedIndex = 0;
        }
        void AtivaModoPiso()
        {
            try
            {
                formDesenho.MostraPlano = false;
                btSelecaoPiso.Visible = true;
                lbCoordPlano.Text = "Z (m):";
                lbCoordPlano.Left = 260;
                edIncPlano.Left = 300;
                lbCoordPlano.Visible = true;
                btModo.Text = "Piso";
                formDesenho.Text = btModo.Text;
                edIncPlano.Visible = true;
                btPisoBaixo.Visible = true;
                btPisoCima.Visible = true;
                Modo3D = false;
                ModoPiso = true;
                ModoPlano = false;
                cbPiso.Visible = true;
                formDesenho.Cursor = Cursors.Cross;
                ConfiguracoesPGi.F3DOpcoesVisualizacao.EixosCentrais = false;
                formDesenho.Datum = false;
                for (int i = 0; i < 20; i++)
                    ultPisosSelecionados[i] = -2;


                if (Pavimentos.Count > 0)
                {
                    edIncPlano.Text = Pavimentos[cbPiso.SelectedIndex].PeDireito.ToString("n2");
                    formDesenho.PlanoTrabalho.Normal.x = 0;
                    formDesenho.PlanoTrabalho.Normal.y = 0;
                    formDesenho.PlanoTrabalho.Normal.z = 1;
                    formDesenho.PlanoTrabalho.Posicao.z = Pavimentos[cbPiso.SelectedIndex].Nivel * -1;
                    formDesenho.PlanoTrabalho.Posicao.x = 0;
                    formDesenho.PlanoTrabalho.Posicao.y = 0;
                }

                string face = "top";
                formDesenho.VisualizarConformeGuizmo(ref face);
            }
            catch(Exception e)
            {
                MessageBox.Show(e.Message);
            }
        }

        void AlternaCamera(bool orto = false)
        {
            formDesenho.CameraOrto = !formDesenho.CameraOrto;
            if (orto)
                formDesenho.CameraOrto = orto;
            ConfiguracoesPGi.F3DOpcoesVisualizacao.Perspectiva = formDesenho.CameraOrto;
            formDesenho.SetupCamera(true);
            formDesenho.DesenhaObjetos();
            formDesenho.glControl.SwapBuffers();
            formDesenho.Enquadrar();
            formDesenho.AtualizaDisplayList_Nos();
            formDesenho.DesenhaObjetos();
            formDesenho.glControl.SwapBuffers();
        }
        private void ribbonButton52_Click_1(object sender, EventArgs e)
        {
            
        }

        private void ribbonButton58_Click(object sender, EventArgs e)
        {

        }
        FRotacionarElementos FOpcVis3D;

        private void ribbonButton2_Click_2(object sender, EventArgs e)
        {
            NovoProjeto(true);
        }

        private void ribbonButton22_Click_1(object sender, EventArgs e)
        {
            Abrir();
        }

        private void ribbonButton24_Click_1(object sender, EventArgs e)
        {
            Salvar();
        }

        private void ribbonButton5_Click_3(object sender, EventArgs e)
        {
            NovoProjeto(false);
        }

        private void ribbonButton23_Click_1(object sender, EventArgs e)
        {
            NovoProjeto(true);
        }

        private void ribbonButton28_Click(object sender, EventArgs e)
        {
            Abrir();
        }

        private void ribbonButton36_Click_3(object sender, EventArgs e)
        {
            Salvar();
        }
        public FIncrementaPlano f_incPlano;
        void AtivaModoPlano(string plano, bool PlanoCorte = false)
        {
            if (f_incPlano != null)
              f_incPlano.Close();

            f_incPlano = new FIncrementaPlano(this); 

            formDesenho.Cursor = Cursors.Arrow;
            
           // f_incPlano.MdiParent = this;
            btSelecaoPiso.Visible = false;
            f_incPlano.edIncPlano.Text = edIncPlano.Text;
            f_incPlano.plano = plano;
            f_incPlano.PlanoCorte = PlanoCorte;
            f_incPlano.Show(this);

            if (plano == "xy")
            {
                f_incPlano.rbxy.Checked = true;
                f_incPlano.lbCoordPlano.Text = "Z (m):";
            }
            if (plano == "yz")
            {
                f_incPlano.rbyz.Checked = true;
                f_incPlano.lbCoordPlano.Text = "X (m):";
            }
            if (plano == "xz")
            {
                f_incPlano.rbxz.Checked = true;
                f_incPlano.lbCoordPlano.Text = "Y (m):";
            } 
            
            if (result == DialogResult.Yes)
            {
                edIncPlano.Text = f_incPlano.edIncPlano.Text;
            }
            formDesenho.MostraPlano = true;

            lbCoordPlano.Visible = true;
            Modo3D = false;
            ModoPiso = false;
            ModoPlano = true;
            cbPiso.Visible = false;
            btPisoBaixo.Visible = false;
            btPisoCima.Visible = false;
            lbCoordPlano.Left = 75;
            edIncPlano.Left = 120;
            edIncPlano.Visible = true;

            if (plano == "xy")
                lbCoordPlano.Text = "Z (m):";
            if (plano == "yz")
                lbCoordPlano.Text = "X (m):";
            if (plano == "xz")
                lbCoordPlano.Text = "Y (m):";
        }

        private void btPisoBaixo_Click(object sender, EventArgs e)
        {
            PisoAbaixo();
            formDesenho.glControl.Focus();
        }

        private void btPisoCima_Click(object sender, EventArgs e)
        {
            PisoAcima();
            formDesenho.glControl.Focus();
        }

        private void TimerModo_Tick(object sender, EventArgs e)
        {
            panelModo.Visible = true;
            panelModo.Left = btModo.Left;
            panelModo.Top = Ribbon.Height + btModo.Height+40;
            TimerModo.Stop();
            GL.Clear(ClearBufferMask.ColorBufferBit | ClearBufferMask.DepthBufferBit);
            formDesenho.DesenhaObjetos();
            formDesenho.glControl.SwapBuffers();
        }

        private void button4_Click(object sender, EventArgs e)
        {
            AtivaModo3D();
            panelModo.Visible = false;
        }

        private void button6_Click(object sender, EventArgs e)
        {
            AtivaModoPiso();
            panelModo.Visible = false;
        }

        private void button10_Click(object sender, EventArgs e)
        {
            AtivaModoPlano("xy");
            btModo.Text = (sender as Button).Text;
            panelModo.Visible = false;
        }

        private void button11_Click(object sender, EventArgs e)
        {
            AtivaModoPlano("yz");
            btModo.Text = (sender as Button).Text;
            panelModo.Visible = false;
        }


        private void cbPiso_SelectedIndexChanged(object sender, EventArgs e)
        {
            AlternaPavimento();
        }

        private void ribbonButton40_Click_1(object sender, EventArgs e)
        {
            MessageBox.Show("Ferramenta em desenvolvimento");
           // formDesenho.CommandEdit(Const.ID_GIRAR_ELEMENTOS);
        }
         private void ribbonButton15_Click_1(object sender, EventArgs e)
        {

        }

        private void ribbonButton19_Click_2(object sender, EventArgs e)
        {

        }

        private void ribbonButton34_Click_2(object sender, EventArgs e)
        {

        }
        public void AtualizaDesenho()
        {
            formDesenho.DesenhaObjetos();
            formDesenho.glControl.SwapBuffers();
        }

        private void btSelecaoPiso_Click(object sender, EventArgs e)
        {
            NovoProjeto(false);
            formDesenho.glControl.Focus();
        }

        private void Gerenciador_Load(object sender, EventArgs e)
        {
            this.DoubleBuffered = true;

            // CriaFormDesenho();

            /*  CriaConfiguracoesPadrao();
              CarregaConfiguracaoPadrao();
              SetaEnableToolBarButtons(false);*/

        }

        private void ribbonButton35_Click_3(object sender, EventArgs e)
        {
            
        }
        private void btInsercaoCotas_Click_1(object sender, EventArgs e)
        {
            formDesenho.InsercaoPorCotas = !formDesenho.InsercaoPorCotas;

            if ((string)btInsercaoCotas.Tag == "0")
            {
                btInsercaoCotas.FlatAppearance.BorderColor = System.Drawing.Color.FromArgb(64, 64, 64);
                btInsercaoCotas.Tag = "1";
            }
            else
            {
                btInsercaoCotas.FlatAppearance.BorderColor = System.Drawing.Color.Cyan;
                btInsercaoCotas.Tag = "0";
            }
        }

        private void button15_Click(object sender, EventArgs e)
        {
            floaty.Show();
        }

        private void btSnap_MouseClick(object sender, System.Windows.Forms.MouseEventArgs e)
        {
            menuSnap.Show(btSnap, new Point(e.X, e.Y));
        }

        private void menuSnap_Closing(object sender, ToolStripDropDownClosingEventArgs e)
        {
            if (e.CloseReason == ToolStripDropDownCloseReason.ItemClicked)
                e.Cancel = true;
        }
        
        private void PontoFinal_Click(object sender, EventArgs e)
        {
            formDesenho.snap_PontoFinal = !formDesenho.snap_PontoFinal;
            ConfiguracoesPGi.snap_PontoFinal = formDesenho.snap_PontoFinal;
        }

        private void PontoMedio_Click(object sender, EventArgs e)
        {
            formDesenho.snap_PontoMeio = !formDesenho.snap_PontoMeio;
            ConfiguracoesPGi.snap_PontoMedio = formDesenho.snap_PontoMeio;
        }

        private void Intersecao_Click(object sender, EventArgs e)
        {
            formDesenho.snap_Intersecao = !formDesenho.snap_Intersecao;
            ConfiguracoesPGi.snap_Intersecao = formDesenho.snap_Intersecao;
            if (formDesenho.snap_Intersecao)
                formDesenho.AtualizaListaIntersecoes(null);
        }

        private void ribbonButton29_Click(object sender, EventArgs e)
        {
           
        }

        public void MostraInsercaoCota(string cota)
        {
            formDesenho.pnDivBarras.Left = formDesenho.mouseX + 10;
            formDesenho.pnDivBarras.Top = formDesenho.mouseY - 10;
            formDesenho.pnDivBarras.Visible = true;
            formDesenho.edCota.Focus();
            formDesenho.edCota.Text = cota;
            formDesenho.edCota.SelectionStart = 2;
            formDesenho.aguardandoCota = true;
            formDesenho.lbUnidadeCota.Text = ConfiguracoesPGi.CfgProjeto.unidadesProjeto.geo_un_comprimento.ToString();

        }
        private void Gerenciador_KeyPress(object sender, System.Windows.Forms.KeyPressEventArgs e)
        {
            if (char.IsDigit(e.KeyChar))
            if (formDesenho.distCota1 != 0 && !formDesenho.pnDivBarras.Visible)
            {
                    MostraInsercaoCota(e.KeyChar.ToString());
                    e.Handled = true;
            }
        }

        private void ribbonButton25_Click_1(object sender, EventArgs e)
        {

        }

        private void cbUnComp_SelectedIndexChanged(object sender, EventArgs e)
        {
            //todo calculo eh feito em metros e kn

          /*  if (ribbon2.ActiveTab == ribbonTabEstrutura)
            {
                if (cbUnComp.SelectedIndex == 0) //mm
                    desenho.conversaoComprimento = 1000; // metros para mm
                if (cbUnComp.SelectedIndex == 1) //cm
                    desenho.conversaoComprimento = 100;
                if (cbUnComp.SelectedIndex == 2) //m
                    desenho.conversaoComprimento = 1;
            }
            if (ribbon2.ActiveTab == ribbonTabResultados)
            {
                if (cbUnComp.SelectedIndex == 0) //mm
                    desenho.conversaoComprimentoResultado = 1000; // metros para mm
                if (cbUnComp.SelectedIndex == 1) //cm
                    desenho.conversaoComprimentoResultado = 100;
                if (cbUnComp.SelectedIndex == 2) //m
                    desenho.conversaoComprimentoResultado = 1;*/
     //       }
        }

        private void cbUnForca_SelectedIndexChanged(object sender, EventArgs e)
        {
            //todo calculo eh feito em metros e kn
           /* if (ribbon2.ActiveTab == ribbonTabEstrutura)
            {
                if (cbUnForca.SelectedIndex == 0)
                    desenho.conversaoForca = 1000; //tf para kg
                if (cbUnForca.SelectedIndex == 1)
                    desenho.conversaoForca = 1;
                if (cbUnForca.SelectedIndex == 2)
                    desenho.conversaoForca = 10000;
                if (cbUnForca.SelectedIndex == 3)
                    desenho.conversaoForca = 10;
            }

            if (ribbon2.ActiveTab == ribbonTabResultados)
            {
                if (cbUnForca.SelectedIndex == 0)
                    desenho.conversaoForcaResultado = 100; //kn para kg
                if (cbUnForca.SelectedIndex == 1)
                    desenho.conversaoForcaResultado = 0.1;
                if (cbUnForca.SelectedIndex == 2)
                    desenho.conversaoForcaResultado = 1000;
                if (cbUnForca.SelectedIndex == 3)
                    desenho.conversaoForcaResultado = 1;

                desenho.conversaoForca = desenho.conversaoForcaResultado;
            }*/

            /*kgf
            tf
            N
            kN
            */
        }

        public void cbCargas_SelectedIndexChanged(object sender, EventArgs e)
        {
            
        }

        private void ribbonButton34_Click_3(object sender, EventArgs e)
        {

        }


        private void ribbonButton54_Click_1(object sender, EventArgs e)
        {

           
        }

        private void button5_Click_1(object sender, EventArgs e)
        {
            AtivaModoPlano("xz");
            btModo.Text = (sender as Button).Text;
            panelModo.Visible = false;
        }

        private void menuModoSelecao_Closing(object sender, ToolStripDropDownClosingEventArgs e)
        {
            if (e.CloseReason == ToolStripDropDownCloseReason.ItemClicked)
                e.Cancel = true;
        }

        private void ribbonButton14_Click_1(object sender, EventArgs e)
        {
            ribbonTabResultados.Visible = true;

            btDeformacoes_Click(btDeformacoes2,null);
        }

        public void CancelaResultados()
        {
            if ((string)btDeformacao.Tag == "1")
                btDeformacao_Click(btDeformacao, null);

            if ((string)btDinamica1.Tag == "1")
                btDinamica1_Click(btDinamica1, null);
            
            if ((string)btFlambagem.Tag == "1")
                btFlambagem_Click(btFlambagem, null);

            if ((string)btDiagrama.Tag == "1")
                btDiagrama_Click(btDiagrama, null);

            if ((string)btTensoes.Tag == "1")
                btTensoes_Click(btTensoes, null);

            if ((string)btValoresDiagramas.Tag == "1")
                btValoresDiagramas_Click_1(btValoresDiagramas, null);

            if ((string)btDeformacaoTextos.Tag == "1")
                btDeformacaoTextos_Click(btDeformacaoTextos, null);

            if ((string)btMostraIndeformada.Tag == "1")
                btMostraIndeformada_Click_1(btMostraIndeformada, null);
            
            Ribbon.SelectedTab = tabPrincipal;

            formDesenho.MostrarCargasPeloCaso();
        //    formDesenho.MostraCargasNosPeloCaso();
        }

        private void btDeformacoes_Click(object sender, EventArgs e)
        {
            btDeformacoes2.Checked = !btDeformacoes2.Checked;
            formDesenho.MostraDeformacoes = btDeformacoes2.Checked;                     

            panelDeformacoes2.Visible = btDeformacoes2.Checked;
            ribbonButton5_Click_6(rbDefU, null);
            if (!btDeformacoes2.Checked)
            {
                if (btDeformacaoTextos2.Checked)
                  ribbonButton5_Click_4(btDeformacaoTextos2,null);

                if (btDeformacaoSolida2.Checked)
                    btDeformacaoSolida_Click(btDeformacaoSolida2, null);

                if (rbMostraIndeformada2.Checked)
                    rbMostraIndeformada_Click(rbMostraIndeformada2, null);

               foreach (TBarraGenerica b in formDesenho.Estrutura.barras)
                 b.Visivel = !btDeformacoes2.Checked;

                btAnimarDeformacao2.Checked = false;
                formDesenho.AnimarDeformacoes(false);
            }
            else
            {
                if (btAnimarDeformacao2.Checked)
                {
                    btAnimarDeformacao_Click(btAnimarDeformacao2, null);
                }
                if (btfx2.Checked || btfy2.Checked || btfz2.Checked || btmx2.Checked || btmy2.Checked || btmz2.Checked)
                    return;

                foreach (TBarraGenerica b in formDesenho.Estrutura.barras)
                    b.Visivel = false;

            }
/*
            if (Ribbon.SelectedTab == tabResultados)
            {
                ribbon2.ActiveTab = ribbonTabEstrutura;
                ribbon2.ActiveTab = ribbonTabResultados;
            }*/

 
            formDesenho.AtualizaShaders();
            AtualizaDesenho();
        }

        private void btDefColorida_Click(object sender, EventArgs e)
        {
            btDeformacaoColorida2.Checked = !btDeformacaoColorida2.Checked;
            formDesenho.DeformacaoColorida = btDeformacaoColorida2.Checked;
            formDesenho.AtualizaShaders();
            AtualizaDesenho();
        }

        private void btDeformacaoSolida_Click(object sender, EventArgs e)
        {
            if (formDesenho.Estrutura.PorticoEspacial == null)
                return;
            btDeformacaoSolida2.Checked = !btDeformacaoSolida2.Checked;

            formDesenho.DeformacaoSolida = btDeformacaoSolida2.Checked;

            if (btDeformacaoSolida2.Checked)
            {
                formDesenho.CriaDeformacaoSolida(formDesenho.Estrutura.PorticoEspacial.barras);

                if (escalaDeformacao == 0)
                {
                    edEscalaDeformacao.Text = "0.5";
                    formDesenho.EscalaDiagramas = 0.5;
                }
                else
                {
                    edEscalaDeformacao.Text = escalaDeformacao.ToString("n2");

                    formDesenho.EscalaDiagramas = vEscalaDeformacao;
                }
            }

          /*  foreach (TBarraGenerica b in formDesenho.Estrutura.barras)
                b.Visivel = !btDeformacaoSolida.Checked;*/

            formDesenho.ArestasResultado = !formDesenho.ArestasResultado;
          //  formDesenho.AtualizaDisplayList();
            //AtualizaDesenho();

            ConfirmaEscalaDef();
        }

        private void ribbonButton5_Click_4(object sender, EventArgs e)
        {
            if (formDesenho.Estrutura.PorticoEspacial == null)
                return;

            btDeformacaoTextos2.Checked = !btDeformacaoTextos2.Checked;
            formDesenho.MostraTextoDeformacoes = btDeformacaoTextos2.Checked;

            if (formDesenho.MostraTextoDeformacoes)
            {
                Preenche_nos_valores_deformacao();
            }

            AtualizaDesenho();
        }
        public void Preenche_nos_valores_deformacao()
        {
            if (formDesenho.Estrutura.PorticoEspacial != null)
            {
                double perc = 0;
                if (ConfiguracoesPGi.PorticoOpcoesVisualizacao.exibir_percentual_maximo_deformacao)
                   perc = ConfiguracoesPGi.PorticoOpcoesVisualizacao.percentual_maximo_deformacao;
                else
                if (ConfiguracoesPGi.PorticoOpcoesVisualizacao.exibir_somente_maximo)
                   perc = 100;

                double valor = 0;

                if (ConfiguracoesPGi.PorticoOpcoesVisualizacao.valores == 7)
                    valor = formDesenho.Estrutura.PorticoEspacial.Maximo_U_Total(cbTipoCargaResultado.SelectedIndex,
                                                                                 cbResultadoCasos.SelectedIndex,
                                                                                 cbResultadoCombinacoes.SelectedIndex);
                else
                    valor = formDesenho.Estrutura.PorticoEspacial.MaximoDeslocamento(ConfiguracoesPGi.PorticoOpcoesVisualizacao.valores,
                                                                           cbTipoCargaResultado.SelectedIndex,
                                                                           cbResultadoCasos.SelectedIndex,
                                                                           cbResultadoCombinacoes.SelectedIndex);

                double maxDef_perc = valor * (perc / 100);

                List<TNoPortico> nos = new List<TNoPortico>();
                double[] p_Deslocamento;

                if (ConfiguracoesPGi.PorticoOpcoesVisualizacao.valores == 7)
                {
                    if (ConfiguracoesPGi.PorticoOpcoesVisualizacao.exibir_tudo)
                    {
                        for (int i = 1; i <= formDesenho.Estrutura.PorticoEspacial.nNos; i++)
                          nos.Add(formDesenho.Estrutura.PorticoEspacial.nos[i]);
                    }
                    else
                    {
                        double utotal;
                        for (int i = 1; i <= formDesenho.Estrutura.PorticoEspacial.nNos; i++)
                        {
                            if (cbTipoCargaResultado.SelectedIndex == 0)
                                utotal = formDesenho.Estrutura.PorticoEspacial.nos[i].casos_x_deslocamentos[cbResultadoCasos.SelectedIndex].U_Total;
                            else
                                utotal = formDesenho.Estrutura.PorticoEspacial.nos[i].combinacoes_x_deslocamentos[cbResultadoCombinacoes.SelectedIndex].U_Total;

                            if (utotal >= maxDef_perc)
                              nos.Add(formDesenho.Estrutura.PorticoEspacial.nos[i]);
                        }
                    }
                }
                else
                {
                    if (ConfiguracoesPGi.PorticoOpcoesVisualizacao.exibir_tudo)
                    {
                        for (int i = 1; i <= formDesenho.Estrutura.PorticoEspacial.nNos; i++)
                        {
                            nos.Add(formDesenho.Estrutura.PorticoEspacial.nos[i]);
                        }
                    }
                    else
                    {

                        for (int i = 1; i <= formDesenho.Estrutura.PorticoEspacial.nNos; i++)
                        {
                            if (cbTipoCargaResultado.SelectedIndex == 0)
                                p_Deslocamento = formDesenho.Estrutura.PorticoEspacial.nos[i].casos_x_deslocamentos[cbResultadoCasos.SelectedIndex].DeslocamentoGlobal;
                            else
                                p_Deslocamento = formDesenho.Estrutura.PorticoEspacial.nos[i].combinacoes_x_deslocamentos[cbResultadoCombinacoes.SelectedIndex].DeslocamentoGlobal;

                            if (Math.Abs(p_Deslocamento[ConfiguracoesPGi.PorticoOpcoesVisualizacao.valores]) >= maxDef_perc)
                                nos.Add(formDesenho.Estrutura.PorticoEspacial.nos[i]);
                        }
                    }
                }
                formDesenho.nos_deformacao = nos.ToArray();
            }
        }

        public void Preenche_barras_valores_esforco()
        {
            if (formDesenho.Estrutura.PorticoEspacial != null)
            {
                double perc = 0;

                formDesenho.barras_esforco = new List<TBarraPortico>();

                int esf_i = 0, esf_f = 0;
                vec3 coordEsforco_i = new vec3(0);
                vec3 coordEsforco_f = new vec3(0);

                double valor = 0;

                if (formDesenho.fx)
                {
                    esf_i = 1; esf_f = 7;
                    valor = formDesenho.Estrutura.PorticoEspacial.MaximoEsforco("fx", cbTipoCargaResultado.SelectedIndex,
                                                                                 cbResultadoCasos.SelectedIndex,
                                                                                 cbResultadoCombinacoes.SelectedIndex,
                                                                                 formDesenho.MostrarEsforcosEmSelecionados);
                }
                else
                if (formDesenho.fz)
                {
                    esf_i = 2; esf_f = 8;
                    valor = formDesenho.Estrutura.PorticoEspacial.MaximoEsforco("fz", cbTipoCargaResultado.SelectedIndex,
                                                                                 cbResultadoCasos.SelectedIndex,
                                                                                 cbResultadoCombinacoes.SelectedIndex,
                                                                                 formDesenho.MostrarEsforcosEmSelecionados);
                }
                else
                if (formDesenho.fy)
                {
                    esf_i = 3; esf_f = 9;
                    valor = formDesenho.Estrutura.PorticoEspacial.MaximoEsforco("fy", cbTipoCargaResultado.SelectedIndex, cbResultadoCasos.SelectedIndex, cbResultadoCombinacoes.SelectedIndex,
                                                                                 formDesenho.MostrarEsforcosEmSelecionados);
                }
                else
                if (formDesenho.mx)
                {
                    esf_i = 4; esf_f = 10;
                    valor = formDesenho.Estrutura.PorticoEspacial.MaximoEsforco("mx", cbTipoCargaResultado.SelectedIndex, cbResultadoCasos.SelectedIndex, cbResultadoCombinacoes.SelectedIndex,
                                                                                 formDesenho.MostrarEsforcosEmSelecionados);
                }
                else
                if (formDesenho.my)
                {
                    esf_i = 6; esf_f = 12;
                    valor = formDesenho.Estrutura.PorticoEspacial.MaximoEsforco("my", cbTipoCargaResultado.SelectedIndex, cbResultadoCasos.SelectedIndex, cbResultadoCombinacoes.SelectedIndex,
                                                                                 formDesenho.MostrarEsforcosEmSelecionados);
                }
                else
                if (formDesenho.mz)
                {
                    esf_i = 5; esf_f = 11;
                    valor = formDesenho.Estrutura.PorticoEspacial.MaximoEsforco("mz", cbTipoCargaResultado.SelectedIndex, cbResultadoCasos.SelectedIndex, cbResultadoCombinacoes.SelectedIndex,
                                                                                 formDesenho.MostrarEsforcosEmSelecionados);
                }

                perc = ConfiguracoesPGi.DiagramaOpcoesVisualizacao.percentual_maximo;

                double maxEsf_perc = valor * (perc / 100);

                List<TBarraPortico> barras = new List<TBarraPortico>();

                for (int i = 1; i <= formDesenho.Estrutura.PorticoEspacial.nBarras; i++)
                {
                    if ((formDesenho.MostrarEsforcosEmSelecionados && formDesenho.Estrutura.PorticoEspacial.barras[i].barraOriginal.Selecionado) || (!formDesenho.MostrarEsforcosEmSelecionados))
                    {
                        if (cbTipoCargaResultado.SelectedIndex == 0)
                        {
                            if (maxEsf_perc != 0)
                            {
                                if ((Math.Abs(formDesenho.Estrutura.PorticoEspacial.barras[i].casos_x_esforcos[cbResultadoCasos.SelectedIndex].Esforcos[esf_i]) > maxEsf_perc || Geom.Iguais(Math.Abs(formDesenho.Estrutura.PorticoEspacial.barras[i].casos_x_esforcos[cbResultadoCasos.SelectedIndex].Esforcos[esf_i]), maxEsf_perc)) ||
                                    (Math.Abs(formDesenho.Estrutura.PorticoEspacial.barras[i].casos_x_esforcos[cbResultadoCasos.SelectedIndex].Esforcos[esf_f]) > maxEsf_perc || Geom.Iguais(Math.Abs(formDesenho.Estrutura.PorticoEspacial.barras[i].casos_x_esforcos[cbResultadoCasos.SelectedIndex].Esforcos[esf_f]), maxEsf_perc)))
                                    barras.Add(formDesenho.Estrutura.PorticoEspacial.barras[i]);
                            }
                            else
                                barras.Add(formDesenho.Estrutura.PorticoEspacial.barras[i]);
                        }
                        else
                        {
                            if (maxEsf_perc != 0)
                            {
                                if ((Math.Abs(formDesenho.Estrutura.PorticoEspacial.barras[i].combinacoes_x_esforcos[cbResultadoCombinacoes.SelectedIndex].Esforcos[esf_i]) > maxEsf_perc || Geom.Iguais(Math.Abs(formDesenho.Estrutura.PorticoEspacial.barras[i].combinacoes_x_esforcos[cbResultadoCombinacoes.SelectedIndex].Esforcos[esf_i]), maxEsf_perc)) ||
                                    (Math.Abs(formDesenho.Estrutura.PorticoEspacial.barras[i].combinacoes_x_esforcos[cbResultadoCombinacoes.SelectedIndex].Esforcos[esf_f]) > maxEsf_perc || Geom.Iguais(Math.Abs(formDesenho.Estrutura.PorticoEspacial.barras[i].combinacoes_x_esforcos[cbResultadoCombinacoes.SelectedIndex].Esforcos[esf_f]), maxEsf_perc)))
                                    barras.Add(formDesenho.Estrutura.PorticoEspacial.barras[i]);
                            }
                            else
                                barras.Add(formDesenho.Estrutura.PorticoEspacial.barras[i]);
                        }
                    }
                }              

                formDesenho.barras_esforco = barras;
            }
        }

        private void ribbonButton60_Click(object sender, EventArgs e)
        {
            DialogResult result = DialogResult.Yes;
            {
                ConfiguraDeformacao = new FConfiguraDeformacao(this);

                result = ConfiguraDeformacao.ShowDialog(this);
            }

            if (result == DialogResult.Yes)
            {
                /*   FVisPortico.Atualizar();
                   if (FVisPortico != null)
                   {
                       FVisPortico.AtualizaEsforcos("Atualizando pórtico. Aguarde...");
                       FVisPortico.SetupCamera(!ConfiguracoesPGi.PorticoOpcoesVisualizacao.Perspectiva);
                   }*/
            }
        }


        private void ribbonButton55_Click_2(object sender, EventArgs e)
        {
            if (formDesenho.EscalaDiagramas - 1.3 < 0)
                formDesenho.EscalaDiagramas = 0;
            else
                formDesenho.EscalaDiagramas -= 1.3;

            formDesenho.AtualizaShaders();

          //  if (btDeformacaoSolida.Checked)
            //    formDesenho.CriaDeformacaoSolida(formDesenho.Estrutura.PorticoEspacial.barras); 

            AtualizaDesenho();
        }
        System.Drawing.Graphics GraphicsMatrizRigidez;
        System.Drawing.Pen mPen = new System.Drawing.Pen(System.Drawing.Color.Black);
        System.Drawing.Brush aBrush = (System.Drawing.Brush)System.Drawing.Brushes.Black;
        void DesenhaMatriz()
        {
            int tamQuadrado = 1;
            if (mostrarMatrizDeRigidezDuranteCalculo.Checked)
            {
                int Tam = formDesenho.Estrutura.PorticoEspacial.NLinhas;
              //  int tamQuadrado = pnMatrizRigidez.Width-80;
                int tamBlocos = (int)(Tam / tamQuadrado);

                try
                {
                    int i = 0, j = 0, k = 0, l = 0;
                    int lin = 0, col = 0;
                    bool achou = false;
                    if (formDesenho.Estrutura.PorticoEspacial.MatrizRigidez != null)
                    {
                        for (i = 0; i < tamQuadrado; i++)
                        {
                            lin = (i * tamBlocos);
                 ///           pnMatrizRigidez.Update();

                            try
                            {
                                for (j = 0; j < tamQuadrado; j++)
                                {
                                    col = (j * tamBlocos) - 1;
                                    achou = false;
                                    for (k = 0; k < tamBlocos; k++)
                                    {
                                        lin += k;
                                        for (l = 0; l < tamBlocos; l++)
                                        {
                                            col += 1;

                                            if (!Geom.Iguais(alglib.sparseget(formDesenho.Estrutura.PorticoEspacial.MatrizRigidez.s, lin, col), 0))
                                            {
                                                GraphicsMatrizRigidez.FillRectangle(aBrush, i + 1, j + 1, 1, 1);
                                                achou = true;
                                                break;
                                            }
                                        }
                                        if (achou)
                                            break;

                                        col = (j * tamBlocos) - 1;
                                    }

                                    lin = (i * tamBlocos);
                                }
                            }
                            catch (Exception)
                            {
                                //      MessageBox.Show("i:" + i.ToString() + " j:" + j.ToString() + " k:" + k.ToString() + " l:" + l.ToString()
                                //     + " lin:" + lin.ToString() + " col:" +col.ToString());
                            }

                        }
                    }
                }

                catch (Exception e)
                {
                    MessageBox.Show(e.Message);
                }
            }
        }

        private void pnMatrizRigidez_Paint(object sender, PaintEventArgs e)
        {
            GraphicsMatrizRigidez = e.Graphics;
            //      ControleCAD.DrawLine(mPen, 0, 0, 50, 50);


            if (formDesenho.Estrutura.PorticoEspacial != null)
              DesenhaMatriz();
        }
        void ConfirmaEscalaDef()
        {
            formDesenho.DirtyPortico();

            if (edEscalaDeformacao.Text.ToString() == string.Empty)
                return; 
            vEscalaDeformacao = double.Parse(edEscalaDeformacao.Text.ToString());
            formDesenho.fatorDeformacao = vEscalaDeformacao;
            if ((string)btAnimarDeformacao.Tag == "1")
                btAnimarDeformacao_Click_1(btAnimarDeformacao, null);

            ChamarAtualizacaoResultados(btConfirmaDeslocamentos, true);

            //formDesenho.AtualizaShaders();
            // AtualizaDesenho();  
        }
        void ConfirmaEscalaModoVibracao()
        {
            if (edEscalaModo.Text.ToString() == string.Empty)
                return;

            vEscalaModoVibracao = double.Parse(edEscalaModo.Text.ToString());
            // O campo já contém o fator absoluto, como na escala das deformações.
            formDesenho.fatorModoVibracao = vEscalaModoVibracao;
            formDesenho.DirtyPortico();

            ChamarAtualizacaoResultados(btConfirmaModos, true);
        }

        void ConfirmaEscalaFlambagem()
        {
            if (edEscalaFlambagem.Text.ToString() == string.Empty)
                return;

            vEscalaFlambagem = double.Parse(edEscalaFlambagem.Text.ToString());
            // O campo já contém o fator absoluto, como na escala das deformações.
            formDesenho.fatorFlambagem = vEscalaFlambagem;
            formDesenho.DirtyPortico();

            ChamarAtualizacaoResultados(btConfirmaFlambagem, true);
        }

        void ConfirmaEscalaDiagrama()
        {
            if (edEscalaDiagrama.Text.ToString() == string.Empty)
                return;

            vEscalaDiagrama = double.Parse(edEscalaDiagrama.Text.ToString());
            formDesenho.fatorDiagramas = vEscalaDiagrama * escalaDiagrama;
            
         //   formDesenho.AtualizaShaders();
            if (formDesenho.MostraTextoDiagramas)
            {
                //if (formDesenho.fx || formDesenho.mx)
                //    formDesenho.CriarTextosAxial_Torcor();
             //   else
          //          formDesenho.CriarTextosEsforco();
            }
            ChamarAtualizacaoResultados(btOkEsforco, true);

            //  formDesenho.AtualizaShaders();
            //     AtualizaDesenho();

        }

        private void ribbonUpDown1_TextBoxKeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyData == Keys.Enter)
            {
                ConfirmaEscalaDef(); 
            }
        }





        FOpcoesDeformacaoSolida FOpcDefSolida;

        private void btUndo_Click(object sender, EventArgs e)
        {
            
        }

        private void btAnimarDeformacao_Click(object sender, EventArgs e)
        {
            btAnimarDeformacao2.Checked = !btAnimarDeformacao2.Checked;
            formDesenho.AnimarDeformacoes(btAnimarDeformacao2.Checked);
        }

        private void btBarra_MouseEnter(object sender, System.Windows.Forms.MouseEventArgs e)
        {
            lbTip.BringToFront();
            lbTip.Visible = true;
            lbTip.Left = e.X;
            lbTip.Top = e.Y+25;
            lbTip.Text = (sender as RibbonButton).Value.ToString();
           // this.Refresh();
        }

        private void btBarra_MouseLeave(object sender, System.Windows.Forms.MouseEventArgs e)
        {
            lbTip.Visible = false;
        }

        private void ribbon2_MouseLeave(object sender, EventArgs e)
        {
           // lbTip.Visible = false;
        }

        private void ribbon2_MouseLeave_1(object sender, EventArgs e)
        {
            lbTip.Visible = false;
        }


        private void comboBox1_SelectedIndexChanged(object sender, EventArgs e)
        {
            formDesenho.MostrarCargasPeloCaso();
        }
        ToolTip tipBotoes = new ToolTip();
        private void btBarra_MouseEnter(object sender, EventArgs e)
        {
            Button btn = (Button)sender;
            tipBotoes.SetToolTip(btn, btn.AccessibleName);
            /*lbTip.BringToFront();
            lbTip.Visible = true;
            lbTip.Left = e.X;
            lbTip.Top = e.Y + 25;
            lbTip.Text = (sender as Button).AccessibleName;*/
        }

        private void menuSelecao_Closing(object sender, ToolStripDropDownClosingEventArgs e)
        {
            if (e.CloseReason == ToolStripDropDownCloseReason.ItemClicked)
                e.Cancel = true;
        }

        private void toolStripComboBox1_SelectedIndexChanged_1(object sender, EventArgs e)
        {
            formDesenho.MostrarCargasPeloCaso();
        }


        private void toolStripButton1_Click_1(object sender, EventArgs e)
        {

        }

        private void btModo_Click(object sender, EventArgs e)
        {
            panelModo.Visible = true;
            panelModo.Left = btModo.Left;
            panelModo.Top = Ribbon.Height + btModo.Height +20;
            TimerModo.Stop();
            GL.Clear(ClearBufferMask.ColorBufferBit | ClearBufferMask.DepthBufferBit);
            formDesenho.DesenhaObjetos();
            formDesenho.glControl.SwapBuffers();
        }

        private void button2_Click_2(object sender, EventArgs e)
        {
            if (FatorCarga == null)
                FatorCarga = new FFatorCargas(this);

            FatorCarga.Show();
        }

        private void button1_Click_2(object sender, EventArgs e)
        {
            if (OpcoesCaptura == null)
                OpcoesCaptura = new FOpcoesCaptura(this);

            OpcoesCaptura.Show();
        }

        private void ribbonButton16_Click_1(object sender, EventArgs e)
        {

        }

        private void ribbonButton17_Click(object sender, EventArgs e)
        {
            CancelaResultados();
            formDesenho.MostrarTudo();
        }
        public ChecarConectividades checarconectividade;
        private void ribbonButton31_Click(object sender, EventArgs e)
        {

        }

        private void Perpendicular_Click(object sender, EventArgs e)
        {
            formDesenho.snap_Perpendicular = !formDesenho.snap_Perpendicular;
            ConfiguracoesPGi.snap_Perpendicular = formDesenho.snap_Perpendicular;
        }

        private void ribbonButton45_Click(object sender, EventArgs e)
        {
            MessageBox.Show("Ferramenta em desenvolvimento");
        }

        private void ribbonButton48_Click(object sender, EventArgs e)
        {

        }
        int ultimaTab = 1;
        private void flatTabControl1_SelectedIndexChanged(object sender, EventArgs e)
        {
            for (int i = 0; i < Ribbon.TabCount; i++)
            {
                if (i == Ribbon.SelectedIndex)
                    Ribbon.TabPages[i].ForeColor = System.Drawing.Color.White;
                else
                    Ribbon.TabPages[i].ForeColor = System.Drawing.Color.Gray;
            }

            if (Ribbon.SelectedIndex == 0)
            {
                pnArquivo.BringToFront();
                Ribbon.SelectedIndex = ultimaTab;
                Ribbon.TabPages[0].ForeColor = System.Drawing.Color.White;
                pnArquivo.Top = 25;
                pnArquivo.Left = 0;
                pnArquivo.Visible = true;
            }
            ultimaTab = Ribbon.SelectedIndex;
        }

        private void tabPage1_Click(object sender, EventArgs e)
        {
            pnArquivo.Visible = false;
        }

        private void tabPage2_Click(object sender, EventArgs e)
        {
            pnArquivo.Visible = false;
        }

        private void Gerenciador_Click(object sender, EventArgs e)
        {
            pnArquivo.Visible = false;
        }

        private void Ribbon_Selecting(object sender, TabControlCancelEventArgs e)
        {
            if (Ribbon.SelectedTab == tabResultados)
            {
                pnCombinacoes.Top = 2;
                pnCasos.Top = 2;
                cbTipoCargaResultado.Top = 2;
                AlternaComboBoxResultados();
                cbTipoCargaResultado.Visible = true;

                /* cbTipoCargaResultado.Top = pnVisualizacaoCargas.Top;
                  cbTipoCargaResultado.Visible = true;*/
                label13.Visible = false;
                label13.Text = "Combinações";
                pnVisualizacaoCargas.Visible = false;
                if (formDesenho.Diagrama_gradiente && (formDesenho.my || formDesenho.mz || formDesenho.mx || formDesenho.fx || formDesenho.fy || formDesenho.fz))
                  pnCorResultados.Visible = true;

                if (formDesenho.DeformacaoColorida)
                    pnCorResultados.Visible = true;

                if (formDesenho.MostraTensoesNormaisGradiente)
                    pnCorResultados.Visible = true;

            }
            else
            {
                label13.Visible = true;
                if (formDesenho.Diagrama_gradiente || formDesenho.DeformacaoColorida)
                    pnCorResultados.Visible = false;
                cbTipoCargaResultado.Visible = false;
                label13.Text = "Casos de carga";
                pnCombinacoes.Visible = false;
                pnCasos.Visible = false;
                pnVisualizacaoCargas.Visible = true;
            }

        }

        private void Ribbon_Move(object sender, EventArgs e)
        {

        }

        private void tabPrincipal_MouseHover(object sender, EventArgs e)
        {
           
        }

        private void button15_Click_1(object sender, EventArgs e)
        {
            Abrir();
        }

        private void panel28_Paint(object sender, PaintEventArgs e)
        {

        }

        public void AlternarVisualizacaoCargas()
        {
            formDesenho.MostrarCargas = !formDesenho.MostrarCargas;
            formDesenho.MostrarCargasPeloCaso();

            if ((string)btVisualizarCargas.Tag == "0")
            {
                btVisualizarCargas.FlatAppearance.BorderColor = System.Drawing.Color.FromArgb(64, 64, 64);
                btVisualizarCargas.Tag = "1";
            }
            else
            {
                btVisualizarCargas.FlatAppearance.BorderColor = System.Drawing.Color.Cyan;
                btVisualizarCargas.Tag = "0";
            }
        }
        private void btVisualizarCargas_Click(object sender, EventArgs e)
        {
            AlternarVisualizacaoCargas();
        }

        private void cbCasoCarga_SelectedIndexChanged(object sender, EventArgs e)
        {
            formDesenho.MostrarCargasPeloCaso();
        }

        private void button14_Click(object sender, EventArgs e)
        {
            NovoProjeto(true);
        }

        private void button16_Click(object sender, EventArgs e)
        {
            Salvar();
        }

        private void button18_Click(object sender, EventArgs e)
        {
            formDesenho.Desfazer();
        }

        private void button19_Click(object sender, EventArgs e)
        {
            if (this.ActiveMdiChild == desenho)
            {
                formDesenho.Enquadrar();
                // formDesenho.Enquadrar();
            }
        }

        private void button22_Click(object sender, EventArgs e)
        {
            if (fConfiguracaoPrograma == null)
            {
                fConfiguracaoPrograma = new FConfiguracaoPrograma(this);
                fConfiguracaoPrograma.Show(this);
            }
        }

        private void button21_Click(object sender, EventArgs e)
        {
            ChamaCfgCalculo();
        }

        private void button20_Click(object sender, EventArgs e)
        {
            if (Materiais == null)
                Materiais = new FMateriais(this);
            Materiais.Show(this);
        }

        private void button25_Click(object sender, EventArgs e)
        {
            CriaNovaBarra();
        }

        private void button24_Click(object sender, EventArgs e)
        {
            CriaNovoApoio();
        }

        private void button26_Click(object sender, EventArgs e)
        {
            if (checarconectividade == null)
            {
                checarconectividade = new ChecarConectividades(this);
                checarconectividade.Show(this);
            }
        }

        private void button28_Click(object sender, EventArgs e)
        {
            if (CasosCarga == null)
            {
                CasosCarga = new FCasosCarga(this);
                CasosCarga.Show(this);
            }
        }

        private void button3_Click(object sender, EventArgs e)
        {
            if (Combinacoes == null)
            {
                Combinacoes = new FCombinacoes(this);
                Combinacoes.Show(this);
            }
        }

        private void button29_Click(object sender, EventArgs e)
        {
            if (CargaNodal == null)
            {
                CargaNodal = new FCargaNodal(this, "", null, false);

                CargaNodal.Show(this);
            }
        }

        private void button30_Click(object sender, EventArgs e)
        {
            if (CargaBarra == null)
            {
                CargaBarra = new FCarga(this, "", null, null, false);

                CargaBarra.Show(this);
            }
        }

        private void button34_Click(object sender, EventArgs e)
        {
            CancelaResultados();
            formDesenho.MostrarTudo();

            Calcular();
        }

        private void button35_Click(object sender, EventArgs e)
        {
            formDesenho.RemeverDosObjetosSelecionados_Nao_Copiaveis();

            if (formDesenho.ObjetosSelecionados.Count > 0)
            {
                formDesenho.ComandoEdicao(Const.ID_MOVER_ELEMENTOS, eTipoComando.edit);
                formDesenho.MouseEdit(0, 0, 0, 0, "", 0);
            }
            else
                formDesenho.ComandoEdicao(Const.ID_MOVER_ELEMENTOS);
            /* if (MoverCopiar != null)
                 MoverCopiar.Close();
             MoverCopiar = new FMoverCopiar(this);
             MoverCopiar.Show();*/
        }

        private void button33_Click(object sender, EventArgs e)
        {
            formDesenho.RemeverDosObjetosSelecionados_Nao_Copiaveis();

            if (formDesenho.ObjetosSelecionados.Count > 0)
            {
                formDesenho.ComandoEdicao(Const.ID_MOVER_EXTREMO_ELEMENTOS, eTipoComando.edit);
                formDesenho.MouseEdit(0, 0, 0, 0, "", 0);
            }
            else
                formDesenho.ComandoEdicao(Const.ID_MOVER_EXTREMO_ELEMENTOS);
        }

        private void button32_Click(object sender, EventArgs e)
        {
            formDesenho.RemeverDosObjetosSelecionados_Nao_Copiaveis();

            if (formDesenho.ObjetosSelecionados.Count > 0)
            {
                formDesenho.ComandoEdicao(Const.ID_COPIAR_ELEMENTOS, eTipoComando.edit);
                formDesenho.MouseEdit(0, 0, 0, 0, "", 0);
            }
            else
                formDesenho.ComandoEdicao(Const.ID_COPIAR_ELEMENTOS, eTipoComando.selecionar);

            /*if (MoverCopiar != null)
                 MoverCopiar.Close(); 
             MoverCopiar = new FMoverCopiar(this);
             MoverCopiar.Show();*/
        }

        private void button40_Click(object sender, EventArgs e)
        {
            
        }

        private void button39_Click(object sender, EventArgs e)
        {

        }

        private void button37_Click(object sender, EventArgs e)
        {
            if (sobre == null)
            {
                sobre = new FSobre(this);

            }
            sobre.Show();
        }

        private void btDeformacao_Click(object sender, EventArgs e)
        {
            formDesenho.AtualizaShaders();
            /*if ((string)btDeformacao.Tag == "1")
            {
                btDeformacao.BackColor = System.Drawing.Color.FromArgb(64, 64, 64);
                btDeformacao.Tag = "0";
            }
            else
            {


                btDeformacao.BackColor = System.Drawing.Color.White;
                btDeformacao.Tag = "1";
            }
            */
            UpdateCorBotao(btDeformacao);

            //  if ((string)btTensoes.Tag == "1")
            //      btTensoes_Click(btTensoes, null);
            cbFiltroDeslocamentos.SelectedIndex = 0;

            bool deformar = (string)btDeformacao.Tag == "1";

            formDesenho.MostraDeformacoes = deformar;

            panelDeformacoes.Visible = deformar;
           
            btDefTotal_Click(btDefTotal, null);
            
            if (!deformar)
            {
                formDesenho.Text = "Modelo 3D";

                btDefTotal.BackColor = System.Drawing.Color.FromArgb(64, 64, 64);
                btDefTotal.Tag = "0";

                if ((string)btDeformacaoTextos.Tag == "1")
                    btDeformacaoTextos_Click(btDeformacaoTextos, null);

                if ((string)btDeformacaoSolida.Tag == "1")
                    btDeformacaoSolida_Click_1(btDeformacaoSolida, null);
               
                if ((string)btDeformacaoColorida.Tag == "1")
                    btDeformacaoColorida_Click(btDeformacaoColorida, null);

                if ((string)btMostraIndeformada.Tag == "1")
                    btMostraIndeformada_Click_1(btMostraIndeformada, null);

                formDesenho.MostraTextoDeformacoes = false;

                foreach (TBarraGenerica b in formDesenho.Estrutura.barras)
                {
                    b.Visivel = !btDeformacoes2.Checked;
                    b.DirtyTriangulos = true;
                    b.DirtyArestas = true;
                    b.DirtySelecao = true;
                }

                btAnimarDeformacao.Tag = "0";
                btAnimarDeformacao.BackColor = System.Drawing.Color.FromArgb(64, 64, 64);

                formDesenho.AnimarDeformacoes(false);

                pnCorResultados.Visible = false;
            }
            else
            {
                //cancela os outros resultados
                if ((string)btTensoes.Tag == "1")
                    btTensoes_Click(btTensoes, null);
                if ((string)btDinamica1.Tag == "1")
                  btDinamica1_Click(btDinamica1, null);
                if ((string)btFlambagem.Tag == "1")
                    btFlambagem_Click(btFlambagem, null);

                formDesenho.Text = "Resultados: " + btDeformacao.AccessibleName;

                if ((string)btAnimarDeformacao.Tag == "1")
                {
                    btAnimarDeformacao_Click_1(btAnimarDeformacao, null);
                }
                if ((string)btfx.Tag == "1" || (string)btfy.Tag == "1" || (string)btfz.Tag == "1" || (string)btmx.Tag == "1" || (string)btmy.Tag == "1" || (string)btmz.Tag == "1")
                    return;

                foreach (TBarraGenerica b in formDesenho.Estrutura.barras)
                {
                    b.Visivel = false;
                    b.DirtyTriangulos = true;
                    b.DirtyArestas = true;
                    b.DirtySelecao = true;
                }

                formDesenho.DirtyPortico();

            }

            formDesenho.AtualizaShaders();
            AtualizaDesenho();
        }

        private void btDefTotal_Click(object sender, EventArgs e)
        {
            btDefY.Tag = "0";
            btDefZ.Tag = "0";
            btDefX.Tag = "0";

            btDefY.BackColor = System.Drawing.Color.FromArgb(64, 64, 64);
            btDefX.BackColor = System.Drawing.Color.FromArgb(64, 64, 64);
            btDefZ.BackColor = System.Drawing.Color.FromArgb(64, 64, 64);

         /*   if ((string)btDefTotal.Tag == "1")
            {
                btDefTotal.BackColor = System.Drawing.Color.FromArgb(64, 64, 64);
                btDefTotal.Tag = "0";
            }
            else
            {*/
                btDefTotal.BackColor = System.Drawing.Color.White;
                btDefTotal.Tag = "1";
            //}

            formDesenho.deformacao_U = 4;
            formDesenho.DirtyPortico();
            ChamarAtualizacaoResultados(btConfirmaDeslocamentos, true);

            //formDesenho.AtualizaShaders();
            //AtualizaDesenho();
        }

        private void btDefY_Click(object sender, EventArgs e)
        {
            btDefX.Tag = "0";
            btDefZ.Tag = "0";
            btDefTotal.Tag = "0";
            btDefTotal.BackColor = System.Drawing.Color.FromArgb(64, 64, 64);
            btDefX.BackColor = System.Drawing.Color.FromArgb(64, 64, 64);
            btDefZ.BackColor = System.Drawing.Color.FromArgb(64, 64, 64);

          /*  if ((string)btDefY.Tag == "1")
            {
                btDefY.BackColor = System.Drawing.Color.FromArgb(64, 64, 64);
                btDefY.Tag = "0";
            }
            else
            {*/
                btDefY.BackColor = System.Drawing.Color.White;
                btDefY.Tag = "1";
          //  }

            formDesenho.deformacao_U = 2;
            formDesenho.DirtyPortico();
            ChamarAtualizacaoResultados(btConfirmaDeslocamentos, true);
        }

        private void btDefX_Click(object sender, EventArgs e)
        {
            btDefY.Tag = "0";
            btDefZ.Tag = "0";
            btDefTotal.Tag = "0";

            btDefTotal.BackColor = System.Drawing.Color.FromArgb(64, 64, 64);
            btDefY.BackColor = System.Drawing.Color.FromArgb(64, 64, 64);
            btDefZ.BackColor = System.Drawing.Color.FromArgb(64, 64, 64);

            /*    if ((string)btDefX.Tag == "1")
                {
                    btDefX.BackColor = System.Drawing.Color.FromArgb(64, 64, 64);
                    btDefX.Tag = "0";
                }*/
            /*  else
              {
                  btDefX.BackColor = System.Drawing.Color.White;
                  btDefX.Tag = "1";
              }*/
           
            btDefX.BackColor = System.Drawing.Color.White;
            btDefX.Tag = "1";
            formDesenho.deformacao_U = 1;
            formDesenho.DirtyPortico();
            ChamarAtualizacaoResultados(btConfirmaDeslocamentos, true);
        }

        private void btDefZ_Click(object sender, EventArgs e)
        {
            btDefY.Tag = "0";
            btDefX.Tag = "0";
            btDefTotal.Tag = "0";

            btDefTotal.BackColor = System.Drawing.Color.FromArgb(64, 64, 64);
            btDefX.BackColor = System.Drawing.Color.FromArgb(64, 64, 64);
            btDefY.BackColor = System.Drawing.Color.FromArgb(64, 64, 64);

           /* if ((string)btDefZ.Tag == "1")
            {
                btDefZ.BackColor = System.Drawing.Color.FromArgb(64, 64, 64);
                btDefZ.Tag = "0";
            }
            else
            {*/
                btDefZ.BackColor = System.Drawing.Color.White;
                btDefZ.Tag = "1";
           // }

            formDesenho.deformacao_U = 3;
            formDesenho.DirtyPortico();
            ChamarAtualizacaoResultados(btConfirmaDeslocamentos, true);
        }

        private void btDiagrama_Click(object sender, EventArgs e)
        {
            if ((string)btDiagrama.Tag == "1")
            {
                btDiagrama.BackColor = System.Drawing.Color.FromArgb(64, 64, 64);
                btDiagrama.Tag = "0";
            }
            else
            {
                if (necessitaCalculo)
                {
                    MessageBox.Show("Modificações foram feitas. É necessário calcular a estrutura.", "Atenção", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                    return;
                }

                btDiagrama.BackColor = System.Drawing.Color.White;
                btDiagrama.Tag = "1";
            }

            bool diagramas = (string)btDiagrama.Tag == "1";
            
            cbFiltroEsforcos.SelectedIndex = 0;

            panelDiagramas.Visible = diagramas;
            if (!diagramas)
            {
                formDesenho.Text = "Modelo 3D";

                CancelaDiagramas();

                formDesenho.MostraTextoDiagramas = false;
                if ((string)btValoresDiagramas.Tag == "1")
                    btValoresDiagramas_Click_1(btValoresDiagramas, null);

                foreach (TBarraGenerica b in formDesenho.Estrutura.barras)
                {
                    b.DirtyTriangulos = true;
                    b.DirtyArestas = true;
                    b.DirtySelecao = true;
                    b.Visivel = (string)btDeformacao.Tag == "0";
                }

                btfx.Tag = "0";
                btfy.Tag = "0";
                btfz.Tag = "0";
                btmx.Tag = "0";
                btmy.Tag = "0";
                btmz.Tag = "0";

                btfx.BackColor = System.Drawing.Color.FromArgb(64, 64, 64);
                btfz.BackColor = System.Drawing.Color.FromArgb(64, 64, 64);
                btfy.BackColor = System.Drawing.Color.FromArgb(64, 64, 64);
                btmx.BackColor = System.Drawing.Color.FromArgb(64, 64, 64);
                btmz.BackColor = System.Drawing.Color.FromArgb(64, 64, 64);
                btmy.BackColor = System.Drawing.Color.FromArgb(64, 64, 64);

                formDesenho.fx = false;
                formDesenho.fy = false;
                formDesenho.fz = false;
                formDesenho.mx = false;
                formDesenho.my = false;
                formDesenho.mz = false;

                pnCorResultados.Visible = false;
            }
            else
            {
                formDesenho.Text = "Resultados: " + btDiagrama.AccessibleName;

                foreach (TBarraGenerica b in formDesenho.Estrutura.barras)
                {
                    b.Visivel = true;
                    b.DirtyTriangulos = true;
                    b.DirtyArestas = true;
                    b.DirtySelecao = true;
                }
            }

//            formDesenho.AtualizaShaders();
 //           AtualizaDesenho();
        }

        private void button46_Click(object sender, EventArgs e)
        {
            DialogResult result = DialogResult.Yes;
            {
                ConfiguraDeformacao = new FConfiguraDeformacao(this);

                result = ConfiguraDeformacao.ShowDialog(this);
            }
        }

        private void btDeformacaoSolida_Click_1(object sender, EventArgs e)
        {
            if (formDesenho.Estrutura.PorticoEspacial == null)
                return;

            if ((string)btDeformacaoSolida.Tag == "1")
            {
                btDeformacaoSolida.BackColor = System.Drawing.Color.FromArgb(64, 64, 64);
                btDeformacaoSolida.Tag = "0";
            }
            else
            {
                btDeformacaoSolida.BackColor = System.Drawing.Color.White;
                btDeformacaoSolida.Tag = "1";
            }

            bool defSolida = (string)btDeformacaoSolida.Tag == "1";

            formDesenho.DeformacaoSolida = defSolida;
          
            if ((string)btAnimarDeformacao.Tag == "1") 
                btAnimarDeformacao_Click_1(btAnimarDeformacao, null);

            if (defSolida)
            {
                TBarraPortico[] barrasP = formDesenho.Estrutura.PorticoEspacial.barras.ToArray();
                formDesenho.CriaDeformacaoSolida(barrasP);

                if (escalaDeformacao == 0)
                {
                    rbEscalaDeformacao.TextBoxText = "0.5";
                    formDesenho.EscalaDiagramas = 0.5;
                }
                else
                {
                    rbEscalaDeformacao.TextBoxText = escalaDeformacao.ToString("n2");

                    formDesenho.EscalaDiagramas = vEscalaDeformacao;
                }

                //forçar visualização de estrutura original unifilar
                if ((string)btMostraIndeformada.Tag == "0")
                {
                    if (!formDesenho.Unifilar)
                        AlternaVisualizacaoUnifilar(false);

                    btMostraIndeformada_Click_1(btMostraIndeformada, null);
                }
            }

            formDesenho.ArestasResultado = !formDesenho.ArestasResultado;

            ConfirmaEscalaDef();
        }

        private void btDeformacaoTextos_Click(object sender, EventArgs e)
        {
            if (formDesenho.Estrutura.PorticoEspacial == null)
                return;

            if ((string)btDeformacaoTextos.Tag == "1")
            {
                btDeformacaoTextos.BackColor = System.Drawing.Color.FromArgb(64, 64, 64);
                btDeformacaoTextos.Tag = "0";
            }
            else
            {
                btDeformacaoTextos.BackColor = System.Drawing.Color.White;
                btDeformacaoTextos.Tag = "1";
            }

            bool texto = (string)btDeformacaoTextos.Tag == "1";

       //     formDesenho.MostraTextoDeformacoes = texto;

            if (formDesenho.MostraTextoDeformacoes)
            {
//                ConfiguracoesPGi.PorticoOpcoesVisualizacao.percentual_maximo_deformacao = 100 - trackValorDeformacao.Value;
 //               Preenche_nos_valores_deformacao();
            }
            ChamarAtualizacaoResultados(btConfirmaDeslocamentos, true);

            // AtualizaDesenho();
        }

        private void btAnimarDeformacao_Click_1(object sender, EventArgs e)
        {
            if (formDesenho.MostraTensoesNormaisGradiente || (formDesenho.MostraDeformacoes))
            {
                if ((string)btAnimarDeformacao.Tag == "1")
                {
                    btAnimarDeformacao.BackColor = System.Drawing.Color.FromArgb(64, 64, 64);
                    btAnimarDeformacao.Tag = "0";
                }
                else
                {
                    btAnimarDeformacao.BackColor = System.Drawing.Color.White;
                    btAnimarDeformacao.Tag = "1";
                }

                bool animar = (string)btAnimarDeformacao.Tag == "1";

                formDesenho.AnimarDeformacoes(animar);
            }
        }


        private void ribbonButton1_Click_1(object sender, EventArgs e)
        {

        }

        private void ribbonButton2_Click_1(object sender, EventArgs e)
        {

        }

        private void ribbonButton5_Click_6(object sender, EventArgs e)
        {
            rbDefY.Checked = false;
            rbDefZ.Checked = false;
            rbDefX.Checked = false;

            rbDefU.Checked = !rbDefU.Checked;
            formDesenho.deformacao_U = 4;
            formDesenho.AtualizaShaders();
            AtualizaDesenho();
        }

        private void ribbonButton55_Click_3(object sender, EventArgs e)
        {
            rbDefY.Checked = false;
            rbDefZ.Checked = false;
            rbDefU.Checked = false;

            rbDefX.Checked = !rbDefX.Checked;
            formDesenho.deformacao_U = 1;
            formDesenho.AtualizaShaders();
            AtualizaDesenho();       
        }

        private void ribbonButton45_Click_2(object sender, EventArgs e)
        {
            rbDefX.Checked = false;
            rbDefZ.Checked = false;
            rbDefU.Checked = false;

            rbDefY.Checked = !rbDefY.Checked;
            formDesenho.deformacao_U = 2;
            formDesenho.AtualizaShaders();
            AtualizaDesenho();
        }

        private void ribbonButton26_Click_3(object sender, EventArgs e)
        {
            rbDefY.Checked = false;
            rbDefX.Checked = false;
            rbDefU.Checked = false;

            rbDefZ.Checked = !rbDefZ.Checked;
            formDesenho.deformacao_U = 3;
            formDesenho.AtualizaShaders();
            AtualizaDesenho();
        }

        private void edEscalaDeformacao_KeyPress(object sender, System.Windows.Forms.KeyPressEventArgs e)
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

        private void edEscalaDeformacao_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyData == Keys.Enter)
            {
                ConfirmaEscalaDef();
            }
        }

        private void edEscalaDeformacao_TextChanged(object sender, EventArgs e)
        {
           
        }
        int escalaDiagrama_anterior = 1;

        private void EscalaDiagrama_UpDown_ValueChanged(object sender, EventArgs e)
        {
            if ((int)EscalaDiagrama_UpDown.Value < escalaDiagrama_anterior)
            {
                if (edEscalaDiagrama.Text.ToString() == string.Empty)
                    return;

                if (edEscalaDiagrama.Text.ToString().Trim() != "")
                {
                    if (double.Parse(edEscalaDiagrama.Text.ToString()) > 1)
                    {
                        vEscalaDiagrama = double.Parse(edEscalaDiagrama.Text.ToString()) - 1;
                    }
                    else
                    {
                        vEscalaDiagrama = double.Parse(edEscalaDiagrama.Text.ToString()) - 0.25;

                        if (vEscalaDiagrama < 0)
                            vEscalaDiagrama = 0;
                    }

                    edEscalaDiagrama.Text = System.Convert.ToString(vEscalaDiagrama);
                    formDesenho.fatorDiagramas = vEscalaDiagrama * escalaDiagrama;
              //      formDesenho.AtualizaShaders();

                    if (formDesenho.MostraTextoDiagramas)
                    {
                        //if (formDesenho.fx || formDesenho.mx)
                        //    formDesenho.CriarTextosAxial_Torcor();
                      //  else
                  //          formDesenho.CriarTextosEsforco();
                    }

                   // AtualizaDesenho();
                   // }

                }
            }
            else
            {
                if (edEscalaDiagrama.Text.ToString() == string.Empty)
                    return;

                if (edEscalaDiagrama.Text.ToString().Trim() != "")
                {
                    vEscalaDiagrama = double.Parse(edEscalaDiagrama.Text.ToString()) + 1;

                    edEscalaDiagrama.Text = System.Convert.ToString(vEscalaDiagrama);

                    formDesenho.fatorDiagramas = vEscalaDiagrama * escalaDiagrama;

                 //   formDesenho.AtualizaShaders();
                    if (formDesenho.MostraTextoDiagramas)
                    {
                     //   if (formDesenho.fx || formDesenho.mx)
                     //       formDesenho.CriarTextosAxial_Torcor();
                     //   else
      //                      formDesenho.CriarTextosEsforco();
                    }
                 //   AtualizaDesenho();
                }
            }
            ChamarAtualizacaoResultados(btOkEsforco, true);
            escalaDiagrama_anterior = (int)EscalaDiagrama_UpDown.Value;
        }

        private void btfx_Click_1(object sender, EventArgs e)
        {
            if (formDesenho.Estrutura.PorticoEspacial == null)
                return;

            btfz.Tag = "0";
            btfy.Tag = "0";
            btmz.Tag = "0";
            btmy.Tag = "0";
            btmx.Tag = "0";

            btfz.BackColor = System.Drawing.Color.FromArgb(64, 64, 64);
            btfy.BackColor = System.Drawing.Color.FromArgb(64, 64, 64);
            btmz.BackColor = System.Drawing.Color.FromArgb(64, 64, 64);
            btmy.BackColor = System.Drawing.Color.FromArgb(64, 64, 64);
            btmx.BackColor = System.Drawing.Color.FromArgb(64, 64, 64);
          
            formDesenho.fz = false;
            formDesenho.fy = false;
            formDesenho.mz = false;
            formDesenho.mx = false;
            formDesenho.my = false;

            if ((string)btfx.Tag == "1")
            {
                btfx.BackColor = System.Drawing.Color.FromArgb(64, 64, 64);
                btfx.Tag = "0";
            }
            else
            {
                btfx.BackColor = System.Drawing.Color.White;
                btfx.Tag = "1";
            }

            bool fx = (string)btfx.Tag == "1";

            formDesenho.fx = fx;

        //    if ((string)btValoresDiagramas.Tag == "1")
       //         btValoresDiagramas_Click_1(btValoresDiagramas, null);

            pnCorResultados.Visible = false;

            if (fx)
            {
                CalcularEscalaDiagramas("fx");

                if ((string)btValoresDiagramas.Tag == "1")
                {
               //     btValoresDiagramas.Tag = "0";
              //      btValoresDiagramas_Click_1(btValoresDiagramas, null);
               //     ConfirmaEscalaDiagrama();
                }
                else
                {
             //       ChamarAtualizacaoResultados(btOkEsforco, true);
                }

            //    if (formDesenho.Diagrama_gradiente)
              //      pnCorResultados.Visible = true;
            }
            else
            {
                if ((string)btValoresDiagramas.Tag == "1")
                {
                    //  btValoresDiagramas.Tag = "0";
           //         btValoresDiagramas_Click_1(btValoresDiagramas, null);
                }
            }
            ChamarAtualizacaoResultados(btOkEsforco, true);
        }

        private void btValoresDiagramas_Click_1(object sender, EventArgs e)
        {
            if ((string)btValoresDiagramas.Tag == "1")
            {
                btValoresDiagramas.BackColor = System.Drawing.Color.FromArgb(64, 64, 64);
                btValoresDiagramas.Tag = "0";
            }
            else
            {
                btValoresDiagramas.BackColor = System.Drawing.Color.White;
                btValoresDiagramas.Tag = "1";
            }

            bool valores = (string)btValoresDiagramas.Tag == "1";

          //  if (valores) 
          //      Preenche_barras_valores_esforco();

            if (formDesenho.fx || formDesenho.my || formDesenho.mz || formDesenho.mx || formDesenho.fy || formDesenho.fz)
            {
                /* if (!formDesenho.fx && !formDesenho.mx) */
                //if (formDesenho.fx || formDesenho.mx) formDesenho.CriarTextosAxial_Torcor();
             //   formDesenho.CriarTextosEsforco();
            //    formDesenho.MostraTextoDiagramas = valores;
            }
            else
            {
                btValoresDiagramas.Tag = "0";
                btValoresDiagramas.BackColor = System.Drawing.Color.FromArgb(64, 64, 64);
                formDesenho.MostraTextoDiagramas = false;
            }

            ChamarAtualizacaoResultados(btOkEsforco, true);
        }

        private void btMostraIndeformada_Click(object sender, EventArgs e)
        {

        }

        private void button48_Click(object sender, EventArgs e)
        {
            DialogResult result = DialogResult.Yes;
            {
                fConfiguraDiagramas = new FConfiguraDiagramas(this);

                result = fConfiguraDiagramas.ShowDialog(this);
            }
        }

        private void btfy_Click_1(object sender, EventArgs e)
        {
            if (formDesenho.Estrutura.PorticoEspacial == null)
                return;

            btfz.Tag = "0";
            btfx.Tag = "0";
            btmz.Tag = "0";
            btmy.Tag = "0";
            btmx.Tag = "0";

            btfz.BackColor = System.Drawing.Color.FromArgb(64, 64, 64);
            btfx.BackColor = System.Drawing.Color.FromArgb(64, 64, 64);
            btmz.BackColor = System.Drawing.Color.FromArgb(64, 64, 64);
            btmy.BackColor = System.Drawing.Color.FromArgb(64, 64, 64);
            btmx.BackColor = System.Drawing.Color.FromArgb(64, 64, 64);

            formDesenho.fz = false;
            formDesenho.fx = false;
            formDesenho.mz = false;
            formDesenho.mx = false;
            formDesenho.my = false;

            if ((string)btfy.Tag == "1")
            {
                btfy.BackColor = System.Drawing.Color.FromArgb(64, 64, 64);
                btfy.Tag = "0";
            }
            else
            {
                btfy.BackColor = System.Drawing.Color.White;
                btfy.Tag = "1";
            }

            bool fy = (string)btfy.Tag == "1";

            formDesenho.fy = fy;


        //    if ((string)btValoresDiagramas.Tag == "1")
         //       btValoresDiagramas_Click_1(btValoresDiagramas, null);

            pnCorResultados.Visible = false;

            if (fy)
            {
                CalcularEscalaDiagramas("fy");

                if ((string)btValoresDiagramas.Tag == "1")
                {
            //        btValoresDiagramas.Tag = "0";
         //           btValoresDiagramas_Click_1(btValoresDiagramas, null);
            //        ConfirmaEscalaDiagrama();
                }
                else
                {
           //         ChamarAtualizacaoResultados(btOkEsforco, true);
                }

             //   if (formDesenho.Diagrama_gradiente)
             //       pnCorResultados.Visible = true;
            }
            else
            {
                if ((string)btValoresDiagramas.Tag == "1")
                {
                    //  btValoresDiagramas.Tag = "0";
   //                 btValoresDiagramas_Click_1(btValoresDiagramas, null);
                }
            }
            ChamarAtualizacaoResultados(btOkEsforco, true);
        }

        private void btfz_Click_1(object sender, EventArgs e)
        {
            if (formDesenho.Estrutura.PorticoEspacial == null)
                return;

            if ((string)btfz.Tag == "1")
            {
                btfz.BackColor = System.Drawing.Color.FromArgb(64, 64, 64);
                btfz.Tag = "0";
            }
            else
            {
                btfz.BackColor = System.Drawing.Color.White;
                btfz.Tag = "1";
            }

            btfy.Tag = "0";
            btfx.Tag = "0";
            btmz.Tag = "0";
            btmy.Tag = "0";
            btmx.Tag = "0";

            btfy.BackColor = System.Drawing.Color.FromArgb(64, 64, 64);
            btfx.BackColor = System.Drawing.Color.FromArgb(64, 64, 64);
            btmz.BackColor = System.Drawing.Color.FromArgb(64, 64, 64);
            btmy.BackColor = System.Drawing.Color.FromArgb(64, 64, 64);
            btmx.BackColor = System.Drawing.Color.FromArgb(64, 64, 64);

            formDesenho.fy = false;
            formDesenho.fx = false;
            formDesenho.mz = false;
            formDesenho.mx = false;
            formDesenho.my = false;

            bool fz = (string)btfz.Tag == "1";

            formDesenho.fz = fz;

            pnCorResultados.Visible = false;

            if (fz)
            {
                CalcularEscalaDiagramas("fz");

                if ((string)btValoresDiagramas.Tag == "1")
                {
              //      btValoresDiagramas.Tag = "0";
              //      btValoresDiagramas_Click_1(btValoresDiagramas, null);
               //     ConfirmaEscalaDiagrama();
                }
                else
                {
                    //formDesenho.AtualizaShaders();
                    // AtualizaDesenho();
              //      ChamarAtualizacaoResultados(btOkEsforco, true);
                }

            //    if (formDesenho.Diagrama_gradiente)
            //        pnCorResultados.Visible = true;
            }
            else
            {
                if ((string)btValoresDiagramas.Tag == "1")
                {
     //               btValoresDiagramas_Click_1(btValoresDiagramas, null);
                }
            }
            ChamarAtualizacaoResultados(btOkEsforco, true);
        }

        private void btmx_Click_1(object sender, EventArgs e)
        {
            if (formDesenho.Estrutura.PorticoEspacial == null)
                return;

            if ((string)btmx.Tag == "1")
            {
                btmx.BackColor = System.Drawing.Color.FromArgb(64, 64, 64);
                btmx.Tag = "0";
            }
            else
            {
                btmx.BackColor = System.Drawing.Color.White;
                btmx.Tag = "1";
            }

            btfz.Tag = "0"; 
            btfy.Tag = "0";
            btfx.Tag = "0";
            btmz.Tag = "0";
            btmy.Tag = "0";

            btfz.BackColor = System.Drawing.Color.FromArgb(64, 64, 64);
            btfy.BackColor = System.Drawing.Color.FromArgb(64, 64, 64);
            btfx.BackColor = System.Drawing.Color.FromArgb(64, 64, 64);
            btmz.BackColor = System.Drawing.Color.FromArgb(64, 64, 64);
            btmy.BackColor = System.Drawing.Color.FromArgb(64, 64, 64);

            formDesenho.fz = false;
            formDesenho.fx = false;
            formDesenho.fy = false;
            formDesenho.mz = false;
            formDesenho.my = false;

            bool mx = (string)btmx.Tag == "1";

            formDesenho.mx = mx;

       //     if ((string)btValoresDiagramas.Tag == "1")
     //           btValoresDiagramas_Click_1(btValoresDiagramas, null);

            pnCorResultados.Visible = false;

            if (mx)
            {
                CalcularEscalaDiagramas("mx");

                if ((string)btValoresDiagramas.Tag == "1")
                {
           //         btValoresDiagramas.Tag = "0";
           //         btValoresDiagramas_Click_1(btValoresDiagramas, null);
            //        ConfirmaEscalaDiagrama();
                }
                else
                {
            //        ChamarAtualizacaoResultados(btOkEsforco, true);
                }

        //        if (formDesenho.Diagrama_gradiente)
         //           pnCorResultados.Visible = true;
            }
            else
            {
                if ((string)btValoresDiagramas.Tag == "1")
                {
                    //  btValoresDiagramas.Tag = "0";
            //        btValoresDiagramas_Click_1(btValoresDiagramas, null);
                }
            }
            ChamarAtualizacaoResultados(btOkEsforco, true);

        }

        private void btmy_Click_1(object sender, EventArgs e)
        {
            if (formDesenho.Estrutura.PorticoEspacial == null)
                return;

            if ((string)btmy.Tag == "1")
            {
                btmy.BackColor = System.Drawing.Color.FromArgb(64, 64, 64);
                btmy.Tag = "0";
            }
            else
            {
                btmy.BackColor = System.Drawing.Color.White;
                btmy.Tag = "1";
            }

            btfz.Tag = "0";
            btfy.Tag = "0";
            btfx.Tag = "0";
            btmz.Tag = "0";
            btmx.Tag = "0";

            btfz.BackColor = System.Drawing.Color.FromArgb(64, 64, 64);
            btfy.BackColor = System.Drawing.Color.FromArgb(64, 64, 64);
            btfx.BackColor = System.Drawing.Color.FromArgb(64, 64, 64);
            btmz.BackColor = System.Drawing.Color.FromArgb(64, 64, 64);
            btmx.BackColor = System.Drawing.Color.FromArgb(64, 64, 64);

            formDesenho.fz = false;
            formDesenho.fx = false;
            formDesenho.fy = false;
            formDesenho.mz = false;
            formDesenho.mx = false;

            bool my = (string)btmy.Tag == "1";

            formDesenho.my = my;


            pnCorResultados.Visible = false;

            if (my)
            {
                CalcularEscalaDiagramas("my");

                if ((string)btValoresDiagramas.Tag == "1")
                {
                //    btValoresDiagramas.Tag = "0";
                //    btValoresDiagramas_Click_1(btValoresDiagramas, null);
                  //  ConfirmaEscalaDiagrama();
                }
                else
                {
                //    ChamarAtualizacaoResultados(btOkEsforco, true);
                }

            //    if (formDesenho.Diagrama_gradiente)
             //     pnCorResultados.Visible = true;

            }
            else
            {

                if ((string)btValoresDiagramas.Tag == "1")
                {
                    //  btValoresDiagramas.Tag = "0";
              //      btValoresDiagramas_Click_1(btValoresDiagramas, null);
                }
//                ChamarAtualizacaoResultados(btOkEsforco, true);
            }
            ChamarAtualizacaoResultados(btOkEsforco, true);
        }

        public void CalcularEscalaDiagramas(string esforco, bool sohSelecionados = false)
        {
            double max = formDesenho.Estrutura.PorticoEspacial.MaximoEsforco(esforco, cbTipoCargaResultado.SelectedIndex,
                                                                 cbResultadoCasos.SelectedIndex,
                                                                 cbResultadoCombinacoes.SelectedIndex,
                                                                 sohSelecionados);

            if (!Geom.Iguais(max, 0))
            {
                escalaDiagrama = 1 / max;

                if (escalaDiagrama == 0)
                {
                    escalaDiagrama = 0.5;
                    formDesenho.fatorDiagramas = 0.5;
                    edEscalaDiagrama.Text = escalaDiagrama.ToString();
                }
                else
                {
                    edEscalaDiagrama.Text = "1";// escalaDiagrama.ToString();
                    formDesenho.fatorDiagramas = 1 * escalaDiagrama;
                }
            }
        }

        private void btmz_Click_1(object sender, EventArgs e)
        {
            if (formDesenho.Estrutura.PorticoEspacial == null)
                return;

            if ((string)btmz.Tag == "1")
            {
                btmz.BackColor = System.Drawing.Color.FromArgb(64, 64, 64);
                btmz.Tag = "0";
            }
            else
            {
                btmz.BackColor = System.Drawing.Color.White;
                btmz.Tag = "1";
            }

            btfz.Tag = "0";
            btfy.Tag = "0";
            btfx.Tag = "0";
            btmx.Tag = "0";
            btmy.Tag = "0";

            btfz.BackColor = System.Drawing.Color.FromArgb(64, 64, 64);
            btfy.BackColor = System.Drawing.Color.FromArgb(64, 64, 64);
            btfx.BackColor = System.Drawing.Color.FromArgb(64, 64, 64);
            btmx.BackColor = System.Drawing.Color.FromArgb(64, 64, 64);
            btmy.BackColor = System.Drawing.Color.FromArgb(64, 64, 64);

            formDesenho.fz = false;
            formDesenho.fx = false;
            formDesenho.fy = false;
            formDesenho.mx = false;
            formDesenho.my = false;

            bool mz = (string)btmz.Tag == "1";

            formDesenho.mz = mz;

         //   if ((string)btValoresDiagramas.Tag == "1")
            //  btValoresDiagramas_Click_1(btValoresDiagramas, null);
           
            pnCorResultados.Visible = false;

            if (mz)
            {
                CalcularEscalaDiagramas("mz");

                if ((string)btValoresDiagramas.Tag == "1")
                {
               //     btValoresDiagramas.Tag = "0";
              //      btValoresDiagramas_Click_1(btValoresDiagramas, null);
           //         ConfirmaEscalaDiagrama();
                }
                else
                {
           //         ChamarAtualizacaoResultados(btOkEsforco, true);
                }

             //   if (formDesenho.Diagrama_gradiente)
              //      pnCorResultados.Visible = true;
            }
            else
            {
                if ((string)btValoresDiagramas.Tag == "1")
                {
                    //  btValoresDiagramas.Tag = "0";
        //            btValoresDiagramas_Click_1(btValoresDiagramas, null);
                }
            }
            ChamarAtualizacaoResultados(btOkEsforco, true);

        }

        private void btDivBarras_Click(object sender, EventArgs e)
        {

        }
        public FEdicaoNos fRotacionarElementos;
        private void button36_Click(object sender, EventArgs e)
        {
            /* if (fRotacionarElementos == null)
                 fRotacionarElementos = new FRotacionarElementos(this);

             fRotacionarElementos.Show();*/
            formDesenho.RemeverDosObjetosSelecionados_Nao_Copiaveis();

            if (formDesenho.ObjetosSelecionados.Count > 0)
            {
                formDesenho.ComandoEdicao(Const.ID_ROTACIONAR_ELEMENTOS, eTipoComando.edit);
                formDesenho.MouseEdit(0, 0, 0, 0, "", 0);
            }
            else
                formDesenho.ComandoEdicao(Const.ID_ROTACIONAR_ELEMENTOS, eTipoComando.selecionar);

            if (!formDesenho.pnRotacionar.Visible)
                formDesenho.pnRotacionar.Visible = true;

          /*  if (fRotacionarElementos == null)
            {
                fRotacionarElementos = new FRotacaoElementos(this);
                fRotacionarElementos.Show();
            }*/

            AtualizaDesenho();
        }

        private void button23_Click(object sender, EventArgs e)
        {
           
        }

        private void button9_Click(object sender, EventArgs e)
        {
            NovoProjeto(true);
            pnArquivo.Visible = false;
        }

        private void button8_Click(object sender, EventArgs e)
        {
            Abrir();
            pnArquivo.Visible = false;
        }

        private void label1_Click(object sender, EventArgs e)
        {

        }

        private void lbRecentes_DoubleClick(object sender, EventArgs e)
        {
            pnArquivo.Visible = false;
            Application.DoEvents();
            formDesenho.glControl.SwapBuffers();
            Abrir((string)lbRecentes.SelectedItem);
        }
        public FEspelharElementos fEspelharelementos;
        private void button31_Click(object sender, EventArgs e)
        {
            formDesenho.RemeverDosObjetosSelecionados_Nao_Copiaveis();
            formDesenho.ObjetosSelecionados.RemoveAll(o => o.Tipo != Const.ID_BARRAGENERICA);

            if (formDesenho.ObjetosSelecionados.Count > 0)
            {
                formDesenho.ComandoEdicao(Const.ID_ESPELHAR_ELEMENTOS, eTipoComando.edit);
                formDesenho.MouseEdit(0, 0, 0, 0, "", 0);
            }
            else
                formDesenho.ComandoEdicao(Const.ID_ESPELHAR_ELEMENTOS, eTipoComando.selecionar);
           
          /*  if (frot == null)
            {
                frot = new FRotacionarElementos(this);
                frot.Show();
            }*/


          /*  if (fEspelharelementos == null)
            {
                fEspelharelementos = new FEspelharElementos(this);
                fEspelharelementos.Show();
            }
            */
            AtualizaDesenho();
        }
       public  FRotacionarElementos frot;

        private void button41_Click(object sender, EventArgs e)
        {
            Salvar();
        }

        private void button38_Click_1(object sender, EventArgs e)
        {
            Salvar(true);
        }

        private void panelGeral_Paint(object sender, PaintEventArgs e)
        {

        }

        private void textBox1_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyData == Keys.Enter)
            {
                ConfirmaEscalaDiagrama();
            }
        }

        private void panel49_Paint(object sender, PaintEventArgs e)
        {
        }
        double pnCargasLeft, pnCargasTop;

        void ReposicionaPanel(Panel pn, double left, double top)
        {
            pn.Left = (int)(left * this.ClientSize.Width);

            pn.Top = (int)(top * this.ClientSize.Height);

            // Garante que não ultrapasse a borda direita
            if (pn.Right > this.ClientSize.Width)
                pn.Left = this.ClientSize.Width - pn.Width;

            // Garante que não ultrapasse a borda inferior
            if (pn.Bottom > this.ClientSize.Height)
                pn.Top = this.ClientSize.Height - pn.Height;

            // Garante esquerda e topo
            if (pn.Left < 0)
                pn.Left = 0;

            if (pn.Top < 0)
                pn.Top = 0;
        }
        private void Gerenciador_Resize(object sender, EventArgs e)
        {
            if (this.ClientSize.Width <= 0 ||
                this.ClientSize.Height <= 0)
                return;

            ReposicionaPanel(pnUtilitarios, pnUtilitariosLeftRel, pnUtilitariosTopRel);
            ReposicionaPanel(pnCargasCombinacoes, pnCargasLeftRel, pnCargasTopRel);


            /*     pnUtilitarios.Left =
                     (int)(pnUtilitariosLeftRel * this.ClientSize.Width);

                 pnUtilitarios.Top =
                     (int)(pnUtilitariosTopRel * this.ClientSize.Height);

                 // Garante que não ultrapasse a borda direita
                 if (pnUtilitarios.Right > this.ClientSize.Width)
                     pnUtilitarios.Left =
                         this.ClientSize.Width - pnUtilitarios.Width;

                 // Garante que não ultrapasse a borda inferior
                 if (pnUtilitarios.Bottom > this.ClientSize.Height)
                     pnUtilitarios.Top =
                         this.ClientSize.Height - pnUtilitarios.Height;

                 // Garante esquerda e topo
                 if (pnUtilitarios.Left < 0)
                     pnUtilitarios.Left = 0;

                 if (pnUtilitarios.Top < 0)
                     pnUtilitarios.Top = 0;*/
        }

        private void Gerenciador_MouseDown(object sender, System.Windows.Forms.MouseEventArgs e)
        {
      
        }

        private void Gerenciador_MouseMove(object sender, System.Windows.Forms.MouseEventArgs e)
        {
            
        }
        bool clicouPanel;
        double xant = 0, yant;

        private void panel50_MouseDown(object sender, System.Windows.Forms.MouseEventArgs e)
        {
            clicouPanel = true;
            xant = e.X;
            yant = e.Y;
        }

        private void panel50_MouseUp(object sender, System.Windows.Forms.MouseEventArgs e)
        {
            clicouPanel = false;
        }

        private void Ribbon_MouseDown(object sender, System.Windows.Forms.MouseEventArgs e)
        {
          
        }

        private void Ribbon_Click(object sender, EventArgs e)
        {
        
        }

        private void panel51_MouseDown(object sender, System.Windows.Forms.MouseEventArgs e)
        {
           /* clicouPanel = true;
            xant = e.X;
            yant = e.Y;
            int he = this.Height;
            int wi = this.Width;
            this.WindowState = System.Windows.Forms.FormWindowState.Normal;
            this.Height = he;
            this.Width = wi;*/
        }

        private void panel51_MouseUp(object sender, System.Windows.Forms.MouseEventArgs e)
        {
            clicouPanel = false;
        }

        private void panel51_MouseMove(object sender, System.Windows.Forms.MouseEventArgs e)
        {
            /*if (clicouPanel)
            {
                this.Left += (int)(e.X - xant);
                this.Top += (int)(e.Y - yant);
                //ger.xant = e.X;
                AtualizaDesenho();

            }*/
        }

        private void button12_Click_1(object sender, EventArgs e)
        {
        }

        private void button12_MouseClick(object sender, System.Windows.Forms.MouseEventArgs e)
        {
            menuSelecao.Show(button12, new Point(e.X, e.Y));
        }
        double pnUtilitariosTopRel = 0;
        double pnUtilitariosLeftRel = 0;
        double pnCargasTopRel = 0;
        double pnCargasLeftRel = 0;
        private void pnUtilitarios_MouseMove(object sender, System.Windows.Forms.MouseEventArgs e)
        {
            if ((clicouPanel) && ((pnUtilitarios.Top + ((int)(e.Y - yant))) < 0))
                maxtop2 = true;
            else
                maxtop2 = false;

            if ((clicouPanel) && ((pnUtilitarios.Top + (pnUtilitarios.Height * 2 + 15) + ((int)(e.Y - yant))) > this.Height))
                maxtop = true;
            else
                maxtop = false;

            if ((clicouPanel) && ((pnUtilitarios.Left + ((int)(e.X - xant))) < 0))
                maxleft = true;
            else
                maxleft = false;

            if ((clicouPanel) && (((pnUtilitarios.Left + pnUtilitarios.Width + 15) + ((int)(e.X - xant))) > this.Width))
                maxleft2 = true;
            else
                maxleft2 = false;

            if (clicouPanel && !maxtop && !maxtop2 && !maxleft && !maxleft2)
            {
                pnUtilitarios.Left += (int)(e.X - xant);
                pnUtilitarios.Top += (int)(e.Y - yant);
                pnCargasLeft = (pnUtilitarios.Left * 100) / this.Width;
                pnCargasTop = (pnUtilitarios.Top * 100) / this.Height;

                // Posição relativa ao Form
                pnUtilitariosLeftRel = (double)pnUtilitarios.Left / this.ClientSize.Width;
                pnUtilitariosTopRel = (double)pnUtilitarios.Top / this.ClientSize.Height;

                AtualizaDesenho();
            }
        }

        private void trackValorDiagramas_Scroll(object sender, EventArgs e)
        {
            if (trackValorDiagramas.Value == 0)
                lbValoresDiag.Text = "Valor máximo";
            else
                lbValoresDiag.Text = "Valores (" + trackValorDiagramas.Value + "%)";

            ConfiguracoesPGi.DiagramaOpcoesVisualizacao.percentual_maximo = 100 - trackValorDiagramas.Value;

     //       Preenche_barras_valores_esforco();

        //   if (formDesenho.MostraTextoDiagramas)
         //      formDesenho.CriarTextosEsforco();

            ChamarAtualizacaoResultados(btOkEsforco,true);
         //   formDesenho.AtualizarDesenho();
         //   formDesenho.glControl.SwapBuffers();
        }

        private void trackValorDeformacao_Scroll(object sender, EventArgs e)
        {
            if (trackValorDeformacao.Value == 0)
                lbValoresDef.Text = "Valor máximo";
            else
                lbValoresDef.Text = "Valores (" + trackValorDeformacao.Value + "%)";

            ConfiguracoesPGi.PorticoOpcoesVisualizacao.percentual_maximo_deformacao = 100 - trackValorDeformacao.Value;
          //  Preenche_nos_valores_deformacao();

            ChamarAtualizacaoResultados(btConfirmaDeslocamentos, true);
           // formDesenho.AtualizarDesenho();
            //formDesenho.glControl.SwapBuffers();
        }

        private void panel52_Paint(object sender, PaintEventArgs e)
        {

        }

        private void lbVal7_Click(object sender, EventArgs e)
        {

        }

        private void button42_Click(object sender, EventArgs e)
        {

        }
        void MudaStatusBotao(Button botao)
        {
            if (botao == btMostrarNos)
            {
                if (!ConfiguracoesPGi.F3DOpcoesVisualizacao.Nos)
                {
                    botao.FlatAppearance.BorderColor = System.Drawing.Color.FromArgb(64, 64, 64);
                    botao.Tag = "1";
                }
                else
                {
                    botao.FlatAppearance.BorderColor = System.Drawing.Color.Cyan;
                    botao.Tag = "0";
                }
            }
            else
            if (botao == btMostrarEixosLocais)
            {
                if (!ConfiguracoesPGi.F3DOpcoesVisualizacao.MostrarEixosLocais)
                {
                    botao.FlatAppearance.BorderColor = System.Drawing.Color.FromArgb(64, 64, 64);
                    botao.Tag = "1";
                }
                else
                {
                    botao.FlatAppearance.BorderColor = System.Drawing.Color.Cyan;
                    botao.Tag = "0";
                }
            }
            else
            if (botao == btVisualizarCargas)
            {
                if (!formDesenho.MostrarCargas)
                {
                    botao.FlatAppearance.BorderColor = System.Drawing.Color.FromArgb(64, 64, 64);
                    botao.Tag = "1";
                }
                else
                {
                    botao.FlatAppearance.BorderColor = System.Drawing.Color.Cyan;
                    botao.Tag = "0";
                }
            }
        }

        public void HabilitaBotaoNos()
        {
            ConfiguracoesPGi.F3DOpcoesVisualizacao.Nos = !ConfiguracoesPGi.F3DOpcoesVisualizacao.Nos;
            formDesenho.MostrarNos = !formDesenho.MostrarNos;

            formDesenho.Estrutura.nos.ForEach(o => o.Selecionado = false);
            
            if (!formDesenho.MostrarNos)
            {
                formDesenho.Estrutura.nos.ForEach(o => o.Visivel = false);
            }

            if ((string)btMostrarNos.Tag == "0")
            {
                btMostrarNos.FlatAppearance.BorderColor = System.Drawing.Color.FromArgb(64, 64, 64);
                btMostrarNos.Tag = "1";
            }
            else
            {
                btMostrarNos.FlatAppearance.BorderColor = System.Drawing.Color.Cyan;
                btMostrarNos.Tag = "0";
            }

            formDesenho.Alterou(true, false);
            formDesenho.AtualizaConfiguracoes3D();
        }

        private void button47_Click(object sender, EventArgs e)
        {
            HabilitaBotaoNos();
        }

        private void btMostrarEixosLocais_Click(object sender, EventArgs e)
        {
            ConfiguracoesPGi.F3DOpcoesVisualizacao.MostrarEixosLocais = !ConfiguracoesPGi.F3DOpcoesVisualizacao.MostrarEixosLocais;
            formDesenho.EixosLocais = !formDesenho.EixosLocais;

            if ((string)btMostrarEixosLocais.Tag == "0")
            {
                btMostrarEixosLocais.FlatAppearance.BorderColor = System.Drawing.Color.FromArgb(64, 64, 64);
                btMostrarEixosLocais.Tag = "1";
            }
            else
            {
                btMostrarEixosLocais.FlatAppearance.BorderColor = System.Drawing.Color.Cyan;
                btMostrarEixosLocais.Tag = "0";
            }

            formDesenho.Alterou(true, false);
            formDesenho.AtualizaConfiguracoes3D();
        }
        public void AlternaComboBoxResultados()
        {
            if (Ribbon.SelectedTab == tabResultados)
            {
                if (cbTipoCargaResultado.SelectedIndex == 0)
                {
                    pnCasos.Visible = true;
                    pnCasos.Left = 110;

                    pnCombinacoes.Visible = false;
                    pnCasos.Visible = true;

                    if (cbResultadoCasos.Items.Count > 0)
                    {
                        AlternaResultadosCaso(false);
                        cbResultadoCasos.Focus();
                    }
                }
                else
               if (cbTipoCargaResultado.SelectedIndex == 1)
                {
                    pnCombinacoes.Visible = true;
                    pnCombinacoes.Left = 110;

                    pnCasos.Visible = false;

                    if (cbResultadoCombinacoes.Items.Count > 0)
                    {
                        AlternaResultadosCombinacao(false);
                        cbResultadoCombinacoes.Focus();
                    }
                }

            }
            formDesenho.tipoCargaResultado = cbTipoCargaResultado.SelectedIndex;
            /*    if ((formDesenho.tipoCargaResultado == 1 && cbResultadoCombinacoes.Items.Count > 0)
                    ||
                    (formDesenho.tipoCargaResultado == 0 && cbResultadoCasos.Items.Count > 0))
                {
                    formDesenho.AtualizaShaders();
                    AtualizaDesenho();
                    ConfirmaEscalaDiagrama();
                }*/
        }

        private void cbTipoCargaResultado_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (cbTipoCargaResultado.SelectedIndex == 1)
            {
                if (desenho.Estrutura.combinacoes.Count == 0)
                {
                    MessageBox.Show("Ainda não foi criada nenhuma combinação de carga.", "Atenção", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    cbTipoCargaResultado.SelectedIndex = 0;
                    return;
                }
            }

            AlternaComboBoxResultados();
        }

        void AlternaResultadosCombinacao(bool atu = true)
        {
            if (formDesenho.Estrutura.PorticoEspacial != null && cbResultadoCombinacoes.Items.Count > 0 && Ribbon.SelectedTab == tabResultados)
            {
                string esforco = "my";
                if (formDesenho.fx) esforco = "fx";
                else
                if (formDesenho.fy) esforco = "fy";
                else
                if (formDesenho.fz) esforco = "fz";
                else
                if (formDesenho.mx) esforco = "mx";
                else
                if (formDesenho.my) esforco = "my";
                else
                if (formDesenho.mz) esforco = "mz";

                formDesenho.id_combinacao = cbResultadoCombinacoes.SelectedIndex;
             
                if ((string)btTensoes.Tag == "1")
                    ChamarAtualizacaoResultados(btOkTensoes, true);

                if ((string)btDeformacao.Tag == "1")
                    ChamarAtualizacaoResultados(btConfirmaDeslocamentos, true);

                if ((string)btDiagrama.Tag == "1")
                    ChamarAtualizacaoResultados(btOkEsforco, true);
               
                CalcularEscalaDeformacoes();
                CalcularEscalaDiagramas(esforco);

                formDesenho.DirtyPortico();

                // cbCasoCarga.SelectedIndex = cbResultadoCasos.SelectedIndex + 1;

                // cbCargas_SelectedIndexChanged(cbCasoCarga, null);

                if ((string)btAnimarDeformacao.Tag == "1")
                    btAnimarDeformacao_Click_1(btAnimarDeformacao, null);

                if ((string)btDeformacaoTextos.Tag == "1")
                {
                    btDeformacaoTextos.Tag = "0";
                    btDeformacaoTextos_Click(btDeformacaoTextos, null);
                }

                if ((string)btValoresDiagramas.Tag == "1")
                {
                    btValoresDiagramas.Tag = "0";
                    btValoresDiagramas_Click_1(btValoresDiagramas, null);
                }

               /* if ((string)btTensoes.Tag == "1" && cbResultadoTensoes.SelectedIndex == 1)
                {
                    CriaTensaoNormal();
                }*/

                if (atu)
                {
             //       formDesenho.AtualizaShaders();
           //         AtualizaDesenho();
                }
                cbResultadoCasos.Focus();
                index_combinacao_flambagem = -1;
                if (ConfiguracoesPGi.CfgProjeto.sistema.CalculaModosFlambagem && (string)btFlambagem.Tag == "1")
                {
                    cbModosFlambagem.Items.Clear();
                    for (int j = 0; j < formDesenho.Estrutura.PorticoEspacial.ResultadosFlambagem.Count; j++)
                    {
                        if (cbResultadoCombinacoes.SelectedIndex >= 0 &&
                            cbResultadoCombinacoes.SelectedIndex < formDesenho.Estrutura.combinacoes.Count &&
                            formDesenho.Estrutura.PorticoEspacial.ResultadosFlambagem[j].EhCombinacao &&
                            formDesenho.Estrutura.PorticoEspacial.ResultadosFlambagem[j].IdReferencia ==
                                formDesenho.Estrutura.combinacoes[cbResultadoCombinacoes.SelectedIndex].Id)
                        {
                            index_combinacao_flambagem = j;
                            for (int i = 1; i <= ConfiguracoesPGi.CfgProjeto.sistema.numeroModosFlambagem; i++)
                            {
                                cbModosFlambagem.Items.Add(i + " - Fator: " + formDesenho.Estrutura.PorticoEspacial.ResultadosFlambagem[j].Multiplicadores[i - 1].ToString("n3"));
                            }
                        }
                    }

                    if (cbModosFlambagem.Items.Count > 0)
                    {
                        id_combinacao_flambagem = formDesenho.Estrutura.PorticoEspacial.ResultadosFlambagem[index_combinacao_flambagem].IdReferencia;
                        cbModosFlambagem.SelectedIndex = 0;
                        CalcularEscalaModosFlambagem(id_combinacao_flambagem);
                    }
                }
            }

        }
        public int index_combinacao_flambagem = -1, id_combinacao_flambagem = -1;
        private void cbResultadoCombinacoes_SelectedIndexChanged(object sender, EventArgs e)
        {
            AlternaResultadosCombinacao(false);
        }

        void AlternaResultadosCaso(bool atu = true)
        {
            if (formDesenho.Estrutura.PorticoEspacial != null)
            {
                cbCasoCarga.SelectedIndex = cbResultadoCasos.SelectedIndex + 1;

                string esforco = "my";
                if (formDesenho.fx) esforco = "fx";
                else
                if (formDesenho.fy) esforco = "fy";
                else
                if (formDesenho.fz) esforco = "fz";
                else
                if (formDesenho.mx) esforco = "mx";
                else
                if (formDesenho.my) esforco = "my";
                else
                if (formDesenho.mz) esforco = "mz";

                if ((string)btTensoes.Tag == "1")
                    ChamarAtualizacaoResultados(btOkTensoes, true);

                if ((string)btDeformacao.Tag == "1")
                    ChamarAtualizacaoResultados(btConfirmaDeslocamentos,true);

                if ((string)btDiagrama.Tag == "1")
                    ChamarAtualizacaoResultados(btOkEsforco, true);

                formDesenho.id_caso = cbResultadoCasos.SelectedIndex;

                CalcularEscalaDeformacoes();
                CalcularEscalaDiagramas(esforco);

                //cbCargas_SelectedIndexChanged(cbCasoCarga, null);

                formDesenho.DirtyPortico();

                if ((string)btAnimarDeformacao.Tag == "1") 
                    btAnimarDeformacao_Click_1(btAnimarDeformacao, null);

            //    formDesenho.CriaDeformacaoSolida(formDesenho.Estrutura.PorticoEspacial.barras);

                if ((string)btDeformacaoTextos.Tag == "1")
                {
                    btDeformacaoTextos.Tag = "0";
                    btDeformacaoTextos_Click(btDeformacaoTextos, null);
                }

                if ((string)btValoresDiagramas.Tag == "1")
                {
                    btValoresDiagramas.Tag = "0";
                    btValoresDiagramas_Click_1(btValoresDiagramas, null);
                }

              /*  if ((string)btTensoes.Tag == "1" && cbResultadoTensoes.SelectedIndex == 1)
                {
                    CriaTensaoNormal();
                }*/

                if (atu)
                {
               //     formDesenho.AtualizaShaders();
             //       AtualizaDesenho();
                }

                cbResultadoCasos.Focus();
            }
        }
        private void cbResultadoCasos_SelectedIndexChanged(object sender, EventArgs e)
        {
            AlternaResultadosCaso(false);
        }

        private void button27_Click(object sender, EventArgs e)
        {

        }

        private void label11_Click(object sender, EventArgs e)
        {

        }

        private void btMostraIndeformada_Click_1(object sender, EventArgs e)
        {
            if ((string)btMostraIndeformada.Tag == "1")
            {
                btMostraIndeformada.BackColor = System.Drawing.Color.FromArgb(64, 64, 64);
                btMostraIndeformada.Tag = "0";
            }
            else
            {
                btMostraIndeformada.BackColor = System.Drawing.Color.White;
                btMostraIndeformada.Tag = "1";
            }

            bool mostrar = (string)btMostraIndeformada.Tag == "1";
            formDesenho.MostrarIndeformada = mostrar;

            foreach (TBarraGenerica b in formDesenho.Estrutura.barras)
            {
                b.Visivel = mostrar;
              //  if (mostrar)
                {
                    b.DirtyArestas = true;
                    b.DirtyTriangulos = true;
                    b.DirtySelecao = true;
                }
            }
            //    formDesenho.DirtyPortico();
            if ((string)btTensoes.Tag == "1")
                ChamarAtualizacaoResultados(btOkTensoes, true);

            if ((string)btDeformacao.Tag == "1")
                ChamarAtualizacaoResultados(btConfirmaDeslocamentos, true);

        }

        private void button23_Click_1(object sender, EventArgs e)
        {
            CancelaResultados();
            formDesenho.MostrarTudo();
        }

        private void cbResultadoTensoes_SelectedIndexChanged(object sender, EventArgs e)
        {
           /* if (cbResultadoTensoes.SelectedIndex > 0)
            {
                if (cbResultadoTensoes.SelectedIndex == 1)
                    CriaTensaoNormal();
            }
            else
            if (cbResultadoTensoes.SelectedIndex == 0)
            {
                CancelaTensaoAtual();

                formDesenho.AtualizaShaders();
                AtualizaDesenho();
            }*/
        }

        private void btReacao_Click(object sender, EventArgs e)
        {

        }
        void CriaTensaoNormal()
        {
            try
            {
                formDesenho.CriaDeformacaoSolida_Tensoes(cbFiltroTensao.SelectedIndex);
              
                formDesenho.MostraTensoesNormaisGradiente = (string)btTensaoNormal.Tag == "1";
                formDesenho.TipoTensao = 1;

                pnCorResultados.Visible = true;

                formDesenho.MostraTensoesNormaisIsobandas = isotensoes;

                formDesenho.GeraGradienteTensaoNormal(cbFiltroTensao.SelectedIndex);

                if (!formDesenho.Unifilar)
                    AlternaVisualizacaoUnifilar();

                formDesenho.AtualizaShaders();
                AtualizaDesenho();
            }
            catch(Exception ee)
            {

            }
            FechaAguardar();
        }

        void CancelaTensaoAtual()
        {
            CancelaDiagramas();

            formDesenho.MostraTensoesNormaisGradiente = false;
            foreach (TBarraGenerica b in formDesenho.Estrutura.barras)
            {
                b.Visivel = true;
                b.DirtyArestas = true;
                b.DirtyTriangulos = true;
                b.DirtySelecao = true;
            }

            formDesenho.MostraTensoesNormaisGradiente = false;
            formDesenho.TipoTensao = -1;

            pnCorResultados.Visible = false;

        }

        private void btTensoes_Click(object sender, EventArgs e)
        {
            if ((string)btTensoes.Tag == "1")
            {
                btTensoes.BackColor = System.Drawing.Color.FromArgb(64, 64, 64);
                btTensoes.Tag = "0";
            }
            else
            {
                if (necessitaCalculo)
                {
                    MessageBox.Show("Modificações foram feitas. É necessário calcular a estrutura.", "Atenção", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                    return;
                }

                btTensoes.BackColor = System.Drawing.Color.White;
                btTensoes.Tag = "1";
            }
            cbFiltroTensao.SelectedIndex = 0;
            
            ChamarAtualizacaoResultados(btOkTensoes, false);

            if ((string)btIsobandaTensao.Tag == "1")
                btIsobandaTensao_Click(btIsobandaTensao, null);

            bool tensoes = (string)btTensoes.Tag == "1";

            if (tensoes)
              if ((string)btMostraIndeformada.Tag == "1")
                btMostraIndeformada_Click_1(btMostraIndeformada, null);

            panelTensoes.Visible = tensoes;
            if (!tensoes)
            {
                formDesenho.Text = "Modelo 3D";

                CancelaTensaoAtual();

                if ((string)btTensaoNormal.Tag == "1")
                    btTensaoNormal_Click(btTensaoNormal, null);

                foreach (TBarraGenerica b in formDesenho.Estrutura.barras)
                {
                    b.Visivel = true;
                    b.DirtyArestas = true;
                    b.DirtyTriangulos = true;
                    b.DirtySelecao = true;
                }

                pnCorResultados.Visible = false;
            }
            else
            {
                //cancela os outros resultados
                if ((string)btDeformacao.Tag == "1")
                    btDeformacao_Click(btDeformacao, null);
                if ((string)btDinamica1.Tag == "1")
                    btDinamica1_Click(btDinamica1, null);
                if ((string)btFlambagem.Tag == "1")
                    btFlambagem_Click(btFlambagem, null);

                formDesenho.Text = "Resultados: " + btTensoes.AccessibleName;

                if ((string)btAnimarDeformacao.Tag == "1")
                {
                    btAnimarDeformacao_Click_1(btAnimarDeformacao, null);
                }

                if ((string)btTensaoNormal.Tag == "1")
                    btTensaoNormal_Click(btTensaoNormal, null);
            }

            formDesenho.AtualizaShaders();
            AtualizaDesenho();
        }

        private void edEscalaDiagrama_TextChanged(object sender, EventArgs e)
        {

        }

        private void EscalaTensao_UpDown_ValueChanged(object sender, EventArgs e)
        {
            if ((int)EscalaTensao_UpDown.Value < escalaDef_anterior)
            {
                if (edEscalaTensao.Text.ToString() == string.Empty)
                    return;

                if (escalaDeformacao == 0)
                    vEscalaDeformacao = double.Parse(edEscalaTensao.Text.ToString()) - 1;
                else
                    vEscalaDeformacao = double.Parse(edEscalaTensao.Text.ToString()) - escalaDeformacao;

                if (vEscalaDeformacao - 1 < 0)
                    vEscalaDeformacao = 0;
                else
                    vEscalaDeformacao -= 1;

                edEscalaTensao.Text = System.Convert.ToString(vEscalaDeformacao);
                formDesenho.fatorDeformacao = vEscalaDeformacao;
            //    formDesenho.AtualizaShaders();
            //    AtualizaDesenho();
            }
            else
            {
                if (edEscalaTensao.Text.ToString() == string.Empty)
                    return;

                if (escalaDeformacao == 0)
                    vEscalaDeformacao = double.Parse(edEscalaTensao.Text.ToString()) + 1;
                else
                    vEscalaDeformacao = double.Parse(edEscalaTensao.Text.ToString()) + escalaDeformacao;

                edEscalaTensao.Text = System.Convert.ToString(vEscalaDeformacao);

                formDesenho.fatorDeformacao = vEscalaDeformacao;

                if ((string)btAnimarDeformacao.Tag == "1") btAnimarDeformacao_Click_1(btAnimarDeformacao, null);

                formDesenho.AtualizaShaders();

                //   if (btDeformacaoSolida.Checked)
                //       formDesenho.CriaDeformacaoSolida(formDesenho.Estrutura.PorticoEspacial.barras);

            //    AtualizaDesenho();
            }

            ChamarAtualizacaoResultados(btOkTensoes, true);


            escalaDef_anterior = (int)EscalaTensao_UpDown.Value;
        }

        private void edEscalaTensao_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyData == Keys.Enter)
            {
                ConfirmaEscalaTensao();
            }
        }
        void ConfirmaEscalaTensao()
        {
            if (edEscalaTensao.Text.ToString() == string.Empty)
                return;
            formDesenho.DirtyPortico();

            vEscalaDeformacao = double.Parse(edEscalaTensao.Text.ToString());
            formDesenho.fatorDeformacao = vEscalaDeformacao;

            ChamarAtualizacaoResultados(btOkTensoes, true);

            //formDesenho.AtualizaShaders();
            //  AtualizaDesenho();
        }

        private void edEscalaTensao_TextChanged(object sender, EventArgs e)
        {

        }
        bool isotensoes;
        private void btIsobandaTensao_Click(object sender, EventArgs e)
        {
            ChamarAtualizacaoResultados(btOkTensoes, true); 
            
            if ((string)btIsobandaTensao.Tag == "1")
            {
                btIsobandaTensao.BackColor = System.Drawing.Color.FromArgb(64, 64, 64);
                btIsobandaTensao.Tag = "0";
            }
            else
            {
                btIsobandaTensao.BackColor = System.Drawing.Color.White;
                btIsobandaTensao.Tag = "1";
            }

            isotensoes = (string)btIsobandaTensao.Tag == "1";
        }

        private void button39_Click_1(object sender, EventArgs e)
        {

        }

        private void btTensaoNormal_Click(object sender, EventArgs e)
        {
            ChamarAtualizacaoResultados(btOkTensoes, true);

            if ((string)btTensaoNormal.Tag == "1")
            {
                btTensaoNormal.BackColor = System.Drawing.Color.FromArgb(64, 64, 64);
                btTensaoNormal.Tag = "0";
            }
            else
            {
                btTensaoNormal.BackColor = System.Drawing.Color.White;
                btTensaoNormal.Tag = "1";
            }

            //forçar visualização de estrutura original unifilar
            if (!formDesenho.MostrarIndeformada)
            {
                if (!formDesenho.Unifilar)
                    AlternaVisualizacaoUnifilar(false);
            }
                btMostraIndeformada_Click_1(btMostraIndeformada, null);
          //  }
        }

        private void button39_Click_2(object sender, EventArgs e)
        {

        }

        private void button44_Click(object sender, EventArgs e)
        {
            if (!formDesenho.Estrutura.barras.Exists(o => o.Selecionado) && cbFiltroTensao.SelectedIndex == 1)
            {
                MessageBox.Show("Nenhum elemento foi selecionado!");
                return;
            }
            
            ConfirmaEscalaTensao(); 
            ChamaAguardar(this, "Processando. Aguarde...");
            if ((string)btTensaoNormal.Tag == "1")
            {
                CriaTensaoNormal();
            }
            else
            {
                CancelaTensaoAtual();

                formDesenho.AtualizaShaders();
                AtualizaDesenho();
            }

            ChamarAtualizacaoResultados(btOkTensoes, false);
            FechaAguardar();
        }

        private void button43_Click(object sender, EventArgs e)
        {
            if (!formDesenho.Estrutura.barras.Exists(o => o.Selecionado) && cbFiltroEsforcos.SelectedIndex == 1)
            {
                MessageBox.Show("Nenhum elemento foi selecionado!");
                return;
            }
            
            ChamaAguardar(this, "Processando. Aguarde...");

            if (!formDesenho.fx && !formDesenho.fy && !formDesenho.fz && !formDesenho.mz && !formDesenho.my && !formDesenho.mx)
                btValoresDiagramas_Click_1(btValoresDiagramas, null);

            formDesenho.AtualizaShaders(true, false);

            string esf = "";
            formDesenho.MostrarEsforcosEmSelecionados = cbFiltroEsforcos.SelectedIndex == 1;
            
            if (formDesenho.Diagrama_gradiente)
                pnCorResultados.Visible = true;


            formDesenho.MostraTextoDiagramas = ((string)btValoresDiagramas.Tag == "1");
         
            ConfirmaEscalaDiagrama();

            if (formDesenho.MostraTextoDiagramas)
            {
                ConfiguracoesPGi.DiagramaOpcoesVisualizacao.percentual_maximo = 100 - trackValorDiagramas.Value;
    
              //  btValoresDiagramas.Tag = "0";
              //  btValoresDiagramas_Click_1(btValoresDiagramas, null);

                Preenche_barras_valores_esforco();

                formDesenho.CriarTextosEsforco();
                //      Preenche_barras_valores_esforco();
               

            }
            //   else
            //        ConfirmaEscalaDiagrama();


            //if (formDesenho.fx) esf = "fx";

            //   CalcularEscalaDiagramas(esf, formDesenho.MostrarEsforcosEmSelecionados);

            formDesenho.AtualizaShaders(true, false);
            
            AtualizaDesenho();

            ChamarAtualizacaoResultados(btOkEsforco, false);
            FechaAguardar();
        }

        private void button40_Click_1(object sender, EventArgs e)
        {

        }

        private void btConfirmaDeslocamentos_Click(object sender, EventArgs e)
        {
            //   formDesenho.DirtyPortico();
            // ConfirmaEscalaDef();
            if (!formDesenho.Estrutura.barras.Exists(o => o.Selecionado) && cbFiltroDeslocamentos.SelectedIndex == 1)
            {
                MessageBox.Show("Nenhum elemento foi selecionado!");
                return;
            }

            ChamaAguardar(this, "Processando. Aguarde...");

            bool texto = (string)btDeformacaoTextos.Tag == "1";
            formDesenho.MostraTextoDeformacoes = texto;

            ConfirmaEscalaDef();

            if (formDesenho.MostraTextoDeformacoes)
            {
                ConfiguracoesPGi.PorticoOpcoesVisualizacao.percentual_maximo_deformacao = 100 - trackValorDeformacao.Value;
                Preenche_nos_valores_deformacao();


               // btDeformacaoTextos.Tag = "0";
               // btDeformacaoTextos_Click(btDeformacaoTextos, null);

                //               Preenche_barras_valores_esforco();
                //             formDesenho.CriarTextosEsforco();
            }

            pnCorResultados.Visible = formDesenho.DeformacaoColorida;
            formDesenho.MostrarDeslocamentosEmSelecionados = cbFiltroDeslocamentos.SelectedIndex == 1;

            formDesenho.AtualizaShaders();
            AtualizaDesenho();

            ChamarAtualizacaoResultados(btConfirmaDeslocamentos, false);
            FechaAguardar();
        }

        private void btProjecao_Click(object sender, EventArgs e)
        {
            AlternaCamera();
        }

        void ChamarAtualizacaoResultados(Button botao, bool vermelho)
        {
            if (vermelho)
              botao.BackColor = System.Drawing.Color.Red;
            else
              botao.BackColor = SystemColors.Control;
        }

        private void cbFiltroTensao_SelectedIndexChanged(object sender, EventArgs e)
        {
            ChamarAtualizacaoResultados(btOkTensoes, true);
        }

        private void cbFiltroDeslocamentos_SelectedIndexChanged(object sender, EventArgs e)
        {
            ChamarAtualizacaoResultados(btConfirmaDeslocamentos, true);
        }
        
        private void btReacao_Click_1(object sender, EventArgs e)
        {

        }

        private void panel41_Paint(object sender, PaintEventArgs e)
        {

        }

        private void btDinamica1_Click(object sender, EventArgs e)
        {
            if (ConfiguracoesPGi.CfgProjeto.sistema.CalculaModosVibracao && cbModosVibracao.Items.Count > 0/* && formDesenho.Estrutura.PorticoEspacial != null*/)
            {
                formDesenho.AtualizaShaders();
                
                if ((string)btDinamica1.Tag == "1")
                {
                    btDinamica1.BackColor = System.Drawing.Color.FromArgb(64, 64, 64);
                    btDinamica1.Tag = "0";
                }
                else
                {
                    if (necessitaCalculo)
                    {
                        MessageBox.Show("Modificações foram feitas. É necessário calcular a estrutura.", "Atenção", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                        return;
                    }

                    btDinamica1.BackColor = System.Drawing.Color.White;
                    btDinamica1.Tag = "1";
                }

         //       btDeformacao.Tag = "0";
             //   btDeformacao.BackColor = System.Drawing.Color.FromArgb(64, 64, 64);

                cbModosVibracao.SelectedIndex = 0;
                
                bool modos = (string)btDinamica1.Tag == "1";

                formDesenho.MostraModosVibracao = modos;

                panelDinamica1.Visible = modos;

                if (!modos)
                {
                    formDesenho.Text = "Modelo 3D";

                    this.tabResultados.Controls.Remove(this.panelDinamica1);

                    if ((string)btModoRenderizado.Tag == "1")
                        btModoRenderizado_Click(btModoRenderizado, null);

                    if ((string)btModoColorido.Tag == "1")
                        btModoColorido_Click(btModoColorido, null);

                    if ((string)btMostraIndeformada.Tag == "1")
                        btMostraIndeformada_Click_1(btMostraIndeformada, null);

                    foreach (TBarraGenerica b in formDesenho.Estrutura.barras)
                    {
                        b.Visivel = true;
                        b.DirtyTriangulos = true;
                        b.DirtyArestas = true;
                        b.DirtySelecao = true;
                    }

                    btAnimarModo.Tag = "0";
                    btAnimarModo.BackColor = System.Drawing.Color.FromArgb(64, 64, 64);

                    formDesenho.AnimarModo(false, formDesenho.ModoVibracaoSolido, formDesenho.fatorModoVibracao);
                }
                else
                {
                    //cancela os outros resultados
                    if ((string)btDiagrama.Tag == "1")
                        btDiagrama_Click(btDiagrama, null);
                    if ((string)btDeformacao.Tag == "1")
                        btDeformacao_Click(btDeformacao, null);
                    if ((string)btTensoes.Tag == "1")
                        btTensoes_Click(btTensoes, null);
                    if ((string)btFlambagem.Tag == "1")
                        btFlambagem_Click(btFlambagem, null);

                    formDesenho.Text = "Resultados: " + btDinamica1.AccessibleName;

                    this.tabResultados.Controls.Remove(this.panelTipoResultado);

                    this.tabResultados.Controls.Add(this.panelDinamica1);
                    this.tabResultados.Controls.Add(this.panelTipoResultado);

                    panelTipoResultado.Dock = DockStyle.Left;
                    panelDinamica1.Dock = DockStyle.Left;

                    this.panelDinamica1.Location = new System.Drawing.Point(175, 3);

                    if ((string)btAnimarModo.Tag == "1")
                    {
                        btAnimarModo_Click(btAnimarModo, null);
                    }

                    foreach (TBarraGenerica b in formDesenho.Estrutura.barras)
                    {
                        b.Visivel = false;
                        b.DirtyTriangulos = true;
                        b.DirtyArestas = true;
                        b.DirtySelecao = true;
                    }

                    formDesenho.DirtyPortico();
                }

                formDesenho.AtualizaShaders();
                AtualizaDesenho();
            }
        }

        private void btModoColorido_Click(object sender, EventArgs e)
        {
            if ((string)btModoColorido.Tag == "1")
            {
                btModoColorido.BackColor = System.Drawing.Color.FromArgb(64, 64, 64);
                btModoColorido.Tag = "0";
            }
            else
            {
                btModoColorido.BackColor = System.Drawing.Color.White;
                btModoColorido.Tag = "1";
            }

            bool defCol = (string)btModoColorido.Tag == "1";

            formDesenho.ModoVibracaoColorido = defCol;

    //      if (formDesenho.Estrutura.PorticoEspacial == null)
        //        return;

            //  formDesenho.AtualizaShaders();
            // AtualizaDesenho();

            formDesenho.DirtyPortico();
            ChamarAtualizacaoResultados(btConfirmaModos, true);
        }

        private void btModoRenderizado_Click(object sender, EventArgs e)
        {
            if ((string)btModoRenderizado.Tag == "1")
            {
                btModoRenderizado.BackColor = System.Drawing.Color.FromArgb(64, 64, 64);
                btModoRenderizado.Tag = "0";
            }
            else
            {
                btModoRenderizado.BackColor = System.Drawing.Color.White;
                btModoRenderizado.Tag = "1";
            }

            bool defSolida = (string)btModoRenderizado.Tag == "1";

            formDesenho.ModoVibracaoSolido = defSolida;

            if ((string)btAnimarModo.Tag == "1")
              btAnimarModo_Click(btAnimarModo, null);

         //   if (formDesenho.Estrutura.PorticoEspacial == null)
            //    return;

            if (defSolida)
            {
                TBarraPortico[] barrasP = formDesenho.Estrutura.PorticoEspacial.barras.ToArray();
                formDesenho.CriaDeformacaoSolida(barrasP);

                if (escalaModoVibracao == 0)
                {
                    rbEscalaDeformacao.TextBoxText = "0.5";
                    formDesenho.EscalaModoVibracao = 0.5;
                }
                else
                {
                    rbEscalaDeformacao.TextBoxText = escalaDeformacao.ToString("n2");

                    formDesenho.EscalaModoVibracao = vEscalaDeformacao;
                }

                //forçar visualização de estrutura original unifilar
                if ((string)btMostraIndeformada.Tag == "0")
                {
                    if (!formDesenho.Unifilar)
                        AlternaVisualizacaoUnifilar(false);

                    btMostraIndeformada_Click_1(btMostraIndeformada, null);
                }
            }

            formDesenho.ArestasResultado = !formDesenho.ArestasResultado;

            if (formDesenho.Estrutura.PorticoEspacial == null)
                return;

            ConfirmaEscalaModoVibracao();
        }

        private void EscalaModo_UpDown_ValueChanged(object sender, EventArgs e)
        {
            if ((int)EscalaModo_UpDown.Value < escalaDef_anterior)
            {
                if (edEscalaModo.Text.ToString() == string.Empty)
                    return;

                if (escalaModoVibracao == 0)
                    vEscalaModoVibracao = double.Parse(edEscalaModo.Text.ToString()) - 1;
                else
                    vEscalaModoVibracao = double.Parse(edEscalaModo.Text.ToString()) - escalaModoVibracao;

                if (vEscalaModoVibracao - 1 < 0)
                    vEscalaModoVibracao = 0;
                else
                    vEscalaModoVibracao -= 1;

                if ((string)btAnimarModo.Tag == "1") btAnimarModo_Click(btAnimarModo, null);

                edEscalaModo.Text = System.Convert.ToString(vEscalaModoVibracao);
                formDesenho.fatorModoVibracao = vEscalaModoVibracao;
                //   formDesenho.AtualizaShaders();
                //     AtualizaDesenho();
            }
            else
            {
                if (edEscalaModo.Text.ToString() == string.Empty)
                    return;

                if (escalaModoVibracao == 0)
                    vEscalaModoVibracao = double.Parse(edEscalaModo.Text.ToString()) + 1;
                else
                    vEscalaModoVibracao = double.Parse(edEscalaModo.Text.ToString()) + escalaModoVibracao;

                edEscalaModo.Text = System.Convert.ToString(vEscalaModoVibracao);

                formDesenho.fatorModoVibracao = vEscalaModoVibracao;

                if ((string)btAnimarDeformacao.Tag == "1") btAnimarDeformacao_Click_1(btAnimarDeformacao, null);

                //   formDesenho.AtualizaShaders();

                //   if (btDeformacaoSolida.Checked)
                //       formDesenho.CriaDeformacaoSolida(formDesenho.Estrutura.PorticoEspacial.barras);

                //  AtualizaDesenho();
            }
            formDesenho.DirtyPortico();
            ChamarAtualizacaoResultados(btConfirmaModos, true);

            escalaDef_anterior = (int)EscalaModo_UpDown.Value;
        }

        private void edEscalaModo_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyData == Keys.Enter)
            {
                ConfirmaEscalaModoVibracao();
            }
        }

        private void cbModosVibracao_SelectedIndexChanged(object sender, EventArgs e)
        {
            if ((string)btAnimarModo.Tag == "1")
                btAnimarModo_Click(btAnimarModo, null);

            int modo = cbModosVibracao.SelectedIndex;
            var portico = formDesenho?.Estrutura?.PorticoEspacial;
            if (!ConfiguracoesPGi.CfgProjeto.sistema.CalculaModosVibracao ||
                portico?.ModosVibracao == null || modo < 0 ||
                modo >= portico.ModosVibracao.GetLength(1))
                return;

            CalcularEscalaModosVibracao();
            if (formDesenho.MostraModosVibracao)
            {
                ChamarAtualizacaoResultados(btConfirmaModos, true);
            }
        }

        private void btConfirmaModos_Click(object sender, EventArgs e)
        {
            ChamaAguardar(this, "Processando. Aguarde...");

            ConfirmaEscalaModoVibracao();

            formDesenho.AtualizaShaders();
            AtualizaDesenho();

            ChamarAtualizacaoResultados(btConfirmaModos, false);
            FechaAguardar();
        }

        private void btRelatorioAnaliseModal_Click(object sender, EventArgs e)
        {
            var portico = formDesenho?.Estrutura?.PorticoEspacial;
            if (necessitaCalculo || portico?.PercentuaisMassaModal == null)
            {
                MessageBox.Show("Calcule a análise modal antes de gerar o relatório.", "Análise modal",
                    MessageBoxButtons.OK, MessageBoxIcon.Information);
                return;
            }
            try
            {
                string texto = new TRelatoriosResultados().RelatorioAnaliseModal(portico);
                var relatorio = new FRelatorios
                {
                    Text = "Relatório da análise modal",
                    TextoRelatorio = texto,
                    StartPosition = FormStartPosition.CenterScreen
                };
                relatorio.Show(this);
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Relatório da análise modal", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void button51_Click(object sender, EventArgs e)
        {
            formDesenho.RemeverDosObjetosSelecionados_Nao_Copiaveis();

            if (formDesenho.ObjetosSelecionados.Count > 0)
            {
                formDesenho.ComandoEdicao(Const.ID_DIVIDIR_NAS_INTERSECOES, eTipoComando.edit);
                formDesenho.MouseEdit(0, 0, 0, 0, "", 0);
            }
            else
                formDesenho.ComandoEdicao(Const.ID_DIVIDIR_NAS_INTERSECOES);
        }

        private void btFlambagem_Click(object sender, EventArgs e)
        {
            if (ConfiguracoesPGi.CfgProjeto.sistema.CalculaModosFlambagem /*&& cbModosFlambagem.Items.Count > 0&& formDesenho.Estrutura.PorticoEspacial != null*/)
            {
                formDesenho.AtualizaShaders();
               /* if ((string)btFlambagem.Tag == "1")
                {
                    btFlambagem.BackColor = System.Drawing.Color.FromArgb(64, 64, 64);
                    btFlambagem.Tag = "0";
                }
                else
                {
                    if (necessitaCalculo)
                    {
                        MessageBox.Show("Modificações foram feitas. É necessário calcular a estrutura.", "Atenção", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                        return;
                    }
                    btFlambagem.BackColor = System.Drawing.Color.White;
                    btFlambagem.Tag = "1";
                }*/
                UpdateCorBotao(btFlambagem);
                
              //  cbModosFlambagem.SelectedIndex = 0;
                bool modos = (string)btFlambagem.Tag == "1";
                formDesenho.MostraModosFlambagem = modos;
                panelModosFlambagem.Visible = modos;

                if (!modos)
                {
                    formDesenho.Text = "Modelo 3D";

                    this.tabResultados.Controls.Remove(this.panelModosFlambagem);

                    if ((string)btFlambagemRenderizado.Tag == "1")
                        btFlambagemRenderizado_Click(btFlambagemRenderizado, null);

                    if ((string)btFlambagemColorido.Tag == "1")
                        btFlambagemColorido_Click(btFlambagemColorido, null);

                    if ((string)btMostraIndeformada.Tag == "1")
                        btMostraIndeformada_Click_1(btMostraIndeformada, null);

                    foreach (TBarraGenerica b in formDesenho.Estrutura.barras)
                    {
                        b.Visivel = true;
                        b.DirtyTriangulos = true;
                        b.DirtyArestas = true;
                        b.DirtySelecao = true;
                    }

                    btAnimarModo.Tag = "0";
                    btAnimarModo.BackColor = System.Drawing.Color.FromArgb(64, 64, 64);

                    formDesenho.AnimarModo(false, formDesenho.FlambagemSolido, formDesenho.fatorFlambagem);
                }
                else
                {
                    cbTipoCargaResultado.SelectedIndex = 1;
                    for (int i = 0; i < cbTipoCargaResultado.Items.Count; i++)
                    {
                        if (formDesenho.Estrutura.combinacoes[i].categoriaCombinacao == CategoriaCombinacao.Estabilidade)
                        {
                            cbResultadoCombinacoes.SelectedIndex = i;
                            AlternaResultadosCombinacao(false);
                            break;
                        }
                    }
                    //cancela os outros resultados
                    if ((string)btDiagrama.Tag == "1")
                        btDiagrama_Click(btDiagrama, null);
                    if ((string)btDeformacao.Tag == "1")
                        btDeformacao_Click(btDeformacao, null);
                    if ((string)btTensoes.Tag == "1")
                        btTensoes_Click(btTensoes, null);
                    if ((string)btDinamica1.Tag == "1")
                        btDinamica1_Click(btDinamica1, null);

                    formDesenho.Text = "Resultados: " + btFlambagem.AccessibleName;

                    this.tabResultados.Controls.Remove(this.panelTipoResultado);
                    this.tabResultados.Controls.Add(this.panelModosFlambagem);
                    this.tabResultados.Controls.Add(this.panelTipoResultado);

                    panelTipoResultado.Dock = DockStyle.Left;
                    panelModosFlambagem.Dock = DockStyle.Left;

                    this.panelModosFlambagem.Location = new System.Drawing.Point(175, 3);

                    if ((string)btAnimarModo.Tag == "1")
                    {
                        btAnimarModo_Click(btAnimarModo, null);
                    }

                    foreach (TBarraGenerica b in formDesenho.Estrutura.barras)
                    {
                        b.Visivel = false;
                        b.DirtyTriangulos = true;
                        b.DirtyArestas = true;
                        b.DirtySelecao = true;
                    }

                    formDesenho.DirtyPortico();
                }

                formDesenho.AtualizaShaders();
                AtualizaDesenho();
            }
        }

        void UpdateCorBotao(Button bt)
        {
            if ((string)bt.Tag == "1")
            {
                bt.BackColor = System.Drawing.Color.FromArgb(64, 64, 64);
                bt.Tag = "0";
            }
            else
            {
                if (necessitaCalculo)
                {
                    MessageBox.Show("Modificações foram feitas. É necessário calcular a estrutura.", "Atenção", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                    return;
                }

                bt.BackColor = System.Drawing.Color.White;
                bt.Tag = "1";
            }
        }

        private void btFlambagemRenderizado_Click(object sender, EventArgs e)
        {
            UpdateCorBotao(btFlambagemRenderizado);

            bool defSolida = (string)btFlambagemRenderizado.Tag == "1";

            formDesenho.FlambagemSolido = defSolida;

            if ((string)btAnimarModo.Tag == "1")
                btAnimarModo_Click(btAnimarModo, null);

            //   if (formDesenho.Estrutura.PorticoEspacial == null)
            //    return;

            if (defSolida)
            {
                TBarraPortico[] barrasP = formDesenho.Estrutura.PorticoEspacial.barras.ToArray();
                formDesenho.CriaDeformacaoSolida(barrasP);

                if (escalaFlambagem == 0)
                {
                    rbEscalaDeformacao.TextBoxText = "0.5";
                    formDesenho.EscalaFlambagem = 0.5;
                }
                else
                {
                    rbEscalaDeformacao.TextBoxText = escalaDeformacao.ToString("n2");

                    formDesenho.EscalaFlambagem = vEscalaFlambagem;
                }

                //forçar visualização de estrutura original unifilar
                if ((string)btMostraIndeformada.Tag == "0")
                {
                    if (!formDesenho.Unifilar)
                        AlternaVisualizacaoUnifilar(false);

                    btMostraIndeformada_Click_1(btMostraIndeformada, null);
                }
            }

            formDesenho.ArestasResultado = !formDesenho.ArestasResultado;

            if (formDesenho.Estrutura.PorticoEspacial == null)
                return;

            ConfirmaEscalaFlambagem();
        }

        private void btConfirmaFlambagem_Click(object sender, EventArgs e)
        {
            ChamaAguardar(this, "Processando. Aguarde...");
            ConfirmaEscalaFlambagem();
            formDesenho.AtualizaShaders();
            AtualizaDesenho();
            ChamarAtualizacaoResultados(btConfirmaFlambagem, false);
            FechaAguardar();
        }

        private void edEscalaFlambagem_UpDown_ValueChanged(object sender, EventArgs e)
        {
            if ((int)edEscalaFlambagem_UpDown.Value < escalaDef_anterior)
            {
                if (edEscalaFlambagem.Text.ToString() == string.Empty)
                    return;

                if (escalaFlambagem == 0)
                    vEscalaFlambagem = double.Parse(edEscalaFlambagem.Text.ToString()) - 1;
                else
                    vEscalaFlambagem = double.Parse(edEscalaFlambagem.Text.ToString()) - escalaFlambagem;

                if (vEscalaFlambagem - 1 < 0)
                    vEscalaFlambagem = 0;
                else
                    vEscalaFlambagem -= 1;

                if ((string)btAnimarModo.Tag == "1") btAnimarModo_Click(btAnimarModo, null);

                edEscalaFlambagem.Text = System.Convert.ToString(vEscalaFlambagem);
                formDesenho.fatorFlambagem = vEscalaFlambagem;
                //   formDesenho.AtualizaShaders();
                //     AtualizaDesenho();
            }
            else
            {
                if (edEscalaFlambagem.Text.ToString() == string.Empty)
                    return;

                if (escalaFlambagem == 0)
                    vEscalaFlambagem = double.Parse(edEscalaFlambagem.Text.ToString()) + 1;
                else
                    vEscalaFlambagem = double.Parse(edEscalaFlambagem.Text.ToString()) + escalaFlambagem;

                edEscalaFlambagem.Text = System.Convert.ToString(vEscalaFlambagem);

                formDesenho.fatorFlambagem = vEscalaFlambagem;

                if ((string)btAnimarDeformacao.Tag == "1") btAnimarDeformacao_Click_1(btAnimarDeformacao, null);

                //   formDesenho.AtualizaShaders();

                //   if (btDeformacaoSolida.Checked)
                //       formDesenho.CriaDeformacaoSolida(formDesenho.Estrutura.PorticoEspacial.barras);

                //  AtualizaDesenho();
            }
            formDesenho.DirtyPortico();
            ChamarAtualizacaoResultados(btConfirmaFlambagem, true);

            escalaDef_anterior = (int)edEscalaFlambagem_UpDown.Value;
        }

        private void btAnimarFlambagem_Click(object sender, EventArgs e)
        {
            if ((formDesenho.MostraModosFlambagem))
            {
                if ((string)btAnimarModo.Tag == "1")
                {
                    btAnimarModo.BackColor = System.Drawing.Color.FromArgb(64, 64, 64);
                    btAnimarModo.Tag = "0";
                }
                else
                {
                    btAnimarModo.BackColor = System.Drawing.Color.White;
                    btAnimarModo.Tag = "1";
                }

                bool animar = (string)btAnimarModo.Tag == "1";

                formDesenho.AnimarModo(animar, formDesenho.FlambagemSolido, formDesenho.fatorFlambagem);
            }
        }

        private void btFlambagemColorido_Click(object sender, EventArgs e)
        {
            if ((string)btFlambagemColorido.Tag == "1")
            {
                btFlambagemColorido.BackColor = System.Drawing.Color.FromArgb(64, 64, 64);
                btFlambagemColorido.Tag = "0";
            }
            else
            {
                btFlambagemColorido.BackColor = System.Drawing.Color.White;
                btFlambagemColorido.Tag = "1";
            }

            bool defCol = (string)btFlambagemColorido.Tag == "1";

            formDesenho.FlambagemColorido = defCol;

            formDesenho.DirtyPortico();
            ChamarAtualizacaoResultados(btConfirmaFlambagem, true);
        }

        private void btAnimarModo_Click(object sender, EventArgs e)
        {
            if ((formDesenho.MostraModosVibracao))
            {
                if ((string)btAnimarModo.Tag == "1")
                {
                    btAnimarModo.BackColor = System.Drawing.Color.FromArgb(64, 64, 64);
                    btAnimarModo.Tag = "0";
                }
                else
                {
                    btAnimarModo.BackColor = System.Drawing.Color.White;
                    btAnimarModo.Tag = "1";
                }

                bool animar = (string)btAnimarModo.Tag == "1";

                formDesenho.AnimarModo(animar,formDesenho.ModoVibracaoSolido,formDesenho.fatorModoVibracao);
            }
        }

        private void cbFiltroEsforcos_SelectedIndexChanged(object sender, EventArgs e)
        {
            formDesenho.DirtyPortico();
            ChamarAtualizacaoResultados(btOkEsforco, true);
        }

        void AlternaVisualizacaoUnifilar(bool atuShaders= true)
        {
            formDesenho.Unifilar = !formDesenho.Unifilar;
            ConfiguracoesPGi.F3DOpcoesVisualizacao.Unifilar = formDesenho.Unifilar;

            formDesenho.Estrutura.barras.ForEach(o => o.DirtyArestas = true);

            if (!formDesenho.Unifilar)
            {
                formDesenho.Estrutura.barras.ForEach(o => o.DirtySelecao = true);
                formDesenho.Estrutura.barras.ForEach(o => o.DirtyTriangulos = true);
            }

            if (atuShaders)
            {
                formDesenho.AtualizaShaders();
                formDesenho.DesenhaObjetos();
                formDesenho.glControl.SwapBuffers();
            }
        }
        private void button47_Click_1(object sender, EventArgs e)
        {
            AlternaVisualizacaoUnifilar();
        }

        private void pnCargasCombinacoes_Paint(object sender, PaintEventArgs e)
        {

        }

        private void btDeformacaoColorida_Click(object sender, EventArgs e)
        {
            if (formDesenho.Estrutura.PorticoEspacial == null)
                return;

            if ((string)btDeformacaoColorida.Tag == "1")
            {
                btDeformacaoColorida.BackColor = System.Drawing.Color.FromArgb(64, 64, 64);
                btDeformacaoColorida.Tag = "0";
            }
            else
            {
                btDeformacaoColorida.BackColor = System.Drawing.Color.White;
                btDeformacaoColorida.Tag = "1";
            }

            bool defCol = (string)btDeformacaoColorida.Tag == "1";

            formDesenho.DeformacaoColorida = defCol;

            //  formDesenho.AtualizaShaders();
            // AtualizaDesenho();

            formDesenho.DirtyPortico();
            ChamarAtualizacaoResultados(btConfirmaDeslocamentos, true);

         //   pnCorResultados.Visible = defCol;
        }

        bool maxtop, maxtop2, maxleft, maxleft2;
        private void panel50_MouseMove(object sender, System.Windows.Forms.MouseEventArgs e)
        {
            if ((clicouPanel) && (((sender as Panel).Top + ((int)(e.Y - yant))) <0))
                maxtop2 = true;
            else
                maxtop2 = false;

            if ((clicouPanel) && (((sender as Panel).Top + ((sender as Panel).Height +40) + ((int)(e.Y - yant))) > this.Height))
                maxtop = true;
            else
                maxtop = false;

            if ((clicouPanel) && (((sender as Panel).Left +  ((int)(e.X - xant))) <0))
                maxleft = true;
            else
                maxleft = false;

            if ((clicouPanel) && ((((sender as Panel).Left + (sender as Panel).Width+15) + ((int)(e.X - xant))) > this.Width))
                maxleft2 = true;
            else
                maxleft2 = false;

            if (clicouPanel && ! maxtop && !maxtop2 && ! maxleft && !maxleft2)
            {
                (sender as Panel).Left += (int)(e.X - xant);
                (sender as Panel).Top += (int)(e.Y - yant);
                pnCargasLeft = ((sender as Panel).Left*100) / this.Width;
                pnCargasTop = ((sender as Panel).Top * 100) / this.Height;

                pnCargasLeftRel = (double)pnCargasCombinacoes.Left / this.ClientSize.Width;
                pnCargasTopRel = (double)pnCargasCombinacoes.Top / this.ClientSize.Height;

                AtualizaDesenho();           
            }
        }

        private void ribbonButton56_Click(object sender, EventArgs e)
        {
            
        }

        private void ribbonButton57_Click_1(object sender, EventArgs e)
        {
            
        }

        private void ribbonButton61_Click_1(object sender, EventArgs e)
        {
            
        }

        private void rbMostraIndeformada_Click(object sender, EventArgs e)
        {
            rbMostraIndeformada2.Checked = !rbMostraIndeformada2.Checked;

            foreach (TBarraGenerica b in formDesenho.Estrutura.barras)
                b.Visivel = rbMostraIndeformada2.Checked;

            formDesenho.AtualizaShaders();
            AtualizaDesenho();
        }

        private void toolStripButton1_Click(object sender, EventArgs e)
        {
            formDesenho.MostrarCargas = !formDesenho.MostrarCargas;
            formDesenho.MostrarCargasPeloCaso();

           /* if (btVisualizarCargas.Checked)
              btVisualizarCargas.Image = imgLigaDesliga19x19.Images[0];
            else
              btVisualizarCargas.Image = imgLigaDesliga19x19.Images[1];*/
        }

        private void toolStripComboBox1_SelectedIndexChanged(object sender, EventArgs e)
        {
            formDesenho.MostrarCargasPeloCaso();
        }

        void CancelaDiagramas()
        {
            /*if (btMx.Checked)
                ribbonButton5_Click_4(btDeformacaoTextos, null);

            if (btMy.Checked)
                btDeformacaoSolida_Click(btDeformacaoSolida, null);

            if (btMz.Checked)
                rbMostraIndeformada_Click(rbMostraIndeformada, null);*/
        }
        private void btDiagramas_Click(object sender, EventArgs e)
        {
            btDiagramas2.Checked = !btDiagramas2.Checked;
            PanelDiagramas2.Visible = btDiagramas2.Checked;
            if (!btDiagramas2.Checked)
            {
                CancelaDiagramas();
               
                btValoresDiagramas2.Checked = false;
                formDesenho.MostraTextoDiagramas = false;

                foreach (TBarraGenerica b in formDesenho.Estrutura.barras)
                    b.Visivel = !btDeformacoes2.Checked;

                btfx2.Checked = false;
                btfy2.Checked = false;
                btfz2.Checked = false;
                btmx2.Checked = false;
                btmy2.Checked = false;
                btmz2.Checked = false;

                formDesenho.fx = false;
                formDesenho.fy = false;
                formDesenho.fz = false;
                formDesenho.mx = false;
                formDesenho.my = false;
                formDesenho.mz = false;
            }
            else
            {
                foreach (TBarraGenerica b in formDesenho.Estrutura.barras)
                    b.Visivel = true;
            }

            formDesenho.AtualizaShaders();
            AtualizaDesenho();
        }

        private void btfx_Click(object sender, EventArgs e)
        {
            btfx2.Checked = !btfx2.Checked;
            formDesenho.fx = btfx2.Checked;

            if (btValoresDiagramas2.Checked)
                btValoresDiagramas_Click(btValoresDiagramas2, null);

            if (btfx2.Checked)
            {
                double max = formDesenho.Estrutura.PorticoEspacial.MaximoEsforco("fx", cbTipoCargaResultado.SelectedIndex,
                                                                                 cbResultadoCasos.SelectedIndex,
                                                                                 cbResultadoCombinacoes.SelectedIndex);

                if (!Geom.Iguais(max,0))
                {
                    escalaDiagrama = 1 / max;

                    if (escalaDiagrama == 0)
                    {
                        escalaDiagrama = 0.5;
                        formDesenho.fatorDiagramas = 0.5;
                        rbEscalaDiagrama.TextBoxText = escalaDiagrama.ToString();
                    }
                    else
                    {
                        rbEscalaDiagrama.TextBoxText = "1";// escalaDiagrama.ToString();
                        formDesenho.fatorDiagramas = 1 * escalaDiagrama;
                    }
                }
            }

            formDesenho.AtualizaShaders();
            AtualizaDesenho();
        }

        private void btValoresDiagramas_Click(object sender, EventArgs e)
        {
            btValoresDiagramas2.Checked = !btValoresDiagramas2.Checked;
            if (btValoresDiagramas2.Checked)
                Preenche_barras_valores_esforco();

            if (formDesenho.fx || formDesenho.my || formDesenho.mz || formDesenho.mx || formDesenho.fy || formDesenho.fz)
            {
               /* if (!formDesenho.fx && !formDesenho.mx)*/ 
                
                //formDesenho.CriarTextosEsforco();
                //if (formDesenho.fx || formDesenho.mx) formDesenho.CriarTextosAxial_Torcor();

                formDesenho.MostraTextoDiagramas = btValoresDiagramas2.Checked;
            }
            else
            {
                btValoresDiagramas2.Checked = false;
                formDesenho.MostraTextoDiagramas = false;
            }



            AtualizaDesenho();
        }

        private void btfy_Click(object sender, EventArgs e)
        {
            btfy2.Checked = !btfy2.Checked;
            formDesenho.fy = btfy2.Checked;
        }

        private void btfz_Click(object sender, EventArgs e)
        {
            btfz2.Checked = !btfz2.Checked;
            formDesenho.fz = btfz2.Checked;
        }

        private void btmx_Click(object sender, EventArgs e)
        {
            btmx2.Checked = !btmx2.Checked;

            formDesenho.mx = btmx2.Checked;

            if (btValoresDiagramas2.Checked)
                btValoresDiagramas_Click(btValoresDiagramas2, null);

            if (btmx2.Checked)
            {
                double max = formDesenho.Estrutura.PorticoEspacial.MaximoEsforco("mx", cbTipoCargaResultado.SelectedIndex,
                                                                                 cbResultadoCasos.SelectedIndex,
                                                                                 cbResultadoCombinacoes.SelectedIndex);

                if (!Geom.Iguais(max, 0))
                {
                    escalaDiagrama = 1 / max;

                    if (escalaDiagrama == 0)
                    {
                        escalaDiagrama = 0.5;
                        formDesenho.fatorDiagramas = 0.5;
                        rbEscalaDiagrama.TextBoxText = escalaDiagrama.ToString();
                    }
                    else
                    {
                        rbEscalaDiagrama.TextBoxText = "1";// escalaDiagrama.ToString();
                        formDesenho.fatorDiagramas = 1 * escalaDiagrama;
                    }
                }
            }

            formDesenho.AtualizaShaders();
            AtualizaDesenho();
        }

        private void btmy_Click(object sender, EventArgs e)
        {
            btmy2.Checked = !btmy2.Checked;

            formDesenho.my = btmy2.Checked;

            if (btValoresDiagramas2.Checked)
              btValoresDiagramas_Click(btValoresDiagramas2, null);

            if (btmy2.Checked)
            {
                double max = formDesenho.Estrutura.PorticoEspacial.MaximoEsforco("my", cbTipoCargaResultado.SelectedIndex,
                                                                                 cbResultadoCasos.SelectedIndex,
                                                                                 cbResultadoCombinacoes.SelectedIndex);

                if (!Geom.Iguais(max, 0))
                {
                    escalaDiagrama = 1 / max;

                    if (escalaDiagrama == 0)
                    {
                        escalaDiagrama = 0.5;
                        formDesenho.fatorDiagramas = 0.5;
                        rbEscalaDiagrama.TextBoxText = escalaDiagrama.ToString();
                    }
                    else
                    {
                        rbEscalaDiagrama.TextBoxText = "1";// escalaDiagrama.ToString();
                        formDesenho.fatorDiagramas = 1 * escalaDiagrama;
                    }
                }
            }
            
            formDesenho.AtualizaShaders();
            AtualizaDesenho();
        }

        private void btmz_Click(object sender, EventArgs e)
        {
            btmz2.Checked = !btmz2.Checked;

            formDesenho.mz = btmz2.Checked;

            if (btValoresDiagramas2.Checked)
                btValoresDiagramas_Click(btValoresDiagramas2, null);

            if (btmz2.Checked)
            {
                double max = formDesenho.Estrutura.PorticoEspacial.MaximoEsforco("mz", cbTipoCargaResultado.SelectedIndex,
                                                                                 cbResultadoCasos.SelectedIndex,
                                                                                 cbResultadoCombinacoes.SelectedIndex);

                if (!Geom.Iguais(max, 0))
                {
                    escalaDiagrama = 1 / max;

                    if (escalaDiagrama == 0)
                    {
                        escalaDiagrama = 0.5;
                        formDesenho.fatorDiagramas = 0.5;
                        rbEscalaDiagrama.TextBoxText = "0.5";
                    }
                    else
                    {
                        rbEscalaDiagrama.TextBoxText = "1";// escalaDiagrama.ToString();
                        formDesenho.fatorDiagramas = 1 * escalaDiagrama;
                    }
                }
            }

            formDesenho.AtualizaShaders();
            AtualizaDesenho();
        }

        private void rbEscalaDiagrama_DownButtonClicked(object sender, System.Windows.Forms.MouseEventArgs e)
        {
            if (rbEscalaDiagrama.TextBoxText.ToString() == string.Empty)
                return;

            if (rbEscalaDiagrama.TextBoxText.ToString().Trim() != "")
            {
                if (double.Parse(rbEscalaDiagrama.TextBoxText.ToString()) > 1)
                {
                    vEscalaDiagrama = double.Parse(rbEscalaDiagrama.TextBoxText.ToString()) - 1;

           /*     if (vEscalaDiagrama - 1 < 0)
                    vEscalaDiagrama = 0;
                else
                    vEscalaDiagrama -= 1;*/

                rbEscalaDiagrama.TextBoxText = System.Convert.ToString(vEscalaDiagrama);
                formDesenho.fatorDiagramas = vEscalaDiagrama * escalaDiagrama;
                formDesenho.AtualizaShaders();

                if (formDesenho.MostraTextoDiagramas)
                {
                  //  if (formDesenho.fx || formDesenho.mx)
                  //      formDesenho.CriarTextosAxial_Torcor();
                  //  else
                        formDesenho.CriarTextosEsforco();
                }

                AtualizaDesenho();
                }
            }
        }

        private void rbEscalaDiagrama_TextBoxKeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyData == Keys.Enter)
            {
                ConfirmaEscalaDiagrama();
            }
        }

        private void rbEscalaDiagrama_UpButtonClicked(object sender, System.Windows.Forms.MouseEventArgs e)
        {
            if (rbEscalaDiagrama.TextBoxText.ToString() == string.Empty)
                return;

            if (rbEscalaDiagrama.TextBoxText.ToString().Trim() != "")
            {
                vEscalaDiagrama = double.Parse(rbEscalaDiagrama.TextBoxText.ToString()) + 1;

                rbEscalaDiagrama.TextBoxText = System.Convert.ToString(vEscalaDiagrama);

                formDesenho.fatorDiagramas = vEscalaDiagrama * escalaDiagrama;

                formDesenho.AtualizaShaders();
                if (formDesenho.MostraTextoDiagramas)
                {
                   // if (formDesenho.fx || formDesenho.mx)
                   //     formDesenho.CriarTextosAxial_Torcor();
                   // else
                        formDesenho.CriarTextosEsforco();
                }
                AtualizaDesenho();
            }
        }

        private void rbEscalaDiagrama_TextBoxKeyPress(object sender, System.Windows.Forms.KeyPressEventArgs e)
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

        private void ribbonButton66_Click(object sender, EventArgs e)
        {

        }

        private void ribbonButton30_Click(object sender, EventArgs e)
        {
            
        }

        private void ribbonButton32_Click(object sender, EventArgs e)
        {
            formDesenho.PlanoCorte = true;
            AtivaModoPlano("xy", true);
        }

        double vEscalaDeformacao, vEscalaDiagrama, vEscalaModoVibracao, vEscalaFlambagem;
        private void ribbonUpDown1_UpButtonClicked_1(object sender, System.Windows.Forms.MouseEventArgs e)
        {
            if (rbEscalaDeformacao.TextBoxText.ToString() == string.Empty)
                return;

            if (escalaDeformacao == 0)
                vEscalaDeformacao = double.Parse(rbEscalaDeformacao.TextBoxText.ToString()) + 1;
            else
                vEscalaDeformacao = double.Parse(rbEscalaDeformacao.TextBoxText.ToString()) + escalaDeformacao;

            rbEscalaDeformacao.TextBoxText = System.Convert.ToString(vEscalaDeformacao);

            formDesenho.fatorDeformacao = vEscalaDeformacao;

            if (btAnimarDeformacao2.Checked) btAnimarDeformacao_Click(btAnimarDeformacao2, null);

            formDesenho.AtualizaShaders();

            //   if (btDeformacaoSolida.Checked)
            //       formDesenho.CriaDeformacaoSolida(formDesenho.Estrutura.PorticoEspacial.barras);

            AtualizaDesenho();
        }

        private void ribbonUpDown1_DownButtonClicked_1(object sender, System.Windows.Forms.MouseEventArgs e)
        {
            if (rbEscalaDeformacao.TextBoxText.ToString() == string.Empty)
                return;

            if (escalaDeformacao == 0)
                vEscalaDeformacao = double.Parse(rbEscalaDeformacao.TextBoxText.ToString()) - 1;
            else
                vEscalaDeformacao = double.Parse(rbEscalaDeformacao.TextBoxText.ToString()) - escalaDeformacao;

            if (vEscalaDeformacao - 1 < 0)
                vEscalaDeformacao = 0;
            else
                vEscalaDeformacao -= 1;

            if (btAnimarDeformacao2.Checked) btAnimarDeformacao_Click(btAnimarDeformacao2, null);

            rbEscalaDeformacao.TextBoxText = System.Convert.ToString(vEscalaDeformacao);
            formDesenho.fatorDeformacao = vEscalaDeformacao;
            formDesenho.AtualizaShaders();
            AtualizaDesenho();
        }
        protected override bool ProcessCmdKey(ref Message msg, Keys keyData)
        {
            if ((this.ActiveControl == cbCasoCarga) && (keyData == Keys.Down))
            {
                if (cbCasoCarga.SelectedIndex < cbCasoCarga.Items.Count-1)
                  cbCasoCarga.SelectedIndex += 1;
                
                cbCasoCarga.Focus();
                return true;
            }
            else
            if ((this.ActiveControl == cbCasoCarga) && (keyData == Keys.Up))
            {
                if (cbCasoCarga.SelectedIndex > 0)
                    cbCasoCarga.SelectedIndex -= 1;

                cbCasoCarga.Focus();
                return true;
            }
            if ((this.ActiveControl == cbResultadoCombinacoes) && (keyData == Keys.Down))
            {
                if (cbResultadoCombinacoes.SelectedIndex < cbResultadoCombinacoes.Items.Count - 1)
                    cbResultadoCombinacoes.SelectedIndex += 1;

                cbResultadoCombinacoes.Focus();
                return true;
            }
            else
            if ((this.ActiveControl == cbResultadoCombinacoes) && (keyData == Keys.Up))
            {
                if (cbResultadoCombinacoes.SelectedIndex > 0)
                    cbResultadoCombinacoes.SelectedIndex -= 1;

                cbResultadoCombinacoes.Focus();
                return true;
            }
            else
            {
                return base.ProcessCmdKey(ref msg, keyData);
            }
        }

        int escalaDef_anterior = 1;
        private void EscalaDeformacao_UpDown_ValueChanged(object sender, EventArgs e)
        {
            if ((int)EscalaDeformacao_UpDown.Value < escalaDef_anterior)
            {
                if (edEscalaDeformacao.Text.ToString() == string.Empty)
                    return;

                if (escalaDeformacao == 0)
                    vEscalaDeformacao = double.Parse(edEscalaDeformacao.Text.ToString()) - 1;
                else
                    vEscalaDeformacao = double.Parse(edEscalaDeformacao.Text.ToString()) - escalaDeformacao;

                if (vEscalaDeformacao - 1 < 0)
                    vEscalaDeformacao = 0;
                else
                    vEscalaDeformacao -= 1;

                if ((string)btAnimarDeformacao.Tag == "1") btAnimarDeformacao_Click_1(btAnimarDeformacao, null);

                edEscalaDeformacao.Text = System.Convert.ToString(vEscalaDeformacao);
                formDesenho.fatorDeformacao = vEscalaDeformacao;
             //   formDesenho.AtualizaShaders();
           //     AtualizaDesenho();
            }
            else
            {
                if (edEscalaDeformacao.Text.ToString() == string.Empty)
                    return;

                if (escalaDeformacao == 0)
                    vEscalaDeformacao = double.Parse(edEscalaDeformacao.Text.ToString()) + 1;
                else
                    vEscalaDeformacao = double.Parse(edEscalaDeformacao.Text.ToString()) + escalaDeformacao;

                edEscalaDeformacao.Text = System.Convert.ToString(vEscalaDeformacao);

                formDesenho.fatorDeformacao = vEscalaDeformacao;

                if ((string)btAnimarDeformacao.Tag == "1") btAnimarDeformacao_Click_1(btAnimarDeformacao, null);

             //   formDesenho.AtualizaShaders();

                //   if (btDeformacaoSolida.Checked)
                //       formDesenho.CriaDeformacaoSolida(formDesenho.Estrutura.PorticoEspacial.barras);

              //  AtualizaDesenho();
            }
            formDesenho.DirtyPortico();
            ChamarAtualizacaoResultados(btConfirmaDeslocamentos, true);

            escalaDef_anterior = (int)EscalaDeformacao_UpDown.Value;
        }

    }
}
