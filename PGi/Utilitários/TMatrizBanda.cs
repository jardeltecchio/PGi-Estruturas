using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.IO;
/*
 
 FatoraMatrizBanda  - Rotina BANFAC livro Gere & Weaver (Matriz Analysis of Framed Structures) 
 ResolveMatrizBanda - Rotina BANSOL livro Gere & Weaver (Matriz Analysis of Framed Structures) 

 */

namespace PG
{
    [Serializable]
    public unsafe class TMatrizBanda
    {
        int nLinhas, nColunas, Ngl;

        TModeloEstrutural Modelo;
        public double[] Sff;
        public double[][] Sff_, Banda;
        public double[,] Sff2;
        public double[] Sff_SemBanda;
        double[] copiaSff;
       // public alglib.sparsematrix s;
        int banda;
        bool Cholesky_Padrao;
        public alglib.sparsematrix s;
        public unsafe TMatrizBanda(int nLinhas, int nColunas, int Ngl, TModeloEstrutural Modelo, bool cholesky_padrao, bool usardll = false)
        {
            this.nColunas = nColunas; // incremento 1 para usar os índices da matriz como 1,2,3,4 e não 0,1,2,3... (essa primeira coluna fica nula). Isso facilita o entendimento 
            this.nLinhas  = nLinhas;
            this.Modelo   = Modelo;
            this.Ngl      = Ngl;
            this.Modelo   = Modelo;
            this.Cholesky_Padrao   = cholesky_padrao;
            try
            {
                if (Cholesky_Padrao)         
                  Sff = new double[(Ngl + 1) * (nColunas + 1)];
                else
                {
                     #region teste
                    //nLinhas = 4;

                    s = new alglib.sparsematrix();
                    //alglib.sparsecreatecrs(nLinhas, nLinhas, out s);
                    alglib.sparsecreate(nLinhas, nLinhas, out s);

                 /*   alglib.sparseset(s, 0, 0, 2); alglib.sparseset(s,   0, 1, 1); //alglib.sparseset(s,  0, 2, -3); alglib.sparseset(s,   0, 3, 6);
                    alglib.sparseset(s, 1, 0,1); alglib.sparseset(s,1, 1, 3); alglib.sparseset(s,1, 2, 1); //alglib.sparseset(s,  1, 3, -6);
                    alglib.sparseset(s, 2, 1, 1); alglib.sparseset(s,2, 2, 3); alglib.sparseset(s, 2, 3, 1); //alglib.sparseset(s, 2, 3, 5.5);
                    alglib.sparseset(s, 3,2, 1); alglib.sparseset(s, 3, 3, 2);//alglib.sparseset(s,  3, 2, 15.5); //alglib.sparseset(s, 3, 3, -1);
                
                    //    alglib.sparseset(s, 4, 4, 1); //alglib.sparseset(s, 3, 3, 2);//alglib.sparseset(s,  3, 2, 15.5); //alglib.sparseset(s, 3, 3, -1);
                    alglib.sparseconverttocrs(s);


                 /*   alglib.sparseset(s, 0, 0, 1);
                    alglib.sparseset(s, 1, 1, 9450); alglib.sparseset(s, 1, 3, 4725); 
                    alglib.sparseset(s, 2, 2, 1);
                    alglib.sparseset(s, 3, 1, 4725); alglib.sparseset(s, 3, 3, 18900);
                    alglib.sparseset(s, 4, 4, 1);
                    alglib.sparseset(s, 5, 5, 1);
                    */
                   // bool resultadoOK = alglib.sparsecholesky(s, true);
                  /*  bool resultadoOK = true;

                    double [,] cha = new double[nLinhas, nLinhas];
                    double[,] cha2 = new double[nLinhas, nLinhas];
                    
                    double[,] b = new double[nLinhas,1];
                    b[0, 0] = 4; b[1, 0] = 10; b[2, 0] = 15; b[3, 0] = 11; //b[4, 0] = 0; b[5, 0] = 0;
                    double[] ladodireito = new double[nLinhas];
                    ladodireito[0] = 4; ladodireito[1] = 10; ladodireito[2] = 15; ladodireito[3] = 11; //b[4, 0] = 0; b[5, 0] = 0;
                    double[] resultado = new double[nLinhas];
                    
                    if (resultadoOK)
                    {
                        int info = 0;

                        for (int i = 0; i < nLinhas; i++)
                        {
                            for (int j = 0; j < nLinhas; j++)
                            {
                                //   if (j>=i)
                                cha[i, j] = alglib.sparseget(s, i, j);
                                cha2[i, j] = alglib.sparseget(s, i, j);
                            }
                        }
                   
                        alglib.matinvreport repo;
                        int[] p0 = new int[nLinhas];
                        bool decomposicaoOk = alglib.sparsecholeskyp(s, false ,out p0);

                       // bool decomposicaoOk = alglib.spdmatrixcholesky(ref cha, nLinhas, true);
                      //  MessageBox.Show(cha[2, 2].ToString()); 
                        MessageBox.Show(alglib.sparseget(s,2,2).ToString());

                        Cholesky_subst(s,nLinhas,ladodireito, ref resultado);
                      //  alglib.spdmatrixcholeskyinverse(ref cha, out info, out repo);
                      //  alglib.spdmatrixinverse(ref cha2, out info, out repo);

                        if (decomposicaoOk)
                        {
                            alglib.spdmatrixcholeskysolvemfast(cha, nLinhas, true, ref b, 1, out info);
                            if (info != 0)
                            {
                        
                            }
                        }
                    }*/

                    #endregion

                   /* Banda = new double[nLinhas][];

                  //  for (int i = 0; i < Banda.Length; i++)
                  //    Banda[i] = new double[nColunas];

                    for (int i = 0; i < Banda.Length; i++)
                        Banda[i] = new double[nLinhas];


                    banda = nColunas - 1;*/
                   // alglib.sparsecreatesksband(nLinhas, nLinhas, banda, out s);*/
                }

           /*     Sff_ = new double[nLinhas][];
                for (int i = 0; i < Sff_.Length; i++)
                  Sff_[i] = new double[nLinhas];*/
            }
            
            catch(System.OutOfMemoryException)
            {
                MessageBox.Show("Falta de memória!", "Erro", MessageBoxButtons.OK, MessageBoxIcon.Error);   
            }

            copiaSff = null;
        }

