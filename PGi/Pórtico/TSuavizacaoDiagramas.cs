using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static PG.TPorticoEspacial;

namespace PG
{
    public class TSuavizacaoDiagramas
    {
        List<BarrasDivididasXBarrasPortico> barras;
        string esforco;
        public TSuavizacaoDiagramas(List<BarrasDivididasXBarrasPortico> _barras,string tipo, int item)
        {
            barras  = _barras;

           // Suavizar(1, 7,tipo, item); //fx
         //   Suavizar(2, 8,tipo, item); //fz
          //  Suavizar(3, 9,tipo, item); //fy
        }
        vec3 vSuavizado2, vSuavizado1;
        double eSuavizado2, eSuavizado1;
        public void Suavizar(int g1, int g2, string tipo, int item)
        {
            if (tipo == Const.ID_TIPO_COMBINACAO)
            {
                for (int i = 0; i < barras.Count; i++)
                {
                    double cordBarraAnt2 = barras[i].barrasPortico[0].combinacoes_x_esforcos[item].Esforcos[g2];

                    for (int j = 0; j < barras[i].barrasPortico.Count; j++)
                    {
                        if (g1 == 1)
                        {
                            eSuavizado1 = ((cordBarraAnt2 + barras[i].barrasPortico[j].combinacoes_x_esforcos[item].Esforcos[g1] * -1) / 2);
                            /*      if (barras[i].barrasPortico[j].Esforcos[g2] > barras[i].barrasPortico[j].Esforcos[g1])
                                     eSuavizado1 = Math.Abs((cordBarraAnt2 + barras[i].barrasPortico[j].Esforcos[g1]) / 2);
                                  else
                                     eSuavizado1 = ((cordBarraAnt2 + barras[i].barrasPortico[j].Esforcos[g1]) / 2);*/
                            cordBarraAnt2 = barras[i].barrasPortico[j].combinacoes_x_esforcos[item].Esforcos[g2] * -1;
                        }
                        else
                        {
                            eSuavizado1 = ((cordBarraAnt2 + barras[i].barrasPortico[j].combinacoes_x_esforcos[item].Esforcos[g1] * -1) / 2);

                            cordBarraAnt2 = barras[i].barrasPortico[j].combinacoes_x_esforcos[item].Esforcos[g2] * -1;
                        }
                        // for (int k = 0; k < 4; k++)
                        //  {
                        //  barras[i].barrasPortico[j].Esforcos[g1] = eSuavizado1;

                        //  }

                        if (j == 0) continue;

                        barras[i].barrasPortico[j].combinacoes_x_esforcos[item].Esforcos[g1] += eSuavizado1;

                        if (j > 0)
                            barras[i].barrasPortico[j - 1].combinacoes_x_esforcos[item].Esforcos[g2] += eSuavizado1;
                    }

                }
            }
            else
            if (tipo == Const.ID_TIPO_CASO)
            {
                for (int i = 0; i < barras.Count; i++)
                {
                    double cordBarraAnt2 = barras[i].barrasPortico[0].casos_x_esforcos[item].Esforcos[g2];

                    for (int j = 0; j < barras[i].barrasPortico.Count; j++)
                    {
                        if (g1 == 1)
                        {
                            eSuavizado1 = ((cordBarraAnt2 + barras[i].barrasPortico[j].casos_x_esforcos[item].Esforcos[g1] * -1) / 2);
                            /*      if (barras[i].barrasPortico[j].Esforcos[g2] > barras[i].barrasPortico[j].Esforcos[g1])
                                     eSuavizado1 = Math.Abs((cordBarraAnt2 + barras[i].barrasPortico[j].Esforcos[g1]) / 2);
                                  else
                                     eSuavizado1 = ((cordBarraAnt2 + barras[i].barrasPortico[j].Esforcos[g1]) / 2);*/
                            cordBarraAnt2 = barras[i].barrasPortico[j].casos_x_esforcos[item].Esforcos[g2] * -1;
                        }
                        else
                        {
                            eSuavizado1 = ((cordBarraAnt2 + barras[i].barrasPortico[j].casos_x_esforcos[item].Esforcos[g1] * -1) / 2);

                            cordBarraAnt2 = barras[i].barrasPortico[j].casos_x_esforcos[item].Esforcos[g2] * -1;
                        }
                        // for (int k = 0; k < 4; k++)
                        //  {
                        //  barras[i].barrasPortico[j].Esforcos[g1] = eSuavizado1;

                        //  }

                        if (j == 0) continue;

                        barras[i].barrasPortico[j].casos_x_esforcos[item].Esforcos[g1] += eSuavizado1;

                        if (j > 0)
                            barras[i].barrasPortico[j - 1].casos_x_esforcos[item].Esforcos[g2] += eSuavizado1;
                    }

                }
            }
        } 
    }
}
