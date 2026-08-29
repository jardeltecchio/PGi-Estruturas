using GeometryUtility;
using MathNet.Numerics.Distributions;
using OpenTK;
using OpenTK.Graphics.OpenGL;
using OpenTK.Input;
using OpenTK.Platform;
using PG.ConfiguracoesCaptura;
using Poly2Tri;
using PolygonCuttingEar;
using System;
using System.Collections.Generic;
using System.Diagnostics.Eventing.Reader;
using System.Drawing;
using System.Linq;
using System.Security.Principal;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using TriangleNet;
using TriangleNet.Geometry;
using TriangleNet.Meshing;
using TriangleNet.Smoothing;
using TriangleNet.Topology;
using Win32Interop.Enums;
using Win32Interop.Structs;

namespace PG
{
        [Serializable]
    public class TBarraGenerica: TObjetoDesenho
    {
   //     public TPonto pIni, pFin;
        public TDadosBarra Dados;
        public int IDBarra { get; set; }
        
        public TPonto pIni_org, pFin_org; // usado para quando tem offsets;
        
        public bool pinta;
        public List<int> ids_barras_rigidas; //barras rigidas provenientes dos offsets
        public int id_barra_rigida_1 = -1, id_barra_rigida_2 = -1; // no caso de ter duas barra rigidas conectadas nas pontas, oriundas de um offset.
                                                                   //assim eu consigo exluir essas barras rigidas caso a barra original do offset tbm for excluida
        [NonSerialized]
    //    public TEstrutura Estrutura;
        public int Pavimento;
        [NonSerialized] public double comprimento;
        [NonSerialized]
        public double anguloRotacao;
        public TLinha Linha_Eixo { get; set; }
        bool criando;
        [NonSerialized]
        public TTexto texto1, texto2;
        public vec3 pMedioBarra = new vec3(0);
        public bool barra_offset;
        public TCargaLinear PesoProprio { get; set; } // em kn/m

        public bool ArticulacaoIni, ArticulacaoFim;

        public bool Articulacao; //0 nenhum , 1 inicio e fim , 2 inicio, 3 fim
        public TBarraGenerica()
        {

        }
        public TBarraGenerica(TDadosBarra dados = null) 
        {
            base.Visivel       = true;
            base.IdLayer       = Lay.Barras;
            this.Tipo = Const.ID_BARRAGENERICA;
            this.Dados = dados;
            this.Selecionado = false;
            base.Selecionado = false;
            NaoPermiteMoverOuCopiar = false;
           

        }

        public TBarraGenerica(TPonto pInicial, TPonto pFinal)
        {
            pIni = pInicial;
            pFin = pFinal;
        }
        public void AcertaPontosNotacaoCientifica()
        {
            if (Geom.Iguais(pFin.x, 0))
                pFin.x = 0;
            if (Geom.Iguais(pFin.y, 0))
                pFin.y = 0;
            if (Geom.Iguais(pFin.z, 0))
                pFin.z = 0;

            if (Geom.Iguais(pIni.x, 0))
                pIni.x = 0;
            if (Geom.Iguais(pIni.y, 0))
                pIni.y = 0;
            if (Geom.Iguais(pIni.z, 0))
                pIni.z = 0;

            if (Geom.Iguais(Linha_Eixo.pIni.x, 0))
                Linha_Eixo.pIni.x = 0;
            if (Geom.Iguais(Linha_Eixo.pIni.y, 0))
                Linha_Eixo.pIni.y = 0;
            if (Geom.Iguais(Linha_Eixo.pIni.z, 0))
                Linha_Eixo.pIni.z = 0;

            if (Geom.Iguais(Linha_Eixo.pFin.x, 0))
                Linha_Eixo.pFin.x = 0;
            if (Geom.Iguais(Linha_Eixo.pFin.y, 0))
                Linha_Eixo.pFin.y = 0;
            if (Geom.Iguais(Linha_Eixo.pFin.z, 0))
                Linha_Eixo.pFin.z = 0;

            //força a mesma coordenada caso tenham a mesma..pois as vezes o programa deixa alguns decimos de diferença 
            if (Geom.Iguais(pFin.x, pIni.x))
            {
                pFin.x = pIni.x;
                Linha_Eixo.pIni.x = pIni.x;
                Linha_Eixo.pFin.x = pIni.x;
            }

            if (Geom.Iguais(pFin.y, pIni.y))
            {
                pFin.y = pIni.y;
                Linha_Eixo.pIni.y = pIni.y;
                Linha_Eixo.pFin.y = pIni.y;
            }

            if (Geom.Iguais(pFin.z, pIni.z))
            {
                pFin.z = pIni.z;
                Linha_Eixo.pIni.z = pIni.z;
                Linha_Eixo.pFin.z = pIni.z;
            }

        }

        public TBarraGenerica(TPonto p1, TPonto p2, TLayer lay,TDadosBarra dados, int pav)
        {
           // this.Estrutura = estrutura;
            string ss = "";
            NaoPermiteMoverOuCopiar = false;
            List<TLinha> linhas = new List<TLinha>();
            this.Initialize(ref p1, ref ss, dados, lay, ref linhas, ref pav);
            criando = false;
            pIni = (TPonto)p1.Clone();
            pFin = (TPonto)p2.Clone();


            this.Selecionado = false;
            base.Selecionado = false;
            base.Visivel = true;
            base.IdLayer = Lay.Barras;
            this.Tipo    = Const.ID_BARRAGENERICA;
            this.angulo  = (float)(FuncoesGerais.atand((p1.y - p2.y) / (p1.x - p2.x)));
   
        }
        public override void Initialize(ref TPonto point, ref string command, ISettings Dados, TLayer layer, ref List<TLinha> Linhas, ref int pavimento)
        {
            
            this.Dados = Dados as TDadosBarra;
            base.Tipo = Const.ID_BARRAGENERICA;

            base.pIni = (TPonto)point.Clone();
            pIni = (TPonto)point.Clone();
            base.pFin = (TPonto)point.Clone();
            pFin = (TPonto)point.Clone();
            temOffset = (!Geom.Iguais(this.Dados.ez, 0) || !Geom.Iguais(this.Dados.ey, 0) || !Geom.Iguais(this.Dados.ex_i, 0) || !Geom.Iguais(this.Dados.ex_f, 0));
            this.layer = layer;
            this.Pavimento = pavimento;
            //this.Estrutura = estrutura;

            CoordsSecao_i = new vec3[1];
            CoordsSecao_f = new vec3[1];

            coordssecao_i = new List<vec3[]>();
            coordssecao_f = new List<vec3[]>();

            criando = true;

            Linha_Eixo = new TLinha();
            Linha_Eixo.Initialize(ref point, ref command, null, layer, ref Linhas, ref pavimento);
            Linha_Eixo.Barra = this;

            base.Initialize(ref point, ref command, Dados, layer, ref Linhas, ref pavimento);
            command = "Novo elemento - " + (Dados as TDadosBarra) .secao.descricao + " - Selecione o segundo ponto:";

            AcertaPontosNotacaoCientifica();

        }
        public override string PrimeiroComando()
        {
            return "Novo elemento - Selecione o primeiro ponto";
        }
        public override void Proximo()
        {

            base.Proximo();
        }
        [NonSerialized]

        bool ForaDaTela, naTela1, naTela2;
        public bool realcar;
        public bool NaTela()
        {
            FPrincipal.pixel1(ref pIni.x, ref pIni.y, ref pIni.z, ref z_clip1);
            FPrincipal.pixel2(ref pFin.x, ref pFin.y, ref pFin.z, ref z_clip2);

            ForaDaTela = ((FPrincipal.px_x1[0] < 0) && (FPrincipal.px_x2[0] < 0)) ||
                              ((FPrincipal.px_x1[0] > FPrincipal.w) && (FPrincipal.px_x2[0] > FPrincipal.w)) ||
                              ((FPrincipal.px_y1[0] < 0) && (FPrincipal.px_y2[0] < 0)) ||
                              ((FPrincipal.px_y1[0] > FPrincipal.h) && (FPrincipal.px_y2[0] > FPrincipal.h));
            if (ForaDaTela)
                return false;

            naTela1 = (z_clip1 < 1 && z_clip1 > 0);
            naTela2 = (z_clip2 < 1 && z_clip2 > 0);

            // ForaDaTela = !((z_clip1 < 1 && z_clip1 > 0) && (z_clip2 < 1 && z_clip2 > 0));
            if (!naTela1 || !naTela2)
                return false;

            //            if (ForaDaTela)
            //              return;

            Soma_Z = z_clip1 + z_clip2;

            return true;
        }

        public override void SetaSelecao(bool s, bool MostraGrip, bool SelecaoDeCandidato = false, TObjetoDesenho Owner = null)
        {
            base.Selecionado = false;
            if (!this.Visivel)
                return;
            
           /* if (!SelecaoDeCandidato)
            {
                FPrincipal.pixel1(ref pIni.x, ref pIni.y, ref pIni.z, ref z_clip1);
                FPrincipal.pixel2(ref pFin.x, ref pFin.y, ref pFin.z, ref z_clip2);

                ForaDaTela = ((FPrincipal.px_x1[0] < 0) && (FPrincipal.px_x2[0] < 0)) ||
                                  ((FPrincipal.px_x1[0] > FPrincipal.w) && (FPrincipal.px_x2[0] > FPrincipal.w)) ||
                                  ((FPrincipal.px_y1[0] < 0) && (FPrincipal.px_y2[0] < 0)) ||
                                  ((FPrincipal.px_y1[0] > FPrincipal.h) && (FPrincipal.px_y2[0] > FPrincipal.h));
                if (ForaDaTela)
                    return;

                naTela1 = (z_clip1 < 1 && z_clip1 > 0);
                naTela2 = (z_clip2 < 1 && z_clip2 > 0);

                // ForaDaTela = !((z_clip1 < 1 && z_clip1 > 0) && (z_clip2 < 1 && z_clip2 > 0));
                if (!naTela1 && !naTela2)
                    return;

                //            if (ForaDaTela)
                //              return;

                Soma_Z = z_clip1 + z_clip2;
            }*/

            //  if (Soma_Z > 800)
            //       return; 
            realcar = false;
            base.Selecionado = s;

           // if (s)
               DirtyArestas = true;
           
            base.MostrarGrip = MostraGrip;
            Linha_Eixo.Selecionado = s;
            ShowHideGrips(MostraGrip);
        }
        double z_clip1, z_clip2;
        public override bool Selecionar(float clicx, float clicy, float coordx, float coordy, TObjetoDesenho Owner = null)
        {
            try
            {
                if (!this.Visivel)
                   return false;
 
                Vector3d p1 = new Vector3d(pIni.x, pIni.y, pIni.z);
                Vector3d p2 = new Vector3d(pFin.x, pFin.y, pFin.z);

                double x1 =0 ,y1 =0, x2 =0, y2 = 0;
                
                if (!Clipper3D.ProjetarLinhaSelecao(FPrincipal.View_x_Proj, FPrincipal.cameraPerspectiva, p1, p2, out x1, out y1,out x2, out y2))
                    return false;

                if (Geom.PontoEmLinha2(clicx, clicy,x1, y1,x2, y2, 0.05))
                {
                    DirtyArestas = true;
                    SetaSelecao(true, true);
                    return base.Selecionado;
                }

                return false;
            }
            catch (Exception)
            {

            }
            return false;
        }
        [NonSerialized]
        public bool PreSelecionado = false;
        public override void PreSelecionar(ref int clicx, ref int clicy)
        {
            try
            {
                FPrincipal.pixel1(ref pIni.x, ref pIni.y, ref pIni.z);
                FPrincipal.pixel2(ref pFin.x, ref pFin.y, ref pFin.z);

                PreSelecionado = false;
                if (Geom.PontoEmLinha2(clicx, clicy, FPrincipal.px_x1[0], FPrincipal.px_y1[0], FPrincipal.px_x2[0], FPrincipal.px_y2[0], 1))
                  PreSelecionado = true;
            }
            catch (Exception)
            {

            }
        }

        public override eObjetoDesenhoMouseDown OnMouseDown(ref TPonto point, ref string command, List<TLinha> Linhas, List<TPonto> Pontos, List<TPilar> Pilares, ref List<TObjetoDesenho> Objects, bool FromGrip = false)
        {
            if ((Geom.Iguais(point.x, pIni.x)) && (Geom.Iguais(point.y, pIni.y)) && (Geom.Iguais(point.z, pIni.z)))
            {
                return eObjetoDesenhoMouseDown.Continue;
            }
            NaoPermiteMoverOuCopiar = false;
            /*      if (!primeiroPonto)
                  {
                      this.pIni = (TPonto)point.Clone();
                  }
                  else
                  {*/
            //  primeiroPonto = true;
            pFin = (TPonto)point.Clone();
            //    }
            if (Geom.Iguais(pFin.x, 0))
                pFin.x = 0;
            if (Geom.Iguais(pFin.y, 0))
                pFin.y = 0;
            if (Geom.Iguais(pFin.z, 0))
                pFin.z = 0;

            if (Geom.Iguais(pIni.x, 0))
                pIni.x = 0;
            if (Geom.Iguais(pIni.y, 0))
                pIni.y = 0;
            if (Geom.Iguais(pIni.z, 0))
                pIni.z = 0;

            criando = false;
            this.comprimento = (float)pFin.DistanceTo(pIni);

            Linha_Eixo.pIni = pIni;
            Linha_Eixo.pFin = pFin;
            
            OrientaSecaoNoEspaco();

            CriaPesoProprio();

            return eObjetoDesenhoMouseDown.DoneRepeat;
        }
        public void CriaPesoProprio()
        {
            if (Dados.secao.tipo == Const.SECAO_GENERICA)//|| Dados.Tipo == 4)
                return;
           
            List<TLinha> lin = new List<TLinha>();
            PesoProprio = new TCargaLinear();
            PesoProprio.pIni = Linha_Eixo.pIni;
            PesoProprio.pFin = Linha_Eixo.pFin;
            // double carga = (this.Dados.secao.idMaterial.pesoEspecifico * (t.Dados.b1 / 100) * (t.Dados.h1 / 100) * (t.comprimento / 100)) / 100; //em kN

            // carga == b1.TrechoViga.PP / (b1.TrechoViga.comprimento / 100); // tf/m


            TDadosCarga dados = new TDadosCarga(-(Dados.secao.PesoProprio), 0, 2, 0, true, false, 1, Color.Orange, false,true,false);
            string cmd = "";
            int pav = 0;
            PesoProprio.Initialize(ref Linha_Eixo.pIni, ref cmd, dados as TDadosCarga, this.layer, ref lin, ref pav);
            PesoProprio.pFin = Linha_Eixo.pFin.Clone() as TPonto;
            PesoProprio.idBarra = IDBarra;
        }

        public override void Mover(ref TPonto ponto1, ref TPonto ponto2, bool dinamico)
        {
            /* ponto1.x = 0;
             ponto1.y = 0;
             ponto1.z = 0;

             ponto2.x = 200;
             ponto2.y = 200;
             ponto2.z = 200;
             */

            if (dinamico)
            {
                pIni.primeiraVezMover = true;
                pFin.primeiraVezMover = true;
                Linha_Eixo.pIni.primeiraVezMover = true;
                Linha_Eixo.pFin.primeiraVezMover = true;
            }
       
            pIni.Mover(ref ponto1, ref ponto2,  dinamico);
            pFin.Mover(ref ponto1, ref ponto2,  dinamico);

            // Linha_Eixo.Mover(ref ponto1, ref ponto2, dinamico);
            if (PesoProprio != null)
            {
                PesoProprio.pIni.primeiraVezMover = true;
                PesoProprio.Mover(ref ponto1, ref ponto2, dinamico);
            }

            OrientaSecaoNoEspaco();
        }

        [NonSerialized]
        public vec3[] CoordsSecao_i;
        [NonSerialized]
        public vec3[] CoordsSecao_f;

