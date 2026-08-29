using MathNet.Numerics.Distributions;
using OpenTK;
using OpenTK.Graphics.OpenGL;
using OpenTK.Input;
using OpenTK.Platform;
using Poly2Tri;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Linq;
using System.Net.NetworkInformation;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using Win32Interop.Enums;
using Win32Interop.Structs;
using static PG.Geom;

namespace PG
{
    public class TArticularElemento : IEditTool
    {
        public List<string> ListaComandos;
        public string Comando;
        public TPonto ponto1;
        public TArticularElemento()
        {
            ListaComandos = new List<string>();
            ListaComandos.Add(Const.ROTULAR_COMANDO_1);
        }

        public IEditTool Clone()
        {
            return new TArticularElemento();
        }

        public void Initialize(TObjetoDesenho objeto, ref TPonto point, ref string command, ISettings settings, TLayer layer)
        {
            // command = Const.CMD_PILAR_ROTACIONAR_1_P;

            // this.trecho = objeto as TTrechoViga;
            // ComandoSolicitado = 0;
            ponto1 = new TPonto(0);
            ponto2 = new TPonto(0);

        }
        int totalSelecao;
        public eObjetoDesenhoMouseDown OnMouseDown(Gerenciador gerenciador, FPrincipal desenho, ref List<TObjetoDesenho> ObjetosSelecionados, ref List<TObjetoDesenho> ObjetosResultado, ref TPonto point, ref string command, List<TLinha> Linhas, List<TPonto> Pontos, bool FromGrip = false)
        {
            if (Comando == ListaComandos[0])
            {
                ponto1 = new TPonto(0);
                ponto2 = new TPonto(0);

                List<TBarraGenerica> Objetos = new List<TBarraGenerica>();

                foreach (TObjetoDesenho obj in ObjetosSelecionados)
                  if (obj.Tipo == Const.ID_BARRAGENERICA)
                    Objetos.Add((TBarraGenerica)obj.Clone());

                if (Objetos.Count == 0)
                  command = "Nenhum objeto selecionado";

                command = "";
                return eObjetoDesenhoMouseDown.DoneRepeat;
            }
            else
                return eObjetoDesenhoMouseDown.Continue;
        }

        public void OnMouseMove(ref TPonto point, bool ShowInfo, bool orto = true, double angulo = 0, bool PontoInicial = false)
        {
        }

        public string GetComando(int numeroComando)
        {
            Comando = ListaComandos[numeroComando];
            return ListaComandos[numeroComando];
        }
        public void Continue()
        {

        }
        public bool ApagarSelecionados
        {
            get { return false; }
        }

        public string TipoObjetoParaSelecionar
        {
            get { return "TODOS"; }
        }
        public string LayerObjetoParaSelecionar
        {
            get { return "TODOS"; }
        }
        public eEditToolTipoSelecao editToolTipoSelecao
        {
            get { return eEditToolTipoSelecao.multiSelecao; }
        }
        [NonSerialized]
        public Pen mPen = new Pen(Color.White);
        public double angRotacao;
        public TPonto ponto2;

        public void OnMouseUp(ref TPonto point)
        {
        }
        public void OnKeyDown(System.Windows.Forms.KeyEventArgs e)
        {
        }
        public void Finished()
        {
        }
        public void Undo()
        {
        }
        public void Redo()
        {
        }
    }
    public class TDividirNasInterseccoes : IEditTool
    {
        public List<string> ListaComandos;
        bool DefinindoPontoBase = false;
        bool DefinindoPosicoes = false;
        public string Comando;
        TPonto centro;
        public List<TBarraGenerica> Objetos;
        TTexto TextoInfo;
        int pavimento;
        public TDividirNasInterseccoes()
        {
            ListaComandos = new List<string>();
            ListaComandos.Add("Dividir - Selecione os objetos e clique com o botão direito do mouse para confirmar");
        }

        public IEditTool Clone()
        {
            return new TDividirNasInterseccoes();
        }

        public void Initialize(TObjetoDesenho objeto, ref TPonto point, ref string command, ISettings settings, TLayer layer)
        {
        }


        public eObjetoDesenhoMouseDown OnMouseDown(Gerenciador gerenciador, FPrincipal desenho, ref List<TObjetoDesenho> ObjetosSelecionados, ref List<TObjetoDesenho> ObjetosResultado, ref TPonto point, ref string command, List<TLinha> Linhas, List<TPonto> Pontos, bool FromGrip = false)
        {
            if (Comando == ListaComandos[0])
            {
                Objetos = new List<TBarraGenerica>();
                double tt = 0, uu = 0;
                vec3 p_1, p_2, p_3, p_4;
                vec3 p_r = new vec3(0, 0, 0);
                vec3 ponto = new vec3(0, 0, 0);
                List<IntersecBarras> IntersecoesBarras = new List<IntersecBarras>();
                vec3 pontoToque = new vec3(0);
                
                List<TBarraGenerica> barras_com_offset = new List<TBarraGenerica>();

               

               /* foreach (TObjetoDesenho obj in ObjetosSelecionados)
                {
                    if (obj.Tipo == Const.ID_BARRAGENERICA)
                        if ((obj as TBarraGenerica).ids_barras_rigidas != null)
                            barras_com_offset.Add((obj as TBarraGenerica));
                }*/

               // desenho.SetaSelecionados(false, -1, true,false);
             //   TIntersecoes.RefazerOffsetsBarras(barras_com_offset, desenho.Estrutura.barras);
              //  desenho.DeletaSelecionados(-1,false); //deleta as barras de offsets
       

                foreach (TObjetoDesenho obj in ObjetosSelecionados)
                {
                    if (obj.Tipo == Const.ID_BARRAGENERICA)
                    {
                        Objetos.Add((TBarraGenerica)obj.Clone());
                        Objetos[Objetos.Count - 1].IDBarra = (obj as TBarraGenerica).IDBarra;
                        Objetos[Objetos.Count - 1].SetaSelecao(false, false);

                       /* if ((obj as TBarraGenerica).ids_barras_rigidas != null)
                        {
                            barras_com_offset.Add((obj as TBarraGenerica));
                        }*/
                    }
                }


                //return eObjetoDesenhoMouseDown.DoneRepeat;

                foreach (TBarraGenerica b1 in Objetos)
                {
                  //  if (b1.barra_offset) continue;

                    {
                        p_1 = new vec3(b1.pIni.x, b1.pIni.y, b1.pIni.z);
                        p_2 = new vec3(b1.pFin.x, b1.pFin.y, b1.pFin.z);
                    }

                    IntersecBarras ib = new IntersecBarras(b1);
   
                    foreach (TBarraGenerica b2 in Objetos)
                    {
                        if (!b2.Visivel/*|| b2.barra_offset*/) continue;
                        
                        if (b1.IDBarra != b2.IDBarra)
                        {
                            p_3 = new vec3(b2.pIni.x, b2.pIni.y, b2.pIni.z);
                            p_4 = new vec3(b2.pFin.x, b2.pFin.y, b2.pFin.z);

                            {
                                p_3 = new vec3(b2.pIni.x, b2.pIni.y, b2.pIni.z);
                                p_4 = new vec3(b2.pFin.x, b2.pFin.y, b2.pFin.z);
                            }

                            if (Geom.PontoTocaAresta(p_3, p_1, p_2, ref pontoToque, false))
                            {
                                if (TIntersecoes.LocalizaPonto(ref ib.pontos, pontoToque.x, pontoToque.y, pontoToque.z) > -1)
                                    continue;

                                ib.pontos.Add(new vec3(pontoToque.x, pontoToque.y, pontoToque.z));
                                ib.barras.Add(b2);
                            }
                            else
                            if (Geom.PontoTocaAresta(p_4, p_1, p_2, ref pontoToque, false))
                            {
                                if (TIntersecoes.LocalizaPonto(ref ib.pontos, pontoToque.x, pontoToque.y, pontoToque.z) > -1)
                                    continue;
                                
                                ib.pontos.Add(new vec3(pontoToque.x, pontoToque.y, pontoToque.z));
                                ib.barras.Add(b2);                        
                            }
                            else
                            if (Geom.TemInterseccao3D(ref p_1, ref p_2, ref p_3, ref p_4, ref ponto, ref tt, ref uu))
                            {
                                pontoToque = new vec3(0);
                                if (TIntersecoes.LocalizaPonto(ref ib.pontos, ponto.x, ponto.y, ponto.z) > -1)
                                    continue;
                                
                                if (Geom.PontoTocaAresta(ponto, p_1, p_2, ref pontoToque, false) &&  //testa se realmente o ponto de intersec esta contido nas duas barras
                                    Geom.PontoTocaAresta(ponto, p_3, p_4, ref pontoToque, false))
                                {
                                    ib.pontos.Add(new vec3(ponto.x, ponto.y, ponto.z));
                                    ib.barras.Add(b2);
                                }
                            }
                        }
                    }

                    IntersecoesBarras.Add(ib);
                }

                TBarraGenerica barranova = null;
                List<TBarraGenerica> barrasModelo = new List<TBarraGenerica>();
                List<vec3> nos_ordenados = new List<vec3>();
                List<vec3> nos_com_articulacao = new List<vec3>();

                foreach (IntersecBarras intersec in IntersecoesBarras)
                {
                    TPonto ultNo   = new TPonto(intersec.barra.pIni.x, intersec.barra.pIni.y, intersec.barra.pIni.z);
                    TPonto NoAtual = new TPonto(intersec.barra.pIni.x, intersec.barra.pIni.y, intersec.barra.pIni.z);
                    vec3 temp      = new vec3(0, 0, 0);
                    
                    nos_ordenados.Clear();
                    nos_com_articulacao.Clear();
/*<Nenhum>
Início e fim
Início
Fim*/
                    int Articulacao_my_barraoriginal = intersec.barra.Dados.Articulacao_my;
                    int Articulacao_mz_barraoriginal = intersec.barra.Dados.Articulacao_mz;
                                       
                    double dist;
                    for (int k = 0; k < intersec.barras.Count; k++)
                    {
                        dist = 9999;

                        for (int m = 0; m < intersec.pontos.Count; m++)
                        {
                            if (TIntersecoes.LocalizaPonto(ref nos_ordenados, intersec.pontos[m].x, intersec.pontos[m].y, intersec.pontos[m].z) > -1)
                              continue;

                            NoAtual = new TPonto(intersec.pontos[m].x, intersec.pontos[m].y, intersec.pontos[m].z);
                            double d2 = ultNo.DistanceTo(NoAtual);

                            if (d2 < dist)
                            {
                                dist = d2;
                                temp = intersec.pontos[m];
                            }
                        }

                        ultNo = new TPonto(temp.x, temp.y, temp.z);
                        nos_ordenados.Add(temp);
                    }
                    nos_ordenados.Add(new vec3(intersec.barra.pFin.x, intersec.barra.pFin.y, intersec.barra.pFin.z));

                    int max_id_barra_atual = desenho.Estrutura.barras.Max(o => o.IDBarra);
                    for (int k = 0; k < nos_ordenados.Count - 1; k++)
                    {
                        max_id_barra_atual++;
                        barranova = new TBarraGenerica(new TPonto(nos_ordenados[k].x, nos_ordenados[k].y, nos_ordenados[k].z),
                                                       new TPonto(nos_ordenados[k+1].x, nos_ordenados[k+1].y, nos_ordenados[k+1].z),
                                                       intersec.barra.layer, (TDadosBarra)intersec.barra.Dados.Clone(), -1);

                        barranova.Dados.Articulacao_my = 0;
                        barranova.Dados.Articulacao_mz = 0;

                        if (Articulacao_mz_barraoriginal > 0)
                        {
                            if (Articulacao_mz_barraoriginal == 3) // fim
                            {
                                if (Geom.Iguais(intersec.barra.pFin.x, barranova.pIni.x) && Geom.Iguais(intersec.barra.pFin.y, barranova.pIni.y) && Geom.Iguais(intersec.barra.pFin.z, barranova.pIni.z))
                                    barranova.Dados.Articulacao_mz = 2;//inicio
                                if (Geom.Iguais(intersec.barra.pFin.x, barranova.pFin.x) && Geom.Iguais(intersec.barra.pFin.y, barranova.pFin.y) && Geom.Iguais(intersec.barra.pFin.z, barranova.pFin.z))
                                    barranova.Dados.Articulacao_mz = 3; // fim
                            }
                            else
                            if (Articulacao_mz_barraoriginal == 2) // inicio
                            {
                                if (Geom.Iguais(intersec.barra.pIni.x, barranova.pIni.x) && Geom.Iguais(intersec.barra.pIni.y, barranova.pIni.y) && Geom.Iguais(intersec.barra.pIni.z, barranova.pIni.z))
                                    barranova.Dados.Articulacao_mz = 2;//inicio
                                if (Geom.Iguais(intersec.barra.pIni.x, barranova.pFin.x) && Geom.Iguais(intersec.barra.pIni.y, barranova.pFin.y) && Geom.Iguais(intersec.barra.pIni.z, barranova.pFin.z))
                                    barranova.Dados.Articulacao_mz = 3; // fim
                            }
                            else
                            if (Articulacao_mz_barraoriginal == 1) // inicio e fim
                            {
                                if (Geom.Iguais(intersec.barra.pIni.x, barranova.pIni.x) && Geom.Iguais(intersec.barra.pIni.y, barranova.pIni.y) && Geom.Iguais(intersec.barra.pIni.z, barranova.pIni.z))
                                    barranova.Dados.Articulacao_mz = 2;//inicio
                                if (Geom.Iguais(intersec.barra.pIni.x, barranova.pFin.x) && Geom.Iguais(intersec.barra.pIni.y, barranova.pFin.y) && Geom.Iguais(intersec.barra.pIni.z, barranova.pFin.z))
                                    barranova.Dados.Articulacao_mz = 3; // fim
                                if (Geom.Iguais(intersec.barra.pFin.x, barranova.pIni.x) && Geom.Iguais(intersec.barra.pFin.y, barranova.pIni.y) && Geom.Iguais(intersec.barra.pFin.z, barranova.pIni.z))
                                    barranova.Dados.Articulacao_mz = 2;//inicio
                                if (Geom.Iguais(intersec.barra.pFin.x, barranova.pFin.x) && Geom.Iguais(intersec.barra.pFin.y, barranova.pFin.y) && Geom.Iguais(intersec.barra.pFin.z, barranova.pFin.z))
                                    barranova.Dados.Articulacao_mz = 3; // fim
                            }
                        }

                        if (Articulacao_my_barraoriginal > 0)
                        {
                            if (Articulacao_my_barraoriginal == 3) // fim
                            {
                                if (Geom.Iguais(intersec.barra.pFin.x , barranova.pIni.x) && Geom.Iguais(intersec.barra.pFin.y, barranova.pIni.y) && Geom.Iguais(intersec.barra.pFin.z, barranova.pIni.z))
                                    barranova.Dados.Articulacao_my = 2;//inicio
                                if (Geom.Iguais(intersec.barra.pFin.x, barranova.pFin.x) && Geom.Iguais(intersec.barra.pFin.y, barranova.pFin.y) && Geom.Iguais(intersec.barra.pFin.z, barranova.pFin.z))
                                    barranova.Dados.Articulacao_my = 3; // fim
                            }
                            else
                            if (Articulacao_my_barraoriginal == 2) // inicio
                            {
                                if (Geom.Iguais(intersec.barra.pIni.x, barranova.pIni.x) && Geom.Iguais(intersec.barra.pIni.y, barranova.pIni.y) && Geom.Iguais(intersec.barra.pIni.z, barranova.pIni.z))
                                    barranova.Dados.Articulacao_my = 2;//inicio
                                if (Geom.Iguais(intersec.barra.pIni.x, barranova.pFin.x) && Geom.Iguais(intersec.barra.pIni.y, barranova.pFin.y) && Geom.Iguais(intersec.barra.pIni.z, barranova.pFin.z))
                                    barranova.Dados.Articulacao_my = 3; // fim
                            }
                            else
                            if (Articulacao_my_barraoriginal == 1) // inicio e fim
                            {
                                if (Geom.Iguais(intersec.barra.pIni.x, barranova.pIni.x) && Geom.Iguais(intersec.barra.pIni.y, barranova.pIni.y) && Geom.Iguais(intersec.barra.pIni.z, barranova.pIni.z))
                                    barranova.Dados.Articulacao_my = 2;//inicio
                                if (Geom.Iguais(intersec.barra.pIni.x, barranova.pFin.x) && Geom.Iguais(intersec.barra.pIni.y, barranova.pFin.y) && Geom.Iguais(intersec.barra.pIni.z, barranova.pFin.z))
                                    barranova.Dados.Articulacao_my = 3; // fim
                                if (Geom.Iguais(intersec.barra.pFin.x, barranova.pIni.x) && Geom.Iguais(intersec.barra.pFin.y, barranova.pIni.y) && Geom.Iguais(intersec.barra.pFin.z, barranova.pIni.z))
                                    barranova.Dados.Articulacao_my = 2;//inicio
                                if (Geom.Iguais(intersec.barra.pFin.x, barranova.pFin.x) && Geom.Iguais(intersec.barra.pFin.y, barranova.pFin.y) && Geom.Iguais(intersec.barra.pFin.z, barranova.pFin.z))
                                    barranova.Dados.Articulacao_my = 3; // fim
                            }
                        }

                        barranova.OrientaSecaoNoEspaco();

                        barranova.pIni.BarrasConectadas.Add(barranova);
                        barranova.pFin.BarrasConectadas.Add(barranova);

                        barranova.CriaPesoProprio();
                        ObjetosResultado.Add(barranova);

                        List<TCargaLinear> cl = desenho.CargasLineares.FindAll(c => c.idBarra == intersec.barra.IDBarra && c.Dados.idCaso != 1);
                        foreach (TCargaLinear clinear in cl)
                            ObjetosResultado.Add(new TCargaLinear(barranova.pIni, barranova.pFin, (TDadosCarga)clinear.Dados.Clone(),
                                                                  barranova.Dados.anguloRotacao, max_id_barra_atual, clinear.layer));
                    }

                    foreach (TObjetoDesenho ob in ObjetosSelecionados)
                    {
                        if (ob == null) continue;
                        if (ob.Tipo != Const.ID_BARRAGENERICA) continue;
                           
                        if ((ob as TBarraGenerica).IDBarra == intersec.barra.IDBarra)
                        {
                            //se tem articulacao nopfin da barra selecionada e o primeiro nó da lista for igual ao pfin, entao mantem a articulacao, senao apaga
                            if ((ob as TBarraGenerica).Dados.Articulacao_my > 0)
                            {
                                if ((ob as TBarraGenerica).Dados.Articulacao_my == 3) // 'fim' 
                                {
                                    if (!Geom.Iguais((ob as TBarraGenerica).pFin.x, nos_ordenados[0].x) || !Geom.Iguais((ob as TBarraGenerica).pFin.y, nos_ordenados[0].y) || !Geom.Iguais((ob as TBarraGenerica).pFin.z, nos_ordenados[0].z))
                                        (ob as TBarraGenerica).Dados.Articulacao_my = 0; // nenhuma
                                }
                                else
                                if ((ob as TBarraGenerica).Dados.Articulacao_my == 1) // 'inicio e fim'
                                {
                                    if (!Geom.Iguais((ob as TBarraGenerica).pFin.x, nos_ordenados[0].x) || !Geom.Iguais((ob as TBarraGenerica).pFin.y, nos_ordenados[0].y) || !Geom.Iguais((ob as TBarraGenerica).pFin.z, nos_ordenados[0].z))
                                        (ob as TBarraGenerica).Dados.Articulacao_my = 2; // inicio
                                }
                            }

                            if ((ob as TBarraGenerica).Dados.Articulacao_mz > 0)
                            {
                                if ((ob as TBarraGenerica).Dados.Articulacao_mz == 3) // 'fim'
                                {
                                    if (!Geom.Iguais((ob as TBarraGenerica).pFin.x, nos_ordenados[0].x) || !Geom.Iguais((ob as TBarraGenerica).pFin.y, nos_ordenados[0].y) || !Geom.Iguais((ob as TBarraGenerica).pFin.z, nos_ordenados[0].z))
                                        (ob as TBarraGenerica).Dados.Articulacao_mz = 0; // nenhuma
                                }
                                else
                                if ((ob as TBarraGenerica).Dados.Articulacao_mz == 1) // 'inicio e fim'
                                {
                                    if (!Geom.Iguais((ob as TBarraGenerica).pFin.x, nos_ordenados[0].x) || !Geom.Iguais((ob as TBarraGenerica).pFin.y, nos_ordenados[0].y) || !Geom.Iguais((ob as TBarraGenerica).pFin.z, nos_ordenados[0].z))
                                        (ob as TBarraGenerica).Dados.Articulacao_mz = 2; // inicio
                                }
                            }

                            (ob as TBarraGenerica).pFin.x = nos_ordenados[0].x; (ob as TBarraGenerica).pFin.y = nos_ordenados[0].y; (ob as TBarraGenerica).pFin.z = nos_ordenados[0].z;

                        }
                    }
                }
              
                command = GetComando(0);
                return eObjetoDesenhoMouseDown.DoneRepeat;
            }
            else
                return eObjetoDesenhoMouseDown.Done;
        }
        public string GetComando(int numeroComando)
        {
            Comando = ListaComandos[numeroComando];
            return ListaComandos[numeroComando];
        }
        public void Continue()
        {
            DefinindoPontoBase = true;
        }
        public bool ApagarSelecionados
        {
            get { return false; }
        }

