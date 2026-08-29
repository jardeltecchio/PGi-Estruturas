using MathNet.Numerics.Integration;
using OpenTK.Graphics.OpenGL;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms.VisualStyles;
using System.Xml.Linq;

namespace PG
{
    public class TCarregamentoPortico
    {
        public TEstrutura estrutura;
        TBarraPortico[] barrasPortico;
        public TCarregamentoPortico(TEstrutura _estrutura)
        {
            this.estrutura = _estrutura;
            barrasPortico = new TBarraPortico[estrutura.PorticoEspacial.nBarras];

            //copiar a partir do indice 1, pois a primeira barra do array é nula
            Array.Copy(estrutura.PorticoEspacial.barras, 1, barrasPortico, 0, estrutura.PorticoEspacial.nBarras);
        }
        vec3 vetorCarga;
        double cargaABS, angulocarga_Z, angulocarga_Y, angulocarga_X, cargaFator, cargaDecomposta_X, cargaDecomposta_Y, cargaDecomposta_Z;
        vec3 z_local, x_local;
        vec3 y_local;
        double[] forcasLocais = new double[13];
        double[] forcasSistemaGlobal = new double[13];
        int gl;
        private TBarraPortico EncontraBarraPortico(TBarraPortico[] bars,double d,out double a)
        {
            a = 0;

            double acumulado = 0;

            for (int i = 0; i < bars.Length; i++)
            {
                double L = bars[i].L;

                if (d <= acumulado + L)
                {
                    a = d - acumulado;

                    // Pequenos erros numéricos nos extremos
                    if (a < 0)
                        a = 0;

                    if (a > L)
                        a = L;

                    return bars[i];
                }

                acumulado += L;
            }

            return null;
        }

        public void GerarCargasNos_Casos(ref double[] forcasNos, int index_caso, TCasosCarga caso, ref bool[] glRestrito, ref int[] id)
        {
            List<TCargaPontual> cargas_do_caso = estrutura.cargaPontual.FindAll(o => o.Dados.idCaso == caso.ID).ToList();
            for (int j = 0; j < cargas_do_caso.Count; j++)
            {
                for (int i = 1; i <= estrutura.PorticoEspacial.nNos; i++)
                {
                    if (Geom.Iguais(cargas_do_caso[j].ponto.x, estrutura.PorticoEspacial.nos[i].x) && Geom.Iguais(cargas_do_caso[j].ponto.y, estrutura.PorticoEspacial.nos[i].y) && Geom.Iguais(cargas_do_caso[j].ponto.z, estrutura.PorticoEspacial.nos[i].z))
                    {
                        int numero = estrutura.PorticoEspacial.nos[i].Numero;

                        if (cargas_do_caso[j].Dados.DirecaoProjecao == 0) //x
                            gl = (numero - 1) * 6 + 1;
                        else
                        if (cargas_do_caso[j].Dados.DirecaoProjecao == 1) //y
                            gl = (numero - 1) * 6 + 3;
                        else
                        if (cargas_do_caso[j].Dados.DirecaoProjecao == 2) //z
                            gl = (numero - 1) * 6 + 2;
                        
                        double[] forcasNo = new double[6];

                        /*if (bars[j].temOffset)
                        {
                            double[] f2 = new double[13];
                            f2 = forcasLocais.ToArray();
                            TAlgebra.Multiplica_Matriz_Vetor(ref bars[j].MatrizOffset_Transposta, ref f2, ref forcasLocais, 12, 12);
                        }*/

                        if (!glRestrito[gl])
                        {
                            int jr = id[gl];
                            forcasNos[jr] += cargas_do_caso[j].Dados.valor;

                            TBarraPortico[] bars = Array.FindAll(barrasPortico, o => o.pIni.Numero == numero || o.pFin.Numero == numero);
                            if (bars != null)
                            {
                                if (bars[0].temOffset)
                                {
                                    double[] f2 = new double[13];
                                    f2 = forcasLocais.ToArray();
                                }
                            }
                        }                      
                    }
                }
            }
        }
        public void GerarCargasNos_Combinacoes(ref double[] forcas, int index_comb, TCombinacoes combinacao, ref bool[] glRestrito, ref int[] id)
        {
            for (int cc = 0; cc < combinacao.Coeficientes.Count; cc++)
            {        
                List<TCargaPontual> cargas_do_caso = estrutura.cargaPontual.FindAll(o => o.Dados.idCaso == combinacao.Coeficientes[cc].caso).ToList();

                double coef = combinacao.Coeficientes[cc].coef;

                for (int i = 0; i < cargas_do_caso.Count; i++)
                {
                    for (int j = 1; j <= estrutura.PorticoEspacial.nNos; j++)
                    {
                        if (Geom.Iguais(cargas_do_caso[i].ponto.x, estrutura.PorticoEspacial.nos[j].x) && Geom.Iguais(cargas_do_caso[i].ponto.y, estrutura.PorticoEspacial.nos[j].y) && Geom.Iguais(cargas_do_caso[i].ponto.z, estrutura.PorticoEspacial.nos[j].z))
                        {
                            int numero = estrutura.PorticoEspacial.nos[j].Numero;

                            if (cargas_do_caso[i].Dados.DirecaoProjecao == 0) //x
                                gl = (numero - 1) * 6 + 1;
                            else
                            if (cargas_do_caso[i].Dados.DirecaoProjecao == 1) //y
                                gl = (numero - 1) * 6 + 3;
                            else
                            if (cargas_do_caso[i].Dados.DirecaoProjecao == 2) //z
                                gl = (numero - 1) * 6 + 2;

                            if (!glRestrito[gl])
                            {
                                int jr = id[gl];
                                forcas[jr] += (cargas_do_caso[i].Dados.valor * coef);
                            }
                        }
                    }
                }
            }
        }