        [NonSerialized]
        public List<vec3[]> coordssecao_i;
        [NonSerialized]
        public List<vec3[]> coordssecao_f;


        [NonSerialized]
        vec3 u1, u2, u, normxy, normxz;
        public double NdotU, ndotu_mod, cos_alfa, angXY, angXZ, divisoes, divisoesFrac, xAnt;

        List<vec3> CoordsSubdvisao = new List<vec3>();
        double[] posicao = new double[4];
        double[] posicaoFinal = new double[4];
        double[] posicaoFinal2 = new double[4];
        double[] posicaoFinal_Trans = new double[5];

        double tx, ty, tz;
        bool zi_maior_que_zf, xi_igual_xf;
        vec3 pos;
        public double cx, cy, L, cz, xi,yi,xf,yf,zi,zf;

    
        [NonSerialized]
        CPolygonShape PoligonoFace1;

        [NonSerialized]
        public Geom.poligono[] triangulos_face_1, triangulos_face_2;
        public void TriangularizarFaces()
        {
            try
            {

                vec3[] coordenadas_Externo;

                if (coordssecao_i[0][0].vv == 2) // externo
                    coordenadas_Externo = coordssecao_i[0];
                else
                    coordenadas_Externo = coordssecao_i[1];

                List<PolygonPoint> PontosSecao = new List<PolygonPoint>();  
                for (int i = coordenadas_Externo.Count()- 1; i >=1 ; i--)
                   PontosSecao.Add(new PolygonPoint((float)coordenadas_Externo[i].y, (float)coordenadas_Externo[i].z));

                Polygon_Poly2Tri ContornoSecao = new Polygon_Poly2Tri(PontosSecao);
                TriangulationContext tcx = new DTSweepContext();

                if (Dados.secaoSemRotacao.poligonos.Exists(o => !o.externo))
                {
                    List<PolygonPoint> PontosContornoInterno = new List<PolygonPoint>();
   
                    for (int q = 0; q < Dados.secaoSemRotacao.poligonos.Count; q++)
                    {
                        if (!Dados.secaoSemRotacao.poligonos[q].externo)
                        {
                            for (int i = Dados.secaoSemRotacao.poligonos[q].coords.Count() - 1; i >= 1; i--)
                                PontosContornoInterno.Add(new PolygonPoint((float)coordssecao_i[q][i].y, (float)coordssecao_i[q][i].z));

                            Polygon_Poly2Tri  ContornoInterno = new Polygon_Poly2Tri(PontosContornoInterno);

                            ContornoSecao.AddHole(ContornoInterno);
                        }

                    }
                }

                tcx.PrepareTriangulation(ContornoSecao);
                DTSweep.Triangulate((DTSweepContext)tcx);

                int jj =-1;

                triangulos_face_1 = new Geom.poligono[ContornoSecao.Triangles.Count];
                triangulos_face_2 = new Geom.poligono[ContornoSecao.Triangles.Count];

                double length = comprimento;
                double x_i = Dados.ex_i / 1000;

                if (!Geom.Iguais(pIni.x, pFin.x))
                {
                    if (pIni.x > pFin.x)
                    {
                        length = -comprimento;
                        length -= Dados.ex_f / 1000;
                        x_i = -Dados.ex_i / 1000;
                    }
                    else
                        length += Dados.ex_f / 1000;
                }
                else
                    length = comprimento + Dados.ex_f / 1000; 

                foreach (DelaunayTriangle t in ContornoSecao.Triangles)
                {
                    triangulos_face_1[++jj] = new Geom.poligono(t.Points.Count());
                    triangulos_face_2[jj]   = new Geom.poligono(t.Points.Count());

                    for (j = 0; j < t.Points.Count(); j++)
                    {
                        triangulos_face_1[jj].vertices[j].x = x_i;//Dados.ex_i/1000;
                        triangulos_face_1[jj].vertices[j].y = t.Points[j].X;
                        triangulos_face_1[jj].vertices[j].z = t.Points[j].Y;

                        triangulos_face_2[jj].vertices[j].x = length;
                        triangulos_face_2[jj].vertices[j].y = t.Points[j].X;
                        triangulos_face_2[jj].vertices[j].z = t.Points[j].Y;
/*
                        triangulos_face_2[jj].vertices[j].y += -Dados.ey / 1000;
                        triangulos_face_2[jj].vertices[j].z += -Dados.ez / 1000;

                        triangulos_face_1[jj].vertices[j].y += -Dados.ey / 1000;
                        triangulos_face_1[jj].vertices[j].z += -Dados.ez / 1000;*/

                       /* if (!Geom.Iguais(pIni.x, pFin.x))
                        {
                            if (pIni.x > pFin.x)
                                triangulos_face_2[jj].vertices[j].x = -comprimento;
                            else
                                triangulos_face_2[jj].vertices[j].x = comprimento;
                        }
                        else
                            triangulos_face_2[jj].vertices[j].x = comprimento;

                        */

                    }
                }
            }
            catch(Exception ee)
            {
                System.Windows.Forms.MessageBox.Show("TBarraGenerica.TriangularizarFaces() - Erro ao triangularizar face da seção transversal:  " +ee.Message);
            }
        }

