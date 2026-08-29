using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using OpenTK;
using OpenTK.Input;
using OpenTK.Graphics.OpenGL;
using OpenTK.Platform;
using System.Windows.Forms;

namespace PG
{
    public class TCelulaLinha
    {
        public List<TLinha> barras;
        public int max_barra;
        int i, j, k;
        public TCelulaLinha()
        {
            barras = new List<TLinha>();
        }
    }

    public class TCelulaGrelha
    {
        public List<TBarraGrelha> barras;
        public int max_barra;
        int i,j,k;
        public TCelulaGrelha()
        {
            barras = new List<TBarraGrelha>();
        }
        
        bool inside = false;
        double x_intersec;
        public bool PontoEmPoligono(ref double x, ref double y)
        {
            inside = false;
            for (i = 0; i < barras.Count; i++)
            {
                if (Geom.Iguais(barras[i].pFin.y, y)) continue;
                if (Geom.Iguais(barras[i].pIni.y, y)) continue;

                if (((barras[i].pIni.y > y) && (barras[i].pFin.y < y)) ||
                    ((barras[i].pIni.y < y) && (barras[i].pFin.y > y)))
                {
                    x_intersec = barras[i].pIni.x + (y - barras[i].pIni.y) * (barras[i].pFin.x - barras[i].pIni.x) / (barras[i].pFin.y - barras[i].pIni.y);
                    
                    if (!Geom.Iguais(x_intersec, x))
                        if (x_intersec > x)
                          inside = !inside;
                };
            };

            return inside;
        }
    }

    [Serializable]
    public class TBarraGrelha : TObjetoDesenho
    {
        #region Variáveis
        int i, j;
        public List<int> CelulasIncidentes;

        public CoordenadaD pontoAreaInf1, pontoAreaInf2;
        public TPoligono areaInf1, areaInf2;
        public vec3 coordIsoDeslocamento;
        public bool IncideEmViga;
        public TNoGrelha pIni;
		public TNoGrelha pFin;

        public int NEE; // número externo do elemento. Ordem em que foi inserido na geração da malha

        public int[] GlGlobal; /*vetor que retorna o gl global em função do gl local*/

        private double cx, cy;

        public double comprimento,
                      CargaDistribuida,
                      E, I, J, G, L,
                      angulo,
                      anguloGlobal,
                      areaSecao;
        [NonSerialized]
        public TPavimento Pavimento;

        public double CargaPontual,
                      posX_cargaPontual, posY_cargaPontual;

        public bool barraViga, barraLaje, barraRigida,
                     barra90, barraVertical, barraHorizontal,
                     barraObliqua,
                     direcaoX, direcaoY,
                     selecaoPorProximidade, selecaoPorPontoMedio, contornoLajeLinhaEixoViga, aberturaLaje,
                     barraErro;

        public int  numViga, numLaje
		           ,indiceNoIni, indiceNoFin
                   ,codBarra
                   ,CodigoObjetosTela;

        [NonSerialized]
        public TLaje Laje;

        [NonSerialized]
        public TTrechoViga TrechoViga;
        [NonSerialized]
        public TPilar Pilar; // se for barra rígida, aqui se guarda o pilar a qual ela pertence

        [NonSerialized]
        public double[,] RGB_Deslocamentos;

        [NonSerialized]
        public double[,] RGB_FletoresNegativos;
        [NonSerialized]
        public double[,] RGB_FletoresPositivos;

        [NonSerialized]
        public double[,] RGB_TorcoresNegativos;
        [NonSerialized]
        public double[,] RGB_TorcoresPositivos;

        [NonSerialized]
        public double[,] RGB_CortantesNegativos;
        [NonSerialized]
        public double[,] RGB_CortantesPositivos;


        public double[,] MatrizLocal,
                         MatrizGlobal, 
                         MatrizRotacao,
                         MatRotacaoTransposta, 
                         M;

        double[] DeslocamentosLocais;
        public double[] DeslocamentosGlobais;
        public double[] Esforcos;
        
        public vec3[] CoordsFletor, CoordsTorcor, CoordsCortante;
        private vec3[] FletorColorido, CortanteColorido, TorcorColorido;
      ///  TPoligono[] Fletor, Torcor, Cortante;
        
        double[] coordLocal;
        double[] coordGlobal;
        
        bool proxima;

        int iFletor   = 0;
        int iTorcor   = 1;
        int iCortante = 2;

        #endregion

     //   public TBarraGrelha(CoordenadaD[] Coords, bool catalogada = false) : base(Coords, catalogada) { }
        
        public TBarraGrelha(): base(){}
        public TBarraGrelha(TNoGrelha pInicial, TNoGrelha pFinal, TTrechoViga trecho,
                      float cargaDistribuida, 
                      int numViga, int numLaje,
                      int indiceNoIni, int indiceNoFin, int CodigoObjetosTela, double[,] matrizBarra,
                      bool barraViga = false, 
                      bool barraLaje = false,
                      bool direcaoX = false, 
                      bool direcaoY = false,
                      int NEE = -1,
                      bool barraObliqua = false)   
		 {
            this.Visivel           = true; 
            this.NEE = NEE - 1;
            this.pIni              = pInicial;
			this.pFin              = pFinal;
            this.L                 = comprimento;
            this.numViga           = numViga;
			this.numLaje           = numLaje;
			this.indiceNoIni       = indiceNoIni;
			this.indiceNoFin       = indiceNoFin;
            this.barraViga         = barraViga;
            this.barraLaje         = barraLaje;
            this.direcaoX = direcaoX;
            this.direcaoY = direcaoY;
            this.CargaDistribuida             = cargaDistribuida;

            this.TrechoViga        = trecho;
            this.CodigoObjetosTela = CodigoObjetosTela;
            this.GlGlobal             = new int[7];
            this.DeslocamentosLocais  = new double[7];
            this.DeslocamentosGlobais = new double[7];
            this.Esforcos             = new double[7];

            base.Tipo                 = Const.ID_BARRAGRELHA;

            coordLocal  = new double[7];
            coordGlobal = new double[7];
            
            coordLocal[1] = pIni.x;
            coordLocal[2] = pIni.y;
            coordLocal[3] = 0;
            
            coordLocal[4] = pFin.x;
            coordLocal[5] = pFin.y;
            coordLocal[6] = 0;
		 }

         [NonSerialized]
         public System.Drawing.Pen mPen = new System.Drawing.Pen(System.Drawing.Color.White);

         int lin, col, gl;
         public bool Intersec(Linha outra, ref double x, ref double y)
         {
             return (Geom.calcIntersecEQU_RETA(this.pIni.x, this.pIni.y, this.pFin.x, this.pFin.y,
                                                         outra.pIni.x, outra.pIni.y, outra.pFin.x, outra.pFin.y, ref x, ref y));

         }

         public bool Intersec(double x1, double y1, double x2, double y2, ref double x, ref double y)
         {
             return (Geom.calcIntersecEQU_RETA(this.pIni.x, this.pIni.y, this.pFin.x, this.pFin.y,
                                                         x1, y1, x2, y2, ref x, ref y));
         }
        
         public void PreencheAcoesEngPerf(ref double[] ae)
         {
             double g;
             for (lin = 1; lin <= 6; lin++)
             {
                 gl = GlGlobal[lin]; 

                 for (col = 1; col <= 6; col++)
                 {           
                     forca = AcoesEngPerfeitoCargaDistribuida(col);
                   //  if (col == 3 || col == 6)
                   //    coef = 1;
                 //    else
                     coef  = MatRotacaoTransposta[lin, col];
                    
                     g      = ae[gl];
                     ae[gl] = g + (coef * forca);
                 }
             }

          /*   for (lin = 1; lin <= 6; lin++)
             {
                 gl = GlGlobal[lin];

                 for (col = 1; col <= 6; col++)
                 {
                     forca = AcoesEngPerfeitoCargaPontual(col);
                     //  if (col == 3 || col == 6)
                     //    coef = 1;
                     //    else
                     coef = MatRotacaoTransposta[lin, col];

                     g = ae[gl];
                     ae[gl] = g + (coef * forca);
                 }
             }*/
         }

         double valor, axial,forca, fletor, cortante, coef;
         double AcoesEngPerfeitoCargaDistribuida(int indice)
         {
             valor = 0;
             switch (indice)
             {
                 case 1: valor = 0; break; // axial
                 case 2: valor = CargaDistribuida * Math.Pow(L, 2) / 12; break; // fletor
                 case 3: valor = CargaDistribuida * L / 2; break; // cortante

                 case 4: valor = 0; break; // axial
                 case 5: valor = -CargaDistribuida * Math.Pow(L, 2) / 12; break; // fletor
                 case 6: valor = CargaDistribuida * L / 2; break; // cortante}
             }

             return valor;
         }
         double AcoesEngPerfeitoCargaPontual(int indice)
         {
             valor = 0;
             switch (indice)
             {
                 case 1: valor = 0; break; // axial
                 case 2: valor = (CargaPontual * posX_cargaPontual * (Math.Pow(posY_cargaPontual,2))) / Math.Pow(L, 2); break; // fletor
                 case 3: valor = CargaDistribuida * L / 2; break; // cortante

                 case 4: valor = 0; break; // axial
                 case 5: valor = -(CargaPontual * posY_cargaPontual * (Math.Pow(posX_cargaPontual, 2))) / Math.Pow(L, 2); break; // fletor
                 case 6: valor = CargaDistribuida * L / 2; break; // cortante}
             }

             return valor;
         }
    
