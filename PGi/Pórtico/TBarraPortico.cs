using OpenTK;
using OpenTK.Graphics.OpenGL;
using OpenTK.Input;
using OpenTK.Platform;
using Poly2Tri;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Net.NetworkInformation;
using System.Runtime.Serialization.Json;
using System.Security.Policy;
using System.ServiceModel.Security;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Markup.Localizer;
using System.Windows.Media.Converters;

namespace PG
{
    public class InfoBarraPortico
    {
        public string C1 { get; set; }
        public string C2 { get; set; }
        public string C3 { get; set; }
        public string C4 { get; set; }
        public string C5 { get; set; }
        public string C6 { get; set; }
        public string C7 { get; set; }
        public string C8 { get; set; }
        public string c9 { get; set; }
        public string C10 { get; set; }
        public string C11 { get; set; }
        public string C12 { get; set; }
        public InfoBarraPortico()
        {

        }
        public InfoBarraPortico(string col1, string col2, string col3, string col4, string col5, string col6, string col7, string col8, string col9, string col10, string col11, string col12)
        {
            this.C1 = col1;
            this.C2 = col2;
            this.C3 = col3;
            this.C4 = col4;
            this.C5 = col5;
            this.C6 = col6;
            this.C7 = col7;
            this.C8 = col8;
            this.c9 = col9;
            this.C10 = col10;
            this.C11 = col11;
            this.C12 = col12;
        }
    }

    public struct Tensoes_Barra
    {
        public double[] tensoes_i, tensoes_f;
        public int id;//id da combinacao ou do caso 
        public Tensoes_Barra(double[] _tensoes_i, double[] _tensoes_f, int _id)
        {
            tensoes_i = _tensoes_i;
            tensoes_f = _tensoes_f;

            this.id = _id;
        }
    }

    public class Esforcos_Barra
    {
        public double[] DeslocamentosLocais;
        public double[] DeslocamentosLocaisBarraSemiRigida;

        public double[] DeslocamentosGlobais;
        public double[] Esforcos;
        public double[] forcasLocais;
        public int id; // id do caso ou da combinacao

        public double MyMolaIni, MzMolaIni, MyMolaFim, MzMolaFim; // momentos nas molas, caso seja uma barra semirigida
        public Esforcos_Barra(int _id)
        {
            DeslocamentosLocais = new double[13];
            DeslocamentosGlobais = new double[13];
            DeslocamentosLocaisBarraSemiRigida =  new double[13]; ;
            Esforcos = new double[13];
            forcasLocais = new double[13];
            id = _id;
        }
    }
    public class ForcasLocais_Barra
    {
        public double[] forcasLocais, forcasLocais_sem_offset;
        public int id; // id do caso ou da combinacao
        public ForcasLocais_Barra(int _id)
        {
            forcasLocais = new double[13];
            forcasLocais_sem_offset = new double[13];

            id = _id;
        }
    }
    [Serializable]
    public partial class TBarraPortico : TObjetoDesenho
    {
        #region Variáveis
        int i, j, k;

        public List<ForcasLocais_Barra> combinacoes_x_forcasLocais;
        public List<ForcasLocais_Barra> casos_x_forcasLocais;

        public List<Esforcos_Barra> combinacoes_x_esforcos;
        public List<Esforcos_Barra> casos_x_esforcos;

        public List<Tensoes_Barra> combinacoes_x_tensoes;
        public List<Tensoes_Barra> casos_x_tensoes;

        public vec3 coordIsoDeslocamento;

        public TNoPortico pIni;
        public TNoPortico pFin;

        public TNoPortico pIni_offset;
        public TNoPortico pFin_offset;

        public int NEE; // número externo do elemento. Ordem em que foi inserido na geração da malha

        public int[] GlGlobal; /*vetor que retorna o gl global em função do gl local*/
        public bool PreSelecionada;
        private double cos_alpha, cos_teta, sen_alpha, sen_teta;
        public double alfa;
        public bool InseridaManualmente;
        public double comprimento,
                      carga,
                      E1, Iz1, Iy1, J1, G1, L, A1,
                      angulo,
                      anguloGlobal,
                      areaSecao,
                      CargaDistribuida;
         
        public double cargaPontual,
                      posA_cargaPontual, posB_cargaPontual;

        public bool  barraRigida,
                     barra90, barraVertical, barraHorizontal, barraOriginal_Vertical,
                     barraObliqua,barraViga,barraPilar,
                     direcaoX, direcaoY,
                     selecaoPorProximidade, selecaoPorPontoMedio, contornoLajeLinhaEixoViga, aberturaLaje,
                     barraErro, flag, BarraDeExtremidade;

        public int numViga, numLaje
                   , indiceNoIni, indiceNoFin
                   , IDBarra
                   , CodigoObjetosTela;
     
        [NonSerialized]
        public TTrechoViga TrechoViga;
        [NonSerialized]
        public TPilar Pilar; // se for barra rígida, aqui se guarda o pilar a qual ela pertence

        [NonSerialized]
        public double[,] RGB_Deslocamentos;

        [NonSerialized]
        public double[,] RGB_TensaoNormalPositivos;
        [NonSerialized]
        public double[,] RGB_TensaoNormalNegativos;

        [NonSerialized]
        public double[,] RGB_Axiais;


        [NonSerialized]
        public double[,] RGB_TorcoresNegativos;
        [NonSerialized]
        public double[,] RGB_TorcoresPositivos;

        [NonSerialized]
        public double[,] RGB_CortantesNegativos;
        [NonSerialized]
        public double[,] RGB_CortantesPositivos;

        public double[,] MatrizLocal, MatrizLocal_sem_offset, MatrizOffset, MatrizGeometrica,
                         MatrizGlobal, MatrizOffset_Transposta,
                         MatrizRotacao,SubMatrizRotacao,
                         MatRotacaoTransposta,
                         M;
        [NonSerialized]
        public TDadosBarra Dados;
        double[] DeslocamentosLocais;
        public double[] DeslocamentosGlobais;
        public double[] Esforcos;
        [NonSerialized]
        public TBarraGenerica barraOriginal;

        public vec3[] CoordsFletorY, CoordsFletorZ, CoordsCortante, CoordsAxial;
        private vec3[] FletorYColorido, CortanteColorido, TorcorColorido;
        ///  TPoligono[] Fletor, Torcor, Cortante;

        double[] coordLocal;
        double[] coordGlobal;

        bool proxima;

        int iFletorY = 0;
        int iFletorZ = 1;
        int iTorcor = 2;
        int iCortanteY = 3;
        int iCortanteZ = 4;
        int iAxial = 5;

        #endregion
        public TBarraPortico(): base(){}
        public TBarraPortico(TNoPortico pInicial, TNoPortico pFinal)
        {
            this.pIni = pInicial;
            this.pFin = pFinal;
        }
        public void MouseMove(double x, double y, double z)
        {
            pFin.x = x;
            pFin.y = y;
            pFin.z = z;
        }
        public TBarraPortico(TNoPortico pInicial, TNoPortico pFinal, 
              double E, double I, double G, double J,
              float cargaDistribuida,
              int numViga, int numLaje,
              int indiceNoIni, int indiceNoFin, int CodigoObjetosTela, double[,] matrizBarra,
              bool barraViga = false,
              bool barraPilar = false,
              bool barraVertical = false,
              bool barraHorizontal = false,
              int NEE = -1,
              bool barraObliqua = false)
        {
            this.Visivel = true;
            this.NEE = NEE - 1;
            this.pIni = pInicial;
            this.pFin = pFinal;

            this.E1 = E;
            this.A1 = 1;
            this.G1 = G;

            this.Iz1 = 1;
            this.Iy1 = 1;
            this.J1 = 1;

            this.L = comprimento;
            this.numViga = numViga;
            this.numLaje = numLaje;
            this.indiceNoIni = indiceNoIni;
            this.indiceNoFin = indiceNoFin;
            this.barraViga = barraViga;
            this.barraPilar = barraPilar;
            this.barraHorizontal = barraHorizontal;
            this.barraVertical = barraVertical;
            this.CargaDistribuida = cargaDistribuida;

            this.CodigoObjetosTela = CodigoObjetosTela;
            this.GlGlobal = new int[13];
            this.DeslocamentosLocais = new double[13];
            this.DeslocamentosGlobais = new double[13];
            this.Esforcos = new double[13];
            base.Tipo = Const.ID_BARRAGRELHA;

            combinacoes_x_esforcos = new List<Esforcos_Barra>();
            casos_x_esforcos       = new List<Esforcos_Barra>();

            combinacoes_x_tensoes = new List<Tensoes_Barra>();
            casos_x_tensoes = new List<Tensoes_Barra>();

            combinacoes_x_forcasLocais = new List<ForcasLocais_Barra>();
            casos_x_forcasLocais = new List<ForcasLocais_Barra>();

            coordLocal  = new double[13];
            coordGlobal = new double[13];

            coordLocal[1] = pIni.x;
            coordLocal[2] = pIni.y;
            coordLocal[3] = pIni.z;

            coordLocal[4] = pFin.x;
            coordLocal[5] = pFin.y;
            coordLocal[6] = pFin.z;
            
            coordsForca = new vec3[4];
            coordsFY = new vec3[4];
            coordsFletorY = new vec3[4];
            coordsFletorZ = new vec3[4];
            coordsTorcor = new vec3[4];
            coordsSuavizadas = new vec3[4];

            for (int n = 0; n < 4; n++)
            {
                coordsForca[n] = new vec3(0);
                coordsFY[n] = new vec3(0);
                coordsFletorY[n] = new vec3(0);
                coordsFletorZ[n] = new vec3(0);
                coordsTorcor[n] = new vec3(0);
                coordsSuavizadas[n] = new vec3(0);
            }

        }

        public TBarraPortico(TNoPortico pInicial, TNoPortico pFinal, TTrechoViga trecho, TPilar pilar,
                      double E, double I, double G, double J,
                      float cargaDistribuida, 
                      int numViga, int numLaje,
                      int indiceNoIni, int indiceNoFin, int CodigoObjetosTela, double[,] matrizBarra,
                      bool barraViga = false, 
                      bool barraPilar = false,
                      bool barraVertical = false, 
                      bool barraHorizontal = false,
                      int NEE = -1,
                      bool barraObliqua = false)   
		 {
            this.Visivel           = true; 
            this.NEE = NEE - 1;
            this.pIni              = pInicial;
			this.pFin              = pFinal;
			
            this.E1 = E;
            this.A1 = 1;
            this.G1 = G ;
            
            this.Iz1 = 1;
            this.Iy1 = 1;
            this.J1  = 1;

            this.L                 = comprimento;
            this.numViga           = numViga;
			this.numLaje           = numLaje;
			this.indiceNoIni       = indiceNoIni;
			this.indiceNoFin       = indiceNoFin;
            this.barraViga         = barraViga;
            this.barraPilar        = barraPilar;
            this.barraHorizontal   = barraHorizontal;
            this.barraVertical     = barraVertical;
            this.CargaDistribuida  = cargaDistribuida;
            this.TrechoViga        = trecho;
            this.Pilar = pilar;
            this.CodigoObjetosTela    = CodigoObjetosTela;
            this.GlGlobal             = new int[13];
            this.DeslocamentosLocais  = new double[13];
            this.DeslocamentosGlobais = new double[13];
            this.Esforcos             = new double[13];
            base.Tipo                 = Const.ID_BARRAGRELHA;

            combinacoes_x_esforcos = new List<Esforcos_Barra>();
            casos_x_esforcos = new List<Esforcos_Barra>();

            combinacoes_x_tensoes = new List<Tensoes_Barra>();
            casos_x_tensoes = new List<Tensoes_Barra>();

            combinacoes_x_forcasLocais = new List<ForcasLocais_Barra>();
            casos_x_forcasLocais = new List<ForcasLocais_Barra>();

            coordLocal  = new double[13];
            coordGlobal = new double[13];
            
            coordLocal[1] = pIni.x;
            coordLocal[2] = pIni.y;
            coordLocal[3] = pIni.z;
            
            coordLocal[4] = pFin.x;
            coordLocal[5] = pFin.y;
            coordLocal[6] = pFin.z;
		 }
      
        [NonSerialized]
        public List<vec3[]> coordssecao_i, coordssecao_i_temp;
        [NonSerialized]
        public List<vec3[]> coordssecao_f, coordssecao_f_temp;

        [NonSerialized]
        vec3 u1, u2, u, normxy, normxz;
        double NdotU, ndotu_mod, cos_alfa,  divisoes, divisoesFrac, xAnt;
        public double angXY, angXZ;

        public int qtd_CoordsSecao = 0;
        List<vec3> CoordsSubdvisao = new List<vec3>();
        double[] posicao = new double[4];
        double[] posicaoFinal = new double[4];
        double[] posicaoFinal2 = new double[4];
        double[] posicaoFinal_Trans = new double[5];

        double tx, ty, tz;
        bool zi_maior_que_zf, xi_igual_xf;
        vec3 pos;
        double xi, yi, xf, yf, zi, zf;
        vec3 coord1 = new vec3(0, 0, 0);
        vec3 coord2 = new vec3(0, 0, 0);
        vec3 centroRotacao = new vec3(0, 0, 0);
        TSecao secaoCopia_i, secaoCopia_f;

        public double AlfaAlterado, AlfaEixosPrincipaisAlterado;
        public bool apoio_simples_pIni, apoio_simples_pFin;
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
        double r, g, b;
        void SetaTriangulo(ref double x1,  double y1, double z1,
                   ref double x2,  double y2, double z2,
                   ref double x3,  double y3, double z3,
                   ref List<float> coords_triangulos, 
                   ref List<Triangulo> triangulos_selecao, 
                   ref double nx, ref double ny, ref double nz, 
                   bool inverterNormal = false)
        {
         /*   p1 = new vec3(x1, y1, z1);
            p2 = new vec3(x2, y2, z2);
            p3 = new vec3(x3, y3, z3);
            v1 = p2 - p1;
            v2 = p3 - p1;
            n1 = v1.CrossProduct(v2);
            n1.Normalize();
            if (inverterNormal)
                n1 *= -1;

            if (!Geom.Iguais(pIni.x, pFin.x))
            {
                if (pIni.x > pFin.x)
                  n1 *= -1;
            }*/
            setLista(ref coords_triangulos, x1, y1, z1);
            setLista(ref coords_triangulos, nx, ny, nz);
            setLista(ref coords_triangulos, r, g, b);
            setListaTextura(ref coords_triangulos, 1, 1);

            setLista(ref coords_triangulos, x2, y2, z2);
            setLista(ref coords_triangulos, nx, ny, nz);
            setLista(ref coords_triangulos, r, g, b);
            setListaTextura(ref coords_triangulos, 1, 1);

            setLista(ref coords_triangulos, x3, y3, z3);
            setLista(ref coords_triangulos, nx, ny, nz);
            setLista(ref coords_triangulos, r, g, b);
            setListaTextura(ref coords_triangulos, 1, 1);

            Triangulo tr = new Triangulo(-1, "barra portico");
            tr.p0 = new vec3(x1, y1, z1);
            tr.p1 = new vec3(x2, y2, z2);
            tr.p2 = new vec3(x3, y3, z3);
            tr._normal.x = -nx;
            tr._normal.y = -ny;
            tr._normal.z = -nz;
            tr.CriaPlano(tr.p0);
            tr.idBarraPortico = IDBarra;
        //    triangulos_selecao.Add(tr);
        }
        void SetaTriangulo(ref double x1, double y1, double z1,
           ref double x2, double y2, double z2,
           ref double x3, double y3, double z3,
           double r1, double g1, double b1,
           double r2, double g2, double b2,
           double r3, double g3, double b3,
           ref List<float> coords_triangulos)
        {
            setLista(ref coords_triangulos, x1, y1, z1);
            setLista(ref coords_triangulos, r1, g1, b1);

            setLista(ref coords_triangulos, x2, y2, z2);
            setLista(ref coords_triangulos, r2, g2, b2);
            
            setLista(ref coords_triangulos, x3, y3, z3);
            setLista(ref coords_triangulos, r3, g3, b3);
        }
     
        double[] pIni_Deslocamento, pFin_Deslocamento;

