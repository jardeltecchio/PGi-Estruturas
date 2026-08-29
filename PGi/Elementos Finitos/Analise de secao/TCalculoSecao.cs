using MathNet.Numerics.Distributions;
using MathNet.Numerics.LinearAlgebra;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Media.Animation;
using System.Xml.Linq;
using Win32Interop.Structs;
using static Microsoft.WindowsAPICodePack.Shell.PropertySystem.SystemProperties.System;

namespace PG
{
    public class Linha_propriedades
    {
        public string propriedade { get; set; }
        public string descricao { get; set; }
        public string val { get; set; }
        public Linha_propriedades(string d, string v, string descr)
        {
            propriedade = d;
            descricao = descr;
            val = v;
        }
    }
    [Serializable]
    public class TPropriedades_Secao
    {
        public TPropriedades_Secao() { }

        public double area, anguloEixosPrincipais, areaCisalhamentoY, areaCisalhamentoZ, areaCisalhamentoYZ;
        public double inercia_torcao; // it - torção de saint venant
        public double raio_giracao_y, raio_giracao_z; //rx,ry
        public double prod_inercia;//ixy
        public double cy, cz;//Distância Z,Y do centróide até a fibra mais baixa/ mais esquerda da seção
        public double primeiro_momento_area_y, primeiro_momento_area_z;//qy, qz
        public double inercia_flexao_z, inercia_flexao_y;//ix,iy -> segundos momentos de area
        public double ModuloPlastico_y, ModuloPlastico_z;//Zy, Zz; 

        public double ModuloElastico_y_sup, ModuloElastico_z_dir; // wy,wz
        public double ModuloElastico_y_inf, ModuloElastico_z_esq; // wy,wz

        public double dist_modulo_plastico_z_do_centroide; //d_Zz 
        public double dist_modulo_plastico_y_do_centroide;// d_Zy
        public double centro_cis_y, centro_cis_z;//coordenadas do centro de cisalhamento
        public double prod_setorial_area_y, prod_setorial_area_z;  //iyw, izw
        public double Coef_Deformacao_Corte_y, Coef_Deformacao_Corte_z, Coef_Deformacao_Corte_yz;//kz,ky,kzy
        public double ConstanteEmpenamento; //cw
        public string SimetriaY, SimetriaZ;
        public bool SecaoComposta;
    }

        [Serializable]
    public partial class TCalculoSecao
    {
        public TPropriedades_Secao propriedades;

        public bool calculoOk = true;
        public bool Manual, somenteQ4, SecaoComposta;
        [NonSerialized]
        public T6_Secao[] tris6;
        public Q4_Secao[] quad4;

        [NonSerialized]
        public TMatrizBanda MatrizRigidez;
        static int Numero;
        public Gerenciador gerenciador;

        int Ndj = 1;  //número de vínculos possíveis em um nó

        public int nTris, nQuads, nNos;
        int i, j, k;
        public int LarguraBanda;

        public int Ngl,
                    NglTri6, NglQuad9, NglQuad4,
                    NLinhas,
                    NumeroDeRestricoes,
                    NumeroDeNosComRestricoes,
                    SequenciaPavimento, divBarrasPortico;

        public int NumeroRepeticoes;
        [NonSerialized]
        double[] resultados_torcao;
        double[] phi_cisalhamento, psi_cisalhamento; //resultados eixo x e eixo y - funcoes de cisalhamento dos dois eixos

        [NonSerialized]
        double[] forcas_torcao, forcas_cisalhamento;
        [NonSerialized]
        public int[] id;
        [NonSerialized]
        public int[] Linhas; //vetor que retorna o número da linha/coluna da matriz em que o gl se encontra
        [NonSerialized]
        double[] K_Mola;
        [NonSerialized]
        bool[] glRestrito;
        public TNoMEF[] nosElementos;
        public TCalculoSecao()
        {

        }

        public TCalculoSecao(Gerenciador _gerenciador, T6_Secao[] _tris6, Q4_Secao[] _quad4, bool q4, TNoMEF[] nos)
        {
            this.gerenciador = _gerenciador;
       
            this.nosElementos = nos;

            this.tris6 = _tris6;
            this.quad4 = _quad4;
            
            propriedades = new TPropriedades_Secao();

            Ndj = 1;
            nNos = nosElementos.Count() - 1;

            nTris = tris6.Count() - 1;

            somenteQ4 = q4;
            nQuads += quad4.Count() - 1;
        }
        private bool CriarVetores()
        {
            Ndj = 1;
            Ngl = (nNos * Ndj);
            NglTri6 = 6;
            NglQuad9 = 9;
            NglQuad4 = 4;

            NLinhas = Ngl - NumeroDeRestricoes;

            this.glRestrito = new bool[Ngl + 1];
            this.id = new int[Ngl + 1];
            this.Linhas = new int[Ngl + 1];
            this.forcas_torcao = new double[NLinhas + 1 + 1];
            this.forcas_cisalhamento = new double[NLinhas + 1 + 1];

            this.resultados_torcao = new double[NLinhas + 1 + 1];
            this.phi_cisalhamento = new double[NLinhas + 1 + 1];
            this.psi_cisalhamento= new double[NLinhas + 1 + 1];


            return true;
        }
   