         public void SetMatrizGlobal()
         {
             M = new double[7, 7];
             MatRotacaoTransposta = new double[7, 7];
             MatrizGlobal = new double[7, 7];

             //[K] = [R]t.[k]e.[R]
             
             TAlgebra.Transposta(ref MatrizRotacao, ref MatRotacaoTransposta, 6, 6);                          // [R] -> [R]t                        
             TAlgebra.Multiplica_Matriz_Matriz(ref MatRotacaoTransposta, ref MatrizLocal, ref M, 6, 6);    // [M]  = [R]t.[K]e
             TAlgebra.Multiplica_Matriz_Matriz(ref M, ref MatrizRotacao, ref MatrizGlobal, 6, 6);          // [k]g = [M].[R]
         }

         double r1, r2, r3, r4, r5;
         public void SetMatrizLocal()
         {
            r1 = (G*J)/L;
            r2 = (4*E*I)/L;
            r3 = (6*E*I)/(L*L);
            r4 = (2*E*I)/L;
            r5 = (12*E*I)/(L*L*L);

            MatrizLocal = new double[7, 7];

            MatrizLocal[1,1]= r1;
            MatrizLocal[1,4]=-r1;
            MatrizLocal[2,2]= r2;
            MatrizLocal[2,3]=-r3;
            MatrizLocal[2,5]= r4;
            MatrizLocal[2,6]= r3;
                    
            MatrizLocal[3,3]= r5;
            MatrizLocal[3,5]=-r3;
            MatrizLocal[3,6]=-r5;
            MatrizLocal[4,4]= r1;
            MatrizLocal[5,5]= r2;
            MatrizLocal[5,6]= r3;
            MatrizLocal[6,6]= r5;

            MatrizLocal[1,2]=0;
            MatrizLocal[1,3]=0;
            MatrizLocal[1,5]=0;
            MatrizLocal[1,6]=0;
            MatrizLocal[2,4]=0;
            MatrizLocal[3,4]=0;
            MatrizLocal[4,5]=0;
            MatrizLocal[4,6]=0;

            //preenche lado simétrico
            for (i = 1; i<=6; i++)
              for (j = i; j<=6; j++)
                MatrizLocal[j,i] = MatrizLocal[i,j];
         }

         public void SetMatrizRotacao()
         {

             /*  Pg. 243 . Gere & Weaver 
                 Utiliza a mesma matriz de rotação de pórtico plano*/

            MatrizRotacao = new double[7, 7];

            cx = Math.Cos(anguloGlobal * Const.PIDiv180);
            cy = Math.Sin(anguloGlobal * Const.PIDiv180);

            for (i = 1; i<=6; i++)
              for (j = 1; j<=6; j++)
                MatrizRotacao[i,j] = 0;
           
            MatrizRotacao[1,1]= cx;
            MatrizRotacao[1,2]= cy;
            MatrizRotacao[2,1]=-cy;
            MatrizRotacao[2,2]= cx;
            MatrizRotacao[3,3]= 1;

            for (i = 4; i<=6; i++)
              for (j = 4; j<=6; j++)
                MatrizRotacao[i, j] = MatrizRotacao[i - 3, j - 3];
         }

         public void SetAnguloGlobal()
         {
               anguloGlobal = pIni.getAngleTo(pFin) / Const.PIDiv180;//passa para graus
         }

         /* método que preenche o vetor glglobal, ou seja, cada posição até a posição 6 será preenchida com os 
            gl globais em função do noini e nofin */
         public void SetGlGlobal()
         {
             int jj = pIni.Numero, jk = pFin.Numero;

             for (int j = 1; j <= 3; j++)
                 GlGlobal[j] = 3 * jj - (3 - j);

             for (int j = 1; j <= 3; j++)
                 GlGlobal[j + 3] = 3 * jk - (3 - j);

             pIni.GlGlobal[1] = GlGlobal[1];
             pIni.GlGlobal[2] = GlGlobal[2];
             pIni.GlGlobal[3] = GlGlobal[3];

             pFin.GlGlobal[1] = GlGlobal[4];
             pFin.GlGlobal[2] = GlGlobal[5];
             pFin.GlGlobal[3] = GlGlobal[6];
         }


         public bool DoisPoligonosNaBarra;