        public string TipoObjetoParaSelecionar
        {
            get { return "TODOS"; }
        }
        public string LayerObjetoParaSelecionar
        {
            get { return "TODOS"; }
        }
        public eEditToolTipoSelecao editToolTipoSelecao
        {
            get { return eEditToolTipoSelecao.multiSelecao; }
        }
        [NonSerialized]
        public Pen mPen = new Pen(Color.White);
        public double angRotacao;

        public void OnMouseMove(ref TPonto point, bool ShowInfo, bool orto = true, double angulo = 0, bool PontoInicial = false)
        {
        }

        public void OnMouseUp(ref TPonto point)
        {
        }
        public void OnKeyDown(System.Windows.Forms.KeyEventArgs e)
        {
        }
        public void Finished()
        {
        }
        public void Undo()
        {
        }
        public void Redo()
        {
        }
    }

    public struct IntersecBarras
    {
        public IntersecBarras(TBarraGenerica b)
        {
            barra = b;
            barras = new List<TBarraGenerica>();
            pontos = new List<vec3>();
        }
        public List<vec3> pontos;
        public TBarraGenerica barra;
        public List<TBarraGenerica> barras;
    }

    public class TGirarElementos : IEditTool
    {
        public List<string> ListaComandos;
        bool DefinindoPontoBase = false;
        bool DefinindoPosicoes = false;
        public string Comando;
        TPonto centro;
        public List<TObjetoDesenho> Objetos;
        TTexto TextoInfo;
        int pavimento;
        public TGirarElementos()
        {
            ListaComandos = new List<string>();
            ListaComandos.Add("Rotacionar - Selecione os objetos e clique com o botão direito do mouse para confirmar");
            ListaComandos.Add("Rotacionar - Indique o ponto base");
            ListaComandos.Add("Rotacionar - Ângulo de rotação: ");
            TextoInfo = new TTexto("Ang: ", 0, 0, 0.08, 0.08, 1, 1, 0, 0, null,"");
        }

        public IEditTool Clone()
        {
            return new TGirarElementos();
        }

        public void Initialize(TObjetoDesenho objeto, ref TPonto point, ref string command, ISettings settings, TLayer layer)
        {
            // command = Const.CMD_PILAR_ROTACIONAR_1_P;

            // this.trecho = objeto as TTrechoViga;
            // ComandoSolicitado = 0;
        }

        public eObjetoDesenhoMouseDown OnMouseDown(Gerenciador gerenciador, FPrincipal desenho, ref List<TObjetoDesenho> ObjetosSelecionados, ref List<TObjetoDesenho> ObjetosResultado, ref TPonto point, ref string command, List<TLinha> Linhas, List<TPonto> Pontos, bool FromGrip = false)
        {
            if (Comando == ListaComandos[0])
            {
                pavimento = desenho.PavimentoAtual;
                command = GetComando(1);
                return eObjetoDesenhoMouseDown.Continue;
            }
            else
            if (Comando == ListaComandos[1])
            {
                centro = point;
                Objetos = new List<TObjetoDesenho>();

                foreach (TObjetoDesenho obj in ObjetosSelecionados)
                {
                    Objetos.Add(obj.Clone());
                    Objetos[Objetos.Count - 1].SetaSelecao(false,false);
                    Objetos[Objetos.Count - 1].ObjetoCopia = true;
                }
                command = GetComando(2);
                return eObjetoDesenhoMouseDown.Continue;
            }
            else
            if (Comando == ListaComandos[2])
            {
                foreach (TObjetoDesenho obj in Objetos)
                {
                    obj.Atualiza(pavimento);
                    obj.ObjetoCopia = false;
                    ObjetosResultado.Add(obj);
                }
                command = "";
                return eObjetoDesenhoMouseDown.DoneRepeat;
            }
            else
                return eObjetoDesenhoMouseDown.Continue;
        }
        public string GetComando(int numeroComando)
        {
            Comando = ListaComandos[numeroComando];
            return ListaComandos[numeroComando];
        }
        public void Continue()
        {
            DefinindoPontoBase = true;
        }
        public bool ApagarSelecionados
        {
            get { return true; }
        }

        public string TipoObjetoParaSelecionar
        {
            get { return "TODOS"; }
        }
        public string LayerObjetoParaSelecionar
        {
            get { return "TODOS"; }
        }
        public eEditToolTipoSelecao editToolTipoSelecao
        {
            get { return eEditToolTipoSelecao.multiSelecao; }
        }     
        [NonSerialized]
        public Pen mPen = new Pen(Color.White);
        public double angRotacao;

        public void OnMouseMove(ref TPonto point, bool ShowInfo, bool orto = true, double angulo = 0, bool PontoInicial = false)
        {

            GL.Color3(Color.Red);
            GL.Begin(PrimitiveType.Lines);
            GL.Vertex3(centro.x, centro.y, centro.z);
            GL.Vertex3(point.x, point.y, point.z);
            GL.End();

                if (angulo != 0)
                    angRotacao = angulo;
                else
                    angRotacao = Geom.GetAnguloGlobal(centro.x, centro.y, point.x, point.y);

            //    TextoInfo.DesenhaSemZoom(ref Cad, point.x + 5, point.y + 5, "ϴ: " + (this.angRotacao).ToString("n2") + "º");
           

            foreach (TObjetoDesenho obj in Objetos)
            {
               //  if (obj.Tipo == Const.ID_LINHA)
                     (obj).Rotacionar(centro, point, angRotacao);
            }
        }

