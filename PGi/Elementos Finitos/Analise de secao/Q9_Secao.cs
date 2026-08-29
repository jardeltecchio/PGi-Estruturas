using MathNet.Numerics.Distributions;
using MathNet.Numerics.LinearAlgebra;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Tao.OpenGl;

namespace PG
{
    public class Q9_Secao
    {
        public TNoMEF n1, n2, n3, n4, n5, n6, n7, n8, n9;
        public double jacobiano, area;
        public vec3 centro;
        public int id;
        public Matrix<double> Jacobiana, F, M_Coordenadas, P, B_Iso, B_T, Matriz_Rigidez;

        public Vector<double> ye, N, xe;
        public double[,] MatrizGlobal;

        public Q9_Secao(TNoMEF _n1, TNoMEF _n2, TNoMEF _n3, TNoMEF _n4, TNoMEF _n5, TNoMEF _n6, TNoMEF _n7, TNoMEF _n8, TNoMEF _n9, int _id)
        {
            GlGlobal = new int[10];
            MatrizGlobal = new double[10, 10];

            id = _id;

            n1 = _n1;
            n2 = _n2;
            n3 = _n3;
            n4 = _n4;
            n5 = _n5;
            n6 = _n6;
            n7 = _n7;
            n8 = _n8;
            n9 = _n9;

            N = Vector<double>.Build.Dense(9);
            F = Matrix<double>.Build.Dense(9, 1);

            Jacobiana = Matrix<double>.Build.Dense(2, 2);
            B_Iso = Matrix<double>.Build.Dense(9, 2);
            Matriz_Rigidez = Matrix<double>.Build.Dense(9, 9);
            M_Coordenadas = Matrix<double>.Build.Dense(9, 2);
            xe = Vector<double>.Build.Dense(9);
            ye = Vector<double>.Build.Dense(9);
        }

        public void Matriz_de_Rigidez()
        {
            double peso = 0;
            double eta = 0.00, xi = 0.00, zeta = 0.00;
            Matrix<double> Jacobiana_inversa_transposta, B;
            for (int j = 1; j <= 4; j++)
            {
                QuatroPontosIntegracao(j, ref peso, ref xi, ref eta);

              //  Avalia_N(eta, xi);
                Avalia_Matriz_Derivadas(xi,eta);
                Avalia_Jacobiana();

                Jacobiana_inversa_transposta = (Jacobiana.Inverse());
                Jacobiana_inversa_transposta = Jacobiana_inversa_transposta.Transpose();
                
                //    2x2                         2x9
                B = Jacobiana.Inverse() * B_Iso.Transpose();

                    //(9x2)           (2x2)
                B_T = B_Iso * Jacobiana_inversa_transposta;

                Matriz_Rigidez += (peso * B_T * B) * jacobiano;
                //  Matriz_Rigidez += B.Transpose() *B* jacobiano;
            }

            for (int i = 1; i <= 9; i++)
                for (int j = 1; j <= 9; j++)
                    MatrizGlobal[i, j] = Matriz_Rigidez[i - 1, j - 1];
        }


        public void SetGlGlobal()
        {
            GlGlobal[1] = n1.numero;
            GlGlobal[2] = n2.numero;
            GlGlobal[3] = n3.numero;
            GlGlobal[4] = n4.numero;
            GlGlobal[5] = n5.numero;
            GlGlobal[6] = n6.numero;
            GlGlobal[7] = n7.numero;
            GlGlobal[8] = n8.numero;
            GlGlobal[9] = n9.numero;
        }

        public int[] GlGlobal;

        public void Vetor_F()
        {
            double peso = 0;
            double eta = 0.00, xi = 0.00;
            Matrix<double> ny_nx = Matrix<double>.Build.Dense(2, 1);
            //  Matrix<double> n_x_y;
            double ny, nx;
            Matrix<double> Jacobiana_inversa_transposta, B;
            for (int j = 1; j <= 4; j++)
            {
                QuatroPontosIntegracao(j, ref peso, ref xi, ref eta);

                Avalia_N(xi, eta);
                Avalia_Matriz_Derivadas(xi, eta);
                Avalia_Jacobiana();

                //Jacobiana_inversa_transposta = (Jacobiana.Inverse());
                Jacobiana_inversa_transposta = Jacobiana.Inverse().Transpose();

                //    2x2                  2x9
              //  B = Jacobiana.Inverse() * B_Iso.Transpose();

                //(9x2)           (2x2)
                B_T = B_Iso * Jacobiana_inversa_transposta;


                ny = N * ye;
                nx = N * xe;
                //nx *= -1;

                ny_nx[0, 0] = ny;
                ny_nx[1, 0] = -nx;

                F += peso * B_T * ny_nx * jacobiano;

                //  Matriz_Rigidez += B.Transpose() *B* jacobiano;
            }
        }