         vec3[] EsforcoCoords   = new vec3[4];
         vec3[] EsforcoColorido = new vec3[6];
         private void CriaDiagramas()
         {

             /* PS: Na reordenação nodal, o ponto inicial fica sempre na esquerda ou embaixo, no caso de barra vertical*/

             #region Cria as listas

             FletorColorido    = new vec3[6];
             FletorColorido[0] = new vec3(0);
             FletorColorido[1] = new vec3(0);
             FletorColorido[2] = new vec3(0);
             FletorColorido[3] = new vec3(0);
             FletorColorido[4] = new vec3(0);
             FletorColorido[5] = new vec3(0);

             CoordsFletor = new vec3[4];

             TorcorColorido    = new vec3[6];
             TorcorColorido[0] = new vec3(0);
             TorcorColorido[1] = new vec3(0);
             TorcorColorido[2] = new vec3(0);
             TorcorColorido[3] = new vec3(0);
             TorcorColorido[4] = new vec3(0);
             TorcorColorido[5] = new vec3(0);

             CoordsTorcor = new vec3[4];

             CortanteColorido    = new vec3[6];
             CortanteColorido[0] = new vec3(0);
             CortanteColorido[1] = new vec3(0);
             CortanteColorido[2] = new vec3(0);
             CortanteColorido[3] = new vec3(0);
             CortanteColorido[4] = new vec3(0);
             CortanteColorido[5] = new vec3(0);

             CoordsCortante = new vec3[4];

             /*Esforços-  no máximo 2 polígonos por barra*/

             CoordsFletor[0]   = new vec3(0);CoordsFletor[1]    = new vec3(0); CoordsFletor[2]   = new vec3(0); CoordsFletor[3]   = new vec3(0);
             CoordsTorcor[0]   = new vec3(0); CoordsTorcor[1]   = new vec3(0); CoordsTorcor[2]   = new vec3(0); CoordsTorcor[3]   = new vec3(0);
             CoordsCortante[0] = new vec3(0); CoordsCortante[1] = new vec3(0); CoordsCortante[2] = new vec3(0); CoordsCortante[3] = new vec3(0);

             #endregion

             //tem q rodar as coords da barra para coord globais
             TAlgebra.Multiplica_Matriz_Vetor(ref MatrizRotacao, ref coordLocal, ref coordGlobal, 6, 6);

             for (int iEsforco = iFletor; iEsforco < iCortante+1; iEsforco++)
             {
                 if (iEsforco == iFletor)
                 {
                     EsforcoColorido = FletorColorido;
                     EsforcoCoords   = CoordsFletor;
                 }
                 if (iEsforco == iTorcor)
                 {
                     EsforcoColorido = TorcorColorido;
                     EsforcoCoords   = CoordsTorcor;
                 }
                 if (iEsforco == iCortante)
                 {
                     EsforcoColorido = CortanteColorido;
                     EsforcoCoords   = CoordsCortante;
                 }

                 EsforcoCoords[0].x = pIni.x;
                 EsforcoCoords[0].y = pIni.y;
                 EsforcoCoords[0].z = 0;

                 EsforcoCoords[1].x = pIni.x;
                 EsforcoCoords[1].y = pIni.y;
              
                 if (iEsforco == iFletor)
                   EsforcoCoords[1].z = Esforcos[2] * -1;
                 else
                 if (iEsforco == iCortante)
                   EsforcoCoords[1].z = Esforcos[3]*-1;
                 else
                 if (iEsforco == iTorcor)
                   EsforcoCoords[1].z = Esforcos[1];

                 EsforcoCoords[2].x = pFin.x;
                 EsforcoCoords[2].y = pFin.y;
                 
                 if (iEsforco == iFletor)
                   EsforcoCoords[2].z = Esforcos[5];
                 else
                 if (iEsforco == iCortante)
                   EsforcoCoords[2].z = Esforcos[6];
                 else
                 if (iEsforco == iTorcor)
                   EsforcoCoords[2].z = Esforcos[4]*-1;

                 double inx = 0, iny = 0;
                
              /*   if ((iEsforco == iFletor) && (Geom.calcIntersecEQU_RETA(Math.Abs(coordGlobal[1]), Math.Abs(coordGlobal[2]),
                                               Math.Abs(coordGlobal[4]), Math.Abs(coordGlobal[5]),
                                               Math.Abs(coordGlobal[1]), Math.Abs(coordGlobal[2]) + EsforcoCoords[1].z,
                                               Math.Abs(coordGlobal[4]), Math.Abs(coordGlobal[5]) + EsforcoCoords[2].z,
                                               ref inx, ref iny)))*/
              //   if (1==1)
                 //{
                     vec3 l1_p1 = new vec3(pIni.x, pIni.y, 0);
                     vec3 l1_p2 = new vec3(pFin.x, pFin.y, 0);
                     vec3 l2_p1 = new vec3(pIni.x, pIni.y, EsforcoCoords[1].z);
                     vec3 l2_p2 = new vec3(pFin.x, pFin.y, EsforcoCoords[2].z);
                     vec3 ip = new vec3(0);
                     double tt = 0, uu = 0;
                     if (Geom.Intersec3D(ref l1_p1, ref l1_p2, ref l2_p1, ref l2_p2, ref ip, ref tt, ref uu) != 10)
                     {
                         DoisPoligonosNaBarra = true;

                         if (EsforcoCoords[1].z > 0)
                         {
                             // primeiro triangulo
                             EsforcoColorido[0].x = pIni.x;
                             EsforcoColorido[0].y = pIni.y;
                             EsforcoColorido[0].z = 0;

                             EsforcoColorido[1].x = ip.x;
                             EsforcoColorido[1].y = ip.y;
                             EsforcoColorido[1].z = 0;

                             EsforcoColorido[2].x = pIni.x;
                             EsforcoColorido[2].y = pIni.y;
                             EsforcoColorido[2].z = EsforcoCoords[1].z;

                             // segundo triangulo
                             EsforcoColorido[3].x = ip.x;
                             EsforcoColorido[3].y = ip.y;
                             EsforcoColorido[3].z = 0;

                             EsforcoColorido[4].x = pFin.x;
                             EsforcoColorido[4].y = pFin.y;
                             EsforcoColorido[4].z = EsforcoCoords[2].z;

                             EsforcoColorido[5].x = pFin.x;
                             EsforcoColorido[5].y = pFin.y;
                             EsforcoColorido[5].z = 0;
                         }

                         if (EsforcoCoords[1].z < 0)
                         {
                             // primeiro triangulo
                             EsforcoColorido[0].x = pIni.x;
                             EsforcoColorido[0].y = pIni.y;
                             EsforcoColorido[0].z = 0;

                             EsforcoColorido[1].x = pIni.x;
                             EsforcoColorido[1].y = pIni.y;
                             EsforcoColorido[1].z = EsforcoCoords[1].z;

                             EsforcoColorido[2].x = ip.x;
                             EsforcoColorido[2].y = ip.y;
                             EsforcoColorido[2].z = 0;

                             // segundo triangulo
                             EsforcoColorido[3].x = ip.x;
                             EsforcoColorido[3].y = ip.y;
                             EsforcoColorido[3].z = 0;

                             EsforcoColorido[4].x = pFin.x;
                             EsforcoColorido[4].y = pFin.y;
                             EsforcoColorido[4].z = 0;

                             EsforcoColorido[5].x = pFin.x;
                             EsforcoColorido[5].y = pFin.y;
                             EsforcoColorido[5].z = EsforcoCoords[2].z;
                         }
                     }
                 
                 else
                 {
                     EsforcoColorido[0].x = pIni.x;
                     EsforcoColorido[0].y = pIni.y;
                     EsforcoColorido[0].z = 0;

                     if (EsforcoCoords[1].z > 0)
                     {
                         EsforcoColorido[1].x = pFin.x;
                         EsforcoColorido[1].y = pFin.y;
                         EsforcoColorido[1].z = 0;

                         EsforcoColorido[2].x = pFin.x;
                         EsforcoColorido[2].y = pFin.y;
                         EsforcoColorido[2].z = EsforcoCoords[2].z;

                         EsforcoColorido[3].x = pIni.x;
                         EsforcoColorido[3].y = pIni.y;
                         EsforcoColorido[3].z = EsforcoCoords[1].z;
                     }

                     if (EsforcoCoords[1].z < 0)
                     {
                         EsforcoColorido[1].x = pIni.x;
                         EsforcoColorido[1].y = pIni.y;
                         EsforcoColorido[1].z = EsforcoCoords[1].z;

                         EsforcoColorido[2].x = pFin.x;
                         EsforcoColorido[2].y = pFin.y;
                         EsforcoColorido[2].z = EsforcoCoords[2].z;

                         EsforcoColorido[3].x = pFin.x;
                         EsforcoColorido[3].y = pFin.y;
                         EsforcoColorido[3].z = 0;
                     }
                 }

                 EsforcoCoords[3].x = pFin.x;
                 EsforcoCoords[3].y = pFin.y;
                 EsforcoCoords[3].z = 0;

             }
             /*
             CoordsFletor[0].x = pIni.x;
             CoordsFletor[0].y = pIni.y;
             CoordsFletor[0].z = 0;

             CoordsFletor[1].x = pIni.x;
             CoordsFletor[1].y = pIni.y;
             CoordsFletor[1].z = Esforcos[2]*-1;

             CoordsFletor[2].x = pFin.x;
             CoordsFletor[2].y = pFin.y;
             CoordsFletor[2].z = Esforcos[5];
          
             double inx = 0, iny = 0;

             //tem q rodar as coords da barra para coord globais
             TAlgebra.Multiplica_Matriz_Vetor(ref MatrizRotacao, ref coordLocal, ref coordGlobal, 6, 6);


             if (Geom.calcIntersecEQU_RETA(Math.Abs(coordGlobal[1]), Math.Abs(coordGlobal[2]), 
                                           Math.Abs(coordGlobal[4]), Math.Abs(coordGlobal[5]),
                                           Math.Abs(coordGlobal[1]), Math.Abs(coordGlobal[2]) + CoordsFletor[1].z,
                                           Math.Abs(coordGlobal[4]), Math.Abs(coordGlobal[5]) + CoordsFletor[2].z, 
                                           ref inx, ref iny))
             {
                 vec3 l1_p1 = new vec3(pIni.x, pIni.y,0);
                 vec3 l1_p2 = new vec3(pFin.x, pFin.y, 0);
                 vec3 l2_p1 = new vec3(pIni.x, pIni.y, CoordsFletor[1].z);
                 vec3 l2_p2 = new vec3(pFin.x, pFin.y, CoordsFletor[2].z);
                 vec3 ip = new vec3(0);

                 if (Geom.Intersec3D(l1_p1,l1_p2,l2_p1,l2_p2, ref ip))
                 {                   
                     DoisPoligonosNaBarra = true;

                     if (CoordsFletor[1].z > 0)
                     {
                         // primeiro triangulo
                         FletorColorido[0].x = pIni.x;
                         FletorColorido[0].y = pIni.y;
                         FletorColorido[0].z = 0;

                         FletorColorido[1].x = ip.x;
                         FletorColorido[1].y = ip.y;
                         FletorColorido[1].z = 0;

                         FletorColorido[2].x = pIni.x;
                         FletorColorido[2].y = pIni.y;
                         FletorColorido[2].z = CoordsFletor[1].z;

                         // segundo triangulo
                         FletorColorido[3].x = ip.x;
                         FletorColorido[3].y = ip.y;
                         FletorColorido[3].z = 0;

                         FletorColorido[4].x = pFin.x;
                         FletorColorido[4].y = pFin.y;
                         FletorColorido[4].z = CoordsFletor[2].z;

                         FletorColorido[5].x = pFin.x;
                         FletorColorido[5].y = pFin.y;
                         FletorColorido[5].z = 0;
                     }

                     if (CoordsFletor[1].z < 0)
                     {
                         // primeiro triangulo
                         FletorColorido[0].x = pIni.x;
                         FletorColorido[0].y = pIni.y;
                         FletorColorido[0].z = 0;

                         FletorColorido[1].x = pIni.x;
                         FletorColorido[1].y = pIni.y;
                         FletorColorido[1].z = CoordsFletor[1].z;

                         FletorColorido[2].x = ip.x;
                         FletorColorido[2].y = ip.y;
                         FletorColorido[2].z = 0;

                         // segundo triangulo
                         FletorColorido[3].x = ip.x;
                         FletorColorido[3].y = ip.y;
                         FletorColorido[3].z = 0;

                         FletorColorido[4].x = pFin.x;
                         FletorColorido[4].y = pFin.y;
                         FletorColorido[4].z = 0;

                         FletorColorido[5].x = pFin.x;
                         FletorColorido[5].y = pFin.y;
                         FletorColorido[5].z = CoordsFletor[2].z;
                     }
                 }
             }
             else
             {
                 FletorColorido[0].x = pIni.x;
                 FletorColorido[0].y = pIni.y;
                 FletorColorido[0].z = 0;

                 if (CoordsFletor[1].z > 0)
                 {
                     FletorColorido[1].x = pFin.x;
                     FletorColorido[1].y = pFin.y;
                     FletorColorido[1].z = 0;

                     FletorColorido[2].x = pFin.x;
                     FletorColorido[2].y = pFin.y;
                     FletorColorido[2].z = CoordsFletor[2].z;

                     FletorColorido[3].x = pIni.x;
                     FletorColorido[3].y = pIni.y;
                     FletorColorido[3].z = CoordsFletor[1].z;
                 }

                 if (CoordsFletor[1].z < 0)
                 {
                     FletorColorido[1].x = pIni.x;
                     FletorColorido[1].y = pIni.y;
                     FletorColorido[1].z = CoordsFletor[1].z;

                     FletorColorido[2].x = pFin.x;
                     FletorColorido[2].y = pFin.y;
                     FletorColorido[2].z = CoordsFletor[2].z;

                     FletorColorido[3].x = pFin.x;
                     FletorColorido[3].y = pFin.y;
                     FletorColorido[3].z = 0;
                 }
             }

             CoordsFletor[3].x = pFin.x;
             CoordsFletor[3].y = pFin.y;
             CoordsFletor[3].z = 0;*/
         }