        public void OnMouseUp(ref TPonto point)
        {
        }
        public void OnKeyDown(System.Windows.Forms.KeyEventArgs e)
        {
        }
        public void Finished()
        {
        }
        public void Undo()
        {
        }
        public void Redo()
        {
        }
    }
    public class TMoverElementos : IEditTool
    {
        public List<string> ListaComandos;
        bool DefinindoPontoBase = false;
        bool DefinindoPosicoes = false;
        public string Comando;
        public List<TObjetoDesenho> Objetos;
        TTexto TextoInfo;
        int pavimento;
        public TPonto ponto1;
        public TMoverElementos()
        {
            ListaComandos = new List<string>();
            ListaComandos.Add(Const.MOVER_COMANDO_1);
            ListaComandos.Add(Const.MOVER_COMANDO_2);
            ListaComandos.Add(Const.MOVER_COMANDO_3);
        }

        public IEditTool Clone()
        {
            return new TMoverElementos();
        }

        public void Initialize(TObjetoDesenho objeto, ref TPonto point, ref string command, ISettings settings, TLayer layer)
        {
            // command = Const.CMD_PILAR_ROTACIONAR_1_P;

            // this.trecho = objeto as TTrechoViga;
            // ComandoSolicitado = 0;
            ponto1 = new TPonto(0);
            ponto2 = new TPonto(0);

        }

        public eObjetoDesenhoMouseDown OnMouseDown(Gerenciador gerenciador, FPrincipal desenho, ref List<TObjetoDesenho> ObjetosSelecionados, ref List<TObjetoDesenho> ObjetosResultado, ref TPonto point, ref string command, List<TLinha> Linhas, List<TPonto> Pontos, bool FromGrip = false)
        {
            if (Comando == ListaComandos[0])
            {
                ponto1 = new TPonto(0);
                ponto2 = new TPonto(0);

                pavimento = desenho.PavimentoAtual;

                foreach (TObjetoDesenho o in ObjetosSelecionados)
                {
                    if (o.Tipo == Const.ID_BARRAGENERICA)
                    {
                        ((o as TBarraGenerica).pIni.Selecionado) = false;
                        ((o as TBarraGenerica).pFin.Selecionado) = false;
                    }
                }

                ObjetosSelecionados.RemoveAll(o=>o.Selecionado == false);

                int totalSelecao = ObjetosSelecionados.Count();
                int repeticoesNoMesmaCoordenada = 0;
                List<TPonto> pts = new List<TPonto>();
                foreach (TObjetoDesenho o in ObjetosSelecionados)
                {
                    if (o.Tipo == Const.ID_PONTO && o.Selecionado)
                    {
                        pts.Add(o as TPonto);
                    }
                }

                foreach (TPonto o in pts)
                {
                    if (pts.FindAll(p => Geom.Iguais(p.x ,o.x, Const.Tol) && Geom.Iguais(p.y,o.y, Const.Tol) && Geom.Iguais(p.z,o.z, Const.Tol) && p.Snap == false).Count() > 1)
                    {
                        repeticoesNoMesmaCoordenada++;
                        o.Snap = true;
                    }
                }

                foreach (TPonto o in pts) o.Snap = false;

                totalSelecao -= repeticoesNoMesmaCoordenada;

                if (totalSelecao == 0)
                    command = "Nenhum objeto selecionado";
                else
                if (totalSelecao > 1)
                    command = GetComando(1) + " <" + totalSelecao.ToString() + " objetos selecionados>";
                else
                    command = GetComando(1) + " <" + totalSelecao.ToString() + " objeto selecionado>";
                return eObjetoDesenhoMouseDown.Continue;
            }
            else
            if (Comando == ListaComandos[1])
            {
                this.ponto1.x = point.x;
                this.ponto1.y = point.y;
                this.ponto1.z = point.z;
                Objetos = new List<TObjetoDesenho>();

                foreach (TObjetoDesenho obj in ObjetosSelecionados)
                {
                    Objetos.Add(obj.Clone());
                    Objetos[Objetos.Count - 1].SetaSelecao(false, false);
                    Objetos[Objetos.Count - 1].ObjetoCopia = true;
                }
                command = GetComando(2);
                return eObjetoDesenhoMouseDown.Continue;
            }
            else
            if (Comando == ListaComandos[2])
            {
                //   foreach (TObjetoDesenho obj in ObjetosSelecionados)
                //     obj.SetaSelecao(false, false);

                OnMouseMove(ref point, false);

                foreach (TObjetoDesenho obj in Objetos)
                {
                    obj.Atualiza(pavimento);
                    obj.ObjetoCopia = false;
                    //      ObjetosResultado.Add(obj);
 
                }

                foreach (TObjetoDesenho obj in ObjetosSelecionados)
                {
                    if (obj.Tipo == Const.ID_PONTO)
                    {
                        for (int i = 0; i < desenho.Estrutura.cargaPontual.Count; i++)
                        {
                            if (desenho.Estrutura.cargaPontual[i].ponto == (obj as TPonto))
                            {
                                desenho.Estrutura.cargaPontual[i].pIni.Mover(ref ponto1, ref ponto2, false);
                                desenho.Estrutura.cargaPontual[i].ponto.Mover(ref ponto1, ref ponto2, false);
                            }
                        }
  
                        (obj).Mover(ref ponto1, ref ponto2, false);
                    }
                    else
                    if (obj.Tipo == Const.ID_BARRAGENERICA)
                    {
                        for (int i = 0; i < desenho.Estrutura.cargaPontual.Count; i++)
                        {
                            if (desenho.Estrutura.cargaPontual[i].ponto == (obj.pIni as TPonto))
                            {
                                desenho.Estrutura.cargaPontual[i].pIni.Mover(ref ponto1, ref ponto2, false);
                                desenho.Estrutura.cargaPontual[i].ponto.Mover(ref ponto1, ref ponto2, false);
                            }
                            if (desenho.Estrutura.cargaPontual[i].ponto == (obj.pFin as TPonto))
                            {
                                desenho.Estrutura.cargaPontual[i].pIni.Mover(ref ponto1, ref ponto2, false);
                                desenho.Estrutura.cargaPontual[i].ponto.Mover(ref ponto1, ref ponto2, false);
                            }
                        }

                        (obj).Mover(ref ponto1, ref ponto2, false);
                    }
                    else
                        (obj).Mover(ref ponto1, ref ponto2, false);
                }


                command = "";
                return eObjetoDesenhoMouseDown.DoneRepeat;
            }
            else
                return eObjetoDesenhoMouseDown.Continue;
        }

        public void DesenhaSeta()
        {
            GL.Color3(Color.Red);
            GL.Begin(PrimitiveType.Lines);
            GL.Vertex3(ponto1.x, ponto1.y, ponto1.z);
            GL.Vertex3(ponto2.x, ponto2.y, ponto2.z);
            GL.End();
        }

        public void OnMouseMove(ref TPonto point, bool ShowInfo, bool orto = true, double angulo = 0, bool PontoInicial = false)
        {

         /*   GL.Color3(Color.Red);
            GL.Begin(PrimitiveType.Lines);
            GL.Vertex3(ponto1.x, ponto1.y, ponto1.z);
            GL.Vertex3(point.x, point.y, point.z);
            GL.End();*/
            /*   if (angulo != 0)
                   angRotacao = angulo;
               else
                   angRotacao = Geom.GetAnguloGlobal(centro.x, centro.y, point.x, point.y);*/

            //    TextoInfo.DesenhaSemZoom(ref Cad, point.x + 4, point.y + 3, "Desloc: " + (this.angRotacao).ToString("n2") + "º");

            ponto2.x = point.x;
            ponto2.y = point.y;
            ponto2.z = point.z;

            // Orto(ref ponto1.x, ref ponto1.y, ref ponto2.x, ref ponto2.y);
            foreach (TObjetoDesenho obj in Objetos)
                (obj).Mover(ref ponto1, ref ponto2, true);
        }

        public string GetComando(int numeroComando)
        {
            Comando = ListaComandos[numeroComando];
            return ListaComandos[numeroComando];
        }
        public void Continue()
        {
            DefinindoPontoBase = true;
        }
        public bool ApagarSelecionados
        {
            get { return false; }
        }

        public string TipoObjetoParaSelecionar
        {
            get { return "TODOS"; }
        }
        public string LayerObjetoParaSelecionar
        {
            get { return "TODOS"; }
        }
        public eEditToolTipoSelecao editToolTipoSelecao
        {
            get { return eEditToolTipoSelecao.multiSelecao; }
        }
        [NonSerialized]
        public Pen mPen = new Pen(Color.White);
        public double angRotacao;
        public TPonto ponto2;


        double DifCoordX, DifCoordY, alfa;
        public void Orto(ref double xini, ref double yini, ref double xfin, ref double yfin)
        {
            //     if (pFin.Snap)
            //     MessageBox.Show("");
          /*  alfa = (float)(FuncoesGerais.atand((yini - yfin) / (xini - xfin)));

           /* if (ConfiguracoesCaptura.Captura.OrtogonalLigado)
            {
                if (alfa > 85)
                {
                    alfa = 90;
                    xfin = xini;
                }
                else
                    if (alfa > 0 && alfa < 2)
                        yfin = yini;
            };*/

         /*   DifCoordX = Math.Abs(xini - xfin);
            DifCoordY = Math.Abs(yini - yfin);*/

           /* for (int i = 0; i < ConfiguracoesCaptura.Captura.OutrosAngulos.Count; i++)
            {
                if (alfa >= (ConfiguracoesCaptura.Captura.OutrosAngulos[i] - 5) && alfa <= (ConfiguracoesCaptura.Captura.OutrosAngulos[i] + 5))
                {
                    alfa = ConfiguracoesCaptura.Captura.OutrosAngulos[i];

                    if (yfin > yini)
                        yfin = (yini + (float)(Math.Tan(alfa * Const.PIDiv180) * DifCoordX));
                    else
                        yfin = (yini - (float)(Math.Tan(alfa * Const.PIDiv180) * DifCoordX));
                };
            };*/
        }
        public void OnMouseUp(ref TPonto point)
        {
        }
        public void OnKeyDown(System.Windows.Forms.KeyEventArgs e)
        {
        }
        public void Finished()
        {
        }
        public void Undo()
        {
        }
        public void Redo()
        {
        }
    }
    public class TMoverExtremoElemento : IEditTool
    {
        public List<string> ListaComandos;
        public string Comando;
        public List<TObjetoDesenho> Objetos;
        public TPonto ponto1;
        public TMoverExtremoElemento()
        {
            ListaComandos = new List<string>();
            ListaComandos.Add(Const.MOVER_EXTREMO_COMANDO_1);
            ListaComandos.Add(Const.MOVER_EXTREMO_COMANDO_2);
            ListaComandos.Add(Const.MOVER_EXTREMO_COMANDO_3);
            ListaComandos.Add(Const.MOVER_EXTREMO_COMANDO_4);
        }

        public IEditTool Clone()
        {
            return new TMoverExtremoElemento();
        }

