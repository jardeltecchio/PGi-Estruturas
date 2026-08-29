using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace PG
{
       [Serializable]
    public static class TReordenaNosGrelha
    {
        static int  j, i,k, i1, NCN,
                   DMAX;
        
        static double P1, P2, P3;

        static double[] C;

        static double[,] RCOR;

        static TNoGrelha[] nos;
        static TBarraGrelha[] barras;

        public static void ReordenaNos2(ref TNoGrelha[] nosGrelha, TBarraGrelha[] barrasGrelha, int qn, int qe, Form gerenc, TPavimento pavimento)
        {
            List<TNoGrelha> Front = new List<TNoGrelha>();
            TBarraGrelha barra;
           
            int ultNumero = 1;
            TNoGrelha temp;
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

                    foreach (TNoGrelha n in Front)
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
                nbi = 3 * (Math.Abs(barrasGrelha[i].pIni.Numero - barrasGrelha[i].pFin.Numero) + 1);

                if (nbi > maxi)
                {
                    maxi = nbi;

                    x = barrasGrelha[i].pIni.x;
                    y = barrasGrelha[i].pIni.y;
                };
            };

            pavimento.maxBandaReordenacao = maxi;
        }

        /*
          Reordenação nodal baseado no algoritmo "ARN3" do trabalho de Teixeira(1991):  "Sistema de reordenação nodal para soluções do tipo banda"          
          Modifiquei o algoritmo, mas a idéia é a mesma: criar uma estrutura de níveis de acordo com reordenção crescente de X e Y, dependendo da dimensão maior do pavimento.
         */
        public static void ReordenaNos(ref TNoGrelha[] nosGrelha, TBarraGrelha[] barrasGrelha, int qn, int qe, TPavimento pavimento)
        {
            barras  = new TBarraGrelha[qe + 1];
            nos     = new TNoGrelha[qn + 1];
            TNoGrelha [] nosNovo = new TNoGrelha[qn]; 

            TNoGrelha [] nivel = new TNoGrelha[qn/2];

            C      = new double[7];
            RCOR   = new double[qn + 1, 3];
            
            TNoGrelha temp;

            for (i = 1; i <= qe; i++)
              barras[i] = barrasGrelha[i];

            for (i = 1; i <= qn; i++)
            {
                nos[i] = new TNoGrelha(nosGrelha[i].x, nosGrelha[i].y);
                nos[i].Numero = nosGrelha[i].Numero;

                RCOR[i, 1] = nos[i].x;
                RCOR[i, 2] = nos[i].y; 
            };

            C[1] = 1000E14;  //xMin
            C[2] = -1000E14; //xMax
            C[3] = 1000E14;  //yMin
            C[4] = -1000E14; //yMax
            C[5] = 1000E14;  //zMin
            C[6] = -1000E14; //zMax
            
            NCN = 2;

            for (i = 1; i <= qn; i++)
            {
                for (j = 1; j <= NCN; j++)
                {
                    i1 = (j -1) * 2;
                 
                    if (C[i1 + 1] > RCOR[i,j])
                      C[i1 + 1] = RCOR[i,j];

                    if (C[i1 + 2] < RCOR[i,j])
                      C[i1 + 2] = RCOR[i,j];
                }
            };

            P1 = C[2] - C[1];
            P2 = C[4] - C[3];
            P3 = C[6] - C[5];

            if (Geom.Iguais(P1, P2))
            {
                DMAX = 1;

            }
            else
            if ((P1 > P2) && (P2 > P3)) 
            {
                DMAX = 1;

            };
            if ((P2 > P1) && (P1 > P3))
            {
                DMAX = 2;

            };
            if ((P3 > P2) && (P2 > P1))
            {
                DMAX = 3;

            };
            if ((P3 > P1) && (P1 > P2))
            {
                DMAX = 3;

            };
            if ((P1 > P3) && (P3 > P2))
            {
                DMAX = 1;

            };
            if ((P2 > P3) && (P3 > P1))
            {
                DMAX = 2;

            };

            int c = -1;
            int totnos = qn * 3;
            
            if (DMAX == 1) //reordena vetor 'nos' em ordem crescente de x
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

                double xAnt = nos[1].x;
                
                nos[1].nivel = 1;

                for (i = 2; i <= qn; i++)
                {
                    if (Geom.Iguais(nos[i].x, xAnt))
                      nos[i].nivel = nos[i-1].nivel;
                    else
                      nos[i].nivel = nos[i-1].nivel+1;

                    xAnt = nos[i].x;
                };

                int nivAnt = 1;
                int n = -1;

                for (i = 1; i <= qn; i++)
                {
                    if (nos[i].nivel == nivAnt)
                    {
                        nivel[++n]      = new TNoGrelha(nos[i].x, nos[i].y);
                        nivel[n].Numero = nos[i].Numero;
                        nivAnt          = nos[i].nivel;

                        if ((i == qn))
                        {
                            if (n > 0)
                            {
                                for (j = 0; j < (n + 1); j++)//reordena vetor 'nivel' em ordem crescente de y
                                {
                                    for (k = j; k < (n + 1); k++)
                                    {
                                        if (nivel[k].y < nivel[j].y)
                                        {
                                            temp = nivel[j];
                                            nivel[j] = nivel[k];
                                            nivel[k] = temp;
                                        };
                                    };
                                };
                            };

                            for (j = 0; j < (n + 1); j++)
                              nosNovo[++c] = nivel[j];
                        };
                    }
                    else
                    {
                        //reordena vetor 'nivel' em ordem crescente de y
                        if (n > 0)
                        {
                            for (j = 0; j < (n + 1); j++)
                            {
                                for (k = j; k < (n + 1); k++)
                                {
                                    if (nivel[k].y < nivel[j].y)
                                    {
                                        temp = nivel[j];
                                        nivel[j] = nivel[k];
                                        nivel[k] = temp;
                                    };
                                };
                            };
                        };

                        for (j = 0; j < (n + 1); j++)
                        {
                            nosNovo[++c] = new TNoGrelha(nivel[j].x, nivel[j].y);
                            nosNovo[c].Numero = nivel[j].Numero;
                        }
                        n = -1;

                        for (j = 0; j < (qn / 2); j++)
                            nivel[j] = null;
  
                        nivel[++n] = new TNoGrelha(nos[i].x, nos[i].y);
                        nivel[n].Numero = nos[i].Numero;
                        nivAnt = nos[i].nivel;

                        if (i == qn)
                        {
                            nosNovo[++c] = new TNoGrelha(nos[i].x, nos[i].y);
                            nosNovo[c].Numero = nos[i].Numero;                        
                        }
                    }
                };
                for (i = 0; i < qn; i++)
                {
                    int numero = nosNovo[i].Numero;
                    for (j = 0; j < qn; j++)
                        if ((Object)nosNovo[j] != (Object)nosNovo[i])
                            if (nosNovo[j].Numero == numero)
                                MessageBox.Show("");


                }
                for (i = 0; i < qn; i++)
                  nosGrelha[nosNovo[i].Numero].Numero = i+1;
            }
            else
            if (DMAX == 2) //y
            {
                for (i = 1; i <= qn; i++)
                {
                    for (j = i; j <= qn; j++)
                    {
                        if (nos[j].y > nos[i].y) //reordena vetor 'nos' em ordem crescente de y
                        {
                            temp = nos[i];
                            nos[i] = nos[j];
                            nos[j] = temp;
                        };
                    };
                };

                double yAnt = nos[1].y;

                nos[1].nivel = 1;

                for (i = 2; i <= qn; i++)
                {
                    if (Geom.Iguais(nos[i].y, yAnt))
                      nos[i].nivel = nos[i - 1].nivel;
                    else
                      nos[i].nivel = nos[i - 1].nivel + 1;

                    yAnt = nos[i].y;
                };

                int nivAnt = 1;
                int n = -1;

                for (i = 1; i <= qn; i++)
                {
                    if (nos[i].nivel == nivAnt)
                    {
                        nivel[++n] = nos[i];
                        nivAnt = nos[i].nivel;

                        if ((i == qn))
                        {
                            if (n > 0)
                            {
                                for (j = 0; j < (n + 1); j++) //reordena vetor 'nivel' em ordem crescente de x
                                {
                                    for (k = j; k < (n + 1); k++)
                                    {
                                        if (nivel[k].x < nivel[j].x)
                                        {
                                            temp = nivel[j];
                                            nivel[j] = nivel[k];
                                            nivel[k] = temp;
                                        };
                                    };
                                };
                            };

                            for (j = 0; j < (n + 1); j++)
                              nosNovo[++c] = nivel[j];
                        };
                    }
                    else
                    if (nos[i].nivel != nivAnt)
                    {
                            if (n > 0)
                            {
                                for (j = 0; j < (n + 1); j++) //reordena vetor 'nivel' em ordem crescente de x
                                {
                                    for (k = j; k < (n + 1); k++)
                                    {
                                        if (nivel[k].x < nivel[j].x)
                                        {
                                            temp = nivel[j];
                                            nivel[j] = nivel[k];
                                            nivel[k] = temp;
                                        };
                                    };
                                };
                            };

                            for (j = 0; j < (n + 1); j++)
                                nosNovo[++c] = nivel[j];

                            n = -1;

                            nivel[++n] = nos[i];
                            nivAnt = nos[i].nivel;
                    
                            if (i == qn)
                              nosNovo[++c] = nos[i];
                    }
                };

                for (i = 0; i < qn; i++)
                    nosGrelha[nosNovo[i].Numero].Numero = i + 1;
            };

            int maxi = -1;
            int nbi;
            double x, y;

            x = 0;
            y = 0;

            for (int i = 1; i <= qe; i++)
            {
               nbi = 3* (Math.Abs(barrasGrelha[i].pIni.Numero - barrasGrelha[i].pFin.Numero) + 1);

               if (nbi > maxi)
               {
                  maxi = nbi;

                  x = barrasGrelha[i].pIni.x;
                  y = barrasGrelha[i].pIni.y;
               };
            };

            pavimento.maxBandaReordenacao = maxi;
        }
        
    }
}
