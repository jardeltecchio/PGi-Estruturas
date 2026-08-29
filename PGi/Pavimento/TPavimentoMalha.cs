using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;


namespace PG
{
    public struct sBarrasBase
    {
        public vec3 p1, p2;
        public bool direcaoX, direcaoY;
        public sBarrasBase(vec3 _p1, vec3 _p2, bool _direcaoX, bool _direcaoY)
        {
            this.p1 = _p1;
            this.p2 = _p2;
            this.direcaoX = _direcaoX;
            this.direcaoY = _direcaoY;
        }
    }

    public struct sMalhaBase 
    {
        public List<sBarrasBase> BarraBase;
        public TLaje Laje;
        public double espX, espY, angulo;

        public sMalhaBase(List<sBarrasBase> _BarraBase, TLaje _laje, double _espX, double _espY,double _angulo)
        {
            this.espX      = _espX;
            this.espY      = _espY;
            this.Laje      = _laje;
            this.BarraBase = _BarraBase;
            this.angulo    = _angulo;
        }
    }

    [Serializable]
    public partial class TPavimento
    {
        public int max_sNo    = 0;
        public int max_sBarra = 0;
        int jk;
        
        [NonSerialized] public FPrincipal desenho;
        [NonSerialized]
        TNoGrelha[] NoExt;

        public int LocalizaNo(ref double x, ref double y)
        {
            for (jk = 1; jk <= max_sNo; jk++)
              if ((Object)nos[jk] != null)
                if (Geom.Iguais(x, nos[jk].x,0.1) && Geom.Iguais(y, nos[jk].y,0.1))
                    return jk;

            return -1;
        }

        public bool LocalizaNo(ref TNoGrelha no)
        {
            for (jk = 1; jk <= max_sNo; jk++)
              if ((Object)nos[jk] == (Object)no)
                return true;

            return false;
        }
        [NonSerialized]
        DateTime span1, span2;

        [NonSerialized]
        public List<sBarrasBase> MalhaBase;
        [NonSerialized]
        public List<sMalhaBase> MalhasBase;
        double malha_horiz, malha_vert;
        public double maxBanda, maxBandaReordenacao;
        
        [NonSerialized]
        double xIni, yIni, xFin, yFin;
        void CriaBarrasBase(double anguloBarras, double espX, double espY, TLaje laj)
        {
            double centroY, centroX;
            double xAnt, yAnt, x_, y_;
            double fracao, quebra, divisoes_quebrada; 
            double comp_horiz, comp_vert, divisoes_inteiras_horiz, divisoes_inteiras_vert;
            double ang;
            vec3 p1, p2, centro;

            int i;

            List<sBarrasBase> BarrasBase;

            comp_horiz = xFin - xIni;
            comp_vert  = yFin - yIni;

            if (anguloBarras > 0)
            {
                comp_horiz = xFin - xIni;
                comp_vert  = yFin - yIni;

                if (comp_horiz > comp_vert)
                {
                    yFin += Math.Abs((comp_vert / 2) - (comp_horiz / 2));
                    yIni -= Math.Abs((comp_vert / 2) - (comp_horiz / 2));
                }

                if (comp_horiz < comp_vert)
                {
                    xFin += Math.Abs((comp_horiz / 2) - (comp_vert / 2));
                    xIni -= Math.Abs((comp_horiz / 2) - (comp_vert / 2));
                }

                if (comp_horiz > comp_vert)
                {
                    xIni -= (comp_horiz * .2);
                    yIni -= (comp_horiz * .2);
                    xFin += (comp_horiz * .2);
                    yFin += (comp_horiz * .2);
                }
                else
                {
                    xIni -= (comp_vert * .2);
                    yIni -= (comp_vert * .2);
                    xFin += (comp_vert * .2);
                    yFin += (comp_vert * .2);
                }

                comp_horiz = xFin - xIni;
                comp_vert = yFin - yIni;
            }

            /*----------------------------------*/
            fracao = FuncoesGerais.Frac(comp_horiz / espX);
            quebra = fracao * espX;

            divisoes_inteiras_horiz = ((comp_horiz / espX) - fracao);
            divisoes_quebrada = quebra / divisoes_inteiras_horiz;

            malha_horiz = divisoes_quebrada + espX;
            /*----------------------------------*/
            fracao = FuncoesGerais.Frac(comp_vert / espY);
            quebra = fracao * espY;

            divisoes_inteiras_vert = ((comp_vert / espY) - fracao);
            divisoes_quebrada = quebra / divisoes_inteiras_vert;

            malha_vert = divisoes_quebrada + espY;
            /*----------------------------------*/

            yAnt = yIni;
            xAnt = xIni;

            y_   = yIni;
            yAnt = y_;

            centroX = xIni + ((xFin - xIni) / 2);
            centroY = yIni + ((yFin - yIni) / 2);

            centro     = new vec3(centroX, centroY, 0);
            ang        = anguloBarras * Const.PIDiv180;
            BarrasBase = new List<sBarrasBase>();
            for (i = 0; i < divisoes_inteiras_vert + 1; i++)
            {
                if (i > 0)
                  y_ = yAnt + malha_vert;

                yAnt = y_;

                x_ = xIni;

                for (j = 0; j < divisoes_inteiras_horiz + 1; j++)
                {
                    if (j > 0)
                      x_ = xAnt + malha_horiz;

                    xAnt = x_;

                    //primeira barra - horizontal - da esquerda pra direita
                    p1 = new vec3(x_ - malha_horiz, y_, 0);
                    p2 = new vec3(x_, y_, 0);
                    p1 = p1.Rotate(centro, ang);
                    p2 = p2.Rotate(centro, ang);
                    BarrasBase.Add(new sBarrasBase(p1, p2, true, false));

                    //segunda barra - vertical - de baixo pra cima
                    p1 = new vec3(x_, y_ - malha_vert, 0);
                    p2 = new vec3(x_, y_, 0);
                    p1 = p1.Rotate(centro, ang);
                    p2 = p2.Rotate(centro, ang);
                    BarrasBase.Add(new sBarrasBase(p1, p2, false, true));
                }
            }

            sMalhaBase mb = new sMalhaBase(BarrasBase, laj, malha_horiz,malha_vert, anguloBarras);
            MalhasBase.Add(mb);
        }

        void CriaMalhasBase()
        {
            MalhasBase = new List<sMalhaBase>();
            span1 = DateTime.Now;
            double p_xini, p_xfin, p_yini, p_yfin;

            int max_linhas = linhasCount;

            int i;

            VerificaSeCancelou();           

            foreach (TLaje l in lajes)
            {
                if (l.Dados.GerarGrelhaEspecifica)
                {
                    xIni = 99999999999;
                    yIni = 99999999999;
                    xFin = -99999999999;
                    yFin = -99999999999;

                    for (i = 0; i < l.poligono.linhas_poligonal.Count; i++)
                    {
                        if (l.poligono.linhas_poligonal[i].pIni.x < xIni)
                            xIni = l.poligono.linhas_poligonal[i].pIni.x;

                        if (l.poligono.linhas_poligonal[i].pIni.y < yIni)
                            yIni = l.poligono.linhas_poligonal[i].pIni.y;

                        if (l.poligono.linhas_poligonal[i].pIni.x > xFin)
                            xFin = l.poligono.linhas_poligonal[i].pIni.x;

                        if (l.poligono.linhas_poligonal[i].pIni.y > yFin)
                            yFin = l.poligono.linhas_poligonal[i].pIni.y;
                        
                        /**/
                        
                        if (l.poligono.linhas_poligonal[i].pFin.x < xIni)
                            xIni = l.poligono.linhas_poligonal[i].pFin.x;

                        if (l.poligono.linhas_poligonal[i].pFin.y < yIni)
                            yIni = l.poligono.linhas_poligonal[i].pFin.y;

                        if (l.poligono.linhas_poligonal[i].pFin.x > xFin)
                            xFin = l.poligono.linhas_poligonal[i].pFin.x;

                        if (l.poligono.linhas_poligonal[i].pFin.y > yFin)
                            yFin = l.poligono.linhas_poligonal[i].pFin.y;
                    };

                    xIni -= 8;
                    yIni -= 8;
                    xFin += 8;
                    yFin += 8; 
                    
                    CriaBarrasBase(l.Dados.ConfiguracaGrelha.AnguloBarras,
                                   l.Dados.ConfiguracaGrelha.EspacamentoX,
                                   l.Dados.ConfiguracaGrelha.EspacamentoY, l); 
                }
            }

            xIni = 99999999999;
            yIni = 99999999999;
            xFin = -99999999999;
            yFin = -99999999999; 
            
            for (i = 0; i < pontosCount; i++)
            {
                if (pontos[i].PontoCentroidePilar)
                  pontos[i].PontoCentroidePilar = true;

                if (!pontos[i].PontoEixoViga) continue;
                if (pontos[i].PontoAuxiliar) continue;

                if (pontos[i].x < xIni)
                {
                    xIni = pontos[i].x;
                    p_xini = pontos[i].px_x;
                };

                if (pontos[i].y < yIni)
                {
                    yIni = pontos[i].y;
                    p_yini = pontos[i].px_y;
                };

                if (pontos[i].x > xFin)
                {
                    xFin = pontos[i].x;
                    p_xfin = pontos[i].px_x;
                };

                if (pontos[i].y > yFin)
                {
                    yFin = pontos[i].y;
                    p_yfin = pontos[i].px_y;
                };
            };

            xIni -= 8;
            yIni -= 8;

            xFin += 8;
            yFin += 8;
           
            CriaBarrasBase(ConfiguracaGrelha.AnguloBarras, espacamentoX, espacamentoY ,null); 
/*
            comp_horiz = xFin - xIni;
            comp_vert  = yFin - yIni;

            if (ConfiguracaGrelha.AnguloBarras > 0)
            {
                comp_horiz = xFin - xIni;
                comp_vert  = yFin - yIni;

                if (comp_horiz > comp_vert)
                {
                    yFin += Math.Abs((comp_vert / 2) - (comp_horiz / 2));
                    yIni -= Math.Abs((comp_vert / 2) - (comp_horiz / 2));
                }

                if (comp_horiz < comp_vert)
                {
                    xFin += Math.Abs((comp_horiz / 2) - (comp_vert / 2));
                    xIni -= Math.Abs((comp_horiz / 2) - (comp_vert / 2));
                }

                if (comp_horiz > comp_vert)
                {
                    xIni -= (comp_horiz * .2);
                    yIni -= (comp_horiz * .2);
                    xFin += (comp_horiz * .2);
                    yFin += (comp_horiz * .2);
                }
                else
                {
                    xIni -= (comp_vert * .2);
                    yIni -= (comp_vert * .2);
                    xFin += (comp_vert * .2);
                    yFin += (comp_vert * .2);
                }

                comp_horiz = xFin - xIni;
                comp_vert = yFin - yIni;
            }

            fracao = FuncoesGerais.Frac(comp_horiz / espacamentoX);
            quebra = fracao * espacamentoX;

            divisoes_inteiras_horiz = ((comp_horiz / espacamentoX) - fracao);
            divisoes_quebrada = quebra / divisoes_inteiras_horiz;

            malha_horiz = divisoes_quebrada + espacamentoX;
      
            fracao = FuncoesGerais.Frac(comp_vert / espacamentoY);
            quebra = fracao * espacamentoY;

            divisoes_inteiras_vert = ((comp_vert / espacamentoY) - fracao);
            divisoes_quebrada = quebra / divisoes_inteiras_vert;

            malha_vert = divisoes_quebrada + espacamentoY;
            */
        }
        sMalhaBase RetMalhaBaseLaje(TLaje laj)
        {
            foreach (sMalhaBase m in MalhasBase)
                if ((Object)m.Laje == (Object)laj)
                    return m;

            return MalhasBase[0];
        } 
        [NonSerialized]
        double Diferenca_Entre_NoI_e_NoF;
        [NonSerialized]
        int noI, noF;
        [NonSerialized]
        TNoGrelha noIni, noFin;
        [NonSerialized]
        int max_linhas;

