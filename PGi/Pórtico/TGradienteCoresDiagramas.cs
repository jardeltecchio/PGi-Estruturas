using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.NetworkInformation;
using System.Text;
using System.Threading.Tasks;
using System.Web.UI.WebControls;

namespace PG
{

    public class TGradienteCoresDiagramas
    {
        int Ngl;
        TBarraPortico[] barras;
        TPorticoEspacial Portico;
        System.Windows.Forms.Panel p1, p2, p3, p4, p5, p6,
            p7, p8, p9, p10, p11, p12, p13, p14;
        bool somenteSelecionados;
        //Gera as cores em gradiente em todas as barras TBarraPortico para cada tipo de diagrama conforme o usuario
        //vai trocando a combinacao/caso ou o tipo do esforço a ser mostrado na tela
        // Inicializa a classe no metodo PreencheBatchTriangulos do FPrincipal, ou seja, faz o calculo das cores
        // somente quando necessario, quando o usuario clicar no esforço ou mudar o caso/combinacao

        public TGradienteCoresDiagramas(int ngl, TPorticoEspacial _portico, List<System.Windows.Forms.Panel> panelCores, 
            List<System.Windows.Forms.Label> labelCores,
            string tipo, int tipocarga, int caso, int comb, bool sohSelecionados)
        {
            try
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
                Portico = _portico;
                somenteSelecionados = sohSelecionados;

                CriaVetores();
                Preenche_e_Ordena(tipo, tipocarga, caso, comb);

                if (i_MomNegativo > 0)
                    AtribuiCoresEsforcos("neg",tipo + "-", ref Negativos, ref RGB_Negativos, ref ListaRGB_EsforcosNegativos, i_MomNegativo, 7);


                if (i_MomPositivo > 0)
                    AtribuiCoresEsforcos("pos",tipo + "+", ref Positivos, ref RGB_Positivos, ref ListaRGB_EsforcosPositivos, i_MomPositivo, 7);

                double conv;
                if (tipo == "MY" || tipo == "MZ" || tipo == "MX")
                {
                    labelCores[0].Text = tipo + " (" + Portico.gerenciador.ConfiguracoesPGi.CfgProjeto.unidadesProjeto.res_un_forca + "" +
                                        "." + Portico.gerenciador.ConfiguracoesPGi.CfgProjeto.unidadesProjeto.res_un_comprimento + ")";

                    conv = Portico.gerenciador.formDesenho.conversaoForcaResultado / Portico.gerenciador.formDesenho.conversaoComprimentoResultado;
                }
                else
                {
                    labelCores[0].Text = tipo + " (" + Portico.gerenciador.ConfiguracoesPGi.CfgProjeto.unidadesProjeto.res_un_forca + ")";
                    conv = Portico.gerenciador.formDesenho.conversaoForcaResultado;
                }

                string casas = Portico.gerenciador.formDesenho.casas_decimais_resultado;

                if (RGB_Negativos == null)
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
                    labelCores[15].Text = ((RGB_Negativos[0, 0] * conv * -1).ToString(casas));
                    labelCores[14].Text = ((RGB_Negativos[0, 1] * conv * -1).ToString(casas));
                    labelCores[13].Text = ((RGB_Negativos[1, 1] * conv * -1).ToString(casas));
                    labelCores[12].Text = ((RGB_Negativos[2, 1] * conv * -1).ToString(casas));
                    labelCores[11].Text = ((RGB_Negativos[3, 1] * conv * -1).ToString(casas));
                    labelCores[10].Text = ((RGB_Negativos[4, 1] * conv * -1).ToString(casas));
                    labelCores[9].Text = ((RGB_Negativos[5, 1] * conv * -1).ToString(casas));
                }

