using MathNet.Numerics.LinearAlgebra;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography.Xml;
using System.Text;
using System.Threading.Tasks;

namespace PG
{
    public static class TAlgebra
    {
        static int i, j, k;
        
        public static void RotacionaAresta3D()
        {

        }
        public static Matrix<double> ToMatrix(double[,] A)
        {
            int linhas  = A.GetLength(0) - 1;
            int colunas = A.GetLength(1) - 1;

            Matrix<double> M = Matrix<double>.Build.Dense(linhas, colunas);

            for (int i = 1; i <= linhas; i++)
            {
                for (int j = 1; j <= colunas; j++)
                {
                    M[i - 1, j - 1] = A[i, j];
                }
            }

            return M;
        }
        public static double[,] FromMatrix(Matrix<double> M)
        {
            int linhas = M.RowCount;
            int colunas = M.ColumnCount;

            double[,] A = new double[linhas + 1, colunas + 1];

            for (int i = 1; i <= linhas; i++)
            {
                for (int j = 1; j <= colunas; j++)
                {
                    A[i, j] = M[i - 1, j - 1];
                }
            }

            return A;
        }
        public static double[,] ExtrairSubMatriz(double[,] K,int[] linhas,int[] colunas)
        {
            double[,] R = new double[linhas.Length + 1, colunas.Length + 1];

            for (int i = 1; i <= linhas.Length; i++)
            {
                for (int j = 1; j <= colunas.Length; j++)
                {
                    R[i, j] = K[linhas[i-1], colunas[j-1]];
                }
            }

            return R;
        }
        static Matrix<double> MKii;
        public static double[,] Condensar(double[,] K, int[] externos, int[] internos, ref double[,] Kie, ref double[,] Kei, ref double[,] KiiInv)
        {
            // Submatrizes
            double[,] Kee = ExtrairSubMatriz(K, externos, externos);
            Kei = ExtrairSubMatriz(K, externos, internos);
            Kie = ExtrairSubMatriz(K, internos, externos);
            double[,] Kii = ExtrairSubMatriz(K, internos, internos);

            // Inversa de Kii
             MKii = ToMatrix(Kii);

            if (Math.Abs(MKii.Determinant()) < 1e-12)
                throw new Exception("A matriz Kii é singular durante a condensação da matriz de rigidez com mola elástica.");

            Matrix<double> MKiiInv = MKii.Inverse();
            KiiInv = FromMatrix(MKiiInv);

            int ne = externos.Length;
            int ni = internos.Length;

            // A = Kei * Kii^-1
            double[,] A = new double[ne + 1, ni + 1];
            // A = Kei (12x4) * KiiInv (4x4)
            Multiplica_Matriz_Matriz(ref Kei,ref KiiInv,ref A,
                                    ne,   // linhas de Kei
                                    ni,   // colunas de Kei
                                    ni);  // colunas de KiiInv

            // B = A * Kie
            double[,] B = new double[ne + 1, ne + 1];
            Multiplica_Matriz_Matriz(ref A,ref Kie,ref B,
                ne,   // linhas de A
                ni,   // colunas de A
                ne);  // colunas de Kie

            double[,] Keq = new double[ne + 1, ne + 1];

            // Keq = Kee - B
            Subtrair_Matriz_Matriz(ref Kee, ref B, ref Keq, ne, ne);

           /* for (int i = 1; i <= ne; i++)
            {
                for (int j = 1; j <= ne; j++)
                {
                    Keq[i, j] = Kee[i, j] - B[i, j];
                }
            }*/

            return Keq;
        }
        public static void Subtrair_Matriz_Matriz(ref double[,] A,ref double[,] B,ref double[,] C,int n,int m)
        {
            for (int i = 1; i <= n; i++)
            {
                for (int j = 1; j <= m; j++)
                {
                    C[i, j] = A[i, j] - B[i, j];
                }
            }
        }
        public static void Transposta(ref double[,] Matriz, ref double[,] MatrizRetorno, int linhas, int colunas)
        {
            for (i = 1; i <= linhas; i++)
                for (j = 1; j <= colunas; j++)
                    MatrizRetorno[j, i] = Matriz[i, j];
        }

        //multiplica matrizes nao quadradas. ex: 12x4
        public static void Multiplica_Matriz_Matriz(ref double[,] A,ref double[,] B,ref double[,] C,int linhasA,int colunasA,int colunasB)
        {
            for (int i = 1; i <= linhasA; i++)
            {
                for (int j = 1; j <= colunasB; j++)
                {
                    C[i, j] = 0.0;

                    for (int k = 1; k <= colunasA; k++)
                    {
                        C[i, j] += A[i, k] * B[k, j];
                    }
                }
            }
        }

