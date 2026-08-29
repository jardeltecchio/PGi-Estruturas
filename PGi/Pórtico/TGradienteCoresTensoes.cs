using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Media.Media3D;
using Win32Interop.Enums;

namespace PG
{
    public struct NoIsoBanda
    {
        public vec3 posicao;
        public double tensao;

        public NoIsoBanda(vec3 _posicao, double v)
        {
            posicao = _posicao;
            tensao = v;
        }
    }

    public struct ArestaIsoBanda
    {
        public int N1;
        public int N2;

        public ArestaIsoBanda(int n1, int n2)
        {
            N1 = n1;
            N2 = n2;
        }
    }

    public static class PolygonUtils3D
    {
        /// <summary>
        /// Remove vértices colineares/redundantes
        /// de um polígono 3D coplanar.
        /// </summary>
        public static void RemoveCollinear(
            List<NoIsoBanda> poly,
            float eps = 1e-6f)
        {
            if (poly == null)
                return;

            if (poly.Count < 3)
                return;

            int i = 0;

            while (i < poly.Count)
            {
                vec3 prev =
                    poly[(i - 1 + poly.Count) % poly.Count].posicao;

                vec3 curr =
                    poly[i].posicao;

                vec3 next =
                    poly[(i + 1) % poly.Count].posicao;

                vec3 v1 = curr - prev;
                vec3 v2 = next - curr;

                // Produto vetorial
                vec3 cross = v1.CrossProduct(v2);
                
                // Área local ~ magnitude do cross
                double area = cross.Magnitude();

                double sl1 = v1.SquaredLength();
                double sl2 = v2.SquaredLength();
                // Segmentos degenerados
                bool short1 = sl1 < eps * eps;
                bool short2 = sl2 < eps * eps;

                // Colinearidade
                if (area < eps || short1 || short2)
                {
                    poly.RemoveAt(i);

                    if (poly.Count < 3)
                        break;

                    continue;
                }

                i++;
            }
        }
    }

    public struct Quad3D
    {
        public vec3 P1; // top-left
        public vec3 P2; // top-right
        public vec3 P3; // bottom-right
        public vec3 P4; // bottom-left

        public double S1;
        public double S2;
        public double S3;
        public double S4;
    }

    public struct QuadFatia3D
    {
        public NoIsoBanda P1;
        public NoIsoBanda P2;
        public NoIsoBanda P3;
        public NoIsoBanda P4;
        
        public ArestaIsoBanda aresta1;
        public ArestaIsoBanda aresta2;
        public ArestaIsoBanda aresta3;
        public ArestaIsoBanda aresta4;

        public double S1;
        public double S2;
        public double S3;
        public double S4;
    }

    public static class QuadSubdivisao
    {
        /// <summary>
        /// Subdivide verticalmente um retângulo Q4
        /// em várias fatias menores.
        /// </summary>
        /// 

        //aqui é quando por exemplo o momento cruza em zero na barra, os sinais das tensoes se alternam nos cantos
        public static bool TemSinaisAlternados(
            double s1,
            double s2,
            double s3,
            double s4)
        {
            bool p1Positive = s1 >= 0.0;
            bool p2Positive = s2 >= 0.0;
            bool p3Positive = s3 >= 0.0;
            bool p4Positive = s4 >= 0.0;

            // Caso:
            // + -
            // - +
            bool case1 =
                p1Positive &&
                !p2Positive &&
                p3Positive &&
                !p4Positive;

            // Caso:
            // - +
            // + -
            bool case2 =
                !p1Positive &&
                p2Positive &&
                !p3Positive &&
                p4Positive;

            return case1 || case2;
        }