        public void GerarCargasLineares_Combinacoes(ref double[] forcasBarras, int index_comb, TCombinacoes combinacao, ref bool[] glRestrito, ref int[] id)
        {
            TBarraGenerica elemento = new TBarraGenerica();
            try 
            {
                for (int cc = 0; cc < combinacao.Coeficientes.Count; cc++)
                {
                    List<TCargaLinear> cargas_do_caso = estrutura.cargaLinear.FindAll(o => o.Dados.idCaso == combinacao.Coeficientes[cc].caso && o.Dados.distribuida).ToList();

                    double coef = combinacao.Coeficientes[cc].coef;

                    for (int i = 0; i < cargas_do_caso.Count; i++)
                    {
                        TCargaLinear cl = cargas_do_caso[i];
                        elemento = estrutura.barras.Find(o => o.IDBarra == cargas_do_caso[i].idBarra && o.Dados.Tipo != 4);
                        if (elemento != null)
                        {
                            if (elemento.seta_eixo_local_Z.l_principal.p1 == null ||
                                elemento.seta_eixo_local_Z.l_principal.p2 == null ||
                                elemento.seta_eixo_local_Y.l_principal.p1 == null ||
                                elemento.seta_eixo_local_Y.l_principal.p2 == null ||
                                elemento.seta_eixo_local_X.l_principal.p1 == null ||
                                elemento.seta_eixo_local_X.l_principal.p2 == null)
                                continue;

                            DecompoeCarga(elemento, cargas_do_caso[i], ref cargaDecomposta_X, ref cargaDecomposta_Y, ref cargaDecomposta_Z, coef);

                            TBarraPortico[] bars = Array.FindAll(barrasPortico, o => o.barraOriginal.IDBarra == elemento.IDBarra && !o.barra_de_articulacao);
                            for (int j = 0; j < bars.Count(); j++)
                            {
                                forcasLocais = new double[13];
                                forcasSistemaGlobal = new double[13];
                                if (!bars[j].barraRigida)
                                {
                                    for (int k = 1; k < 13; k++)
                                    {
                                        forcasLocais[k] += AcoesEngPerfeitoCargaDistribuida(k, cargaDecomposta_Z, "z", bars[j].L);
                                        forcasLocais[k] += AcoesEngPerfeitoCargaDistribuida(k, cargaDecomposta_Y, "y", bars[j].L);
                                    }

                                    forcasLocais[7] += cargaDecomposta_X * bars[j].L / 2;
                                    forcasLocais[1] += cargaDecomposta_X * bars[j].L / 2;
                                }

                                for (int k = 1; k < 13; k++)
                                    if (Geom.Iguais(forcasLocais[k], 0, 0.001))
                                        forcasLocais[k] = 0;
                                /*
                                if (bars[j].temOffset)
                                {
                                    double[] f2 = new double[13];
                                    f2 = forcasLocais.ToArray();
                                    TAlgebra.Multiplica_Matriz_Vetor(ref bars[j].MatrizOffset_Transposta, ref f2, ref forcasLocais, 12, 12);
                                }
                                */
                                if (bars[j].temOffset)
                                {
                                    double[] f2 = new double[13];

                                    //   bars[j].combinacoes_x_forcasLocais[index_comb].forcasLocais_sem_offset = forcasLocais.ToArray();

                                    for (int jj = 0; jj < 13; jj++)
                                        bars[j].combinacoes_x_forcasLocais[index_comb].forcasLocais_sem_offset[jj] += forcasLocais[jj];

                                    f2 = forcasLocais.ToArray();

                                    TAlgebra.Multiplica_Matriz_Vetor(ref bars[j].MatrizOffset_Transposta, ref f2, ref forcasLocais, 12, 12);
                                }

                                for (int jj = 0; jj < 13; jj++)
                                    bars[j].combinacoes_x_forcasLocais[index_comb].forcasLocais[jj] += forcasLocais[jj];

                                // bars[j].combinacoes_x_forcasLocais[index_comb].forcasLocais = forcasLocais.ToArray();

                                TAlgebra.Multiplica_Matriz_Vetor(ref bars[j].MatRotacaoTransposta, ref forcasLocais, ref forcasSistemaGlobal, 12, 12);

                                for (int k = 1; k <= 12; k++)
                                {
                                    gl = bars[j].GlGlobal[k];

                                    int jr = id[gl];

                                    if (!glRestrito[gl])
                                    {
                                        double g = forcasBarras[jr];

                                        forcasBarras[jr] = g + forcasSistemaGlobal[k];
                                    }
                                }
                            }
                        }
                    }
                }
            }
            catch (Exception ee)
            {
                System.Windows.Forms.MessageBox.Show(ee.Message +"\r erro na função GerarCargasLineares_Combinacoes. Comb: " + combinacao.Id + " Elemento: " + elemento.IDBarra);
            }        
        }