        void SetaTriangulo(
           double x1, double y1, double z1,
           double x2, double y2, double z2,
           double x3, double y3, double z3,
           ref List<float> coords_triangulos, 
           ref List<Triangulo> triangulos_selecao,
           ref double nx, ref double ny, ref double nz,
           double r1, double g1, double b1,
           double r2, double g2, double b2,
           double r3, double g3, double b3,
           bool selecionar = true)
        {
            setLista(ref coords_triangulos, x1, y1, z1);
            setLista(ref coords_triangulos, nx, ny, nz);
            setLista(ref coords_triangulos, r1, g1, b1);
            setListaTextura(ref coords_triangulos, 1, 1);

            setLista(ref coords_triangulos, x2, y2, z2);
            setLista(ref coords_triangulos, nx, ny, nz);
            setLista(ref coords_triangulos, r2, g2, b2);
            setListaTextura(ref coords_triangulos, 1, 1);

            setLista(ref coords_triangulos, x3, y3, z3);
            setLista(ref coords_triangulos, nx, ny, nz);
            setLista(ref coords_triangulos, r3, g3, b3);
            setListaTextura(ref coords_triangulos, 1, 1);

            if (selecionar)
            {
                Triangulo tr = new Triangulo(-1, "barra portico");
                tr.p0 = new vec3(x1, y1, z1);
                tr.p1 = new vec3(x2, y2, z2);
                tr.p2 = new vec3(x3, y3, z3);
                tr._normal.x = -nx;
                tr._normal.y = -ny;
                tr._normal.z = -nz;
                tr.CriaPlano(tr.p0);
                tr.idBarraPortico = IDBarra;
                triangulos_selecao.Add(tr);
            }
        }
        vec3[] tensoes;
        public void PreencheTriangulosTensoes(ref List<float> coords_triangulos,
                                ref List<Triangulo> triangulos_selecao,
                                bool MostrarIndeformada,
                                ref double MultiplicadorAltura,
                                ref bool arestas,
                                ref bool LinhaContorno,
                                ref int Deformacao_U,
                                int tipocarga,
                                int caso,
                                int comb,
                                bool tensaoGradiente,
                                bool isoBandasTensoes)
        {
            try
            {
                if (barra_de_articulacao || barraRigida || coordssecao_f_temp == null) return;

                //if ((selecionados && barraOriginal.Selecionado) || (!selecionados))
                {

                    r = (double)Rgb[0] / 255;
                    g = (double)Rgb[1] / 255;
                    b = (double)Rgb[2] / 255;

                    if (tipocarga == 0)
                    {
                        pIni_Deslocamento = pIni.casos_x_deslocamentos[caso].DeslocamentoGlobal;
                        pFin_Deslocamento = pFin.casos_x_deslocamentos[caso].DeslocamentoGlobal;
                    }
                    else
                    if (tipocarga == 1)
                    {
                        pIni_Deslocamento = pIni.combinacoes_x_deslocamentos[comb].DeslocamentoGlobal;
                        pFin_Deslocamento = pFin.combinacoes_x_deslocamentos[comb].DeslocamentoGlobal;
                    }

                    for (int q = 0; q < secaoCopia_i.poligonos.Count; q++)
                    {
                        for (i = 0; i < secaoCopia_i.poligonos[q].coords.Count(); i++)
                        {
                            coordx_i = coordssecao_i[q][i].x;
                            coordy_i = coordssecao_i[q][i].y;
                            coordz_i = coordssecao_i[q][i].z;

                            coordx_f = coordssecao_f[q][i].x;
                            coordy_f = coordssecao_f[q][i].y;
                            coordz_f = coordssecao_f[q][i].z;

                            if (Deformacao_U == 4)
                            {
                                coordx_i += (pIni_Deslocamento[1] * MultiplicadorAltura);
                                coordy_i += (pIni_Deslocamento[3] * MultiplicadorAltura);
                                coordz_i += (pIni_Deslocamento[2] * MultiplicadorAltura);

                                coordx_f += (pFin_Deslocamento[1] * MultiplicadorAltura);
                                coordy_f += (pFin_Deslocamento[3] * MultiplicadorAltura);
                                coordz_f += (pFin_Deslocamento[2] * MultiplicadorAltura);
                            }
                            else
                            if (Deformacao_U == 1)
                            {
                                coordx_i += (pIni_Deslocamento[1] * MultiplicadorAltura);
                                coordx_f += (pFin_Deslocamento[1] * MultiplicadorAltura);
                            }
                            else
                            if (Deformacao_U == 2)
                            {
                                coordy_i += (pIni_Deslocamento[3] * MultiplicadorAltura);
                                coordy_f += (pFin_Deslocamento[3] * MultiplicadorAltura);
                            }
                            else
                            if (Deformacao_U == 3)
                            {
                                coordz_i += (pIni_Deslocamento[2] * MultiplicadorAltura);
                                coordz_f += (pFin_Deslocamento[2] * MultiplicadorAltura);
                            }

                            coordssecao_f_temp[q][i].x = coordx_f;
                            coordssecao_f_temp[q][i].y = coordy_f;
                            coordssecao_f_temp[q][i].z = coordz_f;

                            coordssecao_i_temp[q][i].x = coordx_i;
                            coordssecao_i_temp[q][i].y = coordy_i;
                            coordssecao_i_temp[q][i].z = coordz_i;
                        }
                    }

                    coordx_i = pIni_offset.x;
                    coordy_i = pIni_offset.y;
                    coordz_i = pIni_offset.z;

                    coordx_f = pFin_offset.x;
                    coordy_f = pFin_offset.y;
                    coordz_f = pFin_offset.z;

                    if (Deformacao_U == 4)
                    {
                        coordx_i += (pIni_Deslocamento[1] * MultiplicadorAltura);
                        coordy_i -= (pIni_Deslocamento[3] * MultiplicadorAltura);
                        coordz_i -= (pIni_Deslocamento[2] * MultiplicadorAltura);

                        coordx_f += (pFin_Deslocamento[1] * MultiplicadorAltura);
                        coordy_f -= (pFin_Deslocamento[3] * MultiplicadorAltura);
                        coordz_f -= (pFin_Deslocamento[2] * MultiplicadorAltura);
                    }
                    else
                    if (Deformacao_U == 1)
                    {
                        coordx_i += (pIni_Deslocamento[1] * MultiplicadorAltura);
                        coordx_f += (pFin_Deslocamento[1] * MultiplicadorAltura);
                    }
                    else
                    if (Deformacao_U == 2)
                    {
                        coordy_i -= (pIni_Deslocamento[3] * MultiplicadorAltura);
                        coordy_f -= (pFin_Deslocamento[3] * MultiplicadorAltura);
                    }
                    else
                    if (Deformacao_U == 3)
                    {
                        coordz_i -= (pIni_Deslocamento[2] * MultiplicadorAltura);
                        coordz_f -= (pFin_Deslocamento[2] * MultiplicadorAltura);
                    }

                    pIni.coordx_tela = coordx_i;
                    pIni.coordy_tela = coordy_i;
                    pIni.coordz_tela = coordz_i;
                    pFin.coordx_tela = coordx_f;
                    pFin.coordy_tela = coordy_f;
                    pFin.coordz_tela = coordz_f;

                    if (tensaoGradiente && tensoes_ini != null && tensoes_fin != null)
                    {
                        double r1 = 0, g1 = 0, b1 = 0, g2 = 0, b2 = 0, r2 = 0;
                        double r3 = 0, g3 = 0, b3 = 0;
                        double r4 = 0, g4 = 0, b4 = 0;

                        //  if (IDBarra == 2)
                        
                        if (tipocarga == 0)
                        {
                            tensoes_ini = casos_x_tensoes[caso].tensoes_i;
                            tensoes_fin = casos_x_tensoes[caso].tensoes_f;
                        }
                        else
                        if (tipocarga == 1)
                        {
                            tensoes_ini = combinacoes_x_tensoes[comb].tensoes_i;
                            tensoes_fin = combinacoes_x_tensoes[comb].tensoes_f;
                        }

                        int i_externo = secaoCopia_i.poligonos.IndexOf(secaoCopia_i.poligonos.First(o => o.externo));

                        //     for (int q = 0; q < secaoCopia_i.poligonos.FindAll(o=>o.externo).Count; q++)
                        
                        for (i = 0; i < secaoCopia_i.poligonos[i_externo].coords.Count() - 1; i++)
                        {
                            p1 = new vec3(coordssecao_i_temp[i_externo][i].x, -coordssecao_i_temp[i_externo][i].y, -coordssecao_i_temp[i_externo][i].z);
                            p2 = new vec3(coordssecao_f_temp[i_externo][i].x, -coordssecao_f_temp[i_externo][i].y, -coordssecao_f_temp[i_externo][i].z);
                            p3 = new vec3(coordssecao_f_temp[i_externo][i + 1].x, -coordssecao_f_temp[i_externo][i + 1].y, -coordssecao_f_temp[i_externo][i + 1].z);
                            v1 = p2 - p1;
                            v2 = p3 - p1;
                            n1 = v1.CrossProduct(v2) * -1;
                            n1.Normalize();

                            int prox_coord = (i + 1) % (secaoCopia_i.poligonos[i_externo].coords.Count() - 1);

                            if (!isoBandasTensoes)
                            {
                                RetRGBTensao("i", i, ref r1, ref g1, ref b1, tipocarga, caso, comb);
                                RetRGBTensao("f", i, ref r2, ref g2, ref b2, tipocarga, caso, comb);
                                RetRGBTensao("f", prox_coord, ref r3, ref g3, ref b3, tipocarga, caso, comb);
                                RetRGBTensao("i", prox_coord, ref r4, ref g4, ref b4, tipocarga, caso, comb);

                                SetaTriangulo(coordssecao_i_temp[i_externo][i].x, -coordssecao_i_temp[i_externo][i].y, -coordssecao_i_temp[i_externo][i].z,
                                                  coordssecao_f_temp[i_externo][i].x, -coordssecao_f_temp[i_externo][i].y, -coordssecao_f_temp[i_externo][i].z,
                                             coordssecao_f_temp[i_externo][prox_coord].x, -coordssecao_f_temp[i_externo][prox_coord].y, -coordssecao_f_temp[i_externo][prox_coord].z,
                                             ref coords_triangulos,
                                             ref triangulos_selecao,
                                             ref n1.x, ref n1.y, ref n1.z,
                                             r1, g1, b1,
                                             r2, g2, b2,
                                             r3, g3, b3,
                                             false);

                                SetaTriangulo(coordssecao_f_temp[i_externo][prox_coord].x, -coordssecao_f_temp[i_externo][prox_coord].y, -coordssecao_f_temp[i_externo][prox_coord].z,
                                               coordssecao_i_temp[i_externo][prox_coord].x, -coordssecao_i_temp[i_externo][prox_coord].y, -coordssecao_i_temp[i_externo][prox_coord].z,
                                               coordssecao_i_temp[i_externo][i].x, -coordssecao_i_temp[i_externo][i].y, -coordssecao_i_temp[i_externo][i].z,
                                          ref coords_triangulos, ref triangulos_selecao, ref n1.x, ref n1.y, ref n1.z,
                                          r3, g3, b3,
                                          r4, g4, b4,
                                          r1, g1, b1,
                                          false);
                            }
                            else
                            {
                                double t_pIni_coord = tensoes_ini[i];
                                double t_pFin_coord = tensoes_fin[i];

                                double t_pFin_proxcoord = tensoes_fin[prox_coord];
                                double t_pIni_proxcoord = tensoes_ini[prox_coord];

                                NoIsoBanda n_iso1 = new NoIsoBanda(new vec3(coordssecao_i_temp[i_externo][i].x, -coordssecao_i_temp[i_externo][i].y, -coordssecao_i_temp[i_externo][i].z), t_pIni_coord);
                                NoIsoBanda n_iso2 = new NoIsoBanda(new vec3(coordssecao_f_temp[i_externo][i].x, -coordssecao_f_temp[i_externo][i].y, -coordssecao_f_temp[i_externo][i].z), t_pFin_coord);
                                NoIsoBanda n_iso3 = new NoIsoBanda(new vec3(coordssecao_f_temp[i_externo][prox_coord].x, -coordssecao_f_temp[i_externo][prox_coord].y, -coordssecao_f_temp[i_externo][prox_coord].z), t_pFin_proxcoord);
                                NoIsoBanda n_iso4 = new NoIsoBanda(new vec3(coordssecao_i_temp[i_externo][prox_coord].x, -coordssecao_i_temp[i_externo][prox_coord].y, -coordssecao_i_temp[i_externo][prox_coord].z), t_pIni_proxcoord);

                                Quad3D quad = new Quad3D()
                                {
                                    P1 = n_iso1.posicao,
                                    P2 = n_iso2.posicao,
                                    P3 = n_iso3.posicao,
                                    P4 = n_iso4.posicao,

                                    S1 = t_pIni_coord,
                                    S2 = t_pFin_coord,
                                    S3 = t_pFin_proxcoord,
                                    S4 = t_pIni_proxcoord
                                };

                                List<QuadFatia3D> fatiasQuad;
                                bool sinaisAlternandos = QuadSubdivisao.TemSinaisAlternados(t_pIni_coord, t_pFin_coord, t_pFin_proxcoord, t_pIni_proxcoord);

                                //  bool cantos_alternados1 = (t_pIni_coord > 0 && t_pFin_proxcoord > 0);// || (t_pFin_coord < 0 && t_pIni_proxcoord < 0);
                                //   bool cantos_alternados2 = (t_pFin_coord < 0 && t_pIni_proxcoord < 0);// || (t_pFin_coord > 0 && t_pIni_proxcoord > 0);

                                if (sinaisAlternandos)
                                {
                                    fatiasQuad = QuadSubdivisao.SubdivideVertical(quad, 32);
                                }
                                else
                                {
                                    fatiasQuad = QuadSubdivisao.SubdivideVertical(quad, 2);
                                }

                                //                                NoIsoBanda n_iso4 = new NoIsoBanda(new vec3(CoordsSecao_f_temp[i+1].x, -CoordsSecao_f_temp[i+1].y, -CoordsSecao_f_temp[i+1].z), t_pFin_proxcoord);
                                //                              NoIsoBanda n_iso5 = new NoIsoBanda(new vec3(CoordsSecao_i_temp[i+1].x, -CoordsSecao_i_temp[i+1].y, -CoordsSecao_i_temp[i+1].z), t_pIni_proxcoord);
                                //                            NoIsoBanda n_iso6 = new NoIsoBanda(new vec3(CoordsSecao_i_temp[i].x, -CoordsSecao_i_temp[i].y, -CoordsSecao_i_temp[i].z), t_pIni_coord);
                                int tot_isobandas = 0;
                                double t_min_tracao, t_max_tracao, t_min_compr, t_max_compr;

                                if (Gerenciador.RGB_TensoesNormaisPositivas != null)
                                {
                                    for (int t = 0; t < Const.totCoresTensao; t++)
                                    {
                                        t_min_tracao = Gerenciador.RGB_TensoesNormaisPositivas[t, 1];
                                        t_max_tracao = Gerenciador.RGB_TensoesNormaisPositivas[t, 0];

                                        r1 = Gerenciador.RGB_TensoesNormaisPositivas[t, 2];
                                        g1 = Gerenciador.RGB_TensoesNormaisPositivas[t, 3];
                                        b1 = Gerenciador.RGB_TensoesNormaisPositivas[t, 4];

                                        //  List<NoIsoBanda> PoligonoIsoBanda = TrianguloIsoBanda.ProcessarIsoBanda(n_iso1, n_iso2, n_iso3, n_iso4, t_min, t_max);
                                        for (int p = 0; p < fatiasQuad.Count; p++)
                                        {
                                            List<NoIsoBanda> PoligonoIsoBanda = QuadIsoBanda.ProcessarIsoBanda(fatiasQuad[p].P1, fatiasQuad[p].P2, fatiasQuad[p].P3, fatiasQuad[p].P4, t_min_tracao, t_max_tracao);

                                            //     PolygonUtils3D.RemoveCollinear(PoligonoIsoBanda);  

                                            tot_isobandas += PoligonoIsoBanda.Count();

                                            int ult = 1;
                                            int tot_tris = PoligonoIsoBanda.Count - 2;

                                            for (int k = 0; k < tot_tris; k++)
                                            {
                                                SetaTriangulo(PoligonoIsoBanda[0].posicao.x, PoligonoIsoBanda[0].posicao.y, PoligonoIsoBanda[0].posicao.z,
                                                    PoligonoIsoBanda[ult].posicao.x, PoligonoIsoBanda[ult].posicao.y, PoligonoIsoBanda[ult].posicao.z,
                                                    PoligonoIsoBanda[ult + 1].posicao.x, PoligonoIsoBanda[ult + 1].posicao.y, PoligonoIsoBanda[ult + 1].posicao.z,
                                                    ref coords_triangulos,
                                                    ref triangulos_selecao,
                                                    ref n1.x, ref n1.y, ref n1.z,
                                                    r1, g1, b1,
                                                    r1, g1, b1,
                                                    r1, g1, b1,
                                                    false);

                                                ult = ult + 1;
                                            }
                                        }
                                    }
                                }

                                if (Gerenciador.RGB_TensoesNormaisNegativas != null)
                                {
                                    for (int t = 0; t < Const.totCoresTensao; t++)
                                    {
                                        t_min_compr = Gerenciador.RGB_TensoesNormaisNegativas[t, 1];
                                        t_max_compr = Gerenciador.RGB_TensoesNormaisNegativas[t, 0];

                                        r2 = Gerenciador.RGB_TensoesNormaisNegativas[t, 2];
                                        g2 = Gerenciador.RGB_TensoesNormaisNegativas[t, 3];
                                        b2 = Gerenciador.RGB_TensoesNormaisNegativas[t, 4];

                                        //  List<NoIsoBanda> PoligonoIsoBanda = TrianguloIsoBanda.ProcessarIsoBanda(n_iso1, n_iso2, n_iso3, n_iso4, t_min, t_max);
                                        for (int p = 0; p < fatiasQuad.Count; p++)
                                        {
                                            //PoligonoIsoBanda = TrianguloIsoBanda.ProcessarIsoBanda(n_iso1, n_iso2, n_iso3, n_iso4, t_min, t_max);
                                            List<NoIsoBanda> PoligonoIsoBanda = QuadIsoBanda.ProcessarIsoBanda(
                                                fatiasQuad[p].P1,
                                                fatiasQuad[p].P2,
                                                fatiasQuad[p].P3,
                                                fatiasQuad[p].P4,
                                                t_min_compr, t_max_compr);

                                            int ult = 1;
                                            int tot_tris = PoligonoIsoBanda.Count - 2;

                                            tot_isobandas += PoligonoIsoBanda.Count();

                                            for (int k = 0; k < tot_tris; k++)
                                            {
                                                SetaTriangulo(PoligonoIsoBanda[0].posicao.x, PoligonoIsoBanda[0].posicao.y, PoligonoIsoBanda[0].posicao.z,
                                                    PoligonoIsoBanda[ult].posicao.x, PoligonoIsoBanda[ult].posicao.y, PoligonoIsoBanda[ult].posicao.z,
                                                    PoligonoIsoBanda[ult + 1].posicao.x, PoligonoIsoBanda[ult + 1].posicao.y, PoligonoIsoBanda[ult + 1].posicao.z,
                                                    ref coords_triangulos,
                                                    ref triangulos_selecao,
                                                    ref n1.x, ref n1.y, ref n1.z,
                                                    r2, g2, b2,
                                                    r2, g2, b2,
                                                    r2, g2, b2,
                                                    false);

                                                ult = ult + 1;
                                            }
                                        }
                                    }
                                }
                            }
                        }                      
                    }
                }
            }
            catch (Exception ms)
            {
                System.Windows.Forms.MessageBox.Show("erro ao preencher triangulo na barra de portico:" + this.IDBarra + " elemento: " + barraOriginal.IDBarra + " -  " + ms.Message);
            }
        }
        public bool DirtyTriangulos = true;
        public bool DirtySelecao = true;
        [NonSerialized]
        public float[] BatchTriangulos;
        [NonSerialized]
        public Triangulo[] TriangulosSelecao;
        public void AtualizaTriangulos(ref List<float> coords_triangulos,
                                        ref List<Triangulo> triangulos_selecao,
                                        bool MostrarIndeformada,
                                        ref double MultiplicadorAltura,
                                        ref bool arestas,
                                        ref bool LinhaContorno,
                                        ref int Deformacao_U,
                                        int tipocarga,
                                        int caso,
                                        int comb,
                                        bool deformacao_colorida,
                                        bool tensaoGradiente,
                                        bool isoBandasTensoes)
        {
            List<float> coords = new List<float>();
            List<Triangulo> selecao = new List<Triangulo>();


            PreencheTriangulos(ref coords, ref selecao, MostrarIndeformada,
                               ref MultiplicadorAltura, ref arestas, ref LinhaContorno, ref Deformacao_U,
                               tipocarga, caso, comb, deformacao_colorida, tensaoGradiente, isoBandasTensoes);


            BatchTriangulos = coords.ToArray();
            TriangulosSelecao = selecao.ToArray();

            DirtyTriangulos = false;
            DirtySelecao = false;
        }

