using MathNet.Numerics.LinearAlgebra;
using PG;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xaml;

namespace PG
{
    //quad de 4 nós para cálculo de seção

    public class Q4_Secao
    {
        public TNoMEF n1, n2, n3, n4;
        public double jacobiano, area, E;
        public vec3 centro;
        public int id;
        public Matrix<double> Jacobiana,psi,phi,w, F_torcao,Fx_Cisalhamento, Fy_Cisalhamento, M_Coordenadas, P, B_Iso, B_T, Matriz_Rigidez;
        public Matrix<double> kx, ky, kxy;
        public Vector<double> qw, iw, ixw, iyw;

        public Vector<double> ye, N, xe;
        public double[,] MatrizGlobal;

        public Q4_Secao(TNoMEF _n1, TNoMEF _n2, TNoMEF _n3, TNoMEF _n4, int _id)
        {
            GlGlobal = new int[5];
            MatrizGlobal = new double[5, 5];

            id = _id;

            n1 = _n1;
            n2 = _n2;
            n3 = _n3;
            n4 = _n4;

            N = Vector<double>.Build.Dense(4);
            F_torcao = Matrix<double>.Build.Dense(4, 1);
            Fx_Cisalhamento = Matrix<double>.Build.Dense(4, 1);
            Fy_Cisalhamento = Matrix<double>.Build.Dense(4, 1);

            Jacobiana = Matrix<double>.Build.Dense(2, 2);

            psi = Matrix<double>.Build.Dense(4, 1); // deslocamentos resultado do calculo do cisalhamento em y
            phi = Matrix<double>.Build.Dense(4, 1);// deslocamentos resultado do calculo do cisalhamento em x

            w = Matrix<double>.Build.Dense(4, 1);// deslocamentos/empenamentos resultado do calculo a torção

            kx = Matrix<double>.Build.Dense(1, 1);
            ky = Matrix<double>.Build.Dense(1, 1);
            kxy = Matrix<double>.Build.Dense(1, 1);

            iw = Vector<double>.Build.Dense(1);
            qw = Vector<double>.Build.Dense(1);

            ixw = Vector<double>.Build.Dense(1);
            iyw = Vector<double>.Build.Dense(1);

            B_Iso = Matrix<double>.Build.Dense(4, 2);
            Matriz_Rigidez = Matrix<double>.Build.Dense(4, 4);
            M_Coordenadas = Matrix<double>.Build.Dense(4, 2);
            xe = Vector<double>.Build.Dense(4);
            ye = Vector<double>.Build.Dense(4);
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
                Avalia_Matriz_Derivadas(xi, eta);
                Avalia_Jacobiana();

                Jacobiana_inversa_transposta = (Jacobiana.Inverse());
                Jacobiana_inversa_transposta = Jacobiana_inversa_transposta.Transpose();

                //    2x2                         2x4
                B = Jacobiana.Inverse() * B_Iso.Transpose();

                //(4x2)           (2x2)
                B_T = B_Iso * Jacobiana_inversa_transposta;

                Matriz_Rigidez += (peso * B_T * B) * jacobiano;
                //  Matriz_Rigidez += B.Transpose() *B* jacobiano;
            }

            for (int i = 1; i <= 4; i++)
                for (int j = 1; j <= 4; j++)
                    MatrizGlobal[i, j] = Matriz_Rigidez[i - 1, j - 1];
        }


        public void SetGlGlobal()
        {
            GlGlobal[1] = n1.numero;
            GlGlobal[2] = n2.numero;
            GlGlobal[3] = n3.numero;
            GlGlobal[4] = n4.numero;
        }

        public int[] GlGlobal;

        public void Forcas_Torcao()
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

                //    2x2                  2x4
                //  B = Jacobiana.Inverse() * B_Iso.Transpose();

                //(4x2)           (2x2)
                B_T = B_Iso * Jacobiana_inversa_transposta;


                ny = N * ye;
                nx = N * xe;
                //nx *= -1;

                ny_nx[0, 0] = ny;
                ny_nx[1, 0] = -nx;

                F_torcao += peso * B_T * ny_nx * jacobiano;

