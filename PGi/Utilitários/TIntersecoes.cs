using MathNet.Numerics;
using OpenTK.Graphics;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Net.NetworkInformation;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Documents;
using static PG.Geom;

namespace PG
{
    public static class TIntersecoes
    {
        struct sPilarXPontos
        {
            public TPilar pilar;
            public TTrechoViga trecho;
            public List<TPonto> PontosIntersecao;
            // public TTrechoViga trecho;
            public sPilarXPontos(TPilar pil, TTrechoViga trech)
            {
                pilar = pil;
                trecho = trech;
                PontosIntersecao = new List<TPonto>();
            }
        }

        struct sPontosXTrechos
        {
            public TTrechoViga Trecho;
            public List<TTrechoViga> TrechosTestados;
            public List<TPilar> PilaresTestados;
            public List<TPonto> Pontos;
            public sPontosXTrechos(TTrechoViga trecho)
            {
                PilaresTestados = new List<TPilar>();
                TrechosTestados = new List<TTrechoViga>();
                Pontos = new List<TPonto>();
                this.Trecho = trecho;
            }
        }

        static List<sPontosXTrechos> PontosXTrechos_FaceCima_pIni;
        static List<sPontosXTrechos> PontosXTrechos_FaceCima_pFin;

        static List<sPontosXTrechos> PontosXTrechos_FaceBaixo_pIni;
        static List<sPontosXTrechos> PontosXTrechos_FaceBaixo_pFin;

        public static void RemoveTrechosSobrepostos(ref List<TTrechoViga> TrechosVigas, FPrincipal desenho)
        {
            foreach (TTrechoViga t1 in TrechosVigas)
            {
                foreach (TTrechoViga t2 in TrechosVigas)
                {
                    if (t1.ID != t2.ID)
                    if ((t1.linhas_eixo.pIni == t2.linhas_eixo.pIni && t1.linhas_eixo.pFin == t2.linhas_eixo.pFin)
                     || (t1.linhas_eixo.pIni == t2.linhas_eixo.pFin && t1.linhas_eixo.pFin == t2.linhas_eixo.pIni))
                        t1.SelecionaTodosElementos(t1);
                }
            }

          //  desenho.RemoveSelecionados();

        }