        public void Initialize(TObjetoDesenho objeto, ref TPonto point, ref string command, ISettings settings, TLayer layer)
        {
            // command = Const.CMD_PILAR_ROTACIONAR_1_P;

            // this.trecho = objeto as TTrechoViga;
            // ComandoSolicitado = 0;
            ponto1 = new TPonto(0);
            ponto2 = new TPonto(0);

        }
        int totalSelecao;
        public eObjetoDesenhoMouseDown OnMouseDown(Gerenciador gerenciador, FPrincipal desenho, ref List<TObjetoDesenho> ObjetosSelecionados, ref List<TObjetoDesenho> ObjetosResultado, ref TPonto point, ref string command, List<TLinha> Linhas, List<TPonto> Pontos, bool FromGrip = false)
        {
            if (Comando == ListaComandos[0])
            {
                ponto1 = new TPonto(0);
                ponto2 = new TPonto(0);

                foreach (TObjetoDesenho o in ObjetosSelecionados)
                {
                    if (o.Tipo == Const.ID_BARRAGENERICA)
                    {
                        ((o as TBarraGenerica).pIni.Selecionado) = false;
                        ((o as TBarraGenerica).pFin.Selecionado) = false;
                    }
                    if (o.Tipo == Const.ID_PONTO)
                        o.Selecionado = false;
                }

                ObjetosSelecionados.RemoveAll(o => o.Selecionado == false);
                gerenciador.formDesenho.AtualizaDisplayList_Nos();

                totalSelecao = ObjetosSelecionados.Count();
                int repeticoesNoMesmaCoordenada = 0;
                List<TPonto> pts = new List<TPonto>();
                foreach (TObjetoDesenho o in ObjetosSelecionados)
                {
                    if (o.Tipo == Const.ID_PONTO && o.Selecionado)
                    {
                        pts.Add(o as TPonto);
                    }
                }

                foreach (TPonto o in pts)
                {
                    if (pts.FindAll(p => Geom.Iguais(p.x, o.x) && Geom.Iguais(p.y, o.y) && Geom.Iguais(p.z, o.z) && p.Snap == false).Count() > 1)
                    {
                        repeticoesNoMesmaCoordenada++;
                        o.Snap = true;
                    }
                }

                foreach (TPonto o in pts) o.Snap = false;

                totalSelecao -= repeticoesNoMesmaCoordenada;

                if (totalSelecao > 1)
                    command = GetComando(1) ;
                else
                if (totalSelecao == 0)
                {
                    command = "Nenhum elemento selecionado";
                    GetComando(0);
                }
                else
                    command = GetComando(1);

                gerenciador.formDesenho.tipoComando = eTipoComando.selecionar;
                gerenciador.formDesenho.clickShift = true;
                return eObjetoDesenhoMouseDown.Continue;
            }
            else
            if (Comando == ListaComandos[1])
            {
                ponto1 = new TPonto(0);
                ponto2 = new TPonto(0);

                if (ObjetosSelecionados.FindAll(o => o.Tipo == Const.ID_PONTO).Count == 0)
                {
                    if (totalSelecao > 1)
                        command = GetComando(1) + " <" + totalSelecao.ToString() + " elementos selecionados>";
                    else
                    if (totalSelecao == 0)
                    {
                        command = "Nenhum elemento selecionado";
                        GetComando(0);
                    }
                    else
                        command = GetComando(1) + " <" + totalSelecao.ToString() + " elemento selecionado>";

                    gerenciador.formDesenho.tipoComando = eTipoComando.selecionar;
                    gerenciador.formDesenho.clickShift = true;
                }
                else
                {
                    command = GetComando(2);
                    gerenciador.formDesenho.clickShift = false;
                }

                return eObjetoDesenhoMouseDown.Continue;
            }
            else
            if (Comando == ListaComandos[2])
            {
                this.ponto1.x = point.x;
                this.ponto1.y = point.y;
                this.ponto1.z = point.z;
                Objetos = new List<TObjetoDesenho>();

                foreach (TObjetoDesenho obj in ObjetosSelecionados)
                {
                    if (obj.Tipo == Const.ID_BARRAGENERICA)
                    {
                        if ((obj as TBarraGenerica).pIni.Selecionado)
                        {
                            Objetos.Add((obj as TBarraGenerica).pIni.Clone());
                            Objetos[Objetos.Count - 1].SetaSelecao(false, false);
                            Objetos[Objetos.Count - 1].ObjetoCopia = true;
                        }
             
                        if ((obj as TBarraGenerica).pFin.Selecionado)
                        {
                            Objetos.Add((obj as TBarraGenerica).pFin.Clone());
                            Objetos[Objetos.Count - 1].SetaSelecao(false, false);
                            Objetos[Objetos.Count - 1].ObjetoCopia = true;
                        }
                    }
                }
                command = GetComando(3);
                return eObjetoDesenhoMouseDown.Continue;
            }
            else
            if (Comando == ListaComandos[3])
            {
                //   foreach (TObjetoDesenho obj in ObjetosSelecionados)
                //     obj.SetaSelecao(false, false);

                OnMouseMove(ref point, false);

                foreach (TObjetoDesenho obj in Objetos)
                {
                    obj.Atualiza(0);
                    obj.ObjetoCopia = false;
                    //      ObjetosResultado.Add(obj);
                }

                foreach (TObjetoDesenho obj in ObjetosSelecionados)
                {
                    if (obj.Tipo == Const.ID_BARRAGENERICA)
                    {
                        if ((obj as TBarraGenerica).pIni.Selecionado)
                        {
                            (obj as TBarraGenerica).pIni.Mover(ref ponto1, ref ponto2, false);

                            for (int i = 0; i < desenho.Estrutura.cargaPontual.Count; i++)
                            {
                                if (desenho.Estrutura.cargaPontual[i].ponto == (obj as TBarraGenerica).pIni)
                                {
                                    desenho.Estrutura.cargaPontual[i].ponto.Mover(ref ponto1, ref ponto2, false);
                                    desenho.Estrutura.cargaPontual[i].pIni.Mover(ref ponto1, ref ponto2, false);
                                }
                            }
                        }

                        if ((obj as TBarraGenerica).pFin.Selecionado)
                        {
                            (obj as TBarraGenerica).pFin.Mover(ref ponto1, ref ponto2, false);

                            for (int i = 0; i < desenho.Estrutura.cargaPontual.Count; i++)
                            {
                                if (desenho.Estrutura.cargaPontual[i].ponto == (obj as TBarraGenerica).pFin)
                                {
                                    desenho.Estrutura.cargaPontual[i].ponto.Mover(ref ponto1, ref ponto2, false);
                                    desenho.Estrutura.cargaPontual[i].pIni.Mover(ref ponto1, ref ponto2, false);
                                }
                            }
                        }
                    }
                }

                command = "";
                return eObjetoDesenhoMouseDown.DoneRepeat;
            }
            else
                return eObjetoDesenhoMouseDown.Continue;
        }

        public void DesenhaSeta()
        {
            GL.Color3(Color.Red);
            GL.Begin(PrimitiveType.Lines);
            GL.Vertex3(ponto1.x, ponto1.y, ponto1.z);
            GL.Vertex3(ponto2.x, ponto2.y, ponto2.z);
            GL.End();
        }

        public void OnMouseMove(ref TPonto point, bool ShowInfo, bool orto = true, double angulo = 0, bool PontoInicial = false)
        {

            /*   GL.Color3(Color.Red);
               GL.Begin(PrimitiveType.Lines);
               GL.Vertex3(ponto1.x, ponto1.y, ponto1.z);
               GL.Vertex3(point.x, point.y, point.z);
               GL.End();*/
            /*   if (angulo != 0)
                   angRotacao = angulo;
               else
                   angRotacao = Geom.GetAnguloGlobal(centro.x, centro.y, point.x, point.y);*/

            //    TextoInfo.DesenhaSemZoom(ref Cad, point.x + 4, point.y + 3, "Desloc: " + (this.angRotacao).ToString("n2") + "º");

            ponto2.x = point.x;
            ponto2.y = point.y;
            ponto2.z = point.z;

            // Orto(ref ponto1.x, ref ponto1.y, ref ponto2.x, ref ponto2.y);
            foreach (TObjetoDesenho obj in Objetos)
                (obj).Mover(ref ponto1, ref ponto2, true);
        }

        public string GetComando(int numeroComando)
        {
            Comando = ListaComandos[numeroComando];
            return ListaComandos[numeroComando];
        }
        public void Continue()
        {
          
        }
        public bool ApagarSelecionados
        {
            get { return false; }
        }

        public string TipoObjetoParaSelecionar
        {
            get { return "TODOS"; }
        }
        public string LayerObjetoParaSelecionar
        {
            get { return "TODOS"; }
        }
        public eEditToolTipoSelecao editToolTipoSelecao
        {
            get { return eEditToolTipoSelecao.multiSelecao; }
        }
        [NonSerialized]
        public Pen mPen = new Pen(Color.White);
        public double angRotacao;
        public TPonto ponto2;

        public void OnMouseUp(ref TPonto point)
        {
        }
        public void OnKeyDown(System.Windows.Forms.KeyEventArgs e)
        {
        }
        public void Finished()
        {
        }
        public void Undo()
        {
        }
        public void Redo()
        {
        }
    }
    public class TCopiarElementos: IEditTool
    {
        public List<string> ListaComandos;
        bool DefinindoPontoBase = false;
        bool DefinindoPosicoes  = false;
        public int NumCopias =1;
        public string Comando;
        public List<TObjetoDesenho> Objetos;
        TTexto TextoInfo;
        int pavimento;
        public TPonto ponto1,ponto2;

        public TCopiarElementos()
        {
            ListaComandos = new List<string>();
            ListaComandos.Add(Const.COPIAR_COMANDO_1);
            ListaComandos.Add(Const.COPIAR_COMANDO_2);
            ListaComandos.Add(Const.COPIAR_COMANDO_3);
        }

        public IEditTool Clone()
        {
            return new TCopiarElementos();
        }

        public void Initialize(TObjetoDesenho objeto, ref TPonto point, ref string command, ISettings settings, TLayer layer)
        {
            // command = Const.CMD_PILAR_ROTACIONAR_1_P;

            // this.trecho = objeto as TTrechoViga;
            // ComandoSolicitado = 0;

        }
        public void AtualizaRepeticoes(int rep)
        {
          /*    List<TObjetoDesenho> objetosTemp = new List<TObjetoDesenho>();

              if (Objetos != null)
              {
                  int id = 0;
                  foreach (TObjetoDesenho ob in Objetos)
                  {
                      id++;
                      ob.ID = id;
                      for (int i = 0; i < rep; i++)
                      {
                          objetosTemp.Add(ob.Clone());
                          objetosTemp[objetosTemp.Count - 1].ID = ob.ID;
                      }
                  }

                  Objetos.Clear();
                  foreach (TObjetoDesenho ob in objetosTemp)
                  {
                      Objetos.Add(ob.Clone());
                      Objetos[Objetos.Count - 1].ID = ob.ID;
                      Objetos[Objetos.Count - 1].SetaSelecao(false, false);
                      Objetos[Objetos.Count - 1].ObjetoCopia = true;
                  }
              }*/
        }

        public eObjetoDesenhoMouseDown OnMouseDown(Gerenciador gerenciador, FPrincipal desenho, ref List<TObjetoDesenho> ObjetosSelecionados, ref List<TObjetoDesenho> ObjetosResultado, ref TPonto point, ref string command, List<TLinha> Linhas, List<TPonto> Pontos, bool FromGrip = false)
        {
            if (Comando == ListaComandos[0])
            {
                ponto1 = new TPonto(0);
                ponto2 = new TPonto(0);

                pavimento = desenho.PavimentoAtual;
                if (desenho.edNumRepeticoes.Value > 0)
                {
                    if (ObjetosSelecionados.Count() ==  0)
                        command = "Nenhum objeto selecionado";
                    else
                    if (ObjetosSelecionados.Count() > 1)
                        command = GetComando(1) + " <" + ObjetosSelecionados.Count().ToString() + " objetos selecionados>";
                    else
                        command = GetComando(1) + " <" + ObjetosSelecionados.Count().ToString() + " objeto selecionado>";
                }
                return eObjetoDesenhoMouseDown.Continue;
            }
            else
            if (Comando == ListaComandos[1])
            {
                this.ponto1.x = point.x;
                this.ponto1.y = point.y;
                this.ponto1.z = point.z;
                //Objetos = new List<TObjetoDesenho>(ObjetosSelecionados);
                var new1 = new List<TObjetoDesenho>(ObjetosSelecionados.Select(x => x.Clone()));
                Objetos = new1;
                // Objetos = ObjetosSelecionados.ConvertAll(o=> new TObjetoDesenho());// ObjetosSelecionados.Where(o => o.Tipo != Const.ID_PONTO).ToList();
                Objetos.RemoveAll(o => o.Tipo == Const.ID_PONTO);
                Objetos.RemoveAll(o => o.Tipo == Const.ID_CARGA_LINEAR);
                Objetos.RemoveAll(o => o.Tipo == Const.ID_CARGA_PONTUAL);
                Objetos.RemoveAll(o => o.Tipo == Const.ID_CARGA_MOMENTO);
                Objetos.RemoveAll(o => o.Tipo == Const.ID_CARGA_AREA);
                Objetos.ForEach(o => o.Selecionado = false);

                // foreach (TObjetoDesenho obj in ObjetosSelecionados)
                // {
                //Objetos.Add(obj.Clone());

                //  Objetos[Objetos.Count - 1].SetaSelecao(false, false);
                // Objetos[Objetos.Count - 1].ObjetoCopia = true;
                //}

                if (desenho.edNumRepeticoes.Value > 0)
                  command = GetComando(2);

                return eObjetoDesenhoMouseDown.Continue;
            }
            else
            if (Comando == ListaComandos[2])
            {
               // foreach (TObjetoDesenho obj in ObjetosSelecionados)
                //    obj.SetaSelecao(false, false);
               
                ObjetosSelecionados.ForEach(o => o.SetaSelecao(false,false));

                OnMouseMove(ref point,false);
               // gerenciador.formDesenho.textBox4.Text = point.x.ToString("n3");
                ObjetosResultado = new List<TObjetoDesenho>(Objetos);

                if (desenho.chCopiarCargas.Checked)
                {
                    List<TBarraGenerica> barrasNovas = ObjetosResultado.FindAll(x => x.Tipo == Const.ID_BARRAGENERICA).Cast<TBarraGenerica>().ToList();
                    List<TCargaLinear> cargas;
                    //copia as cargas
                    int pb = desenho.Estrutura.barras.Max(o => o.IDBarra);
                    foreach (TBarraGenerica bn in barrasNovas)
                    {
                        cargas = new List<TCargaLinear>(desenho.Estrutura.cargaLinear.FindAll(x => x.idBarra == bn.idBarraCopiada && x.Dados.idCaso != 1));
                        
                        pb++;

                        foreach (TCargaLinear t in cargas)
                        {
                            TCargaLinear carganova = new TCargaLinear(bn.pIni, bn.pFin, (TDadosCarga)t.Dados.Clone(),
                                           bn.Dados.anguloRotacao, pb, bn.layer);

                            ObjetosResultado.Add(carganova);
                        }
                    }
                }



                //  foreach (TObjetoDesenho obj in Objetos)
                //  {
                //   obj.Atualiza(pavimento);
                // obj.ObjetoCopia = false;
                //  ObjetosResultado.Add(obj);
                // }


                command = "";
                return eObjetoDesenhoMouseDown.DoneRepeat;
            }
            else
                return eObjetoDesenhoMouseDown.Continue;
        }
        public string GetComando(int numeroComando)
        {
            Comando = ListaComandos[numeroComando];
            return ListaComandos[numeroComando];
        }
        public void Continue()
        {
            DefinindoPontoBase = true;
        }
        public bool ApagarSelecionados
        {
            get { return false; }
        }

        public string TipoObjetoParaSelecionar
        {
            get { return "TODOS"; }
        }
        public string LayerObjetoParaSelecionar
        {
            get { return "TODOS"; }
        }
        public eEditToolTipoSelecao editToolTipoSelecao
        {
            get { return eEditToolTipoSelecao.multiSelecao; }
        }
        [NonSerialized]
        public Pen mPen = new Pen(Color.White);
        public double angRotacao, intervalo;
        int rep, id_antes, id;