        [NonSerialized]
        public List<vec3> coordsAABB_i, coordsAABB_f; // OBB -> oriented bounding box-> coords do boundingbox já orientado (rotacionado) nos eixos do objeto
        [NonSerialized]
        List<vec3> coordAABB_SemRotacao;
        [NonSerialized]
        public double max_x_AABB, max_y_AABB, max_z_AABB, min_x_AABB, min_y_AABB, min_z_AABB;
        public vec3 centroRotacao;
        public double AlfaAlterado, AlfaEixosPrincipaisAlterado, indiceCoord_Comparacao;
        public vec3 pIni_Offset, pFin_Offset;
        public bool temOffset;
        public void OrientaSecaoNoEspaco()
        {
            // CoordsSecao_f.Clear();
            if (Dados.secao.poligonos == null)
                return;
            
            DirtyTriangulos = true;
            DirtySelecao = true;
            DirtyArestas = true;

            coordssecao_i = new List<vec3[]>();
            coordssecao_f = new List<vec3[]>();

            vec3 coord1 = new vec3(0, 0, 0);
            centroRotacao = new vec3(0, 0, 0);

            AcertaPontosNotacaoCientifica();

            cy = ((((pFin.z * -1) - (pIni.z * -1))) / comprimento);

            centroRotacao.x = 0;
            centroRotacao.y = 0;

            AlfaAlterado = 0;
            AlfaEixosPrincipaisAlterado = 0;
            double angEixosPrincipaisGraus = Dados.secao.propriedades.anguloEixosPrincipais * (180 / Math.PI);
            coordsAABB_f = new List<vec3>();
            coordsAABB_i = new List<vec3>();

            coordAABB_SemRotacao = new List<vec3>();
            coordAABB_SemRotacao.Add(new vec3(Dados.secaoSemRotacao.poligonos.Find(o=>o.externo).coords.Min(o => o.X), Dados.secaoSemRotacao.poligonos.Find(o => o.externo).coords.Min(o => o.Y), 0));
            coordAABB_SemRotacao.Add(new vec3(Dados.secaoSemRotacao.poligonos.Find(o=>o.externo).coords.Max(o => o.X), Dados.secaoSemRotacao.poligonos.Find(o=>o.externo).coords.Min(o => o.Y), 0));
            coordAABB_SemRotacao.Add(new vec3(Dados.secaoSemRotacao.poligonos.Find(o=>o.externo).coords.Max(o => o.X), Dados.secaoSemRotacao.poligonos.Find(o=>o.externo).coords.Max(o => o.Y), 0));
            coordAABB_SemRotacao.Add(new vec3(Dados.secaoSemRotacao.poligonos.Find(o => o.externo).coords.Min(o => o.X), Dados.secaoSemRotacao.poligonos.Find(o => o.externo).coords.Max(o => o.Y), 0));

            int vv = -1;
            for (int q = 0; q < Dados.secaoSemRotacao.poligonos.Count; q++)
            {
                if (Dados.secaoSemRotacao.poligonos[q].externo)
                    vv = 2;
                else
                    vv = 1;

                if (!Geom.Iguais(pIni.x, pFin.x))
                {
                    if (pIni.x > pFin.x)
                    {
                        AlfaAlterado = -Dados.anguloRotacao;
                        AlfaEixosPrincipaisAlterado = -angEixosPrincipaisGraus;

                        for (int i = 0; i < (Dados.secaoSemRotacao.poligonos[q].coords.Count()); i++)
                        {
                            coord1 = new vec3(Dados.secaoSemRotacao.poligonos[q].coords[i].X + (Dados.ey / 1000), -Dados.secaoSemRotacao.poligonos[q].coords[i].Y + (-Dados.ez / 1000), 0);
                            coord1 = coord1.Rotate(centroRotacao, ((-Dados.anguloRotacao)) * Const.PIDiv180);

                            Dados.secao.poligonos[q].coords[i].X = coord1.x;
                            Dados.secao.poligonos[q].coords[i].Y = coord1.y;
                        }
                    }
                    else
                    {
                        AlfaAlterado = Dados.anguloRotacao;
                        AlfaEixosPrincipaisAlterado = angEixosPrincipaisGraus;

                        for (int i = 0; i < (Dados.secaoSemRotacao.poligonos[q].coords.Count()); i++)
                        {
                            coord1 = new vec3(-Dados.secaoSemRotacao.poligonos[q].coords[i].X + (-Dados.ey / 1000), -Dados.secaoSemRotacao.poligonos[q].coords[i].Y + (-Dados.ez / 1000), 0);
                            coord1 = coord1.Rotate(centroRotacao, ((Dados.anguloRotacao)) * Const.PIDiv180);

                            Dados.secao.poligonos[q].coords[i].X = coord1.x;
                            Dados.secao.poligonos[q].coords[i].Y = coord1.y;
                        }

                    }
                }
                else
                if (Geom.Iguais(Math.Abs(cy), 1, 0.000001))
                {
                    AlfaAlterado = Dados.anguloRotacao + 90;
                    AlfaEixosPrincipaisAlterado = angEixosPrincipaisGraus;// - 90;

                    if ((pIni.z * -1) < (pFin.z * -1))
                    {

                        for (int i = 0; i < (Dados.secaoSemRotacao.poligonos[q].coords.Count()); i++)
                        {
                            coord1 = new vec3(Dados.secaoSemRotacao.poligonos[q].coords[i].X + (Dados.ey / 1000), Dados.secaoSemRotacao.poligonos[q].coords[i].Y + (Dados.ez / 1000), 0);
                            coord1 = coord1.Rotate(centroRotacao, ((AlfaAlterado)) * Const.PIDiv180);

                            Dados.secao.poligonos[q].coords[i].X = coord1.x;
                            Dados.secao.poligonos[q].coords[i].Y = coord1.y;
                        }

                    }
                    else
                    {
                        for (int i = 0; i < (Dados.secaoSemRotacao.poligonos[q].coords.Count()); i++)
                        {
                            coord1 = new vec3(-Dados.secaoSemRotacao.poligonos[q].coords[i].X + (-Dados.ey / 1000), -Dados.secaoSemRotacao.poligonos[q].coords[i].Y + (-Dados.ez / 1000), 0);
                            coord1 = coord1.Rotate(centroRotacao, ((AlfaAlterado)) * Const.PIDiv180);

                            Dados.secao.poligonos[q].coords[i].X = coord1.x;
                            Dados.secao.poligonos[q].coords[i].Y = coord1.y;
                        }

                    }
                }
                else
                {
                    AlfaAlterado = Dados.anguloRotacao;
                    AlfaEixosPrincipaisAlterado = angEixosPrincipaisGraus;

                    for (int i = 0; i < (Dados.secaoSemRotacao.poligonos[q].coords.Count()); i++)
                    {
                        coord1 = new vec3(-Dados.secaoSemRotacao.poligonos[q].coords[i].X + (-Dados.ey / 1000), -Dados.secaoSemRotacao.poligonos[q].coords[i].Y + (-Dados.ez / 1000), 0);
                        coord1 = coord1.Rotate(centroRotacao, ((Dados.anguloRotacao)) * Const.PIDiv180);

                        Dados.secao.poligonos[q].coords[i].X = coord1.x;
                        Dados.secao.poligonos[q].coords[i].Y = coord1.y;
                    }
                }

                if (Geom.Iguais(Dados.secao.propriedades.anguloEixosPrincipais, 0))
                    AlfaEixosPrincipaisAlterado = 0;

                //se tem offset cria um vetor no centro, aplica o offset e rotaciona, senao só cria o vetor no centro(0,0), que é o próprio ponto da linha_eixo
                vec3 pCentro_com_offset = new vec3(0);
                if (temOffset)
                {
                    pCentro_com_offset = new vec3(-(Dados.ey / 1000), (-Dados.ez / 1000), 0);

                    if (!Geom.Iguais(pIni.x, pFin.x))
                    {
                        if (pIni.x > pFin.x)
                            pCentro_com_offset = new vec3((Dados.ey / 1000), (-Dados.ez / 1000), 0);
                    }
                    else
                    if (Geom.Iguais(Math.Abs(cy), 1, 0.000001))
                    {
                        if ((pIni.z * -1) < (pFin.z * -1))
                            pCentro_com_offset = new vec3((Dados.ey / 1000), (Dados.ez / 1000), 0);
                    }

                    pCentro_com_offset = pCentro_com_offset.Rotate(centroRotacao, ((AlfaAlterado)) * Const.PIDiv180);
                }
                else
                    pCentro_com_offset = new vec3(0, 0, 0);

                pIni_Offset = new vec3(0);
                pFin_Offset = new vec3(0);

                qtd_CoordsSecao = Dados.secao.poligonos[q].coords.Count();

          //     CoordsSecao_f = new vec3[qtd_CoordsSecao];
      //          CoordsSecao_i = new vec3[qtd_CoordsSecao];

                coordssecao_f.Add(new vec3[qtd_CoordsSecao]);
                coordssecao_i.Add(new vec3[qtd_CoordsSecao]);

                if (!Geom.Iguais(pIni.x, pFin.x))
                {
                    double length = comprimento;
                    double x_i = Dados.ex_i / 1000;

                    pFin_Offset.x2 = comprimento;

                    if (pIni.x > pFin.x)
                    {
                        length = -comprimento;

                        pFin_Offset.x2 = -comprimento;

                        length -= Dados.ex_f / 1000;
                        x_i = -Dados.ex_i / 1000;
                    }
                    else
                        length += Dados.ex_f / 1000;

                    //-----------------//
                    pIni_Offset.x2 = 0;
                    pIni_Offset.y2 = pCentro_com_offset.x;
                    pIni_Offset.z2 = pCentro_com_offset.y;

                    pFin_Offset.y2 = pCentro_com_offset.x;
                    pFin_Offset.z2 = pCentro_com_offset.y;

                    //-----------------//
                    pIni_Offset.x = x_i;
                    pIni_Offset.y = pCentro_com_offset.x;
                    pIni_Offset.z = pCentro_com_offset.y;

                    pFin_Offset.x = length;
                    pFin_Offset.y = pCentro_com_offset.x;
                    pFin_Offset.z = pCentro_com_offset.y;

                    for (i = 0; i < qtd_CoordsSecao; i++)
                    {
                        // CoordsSecao_i[i] = (new vec3(0, Dados.secao.poligonos[q].coords[i].X, Dados.secao.poligonos[q].coords[i].Y));
                        coordssecao_i[coordssecao_i.Count-1][i] = (new vec3(x_i, Dados.secao.poligonos[q].coords[i].X, Dados.secao.poligonos[q].coords[i].Y, false, vv));
                        coordssecao_f[coordssecao_f.Count-1][i] = new vec3(length, Dados.secao.poligonos[q].coords[i].X, Dados.secao.poligonos[q].coords[i].Y, false, vv);
                    }
                }
                else
                {
                    double length = comprimento + Dados.ex_f / 1000;

                    pIni_Offset.x2 = 0;
                    pIni_Offset.y2 = pCentro_com_offset.x;
                    pIni_Offset.z2 = pCentro_com_offset.y;

                    pFin_Offset.x2 = comprimento;
                    pFin_Offset.y2 = pCentro_com_offset.x;
                    pFin_Offset.z2 = pCentro_com_offset.y;

                    //--------------------------//
                    pIni_Offset.x = Dados.ex_i / 1000;
                    pIni_Offset.y = pCentro_com_offset.x;
                    pIni_Offset.z = pCentro_com_offset.y;

                    pFin_Offset.x = length;
                    pFin_Offset.y = pCentro_com_offset.x;
                    pFin_Offset.z = pCentro_com_offset.y;

                    for (i = 0; i < qtd_CoordsSecao; i++)
                    {
                        coordssecao_i[coordssecao_i.Count-1][i] = (new vec3(Dados.ex_i / 1000, Dados.secao.poligonos[q].coords[i].X, Dados.secao.poligonos[q].coords[i].Y, false, vv));
                        coordssecao_f[coordssecao_f.Count-1][i] = new vec3(length, Dados.secao.poligonos[q].coords[i].X, Dados.secao.poligonos[q].coords[i].Y, false, vv);
                    }

                   /* for (i = 0; i < qtd_CoordsSecao; i++)
                    {
                        CoordsSecao_i[i] = new vec3(0, Dados.secao.poligonos[q].coords[i].X, Dados.secao.poligonos[q].coords[i].Y);
                        CoordsSecao_f[i] = (new vec3(comprimento, Dados.secao.poligonos[q].coords[i].X, Dados.secao.poligonos[q].coords[i].Y));
                    }*/
                }
            }

            double xu = centroRotacao.x;
            double yu = centroRotacao.y;
            centroRotacao.x = 0;
            centroRotacao.y = xu;
            centroRotacao.z = yu;

          //  if (coordssecao_i.Count == 1)
           TriangularizarFaces();


            try
            {

                {
                    xi = pIni.x;
                    xf = pFin.x;
                    yi = pIni.y;
                    yf = pFin.y;
                    zi = pIni.z;
                    zf = pFin.z;

                    if (Geom.Iguais(yi, 0))
                        yi = 0;
                    if (Geom.Iguais(yf, 0))
                        yf = 0;

                    L = (Math.Sqrt(Math.Pow(xi - xf, 2) + Math.Pow(yi - yf, 2) + Math.Pow(zi - zf, 2)));

                    zi_maior_que_zf = false;
                    //  if (! Geom.Iguais(Math.Abs(zf), Math.Abs(zi)))
                    zi_maior_que_zf = ((zi * -1) > (zf * -1));

                    xi_igual_xf = Geom.Iguais(xi, xf);

                    tx = (xi);
                    ty = (yi);
                    tz = (zi);

                    xi -= tx;
                    yi -= ty;
                    zi -= tz;

                    xf -= tx;
                    yf -= ty;
                    zf -= tz;

                    /*faço uma translação da barra para o ponto zero do sistema global, como se eu fizesse o comando mover do programa
                     * para o ponto zero pegando o pIni como pivo */

                    /**/

                    //  if (xi < xf)
                    cx = ((xi) - (xf)) / L;
                    //else
                    //    cx = ((xf) - (xi)) / L;

                    //  if (yi < yf)
                    cy = ((yi) - (yf)) / L;
                    //  else
                    //      cy = ((yf) - (yi)) / L;

                    cz = ((zi) - (zf)) / L;

                    u1 = new vec3(xi, yi, zi * -1);
                    u2 = new vec3(xf, yf, zf * -1);

                    //Encontrar angulo que a barra faz com os planos XY e XZ

                    //PLANO XY
                    normxy = new vec3(0, 0, 1);

                    //  if (xi < xf)
                    u = u1 - u2;
                    //else
                    //    u = u2 - u1;

                    NdotU = (normxy.DotProduct(u));
                    ndotu_mod = normxy.Magnitude() * u.Magnitude();
                    cos_alfa = Math.Abs(NdotU / ndotu_mod);
                    angXY = RMath.rad2deg(Math.Acos(cos_alfa));
                    angXY =/* RMath.rad2deg(Math.Atan(u1.z - u2.z / (u1.y - u2.y)));*/(90 - angXY) * 1;

                    //PLANO XZ
                    normxz = new vec3(0, 1, 0);
                    //  if (xi< xf)
                    u = u1 - u2;
                    //  else
                    //     u = u1 - u2;

                    NdotU = (normxz.DotProduct(u));
                    ndotu_mod = normxz.Magnitude() * u.Magnitude();
                    cos_alfa = Math.Abs(NdotU / ndotu_mod);
                    //   angXZ = RMath.rad2deg(Math.Acos(cos_alfa));

                    if (!Geom.Iguais(u1.x - u2.x, 0))
                        angXZ = RMath.rad2deg(Math.Atan(u1.y - u2.y / (u1.x - u2.x)));/* (90-angXZ) * 1;*/
                    else
                    {
                        if (!Geom.Iguais(yi, yf))
                        {
                            if (yi < yf)
                                angXZ = -90;
                            else
                                angXZ = 90;
                        }
                        else
                            angXZ = 90;
                    }

                    pos = new vec3(0, 0, 0);
                    xAnt = xi;

                    if (!xi_igual_xf)
                        if (xi < xf)
                            angXY *= -1;

                    if (!Geom.Iguais(Math.Abs(angXZ), 90))
                        angXZ *= -1;

                    if (xi_igual_xf)
                        angXY *= -1;

                    if (zi_maior_que_zf)
                        angXY *= -1;

                    /*  if (!zi_maior_que_zf && xi_igual_xf && (!Geom.Iguais(zf-zi, 0))))
                        angXY *= -1;*/

                    if (triangulos_face_1 != null)
                    {
                        for (i = 0; i < triangulos_face_1.Count(); i++)
                        {
                            for (j = 0; j < triangulos_face_1[i].vertices.Count(); j++)
                            {
                                xi = triangulos_face_1[i].vertices[j].x;
                                yi = triangulos_face_1[i].vertices[j].y;
                                zi = triangulos_face_1[i].vertices[j].z;

                                xf = triangulos_face_2[i].vertices[j].x;
                                yf = triangulos_face_2[i].vertices[j].y;
                                zf = triangulos_face_2[i].vertices[j].z;

                                posicao[1] = xi;
                                posicao[2] = yi;
                                posicao[3] = zi;
                                Geom.rotY(angXY, ref posicao, ref posicaoFinal);
                                Geom.rotZ(angXZ, ref posicaoFinal, ref posicaoFinal2);

                                triangulos_face_1[i].vertices[j].x = posicaoFinal2[1];
                                triangulos_face_1[i].vertices[j].y = posicaoFinal2[2];
                                triangulos_face_1[i].vertices[j].z = posicaoFinal2[3];

                                triangulos_face_1[i].vertices[j].x += tx;
                                triangulos_face_1[i].vertices[j].y += ty;
                                triangulos_face_1[i].vertices[j].z += tz;

                                posicao[1] = xf;
                                posicao[2] = yf;
                                posicao[3] = zf;

                                Geom.rotY(angXY, ref posicao, ref posicaoFinal);
                                Geom.rotZ(angXZ, ref posicaoFinal, ref posicaoFinal2);

                                triangulos_face_2[i].vertices[j].x = posicaoFinal2[1];
                                triangulos_face_2[i].vertices[j].y = posicaoFinal2[2];
                                triangulos_face_2[i].vertices[j].z = posicaoFinal2[3];

                                triangulos_face_2[i].vertices[j].x += tx;
                                triangulos_face_2[i].vertices[j].y += ty;
                                triangulos_face_2[i].vertices[j].z += tz;
                            }
                        }
                    }

                    min_z_AABB = double.MaxValue; min_y_AABB = double.MaxValue; min_x_AABB = double.MaxValue;
                    max_z_AABB = double.MinValue; max_y_AABB = double.MinValue; max_x_AABB = double.MinValue;


                    /*--------------------------------------------*/
                    posicao[1] = pIni_Offset.x;
                    posicao[2] = pIni_Offset.y;
                    posicao[3] = pIni_Offset.z;

                    Geom.rotY(angXY, ref posicao, ref posicaoFinal);
                    Geom.rotZ(angXZ, ref posicaoFinal, ref posicaoFinal2);

                    pIni_Offset.x = posicaoFinal2[1] + tx;
                    pIni_Offset.y = posicaoFinal2[2] + ty;
                    pIni_Offset.z = posicaoFinal2[3] + tz;
                    pIni_Offset.z *= -1;
                    pIni_Offset.y *= -1;

                    posicao[1] = pFin_Offset.x;
                    posicao[2] = pFin_Offset.y;
                    posicao[3] = pFin_Offset.z;

                    Geom.rotY(angXY, ref posicao, ref posicaoFinal);
                    Geom.rotZ(angXZ, ref posicaoFinal, ref posicaoFinal2);

                    pFin_Offset.x = posicaoFinal2[1] + tx;
                    pFin_Offset.y = posicaoFinal2[2] + ty;
                    pFin_Offset.z = posicaoFinal2[3] + tz;
                    pFin_Offset.z *= -1;
                    pFin_Offset.y *= -1;

                    /*--------------------------------------------*/
                    posicao[1] = pIni_Offset.x2;
                    posicao[2] = pIni_Offset.y2;
                    posicao[3] = pIni_Offset.z2;
                    Geom.rotY(angXY, ref posicao, ref posicaoFinal);
                    Geom.rotZ(angXZ, ref posicaoFinal, ref posicaoFinal2);
                    pIni_Offset.x2 = posicaoFinal2[1] + tx;
                    pIni_Offset.y2 = posicaoFinal2[2] + ty;
                    pIni_Offset.z2 = posicaoFinal2[3] + tz;
                    pIni_Offset.z2 *= -1;
                    pIni_Offset.y2 *= -1;

                    posicao[1] = pFin_Offset.x2;
                    posicao[2] = pFin_Offset.y2;
                    posicao[3] = pFin_Offset.z2;
                    Geom.rotY(angXY, ref posicao, ref posicaoFinal);
                    Geom.rotZ(angXZ, ref posicaoFinal, ref posicaoFinal2);
                    pFin_Offset.x2 = posicaoFinal2[1] + tx;
                    pFin_Offset.y2 = posicaoFinal2[2] + ty;
                    pFin_Offset.z2 = posicaoFinal2[3] + tz;
                    pFin_Offset.z2 *= -1;
                    pFin_Offset.y2 *= -1;

                    /*--------------------------------------------*/
                    posicao[1] = centroRotacao.x;
                    posicao[2] = centroRotacao.y;
                    posicao[3] = centroRotacao.z;

                    Geom.rotY(angXY, ref posicao, ref posicaoFinal);
                    Geom.rotZ(angXZ, ref posicaoFinal, ref posicaoFinal2);

                    centroRotacao.x = posicaoFinal2[1] + tx;
                    centroRotacao.y = posicaoFinal2[2] + ty;
                    centroRotacao.z = posicaoFinal2[3] + tz;
                    centroRotacao.z *= -1;
                    centroRotacao.y *= -1;
                    /*--------------------------------------------*/
                    for (int q = 0; q < Dados.secaoSemRotacao.poligonos.Count; q++)
                    {
                        for (i = 0; i < coordssecao_i[q].Count(); i++)
                        {
                            xi = coordssecao_i[q][i].x;
                            yi = coordssecao_i[q][i].y;
                            zi = coordssecao_i[q][i].z;

                            xf = coordssecao_f[q][i].x;
                            yf = coordssecao_f[q][i].y;
                            zf = coordssecao_f[q][i].z;

                            posicao[1] = xi;
                            posicao[2] = yi;
                            posicao[3] = zi;
                            Geom.rotY(angXY, ref posicao, ref posicaoFinal);
                            Geom.rotZ(angXZ, ref posicaoFinal, ref posicaoFinal2);

                            coordssecao_i[q][i].x = posicaoFinal2[1];
                            coordssecao_i[q][i].y = posicaoFinal2[2];
                            coordssecao_i[q][i].z = posicaoFinal2[3];

                            posicao[1] = xf;
                            posicao[2] = yf;
                            posicao[3] = zf;

                            Geom.rotY(angXY, ref posicao, ref posicaoFinal);
                            Geom.rotZ(angXZ, ref posicaoFinal, ref posicaoFinal2);

                            coordssecao_f[q][i].x = posicaoFinal2[1];
                            coordssecao_f[q][i].y = posicaoFinal2[2];
                            coordssecao_f[q][i].z = posicaoFinal2[3];

                            coordssecao_i[q][i].x += tx;
                            coordssecao_i[q][i].y += ty;
                            coordssecao_i[q][i].z += tz;
                            coordssecao_i[q][i].z *= -1;
                            coordssecao_i[q][i].y *= -1;

                            coordssecao_f[q][i].x += tx;
                            coordssecao_f[q][i].y += ty;
                            coordssecao_f[q][i].z += tz;
                            coordssecao_f[q][i].y *= -1;
                            coordssecao_f[q][i].z *= -1;

                            coordssecao_f[q][i].pontoEmRaio = Dados.secao.poligonos[q].coords[i].pontoEmRaio;
                            coordssecao_i[q][i].pontoEmRaio = Dados.secao.poligonos[q].coords[i].pontoEmRaio;

                            /*
                                                     xi = CoordsSecao_i[i].x;
                        yi = CoordsSecao_i[i].y;
                        zi = CoordsSecao_i[i].z;

                        xf = CoordsSecao_f[i].x;
                        yf = CoordsSecao_f[i].y;
                        zf = CoordsSecao_f[i].z;

                        posicao[1] = xi;
                        posicao[2] = yi;
                        posicao[3] = zi;
                        Geom.rotY(angXY, ref posicao, ref posicaoFinal);
                        Geom.rotZ(angXZ, ref posicaoFinal, ref posicaoFinal2);

                        CoordsSecao_i[i].x = posicaoFinal2[1];
                        CoordsSecao_i[i].y = posicaoFinal2[2];
                        CoordsSecao_i[i].z = posicaoFinal2[3];                      
                    
                        posicao[1] = xf;
                        posicao[2] = yf;
                        posicao[3] = zf;
                        
                        Geom.rotY(angXY, ref posicao, ref posicaoFinal);
                        Geom.rotZ(angXZ, ref posicaoFinal, ref posicaoFinal2);

                        CoordsSecao_f[i].x = posicaoFinal2[1];
                        CoordsSecao_f[i].y = posicaoFinal2[2];
                        CoordsSecao_f[i].z = posicaoFinal2[3];

                        CoordsSecao_i[i].x += tx;
                        CoordsSecao_i[i].y += ty;
                        CoordsSecao_i[i].z += tz;
                        CoordsSecao_i[i].z *= -1;
                        CoordsSecao_i[i].y *= -1;

                        CoordsSecao_f[i].x += tx;
                        CoordsSecao_f[i].y += ty;
                        CoordsSecao_f[i].z += tz;
                        CoordsSecao_f[i].y *= -1;
                        CoordsSecao_f[i].z *= -1;

                        CoordsSecao_f[i].pontoEmRaio = Dados.secao.poligono.coords[i].pontoEmRaio;
                        CoordsSecao_i[i].pontoEmRaio = Dados.secao.poligono.coords[i].pontoEmRaio;
                            */

                            if (Dados.secaoSemRotacao.poligonos[q].externo)
                            {
                                if (coordssecao_f[q][i].x < min_x_AABB) min_x_AABB = coordssecao_f[q][i].x;
                                if (coordssecao_i[q][i].x < min_x_AABB) min_x_AABB = coordssecao_i[q][i].x;

                                if (coordssecao_f[q][i].y < min_y_AABB) min_y_AABB = coordssecao_f[q][i].y;
                                if (coordssecao_i[q][i].y < min_y_AABB) min_y_AABB = coordssecao_i[q][i].y;

                                if (coordssecao_f[q][i].z < min_z_AABB) min_z_AABB = coordssecao_f[q][i].z;
                                if (coordssecao_i[q][i].z < min_z_AABB) min_z_AABB = coordssecao_i[q][i].z;

                                if (coordssecao_f[q][i].x > max_x_AABB) max_x_AABB = coordssecao_f[q][i].x;
                                if (coordssecao_i[q][i].x > max_x_AABB) max_x_AABB = coordssecao_i[q][i].x;

                                if (coordssecao_f[q][i].y > max_y_AABB) max_y_AABB = coordssecao_f[q][i].y;
                                if (coordssecao_i[q][i].y > max_y_AABB) max_y_AABB = coordssecao_i[q][i].y;
                                if (coordssecao_f[q][i].z > max_z_AABB) max_z_AABB = coordssecao_f[q][i].z;
                                if (coordssecao_i[q][i].z > max_z_AABB) max_z_AABB = coordssecao_i[q][i].z;
                            }
                        }
                    }

                    coordsAABB_i.Add(new vec3(min_x_AABB, min_y_AABB, min_z_AABB));
                    coordsAABB_i.Add(new vec3(max_x_AABB, min_y_AABB, min_z_AABB));
                    coordsAABB_i.Add(new vec3(max_x_AABB, max_y_AABB, min_z_AABB));
                    coordsAABB_i.Add(new vec3(min_x_AABB, max_y_AABB, min_z_AABB));

                    coordsAABB_f.Add(new vec3(min_x_AABB, min_y_AABB, max_z_AABB));
                    coordsAABB_f.Add(new vec3(max_x_AABB, min_y_AABB, max_z_AABB));
                    coordsAABB_f.Add(new vec3(max_x_AABB, max_y_AABB, max_z_AABB));
                    coordsAABB_f.Add(new vec3(min_x_AABB, max_y_AABB, max_z_AABB));
                }
                /*                       
                 *      if (CoordsSecao_f[i].x < min_x_AABB) min_x_AABB = CoordsSecao_f[i].x;
                        if (CoordsSecao_i[i].x < min_x_AABB) min_x_AABB = CoordsSecao_i[i].x;
                        if (CoordsSecao_f[i].y < min_y_AABB) min_y_AABB = CoordsSecao_f[i].y;
                        if (CoordsSecao_i[i].y < min_y_AABB) min_y_AABB = CoordsSecao_i[i].y;
                        if (CoordsSecao_f[i].z < min_z_AABB) min_z_AABB = CoordsSecao_f[i].z;
                        if (CoordsSecao_i[i].z < min_z_AABB) min_z_AABB = CoordsSecao_i[i].z;
                                                                          
                        if (CoordsSecao_f[i].x > max_x_AABB) max_x_AABB = CoordsSecao_f[i].x;
                        if (CoordsSecao_i[i].x > max_x_AABB) max_x_AABB = CoordsSecao_i[i].x;
                        if (CoordsSecao_f[i].y > max_y_AABB) max_y_AABB = CoordsSecao_f[i].y;
                        if (CoordsSecao_i[i].y > max_y_AABB) max_y_AABB = CoordsSecao_i[i].y;
                        if (CoordsSecao_f[i].z > max_z_AABB) max_z_AABB = CoordsSecao_f[i].z;
                        if (CoordsSecao_i[i].z > max_z_AABB) max_z_AABB = CoordsSecao_i[i].z;
                    }
                    
                    coordsAABB_i.Add(new vec3(min_x_AABB, min_y_AABB, min_z_AABB));
                    coordsAABB_i.Add(new vec3(max_x_AABB, min_y_AABB, min_z_AABB));
                    coordsAABB_i.Add(new vec3(max_x_AABB, max_y_AABB, min_z_AABB));
                    coordsAABB_i.Add(new vec3(min_x_AABB, max_y_AABB, min_z_AABB));
                                         
                    coordsAABB_f.Add(new vec3(min_x_AABB, min_y_AABB, max_z_AABB));
                    coordsAABB_f.Add(new vec3(max_x_AABB, min_y_AABB, max_z_AABB));
                    coordsAABB_f.Add(new vec3(max_x_AABB, max_y_AABB, max_z_AABB));
                    coordsAABB_f.Add(new vec3(min_x_AABB, max_y_AABB, max_z_AABB));*/
            }

            catch (Exception e)
            {
                System.Windows.Forms.MessageBox.Show(e.Message);
            }

        }
        public void OrientaSecaoNoEspaco2()
        {
           // CoordsSecao_f.Clear();
            if (Dados.secao.poligono == null)
                return;

            vec3 coord1 = new vec3(0, 0, 0);
            centroRotacao = new vec3(0, 0, 0);

            AcertaPontosNotacaoCientifica();

            cy = ((((pFin.z * -1) - (pIni.z * -1))) / comprimento);
            
            centroRotacao.x = Geom.Iguais(Dados.secaoSemRotacao.poligono.centroide.X,0)?0: Dados.secaoSemRotacao.poligono.centroide.X;
            centroRotacao.y = Geom.Iguais(Dados.secaoSemRotacao.poligono.centroide.Y, 0) ? 0 : Dados.secaoSemRotacao.poligono.centroide.Y;


            AlfaAlterado = 0;

            coordsAABB_f = new List<vec3>();
            coordsAABB_i = new List<vec3>();

            coordAABB_SemRotacao = new List<vec3>();
            coordAABB_SemRotacao.Add(new vec3(Dados.secaoSemRotacao.poligono.coords.Min(o => o.X), Dados.secaoSemRotacao.poligono.coords.Min(o => o.Y), 0));
            coordAABB_SemRotacao.Add(new vec3(Dados.secaoSemRotacao.poligono.coords.Max(o => o.X), Dados.secaoSemRotacao.poligono.coords.Min(o => o.Y), 0));
            coordAABB_SemRotacao.Add(new vec3(Dados.secaoSemRotacao.poligono.coords.Max(o => o.X), Dados.secaoSemRotacao.poligono.coords.Max(o => o.Y), 0));
            coordAABB_SemRotacao.Add(new vec3(Dados.secaoSemRotacao.poligono.coords.Min(o => o.X), Dados.secaoSemRotacao.poligono.coords.Max(o => o.Y), 0));

            for (i = 0; i < Dados.secaoSemRotacao.poligono.coords.Count(); i++)
            {
              //  Dados.secaoSemRotacao.poligono.coords[i].X += (-Dados.ey / 1000);
               // Dados.secaoSemRotacao.poligono.coords[i].Y += (-Dados.ez / 1000);
            }

            if (!Geom.Iguais(pIni.x, pFin.x))
            {
                if (pIni.x > pFin.x)
                {
                    AlfaAlterado = -Dados.anguloRotacao;
                    for (int i = 0; i < (Dados.secaoSemRotacao.poligono.coords.Count()); i++)
                    {
                        coord1 = new vec3(Dados.secaoSemRotacao.poligono.coords[i].X + (Dados.ey / 1000), -Dados.secaoSemRotacao.poligono.coords[i].Y + (-Dados.ez / 1000), 0);
                        coord1 = coord1.Rotate(centroRotacao, ((-Dados.anguloRotacao)) * Const.PIDiv180);

                        Dados.secao.poligono.coords[i].X = coord1.x; 
                        Dados.secao.poligono.coords[i].Y = coord1.y;
                    }
                }
                else
                {
                    AlfaAlterado = Dados.anguloRotacao;

                    for (int i = 0; i < (Dados.secaoSemRotacao.poligono.coords.Count()); i++)
                    {
                        coord1 = new vec3(-Dados.secaoSemRotacao.poligono.coords[i].X + (-Dados.ey / 1000), -Dados.secaoSemRotacao.poligono.coords[i].Y + (-Dados.ez / 1000), 0);
                        coord1 = coord1.Rotate(centroRotacao, ((Dados.anguloRotacao)) * Const.PIDiv180);

                        Dados.secao.poligono.coords[i].X = coord1.x;  
                        Dados.secao.poligono.coords[i].Y = coord1.y;
                    }
                }
            }
            else
            if (Geom.Iguais(Math.Abs(cy) , 1,0.000001))
            {
                AlfaAlterado = Dados.anguloRotacao + 90;

                if ((pIni.z * -1) < (pFin.z * -1))
                {
                    for (int i = 0; i < (Dados.secaoSemRotacao.poligono.coords.Count()); i++)
                    {
                        coord1 = new vec3(Dados.secaoSemRotacao.poligono.coords[i].X + (Dados.ey / 1000), Dados.secaoSemRotacao.poligono.coords[i].Y + (Dados.ez / 1000), 0);
                        coord1 = coord1.Rotate(centroRotacao, ((AlfaAlterado)) * Const.PIDiv180);

                        Dados.secao.poligono.coords[i].X = coord1.x;
                        Dados.secao.poligono.coords[i].Y = coord1.y;
                    }
                }
                else
                {
                    for (int i = 0; i < (Dados.secaoSemRotacao.poligono.coords.Count()); i++)
                    {
                        coord1 = new vec3(-Dados.secaoSemRotacao.poligono.coords[i].X + (-Dados.ey / 1000), -Dados.secaoSemRotacao.poligono.coords[i].Y + (-Dados.ez / 1000), 0);
                        coord1 = coord1.Rotate(centroRotacao, ((AlfaAlterado)) * Const.PIDiv180);

                        Dados.secao.poligono.coords[i].X = coord1.x;
                        Dados.secao.poligono.coords[i].Y = coord1.y;
                    }
                }
            }
            else
            {
                AlfaAlterado = Dados.anguloRotacao;

                for (int i = 0; i < (Dados.secaoSemRotacao.poligono.coords.Count()); i++)
                {
                    coord1 = new vec3(-Dados.secaoSemRotacao.poligono.coords[i].X + (-Dados.ey / 1000), -Dados.secaoSemRotacao.poligono.coords[i].Y + (-Dados.ez / 1000), 0);
                    coord1 = coord1.Rotate(centroRotacao, ((Dados.anguloRotacao)) * Const.PIDiv180);

                    Dados.secao.poligono.coords[i].X = coord1.x; 
                    Dados.secao.poligono.coords[i].Y = coord1.y;
                }
            }

//se tem offset cria um vetor no centro, aplica o offset e rotaciona, senao só cria o vetor no centro(0,0), que é o próprio ponto da linha_eixo
            vec3 pCentro_com_offset = new vec3(0);
            if (temOffset)
            {
                pCentro_com_offset = new vec3(-(Dados.ey / 1000), (-Dados.ez / 1000), 0);

                if (!Geom.Iguais(pIni.x, pFin.x))
                {
                    if (pIni.x > pFin.x)
                        pCentro_com_offset = new vec3((Dados.ey / 1000), (-Dados.ez / 1000), 0);
                }
                else
                if (Geom.Iguais(Math.Abs(cy), 1, 0.000001))
                {
                    if ((pIni.z * -1) < (pFin.z * -1))
                        pCentro_com_offset = new vec3((Dados.ey / 1000), (Dados.ez / 1000), 0);
                }
 
                pCentro_com_offset = pCentro_com_offset.Rotate(centroRotacao, ((AlfaAlterado)) * Const.PIDiv180);
            }
            else
                pCentro_com_offset = new vec3(0, 0, 0);

            pIni_Offset = new vec3(0);
            pFin_Offset = new vec3(0);

            qtd_CoordsSecao = Dados.secao.poligono.coords.Count();

            CoordsSecao_f = new vec3[qtd_CoordsSecao];
            CoordsSecao_i = new vec3[qtd_CoordsSecao];
            if (!Geom.Iguais(pIni.x, pFin.x))
            {
                double length = comprimento;
                if (pIni.x > pFin.x)
                {
                    length = -comprimento;
                }

                pIni_Offset.x = 0;
                pIni_Offset.y = pCentro_com_offset.x;
                pIni_Offset.z = pCentro_com_offset.y;

                pFin_Offset.x = length;
                pFin_Offset.y = pCentro_com_offset.x;
                pFin_Offset.z = pCentro_com_offset.y;

                for (i = 0; i < qtd_CoordsSecao; i++)
                {
                    CoordsSecao_i[i] = (new vec3(0, Dados.secao.poligono.coords[i].X, Dados.secao.poligono.coords[i].Y));
                    CoordsSecao_f[i] = new vec3(length, Dados.secao.poligono.coords[i].X, Dados.secao.poligono.coords[i].Y);
                }
            }
            else
            {
                pIni_Offset.x = 0;
                pIni_Offset.y = pCentro_com_offset.x;
                pIni_Offset.z = pCentro_com_offset.y;

                pFin_Offset.x = comprimento;
                pFin_Offset.y = pCentro_com_offset.x;
                pFin_Offset.z = pCentro_com_offset.y;

                for (i = 0; i < qtd_CoordsSecao; i++)
                {
                    CoordsSecao_i[i] = new vec3(0, Dados.secao.poligono.coords[i].X, Dados.secao.poligono.coords[i].Y);
                    CoordsSecao_f[i] = (new vec3(comprimento, Dados.secao.poligono.coords[i].X, Dados.secao.poligono.coords[i].Y));
                }
            }

            double xu = centroRotacao.x;
            double yu = centroRotacao.y;
            centroRotacao.x = 0;
            centroRotacao.y = xu;
            centroRotacao.z = yu;

           /* for (i = 0; i < qtd_CoordsSecao; i++)
            {
                CoordsSecao_f[i].y += -Dados.ey / 1000;
                CoordsSecao_f[i].z += -Dados.ez / 1000;

                CoordsSecao_i[i].y += -Dados.ey / 1000;
                CoordsSecao_i[i].z += -Dados.ez / 1000;
            }*/


            TriangularizarFaces();

            

            try
            {

                //  for (i = 0; i < CoordsSecao_i.Count; i++)
                {
                    xi = pIni.x;
                    xf = pFin.x;
                    yi = pIni.y;
                    yf = pFin.y;
                    zi = pIni.z;
                    zf = pFin.z;

                    if (Geom.Iguais(yi, 0))
                        yi = 0;
                    if (Geom.Iguais(yf, 0))
                        yf = 0;

                    L = (Math.Sqrt(Math.Pow(xi - xf, 2) + Math.Pow(yi - yf, 2) + Math.Pow(zi - zf, 2)));

                    zi_maior_que_zf = false;
                  //  if (! Geom.Iguais(Math.Abs(zf), Math.Abs(zi)))
                      zi_maior_que_zf = ((zi * -1) > (zf * -1));
                    
                    xi_igual_xf = Geom.Iguais(xi, xf);
 
                    tx = ( xi);
                    ty = ( yi);
                    tz = ( zi);

                    xi -= tx;
                    yi -= ty;
                    zi -= tz;

                    xf -= tx;
                    yf -= ty;
                    zf -= tz;

                    /*faço uma translação da barra para o ponto zero do sistema global, como se eu fizesse o comando mover do programa
                     * para o ponto zero pegando o pIni como pivo */

                    /**/

                    //  if (xi < xf)
                    cx = ((xi) - (xf)) / L;
                    //else
                    //    cx = ((xf) - (xi)) / L;

                    //  if (yi < yf)
                    cy = ((yi) - (yf)) / L;
                    //  else
                    //      cy = ((yf) - (yi)) / L;

                    cz = ((zi) - (zf)) / L;

                    u1 = new vec3(xi, yi, zi * -1);
                    u2 = new vec3(xf, yf, zf * -1);

                    //Encontrar angulo que a barra faz com os planos XY e XZ

                    //PLANO XY
                    normxy = new vec3(0, 0, 1);

                    //  if (xi < xf)
                    u = u1 - u2;
                    //else
                    //    u = u2 - u1;

                    NdotU = (normxy.DotProduct(u));
                    ndotu_mod = normxy.Magnitude() * u.Magnitude();
                    cos_alfa = Math.Abs(NdotU / ndotu_mod);
                    angXY = RMath.rad2deg(Math.Acos(cos_alfa));
                    angXY =/* RMath.rad2deg(Math.Atan(u1.z - u2.z / (u1.y - u2.y)));*/(90 - angXY) * 1;

                    //PLANO XZ
                    normxz = new vec3(0, 1, 0);
                    //  if (xi< xf)
                    u = u1 - u2;
                    //  else
                    //     u = u1 - u2;

                    NdotU = (normxz.DotProduct(u));
                    ndotu_mod = normxz.Magnitude() * u.Magnitude();
                    cos_alfa = Math.Abs(NdotU / ndotu_mod);
                 //   angXZ = RMath.rad2deg(Math.Acos(cos_alfa));

                    if (!Geom.Iguais(u1.x - u2.x, 0))
                       angXZ = RMath.rad2deg(Math.Atan(u1.y - u2.y / (u1.x - u2.x)));/* (90-angXZ) * 1;*/
                    else
                    {
                        if (!Geom.Iguais(yi, yf))
                        {
                            if (yi < yf)
                                angXZ = -90;
                            else
                                angXZ = 90;
                        }
                        else
                            angXZ = 90;
                    }

                    pos = new vec3(0, 0, 0);
                    xAnt = xi;

                    if (!xi_igual_xf)
                      if (xi < xf)
                        angXY *= -1;

                    if (!Geom.Iguais(Math.Abs(angXZ), 90))
                      angXZ *= -1;

                    if (xi_igual_xf)
                      angXY *= -1;

                    if (zi_maior_que_zf)
                      angXY *= -1;
 
                  /*  if (!zi_maior_que_zf && xi_igual_xf && (!Geom.Iguais(zf-zi, 0))))
                      angXY *= -1;*/

                    if (triangulos_face_1 != null)
                    {
                        for (i = 0; i < triangulos_face_1.Count(); i++)
                        {
                            for (j = 0; j < triangulos_face_1[i].vertices.Count(); j++)
                            {
                                xi = triangulos_face_1[i].vertices[j].x;
                                yi = triangulos_face_1[i].vertices[j].y;
                                zi = triangulos_face_1[i].vertices[j].z;

                                xf = triangulos_face_2[i].vertices[j].x;
                                yf = triangulos_face_2[i].vertices[j].y;
                                zf = triangulos_face_2[i].vertices[j].z;

                                posicao[1] = xi;
                                posicao[2] = yi;
                                posicao[3] = zi;
                                Geom.rotY(angXY, ref posicao, ref posicaoFinal);
                                Geom.rotZ(angXZ, ref posicaoFinal, ref posicaoFinal2);

                                triangulos_face_1[i].vertices[j].x = posicaoFinal2[1];
                                triangulos_face_1[i].vertices[j].y = posicaoFinal2[2];
                                triangulos_face_1[i].vertices[j].z = posicaoFinal2[3];

                                triangulos_face_1[i].vertices[j].x += tx;
                                triangulos_face_1[i].vertices[j].y += ty;
                                triangulos_face_1[i].vertices[j].z += tz;                                
                                
                                posicao[1] = xf;
                                posicao[2] = yf;
                                posicao[3] = zf;

                                Geom.rotY(angXY, ref posicao, ref posicaoFinal);
                                Geom.rotZ(angXZ, ref posicaoFinal, ref posicaoFinal2);

                                triangulos_face_2[i].vertices[j].x = posicaoFinal2[1];
                                triangulos_face_2[i].vertices[j].y = posicaoFinal2[2];
                                triangulos_face_2[i].vertices[j].z = posicaoFinal2[3];

                                triangulos_face_2[i].vertices[j].x += tx;
                                triangulos_face_2[i].vertices[j].y += ty;
                                triangulos_face_2[i].vertices[j].z += tz;   
                            }
                        }
                    }

                    min_z_AABB = 9999999; min_y_AABB = 9999999; min_x_AABB = 9999999;
                    max_z_AABB = -9999999; max_y_AABB = -9999999; max_x_AABB = -9999999;


                    /*--------------------------------------------*/
                    posicao[1] = pIni_Offset.x;
                    posicao[2] = pIni_Offset.y;
                    posicao[3] = pIni_Offset.z;

                    Geom.rotY(angXY, ref posicao, ref posicaoFinal);
                    Geom.rotZ(angXZ, ref posicaoFinal, ref posicaoFinal2);

                    pIni_Offset.x = posicaoFinal2[1] + tx;
                    pIni_Offset.y = posicaoFinal2[2] + ty;
                    pIni_Offset.z = posicaoFinal2[3] + tz;
                    pIni_Offset.z *= -1;
                    pIni_Offset.y *= -1;

                    posicao[1] = pFin_Offset.x;
                    posicao[2] = pFin_Offset.y;
                    posicao[3] = pFin_Offset.z;

                    Geom.rotY(angXY, ref posicao, ref posicaoFinal);
                    Geom.rotZ(angXZ, ref posicaoFinal, ref posicaoFinal2);

                    pFin_Offset.x = posicaoFinal2[1] + tx;
                    pFin_Offset.y = posicaoFinal2[2] + ty;
                    pFin_Offset.z = posicaoFinal2[3] + tz;
                    pFin_Offset.z *= -1;
                    pFin_Offset.y *= -1;

                    /*--------------------------------------------*/

                    /*--------------------------------------------*/
                    posicao[1] = centroRotacao.x;
                    posicao[2] = centroRotacao.y;
                    posicao[3] = centroRotacao.z;

                    Geom.rotY(angXY, ref posicao, ref posicaoFinal);
                    Geom.rotZ(angXZ, ref posicaoFinal, ref posicaoFinal2);

                    centroRotacao.x = posicaoFinal2[1] + tx;
                    centroRotacao.y = posicaoFinal2[2] + ty;
                    centroRotacao.z = posicaoFinal2[3] + tz;
                    centroRotacao.z *= -1;
                    centroRotacao.y *= -1;
                    /*--------------------------------------------*/

                    for (i = 0; i < CoordsSecao_i.Count(); i++)
                    {
                        xi = CoordsSecao_i[i].x;
                        yi = CoordsSecao_i[i].y;
                        zi = CoordsSecao_i[i].z;

                        xf = CoordsSecao_f[i].x;
                        yf = CoordsSecao_f[i].y;
                        zf = CoordsSecao_f[i].z;

                        posicao[1] = xi;
                        posicao[2] = yi;
                        posicao[3] = zi;
                        Geom.rotY(angXY, ref posicao, ref posicaoFinal);
                        Geom.rotZ(angXZ, ref posicaoFinal, ref posicaoFinal2);

                        CoordsSecao_i[i].x = posicaoFinal2[1];
                        CoordsSecao_i[i].y = posicaoFinal2[2];
                        CoordsSecao_i[i].z = posicaoFinal2[3];                      
                    
                        posicao[1] = xf;
                        posicao[2] = yf;
                        posicao[3] = zf;
                        
                        Geom.rotY(angXY, ref posicao, ref posicaoFinal);
                        Geom.rotZ(angXZ, ref posicaoFinal, ref posicaoFinal2);

                        CoordsSecao_f[i].x = posicaoFinal2[1];
                        CoordsSecao_f[i].y = posicaoFinal2[2];
                        CoordsSecao_f[i].z = posicaoFinal2[3];

                        CoordsSecao_i[i].x += tx;
                        CoordsSecao_i[i].y += ty;
                        CoordsSecao_i[i].z += tz;
                        CoordsSecao_i[i].z *= -1;
                        CoordsSecao_i[i].y *= -1;

                        CoordsSecao_f[i].x += tx;
                        CoordsSecao_f[i].y += ty;
                        CoordsSecao_f[i].z += tz;
                        CoordsSecao_f[i].y *= -1;
                        CoordsSecao_f[i].z *= -1;

                        CoordsSecao_f[i].pontoEmRaio = Dados.secao.poligono.coords[i].pontoEmRaio;
                        CoordsSecao_i[i].pontoEmRaio = Dados.secao.poligono.coords[i].pontoEmRaio;

                        if (CoordsSecao_f[i].x < min_x_AABB) min_x_AABB = CoordsSecao_f[i].x;
                        if (CoordsSecao_i[i].x < min_x_AABB) min_x_AABB = CoordsSecao_i[i].x;
                        if (CoordsSecao_f[i].y < min_y_AABB) min_y_AABB = CoordsSecao_f[i].y;
                        if (CoordsSecao_i[i].y < min_y_AABB) min_y_AABB = CoordsSecao_i[i].y;
                        if (CoordsSecao_f[i].z < min_z_AABB) min_z_AABB = CoordsSecao_f[i].z;
                        if (CoordsSecao_i[i].z < min_z_AABB) min_z_AABB = CoordsSecao_i[i].z;
                                                                          
                        if (CoordsSecao_f[i].x > max_x_AABB) max_x_AABB = CoordsSecao_f[i].x;
                        if (CoordsSecao_i[i].x > max_x_AABB) max_x_AABB = CoordsSecao_i[i].x;
                        if (CoordsSecao_f[i].y > max_y_AABB) max_y_AABB = CoordsSecao_f[i].y;
                        if (CoordsSecao_i[i].y > max_y_AABB) max_y_AABB = CoordsSecao_i[i].y;
                        if (CoordsSecao_f[i].z > max_z_AABB) max_z_AABB = CoordsSecao_f[i].z;
                        if (CoordsSecao_i[i].z > max_z_AABB) max_z_AABB = CoordsSecao_i[i].z;
                    }
                    
                    coordsAABB_i.Add(new vec3(min_x_AABB, min_y_AABB, min_z_AABB));
                    coordsAABB_i.Add(new vec3(max_x_AABB, min_y_AABB, min_z_AABB));
                    coordsAABB_i.Add(new vec3(max_x_AABB, max_y_AABB, min_z_AABB));
                    coordsAABB_i.Add(new vec3(min_x_AABB, max_y_AABB, min_z_AABB));
                                         
                    coordsAABB_f.Add(new vec3(min_x_AABB, min_y_AABB, max_z_AABB));
                    coordsAABB_f.Add(new vec3(max_x_AABB, min_y_AABB, max_z_AABB));
                    coordsAABB_f.Add(new vec3(max_x_AABB, max_y_AABB, max_z_AABB));
                    coordsAABB_f.Add(new vec3(min_x_AABB, max_y_AABB, max_z_AABB));
                }
            }

            catch (Exception e)
            {
                System.Windows.Forms.MessageBox.Show(e.Message);
            }

        }
        [NonSerialized]
        public vec3[] coords_box;
        [NonSerialized]
        OpenTK.Graphics.TextPrinter textoCota = new OpenTK.Graphics.TextPrinter(OpenTK.Graphics.TextQuality.High);
        Font fonte = new Font("Arial", 10);
        public TPonto getMiddlePoint()
        {
            return (pIni + pFin) / 2;
        }
        
        
        public override void OnMouseMove(ref TPonto point, bool ShowInfo, bool orto = true,
                                         double angulo = 0, bool PontoInicial = false)
        {
            //se está movendo a viga a partir do ponto inicial ou final
            if (PontoInicial)
            {
                pIni = (TPonto)point.Clone();
                pIni = (TPonto)point.Clone();
            }
            else
            {
                pFin.x = point.x;
                pFin.y = point.y;
                pFin.z = point.z;

                pFin.x = point.x;
                pFin.y = point.y;
                pFin.z = point.z;
            }
        }
        int qtd_CoordsSecao = 0;
        int j;