        public static void Multiplica_Matriz_Matriz(ref double[,] Matriz1, ref double[,] Matriz2, ref double[,] MatrizRetorno, int n, int m)
        {
            double v1,v2;
            for (i = 1; i <= n; i++)
                for (j = 1; j <= m; j++)
                {
                    MatrizRetorno[i, j] = 0;

                    for (k = 1; k <= m; k++)
                    {
                        v1 = Matriz1[i, k];
                        v2 = Matriz2[k, j];

                        MatrizRetorno[i, j] += v1 * v2;
                    }
                };
        }

        public static void Multiplica_Matriz_Vetor(ref double[,] Matriz, ref  double[] Vetor, ref double[] VetorRetorno, int n, int m)
        {
            for (i = 1; i <= n; i++)
            {
                VetorRetorno[i] = 0;

                for (k = 1; k <= m; k++)
                    VetorRetorno[i] += Matriz[i, k] * Vetor[k];
            };
        }

        public static void Multiplica_Matriz_Vetor(ref OpenTK.Matrix4 Matriz, ref  OpenTK.Vector4 Vetor, ref OpenTK.Vector4 VetorRetorno, int n, int m)
        {
            for (i = 0; i < n; i++)
            {
                VetorRetorno[i] = 0;

                for (k = 0; k < m; k++)
                    VetorRetorno[i] += Matriz[i, k] * Vetor[k];
            };
        }

        public static void Multiplica_Matriz_Vetor(ref OpenTK.Matrix4d Matriz, ref OpenTK.Vector4d Vetor, ref OpenTK.Vector4d VetorRetorno, int n, int m)
        {
            for (i = 0; i < n; i++)
            {
                VetorRetorno[i] = 0;

                for (k = 0; k < m; k++)
                    VetorRetorno[i] += Matriz[i, k] * Vetor[k];
            }
            ;
        }

    }

    public struct soluc
    {
        public float[] x;
        public soluc(int matrizmax)
        {
            x = new float[matrizmax];
        }
    }
    
    public class Gauss_seidel
    {
        const int matrizmax = 20;
        float[,] matriz;

        int numeq;
        float valorinicial;
        float[] xo ;
        float[] xj;

        float tol;
        int k = 0;
        int i, j, h, n;
        float par1, par2, par3, par4;

        soluc[] solucao;

        public Gauss_seidel(int _numeq, float _tol, float _valorinicial, int numIteracoes)
        {
            this.numeq = _numeq;
            matriz = new float[_numeq, _numeq];
            xo = new float[_numeq];
            xo = new float[_numeq];
            this.tol = _tol;
            this.valorinicial = _valorinicial;
            this.solucao = new soluc[_numeq];
            this.n = numIteracoes;
        }

        public void Resolve()
        {
            for (i = 0; i < numeq; i++)
              xo[i] = valorinicial;
            
            k = 0;
            for (i = 0; i < numeq; i++)
            {
                solucao[i] = new soluc(numeq);
                solucao[k].x[i] = xo[i];
            }

            k = 1;

            while (k<=n)
            {
               for (i=0;i<numeq;i++) //controla os valores de cada variavel na iteracao k
               {
                   par1=par2=0;
                   
                   for (int j=0;j<=i-1;j++) //calcula a primeira somatoria
                      par1 += matriz[i,j]*xj[j];
                   
                   for ( j=i+1;j<=numeq;j++) //calcula a segunda somatoria
                      par2 += matriz[i,j]*xo[j];
                   
                   solucao[k].x[i] = (-par1 - par2+ matriz[i,numeq])/matriz[i,i];
                   
                   for (int y=0;y<j;y++)
                     xj[y]=solucao[k].x[y];
               }
               
               par2=0;

               for (j=0;j<numeq;j++) //calcula
               { 
                   par1=0;
                   par1 += solucao[k].x[j] - xo[j];
                   par2 += (par1)*(par1);
               }
               
               if ((Math.Sqrt(par2)) < tol)
               {
                  for (i=0;i<numeq;i++)
                     //printf("\n%4.7f", solucao[k].x[i]);
                  return;
               }

               for(i=0;i<numeq;i++)
                 xo[i] = solucao[k].x[i];
               
               k++ ; 
         }

         System.Windows.Forms.MessageBox.Show("Numero maximo de iteracoes excedidas : " + k.ToString());
       }
    }

}