        public void Avalia_N(double xi, double eta)
        {
            N[0] = ((xi*eta)*(xi- 1)*(eta-1))/4;
            N[1] =(-eta*(eta-1)*(Math.Pow(xi, 2) - 1))/2;
            N[2] =((xi*eta)*(xi + 1)*(eta-1))/4;
            N[3] = (-xi * (xi + 1) * (Math.Pow(eta, 2) - 1)) / 2;
            N[4] =((xi*eta)*(xi+1)*(eta+1))/4;
            N[5] = (-eta * (eta + 1) * (Math.Pow(xi,2) - 1)) / 2;
            N[6] =((xi*eta)*(xi- 1)*(eta+1))/4;
            N[7] = (-xi * (xi - 1) * (Math.Pow(eta, 2) - 1)) / 2;
            N[8] = (Math.Pow(xi, 2)-1)*(Math.Pow(eta, 2) - 1);
        }

        public void Area()
        {
            double eta = 0, xi = 0, peso = 0;
            Preenche_Matriz_Coordenadas();
            area = 0;

          /*  for (int i = 1; i <= 9; i++)
            {
                NovePontosIntegracao(i, ref peso, ref eta, ref xi);
              //  Avalia_N(eta, xi);
                Avalia_Matriz_Derivadas(eta, xi);
                Avalia_Jacobiana();
                area += (jacobiano * peso);
            }*/

            for (int i = 1; i <= 4; i++)
            {
                QuatroPontosIntegracao(i, ref peso, ref xi, ref eta);
                //  Avalia_N(eta, xi);
                Avalia_Matriz_Derivadas(xi, eta);
                Avalia_Jacobiana();
                area += (jacobiano * peso);
            }
        }

        public void Avalia_Jacobiana()
        {
            Jacobiana = B_Iso.Transpose() * M_Coordenadas;

            jacobiano = Jacobiana.Determinant();
        }

        public void Preenche_Matriz_Coordenadas()
        {
            M_Coordenadas[0, 0] = n1.x; M_Coordenadas[0, 1] = n1.y;

            M_Coordenadas[1, 0] = n2.x; M_Coordenadas[1, 1] = n2.y;

            M_Coordenadas[2, 0] = n3.x; M_Coordenadas[2, 1] = n3.y;

            M_Coordenadas[3, 0] = n4.x; M_Coordenadas[3, 1] = n4.y;

            M_Coordenadas[4, 0] = n5.x; M_Coordenadas[4, 1] = n5.y;

            M_Coordenadas[5, 0] = n6.x; M_Coordenadas[5, 1] = n6.y;

            M_Coordenadas[6, 0] = n7.x; M_Coordenadas[6, 1] = n7.y;

            M_Coordenadas[7, 0] = n8.x; M_Coordenadas[7, 1] = n8.y;

            M_Coordenadas[8, 0] = n9.x; M_Coordenadas[8, 1] = n9.y;

            for (int i = 0; i < 9; i++)
            {
                xe[i] = M_Coordenadas[i, 0];
                ye[i] = M_Coordenadas[i, 1];
            }
        }