        public static void CalculaIntersecoesTrechos(ref List<TTrechoViga> TrechosVigas, ref List<TPilar> Pilares)
        {
            List<TPonto> emCimaFB = new List<TPonto>();
            List<TPonto> emCimaFC = new List<TPonto>();

            PontosXTrechos_FaceCima_pIni = new List<sPontosXTrechos>();
            PontosXTrechos_FaceCima_pFin = new List<sPontosXTrechos>();

            PontosXTrechos_FaceBaixo_pIni = new List<sPontosXTrechos>();
            PontosXTrechos_FaceBaixo_pFin = new List<sPontosXTrechos>();
   
            double ix_Fc = 0, iy_Fc = 0;
            double ix_Fb = 0, iy_Fb = 0;

            TLinha trecho1_facebaixo_m1, trecho1_facebaixo_m2;
            TLinha trecho1_facecima_m1, trecho1_facecima_m2;

            TLinha trecho2_facebaixo_m1, trecho2_facebaixo_m2;
            TLinha trecho2_facecima_m1, trecho2_facecima_m2;

            TPonto pFaceCima, pFaceBaixo;
            int Pilar_pFin_t1 = 0, Pilar_pIni_t1 = 0, Pilar_pFin_t2 = 0, Pilar_pIni_t2 = 0;

            for (int i = 0; i < 2; i++)
            {
                foreach (TTrechoViga t1 in TrechosVigas)
                {
                    TPonto pFin_t1 = t1.linhas_eixo.pFin;
                    TPonto pIni_t1 = t1.linhas_eixo.pIni;

                    Pilar_pIni_t1  = t1.NumPilar_PontoInicial;
                    Pilar_pFin_t1  = t1.NumPilar_PontoFinal;

                    trecho1_facecima_m1 = t1.linhas_facecima_longa_m1;
                    trecho1_facecima_m2 = t1.linhas_facecima_longa_m2;
                    trecho1_facebaixo_m1 = t1.linhas_facebaixo_longa_m1;
                    trecho1_facebaixo_m2 = t1.linhas_facebaixo_longa_m2;

                    emCimaFB.Clear();
                    emCimaFC.Clear();

                    /*Cria lista de pontos de interesecao que estao sobre a face de cima e sobre a face de baixo do trecho em questao
                   Seguindo a seguinte regra: 
                   se o trecho 1.eixo.pFin = trecho 2.eixo.pIni, entao insere o ponto de intersecao do trecho1_facecima_m2 com trecho2_facecima_m1 na lista emCimaFC 
                                                                       insere o ponto de intersecao do trecho1_facebaixo_m2 com trecho2_facebaixo_m1 na lista emCimaFB
                 
                    */
                    /*Depois ordenar o os pontos de intersecao juntamente com o ponto medio da face de cima ou baixo, dependendo de qual face estiver interessado
                     feito isso, o ponto de intersecao correto é o primeiro depois do ponto medio da face
                     */

                    emCimaFB.Add(t1.linhas_facebaixo.getMiddlePoint());
                    emCimaFB[0].PontoMedio = true;

                    emCimaFC.Add(t1.linhas_facecima.getMiddlePoint());
                    emCimaFC[0].PontoMedio = true;

                    if (i == 0)
                    {
                        PontosXTrechos_FaceCima_pFin.Add(new sPontosXTrechos(t1));
                        PontosXTrechos_FaceBaixo_pFin.Add(new sPontosXTrechos(t1));
                    }
                    else
                    {
                        PontosXTrechos_FaceBaixo_pIni.Add(new sPontosXTrechos(t1));
                        PontosXTrechos_FaceCima_pIni.Add(new sPontosXTrechos(t1));
                    }

                    foreach (TTrechoViga t2 in TrechosVigas)
                    {
                        if ((Object)t1 == (Object)t2)
                            continue;
                      //  if (t1.angulo == t2.angulo) continue;

                        if (t1.Dados.numero == 33 || t2.Dados.numero == 33)
                            Pilar_pIni_t2 = t2.NumPilar_PontoInicial;
                        
                        Pilar_pIni_t2 = t2.NumPilar_PontoInicial;
                        Pilar_pFin_t2 = t2.NumPilar_PontoFinal;
                        
                        TPonto pFin_t2 = t2.linhas_eixo.pFin;
                        TPonto pIni_t2 = t2.linhas_eixo.pIni;

                        trecho2_facebaixo_m1 = t2.linhas_facebaixo_longa_m1;
                        trecho2_facebaixo_m2 = t2.linhas_facebaixo_longa_m2;
                        trecho2_facecima_m1 = t2.linhas_facecima_longa_m1;
                        trecho2_facecima_m2 = t2.linhas_facecima_longa_m2;

                        //se for a primeira iteracao, testa o ponto final do trecho 1. Se for a segunda iteracao, testa o ponto inicial do trecho 1
                        bool InterceptouArestaOriginal = false; 
                        
                        if (i == 0)
                        {
                            if ((pFin_t1 == pIni_t2) || (Pilar_pFin_t1 == Pilar_pIni_t2 && Pilar_pFin_t1 > 0))
                            {
                                //se for um pilar o ponto em comum
                               if (Pilar_pFin_t1 > 0)
                               {
                                    TPilar PilarAtual = null;
                                    foreach (TPilar pilar in Pilares)
                                       if (pilar.Dados.numero == Pilar_pFin_t1)
                                           PilarAtual = pilar;

                                    if (PilarAtual != null)
                                    {
                                        InterceptouArestaOriginal = false;
                                     /*   foreach (TLinha ArestaAtual in PilarAtual.Dados.Poligono.linhas_poligonal)
                                        {
                                            if (trecho1_facecima_m2.Intersec(ArestaAtual, ref ix_Fc, ref iy_Fc))
                                            {
                                                emCimaFC.Add(new TPonto(ix_Fc, iy_Fc, 0));
                                                InterceptouArestaOriginal = true;
                                            }

                                            if (trecho1_facebaixo_m2.Intersec(ArestaAtual, ref ix_Fb, ref iy_Fb))
                                            {
                                                emCimaFB.Add(new TPonto(ix_Fb, iy_Fb, 0));
                                                InterceptouArestaOriginal = true;
                                            }
                                        }*/

                                        if (!InterceptouArestaOriginal)
                                        foreach (TLinha ArestaAtual in PilarAtual.linhas_poligonal_aux)
                                        {
                                            if (trecho1_facecima_m2.Intersec(ArestaAtual, ref ix_Fc, ref iy_Fc))
                                                emCimaFC.Add(new TPonto(ix_Fc, iy_Fc, trecho1_facecima_m2.pIni.z));

                                            if (trecho1_facebaixo_m2.Intersec(ArestaAtual, ref ix_Fb, ref iy_Fb))
                                                emCimaFB.Add(new TPonto(ix_Fb, iy_Fb, trecho1_facebaixo_m2.pIni.z));
                                        }

                                        PontosXTrechos_FaceCima_pFin[PontosXTrechos_FaceCima_pFin.Count - 1].PilaresTestados.Add(PilarAtual);
                                        PontosXTrechos_FaceBaixo_pFin[PontosXTrechos_FaceBaixo_pFin.Count - 1].PilaresTestados.Add(PilarAtual);
                                    }
                               }


                               //mesmo assim tem que encontrar a intersecao das faces, pois elas sempre terão intersecao tbm
                               if (Math.Abs(t1.angulo) != Math.Abs(t2.angulo))
                               {
                                   trecho1_facecima_m2.Intersec2(trecho2_facecima_m1, ref ix_Fc, ref iy_Fc);
                                   trecho1_facebaixo_m2.Intersec2(trecho2_facebaixo_m1, ref ix_Fb, ref iy_Fb);
                                   emCimaFC.Add(new TPonto(ix_Fc, iy_Fc, trecho1_facecima_m2.pIni.z));
                                   emCimaFB.Add(new TPonto(ix_Fb, iy_Fb, trecho1_facebaixo_m2.pIni.z));

                                   PontosXTrechos_FaceCima_pFin[PontosXTrechos_FaceCima_pFin.Count - 1].TrechosTestados.Add(t2);
                                   PontosXTrechos_FaceBaixo_pFin[PontosXTrechos_FaceBaixo_pFin.Count - 1].TrechosTestados.Add(t2);
                               }
                            }
                            else
                            if ((pFin_t1 == pFin_t2) || (Pilar_pFin_t1 == Pilar_pFin_t2 && Pilar_pFin_t1 > 0))
                            {
                                //se for um pilar o ponto em comum
                                if (Pilar_pFin_t1 > 0)
                                {
                                    TPilar PilarAtual = null;
                                    foreach (TPilar pilar in Pilares)
                                        if (pilar.Dados.numero == Pilar_pFin_t1)
                                            PilarAtual = pilar;

                                    if (PilarAtual != null)
                                    {
                                        InterceptouArestaOriginal = false; 
                                     /*   foreach (TLinha ArestaAtual in PilarAtual.linhas_poligonal_aux)
                                        {
                                            if (trecho1_facecima_m2.Intersec(ArestaAtual, ref ix_Fc, ref iy_Fc))
                                            {
                                                emCimaFC.Add(new TPonto(ix_Fc, iy_Fc, 0));
                                                InterceptouArestaOriginal = true;
                                            }
                                            if (trecho1_facebaixo_m2.Intersec(ArestaAtual, ref ix_Fb, ref iy_Fb))
                                            {
                                                emCimaFB.Add(new TPonto(ix_Fb, iy_Fb, 0));
                                                InterceptouArestaOriginal = true;
                                            }                                        
                                        } */
                                        
                                        if (!InterceptouArestaOriginal) 
                                        foreach (TLinha ArestaAtual in PilarAtual.linhas_poligonal_aux)
                                        {
                                            if (trecho1_facecima_m2.Intersec(ArestaAtual, ref ix_Fc, ref iy_Fc))
                                                emCimaFC.Add(new TPonto(ix_Fc, iy_Fc, trecho1_facecima_m2.pIni.z));

                                            if (trecho1_facebaixo_m2.Intersec(ArestaAtual, ref ix_Fb, ref iy_Fb))
                                                emCimaFB.Add(new TPonto(ix_Fb, iy_Fb, trecho1_facebaixo_m2.pIni.z));
                                        }

                                        PontosXTrechos_FaceCima_pFin[PontosXTrechos_FaceCima_pFin.Count - 1].PilaresTestados.Add(PilarAtual);
                                        PontosXTrechos_FaceBaixo_pFin[PontosXTrechos_FaceBaixo_pFin.Count - 1].PilaresTestados.Add(PilarAtual);
                                    }
                                }


                                //mesmo assim tem que encontrar a intersecao das faces, pois elas sempre terão intersecao tbm   
                                if (Math.Abs(t1.angulo) != Math.Abs(t2.angulo))
                                {
                                    trecho1_facecima_m2.Intersec2(trecho2_facebaixo_m2, ref ix_Fc, ref iy_Fc);
                                    trecho1_facebaixo_m2.Intersec2(trecho2_facecima_m2, ref ix_Fb, ref iy_Fb);
                                    emCimaFC.Add(new TPonto(ix_Fc, iy_Fc, trecho1_facecima_m2.pIni.z));
                                    emCimaFB.Add(new TPonto(ix_Fb, iy_Fb, trecho1_facebaixo_m2.pIni.z));

                                    PontosXTrechos_FaceCima_pFin[PontosXTrechos_FaceCima_pFin.Count - 1].TrechosTestados.Add(t2);
                                    PontosXTrechos_FaceBaixo_pFin[PontosXTrechos_FaceBaixo_pFin.Count - 1].TrechosTestados.Add(t2);
                                }
                            }                          
                        }
                        else
                        {
                            if ((pIni_t1 == pFin_t2) || (Pilar_pIni_t1 == Pilar_pFin_t2 && Pilar_pFin_t2 > 0))
                            {
                                //se for um pilar o ponto em comum
                                if (Pilar_pIni_t1 > 0)
                                {
                                    TPilar PilarAtual = null;
                                    foreach (TPilar pilar in Pilares)
                                        if (pilar.Dados.numero == Pilar_pIni_t1)
                                            PilarAtual = pilar;


                                    if (PilarAtual != null)
                                    {
                                        InterceptouArestaOriginal = false; 
                                        foreach (TLinha ArestaAtual in PilarAtual.linhas_poligonal_aux)
                                        {
                                            if (trecho1_facecima_m1.Intersec(ArestaAtual, ref ix_Fc, ref iy_Fc))
                                            {
                                                emCimaFC.Add(new TPonto(ix_Fc, iy_Fc, trecho1_facecima_m1.pIni.z));
                                                InterceptouArestaOriginal = true;
                                            }
                                            if (trecho1_facebaixo_m1.Intersec(ArestaAtual, ref ix_Fb, ref iy_Fb))
                                            {
                                                emCimaFB.Add(new TPonto(ix_Fb, iy_Fb, trecho1_facebaixo_m1.pIni.z));
                                                 InterceptouArestaOriginal = true;
                                            }                                       
                                        } 
                                        
                                    /*    if (!InterceptouArestaOriginal) 
                                        foreach (TLinha ArestaAtual in PilarAtual.linhas_poligonal_aux)
                                        {
                                            if (trecho1_facecima_m1.Intersec(ArestaAtual, ref ix_Fc, ref iy_Fc))
                                                emCimaFC.Add(new TPonto(ix_Fc, iy_Fc, 0));

                                            if (trecho1_facebaixo_m1.Intersec(ArestaAtual, ref ix_Fb, ref iy_Fb))
                                                emCimaFB.Add(new TPonto(ix_Fb, iy_Fb, 0));
                                        }*/

                                        PontosXTrechos_FaceCima_pIni[PontosXTrechos_FaceCima_pIni.Count - 1].PilaresTestados.Add(PilarAtual);
                                        PontosXTrechos_FaceBaixo_pIni[PontosXTrechos_FaceBaixo_pIni.Count - 1].PilaresTestados.Add(PilarAtual);
                                    }
                                } 
                                
                                //mesmo assim tem que encontrar a intersecao das faces, pois elas sempre terão intersecao tbm
                                if (Math.Abs(t1.angulo) != Math.Abs(t2.angulo))
                                {
                                    trecho1_facecima_m1.Intersec2(trecho2_facecima_m2, ref ix_Fc, ref iy_Fc);
                                    trecho1_facebaixo_m1.Intersec2(trecho2_facebaixo_m2, ref ix_Fb, ref iy_Fb);
                                    emCimaFC.Add(new TPonto(ix_Fc, iy_Fc, trecho1_facecima_m1.pIni.z));
                                    emCimaFB.Add(new TPonto(ix_Fb, iy_Fb, trecho1_facebaixo_m1.pIni.z));

                                    PontosXTrechos_FaceCima_pIni[PontosXTrechos_FaceCima_pIni.Count - 1].TrechosTestados.Add(t2);
                                    PontosXTrechos_FaceBaixo_pIni[PontosXTrechos_FaceBaixo_pIni.Count - 1].TrechosTestados.Add(t2);
                                }
                            }
                            else
                            if ((pIni_t1 == pIni_t2) || (Pilar_pIni_t1 == Pilar_pIni_t2 && Pilar_pIni_t1 > 0))
                            {
                                //se for um pilar o ponto em comum
                                if (Pilar_pIni_t1 > 0)
                                {
                                    TPilar PilarAtual = null;
                                    foreach (TPilar pilar in Pilares)
                                        if (pilar.Dados.numero == Pilar_pIni_t1)
                                            PilarAtual = pilar;

                                    if (PilarAtual != null)
                                    {
                                        InterceptouArestaOriginal = false; 
                                        foreach (TLinha ArestaAtual in PilarAtual.linhas_poligonal_aux)
                                        {
                                            if (trecho1_facecima_m1.Intersec(ArestaAtual, ref ix_Fc, ref iy_Fc))
                                            {
                                                emCimaFC.Add(new TPonto(ix_Fc, iy_Fc, trecho1_facecima_m1.pIni.z));
                                                InterceptouArestaOriginal = true;
                                            }
                                            if (trecho1_facebaixo_m1.Intersec(ArestaAtual, ref ix_Fb, ref iy_Fb))
                                            {
                                                emCimaFB.Add(new TPonto(ix_Fb, iy_Fb, trecho1_facebaixo_m1.pIni.z));
                                                 InterceptouArestaOriginal = true;
                                            }                                       
                                        } 
                                        
                                    /*    if (!InterceptouArestaOriginal) 
                                        foreach (TLinha ArestaAtual in PilarAtual.linhas_poligonal_aux)
                                        {
                                            if (trecho1_facecima_m1.Intersec(ArestaAtual, ref ix_Fc, ref iy_Fc))
                                                emCimaFC.Add(new TPonto(ix_Fc, iy_Fc, 0));

                                            if (trecho1_facebaixo_m1.Intersec(ArestaAtual, ref ix_Fb, ref iy_Fb))
                                                emCimaFB.Add(new TPonto(ix_Fb, iy_Fb, 0));
                                        }*/

                                        PontosXTrechos_FaceCima_pIni[PontosXTrechos_FaceCima_pIni.Count - 1].PilaresTestados.Add(PilarAtual);
                                        PontosXTrechos_FaceBaixo_pIni[PontosXTrechos_FaceBaixo_pIni.Count - 1].PilaresTestados.Add(PilarAtual);
                                    }
                                }

                                if (Math.Abs(t1.angulo) != Math.Abs(t2.angulo))
                                {
                                    trecho1_facecima_m1.Intersec2(trecho2_facebaixo_m1, ref ix_Fc, ref iy_Fc);
                                    trecho1_facebaixo_m1.Intersec2(trecho2_facecima_m1, ref ix_Fb, ref iy_Fb);
                                    emCimaFC.Add(new TPonto(ix_Fc, iy_Fc, trecho1_facecima_m1.pIni.z));
                                    emCimaFB.Add(new TPonto(ix_Fb, iy_Fb, trecho1_facebaixo_m1.pIni.z));

                                    PontosXTrechos_FaceCima_pIni[PontosXTrechos_FaceCima_pIni.Count - 1].TrechosTestados.Add(t2);
                                    PontosXTrechos_FaceBaixo_pIni[PontosXTrechos_FaceBaixo_pIni.Count - 1].TrechosTestados.Add(t2);
                                }
                            }
                        }
                    }

                    if (emCimaFB.Count > 1) //se tiver mais que um ponto, pois o primeiro ponto é sempre o médio e sempre tem
                    {
                        Geom.ReordenaPontos(t1.linhas_facebaixo_org.pIni.y, t1.linhas_facebaixo_org.pFin.y, t1.linhas_facebaixo_org.pIni.x, t1.linhas_facebaixo_org.pFin.y, ref emCimaFB);

                        int pmFB = 0;
                        for (pmFB = 0; pmFB < emCimaFB.Count; pmFB++)
                            if (emCimaFB[pmFB].PontoMedio)
                                break;

                        // o ponto médio vai ser o primeiro ou o ultimo
                        //se for o primeiro da lista, o ponto de intersecao é o segundo
                        //se for o ultimo da lista, o  ponto de intersecao é o penultimo
                        if (pmFB == 0)
                            pFaceBaixo = new TPonto(emCimaFB[pmFB + 1].x, emCimaFB[pmFB + 1].y, emCimaFB[pmFB + 1].z);
                        else
                            pFaceBaixo = new TPonto(emCimaFB[pmFB - 1].x, emCimaFB[pmFB - 1].y, emCimaFB[pmFB - 1].z);

                        if (i == 0)
                        {
                            t1.linhas_facebaixo.pFin.x = pFaceBaixo.x;
                            t1.linhas_facebaixo.pFin.y = pFaceBaixo.y;
                            t1.linhas_facebaixo.pFin.z = pFaceBaixo.z;
                            
                            foreach (TPonto p in emCimaFB)
                              PontosXTrechos_FaceBaixo_pFin[PontosXTrechos_FaceBaixo_pFin.Count - 1].Pontos.Add(p);

                        }
                        else
                        {
                            t1.linhas_facebaixo.pIni.x = pFaceBaixo.x;
                            t1.linhas_facebaixo.pIni.y = pFaceBaixo.y;
                            t1.linhas_facebaixo.pIni.z = pFaceBaixo.z;

                            foreach (TPonto p in emCimaFB)
                                PontosXTrechos_FaceBaixo_pIni[PontosXTrechos_FaceBaixo_pIni.Count - 1].Pontos.Add(p);
                        }
                    }

                    if (emCimaFC.Count > 1)
                    {
                        Geom.ReordenaPontos(t1.linhas_facecima_org.pIni.y, t1.linhas_facecima_org.pFin.y, t1.linhas_facecima_org.pIni.x, t1.linhas_facecima_org.pFin.y, ref emCimaFC);

                        int pmFC = 0;
                        for (pmFC = 0; pmFC < emCimaFC.Count; pmFC++)
                            if (emCimaFC[pmFC].PontoMedio)
                                break;

                        if (pmFC == 0)
                            pFaceCima = new TPonto(emCimaFC[pmFC + 1].x, emCimaFC[pmFC + 1].y, emCimaFC[pmFC + 1].z);
                        else
                            pFaceCima = new TPonto(emCimaFC[pmFC - 1].x, emCimaFC[pmFC - 1].y, emCimaFC[pmFC - 1].z);

                        if (i == 0)
                        {
                            t1.linhas_facecima.pFin.x = pFaceCima.x;
                            t1.linhas_facecima.pFin.y = pFaceCima.y;
                            t1.linhas_facecima.pFin.z = pFaceCima.z;

                            foreach (TPonto p in emCimaFC)
                                PontosXTrechos_FaceCima_pFin[PontosXTrechos_FaceCima_pFin.Count - 1].Pontos.Add(p);
                        }
                        else
                        {
                            t1.linhas_facecima.pIni.x = pFaceCima.x;
                            t1.linhas_facecima.pIni.y = pFaceCima.y;
                            t1.linhas_facecima.pIni.z = pFaceCima.z;

                            foreach (TPonto p in emCimaFC)
                                PontosXTrechos_FaceCima_pIni[PontosXTrechos_FaceCima_pIni.Count - 1].Pontos.Add(p);
                        }
                    }
                }
            }

            PontosXTrechos_FaceBaixo_pFin.RemoveAll(obj => obj.Pontos.Count <= 1);
            PontosXTrechos_FaceBaixo_pIni.RemoveAll(obj => obj.Pontos.Count <= 1);
            PontosXTrechos_FaceCima_pFin.RemoveAll(obj => obj.Pontos.Count <= 1);
            PontosXTrechos_FaceCima_pIni.RemoveAll(obj => obj.Pontos.Count <= 1);


            //verificação para corrigir "vazios". Ver exemplo disso no aqruivo "pilaresXtrecho 4" na pasta C:\PGi - GDI\Raciocínios

            //testa pontos sem nenhuma outra face chegando nele, se sim, entao pega o proximo ponto depois
            //daquele considerado como intersecao (o segundo depois do ponto medio)
            //se esse ponto considir com um ponto de alguma face, entao faço a intersecao ser ali, senao nao
            bool IncideEmOutraFace;
            
            List<sPontosXTrechos> ListaPontosXTrechos = null;

            for (int i = 0; i < 4; i++)
            {
                if (i == 0) ListaPontosXTrechos = PontosXTrechos_FaceBaixo_pFin; else
                if (i == 1) ListaPontosXTrechos = PontosXTrechos_FaceBaixo_pIni;else
                if (i == 2) ListaPontosXTrechos = PontosXTrechos_FaceCima_pFin;else
                if (i == 3) ListaPontosXTrechos = PontosXTrechos_FaceCima_pIni;
              
                TPonto PontoFinal;
                TTrechoViga Trecho;

                foreach (sPontosXTrechos PontosXTrechos in ListaPontosXTrechos)
                {
                    Trecho     = PontosXTrechos.Trecho;
                    PontoFinal = null;
                  
                    if (i == 0) PontoFinal = Trecho.linhas_facebaixo.pFin;else
                    if (i == 1) PontoFinal = Trecho.linhas_facebaixo.pIni;else
                    if (i == 2) PontoFinal = Trecho.linhas_facecima.pFin;else
                    if (i == 3) PontoFinal = Trecho.linhas_facecima.pIni;    

                    IncideEmOutraFace = false;
                    foreach (TTrechoViga TrechoTestado in PontosXTrechos.TrechosTestados)
                    {
                        if ((TrechoTestado.linhas_facebaixo.pFin == PontoFinal) || (TrechoTestado.linhas_facebaixo.pIni == PontoFinal) ||
                            (TrechoTestado.linhas_facecima.pFin == PontoFinal) || (TrechoTestado.linhas_facecima.pIni == PontoFinal))
                        {
                            IncideEmOutraFace = true;
                            break;
                        }
                    }

                    foreach (TPilar pilar in PontosXTrechos.PilaresTestados)
                    {
                        foreach (TLinha ArestaAtual in pilar.linhas_poligonal_aux)
                        {
                           if (ArestaAtual.PontoEmLinha(PontoFinal.x, PontoFinal.y))
                           {
                               IncideEmOutraFace = true;
                               break;
                           }
                        }
                    }


                    //se o ponto final está avulso, sem nada chegando nele, entao considero como pt de intersecao aquele imdiatamente
                    // depois do ponto considerado anteriormente
                    //se esse novo ponto concidir com algum outro ponto das faces dos trechos, entao esse é o novo ponto, senao nao...
                    
                    // OLHAR O ARQUIVO PARA ENTENDER MELHOR : "pilaresXtrecho 4" na pasta C:\PGi - GDI\Raciocínios
                    
                    if (!IncideEmOutraFace)
                    {
                        int pm = 0;
                        for (pm = 0; pm < PontosXTrechos.Pontos.Count; pm++)
                            if (PontosXTrechos.Pontos[pm].PontoMedio)
                                break;

                        // o ponto médio vai ser o primeiro ou o ultimo
                        //se for o primeiro da lista, o ponto de intersecao é o terceiro (o segundo nao é mais, pois esta avulso, sem nada chegando nele, entao pego o terceiro)
                        //se for o ultimo da lista, o  ponto de intersecao é o antepenultimo
                        int PontoIntersecao;
                        if (pm == 0)
                            PontoIntersecao = pm + 2;
                        else
                            PontoIntersecao = pm - 2;

                        if (PontosXTrechos.Pontos.Count == 2 && pm == 0)
                            PontoIntersecao = 1;
                        else
                        if (PontosXTrechos.Pontos.Count == 2 && pm == 1)
                           PontoIntersecao = 0;

                        if (PontoIntersecao > -1)
                        {

                            IncideEmOutraFace = false;
                            foreach (TTrechoViga TrechoTestado in PontosXTrechos.TrechosTestados)
                            {
                                if ((TrechoTestado.linhas_facebaixo.pFin == PontosXTrechos.Pontos[PontoIntersecao]) || (TrechoTestado.linhas_facebaixo.pIni == PontosXTrechos.Pontos[PontoIntersecao]) ||
                                    (TrechoTestado.linhas_facecima.pFin == PontosXTrechos.Pontos[PontoIntersecao]) || (TrechoTestado.linhas_facecima.pIni == PontosXTrechos.Pontos[PontoIntersecao]))
                                {
                                    IncideEmOutraFace = true;
                                    break;
                                }
                            }

                            foreach (TPilar PilarTestado in PontosXTrechos.PilaresTestados)
                            {
                                foreach (TLinha ArestaAtual in PilarTestado.linhas_poligonal_aux)
                                {
                                    if (ArestaAtual.PontoEmLinha(PontosXTrechos.Pontos[PontoIntersecao].x, PontosXTrechos.Pontos[PontoIntersecao].y))
                                    {
                                        IncideEmOutraFace = true;
                                        break;
                                    }
                                }
                            }

                            if (IncideEmOutraFace)
                            {
                                PontoFinal.x = PontosXTrechos.Pontos[PontoIntersecao].x;
                                PontoFinal.y = PontosXTrechos.Pontos[PontoIntersecao].y;
                                PontoFinal.z = PontosXTrechos.Pontos[PontoIntersecao].z;
                            }
                        }
                    }

                }
            }

            foreach (TTrechoViga Trecho in TrechosVigas)
            {
                for (int i = 0; i < 4; i++)
                {
                    TPonto PontoFinal = null;

                    if (i == 0) PontoFinal = Trecho.linhas_facebaixo.pFin;
                    else
                    if (i == 1) PontoFinal = Trecho.linhas_facebaixo.pIni;
                    else
                    if (i == 2) PontoFinal = Trecho.linhas_facecima.pFin;
                    else
                    if (i == 3) PontoFinal = Trecho.linhas_facecima.pIni;

                    IncideEmOutraFace = false;
                    foreach (TTrechoViga TrechoTestado in TrechosVigas)
                    {
                       if ((Object)(TrechoTestado) != (Object)(Trecho))
                       if ((TrechoTestado.linhas_facebaixo.pFin == PontoFinal) || (TrechoTestado.linhas_facebaixo.pIni == PontoFinal) ||
                           (TrechoTestado.linhas_facecima.pFin == PontoFinal) || (TrechoTestado.linhas_facecima.pIni == PontoFinal))
                       {
                           IncideEmOutraFace = true;
                           break;
                       }
                    }

                    foreach (TPilar pilar in Pilares)
                    {
                       foreach (TLinha ArestaAtual in pilar.linhas_poligonal_aux)
                       {
                           if (ArestaAtual.PontoEmLinha(PontoFinal.x, PontoFinal.y))
                           {
                               IncideEmOutraFace = true;
                               break;
                           }
                       }
                    }

                   if (!IncideEmOutraFace)
                   {
                       if (i == 0)
                       {
                         Trecho.linhas_facebaixo.pFin.x = Trecho.linhas_facebaixo_org.pFin.x;
                         Trecho.linhas_facebaixo.pFin.y = Trecho.linhas_facebaixo_org.pFin.y;
                         Trecho.linhas_facebaixo.pFin.z = Trecho.linhas_facebaixo_org.pFin.z;
                       }
                       else
                       if (i == 1)
                       {
                           Trecho.linhas_facebaixo.pIni.x = Trecho.linhas_facebaixo_org.pIni.x;
                           Trecho.linhas_facebaixo.pIni.y = Trecho.linhas_facebaixo_org.pIni.y;
                           Trecho.linhas_facebaixo.pIni.z = Trecho.linhas_facebaixo_org.pIni.z;
                       }
                       else
                       if (i == 2)
                       {
                           Trecho.linhas_facecima.pFin.x = Trecho.linhas_facecima_org.pFin.x;
                           Trecho.linhas_facecima.pFin.y = Trecho.linhas_facecima_org.pFin.y;
                           Trecho.linhas_facecima.pFin.z = Trecho.linhas_facecima_org.pFin.z;
                       }
                       else
                       if (i == 3)
                       {
                           Trecho.linhas_facecima.pIni.x = Trecho.linhas_facecima_org.pIni.x;
                           Trecho.linhas_facecima.pIni.y = Trecho.linhas_facecima_org.pIni.y;
                           Trecho.linhas_facecima.pIni.z = Trecho.linhas_facecima_org.pIni.z;
                       }
                   }
                }

                foreach (TTrechoViga t1 in TrechosVigas)
                {
                    t1.ComVigaFaceInicial = false;
                    t1.ComVigaFaceFinal   = false;

                    foreach (TTrechoViga t2 in TrechosVigas)
                    {
                        if ((Object)t1 == (Object)t2)
                            continue;

                        if (t1.linhas_eixo.pIni == t2.linhas_eixo.pIni || t1.linhas_eixo.pIni == t2.linhas_eixo.pFin)
                            t1.ComVigaFaceInicial = true;

                        if (t1.linhas_eixo.pFin == t2.linhas_eixo.pIni || t1.linhas_eixo.pFin == t2.linhas_eixo.pFin)
                            t1.ComVigaFaceFinal = true;
                    }
                }
                    

                Trecho.UpdateAnguloTodas2();

                if (Math.Abs(Trecho.linhas_eixo.angulo - Trecho.linhas_facebaixo.angulo) > 1)
                {
                    Trecho.linhas_facebaixo.pFin.x = Trecho.linhas_facebaixo_org.pFin.x;
                    Trecho.linhas_facebaixo.pFin.y = Trecho.linhas_facebaixo_org.pFin.y;
                    Trecho.linhas_facebaixo.pFin.z = Trecho.linhas_facebaixo_org.pFin.z;
                    Trecho.linhas_facebaixo.pIni.x = Trecho.linhas_facebaixo_org.pIni.x;
                    Trecho.linhas_facebaixo.pIni.y = Trecho.linhas_facebaixo_org.pIni.y;
                    Trecho.linhas_facebaixo.pIni.z = Trecho.linhas_facebaixo_org.pIni.z;
                }
                if (Math.Abs(Trecho.linhas_eixo.angulo - Trecho.linhas_facecima.angulo) > 1)
                {
                    Trecho.linhas_facecima.pFin.x = Trecho.linhas_facecima_org.pFin.x;
                    Trecho.linhas_facecima.pFin.y = Trecho.linhas_facecima_org.pFin.y;
                    Trecho.linhas_facecima.pFin.z = Trecho.linhas_facecima_org.pFin.z;
                    Trecho.linhas_facecima.pIni.x = Trecho.linhas_facecima_org.pIni.x;
                    Trecho.linhas_facecima.pIni.y = Trecho.linhas_facecima_org.pIni.y;
                    Trecho.linhas_facecima.pIni.z = Trecho.linhas_facecima_org.pIni.z;
                }
                Trecho.UpdateAnguloTodas2();


            }
        }