        public void GerarCargasLineares_Casos(ref double[] forcasBarras, int index_caso, TCasosCarga caso, ref bool[] glRestrito,ref int[] id)
        {
            TBarraGenerica elemento = new TBarraGenerica();
            try
            { 
            //copiar a partir do indice 1, pois a primeira barra do array é nula
                List<TCargaLinear> cargas_do_caso = estrutura.cargaLinear.FindAll(o => o.Dados.idCaso == caso.ID && o.Dados.distribuida).ToList();
                int i;
                for (i = 0; i < cargas_do_caso.Count; i++)
                {
                    TCargaLinear CargaLinear = cargas_do_caso[i];
                    if (i == 981)
                        i = 981;

                    elemento = estrutura.barras.Find(o => o.IDBarra == CargaLinear.idBarra && o.Dados.Tipo != 4);
                    if (elemento != null)
                    {

                        if (elemento.seta_eixo_local_Z.l_principal.p1 == null || 
                            elemento.seta_eixo_local_Z.l_principal.p2 == null ||
                            elemento.seta_eixo_local_Y.l_principal.p1 == null ||
                            elemento.seta_eixo_local_Y.l_principal.p2 == null ||
                            elemento.seta_eixo_local_X.l_principal.p1 == null ||
                            elemento.seta_eixo_local_X.l_principal.p2 == null)
                            continue;

                        //acha as barras de portico associadas ao elemento
                        TBarraPortico[] bars = Array.FindAll(barrasPortico, o => o.barraOriginal.IDBarra == elemento.IDBarra && !o.barra_de_articulacao);

                      /*  double d,b = 0;
                        double a = 0;
                        TBarraPortico barraCarga = null;
                        if (CargaLinear.Dados.concentrada)
                        {
                            if (CargaLinear.Dados.posicaoRelativa)
                                d = CargaLinear.Dados.d * elemento.L;
                            else
                                d = CargaLinear.Dados.d;                         

                            barraCarga = EncontraBarraPortico(bars,d,out a);

                            if (barraCarga == null)
                                continue;

                            b = barraCarga.L - a;

                        }*/

                        DecompoeCarga(elemento, CargaLinear, ref cargaDecomposta_X, ref cargaDecomposta_Y, ref cargaDecomposta_Z, 1);

                        for (int j = 0; j < bars.Count(); j++)
                        {
                           /* if (CargaLinear.Dados.concentrada)
                                if (bars[j].IDBarra != barraCarga.IDBarra)
                                   continue;*/

                            forcasLocais = new double[13];
                            forcasSistemaGlobal = new double[13];

                            if (!bars[j].barraRigida)
                            {
                               /* if (CargaLinear.Dados.concentrada)
                                {
                                    // A carga concentrada atua somente na barra
                                    // encontrada por EncontraBarraPortico()

                                    for (int k = 1; k < 13; k++)
                                    {
                                        forcasLocais[k] += AcoesEngPerfeitoCargaConcentrada(k,cargaDecomposta_Z,"z",barraCarga.L,a);
                                        forcasLocais[k] += AcoesEngPerfeitoCargaConcentrada(k,cargaDecomposta_Y,"y",barraCarga.L,a);
                                        forcasLocais[k] +=AcoesEngPerfeitoCargaConcentrada(k,cargaDecomposta_X,"x",barraCarga.L,a);
                                    }
                                }
                                else*/
                                {
                                    for (int k = 1; k < 13; k++)
                                    {
                                        forcasLocais[k] += AcoesEngPerfeitoCargaDistribuida(k, cargaDecomposta_Z, "z", bars[j].L);
                                        forcasLocais[k] += AcoesEngPerfeitoCargaDistribuida(k, cargaDecomposta_Y, "y", bars[j].L);
                                    }

                                    forcasLocais[7] += cargaDecomposta_X * bars[j].L / 2;
                                    forcasLocais[1] += cargaDecomposta_X * bars[j].L / 2;
                                }
                            }

                            for (int k = 1; k < 13; k++)
                                if (Geom.Iguais(forcasLocais[k], 0, 0.001))
                                    forcasLocais[k] = 0;

                            if (bars[j].temOffset)
                            {
                                double[] f2 = new double[13];

                                //bars[j].casos_x_forcasLocais[index_caso].forcasLocais_sem_offset = forcasLocais.ToArray();

                                for (int jj = 0; jj < 13; jj++)
                                    bars[j].casos_x_forcasLocais[index_caso].forcasLocais_sem_offset[jj] += forcasLocais[jj];

                                f2 = forcasLocais.ToArray();

                                TAlgebra.Multiplica_Matriz_Vetor(ref bars[j].MatrizOffset_Transposta, ref f2, ref forcasLocais, 12, 12);
                            }

                            //      bars[j].casos_x_forcasLocais.Add(new ForcasLocais_Barra(index_caso));
                            // ForcasLocais_Barra fl = bars[j].casos_x_forcasLocais[bars[j].casos_x_forcasLocais.Count-1];


                            // bars[j].casos_x_forcasLocais[index_caso].forcasLocais = forcasLocais.ToArray();//tava dando bug e entao eu comentei
                            for (int jj = 0; jj < 13; jj++)
                                bars[j].casos_x_forcasLocais[index_caso].forcasLocais[jj] += forcasLocais[jj];

                            TAlgebra.Multiplica_Matriz_Vetor(ref bars[j].MatRotacaoTransposta, ref forcasLocais, ref forcasSistemaGlobal, 12, 12);

                            for (int k = 1; k <= 12; k++)
                            {
                                gl = bars[j].GlGlobal[k];

                                int jr = id[gl];

                                if (!glRestrito[gl])
                                {
                                    double g = forcasBarras[jr];

                                    forcasBarras[jr] = g + forcasSistemaGlobal[k];
                                }
                            }
                        }
                    }
                }
            }
            catch(Exception ee)
            {
                System.Windows.Forms.MessageBox.Show(ee.Message + "\r erro na função GerarCargasLineares_Casos. Caso: " + caso.ID + " Elemento: " + elemento.IDBarra);
            }
        }
        void DecompoeCarga(TBarraGenerica elemento, TCargaLinear cl, ref double cargax, ref double cargay, ref double cargaz, double coef)
        {
            if (cl.Dados.ProjecaoGlobal == 0)
            {
                z_local = Geom.CriaVetor(elemento.seta_eixo_local_Z.l_principal.p1, elemento.seta_eixo_local_Z.l_principal.p2, true);
                y_local = Geom.CriaVetor(elemento.seta_eixo_local_Y.l_principal.p1, elemento.seta_eixo_local_Y.l_principal.p2, true);
                x_local = Geom.CriaVetor(elemento.seta_eixo_local_X.l_principal.p1, elemento.seta_eixo_local_X.l_principal.p2, true);

                if (cl.Dados.distribuida)
                   vetorCarga = Geom.CriaVetor(cl.setas[3].l_principal.p1, cl.setas[3].l_principal.p2, true);
                else
                if (cl.Dados.concentrada)
                    vetorCarga = Geom.CriaVetor(cl.setas[0].l_principal.p1, cl.setas[0].l_principal.p2, true);

                cargaABS = Math.Abs(cl.Dados.valor * coef);
                // if (coot > 0)
                //      cargaABS *= 1;
                angulocarga_Z = Geom.AnguloEntre2Vetores(vetorCarga, z_local);
                cargaFator = Math.Cos(angulocarga_Z * Const.PIDiv180);
                cargaDecomposta_Z = cargaFator * cargaABS;

                angulocarga_Y = Geom.AnguloEntre2Vetores(vetorCarga, y_local);
                cargaFator = Math.Cos(angulocarga_Y * Const.PIDiv180);
                cargaDecomposta_Y = cargaFator * cargaABS;

                angulocarga_X = Geom.AnguloEntre2Vetores(vetorCarga, x_local);
                cargaFator = Math.Cos(angulocarga_X * Const.PIDiv180);
                cargaDecomposta_X = cargaFator * cargaABS;

                if (Geom.Iguais(cargaDecomposta_X, 0, 0.001))
                    cargaDecomposta_X = 0;
                if (Geom.Iguais(cargaDecomposta_Y, 0, 0.001))
                    cargaDecomposta_Y = 0;
                if (Geom.Iguais(cargaDecomposta_Z, 0, 0.001))
                    cargaDecomposta_Z = 0;
            }
            else
            {
                cargaDecomposta_X = 0;
                cargaDecomposta_Y = 0;
                cargaDecomposta_Z = 0;

                if (cl.Dados.DirecaoProjecao == 0)
                    cargaDecomposta_X = cl.Dados.valor * coef;
                else
                if (cl.Dados.DirecaoProjecao == 1)
                    cargaDecomposta_Y = cl.Dados.valor * coef;
                else
                if (cl.Dados.DirecaoProjecao == 2)
                    cargaDecomposta_Z = cl.Dados.valor * coef;
            }
        }
        private double AcoesEngPerfeitoCargaConcentrada(int indice,double carga,string eixo,double L,double a)
        {
            double b = L - a;

            double valor = 0;

            if (eixo == "x")
            {
                switch (indice)
                {
                    // Força normal no nó inicial
                    case 1:
                        valor = carga * b / L;
                        break;

                    // Força normal no nó final
                    case 7:
                        valor = carga * a / L;
                        break;
                }
            }
            else
            if (eixo == "y")
            {
                switch (indice)
                {
                    // Cortante no nó inicial
                    case 3:
                        valor = carga *
                            b * b * (3 * a + b) /
                            Math.Pow(L, 3);
                        break;

                    // Momento no nó inicial
                    case 5:
                        valor = -carga *
                            a * b * b /
                            Math.Pow(L, 2);
                        break;

                    // Cortante no nó final
                    case 9:
                        valor = carga *
                            a * a * (a + 3 * b) /
                            Math.Pow(L, 3);
                        break;

                    // Momento no nó final
                    case 11:
                        valor = carga *
                            a * a * b /
                            Math.Pow(L, 2);
                        break;
                }
            }
            else
            if (eixo == "z")
            {
                switch (indice)
                {
                    // Cortante no nó inicial
                    case 2:
                        valor = carga *
                            b * b * (3 * a + b) /
                            Math.Pow(L, 3);
                        break;

                    // Momento no nó inicial
                    case 6:
                        valor = carga *
                            a * b * b /
                            Math.Pow(L, 2);
                        break;

                    // Cortante no nó final
                    case 8:
                        valor = carga *
                            a * a * (a + 3 * b) /
                            Math.Pow(L, 3);
                        break;

                    // Momento no nó final
                    case 12:
                        valor = -carga *
                            a * a * b /
                            Math.Pow(L, 2);
                        break;
                }
            }

            return valor;
        }