        public void PreencheTriangulos( ref List<float> coords_triangulos, 
                                        ref List<Triangulo> triangulos_selecao,
                                        bool MostrarIndeformada, 
                                        ref double MultiplicadorAltura, 
                                        ref bool arestas,
                                        ref bool LinhaContorno, 
                                        ref int Deformacao_U, 
                                        int tipocarga, 
                                        int caso, 
                                        int comb,
                                        bool deformacao_colorida,
                                        bool tensaoGradiente,
                                        bool isoBandasTensoes)
        {
            try
            {
                if (barra_de_articulacao || barraRigida || coordssecao_f_temp  ==null) return;

              //  if (   (MostrarIndeformada && barraOriginal.Selecionado) 
              //      || ((!MostrarIndeformada) && Visivel))
                {

                    r = (double)Rgb[0] / 255;
                    g = (double)Rgb[1] / 255;
                    b = (double)Rgb[2] / 255;

                    if (tipocarga == 0)
                    {
                        pIni_Deslocamento = pIni.casos_x_deslocamentos[caso].DeslocamentoGlobal;
                        pFin_Deslocamento = pFin.casos_x_deslocamentos[caso].DeslocamentoGlobal;
                    }
                    else
                    if (tipocarga == 1)
                    {
                        pIni_Deslocamento = pIni.combinacoes_x_deslocamentos[comb].DeslocamentoGlobal;
                        pFin_Deslocamento = pFin.combinacoes_x_deslocamentos[comb].DeslocamentoGlobal;
                    }

                    for (int q = 0; q < secaoCopia_i.poligonos.Count; q++)
                    {
                        for (i = 0; i < secaoCopia_i.poligonos[q].coords.Count(); i++)
                        {
                            coordx_i = coordssecao_i[q][i].x;
                            coordy_i = coordssecao_i[q][i].y;
                            coordz_i = coordssecao_i[q][i].z;

                            coordx_f = coordssecao_f[q][i].x;
                            coordy_f = coordssecao_f[q][i].y;
                            coordz_f = coordssecao_f[q][i].z;

                            if (Deformacao_U == 4)
                            {
                                coordx_i += (pIni_Deslocamento[1] * MultiplicadorAltura);
                                coordy_i += (pIni_Deslocamento[3] * MultiplicadorAltura);
                                coordz_i += (pIni_Deslocamento[2] * MultiplicadorAltura);

                                coordx_f += (pFin_Deslocamento[1] * MultiplicadorAltura);
                                coordy_f += (pFin_Deslocamento[3] * MultiplicadorAltura);
                                coordz_f += (pFin_Deslocamento[2] * MultiplicadorAltura);
                            }
                            else
                            if (Deformacao_U == 1)
                            {
                                coordx_i += (pIni_Deslocamento[1] * MultiplicadorAltura);
                                coordx_f += (pFin_Deslocamento[1] * MultiplicadorAltura);
                            }
                            else
                            if (Deformacao_U == 2)
                            {
                                coordy_i += (pIni_Deslocamento[3] * MultiplicadorAltura);
                                coordy_f += (pFin_Deslocamento[3] * MultiplicadorAltura);
                            }
                            else
                            if (Deformacao_U == 3)
                            {
                                coordz_i += (pIni_Deslocamento[2] * MultiplicadorAltura);
                                coordz_f += (pFin_Deslocamento[2] * MultiplicadorAltura);
                            }

                            coordssecao_f_temp[q][i].x = coordx_f;
                            coordssecao_f_temp[q][i].y = coordy_f;
                            coordssecao_f_temp[q][i].z = coordz_f;
                       
                            coordssecao_i_temp[q][i].x = coordx_i;
                            coordssecao_i_temp[q][i].y = coordy_i;
                            coordssecao_i_temp[q][i].z = coordz_i;
                        }
                    }

                    coordx_i = pIni_offset.x;
                    coordy_i = pIni_offset.y;
                    coordz_i = pIni_offset.z;
                                  
                    coordx_f = pFin_offset.x;
                    coordy_f = pFin_offset.y;
                    coordz_f = pFin_offset.z;

                    if (Deformacao_U == 4)
                    {
                        coordx_i += (pIni_Deslocamento[1] * MultiplicadorAltura);
                        coordy_i -= (pIni_Deslocamento[3] * MultiplicadorAltura);
                        coordz_i -= (pIni_Deslocamento[2] * MultiplicadorAltura);
                                         
                        coordx_f += (pFin_Deslocamento[1] * MultiplicadorAltura);
                        coordy_f -= (pFin_Deslocamento[3] * MultiplicadorAltura);
                        coordz_f -= (pFin_Deslocamento[2] * MultiplicadorAltura);
                    }
                    else
                    if (Deformacao_U == 1)
                    {
                        coordx_i += (pIni_Deslocamento[1] * MultiplicadorAltura);
                        coordx_f += (pFin_Deslocamento[1] * MultiplicadorAltura);
                    }
                    else
                    if (Deformacao_U == 2)
                    {
                        coordy_i -= (pIni_Deslocamento[3] * MultiplicadorAltura);
                        coordy_f -= (pFin_Deslocamento[3] * MultiplicadorAltura);
                    }
                    else
                    if (Deformacao_U == 3)
                    {
                        coordz_i -= (pIni_Deslocamento[2] * MultiplicadorAltura);
                        coordz_f -= (pFin_Deslocamento[2] * MultiplicadorAltura);
                    }

                    pIni.coordx_tela = coordx_i;
                    pIni.coordy_tela = coordy_i;
                    pIni.coordz_tela = coordz_i;
                    pFin.coordx_tela = coordx_f;
                    pFin.coordy_tela = coordy_f;
                    pFin.coordz_tela = coordz_f;

                    if (deformacao_colorida)
                    {
                        double r1 = 0, r2 = 0, g1 = 0, g2 = 0, b1 = 0, b2 = 0;

                        RetCorDeslocamento(ref r1, ref g1, ref b1, ref r2, ref g2, ref b2, tipocarga, caso, comb);

                        for (int q = 0; q < secaoCopia_i.poligonos.Count; q++)
                        {
                            for (i = 0; i < secaoCopia_i.poligonos[q].coords.Count()-1; i++)
                            {
                                p1 = new vec3(coordssecao_i_temp[q][i].x, -coordssecao_i_temp[q][i].y, -coordssecao_i_temp[q][i].z);
                                p2 = new vec3(coordssecao_f_temp[q][i].x, -coordssecao_f_temp[q][i].y, -coordssecao_f_temp[q][i].z);
                                p3 = new vec3(coordssecao_f_temp[q][i + 1].x, -coordssecao_f_temp[q][i + 1].y, -coordssecao_f_temp[q][i + 1].z);
                                v1 = p2 - p1;
                                v2 = p3 - p1;
                                n1 = v1.CrossProduct(v2) * -1;
                                n1.Normalize();

                                SetaTriangulo(coordssecao_i_temp[q][i].x, -coordssecao_i_temp[q][i].y, -coordssecao_i_temp[q][i].z,
                                              coordssecao_f_temp[q][i].x, -coordssecao_f_temp[q][i].y, -coordssecao_f_temp[q][i].z,
                                              coordssecao_f_temp[q][i + 1].x, -coordssecao_f_temp[q][i + 1].y, -coordssecao_f_temp[q][i + 1].z,
                                          ref coords_triangulos,
                                          ref triangulos_selecao,
                                          ref n1.x, ref n1.y, ref n1.z,
                                          r1, g1, b1,
                                          r2, g2, b2,
                                          r2, g2, b2);

                                SetaTriangulo(coordssecao_i_temp[q][i].x, -coordssecao_i_temp[q][i].y, -coordssecao_i_temp[q][i].z,
                                               coordssecao_f_temp[q][i + 1].x, -coordssecao_f_temp[q][i + 1].y, -coordssecao_f_temp[q][i + 1].z,
                                               coordssecao_i_temp[q][i + 1].x, -coordssecao_i_temp[q][i + 1].y, -coordssecao_i_temp[q][i + 1].z,
                                          ref coords_triangulos, ref triangulos_selecao, ref n1.x, ref n1.y, ref n1.z,
                                          r1, g1, b1,
                                          r2, g2, b2,
                                          r1, g1, b1);
                            }
                        }
                    }
                    else
                    {
                        for (int q = 0; q < secaoCopia_i.poligonos.Count; q++)
                        {
                            for (i = 0; i < secaoCopia_i.poligonos[q].coords.Count()-1; i++)
                            {
                                p1 = new vec3(coordssecao_i_temp[q][i].x, -coordssecao_i_temp[q][i].y, -coordssecao_i_temp[q][i].z);
                                p2 = new vec3(coordssecao_f_temp[q][i].x, -coordssecao_f_temp[q][i].y, -coordssecao_f_temp[q][i].z);
                                p3 = new vec3(coordssecao_f_temp[q][i + 1].x, -coordssecao_f_temp[q][i + 1].y, -coordssecao_f_temp[q][i + 1].z);
                                v1 = p2 - p1;
                                v2 = p3 - p1;
                                n1 = v1.CrossProduct(v2) * -1;
                                n1.Normalize();

                                SetaTriangulo(ref coordssecao_f_temp[q][i].x, -coordssecao_f_temp[q][i].y, -coordssecao_f_temp[q][i].z,
                                              ref coordssecao_i_temp[q][i].x, -coordssecao_i_temp[q][i].y, -coordssecao_i_temp[q][i].z,
                                              ref coordssecao_i_temp[q][i + 1].x, -coordssecao_i_temp[q][i + 1].y, -coordssecao_i_temp[q][i + 1].z,
                                          ref coords_triangulos,
                                          ref triangulos_selecao,
                                          ref n1.x, ref n1.y, ref n1.z);

                                SetaTriangulo(ref coordssecao_f_temp[q][i].x, -coordssecao_f_temp[q][i].y, -coordssecao_f_temp[q][i].z,
                                              ref coordssecao_i_temp[q][i + 1].x, -coordssecao_i_temp[q][i + 1].y, -coordssecao_i_temp[q][i + 1].z,
                                              ref coordssecao_f_temp[q][i + 1].x, -coordssecao_f_temp[q][i + 1].y, -coordssecao_f_temp[q][i + 1].z,
                                          ref coords_triangulos, ref triangulos_selecao, ref n1.x, ref n1.y, ref n1.z);
                            }
                        }
                    }

                  /*  for (int i = 0; i < trisFace1.Count; i++)
                    {

                        p1 = new vec3(coordssecao_i_temp[trisFace1[i].indices[0]].x, -CoordsSecao_i_temp[trisFace1[i].indices[0]].y, -CoordsSecao_i_temp[trisFace1[i].indices[0]].z);
                        p2 = new vec3(CoordsSecao_i_temp[trisFace1[i].indices[1]].x, -CoordsSecao_i_temp[trisFace1[i].indices[1]].y, -CoordsSecao_i_temp[trisFace1[i].indices[1]].z);
                        p3 = new vec3(CoordsSecao_i_temp[trisFace1[i].indices[2]].x, -CoordsSecao_i_temp[trisFace1[i].indices[2]].y, -CoordsSecao_i_temp[trisFace1[i].indices[2]].z);
                        v1 = p2 - p1;
                        v2 = p3 - p1;
                        n1 = v1.CrossProduct(v2)*-1;
                        n1.Normalize();

                        if (!Geom.Iguais(pIni.x, pFin.x))
                        {
                            if (pIni.x> pFin.x)
                                n1 *= -1;
                        }

                        SetaTriangulo(ref CoordsSecao_i_temp[trisFace1[i].indices[0]].x, -CoordsSecao_i_temp[trisFace1[i].indices[0]].y, -CoordsSecao_i_temp[trisFace1[i].indices[0]].z,
                                      ref CoordsSecao_i_temp[trisFace1[i].indices[1]].x, -CoordsSecao_i_temp[trisFace1[i].indices[1]].y, -CoordsSecao_i_temp[trisFace1[i].indices[1]].z,
                                      ref CoordsSecao_i_temp[trisFace1[i].indices[2]].x, -CoordsSecao_i_temp[trisFace1[i].indices[2]].y, -CoordsSecao_i_temp[trisFace1[i].indices[2]].z,
                                      ref coords_triangulos, ref triangulos_selecao, ref n1.x, ref n1.y, ref n1.z);
                    }

                    for (int i = 0; i < trisFace2.Count; i++)
                    {
                        p1 = new vec3(CoordsSecao_f_temp[trisFace2[i].indices[0]].x, -CoordsSecao_f_temp[trisFace2[i].indices[0]].y, -CoordsSecao_f_temp[trisFace2[i].indices[0]].z);
                        p2 = new vec3(CoordsSecao_f_temp[trisFace2[i].indices[1]].x, -CoordsSecao_f_temp[trisFace2[i].indices[1]].y, -CoordsSecao_f_temp[trisFace2[i].indices[1]].z);
                        p3 = new vec3(CoordsSecao_f_temp[trisFace2[i].indices[2]].x, -CoordsSecao_f_temp[trisFace2[i].indices[2]].y, -CoordsSecao_f_temp[trisFace2[i].indices[2]].z);
                        v1 = p2 - p1;
                        v2 = p3 - p1;
                        n1 = v1.CrossProduct(v2);
                        n1.Normalize();

                        if (!Geom.Iguais(pIni.x, pFin.x))
                        {
                            if (pIni.x> pFin.x)
                                n1 *= -1;
                        }

                        SetaTriangulo(ref CoordsSecao_f_temp[trisFace2[i].indices[0]].x, -CoordsSecao_f_temp[trisFace2[i].indices[0]].y, -CoordsSecao_f_temp[trisFace2[i].indices[0]].z,
                                      ref CoordsSecao_f_temp[trisFace2[i].indices[1]].x, -CoordsSecao_f_temp[trisFace2[i].indices[1]].y, -CoordsSecao_f_temp[trisFace2[i].indices[1]].z,
                                      ref CoordsSecao_f_temp[trisFace2[i].indices[2]].x, -CoordsSecao_f_temp[trisFace2[i].indices[2]].y, -CoordsSecao_f_temp[trisFace2[i].indices[2]].z,
                                      ref coords_triangulos, ref triangulos_selecao, ref n1.x, ref n1.y, ref n1.z);
                    }*/
                }
            }
            catch (Exception ms)
            {
                System.Windows.Forms.MessageBox.Show("erro ao preencher triangulo na barra de portico:" + this.IDBarra + " elemento: "+barraOriginal.IDBarra+" -  " + ms.Message);
            }
        }
        [NonSerialized]
        Triangulo tr;

        public void Preenche_Arestas_Solido_Tensoes(
            ref List<float> coords_arestas,
            ref bool LinhaContorno,
            ref bool arestasConfObjeto)
        {
            if (barra_de_articulacao || barraRigida || coordssecao_f_temp == null) return;
            {
                if (arestasConfObjeto)
                {
                    r = (double)barraOriginal.Rgb[0] / 255;
                    g = (double)barraOriginal.Rgb[1] / 255;
                    b = (double)barraOriginal.Rgb[2] / 255;
                }
                else
                {
                    r = 0;
                    g = 0;
                    b = 0;
                }

                if (LinhaContorno)
                {
                    for (int q = 0; q < secaoCopia_i.poligonos.Count; q++)
                    {
                        for (i = 0; i < secaoCopia_i.poligonos[q].coords.Count() - 1; i++)
                        {
                            if (!coordssecao_i[q][i].pontoEmRaio)
                            {
                                setLista(ref coords_arestas, coordssecao_f_temp[q][i].x, -coordssecao_f_temp[q][i].y, -coordssecao_f_temp[q][i].z);
                                setLista(ref coords_arestas, r, g, b);
                                setLista(ref coords_arestas, coordssecao_i_temp[q][i].x, -coordssecao_i_temp[q][i].y, -coordssecao_i_temp[q][i].z);
                                setLista(ref coords_arestas, r, g, b);

                                setLista(ref coords_arestas, coordssecao_i_temp[q][i].x, -coordssecao_i_temp[q][i].y, -coordssecao_i_temp[q][i].z);
                                setLista(ref coords_arestas, r, g, b);
                                setLista(ref coords_arestas, coordssecao_i_temp[q][i + 1].x, -coordssecao_i_temp[q][i + 1].y, -coordssecao_i_temp[q][i + 1].z);
                                setLista(ref coords_arestas, r, g, b);

                                setLista(ref coords_arestas, coordssecao_i_temp[q][i + 1].x, -coordssecao_i_temp[q][i + 1].y, -coordssecao_i_temp[q][i + 1].z);
                                setLista(ref coords_arestas, r, g, b);
                                setLista(ref coords_arestas, coordssecao_f_temp[q][i + 1].x, -coordssecao_f_temp[q][i + 1].y, -coordssecao_f_temp[q][i + 1].z);
                                setLista(ref coords_arestas, r, g, b);

                                setLista(ref coords_arestas, coordssecao_f_temp[q][i + 1].x, -coordssecao_f_temp[q][i + 1].y, -coordssecao_f_temp[q][i + 1].z);
                                setLista(ref coords_arestas, r, g, b);
                                setLista(ref coords_arestas, coordssecao_f_temp[q][i].x, -coordssecao_f_temp[q][i].y, -coordssecao_f_temp[q][i].z);
                                setLista(ref coords_arestas, r, g, b);
                            }
                        }
                    }
                }
                else
                {
                    for (int q = 0; q < secaoCopia_i.poligonos.Count; q++)
                    {
                        for (i = 0; i < secaoCopia_i.poligonos[q].coords.Count() - 1; i++)
                        {
                            if (!coordssecao_f_temp[q][i].pontoEmRaio)
                            {
                                setLista(ref coords_arestas, coordssecao_f_temp[q][i].x, -coordssecao_f_temp[q][i].y, -coordssecao_f_temp[q][i].z);
                                setLista(ref coords_arestas, r, g, b);
                                setLista(ref coords_arestas, coordssecao_i_temp[q][i].x, -coordssecao_i_temp[q][i].y, -coordssecao_i_temp[q][i].z);
                                setLista(ref coords_arestas, r, g, b);

                                setLista(ref coords_arestas, coordssecao_f_temp[q][i + 1].x, -coordssecao_f_temp[q][i + 1].y, -coordssecao_f_temp[q][i + 1].z);
                                setLista(ref coords_arestas, r, g, b);
                                setLista(ref coords_arestas, coordssecao_i_temp[q][i + 1].x, -coordssecao_i_temp[q][i + 1].y, -coordssecao_i_temp[q][i + 1].z);
                                setLista(ref coords_arestas, r, g, b);
                            }

                            if ((Geom.Iguais(pIni.x, barraOriginal.pIni.x) && Geom.Iguais(pIni.y, barraOriginal.pIni.y) && Geom.Iguais(pIni.z, barraOriginal.pIni.z)))
                            {
                                setLista(ref coords_arestas, coordssecao_i_temp[q][i].x, -coordssecao_i_temp[q][i].y, -coordssecao_i_temp[q][i].z);
                                setLista(ref coords_arestas, r, g, b);
                                setLista(ref coords_arestas, coordssecao_i_temp[q][i + 1].x, -coordssecao_i_temp[q][i + 1].y, -coordssecao_i_temp[q][i + 1].z);
                                setLista(ref coords_arestas, r, g, b);
                            }


                            if ((Geom.Iguais(pFin.x, barraOriginal.pFin.x) && Geom.Iguais(pFin.y, barraOriginal.pFin.y) && Geom.Iguais(pFin.z, barraOriginal.pFin.z)))                                                                                                                                                                 //   (Geom.Iguais(pFin.x, barraOriginal.pFin.x) && Geom.Iguais(pFin.y, barraOriginal.pFin.y) && Geom.Iguais(pFin.z, barraOriginal.pFin.z)))
                            {
                                setLista(ref coords_arestas, coordssecao_f_temp[q][i].x, -coordssecao_f_temp[q][i].y, -coordssecao_f_temp[q][i].z);
                                setLista(ref coords_arestas, r, g, b);
                                setLista(ref coords_arestas, coordssecao_f_temp[q][i + 1].x, -coordssecao_f_temp[q][i + 1].y, -coordssecao_f_temp[q][i + 1].z);
                                setLista(ref coords_arestas, r, g, b);
                            }
                        }
                    }
                }
            }
        }
        public void Preenche_Arestas_Solido(
            ref List<float> coords_arestas, bool MostrarIndeformada,
            ref bool LinhaContorno,
            ref bool arestasConfObjeto)
        {
            if (barra_de_articulacao || barraRigida || coordssecao_i_temp == null) return;

            {
                if (arestasConfObjeto)
                {
                    r = (double)barraOriginal.Rgb[0] / 255;
                    g = (double)barraOriginal.Rgb[1] / 255;
                    b = (double)barraOriginal.Rgb[2] / 255;
                }
                else
                {
                    r = 0;
                    g = 0;
                    b = 0;
                }

                if (LinhaContorno)
                {
                    for (int q = 0; q < secaoCopia_i.poligonos.Count; q++)
                    {
                        for (i = 0; i < secaoCopia_i.poligonos[q].coords.Count() - 1; i++)
                        {
                            if (!coordssecao_i[q][i].pontoEmRaio)
                            {
                                setLista(ref coords_arestas, coordssecao_f_temp[q][i].x, -coordssecao_f_temp[q][i].y, -coordssecao_f_temp[q][i].z);
                                setLista(ref coords_arestas, r, g, b);
                                setLista(ref coords_arestas, coordssecao_i_temp[q][i].x, -coordssecao_i_temp[q][i].y, -coordssecao_i_temp[q][i].z);
                                setLista(ref coords_arestas, r, g, b);

                                setLista(ref coords_arestas, coordssecao_i_temp[q][i].x, -coordssecao_i_temp[q][i].y, -coordssecao_i_temp[q][i].z);
                                setLista(ref coords_arestas, r, g, b);
                                setLista(ref coords_arestas, coordssecao_i_temp[q][i + 1].x, -coordssecao_i_temp[q][i + 1].y, -coordssecao_i_temp[q][i + 1].z);
                                setLista(ref coords_arestas, r, g, b);

                                setLista(ref coords_arestas, coordssecao_i_temp[q][i + 1].x, -coordssecao_i_temp[q][i + 1].y, -coordssecao_i_temp[q][i + 1].z);
                                setLista(ref coords_arestas, r, g, b);
                                setLista(ref coords_arestas, coordssecao_f_temp[q][i + 1].x, -coordssecao_f_temp[q][i + 1].y, -coordssecao_f_temp[q][i + 1].z);
                                setLista(ref coords_arestas, r, g, b);

                                setLista(ref coords_arestas, coordssecao_f_temp[q][i + 1].x, -coordssecao_f_temp[q][i + 1].y, -coordssecao_f_temp[q][i + 1].z);
                                setLista(ref coords_arestas, r, g, b);
                                setLista(ref coords_arestas, coordssecao_f_temp[q][i].x, -coordssecao_f_temp[q][i].y, -coordssecao_f_temp[q][i].z);
                                setLista(ref coords_arestas, r, g, b);
                            }
                        }
                    }
                }
                else
                {
                    for (int q = 0; q < secaoCopia_i.poligonos.Count; q++)
                    {
                        for (i = 0; i < secaoCopia_i.poligonos[q].coords.Count() - 1; i++)
                        {
                            if (!coordssecao_f_temp[q][i].pontoEmRaio)
                            {
                                setLista(ref coords_arestas, coordssecao_f_temp[q][i].x, -coordssecao_f_temp[q][i].y, -coordssecao_f_temp[q][i].z);
                                setLista(ref coords_arestas, r, g, b);
                                setLista(ref coords_arestas, coordssecao_i_temp[q][i].x, -coordssecao_i_temp[q][i].y, -coordssecao_i_temp[q][i].z);
                                setLista(ref coords_arestas, r, g, b);

                                setLista(ref coords_arestas, coordssecao_f_temp[q][i + 1].x, -coordssecao_f_temp[q][i + 1].y, -coordssecao_f_temp[q][i + 1].z);
                                setLista(ref coords_arestas, r, g, b);
                                setLista(ref coords_arestas, coordssecao_i_temp[q][i + 1].x, -coordssecao_i_temp[q][i + 1].y, -coordssecao_i_temp[q][i + 1].z);
                                setLista(ref coords_arestas, r, g, b);
                            }

                            if ((Geom.Iguais(pIni.x, barraOriginal.pIni.x) && Geom.Iguais(pIni.y, barraOriginal.pIni.y) && Geom.Iguais(pIni.z, barraOriginal.pIni.z)))
                            {
                                setLista(ref coords_arestas, coordssecao_i_temp[q][i].x, -coordssecao_i_temp[q][i].y, -coordssecao_i_temp[q][i].z);
                                setLista(ref coords_arestas, r, g, b);
                                setLista(ref coords_arestas, coordssecao_i_temp[q][i + 1].x, -coordssecao_i_temp[q][i + 1].y, -coordssecao_i_temp[q][i + 1].z);
                                setLista(ref coords_arestas, r, g, b);
                            }


                            if ((Geom.Iguais(pFin.x, barraOriginal.pFin.x) && Geom.Iguais(pFin.y, barraOriginal.pFin.y) && Geom.Iguais(pFin.z, barraOriginal.pFin.z)))                                                                                                                                                                 //   (Geom.Iguais(pFin.x, barraOriginal.pFin.x) && Geom.Iguais(pFin.y, barraOriginal.pFin.y) && Geom.Iguais(pFin.z, barraOriginal.pFin.z)))
                            {
                                setLista(ref coords_arestas, coordssecao_f_temp[q][i].x, -coordssecao_f_temp[q][i].y, -coordssecao_f_temp[q][i].z);
                                setLista(ref coords_arestas, r, g, b);
                                setLista(ref coords_arestas, coordssecao_f_temp[q][i + 1].x, -coordssecao_f_temp[q][i + 1].y, -coordssecao_f_temp[q][i + 1].z);
                                setLista(ref coords_arestas, r, g, b);
                            }
                        }
                    }
                }
            }
        }
        bool ForaDaTela, naTela2, naTela1;
        double z_clip1, z_clip2;
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