        public void DesenhaSeta()
        {
            GL.Color3(Color.Red);
            GL.Begin(PrimitiveType.Lines);
            GL.Vertex3(ponto1.x, ponto1.y, ponto1.z);
            GL.Vertex3(ponto2.x, ponto2.y, ponto2.z);
            GL.End();
        }

        public void OnMouseMove(ref TPonto point, bool ShowInfo, bool orto = true, double angulo = 0, bool PontoInicial = false)
        {
              //  GL.Color3(Color.Red);
              //  GL.Begin(PrimitiveType.Lines);
              //  GL.Vertex3(ponto1.x, ponto1.y, ponto1.z);
              //  GL.Vertex3(point.x, point.y, point.z);
              //  GL.End();
                //    Cad.DrawLine(mPen, Desenho.pixelX(ponto1.x), Desenho.pixelY(ponto1.y), Desenho.pixelX(point.x), Desenho.pixelY(point.y));
             /*   if (angulo != 0)
                    angRotacao = angulo;
                else
                    angRotacao = Geom.GetAnguloGlobal(centro.x, centro.y, point.x, point.y);*/

            //    TextoInfo.DesenhaSemZoom(ref Cad, point.x + 4, point.y + 3, "Desloc: " + (this.angRotacao).ToString("n2") + "º");
                this.ponto2.x = point.x;
                this.ponto2.y = point.y;
                this.ponto2.z = point.z;
            /*  rep = 0;
              id_antes = Objetos[0].ID;
              for (int k = 0; k < Objetos.Count; k++)
              {
                  id = Objetos[k].ID;

                  foreach (TObjetoDesenho ob in Objetos)
                      if (ob.ID == id)
                          pontoInicial = ob.pFin;

                  if (id == id_antes)
                      rep++;
                  else
                      rep = 1;

                  pontoFinal.x = pontoInicial.x + ((intervalo * (Math.Cos(angulo * Const.PIDiv180))) * (rep));
                  pontoFinal.y = pontoInicial.y + ((intervalo * (Math.Sin(angulo * Const.PIDiv180))) * (rep));

                  Objetos[k].Mover(pontoInicial, pontoFinal);

                  id_antes = id;
              }*/




          //  Orto(ref ponto1.x, ref ponto1.y, ref ponto2.x, ref ponto2.y);
            foreach (TObjetoDesenho obj in Objetos)
              (obj).Mover(ref ponto1, ref ponto2, true);
        }

        double DifCoordX, DifCoordY, alfa;
        public void Orto(ref double xini, ref double yini, ref double xfin, ref double yfin)
        {
            //     if (pFin.Snap)
            //     MessageBox.Show("");
            alfa = (float)(FuncoesGerais.atand((yini - yfin) / (xini - xfin)));

            if (ConfiguracoesCaptura.Captura.OrtogonalLigado)
            {
                if (alfa > 88)
                {
                    alfa = 90;
                    xfin = xini;
                }
                else
                if (alfa > 0 && alfa < 2)
                  yfin = yini;
            };

            DifCoordX = Math.Abs(xini - xfin);
            DifCoordY = Math.Abs(yini - yfin);

            for (int i = 0; i < ConfiguracoesCaptura.Captura.OutrosAngulos.Count; i++)
            {
                if (alfa >= (ConfiguracoesCaptura.Captura.OutrosAngulos[i] - 5) && alfa <= (ConfiguracoesCaptura.Captura.OutrosAngulos[i] + 5))
                {
                    alfa = ConfiguracoesCaptura.Captura.OutrosAngulos[i];
                    
                    if (yfin > yini)
                        yfin = (yini + (float)(Math.Tan(alfa * Const.PIDiv180) * DifCoordX));
                    else
                        yfin = (yini - (float)(Math.Tan(alfa * Const.PIDiv180) * DifCoordX));
                };
            };
        }
        public void OnMouseUp(ref TPonto point)
        {
        }
        public void OnKeyDown(System.Windows.Forms.KeyEventArgs e)
        {
        }
        public void Finished()
        {
        }
        public void Undo()
        {
        }
        public void Redo()
        {
        }
    }
    public class TRotacionarElementos : IEditTool
    {
        public List<string> ListaComandos;
        public int NumCopias = 1;
        public string Comando;
        public List<TObjetoDesenho> Objetos;
        public TPonto ponto1, ponto2;
        public double angulo;

        public TRotacionarElementos()
        {
            ListaComandos = new List<string>();
            ListaComandos.Add(Const.ROTACIONAR_COMANDO_1);
            ListaComandos.Add(Const.ROTACIONAR_COMANDO_2);
            ListaComandos.Add(Const.ROTACIONAR_COMANDO_3);
        }

        public IEditTool Clone()
        {
            return new TRotacionarElementos();
        }