        private bool CriarMatrizSFF(bool UsarDll)
        {
            if (MatrizRigidez != null)
                MatrizRigidez = null;

            MatrizRigidez = null;
            MatrizRigidez = new TMatrizBanda(NLinhas + 1, LarguraBanda, Ngl, null, choleskypadrao, false);

            return true;
        }


        int GetNumeroDeRestricoes()
        {
            int nn = 0;
            for (i = 1; i <= nNos; i++)
            {
                for (j = 1; j <= 6; j++)
                    if (nosElementos[i].Restricao[j] == true)
                        nn++;
            }

            return nn;
        }

        private bool DadosEstruturais()
        {
            try
            {
                //  NumeroDeNosComRestricoes = GetNumeroDeNosComRestricoes();*/

                //  if (!CriarVetores())
                //   throw new TErroPortico(this, "Pórtico Espacial  -  Erro dados estruturais.");

                int numero;

                // NumeroDeRestricoes = 1;

                CriarVetores();

                //preenche o vetor glRestrito que diz para cada GL se este está restrito ou nao

                for (i = 1; i <= nNos; i++)
                    nosElementos[i].Restricao = new bool[7];

                // nosTris6[1].Restricao[1] = true;
                // nosTris6[1].Restricao[2] = true;
                // nosTris6[1].Restricao[5] = true;
                //   nosTris6[1].Restricao[1] = true;

                /*  for (i = 1; i <= nNos; i++)
                  {
                      numero = nosTris6[i].numero;

                      for (j = 1; j <= 6; j++)
                          glRestrito[(numero - 1) * Ndj + j] = nosTris6[i].Restricao[j];
                  }*/

                //  glRestrito[1] = true;
                //  glRestrito[2] = true;
                //   glRestrito[5] = true;
                ///  glRestrito[11] = true;
                //   glRestrito[128] = true;

                //indice das equações para cada grau de liberdade em cada nó na matriz guardada no formato de banda
                int n1 = 0;
                for (j = 1; j <= Ngl; j++)
                {
                    //       id[j] = j - n1;

                    if (glRestrito[j])
                        n1++;

                    if (!glRestrito[j])
                        id[j] = j - n1;
                    else
                        id[j] = NLinhas + n1;
                }
                //calcula a largura da banda
                int nbi = 0;
                LarguraBanda = 0;

                for (i = 1; i <= nTris; i++)
                {
                    int ni = 99999;
                    int nf = -99999;

                    // for (int j = 0; j < 6; j++)
                    //     {
                    if (tris6[i].n1.numero < ni)
                        ni = tris6[i].n1.numero;

                    if (tris6[i].n2.numero < ni)
                        ni = tris6[i].n2.numero;

                    if (tris6[i].n3.numero < ni)
                        ni = tris6[i].n3.numero;

                    if (tris6[i].n4.numero < ni)
                        ni = tris6[i].n4.numero;

                    if (tris6[i].n5.numero < ni)
                        ni = tris6[i].n5.numero;

                    if (tris6[i].n6.numero < ni)
                        ni = tris6[i].n6.numero;

                    if (tris6[i].n1.numero > nf)
                        nf = tris6[i].n1.numero;

                    if (tris6[i].n2.numero > nf)
                        nf = tris6[i].n2.numero;

                    if (tris6[i].n3.numero > nf)
                        nf = tris6[i].n3.numero;

                    if (tris6[i].n4.numero > nf)
                        nf = tris6[i].n4.numero;

                    if (tris6[i].n5.numero > nf)
                        nf = tris6[i].n5.numero;

                    if (tris6[i].n6.numero > nf)
                        nf = tris6[i].n6.numero;

                    nbi = Ndj * (Math.Abs(nf - ni) + 1);

                    if (nbi > LarguraBanda)
                        LarguraBanda = nbi;
                }
            }
            catch (Exception ee)
            {

            }
            // LarguraBanda = NLinhas;
            return true;
        }
        bool choleskypadrao = false;