        bool CriaBarra(ref double xI, ref double yI, ref double xF, ref double yF, TLaje laj,  bool direcaoX, bool direcaoY, bool IncideEmViga = false)
        {
            Diferenca_Entre_NoI_e_NoF = Math.Sqrt(Math.Pow(xI - xF, 2) + Math.Pow(yI - yF, 2));

            if (Geom.Iguais(Diferenca_Entre_NoI_e_NoF, 0))
            {
              //  MessageBox.Show("");
                return false;
            }

            noI = LocalizaNo(ref xI, ref yI);
            noF = LocalizaNo(ref xF, ref yF);

            if (noI == -1)
            {
                nos[++max_sNo] = new TNoGrelha(xI, yI, 0, 0, 0, max_sNo + 1, null, laj, false, true);
                noIni = nos[max_sNo];
            }
            else
                noIni = nos[noI];

            if (noF == -1)
            {
                nos[++max_sNo] = new TNoGrelha(xF, yF, 0, 0, 0, max_sNo + 1, null, laj, false, true);
                noFin = nos[max_sNo];
            }
            else
                noFin = nos[noF];

            //testa se a barra esta sobre uma barra de contorno. Aí ignora, pois o contorno é no outro códgio
            foreach (TLinha linha in this.linhas)
            {
                if (!linha.LinhaEixoViga) continue;
                if (linha.auxiliar) continue;

                if (Geom.PontoEmLinha(noIni.x, noIni.y, linha.pIni.x, linha.pIni.y, linha.pFin.x, linha.pFin.y, 0.1) &&
                   (Geom.PontoEmLinha(noFin.x, noFin.y, linha.pIni.x, linha.pIni.y, linha.pFin.x, linha.pFin.y, 0.1)))
                    return false;
            }

            barras[++max_sBarra] = (new TBarraGrelha(noIni, noFin, null,0, 0, 0, 0, 0, 0, null, false, true, direcaoX, direcaoY, max_sBarra));
            barras[max_sBarra].pIni.barrasIncidentes.Add(barras[max_sBarra]);
            barras[max_sBarra].pFin.barrasIncidentes.Add(barras[max_sBarra]);
            barras[max_sBarra].Laje = laj;
            barras[max_sBarra].IncideEmViga = IncideEmViga;
            return true;
        }

        void CriaBarrasLaje()
        {
            sMalhaBase MalhaBasePavimento = MalhasBase[MalhasBase.Count - 1];
            sMalhaBase MalhaBaseLaje;
            double Ax, Ay, x_, y_, Dx, Dy, Ex, Ey;
            double intersecx = 0, intersecy = 0;

           // gerenciador.BringToFront();
            MsgCalculo("Pavimento: " + this.Descricao, "Pavimento: " + this.Descricao + " - Gerando grelha de lajes...", (int)lajes.Count(), false, false);
       //     gerenciador.BringToFront();
            foreach (TLaje laj in lajes)
            {
               // Progresso.Increment(1);

                if (!laj.Dados.GerarGrelha) continue;

                if (laj.Dados.GerarGrelhaEspecifica)
                    MalhaBaseLaje = RetMalhaBaseLaje(laj);
                else
                    MalhaBaseLaje = MalhaBasePavimento;

                #region cria as barras
                foreach (sBarrasBase b in MalhaBaseLaje.BarraBase)
                {
                    Ax = b.p1.x;
                    Ay = b.p1.y;

                    x_ = b.p2.x;
                    y_ = b.p2.y;

                    // if (!laj.PontoEmPoligono(ref Ax, ref Ay) && !laj.PontoEmPoligono(ref x_, ref y_))
                    //    continue;

                    for (kk = 0; kk < laj.poligono_eixo.linhas_poligonal.Count; kk++)
                    {
                        if (!laj.poligono_eixo.linhas_poligonal[kk].LinhaEixoViga) continue;
                        if (laj.poligono_eixo.linhas_poligonal[kk].auxiliar) continue;

                        Dx = laj.poligono_eixo.linhas_poligonal[kk].pIni.x;
                        Dy = laj.poligono_eixo.linhas_poligonal[kk].pIni.y;
                        Ex = laj.poligono_eixo.linhas_poligonal[kk].pFin.x;
                        Ey = laj.poligono_eixo.linhas_poligonal[kk].pFin.y;
                                                
                        if (Geom.calcIntersecEQU_RETA(ref Ax, ref Ay, ref x_, ref y_, ref Dx, ref Dy, ref Ex, ref Ey, ref intersecx, ref intersecy))
                        {
                            if (!laj.PontoEmPoligono(ref Ax, ref Ay) && laj.PontoEmPoligono(ref x_, ref y_))
                            {
                                if (CriaBarra(ref intersecx, ref intersecy, ref x_, ref y_, laj, b.direcaoX, b.direcaoY, true))
                                    break;
                            }
                            else
                            if (laj.PontoEmPoligono(ref Ax, ref Ay) && !laj.PontoEmPoligono(ref x_, ref y_))
                            {
                                if (CriaBarra(ref Ax, ref Ay, ref intersecx, ref intersecy, laj, b.direcaoX, b.direcaoY, true))
                                    break;
                            }
                        }
                        else
                        if (laj.PontoEmPoligono(ref Ax, ref Ay) && laj.PontoEmPoligono(ref x_, ref y_))
                        {
                            if (CriaBarra(ref Ax, ref Ay, ref x_, ref y_, laj, b.direcaoX, b.direcaoY))
                                break;
                        };
                    }
                };
                #endregion
            }
        }

        void CriaNosExtremidade()
        {
            /* --- Rotina para criar os nós de grelha nas extremidade das arestas ---*/
           // Progresso.Value = 0;
            int nn;
            foreach (TLinha linha in this.linhas)
            {
                if (linha.barraRigida) continue;

                if (!linha.LinhaEixoViga) continue;
                if (linha.auxiliar) continue;

                if (Geom.Iguais(linha.pFin.x, linha.pIni.x, 0.1)) //linha vertical...
                    for (j = 1; j <= max_sBarra; j++)
                        if ((Object)barras[j] != null)
                            if (Geom.Iguais(linha.pIni.x, barras[j].pIni.x, 0.1) && Geom.Iguais(barras[j].pIni.x, barras[j].pFin.x, 0.1))
                                barras[j] = null;

                if (Geom.Iguais(linha.pFin.y, linha.pIni.y, 0.1)) //linha horizontal...
                    for (j = 1; j <= max_sBarra; j++)
                        if ((Object)barras[j] != null)
                            if (Geom.Iguais(linha.pIni.y, barras[j].pIni.y, 0.1f) && Geom.Iguais(barras[j].pIni.y, barras[j].pFin.y, 0.1))
                                barras[j] = null;

                nn = LocalizaNo(ref linha.pIni.x, ref linha.pIni.y);

                if (nn == -1)
                    nos[++max_sNo] = (new TNoGrelha(linha.pIni.x, linha.pIni.y, linha.pIni.px_x, linha.pIni.px_y, 0, max_sNo + 1, linha.TrechoViga, null, true, true));
                else
                    nos[nn].vertice = true;

                nn = LocalizaNo(ref linha.pFin.x, ref linha.pFin.y);

                if (nn == -1)
                    nos[++max_sNo] = (new TNoGrelha(linha.pFin.x, linha.pFin.y, linha.pFin.px_x, linha.pFin.px_y, 0, max_sNo + 1, linha.TrechoViga, null, true, true));
                else
                    nos[nn].vertice = true;
            };
        }

        void CriaBarrasViga()
        {
            TNoGrelha[] NoExt = new TNoGrelha[200];

            MsgCalculo("Pavimento: " + this.Descricao, "Pavimento: " + this.Descricao + " - Gerando grelha de vigas...", this.linhas.Count() + max_sNo, false, false);

            VerificaSeCancelou();
           
            foreach (TLinha linha in this.linhas)
                CriarBarrasContorno(linha);
          
            for (int p = 1; p <= max_sNo; p++)
                nos[p].UpdatePixel();
        }

        void Finaliza()
        {
            maxBanda = 0;
            int nbi;

            TBarraGrelha[] barrasAux = new TBarraGrelha[50000];
            TNoGrelha[] nosAux = new TNoGrelha[50000];

            int cb = 0, cn = 0;

            for (int i = 1; i <= max_sBarra; i++)
                if ((Object)barras[i] != null)
                    barrasAux[++cb] = barras[i];

            for (int i = 1; i <= max_sNo; i++)
                if ((Object)nos[i] != null)
                    nosAux[++cn] = nos[i];

            //reinicia contagem e insere novamente as barras, pois pode haver barra com valor nulo na lista
            max_sBarra = 0;
            max_sNo = 0;

            barras = new TBarraGrelha[cb + 1];
            for (int i = 1; i <= cb; i++)
                if ((Object)barrasAux[i] != null)
                    barras[++max_sBarra] = barrasAux[i];

            //reposiciona o nós e atribui o número correto, pois pode haver nós nulos na lista anterior provindos da correção da malha
            nos = new TNoGrelha[cn + 1];
            for (int i = 1; i <= cn; i++)
                if ((Object)nosAux[i] != null)
                {
                    nos[++max_sNo] = nosAux[i];
               /*     if (nos[max_sNo].TrechoViga != null)
                        if (nos[max_sNo].TrechoViga.Dados.numero == 1)
                            if (nos[max_sNo].vinculo == 0)
                           nos[max_sNo].vinculo = 2;*/
                 //   if (nos[max_sNo].vertice && nos[max_sNo].vinculo == 0)
               //         nos[max_sNo].vinculo = 1;

                    nos[max_sNo].Numero = max_sNo;
                    nos[max_sNo].s_incidencias = new List<Sincidencias>();
                }

            nBarras = max_sBarra;
            nNos    = max_sNo;

            //Atribui as características para as barras finais...
            desenho.BarraGrelha.Clear();
        //    LayersByIdPrincipal[Lay.GrelhaLajes].Obj.Clear();

            VerificaSeCancelou();
            
            MsgCalculo("Pavimento: " + this.Descricao, "Pavimento: " + this.Descricao + " - Atualizando objetos...", max_sBarra, false, false);
            
            for (int i = 1; i <= max_sBarra; i++)
            {
                if ((Object)barras[i] != null)
                {
                    barras[i].angulo = FuncoesGerais.atand((barras[i].pIni.y - barras[i].pFin.y) / (barras[i].pIni.x - barras[i].pFin.x));
                    barras[i].comprimento = (Math.Sqrt(Math.Pow(barras[i].pIni.x - barras[i].pFin.x, 2) + Math.Pow(barras[i].pIni.y - barras[i].pFin.y, 2)));
                    barras[i].L = barras[i].comprimento/100;
                    barras[i].barraVertical   = Geom.Iguais(barras[i].pIni.x, barras[i].pFin.x);
                    barras[i].barraHorizontal = Geom.Iguais(barras[i].pIni.y, barras[i].pFin.y);
                    barras[i].barraObliqua = (!barras[i].barraVertical && !barras[i].barraHorizontal);
                    barras[i].codBarra = i;
                    barras[i].Pavimento = this;
                    barras[i].layer = this.LayersByIdPrincipal[Lay.GrelhaLajes];
                    barras[i].CelulasIncidentes = new List<int>();
                    barras[i].AddGrips();
                    barras[i].pIni.s_incidencias.Add(new Sincidencias(barras[i], false));
                    barras[i].pFin.s_incidencias.Add(new Sincidencias(barras[i], true));
                   /* LayersByIdPrincipal[Lay.GrelhaLajes].AddObject(barras[i]);

                    LayersByIdPrincipal[Lay.GrelhaLajes].AddObject(barras[i].Grips[0]);
                    LayersByIdPrincipal[Lay.GrelhaLajes].AddObject(barras[i].Grips[1]);
                    LayersByIdPrincipal[Lay.GrelhaLajes].AddObject(barras[i].Grips[2]);*/

                    desenho.Grips.Add(barras[i].Grips[0]);
                    desenho.Grips.Add(barras[i].Grips[1]);
                    desenho.Grips.Add(barras[i].Grips[2]);

                    desenho.BarraGrelha.Add(barras[i]);

                    LayersByIdPrincipal[Lay.GrelhaLajes].Congelado = true;

                    nbi = 3 * (Math.Abs(barras[i].pIni.Numero - barras[i].pFin.Numero) + 1);
                    if (nbi > maxBanda)
                        maxBanda = nbi;
                }
            };

            if (!TestesManuais)
               Define_Propriedades_Geometricas_e_Molas();
            //CalculaCarregamentoBarras();

         //   CriaCelulas();

            span2 = DateTime.Now;
        }

