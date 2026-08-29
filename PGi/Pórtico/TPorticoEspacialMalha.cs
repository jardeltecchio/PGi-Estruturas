using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.NetworkInformation;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using static PG.Geom;

namespace PG
{
    public partial class TPorticoEspacial
    {
        void MalhaTeste()
        {
        /*    nos[++nNos] = new TNoPortico(0, 0, 0, 0, 0, 2, nNos + 1, null, null, false, true);
            nos[++nNos] = new TNoPortico(0, 0, 50, 0, 0, 0, nNos + 1, null, null, false, true);
            nos[++nNos] = new TNoPortico(0, 0, 100, 0, 0, 0, nNos + 1, null, null, false, true);

            barras[++nBarras] = (new TBarraPortico(nos[1], nos[2], null, null, 1, 2, 2.4, 2.1, 0, 0, 0, 0, 0, 0, null, false, true, true,
                                                      false, nBarras, false));
            barras[++nBarras] = (new TBarraPortico(nos[2], nos[3], null, null, 1, 2, 2.4, 2.1, 0, 0, 0, 0, 0, 0, null, false, true, true,
                                                      false, nBarras, false));*/
            

            
            
          nos[++nNos] = new TNoPortico(0, 0, 0, 0, 0, 2, nNos + 1, null, null, false, true);
          nos[++nNos] = new TNoPortico(0, 0, 50, 0, 0, 0, nNos + 1, null, null, false, true);
          nos[++nNos] = new TNoPortico(0, 0, 100, 0, 0, 0, nNos + 1, null, null, false, true);
          nos[++nNos] = new TNoPortico(0, 0, 150, 0, 0, 0, nNos + 1, null, null, false, true);
          nos[++nNos] = new TNoPortico(0, 0, 200, 0, 0, 0, nNos + 1, null, null, false, true);

          barras[++nBarras] = (new TBarraPortico(nos[1], nos[2], null, null, 1, 2, 2.4, 2.1, 0, 0, 0, 0, 0, 0, null, false, true, true,
                                                    false, nBarras, false));
          barras[++nBarras] = (new TBarraPortico(nos[2], nos[3], null, null, 1, 2, 2.4, 2.1, 0, 0, 0, 0, 0, 0, null, false, true, true,
                                                    false, nBarras, false));
          barras[++nBarras] = (new TBarraPortico(nos[3], nos[4], null, null, 1, 2, 2.4, 2.1, 0, 0, 0, 0, 0, 0, null, false, true, true,
                                                    false, nBarras, false));
          barras[++nBarras] = (new TBarraPortico(nos[4], nos[5], null, null, 1, 2, 2.4, 2.1, 0, 0, 0, 0, 0, 0, null, false, true, true,
                                                    false, nBarras, false));


          nos[++nNos] = new TNoPortico(50,  0, 200, 0, 0, 0, nNos + 1, null, null, false, true);
          nos[++nNos] = new TNoPortico(100, 0, 200, 0, 0, 0, nNos + 1, null, null, false, true);
          nos[++nNos] = new TNoPortico(150, 0, 200, 0, 0, 0, nNos + 1, null, null, false, true);
          nos[++nNos] = new TNoPortico(200, 0, 200, 0, 0, 0, nNos + 1, null, null, false, true);

          barras[++nBarras] = (new TBarraPortico(nos[5], nos[6], null, null, 1, 2, .4, .1, 0, 0, 0, 0, 0, 0, null, true, false, true,
                                                    false, nBarras, false));
          barras[++nBarras] = (new TBarraPortico(nos[6], nos[7], null, null, 1, 2, .4, .1, 0, 0, 0, 0, 0, 0, null, true, false, true,
                                                    false, nBarras, false));
          barras[++nBarras] = (new TBarraPortico(nos[7], nos[8], null, null, 1, 2, .4, .1, 0, 0, 0, 0, 0, 0, null, true, false, true,
                                                    false, nBarras, false));
          barras[++nBarras] = (new TBarraPortico(nos[8], nos[9], null, null, 1, 2, .4, .1, 0, 0, 0, 0, 0, 0, null, true, false, true,
                                                    false, nBarras, false));
          nos[6].Carga[2] = -1;
          nos[7].Carga[2] = -1;
          nos[8].Carga[2] = -1;

      /*    nos[++nNos] = new TNoPortico(200, 0, 150, 0, 0, 0, nNos + 1, null, null, false, true);
          nos[++nNos] = new TNoPortico(200, 0, 100, 0, 0, 0, nNos + 1, null, null, false, true);
          nos[++nNos] = new TNoPortico(200, 0, 50, 0, 0, 0, nNos + 1, null, null, false, true);
          nos[++nNos] = new TNoPortico(200, 0, 0, 0, 0, 2, nNos + 1, null, null, false, true);

          barras[++nBarras] = (new TBarraPortico(nos[9], nos[10], null, null, 1, 2, 2.4, 2.1, 0, 0, 0, 0, 0, 0, null, false, true, true,
                                                    false, nBarras, false));
          barras[++nBarras] = (new TBarraPortico(nos[10], nos[11], null, null, 1, 2, 2.4, 2.1, 0, 0, 0, 0, 0, 0, null, false, true, true,
                                                    false, nBarras, false));
          barras[++nBarras] = (new TBarraPortico(nos[11], nos[12], null, null, 1, 2, 2.4, 2.1, 0, 0, 0, 0, 0, 0, null, false, true, true,
                                                    false, nBarras, false));
          barras[++nBarras] = (new TBarraPortico(nos[12], nos[13], null, null, 1, 2, 2.4, 2.1, 0, 0, 0, 0, 0, 0, null, false, true, true,
                                                    false, nBarras, false)); RefazerMalha = false;*/

        }
   
        //public int nNos = 0;
       //public int nBarras = 0;
        int jk;
        public double tam_barra_viga = 70;
        public double tam_barra_pilar = 70;
        public int LocalizaNo(ref double x, ref double y, ref double z)
        {
            for (jk = 1; jk <= nNos; jk++)
                if ((Object)nos[jk] != null)
                    if (Geom.Iguais(x, nos[jk].x, 0.001) && Geom.Iguais(y, nos[jk].y, 0.001) && Geom.Iguais(z, nos[jk].z, 0.001))
                        return jk;

            return -1;
        }

        public int LocalizaNo(double x, double y, double z)
        {
            for (jk = 1; jk <= nNos; jk++)
                if ((Object)nos[jk] != null)
                    if (Geom.Iguais(x, nos[jk].x, 0.001) && Geom.Iguais(y, nos[jk].y, 0.001) && Geom.Iguais(z, nos[jk].z, 0.001))
                        return jk;

            return -1;
        }
        public bool LocalizaNo(ref TNoGrelha no)
        {
            for (jk = 1; jk <= nNos; jk++)
                if ((Object)nos[jk] == (Object)no)
                    return true;

            return false;
        }
        [NonSerialized]
        DateTime span1, span2;
        public double maxBanda, maxBandaReordenacao;
        [NonSerialized]
        double Diferenca_Entre_NoI_e_NoF;
        [NonSerialized]
        int noI, noF;
        [NonSerialized]
        TNoPortico noIni, noFin;
        [NonSerialized]
        int max_linhas;
        void InicializaNosBarras()
        {
            this.nos = new TNoPortico[50000];
            this.barras = new TBarraPortico[50000];
            nNos = 0;
            nBarras = 0;
            nNos = 0;
            nBarras = 0;
        //    max_linhas = linhasCount;
        }
        [NonSerialized]
        TNoPortico[] NoExt;


        bool PilarGerado(int num)
        {
            foreach (int i in PilaresGerados)
                if (i == num)
                    return true;

            return false;
        }
        List<int> PilaresGerados;