        double[][] mBanda_Dll;
        private bool MatrizDeRigidez(bool UsarDll)
        {
            int ir, ic, i1, i2, linAux;
            double coef1, coef2;
            // MsgCalculo("", "Pórtico - Gerando sistema de equações...", nBarras, false, false);
            #region solver2
            if (!choleskypadrao)
            {
                for (i = 1; i <= nTris; i++)
                {
                    for (j = 1; j <= NglTri6; j++)
                    {
                        i1 = tris6[i].GlGlobal[j];

                        //   if (!glRestrito[i1])
                        {
                            for (k = j; k <= NglTri6; k++)
                            {
                                i2 = tris6[i].GlGlobal[k];

                                //         if (!glRestrito[i2])
                                {
                                    ir = id[i1] - 1;
                                    ic = id[i2] - 1;

                                    coef1 = alglib.sparseget(MatrizRigidez.s, ir, ic);
                                    coef2 = tris6[i].MatrizGlobal[j, k];

                                    alglib.sparseset(MatrizRigidez.s, ir, ic, coef1 + coef2);
                                    alglib.sparseset(MatrizRigidez.s, ic, ir, coef1 + coef2);
                                }
                            }
                        }
                    }
                }

                {
                    for (i = 1; i <= nQuads; i++)
                    {
                        if (i == 782)
                            i = 782;
                        for (j = 1; j <= NglQuad4; j++)
                        {
                            i1 = quad4[i].GlGlobal[j];

                            //   if (!glRestrito[i1])
                            {
                                for (k = j; k <= NglQuad4; k++)
                                {
                                    i2 = quad4[i].GlGlobal[k];

                                    //         if (!glRestrito[i2])
                                    {
                                        ir = id[i1] - 1;
                                        ic = id[i2] - 1;

                                        coef1 = alglib.sparseget(MatrizRigidez.s, ir, ic);
                                        coef2 = quad4[i].MatrizGlobal[j, k];

                                        alglib.sparseset(MatrizRigidez.s, ir, ic, coef1 + coef2);
                                        alglib.sparseset(MatrizRigidez.s, ic, ir, coef1 + coef2);
                                    }
                                }
                            }
                        }
                    }
                }
            }
            #endregion
            else
            {
                for (i = 1; i <= nTris; i++)
                {
                    for (j = 1; j <= NglTri6; j++)
                    {
                        i1 = tris6[i].GlGlobal[j];

                        //    if (!glRestrito[i1])
                        {
                            for (k = j; k <= NglTri6; k++)
                            {
                                i2 = tris6[i].GlGlobal[k];

                                // if (!glRestrito[i2])
                                {
                                    ir = id[i1];
                                    ic = id[i2];

                                    if (ir >= ic)
                                    {
                                        linAux = ir;

                                        ir = ic;
                                        ic = linAux;
                                    }

                                    ic = ic - ir + 1;

                                    /* Zeros e um*/
                                    if (glRestrito[i1])
                                    {
                                        if (i1 == i2)
                                            MatrizRigidez.Sff[ir * LarguraBanda + ic] = 1;
                                        else
                                            MatrizRigidez.Sff[ir * LarguraBanda + ic] = 0;
                                    }
                                    else
                                    if (glRestrito[i2])
                                        MatrizRigidez.Sff[ir * LarguraBanda + ic] = 0;
                                    else
                                        MatrizRigidez.Sff[ir * LarguraBanda + ic] += tris6[i].MatrizGlobal[j, k];
                                }
                            }
                        }
                    }
                }
            }

            for (int i = 0; i <= NLinhas; i++)
            {
                alglib.sparseset(MatrizRigidez.s, NLinhas, i, 0);
            }
            for (int i = 0; i <= NLinhas; i++)
            {
                alglib.sparseset(MatrizRigidez.s, i, NLinhas, 0);
            }

            alglib.sparseset(MatrizRigidez.s, NLinhas, NLinhas, 1);


            int gl;

            return true;
        }
        public double J, ixx, iyy;
        public Matrix<double> w,f_t, phi,psi, wT, K, j_, wT_x_K_x_w;
        private bool Resultados_Torcao()
        {
            w = Matrix<double>.Build.Dense(NLinhas, 1); // vetor com o valor dos empenamentos, menos a ultima linha, pois é o multiplicador de lagrange, e eu posso ignorar da solução final

            K = Matrix<double>.Build.Dense(NLinhas, NLinhas);// matriz de rigidez, menos a ultima linha e a ultima coluna, pois são as constantes de valor '1' e eu posso ignorar da solução final

            for (int i = 1; i <= NLinhas; i++)
            {
                w[i - 1, 0] = resultados_torcao[i];
            }

            double coef;
            for (int i = 0; i < NLinhas; i++)
            {
                for (int j = 0; j < NLinhas; j++)
                {
                    coef = alglib.sparseget(MatrizRigidez.s, i, j);
                    K[i, j] = coef;
                }
            }

            wT = w.Transpose();
            wT_x_K_x_w = wT * K * w;
            j_ = propriedades.inercia_flexao_y + propriedades.inercia_flexao_z - wT_x_K_x_w;
            propriedades.inercia_torcao = j_[0,0];

            return true;
        }
        private bool Resultados_Cisalhamento()
        {
            double geomTriY = 0, geomTriZ = 0;
            double geomY = 0, geomZ = 0;
            double poisson = 0.2;
       
            double deltaS = (2 * (1 + poisson)) * (propriedades.inercia_flexao_y * propriedades.inercia_flexao_z - (propriedades.prod_inercia * propriedades.prod_inercia));

            f_t = Matrix<double>.Build.Dense(NLinhas+1, 1);

            phi = Matrix<double>.Build.Dense(NLinhas+1, 1);
            psi = Matrix<double>.Build.Dense(NLinhas+1, 1);

            for (int i = 1; i <= NLinhas; i++)
            {
                f_t[i - 1, 0] = forcas_torcao[i];
                phi[i - 1, 0] = phi_cisalhamento[i];
                psi[i - 1, 0] = psi_cisalhamento[i];
            }

           /* double maxPhi = 0;
            double maxPsi = 0;
            double maxForcaCisalhamento = 0;
            double max_W = 0;
            double maxFt = 0;

            for (int i = 1; i <= NLinhas; i++)
            {
                maxFt = Math.Max(maxFt,
                                 Math.Abs(forcas_torcao[i]));
            }

            for (int i = 1; i <= NLinhas; i++)
            {
                maxPhi = Math.Max(maxPhi, Math.Abs(phi_cisalhamento[i]));
                maxPsi = Math.Max(maxPsi, Math.Abs(psi_cisalhamento[i]));
                maxForcaCisalhamento = Math.Max(maxForcaCisalhamento, Math.Abs(forcas_cisalhamento[i]));
                max_W = Math.Max(max_W, Math.Abs(resultados_torcao[i]));

            }

            Console.WriteLine(maxPhi);
            Console.WriteLine(maxPsi);
            Console.WriteLine(maxForcaCisalhamento);
            Console.WriteLine(max_W); 
            Console.WriteLine(maxFt);*/

            double FT_PHI = (f_t.Transpose() * phi)[0, 0];
            double FT_PSI = (f_t.Transpose() * psi)[0, 0];

            for (int i = 1; i <= nQuads; i++)
            {
                quad4[i].phi[0, 0] = phi_cisalhamento[quad4[i].n1.numero];
                quad4[i].phi[1, 0] = phi_cisalhamento[quad4[i].n2.numero];
                quad4[i].phi[2, 0] = phi_cisalhamento[quad4[i].n3.numero];
                quad4[i].phi[3, 0] = phi_cisalhamento[quad4[i].n4.numero];

                quad4[i].psi[0, 0] = psi_cisalhamento[quad4[i].n1.numero];
                quad4[i].psi[1, 0] = psi_cisalhamento[quad4[i].n2.numero];
                quad4[i].psi[2, 0] = psi_cisalhamento[quad4[i].n3.numero];
                quad4[i].psi[3, 0] = psi_cisalhamento[quad4[i].n4.numero];

                quad4[i].w[0, 0] = w[quad4[i].n1.numero-1,0];
                quad4[i].w[1, 0] = w[quad4[i].n2.numero-1,0];
                quad4[i].w[2, 0] = w[quad4[i].n3.numero-1,0];
                quad4[i].w[3, 0] = w[quad4[i].n4.numero-1,0];

                quad4[i].Centros_Cisalhamento(propriedades.inercia_flexao_y, propriedades.inercia_flexao_z, propriedades.prod_inercia);

                geomY += quad4[i].xs;
                geomZ += quad4[i].ys;
            }

            for (int i = 1; i <= nTris; i++)
            {
                tris6[i].Centros_Cisalhamento(propriedades.inercia_flexao_y, propriedades.inercia_flexao_z, propriedades.prod_inercia);

                tris6[i].phi[0, 0] = phi_cisalhamento[tris6[i].n1.numero];
                tris6[i].phi[1, 0] = phi_cisalhamento[tris6[i].n2.numero];
                tris6[i].phi[2, 0] = phi_cisalhamento[tris6[i].n3.numero];
                tris6[i].phi[3, 0] = phi_cisalhamento[tris6[i].n4.numero];
                tris6[i].phi[4, 0] = phi_cisalhamento[tris6[i].n5.numero];
                tris6[i].phi[5, 0] = phi_cisalhamento[tris6[i].n6.numero];
                                                      
                tris6[i].psi[0, 0] = psi_cisalhamento[tris6[i].n1.numero];
                tris6[i].psi[1, 0] = psi_cisalhamento[tris6[i].n2.numero];
                tris6[i].psi[2, 0] = psi_cisalhamento[tris6[i].n3.numero];
                tris6[i].psi[3, 0] = psi_cisalhamento[tris6[i].n4.numero];
                tris6[i].psi[4, 0] = psi_cisalhamento[tris6[i].n5.numero];
                tris6[i].psi[5, 0] = psi_cisalhamento[tris6[i].n6.numero];

                tris6[i].w[0, 0] = w[tris6[i].n1.numero - 1, 0];
                tris6[i].w[1, 0] = w[tris6[i].n2.numero - 1, 0];
                tris6[i].w[2, 0] = w[tris6[i].n3.numero - 1, 0];
                tris6[i].w[3, 0] = w[tris6[i].n4.numero - 1, 0];
                tris6[i].w[4, 0] = w[tris6[i].n5.numero - 1, 0];
                tris6[i].w[5, 0] = w[tris6[i].n6.numero - 1, 0];

                geomTriY += tris6[i].xs;
                geomTriZ += tris6[i].ys;
            }

            geomY += geomTriY;
            geomZ += geomTriZ;

            geomY *= (0.5 * poisson);
            geomZ *= (0.5 * poisson);

            propriedades.centro_cis_y = (geomY - FT_PHI) / deltaS;
            propriedades.centro_cis_z = (geomZ + FT_PSI) / deltaS;

            if (Geom.Iguais(propriedades.centro_cis_y, 0, 1))
                propriedades.centro_cis_y = 0;

            if (Geom.Iguais(propriedades.centro_cis_z, 0, 1))
                propriedades.centro_cis_z = 0;

            CentroCisalhamento_Trefftz(); //acho melhor calcular os centros de cis. por esse, pois é mais simples e mais robusto

            CalculaConstanteEmpenamento();

            CalculaAreaCisalhamento();

            return true;
        }

