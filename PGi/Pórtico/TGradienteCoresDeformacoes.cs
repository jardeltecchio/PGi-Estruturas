using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Documents;

namespace PG
{
    public class TGradienteCoresDeformacao
    {
        int Ngl;
        TBarraPortico[] barras;
        TPorticoEspacial Portico;
        System.Windows.Forms.Panel p1, p2, p3, p4, p5, p6,
            p7, p8, p9, p10, p11, p12, p13, p14;
        public bool somenteSelecionados;
        //Gera as cores em gradiente em todas as barras TBarraPortico conforme o usuario
        //vai trocando a combinacao/caso ou quando o usuario clica no botao das deformacoes
        // Inicializa a classe no metodo PreencheBatchTriangulos do FPrincipal, ou seja, faz o calculo das cores
        // somente quando necessario, quando o usuario clicar no botao de deformacoes ou mudar o caso/combinacao

        public TGradienteCoresDeformacao(int ngl, TPorticoEspacial _portico, 
            List<System.Windows.Forms.Panel> panelCores,
            List<System.Windows.Forms.Label> labelCores, int tipocarga, int caso, int comb,
            bool sohSelecionados)
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
            Preencher(tipocarga, caso, comb);
            Ordenar(tipocarga, caso, comb);
            AtribuiCoresDeslocamentos(14);

            double conv;

            labelCores[0].Text = "Deslocamento\r total (" + Portico.gerenciador.ConfiguracoesPGi.CfgProjeto.unidadesProjeto.res_un_deformacao + ")";
            conv = Portico.gerenciador.formDesenho.conversaoComprimento_Def;

            string casas = Portico.gerenciador.formDesenho.casas_decimais_resultado;
              
            if (RGB_Deslocamentos != null)
            {
                labelCores[1].Text =((  RGB_Deslocamentos[13, 1] * conv).ToString(casas));
                labelCores[2].Text =((  RGB_Deslocamentos[12, 1] * conv).ToString(casas));
                labelCores[3].Text =((  RGB_Deslocamentos[11, 1] * conv).ToString(casas));
                labelCores[4].Text =((  RGB_Deslocamentos[10, 1] * conv).ToString(casas));
                labelCores[5].Text = (( RGB_Deslocamentos[9, 1] * conv).ToString(casas));
                labelCores[6].Text = (( RGB_Deslocamentos[8, 1] * conv).ToString(casas));
                
                labelCores[7].Text = (( RGB_Deslocamentos[7, 1] * conv).ToString(casas));
                labelCores[8].Text = (( RGB_Deslocamentos[6, 1] * conv).ToString(casas));
                labelCores[9].Text = (( RGB_Deslocamentos[5, 1] * conv).ToString(casas));
                labelCores[10].Text = ((RGB_Deslocamentos[4, 1] * conv).ToString(casas));
                labelCores[11].Text = ((RGB_Deslocamentos[3, 1] * conv).ToString(casas));
                labelCores[12].Text = ((RGB_Deslocamentos[2, 1] * conv).ToString(casas));
                labelCores[13].Text = ((RGB_Deslocamentos[1, 1] * conv).ToString(casas));

                labelCores[14].Text = ((RGB_Deslocamentos[0, 1] * conv).ToString(casas));
                labelCores[15].Text = ((RGB_Deslocamentos[0, 0] * conv).ToString(casas));
            }
        }

      //  public double[] Deslocamentos;
        public double[,] RGB_Deslocamentos;
        public double[,] ListaRGB_Deslocamentos;
        int iDeslocamentos = 0;

        public System.Windows.Forms.Panel P7 { get => p7; set => p7 = value; }