        //calcula prop das barras (E,I,G,J) e molas dos nós
        void Define_Propriedades_Geometricas_e_Molas()
        {
            double pdi = PeDireito / 100;
            double pdf = 0;

            if (SequenciaPavimento == 1)
              pdf = 0;
            else
              pdf = gerenciador.Pavimentos[SequenciaPavimento-2].PeDireito/100;
            
            if (pdi == 0) 
              pdi = 2.8;
            
            double perTorc;
            
            for (int i = 1; i <= max_sBarra; i++)
            {
                if ((Object)barras[i] != null)
                {
                    if (barras[i].barraRigida)
                    {
                        barras[i].E = this.classePilar.ecs * 1000 ; // mpa para kpa
                        barras[i].G = this.classePilar.gc * 1000 ; // mpa para kpa
                        barras[i].I = ((barras[i].Pilar.Dados.B1 / 100) * Math.Pow(barras[i].Pilar.AlturaAcima/100, 3)) / 12 +
                                      ((barras[i].Pilar.Dados.B1 / 100) * Math.Pow(barras[i].Pilar.AlturaAbaixo / 100, 3)) / 12;
                        barras[i].J = 1;//.1;
                    }
                    else
                    if (barras[i].barraViga)
                    {
                        barras[i].E = barras[i].TrechoViga.Dados.RigidezEI > 0 ? barras[i].TrechoViga.Dados.RigidezEI: this.classeViga.ecs * 1000; // mpa para kpa
                        barras[i].I = barras[i].TrechoViga.Dados.RigidezEI > 0 ? 1: barras[i].TrechoViga.Dados.Poligono.Ixcg / 1E8; // cm4 para m4
              
                        barras[i].G = barras[i].TrechoViga.Dados.RigidezGJ > 0 ? barras[i].TrechoViga.Dados.RigidezGJ : this.classeViga.gc * 1000; // mpa para kpa
                        barras[i].J = barras[i].TrechoViga.Dados.RigidezGJ > 0 ? 1 : (barras[i].TrechoViga.Dados.h1 / 100 * (Math.Pow(barras[i].TrechoViga.Dados.b1 / 100, 3))) / 3; //(Viga_Altura * (pow(Viga_Base, 3))) / 3;
                        perTorc = (100 - (double)barras[i].TrechoViga.Dados.redTorcao)/100;
                        barras[i].G *= perTorc;// .15;
                    }
                    else
                    if (barras[i].barraLaje)
                    {
                        barras[i].E = this.classeLaje.ecs * 1000; // mpa para kpa
                        barras[i].G = this.classeLaje.gc * 1000; // mpa para kpa07
                        barras[i].I = (((espacamentoX + espacamentoY) / 100) / 2) * (Math.Pow(barras[i].Laje.Dados.h / 100, 3) / 12);// espacamento*pow(lajes[i]->espessura,3)/12;
                        barras[i].J = 2 * barras[i].I;
                        barras[i].G *= .15;
                    }
                }
            }

            /* Calcula molas     dz = e.a/l     rx = 4.e.iy/l      ry = 4.e.ix/l  */
            /* Dissertação de Barboza (ufscar) - 1992 pg.20 */
            double Ep = this.classePilar.ecs * 1000; //mpa para kpa
            for (int i = 1; i <= max_sNo; i++)
            {       
              if (nos[i].vinculo == 3 && (pdi+pdf) > 0)
              {
                  //e.a/l 
                  nos[i].K_Mola_DZ = (nos[i].Pilar.AlturaAcima == 0 ? 0 : ((Ep * (nos[i].Pilar.Dados.Poligono.area / 10000)) / (nos[i].Pilar.AlturaAcima / 100)))
                      + (nos[i].Pilar.AlturaAbaixo ==0?0:((Ep * (nos[i].Pilar.Dados.Poligono.area / 10000)) / (nos[i].Pilar.AlturaAbaixo / 100)));

                      // 4ei/l
                  nos[i].K_Mola_RX = (nos[i].Pilar.AlturaAcima == 0 ? 0 : ((4 * Ep * (nos[i].Pilar.Dados.Poligono.Ixcg / 1E8)) / (nos[i].Pilar.AlturaAcima / 100))) + (nos[i].Pilar.AlturaAbaixo == 0 ? 0 : ((4 * Ep * (nos[i].Pilar.Dados.Poligono.Ixcg / 1E8)) / (nos[i].Pilar.AlturaAbaixo / 100)));
                  nos[i].K_Mola_RY = (nos[i].Pilar.AlturaAcima == 0 ? 0 : ((4 * Ep * (nos[i].Pilar.Dados.Poligono.Iycg / 1E8)) / (nos[i].Pilar.AlturaAcima / 100))) + (nos[i].Pilar.AlturaAbaixo == 0 ? 0 : ((4 * Ep * (nos[i].Pilar.Dados.Poligono.Iycg / 1E8)) / (nos[i].Pilar.AlturaAbaixo / 100)));
              }
            }
        }

        [NonSerialized]
        public List<TCelulaGrelha> celulas;
        [NonSerialized]
        public List<vec3> nBaseCelulas;
        List<TBarraGrelha> bTemp;
        [NonSerialized]
        double lastX, lastY;
        [NonSerialized]
        AnyPt[] pts;
        [NonSerialized]
        AnyPt[] last_pts;
        [NonSerialized]
        public barra[] bar;
        [NonSerialized]
        public int i_pts, i_last_pts, i_bar;
        TBarraGrelha l1;
        Linha outra = new Linha(0, 0, 0, 0);

        [NonSerialized]
        int ii;
        [NonSerialized]
        double lx, ly, inx, iny, dif_i, dif_f;
        [NonSerialized]
        int inc = 1;
        [NonSerialized]
        bool intersec;
        [NonSerialized]
        TBarraGrelha[] barrasTemp;
        [NonSerialized]
        List<BarrasProximas> BarrasProximasAoNo;
        [NonSerialized]
        int noBase;

        void CalculaPP()
        {
            this.area = 0;
            this.PP = 0;

            /*Cálculo aprox. do PP das vigas e das lajes, juntamente com suas áreas em planta*/
            foreach (TTrechoViga t in vigas)
            {
                t.comprimento = (float)t.pFin.DistanceTo(t.pIni);
                t.CalculaArea();
                this.area += (t.areaSuperior);
             //   t.Dados.CargaParede = 600;
                if (t.Dados.NaoUsarPP)
                    t.PP = 0;
                else
                    t.PP = (this.classeViga.pesoEspecifico * (t.Dados.b1 / 100) * (t.Dados.h1 / 100) * (t.comprimento / 100)) / 100; //em kN
                     
                t.PP += (t.Dados.CargaParede / 100) + (t.Dados.CargaExtra / 100);
                
                this.PP += t.PP;
            }

            foreach (TLaje l in lajes)
            {
                l.CalculaArea();
                this.area += (l.area);
               
                l.PP = (this.classeLaje.pesoEspecifico * (l.Dados.h / 100) * (l.area)) / 100; //em kN;
                l.PP += ((l.Dados.CargaAcidental / 100) * l.area) + ((l.Dados.CargaPermanente / 100) * l.area);

                this.PP += l.PP;
            }
        }

        [NonSerialized]
        public List<TCelulaGrelha> cels;// = new List<TCelulaGrelha>();
        [NonSerialized]
        vec3 rot = new vec3(0, 0, 0);
        [NonSerialized]
        List<vec3> pts_rot = new List<vec3>();
        [NonSerialized]
        vec3 ponto = new vec3(0, 0, 0);
        [NonSerialized]
        vec3 ultimo, penultimo;
        [NonSerialized]
        TPonto med;
        [NonSerialized]
        double angulo = 0;
        [NonSerialized]
        double intx = 0, inty = 0;
        [NonSerialized]
        double alfa;
        [NonSerialized]
        bool EstaNumaCelula = false;
        [NonSerialized]
        TCelulaGrelha Celula;
        [NonSerialized]
        int LinhasRestantes;
        [NonSerialized]
        bool Fim = false;
        [NonSerialized]
        bool ForaDeLaje, ForaDeLaje2 = false;
        bool EmLaje1, EmLaje2, EmCelula1, EmCelula2;
        bool DentroDeCelula = false;

        int IndiceRGBi, IndiceRGBf;
        
        byte Ri, Gi, Bi, Rf, Gf, Bf;
        public void DesenhaCelulas(double OffSetX, double OffSetY, double MultiplicadorAltura)
        {
         /*  foreach (TCelulaGrelha cel in cels)
           {
               Gl.glBegin(Gl.GL_POLYGON);
               foreach (TBarraGrelha b in cel.barras)
               {
                   IndiceRGBi = b.RetIndiceRGB_Deslocamentos(b.pIni.Deslocamento[3]);
                   Ri = (byte)b.RGB_Deslocamentos[IndiceRGBi, 2];
                   Gi = (byte)b.RGB_Deslocamentos[IndiceRGBi, 3];
                   Bi = (byte)b.RGB_Deslocamentos[IndiceRGBi, 4];

                   Gl.glColor3ub(Ri, Gi, Bi);
                   Gl.glVertex3d(b.pIni.x - OffSetX, b.pIni.y - OffSetY, b.pIni.Deslocamento[3] * MultiplicadorAltura);

                   IndiceRGBi = b.RetIndiceRGB_Deslocamentos(b.pFin.Deslocamento[3]);
                   Ri = (byte)b.RGB_Deslocamentos[IndiceRGBi, 2];
                   Gi = (byte)b.RGB_Deslocamentos[IndiceRGBi, 3];
                   Bi = (byte)b.RGB_Deslocamentos[IndiceRGBi, 4];

                   Gl.glColor3ub(Ri, Gi, Bi);
                   Gl.glVertex3d(b.pFin.x - OffSetX, b.pFin.y - OffSetY, b.pFin.Deslocamento[3] * MultiplicadorAltura);
               }
               Gl.glEnd();
           }*/
        }
        int iteracoes;
        void CriaCelulas()
        {
            Fim = false;
            cels = new List<TCelulaGrelha>();
            pts_rot = new List<vec3>();
            ponto = new vec3(0, 0, 0);
            rot = new vec3(0, 0, 0);
            iteracoes = 0;
            TBarraGrelha inicial = null;
            for (int i = 1; i <= max_sBarra; i++)
            {
                if (barras[i].IncideEmViga)
                {
                    inicial = barras[i];
                    break;
                }
            }

            ProcuraProximaAresta(inicial, true, true, inicial.pFin);
        //    cels.RemoveAll(obj => obj.barras.Count() <= 2);
      //      cels.RemoveAll(obj => obj.barras.Count() >= 7);
            MessageBox.Show(iteracoes.ToString() + " celulas: " +cels.Count.ToString());
        }
        const double anguloConst = 10;
        void ProcuraProximaAresta(TBarraGrelha BarraRef, bool NovaCelula, bool pFinal, TNoGrelha pRef)
        {
            try
            {
  
           //     if (Fim)
            //      return;
                pts_rot.Clear();

                if (NovaCelula)
                {
                    cels.Add(new TCelulaGrelha());
                    Celula = cels[cels.Count - 1];
                    Celula.barras.Add(BarraRef);
                    BarraRef.CelulasIncidentes.Add(cels.Count);

                    //    corr += 35;
                    angulo = anguloConst*-1;
                }
                if (cels[cels.Count - 1].barras.Count > 10)
                {
                    Fim = true;
                    return;
                }
                    //MessageBox.Show("");

                //   Gl.glColor3ub((byte)corr, (byte)(100 - corr + 20), 0);
                //  foreach (TLinha l in Linhas)
                {
                    if (pFinal)
                    {
                        ponto.x = BarraRef.pFin.x;
                        ponto.y = BarraRef.pFin.y;
                        alfa = BarraRef.pFin.getAngleTo(BarraRef.pIni) / Const.PIDiv180;
                    }
                    else
                    {
                        ponto.x = BarraRef.pIni.x;
                        ponto.y = BarraRef.pIni.y;
                        alfa = BarraRef.pIni.getAngleTo(BarraRef.pFin) / Const.PIDiv180;
                    }

                    rot.x = ponto.x + 1;
                    rot.y = ponto.y;
                    rot = rot.Rotate(ponto, alfa * Const.PIDiv180);

                    pts_rot.Add(new vec3(rot.x, rot.y, 0));

                    for (i = 0; i < 35; i++)
                    {
                        if (Fim)
                        {
                            //      break;
                            return;
                        }
                        iteracoes++;
           
                        rot = rot.Rotate(ponto, angulo * Const.PIDiv180);

                     /*   if (i == 0 && NovaCelula)
                        {
                            ForaDeLaje = true;
                            foreach (TLaje laj in lajes)
                            {
                                if (laj.PontoEmPoligono(ref rot.x, ref rot.y))
                                {
                                    rot = rot.Rotate(ponto, angulo * Const.PIDiv180);
                                    ForaDeLaje = false;
                                    break;
                                }
                            }

                            if (ForaDeLaje)
                            {
                                angulo *= -1;
                                rot = rot.Rotate(ponto, (angulo * 2) * Const.PIDiv180);
                            }
                        }*/
                        //if ()

                     //   EmCelula1 = false;
                        if (i == 0 && NovaCelula)
                        {
                            foreach (TCelulaGrelha CEL in cels)
                            {
                                if (CEL.PontoEmPoligono(ref rot.x, ref rot.y))
                                {
                                    angulo *= -1;
                                    rot = rot.Rotate(ponto, (angulo * 2) * Const.PIDiv180);
                                    //       EmCelula1 = true;
                                    break;
                                }
                            }

                            foreach (TCelulaGrelha CEL in cels)
                            {
                                if (CEL.PontoEmPoligono(ref rot.x, ref rot.y))
                                {
                              //      MessageBox.Show("opa");
                                    break;
                                }
                            }

                        }
                         //  EmCelula2 = false;
                          /* foreach (TCelulaGrelha CEL in cels)
                           {
                               if (CEL.PontoEmPoligono(ref rot.x, ref rot.y))
                               {
                                   angulo *= -1;
                                 //  EmCelula2 = true;
                                   rot = rot.Rotate(ponto, (angulo * 2) * Const.PIDiv180);
                               }
                           }*/

                        pts_rot.Add(new vec3(rot.x, rot.y, 0));

                        ultimo = pts_rot[pts_rot.Count - 1];
                        penultimo = pts_rot[pts_rot.Count - 2];

                        /*     Gl.glBegin(Gl.GL_LINES);
                             Gl.glVertex2d(pixelX(ultimo.x), pixelY(ultimo.y));
                             Gl.glVertex2d(pixelX(penultimo.x), pixelY(penultimo.y));
                             //    Gl.glVertex2d(pixelX(ponto.x), pixelY(ponto.y));
                             //      Gl.glVertex2d(pixelX(rot.x), pixelY(rot.y));
                             Gl.glEnd();
                             Controle.SwapBuffers();*/

                        foreach (Sincidencias si in pRef.s_incidencias)
                        {
                            if (Fim)
                              return;

                            if ((Object)si.l == (Object)BarraRef)
                                continue;

                            if (si.l.Intersec(ultimo.x, ultimo.y, penultimo.x, penultimo.y, ref intx, ref inty))
                            {
                                if ((Object)si.l == (Object)Celula.barras[0]) // se atingiu a primeira linha, entao a celula esta concluida
                                {
                                    for (kk = 1; kk <= max_sBarra; kk++)
                                    {
                                        l1 = barras[kk];

                                        if (Fim)
                                          return;

                                        if (barras[kk].barraViga || barras[kk].barraRigida) 
                                          continue;
                                        
                                        if (barras[kk].CelulasIncidentes.Count < 2)// && barras[kk].IncideEmViga)
                                          ProcuraProximaAresta(l1, true, true, l1.pFin);
                                    }
                                    Fim = true;
                                    return;
                                }
                                else
                                {
                              //      if (Celula.barras.Count <= 6)
                                    {
                                 //       Fim = true;

                                        Celula.barras.Add(si.l);
                                        Celula.barras[Celula.barras.Count - 1].CelulasIncidentes.Add(cels.Count);

                                        ProcuraProximaAresta(si.l, false, !si.final, si.final ? si.l.pIni : si.l.pFin); // se esta conectada pelo ponto final, analisa o inicial da proxima linha
                                      
                                        if (Fim)
                                          return;
                                    }
                                }
                            }
                        }
                    }
                }

            }
            catch(StackOverflowException e)
            {
                MessageBox.Show(e.Message);
            }
            return;
        }