         double newx1, newy1, newz1, px_x, px_y;

         public int RetIndiceRGB_Deslocamentos(double Deslocamento)
         {
             try
             {
                 if (Deslocamento >= RGB_Deslocamentos[0, 1] && Deslocamento <= RGB_Deslocamentos[0, 0])
                     return 0;
                 if (Deslocamento >= RGB_Deslocamentos[1, 1] && Deslocamento <= RGB_Deslocamentos[1, 0])
                     return 1;
                 if (Deslocamento >= RGB_Deslocamentos[2, 1] && Deslocamento <= RGB_Deslocamentos[2, 0])
                     return 2;
                 if (Deslocamento >= RGB_Deslocamentos[3, 1] && Deslocamento <= RGB_Deslocamentos[3, 0])
                     return 3;
                 if (Deslocamento >= RGB_Deslocamentos[4, 1] && Deslocamento <= RGB_Deslocamentos[4, 0])
                     return 4;
                 if (Deslocamento >= RGB_Deslocamentos[5, 1] && Deslocamento <= RGB_Deslocamentos[5, 0])
                     return 5;
                 if (Deslocamento >= RGB_Deslocamentos[6, 1] && Deslocamento <= RGB_Deslocamentos[6, 0])
                     return 6;
                 if (Deslocamento >= RGB_Deslocamentos[7, 1] && Deslocamento <= RGB_Deslocamentos[7, 0])
                     return 7;
                 if (Deslocamento >= RGB_Deslocamentos[8, 1] && Deslocamento <= RGB_Deslocamentos[8, 0])
                     return 8;
                 if (Deslocamento >= RGB_Deslocamentos[9, 1] && Deslocamento <= RGB_Deslocamentos[9, 0])
                     return 9;             //MessageBox.Show(Momento.ToString("n5"));
             }
             catch(NullReferenceException)
             {
                 MessageBox.Show(RGB_Deslocamentos[0,1].ToString());
             }
             return 0;
         }

         private int RetIndiceRGB_Negativos(ref double [,]RGB, double Momento)
         {
             try
             {
                 if (Momento >= RGB[0, 1] && Momento <= RGB[0, 0])
                     return 0;
                 if (Momento >= RGB[1, 1] && Momento <= RGB[1, 0])
                     return 1;
                 if (Momento >= RGB[2, 1] && Momento <= RGB[2, 0])
                     return 2;
                 if (Momento >= RGB[3, 1] && Momento <= RGB[3, 0])
                     return 3;
                 if (Momento >= RGB[4, 1] && Momento <= RGB[4, 0])
                     return 4;
                 if (Momento >= RGB[5, 1] && Momento <= RGB[5, 0])
                     return 5;
             }
             catch(Exception e)
             {
                 MessageBox.Show(e.Message);
                 return 0;
             }

             //MessageBox.Show(Momento.ToString("n5"));

             return 0;
         }

         private int RetIndiceRGB_Positivos(ref double[,] RGB, double Momento)
         {
             try
             {
                 if (Momento >= RGB[0, 1] && Momento <= RGB[0, 0])
                 return 0;
             if (Momento >= RGB[1, 1] && Momento <= RGB[1, 0])
                 return 1;
             if (Momento >= RGB[2, 1] && Momento <= RGB[2, 0])
                 return 2;
             if (Momento >= RGB[3, 1] && Momento <= RGB[3, 0])
                 return 3;
             }
             catch(Exception e)
             {
                 MessageBox.Show(e.Message);
                 return 0;
             }
             return 0;
          }

         int IndiceRGBi, IndiceRGBf;
         int Ri, Gi, Bi, Rf, Gf, Bf;
         double coordx, coordy, coordz;

         public void Desenha3D(double OffSetX, double OffSetY, double MultiplicadorAltura, bool Colorido, bool ComDeslocamento)
         {
             try
             {
                 GL.Color3(35, 142, 35);
                 if (!barraViga)
                     GL.Color3(this.layer.Rgb[0], this.layer.Rgb[1], this.layer.Rgb[2]);

                 if (ComDeslocamento)
                 {
                     /* ponto inicial*/
                     coordx = pIni.x - OffSetX;
                     coordy = pIni.y - OffSetY;
                     coordz = pIni.Deslocamento[3] * MultiplicadorAltura;

                     if (Colorido)
                     {
                         IndiceRGBi = RetIndiceRGB_Deslocamentos(pIni.Deslocamento[3]);
                         Ri = (byte)RGB_Deslocamentos[IndiceRGBi, 2];
                         Gi = (byte)RGB_Deslocamentos[IndiceRGBi, 3];
                         Bi = (byte)RGB_Deslocamentos[IndiceRGBi, 4];

                         GL.Color3(Ri, Gi, Bi);
                     }
                     if (barraRigida)
                         GL.Color3(255, 0, 0);

                     GL.Vertex3(coordx, coordy, coordz);

                     g2d.Project(ref px_x, ref px_y, coordx, coordy, coordz);
                     pIni.px_x = (int)px_x;
                     pIni.px_y = (int)px_y;

                     /* ponto final*/
                     coordx = pFin.x - OffSetX;
                     coordy = pFin.y - OffSetY;
                     coordz = pFin.Deslocamento[3] * MultiplicadorAltura;

                     if (Colorido)
                     {
                         IndiceRGBf = RetIndiceRGB_Deslocamentos(pFin.Deslocamento[3]);
                         Rf = (byte)RGB_Deslocamentos[IndiceRGBf, 2];
                         Gf = (byte)RGB_Deslocamentos[IndiceRGBf, 3];
                         Bf = (byte)RGB_Deslocamentos[IndiceRGBf, 4];
                         GL.Color3(Rf, Gf, Bf);
                     }

                     if (barraRigida)
                         GL.Color3(255, 0, 0);
                     GL.Vertex3(coordx, coordy, coordz);

                     g2d.Project(ref px_x, ref px_y, coordx, coordy, coordz);
                     pFin.px_x = (int)px_x;
                     pFin.px_y = (int)px_y;
                 }
                 else
                 {
                     /* ponto inicial*/
                     if (barraRigida)
                         GL.Color3(255, 0, 0);

                     coordx = pIni.x - OffSetX;
                     coordy = pIni.y - OffSetY;
                     coordz = 0;

                     GL.Vertex3(coordx, coordy, coordz);

                     g2d.Project(ref px_x, ref px_y, coordx, coordy, coordz);
                     pIni.px_x = (int)px_x;
                     pIni.px_y = (int)px_y;

                     /* ponto final*/
                     coordx = pFin.x - OffSetX;
                     coordy = pFin.y - OffSetY;
                     coordz = 0;

                     GL.Vertex3(coordx, coordy, coordz);

                     g2d.Project(ref px_x, ref px_y, coordx, coordy, coordz);
                     pFin.px_x = (int)px_x;
                     pFin.px_y = (int)px_y;
                 }
                 //  GL.LineWidth(1);
                 /* if (coordIsoDeslocamento != null)
                  {
                      xNo = coordIsoDeslocamento.x;
                      yNo = coordIsoDeslocamento.y;
                      GL.Color3(255, 0, 0);
                      for (j = 0; j <= 10; j++)
                      {
                          GL.Begin(PrimitiveType.Lines);
                          GL.Vertex2d((xNo + (2 * Math.Cos(j * twicePi / 10))), (yNo + (2 * Math.Sin(j * twicePi / 10))));
                          GL.Vertex2d((xNo + (2 * Math.Cos((j + 1) * twicePi / 10))), (yNo + (2 * Math.Sin((j + 1) * twicePi / 10))));
                          GL.End();
                      };
                  }*/
             }
             catch(Exception e)
             {
                 MessageBox.Show(e.Message);
             }

         }
         bool InicialNegativo, InicialPositivo;
         double[,] RGB_Negativos, RGB_Positivos;