            if (!naTela1 || !naTela2)
                return false;

            return true;
        }

        public void Preenche_Barra(
                         ref List<float> coords_arestas, bool MostrarIndeformada,
                         ref double MultiplicadorAltura,
                         ref bool Colorido,
                         ref bool arestas,
                         ref int Deformacao_U,
                         int tipocarga, int caso, int comb)
        {
            try
            {
                if (barra_de_articulacao) return;

                if ((MostrarIndeformada && barraOriginal.Visivel) || ((!MostrarIndeformada) && Visivel))
                {
                    if (Selecionado)
                    {
                        r = 1;
                        g = 0;
                        b = 0;
                    }
                    else
                    {
                        r = (double)Rgb[0] / 255;
                        g = (double)Rgb[1] / 255;
                        b = (double)Rgb[2] / 255;
                    }

                    if (tipocarga == 0)
                    {
                        pIni_Deslocamento = pIni.casos_x_deslocamentos[caso].DeslocamentoGlobal;
                        pFin_Deslocamento = pFin.casos_x_deslocamentos[caso].DeslocamentoGlobal;
                    }
                    else
                    if (tipocarga == 1)
                    {
                        pIni_Deslocamento = pIni.combinacoes_x_deslocamentos[comb].DeslocamentoGlobal;
                        pFin_Deslocamento = pFin.combinacoes_x_deslocamentos[comb].DeslocamentoGlobal;
                    }

                    coordx_i = pIni_offset.x;
                    coordy_i = pIni_offset.y;
                    coordz_i = pIni_offset.z;

                    coordx_f = pFin_offset.x;
                    coordy_f = pFin_offset.y;
                    coordz_f = pFin_offset.z;

                    if (Deformacao_U == 4)
                    {
                        coordx_i += (pIni_Deslocamento[1] * MultiplicadorAltura);
                        coordy_i -= (pIni_Deslocamento[3] * MultiplicadorAltura);
                        coordz_i -= (pIni_Deslocamento[2] * MultiplicadorAltura);
                                         
                        coordx_f += (pFin_Deslocamento[1] * MultiplicadorAltura);
                        coordy_f -= (pFin_Deslocamento[3] * MultiplicadorAltura);
                        coordz_f -= (pFin_Deslocamento[2] * MultiplicadorAltura);
                    }
                    else
                    if (Deformacao_U == 1)
                    {
                        coordx_i += (pIni_Deslocamento[1] * MultiplicadorAltura);
                        coordx_f += (pFin_Deslocamento[1] * MultiplicadorAltura);
                    }
                    else
                    if (Deformacao_U == 2)
                    {
                        coordy_i -= (pIni_Deslocamento[3] * MultiplicadorAltura);
                        coordy_f -= (pFin_Deslocamento[3] * MultiplicadorAltura);
                    }
                    else
                    if (Deformacao_U == 3)
                    {
                        coordz_i -= (pIni_Deslocamento[2] * MultiplicadorAltura);
                        coordz_f -= (pFin_Deslocamento[2] * MultiplicadorAltura);
                    }

                    pIni.coordx_tela = coordx_i;
                    pIni.coordy_tela = coordy_i;
                    pIni.coordz_tela = coordz_i;

                    if (Colorido)
                    {
                        double r1 = 0, r2 = 0, g1 = 0, g2 = 0, b1 = 0, b2 = 0;
                        RetCorDeslocamento(ref r1, ref g1, ref b1, ref r2, ref g2, ref b2, tipocarga, caso, comb);
                    
                        setLista(ref coords_arestas, coordx_i, coordy_i, coordz_i);
                        setLista(ref coords_arestas, r1, g1, b1);

                        pFin.coordx_tela = coordx_f;
                        pFin.coordy_tela = coordy_f;
                        pFin.coordz_tela = coordz_f;

                        setLista(ref coords_arestas, coordx_f, coordy_f, coordz_f);
                        setLista(ref coords_arestas, r2, g2, b2);
                    }
                    else
                    {

                        setLista(ref coords_arestas, coordx_i, coordy_i, coordz_i);
                        setLista(ref coords_arestas, r, g, b);

                        pFin.coordx_tela = coordx_f;
                        pFin.coordy_tela = coordy_f;
                        pFin.coordz_tela = coordz_f;

                        setLista(ref coords_arestas, coordx_f, coordy_f, coordz_f);
                        setLista(ref coords_arestas, r, g, b);
                    }
                }
            }
            catch (Exception ms)
            {
                System.Windows.Forms.MessageBox.Show("erro função Preenche_Barra: barra " + this.IDBarra + "  -  " + ms.Message);
            }
        }

      
      

        void CriarDoisTriangulos(vec3[] coordEsforco, List<float> coords_triangulos,
            double r1 , double g1, double b1, double r2, double g2, double b2)
        {
            p_1 = new vec3(coordEsforco[0].x, coordEsforco[0].y, coordEsforco[0].z);
            p_2 = new vec3(coordEsforco[3].x, coordEsforco[3].y, coordEsforco[3].z);

            p_3 = new vec3(coordEsforco[1].x, coordEsforco[1].y, coordEsforco[1].z);
            p_4 = new vec3(coordEsforco[2].x, coordEsforco[2].y, coordEsforco[2].z);
            temIntersec = Geom.TemInterseccao3D(ref p_1, ref p_2, ref p_3, ref p_4, ref ponto, ref tt, ref uu, 0.0000001);

            if (temIntersec)
                SetaTriangulo(ref coordEsforco[0].x, coordEsforco[0].y, coordEsforco[0].z,
                              ref coordEsforco[1].x, coordEsforco[1].y, coordEsforco[1].z,
                              ref ponto.x, ponto.y, ponto.z,
                              r1, g1, b1,
                              r1, g1, b1,
                              r2, g2, b2,
                              ref coords_triangulos);
            else
                SetaTriangulo(ref coordEsforco[0].x, coordEsforco[0].y, coordEsforco[0].z,
                              ref coordEsforco[1].x, coordEsforco[1].y, coordEsforco[1].z,
                              ref coordEsforco[2].x, coordEsforco[2].y, coordEsforco[2].z,
                              r1, g1, b1,
                              r1, g1, b1,
                              r2, g2, b2,
                              ref coords_triangulos);
            if (temIntersec)
                SetaTriangulo(ref ponto.x, ponto.y, ponto.z,
                          ref coordEsforco[2].x, coordEsforco[2].y, coordEsforco[2].z,
                          ref coordEsforco[3].x, coordEsforco[3].y, coordEsforco[3].z,
                          r1, g1, b1,
                          r2, g2, b2,
                          r2, g2, b2,
                          ref coords_triangulos);
            else
                SetaTriangulo(ref coordEsforco[0].x, coordEsforco[0].y, coordEsforco[0].z,
                         ref coordEsforco[2].x, coordEsforco[2].y, coordEsforco[2].z,
                         ref coordEsforco[3].x, coordEsforco[3].y, coordEsforco[3].z,
                         r1, g1, b1,
                         r2, g2, b2,
                         r2, g2, b2,
                         ref coords_triangulos);

        }

        double tt = 0, uu = 0;
        vec3 p_1, p_2, p_3, p_4;
        vec3 p_r = new vec3(0, 0, 0);
        vec3 ponto = new vec3(0, 0, 0);
        bool temIntersec;
     
     
        public int id_barraAnterior;

        public void AcertaPontosNotacaoCientifica()
        {
            if (Geom.Iguais(pFin.x, 0)) pFin.x = 0;
            if (Geom.Iguais(pFin.y, 0)) pFin.y = 0;
            if (Geom.Iguais(pFin.z, 0)) pFin.z = 0;

            if (Geom.Iguais(pIni.x, 0)) pIni.x = 0;
            if (Geom.Iguais(pIni.y, 0)) pIni.y = 0;
            if (Geom.Iguais(pIni.z, 0)) pIni.z = 0;

            if (Geom.Iguais(pFin.x, pIni.x)) pFin.x = pIni.x;

            if (Geom.Iguais(pFin.y, pIni.y)) pFin.y = pIni.y;

            if (Geom.Iguais(pFin.z, pIni.z)) pFin.z = pIni.z;

            /**/
/*
            if (Geom.Iguais(pFin_offset.x, 0)) pFin_offset.x = 0;
            if (Geom.Iguais(pFin_offset.y, 0)) pFin_offset.y = 0;
            if (Geom.Iguais(pFin_offset.z, 0)) pFin_offset.z = 0;

            if (Geom.Iguais(pIni_offset.x, 0)) pIni_offset.x = 0;
            if (Geom.Iguais(pIni_offset.y, 0)) pIni_offset.y = 0;
            if (Geom.Iguais(pIni_offset.z, 0)) pIni_offset.z = 0;

            if (Geom.Iguais(pFin_offset.x, pIni_offset.x)) pFin_offset.x= pIni_offset.x;

            if (Geom.Iguais(pFin_offset.y, pIni_offset.y)) pFin_offset.y= pIni_offset.y;

            if (Geom.Iguais(pFin_offset.z, pIni_offset.z)) pFin_offset.z= pIni_offset.z;*/
        }
        void InicializaSecaoCopia(ref double MultiplicadorAltura, int tipocarga, int caso, int comb)
        {
            secaoCopia_i = (TSecao)barraOriginal.Dados.secaoSemRotacao.Clone();
            secaoCopia_f = (TSecao)barraOriginal.Dados.secaoSemRotacao.Clone();


            cy = ((((pFin.z * -1) - (pIni.z * -1))) / comprimento);

            if (!Geom.Iguais(barraOriginal.pIni.x, barraOriginal.pFin.x))
            {
                if (barraOriginal.pIni.x > barraOriginal.pFin.x)
                {
                    AlfaAlterado = -alfa;
                }
                else
                {
                    AlfaAlterado = alfa;
                }
            }
            else
            if (Geom.Iguais(Math.Abs(cy), 1,0.00001))
            {
                AlfaAlterado = alfa + 90;
            }
            else
            {
                AlfaAlterado = alfa;
            }

            if (secaoCopia_i.poligonos != null)
            {
                if (tipocarga == 0)
                    DeslocamentosLocais = casos_x_esforcos[caso].DeslocamentosLocais;
                else
                if (tipocarga == 1)
                    DeslocamentosLocais = combinacoes_x_esforcos[comb].DeslocamentosLocais;

                centroRotacao.x = 0;
                centroRotacao.y = 0;

                for (int q = 0; q < secaoCopia_i.poligonos.Count; q++)
                {
                    for (int i = 0; i < (secaoCopia_i.poligonos[q].coords.Count()); i++)
                    {
                        coord1 = new vec3(secaoCopia_i.poligonos[q].coords[i].X, secaoCopia_i.poligonos[q].coords[i].Y, 0);

                        if (!Geom.Iguais(barraOriginal.pIni.x, barraOriginal.pFin.x))
                        {
                            if (barraOriginal.pIni.x > barraOriginal.pFin.x)
                            {
                                coord1 = new vec3(secaoCopia_i.poligonos[q].coords[i].X, -secaoCopia_i.poligonos[q].coords[i].Y, 0);

                                //    coord1 = coord1.Rotate(centroRotacao, (AlfaAlterado * Const.PIDiv180) + ((DeslocamentosLocais[4]) * MultiplicadorAltura));
                            }
                            else
                            {
                                coord1 = new vec3(-secaoCopia_i.poligonos[q].coords[i].X, -secaoCopia_i.poligonos[q].coords[i].Y, 0);
                                // coord1 = coord1.Rotate(centroRotacao, (AlfaAlterado * Const.PIDiv180) + ((DeslocamentosLocais[4] * -1) * MultiplicadorAltura));
                            }
                        }
                        else
                        if (Geom.Iguais(Math.Abs(cy), 1, 0.00001))
                        {
                            if ((pIni.z * -1) < (pFin.z * -1))
                            {
                                coord1 = new vec3(secaoCopia_i.poligonos[q].coords[i].X, secaoCopia_i.poligonos[q].coords[i].Y, 0);
                            }
                            else
                            {
                                coord1 = new vec3(-secaoCopia_i.poligonos[q].coords[i].X, -secaoCopia_i.poligonos[q].coords[i].Y, 0);
                            }

                            //   coord1 = coord1.Rotate(centroRotacao, (AlfaAlterado * Const.PIDiv180) + ((DeslocamentosLocais[4] * -1) * MultiplicadorAltura));
                        }
                        else
                        {
                            coord1 = new vec3(-secaoCopia_i.poligonos[q].coords[i].X, -secaoCopia_i.poligonos[q].coords[i].Y, 0);
                            //         coord1 = coord1.Rotate(centroRotacao, (AlfaAlterado * Const.PIDiv180) + ((DeslocamentosLocais[4] * -1) * MultiplicadorAltura));
                        }

                        secaoCopia_i.poligonos[q].coords[i].X = (coord1.x);
                        secaoCopia_i.poligonos[q].coords[i].Y = (coord1.y);
                    }
                }
                centroRotacao.x = 0;
                centroRotacao.y = 0;

                for (int q = 0; q < secaoCopia_f.poligonos.Count; q++)
                {
                    for (int i = 0; i < (secaoCopia_f.poligonos[q].coords.Count()); i++)
                    {
                        coord1 = new vec3(secaoCopia_f.poligonos[q].coords[i].X, secaoCopia_f.poligonos[q].coords[i].Y, 0);

                        if (!Geom.Iguais(barraOriginal.pIni.x, barraOriginal.pFin.x))
                        {
                            if (barraOriginal.pIni.x > barraOriginal.pFin.x)
                            {
                                coord1 = new vec3(secaoCopia_f.poligonos[q].coords[i].X, -secaoCopia_f.poligonos[q].coords[i].Y, 0);
                                //         coord1 = coord1.Rotate(centroRotacao, (AlfaAlterado * Const.PIDiv180) + ((DeslocamentosLocais[10]) * MultiplicadorAltura));
                            }
                            else
                            {
                                coord1 = new vec3(-secaoCopia_f.poligonos[q].coords[i].X, -secaoCopia_f.poligonos[q].coords[i].Y, 0);
                                //            coord1 = coord1.Rotate(centroRotacao, (AlfaAlterado * Const.PIDiv180) + ((DeslocamentosLocais[10] * -1) * MultiplicadorAltura));
                            }
                        }
                        else
                        if (Geom.Iguais(Math.Abs(cy), 1, 0.00001))
                        {
                            if ((pIni.z * -1) < (pFin.z * -1))
                            {
                                coord1 = new vec3(secaoCopia_f.poligonos[q].coords[i].X, secaoCopia_f.poligonos[q].coords[i].Y, 0);
                            }
                            else
                            {
                                coord1 = new vec3(-secaoCopia_f.poligonos[q].coords[i].X, -secaoCopia_f.poligonos[q].coords[i].Y, 0);
                            }
                            // teste jardel
                            //       coord1 = coord1.Rotate(centroRotacao, (AlfaAlterado * Const.PIDiv180) + ((DeslocamentosLocais[10] * -1) * MultiplicadorAltura));
                        }
                        else
                        {
                            coord1 = new vec3(-secaoCopia_f.poligonos[q].coords[i].X, -secaoCopia_f.poligonos[q].coords[i].Y, 0);
                            //        coord1 = coord1.Rotate(centroRotacao, (AlfaAlterado * Const.PIDiv180) + ((DeslocamentosLocais[10] * -1) * MultiplicadorAltura));
                        }

                        secaoCopia_f.poligonos[q].coords[i].X = (coord1.x);
                        secaoCopia_f.poligonos[q].coords[i].Y = (coord1.y);
                    }
                }
             }
        }