        void CalculaCarregamentoBarras()
        { 
            celulas      = new List<TCelulaGrelha>();
            sMalhaBase MalhaBasePavimento = MalhasBase[MalhasBase.Count - 1]; 
            
            nBaseCelulas = new List<vec3>();
            BarrasProximasAoNo = new List<BarrasProximas>();
            List<TBarraGrelha> bTemp = new List<TBarraGrelha>();

            barrasTemp   = new TBarraGrelha[max_sBarra];
            vec3 nbase   = new vec3(0, 0, 0);
            vec3 n1      = new vec3(0, 0, 0), n2 = new vec3(0, 0, 0), nm = new vec3(0, 0, 0), nOffset;
            inc = 4;
            TBarraGrelha b1;
            double compTotalBarras, cargaMediaBarra;
            int totBarras;
            
            try
            {               
                foreach (TLaje l in lajes)
                {
                    compTotalBarras = 0;
                    totBarras = 0;
                    bTemp.Clear();
                    for (i = 1; i <= max_sBarra; i++)
                    {
                        b1 = barras[i];

                        if (b1.barraLaje && !b1.barraRigida && b1.Laje.Dados.GerarGrelha && (Object)b1.Laje == (Object)l)
                        {
                            compTotalBarras += barras[i].L;
                            totBarras++;
                            bTemp.Add(b1);
                        }
                    }

                    cargaMediaBarra =(l.PP + ((l.Dados.CargaAcidental / 100) * l.area) + ((l.Dados.CargaPermanente/ 100) * l.area)) / compTotalBarras;
                    foreach (TBarraGrelha b in bTemp)
                    {
                     //   if (b.IncideEmViga)
                        b.CargaDistribuida = -cargaMediaBarra;
                  ///      else
                     //       if (b.barraViga)
                       //         b.carga = -(cargaMediaBarra / 2);
                       // b.pIni.Carga[3] += cargaMediaBarra / 2;
                      //  b.pFin.Carga[3] += cargaMediaBarra / 2;
                    }
                }

               // return;

                for (i = 1; i <= max_sBarra; i++)
                {
                    b1 = barras[i];

                    if (b1.barraViga && !b1.barraRigida)
                    {           
                     //   b1.carga = ((-(b1.TrechoViga.Dados.Poligono.area/10000)* this.classeViga.pesoEspecifico * (b1.TrechoViga.comprimento / 100))) / 100; // carga em kN
                        b1.CargaDistribuida = b1.TrechoViga.PP / (b1.TrechoViga.comprimento/100); // carga em kN/m - pp
                        b1.CargaDistribuida += (b1.TrechoViga.Dados.CargaParede / 100) + (b1.TrechoViga.Dados.CargaExtra / 100);  // parede + extra
                        b1.CargaDistribuida *= -1; // carga equivalente na barra                    
                   //      b1.carga = b1.carga * b1.L; 
 
                       // if (b1.pIni.vinculo == 0)
                       //     b1.pIni.Carga[3] += b1.carga / 2;
                   //     if (b1.pFin.vinculo == 0)
                         //   b1.pFin.Carga[3] += b1.carga / 2;
          
                      //  b1.carga =0;
                    }
                   /* else
                    if (b1.barraLaje && !b1.barraRigida)// && !b1.IncideEmViga)
                    if (b1.Laje.Dados.GerarGrelha)
                    {
                        if (b1.Laje.Dados.GerarGrelhaEspecifica)
                          MalhaBaseLaje = RetMalhaBaseLaje(b1.Laje);
                        else
                          MalhaBaseLaje = MalhaBasePavimento;

                        n1.x = b1.pIni.x;
                        n1.y = b1.pIni.y;
                        n2.x = b1.pFin.x;
                        n2.y = b1.pFin.y;

                        nm = (n1 + n2) / 2;
                        nOffset = new vec3(0, 0, 0);

                        if (b1.direcaoX)// && Geom.Iguais(MalhaBaseLaje.espX , b1.comprimento))
                        {
                            nOffset.x = nm.x + (Math.Cos(b1.angulo * Const.PIDiv180) * MalhaBaseLaje.espY/2);
                            nOffset.y = nm.y + (Math.Sin(b1.angulo * Const.PIDiv180) * MalhaBaseLaje.espY/2);
                            nOffset   = nOffset.Rotate(nm, 90 * Const.PIDiv180);

                            b1.pontoAreaInf1 = new CoordenadaD(nOffset.x, nOffset.y);

                            nOffset          = nOffset.Rotate(nm, -180 * Const.PIDiv180);
                            b1.pontoAreaInf2 = new CoordenadaD(nOffset.x, nOffset.y);

                            b1          = barras[i];
                            b1.areaInf1 = new TPoligono(new CoordenadaD[]{ new CoordenadaD(b1.pIni.x, b1.pIni.y),
                                                                        b1.pontoAreaInf2,new CoordenadaD(b1.pFin.x, b1.pFin.y),
                                                                        b1.pontoAreaInf1, new CoordenadaD(b1.pIni.x, b1.pIni.y)}, true);
                            b1.areaInf1.area /= 10000;
                            b1.CargaDistribuida = (b1.areaInf1.area * b1.Laje.PP) / b1.Laje.area; //((b1.areaInf1.area * this.classeLaje.pesoEspecifico * (b1.Laje.Dados.h / 100)))/ 100;  // carga em kN
                          //  b1.carga += ((350/100) * b1.areaInf1.area);
                            b1.CargaDistribuida += ((b1.Laje.Dados.CargaAcidental / 100) * b1.areaInf1.area) + ((b1.Laje.Dados.CargaPermanente / 100) * b1.areaInf1.area);
                            b1.CargaDistribuida *= -1;

                           // b1.pIni.Carga[3] += b1.carga / 2;
                           // b1.pFin.Carga[3] += b1.carga / 2;
                            b1.CargaDistribuida /= b1.L;

                         //   b1.carga = 0;
                        }
                        else
                        if (b1.direcaoY)// && Geom.Iguais(MalhaBaseLaje.espY, b1.comprimento))
                        {
                            nOffset.x = nm.x + (Math.Cos(b1.angulo * Const.PIDiv180) * MalhaBaseLaje.espX / 2);
                            nOffset.y = nm.y + (Math.Sin(b1.angulo * Const.PIDiv180) * MalhaBaseLaje.espX / 2);
                            nOffset = nOffset.Rotate(nm, 90 * Const.PIDiv180);

                            b1.pontoAreaInf1 = new CoordenadaD(nOffset.x, nOffset.y);

                            nOffset = nOffset.Rotate(nm, -180 * Const.PIDiv180);
                            b1.pontoAreaInf2 = new CoordenadaD(nOffset.x, nOffset.y);

                            b1 = barras[i];
                            b1.areaInf1 = new TPoligono(new CoordenadaD[]{ new CoordenadaD(b1.pIni.x, b1.pIni.y),
                                                                        b1.pontoAreaInf2,new CoordenadaD(b1.pFin.x, b1.pFin.y),
                                                                        b1.pontoAreaInf1, new CoordenadaD(b1.pIni.x, b1.pIni.y)}, true);
                            b1.areaInf1.area /= 10000;
                            b1.CargaDistribuida = (b1.areaInf1.area * b1.Laje.PP) / b1.Laje.area; //((b1.areaInf1.area * this.classeLaje.pesoEspecifico * (b1.Laje.Dados.h / 100)))/ 100;  // carga em kN
                          //  b1.carga += ((350 / 100) * b1.areaInf1.area);
                            b1.CargaDistribuida += ((b1.Laje.Dados.CargaAcidental / 100) * b1.areaInf1.area) + ((b1.Laje.Dados.CargaPermanente / 100) * b1.areaInf1.area);
                            b1.CargaDistribuida *= -1;

                            b1.CargaDistribuida /= b1.L;
                          //  b1.pIni.Carga[3] += b1.carga / 2;
                          //  b1.pFin.Carga[3] += b1.carga / 2;

                           // b1.carga = 0;
                        }
                    }*/
                }

                /*Cargas pontuais aplicadas*/

                bool EncontrouNo = false;
                double distancia, distanciaMax = 999999;
                int no = 1;
              /*  for (i = 0; i < this.LayersByIdPrincipal[Lay.CargaPontual].Obj.Count; i++)
                {
                    if (LayersByIdPrincipal[Lay.CargaPontual].Obj[i].Tipo == Const.ID_CARGA_PONTUAL)
                    {
                        EncontrouNo = false;
                        for (j = 1; j <= max_sNo; j++)
                        {
                            if (Geom.Iguais((LayersByIdPrincipal[Lay.CargaPontual].Obj[i] as TCargaPontual).ponto.x, nos[j].x) && Geom.Iguais((LayersByIdPrincipal[Lay.CargaPontual].Obj[i] as TCargaPontual).ponto.y, nos[j].y))
                            {
                                nos[j].Carga[3] += (LayersByIdPrincipal[Lay.CargaPontual].Obj[i] as TCargaPontual).Dados.valor;
                                EncontrouNo = true;
                                break;
                            }
                        }

                        if (!EncontrouNo) //busca por proximidade
                        {
                            for (j = 1; j <= max_sNo; j++)
                            {
                                distancia = Math.Sqrt(Math.Pow(nos[j].x - (LayersByIdPrincipal[Lay.CargaPontual].Obj[i] as TCargaPontual).ponto.x, 2) + Math.Pow(nos[j].y - (LayersByIdPrincipal[Lay.CargaPontual].Obj[i] as TCargaPontual).ponto.y, 2));

                                if (distancia < distanciaMax)
                                {
                                    no = j;
                                    distanciaMax = distancia;
                                    EncontrouNo = true;
                                }
                            }

                            if (EncontrouNo)
                                nos[no].Carga[3] += (LayersByIdPrincipal[Lay.CargaPontual].Obj[i] as TCargaPontual).Dados.valor;
                        }
                    }
                }

                TCargaLinear cl;

                /*Cargas lineares aplicadas*/
           /*     for (i = 0; i < this.LayersByIdPrincipal[Lay.CargaLinear].Obj.Count; i++)
                {
                    if (LayersByIdPrincipal[Lay.CargaLinear].Obj[i].Tipo == Const.ID_CARGA_LINEAR)
                    {
                        cl = LayersByIdPrincipal[Lay.CargaLinear].Obj[i] as TCargaLinear;

                        for (j = 1; j <= max_sBarra; j++)
                            if (!barras[j].barraRigida)
                               if (Geom.PontoEmLinha2(barras[j].pIni.x, barras[j].pIni.y, cl.pIni.x, cl.pIni.y, cl.pFin.x, cl.pFin.y,1))
                                  if (Geom.PontoEmLinha2(barras[j].pFin.x, barras[j].pFin.y, cl.pIni.x, cl.pIni.y, cl.pFin.x, cl.pFin.y, 1))
                                     barras[j].CargaDistribuida += cl.Dados.valor;
                    }
                }

                return;

                for (i = 1; i <= max_sBarra; i++)
                {
                    if (barras[i].barraViga)
                    {
                        barrasTemp[i - 1] = barras[i];

                        n1.x = barras[i].pIni.x;
                        n1.y = barras[i].pIni.y;
                        n2.x = barras[i].pFin.x;
                        n2.y = barras[i].pFin.y;

                        nm = (n1 + n2) / 2;
                        nOffset = new vec3(0, 0, 0);
                        nOffset.x = nm.x + (Math.Cos(barras[i].angulo * Const.PIDiv180) * 3);
                        nOffset.y = nm.y + (Math.Sin(barras[i].angulo * Const.PIDiv180) * 3);
                        nOffset = nOffset.Rotate(nm, 90 * Const.PIDiv180);
                        nBaseCelulas.Add(nOffset);
                        BarrasProximasAoNo.Add(new BarrasProximas(true));

                        nOffset = new vec3(0, 0, 0);
                        nOffset.x = nm.x + (Math.Cos(barras[i].angulo * Const.PIDiv180) * 3);
                        nOffset.y = nm.y + (Math.Sin(barras[i].angulo * Const.PIDiv180) * 3);
                        nOffset = nOffset.Rotate(nm, -90 * Const.PIDiv180);
                        nBaseCelulas.Add(nOffset);
                        BarrasProximasAoNo.Add(new BarrasProximas(true));
                    }
                }

                for (i = 0; i < nBaseCelulas.Count; i++)
                    for (j = 1; j <= max_sBarra; j++)
                    {
                        dif_i = (Math.Sqrt(Math.Pow(nBaseCelulas[i].x - barras[j].pFin.x, 2) + Math.Pow(nBaseCelulas[i].y - barras[j].pFin.y, 2)));
                        dif_f = (Math.Sqrt(Math.Pow(nBaseCelulas[i].x - barras[j].pIni.x, 2) + Math.Pow(nBaseCelulas[i].y - barras[j].pIni.y, 2)));
                        if (dif_i < 30 || dif_f < 30)
                            BarrasProximasAoNo[i].b.Add(barras[j]);           
                    }

                return;


                bool pula = false;
                int cc = lajes.Count;
                int ml = cc;
                pontosIntersec = new List<PontoAux>();
                for (i = 0; i < nBaseCelulas.Count; i++)
                {                   
                    pula = false;
                    foreach (TCelulaGrelha c in celulas)
                        if (c.PontoEmPoligono(ref nBaseCelulas[i].x, ref nBaseCelulas[i].y))
                        {
                            pula = true;
                            break;
                        }

                    cc = ml;
                    if (!pula)
                    {
                      foreach (TLaje l in lajes)
                        if (!l.PontoEmPoligono(ref nBaseCelulas[i].x, ref nBaseCelulas[i].y))
                          cc--;
                    }

                    if (cc < 1) 
                      pula = true;

                    noBase = i;
                    //if (!pula)
                     // FloodFill(nBaseCelulas[i].x, nBaseCelulas[i].y);
                     // Sweep360(nBaseCelulas[i].x, nBaseCelulas[i].y, i);
                }*/
            }
            catch(Exception rr)
            {
                MessageBox.Show(rr.Message);
            }
        }
        public struct BarrasProximas
        {
            public vec3 no;
            public int cod;
            public List<TBarraGrelha> b;
            public BarrasProximas(bool padra = true)
            {
                b  = new List<TBarraGrelha>();
                no = null;
                cod = 0;
            }
        }