        void setListaCor(ref List<float> coord_objeto, double x, double y, double z)
        {
            coord_objeto.Add((float)x);
            coord_objeto.Add((float)y);
            coord_objeto.Add((float)z);
        }

        void setLista(ref List<float> coord_objeto, double x, double y, double z)
        {
            coord_objeto.Add((float)x);
            coord_objeto.Add((float)y);
            coord_objeto.Add((float)z);
        }
        void setListaTextura(ref List<float> coords_triangulos, double x, double y)
        {
            coords_triangulos.Add((float)x);
            coords_triangulos.Add((float)y);
        }
        [NonSerialized]
        double r, g, b;

        [NonSerialized]
        public vec3[] CoordsEixoLocal_X, CoordsEixoLocal_Y, CoordsEixoLocal_Z;
        [NonSerialized]
        public Seta seta_eixo_local_X, seta_eixo_local_Y, seta_eixo_local_Z;

        [NonSerialized]
        public Seta seta_eixo_local_Y_principal, seta_eixo_local_X_principal, seta_eixo_local_Z_principal;

        [NonSerialized]
        List<LinhaVec3> linhasArt;

        void DesenhaArticulacao(string momento, string posicao, double tam)
        {
            List<vec3> posArticulacao;
            
          //  if (posicao == "I")
                posArticulacao = new List<vec3>();
            
            double cx = (((pFin.x - pIni.x)) / comprimento);
            double cy = ((((pFin.y * -1) - (pIni.y * -1))) / comprimento);

            if (Geom.Iguais(cx, 0))
                cx = 0;
            if (Geom.Iguais(cy, 0))
                cy = 0;

            string eixoRodar;
            double ang = Dados.anguloRotacao;

            double local = 1;
            double raio = 1;
            if (posicao == "I")
            {
                local = 0.005 * comprimento;
                raio = tam;// * comprimento;
            }
            else
            if (posicao == "F")
            {
                local = 0.995 * comprimento ;
                raio = tam;// comprimento ;
            }

            if (Geom.Iguais(cx, 0) && Geom.Iguais(cy, 0))
                ang += 90;
            else
            {
                eixoRodar = "Z";
                if (!Geom.Iguais(pIni.x, pFin.x))
                    if (pIni.x > pFin.x)
                    {
                        ang *= -1;
                        local *= -1;
                    }
            }

            if (momento == "MY")
              ang += 90;

            eixoRodar = "Y";
            for (int j = 0; j <= 10; j++)
            {
                vec3 pp = new vec3(local + (raio * Math.Cos(j * 6.28318 / 10))
                                       , 0 + (raio * Math.Sin(j * 6.28318 / 10)), 0);

                Geom.GiraConformeAnguloAlfa(ref pp, eixoRodar, ang);

                posArticulacao.Add(pp);
            }

            for (int i = 0; i < posArticulacao.Count; i++)
            {
                vec3 p1 = posArticulacao[i];
                vec3 p2;

                if (i == posArticulacao.Count - 1)
                    p2 = posArticulacao[0];
                else
                    p2 = posArticulacao[i + 1];

                linhasArt.Add(new LinhaVec3(new vec3(p1.x, p1.y, p1.z), new vec3(p2.x, p2.y, p2.z)));
              
                p1 = linhasArt[linhasArt.Count-1].p1;
                p2 = linhasArt[linhasArt.Count - 1].p2;

                Geom.RotacionaLinhaNoEspaco(pIni.x, pIni.y, pIni.z, pFin.x, pFin.y, pFin.z, ref p1, ref p2);
                linhasArt[linhasArt.Count-1].p1.x = p1.x;   linhasArt[linhasArt.Count-1].p1.y = p1.y;   linhasArt[linhasArt.Count-1].p1.z = p1.z;
                linhasArt[linhasArt.Count - 1].p2.x = p2.x; linhasArt[linhasArt.Count - 1].p2.y = p2.y; linhasArt[linhasArt.Count - 1].p2.z = p2.z;
            }
        }