        void CentroCisalhamento_Trefftz() //suposição das paredes finas
        {
            propriedades.prod_setorial_area_y = 0;  //ixw
            propriedades.prod_setorial_area_z = 0;  //iyw

            for (int i = 1; i <= nQuads; i++)
            {
                quad4[i].ProdutoSetorialArea();

                propriedades.prod_setorial_area_y += quad4[i].ixw[0];
                propriedades.prod_setorial_area_z += quad4[i].iyw[0];
            }

            for (int i = 1; i <= nTris; i++)
            {
                tris6[i].ProdutoSetorialArea();

                propriedades.prod_setorial_area_y += tris6[i].ixw[0];
                propriedades.prod_setorial_area_z += tris6[i].iyw[0];
            }

            propriedades.centro_cis_y = ((propriedades.prod_inercia * propriedades.prod_setorial_area_y) - (propriedades.inercia_flexao_z * propriedades.prod_setorial_area_z)) / ((propriedades.inercia_flexao_y * propriedades.inercia_flexao_z) - (propriedades.prod_inercia * propriedades.prod_inercia));
            propriedades.centro_cis_z = ((propriedades.inercia_flexao_y * propriedades.prod_setorial_area_y) - (propriedades.prod_inercia * propriedades.prod_setorial_area_z)) / ((propriedades.inercia_flexao_y * propriedades.inercia_flexao_z) - (propriedades.prod_inercia * propriedades.prod_inercia));

            if (Geom.Iguais(propriedades.centro_cis_y, 0, 1))
                propriedades.centro_cis_y = 0;

            if (Geom.Iguais(propriedades.centro_cis_z, 0, 1))
                propriedades.centro_cis_z = 0;
        }