        void FloodFill(double x, double y)
        {
            lastX = x;
            lastY = y;
            bar = new barra[7000];
            last_pts = new AnyPt[1000];
            pts = new AnyPt[1000];
            i_pts = 0;
            i_last_pts = 0;
            i_bar = 0;

            last_pts[i_last_pts++] = new AnyPt(x, y, true);
            bTemp = new List<TBarraGrelha>();

            FloodFillMesh();

            celulas.Add(new TCelulaGrelha());
            foreach (TBarraGrelha b in bTemp)
              celulas[celulas.Count - 1].barras.Add(b);
        }

        bool FindBar(double xi, double yi, double xf, double yf)
        {
            foreach (barra b in bar)
                if ((Geom.Iguais(b.xi, xi) && Geom.Iguais(b.yi, yi) && Geom.Iguais(b.xf, xf) && Geom.Iguais(b.yf, yf))
                 || (Geom.Iguais(b.xf, xi) && Geom.Iguais(b.yf, yi) && Geom.Iguais(b.xi, xf) && Geom.Iguais(b.yi, yf)))
                    return true;

            return false;
        }

        bool FindPtD(double x, double y)
        {
            for (jk = 0; jk < i_pts; jk++)
                if (Geom.Iguais(x, pts[jk].x) && Geom.Iguais(y, pts[jk].y))
                    return true;

            return false;
        }