        public void CriarDesenhoArticulacoes(double tam)
        {
            /*<Nenhum>
            Início e fim
            Início
            Fim*/
            linhasArt = new List<LinhaVec3>();

            if (tam == 0)
                tam = 0.04;

            if (Dados.Articulacao_my == 1)
            {
                DesenhaArticulacao("MY", "I", tam);
                DesenhaArticulacao("MY", "F", tam);
            }
            else
            if (Dados.Articulacao_my == 2)
                DesenhaArticulacao("MY", "I", tam);
            else
            if (Dados.Articulacao_my == 3)
                DesenhaArticulacao("MY", "F", tam);

            if (Dados.Articulacao_mz == 1)
            {
                DesenhaArticulacao("MZ", "I", tam);
                DesenhaArticulacao("MZ", "F", tam);
            }
            else
            if (Dados.Articulacao_mz == 2)
                DesenhaArticulacao("MZ", "I", tam);
            else
            if (Dados.Articulacao_mz == 3)
                DesenhaArticulacao("MZ", "F", tam);
            
        }

        public void CriarEixosLocais(double tamArt = .04)
        {
            //CoordsEixoLocal_X = new vec3[2];
            seta_eixo_local_X = Geom.CriarSetaEixoLocalX(0.05 * comprimento, 0.5 * comprimento, pIni, pFin, Dados.anguloRotacao);
            //{
          //      tamanho = 0.05 * comprimento;
         //       local = 0.5 * comprimento;

      //          CriarSetaEixoLocalX();
          /*  Geom.RotacionaLinhaNoEspaco(pIni.x, pIni.y, pIni.z, 
                                        pFin.x, pFin.y, pFin.z,
                                        ref CoordsEixoLocal_X[0], ref CoordsEixoLocal_X[1]);*/

          //  CoordsEixoLocal_Y = new vec3[2];
            //CriarSetaEixoLocalY();
            seta_eixo_local_Y = Geom.CriarSetaEixoLocalY(0.05 * comprimento, 0.5 * comprimento, pIni, pFin, Dados.anguloRotacao);
            // Geom.RotacionaLinhaNoEspaco(pIni.x, pIni.y, pIni.z,
            //                           pFin.x, pFin.y, pFin.z,
            //                        ref CoordsEixoLocal_Y[0], ref CoordsEixoLocal_Y[1]);

            // CoordsEixoLocal_Z = new vec3[2];
           seta_eixo_local_Z = Geom.CriarSetaEixoLocalZ(0.05 * comprimento, 0.5 * comprimento, pIni, pFin, Dados.anguloRotacao);
            //  CriarSetaEixoLocalZ();
            //   Geom.RotacionaLinhaNoEspaco(pIni.x, pIni.y, pIni.z,
            //                       pFin.x, pFin.y, pFin.z,
            //                      ref CoordsEixoLocal_Z[0], ref CoordsEixoLocal_Z[1]);

            seta_eixo_local_X_principal = Geom.CriarSetaEixoLocalX(0.05 * comprimento, 0.2 * comprimento, pIni, pFin, Dados.anguloRotacao + (Dados.secao.propriedades.anguloEixosPrincipais / Const.PIDiv180));
            seta_eixo_local_Y_principal = Geom.CriarSetaEixoLocalY(0.05 * comprimento, 0.2 * comprimento, pIni, pFin, Dados.anguloRotacao + (Dados.secao.propriedades.anguloEixosPrincipais / Const.PIDiv180));
            seta_eixo_local_Z_principal = Geom.CriarSetaEixoLocalZ(0.05 * comprimento, 0.2 * comprimento, pIni, pFin, Dados.anguloRotacao + (Dados.secao.propriedades.anguloEixosPrincipais / Const.PIDiv180));

            CriarDesenhoArticulacoes(tamArt);
        }