                //  Matriz_Rigidez += B.Transpose() *B* jacobiano;
            }
        }

        public void Forcas_Cisalhamento(double poisson, double ixx, double iyy, double prod_inercia)
        {
            double d1 = 0, h1 = 0, d2 = 0, h2=0, r = 0, q = 0;
         
            double peso = 0;
            double eta = 0.00, xi = 0.00;
            Matrix<double> d1_d2 = Matrix<double>.Build.Dense(2, 1);
            Matrix<double> h1_h2 = Matrix<double>.Build.Dense(2, 1);

            Matrix<double> N_T  = Matrix<double>.Build.Dense(4, 1);
      
            Matrix<double> Jacobiana_inversa_transposta, B;

            ixy = prod_inercia;
            ix = ixx;
            iy = iyy;

            Fx_Cisalhamento.Clear();
            Fy_Cisalhamento.Clear();

            B_T.Clear();
            N.Clear();
            B_Iso.Clear();
            Jacobiana.Clear();

            for (int j = 1; j <= 4; j++)
            {
                QuatroPontosIntegracao(j, ref peso, ref xi, ref eta);

                Avalia_N(xi, eta);
                Avalia_Matriz_Derivadas(xi, eta);
                Avalia_Jacobiana();

                Jacobiana_inversa_transposta = Jacobiana.Inverse().Transpose();
                
                N_T = N.ToColumnMatrix();
                
                r = ((N * xe) * (N * xe)) - ((N * ye) * (N * ye));
                q = 2 * ((N * xe) * (N * ye));
               
                d1 = (ix * r) - (ixy * q);
                h1 = (-ixy * r) + (iy * q);

                d2 = (ixy * r) + (ix * q);
                h2 = (-iy * r) - (ixy * q);

                d1_d2[0, 0] = d1;
                d1_d2[1, 0] = d2;

                h1_h2[0, 0] = h1;
                h1_h2[1, 0] = h2;

                //(4x2)           (2x2)
                B_T = B_Iso * Jacobiana_inversa_transposta;

                Fx_Cisalhamento += peso * (((poisson*0.5) * (B_T * d1_d2)) + (2*(1+poisson)) * N_T * ((ix * N * xe) - (ixy * N * ye))) * jacobiano;
                Fy_Cisalhamento += peso * (((poisson*0.5) * (B_T * h1_h2)) + (2*(1+poisson)) * N_T * ((iy * N * ye) - (ixy * N * xe))) * jacobiano;
            }
        }

        public double xs, ys;
        public void Centros_Cisalhamento(double ixx, double iyy, double prod_inercia)
        {
            double peso = 0;
            double eta = 0.00, xi = 0.00;
            
            xs = 0;
            ys = 0;

            ix = ixx;
            iy = iyy;
            ixy = prod_inercia;

            B_T.Clear();
            N.Clear();
            B_Iso.Clear();
            Jacobiana.Clear();

            for (int j = 1; j <= 4; j++)
            {
                QuatroPontosIntegracao(j, ref peso, ref xi, ref eta);

                Avalia_N(xi, eta);
                Avalia_Matriz_Derivadas(xi, eta);
                Avalia_Jacobiana();

                xs += (peso * ((iy*N*xe) + (ixy * N * ye)) * (((N * xe) * (N * xe)) + ((N * ye) * (N * ye))) * jacobiano);
                ys += (peso * ((ix * N * ye) + (ixy * N * xe)) * (((N * xe) * (N * xe)) + ((N * ye) * (N * ye))) * jacobiano);
            }
        }
        public void CoeficientesDeformacaoCisalhamento(double poisson, double ixx, double iyy, double prod_inercia)
        {
            double d1 = 0, h1 = 0, d2 = 0, h2 = 0, r = 0, q = 0;

            double peso = 0;
            double eta = 0.00, xi = 0.00;
            Matrix<double> d1_d2 = Matrix<double>.Build.Dense(2, 1);
            Matrix<double> h1_h2 = Matrix<double>.Build.Dense(2, 1);

            Matrix<double> Jacobiana_inversa_transposta, B;

            ixy = prod_inercia;
            ix = ixx;
            iy = iyy;

            kx.Clear();
            ky.Clear();
            kxy.Clear();
            B_T.Clear();
            N.Clear();
            B_Iso.Clear();
            Jacobiana.Clear();

            for (int j = 1; j <= 4; j++)
            {
                QuatroPontosIntegracao(j, ref peso, ref xi, ref eta);

                Avalia_N(xi, eta);
                Avalia_Matriz_Derivadas(xi, eta);
                Avalia_Jacobiana();

                Jacobiana_inversa_transposta = Jacobiana.Inverse().Transpose();

                r = ((N * xe) * (N * xe)) - ((N * ye) * (N * ye));
                q = 2 * ((N * xe) * (N * ye));

                d1 = (ix * r) - (ixy * q);
                h1 = (-ixy * r) + (iy * q);

                d2 = (ixy * r) + (ix * q);
                h2 = (-iy * r) - (ixy * q);

                d1_d2[0, 0] = d1;
                d1_d2[1, 0] = d2;

                h1_h2[0, 0] = h1;
                h1_h2[1, 0] = h2;

                //    2x2                         2x4
                B = Jacobiana.Inverse() * B_Iso.Transpose();

                //(4x2)           (2x2)
                B_T = B_Iso * Jacobiana_inversa_transposta;

                kx  += peso * (psi.Transpose() * B_T - ((poisson * 0.5) * d1_d2.Transpose())) * (B * psi - ((poisson * 0.5) * d1_d2)) * jacobiano;
                ky  += peso * (phi.Transpose() * B_T - ((poisson * 0.5) * h1_h2.Transpose())) * (B * phi - ((poisson * 0.5) * h1_h2)) * jacobiano;
                kxy += peso * (psi.Transpose() * B_T - ((poisson * 0.5) * d1_d2.Transpose())) * (B * phi - ((poisson * 0.5) * h1_h2)) * jacobiano;

                //            Fx_Cisalhamento += peso * (((poisson * 0.5) * (B_T * d1_d2)) + (2 * (1 + poisson)) * ((ix * N * xe) - (ixy * N * ye))) * jacobiano;
            }
        }

        public void MomentosEmpenamentos()
        {
            double peso = 0;
            double eta = 0.00, xi = 0.00;
            iw.Clear();
            qw.Clear();
            B_T.Clear();
            N.Clear();
            B_Iso.Clear();
            Jacobiana.Clear();

            for (int j = 1; j <= 4; j++)
            {
                QuatroPontosIntegracao(j, ref peso, ref xi, ref eta);

                Avalia_N(xi, eta);
                Avalia_Matriz_Derivadas(xi, eta);
                Avalia_Jacobiana();

                qw += peso * (N * w) * jacobiano;
                iw += peso * (N * w) * (N * w) * jacobiano;
            }
        }
        public void ProdutoSetorialArea()
        {
            double peso = 0;
            double eta = 0.00, xi = 0.00;
            B_T.Clear();
            N.Clear();
            B_Iso.Clear();
            Jacobiana.Clear();
            ixw.Clear();
            iyw.Clear();

            for (int j = 1; j <= 4; j++)
            {
                QuatroPontosIntegracao(j, ref peso, ref xi, ref eta);

                Avalia_N(xi, eta);
                Avalia_Matriz_Derivadas(xi, eta);
                Avalia_Jacobiana();

                ixw += peso * (N * xe * N * w) * jacobiano;
                iyw += peso * (N * ye * N * w) * jacobiano;
            }
        }

        public void Avalia_N(double xi, double eta)
        {
            N[0] = ((1 - xi) * (1 - eta)) / 4;
            N[1] = ((1 + xi) * (1 - eta)) / 4;
            N[2] = ((1 + xi) * (1 + eta)) / 4;
            N[3] = ((1 - xi) * (1 + eta)) / 4;
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

                if (Geom.Iguais(area, 0))
                    area = 0;
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

            for (int i = 0; i < 4; i++)
            {
                xe[i] = M_Coordenadas[i, 0];
                ye[i] = M_Coordenadas[i, 1];
            }
        }

        public void Avalia_Matriz_Derivadas(double xi, double eta) // B_Iso: matriz 4x2 de derivadas das funcoes de forma[N]
        {
            //d_n/d_xi                                     d_n/d_eta
            B_Iso[0, 0] = (-1) * (1 - eta); B_Iso[0, 1] = (-1) * (1 - xi);
            B_Iso[1, 0] = 1 - eta; B_Iso[1, 1] = (-1) * (1 + xi);
            B_Iso[2, 0] = 1 + eta; B_Iso[2, 1] = (1 + xi);
            B_Iso[3, 0] = (-1) * (1 + eta); B_Iso[3, 1] = (1 - xi);

            for (int i = 0; i < 4; i++)
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
                eta = -coord;
                xi = -coord;
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
                xi = coord;
                eta = 0;
            }
            else
            if (ponto_de_gauss == 5)
            {
                peso = peso_25div81_regra_9;
                xi = coord;
                eta = coord;
            }
            else
            if (ponto_de_gauss == 6)
            {
                peso = peso_40div81_regra_9;
                xi = 0;
                eta = coord;
            }
            else
            if (ponto_de_gauss == 7)
            {
                peso = peso_25div81_regra_9;
                xi = -coord;
                eta = coord;
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
                Avalia_Matriz_Derivadas(xi, eta);
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
            if (!avulso)
            {
               /* Gl.glLineWidth(1);
                Gl.glColor3f(0.1f, 0.8f, 0.2f);

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
                    Gl.glVertex2f(System.Convert.ToSingle(n2.x / Desenho.precisaoPixel + Desenho.ponto_zero[0]), System.Convert.ToSingle(n2.y / -Desenho.precisaoPixel + Desenho.ponto_zero[1]));
                    Gl.glVertex2f(System.Convert.ToSingle(n3.x / Desenho.precisaoPixel + Desenho.ponto_zero[0]), System.Convert.ToSingle(n3.y / -Desenho.precisaoPixel + Desenho.ponto_zero[1]));
                    Gl.glVertex2f(System.Convert.ToSingle(n4.x / Desenho.precisaoPixel + Desenho.ponto_zero[0]), System.Convert.ToSingle(n4.y / -Desenho.precisaoPixel + Desenho.ponto_zero[1]));
                    Gl.glVertex2f(System.Convert.ToSingle(n1.x / Desenho.precisaoPixel + Desenho.ponto_zero[0]), System.Convert.ToSingle(n1.y / -Desenho.precisaoPixel + Desenho.ponto_zero[1]));
                    Gl.glEnd();
                }*/
            }
        }
    }
}