        public void Avalia_Matriz_Derivadas(double xi, double eta) // B_Iso: matriz 9x2 de derivadas das funcoes de forma[N]
        {
            double xi_2 = Math.Pow(xi, 2);
            double eta_2 = Math.Pow(eta, 2);
                           //d_n/d_xi                        d_n/d_eta
            B_Iso[0, 0] = eta*(1-(2*xi))*(1-eta);      B_Iso[0, 1] = xi*(1-xi)*(1-(2*eta));
            B_Iso[1, 0] = 4 * xi * eta * (1-eta);      B_Iso[1, 1] =-2*(1-xi_2)*(1-(2*eta));  
            B_Iso[2, 0] = -eta * (1 + (2*xi))*(1-eta); B_Iso[2, 1] = -xi*(1+xi)*(1-(2*eta));
            B_Iso[3, 0] = 2* (1 + (2*xi))*(1-eta_2);   B_Iso[3, 1] = -4*eta*xi*(1+xi);
            B_Iso[4, 0] = eta * (1+(2*xi)) * (1 +eta); B_Iso[4, 1] = xi * (1+xi)*(1+(2*eta));
            B_Iso[5, 0] = -4*xi*eta*(1 + eta);         B_Iso[5, 1] = 2*(1-xi_2)*(1+(2*eta));
            B_Iso[6, 0] = -eta*(1-(2*xi))*(1+eta);     B_Iso[6, 1] = -xi*(1-xi)*(1+(2*eta));
            B_Iso[7, 0] = -2*(1 - (2*xi))*(1-eta_2);   B_Iso[7, 1] = 4*eta*xi*(1-xi);
            B_Iso[8, 0] = -8*xi*(1 - eta_2);           B_Iso[8, 1] = -8*eta*(1 - xi_2);

            for (int i = 0; i < 9; i++)
            {
                B_Iso[i, 0] /= 4;
                B_Iso[i, 1] /= 4;
            }
        }

        public Matrix<double> M_Coordenadas_T, M_Derivadas_FuncoesForma_T;

        public double Qx, Qy, ix, iy;
        public double ixy; // prod. de inercia
        public double ixx, iyy, ry, rx;
        public void QuatroPontosIntegracao(int ponto_de_gauss, ref double peso, ref double xi, ref double eta)
        {
            peso = 1;
            double coord = 0.577350;

            if (ponto_de_gauss == 1)
            {
                xi = -coord;
                eta = -coord;
            }
            else
            if (ponto_de_gauss == 2)
            {
                xi = coord;
                eta = -coord;
            }
            else
            if (ponto_de_gauss == 3)
            {
                xi = coord;
                eta = coord;
            }
            else
            if (ponto_de_gauss == 4)
            {
                xi = -coord;
                eta = coord;
            }
        }

        const double peso_25div81_regra_9 = 0.308641975308641;
        const double peso_40div81_regra_9 = 0.4938271604938;
        const double peso_64div81_regra_9 = 0.7901234567901;

        public void NovePontosIntegracao(int ponto_de_gauss, ref double peso, ref double xi, ref double eta)
        {
            peso = 1;

            double coord = 0.77459666924;

            if (ponto_de_gauss == 1)
            {
                peso = peso_25div81_regra_9;
                eta  = -coord;
                xi   = -coord;
            }
            else
            if (ponto_de_gauss == 2)
            {
                peso = peso_40div81_regra_9;
                xi = 0;
                eta = -coord;
            }
            else
            if (ponto_de_gauss == 3)
            {
                peso = peso_25div81_regra_9;
                xi = coord;
                eta = -coord;
            }
            else
            if (ponto_de_gauss == 4)
            {
                peso = peso_40div81_regra_9;
                xi  = coord;
                eta   = 0;
            }
            else
            if (ponto_de_gauss == 5)
            {
                peso = peso_25div81_regra_9;
                xi = coord;
                eta  = coord;
            }
            else
            if (ponto_de_gauss == 6)
            {
                peso = peso_40div81_regra_9;
                xi = 0;
                eta  = coord;
            }
            else
            if (ponto_de_gauss == 7)
            {
                peso = peso_25div81_regra_9;
                xi = -coord;
                eta  = coord;
            }
            else
            if (ponto_de_gauss == 8)
            {
                peso = peso_40div81_regra_9;
                xi = -coord;
                eta = 0;
            }
            else
            if (ponto_de_gauss == 9)
            {
                peso = peso_64div81_regra_9;
                xi = 0;
                eta = 0;
            }
        }
        public void PrimeirosMomentosDeArea()
        {
            double eta = 0.00, xi = 0.00;
            Qx = 0;
            Qy = 0;

            double peso = 0;

            /*  for (int j = 1; j <= 3; j++)
              {
                  TresPontosIntegracao(j, ref peso, ref eta, ref xi, ref zeta);

                  Avalia_N(eta, xi, zeta);
               //   Avalia_M_Derivadas_FuncoesForma(eta, xi, zeta);
               //   Avalia_Jacobiana(eta, xi, zeta);

                  Qx += peso * N * ye * jacobiano;
                  Qy += peso * N * xe * jacobiano;
              }*/

            for (int j = 1; j <= 4; j++)
            {
                QuatroPontosIntegracao(j, ref peso, ref xi, ref eta);

                Avalia_N(xi, eta);
                Avalia_Matriz_Derivadas(xi,eta);
                Avalia_Jacobiana();

                /*Avalia_M_Derivadas_FuncoesForma(1/3, 1 / 3, 1 / 3);
                Avalia_Jacobiana(1 / 3, 1 / 3, 1 / 3);*/

                Qx += peso * N * ye * jacobiano;
                Qy += peso * N * xe * jacobiano;
            }
        }
        public double jaa;
        public void SegundosMomentosDeArea()
        {
            double eta = 0.00, xi = 0.00;
            ix = 0;
            iy = 0;
            ixy = 0;

            double peso = 0;
            
            for (int j = 1; j <= 4; j++)
            {
                QuatroPontosIntegracao(j, ref peso, ref xi, ref eta);

                Avalia_N(xi, eta);
                Avalia_Matriz_Derivadas(xi, eta);
                Avalia_Jacobiana();

                ix += peso * ((N * ye) * (N * ye)) * jacobiano;
                iy += peso * ((N * xe) * (N * xe)) * jacobiano;
                ixy += peso * ((N * ye) * (N * xe)) * jacobiano;
            }
        }