        void CalculaConstanteEmpenamento()
        {
            double kx=0, ky=0, kxy=0, qw=0, iw=0;
            double poisson = 0.2;

            double deltaS = (2 * (1 + poisson)) * (propriedades.inercia_flexao_y * propriedades.inercia_flexao_z - (propriedades.prod_inercia * propriedades.prod_inercia));

            for (int i = 1; i <= nQuads; i++)
            {
                quad4[i].CoeficientesDeformacaoCisalhamento(poisson, propriedades.inercia_flexao_y, propriedades.inercia_flexao_z, propriedades.prod_inercia);

                kx  += quad4[i].kx[0,0];
                ky  += quad4[i].ky[0, 0];
                kxy += quad4[i].kxy[0, 0];

                quad4[i].MomentosEmpenamentos();
                qw += quad4[i].qw[0];
                iw += quad4[i].iw[0];
            }

            for (int i = 1; i <= nTris; i++)
            {
                tris6[i].CoeficientesDeformacaoCisalhamento(poisson, propriedades.inercia_flexao_y, propriedades.inercia_flexao_z, propriedades.prod_inercia);

                kx += tris6[i].kx[0, 0];
                ky += tris6[i].ky[0, 0];
                kxy += tris6[i].kxy[0, 0];

                tris6[i].MomentosEmpenamentos();
                qw += tris6[i].qw[0];
                iw += tris6[i].iw[0];
            }

            propriedades.Coef_Deformacao_Corte_y   = kx;
            propriedades.Coef_Deformacao_Corte_z   = ky;
            propriedades.Coef_Deformacao_Corte_yz  = kxy;

            propriedades.ConstanteEmpenamento = iw - ((qw * qw) / propriedades.area) - (propriedades.centro_cis_z * propriedades.prod_setorial_area_y) + (propriedades.centro_cis_y * propriedades.prod_setorial_area_z);
        }

        void CalculaAreaCisalhamento()
        {
            double poisson = 0.2;

            double deltaS = (2 * (1 + poisson)) * (propriedades.inercia_flexao_y * propriedades.inercia_flexao_z - (propriedades.prod_inercia * propriedades.prod_inercia));

            propriedades.areaCisalhamentoY = (deltaS * deltaS) / propriedades.Coef_Deformacao_Corte_y;
            propriedades.areaCisalhamentoZ = (deltaS * deltaS) / propriedades.Coef_Deformacao_Corte_z;
            propriedades.areaCisalhamentoYZ = (deltaS * deltaS) / propriedades.Coef_Deformacao_Corte_yz;

        }


        private bool SetMatrizesElementos()
        {

            for (int i = 1; i <= nTris; i++)
                tris6[i].Matriz_de_Rigidez();

            for (int i = 1; i <= nQuads; i++)
                quad4[i].Matriz_de_Rigidez();

            for (int i = 1; i <= nTris; i++)
            {
                tris6[i].SetGlGlobal();
            }

            for (int i = 1; i <= nQuads; i++)
            {
                if (i == 782)
                    i = 782;
                quad4[i].SetGlGlobal();
            }

            return true;
        }