        public static void DesfazIntersecoesTrechos(ref List<TTrechoViga> TrechosVigas)
        {
            TLinha fbo, fco, le = null, fc = null, fb = null;
            TTrechoViga tr1;

            for (int cc = 0; cc < TrechosVigas.Count; cc++)
            {
                tr1 = TrechosVigas[cc];

                fco = tr1.linhas_facecima_org;
                fbo = tr1.linhas_facebaixo_org;
                fb = tr1.linhas_facebaixo;
                fc = tr1.linhas_facecima;
                le = tr1.linhas_eixo;

                fc.pIni.x = fco.pIni.x;
                fc.pIni.y = fco.pIni.y;
                fc.pIni.z = fco.pIni.z;
                fc.pFin.x = fco.pFin.x;
                fc.pFin.y = fco.pFin.y;
                fc.pFin.z = fco.pFin.z;
               
                fc.UpdatePixel();

                fb.pIni.x = fbo.pIni.x;
                fb.pIni.y = fbo.pIni.y;
                fb.pIni.z = fbo.pIni.z;
                fb.pFin.x = fbo.pFin.x;
                fb.pFin.y = fbo.pFin.y;
                fb.pFin.z = fbo.pFin.z;
                fb.UpdatePixel();

                tr1.UpdateFBL(tr1);
                tr1.UpdateFBL2(tr1);
                tr1.UpdateFBM1(tr1);
                tr1.UpdateFBM2(tr1);

                tr1.UpdateFCL(tr1);
                tr1.UpdateFCM2(tr1);
                tr1.UpdateFCM1(tr1);
                tr1.UpdateFCL2(tr1);
            }

        }

        public static void ReordenaPontosIntersecao(List<TPonto> PontosIntersecao, TTrechoViga trecho)
        {
            TPonto temp;
            int kk;

            if (Geom.Iguais(trecho.linhas_eixo.pIni.y, trecho.linhas_eixo.pFin.y))
            {
                if (trecho.linhas_eixo.pIni.x > trecho.linhas_eixo.pFin.x)
                {
                    for (kk = 0; kk < PontosIntersecao.Count; kk++)
                    {
                        for (int j = kk; j < PontosIntersecao.Count; j++)
                        {
                            if (PontosIntersecao[j].x > PontosIntersecao[kk].x)
                            {
                                temp = PontosIntersecao[kk];
                                PontosIntersecao[kk] = PontosIntersecao[j];
                                PontosIntersecao[j] = temp;
                            }
                        }
                    }
                }
                else
                {
                    for (kk = 0; kk < PontosIntersecao.Count; kk++)
                    {
                        for (int j = kk; j < PontosIntersecao.Count; j++)
                        {
                            if (PontosIntersecao[j].x < PontosIntersecao[kk].x)
                            {
                                temp = PontosIntersecao[kk];
                                PontosIntersecao[kk] = PontosIntersecao[j];
                                PontosIntersecao[j] = temp;
                            }
                        }
                    }
                }
            }
            else
            {
                //senao reordena os nós em ordem crescente de Y, independente se for trecho vertical ou oblíquo
                if (trecho.linhas_eixo.pIni.y > trecho.linhas_eixo.pFin.y)
                {
                    for (kk = 0; kk < PontosIntersecao.Count; kk++)
                    {
                        for (int j = kk; j < PontosIntersecao.Count; j++)
                        {
                            if (PontosIntersecao[j].y > PontosIntersecao[kk].y)
                            {
                                temp = PontosIntersecao[kk];
                                PontosIntersecao[kk] = PontosIntersecao[j];
                                PontosIntersecao[j] = temp;
                            }
                        }
                    }
                }
                else
                {
                    for (kk = 0; kk < PontosIntersecao.Count; kk++)
                    {
                        for (int j = kk; j < PontosIntersecao.Count; j++)
                        {
                            if (PontosIntersecao[j].y < PontosIntersecao[kk].y)
                            {
                                temp = PontosIntersecao[kk];
                                PontosIntersecao[kk] = PontosIntersecao[j];
                                PontosIntersecao[j] = temp;
                            }
                        }
                    }
                }
            }
        }

        public static void DivideBarras(List<TBarraGenerica> barras)
        {

        }