                if (RGB_Positivos == null)
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
                    labelCores[8].Text = ((RGB_Positivos[6, 1] * conv).ToString(casas));
                    labelCores[7].Text = ((RGB_Positivos[5, 1] * conv).ToString(casas));
                    labelCores[6].Text = ((RGB_Positivos[4, 1] * conv).ToString(casas));
                    labelCores[5].Text = ((RGB_Positivos[3, 1] * conv).ToString(casas));
                    labelCores[4].Text = ((RGB_Positivos[2, 1] * conv).ToString(casas));
                    labelCores[3].Text = ((RGB_Positivos[1, 1] * conv).ToString(casas));
                    labelCores[2].Text = ((RGB_Positivos[0, 1] * conv).ToString(casas));
                    labelCores[1].Text = ((RGB_Positivos[0, 0] * conv).ToString(casas));
                }
            }
            catch(Exception ee)
            {

            }
        }

        public double[] Negativos;
        public double[] Positivos;
        public double[] Deslocamentos;
        public double[,] RGB_Negativos;
        public double[,] RGB_Positivos;
        public double[,] ListaRGB_EsforcosNegativos;
        public double[,] ListaRGB_EsforcosPositivos;

        int i_MomPositivo = 0, i_MomNegativo = 0;

        public System.Windows.Forms.Panel P7 { get => p7; set => p7 = value; }

        void CriaVetores()
        {
            barras = new TBarraPortico[Portico.nBarras];
            Array.Copy(Portico.barras, 1, barras, 0, Portico.nBarras); //copiar a partir do indice 1, pois a primeira barra do array é nula

            Negativos = new double[Ngl * 2];
            Positivos = new double[Ngl * 2];

            ListaRGB_EsforcosNegativos = new double[7, 3];  //7 cores diferentes - 3 são os tons r g b
            ListaRGB_EsforcosPositivos = new double[7, 3];  //7 cores diferentes - 3 são os tons r g b

            #region Cores - Positivos
            ListaRGB_EsforcosPositivos[0, 0] = (double)p14.BackColor.R/255;
            ListaRGB_EsforcosPositivos[0, 1] = (double)p14.BackColor.G/255;
            ListaRGB_EsforcosPositivos[0, 2] = (double)p14.BackColor.B/255;
                                                          
            ListaRGB_EsforcosPositivos[1, 0] = (double)p13.BackColor.R/255;
            ListaRGB_EsforcosPositivos[1, 1] = (double)p13.BackColor.G/255;
            ListaRGB_EsforcosPositivos[1, 2] = (double)p13.BackColor.B/255;
                                                          
            ListaRGB_EsforcosPositivos[2, 0] = (double)p12.BackColor.R/255;
            ListaRGB_EsforcosPositivos[2, 1] = (double)p12.BackColor.G/255;
            ListaRGB_EsforcosPositivos[2, 2] = (double)p12.BackColor.B/255;
                                                           
            ListaRGB_EsforcosPositivos[3, 0] = (double)p11.BackColor.R/255;
            ListaRGB_EsforcosPositivos[3, 1] = (double)p11.BackColor.G/255;
            ListaRGB_EsforcosPositivos[3, 2] = (double)p11.BackColor.B/255;
                                                            
            ListaRGB_EsforcosPositivos[4, 0] = (double)p10.BackColor.R/255;
            ListaRGB_EsforcosPositivos[4, 1] = (double)p10.BackColor.G/255;
            ListaRGB_EsforcosPositivos[4, 2] = (double)p10.BackColor.B/255;
                                                           
            ListaRGB_EsforcosPositivos[5, 0] = (double)p9.BackColor.R/255;
            ListaRGB_EsforcosPositivos[5, 1] = (double)p9.BackColor.G/255;
            ListaRGB_EsforcosPositivos[5, 2] = (double)p9.BackColor.B/255;

            ListaRGB_EsforcosPositivos[6, 0] = (double)p8.BackColor.R / 255;
            ListaRGB_EsforcosPositivos[6, 1] = (double)p8.BackColor.G / 255;
            ListaRGB_EsforcosPositivos[6, 2] = (double)p8.BackColor.B / 255;
            #endregion

            #region Cores - Negativos
            ListaRGB_EsforcosNegativos[0, 0] = (double)p1.BackColor.R/255; 
            ListaRGB_EsforcosNegativos[0, 1] = (double)p1.BackColor.G/255;
            ListaRGB_EsforcosNegativos[0, 2] = (double)p1.BackColor.B/255;
                                                          
            ListaRGB_EsforcosNegativos[1, 0] = (double)p2.BackColor.R/255; 
            ListaRGB_EsforcosNegativos[1, 1] = (double)p2.BackColor.G/255;
            ListaRGB_EsforcosNegativos[1, 2] = (double)p2.BackColor.B/255;
                                                        
            ListaRGB_EsforcosNegativos[2, 0] = (double)p3.BackColor.R/255; 
            ListaRGB_EsforcosNegativos[2, 1] = (double)p3.BackColor.G/255;
            ListaRGB_EsforcosNegativos[2, 2] = (double)p3.BackColor.B/255;
                                                         
            ListaRGB_EsforcosNegativos[3, 0] = (double)p4.BackColor.R/255; 
            ListaRGB_EsforcosNegativos[3, 1] = (double)p4.BackColor.G/255;
            ListaRGB_EsforcosNegativos[3, 2] = (double)p4.BackColor.B/255;
                                                     
            ListaRGB_EsforcosNegativos[4, 0] = (double)p5.BackColor.R/255; 
            ListaRGB_EsforcosNegativos[4, 1] = (double)p5.BackColor.G/255;
            ListaRGB_EsforcosNegativos[4, 2] = (double)p5.BackColor.B/255;
                                                        
            ListaRGB_EsforcosNegativos[5, 0] = (double)p6.BackColor.R/255; 
            ListaRGB_EsforcosNegativos[5, 1] = (double)p6.BackColor.G/255;
            ListaRGB_EsforcosNegativos[5, 2] = (double)p6.BackColor.B/255;

            ListaRGB_EsforcosNegativos[6, 0] = (double)p7.BackColor.R / 255;
            ListaRGB_EsforcosNegativos[6, 1] = (double)p7.BackColor.G / 255;
            ListaRGB_EsforcosNegativos[6, 2] = (double)p7.BackColor.B / 255;
            #endregion

            double[,] ListaRGB_EsforcosNegativos_aux = new double[6, 3];

            ListaRGB_EsforcosNegativos_aux = ListaRGB_EsforcosNegativos;
            
         //   ListaRGB_EsforcosNegativos = ListaRGB_EsforcosPositivos;
          //  ListaRGB_EsforcosPositivos = ListaRGB_EsforcosNegativos_aux;

        }
        double alfa;
        double[] posicao = new double[4];
        public double[] Esforcos;
        vec3 z_local;
        void Preencher(TBarraPortico bar, int g1, int g2, int tipocarga, int caso, int comb)
        {
            double pos_ini, pos_fin;

            if (g1 != 1 && g1 != 2 && g1 != 3)
            {
                if (tipocarga == 0)
                {
                    Esforcos = bar.casos_x_esforcos[caso].Esforcos;
                }
                else
                {
                    Esforcos = bar.combinacoes_x_esforcos[comb].Esforcos;
                }

                if (g1 == 6)
                {
                    if (bar.sinal_my_ini > 0)
                        Positivos[i_MomPositivo++] = Math.Abs(Esforcos[g1]);
                    else
                    if (bar.sinal_my_ini < 0)
                        Negativos[i_MomNegativo++] = Math.Abs(Esforcos[g1]);
                    else
                    if (bar.sinal_my_ini == 0)
                        Negativos[i_MomNegativo++] = 0;

                    if (bar.sinal_my_fin > 0)
                        Positivos[i_MomPositivo++] = Math.Abs(Esforcos[g2]);
                    else
                    if (bar.sinal_my_fin < 0)
                        Negativos[i_MomNegativo++] = Math.Abs(Esforcos[g2]);
                    else
                    if (bar.sinal_my_fin == 0)
                        Negativos[i_MomNegativo++] = 0;
                }
                else
                if (g1 == 5)
                {
                    if (bar.sinal_mz_ini > 0)
                        Positivos[i_MomPositivo++] = Math.Abs(Esforcos[g1]);
                    else
                    if (bar.sinal_mz_ini < 0)
                        Negativos[i_MomNegativo++] = Math.Abs(Esforcos[g1]);
                    else
                    if (bar.sinal_mz_ini == 0)
                        Negativos[i_MomNegativo++] = 0;

                    if (bar.sinal_mz_fin > 0)
                        Positivos[i_MomPositivo++] = Math.Abs(Esforcos[g2]);
                    else
                    if (bar.sinal_mz_fin < 0)
                        Negativos[i_MomNegativo++] = Math.Abs(Esforcos[g2]);
                    else
                    if (bar.sinal_mz_fin == 0)
                        Negativos[i_MomNegativo++] = 0;
                }
                else
                if (g1 == 4)
                {
                    if (bar.sinal_mx_ini > 0)
                        Positivos[i_MomPositivo++] = Math.Abs(Esforcos[g1]);
                    else
                    if (bar.sinal_mx_ini < 0)
                        Negativos[i_MomNegativo++] = Math.Abs(Esforcos[g1]);
                    else
                    if (bar.sinal_mx_ini == 0)
                        Negativos[i_MomNegativo++] = 0;

                    if (bar.sinal_mx_fin > 0)
                        Positivos[i_MomPositivo++] = Math.Abs(Esforcos[g2]);
                    else
                    if (bar.sinal_mx_fin < 0)
                        Negativos[i_MomNegativo++] = Math.Abs(Esforcos[g2]);
                    else
                    if (bar.sinal_mx_fin == 0)
                        Negativos[i_MomNegativo++] = 0;
                }

                /*  pos_ini = Esforcos[g1] * -1;
                  pos_fin = Esforcos[g2];

                  if (bar.barraOriginal_Vertical)
                      if (bar.pIni.z * -1 < bar.pFin.z * -1)
                          pos_ini *= -1;

                  if (g1 == 5) // mz
                  {
                      if (!Geom.Iguais(bar.pIni.x, bar.pFin.x))
                      {
                          if (bar.pIni.x > bar.pFin.x)
                          {
                              pos_ini *= -1;
                              pos_fin *= -1;
                          }
                      }
                  }

                  if (bar.barraOriginal_Vertical)
                      if (bar.pIni.z * -1 < bar.pFin.z * -1)
                          pos_fin *= -1;

                  if (pos_fin > 0)
                      Positivos[i_MomPositivo++] = Math.Abs(Esforcos[g2]);
                  else
                      Negativos[i_MomNegativo++] = Math.Abs(Esforcos[g2]);*/
            }
            else
            {
                if (tipocarga == 0)
                {
                    Esforcos = bar.casos_x_esforcos[caso].Esforcos;
                }
                else
                {
                    Esforcos = bar.combinacoes_x_esforcos[comb].Esforcos;
                }

                if (g1 == 2) //fz
                {
                    pos_ini = Esforcos[g1]*-1;
                    pos_fin = Esforcos[g2];

                    if (bar.sinal_fz_ini > 0)
                        Positivos[i_MomPositivo++] = Math.Abs(Esforcos[g1]);
                    else
                    if (bar.sinal_fz_ini < 0)
                        Negativos[i_MomNegativo++] = Math.Abs(Esforcos[g1]);
                    else
                    if (bar.sinal_fz_ini == 0)
                        Negativos[i_MomNegativo++] = 0;

                    if (bar.sinal_fz_fin > 0)
                        Positivos[i_MomPositivo++] = Math.Abs(Esforcos[g2]);
                    else
                    if (bar.sinal_fz_fin < 0)
                        Negativos[i_MomNegativo++] = Math.Abs(Esforcos[g2]);
                    else
                    if (bar.sinal_fz_fin == 0)
                        Negativos[i_MomNegativo++] = 0;
                }
                if (g1 == 1)  // fx
                {
                    if (bar.sinal_fx_ini > 0)
                        Positivos[i_MomPositivo++] = Math.Abs(Esforcos[g1]);
                    else
                    if (bar.sinal_fx_ini < 0)
                        Negativos[i_MomNegativo++] = Math.Abs(Esforcos[g1]);
                    else
                    if (bar.sinal_fx_ini == 0)
                        Negativos[i_MomNegativo++] = 0;

                    if (bar.sinal_fx_fin > 0)
                        Positivos[i_MomPositivo++] = Math.Abs(Esforcos[g2]);
                    else
                    if (bar.sinal_fx_fin < 0)
                        Negativos[i_MomNegativo++] = Math.Abs(Esforcos[g2]);
                    else
                    if (bar.sinal_fx_fin == 0)
                        Negativos[i_MomNegativo++] = 0;
                }
                else //fy
                if (g1 == 3) 
                {
                    if (bar.sinal_fy_ini > 0)
                        Positivos[i_MomPositivo++] = Math.Abs(Esforcos[g1]);
                    else
                    if (bar.sinal_fy_ini < 0)
                        Negativos[i_MomNegativo++] = Math.Abs(Esforcos[g1]);
                    else
                    if (bar.sinal_fy_ini == 0)
                        Negativos[i_MomNegativo++] = 0;

                    if (bar.sinal_fy_fin > 0)
                        Positivos[i_MomPositivo++] = Math.Abs(Esforcos[g2]);
                    else
                    if (bar.sinal_fy_fin < 0)
                        Negativos[i_MomNegativo++] = Math.Abs(Esforcos[g2]);
                    else
                    if (bar.sinal_fy_fin == 0)
                        Negativos[i_MomNegativo++] = 0;
                }

            }
        }
        public bool fx_ini_negativo, fx_fin_negativo;
        void Preenche_e_Ordena(string esforco, int tipocarga, int caso, int comb)
        {
            double temp;
            int j, k;

            if (esforco == "MY")
            {
                foreach (TBarraPortico b in barras)
                    if ((somenteSelecionados && b.barraOriginal.Selecionado) || (!somenteSelecionados))
                        Preencher(b, 6, 12, tipocarga, caso, comb);
            }
            else
            if (esforco == "MZ")
            {
                foreach (TBarraPortico b in barras)
                    if ((somenteSelecionados && b.barraOriginal.Selecionado) || (!somenteSelecionados))
                        Preencher(b, 5, 11, tipocarga, caso, comb);
            }
            else
            if (esforco == "MX")
            {
                foreach (TBarraPortico b in barras)
                    if ((somenteSelecionados && b.barraOriginal.Selecionado) || (!somenteSelecionados))
                        Preencher(b, 4, 10, tipocarga, caso, comb);
            }
            else
            if (esforco == "FX")
            {
                foreach (TBarraPortico b in barras)
                    if ((somenteSelecionados && b.barraOriginal.Selecionado) || (!somenteSelecionados))
                        Preencher(b, 1, 7, tipocarga, caso, comb);
            }
            else
            if (esforco == "FZ")
            {
                foreach (TBarraPortico b in barras)
                    if ((somenteSelecionados && b.barraOriginal.Selecionado) || (!somenteSelecionados))
                        Preencher(b, 2, 8, tipocarga, caso, comb);
            }
            else
            if (esforco == "FY")
            {
                foreach (TBarraPortico b in barras)
                    if ((somenteSelecionados && b.barraOriginal.Selecionado) || (!somenteSelecionados))
                        Preencher(b, 3, 9, tipocarga, caso, comb);
            }

            Array.Sort(Positivos, 0, i_MomPositivo);
            Array.Reverse(Positivos, 0, i_MomPositivo);

            Array.Sort(Negativos, 0, i_MomNegativo);
            Array.Reverse(Negativos, 0, i_MomNegativo);
        }

        void AtribuiCoresEsforcos(string tipo,string NomeEsforco, ref double[] Esforco, ref double[,] RGB, ref double[,] ListaRGB_Esforco, int iTotalEsforcos, int iTotalCores)
        {
            if (iTotalEsforcos > 0)
            {
                RGB = new double[iTotalCores, 5];

                double max = Esforco[0];
                double min = Esforco[iTotalEsforcos - 1];
                double dif = max - min;
                double intervalo = dif / iTotalCores;
                if (Geom.Iguais(dif, 0))
                {
                    for (int i = 0; i < iTotalCores; i++)
                    {
                        RGB[i, 0] = max;
                        RGB[i, 1] = min;

                        RGB[i, 2] = ListaRGB_Esforco[0, 0];
                        RGB[i, 3] = ListaRGB_Esforco[0, 1];
                        RGB[i, 4] = ListaRGB_Esforco[0, 2];
                    }
                }
                else
                {
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
                }

                if (tipo == "neg")
                {
                    Gerenciador.RGB_EsforcosNegativos = new double[iTotalCores, 5];
                    Gerenciador.RGB_EsforcosNegativos = RGB;
                }
                else
                {
                    Gerenciador.RGB_EsforcosPositivos = new double[iTotalCores, 5];
                    Gerenciador.RGB_EsforcosPositivos = RGB;
                }
            }
        }
    }
}