        vec3 p2_eixorotacao_y, p2_eixorotacao_z, p2_eixorotacao_x;
        vec3 vecRotacao_Y, vecRotacao_X, vecRotacao_Z;
        double pos_final;
        void InicializaEixosRotacoes(string inicio_ou_fim)
        {
            double pos_fin;

            if (!Geom.Iguais(barraOriginal.pIni.x, barraOriginal.pFin.x))
            {
                if (barraOriginal.pIni.x > barraOriginal.pFin.x)
                    pos_fin = -comprimento;
                else
                    pos_fin = comprimento;
            }
            else
            if (Geom.Iguais(Math.Abs(cy), 1, 0.00001))
                pos_fin = comprimento;
            else
                pos_fin = comprimento;
           
            pos_final = pos_fin;
            pos_fin = 0;

            if (inicio_ou_fim == "I")
            {
                centroRotacao.x = 0;
                centroRotacao.y = 0;
                centroRotacao.z = 0;
            }
            else
            if (inicio_ou_fim == "F")
            {
                centroRotacao.x = pos_fin;
                centroRotacao.y = 0;
                centroRotacao.z = 0;
            }

            vec3 centro = new vec3(centroRotacao.x, centroRotacao.y, centroRotacao.z);

            vec3 pivo = new vec3(centro.x, -centro.y, -centro.z);
            vec3 p1_eixorotacao = new vec3(centro.x, -centro.y, -centro.z);

            p2_eixorotacao_x = new vec3(centro.x + pos_final, -centro.y, -centro.z);
            p2_eixorotacao_y = new vec3(centro.x, -centro.y + 10, -centro.z);
            p2_eixorotacao_z = new vec3(centro.x, -centro.y, -centro.z+10);

            p1_eixorotacao.x -= pivo.x;
            p1_eixorotacao.y -= pivo.y;
            p1_eixorotacao.z -= pivo.z;

            p2_eixorotacao_x.x -= pivo.x;
            p2_eixorotacao_x.y -= pivo.y;
            p2_eixorotacao_x.z -= pivo.z;

            p2_eixorotacao_y.x -= pivo.x;
            p2_eixorotacao_y.y -= pivo.y;
            p2_eixorotacao_y.z -= pivo.z;

            p2_eixorotacao_z.x -= pivo.x;
            p2_eixorotacao_z.y -= pivo.y;
            p2_eixorotacao_z.z -= pivo.z;

            vecRotacao_X = p2_eixorotacao_x - p1_eixorotacao;
            vecRotacao_X.Normalize();

            vecRotacao_Y = p2_eixorotacao_y - p1_eixorotacao;
            vecRotacao_Y.Normalize();

            vecRotacao_Z = p2_eixorotacao_z - p1_eixorotacao;
            vecRotacao_Z.Normalize();
           
            //CoordsSecao_f[i] = new vec3(-comprimento, secaoCopia_i.poligono.coords[i].X, secaoCopia_i.poligono.coords[i].Y);
           
            if (!Geom.Iguais(barraOriginal.pIni.x, barraOriginal.pFin.x))
            {
                if (barraOriginal.pIni.x > barraOriginal.pFin.x)
                {
                    vecRotacao_X.x *= -1;
                    vecRotacao_Z.z *= -1;
              //      vecRotacao_Y.y *= -1;

                    //   coord1 = new vec3(secaoCopia_i.poligono.coords[i].X, -secaoCopia_i.poligono.coords[i].Y, 0);
                    //vecRotacao_X.z
                }
                else
                {
               //     coord1 = new vec3(-secaoCopia_i.poligono.coords[i].X, -secaoCopia_i.poligono.coords[i].Y, 0);
                }
            }
            else
            if (Geom.Iguais(Math.Abs(cy), 1, 0.00001))
            {
                if ((pIni.z * -1) < (pFin.z * -1))
                {
                    //       coord1 = new vec3(secaoCopia_i.poligono.coords[i].X, secaoCopia_i.poligono.coords[i].Y, 0);

                   // vecRotacao_X.x *= -1;
                    vecRotacao_Y.y *= -1;
                    vecRotacao_Z.z *= -1;
                }
                else
                {
             //       coord1 = new vec3(-secaoCopia_i.poligono.coords[i].X, -secaoCopia_i.poligono.coords[i].Y, 0);
                }
            }
            else
            {
            //    coord1 = new vec3(-secaoCopia_i.poligono.coords[i].X, -secaoCopia_i.poligono.coords[i].Y, 0);
            }


        }
        double pos_fin;
        void RotacionaBarraConformeEsforcos(string inicio_ou_fim, string rotacao, ref double MultiplicadorAltura, int tipocarga, int caso, int comb)
        {
            int indice_i = 0, indice_f = 0;

            double angulo, rot = 0;

            if (articulacao_my_ini || articulacao_my_fin || articulacao_mz_ini || articulacao_mz_fin)
            {
                if (tipocarga == 0)
                    DeslocamentosLocais = casos_x_esforcos[caso].DeslocamentosLocaisBarraSemiRigida;
                else
                if (tipocarga == 1)
                    DeslocamentosLocais = combinacoes_x_esforcos[comb].DeslocamentosLocaisBarraSemiRigida;
            }
            else
            {
                if (tipocarga == 0)
                    DeslocamentosLocais = casos_x_esforcos[caso].DeslocamentosLocais;
                else
                if (tipocarga == 1)
                    DeslocamentosLocais = combinacoes_x_esforcos[comb].DeslocamentosLocais;
            }

            if (rotacao == "x")
            {
                indice_i = 4;
                indice_f = 10;
            }
            else
            if (rotacao == "y")
            {
                indice_i = 6;
                indice_f = 12;
            }
            else
            if (rotacao == "z")
            {
                indice_i = 5;
                indice_f = 11;
            }

            if (inicio_ou_fim == "I")
                rot = (DeslocamentosLocais[indice_i]) * MultiplicadorAltura;
            else
            if (inicio_ou_fim == "F")
                rot = (DeslocamentosLocais[indice_f]) * MultiplicadorAltura;
            
            /*limitação de renderização quando a rotação ESCALADA ultrapassar 90 graus. senao a renderização vai rodar a secao demais*/
            if (rotacao == "x" && rot !=0)
            {
                double ang = rot * 57.2958;
                if (ang > 90)
                    rot = 90;
                else
                if (ang < -90)
                    rot = -90;
            }

            if (!Geom.Iguais(barraOriginal.pIni.x, barraOriginal.pFin.x))
            {
                if (barraOriginal.pIni.x > barraOriginal.pFin.x)
                {
                    pos_fin = -comprimento;
                    angulo = rot;
                }
                else
                {
                    pos_fin = comprimento;
                    angulo = (rot * -1) ;
                }
            }
            else
            if (Geom.Iguais(Math.Abs(cy), 1, 0.00001))// && Geom.Iguais(Math.Abs(cz), 0, 0.0001) && Geom.Iguais(Math.Abs(cx), 0, 0.0001))
            {
                //if ((pIni.z * -1) < (pFin.z * -1))
                {
                    pos_fin = comprimento;
                    angulo = (rot * -1);
                }
                /*else
                {
                    pos_fin = -comprimento;
                    angulo = (rot * -1);
                }*/
            }
            else
            {
                pos_fin = comprimento;
                angulo = (rot * -1);
            }

            pos_final = pos_fin;
            pos_fin = 0;

            angulo /= Const.PIDiv180;

            if (rotacao == "y")
              angulo *= 1;

            if (rotacao == "x")
              angulo += AlfaAlterado;
           
            if (inicio_ou_fim == "I")
            {
                centroRotacao.x = 0;
                centroRotacao.y = 0;
                centroRotacao.z = 0;
            }
            else
            if (inicio_ou_fim == "F")
            {
                centroRotacao.x = pos_fin;
                centroRotacao.y = 0;
                centroRotacao.z = 0;
            }
            vec3 p_;
            vec3 centro = new vec3(centroRotacao.x, centroRotacao.y, centroRotacao.z);

            vec3 pivo = new vec3(centro.x, -centro.y, -centro.z);
            vec3 p1_eixorotacao = new vec3(centro.x, -centro.y, -centro.z);
            vec3 eixo = new vec3(0);

            if (rotacao == "x")
                eixo = vecRotacao_X;
            else
            if (rotacao == "y")
                eixo = vecRotacao_Y;
            else
            if (rotacao == "z")
                eixo = vecRotacao_Z;

            p_ = new vec3(vecRotacao_X.x, -vecRotacao_X.y, -vecRotacao_X.z);
            Geom.RotacionaVetor(ref p_, angulo, pivo, eixo, p1_eixorotacao);
            vecRotacao_X.x = p_.x;
            vecRotacao_X.y = p_.y;
            vecRotacao_X.z = p_.z;

            p_ = new vec3(vecRotacao_Y.x, -vecRotacao_Y.y, -vecRotacao_Y.z);
            Geom.RotacionaVetor(ref p_, angulo, pivo, eixo, p1_eixorotacao);
            vecRotacao_Y.x = p_.x;
            vecRotacao_Y.y = p_.y;
            vecRotacao_Y.z = p_.z;

            p_ = new vec3(vecRotacao_Z.x, -vecRotacao_Z.y, -vecRotacao_Z.z);
            Geom.RotacionaVetor(ref p_, angulo, pivo, eixo, p1_eixorotacao);
            vecRotacao_Z.x = p_.x;
            vecRotacao_Z.y = p_.y;
            vecRotacao_Z.z = p_.z;

            for (int q = 0; q < coordssecao_i.Count; q++)
            {
                qtd_CoordsSecao = coordssecao_i[q].Count();

                if (inicio_ou_fim == "I")
                {
                    for (i = 0; i < qtd_CoordsSecao; i++)
                    {
                        p_ = new vec3(coordssecao_i[q][i].x, -coordssecao_i[q][i].y, -coordssecao_i[q][i].z);
                        Geom.RotacionaVetor(ref p_, angulo, pivo, eixo, p1_eixorotacao);
                        coordssecao_i[q][i].x = p_.x;
                        coordssecao_i[q][i].y = p_.y;
                        coordssecao_i[q][i].z = p_.z;
                    }
                }
                else
                if (inicio_ou_fim == "F")
                {
                    for (i = 0; i < qtd_CoordsSecao; i++)
                    {
                        p_ = new vec3(coordssecao_f[q][i].x, -coordssecao_f[q][i].y, -coordssecao_f[q][i].z);
                        Geom.RotacionaVetor(ref p_, angulo, pivo, eixo, p1_eixorotacao);
                        coordssecao_f[q][i].x = p_.x;
                        coordssecao_f[q][i].y = p_.y;
                        coordssecao_f[q][i].z = p_.z;
                    }
                }
            }
        }

        [NonSerialized]
        public Geom.poligono[] triangulos_face_1, triangulos_face_2;
        [NonSerialized]
        public double[] tensoes_ini;
        [NonSerialized]
        public double[] tensoes_fin;

        public void OrientaSecaoNoEspaco(ref double MultiplicadorAltura, int tipocarga, int caso, int comb)
        {
            // if (barra_de_articulacao) return;

            if (Dados.secao.poligonos == null)
                return;
            if (IDBarra == 190)
                IDBarra = 190;

            AcertaPontosNotacaoCientifica();

            InicializaSecaoCopia(ref MultiplicadorAltura, tipocarga, caso, comb);
            //RotacionaBarra_conforme_as_rotacoes(ref MultiplicadorAltura, tipocarga, caso, comb);

        /*    qtd_CoordsSecao = Dados.secao.poligonos[q].coords.Count();
            coordssecao_f.Add(new vec3[qtd_CoordsSecao]);
            coordssecao_i.Add(new vec3[qtd_CoordsSecao]);*/

           // qtd_CoordsSecao = Dados.secao.poligono.coords.Count();

            coordssecao_i = new List<vec3[]>();
            coordssecao_f = new List<vec3[]>();

            xi = pIni_offset.x;//xi_;
            yi = pIni_offset.y;//yi_;
            zi = pIni_offset.z;//zi_;

            xf = pFin_offset.x;//xf_;
            yf = pFin_offset.y;//yf_;
            zf = pFin_offset.z;//zf_;

           // xi-= ex_i;
          //  xf -= ex_f;

            if (MultiplicadorAltura > 0)
                IDBarra *= 1;

            double CY = ((((pFin.z * -1) - (pIni.z * -1))) / comprimento);
            
            double length;
            double x_i;
            if (!Geom.Iguais(pIni.x, pFin.x))
            {
                length = comprimento;
                x_i = 0;// ex_i;

                if (pIni.x > pFin.x)
                {
                    length = -comprimento;
                    //  length -= ex_f;
              //      x_i =  -ex_i;
                }
                //else
                  //  length += ex_f;
            }
            else
            {
                length = comprimento;// + ex_f;
                x_i = 0;// ex_i;
            }

            for (int q = 0; q < secaoCopia_i.poligonos.Count; q++)
            {
                qtd_CoordsSecao = secaoCopia_i.poligonos[q].coords.Count();
                
                coordssecao_f.Add(new vec3[qtd_CoordsSecao]);
                coordssecao_i.Add(new vec3[qtd_CoordsSecao]);

                if (!Geom.Iguais(pIni.x, pFin.x))
                {
                    if (pIni.x > pFin.x)
                    {
                        //AlfaAlterado = -alfa;

                        for (i = 0; i < qtd_CoordsSecao; i++)
                        {
                            coordssecao_i[q][i] = (new vec3(x_i, secaoCopia_i.poligonos[q].coords[i].X, secaoCopia_i.poligonos[q].coords[i].Y));
                            coordssecao_f[q][i] = new vec3(length, secaoCopia_i.poligonos[q].coords[i].X, secaoCopia_i.poligonos[q].coords[i].Y);
                            //   coordssecao_f[q][i] = new vec3(-comprimento, secaoCopia_i.poligonos[q].coords[i].X, secaoCopia_i.poligonos[q].coords[i].Y);
                        }
                    }
                    else
                    {
                        for (i = 0; i < qtd_CoordsSecao; i++)
                        {
                            coordssecao_i[q][i] = new vec3(x_i, secaoCopia_i.poligonos[q].coords[i].X, secaoCopia_i.poligonos[q].coords[i].Y);
                            coordssecao_f[q][i] = new vec3(length, secaoCopia_i.poligonos[q].coords[i].X, secaoCopia_i.poligonos[q].coords[i].Y);
                            //      coordssecao_f[q][i] = new vec3(comprimento, secaoCopia_i.poligonos[q].coords[i].X, secaoCopia_i.poligonos[q].coords[i].Y);
                        }
                    }
                }
                else
                if (Geom.Iguais(Math.Abs(CY), 1, 0.00001))
                {
                    if ((pIni.z * -1) < (pFin.z * -1))
                    {
                        for (i = 0; i < qtd_CoordsSecao; i++)
                        {
                            coordssecao_i[q][i] = new vec3(x_i, secaoCopia_i.poligonos[q].coords[i].X, secaoCopia_i.poligonos[q].coords[i].Y);
                            //  coordssecao_f[q][i] = (new vec3(comprimento, secaoCopia_i.poligonos[q].coords[i].X, secaoCopia_i.poligonos[q].coords[i].Y));
                            coordssecao_f[q][i] = (new vec3(length, secaoCopia_i.poligonos[q].coords[i].X, secaoCopia_i.poligonos[q].coords[i].Y));
                        }
                    }
                    else
                    {
                        for (i = 0; i < qtd_CoordsSecao; i++)
                        {
                            coordssecao_i[q][i] = new vec3(x_i, secaoCopia_i.poligonos[q].coords[i].X, secaoCopia_i.poligonos[q].coords[i].Y);
                            //coordssecao_f[q][i] = (new vec3(comprimento, secaoCopia_i.poligonos[q].coords[i].X, secaoCopia_i.poligonos[q].coords[i].Y));
                            coordssecao_f[q][i] = (new vec3(length, secaoCopia_i.poligonos[q].coords[i].X, secaoCopia_i.poligonos[q].coords[i].Y));
                        }
                    }
                }
                else
                {
                    for (i = 0; i < qtd_CoordsSecao; i++)
                    {
                        coordssecao_i[q][i] = new vec3(x_i, secaoCopia_i.poligonos[q].coords[i].X, secaoCopia_i.poligonos[q].coords[i].Y);
                        //     coordssecao_f[q][i] = (new vec3(comprimento, secaoCopia_i.poligonos[q].coords[i].X, secaoCopia_i.poligonos[q].coords[i].Y));
                        coordssecao_f[q][i] = (new vec3(length, secaoCopia_i.poligonos[q].coords[i].X, secaoCopia_i.poligonos[q].coords[i].Y));
                    }
                }
            }

            if (!Geom.Iguais(barraOriginal.pIni.x, barraOriginal.pFin.x))
            {
                if (barraOriginal.pIni.x > barraOriginal.pFin.x)
                {
                    AlfaAlterado = -alfa;
                }
                else
                {
                    AlfaAlterado = alfa;
                }
            }
            else
            if (Geom.Iguais(Math.Abs(cy), 1, 0.00001))
            {
                AlfaAlterado = alfa + 90;
            }
            else
            {
                AlfaAlterado = alfa;
            }
            

            /* for (i = 0; i < qtd_CoordsSecao; i++)
             {
                    CoordsSecao_f[i].y += -barraOriginal.Dados.ey/1000;
                    CoordsSecao_f[i].z += -barraOriginal.Dados.ez/1000;

                    CoordsSecao_i[i].y += -barraOriginal.Dados.ey / 1000;
                    CoordsSecao_i[i].z += -barraOriginal.Dados.ez / 1000;
             }*/

            InicializaEixosRotacoes("I");
            RotacionaBarraConformeEsforcos("I", "x", ref MultiplicadorAltura, tipocarga, caso, comb);
            RotacionaBarraConformeEsforcos("I", "y", ref MultiplicadorAltura, tipocarga, caso, comb);
            RotacionaBarraConformeEsforcos("I", "z", ref MultiplicadorAltura, tipocarga, caso, comb);

            for (int q = 0; q < secaoCopia_i.poligonos.Count; q++)
                for (i = 0; i < secaoCopia_i.poligonos[q].coords.Count(); i++)
                    coordssecao_f[q][i].x = 0;

            InicializaEixosRotacoes("F");
            RotacionaBarraConformeEsforcos("F", "x", ref MultiplicadorAltura, tipocarga, caso, comb);
            RotacionaBarraConformeEsforcos("F", "y", ref MultiplicadorAltura, tipocarga, caso, comb);
            RotacionaBarraConformeEsforcos("F", "z", ref MultiplicadorAltura, tipocarga, caso, comb);

            for (int q = 0; q < secaoCopia_i.poligonos.Count; q++)
                for (i = 0; i < secaoCopia_i.poligonos[q].coords.Count(); i++)
                   coordssecao_f[q][i].x += length;

            try
            {

                {
                    zi_maior_que_zf = false;
                    //  if (!Geom.Iguais(Math.Abs(zf), Math.Abs(zi), 0.001))
                    zi_maior_que_zf = ((zi * -1) > (zf * -1));

                    xi_igual_xf = Geom.Iguais(pIni_offset.x, pFin_offset.x);

                    tx = (xi);
                    ty = (yi);
                    tz = (zi);

                    xi -= tx;
                    yi -= ty;
                    zi -= tz;

                    xf -= tx;
                    yf -= ty;
                    zf -= tz;

                    if (Geom.Iguais(xi, 0))
                        xi = 0;
                    if (Geom.Iguais(yi, 0))
                        yi = 0;
                    if (Geom.Iguais(zi, 0))
                        zi = 0;

                    if (Geom.Iguais(xf, 0))
                        xf = 0;
                    if (Geom.Iguais(yf, 0))
                        yf = 0;
                    if (Geom.Iguais(zf, 0))
                        zf = 0;

                    /*faço uma translação da barra para o ponto zero, como se eu fizesse o comando mover do programa
                     * para o ponto zero pegando o pIni como pivo */

                    cx = ((xi) - (xf)) / comprimento;
                    cy = ((yi) - (yf)) / comprimento;

                    cz = ((zi) - (zf)) / comprimento;

                    u1 = new vec3(xi, yi, zi * -1);
                    u2 = new vec3(xf, yf, zf * -1);

                    //Encontrar angulo que a barra faz com os planos XY e XZ

                    //PLANO XY
                    normxy = new vec3(0, 0, 1);

                    u = u1 - u2;


                    NdotU = (normxy.DotProduct(u));
                    ndotu_mod = normxy.Magnitude() * u.Magnitude();
                    cos_alfa = Math.Abs(NdotU / ndotu_mod);
                    angXY = RMath.rad2deg(Math.Acos(cos_alfa));
                    angXY = (90 - angXY) * 1;

                    //PLANO XZ
                    normxz = new vec3(0, 1, 0);
                    u = u1 - u2;

                    NdotU = (normxz.DotProduct(u));
                    ndotu_mod = normxz.Magnitude() * u.Magnitude();
                    cos_alfa = Math.Abs(NdotU / ndotu_mod);

                    if (!Geom.Iguais(u1.x - u2.x, 0))
                        angXZ = RMath.rad2deg(Math.Atan(u1.y - u2.y / (u1.x - u2.x)));
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
                        if (pIni_offset.x < pFin_offset.x)
                            angXY *= -1;

                    if (!Geom.Iguais(Math.Abs(angXZ), 90))
                        angXZ *= -1;

                    if (xi_igual_xf)
                        angXY *= -1;

                    if (zi_maior_que_zf)
                        angXY *= -1;

                    for (int q = 0; q < secaoCopia_i.poligonos.Count; q++)
                    {
                        for (i = 0; i < secaoCopia_i.poligonos[q].coords.Count(); i++)
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

                            coordssecao_i[q][i].x = posicaoFinal2[1] + tx;
                            coordssecao_i[q][i].y = (posicaoFinal2[2] + ty) * -1;
                            coordssecao_i[q][i].z = (posicaoFinal2[3] + tz) * -1;

                            posicao[1] = xf;
                            posicao[2] = yf;
                            posicao[3] = zf;

                            Geom.rotY(angXY, ref posicao, ref posicaoFinal);
                            Geom.rotZ(angXZ, ref posicaoFinal, ref posicaoFinal2);

                            coordssecao_f[q][i].x = posicaoFinal2[1] + tx;
                            coordssecao_f[q][i].y = (posicaoFinal2[2] + ty) * -1;
                            coordssecao_f[q][i].z = (posicaoFinal2[3] + tz) * -1;

                            coordssecao_f[q][i].pontoEmRaio = Dados.secao.poligonos[q].coords[i].pontoEmRaio;
                            coordssecao_i[q][i].pontoEmRaio = Dados.secao.poligonos[q].coords[i].pontoEmRaio;
                        }
                    }

                    if (MultiplicadorAltura == 0)
                    {
                       // trisFace1 = new List<triangulos>();
                   //     trisFace2 = new List<triangulos>();

                        /*if (Geom.Iguais(pIni.x, barraOriginal.pIni.x) && Geom.Iguais(pIni.y, barraOriginal.pIni.y) && Geom.Iguais(pIni.z, barraOriginal.pIni.z))
                        {
                            for (i = 0; i < barraOriginal.triangulos_face_1.Count(); i++)
                            {
                                vec3 p1 = barraOriginal.triangulos_face_1[i].vertices[0];
                                vec3 p2 = barraOriginal.triangulos_face_1[i].vertices[1];
                                vec3 p3 = barraOriginal.triangulos_face_1[i].vertices[2];


                                List<int[]> indices = new List<int[]>();

                                for (int q = 0; q < coordssecao_i.Count; q++)
                                {
                                    indices.Add(new int[coordssecao_i[q].Count()]);
                                    
                                    for (j = 0; j < coordssecao_i[q].Count(); j++)
                                    {
                                        if (Geom.Iguais(p1.x, coordssecao_i[q][j].x) && Geom.Iguais(p1.y, -coordssecao_i[q][j].y) && Geom.Iguais(p1.z, -coordssecao_i[q][j].z))
                                        {
                                            indices[q][j] = j;
                                            break;
                                        }
                                    }

                                    for (j = 0; j < coordssecao_i[q].Count(); j++)
                                    {
                                        if (Geom.Iguais(p2.x, coordssecao_i[q][j].x) && Geom.Iguais(p2.y, -coordssecao_i[q][j].y) && Geom.Iguais(p2.z, -coordssecao_i[q][j].z))
                                        {
                                            indices.Add(j);
                                            break;
                                        }
                                    }

                                    for (j = 0; j < coordssecao_i[q].Count(); j++)
                                    {
                                        if (Geom.Iguais(p3.x, coordssecao_i[q][j].x) && Geom.Iguais(p3.y, -coordssecao_i[q][j].y) && Geom.Iguais(p3.z, -coordssecao_i[q][j].z))
                                        {
                                            indices.Add(j);
                                            break;
                                        }
                                    }
                                }

                                trisFace1.Add(new triangulos(indices));
                            }
                        }

                        if (Geom.Iguais(pFin.x, barraOriginal.pFin.x) && Geom.Iguais(pFin.y, barraOriginal.pFin.y) && Geom.Iguais(pFin.z, barraOriginal.pFin.z))
                        {
                            for (i = 0; i < barraOriginal.triangulos_face_2.Count(); i++)
                            {
                                vec3 p1 = barraOriginal.triangulos_face_2[i].vertices[0];
                                vec3 p2 = barraOriginal.triangulos_face_2[i].vertices[1];
                                vec3 p3 = barraOriginal.triangulos_face_2[i].vertices[2];

                                /*  p1.y += -barraOriginal.Dados.ey / 1000;
                                  p1.z += -barraOriginal.Dados.ez / 1000;

                                  p2.y += -barraOriginal.Dados.ey / 1000;
                                  p2.z += -barraOriginal.Dados.ez / 1000;

                                  p3.y += -barraOriginal.Dados.ey / 1000;
                                  p3.z += -barraOriginal.Dados.ez / 1000;*/

                           /*     List<int> indices = new List<int>();

                                for (int q = 0; q < coordssecao_f.Count; q++)
                                {
                                    for (j = 0; j < coordssecao_f[q].Count(); j++)
                                    {
                                        if (Geom.Iguais(p1.x, coordssecao_f[q][j].x) && Geom.Iguais(p1.y, -coordssecao_f[q][j].y) && Geom.Iguais(p1.z, -coordssecao_f[q][j].z))
                                        {
                                            indices.Add(j);
                                            break;
                                        }
                                    }

                                    for (j = 0; j < coordssecao_f[q].Count(); j++)
                                    {
                                        if (Geom.Iguais(p2.x, coordssecao_f[q][j].x) && Geom.Iguais(p2.y, -coordssecao_f[q][j].y) && Geom.Iguais(p2.z, -coordssecao_f[q][j].z))
                                        {
                                            indices.Add(j);
                                            break;
                                        }
                                    }

                                    for (j = 0; j < coordssecao_f[q].Count(); j++)
                                    {
                                        if (Geom.Iguais(p3.x, coordssecao_f[q][j].x) && Geom.Iguais(p3.y, -coordssecao_f[q][j].y) && Geom.Iguais(p3.z, -coordssecao_f[q][j].z))
                                        {
                                            indices.Add(j);
                                            break;
                                        }
                                    }
                                }

                                trisFace2.Add(new triangulos(indices));
                            }
                        }

                        if (Geom.Iguais(pIni.x, barraOriginal.pFin.x) && Geom.Iguais(pIni.y, barraOriginal.pFin.y) && Geom.Iguais(pIni.z, barraOriginal.pFin.z))
                        {
                            for (i = 0; i < barraOriginal.triangulos_face_2.Count(); i++)
                            {
                                vec3 p1 = barraOriginal.triangulos_face_2[i].vertices[0];
                                vec3 p2 = barraOriginal.triangulos_face_2[i].vertices[1];
                                vec3 p3 = barraOriginal.triangulos_face_2[i].vertices[2];

                     /*           List<int> indices = new List<int>();
                                for (int q = 0; q < coordssecao_f.Count; q++)
                                {
                                    for (j = 0; j < coordssecao_f[q].Count(); j++)
                                    {
                                        if (Geom.Iguais(p1.x, coordssecao_f[q][j].x) && Geom.Iguais(p1.y, -coordssecao_f[q][j].y) && Geom.Iguais(p1.z, -coordssecao_f[q][j].z))
                                        {
                                            indices.Add(j);
                                            break;
                                        }
                                    }

                                    for (j = 0; j < coordssecao_f[q].Count(); j++)
                                    {
                                        if (Geom.Iguais(p2.x, coordssecao_f[q][j].x) && Geom.Iguais(p2.y, -coordssecao_f[q][j].y) && Geom.Iguais(p2.z, -coordssecao_f[q][j].z))
                                        {
                                            indices.Add(j);
                                            break;
                                        }
                                    }

                                    for (j = 0; j < coordssecao_f[q].Count(); j++)
                                    {
                                        if (Geom.Iguais(p3.x, coordssecao_f[q][j].x) && Geom.Iguais(p3.y, -coordssecao_f[q][j].y) && Geom.Iguais(p3.z, -coordssecao_f[q][j].z))
                                        {
                                            indices.Add(j);
                                            break;
                                        }
                                    }
                                }

                                trisFace1.Add(new triangulos(indices));
                            }
                        }*/
                    }
                }
            }

