using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PG
{
    public struct ListaMax
    {
        public List<TNoPortico> nos;
        public int nivel;
        public ListaMax(int _nivel)
        {
            nos = new List<TNoPortico>();
            nivel = _nivel;
        }
    }

    public struct ListaMed
    {
        public List<TNoPortico> nos;
        public int nivel;
        public ListaMed(int _nivel)
        {
            nos = new List<TNoPortico>();
            nivel = _nivel;
        }
    }

    public static class TReordenaNosPortico
    {
        static int j, i, k, i1, NCN,
                   DMAX, DMED, DMIN;

        static double P1, P2, P3;

        static double[] C;

        static double[,] RCOR;

        static TNoPortico[] nos;
        static TBarraPortico[] barras;
        static TNoPortico n1;
        static int nnp;
        static TNoPortico[] nosPortico2;
         /*
          Reordenação nodal baseado no algoritmo "ARN3" do trabalho de Teixeira(1991):  "Sistema de reordenação nodal para soluções do tipo banda"          
          Modifiquei o algoritmo, mas a idéia é a mesma: criar uma estrutura de níveis de acordo com reordenção crescente de X e Y, dependendo da dimensão maior do pavimento.
         */
         static void noPortico(double x, double y, double z, int num)
         {
             n1 = new TNoPortico(x,y,z);
             n1.Numero = num;

             ++nnp;
             nosPortico2[nnp] = n1;
         }
         public static void ReordenaNos2(ref TNoPortico[] nosGrelha, TBarraPortico[] barrasGrelha, int qn, int qe, TPorticoEspacial Portico)
         {
             List<TNoPortico> Front = new List<TNoPortico>();
             TBarraPortico barra;

             int ultNumero = 1;
             TNoPortico temp;
             nosGrelha[1].Reordenado = true;

             for (int i = 1; i <= qn; i++)
             {
                 if (!nosGrelha[i].Reordenado || i == 1)
                 {
                     for (int j = 0; j < nosGrelha[i].barrasIncidentes.Count; j++)
                     {
                         barra = nosGrelha[i].barrasIncidentes[j];

                         if ((Object)barra.pFin == (Object)nosGrelha[i])
                             Front.Add(barra.pIni);
                         else
                             if ((Object)barra.pIni == (Object)nosGrelha[i])
                                 Front.Add(barra.pFin);
                     }

                     for (int k = 0; k < Front.Count; k++)
                     {
                         for (int j = k; j < Front.Count; j++)
                         {
                             if (Front[j].barrasIncidentes.Count > Front[k].barrasIncidentes.Count)
                             {
                                 temp = Front[k];
                                 Front[k] = Front[j];
                                 Front[j] = temp;
                             };
                         };
                     };

                     foreach (TNoPortico n in Front)
                     {
                         ultNumero++;
                         n.Numero = ultNumero;
                         n.Reordenado = true;
                     }

                     Front.Clear();
                 }
             }

             int maxi = -1;
             int nbi;
             double x, y;

             x = 0;
             y = 0;

             for (int i = 1; i <= qe; i++)
             {
                 nbi = 6 * (Math.Abs(barrasGrelha[i].pIni.Numero - barrasGrelha[i].pFin.Numero) + 1);

                 if (nbi > maxi)
                 {
                     maxi = nbi;

                     x = barrasGrelha[i].pIni.x;
                     y = barrasGrelha[i].pIni.y;
                 };
             };

             Portico.maxBandaReordenacao = maxi;
         }
        public static void ReordenaNos(ref TNoPortico[] nosPortico, TBarraPortico[] barrasPortico, int qn, int qe, TPorticoEspacial Portico)
        {
            barras = new TBarraPortico[qe + 1];
            nos    = new TNoPortico[qn + 1];
            TNoPortico[] nosNovo = new TNoPortico[qn];
            int i;
            List<ListaMax> lMax = new List<ListaMax>();
            List<ListaMed> lMed = new List<ListaMed>();

            TNoPortico[] nivel = new TNoPortico[qn / 2];

            C    = new double[7];
            RCOR = new double[qn + 1, 4];

            TNoPortico temp;

            for (i = 1; i <= qe; i++)
              barras[i] = barrasPortico[i];

            /* testes */
          /*  nosPortico2 = new TNoPortico[50];
            nnp = 0;
            double iy = 0;

            noPortico(0, iy, 0, 16);
            noPortico(0, iy, 100, 21);
            noPortico(0, iy, 200, 26);

            noPortico(100, iy, 0, 1);
            noPortico(100, iy, 100, 6);
            noPortico(100, iy, 200, 11);

            iy += 100;

            noPortico(0, iy, 0, 17);
            noPortico(0, iy, 100, 22);
            noPortico(0, iy, 200, 27);

            noPortico(100, iy, 0, 2);
            noPortico(100, iy, 100, 7);
            noPortico(100, iy, 200, 12);

            iy += 100;

            noPortico(0, iy, 0, 18);
            noPortico(0, iy, 100, 23);
            noPortico(0, iy, 200, 28);

            noPortico(100, iy, 0, 3);
            noPortico(100, iy, 100, 8);
            noPortico(100, iy, 200, 13);

            iy += 100;

            noPortico(0, iy, 0, 19);
            noPortico(0, iy, 100, 24);
            noPortico(0, iy, 200, 29);

            noPortico(100, iy, 0, 4);
            noPortico(100, iy, 100, 9);
            noPortico(100, iy, 200, 14);

            iy += 100;

            noPortico(0, iy, 0, 20);
            noPortico(0, iy, 100, 25);
            noPortico(0, iy, 200, 30);

            noPortico(100, iy, 0, 5);
            noPortico(100, iy, 100, 10);
            noPortico(100, iy, 200, 15);
            qn = 30;
            nos = new TNoPortico[qn + 1];*/
            /**/


           /* for (i = 1; i <= qn; i++)
            {
                nos[i] = new TNoPortico(nosPortico2[i].x, nosPortico2[i].y, nosPortico2[i].z);
                nos[i].Numero = nosPortico2[i].Numero;
                nos[i].NAR    = nosPortico2[i].Numero;
                nos[i].indiceMatriz = i;

                RCOR[i, 1] = nos[i].x;
                RCOR[i, 2] = nos[i].y;
                RCOR[i, 3] = nos[i].z;
            };*/

            for (i = 1; i <= qn; i++)
            {
                nos[i] = new TNoPortico(nosPortico[i].x, nosPortico[i].y, nosPortico[i].z);
                nos[i].Numero = nosPortico[i].Numero;
                nos[i].NAR = nosPortico[i].Numero;
                nos[i].indiceMatriz = i;

                RCOR[i, 1] = nos[i].x;
                RCOR[i, 2] = nos[i].y;
                RCOR[i, 3] = nos[i].z;
            };
           

            C[1] = 1000E14;  //xMin
            C[2] = -1000E14; //xMax
            C[3] = 1000E14;  //yMin
            C[4] = -1000E14; //yMax
            C[5] = 1000E14;  //zMin
            C[6] = -1000E14; //zMax

            NCN = 3;

            for (i = 1; i <= qn; i++)
            {
                for (j = 1; j <= NCN; j++)
                {
                    i1 = (j - 1) * 2;

                    if (C[i1 + 1] > RCOR[i, j])
                        C[i1 + 1] = RCOR[i, j];

                    if (C[i1 + 2] < RCOR[i, j])
                        C[i1 + 2] = RCOR[i, j];
                }
            };

            P1 = C[2] - C[1];
            P2 = C[4] - C[3];
            P3 = C[6] - C[5];

            if (Geom.Iguais(P1, P2))
            {
                DMAX = 1;
                DMED = 2;
                DMIN = 3;
            }
            else
            if ((P1 > P2) && (P2 > P3))
            {
                DMAX = 1;
                DMED = 2;
                DMIN = 3;                
            };

            if ((P2 > P1) && (P1 > P3))
            {
                DMAX = 2;
                DMED = 1;
                DMIN = 3;
            };
            if ((P3 > P2) && (P2 > P1))
            {
                DMAX = 3;
                DMED = 2;
                DMIN = 1;
            };
            if ((P3 > P1) && (P1 > P2))
            {
                DMAX = 3;
                DMED = 1;
                DMIN = 2;
            };
            if ((P1 > P3) && (P3 > P2))
            {
                DMAX = 1;
                DMED = 3;
                DMIN = 2;
            };
            if ((P2 > P3) && (P3 > P1))
            {
                DMAX = 2;
                DMED = 3;
                DMIN = 1;
            };

            string max = "", med = "", min = "";

            if (DMAX == 1)
                max = "x";
            else
            if (DMAX == 2)
                max = "y";
            else
            if (DMAX == 3)
                max = "z";

            if (DMED == 1)
                med = "x";
            else
            if (DMED == 2)
                med = "y";
            else
            if (DMED == 3)
                med = "z";
            
            if (DMIN == 1)
                min = "x";
            else
            if (DMIN == 2)
                min = "y";
            else
            if (DMIN == 3)
                min = "z";
            
            double zAnt, xAnt, yAnt;

            int c = -1;
            int cc;
            int totnos = qn * 6;
            int incNivel = 0;
            ListaMax lma;
            ListaMed lme;
            int nMaxAtual;

            max = "x";
            med = "z";
            min = "y";
            #region max = x
            if (max == "x")
            {
                for (i = 1; i <= qn; i++)
                {
                    for (j = i; j <= qn; j++)
                    {
                        if (nos[j].x < nos[i].x)
                        {
                            temp = nos[i];
                            nos[i] = nos[j];
                            nos[j] = temp;
                        };
                    };
                };

                incNivel = 1;
                lma = new ListaMax(incNivel);
                lMax.Add(lma);
                xAnt = nos[1].x;

                for (i = 1; i <= qn; i++)
                {
                    if (Geom.Iguais(nos[i].x, xAnt))
                    {
                        lma = lMax[lMax.Count - 1];
                        lma.nivel = incNivel;
                        lma.nos.Add(nos[i]);
                    }
                    else
                    {
                        incNivel++;
                        lMax.Add(new ListaMax(incNivel));
                        lMax[lMax.Count - 1].nos.Add(nos[i]);
                    }
                    xAnt = nos[i].x;
                };

                if (med == "z")
                {
                    for (cc = 0; cc < lMax.Count; cc++)
                    {
                        for (i = 0; i < lMax[cc].nos.Count; i++)
                        {
                            for (j = i; j < lMax[cc].nos.Count; j++)
                            {
                                if (lMax[cc].nos[j].z < lMax[cc].nos[i].z) //reordena vetor 'nos' em ordem crescente de z
                                {
                                    temp = lMax[cc].nos[i];
                                    lMax[cc].nos[i] = lMax[cc].nos[j];
                                    lMax[cc].nos[j] = temp;
                                };
                            };
                        };
                    }
                }
                else
                if (med == "y")
                {
                    for (cc = 0; cc < lMax.Count; cc++)
                    {
                        for (i = 0; i < lMax[cc].nos.Count; i++)
                        {
                            for (j = i; j < lMax[cc].nos.Count; j++)
                            {
                                if (lMax[cc].nos[j].y < lMax[cc].nos[i].y) 
                                {
                                    temp = lMax[cc].nos[i];
                                    lMax[cc].nos[i] = lMax[cc].nos[j];
                                    lMax[cc].nos[j] = temp;
                                };
                            };
                        };
                    }
                }

                if (min == "y") //-->>Y
                {
                    nMaxAtual = lMax[0].nivel;

                    for (cc = 0; cc < lMax.Count; cc++)
                    {
                        incNivel = 1;
                        lme = new ListaMed(incNivel);
                        lMed.Add(lme);
                        zAnt = lMax[cc].nos[0].z;

                        for (i = 0; i < lMax[cc].nos.Count; i++)
                        {
                            if (Geom.Iguais(lMax[cc].nos[i].z, zAnt))
                            {
                                lme = lMed[lMed.Count - 1];
                                lme.nivel = incNivel;
                                lme.nos.Add(lMax[cc].nos[i]);
                            }
                            else
                            {
                                incNivel++;
                                lMed.Add(new ListaMed(incNivel));
                                lMed[lMed.Count - 1].nos.Add(lMax[cc].nos[i]);
                            }
                            zAnt = lMax[cc].nos[i].z;
                        };
                    }

                    for (cc = 0; cc < lMed.Count; cc++)
                    {
                        for (i = 0; i < lMed[cc].nos.Count; i++)
                        {
                            for (j = i; j < lMed[cc].nos.Count; j++)
                            {
                                if (lMed[cc].nos[j].y < lMed[cc].nos[i].y) //reordena vetor 'nos' em ordem crescente de z
                                {
                                    temp = lMed[cc].nos[i];
                                    lMed[cc].nos[i] = lMed[cc].nos[j];
                                    lMed[cc].nos[j] = temp;
                                };
                            };
                        };
                    }
                }
                else
                if (min == "z")
                {
                    for (cc = 0; cc < lMax.Count; cc++)
                    {
                        incNivel = 1;
                        lme = new ListaMed(incNivel);
                        lMed.Add(lme);
                        yAnt = lMax[cc].nos[0].y;

                        for (i = 1; i < lMax[cc].nos.Count; i++)
                        {
                            if (Geom.Iguais(lMax[cc].nos[i].y, yAnt))
                            {
                                lme = lMed[lMed.Count - 1];
                                lme.nivel = incNivel;
                                lme.nos.Add(lMax[cc].nos[i]);
                            }
                            else
                            {
                                incNivel++;
                                lMed.Add(new ListaMed(incNivel));
                                lMed[lMed.Count - 1].nos.Add(lMax[cc].nos[i]);
                            }
                            yAnt = lMax[cc].nos[i].y;
                        };
                    }

                    for (cc = 0; cc < lMed.Count; cc++)
                    {
                        try
                        {
                            for (i = 0; i < lMed[cc].nos.Count; i++)
                            {
                                for (j = i; j < lMed[cc].nos.Count; j++)
                                {
                                    if (lMed[cc].nos[j].z < lMed[cc].nos[i].z) //reordena vetor 'nos' em ordem crescente de z
                                    {
                                        temp = lMed[cc].nos[i];
                                        lMed[cc].nos[i] = lMed[cc].nos[j];
                                        lMed[cc].nos[j] = temp;
                                    };
                                };
                            };
                        }
                        catch(Exception e)
                        {
                            System.Windows.MessageBox.Show(e.Message);
                        }
                    }
                }
            };
            #endregion

            #region max = y
            if (max == "y") 
            {
                for (i = 1; i <= qn; i++)
                {
                    for (j = i; j <= qn; j++)
                    {
                        if (nos[j].y < nos[i].y) 
                        {
                            temp = nos[i];
                            nos[i] = nos[j];
                            nos[j] = temp;
                        };
                    };
                };

                incNivel = 1;
                lma = new ListaMax(incNivel);
                lMax.Add(lma);     
                yAnt = nos[1].y;
               
                for (i = 1; i <= qn; i++)
                {
                    if (Geom.Iguais(nos[i].y, yAnt))
                    {
                        lma = lMax[lMax.Count - 1];
                        lma.nivel = incNivel;
                        lma.nos.Add(nos[i]);
                    }
                    else
                    {
                        incNivel++;
                        lMax.Add(new ListaMax(incNivel));
                        lMax[lMax.Count - 1].nos.Add(nos[i]);
                    }
                    yAnt = nos[i].y;
                };

                if (med == "z")
                {
                    for (cc = 0; cc < lMax.Count; cc++)
                    {
                        for (i = 0; i < lMax[cc].nos.Count; i++)
                        {
                            for (j = i; j < lMax[cc].nos.Count; j++)
                            {
                                if (lMax[cc].nos[j].z < lMax[cc].nos[i].z) //reordena vetor 'nos' em ordem crescente de z
                                {
                                    temp = lMax[cc].nos[i];
                                    lMax[cc].nos[i] = lMax[cc].nos[j];
                                    lMax[cc].nos[j] = temp;
                                };
                            };
                        };         
                    }
                }
                else
                if (med == "x")
                {
                    for (cc = 0; cc < lMax.Count; cc++)
                    {
                        for (i = 0; i < lMax[cc].nos.Count; i++)
                        {
                            for (j = i; j < lMax[cc].nos.Count; j++)
                            {
                                if (lMax[cc].nos[j].x < lMax[cc].nos[i].x) //reordena vetor 'nos' em ordem crescente de z
                                {
                                    temp = lMax[cc].nos[i];
                                    lMax[cc].nos[i] = lMax[cc].nos[j];
                                    lMax[cc].nos[j] = temp;
                                };
                            };
                        };         
                    }
                }
                
                if (min == "x")
                {
                    nMaxAtual = lMax[0].nivel;
                    
                    for (cc = 0; cc < lMax.Count; cc++)
                    {
                        incNivel = 1;
                        lme = new ListaMed(incNivel);
                        lMed.Add(lme);
                        zAnt = lMax[cc].nos[0].z;

                        for (i = 0; i < lMax[cc].nos.Count; i++)
                        {
                            if (Geom.Iguais(lMax[cc].nos[i].z, zAnt))
                            {
                                lme = lMed[lMed.Count - 1];
                                lme.nivel = incNivel;
                                lme.nos.Add(lMax[cc].nos[i]);
                            }
                            else
                            {
                                incNivel++;
                                lMed.Add(new ListaMed(incNivel));
                                lMed[lMed.Count - 1].nos.Add(lMax[cc].nos[i]);
                            }
                            zAnt = lMax[cc].nos[i].z;
                        };
                    }

                    for (cc = 0; cc < lMed.Count; cc++)
                    {
                        for (i = 0; i < lMed[cc].nos.Count; i++)
                        {
                            for (j = i; j < lMed[cc].nos.Count; j++)
                            {
                                if (lMed[cc].nos[j].x < lMed[cc].nos[i].x) //reordena vetor 'nos' em ordem crescente de z
                                {
                                    temp = lMed[cc].nos[i];
                                    lMed[cc].nos[i] = lMed[cc].nos[j];
                                    lMed[cc].nos[j] = temp;
                                };
                            };
                        };
                    }
                }
                else
                if (min == "z")
                {
                    for (cc = 0; cc < lMax.Count; cc++)
                    {
                        incNivel = 1;
                        lme = new ListaMed(incNivel);
                        lMed.Add(lme);
                        xAnt = lMax[cc].nos[0].x;

                        for (i = 1; i < lMax[cc].nos.Count; i++)
                        {
                            if (Geom.Iguais(lMax[cc].nos[i].x, xAnt))
                            {
                                lme = lMed[lMed.Count - 1];
                                lme.nivel = incNivel;
                                lme.nos.Add(lMax[cc].nos[i]);
                            }
                            else
                            {
                                incNivel++;
                                lMed.Add(new ListaMed(incNivel));
                                lMed[lMed.Count - 1].nos.Add(lMax[cc].nos[i]);
                            }
                            xAnt = lMax[cc].nos[i].x;
                        };
                    }

                    for (cc = 0; cc < lMed.Count; cc++)
                    {
                        for (i = 0; i < lMed[cc].nos.Count; i++)
                        {
                            for (j = i; j < lMed[cc].nos.Count; j++)
                            {
                                if (lMed[cc].nos[j].z < lMed[cc].nos[i].z) //reordena vetor 'nos' em ordem crescente de z
                                {
                                    temp = lMed[cc].nos[i];
                                    lMed[cc].nos[i] = lMed[cc].nos[j];
                                    lMed[cc].nos[j] = temp;
                                };
                            };
                        };
                    }
                }      
            };
            #endregion

            #region max = z
            if (max == "z")
            {
                for (i = 1; i <= qn; i++)
                {
                    for (j = i; j <= qn; j++)
                    {
                        if (nos[j].z < nos[i].z)
                        {
                            temp = nos[i];
                            nos[i] = nos[j];
                            nos[j] = temp;
                        };
                    };
                };

                incNivel = 1;
                lma = new ListaMax(incNivel);
                lMax.Add(lma);
                zAnt = nos[1].z;

                for (i = 1; i <= qn; i++)
                {
                    if (Geom.Iguais(nos[i].z, zAnt))
                    {
                        lma = lMax[lMax.Count - 1];
                        lma.nivel = incNivel;
                        lma.nos.Add(nos[i]);
                    }
                    else
                    {
                        incNivel++;
                        lMax.Add(new ListaMax(incNivel));
                        lMax[lMax.Count - 1].nos.Add(nos[i]);
                    }
                    zAnt = nos[i].z;
                };

                if (med == "y")
                {
                    for (cc = 0; cc < lMax.Count; cc++)
                    {
                        for (i = 0; i < lMax[cc].nos.Count; i++)
                        {
                            for (j = i; j < lMax[cc].nos.Count; j++)
                            {
                                if (lMax[cc].nos[j].y < lMax[cc].nos[i].y) //reordena vetor 'nos' em ordem crescente de z
                                {
                                    temp = lMax[cc].nos[i];
                                    lMax[cc].nos[i] = lMax[cc].nos[j];
                                    lMax[cc].nos[j] = temp;
                                };
                            };
                        };
                    }
                }
                else
                if (med == "x")
                {
                    for (cc = 0; cc < lMax.Count; cc++)
                    {
                        for (i = 0; i < lMax[cc].nos.Count; i++)
                        {
                            for (j = i; j < lMax[cc].nos.Count; j++)
                            {
                                if (lMax[cc].nos[j].x < lMax[cc].nos[i].x) //reordena vetor 'nos' em ordem crescente de z
                                {
                                    temp = lMax[cc].nos[i];
                                    lMax[cc].nos[i] = lMax[cc].nos[j];
                                    lMax[cc].nos[j] = temp;
                                };
                            };
                        };
                    }
                }

                if (min == "x")
                {
                    nMaxAtual = lMax[0].nivel;

                    for (cc = 0; cc < lMax.Count; cc++)
                    {
                        incNivel = 1;
                        lme = new ListaMed(incNivel);
                        lMed.Add(lme);
                        yAnt = lMax[cc].nos[0].y;

                        for (i = 0; i < lMax[cc].nos.Count; i++)
                        {
                            if (Geom.Iguais(lMax[cc].nos[i].y, yAnt))
                            {
                                lme = lMed[lMed.Count - 1];
                                lme.nivel = incNivel;
                                lme.nos.Add(lMax[cc].nos[i]);
                            }
                            else
                            {
                                incNivel++;
                                lMed.Add(new ListaMed(incNivel));
                                lMed[lMed.Count - 1].nos.Add(lMax[cc].nos[i]);
                            }
                            yAnt = lMax[cc].nos[i].y;
                        };
                    }

                    for (cc = 0; cc < lMed.Count; cc++)
                    {
                        for (i = 0; i < lMed[cc].nos.Count; i++)
                        {
                            for (j = i; j < lMed[cc].nos.Count; j++)
                            {
                                if (lMed[cc].nos[j].x < lMed[cc].nos[i].x) //reordena vetor 'nos' em ordem crescente de z
                                {
                                    temp = lMed[cc].nos[i];
                                    lMed[cc].nos[i] = lMed[cc].nos[j];
                                    lMed[cc].nos[j] = temp;
                                };
                            };
                        };
                    }
                }
                else
                if (min == "y")
                {
                    for (cc = 0; cc < lMax.Count; cc++)
                    {
                        incNivel = 1;
                        lme = new ListaMed(incNivel);
                        lMed.Add(lme);
                        xAnt = lMax[cc].nos[0].x;

                        for (i = 1; i < lMax[cc].nos.Count; i++)
                        {
                            if (Geom.Iguais(lMax[cc].nos[i].x, xAnt))
                            {
                                lme = lMed[lMed.Count - 1];
                                lme.nivel = incNivel;
                                lme.nos.Add(lMax[cc].nos[i]);
                            }
                            else
                            {
                                incNivel++;
                                lMed.Add(new ListaMed(incNivel));
                                lMed[lMed.Count - 1].nos.Add(lMax[cc].nos[i]);
                            }
                            xAnt = lMax[cc].nos[i].x;
                        };
                    }

                    for (cc = 0; cc < lMed.Count; cc++)
                    {
                        for (i = 0; i < lMed[cc].nos.Count; i++)
                        {
                            for (j = i; j < lMed[cc].nos.Count; j++)
                            {
                                if (lMed[cc].nos[j].y < lMed[cc].nos[i].y) //reordena vetor 'nos' em ordem crescente de z
                                {
                                    temp = lMed[cc].nos[i];
                                    lMed[cc].nos[i] = lMed[cc].nos[j];
                                    lMed[cc].nos[j] = temp;
                                };
                            };
                        };
                    }
                }
            };
            #endregion

            
            int NovoNumero = 0;

            for (cc = 0; cc < lMed.Count; cc++)
              for (i = 0; i < lMed[cc].nos.Count; i++)
                lMed[cc].nos[i].Numero = ++NovoNumero;

            for (cc = 0; cc < lMed.Count; cc++)
                for (i = 0; i < lMed[cc].nos.Count; i++)
                {
                    nosPortico[lMed[cc].nos[i].indiceMatriz].Numero = lMed[cc].nos[i].Numero;
                }
 
            int maxi = -1;
            int nbi;
            double x, y;

            x = 0;
            y = 0;

            for (i = 1; i <= qe; i++)
            {
                nbi = 6 * (Math.Abs(barrasPortico[i].pIni.Numero - barrasPortico[i].pFin.Numero) + 1);

                if (nbi > maxi)
                {
                    maxi = nbi;

                    x = barrasPortico[i].pIni.x;
                    y = barrasPortico[i].pIni.y;
                };
            };

            Portico.maxBandaReordenacao = maxi;
        }

    }
}
