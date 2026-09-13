using Microsoft.TeamFoundation.MVVM;
using OpenTK;
using OpenTK.Graphics.OpenGL;
using OpenTK.Input;
using OpenTK.Platform;
using OpenTK.Platform.Windows;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Drawing.Imaging;
using System.IO;
using System.Linq;
using System.Net.NetworkInformation;
using System.Runtime.InteropServices;
using System.Runtime.InteropServices.WindowsRuntime;
using System.Runtime.Serialization;
using System.Runtime.Serialization.Formatters.Binary;
using System.Security;
using System.ServiceModel;
using System.ServiceModel.Description;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Controls;
using System.Windows.Forms;
using System.Windows.Ink;
using System.Windows.Media;
using System.Windows.Media.Animation;
using System.Windows.Media.TextFormatting;
using System.Windows.Shapes;
using System.Xml.Linq;
using Win32Interop.Enums;
using Win32Interop.Structs;
using WinFormAnimation;
using static alglib;
using static Microsoft.WindowsAPICodePack.Shell.PropertySystem.SystemProperties.System;
using static PG.Geom;

namespace PG
{

    public enum eObjetoDesenhoMouseDown
    {
        Done,       // objeto está completo
        DoneRepeat, // objeto completo, mas cria novo objeto do mesmo tipo
        Continue,   // este objeto necessita clique de mouse adicional
    }

    public enum eEditToolTipoSelecao
    {
        nenhum,
        multiSelecao,
        selecaoUnica
    }
    public enum eTipoComando
    {
        selecionar,
        pan,
        move,
        draw,
        edit
    }

    public partial class FPrincipal : WeifenLuo.WinFormsUI.Docking.DockContent
    {
        public Gerenciador gerenciador;

        public TDadosLaje DadosLaje;
        public TDadosCarga DadosCarga;
        public TDadosPilar DadosPilar;
        public TDadosViga DadosTrecho;
        public TDadosBarra DadosBarra;
        public TDadosApoio DadosApoio;
        public string arquivoAtual;

        public eTipoComando tipoComando = eTipoComando.selecionar;
        public eEditToolTipoSelecao editToolTipoSelecao = eEditToolTipoSelecao.nenhum;

        public string IdObjetoDesenho = string.Empty;
        public string IdFerramentaEdicao = string.Empty;
        public IEditTool FerramentaEdicao = null;

        public static float precisaoPixel, r, g, b;

        double fatorZoom;

        public static Coordenada Posicao;

        public CoordenadaD Ponto1_Coord, Ponto2_Coord, Ponto3_Coord, Posicao_Coord, PontoMedio1, PontoMedio2, PontoBase;

        public Coordenada Ponto1, Ponto2, Ponto3;

        public static int h, w;
        public string Texto;

        private int i;

        public static double[] ponto_zero;

        public bool Panning, CaixaSelecao, SelParcial, SelIntegral, OrtogonalLigado = true;
        public bool MostraDeformacoes, MostraTextoDeformacoes, MostraTextoDiagramas, mostraux, mostrauy, mostrauz;
        public bool MostraTensoesNormaisGradiente, MostraTensoesNormaisIsobandas;
        public bool MostraModosVibracao, MostraModosFlambagem;
        public int deformacao_U = 4;
        public int TipoTensao = 0;
        public bool CapPM, CapPR, CapQua, EditandoGrelha;

        public List<TPonto> Pontos;
        public List<TLinha> Linhas;
        public List<TCirculo> Circulos;
        public List<TArcoIMF> Arcos;
        public List<TTrechoViga> TrechosVigas;
        public List<TBarraGenerica> Barras;
        public List<TLaje> Lajes;
        public List<TPilar> Pilares;
        public List<TCargaPontual> CargasPontuais;
        public List<TCargaLinear> CargasLineares;
        public List<TSecao> Secoes;
        public List<TCasosCarga> CasosCarga;

        public List<TTexto> Textos;

        public List<TBarraGrelha> BarraGrelha;
        public List<TNoGrelha> NoGrelha;

        public PontoD[] RetanguloSelecao, RetanguloSelecao2;

        public List<TLayer> Layers;
        public List<TObjetoDesenho> ObjetosSelecionados;
        public List<TObjetoDesenho> NosSelecionados;
        public TObjetoDesenho ObjetoNovo = null;

        public List<TGrip> Grips;
        public List<TPavimento> Pavimentos;
        public int PavimentoAtual;

        private Dictionary<string, TObjetoDesenho> tiposObjetosDesenho;
        private Dictionary<string, IEditTool> editTools;

        TPonto pt1 = null;

        TGrip GripAtual = null;

        public TLayer ActiveLayer;
        public Dictionary<string, TLayer> LayersById;

        TPonto mousepoint = new TPonto(1);
        TPonto mouse3D = new TPonto(1);
        bool naTela1, naTela2;
        double z_clip1, z_clip2;


        public void LimpaTela()
        {
            Linhas.Clear();
            Circulos.Clear();
            Pontos.Clear();
            Textos.Clear();
            TrechosVigas.Clear();
            Barras.Clear();
            CasosCarga.Clear();
            Lajes.Clear();
            CargasPontuais.Clear();
            CargasLineares.Clear();
            Secoes.Clear();
            Pilares.Clear();
            Arcos.Clear();
            ObjetosSelecionados.Clear();
            NosSelecionados.Clear();
            undoBuffer.Clear();
            //if (LimpaLayers)
            //   PavimentoAtual.layers.Clear();

            Grips.Clear();

        }

        public void HabilitaCoords(bool h, bool copia = false, bool Foco = true, bool rotacionar = false, bool espelhar = false, bool mover = false)
        {
            edX.Visible = h;
            edY.Visible = h;
            edZ.Visible = h;
            btConfirmaCoord.Visible = h;

            //       edNumRepeticoes.Value = 1;

            if (copia)
                pnRepeticoesCopia.Visible = h;

            if (espelhar)
                pnEspelhar.Visible = h;
            else
                pnEspelhar.Visible = false;

           /* if (mover)
                pnMover.Visible = h;
            else
                pnMover.Visible = false;*/

            //   if (copia && edNumRepeticoes.Value > 1)
            //     edNumRepeticoes.Value = 1 ;

            if (h == false)
            {
                pnRepeticoesCopia.Visible = h;
            }

            if (rotacionar)
            {
                    //pnRotacionar.Top = this.Height - 100;
                   // pnRotacionar.Left = edZ.Left + 200;
                    pnRotacionar.Visible = true;

             /*   if (gerenciador.fRotacionarElementos == null)
                {
                    gerenciador.fRotacionarElementos = new FRotacaoElementos(gerenciador);
                    gerenciador.fRotacionarElementos.Show();
                }*/
            }
            else
            {
                if (gerenciador.fRotacionarElementos != null)
                   gerenciador.fRotacionarElementos.Close();

                    pnRotacionar.Visible = false;
            }

            // lbCoordenadas.Visible = h;
            btCoordRelativa.Visible = h;
            //  lbCoordenadas.BackColor = Color.FromArgb(100, Color.Gray);
            edX.Top = this.Height - 30;
            edY.Top = this.Height - 30;
            edZ.Top = this.Height - 30;

            edX.Left = this.Width / 2 - 120;
            edY.Left = this.Width / 2 - 60;
            edZ.Left = this.Width / 2;
            btConfirmaCoord.Left = edZ.Left + 55;
            btConfirmaCoord.Top = this.Height - 30;
                  
            if (copia)
            {
              pnRepeticoesCopia.Top = this.Height - 46;
              pnRepeticoesCopia.Left = edZ.Left + 115;
            }

            if (espelhar)
            {
                pnEspelhar.Top = this.Height - 50;
                pnEspelhar.Left = edZ.Left + 115;
            }

            if (mover)
            {
                pnMover.Top = this.Height - 50;
                pnMover.Left = edZ.Left + 115;
            }

            btConfirmaCoord.BackColor = System.Drawing.Color.FromArgb(100, System.Drawing.Color.Gray);

            // lbCoordenadas.Left = edY.Left - 30;
            // lbCoordenadas.Top = this.Height - 25;

            btCoordRelativa.Left = edX.Left - 30;
            btCoordRelativa.Top = this.Height - 30;
            if (Foco)
                edX.Focus();
        }

        public void AtualizarPixels()
        {
            for (i = 0; i < Linhas.Count; i++)
            {
                Linhas[i].pIni.px_x = pixelX(Linhas[i].pIni.x);
                Linhas[i].pIni.px_y = pixelY(Linhas[i].pIni.y);
                Linhas[i].pFin.px_x = pixelX(Linhas[i].pFin.x);
                Linhas[i].pFin.px_y = pixelY(Linhas[i].pFin.y);
            };

            for (int i = 0; i < Pontos.Count; i++)
            {
                Pontos[i].px_x = FPrincipal.pixelX(Pontos[i].x);
                Pontos[i].px_y = FPrincipal.pixelY(Pontos[i].y);
            };

            for (i = 0; i < TrechosVigas.Count; i++)
            {
                TrechosVigas[i].UpdateTodas(TrechosVigas[i]);
            }
        }

        public void CreateStandardLayers()
        {
            Layers.Add(new TLayer(Lay.Zero, true, false, new byte[3] { 255, 0, 0 }, 0, false));
            LayersById[Lay.Zero] = Layers[0]; // pontos

            Layers.Add(new TLayer(Lay.ElementosBasicos, true, false, new byte[3] { 255, 255, 255 }, 0, false));
            LayersById[Lay.ElementosBasicos] = Layers[1]; //linhas, circulos, arcos, polylines 

            Layers.Add(new TLayer(Lay.Vigas, true, false, new byte[3] { 255, 255, 255 }, 0, false));
            LayersById[Lay.Vigas] = Layers[2];

            Layers.Add(new TLayer(Lay.Pilares, true, false, new byte[3] { 80, 200, 120 }, 0, false));
            LayersById[Lay.Pilares] = Layers[3];

            Layers.Add(new TLayer(Lay.Lajes, true, false, new byte[3] { 211, 211, 211 }, 0, false));
            LayersById[Lay.Lajes] = Layers[4];

            Layers.Add(new TLayer(Lay.Barras, true, false, new byte[3] { 211, 211, 211 }, 0, false));
            LayersById[Lay.Barras] = Layers[4];

            Layers.Add(new TLayer(Lay.GrelhaLajes, false, true, new byte[3] { 0, 0, 255 }, 0, false));
            LayersById[Lay.GrelhaLajes] = Layers[5];

            Layers.Add(new TLayer(Lay.TextosVigas, true, false, new byte[3] { 0, 255, 10 }, 0, false));
            LayersById[Lay.TextosVigas] = Layers[6];

            Layers.Add(new TLayer(Lay.TextosLajes, true, false, new byte[3] { 255, 215, 0 }, 0, false));
            LayersById[Lay.TextosLajes] = Layers[7];

            Layers.Add(new TLayer(Lay.TextosPilares, true, false, new byte[3] { 0, 100, 10 }, 0, false));
            LayersById[Lay.TextosPilares] = Layers[8];
        }

      //  static readonly object bloqueador = new object();
        
        bool threadOk;

        public void CancelaInsercoes(bool MantemSelecionados = false)
        {
            // Thread trd = new Thread(new ThreadStart(this.PreencheObjetosNaViewport));
            //  trd.IsBackground = true;
            //  trd.Start();
            tipoComando = eTipoComando.selecionar;
            IdObjetoDesenho = string.Empty;
            editToolTipoSelecao = eEditToolTipoSelecao.nenhum;
            FerramentaEdicao = null;
            IdFerramentaEdicao = "";
            RefazEstadoLayers();
            pnDivBarras.Visible = false;
            Linhas.RemoveAll(o => o.auxiliar);
            LinhasProlongamento.Clear();
            CoordsSubdvisao.Clear();

            AtualizaListaSnap();

            clickCtrl = false;
            clickShift = false;
            //   edNumRepeticoes.Value = 1;
            if (ObjetoNovo != null)
            {
                string arg = string.Empty;

                List<TPonto> points = new List<TPonto>();

                ObjetoNovo.Cancel(false, ref arg, ref points);

                foreach (TPonto pt1 in points)
                    foreach (TPonto pt2 in Pontos)
                        if ((object)pt1 == (object)pt2)
                            if (pt1.incidencias.Count == 0)
                                pt2.Selecionado = true;

                Pontos.RemoveAll(obj => obj.Selecionado);

                ObjetoNovo = null;

            }

            if (GripAtual != null)
            {
                if (GripAtual.ObjetoDesenho.Tipo == Const.ID_TRECHOVIGA)
                {
                    (GripAtual.ObjetoDesenho as TTrechoViga).UpdateTodas((GripAtual.ObjetoDesenho as TTrechoViga));
                    (GripAtual.ObjetoDesenho as TTrechoViga).UpdateGrips();
                }
                GripAtual.ObjetoDesenho.Visivel = true;
                GripAtual = null;
            }

            if (!MantemSelecionados)
                SetaSelecionados(false, PavimentoAtual);

            Comando.Text = "";

            gerenciador.DigitarComando.Text = "";

            gerenciador.CancelaTudo();
            NovaCoordZ = false;
            NovaCoordY = false;
            NovaCoordX = false;
            aguardandoCota = false;
            pnDivBarras.Visible = false;

            this.ContextMenuStrip = menuPrincipal;

            HabilitaCoords(false);

            AtualizarDesenho(false,true);
            glControl.SwapBuffers();
        }


        /*    TestaSelecao();

           if (Selecting)
           {
               HandleMouseDownSelecao(e);
               Selecting = false;
           }
   */
        bool segundoCliqueSelecao, primeiroCliqueSelecao;
        private void MouseSelecao(System.Windows.Forms.MouseEventArgs e)
        {
         //   AtualizaShaders();
            CaixaSelecao = true;
            Ponto1.X = e.X;
            Ponto1.Y = e.Y;
            Ponto1_Selecao.x = Coord_PlanoSelecao.x;
            Ponto1_Selecao.y = Coord_PlanoSelecao.y;
            Ponto1_Selecao.z = Coord_PlanoSelecao.z;
            Ponto1_Coord.X = Posicao_Coord.X;
            Ponto1_Coord.Y = Posicao_Coord.Y;

            //   if (!ClicouObjeto())
            {
                if (!primeiroCliqueSelecao && !segundoCliqueSelecao)
                {
                 /*   SetaSelecionados(false, PavimentoAtual);
                    AtualizaShaders();
                    CaixaSelecao = true;

                    Ponto1_Selecao.x = Coord_PlanoSelecao.x;
                    Ponto1_Selecao.y = Coord_PlanoSelecao.y;
                    Ponto1_Selecao.z = Coord_PlanoSelecao.z;

                    primeiroCliqueSelecao = true;
                    segundoCliqueSelecao = false;

                    Ponto1.X = e.X;
                    Ponto1.Y = e.Y;

                    Ponto1_Coord.X = Posicao_Coord.X;
                    Ponto1_Coord.Y = Posicao_Coord.Y;*/
                }
                else
                if (!segundoCliqueSelecao && primeiroCliqueSelecao)
                {
                   /* CaixaSelecao = false;
                    primeiroCliqueSelecao = false;
                    segundoCliqueSelecao = false;
                    TestaSelecao(e);

                    if (IdObjetoDesenho != "")
                    {
                        if (IdObjetoDesenho == Const.ID_APOIO)
                        {
                            SetaSelecionados(false, -1, false);
                            foreach (TObjetoDesenho o in NosSelecionados)
                            {
                                MouseDrawing((o as TPonto).x, (o as TPonto).y, (o as TPonto).z);
                                (ObjetoNovo as TApoio).InsercaoIndividual = false;
                                MouseDrawing((o as TPonto).x, (o as TPonto).y, (o as TPonto).z);
                            }
                            IdObjetoDesenho = string.Empty;

                            if (gerenciador.fApoio != null)
                                gerenciador.fApoio.Salvar();
                        }
                    }*/
                };
            };
        }
        struct sPecas
        {
            public List<TBarraPortico> barras;

            public sPecas(TBarraPortico b)
            {
                barras = new List<TBarraPortico>();
                //    barras.Add(b);
            }
        }

        bool Barra_ja_inserida_nas_pecas(TBarraPortico b)
        {
            TBarraPortico barra_peca;
            foreach (sPecas p in pecas)
            {
                barra_peca = p.barras.Find(bb => bb.IDBarra == b.IDBarra);
                if (barra_peca != null)
                {
                    return true;
                }
            }
            return false;
        }

        List<sPecas> pecas;
        public void CriaDeformacaoSolida(TBarraPortico[] barrasPortico)
        {
            gerenciador.ChamaAguardar(this, "Processando. Aguarde...");
            try
            {
                List<TBarraPortico> peca = new List<TBarraPortico>(); // lista que grava a peça inteira composta por várias barras de portico
               
                Array.Copy(Estrutura.PorticoEspacial.barras, 1, barrasPortico, 0, Estrutura.PorticoEspacial.nBarras);

                pecas = new List<sPecas>();
                double FATOR = 0;
                List<TBarraPortico> barras_p = new List<TBarraPortico>();
                foreach (TBarraGenerica b in Estrutura.barras)
                {
                    Application.DoEvents();
                 //   pecas.Add(new sPecas(barrasPortico[1]));
                   
                    TBarraPortico[] bars = Array.FindAll(barrasPortico, o => o.barraOriginal.IDBarra == b.IDBarra);

                    foreach (TBarraPortico b1 in bars)
                    {
                        Application.DoEvents();
                        if (b1 == null)
                            continue;
                        if (b1.barraOriginal.IDBarra == b.IDBarra)
                        {
                         //   pecas[pecas.Count - 1].barras.Add(b1);
                            
                        //    for (int i = 0; i < b1.Dados.secao.poligonos.Count; i++)
                            {
                                //    b1.CoordsSecao_f_temp = new vec3[b1.Dados.secao.poligono.coords.Count()];
                                //  b1.CoordsSecao_i_temp = new vec3[b1.Dados.secao.poligono.coords.Count()];

                                // b1.coordssecao_i = new List<vec3[]>();
                                // b1.coordssecao_f = new List<vec3[]>();
                                
                                b1.coordssecao_i_temp = new List<vec3[]>();
                                b1.coordssecao_f_temp = new List<vec3[]>();
                                
                                b1.OrientaSecaoNoEspaco(ref FATOR, tipoCargaResultado, id_caso, id_combinacao);

                                for (i = 0; i < b1.Dados.secao.poligonos.Count(); i++)
                                {
                                    b1.coordssecao_i_temp.Add(new vec3[b1.Dados.secao.poligonos[i].coords.Count()]);
                                    b1.coordssecao_f_temp.Add(new vec3[b1.Dados.secao.poligonos[i].coords.Count()]);

                                    for (int k = 0; k < b1.Dados.secao.poligonos[i].coords.Count(); k++)
                                    {
                                        b1.coordssecao_i_temp[i][k] = new vec3(b1.coordssecao_i[i][k].x, b1.coordssecao_i[i][k].y, b1.coordssecao_i[i][k].z);
                                        b1.coordssecao_i_temp[i][k].pontoEmRaio = b1.coordssecao_i[i][k].pontoEmRaio;

                                        b1.coordssecao_f_temp[i][k] = new vec3(b1.coordssecao_f[i][k].x, b1.coordssecao_f[i][k].y, b1.coordssecao_f[i][k].z);
                                        b1.coordssecao_f_temp[i][k].pontoEmRaio = b1.coordssecao_f[i][k].pontoEmRaio;
                                    }
                                }
                            }
                        }
                    }
                }
                Application.DoEvents();
    //            AtualizaShaders();

                gerenciador.FechaAguardar();
            }
            catch (Exception ed)
            {
                MessageBox.Show("erro ao gerar deformação sólida: " + ed);
            }
        }

        public void CriaDeformacaoSolida_Tensoes(int filtro)
        {
            gerenciador.ChamaAguardar(this, "Renderizando. Aguarde...");
            try
            {
                bool MostrarEmSelecionados = (filtro == 1);

                TBarraPortico[] barrasPortico = new TBarraPortico[Estrutura.PorticoEspacial.nBarras];

                Array.Copy(Estrutura.PorticoEspacial.barras, 1, barrasPortico, 0, Estrutura.PorticoEspacial.nBarras);

                if (MostrarEmSelecionados)
                    barrasPortico = barrasPortico.Where(o=>o.barraOriginal.Selecionado).ToArray();

                double FATOR = 0;
                List<TBarraPortico> barras_p = new List<TBarraPortico>();

                Application.DoEvents();


                foreach (TBarraPortico b1 in barrasPortico)
                {
                    if (b1 == null)
                        continue;

         //           if ((MostrarEmSelecionados && b1.barraOriginal.Selecionado) || (!MostrarEmSelecionados))
                    {
                        b1.coordssecao_i_temp = new List<vec3[]>();
                        b1.coordssecao_f_temp = new List<vec3[]>();

                        b1.OrientaSecaoNoEspaco(ref FATOR, tipoCargaResultado, id_caso, id_combinacao);

                        for (i = 0; i < b1.Dados.secao.poligonos.Count(); i++)
                        {
                            b1.coordssecao_i_temp.Add(new vec3[b1.Dados.secao.poligonos[i].coords.Count()]);
                            b1.coordssecao_f_temp.Add(new vec3[b1.Dados.secao.poligonos[i].coords.Count()]);

                            for (int k = 0; k < b1.Dados.secao.poligonos[i].coords.Count(); k++)
                            {
                                b1.coordssecao_i_temp[i][k] = new vec3(b1.coordssecao_i[i][k].x, b1.coordssecao_i[i][k].y, b1.coordssecao_i[i][k].z);
                                b1.coordssecao_i_temp[i][k].pontoEmRaio = b1.coordssecao_i[i][k].pontoEmRaio;
                                b1.coordssecao_f_temp[i][k] = new vec3(b1.coordssecao_f[i][k].x, b1.coordssecao_f[i][k].y, b1.coordssecao_f[i][k].z);
                                b1.coordssecao_f_temp[i][k].pontoEmRaio = b1.coordssecao_f[i][k].pontoEmRaio;
                            }
                        }
                    }
                }

                Application.DoEvents();
                AtualizaShaders();

                gerenciador.FechaAguardar();
            }
            catch (Exception ed)
            {
                MessageBox.Show("erro ao gerar deformação sólida: " + ed);
            }
        }

        public void MultiMouseDraw()
        {
            if (IdObjetoDesenho == Const.ID_CARGA_LINEAR)
            {
                ChamaProgresso("Inserindo. Aguarde...", 0);
                ObjetosCopiados = new List<TObjetoDesenho>();
                ObjetosAdicionados = new List<TObjetoDesenho>();

                foreach (TObjetoDesenho o in ObjetosSelecionados)
                {
                    if (o.Tipo == Const.ID_BARRAGENERICA)
                    {

                        
                        //  MouseDrawing((o as TBarraGenerica).pIni.x, (o as TBarraGenerica).pIni.y, (o as TBarraGenerica).pIni.z);

                        //  (ObjetoNovo as TCargaLinear).InsercaoIndividual = false;
                        //  MouseDrawing((o as TBarraGenerica).pFin.x, (o as TBarraGenerica).pFin.y, (o as TBarraGenerica).pFin.z);
                      /* if (CargasLineares.Exists(c=>c.idBarra == (o as TBarraGenerica).IDBarra && c.Dados.valor == DadosCarga.valor))
                        {
                            continue;
                        }*/

                        List<TCargaLinear> cargasRepetidas = CargasLineares.FindAll(
                                    c => c.Dados.valor == DadosCarga.valor
                                && c.Dados.idCaso == DadosCarga.idCaso
                                && c.Dados.ProjecaoGlobal == DadosCarga.ProjecaoGlobal
                                && c.Dados.concentrada == false
                                && c.Dados.distribuida == true
                                && c.Dados.posicaoRelativa == DadosCarga.posicaoRelativa
                                && c.Dados.DirecaoProjecao == DadosCarga.DirecaoProjecao);


                        if (DadosCarga.concentrada)
                        {
                            double posicaoFinal = DadosCarga.posicaoRelativa ? (o as TBarraGenerica).comprimento * DadosCarga.d : DadosCarga.d;
                            if (posicaoFinal > (o as TBarraGenerica).comprimento)
                            {
                                MessageBox.Show("Com a distância de " + posicaoFinal.ToString("n2") + " m, a carga concentrada de " + DadosCarga.valor.ToString("n2") + " kN no elemento " + (o as TBarraGenerica).IDBarra + " não ficará posicionada no interior do elemento, portanto ela não poderá ser inserida.", "Erro", MessageBoxButtons.OK, MessageBoxIcon.Error);
                                continue;
                            }
                        }

                        if (DadosCarga.concentrada)
                        {
                            cargasRepetidas = CargasLineares.FindAll(
                                    c => c.Dados.valor == DadosCarga.valor
                                && c.Dados.idCaso == DadosCarga.idCaso
                                && c.Dados.ProjecaoGlobal == DadosCarga.ProjecaoGlobal
                                && c.Dados.concentrada == true
                                && c.Dados.distribuida == false
                                && c.Dados.d == DadosCarga.d
                                && c.Dados.posicaoRelativa == DadosCarga.posicaoRelativa
                                && c.Dados.DirecaoProjecao == DadosCarga.DirecaoProjecao);

                            if (cargasRepetidas.Exists(cr => cr.idBarra == (o as TBarraGenerica).IDBarra))
                            {
                                MessageBox.Show("Já existe a carga concentrada de " + cargasRepetidas[0].Dados.valor.ToString("n2") + " kN na mesma posição no elemento " + (o as TBarraGenerica).IDBarra + " para esse mesmo caso de carga!", "Erro", MessageBoxButtons.OK, MessageBoxIcon.Error);
                                return;
                            }
                        }
                        else
                        if (cargasRepetidas.Exists(cr => cr.idBarra == (o as TBarraGenerica).IDBarra))
                        {
                           MessageBox.Show("Já existe a carga distribuída de " + cargasRepetidas[0].Dados.valor.ToString("n2")+" kN no elemento "+ (o as TBarraGenerica).IDBarra + " para esse mesmo caso de carga!" , "Erro", MessageBoxButtons.OK, MessageBoxIcon.Error);
                           continue;
                        }

                        if ((o as TBarraGenerica).Dados.Tipo == 4)
                        {
                        //    MessageBox.Show("O carregamento foi desconsiderado no braço rígido!", "Atenção", MessageBoxButtons.OK, MessageBoxIcon.Stop);
                            continue;
                        }

                        TCargaLinear carganova = new TCargaLinear((o as TBarraGenerica).pIni, (o as TBarraGenerica).pFin, (TDadosCarga)DadosCarga.Clone(),
                                                                  (o as TBarraGenerica).Dados.anguloRotacao, (o as TBarraGenerica).IDBarra, (o as TBarraGenerica).layer);

                        Estrutura.cargaLinear.Add(carganova);
                        CargasLineares.Add(Estrutura.cargaLinear[Estrutura.cargaLinear.Count - 1]);
                        carganova.ID = Estrutura.cargaLinear.Max(cl => cl.ID) + 1;

                        Estrutura.cargaLinear[Estrutura.cargaLinear.Count - 1].CriaSetas(true, 0, 0, fatorCarga);
                        ObjetosAdicionados.Add(carganova);
/*
                        Estrutura.cargaLinear[Estrutura.cargaLinear.Count - 1].idBarra = (o as TBarraGenerica).IDBarra;
                        Estrutura.cargaLinear[Estrutura.cargaLinear.Count - 1].Dados = (TDadosCarga)DadosCarga.Clone();
                        Estrutura.cargaLinear[Estrutura.cargaLinear.Count - 1].anguloRotacao = (o as TBarraGenerica).Dados.anguloRotacao;
                        Estrutura.cargaLinear[Estrutura.cargaLinear.Count - 1].RotacionaDiagramaCarga(true, 0, 0, fatorCarga);*/
                    }
                }

                Alterou(true);
                Atualiza_pIni_pFin_das_Barras_e_Cargas();

                if (undoBuffer.CanCapture)
                {
                    undoBuffer.AdicionaComando(new ComandoAdicionar(ObjetosAdicionados, new List<ComandoMover>()));
                    ObjetosAdicionados.Clear();
                }

                FechaProgresso();
                MostrarCargasPeloCaso();
                gerenciador.NovosDadosDeCargaBarra("");
            }

            if (IdObjetoDesenho == Const.ID_CARGA_PONTUAL)
            {
                ChamaProgresso("Inserindo. Aguarde...", 0);
                ObjetosCopiados = new List<TObjetoDesenho>();
                ObjetosAdicionados = new List<TObjetoDesenho>();

                List<TPonto> pontos = new List<TPonto>();
                foreach (TObjetoDesenho o in ObjetosSelecionados)
                {
                    if (o.Tipo == Const.ID_PONTO)
                    {
                        TPonto pp = o as TPonto;
                        if (!pontos.Exists(p => (Geom.Iguais(p.x, pp.x) && Geom.Iguais(p.y, pp.y) && Geom.Iguais(p.z, pp.z))))
                          pontos.Add(pp);
                    }
                }

                foreach (TPonto o in pontos)
                {

                    List<TCargaPontual> cargasRepetidas = CargasPontuais.FindAll(
                        c => c.Dados.valor == DadosCarga.valor
                     && c.Dados.idCaso == DadosCarga.idCaso
                    && c.Dados.tipoForca == true
                    && c.Dados.DirecaoProjecao == DadosCarga.DirecaoProjecao
                    && Geom.Iguais(c.pIni.x, o.x)
                    && Geom.Iguais(c.pIni.y, o.y)
                    && Geom.Iguais(c.pIni.z, o.z));

                    if (cargasRepetidas.Count > 0)
                    {
                        MessageBox.Show("A carga pontual de " + cargasRepetidas[0].Dados.valor.ToString("n2") + " kN em (x: " + o.x.ToString("n2") +" y: "+ (o.y*-1).ToString("n2") + " z: " + (o.z*-1).ToString("n2") + ") \r já foi inserida para esse mesmo caso de carga!", "Erro", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        continue;
                    }

                    TCargaPontual carganova = new TCargaPontual( (TDadosCarga)DadosCarga.Clone(), (o as TPonto), Barras[0].layer);

                    if (Estrutura.cargaPontual.Count == 0)
                        carganova.ID = 1;
                    else
                        carganova.ID = Estrutura.cargaPontual.Max(cl => cl.ID) + 1;
                   
                    Estrutura.cargaPontual.Add(carganova);
                    CargasPontuais.Add(Estrutura.cargaPontual[Estrutura.cargaPontual.Count - 1]);

                   // Estrutura.cargaPontual[Estrutura.cargaPontual.Count - 1].CriaSetas(true, 0, 0, fatorCarga);
                    ObjetosAdicionados.Add(carganova);
                    

                   /* MouseDrawing((o as TPonto).x, (o as TPonto).y, (o as TPonto).z);
                   MouseDrawing((o as TPonto).x, (o as TPonto).y, (o as TPonto).z);

                   Estrutura.cargaPontual[Estrutura.cargaPontual.Count - 1].ponto = (TPonto)(o as TPonto).Clone();
                   Estrutura.cargaPontual[Estrutura.cargaPontual.Count - 1].Dados = (TDadosCarga)DadosCarga.Clone();*/
                }

                Alterou(true);
                Atualiza_pIni_pFin_das_Barras_e_Cargas();

                if (undoBuffer.CanCapture)
                {
                    undoBuffer.AdicionaComando(new ComandoAdicionar(ObjetosAdicionados, new List<ComandoMover>()));
                    ObjetosAdicionados.Clear();
                }

                FechaProgresso();
                MostrarCargasPeloCaso();
             //   MostraCargasNosPeloCaso();
                gerenciador.NovosDadosDeCargaNodal("");
            }

            Estrutura.AtualizaListaObjetos();
        }
        public UndoRedoBuffer undoBuffer = new UndoRedoBuffer();
        public List<ComandoMover> comandosMover;
        public void DeletaSelecionados(int pav, bool limpaSelecionadosNoFinal = true)
        {
            try
            {
                CancelaResultados();
                
                foreach (TBarraGenerica b in Estrutura.barras)
                {
                    if (b.Selecionado)
                    {
                        b.pIni.BarrasConectadas.Remove(b);
                        b.pFin.BarrasConectadas.Remove(b);

                        foreach (TCargaLinear o in Estrutura.cargaLinear)
                            if (b.IDBarra == o.idBarra)
                                o.Selecionado = true;

                        b.pIni.Selecionado = true;
                        b.pFin.Selecionado = true;

                        if (b.ids_barras_rigidas != null) 
                          for (int i = 0; i < b.ids_barras_rigidas.Count; i++)
                          {
                              if (Estrutura.barras.Exists(o => o.IDBarra == b.ids_barras_rigidas[i]))
                                  Estrutura.barras.Find(o => o.IDBarra == b.ids_barras_rigidas[i]).SetaSelecao(true, false);
                            }

                       /* if (b.id_barra_rigida_1 != -1)
                            if (Estrutura.barras.Exists(o => o.IDBarra == b.id_barra_rigida_1))
                              Estrutura.barras.Find(o => o.IDBarra == b.id_barra_rigida_1).Selecionado = true;

                        if (b.id_barra_rigida_2 != -1)
                            if (Estrutura.barras.Exists(o => o.IDBarra == b.id_barra_rigida_2))
                                Estrutura.barras.Find(o => o.IDBarra == b.id_barra_rigida_2).Selecionado = true;*/
                    }
                }

                foreach (TPonto p in Estrutura.nos)
                {
                    if (p.Selecionado)
                    {
                      //  foreach (TCargaPontual b in Estrutura.cargaPontual)
                      //      if ((Geom.Iguais(b.pIni.x, p.x) && Geom.Iguais(b.pIni.y, p.y) && Geom.Iguais(b.pIni.z, p.z)))
                      //          b.Selecionado = true;

                        foreach (TBarraGenerica b in Estrutura.barras)
                            if ((Object)b.pIni == (Object)p || (Object)b.pFin == (Object)p)
                            {
                                b.SetaSelecao(true, true);

                                b.pIni.Selecionado = true;
                                b.pFin.Selecionado = true;
                            }
                    }
                }


                Estrutura.vigas.RemoveAll(obj => obj.Selecionado);
                Estrutura.pilares.RemoveAll(obj => obj.Selecionado);
                Estrutura.lajes.RemoveAll(obj => obj.Selecionado);
                Estrutura.barras.RemoveAll(obj => obj.Selecionado);
                Estrutura.cargaLinear.RemoveAll(obj => obj.Selecionado);

                /*  bool noConectado = false;
                  foreach (TPonto p in Estrutura.nos)
                  {
                      noConectado = false;
                    /*  foreach (TBarraGenerica b in Estrutura.barras)
                      {
                          if ((Geom.Iguais(b.pIni.x, p.x) && Geom.Iguais(b.pIni.y, p.y) && Geom.Iguais(b.pIni.z, p.z)) ||
                              (Geom.Iguais(b.pFin.x, p.x) && Geom.Iguais(b.pFin.y, p.y) && Geom.Iguais(b.pFin.z, p.z)))
                          {
                              noConectado = true;
                              break;
                          }
                      }*/

                //if (noConectado)
                //  continue;

                /*    foreach (TApoio a in Estrutura.apoios)
                    {
                        if (Geom.Iguais(a.pIni.x, p.x) && Geom.Iguais(a.pIni.y, p.y) && Geom.Iguais(a.pIni.z, p.z))
                        {
                            noConectado = true;
                            break;
                        }
                    }

                    if (noConectado)
                        continue;

                    foreach (TCargaPontual a in Estrutura.cargaPontual)
                      if (Geom.Iguais(a.pIni.x, p.x) && Geom.Iguais(a.pIni.y, p.y) && Geom.Iguais(a.pIni.z, p.z))
                         a.Selecionado = true;

                    p.Selecionado = true;
                }*/

                /*foreach (TPonto p in Estrutura.nos)
                {
                    if (p.Selecionado)
                    {
                       /* foreach (TBarraGenerica b in Estrutura.barras)
                        {
                            if ((Geom.Iguais(b.pIni.x, p.x) && Geom.Iguais(b.pIni.y, p.y) && Geom.Iguais(b.pIni.z, p.z)) ||
                                (Geom.Iguais(b.pFin.x, p.x) && Geom.Iguais(b.pFin.y, p.y) && Geom.Iguais(b.pFin.z, p.z)))
                                b.SetaSelecao(true, true);
                        }*/

                /*  foreach (TApoio a in Estrutura.apoios)
                  {
                      if (Geom.Iguais(a.pIni.x, p.x) && Geom.Iguais(a.pIni.y, p.y) && Geom.Iguais(a.pIni.z, p.z))
                          a.SetaSelecao(true, true);
                  }*/
                //  }
                // }

                //verifica se tem carga pontual em algum nó selecionado para deletar
                List<TPonto> nos_selecionados_para_deletar = Estrutura.nos.FindAll(o=>o.Selecionado);
                foreach (TPonto p in nos_selecionados_para_deletar)
                {
                  if (Estrutura.nos.FindAll(o => Geom.Iguais(p.x, o.x) && Geom.Iguais(p.y, o.y) && Geom.Iguais(p.z, o.z) && p.Snap == false).Count() > 1)
                        continue;

                    foreach (TCargaPontual a in Estrutura.cargaPontual)
                        if (Geom.Iguais(a.pIni.x, p.x) && Geom.Iguais(a.pIni.y, p.y) && Geom.Iguais(a.pIni.z, p.z))
                            a.Selecionado = true;
                }

                //ultima verificação de carga pontual sobrando...avulsa no espaço
                foreach (TCargaPontual a in Estrutura.cargaPontual)
                    if (Estrutura.nos.FindAll(o => Geom.Iguais(a.pIni.x, o.x) && Geom.Iguais(a.pIni.y, o.y) && Geom.Iguais(a.pIni.z, o.z) && o.Selecionado).Count() > 1)
                        a.Selecionado = true;


                foreach (TPonto pt in Pontos)
                    foreach (TLinha lin in Linhas)
                        if ((object)lin.pIni == (object)pt || (object)lin.pFin == (object)pt || (object)lin.pMedio == (object)pt)
                            pt.Selecionado = false;

                Pontos.RemoveAll(obj => obj.Selecionado);

                Estrutura.apoios.RemoveAll(obj => obj.Selecionado);
                Estrutura.cargaPontual.RemoveAll(obj => obj.Selecionado);
                Estrutura.nos.RemoveAll(obj => obj.Selecionado);
                Arcos.RemoveAll(obj => obj.Selecionado);
                Linhas.RemoveAll(obj => obj.Selecionado);
                Circulos.RemoveAll(obj => obj.Selecionado);
                TrechosVigas.RemoveAll(obj => obj.Selecionado);
                Barras.RemoveAll(obj => obj.Selecionado);
                Pilares.RemoveAll(obj => obj.Selecionado);
                Lajes.RemoveAll(obj => obj.Selecionado);
                CargasPontuais.RemoveAll(obj => obj.Selecionado);
                CargasLineares.RemoveAll(obj => obj.Selecionado);
                Textos.RemoveAll(obj => obj.Selecionado);

                Atualiza_Segmentos_e_Snap();
                MostrarCargasPeloCaso();

                if (!AbrindoArquivo)
                  Alterou(true);
                
               
                  ObjetosSelecionados.Clear();
                
                Estrutura.AtualizaListaObjetos();
            }
            catch (Exception e)
            {
                MessageBox.Show("erro deletaselecionados : " + e.Message);
            }
        }

        public bool ObjetoJaEstaEstaNaListaSelecionado(TObjetoDesenho obj)
        {
            foreach (TObjetoDesenho ob in ObjetosSelecionados)
                if ((object)ob == (object)obj) return true;
            return false;
        }

        TObjetoDesenho ObjetoSelecionado;

        private bool ClicouObjeto()
        {
            //  int count = 0;
           // if (CaixaSelecao)
            //    return false;


            List<TObjetoDesenho> objs = new List<TObjetoDesenho>();
            /* if (ehGrip(ref od))
             {
                 GripAtual = od as TGrip;
                 GripAtual.uxi = mousepoint.x;
                 GripAtual.uyi = mousepoint.y;
                 GripAtual.movendo = true;

                 return true;
             }
             else*/
            
            if (!clickShift)
                SetaSelecionados(false, -1);

            //  Estrutura.AtualizaListaObjetos();

            /*  foreach (TObjetoDesenho objeto in Estrutura.objetos)
              {
                  if (ObjetosSelecionados.Count > 1)
                  if (objeto.Visivel && objeto.Selecionado)
                  { 
                     if (!objeto.Selecionar(mouseX, mouseY, (float)Posicao_Coord.X, (float)Posicao_Coord.Y))
                     {
                         objeto.SetaSelecao(false,false);
                     }
                  }
              }*/

            view = camera.viewMatrix;
            proj = camera.Projecao;
            View_x_Proj = RMath.Multiply(view, proj);

            bool SelecionaAntes = false;
            ObjetoSelecionado = null;
            // o teste do nó tem que vir antes, pois é objeto menor, e o sistema pode achar que voce clicou num apoio por exemplo
            bool clicouNo = false;
            if (gerenciador.SelecaoNos.Checked)
            {
                List<double> nos_z = new List<double>();
                if (ModelagemEmPlano)
                {
                    TPonto pontoSelecionado = new TPonto(0);
                    double min_z = 99999;
                    foreach (TPonto objeto in Estrutura.nos)
                    {
                        if (objeto.Visivel)
                        {
                            interx = objeto.x;
                            intery = objeto.y;
                            interz = objeto.z;
                            if (objeto.Selecionar(mouseX, mouseY, (float)Posicao_Coord.X, (float)Posicao_Coord.Y))
                            {
                                objeto.Selecionado = false;

                                pixel1(ref interx, ref intery, ref interz, ref z_pixel);
                                if ((z_pixel < min_z) || nos_z.Contains(z_pixel))
                                {
                                    nos_z.Add(z_pixel);
                                    clicouNo = true;
                                    min_z = z_pixel;
                                    ObjetoSelecionado = objeto;
                                    pontoSelecionado = objeto;
                                }
                            }
                        }
                    }

                    if (ObjetoSelecionado != null)
                    {
                        ObjetoSelecionado.Selecionado = true;
                        pontoSelecionado.Selecionado = true;
                        if (!ObjetosSelecionados.Exists(o => (object)o == (object)ObjetoSelecionado))
                        {
                            clicouNo = true;
                            AtualizaShaders(false);
                            DesenhaObjetos();
                            glControl.SwapBuffers();

                            ObjetosSelecionados.Add(ObjetoSelecionado);
                        }
                    }
                }
                else
                {
                    foreach (TPonto objeto in Estrutura.nos)
                    {
                        if (objeto.Visivel && objeto.habilitado)
                        {
                            SelecionaAntes = objeto.Selecionado;

                            if (SelecionaAntes && clickShift && objeto.Selecionar(mouseX, mouseY, (float)Posicao_Coord.X, (float)Posicao_Coord.Y))
                            {
                                objeto.SetaSelecao(false, false);
                                if (ObjetosSelecionados.Exists(o => (object)o == (object)objeto))
                                    ObjetosSelecionados.Remove(objeto);

                                DesenhaObjetos();
                                glControl.SwapBuffers();
                                AtualizaShaders(false);
                            }
                            else
                            if (objeto.Selecionar(mouseX, mouseY, (float)Posicao_Coord.X, (float)Posicao_Coord.Y))
                            {
                                ObjetoSelecionado = objeto;

                                if (!ObjetosSelecionados.Exists(o => (object)o == (object)objeto))
                                {
                                    clicouNo = true;
                                    AtualizaShaders(false);
                                    ObjetosSelecionados.Add(objeto);
                                    DesenhaObjetos();
                                    glControl.SwapBuffers();

                                 //   return true;
                                }
                            }
                        }
                    }
                }
            }

            if (clicouNo)
            {
            //    Estrutura.nos.ForEach(o => o.SetaSelecao(false, false));
            //    ObjetosSelecionados.Add(ObjetoSelecionado);
              //  ObjetoSelecionado.SetaSelecao(true, true);
                AtualizaShaders(false);
                return true;
            }

            double dist_zNear_objeto = 999999;
            int houveIntersecao = 0;
            bool objeto_selecionado = false;
            trianguloRef = null;

          //  if (!Unifilar)
            {
                if (CameraOrto)
                {
                    IntersecaoAABB(Unifilar);
                    for (i = 0; i < triangulos_temp.Count(); i++)
                    {
                        if (IntersecaoEmObjeto_Ortografica(ref triangulos_temp[i], ref Coord_TrianguloSelecao) != -1)
                        {
                            if ((triangulos_temp[i].idBarra > 0) || (triangulos_temp[i].idApoio > 0))
                            {
                                distancia_raio_triangulo = (raioZoom.p0 - Coord_TrianguloSelecao).Magnitude();

                                if (distancia_raio_triangulo < dist_zNear_objeto)
                                {
                                    trianguloRef = triangulos_temp[i];
                                    dist_zNear_objeto = distancia_raio_triangulo;
                                 //   break;
                                }
                            }
                        }
                    }
                }
                else
                {                   
                     VP = RMath.Multiply(camera.viewMatrix, camera.Projecao_zNear_zero);
                    
                     if (Unifilar)
                        IntersecaoAABB_Unifilar(Unifilar);
                     else
                        IntersecaoAABB(Unifilar);
                 
                    for (i = 0; i < triangulos_temp.Count(); i++)
                    {
                        if (IntersecaoEmObjeto_Perspectiva(ref triangulos_temp[i], ref Coord_TrianguloSelecao, ref mouseX, ref mouseY) != -1)
                        {
                            if ((triangulos_temp[i].idBarra > 0) || (triangulos_temp[i].idApoio > 0))
                            {
                                distancia_raio_triangulo = (raioZoom.p0 - Coord_TrianguloSelecao).Magnitude();

                                if (distancia_raio_triangulo < dist_zNear_objeto)
                                {
                                    trianguloRef = triangulos_temp[i];
                                    dist_zNear_objeto = distancia_raio_triangulo;
                                    //if (!clickShift)
                                    //  break;
                                }
                            }
                        }
                    }                   
                }
            }

            TObjetoDesenho obj_triangulo_selecao = null;
            if (trianguloRef != null)
            {              
                if (trianguloRef.idBarra > 0)
                    obj_triangulo_selecao = Estrutura.barras.Find(b => b.IDBarra == trianguloRef.idBarra);
                else
                if (trianguloRef.idApoio > 0)
                    obj_triangulo_selecao = Estrutura.apoios.Find(b => b.IDApoio == trianguloRef.idApoio);
               
                if (obj_triangulo_selecao.Visivel)
                {
                    SelecionaAntes = obj_triangulo_selecao.Selecionado;

                    if (SelecionaAntes && clickShift)
                    {
                        obj_triangulo_selecao.SetaSelecao(false, false);
                        if (ObjetosSelecionados.Exists(o => (object)o == (object)obj_triangulo_selecao))
                            ObjetosSelecionados.Remove(obj_triangulo_selecao);

                        DesenhaObjetos();
                        glControl.SwapBuffers();
                        AtualizaShaders(false);
                        return true;
                    }
                    else
                    {
                        if (obj_triangulo_selecao.Tipo == Const.ID_BARRAGENERICA && !gerenciador.SelecaoBarras.Checked)
                        {
                            obj_triangulo_selecao.SetaSelecao(false, false);
                            return false;
                        }
                        else
                        if (obj_triangulo_selecao.Tipo == Const.ID_APOIO && !gerenciador.SelecaoApoios.Checked)
                        {
                            obj_triangulo_selecao.SetaSelecao(false, false);
                            return false;
                        }

                        if (IdFerramentaEdicao == Const.ID_COPIAR_ELEMENTOS || IdFerramentaEdicao == Const.ID_MOVER_ELEMENTOS)
                        {
                            if (obj_triangulo_selecao.NaoPermiteMoverOuCopiar)
                            {
                                obj_triangulo_selecao.SetaSelecao(false, false);
                                return false;
                            }
                        }

                        ObjetoSelecionado = obj_triangulo_selecao;

                        if (!ObjetosSelecionados.Exists(o => (object)o == (object)obj_triangulo_selecao))
                          objs.Add(obj_triangulo_selecao);

                       // ActiveLayer = obj_triangulo_selecao.Layer;

                      //  AtualizaDisplayList();
                    }
                }
            }
            else
            if (trianguloRef == null)
            {
                foreach (TObjetoDesenho objeto in Estrutura.objetos)
                {
                    if (objeto.Visivel)
                    {
                        SelecionaAntes     = objeto.Selecionado;
                        objeto_selecionado = objeto.Selecionar(mouseX, mouseY, (float)Posicao_Coord.X, (float)Posicao_Coord.Y);

                        if (SelecionaAntes && clickShift && objeto_selecionado)
                        {
                            objeto.SetaSelecao(false, false);
                            if (ObjetosSelecionados.Exists(o => (object)o == (object)objeto))
                                ObjetosSelecionados.Remove(objeto);

                            DesenhaObjetos();
                            glControl.SwapBuffers();
                            AtualizaShaders(false);
                            return true;
                        }
                        else
                        if (objeto.Selecionar(mouseX, mouseY, (float)Posicao_Coord.X, (float)Posicao_Coord.Y))
                        {
                            if (objeto.Tipo == Const.ID_GRIP)
                            {
                                if ((objeto as TGrip).ObjetoDesenho.Tipo == Const.ID_LAJE)
                                {
                                    return false;

                                }
                                else
                                    GripAtual = objeto as TGrip;
                            }

                            if (objeto.Tipo == Const.ID_BARRAGENERICA && !gerenciador.SelecaoBarras.Checked)
                            {
                                objeto.SetaSelecao(false, false);
                                return false;
                            }

                            if (objeto.Tipo == Const.ID_APOIO && !gerenciador.SelecaoApoios.Checked)
                            {
                                objeto.SetaSelecao(false, false);
                                return false;
                            }

                            if (IdFerramentaEdicao == Const.ID_COPIAR_ELEMENTOS || IdFerramentaEdicao == Const.ID_MOVER_ELEMENTOS)
                            {
                                if (objeto.NaoPermiteMoverOuCopiar)
                                {
                                    objeto.SetaSelecao(false, false);
                                    return false;
                                }
                            }

                            ObjetoSelecionado = objeto;

                            if (!ObjetosSelecionados.Exists(o => (object)o == (object)objeto))
                                objs.Add(objeto);

                            ActiveLayer = objeto.Layer;

                          //  AtualizaDisplayList();
                        }
                    }
                }
            }

            for (int i = 0; i < objs.Count; i++)
            {
                if (objs[i].Tipo == Const.ID_BARRAGENERICA)
                {
                    TBarraGenerica bar = (TBarraGenerica)objs[i];
                    if (bar.ids_barras_rigidas!=null)
                    {
                        for (int j = 0; j < bar.ids_barras_rigidas.Count; j++)
                        {
                            if (Estrutura.barras.Exists(o => o.IDBarra == bar.ids_barras_rigidas[j]))
                            {
                                TBarraGenerica br = Estrutura.barras.Find(o => o.IDBarra == bar.ids_barras_rigidas[j]);
                                br.SetaSelecao(true, false);
                                ObjetosSelecionados.Add(br);
                            }
                        }
                    }
                }
            }

            TObjetoDesenho od = null;

            if (trianguloRef == null)
            {
                double soma_z = 99999999;
                foreach (TObjetoDesenho o in objs)
                {
                    o.SetaSelecao(false, false);
                    if (o.Soma_Z < soma_z)
                    {
                        soma_z = o.Soma_Z;
                        od = o;
                    }
                }
                if (od != null)
                {
                    od.SetaSelecao(true, true);
                    ObjetosSelecionados.Add(od);
                    AtualizaShaders(false);
                    DesenhaObjetos();
                    glControl.SwapBuffers();
                    return true;
                }
            }
            else
            {
                obj_triangulo_selecao.SetaSelecao(true, true, true);
                ObjetosSelecionados.Add(obj_triangulo_selecao);
                AtualizaShaders(false);
                DesenhaObjetos();
                glControl.SwapBuffers();
                return true;
            }

           // AtualizaShaders();

            return false;
        }
      //  public float x_rot_angle, y_rot_angle;
     //   public double x_trans, y_trans, z_trans;
        private void Desenho_Load(object sender, EventArgs e)
        {
            fatorZoom = 1.12;

            ponto_zero = new double[2];
            ponto_zero[0] = 0;
            ponto_zero[1] = this.ClientSize.Height;

            precisaoPixel = 0.8f;

            //  this.DoubleBuffered = true;
            //  sb  = new SolidBrush(c1);
            //     sb2 = new SolidBrush(c2);

            camera.x_rot_angle = 0;
            camera.y_rot_angle = 0;
            camera.z_trans = 20;

            camera.x_ang_rad = (camera.x_rot_angle * Math.PI) / 180;
            camera.z_ang_rad = (camera.y_rot_angle * Math.PI) / 180;

            /*camera.x_rot_angle = x_rot_angle;
            camera.y_rot_angle = y_rot_angle;
            camera.x_ang_rad = x_ang_rad;
            camera.z_ang_rad = z_ang_rad;
            camera.z_trans = 20;*/

            camera.ViewDirty = true;

            NovaEstrutura();
            AtualizaConfiguracoes3D();
            Comando.Text = string.Empty;

            //  glControl.Invalidate();
            glControl.SwapBuffers();

            if (gerenciador.ConfiguracaoPrograma.AbrirUltimoProjeto && gerenciador.Inicializando)
            {
                if (File.Exists(gerenciador.ArquivosRecentes[gerenciador.ArquivosRecentes.Count - 1]))
                {
                    gerenciador.Abrir(gerenciador.ArquivosRecentes[gerenciador.ArquivosRecentes.Count - 1], true);
                }
                else
                    gerenciador.ProjetoNaoEcontrado = true;

            }
        }
        public void PreparaNovoProjeto()
        {
            LimpaTela();
            NovaEstrutura();
            AtualizaConfiguracoes3D();
            AtualizaConfiguracaoDiagramas();
            CancelaInsercoes();
            Secoes = new List<TSecao>();
            Linhas = new List<TLinha>();
            TPonto pt = new TPonto(0, 0, 0);
            CriaLinhasProjecaoTemporarias(ref pt);

            CasosCarga = new List<TCasosCarga>
            {
                new TCasosCarga(1, "PP", "Peso próprio", TipoCasoCarga.Permanente, System.Drawing.Color.Green, SimNao.Sim)
            };
            
            gerenciador.AtualizaComboCarga();
            
            gerenciador.cbCasoCarga.SelectedIndex = 0;

            CargasPontuais = new List<TCargaPontual>();
            CargasLineares = new List<TCargaLinear>();
            CargasBarrasAtuais = new List<TCargaLinear>();
            CargasNosAtuais = new List<TCargaPontual>();
            CameraOrto = true;
            SetupCamera();
            camera.x_trans = 0;
            camera.y_trans = 0;

            camera.x_rot_angle = 135;
            camera.y_rot_angle = 225;

            camera.x_ang_rad = (camera.x_rot_angle * Math.PI) / 180;
            camera.z_ang_rad = (camera.y_rot_angle * Math.PI) / 180;

         /*  camera.x_rot_angle = x_rot_angle;
            camera.y_rot_angle = y_rot_angle;
            camera.x_ang_rad = x_ang_rad;
            camera.z_ang_rad = z_ang_rad;
            camera.x_trans = x_trans;
            camera.y_trans = y_trans;*/
            camera.ViewDirty = true;

            AtualizarDesenho();
            glControl.SwapBuffers();
        }

        public void NovaEstrutura()
        {
            Estrutura = new TEstrutura(ref gerenciador);
        }

        public bool Datum, EixosLocais;
        public bool snap_PontoFinal, snap_PontoMeio, snap_Intersecao, snap_Perpendicular;
        System.Drawing.Color CorBaixo = System.Drawing.Color.White, CorCima = System.Drawing.Color.White;
        public void ApagaElementosDesnecessarios()
        {
            foreach (TCargaLinear o in Estrutura.cargaLinear)
              if (!Estrutura.barras.Exists(b => b.IDBarra == o.idBarra))
                 o.Selecionado = true;

            Estrutura.cargaLinear.RemoveAll(obj => obj.Selecionado);
            if (CargasLineares.Count > 0)
                if (CargasLineares.Exists(obj => obj.Selecionado))
                    CargasLineares.RemoveAll(obj => obj.Selecionado);
        }

        void AtualizaUltimaBarra()
        {
            try
            {
             //   Estrutura.Rgb_Barras[0] = Estrutura.layers[5].Rgb[0];
            //    Estrutura.Rgb_Barras[1] = Estrutura.layers[5].Rgb[1];
        //        Estrutura.Rgb_Barras[2] = Estrutura.layers[5].Rgb[2];

                if (gerenciador.ConfiguracoesPGi.F3DOpcoesVisualizacao.CorPorSecao)
                {
                    TSecao sec = Secoes.Find(o=>o.id == Barras[Barras.Count-1].Dados.secao.id);
                    Barras[Barras.Count-1].Rgb[0] = sec.Rgb[0];
                    Barras[Barras.Count-1].Rgb[1] = sec.Rgb[1];
                    Barras[Barras.Count-1].Rgb[2] = sec.Rgb[2];
                }
                else
                if (gerenciador.ConfiguracoesPGi.F3DOpcoesVisualizacao.CorPorTipoDeElemento)
                {
                    foreach (TBarraGenerica o in Barras)
                    {
                        Barras[Barras.Count-1].Rgb[0] = Estrutura.Rgb_Barras[0];
                        Barras[Barras.Count-1].Rgb[1] = Estrutura.Rgb_Barras[1];
                        Barras[Barras.Count-1].Rgb[2] = Estrutura.Rgb_Barras[2];
                    }
                }

                Barras[Barras.Count-1].pMedioBarra = new vec3((Barras[Barras.Count - 1].pIni.x + Barras[Barras.Count - 1].pFin.x) / 2, (Barras[Barras.Count - 1].pIni.y + Barras[Barras.Count - 1].pFin.y) / 2, (Barras[Barras.Count - 1].pIni.z + Barras[Barras.Count - 1].pFin.z) / 2);
                Barras[Barras.Count-1].OrientaSecaoNoEspaco();
                Barras[Barras.Count-1].CriaPesoProprio();
                Barras[Barras.Count - 1].CriarEixosLocais(tamArticulacoes);

                //   AtualizaCargas();
                AtualizaShaders(!Unifilar);

              //  if (MostrarNos)
               //   AtualizaDisplayList_Nos();
             //   DesenhaObjetos();
               // glControl.Focus();
               // glControl.SwapBuffers();
            }
            catch (Exception e)
            {
                MessageBox.Show(e.Message);
            }
        }

        public void AtualizaCoresBarras()
        {
           // Estrutura.Rgb_Barras[0] = Estrutura.layers[5].Rgb[0];
           // Estrutura.Rgb_Barras[1] = Estrutura.layers[5].Rgb[1];
           // Estrutura.Rgb_Barras[2] = Estrutura.layers[5].Rgb[2];

            if (gerenciador.ConfiguracoesPGi.F3DOpcoesVisualizacao.CorPorSecao)
            {
                foreach (TSecao s in Secoes)
                {
                    foreach (TBarraGenerica o in Barras)
                    {
                        if (o.Dados.secao.id == s.id)
                        {
                            o.Rgb[0] = s.Rgb[0];
                            o.Rgb[1] = s.Rgb[1];
                            o.Rgb[2] = s.Rgb[2];
                        }
                    }
                }
            }
            else
            if (gerenciador.ConfiguracoesPGi.F3DOpcoesVisualizacao.CorPorTipoDeElemento)
            {
                foreach (TBarraGenerica o in Barras)
                {
                    o.Rgb[0] = Estrutura.Rgb_Barras[0];
                    o.Rgb[1] = Estrutura.Rgb_Barras[1];
                    o.Rgb[2] = Estrutura.Rgb_Barras[2];
                }
            }
        }
        public void Atualiza_Orientacao3D_PesoProprio_Barras()
        {
            foreach (TBarraGenerica o in Barras)
            {
                o.pMedioBarra = new vec3((o.pIni.x + o.pFin.x) / 2, (o.pIni.y + o.pFin.y) / 2, (o.pIni.z + o.pFin.z) / 2);
                o.OrientaSecaoNoEspaco();
                o.CriaPesoProprio();
                o.CriarEixosLocais(tamArticulacoes);
            }
        }

        public void AtualizaBarras(bool atuShaders = true)
        {
            try
            {
                AtualizaCoresBarras();

                Atualiza_Orientacao3D_PesoProprio_Barras();

                AtualizaCargas(atuShaders);
                AtualizarDesenho(true, atuShaders);
                glControl.SwapBuffers();
            }
            catch (Exception e)
            {
                MessageBox.Show(e.Message);
            }
        }
        bool LinhaContornoDeformacao;
        public string casas_decimais_resultado;
       System.Drawing.Color corDescricaoElementos, corValorDeformacao, corValorDiagrama;
        public void AtualizaConfiguracoes3D(bool atualizabarras = true, bool atuShaders = true)
        {
            GL.ClearColor(gerenciador.ConfiguracoesPGi.F3DOpcoesVisualizacao.CorFundoCima);
            Arestas = gerenciador.ConfiguracoesPGi.F3DOpcoesVisualizacao.Arestas;
            ArestasConfObjeto = gerenciador.ConfiguracoesPGi.F3DOpcoesVisualizacao.ArestasConformeObjetos;
            MostrarNos = gerenciador.ConfiguracoesPGi.F3DOpcoesVisualizacao.Nos;
            corDescricaoElementos = gerenciador.ConfiguracoesPGi.F3DOpcoesVisualizacao.CorDescElementos;
            MostrarDescricaoElementos = gerenciador.ConfiguracoesPGi.F3DOpcoesVisualizacao.MostrarDescricaoElementos;
            MostrarNumeroElementos = gerenciador.ConfiguracoesPGi.F3DOpcoesVisualizacao.MostrarNumeroElementos;

            FundoGradiente = gerenciador.ConfiguracoesPGi.F3DOpcoesVisualizacao.UsarPlanoFundoGradiente;
            TamanhoNo = gerenciador.ConfiguracoesPGi.F3DOpcoesVisualizacao.tamanhoNo / 2/100;
            Transparencia = gerenciador.ConfiguracoesPGi.F3DOpcoesVisualizacao.Transparencia;
            Datum = gerenciador.ConfiguracoesPGi.F3DOpcoesVisualizacao.EixosCentrais;
            EixosLocais = gerenciador.ConfiguracoesPGi.F3DOpcoesVisualizacao.MostrarEixosLocais;

            tamApoios = (double)gerenciador.ConfiguracoesPGi.F3DOpcoesVisualizacao.TamApoios/100/2;
            tamArticulacoes = (double)gerenciador.ConfiguracoesPGi.F3DOpcoesVisualizacao.TamArt / 100 / 2;
           
        /*    if (Estrutura.barras.Exists(o=> o.Dados.Articulacao_my == 1 || o.Dados.Articulacao_my == 2 || o.Dados.Articulacao_my == 3 ||
                                  o.Dados.Articulacao_mz == 1 || o.Dados.Articulacao_mz == 2 || o.Dados.Articulacao_mz == 3))
            {
                foreach (TBarraGenerica o in Estrutura.barras)
                {
                    o.CriarDesenhoArticulacoes(tamArticulacoes);
                }
            }*/

            CorBaixo = gerenciador.ConfiguracoesPGi.F3DOpcoesVisualizacao.CorFundoBaixo;
            CorCima = gerenciador.ConfiguracoesPGi.F3DOpcoesVisualizacao.CorFundoCima;
            LinhaContornoDeformacao = gerenciador.ConfiguracoesPGi.PorticoOpcoesVisualizacao.MostrarLinhasContorno;
            CorCota = CorCota = gerenciador.ConfiguracoesPGi.CorCota;
           
            Unifilar = gerenciador.ConfiguracoesPGi.F3DOpcoesVisualizacao.Unifilar;

            if (atualizabarras) 
                AtualizaBarras(atuShaders);

            if (gerenciador.ConfiguracoesPGi.CfgProjeto.unidadesProjeto.res_un_comprimento == und.mm) //m -> mm
                conversaoComprimentoResultado = 1000; // metros para mm
            if (gerenciador.ConfiguracoesPGi.CfgProjeto.unidadesProjeto.res_un_comprimento == und.cm) //m-> cm
                conversaoComprimentoResultado = 100;
            if (gerenciador.ConfiguracoesPGi.CfgProjeto.unidadesProjeto.res_un_comprimento == und.m) //m
                conversaoComprimentoResultado = 1;

            if (gerenciador.ConfiguracoesPGi.CfgProjeto.unidadesProjeto.res_un_deformacao == und.mm) //m -> mm
                conversaoComprimento_Def = 1000; // metros para mm
            if (gerenciador.ConfiguracoesPGi.CfgProjeto.unidadesProjeto.res_un_deformacao == und.cm) //m-> cm
                conversaoComprimento_Def = 100;
            if (gerenciador.ConfiguracoesPGi.CfgProjeto.unidadesProjeto.res_un_deformacao == und.m) //m
                conversaoComprimento_Def = 1;


            if (gerenciador.ConfiguracoesPGi.CfgProjeto.unidadesProjeto.res_un_forca == und.kgf) 
                conversaoForcaResultado = 100; //kn para kg
            if (gerenciador.ConfiguracoesPGi.CfgProjeto.unidadesProjeto.res_un_forca == und.kN) 
                conversaoForcaResultado = 1; //kn para kn
            if (gerenciador.ConfiguracoesPGi.CfgProjeto.unidadesProjeto.res_un_forca == und.tonf)
                conversaoForcaResultado = 0.1; //kn para tf
            casas_decimais_resultado = "n"+gerenciador.ConfiguracoesPGi.CfgProjeto.unidadesProjeto.res_casas.ToString();


            if (gerenciador.ConfiguracoesPGi.CfgProjeto.unidadesProjeto.res_un_tensao == und.kNm2)
                conversaoTensaoResultado = 1;

            if (gerenciador.ConfiguracoesPGi.CfgProjeto.unidadesProjeto.res_un_tensao == und.kNcm2)
                conversaoTensaoResultado = 1e-4;

            if (gerenciador.ConfiguracoesPGi.CfgProjeto.unidadesProjeto.res_un_tensao == und.kgfcm2)
                conversaoTensaoResultado = 0.01;

            if (gerenciador.ConfiguracoesPGi.CfgProjeto.unidadesProjeto.res_un_tensao == und.kgfm2)
                conversaoTensaoResultado = 100;

            if (gerenciador.ConfiguracoesPGi.CfgProjeto.unidadesProjeto.res_un_tensao== und.tonfm2)
                conversaoTensaoResultado = 0.1;

            if (gerenciador.ConfiguracoesPGi.CfgProjeto.unidadesProjeto.res_un_tensao == und.mpa)
                conversaoTensaoResultado = 0.001;
        }
        public double tamApoios, tamArticulacoes;
        public bool Diagrama_linha_gradiente, Diagrama_somente_preenchido, Diagrama_gradiente, Diagrama_somente_linhas, Diagrama_linha_contorno;
        System.Drawing.Color CorArestaDiagrama;
        public void AtualizaConfiguracaoDiagramas()
        {
            Diagrama_somente_preenchido = gerenciador.ConfiguracoesPGi.DiagramaOpcoesVisualizacao.gradiente && !gerenciador.ConfiguracoesPGi.DiagramaOpcoesVisualizacao.contorno;
            Diagrama_gradiente = gerenciador.ConfiguracoesPGi.DiagramaOpcoesVisualizacao.gradiente;
            Diagrama_somente_linhas = gerenciador.ConfiguracoesPGi.DiagramaOpcoesVisualizacao.linhas && !gerenciador.ConfiguracoesPGi.DiagramaOpcoesVisualizacao.contorno;
            Diagrama_linha_contorno = gerenciador.ConfiguracoesPGi.DiagramaOpcoesVisualizacao.linhas && gerenciador.ConfiguracoesPGi.DiagramaOpcoesVisualizacao.contorno;
            CorArestaDiagrama = gerenciador.ConfiguracoesPGi.DiagramaOpcoesVisualizacao.CorLinha;
            corValorDiagrama = gerenciador.ConfiguracoesPGi.DiagramaOpcoesVisualizacao.CorValor;
            Diagrama_linha_gradiente = gerenciador.ConfiguracoesPGi.DiagramaOpcoesVisualizacao.linha_gradiente;
            if (Diagrama_gradiente && (my || mz || mx || fx || fy || fz))
                gerenciador.pnCorResultados.Visible = true;
            else
                gerenciador.pnCorResultados.Visible = false;
        }

        public void CriaListasPrincipais()
        {
            try
            {
                RetanguloSelecao = new PontoD[4];
                RetanguloSelecao2 = new PontoD[4];

                Pontos = new List<TPonto>();
                Linhas = new List<TLinha>();
                Circulos = new List<TCirculo>();
                Arcos = new List<TArcoIMF>();
                TrechosVigas = new List<TTrechoViga>();
                Barras = new List<TBarraGenerica>();

                Lajes = new List<TLaje>();
                CargasPontuais = new List<TCargaPontual>();
                CargasLineares = new List<TCargaLinear>();
                Pilares = new List<TPilar>();
                Secoes = new List<TSecao>();
                CasosCarga = new List<TCasosCarga>();
                Textos = new List<TTexto>();

                BarraGrelha = new List<TBarraGrelha>();
                NoGrelha = new List<TNoGrelha>();

                Grips = new List<TGrip>();

                ObjetosSelecionados = new List<TObjetoDesenho>();
                NosSelecionados = new List<TObjetoDesenho>();
                Layers = new List<TLayer>();

                tiposObjetosDesenho = new Dictionary<string, TObjetoDesenho>();
                editTools = new Dictionary<string, IEditTool>();

                LayersById = new Dictionary<string, TLayer>();
            }
            catch (Exception e)
            {
                MessageBox.Show(e.Message);
            }
        }

        public void Desfazer()
        {
            undoBuffer.DoUndo(this);

            Atualiza_pIni_pFin_das_Barras_e_Cargas();
            AtualizaBarras();

            AtualizaListaSnap();
            DesenhaObjetos();
            glControl.SwapBuffers();
        }

        public void AdicionaFerramentasDesenho(string key, TObjetoDesenho ferrramenta)
        {
            tiposObjetosDesenho[key] = ferrramenta;
        }

        public void AddEditTool(string key, IEditTool edittool)
        {
            editTools.Add(key, edittool);
        }

        public void Coord(double pX, double pY, ref double coordX, ref double coordY)
        {
            if ((pX >= ponto_zero[0]) && (pY <= ponto_zero[1]))
            // x positivo   y positivo
            {
                coordX = (pX - ponto_zero[0]) * (double)(precisaoPixel);
                coordY = (pY - ponto_zero[1]) * (double)(precisaoPixel) * -1;
            }
            else if ((pX >= ponto_zero[0]) && (pY >= ponto_zero[1]))
            // x positivo   y negativo
            {
                coordX = (pX - ponto_zero[0]) * (double)(precisaoPixel);
                coordY = (ponto_zero[1] - pY) * (double)(precisaoPixel);
            }
            else if ((pX <= ponto_zero[0]) && (pY <= ponto_zero[1]))
            // x negativo   y positivo
            {
                coordX = (ponto_zero[0] - pX) * precisaoPixel * -1;
                coordY = (pY - ponto_zero[1]) * precisaoPixel * -1;
            }
            else if ((pX <= ponto_zero[0]) && (pY >= ponto_zero[1]))
            // x negativo   y negativo
            {
                coordX = (ponto_zero[0] - pX) * precisaoPixel * -1;
                coordY = (ponto_zero[1] - pY) * precisaoPixel;
            };
        }

        public static float pixelX(float coordX)
        {
            return (float)(coordX / precisaoPixel + ponto_zero[0]);
        }

        public static float pixelY(float coordY)
        {
            return (float)(coordY / -precisaoPixel + ponto_zero[1]);
        }

        public static float pixelX(double coordX)
        {
            return (float)(coordX / precisaoPixel + ponto_zero[0]);
        }

        public static float pixelY(double coordY)
        {
            return (float)(coordY / -precisaoPixel + ponto_zero[1]);
        }

        private void SetaRetanguloSelecao(double x1, double y1, double x2, double y2, double x3, double y3, double x4, double y4)
        {
            RetanguloSelecao[0].x = x1;
            RetanguloSelecao[0].y = y1;

            RetanguloSelecao[1].x = x2;
            RetanguloSelecao[1].y = y2;

            RetanguloSelecao[2].x = x3;
            RetanguloSelecao[2].y = y3;

            RetanguloSelecao[3].x = x4;
            RetanguloSelecao[3].y = y4;

            /* RetanguloSelecao2[0].X = (int)pixelX(x1);
             RetanguloSelecao2[0].Y = (int)pixelY(y1);

             RetanguloSelecao2[1].X = (int)pixelX(x2);
             RetanguloSelecao2[1].Y = (int)pixelY(y2);

             RetanguloSelecao2[2].X = (int)pixelX(x3);
             RetanguloSelecao2[2].Y = (int)pixelY(y3);

             RetanguloSelecao2[3].X = (int)pixelX(x4);
             RetanguloSelecao2[3].Y = (int)pixelY(y4);*/
        }

        public void RepintarObjeto(TObjetoDesenho obj)
        {
            if (obj == null)
                return;

            obj.Desenha(ref Unifilar, ref Transparencia, ref Arestas);
        }

        double x_min, y_min, x_max, y_max;

        List<TPonto> nos;

        Vector3d Ponto_Projetado;
        bool enquadrando = false;
        public void Enquadrar()
        {

            try
            {
                enquadrando = true;
                Pivo_no_centro();

                double box_x = 0;
                double box_y = 0;

                nos = new List<TPonto>();
                foreach (TBarraGenerica b in Estrutura.barras)
                {
                    if (LocalizaPonto(ref nos, b.pIni.x, b.pIni.y, b.pIni.z) == -1)
                        nos.Add(b.pIni);
                    if (LocalizaPonto(ref nos, b.pFin.x, b.pFin.y, b.pFin.z) == -1)
                        nos.Add(b.pFin);
                }
                if (nos.Count == 0)
                {
                    enquadrando = false;
                    return;
                }
                x_min = 99999999;
                y_min = 99999999;
                x_min = 99999999;
                x_max = -99999999;
                y_max = -99999999;

                bool necessario_menos_zoom = false;
                bool necessario_mais_zoom;
                bool frente_da_tela = false;
                if (CameraOrto)
                {
                    foreach (TPonto b in nos)
                    {
                        pixel1(ref b.x, ref b.y, ref b.z);

                        if (px_x1[0] < x_min)
                            x_min = px_x1[0];
                        if (px_y1[0] < y_min)
                            y_min = px_y1[0];

                        if (px_x1[0] > x_max)
                            x_max = px_x1[0];
                        if (px_y1[0] > y_max)
                            y_max = px_y1[0];
                    }
                }
                else
                {
                    foreach (TPonto b in nos)
                    {
                        pNDC = new Vector4d(b.x, b.y, b.z, 1);
                        //pNDC = pNDC * viewMatrix * Projecao;

                        pNDC = RMath.Multiply(pNDC, camera.viewMatrix);
                        pNDC = RMath.Multiply(pNDC, camera.Projecao/*Projecao_Panning*/);

                        pNDC /= pNDC.W;
                        if (pNDC.Z > 1 || pNDC.Z < 0)
                        {
                            necessario_menos_zoom = true;
                            frente_da_tela = true;
                            break;
                        }

                        pixel1(ref b.x, ref b.y, ref b.z);

                        if (px_x1[0] < x_min)
                            x_min = px_x1[0];
                        if (px_y1[0] < y_min)
                            y_min = px_y1[0];

                        if (px_x1[0] > x_max)
                            x_max = px_x1[0];
                        if (px_y1[0] > y_max)
                            y_max = px_y1[0];
                    }
                }

                box_x = x_max - x_min;
                box_y = y_max - y_min;

                if (((box_x) > (w - 200)) || ((box_y) > (h - 200)))
                    necessario_menos_zoom = true;

                mouseX = w / 2;
                mouseY = h / 2;

                if (necessario_menos_zoom)
                {
                    int num = 0;
                    while (necessario_menos_zoom)
                    {
                        DesenhaObjetos();
                        glControl.SwapBuffers();
                        num++;
                        if (num > 70)
                        {

                        }
                        Wheel(-500);
                        x_min = 99999999;
                        y_min = 99999999;
                        x_min = 99999999;
                        x_max = -99999999;
                        y_max = -99999999;

                        frente_da_tela = false;
                        foreach (TPonto b in nos)
                        {
                            pixel1(ref b.x, ref b.y, ref b.z);

                            if (px_x1[0] < x_min)
                                x_min = px_x1[0];
                            if (px_y1[0] < y_min)
                                y_min = px_y1[0];

                            if (px_x1[0] > x_max)
                                x_max = px_x1[0];
                            if (px_y1[0] > y_max)
                                y_max = px_y1[0];

                            if (!CameraOrto)
                            {
                                pNDC = new Vector4d(b.x, b.y, b.z, 1);
                                //  pNDC = pNDC * viewMatrix * Projecao;
                                pNDC = RMath.Multiply(pNDC, camera.viewMatrix);
                                pNDC = RMath.Multiply(pNDC, camera.Projecao/*Projecao_Panning*/);
                                
                                pNDC /= pNDC.W;

                                if (!Geom.Iguais(pNDC.Z, 0))
                                    if (pNDC.Z > 1 || pNDC.Z < 0)
                                    {
                                        frente_da_tela = true;
                                        break;
                                    }
                            }
                        }

                        if (!CameraOrto)
                            if (frente_da_tela)
                            {
                                necessario_menos_zoom = true;
                                continue;
                            }

                        box_x = x_max - x_min;
                        box_y = y_max - y_min;

                        if (((box_x) < (w - 200)) && ((box_y) < (h - 200)))
                        {
                            necessario_menos_zoom = false;
                        }
                        else
                        {
                            necessario_menos_zoom = true;
                            continue;
                        }
                    }
                }
                else
                {
                    x_min = 99999999;
                    y_min = 99999999;
                    x_min = 99999999;
                    x_max = -99999999;
                    y_max = -99999999;
                    int num = 0;
                    foreach (TPonto b in nos)
                    {
                        pixel1(ref b.x, ref b.y, ref b.z);

                        if (px_x1[0] < x_min)
                            x_min = px_x1[0];
                        if (px_y1[0] < y_min)
                            y_min = px_y1[0];

                        if (px_x1[0] > x_max)
                            x_max = px_x1[0];
                        if (px_y1[0] > y_max)
                            y_max = px_y1[0];
                    }

                    box_x = x_max - x_min;
                    box_y = y_max - y_min;
                    necessario_mais_zoom = false;
                    if (((box_x) < (w - 200)) && ((box_y) < (h - 200)))
                        necessario_mais_zoom = true;

                    while (necessario_mais_zoom)
                    {
                        DesenhaObjetos();
                        glControl.SwapBuffers();

                        x_min = 99999999;
                        y_min = 99999999;
                        x_min = 99999999;
                        x_max = -99999999;
                        y_max = -99999999;

                        num++;
                        if (num > 500)
                        {
                            Wheel(-500);
                            break;
                        }

                        Wheel(500);
                        foreach (TPonto b in nos)
                        {
                            pixel1(ref b.x, ref b.y, ref b.z);

                            if (px_x1[0] < x_min)
                                x_min = px_x1[0];
                            if (px_y1[0] < y_min)
                                y_min = px_y1[0];

                            if (px_x1[0] > x_max)
                                x_max = px_x1[0];
                            if (px_y1[0] > y_max)
                                y_max = px_y1[0];

                            if (!CameraOrto)
                            {
                                pNDC = new Vector4d(b.x, b.y, b.z, 1);
                                //   pNDC = pNDC * viewMatrix * Projecao;
                                pNDC = RMath.Multiply(pNDC, camera.viewMatrix);
                                pNDC = RMath.Multiply(pNDC, camera.Projecao/*Projecao_Panning*/);
                                pNDC /= pNDC.W;

                                if (pNDC.Z > 1 || pNDC.Z < 0)
                                {
                                    frente_da_tela = true;
                                    break;
                                }
                            }
                        }

                        box_x = x_max - x_min;
                        box_y = y_max - y_min;

                        if (!CameraOrto)
                            if (frente_da_tela)
                            {
                                necessario_mais_zoom = false;
                                Wheel(-500);
                                continue;
                            }

                        if (Geom.Iguais(box_x, 0) || Geom.Iguais(box_y, 0))
                            necessario_mais_zoom = false;
                        else
                        if (((box_x) < (w - 200)) && ((box_y) < (h - 200)))
                        {
                            necessario_mais_zoom = true;
                        }
                        else
                        {
                            necessario_mais_zoom = false;
                            Wheel(-500);
                            foreach (TPonto b in nos)
                            {
                                pixel1(ref b.x, ref b.y, ref b.z);

                                if (px_x1[0] < x_min)
                                    x_min = px_x1[0];
                                if (px_y1[0] < y_min)
                                    y_min = px_y1[0];

                                if (px_x1[0] > x_max)
                                    x_max = px_x1[0];
                                if (px_y1[0] > y_max)
                                    y_max = px_y1[0];
                            }

                            if (((box_x) < (w - 200)) && ((box_y) < (h - 200)))
                            {
                                continue;
                            }
                            else
                            {
                                Wheel(-500);
                            }
                        }
                    }
                }

                CalculaCentroEstrutura();

                /*CENTRALIZA*/
                double cx = centroX;
                double cy = centroY;
                double cz = centroZ;
                pixel1(ref cx, ref cy, ref cz);
                int mx = (int)px_x1[0];
                int my = (int)px_y1[0];

                if (CameraOrto) 
                    raioZoom.GerarRaio3D(ref mx, ref my, ref ViewPortPrincipal, ref camera.viewMatrix, ref camera.Projecao);
                else 
                    raioZoom.GerarRaio3D(ref mx, ref my, ref ViewPortPrincipal, ref camera.viewMatrix, ref camera.Projecao /*Projecao_Panning*/);

                Ponto_zNear = new vec3(raioZoom.p0.x, raioZoom.p0.y, raioZoom.p0.z);

                double dist_near_HitObjeto = (Ponto_zNear - new vec3(centroX, centroY, centroZ)).Magnitude();

                /*raio partindo do meio da tela*/
                int meio_x = (int)(w / 2);
                int meio_y = (int)(h / 2);
                if (CameraOrto) 
                    raioZoom.GerarRaio3D(ref meio_x, ref meio_y, ref ViewPortPrincipal, ref camera.viewMatrix, ref camera.Projecao);
                else 
                    raioZoom.GerarRaio3D(ref meio_x, ref meio_y, ref ViewPortPrincipal, ref camera.viewMatrix, ref camera.Projecao/*Projecao_Panning*/);

                v1 = new Vector3d(raioZoom.p0.x, raioZoom.p0.y, raioZoom.p0.z);
                v2 = new Vector3d(raioZoom.p1.x, raioZoom.p1.y, raioZoom.p1.z);
                /******************************/

                v3 = new Vector3d(Ponto_zNear.x, Ponto_zNear.y, Ponto_zNear.z);
                v4 = new Vector3d(centroX, centroY, centroZ);

                double angulo_entre = Vector3d.CalculateAngle(v2 - v1, v4 - v3);
                double distancia_znear_ate_plano = (Math.Cos(angulo_entre) * dist_near_HitObjeto);
                vec3 normalPlano = new vec3(0, 0, 1);
                PlanoPanning = new Plano(normalPlano, new vec3(0, 0, -distancia_znear_ate_plano), "xy", false);
                PlanoPanning.xmin = -2; PlanoPanning.ymin = -2; PlanoPanning.xmax = 2; PlanoPanning.ymax = 2;

                viewMatrix_Panning = Matrix4d.CreateTranslation(0, 0, 0);
                if (CameraOrto)
                    raioZoom.GerarRaio3D(ref mx, ref my, ref ViewPortPrincipal, ref viewMatrix_Panning, ref camera.Projecao);
                else 
                    raioZoom.GerarRaio3D(ref mx, ref my, ref ViewPortPrincipal, ref viewMatrix_Panning, ref camera.Projecao/*Projecao_Panning*/);

                raioZoom.CalculaIntersecao_Raio_x_Plano(ref PlanoPanning, ref Coord2_PlanoPanning);

                if (CameraOrto) 
                    raioZoom.GerarRaio3D(ref meio_x, ref meio_y, ref ViewPortPrincipal, ref viewMatrix_Panning, ref camera.Projecao);
                else 
                    raioZoom.GerarRaio3D(ref meio_x, ref meio_y, ref ViewPortPrincipal, ref viewMatrix_Panning, ref camera.Projecao/*Projecao_Panning*/);

                raioZoom.CalculaIntersecao_Raio_x_Plano(ref PlanoPanning, ref Coord_PlanoPanning);
                vec3 dif = Coord_PlanoPanning - Coord2_PlanoPanning;

                if (CameraOrto) 
                    dif *= -1;

                camera.x_trans += dif.x;
                camera.y_trans += dif.y;

           //     camera.x_trans = x_trans;
           //     camera.y_trans = y_trans;
                camera.ViewDirty = true;

                DesenhaObjetos();
                glControl.SwapBuffers();
                enquadrando = false;
            }
            catch (Exception ee)
            {
                enquadrando = false;
                MessageBox.Show("erro ao enqudrar -> " + ee.Message);
            }
        }

        public FPrincipal()
        {
            InitializeComponent();


        }

        BackgroundWorker worker_AtualizaIntersecoes = new BackgroundWorker();

        public FPrincipal(Gerenciador frmPai)
        {
            //this.glControl = new OpenTK.GLControl(new OpenTK.Graphics.GraphicsMode(32, 24, 0, 8));
            if (frmPai.ConfiguracaoPrograma.SuavizacaoOpenGl)
                this.glControl = new OpenTK.GLControl(new OpenTK.Graphics.GraphicsMode(32, 24, 0, 8));
            else
               this.glControl = new OpenTK.GLControl(new OpenTK.Graphics.GraphicsMode(32, 24, 4, 0));
            
            InitializeComponent();

            gerenciador = frmPai;

            CriaListasPrincipais();
            CreateStandardLayers();
            tipoComando = eTipoComando.selecionar;
            CloseButtonVisible = false;
        }

        public void SetaSelecionados(bool s, int pav, bool Nos = true, bool limpaSelecionados = true)
        {
            if (!clickShift)
            {
                CaixaSelecao = false;

               foreach (TObjetoDesenho obj in ObjetosSelecionados)
                    if (obj.Selecionado)
                        obj.SetaSelecao(s, s);

                ObjetosSelecionados.Clear();
                
                if (Nos)
                    NosSelecionados.Clear();

                foreach (TObjetoDesenho obj in Estrutura.nos)
                    if (obj.Selecionado)
                        obj.SetaSelecao(s, s);

                /*  foreach (TObjetoDesenho obj in Linhas)
                      if (obj.Selecionado)
                          obj.SetaSelecao(s, s);*/


                /* foreach (TObjetoDesenho obj in Barras)
                     if ((obj as TBarraGenerica).Selecionado)
                         obj.SetaSelecao(s, s);


                  foreach (TObjetoDesenho obj in Estrutura.barras)
                      if ((obj as TBarraGenerica).Selecionado)
                        obj.SetaSelecao(s, s);*/
/*
                  foreach (TObjetoDesenho obj in Estrutura.apoios)
                      obj.SetaSelecao(s, s);

                  foreach (TObjetoDesenho obj in Estrutura.cargaLinear)
                      if (obj.Selecionado)
                          obj.SetaSelecao(s, s);
                  foreach (TObjetoDesenho obj in Estrutura.cargaPontual)
                      if (obj.Selecionado)
                          obj.SetaSelecao(s, s);*/
            }
        }

        public ISettings Settings = null;
        TPonto pointRel, point1, point2, pointM = null;

        TUndoRedo UndoBuffer = new TUndoRedo();

        public void ComandoEdicao(string m_editId, eTipoComando comando = eTipoComando.selecionar)
        {
            CancelaResultados();
            CancelaInsercoes(comando == eTipoComando.edit);

            FimCopiaRepetida = false;
            tipoComando = comando; //eTipoComando.selecionar;

            IdFerramentaEdicao = m_editId;
            FerramentaEdicao = editTools[m_editId].Clone();

            Comando.Text = FerramentaEdicao.GetComando(0);
            
            editToolTipoSelecao = FerramentaEdicao.editToolTipoSelecao;

            LayersTemp = new List<TLayer>();

            if (FerramentaEdicao.LayerObjetoParaSelecionar != "TODOS")
            {
                /*Travo os layers que nao fazem parte dos objetos possiveis de serem editados nessa operação*/
                foreach (TLayer lay in Estrutura.layers)
                {
                    //cria uma lista de historico dos layers, para poder refazer o estado deles apos a edição, ja que aqui eu 
                    //travo a maioria deles...tenho que ligar eles de novo se eles estavam ligados antes
                    LayersTemp.Add(new TLayer(lay.nome, lay.Ligado, lay.Congelado, null, 0, lay.Travado));

                    if (FerramentaEdicao.LayerObjetoParaSelecionar != null)
                        if (FerramentaEdicao.LayerObjetoParaSelecionar != lay.nome)
                        {
                            lay.Congelado = false;
                            lay.Travado = true;
                            lay.Ligado = false;
                        }
                }
            }
        }

        private void ComandoTexto(string msg, string m_drawobjectid, bool first = true)
        {
            gerenciador.DigitarComando.Focus();

            string cmd = msg;

            Comando.Text = cmd;

           // Comando.Top = this.Height - 15;
          //  Comando.Left = this.Width/2 - 180;
            this.Focus();
        }
        void CancelaResultados()
        {
            gerenciador.CancelaResultados();
        }
        public void ComandoDesenhar(string m_drawobjectid, ISettings set, string arg, string ModoInsercao)
        {
            CancelaInsercoes();
            CancelaResultados();

            Settings = set;

            if (ModoInsercao == Const.ID_INSERCAO_INDIVIDUAL)
                tipoComando = eTipoComando.draw;
            else
            if (ModoInsercao == Const.ID_INSERCAO_SELECAO)
                tipoComando = eTipoComando.selecionar;

            IdObjetoDesenho = m_drawobjectid;

            ComandoTexto(tiposObjetosDesenho[m_drawobjectid].PrimeiroComando(), m_drawobjectid, true);
        }
        List<TLinha> LinhasProlongamento = new List<TLinha>();
        vec3 p1 = new vec3(0);
        vec3 p2 = new vec3(0);

        public void CriaLinhasProjecaoTemporarias(ref TPonto ponto, bool atuSnap = true, bool shift = false, TBarraGenerica bar = null, bool sohProlongamentos = false)
        {
            CoordsSubdvisao.Clear();

            LinhasProlongamento.RemoveAll(o => o.auxiliar);
            LinhasProlongamento.Add(new TLinha(new TPonto(ponto.x, ponto.y, ponto.z), new TPonto(ponto.x, ponto.y, ponto.z + 70)));
            LinhasProlongamento.Add(new TLinha(new TPonto(ponto.x, ponto.y, ponto.z), new TPonto(ponto.x, ponto.y, ponto.z - 70)));
            LinhasProlongamento.Add(new TLinha(new TPonto(ponto.x, ponto.y, ponto.z), new TPonto(ponto.x + 70, ponto.y, ponto.z)));
            LinhasProlongamento.Add(new TLinha(new TPonto(ponto.x, ponto.y, ponto.z), new TPonto(ponto.x - 70, ponto.y, ponto.z)));
            LinhasProlongamento.Add(new TLinha(new TPonto(ponto.x, ponto.y, ponto.z), new TPonto(ponto.x, ponto.y + 70, ponto.z)));
            LinhasProlongamento.Add(new TLinha(new TPonto(ponto.x, ponto.y, ponto.z), new TPonto(ponto.x, ponto.y - 70, ponto.z)));
            TBarraGenerica b2;
            //prolonga nas direções das barras que chegam no nó
            if (shift)
            {
                vec3 p_1, p_2, p_3, p_4, pontoAvaliar = new vec3(ponto.x,ponto.y,ponto.z);
              /*  for (j = 0; j < Estrutura.barras.Count; j++)
                {
                    b2 = Estrutura.barras[j];
                    if (b2 == null) continue;
                    if ((Object)b != (Object)b2)
                    {
                        p_3 = new vec3(b2.pIni.x, b2.pIni.y, b2.pIni.z);
                        p_4 = new vec3(b2.pFin.x, b2.pFin.y, b2.pFin.z);

                        var r1 = Geom.ClassificarPontoNoSegmento(p_3, p_1, p_2);
                        var r2 = Geom.ClassificarPontoNoSegmento(p_4, p_1, p_2);

                        if (r1.Posicao == PosicaoNoSegmento.Fora && r2.Posicao == PosicaoNoSegmento.Fora)
                            continue;
                    }
                }*/
                vec3 offset = new vec3(0);
                foreach (TBarraGenerica b in Estrutura.barras)
                {
                    p_1 = new vec3(b.pIni.x, b.pIni.y, b.pIni.z);
                    p_2 = new vec3(b.pFin.x, b.pFin.y, b.pFin.z);

                    var r1 = Geom.ClassificarPontoNoSegmento(pontoAvaliar, p_1, p_2);

                    if (r1.Posicao == PosicaoNoSegmento.Interior)
                    {
                        offset = new vec3(b.pIni.x, -b.pIni.y, -b.pIni.z);

                        p1 = new vec3(b.pFin.x, -b.pFin.y, -b.pFin.z) - offset;
                        p2 = new vec3(b.pIni.x, -b.pIni.y, -b.pIni.z) - offset;

                        p1 = p1 - p2;
                        p2 = p1 * 20;

                        p2 += offset;

                        LinhasProlongamento.Add(new TLinha(new TPonto(ponto.x, ponto.y, ponto.z), new TPonto(p2.x, -p2.y, -p2.z)));

                        p2 -= offset;
                        p2 = p1 / 20;
                        p2 = p1 * -20;
                        p2 += offset;

                        LinhasProlongamento.Add(new TLinha(new TPonto(ponto.x, ponto.y, ponto.z), new TPonto(p2.x, -p2.y, -p2.z)));
                    }

                    if (b.pIni == ponto)
                    {
                        offset = new vec3(b.pIni.x, -b.pIni.y, -b.pIni.z);

                        p1 = new vec3(b.pFin.x, -b.pFin.y, -b.pFin.z) - offset;
                        p2 = new vec3(b.pIni.x, -b.pIni.y, -b.pIni.z) - offset;

                        p1 = p1 - p2;
                        p2 = p1 * 20;

                        p2 += offset;

                        LinhasProlongamento.Add(new TLinha(new TPonto(ponto.x, ponto.y, ponto.z), new TPonto(p2.x, -p2.y, -p2.z)));

                        p2 -= offset;
                        p2 = p1 / 20;
                        p2 = p1 * -20;
                        p2 += offset;

                        LinhasProlongamento.Add(new TLinha(new TPonto(ponto.x, ponto.y, ponto.z), new TPonto(p2.x, -p2.y, -p2.z)));
                    }
                    else
                    if (b.pFin == ponto)
                    {
                        offset = new vec3(b.pFin.x, -b.pFin.y, -b.pFin.z);
                        p1 = new vec3(b.pIni.x, -b.pIni.y, -b.pIni.z) - offset;
                        p2 = new vec3(b.pFin.x, -b.pFin.y, -b.pFin.z) - offset;

                        p1 = p1 - p2;
                        p2 = p1 * 20;

                        p2 += offset;

                        LinhasProlongamento.Add(new TLinha(new TPonto(ponto.x, ponto.y, ponto.z), new TPonto(p2.x, -p2.y, -p2.z)));

                        p2 -= offset;
                        p2 = p1 / 20;
                        p2 = p1 * - 20;
                        p2 += offset;
                        LinhasProlongamento.Add(new TLinha(new TPonto(ponto.x, ponto.y, ponto.z), new TPonto(p2.x, -p2.y, -p2.z)));
                    }
                }
            }

            if (atuSnap)
              AtualizaListaSnap(true,bar, sohProlongamentos);
        }

        public TObjetoDesenho InicializaObjeto(string type, ref TPonto point)
        {
            try
            {
                // SalvaPavimentoAtual();

                TObjetoDesenho newobj = tiposObjetosDesenho[type].Clone() as TObjetoDesenho;
                string cmd = string.Empty;

                if (IdObjetoDesenho == Const.ID_PILAR)
                    Settings = DadosPilar;
                if (IdObjetoDesenho == Const.ID_LAJE)
                    Settings = DadosLaje;
                if (IdObjetoDesenho == Const.ID_TRECHOVIGA)
                    Settings = DadosTrecho;
                if (IdObjetoDesenho == Const.ID_BARRAGENERICA)
                    Settings = DadosBarra;
                if (IdObjetoDesenho == Const.ID_APOIO)
                    Settings = DadosApoio;

                if (newobj != null)
                {
                    newobj.Layer = Estrutura.LayersByIdPrincipal[newobj.IdLayer];

                    try
                    {
                        newobj.Initialize(ref point, ref cmd, Settings, newobj.Layer, ref Linhas, ref PavimentoAtual);

                        if (CorCima == System.Drawing.Color.Black)
                        {
                            newobj.Rgb[0] = 255;
                            newobj.Rgb[1] = 255;
                            newobj.Rgb[2] = 255;
                        }
                        else
                        {
                            newobj.Rgb[0] = 0;
                            newobj.Rgb[1] = 0;
                            newobj.Rgb[2] = 0;
                        }
                    }
                    catch (TErroInicializacaoObjeto e)
                    {
                        MessageBox.Show(e.msg, "Erro ao inicializar objeto: " + newobj.Tipo, System.Windows.Forms.MessageBoxButtons.OK, MessageBoxIcon.Error);
                        ComandoTexto(newobj.PrimeiroComando(), "", false);
                        return null;
                    }

                    ComandoTexto(cmd, "", false);
                }

                return newobj as TObjetoDesenho;
            }
            catch (Exception e)
            {
                MessageBox.Show("erro InicializaObjetoAtravesDoClick:" + e.Message);
            }
            return null;
        }
        TPonto point;

        public void HandleEditTools()
        {
            MessageBox.Show(ObjetosSelecionados.Count.ToString());
        }
        List<TObjetoDesenho> SelecionadosAntes;
        List<TPonto> PontosBaseCopia = new List<TPonto>();
        public List<TObjetoDesenho> ObjetosCopiados, ObjetosAdicionados, ObjetosMovidos, ObjetosExcluidos;
        double distCopiaRepetida1, distCopiaRepetida2;
        TPonto vdir2, vdir1;
        void CriaCopiaRepetida()
        {
            bool eixoslocais = gerenciador.ConfiguracoesPGi.F3DOpcoesVisualizacao.MostrarEixosLocais;
            gerenciador.ConfiguracoesPGi.F3DOpcoesVisualizacao.MostrarEixosLocais = false;
            EixosLocais = false;

                //EixosLocais = gerenciador.ConfiguracoesPGi.F3DOpcoesVisualizacao.MostrarEixosLocais;
            gerenciador.ChamaAguardar(this, "Criando cópias. Aguarde...");
            gerenciador.pbAguardar.Maximum = (int)edNumRepeticoes.Value+1;
            gerenciador.pbAguardar.Value = 0;
            TPonto p1 = new TPonto(0, 0, 0);
            TPonto pbase;
            vdir1 = PontosBaseCopia[1] - PontosBaseCopia[0];
            distCopiaRepetida1 = PontosBaseCopia[1].DotProduct(PontosBaseCopia[0]);

            if (PontosBaseCopia[1] is object)
            {
                TPonto p2 = PontosBaseCopia[1].Clone() as TPonto;
                pbase = PontosBaseCopia[0];
                for (int i = 0; i < edNumRepeticoes.Value - 1; i++)
                {
                   // gerenciador.pbAguardar.Value += 1; Application.DoEvents();
                    //   PontosBaseCopia[0].primeiraVezMover = true;
                    //   PontosBaseCopia[0].Mover(p1, p2);

                    PontosBaseCopia[1].primeiraVezMover = true;
                    PontosBaseCopia[1].Mover(ref pbase, ref  p2, true);

                    p2 = PontosBaseCopia[1].Clone() as TPonto;
                    tipoComando = eTipoComando.edit;
                    MouseEditCopiaRepetida(0, 0, 0, 0, "", 0);
                    MouseEditCopiaRepetida(PontosBaseCopia[0].x, PontosBaseCopia[0].y, PontosBaseCopia[0].z, 0, "", 0);

                    //  (FerramentaEdicao as TCopiarElementos).OnMouseMove(ref p2, true, true, gerenciador.AnguloRotacao);
                    MouseEditCopiaRepetida(PontosBaseCopia[1].x, PontosBaseCopia[1].y, PontosBaseCopia[1].z, 0, "", 0);
                    //  AtualizaElementosEstrutura(false, true);

                }
            }
            vdir2 = PontosBaseCopia[1] - PontosBaseCopia[0];

            double mod1 = vdir1.Magnitude();
            double mod2 = vdir2.Magnitude();

            double dot = vdir1.DotProduct(vdir2);
            double angto = dot / (mod1 * mod2);
            double anguloGrau = Math.Acos(angto) * 57.2958;
            if (!Geom.Iguais(anguloGrau, 0) && !Geom.Iguais(anguloGrau, 180))
            {
                MessageBox.Show("copiou errado");
            }

            SetaSelecionados(false, PavimentoAtual);
           // AtualizaElementosEstrutura(false, true);

            MostrarCargasPeloCaso();

            PontosBaseCopia.Clear();
            FimCopiaRepetida = true;

            Atualiza_pIni_pFin_das_Barras_e_Cargas();
            AtualizaListaSnap(false);

            if (eixoslocais)
            {
                gerenciador.ConfiguracoesPGi.F3DOpcoesVisualizacao.MostrarEixosLocais = true;
                EixosLocais = true;
            }

            gerenciador.pbAguardar.Value += 1;
          //  Application.DoEvents();
            AtualizaShaders();

            gerenciador.FechaAguardar();
        }

        public void Atualiza_pIni_pFin_das_Barras_e_Cargas()
        {

            List<TBarraGenerica> bg = new List<TBarraGenerica>();

            foreach (TBarraGenerica o in Estrutura.barras)
            {
               o.Linha_Eixo.pIni.x = o.pIni.x;
               o.Linha_Eixo.pIni.y = o.pIni.y;
               o.Linha_Eixo.pIni.z = o.pIni.z;

               o.Linha_Eixo.pFin.x = o.pFin.x;
               o.Linha_Eixo.pFin.y = o.pFin.y;
               o.Linha_Eixo.pFin.z = o.pFin.z;

               o.Linha_Eixo.IDBarra = o.IDBarra;
               o.pMedioBarra = new vec3((o.pIni.x + o.pFin.x) / 2, (o.pIni.y + o.pFin.y) / 2, (o.pIni.z + o.pFin.z) / 2);
               o.comprimento = (float)o.pFin.DistanceTo(o.pIni);
            }

            foreach (TCargaLinear o in Estrutura.cargaLinear)
            {
                bg = Estrutura.barras.FindAll(b => b.IDBarra == o.idBarra);

                foreach (TBarraGenerica b in bg)
                {
                    o.pIni.x = b.Linha_Eixo.pIni.x;
                    o.pIni.y = b.Linha_Eixo.pIni.y;
                    o.pIni.z = b.Linha_Eixo.pIni.z;

                    o.pFin.x = b.Linha_Eixo.pFin.x;
                    o.pFin.y = b.Linha_Eixo.pFin.y;
                    o.pFin.z = b.Linha_Eixo.pFin.z;

                    o.anguloRotacao = Estrutura.barras[Estrutura.barras.Count - 1].Dados.anguloRotacao;
                    o.CriaSetas(true, 0, 0, fatorCarga);
                }
            }

            AtualizaBarras();
        }

        public void MouseEditCopiaRepetida(double x, double y, double z, double arg1, string arg2, double ang)
        {
            try
            { 
                point = null;
                string cmd = string.Empty;
                cmd = ang.ToString();

                point = new TPonto(x, y, z, 0, 0, 0);

                //point.x = NormalizarCoordenada(x);
               // point.y = NormalizarCoordenada(y);
                //point.z = NormalizarCoordenada(z);

                List<TObjetoDesenho> ObjetosResultado = new List<TObjetoDesenho>();
                eObjetoDesenhoMouseDown resultado = FerramentaEdicao.OnMouseDown(gerenciador, this, ref ObjetosSelecionados, ref ObjetosResultado, ref point, ref cmd, Linhas, Pontos, false);

                ComandoTexto(cmd, "", false);

                if (resultado == eObjetoDesenhoMouseDown.Done || (resultado == eObjetoDesenhoMouseDown.DoneRepeat))
                {
                /*    if (ObjetosResultado.Count > 0)
                    {
                        foreach (TObjetoDesenho obj in ObjetosResultado)
                        {
                          //  if (IdFerramentaEdicao == Const.ID_COPIAR_ELEMENTOS || IdFerramentaEdicao == Const.ID_COPIA_PADRAO)
                                ProximaNumeracao(obj);
                           
                            if (obj.Tipo == Const.ID_BARRAGENERICA)
                                AdicionaBarraGenerica(obj as TBarraGenerica);
                            else
                                AdicionaObjeto(obj, PavimentoAtual, false,false);

                            ObjetosCopiados.Add(obj);
                        }
                    }*/

                    List<TObjetoDesenho> bars = ObjetosResultado.FindAll(o => o.Tipo == Const.ID_BARRAGENERICA);
                    List<TObjetoDesenho> outros = ObjetosResultado.FindAll(o => o.Tipo != Const.ID_BARRAGENERICA);

                    foreach (TObjetoDesenho obj in bars)
                    {
                    //    ProximaNumeracao(obj);
                        AdicionaBarraGenerica(obj as TBarraGenerica, true);
                        ObjetosCopiados.Add(obj);
                    }
                    foreach (TObjetoDesenho obj in outros)
                    {
                        ProximaNumeracao(obj);
                        AdicionaObjeto(obj, PavimentoAtual, false, false, false);
                        ObjetosCopiados.Add(obj);
                    }

                  //  AtualizaPontos_Barras_e_Cargas();

                   // if (FerramentaEdicao.ApagarSelecionados)
                   //     DeletaSelecionados(-1);

                    //RefazEstadoLayers();

                    tipoComando = eTipoComando.selecionar;

                    if (resultado == eObjetoDesenhoMouseDown.DoneRepeat)
                    {
                        if (IdFerramentaEdicao == Const.ID_COPIAR_ELEMENTOS)
                        {
                            double x1 = (FerramentaEdicao as TCopiarElementos).ponto1.x;
                            double y1 = (FerramentaEdicao as TCopiarElementos).ponto1.y;
                            double z1 = (FerramentaEdicao as TCopiarElementos).ponto1.z;

                            ComandoEdicao(IdFerramentaEdicao);

                            ObjetosSelecionados = SelecionadosAntes.ToList();
                            ObjetosSelecionados.ForEach(o => o.SetaSelecao(true, true));

                            tipoComando = eTipoComando.selecionar;
                            //   MouseEdit(0, 0, 0, 0, "", 0);
                            //   MouseEdit(x1, y1, z1, 0, "", 0);
                        }
                        else
                            ComandoEdicao(IdFerramentaEdicao);

                       // AtualizaListaSnap();
                    }
                    //editTool = null;
                    pnProgresso.Visible = false;
                }
                else
                if (resultado == eObjetoDesenhoMouseDown.Continue)
                {
                 //   SelecionadosAntes = new List<TObjetoDesenho>();
                  //  foreach (TObjetoDesenho obj in ObjetosSelecionados)
                 //       SelecionadosAntes.Add(obj);

                    FerramentaEdicao.Continue();

                   // CriaLinhasProjecaoTemporarias(ref point, true);
                    HabilitaCoords(true, true, false);
                }
            }
            catch (Exception ee)
            {
                MessageBox.Show("erro handlemousedownedit" + ee.Message);
            }
        }
        public void RemeverDosObjetosSelecionados_Nao_Copiaveis()
        {
            foreach (TObjetoDesenho obj in ObjetosSelecionados)
                if (obj.NaoPermiteMoverOuCopiar)
                    obj.SetaSelecao(false, false);

            ObjetosSelecionados.RemoveAll(obj => obj.NaoPermiteMoverOuCopiar == true);
        }

        public void MouseEdit(double x, double y, double z, double arg1, string arg2, double ang)
        {
            try
            {
                if (FerramentaEdicao == null)
                    return;

                point = null;
                string cmd = string.Empty;
                cmd = ang.ToString();

                point = new TPonto(x, y, z, 0, 0, 0);

                /*  point.x = NormalizarCoordenada(x);
                  point.y = NormalizarCoordenada(y);
                  point.z = NormalizarCoordenada(z);
                  */

                List<TObjetoDesenho> ObjetosResultado = new List<TObjetoDesenho>();
                eObjetoDesenhoMouseDown resultado = FerramentaEdicao.OnMouseDown(
                    gerenciador, 
                    this, 
                    ref ObjetosSelecionados, 
                    ref ObjetosResultado, 
                    ref point,
                    ref cmd, 
                    Linhas,
                    Pontos,
                    false);

           
                ComandoTexto(cmd, "", false);

                if (resultado == eObjetoDesenhoMouseDown.Done || (resultado == eObjetoDesenhoMouseDown.DoneRepeat))
                {
                    if (ObjetosResultado.Count > 0)
                    {
                        ObjetosCopiados    = new List<TObjetoDesenho>();
                        ObjetosAdicionados = new List<TObjetoDesenho>();
                        comandosMover      = new List<ComandoMover>();

                        List<TObjetoDesenho> bars   = ObjetosResultado.FindAll(o => o.Tipo == Const.ID_BARRAGENERICA);
                        List<TObjetoDesenho> outros = ObjetosResultado.FindAll(o => o.Tipo != Const.ID_BARRAGENERICA);

                        foreach (TObjetoDesenho obj in bars)
                        {
                            AdicionaBarraGenerica(obj as TBarraGenerica, IdFerramentaEdicao == Const.ID_COPIAR_ELEMENTOS);
                            ObjetosCopiados.Add(obj);
                        }
                        foreach (TObjetoDesenho obj in outros)
                        {
                            AdicionaObjeto(obj, PavimentoAtual, false, false, false);
                            ObjetosCopiados.Add(obj);
                        }
                    }

                    //essa merda de doevents estava bugando as copias as vezes, fazia a posicao final da copia ser a posicao em que eu estava com o mouse
                  //  Application.DoEvents();


                    if ((IdFerramentaEdicao == Const.ID_ROTACIONAR_ELEMENTOS) && chApagarOriginalRotacao.Checked)
                        DeletaSelecionados(-1);


                    if (FerramentaEdicao.ApagarSelecionados)
                        DeletaSelecionados(-1);
                    else
                    if (IdFerramentaEdicao == Const.ID_COPIA_PADRAO)
                        SetaSelecionados(false, this.PavimentoAtual);


                    Atualiza_pIni_pFin_das_Barras_e_Cargas();
                    
                    tipoComando = eTipoComando.selecionar;

                    SetaSelecionados(false, PavimentoAtual);
                   
                    AtualizaListaSnap();

                    if (resultado == eObjetoDesenhoMouseDown.DoneRepeat)
                    {
                        if (IdFerramentaEdicao == Const.ID_COPIAR_ELEMENTOS)
                        {
                            double x1 = (FerramentaEdicao as TCopiarElementos).ponto1.x;
                            double y1 = (FerramentaEdicao as TCopiarElementos).ponto1.y;
                            double z1 = (FerramentaEdicao as TCopiarElementos).ponto1.z;

                            if (!FimCopiaRepetida && edNumRepeticoes.Value > 1)
                            {
                                PontosBaseCopia.Add((FerramentaEdicao as TCopiarElementos).ponto1);
                                PontosBaseCopia.Add((FerramentaEdicao as TCopiarElementos).ponto2);
                            }

                            ComandoEdicao(IdFerramentaEdicao);

                            ObjetosSelecionados = SelecionadosAntes.ToList();
                            ObjetosSelecionados.ForEach(o => o.SetaSelecao(true, true));

                            // foreach (TObjetoDesenho obj in SelecionadosAntes)
                            // {
                            //     ObjetosSelecionados.Add(obj);
                            //    obj.SetaSelecao(true, true);
                            // }
                            bool copiarepetida = false;
                            if (IdFerramentaEdicao == Const.ID_COPIAR_ELEMENTOS && edNumRepeticoes.Value > 1 && PontosBaseCopia.Count > 0)
                            {
                                Comando.Text = "";
                           //     Application.DoEvents();
                                copiarepetida = true;
                                CriaCopiaRepetida();
                            }

                            PontosBaseCopia.Clear();

                            //Application.DoEvents();
                            if (undoBuffer.CanCapture)
                            {
                                foreach (TObjetoDesenho o in ObjetosAdicionados)
                                    if (!ObjetosCopiados.Exists(oo => (object)oo == (object)o))
                                        ObjetosCopiados.Add(o);
                             //   Application.DoEvents();
                                undoBuffer.AdicionaComando(new ComandoAdicionar(ObjetosCopiados, comandosMover));
                                ObjetosCopiados.Clear();
                                Alterou(true);
                            }

                            //tive q botar esse teste pois copia errado na hora de copias repetidas
                            if (copiarepetida)
                                return;

                            tipoComando = eTipoComando.edit;
                            MouseEdit(0, 0, 0, 0, "", 0);
                            FimCopiaRepetida = true;
                            MouseEdit(x1, y1, z1, 0, "", 0);

                            AtualizaListaSnap();
                            AtualizaShaders();
                        }
                        else
                        {
                            Alterou(true);

                            if (undoBuffer.CanCapture)
                            {
                                if (ObjetosAdicionados != null)
                                {
                                    if (ObjetosAdicionados.Count > 0)
                                    {
                                        foreach (TObjetoDesenho o in ObjetosAdicionados)
                                            if (!ObjetosCopiados.Exists(oo => (object)oo == (object)o))
                                                ObjetosCopiados.Add(o);
                                    }
                                }
                                if (ObjetosCopiados != null)
                                {
                                    if (ObjetosCopiados.Count > 0)
                                    {
                                //        Application.DoEvents();
                                        undoBuffer.AdicionaComando(new ComandoAdicionar(ObjetosCopiados, comandosMover));
                                        ObjetosCopiados.Clear();
                                    }
                                }
                                Alterou(true);
                            }

                            ComandoEdicao(IdFerramentaEdicao);

                            if (IdFerramentaEdicao == Const.ID_ROTACIONAR_ELEMENTOS || IdFerramentaEdicao == Const.ID_DIVIDIR_NAS_INTERSECOES)
                                MostrarCargasPeloCaso();

                            menuPrincipal.Close();
                        }

                        if (IdFerramentaEdicao == Const.ID_MOVER_ELEMENTOS)
                        {
                            ObjetosSelecionados = SelecionadosAntes.ToList();
                           // ObjetosSelecionados.ForEach(o => o.SetaSelecao(true, true));

                        }

                   //     SetaSelecionados(false, PavimentoAtual);

                    }
                    else
                    {
                  
                        //CriaLinhasProjecaoTemporarias(ref point, true);
                    }
                    // AtualizaElementosEstrutura(false, true);
                    //editTool = null;
                    pnProgresso.Visible = false;
                    //    CancelaInsercoes();
                 /*  if (FerramentaEdicao.ApagarSelecionados)
                        DeletaSelecionados(-1);
                    else
                    if (IdFerramentaEdicao == Const.ID_COPIA_PADRAO)
                        SetaSelecionados(false, this.PavimentoAtual);*/

                   // SetaSelecionados(false, PavimentoAtual);
                     
                    // AtualizaShaders();
                }
                else
                if (resultado == eObjetoDesenhoMouseDown.Continue)
                {
                    FimCopiaRepetida = false;
                    SelecionadosAntes = new List<TObjetoDesenho>();
                  //  foreach (TObjetoDesenho obj in ObjetosSelecionados)
                        SelecionadosAntes = ObjetosSelecionados.ToList();

                    FerramentaEdicao.Continue();

                    CriaLinhasProjecaoTemporarias(ref point, false);

                //    if (IdFerramentaEdicao != Const.ID_ROTACIONAR_ELEMENTOS)
                    HabilitaCoords(true, IdFerramentaEdicao == Const.ID_COPIAR_ELEMENTOS, false, IdFerramentaEdicao == Const.ID_ROTACIONAR_ELEMENTOS, IdFerramentaEdicao == Const.ID_ESPELHAR_ELEMENTOS, IdFerramentaEdicao == Const.ID_MOVER_ELEMENTOS);

                    if (IdFerramentaEdicao == Const.ID_COPIAR_ELEMENTOS)
                    {
                        if ((FerramentaEdicao as TCopiarElementos).Objetos == null)
                        {
                            edNumRepeticoes.Value = 1;
                            MostrarCargasPeloCaso(false);
                        }
                    }

                    AtualizaListaSnap();
                }
            }
            catch (Exception ee)
            {
                MessageBox.Show("erro handlemousedownedit" + ee.Message);
            }
        }


        bool FimCopiaRepetida = false;
        public void FechaProgresso()
        {
            pnProgresso.Visible = false;
            Progresso.Value = 0;
            lbprogresso.Visible = false;
        }
        public void ChamaProgresso(string texto, int max)
        {
            lbprogresso.Text = texto;
            // pnProgresso.Width = lbprogresso.Width + 10;

            lbprogresso.Left = (this.Width / 2) - (lbprogresso.Width / 2);
            lbprogresso.Top = 50;
            lbprogresso.Visible = true;
            Progresso.Maximum = max;
            Progresso.Value = 0;
            this.Update();
        }

        void ProximaNumeracao(TObjetoDesenho obj)
        {
            if (obj.Tipo == Const.ID_TRECHOVIGA)
            {
                if (TrechosVigas.Count == 0)
                    return;

                int max = 0;
                for (int j = 0; j < TrechosVigas.Count; j++)
                    if (TrechosVigas[j].Dados.numero > max)
                        max = TrechosVigas[j].Dados.numero;

                (obj as TTrechoViga).Dados.numero = max + 1;
                (obj as TTrechoViga).Texto1.texto = (obj as TTrechoViga).Dados.nome + (obj as TTrechoViga).Dados.numero;
            }
            if (obj.Tipo == Const.ID_PILAR)
            {
                if (Pilares.Count == 0)
                    return;

                int max = 0;
                for (int j = 0; j < Pilares.Count; j++)
                    if (Pilares[j].Dados.numero > max)
                        max = Pilares[j].Dados.numero;

                (obj as TPilar).Dados.numero = max + 1;
                (obj as TPilar).Texto1.texto = (obj as TPilar).Dados.nome + (obj as TPilar).Dados.numero;
            }

            if (obj.Tipo == Const.ID_LAJE)
            {
                if (Lajes.Count == 0)
                    return;

                int max = 0;
                for (int j = 0; j < Lajes.Count; j++)
                    if (Lajes[j].Dados.numero > max)
                        max = Lajes[j].Dados.numero;

                (obj as TLaje).Dados.numero = max + 1;
                (obj as TLaje).Titulo.texto = (obj as TLaje).Dados.nome + (obj as TLaje).Dados.numero;
            }
        }

        void ConfirmaCota()
        {
            double dist1, dist2;
            ponto1.x = LinhaSnapNearest.pIni.x; ponto1.y = LinhaSnapNearest.pIni.y; ponto1.z = LinhaSnapNearest.pIni.z;
            ponto3.x = LinhaSnapNearest.pFin.x; ponto3.y = LinhaSnapNearest.pFin.y; ponto3.z = LinhaSnapNearest.pFin.z;
            ponto2.x = mousepoint.x; ponto2.y = mousepoint.y; ponto2.z = mousepoint.z;

            ponto1.x = NormalizarCoordenada(ponto1.x);
            ponto1.y = NormalizarCoordenada(ponto1.y);
            ponto1.z = NormalizarCoordenada(ponto1.z);

            ponto2.x = NormalizarCoordenada(ponto2.x);
            ponto2.y = NormalizarCoordenada(ponto2.y);
            ponto2.z = NormalizarCoordenada(ponto2.z);

            ponto3.x = NormalizarCoordenada(ponto3.x);
            ponto3.y = NormalizarCoordenada(ponto3.y);
            ponto3.z = NormalizarCoordenada(ponto3.z);

            dist1 = ponto1.DistanceTo(ponto2);
            dist2 = ponto3.DistanceTo(ponto2);

            //if (ponto1.z > ponto3.z)
            string PontoMaisProximo = "F";
            bool zi_maior_que_zf = false;

            if (dist1 < dist2)
                PontoMaisProximo = "I";

            bool vertical = (Geom.Iguais(ponto1.x - ponto3.x, 0) && Geom.Iguais(ponto1.y - ponto3.y, 0));

            if (!Geom.Iguais(Math.Abs(ponto1.z), Math.Abs(ponto3.z)))
                zi_maior_que_zf = ((ponto1.z * -1) > (ponto3.z * -1));

            double cota = System.Convert.ToSingle(edCota.Text);
            cota = conv.comp(cota,gerenciador.ConfiguracoesPGi.CfgProjeto.unidadesProjeto.geo_un_comprimento.ToString(),und.m);
            double comp_linha= (Math.Sqrt(Math.Pow(LinhaSnapNearest.pIni.x - LinhaSnapNearest.pFin.x, 2) + Math.Pow(LinhaSnapNearest.pIni.y - LinhaSnapNearest.pFin.y, 2) + Math.Pow(LinhaSnapNearest.pIni.z - LinhaSnapNearest.pFin.z, 2)));

            if (PontoMaisProximo == "I" && cota > (comp_linha / 2)/* && (!Geom.Iguais(LinhaSnapNearest.pIni.x, LinhaSnapNearest.pFin.x))*/)
              SubdivideBarra(cota, LinhaSnapNearest.pFin.x, LinhaSnapNearest.pFin.y, LinhaSnapNearest.pFin.z,
                                 LinhaSnapNearest.pIni.x, LinhaSnapNearest.pIni.y, LinhaSnapNearest.pIni.z,null,
                                                                 true,
                                                                 PontoMaisProximo);
            else
              SubdivideBarra(cota, LinhaSnapNearest.pIni.x, LinhaSnapNearest.pIni.y, LinhaSnapNearest.pIni.z,
                                   LinhaSnapNearest.pFin.x, LinhaSnapNearest.pFin.y, LinhaSnapNearest.pFin.z, null,
                                                                     true,
                                                                     PontoMaisProximo);

            /*  else
               SubdivideBarra(System.Convert.ToSingle(edCota.Text),  LinhaSnapNearest.pFin.x, LinhaSnapNearest.pFin.y, LinhaSnapNearest.pFin.z,
                                                                     LinhaSnapNearest.pIni.x, LinhaSnapNearest.pIni.y, LinhaSnapNearest.pIni.z, true);*/
            if (CoordsSubdvisao.Count > 0)
            {
                distCota1 = 0;

                aguardandoCota = false;
                double x = 0, y = 0, z = 0;

                if (zi_maior_que_zf)
                {
                    if (PontoMaisProximo == "F" && vertical)
                    {
                        x = CoordsSubdvisao[CoordsSubdvisao.Count() - 1].x;
                        y = CoordsSubdvisao[CoordsSubdvisao.Count() - 1].y;
                        z = CoordsSubdvisao[CoordsSubdvisao.Count() - 1].z;
                    }
                    else
                    if (PontoMaisProximo == "I" && !vertical)
                    {
                        x = CoordsSubdvisao[0].x;
                        y = CoordsSubdvisao[0].y;
                        z = CoordsSubdvisao[0].z;
                    }
                    else
                    if (PontoMaisProximo == "I" && vertical)
                    {
                        x = CoordsSubdvisao[0].x;
                        y = CoordsSubdvisao[0].y;
                        z = CoordsSubdvisao[0].z;
                    }
                    else
                    if (PontoMaisProximo == "F" && !vertical)
                    {
                        x = CoordsSubdvisao[CoordsSubdvisao.Count() - 1].x;
                        y = CoordsSubdvisao[CoordsSubdvisao.Count() - 1].y;
                        z = CoordsSubdvisao[CoordsSubdvisao.Count() - 1].z;
                    }
                }
                else
                {
                    if (PontoMaisProximo == "I")
                    {
                        if (CoordsSubdvisao.Count == 1)
                        {
                            x =CoordsSubdvisao[0].x;
                            y =CoordsSubdvisao[0].y;
                            z =CoordsSubdvisao[0].z;

                            /* x = LinhaSnapNearest.pFin.x - CoordsSubdvisao[0].x;
                             y = LinhaSnapNearest.pFin.y - CoordsSubdvisao[0].y;
                             z = LinhaSnapNearest.pFin.z - CoordsSubdvisao[0].z;*/
                        }
                        else
                        {
                            x = CoordsSubdvisao[0].x;
                            y = CoordsSubdvisao[0].y;
                            z = CoordsSubdvisao[0].z;
                        }
                    }
                    else
                     if (PontoMaisProximo == "F")
                    {
                        x = CoordsSubdvisao[CoordsSubdvisao.Count() - 1].x;
                        y = CoordsSubdvisao[CoordsSubdvisao.Count() - 1].y;
                        z = CoordsSubdvisao[CoordsSubdvisao.Count() - 1].z;
                    }
                }
                if (Geom.Iguais(x, 0)) x = 0;
                if (Geom.Iguais(y, 0)) y = 0;
                if (Geom.Iguais(z, 0)) z = 0;
 
                if (tipoComando == eTipoComando.draw)
                    MouseDrawing(x, y, z, false);
                else
                if (tipoComando == eTipoComando.edit)
                    MouseEdit(x, y, z, 0, "", 0);
            }
        }

        public bool aguardando_clickUm_Selecao = true;
        public bool aguardandoCota = false;
        List<TLinha> linhas_projecao = new List<TLinha>();

        public double NormalizarCoordenada(double val)
        {
            val = Math.Round(val, 8);

            double inteiro = Math.Round(val);

            if (Math.Abs(val - inteiro) < 1e-8)
                val = inteiro;

            return val;
        }

        public void MouseDrawing(double x, double y, double z, bool addTextos = true)
        {
            try
            {
                point = null;
                string cmd = string.Empty;

                point = new TPonto(x, y, z, pixelX(x), pixelY(y), -1);

                point.x = NormalizarCoordenada(x);
                point.y = NormalizarCoordenada(y);
                point.z = NormalizarCoordenada(z);

                if (ObjetoNovo == null)
                {
                   if (IdObjetoDesenho != Const.ID_TRECHOVIGA)
                        if (LinhaSnapNearest != null)
                            if (!LinhaSnapNearest.auxiliar)
                                LinhaPonto1 = (TLinha)LinhaSnapNearest.Clone();

                    ObjetoNovo = InicializaObjeto(IdObjetoDesenho, ref point);

                    CriaLinhasProjecaoTemporarias(ref point, true, false, null, true);
                }
                else
                {
                    if (ObjetoNovo != null)
                    {
                        ObjetosAdicionados = new List<TObjetoDesenho>();

                        List<TObjetoDesenho> ObjetosResultado = new List<TObjetoDesenho>();

                        eObjetoDesenhoMouseDown result = ObjetoNovo.OnMouseDown(ref point, ref cmd, Linhas, Pontos, Pilares, ref ObjetosResultado);

                        ComandoTexto(cmd, "", false);

                        if (result == eObjetoDesenhoMouseDown.Done || result == eObjetoDesenhoMouseDown.DoneRepeat)
                        {
                            if (ObjetosResultado.Count == 0)
                            {                              
                                if (ObjetoNovo.Tipo == Const.ID_BARRAGENERICA)
                                    AdicionaBarraGenerica(ObjetoNovo as TBarraGenerica);
                                else
                                    AdicionaObjeto(ObjetoNovo, PavimentoAtual, addTextos);

                                ObjetosAdicionados.Add(ObjetoNovo);
                            }
                            else
                            {
                                foreach (TObjetoDesenho obj in ObjetosResultado)
                                {
                                    AdicionaObjeto(obj, PavimentoAtual, addTextos);
                                    ObjetosAdicionados.Add(obj);
                                }
                              //  RemoveSelecionados();
                            }

                       //     Atualiza_pIni_pFin_das_Barras_e_Cargas();

                            if (IdObjetoDesenho == Const.ID_PILAR)
                                RefazContornoLajes();
                        }

                        if (result == eObjetoDesenhoMouseDown.DoneRepeat)
                        {
                            mousepoint.x = ObjetoNovo.pFin.x;
                            mousepoint.y = ObjetoNovo.pFin.y;
                            mousepoint.z = ObjetoNovo.pFin.z;

                            if (ObjetoNovo.Tipo == Const.ID_BARRAGENERICA)
                            {
                                LinhaInserida = (TLinha)(ObjetoNovo as TBarraGenerica).Linha_Eixo.Clone();

                                if (LinhaSnapNearest != null)
                                    if (!LinhaSnapNearest.auxiliar)
                                        LinhaPonto3 = (TLinha)LinhaSnapNearest.Clone();
                            }

                            LinhaSnapNearest = null;
                            snapNearest = false;
                            // AtualizaElementosEstrutura();

                            if (IdObjetoDesenho == Const.ID_PILAR)
                                NovosDadosPilar();
                            else
                            if (IdObjetoDesenho == Const.ID_TRECHOVIGA)
                                NovosDadosViga(false);
                            else
                            if (IdObjetoDesenho == Const.ID_BARRAGENERICA)
                               NovosDadosBarra(true);

                            //   RemoveSelecionados();

                            CriaLinhasProjecaoTemporarias(ref point, true,false, ObjetoNovo as TBarraGenerica);

                            ObjetoNovo = null;
                            ObjetoNovo = InicializaObjeto(IdObjetoDesenho, ref point);
                            comandosMover = new List<ComandoMover>();


                             //AtualizaElementosEstrutura();
                          //  AtualizaListaSnap();
                        //    CancelaResultados();
                            AtualizaUltimaBarra();

                         //   if (IdObjetoDesenho == Const.ID_BARRAGENERICA)
                          //      Atualiza_Segmentos_e_Snap();

                            if (CasoCargaAtual != null)
                            {
                                if (CasoCargaAtual.ID == 1)
                                    MostrarCargasPeloCaso();
                            }
                            else
                            if (gerenciador.cbCasoCarga.SelectedIndex == gerenciador.cbCasoCarga.Items.Count - 1) //<TODOS>
                                MostrarCargasPeloCaso();
                        }
                        else
                        if (result == eObjetoDesenhoMouseDown.Continue)
                           ObjetoNovo.Continue();
                        else
                        if (result == eObjetoDesenhoMouseDown.Done)
                        {
                            comandosMover = new List<ComandoMover>();

                            if (LinhaSnapNearest != null)
                                LinhaPonto3 = LinhaSnapNearest;

                            ObjetoNovo = null;
                            tipoComando = eTipoComando.selecionar;

                            if (IdObjetoDesenho == Const.ID_LAJE)
                                NovosDadosLaje();

                            if (IdObjetoDesenho == Const.ID_BARRAGENERICA)
                                Atualiza_Segmentos_e_Snap();
                        }
                    }
                  //  AtualizaConfiguracoes3D();

                    if (undoBuffer.CanCapture)
                    {
                        undoBuffer.AdicionaComando(new ComandoAdicionar(ObjetosAdicionados, comandosMover));
                        comandosMover.Clear();
                        ObjetosAdicionados.Clear();
                    }
                }

                NovaCoordZ = false;
                NovaCoordX = false;
                NovaCoordY = false;
            }
            catch (Exception e)
            {
                MessageBox.Show("Erro mousedrawing: " + e.Message);
            }

        }
        public TLinha LinhaPonto1, LinhaInserida, LinhaPonto3;

        void RefazContornoLajes()
        {
            TPilar pilar = Pilares[Pilares.Count - 1];
            List<TLaje> lajes = new List<TLaje>();

            /* foreach(TTrechoViga trecho in TrechosVigas)
             {
                 if (trecho.NumPilar_PontoInicial == pilar.Dados.numero || trecho.NumPilar_PontoFinal == pilar.Dados.numero)
                 {
                     foreach (TLaje laje in Lajes)
                     {
                         foreach (TLinha linha in laje.poligono.linhas_poligonal)
                         {
                            TTrechoViga tr = linha.TrechoViga;
                            if (tr.ID == trecho.ID)
                            {
                                lajes.Add(laje);
                                break;
                            }
                         }
                     }
                 }
             }*/

        }

        bool PilarGerado(int num)
        {
            foreach (int i in PilaresGerados)
                if (i == num)
                    return true;

            return false;
        }
        List<int> PilaresGerados;
        void AtualizaAlturasPilar()
        {
            try
            {
                /*   SalvaPavimentoAtual();

                   List<TPilar> Pilar = new List<TPilar>();
                   PilaresGerados = new List<int>();
                   int kk;

                   if (gerenciador.Pavimentos[0].pilares.Count == 0)
                       return;

                   int numAtual = gerenciador.Pavimentos[0].pilares[0].Dados.numero;
                   int cc = -1;
                   List<TPilar> pilares = new List<TPilar>();

                   foreach (TPavimento pav in gerenciador.Pavimentos)
                       for (int i = 0; i < pav.pilares.Count(); i++)
                       {
                           pav.pilares[i].AlturaAbaixo = 0;
                           pav.pilares[i].AlturaAcima = 0;
                           pilares.Add(pav.pilares[i]);
                       }

                   for (int i = 0; i < pilares.Count(); i++)
                   {
                       cc++;
                       numAtual = pilares[i].Dados.numero;

                       if (!PilarGerado(pilares[i].Dados.numero))
                       {
                           foreach (TPilar p1 in pilares)
                           {
                               if (numAtual == p1.Dados.numero)
                               {
                                   Pilar.Add(p1);
                                   numAtual = p1.Dados.numero;
                               }
                           }

                           PilaresGerados.Add(numAtual);

                           TPilar pil;
                           for (kk = 0; kk < Pilar.Count; kk++)
                           {
                               for (int j = kk; j < Pilar.Count; j++)
                               {
                                   if (Pilar[j].Pavimento.Nivel < Pilar[kk].Pavimento.Nivel)
                                   {
                                       pil = Pilar[kk];
                                       Pilar[kk] = Pilar[j];
                                       Pilar[j] = pil;
                                   };
                               };
                           };

                           for (cc = 0; cc < Pilar.Count; cc++)
                           {
                               pil = Pilar[cc];

                               if (cc == Pilar.Count - 1)
                               {
                                   Pilar[cc].Morre = true;
                                   Pilar[cc].Passa = false;
                                   Pilar[cc].Nasce = false;
                               }
                               else
                               if (cc == 0)
                               {
                                   Pilar[cc].Morre = false;
                                   Pilar[cc].Passa = false;
                                   Pilar[cc].Nasce = true;
                               }
                               else
                               {
                                   Pilar[cc].Morre = false;
                                   Pilar[cc].Passa = true;
                                   Pilar[cc].Nasce = false;
                               }

                               if (Pilar[cc].Pavimento.SequenciaPavimento > 1)
                                   Pilar[cc].AlturaLance = gerenciador.Pavimentos[Pilar[cc].Pavimento.SequenciaPavimento - 2].PeDireito;

                               if (Pilar[cc].Passa || Pilar[cc].Nasce && (cc  < Pilar.Count))
                               {
                                   if (cc + 1 >= Pilar.Count)
                                       MessageBox.Show("");

                                   pil.AlturaAcima = Pilar[cc + 1].Pavimento.PeDireito;
                               }

                               if (Pilar[cc].Passa || Pilar[cc].Morre)
                                   pil.AlturaAbaixo = Pilar[cc].Pavimento.PeDireito;
                           }

                           Pilar.Clear();
                       }
                   }*/
            }
            catch (Exception em)
            {
                MessageBox.Show("erro atualizaalturaspilar :" + em.Message);
            }
        }

        public void AtualizaPilares(bool ComTexto = false)
        {
            /*  foreach (TPavimento pav in gerenciador.Pavimentos)
                  foreach (TPilar Pilar in pav.pilares)
                  {
                      Pilar.Morre = false;
                      Pilar.Nasce = false;
                      Pilar.Passa = false; 

                      if (pav.SequenciaPavimento > 1)
                      {
                          Pilar.AlturaLance = gerenciador.Pavimentos[pav.SequenciaPavimento - 2].PeDireito;

                          if (pav.SequenciaPavimento == (gerenciador.Pavimentos.Count()))
                            Pilar.Nasce = true;
                          else
                            Pilar.Passa = true;  
                      }
                      else
                      if (pav.SequenciaPavimento == 1)
                          Pilar.Morre = true;

                      if (ComTexto)
                      {
                          if (Pilar.Nasce)
                              Pilar.Texto1.texto = Pilar.Dados.nome + Pilar.Dados.numero + " (Nasce)";
                          if (Pilar.Passa)
                              Pilar.Texto1.texto = Pilar.Dados.nome + Pilar.Dados.numero + " (Passa)";
                          if (Pilar.Morre)
                              Pilar.Texto1.texto = Pilar.Dados.nome + Pilar.Dados.numero + " (Morre)";
                      }
                  }*/

            AtualizaAlturasPilar();
        }

        void AtualizaSegmentacoes(bool SegmentaSomenteVigas = false, bool MouseEdit = false)
        {
            /* TIntersecoes.DesfazIntersecoesTrechos(ref TrechosVigas);
             TIntersecoes.SegmentaTrechosPelosPilares(PavimentoAtual, this, ref TrechosVigas, ref Pilares, ref Linhas);
             TIntersecoes.CalculaIntersecoesTrechos(ref TrechosVigas, ref Pilares);
             TIntersecoes.RemoveTrechosSobrepostos(ref TrechosVigas, this);*/

         //   if (!SegmentaSomenteVigas)
             //   TIntersecoes.CalculaIntersecoesBarras(Estrutura.barras, Estrutura.cargaLinear, ref Linhas, this, MouseEdit);
        }

        public void Atualiza_Segmentos_e_Snap(bool SegmentaSomenteVigas = false, bool MouseEdit = false)
        {
            AtualizaSegmentacoes(SegmentaSomenteVigas, MouseEdit);
            // AtualizaPilares();
            AtualizaListaSnap();
            CancelaResultados();
            AtualizaBarras();
            // CalculaCentro();
        }


        //função para remover objetos que o OnMouseDown do objeto novo selecionou para deletar
        public void RemoveSelecionados()
        {
            Barras.RemoveAll(obj => obj.Selecionado);
            TrechosVigas.RemoveAll(obj => obj.Selecionado);
            Textos.RemoveAll(obj => obj.Selecionado);
            Linhas.RemoveAll(obj => obj.Selecionado);
            Grips.RemoveAll(obj => obj.Selecionado);
            Lajes.RemoveAll(obj => obj.Selecionado);
            CargasPontuais.RemoveAll(obj => obj.Selecionado);
            CargasLineares.RemoveAll(obj => obj.Selecionado);
            Pilares.RemoveAll(obj => obj.Selecionado);
            Pontos.RemoveAll(obj => obj.Selecionado);
            Circulos.RemoveAll(obj => obj.Selecionado);
            // if (ActiveLayer != null)
            //  ActiveLayer.Obj.RemoveAll(obj => obj.Selecionado);

            Estrutura.vigas.RemoveAll(vigas => vigas.Selecionado);
            Estrutura.pilares.RemoveAll(pilares => pilares.Selecionado);
            Estrutura.lajes.RemoveAll(lajes => lajes.Selecionado);
            Estrutura.barras.RemoveAll(barras => barras.Selecionado);
            Estrutura.cargaLinear.RemoveAll(cl => cl.Selecionado);
            Estrutura.cargaPontual.RemoveAll(cp => cp.Selecionado);

            Estrutura.nos.RemoveAll(nos => nos.Selecionado);
            Estrutura.apoios.RemoveAll(barras => barras.Selecionado);

            AtualizaListaSnap();
            Estrutura.AtualizaListaObjetos();
            /*     pav.LayersByIdPrincipal[Lay.TextosVigas].Obj.RemoveAll(obj => obj.Selecionado);
                 pav.LayersByIdPrincipal[Lay.TextosPilares].Obj.RemoveAll(obj => obj.Selecionado);
                 pav.LayersByIdPrincipal[Lay.TextosLajes].Obj.RemoveAll(obj => obj.Selecionado);
                 pav.LayersByIdPrincipal[Lay.Vigas].Obj.RemoveAll(obj => obj.Selecionado);
                 pav.LayersByIdPrincipal[Lay.Lajes].Obj.RemoveAll(obj => obj.Selecionado);
                 pav.LayersByIdPrincipal[Lay.Pilares].Obj.RemoveAll(obj => obj.Selecionado);
                 pav.LayersByIdPrincipal[Lay.ElementosBasicos].Obj.RemoveAll(obj => obj.Selecionado);*/
        }

        public void ProcuraComandoXObjeto(Keys key)
        {
            if (ObjetoNovo != null)
            {
                if (IdObjetoDesenho == Const.ID_PILAR)
                {
                    if (key == Keys.A)
                    {
                        (ObjetoNovo as TPilar).AlternaVertice();
                        gerenciador.DigitarComando.Clear();
                    }
                }
                if (IdObjetoDesenho == Const.ID_TRECHOVIGA)
                {
                    if (key == Keys.A)
                    {
                        (ObjetoNovo as TTrechoViga).AlternaFace();
                        gerenciador.DigitarComando.Clear();
                    }
                }
            }

            DesenhaObjetos();
            glControl.SwapBuffers();

        }

        void AlternaVerticePilar()
        {
            if (IdObjetoDesenho == Const.ID_PILAR && ObjetoNovo != null)
                (ObjetoNovo as TPilar).AlternaVertice();
            DesenhaObjetos();
            glControl.SwapBuffers();
        }

        public void AddLinha(TLinha lin, int Pav,
            bool visivel = true,
            bool auxiliar = false,
            bool LinhaTipoViga = false,
            Dictionary<string, TLayer> LayersById = null,
            int IDTrecho = -1,
            bool ObjetoPrimario = true)
        {
            if (lin == null)
                return;

            Linhas.Add(lin);
            Linhas[Linhas.Count - 1].Visivel = visivel;
            Linhas[Linhas.Count - 1].auxiliar = auxiliar;
            Linhas[Linhas.Count - 1].IDTrecho = IDTrecho;
            Linhas[Linhas.Count - 1].LinhaTipoViga = LinhaTipoViga;
            lin.ID = Linhas.Count;
            lin.ObjetoPrimario = ObjetoPrimario;
            /*
                        if (LayersById == null)
                        {
                            if (lin.layer.Grupo == GrupoLay.Arquitetura)
                                Estrutura.LayersByIdArquitetura[lin.layer.nome].AddObject(lin);
                            else
                            if (lin.layer.Grupo == GrupoLay.Principal)
                                Estrutura.LayersByIdPrincipal[lin.layer.nome].AddObject(lin);               
                         //   PavimentoAtual.LayersByIdPrincipal[lin.layer.nome].AddObject(lin);
                        }
                        else
                            LayersById[lin.layer.nome].AddObject(lin); */

            //   if (layer == null)
            //       ActiveLayer.AddObject(lin);
            //    else
            //         layer.AddObject(lin);
        }
        private void AddCirculo(ref TCirculo circ, int Pav, TLayer layer = null, bool ObjetoPrimario = true)
        {
            if (circ == null)
                return;

            Circulos.Add(circ);
            circ.ID = Circulos.Count;
            circ.ObjetoPrimario = ObjetoPrimario;

            /* if (layer == null)
             {
                 if (circ.layer.Grupo == GrupoLay.Arquitetura)
                     Estrutura.LayersByIdArquitetura[circ.layer.nome].AddObject(circ);
                 else
                 if (circ.layer.Grupo == GrupoLay.Principal)
                     Estrutura.LayersByIdPrincipal[circ.layer.nome].AddObject(circ); 
                 //AveLayer.AddObject(circ);
             }
             else
                 layer.AddObject(circ);*/
        }

        void AddGrip(TGrip grip, int Pav, Dictionary<string, TLayer> LayersById = null, bool ObjetoPrimario = true)
        {
            if (grip == null)
                return;

            Grips.Add(grip);
            grip.ID = Grips.Count;
            grip.ObjetoPrimario = ObjetoPrimario;

            grip.mPen = new System.Drawing.Pen(System.Drawing.Color.White);
            grip.mBrush = new System.Drawing.SolidBrush(System.Drawing.Color.Blue);
            grip.points = new System.Drawing.Point[4];
            /*
                        if (LayersById == null)
                        {
                            if (grip.layer.Grupo == GrupoLay.Arquitetura)
                                Estrutura.LayersByIdArquitetura[grip.layer.nome].AddObject(grip);
                            else
                            if (grip.layer.Grupo == GrupoLay.Principal)
                                Estrutura.LayersByIdPrincipal[grip.layer.nome].AddObject(grip); 
                        }
                        else
                            LayersById[grip.layer.nome].AddObject(grip);*/
        }

        void AddTexto(ref TTexto texto, int Pav, Dictionary<string, TLayer> LayersById = null, bool ObjetoPrimario = true)
        {
            if (texto == null)
                return;
            Textos.Add(texto);
            texto.ID = Textos.Count;
            texto.ObjetoPrimario = ObjetoPrimario;

            AddGrip(texto.Grips[0], Pav, null, false);
            AddGrip(texto.Grips[1], Pav, null, false);
            AddGrip(texto.Grips[2], Pav, null, false);

            /*  if (LayersById == null)
              {
                  if (texto.layer.Grupo == GrupoLay.Arquitetura)
                      Estrutura.LayersByIdArquitetura[texto.layer.nome].AddObject(texto);
                  else
                  if (texto.layer.Grupo == GrupoLay.Principal)
                      Estrutura.LayersByIdPrincipal[texto.layer.nome].AddObject(texto); 

              //    PavimentoAtual.LayersByIdPrincipal[texto.layer.nome].AddObject(texto);
              }
              else
                  LayersById[texto.layer.nome].AddObject(texto);*/
        }
        public bool AbrindoArquivo;
        public TEstrutura Estrutura;
        public void InsereNos()
        {
            foreach (TBarraGenerica barra in Estrutura.barras)
            {
            //    if (LocalizaPonto(ref Estrutura.nos, barra.pIni.x, barra.pIni.y, barra.pIni.z) == -1)
                    Estrutura.nos.Add(barra.pIni);
              //  if (LocalizaPonto(ref Estrutura.nos, barra.pFin.x, barra.pFin.y, barra.pFin.z) == -1)
                    Estrutura.nos.Add(barra.pFin);
            }
        }

        public void AdicionaBarraGenerica(TBarraGenerica barra, bool copiando = false)
        {
            double comp = barra.pIni.DistanceTo(barra.pFin);
            if (comp > 0)
            {
                // Barras[Barras.Count - 1].RotacionaSecao();
                //     Barras[Barras.Count - 1].CriaPesoProprio();

                barra.DirtyTriangulos = true;
                barra.DirtyArestas    = true;
                barra.DirtySelecao = true;

                Estrutura.barras.Add(barra);
                Barras.Add(barra);
                barra.IDBarra = Estrutura.barras.Max(o => o.IDBarra) + 1;

                //  AddPoint(ref (barra).Linha_Eixo.pIni);
                //  AddPoint(ref (barra).Linha_Eixo.pFin, "f");
                AddLinha(barra.Linha_Eixo, 0, false, false, false, null, -1, false);
                barra.Linha_Eixo.Selecionado = false;
                Linhas[Linhas.Count - 1].IDBarra = barra.IDBarra;
                Linhas[Linhas.Count - 1].Barra = barra;

                //if (barra.Dados.Tipo != 4)
                {
                    //adiciona peso proprio
                    Estrutura.cargaLinear.Add(Estrutura.barras[Estrutura.barras.Count - 1].PesoProprio as TCargaLinear);
                    CargasLineares.Add(Estrutura.cargaLinear[Estrutura.cargaLinear.Count - 1]);
                    (Estrutura.barras[Estrutura.barras.Count - 1].PesoProprio as TCargaLinear).ID = Estrutura.cargaLinear.Max(o => o.ID) + 1;

                    //    AdicionaObjeto(Estrutura.barras[Estrutura.barras.Count - 1].PesoProprio as TCargaLinear, -1, false, false);
                    Estrutura.cargaLinear[Estrutura.cargaLinear.Count - 1].idBarra = Estrutura.barras[Estrutura.barras.Count - 1].IDBarra;
                    Estrutura.cargaLinear[Estrutura.cargaLinear.Count - 1].Dados = (TDadosCarga)Estrutura.barras[Estrutura.barras.Count - 1].PesoProprio.Dados.Clone();

                    //  AtualizaPontos_Barras_e_Cargas();

                    Estrutura.cargaLinear[Estrutura.cargaLinear.Count - 1].anguloRotacao = Estrutura.barras[Estrutura.barras.Count - 1].Dados.anguloRotacao;
                    Estrutura.cargaLinear[Estrutura.cargaLinear.Count - 1].CriaSetas(true, 0, 0, fatorCarga);
                }
                // if (LocalizaPonto(ref Estrutura.nos, barra.pIni.x, barra.pIni.y, barra.pIni.z) == -1)
                Estrutura.nos.Add(barra.pIni);
                //   if (LocalizaPonto(ref Estrutura.nos, barra.pFin.x, barra.pFin.y, barra.pFin.z) == -1)
                Estrutura.nos.Add(barra.pFin);

                barra.pIni.BarrasConectadas.Add(barra);
                barra.pFin.BarrasConectadas.Add(barra);

                Estrutura.AtualizaListaObjetos();
            }
            /*   TBarraGenerica bg;
               foreach (TCargaLinear o in Estrutura.cargaLinear)
               {
                   bg = Estrutura.barras.Find(b => b.IDBarra == o.idBarra);
                   if (bg != null)
                   {
                       o.pIni.x = bg.Linha_Eixo.pIni.x;
                       o.pIni.y = bg.Linha_Eixo.pIni.y;
                       o.pIni.z = bg.Linha_Eixo.pIni.z;

                       o.pFin.x = bg.Linha_Eixo.pFin.x;
                       o.pFin.y = bg.Linha_Eixo.pFin.y;
                       o.pFin.z = bg.Linha_Eixo.pFin.z;
                   }
               }**/

            //  if (m_undoBuffer.CanCapture)
            //  m_undoBuffer.AdicionaComando(new ComandoAdicionar(barra));
        }

        public void AdicionaObjeto(TObjetoDesenho obj, int Pav, bool addTextos = true, bool AdicionaNoBufferUndo = true, bool atualizaShader = true)
        {
            /*Essa rotina adiciona os objetos que podem ser capturáveis*/
            try
            {
                Alterou(true);

                obj.ObjetoPrimario = true;

                //   if (obj.layer.Grupo == GrupoLay.Arquitetura)
                //       Estrutura.LayersByIdArquitetura[obj.layer.nome].AddObject(obj);
                //   else
                //    if (obj.layer.Grupo == GrupoLay.Principal)
                //         Estrutura.LayersByIdPrincipal[obj.layer.nome].AddObject(obj);

                if (gerenciador.ModoPiso)
                    obj.Piso = gerenciador.cbPiso.SelectedIndex;
                else
                    obj.Piso = -1;

                if (obj.Tipo == Const.ID_BARRAGENERICA)
                {
                    Estrutura.barras.Add(obj as TBarraGenerica);
                    Barras.Add(obj as TBarraGenerica);

                    (obj as TBarraGenerica).IDBarra = Estrutura.barras.Max(o => o.IDBarra) + 1;

                    AddPoint(ref ((obj as TBarraGenerica).Linha_Eixo.pIni));
                    AddPoint(ref ((obj as TBarraGenerica).Linha_Eixo.pFin), "f");
                    AddLinha((obj as TBarraGenerica).Linha_Eixo, Pav, false, false, false, null, -1, false);
                    Linhas[Linhas.Count - 1].IDBarra = (obj as TBarraGenerica).IDBarra;
                    Linhas[Linhas.Count - 1].Barra = (obj as TBarraGenerica);

                    //adiciona peso proprio
                    CargasLineares.Add((obj as TBarraGenerica).PesoProprio);
                    Estrutura.cargaLinear.Add((obj as TBarraGenerica).PesoProprio);
                    (obj as TBarraGenerica).PesoProprio.ID = Estrutura.cargaLinear.Max(o => o.ID) + 1;
                    Estrutura.cargaLinear[Estrutura.cargaLinear.Count - 1].idBarra = (obj as TBarraGenerica).IDBarra;
                    Estrutura.cargaLinear[Estrutura.cargaLinear.Count - 1].Dados = (TDadosCarga)(obj as TBarraGenerica).PesoProprio.Dados.Clone();
                    Estrutura.cargaLinear[Estrutura.cargaLinear.Count - 1].anguloRotacao = (obj as TBarraGenerica).Dados.anguloRotacao;
                    Estrutura.cargaLinear[Estrutura.cargaLinear.Count - 1].CriaSetas(true, 0, 0, fatorCarga);

                    /*TBarraGenerica bg;
                    foreach (TCargaLinear o in CargasLineares)
                    {
                        bg = Estrutura.barras.Find(b => b.IDBarra == o.idBarra);
                        if (bg != null)
                        {
                            o.pIni.x = bg.Linha_Eixo.pIni.x;
                            o.pIni.y = bg.Linha_Eixo.pIni.y;
                            o.pIni.z = bg.Linha_Eixo.pIni.z;

                            o.pFin.x = bg.Linha_Eixo.pFin.x;
                            o.pFin.y = bg.Linha_Eixo.pFin.y;
                            o.pFin.z = bg.Linha_Eixo.pFin.z;
                        }
                    }*/

                    Atualiza_pIni_pFin_das_Barras_e_Cargas();
                }
                else
                if (obj.Tipo == Const.ID_CARGA_PONTUAL)
                {
                    CargasPontuais.Add(obj as TCargaPontual);
                    Estrutura.cargaPontual.Add(obj as TCargaPontual);
                    (obj as TCargaPontual).ID = Estrutura.cargaPontual.Max(o => o.ID) + 1;
                }
                else
                if (obj.Tipo == Const.ID_CARGA_LINEAR)
                {
                    Estrutura.cargaLinear.Add(obj as TCargaLinear);
                    CargasLineares.Add(Estrutura.cargaLinear[Estrutura.cargaLinear.Count - 1]);
                    (obj as TCargaLinear).ID = Estrutura.cargaLinear.Max(o => o.ID) + 1;
                    Estrutura.cargaLinear[Estrutura.cargaLinear.Count - 1].CriaSetas(true, 0, 0, fatorCarga);
                }
                else
                if (obj.Tipo == Const.ID_APOIO)
                {
                    Estrutura.apoios.Add(obj as TApoio);
                    (obj as TApoio).IDApoio = Estrutura.apoios.Max(o => o.IDApoio) + 1;
                    //  apo.Add(obj as TBarraGenerica);
                    // (obj as TBarraGenerica).IDBarra = Barras.Count;

                    /*   AddPoint(ref ((obj as TBarraGenerica).Linha_Eixo.pIni));
                       AddPoint(ref ((obj as TBarraGenerica).Linha_Eixo.pFin), "f");
                       AddLinha((obj as TBarraGenerica).Linha_Eixo, Pav, false, false, false, null, -1, false);
                       Linhas[Linhas.Count - 1].IDBarra = (obj as TBarraGenerica).IDBarra;
                       Linhas[Linhas.Count - 1].Barra = (obj as TBarraGenerica);*/
                }
                else
                if (obj.Tipo == Const.ID_TRECHOVIGA)
                {
                    Estrutura.vigas.Add(obj as TTrechoViga);

                    // no caso do Pav ser outro pavimento, nao pode adcionar na lista, pois a lista so vai os objetos do Pavimento Atual
                    // if ((Object)Pav == (Object)PavimentoAtual)
                    // if (!AbrindoArquivo)
                    TrechosVigas.Add(obj as TTrechoViga);

                    (obj as TTrechoViga).UpdateTrechosDasLinhas();
                    (obj as TTrechoViga).ID = Estrutura.vigas.Max(o => o.ID) + 1;

                    AddPoint(ref ((obj as TTrechoViga).linhas_facebaixo.pIni));
                    AddPoint(ref ((obj as TTrechoViga).linhas_facebaixo.pFin), "f");
                    AddPoint(ref ((obj as TTrechoViga).linhas_eixo.pIni), "i", true);
                    AddPoint(ref ((obj as TTrechoViga).linhas_eixo.pFin), "f", true);
                    AddPoint(ref ((obj as TTrechoViga).linhas_facecima.pIni));
                    AddPoint(ref ((obj as TTrechoViga).linhas_facecima.pFin), "f");
                    //    AddPoint(ref ((obj as TTrechoViga).linhas_faceeixo_longa_m2.pIni), "i", false, true, false);
                    //   AddPoint(ref ((obj as TTrechoViga).linhas_faceeixo_longa_m2.pFin), "f", false, true, false);
                    //   AddPoint(ref ((obj as TTrechoViga).linhas_faceeixo_longa_m1.pIni), "i", false, true, false);
                    //  AddPoint(ref ((obj as TTrechoViga).linhas_faceeixo_longa_m1.pFin), "f", false, true, false);

                    //   AddLinha((obj as TTrechoViga).linhas_faceeixo_longa_m1, Pav, false, true, false, null, (obj as TTrechoViga).ID, false);
                    //   AddLinha((obj as TTrechoViga).linhas_faceeixo_longa_m2, Pav, false, true, false, null, (obj as TTrechoViga).ID, false);
                    AddLinha((obj as TTrechoViga).linhas_facecima, Pav, false, false, true, null, (obj as TTrechoViga).ID, false);
                    AddLinha((obj as TTrechoViga).linhas_eixo, Pav, false, false, true, null, (obj as TTrechoViga).ID, false);
                    AddLinha((obj as TTrechoViga).linhas_facebaixo, Pav, false, false, true, null, (obj as TTrechoViga).ID, false);

                    if (addTextos)
                    {
                        AddTexto(ref (obj as TTrechoViga).Texto1, Pav, null, false);
                        AddTexto(ref (obj as TTrechoViga).Texto2, Pav, null, false);
                    }

                    AddGrip((obj as TTrechoViga).Grips[0], Pav, null, false);
                    AddGrip((obj as TTrechoViga).Grips[1], Pav, null, false);
                }
                else
                if (obj.Tipo == Const.ID_LAJE)
                {
                    // no caso do Pav ser outro pavimento, nao pode adcionar na lista, pois a lista so vai os objetos do Pavimento Atual
                    // if ((Object)Pav == (Object)PavimentoAtual) 
                    Lajes.Add(obj as TLaje);

                    (obj as TLaje).ID = Estrutura.lajes.Max(o => o.ID) + 1;

                    AddTexto(ref (obj as TLaje).Titulo, Pav, null, false);

                    AddGrip((obj as TLaje).Grips[0], Pav, null, false);

                    AddTexto(ref (obj as TLaje).Titulo2, Pav, null, false);
                }
                else

                if (obj.Tipo == Const.ID_PILAR)
                {
                    Estrutura.pilares.Add(obj as TPilar);
                    // no caso do Pav ser outro pavimento, nao pode adcionar na lista, pois a lista so vai os objetos do Pavimento Atual
                    //  if ((Object)Pav == (Object)PavimentoAtual)
                    Pilares.Add(obj as TPilar);
                    (obj as TPilar).ID = Estrutura.pilares.Max(o => o.ID) + 1;

                    for (int i = 0; i < (obj as TPilar).Dados.Poligono.linhas_poligonal.Count; i++)
                        AddLinha((obj as TPilar).Dados.Poligono.linhas_poligonal[i], Pav, true, false, false, null, -1, false);

                    for (int i = 0; i < (obj as TPilar).VerticesNosVazios.Count; i++)
                        AddLinha((obj as TPilar).VerticesNosVazios[i], Pav, true, false, false, null, -1, false);

                    for (int i = 0; i < (obj as TPilar).Vertices.Count - 1; i++)
                        AddPoint(((obj as TPilar).Vertices[i]));

                    for (int i = 0; i < (obj as TPilar).Grips.Count; i++)
                        AddGrip((obj as TPilar).Grips[i], Pav, null, false);

                    AddLinha((obj as TPilar).LinhaEixo, Pav, true, false, false, null, -1, false);

                    if (addTextos)
                    {
                        AddTexto(ref (obj as TPilar).Texto1, Pav, null, false);

                        AddGrip((obj as TPilar).Texto1.Grips[0], Pav, null, false);

                        AddTexto(ref (obj as TPilar).Texto2, Pav, null, false);
                    }
                }
                else
                if (obj.Tipo == Const.ID_LINHA)
                {
                    // no caso do Pav ser outro pavimento, nao pode adcionar na lista, pois a lista so vai os objetos do Pavimento Atual
                    //     if ((Object)Pav == (Object)PavimentoAtual)
                    Linhas.Add(obj as TLinha);
                    (obj as TLinha).ID = Linhas.Max(o => o.ID) + 1;

                    AddPoint(ref ((obj as TLinha).pIni));
                    AddPoint(ref ((obj as TLinha).pFin), "f");

                    AddGrip((obj as TLinha).Grips[0], Pav, null, false);
                    AddGrip((obj as TLinha).Grips[1], Pav, null, false);
                    AddGrip((obj as TLinha).Grips[2], Pav, null, false);
                }
                else
                if (obj.Tipo == Const.ID_CIRCULO)
                {
                    // no caso do Pav ser outro pavimento, nao pode adcionar na lista, pois a lista so vai os objetos do Pavimento Atual
                    //   if ((Object)Pav == (Object)PavimentoAtual)
                    Circulos.Add(obj as TCirculo);
                    (obj as TCirculo).ID = Circulos.Count;

                    AddGrip((obj as TCirculo).Grips[0], Pav, null, false);
                }

                Estrutura.AtualizaListaObjetos();

                if (atualizaShader)
                  AtualizaShaders();

             //   if (AdicionaNoBufferUndo && m_undoBuffer.CanCapture)
              //      m_undoBuffer.AdicionaComando(new ComandoAdicionar(obj));
            }
            catch (Exception e)
            {
                MessageBox.Show(e.Message, "Erro na inserção do objeto : ", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        struct sPilarXPontos
        {
            public TPilar pilar;
            public TTrechoViga trecho;
            public List<TPonto> PontosIntersecao;
            // public TTrechoViga trecho;
            public sPilarXPontos(TPilar pil, TTrechoViga trech)
            {
                pilar = pil;
                trecho = trech;
                PontosIntersecao = new List<TPonto>();
            }
        }
        public void AddPoint(ref TPonto pt, string Posicao = "i", bool PontoEixo = false, bool Auxiliar = false, bool enquadrar = true)
        {
            pt.px_x = pixelX(pt.x);
            pt.px_y = pixelY(pt.y);
            pt.codigo = Pontos.Count;
            pt.Enquadrar = enquadrar;

            pt.PontoInicial = false;
            pt.PontoFinal = false;
            pt.PontoEixoViga = PontoEixo;

            if (Posicao == "i")
                pt.PontoInicial = true;
            if (Posicao == "f")
                pt.PontoFinal = true;

            pt.PontoAuxiliar = Auxiliar;

            pt.x = NormalizarCoordenada(pt.x);
            pt.y = NormalizarCoordenada(pt.y);
            pt.z = NormalizarCoordenada(pt.z);

            Pontos.Add(pt);
        }

        public void AddPoint(TPonto pt, string Posicao = "i", bool PontoEixo = false, bool Auxiliar = false, bool enquadrar = true)
        {
            pt.px_x = pixelX(pt.x);
            pt.px_y = pixelY(pt.y);
            pt.codigo = Pontos.Count;
            pt.Enquadrar = enquadrar;

            pt.PontoInicial = false;
            pt.PontoFinal = false;
            pt.PontoEixoViga = PontoEixo;

            if (Posicao == "i")
                pt.PontoInicial = true;
            if (Posicao == "f")
                pt.PontoFinal = true;

            pt.PontoAuxiliar = Auxiliar;

            pt.x = NormalizarCoordenada(pt.x);
            pt.y = NormalizarCoordenada(pt.y);
            pt.z = NormalizarCoordenada(pt.z);

            Pontos.Add(pt);
        }

        public void Alterou(bool f, bool geometria = true)
        {
            gerenciador.alterado = f;

            if (geometria)
            {
                gerenciador.necessitaCalculo = f;
                gerenciador.CancelaResultados();
            }
        }

        private void MouseGrip(double x, double y)
        {
            Alterou(true);

            List<TObjetoDesenho> ResultObjects = new List<TObjetoDesenho>();
            string cmd = string.Empty;
            eObjetoDesenhoMouseDown result = GripAtual.OnMouseDown(ref point, ref cmd, Linhas, Pontos, Pilares, ref ResultObjects, true);
            GripAtual.movendo = true;
            if (!GripAtual.translacao)
            {
                if (ResultObjects.Count > 0)
                {
                    //    ResultObjects[0].Layer.Obj.RemoveAll(obj => obj.Selecionado);

                    foreach (TObjetoDesenho obj in ResultObjects)
                    {
                        // obj.Layer.AddObject(obj);
                        AdicionaObjeto(obj, PavimentoAtual);
                    }

                  //  RemoveSelecionados();
                }
            }

            GripAtual.movendo = false;

            SetaSelecionados(false, PavimentoAtual);

            // if (GripAtual.ObjetoDesenho.Tipo == Const.ID_TRECHOVIGA || GripAtual.ObjetoDesenho.Tipo == Const.ID_PILAR)
            Atualiza_Segmentos_e_Snap();

            GripAtual = null;
        }

        public bool PontoEmRetanguloSelecao(double x, double y)
        {
            bool inside = false;
            double x_intersec;

            Linha[] lins = new Linha[4];

            lins[0].pIni.x = RetanguloSelecao2[0].x;
            lins[0].pIni.y = RetanguloSelecao2[0].y;
            lins[0].pFin.x = RetanguloSelecao2[1].x;
            lins[0].pFin.y = RetanguloSelecao2[1].y;

            lins[1].pIni.x = RetanguloSelecao2[1].x;
            lins[1].pIni.y = RetanguloSelecao2[1].y;
            lins[1].pFin.x = RetanguloSelecao2[2].x;
            lins[1].pFin.y = RetanguloSelecao2[2].y;

            lins[2].pIni.x = RetanguloSelecao2[2].x;
            lins[2].pIni.y = RetanguloSelecao2[2].y;
            lins[2].pFin.x = RetanguloSelecao2[3].x;
            lins[2].pFin.y = RetanguloSelecao2[3].y;

            lins[3].pIni.x = RetanguloSelecao2[3].x;
            lins[3].pIni.y = RetanguloSelecao2[3].y;
            lins[3].pFin.x = RetanguloSelecao2[0].x;
            lins[3].pFin.y = RetanguloSelecao2[0].y;

            for (int g = 0; g < 4; g++)
            {
                if (Geom.Iguais(lins[g].pFin.y, y)) continue;
                if (Geom.Iguais(lins[g].pIni.y, y)) continue;

                if (((lins[g].pIni.y > y) && (lins[g].pFin.y < y)) ||
                    ((lins[g].pIni.y < y) && (lins[g].pFin.y > y)))
                {
                    x_intersec = lins[g].pIni.x + (y - lins[g].pIni.y) * (lins[g].pFin.x - lins[g].pIni.x) / (lins[g].pFin.y - lins[g].pIni.y);
               
                    if (!Geom.Iguais(x_intersec, x))
                        if (x_intersec > x)
                        inside = !inside;
                };
            };

            return inside;
        }
        private void Desenho_DoubleClick(object sender, EventArgs e)
        {
            //       SetaSelecionados(false, PavimentoAtual);
        }

        private void Desenho_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyData == Keys.Escape)
                CancelaInsercoes();


        }

        private void Desenho_KeyPress(object sender, System.Windows.Forms.KeyPressEventArgs e)
        {
            if (e.KeyChar == (char)27)
            {
                CancelaInsercoes();
            }
            else
            {
                //    if ((Object)NewObject != null)
                //       NewObject.Command(e.KeyChar.ToString());
            }
        }

        private void Desenho_Activated(object sender, EventArgs e)
        {
            if (gerenciador.FVisGrelha != null)
                gerenciador.VoltaParaPavimento();
        }

        public bool PontoEmPoligono(ref double x, ref double y)
        {
            bool inside = false;
            double x_intersec;

            for (int g = 0; g < Linhas.Count; g++)
            {
                if (!Linhas[g].LinhaEixoViga || Linhas[g].auxiliar) continue;

                if (((Linhas[g].pIni.y > y) && (Linhas[g].pFin.y < y)) ||
                    ((Linhas[g].pIni.y < y) && (Linhas[g].pFin.y > y)))
                {
                    //    x_intersec = U0.x + (P.y -U0.y) * (U1.x - U0.x) / (U1.y - U0.y);

                    //livro "geometric tools for computer graphics"  pg.700
                    x_intersec = Linhas[g].pIni.x + (y - Linhas[g].pIni.y) * (Linhas[g].pFin.x - Linhas[g].pIni.x) / (Linhas[g].pFin.y - Linhas[g].pIni.y);

                    if (x_intersec > x)
                        inside = !inside;
                };
            };

            return inside;
        }

        TNoGrelha[] no;

        int max_sNo = 0;

        int jk;

        int LocalizaNo(ref double x, ref double y)
        {
            for (jk = 0; jk < max_sNo; jk++)
                if (Geom.Iguais(x, no[jk].x) && Geom.Iguais(y, no[jk].y))
                    return jk;

            return -1;
        }

        int LocalizaPonto(double x, double y)
        {
            for (jk = 0; jk < Pontos.Count; jk++)
                if (Geom.Iguais(x, Pontos[jk].x) && Geom.Iguais(y, Pontos[jk].y))
                    return jk;

            return -1;
        }

        int LocalizaPonto(ref List<TPonto> lista, double x, double y, double z)
        {
       
            for (jk = 0; jk < lista.Count; jk++)
                if (Geom.Iguais(x, lista[jk].x) && Geom.Iguais(y, lista[jk].y) && Geom.Iguais(z, lista[jk].z))
                    return jk;
            
            return -1;
        }

        TPonto FindPt(double x, double y, double z)
        {
            for (jk = 0; jk < Pontos.Count; jk++)
                if (Geom.Iguais(x, Pontos[jk].x) && Geom.Iguais(y, Pontos[jk].y) && Geom.Iguais(z, Pontos[jk].z))
                    return Pontos[jk];

            return null;
        }

        public void ShowMatrizTela(bool global, int barra = -1, double esp = 120, bool grelha = true)
        {
            string tex;
            /*
                        for (int i = 1; i <= 12; i++)
                        {
                            for (int j = 1; j <= 12; j++)
                            {
                                    tex = gerenciador.PorticoEspacial.barras[1].MatrizGlobal[i, j].ToString("n4");

                                    //    if (!Geom.Iguais(pav.MatrizRigidez.Sff[i, j], 0, 0.000001))
                                    Textos.Add(new TTexto(tex, j * 9 + esp, (i * -9) + esp, 0.01, -0.01, 1, 1, 1, 0));
                                    //   else
                                    //     Textos.Add(new TTexto(tex, j * 9 + 800, i * 9 + esp, 0.01, -0.01, 1, 0, 0, 0));

                                    ActiveLayer.AddObject(Textos[Textos.Count - 1]);
                            }
                        }

                       return;*/
            if (!grelha)
            {
                TTexto t;
                double dd;
                int ii = 0;
                /*  for (int i = 0; i < gerenciador.PorticoEspacial.NLinhas; i++)
                  {
                      for (int j = 0; j < gerenciador.PorticoEspacial.LarguraBanda; j++)
                      {
                          //   if (!Geom.Iguais(pav.MatrizRigidez.Sff[i, j], 0, 0.000001))
                          {
                              dd = gerenciador.PorticoEspacial.MatrizRigidez.Sff_[i][j];
                              tex = dd.ToString("n5");

                              //if (dd != 0)
                              {
                                  t = new TTexto(tex, j * 8 + esp, (i * -2) + esp, 0.01, -0.01, 1, 1, 1, 0);

                                  Textos.Add(new TTexto(tex, j * 9 + 800, i * 9 + esp, 0.01, -0.01, 1, 0, 0, 0));
                                  t.layer = PavimentoAtual.LayersByIdPrincipal[Lay.Vigas];
                                  AddTexto(ref t);
                                  ii++;
                              }
                          }
                      }
                  }*/

                for (int i = 1; i <= 12; i++)
                {
                    for (int j = 1; j <= 12; j++)
                    {
                        //   if (!Geom.Iguais(pav.MatrizRigidez.Sff[i, j], 0, 0.000001))
                        {
                            dd = gerenciador.PorticoEspacial.barras[1].MatrizGlobal[i, j];
                            tex = dd.ToString("n5");

                            //if (dd != 0)
                            {
                                t = new TTexto(tex, j * 8 + esp, (i * -2) + esp, 0.01, -0.01, 1, 1, 1, 0);

                                Textos.Add(new TTexto(tex, j * 9 + 800, i * 9 + esp, 0.01, -0.01, 1, 0, 0, 0));
                                t.layer = Estrutura.LayersByIdPrincipal[Lay.Vigas];
                                AddTexto(ref t, PavimentoAtual);
                                ii++;
                            }
                        }
                    }
                }
                /*  for (int i = 1; i <= gerenciador.PorticoEspacial.NLinhas; i++)
                  {
                      for (int j = 1; j <= gerenciador.PorticoEspacial.LarguraBanda; j++)
                      {
                          //   if (!Geom.Iguais(pav.MatrizRigidez.Sff[i, j], 0, 0.000001))
                          {
                              dd = gerenciador.PorticoEspacial.MatrizRigidez.Sff[i * gerenciador.PorticoEspacial.LarguraBanda + j];

                              tex = dd.ToString("n5");

                              if (dd != 0)
                              {
                                  t = new TTexto(tex, j * 8 + esp, (i * -2) + esp, 0.01, -0.01, 1, 1, 1, 0);

                                  Textos.Add(new TTexto(tex, j * 9 + 800, i * 9 + esp, 0.01, -0.01, 1, 0, 0, 0));
                                  t.layer = PavimentoAtual.LayersByIdPrincipal[Lay.Vigas];
                                  AddTexto(ref t);
                                  ii++;
                              }
                          }
                      }
                  }*/
                MessageBox.Show(ii.ToString());
            }
            else
            {
                TTexto t;
                double dd;
                int ii = 0;

                if (global)
                {
                    for (int i = 0; i < gerenciador.Pavimentos[gerenciador.cbPiso.SelectedIndex].NLinhas; i++)
                    {
                        for (int j = 0; j < gerenciador.Pavimentos[gerenciador.cbPiso.SelectedIndex].NLinhas; j++)
                        {
                            //   if (!Geom.Iguais(pav.MatrizRigidez.Sff[i, j], 0, 0.000001))
                            {
                                dd = gerenciador.Pavimentos[gerenciador.cbPiso.SelectedIndex].MatrizRigidez.Sff_[i][j];
                                tex = dd.ToString("n5");

                                //  if (dd != 0)
                                {
                                    t = new TTexto(tex, j * 8 + esp, (i * -2) + esp, 0.01, -0.01, 1, 1, 1, 0);

                                    Textos.Add(new TTexto(tex, j * 9 + 800, i * 9 + esp, 0.01, -0.01, 1, 0, 0, 0));
                                    t.layer = Estrutura.LayersByIdPrincipal[Lay.Vigas];
                                    AddTexto(ref t, PavimentoAtual);
                                    t.Grips[0].layer = t.layer;
                                    t.Grips[1].layer = t.layer;
                                    t.Grips[2].layer = t.layer;

                                    AddGrip(t.Grips[0], PavimentoAtual);
                                    AddGrip(t.Grips[1], PavimentoAtual);
                                    AddGrip(t.Grips[2], PavimentoAtual);

                                    ii++;
                                }
                            }
                        }
                    }
                }
            }
            /*  for (int i = 1; i <= 6; i++)
              {
                  for (int j = 1; j <= 6; j++)
                  {
                      // if (!Geom.Iguais(pav.MatrizRigidez.Sff_SemBanda[i, j], 0, 0.000001))
                      {
                          tex = pav.barras[2].MatRotacaoTransposta[i, j].ToString("n3");

                          //    if (!Geom.Iguais(pav.MatrizRigidez.Sff[i, j], 0, 0.000001))
                          Textos.Add(new TTexto(tex, j * 9 + esp, (i * -9) + esp, 0.01, -0.01, 1, 1, 1, 0));
                          //   else
                          //     Textos.Add(new TTexto(tex, j * 9 + 800, i * 9 + esp, 0.01, -0.01, 1, 0, 0, 0));

                          ActiveLayer.AddObject(Textos[Textos.Count - 1]);
                      }
                  }
              }*/
        }



        public void NovosDadosLaje()
        {
            if (gerenciador.FDadosLaje != null)
            {
                gerenciador.FDadosLaje.NumLaje.Value += 1;
                gerenciador.NovosDadosDeLaje();
            }
        }

        public void NovosDadosViga(bool incrementa)
        {
            if (TrechosVigas.Count == 0)
                return;

            int tr = 0;
            int max = 0;
            for (int j = 0; j < TrechosVigas.Count; j++)
            {
                if (TrechosVigas[j].Dados.numero > max)
                {
                    tr = j;
                    max = TrechosVigas[j].Dados.numero;
                }
            }

            List<string> tipo = new List<string>();
            tipo.Add("Retangular");
            tipo.Add("T");
            tipo.Add("I");
            tipo.Add("L");

            TPoligono Poligono = null;

            if (TrechosVigas[tr].Dados.indiceTipo < tipo.Count - 1)
                Poligono = new TPoligono(tipo[TrechosVigas[tr].Dados.indiceTipo], TrechosVigas[tr].Dados.h1.ToString(), TrechosVigas[tr].Dados.b1.ToString(), TrechosVigas[tr].Dados.b2.ToString(), TrechosVigas[tr].Dados.h2.ToString(), "", "", false);

            DadosTrecho = (TDadosViga)TrechosVigas[tr].Dados.Clone();
            if (incrementa)
            {
                DadosTrecho.numero += 1;
                if (gerenciador.FDadosViga != null)
                    gerenciador.FDadosViga.NumViga.Value = DadosTrecho.numero;
            }
            /*new TDadosViga();
    /*    DadosTrecho.h1 = TrechosVigas[tr].Dados.h1;
        DadosTrecho.b1 = TrechosVigas[tr].Dados.b1;
        DadosTrecho.h2 = TrechosVigas[tr].Dados.h2;
        DadosTrecho.b2 = TrechosVigas[tr].Dados.b2;
        DadosTrecho.face_insercao = TrechosVigas[tr].Dados.face_insercao;
        DadosTrecho.Poligono = Poligono;
        DadosTrecho.indiceTipo = 0;

        DadosTrecho.numero  = TrechosVigas.Max(obj => obj.Dados.numero);
        DadosTrecho.numero += 1;

        DadosTrecho.nome = "V";*/

            Comando.Text = Const.CMD_TRECHOVIGA_1_P;
        }
        public void NovosDadosBarra(bool incrementa)
        {
            if (Barras.Count == 0)
                return;

            int tr = 0;
            int max = 0;
            for (int j = 0; j < Barras.Count; j++)
            {
                if (Barras[j].Dados.numero > max)
                {
                    tr = j;
                    max = Barras[j].Dados.numero;
                }
            }

            TPoligono Poligono = null;

            // if (Barras[tr].Dados.indiceTipo < tipo.Count - 1)
            //    Poligono = new TPoligono(tipo[Barras[tr].Dados.indiceTipo], Barras[tr].Dados.h1.ToString(), Barras[tr].Dados.b1.ToString(), Barras[tr].Dados.b2.ToString(), Barras[tr].Dados.h2.ToString(), "", "", false);

            DadosBarra = (TDadosBarra)Barras[tr].Dados.Clone();
            if (incrementa)
            {
                DadosBarra.numero += 1;
                if (gerenciador.DadosBarra != null)
                {
                    gerenciador.DadosBarra.numero.Text = DadosBarra.numero.ToString();

                }
            }

            /*new TDadosViga();
    /*    DadosTrecho.h1 = TrechosVigas[tr].Dados.h1;
        DadosTrecho.b1 = TrechosVigas[tr].Dados.b1;
        DadosTrecho.h2 = TrechosVigas[tr].Dados.h2;
        DadosTrecho.b2 = TrechosVigas[tr].Dados.b2;
        DadosTrecho.face_insercao = TrechosVigas[tr].Dados.face_insercao;
        DadosTrecho.Poligono = Poligono;
        DadosTrecho.indiceTipo = 0;

        DadosTrecho.numero  = TrechosVigas.Max(obj => obj.Dados.numero);
        DadosTrecho.numero += 1;
            
        DadosTrecho.nome = "V";*/

            Comando.Text = Const.CMD_BARRA_1_P;
        }
        public void NovosDadosPilar()
        {
            if (Pilares.Count == 0)
                return;

            int tr = 0;
            int max = 0;
            for (int j = 0; j < Pilares.Count; j++)
            {
                if (Pilares[j].Dados.numero > max)
                {
                    tr = j;
                    max = Pilares[j].Dados.numero;
                }
            }

            List<string> tipo = new List<string>();
            tipo.Add("Retangular");
            tipo.Add("T");
            tipo.Add("I");
            tipo.Add("L");
            tipo.Add("Circular");
            tipo.Add("Definir polígono...");

            TPoligono Poligono = null;

            if (Pilares[tr].Dados.indiceTipo < tipo.Count - 1)
                Poligono = new TPoligono(tipo[Pilares[tr].Dados.indiceTipo], Pilares[tr].Dados.h1.ToString(), Pilares[tr].Dados.b1.ToString(), Pilares[tr].Dados.b2.ToString(), Pilares[tr].Dados.h2.ToString(), "", "", Pilares[tr].Dados.vazada);

            DadosPilar = null;
            DadosPilar = new TDadosPilar();
            DadosPilar.h1 = Pilares[tr].Dados.h1;
            DadosPilar.b1 = Pilares[tr].Dados.b1;
            DadosPilar.h2 = Pilares[tr].Dados.h2;
            DadosPilar.b2 = Pilares[tr].Dados.b2;
            DadosPilar.altura = Pilares[tr].Dados.altura;
            DadosPilar.indiceTipo = Pilares[tr].Dados.indiceTipo;
            DadosPilar.excentricidade = Pilares[tr].Dados.excentricidade;
            DadosPilar.nome = "P";
            DadosPilar.numero = Pilares[tr].Dados.numero + 1;

            if (gerenciador.FDadosPilar != null)
                gerenciador.FDadosPilar.Num.Value = DadosPilar.numero;

            DadosPilar.Poligono = Poligono;
            DadosPilar.vazada = Pilares[tr].Dados.vazada;

            Comando.Text = Const.CMD_PILAR_1_P;
        }


        //  Pen mPen = new Pen(Color.White);
        // public Pen mPenCursor = new Pen(Color.White);
        vec3 Ponto1_Selecao = new vec3(0);

        float[] dashValues = { 3, 3, 5 };

        PontoD ptb, pta, ptc, ptd, intersec;
        bool selec = false;
        void PintaSelecao(bool SelecionandoCandidatos)
        {
            intersec.x = 0;
            intersec.y = 0;

            if (SelParcial)
            {
                for (int p = 0; p < Linhas.Count; p++)
                {
                    if (Linhas[p].auxiliar) continue;
                    if (Linhas[p].Selecionado) continue;

                    selec = false;

                    pta.x = Linhas[p].pIni.x;
                    pta.y = Linhas[p].pIni.y;
                    ptb.x = Linhas[p].pFin.x;
                    ptb.y = Linhas[p].pFin.y;

                    ptc.x = RetanguloSelecao[0].x;
                    ptc.y = RetanguloSelecao[0].y;
                    ptd.x = RetanguloSelecao[1].x;
                    ptd.y = RetanguloSelecao[1].y;

                    if (Geom.calcIntersecEQU_RETA(ref pta, ref ptb, ref ptc, ref ptd, ref intersec))
                        selec = true;

                    ptc.x = RetanguloSelecao[1].x;
                    ptc.y = RetanguloSelecao[1].y;
                    ptd.x = RetanguloSelecao[2].x;
                    ptd.y = RetanguloSelecao[2].y;

                    if (Geom.calcIntersecEQU_RETA(ref pta, ref ptb, ref ptc, ref ptd, ref intersec))
                        selec = true;

                    ptc.x = RetanguloSelecao[2].x;
                    ptc.y = RetanguloSelecao[2].y;
                    ptd.x = RetanguloSelecao[3].x;
                    ptd.y = RetanguloSelecao[3].y;

                    if (Geom.calcIntersecEQU_RETA(ref pta, ref ptb, ref ptc, ref ptd, ref intersec))
                        selec = true;

                    ptc.x = RetanguloSelecao[3].x;
                    ptc.y = RetanguloSelecao[3].y;
                    ptd.x = RetanguloSelecao[0].x;
                    ptd.y = RetanguloSelecao[0].y;

                    if (Geom.calcIntersecEQU_RETA(ref pta, ref ptb, ref ptc, ref ptd, ref intersec))
                        selec = true;

                    if (PontoEmRetanguloSelecao(Linhas[p].pIni.x, Linhas[p].pIni.y) && PontoEmRetanguloSelecao(Linhas[p].pFin.x, Linhas[p].pFin.y))
                        selec = true;

                    if (selec)
                    {
                        if (Linhas[p].LinhaContornoPilar)
                        {
                            foreach (TPilar pilar in Pilares)
                            {
                                foreach (TLinha lin in pilar.Dados.Poligono.linhas_poligonal)
                                    if ((object)lin == (object)Linhas[p])
                                        pilar.SetaSelecao(true, true);
                            }
                        }
                        else
                        {
                            //    this.Text = p.ToString();;
                            Linhas[p].SetaSelecao(true, true, SelecionandoCandidatos);
                        }
                    }
                    else
                    {
                        //Linhas[p].SetaSelecao(false, false);
                        if (Linhas[p].LinhaContornoPilar)
                        {
                            foreach (TPilar pilar in Pilares)
                            {
                                foreach (TLinha lin in pilar.Dados.Poligono.linhas_poligonal)
                                    if ((object)lin == (object)Linhas[p])
                                        pilar.SetaSelecao(false, false, SelecionandoCandidatos);
                            }
                        }
                        else
                        {
                            //   this.Text = "des - " + p.ToString(); 
                            Linhas[p].SetaSelecao(false, false, SelecionandoCandidatos);
                        }
                    }
                }

                foreach (TTexto t in Textos)
                    if (PontoEmRetanguloSelecao(t.x, t.y))
                        t.SetaSelecao(true, true, SelecionandoCandidatos);
                // else
                //   t.SetaSelecao(false, false);

                foreach (TGrip gr in Grips)
                    if (PontoEmRetanguloSelecao(gr.x, gr.y))
                        if (gr.ObjetoDesenho.Tipo == Const.ID_LAJE)
                            gr.SetaSelecao(true, true, SelecionandoCandidatos);
                // else
                // gr.SetaSelecao(false, false);
            }
            else
            if (SelIntegral)
            {
                for (int p = 0; p < Linhas.Count; p++)
                {
                    //  if (Linhas[p].Selecionado) 
                    //     continue;

                    selec = false;

                    if (PontoEmRetanguloSelecao(Linhas[p].pIni.x, Linhas[p].pIni.y) && PontoEmRetanguloSelecao(Linhas[p].pFin.x, Linhas[p].pFin.y))
                        selec = true;

                    if (selec)
                    {
                        if (Linhas[p].LinhaContornoPilar)
                        {
                            foreach (TPilar pilar in Pilares)
                            {
                                foreach (TLinha lin in pilar.Dados.Poligono.linhas_poligonal)
                                    if ((object)lin == (object)Linhas[p])
                                        pilar.SetaSelecao(true, true, SelecionandoCandidatos);
                            }
                        }
                        else
                            Linhas[p].SetaSelecao(true, true, SelecionandoCandidatos);
                    }
                    else
                    {
                        if (Linhas[p].LinhaContornoPilar)
                        {
                            foreach (TPilar pilar in Pilares)
                            {
                                foreach (TLinha lin in pilar.Dados.Poligono.linhas_poligonal)
                                    if ((object)lin == (object)Linhas[p])
                                        pilar.SetaSelecao(false, false, SelecionandoCandidatos);
                            }
                        }
                        else
                            Linhas[p].SetaSelecao(false, false, SelecionandoCandidatos);
                    }
                };

                foreach (TGrip gr in Grips)
                    if (PontoEmRetanguloSelecao(gr.x, gr.y))
                        if (gr.ObjetoDesenho.Tipo == Const.ID_LAJE)
                        {
                            gr.SetaSelecao(true, true, SelecionandoCandidatos);
                        }
                        else
                        {
                            //       gr.SetaSelecao(false, false);
                        }


                foreach (TTexto t in Textos)
                    if (PontoEmRetanguloSelecao(t.x, t.y))
                        t.SetaSelecao(true, true, SelecionandoCandidatos);
                //else
                ///   t.SetaSelecao(false, false);
            }

            RetanguloSelecao[3].x = 0;
            RetanguloSelecao[3].y = 0;
            RetanguloSelecao[0].x = 0;
            RetanguloSelecao[0].y = 0;
            RetanguloSelecao[1].x = 0;
            RetanguloSelecao[1].y = 0;
            RetanguloSelecao[2].x = 0;
            RetanguloSelecao[2].y = 0;
        }

        System.Drawing.Color c1 = System.Drawing.Color.FromArgb(50, System.Drawing.Color.Blue);
        System.Drawing.Color c2 = System.Drawing.Color.FromArgb(50, System.Drawing.Color.Green);
        SolidBrush sb;
        SolidBrush sb2;
        public void glControl1_MouseWheel(object sender, System.Windows.Forms.MouseEventArgs e)
        {
            //  this.Refresh();
        }
        Raio raioZoom = new Raio(0, 0);
        Raio raioSelecao = new Raio(0, 0);
        Raio raioPlanoTrabalho = new Raio(0, 0);

        vec3 Coord_TrianguloSelecao = new vec3(0, 0, 0);
        vec3 Coord_PlanoZoom = new vec3(0, 0, 0);
        vec3 Coord_PlanoSelecao = new vec3(0, 0, 0);
        vec3 Coord_PlanoSnap = new vec3(0, 0, 0);
        vec3 Coord_PlanoTrabalho = new vec3(0, 0, 0);
        vec3 Coord_PlanoRotacao = new vec3(0, 0, 0);
        vec3 Coord_PlanoPanning = new vec3(0, 0, 0);
        vec3 Coord2_PlanoPanning = new vec3(0, 0, 0);


        Plano planoZoom, PlanoFrente, planoSelecao, planoRotacao, planoSnap;
        double xant, yant = 0, zant = 0;
        vec3 normXY;
        double z1_selecao;
        void CriaPlanos()
        {
            normXY = new vec3(0.0f, 0.0f, 1.0f);
            vec3 pt = new vec3(0.0f, 0.0f, 200);
            planoZoom = new Plano(normXY, pt, "xy");
            PlanoFrente = new Plano(normXY, new vec3(0, 0, 300), "xy");

            z1_selecao = -0.069;
            vec3 ptSelecao = new vec3(0.0f, 0.0f, z1_selecao);
            planoSelecao = new Plano(normXY, ptSelecao, "xy");

            planoSnap = new Plano(new vec3(0, 0, 1), ptSelecao, "xy");

            //  vec3 ptPlanoRotacao = new vec3(0.0f, 0.0f, Pavimentos[0].PeDireito);
            // planoRotacao = new Plano(normPlanoRotacao, ptPlanoRotacao, 0);
            //  CriaPlanoTrabalho("xy",0);
        }

        // Cursor Cur_rotacao = new Cursor(PGi.Properties.Resources.conf_cota1.);

        public Plano PlanoTrabalho, PlanoPanning;
        public void CriaPlanoTrabalho(string plano, double coordPlano)
        {
            if (Estrutura != null)
                Estrutura.CalculaMinMaxCoordenadas(false);

            vec3 pt = new vec3(0);
            vec3 normal = new vec3(0);
            if (plano == "xy")
            {
                normal = new vec3(0.0f, 0.0f, 1.0f);
                pt = new vec3(0.0f, 0.0f, -coordPlano);
            }
            if (plano == "yz")
            {
                normal = new vec3(1, 0.0f, 0);
                pt = new vec3(coordPlano, 0.0f, 0);
            }

            if (plano == "xz")
            {
                normal = new vec3(0.0f, 1, 0);
                pt = new vec3(0.0f, coordPlano, 0);
            }

            PlanoTrabalho = new Plano(normal, pt, plano, PlanoCorte);

            if (Estrutura != null)
            {
                if (Estrutura.xMin != 99999999999)
                {
                    if (plano == "xy")
                    {
                        PlanoTrabalho.xmin = Estrutura.xMin;
                        PlanoTrabalho.ymin = Estrutura.yMin;
                        PlanoTrabalho.xmax = Estrutura.xMax;
                        PlanoTrabalho.ymax = Estrutura.yMax;
                    }
                    if (plano == "xz")
                    {
                        PlanoTrabalho.xmin = Estrutura.xMin;
                        PlanoTrabalho.zmin = Estrutura.zMin;
                        PlanoTrabalho.xmax = Estrutura.xMax;
                        PlanoTrabalho.zmax = Estrutura.zMax;
                    }
                    if (plano == "yz")
                    {
                        PlanoTrabalho.ymin = Estrutura.yMin;
                        PlanoTrabalho.zmin = Estrutura.zMin;
                        PlanoTrabalho.ymax = Estrutura.yMax;
                        PlanoTrabalho.zmax = Estrutura.zMax;
                    }
                }
                else
                {
                    if (plano == "xy")
                    {
                        PlanoTrabalho.xmin = -10;
                        PlanoTrabalho.ymin = -10;
                        PlanoTrabalho.xmax = 10;
                        PlanoTrabalho.ymax = 10;
                    }
                    if (plano == "xz")
                    {
                        PlanoTrabalho.xmin = -10;
                        PlanoTrabalho.zmin = -10;
                        PlanoTrabalho.xmax = 10;
                        PlanoTrabalho.zmax = 10;
                    }
                    if (plano == "yz")
                    {
                        PlanoTrabalho.ymin = -10;
                        PlanoTrabalho.zmin = -10;
                        PlanoTrabalho.ymax = 10;
                        PlanoTrabalho.zmax = 10;
                    }
                }
            }
            else
            {  // modo preguiça ativado
                if (plano == "xy")
                {
                    PlanoTrabalho.xmin = -10;
                    PlanoTrabalho.ymin = -10;
                    PlanoTrabalho.xmax = 10;
                    PlanoTrabalho.ymax = 10;
                }
                if (plano == "xz")
                {
                    PlanoTrabalho.xmin = -10;
                    PlanoTrabalho.zmin = -10;
                    PlanoTrabalho.xmax = 10;
                    PlanoTrabalho.zmax = 10;
                }
                if (plano == "yz")
                {
                    PlanoTrabalho.ymin = -10;
                    PlanoTrabalho.zmin = -10;
                    PlanoTrabalho.ymax = 10;
                    PlanoTrabalho.zmax = 10;
                }
            }

            PlanoTrabalho.xmin -= 1;
            PlanoTrabalho.ymin -= 1;
            PlanoTrabalho.zmin += 1;
            PlanoTrabalho.xmax += 1;
            PlanoTrabalho.ymax += 1;
            PlanoTrabalho.zmax -= 1;

            PlanoTrabalho.zmax *= -1;
            PlanoTrabalho.zmin *= -1;


            eqn1 = new double[4];
            eqn1[0] = PlanoTrabalho.a;
            eqn1[1] = PlanoTrabalho.b;
            eqn1[2] = PlanoTrabalho.c;
            eqn1[3] = PlanoTrabalho.d;
        }

        public void AtualizaPlanoTrabalho(string plano, double coord)
        {
            double POS = 0;
            if (plano == "xy")
            {
                PlanoTrabalho.Normal.x = 0;
                PlanoTrabalho.Normal.y = 0;
                PlanoTrabalho.Normal.z = 1;

                PlanoTrabalho.Posicao.z = -coord;
                PlanoTrabalho.Posicao.x = 0;
                PlanoTrabalho.Posicao.y = 0;
                POS = PlanoTrabalho.Posicao.z;
            }
            if (plano == "yz")
            {
                PlanoTrabalho.Normal.x = 1;
                PlanoTrabalho.Normal.y = 0;
                PlanoTrabalho.Normal.z = 0;

                PlanoTrabalho.Posicao.z = 0;
                PlanoTrabalho.Posicao.x = coord;
                PlanoTrabalho.Posicao.y = 0;
                POS = PlanoTrabalho.Posicao.x;
            }
            if (plano == "xz")
            {
                PlanoTrabalho.Normal.x = 0;
                PlanoTrabalho.Normal.y = 1;
                PlanoTrabalho.Normal.z = 0;

                PlanoTrabalho.Posicao.z = 0;
                PlanoTrabalho.Posicao.x = 0;
                PlanoTrabalho.Posicao.y = -coord;
                POS = PlanoTrabalho.Posicao.y;
            }

            PlanoTrabalho.a = PlanoTrabalho.Normal.x;
            PlanoTrabalho.b = PlanoTrabalho.Normal.y;
            PlanoTrabalho.c = PlanoTrabalho.Normal.z;
            PlanoTrabalho.d = (-PlanoTrabalho.Normal.DotProduct(PlanoTrabalho.Posicao));

            eqn1[3] = POS * -1;
        }

        double leftOrtoZoom, rightOrtoZoom, topOrtoZoom, bottonOrtoZoom, distancia_raio_triangulo, dist_zNear_objeto;
        bool FazendoZoom;
        Triangulo trianguloRef, triangulo_wheel;
        Vector4d pNDC;
        Vector4d pNDC_d;

        float z_trans_orto;
        void Wheel(int Delta)
        {
            try
            {
                FazendoZoom = true;

                /*if (Delta > 0)
                {
                    if (fatorzoom_orto + 0.6f < -0.6)
                        fatorzoom_orto += 0.6f;
                    else
                     if (fatorzoom_orto < -0.03f)
                        fatorzoom_orto += 0.03f;
                }
                else
                {
                    if (fatorzoom_orto < -0.6)
                        fatorzoom_orto -= 0.6f;
                    else
                    if (fatorzoom_orto < -0.01 || Geom.Iguais(fatorzoom_orto, 0))
                        fatorzoom_orto -= 0.05f;
                }*/
                z_trans_orto -= (Delta * 0.00055f);

                if (CameraOrto)
                {
                    if (Delta > 0)
                    {
                        /*if (fatorzoom_orto + 0.6f < -0.6)
                        {
                           /* if (clickShift && IdFerramentaEdicao != Const.ID_MOVER_EXTREMO_ELEMENTOS)
                                fatorzoom_orto += 2f;
                            else
                            if (clickCtrl)
                                fatorzoom_orto += 0.1f;
                            else   */
                        /*       fatorzoom_orto += 0.6f;
                       }
                       else
                        if (fatorzoom_orto < -0.03f)
                           fatorzoom_orto += 0.03f;*/

                        fatorzoom_orto -= (fatorzoom_orto * 0.21f);
                    }
                    else
                    {
                        fatorzoom_orto += (fatorzoom_orto * 0.21f);
                        /*   if (fatorzoom_orto < -0.6)
                           {
                               fatorzoom_orto -= 0.6f;
                           }
                           else
                           if (fatorzoom_orto < -0.01 || Geom.Iguais(fatorzoom_orto, 0, 0.01))
                               fatorzoom_orto -= 0.05f;*/
                    }

                    modelViewMatrix_Zoom = Matrix4d.CreateTranslation(-camera.x_trans, -camera.y_trans, -camera.z_trans);
                    raioZoom.GerarRaio3D(ref mouseX, ref mouseY, ref ViewPortPrincipal, ref modelViewMatrix_Zoom, ref camera.Projecao);
                    raioZoom.CalculaIntersecao_Raio_x_Plano(ref planoZoom, ref Coord_PlanoZoom);
                    xant = Coord_PlanoZoom.x;
                    yant = Coord_PlanoZoom.y;
                    camera.z_trans -= (Delta * 0.00055f);
                    leftOrtoZoom = GLleft * fatorzoom_orto;
                    rightOrtoZoom = GLright * fatorzoom_orto;
                    topOrtoZoom = GLtop * fatorzoom_orto;
                    bottonOrtoZoom = GLbotton * fatorzoom_orto;

                   // Projecao = Matrix4d.CreateOrthographicOffCenter((float)leftOrtoZoom, (float)rightOrtoZoom, (float)bottonOrtoZoom, (float)topOrtoZoom, (float)zNear, (float)zFar);
                    camera.Projecao_Ortografica( leftOrtoZoom, rightOrtoZoom, bottonOrtoZoom, topOrtoZoom, zNear, zFar);

                    modelViewMatrix_Zoom = Matrix4d.CreateTranslation(-camera.x_trans, -camera.y_trans, -camera.z_trans);
                    raioZoom.GerarRaio3D(ref mouseX, ref mouseY, ref ViewPortPrincipal, ref modelViewMatrix_Zoom, ref camera.Projecao);
                    raioZoom.CalculaIntersecao_Raio_x_Plano(ref planoZoom, ref Coord_PlanoZoom);

                    offset_x = (Coord_PlanoZoom.x - xant);
                    offset_y = (Coord_PlanoZoom.y - yant);

                    camera.x_trans += offset_x * -1;
                    camera.y_trans += offset_y * -1;
                }
                else
                {
                    dist_zNear_objeto = 999999;
                    if (!DeformacaoSolida && !enquadrando && Delta > 0 && TriangulosSelecao.Count() < 40000)
                    {
                        IntersecaoAABB();

                        VP = RMath.Multiply(camera.viewMatrix, camera.Projecao /*Projecao_Panning*/);

                        raioZoom.GerarRaio3D(ref mouseX, ref mouseY, ref ViewPortPrincipal, ref camera.viewMatrix, ref camera.Projecao /*Projecao_Panning*/);

                        for (i = 0; i < triangulos_temp.Count(); i++)
                        {
                            trianguloRef = triangulos_temp[i];

                            if (raioZoom.CalculaIntersecao_Raio_x_Triangulo(ref trianguloRef, ref Coord_TrianguloSelecao))
                            {
                                pNDC = new Vector4d(Coord_TrianguloSelecao.x, Coord_TrianguloSelecao.y, Coord_TrianguloSelecao.z, 1);

                                pNDC = RMath.Multiply(pNDC, VP);
                               // pNDC = pNDC * VP;// viewMatrix * Projecao_Panning;
                                pNDC /= pNDC.W;

                                if (pNDC.Z > 0 && pNDC.Z < 1)
                                {
                                    distancia_raio_triangulo = Math.Sqrt((raioZoom.p0.x - Coord_TrianguloSelecao.x) * (raioZoom.p0.x - Coord_TrianguloSelecao.x) + (raioZoom.p0.y - Coord_TrianguloSelecao.y) * (raioZoom.p0.y - Coord_TrianguloSelecao.y) + (raioZoom.p0.z - Coord_TrianguloSelecao.z) * (raioZoom.p0.z - Coord_TrianguloSelecao.z));//(raioZoom.p0 - Coord_TrianguloSelecao).Magnitude();
                                    if (distancia_raio_triangulo < dist_zNear_objeto)
                                        dist_zNear_objeto = distancia_raio_triangulo;
                                }
                            }
                        }
                    }

                    // if (triangulo_wheel != null)
                    //   triangulo_wheel.id = 55;
                //    mouseX = 200;
                //    mouseY = 200;

                    modelViewMatrix_Zoom = Matrix4d.CreateTranslation(0,0, -camera.z_trans);
                    raioZoom.GerarRaio3D(ref mouseX, ref mouseY, ref ViewPortPrincipal, ref modelViewMatrix_Zoom, ref camera.Projecao);
                    raioZoom.CalculaIntersecao_Raio_x_Plano(ref planoZoom, ref Coord_PlanoZoom);
                    xant = Coord_PlanoZoom.x;
                    yant = Coord_PlanoZoom.y;

                    if (/*!DeformacaoSolida && */!enquadrando)
                    {
                        if (distancia_raio_triangulo < 2 && distancia_raio_triangulo > 0)
                            camera.z_trans -= (Delta * 0.00055f);
                        else
                        if (distancia_raio_triangulo < 4 && distancia_raio_triangulo > 0)
                            camera.z_trans -= (Delta * 0.005f);
                        else
                        {
                            if (clickShift && IdFerramentaEdicao != Const.ID_MOVER_EXTREMO_ELEMENTOS)
                                camera.z_trans -= (Delta * 0.055f);
                            else
                            if (clickCtrl)
                                camera.z_trans -= (Delta * 0.00055f);
                            else
                                camera.z_trans -= (Delta * 0.015f);
                        }
                    }
                    else
                        camera.z_trans -= (Delta * 0.007f);

                    distancia_raio_triangulo = 0;
                    dist_zNear_objeto = 99999;

                    modelViewMatrix_Zoom = Matrix4d.CreateTranslation(0,0, -camera.z_trans);
                    raioZoom.GerarRaio3D(ref mouseX, ref mouseY, ref ViewPortPrincipal, ref modelViewMatrix_Zoom, ref camera.Projecao);
                    raioZoom.CalculaIntersecao_Raio_x_Plano(ref planoZoom, ref Coord_PlanoZoom);

                    offset_x = (Coord_PlanoZoom.x - xant);
                    offset_y = (Coord_PlanoZoom.y - yant);

                    camera.x_trans += offset_x;
                    camera.y_trans += offset_y;
                }

              //  camera.x_trans = x_trans;
             //   camera.y_trans = y_trans;
                camera.ViewDirty = true;

                DesenhaObjetos();
            }

            catch (Exception ex)
            {
                MessageBox.Show("erro mousewheel: " + ex.Message);
            }
            FazendoZoom = false;

           // glControl.SwapBuffers();
        }
        public void Desenho_MouseWheel(object sender, System.Windows.Forms.MouseEventArgs e)
        {
            Wheel(e.Delta);

          //  AtualizaDisplayList_Nos();
            DesenhaObjetos();
            glControl.SwapBuffers();

        }
        double iEscalaAnimacao = 0, incrementoAnimacao = 0;
        bool decrescer = false;

        float[][] BatchArestas_aux;
        float[][] BatchTriangulos_Deformacao_aux;
        int incrementosAnimacao = 25;
        public int tipoCargaResultado, id_combinacao, id_caso;
        public void AnimarModo(bool animar, bool solido, double fator)
        {
            try
            {
                AnimarDeformacao = animar;
                decrescer = false;
                iEscalaAnimacao = 0;
                incrementoAnimacao = 0;

                if (animar)
                {
                    if (solido)
                    {
                        incrementosAnimacao = 10;
                        this.timer1.Interval = 50;
                    }
                    else
                    {
                        incrementosAnimacao = 35;
                        this.timer1.Interval = 20;
                    }

                    gerenciador.ChamaAguardar(this, "Criando animação. Aguarde...");
                    incAnimar = incrementosAnimacao;

                    if (solido)
                        incrementoAnimacao = fator / incrementosAnimacao;
                    else
                        incrementoAnimacao = fator / incrementosAnimacao;

                    if (solido)
                    {
                        PreencheBatchTriangulos();
                        BatchTriangulos_Deformacao_aux = new float[incrementosAnimacao][];
                        for (int j = 0; j < incrementosAnimacao; j++)
                        {
                            Application.DoEvents();
                            BatchTriangulos_Deformacao_aux[j] = new float[BatchTriangulos_Deformacao.Count()];
                        }
                    }

                    PreencheBatchArestas();
                    BatchArestas_aux = new float[incrementosAnimacao][];
                    for (int j = 0; j < incrementosAnimacao; j++)
                    {
                        Application.DoEvents();
                        BatchArestas_aux[j] = new float[BatchArestas.Count()];
                    }

                    iEscalaAnimacao -= incrementoAnimacao;
                    for (int i = 0; i < incrementosAnimacao; i++)
                    {
                        Application.DoEvents();
                        iEscalaAnimacao += incrementoAnimacao;
                        if (solido)
                        {
                            PreencheBatchTriangulos();
                            BatchTriangulos_Deformacao_aux[i] = BatchTriangulos_Deformacao;
                        }

                        PreencheBatchArestas();
                        BatchArestas_aux[i] = BatchArestas;
                    }

                    iEscalaAnimacao += incrementoAnimacao;
                    timer1.Enabled = true;

                    gerenciador.FechaAguardar();
                }
                else
                {
                    DirtyPortico();
                    decrescer = false;
                    iEscalaAnimacao = 0;
                    incrementoAnimacao = 0;
                    timer1.Enabled = false;
                    PreencheBatchTriangulos();
                    PreencheBatchArestas();
                    AtualizaVBO();
                    DesenhaObjetos();
                    glControl.SwapBuffers();
                }
            }
            catch (Exception ee)
            {
                gerenciador.FechaAguardar();
            }
        }
        public void AnimarDeformacoes(bool animar)
        {
            try
            {
                AnimarDeformacao = animar;
                decrescer = false;
                iEscalaAnimacao = 0;
                incrementoAnimacao = 0;

                if (animar)
                {

                    if (DeformacaoSolida)
                    {
                        incrementosAnimacao = 10;
                        this.timer1.Interval = 100;
                    }
                    else
                    {
                        incrementosAnimacao = 25;
                        this.timer1.Interval = 50;
                    }

                    gerenciador.ChamaAguardar(this, "Criando animação. Aguarde...");
                    incAnimar = incrementosAnimacao;

                    if (DeformacaoSolida || MostraTensoesNormaisGradiente   )
                        incrementoAnimacao = fatorDeformacao / incrementosAnimacao;
                    else
                        incrementoAnimacao = fatorDeformacao / incrementosAnimacao;
                    
                    if (DeformacaoSolida || MostraTensoesNormaisGradiente)
                    {
                        PreencheBatchTriangulos();
                        BatchTriangulos_Deformacao_aux = new float[incrementosAnimacao][];
                        for (int j = 0; j < incrementosAnimacao; j++)
                        {
                            Application.DoEvents();
                            BatchTriangulos_Deformacao_aux[j] = new float[BatchTriangulos_Deformacao.Count()];
                        }
                    }

                    PreencheBatchArestas();
                    BatchArestas_aux = new float[incrementosAnimacao][];
                    for (int j = 0; j < incrementosAnimacao; j++)
                    {
                        Application.DoEvents();
                        BatchArestas_aux[j] = new float[BatchArestas.Count()];
                    }

                    iEscalaAnimacao -= incrementoAnimacao;
                    for (int i = 0; i < incrementosAnimacao; i++)
                    {
                        Application.DoEvents();
                        iEscalaAnimacao += incrementoAnimacao;
                        if (DeformacaoSolida || MostraTensoesNormaisGradiente)
                        {
                            PreencheBatchTriangulos();
                            BatchTriangulos_Deformacao_aux[i] = BatchTriangulos_Deformacao;
                        }

                        PreencheBatchArestas();
                        BatchArestas_aux[i] = BatchArestas;
                    }

                    iEscalaAnimacao += incrementoAnimacao;
                    timer1.Enabled = true;

                    gerenciador.FechaAguardar();
                }
                else
                {
                    DirtyPortico();
                    decrescer = false;
                    iEscalaAnimacao = 0;
                    incrementoAnimacao = 0;
                    timer1.Enabled = false;
                    PreencheBatchTriangulos();
                    PreencheBatchArestas();
                    AtualizaVBO();
                    DesenhaObjetos();
                    glControl.SwapBuffers();
                }
            }
            catch(Exception ee)
            {
                gerenciador.FechaAguardar();
            }
        }
        int incAnimar = 12;
        private void timer1_Tick(object sender, EventArgs e)
        {
            if (AnimarDeformacao && MostraDeformacoes)
            {
                if (Geom.Iguais(iEscalaAnimacao, fatorDeformacao) && decrescer == false)
                {
                    incAnimar = incrementosAnimacao;
                    decrescer = true;
                }
            }
            else
            if (AnimarDeformacao && MostraModosVibracao)
            {
                if (Geom.Iguais(iEscalaAnimacao, fatorModoVibracao) && decrescer == false)
                {
                    incAnimar = incrementosAnimacao;
                    decrescer = true;
                }
            }
            else
            if (AnimarDeformacao && MostraModosFlambagem)
            {
                if (Geom.Iguais(iEscalaAnimacao, fatorFlambagem) && decrescer == false)
                {
                    incAnimar = incrementosAnimacao;
                    decrescer = true;
                }
            }


            if (decrescer)
            {
                incAnimar--;
                iEscalaAnimacao -= incrementoAnimacao;
            }
            else
            {
                incAnimar++;
                iEscalaAnimacao += incrementoAnimacao;
            }

            if (DeformacaoSolida || MostraTensoesNormaisGradiente ||
                (MostraModosVibracao && ModoVibracaoSolido) ||
                (MostraModosFlambagem && FlambagemSolido))
                BatchTriangulos_Deformacao = BatchTriangulos_Deformacao_aux[incAnimar];
            
            BatchArestas = BatchArestas_aux[incAnimar];
            
            if (Geom.Iguais(iEscalaAnimacao, 0) && decrescer)
            {
                incAnimar = -1;

                decrescer = false;
            }
            
           // PreencheBatchTriangulos();
           // PreencheBatchArestas();
            AtualizaVBO();
            DesenhaObjetos();
            glControl.SwapBuffers();
        }
        double offset_x, offset_y;
        double fatorzoom_orto = -5;
        public static OpenTK.Matrix4d /*Projecao,*/ proj_Cubo, ViewMatrix;
        double zNear, zFar;
        double GLtop, GLbotton, GLleft, GLright;
        CuboRotacao cubo;
        public static bool cameraPerspectiva;
        public void SetupCamera(bool mudancaManual = false, double zfar = 300)
        {
            try
            {
                cubo = new CuboRotacao(this.Width, this.Height);
             
                h = this.Height;
                w = this.Width;

                GL.Viewport(0, 0, this.Width, this.Height);

                fatorzoom_orto = -5;
                
              //  camera = new Camera3D();

                GL.MatrixMode(MatrixMode.Projection);

                cameraPerspectiva = !CameraOrto;
                if (CameraOrto)
                {
                    if (mudancaManual)
                    {
                        Wheel(-120);
                        Wheel(120);
                    }

                    cubo.CameraOrto = true;
                    cubo.CarregaTextura();

                    GLtop = 1;
                    GLbotton = -1;

                    if (this.Height == 0)
                    {
                        GLleft = 0;
                        GLright = 0;
                    }
                    else
                    {
                        GLleft = -(double)(this.Width) / (double)this.Height;
                        GLright = -GLleft;
                    }

                    zNear = -100;
                    zFar = 300;

                    if (mudancaManual)
                    {
                        camera.y_rot_angle -= 180;
                        camera.z_ang_rad = (camera.y_rot_angle * Math.PI) / 180;
                    }
                    leftOrtoZoom = GLleft * fatorzoom_orto;
                    rightOrtoZoom = GLright * fatorzoom_orto;
                    topOrtoZoom = GLtop * fatorzoom_orto;
                    bottonOrtoZoom = GLbotton * fatorzoom_orto;

                    //Projecao = Matrix4d.CreateOrthographicOffCenter(leftOrtoZoom, rightOrtoZoom, bottonOrtoZoom, topOrtoZoom, zNear, zFar);
                   
                    camera.Projecao_Ortografica(leftOrtoZoom, rightOrtoZoom, bottonOrtoZoom, topOrtoZoom, zNear, zFar);

                    GL.LoadMatrix(ref camera.Projecao);
                    double near = 0.00001;
                    double far = 300;                  
                }
                else
                { //Perspectiva

                    cubo.CameraOrto = false;
                    cubo.CarregaTextura();
                    zNear = 0.1;// 0.05;
                    zFar = zfar;

                    if (mudancaManual)
                    {
                        camera.y_rot_angle += 180;
                        camera.z_ang_rad = (camera.y_rot_angle * Math.PI) / 180;
                    }
                    
                //    Projecao = Matrix4d.CreatePerspectiveFieldOfView((45 * Math.PI/180), this.Width / this.Height, zNear, zFar);
                    camera.Projecao_Perspectiva(45, zNear, zFar);

                    GL.LoadMatrix(ref camera.Projecao);

                    double near = 0.00001;
                    double far = 300;
              //      Projecao_Panning = Matrix4d.CreatePerspectiveFieldOfView((45 * Math.PI / 180), this.Width / this.Height, near, far);
                }

                CriaPlanos();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }
        public bool Unifilar,FundoGradiente, Arestas, ArestasConfObjeto, AnimarDeformacao, ArestasResultado, MostrarNos, MostrarDescricaoElementos, MostrarNumeroElementos;
        public int Transparencia;
        double /*pivoX, pivoY, pivoZ,*/ centroX,centroY,centroZ;
        void Pivo_no_centro()
        {
            Estrutura.CalculaMinMaxCoordenadas(MostraDeformacoes);
            camera.pivoX = (Estrutura.xMin + ((Estrutura.xMax - Estrutura.xMin) / 2));
            camera.pivoY = (Estrutura.yMin + ((Estrutura.yMax - Estrutura.yMin) / 2));
            camera.pivoZ = (Estrutura.zMin + ((Estrutura.zMax - Estrutura.zMin) / 2));

            //camera.pivoX = pivoX;
            //camera.pivoY = pivoY;
            //camera.pivoZ = pivoZ;
            camera.ViewDirty = true;
        }
        void CalculaCentroEstrutura()
        {
            Estrutura.CalculaMinMaxCoordenadas(MostraDeformacoes);
            centroX = (Estrutura.xMin + ((Estrutura.xMax - Estrutura.xMin) / 2));
            centroY = (Estrutura.yMin + ((Estrutura.yMax - Estrutura.yMin) / 2));
            centroZ = (Estrutura.zMin + ((Estrutura.zMax - Estrutura.zMin) / 2));
        }

        public bool MostraPlano;

        int[] lists = new int[0];
        int[] lists_nos = new int[0];

        int num_lists, num_nos;

        public void AtualizaCargas(bool atuShaders = true)
        {
            TBarraGenerica bb;
            TCasosCarga cc;

            foreach (TCargaLinear c in CargasLineares)
            {
                cc = CasosCarga.Find(o => o.ID == c.Dados.idCaso);
                c.Dados.cor = cc.Cor;

                bb = Estrutura.barras.Find(o => o.IDBarra == c.idBarra);
                if (bb != null)
                {

                    if (cc.ID == 1)
                        c.Dados.valor = bb.PesoProprio.Dados.valor;

                    c.anguloRotacao = bb.Dados.anguloRotacao;

                    c.CriaSetas(true, 0, 0, fatorCarga);
                }
            }

            foreach (TCargaPontual c in CargasPontuais)
            {
                cc = CasosCarga.Find(o => o.ID == c.Dados.idCaso);
                c.Dados.cor = cc.Cor;

                //                bb = Estrutura..Find(o => o.IDBarra == c.idPonto);
                //              if (bb != null)
                {

                    //if (cc.Id == -1)
                    // c.Dados.valor = bb.PesoProprio.Dados.valor;

                    //   c.anguloRotacao = bb.Dados.anguloRotacao;

                    c.CriaSeta(true, fatorCarga);
                }
            }

            if (atuShaders)
              AtualizaShaders(false);
        }
        double TamanhoNo = 0.2;
        int tamNo;
        public void AtualizaDisplayList_Nos()
        {
        //    if (num_nos > 0)
        //        GL.DeleteLists(lists_nos[0], num_nos);

            tamNo = (int)gerenciador.ConfiguracoesPGi.F3DOpcoesVisualizacao.tamanhoNo;
            GL.PointSize(tamNo);
            
         /*   num_nos = Estrutura.nos.FindAll(o=>o.habilitado).Count();
            lists_nos = new int[num_nos];

            int first_list = GL.GenLists(num_nos);
            int cc = -1;

            bool visivel = false;
            foreach (TPonto o in Estrutura.nos)
            {
                o.Visivel = MostrarNos;
                
                if (!o.habilitado)
                    continue;

                {
                    cc++;
                    lists_nos[cc] = first_list + cc;
                    GL.NewList(first_list + cc, ListMode.Compile);

                    if (o.Selecionado)
                        GL.Color3(Color.Red);
                    else
                        GL.Color3(gerenciador.ConfiguracoesPGi.F3DOpcoesVisualizacao.CorNo);
                    o.Desenha(ref TamanhoNo);

                    GL.EndList();
                }
            }*/
        }
        public double EscalaDiagramas = 1;
        public double EscalaModoVibracao = 1;
        public double EscalaFlambagem = 1;

        public bool DeformacaoColorida, FlambagemColorido, FlambagemSolido, ModoVibracaoColorido, ModoVibracaoSolido, DeformacaoSolida = false;
        double h_, v, max, fatorRotacao;
        public double fatorDiagramas, fatorDeformacao, fatorModoVibracao, fatorFlambagem;
        double maxDef;
        public bool fx, fy, fz, mx, my, mz;
        TBarraGenerica bg;
        int first_list;
        int cc;
        float[] BatchTriangulos_Principal;
        float[] BatchTriangulos_Diagramas;

        float[] BatchTriangulos_Deformacao;
        float[] BatchArestas, BatchEixosLocais;
        Triangulo[] TriangulosSelecao, TriangulosBoundingBox, triangulos_temp;
        

        public void AtualizaShaders(bool atualizaTriangulos = true, bool eixosLocais = true)
        {
            try
            {              
                if (atualizaTriangulos)
                  PreencheBatchTriangulos();

                PreencheBatchArestas();
               
                if (eixosLocais)
                  PreencheBatchArestas_EixosLocais();
               
                AtualizaVBO(atualizaTriangulos);

                if (MostrarNos)
                  AtualizaDisplayList_Nos();
            }
            catch (Exception ee)
            {
                MessageBox.Show("Erro ao atualizar os shaders: " + ee.Message);
            }
        }

        public double EscalaCargas = 1;
        public double fatorCarga = 1;
        public List<string> dx_dy_dz = new List<string>(new string[] { "  ", "dx: ", "dz: ", "dy: ", "rx: ", "rz: ", "ry: " });

        double[] eqn1;
        public bool PlanoCorte;

       public static OpenTK.Matrix4d viewMatrix_Panning, /*viewMatrix,*/ viewMatrix_temp, modelViewMatrix_Zoom;

        public void CriarShaders(string vs, string fs,
                   out int vertexObject, out int fragmentObject,
                   out int program)
        {
            int status_code;
            string info;

            vertexObject   = GL.CreateShader(ShaderType.VertexShader);
            fragmentObject = GL.CreateShader(ShaderType.FragmentShader);

            // compila vertex shader
            GL.ShaderSource(vertexObject, vs);
            GL.CompileShader(vertexObject);
            GL.GetShaderInfoLog(vertexObject, out info);
            GL.GetShader(vertexObject, ShaderParameter.CompileStatus, out status_code);

            if (status_code != 1)
                throw new ApplicationException(info);

            // compila fragment shader
            GL.ShaderSource(fragmentObject, fs);
            GL.CompileShader(fragmentObject);
            GL.GetShaderInfoLog(fragmentObject, out info);
            GL.GetShader(fragmentObject, ShaderParameter.CompileStatus, out status_code);

            if (status_code != 1)
                throw new ApplicationException(info);

            program = GL.CreateProgram();
            GL.AttachShader(program, fragmentObject);
            GL.AttachShader(program, vertexObject);

            GL.LinkProgram(program);
            GL.UseProgram(program);

            GL.DetachShader(program, vertexObject);
            GL.DetachShader(program, fragmentObject);
            GL.DeleteShader(fragmentObject);
            GL.DeleteShader(vertexObject);
        }
        int qtd_coords_triangulos_principal  = 0;
        int qtd_coords_triangulos_deformacao = 0;
        int qtd_coords_triangulos_diagramas  = 0;

        int qtd_coords_arestas = 0;
        int qtd_coords_arestas_eixoslocais = 0;


        int VAO_triangulos_principal, VAO_triangulos_deformacoes, VAO_triangulos_diagramas, VAO_arestas, VAO_EixosLocais;

        /*        float[] batch_triangulos = new float[]{
   //coord                normal             cor                           textura
    -500,  200,  400, -1.0f,  0.0f,  0.0f, 1, 0, 0,                    0,0,
    -500,  200, -400, -1.0f,  0.0f,  0.0f, 0, 1, 0,                    1,0,
    -500, -200, -400, -1.0f,  0.0f,  0.0f, 0, 0, 1,                    1,1,
    -500, -200, -400, -1.0f,  0.0f,  0.0f, 0.24725f, 0.1995f, 0.0745f, 1,1,
    -500, -200,  400, -1.0f,  0.0f,  0.0f, 0.24725f, 0.1995f, 0.0745f, 0,1,
    -500,  200,  400, -1.0f,  0.0f,  0.0f, 0.24725f, 0.1995f, 0.0745f, 0,0
};*/
        public bool MostrarIndeformada;
        public bool MostrarTensoesEmSelecionados = false, MostrarDeslocamentosEmSelecionados = false, MostrarEsforcosEmSelecionados = false;
        public void GeraGradienteTensaoNormal(int filtro)
        {
            TGradienteCoresTensoes calcTensoes = new TGradienteCoresTensoes(Estrutura.PorticoEspacial.Ngl,
            Estrutura.PorticoEspacial,
            gerenciador.panelCores,
            gerenciador.labelCores,
            tipoCargaResultado, id_caso,
            id_combinacao,
            filtro == 1,
            MostraTensoesNormaisIsobandas);

            MostrarTensoesEmSelecionados = (filtro == 1);
        }

        public void DirtyPortico()
        {
            if (Estrutura.PorticoEspacial != null)
            {
                for (int i = 1; i <= Estrutura.PorticoEspacial.nBarras; i++)
                    Estrutura.PorticoEspacial.barras[i].DirtyTriangulos = true;
            }
        }
        public void PreencheBatchTriangulos()
        {
            try
            {
                List<float> coords_triangulos = new List<float>();
                List<float> coords_triangulos_diagramas = new List<float>();

                List<Triangulo> triangulos_selecao = new List<Triangulo>();

                List<float> coords_triangulos_deformacao = new List<float>();

                //    if (Unifilar)
                foreach (TBarraGenerica b in Estrutura.barras)
                {
                    if (b.DirtyTriangulos)                    
                       b.AtualizaTriangulos(MostraDeformacoes || MostraTensoesNormaisGradiente, Unifilar);

                    coords_triangulos.AddRange(b.BatchTriangulos);
                    triangulos_selecao.AddRange(b.TriangulosSelecao);


                    //  b.PreencheTriangulos(ref coords_triangulos, ref triangulos_selecao, MostraDeformacoes || MostraTensoesNormaisGradiente, Unifilar);
                }

                if (Unifilar)
                   coords_triangulos.Clear();

                foreach (TApoio a in Estrutura.apoios)
                    a.Preenche_Triangulos(ref coords_triangulos, tamApoios, ref triangulos_selecao);

                if (Estrutura.PorticoEspacial != null)
                {
                    if (Estrutura.PorticoEspacial.nBarras > 0 && (MostraDeformacoes || MostraTensoesNormaisGradiente || MostraModosVibracao || MostraModosFlambagem))
                    {
                        if (MostraModosVibracao && ModoVibracaoSolido)
                        {
                            double escalaModalQuadro = AnimarDeformacao ? iEscalaAnimacao : fatorModoVibracao;

                            for (j = 1; j <= Estrutura.PorticoEspacial.nBarras; j++)
                            {
                               if (Estrutura.PorticoEspacial.barras[j].DirtyTriangulos || AnimarDeformacao)
                               {
                                   //Estrutura.PorticoEspacial.barras[j].OrientaSecaoNoEspaco_ModoVibracao(ref escalaModalQuadro, gerenciador.cbModosVibracao.SelectedIndex);
                                   Estrutura.PorticoEspacial.barras[j].AtualizaTriangulos_ModoVibracao(ref coords_triangulos_deformacao, ref triangulos_selecao,
                                                                            ref escalaModalQuadro, 
                                                                            ref Arestas, 
                                                                            ref LinhaContornoDeformacao, 
                                                                            gerenciador.cbModosVibracao.SelectedIndex,
                                                                            ModoVibracaoColorido);
                               }

                               coords_triangulos_deformacao.AddRange(Estrutura.PorticoEspacial.barras[j].BatchTriangulos);
                            }
                        }
                        else
                        if (MostraModosFlambagem && FlambagemSolido)
                        {
                            double escalaModalQuadro = AnimarDeformacao ? iEscalaAnimacao : fatorFlambagem;

                            for (j = 1; j <= Estrutura.PorticoEspacial.nBarras; j++)
                            {
                                if (Estrutura.PorticoEspacial.barras[j].DirtyTriangulos || AnimarDeformacao)
                                {
                                    //Estrutura.PorticoEspacial.barras[j].OrientaSecaoNoEspaco_ModoVibracao(ref escalaModalQuadro, gerenciador.cbModosVibracao.SelectedIndex);
                                    Estrutura.PorticoEspacial.barras[j].AtualizaTriangulos_Flambagem(ref coords_triangulos_deformacao, ref triangulos_selecao,
                                                                             ref escalaModalQuadro,
                                                                             ref Arestas,
                                                                             ref LinhaContornoDeformacao,
                                                                             gerenciador.cbModosFlambagem.SelectedIndex,
                                                                             FlambagemColorido,
                                                                             gerenciador.id_combinacao_flambagem);
                                }

                                coords_triangulos_deformacao.AddRange(Estrutura.PorticoEspacial.barras[j].BatchTriangulos);
                            }
                        }
                        else
                        if (DeformacaoSolida || MostraTensoesNormaisGradiente)
                        {
                            maxDef = Estrutura.PorticoEspacial.MaximaRotacao(tipoCargaResultado, id_caso, id_combinacao);

                            fatorRotacao = (0.2 / maxDef) * EscalaDiagramas;
                           // if (!AnimarDeformacao)
                            {
                                if (!MostraTensoesNormaisGradiente)
                                {
                                    if (DeformacaoColorida)
                                    {
                                        TGradienteCoresDeformacao gercorDef = new TGradienteCoresDeformacao(Estrutura.PorticoEspacial.Ngl, Estrutura.PorticoEspacial,
                                        gerenciador.panelCores, gerenciador.labelCores, tipoCargaResultado, id_caso, id_combinacao, MostrarDeslocamentosEmSelecionados);
                                    }
                                }

                                if (MostraTensoesNormaisGradiente)
                                {
                                    for (j = 1; j <= Estrutura.PorticoEspacial.nBarras; j++)
                                    {
                                        if ((MostrarTensoesEmSelecionados && Estrutura.PorticoEspacial.barras[j].barraOriginal.Selecionado) || (!MostrarTensoesEmSelecionados))
                                        {
                                            Estrutura.PorticoEspacial.barras[j].OrientaSecaoNoEspaco(ref fatorDeformacao, tipoCargaResultado, id_caso, id_combinacao);
                                            Estrutura.PorticoEspacial.barras[j].PreencheTriangulosTensoes(ref coords_triangulos_deformacao, ref triangulos_selecao, MostrarIndeformada,
                                                                                   ref fatorDeformacao, ref Arestas, ref LinhaContornoDeformacao, ref deformacao_U,
                                                                                    tipoCargaResultado,
                                                                                    id_caso,
                                                                                    id_combinacao,
                                                                                    MostraTensoesNormaisGradiente,
                                                                                    MostraTensoesNormaisIsobandas);
                                        }
                                    }
                                }
                                else
                                {
                                    double escalaDefQuadro = AnimarDeformacao ? iEscalaAnimacao : fatorDeformacao;

                                    for (j = 1; j <= Estrutura.PorticoEspacial.nBarras; j++)
                                    {
                                        if ((MostrarDeslocamentosEmSelecionados && Estrutura.PorticoEspacial.barras[j].barraOriginal.Selecionado) || (!MostrarDeslocamentosEmSelecionados))
                                        {
                                            if (Estrutura.PorticoEspacial.barras[j].DirtyTriangulos || AnimarDeformacao)
                                            {
                                                Estrutura.PorticoEspacial.barras[j].OrientaSecaoNoEspaco(ref escalaDefQuadro, tipoCargaResultado, id_caso, id_combinacao);
                                                Estrutura.PorticoEspacial.barras[j].AtualizaTriangulos(ref coords_triangulos_deformacao, ref triangulos_selecao, MostrarIndeformada,
                                                                                       ref escalaDefQuadro, ref Arestas, ref LinhaContornoDeformacao, ref deformacao_U,
                                                                                        tipoCargaResultado, id_caso, id_combinacao, DeformacaoColorida, MostraTensoesNormaisGradiente, MostraTensoesNormaisIsobandas);
                                            }

                                      //   if (!AnimarDeformacao)
                                        }

                                        coords_triangulos_deformacao.AddRange(Estrutura.PorticoEspacial.barras[j].BatchTriangulos);
                                    }
                                }
                            }
                           /* else
                            {
                                for (j = 1; j <= Estrutura.PorticoEspacial.nBarras; j++)
                                {
                                    Application.DoEvents();
                                    Estrutura.PorticoEspacial.barras[j].OrientaSecaoNoEspaco(ref iEscalaAnimacao, tipoCargaResultado, id_caso, id_combinacao);
                                    Estrutura.PorticoEspacial.barras[j].PreencheTriangulos(ref coords_triangulos_deformacao, ref triangulos_selecao, MostrarIndeformada,
                                                                           ref iEscalaAnimacao, ref Arestas, ref LinhaContornoDeformacao, ref deformacao_U,
                                                                            tipoCargaResultado, id_caso, id_combinacao, DeformacaoColorida, MostraTensoesNormaisGradiente, MostraTensoesNormaisIsobandas);
                                }
                            }*/
                        }
                    }

                    if (Diagrama_gradiente)
                    {
                        if (my) 
                        {
                           for (j = 1; j <= Estrutura.PorticoEspacial.nBarras; j++)
                           {
                              if ((MostrarEsforcosEmSelecionados && Estrutura.PorticoEspacial.barras[j].barraOriginal.Selecionado) || (!MostrarEsforcosEmSelecionados))
                              {
                                 Estrutura.PorticoEspacial.barras[j].OrientaSecaoNoEspaco(ref fatorDiagramas, tipoCargaResultado, id_caso, id_combinacao);
                                 Estrutura.PorticoEspacial.barras[j].MY_Positivos_Negativos(tipoCargaResultado, id_caso, id_combinacao);
                              }
                           }

                           TGradienteCoresDiagramas gercorMY = new TGradienteCoresDiagramas(
                               Estrutura.PorticoEspacial.Ngl, 
                               Estrutura.PorticoEspacial,
                               gerenciador.panelCores, 
                               gerenciador.labelCores,
                               "MY",
                               tipoCargaResultado, 
                               id_caso, 
                               id_combinacao, 
                               MostrarEsforcosEmSelecionados);

                            for (j = 1; j <= Estrutura.PorticoEspacial.nBarras; j++)
                            {
                                if ((MostrarEsforcosEmSelecionados && Estrutura.PorticoEspacial.barras[j].barraOriginal.Selecionado) || (!MostrarEsforcosEmSelecionados))
                                {
                                    Estrutura.PorticoEspacial.barras[j].Preenche_MY_Gradiente(ref coords_triangulos_diagramas, ref fatorDiagramas,
                                                                                         tipoCargaResultado, id_caso, id_combinacao);
                                }
                            }
                        }
                        else
                        if (mz)
                        {
                            for (j = 1; j <= Estrutura.PorticoEspacial.nBarras; j++)
                            {
                                if ((MostrarEsforcosEmSelecionados && Estrutura.PorticoEspacial.barras[j].barraOriginal.Selecionado) || (!MostrarEsforcosEmSelecionados))
                                {
                                    Estrutura.PorticoEspacial.barras[j].OrientaSecaoNoEspaco(ref fatorDiagramas, tipoCargaResultado, id_caso, id_combinacao);
                                    Estrutura.PorticoEspacial.barras[j].MZ_Positivos_Negativos(tipoCargaResultado, id_caso, id_combinacao);
                                }
                            }

                            TGradienteCoresDiagramas gercorMZ = new TGradienteCoresDiagramas(Estrutura.PorticoEspacial.Ngl, 
                                Estrutura.PorticoEspacial, gerenciador.panelCores, gerenciador.labelCores, 
                                "MZ",
                             tipoCargaResultado, id_caso, id_combinacao, MostrarEsforcosEmSelecionados);

                            for (j = 1; j <= Estrutura.PorticoEspacial.nBarras; j++)
                            {
                                if ((MostrarEsforcosEmSelecionados && Estrutura.PorticoEspacial.barras[j].barraOriginal.Selecionado) || (!MostrarEsforcosEmSelecionados))
                                {
                                    Estrutura.PorticoEspacial.barras[j].Preenche_MZ_Gradiente(ref coords_triangulos_diagramas, ref fatorDiagramas,
                                                                                            tipoCargaResultado, id_caso, id_combinacao);
                                }
                            }
                        }
                        else
                        if (mx)
                        {
                            for (j = 1; j <= Estrutura.PorticoEspacial.nBarras; j++)
                            {
                                if ((MostrarEsforcosEmSelecionados && Estrutura.PorticoEspacial.barras[j].barraOriginal.Selecionado) || (!MostrarEsforcosEmSelecionados))
                                {
                                    Estrutura.PorticoEspacial.barras[j].OrientaSecaoNoEspaco(ref fatorDiagramas, tipoCargaResultado, id_caso, id_combinacao);
                                    Estrutura.PorticoEspacial.barras[j].MX_Positivos_Negativos(tipoCargaResultado, id_caso, id_combinacao);
                                }
                            }

                            TGradienteCoresDiagramas gercorMX = new TGradienteCoresDiagramas(Estrutura.PorticoEspacial.Ngl,
                                Estrutura.PorticoEspacial, gerenciador.panelCores, 
                                gerenciador.labelCores, "MX",
                             tipoCargaResultado, id_caso, id_combinacao, MostrarEsforcosEmSelecionados);

                            for (j = 1; j <= Estrutura.PorticoEspacial.nBarras; j++)
                            {
                                if ((MostrarEsforcosEmSelecionados && Estrutura.PorticoEspacial.barras[j].barraOriginal.Selecionado) || (!MostrarEsforcosEmSelecionados))
                                {
                                    Estrutura.PorticoEspacial.barras[j].Preenche_MX_Gradiente(ref coords_triangulos_diagramas, ref fatorDiagramas,
                                                                                              tipoCargaResultado, id_caso, id_combinacao);
                                }
                            }
                        }
                        else
                        if (fx)
                        {
                            for (j = 1; j <= Estrutura.PorticoEspacial.nBarras; j++)
                            {
                                if ((MostrarEsforcosEmSelecionados && Estrutura.PorticoEspacial.barras[j].barraOriginal.Selecionado) || (!MostrarEsforcosEmSelecionados))
                                {
                                    Estrutura.PorticoEspacial.barras[j].OrientaSecaoNoEspaco(ref fatorDiagramas, tipoCargaResultado, id_caso, id_combinacao);
                                    Estrutura.PorticoEspacial.barras[j].FX_Positivos_Negativos(tipoCargaResultado, id_caso, id_combinacao);
                                }
                            }

                            TGradienteCoresDiagramas gercorFX = new TGradienteCoresDiagramas(Estrutura.PorticoEspacial.Ngl,
                                Estrutura.PorticoEspacial, gerenciador.panelCores, 
                                gerenciador.labelCores, "FX",
                             tipoCargaResultado, id_caso, id_combinacao, MostrarEsforcosEmSelecionados);

                            for (j = 1; j <= Estrutura.PorticoEspacial.nBarras; j++)
                            {
                                if ((MostrarEsforcosEmSelecionados && Estrutura.PorticoEspacial.barras[j].barraOriginal.Selecionado) || (!MostrarEsforcosEmSelecionados))
                                {
                                    Estrutura.PorticoEspacial.barras[j].Preenche_FX_Gradiente(ref coords_triangulos_diagramas, ref fatorDiagramas,
                                                                                          tipoCargaResultado, id_caso, id_combinacao);
                                }
                            }
                        }
                        else
                        if (fz)
                        {
                            for (j = 1; j <= Estrutura.PorticoEspacial.nBarras; j++)
                            {
                                if ((MostrarEsforcosEmSelecionados && Estrutura.PorticoEspacial.barras[j].barraOriginal.Selecionado) || (!MostrarEsforcosEmSelecionados))
                                {
                                    Estrutura.PorticoEspacial.barras[j].OrientaSecaoNoEspaco(ref fatorDiagramas, tipoCargaResultado, id_caso, id_combinacao);
                                    Estrutura.PorticoEspacial.barras[j].FZ_Positivos_Negativos(tipoCargaResultado, id_caso, id_combinacao);
                                }
                            }

                            TGradienteCoresDiagramas gercorFZ = new TGradienteCoresDiagramas(Estrutura.PorticoEspacial.Ngl, 
                                Estrutura.PorticoEspacial, gerenciador.panelCores, 
                                gerenciador.labelCores, "FZ",
                             tipoCargaResultado, id_caso, id_combinacao, MostrarEsforcosEmSelecionados);

                            for (j = 1; j <= Estrutura.PorticoEspacial.nBarras; j++)
                            {
                                if ((MostrarEsforcosEmSelecionados && Estrutura.PorticoEspacial.barras[j].barraOriginal.Selecionado) || (!MostrarEsforcosEmSelecionados))
                                {
                                    Estrutura.PorticoEspacial.barras[j].Preenche_FZ_Gradiente(ref coords_triangulos_diagramas, ref fatorDiagramas,
                                                                                            tipoCargaResultado, id_caso, id_combinacao);
                                }
                            }
                        }
                        else
                        if (fy)
                        {
                            for (j = 1; j <= Estrutura.PorticoEspacial.nBarras; j++)
                            {
                                if ((MostrarEsforcosEmSelecionados && Estrutura.PorticoEspacial.barras[j].barraOriginal.Selecionado) || (!MostrarEsforcosEmSelecionados))
                                {
                                    Estrutura.PorticoEspacial.barras[j].OrientaSecaoNoEspaco(ref fatorDiagramas, tipoCargaResultado, id_caso, id_combinacao);
                                    Estrutura.PorticoEspacial.barras[j].FY_Positivos_Negativos(tipoCargaResultado, id_caso, id_combinacao);
                                }
                            }

                            TGradienteCoresDiagramas gercorF = new TGradienteCoresDiagramas(Estrutura.PorticoEspacial.Ngl, Estrutura.PorticoEspacial, gerenciador.panelCores, gerenciador.labelCores, "FY",
                                                                                         tipoCargaResultado, id_caso, id_combinacao, MostrarEsforcosEmSelecionados);

                            for (j = 1; j <= Estrutura.PorticoEspacial.nBarras; j++)
                            {
                                if ((MostrarEsforcosEmSelecionados && Estrutura.PorticoEspacial.barras[j].barraOriginal.Selecionado) || (!MostrarEsforcosEmSelecionados))
                                {
                                    Estrutura.PorticoEspacial.barras[j].Preenche_FY_Gradiente(ref coords_triangulos_diagramas, ref fatorDiagramas,
                                                                                         tipoCargaResultado, id_caso, id_combinacao);
                                }
                            }
                        }
                        BatchTriangulos_Diagramas = new float[coords_triangulos_diagramas.Count];
                        BatchTriangulos_Diagramas = coords_triangulos_diagramas.ToArray();
                    }
                }

                BatchTriangulos_Principal = new float[coords_triangulos.Count];
                BatchTriangulos_Principal = coords_triangulos.ToArray();

                BatchTriangulos_Deformacao = new float[coords_triangulos_deformacao.Count];
                BatchTriangulos_Deformacao = coords_triangulos_deformacao.ToArray();

                TriangulosSelecao = new Triangulo[triangulos_selecao.Count];
                TriangulosSelecao = triangulos_selecao.ToArray();
            }
            catch (Exception ee)
            {
                MessageBox.Show("erro PreencheBatchTriangulos: " + ee.Message);
            }
        }
        public void PreencheBatchArestas_EixosLocais()
        {
            try
            {
                List<float> coords_arestas_EixosLocais = new List<float>();

                if (EixosLocais)
                {
                    foreach (TBarraGenerica b in Estrutura.barras)
                        b.Preenche_Arestas_EixosLocais(ref coords_arestas_EixosLocais);
                }

                BatchEixosLocais = new float[coords_arestas_EixosLocais.Count];
                BatchEixosLocais = coords_arestas_EixosLocais.ToArray();
            }
            catch (Exception ee)
            {
                MessageBox.Show("erro PreencheBatchTriangulos: " + ee.Message);
            }
        }
        public void PreencheBatchArestas()
        {
            try
            {
                List<float> coords_arestas = new List<float>();
                List<float> coords_arestas_EixosLocais = new List<float>();

                foreach (TBarraGenerica b in Estrutura.barras)
                {
                    if (b.DirtyArestas)
                       b.AtualizaArestas(Arestas, ArestasConfObjeto, Unifilar);

                    coords_arestas.AddRange(b.BatchArestas);

              //      b.Preenche_Arestas(ref coords_arestas, ref Arestas, ref ArestasConfObjeto, ref Unifilar);
                }

                foreach (TApoio b in Estrutura.apoios)
                    b.Preenche_Arestas(ref coords_arestas,tamApoios, ref Arestas, ref ArestasConfObjeto, ref Unifilar);

               /* if (Estrutura.apoios.Count> 0)
                {
                    PreencheBatchTriangulos();
                    AtualizaVBO();
                }
                */

               /* if (EixosLocais)
                {
                    foreach (TBarraGenerica b in Estrutura.barras)
                        b.Preenche_Arestas_EixosLocais(ref coords_arestas_EixosLocais);
                }*/

                foreach (TBarraGenerica b in Estrutura.barras)
                    b.Preenche_Arestas_Articulacoes(ref coords_arestas);

                if (Estrutura.PorticoEspacial != null)
                {
                    if (Estrutura.PorticoEspacial.nBarras > 0 && (MostraDeformacoes || MostraTensoesNormaisGradiente || MostraModosVibracao || MostraModosFlambagem))
                    {
                        if (Arestas)
                        {
                            if (DeformacaoSolida)
                            {
                                for (i = 1; i <= Estrutura.PorticoEspacial.nBarras; i++)
                                {
                                    if ((MostrarDeslocamentosEmSelecionados && Estrutura.PorticoEspacial.barras[i].barraOriginal.Selecionado) || (!MostrarDeslocamentosEmSelecionados))
                                        Estrutura.PorticoEspacial.barras[i].Preenche_Arestas_Solido(ref coords_arestas, MostrarIndeformada, ref LinhaContornoDeformacao, ref ArestasConfObjeto);
                                }
                            }
                            else
                            if (MostraTensoesNormaisGradiente)
                            {
                                for (i = 1; i <= Estrutura.PorticoEspacial.nBarras; i++)
                                {
                                    if ((MostrarTensoesEmSelecionados && Estrutura.PorticoEspacial.barras[i].barraOriginal.Selecionado) || (!MostrarTensoesEmSelecionados))
                                        Estrutura.PorticoEspacial.barras[i].Preenche_Arestas_Solido_Tensoes(ref coords_arestas, ref LinhaContornoDeformacao, ref ArestasConfObjeto);
                                }
                            }
                            else
                            if (ModoVibracaoSolido || FlambagemSolido)
                            {
                                for (i = 1; i <= Estrutura.PorticoEspacial.nBarras; i++)
                                    Estrutura.PorticoEspacial.barras[i].Preenche_Arestas_Solido(ref coords_arestas, MostrarIndeformada, ref LinhaContornoDeformacao, ref ArestasConfObjeto);
                            }

                        }

                        if (MostraModosVibracao)
                        {
                            if (!ModoVibracaoSolido)
                            {
                                double fator = AnimarDeformacao ? iEscalaAnimacao : fatorModoVibracao;
                                for (i = 1; i <= Estrutura.PorticoEspacial.nBarras; i++)
                                    Estrutura.PorticoEspacial.barras[i].Preenche_Barra_ModoVibracao(ref coords_arestas, ref fator, ref ModoVibracaoColorido, ref ArestasResultado, gerenciador.cbModosVibracao.SelectedIndex);
                            }
                        }
                        else
                        if (MostraModosFlambagem)
                        {
                            if (!FlambagemSolido)
                            {
                                double fator = AnimarDeformacao ? iEscalaAnimacao : fatorFlambagem;
                                for (i = 1; i <= Estrutura.PorticoEspacial.nBarras; i++)
                                   Estrutura.PorticoEspacial.barras[i].Preenche_Barra_Flambagem(ref coords_arestas, ref fator, ref FlambagemColorido, ref ArestasResultado, gerenciador.cbModosFlambagem.SelectedIndex,gerenciador.id_combinacao_flambagem);
                            }
                        }
                        else
                        if (!DeformacaoSolida)
                        {
                            if (!AnimarDeformacao)
                            {
                                if (DeformacaoColorida)
                                {
                                    TGradienteCoresDeformacao gercorDef = new TGradienteCoresDeformacao(Estrutura.PorticoEspacial.Ngl, Estrutura.PorticoEspacial,
                                    gerenciador.panelCores, gerenciador.labelCores, tipoCargaResultado, id_caso, id_combinacao, MostrarDeslocamentosEmSelecionados);
                                }

                                if (MostraTensoesNormaisGradiente)
                                {

                                }
                                else
                                {
                                    for (i = 1; i <= Estrutura.PorticoEspacial.nBarras; i++)
                                        if ((MostrarDeslocamentosEmSelecionados && Estrutura.PorticoEspacial.barras[i].barraOriginal.Selecionado) || (!MostrarDeslocamentosEmSelecionados))
                                            Estrutura.PorticoEspacial.barras[i].Preenche_Barra(ref coords_arestas, MostrarIndeformada, ref fatorDeformacao, ref DeformacaoColorida, ref ArestasResultado, ref deformacao_U,
                                                                               tipoCargaResultado,
                                                                               id_caso,
                                                                               id_combinacao);
                                }
                            }
                            else
                            {
                                for (i = 1; i <= Estrutura.PorticoEspacial.nBarras; i++)
                                    Estrutura.PorticoEspacial.barras[i].Preenche_Barra(ref coords_arestas, MostrarIndeformada, ref iEscalaAnimacao, ref DeformacaoColorida, ref ArestasResultado, ref deformacao_U,
                                                                             tipoCargaResultado,
                                                                             id_caso,
                                                                             id_combinacao);
                            }
                        }
                    }

                    if (my)
                    {
                        for (j = 1; j <= Estrutura.PorticoEspacial.nBarras; j++)
                        {
                            if ((MostrarEsforcosEmSelecionados && Estrutura.PorticoEspacial.barras[j].barraOriginal.Selecionado) || (!MostrarEsforcosEmSelecionados))
                            { 
                               Estrutura.PorticoEspacial.barras[j].OrientaSecaoNoEspaco(ref fatorDiagramas, tipoCargaResultado, id_caso, id_combinacao);
                               Estrutura.PorticoEspacial.barras[j].MY_Positivos_Negativos(tipoCargaResultado, id_caso, id_combinacao);
                            }    
                        }
                        
                        for (j = 1; j <= Estrutura.PorticoEspacial.nBarras; j++)
                        {
                            //   Estrutura.PorticoEspacial.barras[j].RotacionaNoEspaco(ref fatorDiagramas, tipoCargaResultado, id_caso, id_combinacao);
                            if ((MostrarEsforcosEmSelecionados && Estrutura.PorticoEspacial.barras[j].barraOriginal.Selecionado) || (!MostrarEsforcosEmSelecionados))
                            {
                                Estrutura.PorticoEspacial.barras[j].Preenche_MY(ref coords_arestas, ref fatorDiagramas,
                                 ref Diagrama_linha_gradiente, ref Diagrama_gradiente,
                                 ref Diagrama_somente_linhas, ref Diagrama_linha_contorno,
                                 ref CorArestaDiagrama,
                                 tipoCargaResultado, id_caso, id_combinacao);
                            }
                        }
                    }
                    else
                    if (fx)
                    {
                        for (j = 1; j <= Estrutura.PorticoEspacial.nBarras; j++)
                        {
                            if ((MostrarEsforcosEmSelecionados && Estrutura.PorticoEspacial.barras[j].barraOriginal.Selecionado) || (!MostrarEsforcosEmSelecionados))
                            {
                                Estrutura.PorticoEspacial.barras[j].OrientaSecaoNoEspaco(ref fatorDiagramas, tipoCargaResultado, id_caso, id_combinacao);
                                Estrutura.PorticoEspacial.barras[j].FX_Positivos_Negativos(tipoCargaResultado, id_caso, id_combinacao);
                            }
                        }

                        for (j = 1; j <= Estrutura.PorticoEspacial.nBarras; j++)
                        {
                            //      Estrutura.PorticoEspacial.barras[j].RotacionaNoEspaco(ref fatorDiagramas, tipoCargaResultado, id_caso, id_combinacao);
                            if ((MostrarEsforcosEmSelecionados && Estrutura.PorticoEspacial.barras[j].barraOriginal.Selecionado) || (!MostrarEsforcosEmSelecionados))
                            {
                                Estrutura.PorticoEspacial.barras[j].Arestas_FX(ref coords_arestas, ref fatorDiagramas,
                                ref Diagrama_linha_gradiente, ref Diagrama_gradiente,
                                ref Diagrama_somente_linhas, ref Diagrama_linha_contorno,
                                ref CorArestaDiagrama,
                                tipoCargaResultado, id_caso, id_combinacao);
                            }
                        }
                    }
                    else
                    if (fz)
                    {
                        for (j = 1; j <= Estrutura.PorticoEspacial.nBarras; j++)
                        {
                            if ((MostrarEsforcosEmSelecionados && Estrutura.PorticoEspacial.barras[j].barraOriginal.Selecionado) || (!MostrarEsforcosEmSelecionados))
                            {
                                Estrutura.PorticoEspacial.barras[j].OrientaSecaoNoEspaco(ref fatorDiagramas, tipoCargaResultado, id_caso, id_combinacao);
                                Estrutura.PorticoEspacial.barras[j].FZ_Positivos_Negativos(tipoCargaResultado, id_caso, id_combinacao);
                            }
                        }

                        for (j = 1; j <= Estrutura.PorticoEspacial.nBarras; j++)
                        {
                            // Estrutura.PorticoEspacial.barras[j].RotacionaNoEspaco(ref fatorDiagramas, tipoCargaResultado, id_caso, id_combinacao);
                            if ((MostrarEsforcosEmSelecionados && Estrutura.PorticoEspacial.barras[j].barraOriginal.Selecionado) || (!MostrarEsforcosEmSelecionados))
                            {
                                Estrutura.PorticoEspacial.barras[j].Arestas_FZ(ref coords_arestas, ref fatorDiagramas,
                                ref Diagrama_linha_gradiente, ref Diagrama_gradiente,
                                ref Diagrama_somente_linhas, ref Diagrama_linha_contorno,
                                ref CorArestaDiagrama,
                                tipoCargaResultado, id_caso, id_combinacao);
                            }
                        }
                    }
                    else
                    if (fy)
                    {
                        for (j = 1; j <= Estrutura.PorticoEspacial.nBarras; j++)
                        {
                            if ((MostrarEsforcosEmSelecionados && Estrutura.PorticoEspacial.barras[j].barraOriginal.Selecionado) || (!MostrarEsforcosEmSelecionados))
                            {
                                Estrutura.PorticoEspacial.barras[j].OrientaSecaoNoEspaco(ref fatorDiagramas, tipoCargaResultado, id_caso, id_combinacao);
                                Estrutura.PorticoEspacial.barras[j].FY_Positivos_Negativos(tipoCargaResultado, id_caso, id_combinacao);
                            }
                        }

                        for (j = 1; j <= Estrutura.PorticoEspacial.nBarras; j++)
                        {
                            if ((MostrarEsforcosEmSelecionados && Estrutura.PorticoEspacial.barras[j].barraOriginal.Selecionado) || (!MostrarEsforcosEmSelecionados))
                            {
                                Estrutura.PorticoEspacial.barras[j].Arestas_FY(ref coords_arestas, ref fatorDiagramas,
                                ref Diagrama_linha_gradiente, ref Diagrama_gradiente,
                                ref Diagrama_somente_linhas, ref Diagrama_linha_contorno,
                                 ref CorArestaDiagrama,
                                tipoCargaResultado, id_caso, id_combinacao);
                            }
                        }
                    }
                    else
                    if (mz)
                    {
                        for (j = 1; j <= Estrutura.PorticoEspacial.nBarras; j++)
                        {
                            if ((MostrarEsforcosEmSelecionados && Estrutura.PorticoEspacial.barras[j].barraOriginal.Selecionado) || (!MostrarEsforcosEmSelecionados))
                            {
                                Estrutura.PorticoEspacial.barras[j].OrientaSecaoNoEspaco(ref fatorDiagramas, tipoCargaResultado, id_caso, id_combinacao);
                                Estrutura.PorticoEspacial.barras[j].MZ_Positivos_Negativos(tipoCargaResultado, id_caso, id_combinacao);
                            }
                        }

                        for (j = 1; j <= Estrutura.PorticoEspacial.nBarras; j++)
                        {
                            if ((MostrarEsforcosEmSelecionados && Estrutura.PorticoEspacial.barras[j].barraOriginal.Selecionado) || (!MostrarEsforcosEmSelecionados))
                            {
                                Estrutura.PorticoEspacial.barras[j].Preenche_MZ(ref coords_arestas, ref fatorDiagramas,
                                ref Diagrama_linha_gradiente, ref Diagrama_gradiente,
                                ref Diagrama_somente_linhas, ref Diagrama_linha_contorno,
                                ref CorArestaDiagrama,
                                tipoCargaResultado, id_caso, id_combinacao);
                            }
                        }
                    }
                    else
                    if (mx)
                    {

                        for (j = 1; j <= Estrutura.PorticoEspacial.nBarras; j++)
                        {
                            if ((MostrarEsforcosEmSelecionados && Estrutura.PorticoEspacial.barras[j].barraOriginal.Selecionado) || (!MostrarEsforcosEmSelecionados))
                            {
                                Estrutura.PorticoEspacial.barras[j].OrientaSecaoNoEspaco(ref fatorDiagramas, tipoCargaResultado, id_caso, id_combinacao);
                                Estrutura.PorticoEspacial.barras[j].MX_Positivos_Negativos(tipoCargaResultado, id_caso, id_combinacao);
                            }
                        }

                        for (j = 1; j <= Estrutura.PorticoEspacial.nBarras; j++)
                        {
                            if ((MostrarEsforcosEmSelecionados && Estrutura.PorticoEspacial.barras[j].barraOriginal.Selecionado) || (!MostrarEsforcosEmSelecionados))
                            {
                                Estrutura.PorticoEspacial.barras[j].Preenche_MX(ref coords_arestas, ref fatorDiagramas,
                                ref Diagrama_linha_gradiente, ref Diagrama_gradiente,
                                ref Diagrama_somente_linhas, ref Diagrama_linha_contorno,
                                ref CorArestaDiagrama,
                                tipoCargaResultado, id_caso, id_combinacao);
                            }
                        }
                    }

                }

                if (MostrarCargas)
                {
                    if (gerenciador.cbCasoCarga.Items.Count > 0)
                        if (gerenciador.cbCasoCarga.SelectedIndex > 0)
                        {

                            max = 0;
                            foreach (TCargaLinear o in CargasBarrasAtuais)
                                if (Math.Abs(o.Dados.valor) > max && o.Dados.valor != 0)
                                    max = Math.Abs(o.Dados.valor);

                            foreach (TCargaPontual o in CargasNosAtuais)
                                if (Math.Abs(o.Dados.valor) > max && o.Dados.valor != 0)
                                    max = Math.Abs(o.Dados.valor);

                            if (max > 0)
                                fatorCarga = (1 / max) * EscalaCargas;

                            foreach (TCargaLinear o in CargasBarrasAtuais)
                            {
                                bg = Estrutura.barras.Find(b => b.IDBarra == o.idBarra && b.Dados.Tipo != 4);
                                if (bg != null)
                                {
                                     h_ = bg.Dados.secao.b1;
                                     v = bg.Dados.secao.h1;

                                      //  o.CriaSetas(Unifilar, v, h_, fatorCarga);
                                      o.Preenche_Arestas(ref coords_arestas);
                                }
                            }

                            foreach (TCargaPontual o in CargasNosAtuais)
                            {
                                o.CriaSeta(Unifilar, fatorCarga);
                                o.Preenche_Arestas(ref coords_arestas);
                            }
                        }
                }


                BatchArestas = new float[coords_arestas.Count];
                BatchArestas = coords_arestas.ToArray();

               // BatchEixosLocais = new float[coords_arestas_EixosLocais.Count];
             //  BatchEixosLocais = coords_arestas_EixosLocais.ToArray();
            }
            catch (Exception ee)
            {
                MessageBox.Show("erro PreencheBatcharestas: " + ee.Message);
            }
        }

        public void AtualizaVBO(bool atualizaTriangulos = true)
        {
            int size = 0;

            if (atualizaTriangulos)
            {
                if (VAO_triangulos_principal != 0)
                {
                    GL.DeleteBuffers(1, ref VAO_triangulos_principal);
                    GL.DeleteVertexArray(VAO_triangulos_principal);
                }
                //ptr.ToInt64()
                VAO_triangulos_principal = GL.GenVertexArray();
                GL.BindVertexArray(VAO_triangulos_principal);
                GL.GenBuffers(1, out vertex_buffer_triangulos_principal);
                GL.BindBuffer(BufferTarget.ArrayBuffer, vertex_buffer_triangulos_principal);

                IntPtr ptr = (IntPtr)(BatchTriangulos_Principal.Length * sizeof(float));
                long valor = ptr.ToInt64();

                GL.BufferData(BufferTarget.ArrayBuffer, (int)valor, BatchTriangulos_Principal, BufferUsageHint.StaticDraw);
                qtd_coords_triangulos_principal = BatchTriangulos_Principal.Length / 11;
                //coord                normal             cor                           textura
                GL.VertexAttribPointer(0, 3, VertexAttribPointerType.Float, false, 11 * sizeof(float), IntPtr.Zero);
                GL.VertexAttribPointer(1, 3, VertexAttribPointerType.Float, false, 11 * sizeof(float), 3 * sizeof(float));
                GL.VertexAttribPointer(2, 3, VertexAttribPointerType.Float, false, 11 * sizeof(float), 6 * sizeof(float));
                GL.VertexAttribPointer(3, 2, VertexAttribPointerType.Float, false, 11 * sizeof(float), 9 * sizeof(float));
                GL.EnableVertexAttribArray(0);
                GL.EnableVertexAttribArray(1);
                GL.EnableVertexAttribArray(2);
                GL.EnableVertexAttribArray(3);

                //   CarregaTexturas();

                GL.GetBufferParameter(BufferTarget.ArrayBuffer, BufferParameterName.BufferSize, out size);

                if (size != BatchTriangulos_Principal.Length * sizeof(float))
                    throw new ApplicationException(string.Format(
                        "Problema ao atualizar VBO dos triângulos principais. Foi tentado carregar {0} bytes, carregado {1}.",
                        BatchTriangulos_Principal.Length * sizeof(float), size));


                /*-------------------------*/

                if (VAO_triangulos_deformacoes != 0)
                {
                    GL.DeleteBuffers(1, ref VAO_triangulos_deformacoes);
                    GL.DeleteVertexArray(VAO_triangulos_deformacoes);
                }

                VAO_triangulos_deformacoes = GL.GenVertexArray();
                GL.BindVertexArray(VAO_triangulos_deformacoes);
                GL.GenBuffers(1, out vertex_buffer_triangulos_deformacao);
                GL.BindBuffer(BufferTarget.ArrayBuffer, vertex_buffer_triangulos_deformacao);
                GL.BufferData(BufferTarget.ArrayBuffer, (IntPtr)(BatchTriangulos_Deformacao.Length * sizeof(float)), BatchTriangulos_Deformacao, BufferUsageHint.StaticDraw);
                qtd_coords_triangulos_deformacao = BatchTriangulos_Deformacao.Length / 11;
                                       //coord   normal             cor                           textura
                GL.VertexAttribPointer(0, 3, VertexAttribPointerType.Float, false, 11 * sizeof(float), IntPtr.Zero);
                GL.VertexAttribPointer(1, 3, VertexAttribPointerType.Float, false, 11 * sizeof(float), 3 * sizeof(float));
                GL.VertexAttribPointer(2, 3, VertexAttribPointerType.Float, false, 11 * sizeof(float), 6 * sizeof(float));
                GL.VertexAttribPointer(3, 2, VertexAttribPointerType.Float, false, 11 * sizeof(float), 9 * sizeof(float));
                GL.EnableVertexAttribArray(0);
                GL.EnableVertexAttribArray(1);
                GL.EnableVertexAttribArray(2);
                GL.EnableVertexAttribArray(3);

                // CarregaTexturas();

                GL.GetBufferParameter(BufferTarget.ArrayBuffer, BufferParameterName.BufferSize, out size);

                if (size != BatchTriangulos_Deformacao.Length * sizeof(float))
                    throw new ApplicationException(string.Format(
                        "Problema ao atualizar VBO dos triângulos da deformação. Foi tentado carregar {0} bytes, carregado {1}.",
                        BatchTriangulos_Deformacao.Length * sizeof(float), size));

                /*-------------------------*/

                if (Diagrama_gradiente && (mz || mx || my || fx || fz || fy))
                {
                    if (VAO_triangulos_diagramas != 0)
                    {
                        GL.DeleteBuffers(1, ref VAO_triangulos_diagramas);
                        GL.DeleteVertexArray(VAO_triangulos_diagramas);
                    }

                    VAO_triangulos_diagramas = GL.GenVertexArray();
                    GL.BindVertexArray(VAO_triangulos_diagramas);
                    GL.GenBuffers(1, out vertex_buffer_triangulos_diagramas);
                    GL.BindBuffer(BufferTarget.ArrayBuffer, vertex_buffer_triangulos_diagramas);
                    GL.BufferData(BufferTarget.ArrayBuffer, (IntPtr)(BatchTriangulos_Diagramas.Length * sizeof(float)), BatchTriangulos_Diagramas, BufferUsageHint.StaticDraw);
                    qtd_coords_triangulos_diagramas = BatchTriangulos_Diagramas.Length / 6;
                    //coord                             cor                          
                    GL.VertexAttribPointer(0, 3, VertexAttribPointerType.Float, false, 6 * sizeof(float), IntPtr.Zero);
                    GL.VertexAttribPointer(1, 3, VertexAttribPointerType.Float, false, 6 * sizeof(float), 3 * sizeof(float));
                    GL.EnableVertexAttribArray(0);
                    GL.EnableVertexAttribArray(1);

                    GL.GetBufferParameter(BufferTarget.ArrayBuffer, BufferParameterName.BufferSize, out size);

                    if (size != BatchTriangulos_Diagramas.Length * sizeof(float))
                        throw new ApplicationException(string.Format(
                            "Problema ao atualizar VBO dos triângulos dos diagramas. Foi tentado carregar {0} bytes, carregado {1}.",
                            BatchTriangulos_Diagramas.Length * sizeof(float), size));
                }

                /*-------------------------*/
            }

            GL.EnableClientState(ArrayCap.VertexArray);


            if (VAO_arestas != 0)
            {
                GL.DeleteBuffers(1, ref VAO_arestas);
                GL.DeleteVertexArray(VAO_arestas);
            }
            VAO_arestas = GL.GenVertexArray();
            GL.BindVertexArray(VAO_arestas);
            GL.GenBuffers(1, out vertex_buffer_arestas);
            GL.BindBuffer(BufferTarget.ArrayBuffer, vertex_buffer_arestas);
            GL.BufferData(BufferTarget.ArrayBuffer, (IntPtr)(BatchArestas.Length * sizeof(float)), BatchArestas, BufferUsageHint.StaticDraw);
            qtd_coords_arestas = BatchArestas.Length / 6;
            //coord                cor                          
            GL.VertexAttribPointer(0, 3, VertexAttribPointerType.Float, false, 6 * sizeof(float), IntPtr.Zero);
            GL.VertexAttribPointer(1, 3, VertexAttribPointerType.Float, false, 6 * sizeof(float), 3 * sizeof(float));
            GL.EnableVertexAttribArray(0);
            GL.EnableVertexAttribArray(1);
            GL.GetBufferParameter(BufferTarget.ArrayBuffer, BufferParameterName.BufferSize, out size);

            if (size != BatchArestas.Length * sizeof(float))
                throw new ApplicationException(string.Format(
                    "Problema ao atualizar VBO das arestas. Foi tentado carregar {0} bytes, carregado {1}.",
                    BatchArestas.Length * sizeof(float), size));

            if (EixosLocais)
            {
                if (VAO_EixosLocais != 0)
                {
                    GL.DeleteBuffers(1, ref VAO_EixosLocais);
                    GL.DeleteVertexArray(VAO_EixosLocais);
                }
                VAO_EixosLocais = GL.GenVertexArray();
                GL.BindVertexArray(VAO_EixosLocais);
                GL.GenBuffers(1, out vertex_buffer_arestas_eixoslocais);
                GL.BindBuffer(BufferTarget.ArrayBuffer, vertex_buffer_arestas_eixoslocais);
                GL.BufferData(BufferTarget.ArrayBuffer, (IntPtr)(BatchEixosLocais.Length * sizeof(float)), BatchEixosLocais, BufferUsageHint.StaticDraw);
                qtd_coords_arestas_eixoslocais = BatchEixosLocais.Length / 6;
                //coord                cor                          
                GL.VertexAttribPointer(0, 3, VertexAttribPointerType.Float, false, 6 * sizeof(float), IntPtr.Zero);
                GL.VertexAttribPointer(1, 3, VertexAttribPointerType.Float, false, 6 * sizeof(float), 3 * sizeof(float));
                GL.EnableVertexAttribArray(0);
                GL.EnableVertexAttribArray(1);
                GL.GetBufferParameter(BufferTarget.ArrayBuffer, BufferParameterName.BufferSize, out size);

                if (size != BatchEixosLocais.Length * sizeof(float))
                    throw new ApplicationException(string.Format(
                        "Problema ao atualizar VBO dos eixos locais. Foi tentado carregar {0} bytes, carregado {1}.",
                        BatchEixosLocais.Length * sizeof(float), size));
            }
            
        }

        int texture;
        void CarregaTexturas()
        {
            Bitmap bitmap = new Bitmap(System.IO.Path.GetDirectoryName(System.Windows.Forms.Application.ExecutablePath) + "\\wood.png");

            GL.GenTextures(1, out texture);
            GL.BindTexture(TextureTarget.Texture2D, texture); // all upcoming GL_TEXTURE_2D operations now have effect on this texture object
                                                              // set the texture wrapping parameters
            GL.TexParameter(TextureTarget.Texture2D, TextureParameterName.TextureWrapS, (int)TextureWrapMode.Repeat);   // set texture wrapping to GL_REPEAT (default wrapping method)
            GL.TexParameter(TextureTarget.Texture2D, TextureParameterName.TextureWrapT, (int)TextureWrapMode.Repeat);
            // set texture filtering parameters
            GL.TexParameter(TextureTarget.Texture2D, TextureParameterName.TextureMinFilter, (int)TextureMinFilter.Linear);
            GL.TexParameter(TextureTarget.Texture2D, TextureParameterName.TextureMagFilter, (int)TextureMinFilter.Linear);

            BitmapData data = bitmap.LockBits(new System.Drawing.Rectangle(0, 0, bitmap.Width, bitmap.Height),
                                             ImageLockMode.ReadOnly, System.Drawing.Imaging.PixelFormat.Format32bppArgb);

            GL.TexImage2D(TextureTarget.Texture2D, 0, PixelInternalFormat.Rgba, data.Width, data.Height, 0,
                          OpenTK.Graphics.OpenGL.PixelFormat.Bgra, PixelType.UnsignedByte, data.Scan0);

            GL.GenerateMipmap(GenerateMipmapTarget.Texture2D);
            bitmap.UnlockBits(data);
        }

        int vertex_buffer_triangulos_principal, 
            vertex_buffer_triangulos_diagramas, 
            vertex_buffer_triangulos_deformacao, 
            vertex_buffer_arestas, 
            vertex_buffer_arestas_eixoslocais;

        uint index_buffer_object;
        public int vertex_shader_object_triangulos, fragment_shader_object_triangulos, shader_triangulos_program;
      
        public int 
            vertex_shader_object_arestas,
            vertex_shader_object_com_transparencia, 

            fragment_shader_object_arestas,
            fragment_shader_object_com_transparencia,

            shader_arestas_program, 
            shader_com_transparencia_program;

        int modeViewlLocation = 0, proj_location, modelLocation, LightPos_location, mat_translacao_location;
        Matrix4d mat_translacao;
        float transparencia = 1;
        int localizacao_transparencia;
        
      //  Matrix4 Projecao_float = new Matrix4();
   //     Matrix4 viewMatrix_float = new Matrix4(), viewMatrix_temp_float = new Matrix4();
        public Camera3D camera;

        public void DesenhaObjetos()
        {
            try
            {
                //   this.Text = m_undoBuffer.m_undoBuffer[(ComandoAdicionar)m_undoBuffer.m_undoBuffer.Count-1]._
                GL.Clear(ClearBufferMask.ColorBufferBit | ClearBufferMask.DepthBufferBit);
                GL.Viewport(0, 0, glControl.Width, glControl.Height);

                #region Camera
                if (CameraOrto)
                {
                    camera.Atualizar();

                  //  Projecao_float = RMath.ToMatrix4(Projecao);

                    GL.MatrixMode(MatrixMode.Projection);
                //    GL.LoadMatrix(ref Projecao_float);

                    GL.LoadMatrix(ref camera.Projecao_f);

                    /*só tenho viewmatrix, pois só trabalho na posição da "camera (x,y,z)"
                    * modelmatrix nao é necessário, pois não altero a posição dos objetos na cena*/
                    GL.MatrixMode(MatrixMode.Modelview);
                    /*    viewMatrix = Matrix4d.CreateTranslation(-pivoX, -pivoY, -pivoZ) *
                                     (Matrix4d.CreateRotationZ(-z_ang_rad) * Matrix4d.CreateRotationX(-x_ang_rad))* 
                                      Matrix4d.CreateTranslation(-x_trans, -y_trans, 0);

                        viewMatrix_temp = Matrix4d.CreateTranslation(-pivoX, -pivoY, -pivoZ) *
                                 (Matrix4d.CreateRotationZ(-z_ang_rad) * Matrix4d.CreateRotationX(-x_ang_rad)) *
                                   Matrix4d.CreateTranslation(-x_trans, -y_trans, -200);

                        viewMatrix_float = RMath.ToMatrix4(viewMatrix);
                        viewMatrix_temp_float = RMath.ToMatrix4(viewMatrix_temp);

                        GL.LoadMatrix(ref viewMatrix_float);*/
                    GL.LoadMatrix(ref camera.viewMatrix_f);
                }
                else
                {
                    //Projecao_float = RMath.ToMatrix4(Projecao);
                    camera.Atualizar();
                    GL.MatrixMode(MatrixMode.Projection);
                    GL.LoadMatrix(ref camera.Projecao_f);
                    GL.MatrixMode(MatrixMode.Modelview);
                    GL.LoadMatrix(ref camera.viewMatrix_f);
                    /* viewMatrix = Matrix4d.CreateTranslation(-pivoX, -pivoY, -pivoZ) *
                                      ((Matrix4d.CreateRotationZ(-z_ang_rad) * Matrix4d.CreateRotationX(x_ang_rad)) *
                                      Matrix4d.CreateTranslation(x_trans, y_trans, -z_trans));

                     viewMatrix_temp = Matrix4d.CreateTranslation(-pivoX, -pivoY, -pivoZ) *
                                    ((Matrix4d.CreateRotationZ(-z_ang_rad) * Matrix4d.CreateRotationX(x_ang_rad)) *
                                    Matrix4d.CreateTranslation(x_trans, y_trans, -z_trans));

                     viewMatrix_float = RMath.ToMatrix4(viewMatrix);
                     viewMatrix_temp_float = RMath.ToMatrix4(viewMatrix_temp);
                     GL.LoadMatrix(ref viewMatrix_float);*/
                }

                #endregion
            //    GL.CallLists(num_lists, ListNameType.Int, lists);

                GL.UseProgram(shader_triangulos_program);
                proj_location           = GL.GetUniformLocation(shader_triangulos_program, "projection");
                modeViewlLocation       = GL.GetUniformLocation(shader_triangulos_program, "modelview");
              //  GL.BindTexture(TextureTarget.Texture2D, texture);
                GL.UniformMatrix4(modeViewlLocation, false, ref camera.viewMatrix_temp_f);
                GL.UniformMatrix4(proj_location, false, ref camera.Projecao_f);

                GL.BindVertexArray(VAO_triangulos_principal);
                GL.DrawArrays(PrimitiveType.Triangles, 0, qtd_coords_triangulos_principal);

                GL.BindVertexArray(VAO_triangulos_deformacoes);
                GL.DrawArrays(PrimitiveType.Triangles, 0, qtd_coords_triangulos_deformacao);
                
                GL.UseProgram(shader_arestas_program);
                proj_location     = GL.GetUniformLocation(shader_arestas_program, "projection");
                modeViewlLocation = GL.GetUniformLocation(shader_arestas_program, "modelview");
               // localizacao_transparencia = GL.GetUniformLocation(shader_arestas_program, "aAlpha");

                GL.UniformMatrix4(modeViewlLocation, false, ref camera.viewMatrix_temp_f);
                GL.UniformMatrix4(proj_location, false, ref camera.Projecao_f);
            //    GL.Uniform1(localizacao_transparencia, transparencia);

                GL.BindVertexArray(VAO_arestas);
                GL.DrawArrays(PrimitiveType.Lines, 0, qtd_coords_arestas);
                if (EixosLocais)
                {
                    GL.LineWidth(2);
                    GL.BindVertexArray(VAO_EixosLocais);
                    GL.DrawArrays(PrimitiveType.Lines, 0, qtd_coords_arestas_eixoslocais);
                    GL.LineWidth(1);
                }

                if (Diagrama_gradiente && (mx || my || mz || fx || fy || fz))
                {
                    GL.UseProgram(shader_com_transparencia_program);
                    proj_location = GL.GetUniformLocation(shader_com_transparencia_program, "projection");
                    modeViewlLocation = GL.GetUniformLocation(shader_com_transparencia_program, "modelview");

                    GL.UniformMatrix4(modeViewlLocation, false, ref camera.viewMatrix_temp_f);
                    GL.UniformMatrix4(proj_location, false, ref camera.Projecao_f);
                    GL.BindVertexArray(VAO_triangulos_diagramas);
                    GL.DrawArrays(PrimitiveType.Triangles, 0, qtd_coords_triangulos_diagramas);
                }

                GL.UseProgram(0);

               /* if (MostrarNos)
                  GL.CallLists(num_nos, ListNameType.Int, lists_nos);*/

                if (Datum)
                  DesenhaEixosCentrais();

                #region MouseMove dos objetos
                if (ObjetoNovo != null)
                {
                    RepintarObjeto(ObjetoNovo);
                }
                else
                if (IdFerramentaEdicao != string.Empty)
                {
                    if (IdFerramentaEdicao == Const.ID_COPIAR_ELEMENTOS && (FerramentaEdicao as TCopiarElementos).Comando == Const.COPIAR_COMANDO_3)
                    {
                        foreach (TObjetoDesenho obj in (FerramentaEdicao as TCopiarElementos).Objetos)
                            RepintarObjeto(obj);
                        (FerramentaEdicao as TCopiarElementos).DesenhaSeta();
                    }
                    else
                    if (IdFerramentaEdicao == Const.ID_MOVER_ELEMENTOS && (FerramentaEdicao as TMoverElementos).Comando == Const.MOVER_COMANDO_3)
                    {
                        foreach (TObjetoDesenho obj in (FerramentaEdicao as TMoverElementos).Objetos)
                            RepintarObjeto(obj);

                        (FerramentaEdicao as TMoverElementos).DesenhaSeta();
                    }
                    else
                    if (IdFerramentaEdicao == Const.ID_MOVER_EXTREMO_ELEMENTOS && (FerramentaEdicao as TMoverExtremoElemento).Comando == Const.MOVER_EXTREMO_COMANDO_4)
                    {
                        foreach (TObjetoDesenho obj in (FerramentaEdicao as TMoverExtremoElemento).Objetos)
                            RepintarObjeto(obj);

                        (FerramentaEdicao as TMoverExtremoElemento).DesenhaSeta();
                    }
                    else
                    if (IdFerramentaEdicao == Const.ID_ROTACIONAR_ELEMENTOS && (FerramentaEdicao as TRotacionarElementos).Comando == Const.ROTACIONAR_COMANDO_3)
                    {
                        (FerramentaEdicao as TRotacionarElementos).DesenhaSeta();
                    }
                    else
                    if (IdFerramentaEdicao == Const.ID_ESPELHAR_ELEMENTOS && ((FerramentaEdicao as TEspelharElementos).Comando == Const.ESPELHAR_COMANDO_3
                        || (FerramentaEdicao as TEspelharElementos).Comando == Const.ESPELHAR_COMANDO_4))
                    {
                        (FerramentaEdicao as TEspelharElementos).DesenhaSeta();
                    }
                }

                #endregion
                //      GL.Disable(EnableCap.Lighting);

                //   CapturaGrip();

                /*Plano de trabalho*/
                // if (MostraPlano)
                // {
                //    PlanoTrabalho.desenha();

                //}

                raioPlanoTrabalho.GerarRaio3D(ref mouseX, ref mouseY, ref zNear, ref zFar, ref ViewPortPrincipal, ref Mvm_Principal, ref pm_Principal);
                raioPlanoTrabalho.CalculaIntersecao_Raio_x_Plano(ref PlanoTrabalho, ref Coord_PlanoTrabalho);

                GL.GetDouble(GetPName.ModelviewMatrix, Mvm_Principal);
                GL.GetDouble(GetPName.ProjectionMatrix, pm_Principal);
                GL.GetInteger(GetPName.Viewport, ViewPortPrincipal);

                UnProject(ref mouseX, ref mouseY, ref zNear, ref Mvm_Principal, ref pm_Principal, ref ViewPortPrincipal, ref posX, ref posY, ref posZ);

                GL.PopMatrix();

             /*   if (PlanoPanning != null)
                { 
                    PlanoPanning.DesenhaNormal();
                   PlanoPanning.desenha();
                    GL.LoadMatrix(ref viewMatrix_Panning);
                }*/
                
                //    GL.PopMatrix();


                /*plano selecao*/
                GL.Viewport(0, 0, glControl.Width, glControl.Height);
                GL.MatrixMode(MatrixMode.Projection);
                GL.LoadIdentity();
                if (CameraOrto)
                    GL.Ortho(leftOrtoZoom, rightOrtoZoom, bottonOrtoZoom, topOrtoZoom, z1_selecao, zFar);
                else
                    GL.Ortho(GLleft, GLright, GLbotton, GLtop, z1_selecao, zFar);

                GL.MatrixMode(MatrixMode.Modelview);
                GL.PushMatrix();

             //   if (CameraOrto)
            //        GL.Translate(-x_trans, -y_trans, -z_trans);

                raioSelecao.GerarRaio3D(ref mouseX, ref mouseY, ref z1_selecao, ref zFar, ref ViewPortSelecao, ref Mvm_Selecao, ref pm_Selecao);
                raioSelecao.CalculaIntersecao_Raio_x_Plano(ref planoSelecao, ref Coord_PlanoSelecao);

                if (CaixaSelecao)
                    DesenhaRetanguloSelecao();

                GL.GetDouble(GetPName.ModelviewMatrix, Mvm_Selecao);
                GL.GetDouble(GetPName.ProjectionMatrix, pm_Selecao);
                GL.GetInteger(GetPName.Viewport, ViewPortSelecao);

                GL.PopMatrix();

                /*plano Snap*/
                GL.Viewport(0, 0, glControl.Width, glControl.Height);

                GL.MatrixMode(MatrixMode.Projection);
                GL.LoadIdentity();
                GL.Ortho(0, glControl.Width, glControl.Height, 0, z1_selecao, zFar);
                GL.MatrixMode(MatrixMode.Modelview);

                if (FundoGradiente)
                {
                    GL.PushMatrix();
                    DesenhaFundo();
                    GL.PopMatrix();
                }

                //  if (!FazendoZoom)
                {
                    if (tipoComando != eTipoComando.selecionar)
                        ObtemSnap();
                   
                    GL.PushMatrix();

                    /* GL.PointSize(5);
                     GL.Begin(PrimitiveType.Points);
                     GL.Color3(Color.Red);
                     GL.Vertex2(400, 200);
                     GL.End();*/
 
                    if (!FazendoZoom)
                    {
                        if (snapPerpendicular)
                        {
                            GL.LineWidth(2);
                            GL.Color3(System.Drawing.Color.Red);
                            GL.Begin(PrimitiveType.Lines);
                            GL.Vertex2(mousepoint.px_x - 10, mousepoint.px_y -5);
                            GL.Vertex2(mousepoint.px_x + 10, mousepoint.px_y -5);
                            GL.End();
                            
                            GL.Begin(PrimitiveType.Lines);
                            GL.Vertex2(mousepoint.px_x - 10, mousepoint.px_y -5 );
                            GL.Vertex2(mousepoint.px_x - 10, mousepoint.px_y -20);
                            GL.End();

                            GL.Color3(System.Drawing.Color.Red);
                            GL.Begin(PrimitiveType.Lines);
                            GL.Vertex2(mousepoint.px_x -2, mousepoint.px_y - 5);
                            GL.Vertex2(mousepoint.px_x-2, mousepoint.px_y - 12);
                            GL.End();
                            
                            GL.Color3(System.Drawing.Color.Red);
                            GL.Begin(PrimitiveType.Lines);
                            GL.Vertex2(mousepoint.px_x - 2, mousepoint.px_y - 12);
                            GL.Vertex2(mousepoint.px_x - 10, mousepoint.px_y - 12);
                            GL.End();

                            GL.Enable(EnableCap.LineStipple);
                            GL.LineStipple(10, 0xAAAA);
                            GL.LineWidth(3);
                            GL.Color3(System.Drawing.Color.Red);
                            GL.Begin(PrimitiveType.Lines);

                            interx = LinhaSnapNearest.Barra.pIni.x;
                            intery = LinhaSnapNearest.Barra.pIni.y;
                            interz = LinhaSnapNearest.Barra.pIni.z;
                            pixel1(ref interx, ref intery, ref interz, ref z_pixel);

                            GL.Vertex2(px_x1[0], px_y1[0]);

                            interx = LinhaSnapNearest.Barra.pFin.x;
                            intery = LinhaSnapNearest.Barra.pFin.y;
                            interz = LinhaSnapNearest.Barra.pFin.z;
                            pixel1(ref interx, ref intery, ref interz, ref z_pixel);
                            GL.Vertex2(px_x1[0], px_y1[0]);
                            GL.End();
                            GL.Disable(EnableCap.LineStipple);

                            GL.LineWidth(1);
                            snapPerpendicular = false;
                            snapNearest = true;
                            //    foreach (TBarraGenerica b in Barras)
                            //        b.pinta = false;
                        }
                        else
                        if (snapIntersec)
                        {
                            GL.LineWidth(2);
                            GL.Color3(System.Drawing.Color.DarkGreen);
                            GL.Begin(PrimitiveType.Lines);
                            GL.Vertex2(mousepoint.px_x - 10, mousepoint.px_y - 10);
                            GL.Vertex2(mousepoint.px_x + 10, mousepoint.px_y + 10);
                            GL.End();

                            GL.Begin(PrimitiveType.Lines);
                            GL.Vertex2(mousepoint.px_x - 10, mousepoint.px_y + 10);
                            GL.Vertex2(mousepoint.px_x + 10, mousepoint.px_y - 10);
                            GL.End();
                            GL.LineWidth(1);
                            snapIntersec = false;
                            snapNearest = true;
                            //    foreach (TBarraGenerica b in Barras)
                            //        b.pinta = false;
                        }
                        else
                        if (snapNearest)
                            mousepoint.z = mousepoint.z;
                        else
                        if (snapMiddle)
                        {
                            //  GL.LineWidth(2);
                            GL.Color3(System.Drawing.Color.MediumBlue);
                            GL.Begin(PrimitiveType.LineStrip);
                            GL.Vertex2(mousepoint.px_x - 7, mousepoint.px_y + 7);
                            GL.Vertex2(mousepoint.px_x + 7, mousepoint.px_y + 7);
                            GL.Vertex2(mousepoint.px_x, mousepoint.px_y - 7);
                            GL.Vertex2(mousepoint.px_x - 7, mousepoint.px_y + 7);
                            GL.End();
                            // GL.LineWidth(1);
                            snapMiddle = false;
                         //   timerSnap.Enabled = true;

                            if (projecaoNo)
                            {
                                GL.Color3(System.Drawing.Color.Red);
                                GL.Begin(PrimitiveType.Lines);
                                GL.Vertex2(mousepoint.px_x - 8, mousepoint.px_y - 8);
                                GL.Vertex2(mousepoint.px_x + 8, mousepoint.px_y + 8);
                                GL.End();

                                GL.Begin(PrimitiveType.Lines);
                                GL.Vertex2(mousepoint.px_x - 8, mousepoint.px_y + 8);
                                GL.Vertex2(mousepoint.px_x + 8, mousepoint.px_y - 8);
                                GL.End();
                            }
                        }
                        else
                        if (snapEnd)
                        {
                            GL.LineWidth(2);
                            GL.Color3(System.Drawing.Color.MediumBlue);
                            GL.Begin(PrimitiveType.LineLoop);
                            GL.Vertex2(mousepoint.px_x - 6, mousepoint.px_y - 6);
                            GL.Vertex2(mousepoint.px_x + 6, mousepoint.px_y - 6);
                            GL.Vertex2(mousepoint.px_x + 6, mousepoint.px_y + 6);
                            GL.Vertex2(mousepoint.px_x - 6, mousepoint.px_y + 6);
                            GL.End();
                            GL.LineWidth(1);
                            //    timerSnap.Enabled = true;

                            snapEnd = false;

                            if (projecaoNo)
                            {
                                GL.Color3(System.Drawing.Color.Red);
                                GL.Begin(PrimitiveType.Lines);
                                GL.Vertex2(mousepoint.px_x - 11, mousepoint.px_y - 11);
                                GL.Vertex2(mousepoint.px_x + 11, mousepoint.px_y + 11);
                                GL.End();

                                GL.Begin(PrimitiveType.Lines);
                                GL.Vertex2(mousepoint.px_x - 11, mousepoint.px_y + 11);
                                GL.Vertex2(mousepoint.px_x + 11, mousepoint.px_y - 11);
                                GL.End();
                            }
                            //           timerSnap.Enabled = false;
                        }
                        else
                        {
                            if (!NovaCoordX)
                                mousepoint.x = Coord_PlanoTrabalho.x;
                            if (!NovaCoordY)
                                mousepoint.y = Coord_PlanoTrabalho.y;
                            if (!NovaCoordZ)
                                mousepoint.z = Coord_PlanoTrabalho.z;
                        //    timerSnap.Enabled = false;
                            projecaoNo = false;
                            //  if (NewObject != null)
                            //   Orto(false);
                        }
                    }
                    //cota = conv.comp(cota,gerenciador.ConfiguracoesPGi.CfgProjeto.unidadesProjeto.geo_un_comprimento.ToString(),und.m);

                    if (!NovaCoordX)
                        edX.Text = geometria_un_visualizacao(mousepoint.x).ToString("n"+gerenciador.ConfiguracoesPGi.CfgProjeto.unidadesProjeto.geo_casas.ToString()).Replace(",", ".");
                    if (!NovaCoordY)
                        edY.Text = geometria_un_visualizacao(mousepoint.y*-1).ToString("n" + gerenciador.ConfiguracoesPGi.CfgProjeto.unidadesProjeto.geo_casas.ToString()).Replace(",", ".");
                    if (!NovaCoordZ)
                        edZ.Text = geometria_un_visualizacao(mousepoint.z*-1).ToString("n" + gerenciador.ConfiguracoesPGi.CfgProjeto.unidadesProjeto.geo_casas.ToString()).Replace(",", ".");

                    if (edX.Visible)
                        RetanguloCoordenadas();

                    //   UpdateOGLMatrix(ref Mvm_Snap, ref pm_Snap, ref ViewPortSnap);
                    GL.GetDouble(GetPName.ModelviewMatrix, Mvm_Snap);
                    GL.GetDouble(GetPName.ProjectionMatrix, pm_Snap);
                    GL.GetInteger(GetPName.Viewport, ViewPortSnap);

                    GL.PopMatrix();

                    distCota1 = 0;
                    if (!FazendoZoom)
                    {
                        if (snapNearest)
                        {
                            DesenhaInsercaoPorCotas();

                            GL.PushMatrix();

                            GL.Translate(pontoMedioCota.x, pontoMedioCota.y, 0);
                            GL.Rotate(angCota / Const.PIDiv180, 0, 0, 1);
                            textoCota.Print(conv.comp(distCota1, und.m, gerenciador.ConfiguracoesPGi.CfgProjeto.unidadesProjeto.geo_un_comprimento).ToString("n" + gerenciador.ConfiguracoesPGi.CfgProjeto.unidadesProjeto.geo_casas.ToString()), fonte, CorCota);
                            GL.PopMatrix();
                            snapNearest = false;
                        }

                        if (ObjetoNovo != null)
                        {
                            if (ObjetoNovo.Tipo == Const.ID_BARRAGENERICA /*|| ObjetoNovo.Tipo == Const.ID_TRECHOVIGA*/)
                            {
                                GL.PushMatrix();
                                pMediocota = (ObjetoNovo.pIni + ObjetoNovo.pFin) / 2;

                                pixel1(ref pMediocota.x, ref pMediocota.y, ref pMediocota.z);
                                pixel2(ref ObjetoNovo.pIni.x, ref ObjetoNovo.pIni.y, ref ObjetoNovo.pIni.z);

                                coca = ((px_y1[0] - px_y2[0]) / (px_x1[0] - px_x2[0]));
                                angObjeto = Math.Atan(coca);

                                GL.Translate(px_x1[0], px_y1[0], 0);
                                GL.Rotate(angObjeto / Const.PIDiv180, 0, 0, 1);
                                textoCota.Print(conv.comp(ObjetoNovo.pFin.DistanceTo(ObjetoNovo.pIni), und.m, gerenciador.ConfiguracoesPGi.CfgProjeto.unidadesProjeto.geo_un_comprimento) .ToString("n" + gerenciador.ConfiguracoesPGi.CfgProjeto.unidadesProjeto.geo_casas.ToString()), fonte, System.Drawing.Color.Red);
                                GL.PopMatrix();
                            }
                        }
                    }
                }

                if (MostrarNumeroElementos || MostrarDescricaoElementos)
                {
                    foreach (TBarraGenerica b in Estrutura.barras)
                    {
                        if (!b.Visivel) continue;

                        pixel1(ref b.pMedioBarra.x, ref b.pMedioBarra.y, ref b.pMedioBarra.z, ref Z_Clip);

                        if (((px_x1[0] < 0)) ||
                                  ((px_x1[0] > w)) ||
                                  ((px_y1[0] < 0)) ||
                                  ((px_y1[0] > h)))
                            continue;

                        string textinho = b.Dados.secao.descricao;

                        if (MostrarNumeroElementos)
                          textinho = b.IDBarra.ToString();

                        if (Z_Clip < 1 && Z_Clip > 0)
                        {
                            pixel2(ref b.pFin.x, ref b.pFin.y, ref b.pFin.z);

                            GL.PushMatrix();
                            coca = ((px_y1[0] - px_y2[0]) / (px_x1[0] - px_x2[0]));
                            angObjeto = Math.Atan(coca) / Const.PIDiv180;

                            GL.Translate(px_x1[0], px_y1[0], 0);
                            GL.Rotate(angObjeto, 0, 0, 1);
                            textoCarga.Print(textinho, fonteTam8,b.Selecionado ? System.Drawing.Color.Red: corDescricaoElementos);

                            GL.PopMatrix();
                            GL.End();
                        }
                    }
                }

                if (MostraTextoDiagramas)
                {                 
                    foreach (vec3 c in coordTextoEsforco)
                    {
                        pixel1(ref c.x, ref c.y, ref c.z, ref Z_Clip);

                        if (((px_x1[0] < 0)) ||
                                  ((px_x1[0] > w)) ||
                                  ((px_y1[0] < 0)) ||
                                  ((px_y1[0] > h)))
                            continue;

                        if (Z_Clip < 1 && Z_Clip > 0)
                        {
                            pixel2(ref c.x2, ref c.y2, ref c.z2);

                            if (gerenciador.ConfiguracoesPGi.DiagramaOpcoesVisualizacao.alinhamento == 0)
                            {
                                coca = ((px_y1[0] - px_y2[0]) / (px_x1[0] - px_x2[0]));
                                angObjeto = Math.Atan(coca) / Const.PIDiv180;
                            }
                            else
                            if (gerenciador.ConfiguracoesPGi.DiagramaOpcoesVisualizacao.alinhamento == 1)
                            {
                                angObjeto = 0;
                            }
                            else
                            if (gerenciador.ConfiguracoesPGi.DiagramaOpcoesVisualizacao.alinhamento == 2)
                            {
                                angObjeto = 90;
                            }

                            GL.PushMatrix();
                            GL.Translate(px_x1[0]+1, px_y1[0]+1, 0);
                            GL.Rotate(angObjeto, 0, 0, 1);
                            if (textoMomento)
                              textoCarga.Print(((c.vv * conversaoForcaResultado) * conversaoComprimentoResultado).ToString(casas_decimais_resultado), fonteCarga, corValorDiagrama);
                            else
                            if (textoForca)
                              textoCarga.Print(((c.vv * conversaoForcaResultado)).ToString(casas_decimais_resultado), fonteCarga, corValorDiagrama);

                            GL.PopMatrix();
                            GL.End();
                        }
                    }  
                }

                if (MostraTextoDeformacoes)
                {
                    for (i = 0; i < nos_deformacao.Count(); i++)
                    {
                        pixel1(ref nos_deformacao[i].coordx_tela, ref nos_deformacao[i].coordy_tela, ref nos_deformacao[i].coordz_tela, ref Z_Clip);

                        if (((px_x1[0] < 0)) ||
                                  ((px_x1[0] > w)) ||
                                  ((px_y1[0] < 0)) ||
                                  ((px_y1[0] > h)))
                            continue;

                        if (Z_Clip < 1 && Z_Clip > 0)
                        {
                            GL.PushMatrix();
                            GL.Translate(px_x1[0] + 4, px_y1[0] - 18, 0);
  /*Dx global
  Dy global
  Dz global
  Rx global
  Ry global
  Rz global
  D Total*/
                            if (tipoCargaResultado == 0)
                            {
                                if (gerenciador.ConfiguracoesPGi.PorticoOpcoesVisualizacao.valores == 7)
                                   textoCarga.Print("d: " + ((nos_deformacao[i].casos_x_deslocamentos[id_caso].U_Total * conversaoComprimento_Def).ToString(casas_decimais_resultado)), fonteCarga, gerenciador.ConfiguracoesPGi.PorticoOpcoesVisualizacao.CorValor);
                                else
                                if (gerenciador.ConfiguracoesPGi.PorticoOpcoesVisualizacao.valores > 3 && gerenciador.ConfiguracoesPGi.PorticoOpcoesVisualizacao.valores < 7)
                                    textoCarga.Print(dx_dy_dz[gerenciador.ConfiguracoesPGi.PorticoOpcoesVisualizacao.valores] +
                                                     (Math.Abs(nos_deformacao[i].casos_x_deslocamentos[id_caso].DeslocamentoGlobal[gerenciador.ConfiguracoesPGi.PorticoOpcoesVisualizacao.valores] * 57.2957)).ToString(casas_decimais_resultado)+ "°",
                                                     fonteCarga,
                                                     gerenciador.ConfiguracoesPGi.PorticoOpcoesVisualizacao.CorValor);
                                else
                                    textoCarga.Print(dx_dy_dz[gerenciador.ConfiguracoesPGi.PorticoOpcoesVisualizacao.valores] +
                                                    (nos_deformacao[i].casos_x_deslocamentos[id_caso].DeslocamentoGlobal[gerenciador.ConfiguracoesPGi.PorticoOpcoesVisualizacao.valores] *
                                                    conversaoComprimento_Def).ToString(casas_decimais_resultado),
                                                    fonteCarga,
                                                    gerenciador.ConfiguracoesPGi.PorticoOpcoesVisualizacao.CorValor);
                            }
                            else
                            {
                                if (gerenciador.ConfiguracoesPGi.PorticoOpcoesVisualizacao.valores == 7)
                                    textoCarga.Print("d: " + ((nos_deformacao[i].combinacoes_x_deslocamentos[id_combinacao].U_Total * conversaoComprimento_Def).ToString(casas_decimais_resultado)), fonteCarga, gerenciador.ConfiguracoesPGi.PorticoOpcoesVisualizacao.CorValor);
                                else
                                if (gerenciador.ConfiguracoesPGi.PorticoOpcoesVisualizacao.valores > 3 && gerenciador.ConfiguracoesPGi.PorticoOpcoesVisualizacao.valores < 7)
                                    textoCarga.Print(dx_dy_dz[gerenciador.ConfiguracoesPGi.PorticoOpcoesVisualizacao.valores] +
                                                     (Math.Abs(nos_deformacao[i].combinacoes_x_deslocamentos[id_combinacao].DeslocamentoGlobal[gerenciador.ConfiguracoesPGi.PorticoOpcoesVisualizacao.valores] * 57.2957)).ToString(casas_decimais_resultado) + "°",
                                                     fonteCarga,
                                                     gerenciador.ConfiguracoesPGi.PorticoOpcoesVisualizacao.CorValor);
                                else
                                    textoCarga.Print(dx_dy_dz[gerenciador.ConfiguracoesPGi.PorticoOpcoesVisualizacao.valores] +
                                                     (nos_deformacao[i].combinacoes_x_deslocamentos[id_combinacao].DeslocamentoGlobal[gerenciador.ConfiguracoesPGi.PorticoOpcoesVisualizacao.valores] *
                                                     conversaoComprimento_Def).ToString(casas_decimais_resultado),
                                                     fonteCarga,
                                                     gerenciador.ConfiguracoesPGi.PorticoOpcoesVisualizacao.CorValor);
                            }
                          
                            GL.PopMatrix();
                        }
                    }
                }

                foreach (TCargaPontual o in CargasNosAtuais)
                {
                    if (!o.Visivel) continue;

                    pMediocarga = o.p1_vetor;

                    pixel1(ref pMediocarga.x, ref pMediocarga.y, ref pMediocarga.z, ref Z_Clip);

                    if (((px_x1[0] < 0)) ||
                              ((px_x1[0] > w)) ||
                              ((px_y1[0] < 0)) ||
                              ((px_y1[0] > h)))
                        continue;

                    if (Z_Clip < 1 && Z_Clip > 0)
                    {
                        GL.PushMatrix();
                        GL.Translate(px_x1[0] + 4, px_y1[0] - 18, 0);
                        textoCarga.Print((Math.Abs(o.Dados.valor * conversaoForcaResultado)).ToString(casas_decimais_resultado), fonteCarga, o.Selecionado ? System.Drawing.Color.Cyan : o.Dados.cor);
                        GL.PopMatrix();
                        GL.End();
                    }
                }

                foreach (TCargaLinear o in CargasBarrasAtuais)
                {
                    if (!o.Visivel) continue;

                    bg = Estrutura.barras.Find(b => b.IDBarra == o.idBarra);
                   
                    if (bg == null) continue;

                    if (bg.Dados.Tipo == 4) continue; // barra rigida

                    if (o.Dados.distribuida)
                    {
                        pMediocarga = (o.setas[0].l_principal.p1 + o.setas[o.qtdSetas].l_principal.p1) / 2;

                        pixel1(ref pMediocarga.x, ref pMediocarga.y, ref pMediocarga.z, ref Z_Clip);

                        if (((px_x1[0] < 0)) ||
                                  ((px_x1[0] > w)) ||
                                  ((px_y1[0] < 0)) ||
                                  ((px_y1[0] > h)))
                            continue;

                        if (Z_Clip < 1 && Z_Clip > 0)
                        {
                            GL.PushMatrix();
                            pixel2(ref o.setas[o.qtdSetas].l_principal.p1.x, ref o.setas[o.qtdSetas].l_principal.p1.y, ref o.setas[o.qtdSetas].l_principal.p1.z);

                            coca = ((px_y1[0] - px_y2[0]) / (px_x1[0] - px_x2[0]));
                            angObjeto = Math.Atan(coca) / Const.PIDiv180;

                            GL.Translate(px_x1[0], px_y1[0], 0);
                            GL.Rotate(angObjeto, 0, 0, 1);
                            textoCarga.Print(((Math.Abs(o.Dados.valor) * conversaoForcaResultado) / conversaoComprimentoResultado).ToString(casas_decimais_resultado), fonteCarga, o.Selecionado ? System.Drawing.Color.Cyan : o.Dados.cor);
                            GL.PopMatrix();
                        }
                    }
                    else
                    {
                        pMediocarga = o.setas[0].l_principal.p1;

                        pixel1(ref pMediocarga.x, ref pMediocarga.y, ref pMediocarga.z, ref Z_Clip);

                        if (((px_x1[0] < 0)) ||
                                  ((px_x1[0] > w)) ||
                                  ((px_y1[0] < 0)) ||
                                  ((px_y1[0] > h)))
                            continue;

                        if (Z_Clip < 1 && Z_Clip > 0)
                        {
                     //       if (PlanoCorte)
                     //           if (PlanoTrabalho.Distancia(ref pMediocarga.x, ref pMediocarga.y, ref pMediocarga.z) < 0)
                     //               continue;

                            GL.PushMatrix();
                            GL.Translate(px_x1[0] + 4, px_y1[0] - 10, 0);
                            textoCarga.Print(((o.Dados.valor * conversaoForcaResultado)).ToString(casas_decimais_resultado), fonteCarga, o.Selecionado ? System.Drawing.Color.Cyan : o.Dados.cor);
                            GL.PopMatrix();
                            GL.End();
                        }

                    }
                }

                if (MostrarNos)
                {
                    foreach (TPonto o in Estrutura.nos)
                    {
                        o.Visivel = MostrarNos;
                        if (!o.habilitado)
                            continue;

                        if (o.Selecionado)
                        {
                            GL.Color3(System.Drawing.Color.Red);
                            GL.PointSize(tamNo+2);
                        }
                        else
                        {
                            GL.PointSize(tamNo);
                            GL.Color3(gerenciador.ConfiguracoesPGi.F3DOpcoesVisualizacao.CorNo);
                        }
                        pNo.x = o.x; pNo.y = o.y; pNo.z = o.z;

                        pixel1(ref pNo.x, ref pNo.y, ref pNo.z, ref Z_Clip);

                        if (((px_x1[0] < 0)) ||
                             ((px_x1[0] > w)) ||
                             ((px_y1[0] < 0)) ||
                             ((px_y1[0] > h)))
                            continue;

                        if (Z_Clip < 1 && Z_Clip > 0)
                        {
                       //     GL.PushMatrix();
                         //   GL.Translate(px_x1[0], px_y1[0], 0);
                            GL.Begin(PrimitiveType.Points);
                            GL.Vertex2(px_x1[0], px_y1[0]);
                            GL.End();
                         //   GL.PopMatrix();
                          //  GL.End();
                        }
                    }
                }

                cubo.Desenha((float)camera.x_rot_angle, (float)camera.y_rot_angle, ref mouseX, ref mouseY);

                /*Eixos Principais*/
               /* GL.PushMatrix();
                GL.Viewport(-10, 20, 80, 80);
                GL.MatrixMode(MatrixMode.Projection);
                GL.LoadMatrix(ref proj_eixos);

                GL.Translate(0, 0, -350);

                GL.Rotate(x_rot_angle, 1, 0.0, 0.0);
                GL.Rotate(-y_rot_angle, 0.0, 0, 1);

                ogl.DesenhaEixosPrincipais(ref CameraOrto);
          
                GL.PopMatrix();*/


                /*--------------------------*/
            }
            catch (Exception e)
            {
                MessageBox.Show("erro ao desenhar objetos: " + e.Message);
            }
        }
        public bool MostrarCargas = true;

        public List<vec3> coordTextoEsforco;
        bool textoMomento, textoForca;
        int b_escolhida = -1;
        public TNoPortico[] nos_deformacao;
        public List<TBarraPortico> barras_esforco;
        double angulocarga_Z;
        public void CriarTextosEsforco()
        {

            List<TBarraPortico> bp;
            double max = 0;
            List<TBarraPortico> barras_portico = new List<TBarraPortico>();

            barras_portico = barras_esforco;

            b_escolhida = 0;
            int esf_i = 0, esf_f = 0;
            vec3 coordEsforco_i = new vec3(0);
            vec3 coordEsforco_f = new vec3(0);

            if (mx)
            {
                esf_i = 4; esf_f = 10;
                textoMomento = true;
                textoForca = true;
            }
            else
            if (my)
            {
                esf_i = 6; esf_f = 12;
                textoMomento = true;
                textoForca = true;
            }
            else
            if (mz)
            {
                esf_i = 5; esf_f = 11;
                textoMomento = true;
                textoForca = true;
            }
            else
            if (fx)
            {
                esf_i = 1; esf_f = 7;
                textoMomento = true;
                textoForca = true;
            }
            else
            if (fy)
            {
                esf_i = 3; esf_f = 9;
                textoMomento = true;
                textoForca = true;
            }
            else
            if (fz)
            {
                esf_i = 2; esf_f = 8;
                textoMomento = true;
                textoForca = true;
            }
            int total = 0;

            if (tipoCargaResultado == 0)
            {
                for (int n = 0; n < barras_portico.Count; n++)
                    if (barras_portico[n].barraOriginal.Visivel && barras_portico[n].casos_x_esforcos.Count > 0)
                    {
                        if (!Geom.Iguais(barras_portico[n].casos_x_esforcos[id_caso].Esforcos[esf_f], 0, 0.00))
                            total++;
                        if (!Geom.Iguais(barras_portico[n].casos_x_esforcos[id_caso].Esforcos[esf_i], 0, 0.00))
                            total++;
                    }
            }
            else
            {
                for (int n = 0; n < barras_portico.Count; n++)
                    if (barras_portico[n].barraOriginal.Visivel  && barras_portico[n].combinacoes_x_esforcos.Count>0)
                    {
                        if (!Geom.Iguais(barras_portico[n].combinacoes_x_esforcos[id_combinacao].Esforcos[esf_f], 0, 0.00))
                            total++;
                        if (!Geom.Iguais(barras_portico[n].combinacoes_x_esforcos[id_combinacao].Esforcos[esf_i], 0, 0.00))
                            total++;
                    }
            }

            int nn = -1;
            coordTextoEsforco = new List<vec3>();

            double[] Esforcos;
            vec3 z_local;

            for (int n = 0; n < barras_portico.Count; n++)
            {
                if (barras_portico[n].IDBarra == 82)
                    barras_portico[n].ID = 76;

                if (barras_portico[n].barraOriginal.Visivel)
                {
                    if (tipoCargaResultado == 0 && barras_portico[n].casos_x_esforcos.Count > 0)
                        Esforcos = barras_portico[n].casos_x_esforcos[id_caso].Esforcos;
                    else
                    if (tipoCargaResultado == 1 && barras_portico[n].combinacoes_x_esforcos.Count > 0)
                        Esforcos = barras_portico[n].combinacoes_x_esforcos[id_combinacao].Esforcos;
                    else
                        Esforcos = new double[13];
                        // if (!Geom.Iguais(barras_portico[n].Esforcos[esf_f], 0, 0.000000) || !Geom.Iguais(barras_portico[n].Esforcos[esf_i], 0, 0.000000))

                    ++nn;

                  /*  if (fx)
                    {
                        z_local = Geom.CriaVetor(barras_portico[n].barraOriginal.seta_eixo_local_Z.l_principal.p1, barras_portico[n].barraOriginal.seta_eixo_local_Z.l_principal.p2, true);
                        vec3 vetorCarga = Geom.CriaVetor(barras_portico[n].coordsForca[0], barras_portico[n].coordsForca[1], true);
                        if (Geom.Iguais(vetorCarga.y, 0))
                            vetorCarga.y = 0;
                        if (Geom.Iguais(z_local.y, 0))
                            z_local.y = 0;

                        angulocarga_Z = Geom.AnguloEntre2Vetores(vetorCarga, z_local);

                        if (Geom.Iguais(angulocarga_Z, 0))
                            coordTextoEsforco[nn].vv = Math.Abs(Esforcos[esf_i]);
                        else
                            coordTextoEsforco[nn].vv = Math.Abs(Esforcos[esf_i]) * -1;
                    }
                    else*/
                    {
                        if (my)
                        {
                            coordEsforco_i = barras_portico[n].coordsFletorY[1];
                            coordEsforco_f = barras_portico[n].coordsFletorY[2];
                        }
                        else
                        if (mz)
                        {
                            coordEsforco_i = barras_portico[n].coordsFletorZ[1];
                            coordEsforco_f = barras_portico[n].coordsFletorZ[2];
                        }
                        else
                        if (mx)
                        {
                            coordEsforco_i = barras_portico[n].coordsTorcor[1];
                            coordEsforco_f = barras_portico[n].coordsTorcor[2];
                        }
                        else
                        {
                            coordEsforco_i = barras_portico[n].coordsForca[1];
                            coordEsforco_f = barras_portico[n].coordsForca[2];
                        }
                        double val1 = 0, val2 = 0;
                        double e_i = 0, e_f = 0;

                        if (fz)
                        {
                            e_i = barras_portico[n].sinal_fz_ini;
                            e_f = barras_portico[n].sinal_fz_fin;
                        }
                        else
                        if (fy)
                        {
                            e_i = barras_portico[n].sinal_fy_ini;
                            e_f = barras_portico[n].sinal_fy_fin;
                        }
                        else
                        if (fx)
                        {
                            e_i = barras_portico[n].sinal_fx_ini;
                            e_f = barras_portico[n].sinal_fx_fin;
                        }
                        else
                        if (my)
                        {
                            e_i = barras_portico[n].sinal_my_ini;
                            e_f = barras_portico[n].sinal_my_fin;
                        }
                        else
                        if (mz)
                        {
                            e_i = barras_portico[n].sinal_mz_ini;
                            e_f = barras_portico[n].sinal_mz_fin;
                        }
                        else
                        if (mx)
                        {
                            e_i = barras_portico[n].sinal_mx_ini;
                            e_f = barras_portico[n].sinal_mx_fin;
                        }

                        if (e_i > 0)
                            val1 = Math.Abs(Esforcos[esf_i]);
                        else
                        if (e_i < 0)
                            val1 = Math.Abs(Esforcos[esf_i]) * -1;

                        if (e_f > 0)
                            val2 = Math.Abs(Esforcos[esf_f]);
                        else
                        if (e_f < 0)
                            val2 = Math.Abs(Esforcos[esf_f]) * -1;

                        if (!coordTextoEsforco.Exists(p => (Geom.Iguais(p.vv, val1) && (Geom.Iguais(p.x, coordEsforco_i.x) && Geom.Iguais(p.y, coordEsforco_i.y) && Geom.Iguais(p.z, coordEsforco_i.z)))))
                        {
                            coordTextoEsforco.Add(new vec3(coordEsforco_i.x, coordEsforco_i.y, coordEsforco_i.z));

                            coordTextoEsforco[coordTextoEsforco.Count-1].vv = val1;

                            coordTextoEsforco[coordTextoEsforco.Count - 1].x2 = coordEsforco_f.x;
                            coordTextoEsforco[coordTextoEsforco.Count - 1].y2 = coordEsforco_f.y;
                            coordTextoEsforco[coordTextoEsforco.Count - 1].z2 = coordEsforco_f.z;
                        }

                        if (!coordTextoEsforco.Exists(p => (Geom.Iguais(p.vv, val2) && (Geom.Iguais(p.x, coordEsforco_f.x) && Geom.Iguais(p.y, coordEsforco_f.y) && Geom.Iguais(p.z, coordEsforco_f.z)))))
                        {
                            coordTextoEsforco.Add(new vec3(coordEsforco_f.x, coordEsforco_f.y, coordEsforco_f.z));

                            coordTextoEsforco[coordTextoEsforco.Count - 1].vv = val2;

                            coordTextoEsforco[coordTextoEsforco.Count - 1].x2 = coordEsforco_i.x;
                            coordTextoEsforco[coordTextoEsforco.Count - 1].y2 = coordEsforco_i.y;
                            coordTextoEsforco[coordTextoEsforco.Count - 1].z2 = coordEsforco_i.z;
                        }
                    }

                    /*if (mx)
                    {
                        coordEsforco_f = barras_portico[n].coordsTorcor[2];
                        coordEsforco_i = barras_portico[n].coordsTorcor[1];
                    }
                    else
                    if (my)
                    {
                        coordEsforco_f = barras_portico[n].coordsFletorY[2];
                        coordEsforco_i = barras_portico[n].coordsFletorY[1];
                    }
                    else
                    if (mz)
                    {
                        coordEsforco_f = barras_portico[n].coordsFletorZ[2];
                        coordEsforco_i = barras_portico[n].coordsFletorZ[1];
                    }
                    else
                    if (fx)
                    {
                        coordEsforco_f = barras_portico[n].coordsForca[2];
                        coordEsforco_i = barras_portico[n].coordsForca[1];
                    }
                    else
                    if (fz)
                    {
                        coordEsforco_f = barras_portico[n].coordsForca[2];
                        coordEsforco_i = barras_portico[n].coordsForca[1];
                    }
                    else
                    if (fy)
                    {
                        coordEsforco_f = barras_portico[n].coordsForca[2];
                        coordEsforco_i = barras_portico[n].coordsForca[1];
                    }*/

                   /* if (Math.Abs(Esforcos[esf_f]) > Math.Abs(Esforcos[esf_i]))
                    {
                        if (!Array.Exists(coordTextoEsforco, p => (Geom.Iguais(p.x, coordEsforco_f.x) && Geom.Iguais(p.y, coordEsforco_f.y) && Geom.Iguais(p.z, coordEsforco_f.z))))
                        {
                            coordTextoEsforco[nn].x = (coordEsforco_f.x);
                            coordTextoEsforco[nn].y = (coordEsforco_f.y);
                            coordTextoEsforco[nn].z = (coordEsforco_f.z);
                    
                            coordTextoEsforco[nn].x2 = coordEsforco_i.x;
                            coordTextoEsforco[nn].y2 = coordEsforco_i.y;
                            coordTextoEsforco[nn].z2 = coordEsforco_i.z;
                        }
                    }
                    else
                    {
                        if (!Array.Exists(coordTextoEsforco, p => (Geom.Iguais(p.x, coordEsforco_i.x) && Geom.Iguais(p.y, coordEsforco_i.y) && Geom.Iguais(p.z, coordEsforco_i.z))))
                        {
                            coordTextoEsforco[nn].x = (coordEsforco_i.x);
                            coordTextoEsforco[nn].y = (coordEsforco_i.y);
                            coordTextoEsforco[nn].z = (coordEsforco_i.z);
                    
                            coordTextoEsforco[nn].x2 = coordEsforco_f.x;
                            coordTextoEsforco[nn].y2 = coordEsforco_f.y;
                            coordTextoEsforco[nn].z2 = coordEsforco_f.z;
                        }
                    }*/                  
                }
            }
        }

        public void MostrarCargasPeloCaso(bool atuShaders = true)
        {
            CasoCargaAtual = null;

            CargasBarrasAtuais = new List<TCargaLinear>();
            CargasNosAtuais = new List<TCargaPontual>();
            glControl.Focus();

            CargasLineares.ForEach(obj => obj.Visivel = false);
            CargasPontuais.ForEach(obj => obj.Visivel = false);

            Estrutura.cargaLinear.ForEach(obj => obj.Visivel = false);
            Estrutura.cargaPontual.ForEach(obj => obj.Visivel = false);
            
            if (MostrarCargas)
            {
                if (gerenciador.cbCasoCarga.Items.Count > 0)
                {
                    if (gerenciador.cbCasoCarga.SelectedIndex > 0)
                    {
                        if (gerenciador.cbCasoCarga.SelectedIndex == gerenciador.cbCasoCarga.Items.Count - 1) //<TODOS>
                            CargasBarrasAtuais = CargasLineares.ToList();
                        else
                        {
                            CasoCargaAtual = CasosCarga[gerenciador.cbCasoCarga.SelectedIndex - 1];
                            CargasBarrasAtuais = CargasLineares.FindAll(o => o.Dados.idCaso == CasoCargaAtual.ID).ToList();
                            CargasLineares.FindAll(o => o.Dados.idCaso == CasoCargaAtual.ID).ForEach(o => o.Visivel = true);
                        }
                    }
                }
                if (gerenciador.cbCasoCarga.Items.Count > 0)
                {
                    if (gerenciador.cbCasoCarga.SelectedIndex > 0)
                    {
                        if (gerenciador.cbCasoCarga.SelectedIndex == gerenciador.cbCasoCarga.Items.Count - 1) //<TODOS>
                            CargasNosAtuais = CargasPontuais.ToList();
                        else
                        {
                            CasoCargaAtual = CasosCarga[gerenciador.cbCasoCarga.SelectedIndex - 1];
                            CargasNosAtuais = CargasPontuais.FindAll(o => o.Dados.idCaso == CasoCargaAtual.ID).ToList();
                        }
                    }

                }

                CargasNosAtuais.ForEach(o => o.Visivel = true);
                CargasBarrasAtuais.ForEach(o => o.Visivel = true);

                double max = 0;
                foreach (TCargaLinear o in CargasBarrasAtuais)
                    if (Math.Abs(o.Dados.valor) > max && o.Dados.valor != 0)
                        max = Math.Abs(o.Dados.valor);

                foreach (TCargaPontual o in CargasNosAtuais)
                    if (Math.Abs(o.Dados.valor) > max && o.Dados.valor != 0)
                        max = Math.Abs(o.Dados.valor);
                double fatorCarga = 0;

                if (max > 0)
                    fatorCarga = (1 / max) * EscalaCargas;
                foreach (TCargaLinear o in CargasBarrasAtuais)
                {
                    bg = Estrutura.barras.Find(b => b.IDBarra == o.idBarra && b.Dados.Tipo != 4);
                    if (bg != null)
                    {
                        {
                            h_ = bg.Dados.secao.b1;
                            v = bg.Dados.secao.h1;

                            o.CriaSetas(Unifilar, v, h_, fatorCarga);
                        }
                    }
                }

                /*Estrutura.cargaLinear.ForEach(obj => obj.Visivel = true);
                Estrutura.cargaPontual.ForEach(obj => obj.Visivel = true);*/

                if (!Unifilar)
                {
                    TBarraGenerica ba;
                    for (int i = 0; i < CargasBarrasAtuais.Count; i++)
                    {
                        ba = Estrutura.barras.Find(o => o.IDBarra == CargasBarrasAtuais[i].idBarra);
                        if (ba != null)
                        {
  //                          CargasBarrasAtuais[i].pIni.x = ba.pIni_Offset.x; CargasBarrasAtuais[i].pIni.y = -ba.pIni_Offset.y; CargasBarrasAtuais[i].pIni.z = -ba.pIni_Offset.z;
//                            CargasBarrasAtuais[i].pFin.x = ba.pFin_Offset.x; CargasBarrasAtuais[i].pFin.y = -ba.pFin_Offset.y; CargasBarrasAtuais[i].pFin.z = -ba.pFin_Offset.z;

                            CargasBarrasAtuais[i].pIni.x = ba.pIni.x; CargasBarrasAtuais[i].pIni.y = ba.pIni.y; CargasBarrasAtuais[i].pIni.z = ba.pIni.z;
                            CargasBarrasAtuais[i].pFin.x = ba.pFin.x; CargasBarrasAtuais[i].pFin.y = ba.pFin.y; CargasBarrasAtuais[i].pFin.z = ba.pFin.z;
                        }
                    }
                }

               /* foreach (TCargaLinear cl in CargasBarrasAtuais)
                {
                    TBarraGenerica bar = Estrutura.barras.Find(o => o.IDBarra == cl.idBarra);
                    if (bar.Dados.Tipo == 4)
                        cl.Visivel = false;
                }*/
            }
            if (gerenciador.cbCasoCarga.SelectedIndex == 0 && MostrarCargas)
            {
                gerenciador.AlternarVisualizacaoCargas();
            }
                /*   AtualizaDisplayList();

                   gerenciador.AtualizaDesenho();
                   glControl.Focus();*/

            if (!AbrindoArquivo)
              if (atuShaders) 
                AtualizaShaders(false);

            DesenhaObjetos();
            glControl.SwapBuffers();
        }
        public void AtualizarDesenho(bool atualizaTriangulos = true, bool atuShaders = true)
        {
            if (atuShaders)  
              AtualizaShaders(atualizaTriangulos);
            AtualizaDisplayList_Nos();
            DesenhaObjetos();
         //   glControl.SwapBuffers();
            glControl.Focus();
        }

        public void MostraCargasNosPeloCaso()
        {
            CargasNosAtuais = new List<TCargaPontual>();
            if (MostrarCargas)
            {
                glControl.Focus();
                if (gerenciador.cbCasoCarga.Items.Count > 0)
                    if (gerenciador.cbCasoCarga.SelectedIndex > 0)
                    {
                        CasoCargaAtual = CasosCarga[gerenciador.cbCasoCarga.SelectedIndex - 1];
                        foreach (TCargaPontual o in CargasNosAtuais)
                        {
                            if (o.Dados.idCaso == CasoCargaAtual.ID)
                                CargasNosAtuais.Add(o);
                        }
                    }
            }
            AtualizarDesenho();
        }

        public TCasosCarga CasoCargaAtual;
        public List<TCargaLinear> CargasBarrasAtuais = new List<TCargaLinear>();
        public List<TCargaPontual> CargasNosAtuais = new List<TCargaPontual>();
        public double conversaoComprimento = 1, conversaoComprimento_Def=1, conversaoForca = 1;
        public double conversaoComprimentoResultado = 1, conversaoForcaResultado = 1, conversaoTensaoResultado = 1;


        vec3 centrorot = new vec3(0);
        public bool CameraOrto = true;
        double[] eixo_x1 = new double[1];
        double[] eixo_y1 = new double[1];
        double x_e, y_e, z_e;
        Matrix4d proj_eixos = Matrix4d.CreatePerspectiveFieldOfView((float)(30 * (3.14 / 180)), (100 - 120) / ((float)100 - 120), (float)1, (float)1000);
        int j;
        double dist = 299.99;
        public void DesenhaFundo()
        {
            GL.Begin(PrimitiveType.Polygon);
            GL.Color3(System.Drawing.Color.White);
            GL.Vertex3(0, glControl.Height, -dist);
            GL.Vertex3(glControl.Width, glControl.Height, -dist);

            GL.Color3(CorCima);
            GL.Vertex3(glControl.Width, 0, -dist);
            GL.Vertex3(0, 0, -dist);
            GL.End();
        }

        double ang1, coca, p1x, p1y, p2x, p2y, dist1, dist2;
        vec3 ponto1 = new vec3(0, 0, 0);
        vec3 ponto2 = new vec3(0, 0, 0);
        vec3 ponto3 = new vec3(0, 0, 0);
     
        OpenTK.Graphics.TextPrinter textoCota = new OpenTK.Graphics.TextPrinter(OpenTK.Graphics.TextQuality.Low);
        OpenTK.Graphics.TextPrinter textoCarga = new OpenTK.Graphics.TextPrinter(OpenTK.Graphics.TextQuality.Low);
        Font fonteCarga = new Font("Tahoma", 10);
        Font fonteTam8 = new Font("Tahoma", 8);

        Font fonte = new Font("Arial", 10);
        vec3 pontoMedioCota = new vec3(0, 0, 0);
        vec3 p1Seta = new vec3(0, 0, 0);
        vec3 p2Seta = new vec3(0, 0, 0); 
        vec3 p3Seta = new vec3(0, 0, 0);
        TPonto pMediocota = new TPonto(0);
        vec3 pMediocarga = new vec3(0);
        vec3 pNo = new vec3(0);


        public bool InsercaoPorCotas = true;
        public double distCota1, distCota2 = 0, angCota, angObjeto;
        public System.Drawing.Color CorCota = System.Drawing.Color.Green;
        void DesenhaInsercaoPorCotas()
        { 
            GL.Color3(CorCota);
            /*  GL.Begin(PrimitiveType.LineLoop);
              for (j = 0; j <= 7; j++)
                GL.Vertex2(mousepoint.px_x + (6 * Math.Cos(j+1 * 6.2831 / 7)), mousepoint.px_y + (6 * Math.Sin(j+1 * 6.2831 / 7)));
              GL.End();*/
            if (LinhaSnapNearest != null)
            {
                ponto1.x = LinhaSnapNearest.pIni.x; ponto1.y = LinhaSnapNearest.pIni.y; ponto1.z = LinhaSnapNearest.pIni.z;
                ponto3.x = LinhaSnapNearest.pFin.x; ponto3.y = LinhaSnapNearest.pFin.y; ponto3.z = LinhaSnapNearest.pFin.z;
                ponto2.x = mousepoint.x; ponto2.y = mousepoint.y; ponto2.z = mousepoint.z;

                distCota1 = ponto1.DistanceTo(ponto2);
                distCota2 = ponto3.DistanceTo(ponto2);

                if (distCota1 < distCota2 || LinhaSnapNearest.auxiliar)
                {
                    pixel1(ref LinhaSnapNearest.pIni.x, ref LinhaSnapNearest.pIni.y, ref LinhaSnapNearest.pIni.z);
                    //        pontoMedioCota = (ponto2 + ponto1) / 2;
                    //        pixel2(ref pontoMedioCota.x, ref  pontoMedioCota.y, ref pontoMedioCota.z);

                    //        textoCota.Print(ponto1.DistanceTo(ponto2).ToString("n2"), fonte, Color.OrangeRed, new RectangleF((float)px_x2[0], (float)px_y2[0], 100, 0));
                }
                else
                {
                    pixel1(ref LinhaSnapNearest.pFin.x, ref LinhaSnapNearest.pFin.y, ref LinhaSnapNearest.pFin.z);
                    //            pontoMedioCota = (ponto2 + ponto3) / 2;
                    //              pixel2(ref pontoMedioCota.x, ref  pontoMedioCota.y, ref pontoMedioCota.z);
                    distCota1 = distCota2;
                    //                .Print(ponto3.DistanceTo(ponto2).ToString("n2"), fonte, Color.OrangeRed, new RectangleF((float)px_x2[0], (float)px_y2[0], 100, 0));
                }

                /*  GL.Begin(PrimitiveType.Lines);
                  GL.Vertex2(mousepoint.px_x, mousepoint.px_y);
                  GL.Vertex2(px_x1[0], px_y1[0]);
                  GL.End();*/

                coca = (mousepoint.px_y - px_y1[0]) / (mousepoint.px_x - px_x1[0]);

                ang1 = Math.Atan(coca);

                if (Math.Abs(coca) < 1) ang1 += 90;

                p1x = mousepoint.px_x - (Math.Cos((ang1) * Const.PIDiv180) * 5);
                p1y = mousepoint.px_y - (Math.Sin((ang1) * Const.PIDiv180) * 5);

                p2x = px_x1[0] - (Math.Cos((ang1) * Const.PIDiv180) * 5);
                p2y = px_y1[0] - (Math.Sin((ang1) * Const.PIDiv180) * 5);

                GL.Enable(EnableCap.LineStipple);
                GL.LineStipple(8, 0xAAAA);

                GL.Begin(PrimitiveType.Lines);
                GL.Vertex2(p1x, p1y);
                GL.Vertex2(p2x, p2y);
                GL.End();

                GL.Disable(EnableCap.LineStipple);
                /*SETAS*/
                GL.Begin(PrimitiveType.LineLoop);
                GL.Vertex2(p1x - 2, p1y - 2);
                GL.Vertex2(p1x + 2, p1y - 2);
                GL.Vertex2(p1x + 2, p1y + 2);
                GL.Vertex2(p1x - 2, p1y + 2);
                GL.Vertex2(p1x - 2, p1y - 2);
                GL.End();

                GL.Begin(PrimitiveType.LineLoop);
                GL.Vertex2(p2x - 2, p2y - 2);
                GL.Vertex2(p2x + 2, p2y - 2);
                GL.Vertex2(p2x + 2, p2y + 2);
                GL.Vertex2(p2x - 2, p2y + 2);
                GL.Vertex2(p2x - 2, p2y - 2);
                GL.End();
                /**/
                p1x = mousepoint.px_x - (Math.Cos((ang1) * Const.PIDiv180) * 25);
                p1y = mousepoint.px_y - (Math.Sin((ang1) * Const.PIDiv180) * 25);

                p2x = px_x1[0] - (Math.Cos((ang1) * Const.PIDiv180) * 25);
                p2y = px_y1[0] - (Math.Sin((ang1) * Const.PIDiv180) * 25);

                ponto1.x = p1x; ponto1.y = p1y;
                ponto2.x = p2x; ponto2.y = p2y;
                angCota = Math.Atan(((p2y - p1y) / (p2x - p1x)));
                //  angCota = ang1;
                //    if (Math.Abs(angCota) < 1) angCota += 90; 
                pontoMedioCota = (ponto1 + ponto2) / 2;

            }
        }

        public static void DesenhaEixosCentrais()
        {
            try
            {
                GL.Color3(System.Drawing.Color.Red);
                GL.Begin(PrimitiveType.Lines);
                GL.Vertex3(-2, 0, 0);
                GL.Vertex3(2, 0, 0);
                GL.End();
                GL.Color3(System.Drawing.Color.Green);
                GL.Begin(PrimitiveType.Lines);
                GL.Vertex3(0, -2, 0);
                GL.Vertex3(0, 2, 0);
                GL.End();
                GL.Color3(System.Drawing.Color.Blue);
                GL.Begin(PrimitiveType.Lines);
                GL.Vertex3(0, 0, -2);
                GL.Vertex3(0, 0, 2);
                GL.End();
            }
            catch (Exception EE)
            {
                MessageBox.Show("erro ao desenhar eixos centrais: " + EE.Message);
            }
        }

        public static double[] pm_Principal = new double[16];
        public static double[] pm_Selecao = new double[16];
        public static double[] pm_Snap = new double[16];

        bool clicouPanel;
        double xant_movPanel = 0, yant_movPanel;

        bool maxtop, maxtop2, maxleft, maxleft2;

        private void menuPrincipal_Opening(object sender, CancelEventArgs e)
        {
            if (((IdObjetoDesenho != null && IdObjetoDesenho != string.Empty))
                || (ObjetosSelecionados.Count > 0 && (FerramentaEdicao != null && !pnDivBarras.Visible)))
              e.Cancel = true;

            if (IdFerramentaEdicao == Const.ID_ROTACIONAR_ELEMENTOS || IdFerramentaEdicao == Const.ID_DIVIDIR_NAS_INTERSECOES ||
                IdFerramentaEdicao == Const.ID_ARTICULAR_ELEMENTO)
              e.Cancel = true;

            if (Estrutura.GetCountElementosSelecionados() == 0)
            {
                mostrarSomenteToolStripMenuItem.Enabled = false;
                esconderSomenteToolStripMenuItem.Enabled = false;
            }
            else
            {
                mostrarSomenteToolStripMenuItem.Enabled = true;
                esconderSomenteToolStripMenuItem.Enabled = true;
            }

            if (Estrutura.barras.Exists(o => o.Selecionado))
            {
                editarBarras.Visible = true;
                Elemento_rotacionar45.Visible = true;
                Elemento_rotacionar45.Visible = true;
                List<TBarraGenerica> b = new List<TBarraGenerica>();
                b = Estrutura.barras.FindAll(o => o.Selecionado);
                editarBarras.Text = b.Count() > 1 ? "Editar elementos" : "Editar elemento";
            }
            else
            {
                editarBarras.Visible = false;
                Elemento_rotacionar45.Visible = false;
            }

            if (Estrutura.apoios.Exists(o => o.Selecionado))
            {
                editarApoios.Visible = true;
                List<TApoio> a = new List<TApoio>();
                a = Estrutura.apoios.FindAll(o => o.Selecionado);
                editarApoios.Text = a.Count() > 1 ? "Editar apoios" : "Editar apoio";
            }
            else
            {
                editarApoios.Visible = false;
            }

            if (Estrutura.cargaLinear.Exists(o => o.Selecionado))
            {
                editarCargaDistribuida.Visible = true;
                List<TCargaLinear> a = new List<TCargaLinear>();
                a = Estrutura.cargaLinear.FindAll(o => o.Selecionado);
                editarCargaDistribuida.Text = a.Count() > 1 ? "Editar cargas nos elementos" : "Editar carga no elemento";
            }
            else
            {
                editarCargaDistribuida.Visible = false;
            }

            if (Estrutura.cargaPontual.Exists(o => o.Selecionado))
            {
                editarCargaPontual.Visible = true;
                List<TCargaPontual> a = new List<TCargaPontual>();
                a = Estrutura.cargaPontual.FindAll(o => o.Selecionado);
                editarCargaPontual.Text = a.Count() > 1 ? "Editar cargas nodais" : "Editar carga nodal";
            }
            else
            {
                editarCargaPontual.Visible = false;
            }
        }

        private void qtdRepeticoesRotacao_ValueChanged(object sender, EventArgs e)
        {
            if (qtdRepeticoesRotacao.Value > 0)
            {
                chMoverManterConexoes.Checked = false;
                chMoverManterConexoes.Enabled = false;
                chApagarOriginalRotacao.Enabled = true;
            }
            else
            {
                chMoverManterConexoes.Enabled = true;

                chApagarOriginalRotacao.Enabled = false;
                chApagarOriginalRotacao.Checked = false;

            }
        }

        private void button9_Click_1(object sender, EventArgs e)
        {
            pnRotacionar.Visible = false;
            CancelaInsercoes();
        }

        private void pnRotacionar_MouseUp(object sender, System.Windows.Forms.MouseEventArgs e)
        {
            clicouPanel = false;
        }

        private void pnRotacionar_MouseMove(object sender, System.Windows.Forms.MouseEventArgs e)
        {
            if ((clicouPanel) && (((sender as System.Windows.Forms.Panel).Top + ((int)(e.Y - yant_movPanel))) < 0))
                maxtop2 = true;
            else
                maxtop2 = false;

            if ((clicouPanel) && (((sender as System.Windows.Forms.Panel).Top + ((sender as System.Windows.Forms.Panel).Height + 40) + ((int)(e.Y - yant_movPanel))) > this.Height))
                maxtop = true;
            else
                maxtop = false;

            if ((clicouPanel) && (((sender as System.Windows.Forms.Panel).Left + ((int)(e.X - xant_movPanel))) < 0))
                maxleft = true;
            else
                maxleft = false;

            if ((clicouPanel) && ((((sender as System.Windows.Forms.Panel).Left + (sender as System.Windows.Forms.Panel).Width + 15) + ((int)(e.X - xant_movPanel))) > this.Width))
                maxleft2 = true;
            else
                maxleft2 = false;

            if (clicouPanel && !maxtop && !maxtop2 && !maxleft && !maxleft2)
            {
                (sender as System.Windows.Forms.Panel).Left += (int)(e.X - xant_movPanel);
                (sender as System.Windows.Forms.Panel).Top += (int)(e.Y - yant_movPanel);
                //  pnCargasLeft = ((sender as System.Windows.Forms.Panel).Left * 100) / this.Width;
                //   pnCargasTop = ((sender as System.Windows.Forms.Panel).Top * 100) / this.Height;

                //ger.xant = e.X;
             //   DesenhaObjetos();
                glControl.SwapBuffers();
            }
        }

        private void pnRotacionar_MouseDown(object sender, System.Windows.Forms.MouseEventArgs e)
        {
            clicouPanel = true;
            xant_movPanel = e.X;
            yant_movPanel = e.Y;
        }

        public static double[] pm_Eixos = new double[16];

        public static double[] Mvm_Principal = new double[16];
        public static double[] Mvm_Zoom = new double[16];

        private void FPrincipal_FormClosing(object sender, FormClosingEventArgs e)
        {
          /*  bool cancel = 
            e.Cancel = cancel;
            base.OnFormClosing(e);*/
        }

        public static double[] Mvm_Selecao = new double[16];
        public static double[] Mvm_Snap = new double[16];
        public static double[] Mvm_Eixo = new double[16];

        public static int[] ViewPortPrincipal = new int[4];

        private void editarCargaPontual_Click(object sender, EventArgs e)
        {
            ChamaEditObjeto(Const.ID_CARGA_PONTUAL);
        }

        private void checkBox1_CheckedChanged(object sender, EventArgs e)
        {

        }

        private void editarCargaDistribuida_Click(object sender, EventArgs e)
        {
            ChamaEditObjeto(Const.ID_CARGA_LINEAR);
        }

        private void rotacionarMembros45CtrlQToolStripMenuItem_Click(object sender, EventArgs e)
        {
           
        }

        public static int[] ViewPortSelecao = new int[4];

        private void rotacionar45CtrlQToolStripMenuItem_Click(object sender, EventArgs e)
        {
            RotacionarSecoesSelecionadas();
        }

        private void checarConcetividadesToolStripMenuItem_Click(object sender, EventArgs e)
        {

        }

        private void rotacionarMembro45_Click(object sender, EventArgs e)
        {

        }

        public static int[] ViewPortSnap = new int[4];
        public static int[] ViewPortEixos = new int[4];

        double[] winZ = new double[1];
        double realY;
        double start_x = 0, start_y = 0, start_z =0;
        double start_x_rotacao = 0, start_y_rotacao = 0;

        double[] posX = new double[1];
        double[] posY = new double[1];
        double[] posZ = new double[1];
        public int mouseX = 0, mouseY = 0;

        private void Desenho_FormClosed(object sender, FormClosedEventArgs e)
        {
            if (shader_triangulos_program != 0)
                GL.DeleteProgram(shader_triangulos_program);
            if (fragment_shader_object_triangulos != 0)
                GL.DeleteShader(fragment_shader_object_triangulos);
            if (vertex_shader_object_triangulos != 0)
                GL.DeleteShader(vertex_shader_object_triangulos);
            if (vertex_buffer_triangulos_principal != 0)
                GL.DeleteBuffers(1, ref vertex_buffer_triangulos_principal);
            if (vertex_buffer_triangulos_deformacao != 0)
                GL.DeleteBuffers(1, ref vertex_buffer_triangulos_deformacao);
            if (vertex_buffer_triangulos_diagramas != 0)
                GL.DeleteBuffers(1, ref vertex_buffer_triangulos_diagramas);
            
           if (shader_arestas_program != 0)
                GL.DeleteProgram(shader_arestas_program);

            if (fragment_shader_object_com_transparencia != 0)
                GL.DeleteShader(fragment_shader_object_com_transparencia);
            if (vertex_shader_object_com_transparencia != 0)
                GL.DeleteShader(vertex_shader_object_com_transparencia);
            if (shader_com_transparencia_program != 0)
                GL.DeleteProgram(shader_com_transparencia_program);

            if (fragment_shader_object_arestas != 0)
                GL.DeleteShader(fragment_shader_object_arestas);
            if (vertex_shader_object_arestas != 0)
                GL.DeleteShader(vertex_shader_object_arestas);
            if (vertex_buffer_arestas != 0)
                GL.DeleteBuffers(1, ref vertex_buffer_arestas);

            if (vertex_buffer_arestas_eixoslocais != 0)
                GL.DeleteBuffers(1, ref vertex_buffer_arestas_eixoslocais);

            if (index_buffer_object != 0)
                GL.DeleteBuffers(1, ref index_buffer_object);
        }

        bool rodando = false;
        public static void SalvarMatriz(ref double[] model)
        {
            GL.GetDouble(GetPName.ModelviewMatrix, model);
            GL.GetDouble(GetPName.ProjectionMatrix, pm_Principal);
            GL.GetInteger(GetPName.Viewport, ViewPortPrincipal);
        }
        public static void UpdateOGLMatrix(ref double[] model, ref double[] proj, ref int[] viewport)
        {
            GL.GetDouble(GetPName.ModelviewMatrix, model);
            GL.GetDouble(GetPName.ProjectionMatrix, proj);
            GL.GetInteger(GetPName.Viewport, viewport);
        }
        double[] winZ0 = new double[1];

        public static double RetPixelX(ref double x, ref double y, ref double z, ref double z_pixel)
        {
            Project(ref pixel_x, ref pixel_y, ref x, ref y, ref z, ref Mvm_Principal, ref pm_Principal, ref ViewPortPrincipal, ref z_pixel);

            return pixel_x[0];
        }

        public static double RetPixelY(ref double x, ref double y, ref double z, ref double z_pixel)
        {
            Project(ref pixel_x, ref pixel_y, ref x, ref y, ref z, ref Mvm_Principal, ref pm_Principal, ref ViewPortPrincipal, ref z_pixel);

            return (ViewPortPrincipal[3] - pixel_y[0]);
        }

        public static double RetPixelX(ref double x, ref double y, ref double z)
        {
            Project(ref pixel_x, ref pixel_y, ref x, ref y, ref z, ref Mvm_Principal, ref pm_Principal, ref ViewPortPrincipal);

            return pixel_x[0];
        }

        public static double RetPixelY(ref double x, ref double y, ref double z)
        {
            Project(ref pixel_x, ref pixel_y, ref x, ref y, ref z, ref Mvm_Principal, ref pm_Principal, ref ViewPortPrincipal);

            return (ViewPortPrincipal[3] - pixel_y[0]);
        }

        void UnProject(ref int mx, ref int my, ref double _winZ, ref double[] model, ref double[] proj, ref int[] viewport, ref double[] x_, ref double[] y_, ref double[] z_)
        {
            realY = viewport[3] - (int)my;
            winZ0[0] = _winZ;
            OpenTK.Graphics.Glu.UnProject(mx, realY, winZ0[0], model, proj, viewport, x_, y_, z_);
        }
        public static void Project(ref double[] pixelX, ref double[] pixelY, ref double worldX, ref double worldY, ref double worldZ, ref double[] model, ref double[] proj, ref int[] viewport)
        {
            OpenTK.Graphics.Glu.Project(worldX, worldY, worldZ, model, proj, viewport, pixelX, pixelY, pixelZ);
            pixelY[0] = viewport[3] - pixelY[0];
        }
        static double[] pixelZ = new double[1];
        public static void Project(ref double[] pixelX, ref double[] pixelY, ref double worldX, ref double worldY, ref double worldZ, ref double[] model, ref double[] proj, ref int[] viewport, ref double Z)
        {
            OpenTK.Graphics.Glu.Project(worldX, worldY, worldZ, model, proj, viewport, pixelX, pixelY, pixelZ);

            pixelY[0] = viewport[3] - pixelY[0];
            Z = pixelZ[0];
        }


        void DesenhaCursorXY()
        {
            GL.Color3(System.Drawing.Color.Black);
            GL.Begin(PrimitiveType.Lines);
            GL.Vertex3(Coord_PlanoTrabalho.x - 30, Coord_PlanoTrabalho.y, Coord_PlanoTrabalho.z);
            GL.Vertex3(Coord_PlanoTrabalho.x + 30, Coord_PlanoTrabalho.y, Coord_PlanoTrabalho.z);
            GL.End();

            GL.Begin(PrimitiveType.Lines);
            GL.Vertex3(Coord_PlanoTrabalho.x, Coord_PlanoTrabalho.y - 30, Coord_PlanoTrabalho.z);
            GL.Vertex3(Coord_PlanoTrabalho.x, Coord_PlanoTrabalho.y + 30, Coord_PlanoTrabalho.z);
            GL.End();
        }

        void DesenhaCursorPlano()
        {
            //  ControleCAD.DrawLine(mPenCursor, Posicao.X-15, Posicao.Y, Posicao.X + 15, Posicao.Y);
            //  ControleCAD.DrawLine(mPenCursor, Posicao.X, Posicao.Y-15, Posicao.X, Posicao.Y+15);
            GL.Color3(System.Drawing.Color.Black);
            if (PlanoTrabalho.plano == "xy")
            {
                GL.Begin(PrimitiveType.Lines);
                GL.Vertex3(Coord_PlanoTrabalho.x - 100, Coord_PlanoTrabalho.y, Coord_PlanoTrabalho.z);
                GL.Vertex3(Coord_PlanoTrabalho.x + 100, Coord_PlanoTrabalho.y, Coord_PlanoTrabalho.z);
                GL.End();

                GL.Begin(PrimitiveType.Lines);
                GL.Vertex3(Coord_PlanoTrabalho.x, Coord_PlanoTrabalho.y - 100, Coord_PlanoTrabalho.z);
                GL.Vertex3(Coord_PlanoTrabalho.x, Coord_PlanoTrabalho.y + 100, Coord_PlanoTrabalho.z);
                GL.End();
            }
            else
            if (PlanoTrabalho.plano == "xz")
            {
                GL.Begin(PrimitiveType.Lines);
                GL.Vertex3(Coord_PlanoTrabalho.x - 100, Coord_PlanoTrabalho.y, Coord_PlanoTrabalho.z);
                GL.Vertex3(Coord_PlanoTrabalho.x + 100, Coord_PlanoTrabalho.y, Coord_PlanoTrabalho.z);
                GL.End();

                GL.Begin(PrimitiveType.Lines);
                GL.Vertex3(Coord_PlanoTrabalho.x, Coord_PlanoTrabalho.y, Coord_PlanoTrabalho.z - 100);
                GL.Vertex3(Coord_PlanoTrabalho.x, Coord_PlanoTrabalho.y, Coord_PlanoTrabalho.z + 100);
                GL.End();
            }
            else
            if (PlanoTrabalho.plano == "yz")
            {
                GL.Begin(PrimitiveType.Lines);
                GL.Vertex3(Coord_PlanoTrabalho.x, Coord_PlanoTrabalho.y - 100, Coord_PlanoTrabalho.z);
                GL.Vertex3(Coord_PlanoTrabalho.x, Coord_PlanoTrabalho.y + 100, Coord_PlanoTrabalho.z);
                GL.End();

                GL.Begin(PrimitiveType.Lines);
                GL.Vertex3(Coord_PlanoTrabalho.x, Coord_PlanoTrabalho.y, Coord_PlanoTrabalho.z - 100);
                GL.Vertex3(Coord_PlanoTrabalho.x, Coord_PlanoTrabalho.y, Coord_PlanoTrabalho.z + 100);
                GL.End();
            }

        }


        private void Desenho_MouseDown(object sender, System.Windows.Forms.MouseEventArgs e)
        {
 
        }

        List<TLayer> LayersTemp;
        void RefazEstadoLayers()
        {
            if (LayersTemp != null)
            {
                if (LayersTemp.Count > 0)
                {
                    for (int i = 0; i < Estrutura.layers.Count; i++)
                    {
                        Estrutura.layers[i].Congelado = LayersTemp[i].Congelado;
                        Estrutura.layers[i].Travado = LayersTemp[i].Travado;
                        Estrutura.layers[i].Ligado = LayersTemp[i].Ligado;
                    }
                }
                LayersTemp = null;
            }
        }

        private void Desenho_MouseUp(object sender, System.Windows.Forms.MouseEventArgs e)
        {

        }


        public double anguloRotacaoSecao = 0;
        public void RotacionarSecoesSelecionadas()
        {
            Alterou(true);
            TSecao secaoCopia;
            vec3 centroRotacao = new vec3(0, 0, 0);
            vec3 coord1 = new vec3(0, 0, 0);

            TBarraGenerica br;
            foreach (TObjetoDesenho o in ObjetosSelecionados)
            {
                if (o.Tipo == Const.ID_BARRAGENERICA)
                {
                    anguloRotacaoSecao = 0;
                    br = (o as TBarraGenerica);
                    secaoCopia = (TSecao)br.Dados.secao.Clone();

                    if (anguloRotacaoSecao + 45+(o as TBarraGenerica).Dados.anguloRotacao >= 360)
                        anguloRotacaoSecao = 0;
                    else
                        anguloRotacaoSecao += ((o as TBarraGenerica).Dados.anguloRotacao + 45);

                    centroX = pixelX(0);
                    centroY = pixelY(0);

                    if (secaoCopia.poligonos != null)
                    {
                   //     secaoCopia.poligono.CalculaPropriedades();

                        centroRotacao.y = 0;
                        centroRotacao.x = 0;

                        for (int r = 0; r < (secaoCopia.poligonos.Count()); r++)
                        {
                            for (int i = 0; i < (secaoCopia.poligonos[r].coords.Count()); i++)
                            {
                                coord1 = new vec3(secaoCopia.poligonos[r].coords[i].X, secaoCopia.poligonos[r].coords[i].Y, 0);
                                coord1 = coord1.Rotate(centroRotacao, (anguloRotacaoSecao) * Const.PIDiv180);

                                secaoCopia.poligonos[r].coords[i].X = coord1.x;
                                secaoCopia.poligonos[r].coords[i].Y = coord1.y;
                            }
                        }
                        (o as TBarraGenerica).Dados.secao = (TSecao)secaoCopia.Clone();
                       // (o as TBarraGenerica).Dados.secaoSemRotacao = (TSecao)gerenciador.DadosBarra.secao.Clone();
                        (o as TBarraGenerica).Dados.anguloRotacao = anguloRotacaoSecao;

                    }

                    br.pMedioBarra = new vec3((br.pIni.x + br.pFin.x) / 2, (br.pIni.y + br.pFin.y) / 2, (br.pIni.z + br.pFin.z) / 2);
                    br.OrientaSecaoNoEspaco();
                    br.CriaPesoProprio();
                    br.CriarEixosLocais(tamArticulacoes);
                    br.DirtySelecao = true;
                    br.DirtyArestas = true;
                    br.DirtyTriangulos = true;
                }
            }
            AtualizarDesenho(true, true);
            glControl.SwapBuffers();
            //Atualiza_Segmentos_e_Snap();
        }
        public bool chamouEdicaoElemento = false;

        public void AtualizaBarrasSelecionadas(bool atuShaders = true)
        {
            try
            {
                /* Estrutura.Rgb_Barras[0] = Estrutura.layers[5].Rgb[0];
                 Estrutura.Rgb_Barras[1] = Estrutura.layers[5].Rgb[1];
                 Estrutura.Rgb_Barras[2] = Estrutura.layers[5].Rgb[2];*/
   
                List<TBarraGenerica> barras = ObjetosSelecionados
                    .OfType<TBarraGenerica>()
                    .ToList();

                for (int i = 0; i < barras.Count; i++)
                {                   
                   TBarraGenerica o = barras[i];
                   o.pMedioBarra = new vec3((o.pIni.x + o.pFin.x) / 2, (o.pIni.y + o.pFin.y) / 2, (o.pIni.z + o.pFin.z) / 2);
                   o.OrientaSecaoNoEspaco();
                   o.CriaPesoProprio();
                   o.CriarEixosLocais(tamArticulacoes);

                   if (gerenciador.ConfiguracoesPGi.F3DOpcoesVisualizacao.CorPorSecao)
                   {
                       TSecao ss = Secoes.Find(s => s.id == o.Dados.secao.id);
                       o.Rgb[0] = ss.Rgb[0];
                       o.Rgb[1] = ss.Rgb[1];
                       o.Rgb[2] = ss.Rgb[2];
                   }
                   else
                   if (gerenciador.ConfiguracoesPGi.F3DOpcoesVisualizacao.CorPorTipoDeElemento)
                   {
                       o.Rgb[0] = Estrutura.Rgb_Barras[0];
                       o.Rgb[1] = Estrutura.Rgb_Barras[1];
                       o.Rgb[2] = Estrutura.Rgb_Barras[2];
                   }                   
                }

                AtualizaCargas(atuShaders);
            //    AtualizarDesenho(true, atuShaders);
                glControl.SwapBuffers();

            }
            catch (Exception e)
            {
                gerenciador.FechaAguardar();
                MessageBox.Show(e.Message);
            }
        }

        public void ChamaEditObjeto(string tipo = "")
        {
         //   Estrutura.AtualizaListaObjetos();
            //CancelaResultados();
            TObjetoDesenho objetoUnico;
            //      List<TObjetoDesenho> 
          //  foreach (TObjetoDesenho objeto in ObjetosSelecionados)
            {
                //   if (objetoUnico != null || (objetoUnico == null && ObjetosSelecionados.Count > 0))
                // if (objeto.layer != null)
                //   if (objeto.layer.Ligado || !objeto.layer.Congelado || objeto.layer.Travado)
                objetoUnico = ObjetosSelecionados[0];
                {
                    if (tipo == Const.ID_PONTO)
                    {
                        if (gerenciador.fEditaNos != null)
                           gerenciador.fEditaNos.Close();

                        gerenciador.fEditaNos = new FEdicaoNos(gerenciador);
                        objetoUnico = ObjetosSelecionados.First(o => o.Tipo == Const.ID_PONTO);
                        gerenciador.fEditaNos.edX.Text = (objetoUnico as TPonto).x.ToString("n5");
                        gerenciador.fEditaNos.edY.Text = ((objetoUnico as TPonto).y*-1).ToString("n5");
                        gerenciador.fEditaNos.edZ.Text = ((objetoUnico as TPonto).z * -1).ToString("n5");
                        DialogResult result = gerenciador.fEditaNos.ShowDialog();

                        if (result == DialogResult.Yes)
                        {
                            CancelaInsercoes();
                            SetaSelecionados(false, -1);
                            Atualiza_Segmentos_e_Snap();
                         //   DeletaSelecionados(-1);
                            Atualiza_pIni_pFin_das_Barras_e_Cargas();

                            gerenciador.FechaAguardar();
                        }
                    }
                    else
                    if (tipo == Const.ID_BARRAGENERICA)
                    {
                        objetoUnico = ObjetosSelecionados.First(o => o.Tipo == Const.ID_BARRAGENERICA);

                        List<TObjetoDesenho> barra = ObjetosSelecionados.Where(o => o.Tipo == Const.ID_BARRAGENERICA).ToList();
                        for (int i = 0; i < barra.Count; i++)
                        {
                            if ((barra[i] as TBarraGenerica).Dados.Tipo != 4)
                            {
                                objetoUnico = barra[i];
                                break;
                            }
                        }

                        chamouEdicaoElemento = true;

                        if (gerenciador.DadosBarra != null)
                            gerenciador.DadosBarra.Close();

                        chamouEdicaoElemento = false;
                        gerenciador.DadosBarra = new FDadosBarra(gerenciador, (objetoUnico as TBarraGenerica).Dados);
                        gerenciador.DadosBarra.alterando = true;
                        gerenciador.DadosBarra.Text = "Alterar elemento";
                        gerenciador.DadosBarra.lbComprimento.Visible = true;
                        gerenciador.DadosBarra.lbComprimento.Text =
                        "Comprimento: " + (objetoUnico as TBarraGenerica).comprimento.ToString("n3") + " m";
                        gerenciador.DadosBarra.numero.Text = (objetoUnico as TBarraGenerica).IDBarra.ToString();

                        //     gerenciador.DadosBarra.cbPosicaoArticulacao.SelectedIndex = (objetoUnico as TBarraGenerica).Articulacao;

                        if (ObjetosSelecionados.Count > 1)
                        {
                            gerenciador.DadosBarra.numero.Visible = false;
                            gerenciador.DadosBarra.lbNumero.Visible = false;
                            gerenciador.DadosBarra.Text = "Alterar elementos";
                            gerenciador.DadosBarra.multi = true;
                        }

                        DialogResult result = gerenciador.DadosBarra.ShowDialog();
                        gerenciador.DadosBarra.Enquadrar();

                        if (result == DialogResult.Yes)
                        {
                            gerenciador.ChamaAguardar(gerenciador, "Atualizando. Aguarde...");

                            AtualizaBarrasSelecionadas();

                            CancelaInsercoes();
                            //    SetaSelecionados(false, -1);


                            //  Atualiza_Segmentos_e_Snap();
                            //     DeletaSelecionados(-1);

                            //     AtualizaCargas();
                            gerenciador.FechaAguardar();
                        }

                        gerenciador.DadosBarra = null;
                        GC.Collect();

                        //  break;
                    }
                    else
                    if (tipo == Const.ID_APOIO)
                    {
                        objetoUnico = ObjetosSelecionados.First(o => o.Tipo == tipo);

                        if (gerenciador.fApoio != null)
                            gerenciador.fApoio.Close();

                        gerenciador.fApoio = new FApoio(gerenciador, (objetoUnico as TApoio).Dados);
                        gerenciador.fApoio.alterando = true;
                        gerenciador.fApoio.Text = "Alterar restrição de apoio";
                        DialogResult result = gerenciador.fApoio.ShowDialog();

                        if (result == DialogResult.Yes)
                        {
                            foreach (TObjetoDesenho o in ObjetosSelecionados)
                            {
                                if (o.Tipo == Const.ID_APOIO)
                                    (o as TApoio).Dados = DadosApoio;
                            }

                            CancelaInsercoes();
                            SetaSelecionados(false, -1);
                            Atualiza_Segmentos_e_Snap();
                        }
                        gerenciador.fApoio = null;
                        AtualizaShaders();
                        //       break;
                    }
                    else
                    if (objetoUnico.Tipo == Const.ID_TRECHOVIGA)
                    {
                        gerenciador.FDadosViga = new DadosViga(gerenciador, (objetoUnico as TTrechoViga).Dados, (objetoUnico as TTrechoViga).Dados.indiceTipo);
                        gerenciador.FDadosViga.alterando = true;
                        gerenciador.FDadosViga.Text = "Alterar dados de viga";
                        gerenciador.FDadosViga.btSalvar.Text = "Salvar";
                        DialogResult result = gerenciador.FDadosViga.ShowDialog();

                        if (result == DialogResult.Yes)
                        {
                            //    gerenciador.FDadosViga.GravaDados();

                            TTrechoViga New = new TTrechoViga((TPonto)(objetoUnico as TTrechoViga).pIni.Clone(),
                                                              (TPonto)(objetoUnico as TTrechoViga).pFin.Clone(),
                                                              ((TDadosViga)(objetoUnico as TTrechoViga).Dados.Clone()),
                                                              objetoUnico.layer, (objetoUnico as TTrechoViga).Pavimento, Linhas, true);

                            DeletaSelecionados((objetoUnico as TTrechoViga).Pavimento);
                            AdicionaObjeto(New, (objetoUnico as TTrechoViga).Pavimento);
                            Atualiza_Segmentos_e_Snap();
                            AtualizaShaders();
                            //                  break;
                        }
                    }
                    else
                    if (objetoUnico.Tipo == Const.ID_LAJE)
                    {
                        gerenciador.FDadosLaje = new DadosLaje(gerenciador, (objetoUnico as TLaje).Dados, (objetoUnico as TLaje).Dados.indiceTipo);
                        gerenciador.FDadosLaje.Text = "Alterar dados de laje";
                        gerenciador.FDadosLaje.btSalvar.Text = "Salvar";
                        gerenciador.FDadosLaje.alterando = true;

                        DialogResult result = gerenciador.FDadosLaje.ShowDialog();

                        if (result == DialogResult.Yes)
                        {
                            //  gerenciador.FDadosLaje.GravaDados();
                            (objetoUnico as TLaje).Titulo.texto = (objetoUnico as TLaje).Dados.nome + (objetoUnico as TLaje).Dados.numero;
                            (objetoUnico as TLaje).Titulo2.texto = "h: " + (objetoUnico as TLaje).Dados.h;

                            //      break;
                            //  AtualizaElementosEstrutura();
                            //  AtualizaDisplayList();
                        }

                    }
                    else
                        if (objetoUnico.Tipo == Const.ID_PILAR)
                    {
                        gerenciador.FDadosPilar = new DadosPilar(gerenciador, (objetoUnico as TPilar).Dados, (objetoUnico as TPilar).Dados.indiceTipo);
                        gerenciador.FDadosPilar.Text = "Alterar dados de pilar";
                        gerenciador.FDadosPilar.btSalvar.Text = "Salvar";
                        gerenciador.FDadosPilar.alterando = true;

                        string secaoAnterior = (objetoUnico as TPilar).Dados.Poligono.TipoSecao;
                        DialogResult result = gerenciador.FDadosPilar.ShowDialog();

                        if (result == DialogResult.Yes)
                        {
                            // gerenciador.FDadosPilar.GravaDados();

                            int VerticeAtual = (objetoUnico as TPilar).VerticeAtual;
                            if (secaoAnterior != (((TDadosPilar)(objetoUnico as TPilar).Dados).Poligono.TipoSecao))
                                VerticeAtual = 0;

                            TPilar New = new TPilar((TPonto)(objetoUnico as TPilar).pIni.Clone(),
                                                     (TPonto)(objetoUnico as TPilar).pFin.Clone(),
                                                     ((TDadosPilar)(objetoUnico as TPilar).Dados.Clone()),
                                                     objetoUnico.layer, (objetoUnico as TPilar).Pavimento, Linhas, VerticeAtual);

                            DeletaSelecionados(-1);

                            AdicionaObjeto(New, PavimentoAtual);

                            //(objeto as TPilar).Texto1.texto = (objeto as TPilar).Dados.nome + (objeto as TPilar).Dados.numero;

                            DeletaSelecionados(PavimentoAtual);
                            Atualiza_Segmentos_e_Snap();
                            AtualizaShaders();
                            //     break;
                        }
                    }
                    else
                    if (gerenciador.SelecaoCargaDistribuida.Checked && tipo == Const.ID_CARGA_LINEAR)
                    {
                        if (ObjetosSelecionados.Exists(o => o.Tipo == tipo))
                        {
                            objetoUnico = ObjetosSelecionados.First(o => o.Tipo == tipo);
                            

                            if ((objetoUnico as TCargaLinear).Dados.idCaso != 1)
                            {
                                if (gerenciador.CargaBarra != null)
                                    gerenciador.CargaBarra.Close();

                                gerenciador.CargaBarra = new FCarga(gerenciador, Const.ID_CARGA_LINEAR, null, (objetoUnico as TCargaLinear), true);
                                DialogResult result = gerenciador.CargaBarra.ShowDialog(gerenciador);
                                if (result == DialogResult.Yes)
                                {
                                    foreach (TObjetoDesenho o in ObjetosSelecionados)
                                    {
                                        if (o.Tipo == Const.ID_CARGA_LINEAR)
                                        {
                                          /*  TCargaLinear carga = (TCargaLinear)o;

                                            List<TCargaLinear> cargasRepetidas = CargasLineares.FindAll(  c => c.Dados.valor == carga.Dados.valor
                                                    && c.Dados.idCaso == carga.Dados.idCaso
                                                    && c.Dados.ProjecaoGlobal == carga.Dados.ProjecaoGlobal
                                                    && c.Dados.concentrada == carga.Dados.concentrada
                                                    && c.Dados.distribuida == carga.Dados.distribuida
                                                    && c.Dados.posicaoRelativa == carga.Dados.posicaoRelativa
                                                    && c.Dados.DirecaoProjecao == carga.Dados.DirecaoProjecao);

                                            if (carga.Dados.concentrada)
                                            {
                                                double posicaoFinal = DadosCarga.posicaoRelativa ? (o as TBarraGenerica).comprimento * DadosCarga.d : DadosCarga.d;
                                                if (posicaoFinal > (o as TBarraGenerica).comprimento)
                                                {
                                                    MessageBox.Show("A carga concentrada de " + DadosCarga.valor.ToString("n2") + " kN no elemento " + (o as TBarraGenerica).IDBarra + " não está posicionada no interior do elemento!", "Erro", MessageBoxButtons.OK, MessageBoxIcon.Error);
                                                    continue;
                                                }
                                            }

                                            if (cargasRepetidas.Exists(cr => cr.idBarra == (o as TBarraGenerica).IDBarra))
                                            {
                                                MessageBox.Show("Já existe a carga de " + cargasRepetidas[0].Dados.valor.ToString("n2") + " kN no elemento " + (o as TBarraGenerica).IDBarra + " para esse mesmo caso de carga!", "Erro", MessageBoxButtons.OK, MessageBoxIcon.Error);
                                                continue;
                                            }*/

                                            (o as TCargaLinear).Dados = (TDadosCarga)gerenciador.CargaBarra.cargaLinear.Dados.Clone();
                                            //     (o as TCargaLinear).RotacionaDiagramaCarga(true, EscalaCargas);
                                        }
                                    }

                                    CancelaInsercoes();
                                    SetaSelecionados(false, -1);
                                    Atualiza_Segmentos_e_Snap();
                                }
                                gerenciador.CargaBarra = null;
                                GC.Collect();
                                //        break;
                            }
                        }
                    }
                    else
                        if (gerenciador.SelecaoCargaPontual.Checked && tipo == Const.ID_CARGA_PONTUAL)
                    {
                        if (ObjetosSelecionados.Exists(o => o.Tipo == tipo))
                        {
                            objetoUnico = ObjetosSelecionados.First(o => o.Tipo == tipo);

                            if (gerenciador.CargaNodal != null)
                                gerenciador.CargaNodal.Close();

                            gerenciador.CargaNodal = new FCargaNodal(gerenciador, Const.ID_CARGA_PONTUAL, (objetoUnico as TCargaPontual), true);
                            DialogResult result = gerenciador.CargaNodal.ShowDialog();
                            if (result == DialogResult.Yes)
                            {
                                foreach (TObjetoDesenho o in ObjetosSelecionados)
                                    if (o.Tipo == Const.ID_CARGA_PONTUAL)
                                        (o as TCargaPontual).Dados = (TDadosCarga)gerenciador.CargaNodal.cargaPontual.Dados.Clone();

                                CancelaInsercoes();
                                SetaSelecionados(false, -1);
                                Atualiza_Segmentos_e_Snap();
                                MostrarCargasPeloCaso();
                            }
                            gerenciador.CargaNodal = null;
                            GC.Collect();
                            //        break;
                        }
                    }

                        //objeto.SetaSelecao(false, false);
                    }
            }

            SetaSelecionados(false, -1);
            AtualizaShaders();
            this.Refresh();
            GC.Collect();
        }

        public void Desenho_MouseDoubleClick(object sender, System.Windows.Forms.MouseEventArgs e)
        {

        }

        private void copiarToolStripMenuItem_Click(object sender, EventArgs e)
        {
            gerenciador.Copiar();
        }

        private void colarCtrlVToolStripMenuItem_Click(object sender, EventArgs e)
        {
         
        }

        List<vec3> pts;

        bool achouPt(double x, double y)
        {
            foreach (vec3 v in pts)
                if (Geom.Iguais(v.x, x) && Geom.Iguais(v.y, y))
                    return true;

            return false;
        }

        //    vec3 xx = p1_p2 * px_p2;
        //  c /= (p1_p2.getMagnitude()  * px_p2.getMagnitude());
        //  s /= (p1_p2.getMagnitude() * px_p2.getMagnitude());
        // double angle = Math.Atan2(s,c);
        // angle = angle;
        // double alfa = Math.Acos(c / (ab.getMagnitude() * bc.getMagnitude()));
        // alfa = alfa / Const.PIDiv180;

        private void Desenho_MouseEnter(object sender, EventArgs e)
        {

        }

        private void cancelarToolStripMenuItem_Click(object sender, EventArgs e)
        {
            CancelaInsercoes();
        }

        private void Desenho_Click(object sender, EventArgs e)
        {
      //      gerenciador.dockPanel.Focus();
        }

        private void Desenho_Resize(object sender, EventArgs e)
        {
            //   h = this.Height;
            //  w = this.Width;
        }

        private void glControl_Resize(object sender, EventArgs e)
        {
            try
            {
                try
                {
                    SetupCamera(false);

                    h = this.Height;
                    w = this.Width;

                    if (edX.Visible && pnRepeticoesCopia.Visible)
                        HabilitaCoords(true, true, false);

                    if (edX.Visible && !pnRepeticoesCopia.Visible)
                        HabilitaCoords(true, false, true);

                    if (edX.Visible && pnEspelhar.Visible)
                        HabilitaCoords(true, false, false,false, false);

                    if (edX.Visible && !pnEspelhar.Visible)
                        HabilitaCoords(true, false, true, false , true);

                    if (edX.Visible && pnRotacionar.Visible)
                        HabilitaCoords(true, false, false, true);

                    if (edX.Visible && !pnRotacionar.Visible)
                        HabilitaCoords(true, false, true, false);

                    if (edX.Visible && pnMover.Visible)
                        HabilitaCoords(true, false, false, false, false, false);

                    if (edX.Visible && !pnMover.Visible)
                        HabilitaCoords(true, false, true, false, false, true);

                    Comando.Top = this.Height - 15;
                    Comando.Left = this.Width / 2 ;

                }
                catch (ArgumentException err)
                {
                    MessageBox.Show(err.Message);
                }
            }

            catch (Exception err2)
            {
                MessageBox.Show(err2.Message);
            }
        }

        /*            edX.Top = this.Height - 50;
            edY.Top = this.Height-50;
            edZ.Top = this.Height - 50;

            edX.Left = this.Width/2 - 100;
            edY.Left = this.Width / 2 -50;
            edZ.Left = this.Width / 2;*/

        void RetanguloCoordenadas()
        {
            GL.Color3(System.Drawing.Color.Black);
            GL.Begin(PrimitiveType.LineLoop);
            GL.Vertex3(this.Width / 2 - 155, this.Height - 5, Coord_PlanoSelecao.z);
            GL.Vertex3(this.Width / 2 + 90, this.Height - 5, Coord_PlanoSelecao.z);
            GL.Vertex3(this.Width / 2 + 90, this.Height - 35, Coord_PlanoSelecao.z);
            GL.Vertex3(this.Width / 2 - 155, this.Height - 35, Coord_PlanoSelecao.z);
            GL.End();

            GL.Color4(System.Drawing.Color.FromArgb(100, System.Drawing.Color.Gray));
            GL.Begin(PrimitiveType.Polygon);
            GL.Vertex3(this.Width / 2 - 155, this.Height - 5, Coord_PlanoSelecao.z);
            GL.Vertex3(this.Width / 2 + 90, this.Height - 5, Coord_PlanoSelecao.z);
            GL.Vertex3(this.Width / 2 + 90, this.Height - 35, Coord_PlanoSelecao.z);
            GL.Vertex3(this.Width / 2 - 155, this.Height - 35, Coord_PlanoSelecao.z);
            GL.End();

            if (!NovaCoordX && !NovaCoordY && !NovaCoordZ)
            {
                //  edX.Focus();
                // edX.SelectAll();
            }
        }

        void DesenhaRetanguloSelecao()
        {
            //   Selecting   = true;
            SelIntegral = false;
            SelParcial = false;

            h = this.Height;
            w = this.Width;

            if (Coord_PlanoSelecao.x < Ponto1_Selecao.x)
            {

                SelIntegral = true;
                if (Coord_PlanoSelecao.y > Ponto1_Selecao.y)
                {
                    SetaRetanguloSelecao(Coord_PlanoSelecao.x, Coord_PlanoSelecao.y,
                                         Coord_PlanoSelecao.x, Ponto1_Selecao.y,
                                         Ponto1_Selecao.x, Ponto1_Selecao.y,
                                         Ponto1_Selecao.x, Coord_PlanoSelecao.y);

                    RetanguloSelecao2[0].x = Ponto1.X;
                    RetanguloSelecao2[0].y = Ponto1.Y;

                    RetanguloSelecao2[1].x = Ponto1.X;
                    RetanguloSelecao2[1].y = mouseY;

                    RetanguloSelecao2[2].x = mouseX;
                    RetanguloSelecao2[2].y = mouseY;

                    RetanguloSelecao2[3].x = mouseX;
                    RetanguloSelecao2[3].y = Ponto1.Y;

                    //   1____4 

                    //   2____3
                }
                else
                if (Coord_PlanoSelecao.y < Ponto1_Selecao.y)
                {
                    SetaRetanguloSelecao(Coord_PlanoSelecao.x, Coord_PlanoSelecao.y,
                                          Ponto1_Selecao.x, Coord_PlanoSelecao.y,
                                          Ponto1_Selecao.x, Ponto1_Selecao.y,
                                          Coord_PlanoSelecao.x, Ponto1_Selecao.y);

                    RetanguloSelecao2[0].x = Ponto1.X;
                    RetanguloSelecao2[0].y = Ponto1.Y;

                    RetanguloSelecao2[1].x = mouseX;
                    RetanguloSelecao2[1].y = Ponto1.Y;

                    RetanguloSelecao2[2].x = mouseX;
                    RetanguloSelecao2[2].y = mouseY;

                    RetanguloSelecao2[3].x = Ponto1.X;
                    RetanguloSelecao2[3].y = mouseY;

                    //   4____3 
                    //   |    |
                    //   1____2
                };
            }
            else
            {
                SelParcial = true;

                if (Coord_PlanoSelecao.y > Ponto1_Selecao.y)
                {
                    SetaRetanguloSelecao(Coord_PlanoSelecao.x, Coord_PlanoSelecao.y,
                                         Ponto1_Selecao.x, Coord_PlanoSelecao.y,
                                         Ponto1_Selecao.x, Ponto1_Selecao.y,
                                         Coord_PlanoSelecao.x, Ponto1_Selecao.y);

                    RetanguloSelecao2[0].x = Ponto1.X;
                    RetanguloSelecao2[0].y = Ponto1.Y;

                    RetanguloSelecao2[1].x = mouseX;
                    RetanguloSelecao2[1].y = Ponto1.Y;

                    RetanguloSelecao2[2].x = mouseX;
                    RetanguloSelecao2[2].y = mouseY;

                    RetanguloSelecao2[3].x = Ponto1.X;
                    RetanguloSelecao2[3].y = mouseY;

                    //   2____1
                    //   |    |
                    //   3____4
                }
                else
                    if (Coord_PlanoSelecao.y < Ponto1_Selecao.y)
                {
                    SetaRetanguloSelecao(Coord_PlanoSelecao.x, Coord_PlanoSelecao.y,
                                         Coord_PlanoSelecao.x, Ponto1_Selecao.y,
                                         Ponto1_Selecao.x, Ponto1_Selecao.y,
                                         Ponto1_Selecao.x, Coord_PlanoSelecao.y);

                    RetanguloSelecao2[0].x = Ponto1.X;
                    RetanguloSelecao2[0].y = Ponto1.Y;

                    RetanguloSelecao2[1].x = Ponto1.X;
                    RetanguloSelecao2[1].y = mouseY;

                    RetanguloSelecao2[2].x = mouseX;
                    RetanguloSelecao2[2].y = mouseY;

                    RetanguloSelecao2[3].x = mouseX;
                    RetanguloSelecao2[3].y = Ponto1.Y;

                    //   3____2
                    //   |    |
                    //   4____1
                };
            };

            //   if (Coord_PlanoSelecao.x != Ponto1_Selecao.x)
            {
                //  this.Text = "x: " + Coord_PlanoSelecao.x.ToString("n2") + " y: " + Coord_PlanoSelecao.y.ToString("n2");
                if (Coord_PlanoSelecao.x > Ponto1_Selecao.x)
                {
                    GL.Enable(EnableCap.LineStipple);
                    GL.LineStipple(6, 0xAAAA);
                }

                GL.Color3(System.Drawing.Color.Black);
                if (CorCima == System.Drawing.Color.Black)
                    GL.Color3(System.Drawing.Color.Gray);

                GL.Begin(PrimitiveType.LineLoop);
                for (int i = 0; i < RetanguloSelecao.Count(); i++)
                    GL.Vertex3(RetanguloSelecao[i].x, RetanguloSelecao[i].y, Coord_PlanoSelecao.z);
                GL.End();

                GL.Color4(System.Drawing.Color.FromArgb(40, 0, 255, 0));
                GL.Begin(PrimitiveType.Quads);
                for (int i = 0; i < RetanguloSelecao.Count(); i++)
                    GL.Vertex3(RetanguloSelecao[i].x, RetanguloSelecao[i].y, Coord_PlanoSelecao.z);
                GL.End();

                GL.Disable(EnableCap.LineStipple);
                //     if (this.BackColor == Color.White)
                //        GL.ClearColor(Color.Black);
                /*
                                ControleCAD.DrawLine(mPen, pixelX(RetanguloSelecao[0].x), pixelY(RetanguloSelecao[0].y), pixelX(RetanguloSelecao[1].x), pixelY(RetanguloSelecao[1].y));
                                ControleCAD.DrawLine(mPen, pixelX(RetanguloSelecao[1].x), pixelY(RetanguloSelecao[1].y), pixelX(RetanguloSelecao[2].x), pixelY(RetanguloSelecao[2].y));
                                ControleCAD.DrawLine(mPen, pixelX(RetanguloSelecao[2].x), pixelY(RetanguloSelecao[2].y), pixelX(RetanguloSelecao[3].x), pixelY(RetanguloSelecao[3].y));
                                ControleCAD.DrawLine(mPen, pixelX(RetanguloSelecao[3].x), pixelY(RetanguloSelecao[3].y), pixelX(RetanguloSelecao[0].x), pixelY(RetanguloSelecao[0].y));
                                */
                //     mPen.DashStyle = DashStyle.Solid;
            };
        }
        [NonSerialized]
        public bool clickCtrl, clickShift;

        static double[] pixel_x = new double[1];
        static double[] pixel_y = new double[1];

        public static double RetPixelX_RetanguloSelecao(ref double x, ref double y, ref double z)
        {
            Project(ref pixel_x, ref pixel_y, ref x, ref y, ref z, ref Mvm_Selecao, ref pm_Selecao, ref ViewPortSelecao);

            return pixel_x[0];
        }

        public static double RetPixelY_RetanguloSelecao(ref double x, ref double y, ref double z)
        {
            Project(ref pixel_x, ref pixel_y, ref x, ref y, ref z, ref Mvm_Selecao, ref pm_Selecao, ref ViewPortSelecao);

            return (ViewPortSelecao[3] - pixel_y[0]);
        }
        bool PontoExiste(double x_, double y_, double z_)
        {
            foreach (TPonto l in NosSelecionados)
            {
                if ((Geom.Iguais(l.x, x_)) && (Geom.Iguais(l.y, y_) && (Geom.Iguais(l.z, z_))))
                    return true;
            }
            return false;
        }
        bool Dentro = false;
        Matrix4d MVP = new Matrix4d();
        Matrix4d MV = new Matrix4d();

        Vector3d p3;
        Vector4d pp3;
        Vector4d pClip = new Vector4d();
        Vector4d pClip2 = new Vector4d();
        Vector4d MVP_transformada = new Vector4d();

        vec3 pTela, v0, pZ, centroCamera;
        Plano plano_zNear;
        float[] modelview = new float[16];
        float[] projection = new float[16];
        float[] mvm = new float[16];

        void TestaSelecao(System.Windows.Forms.MouseEventArgs e)
        {
            try
            {

                if (e.Button == System.Windows.Forms.MouseButtons.Left)
                {
                    TBarraGenerica b1;
                    TLinha eixo;
                    double x1 = 0, y1 = 0, z1 = 0;
                    double x2 = 0, y2 = 0, z2 = 0;
                    PontoD a = new PontoD(0, 0, 0);
                    PontoD b = new PontoD(0, 0, 0);
                    PontoD c = new PontoD(0, 0, 0);
                    PontoD d = new PontoD(0, 0, 0);
                    PontoD intersec = new PontoD(0, 0, 0);
                    double z_pixel1 = 0;
                    double z_pixel2 = 0;
                    double z_pixel3 = 0;
                    double z_pixel4 = 0;
                    Vector3d p1 = new Vector3d(), p2 = new Vector3d();

                    bool selec = false;
                    intersec.x = 0;
                    intersec.y = 0;

                    RetanguloSelecao2[0].y = ViewPortSelecao[3] - RetanguloSelecao2[0].y;
                    RetanguloSelecao2[1].y = ViewPortSelecao[3] - RetanguloSelecao2[1].y;
                    RetanguloSelecao2[2].y = ViewPortSelecao[3] - RetanguloSelecao2[2].y;
                    RetanguloSelecao2[3].y = ViewPortSelecao[3] - RetanguloSelecao2[3].y;
                    
                    view = camera.viewMatrix;
                    proj = camera.Projecao;
                    View_x_Proj = RMath.Multiply(view, proj);

                    if (SelParcial)
                    {
                        TBarraGenerica barAtual;
                        for (int p = 0; p < Estrutura.barras.Count; p++)
                        {
                            Dentro = false;

                            if (Estrutura.barras[p] == null)
                                continue;

                            barAtual = Estrutura.barras[p];
                            
                            eixo = Estrutura.barras[p].Linha_Eixo;

                            if (eixo.auxiliar)
                                continue;

                            if (barAtual.Selecionado)
                                continue;

                            if (!Estrutura.barras[p].Visivel) continue;
                            /*  if (ObjetosNaViewport[p].TrechoViga != null)
                              {
                                  if (ObjetosNaViewport[p].LinhaEixoViga)
                                  {
                                      x1 = ObjetosNaViewport[p].TrechoViga.pIni.x;
                                      y1 = ObjetosNaViewport[p].TrechoViga.pIni.y;
                                      z1 = ObjetosNaViewport[p].TrechoViga.pIni.z;
                                      x2 = ObjetosNaViewport[p].TrechoViga.pFin.x;
                                      y2 = ObjetosNaViewport[p].TrechoViga.pFin.y;
                                      z2 = ObjetosNaViewport[p].TrechoViga.pFin.z;
                                  }
                                  else
                                      continue;
                              }
                              else*/
                            if (eixo.Barra != null)
                            {
                                /*if (ObjetosNaViewport[p].Barra.Tipo == Const.ID_PILAR)
                                {
                                    x1 = (ObjetosNaViewport[p].Barra as TPilar).LinhaEixo.pIni.x;
                                    y1 = (ObjetosNaViewport[p].Barra as TPilar).LinhaEixo.pIni.y;
                                    z1 = (ObjetosNaViewport[p].Barra as TPilar).LinhaEixo.pIni.z;
                                    x2 = (ObjetosNaViewport[p].Barra as TPilar).LinhaEixo.pFin.x;
                                    y2 = (ObjetosNaViewport[p].Barra as TPilar).LinhaEixo.pFin.y;
                                    z2 = (ObjetosNaViewport[p].Barra as TPilar).LinhaEixo.pFin.z;
                                }
                                else*/
                                if (eixo.Barra.Tipo == Const.ID_BARRAGENERICA)
                                {
                                    x1 = barAtual.pIni_Offset.x;
                                    y1 = -barAtual.pIni_Offset.y;
                                    z1 =- barAtual.pIni_Offset.z;
                                    x2 = barAtual.pFin_Offset.x;
                                    y2 = -barAtual.pFin_Offset.y;
                                    z2 = -barAtual.pFin_Offset.z;

                                    p1 = new Vector3d(x1, y1, z1);
                                    p2 = new Vector3d(x2, y2, z2);

                                }
                                else
                                    continue;
                            }

                            selec = false;
                            double zx1=0, zy1=0, zx2=0, zy2 = 0;

                            if (!Clipper3D.ProjetarLinhaSelecao(View_x_Proj, cameraPerspectiva, p1, p2, out zx1, out zy1, out zx2, out zy2))
                                continue;

                            a.x = zx1;
                            a.y = ViewPortSelecao[3] - zy1 ;
                            b.x = zx2;
                            b.y = ViewPortSelecao[3] - zy2;

                          /*  a.x = RetPixelX(ref x1, ref y1, ref z1, ref z_pixel1);
                            a.y = RetPixelY(ref x1, ref y1, ref z1, ref z_pixel2);
                            b.x = RetPixelX(ref x2, ref y2, ref z2, ref z_pixel3);
                            b.y = RetPixelY(ref x2, ref y2, ref z2, ref z_pixel4);*/

                            //      if ((z_pixel1 > 1 || z_pixel1 < 0) && z_pixel2 > 1 || z_pixel2 < 0 && z_pixel3 > 1 && z_pixel3 < 0 && z_pixel4 > 1 && z_pixel4 < 0)
                            //     continue;

                            c.x = RetanguloSelecao2[0].x;
                            c.y = RetanguloSelecao2[0].y;
                            d.x = RetanguloSelecao2[1].x;
                            d.y = RetanguloSelecao2[1].y;

                            if (Geom.calcIntersecEQU_RETA(ref a, ref b, ref c, ref d, ref intersec))
                                selec = true;

                            c.x = RetanguloSelecao2[0].x;
                            c.y = RetanguloSelecao2[0].y;
                            d.x = RetanguloSelecao2[3].x;
                            d.y = RetanguloSelecao2[3].y;

                            if (Geom.calcIntersecEQU_RETA(ref a, ref b, ref c, ref d, ref intersec))
                                selec = true;

                            c.x = RetanguloSelecao2[1].x;
                            c.y = RetanguloSelecao2[1].y;
                            d.x = RetanguloSelecao2[2].x;
                            d.y = RetanguloSelecao2[2].y;

                            if (Geom.calcIntersecEQU_RETA(ref a, ref b, ref c, ref d, ref intersec))
                                selec = true;

                            c.x = RetanguloSelecao2[2].x;
                            c.y = RetanguloSelecao2[2].y;
                            d.x = RetanguloSelecao2[3].x;
                            d.y = RetanguloSelecao2[3].y;

                            if (Geom.calcIntersecEQU_RETA(ref a, ref b, ref c, ref d, ref intersec))
                                selec = true;

                            if (PontoEmRetanguloSelecao(a.x, a.y) && PontoEmRetanguloSelecao(b.x, b.y))
                                selec = true;

                            if (PontoEmRetanguloSelecao(a.x, a.y) && gerenciador.SelecaoNos.Checked)
                                if (!PontoExiste(barAtual.pIni_Offset.x, barAtual.pIni_Offset.y, barAtual.pIni_Offset.z))
                                    NosSelecionados.Add(eixo.pIni as TPonto);

                            if (PontoEmRetanguloSelecao(b.x, b.y) && gerenciador.SelecaoNos.Checked)
                                if (!PontoExiste(barAtual.pFin_Offset.x, barAtual.pFin_Offset.y, barAtual.pFin_Offset.z))
                                    NosSelecionados.Add(eixo.pFin as TPonto);

                            if (selec)
                            {
                                if (eixo.Barra != null)
                                {
                                    if (eixo.Barra.Tipo == Const.ID_PILAR)
                                    {
                                        (eixo.Barra as TPilar).SetaSelecao(true, true);
                                    }
                                    if (eixo.Barra.Tipo == Const.ID_BARRAGENERICA && gerenciador.SelecaoBarras.Checked)
                                    {
                                        {
                                            (eixo.Barra as TBarraGenerica).SetaSelecao(true, true);
                                            ObjetosSelecionados.Add((eixo.Barra as TBarraGenerica));
                                        }
                                    }

                                }

                                else
                                {
                                    eixo.SetaSelecao(true, true);

                                    if (eixo.LinhaEixoViga && eixo.TrechoViga != null && !eixo.barraRigida)
                                    {
                                    }
                                }
                            }
                        }

                        foreach (TApoio o in Estrutura.apoios)
                        {
                            //pixel1(ref o.pIni.x, ref o.pIni.y, ref o.pIni.z, ref z_pixel1);
                            if (!o.Visivel)
                                continue;

                            a.x = RetPixelX(ref o.pIni.x, ref o.pIni.y, ref o.pIni.z, ref z_pixel1);
                            a.y = RetPixelY(ref o.pIni.x, ref o.pIni.y, ref o.pIni.z, ref z_pixel2);

                            //   if (z_pixel1 < 1 && z_pixel2 < 1)
                            if (PontoEmRetanguloSelecao(a.x, a.y) && gerenciador.SelecaoApoios.Checked)
                            {
                                o.SetaSelecao(true, true);
                                ObjetosSelecionados.Add((o as TApoio));
                            }
                        }

                        foreach (TPonto o in Estrutura.nos)
                        {
                            if (!o.Visivel) continue;

                            a.y = RetPixelY(ref o.x, ref o.y, ref o.z, ref z_pixel1);
                            a.x = RetPixelX(ref o.x, ref o.y, ref o.z, ref z_pixel2);

                            ForaDaTela = ((a.x < 0)) || ((a.x > w)) || ((a.y < 0)) || ((a.y > h));
                            if (ForaDaTela)
                              continue;

                            naTela1 = (z_pixel1 < 1 && z_pixel1 > 0);
                            naTela2 = (z_pixel2 < 1 && z_pixel2 > 0);

                            // ForaDaTela = !((z_clip1 < 1 && z_clip1 > 0) && (z_clip2 < 1 && z_clip2 > 0));
                            if (!naTela1 || !naTela2)
                                continue;

                            //   if (z_pixel1 < 1 && z_pixel2 < 1)
                            if (PontoEmRetanguloSelecao(a.x, a.y) && gerenciador.SelecaoNos.Checked)
                            {
                                o.SetaSelecao(true, true);
                                ObjetosSelecionados.Add((o as TPonto));
                            }
                        }

                        foreach (TCargaLinear o in Estrutura.cargaLinear)
                        {
                            selec = false;

                            if (!o.Visivel) continue;

                            if (o.Dados.idCaso == 1)
                                continue;
                            
                            if (o.Dados.distribuida)
                            {
                                p1 = new Vector3d(o.setas[o.qtdSetas].l_principal.p1.x, o.setas[o.qtdSetas].l_principal.p1.y, o.setas[o.qtdSetas].l_principal.p1.z);
                                p2 = new Vector3d(o.setas[0].l_principal.p1.x, o.setas[0].l_principal.p1.y, o.setas[0].l_principal.p1.z);
                                
                                double zx1 = 0, zy1 = 0, zx2 = 0, zy2 = 0;

                                if (!Clipper3D.ProjetarLinhaSelecao(View_x_Proj, cameraPerspectiva, p1, p2, out zx1, out zy1, out zx2, out zy2))
                                    continue;

                                a.x = zx1;//RetPixelX(ref o.setas[o.qtdSetas].l_principal.p1.x, ref o.setas[o.qtdSetas].l_principal.p1.y, ref o.setas[o.qtdSetas].l_principal.p1.z, ref z_pixel1);
                                a.y = ViewPortSelecao[3] - zy1;//RetPixelY(ref o.setas[o.qtdSetas].l_principal.p1.x, ref o.setas[o.qtdSetas].l_principal.p1.y, ref o.setas[o.qtdSetas].l_principal.p1.z, ref z_pixel2);
                                b.x = zx2;//RetPixelX(ref o.setas[0].l_principal.p1.x, ref o.setas[0].l_principal.p1.y, ref o.setas[0].l_principal.p1.z, ref z_pixel3);
                                b.y = ViewPortSelecao[3] - zy2;//RetPixelY(ref o.setas[0].l_principal.p1.x, ref o.setas[0].l_principal.p1.y, ref o.setas[0].l_principal.p1.z, ref z_pixel4);
                            }
                            else
                            if (o.Dados.concentrada)
                            {
                                p1 = new Vector3d(o.setas[0].l_principal.p1.x, o.setas[0].l_principal.p1.y, o.setas[0].l_principal.p1.z);
                                p2 = new Vector3d(o.setas[0].l_principal.p2.x, o.setas[0].l_principal.p2.y, o.setas[0].l_principal.p2.z);

                                double zx1 = 0, zy1 = 0, zx2 = 0, zy2 = 0;

                                if (!Clipper3D.ProjetarLinhaSelecao(View_x_Proj, cameraPerspectiva, p1, p2, out zx1, out zy1, out zx2, out zy2))
                                    continue;

                                a.x = zx1;
                                a.y = ViewPortSelecao[3] - zy1;
                                b.x = zx2;
                                b.y = ViewPortSelecao[3] - zy2;
                                
                                //a.x = RetPixelX(ref o.setas[0].l_principal.p1.x, ref o.setas[0].l_principal.p1.y, ref o.setas[0].l_principal.p1.z, ref z_pixel1);
                                //a.y = RetPixelY(ref o.setas[0].l_principal.p1.x, ref o.setas[0].l_principal.p1.y, ref o.setas[0].l_principal.p1.z, ref z_pixel2);
                               // b.x = RetPixelX(ref o.setas[0].l_principal.p2.x, ref o.setas[0].l_principal.p2.y, ref o.setas[0].l_principal.p2.z, ref z_pixel3);
                                //b.y = RetPixelY(ref o.setas[0].l_principal.p2.x, ref o.setas[0].l_principal.p2.y, ref o.setas[0].l_principal.p2.z, ref z_pixel4);
                            }
                            
                           /* ForaDaTela = ((a.x < 0) && (b.x < 0)) ||
                            ((a.x > w) && (b.x > w)) ||
                            ((a.y < 0) && (b.y < 0)) ||
                            ((a.y > h) && (b.y > h));
                            if (ForaDaTela)
                                continue;

                            ForaDaTela = !(z_pixel1 < 1 && z_pixel1 > 0 && z_pixel2 < 1 && z_pixel2 > 0);

                            if (ForaDaTela)
                                continue;*/

                            c.x = RetanguloSelecao2[0].x;
                            c.y = RetanguloSelecao2[0].y;
                            d.x = RetanguloSelecao2[1].x;
                            d.y = RetanguloSelecao2[1].y;

                            if (Geom.calcIntersecEQU_RETA(ref a, ref b, ref c, ref d, ref intersec))
                                selec = true;

                            c.x = RetanguloSelecao2[0].x;
                            c.y = RetanguloSelecao2[0].y;
                            d.x = RetanguloSelecao2[3].x;
                            d.y = RetanguloSelecao2[3].y;

                            if (Geom.calcIntersecEQU_RETA(ref a, ref b, ref c, ref d, ref intersec))
                                selec = true;

                            c.x = RetanguloSelecao2[1].x;
                            c.y = RetanguloSelecao2[1].y;
                            d.x = RetanguloSelecao2[2].x;
                            d.y = RetanguloSelecao2[2].y;

                            if (Geom.calcIntersecEQU_RETA(ref a, ref b, ref c, ref d, ref intersec))
                                selec = true;

                            c.x = RetanguloSelecao2[2].x;
                            c.y = RetanguloSelecao2[2].y;
                            d.x = RetanguloSelecao2[3].x;
                            d.y = RetanguloSelecao2[3].y;

                            if (Geom.calcIntersecEQU_RETA(ref a, ref b, ref c, ref d, ref intersec))
                                selec = true;

                            if (PontoEmRetanguloSelecao(a.x, a.y) && PontoEmRetanguloSelecao(b.x, b.y))
                                selec = true;

                            if (selec)
                            {
                                o.SetaSelecao(true, true);
                                ObjetosSelecionados.Add((o as TCargaLinear));
                            }
                        }

                        foreach (TCargaPontual o in Estrutura.cargaPontual)
                        {
                            selec = false;
 
                            if (!o.Visivel) continue;

                            if ((o as TCargaPontual).Dados.idCaso == 1)
                              continue;
                            
                            if ((o as TCargaPontual).Dados.tipoForca)
                            {

                                p1 = new Vector3d(o.setas[0].l_principal.p1.x, o.setas[0].l_principal.p1.y, o.setas[0].l_principal.p1.z);
                                p2 = new Vector3d(o.setas[0].l_principal.p2.x, o.setas[0].l_principal.p2.y, o.setas[0].l_principal.p2.z);

                                double zx1 = 0, zy1 = 0, zx2 = 0, zy2 = 0;

                                if (!Clipper3D.ProjetarLinhaSelecao(View_x_Proj, cameraPerspectiva, p1, p2, out zx1, out zy1, out zx2, out zy2))
                                    continue;

                                a.x = zx1;
                                a.y = ViewPortSelecao[3] - zy1;
                                b.x = zx2;
                                b.y = ViewPortSelecao[3] - zy2;

                               // a.x = RetPixelX(ref o.setas[0].l_principal.p1.x, ref o.setas[0].l_principal.p1.y, ref o.setas[0].l_principal.p1.z, ref z_pixel1);
                               // a.y = RetPixelY(ref o.setas[0].l_principal.p1.x, ref o.setas[0].l_principal.p1.y, ref o.setas[0].l_principal.p1.z, ref z_pixel2);
                              //  b.x = RetPixelX(ref o.setas[0].l_principal.p2.x, ref o.setas[0].l_principal.p2.y, ref o.setas[0].l_principal.p2.z, ref z_pixel3);
                              //  b.y = RetPixelY(ref o.setas[0].l_principal.p2.x, ref o.setas[0].l_principal.p2.y, ref o.setas[0].l_principal.p2.z, ref z_pixel4);
                            }
                            else
                            {
                                a.x = RetPixelX(ref o.setasMomento[0].l_principal.p1.x, ref o.setasMomento[0].l_principal.p1.y, ref o.setasMomento[0].l_principal.p1.z, ref z_pixel1);
                                a.y = RetPixelY(ref o.setasMomento[0].l_principal.p1.x, ref o.setasMomento[0].l_principal.p1.y, ref o.setasMomento[0].l_principal.p1.z, ref z_pixel2);
                                b.x = RetPixelX(ref o.setasMomento[0].l_principal.p2.x, ref o.setasMomento[0].l_principal.p2.y, ref o.setasMomento[0].l_principal.p2.z, ref z_pixel3);
                                b.y = RetPixelY(ref o.setasMomento[0].l_principal.p2.x, ref o.setasMomento[0].l_principal.p2.y, ref o.setasMomento[0].l_principal.p2.z, ref z_pixel4);
                            }

                               c.x = RetanguloSelecao2[0].x;
                               c.y = RetanguloSelecao2[0].y;
                               d.x = RetanguloSelecao2[1].x;
                               d.y = RetanguloSelecao2[1].y;

                               if (Geom.calcIntersecEQU_RETA(ref a, ref b, ref c, ref d, ref intersec))
                                   selec = true;

                               c.x = RetanguloSelecao2[0].x;
                               c.y = RetanguloSelecao2[0].y;
                               d.x = RetanguloSelecao2[3].x;
                               d.y = RetanguloSelecao2[3].y;

                               if (Geom.calcIntersecEQU_RETA(ref a, ref b, ref c, ref d, ref intersec))
                                   selec = true;

                               c.x = RetanguloSelecao2[1].x;
                               c.y = RetanguloSelecao2[1].y;
                               d.x = RetanguloSelecao2[2].x;
                               d.y = RetanguloSelecao2[2].y;

                               if (Geom.calcIntersecEQU_RETA(ref a, ref b, ref c, ref d, ref intersec))
                                   selec = true;

                               c.x = RetanguloSelecao2[2].x;
                               c.y = RetanguloSelecao2[2].y;
                               d.x = RetanguloSelecao2[3].x;
                               d.y = RetanguloSelecao2[3].y;

                               if (Geom.calcIntersecEQU_RETA(ref a, ref b, ref c, ref d, ref intersec))
                                   selec = true;

                               if (PontoEmRetanguloSelecao(a.x, a.y) && PontoEmRetanguloSelecao(b.x, b.y))
                                   selec = true;

                               if (selec)
                               {
                                   o.SetaSelecao(true, true);
                                   ObjetosSelecionados.Add((o as TCargaPontual));
                               }
                        }
                    }
                    else
                    if (SelIntegral)
                    {
                        TBarraGenerica barAtual;
                        for (int p = 0; p < Estrutura.barras.Count; p++)
                        {      
                            eixo = Estrutura.barras[p].Linha_Eixo;

                            if (!Estrutura.barras[p].Visivel) continue;

                            barAtual = Estrutura.barras[p];

                            if (barAtual.Selecionado) continue;

                            if (eixo.TrechoViga != null)
                            {
                                if (eixo.LinhaEixoViga)
                                {
                                    x1 = eixo.TrechoViga.pIni.x;
                                    y1 = eixo.TrechoViga.pIni.y;
                                    z1 = eixo.TrechoViga.pIni.z;
                                    x2 = eixo.TrechoViga.pFin.x;
                                    y2 = eixo.TrechoViga.pFin.y;
                                    z2 = eixo.TrechoViga.pFin.z;
                                }
                                else continue;
                            }
                            else
                            if (eixo.Barra != null)
                            {
                                if (eixo.Barra.Tipo == Const.ID_PILAR)
                                {
                                    x1 = (eixo.Barra as TPilar).LinhaEixo.pIni.x;
                                    y1 = (eixo.Barra as TPilar).LinhaEixo.pIni.y;
                                    z1 = (eixo.Barra as TPilar).LinhaEixo.pIni.z;
                                    x2 = (eixo.Barra as TPilar).LinhaEixo.pFin.x;
                                    y2 = (eixo.Barra as TPilar).LinhaEixo.pFin.y;
                                    z2 = (eixo.Barra as TPilar).LinhaEixo.pFin.z;
                                }
                                else
                                if (eixo.Barra.Tipo == Const.ID_BARRAGENERICA)
                                {
                                    x1 = barAtual.pIni_Offset.x;
                                    y1 = -barAtual.pIni_Offset.y;
                                    z1 = -barAtual.pIni_Offset.z;
                                    x2 = barAtual.pFin_Offset.x;
                                    y2 = -barAtual.pFin_Offset.y;
                                    z2 = -barAtual.pFin_Offset.z;
                                }
                            }
                            else
                                continue;

                            selec = false;

                            a.x = RetPixelX(ref x1, ref y1, ref z1, ref z_pixel1);
                            a.y = RetPixelY(ref x1, ref y1, ref z1, ref z_pixel2);
                            b.x = RetPixelX(ref x2, ref y2, ref z2, ref z_pixel3);
                            b.y = RetPixelY(ref x2, ref y2, ref z2, ref z_pixel4);

                            //    if (z_pixel1 > 1 && z_pixel2 > 1 && z_pixel3 > 1 && z_pixel4 > 1)
                            //       continue;

                            if (PontoEmRetanguloSelecao(a.x, a.y) && PontoEmRetanguloSelecao(b.x, b.y))
                                selec = true;

                            if (PontoEmRetanguloSelecao(a.x, a.y) && gerenciador.SelecaoNos.Checked)
                                if (!PontoExiste(barAtual.pIni_Offset.x, barAtual.pIni_Offset.y, barAtual.pIni_Offset.z))
                                    NosSelecionados.Add(eixo.pIni as TPonto);

                            if (PontoEmRetanguloSelecao(b.x, b.y) && gerenciador.SelecaoNos.Checked)
                                if (!PontoExiste(barAtual.pFin_Offset.x, barAtual.pFin_Offset.y, barAtual.pFin_Offset.z))
                                    NosSelecionados.Add(eixo.pFin as TPonto);

                            if (selec)
                            {
                                if (eixo.Barra != null)
                                {
                                    if (eixo.Barra.Tipo == Const.ID_PILAR)
                                    {
                                        (eixo.Barra as TPilar).SetaSelecao(true, true);
                                    }
                                    if (eixo.Barra.Tipo == Const.ID_BARRAGENERICA && gerenciador.SelecaoBarras.Checked)
                                    {
                                        (eixo.Barra as TBarraGenerica).SetaSelecao(true, true);

                                        ObjetosSelecionados.Add((eixo.Barra as TBarraGenerica));
                                    }
                                }
                                else
                                {
                                    if (eixo.LinhaEixoViga && eixo.TrechoViga != null && !eixo.barraRigida)
                                    {

                                    }
                                    eixo.SetaSelecao(true, true);
                                }
                            }
                        };

                        foreach (TCargaLinear o in Estrutura.cargaLinear)
                        {
                            selec = false;
                            
                            if (!o.Visivel) continue; 
                            
                            if ((o as TCargaLinear).Dados.idCaso == 1)
                                continue;
                            
 
                            if (o.Dados.distribuida)
                            {
                                a.x = RetPixelX(ref o.setas[o.qtdSetas].l_principal.p1.x, ref o.setas[o.qtdSetas].l_principal.p1.y, ref o.setas[o.qtdSetas].l_principal.p1.z, ref z_pixel1);
                                a.y = RetPixelY(ref o.setas[o.qtdSetas].l_principal.p1.x, ref o.setas[o.qtdSetas].l_principal.p1.y, ref o.setas[o.qtdSetas].l_principal.p1.z, ref z_pixel2);
                                b.x = RetPixelX(ref o.setas[0].l_principal.p1.x, ref o.setas[0].l_principal.p1.y, ref o.setas[0].l_principal.p1.z, ref z_pixel3);
                                b.y = RetPixelY(ref o.setas[0].l_principal.p1.x, ref o.setas[0].l_principal.p1.y, ref o.setas[0].l_principal.p1.z, ref z_pixel4);
                            }
                            else
                            if (o.Dados.concentrada)
                            {
                                a.x = RetPixelX(ref o.setas[0].l_principal.p1.x, ref o.setas[0].l_principal.p1.y, ref o.setas[0].l_principal.p1.z, ref z_pixel1);
                                a.y = RetPixelY(ref o.setas[0].l_principal.p1.x, ref o.setas[0].l_principal.p1.y, ref o.setas[0].l_principal.p1.z, ref z_pixel2);
                                b.x = RetPixelX(ref o.setas[0].l_principal.p2.x, ref o.setas[0].l_principal.p2.y, ref o.setas[0].l_principal.p2.z, ref z_pixel3);
                                b.y = RetPixelY(ref o.setas[0].l_principal.p2.x, ref o.setas[0].l_principal.p2.y, ref o.setas[0].l_principal.p2.z, ref z_pixel4);
                            }

                            if (PontoEmRetanguloSelecao(a.x, a.y) && PontoEmRetanguloSelecao(b.x, b.y))
                            {
                                o.SetaSelecao(true, true);
                                ObjetosSelecionados.Add((o as TCargaLinear));
                            }
                        }

                        foreach (TCargaPontual o in Estrutura.cargaPontual)
                        {
                            selec = false;

                            if (!o.Visivel) continue;

                            if ((o as TCargaPontual).Dados.idCaso == 1)
                                continue;

                            if ((o as TCargaPontual).Dados.tipoForca)
                            {
                                a.x = RetPixelX(ref o.setas[0].l_principal.p1.x, ref o.setas[0].l_principal.p1.y, ref o.setas[0].l_principal.p1.z, ref z_pixel1);
                                a.y = RetPixelY(ref o.setas[0].l_principal.p1.x, ref o.setas[0].l_principal.p1.y, ref o.setas[0].l_principal.p1.z, ref z_pixel2);
                                b.x = RetPixelX(ref o.setas[0].l_principal.p2.x, ref o.setas[0].l_principal.p2.y, ref o.setas[0].l_principal.p2.z, ref z_pixel3);
                                b.y = RetPixelY(ref o.setas[0].l_principal.p2.x, ref o.setas[0].l_principal.p2.y, ref o.setas[0].l_principal.p2.z, ref z_pixel4);
                            }
                            else
                            {
                                a.x = RetPixelX(ref o.setasMomento[0].l_principal.p1.x, ref o.setasMomento[0].l_principal.p1.y, ref o.setasMomento[0].l_principal.p1.z, ref z_pixel1);
                                a.y = RetPixelY(ref o.setasMomento[0].l_principal.p1.x, ref o.setasMomento[0].l_principal.p1.y, ref o.setasMomento[0].l_principal.p1.z, ref z_pixel2);
                                b.x = RetPixelX(ref o.setasMomento[0].l_principal.p2.x, ref o.setasMomento[0].l_principal.p2.y, ref o.setasMomento[0].l_principal.p2.z, ref z_pixel3);
                                b.y = RetPixelY(ref o.setasMomento[0].l_principal.p2.x, ref o.setasMomento[0].l_principal.p2.y, ref o.setasMomento[0].l_principal.p2.z, ref z_pixel4);
                            }

                            if (PontoEmRetanguloSelecao(a.x, a.y) && PontoEmRetanguloSelecao(b.x, b.y))
                            {
                                o.SetaSelecao(true, true);
                                ObjetosSelecionados.Add((o as TCargaPontual));
                            }
                        }

                        foreach (TGrip gr in Grips)
                            if (PontoEmRetanguloSelecao(gr.x, gr.y))
                                if (gr.ObjetoDesenho.Tipo == Const.ID_LAJE)
                                    gr.SetaSelecao(true, true);

                        foreach (TTexto t in Textos)
                            if (PontoEmRetanguloSelecao(t.x, t.y))
                                t.SetaSelecao(true, true);

                        foreach (TApoio o in Estrutura.apoios)
                        {
                            if (!o.Visivel) continue;
                            
                            a.x = RetPixelX(ref o.pIni.x, ref o.pIni.y, ref o.pIni.z, ref z_pixel1);
                            a.y = RetPixelY(ref o.pIni.x, ref o.pIni.y, ref o.pIni.z, ref z_pixel2);

                            //  if (z_pixel1 < 1 && z_pixel2 < 1)
                            if (PontoEmRetanguloSelecao(a.x, a.y) && gerenciador.SelecaoApoios.Checked)
                            {
                                o.SetaSelecao(true, true);
                                ObjetosSelecionados.Add((o as TApoio));
                            }
                        }

                        foreach (TPonto o in Estrutura.nos)
                        {
                            if (!o.Visivel) continue;
                            
                            a.x = RetPixelX(ref o.x, ref o.y, ref o.z, ref z_pixel1);
                            a.y = RetPixelY(ref o.x, ref o.y, ref o.z, ref z_pixel2);

                            //   if (z_pixel1 < 1 && z_pixel2 < 1)
                            if (PontoEmRetanguloSelecao(a.x, a.y) && gerenciador.SelecaoNos.Checked)
                            {
                                o.SetaSelecao(true, true);
                                ObjetosSelecionados.Add((o as TPonto));
                            }
                        }
                    }

                    RetanguloSelecao[3].x = 0;
                    RetanguloSelecao[3].y = 0;
                    RetanguloSelecao[0].x = 0;
                    RetanguloSelecao[0].y = 0;
                    RetanguloSelecao[1].x = 0;
                    RetanguloSelecao[1].y = 0;
                    RetanguloSelecao[2].x = 0;
                    RetanguloSelecao[2].y = 0;

                    RetanguloSelecao2[3].x = 0;
                    RetanguloSelecao2[3].y = 0;
                    RetanguloSelecao2[0].x = 0;
                    RetanguloSelecao2[0].y = 0;
                    RetanguloSelecao2[1].x = 0;
                    RetanguloSelecao2[1].y = 0;
                    RetanguloSelecao2[2].x = 0;
                    RetanguloSelecao2[2].y = 0;

                    for (int i = 0; i < ObjetosSelecionados.Count; i++)
                    { 
                        if (ObjetosSelecionados[i].Tipo == Const.ID_BARRAGENERICA)
                        {
                            TBarraGenerica bar = (TBarraGenerica)ObjetosSelecionados[i];
                            if (bar.ids_barras_rigidas != null)
                            {
                                for (int j = 0; j < bar.ids_barras_rigidas.Count; j++)
                                {
                                    if (Estrutura.barras.Exists(o => o.IDBarra == bar.ids_barras_rigidas[j]))
                                        Estrutura.barras.Find(o => o.IDBarra == bar.ids_barras_rigidas[j]).SetaSelecao(true,false);
                                }
                            }
                        }
                    }

                    AtualizaShaders(false);
                }
            }
            catch (Exception er)
            {
                MessageBox.Show("erro na seleção múltipla: -> " + er.Message);
            }
        }

        private void glControl_MouseUp(object sender, System.Windows.Forms.MouseEventArgs e)
        {
            Panning = false;
            rodando = false;

            if (e.Button == System.Windows.Forms.MouseButtons.Middle && !clickCtrl)
            {
                //     this.Cursor = System.Windows.Forms.Cursors.Cross;
                Panning = false;
            };
            if (e.Button == System.Windows.Forms.MouseButtons.Middle && clickCtrl)
            {
                if (rodando)
                {
                    menuPrincipal.Close();
                    rodando = false;
                }
            }
            
            if (e.Button == System.Windows.Forms.MouseButtons.Left)
            {
                if (tipoComando == eTipoComando.selecionar)
                {
                    if ((Ponto1.X == e.X) && (Ponto1.Y == e.Y))
                    {
                        CaixaSelecao = false;
                        ClicouObjeto();
                    }
                    else
                    if (CaixaSelecao)
                    {
                        CaixaSelecao = false;
                      //  AtualizaShaders();
                        TestaSelecao(e);

                        if (IdObjetoDesenho != "")
                        {
                            if (IdObjetoDesenho == Const.ID_APOIO)
                            {
                                SetaSelecionados(false, -1, false);
                                foreach (TObjetoDesenho o in NosSelecionados)
                                {
                                    MouseDrawing((o as TPonto).x, (o as TPonto).y, (o as TPonto).z);
                                    (ObjetoNovo as TApoio).InsercaoIndividual = false;
                                    MouseDrawing((o as TPonto).x, (o as TPonto).y, (o as TPonto).z);
                                }
                                IdObjetoDesenho = string.Empty;

                                if (gerenciador.fApoio != null)
                                    gerenciador.fApoio.Salvar();
                            }
                        }
                    }
                    //  if (GripAtual != null)
                    //      MouseGrip(mousepoint.x, mousepoint.y);
                    //   else
                    //        MouseSelecao(e);
                }
            }

            /*if (IdFerramentaEdicao == Const.ID_COPIA_PADRAO)
            {
                PreencheListaDeSelecionados();
                gerenciador.CopiaPadrao.lbQtdElementos.Text = "";

                if ((FerramentaEdicao as TCopiaPadrao).Comando == "selecao")
                {
                    if (ObjetosSelecionados.Count > 0)
                    {
                        (FerramentaEdicao as TCopiaPadrao).Objetos = new List<TObjetoDesenho>();
                        (FerramentaEdicao as TCopiaPadrao).ObjetosCopia = new List<TObjetoDesenho>();
                        foreach (TObjetoDesenho ob in ObjetosSelecionados)
                        {
                            gerenciador.CopiaPadrao.objetos.Add(ob.Clone());
                            (FerramentaEdicao as TCopiaPadrao).Objetos.Add(ob.Clone());
                            (FerramentaEdicao as TCopiaPadrao).ObjetosCopia.Add(ob.Clone());
                        }
                        gerenciador.CopiaPadrao.lbQtdElementos.Text = "<" + ObjetosSelecionados.Count + ">";
                        gerenciador.CopiaPadrao.AtualizaQuantidades();
                    }
                }
            }*/

            this.Cursor = Cursors.Arrow;
            this.glControl.Cursor = Cursors.Arrow;
        }
        Cursor Cur_rotacao;
        float start_x2,start_y2;
        vec3  pb, pd, Ponto_zNear, Ponto_HitObjeto, Ponto_zFar;


    //    public float x_ang_rad, z_ang_rad;
        double a_b;
        private void glControl_MouseMove(object sender, System.Windows.Forms.MouseEventArgs e)
        {
            if (!aguardandoCota)
            {
                mouseX = e.X;
                mouseY = e.Y;

                if (e.Button == System.Windows.Forms.MouseButtons.Middle && clickCtrl)
                {
                    //    AtualizaDisplayList_Nos();
                    ModelagemEmPlano = false;
                    rodando = true;

                    camera.x_rot_angle += (e.Y - start_y_rotacao) * .4f;
                    camera.y_rot_angle += (e.X - start_x_rotacao) * .4f;

                    if (camera.y_rot_angle < -360 || camera.y_rot_angle > 360)
                        camera.y_rot_angle = 0;

                    camera.x_ang_rad = (camera.x_rot_angle * Math.PI) / 180;
                    camera.z_ang_rad = (camera.y_rot_angle * Math.PI) / 180;

                    start_x_rotacao = e.X;
                    start_y_rotacao = e.Y;

                   /* camera.x_rot_angle = x_rot_angle;
                    camera.y_rot_angle = y_rot_angle;
                    camera.x_ang_rad = x_ang_rad;
                    camera.z_ang_rad = z_ang_rad;*/
                    camera.ViewDirty = true;
                }
                else
                if (e.Button == System.Windows.Forms.MouseButtons.Middle && !clickCtrl)
                {
                    if (Panning)
                    {
                      //  AtualizaDisplayList_Nos();
                        if (Ancorou_Panning)
                        {
                            raioZoom.GerarRaio3D(ref mouseX, ref mouseY,ref ViewPortPrincipal, ref viewMatrix_Panning, ref camera.Projecao);
                            raioZoom.CalculaIntersecao_Raio_x_Plano(ref PlanoPanning, ref Coord2_PlanoPanning);

                            if (CameraOrto)
                            {
                                camera.x_trans = camera.x_trans - (Coord2_PlanoPanning.x - start_x);
                                camera.y_trans = camera.y_trans - (Coord2_PlanoPanning.y - start_y);
                            }
                            else
                            {
                                camera.x_trans = camera.x_trans + (Coord2_PlanoPanning.x - start_x);
                                camera.y_trans = camera.y_trans + (Coord2_PlanoPanning.y - start_y);

                            }
                            start_x = Coord2_PlanoPanning.x;
                            start_y = Coord2_PlanoPanning.y;
                        }
                        else
                        {
                            if (CameraOrto)
                            {
                                if (fatorzoom_orto + 0.5f < -13)
                                {
                                    camera.x_trans = camera.x_trans + (mouseX - start_x) / 20f;
                                    camera.y_trans = camera.y_trans - (mouseY - start_y) / 20f;
                                }
                                else
                                if (fatorzoom_orto + 0.5f < -0.5f)
                                {
                                    camera.x_trans = camera.x_trans + (mouseX - start_x) / 40;
                                    camera.y_trans = camera.y_trans - (mouseY - start_y) / 40;
                                }
                                else
                                if (fatorzoom_orto < -0.03f)
                                {
                                    camera.x_trans = camera.x_trans + (mouseX - start_x) / 250f;
                                    camera.y_trans = camera.y_trans - (mouseY - start_y) / 250f;
                                }
                            }
                            else
                            {
                                if (camera.z_trans > 50)
                                {
                                    camera.x_trans = camera.x_trans + (mouseX - start_x) / 15;
                                    camera.y_trans = camera.y_trans - (mouseY - start_y) / 15;
                                }
                                else
                                {
                                    camera.x_trans = camera.x_trans + (mouseX - start_x) / 30;
                                    camera.y_trans = camera.y_trans - (mouseY - start_y) / 30;
                                }
                            }

                            start_x = mouseX;
                            start_y = mouseY;
                        }

                   //     camera.x_trans = x_trans;
                      //  camera.y_trans = y_trans;
                        camera.ViewDirty = true;
                    }
                }

                DesenhaObjetos();
                glControl.SwapBuffers();
            }

            if (ObjetoNovo != null)
            {
                ObjetoNovo.OnMouseMove(ref mousepoint, true);
                //  RepintarObjeto(ObjetoNovo);
            }
            else
            if (IdFerramentaEdicao != string.Empty)
            {
               if (IdFerramentaEdicao == Const.ID_COPIAR_ELEMENTOS && (FerramentaEdicao as TCopiarElementos).Comando == Const.COPIAR_COMANDO_3)
               {
                   (FerramentaEdicao as TCopiarElementos).OnMouseMove(ref mousepoint, true, true, gerenciador.AnguloRotacao);
               }
               else
               if (IdFerramentaEdicao == Const.ID_MOVER_ELEMENTOS && (FerramentaEdicao as TMoverElementos).Comando == Const.MOVER_COMANDO_3)
               {
                   (FerramentaEdicao as TMoverElementos).OnMouseMove(ref mousepoint, true, true, gerenciador.AnguloRotacao);
               }
               else
               if (IdFerramentaEdicao == Const.ID_MOVER_EXTREMO_ELEMENTOS && (FerramentaEdicao as TMoverExtremoElemento).Comando == Const.MOVER_EXTREMO_COMANDO_4)
               {
                   (FerramentaEdicao as TMoverExtremoElemento).OnMouseMove(ref mousepoint, true, true, gerenciador.AnguloRotacao);
               }
               else
               if (IdFerramentaEdicao == Const.ID_ROTACIONAR_ELEMENTOS && (FerramentaEdicao as TRotacionarElementos).Comando == Const.ROTACIONAR_COMANDO_3)
               {
                   (FerramentaEdicao as TRotacionarElementos).OnMouseMove(ref mousepoint, true, true, gerenciador.AnguloRotacao);
               }
               else
               if (IdFerramentaEdicao == Const.ID_ESPELHAR_ELEMENTOS && ((FerramentaEdicao as TEspelharElementos).Comando == Const.ESPELHAR_COMANDO_3
               || (FerramentaEdicao as TEspelharElementos).Comando == Const.ESPELHAR_COMANDO_4))
                {
                    (FerramentaEdicao as TEspelharElementos).OnMouseMove(ref mousepoint, true, true, gerenciador.AnguloRotacao);
               }
            }
        }
        bool emObjeto;
        private void glControl_MouseDoubleClick(object sender, System.Windows.Forms.MouseEventArgs e)
        {
            if (e.Button == System.Windows.Forms.MouseButtons.Left)
            {
                emObjeto = ClicouObjeto();
                if ( emObjeto && tipoComando == eTipoComando.selecionar && ObjetoSelecionado != null)
                    ChamaEditObjeto(ObjetoSelecionado.Tipo);
            }
            else
            if (e.Button == System.Windows.Forms.MouseButtons.Middle)
            {
            }
        }
        bool Ancorou_Panning;
        Vector3d v1, v2, v3, v4;
        Matrix4d VP;
        int IntersecaoEmObjeto_Perspectiva(ref Triangulo triangulo, ref vec3 I, ref int mx, ref int my)
        {
            //raioZoom.GerarRaio3D(ref mx, ref my, ref ViewPortPrincipal, ref viewMatrix, ref Projecao_Panning);
            if (raioZoom.CalculaIntersecao_Raio_x_Triangulo(ref triangulo, ref I))
            {
                pNDC = new Vector4d(I.x, I.y, I.z, 1);


                //  VP = viewMatrix * Projecao_Panning;
                // pNDC = pNDC * VP;

                pNDC = RMath.Multiply(pNDC, VP);

                //   pNDC = pNDC * viewMatrix * Projecao_Panning;

                pNDC /= pNDC.W;

                //        gerenciador.Text = "pview:  x:" + pview.X.ToString("n3") + " y:" + pview.Y.ToString("n3") + "  z:"+ pview.Z.ToString("n3") ;
                if (pNDC.Z > 0 && pNDC.Z < 1) //se está na frente da tela
                    return triangulo.idBarra;
            }
            return -1;
        }

        int IntersecaoEmObjeto_Ortografica(ref Triangulo triangulo, ref vec3 I)
        {
            //  raioZoom.GerarRaio3D(ref mouseX, ref mouseY, ref ViewPortPrincipal, ref viewMatrix, ref Projecao);
            if (raioZoom.CalculaIntersecao_Raio_x_Triangulo(ref triangulo, ref I))
            {
                return triangulo.idBarra;
            }

            return -1;
        }
        List<int> ids_barras = new List<int>();
        List<Triangulo> triangulos_temporarios = new List<Triangulo>();
        void IntersecaoAABB(bool soApoios = false)
        {
            if (CameraOrto)
                raioZoom.GerarRaio3D(ref mouseX, ref mouseY, ref ViewPortPrincipal, ref camera.viewMatrix, ref camera.Projecao);
            else
                raioZoom.GerarRaio3D(ref mouseX, ref mouseY, ref ViewPortPrincipal, ref camera.viewMatrix, ref camera.Projecao_zNear_zero);

            //            if (!Unifilar)
            {
                ids_barras.Clear();
                Geom.InterceptaAABB(raioZoom, ref Estrutura.barras, ref ids_barras);
            }

            dist_zNear_objeto = 999999;

            triangulos_temporarios.Clear();

            if (!soApoios)
            {
                for (i = 0; i < ids_barras.Count; i++)
                {
                    triangulos_temp = TriangulosSelecao.Where(o => o.idBarra == ids_barras[i]).ToArray();
                    triangulos_temporarios.AddRange(triangulos_temp);

                    //for (j = 0; j < triangulos_temp.Count(); j++)
                    //   triangulos_temporarios.Add(triangulos_temp[j]);
                }
            }

            for (i = 0; i < Estrutura.apoios.Count; i++)
            {
                triangulos_temp = TriangulosSelecao.Where(o => o.idApoio == Estrutura.apoios[i].IDApoio).ToArray();
                for (j = 0; j < triangulos_temp.Count(); j++)
                    triangulos_temporarios.Add(triangulos_temp[j]);
            }

            triangulos_temp = triangulos_temporarios.ToArray();
        }

        void IntersecaoAABB_Unifilar(bool soApoios = false)
        {
            if (CameraOrto)
                raioZoom.GerarRaio3D(ref mouseX, ref mouseY, ref ViewPortPrincipal, ref camera.viewMatrix, ref camera.Projecao);
            else
                raioZoom.GerarRaio3D(ref mouseX, ref mouseY, ref ViewPortPrincipal, ref camera.viewMatrix, ref camera.Projecao_zNear_zero);

            triangulos_temporarios.Clear();

            for (i = 0; i < Estrutura.apoios.Count; i++)
            {
                triangulos_temp = TriangulosSelecao.Where(o => o.idApoio == Estrutura.apoios[i].IDApoio).ToArray();
                for (j = 0; j < triangulos_temp.Count(); j++)
                    triangulos_temporarios.Add(triangulos_temp[j]);
            }

            triangulos_temp = triangulos_temporarios.ToArray();
        }

        private void glControl_MouseDown(object sender, System.Windows.Forms.MouseEventArgs e)
        {
            if (cubo.FaceClique.Length > 0)
                return;

            this.Focus();
            if (e.Button == System.Windows.Forms.MouseButtons.Middle)
            {
                Cursor.Show();
                // if (!clickShift)
                {
                    this.Cursor = System.Windows.Forms.Cursors.Hand;
                    Panning = true;
                    Ancorou_Panning = false;

                    if (!CameraOrto)
                    {
                        IntersecaoAABB();

                        Ponto_zNear = new vec3(raioZoom.p0.x, raioZoom.p0.y, raioZoom.p0.z);
                        Ponto_zFar = new vec3(raioZoom.p1.x, raioZoom.p1.y, raioZoom.p1.z);
                        dist_zNear_objeto = 999999;
                        VP = RMath.Multiply(camera.viewMatrix, camera.Projecao_zNear_zero/*Projecao_Panning*/);
                        for (i = 0; i < triangulos_temp.Count(); i++)
                        {
                            trianguloRef = triangulos_temp[i];

                            if (IntersecaoEmObjeto_Perspectiva(ref trianguloRef, ref Coord_TrianguloSelecao, ref mouseX, ref mouseY) != -1)
                            {
                                distancia_raio_triangulo = (Ponto_zNear - Coord_TrianguloSelecao).Magnitude();
                                if (distancia_raio_triangulo < dist_zNear_objeto)
                                {
                                    Ponto_HitObjeto = new vec3(Coord_TrianguloSelecao.x, Coord_TrianguloSelecao.y, Coord_TrianguloSelecao.z);
                                    dist_zNear_objeto = distancia_raio_triangulo;
                                    Ancorou_Panning = true;
                                }
                            }
                        }

                        if (Ancorou_Panning)
                        {
                            /*raio partindo do meio da tela*/
                            int meio_x = (int)(w / 2);
                            int meio_y = (int)(h / 2);

                            if (CameraOrto) 
                                raioZoom.GerarRaio3D(ref meio_x, ref meio_y, ref ViewPortPrincipal, ref camera.viewMatrix, ref camera.Projecao);
                            else 
                                raioZoom.GerarRaio3D(ref meio_x, ref meio_y, ref ViewPortPrincipal, ref camera.viewMatrix, ref camera.Projecao_zNear_zero /*Projecao_Panning*/);

                            v1 = new Vector3d(raioZoom.p0.x, raioZoom.p0.y, raioZoom.p0.z);
                            v2 = new Vector3d(raioZoom.p1.x, raioZoom.p1.y, raioZoom.p1.z);
                            /******************************/

                            v3 = new Vector3d(Ponto_zNear.x, Ponto_zNear.y, Ponto_zNear.z);
                            v4 = new Vector3d(Ponto_HitObjeto.x, Ponto_HitObjeto.y, Ponto_HitObjeto.z);

                            double angulo_entre = Vector3d.CalculateAngle(v2 - v1, v4 - v3);
                            double distancia_znear_ate_plano = (Math.Cos(angulo_entre) * dist_zNear_objeto);

                            vec3 normalPlano = new vec3(0, 0, 1);
                            PlanoPanning = new Plano(normalPlano, new vec3(0, 0, -distancia_znear_ate_plano), "xy", false);
                            PlanoPanning.xmin = -2; PlanoPanning.ymin = -2; PlanoPanning.xmax = 2; PlanoPanning.ymax = 2;

                            viewMatrix_Panning = Matrix4d.CreateTranslation(0, 0, 0);
                            if (CameraOrto)
                                raioZoom.GerarRaio3D(ref mouseX, ref mouseY, ref ViewPortPrincipal, ref viewMatrix_Panning, ref camera.Projecao);
                            else
                                raioZoom.GerarRaio3D(ref mouseX, ref mouseY, ref ViewPortPrincipal, ref viewMatrix_Panning, ref camera.Projecao_zNear_zero/*Projecao_Panning*/);

                            raioZoom.CalculaIntersecao_Raio_x_Plano(ref PlanoPanning, ref Coord2_PlanoPanning);
                            start_x = Coord2_PlanoPanning.x;
                            start_y = Coord2_PlanoPanning.y;
                        }
                        else
                        {
                            start_x = e.X;
                            start_y = e.Y;
                        }
                    }
                    else
                    {
                        Ponto_zNear = new vec3(raioZoom.p0.x, raioZoom.p0.y, raioZoom.p0.z);
                        Ancorou_Panning = true; // camera ortografica sempre tem que ancorar
                        double distancia_znear_ate_plano = 1;

                        vec3 normalPlano = new vec3(0, 0, 1);
                        PlanoPanning = new Plano(normalPlano, new vec3(0, 0, -distancia_znear_ate_plano), "xy", false);
                        PlanoPanning.xmin = -2; PlanoPanning.ymin = -2; PlanoPanning.xmax = 2; PlanoPanning.ymax = 2;

                        viewMatrix_Panning = Matrix4d.CreateTranslation(0, 0, 0);
                        raioZoom.GerarRaio3D(ref mouseX, ref mouseY, ref ViewPortPrincipal, ref viewMatrix_Panning, ref camera.Projecao);

                        raioZoom.CalculaIntersecao_Raio_x_Plano(ref PlanoPanning, ref Coord2_PlanoPanning);
                        start_x = Coord2_PlanoPanning.x;
                        start_y = Coord2_PlanoPanning.y;
                    }

                    if (clickCtrl)
                    {
                        CalculaPivoRotacao();

                        start_x_rotacao = e.X;
                        start_y_rotacao = e.Y;

                        Cur_rotacao = new Cursor("rotate.ico");
                        this.Cursor = Cur_rotacao;
                    }
                }

                Ponto1.X = Posicao.X;
                Ponto1.Y = Posicao.Y;
            }
            

            this.Focus();
            if (e.Button != System.Windows.Forms.MouseButtons.Middle)
            {
                if (e.Button == System.Windows.Forms.MouseButtons.Right)
                {
                    if (ObjetoNovo != null)
                    {
                        string msg = string.Empty;

                        List<TPonto> points = new List<TPonto>();

                        ObjetoNovo.Cancel(true, ref msg, ref points);

                        foreach (TPonto pt1 in points)
                            foreach (TPonto pt2 in Pontos)
                                if ((object)pt1 == (object)pt2)
                                    if (pt1.incidencias.Count == 0)
                                        pt2.Selecionado = true;
                        Pontos.RemoveAll(obj => obj.Selecionado);

                        ComandoTexto(msg, "", false);
                        ObjetoNovo = null;

                        if (IdObjetoDesenho == Const.ID_TRECHOVIGA)
                            NovosDadosViga(true);

                        if (IdObjetoDesenho == Const.ID_BARRAGENERICA)
                            Comando.Text = "Novo elemento - " + DadosBarra.secao.descricao + " - Selecione o primeiro ponto";

                        if (IdObjetoDesenho != "")
                            this.ContextMenuStrip = null;

                        LinhasProlongamento.Clear();
                        AtualizaListaSnap();

                    //    RemoveSelecionados();
                    }

                    btCoordRelativa.ImageIndex = 0;
                    InsereCoordRelativa = false;
                    //     lbCoordenadas.Text = "Coordenadas absolutas";
                    pnDivBarras.Visible = false;
                }
                else
                {
                    if (tipoComando == eTipoComando.draw)
                        MouseDrawing(mousepoint.x, mousepoint.y, mousepoint.z);
                    else
                    if (tipoComando == eTipoComando.edit)
                        MouseEdit(mousepoint.x, mousepoint.y, mousepoint.z, 0, "", 0);
                    else
                    if (tipoComando == eTipoComando.selecionar)
                    {
                   //     if (GripAtual != null)
                     //       MouseGrip(mousepoint.x, mousepoint.y);
                     //   else
                            MouseSelecao(e);
                    }
                }
            }
            else
            if (e.Button == System.Windows.Forms.MouseButtons.Middle && !clickCtrl)
            {
                Cursor.Show();
                this.Cursor = System.Windows.Forms.Cursors.NoMove2D;
                Ponto1.X = Posicao.X;
                Ponto1.Y = Posicao.Y;
                Panning = true;

              //  start_x = e.X;
             //   start_y = e.Y;
            };

            if (IdObjetoDesenho != null && IdObjetoDesenho != string.Empty && e.Button == System.Windows.Forms.MouseButtons.Right)
            {
                //  this.ContextMenuStrip = null;
                MultiMouseDraw();
            }
            if (ObjetosSelecionados.Count > 0 && (FerramentaEdicao != null && !pnDivBarras.Visible && Comando.Text.Contains("direito do mouse")) && e.Button == System.Windows.Forms.MouseButtons.Right)
            {
                tipoComando = eTipoComando.edit;
                MouseEdit(0, 0, 0, 0, "", 0);
            }
        }

        void CalculaPivoRotacao()
        {
            try
            {
                dist_zNear_objeto = 999999;
                bool hitTriangulo = false;
                bool hitVazio = false;
                double hitX = 0, hitY = 0, hitZ = 0;
                int houveIntersecao = 0;
                Triangulo triangulo_hit = null;
                if (CameraOrto)
                {
                    IntersecaoAABB();

                    for (i = 0; i < triangulos_temp.Count(); i++)
                    {
                        trianguloRef = triangulos_temp[i];

                        if (IntersecaoEmObjeto_Ortografica(ref trianguloRef, ref Coord_TrianguloSelecao) != -1)
                        {
                            distancia_raio_triangulo = (raioZoom.p0 - Coord_TrianguloSelecao).Magnitude();

                            if (distancia_raio_triangulo < dist_zNear_objeto)
                            {
                                hitX = Coord_TrianguloSelecao.x;
                                hitY = Coord_TrianguloSelecao.y;
                                hitZ = Coord_TrianguloSelecao.z;
                                triangulo_hit = trianguloRef;
                                Ponto_HitObjeto = new vec3(Coord_TrianguloSelecao.x, Coord_TrianguloSelecao.y, Coord_TrianguloSelecao.z);
                                dist_zNear_objeto = distancia_raio_triangulo;
                                hitTriangulo = true;
                            }
                        }
                    }
                }
                else
                {
                    IntersecaoAABB();

                    dist_zNear_objeto = 999999;

                    // triangulos_temp = TriangulosSelecao.Where(o => o.idBarra == trianguloRef.idBarra).ToArray();
                    //   trianguloRef = null;
                    VP = RMath.Multiply(camera.viewMatrix, camera.Projecao_zNear_zero);
                    for (i = 0; i < triangulos_temp.Count(); i++)
                    {
                        trianguloRef = triangulos_temp[i];

                        if (IntersecaoEmObjeto_Perspectiva(ref trianguloRef, ref Coord_TrianguloSelecao, ref mouseX, ref mouseY) != -1)
                        {
                            distancia_raio_triangulo = (raioZoom.p0 - Coord_TrianguloSelecao).Magnitude();

                            if (distancia_raio_triangulo < dist_zNear_objeto)
                            {
                                hitX = Coord_TrianguloSelecao.x;
                                hitY = Coord_TrianguloSelecao.y;
                                hitZ = Coord_TrianguloSelecao.z;
                                triangulo_hit = trianguloRef;
                                Ponto_HitObjeto = new vec3(Coord_TrianguloSelecao.x, Coord_TrianguloSelecao.y, Coord_TrianguloSelecao.z);
                                dist_zNear_objeto = distancia_raio_triangulo;
                                hitTriangulo = true;
                            }
                        }
                    }
                }

                if (hitTriangulo/* || hitVazio*/)
                {
                    camera.x_trans = 0;
                    camera.y_trans = 0;
                    camera.pivoX = hitX;
                    camera.pivoY = hitY;
                    camera.pivoZ = hitZ;
                    
                   /* camera.pivoX = pivoX;
                    camera.pivoY = pivoY;
                    camera.pivoZ = pivoZ;*/
                    camera.ViewDirty = true;

                    DesenhaObjetos();

                    //testar se o ponto que cliquei nao foi para "tras" da camera, se foi, entao eu translado em Z até o ponto voltar pra frente
                    pNDC = new Vector4d(hitX, hitY, hitZ, 1);
                 //   pNDC = pNDC * viewMatrix * Projecao;

                    pNDC = RMath.Multiply(pNDC, camera.viewMatrix);
                    pNDC = RMath.Multiply(pNDC, camera.Projecao_zNear_zero);

                    pNDC /= pNDC.W;
                    int parar = 15;
                    if (pNDC.Z > 1)
                    {
                        while (pNDC.Z > 1f && pNDC.Z > 0.00001)
                        {
                            parar--;
                            if (parar == 0)
                                break;
                            camera.z_trans += 2f;

                            camera.viewMatrix = Matrix4d.CreateTranslation(-camera.pivoX, -camera.pivoY, -camera.pivoZ) *
                                         ((Matrix4d.CreateRotationZ(-camera.z_ang_rad) * Matrix4d.CreateRotationX(camera.x_ang_rad)) *
                                          Matrix4d.CreateTranslation(0, 0, -camera.z_trans));

                            pNDC = new Vector4d(hitX, hitY, hitZ, 1);
                          //  pNDC = pNDC * viewMatrix * Projecao;

                            pNDC = RMath.Multiply(pNDC, camera.viewMatrix);
                            pNDC = RMath.Multiply(pNDC, camera.Projecao_zNear_zero);

                            pNDC /= pNDC.W;
                        }
                    }

                    /*raio partindo do meio da tela para testar a nova distancia entre raio.p0 e o ponto que cliquei no objeto
                     * o raio parte do meio da tela pq o ponto que cliquei no objeto já está no meio da tela. Foi feito isso através de  x_trans = 0, y_trans= 0, pivoX=..,pivoY=..,pivoZ=.. )*/
                    int meio_x = (int)(w / 2);
                    int meio_y = (int)(h / 2);
                    double z_ant = PlanoPanning.Posicao.z;

                    if (hitTriangulo)
                    {
                        if (CameraOrto)
                        {
                            dist_zNear_objeto = 999999;
                            for (i = 0; i < triangulos_temp.Count(); i++)
                            {
                                trianguloRef = triangulos_temp[i];

                                houveIntersecao = IntersecaoEmObjeto_Ortografica(ref trianguloRef, ref Coord_TrianguloSelecao);

                                //aqui tem que testar se o triangulo é o mesmo do triangulo que interceptou no primeiro clique, 
                                //pois o modelo sofreu uma translação (x_trans = 0, y_trans= 0, pivoX,pivoY,pivoZ )e esse raio pode interceptar outro objeto que agora apareceu na frente do raio devido a translação, mas obviamente o ponto inicial ainda continua no raio.
                                if ((houveIntersecao != -1 && ((object)trianguloRef == (object)triangulo_hit)))
                                {
                                    distancia_raio_triangulo = (Ponto_zNear - Coord_TrianguloSelecao).Magnitude();
                                    if (distancia_raio_triangulo < dist_zNear_objeto)
                                    {
                                        Ponto_HitObjeto = new vec3(Coord_TrianguloSelecao.x, Coord_TrianguloSelecao.y, Coord_TrianguloSelecao.z);
                                        dist_zNear_objeto = distancia_raio_triangulo;
                                    }
                                }
                            }
                        }
                        else
                        {
                            VP = RMath.Multiply(camera.viewMatrix, camera.Projecao_zNear_zero);
                            dist_zNear_objeto = 999999;
                            raioZoom.GerarRaio3D(ref mouseX, ref mouseY, ref ViewPortPrincipal, ref camera.viewMatrix, ref camera.Projecao_zNear_zero);
                            for (i = 0; i < triangulos_temp.Count(); i++)
                            {
                                trianguloRef = triangulos_temp[i];

                                houveIntersecao = IntersecaoEmObjeto_Perspectiva(ref trianguloRef, ref Coord_TrianguloSelecao, ref meio_x, ref meio_y);

                                //aqui tem que testar se o triangulo é o mesmo do triangulo que interceptou no primeiro clique, 
                                //pois o modelo sofreu uma translação (x_trans = 0, y_trans= 0, pivoX,pivoY,pivoZ )e esse raio pode interceptar outro objeto que agora apareceu na frente do raio devido a translação, mas obviamente o ponto inicial ainda continua no raio.
                                if ((houveIntersecao != -1 && ((object)trianguloRef == (object)triangulo_hit)))
                                {
                                    distancia_raio_triangulo = (Ponto_zNear - Coord_TrianguloSelecao).Magnitude();
                                    if (distancia_raio_triangulo < dist_zNear_objeto)
                                    {
                                        Ponto_HitObjeto = new vec3(Coord_TrianguloSelecao.x, Coord_TrianguloSelecao.y, Coord_TrianguloSelecao.z);
                                        dist_zNear_objeto = distancia_raio_triangulo;
                                    }
                                }
                            }
                        }
                    }
                    else
                    {
                        /*   if (CameraOrto)
                           {
                               dist_zNear_objeto = 999999;
                               distancia_raio_triangulo = (Ponto_zNear - Coord_TrianguloSelecao).Magnitude();
                               if (distancia_raio_triangulo < dist_zNear_objeto)
                               {
                                   Ponto_HitObjeto = new vec3(Coord_TrianguloSelecao.x, Coord_TrianguloSelecao.y, Coord_TrianguloSelecao.z);
                                   dist_zNear_objeto = distancia_raio_triangulo;
                               }
                           }
                           else
                           {
                               dist_zNear_objeto = 999999;
                               distancia_raio_triangulo = (Ponto_zNear - Coord_TrianguloSelecao).Magnitude();
                               if (distancia_raio_triangulo < dist_zNear_objeto)
                               {
                                   Ponto_HitObjeto = new vec3(Coord_TrianguloSelecao.x, Coord_TrianguloSelecao.y, Coord_TrianguloSelecao.z);
                                   dist_zNear_objeto = distancia_raio_triangulo;
                               }
                           }*/
                    }
                    /*ciar um plano paralelo/apontando para a tela na posição do clique */
                    double distancia_znear_ate_plano = Ponto_HitObjeto.DistanceTo(raioZoom.p0);// o raioZoom eu alimento na função "intersecaoemobjeto" ali em cima
                                                                                               //aqui nao precisa de trigonometria para achar o distancia_znear_ate_plano, pois o ponto vai ser sempre no meio da tela

                    vec3 normalPlano = new vec3(0, 0, 1);
                    PlanoPanning = new Plano(normalPlano, new vec3(0, 0, -distancia_znear_ate_plano), "xy", false);
                    PlanoPanning.xmin = -2; PlanoPanning.ymin = -2; PlanoPanning.xmax = 2; PlanoPanning.ymax = 2;

                    viewMatrix_Panning = Matrix4d.CreateTranslation(0, 0, 0);
                    if (CameraOrto)
                        raioZoom.GerarRaio3D(ref mouseX, ref mouseY, ref ViewPortPrincipal, ref viewMatrix_Panning, ref camera.Projecao);
                    else
                        raioZoom.GerarRaio3D(ref mouseX, ref mouseY, ref ViewPortPrincipal, ref viewMatrix_Panning, ref camera.Projecao_zNear_zero);

                    raioZoom.CalculaIntersecao_Raio_x_Plano(ref PlanoPanning, ref Coord2_PlanoPanning);
                    double novoZ = PlanoPanning.Posicao.z;

                    if (CameraOrto) 
                        Coord2_PlanoPanning *= -1;

                    //translado para a posição original
                    camera.x_trans += Coord2_PlanoPanning.x;
                    camera.y_trans += Coord2_PlanoPanning.y;

                    if (!CameraOrto)//se for ortografica, não precisa deslocar em z
                    {
                        //translado em z para a posição que estava, pois na funcão acima ocorre um deslocamento em z
                        modelViewMatrix_Zoom = Matrix4d.CreateTranslation(camera.x_trans, camera.y_trans, -camera.z_trans);
                        raioZoom.GerarRaio3D(ref mouseX, ref mouseY, ref ViewPortPrincipal, ref modelViewMatrix_Zoom, ref camera.Projecao_zNear_zero);
                        raioZoom.CalculaIntersecao_Raio_x_Plano(ref planoZoom, ref Coord_PlanoZoom);
                        xant = Coord_PlanoZoom.x;
                        yant = Coord_PlanoZoom.y;
                        camera.z_trans -= (z_ant - novoZ);

                        modelViewMatrix_Zoom = Matrix4d.CreateTranslation(camera.x_trans, camera.y_trans, -camera.z_trans);
                        raioZoom.GerarRaio3D(ref mouseX, ref mouseY, ref ViewPortPrincipal, ref modelViewMatrix_Zoom, ref camera.Projecao_zNear_zero);
                        raioZoom.CalculaIntersecao_Raio_x_Plano(ref planoZoom, ref Coord_PlanoZoom);

                        offset_x = (Coord_PlanoZoom.x - xant);
                        offset_y = (Coord_PlanoZoom.y - yant);

                        camera.x_trans += offset_x;
                        camera.y_trans += offset_y;
                        camera.ViewDirty = true;

                        DesenhaObjetos();
                    }
                }
                else
                {
                    //      Pivo_no_centro();
                }
            }
            catch (Exception er)
            {
                MessageBox.Show("erro ao calcular pivô para rotação: -> " + er.Message);
            }
        }

        float ult_centrox, ult_centroy, ult_centroz;
        public void IniciaOGL()
        {
            ogl.Inicializa(glControl.Width, glControl.Height);
            CriaPlanoTrabalho("xy", 0);
            this.Tag = "Principal";

            CriarShaders(ogl.vertex_shader, ogl.fragment_shader, out vertex_shader_object_triangulos, out fragment_shader_object_triangulos, out shader_triangulos_program);
            CriarShaders(ogl.vertex_shader_sem_iluminacao, ogl.fragment_shader_sem_iluminacao, out vertex_shader_object_arestas, out fragment_shader_object_arestas, out shader_arestas_program);
            CriarShaders(ogl.vertex_shader_com_transparencia, ogl.fragment_shader_com_transparencia, out vertex_shader_object_com_transparencia, out fragment_shader_object_com_transparencia, out shader_com_transparencia_program);
        }

        private void glControl_Load(object sender, EventArgs e)
        {
            try
            {
                IniciaOGL();
            }
            catch (Exception er)
            {
                MessageBox.Show(er.Message);
            }
        }

        private void glControl_MouseEnter(object sender, EventArgs e)
        {
            gerenciador.panelModo.Visible = false;
        }
        bool ModelagemEmPlano = false;
        public void VisualizarConformeGuizmo(ref string face)
        {
            int fatorX = 0;
            int fatorY = 0;

            double xT = 0, yT = 0;

            if (face != "")
            {
                ModelagemEmPlano = true;

                if (face == "top")
                {
                    xT = 180; yT = 180;

                    if (!CameraOrto)
                    {
                        xT = 180;
                        yT = 0;
                    }

                    fatorX = 1;
                    fatorY = -1;

                    if (camera.y_rot_angle < yT)
                        fatorY = 1;
                    else
                        fatorY = -1;

                    CriaPlanoTrabalho("xy", 0);
                }
                else
                if (face == "left")
                {
                    xT = -270; yT = 90;

                    if (camera.x_rot_angle < xT)
                        fatorX = 1;
                    else
                        fatorX = -1;

                    if (camera.y_rot_angle < yT)
                        fatorY = 1;
                    else
                        fatorY = -1;

                    CriaPlanoTrabalho("yz", 0);

                }
                else
                if (face == "botton")
                {
                    xT = -360; yT = -180;

                    if (camera.x_rot_angle < xT)
                        fatorX = 1;
                    else
                        fatorX = -1;

                    if (camera.y_rot_angle < yT)
                        fatorY = 1;
                    else
                        fatorY = -1;

                    CriaPlanoTrabalho("xy", 0);
                }
                else
                if (face == "right")
                {
                    xT = 90; yT = 270;

                    if (!CameraOrto)
                    {
                        xT = -270;
                        yT = -90;
                    }

                    if (camera.x_rot_angle < xT)
                        fatorX = 1;
                    else
                        fatorX = -1;

                    if (camera.y_rot_angle < yT)
                        fatorY = 1;
                    else
                        fatorY = -1;
                    CriaPlanoTrabalho("yz", 0);
                }
                else
                if (face == "front")
                {
                    xT = 90; yT = -180;

                    if (!CameraOrto)
                    {
                        xT = 90;
                        yT = 180;
                    }


                    if (camera.x_rot_angle < xT)
                        fatorX = 1;
                    else
                        fatorX = -1;

                    if (camera.y_rot_angle < yT)
                        fatorY = 1;
                    else
                        fatorY = -1;

                    CriaPlanoTrabalho("xz", 0);
                }
                else
                if (face == "back")
                {
                    xT = 90; yT = 0;

                    if (!CameraOrto)
                    {
                        xT = 90;
                        yT = 0;
                    }


                    if (camera.x_rot_angle < xT)
                        fatorX = 1;
                    else
                        fatorX = -1;

                    if (camera.y_rot_angle < yT)
                        fatorY = 1;
                    else
                        fatorY = -1;
                    CriaPlanoTrabalho("xz", 0);
                }

                double fracionada = FuncoesGerais.Frac(camera.x_rot_angle);
                double steps = Math.Abs((int)(camera.x_rot_angle - fracionada - xT));

                for (int ui = 0; ui < steps; ui++)
                {
                    camera.x_rot_angle += fatorX;
                    //    DesenhaObjetos();
                    //    glControl.SwapBuffers();
                }

                camera.x_rot_angle -= fracionada;

                fracionada = FuncoesGerais.Frac(camera.y_rot_angle);
                steps = Math.Abs((int)(camera.y_rot_angle - fracionada - yT));
                //  y_rot_angle += fatorY;
                for (int ui = 0; ui < steps; ui++)
                {
                    camera.y_rot_angle += fatorY;
                    //  DesenhaObjetos();
                    // glControl.SwapBuffers();
                }

                camera.y_rot_angle -= fracionada;

                camera.x_ang_rad = (camera.x_rot_angle * Math.PI) / 180;
                camera.z_ang_rad = (camera.y_rot_angle * Math.PI) / 180;

                /*camera.x_rot_angle = x_rot_angle;
                camera.y_rot_angle = y_rot_angle;
                camera.x_ang_rad = x_ang_rad;
                camera.z_ang_rad = z_ang_rad;*/
                camera.ViewDirty = true;

                Enquadrar();
            }

            face = "";
        }

        private void glControl_Click(object sender, EventArgs e)
        {
            VisualizarConformeGuizmo(ref cubo.FaceClique);
            gerenciador.pnArquivo.Visible = false;
            //  Cubo.FaceClique = "";
        }


        private void glControl_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.ControlKey)
            {
                clickCtrl = true;
            }
            else
            if (e.KeyData == Keys.Escape)
            {
                CancelaInsercoes();
              //  gerenciador.CancelaResultados();
            }
            if (ObjetoNovo is object)
            {
                ObjetoNovo.Command(e.KeyData);

                DesenhaObjetos();
                glControl.SwapBuffers();

                if (edX.Visible)
                    edX.Focus();
                //  if (e.KeyData == Keys.a)
            }
        }

        private void button1_Click(object sender, EventArgs e)
        {

        }

        private void button2_Click(object sender, EventArgs e)
        {
            TrechosVigas[0].linhas_eixo.pIni.x += double.Parse(textBox1.Text);
            TrechosVigas[0].linhas_eixo.pIni.y += double.Parse(textBox2.Text);
            TrechosVigas[0].linhas_eixo.pIni.z += double.Parse(textBox3.Text);

            TrechosVigas[0].linhas_eixo.pFin.x += double.Parse(textBox1.Text);
            TrechosVigas[0].linhas_eixo.pFin.y += double.Parse(textBox2.Text);
            TrechosVigas[0].linhas_eixo.pFin.z += double.Parse(textBox3.Text);
        }

        private void button4_Click(object sender, EventArgs e)
        {
           // textBox6.Text = (y_trans - pivoY).ToString("n2");
        }

        private void button3_Click_1(object sender, EventArgs e)
        {
          //  textBox5.Text = (x_trans - pivoX).ToString("n2");
        }

        private void button5_Click(object sender, EventArgs e)
        {
            // raioZoom.GerarRaio3D(ref mouseX, ref  mouseY, ref zNear, ref zFar, ref ViewPortPrincipal, ref  Mvm_Zoom, ref pm_Principal);
            //   raioZoom.CalculaIntersecao_Raio_x_Plano(ref planoZoom, ref Coord_PlanoZoom);


        }

        private void button6_Click(object sender, EventArgs e)
        {
            double ang = double.Parse(textBox5.Text);
            for (int i = 0; i < CoordsSubdvisao.Count; i++)
            {
                posicao[1] = CoordsSubdvisao[i].x;
                posicao[2] = CoordsSubdvisao[i].y;
                posicao[3] = CoordsSubdvisao[i].z;

                Geom.rotY(ang, ref posicao, ref posicaoFinal2);
                //   Geom.rotZ(angXZ, ref posicaoFinal, ref posicaoFinal2);

                CoordsSubdvisao[i].x = posicaoFinal2[1];
                CoordsSubdvisao[i].y = posicaoFinal2[2];
                CoordsSubdvisao[i].z = posicaoFinal2[3];
            }
            DesenhaObjetos();
            glControl.SwapBuffers();
        }

        private void button7_Click(object sender, EventArgs e)
        {
            double ang = double.Parse(textBox6.Text);
            for (int i = 0; i < CoordsSubdvisao.Count; i++)
            {
                posicao[1] = CoordsSubdvisao[i].x;
                posicao[2] = CoordsSubdvisao[i].y;
                posicao[3] = CoordsSubdvisao[i].z;

                Geom.rotZ(ang, ref posicao, ref posicaoFinal2);
                //   Geom.rotZ(angXZ, ref posicaoFinal, ref posicaoFinal2);

                CoordsSubdvisao[i].x = posicaoFinal2[1];
                CoordsSubdvisao[i].y = posicaoFinal2[2];
                CoordsSubdvisao[i].z = posicaoFinal2[3];
            }

            DesenhaObjetos();
            glControl.SwapBuffers();
        }

        private void button8_Click(object sender, EventArgs e)
        {
            FInfoBarraPortico info = new FInfoBarraPortico(Estrutura.PorticoEspacial.barras[(int)numericUpDown1.Value], gerenciador);
            info.Show();
        }

        bool NovaCoordX = false;
        bool NovaCoordY = false;
        bool NovaCoordZ = false;

        private void edZ_KeyUp(object sender, KeyEventArgs e)
        {

            if (e.KeyData == Keys.Enter)
            {
                ConfirmaCoordenada();
            }
            else
            {
                try
                {
                    if (NovaCoordZ)
                    {
                        if (edZ.Text.Trim() == "")
                        {
                            NovaCoordZ = false;
                        }
                        else
                        {
                            if (edZ.Text.Trim() != "-")
                            {
                                double z_conv = geometria_un_interna(System.Convert.ToSingle(edZ.Text)) * -1;

                                if (InsereCoordRelativa)
                                {
                                    if (tipoComando == eTipoComando.edit)
                                        if (IdFerramentaEdicao == Const.ID_COPIAR_ELEMENTOS)
                                            mousepoint.z = (FerramentaEdicao as TCopiarElementos).ponto1.z + z_conv;

                                    if (tipoComando == eTipoComando.draw)
                                    {
                                        if (pontoOrigemProjecao is object && ObjetoNovo is null)
                                            mousepoint.z = pontoOrigemProjecao.z + z_conv;
                                        else
                                        if (ObjetoNovo != null)
                                            mousepoint.z = ObjetoNovo.pIni.z + z_conv;
                                    }
                                }
                                else
                                    mousepoint.z = z_conv;
                            }
                        }
                    }
                    DesenhaObjetos();
                    glControl.SwapBuffers();
                }
                catch (Exception ec)
                {
                    MessageBox.Show(ec.Message);
                }
            }
        }
        public double geometria_un_interna(double v)
        {
            return conv.comp(v, gerenciador.ConfiguracoesPGi.CfgProjeto.unidadesProjeto.geo_un_comprimento, und.m);
        }
        public double geometria_un_visualizacao(double v)
        {
            return conv.comp(v, und.m, gerenciador.ConfiguracoesPGi.CfgProjeto.unidadesProjeto.geo_un_comprimento);
        }

        private void edX_KeyUp(object sender, KeyEventArgs e)
        {
            if (!edX.Visible)
                return;
            if (e.KeyData == Keys.Enter)
            {
                ConfirmaCoordenada();
            }
            else
            {
                try
                {
                    if (NovaCoordX)
                    {
                        if (edX.Text.Trim() == "")
                        {
                            NovaCoordX = false;
                        }
                        else
                        {
                            if (edX.Text.Trim() != "-")
                            {
                                double x_conv = geometria_un_interna(System.Convert.ToSingle(edX.Text));

                                if (InsereCoordRelativa)
                                {

                                    if (tipoComando == eTipoComando.edit)
                                        if (IdFerramentaEdicao == Const.ID_COPIAR_ELEMENTOS)
                                            mousepoint.x = (FerramentaEdicao as TCopiarElementos).ponto1.x + x_conv;

                                    if (tipoComando == eTipoComando.draw)
                                    {
                                        if (pontoOrigemProjecao is object && ObjetoNovo is null)
                                            mousepoint.x = pontoOrigemProjecao.x + x_conv;  
                                        else
                                        if (ObjetoNovo != null)
                                            mousepoint.x = ObjetoNovo.pIni.x + x_conv; 
                                    }
                                }
                                else
                                    mousepoint.x = x_conv;
                            }
                        }
                    }
                    DesenhaObjetos();
                    glControl.SwapBuffers();
                }
                catch (Exception ec)
                {
                    MessageBox.Show(ec.Message);
                }
            }
        }

        void ConfirmaCoordenada()
        {
            if (tipoComando == eTipoComando.draw)
                MouseDrawing(mousepoint.x, mousepoint.y, mousepoint.z);
            else
            if (tipoComando == eTipoComando.edit)
                MouseEdit(mousepoint.x, mousepoint.y, mousepoint.z, 0, "", 0);

            DesenhaObjetos();
            glControl.SwapBuffers();
        }

        private void edY_KeyUp(object sender, KeyEventArgs e)
        {
            if (e.KeyData == Keys.Enter)
            {
                ConfirmaCoordenada();
            }
            else
            {
                try
                {
                    if (NovaCoordY)
                    {
                        if (edY.Text.Trim() == "")
                        {
                            NovaCoordY = false;
                        }
                        else
                        if (edY.Text.Trim() != "-")
                        {
                            double y_conv = geometria_un_interna(System.Convert.ToSingle(edY.Text))*-1;
                            if (InsereCoordRelativa)
                            {
                                if (tipoComando == eTipoComando.edit)
                                    if (IdFerramentaEdicao == Const.ID_COPIAR_ELEMENTOS)
                                        mousepoint.y = (FerramentaEdicao as TCopiarElementos).ponto1.y + y_conv;

                                if (tipoComando == eTipoComando.draw)
                                {
                                    if (pontoOrigemProjecao is object && ObjetoNovo is null)
                                        mousepoint.y = pontoOrigemProjecao.y + y_conv;
                                    else
                                    if (ObjetoNovo != null)
                                        mousepoint.y = ObjetoNovo.pIni.y + y_conv;
                                }

                            }
                            else
                                mousepoint.y = y_conv;
                        }
                    }

                    DesenhaObjetos();
                    glControl.SwapBuffers();
                }
                catch (Exception ec)
                {
                    MessageBox.Show(ec.Message);
                }
            }
        }

        private void btConfirmaCoord_Click(object sender, EventArgs e)
        {
            ConfirmaCoordenada();
        }

        private void edX_KeyPress(object sender, System.Windows.Forms.KeyPressEventArgs e)
        {
            //    MessageBox.Show("keypress");

            if (e.KeyChar == (char)Keys.Enter || e.KeyChar == (char)Keys.Escape)
            {
                e.Handled = true;   // stop annoying beep
            }

            if (!char.IsControl(e.KeyChar) && !char.IsDigit(e.KeyChar) && (e.KeyChar != '.') && (e.KeyChar != '-'))
            {
                e.Handled = true;
            }

            // only allow one decimal point
            if ((e.KeyChar == '.') && ((sender as System.Windows.Forms.TextBox).Text.IndexOf('.') > -1))
            {
                e.Handled = true;
            }


            if ((sender as System.Windows.Forms.TextBox).Name == "edCota")
            {
                if (e.KeyChar == (char)Keys.Enter)
                    btConfirmaCota.Focus();
            }

            if ((sender as System.Windows.Forms.TextBox).Name == "edX")
            {
                if (!NovaCoordX)
                {
                    edX.Clear();
                }
                NovaCoordX = true;
            }

            if ((sender as System.Windows.Forms.TextBox).Name == "edY")
            {
                if (!NovaCoordY)
                {
                    edY.Clear();
                }
                NovaCoordY = true;
            }

            if ((sender as System.Windows.Forms.TextBox).Name == "edZ")
            {
                if (!NovaCoordZ)
                {
                    edZ.Clear();
                }
                NovaCoordZ = true;
            }

            // call base handler...
            base.OnKeyPress(e);
        }

        bool InsereCoordRelativa = false;
        private void btCoordRelativa_Click(object sender, EventArgs e)
        {
            if ((ObjetoNovo != null && tipoComando == eTipoComando.draw)
                || (tipoComando == eTipoComando.edit) || (pontoOrigemProjecao is object))
            {
                InsereCoordRelativa = !InsereCoordRelativa;

                if (InsereCoordRelativa)
                {
                    btCoordRelativa.ImageIndex = 1;
                    edX.Focus();
                    edX.SelectAll();
                    // lbCoordenadas.Text = "Coordenadas absolutas";
                }
                else
                {
                    btCoordRelativa.ImageIndex = 0;
                    //   lbCoordenadas.Text = "Coordenadas relativas";
                }

            }
        }

        private void button10_Click(object sender, EventArgs e)
        {
            pnDivBarras.Visible = false;
            aguardandoCota = false;
        }

        private void button9_Click(object sender, EventArgs e)
        {
            if (edCota.Text == "0.00" || edCota.Text == "0" || edCota.Text.Trim() == "")
            {
                MessageBox.Show("Valor inválido.", "Atenção", MessageBoxButtons.OK, MessageBoxIcon.Stop);
                gerenciador.AtualizaDesenho();
                edCota.Select();
                return;
            }

            ConfirmaCota();
            pnDivBarras.Visible = false;
        }

        private void edCota_KeyUp(object sender, KeyEventArgs e)
        {
            //  if (e.KeyData == Keys.Enter)
            // {
            //      btConfirmaCota.Focus();
            //   }
        }

        private void numericUpDown1_ValueChanged(object sender, EventArgs e)
        {
            for (int i = 1; i <= Estrutura.PorticoEspacial.nBarras; i++)
            {
                Estrutura.PorticoEspacial.barras[i].Selecionado = false;
                if (i == (int)numericUpDown1.Value)
                    Estrutura.PorticoEspacial.barras[i].Selecionado = true;

            }

            AtualizaShaders();
            gerenciador.AtualizaDesenho();
        }

        private void btCancelaCoord_Click(object sender, EventArgs e)
        {

        }

        private void edNumRepeticoes_ValueChanged(object sender, EventArgs e)
        {
            (FerramentaEdicao as TCopiarElementos).AtualizaRepeticoes((int)edNumRepeticoes.Value);
           // AtualizaShaders();
        }

        private void numericUpDown2_ValueChanged(object sender, EventArgs e)
        {
            //ogl.Inicializa(glControl.Width, glControl.Height, (float)numericUpDown2.Value, (float)numericUpDown3.Value, (float)numericUpDown4.Value);
            // AtualizaDisplayList();
            DesenhaObjetos();
            glControl.SwapBuffers();

        }

        private void mostrarSomenteToolStripMenuItem_Click(object sender, EventArgs e)
        {
            List<TCargaLinear> cargas;

            Estrutura.cargaLinear.ForEach(o => o.Visivel = false);

            foreach (TBarraGenerica b in Estrutura.barras)
            {
                b.Visivel = false;
                b.DirtySelecao = true;

                b.DirtyTriangulos = true;
                b.DirtyArestas = true;

                b.Linha_Eixo.Visivel = false;
                b.Linha_Eixo.pFin.habilitado = false;
                b.Linha_Eixo.pIni.habilitado = false;
                b.pFin.habilitado = false;
                b.pIni.habilitado = false;

                if (b.Selecionado)
                {
                    cargas = new List<TCargaLinear>(Estrutura.cargaLinear.FindAll(x => x.idBarra == b.IDBarra));

                    cargas.ForEach(o => o.Visivel = true);

                    b.pFin.habilitado = true;
                    b.pIni.habilitado = true;
                    b.Linha_Eixo.pFin.habilitado = true;
                    b.Linha_Eixo.pIni.habilitado = true;
                    b.Visivel = true;
                    b.Linha_Eixo.Visivel = true;
                }
            }

            foreach (TApoio b in Estrutura.apoios)
            {
                b.Visivel = false;

                if (b.Selecionado)
                    b.Visivel = true;
            }

            if (MostraTextoDiagramas)
                if (fx || my || mz || mx || fy || fz)
                {
                   /* if (!fx && !mx)*/ CriarTextosEsforco();
                 //   if (fx || mx) CriarTextosAxial_Torcor();
                }

            SetaSelecionados(false,0);
       //     Enquadrar();
            AtualizaShaders();

            AtualizaListaSnap();

            SetaSelecionados(false,0);

        }

        private void esconderSomenteToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Estrutura.cargaLinear.ForEach(o => o.Visivel = true);
            List<TCargaLinear> cargas;

            foreach (TBarraGenerica b in Estrutura.barras)
            {
              /*  b.Visivel = true;
                b.Linha_Eixo.Visivel = true;
                b.Linha_Eixo.pFin.habilitado = true;
                b.Linha_Eixo.pIni.habilitado = true;
                b.pFin.habilitado = true;
                b.pIni.habilitado = true;*/
                
                if (b.Selecionado)
                {
                    cargas = new List<TCargaLinear>(Estrutura.cargaLinear.FindAll(x => x.idBarra == b.IDBarra));
                    cargas.ForEach(o => o.Visivel = false);
                    b.DirtySelecao = true;
                    b.DirtyArestas = true;
                    b.DirtyTriangulos = true;
                    b.pFin.habilitado = false;
                    b.pIni.habilitado = false;
                    b.Visivel = false;
                    b.Selecionado = false;
                    b.Linha_Eixo.Visivel = false;
                    b.Linha_Eixo.pIni.habilitado = false;
                    b.Linha_Eixo.pFin.habilitado = false;
                }
            }

            foreach (TApoio b in Estrutura.apoios)
            {
            //    b.Visivel = true;
                if (b.Selecionado)
                {
                    b.Visivel = false;
                    b.Selecionado = false;
                }
            }

            if (MostraTextoDiagramas)
                if (fx || my || mz || mx || fy || fz)
                {
                    /*if (!fx && !mx) */CriarTextosEsforco();
                  //  if (fx || mx) CriarTextosAxial_Torcor();
                }

            SetaSelecionados(false,0);
            AtualizaShaders();
            AtualizaListaSnap();
            AtualizarDesenho();
            glControl.SwapBuffers();
        }

        public void MostrarTudo()
        {
         //   Estrutura.cargaLinear.ForEach(o => o.Visivel = true);

            foreach (TBarraGenerica b in Estrutura.barras)
            {
                b.Visivel = true;
                b.Linha_Eixo.pFin.habilitado = true;
                b.Linha_Eixo.pIni.habilitado = true;
                b.pFin.habilitado = true;
                b.pIni.habilitado = true;
                b.DirtySelecao = true;
                b.DirtyTriangulos = true;
                b.DirtyArestas = true;

                b.Linha_Eixo.Visivel = true;
            }

            foreach (TApoio b in Estrutura.apoios)
                b.Visivel = true;
           
            foreach (TObjetoDesenho o in Estrutura.objetos)
                o.Visivel = true;

            if (MostraTextoDiagramas)
              if (fx || my || mz || mx || fy || fz)
              {
                /*if (!fx && !mx) */CriarTextosEsforco();
               // if (fx || mx) CriarTextosAxial_Torcor();
              }
            SetaSelecionados(false, 0);
            AtualizaShaders();
            AtualizaListaSnap();
            AtualizarDesenho();
            glControl.SwapBuffers();
        }

        private void mostrarTudoToolStripMenuItem_Click(object sender, EventArgs e)
        {
            MostrarTudo();
            AtualizarDesenho();
        }


        private void editarBarras_Click(object sender, EventArgs e)
        {
            ChamaEditObjeto(Const.ID_BARRAGENERICA);
        }

        private void editarApoios_Click(object sender, EventArgs e)
        {
            ChamaEditObjeto(Const.ID_APOIO);
        }
        bool projecaoNo = false;
        TPonto pontoOrigemProjecao;

        public void ProjetaLinhasNoPonto()
        {
             CriaLinhasProjecaoTemporarias(ref mousepoint, true, true);
             pontoOrigemProjecao = new TPonto(mousepoint.x, mousepoint.y, mousepoint.z);
             pontoOrigemProjecao.px_x = mousepoint.px_x;
             pontoOrigemProjecao.px_y = mousepoint.px_y;
             snapEnd = false;
             projecaoNo = true;
             gerenciador.AtualizaDesenho();
        }

        private void timerSnap_Tick(object sender, EventArgs e)
        {
            // MessageBox.Show("");

                timerSnap.Enabled = false;

                CriaLinhasProjecaoTemporarias(ref mousepoint, true);
            pontoOrigemProjecao = new TPonto(mousepoint.x, mousepoint.y, mousepoint.z);
            pontoOrigemProjecao.px_x = mousepoint.px_x;
            pontoOrigemProjecao.px_y = mousepoint.px_y;
            snapEnd = false;
                projecaoNo = true;
                gerenciador.AtualizaDesenho();
        }

        private void pnRepeticoesCopia_Validated(object sender, EventArgs e)
        {

        }

        private void Desenho_Shown(object sender, EventArgs e)
        {
            pnDivBarras.Visible = false;

            pnRotacionar.Top = this.Height - 100;
            pnRotacionar.Left = this.Width - 350;

            Enquadrar();

            gerenciador.Activate();
        }
    }

}
//new OpenTK.Graphics.GraphicsMode(32, 24, 0, 8)