            catch (Exception e)
            {
                System.Windows.Forms.MessageBox.Show(e.Message);
            }
        }

        List<triangulos> trisFace1, trisFace2;
        struct triangulos
        {
            public List<int> indices;
            public triangulos(List<int> _indices)
            {
                indices = _indices;
            }
        }

        void GiraDiagramaConformeAnguloAlfa(ref vec3 coord, string eixo)
        {
            vec3 vetorRotacao;
            vec3 centroRotacao = new vec3(0);
            if (eixo == "Z")
            {
                vetorRotacao = new vec3(0, coord.z, 0);
                vetorRotacao = vetorRotacao.Rotate(centroRotacao, (AlfaEixosPrincipaisAlterado + barraOriginal.AlfaAlterado) * Const.PIDiv180);
                coord.y = vetorRotacao.x;
                coord.z = vetorRotacao.y;
            }
            else
            if (eixo == "Y")
            {
                vetorRotacao = new vec3(coord.y, 0, 0);
                vetorRotacao = vetorRotacao.Rotate(centroRotacao, (AlfaEixosPrincipaisAlterado + barraOriginal.AlfaAlterado) * Const.PIDiv180);
                coord.y = vetorRotacao.x;
                coord.z = vetorRotacao.y;
            }
            else
            if (eixo == "X")
            {
                vetorRotacao = new vec3(coord.y, 0, 0);
                vetorRotacao = vetorRotacao.Rotate(centroRotacao, (AlfaEixosPrincipaisAlterado + barraOriginal.AlfaAlterado) * Const.PIDiv180);
                coord.y = -vetorRotacao.x;
                coord.z = -vetorRotacao.y;
            }

            /*double ang = (AlfaEixosPrincipaisAlterado + barraOriginal.AlfaAlterado) * Const.PIDiv180;

            double c = Math.Cos(ang);
            double s = Math.Sin(ang);

            double y = coord.y;
            double z = coord.z;

            coord.y = c * y - s * z;
            coord.z = s * y + c * z;*/
        }

        public vec3[] coordsForca, coordsFY, coordsSuavizadas, coordsFletorY, coordsFletorZ, coordsTorcor;
         
       vec3 n1, v1, v2, p1, p2, p3;

        public bool temOffset;
        public void SetMatrizGlobal()
        {
            M = new double[13, 13];
            MatRotacaoTransposta = new double[13, 13];
            MatrizGlobal = new double[13, 13];

            //[K] = [R]t.[k]e.[R]
            temOffset = !Geom.Iguais(barraOriginal.Dados.ez, 0) || !Geom.Iguais(barraOriginal.Dados.ey, 0) || tem_ex_f || tem_ex_i;

            if (temOffset)
            {
                SetMatrizOffset(
                    0,
                    barraOriginal.Dados.ey / 1000, //mm -> m
                    barraOriginal.Dados.ez / 1000,
                    0,
                    barraOriginal.Dados.ey / 1000,
                    barraOriginal.Dados.ez / 1000);

                MatrizLocal_sem_offset = new double[13, 13];

                /*for (int i = 1; i <= 13; i++)
                    for (int j = 1; j <= 13; j++)
                        MatrizLocal2[i, j] = MatrizLocal[i, j];
                */
                MatrizLocal_sem_offset = (double[,])MatrizLocal.Clone();

                MatrizOffset_Transposta = new double[13, 13];
                
                TAlgebra.Transposta(ref MatrizOffset, ref MatrizOffset_Transposta, 12, 12);
                TAlgebra.Multiplica_Matriz_Matriz(ref MatrizOffset_Transposta, ref MatrizLocal, ref M, 12, 12);

                MatrizLocal = new double[13, 13];

                TAlgebra.Multiplica_Matriz_Matriz(ref M, ref MatrizOffset, ref MatrizLocal, 12, 12);

                TAlgebra.Transposta(ref MatrizRotacao, ref MatRotacaoTransposta, 12, 12);                           // [R] -> [R]t                        
                TAlgebra.Multiplica_Matriz_Matriz(ref MatRotacaoTransposta, ref MatrizLocal, ref M, 12, 12);    // [M]  = [R]t.[K]e
                TAlgebra.Multiplica_Matriz_Matriz(ref M, ref MatrizRotacao, ref MatrizGlobal, 12, 12);          // [k] = [M].[R]

            }
            else
            { 
               TAlgebra.Transposta(ref MatrizRotacao, ref MatRotacaoTransposta, 12, 12);                           // [R] -> [R]t                        
               TAlgebra.Multiplica_Matriz_Matriz(ref MatRotacaoTransposta, ref MatrizLocal, ref M, 12, 12);    // [M]  = [R]t.[K]e
               TAlgebra.Multiplica_Matriz_Matriz(ref M, ref MatrizRotacao, ref MatrizGlobal, 12, 12);          // [k] = [M].[R]
             }
        }

        double r1, r2, r3, r4, r5, r6, r7, r8, eiy, eiz,ea, l2, l3, ari, arj;
        public bool articulacao_my_ini, articulacao_my_fin, articulacao_mz_ini, articulacao_mz_fin, barra_de_articulacao;
        public void SetRigidezNos()
        {
            ari = 0;
            arj = 0;

              if (barraOriginal.Dados.Articulacao_mz > 0)
              {
                  double correcao_3x3 = 1;// (ari + arj + (ari * arj)) / (4 - (ari * arj));
                  double correcao_3x5 = (ari * (2 + arj)) / (4 - (ari * arj));
                  double correcao_5x3 = correcao_3x5;
                  double correcao_5x5 = 3 * ari / (4 - (ari * arj));

                  double correcao_3x9 = correcao_3x3;
                  double correcao_3x11 = (arj * (2 + ari)) / (4 - (ari * arj));
                  double correcao_5x9 = correcao_3x5;
                  double correcao_5x11 = (3 * ari * arj) / (4 - (ari * arj));

                  double correcao_9x3 = correcao_3x3;
                  double correcao_9x5 = correcao_3x5;
                  double correcao_11x3 = correcao_3x11;
                  double correcao_11x5 = correcao_5x11;

                  double correcao_9x9 = correcao_3x3;
                  double correcao_9x11 = correcao_3x11;
                  double correcao_11x9 = correcao_3x3;
                  double correcao_11x11 = 3 * arj / (4 - (ari * arj));

                  MatrizLocal[3, 3] *= correcao_3x3;
                  MatrizLocal[3, 5] *= correcao_3x5;
                  MatrizLocal[5, 3] *= correcao_5x3;
                  MatrizLocal[5, 5] *= correcao_5x5;

                  MatrizLocal[3, 9] *= correcao_3x9;
                  MatrizLocal[3, 11] *= correcao_3x11;
                  MatrizLocal[5, 9] *= correcao_5x9;
                  MatrizLocal[5, 11] *= correcao_5x11;

                  MatrizLocal[9, 3] *= correcao_9x3;
                  MatrizLocal[9, 5] *= correcao_9x5;
                  MatrizLocal[11, 3] *= correcao_11x3;
                  MatrizLocal[11, 5] *= correcao_11x5;

                  MatrizLocal[9, 9] *= correcao_9x9;
                  MatrizLocal[9, 11] *= correcao_9x11;
                  MatrizLocal[11, 9] *= correcao_11x9;
                  MatrizLocal[11, 11] *= correcao_11x11;

                double coef = barraOriginal.Dados.k_mz; // se o usuario informa uma rigidez explicita

                if (coef > 0)
                {
                    MatrizLocal[3, 3] *= 1;
                    MatrizLocal[3, 5] *= 1;
                    MatrizLocal[5, 3] = coef;
                    MatrizLocal[5, 5] = coef;

                    MatrizLocal[3, 9] *= 1;
                    MatrizLocal[3, 11] *= 1;
                    MatrizLocal[5, 9] = coef;
                    MatrizLocal[5, 11] = coef;

                    MatrizLocal[9, 3] *= 1;
                    MatrizLocal[9, 5] *= 1;
                    MatrizLocal[11, 3] = coef;
                    MatrizLocal[11, 5] = coef;

                    MatrizLocal[9, 9] *= 1;
                    MatrizLocal[9, 11] *= 1;
                    MatrizLocal[11, 9] = coef;
                    MatrizLocal[11, 11] = coef;
                }

            }

            if (barraOriginal.Dados.Articulacao_my > 0)
             {
                 double correcao_2x2 = 1;// ari + arj + (ari * arj) / (4 - (ari * arj));
                 double correcao_2x6 = (ari * (2 + arj)) / (4 - (ari * arj));
                 double correcao_6x2 = correcao_2x6;
                 double correcao_6x6 = 3 * ari / (4 - (ari * arj));

                 double correcao_2x8  = correcao_2x2;
                 double correcao_2x12 = (arj * (2 + ari)) / (4 - (ari * arj));
                 double correcao_6x8  = correcao_2x6;
                 double correcao_6x12 = (3 * ari * arj) / (4 - (ari * arj));

                 double correcao_8x2  = correcao_2x2;
                 double correcao_8x6  = correcao_2x6;
                 double correcao_12x2 = correcao_2x12;
                 double correcao_12x6 = correcao_6x12;

                 double correcao_8x8   = correcao_2x2;
                 double correcao_8x12  = correcao_2x12;
                 double correcao_12x8  = correcao_2x2;
                 double correcao_12x12 = 3 * arj / (4 - (ari * arj));

                 MatrizLocal[2,2] *= correcao_2x2;
                 MatrizLocal[2,6] *= correcao_2x6;
                 MatrizLocal[6,2] *= correcao_6x2;
                 MatrizLocal[6,6] *= correcao_6x6;

                 MatrizLocal[2,8] *= correcao_2x8;
                 MatrizLocal[2,12] *= correcao_2x12;
                 MatrizLocal[6,8] *= correcao_6x8;
                 MatrizLocal[6,12] *= correcao_6x12;

                 MatrizLocal[8,2] *= correcao_8x2;
                 MatrizLocal[8,6] *= correcao_8x6;
                 MatrizLocal[12,2] *= correcao_12x2;
                 MatrizLocal[12,6] *= correcao_12x6;

                 MatrizLocal[8,8] *= correcao_8x8;
                 MatrizLocal[8,12] *= correcao_8x12;
                 MatrizLocal[12,8] *= correcao_12x8;
                 MatrizLocal[12,12] *= correcao_12x12;

                double coef = barraOriginal.Dados.k_my; // se o usuario informa uma rigidez explicita

                if (coef > 0)
                {
                    MatrizLocal[2, 2] *= 1;
                    MatrizLocal[2, 6] *= 1;
                    MatrizLocal[6, 2] = coef;
                    MatrizLocal[6, 6] = coef;

                    MatrizLocal[2, 8] *= 1;
                    MatrizLocal[2, 12] *= 1;
                    MatrizLocal[6, 8] = coef;
                    MatrizLocal[6, 12] = coef;

                    MatrizLocal[8, 2] *= 1;
                    MatrizLocal[8, 6] *= 1;
                    MatrizLocal[12, 2] = coef;
                    MatrizLocal[12, 6] = coef;

                    MatrizLocal[8, 8] *= 1;
                    MatrizLocal[8, 12] *= 1;
                    MatrizLocal[12, 8] = coef;
                    MatrizLocal[12, 12] = coef;
                }
            }
        }
        void InsereSubMatriz(double[,] target, double[,] sub, int rowStart, int colStart)
        {
            int rows = sub.GetLength(0);
            int cols = sub.GetLength(1);

            for (int i = 0; i < rows; i++)
            {
                for (int j = 0; j < cols; j++)
                {
                    target[rowStart + i, colStart + j] = sub[i, j];
                }
            }
        }
        //matriz de transformação que modifica a cinematica da barra considerando offsets
        public void SetMatrizOffset(double ex1, double ey1, double ez1, double ex2, double ey2, double ez2)
        {                 
            
            double[,] mtr_no1 = new double[,]
                   {
           { 1, 0, 0, 0,  ey1, -ez1 },
           { 0, 1, 0, -ey1, 0,  -ex1 },
           { 0, 0, 1,  ez1,ex1 , 0 },
           { 0,0 , 0, 1,  0,   0 },
           { 0, 0, 0, 0,  1,   0 },
           { 0, 0, 0, 0,  0,   1 }
                   };

     
              double[,] mtr_no2 = new double[,]
                   {
           { 1, 0, 0, 0,  ey2, -ez2 },
           { 0, 1, 0, -ey2, 0,  -ex2 },
           { 0, 0, 1,  ez2,ex2 , 0 },
           { 0, 0, 0, 1,  0,   0 },
           { 0, 0, 0, 0,  1,   0 },
           { 0, 0, 0, 0,  0,   1 }
                   };

            MatrizOffset = new double[13, 13];

            // bloco superior esquerdo
            InsereSubMatriz(MatrizOffset, mtr_no1, 1, 1);

            // bloco inferior direito
            InsereSubMatriz(MatrizOffset, mtr_no2, 7, 7);
        }
        double Iyz1;
     
        //atributos caso seja uma barra semirigida
        double[,] MatrizLocalExpandida_ComMolas;
        double[,] MatrizLocalExpandida_SemMolas;
        double[,] KiiInv;
        double[,] Kie;
        double[,] Kei;
        public double KMy_Inicio, KMz_Inicio, KMy_Final, KMz_Final, ex_i, ex_f;
        public bool tem_ex_f, tem_ex_i;
        public void SetMatrizLocalSemiRigida()
        {
           /* if (articulacao_my_ini)
            {
                if (barraOriginal.Dados.k_my > 0)
                {
                    KMy_Inicio = barraOriginal.Dados.k_my;
                }
            }
            if (articulacao_my_fin)
            {
                if (barraOriginal.Dados.k_my > 0)
                {
                    KMy_Inicio = barraOriginal.Dados.k_my;
                    KMy_Final = barraOriginal.Dados.k_my;
                }
            }*/

            MatrizLocalExpandida_ComMolas = new double[17, 17];

            int[] mapa =
                {0,   // não usado
                1,   // ux_i
                2,   // uy_i
                3,   // uz_i
                4,   // rx_i

                13,  // ry_i da barra
                14,  // rz_i da barra

                7,   // ux_j
                8,   // uy_j
                9,   // uz_j
                10,  // rx_j

                15,  // ry_j da barra
                16   // rz_j da barra
            };

            for (int i = 1; i <= 12; i++)
                for (int j = 1; j <= 12; j++)
                    MatrizLocalExpandida_ComMolas[mapa[i], mapa[j]] += MatrizLocal[i, j];

            MatrizLocalExpandida_SemMolas = (double[,])MatrizLocalExpandida_ComMolas.Clone();

            AdicionarMolaRotacional(MatrizLocalExpandida_ComMolas, 5, 13, KMz_Inicio);
            AdicionarMolaRotacional(MatrizLocalExpandida_ComMolas, 11, 15, KMz_Final);

            AdicionarMolaRotacional(MatrizLocalExpandida_ComMolas, 6, 14, KMy_Inicio);
            AdicionarMolaRotacional(MatrizLocalExpandida_ComMolas, 12, 16, KMy_Final);
 
            int[] externos = {1,2,3,4,5,6,7,8,9,10,11,12};

            int[] internos = {13,14,15,16};

            // Condensação estática
            double[,] Keq = TAlgebra.Condensar(MatrizLocalExpandida_ComMolas,externos,internos, ref Kie, ref Kei, ref KiiInv);
            
            // Copia para a matriz local da barra
            for (int i = 1; i <= 12; i++)
            {
                for (int j = 1; j <= 12; j++)
                {
                    MatrizLocal[i, j] = Keq[i, j];
                }
            }
        }

        void AdicionarMolaRotacional(double[,] K,int glNo,int glBarra,double rigidez)
        {
            K[glNo, glNo] += rigidez;

            K[glNo, glBarra] -= rigidez;

            K[glBarra, glNo] -= rigidez;

            K[glBarra, glBarra] += rigidez;
        }

        public void SetMatrizLocal()
        {
            eiy = E1 * Iy1;
            eiz = E1 * Iz1;
            l2 = Math.Pow(L, 2);
            l3 = Math.Pow(L, 3);
            ea = A1 * E1;
            
            if (barraRigida)
            {
                eiz = 150000000;
                eiy = 150000000;
                ea = 150000000;
                G1 = 150000000;
                J1 = 1;
            }
           
            //no caso de laje, ou grelha de barras..
          // J1 = Iz1*2;

            MatrizLocal = new double[13, 13];
          
            for (i = 1; i <= 12; i++)
                for (j = i; j <= 12; j++)
                    MatrizLocal[j, i] = 0;

          MatrizLocal[1,1]  =  ea / L;    
          MatrizLocal[1,7]  = -ea / L;
          
          MatrizLocal[2,2]   = 12* eiz / l3;
          MatrizLocal[2,6]   = 6 * eiz / l2;
          MatrizLocal[2, 8]  = -12 * eiz / l3;
          MatrizLocal[2, 12] = 6 * eiz / l2;

          MatrizLocal[3, 3] = 12 * eiy / l3 ; 
          MatrizLocal[3,5]  = -6*eiy/l2; 
          MatrizLocal[3,9]  = -12*eiy/l3; 
          MatrizLocal[3,11] =  -6*eiy/l2;

          MatrizLocal[4,4]   =  (G1 * J1) / L;
          MatrizLocal[4, 10] = -(G1 * J1) / L;

          MatrizLocal[5, 5] = 4 * eiy / L;
          MatrizLocal[5,9]  = 6 * eiy / l2; 
          MatrizLocal[5,11] = 2 * eiy / L;

          MatrizLocal[6, 6] = (4 * eiz / L);
          MatrizLocal[6,8]  = -6*eiz / l2; 
          MatrizLocal[6,12] = 2*eiz/L;

          MatrizLocal[7,7]  = ea/L;

          MatrizLocal[8,8]  = 12*eiz/l3; 
          MatrizLocal[8,12] = -6*eiz/l2;

          MatrizLocal[9,9]  = 12*eiy/l3; 
          MatrizLocal[9,11] = 6*eiy/l2; 

          MatrizLocal[10, 10] = (G1 * J1) / L; 

          MatrizLocal[11,11]  = 4*eiy / L ;

          MatrizLocal[12, 12] = (4 * eiz / L);

            //preenche lado simétrico
          for (i = 1; i <= 12; i++)
            for (j = i; j <= 12; j++)
              MatrizLocal[j, i] = MatrizLocal[i, j];

            /*if (IDBarra == 1)
            {
                articulacao_ini = true;
                articulacao_fin = false;
            }*/

         //  if (barra_de_articulacao)
           //  SetRigidezNos();

           /*if (barraOriginal.Dados.Tipo == 4) // braço rígido
           {
                for (i = 1; i <= 12; i++)
                {
                    for (j = 1; j <= 12; j++)
                    {
                        MatrizLocal[i, j] = -1 * 1000000;

                        if (i == j)
                            MatrizLocal[i, j] = 1 * 1000000;
                    }
                }
            }*/

           if (!Geom.Iguais(barraOriginal.Dados.secao.propriedades.anguloEixosPrincipais, 0))
              TransformaMatrizLocal_EixosPrincipais_Para_EixosGeometricos();
        }

        void TransformaMatrizLocal_EixosPrincipais_Para_EixosGeometricos()
        {
            /*               27/06/2026
             *  Se os angulos principais nao coincidem com os eixos geometricos da secao,uma cantoneira de 
             * abas iguais por exemplo com angulo principal = 45°,
             * entao tem que transformar a matriz local, que está nos eixos principais, ou seja usando inercia u e inercia v, 
             * transformar a matriz local para eixos geometricos
            aplicando a matriz de transformação T  ---->   Kgeom = Tt * matrizLocal * T
            pois o elemento nao entende que agora ele esta usando as inercias principais u,v e nao nao Iz e Iy */

            /*a matriz local nao pode ficar escrita nos eixos principais;
             o restante do código (cargas, visualização, rotação da barra) trabalha nos eixos geométricos;
                tem que fazer a mudança de base da matriz local.*/

            T_EixosPrincipais_bloco = new double[4, 4];
            T_EixosPrincipais = new double[13, 13];
            T_EixosPrincipais_Transposta = new double[13, 13];
            M = new double[13, 13];

            double alfaPrincipal = barraOriginal.Dados.secao.propriedades.anguloEixosPrincipais;
            cos_alpha = Math.Cos(alfaPrincipal );
            sen_alpha = Math.Sin(alfaPrincipal );

            T_EixosPrincipais_bloco[1, 1] = 1;
            T_EixosPrincipais_bloco[1, 2] = 0;
            T_EixosPrincipais_bloco[1, 3] = 0;

            T_EixosPrincipais_bloco[2, 1] = 0;
            T_EixosPrincipais_bloco[2, 2] = cos_alpha;
            T_EixosPrincipais_bloco[2, 3] = sen_alpha;

            T_EixosPrincipais_bloco[3, 1] = 0;
            T_EixosPrincipais_bloco[3, 2] = -sen_alpha;
            T_EixosPrincipais_bloco[3, 3] = cos_alpha;

            int lin, col, incr = -4;
            for (k = 0; k < 4; k++)
            {
                incr += 4;
                for (i = 1; i <= 3; i++)
                    for (j = 1; j <= 3; j++)
                    {
                        lin = i + incr - k;
                        col = j + incr - k;
                        T_EixosPrincipais[lin, col] = T_EixosPrincipais_bloco[i, j];
                    }
            }

            TAlgebra.Transposta(ref T_EixosPrincipais, ref T_EixosPrincipais_Transposta, 12, 12);
            TAlgebra.Multiplica_Matriz_Matriz(ref T_EixosPrincipais_Transposta, ref MatrizLocal, ref M, 12, 12);

            MatrizLocal = new double[13, 13];
            TAlgebra.Multiplica_Matriz_Matriz(ref M, ref T_EixosPrincipais, ref MatrizLocal, 12, 12);
        }

        public void SetMatrizGeometrica(double P, double M1_z, double M1_y, double M2_z, double M2_y, double T)
        {
            MatrizGeometrica = new double[13, 13];

            for (i = 1; i <= 12; i++)
                for (j = i; j <= 12; j++)
                    MatrizGeometrica[j, i] = 0;

            MatrizGeometrica[1, 1] = P / L;

            MatrizGeometrica[2, 2] = 6/5 * (P/L);

            MatrizGeometrica[3, 3] = 6 / 5 * (P / L);

            MatrizGeometrica[4, 2] = M1_y/L;
            MatrizGeometrica[4, 3] = M1_z/L;
            MatrizGeometrica[4, 4] = (P * G1) / (A1*L);
            
            MatrizGeometrica[5, 2] = T/L;
            MatrizGeometrica[5, 3] = -P/10;
            MatrizGeometrica[5, 4] = -(2*M1_z - M2_z)/6;
            MatrizGeometrica[5, 5] = (2*P*L) / 15;
            
            MatrizGeometrica[6, 2] = P / 10;
            MatrizGeometrica[6, 3] = T / L;
            MatrizGeometrica[6, 4] = (2 * M1_y - M2_y) / 6;
            MatrizGeometrica[6, 6] = (2 * P * L) / 15;
            
            MatrizGeometrica[7, 1] = -P / L;
            MatrizGeometrica[7, 7] =  P / L;
            
            MatrizGeometrica[8, 2] = -6 / 5 * (P / L);
            MatrizGeometrica[8, 4] = -M1_y / L;
            MatrizGeometrica[8, 5] = -T / L;
            MatrizGeometrica[8, 6] = -P / 10;
            MatrizGeometrica[8, 8] = 6 / 5 * (P / L);


            MatrizGeometrica[9, 3] = -6 / 5 * (P / L);
            MatrizGeometrica[9, 4] = -M1_z / L;
            MatrizGeometrica[9, 5] = P / 10;
            MatrizGeometrica[9, 6] = -T / L;
            MatrizGeometrica[9, 9] = 6 / 5 * (P / L);


            MatrizGeometrica[10, 2] = M2_y / L;
            MatrizGeometrica[10, 3] = M2_z / L;
            MatrizGeometrica[10, 4] = -(P * G1) / (A1 * L);
            MatrizGeometrica[10, 5] = -(M1_z + M2_z) / 6;
            MatrizGeometrica[10, 6] = (M1_y + M2_y) / 6;
            MatrizGeometrica[10, 8] = -(M2_y) / L;
            MatrizGeometrica[10, 9] = -(M2_z) / L;
            MatrizGeometrica[10, 10] = (P * G1) / (A1 * L);

            MatrizGeometrica[11, 2] = -T / L;
            MatrizGeometrica[11, 3] = -P / 10;
            MatrizGeometrica[11, 4] = -(M1_z + M2_z) / 6;
            MatrizGeometrica[11, 5] = -(P*L) / 30;
            MatrizGeometrica[11, 6] = -(T) / 2;
            MatrizGeometrica[11, 8] = (T) / L;
            MatrizGeometrica[11, 9] = (P) / 10;
            MatrizGeometrica[11, 10] = (M1_z - M2_z) / 6;
            MatrizGeometrica[11, 11] = (2*P*L) / 15;


            MatrizGeometrica[12, 2] = P / 10;
            MatrizGeometrica[12, 3] = -T / L;
            MatrizGeometrica[12, 4] = (M1_y + M2_y) / 6;
            MatrizGeometrica[12, 5] = T / 2;
            MatrizGeometrica[12, 6] = -P*L/30;
            MatrizGeometrica[12, 8] = -P / 10;
            MatrizGeometrica[12, 9] = T / L;
            MatrizGeometrica[12, 10] = -(M1_y + 2*M2_y) / 6;
            MatrizGeometrica[12, 12] = (2 * P * L) / 15;


            //preenche lado simétrico
            for (i = 1; i <= 12; i++)
                for (j = i; j <= 12; j++)
                    MatrizGeometrica[j, i] = MatrizGeometrica[i, j];
        }

        int lin, col, gl;
        public List<TCargaLinear> cargasLineares;
        TCargaLinear c1;
        vec3 vetorCarga;
        double angulocarga1;
        vec3 z_local,x_local;
        vec3 y_local;

        double valor, axial, forca, fletor, cortante, coef;
        double sqrtCXCZ, comp1, comp2, c,alfa_aux;

        public void SetMatrizRotacao()
        {

            /* 
             *  Pg. 273 . Gere & Weaver   
             *  alfa = rotação da seção
            */
            
            SubMatrizRotacao = new double[4, 4];

            /*Pra compatiblizar com o livro, uso cz com as coordenadas y, e o cy com as coords. z*/

            cx = (((pFin.x - pIni.x)) / L);
            cz = ((((pFin.y*-1) - (pIni.y * -1))) / L);
            cy = ((((pFin.z*-1) - (pIni.z * -1))) / L);

            if (Geom.Iguais(cx,0, 0.001))
                cx =0;
            if (Geom.Iguais(cy,0, 0.001))
                cy =0;
            if (Geom.Iguais(cz, 0, 0.001))
                cz = 0;

            if (Geom.Iguais(Math.Abs(cy), 1, 0.00001)) // se é elemento vertical, trata de outra forma, conforme o livro de Gere & Weaver
            {
                cos_alpha = Math.Cos(alfa*-1 * Const.PIDiv180);
                sen_alpha = Math.Sin(alfa * -1 * Const.PIDiv180);

                if (alfa == 0 || Math.Abs(alfa) == 90)
                {
                    if ((pFin.z*-1) > (pIni.z * -1))
                      cy = 1;
                    else
                    if ((pFin.z * -1) < (pIni.z * -1))
                     cy = -1;
                }
                else
                {
                    if ((pFin.z * -1) > (pIni.z * -1))
                      cy = 1;
                    else
                    if ((pFin.z * -1) < (pIni.z * -1))
                      cy = -1;
                }

                SubMatrizRotacao[1, 1] = 0;
                SubMatrizRotacao[1, 2] = cy;
                SubMatrizRotacao[1, 3] = 0;

                SubMatrizRotacao[2, 1] = -cy * cos_alpha;
                SubMatrizRotacao[2, 2] = 0;
                SubMatrizRotacao[2, 3] = sen_alpha;

                SubMatrizRotacao[3, 1] = cy * sen_alpha;
                SubMatrizRotacao[3, 2] = 0;
                SubMatrizRotacao[3, 3] = cos_alpha;
            }
            else
            {
                cos_alpha = Math.Cos((alfa) * -1 * Const.PIDiv180);
                sen_alpha = Math.Sin((alfa) * -1 * Const.PIDiv180);

                SubMatrizRotacao[1, 1] = cx; 
                SubMatrizRotacao[1, 2] = cy; 
                SubMatrizRotacao[1, 3] = cz;

                sqrtCXCZ = Math.Sqrt(Math.Pow(cx, 2) + Math.Pow(cz, 2));

                SubMatrizRotacao[2, 1] = ((-cx * cy * cos_alpha) - (cz * sen_alpha)) / sqrtCXCZ;
                SubMatrizRotacao[2, 2] = sqrtCXCZ * cos_alpha;
                SubMatrizRotacao[2, 3] = ((-cy * cz * cos_alpha) + (cx * sen_alpha)) / sqrtCXCZ;

                SubMatrizRotacao[3, 1] = ((cx * cy * sen_alpha) - (cz * cos_alpha)) / sqrtCXCZ;
                SubMatrizRotacao[3, 2] = -(sqrtCXCZ) * sen_alpha;
                SubMatrizRotacao[3, 3] = (cy * cz * sen_alpha + cx * cos_alpha) / sqrtCXCZ;
            }
            
             MatrizRotacao = new double[13, 13];
             int li, co, inc = -4 ;
             for (k = 0; k < 4; k++)
             {
                 inc+=4;
                 for (i = 1; i <= 3; i++)
                   for (j = 1; j <= 3; j++)
                   {
                       li = i+inc-k;
                       co = j+inc-k;
                       MatrizRotacao[li, co] = SubMatrizRotacao[i, j];
                   }
             }
        }

        double xcl, ycl, zcl, el, cx, cy, cz, cxz, xps, xp,yp,zp;

        /* método que preenche o vetor glglobal, ou seja, cada posição até a posição 6 será preenchida com os 
           gl globais em função do noini e nofin */
        public void SetGlGlobal()
        {
            int jj = pIni.Numero, jk = pFin.Numero;

            for (int j = 1; j <= 6; j++)
               GlGlobal[j] = 6 * jj - (6 - j);

            for (int j = 1; j <= 6; j++)
               GlGlobal[j + 6] = 6 * jk - (6 - j);

            pIni.GlGlobal[1] = GlGlobal[1];
            pIni.GlGlobal[2] = GlGlobal[2];
            pIni.GlGlobal[3] = GlGlobal[3];
            pIni.GlGlobal[4] = GlGlobal[4];
            pIni.GlGlobal[5] = GlGlobal[5];
            pIni.GlGlobal[6] = GlGlobal[6];

            pFin.GlGlobal[1] = GlGlobal[7];
            pFin.GlGlobal[2] = GlGlobal[8];
            pFin.GlGlobal[3] = GlGlobal[9];
            pFin.GlGlobal[4] = GlGlobal[10];
            pFin.GlGlobal[5] = GlGlobal[11];
            pFin.GlGlobal[6] = GlGlobal[12];
        }
        double[] forcasLocais;

        double sigma_mx, sigma_my = 0;
        double Sigma(double iy, double iz, double area, double my, double mz, double fx, double xp, double yp)
        {
            sigma_mx = (my * yp) / iy;
            sigma_my = (mz * xp) / iz;

            return (fx / area) - sigma_mx - sigma_my;
        }


        public void CalculaTensoesNormais(int tipocarga, int caso, int comb)
        {
                secaoCopia_i = (TSecao)barraOriginal.Dados.secaoSemRotacao.Clone();
                secaoCopia_f = (TSecao)barraOriginal.Dados.secaoSemRotacao.Clone();

                int qtd_CoordsSecao = Dados.secao.poligonos.First(o=>o.externo).coords.Count();
                int polExterno = Dados.secao.poligonos.IndexOf(Dados.secao.poligonos.First( o => o.externo));

                tensoes_ini = new double[qtd_CoordsSecao - 1];
                tensoes_fin = new double[qtd_CoordsSecao - 1];

                double iy = Dados.secao.propriedades.inercia_flexao_y / 1e12;
                double iz = Dados.secao.propriedades.inercia_flexao_z / 1e12;

                double area = Dados.secao.propriedades.area / 1000000;

                double[] Esforcos = new double[13];

                if (tipocarga == 0 && casos_x_esforcos.Count > 0)
                    Esforcos = casos_x_esforcos[caso].Esforcos;
                else
                if (tipocarga == 1 && combinacoes_x_esforcos.Count > 0)
                    Esforcos = combinacoes_x_esforcos[comb].Esforcos;
                else
                    return;

                double my_i = Math.Abs(Esforcos[6]) * sinal_my_ini;
                double my_f = Math.Abs(Esforcos[12]) * sinal_my_fin;

                double mz_i = Math.Abs(Esforcos[5]) * sinal_mz_ini;
                double mz_f = Math.Abs(Esforcos[11]) * sinal_mz_fin;

                double fx_i = Math.Abs(Esforcos[1]) * sinal_fx_ini;
                double fx_f = Math.Abs(Esforcos[7]) * sinal_fx_fin;
                
                vec3 coord_real;
                cy = ((((pFin.z * -1) - (pIni.z * -1))) / comprimento);

                double sigma_i = 0, sigma_f = 0;

                for (int i = 0; i < qtd_CoordsSecao - 1; i++)
                {
                    if (!Geom.Iguais(barraOriginal.pIni.x, barraOriginal.pFin.x))
                    {
                      //  if (barraOriginal.pIni.x > barraOriginal.pFin.x)
                      //      coord_real = new vec3(secaoCopia_i.poligono.coords[i].X, -secaoCopia_i.poligono.coords[i].Y, 0);
                     //   else
                            coord_real = new vec3(secaoCopia_i.poligonos[polExterno].coords[i].X, secaoCopia_i.poligonos[polExterno].coords[i].Y, 0);
                    }
                    else
                    if (Geom.Iguais(Math.Abs(cy), 1, 0.00001))
                    {
                       // if ((pIni.z * -1) < (pFin.z * -1))
                        //    coord_real = new vec3(secaoCopia_i.poligono.coords[i].X, secaoCopia_i.poligono.coords[i].Y, 0);
                      //  else
                            coord_real = new vec3(secaoCopia_i.poligonos[polExterno].coords[i].X, secaoCopia_i.poligonos[polExterno].coords[i].Y, 0);
                    }
                    else
                        coord_real = new vec3(secaoCopia_i.poligonos[polExterno].coords[i].X, secaoCopia_i.poligonos[polExterno].coords[i].Y, 0);

                    //se tiver eixos principais desalinhado com eixos geometricos...exemplo cantoneira etc
                    coord_real = coord_real.Rotate(barraOriginal.Dados.secao.propriedades.anguloEixosPrincipais);

                    sigma_i = Sigma(iy, iz, area, my_i, mz_i, fx_i, coord_real.x * 1, coord_real.y * 1);
                    sigma_f = Sigma(iy, iz, area, my_f, mz_f, fx_f, coord_real.x * 1, coord_real.y * 1);
              
                    tensoes_ini[i] = sigma_i;
                    tensoes_fin[i] = sigma_f;
                }
        }

        public void CalcularTensoes(string tipo, int id)
        {
            if (tipo == Const.ID_TIPO_CASO)
            {
                double fator = 1;
                OrientaSecaoNoEspaco(ref fator, 0, id, 0);
                MY_Positivos_Negativos(0, id, 0);
                MZ_Positivos_Negativos(0, id, 0);
                FX_Positivos_Negativos(0, id, 0);

                CalculaTensoesNormais(0, id, 0);
                casos_x_tensoes.Add(new Tensoes_Barra((double[])tensoes_ini.Clone(), (double[])tensoes_fin.Clone(), id));
            }
            else
            if (tipo == Const.ID_TIPO_COMBINACAO)
            {
                double fator = 1;
                OrientaSecaoNoEspaco(ref fator, 1,0, id);
                MY_Positivos_Negativos(1,0, id);
                MZ_Positivos_Negativos(1,0, id);
                FX_Positivos_Negativos(1,0, id);

                CalculaTensoesNormais(1,0, id);
                combinacoes_x_tensoes.Add(new Tensoes_Barra((double[])tensoes_ini.Clone(), (double[])tensoes_fin.Clone(), id));
            }
        }
        double[,] T_EixosPrincipais_bloco ;
        double[,] T_EixosPrincipais;
        double[,] T_EixosPrincipais_Transposta;
        void CriarMatrizDeTransformacao()
        {
            T_EixosPrincipais_bloco = new double[4, 4];
            T_EixosPrincipais = new double[13, 13];
            T_EixosPrincipais_Transposta = new double[13, 13];
            M = new double[13, 13];

            double alfaPrincipal = barraOriginal.Dados.secao.propriedades.anguloEixosPrincipais;
            cos_alpha = Math.Cos(alfaPrincipal );
            sen_alpha = Math.Sin(alfaPrincipal );

            T_EixosPrincipais_bloco[1, 1] = 1;
            T_EixosPrincipais_bloco[1, 2] = 0;
            T_EixosPrincipais_bloco[1, 3] = 0;

            T_EixosPrincipais_bloco[2, 1] = 0;
            T_EixosPrincipais_bloco[2, 2] = cos_alpha;
            T_EixosPrincipais_bloco[2, 3] = sen_alpha;

            T_EixosPrincipais_bloco[3, 1] = 0;
            T_EixosPrincipais_bloco[3, 2] = -sen_alpha;
            T_EixosPrincipais_bloco[3, 3] = cos_alpha;

            int lin, col, incr = -4;
            for (k = 0; k < 4; k++)
            {
                incr += 4;
                for (i = 1; i <= 3; i++)
                    for (j = 1; j <= 3; j++)
                    {
                        lin = i + incr - k;
                        col = j + incr - k;
                        T_EixosPrincipais[lin, col] = T_EixosPrincipais_bloco[i, j];
                    }
            }
        }
        double[] EsforcosExpandidos; // esforcos no caso de barra semirigida
        void RetForcasMolas()
        {

        }

        void RecuperarDeslocamentos(ref Esforcos_Barra caso_ou_combinacao_atual)
        {
           /* for (i = 1; i <= 4; i++)
                for (j = 1; j <= 4; j++)
                {
                    KiiInv[i, j] *= -1;
                }*/
            double[,] A = new double[5,13];
                                                  //4x4      4x12
            TAlgebra.Multiplica_Matriz_Matriz(ref KiiInv, ref Kie,ref A, 4,4,12);

            double[] qi = new double[5];
            double[] qe = DeslocamentosLocais.ToArray();
            TAlgebra.Multiplica_Matriz_Vetor(ref A,ref qe,ref qi,4,12);

            for (int i = 1; i <= 4; i++)
                qi[i] *= -1;

            double[] q16 = new double[17];

            for (int i = 1; i <= 12; i++)
                q16[i] = qe[i];

            q16[13] = qi[1];
            q16[14] = qi[2];
            q16[15] = qi[3];
            q16[16] = qi[4];

            DeslocamentosLocaisBarraSemiRigida = new double[13];

            Array.Copy(DeslocamentosLocais, DeslocamentosLocaisBarraSemiRigida, 13);
            DeslocamentosLocaisBarraSemiRigida[5] = qi[1];
            DeslocamentosLocaisBarraSemiRigida[6] = qi[2];

            DeslocamentosLocaisBarraSemiRigida[11] = qi[3];
            DeslocamentosLocaisBarraSemiRigida[12] = qi[4];

            caso_ou_combinacao_atual.DeslocamentosLocaisBarraSemiRigida = new double[13];
            Array.Copy(DeslocamentosLocaisBarraSemiRigida, caso_ou_combinacao_atual.DeslocamentosLocaisBarraSemiRigida, 13);

            //forcas nas molas, se por ventura for necessario essa informacao para dimensionar uma ligacao semirigida
            caso_ou_combinacao_atual.MzMolaIni = KMz_Inicio * (q16[5] - q16[13]);

            caso_ou_combinacao_atual.MyMolaIni = KMy_Inicio * (q16[6] - q16[14]);

            caso_ou_combinacao_atual.MzMolaFim = KMz_Final * (q16[11] - q16[15]);

            caso_ou_combinacao_atual.MyMolaFim = KMy_Final * (q16[12] - q16[16]);

            /*Expandir Esforços*/
            EsforcosExpandidos = new double[17];
            TAlgebra.Multiplica_Matriz_Vetor(ref MatrizLocalExpandida_SemMolas, ref q16, ref EsforcosExpandidos, 16, 16);
        }

        void CondensarForcasLocais()
        {
            double[] A = new double[5];
            double[] B = new double[13];
            double[] fi = new double[5];
            double[] fc = new double[13];
            double[] fe = new double[13];
            double[] f16 = new double[17];
            
            // GDL que permanecem iguais
            f16[1] = forcasLocais[1];
            f16[2] = forcasLocais[2];
            f16[3] = forcasLocais[3];
            f16[4] = forcasLocais[4];

            f16[7] = forcasLocais[7];
            f16[8] = forcasLocais[8];
            f16[9] = forcasLocais[9];
            f16[10] = forcasLocais[10];

            // Os momentos da carga pertencem à BARRA
            f16[13] = forcasLocais[5];
            f16[14] = forcasLocais[6];
            f16[15] = forcasLocais[11];
            f16[16] = forcasLocais[12];

            // GDL do nó ficam inicialmente zerados
            f16[5] = 0;
            f16[6] = 0;
            f16[11] = 0;
            f16[12] = 0;

            fi[1] = f16[13];
            fi[2] = f16[14];
            fi[3] = f16[15];
            fi[4] = f16[16];

            for (int i = 1; i < 13; i++)
              fe[i] = f16[i];
        
            //    Array.Copy(f16, fe, 13);

            //4x4        1x4
            TAlgebra.Multiplica_Matriz_Vetor(ref KiiInv, ref fi, ref A, 4,4);
                                           // Kei(12x4)  A(4x1)  B(12x1)
            TAlgebra.Multiplica_Matriz_Vetor(ref Kei, ref A, ref B, 12, 4);
            for (int i = 1; i < 13; i++)
                fc[i] = fe[i] - B[i];
           
            //fc = fe - Kei * KiiInv * fi
            Array.Copy(fc, forcasLocais, 13);
        }

        double[] DeslocamentosLocaisBarraSemiRigida ;
        public void CalcularEsforcos(string tipo, int id)
        {
            try
            {
                Esforcos_Barra esforcos_caso_ou_combinacao_barra;
                Deslocamentos_Nos deslocamento_ini, deslocamento_fin;
                forcasLocais = new double[13];
                double[] forcasLocaisPrincipais = new double[13];
                double[] DeslocamentosGlobaisPrincipais = new double[13];

                //combinacoes_x_esforcos[comb].DeslocamentosLocais;
                if (tipo == Const.ID_TIPO_CASO)
                {
                    if (temOffset)
                        forcasLocais = casos_x_forcasLocais[id].forcasLocais_sem_offset;
                    else
                        forcasLocais = casos_x_forcasLocais[id].forcasLocais;

                    casos_x_esforcos.Add(new Esforcos_Barra(id));
                    int cc = casos_x_esforcos.Count;

                    esforcos_caso_ou_combinacao_barra = casos_x_esforcos[cc - 1];
                    deslocamento_ini = pIni.casos_x_deslocamentos[cc - 1];
                    deslocamento_fin = pFin.casos_x_deslocamentos[cc - 1];
                }
                else //combinacao
                {
                    if (temOffset)
                        forcasLocais = combinacoes_x_forcasLocais[id].forcasLocais_sem_offset;
                    else
                        forcasLocais = combinacoes_x_forcasLocais[id].forcasLocais;

                    combinacoes_x_esforcos.Add(new Esforcos_Barra(id));
                    int cc = combinacoes_x_esforcos.Count;

                    esforcos_caso_ou_combinacao_barra = combinacoes_x_esforcos[cc - 1];
                    deslocamento_ini = pIni.combinacoes_x_deslocamentos[cc - 1];
                    deslocamento_fin = pFin.combinacoes_x_deslocamentos[cc - 1];
                }

                esforcos_caso_ou_combinacao_barra.DeslocamentosGlobais[1] =  deslocamento_ini.DeslocamentoGlobal[1];
                esforcos_caso_ou_combinacao_barra.DeslocamentosGlobais[2] =  deslocamento_ini.DeslocamentoGlobal[2];
                esforcos_caso_ou_combinacao_barra.DeslocamentosGlobais[3] =  deslocamento_ini.DeslocamentoGlobal[3];
                esforcos_caso_ou_combinacao_barra.DeslocamentosGlobais[4] =  deslocamento_ini.DeslocamentoGlobal[4];
                esforcos_caso_ou_combinacao_barra.DeslocamentosGlobais[5] =  deslocamento_ini.DeslocamentoGlobal[5];
                esforcos_caso_ou_combinacao_barra.DeslocamentosGlobais[6] =  deslocamento_ini.DeslocamentoGlobal[6];

                esforcos_caso_ou_combinacao_barra.DeslocamentosGlobais[7] =  deslocamento_fin.DeslocamentoGlobal[1];
                esforcos_caso_ou_combinacao_barra.DeslocamentosGlobais[8] =  deslocamento_fin.DeslocamentoGlobal[2];
                esforcos_caso_ou_combinacao_barra.DeslocamentosGlobais[9] =  deslocamento_fin.DeslocamentoGlobal[3];
                esforcos_caso_ou_combinacao_barra.DeslocamentosGlobais[10] = deslocamento_fin.DeslocamentoGlobal[4];
                esforcos_caso_ou_combinacao_barra.DeslocamentosGlobais[11] = deslocamento_fin.DeslocamentoGlobal[5];
                esforcos_caso_ou_combinacao_barra.DeslocamentosGlobais[12] = deslocamento_fin.DeslocamentoGlobal[6];

                DeslocamentosGlobais = esforcos_caso_ou_combinacao_barra.DeslocamentosGlobais.ToArray();

                if (articulacao_my_ini || articulacao_my_fin || articulacao_mz_ini || articulacao_mz_fin)
                    CondensarForcasLocais();

                Array.Copy(forcasLocais, forcasLocaisPrincipais, forcasLocais.Length);

                if (temOffset)
                {
                    if (!Geom.Iguais(barraOriginal.Dados.secao.propriedades.anguloEixosPrincipais, 0))
                    {
                        double[] Esforcos_Temp = new double[13];
                        double[] f2 = new double[13];
                        CriarMatrizDeTransformacao();

                        TAlgebra.Multiplica_Matriz_Vetor(ref T_EixosPrincipais, ref forcasLocais, ref forcasLocaisPrincipais, 12, 12);
                        
                        TAlgebra.Multiplica_Matriz_Vetor(ref MatrizRotacao, ref DeslocamentosGlobais, ref DeslocamentosLocais, 12, 12);
                        TAlgebra.Multiplica_Matriz_Vetor(ref MatrizOffset, ref DeslocamentosLocais, ref f2, 12, 12);
                        TAlgebra.Multiplica_Matriz_Vetor(ref MatrizLocal_sem_offset, ref f2, ref Esforcos_Temp, 12, 12);

                        TAlgebra.Multiplica_Matriz_Vetor(ref T_EixosPrincipais, ref Esforcos_Temp, ref Esforcos, 12, 12);
                    }
                    else
                    {
                        double[] f2 = new double[13];
                        TAlgebra.Multiplica_Matriz_Vetor(ref MatrizRotacao, ref DeslocamentosGlobais, ref DeslocamentosLocais, 12, 12);

                        TAlgebra.Multiplica_Matriz_Vetor(ref MatrizOffset, ref DeslocamentosLocais, ref f2, 12, 12);
                        TAlgebra.Multiplica_Matriz_Vetor(ref MatrizLocal_sem_offset, ref f2, ref Esforcos, 12, 12);
                    }
                }
                else
                { 
                    if (!Geom.Iguais(barraOriginal.Dados.secao.propriedades.anguloEixosPrincipais, 0))
                    {
                        double[] Esforcos_Temp = new double[13];
                        CriarMatrizDeTransformacao();
                        forcasLocaisPrincipais = new double[13];
                        TAlgebra.Multiplica_Matriz_Vetor(ref T_EixosPrincipais, ref forcasLocais, ref forcasLocaisPrincipais, 12, 12);

                        TAlgebra.Multiplica_Matriz_Vetor(ref MatrizRotacao, ref DeslocamentosGlobais, ref DeslocamentosLocais, 12, 12);
                        TAlgebra.Multiplica_Matriz_Vetor(ref MatrizLocal, ref DeslocamentosLocais, ref Esforcos_Temp, 12, 12);
                        TAlgebra.Multiplica_Matriz_Vetor(ref T_EixosPrincipais, ref Esforcos_Temp, ref Esforcos, 12, 12);
                    }
                    else
                    {
                        TAlgebra.Multiplica_Matriz_Vetor(ref MatrizRotacao, ref DeslocamentosGlobais, ref DeslocamentosLocais, 12, 12);
                        TAlgebra.Multiplica_Matriz_Vetor(ref MatrizLocal, ref DeslocamentosLocais, ref Esforcos, 12, 12);

                       /* if (articulacao_my_ini || articulacao_my_fin || articulacao_mz_ini || articulacao_mz_fin)
                        {
                            RecuperarDeslocamentos();

                            esforcos_barra.DeslocamentosLocaisBarraSemiRigida = new double[13];
                            Array.Copy(DeslocamentosLocaisBarraSemiRigida, esforcos_barra.DeslocamentosLocaisBarraSemiRigida, 13);
                            Esforcos[5]  = EsforcosExpandidos[13];
                            Esforcos[6]  = EsforcosExpandidos[14];
                            Esforcos[11] = EsforcosExpandidos[15];
                            Esforcos[12] = EsforcosExpandidos[16];
                        }
                        */
                    }


               //     for (int jj = 0; jj < 13; jj++)
             //           esforcos_barra.Esforcos[jj] = (Esforcos[jj] - forcasLocais[jj]);
                }

                if (articulacao_my_ini || articulacao_my_fin || articulacao_mz_ini || articulacao_mz_fin)
                {
                    RecuperarDeslocamentos(ref esforcos_caso_ou_combinacao_barra);

                    if (!Geom.Iguais(barraOriginal.Dados.secao.propriedades.anguloEixosPrincipais, 0))
                    {
                        // EsforcosExpandidos está nos eixos geométricos
                        double[] EsforcosExpandidosGeom = new double[13];

                        for (int i = 1; i <= 12; i++)
                            EsforcosExpandidosGeom[i] = EsforcosExpandidos[i];

                        EsforcosExpandidosGeom[5] = EsforcosExpandidos[13];
                        EsforcosExpandidosGeom[6] = EsforcosExpandidos[14];
                        EsforcosExpandidosGeom[11] = EsforcosExpandidos[15];
                        EsforcosExpandidosGeom[12] = EsforcosExpandidos[16];

                        double[] EsforcosExpandidosPrincipais = new double[13];
                        TAlgebra.Multiplica_Matriz_Vetor(
                            ref T_EixosPrincipais,
                            ref EsforcosExpandidosGeom,
                            ref EsforcosExpandidosPrincipais,
                            12, 12);

                        Esforcos[5] = EsforcosExpandidosPrincipais[5];
                        Esforcos[6] = EsforcosExpandidosPrincipais[6];
                        Esforcos[11] = EsforcosExpandidosPrincipais[11];
                        Esforcos[12] = EsforcosExpandidosPrincipais[12];
                    }
                    else
                    {
                        Esforcos[5] = EsforcosExpandidos[13];
                        Esforcos[6] = EsforcosExpandidos[14];
                        Esforcos[11] = EsforcosExpandidos[15];
                        Esforcos[12] = EsforcosExpandidos[16];
                    }
                    /*Esforcos[2] = EsforcosExpandidosPrincipais[2];
                    Esforcos[3] = EsforcosExpandidosPrincipais[3];
                    Esforcos[8] = EsforcosExpandidosPrincipais[8];
                    Esforcos[9] = EsforcosExpandidosPrincipais[9];*/

                    /*  Esforcos[5]  = EsforcosExpandidos[13];
                      Esforcos[6]  = EsforcosExpandidos[14];
                      Esforcos[11] = EsforcosExpandidos[15];
                      Esforcos[12] = EsforcosExpandidos[16];*/
                }

                for (int jj = 0; jj < 13; jj++)
                  esforcos_caso_ou_combinacao_barra.Esforcos[jj] = (Esforcos[jj] - forcasLocaisPrincipais[jj]);
               
                Array.Copy(DeslocamentosLocais, esforcos_caso_ou_combinacao_barra.DeslocamentosLocais, 13);

                if (Geom.Iguais(esforcos_caso_ou_combinacao_barra.DeslocamentosLocais[1], 0,0.00001)) esforcos_caso_ou_combinacao_barra.DeslocamentosLocais[1] = 0;
                if (Geom.Iguais(esforcos_caso_ou_combinacao_barra.DeslocamentosLocais[2], 0,0.00001)) esforcos_caso_ou_combinacao_barra.DeslocamentosLocais[2] = 0;
                if (Geom.Iguais(esforcos_caso_ou_combinacao_barra.DeslocamentosLocais[3], 0,0.00001)) esforcos_caso_ou_combinacao_barra.DeslocamentosLocais[3] = 0;
                if (Geom.Iguais(esforcos_caso_ou_combinacao_barra.DeslocamentosLocais[4], 0,0.00001)) esforcos_caso_ou_combinacao_barra.DeslocamentosLocais[4] = 0;
                if (Geom.Iguais(esforcos_caso_ou_combinacao_barra.DeslocamentosLocais[5], 0,0.00001)) esforcos_caso_ou_combinacao_barra.DeslocamentosLocais[5] = 0;
                if (Geom.Iguais(esforcos_caso_ou_combinacao_barra.DeslocamentosLocais[6], 0, 0.00001))esforcos_caso_ou_combinacao_barra.DeslocamentosLocais[6] = 0;
                                               
                if (Geom.Iguais(esforcos_caso_ou_combinacao_barra.DeslocamentosLocais[7], 0,0.00001)) esforcos_caso_ou_combinacao_barra.DeslocamentosLocais[7] = 0;
                if (Geom.Iguais(esforcos_caso_ou_combinacao_barra.DeslocamentosLocais[8], 0,0.00001)) esforcos_caso_ou_combinacao_barra.DeslocamentosLocais[8] = 0;
                if (Geom.Iguais(esforcos_caso_ou_combinacao_barra.DeslocamentosLocais[9], 0,0.00001)) esforcos_caso_ou_combinacao_barra.DeslocamentosLocais[9] = 0;
                if (Geom.Iguais(esforcos_caso_ou_combinacao_barra.DeslocamentosLocais[10], 0,0.00001)) esforcos_caso_ou_combinacao_barra.DeslocamentosLocais[10] = 0;
                if (Geom.Iguais(esforcos_caso_ou_combinacao_barra.DeslocamentosLocais[11], 0,0.00001)) esforcos_caso_ou_combinacao_barra.DeslocamentosLocais[11] = 0;
                if (Geom.Iguais(esforcos_caso_ou_combinacao_barra.DeslocamentosLocais[12], 0, 0.00001))esforcos_caso_ou_combinacao_barra.DeslocamentosLocais[12] = 0;

                deslocamento_ini.U_Total = Math.Sqrt(Math.Pow(deslocamento_ini.DeslocamentoGlobal[1], 2) + Math.Pow(deslocamento_ini.DeslocamentoGlobal[2], 2) + Math.Pow(deslocamento_ini.DeslocamentoGlobal[3], 2));
                deslocamento_fin.U_Total = Math.Sqrt(Math.Pow(deslocamento_fin.DeslocamentoGlobal[1], 2) + Math.Pow(deslocamento_fin.DeslocamentoGlobal[2], 2) + Math.Pow(deslocamento_fin.DeslocamentoGlobal[3], 2));
                ///////////////////////

                if (tipo == Const.ID_TIPO_CASO)
                {
                    pIni.casos_x_deslocamentos[pIni.casos_x_deslocamentos.Count - 1] = deslocamento_ini;
                    pFin.casos_x_deslocamentos[pFin.casos_x_deslocamentos.Count - 1] = deslocamento_fin;
                }
                else
                {
                    pIni.combinacoes_x_deslocamentos[pIni.combinacoes_x_deslocamentos.Count - 1] = deslocamento_ini;
                    pFin.combinacoes_x_deslocamentos[pFin.combinacoes_x_deslocamentos.Count - 1] = deslocamento_fin;
                }
            }
            catch(Exception ee)
            {
                MessageBox.Show(ee.Message);
            }
        }

        int IndiceRGBi, IndiceRGBf;
        int ri, gi, bi, rf, gf, bf;
        double coordx_f, coordy_f, coordz_f, coordx_i, coordy_i, coordz_i;
        double newx1, newy1, newz1;
        double[] px_x = new double[1];
        double[] px_y = new double[1];

        bool InicialNegativo, InicialPositivo;
        double inx = 0, iny = 0;

        public bool DoisPoligonosNaBarra_Viga, DoisPoligonosNaBarra_Pilar_Fletor_Y, DoisPoligonosNaBarra_Pilar_Fletor_Z;

        vec3[] EsforcoCoords = new vec3[4];
        vec3[] EsforcoColorido = new vec3[6];
        [NonSerialized]
        double[,] RGB_Negativos, RGB_Positivos;
        vec3 pRotacao, pInicial, pFinal, pFinal_Aux;
        int ll;




        public double fletorZ_inicial_pilar, fletorZ_final_pilar, valor_y_inicial, valor_y_final;
        
        public double fletorY_inicial_pilar, fletorY_final_pilar, valor_x_inicial, valor_x_final;

        public double axial_inicial_pilar, axial_final_pilar;

        public double torcor_inicial_viga, torcor_final_viga;
  
        public double fletorY_inicial_viga, fletorY_final_viga, valor_z_inicial, valor_z_final;

        public double cortanteY_inicial_viga, cortanteY_final_viga;
 
        public List<vec3> Poligono1_FletorY_Viga, Poligono2_FletorY_Viga;
        public List<vec3> Poligono1_FletorY_Pilar, Poligono2_FletorY_Pilar;
        public List<vec3> Poligono1_FletorZ_Pilar, Poligono2_FletorZ_Pilar;
        public List<vec3> Poligono1_Axial_Pilar;
        public List<vec3> Poligono1_CortanteY_Viga, Poligono1_CortanteY_Pilar;
        public List<vec3> Poligono1_Torcor_Viga, Poligono1_Torcor_Pilar;

        double Altura;
        string cargatxt;
        int w; 
    }
}