         public void DesenhaDiagramas(int iEsforco, 
                                    double OffSetX, double OffSetY, 
                                    double MultiplicadorAltura, 
                                    bool Gradiente, bool DuasCores, bool Arestas, bool ApenasBarrasColoridas, 
                                    bool BarrasLaje, bool BarrasViga, bool BarrasRigidas,
                                    bool MostrarDirecaoX, bool MostrarDirecaoY)
         {
             if (!MostrarDirecaoX && direcaoX) return;
             if (!MostrarDirecaoY && direcaoY) return;
             if (!BarrasRigidas && barraRigida) return;

             if (iEsforco == iFletor)
             {
                 EsforcoColorido = FletorColorido;
                 EsforcoCoords   = CoordsFletor;

                 InicialNegativo = EsforcoCoords[1].z > 0;
                 InicialPositivo = EsforcoCoords[1].z < 0;

                 RGB_Negativos = RGB_FletoresNegativos;
                 RGB_Positivos = RGB_FletoresPositivos;
             }
             if (iEsforco == iTorcor)
             {
                 EsforcoColorido = TorcorColorido;
                 EsforcoCoords   = CoordsTorcor;

                 InicialNegativo = EsforcoCoords[1].z < 0;
                 InicialPositivo = EsforcoCoords[1].z > 0;

                 RGB_Negativos = RGB_TorcoresNegativos;
                 RGB_Positivos = RGB_TorcoresPositivos;
             }
             if (iEsforco == iCortante)
             {
                 EsforcoColorido = CortanteColorido;
                 EsforcoCoords   = CoordsCortante;

                 InicialNegativo = EsforcoCoords[1].z < 0;
                 InicialPositivo = EsforcoCoords[1].z > 0;

                 RGB_Negativos = RGB_CortantesNegativos;
                 RGB_Positivos = RGB_CortantesPositivos;
             }
                
             if ((BarrasViga && barraViga) || 
                 (BarrasLaje && !barraViga))
             {
                 
                 GL.Begin(PrimitiveType.LineStrip);
               
                 for (i = 0; i < 4; i++)
                 {
                     newx1 = EsforcoCoords[i].x - OffSetX;
                     newy1 = EsforcoCoords[i].y - OffSetY;
                     newz1 = EsforcoCoords[i].z * MultiplicadorAltura;

                     GL.Color3(0.3f, .3, 0.3);
                     if (!Gradiente && !DuasCores)
                     {
                         if (iEsforco == iFletor)
                         {
                             //if (EsforcoCoords[i].z > 0)
                            //     GL.Color3(1f, 0, 0.0);
                            // else
                                 GL.Color3(0, 127, 255);
                         }
                         else
                         {
                           //  if (EsforcoCoords[i].z < 0)
                           //      GL.Color3(1f, 0, 0.0);
                          //   else
                                 GL.Color3(0, 127, 255);
                         }
 
                     }

                     if (Arestas)
                       GL.Vertex3(newx1, newy1, newz1);

                     g2d.Project(ref px_x, ref px_y, newx1, newy1, newz1);
                     EsforcoCoords[i].px_x = px_x;
                     EsforcoCoords[i].px_y = px_y;
                 }

                 GL.End();

                 if (Gradiente || DuasCores)
                 {
                     #region Dois polígonos
                     GL.Enable(EnableCap.Blend);
                    GL.BlendFunc(BlendingFactor.SrcAlpha, BlendingFactor.OneMinusSrcAlpha);

                    if (DoisPoligonosNaBarra && (iEsforco == iFletor))
                     {
                         if (EsforcoCoords[1].z > 0)  // mom negativo
                         {
                             if (DuasCores)
                             {
                                 Ri = 255; Rf = 255;
                                 Gi = 0; Gf = 0;
                                 Bi = 0; Bf = 0;
                             }
                             else
                             {
                                 IndiceRGBi = RetIndiceRGB_Negativos(ref RGB_Negativos, Math.Abs(EsforcoCoords[1].z));

                                 Ri = (byte)RGB_Negativos[IndiceRGBi, 2];
                                 Gi = (byte)RGB_Negativos[IndiceRGBi, 3];
                                 Bi = (byte)RGB_Negativos[IndiceRGBi, 4];

                                 Rf = 255; // branco, pois é zero
                                 Gf = 255;
                                 Bf = 255;
                             }

                             if (ApenasBarrasColoridas)
                             {
                               //  GL.LineWidth(2);

                                 GL.Begin(PrimitiveType.Lines);
                                 GL.Color4(Ri, Gi, Bi, 200);
                                 GL.Vertex3(pIni.x - OffSetX, pIni.y - OffSetY, 0);

                                 IndiceRGBf = RetIndiceRGB_Positivos(ref RGB_Positivos, Math.Abs(EsforcoCoords[2].z));
                                 Rf = (byte)RGB_Positivos[IndiceRGBf, 2];
                                 Gf = (byte)RGB_Positivos[IndiceRGBf, 3];
                                 Bf = (byte)RGB_Positivos[IndiceRGBf, 4];

                                 GL.Color4(Rf, Gf, Bf, 200);
                                 GL.Vertex3(pFin.x - OffSetX, pFin.y - OffSetY, 0);
                                 GL.End();
                             }
                             else
                             {

                                 GL.Begin(PrimitiveType.TriangleFan);
                                 GL.Color4(Ri, Gi, Bi, 200);
                                 GL.Vertex3(EsforcoColorido[0].x - OffSetX, EsforcoColorido[0].y - OffSetY, EsforcoColorido[0].z * MultiplicadorAltura);

                                 GL.Color4(Rf, Gf, Bf, 200);
                                 GL.Vertex3(EsforcoColorido[1].x - OffSetX, EsforcoColorido[1].y - OffSetY, EsforcoColorido[1].z * MultiplicadorAltura);

                                 GL.Color4(Ri, Gi, Bi, 200);
                                 GL.Vertex3(EsforcoColorido[2].x - OffSetX, EsforcoColorido[2].y - OffSetY, EsforcoColorido[2].z * MultiplicadorAltura);
                                 GL.End();

                                 //segundo triangulo
                                 Ri = Rf;
                                 Gi = Gf;
                                 Bi = Bf;
                             
                                 if (DuasCores)
                                 {
                                     Ri = 0; Rf = 0;
                                     Gi = 0; Gf = 0;
                                     Bi = 255; Bf = 255;
                                 }
                                 else
                                 {
                                     IndiceRGBf = RetIndiceRGB_Positivos(ref RGB_Positivos, Math.Abs(EsforcoCoords[2].z));
                                     Rf = (byte)RGB_Positivos[IndiceRGBf, 2];
                                     Gf = (byte)RGB_Positivos[IndiceRGBf, 3];
                                     Bf = (byte)RGB_Positivos[IndiceRGBf, 4];
                                 }

                                 GL.Begin(PrimitiveType.TriangleFan);
                                 GL.Color4(Ri, Gi, Bi, 200);
                                 GL.Vertex3(EsforcoColorido[3].x - OffSetX, EsforcoColorido[3].y - OffSetY, EsforcoColorido[3].z * MultiplicadorAltura);

                                 GL.Color4(Rf, Gf, Bf, 200);
                                 GL.Vertex3(EsforcoColorido[4].x - OffSetX, EsforcoColorido[4].y - OffSetY, EsforcoColorido[4].z * MultiplicadorAltura);

                                 GL.Color4(Rf, Gf, Bf, 200);
                                 GL.Vertex3(EsforcoColorido[5].x - OffSetX, EsforcoColorido[5].y - OffSetY, EsforcoColorido[5].z * MultiplicadorAltura);

                                 GL.Color4(Ri, Gi, Bi, 200);
                                 GL.Vertex3(EsforcoColorido[3].x - OffSetX, EsforcoColorido[3].y - OffSetY, EsforcoColorido[3].z * MultiplicadorAltura);
                                 GL.End();
                            }
                             
                         } 
                         else
                         if (EsforcoCoords[1].z < 0) // mom positivo
                         {
                             if (DuasCores)
                             {
                                 Ri = 0; Rf = 0;
                                 Gi = 0; Gf = 0;
                                 Bi = 255; Bf = 255;
                             }
                             else
                             {
                                 IndiceRGBi = RetIndiceRGB_Positivos(ref RGB_Positivos, Math.Abs(EsforcoCoords[1].z));

                                 Ri = (byte)RGB_Positivos[IndiceRGBi, 2];
                                 Gi = (byte)RGB_Positivos[IndiceRGBi, 3];
                                 Bi = (byte)RGB_Positivos[IndiceRGBi, 4];

                                 Rf = 255; // branco, pois é zero
                                 Gf = 255;
                                 Bf = 255;
                             }

                             if (ApenasBarrasColoridas)
                             {
                                 GL.Begin(PrimitiveType.Lines);
                                 GL.Color4(Ri, Gi, Bi, 200);
                                 GL.Vertex3(pIni.x - OffSetX, pIni.y - OffSetY, 0);

                                 IndiceRGBf = RetIndiceRGB_Negativos(ref RGB_Negativos, Math.Abs(EsforcoCoords[2].z));
                                 Rf = (byte)RGB_Negativos[IndiceRGBf, 2];
                                 Gf = (byte)RGB_Negativos[IndiceRGBf, 3];
                                 Bf = (byte)RGB_Negativos[IndiceRGBf, 4];

                                 GL.Color4((int)Rf, (int)Gf, (int)Bf, (int)200);
                                 GL.Vertex3(pFin.x - OffSetX, pFin.y - OffSetY, 0);
                                 GL.End();
                             }
                             else
                             {
                                 GL.Begin(PrimitiveType.TriangleFan);
                                 GL.Color4(Ri, Gi, Bi, 200);
                                 GL.Vertex3(EsforcoColorido[0].x - OffSetX, EsforcoColorido[0].y - OffSetY, EsforcoColorido[0].z * MultiplicadorAltura);
                                 GL.Vertex3(EsforcoColorido[1].x - OffSetX, EsforcoColorido[1].y - OffSetY, EsforcoColorido[1].z * MultiplicadorAltura);

                                 GL.Color4(Rf, Gf, Bf, 200);
                                 GL.Vertex3(EsforcoColorido[2].x - OffSetX, EsforcoColorido[2].y - OffSetY, EsforcoColorido[2].z * MultiplicadorAltura);

                                 GL.Color4(Ri, Gi, Bi, 200);
                                 GL.Vertex3(EsforcoColorido[0].x - OffSetX, EsforcoColorido[0].y - OffSetY, EsforcoColorido[0].z * MultiplicadorAltura);
                                 GL.End();

                                 //segundo triangulo
                                 Ri = Rf;
                                 Gi = Gf;
                                 Bi = Bf;

                                 if (DuasCores)
                                 {
                                     Ri = 255; Rf = 255;
                                     Gi = 0; Gf = 0;
                                     Bi = 0; Bf = 0;
                                 }
                                 else
                                 {
                                     IndiceRGBf = RetIndiceRGB_Negativos(ref RGB_Negativos, Math.Abs(EsforcoCoords[2].z));
                                     Rf = (byte)RGB_Negativos[IndiceRGBf, 2];
                                     Gf = (byte)RGB_Negativos[IndiceRGBf, 3];
                                     Bf = (byte)RGB_Negativos[IndiceRGBf, 4];
                                 }

                                 GL.Begin(PrimitiveType.TriangleFan);
                                 GL.Color4(Ri, Gi, Bi, 200);
                                 GL.Vertex3(EsforcoColorido[3].x - OffSetX, EsforcoColorido[3].y - OffSetY, EsforcoColorido[3].z * MultiplicadorAltura);

                                 GL.Color4(Rf, Gf, Bf, 200);
                                 GL.Vertex3(EsforcoColorido[4].x - OffSetX, EsforcoColorido[4].y - OffSetY, EsforcoColorido[4].z * MultiplicadorAltura);

                                 GL.Color4(Rf, Gf, Bf, 200);
                                 GL.Vertex3(EsforcoColorido[5].x - OffSetX, EsforcoColorido[5].y - OffSetY, EsforcoColorido[5].z * MultiplicadorAltura);

                                 GL.Color4(Ri, Gi, Bi, 200);
                                 GL.Vertex3(EsforcoColorido[3].x - OffSetX, EsforcoColorido[3].y - OffSetY, EsforcoColorido[3].z * MultiplicadorAltura);
                                 GL.End();
                             }
                         }
                     }
                     #endregion
                     else
                     {
                         if (InicialNegativo)
                         //if (EsforcoCoords[1].z > 0)
                         {                            
                             if (DuasCores)
                             {
                                Ri = 255; Rf = 255;
                                Gi = 0; Gf = 0;
                                Bi = 0; Bf = 0;
                             }
                             else
                             {
                                 IndiceRGBi = RetIndiceRGB_Negativos(ref RGB_Negativos, Math.Abs(EsforcoCoords[1].z));
                                 Ri = (byte)RGB_Negativos[IndiceRGBi, 2];
                                 Gi = (byte)RGB_Negativos[IndiceRGBi, 3];
                                 Bi = (byte)RGB_Negativos[IndiceRGBi, 4];

                                 IndiceRGBf = RetIndiceRGB_Negativos(ref RGB_Negativos, Math.Abs(EsforcoCoords[2].z));
                                 Rf = (byte)RGB_Negativos[IndiceRGBf, 2];
                                 Gf = (byte)RGB_Negativos[IndiceRGBf, 3];
                                 Bf = (byte)RGB_Negativos[IndiceRGBf, 4];
                             }
                             
                             if (ApenasBarrasColoridas)
                             {
                                 GL.Begin(PrimitiveType.Lines);
                                 GL.Color4(Ri, Gi, Bi, 200);
                                 GL.Vertex3(pIni.x - OffSetX, pIni.y - OffSetY, 0);

                                 GL.Color4(Rf, Gf, Bf, 200);
                                 GL.Vertex3(pFin.x - OffSetX, pFin.y - OffSetY, 0);
                                 GL.End();
                             }
                             else
                             {
                                 GL.Begin(PrimitiveType.Polygon);
                                 GL.Color4(Ri, Gi, Bi, 200);
                                 GL.Vertex3(EsforcoColorido[0].x - OffSetX, EsforcoColorido[0].y - OffSetY, EsforcoColorido[0].z * MultiplicadorAltura);

                                 GL.Color4(Rf, Gf, Bf, 200);
                                 GL.Vertex3(EsforcoColorido[1].x - OffSetX, EsforcoColorido[1].y - OffSetY, EsforcoColorido[1].z * MultiplicadorAltura);
                                 GL.Vertex3(EsforcoColorido[2].x - OffSetX, EsforcoColorido[2].y - OffSetY, EsforcoColorido[2].z * MultiplicadorAltura);

                                 GL.Color4(Ri, Gi, Bi, 200);
                                 GL.Vertex3(EsforcoColorido[3].x - OffSetX, EsforcoColorido[3].y - OffSetY, EsforcoColorido[3].z * MultiplicadorAltura);
                                 GL.Vertex3(EsforcoColorido[0].x - OffSetX, EsforcoColorido[0].y - OffSetY, EsforcoColorido[0].z * MultiplicadorAltura);
                                 GL.End();
                             }
                         } 
                         else
                         if (InicialPositivo)
                         {
                             if (DuasCores)
                             {
                                 Ri = 0; Rf = 0;
                                 Gi = 0; Gf = 0;
                                 Bi = 255; Bf = 255;
                             }
                             else
                             {
                                 IndiceRGBi = RetIndiceRGB_Positivos(ref RGB_Positivos, Math.Abs(EsforcoCoords[1].z));
                                 Ri = (byte)RGB_Positivos[IndiceRGBi, 2];
                                 Gi = (byte)RGB_Positivos[IndiceRGBi, 3];
                                 Bi = (byte)RGB_Positivos[IndiceRGBi, 4];

                                 IndiceRGBf = RetIndiceRGB_Positivos(ref RGB_Positivos, Math.Abs(EsforcoCoords[2].z));
                                 Rf = (byte)RGB_Positivos[IndiceRGBf, 2];
                                 Gf = (byte)RGB_Positivos[IndiceRGBf, 3];
                                 Bf = (byte)RGB_Positivos[IndiceRGBf, 4];
                             }

                             if (ApenasBarrasColoridas)
                             {
                                 GL.Begin(PrimitiveType.Lines);
                                 GL.Color4(Ri, Gi, Bi, 200);
                                 GL.Vertex3(pIni.x - OffSetX, pIni.y - OffSetY, 0);

                                 GL.Color4(Rf, Gf, Bf, 200);
                                 GL.Vertex3(pFin.x - OffSetX, pFin.y - OffSetY, 0);
                                 GL.End();
                             }
                             else
                             {
                                 GL.Begin(PrimitiveType.Polygon);

                                 GL.Color4(Ri, Gi, Bi, 200);
                                 GL.Vertex3(EsforcoColorido[0].x - OffSetX, EsforcoColorido[0].y - OffSetY, EsforcoColorido[0].z * MultiplicadorAltura);
                                 GL.Vertex3(EsforcoColorido[1].x - OffSetX, EsforcoColorido[1].y - OffSetY, EsforcoColorido[1].z * MultiplicadorAltura);

                                 GL.Color4(Rf, Gf, Bf, 200);
                                 GL.Vertex3(EsforcoColorido[2].x - OffSetX, EsforcoColorido[2].y - OffSetY, EsforcoColorido[2].z * MultiplicadorAltura);
                                 GL.Vertex3(EsforcoColorido[3].x - OffSetX, EsforcoColorido[3].y - OffSetY, EsforcoColorido[3].z * MultiplicadorAltura);

                                 GL.Color4(Ri, Gi, Bi, 200);
                                 GL.Vertex3(EsforcoColorido[0].x - OffSetX, EsforcoColorido[0].y - OffSetY, EsforcoColorido[0].z * MultiplicadorAltura);
                                 GL.End();
                             }
                         }
                     }
                 //    GL.Disable(Gl.GL_BLEND);
                 }
             }
         }