        public static List<QuadFatia3D> SubdivideVertical(Quad3D q, int slices)
        {
            List<QuadFatia3D> result = new List<QuadFatia3D>();

            for (int i = 0; i < slices; i++)
            {
                double u0 = (double)i / slices;
                double u1 = (double)(i + 1) / slices;

                // Linha superior
                vec3 topLeft =
                    Lerp(q.P1, q.P2, u0);

                vec3 topRight =
                    Lerp(q.P1, q.P2, u1);

                // Linha inferior
                vec3 bottomLeft =
                    Lerp(q.P4, q.P3, u0);

                vec3 bottomRight =
                    Lerp(q.P4, q.P3, u1);

                // Tensões interpoladas
                double sTopLeft =
                    Lerp(q.S1, q.S2, u0);

                double sTopRight =
                    Lerp(q.S1, q.S2, u1);

                double sBottomLeft =
                    Lerp(q.S4, q.S3, u0);

                double sBottomRight =
                    Lerp(q.S4, q.S3, u1);

                QuadFatia3D slice = new QuadFatia3D()
                {
                    P1 = new NoIsoBanda(topLeft, sTopLeft),
                    P2 = new NoIsoBanda(topRight, sTopRight),
                    P3 = new NoIsoBanda(bottomRight, sBottomRight),
                    P4 = new NoIsoBanda(bottomLeft, sBottomLeft),

                    S1 = sTopLeft,
                    S2 = sTopRight,
                    S3 = sBottomRight,
                    S4 = sBottomLeft
                };

                result.Add(slice);
            }

            return result;
        }

        // -----------------------------------------------------
        // INTERPOLAÇÃO VECTOR3
        // -----------------------------------------------------

        private static vec3 Lerp(
            vec3 a,
            vec3 b,
            double t)
        {
            return new vec3(
                (float)(a.x + (b.x - a.x) * t),
                (float)(a.y + (b.y - a.y) * t),
                (float)(a.z + (b.z - a.z) * t));
        }

        // -----------------------------------------------------
        // INTERPOLAÇÃO ESCALAR
        // -----------------------------------------------------

        private static double Lerp(
            double a,
            double b,
            double t)
        {
            return a + (b - a) * t;
        }
    }


    public static class QuadIsoBanda
    {
        const double EPS = 1e-12;

        //Teste de interseção na aresta
        public static bool TemInterseccao(double vA, double vB, double L)
        {
            double min = Math.Min(vA, vB);
            double max = Math.Max(vA, vB);

            return (L >= min - EPS && L <= max + EPS && Math.Abs(vA - vB) > EPS);
        }

        //Interpolação
        static NoIsoBanda Interp(NoIsoBanda a, NoIsoBanda b, double nivel)
        {
            double t = (nivel - a.tensao) / (b.tensao - a.tensao);

            return new NoIsoBanda(new vec3(
                a.posicao.x + t * (b.posicao.x - a.posicao.x),
                a.posicao.y + t * (b.posicao.y - a.posicao.y),
                a.posicao.z + t * (b.posicao.z - a.posicao.z)),
                nivel
            );
        }
        // Clipping inferior (σ ≥ Lmin)
        static List<NoIsoBanda> ClipMin(List<NoIsoBanda> poly, double Lmin)
        {
            var output = new List<NoIsoBanda>();

            for (int i = 0; i < poly.Count; i++)
            {
                var A = poly[i];
                var B = poly[(i + 1) % poly.Count];

                bool Ain = A.tensao >= Lmin;
                bool Bin = B.tensao >= Lmin;

                if (Ain && Bin)
                {
                    output.Add(B);
                }
                else if (Ain && !Bin)
                {
                    if (TemInterseccao(A.tensao, B.tensao, Lmin))
                    {
                        output.Add(Interp(A, B, Lmin));
                    }
                }
                else if (!Ain && Bin)
                {
                    if (TemInterseccao(A.tensao, B.tensao, Lmin))
                    {
                        NoIsoBanda no_intersec = Interp(A, B, Lmin);
                        output.Add(no_intersec);
                    }

                    output.Add(B);
                }
            }

            return output;
        }