        public void Initialize(TObjetoDesenho objeto, ref TPonto point, ref string command, ISettings settings, TLayer layer)
        {
            // command = Const.CMD_PILAR_ROTACIONAR_1_P;

            // this.trecho = objeto as TTrechoViga;
            // ComandoSolicitado = 0;

        }
        double mod1, mod2, dot;
        public eObjetoDesenhoMouseDown OnMouseDown(Gerenciador gerenciador, FPrincipal desenho, ref List<TObjetoDesenho> ObjetosSelecionados, ref List<TObjetoDesenho> ObjetosResultado, ref TPonto point, ref string command, List<TLinha> Linhas, List<TPonto> Pontos, bool FromGrip = false)
        {
            if (Comando == ListaComandos[0])
            {
                ponto1 = new TPonto(0);
                ponto2 = new TPonto(0);

                if (desenho.edNumRepeticoes.Value > 0)
                {
                    if (ObjetosSelecionados.Count() == 0)
                        command = "Nenhum objeto selecionado";
                    else
                    if (ObjetosSelecionados.Count() > 1)
                        command = GetComando(1) + " <" + ObjetosSelecionados.Count().ToString() + " objetos selecionados>";
                    else
                        command = GetComando(1) + " <" + ObjetosSelecionados.Count().ToString() + " objeto selecionado>";
                }
                return eObjetoDesenhoMouseDown.Continue;
            }
            else
            if (Comando == ListaComandos[1])
            {
                this.ponto1.x = point.x;
                this.ponto1.y = point.y;
                this.ponto1.z = point.z;
                //Objetos = new List<TObjetoDesenho>(ObjetosSelecionados);
                var new1 = new List<TObjetoDesenho>(ObjetosSelecionados.Select(x => x.Clone()));
                Objetos = new1;
                // Objetos = ObjetosSelecionados.ConvertAll(o=> new TObjetoDesenho());// ObjetosSelecionados.Where(o => o.Tipo != Const.ID_PONTO).ToList();
                Objetos.RemoveAll(o => o.Tipo == Const.ID_PONTO);
                Objetos.ForEach(o => o.Selecionado = false);

                command = GetComando(2);

                return eObjetoDesenhoMouseDown.Continue;
            }
            else
            if (Comando == ListaComandos[2])
            {
                // foreach (TObjetoDesenho obj in ObjetosSelecionados)
                //    obj.SetaSelecao(false, false);
                this.ponto2.x = point.x;
                this.ponto2.y = point.y;
                this.ponto2.z = point.z;

                angulo = System.Convert.ToDouble(gerenciador.formDesenho.edAnguloRotacao.Text);
                TBarraGenerica bar;

                vec3 p1_eixorotacao = new vec3(ponto1.x, -ponto1.y, -ponto1.z);
                vec3 p2_eixorotacao = new vec3(ponto2.x, -ponto2.y, -ponto2.z);
                vec3 pivo = new vec3(ponto1.x, -ponto1.y, -ponto1.z);
                vec3 p_;

                p1_eixorotacao.x -= pivo.x;
                p1_eixorotacao.y -= pivo.y;
                p1_eixorotacao.z -= pivo.z;

                p2_eixorotacao.x -= pivo.x;
                p2_eixorotacao.y -= pivo.y;
                p2_eixorotacao.z -= pivo.z;

                vec3 vecRotacao = p2_eixorotacao - p1_eixorotacao;
                vecRotacao.Normalize();

                List<TBarraGenerica> novasBarras = new List<TBarraGenerica>();
                double angInc = 0;
                if ((int)gerenciador.formDesenho.qtdRepeticoesRotacao.Value > 0)
                {
                    for (int i = 0; i < (int)gerenciador.formDesenho.qtdRepeticoesRotacao.Value; i++)
                    {
                        angInc += angulo;

                        foreach (TObjetoDesenho obj in ObjetosSelecionados)
                        {
                            if (obj.Tipo == Const.ID_BARRAGENERICA)
                            {
                                bar = (TBarraGenerica)(obj as TBarraGenerica).Clone();
                     
                                pAposRotacao = new vec3(bar.seta_eixo_local_Y.l_principal.p2.x, bar.seta_eixo_local_Y.l_principal.p2.y*-1, bar.seta_eixo_local_Y.l_principal.p2.z*-1);

                                p_ = new vec3(bar.pFin.x, -bar.pFin.y, -bar.pFin.z);
                                Geom.RotacionaVetor(ref p_, angInc, pivo, vecRotacao, p1_eixorotacao);
                                bar.pFin.x = p_.x; bar.pFin.y = p_.y; bar.pFin.z = p_.z;
                                p_ = new vec3(bar.pIni.x, -bar.pIni.y, -bar.pIni.z);
                                Geom.RotacionaVetor(ref p_, angInc, pivo, vecRotacao, p1_eixorotacao);
                                bar.pIni.x = p_.x; bar.pIni.y = p_.y; bar.pIni.z = p_.z;
                                novasBarras.Add(bar);
                                bar.OrientaSecaoNoEspaco();
                                bar.CriarEixosLocais((double)gerenciador.ConfiguracoesPGi.F3DOpcoesVisualizacao.TamArt / 100 / 2);

                                pCentroRotacao = new vec3(bar.seta_eixo_local_Y.l_principal.p1.x, bar.seta_eixo_local_Y.l_principal.p1.y * -1, bar.seta_eixo_local_Y.l_principal.p1.z * -1);

                                p12 = new vec3(bar.seta_eixo_local_Y.l_principal.p2.x, bar.seta_eixo_local_Y.l_principal.p2.y * -1, bar.seta_eixo_local_Y.l_principal.p2.z * -1);

                                Geom.RotacionaVetor(ref pAposRotacao, angInc, pivo, vecRotacao, p1_eixorotacao);
                                pAposRotacao *= -1;
                                pAposRotacao.x *= -1;

                                p23 = (pAposRotacao - pCentroRotacao);
                             
                                p24 = (p12 - pCentroRotacao);

                               /* mod1 = p23.Magnitude();
                                mod2 = p24.Magnitude();

                                dot = p24.DotProduct(p23);
                                angto = dot / (mod1 * mod2);
                                anguloGrau = Math.Acos(angto) * 57.2958;
                                if (double.IsNaN(anguloGrau))
                                    anguloGrau = 0;
                                */
                                anguloGrau = Geom.AnguloEntre2Vetores(p23, p24);

                                if (!Geom.Iguais(anguloGrau, 0))
                                {
                                    rotacionasecao(ref bar, anguloGrau);
                                    bar.OrientaSecaoNoEspaco();
                                    bar.CriarEixosLocais((double)gerenciador.ConfiguracoesPGi.F3DOpcoesVisualizacao.TamArt / 100 / 2);

                                   // if ((coordsecao_inicial_antihorario != coordsecao_final_antihorario))
                                   //     p12 = new vec3(bar.CoordsSecao_i[1].x, bar.CoordsSecao_i[1].y, bar.CoordsSecao_i[1].z);
                                 //   else
                                    p12 = new vec3(bar.seta_eixo_local_Y.l_principal.p2.x, bar.seta_eixo_local_Y.l_principal.p2.y*-1, bar.seta_eixo_local_Y.l_principal.p2.z*-1);

                                    //avalio se o ponto correto apos a rotação coincide com umas das coordenas da secao, senao eu rodo pro outro lado
                                    if ((Geom.Iguais(bar.seta_eixo_local_Y.l_principal.p2.x, pAposRotacao.x) && Geom.Iguais(bar.seta_eixo_local_Y.l_principal.p2.y*-1, pAposRotacao.y) && Geom.Iguais(bar.seta_eixo_local_Y.l_principal.p2.z*-1, pAposRotacao.z)))
                                       // (Geom.Iguais(bar.CoordsSecao_i[1].x, pAposRotacao.x) && Geom.Iguais(bar.CoordsSecao_i[1].y, pAposRotacao.y) && Geom.Iguais(bar.CoordsSecao_i[1].z, pAposRotacao.z)) ||
                                        //(Geom.Iguais(bar.CoordsSecao_i[2].x, pAposRotacao.x) && Geom.Iguais(bar.CoordsSecao_i[2].y, pAposRotacao.y) && Geom.Iguais(bar.CoordsSecao_i[2].z, pAposRotacao.z)))
                                    {
                                        dot2 = 0; // teste de parada
                                    }
                                    else
                                    if (!Geom.Iguais(p12.x, pAposRotacao.x) || !Geom.Iguais(p12.y, pAposRotacao.y) || !Geom.Iguais(p12.z, pAposRotacao.z))
                                    {
                                        if (Geom.Iguais(bar.Dados.anguloRotacao - (2 * anguloGrau), 360))
                                        {
                                            bar.Dados.anguloRotacao = 0;
                                            rotacionasecao(ref bar, 0);
                                        }
                                        else
                                        {
                                            rotacionasecao(ref bar, -(2 * anguloGrau));
                                        }
                                    }
                                    else
                                    {
                                        rotacionasecao(ref bar, -bar.Dados.anguloRotacao);
                                    }
                                }
                            }
                        }
                    }

                    foreach (TBarraGenerica obj in novasBarras)
                       ObjetosResultado.Add(obj);

                    if (gerenciador.formDesenho.chCopiarCargasRotacao.Checked)
                    {
                        List<TBarraGenerica> barrasNovas = ObjetosResultado.FindAll(x => x.Tipo == Const.ID_BARRAGENERICA).Cast<TBarraGenerica>().ToList();
                        List<TCargaLinear> cargas;
                        //copia as cargas
                        int pb = desenho.Estrutura.barras.Max(o => o.IDBarra);
                        foreach (TBarraGenerica bn in barrasNovas)
                        {
                            cargas = new List<TCargaLinear>(desenho.Estrutura.cargaLinear.FindAll(x => x.idBarra == bn.idBarraCopiada && x.Dados.idCaso != 1));

                            pb++;

                            foreach (TCargaLinear t in cargas)
                            {
                                TCargaLinear carganova = new TCargaLinear(bn.pIni, bn.pFin, (TDadosCarga)t.Dados.Clone(),
                                               bn.Dados.anguloRotacao, pb, bn.layer);

                                ObjetosResultado.Add(carganova);
                            }
                        }
                    }
                }
                else
                {

                    foreach (TObjetoDesenho obj in ObjetosSelecionados)
                    {
                        if (obj.Tipo == Const.ID_BARRAGENERICA)
                        {
                            bar = obj as TBarraGenerica;
                            pAposRotacao = new vec3(bar.seta_eixo_local_Y.l_principal.p2.x, bar.seta_eixo_local_Y.l_principal.p2.y * -1, bar.seta_eixo_local_Y.l_principal.p2.z * -1);
                            pCentroRotacao = new vec3(bar.seta_eixo_local_Y.l_principal.p1.x, bar.seta_eixo_local_Y.l_principal.p1.y * -1, bar.seta_eixo_local_Y.l_principal.p1.z * -1);

                            vec3 ppp = (pAposRotacao - pCentroRotacao);
                            Geom.RotacionaVetor(ref ppp, 1, pivo, vecRotacao, p1_eixorotacao);

                            normaloriginal = ppp.CrossProduct(pAposRotacao - pCentroRotacao).Normalize();
                            
                            /*------------------------------------------*/
                            p_ = new vec3(bar.pFin.x, -bar.pFin.y, -bar.pFin.z);
                            Geom.RotacionaVetor(ref p_, angulo, pivo, vecRotacao, p1_eixorotacao);
                            bar.pFin.x = p_.x; bar.pFin.y = p_.y; bar.pFin.z = p_.z;
                            p_ = new vec3(bar.pIni.x, -bar.pIni.y, -bar.pIni.z);
                            Geom.RotacionaVetor(ref p_, angulo, pivo, vecRotacao, p1_eixorotacao);
                            bar.pIni.x = p_.x; bar.pIni.y = p_.y; bar.pIni.z = p_.z;
                            bar.OrientaSecaoNoEspaco();
                            bar.CriarEixosLocais((double)gerenciador.ConfiguracoesPGi.F3DOpcoesVisualizacao.TamArt / 100 / 2);
                            pCentroRotacao = new vec3(bar.seta_eixo_local_Y.l_principal.p1.x, bar.seta_eixo_local_Y.l_principal.p1.y * -1, bar.seta_eixo_local_Y.l_principal.p1.z * -1);

                            p12 = new vec3(bar.seta_eixo_local_Y.l_principal.p2.x, bar.seta_eixo_local_Y.l_principal.p2.y * -1, bar.seta_eixo_local_Y.l_principal.p2.z * -1);

                            Geom.RotacionaVetor(ref pAposRotacao, angulo, pivo, vecRotacao, p1_eixorotacao);
                            pAposRotacao *= -1;
                            pAposRotacao.x *= -1;
                            p23 = (pAposRotacao - pCentroRotacao);
                            p24 = (p12 - pCentroRotacao);
                           
                            mod1 = p23.Magnitude();
                            mod2 = p24.Magnitude();

                            dot = p24.DotProduct(p23);
                            normalNova = p24.CrossProduct(p23).Normalize();
                            angto =  dot / (mod1 * mod2);
                            anguloGrau = Math.Acos(angto) * 57.2958;
                            if (double.IsNaN(anguloGrau))
                                anguloGrau = 0;
                          
                            dot2  = normaloriginal.DotProduct(normalNova);

                            if (!Geom.Iguais(anguloGrau, 0))
                            {
                                rotacionasecao(ref bar, anguloGrau);
                                bar.OrientaSecaoNoEspaco();
                                bar.CriarEixosLocais((double)gerenciador.ConfiguracoesPGi.F3DOpcoesVisualizacao.TamArt / 100 / 2);

                                p12 = new vec3(bar.seta_eixo_local_Y.l_principal.p2.x, bar.seta_eixo_local_Y.l_principal.p2.y * -1, bar.seta_eixo_local_Y.l_principal.p2.z * -1);

                                //avalio se o ponto correto apos a rotação coincide com umas das coordenas da secao, senao eu rodo pro outro lado
                                if ((Geom.Iguais(bar.seta_eixo_local_Y.l_principal.p2.x, pAposRotacao.x) && Geom.Iguais(bar.seta_eixo_local_Y.l_principal.p2.y * -1, pAposRotacao.y) && Geom.Iguais(bar.seta_eixo_local_Y.l_principal.p2.z * -1, pAposRotacao.z)))
                                // (Geom.Iguais(bar.CoordsSecao_i[1].x, pAposRotacao.x) && Geom.Iguais(bar.CoordsSecao_i[1].y, pAposRotacao.y) && Geom.Iguais(bar.CoordsSecao_i[1].z, pAposRotacao.z)) ||
                                //(Geom.Iguais(bar.CoordsSecao_i[2].x, pAposRotacao.x) && Geom.Iguais(bar.CoordsSecao_i[2].y, pAposRotacao.y) && Geom.Iguais(bar.CoordsSecao_i[2].z, pAposRotacao.z)))
                                {
                                    dot2 = 0; // teste de parada
                                }
                                else
                                if (!Geom.Iguais(p12.x, pAposRotacao.x) || !Geom.Iguais(p12.y, pAposRotacao.y) || !Geom.Iguais(p12.z, pAposRotacao.z))
                                {
                                    if (Geom.Iguais(bar.Dados.anguloRotacao - (2 * anguloGrau), 360))
                                    {
                                        bar.Dados.anguloRotacao = 0;
                                        rotacionasecao(ref bar, 0);
                                    }
                                    else
                                        rotacionasecao(ref bar, -(2 * anguloGrau));
                                }
                                else
                                {
                                    rotacionasecao(ref bar, -bar.Dados.anguloRotacao);
                                }
                            }
                        }
                    }
                }

                command = "";

                return eObjetoDesenhoMouseDown.DoneRepeat;
            }
            else
                return eObjetoDesenhoMouseDown.Continue;
        }
        vec3 pCentroRotacao, p12, pAposRotacao, p23, p24, normalNova,rod, normaloriginal;
        double angto,cz, cosa, dot2, anguloGrau;
        void rotacionasecao(ref TBarraGenerica b, double _angulo)
        {
            TSecao secaoCopia;
            vec3 centroRotacao = new vec3(0, 0, 0);
            vec3 coord1 = new vec3(0, 0, 0);
            double anguloRotacaoSecao = 0;
            TBarraGenerica br = b;

            secaoCopia = (TSecao)br.Dados.secao.Clone();

            anguloRotacaoSecao = _angulo + b.Dados.anguloRotacao;

            if (Geom.Iguais(anguloRotacaoSecao, 360))
                anguloRotacaoSecao = 0;

            if (!Geom.Iguais(anguloRotacaoSecao, 180))
                if (anguloRotacaoSecao > 180)
                   anguloRotacaoSecao -= 360;

            if (Geom.Iguais(anguloRotacaoSecao, -180))
                anguloRotacaoSecao = 180;

            if (!Geom.Iguais(anguloRotacaoSecao, -180))
                if (anguloRotacaoSecao < -180)
                    anguloRotacaoSecao += 360;

            if (Geom.Iguais(anguloRotacaoSecao, 0))
                anguloRotacaoSecao = 0;

            if (secaoCopia.poligonos != null)
            {
         //       secaoCopia.poligono.CalculaPropriedades();

                centroRotacao.y = 0;
                centroRotacao.x = 0;

                for (int q = 0; q < (secaoCopia.poligonos.Count()); q++)
                {
                    for (int i = 0; i < (secaoCopia.poligonos[q].coords.Count()); i++)
                    {
                        coord1 = new vec3(secaoCopia.poligonos[q].coords[i].X, secaoCopia.poligonos[q].coords[i].Y, 0);
                        coord1 = coord1.Rotate(centroRotacao, (anguloRotacaoSecao) * Const.PIDiv180);

                        secaoCopia.poligonos[q].coords[i].X = coord1.x;
                        secaoCopia.poligonos[q].coords[i].Y = coord1.y;
                    }
                }

                b.Dados.secao = (TSecao)secaoCopia.Clone();
                b.Dados.anguloRotacao = anguloRotacaoSecao;
            }
        }

        public string GetComando(int numeroComando)
        {
            Comando = ListaComandos[numeroComando];
            return ListaComandos[numeroComando];
        }
        public void Continue()
        {

        }

        public bool ApagarSelecionados
        {
            get 
            {
                return false; 
            }
        }

        public string TipoObjetoParaSelecionar
        {
            get { return "TODOS"; }
        }
        public string LayerObjetoParaSelecionar
        {
            get { return "TODOS"; }
        }
        public eEditToolTipoSelecao editToolTipoSelecao
        {
            get { return eEditToolTipoSelecao.multiSelecao; }
        }
        [NonSerialized]
        public Pen mPen = new Pen(Color.White);
        public double angRotacao, intervalo;
        int rep, id_antes, id;

        public void DesenhaSeta()
        {
              GL.Color3(Color.Red);
              GL.Begin(PrimitiveType.Lines);
              GL.Vertex3(ponto1.x, ponto1.y, ponto1.z);
              GL.Vertex3(ponto2.x, ponto2.y, ponto2.z);
              GL.End();
        }

        public void OnMouseMove(ref TPonto point, bool ShowInfo, bool orto = true, double angulo = 0, bool PontoInicial = false)
        {
            /*  GL.Color3(Color.Red);
              GL.Begin(PrimitiveType.Lines);
              GL.Vertex3(ponto1.x, ponto1.y, ponto1.z);
              GL.Vertex3(point.x, point.y, point.z);
              GL.End();*/
            //    Cad.DrawLine(mPen, Desenho.pixelX(ponto1.x), Desenho.pixelY(ponto1.y), Desenho.pixelX(point.x), Desenho.pixelY(point.y));
                            /*   if (angulo != 0)
                                   angRotacao = angulo;
                               else
                                   angRotacao = Geom.GetAnguloGlobal(centro.x, centro.y, point.x, point.y);*/

                            //    TextoInfo.DesenhaSemZoom(ref Cad, point.x + 4, point.y + 3, "Desloc: " + (this.angRotacao).ToString("n2") + "º");
            this.ponto2.x = point.x;
            this.ponto2.y = point.y;
            this.ponto2.z = point.z;
            /*  rep = 0;
              id_antes = Objetos[0].ID;
              for (int k = 0; k < Objetos.Count; k++)
              {
                  id = Objetos[k].ID;

                  foreach (TObjetoDesenho ob in Objetos)
                      if (ob.ID == id)
                          pontoInicial = ob.pFin;

                  if (id == id_antes)
                      rep++;
                  else
                      rep = 1;

                  pontoFinal.x = pontoInicial.x + ((intervalo * (Math.Cos(angulo * Const.PIDiv180))) * (rep));
                  pontoFinal.y = pontoInicial.y + ((intervalo * (Math.Sin(angulo * Const.PIDiv180))) * (rep));

                  Objetos[k].Mover(pontoInicial, pontoFinal);

                  id_antes = id;
              }*/




            //  Orto(ref ponto1.x, ref ponto1.y, ref ponto2.x, ref ponto2.y);
          //  foreach (TObjetoDesenho obj in Objetos)
            //    (obj).Mover(ref ponto1, ref ponto2, true);
        }

        double DifCoordX,  alfa;
        public void OnMouseUp(ref TPonto point)
        {
        }
        public void OnKeyDown(System.Windows.Forms.KeyEventArgs e)
        {
        }
        public void Finished()
        {
        }
        public void Undo()
        {
        }
        public void Redo()
        {
        }
    }
    public class TEspelharElementos : IEditTool
    {
        public List<string> ListaComandos;
        public int NumCopias = 1;
        public string Comando;
        public List<TObjetoDesenho> Objetos;
        public TPonto ponto1, ponto2, ponto3;

        public TEspelharElementos()
        {
            ListaComandos = new List<string>();
            ListaComandos.Add(Const.ESPELHAR_COMANDO_1);
            ListaComandos.Add(Const.ESPELHAR_COMANDO_2);
            ListaComandos.Add(Const.ESPELHAR_COMANDO_3);
            ListaComandos.Add(Const.ESPELHAR_COMANDO_4);
        }