         public void CalcularEsforcos()
         {
             DeslocamentosGlobais[1] = pIni.Deslocamento[1];
             DeslocamentosGlobais[2] = pIni.Deslocamento[2];
             DeslocamentosGlobais[3] = pIni.Deslocamento[3];

             DeslocamentosGlobais[4] = pFin.Deslocamento[1];
             DeslocamentosGlobais[5] = pFin.Deslocamento[2];
             DeslocamentosGlobais[6] = pFin.Deslocamento[3];

             TAlgebra.Multiplica_Matriz_Vetor(ref MatrizRotacao, ref DeslocamentosGlobais , ref DeslocamentosLocais, 6, 6);
             TAlgebra.Multiplica_Matriz_Vetor(ref MatrizLocal, ref DeslocamentosLocais, ref Esforcos, 6, 6); /*Esforço é sempre local*/

             CriaDiagramas();

         }

         #region Métodos gráficos
         double ultZ, ultX, ultY;
         double xNo, yNo;
         float twicePi = 2.0f * 3.1415f;
         int k;
         public override void Desenha(ref System.Drawing.Graphics Cad)
         {
           //  proxima = false;

                 if (Math.Min(FPrincipal.pixelX(pIni.x), FPrincipal.pixelX(pFin.px_x)) < 0 && Math.Max(FPrincipal.pixelX(pIni.x), FPrincipal.pixelX(pFin.x)) < 0) return;
                else
                if (Math.Min(FPrincipal.pixelX(pIni.x), FPrincipal.pixelX(pFin.x)) > FPrincipal.w && Math.Max(FPrincipal.pixelX(pIni.x), FPrincipal.pixelX(pFin.x)) > FPrincipal.h) return;
                else
                if (Math.Min(FPrincipal.pixelY(pIni.y), FPrincipal.pixelY(pFin.y)) < 0 && Math.Max(FPrincipal.pixelX(pIni.x), FPrincipal.pixelY(pFin.y)) < 0) return;
                else
                if (Math.Min(FPrincipal.pixelY(pIni.y), FPrincipal.pixelY(pFin.y)) > FPrincipal.h && Math.Max(FPrincipal.pixelY(pIni.y), FPrincipal.pixelY(pFin.y)) > FPrincipal.h) return;
                
                if (!base.Visivel) return;
            
            //     if (!proxima)
                 {
                //   g2d.drawCircle(Desenho.pixelX((float)(pIni.x)), Desenho.pixelY((float)(pIni.y)),
                //          0.55f / Desenho.precisaoPixel, 20, 1,0,0);

                //         g2d.drawCircle(Desenho.pixelX((float)(pFin.x)), Desenho.pixelY((float)(pFin.y)),
                //                              0.55f / Desenho.precisaoPixel, 20, 1, 0, 0);*/


                /*   GL.Begin(Gl.GL_LINE_LOOP);
                   GL.Color3(169,169,169);
                   GL.Vertex2f(Desenho.pixelX(pIni.x - 0.3), Desenho.pixelY(pIni.y + 0.3));
                   GL.Vertex2f(Desenho.pixelX(pIni.x + 0.3), Desenho.pixelY(pIni.y + 0.3));
                   GL.Vertex2f(Desenho.pixelX(pIni.x + 0.3), Desenho.pixelY(pIni.y - 0.3));
                   GL.Vertex2f(Desenho.pixelX(pIni.x - 0.3), Desenho.pixelY(pIni.y - 0.3));
                   GL.End();*/

                     if (barraViga)
                         mPen.Color = System.Drawing.Color.Green;
                     else
                         mPen.Color = System.Drawing.Color.FromArgb(this.layer.Rgb[0], this.layer.Rgb[1], this.layer.Rgb[2]);

       
                     if (barraRigida)
                       mPen.Color = System.Drawing.Color.Blue;

                     if (base.Selecionado)
                     {
                         if (base.MostrarGrip && (Object)Grips != null)
                             for (i = 0; i < Grips.Count(); i++)
                                 Grips[i].Desenha(ref Cad);
                     };

                    // if (selecaoPorProximidade)
                  //   {
                      //   GL.Color3f(0.4f, 0.2f, 0.8f);

                       //  mPen.Color = System.Drawing.Color.FromArgb(this.layer.Rgb[0], this.layer.Rgb[1], this.layer.Rgb[2]);
                    //};

                    // if (selecaoPorPontoMedio)
                    // {
                    //     GL.Color3f(1f, 0f, 0.2f);
                    //     GL.LineStipple(5, 0xAAAA);
                    //     GL.Enable(Gl.GL_LINE_STIPPLE);
                     //};
                     if (CargaDistribuida != 0)
                         mPen.Color = System.Drawing.Color.Yellow;

                     Cad.DrawLine(mPen, FPrincipal.pixelX(pIni.x), FPrincipal.pixelY(pIni.y), FPrincipal.pixelX(pFin.x), FPrincipal.pixelY(pFin.y));

                     if (coordIsoDeslocamento != null)
                     {
                         xNo = coordIsoDeslocamento.x;
                         yNo = coordIsoDeslocamento.y;
                         mPen.Color = System.Drawing.Color.Red;
                         for (j = 0; j <= 10; j++)
                         {
                             Cad.DrawLine(mPen, FPrincipal.pixelX((xNo + (1 * Math.Cos(j * twicePi / 10)))), FPrincipal.pixelY((yNo + (1 * Math.Sin(j * twicePi / 10)))),
                                                FPrincipal.pixelX((xNo + (1 * Math.Cos((j + 1) * twicePi / 10)))), FPrincipal.pixelY((yNo + (1 * Math.Sin((j + 1) * twicePi / 10)))));
                         };

                     }
                     
                     if (pFin.vinculo == 2)
                     {
                         xNo = pFin.x;
                         yNo = pFin.y; 
                         for (j = 0; j <= 10; j++)
                         {
                             Cad.DrawLine(mPen, FPrincipal.pixelX((xNo + (1 * Math.Cos(j * twicePi / 10)))), FPrincipal.pixelY((yNo + (1 * Math.Sin(j * twicePi / 10)))),
                                                FPrincipal.pixelX((xNo + (1 * Math.Cos((j + 1) * twicePi / 10)))), FPrincipal.pixelY((yNo + (1 * Math.Sin((j + 1) * twicePi / 10)))));
                         };
                     }

                     if (pIni.vinculo == 2)
                     {
                         xNo = pIni.x;
                         yNo = pIni.y; 
                         for (j = 0; j <= 10; j++)
                         {
                             Cad.DrawLine(mPen, FPrincipal.pixelX((xNo + (1 * Math.Cos(j * twicePi / 10)))), FPrincipal.pixelY((yNo + (1 * Math.Sin(j * twicePi / 10)))),
                                                FPrincipal.pixelX((xNo + (1 * Math.Cos((j + 1) * twicePi / 10)))), FPrincipal.pixelY((yNo + (1 * Math.Sin((j + 1) * twicePi / 10)))));
                         };
                     } 
                     if (pFin.vinculo == 3 || pFin.vinculo == 1)
                     {
                         xNo = pFin.x;
                         yNo = pFin.y;

                         if (pFin.vinculo == 1)
                         {
                             Cad.DrawLine(mPen, FPrincipal.pixelX((xNo - 1)), FPrincipal.pixelY((yNo - 1)),
                                                  FPrincipal.pixelX((xNo + 1)), FPrincipal.pixelY((yNo + 1)));
                             Cad.DrawLine(mPen, FPrincipal.pixelX((xNo - 1)), FPrincipal.pixelY((yNo + 1)),
                                                  FPrincipal.pixelX((xNo + 1)), FPrincipal.pixelY((yNo - 1)));
                         }
                         for (j = 0; j <= 10; j++)
                         {
                             Cad.DrawLine(mPen, FPrincipal.pixelX((xNo + (0.2 * Math.Cos(j * twicePi / 10)))), FPrincipal.pixelY((yNo + (0.2 * Math.Sin(j * twicePi / 10)))),
                                                FPrincipal.pixelX((xNo + (0.2 * Math.Cos((j + 1) * twicePi / 10)))), FPrincipal.pixelY((yNo + (0.2 * Math.Sin((j + 1) * twicePi / 10)))));
                         };

                     }
                     if (pIni.vinculo == 3 || pIni.vinculo == 1 || pIni.vinculo == 2)
                     {
                         xNo = pIni.x;
                         yNo = pIni.y;

                         if (pIni.vinculo == 2)
                         {
                          //   Cad.DrawLine(mPen, Desenho.pixelX((xNo - 1)), Desenho.pixelY((yNo - 1)),Desenho.pixelX((xNo + 1)), Desenho.pixelY((yNo - 1)));
                           //  Cad.DrawLine(mPen, Desenho.pixelX((xNo - 1)), Desenho.pixelY((yNo + 1)),Desenho.pixelX((xNo - 1)), Desenho.pixelY((yNo + 1)));
                         }
                         else
                         {
                             if (pIni.vinculo == 1)
                             {
                                 Cad.DrawLine(mPen, FPrincipal.pixelX((xNo - 1)), FPrincipal.pixelY((yNo - 1)),
                                                      FPrincipal.pixelX((xNo + 1)), FPrincipal.pixelY((yNo + 1)));
                                 Cad.DrawLine(mPen, FPrincipal.pixelX((xNo - 1)), FPrincipal.pixelY((yNo + 1)),
                                                      FPrincipal.pixelX((xNo + 1)), FPrincipal.pixelY((yNo - 1)));
                             }
                             for (j = 0; j <= 10; j++)
                             {
                                 Cad.DrawLine(mPen, FPrincipal.pixelX((xNo + (0.2 * Math.Cos(j * twicePi / 10)))), FPrincipal.pixelY((yNo + (0.2 * Math.Sin(j * twicePi / 10)))),
                                                    FPrincipal.pixelX((xNo + (0.2 * Math.Cos((j + 1) * twicePi / 10)))), FPrincipal.pixelY((yNo + (0.2 * Math.Sin((j + 1) * twicePi / 10)))));
                             };
                         }

                     }

                    // if (pontoAreaInf1.x != 0 && pontoAreaInf1.y != 0)
                   //  {
                     //    Cad.DrawLine(mPen, Desenho.pixelX((pontoAreaInf1.x)), Desenho.pixelY((pontoAreaInf1.y)),
                      //                      Desenho.pixelX((xNo + 1)), Desenho.pixelY((yNo + 1)));

                    // }

                     selecaoPorPontoMedio = false;
                     selecaoPorProximidade = false;

             //        GL.Disable(Gl.GL_LINE_STIPPLE);
                 };
         }