        public static void SegmentaTrechosPelosPilares(int PavimentoAtual, FPrincipal desenho, ref List<TTrechoViga> TrechosVigas, ref List<TPilar> Pilares, ref List<TLinha> Linhas)
        {
            double iEixo_i = 0, iEixo_f = 0;
            double intery = 0, interx = 0;

            TPonto ponto = null;
            List<TPonto> TotalDePontos = new List<TPonto>();

            List<sPilarXPontos> PilarXPontos = new List<sPilarXPontos>();
            List<TTrechoViga> NovosTrechos = new List<TTrechoViga>();

               foreach (TTrechoViga TrechoAtual in TrechosVigas)
               {
               //    TrechoAtual.pIni.z = 0;
               //    TrechoAtual.pFin.z = 0;
               }

                foreach (TTrechoViga TrechoAtual in TrechosVigas)
                {
                PilarXPontos = new List<sPilarXPontos>();
                TotalDePontos.Clear();

                foreach (TPilar PilarAtual in Pilares)
                {
                    foreach (TLinha ArestaAtual in PilarAtual.Dados.Poligono.linhas_poligonal)
                    {
                        if (TrechoAtual.linhas_eixo_org.Intersec(ArestaAtual, ref iEixo_i, ref iEixo_f))
                        {
                         //   if (!TrechoAtual.linhas_eixo.pIni.Igual(iEixo_i, iEixo_f) && !TrechoAtual.linhas_eixo.pFin.Igual(iEixo_i, iEixo_f))
                            {
                                if (PilarXPontos.Count == 0)
                                    PilarXPontos.Add(new sPilarXPontos(PilarAtual, TrechoAtual));
                                else
                                if ((Object)PilarAtual != (Object)PilarXPontos[PilarXPontos.Count - 1].pilar)
                                    PilarXPontos.Add(new sPilarXPontos(PilarAtual, TrechoAtual));

                                //adiciona os pilares que o trecho passou por cima, com os respectivos pontos de intersecao em cada pilar 
                                ponto = new TPonto(iEixo_i, iEixo_f, TrechoAtual.pFin.z);
                                bool JaTem = false;
                                foreach (TPonto p in PilarXPontos[PilarXPontos.Count - 1].PontosIntersecao)
                                {
                                    if (Geom.Iguais(p.x, iEixo_i) && Geom.Iguais(p.y, iEixo_f))
                                        JaTem = true;
                                }

                                if (!JaTem)
                                  PilarXPontos[PilarXPontos.Count - 1].PontosIntersecao.Add(ponto);
                            }
                        }
                    }
                }


                    /*se tem dois pontos de intersecao no pilar e o primeiro esta em cima da aresta, remove ponto incial de intersecao 
                      por exemplo, num pilar retangular, pode significar que o usuario clicou em cima aresta da esquerda e passou por cima da outra
                      aresta da direita para continuar o trecho, por isso desconsidero o primeiro ponto, senao fica errado a segmentação e 
                      vai criar um trechinho "dentro" do pilar */

                if (PilarXPontos.Count > 0 && PilarXPontos[PilarXPontos.Count - 1].PontosIntersecao.Count == 2)
                {
                    bool naAresta_pIni = false;

                    ReordenaPontosIntersecao(PilarXPontos[PilarXPontos.Count - 1].PontosIntersecao, TrechoAtual);
                    foreach (TLinha ArestaAtual in PilarXPontos[PilarXPontos.Count - 1].pilar.Dados.Poligono.linhas_poligonal)
                      if (ArestaAtual.PontoEmLinha(TrechoAtual.linhas_eixo_org.pIni.x, TrechoAtual.linhas_eixo_org.pIni.y))
                      {
                         naAresta_pIni = true;
                         break;
                      }
            
                    if (naAresta_pIni)
                        PilarXPontos[PilarXPontos.Count - 1].PontosIntersecao.RemoveAt(0);
                }

                //reordena os pontos de intersecao.
                //se ocorrer de ter 3 pontos de intersecao, em qual deles começa e em qual termina o segmentação do trecho? só posso
                //saber isso se eu ordenar em x ou em y os pontos de intersecao, pois o primeiro e o ultimo ponto vao ser o inicio e fim de cada trecho
                for (int i = 0; i < PilarXPontos.Count; i++)
                {
                    ReordenaPontosIntersecao(PilarXPontos[i].PontosIntersecao, TrechoAtual);

                    //se tiver mais que 2 pontos, deleto o resto e mantenho somente os dois pontos extremos
                    if (PilarXPontos[i].PontosIntersecao.Count > 2)
                    {
                        int maximo = PilarXPontos[PilarXPontos.Count - 1].PontosIntersecao.Count - 1;

                        //pego os dois pontos extremos
                        TPonto p1 = new TPonto(PilarXPontos[i].PontosIntersecao[0].x, PilarXPontos[i].PontosIntersecao[0].y, PilarXPontos[i].PontosIntersecao[0].z);
                        TPonto p2 = new TPonto(PilarXPontos[i].PontosIntersecao[maximo].x, PilarXPontos[i].PontosIntersecao[maximo].y, PilarXPontos[i].PontosIntersecao[maximo].z);

                        //limpo
                        PilarXPontos[i].PontosIntersecao.Clear();

                        //adiciono
                        PilarXPontos[i].PontosIntersecao.Add(p1);
                        PilarXPontos[i].PontosIntersecao.Add(p2);
                    }

                    foreach (TPonto pt in PilarXPontos[i].PontosIntersecao)
                        TotalDePontos.Add(pt);
                }

                if (TotalDePontos.Count > 0)
                {
                    //por exemplo, se o ponto inicial do trecho não estiver dentro do poligono de nenhum dos pilares, considero como sendo um 
                    //ponto de segmentacao
                    //se ele estiver dentro de algum pilar, ou na aresta de um pilar, desconsidero como ponto de segmentacao, pois o ponto de segmentacao é aquele que ja 
                    //interceptou na aresta

                    bool dentro = false;
                    for (int i = 0; i < PilarXPontos.Count; i++)
                      if (PilarXPontos[i].pilar.Dados.Poligono.PontoEmPoligono(TrechoAtual.linhas_eixo_org.pIni.x, TrechoAtual.linhas_eixo_org.pIni.y))
                         dentro = true;

                    //se ponto inicial estiver na aresta, o ponto esta no poligono, entao desconsidero tbm...
                    for (int i = 0; i < PilarXPontos.Count; i++)
                    {
                        foreach (TLinha ArestaAtual in PilarXPontos[i].pilar.Dados.Poligono.linhas_poligonal)
                        {
                            if (ArestaAtual.PontoEmLinha(TrechoAtual.linhas_eixo_org.pIni.x, TrechoAtual.linhas_eixo_org.pIni.y))
                                dentro = true;
                        }
                    }

                    if (!dentro)
                    {
                        ponto = new TPonto(TrechoAtual.linhas_eixo_org.pIni.x, TrechoAtual.linhas_eixo_org.pIni.y, TrechoAtual.linhas_eixo_org.pIni.z);
                        TotalDePontos.Add(ponto);
                    }

                    dentro = false;
                    for (int i = 0; i < PilarXPontos.Count; i++)
                    {
                        if (PilarXPontos[i].pilar.Dados.Poligono.PontoEmPoligono(TrechoAtual.linhas_eixo_org.pFin.x, TrechoAtual.linhas_eixo_org.pFin.y))
                            dentro = true;
                    }

                    //se ponto inicial estiver na aresta, o ponto esta no poligono, entao desconsidero tbm...
                    for (int i = 0; i < PilarXPontos.Count; i++)
                    {
                        foreach (TLinha ArestaAtual in PilarXPontos[i].pilar.Dados.Poligono.linhas_poligonal)
                        {
                            if (ArestaAtual.PontoEmLinha(TrechoAtual.linhas_eixo_org.pFin.x, TrechoAtual.linhas_eixo_org.pFin.y))
                                dentro = true;
                        }
                    }

                    if (!dentro)
                    {
                        ponto = new TPonto(TrechoAtual.linhas_eixo_org.pFin.x, TrechoAtual.linhas_eixo_org.pFin.y, TrechoAtual.linhas_eixo_org.pFin.z);
                        TotalDePontos.Add(ponto);
                    }

                    //laço de repetição para remover pontos iguais
                    for (int i = 0; i < TotalDePontos.Count; i++)
                    {
                        TPonto p1 = TotalDePontos[i];
                        for (int j = 0; j < TotalDePontos.Count; j++)
                        {
                            if ((Object)p1 != (Object)TotalDePontos[j] && (p1.DistanceTo(TotalDePontos[j]) < 1))
                                p1.z = 1; // se for distancia pequena, entao sinalizo z = 1
                        }
                    }

                    //remove aqueles marcados com z = 1
                    TotalDePontos.RemoveAll(obj => obj.z == 1);

                    //reordena todos os pontos...
                    ReordenaPontosIntersecao(TotalDePontos, TrechoAtual);

                    if (TotalDePontos.Count > 0)
                    {
                        TrechoAtual.SelecionaTodosElementos(TrechoAtual);

                        int totalSegmentos = TotalDePontos.Count / 2;

                        int n1 = 0, n2 = 0;
                        for (int j = 0; j < totalSegmentos; j++)
                        {
                            if (j == 0)
                            {
                                n1 = 0;
                                n2 = 1;
                            }
                            else
                            {
                                n1 = n2 + 1;
                                n2 = n1 + 1;
                            }

                            //  TPoligono pol   = new TPoligono(TrechoAtual.Dados.Poligono.TipoSecao, TrechoAtual.Dados.h1.ToString(), TrechoAtual.Dados.b1.ToString(), TrechoAtual.Dados.b2.ToString(), TrechoAtual.Dados.h2.ToString(), "", "", false);
                            //   TDadosViga dad  = new TDadosViga(TrechoAtual.Dados.b1, TrechoAtual.Dados.b2, TrechoAtual.Dados.h1, TrechoAtual.Dados.h2, TrechoAtual.Dados.revestimento, TrechoAtual.Dados.indiceTipo, TrechoAtual.Dados.Poligono, TrechoAtual.Dados.nome, TrechoAtual.Dados.numero, TrechoAtual.Dados.ins_face1, TrechoAtual.Dados.ins_face2, TrechoAtual.Dados.ins_eixo);
                            TTrechoViga New = new TTrechoViga(TotalDePontos[n1], TotalDePontos[n2], TrechoAtual.Dados, TrechoAtual.layer, TrechoAtual.Pavimento, Linhas);
                            NovosTrechos.Add(New);
                        }
                    }
                }
            }

            foreach (TTrechoViga trecho in NovosTrechos)
            {
           //     PavimentoAtual.LayersByIdPrincipal[Lay.Vigas].AddObject(trecho);
                desenho.AdicionaObjeto(trecho, PavimentoAtual);
            }

            foreach (TLinha lin in Linhas)
                if (lin.barraRigida)
                    lin.Selecionado = true;

            //desenho.RemoveSelecionados();

            foreach (TTrechoViga TrechoAtual in TrechosVigas)
            {
                TrechoAtual.NumPilar_PontoFinal = 0;
                TrechoAtual.NumPilar_PontoInicial = 0;
            }

            foreach (TPilar pilar in Pilares)
            {
                foreach (TTrechoViga TrechoAtual in TrechosVigas)
                {
                    if (pilar.PontoEmAresta(TrechoAtual.linhas_eixo.pFin.x, TrechoAtual.linhas_eixo.pFin.y,.1))
                      TrechoAtual.NumPilar_PontoFinal = pilar.Dados.numero;
                   
                    if (pilar.PontoEmAresta(TrechoAtual.linhas_eixo.pIni.x, TrechoAtual.linhas_eixo.pIni.y,.1))
                      TrechoAtual.NumPilar_PontoInicial = pilar.Dados.numero;
                }
            }

            /*Barras rígidas*/
            TPonto pt1 = null, pt2 = null;
            TPoligono poligono = null;
            TLinha barraRigida;

            foreach (TTrechoViga TrechoAtual in TrechosVigas)
            {

                if (TrechoAtual.NumPilar_PontoInicial > 0)
                {
                    poligono = null;

                    TPilar PilarAtual;
                    foreach (TPilar pilar in Pilares)
                    {
                        if (pilar.Dados.numero == TrechoAtual.NumPilar_PontoInicial)
                        {
                            poligono = pilar.Dados.Poligono;
                            TrechoAtual.BarraRigida_Ini = null;
                            PilarAtual = pilar;
                        }
                    }

                    if (poligono != null)
                    {
                        pt1 = TrechoAtual.linhas_eixo.pIni;
                        pt2 = new TPonto(poligono.centroide.X, poligono.centroide.Y, TrechoAtual.linhas_eixo.pIni.z);

                        desenho.AddPoint(pt2, "", true);
                        desenho.AddPoint(pt1, "", true);

                        barraRigida = new TLinha(pt1, pt2, -1, null, false, false, false, true, false, false, true, TrechoAtual, TrechoAtual.layer, false);

                        barraRigida.barraRigida = true;
                        barraRigida.layer = desenho.Estrutura.LayersByIdPrincipal[Lay.Vigas];

                        desenho.AddLinha(barraRigida, PavimentoAtual,true, false, false, null, -1, false);

                        TrechoAtual.BarraRigida_Ini = barraRigida;
                        //      PilarAtual.BarrasRigidasDeViga.Add(barraRigida);
                    }
                }

                if (TrechoAtual.NumPilar_PontoFinal > 0)
                {
                    poligono = null;
                    foreach (TPilar pilar in Pilares)
                    {
                        if (pilar.Dados.numero == TrechoAtual.NumPilar_PontoFinal)
                        { 
                            poligono = pilar.Dados.Poligono;
                            TrechoAtual.BarraRigida_Fin = null;
                        }
                    }

                    if (poligono != null)
                    {
                        pt1 = TrechoAtual.linhas_eixo.pFin;
                        pt2 = new TPonto(poligono.centroide.X, poligono.centroide.Y, TrechoAtual.linhas_eixo.pFin.z);
                        pt2.PontoCentroidePilar = true;

                        desenho.AddPoint(pt2, "", true);
                        desenho.AddPoint(pt1, "", true);

                        barraRigida = new TLinha(pt1, pt2, -1, null, false, false, false, true, false, false, true, TrechoAtual, TrechoAtual.layer, false);

                        barraRigida.barraRigida = true;
                        barraRigida.layer = desenho.Estrutura.LayersByIdPrincipal[Lay.Vigas];

                        desenho.AddLinha(barraRigida, PavimentoAtual, true, false, false, null, -1, false);

                        TrechoAtual.BarraRigida_Fin = barraRigida;
                    }
                }

                TrechoAtual.UpdateAnguloTodas2();
            }

            return;

            /*Ajusta as faces dos trechos nos pilares*/
            TLinha FaceLonga = null;
            TLinha FaceCimaOuBaixo = null;
            TPonto PontoInicialOuFinalEixo = null;
            TPonto PontoInicialOuFinalFaces = null;
            int NumPilar;
            List<TPonto> PontosIntersec = new List<TPonto>();
            foreach (TTrechoViga TrechoAtual in TrechosVigas)
            {
                for (int i = 0; i < 2; i++)
                {
                    if (i == 0)
                    {
                        PontoInicialOuFinalEixo = TrechoAtual.linhas_eixo.pIni;
                        NumPilar = TrechoAtual.NumPilar_PontoInicial;
                    }
                    else
                    {
                        PontoInicialOuFinalEixo = TrechoAtual.linhas_eixo.pFin;
                        NumPilar = TrechoAtual.NumPilar_PontoFinal;
                    }

                    if (NumPilar > 0)
                    {
                        TPilar PilarAtual = null;
                        foreach (TPilar pilar in Pilares)
                            if (pilar.Dados.numero == NumPilar)
                                PilarAtual = pilar;

                        if (PilarAtual != null)
                        {
                            for (int j = 0; j < 2; j++)
                            {
                                if (j == 0)
                                {
                                    if (i == 0)
                                        FaceLonga = TrechoAtual.linhas_facecima_longa_m1;
                                    else
                                        FaceLonga = TrechoAtual.linhas_facecima_longa_m2;

                                    FaceCimaOuBaixo = TrechoAtual.linhas_facecima;
                                }
                                else
                                {
                                    if (i == 0)
                                        FaceLonga = TrechoAtual.linhas_facebaixo_longa_m1;
                                    else
                                        FaceLonga = TrechoAtual.linhas_facebaixo_longa_m2;

                                    FaceCimaOuBaixo = TrechoAtual.linhas_facebaixo;
                                }

                                PontosIntersec.Clear();
                                foreach (TLinha ArestaAtual in PilarAtual.Dados.Poligono.linhas_poligonal)
                                {
                                    if (FaceLonga.Intersec(ArestaAtual, ref interx, ref intery))
                                        PontosIntersec.Add(new TPonto(interx, intery, ArestaAtual.pIni.z));
                                }

                                //se houver mais de um ponto de interesecao, pega aquele mais proximo do ponto do eixo. Só consegui pensar isso por enquanto...
                                double dist = 9999999;
                                foreach (TPonto pt in PontosIntersec)
                                {
                                    if (pt.DistanceTo(PontoInicialOuFinalEixo) < dist)
                                    {
                                        dist = pt.DistanceTo(PontoInicialOuFinalEixo);
                                        pt1 = pt;
                                    }
                                }

                                if (PontosIntersec.Count > 0)
                                {
                                    if (i == 0)
                                        PontoInicialOuFinalFaces = FaceCimaOuBaixo.pIni;
                                    else
                                        PontoInicialOuFinalFaces = FaceCimaOuBaixo.pFin;

                                    PontoInicialOuFinalFaces.x = pt1.x;
                                    PontoInicialOuFinalFaces.y = pt1.y;
                                    PontoInicialOuFinalFaces.z = pt1.z;
                                    PontoInicialOuFinalFaces.px_x = FPrincipal.pixelX(pt1.x);
                                    PontoInicialOuFinalFaces.px_y = FPrincipal.pixelY(pt1.y);
                                }
                            }
                        }
                    }
                }
            }
        }
        struct IntersecBarras
        {
            public vec3 ponto;
            public TBarraGenerica b1, b2;
        }