        public void Preenche_Arestas_EixosLocais(ref List<float> coords_arestas)
        {

            if (Visivel)
            {
                /*for (int i = 0; i < CoordsEixoLocal_X.Count(); i++)
                {
                    setLista(ref coords_arestas, CoordsEixoLocal_X[i].x, CoordsEixoLocal_X[i].y, CoordsEixoLocal_X[i].z);
                    setLista(ref coords_arestas, 1, 0, 0);
                }*/

                double rx = 1; double gx =0; double bx = 0;
                double ry = 0; double gy = 0.8; double by = 0.2;
                double rz = 0; double gz = 0; double bz = 1;
                if (Selecionado)
                {
                    rx = 1; gx = 0.5; bx = 0;
                    ry = 1; gy = 0.5; by = 0;
                    rz = 1; gz = 0.5; bz = 0;
                }

                LinhaVec3 lin = new LinhaVec3();
                for (int i = 0; i < 7; i++)
                {
                    if (i == 0)
                        lin = seta_eixo_local_X.l1;else
                    if (i == 1)
                        lin = seta_eixo_local_X.l2;else
                    if (i == 2)
                        lin = seta_eixo_local_X.l3;else
                    if (i == 3)
                        lin = seta_eixo_local_X.l4;else
                    if (i == 4)
                        lin = seta_eixo_local_X.l5;else
                    if (i == 5)
                        lin = seta_eixo_local_X.l6;else
                    if (i == 6)
                        lin = seta_eixo_local_X.l_principal;

                    setLista(ref coords_arestas, lin.p1.x, lin.p1.y, lin.p1.z);
                    setLista(ref coords_arestas, rx, gx, bx);
                    setLista(ref coords_arestas, lin.p2.x, lin.p2.y, lin.p2.z);
                    setLista(ref coords_arestas, rx, gx, bx);
                }

                for (int i = 0; i < 7; i++)
                {
                    if (i == 0)
                        lin = seta_eixo_local_Y.l1;
                    else
                    if (i == 1)
                        lin = seta_eixo_local_Y.l2;
                    else
                    if (i == 2)
                        lin = seta_eixo_local_Y.l3;
                    else
                    if (i == 3)
                        lin = seta_eixo_local_Y.l4;
                    else
                    if (i == 4)
                        lin = seta_eixo_local_Y.l5;
                    else
                    if (i == 5)
                        lin = seta_eixo_local_Y.l6;
                    else
                    if (i == 6)
                        lin = seta_eixo_local_Y.l_principal;

                    setLista(ref coords_arestas, lin.p1.x, lin.p1.y, lin.p1.z);
                    setLista(ref coords_arestas, ry, gy, by);
                    setLista(ref coords_arestas, lin.p2.x, lin.p2.y, lin.p2.z);
                    setLista(ref coords_arestas, ry, gy, by);
                }

                for (int i = 0; i < 7; i++)
                {
                    if (i == 0)
                        lin = seta_eixo_local_Z.l1;
                    else
                    if (i == 1)
                        lin = seta_eixo_local_Z.l2;
                    else
                    if (i == 2)
                        lin = seta_eixo_local_Z.l3;
                    else
                    if (i == 3)
                        lin = seta_eixo_local_Z.l4;
                    else
                    if (i == 4)
                        lin = seta_eixo_local_Z.l5;
                    else
                    if (i == 5)
                        lin = seta_eixo_local_Z.l6;
                    else
                    if (i == 6)
                        lin = seta_eixo_local_Z.l_principal;

                    setLista(ref coords_arestas, lin.p1.x, lin.p1.y, lin.p1.z);
                    setLista(ref coords_arestas, rz, gz, bz);
                    setLista(ref coords_arestas, lin.p2.x, lin.p2.y, lin.p2.z);
                    setLista(ref coords_arestas, rz, gz, bz);
                }

             //   Preenche_Arestas_EixosPrincipais(ref coords_arestas);

                /*  for (int i = 0; i < 2; i++)
                  {
                      setLista(ref coords_arestas, CoordsEixoLocal_Y[i].x, CoordsEixoLocal_Y[i].y, CoordsEixoLocal_Y[i].z);
                      setLista(ref coords_arestas, 0, 0.8, 0.2);
                  }*/

                /* for (int i = 0; i < 2; i++)
                 {
                     setLista(ref coords_arestas, CoordsEixoLocal_Z[i].x, CoordsEixoLocal_Z[i].y, CoordsEixoLocal_Z[i].z);
                     setLista(ref coords_arestas, 0, 0, 1);
                 }*/
            }
        }
        public void Preenche_Arestas_EixosPrincipais(ref List<float> coords_arestas)
        {

            if (Visivel)
            {
                /*for (int i = 0; i < CoordsEixoLocal_X.Count(); i++)
                {
                    setLista(ref coords_arestas, CoordsEixoLocal_X[i].x, CoordsEixoLocal_X[i].y, CoordsEixoLocal_X[i].z);
                    setLista(ref coords_arestas, 1, 0, 0);
                }*/

                double rx = 1; double gx = 0; double bx = 0;
                double ry = 0; double gy = 0.8; double by = 0.2;
                double rz = 0; double gz = 0; double bz = 1;
                if (Selecionado)
                {
                    rx = 1; gx = 0.5; bx = 0;
                    ry = 1; gy = 0.5; by = 0;
                    rz = 1; gz = 0.5; bz = 0;
                }

                LinhaVec3 lin = new LinhaVec3();
                for (int i = 0; i < 7; i++)
                {
                    if (i == 0)
                        lin = seta_eixo_local_X_principal.l1;
                    else
                    if (i == 1)
                        lin = seta_eixo_local_X_principal.l2;
                    else
                    if (i == 2)
                        lin = seta_eixo_local_X_principal.l3;
                    else
                    if (i == 3)
                        lin = seta_eixo_local_X_principal.l4;
                    else
                    if (i == 4)
                        lin = seta_eixo_local_X_principal.l5;
                    else
                    if (i == 5)
                        lin = seta_eixo_local_X_principal.l6;
                    else
                    if (i == 6)
                        lin = seta_eixo_local_X_principal.l_principal;

                    setLista(ref coords_arestas, lin.p1.x, lin.p1.y, lin.p1.z);
                    setLista(ref coords_arestas, rx, gx, bx);
                    setLista(ref coords_arestas, lin.p2.x, lin.p2.y, lin.p2.z);
                    setLista(ref coords_arestas, rx, gx, bx);
                }

                for (int i = 0; i < 7; i++)
                {
                    if (i == 0)
                        lin = seta_eixo_local_Y_principal.l1;
                    else
                    if (i == 1)
                        lin = seta_eixo_local_Y_principal.l2;
                    else
                    if (i == 2)
                        lin = seta_eixo_local_Y_principal.l3;
                    else
                    if (i == 3)
                        lin = seta_eixo_local_Y_principal.l4;
                    else
                    if (i == 4)
                        lin = seta_eixo_local_Y_principal.l5;
                    else
                    if (i == 5)
                        lin = seta_eixo_local_Y_principal.l6;
                    else
                    if (i == 6)
                        lin = seta_eixo_local_Y_principal.l_principal;

                    setLista(ref coords_arestas, lin.p1.x, lin.p1.y, lin.p1.z);
                    setLista(ref coords_arestas, ry, gy, by);
                    setLista(ref coords_arestas, lin.p2.x, lin.p2.y, lin.p2.z);
                    setLista(ref coords_arestas, ry, gy, by);
                }

                for (int i = 0; i < 7; i++)
                {
                    if (i == 0)
                        lin = seta_eixo_local_Z_principal.l1;
                    else
                    if (i == 1)
                        lin = seta_eixo_local_Z_principal.l2;
                    else
                    if (i == 2)
                        lin = seta_eixo_local_Z_principal.l3;
                    else
                    if (i == 3)
                        lin = seta_eixo_local_Z_principal.l4;
                    else
                    if (i == 4)
                        lin = seta_eixo_local_Z_principal.l5;
                    else
                    if (i == 5)
                        lin = seta_eixo_local_Z_principal.l6;
                    else
                    if (i == 6)
                        lin = seta_eixo_local_Z_principal.l_principal;

                    setLista(ref coords_arestas, lin.p1.x, lin.p1.y, lin.p1.z);
                    setLista(ref coords_arestas, rz, gz, bz);
                    setLista(ref coords_arestas, lin.p2.x, lin.p2.y, lin.p2.z);
                    setLista(ref coords_arestas, rz, gz, bz);
                }
                /*  for (int i = 0; i < 2; i++)
                  {
                      setLista(ref coords_arestas, CoordsEixoLocal_Y[i].x, CoordsEixoLocal_Y[i].y, CoordsEixoLocal_Y[i].z);
                      setLista(ref coords_arestas, 0, 0.8, 0.2);
                  }*/

                /* for (int i = 0; i < 2; i++)
                 {
                     setLista(ref coords_arestas, CoordsEixoLocal_Z[i].x, CoordsEixoLocal_Z[i].y, CoordsEixoLocal_Z[i].z);
                     setLista(ref coords_arestas, 0, 0, 1);
                 }*/
            }
        }
        public void Preenche_Arestas_Articulacoes(ref List<float> coords_arestas)
        {
            if (Visivel && (linhasArt != null))
            {
                double rx = 0; double gx = 0; double bx = 0;
                rx = (double)Rgb[0] / 255;
                gx = (double)Rgb[1] / 255;
                bx = (double)Rgb[2] / 255;

                if (Selecionado)
                {
                    rx = 1; gx = 0.5; bx = 0;
                }
                LinhaVec3 lin = new LinhaVec3();

                for (int i = 0; i < linhasArt.Count; i++)
                {
                    setLista(ref coords_arestas, linhasArt[i].p1.x, linhasArt[i].p1.y, linhasArt[i].p1.z);
                    setLista(ref coords_arestas, rx, gx, bx);
                    setLista(ref coords_arestas, linhasArt[i].p2.x, linhasArt[i].p2.y, linhasArt[i].p2.z);
                    setLista(ref coords_arestas, rx, gx, bx);
                }
            }
        }
        public void AtualizaArestas(bool arestas, bool arestasConfObjeto, bool unifilar)
        {
            List<float> coords = new List<float>();

            Preenche_Arestas(ref coords, ref arestas, ref arestasConfObjeto, ref unifilar);

            BatchArestas = coords.ToArray();

            DirtyArestas = false;
        }