        void Cholesky_subst(alglib.sparsematrix s, int n, double[] ladoDireito, ref double [] resultado)
        {
            double sum = 0;
            int k, p;
            portico.Atualiza(1);
            Application.DoEvents();
            for (int l = 0; l < n; l++)
            {
                for (sum = ladoDireito[l], k = l - 1; k >= 0; k--)
                    sum -= alglib.sparseget(s,l,k) * resultado[k];

                resultado[l] = sum / alglib.sparseget(s, l, l);
            };

            portico.Atualiza(1);
            Application.DoEvents();
            for (int i = n - 1; i >= 0; i--)
            {
                for (sum = resultado[i], p = i + 1; p < n; p++)
                    sum -= alglib.sparseget(s, p, i) * resultado[p];

                resultado[i] = sum / alglib.sparseget(s, i, i);
            }
        }

        public bool CriaCopia()
        {
            if (Cholesky_Padrao)
            {
                this.copiaSff = new double[(Ngl + 1) * (nColunas + 1)];
                this.copiaSff = this.Sff;
            }

            return true;
        }

        public bool FatoraMatrizBanda(string tit1, string tit2, bool UsarDll)
        {
            //if (copiaSff == null) 
            //  CriaCopia();

            int j, j1, j2, i, i1, k;
            double sum, temp;

            span1 = DateTime.Now;

          // Modelo.MsgCalculo(tit1, tit2, nLinhas,false, false);

            try
            {
                if (Cholesky_Padrao)
                {
                    if (Sff[1 * nColunas + 1] <= 0) return false;

                    if (UsarDll)
                    {
                     //   return PGiSolver.Solver.FatoraMatrizBanda(ref Sff, nLinhas, nColunas);
                    }
                    else
                    {
                        for (j = 2; j <= nLinhas; j++)
                        {
                            j1 = j - 1;
                            j2 = j - nColunas + 1;

                            if (j2 < 1)
                                j2 = 1;

                            if (j1 != 1)
                            {
                                //     Application.DoEvents();

                                for (i = 2; i <= j1; i++)
                                {
                                    i1 = i - 1;

                                    if (i1 >= j2)
                                    {
                                        sum = Sff[i * nColunas + j - i + 1];

                                        for (k = j2; k <= i1; k++)
                                            sum = sum - Sff[k * nColunas + i - k + 1] * Sff[k * nColunas + j - k + 1];

                                        Sff[i * nColunas + j - i + 1] = sum;
                                    }
                                }
                            };

                            sum = Sff[j * nColunas + 1];
                            for (k = j2; k <= j1; k++)
                            {
                                temp = Sff[k * nColunas + j - k + 1] / Sff[k * nColunas + 1];
                                sum = sum - temp * Sff[k * nColunas + j - k + 1];
                                Sff[k * nColunas + j - k + 1] = temp;
                            }

                            if (sum <= 0)
                            {
                                Modelo.Progresso.Value = 0;
                                return false;
                            }
                            Sff[j * nColunas + 1] = sum;
                        };
                    }
                    System.GC.Collect();
                }
              /*  else
                if (!Solver1)
                {
                   
                    double coef;
                    int colMatriz, colBanda;
                    for (i = 0; i < nLinhas; i++)
                    {
                        colBanda = 0;
                        colMatriz = i;
                        for (j = 0; j < nColunas; j++)
                        {
                            if (colBanda < nLinhas)
                            {
                                coef = Banda[i][colBanda];

                                if (coef != 0)
                                    alglib.sparseset(s, i, colMatriz, coef);

                                colMatriz++;
                                colBanda++;
                            }
                            else
                                break;
                        }
                    }
                    Sff2 = null;
                    Sff_ = null;
                    Banda = null;
                    System.GC.Collect();
                }*/
               

                return true;
            }

            catch (System.OutOfMemoryException)
            {
          //      MessageBox.Show("Falta de memória!", "Erro", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return false;
            }
            catch (System.IndexOutOfRangeException)
            {
                MessageBox.Show("Pavimento: " + Modelo.Descricao + "  -  Erro na solução do sistema! Verifique as condições de suporte.", "Matriz de rigidez", System.Windows.Forms.MessageBoxButtons.OK, MessageBoxIcon.Error);
       
                return false;   
            }

        }
        DateTime span1, span2;
        TPorticoEspacial portico;
  