        struct ForcasCasos
        {
            double[] forcasBarras;
            public int idCaso;
            public ForcasCasos(double[] forcas, int id)
            {
                forcasBarras = forcas;
                idCaso = id;
            }
        }
        List<ForcasCasos> forcas_x_caso;

        bool RefazerMalha, Testes;


        public bool CalculouEsforcos;
        public void Calcula_Area_Qx_Qy()
        {
            double areatotal = 0;
            double Qx = 0;
            double Qy = 0;
            T6_Secao tri;
            for (int i = 1; i <= nTris; i++)
            {
                tri = tris6[i];

                tri.Preenche_Matriz_Coordenadas();
                tri.Avalia_Matriz_Derivadas(1 / 3, 1 / 3, 1 / 3);
                tri.Avalia_Jacobiana(1 / 3, 1 / 3, 1 / 3);

                if (Geom.Iguais(tri.jacobiano, 0))
                {
                    tri.sel = true;
                    tri.avulso = true;
                    continue;
                }

                if (tri.jacobiano < 0)
                {
                    TNoMEF n1 = tri.n2;

                    tri.n2 = tri.n1;
                    tri.n1 = n1;

                    tri.n4 = (tri.n1 + tri.n2) / 2;
                    tri.n5 = (tri.n2 + tri.n3) / 2;
                    tri.n6 = (tri.n3 + tri.n1) / 2;

                    tri.Preenche_Matriz_Coordenadas();
                    tri.Avalia_Matriz_Derivadas(1 / 3, 1 / 3, 1 / 3);
                    tri.Avalia_Jacobiana(1 / 3, 1 / 3, 1 / 3);
                }

                tri.jaa = tri.jacobiano;
                areatotal += tri.jacobiano;
            }

            for (int i = 1; i <= nTris; i++)
            {
                tri = tris6[i];

                tri.PrimeirosMomentosDeArea();

                Qx += tri.Qx;
                Qy += tri.Qy;
            }

            for (int i = 1; i <= nQuads; i++)
            {
                quad4[i].Area();

                if (Geom.Iguais(quad4[i].area, 0))
                {
                    quad4[i].sel = true;
                    quad4[i].avulso = true;
                    quad4[i].id *= 1;
                    continue;
                }
                areatotal += quad4[i].area;

                quad4[i].PrimeirosMomentosDeArea();

                Qx += quad4[i].Qx;
                Qy += quad4[i].Qy;
            }

            this.propriedades.area = areatotal;
            this.propriedades.primeiro_momento_area_z = Qy;
            this.propriedades.primeiro_momento_area_y = Qx;

        }

        void SegundosMomentosArea()
        {
            double ix = 0;
            double iy = 0;
            double ixy = 0;
            T6_Secao tri;
            Q4_Secao quad;

            for (int i = 1; i <= nTris; i++)
            {
                tri = tris6[i];

                tri.SegundosMomentosDeArea();

                ix += tri.ix;
                iy += tri.iy;
                ixy += tri.ixy; // produto de inercia
            }

            for (int i = 1; i <= nQuads; i++)
            {
                quad4[i].SegundosMomentosDeArea();

                ix += quad4[i].ix;
                iy += quad4[i].iy;
                ixy += quad4[i].ixy; // produto de inercia
            }


            propriedades.inercia_flexao_y = ix - ((propriedades.primeiro_momento_area_y * propriedades.primeiro_momento_area_y) / propriedades.area);
            propriedades.inercia_flexao_z = iy - ((propriedades.primeiro_momento_area_z * propriedades.primeiro_momento_area_z) / propriedades.area);
            propriedades.prod_inercia     = ixy- ((propriedades.primeiro_momento_area_y * propriedades.primeiro_momento_area_z) / propriedades.area);

            if (Geom.Iguais(propriedades.prod_inercia / 10000, 0, 0.01))
                propriedades.prod_inercia = 0;

            propriedades.raio_giracao_y = (Math.Sqrt(propriedades.inercia_flexao_y / propriedades.area)); // raio de gir. x
            propriedades.raio_giracao_z = (Math.Sqrt(propriedades.inercia_flexao_z / propriedades.area));// raio de gir. y
        }

        void ModulosElasticos()
        {
            MiniMaxCoordenadas();

            propriedades.ModuloElastico_y_sup = propriedades.inercia_flexao_y / yMax;
            propriedades.ModuloElastico_y_inf = propriedades.inercia_flexao_y / (-yMin);

            propriedades.ModuloElastico_z_dir = propriedades.inercia_flexao_z / xMax;
            propriedades.ModuloElastico_z_esq = propriedades.inercia_flexao_z / (-xMin);

            propriedades.cy = Math.Abs(xMin);
            propriedades.cz = Math.Abs(yMin);
        }