        public IEditTool Clone()
        {
            return new TEspelharElementos();
        }
        vec3 p1, p2, p3, p4, p5,p6, normal;
        public void Initialize(TObjetoDesenho objeto, ref TPonto point, ref string command, ISettings settings, TLayer layer)
        {
        }
        public void DesenhaSeta()
        {
             GL.Color3(Color.Red);
             GL.Begin(PrimitiveType.Lines);
             GL.Vertex3(ponto1.x, ponto1.y, ponto1.z);
             GL.Vertex3(ponto2.x, ponto2.y, ponto2.z);
             GL.End();
             GL.Begin(PrimitiveType.Lines);
             GL.Vertex3(ponto2.x, ponto2.y, ponto2.z);
             GL.Vertex3(ponto3.x, ponto3.y, ponto3.z);
             GL.End();
            ///**/////////////***//////////**//////
            ///
            p1 = new vec3(ponto1.x, ponto1.y, ponto1.z);
            p2 = new vec3(ponto2.x, ponto2.y, ponto2.z);
            p3 = new vec3(ponto2.x, ponto2.y, ponto2.z);
            p4 = new vec3(ponto3.x, ponto3.y, ponto3.z);

            p5 = p2 - p1;
            p6 = p4 - p3;

            normal = p5.CrossProduct(p6).Normalize();
            normal.z *= -1;
            normal.y *= -1;

            vec3 posicao = (((p1 + p2) / 2) + (p3 + p4) / 2) / 2;
            posicao.z *= -1;
            posicao.y *= -1;

            TBarraGenerica bar;
            foreach (TObjetoDesenho obj in Objetos)
            {
                if (obj.Tipo == Const.ID_BARRAGENERICA)
                {
                    bar = (TBarraGenerica)(obj);

                    p1 = new vec3(bar.pIni.x, -bar.pIni.y, -bar.pIni.z);
                    p2 = new vec3(bar.pFin.x, -bar.pFin.y, -bar.pFin.z);

                    Geom.EspelhaVetor(normal, p1, posicao);
                    Geom.EspelhaVetor(normal, p2, posicao);

                    GL.Color3(bar.Rgb[0], bar.Rgb[1], bar.Rgb[2]);
                    GL.Begin(PrimitiveType.Lines);
                    GL.Vertex3(p1.x, p1.y, p1.z);
                    GL.Vertex3(p2.x, p2.y, p2.z);
                    GL.End();
                }
            }
        }
        vec3 pAposEspelhar;
        public eObjetoDesenhoMouseDown OnMouseDown(Gerenciador gerenciador, FPrincipal desenho, ref List<TObjetoDesenho> ObjetosSelecionados, ref List<TObjetoDesenho> ObjetosResultado, ref TPonto point, ref string command, List<TLinha> Linhas, List<TPonto> Pontos, bool FromGrip = false)
        {
            if (Comando == ListaComandos[0])
            {
                ponto1 = new TPonto(0);
                ponto2 = new TPonto(0);
                ponto3 = new TPonto(0);

                if (ObjetosSelecionados.Count() == 0)
                    command = "Nenhum objeto selecionado";
                else
                if (ObjetosSelecionados.Count() > 1)
                    command = GetComando(1) + " <" + ObjetosSelecionados.Count().ToString() + " objetos selecionados>";
                else
                    command = GetComando(1) + " <" + ObjetosSelecionados.Count().ToString() + " objeto selecionado>";

                return eObjetoDesenhoMouseDown.Continue;
            }
            else
            if (Comando == ListaComandos[1])
            {
                this.ponto1.x = point.x;
                this.ponto1.y = point.y;
                this.ponto1.z = point.z;

                this.ponto2.x = point.x;
                this.ponto2.y = point.y;
                this.ponto2.z = point.z;

                this.ponto3.x = point.x;
                this.ponto3.y = point.y;
                this.ponto3.z = point.z;

                Objetos = ObjetosSelecionados;
                
                command = GetComando(2);

                return eObjetoDesenhoMouseDown.Continue;

            }
            else
            if (Comando == ListaComandos[2])
            {
                this.ponto2.x = point.x;
                this.ponto2.y = point.y;
                this.ponto2.z = point.z;

                this.ponto3.x = point.x;
                this.ponto3.y = point.y;
                this.ponto3.z = point.z;
                var new1 = new List<TObjetoDesenho>(ObjetosSelecionados.Select(x => x.Clone()));
                Objetos = new1;
                Objetos.ForEach(o => o.Selecionado = false);

                command = GetComando(3);

                return eObjetoDesenhoMouseDown.Continue;
            }
            else
            if (Comando == ListaComandos[3])
            {
                // this.ponto3.x = point.x;
                // this.ponto3.y = point.y;
                //   this.ponto3.z = point.z;

                p1 = new vec3(ponto1.x, ponto1.y, ponto1.z);
                p2 = new vec3(ponto2.x, ponto2.y, ponto2.z);
                p3 = new vec3(ponto2.x, ponto2.y, ponto2.z);
                p4 = new vec3(ponto3.x, ponto3.y, ponto3.z);

                p5 = p2 - p1;
                p6 = p4 - p3;

                normal = p5.CrossProduct(p6).Normalize();
                normal.z *= -1;
                normal.y *= -1;

                vec3 posicao = (((p1 + p2) / 2) + (p3 + p4) / 2) / 2;
                posicao.z *= -1;
                posicao.y *= -1;

                Plano plano = new Plano(normal, posicao, "", false);
                List<TBarraGenerica> novasBarras = new List<TBarraGenerica>();

                TBarraGenerica bar;
                foreach (TObjetoDesenho obj in ObjetosSelecionados)
                {
                    if (obj.Tipo == Const.ID_BARRAGENERICA)
                    {
                        bar = (TBarraGenerica)(obj as TBarraGenerica).Clone();
                        
                        pAposEspelhar = new vec3(bar.seta_eixo_local_Y.l_principal.p2.x, bar.seta_eixo_local_Y.l_principal.p2.y * -1, bar.seta_eixo_local_Y.l_principal.p2.z * -1);

                        p1 = new vec3(bar.pIni.x, -bar.pIni.y, -bar.pIni.z);
                        p2 = new vec3(bar.pFin.x, -bar.pFin.y, -bar.pFin.z);

                        Geom.EspelhaVetor(normal,p1, posicao);
                        Geom.EspelhaVetor(normal,p2, posicao);
                        bar.pIni.x = p1.x;bar.pIni.y = p1.y;bar.pIni.z = p1.z;
                        bar.pFin.x = p2.x; bar.pFin.y = p2.y; bar.pFin.z = p2.z;

                        novasBarras.Add(bar);
                        bar.OrientaSecaoNoEspaco();
                        bar.CriarEixosLocais((double)gerenciador.ConfiguracoesPGi.F3DOpcoesVisualizacao.TamArt / 100 / 2);

                        vec3 pCentroRotacao = new vec3(bar.seta_eixo_local_Y.l_principal.p1.x, bar.seta_eixo_local_Y.l_principal.p1.y * -1, bar.seta_eixo_local_Y.l_principal.p1.z * -1);

                        vec3 p12 = new vec3(bar.seta_eixo_local_Y.l_principal.p2.x, bar.seta_eixo_local_Y.l_principal.p2.y * -1, bar.seta_eixo_local_Y.l_principal.p2.z * -1);
                        
                        Geom.EspelhaVetor(normal, pAposEspelhar, posicao);
                        pAposEspelhar *= -1;
                        pAposEspelhar.x *= -1;

                        vec3 p23 = (pAposEspelhar - pCentroRotacao);

                        vec3 p24 = (p12 - pCentroRotacao);

                        double mod1 = p23.Magnitude();
                        double mod2 = p24.Magnitude();

                        double dot = p24.DotProduct(p23);
                        double angto = dot / (mod1 * mod2);
                        double anguloGrau = Math.Acos(angto) * 57.2958;
                        if (double.IsNaN(anguloGrau))
                           anguloGrau = 0;

                        anguloGrau -= 180;

                        if (!Geom.Iguais(anguloGrau, 0))
                        {
                            rotacionasecao(ref bar, anguloGrau);
                            bar.OrientaSecaoNoEspaco();
                            bar.CriarEixosLocais((double)gerenciador.ConfiguracoesPGi.F3DOpcoesVisualizacao.TamArt / 100 / 2);

                            p12 = new vec3(bar.seta_eixo_local_Y.l_principal.p2.x, bar.seta_eixo_local_Y.l_principal.p2.y * -1, bar.seta_eixo_local_Y.l_principal.p2.z * -1);

                            //avalio se o ponto correto apos a rotação coincide com umas das coordenas da secao, senao eu rodo pro outro lado
                            if ((Geom.Iguais(bar.seta_eixo_local_Y.l_principal.p2.x, pAposEspelhar.x) && Geom.Iguais(bar.seta_eixo_local_Y.l_principal.p2.y * -1, pAposEspelhar.y) && Geom.Iguais(bar.seta_eixo_local_Y.l_principal.p2.z * -1, pAposEspelhar.z)))
                            {
                                double dot2 = 0; // teste de parada
                            }
                            //else
                            /*if (!Geom.Iguais(p12.x, pAposEspelhar.x) || !Geom.Iguais(p12.y, pAposEspelhar.y) || !Geom.Iguais(p12.z, pAposEspelhar.z))
                            {
                                if (Geom.Iguais(bar.Dados.anguloRotacao - (2 * anguloGrau), 360))
                                {
                                    bar.Dados.anguloRotacao = 0;
                                    rotacionasecao(ref bar, 0);
                                }
                                else
                                {
                                    rotacionasecao(ref bar, -(2 * anguloGrau));
                                }
                            }
                            else
                            {
                                rotacionasecao(ref bar, -bar.Dados.anguloRotacao);
                            }*/
                        }

                    }
                }

                foreach (TBarraGenerica obj in novasBarras)
                   ObjetosResultado.Add(obj);

                if (gerenciador.formDesenho.chCopiarCargas_Espelhar.Checked)
                {
                    List<TBarraGenerica> barrasNovas = ObjetosResultado.FindAll(x => x.Tipo == Const.ID_BARRAGENERICA).Cast<TBarraGenerica>().ToList();
                    List<TCargaLinear> cargas;
                    //copia as cargas
                    int pb = desenho.Estrutura.barras.Max(o => o.IDBarra);
                    foreach (TBarraGenerica bn in barrasNovas)
                    {
                        cargas = new List<TCargaLinear>(desenho.Estrutura.cargaLinear.FindAll(x => x.idBarra == bn.idBarraCopiada && x.Dados.idCaso != 1));

                        pb++;

                        foreach (TCargaLinear t in cargas)
                        {
                            TCargaLinear carganova = new TCargaLinear(bn.pIni, bn.pFin, (TDadosCarga)t.Dados.Clone(),
                                           bn.Dados.anguloRotacao, pb, bn.layer);

                            ObjetosResultado.Add(carganova);
                        }
                    }
                }

                command = "";
                return eObjetoDesenhoMouseDown.DoneRepeat;
            }
            else
                return eObjetoDesenhoMouseDown.Continue;
        }

        void rotacionasecao(ref TBarraGenerica b, double _angulo)
        {
            TSecao secaoCopia;
            vec3 centroRotacao = new vec3(0, 0, 0);
            vec3 coord1 = new vec3(0, 0, 0);
            double anguloRotacaoSecao = 0;
            TBarraGenerica br = b;

            secaoCopia = (TSecao)br.Dados.secao.Clone();

            anguloRotacaoSecao = _angulo + b.Dados.anguloRotacao;

            if (Geom.Iguais(anguloRotacaoSecao, 360))
                anguloRotacaoSecao = 0;

            if (!Geom.Iguais(anguloRotacaoSecao, 180))
                if (anguloRotacaoSecao > 180)
                    anguloRotacaoSecao -= 360;

            if (Geom.Iguais(anguloRotacaoSecao, -180))
                anguloRotacaoSecao = 180;

            if (!Geom.Iguais(anguloRotacaoSecao, -180))
                if (anguloRotacaoSecao < -180)
                    anguloRotacaoSecao += 360;

            if (Geom.Iguais(anguloRotacaoSecao, 0))
                anguloRotacaoSecao = 0;

            if (Geom.Iguais(anguloRotacaoSecao, 90))
                anguloRotacaoSecao = 90;

            if (Geom.Iguais(anguloRotacaoSecao, -90))
                anguloRotacaoSecao = -90; 
            
            if (secaoCopia.poligono != null)
            {
                secaoCopia.poligono.CalculaPropriedades();

                centroRotacao.y = secaoCopia.poligono.centroide.Y;
                centroRotacao.x = secaoCopia.poligono.centroide.X;

                if (Geom.Iguais(secaoCopia.poligono.centroide.X, 0))
                    centroRotacao.x = 0;
                if (Geom.Iguais(secaoCopia.poligono.centroide.Y, 0))
                    centroRotacao.y = 0;

                for (int i = 0; i < (secaoCopia.poligono.coords.Count()); i++)
                {
                    coord1 = new vec3(secaoCopia.poligono.coords[i].X, secaoCopia.poligono.coords[i].Y, 0);
                    coord1 = coord1.Rotate(centroRotacao, (anguloRotacaoSecao) * Const.PIDiv180);

                    secaoCopia.poligono.coords[i].X = coord1.x;
                    secaoCopia.poligono.coords[i].Y = coord1.y;
                }

                b.Dados.secao = (TSecao)secaoCopia.Clone();
                b.Dados.anguloRotacao = anguloRotacaoSecao;

            }
        }
        public string GetComando(int numeroComando)
        {
            Comando = ListaComandos[numeroComando];
            return ListaComandos[numeroComando];
        }
        public void Continue()
        {
    
        }
        public bool ApagarSelecionados
        {
            get { return false; }
        }