        public void Preenche_Arestas(ref List<float> coords_arestas, ref bool arestas, ref bool arestasConfObjeto, ref bool unifilar)
        {
            
            if (Visivel)
            {
                if (Selecionado)
                {
                    r = 1;
                    g = 0;
                    b = 0;

                    if (realcar)
                      GL.LineWidth(3);
                }
                else
                {
                    r = (double)Rgb[0] / 255;
                    g = (double)Rgb[1] / 255;
                    b = (double)Rgb[2] / 255;
                }

              //  if (unifilar || Dados.Tipo == 4)
                {
                    setLista(ref coords_arestas, pIni.x, pIni.y, pIni.z);
                    setLista(ref coords_arestas, r, g, b);
                    setLista(ref coords_arestas, pFin.x, pFin.y, pFin.z);
                    setLista(ref coords_arestas, r, g, b);

                    if (temOffset && !unifilar)
                    {
                        setLista(ref coords_arestas, pIni_Offset.x, -pIni_Offset.y, -pIni_Offset.z);
                        setLista(ref coords_arestas, r, g, b);
                        setLista(ref coords_arestas, pIni.x, pIni.y, pIni.z);
                        setLista(ref coords_arestas, r, g, b);

                        setLista(ref coords_arestas, pFin_Offset.x, -pFin_Offset.y, -pFin_Offset.z);
                        setLista(ref coords_arestas, r, g, b);
                        setLista(ref coords_arestas, pFin.x, pFin.y, pFin.z);
                        setLista(ref coords_arestas, r, g, b);
                    }
                }
                /*   for (int i = 0; i < 2; i++)
                   {
                       setLista(ref coords_arestas, CoordsEixoLocal_X[i].x, CoordsEixoLocal_X[i].y, CoordsEixoLocal_X[i].z);
                       setLista(ref coords_arestas, 1, 0, 0);
                   }
                   for (int i = 0; i < 2; i++)
                   {
                       setLista(ref coords_arestas, CoordsEixoLocal_Y[i].x, CoordsEixoLocal_Y[i].y, CoordsEixoLocal_Y[i].z);
                       setLista(ref coords_arestas, 0, 0.8, 0.2);
                   }
                   for (int i = 0; i < 2; i++)
                   {
                       setLista(ref coords_arestas, CoordsEixoLocal_Z[i].x, CoordsEixoLocal_Z[i].y, CoordsEixoLocal_Z[i].z);
                       setLista(ref coords_arestas, 0, 0, 1);
                   }*/
                
                GL.LineWidth(1);

                if (Dados.Tipo == 4) 
                    return;

                if (((!unifilar && (arestas)) || (!unifilar && Selecionado)))
                {
                    if (Selecionado)
                    {
                        r = 1;
                        g = 0;
                        b = 0;
                    }
                    else
                    if (arestas)
                    {
                        if (arestasConfObjeto)
                        {
                            r = (double)Rgb[0] / 255;
                            g = (double)Rgb[1] / 255;
                            b = (double)Rgb[2] / 255;
                        }
                        else
                        {
                            r = 0;
                            g = 0;
                            b = 0;
                        }
                    }

                    for (int q = 0; q < coordssecao_i.Count; q++)
                    {
                        for (i = 0; i < coordssecao_i[q].Count() - 1; i++)
                        {
                            if (!coordssecao_i[q][i].pontoEmRaio || (coordssecao_i[q][i].pontoEmRaio && Selecionado))
                            {
                                setLista(ref coords_arestas, coordssecao_i[q][i].x, -coordssecao_i[q][i].y, -coordssecao_i[q][i].z);
                                setLista(ref coords_arestas, r, g, b);
                                setLista(ref coords_arestas, coordssecao_f[q][i].x, -coordssecao_f[q][i].y, -coordssecao_f[q][i].z);
                                setLista(ref coords_arestas, r, g, b);
                            }

                            setLista(ref coords_arestas, coordssecao_i[q][i].x, -coordssecao_i[q][i].y, -coordssecao_i[q][i].z);
                            setLista(ref coords_arestas, r, g, b);
                            setLista(ref coords_arestas, coordssecao_i[q][i + 1].x, -coordssecao_i[q][i + 1].y, -coordssecao_i[q][i + 1].z);
                            setLista(ref coords_arestas, r, g, b);

                            setLista(ref coords_arestas, coordssecao_f[q][i].x, -coordssecao_f[q][i].y, -coordssecao_f[q][i].z);
                            setLista(ref coords_arestas, r, g, b);
                            setLista(ref coords_arestas, coordssecao_f[q][i + 1].x, -coordssecao_f[q][i + 1].y, -coordssecao_f[q][i + 1].z);
                            setLista(ref coords_arestas, r, g, b);

                        }
                    }
                    //teste de pintura do boundingbox
                  /*  for (i = 0; i < 4; i++)
                    {
                        setLista(ref coords_arestas, coordsAABB_i[i].x, -coordsAABB_i[i].y, -coordsAABB_i[i].z);
                        setLista(ref coords_arestas, r, g, b);
                        setLista(ref coords_arestas, coordsAABB_f[i].x, -coordsAABB_f[i].y, -coordsAABB_f[i].z);
                        setLista(ref coords_arestas, r, g, b);
                    }*/
                }
            }
            GL.LineWidth(1);
        }
        void AddTriangulo(vec3 p1, vec3 p2, vec3 p3, ref List<Triangulo> triangulos_selecao)
        {
            tr = new Triangulo(-1, "boundingbox");
            tr.p0 = p1;
            tr.p1 = p2;
            tr.p2 = p3;

            v1 = p2 - p1;
            v2 = p3 - p1;
            n1 = v1.CrossProduct(v2);
            n1.Normalize();

            if (!Geom.Iguais(pIni.x, pFin.x))
            {
                if (pIni.x > pFin.x)
                {
                    n1.x *= -1;
                    n1.y *= -1;
                    n1.z *= -1;
                }
            }

            tr._normal.x = -n1.x;
            tr._normal.y = -n1.y;
            tr._normal.z = -n1.z;
            tr.CriaPlano(tr.p0);
            tr.idBarra = this.IDBarra;
            triangulos_selecao.Add(tr);
        }

        public void Preenche_Triangulos_BoundingBox(ref List<Triangulo> triangulos_selecao)
        {
            /*    bounding box
                2_____ 1
               /|     /|
              / |    / |
             /  |3__/__|0
            2__/__1/  /
            | /   |  /
            |/    | /
            3_____0

            */


            AddTriangulo(new vec3(coordsAABB_i[0].x, -coordsAABB_i[0].y, -coordsAABB_i[0].z),
                         new vec3(coordsAABB_f[0].x, -coordsAABB_f[0].y, -coordsAABB_f[0].z),
                         new vec3(coordsAABB_f[1].x, -coordsAABB_f[1].y, -coordsAABB_f[1].z), ref triangulos_selecao);
            AddTriangulo(new vec3(coordsAABB_i[0].x, -coordsAABB_i[0].y, -coordsAABB_i[0].z),
                         new vec3(coordsAABB_f[1].x, -coordsAABB_f[1].y, -coordsAABB_f[1].z),
                         new vec3(coordsAABB_i[1].x, -coordsAABB_i[1].y, -coordsAABB_i[1].z), ref triangulos_selecao);
           
            AddTriangulo(new vec3(coordsAABB_i[1].x, -coordsAABB_i[1].y, -coordsAABB_i[1].z),
                         new vec3(coordsAABB_f[1].x, -coordsAABB_f[1].y, -coordsAABB_f[1].z),
                         new vec3(coordsAABB_f[2].x, -coordsAABB_f[2].y, -coordsAABB_f[2].z), ref triangulos_selecao);
            AddTriangulo(new vec3(coordsAABB_i[1].x, -coordsAABB_i[1].y, -coordsAABB_i[1].z),
                         new vec3(coordsAABB_f[2].x, -coordsAABB_f[2].y, -coordsAABB_f[2].z),
                         new vec3(coordsAABB_i[2].x, -coordsAABB_i[2].y, -coordsAABB_i[2].z), ref triangulos_selecao);
          
            AddTriangulo(new vec3(coordsAABB_i[2].x, -coordsAABB_i[2].y, -coordsAABB_i[2].z),
                         new vec3(coordsAABB_f[2].x, -coordsAABB_f[2].y, -coordsAABB_f[2].z),
                         new vec3(coordsAABB_i[3].x, -coordsAABB_i[3].y, -coordsAABB_i[3].z), ref triangulos_selecao);
            AddTriangulo(new vec3(coordsAABB_i[3].x, -coordsAABB_i[3].y, -coordsAABB_i[3].z),
                         new vec3(coordsAABB_f[2].x, -coordsAABB_f[2].y, -coordsAABB_f[2].z),
                         new vec3(coordsAABB_f[3].x, -coordsAABB_f[3].y, -coordsAABB_f[3].z), ref triangulos_selecao);

            AddTriangulo(new vec3(coordsAABB_i[0].x, -coordsAABB_i[0].y, -coordsAABB_i[0].z),
                         new vec3(coordsAABB_f[0].x, -coordsAABB_f[0].y, -coordsAABB_f[0].z),
                         new vec3(coordsAABB_f[3].x, -coordsAABB_f[3].y, -coordsAABB_f[3].z), ref triangulos_selecao);
            AddTriangulo(new vec3(coordsAABB_i[0].x, -coordsAABB_i[0].y, -coordsAABB_i[0].z),
                         new vec3(coordsAABB_f[3].x, -coordsAABB_f[3].y, -coordsAABB_f[3].z),
                         new vec3(coordsAABB_i[3].x, -coordsAABB_i[3].y, -coordsAABB_i[3].z), ref triangulos_selecao);

            AddTriangulo(new vec3(coordsAABB_i[0].x, -coordsAABB_i[0].y, -coordsAABB_i[0].z),
                         new vec3(coordsAABB_i[1].x, -coordsAABB_i[1].y, -coordsAABB_i[1].z),
                         new vec3(coordsAABB_i[3].x, -coordsAABB_i[3].y, -coordsAABB_i[3].z), ref triangulos_selecao);
            AddTriangulo(new vec3(coordsAABB_i[3].x, -coordsAABB_i[3].y, -coordsAABB_i[3].z),
                         new vec3(coordsAABB_i[1].x, -coordsAABB_i[1].y, -coordsAABB_i[1].z),
                         new vec3(coordsAABB_i[2].x, -coordsAABB_i[2].y, -coordsAABB_i[2].z), ref triangulos_selecao);

            AddTriangulo(new vec3(coordsAABB_f[0].x, -coordsAABB_f[0].y, -coordsAABB_f[0].z),
                         new vec3(coordsAABB_f[1].x, -coordsAABB_f[1].y, -coordsAABB_f[1].z),
                         new vec3(coordsAABB_f[3].x, -coordsAABB_f[3].y, -coordsAABB_f[3].z), ref triangulos_selecao);
            AddTriangulo(new vec3(coordsAABB_f[3].x, -coordsAABB_f[3].y, -coordsAABB_f[3].z),
                         new vec3(coordsAABB_f[1].x, -coordsAABB_f[1].y, -coordsAABB_f[1].z),
                         new vec3(coordsAABB_f[2].x, -coordsAABB_f[2].y, -coordsAABB_f[2].z), ref triangulos_selecao);
        }

        [NonSerialized]
        Triangulo tr;
        [NonSerialized]
        public float[] BatchTriangulos;
        [NonSerialized]
        public Triangulo[] TriangulosSelecao;
        [NonSerialized]
        public float[] BatchArestas;
        public bool DirtyTriangulos = true;
        public bool DirtySelecao = true;

        public bool DirtyArestas = true;
        public bool DirtySelecaoArestas = true;