        void ModulosPlasticos()
        {
            List<Q4_Secao> q4s = new List<Q4_Secao>();  
            List<T6_Secao> t6s = new List<T6_Secao>();

            for (int i = 1; i <= nQuads; i++)
                q4s.Add(quad4[i]);

            for (int i = 1; i <= nTris; i++)
                t6s.Add(tris6[i]);

            TAnalisePlastica analise = new TAnalisePlastica(q4s, t6s);
            analise.CriarElementos();
            analise.CalcularModulos();
            propriedades.ModuloPlastico_y = analise.moduloPlasticoY;
            propriedades.ModuloPlastico_z = analise.moduloPlasticoZ;
            propriedades.dist_modulo_plastico_y_do_centroide = analise.zPlastico;
            propriedades.dist_modulo_plastico_z_do_centroide = analise.yPlastico;

            if (Geom.Iguais(propriedades.dist_modulo_plastico_y_do_centroide, 0,1))
                propriedades.dist_modulo_plastico_y_do_centroide = 0;

            if (Geom.Iguais(propriedades.dist_modulo_plastico_z_do_centroide, 0, 1))
                propriedades.dist_modulo_plastico_z_do_centroide = 0;

        }

        double xMin = double.MaxValue;
        double yMin = double.MaxValue;

        double xMax = double.MinValue;
        double yMax = double.MinValue;
        public void MiniMaxCoordenadas()
        {
            for (int i = 1; i <= nNos; i++)
            {
                xMax = Math.Max(xMax, nosElementos[i].x);
                xMin = Math.Min(xMin, nosElementos[i].x);

                yMax = Math.Max(yMax, nosElementos[i].y);
                yMin = Math.Min(yMin, nosElementos[i].y);
            }
        }

        
        public double AnguloEixosPrincipais()
        {
            bool degenerado = Geom.Iguais(propriedades.inercia_flexao_y, propriedades.inercia_flexao_z) && Geom.Iguais(propriedades.prod_inercia, 0);

            if (!degenerado)
            {
                if (Geom.Iguais(propriedades.prod_inercia, 0))
                    propriedades.anguloEixosPrincipais = 0;
                else
                    propriedades.anguloEixosPrincipais = 0.5 * Math.Atan2(2.0 * propriedades.prod_inercia, propriedades.inercia_flexao_y - propriedades.inercia_flexao_z);
            }
            else
                propriedades.anguloEixosPrincipais = 0;

         //   propriedades.anguloEixosPrincipais *= -1;

            return propriedades.anguloEixosPrincipais;
        }