        void CriaVetores()
        {
            barras = new TBarraPortico[Portico.nBarras];
            Array.Copy(Portico.barras, 1, barras, 0, Portico.nBarras); //copiar a partir do indice 1, pois a primeira barra do array é nula

          //  Deslocamentos = new double[Ngl / 6];

            RGB_Deslocamentos = new double[14, 5];  //12 cores diferentes - 3 são os tons r g b

            ListaRGB_Deslocamentos = new double[14, 3];  //12 cores diferentes - 3 são os tons r g b

            ListaRGB_Deslocamentos[0, 0] = (double)p1.BackColor.R / 255;
            ListaRGB_Deslocamentos[0, 1] = (double)p1.BackColor.G / 255;
            ListaRGB_Deslocamentos[0, 2] = (double)p1.BackColor.B / 255;

            ListaRGB_Deslocamentos[1, 0] = (double)p2.BackColor.R / 255;
            ListaRGB_Deslocamentos[1, 1] = (double)p2.BackColor.G / 255;
            ListaRGB_Deslocamentos[1, 2] = (double)p2.BackColor.B / 255;

            ListaRGB_Deslocamentos[2, 0] = (double)p3.BackColor.R / 255;
            ListaRGB_Deslocamentos[2, 1] = (double)p3.BackColor.G / 255;
            ListaRGB_Deslocamentos[2, 2] = (double)p3.BackColor.B / 255;

            ListaRGB_Deslocamentos[3, 0] = (double)p4.BackColor.R / 255;
            ListaRGB_Deslocamentos[3, 1] = (double)p4.BackColor.G / 255;
            ListaRGB_Deslocamentos[3, 2] = (double)p4.BackColor.B / 255;

            ListaRGB_Deslocamentos[4, 0] = (double)p5.BackColor.R / 255;
            ListaRGB_Deslocamentos[4, 1] = (double)p5.BackColor.G / 255;
            ListaRGB_Deslocamentos[4, 2] = (double)p5.BackColor.B / 255;

            ListaRGB_Deslocamentos[5, 0] = (double)p6.BackColor.R / 255;
            ListaRGB_Deslocamentos[5, 1] = (double)p6.BackColor.G / 255;
            ListaRGB_Deslocamentos[5, 2] = (double)p6.BackColor.B / 255;


            ListaRGB_Deslocamentos[6, 0] = (double)p7.BackColor.R / 255;
            ListaRGB_Deslocamentos[6, 1] = (double)p7.BackColor.G / 255;
            ListaRGB_Deslocamentos[6, 2] = (double)p7.BackColor.B / 255;

            ListaRGB_Deslocamentos[7, 0] = (double)p8.BackColor.R / 255;
            ListaRGB_Deslocamentos[7, 1] = (double)p8.BackColor.G / 255;
            ListaRGB_Deslocamentos[7, 2] = (double)p8.BackColor.B / 255;

            ListaRGB_Deslocamentos[8, 0] = (double)p9.BackColor.R / 255;
            ListaRGB_Deslocamentos[8, 1] = (double)p9.BackColor.G / 255;
            ListaRGB_Deslocamentos[8, 2] = (double)p9.BackColor.B / 255;

            ListaRGB_Deslocamentos[9, 0] = (double)p10.BackColor.R / 255;
            ListaRGB_Deslocamentos[9, 1] = (double)p10.BackColor.G / 255;
            ListaRGB_Deslocamentos[9, 2] = (double)p10.BackColor.B / 255;

            ListaRGB_Deslocamentos[10, 0] = (double)p11.BackColor.R / 255;
            ListaRGB_Deslocamentos[10, 1] = (double)p11.BackColor.G / 255;
            ListaRGB_Deslocamentos[10, 2] = (double)p11.BackColor.B / 255;

            ListaRGB_Deslocamentos[11, 0] = (double)p12.BackColor.R / 255;
            ListaRGB_Deslocamentos[11, 1] = (double)p12.BackColor.G / 255;
            ListaRGB_Deslocamentos[11, 2] = (double)p12.BackColor.B / 255;

            ListaRGB_Deslocamentos[12, 0] = (double)p13.BackColor.R / 255;
            ListaRGB_Deslocamentos[12, 1] = (double)p13.BackColor.G / 255;
            ListaRGB_Deslocamentos[12, 2] = (double)p13.BackColor.B / 255;

            ListaRGB_Deslocamentos[13, 0] = (double)p14.BackColor.R / 255;
            ListaRGB_Deslocamentos[13, 1] = (double)p14.BackColor.G / 255;
            ListaRGB_Deslocamentos[13, 2] = (double)p14.BackColor.B / 255;
        }