        public struct BarrasXPonto_Divisao
        {
            public vec3 ponto;
            public TBarraGenerica BarraDividir, BarraConectada;
            public bool PontoInicial;
            public TPonto PontoBarra;
        }

        static List<IntersecBarras> IntersecoesBarras = new List<IntersecBarras>();
        public static bool EhPontoExtremo(ref vec3 ponto, ref List<TLinha> linhas)
        {
            foreach (TLinha l in linhas)
            {
                if (((Geom.Iguais(l.pIni.x, ponto.x)) && (Geom.Iguais(l.pIni.y, ponto.y) && (Geom.Iguais(l.pIni.z, ponto.z)))) ||
                    ((Geom.Iguais(l.pFin.x, ponto.x)) && (Geom.Iguais(l.pFin.y, ponto.y) && (Geom.Iguais(l.pFin.z, ponto.z)))))
                    return true;
            }
            return false;
        }
        static bool EmListaParaDividir(ref TBarraGenerica b, double x, double y, double z)
        {
            foreach (BarrasXPonto_Divisao o in Divisoes)
              if ((Object)o.BarraDividir == (Object)b)
                if ((Geom.Iguais(o.PontoBarra.x, x) && Geom.Iguais(o.PontoBarra.y, y) && Geom.Iguais(o.PontoBarra.z, z)))
                  return true;
          
            return false;
        }

        static bool NaoEstaConectadaEmPontoExtremo(ref TBarraGenerica b1, ref TBarraGenerica b2)
        {
            if ((Geom.Iguais(b1.pIni.x, b2.pIni.x) && Geom.Iguais(b1.pIni.y, b2.pIni.y) && Geom.Iguais(b1.pIni.z, b2.pIni.z)))
                return false;

            if ((Geom.Iguais(b1.pFin.x, b2.pFin.x) && Geom.Iguais(b1.pFin.y, b2.pFin.y) && Geom.Iguais(b1.pFin.z, b2.pFin.z)))
                return false;

            if ((Geom.Iguais(b1.pIni.x, b2.pFin.x) && Geom.Iguais(b1.pIni.y, b2.pFin.y) && Geom.Iguais(b1.pIni.z, b2.pFin.z)))
                return false;

            if ((Geom.Iguais(b1.pFin.x, b2.pIni.x) && Geom.Iguais(b1.pFin.y, b2.pIni.y) && Geom.Iguais(b1.pFin.z, b2.pIni.z)))
                return false;

            return true;
        }

        static List<TBarraGenerica> BarrasDividir;
        static List<BarrasXPonto_Divisao> Divisoes;

        public static void RetornaBarrasDivididas2(ref List<TBarraGenerica> barrasModelo, List<TCargaLinear> CargasConcentradas, List<TCargaPontual> cargasPontuaisEstrutura)
        {
            vec3 p_1, p_2, p_3, p_4;
            vec3 p_r = new vec3(0, 0, 0);
            vec3 ponto = new vec3(0, 0, 0);
            TBarraGenerica b1, b2;
            BarrasDividir = new List<TBarraGenerica>();
            Divisoes = new List<BarrasXPonto_Divisao>();

            List<TBarraGenerica> bars = new List<TBarraGenerica>();
            List<PontoDivisao> pontos = new List<PontoDivisao>();
            List<PontoDivisao> nos_ordenados = new List<PontoDivisao>();

            bars = barrasModelo.ToList();
            TBarraGenerica barranova = null;

            List<int> barras_com_cargas_concentradas = CargasConcentradas.Select(o => o.idBarra).ToList();

            int j;
            for (int i = 0; i < bars.Count; i++)
            {
                b1 = bars[i];
                if (b1 == null) continue;

                if (b1.Dados.Tipo == 1) // tirante
                    continue;
                p_1 = new vec3(b1.pIni.x, b1.pIni.y, b1.pIni.z);
                p_2 = new vec3(b1.pFin.x, b1.pFin.y, b1.pFin.z);

                pontos.Clear();
                for (j = 0; j < bars.Count; j++)
                {
                    b2 = bars[j];
                    if (b2 == null) continue;

                    /*if (b2.Dados.Tipo == 1) // tirante
                    {
                        b2.Dados.Tipo = 1;
                        continue;
                    }
                    */
                    if ((Object)b1 != (Object)b2)
                    {
                  //      if (b1.IDBarra == 261 && (b2.IDBarra == 257))
                    //        b1.IDBarra = 261;

                        p_3 = new vec3(b2.pIni.x, b2.pIni.y, b2.pIni.z);
                        p_4 = new vec3(b2.pFin.x, b2.pFin.y, b2.pFin.z);

                        vec3 pontoToque = new vec3(0);
                        
                        var r1 = Geom.ClassificarPontoNoSegmento(p_3, p_1, p_2);
                        var r2 = Geom.ClassificarPontoNoSegmento(p_4, p_1, p_2);

                        if (r1.Posicao == PosicaoNoSegmento.Fora && r2.Posicao == PosicaoNoSegmento.Fora)
                            continue;

                        if (r1.Posicao != PosicaoNoSegmento.Fora)
                        {
                            if (LocalizaPonto(ref pontos, b2.pIni.x, b2.pIni.y, b2.pIni.z) == -1)
                              pontos.Add(new PontoDivisao(b2.pIni, r1.T));
                        }

                        if (r2.Posicao != PosicaoNoSegmento.Fora)
                        {
                            if (LocalizaPonto(ref pontos, b2.pFin.x, b2.pFin.y, b2.pFin.z) == -1)
                               pontos.Add(new PontoDivisao(b2.pFin, r2.T));
                        }
                    }
                }
 
                if (barras_com_cargas_concentradas.Exists(o => o == b1.IDBarra))
                {
                    List<TCargaLinear> cls = CargasConcentradas.FindAll(o => o.Dados.concentrada && o.idBarra == b1.IDBarra).ToList();
                    for (int jj = 0; jj < cls.Count; jj++)
                    {
                        p_1 = new vec3(b1.pIni.x, b1.pIni.y, b1.pIni.z);
                        p_2 = new vec3(b1.pFin.x, b1.pFin.y, b1.pFin.z);

                        p_3 = new vec3(cls[jj].setas[0].l_principal.p2.x, cls[jj].setas[0].l_principal.p2.y, cls[jj].setas[0].l_principal.p2.z);

                        vec3 pontoToque = new vec3(0);

                        var r1 = Geom.ClassificarPontoNoSegmento(p_3, p_1, p_2);

                        if (r1.Posicao != PosicaoNoSegmento.Fora)
                        {
                            if (LocalizaPonto(ref pontos, p_3.x, p_3.y, p_3.z) == -1)
                            {
                                if (r1.Posicao == PosicaoNoSegmento.Interior)
                                  pontos.Add(new PontoDivisao(new TPonto(p_3.x, p_3.y, p_3.z), r1.T));                               
                            }

                            TDadosCarga DadosCarga = (TDadosCarga)cls[jj].Dados.Clone();
                            cargasPontuaisEstrutura.Add(new TCargaPontual(DadosCarga, new TPonto(p_3.x, p_3.y, p_3.z), null));
                            cargasPontuaisEstrutura[cargasPontuaisEstrutura.Count - 1].apagarAposCalculo = true;

                        }
                    }
                }

                if (LocalizaPonto(ref pontos, b1.pIni.x, b1.pIni.y, b1.pIni.z) == -1)
                    pontos.Add(new PontoDivisao(b1.pIni, 0));

                if (LocalizaPonto(ref pontos, b1.pFin.x, b1.pFin.y, b1.pFin.z) == -1)
                    pontos.Add(new PontoDivisao(b1.pFin, 1));

                pontos.Sort((a, b) => a.T.CompareTo(b.T));

                if (pontos.Count > 0)
                {
                    b1.Tipo = "desconsiderar";

                    for (int k = 0; k < pontos.Count-1; k++)
                    {
                        barranova = new TBarraGenerica(pontos[k].Ponto, pontos[k+1].Ponto, b1.layer, (TDadosBarra)b1.Dados.Clone(), -1);

                        barranova.comprimento = (float)barranova.pFin.DistanceTo(barranova.pIni);
                        barranova.IDBarra = b1.IDBarra;
                        barranova.Linha_Eixo.pIni = barranova.pIni;
                        barranova.Linha_Eixo.pFin = barranova.pFin;
                      

                        barrasModelo.Add(barranova);
                    }
                }
            }

            barrasModelo.RemoveAll(o=>o.Tipo == "desconsiderar");

        }
        private static bool CargaConcentradaNaBarra(TBarraGenerica barra, TCargaLinear CargaLinear)
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
        public class PontoDivisao
        {
            public double T { get; set; }
            public TPonto Ponto { get; set; }
            public PontoDivisao(TPonto _p, double _t)
            {
                Ponto = _p;
                T = _t;
            }
        }
        public static int LocalizaPonto(ref List<PontoDivisao> lista, double x, double y, double z)
        {
            for (int jk = 0; jk < lista.Count; jk++)
                if (Geom.Iguais(x, lista[jk].Ponto.x) && Geom.Iguais(y, lista[jk].Ponto.y) && Geom.Iguais(z, lista[jk].Ponto.z))
                    return jk;

            return -1;
        }

        public static int LocalizaPonto(ref List<TPonto> lista, double x, double y, double z)
        {
            for (int jk = 0; jk < lista.Count; jk++)
                if (Geom.Iguais(x, lista[jk].x) && Geom.Iguais(y, lista[jk].y) && Geom.Iguais(z, lista[jk].z))
                    return jk;

            return -1;
        }
        public static int LocalizaPonto(ref List<vec3> lista, double x, double y, double z)
        {
            for (int jk = 0; jk < lista.Count; jk++)
                if (Geom.Iguais(x, lista[jk].x) && Geom.Iguais(y, lista[jk].y) && Geom.Iguais(z, lista[jk].z))
                    return jk;

            return -1;
        }