        public bool avulso, sel;
        public void Desenha()
        {
            /*if (!avulso)
            {
                Gl.glLineWidth(1);
                Gl.glColor3f(0.1f, 0.5f, 1);

                if (avulso)
                {
                    Gl.glColor3f(0.5f, 0.5f, 0.1f);
                    Gl.glLineWidth(2);
                }

                if (sel)
               // {
                //}

                {
                    Gl.glColor3f(0, 0.2f, 0.7f);
                    Gl.glLineWidth(1);

                    Gl.glBegin(Gl.GL_POLYGON);

                    Gl.glVertex2f(System.Convert.ToSingle(n1.x / Desenho.precisaoPixel + Desenho.ponto_zero[0]), System.Convert.ToSingle(n1.y / -Desenho.precisaoPixel + Desenho.ponto_zero[1]));
                    Gl.glVertex2f(System.Convert.ToSingle(n3.x / Desenho.precisaoPixel + Desenho.ponto_zero[0]), System.Convert.ToSingle(n3.y / -Desenho.precisaoPixel + Desenho.ponto_zero[1]));
                    Gl.glVertex2f(System.Convert.ToSingle(n5.x / Desenho.precisaoPixel + Desenho.ponto_zero[0]), System.Convert.ToSingle(n5.y / -Desenho.precisaoPixel + Desenho.ponto_zero[1]));
                    Gl.glVertex2f(System.Convert.ToSingle(n7.x / Desenho.precisaoPixel + Desenho.ponto_zero[0]), System.Convert.ToSingle(n7.y / -Desenho.precisaoPixel + Desenho.ponto_zero[1]));
                    Gl.glVertex2f(System.Convert.ToSingle(n1.x / Desenho.precisaoPixel + Desenho.ponto_zero[0]), System.Convert.ToSingle(n1.y / -Desenho.precisaoPixel + Desenho.ponto_zero[1]));
                    Gl.glEnd();

                    Gl.glBegin(Gl.GL_POLYGON);
                    Gl.glColor3f(1, 0, 0);
                    Gl.glLineWidth(1);
                    Gl.glVertex2f(System.Convert.ToSingle(n9.x / Desenho.precisaoPixel + Desenho.ponto_zero[0]) - 3, System.Convert.ToSingle(n9.y / -Desenho.precisaoPixel + Desenho.ponto_zero[1]) - 3);
                    Gl.glVertex2f(System.Convert.ToSingle(n9.x / Desenho.precisaoPixel + Desenho.ponto_zero[0]) - 3, System.Convert.ToSingle(n9.y / -Desenho.precisaoPixel + Desenho.ponto_zero[1]) + 3);
                    Gl.glVertex2f(System.Convert.ToSingle(n9.x / Desenho.precisaoPixel + Desenho.ponto_zero[0]) + 3, System.Convert.ToSingle(n9.y / -Desenho.precisaoPixel + Desenho.ponto_zero[1]) + 3);
                    Gl.glVertex2f(System.Convert.ToSingle(n9.x / Desenho.precisaoPixel + Desenho.ponto_zero[0]) + 3, System.Convert.ToSingle(n9.y / -Desenho.precisaoPixel + Desenho.ponto_zero[1]) - 3);
                    Gl.glEnd();
                }*/
         //   }
        }
    }
}