        public double PesoTotal;
        void CalculaPesoTotal()
        {
            if (!Manual)
            {
                PesoTotal = 0;
                for (int kk = 0; kk < pilares.Count; kk++)
                    PesoTotal += pilares[kk].PesoDoLance;
                for (int kk = 0; kk < Pavimentos.Count; kk++)
                    PesoTotal += Pavimentos[kk].PP;
            }
        }

        void Finaliza()
        {
            maxBanda = 0;
            int nbi;

            if (!this.Manual)
            {
                TBarraPortico[] barrasAux = new TBarraPortico[50000];
                TNoPortico[] nosAux = new TNoPortico[50000];

                int cb = 0, cn = 0;

                for (int i = 1; i <= nBarras; i++)
                    if ((Object)barras[i] != null)
                        barrasAux[++cb] = barras[i];

                for (int i = 1; i <= nNos; i++)
                    if ((Object)nos[i] != null)
                        nosAux[++cn] = nos[i];

                //reinicia contagem e insere novamente as barras, pois pode haver barra com valor nulo na lista
                nBarras = 0;
                nNos = 0;

                barras = new TBarraPortico[cb + 1];
                for (int i = 1; i <= cb; i++)
                    if ((Object)barrasAux[i] != null)
                        barras[++nBarras] = barrasAux[i];

                //reposiciona o nós e atribui o número correto, pois pode haver nós nulos na lista anterior provindos da correção da malha
                nos = new TNoPortico[cn + 1];
                for (int i = 1; i <= cn; i++)
                    if ((Object)nosAux[i] != null)
                    {
                        nos[++nNos] = nosAux[i];
                        nos[nNos].Numero = nNos;
                    }
            }

            //Atribui as características para as barras finais...
     //       desenho.BarraGrelha.Clear();
          //  LayersByIdPrincipal[Lay.GrelhaLajes].Obj.Clear();

           // MsgCalculo("Pórtico espacial", "Pórtico - Atualizando objetos...", nBarras);

            for (int i = 1; i <= nBarras; i++)
            {
                if ((Object)barras[i] != null)
                {
                    barras[i].angulo = FuncoesGerais.atand((barras[i].pIni.y - barras[i].pFin.y) / (barras[i].pIni.x - barras[i].pFin.x));
                    barras[i].direcaoX = barras[i].barraHorizontal;
                    barras[i].direcaoY = barras[i].barraVertical;
                    barras[i].barraVertical = Geom.Iguais(barras[i].pIni.x, barras[i].pFin.x);
                    barras[i].barraHorizontal = Geom.Iguais(barras[i].pIni.y, barras[i].pFin.y);
                    barras[i].barraObliqua = (!barras[i].barraVertical && !barras[i].barraHorizontal);
                    barras[i].IDBarra = i;

                 //   barras[i].Pavimento = this;
                  //  barras[i].layer = this.LayersByIdPrincipal[Lay.GrelhaLajes];
                  /* LayersByIdPrincipal[Lay.GrelhaLajes].AddObject(barras[i]);

                    LayersByIdPrincipal[Lay.GrelhaLajes].AddObject(barras[i].Grips[0]);
                    LayersByIdPrincipal[Lay.GrelhaLajes].AddObject(barras[i].Grips[1]);
                    LayersByIdPrincipal[Lay.GrelhaLajes].AddObject(barras[i].Grips[2]);

                    desenho.Grips.Add(barras[i].Grips[0]);
                    desenho.Grips.Add(barras[i].Grips[1]);
                    desenho.Grips.Add(barras[i].Grips[2]);

                    desenho.BarraGrelha.Add(barras[i]);

                    LayersByIdPrincipal[Lay.GrelhaLajes].Congelado = true;*/

                    nbi = 6 * (Math.Abs(barras[i].pIni.Numero - barras[i].pFin.Numero) + 1);
                    if (nbi > maxBanda)
                      maxBanda = nbi;
                }
            };

            span2 = DateTime.Now;

            RefazerMalha = false;
        }

        //calcula prop das barras (E,I,G,J) e molas dos nós
        TBarraPortico b1;

        public struct BarrasDivididasXBarrasPortico
        {
            public TBarraGenerica barraDividida;
            public List<TBarraPortico> barrasPortico;
            public BarrasDivididasXBarrasPortico(TBarraGenerica b)
            {
                barraDividida = b;
                barrasPortico = new List<TBarraPortico>();
            }
        }
        public List<BarrasDivididasXBarrasPortico> barrasDividas_X_barrasPortico;
       
        private bool CargaConcentradaNaBarra(TBarraGenerica barra, TCargaLinear CargaLinear)
        {
           vec3 p_1, p_2, p_3, p_4;
            vec3 p_r = new vec3(0, 0, 0);
            vec3 ponto = new vec3(0, 0, 0);
 
            p_1 = new vec3(barra.pIni.x, barra.pIni.y, barra.pIni.z);
            p_2 = new vec3(barra.pFin.x, barra.pFin.y, barra.pFin.z);

            p_3 = new vec3(CargaLinear.setas[0].l_principal.p2.x, CargaLinear.setas[0].l_principal.p2.y, CargaLinear.setas[0].l_principal.p2.z);

            vec3 pontoToque = new vec3(0);

            var r1 = Geom.ClassificarPontoNoSegmento(p_3, p_1, p_2);

            if (r1.Posicao == PosicaoNoSegmento.Fora)
                return false;
            
            return true;
        }

        public void VerificaValencia_pIni_pFin(TBarraGenerica bar, ref int ini,ref int fin)
        {
            List<TPonto> pontos = new List<TPonto>();
            vec3 p_1 = new vec3(bar.pIni.x, bar.pIni.y, bar.pIni.z);
            vec3 p_2 = new vec3(bar.pFin.x, bar.pFin.y, bar.pFin.z);
            vec3 p_3;
            vec3 p_4;
            vec3 pontoToque = new vec3(0);
           
            foreach (TApoio a in apoios)
            {
                if (Geom.Iguais(bar.pIni.x, a.pIni.x) &&
                   Geom.Iguais(bar.pIni.y, a.pIni.y) &&
                   Geom.Iguais(bar.pIni.z, a.pIni.z))
                    ini++;

                if (Geom.Iguais(bar.pFin.x, a.pIni.x) &&
                   Geom.Iguais(bar.pFin.y, a.pIni.y) &&
                   Geom.Iguais(bar.pFin.z, a.pIni.z))
                    fin++;
            }

            foreach (TBarraGenerica b in barrasgenericas)
            {
                if (b.IDBarra != bar.IDBarra)
                {
                    p_3 = new vec3(b.pIni.x, b.pIni.y, b.pIni.z);
                    p_4 = new vec3(b.pFin.x, b.pFin.y, b.pFin.z);

                    if (Geom.PontoTocaAresta(p_1, p_3, p_4, ref pontoToque, true))
                        ini++;

                    if (Geom.PontoTocaAresta(p_2, p_3, p_4, ref pontoToque, true))
                        fin++;
                }
            }
        }