        public static void RetornaBarrasDivididas(ref List<TBarraGenerica> barrasModelo)
        {
            double resposta;

            vec3 p_1, p_2, p_3, p_4;
            vec3 p_r = new vec3(0, 0, 0);
            vec3 ponto = new vec3(0, 0, 0);
            TBarraGenerica BarraDividir, BarraConectada, b1, b2;

            BarraDividir = null;
            BarraConectada = null;

            BarrasDividir = new List<TBarraGenerica>();
            Divisoes = new List<BarrasXPonto_Divisao>();
            TPonto PontoBarra = null;
            double tt = 0, uu = 0;
            List<TBarraGenerica> bars = new List<TBarraGenerica>();
            bars = barrasModelo.ToList();
            TBarraGenerica barranova = null;
            TPonto pIni = null;
            TPonto pFin = null;
            TPonto pa, pb;
            double xf = 0, yf = 0, zf = 0;
            BarrasXPonto_Divisao div;
            bool temconexao = false;
            int j;
            //else mouseedit, entao uso toda a lista de barras
           // if (bars != null)
              //  if (bars.Count > 1)
                {
                    for (int i = 0; i < bars.Count; i++)
                    {
                        // desenho.Progresso.Increment(1);
                        b1 = bars[i];
                        if (b1 == null) continue;
                        //       desenho.Progresso.Refresh();
                        // System.Windows.Forms.Application.DoEvents();
                        //  b1 = desenho.LinhaPonto1.Barra as TBarraGenerica;

                        p_1 = new vec3(b1.pIni.x, b1.pIni.y, b1.pIni.z);
                        p_2 = new vec3(b1.pFin.x, b1.pFin.y, b1.pFin.z);
                        
                        temconexao = false;

                        for (j = 0; j < bars.Count; j++)
                        {
                            b2 = bars[j];
                            if (b2 == null) continue;

                            if ((Object)b1 != (Object)b2)
                            {

                                p_3 = new vec3(b2.pIni.x, b2.pIni.y, b2.pIni.z);
                                p_4 = new vec3(b2.pFin.x, b2.pFin.y, b2.pFin.z);

                                if (Geom.DuasArestasSeTocam(ref p_1, ref p_2, ref p_3, ref p_4, ref ponto, false))
                                {
                                    BarraDividir = null;
                                    BarraConectada = null;

                                    if (Geom.Iguais(b1.pIni.x, ponto.x) && Geom.Iguais(b1.pIni.y, ponto.y) && Geom.Iguais(b1.pIni.z, ponto.z))
                                    {
                                        BarraDividir = b2;
                                        BarraConectada = b1;
                                        PontoBarra = b1.pIni;
                                    }
                                    else
                                    if (Geom.Iguais(b1.pFin.x, ponto.x) && Geom.Iguais(b1.pFin.y, ponto.y) && Geom.Iguais(b1.pFin.z, ponto.z))
                                    {
                                        BarraDividir = b2;
                                        BarraConectada = b1;
                                        PontoBarra = b1.pFin;
                                    }
                                    else
                                    if (Geom.Iguais(b2.pIni.x, ponto.x) && Geom.Iguais(b2.pIni.y, ponto.y) && Geom.Iguais(b2.pIni.z, ponto.z))
                                    {
                                        BarraDividir = b1;
                                        BarraConectada = b2;
                                        PontoBarra = b2.pIni;
                                    }
                                    else
                                    if (Geom.Iguais(b2.pFin.x, ponto.x) && Geom.Iguais(b2.pFin.y, ponto.y) && Geom.Iguais(b2.pFin.z, ponto.z))
                                    {
                                        BarraDividir = b1;
                                        BarraConectada = b2;
                                        PontoBarra = b2.pFin;
                                    }

                                    if (BarraDividir != null)
                                        if (NaoEstaConectadaEmPontoExtremo(ref BarraDividir, ref BarraConectada))
                                            if (!EmListaParaDividir(ref BarraDividir, PontoBarra.x, PontoBarra.y, PontoBarra.z))
                                            {
                                                BarrasDividir.Add(BarraDividir);
                                                temconexao = true;
                                                break;
                                            }
                                }
                            }
                        }

                        if (temconexao)
                            break;
                    }
                }

            if (temconexao)
            {
                div = new BarrasXPonto_Divisao();
                div.ponto = ponto;
                div.BarraDividir = (TBarraGenerica)BarraDividir;
                div.BarraDividir.IDBarra = BarraDividir.IDBarra;
                div.BarraConectada = BarraConectada;
                div.PontoBarra = PontoBarra;

                pIni = (TPonto)div.PontoBarra.Clone();
                pFin = (TPonto)div.BarraDividir.pFin.Clone();

                div.BarraDividir.pFin = div.PontoBarra;
                div.BarraDividir.Linha_Eixo.pFin = div.BarraDividir.pFin;
                div.BarraDividir.Linha_Eixo.pIni = div.BarraDividir.pIni;

                div.BarraDividir.comprimento = (float)div.BarraDividir.pFin.DistanceTo(div.BarraDividir.pIni);

                barranova = new TBarraGenerica(pIni, pFin, div.BarraDividir.layer, (TDadosBarra)div.BarraDividir.Dados.Clone(), -1);

                barranova.comprimento = (float)barranova.pFin.DistanceTo(barranova.pIni);
                barranova.IDBarra = div.BarraDividir.IDBarra;
                barranova.Linha_Eixo.pIni = barranova.pIni;
                barranova.Linha_Eixo.pFin = barranova.pFin;

                barrasModelo.Add(barranova);

                RetornaBarrasDivididas(ref barrasModelo);
            }


            /*   foreach (BarrasXPonto_Divisao divisao in Divisoes)
               {
                   xf = divisao.BarraDividir.pFin.x;
                   yf = divisao.BarraDividir.pFin.y;
                   zf = divisao.BarraDividir.pFin.z;

                   pIni = (TPonto)divisao.PontoBarra.Clone();
                   pFin = (TPonto)divisao.BarraDividir.pFin.Clone();

                   pa = (TPonto)divisao.BarraDividir.pFin.Clone();

                   divisao.BarraDividir.pFin = divisao.PontoBarra;
                   divisao.BarraDividir.Linha_Eixo.pFin = divisao.BarraDividir.pFin;
                   divisao.BarraDividir.Linha_Eixo.pIni = divisao.BarraDividir.pIni;

                   pb = (TPonto)divisao.PontoBarra.Clone();

                   divisao.BarraDividir.comprimento = (float)divisao.BarraDividir.pFin.DistanceTo(divisao.BarraDividir.pIni);
                //   divisao.BarraDividir.RotacionaSecao();

                   // desenho.ObjetosMovidos.Add(divisao.BarraDividir.pFin);

                   // desenho.undoBuffer.AdicionaComando(new ComandoMover(desenho.ObjetosMovidos,pa, pb));

                   // desenho.comandosMover.Add(new ComandoMover(divisao.BarraDividir, "pFin", pa, pb));

                   barranova = new TBarraGenerica(pIni, pFin, divisao.BarraDividir.layer, (TDadosBarra)divisao.BarraDividir.Dados.Clone(), -1);
                   /*  TTrechoViga New = new TTrechoViga((TPonto)(objeto as TTrechoViga).pIni.Clone(),
                                                       (TPonto)(objeto as TTrechoViga).pFin.Clone(),
                                                       ((TDadosViga)(objeto as TTrechoViga).Dados.Clone()),
                                                       objeto.layer, Estrutura, (objeto as TTrechoViga).Pavimento, Linhas, true);*/

            /*     barranova.comprimento = (float)barranova.pFin.DistanceTo(barranova.pIni);
                 barranova.IDBarra = divisao.BarraDividir.IDBarra;
                 barranova.Linha_Eixo.pIni = barranova.pIni;
                 barranova.Linha_Eixo.pFin = barranova.pFin;

                 barrasNovas.Add(barranova);
                 barrasNovas.Add(divisao.BarraDividir);

                 //  barranova.RotacionaSecao();
                 //    barranova.CriaPesoProprio();

                 //   foreach (TLinha l in linhas)
                 ///       if ((Object)l == (Object)divisao.BarraDividir.Linha_Eixo)
                 //             l.SetaSelecao(true, true);
                 //barranova.CriaPesoProprio();

                 // desenho.AdicionaBarraGenerica(barranova);
                 //  desenho.ObjetosAdicionados.Add(barranova);
                 //                barranova.CriaPesoProprio();
                 //               desenho.AdicionaObjeto(desenho.Estrutura.barras[desenho.Estrutura.barras.Count - 1].PesoProprio as TCargaLinear, -1);
                 /*    for (int i = 0; i < cargalinear.Count; i++)
                     {
                         if (cargalinear[i].idBarra == barranova.IDBarra && cargalinear[i].Dados.idCaso == -1)
                         {
                             desenho.Estrutura.cargaLinear[i].anguloRotacao = barranova.anguloRotacao;
                             desenho.Estrutura.cargaLinear[i].RotacionaDiagramaCarga(true, 0, 0, desenho.fatorCarga);
                         }
                    }*/
            /* List<TCargaLinear> cl = cargalinear.FindAll(c => c.idBarra == divisao.BarraDividir.IDBarra);
             foreach (TCargaLinear clinear in cl)
             {
                 if (clinear.Dados.idCaso != 1)
                 {
                     TCargaLinear carganova = new TCargaLinear(barranova.pIni, barranova.pFin, (TDadosCarga)clinear.Dados.Clone(),
                                                               barranova.Dados.anguloRotacao, barranova.IDBarra, clinear.layer);
                     desenho.AdicionaObjeto(carganova, -1, true, false);
                     desenho.Estrutura.cargaLinear[desenho.Estrutura.cargaLinear.Count - 1].RotacionaDiagramaCarga(true, 0, 0, desenho.fatorCarga);
                     desenho.ObjetosAdicionados.Add(carganova);
                 }
             }*/

            // if (cl != null)
            //{
            /*   if (cl.Dados.idCaso != -1)
               {
                   desenho.AdicionaObjeto(desenho.Estrutura.barras[desenho.Estrutura.barras.Count - 1].PesoProprio as TCargaLinear, -1);


                   desenho.Settings = (TDadosCarga)cl.Dados.Clone();
                   desenho.IdObjetoDesenho = Const.ID_CARGA_LINEAR;
                   desenho.MouseDrawing(barranova.pIni.x, barranova.pIni.y, barranova.pIni.z);
                   desenho.MouseDrawing(barranova.pFin.x, barranova.pFin.y, barranova.pFin.z);

                   desenho.Estrutura.cargaLinear[desenho.Estrutura.cargaLinear.Count - 1].idBarra = barranova.IDBarra;
                   desenho.Estrutura.cargaLinear[desenho.Estrutura.cargaLinear.Count - 1].Dados = (TDadosCarga)cl.Dados.Clone();
                   desenho.Estrutura.cargaLinear[desenho.Estrutura.cargaLinear.Count - 1].anguloRotacao = barranova.Dados.anguloRotacao;
                   desenho.Estrutura.cargaLinear[desenho.Estrutura.cargaLinear.Count - 1].RotacionaDiagramaCarga(true, 0, 0, desenho.fatorCarga);
               }*/

            // }
            // }

            /* if (Divisoes.Count > 0)
             {
                 List<TBarraGenerica> barrasDivididas = new List<TBarraGenerica>();
                 RetornaBarrasDivididas(barrasNovas, ref barrasDivididas);
             }*/
        }
        public static List<TBarraGenerica> RetornaConexoes2(ref List<TBarraGenerica> _elementos, List<TApoio> apoios, bool testarExtremidades, TBarraGenerica barraTeste, ref List<vec3> pontos, bool testaApoio = false, bool testaIntersec3D = false)
        {
            List<TBarraGenerica> retorno = new List<TBarraGenerica>();

            vec3 p_1, p_2, p_3, p_4, pontoToque = new vec3(0);
            vec3 p_r = new vec3(0, 0, 0);
            vec3 ponto = new vec3(0, 0, 0);
            TBarraGenerica b1;
            List<TBarraGenerica> elementos = _elementos.ToList();

            p_1 = new vec3(barraTeste.pIni.x, barraTeste.pIni.y, barraTeste.pIni.z);
            p_2 = new vec3(barraTeste.pFin.x, barraTeste.pFin.y, barraTeste.pFin.z);
            double tt = 0, uu = 0;
            if (testaApoio && testarExtremidades)
            {
                foreach (TApoio a in apoios)
                {
                    if (Geom.Iguais(barraTeste.pIni.x, a.pIni.x) &&
                       Geom.Iguais(barraTeste.pIni.y, a.pIni.y) &&
                       Geom.Iguais(barraTeste.pIni.z, a.pIni.z))
                        pontos.Add(new vec3(a.pIni.x, a.pIni.y, a.pIni.z));

                    if (Geom.Iguais(barraTeste.pFin.x, a.pIni.x) &&
                       Geom.Iguais(barraTeste.pFin.y, a.pIni.y) &&
                       Geom.Iguais(barraTeste.pFin.z, a.pIni.z))
                        pontos.Add(new vec3(a.pIni.x, a.pIni.y, a.pIni.z));
                }
            }

            if (elementos != null)
                if (elementos.Count > 1)
                {
                    for (int i = 0; i < elementos.Count; i++)
                    {
                        b1 = elementos[i];

                        if (b1.IDBarra == barraTeste.IDBarra) continue;

                        p_3 = new vec3(b1.pIni.x, b1.pIni.y, b1.pIni.z);
                        p_4 = new vec3(b1.pFin.x, b1.pFin.y, b1.pFin.z);

                        if (Geom.PontoTocaAresta(p_1, p_3, p_4, ref pontoToque, testarExtremidades))
                        {
                            if (!retorno.Exists(o => o.IDBarra == b1.IDBarra))
                            {
                                retorno.Add(b1);
                                pontos.Add(new vec3(pontoToque.x, pontoToque.y, pontoToque.z));
                            }
                        }
                        else
                        if (Geom.PontoTocaAresta(p_2, p_3, p_4, ref pontoToque, testarExtremidades))
                        {
                            if (!retorno.Exists(o => o.IDBarra == b1.IDBarra))
                            {
                                retorno.Add(b1);
                                pontos.Add(new vec3(pontoToque.x, pontoToque.y, pontoToque.z));
                            }
                        }

                        if (Geom.PontoTocaAresta(p_3, p_1, p_2, ref pontoToque, testarExtremidades))
                        {
                            if (!retorno.Exists(o => o.IDBarra == b1.IDBarra))
                            {
                                retorno.Add(b1);
                                pontos.Add(new vec3(pontoToque.x, pontoToque.y, pontoToque.z));
                            }
                        }
                        else
                        if (Geom.PontoTocaAresta(p_4, p_1, p_2, ref pontoToque, testarExtremidades))
                        {
                            if (!retorno.Exists(o => o.IDBarra == b1.IDBarra))
                            {
                                retorno.Add(b1);
                                pontos.Add(new vec3(pontoToque.x, pontoToque.y, pontoToque.z));
                            }
                        }

                        if (testaIntersec3D)
                        if (Geom.TemInterseccao3D(ref p_1, ref p_2, ref p_3, ref p_4, ref ponto, ref tt, ref uu))
                        {
                            if (LocalizaPonto(ref pontos, ponto.x, ponto.y, ponto.z) ==-1)
                              if (!retorno.Exists(o => o.IDBarra == b1.IDBarra))
                              {
                                  retorno.Add(b1);
                                  pontos.Add(new vec3(ponto.x, ponto.y, ponto.z));
                              }
                        }
                    }
                }
     
            return retorno;
        }