         public override bool Selecionar(float clicx, float clicy, float coordx, float coordy, TObjetoDesenho Owner = null)
         {

             if (this.layer.Congelado || this.layer.Travado ) //|| this.barraViga)
                 return false;

             bool esta_no_intervalo = false;
             double tol = 3.5;

   
             if (clicx - Math.Max(FPrincipal.pixelX(pIni.x), FPrincipal.pixelX(pFin.x)) > tol ||
                                     Math.Min(FPrincipal.pixelX(pIni.x), FPrincipal.pixelX(pFin.x)) - clicx > tol ||

             clicy - Math.Max(FPrincipal.pixelY(pIni.y), FPrincipal.pixelY(pFin.y)) > tol ||
                                 Math.Min(FPrincipal.pixelY(pIni.y), FPrincipal.pixelY(pFin.y)) - clicy > tol)

                 esta_no_intervalo = false;

             if (Math.Abs(FPrincipal.pixelX(pFin.x) - FPrincipal.pixelX(pIni.x)) < tol)
                 esta_no_intervalo = Math.Abs(FPrincipal.pixelX(pIni.x) - clicx) < tol || Math.Abs(FPrincipal.pixelX(pFin.x) - clicx) < tol;

             if (Math.Abs(FPrincipal.pixelY(pFin.y) - FPrincipal.pixelY(pIni.y)) < tol)
                 esta_no_intervalo = Math.Abs(FPrincipal.pixelY(pIni.y) - clicy) < tol || Math.Abs(FPrincipal.pixelY(pFin.y) - clicy) < tol;

             double x, y;

             if ((FPrincipal.pixelY(pFin.y) - FPrincipal.pixelY(pIni.y)) == 0)
                 x = 0;
             else
                 x = FPrincipal.pixelX(pIni.x) + (clicy - FPrincipal.pixelY(pIni.y)) * (FPrincipal.pixelX(pFin.x) - FPrincipal.pixelX(pIni.x)) / (FPrincipal.pixelY(pFin.y) - FPrincipal.pixelY(pIni.y));

             if ((FPrincipal.pixelX(pFin.x) - FPrincipal.pixelX(pIni.x)) == 0)
                 y = 0;
             else
                 y = FPrincipal.pixelY(pIni.y) + (clicx - FPrincipal.pixelX(pIni.x)) * (FPrincipal.pixelY(pFin.y) - FPrincipal.pixelY(pIni.y)) / (FPrincipal.pixelX(pFin.x) - FPrincipal.pixelX(pIni.x));

             float x1 = Math.Min(FPrincipal.pixelX(pIni.x), FPrincipal.pixelX(pFin.x));
             float y1 = Math.Min(FPrincipal.pixelY(pIni.y), FPrincipal.pixelY(pFin.y));
             float x2 = Math.Max(FPrincipal.pixelX(pIni.x), FPrincipal.pixelX(pFin.x));
             float y2 = Math.Max(FPrincipal.pixelY(pIni.y), FPrincipal.pixelY(pFin.y));

             esta_no_intervalo = Math.Abs(clicx - x) < tol || Math.Abs(clicy - y) < tol;
             if (esta_no_intervalo)
             {
                 float tolerancia = 0.01f;

                 if (Geom.Iguais(y1, y2, tolerancia))
                     esta_no_intervalo = ((clicx >= x1) && (clicx <= x2));
                 else
                     if (Geom.Iguais(x1, x2, tolerancia))
                         esta_no_intervalo = ((clicy >= y1) && (clicy <= y2));
                     else
                         esta_no_intervalo = ((clicx >= x1) && (clicx <= x2)) && ((clicy >= y1) && (clicy <= y2));
             };

             if (esta_no_intervalo)
             {
                 SetaSelecao(true, true);
                 return base.Selecionado;
             }

             return false;
         }