        double alfa;
        double[] posicao = new double[4];
        public double[] deslocamentos;
        vec3 z_local;
        double maxDeslocamento;
        void Preencher(int tipocarga, int caso, int comb)
        {
            double u_total;

            maxDeslocamento = double.MinValue;

            iDeslocamentos = 0;
            if (somenteSelecionados)
            {
                foreach (TBarraPortico b in barras)
                {
                    if (b.barraOriginal.Selecionado)
                    {
                        if (tipocarga == 0)
                        {
                            if (b.pIni.casos_x_deslocamentos[caso].U_Total > maxDeslocamento)
                              maxDeslocamento = b.pIni.casos_x_deslocamentos[caso].U_Total;

                            if (b.pFin.casos_x_deslocamentos[caso].U_Total > maxDeslocamento)
                                maxDeslocamento = b.pFin.casos_x_deslocamentos[caso].U_Total;
                        }
                        else
                        if (tipocarga == 1)
                        {
                            if (b.pIni.combinacoes_x_deslocamentos[comb].U_Total > maxDeslocamento)
                                maxDeslocamento = b.pIni.combinacoes_x_deslocamentos[comb].U_Total;

                            if (b.pFin.combinacoes_x_deslocamentos[comb].U_Total > maxDeslocamento)
                                maxDeslocamento = b.pFin.combinacoes_x_deslocamentos[comb].U_Total;
                        }
                    }
                }
            }
            else
            {
                foreach (TBarraPortico b in barras)
                {
                    if (tipocarga == 0)
                    {
                        if (b.pIni.casos_x_deslocamentos[caso].U_Total > maxDeslocamento)
                            maxDeslocamento = b.pIni.casos_x_deslocamentos[caso].U_Total;

                        if (b.pFin.casos_x_deslocamentos[caso].U_Total > maxDeslocamento)
                            maxDeslocamento = b.pFin.casos_x_deslocamentos[caso].U_Total;
                    }
                    else
                    if (tipocarga == 1)
                    {
                        if (b.pIni.combinacoes_x_deslocamentos[comb].U_Total > maxDeslocamento)
                            maxDeslocamento = b.pIni.combinacoes_x_deslocamentos[comb].U_Total;

                        if (b.pFin.combinacoes_x_deslocamentos[comb].U_Total > maxDeslocamento)
                            maxDeslocamento = b.pFin.combinacoes_x_deslocamentos[comb].U_Total;
                    }
                }
            }

            /*if (tipocarga == 0)
            { 
                for (int i = 1; i <= Portico.nNos; i++)
                {
                    u_total = Portico.nos[i].casos_x_deslocamentos[caso].U_Total;

                    Deslocamentos[iDeslocamentos++] = u_total;
                }
            }
            else
            if (tipocarga == 1)
            {
                for (int i = 1; i <= Portico.nNos; i++)
                {
                    u_total = Portico.nos[i].combinacoes_x_deslocamentos[comb].U_Total;

                    Deslocamentos[iDeslocamentos++] = u_total;
                }
            }*/
        }

        void Ordenar(int tipocarga, int caso, int comb)
        {
            double temp;
            int j, k;

        /*    for (j = 0; j < iDeslocamentos; j++)
            {
                for (k = j; k < iDeslocamentos; k++)
                {
                    if (Deslocamentos[k] > Deslocamentos[j])
                    {
                        temp = Deslocamentos[j];
                        Deslocamentos[j] = Deslocamentos[k];
                        Deslocamentos[k] = temp;
                    }
                }               
            }*/

         //   Array.Sort(Deslocamentos, 0, iDeslocamentos);
        //    Array.Reverse(Deslocamentos, 0, iDeslocamentos);
        }

        void AtribuiCoresDeslocamentos(int iTotalCores)
        {

            {
                RGB_Deslocamentos = new double[iTotalCores, 5];

                double max = maxDeslocamento;//Deslocamentos[0];
                double min = 0;// Deslocamentos[iTotalDeslocamentos - 1];
                double dif = max - min;
                double intervalo = dif / iTotalCores;

                for (int i = 0; i < iTotalCores; i++)
                {
                    //Intervalo ...de tanto a tanto
                    RGB_Deslocamentos[i, 0] = max - (intervalo * i);
                    RGB_Deslocamentos[i, 1] = max - (intervalo * (i + 1));

                    //Cores
                    RGB_Deslocamentos[i, 2] = ListaRGB_Deslocamentos[i, 0];
                    RGB_Deslocamentos[i, 3] = ListaRGB_Deslocamentos[i, 1];
                    RGB_Deslocamentos[i, 4] = ListaRGB_Deslocamentos[i, 2];
                }
                Gerenciador.RGB_Deslocamentos = new double[iTotalCores, 5];
                Gerenciador.RGB_Deslocamentos = RGB_Deslocamentos;

              /*  for (int i = 1; i <= Portico.nBarras; i++)
                {
                    Portico.barras[i].RGB_Deslocamentos = new double[iTotalCores, 5];
                    Portico.barras[i].RGB_Deslocamentos = RGB_Deslocamentos;
                }*/
            }
        }

    }

}