        public string TipoObjetoParaSelecionar
        {
            get { return "TODOS"; }
        }
        public string LayerObjetoParaSelecionar
        {
            get { return "TODOS"; }
        }
        public eEditToolTipoSelecao editToolTipoSelecao
        {
            get { return eEditToolTipoSelecao.multiSelecao; }
        }
        [NonSerialized]
        public Pen mPen = new Pen(Color.White);
        public double angRotacao, intervalo;
        int rep, id_antes, id;


        public void OnMouseMove(ref TPonto point, bool ShowInfo, bool orto = true, double angulo = 0, bool PontoInicial = false)
        {
            this.ponto3.x = point.x;
            this.ponto3.y = point.y;
            this.ponto3.z = point.z;

        }

        double DifCoordX, DifCoordY, alfa;
        public void OnMouseUp(ref TPonto point)
        {
        }
        public void OnKeyDown(System.Windows.Forms.KeyEventArgs e)
        {
        }
        public void Finished()
        {
        }
        public void Undo()
        {
        }
        public void Redo()
        {
        }
    }
    public class TCopiaPadrao : IEditTool
    {
        public List<string> ListaComandos;
        bool DefinindoPontoBase = false;
        bool DefinindoPosicoes = false;
        public bool visualizarDinamico;
        public string Comando;
        public List<TObjetoDesenho> Objetos, ObjetosCopia;
        TTexto TextoInfo;
        int pavimento;
        public TPonto ponto1;
        public TPonto p1_direcao, p2_direcao;
        public int sentido;
        public int ocorrencias = 1, intervalo;
        public TCopiaPadrao()
        {
            ListaComandos = new List<string>();
            ListaComandos.Add("selecao");
            ListaComandos.Add("direcao");
            Objetos = new List<TObjetoDesenho>();
            ObjetosCopia = new List<TObjetoDesenho>();
            /*ListaComandos.Add("Cópia padrão - Quantidade de repetições");
            ListaComandos.Add("Cópia padrão - Intervalo (cm)");
            ListaComandos.Add("Cópia padrão - Indique uma direção");
            ListaComandos.Add("<a> - Alterna sentido");*/
        }

        public IEditTool Clone()
        {
            return new TCopiaPadrao();
        }

        public void Initialize(TObjetoDesenho objeto, ref TPonto point, ref string command, ISettings settings, TLayer layer)
        {
            // command = Const.CMD_PILAR_ROTACIONAR_1_P;

            // this.trecho = objeto as TTrechoViga;
            // ComandoSolicitado = 0;
        }

        public eObjetoDesenhoMouseDown OnMouseDown(Gerenciador gerenciador, FPrincipal desenho, ref List<TObjetoDesenho> ObjetosSelecionados, ref List<TObjetoDesenho> ObjetosResultado, ref TPonto point, ref string command, List<TLinha> Linhas, List<TPonto> Pontos, bool FromGrip = false)
        {
            if (Comando == "ok")
            {
                ponto1 = new TPonto(0);
                ponto2 = new TPonto(0);

                foreach (TObjetoDesenho ob in ObjetosCopia)
                    ob.SetaSelecao(false, false);

                foreach (TObjetoDesenho ob in Objetos)
                    ob.SetaSelecao(false, false);

                Objetos.Clear();

                pavimento = desenho.PavimentoAtual;

                foreach (TObjetoDesenho ob in ObjetosCopia)
                {
                    ob.Atualiza(pavimento); 
                    ObjetosResultado.Add(ob);
                }
                return eObjetoDesenhoMouseDown.DoneRepeat;
            }
            else
            if (Comando == "direcao p1")
            {
                Comando = "direcao p2";
                p1_direcao = point;
                return eObjetoDesenhoMouseDown.Continue;
            }
            else
            if (Comando == "direcao p2")
            {
                p2_direcao = point;
                sentido = 1;
                return eObjetoDesenhoMouseDown.Continue;
            }
            else
                return eObjetoDesenhoMouseDown.Continue;

            /* if (Comando == ListaComandos[0])
            {
                pavimento = desenho.PavimentoAtual;
                command = "";

              //  FCopiaPadrao CopiaPadrao = new FCopiaPadrao(gerenciador);
              //  CopiaPadrao.Show();

                return eDrawObjectMouseDown.Continue;
            }
            else
            if (Comando == ListaComandos[1])
            {
                ponto1 = point;
                Objetos = new List<TObjetoDesenho>();

                foreach (TObjetoDesenho obj in ObjetosSelecionados)
                {
                    Objetos.Add(obj.Clone());
                    Objetos[Objetos.Count - 1].SetaSelecao(false, false);
                    Objetos[Objetos.Count - 1].ObjetoCopia = true;
                }
                command = "";
                return eDrawObjectMouseDown.Continue;
            }
            else
            if (Comando == ListaComandos[2])
            {
                foreach (TObjetoDesenho obj in ObjetosSelecionados)
                  obj.SetaSelecao(false, false);

                OnMouseMove(ref point, false, null);

                foreach (TObjetoDesenho obj in Objetos)
                {
                    obj.Atualiza(pavimento);
                    obj.ObjetoCopia = false;
                    ObjetosResultado.Add(obj);
                }
                command = "";
                return eDrawObjectMouseDown.DoneRepeat;
            }
            else
                return eDrawObjectMouseDown.Continue;*/
        }
        public string GetComando(int numeroComando)
        {
            Comando = ListaComandos[numeroComando];
            return ListaComandos[numeroComando];
        }
        public void Continue()
        {
            DefinindoPontoBase = true;
        }
        public bool ApagarSelecionados
        {
            get { return false; }
        }

        public string TipoObjetoParaSelecionar
        {
            get { return "TODOS"; }
        }
        public string LayerObjetoParaSelecionar
        {
            get { return "TODOS"; }
        }
        public eEditToolTipoSelecao editToolTipoSelecao
        {
            get { return eEditToolTipoSelecao.multiSelecao; }
        }
        [NonSerialized]
        public Pen mPen = new Pen(Color.White);
        public double angRotacao;
        public TPonto ponto2, pontoInicial = new TPonto(0,0,0), pontoFinal = new TPonto(0,0,0);
        vec3 p1_seta = new vec3(0, 0, 0), p2_seta = new vec3(0, 0, 0), p3_seta = new vec3(0, 0, 0); vec3 p_centro = new vec3(0,0,0);
        int  id, id_antes = -1, rep = 0;
        public void OnMouseMove(ref TPonto point, bool ShowInfo, bool orto = true, double angulo = 0, bool PontoInicial = false)
        {
         //   if (Cad != null)
            {
                //mPen.Color = Color.BlueViolet;
                //mPen.Width = 2;
                if (Comando != "selecao")
                {
                    GL.Disable(EnableCap.Lighting);
                    if ((Object)p2_direcao != null)
                    {
                       // Cad.DrawLine(mPen, Desenho.pixelX(p1_direcao.x), Desenho.pixelY(p1_direcao.y), Desenho.pixelX(p2_direcao.x), Desenho.pixelY(p2_direcao.y));
                        GL.LineWidth(2);
                        GL.Color3(Color.BlueViolet);
                        GL.Begin(PrimitiveType.Lines);
                        GL.Vertex3(p1_direcao.x, p1_direcao.y, p1_direcao.z);
                        GL.Vertex3(p2_direcao.x, p2_direcao.y, p2_direcao.z);
                        GL.End();

                        if (sentido == 1)
                        {
                            angulo = Geom.GetAnguloGlobal(p1_direcao.x, p1_direcao.y, p2_direcao.x, p2_direcao.y);
                            p1_seta.x = p2_direcao.x; p1_seta.y = p2_direcao.y;
                            p2_seta.x = p2_direcao.x - 15; p2_seta.y = p2_direcao.y - 5;
                            p3_seta.x = p2_direcao.x - 15; p3_seta.y = p2_direcao.y + 5;
                            p_centro.x = p2_direcao.x; p_centro.y = p2_direcao.y;
                        }
                        else
                            if (sentido == 2)
                            {
                                angulo = Geom.GetAnguloGlobal(p2_direcao.x, p2_direcao.y, p1_direcao.x, p1_direcao.y);
                                p1_seta.x = p1_direcao.x; p1_seta.y = p1_direcao.y;
                                p2_seta.x = p1_direcao.x - 15; p2_seta.y = p1_direcao.y - 5;
                                p3_seta.x = p1_direcao.x - 15; p3_seta.y = p1_direcao.y + 5;
                                p_centro.x = p1_direcao.x; p_centro.y = p1_direcao.y;
                            }

                        p2_seta = p2_seta.Rotate(p_centro, angulo * Const.PIDiv180);
                        p3_seta = p3_seta.Rotate(p_centro, angulo * Const.PIDiv180);

                       // GL.Color3(Color.CadetBlue);
                        GL.Begin(PrimitiveType.Lines);
                        GL.Vertex3(p1_seta.x, p1_seta.y, p1_seta.z);
                        GL.Vertex3(p2_seta.x, p2_seta.y, p2_seta.z);
                        GL.End();

                        GL.Begin(PrimitiveType.Lines);
                        GL.Vertex3(p1_seta.x, p1_seta.y, p1_seta.z);
                        GL.Vertex3(p3_seta.x, p3_seta.y, p3_seta.z);
                        GL.End();
                        
                        
                        // Cad.DrawLine(mPen, Desenho.pixelX(p1_seta.x), Desenho.pixelY(p1_seta.y), Desenho.pixelX(p2_seta.x), Desenho.pixelY(p2_seta.y));
                      //  Cad.DrawLine(mPen, Desenho.pixelX(p1_seta.x), Desenho.pixelY(p1_seta.y), Desenho.pixelX(p3_seta.x), Desenho.pixelY(p3_seta.y));
                        rep = 0;
                        id_antes = ObjetosCopia[0].ID;
                        for (int k = 0; k < ObjetosCopia.Count; k++)
                        {
                            id = ObjetosCopia[k].ID;

                            foreach (TObjetoDesenho ob in Objetos)
                                if (ob.ID == id)
                                    pontoInicial = ob.pFin;

                            if (id == id_antes)
                                rep++;
                            else
                                rep = 1;

                            pontoFinal.x = pontoInicial.x + ((intervalo * (Math.Cos(angulo * Const.PIDiv180))) * (rep));
                            pontoFinal.y = pontoInicial.y + ((intervalo * (Math.Sin(angulo * Const.PIDiv180))) * (rep));

                            ObjetosCopia[k].Mover(ref pontoInicial, ref pontoFinal, true);

                            id_antes = id;
                        }
                    }
                    else
                        if (Comando == "direcao p2")
                        {
                            GL.LineWidth(2);
                            GL.Color3(Color.BlueViolet);
                            GL.Begin(PrimitiveType.Lines);
                            GL.Vertex3(p1_direcao.x, p1_direcao.y, p1_direcao.z);
                            GL.Vertex3(point.x, point.y, point.z);
                            GL.End();

                    //         Cad.DrawLine(mPen, Desenho.pixelX(p1_direcao.x), Desenho.pixelY(p1_direcao.y), Desenho.pixelX(point.x), Desenho.pixelY(point.y));
                        }
                    GL.Enable(EnableCap.Lighting);
                }
                GL.LineWidth(1);
                /*   if (angulo != 0)
                       angRotacao = angulo;
                   else
                       angRotacao = Geom.GetAnguloGlobal(centro.x, centro.y, point.x, point.y);*/

                //    TextoInfo.DesenhaSemZoom(ref Cad, point.x + 4, point.y + 3, "Desloc: " + (this.angRotacao).ToString("n2") + "º");
            }
          //  ponto2 = point;

          //  Orto(ref ponto1.x, ref ponto1.y, ref ponto2.x, ref ponto2.y);
         //   foreach (TObjetoDesenho obj in Objetos)
         //       (obj).Mover(ponto1, ponto2);
        }

        double DifCoordX, DifCoordY, alfa;
        public void Orto(ref double xini, ref double yini, ref double xfin, ref double yfin)
        {
            //     if (pFin.Snap)
            //     MessageBox.Show("");
            alfa = (float)(FuncoesGerais.atand((yini - yfin) / (xini - xfin)));

            if (ConfiguracoesCaptura.Captura.OrtogonalLigado)
            {
                if (alfa > 85)
                {
                    alfa = 90;
                    xfin = xini;
                }
                else
                    if (alfa > 0 && alfa < 2)
                        yfin = yini;
            };

            DifCoordX = Math.Abs(xini - xfin);
            DifCoordY = Math.Abs(yini - yfin);

            for (int i = 0; i < ConfiguracoesCaptura.Captura.OutrosAngulos.Count; i++)
            {
                if (alfa >= (ConfiguracoesCaptura.Captura.OutrosAngulos[i] - 5) && alfa <= (ConfiguracoesCaptura.Captura.OutrosAngulos[i] + 5))
                {
                    alfa = ConfiguracoesCaptura.Captura.OutrosAngulos[i];

                    if (yfin > yini)
                        yfin = (yini + (float)(Math.Tan(alfa * Const.PIDiv180) * DifCoordX));
                    else
                        yfin = (yini - (float)(Math.Tan(alfa * Const.PIDiv180) * DifCoordX));
                };
            };
        }
        public void OnMouseUp(ref TPonto point)
        {
        }
        public void OnKeyDown(System.Windows.Forms.KeyEventArgs e)
        {
        }
        public void Finished()
        {
        }
        public void Undo()
        {
        }
        public void Redo()
        {
        }
    }
}