        static List<NoIsoBanda> ClipMax(List<NoIsoBanda> poly, double Lmax)
        {
            var output = new List<NoIsoBanda>();

            for (int i = 0; i < poly.Count; i++)
            {
                var A = poly[i];
                var B = poly[(i + 1) % poly.Count];

                bool Ain = A.tensao <= Lmax;
                bool Bin = B.tensao <= Lmax;

                if (Ain && Bin)
                {
                    output.Add(B);
                }
                else if (Ain && !Bin)
                {
                    if (TemInterseccao(A.tensao, B.tensao, Lmax))
                        output.Add(Interp(A, B, Lmax));
                }
                else if (!Ain && Bin)
                {
                    if (TemInterseccao(A.tensao, B.tensao, Lmax))
                        output.Add(Interp(A, B, Lmax));

                    output.Add(B);
                }
            }

            return output;
        }

        //Sutherland–Hodgman FEM sobre um quadrilátero. (fiz um marching squares simplificado)
        public static List<NoIsoBanda> ProcessarIsoBanda(NoIsoBanda n0, NoIsoBanda n1, NoIsoBanda n2, NoIsoBanda n3, double Lmin, double Lmax)
        {
            var poly = new List<NoIsoBanda> { n0, n1, n2, n3 };
         
            /*  foreach (var ss in poly)
            {
                ss.posicao.y *= -1;
                ss.posicao.z *= -1;
            }
            */
            poly = ClipMin(poly, Lmin);
            if (poly.Count == 0) return poly;

            poly = ClipMax(poly, Lmax);
            return poly;
        }

    }

