using MathNet.Numerics.Distributions;
using MathNet.Numerics.LinearAlgebra;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PG
{
    //triangulo de 6 nós para cálculo de seção
    public class T6_Secao
    {
        public TNoMEF n1, n2, n3, n4, n5, n6;
        public double jacobiano, area, E;
        public vec3 centro;
        public int id;
        public Matrix<double> Jacobiana, psi, phi, w, Fx_Cisalhamento, Fy_Cisalhamento, F_torcao, M_Coordenadas, P, B_Iso, B_T, Matriz_Rigidez;
        public Matrix<double> kx, ky, kxy;
        public Vector<double> qw, iw, ixw, iyw;

        public Vector<double> ye, N, xe;
        public double[,] MatrizGlobal;

        //retorna o valor de cada funcao de forma
        public double N1(double eta)
        {
            return eta * (2 * eta - 1);
        }

        public double N2(double xi)
        {
            return xi * (2 * xi - 1);
        }
        public double N3(double zeta)
        {
            return zeta * (2 * zeta - 1);
        }
        public double N4(double eta, double xi)
        {
            return 4 * eta * xi;
        }
        public double N5(double xi, double zeta)
        {
            return 4 * xi * zeta;
        }
        public double N6(double eta, double zeta)
        {
            return 4 * eta * zeta;
        }

        public T6_Secao(TNoMEF _n1, TNoMEF _n2, TNoMEF _n3, TNoMEF _n4, TNoMEF _n5, TNoMEF _n6, int _id)
        {
            GlGlobal = new int[7];
            MatrizGlobal = new double[7, 7];

            id = _id;

            n1 = _n1;
            n2 = _n2;
            n3 = _n3;
            n4 = _n4;
            n5 = _n5;
            n6 = _n6;

            N = Vector<double>.Build.Dense(6);
            F_torcao = Matrix<double>.Build.Dense(6, 1);

            Fx_Cisalhamento = Matrix<double>.Build.Dense(6, 1);
            Fy_Cisalhamento = Matrix<double>.Build.Dense(6, 1);
            Jacobiana = Matrix<double>.Build.Dense(3, 3);
            B_Iso = Matrix<double>.Build.Dense(6, 3);
            Matriz_Rigidez = Matrix<double>.Build.Dense(6, 6);
            M_Coordenadas = Matrix<double>.Build.Dense(6, 2);
            xe = Vector<double>.Build.Dense(6);
            ye = Vector<double>.Build.Dense(6);

            psi = Matrix<double>.Build.Dense(6, 1); // deslocamentos resultado do calculo do cisalhamento
            phi = Matrix<double>.Build.Dense(6, 1);// deslocamentos resultado do calculo do cisalhamento

            w = Matrix<double>.Build.Dense(6, 1);// deslocamentos/empenamentos resultado do calculo a torção

            kx = Matrix<double>.Build.Dense(1, 1);
            ky = Matrix<double>.Build.Dense(1, 1);
            kxy = Matrix<double>.Build.Dense(1, 1);

            iw = Vector<double>.Build.Dense(1);
            qw = Vector<double>.Build.Dense(1);

            ixw = Vector<double>.Build.Dense(1);
            iyw = Vector<double>.Build.Dense(1);
        }
        public void Matriz_P()
        {
            Matrix<double> Jacobiana_inversa = Jacobiana.Inverse();
            Matrix<double> m_3x2 = Matrix<double>.Build.Dense(3, 2);
            m_3x2[0, 0] = 0; m_3x2[0, 1] = 0;
            m_3x2[1, 0] = 1; m_3x2[1, 1] = 0;
            m_3x2[2, 0] = 0; m_3x2[2, 1] = 1;

            P = Jacobiana_inversa * m_3x2;
        }

        public void Matriz_BT()
        {
            Matriz_P();

            B_T = B_Iso * P;
        }

        public void Matriz_de_Rigidez()
        {
            double peso = 0;
            double eta = 0.00, xi = 0.00, zeta = 0.00;
          
            N.Clear();
            B_Iso.Clear();
    
            Jacobiana.Clear();
         
            for (int j = 1; j <= 3; j++)
            {
                TresPontosIntegracao(j, ref peso, ref eta, ref xi, ref zeta);

                Avalia_N(eta, xi, zeta);
                Avalia_Matriz_Derivadas(eta, xi, zeta);
                Avalia_Jacobiana(eta, xi, zeta);

                Matriz_P();

                B_T = B_Iso * P;

                Matriz_Rigidez += (peso * B_T * B_T.Transpose()) * jacobiano;
                //  Matriz_Rigidez += B.Transpose() *B* jacobiano;
            }

            for (int i = 1; i <= 6; i++)
                for (int j = 1; j <= 6; j++)
                    MatrizGlobal[i, j] = Matriz_Rigidez[i - 1, j - 1];
        }

        public void Avalia_Jacobiana(double eta, double xi, double zeta)
        {
            double j10 = 0, j12 = 0, j11 = 0, j20 = 0, j21 = 0, j22 = 0;

            for (int i = 0; i < 6; i++)
            {
                j10 += B_Iso[i, 0] * xe[i];// M_Coordenadas[i, 0];
                j11 += B_Iso[i, 1] * xe[i];// M_Coordenadas[i, 0];
                j12 += B_Iso[i, 2] * xe[i];//M_Coordenadas[i, 0];

                j20 += B_Iso[i, 0] * ye[i];//M_Coordenadas[i, 1];
                j21 += B_Iso[i, 1] * ye[i];//M_Coordenadas[i, 1];
                j22 += B_Iso[i, 2] * ye[i];//M_Coordenadas[i, 1];
            }

            Jacobiana[0, 0] = 1; Jacobiana[0, 1] = 1; Jacobiana[0, 2] = 1;

            Jacobiana[1, 0] = j10;
            Jacobiana[1, 1] = j11;
            Jacobiana[1, 2] = j12;

            Jacobiana[2, 0] = j20;
            Jacobiana[2, 1] = j21;
            Jacobiana[2, 2] = j22;

            area = Jacobiana.Determinant() / 2;

            jacobiano = 0.5 * Jacobiana.Determinant();

            //                  Jabobiana = M_Derivadas_FuncoesForma_T * M_Coordenadas;

            //      System.Windows.Forms.MessageBox.Show(Jacobiana[0, 2].ToString());
        }

        public void SetGlGlobal()
        {
            GlGlobal[1] = n1.numero;
            GlGlobal[2] = n2.numero;
            GlGlobal[3] = n3.numero;
            GlGlobal[4] = n4.numero;
            GlGlobal[5] = n5.numero;
            GlGlobal[6] = n6.numero;
        }

        public int[] GlGlobal;
        public void Forcas_Cisalhamento(double poisson, double ixx, double iyy, double prod_inercia)
        {
            double d1=0, h1=0, d2=0, h2=0, r = 0, q = 0;

            double peso = 0;
            double eta = 0.00, xi = 0.00, zeta = 0.00;
            Matrix<double> d1_d2 = Matrix<double>.Build.Dense(2, 1);
            Matrix<double> h1_h2 = Matrix<double>.Build.Dense(2, 1);

            Matrix<double> N_T = Matrix<double>.Build.Dense(6, 1);

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
            P.Clear();
            for (int j = 1; j <= 6; j++)
            {
                SeisPontosIntegracao(j, ref peso, ref eta, ref xi, ref zeta);

                Avalia_N(eta, xi, zeta);
                Avalia_Matriz_Derivadas(eta, xi, zeta);
                Avalia_Jacobiana(eta, xi, zeta);

                Matriz_P();

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

                B_T = B_Iso * P;

                Fx_Cisalhamento += peso * (((poisson * 0.5) * (B_T * d1_d2)) + (2 * (1 + poisson)) * N_T * ((ix * N * xe) - (ixy * N * ye))) * jacobiano;
                Fy_Cisalhamento += peso * (((poisson * 0.5) * (B_T * h1_h2)) + (2 * (1 + poisson)) * N_T * ((iy * N * ye) - (ixy * N * xe))) * jacobiano;
            }
        }

        public double xs, ys;
        public void Centros_Cisalhamento(double ixx, double iyy, double prod_inercia)
        {
            double peso = 0;
            double eta = 0.00, xi = 0.00, zeta = 0.00;

            xs = 0;
            ys = 0;

            ix = ixx;
            iy = iyy;
            ixy = prod_inercia;

            N.Clear();
            Jacobiana.Clear();
            P.Clear();

            for (int j = 1; j <= 6; j++)
            {
                SeisPontosIntegracao(j, ref peso, ref eta, ref xi, ref zeta);

                Avalia_N(eta, xi, zeta);
                Avalia_Matriz_Derivadas(eta, xi, zeta);
                Avalia_Jacobiana(eta, xi, zeta);

                xs += (peso * ((iy * N * xe) + (ixy * N * ye)) * (((N * xe) * (N * xe)) + ((N * ye) * (N * ye))) * jacobiano);
                ys += (peso * ((ix * N * ye) + (ixy * N * xe)) * (((N * xe) * (N * xe)) + ((N * ye) * (N * ye))) * jacobiano);
            }
        }

        public void CoeficientesDeformacaoCisalhamento(double poisson, double ixx, double iyy, double prod_inercia)
        {
            double d1 = 0, h1 = 0, d2 = 0, h2 = 0, r = 0, q = 0;

            double peso = 0;
            double eta = 0.00, xi = 0.00, zeta = 0.00;
            Matrix<double> d1_d2 = Matrix<double>.Build.Dense(2, 1);
            Matrix<double> h1_h2 = Matrix<double>.Build.Dense(2, 1);

            Matrix<double> Jacobiana_inversa_transposta, B;

            ixy = prod_inercia;
            ix = ixx;
            iy = iyy;

            B_T.Clear();
            N.Clear();
            B_Iso.Clear();
            Jacobiana.Clear();

            for (int j = 1; j <= 6; j++)
            {
               SeisPontosIntegracao(j, ref peso, ref eta, ref xi, ref zeta);

                Avalia_N(eta, xi, zeta);
                Avalia_Matriz_Derivadas(eta, xi, zeta);
                Avalia_Jacobiana(eta, xi, zeta);

                Jacobiana_inversa_transposta = Jacobiana.Inverse().Transpose();

                Matriz_P();

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
          //      B = Jacobiana.Inverse() * B_Iso.Transpose();
                //(4x2)           (2x2)
                B_T = B_Iso * P;

                B = B_T.Transpose();

                kx += peso * (psi.Transpose() * B_T - ((poisson * 0.5) * d1_d2.Transpose())) * (B * psi - ((poisson * 0.5) * d1_d2)) * jacobiano;
                ky += peso * (phi.Transpose() * B_T - ((poisson * 0.5) * h1_h2.Transpose())) * (B * phi - ((poisson * 0.5) * h1_h2)) * jacobiano;
                kxy += peso * (psi.Transpose() * B_T - ((poisson * 0.5) * d1_d2.Transpose())) * (B * phi - ((poisson * 0.5) * h1_h2)) * jacobiano;

                //            Fx_Cisalhamento += peso * (((poisson * 0.5) * (B_T * d1_d2)) + (2 * (1 + poisson)) * ((ix * N * xe) - (ixy * N * ye))) * jacobiano;
            }
        }

        public void MomentosEmpenamentos()
        {
            double peso = 0;
            double eta = 0.00, xi = 0.00, zeta = 0.00;
            iw.Clear();
            qw.Clear();
            B_T.Clear();
            N.Clear();
            B_Iso.Clear();
            Jacobiana.Clear();

            for (int j = 1; j <= 3; j++)
            {
                TresPontosIntegracao(j, ref peso, ref eta, ref xi, ref zeta);

                Avalia_N(eta, xi, zeta);
                Avalia_Matriz_Derivadas(eta, xi, zeta);
                Avalia_Jacobiana(eta, xi, zeta);

                qw += peso * (N * w) * jacobiano;
                iw += peso * (N * w) * (N * w) * jacobiano;
            }
        }
        public void ProdutoSetorialArea()
        {
            double peso = 0;
            double eta = 0.00, xi = 0.00, zeta = 0.00;
            B_T.Clear();
            N.Clear();
            B_Iso.Clear();
            Jacobiana.Clear();
            ixw.Clear();
            iyw.Clear();

            for (int j = 1; j <= 6; j++)
            {
                SeisPontosIntegracao(j, ref peso, ref eta, ref xi, ref zeta);

                Avalia_N(eta, xi, zeta);
                Avalia_Matriz_Derivadas(eta, xi, zeta);
                Avalia_Jacobiana(eta, xi, zeta);

                ixw += peso * (N * xe * N * w) * jacobiano;
                iyw += peso * (N * ye * N * w) * jacobiano;
            }
        }

        public void Forcas_Torcao()
        {
            double peso = 0;
            double eta = 0.00, xi = 0.00, zeta = 0.00;
            Matrix<double> ny_nx = Matrix<double>.Build.Dense(2, 1);
            //  Matrix<double> n_x_y;
            double ny, nx;

            for (int j = 1; j <= 3; j++)
            {
                TresPontosIntegracao(j, ref peso, ref eta, ref xi, ref zeta);

                Avalia_N(eta, xi, zeta);
                Avalia_Matriz_Derivadas(eta, xi, zeta);
                Avalia_Jacobiana(eta, xi, zeta);

                Matriz_P();

                ny = N * ye;
                nx = N * xe;
                //nx *= -1;

                ny_nx[0, 0] = ny;
                ny_nx[1, 0] = -nx;

                B_T = B_Iso * P;

                F_torcao += peso * B_T * ny_nx * jacobiano;

                //  Matriz_Rigidez += B.Transpose() *B* jacobiano;
            }
        }

        public void Avalia_N(double eta, double xi, double zeta)
        {
            N[0] = eta * (2 * eta - 1);
            N[1] = xi * (2 * xi - 1);
            N[2] = zeta * (2 * zeta - 1);
            N[3] = 4 * eta * xi;
            N[4] = 4 * xi * zeta;
            N[5] = 4 * eta * zeta;
        }

        public void Preenche_Matriz_Coordenadas()
        {
            M_Coordenadas[0, 0] = n1.x; M_Coordenadas[0, 1] = n1.y;

            M_Coordenadas[1, 0] = n2.x; M_Coordenadas[1, 1] = n2.y;

            M_Coordenadas[2, 0] = n3.x; M_Coordenadas[2, 1] = n3.y;

            M_Coordenadas[3, 0] = n4.x; M_Coordenadas[3, 1] = n4.y;

            M_Coordenadas[4, 0] = n5.x; M_Coordenadas[4, 1] = n5.y;

            M_Coordenadas[5, 0] = n6.x; M_Coordenadas[5, 1] = n6.y;

            for (int i = 0; i < 6; i++)
            {
                xe[i] = M_Coordenadas[i, 0];
                ye[i] = M_Coordenadas[i, 1];
            }
        }

        public void Avalia_Matriz_Derivadas(double eta, double xi, double zeta)
        {
            B_Iso[0, 0] = 4 * eta - 1;
            B_Iso[1, 1] = 4 * xi - 1;
            B_Iso[2, 2] = 4 * zeta - 1;

            B_Iso[3, 0] = 4 * xi;
            B_Iso[3, 1] = 4 * eta;

            B_Iso[4, 1] = 4 * zeta;
            B_Iso[4, 2] = 4 * xi;

            B_Iso[5, 0] = 4 * zeta;
            B_Iso[5, 2] = 4 * eta;
        }

        public Matrix<double> M_Coordenadas_T, M_Derivadas_FuncoesForma_T;

        public void Area()
        {

        }

        public double Qx, Qy, ix, iy;
        public double ixy; // prod. de inercia
        public double ixx, iyy, ry, rx;
        public void TresPontosIntegracao(int ponto_de_gauss, ref double peso, ref double eta, ref double xi, ref double zeta)
        {
            peso = 0.33333;

            if (ponto_de_gauss == 1)
            {
                eta = 0.66667;
                xi = 0.16667;
                zeta = 0.16667;
            }
            else
            if (ponto_de_gauss == 2)
            {
                eta = 0.16667;
                xi = 0.66667;
                zeta = 0.16667;
            }
            else
            if (ponto_de_gauss == 3)
            {
                eta = 0.16667;
                xi = 0.16667;
                zeta = 0.66667;
            }
        }

        public void SeisPontosIntegracao(int ponto_de_gauss, ref double peso, ref double eta, ref double xi, ref double zeta)
        {

            double g1 = (1 / 18) * (8 - Math.Sqrt(10) + Math.Sqrt(38 - 44 * Math.Sqrt(2 / 5)));
            double g2 = (1 / 18) * (8 - Math.Sqrt(10) - Math.Sqrt(38 - 44 * Math.Sqrt(2 / 5)));
            double peso1 = (620 + (Math.Sqrt(213125 - 53320 * Math.Sqrt(10)))) / 3720;
            double peso2 = (620 - (Math.Sqrt(213125 - 53320 * Math.Sqrt(10)))) / 3720;

            if (ponto_de_gauss == 1)
            {
                eta = 1 - 2 * g2;
                xi = g2;
                zeta = g2;
                peso = peso2;
            }
            else
            if (ponto_de_gauss == 2)
            {
                eta = g2;
                xi = 1 - 2 * g2;
                zeta = g2;
                peso = peso2;
            }
            else
            if (ponto_de_gauss == 3)
            {
                eta = g2;
                xi = g2;
                zeta = 1 - 2 * g2;
                peso = peso2;
            }
            else
            if (ponto_de_gauss == 4)
            {
                eta = g1;
                xi = g1;
                zeta = 1 - 2 * g1;
                peso = peso1;
            }
            else
            if (ponto_de_gauss == 5)
            {
                eta = 1 - 2 * g1;
                xi = g1;
                zeta = g1;
                peso = peso1;
            }
            else
            if (ponto_de_gauss == 6)
            {
                eta = g1;
                xi = 1 - 2 * g1;
                zeta = g1;
                peso = peso1;
            }
        }

        public void PrimeirosMomentosDeArea()
        {
            double eta = 0.00, xi = 0.00, zeta = 0.00;
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

            for (int j = 1; j <= 3; j++)
            {
                TresPontosIntegracao(j, ref peso, ref eta, ref xi, ref zeta);

                Avalia_N(eta, xi, zeta);
                Avalia_Matriz_Derivadas(eta, xi, zeta);
                Avalia_Jacobiana(eta, xi, zeta);

                /*Avalia_M_Derivadas_FuncoesForma(1/3, 1 / 3, 1 / 3);
                Avalia_Jacobiana(1 / 3, 1 / 3, 1 / 3);*/

                Qx += peso * N * ye * jacobiano;
                Qy += peso * N * xe * jacobiano;
            }
        }
        public double jaa;
        public void SegundosMomentosDeArea()
        {
            double eta = 0.00, xi = 0.00, zeta = 0.00;
            ix = 0;
            iy = 0;
            ixy = 0;

            double peso = 0;

            for (int j = 1; j <= 3; j++)
            {
                TresPontosIntegracao(j, ref peso, ref eta, ref xi, ref zeta);

                Avalia_N(eta, xi, zeta);
                Avalia_Matriz_Derivadas(eta, xi, zeta);
                Avalia_Jacobiana(eta, xi, zeta);

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
              /*  Gl.glLineWidth(1);
                Gl.glColor3f(0, 0.2f, 0.7f);
                if (avulso)
                {
                    Gl.glColor3f(0.5f, 0.5f, 0.1f);
                    Gl.glLineWidth(2);
                }

                if (sel)
                {
                    Gl.glColor3f(1, 0, 0);
                    Gl.glLineWidth(4);
                }
                {
                    Gl.glBegin(Gl.GL_LINES);

                    Gl.glVertex2f(System.Convert.ToSingle(n1.x / Desenho.precisaoPixel + Desenho.ponto_zero[0]), System.Convert.ToSingle(n1.y / -Desenho.precisaoPixel + Desenho.ponto_zero[1]));
                    Gl.glVertex2f(System.Convert.ToSingle(n2.x / Desenho.precisaoPixel + Desenho.ponto_zero[0]), System.Convert.ToSingle(n2.y / -Desenho.precisaoPixel + Desenho.ponto_zero[1]));
                    Gl.glEnd();

                    Gl.glBegin(Gl.GL_LINES);
                    Gl.glVertex2f(System.Convert.ToSingle(n2.x / Desenho.precisaoPixel + Desenho.ponto_zero[0]), System.Convert.ToSingle(n2.y / -Desenho.precisaoPixel + Desenho.ponto_zero[1]));
                    Gl.glVertex2f(System.Convert.ToSingle(n3.x / Desenho.precisaoPixel + Desenho.ponto_zero[0]), System.Convert.ToSingle(n3.y / -Desenho.precisaoPixel + Desenho.ponto_zero[1]));
                    Gl.glEnd();

                    Gl.glBegin(Gl.GL_LINES);
                    Gl.glVertex2f(System.Convert.ToSingle(n3.x / Desenho.precisaoPixel + Desenho.ponto_zero[0]), System.Convert.ToSingle(n3.y / -Desenho.precisaoPixel + Desenho.ponto_zero[1]));
                    Gl.glVertex2f(System.Convert.ToSingle(n1.x / Desenho.precisaoPixel + Desenho.ponto_zero[0]), System.Convert.ToSingle(n1.y / -Desenho.precisaoPixel + Desenho.ponto_zero[1]));
                    Gl.glEnd();
                }*/
            }
        }
    }
}