        public int ResolveMatrizBanda(TPorticoEspacial portico_, ref double[] ac, ref double[] df,string tit1, string tit2, bool solver_cholesky, bool solver_cg, bool fatorar = true)
        {
            try
            {
                portico = portico_;
                if (Cholesky_Padrao)
                {
                    int i, j, k, i1, k1, k2;
                    double sum;
                    //guardar a coluna 1 de sff para usar no segundo loop
                    double[] sff1 = new double[nLinhas + 1];
                    Application.DoEvents();
                    
                    for (i = 1; i <= nLinhas; i++)
                    {
                        Application.DoEvents();
                        j = i - nColunas + 1;

                        if (i <= nColunas)
                            j = 1;

                        sum = ac[i];
                        k1 = i - 1;

                        for (k = j; k <= k1; k++)
                            sum = sum - Sff[k * nColunas + i - k + 1] * df[k];

                        df[i] = sum;

                        sff1[i] = Sff[i * nColunas + 1];
                    }

                    for (i = 1; i <= nLinhas; i++)
                        df[i] = df[i] / sff1[i];

                    for (i1 = 1; i1 <= nLinhas; i1++)
                    {
                        Application.DoEvents();
                        i = nLinhas - i1 + 1;
                        j = i + nColunas - 1;

                        if (j > nLinhas)
                            j = nLinhas;

                        sum = df[i];

                        k2 = i + 1;

                        for (k = k2; k <= j; k++)
                          sum = sum - Sff[i * nColunas + k - i + 1] * df[k];

                        df[i] = sum;
                    }

                    Sff = null;
                    System.GC.Collect();
                }
                else
                if (!Cholesky_Padrao)
                {
                    bool CHOLESKY;

                    CHOLESKY = true;


                    int j,i;
                    span1 = DateTime.Now;
                  //  Modelo.MsgCalculo(tit1, tit2, nLinhas, false, false);
/*
                    int res = PGiSolver.Solver.ResolveAlgLib(ref Banda, ref ac, ref df, ref Sff, nLinhas, nColunas, banda);
                    if (res == -1)
                      Modelo.HistoricoCalculo("ERRO: FALTA DE MEMORIA"  );*/
                     // alglib.sparsematrix s = new alglib.sparsematrix();
                    
                       //alglib.sparsecreatesksband(nLinhas, nLinhas, banda,out s);
                      // alglib.sparsecreate(nLinhas,nLinhas,out s);

                    /*    double coef;
                        int colMatriz, colBanda;
                        int col = nLinhas;
                    for(i=0; i<nLinhas; i++)
                    {
                        col--;
                        for(j=0; j<nLinhas; j++)
                        {
                          //  if (j<=col)
                            {
                                coef = Sff_[i][j];
                                if (coef != 0)
                                alglib.sparseset(s, i, j, coef);
                            }
                        }
                    }*/

                    
                /*    alglib.sparsecreate(4, 4, out s);
                    alglib.sparseset(s, 0, 0, 2.0);
                    alglib.sparseset(s, 0, 1, 1.0);
                    alglib.sparseset(s, 1, 1, 3.0);
                    alglib.sparseset(s, 1, 2, 1.0);
                    alglib.sparseset(s, 2, 2, 3.0);
                    alglib.sparseset(s, 2, 3, 1.0);
                    alglib.sparseset(s, 3, 3, 2.0);*/

                    /*  for (i = 0; i < nLinhas; i++)
                        {
                           colBanda = 0;
                           colMatriz = i;
                           for (j = 0; j < nColunas; j++)
                           {
                              if (colBanda < nLinhas)
                             {
                                 coef = Banda[i][colBanda];

                                  if (coef != 0)
                                     alglib.sparseset(s, i, colMatriz, coef);

                                colMatriz++;
                                  colBanda++;
                              }
                              else
                          break;
                       }
                    }*/
                   // Sff2 = null;
                    //Sff_ = null;
                //    Banda = null;
                //    System.GC.Collect();

                    //bool resultadoOK = alglib.sparsecholesky(s, true);
                    
                    
                    bool resultadoOK = true;
                  /*  if (resultadoOK)
                    {
                        double[,] cha = new double[nLinhas, nLinhas];
                        double[,] b = new double[nLinhas, 1];
                        
                        for (i = 0; i < nLinhas; i++)
                          b[i, 0] = ac[i + 1];

                        int info = 0;

                        for (i = 0; i < nLinhas; i++)
                            for (j = 0; j < nLinhas; j++)
                                cha[i, j] = alglib.sparseget(s, i,j);

                        bool decomposicaoOk = alglib.spdmatrixcholesky(ref cha, nLinhas, true);
                        if (decomposicaoOk)
                        {
                            alglib.spdmatrixcholeskysolvemfast(cha, nLinhas, true, ref b, 1, out info);
                            if (info != 0)
                            {
                                for (i = 0; i < nLinhas; i++)
                                  df[i + 1] = b[i,0];
                            }
                        }
                    }*/
                   /* double[,] cha = new double[nLinhas, nLinhas];

                    for (i = 0; i < nLinhas; i++)
                        for (j = 0; j < nLinhas; j++)
                            cha[i, j] = alglib.sparseget(s, i, j);*/
                   
                    double[] ladodireito = new double[nLinhas];
                    for (i = 0; i < nLinhas; i++)
                        ladodireito[i] = ac[i + 1];
                 
                    double[] resultado = new double[nLinhas];
                    Application.DoEvents();

                    alglib.sparseconverttocrs(s);

                    if (solver_cg)
                    {
                        alglib.lincgstate state;
                        alglib.lincgreport cg_rep;
                        alglib.lincgcreate(nLinhas, out state);

                        alglib.lincgsolvesparse(state, s, false, ladodireito);

                      //  Thread t = new Thread(() =>  alglib.lincgsolvesparse(state, s, false, ladodireito));
                       // t.Start();
                        //t.Abort();

              //          while t.IsAlive
                        alglib.lincgresults(state, out resultado, out cg_rep);

                        for (i = 0; i < nLinhas; i++)
                            df[i + 1] = resultado[i];
                    }
                    else
                    if (solver_cholesky)
                    {

                        alglib.sparsedecompositionanalysis anals;
                        bool decomposicaoOk;
                        int[] p0 = new int[0];
                        double[] d = new double[nLinhas];
                        int[] p = new int[nLinhas];

                        for (i = 0; i <= nLinhas - 1; i++)
                        {
                            p[i] = i;
                            d[i] = 1.0;
                        }

                        //     decomposicaoOk = alglib.spdmatrixcholesky();
                        decomposicaoOk = alglib.sparsecholeskyanalyze(s, false, 0, -1, out anals);

                        if (decomposicaoOk)
                        {
                            if (fatorar)
                              decomposicaoOk = alglib.sparsecholeskyfactorize(anals, false, out s, out d, out p);
                        }
                        //      decomposicaoOk = alglib.sparsecholesky(s, false);

                        if (portico.gerenciador.CalculoCancelado)
                            return -6;

                        if (decomposicaoOk)
                        {
 
                            /*    bool kkk = alglib.spdmatrixcholesky(ref cha, nLinhas, false);
                    
                                double[,] b = new double[nLinhas, 1];
                                for (i = 0; i < nLinhas; i++)
                                    b[i, 0] = ac[i + 1];

                                int info = 0;
                                alglib.spdmatrixcholeskysolvemfast(cha, nLinhas, false, ref b, 1, out info);*/
                            //MessageBox.Show(alglib.sparseget(s,5,5).ToString());
                            alglib.sparsesolverreport rep;
                            portico.Atualiza(1);

                            alglib.sparsespdcholeskysolve(s, false, ladodireito, out resultado, out rep);

                            Application.DoEvents();
                            //  Cholesky_subst(s, nLinhas, ladodireito, ref resultado);
                            for (i = 0; i < nLinhas; i++)
                                df[i + 1] = resultado[i];
                        }
                    }
                 //   b[0, 0] = 0; b[1, 0] = -2; b[2, 0] = 0; b[3, 0] = 0; b[4, 0] = 0; b[5, 0] = 0;


                  /*  double[] b;
                    bool isuppertriangle = true;
                    bool resultadoOK;
                    double ccc;
                    double[] x;
                    alglib.sparsesolverreport rep;
                    double[,] cha;
                    if (CHOLESKY)
                    {
                        b = new double[nLinhas];

                        for (i = 1; i <= nLinhas; i++)
                            b[i - 1] = ac[i];

                        Application.DoEvents();
               
                        ccc = alglib.sparseget(s, 0, 0);
                        resultadoOK = alglib.sparsecholesky(s, true);
                        //     resultadoOK = alglib.sparselu(s, true);

                        ccc = alglib.sparseget(s, 1, 1);

                        MessageBox.Show(ccc.ToString());
                        cha = new double[0, 0];

                        if (resultadoOK)
                            alglib.spdmatrixcholesky(ref cha, nLinhas, true);
                        //alglib.spdmatrixcholeskysolvemfast()

                    }
                    else
                    {

                        b = new double[nLinhas];

                        for (i = 1; i <= nLinhas; i++)
                            b[i - 1] = ac[i];

                        Application.DoEvents();
                        //   alglib.sparsesolvesks(s, nLinhas, isuppertriangle, b, out rep, out x);  //alglib 3.15
                        alglib.sparsespdsolvesks(s, isuppertriangle, b, out x, out rep);

                        ccc = alglib.sparseget(s, 1, 1);
                        resultadoOK = alglib.sparsecholesky(s, true);
                        //     resultadoOK = alglib.sparselu(s, true);

                        ccc = alglib.sparseget(s, 1, 1);

                        MessageBox.Show(ccc.ToString());
                        cha = new double[0, 0];

                        if (resultadoOK)
                          alglib.spdmatrixcholesky(ref cha, nLinhas, true);
                        //alglib.spdmatrixcholeskysolvemfast()
                        for (i = 0; i < nLinhas; i++)
                            df[i + 1] = x[i];
                    }*/

                //    alglib.deallocateimmediately(ref s);
                    
                 //   x = null;
                //    rep = null;
                  //  b = null;
                 //   ac = null;
                    System.GC.Collect();
                    
                }

                span2 = DateTime.Now;
                
              //  Modelo.HistoricoCalculo("      > Resolução de equações - [Ok] - " + span2.Subtract(span1).ToString("mm") + ":" + span2.Subtract(span1).ToString("ss"));
                return 0;
                //       Modelo.MsgCalculo("", "", 0, true);
            }
            catch (System.OutOfMemoryException)
            {
             //   alglib.deallocateimmediately(ref s);
                System.GC.Collect();
                return 1;
              //  MessageBox.Show("Falta de memória!", "Erro", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            catch (Exception e )
            {
                Modelo.HistoricoCalculo("ERRO: A estrutura possui elemento/s instável/is, verifique as articulações ou restrições de apoio! \r " + e.Message,false,true);
                return 2;
              //  MessageBox.Show("Pavimento: " + Modelo.Descricao + "  -  Erro na solução do sistema! Verifique as condições de suporte.\r" + e.Message, "Matriz de rigidez", System.Windows.Forms.MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
        void GC_Solver()
        {
            MessageBox.Show("");
        }
        public bool VerificarPrecisao(ref double[] ac, ref double[] df)
        {
            double[] deltaAC = new double[nLinhas + 1];
            double[] deltaDF = new double[nLinhas + 1];

            double tolerancia = 0.01;
            double maiorDiferenca = 0, valorAC = 0, valorNovoAC = 0;
            bool diferencaOK = false;
            int nRefinamentos = 3;

            for (int i = 1; i < nRefinamentos && !diferencaOK; i++)
            {
                CalcularResiduoEmAC(ac, df, deltaAC, ref maiorDiferenca, ref valorAC, ref valorNovoAC);

                //verificar tolerancia
                if (maiorDiferenca > tolerancia)
                {
                    if (maiorDiferenca > 1)
                        //diferença excessiva, parar refinamento
                        i = nRefinamentos + 1;

                    if (i > 1)
                        deltaDF = new double[nLinhas + 1];

                    //determinar vetor de correção
                   // ResolveMatrizBanda(portico, ref deltaAC, ref deltaDF,"","");

                    //corrigir df
                    for (int i1 = 1; i1 <= nLinhas; i1++)
                        df[i1] += deltaDF[i1];
                }
                else
                    diferencaOK = true;
            };
            return diferencaOK;
        }

        public void CalcularResiduoEmAC(double[] ac, double[] df, double[] deltaAC, ref double maiorDiferenca,
            ref double valorAC, ref double valorNovoAC)
        {
            //vetor resultado da multiplicação copiaSff * df
            double[] novoAC = new double[nLinhas + 1];

            for (int i = 1; i <= nLinhas; i++)
            {
                int i1 = i - nColunas + 1; // linha inicial
                if (i1 < 1)
                    i1 = 1;

                double sum = 0;
                for (int j = i1; j <= i; j++)
                    sum += (df[j] * copiaSff[j * nColunas + i - j + 1]);

                int k2 = Math.Min(nColunas, nLinhas - i + 1);
                for (int j = 2; j <= k2; j++)
                    sum += (df[i + j - 1] * copiaSff[i * nColunas + j]);

                novoAC[i] = sum;
            }

            maiorDiferenca = 0;
            for (int i1 = 1; i1 <= nLinhas; i1++)
            {
                deltaAC[i1] = ac[i1] - novoAC[i1];

                if (Math.Abs(ac[i1]) > 10)
                {
                    double dif = Math.Abs((novoAC[i1] - ac[i1]) / ac[i1]);
                    if (dif > maiorDiferenca)
                    {
                        maiorDiferenca = dif;
                        valorAC = ac[i1];
                        valorNovoAC = novoAC[i1];
                    }
                }
            }

            copiaSff = null;
        }
    }
}