        void CriarBarras(bool metodo2)
        {
            int ni, u;

            TNoPortico p1 = null, p2 = null;

            int div = divBarrasPortico;
            List<TBarraGenerica> barrasDivididas = new List<TBarraGenerica>();

            foreach (TBarraGenerica b in barrasgenericas)
            {
                barrasDivididas.Add((TBarraGenerica)b.Clone());
                barrasDivididas[barrasDivididas.Count - 1].IDBarra = b.IDBarra;
            }

            List<TCargaLinear> cargas_concentradas  = gerenciador.formDesenho.Estrutura.cargaLinear.FindAll(o => o.Dados.concentrada).ToList();

            TIntersecoes.RetornaBarrasDivididas2(ref barrasDivididas, cargas_concentradas, gerenciador.formDesenho.Estrutura.cargaPontual);

            TBarraGenerica barragenerica;

            barrasDividas_X_barrasPortico = new List<BarrasDivididasXBarrasPortico>();

            foreach (TBarraGenerica b in barrasgenericas)
            {
                if (!barrasDivididas.Exists(o => o.IDBarra == b.IDBarra))
                  barrasDivididas.Add(b);
            }

            for (int i = 0; i < barrasDivididas.Count(); i++)
            {
                if (barrasDivididas[i].comprimento >= 1 && barrasDivididas[i].comprimento <= 1.5)
                    div = 5;
                else
                if (barrasDivididas[i].comprimento < 0.5)
                    div = 2;
                else
                if (barrasDivididas[i].comprimento < 1)
                    div = 4;
                else
                    div = divBarrasPortico;
                
                div = Math.Min(div, divBarrasPortico);
                // div = 2;
                barrasDividas_X_barrasPortico.Add(new BarrasDivididasXBarrasPortico(barrasDivididas[i]));

                barragenerica = barrasgenericas.Find(o => o.IDBarra == barrasDivididas[i].IDBarra);

                if (barragenerica.Dados.Tipo == 4)
                    div = 1;

                bool art_ini = (barrasDivididas[i].Dados.Articulacao_my == 1 || barrasDivididas[i].Dados.Articulacao_my == 2 ||
                                barrasDivididas[i].Dados.Articulacao_mz == 1 || barrasDivididas[i].Dados.Articulacao_mz == 2);

                bool art_fim = (barrasDivididas[i].Dados.Articulacao_my == 1 || barrasDivididas[i].Dados.Articulacao_my == 3 ||
                                barrasDivididas[i].Dados.Articulacao_mz == 1 || barrasDivididas[i].Dados.Articulacao_mz == 3);

                int val_ini = 1;
                int val_fin = 1;

                VerificaValencia_pIni_pFin(barragenerica, ref val_ini, ref val_fin);

                if (art_ini)
                {
                    if ((Geom.Iguais(barragenerica.pIni.x, barrasDivididas[i].pIni.x) && Geom.Iguais(barragenerica.pIni.y, barrasDivididas[i].pIni.y) && Geom.Iguais(barragenerica.pIni.z, barrasDivididas[i].pIni.z))
                     || (Geom.Iguais(barragenerica.pIni.x, barrasDivididas[i].pFin.x) && Geom.Iguais(barragenerica.pIni.y, barrasDivididas[i].pFin.y) && Geom.Iguais(barragenerica.pIni.z, barrasDivididas[i].pFin.z)))
                        art_ini = true;
                    else
                        art_ini = false;

                    if (val_ini == 1) // se nao tem nada chegando no ponto inicial, ou seja, so existe uma barra chegando nele mesmo (valencia == 1)
                        art_ini = false;
                }

                if (art_fim)
                {
                    if ((Geom.Iguais(barragenerica.pFin.x, barrasDivididas[i].pIni.x) && Geom.Iguais(barragenerica.pFin.y, barrasDivididas[i].pIni.y) && Geom.Iguais(barragenerica.pFin.z, barrasDivididas[i].pIni.z))
                     || (Geom.Iguais(barragenerica.pFin.x, barrasDivididas[i].pFin.x) && Geom.Iguais(barragenerica.pFin.y, barrasDivididas[i].pFin.y) && Geom.Iguais(barragenerica.pFin.z, barrasDivididas[i].pFin.z)))
                        art_fim = true;
                    else
                        art_fim = false;

                    if (val_fin == 1)// se nao tem nada chegando no ponto final
                        art_fim = false;
                }

                barrasDivididas[i].OrientaSecaoNoEspaco();
               
                bool barragenerica_ex_ini = !Geom.Iguais(barragenerica.Dados.ex_i, 0);
                bool barragenerica_ex_fim = !Geom.Iguais(barragenerica.Dados.ex_f, 0);
                if (barragenerica_ex_ini)
                {
                    if ((Geom.Iguais(barragenerica.pIni.x, barrasDivididas[i].pIni.x) && Geom.Iguais(barragenerica.pIni.y, barrasDivididas[i].pIni.y) && Geom.Iguais(barragenerica.pIni.z, barrasDivididas[i].pIni.z))
                     || (Geom.Iguais(barragenerica.pIni.x, barrasDivididas[i].pFin.x) && Geom.Iguais(barragenerica.pIni.y, barrasDivididas[i].pFin.y) && Geom.Iguais(barragenerica.pIni.z, barrasDivididas[i].pFin.z)))
                        barragenerica_ex_ini = true;
                    else
                        barragenerica_ex_ini = false;
                }

                if (barragenerica_ex_fim)
                {
                    if ((Geom.Iguais(barragenerica.pFin.x, barrasDivididas[i].pIni.x) && Geom.Iguais(barragenerica.pFin.y, barrasDivididas[i].pIni.y) && Geom.Iguais(barragenerica.pFin.z, barrasDivididas[i].pIni.z))
                     || (Geom.Iguais(barragenerica.pFin.x, barrasDivididas[i].pFin.x) && Geom.Iguais(barragenerica.pFin.y, barrasDivididas[i].pFin.y) && Geom.Iguais(barragenerica.pFin.z, barrasDivididas[i].pFin.z)))
                        barragenerica_ex_fim = true;
                    else
                        barragenerica_ex_fim = false;
                }


                /*  if (barragenerica_ex_ini)
                  {
                      if (Geom.Iguais(barras[nBarras].pIni.x, barragenerica.pIni.x) && Geom.Iguais(barras[nBarras].pIni.y, barragenerica.pIni.y) && Geom.Iguais(barras[nBarras].pIni.z, barragenerica.pIni.z))
                      {
                          barras[nBarras].tem_ex_i = true;
                          barras[nBarras].ex_i = barragenerica.Dados.ex_i / 1000;

                          //         barras[nBarras].pIni_offset += eixoLocalX * barras[nBarras].ex_i;
                      }

                      if (Geom.Iguais(barras[nBarras].pFin.x, barragenerica.pIni.x) && Geom.Iguais(barras[nBarras].pFin.y, barragenerica.pIni.y) && Geom.Iguais(barras[nBarras].pFin.z, barragenerica.pIni.z))
                      {
                          barras[nBarras].tem_ex_f = true;
                          barras[nBarras].ex_f = barragenerica.Dados.ex_i / 1000;
                      }
                  }
                  if (barragenerica_ex_fim)
                  {
                      if (Geom.Iguais(barras[nBarras].pIni.x, barragenerica.pFin.x) && Geom.Iguais(barras[nBarras].pIni.y, barragenerica.pFin.y) && Geom.Iguais(barras[nBarras].pIni.z, barragenerica.pFin.z))
                      {
                          barras[nBarras].tem_ex_i = true;
                          barras[nBarras].ex_i = barragenerica.Dados.ex_f / 1000;
                      }

                      if (Geom.Iguais(barras[nBarras].pFin.x, barragenerica.pFin.x) && Geom.Iguais(barras[nBarras].pFin.y, barragenerica.pFin.y) && Geom.Iguais(barras[nBarras].pFin.z, barragenerica.pFin.z))
                      {
                          barras[nBarras].tem_ex_f = true;
                          barras[nBarras].ex_f = barragenerica.Dados.ex_f / 1000;
                      }
                  }*/

                //x2, y2, e z2 sao as coordenadas com offset somente transversal, sem offset em x local
                //x local é feito mais abaixo
                SubdivideBarra(ref CoordsOffsets, div, barrasDivididas[i].pIni_Offset.x2, -barrasDivididas[i].pIni_Offset.y2, -barrasDivididas[i].pIni_Offset.z2,
                                   barrasDivididas[i].pFin_Offset.x2, -barrasDivididas[i].pFin_Offset.y2, -barrasDivididas[i].pFin_Offset.z2, art_ini, art_fim,
                                   barragenerica_ex_ini, barragenerica_ex_fim, barragenerica.Dados.ex_i / 1000, barragenerica.Dados.ex_f / 1000);

                SubdivideBarra(ref Divisoes, div, barrasDivididas[i].pIni.x, barrasDivididas[i].pIni.y, barrasDivididas[i].pIni.z,
                                   barrasDivididas[i].pFin.x, barrasDivididas[i].pFin.y, barrasDivididas[i].pFin.z, art_ini, art_fim,
                                   barragenerica_ex_ini, barragenerica_ex_fim, barragenerica.Dados.ex_i / 1000, barragenerica.Dados.ex_f / 1000);

                u = -1;
                for (int j = 0; j < Divisoes.Count; j++)
                {
                    ni = LocalizaNo(Divisoes[j].x, Divisoes[j].y, Divisoes[j].z);

                    if (ni == -1)
                    {
                        nos[++nNos] = new TNoPortico(Divisoes[j].x, Divisoes[j].y, Divisoes[j].z, CoordsOffsets[j].x, CoordsOffsets[j].y, CoordsOffsets[j].z, 0, nNos + 1);

                        foreach (TApoio a in apoios)
                        {
                            if (Geom.Iguais(Divisoes[j].x, a.pIni.x) &&
                                Geom.Iguais(Divisoes[j].y, a.pIni.y) &&
                                Geom.Iguais(Divisoes[j].z, a.pIni.z))
                            {
                                nos[nNos].restrDX = a.Dados.restringe_dx;
                                nos[nNos].restrDY = a.Dados.restringe_dy;
                                nos[nNos].restrDZ = a.Dados.restringe_dz;

                                nos[nNos].restrRX = a.Dados.restringe_rx;
                                nos[nNos].restrRY = a.Dados.restringe_ry;
                                nos[nNos].restrRZ = a.Dados.restringe_rz;

                                if (!a.Dados.restringe_dx)
                                {
                                    nos[nNos].K_Mola_DX = a.Dados.mola_dx * 10;// tf/m para kn/m
                                    nos[nNos].PossuiMolaDX = true;
                                }
                                if (!a.Dados.restringe_dy)
                                {
                                    nos[nNos].K_Mola_DY = a.Dados.mola_dy * 10;// tf/m para kn/m
                                    nos[nNos].PossuiMolaDY = true;
                                }
                                if (!a.Dados.restringe_dz)
                                {
                                    nos[nNos].K_Mola_DZ = a.Dados.mola_dz * 10;// tf/m para kn/m
                                    nos[nNos].PossuiMolaDZ = true;
                                }
                                if (!a.Dados.restringe_rx)
                                {
                                    nos[nNos].K_Mola_RX = a.Dados.mola_rx * 10;// tf.m/m para kn.m/m 
                                    nos[nNos].PossuiMolaRX = true;
                                }
                                if (!a.Dados.restringe_ry)
                                {
                                    nos[nNos].K_Mola_RY = a.Dados.mola_ry * 10;// tf.m/m para kn.m/m 
                                    nos[nNos].PossuiMolaRY = true;
                                }
                                if (!a.Dados.restringe_rz)
                                {
                                    nos[nNos].K_Mola_RZ = a.Dados.mola_rz * 10;// tf.m/m para kn.m/m 
                                    nos[nNos].PossuiMolaRZ = true;
                                }
                            }
                        }

                        p1 = nos[nNos];
                    }
                    else
                        p1 = nos[ni];

                    if (j > 0)
                    {
                        barras[++nBarras] = (new TBarraPortico(p2, p1, 1, 2, 2.4, 2.1, 0, 0, 0, 0, 0, 0, null, false, true, true, false, nBarras, false));

                        barrasDividas_X_barrasPortico[barrasDividas_X_barrasPortico.Count - 1].barrasPortico.Add(barras[nBarras]);

                        if (barragenerica.Dados.Tipo == 4)
                          barras[nBarras].barraRigida = true;

                        barras[nBarras].pIni_offset = new TNoPortico(CoordsOffsets[j-1].x, CoordsOffsets[j - 1].y, CoordsOffsets[j - 1].z);
                        barras[nBarras].pFin_offset = new TNoPortico(CoordsOffsets[j].x, CoordsOffsets[j].y, CoordsOffsets[j].z);

                       //barras[nBarras].pIni_offset = new TNoPortico(p2.x, p2.y, p2.z);
                       //barras[nBarras].pFin_offset = new TNoPortico(p1.x, p1.y, p1.z);
                       
                        barras[nBarras].Rgb[0] = barragenerica.Rgb[0];
                        barras[nBarras].Rgb[1] = barragenerica.Rgb[1];
                        barras[nBarras].Rgb[2] = barragenerica.Rgb[2];
                        barras[nBarras].barraOriginal = barragenerica;

                        barras[nBarras].Dados = barragenerica.Dados;

                        /*   barras[nBarras].A1 = barragenerica.Dados.secao.area;
                           barras[nBarras].Iz1 = barragenerica.Dados.secao.inercia_flexao_z;
                           barras[nBarras].Iy1 = barragenerica.Dados.secao.inercia_flexao_y;
                           barras[nBarras].J1 = barragenerica.Dados.secao.inercia_torcao;*/

                        barras[nBarras].A1  = barragenerica.Dados.secao.propriedades.area/1000000;
                        barras[nBarras].Iy1 = barragenerica.Dados.secao.inercia_flexao_z/1e12;
                        barras[nBarras].Iz1 = barragenerica.Dados.secao.inercia_flexao_y / 1e12;
                        barras[nBarras].J1  = barragenerica.Dados.secao.propriedades.inercia_torcao / 1e12; 

                        barras[nBarras].E1 = materiais[barragenerica.Dados.secao.idMaterial].E * 1000; // mpa para kpa
                        barras[nBarras].G1 = materiais[barragenerica.Dados.secao.idMaterial].G * 1000; // mpa para kpa 

                        barras[nBarras].barraOriginal_Vertical = (Geom.Iguais(Math.Abs((barragenerica.pFin.z - barragenerica.pIni.z) / barragenerica.comprimento), 1));

                        barras[nBarras].comprimento = (Math.Sqrt(Math.Pow(barras[nBarras].pIni.x - barras[nBarras].pFin.x, 2) + Math.Pow(barras[nBarras].pIni.y - barras[nBarras].pFin.y, 2) + Math.Pow(barras[nBarras].pIni.z - barras[nBarras].pFin.z, 2)));
                        double alfa2 = 0;
                        
                        alfa2 = (barragenerica.Dados.secao.propriedades.anguloEixosPrincipais / (Const.PIDiv180));
                        
                        barras[nBarras].alfa = barragenerica.Dados.anguloRotacao;
                        barras[nBarras].AlfaAlterado = barragenerica.AlfaAlterado;
                        barras[nBarras].AlfaEixosPrincipaisAlterado = barragenerica.AlfaEixosPrincipaisAlterado;
                        
                        barras[nBarras].L = barras[nBarras].comprimento;

                      //  barras[nBarras].AlfaAlterado = barragenerica.AlfaAlterado;
                  //      barras[nBarras].barraVertical = barras[nBarras].barraOriginal_Vertical;

                      /*  if (Geom.Iguais(barras[nBarras].pFin.x, barras[nBarras].pIni.x))
                            barras[nBarras].pFin.x = barras[nBarras].pIni.x;
                        if (Geom.Iguais(barras[nBarras].pFin.y, barras[nBarras].pIni.y))
                            barras[nBarras].pFin.y = barras[nBarras].pIni.y;
                        if (Geom.Iguais(barras[nBarras].pFin.z, barras[nBarras].pIni.z))
                            barras[nBarras].pFin.z = barras[nBarras].pIni.z;*/

                        // perTorc = (100 - (double)barras[i].TrechoViga.Dados.redTorcao) / 100;
                        // barras[nBarras].G1 *= perTorc;// .15;

                        barras[nBarras].BarraDeExtremidade = false;
                        if ((j == 1) || (j == Divisoes.Count - 1))
                        {
                            barras[nBarras].BarraDeExtremidade = true;

                            /* 0 - nenhum
                             * 1 - Início e fim
                              2 - Início
                              3 - Fim*/
                            bool barragenerica_ArticulacaoMY_Ini = barragenerica.Dados.Articulacao_my == 1 || (barragenerica.Dados.Articulacao_my == 2);
                            bool barragenerica_ArticulacaoMY_Fin = barragenerica.Dados.Articulacao_my == 1 || (barragenerica.Dados.Articulacao_my == 3);

                            bool barragenerica_ArticulacaoMZ_Ini = barragenerica.Dados.Articulacao_mz == 1 || (barragenerica.Dados.Articulacao_mz == 2);
                            bool barragenerica_ArticulacaoMZ_Fin = barragenerica.Dados.Articulacao_mz == 1 || (barragenerica.Dados.Articulacao_mz == 3);

                            if (art_fim || art_ini)
                            {
                                barras[nBarras].KMy_Inicio = 1e14;
                                barras[nBarras].KMy_Final = 1e14;
                                barras[nBarras].KMz_Inicio = 1e14;
                                barras[nBarras].KMz_Final = 1e14;
                            }

                            vec3 vetor_eixo_X = new vec3(barras[nBarras].pFin.x, barras[nBarras].pFin.y, barras[nBarras].pFin.z)
                                               - new vec3(barras[nBarras].pIni.x, barras[nBarras].pIni.y, barras[nBarras].pIni.z);

                            TNoPortico eixoLocalX = new TNoPortico(vetor_eixo_X.x, vetor_eixo_X.y, vetor_eixo_X.z);
                            eixoLocalX.Normalize();

                            if (barragenerica_ex_ini)
                            {
                                if (Geom.Iguais(barras[nBarras].pIni.x, barragenerica.pIni.x) && Geom.Iguais(barras[nBarras].pIni.y, barragenerica.pIni.y) && Geom.Iguais(barras[nBarras].pIni.z, barragenerica.pIni.z))
                                    barras[nBarras].barraRigida = true;

                                if (Geom.Iguais(barras[nBarras].pFin.x, barragenerica.pIni.x) && Geom.Iguais(barras[nBarras].pFin.y, barragenerica.pIni.y) && Geom.Iguais(barras[nBarras].pFin.z, barragenerica.pIni.z))
                                    barras[nBarras].barraRigida = true;
                            }
                            if (barragenerica_ex_fim)
                            {
                                if (Geom.Iguais(barras[nBarras].pIni.x, barragenerica.pFin.x) && Geom.Iguais(barras[nBarras].pIni.y, barragenerica.pFin.y) && Geom.Iguais(barras[nBarras].pIni.z, barragenerica.pFin.z))
                                    barras[nBarras].barraRigida = true;

                                if (Geom.Iguais(barras[nBarras].pFin.x, barragenerica.pFin.x) && Geom.Iguais(barras[nBarras].pFin.y, barragenerica.pFin.y) && Geom.Iguais(barras[nBarras].pFin.z, barragenerica.pFin.z))
                                    barras[nBarras].barraRigida = true;
                            }

                            // barragenerica_ex_ini = !Geom.Iguais(barragenerica.Dados.ex_i, 0);
                            //   barragenerica_ex_fim = !Geom.Iguais(barragenerica.Dados.ex_f, 0);

                            /*if (barragenerica_ex_ini)
                            {
                                if (Geom.Iguais(barras[nBarras].pIni.x, barragenerica.pIni.x) && Geom.Iguais(barras[nBarras].pIni.y, barragenerica.pIni.y) && Geom.Iguais(barras[nBarras].pIni.z, barragenerica.pIni.z))
                                {
                                    barras[nBarras].tem_ex_i = true;
                                    barras[nBarras].ex_i = barragenerica.Dados.ex_i/1000;
                                }

                                if (Geom.Iguais(barras[nBarras].pFin.x, barragenerica.pIni.x) && Geom.Iguais(barras[nBarras].pFin.y, barragenerica.pIni.y) && Geom.Iguais(barras[nBarras].pFin.z, barragenerica.pIni.z))
                                {
                                    barras[nBarras].tem_ex_f = true;
                                    barras[nBarras].ex_f = barragenerica.Dados.ex_i / 1000;
                                }
                            }
                            if (barragenerica_ex_fim)
                            {
                                if (Geom.Iguais(barras[nBarras].pIni.x, barragenerica.pFin.x) && Geom.Iguais(barras[nBarras].pIni.y, barragenerica.pFin.y) && Geom.Iguais(barras[nBarras].pIni.z, barragenerica.pFin.z))
                                {
                                    barras[nBarras].tem_ex_i = true;
                                    barras[nBarras].ex_i = barragenerica.Dados.ex_f / 1000;
                                }

                                if (Geom.Iguais(barras[nBarras].pFin.x, barragenerica.pFin.x) && Geom.Iguais(barras[nBarras].pFin.y, barragenerica.pFin.y) && Geom.Iguais(barras[nBarras].pFin.z, barragenerica.pFin.z))
                                {
                                    barras[nBarras].tem_ex_f = true;
                                    barras[nBarras].ex_f = barragenerica.Dados.ex_f / 1000;
                                }
                            }*/

                            if (art_ini)
                            {
                                if (Geom.Iguais(barras[nBarras].pIni.x, barragenerica.pIni.x) && Geom.Iguais(barras[nBarras].pIni.y, barragenerica.pIni.y) && Geom.Iguais(barras[nBarras].pIni.z, barragenerica.pIni.z))
                                {
                                    if (barragenerica_ArticulacaoMY_Ini)
                                    {
                                        barras[nBarras].articulacao_my_ini = true;
                                        barras[nBarras].KMy_Inicio = barragenerica.Dados.k_my;
                                        barras[nBarras].KMy_Final = 1e14;
                                    }

                                    if (barragenerica_ArticulacaoMZ_Ini)
                                    {
                                        barras[nBarras].articulacao_mz_ini = true;
                                        barras[nBarras].KMz_Inicio = barragenerica.Dados.k_mz;
                                        barras[nBarras].KMz_Final  = 1e14;
                                    }
                                }
                                else
                                if (Geom.Iguais(barras[nBarras].pFin.x, barragenerica.pIni.x) && Geom.Iguais(barras[nBarras].pFin.y, barragenerica.pIni.y) && Geom.Iguais(barras[nBarras].pFin.z, barragenerica.pIni.z))
                                {
                                    if (barragenerica_ArticulacaoMY_Ini)
                                    {
                                        barras[nBarras].articulacao_my_fin = true;
                                        barras[nBarras].KMy_Final = barragenerica.Dados.k_my;
                                        barras[nBarras].KMy_Inicio = 1e14;
                                    }

                                    if (barragenerica_ArticulacaoMZ_Ini)
                                    {
                                        barras[nBarras].articulacao_mz_fin = true;
                                        barras[nBarras].KMz_Final = barragenerica.Dados.k_mz;
                                        barras[nBarras].KMz_Inicio = 1e14;
                                    }
                                }
                            }

                            if (art_fim)
                            {
                                if (Geom.Iguais(barras[nBarras].pIni.x, barragenerica.pFin.x) && Geom.Iguais(barras[nBarras].pIni.y, barragenerica.pFin.y) && Geom.Iguais(barras[nBarras].pIni.z, barragenerica.pFin.z))
                                {
                                    if (barragenerica_ArticulacaoMY_Fin)
                                    {
                                        barras[nBarras].articulacao_my_ini = true;
                                        barras[nBarras].KMy_Inicio = barragenerica.Dados.k_my;
                                        barras[nBarras].KMy_Final = 1e14;
                                    }

                                    if (barragenerica_ArticulacaoMZ_Fin)
                                    {
                                        barras[nBarras].articulacao_mz_ini = true;
                                        barras[nBarras].KMz_Inicio = barragenerica.Dados.k_mz;
                                        barras[nBarras].KMz_Final = 1e14;
                                    }
                                }
                                else
                                if (Geom.Iguais(barras[nBarras].pFin.x, barragenerica.pFin.x) && Geom.Iguais(barras[nBarras].pFin.y, barragenerica.pFin.y) && Geom.Iguais(barras[nBarras].pFin.z, barragenerica.pFin.z))
                                {
                                    if (barragenerica_ArticulacaoMY_Fin)
                                    {
                                        barras[nBarras].articulacao_my_fin = true;
                                        barras[nBarras].KMy_Final = barragenerica.Dados.k_my;
                                        barras[nBarras].KMy_Inicio = 1e14;
                                    }

                                    if (barragenerica_ArticulacaoMZ_Fin)
                                    {
                                        barras[nBarras].articulacao_mz_fin = true;
                                        barras[nBarras].KMz_Final = barragenerica.Dados.k_mz;
                                        barras[nBarras].KMz_Inicio = 1e14;
                                    }
                                }
                            }
                        }
                    }

                    p2 = p1;
                }
            }

            foreach (TBarraPortico b in barras)
            {
                if (b == null)
                    continue;
                b.apoio_simples_pIni = false;
                b.apoio_simples_pFin = false;
                foreach (TApoio a in apoios)
                {
                    if (a.Dados.restringe_dx && a.Dados.restringe_dy && a.Dados.restringe_dz &&
                        !a.Dados.restringe_rx && !a.Dados.restringe_ry && !a.Dados.restringe_rz)
                    {
                        if (Geom.Iguais(b.pIni.x, a.pIni.x) &&
                         Geom.Iguais(b.pIni.y, a.pIni.y) &&
                         Geom.Iguais(b.pIni.z, a.pIni.z))
                            b.apoio_simples_pIni = true;

                        if (Geom.Iguais(b.pFin.x, a.pIni.x) &&
                         Geom.Iguais(b.pFin.y, a.pIni.y) &&
                         Geom.Iguais(b.pFin.z, a.pIni.z))
                            b.apoio_simples_pFin = true;
                    }
                }
            }

        }