    public class TGradienteCoresTensoes
    {
        int Ngl;
        TBarraPortico[] barras;
        TPorticoEspacial Portico;
        System.Windows.Forms.Panel p1, p2, p3, p4, p5, p6,
            p7, p8, p9, p10, p11, p12, p13, p14;
        bool somenteSelecionados;
        public TGradienteCoresTensoes(int ngl, TPorticoEspacial _portico, 
                               List<System.Windows.Forms.Panel> panelCores,
                               List<System.Windows.Forms.Label> labelCores,
                               int tipocarga, int caso, int comb, bool sohSelecionados, bool isobandas)
        {
            Ngl = ngl;
            p1 = panelCores[0];
            p2 = panelCores[1];
            p3 = panelCores[2];
            p4 = panelCores[3];
            p5 = panelCores[4];
            p6 = panelCores[5];
            p7 = panelCores[6];
            p8 = panelCores[7];
            p9 = panelCores[8];
            p10 = panelCores[9];
            p11 = panelCores[10];
            p12 = panelCores[11];
            p13 = panelCores[12];
            p14 = panelCores[13];
            somenteSelecionados = sohSelecionados;
            
            Portico = _portico;

            CriaVetores();
            Preencher(tipocarga, caso, comb);
           
            bool temPositivo = maxPositivo > double.MinValue;
            bool temNegativo = minNegativo < double.MaxValue;
            //   Ordenar(tipocarga, caso, comb);

            if (temNegativo)
            {
                double[] tensao_neg_aux = new double[i_TensaoNegativa];

                Array.Copy(Negativos, 0, tensao_neg_aux, 0, i_TensaoNegativa);
                
                Negativos = new double[i_TensaoNegativa];
                Array.Copy(tensao_neg_aux, 0, Negativos, 0, i_TensaoNegativa); 

                AtribuiCoresTensoes("negativos", ref Negativos, ref RGB_Negativos, ref ListaRGB_Negativos, i_TensaoNegativa, Const.totCoresTensao);
            }

            if (temPositivo)
            {
                double[] tensao_aux = new double[i_TensaoPositiva];

                Array.Copy(Positivos, 0, tensao_aux, 0, i_TensaoPositiva);

                Positivos = new double[i_TensaoPositiva];
                Array.Copy(tensao_aux, 0, Positivos, 0, i_TensaoPositiva); 
            
                AtribuiCoresTensoes("positivos", ref Positivos, ref RGB_Positivos, ref ListaRGB_Positivos, i_TensaoPositiva, Const.totCoresTensao);
            }

            labelCores[0].Text = "Tensões normais\r(My+Mz+Fx) " + Portico.gerenciador.ConfiguracoesPGi.CfgProjeto.unidadesProjeto.res_un_tensao;
            double conv = Portico.gerenciador.formDesenho.conversaoTensaoResultado;

            string casas = Portico.gerenciador.formDesenho.casas_decimais_resultado;

            if (RGB_Positivos == null)
            {
                labelCores[9].Text = "-";
                labelCores[10].Text = "-";
                labelCores[11].Text = "-";
                labelCores[12].Text = "-";
                labelCores[13].Text = "-";
                labelCores[14].Text = "-";
                labelCores[15].Text = "-";
            }
            else
            {
                labelCores[15].Text = ((RGB_Positivos[0, 0] * conv ).ToString(casas));
                labelCores[14].Text = ((RGB_Positivos[0, 1] * conv ).ToString(casas));
                labelCores[13].Text = ((RGB_Positivos[1, 1] * conv ).ToString(casas));
                labelCores[12].Text = ((RGB_Positivos[2, 1] * conv ).ToString(casas));
                labelCores[11].Text = ((RGB_Positivos[3, 1] * conv ).ToString(casas));
                labelCores[10].Text = ((RGB_Positivos[4, 1] * conv ).ToString(casas));
                labelCores[9].Text =  ((RGB_Positivos[5, 1] * conv ).ToString(casas));
                labelCores[8].Text =  ((RGB_Positivos[6, 1] * conv).ToString(casas)); // aqui é sempre zero
            }

            if (RGB_Negativos == null)
            {
                labelCores[1].Text = "-";
                labelCores[2].Text = "-";
                labelCores[3].Text = "-";
                labelCores[4].Text = "-";
                labelCores[5].Text = "-";
                labelCores[6].Text = "-";
                labelCores[7].Text = "-";
                labelCores[8].Text = "-";
            }
            else
            {
                labelCores[1].Text = ((RGB_Negativos[6, 1] * conv).ToString(casas)); // maior compressao
                labelCores[2].Text = ((RGB_Negativos[5, 1] * conv).ToString(casas));
                labelCores[3].Text = ((RGB_Negativos[4, 1] * conv).ToString(casas));
                labelCores[4].Text = ((RGB_Negativos[3, 1] * conv).ToString(casas));
                labelCores[5].Text = ((RGB_Negativos[2, 1] * conv).ToString(casas));
                labelCores[6].Text = ((RGB_Negativos[1, 1] * conv).ToString(casas));
                labelCores[7].Text = ((RGB_Negativos[0, 1] * conv).ToString(casas)); 
                labelCores[8].Text = /*(0).ToString(casas);*/ ((RGB_Negativos[0, 0] * conv).ToString(casas));// aqui é sempre zero
                // o RGB_Negativos[0, 0] contem a menor compressao, mas eu nao mostro na legenda, pois o que dividirá compressao de tração será zero
            }

          //  if (isobandas)
            //  CalculaIsobandas(tipocarga, caso, comb);
        }
        
        List<QuadFatia3D> Quads3D;
        NoIsoBanda?[,] edgeCache;
        int IDBarra;

        public double[] Negativos;
        public double[] Positivos;
        public double[,] RGB_Negativos;
        public double[,] RGB_Positivos;
        public double[,] ListaRGB_Negativos;
        public double[,] ListaRGB_Positivos;
        public double[] Tensoes_i, Tensoes_f;
        int iTensoes;

        double maxPositivo;
        double minPositivo;