        void FloodFillMesh()
        {
            try
            {
                for (ii = 0; ii < i_last_pts; ii++)
                {
                    outra.pIni.x = last_pts[ii].x;
                    outra.pIni.y = last_pts[ii].y;

                    if (!FindBar(last_pts[ii].x, last_pts[ii].y, last_pts[ii].x + inc, last_pts[ii].y))
                    {
                        if (!FindPtD(last_pts[ii].x + inc, last_pts[ii].y))
                          pts[i_pts++] = new AnyPt(last_pts[ii].x + inc, last_pts[ii].y, true);

                        inx = last_pts[ii].x + inc;
                        iny = last_pts[ii].y;
                        intersec = false;

                        outra.pFin.x = last_pts[ii].x + inc;
                        outra.pFin.y = last_pts[ii].y;

                        foreach (TBarraGrelha lin in BarrasProximasAoNo[noBase].b)
                        {
                            if (lin.Intersec(outra, ref inx, ref iny))
                            {
                                l1 = lin;
                                //     if (lin.barraRigida) 
                                //         MessageBox.Show("");
                                intersec = true;
                                pts[i_pts - 1].x = inx;
                                pts[i_pts - 1].y = iny;
                                pts[i_pts - 1].continua = false;

                                outra.pFin.x = inx;
                                outra.pFin.y = iny;
                                break;
                            }
                        }

                        if (intersec)
                          if (!bTemp.Contains(l1))
                            bTemp.Add(l1);
                        /*   for (int j = 0; j < i_bar; j++)
                           {
                               Gl.glColor3f(0, 1, 0);
                               Gl.glBegin(Gl.GL_LINES);
                               Gl.glVertex2d(Desenho.pixelX(bar[j].xi), Desenho.pixelY(bar[j].yi));
                               Gl.glVertex2d(Desenho.pixelX(bar[j].xf), Desenho.pixelY(bar[j].yf));
                               Gl.glEnd();
                               Pavimento.desenho.Controle.SwapBuffers();
                           } 
                           */
                        if (i_bar + 1 > 500)
                        {
                            i_bar = 0;
                            return;
                        }
                        bar[i_bar++] = (new barra(last_pts[ii].x, last_pts[ii].y, inx, iny));
                    }

                    if (!FindBar(last_pts[ii].x, last_pts[ii].y, last_pts[ii].x, last_pts[ii].y + inc))
                    {
                        if (!FindPtD(last_pts[ii].x, last_pts[ii].y + inc))
                            pts[i_pts++] = (new AnyPt(last_pts[ii].x, last_pts[ii].y + inc, true));

                        inx = last_pts[ii].x;
                        iny = last_pts[ii].y + inc;

                        intersec = false;
                        l1 = null;

                        outra.pFin.x = last_pts[ii].x;
                        outra.pFin.y = last_pts[ii].y + inc;

                        foreach (TBarraGrelha lin in BarrasProximasAoNo[noBase].b)
                        {
                           if (lin.Intersec(outra, ref inx, ref iny))
                           {
                               //     if (lin.barraRigida)
                               //         MessageBox.Show(""); 

                               pts[i_pts - 1].x = inx;
                               pts[i_pts - 1].y = iny;
                               pts[i_pts - 1].continua = false;
                               l1 = lin;

                               outra.pFin.x = inx;
                               outra.pFin.y = iny;

                               intersec = true;
                               break;
                           }
                        }

                        if (intersec)
                            if (!bTemp.Contains(l1))
                                bTemp.Add(l1);

                        /*   for (int j = 0; j < i_bar; j++)
                           {
                               Gl.glColor3f(0, 1, 0);
                               Gl.glBegin(Gl.GL_LINES);
                               Gl.glVertex2d(Desenho.pixelX(bar[j].xi), Desenho.pixelY(bar[j].yi));
                               Gl.glVertex2d(Desenho.pixelX(bar[j].xf), Desenho.pixelY(bar[j].yf));
                               Gl.glEnd();
                               Pavimento.desenho.Controle.SwapBuffers();
                           } */

                        if (i_bar + 1 > 500)
                        {
                            i_bar = 0;
                            return;
                        }
                        bar[i_bar++] = (new barra(last_pts[ii].x, last_pts[ii].y, inx, iny));
                    }

                    if (!FindBar(last_pts[ii].x, last_pts[ii].y, last_pts[ii].x - inc, last_pts[ii].y))
                    {
                        if (!FindPtD(last_pts[ii].x - inc, last_pts[ii].y))
                            pts[i_pts++] = (new AnyPt(last_pts[ii].x - inc, last_pts[ii].y, true));

                        inx = last_pts[ii].x - inc;
                        iny = last_pts[ii].y;
                        intersec = false;

                        l1 = null;
                        lx = 0;

                        outra.pFin.x = last_pts[ii].x - inc;
                        outra.pFin.y = last_pts[ii].y;

                        foreach (TBarraGrelha lin in BarrasProximasAoNo[noBase].b)
                        {
                            if (lin.Intersec(outra, ref inx, ref iny))
                            {
                                //   if (lin.barraRigida)
                                //       MessageBox.Show("");

                                pts[i_pts - 1].x = inx;
                                pts[i_pts - 1].y = iny;
                                pts[i_pts - 1].continua = false;

                                outra.pFin.x = inx;
                                outra.pFin.y = iny;

                                l1 = lin;

                                lx = inx;
                                ly = iny;

                                intersec = true;
                                break;
                            }
                        }

                        if (intersec)
                            if (!bTemp.Contains(l1))
                                bTemp.Add(l1);

                        /*  for (int j = 0; j < i_bar; j++)
                          {
                              Gl.glColor3f(0, 1, 0);
                              Gl.glBegin(Gl.GL_LINES);
                              Gl.glVertex2d(Desenho.pixelX(bar[j].xi), Desenho.pixelY(bar[j].yi));
                              Gl.glVertex2d(Desenho.pixelX(bar[j].xf), Desenho.pixelY(bar[j].yf));
                              Gl.glEnd();
                              Pavimento.desenho.Controle.SwapBuffers();
                          } */

                        if (i_bar + 1 > 500)
                        {
                            i_bar = 0;
                            return;
                        }
                        bar[i_bar++] = (new barra(last_pts[ii].x, last_pts[ii].y, inx, iny));
                    }

                    if (!FindBar(last_pts[ii].x, last_pts[ii].y, last_pts[ii].x, last_pts[ii].y - inc))
                    {
                        if (!FindPtD(last_pts[ii].x, last_pts[ii].y - inc))
                            pts[i_pts++] = (new AnyPt(last_pts[ii].x, last_pts[ii].y - inc, true));

                        inx = last_pts[ii].x;
                        iny = last_pts[ii].y - inc;
                        intersec = false;
                        l1 = null;

                        outra.pFin.x = last_pts[ii].x;
                        outra.pFin.y = last_pts[ii].y - inc;

                        foreach (TBarraGrelha lin in BarrasProximasAoNo[noBase].b)
                        {
                            if (lin.Intersec(outra, ref inx, ref iny))
                            {
                                //      if (lin.barraRigida)
                                //          MessageBox.Show(""); 

                                pts[i_pts - 1].x = inx;
                                pts[i_pts - 1].y = iny;
                                pts[i_pts - 1].continua = false;

                                l1 = lin;

                                intersec = true;

                                outra.pFin.x = inx;
                                outra.pFin.y = iny;
                                break;
                            }
                        }

                        if (intersec)
                            if (!bTemp.Contains(l1))
                                bTemp.Add(l1);
                        /*   for (int j = 0; j < i_bar; j++)
                           {
                               Gl.glColor3f(0, 1, 0);
                               Gl.glBegin(Gl.GL_LINES);
                               Gl.glVertex2d(Desenho.pixelX(bar[j].xi), Desenho.pixelY(bar[j].yi));
                               Gl.glVertex2d(Desenho.pixelX(bar[j].xf), Desenho.pixelY(bar[j].yf));
                               Gl.glEnd();
                               Pavimento.desenho.Controle.SwapBuffers();
                           } */

                        if (i_bar + 1 > 500)
                        {
                            i_bar = 0;
                            return;
                        }

                        bar[i_bar++] = (new barra(last_pts[ii].x, last_pts[ii].y, inx, iny));
                    }
                }

                i_last_pts = 0;

                for (int k = 0; k < i_pts; k++)
                    if (pts[k].continua)
                        last_pts[i_last_pts++] = pts[k];

                if (i_last_pts == 0)
                    return;

                i_pts = 0;

                FloodFillMesh();
            }

            catch (System.StackOverflowException e)
            {
                MessageBox.Show("Erro de estouro de pilha: " + e.ToString(), "Erro", System.Windows.Forms.MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
        [NonSerialized]
        List<PontoAux> pontosIntersec = new List<PontoAux>();

        void Sweep360(double x, double y, int n, List<TBarraGrelha> bars)
        {
            double angulo = 0;

            double xAnt, yAnt, co, ca;
            bool aborta = false;
            bool intersec = false;
            int i, k;
            bTemp = new List<TBarraGrelha>();
            PontoAux A, B, C, D, inters, pAux;
            TBarraGrelha bar;
            A = new PontoAux(0, 0);
            B = new PontoAux(0, 0);
            C = new PontoAux(0, 0);
            D = new PontoAux(0, 0);
            try
            {
                inters = new PontoAux(0, 0);
                aborta = false;
                for (i = 0; i < 10; i++)   //faz uma varredura 360, iterando de 2 em 2 graus
                {
                    xAnt = x;
                    yAnt = y;

                    if (aborta)
                        break;

                    pontosIntersec.Clear();
                    //for (j = 0; j < 40; j++)

                    //   if (j == 39)
                    //   {
                    //       aborta = true;
                    //        break;
                    //      }

                    co = (Math.Sin(angulo * Const.PIDiv180) * 50) + yAnt;
                    ca = (Math.Cos(angulo * Const.PIDiv180) * 50) + xAnt;

                    A.x = xAnt;
                    A.y = yAnt;

                    B.x = ca;
                    B.y = co;

                    //	 px = xAnt / precisao + ponto_zero[0];
                    //	 py = yAnt / precisao*-1 + ponto_zero[1];

                    xAnt = ca;
                    yAnt = co;

                    //		 px2 = ca / precisao + ponto_zero[0];
                    //py2 = co / precisao*-1 + ponto_zero[1];
                    //
                    //	 _Line(CadWdw, clBlue,psSolid, px,py,px2,py2);
                    //	 this->Refresh();

                   // for (k = 0; k < BarrasProximasAoNo[noBase].b.Count; k++)
                    for (k = 0; k < bars.Count; k++)
                    {
                        bar = bars[k];
                        C.x = bar.pIni.x;
                        C.y = bar.pIni.y;
                        D.x = bar.pFin.x;
                        D.y = bar.pFin.y;

                        intersec = false;

                        if (Geom.calcIntersecEQU_RETA(A, B, C, D, ref inters))
                        {
                            pAux = new PontoAux(inters.x, inters.y, bar);
                            pontosIntersec.Add(pAux);

                            //achou = false;
                            //intersec = true;

                            //	for (l = 0; l < 100; l++)
                            //	{
                            //   if (lin[l] == linha[k])
                            //   {
                            //	  achou = true;
                            //	  break;
                            //  }
                            //	};

                            //	px = C.x / precisao + ponto_zero[0];
                            //	py = C.y / precisao*-1 + ponto_zero[1];

                            //	px2 = D.x / precisao + ponto_zero[0];
                            //	py2 = D.y / precisao*-1 + ponto_zero[1];


                            //  if (!linha[k]->contornoLaje)
                            //  {
                            //    LinhasLaje[++linhas_lajes] = linha[k];
                            //	vigasLaje[linhas_lajes]    = Viga[linha[k]->numViga];

                            //    linha[k]->contornoLaje = true;

                            // _Line(CadWdw, clYellow,psSolid, px,py,px2,py2);
                            //	 this->Refresh();
                            //    };

                            //   break;

                        };
                    };

                    //se for linha horizontal, reordena os nós em ordem crescente de X
                 /*   if (Geom.Iguais(A.y, B.y, 0.1))
                    {
                        for (kk = 0; kk < pontosIntersec.Count; kk++)
                        {
                            for (int jj = kk; jj < pontosIntersec.Count; jj++)
                            {
                                if (pontosIntersec[jj].x < pontosIntersec[kk].x)
                                {
                                    temp = pontosIntersec[kk];
                                    pontosIntersec[kk] = pontosIntersec[jj];
                                    pontosIntersec[jj] = temp;
                                };
                            };
                        };
                    }
                    else
                    {
                        //senao reordena os nós em ordem crescente de Y, independente se for barra vertical ou oblíqua
                        for (kk = 0; kk < pontosIntersec.Count; kk++)
                        {
                            for (int jj = kk; jj < pontosIntersec.Count; jj++)
                            {
                                if (pontosIntersec[jj].y < pontosIntersec[kk].y)
                                {
                                    temp = pontosIntersec[kk];
                                    pontosIntersec[kk] = pontosIntersec[jj];
                                    pontosIntersec[jj] = temp;
                                };
                            };
                        };
                    };*/

                    if (pontosIntersec.Count>0)
                      if (!bTemp.Contains(pontosIntersec[0].b))
                          bTemp.Add(pontosIntersec[0].b);

                    //  if (intersec) 
                    //    break;

                    angulo += 36;
                };
            }
            catch(Exception EE)
            {
                MessageBox.Show(EE.Message);
            }
            if (!aborta)
            {
                celulas.Add(new TCelulaGrelha());
                foreach (TBarraGrelha b in bTemp)
                    celulas[celulas.Count - 1].barras.Add(b);
            }
        }

        void CalculaCarregamentoBarras___()
        {
            for (int i = 1; i <= max_sBarra; i++)
            {
            }
        }

        void Inicializa()
        {
            this.nos = new TNoGrelha[50000];
            this.barras = new TBarraGrelha[50000];
            max_sNo = 0;
            max_sBarra = 0;
            nNos = 0;
            nBarras = 0;
            max_linhas = linhasCount;
        }
        void VerificaSeCancelou()
        {
            if (gerenciador.CalculoCancelado)
            {
                gerenciador.CalculoCancelado = false;
                throw new TCalculoInterrompido(this, "Pavimento: " + Descricao + "  -  " + Const.CANCELOU_CALCULO);
            }
        }

        private bool GerarGrelha2()
        {
            VerificaSeCancelou();               
            
            Inicializa();
            Progresso.Increment(1);
            CriaMalhasBase();

            Progresso.Increment(1);
            CriaBarrasLaje();

            Progresso.Increment(1);
            CriaNosExtremidade();

            Progresso.Increment(1);
            CriaBarrasViga();

            Progresso.Increment(1);
            CriarBarrasRigidas();

            Progresso.Increment(1);
            CorrecaoMalha();

            Progresso.Increment(1);
            Finaliza();            
                                  
            return true;
        }
        public bool RetPilarDoPonto(ref TPilar p, double x, double y)
        {
           foreach (TPilar pilar in pilares)
               if (pilar.Dados.Poligono.PontoEmPoligono(x, y))
               {
                   p = pilar;
                   return true;
               }
           return false;
        }

       
        void RetIntersecaoBarraXPilar(ref TPilar pilar, double xIni, double yIni, double xFin, double yFin, ref double interx, ref double intery)
        {
            foreach (TLinha Aresta in pilar.Dados.Poligono.linhas_poligonal)
              if (Aresta.Intersec(xIni, yIni, xFin, yFin, ref interx, ref interx))
                 return;
        }
       
        bool PilarSozinho(TPilar p)
        {
            foreach (TTrechoViga t in vigas)
                if (t.NumPilar_PontoFinal == p.Dados.numero || t.NumPilar_PontoInicial == p.Dados.numero)
                    return false;

            return true;
        }

        private void CriarBarrasRigidas()
        {
            try
            {
                   List<TBarraGrelha> barrasRigidasAdicionais = new List<TBarraGrelha>();

           MsgCalculo("Pavimento: " + this.Descricao, "Pavimento: " + this.Descricao + " - Barras rígidas...", pilares.Count() + max_sNo + this.linhas.Count() + (int)(max_sBarra * 2),false, false );
           double x, y, cx,cy, x_, y_;
           int NoCentroide,n, NoIni;
           
           /*Cria nós de centroide de pilares*/
        
            VerificaSeCancelou();

            if (!Testes)
            {
                foreach (TPilar pilar in pilares)
                {
                    if (PilarSozinho(pilar)) continue;

                    //     Progresso.Increment(1);
                    x = pilar.Dados.Poligono.centroide.X;
                    y = pilar.Dados.Poligono.centroide.Y;

                    n = LocalizaNo(ref x, ref y);
                    if (n == -1)
                    {
                        nos[++max_sNo] = (new TNoGrelha(pilar.Dados.Poligono.centroide.X, pilar.Dados.Poligono.centroide.Y, 0, 0, 0, max_sNo + 1, null, null, true, true));
                        nos[max_sNo].CentroidePilar = true;
                        nos[max_sNo].Pilar = pilar;
                    }
                    else
                    {
                        nos[n].CentroidePilar = true;
                        nos[n].Pilar = pilar;
                    }
                }
            }

           /*Cria barras rígidas de laje*/
           double inx = 0, iny = 0;
           for (int i = 1; i <= max_sBarra; i++)
           {
            //  Progresso.Increment(1);
              if ((Object)barras[i] != null)
              if (!barras[i].barraRigida && !barras[i].barraViga)
              {
                  foreach (TPilar pilar in pilares)
                  {
                      if (PilarSozinho(pilar)) continue;
                      
                      foreach (TLinha Aresta in pilar.Dados.Poligono.linhas_poligonal)
                      {
                          if (Aresta.Intersec(barras[i].pIni.x, barras[i].pIni.y, barras[i].pFin.x, barras[i].pFin.y, ref inx, ref iny))
                          {
                              if (pilar.Dados.Poligono.PontoEmPoligono(barras[i].pIni.x, barras[i].pIni.y))
                              {
                                  nos[++max_sNo] = new TNoGrelha(inx, iny, 0, 0, 0, max_sNo + 1, null, null, false, true);

                                  barras[i].pIni = nos[max_sNo];
                                  
                                  cx = pilar.Dados.Poligono.centroide.X;
                                  cy = pilar.Dados.Poligono.centroide.Y;
                                  
                                  NoCentroide = LocalizaNo(ref cx, ref cy);

                                  if (NoCentroide > -1 && (nos[max_sNo].DistanceTo(nos[NoCentroide]) > 1))
                                  {
                                     /* barrasRigidasAdicionais.Add(
                                          new TBarraGrelha(
                                          nos[max_sNo],
                                          nos[NoCentroide],
                                          null, 0, 0, 0, 0, 0, 0,
                                          null,
                                          false,
                                          true,
                                          Geom.Iguais(nos[max_sNo].x, barras[i].pIni.x), Geom.Iguais(nos[max_sNo].y, barras[i].pIni.y),
                                          -1,
                                          !Geom.Iguais(nos[max_sNo].y, barras[i].pIni.y) && !Geom.Iguais(nos[max_sNo].x, barras[i].pIni.x)));

                                      barrasRigidasAdicionais[barrasRigidasAdicionais.Count - 1].barraRigida = true;
                                      barrasRigidasAdicionais[barrasRigidasAdicionais.Count - 1].Pilar = pilar;*/

                                      nos[max_sNo].vinculo     = 3;
                                      nos[NoCentroide].vinculo = 3;
                                    
                                      nos[max_sNo].Pilar     = pilar;
                                      nos[NoCentroide].Pilar = pilar;

                                  }
                              }
                              else
                              if (pilar.Dados.Poligono.PontoEmPoligono(barras[i].pFin.x, barras[i].pFin.y))
                              {
                                  nos[++max_sNo] = new TNoGrelha(inx, iny, 0, 0, 0, max_sNo + 1, null, null, false, true);

                                  barras[i].pFin = nos[max_sNo];

                                  cx = pilar.Dados.Poligono.centroide.X;
                                  cy = pilar.Dados.Poligono.centroide.Y;

                                  NoCentroide = LocalizaNo(ref cx, ref cy);

                                  if (NoCentroide > -1 && (nos[max_sNo].DistanceTo(nos[NoCentroide]) > 1))
                                  {                                      
                                     /* barrasRigidasAdicionais.Add(
                                          new TBarraGrelha(
                                          nos[max_sNo],
                                          nos[NoCentroide],
                                          null,  0, 0, 0, 0, 0, 0,
                                          null,
                                          false,
                                          true,
                                          Geom.Iguais(nos[max_sNo].x, barras[i].pFin.x), Geom.Iguais(nos[max_sNo].y, barras[i].pFin.y),
                                          -1,
                                          !Geom.Iguais(nos[max_sNo].y, barras[i].pFin.y) && !Geom.Iguais(nos[max_sNo].x, barras[i].pFin.x)));

                                      barrasRigidasAdicionais[barrasRigidasAdicionais.Count - 1].barraRigida = true;
                                      barrasRigidasAdicionais[barrasRigidasAdicionais.Count - 1].Pilar       = pilar;*/
                                     
                                      nos[max_sNo].vinculo     = 3;
                                      nos[NoCentroide].vinculo = 3;

                                      nos[max_sNo].Pilar = pilar;
                                      nos[NoCentroide].Pilar = pilar;
                                  }
                              }
                              else
                              {
                                   //caso ela passe por dentro do pilar
                              }
                          }
                      }
                  }
              }
           }

           foreach (TBarraGrelha bar in barrasRigidasAdicionais)
             barras[++max_sBarra] = bar;

           /*Apaga barras de laje dentro de projeção de pilares*/
           for (int i = 1; i <= max_sBarra; i++)
           {
              // Progresso.Increment(1); 
               if ((Object)barras[i] != null)
               {
                   if (!barras[i].barraRigida)
                   {
                       foreach (TPilar pilar in pilares)
                       {
                           if (pilar.Dados.Poligono.PontoEmPoligono(barras[i].pIni.x, barras[i].pIni.y) &&
                               pilar.Dados.Poligono.PontoEmPoligono(barras[i].pFin.x, barras[i].pFin.y))
                           {
                               barras[i] = null;
                               break;
                           }
                       }
                   }            
               }
           }

          /*Apaga nós sem barra*/
           bool achou;
           for (int i = 1; i <= max_sNo; i++)
           {
               //Progresso.Increment(1); 
               if ((Object)nos[i] != null)
               {
                   achou = false;
                   for (int j = 1; j <= max_sBarra; j++)
                   {
                       if ((Object)barras[j] != null)
                       {
                           if ((Object)barras[j].pIni == (Object)nos[i] || (Object)barras[j].pFin == (Object)nos[i])
                           {
                               achou = true;
                               break;
                           }
                       }
                   }

                   if (!achou && !nos[i].CentroidePilar)
                       nos[i] = null;                
               }
           }


           foreach (TLinha LinhaContorno in this.linhas)
           {
             //  Progresso.Increment(1);
               if (LinhaContorno.barraRigida)
               {
                   if (LinhaContorno.pIni.x == 0 && LinhaContorno.pIni.y == 15)
                   {
               //        MessageBox.Show("");
                   }

                   cx = LinhaContorno.pIni.x;
                   cy = LinhaContorno.pIni.y;
                   x_ = LinhaContorno.pFin.x;
                   y_ = LinhaContorno.pFin.y;

                   NoIni       = LocalizaNo(ref cx, ref cy);
                   NoCentroide = LocalizaNo(ref x_, ref y_);

                   if (NoIni == -1)
                   {
                       nos[++max_sNo] = new TNoGrelha(cx, cy, 0, 0, 0, max_sNo + 1, null, null, false, true);
                       nos[max_sNo].vinculo = 3;
                       NoIni = max_sNo;
                //       MessageBox.Show("");
                   }

                   if (NoCentroide == -1)
                   {
                //       MessageBox.Show("");
                   }

                   if (NoIni > -1)
                   {
                       if (Testes)
                           nos[NoIni].vinculo = 2;
                       else
                       {
                           barras[++max_sBarra] = (new TBarraGrelha(nos[NoCentroide], nos[NoIni], LinhaContorno.TrechoViga, 0, 0, 0, 0, 0, 0, null, true, false, Geom.Iguais(LinhaContorno.pIni.x, LinhaContorno.pFin.x),
                                                                                                                        Geom.Iguais(LinhaContorno.pIni.y, LinhaContorno.pFin.y),
                                                                                                                        max_sBarra,
                                                                                                                       !Geom.Iguais(LinhaContorno.pIni.y, LinhaContorno.pFin.y) &&
                                                                                                                       !Geom.Iguais(LinhaContorno.pIni.x, LinhaContorno.pFin.x)));
                           nos[NoCentroide].barrasIncidentes.Add(barras[max_sBarra]);
                           nos[NoIni].barrasIncidentes.Add(barras[max_sBarra]);

                           nos[NoIni].vinculo = 3;
                           nos[NoCentroide].vinculo = 3;

                           nos[NoIni].Pilar = nos[NoCentroide].Pilar;

                           //barras[max_sBarra].Pilar = pilar;
                           barras[max_sBarra].barraRigida = true;
                           barras[max_sBarra].Pilar = nos[NoCentroide].Pilar;
                       }
                   }
                  // break;
               }
           }

           /*Cria barras rigidas provenientes das vigas*/
        /*   foreach (TLinha LinhaContorno in this.linhas)
           {
               if (LinhaContorno.barraRigida)
               {
                   if (LinhaContorno.pIni.x == 0 && LinhaContorno.pIni.y == 15)
                   {
                       MessageBox.Show("");
                   }
                   foreach (TPilar pilar in pilares)
                   {
                       if (pilar.PontoDentroOuNaAresta(LinhaContorno.pIni.x, LinhaContorno.pIni.y,.1))
                       {
                           double cx = pilar.Dados.Poligono.centroide.X;
                           double cy = pilar.Dados.Poligono.centroide.Y;

                           int NoCentroide = LocalizaNo(ref cx, ref cy);
                           int NoIni = -1;
                          
                           double x_ = LinhaContorno.pFin.x;
                           double y_ = LinhaContorno.pFin.y;

                           if (Geom.Iguais(cx, LinhaContorno.pIni.x) && Geom.Iguais(cy, LinhaContorno.pIni.y))
                               NoIni = LocalizaNo(ref x_, ref y_);
                           else
                           {
                               x_ = LinhaContorno.pIni.x;
                               y_ = LinhaContorno.pIni.y;

                               if (Geom.Iguais(cx, LinhaContorno.pFin.x) && Geom.Iguais(cy, LinhaContorno.pFin.y))
                                 NoIni = LocalizaNo(ref x_, ref y_);
                           }

                           if (NoIni == -1)
                           {
                               MessageBox.Show("");
                           }

                           barras[++max_sBarra] = (new TBarraGrelha(nos[NoCentroide], nos[NoIni], LinhaContorno.TrechoViga, 100, 2, 2.4, 2.1, 0, 0, 0, 0, 0, 0, null, true, false, Geom.Iguais(LinhaContorno.pIni.x, LinhaContorno.pFin.x),
                                                                                                          Geom.Iguais(LinhaContorno.pIni.y, LinhaContorno.pFin.y),
                                                                                                                         max_sBarra,
                                                                                                                        !Geom.Iguais(LinhaContorno.pIni.y, LinhaContorno.pFin.y) &&
                                                                                                                        !Geom.Iguais(LinhaContorno.pIni.x, LinhaContorno.pFin.x)));
                           nos[NoCentroide].barrasIncidentes.Add(barras[max_sBarra]);
                           nos[NoIni].barrasIncidentes.Add(barras[max_sBarra]);

                           nos[NoIni].vinculo = 3;
                           nos[NoCentroide].vinculo = 3; 

                           barras[max_sBarra].Pilar = pilar;
                           barras[max_sBarra].barraRigida = true;
                           break;
                       }
                   }                 
               }
           }*/

           /*  barras[max_sBarra].barraRigida = LinhaContorno.barraRigida;
 if (LinhaContorno.barraRigida)
 {                   
     barras[max_sBarra].E = 100;

     foreach (TPilar pilar in pilares)
     {
         if (pilar.Dados.Poligono.PontoEmPoligono(barras[max_sBarra].pIni.x, barras[max_sBarra].pIni.y) ||
             pilar.Dados.Poligono.PontoEmPoligono(barras[max_sBarra].pFin.x, barras[max_sBarra].pFin.y))
         {
             barras[max_sBarra].Pilar = pilar;
             break;
         }
     }
 }*/

           TPilar PilarAux = null;

         /*  for (int j = 1; j <= max_sBarra; j++)
           {
               if ((Object)barras[j] != null)
               {
                   if (barras[j].barraRigida)
                   {
                    //   if (barras[j].Pilar.Dados.Poligono.PontoEmPoligono(barras[j].pIni.x, barras[j].pIni.y))
                    //   {
                           //se barra esta totalmente contida no pilar, sem nenhum ponto nos vertices
                           //isso serve para aplicar um engaste em vez de uma mola no ponto interno ao pilar, aplica-se molas somente nos pontos da aresta
                          if (!barras[j].Pilar.PontoEmAresta(barras[j].pIni.x, barras[j].pIni.y) && 
                              !barras[j].Pilar.PontoEmAresta(barras[j].pFin.x, barras[j].pFin.y))
                          {

                              if (Geom.Iguais(barras[j].Pilar.Dados.Poligono.centroide.X, barras[j].pIni.x) &&
                                  Geom.Iguais(barras[j].Pilar.Dados.Poligono.centroide.Y, barras[j].pIni.y))
                              {
                                  for (int k = 1; k <= max_sBarra; k++)
                                  {
                                     if ((Object)barras[k] != null)
                                     {
                                       if (barras[k].barraRigida && (Object)barras[k] != (Object)barras[j])
                                       {   
                                   //        if ()
                                       }
                                     }
                                  }
                                  
                              }
                              else
                              if (Geom.Iguais(barras[j].Pilar.Dados.Poligono.centroide.X, barras[j].pFin.x) &&
                                  Geom.Iguais(barras[j].Pilar.Dados.Poligono.centroide.Y, barras[j].pFin.y))
                              {
                                  barras[j].pFin.vinculo = 3;
                                  barras[j].pIni.vinculo = 1;
                              }
                          }

                          if (barras[j].Pilar.PontoEmAresta(barras[j].pIni.x, barras[j].pIni.y))  
                              barras[j].pIni.vinculo = 3;

                          if (barras[j].Pilar.PontoEmAresta(barras[j].pFin.x, barras[j].pFin.y))  
                              barras[j].pFin.vinculo = 3;

                      // }                       
                   }      
               }
           }*/
            }
            catch (Exception e)
            {
                System.Windows.Forms.MessageBox.Show("Erro ao criar barras rígidas: " + e.Message);
            }
   
        }

        int cc, kk;
        private bool CriarBarrasContorno(TLinha LinhaContorno, double tol = 0.0001)
        {

          //  if (LinhaContorno.barraRigida)
         //       MessageBox.Show(""); 
            
            NoExt = new TNoGrelha[200];
            TNoGrelha temp;

            if (!LinhaContorno.LinhaEixoViga) return false;
            if (LinhaContorno.auxiliar) return false;
            if (LinhaContorno.barraRigida) return false;


            cc = -1;

            for (kk = 0; kk < 200; kk++)
              NoExt[kk] = null;

            // cria a lista dos nós que estão em cima da linha (contorno)
            for (kk = 1; kk <= max_sNo; kk++)
            {
               // if (Geom.Iguais(nos[kk].x,257.197657))
               //   MessageBox.Show("");

                if (Geom.PontoEmLinha2((nos[kk].x), (nos[kk].y), LinhaContorno.pIni.x, LinhaContorno.pIni.y, LinhaContorno.pFin.x, LinhaContorno.pFin.y,tol))
                {
                    NoExt[++cc] = nos[kk];
                    NoExt[cc].TrechoViga = LinhaContorno.TrechoViga;
                  //  Gl.glPointSize(5);
                  /*  Gl.glColor3d(1, 0, 0);
                    Gl.glBegin(Gl.GL_POINTS);
                    Gl.glVertex2d(Desenho.pixelX(NoExt[cc].x), Desenho.pixelY(NoExt[cc].y));
                    Gl.glEnd();

                    desenho.Controle.SwapBuffers();*/
                }
            }

        //    g2d.Color3d(.0, .9, 0.0);

       //     g2d.line(LinhaContorno.pIni.px_x, LinhaContorno.pIni.px_y, LinhaContorno.pFin.px_x, LinhaContorno.pFin.px_y);

      //      desenho.Controle.SwapBuffers();

            if (LinhaContorno.comprimento == 0)
                LinhaContorno.comprimento = (float)LinhaContorno.pIni.DistanceTo(LinhaContorno.pFin);

            // se tem só dois nós e o comprimento da linha tipo viga for maior que o espaçamento, 
            // então é uma viga isolada sem laje. Deve-se gerar somente barras do tamanho do menor espaçamento para representar a  viga
            // Se o tamanho da barra de viga for menor que o menor dos espaçamentos, nao precisa gerar as barras, pois nao faz sentido.
            if ((cc + 1 == 2) && (LinhaContorno.comprimento > Math.Min(espacamentoX, espacamentoY)))
            {
                double fracao, quebra, divisoes_quebrada, divisoes_inteiras;
                double TamanhoBarrinha;
                int    Divisoes;

				if (LinhaContorno.comprimento < (2 * Math.Min(espacamentoX, espacamentoY)))
				{
					Divisoes = 2;
                    TamanhoBarrinha = LinhaContorno.comprimento / 2;
				}
				else
				{
                   fracao = FuncoesGerais.Frac(LinhaContorno.comprimento / Math.Min(espacamentoX, espacamentoY));
                   quebra = fracao * Math.Min(espacamentoX, espacamentoY);

                   divisoes_inteiras = ((LinhaContorno.comprimento / Math.Min(espacamentoX, espacamentoY)) - fracao);
                   divisoes_quebrada = quebra / divisoes_inteiras;

                   TamanhoBarrinha = divisoes_quebrada + Math.Min(espacamentoX, espacamentoY);

                   Divisoes = System.Convert.ToInt32((float)LinhaContorno.comprimento / (float)TamanhoBarrinha);
                }

                vec3 pI = new vec3(LinhaContorno.pIni.x, LinhaContorno.pIni.y, 0);
                vec3 pF = new vec3(LinhaContorno.pFin.x, LinhaContorno.pFin.y,0);
                double alfa = pI.getAngleTo(pF) / Const.PIDiv180;
                double xi = pI.x;
                double yi = pI.y;
                
                int u = cc;

                for (int kk = 0; kk < Divisoes-1; kk++)
                {
                    double xn, yn;
                    xn = xi + (TamanhoBarrinha * (Math.Cos(alfa * 0.01745)));
                    yn = yi + (TamanhoBarrinha * (Math.Sin(alfa * 0.01745)));

                   //    g2d.Color3d(.0, .9, 0.0);

                    //    g2d.line(LinhaContorno.pIni.px_x, LinhaContorno.pIni.px_y, LinhaContorno.pFin.px_x, LinhaContorno.pFin.px_y);
                        
                       /* 
                        Gl.glPointSize(3);
                        Gl.glColor3d(0, 0, 1);
                        Gl.glBegin(Gl.GL_POINTS);
                        Gl.glVertex2d(Desenho.pixelX(xn), Desenho.pixelY(yn));
                        Gl.glEnd();
                        desenho.Controle.SwapBuffers();*/

                        nos[++max_sNo] = new TNoGrelha(xn, yn, 0, 0, 0, max_sNo + 1, LinhaContorno.TrechoViga, null, false, true);

                        NoExt[++u] = nos[max_sNo];

                    xi = xn;
                    yi = yn;
                }

                cc = u;
            }

            //se for linha horizontal, reordena os nós em ordem crescente de X
            if (Geom.Iguais(LinhaContorno.pIni.y, LinhaContorno.pFin.y,0.1))
            {
                for (kk = 0; kk < (cc + 1); kk++)
                {
                    for (int j = kk; j < (cc + 1); j++)
                    {
                        if (NoExt[j].x < NoExt[kk].x)
                        {
                            temp = NoExt[kk];
                            NoExt[kk] = NoExt[j];
                            NoExt[j] = temp;
                        };
                    };
                };
            }
            else
            {
                //senao reordena os nós em ordem crescente de Y, independente se for barra vertical ou oblíqua
                for (kk = 0; kk < (cc + 1); kk++)
                {
                    for (int j = kk; j < (cc + 1); j++)
                    {
                        if (NoExt[j].y < NoExt[kk].y)
                        {
                            temp = NoExt[kk];
                            NoExt[kk] = NoExt[j];
                            NoExt[j] = temp;
                        };
                    };
                };
            };

            // cria as barras ordenadas em x ou y (barra horizontal = ordem x), pois os nós em cima da aresta já estão ordenados, então é só ligar o nó 'p' com o nó 'p + 1' para formar a barra 
            for (kk = 0; kk < cc; kk++)
            {
                if (!LinhaContorno.barraRigida)
                {
                    barras[++max_sBarra] = (new TBarraGrelha(NoExt[kk], NoExt[kk + 1], LinhaContorno.TrechoViga,  0, 0, 0, 0, 0, 0, null, true, false, Geom.Iguais(LinhaContorno.pIni.x, LinhaContorno.pFin.x),
                                                                                                                                       Geom.Iguais(LinhaContorno.pIni.y, LinhaContorno.pFin.y),
                                                                                                                                       max_sBarra,
                                                                                                                                      !Geom.Iguais(LinhaContorno.pIni.y, LinhaContorno.pFin.y) &&
                                                                                                                                      !Geom.Iguais(LinhaContorno.pIni.x, LinhaContorno.pFin.x)));
                    NoExt[kk].barrasIncidentes.Add(barras[max_sBarra]);
                    NoExt[kk + 1].barrasIncidentes.Add(barras[max_sBarra]);
                   
                    NoExt[kk].Laje = null;
                    NoExt[kk+1].Laje = null;
                    NoExt[kk].TrechoViga = LinhaContorno.TrechoViga;
                    NoExt[kk + 1].TrechoViga = LinhaContorno.TrechoViga;

                }
            };

            return true;
        }

        private void ApagarBarrasContorno(TTrechoViga trecho)
        {
            for (int i = 1; i <= max_sBarra; i++)
                if ((Object)barras[i] != null)
                  if ((Object)barras[i].TrechoViga !=null)
                     if ((Object)barras[i].TrechoViga == (Object)trecho)
                        barras[i] = null;
        }

        int ContaErro = 0;
 
        private void CorrecaoMalha()
        {
        //    Application.DoEvents();
            /*
             
             -> Verificação de duas barras sobrepostas
             -> Verificação de dois nós sobrepostos
             -> Verificação de vazio em barra de viga (barra faltando)
             
             */
           
            //  -> Verificação de dois nós sobrepostos
            RefazerGrelha = false;

            VerificaSeCancelou();
                    
            MsgCalculo("Pavimento: " + this.Descricao, "Pavimento: " + this.Descricao + " - Verificando grelha...", max_sNo, false, false);
            for (int i = 1; i <= max_sBarra; i++)
                if ((Object)barras[i] != null)
                    barras[i].comprimento = (Math.Sqrt(Math.Pow(barras[i].pIni.x - barras[i].pFin.x, 2) + Math.Pow(barras[i].pIni.y - barras[i].pFin.y, 2)));

            TBarraGrelha b2 = null;
          /*  for (int i = 1; i <= max_sBarra; i++)
            {
                gerenciador.Progresso.Increment(1); 
                
                b1 = barras[i];
                if ((Object)b1 != null)
                  if (b1.comprimento < 0.1)
                  {
                      g2d.Color3d(1, 0, 0);
                      g2d.line(b1.pIni.px_x, b1.pIni.px_y, b1.pFin.px_x, b1.pFin.px_y);

                      desenho.Controle.SwapBuffers();
                    
                      TNoGrelha pi, pf;
                      TNoGrelha ni = b1.pIni;
                    //  barras[i] = null;

                      for (int k = 0; k < ni.barrasIncidentes.Count; k++)
                      {
                          if ((Object)ni.barrasIncidentes[k] != null)
                          {
                              pi = ni.barrasIncidentes[k].pIni;
                              pf = ni.barrasIncidentes[k].pFin;

                              if ((Object)pi == (Object)ni)
                              {
                                  pi = b1.pFin;
                              }

                              if ((Object)pf == (Object)ni)
                              {
                                  pf = b1.pFin;
                              }

                              nos[b1.pIni.Numero+1] = null;
                          }

                      }

                  }
            }
            */
            bool AvisoOk = false;

            VerificaSeCancelou();
            
                for (int i = 1; i <= max_sNo; i++)
                {
                    //Progresso.Increment(1);
                    for (int j = 1; j <= max_sNo; j++)
                        //   if (!nos[j].Selecionado && !nos[i].Selecionado)
                        if ((Object)nos[i] != null && (Object)nos[j] != null)
                            if ((Object)nos[i] != (Object)nos[j])
                                if (nos[i].DistanceTo(nos[j]) <= 0.1 && !RefazerGrelha && nos[i].TrechoViga != null && nos[j].Laje != null)
                                {

                                    //        g2d.Color3d(1, 0, 0);
                                    //     g2d.line(nos[i].px_x - 5, nos[i].px_y - 5, nos[i].px_x + 10, nos[i].px_y + 10);

                                    //      desenho.Controle.SwapBuffers();
                                    //   MessageBox.Show("Existem dois nós muito próximos (" + nos[i].DistanceTo(nos[j]).ToString("n5") + " cm de distância entre eles). " +
                                    //        " Regere a grelha com outro espaçamento [nó " + j + " e nó " + i + "].");

                                         espacamentoX += .2;
                                         espacamentoY += .2;

                                    //   ContaErro++;
                                    //   gerenciador.HistoricoCalculo("   Correção da malha [" + espacamentoX + " x " + espacamentoY + "]", ContaErro > 1);
                                   // if ()
                                    gerenciador.HistoricoCalculo(" Nós da laje " + nos[j].Laje.Titulo.texto + " muito próximos dos nós da viga " + nos[i].TrechoViga.Texto1.texto + ". Grelha alterada: " + espacamentoX +" x "+espacamentoY,true);
                                  //  AvisoOk = true;

                                    RefazerGrelha = true;
                                
                                    /*     g2d.StrokeText("o", Desenho.pixelX(nos[j].x) - 3, Desenho.pixelY(nos[j].y) + 3,
                                                     0.2f, -0.2f,
                                                     1, 0, 0);
                                         desenho.Controle.SwapBuffers();

                                         g2d.StrokeText("o", Desenho.pixelX(nos[i].x) - 3, Desenho.pixelY(nos[i].y) + 3,
                                                     0.2f, -0.2f,
                                                     1, 0, 0);
                                         desenho.Controle.SwapBuffers();*/


                                    //  TNoGrelha n1, n2;

                                    //   n1 = nos[i];
                                    //   n2 = nos[j];
                                    /*         ok = false;
                                         //    GerarGrelha();
                                         //    break;
                                          //   UnirNos(nos[i], nos[j]); */
                                }

                }
           
          //  ok = true;
         //   if (!regeraMalha)
         //   {
         /*       gerenciador.Progresso.Maximum = System.Convert.ToInt16(max_sBarra);

                gerenciador.Progresso.Value = 0;
                // -> Verificação de duas barras sobrepostas
                for (int i = 1; i <= max_sBarra; i++)
                {
                    gerenciador.Progresso.Increment(1);
                    for (int j = 1; j <= max_sBarra; j++)
                    {
                        b1 = barras[i];
                        b2 = barras[j];

                        if ((Object)b1 != null && (Object)b2 != null)
                            if ((Object)b1 != (Object)b2)
                            {
                                if (Geom.PontoEmLinha(b1.pIni.x, b1.pIni.y, b2.pIni.x, b2.pIni.y, b2.pFin.x, b2.pFin.y, 0.01) &&
                                   (Geom.PontoEmLinha(b1.pFin.x, b1.pFin.y, b2.pIni.x, b2.pIni.y, b2.pFin.x, b2.pFin.y, 0.01)))
                                {
                                    g2d.Color3d(1, 0, 0);
                                    g2d.line(b1.pIni.px_x, b1.pIni.px_y, b1.pFin.px_x, b1.pFin.px_y);

                                    desenho.Controle.SwapBuffers();

                                    g2d.Color3d(0, 1, 0);
                                    g2d.line(b2.pIni.px_x, b2.pIni.px_y, b2.pFin.px_x, b2.pFin.px_y);

                                    desenho.Controle.SwapBuffers();
                                    barras[i] = null;
                                }
                            }
                    }
                }*/
             //  Application.DoEvents();
                // -> Verificação de vazio em barra de viga (barra faltando)
             //  Progresso.Maximum = System.Convert.ToInt32(max_sBarra);

                /*
                 * double compBarras = 0;
                foreach (TLinha linha in this.linhas)
                {
              //      Application.DoEvents();
                    Progresso.Increment(1);
                    if (RefazerGrelha) continue;
                    if (!linha.LinhaEixoViga) continue;
                    if (linha.barraRigida) continue;
                    if ((Object)linha.TrechoViga == null) continue;
                    if (linha.auxiliar) continue;

                    compBarras = 0;
                    if (linha.TrechoViga.comprimento == 0)
                        linha.TrechoViga.comprimento = linha.TrechoViga.pIni.DistanceTo(linha.TrechoViga.pFin);

                    for (int j = 1; j <= max_sBarra; j++)
                    {
                        b2 = barras[j];
                        if ((Object)b2 != null)
                            if (b2.barraViga)
                                if ((Object)b2.TrechoViga == (Object)linha.TrechoViga)
                                    compBarras += b2.comprimento;
                    }

                    // .. diferença entre o comp. do trecho e a somatoria das barras for maior que 1 centímetro...
                    if (compBarras > 0 && (Math.Abs(compBarras - linha.TrechoViga.comprimento) > 1))
                    {
                   //     g2d.Color3d(1, 0, 0);
                   //     g2d.line(linha.pIni.px_x, linha.pIni.px_y, linha.pFin.px_x, linha.pFin.px_y);

                     //   desenho.Controle.SwapBuffers();

      //                  espacamentoX += .1;
                   //     espacamentoY += .1;

                      //  ContaErro++;
                        if (compBarras < linha.TrechoViga.comprimento)
                        {
                            gerenciador.HistoricoCalculo("   Viga " + linha.TrechoViga.Texto1.texto + " com barra faltando.", true);

                      //      espacamentoX += .5;
                       //     espacamentoY += .5;
                       //     RefazerGrelha = true;
                        }
                  //      ok = false;
                      //  GerarGrelha();
      
                //         ApagarBarrasContorno(linha.TrechoViga);
                   //      Gl.glPointSize(3);
                 //        CriarBarrasContorno(linha,0.1);
                    }
                }*/
       //    }
       //    return true;
            //return (!regeraMalha);
        }

    }
}