         public bool TestaSelecao(float clicx, float clicy)
         {
             bool esta_no_intervalo = false;
             double tol = 3.5;

             if (clicx - Math.Max(FPrincipal.pixelX(pIni.x), FPrincipal.pixelX(pFin.x)) > tol ||
                                     Math.Min(FPrincipal.pixelX(pIni.x), FPrincipal.pixelX(pFin.x)) - clicx > tol ||

             clicy - Math.Max(FPrincipal.pixelY(pIni.y), FPrincipal.pixelY(pFin.y)) > tol ||
                                 Math.Min(FPrincipal.pixelY(pIni.y), FPrincipal.pixelY(pFin.y)) - clicy > tol)

                 esta_no_intervalo = false;

             if (Math.Abs(FPrincipal.pixelX(pFin.x) - FPrincipal.pixelX(pIni.x)) < tol)
                 esta_no_intervalo = Math.Abs(FPrincipal.pixelX(pIni.x) - clicx) < tol || Math.Abs(FPrincipal.pixelX(pFin.x) - clicx) < tol;

             if (Math.Abs(FPrincipal.pixelY(pFin.y) - FPrincipal.pixelY(pIni.y)) < tol)
                 esta_no_intervalo = Math.Abs(FPrincipal.pixelY(pIni.y) - clicy) < tol || Math.Abs(FPrincipal.pixelY(pFin.y) - clicy) < tol;

             double x, y;

             if ((FPrincipal.pixelY(pFin.y) - FPrincipal.pixelY(pIni.y)) == 0)
                 x = 0;
             else
                 x = FPrincipal.pixelX(pIni.x) + (clicy - FPrincipal.pixelY(pIni.y)) * (FPrincipal.pixelX(pFin.x) - FPrincipal.pixelX(pIni.x)) / (FPrincipal.pixelY(pFin.y) - FPrincipal.pixelY(pIni.y));

             if ((FPrincipal.pixelX(pFin.x) - FPrincipal.pixelX(pIni.x)) == 0)
                 y = 0;
             else
                 y = FPrincipal.pixelY(pIni.y) + (clicx - FPrincipal.pixelX(pIni.x)) * (FPrincipal.pixelY(pFin.y) - FPrincipal.pixelY(pIni.y)) / (FPrincipal.pixelX(pFin.x) - FPrincipal.pixelX(pIni.x));

             float x1 = Math.Min(FPrincipal.pixelX(pIni.x), FPrincipal.pixelX(pFin.x));
             float y1 = Math.Min(FPrincipal.pixelY(pIni.y), FPrincipal.pixelY(pFin.y));
             float x2 = Math.Max(FPrincipal.pixelX(pIni.x), FPrincipal.pixelX(pFin.x));
             float y2 = Math.Max(FPrincipal.pixelY(pIni.y), FPrincipal.pixelY(pFin.y));

             esta_no_intervalo = Math.Abs(clicx - x) < tol || Math.Abs(clicy - y) < tol;
             if (esta_no_intervalo)
             {
                 float tolerancia = 0.01f;

                 if (Geom.Iguais(y1, y2, tolerancia))
                     esta_no_intervalo = ((clicx >= x1) && (clicx <= x2));
                 else
                     if (Geom.Iguais(x1, x2, tolerancia))
                         esta_no_intervalo = ((clicy >= y1) && (clicy <= y2));
                     else
                         esta_no_intervalo = ((clicx >= x1) && (clicx <= x2)) && ((clicy >= y1) && (clicy <= y2));
             };

             return esta_no_intervalo;
         }
         double alfa;

          public override void ShowHideGrips(bool visivel)
         {
             if (Grips != null)
                 foreach (TGrip grip in Grips)
                     grip.Visivel = visivel;
         }
        
         public override void SetaSelecao(bool s, bool MostraGrip, bool SelecaoDeCandidato=false, TObjetoDesenho Owner = null)
         {
             if (this.layer.Congelado || this.layer.Travado)
                return;

             this.Selecionado = false;
             base.Selecionado = s;
             base.MostrarGrip = MostraGrip;
             ShowHideGrips(MostraGrip);
         }

         public override void AddGrips()
         {
             Grips = new List<TGrip>(3);

             Grips.Add(new TGrip(this, this.pIni.x, this.pIni.y, false, false, false, true, this.layer));

             Grips.Add(new TGrip(this, this.getMiddlePoint().x, this.getMiddlePoint().y, true, false, false, false, this.layer));

             Grips.Add(new TGrip(this, this.pFin.x, this.pFin.y, false, false, false, true, this.layer));
         }

         public TNoGrelha getMiddlePoint()
         {
             return (pIni + pFin) / 2;
         }

          #endregion
    }
}