        double AcoesEngPerfeitoCargaDistribuida(int indice, double CargaDistribuida, string eixo, double comprimentoBarra)
        {
            double valor = 0;
            if (eixo == "z")
            {
                switch (indice)
                {
                    case 1: valor = 0; break; // axial
                    case 2: valor = (CargaDistribuida * comprimentoBarra) / 2; break; // cortante
                    case 3: valor = 0; break;
                    case 4: valor = 0; break;
                    case 5: valor = 0; break;
                    case 6: valor = (CargaDistribuida * Math.Pow(comprimentoBarra, 2)) / 12; break; // fletor

                    case 7: valor = 0; break; // axial
                    case 8: valor = (CargaDistribuida * comprimentoBarra) / 2; break; // cortante
                    case 9: valor = 0; break;
                    case 10: valor = 0; break;
                    case 11: valor = 0; break;
                    case 12: valor = -(CargaDistribuida * Math.Pow(comprimentoBarra, 2)) / 12; break; // fletor           
                }
            }
            else
            if (eixo == "y")
            {
                switch (indice)
                {
                    case 1: valor = 0; break; // axial
                    case 2: valor = 0; break;
                    case 3: valor = (CargaDistribuida * comprimentoBarra) / 2; break; // cortante
                    case 4: valor = 0; break;
                    case 5: valor = -(CargaDistribuida * Math.Pow(comprimentoBarra, 2)) / 12; break; // fletor
                    case 6: valor = 0; break;

                    case 7: valor = 0; break; // axial
                    case 8: valor = 0; break;
                    case 9: valor = (CargaDistribuida * comprimentoBarra) / 2; break; // cortante
                    case 10: valor = 0; break;
                    case 11: valor = (CargaDistribuida * Math.Pow(comprimentoBarra, 2)) / 12; break; // fletor      
                    case 12: valor = 0; break;
                }
            }
            return valor;
        }
    }
}