        [NonSerialized]
        double tx, ty, tz, L,cx, cy, cz;
        bool zi_maior_que_zf, xi_igual_xf;
        double y__, z__, x__;
        vec3 pos;
        vec3 pinicial = new vec3(0);
        vec3 pfinal = new vec3(0);
        vec3 pmeio = new vec3(0);
        int divbarras = 15;
        vec3 u1, u2, u, normxy, normxz;
        double NdotU, ndotu_mod, cos_alfa, angXY, angXZ, divisoes, divisoesFrac, xAnt;
        int divint;
        List<vec3> Divisoes = new List<vec3>();
        List<vec3> CoordsOffsets = new List<vec3>();

        double[] posicao = new double[4];
        double[] posicaoFinal = new double[4];
        double[] posicaoFinal2 = new double[4];
        double[] posicaoFinal_Trans = new double[5];

        void SubdivideBarra(ref List<vec3> CoordsSubdvisao, double div, double xi, double yi, double zi, double xf, double yf, double zf, bool articulacao_ini, bool articulacao_fim, bool tem_ex_i, bool tem_ex_f, double ex_i, double ex_f)
        {
            /* - faço translação da barra para o ponto zero usando tx ty  tz
               - encontro os angulos com os planos xy e xz
               - crio a barra no eixo global x a partir do ponto zero ( x=0 y=0 z=0) e faço a subdivisao nos tamanhos
                  que eu quero criando a lista de pontos nesse eixo global x
               - rotaciono esses pontos em torno do ponto zero de acordo com os angulos que achei nos planos xy e xz
               - faço a translação dos pontos com sinal contrário ( -tx -ty -tz)  para voltar para a posição original 
             */


            try
            {
                if (Geom.Iguais(xi, 0))
                    xi = 0;
                if (Geom.Iguais(yi, 0))
                    yi = 0;
                if (Geom.Iguais(zi, 0))
                    zi = 0;

                if (Geom.Iguais(xf, 0))
                    xf = 0;
                if (Geom.Iguais(yf, 0))
                    yf = 0;
                if (Geom.Iguais(zf, 0))
                    zf = 0;

                L = (Math.Sqrt(Math.Pow(xi - xf, 2) + Math.Pow(yi - yf, 2) + Math.Pow(zi - zf, 2)));
                
                //  if (!checkBox1.Checked)
                // {               
                zi_maior_que_zf = false;

               // if (!Geom.Iguais(Math.Abs(zf), Math.Abs(zi)))
                    zi_maior_que_zf = ((zi * -1) > (zf * -1));

                xi_igual_xf = Geom.Iguais(xi, xf);
                /*
                    if (zi_maior_que_zf)
                    {
                        x__ = xi;
                        y__ = yi;
                        z__ = zi;

                        xi = xf;
                        yi = yf;
                        zi = zf;

                        xf = x__;
                        yf = y__;
                        zf = z__;
                    }*/

                /*faço uma translação da barra para o ponto zero, como se eu fizesse o comando mover do programa
                 * para o ponto zero pegando o pIni como pivo */

                pinicial.x = xi; pinicial.y = yi; pinicial.z = zi;
                pfinal.x = xi; pfinal.y = yi; pfinal.z = zi;
                pmeio = (pinicial + pfinal) / 2;

                tx = xi;
                ty = yi;
                tz = zi;

                xi -= tx;
                yi -= ty;
                zi -= tz;

                xf -= tx;
                yf -= ty;
                zf -= tz;
                /**/
                if (Geom.Iguais(xi, 0))
                    xi = 0;
                if (Geom.Iguais(yi, 0))
                    yi = 0;
                if (Geom.Iguais(zi, 0))
                    zi = 0;

                if (Geom.Iguais(xf, 0))
                    xf = 0;
                if (Geom.Iguais(yf, 0))
                    yf = 0;
                if (Geom.Iguais(zf, 0))
                    zf = 0;

                CoordsSubdvisao.Clear();
                //    L = (Math.Sqrt(Math.Pow(xi - xf, 2) + Math.Pow(yi - yf, 2)));

                //  if (xi < xf)
                cx = ((xi) - (xf)) / L;
                //else
                //    cx = ((xf) - (xi)) / L;

                //  if (yi < yf)
                cy = ((yi) - (yf)) / L;
                //  else
                //      cy = ((yf) - (yi)) / L;

                cz = ((zi) - (zf)) / L;

                u1 = new vec3(xi, yi, zi * -1);
                u2 = new vec3(xf, yf, zf * -1);

                //Encontrar angulo que a barra faz com os planos XY e XZ

                //PLANO XY
                normxy = new vec3(0, 0, 1);

                //  if (xi < xf)
                u = u1 - u2;
                //else
                //    u = u2 - u1;

                NdotU = (normxy.DotProduct(u));
                ndotu_mod = normxy.Magnitude() * u.Magnitude();
                cos_alfa = Math.Abs(NdotU / ndotu_mod);
                angXY = RMath.rad2deg(Math.Acos(cos_alfa));
                angXY =/* RMath.rad2deg(Math.Atan(u1.z - u2.z / (u1.y - u2.y)));*/(90 - angXY) * 1;

                //PLANO XZ
                normxz = new vec3(0, 1, 0);
                //  if (xi< xf)
                u = u1 - u2;
                //  else
                //     u = u1 - u2;

                NdotU = (normxz.DotProduct(u));
                ndotu_mod = normxz.Magnitude() * u.Magnitude();
                cos_alfa = Math.Abs(NdotU / ndotu_mod);
                angXZ = RMath.rad2deg(Math.Acos(cos_alfa));

                if (!Geom.Iguais(u1.x - u2.x, 0))
                    angXZ = RMath.rad2deg(Math.Atan(u1.y - u2.y / (u1.x - u2.x)));/* (90-angXZ) * 1;*/
                else
                {
                    if (!Geom.Iguais(yi, yf))
                    {
                        if (yi < yf)
                            angXZ = -90;
                        else
                            angXZ = 90;
                    }
                    else
                        angXZ = 90;
                }

                divisoes = (L / div);
                divisoesFrac = FuncoesGerais.Frac(L / div);
                divint = (int)(divisoes - divisoesFrac);

                pos = new vec3(0, 0, 0);
                xAnt = xi;

                if (!xi_igual_xf)
                    if (xi < xf)
                       angXY *= -1;

                if (!Geom.Iguais(Math.Abs(angXZ), 90))
                    angXZ *= -1;

                if (xi_igual_xf)
                    angXY *= -1;

                if (zi_maior_que_zf)
                    angXY *= -1;

                //       textBox5.Text = angXY.ToString("n2");
                //         textBox6.Text = angXZ.ToString("n2");

                pos.x = xi;
                pos.y = yi;
                pos.z = zi;
                xAnt = pos.x;

                pos.x = 0;
                pos.y = 0;
                pos.z = 0;
    
                //  CoordsSubdvisao.Add(pos);
                /*   if (InserindoCota)
                   {
                       for (int i = 0; i < divint; i++)
                       {
    //                       pos = new vec3(xi > xf ? xAnt - div : xAnt + div, yi, zi);
                           if ((xi > xf) && PontoMaisProximo == "F")
                             pos = new vec3(xAnt + div, yi, zi);
                           else
                           if ((xi > xf) && PontoMaisProximo == "I")
                             pos = new vec3(xAnt - div, yi, zi);
     
                           CoordsSubdvisao.Add(pos);
                           xAnt = pos.x;
                       }
                   }
                   else*/
                {


                    pos = new vec3(0, 0, 0);
                    CoordsSubdvisao.Add(pos);
                    xAnt = 0;
                    int qtd_div_extremo = 3;

                    /*if (articulacao_ini || articulacao_fim)
                    {
                        double tam_barra_inicio = 0.005; //5 mm
                        double div_original = div;
                        if (div_original % 2 != 0)
                            div -= 1;
                        double tam_malha = 0;

                        double divisoes_nova = 0;
                        
                        if ((articulacao_ini && !articulacao_fim) || (!articulacao_ini && articulacao_fim))
                          divisoes_nova = (L - (tam_barra_inicio)) / div;
                        else
                          divisoes_nova = (L - (tam_barra_inicio * 2)) / div;

                        //int d = (int)div / 2;
                        if (articulacao_ini)
                        {
                            tam_malha = tam_barra_inicio;
                            if (!xi_igual_xf)
                            {
                                if (xi > xf)
                                    pos = new vec3(xAnt - tam_malha, yi, zi);
                                else
                                    pos = new vec3(xAnt + tam_malha, yi, zi);
                            }
                            else
                                pos = new vec3(xAnt + tam_malha, yi, zi);

                            CoordsSubdvisao.Add(pos);
                            xAnt = pos.x;
                        }

                        tam_malha = divisoes_nova;
                        for (int i = 0; i < div; i++)
                        {
                            if (!xi_igual_xf)
                            {
                                if (xi > xf)
                                    pos = new vec3(xAnt - tam_malha, yi, zi);
                                else
                                    pos = new vec3(xAnt + tam_malha, yi, zi);
                            }
                            else
                                pos = new vec3(xAnt + tam_malha, yi, zi);

                            CoordsSubdvisao.Add(pos);
                            xAnt = pos.x;
                        }
                        
                        if (articulacao_fim)
                        {
                            tam_malha = tam_barra_inicio;
                            if (!xi_igual_xf)
                            {
                                if (xi > xf)
                                    pos = new vec3(xAnt - tam_malha, yi, zi);
                                else
                                    pos = new vec3(xAnt + tam_malha, yi, zi);
                            }
                            else
                                pos = new vec3(xAnt + tam_malha, yi, zi);

                            CoordsSubdvisao.Add(pos);
                            xAnt = pos.x;
                        }

                    }
                    else*/

                    if (tem_ex_i || tem_ex_f)
                    {
//                        if (tem_ex_i && !tem_ex_f)
                        {
                            ex_f = Math.Abs(ex_f);
                            ex_i =  Math.Abs(ex_i);

                            qtd_div_extremo = 1;

                            double tam_malha = 0;

                            double divisoes_nova = 0;

                            if (tem_ex_i && !tem_ex_f)
                                divisoes_nova = (L - ex_i) / div;
                            else
                            if (!tem_ex_i && tem_ex_f)
                            {
                                //if (ex_f<0)
                                //  ex_f = Math.Abs(ex_f);
                              //  ex_f *= 1;

                                divisoes_nova = (L - ex_f) / div;
                            }
                            else
                            if (tem_ex_i && tem_ex_f)
                                divisoes_nova = (L - ex_i - ex_f) / div;

                            //-------------------//
                            if (tem_ex_i)
                            {
                                if (!xi_igual_xf)
                                {
                                    if (xi > xf)
                                        pos = new vec3(xAnt - ex_i, yi, zi);
                                    else
                                        pos = new vec3(xAnt + ex_i, yi, zi);
                                }
                                else
                                    pos = new vec3(xAnt + ex_i, yi, zi);

                                CoordsSubdvisao.Add(pos);
                                xAnt = pos.x;
                            }
                            //-------------------//


                            tam_malha = divisoes_nova;
                            for (int i = 0; i < div; i++)
                            {
                                if (!xi_igual_xf)
                                {
                                    if (xi > xf)
                                        pos = new vec3(xAnt - tam_malha, yi, zi);
                                    else
                                        pos = new vec3(xAnt + tam_malha, yi, zi);
                                }
                                else
                                    pos = new vec3(xAnt + tam_malha, yi, zi);

                                CoordsSubdvisao.Add(pos);
                                xAnt = pos.x;
                            }

                            //-------------------//
                            if (tem_ex_f)
                            {
                                if (!xi_igual_xf)
                                {
                                    if (xi > xf)
                                        pos = new vec3(xAnt - ex_f, yi, zi);
                                    else
                                        pos = new vec3(xAnt + ex_f, yi, zi);
                                }
                                else
                                    pos = new vec3(xAnt + ex_f, yi, zi);

                                CoordsSubdvisao.Add(pos);
                                xAnt = pos.x;
                            }
                            //-------------------//


                            /* tam_malha = divRefinamento_extremo;
                             for (int i = 0; i < qtd_div_extremo; i++)
                             {
                                 if (!xi_igual_xf)
                                 {
                                     if (xi > xf)
                                         pos = new vec3(xAnt - tam_malha, yi, zi);
                                     else
                                         pos = new vec3(xAnt + tam_malha, yi, zi);
                                 }
                                 else
                                     pos = new vec3(xAnt + tam_malha, yi, zi);

                                 CoordsSubdvisao.Add(pos);
                                 xAnt = pos.x;
                             }*/
                        }
                    }
                    else
                    {
                        for (int i = 0; i < div; i++)
                        {
                            if (!xi_igual_xf)
                            {
                                if (xi > xf)
                                    pos = new vec3(xAnt - divisoes, yi, zi);
                                else
                                    pos = new vec3(xAnt + divisoes, yi, zi);
                            }
                            else
                            {

                                pos = new vec3(xAnt + divisoes, yi, zi);
                            }

                            CoordsSubdvisao.Add(pos);
                            xAnt = pos.x;
                        }
                    }
                     
                    gerenciador.ConfiguracoesPGi.CfgProjeto.portico.refinar = false;

                    if (gerenciador.ConfiguracoesPGi.CfgProjeto.portico.refinar && L > 1) // obsoleto
                    {
                        double tam_refinamento_extremo = 0.06 * L;
                        if (0.06 * L < 0.2)
                            tam_refinamento_extremo = 0.2 * L;

                        double tam_refinamento_meio = 0.06 * L;

                        if (0.06 * L < 0.2)
                            tam_refinamento_meio = 0.2 * L;

                        double divRefinamento_extremo = tam_refinamento_extremo / qtd_div_extremo;


                        double divRefinamento_meio = tam_refinamento_meio / 3;
                       
                        double div_original = div;
                        if (div_original % 2 != 0)
                            div -= 1;
                        double tam_malha = 0;
                        double divisoes_nova = (L - (((tam_refinamento_extremo * 2) + tam_refinamento_meio))) / div;

                        if (divisoes_nova < 0.2)
                        {
                           
                            if (div_original % 2 != 0)
                                div = div_original - 5;
                            else
                                div = div_original - 2;

                            divisoes_nova = (L - (((tam_refinamento_extremo * 2) + tam_refinamento_meio))) / div;
                        }

                        int d = (int)div / 2;

                        tam_malha = divRefinamento_extremo;
                        for (int i = 0; i < qtd_div_extremo; i++)
                        {
                            if (!xi_igual_xf)
                            {
                                if (xi > xf)
                                    pos = new vec3(xAnt - tam_malha, yi, zi);
                                else
                                    pos = new vec3(xAnt + tam_malha, yi, zi);
                            }
                            else
                                pos = new vec3(xAnt + tam_malha, yi, zi);

                            CoordsSubdvisao.Add(pos);
                            xAnt = pos.x;
                        }

                        tam_malha = divisoes_nova;
                        for (int i = 0; i < d; i++)
                        {
                            if (!xi_igual_xf)
                            {
                                if (xi > xf)
                                    pos = new vec3(xAnt - tam_malha, yi, zi);
                                else
                                    pos = new vec3(xAnt + tam_malha, yi, zi);
                            }
                            else
                                pos = new vec3(xAnt + tam_malha, yi, zi);

                            CoordsSubdvisao.Add(pos);
                            xAnt = pos.x;
                        }

                        tam_malha = divRefinamento_meio;
                        for (int i = 0; i < 3; i++)
                        {
                            if (!xi_igual_xf)
                            {
                                if (xi > xf)
                                    pos = new vec3(xAnt - tam_malha, yi, zi);
                                else
                                    pos = new vec3(xAnt + tam_malha, yi, zi);
                            }
                            else
                                pos = new vec3(xAnt + tam_malha, yi, zi);

                            CoordsSubdvisao.Add(pos);
                            xAnt = pos.x;
                        }

                        tam_malha = divisoes_nova;
                        for (int i = 0; i < d; i++)
                        {
                            if (!xi_igual_xf)
                            {
                                if (xi > xf)
                                    pos = new vec3(xAnt - tam_malha, yi, zi);
                                else
                                    pos = new vec3(xAnt + tam_malha, yi, zi);
                            }
                            else
                                pos = new vec3(xAnt + tam_malha, yi, zi);

                            CoordsSubdvisao.Add(pos);
                            xAnt = pos.x;
                        }

                        tam_malha = divRefinamento_extremo;
                        for (int i = 0; i < qtd_div_extremo; i++)
                        {
                            if (!xi_igual_xf)
                            {
                                if (xi > xf)
                                    pos = new vec3(xAnt - tam_malha, yi, zi);
                                else
                                    pos = new vec3(xAnt + tam_malha, yi, zi);
                            }
                            else
                                pos = new vec3(xAnt + tam_malha, yi, zi);

                            CoordsSubdvisao.Add(pos);
                            xAnt = pos.x;
                        }
                    }
                    else
                    {
                        /* for (int i = 0; i < div; i++)
                         {
                             if (!xi_igual_xf)
                             {
                                 if (xi > xf)
                                     pos = new vec3(xAnt - divisoes, yi, zi);
                                 else
                                     pos = new vec3(xAnt + divisoes, yi, zi);
                             }
                             else
                             {
                                 pos = new vec3(xAnt + divisoes, yi, zi);
                             }
                             CoordsSubdvisao.Add(pos);
                             xAnt = pos.x;
                         }*/
                    }
                }
              //  pos = new vec3(xf,yf,zf);
                
               // if (divisoesFrac != 0)
              //      pos = new vec3(xi > xf ? xAnt - L : xAnt + (div * divisoesFrac), yi, zi);
             //   else
                 //   pos = new vec3(xi > xf ? - L :L, yi, zi);

              //  CoordsSubdvisao.Add(pos);
                //  if (checkBox2.Checked)
                {
                    for (int i = 0; i < CoordsSubdvisao.Count; i++)
                    {
                        posicao[1] = CoordsSubdvisao[i].x;
                        posicao[2] = CoordsSubdvisao[i].y;
                        posicao[3] = CoordsSubdvisao[i].z;

                        Geom.rotY(angXY, ref posicao, ref posicaoFinal);
                        Geom.rotZ(angXZ, ref posicaoFinal, ref posicaoFinal2);

                        CoordsSubdvisao[i].x = posicaoFinal2[1] + tx;
                        CoordsSubdvisao[i].y = posicaoFinal2[2] + ty;
                        CoordsSubdvisao[i].z = posicaoFinal2[3] + tz;

                        if (Geom.Iguais(CoordsSubdvisao[i].x, 0))
                            CoordsSubdvisao[i].x = 0;
                        if (Geom.Iguais(CoordsSubdvisao[i].y, 0))
                            CoordsSubdvisao[i].y = 0;
                        if (Geom.Iguais(CoordsSubdvisao[i].z, 0))
                            CoordsSubdvisao[i].z = 0;

                        /*    CoordsSubdvisao[i].x += tx;
                            CoordsSubdvisao[i].y += ty;
                            CoordsSubdvisao[i].z += tz;*/
                    }
                }

                //Translado pra posição original
               /* for (int i = 0; i < CoordsSubdvisao.Count; i++)
                {
                    CoordsSubdvisao[i].x += tx;
                    CoordsSubdvisao[i].y += ty;
                    CoordsSubdvisao[i].z += tz;
                }*/
            }

            catch (Exception e)
            {
                System.Windows.Forms.MessageBox.Show(e.Message);
            }
        }

        public bool GerarMalha()
        {
            Atualiza(1);
            System.Windows.Forms.Application.DoEvents();
            HistoricoCalculo("      > Gerando malha do pórtico...");
 
            InicializaNosBarras();

            if (Testes)
            {
                MalhaTeste();
                Finaliza();

                return true;
            }

           /* CriaBarrasViga();

            CriaBarrasPilar();

            CriarBarrasRigidas();*/
            CriarBarras(true);

            Finaliza();

            System.Windows.Forms.Application.DoEvents();
            HistoricoCalculo("      - Geração da malha do pórtico [" + nBarras.ToString() + " barras, " + nNos + " nós] - [Ok]", true);
            return true;
        }
    }
}