        public static List<TObjetoDesenho> RetornaConexoes(ref List<TObjetoDesenho> _elementos)
        {
            List<TObjetoDesenho> retorno = new List<TObjetoDesenho>();

            vec3 p_1, p_2, p_3, p_4;
            vec3 p_r = new vec3(0, 0, 0);
            vec3 ponto = new vec3(0, 0, 0);
            TBarraGenerica BarraDividir, BarraConectada, b1, b2;

            BarraDividir = null;
            BarraConectada = null;

            BarrasDividir = new List<TBarraGenerica>();
            Divisoes = new List<BarrasXPonto_Divisao>();
            TPonto PontoBarra = null;
            List<TObjetoDesenho> elementos = new List<TObjetoDesenho>();
            elementos = _elementos.ToList();

            BarrasXPonto_Divisao div;
            bool temconexao = false;
            int j;
            if (elementos != null)
                if (elementos.Count > 1)
                {
                    for (int i = 0; i < elementos.Count; i++)
                    {
                        if (elementos[i].Tipo == Const.ID_BARRAGENERICA && elementos[i].Selecionado)
                        {
                            b1 = (TBarraGenerica)elementos[i];
                            if (b1 == null) continue;

                            p_1 = new vec3(b1.pIni.x, b1.pIni.y, b1.pIni.z );
                            p_2 = new vec3(b1.pFin.x, b1.pFin.y , b1.pFin.z );

                            temconexao = false;

                            for (j = 0; j < elementos.Count; j++)
                            {
                                b2 = (TBarraGenerica)elementos[j];
                                if (b2 == null) continue;

                                if ((Object)b1 != (Object)b2)
                                {

                                  /*  if (b1.IDBarra == 2 && b2.IDBarra == 7)
                                    {
                                        b1.comprimento *= 1;
                                        b2.comprimento *= 1;
                                        b1.IDBarra = 2;
                                    }*/

                                    p_3 = new vec3(b2.pIni.x, b2.pIni.y , b2.pIni.z);
                                    p_4 = new vec3(b2.pFin.x, b2.pFin.y , b2.pFin.z );

                                 //   resposta = Geom.Intersec3D(ref p_1, ref p_2, ref p_3, ref p_4, ref ponto, ref tt, ref uu);

                                    if  (Geom.DuasArestasSeTocam(ref p_1, ref p_2, ref p_3, ref p_4, ref ponto, true))

                                    //if (resposta !=99999)
                                  //  if (resposta == 0 || resposta == 1) // se der 0 ou 1 é pq tem intersecao no extremo da barra, nao no meio
                                    {
                                        //if (tt == 0 && uu == 0)
                                         //   continue; // interseção em ponto extremo, nao é intersecao 

                                        BarraDividir = null;
                                        BarraConectada = null;

                                        if (Geom.Iguais(b1.pIni.x, ponto.x, Const.Tol) && Geom.Iguais(b1.pIni.y, ponto.y, Const.Tol) && Geom.Iguais(b1.pIni.z, ponto.z, Const.Tol))
                                        {
                                            BarraDividir = b2;
                                            BarraConectada = b1;
                                            PontoBarra = b1.pIni;
                                        }

                                        if (Geom.Iguais(b1.pFin.x, ponto.x, Const.Tol) && Geom.Iguais(b1.pFin.y, ponto.y, Const.Tol) && Geom.Iguais(b1.pFin.z, ponto.z, Const.Tol))
                                        {
                                            BarraDividir = b2;
                                            BarraConectada = b1;
                                            PontoBarra = b1.pFin;
                                        }

                                        if (Geom.Iguais(b2.pIni.x, ponto.x, Const.Tol) && Geom.Iguais(b2.pIni.y, ponto.y, Const.Tol) && Geom.Iguais(b2.pIni.z, ponto.z, Const.Tol))
                                        {
                                            BarraDividir = b1;
                                            BarraConectada = b2;
                                            PontoBarra = b2.pIni;
                                        }

                                        if (Geom.Iguais(b2.pFin.x, ponto.x, Const.Tol) && Geom.Iguais(b2.pFin.y, ponto.y, Const.Tol) && Geom.Iguais(b2.pFin.z, ponto.z, Const.Tol))
                                        {
                                            BarraDividir = b1;
                                            BarraConectada = b2;
                                            PontoBarra = b2.pFin;
                                        }

                                        if (BarraDividir != null)
                                         //   if (NaoEstaConectadaEmPontoExtremo(ref BarraDividir, ref BarraConectada))
                                                if (!EmListaParaDividir(ref BarraDividir, PontoBarra.x, PontoBarra.y, PontoBarra.z))
                                                {
                                                    temconexao = true;
                                                    if (BarraConectada.IDBarra != b1.IDBarra)
                                                      retorno.Add(BarraConectada);

                                                    if (BarraDividir.IDBarra != b1.IDBarra)
                                                      retorno.Add(BarraDividir);
                                                //  break;
                                            }
                                    }
                                }
                            }

                            if (temconexao)
                                break;
                        }
                    }
                }
/*
            if (temconexao)
            {
                div = new BarrasXPonto_Divisao();
                
                retorno.Add(BarraDividir);
                */
              /*  div.ponto = ponto;
                div.BarraDividir = (TBarraGenerica)BarraDividir;
                div.BarraDividir.IDBarra = BarraDividir.IDBarra;
                div.BarraConectada = BarraConectada;
                div.PontoBarra = PontoBarra;

                pIni = (TPonto)div.PontoBarra.Clone();
                pFin = (TPonto)div.BarraDividir.pFin.Clone();

                div.BarraDividir.pFin = div.PontoBarra;
                div.BarraDividir.Linha_Eixo.pFin = div.BarraDividir.pFin;
                div.BarraDividir.Linha_Eixo.pIni = div.BarraDividir.pIni;

                div.BarraDividir.comprimento = (float)div.BarraDividir.pFin.DistanceTo(div.BarraDividir.pIni);

                barranova = new TBarraGenerica(pIni, pFin, div.BarraDividir.layer, (TDadosBarra)div.BarraDividir.Dados.Clone(), -1);

                barranova.comprimento = (float)barranova.pFin.DistanceTo(barranova.pIni);
                barranova.IDBarra = div.BarraDividir.IDBarra;
                barranova.Linha_Eixo.pIni = barranova.pIni;
                barranova.Linha_Eixo.pFin = barranova.pFin;

                barrasModelo.Add(barranova);*/

         //   }

            return retorno;
        }
        static double dist;
        public static bool AvaliaPontoElementoSobreposto(vec3 pontoAvaliar, TBarraGenerica b1, bool testaExtremidade)
        {
            vec3 p_1, p_2;            

            p_1 = new vec3(b1.pIni_Offset.x, b1.pIni_Offset.y, b1.pIni_Offset.z);
            p_2 = new vec3(b1.pFin_Offset.x, b1.pFin_Offset.y, b1.pFin_Offset.z);

            if (testaExtremidade)
            {
                dist = p_1.DistanceTo(pontoAvaliar);
                if (Geom.Iguais(dist, 0, 0.001))
                    return true;

                dist = p_2.DistanceTo(pontoAvaliar);
                if (Geom.Iguais(dist, 0, 0.001))
                    return true;
            }

            if (Geom.Iguais(p_1.x, pontoAvaliar.x, 0.001) && Geom.Iguais(p_1.y, pontoAvaliar.y, 0.001) && Geom.Iguais(p_1.z, pontoAvaliar.z, 0.001))
                return false;

            if (Geom.TocaNoMeio(pontoAvaliar, p_1, p_2))
            {
               dist = Geom.DistanciaPerpendicular(p_1, p_2, pontoAvaliar);
               if (Geom.Iguais(dist, 0, 0.001))
                 return true;
            }


            
            return false;
        }

        public static List<TBarraGenerica> RetornaElementosSobrepostos(ref List<TBarraGenerica> _elementos)
        {
            List<TBarraGenerica> retorno = new List<TBarraGenerica>();
            TBarraGenerica b1;
            vec3 pontoAvaliar1, pontoAvaliar2;  

            if (_elementos != null)
                for (int i = 0; i < _elementos.Count; i++)
                {
                    if (_elementos[i].Dados.Tipo == 4)
                            continue;
                    
                    pontoAvaliar1 = new vec3(_elementos[i].pIni_Offset.x, _elementos[i].pIni_Offset.y, _elementos[i].pIni_Offset.z);
                    pontoAvaliar2 = new vec3(_elementos[i].pFin_Offset.x, _elementos[i].pFin_Offset.y, _elementos[i].pFin_Offset.z);

                    for (int jj = 0; jj < _elementos.Count; jj++)
                    {
                        b1 = _elementos[jj];

                        if (b1.IDBarra == _elementos[i].IDBarra) 
                           continue;

                        if (AvaliaPontoElementoSobreposto(pontoAvaliar1, b1, true) && AvaliaPontoElementoSobreposto(pontoAvaliar2, b1, true))
                            retorno.Add(_elementos[i]);
                        else
                        if (AvaliaPontoElementoSobreposto(pontoAvaliar1, b1, false))
                        {
                            if ((Geom.Iguais((b1.angXY), (_elementos[i].angXY)) && Geom.Iguais((b1.angXZ), (_elementos[i].angXZ))))
                                retorno.Add(_elementos[i]);
                        }
                        else
                        if (AvaliaPontoElementoSobreposto(pontoAvaliar2, b1, false))
                        {
                            if ((Geom.Iguais((b1.angXY), (_elementos[i].angXY)) && Geom.Iguais((b1.angXZ), (_elementos[i].angXZ))))
                                retorno.Add(_elementos[i]);
                        }
                    }
                }

            return retorno;
        }
        public static List<TPonto> ConectarNosPerdidos(ref List<TPonto> _nos, ref List<TBarraGenerica> _elementos, double tolerancia)
        {
            List<TPonto> retorno = new List<TPonto>();
            List<TPonto> nos;
            TBarraGenerica b1;
            nos = _nos.ToList();
            vec3 p_1, p_2, pontoAvaliar;
            double dist;
            tolerancia /= 1000;

            int j;
            if (nos != null)
                for (int i = 0; i < nos.Count; i++)
                {
                    pontoAvaliar = new vec3(nos[i].x, nos[i].y, nos[i].z);

                    for (int jj = 0; jj < _elementos.Count; jj++)
                    {
                        {
                            b1 = _elementos[jj];

                            p_1 = new vec3(b1.pIni.x, b1.pIni.y, b1.pIni.z);
                            p_2 = new vec3(b1.pFin.x, b1.pFin.y, b1.pFin.z);

                            if (nos[i] != b1.pIni && nos[i] != b1.pFin)
                                if (Geom.TocaNoMeio(pontoAvaliar, p_1, p_2))
                                    if (!Geom.Iguais(Geom.DistanciaPerpendicular(p_1, p_2, pontoAvaliar), 0, 0.0001))
                                    {
                                        dist = Geom.DistanciaPerpendicular(p_1, p_2, pontoAvaliar);
                                        if (Geom.Iguais(dist, tolerancia, 0.001) || (dist < tolerancia))
                                        {
                                            vec3 perpendicular = Geom.PontoPerpendicular(p_1, p_2, pontoAvaliar);
                                            nos[i].x = perpendicular.x;
                                            nos[i].y = perpendicular.y;
                                            nos[i].z = perpendicular.z;

                                            //      retorno.Add(nos[i]);
                                        }
                                    }

                            dist = p_1.DistanceTo(pontoAvaliar);
                            if (!Geom.Iguais(dist, 0, 0.00001))
                                if (Geom.Iguais(dist, tolerancia, 0.001) || (dist < tolerancia))
                                {
                                  //  retorno.Add(nos[i]);
                                    b1.pIni.x = pontoAvaliar.x;
                                    b1.pIni.y = pontoAvaliar.y;
                                    b1.pIni.z = pontoAvaliar.z;
                                    b1.Linha_Eixo.pIni.x = pontoAvaliar.x;
                                    b1.Linha_Eixo.pIni.y = pontoAvaliar.y;
                                    b1.Linha_Eixo.pIni.z = pontoAvaliar.z;
                                }

                            dist = p_2.DistanceTo(pontoAvaliar);
                            if (!Geom.Iguais(dist, 0, 0.00001))
                                if (Geom.Iguais(dist, tolerancia, 0.001) || (dist < tolerancia))
                                {
                                    b1.pFin.x = pontoAvaliar.x;
                                    b1.pFin.y = pontoAvaliar.y;
                                    b1.pFin.z = pontoAvaliar.z;
                                    b1.Linha_Eixo.pFin.x = pontoAvaliar.x;
                                    b1.Linha_Eixo.pFin.y = pontoAvaliar.y;
                                    b1.Linha_Eixo.pFin.z = pontoAvaliar.z;
                                    //           retorno.Add(nos[i]);
                                }
                        }
                    }
                }

            return retorno;
        }

        public static List<TPonto> RetornaNosProximos(ref List<TPonto> _nos, ref List<TBarraGenerica> _elementos, double tolerancia)
        {
                List<TPonto> retorno = new List<TPonto>();
                List<TPonto> nos;
                TBarraGenerica b1;
                nos = _nos.ToList();
                vec3 p_1, p_2, pontoAvaliar;
                double dist;
                tolerancia /= 1000;

                int j;
                if (nos != null)
                    for (int i = 0; i < nos.Count; i++)
                    {
                        pontoAvaliar = new vec3(nos[i].x, nos[i].y, nos[i].z);

                        for (int jj = 0; jj < _elementos.Count; jj++)
                        {
                            {
                                b1 = _elementos[jj];

                                p_1 = new vec3(b1.pIni.x, b1.pIni.y, b1.pIni.z);
                                p_2 = new vec3(b1.pFin.x, b1.pFin.y, b1.pFin.z);
                                 
                                if (nos[i] != b1.pIni && nos[i] != b1.pFin)
                                  if (Geom.TocaNoMeio(pontoAvaliar, p_1, p_2))
                                  if (!Geom.Iguais(Geom.DistanciaPerpendicular(p_1, p_2, pontoAvaliar), 0,0.0001))
                                  {
                                    dist = Geom.DistanciaPerpendicular(p_1, p_2, pontoAvaliar);
                                    if (Geom.Iguais(dist, tolerancia, 0.001) || (dist < tolerancia))
                                    {
                                        retorno.Add(nos[i]);
                                    }
                                  }

                               dist = p_1.DistanceTo(pontoAvaliar);
                               if (!Geom.Iguais(dist, 0, 0.00001))
                                 if (Geom.Iguais(dist, tolerancia, 0.001) || (dist < tolerancia))
                                 {
                                    retorno.Add(nos[i]);
                                 }

                               dist = p_2.DistanceTo(pontoAvaliar);
                               if (!Geom.Iguais(dist, 0, 0.00001))
                                if (Geom.Iguais(dist, tolerancia, 0.001) || (dist < tolerancia))
                                {
                                    retorno.Add(nos[i]);
                                }
                        }
                        }
                    }

                return retorno;
        }