        double maxNegativo;
        double minNegativo;
        void Preencher(int tipocarga, int caso, int comb)
        {
            HashSet<double> negativosSet = new HashSet<double>();
            HashSet<double> positivosSet = new HashSet<double>();

            if (somenteSelecionados)
            {
                maxPositivo = double.MinValue;
                minPositivo = double.MaxValue;

                maxNegativo = double.MinValue;
                minNegativo = double.MaxValue;

                foreach (TBarraPortico b in barras)
                {
                    if (b.barraOriginal.Selecionado)
                    {
                        if (tipocarga == 0)
                        {
                            Tensoes_i = b.casos_x_tensoes[caso].tensoes_i;
                            Tensoes_f = b.casos_x_tensoes[caso].tensoes_f;
                        }
                        else
                        {
                            Tensoes_i = b.combinacoes_x_tensoes[comb].tensoes_i;
                            Tensoes_f = b.combinacoes_x_tensoes[comb].tensoes_f;
                        }

                        foreach (double sigma in Tensoes_i)
                        {
                            if (sigma > 0)
                            {
                                if (sigma > maxPositivo) maxPositivo = sigma;
                                if (sigma < minPositivo) minPositivo = sigma;
                            }
                            else if (sigma < 0)
                            {
                                if (sigma > maxNegativo) maxNegativo = sigma;
                                if (sigma < minNegativo) minNegativo = sigma;
                            }
                        }

                        foreach (double sigma in Tensoes_f)
                        {
                            if (sigma > 0)
                            {
                                if (sigma > maxPositivo) maxPositivo = sigma;
                                if (sigma < minPositivo) minPositivo = sigma;
                            }
                            else if (sigma < 0)
                            {
                                if (sigma > maxNegativo) maxNegativo = sigma;
                                if (sigma < minNegativo) minNegativo = sigma;
                            }
                        }
                    }
                }
            }
            else
            if (!somenteSelecionados)
            {
                maxPositivo = double.MinValue;
                minPositivo = double.MaxValue;

                maxNegativo = double.MinValue;
                minNegativo = double.MaxValue;
             
                foreach (TBarraPortico b in barras)
                {                   
                    if (tipocarga == 0)
                    {
                        Tensoes_i = b.casos_x_tensoes[caso].tensoes_i;
                        Tensoes_f = b.casos_x_tensoes[caso].tensoes_f;
                    }
                    else
                    {
                        Tensoes_i = b.combinacoes_x_tensoes[comb].tensoes_i;
                        Tensoes_f = b.combinacoes_x_tensoes[comb].tensoes_f;
                    }

                    foreach (double sigma in Tensoes_i)
                    {
                        if (sigma > 0)
                        {
                            if (sigma > maxPositivo) maxPositivo = sigma;
                            if (sigma < minPositivo) minPositivo = sigma;
                        }
                        else if (sigma < 0)
                        {
                            if (sigma > maxNegativo) maxNegativo = sigma;
                            if (sigma < minNegativo) minNegativo = sigma;
                        }
                    }

                    foreach (double sigma in Tensoes_f)
                    {
                        if (sigma > 0)
                        {
                            if (sigma > maxPositivo) maxPositivo = sigma;
                            if (sigma < minPositivo) minPositivo = sigma;
                        }
                        else if (sigma < 0)
                        {
                            if (sigma > maxNegativo) maxNegativo = sigma;
                            if (sigma < minNegativo) minNegativo = sigma;
                        }
                    }                     
                }
                

              /*  if (tipocarga == 1)
                {
                    maxPositivo = double.MinValue;
                    minPositivo = double.MaxValue;

                    maxNegativo = double.MinValue;
                    minNegativo = double.MaxValue;

                    foreach (TBarraPortico b in barras)
                    {
                        if (b.barraOriginal.Visivel)
                        {
                            if (tipocarga == 0)
                            {
                                Tensoes_i = b.combinacoes_x_tensoes[caso].tensoes_i;
                                Tensoes_f = b.combinacoes_x_tensoes[caso].tensoes_f;
                            }
                            else
                            {
                                Tensoes_i = b.combinacoes_x_tensoes[comb].tensoes_i;
                                Tensoes_f = b.combinacoes_x_tensoes[comb].tensoes_f;
                            }

                            foreach (double sigma in Tensoes_i)
                            {
                                if (sigma > 0)
                                {
                                    if (sigma > maxPositivo) maxPositivo = sigma;
                                    if (sigma < minPositivo) minPositivo = sigma;
                                }
                                else if (sigma < 0)
                                {
                                    if (sigma > maxNegativo) maxNegativo = sigma;
                                    if (sigma < minNegativo) minNegativo = sigma;
                                }
                            }

                            foreach (double sigma in Tensoes_f)
                            {
                                if (sigma > 0)
                                {
                                    if (sigma > maxPositivo) maxPositivo = sigma;
                                    if (sigma < minPositivo) minPositivo = sigma;
                                }
                                else if (sigma < 0)
                                {
                                    if (sigma > maxNegativo) maxNegativo = sigma;
                                    if (sigma < minNegativo) minNegativo = sigma;
                                }
                            }
                        }
                    }
                }*/
            }
        }