        public bool Calcula(bool calculaEsforco, bool UsarDll, bool resolverCisalhamento)
        {
            try
            {
                SegundosMomentosArea();
                ModulosElasticos();
                ModulosPlasticos();
                
                DadosEstruturais();

                SetMatrizesElementos();

                CalculouEsforcos = true;
                if (!calculaEsforco)
                {
                    CalculouEsforcos = false;
                    calculoOk = true;
                    return calculoOk;
                }

                CriarMatrizSFF(UsarDll);
                if (!MatrizDeRigidez(UsarDll)) 
                    System.Windows.Forms.MessageBox.Show("erro ao criar matriz global");
              
                calculoOk = ResolveEquacoes_Torcao(UsarDll);
                if (calculoOk)
                {
                    if (!Resultados_Torcao())
                        return false;

                    if (resolverCisalhamento)
                    {
                        CriarMatrizSFF(UsarDll);
                        if (!MatrizDeRigidez(UsarDll)) System.Windows.Forms.MessageBox.Show("erro ao criar matriz global");
                        calculoOk = ResolveEquacoes_Cisalhamento(UsarDll);
                        if (!Resultados_Cisalhamento())
                            return false;
                    }
                }
            }
            catch
            {

            }

            MatrizRigidez = null;
            glRestrito = null;
            id = null;
            Linhas = null;
            forcas_torcao = null;
            forcas_cisalhamento = null;

            resultados_torcao = null;
            psi_cisalhamento = null;
            phi_cisalhamento = null;

            return calculoOk;
        }
        public void MostraMatrizRigidez()
        {

        }
        bool Carregamentos_Torcao()
        {

            for (int i = 1; i <= nTris; i++)
                tris6[i].Forcas_Torcao();

            for (int i = 1; i <= nQuads; i++)
                quad4[i].Forcas_Torcao();

            int jr;

            double[] forcas_sem_restricao = new double[Ngl + 1];

            for (i = 1; i <= nTris; i++)
            {
                for (j = 1; j <= NglTri6; j++)
                {
                    jr = tris6[i].GlGlobal[j];

                    //if (!glRestrito[jr])
                    forcas_sem_restricao[jr] += tris6[i].F_torcao[j - 1, 0];
                }
            }

                for (i = 1; i <= nQuads; i++)
                {
                    for (j = 1; j <= NglQuad4; j++)
                    {
                        jr = quad4[i].GlGlobal[j];

                        //if (!glRestrito[jr])
                        forcas_sem_restricao[jr] += quad4[i].F_torcao[j - 1, 0];
                    }
                }


            for (j = 1; j <= Ngl; j++)
            {
                jr = id[j];

                //      if (!glRestrito[j])
                forcas_torcao[jr] = forcas_sem_restricao[j];
            }

            forcas_torcao[NLinhas + 1] = 0;

            return true;
        }
        bool Carregamentos_Cisalhamento(string eixo)
        {
;

            int jr;

            double[] forcas_sem_restricao = new double[Ngl + 1];

            if (eixo == "x")
            {
                for (int i = 1; i <= nTris; i++)
                    tris6[i].Forcas_Cisalhamento(0.2, propriedades.inercia_flexao_y, propriedades.inercia_flexao_z, propriedades.prod_inercia);

                for (int i = 1; i <= nQuads; i++)
                    quad4[i].Forcas_Cisalhamento(0.2, propriedades.inercia_flexao_y, propriedades.inercia_flexao_z, propriedades.prod_inercia);

                for (i = 1; i <= nTris; i++)
                {
                   for (j = 1; j <= NglTri6; j++)
                   {
                     jr = tris6[i].GlGlobal[j];

                    //if (!glRestrito[jr])
                     forcas_sem_restricao[jr] += tris6[i].Fx_Cisalhamento[j - 1, 0];
                    }
                }

                for (i = 1; i <= nQuads; i++)
                {
                    for (j = 1; j <= NglQuad4; j++)
                    {
                        jr = quad4[i].GlGlobal[j];

                        //if (!glRestrito[jr])
                        forcas_sem_restricao[jr] += quad4[i].Fx_Cisalhamento[j - 1, 0];
                    }
                }
            }
            else
            if (eixo == "y")
            {
                for (int i = 1; i <= nTris; i++)
                    tris6[i].Forcas_Cisalhamento(0.2, propriedades.inercia_flexao_y, propriedades.inercia_flexao_z, propriedades.prod_inercia);

                for (int i = 1; i <= nQuads; i++)
                    quad4[i].Forcas_Cisalhamento(0.2, propriedades.inercia_flexao_y, propriedades.inercia_flexao_z, propriedades.prod_inercia);

                forcas_sem_restricao = new double[Ngl + 1];
                double sumfy = 0;
                for (i = 1; i <= nTris; i++)
                {
                    for (j = 1; j <= NglTri6; j++)
                    {
                        jr = tris6[i].GlGlobal[j];

                        //if (!glRestrito[jr])
                        forcas_sem_restricao[jr] += tris6[i].Fy_Cisalhamento[j - 1, 0];
                    //    sumfy += tris6[i].Fy_Cisalhamento[j - 1, 0];
                    }
                }


                Console.WriteLine(sumfy);

                for (i = 1; i <= nQuads; i++)
                {
                    for (j = 1; j <= NglQuad4; j++)
                    {
                        jr = quad4[i].GlGlobal[j];

                        //if (!glRestrito[jr])
                        forcas_sem_restricao[jr] += quad4[i].Fy_Cisalhamento[j - 1, 0];

                        sumfy += quad4[i].Fy_Cisalhamento[j - 1, 0];
                    }
                }

                Console.WriteLine(sumfy);
            }

            for (j = 1; j <= Ngl; j++)
            {
               jr = id[j];
               forcas_cisalhamento[jr] = forcas_sem_restricao[j];
            }

            forcas_cisalhamento[NLinhas + 1] = 0;

            return true;
        }

        bool ResolveEquacoes_Torcao(bool UsarDll)
        {
            try
            {
                // if (gerenciador.ConfiguracoesPGi.CfgProjeto.sistema.Solver_cholesky)

                if (!MatrizRigidez.FatoraMatrizBanda("", "Resolvendo matriz...", UsarDll))
                    System.Windows.Forms.MessageBox.Show("erro ao fatorar");

                if (!Carregamentos_Torcao())
                    return false;

                int resultado = MatrizRigidez.ResolveMatrizBanda(null, ref forcas_torcao, ref resultados_torcao, "", "Seção - Resolvendo matriz", false, true);
                return true;
            }
            catch (Exception ex)
            {

                return false;
            }
        }


        bool ResolveEquacoes_Cisalhamento(bool UsarDll)
        {
            try
            {
                // if (gerenciador.ConfiguracoesPGi.CfgProjeto.sistema.Solver_cholesky)

                if (!MatrizRigidez.FatoraMatrizBanda("", "Resolvendo matriz...", UsarDll))
                    System.Windows.Forms.MessageBox.Show("erro ao fatorar");

                if (!Carregamentos_Cisalhamento("x"))
                    return false;
                int resultado = MatrizRigidez.ResolveMatrizBanda(null, ref forcas_cisalhamento, ref psi_cisalhamento, "", "Seção - Resolvendo matriz", false, true);

                
                if (!Carregamentos_Cisalhamento("y"))
                    return false;
                resultado = MatrizRigidez.ResolveMatrizBanda(null, ref forcas_cisalhamento, ref phi_cisalhamento, "", "Seção - Resolvendo matriz", false, true);

                return true;
            }
            catch (Exception ex)
            {

                return false;
            }
        }

        /*            PGiSolver.Solver pp = new PGiSolver.Class1();
                double se = PGiSolver.Class1.Foo();
                MessageBox.Show(se.ToString());*/

    }
}