        public static void CalculaIntersecoesBarras(List<TBarraGenerica> barras, List<TCargaLinear> cargalinear, ref List<TLinha> linhas, FPrincipal desenho, bool MouseEdit)
        {
            double resposta;
     
            vec3 p_1, p_2, p_3, p_4;
            vec3 p_r = new vec3(0, 0, 0);
            vec3 ponto = new vec3(0, 0, 0);
            TBarraGenerica BarraDividir,BarraConectada, b1, b2;
            BarrasDividir = new List<TBarraGenerica>();
            Divisoes = new List<BarrasXPonto_Divisao>();
            TPonto PontoBarra = null;
            double tt = 0, uu = 0;
            List<TBarraGenerica> bars = new List<TBarraGenerica>();
            bars = barras.ToList();

          //  if (bars.Count > 100)
             // desenho.ChamaProgresso("Atualizando interseções das barras. Aguarde...", bars.Count);
            
            if (!MouseEdit) // se nao for mouseedit entao eh mousedrawing, 
            {
                bars.Clear();
                if (desenho.LinhaPonto1 != null)
                    bars.Add(desenho.LinhaPonto1.Barra as TBarraGenerica);
                if (desenho.LinhaPonto3 != null)
                    bars.Add(desenho.LinhaPonto3.Barra as TBarraGenerica);
                if (desenho.LinhaInserida != null)
                    bars.Add(desenho.LinhaInserida.Barra as TBarraGenerica);
            }

            int j;
            //else mouseedit, entao uso toda a lista de barras
            if (bars != null)
            if (bars.Count > 1)
            {
                for (int i = 0; i < bars.Count; i++)
                {
                   // desenho.Progresso.Increment(1);
                    b1 = bars[i];
                    if (b1 == null) continue;
                    //       desenho.Progresso.Refresh();
                    // System.Windows.Forms.Application.DoEvents();
                  //  b1 = desenho.LinhaPonto1.Barra as TBarraGenerica;

                    p_1 = new vec3(b1.pIni.x, b1.pIni.y, b1.pIni.z);
                    p_2 = new vec3(b1.pFin.x, b1.pFin.y, b1.pFin.z);

                    for (j = 0; j < bars.Count; j++)
                   {
                       b2 = bars[j];
                       if (b2 == null) continue;

                        if ((Object)b1 != (Object)b2)
                        {
                            p_3 = new vec3(b2.pIni.x, b2.pIni.y, b2.pIni.z);
                            p_4 = new vec3(b2.pFin.x, b2.pFin.y, b2.pFin.z);

                            resposta = Geom.Intersec3D(ref p_1, ref p_2, ref p_3, ref p_4, ref ponto, ref tt, ref uu);

                            if (resposta == 0 || resposta == 1) // se der 0 o 1 é pq tem intersecao no extremo da barra, nao no meio
                            {
                                if (tt == 0 && uu == 0) 
                                    continue; // interseção em ponto extremo, nao é intersecao 

                                BarraDividir = null;
                                BarraConectada = null;

                                if (Geom.Iguais(b1.pIni.x, ponto.x) && Geom.Iguais(b1.pIni.y, ponto.y) && Geom.Iguais(b1.pIni.z, ponto.z))
                                {
                                    BarraDividir = b2;
                                    BarraConectada = b1;
                                    PontoBarra = b1.pIni;
                                }

                                if (Geom.Iguais(b1.pFin.x, ponto.x) && Geom.Iguais(b1.pFin.y, ponto.y) && Geom.Iguais(b1.pFin.z, ponto.z))
                                {
                                    BarraDividir = b2;
                                    BarraConectada = b1;
                                    PontoBarra = b1.pFin;
                                }

                                if (Geom.Iguais(b2.pIni.x, ponto.x) && Geom.Iguais(b2.pIni.y, ponto.y) && Geom.Iguais(b2.pIni.z, ponto.z))
                                {
                                    BarraDividir = b1;
                                    BarraConectada = b2;
                                    PontoBarra = b2.pIni;
                                }

                                if (Geom.Iguais(b2.pFin.x, ponto.x) && Geom.Iguais(b2.pFin.y, ponto.y) && Geom.Iguais(b2.pFin.z, ponto.z))
                                {
                                    BarraDividir = b1;
                                    BarraConectada = b2;
                                    PontoBarra = b2.pFin;
                                }

                                if ((Object)BarraDividir != null)
                                    if (NaoEstaConectadaEmPontoExtremo(ref BarraDividir, ref BarraConectada))
                                        if (!EmListaParaDividir(ref BarraDividir, PontoBarra.x, PontoBarra.y, PontoBarra.z))
                                        {
                                            //         BarraDividir.Linha_Eixo.SetaSelecao(true,true);
                                            BarrasDividir.Add(BarraDividir);

                                            BarrasXPonto_Divisao div = new BarrasXPonto_Divisao();
                                            div.ponto = ponto;
                                            div.BarraDividir = BarraDividir;
                                            div.BarraConectada = BarraConectada;
                                            div.PontoBarra = PontoBarra;
                                            Divisoes.Add(div);
                                        }
                            }
                        }
                   }
                } 
                
            }

         
          /*  for (int i = 0; i < barras.Count; i++)
			{
                desenho.Progresso.Increment(1);
   
         //       desenho.Progresso.Refresh();
               // System.Windows.Forms.Application.DoEvents();
                b1 = barras[i];

                p_1 = new vec3(b1.pIni.x, b1.pIni.y, b1.pIni.z);
                p_2 = new vec3(b1.pFin.x, b1.pFin.y, b1.pFin.z);

                for (int j = 0; j < barras.Count; j++)
                {
                    b2 = barras[j];

                    if ((Object)b1 != (Object)b2)
                    {
                        p_3 = new vec3(b2.pIni.x, b2.pIni.y, b2.pIni.z);
                        p_4 = new vec3(b2.pFin.x, b2.pFin.y, b2.pFin.z);

                        resposta = Geom.Intersec3D(ref p_1, ref p_2, ref p_3, ref p_4, ref ponto, ref tt, ref uu);

                        if (resposta == 0 || resposta == 1) // se der 0 o 1 é pq tem intersecao no extremo da barra, nao no meio
                        {
                            if (tt == 0 && uu == 0) continue; // interseção em ponto extremo, nao é intersecao 

                            BarraDividir = null;
                            BarraConectada = null;

                            if (Geom.Iguais(b1.pIni.x, ponto.x) && Geom.Iguais(b1.pIni.y, ponto.y) && Geom.Iguais(b1.pIni.z, ponto.z))
                            {
                              BarraDividir = b2;
                              BarraConectada = b1;
                              PontoBarra = b1.pIni;
                            }
                    
                            if (Geom.Iguais(b1.pFin.x, ponto.x) && Geom.Iguais(b1.pFin.y, ponto.y) && Geom.Iguais(b1.pFin.z, ponto.z))
                            {
                                BarraDividir = b2;
                                BarraConectada = b1;
                                PontoBarra = b1.pFin;
                            }
            
                            if (Geom.Iguais(b2.pIni.x, ponto.x) && Geom.Iguais(b2.pIni.y, ponto.y) && Geom.Iguais(b2.pIni.z, ponto.z))
                            {
                                BarraDividir = b1;
                                BarraConectada = b2;
                                PontoBarra = b2.pIni;
                            }
                      
                            if (Geom.Iguais(b2.pFin.x, ponto.x) && Geom.Iguais(b2.pFin.y, ponto.y) && Geom.Iguais(b2.pFin.z, ponto.z))
                            {
                                BarraDividir = b1;
                                BarraConectada = b2;
                                PontoBarra = b2.pFin;
                            }

                            if ((Object)BarraDividir != null)
                                if (NaoEstaConectadaEmPontoExtremo(ref BarraDividir, ref BarraConectada))
                                  if (!EmListaParaDividir(ref BarraDividir, PontoBarra.x, PontoBarra.y, PontoBarra.z))
                                  {
                             //         BarraDividir.Linha_Eixo.SetaSelecao(true,true);
                                      BarrasDividir.Add(BarraDividir);
                                      
                                      BarrasXPonto_Divisao div = new BarrasXPonto_Divisao();
                                      div.ponto = ponto;
                                      div.BarraDividir   = BarraDividir;
                                      div.BarraConectada = BarraConectada;
                                      div.PontoBarra = PontoBarra;
                                      Divisoes.Add(div);
                                  }
                        }
                    }
                }
            }*/
            TBarraGenerica barranova = null;
            TPonto pIni = null;
            TPonto pFin = null;
            TPonto pa, pb;
            desenho.ObjetosMovidos = new List<TObjetoDesenho>();
            

            double xf = 0, yf = 0, zf = 0;
            foreach (BarrasXPonto_Divisao divisao in Divisoes)
            {
                xf = divisao.BarraDividir.pFin.x;
                yf = divisao.BarraDividir.pFin.y;
                zf = divisao.BarraDividir.pFin.z;

                pIni = (TPonto)divisao.PontoBarra.Clone();
                pFin = (TPonto)divisao.BarraDividir.pFin.Clone();
                
                pa = (TPonto)divisao.BarraDividir.pFin.Clone();
                
                divisao.BarraDividir.pFin = divisao.PontoBarra;
                divisao.BarraDividir.Linha_Eixo.pFin = divisao.BarraDividir.pFin;
                divisao.BarraDividir.Linha_Eixo.pIni = divisao.BarraDividir.pIni;
               
                pb = (TPonto)divisao.PontoBarra.Clone();

                divisao.BarraDividir.comprimento = (float)divisao.BarraDividir.pFin.DistanceTo(divisao.BarraDividir.pIni);
                divisao.BarraDividir.OrientaSecaoNoEspaco();

                desenho.ObjetosMovidos.Add(divisao.BarraDividir.pFin);

               // desenho.undoBuffer.AdicionaComando(new ComandoMover(desenho.ObjetosMovidos,pa, pb));
               
                desenho.comandosMover.Add(new ComandoMover(divisao.BarraDividir, "pFin", pa, pb));

                barranova = new TBarraGenerica(pIni,pFin,divisao.BarraDividir.layer,(TDadosBarra)divisao.BarraDividir.Dados.Clone(),-1);
              /*  TTrechoViga New = new TTrechoViga((TPonto)(objeto as TTrechoViga).pIni.Clone(),
                                                  (TPonto)(objeto as TTrechoViga).pFin.Clone(),
                                                  ((TDadosViga)(objeto as TTrechoViga).Dados.Clone()),
                                                  objeto.layer, Estrutura, (objeto as TTrechoViga).Pavimento, Linhas, true);*/

                barranova.comprimento = (float)barranova.pFin.DistanceTo(barranova.pIni);

                barranova.Linha_Eixo.pIni = barranova.pIni;
                barranova.Linha_Eixo.pFin = barranova.pFin;

                barranova.OrientaSecaoNoEspaco();
            //    barranova.CriaPesoProprio();

             //   foreach (TLinha l in linhas)
             ///       if ((Object)l == (Object)divisao.BarraDividir.Linha_Eixo)
           //             l.SetaSelecao(true, true);
                barranova.CriaPesoProprio();

                desenho.AdicionaBarraGenerica(barranova);
                desenho.ObjetosAdicionados.Add(barranova);
//                barranova.CriaPesoProprio();
 //               desenho.AdicionaObjeto(desenho.Estrutura.barras[desenho.Estrutura.barras.Count - 1].PesoProprio as TCargaLinear, -1);
            /*    for (int i = 0; i < cargalinear.Count; i++)
                {
                    if (cargalinear[i].idBarra == barranova.IDBarra && cargalinear[i].Dados.idCaso == -1)
                    {
                        desenho.Estrutura.cargaLinear[i].anguloRotacao = barranova.anguloRotacao;
                        desenho.Estrutura.cargaLinear[i].RotacionaDiagramaCarga(true, 0, 0, desenho.fatorCarga);
                    }
                }*/
                List<TCargaLinear> cl = cargalinear.FindAll(c => c.idBarra == divisao.BarraDividir.IDBarra);
                foreach (TCargaLinear clinear in cl)
                {
                    if (clinear.Dados.idCaso != 1)
                    {
                        TCargaLinear carganova = new TCargaLinear(barranova.pIni, barranova.pFin, (TDadosCarga)clinear.Dados.Clone(), 
                                                                  barranova.Dados.anguloRotacao,barranova.IDBarra, clinear.layer);
                        desenho.AdicionaObjeto(carganova,-1, true, false);
                        desenho.Estrutura.cargaLinear[desenho.Estrutura.cargaLinear.Count - 1].CriaSetas(true, 0, 0, desenho.fatorCarga);
                        desenho.ObjetosAdicionados.Add(carganova);
                    }
                }

                if (cl != null)
                {
                 /*   if (cl.Dados.idCaso != -1)
                    {
                        desenho.AdicionaObjeto(desenho.Estrutura.barras[desenho.Estrutura.barras.Count - 1].PesoProprio as TCargaLinear, -1);

                        
                        desenho.Settings = (TDadosCarga)cl.Dados.Clone();
                        desenho.IdObjetoDesenho = Const.ID_CARGA_LINEAR;
                        desenho.MouseDrawing(barranova.pIni.x, barranova.pIni.y, barranova.pIni.z);
                        desenho.MouseDrawing(barranova.pFin.x, barranova.pFin.y, barranova.pFin.z);

                        desenho.Estrutura.cargaLinear[desenho.Estrutura.cargaLinear.Count - 1].idBarra = barranova.IDBarra;
                        desenho.Estrutura.cargaLinear[desenho.Estrutura.cargaLinear.Count - 1].Dados = (TDadosCarga)cl.Dados.Clone();
                        desenho.Estrutura.cargaLinear[desenho.Estrutura.cargaLinear.Count - 1].anguloRotacao = barranova.Dados.anguloRotacao;
                        desenho.Estrutura.cargaLinear[desenho.Estrutura.cargaLinear.Count - 1].RotacionaDiagramaCarga(true, 0, 0, desenho.fatorCarga);
                    }*/
                  //  desenho.AdicionaObjeto(cl as TCargaLinear,-1);

                }
                            // desenho.MouseDrawing();
            }
            if (desenho.ObjetosMovidos.Count > 0)
            {

            }

            // desenho.RemoveSelecionados();
            //  desenho.AtualizaConfiguracoes3D();
            desenho.AtualizaBarras();
            desenho.LinhaPonto1 = null; 
            desenho.LinhaPonto3 = null;
            desenho.LinhaInserida = null;

         ///   desenho.FechaProgresso();
        }

        public static void RefazerOffsetsBarras(List<TBarraGenerica> barras_refazer,List<TBarraGenerica> barras_estrutura)
        {
            for (int i = 0; i < barras_refazer.Count; i++)
            {
                for (int j = 0; j < barras_refazer[i].ids_barras_rigidas.Count; j++)
                {
                    if (barras_estrutura.Exists(o => o.IDBarra == barras_refazer[i].ids_barras_rigidas[j]))
                    {
                        barras_estrutura.Find(o => o.IDBarra == barras_refazer[i].ids_barras_rigidas[j]).SetaSelecao(true, false);
                    }
                }
            }


            for (int i = 0; i < barras_refazer.Count; i++)
            {
                //TBarraGenerica rigida1 = new TBarraGenerica(b.pIni, p1_org, null, (TDadosBarra)b.Dados.Clone(), -1);

                TBarraGenerica br = barras_estrutura.Find(bb => bb.IDBarra == barras_refazer[i].ids_barras_rigidas[0]); //pega a primeira como referencia, ja que em todas as barras rigidas o offsets é o mesmo, nao tem diferenca de offsets 

                TPonto pt1 = new TPonto(barras_refazer[i].pIni_org.x, barras_refazer[i].pIni_org.y, barras_refazer[i].pIni_org.z);
                TPonto pt2 = new TPonto(br.pIni.x, br.pIni.y, br.pIni.z);

                //move a barra para o ponto inicial, ou seja, sem offset
                barras_refazer[i].Mover(ref pt2, ref pt1, false);

                //   CriarOffsets(barras_refazer[i]);
            }


        }
    }


}