        void AtribuiCoresTensoes(string tipo, ref double[] Esforco, ref double[,] RGB, ref double[,] ListaRGB_Esforco, int iTotal, int iTotalCores)
        {
          ///  if (iTotal > 0)
            {
                RGB = new double[iTotalCores, 5];

                double max = 0;
                double min = 0;
                double dif;
                double intervalo;

                if (tipo == "negativos")
                {
                    max = maxNegativo;
                    min = minNegativo;
                }
                else
                if (tipo == "positivos")
                {
                    max = maxPositivo;
                    min = minPositivo;
                }

                dif = max - min;
                intervalo = dif / iTotalCores;

                for (int i = 0; i < iTotalCores; i++)
                    {
                        //Intervalo
                        RGB[i, 0] = max - (intervalo * i);
                        RGB[i, 1] = max - (intervalo * (i + 1));

                        //Cores
                        RGB[i, 2] = ListaRGB_Esforco[i, 0];
                        RGB[i, 3] = ListaRGB_Esforco[i, 1];
                        RGB[i, 4] = ListaRGB_Esforco[i, 2];
                    }
                
                Gerenciador.corSemTensao = p8.BackColor;

                if (tipo == "negativos")
                {
                    RGB[0, 0] = 0;
                    Gerenciador.RGB_TensoesNormaisNegativas = new double[iTotalCores, 5];
                    Gerenciador.RGB_TensoesNormaisNegativas = RGB;

                  /*  StringBuilder sb = new StringBuilder();

                    sb.AppendLine("RGB_TensoesNormaisNegativas");
                    sb.AppendLine();

                    for (int i = 0; i < 7; i++)
                    {
                        sb.AppendLine(
                            i.ToString() + " : " +
                            Gerenciador.RGB_TensoesNormaisNegativas[i, 0].ToString("G17") +
                            "    " +
                            Gerenciador.RGB_TensoesNormaisNegativas[i, 1].ToString("G17"));
                    }


                    System.IO.File.WriteAllText(@"C:\PGi\faixas 1.txt", sb.ToString());*/
                    /*for (int i = 1; i <= Portico.nBarras; i++)
                    {
                        if (Portico.barras[i].barraRigida) continue;

                        Portico.barras[i].RGB_TensaoNormalNegativos = new double[iTotalCores, 5];
                        Portico.barras[i].RGB_TensaoNormalNegativos = RGB;

                    }*/
                }
                else
                {
                    RGB[6, 1] = 0;

                    Gerenciador.RGB_TensoesNormaisPositivas = new double[iTotalCores, 5];
                    Gerenciador.RGB_TensoesNormaisPositivas = RGB;

                  /*  StringBuilder sb = new StringBuilder();

                    sb.AppendLine("RGB_TensoesNormaisPositivas");
                    sb.AppendLine();

                    for (int i = 0; i < 7; i++)
                    {
                        sb.AppendLine(
                            i.ToString() + " : " +
                            Gerenciador.RGB_TensoesNormaisPositivas[i, 0].ToString("G17") +
                            "    " +
                            Gerenciador.RGB_TensoesNormaisPositivas[i, 1].ToString("G17"));
                    }

                    System.IO.File.WriteAllText(@"C:\PGi\faixas 2.txt", sb.ToString());*/
                    /*  for (int i = 1; i <= Portico.nBarras; i++)
                      {
                          if (Portico.barras[i].barraRigida) continue;

                          Portico.barras[i].RGB_TensaoNormalPositivos = new double[iTotalCores, 5];
                          Portico.barras[i].RGB_TensaoNormalPositivos = RGB;

                      }*/
                }




            }
        }