        public int OffsetVBO;
        public int TamanhoVBO;
        public void AtualizaTriangulos(bool visualizando_deformacao, bool unifiliar)
        {
            List<float> coords = new List<float>();
            List<Triangulo> selecao = new List<Triangulo>();


            PreencheTriangulos(ref coords, ref selecao, visualizando_deformacao, unifiliar);


            BatchTriangulos = coords.ToArray();
            TriangulosSelecao = selecao.ToArray();

            DirtyTriangulos = false;
            DirtySelecao = false;
        }
        public void PreencheTriangulos(ref List<float> coords_triangulos, ref List<Triangulo> triangulos_selecao, bool visualizando_deformacao, bool unifiliar)
        {

            if (Dados.Tipo == 4) return;

            //os triangulos selecao tem que preencher no modo de deformacao, assim o usuario pode fazer as rotacoes no pivo aproximado
            if (Visivel || visualizando_deformacao)
            {
                for (int q = 0; q < coordssecao_i.Count; q++)
                {
                    for (i = 0; i < coordssecao_i[q].Count() - 1; i++)
                    {                      
                        p1 = new vec3(coordssecao_i[q][i].x, -coordssecao_i[q][i].y, -coordssecao_i[q][i].z);
                        p2 = new vec3(coordssecao_f[q][i].x, -coordssecao_f[q][i].y, -coordssecao_f[q][i].z);
                        p3 = new vec3(coordssecao_f[q][i + 1].x, -coordssecao_f[q][i + 1].y, -coordssecao_f[q][i + 1].z);
                        v1 = p2 - p1;
                        v2 = p3 - p1;
                        n1 = v1.CrossProduct(v2);
                        n1.Normalize();

                        if (!Dados.secaoSemRotacao.poligonos[q].externo)
                           n1 *= -1;

                        tr = new Triangulo(-1, "barra");
                        tr.p0 = new vec3(coordssecao_i[q][i].x, -coordssecao_i[q][i].y, -coordssecao_i[q][i].z);
                        tr.p1 = new vec3(coordssecao_f[q][i].x, -coordssecao_f[q][i].y, -coordssecao_f[q][i].z);
                        tr.p2 = new vec3(coordssecao_f[q][i + 1].x, -coordssecao_f[q][i + 1].y, -coordssecao_f[q][i + 1].z);
                        tr._normal.x = -n1.x;
                        tr._normal.y = -n1.y;
                        tr._normal.z = -n1.z;
                        tr.CriaPlano(tr.p0);
                        tr.idBarra = this.IDBarra;
                        triangulos_selecao.Add(tr);

                        tr = new Triangulo(-1, "barra");
                        tr.p0 = new vec3(coordssecao_i[q][i].x, -coordssecao_i[q][i].y, -coordssecao_i[q][i].z);
                        tr.p1 = new vec3(coordssecao_f[q][i + 1].x, -coordssecao_f[q][i + 1].y, -coordssecao_f[q][i + 1].z);
                        tr.p2 = new vec3(coordssecao_i[q][i + 1].x, -coordssecao_i[q][i + 1].y, -coordssecao_i[q][i + 1].z);
                        tr._normal.x = -n1.x;
                        tr._normal.y = -n1.y;
                        tr._normal.z = -n1.z;
                        tr.CriaPlano(tr.p0);
                        tr.idBarra = this.IDBarra;
                        triangulos_selecao.Add(tr);
                    }
                }

                if (triangulos_face_1 != null)
                {
                    p1 = new vec3(triangulos_face_1[0].vertices[0].x, triangulos_face_1[0].vertices[0].y, triangulos_face_1[0].vertices[0].z);
                    p2 = new vec3(triangulos_face_1[0].vertices[1].x, triangulos_face_1[0].vertices[1].y, triangulos_face_1[0].vertices[1].z);
                    p3 = new vec3(triangulos_face_1[0].vertices[2].x, triangulos_face_1[0].vertices[2].y, triangulos_face_1[0].vertices[2].z);
                    v1 = p2 - p1;
                    v2 = p3 - p1;
                    n1 = v1.CrossProduct(v2);
                    n1.Normalize();
                    n1 *= -1;
                    if (!Geom.Iguais(pIni.x, pFin.x))
                    {
                        if (pIni.x > pFin.x)
                            n1 *= -1;
                    }

                    for (i = 0; i < triangulos_face_1.Count(); i++)
                    {
                        tr = new Triangulo(-1, "barra");
                        tr.p0 = new vec3(triangulos_face_1[i].vertices[0].x, triangulos_face_1[i].vertices[0].y, triangulos_face_1[i].vertices[0].z);
                        tr.p1 = new vec3(triangulos_face_1[i].vertices[1].x, triangulos_face_1[i].vertices[1].y, triangulos_face_1[i].vertices[1].z);
                        tr.p2 = new vec3(triangulos_face_1[i].vertices[2].x, triangulos_face_1[i].vertices[2].y, triangulos_face_1[i].vertices[2].z);
                        tr._normal.x = n1.x;
                        tr._normal.y = n1.y;
                        tr._normal.z = n1.z;
                        tr.CriaPlano(tr.p0);
                        tr.idBarra = this.IDBarra;
                        triangulos_selecao.Add(tr);

                        tr = new Triangulo(-1, "barra");
                        tr.p0 = new vec3(triangulos_face_2[i].vertices[0].x, triangulos_face_2[i].vertices[0].y, triangulos_face_2[i].vertices[0].z);
                        tr.p1 = new vec3(triangulos_face_2[i].vertices[1].x, triangulos_face_2[i].vertices[1].y, triangulos_face_2[i].vertices[1].z);
                        tr.p2 = new vec3(triangulos_face_2[i].vertices[2].x, triangulos_face_2[i].vertices[2].y, triangulos_face_2[i].vertices[2].z);
                        tr._normal.x = -n1.x;
                        tr._normal.y = -n1.y;
                        tr._normal.z = -n1.z;
                        tr.CriaPlano(tr.p0);
                        tr.idBarra = this.IDBarra;

                        triangulos_selecao.Add(tr);
                    }
                }
            }

            //se a barra estiver visivel, preenche apenas a lista coords_triangulos, mas 
            if (Visivel && !unifiliar)
            {
              /*  if (Selecionado)
                {
                    r = 0.8;
                    g = 0.4;
                    b = 0.3;
                }
                else
                {*/
                    r = (double)Rgb[0] / 255;
                    g = (double)Rgb[1] / 255;
                    b = (double)Rgb[2] / 255;
                //   }

                for (int q = 0; q < coordssecao_i.Count; q++)
                {
                    for (i = 0; i < coordssecao_i[q].Count() - 1; i++)
                    {
                        p1 = new vec3(coordssecao_i[q][i].x, -coordssecao_i[q][i].y, -coordssecao_i[q][i].z);
                        p2 = new vec3(coordssecao_f[q][i].x, -coordssecao_f[q][i].y, -coordssecao_f[q][i].z);
                        p3 = new vec3(coordssecao_f[q][i + 1].x, -coordssecao_f[q][i + 1].y, -coordssecao_f[q][i + 1].z);
                        v1 = p2 - p1;
                        v2 = p3 - p1;
                        n1 = v1.CrossProduct(v2);
                        n1.Normalize();

                        if (!Dados.secaoSemRotacao.poligonos[q].externo)
                            n1 *= -1;
                        /*if (!Geom.Iguais(pIni.x, pFin.x))
                                       {
                                           if (pIni.x > pFin.x)
                                           {
                                               n1.x *= -1;
                                               n1.y *= -1;
                                               n1.z *= -1;
                                           }
                                       }*/

                        // TRIANGULO 1 //
                        setLista(ref coords_triangulos, coordssecao_i[q][i].x, -coordssecao_i[q][i].y, -coordssecao_i[q][i].z);
                        setLista(ref coords_triangulos, -n1.x, -n1.y, -n1.z);
                        setLista(ref coords_triangulos, r, g, b);
                        setListaTextura(ref coords_triangulos, 1, 0);

                        setLista(ref coords_triangulos, coordssecao_f[q][i].x, -coordssecao_f[q][i].y, -coordssecao_f[q][i].z);
                        setLista(ref coords_triangulos, -n1.x, -n1.y, -n1.z);
                        setLista(ref coords_triangulos, r, g, b);
                        setListaTextura(ref coords_triangulos, 1, 1);

                        setLista(ref coords_triangulos, coordssecao_f[q][i + 1].x, -coordssecao_f[q][i + 1].y, -coordssecao_f[q][i + 1].z);
                        setLista(ref coords_triangulos, -n1.x, -n1.y, -n1.z);
                        setLista(ref coords_triangulos, r, g, b);
                        setListaTextura(ref coords_triangulos, 0, 1);

                        // TRIANGULO 2 //
                        setLista(ref coords_triangulos, coordssecao_i[q][i].x, -coordssecao_i[q][i].y, -coordssecao_i[q][i].z);
                        setLista(ref coords_triangulos, -n1.x, -n1.y, -n1.z);
                        setLista(ref coords_triangulos, r, g, b);
                        setListaTextura(ref coords_triangulos, 0, 1);

                        setLista(ref coords_triangulos, coordssecao_f[q][i + 1].x, -coordssecao_f[q][i + 1].y, -coordssecao_f[q][i + 1].z);
                        setLista(ref coords_triangulos, -n1.x, -n1.y, -n1.z);
                        setLista(ref coords_triangulos, r, g, b);
                        setListaTextura(ref coords_triangulos, 0, 0);

                        setLista(ref coords_triangulos, coordssecao_i[q][i + 1].x, -coordssecao_i[q][i + 1].y, -coordssecao_i[q][i + 1].z);
                        setLista(ref coords_triangulos, -n1.x, -n1.y, -n1.z);
                        setLista(ref coords_triangulos, r, g, b);
                        setListaTextura(ref coords_triangulos, 1, 0);
                        // if (!CoordsSecao_i[i].pontoEmRaio)
                        {
                            tr = new Triangulo(-1, "barra");
                            tr.p0 = new vec3(coordssecao_i[q][i].x, -coordssecao_i[q][i].y, -coordssecao_i[q][i].z);
                            tr.p1 = new vec3(coordssecao_f[q][i].x, -coordssecao_f[q][i].y, -coordssecao_f[q][i].z);
                            tr.p2 = new vec3(coordssecao_f[q][i + 1].x, -coordssecao_f[q][i + 1].y, -coordssecao_f[q][i + 1].z);
                            tr._normal.x = -n1.x;
                            tr._normal.y = -n1.y;
                            tr._normal.z = -n1.z;
                            tr.CriaPlano(tr.p0);
                            tr.idBarra = this.IDBarra;
                            //    triangulos_selecao.Add(tr);

                            tr = new Triangulo(-1, "barra");
                            tr.p0 = new vec3(coordssecao_i[q][i].x, -coordssecao_i[q][i].y, -coordssecao_i[q][i].z);
                            tr.p1 = new vec3(coordssecao_f[q][i + 1].x, -coordssecao_f[q][i + 1].y, -coordssecao_f[q][i + 1].z);
                            tr.p2 = new vec3(coordssecao_i[q][i + 1].x, -coordssecao_i[q][i + 1].y, -coordssecao_i[q][i + 1].z);
                            tr._normal.x = -n1.x;
                            tr._normal.y = -n1.y;
                            tr._normal.z = -n1.z;
                            tr.CriaPlano(tr.p0);
                            tr.idBarra = this.IDBarra;
                            //  triangulos_selecao.Add(tr);
                        }
                    }
                }

                if (triangulos_face_1 != null)
                {
                    p1 = new vec3(triangulos_face_1[0].vertices[0].x, triangulos_face_1[0].vertices[0].y, triangulos_face_1[0].vertices[0].z);
                    p2 = new vec3(triangulos_face_1[0].vertices[1].x, triangulos_face_1[0].vertices[1].y, triangulos_face_1[0].vertices[1].z);
                    p3 = new vec3(triangulos_face_1[0].vertices[2].x, triangulos_face_1[0].vertices[2].y, triangulos_face_1[0].vertices[2].z);
                    v1 = p2 - p1;
                    v2 = p3 - p1;
                    n1 = v1.CrossProduct(v2);
                    n1.Normalize();
                    n1 *= -1;
                    if (!Geom.Iguais(pIni.x, pFin.x))
                    {
                        if (pIni.x > pFin.x)
                          n1 *= -1;
                    }


                    for (i = 0; i < triangulos_face_1.Count(); i++)
                    {
                        for (j = 0; j < triangulos_face_1[i].vertices.Count(); j++)
                        {
                            setLista(ref coords_triangulos, triangulos_face_1[i].vertices[j].x, triangulos_face_1[i].vertices[j].y, triangulos_face_1[i].vertices[j].z);
                            setLista(ref coords_triangulos, n1.x, n1.y, n1.z);
                            setLista(ref coords_triangulos, r, g, b);
                            setListaTextura(ref coords_triangulos, 0, 0);
                        }

                        tr = new Triangulo(-1, "barra");
                        tr.p0 = new vec3(triangulos_face_1[i].vertices[0].x, triangulos_face_1[i].vertices[0].y, triangulos_face_1[i].vertices[0].z);
                        tr.p1 = new vec3(triangulos_face_1[i].vertices[1].x, triangulos_face_1[i].vertices[1].y, triangulos_face_1[i].vertices[1].z);
                        tr.p2 = new vec3(triangulos_face_1[i].vertices[2].x, triangulos_face_1[i].vertices[2].y, triangulos_face_1[i].vertices[2].z);
                        tr._normal.x = n1.x;
                        tr._normal.y = n1.y;
                        tr._normal.z = n1.z;
                        tr.CriaPlano(tr.p0);
                        tr.idBarra = this.IDBarra;
                     //   triangulos_selecao.Add(tr);


                        for (j = 0; j < triangulos_face_2[i].vertices.Count(); j++)
                        {
                            setLista(ref coords_triangulos, triangulos_face_2[i].vertices[j].x, triangulos_face_2[i].vertices[j].y, triangulos_face_2[i].vertices[j].z);
                            setLista(ref coords_triangulos, -n1.x, -n1.y, -n1.z);
                            setLista(ref coords_triangulos, r, g, b);
                            setListaTextura(ref coords_triangulos, 0, 0);
                        }

                        tr = new Triangulo(-1, "barra");
                        tr.p0 = new vec3(triangulos_face_2[i].vertices[0].x, triangulos_face_2[i].vertices[0].y, triangulos_face_2[i].vertices[0].z);
                        tr.p1 = new vec3(triangulos_face_2[i].vertices[1].x, triangulos_face_2[i].vertices[1].y, triangulos_face_2[i].vertices[1].z);
                        tr.p2 = new vec3(triangulos_face_2[i].vertices[2].x, triangulos_face_2[i].vertices[2].y, triangulos_face_2[i].vertices[2].z);
                        tr._normal.x = -n1.x;
                        tr._normal.y = -n1.y;
                        tr._normal.z = -n1.z;
                        tr.CriaPlano(tr.p0);
                        tr.idBarra = this.IDBarra;
                       
                     //   triangulos_selecao.Add(tr);
                    }
                }
            }
        }

        public override void Desenha(ref bool Unifilar, ref int transp, ref  bool arestas)
        {
         //   if (Visivel)
            {
                try
                {
                  /*  GL.Color4(Color.FromArgb(transp, Rgb[0], Rgb[1], Rgb[2]));//100 100 255

                    if (base.Selecionado)
                    {
                        GL.LineWidth(3);
                        GL.Color3(Color.Red);
                    }*/

                   // if (Unifilar || (qtd_CoordsSecao == 0))
                    {
                      //  GL.Disable(EnableCap.Lighting);
                     //   if (!base.Selecionado)
                          GL.Color4(Color.FromArgb(transp, Rgb[0], Rgb[1], Rgb[2]));

                        GL.Begin(PrimitiveType.Lines);
                        GL.Vertex3(pIni.x, pIni.y, pIni.z);
                        GL.Vertex3(pFin.x, pFin.y, pFin.z);
                        GL.End();
                       // GL.Enable(EnableCap.Lighting);
                    }
                   // else
                    {     
                      /*  if (Selecionado)
                           GL.Color3(Color.Red);
                       else
                       if (arestas)
                           GL.Color3(Color.Black);

                       if (criando)
                       {
                           GL.Color3(Color.FromArgb(transp, Rgb[0], Rgb[1], Rgb[2])); 
                           GL.Begin(PrimitiveType.Lines);
                           GL.Vertex3(pIni.x, pIni.y, pIni.z);
                           GL.Vertex3(pFin.x, pFin.y, pFin.z);
                           GL.End();
                       }*/

                       /*if (arestas || Selecionado)
                       {
                           for (i = 0; i < qtd_CoordsSecao - 1; i++)
                           {
                               GL.Begin(PrimitiveType.LineLoop);

                               GL.Vertex3(CoordsSecao_i[i].x, -CoordsSecao_i[i].y, -CoordsSecao_i[i].z);
                               GL.Vertex3(CoordsSecao_f[i].x, -CoordsSecao_f[i].y, -CoordsSecao_f[i].z);

                               GL.Vertex3(CoordsSecao_f[i + 1].x, -CoordsSecao_f[i + 1].y, -CoordsSecao_f[i + 1].z);
                               GL.Vertex3(CoordsSecao_i[i + 1].x, -CoordsSecao_i[i + 1].y, -CoordsSecao_i[i + 1].z);

                               GL.End();
                           }
                       }*/

                       /* GL.Color4(Color.FromArgb(transp, Rgb[0], Rgb[1], Rgb[2]));

                        for (i = 0; i < qtd_CoordsSecao - 1; i++)
                        {
                            {
                                p1 = new vec3(CoordsSecao_i[i].x, -CoordsSecao_i[i].y, -CoordsSecao_i[i].z);
                                p2 = new vec3(CoordsSecao_f[i].x, -CoordsSecao_f[i].y, -CoordsSecao_f[i].z);
                                p3 = new vec3(CoordsSecao_f[i + 1].x, -CoordsSecao_f[i + 1].y, -CoordsSecao_f[i + 1].z);
                                v1 = p2 - p1;
                                v2 = p3 - p1;
                                n1 = v1.CrossProduct(v2);
                                n1.Normalize();

                                GL.Begin(PrimitiveType.Polygon);
                                GL.Normal3(-n1.x, -n1.y, -n1.z);
                                GL.Vertex3(CoordsSecao_i[i].x, -CoordsSecao_i[i].y, -CoordsSecao_i[i].z);
                                GL.Vertex3(CoordsSecao_f[i].x, -CoordsSecao_f[i].y, -CoordsSecao_f[i].z);

                                GL.Vertex3(CoordsSecao_f[i + 1].x, -CoordsSecao_f[i + 1].y, -CoordsSecao_f[i + 1].z);
                                GL.Vertex3(CoordsSecao_i[i + 1].x, -CoordsSecao_i[i + 1].y, -CoordsSecao_i[i + 1].z);
                                GL.End();
                            }
                        }*/
                    }
                  //  GL.LineWidth(1);
                }

                catch (Exception ms)
                {
                   System.Windows.Forms.MessageBox.Show("erro ao desenha barra: barra" + this.Dados.numero.ToString() + "  -  " + ms.Message);
                }
            }
        }
        int i;
        [NonSerialized]
        vec3 n1, v1, v2, p1, p2, p3;
        public int idBarraCopiada;
        public void Copy(TBarraGenerica obj)
        {
            base.Copy(obj);
            if ((Object)obj.Dados != null)
            {
                this.Dados = (TDadosBarra)obj.Dados.Clone();
              /*  if ((Object)Dados.Poligono != null)
                  foreach (TLinha lin in Dados.Poligono.linhas_poligonal)
                      lin.layer = this.layer;*/
            }
            this.ArticulacaoFim = obj.ArticulacaoFim;
            this.ArticulacaoIni = obj.ArticulacaoIni;
            this.barra_offset = obj.barra_offset;
            
            if ((Object)obj.pIni_org != null)
                this.pIni_org = (TPonto)obj.pIni_org.Clone();
            if ((Object)obj.pFin_org != null)
                this.pFin_org = (TPonto)obj.pFin_org.Clone();

            if (obj.ids_barras_rigidas != null)
            {
                this.ids_barras_rigidas = new List<int>();
                this.ids_barras_rigidas = obj.ids_barras_rigidas.ToList();
            }

            this.Rgb[0] = obj.Rgb[0];
            this.Rgb[1] = obj.Rgb[1];
            this.Rgb[2] = obj.Rgb[2];
            this.seta_eixo_local_Y = obj.seta_eixo_local_Y.Clone();
           /// this.id_barra_rigida_1 = obj.id_barra_rigida_1;
          ///  this.id_barra_rigida_2 = obj.id_barra_rigida_2;

            this.idBarraCopiada = obj.IDBarra;

            NaoPermiteMoverOuCopiar = false;
            if ((Object)obj.PesoProprio != null)
                this.PesoProprio = (TCargaLinear)obj.PesoProprio.Clone();

            if ((Object)obj.Linha_Eixo != null)
              Linha_Eixo = (TLinha)obj.Linha_Eixo.Clone();
         //   Estrutura = obj.Estrutura;
            comprimento = obj.comprimento;
            if ((Object)obj.pIni != null)
              pIni = (TPonto)obj.pIni.Clone();
            if ((Object)obj.pFin != null)
              pFin = (TPonto)obj.pFin.Clone();
            Tipo = obj.Tipo;
            IdLayer = obj.IdLayer;
            Layer = obj.Layer;
            layer = obj.layer; 
            Visivel    = obj.Visivel;
            qtd_CoordsSecao = obj.qtd_CoordsSecao;
            ObjetoCopia = obj.ObjetoCopia;
            temOffset = obj.temOffset;
            pIni_Offset = obj.pIni_Offset;
            pFin_Offset = obj.pFin_Offset;

            if ((Object)obj.Dados != null)
            {
              //  CoordsSecao_f = new vec3[obj.Dados.secao.poligono.coords.Count()];
              //  CoordsSecao_i = new vec3[obj.Dados.secao.poligono.coords.Count()];

                coordssecao_i = new List<vec3[]>(obj.Dados.secao.poligonos.Count);
                coordssecao_f = new List<vec3[]>(obj.Dados.secao.poligonos.Count);

            }

            if ((Object)obj.pIni != null)
              this.OrientaSecaoNoEspaco();
        }

        public override TObjetoDesenho Clone()
        {
            TBarraGenerica l = new TBarraGenerica();
            l.Copy(this);
            return l;
        }

    }

}