        void Ordenar(int tipocarga, int caso, int comb)
        {
            double temp;
            int j, k;
/*
            for (j = 0; j < i_TensaoPositiva; j++)
            {
                for (k = j; k < i_TensaoPositiva; k++)
                {
                    if (Positivos[k] > Positivos[j])
                    {
                        temp = Positivos[j];
                        Positivos[j] = Positivos[k];
                        Positivos[k] = temp;
                    }
                }
            }
            */
            Array.Sort(Positivos, 0, i_TensaoPositiva);
            Array.Reverse(Positivos, 0, i_TensaoPositiva);

            Array.Sort(Negativos, 0, i_TensaoNegativa);
            Array.Reverse(Negativos, 0, i_TensaoNegativa);
/*
            for (j = 0; j < i_TensaoNegativa; j++)
            {
                for (k = j; k < i_TensaoNegativa; k++)
                {
                    if (Negativos[k] > Negativos[j])
                    {
                        temp = Negativos[j];
                        Negativos[j] = Negativos[k];
                        Negativos[k] = temp;
                    }
                }
            }*/
        }

        public System.Windows.Forms.Panel P7 { get => p7; set => p7 = value; }

        void CriaVetores()
        {
            barras = new TBarraPortico[Portico.nBarras];
            Array.Copy(Portico.barras, 1, barras, 0, Portico.nBarras); //copiar a partir do indice 1, pois a primeira barra do array é nula

            int numCoords = 0;
            int numbar = 0;
            if (somenteSelecionados)
            {
                for (int i = 0; i < barras.Count(); i++)
                {
                    if (barras[i].barraOriginal.Selecionado)
                    {

                        numCoords += ((barras[i].Dados.secaoSemRotacao.poligonos.Find(o=>o.externo).coords.Count() - 1) * 2);
                    }
                }
            }
            else
            {
                for (int i = 0; i < barras.Count(); i++)
                {
                    //        if (barras[i].barraOriginal.Visivel)
                    {

                        numCoords += ((barras[i].Dados.secaoSemRotacao.poligonos.Find(o => o.externo).coords.Count() - 1) * 2);
                    }
                }
            }

            Negativos = new double[numCoords];
            Positivos = new double[numCoords];

            ListaRGB_Negativos = new double[7, 3];  //7 cores dixferentes - 3 são os tons r g b
            ListaRGB_Positivos = new double[7, 3];  //7 cores diferentes - 3 são os tons r g b

            ListaRGB_Positivos[0, 0] = (double)p1.BackColor.R / 255;  // tração mais forte - vermelho escuro
            ListaRGB_Positivos[0, 1] = (double)p1.BackColor.G / 255;
            ListaRGB_Positivos[0, 2] = (double)p1.BackColor.B / 255;

            ListaRGB_Positivos[1, 0] = (double)p2.BackColor.R / 255;
            ListaRGB_Positivos[1, 1] = (double)p2.BackColor.G / 255;
            ListaRGB_Positivos[1, 2] = (double)p2.BackColor.B / 255;

            ListaRGB_Positivos[2, 0] = (double)p3.BackColor.R / 255;
            ListaRGB_Positivos[2, 1] = (double)p3.BackColor.G / 255;
            ListaRGB_Positivos[2, 2] = (double)p3.BackColor.B / 255;

            ListaRGB_Positivos[3, 0] = (double)p4.BackColor.R / 255;
            ListaRGB_Positivos[3, 1] = (double)p4.BackColor.G / 255;
            ListaRGB_Positivos[3, 2] = (double)p4.BackColor.B / 255;

            ListaRGB_Positivos[4, 0] = (double)p5.BackColor.R / 255;
            ListaRGB_Positivos[4, 1] = (double)p5.BackColor.G / 255;
            ListaRGB_Positivos[4, 2] = (double)p5.BackColor.B / 255;

            ListaRGB_Positivos[5, 0] = (double)p6.BackColor.R / 255;
            ListaRGB_Positivos[5, 1] = (double)p6.BackColor.G / 255;
            ListaRGB_Positivos[5, 2] = (double)p6.BackColor.B / 255;

            ListaRGB_Positivos[6, 0] = (double)p7.BackColor.R / 255;
            ListaRGB_Positivos[6, 1] = (double)p7.BackColor.G / 255;
            ListaRGB_Positivos[6, 2] = (double)p7.BackColor.B / 255;  // tração mais fraca
           

            
            ListaRGB_Negativos[6, 0] = (double)p14.BackColor.R / 255;   // compressao mais forte- azul escuro
            ListaRGB_Negativos[6, 1] = (double)p14.BackColor.G / 255;
            ListaRGB_Negativos[6, 2] = (double)p14.BackColor.B / 255;

            ListaRGB_Negativos[5, 0] = (double)p13.BackColor.R / 255;
            ListaRGB_Negativos[5, 1] = (double)p13.BackColor.G / 255;
            ListaRGB_Negativos[5, 2] = (double)p13.BackColor.B / 255;

            ListaRGB_Negativos[4, 0] = (double)p12.BackColor.R / 255;
            ListaRGB_Negativos[4, 1] = (double)p12.BackColor.G / 255;
            ListaRGB_Negativos[4, 2] = (double)p12.BackColor.B / 255;

            ListaRGB_Negativos[3, 0] = (double)p11.BackColor.R / 255;
            ListaRGB_Negativos[3, 1] = (double)p11.BackColor.G / 255;
            ListaRGB_Negativos[3, 2] = (double)p11.BackColor.B / 255;

            ListaRGB_Negativos[2, 0] = (double)p10.BackColor.R / 255;
            ListaRGB_Negativos[2, 1] = (double)p10.BackColor.G / 255;
            ListaRGB_Negativos[2, 2] = (double)p10.BackColor.B / 255;

            ListaRGB_Negativos[1, 0] = (double)p9.BackColor.R / 255;
            ListaRGB_Negativos[1, 1] = (double)p9.BackColor.G / 255;
            ListaRGB_Negativos[1, 2] = (double)p9.BackColor.B / 255;

            ListaRGB_Negativos[0, 0] = (double)p8.BackColor.R / 255;
            ListaRGB_Negativos[0, 1] = (double)p8.BackColor.G / 255;
            ListaRGB_Negativos[0, 2] = (double)p8.BackColor.B / 255; // compressao mais fraca
 
        }
        double sigma_mx, sigma_my= 0;
        double Sigma(double iy, double iz, double area, double prod_inercia, double my, double mz, double fx, double xp, double yp )
        {
            /* deltax = (((prod_inercia * my) + (iy * mz)) / (iy * iz - (prod_inercia * prod_inercia)))/100;
             deltay = ((iz * my) + (prod_inercia * mz)) / (iy * iz - (prod_inercia * prod_inercia)) / 100;

             return ((deltax * xp)*-1) + (deltay * yp) *(-1);*/
            sigma_mx = (my * yp) / iy;
            sigma_my = (mz * xp) / iz;

            return (fx / area) - sigma_mx - sigma_my;


        }

        int i_TensaoPositiva = 0, i_TensaoNegativa = 0;

        TSecao secaoCopia_i, secaoCopia_f;
        //Calcula as tensoes normais nas duas extremidades da barra em cada vertice do poligono da secao


        bool TensaoNegativaInsertida(double tensao)
        {
            for (int i = 0; i < i_TensaoNegativa; i++)
                if (Geom.SaoIguais(Negativos[i], tensao))
                    return true;

            return false;   
        }
        bool TensaoPositivaInsertida(double tensao)
        {
            for (int i = 0; i < i_TensaoPositiva; i++)
                if (Geom.SaoIguais(Positivos[i], tensao))
                    return true;

            return false;
        }
    }


}
