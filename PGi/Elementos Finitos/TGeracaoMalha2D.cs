using Microsoft.TeamFoundation.Server;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WeifenLuo.WinFormsUI.Docking;
using Win32Interop.Structs;

namespace PG
{
    public class TGeracaoMalha2D
    {
        public List<LinhaVec3> arestas;
        public double tamcel;
        public List<NoCelula> nosMalha, nos_vertices;
        List<Celula> celulasMalha;
        List<vec3> nosGeometria;
        List<ArestaCelula> arestasMalha;
        public TGeracaoMalha2D(List<LinhaVec3> _arestas, List<vec3> _nos, double _tamCel)
        {
            tamcel       = _tamCel;
            arestas      = _arestas;
            nosGeometria = _nos;

            nosMalha     = new List<NoCelula>();
            celulasMalha = new List<Celula>();
            nos_vertices = new List<NoCelula>();
            arestasMalha = new List<ArestaCelula>();
        }

        double xIni = 999999999;
        double yIni = 999999999;

        double xFin = -999999999;
        double yFin = -999999999;

        int LocalizaNoMalha(double x, double y)
        {
            for (int jk = 0; jk < nosMalha.Count; jk++)
                if (Geom.Iguais(x, nosMalha[jk].x) && Geom.Iguais(y, nosMalha[jk].y))
                    return jk;

            return -1;
        }

        public void Malha(ref List<Celula> celulas)
        {
            MiniMaxCoordenadas();
            System.Windows.Forms.Application.DoEvents();
            Background();
            System.Windows.Forms.Application.DoEvents();
            NoContorno_Dentro_da_Celula2();

            ApararArestasCelulas();
            System.Windows.Forms.Application.DoEvents();

            StatusNos();
            System.Windows.Forms.Application.DoEvents();
            ClassificaStatusCelula();
            System.Windows.Forms.Application.DoEvents();
            CorrigeCelula_1_ponto_fora();
            System.Windows.Forms.Application.DoEvents();
            StatusNos();
            System.Windows.Forms.Application.DoEvents();
            ClassificaStatusCelula();
            Celulas_31_40_130();
            System.Windows.Forms.Application.DoEvents();

            AtualizacaoFinal();
            System.Windows.Forms.Application.DoEvents();

            AcertaCelulasArestasFora();
            System.Windows.Forms.Application.DoEvents();

            CalculaAreaCelulas();


            celulasMalha.RemoveAll(o => o.N1.status == 1 || o.N2.status == 1 || o.N3.status == 1 || o.N4.status == 1);
            celulasMalha.RemoveAll(o => o.area == 0);

            InsereArestas();
            System.Windows.Forms.Application.DoEvents();

            ValenciaNos();

            System.Windows.Forms.Application.DoEvents();
            Transforma_Tris_em_Quads_nas_bordas();
            System.Windows.Forms.Application.DoEvents();

            System.Windows.Forms.Application.DoEvents();
            AjustesFinais();
            System.Windows.Forms.Application.DoEvents();

            celulas = celulasMalha.ToList();
        }
        void CalculaAreaCelulas()
        {
            for (int i = 0; i < celulasMalha.Count; i++)
            {
                celulasMalha[i].CalculaArea();
            }
        }

        void AjustesFinais()
        {
            AdicionaCelulasCantosFaltantes();

            InsereNos();

            ajustaantihorario();

            for (int i = 0; i < 3; i++)
              Laplaciano();
        }
        List<NoCelula> nos_ordenados;
        void Laplaciano()
        {
            double somaX, somaY;
            for (int i = 0; i < nosvalenciaFrontEspecifico.Count; i++)
            {
                somaX = 0;
                somaY = 0;

                for (int j = 0; j < nosvalenciaFrontEspecifico[i].nos.Count; j++)
                {
                    somaX += nosvalenciaFrontEspecifico[i].nos[j].x;
                    somaY += nosvalenciaFrontEspecifico[i].nos[j].y;
                }

                if (nosvalenciaFrontEspecifico[i].no.id == 84)
                    nosvalenciaFrontEspecifico[i].no.n2_wedge = true;

                if (/*!nosvalenciaFrontEspecifico[i].no.aresta && !nosvalenciaFrontEspecifico[i].no.fim_de_linha && */nosvalenciaFrontEspecifico[i].no.status == 4)
                {
                    nosvalenciaFrontEspecifico[i].no.x = somaX / nosvalenciaFrontEspecifico[i].nos.Count;
                    nosvalenciaFrontEspecifico[i].no.y = somaY / nosvalenciaFrontEspecifico[i].nos.Count;
                }
            }
        }
        void ajustaantihorario()
        {
            //inverte pra antihorario caso o quad seja horario
            NoCelula n1, n2, n3, n4;
            for (int i = 0; i < celulasMalha.Count; i++)
            {
                List<NoCelula> nos = new List<NoCelula>();
                nos.Add(celulasMalha[i].N1); nos.Add(celulasMalha[i].N2); nos.Add(celulasMalha[i].N3); nos.Add(celulasMalha[i].N4);
                if (ehHorario(nos))
                {
                    n1 = celulasMalha[i].N4;
                    n2 = celulasMalha[i].N3;
                    n3 = celulasMalha[i].N2;
                    n4 = celulasMalha[i].N1;

                    celulasMalha[i].N1 = n1;
                    celulasMalha[i].N2 = n2;
                    celulasMalha[i].N3 = n3;
                    celulasMalha[i].N4 = n4;

                    celulasMalha[i].atuNos();
                }
            }
        }
        void Atribui_Nos_x_Linhas()
        {
            double tam_suav = 0;
            double tamlinha = 0;
            List<NoCelula> nosaresta = new List<NoCelula>();
            nos_ordenados = new List<NoCelula>();
            StringBuilder ss = new StringBuilder();
            linhas_nos = new List<linhas_x_nos>();

            for (int j = 0; j < arestas.Count; j++)
            {
                tamlinha = arestas[j].p1.DistanceTo(arestas[j].p2);

                // nosMalha.FindAll(o=>o.id_aresta == 0 && !o.fim_de_linha && !o.contorno_inicial).ToList();
                nosaresta = new List<NoCelula>();

                ss.Clear();

                for (int i = 0; i < nosMalha.Count; i++)
                {
                    if (nosMalha[i].id == 132)
                        nosMalha[i].contorno_inicial = false;

                    if (nosMalha[i].no_de_front) continue;
                    if (nosMalha[i].avulso) continue;
                    if (nosMalha[i].status != 2 && nosMalha[i].status != 3) continue;

                    if (Geom.Iguais(nosMalha[i].x, arestas[j].p1.x) && Geom.Iguais(nosMalha[i].y, arestas[j].p1.y))
                        continue;

                    if (Geom.Iguais(nosMalha[i].x, arestas[j].p2.x) && Geom.Iguais(nosMalha[i].y, arestas[j].p2.y))
                        continue;

                    if (Geom.PontoEmLinha(nosMalha[i].x, nosMalha[i].y, arestas[j].p1.x, arestas[j].p2.y, arestas[j].p2.x, arestas[j].p2.y))
                        nosaresta.Add(nosMalha[i]);

                }

                tam_suav = (tamlinha / nosaresta.Count) - 1;

                // MessageBox.Show(tam_suav.ToString());


                /*SubdivideBarra(nosaresta.Count + 1, arestas[j].p1.x, arestas[j].p1.y, 0,
                        arestas[j].p2.x, arestas[j].p2.y, 0);

                for (int k = 0; k < CoordsSubdvisao.Count; k++)
                {
                    ss.Append(CoordsSubdvisao[k].x.ToString("n2") + "  " + CoordsSubdvisao[k].y.ToString("n2") + "\r\n");
                }*/

                double dist = 9999;

                vec3 ultNo = new vec3(arestas[j].p1.x, arestas[j].p1.y, 0);
                vec3 NoAtual = new vec3(arestas[j].p1.x, arestas[j].p1.y, 0);
                NoCelula temp = new NoCelula();
                NoCelula ultimo = new NoCelula();
                ultimo.id = -999;
                nos_ordenados = new List<NoCelula>();
                for (int k = 0; k < nosaresta.Count; k++)
                {

                    dist = 9999;

                    foreach (NoCelula no in nosaresta)
                    {
                        if (nos_ordenados.Exists(o => o.id == no.id)) continue;

                        NoAtual = new vec3(no.x, no.y, 0);
                        double d2 = ultNo.DistanceTo(NoAtual);

                        if (d2 < dist)
                        {
                            dist = d2;
                            temp = no;
                        }
                    }

                    ultNo = new vec3(temp.x, temp.y, 0);
                    nos_ordenados.Add(temp);
                    ultimo = temp;
                }
                if (arestas[j].id_aresta == 38)
                {
                   // arestas[j].id_aresta = 38;
                }
                nosaresta = new List<NoCelula>();
                nos_ordenados.ForEach(o => nosaresta.Add(o));

                linhas_nos.Add(new linhas_x_nos(nosaresta, arestas[j]));

                /* for (int k = 0; k < nosaresta.Count; k++)
                 {
                     nosaresta[k].x = CoordsSubdvisao[k + 1].x;
                     nosaresta[k].y = CoordsSubdvisao[k + 1].y;
                     DesenhaTudo();
                     Controle.SwapBuffers();
                 }*/
            }
        }
        List<linhas_x_nos> linhas_nos;
        List<vec3> CoordsSubdvisao = new List<vec3>();
        void AdicionaCelulasCantosFaltantes()
        {
            // return;
            //arquivo C:\Users\DELL\Pictures\malhas\ideias\AdicionaCelulasCantosFaltantes

            /*  novo = LocalizaNoQuad(intersecx, intersecy);

              if (novo == null)
              {
                  novo = new NoCelula(intersecx, intersecy, nosMalha.Count);
                  nosMalha.Add(novo);
                  Atribui_aresta_ou_fimdelinha(ref novo);
                  if (novo.fim_de_linha)
                      novo.status = 3;
                  if (novo.aresta)
                      novo.status = 2;
              }*/


            Atribui_Nos_x_Linhas();

            for (int i = 0; i < nos_vertices.Count; i++)
            {
                NoCelula nnn = LocalizaNoQuad(nos_vertices[i].x, nos_vertices[i].y);

                if (nnn == null)
                {
                    nnn = new NoCelula(nos_vertices[i].x, nos_vertices[i].y, nosMalha.Count);
                    nosMalha.Add(nnn);
                    Atribui_aresta_ou_fimdelinha(ref nnn);
                    if (nnn.fim_de_linha)
                        nnn.status = 3;
                    if (nnn.aresta)
                        nnn.status = 2;

                    //   nnn.sel = true;
                    nnn.linhas_incidentes = new List<int>();
                    double dist = 99999;
                    NoCelula _n1 = new NoCelula(), _n2 = new NoCelula();
                    List<TLinha> linhas = new List<TLinha>();
                    for (int j = 0; j < arestas.Count; j++)
                    {
                        if (Geom.Iguais(nnn.x, arestas[j].p1.x) && Geom.Iguais(nnn.y, arestas[j].p1.y))
                            nnn.linhas_incidentes.Add(arestas[j].id_aresta);
                        if (Geom.Iguais(nnn.x, arestas[j].p2.x) && Geom.Iguais(nnn.y, arestas[j].p2.y))
                            nnn.linhas_incidentes.Add(arestas[j].id_aresta);
                    }

                    List<linhas_x_nos> l_x_n;

                    //normalmente vai ter duas linhas somente chegando ao nó...num caso mais especifico 3d tenho que repensar
                    l_x_n = linhas_nos.FindAll(o => o.linha.id_aresta == nnn.linhas_incidentes[0]);

                    if (l_x_n[0].nos_ordenados.Count <= 1)
                        continue;

                    for (int k = 0; k < l_x_n[0].nos_ordenados.Count; k++)
                    {
                        double d = l_x_n[0].nos_ordenados[k].DistanciaAte(nnn);
                        if (d < dist)
                        {
                            dist = d;
                            _n1 = l_x_n[0].nos_ordenados[k];
                        }
                    }

                    if (l_x_n[0].nos_ordenados.Count <= 1)
                        continue;

                    if (nnn.linhas_incidentes.Count < 2)
                        continue;

                    l_x_n = linhas_nos.FindAll(o => o.linha.id_aresta == nnn.linhas_incidentes[1]);

                    dist = 99999;
                    for (int k = 0; k < l_x_n[0].nos_ordenados.Count; k++)
                    {
                        double d = l_x_n[0].nos_ordenados[k].DistanciaAte(nnn);
                        if (d < dist)
                        {
                            dist = d;
                            _n2 = l_x_n[0].nos_ordenados[k];
                        }
                    }

                    NoCelula meio = new NoCelula(((nnn + _n1) / 2).x, ((nnn + _n1) / 2).y, nosMalha.Count);
                    nosMalha.Add(meio);

                    if (meio.fim_de_linha)
                        meio.status = 3;
                    if (meio.aresta)
                        meio.status = 2;

                    AdicionaCelula(ref _n1, ref meio, ref nnn, ref _n2);

                    Atribui_aresta_ou_fimdelinha(ref meio);
                    Atribui_aresta_ou_fimdelinha(ref _n1);
                    Atribui_aresta_ou_fimdelinha(ref nnn);
                    Atribui_aresta_ou_fimdelinha(ref _n2);

                    celulasMalha[celulasMalha.Count - 1].mesclou = true;
                    celulasMalha[celulasMalha.Count - 1].canto_faltante = true;
                }
            }

        }
        void AtualizacaoFinal()
        {
            foreach (Celula ce in celulasMalha)
            {
                if (ce.istat <= 22)
                    ce.avulso = true;
                else
                    ce.atuNos();
            }

            celulasMalha.ForEach(o => o.atuNos());

            for (int i = 0; i < nosMalha.Count; i++)
            {
                nosMalha[i].aresta = false;
                if (nosMalha[i].id == 1184)
                    nosMalha[i].wedge = true;

                Atribui_aresta_ou_fimdelinha(nosMalha[i]);
            }

            celulasMalha.ForEach(o => o.atuNos());
        }

        bool finalizouJuntarTri = false;
        bool finalizouCriarDoisTris = false;
        bool finalizouCriarDoisTris_2 = false;
        void Transforma_Tris_em_Quads_nas_bordas()
        {
            finalizouJuntarTri = false;
            for (int i = 0; i < 500; i++)
            {
                if (!finalizouJuntarTri)
                    JuntarDoisTris();
                else
                    break;
            }
            
            for (int k = 0; k < 1; k++)
            {
                finalizouCriarDoisTris = false;
                for (int i = 0; i < 500; i++)
                {
                    if (!finalizouCriarDoisTris)
                        CriarDoisTris();
                    else
                        break;
                }

                finalizouCriarDoisTris_2 = false;
                for (int i = 0; i < 500; i++)
                {
                    if (!finalizouCriarDoisTris_2)
                        CriarDoisTris_2();
                    else
                        break;
                }

                finalizouJuntarTri = false;
                for (int i = 0; i < 500; i++)
                {
                    if (!finalizouJuntarTri)
                        JuntarDoisTris();
                    else
                        break;
                }
            }
        }

        void Celulas_31_40_130()
        {
            List<Celula> cels_31_40_130 = celulasMalha.FindAll(o => o.istat == 31 || o.istat == 40 || o.istat == 130);
            vec3 v_intersec;
            double Ax = 0, Ay = 0, Cx = 0, Cy = 0, Dx = 0, Dy = 0, Ex = 0, Ey = 0, intersecy = 0, intersecx = 0;
            foreach (Celula ce in cels_31_40_130)
            {
                if (ce.id == 274)
                {
                    //    continue;
                    ce.id = 274;
                }

                Ax = ce.N3.x;
                Ay = ce.N3.y;
                Cx = ce.N1.x;
                Cy = ce.N1.y;

                Dx = ce.N2.x;
                Dy = ce.N2.y;
                Ex = ce.N4.x;
                Ey = ce.N4.y;
                if (Geom.calcIntersecEQU_RETA(ref Ax, ref Ay, ref Cx, ref Cy, ref Dx, ref Dy, ref Ex, ref Ey, ref intersecx, ref intersecy))
                {
                    v_intersec = new vec3(intersecx, intersecy, 0);
                    NoCelula centro = new NoCelula(intersecx, intersecy, 0);

                    triangulo t1 = new triangulo(new ArestaCelula(ce.N1, ce.N2, 0), new ArestaCelula(ce.N2, centro, 0), new ArestaCelula(centro, ce.N1, 0), 0);

                    triangulo t2 = new triangulo(new ArestaCelula(ce.N2, ce.N3, 0), new ArestaCelula(ce.N3, centro, 0), new ArestaCelula(centro, ce.N2, 0), 0);

                    triangulo t3 = new triangulo(new ArestaCelula(ce.N3, ce.N4, 0), new ArestaCelula(ce.N4, centro, 0), new ArestaCelula(centro, ce.N3, 0), 0);

                    triangulo t4 = new triangulo(new ArestaCelula(ce.N4, ce.N1, 0), new ArestaCelula(ce.N1, centro, 0), new ArestaCelula(centro, ce.N4, 0), 0);

                    NoCelula nn = new NoCelula(t1.centro.x, t1.centro.y, nosMalha.Count);
                    // nn.sel = true;
                    nosMalha.Add(nn);
                    nn = new NoCelula(t2.centro.x, t2.centro.y, nosMalha.Count);
                    // nn.sel = true;
                    nosMalha.Add(nn);
                    nn = new NoCelula(t3.centro.x, t3.centro.y, nosMalha.Count);
                    //  nn.sel = true;
                    nosMalha.Add(nn);
                    nn = new NoCelula(t4.centro.x, t4.centro.y, nosMalha.Count);
                    // nn.sel = true;
                    nosMalha.Add(nn);

                    int total_dentro = 0;
                    if (PontoEmPoligono(ref t1.centro.x, ref t1.centro.y)) ++total_dentro;
                    if (PontoEmPoligono(ref t2.centro.x, ref t2.centro.y)) ++total_dentro;
                    if (PontoEmPoligono(ref t3.centro.x, ref t3.centro.y)) ++total_dentro;
                    if (PontoEmPoligono(ref t4.centro.x, ref t4.centro.y)) ++total_dentro;

                    if (ce.id == 758)
                        ce.id = 758;

                    if (total_dentro == 0)
                        ce.avulso = true;
                    else
                    if (total_dentro == 2)
                    {
                        for (int i = 0; i < 4; i++)
                        {
                            if (ce.nos[i].status == 4) // ponto dentro do poligono
                            {
                                int opo = 0;
                                nn = ce.retNoOposto(i, ref opo).Clone();

                                // nn = ce.nos[i].Clone();
                                nn.id = nosMalha.Count;
                                nn.status = 2;
                                nn.aresta = true;
                                nosMalha.Add(nn);
                                NoCelula _n1 = new NoCelula();
                                NoCelula _n2 = new NoCelula();

                                if (i == 0)
                                {
                                    _n1 = ce.nos[1];
                                    _n2 = ce.nos[3];

                                }
                                else
                                if (i == 1)
                                {
                                    _n1 = ce.nos[0];
                                    _n2 = ce.nos[2];

                                }
                                else
                                if (i == 2)
                                {
                                    _n1 = ce.nos[1];
                                    _n2 = ce.nos[3];

                                }
                                else
                                if (i == 3)
                                {
                                    _n1 = ce.nos[0];
                                    _n2 = ce.nos[2];

                                }

                                nn.x = ((_n1 + _n2) / 2).x;
                                nn.y = ((_n1 + _n2) / 2).y;

                                Atribui_aresta_ou_fimdelinha(ref nn);

                                ce.nos[opo] = nn;

                                if (opo == 0) ce.N1 = nn;
                                if (opo == 1) ce.N2 = nn;
                                if (opo == 2) ce.N3 = nn;
                                if (opo == 3) ce.N4 = nn;
                            }


                            if (ce.nos[i].status == 1) // ponto fora do poligono
                            {
                                nn = ce.nos[i].Clone();
                                nn.id = nosMalha.Count;
                                nn.status = 2;
                                nn.aresta = true;

                                nosMalha.Add(nn);
                                NoCelula _n1 = new NoCelula();
                                NoCelula _n2 = new NoCelula();

                                if (i == 0)
                                {
                                    _n1 = ce.nos[1];
                                    _n2 = ce.nos[3];

                                }
                                else
                                if (i == 1)
                                {
                                    _n1 = ce.nos[0];
                                    _n2 = ce.nos[2];

                                }
                                else
                                if (i == 2)
                                {
                                    _n1 = ce.nos[1];
                                    _n2 = ce.nos[3];

                                }
                                else
                                if (i == 3)
                                {
                                    _n1 = ce.nos[0];
                                    _n2 = ce.nos[2];

                                }

                                nn.x = ((_n1 + _n2) / 2).x;
                                nn.y = ((_n1 + _n2) / 2).y;

                                Atribui_aresta_ou_fimdelinha(ref nn);

                                ce.nos[i] = nn;

                                if (i == 0) ce.N1 = nn;
                                if (i == 1) ce.N2 = nn;
                                if (i == 2) ce.N3 = nn;
                                if (i == 3) ce.N4 = nn;
                            }
                        }

                        // ce.istat = 220;
                    }
                    else
                    if (total_dentro == 1 && ce.istat == 130)
                    {
                        for (int i = 0; i < 4; i++)
                        {
                            if (ce.nos[i].status == 4) // ponto dentro do poligono
                            {
                                int opo = 0;
                                nn = ce.retNoOposto(i, ref opo).Clone();

                                // nn = ce.nos[i].Clone();
                                nn.id = nosMalha.Count;
                                nosMalha.Add(nn);
                                NoCelula _n1 = new NoCelula();
                                NoCelula _n2 = new NoCelula();

                                if (i == 0)
                                {
                                    _n1 = ce.nos[1];
                                    _n2 = ce.nos[3];

                                }
                                else
                                if (i == 1)
                                {
                                    _n1 = ce.nos[0];
                                    _n2 = ce.nos[2];

                                }
                                else
                                if (i == 2)
                                {
                                    _n1 = ce.nos[1];
                                    _n2 = ce.nos[3];

                                }
                                else
                                if (i == 3)
                                {
                                    _n1 = ce.nos[0];
                                    _n2 = ce.nos[2];

                                }

                                nn.x = ((_n1 + _n2) / 2).x;
                                nn.y = ((_n1 + _n2) / 2).y;

                                Atribui_aresta_ou_fimdelinha(ref nn);

                                ce.nos[opo] = nn;

                                if (opo == 0) ce.N1 = nn;
                                if (opo == 1) ce.N2 = nn;
                                if (opo == 2) ce.N3 = nn;
                                if (opo == 3) ce.N4 = nn;
                            }


                            if (ce.nos[i].status == 1) // ponto fora do poligono
                            {
                                nn = ce.nos[i].Clone();
                                nn.id = nosMalha.Count;
                                nosMalha.Add(nn);
                                NoCelula _n1 = new NoCelula();
                                NoCelula _n2 = new NoCelula();

                                if (i == 0)
                                {
                                    _n1 = ce.nos[1];
                                    _n2 = ce.nos[3];

                                }
                                else
                                if (i == 1)
                                {
                                    _n1 = ce.nos[0];
                                    _n2 = ce.nos[2];

                                }
                                else
                                if (i == 2)
                                {
                                    _n1 = ce.nos[1];
                                    _n2 = ce.nos[3];

                                }
                                else
                                if (i == 3)
                                {
                                    _n1 = ce.nos[0];
                                    _n2 = ce.nos[2];

                                }

                                nn.x = ((_n1 + _n2) / 2).x;
                                nn.y = ((_n1 + _n2) / 2).y;

                                Atribui_aresta_ou_fimdelinha(ref nn);

                                ce.nos[i] = nn;

                                if (i == 0) ce.N1 = nn;
                                if (i == 1) ce.N2 = nn;
                                if (i == 2) ce.N3 = nn;
                                if (i == 3) ce.N4 = nn;
                            }
                        }
                    }
                }
            }
        }

        List<NoCelula> nosaresta;
        int arestasFora = 0;
        int arestasDentro = 0;
        void ApararArestasCelulas()
        {
            double tolerancia = 0.01;
            int idcel = 0;
            double menor = 99999;
            double dist;
            //    return; 
            NoCelula naux = new NoCelula();
            nosaresta = nosMalha.FindAll(o => o.contorno_inicial && !o.avulso);
            double Ax = 0, Ay = 0, Cx = 0, Cy = 0, Dx = 0, Dy = 0, Ex = 0, Ey = 0, intersecy = 0, intersecx = 0;
            foreach (Celula ce in celulasMalha)
            {
                if (ce.id == 750)
                    ce.in_j = -1;
                //       if (ce.avulso) continue;
                if (ce.temp) continue;
                Ax = ce.N1.x;
                Ay = ce.N1.y;
                Cx = ce.N2.x;
                Cy = ce.N2.y;
             
                foreach (LinhaVec3 l in arestas)
                {
                    Dx = l.p1.x;
                    Dy = l.p1.y;
                    Ex = l.p2.x;
                    Ey = l.p2.y;
                  
                    if (Geom.calcIntersecEQU_RETA(ref Ax, ref Ay, ref Cx, ref Cy, ref Dx, ref Dy, ref Ex, ref Ey, ref intersecx, ref intersecy))
                    {
                        //  if (ce.N1.status == 2 || ce.N1.status == 3 || ce.N2.status == 2 || ce.N2.status == 3)
                        //    continue;

                        if ((Geom.Iguais(ce.N1.x, intersecx, tolerancia) && Geom.Iguais(ce.N1.y, intersecy, tolerancia)) ||
                            (Geom.Iguais(ce.N2.x, intersecx, tolerancia) && Geom.Iguais(ce.N2.y, intersecy, tolerancia)))
                            continue;

                        NoCelula v1 = new NoCelula(intersecx, intersecy, 0);
                        menor = 99999;
                        menor = 99999;

                        if (ce.N1.DistanciaAte(v1) < ce.N2.DistanciaAte(v1))
                        {
                            naux = ce.N1;
                        }
                        else
                            naux = ce.N2;

                        if (naux.status == 2 || naux.status == 3)
                            continue;

                        naux.x = intersecx;
                        naux.y = intersecy;
                        naux.moveu = true;
                        naux.id_aresta = l.id_aresta;
                        naux.aresta = true;
                        naux.no_de_intersecao = true;
                        naux.status = 2;
                        //   ce.temp = true;

                        //      ce.atuNos();
                    }
                }

                Ax = ce.N2.x;
                Ay = ce.N2.y;
                Cx = ce.N3.x;
                Cy = ce.N3.y;
          
                foreach (LinhaVec3 l in arestas)
                {
                    Dx = l.p1.x;
                    Dy = l.p1.y;
                    Ex = l.p2.x;
                    Ey = l.p2.y;

                    if (Geom.calcIntersecEQU_RETA(ref Ax, ref Ay, ref Cx, ref Cy, ref Dx, ref Dy, ref Ex, ref Ey, ref intersecx, ref intersecy))
                    {
                        //     if (ce.N3.status == 2 || ce.N3.status == 3 || ce.N2.status == 2 || ce.N2.status == 3)
                        //       continue;

                        if ((Geom.Iguais(ce.N3.x, intersecx, tolerancia) && Geom.Iguais(ce.N3.y, intersecy, tolerancia)) ||
                            (Geom.Iguais(ce.N2.x, intersecx, tolerancia) && Geom.Iguais(ce.N2.y, intersecy, tolerancia)))
                            continue;

                        NoCelula v1 = new NoCelula(intersecx, intersecy, 0);
                        menor = 99999;
                        menor = 99999;

                        if (ce.N3.DistanciaAte(v1) < ce.N2.DistanciaAte(v1))
                        {
                            naux = ce.N3;
                        }
                        else
                            naux = ce.N2;

                        if (naux.status == 2 || naux.status == 3)
                            continue;

                        naux.x = intersecx;
                        naux.y = intersecy;
                        naux.moveu = true;
                        naux.id_aresta = l.id_aresta;
                        naux.aresta = true;
                        naux.no_de_intersecao = true;
                        naux.status = 2;
                        //        ce.temp = true;

                        //   ce.atuNos();
                    }
                }

                Ax = ce.N3.x;
                Ay = ce.N3.y;
                Cx = ce.N4.x;
                Cy = ce.N4.y;
                foreach (LinhaVec3 l in arestas)
                {
                    Dx = l.p1.x;
                    Dy = l.p1.y;
                    Ex = l.p2.x;
                    Ey = l.p2.y;

                    if (Geom.calcIntersecEQU_RETA(ref Ax, ref Ay, ref Cx, ref Cy, ref Dx, ref Dy, ref Ex, ref Ey, ref intersecx, ref intersecy))
                    {
                        //  if (ce.N3.status == 2 || ce.N3.status == 3 || ce.N4.status == 2 || ce.N4.status == 3)
                        //     continue;

                        if ((Geom.Iguais(ce.N3.x, intersecx, tolerancia) && Geom.Iguais(ce.N3.y, intersecy, tolerancia)) ||
                            (Geom.Iguais(ce.N4.x, intersecx, tolerancia) && Geom.Iguais(ce.N4.y, intersecy, tolerancia)))
                            continue;

                        NoCelula v1 = new NoCelula(intersecx, intersecy, 0);
                        menor = 99999;
                        menor = 99999;

                        if (ce.N3.DistanciaAte(v1) < ce.N4.DistanciaAte(v1))
                        {
                            naux = ce.N3;
                        }
                        else
                            naux = ce.N4;

                        if (naux.status == 2 || naux.status == 3)
                            continue;

                        naux.x = intersecx;
                        naux.y = intersecy;
                        naux.moveu = true;
                        naux.id_aresta = l.id_aresta;
                        naux.aresta = true;
                        naux.no_de_intersecao = true;
                        naux.status = 2;
                        //     ce.temp = true;

                        //     ce.atuNos();
                    }
                }


                Ax = ce.N4.x;
                Ay = ce.N4.y;
                Cx = ce.N1.x;
                Cy = ce.N1.y;
                foreach (LinhaVec3 l in arestas)
                {
                    Dx = l.p1.x;
                    Dy = l.p1.y;
                    Ex = l.p2.x;
                    Ey = l.p2.y;

                    if (Geom.calcIntersecEQU_RETA(ref Ax, ref Ay, ref Cx, ref Cy, ref Dx, ref Dy, ref Ex, ref Ey, ref intersecx, ref intersecy))
                    {
                        //      if (ce.N4.status == 2 || ce.N4.status == 3 || ce.N1.status == 2 || ce.N1.status == 3)
                        //          continue;

                        if ((Geom.Iguais(ce.N4.x, intersecx, tolerancia) && Geom.Iguais(ce.N4.y, intersecy, tolerancia)) ||
                            (Geom.Iguais(ce.N1.x, intersecx, tolerancia) && Geom.Iguais(ce.N1.y, intersecy, tolerancia)))
                            continue;

                        NoCelula v1 = new NoCelula(intersecx, intersecy, 0);
                        menor = 99999;
                        menor = 99999;

                        if (ce.N4.DistanciaAte(v1) < ce.N1.DistanciaAte(v1))
                        {
                            naux = ce.N4;
                        }
                        else
                            naux = ce.N1;

                        if (naux.status == 2 || naux.status == 3)
                            continue;

                        naux.x = intersecx;
                        naux.y = intersecy;
                        naux.moveu = true;
                        naux.id_aresta = l.id_aresta;
                        naux.aresta = true;
                        naux.no_de_intersecao = true;
                        naux.status = 2;
                        //      ce.temp = true;

                        //        ce.atuNos();
                    }
                }

            }
        }

        void AcertaCelulasArestasFora()
        {
            //arquivo C:\Users\DELL\Pictures\malhas\ideias\AcertaCelulasArestasFora

            double tolerancia = 0.01;
            //    return;
            nosaresta = nosMalha.FindAll(o => o.contorno_inicial && !o.avulso);
            double Ax = 0, Ay = 0, Cx = 0, Cy = 0, Dx = 0, Dy = 0, Ex = 0, Ey = 0, intersecy = 0, intersecx = 0, menor;
            NoCelula naux, novo;

            List<Celula> cels = celulasMalha.FindAll(o => /*o.id == 797 && */!o.avulso/* && !o.temp */&& (o.istat != 400));

            foreach (Celula ce in cels)
            {
                if (ce.id == 750)
                    ce.in_j = -1;

                if (ce.avulso) continue;
                if (ce.temp) continue;

                foreach (LinhaVec3 l in arestas)
                {
                    Ax = ce.N1.x;
                    Ay = ce.N1.y;
                    Cx = ce.N2.x;
                    Cy = ce.N2.y;

                    Dx = l.p1.x;
                    Dy = l.p1.y;
                    Ex = l.p2.x;
                    Ey = l.p2.y;

                    if (ce.N1.status == 4 || ce.N2.status == 4)
                        if (Geom.calcIntersecEQU_RETA(ref Ax, ref Ay, ref Cx, ref Cy, ref Dx, ref Dy, ref Ex, ref Ey, ref intersecx, ref intersecy))
                        {
                            //  if (ce.N1.status == 2 || ce.N1.status == 3 || ce.N2.status == 2 || ce.N2.status == 3)
                            //    continue;

                            if ((Geom.Iguais(ce.N1.x, intersecx, tolerancia) && Geom.Iguais(ce.N1.y, intersecy, tolerancia)) ||
                                (Geom.Iguais(ce.N2.x, intersecx, tolerancia) && Geom.Iguais(ce.N2.y, intersecy, tolerancia)))
                                continue;

                            novo = LocalizaNoQuad(intersecx, intersecy);

                            if (novo == null)
                            {
                                novo = new NoCelula(intersecx, intersecy, nosMalha.Count);
                                nosMalha.Add(novo);
                                Atribui_aresta_ou_fimdelinha(ref novo);
                                if (novo.fim_de_linha)
                                    novo.status = 3;
                                if (novo.aresta)
                                    novo.status = 2;
                            }

                            if (ce.N1.status == 4) // se dentro do poligono
                                ce.N2 = novo;
                            else
                                ce.N1 = novo;

                            ce.atuNos();
                        }
                }

                foreach (LinhaVec3 l in arestas)
                {
                    Ax = ce.N2.x;
                    Ay = ce.N2.y;
                    Cx = ce.N3.x;
                    Cy = ce.N3.y;

                    Dx = l.p1.x;
                    Dy = l.p1.y;
                    Ex = l.p2.x;
                    Ey = l.p2.y;

                    if (ce.N2.status == 4 || ce.N3.status == 4)
                        if (Geom.calcIntersecEQU_RETA(ref Ax, ref Ay, ref Cx, ref Cy, ref Dx, ref Dy, ref Ex, ref Ey, ref intersecx, ref intersecy))
                        {
                            //     if (ce.N3.status == 2 || ce.N3.status == 3 || ce.N2.status == 2 || ce.N2.status == 3)
                            //       continue;

                            if ((Geom.Iguais(ce.N3.x, intersecx, tolerancia) && Geom.Iguais(ce.N3.y, intersecy, tolerancia)) ||
                                (Geom.Iguais(ce.N2.x, intersecx, tolerancia) && Geom.Iguais(ce.N2.y, intersecy, tolerancia)))
                                continue;

                            novo = LocalizaNoQuad(intersecx, intersecy);

                            if (novo == null)
                            {
                                novo = new NoCelula(intersecx, intersecy, nosMalha.Count);
                                nosMalha.Add(novo);
                                Atribui_aresta_ou_fimdelinha(ref novo);
                                if (novo.fim_de_linha)
                                    novo.status = 3;
                                if (novo.aresta)
                                    novo.status = 2;
                            }

                            if (ce.N2.status == 4) // se dentro do poligono
                                ce.N3 = novo;
                            else
                                ce.N2 = novo;

                            ce.atuNos();
                        }
                }

                foreach (LinhaVec3 l in arestas)
                {
                    Dx = l.p1.x;
                    Dy = l.p1.y;
                    Ex = l.p2.x;
                    Ey = l.p2.y;

                    Ax = ce.N3.x;
                    Ay = ce.N3.y;
                    Cx = ce.N4.x;
                    Cy = ce.N4.y;

                    if (ce.N3.status == 4 || ce.N4.status == 4)
                        if (Geom.calcIntersecEQU_RETA(ref Ax, ref Ay, ref Cx, ref Cy, ref Dx, ref Dy, ref Ex, ref Ey, ref intersecx, ref intersecy))
                        {
                            //  if (ce.N3.status == 2 || ce.N3.status == 3 || ce.N4.status == 2 || ce.N4.status == 3)
                            //     continue;

                            if ((Geom.Iguais(ce.N3.x, intersecx, tolerancia) && Geom.Iguais(ce.N3.y, intersecy, tolerancia)) ||
                                (Geom.Iguais(ce.N4.x, intersecx, tolerancia) && Geom.Iguais(ce.N4.y, intersecy, tolerancia)))
                                continue;

                            novo = LocalizaNoQuad(intersecx, intersecy);

                            if (novo == null)
                            {
                                novo = new NoCelula(intersecx, intersecy, nosMalha.Count);
                                nosMalha.Add(novo);
                                Atribui_aresta_ou_fimdelinha(ref novo);
                                if (novo.fim_de_linha)
                                    novo.status = 3;
                                if (novo.aresta)
                                    novo.status = 2;
                            }

                            if (ce.N3.status == 4) // se dentro do poligono
                                ce.N4 = novo;
                            else
                                ce.N3 = novo;

                            ce.atuNos();
                        }
                }

                foreach (LinhaVec3 l in arestas)
                {
                    Ax = ce.N4.x;
                    Ay = ce.N4.y;
                    Cx = ce.N1.x;
                    Cy = ce.N1.y;

                    Dx = l.p1.x;
                    Dy = l.p1.y;
                    Ex = l.p2.x;
                    Ey = l.p2.y;

                    if (ce.N1.status == 4 || ce.N4.status == 4)
                        if (Geom.calcIntersecEQU_RETA(ref Ax, ref Ay, ref Cx, ref Cy, ref Dx, ref Dy, ref Ex, ref Ey, ref intersecx, ref intersecy))
                        {
                            //      if (ce.N4.status == 2 || ce.N4.status == 3 || ce.N1.status == 2 || ce.N1.status == 3)
                            //          continue;

                            if ((Geom.Iguais(ce.N4.x, intersecx, tolerancia) && Geom.Iguais(ce.N4.y, intersecy, tolerancia)) ||
                                (Geom.Iguais(ce.N1.x, intersecx, tolerancia) && Geom.Iguais(ce.N1.y, intersecy, tolerancia)))
                                continue;

                            novo = LocalizaNoQuad(intersecx, intersecy);

                            if (novo == null)
                            {
                                novo = new NoCelula(intersecx, intersecy, nosMalha.Count);
                                nosMalha.Add(novo);
                                Atribui_aresta_ou_fimdelinha(ref novo);
                                if (novo.fim_de_linha)
                                    novo.status = 3;
                                if (novo.aresta)
                                    novo.status = 2;
                            }

                            if (ce.N1.status == 4) // se dentro do poligono
                                ce.N4 = novo;
                            else
                                ce.N1 = novo;

                            ce.atuNos();
                        }
                }
            }

            StatusNos();
            ClassificaStatusCelula();
            CorrigeCelula_1_ponto_fora();
        }

        void Background()
        {
            xIni -= (1.1 * tamcel);
            yIni -= (1.1 * tamcel);
            xFin += (1.1 * tamcel);
            yFin += (1.1 * tamcel);

            double ultx = xIni, ulty = yIni;
            double qtd_x = ((xFin - xIni) / tamcel) + 2;
            double qtd_y = ((yFin - yIni) / tamcel) + 2;
            NoCelula n1, n2, n3, n4;

            for (int i = 0; i < qtd_y; i++)
            {
                ultx = xIni;
                for (int j = 0; j < qtd_x; j++)
                {
                    double x_ = ultx;
                    double y_ = ulty;
                   
                    //int idNo = LocalizaNoMalha(x_, y_);
                    n1 = nosMalha.Find(o => o.x == x_ && o.y == y_);
                    if (n1 == null)
                    {
                        n1 = new NoCelula(x_, y_, nosMalha.Count);
                        nosMalha.Add(n1);
                    }
                 //   else
                   //     n1 = nosMalha[idNo];

                    n2 = nosMalha.Find(o => o.x == x_ + tamcel && o.y == y_);
                    // idNo = LocalizaNoMalha(x_ + tamcel, y_);
                    if (n2 == null)
                    {
                        n2 = new NoCelula(x_ + tamcel, y_, nosMalha.Count);
                        nosMalha.Add(n2);
                    }
                 //   else
                 //       n2 = nosMalha[idNo];

                    n3 = nosMalha.Find(o => o.x == x_ + tamcel && o.y == y_ + tamcel);
                    // idNo = LocalizaNoMalha(x_ + tamcel, y_ + tamcel);
                    if (n3 == null)
                    {
                        n3 = new NoCelula(x_ + tamcel, y_ + tamcel, nosMalha.Count);
                        nosMalha.Add(n3);
                    }
                   // else
                   //     n3 = nosMalha[idNo];

                    n4 = nosMalha.Find(o => o.x == x_ && o.y == y_ + tamcel);
                    // idNo = LocalizaNoMalha(x_ , y_ + tamcel);
                    if (n4 == null)
                    {
                        n4 = new NoCelula(x_, y_ + tamcel, nosMalha.Count);
                        nosMalha.Add(n4);
                    }
                   // else
                   //     n4 = nosMalha[idNo];

                    ultx += tamcel;

                    if (!PontoEmPoligono(ref n1.x, ref n1.y) &&
                        !PontoEmPoligono(ref n2.x, ref n2.y) &&
                        !PontoEmPoligono(ref n3.x, ref n3.y) &&
                        !PontoEmPoligono(ref n4.x, ref n4.y))
                        continue;

                    AdicionaCelula(ref n1, ref n2, ref n3, ref n4);
                }
                ulty += tamcel;
            }
        }

        public void MiniMaxCoordenadas()
        {
            for (int i = 0; i < nosGeometria.Count; i++)
            {
                nos_vertices.Add(new NoCelula(nosGeometria[i].x, nosGeometria[i].y, 0));

                if (nos_vertices[i].x < xIni)
                    xIni = nos_vertices[i].x;

                if (nos_vertices[i].y < yIni)
                    yIni = nos_vertices[i].y;

                if (nos_vertices[i].x > xFin)
                    xFin = nos_vertices[i].x;

                if (nos_vertices[i].y > yFin)
                    yFin = nos_vertices[i].y;
            }
        }
        public bool ehHorario(List<NoCelula> vertices)
        {
            double sum = 0.0;
            for (int i = 0; i < vertices.Count; i++)
            {
                vec3 v1 = new vec3(vertices[i].x, vertices[i].y, 0);
                vec3 v2 = new vec3(vertices[(i + 1) % vertices.Count].x, vertices[(i + 1) % vertices.Count].y, 0);
                sum += (v2.x - v1.x) * (v2.y + v1.y);
            }
            return sum > 0.0;
        }
        NoCelula LocalizaNoQuad(double xi, double yi)
        {
            for (int jk = 0; jk < nosMalha.Count; jk++)
                if ((Geom.Iguais(xi, nosMalha[jk].x, 0.01f) && Geom.Iguais(yi, nosMalha[jk].y, 0.01f)))
                    return nosMalha[jk];

            return null;
        }
        void criaNo_entre(Celula ce, int n1, int n2, int n_novo, ref NoCelula no)
        {
            //  ce.sel = true;
            NoCelula meio = (ce.nos[n1] + ce.nos[n2]) / 2;

            NoCelula nn = meio.Clone();
            nn.id = nosMalha.Count;
            Atribui_aresta_ou_fimdelinha(ref nn);

            ce.nos[n_novo] = nn;
            no = nn;
            nosMalha.Add(nn);

            ce.AngulosInternos();
        }


        void Verifica_ArestasFora_ArestasDentro(ref List<NoCelula> nos, ref int arestasDentro, ref int arestasFora)
        {
            arestasDentro = 0;
            arestasFora = 0;
            NoCelula meio = (nos[1] + nos[0]) / 2;
            if (!PontoEmPoligono(ref meio.x, ref meio.y) && (!pontoEmLinha(ref meio.x, ref meio.y) && (!pontoFimLinha(ref meio.x, ref meio.y))))
                arestasFora++;
            meio = (nos[2] + nos[1]) / 2;
            if (!PontoEmPoligono(ref meio.x, ref meio.y) && (!pontoEmLinha(ref meio.x, ref meio.y) && (!pontoFimLinha(ref meio.x, ref meio.y))))
                arestasFora++;
            meio = (nos[3] + nos[2]) / 2;
            if (!PontoEmPoligono(ref meio.x, ref meio.y) && (!pontoEmLinha(ref meio.x, ref meio.y) && (!pontoFimLinha(ref meio.x, ref meio.y))))
                arestasFora++;
            meio = (nos[0] + nos[3]) / 2;
            if (!PontoEmPoligono(ref meio.x, ref meio.y) && (!pontoEmLinha(ref meio.x, ref meio.y) && (!pontoFimLinha(ref meio.x, ref meio.y))))
                arestasFora++;


            meio = (nos[1] + nos[0]) / 2;
            if (PontoEmPoligono(ref meio.x, ref meio.y) || (pontoEmLinha(ref meio.x, ref meio.y) || (pontoFimLinha(ref meio.x, ref meio.y))))
                arestasDentro++;
            meio = (nos[2] + nos[1]) / 2;
            if (PontoEmPoligono(ref meio.x, ref meio.y) || (pontoEmLinha(ref meio.x, ref meio.y) || (pontoFimLinha(ref meio.x, ref meio.y))))
                arestasDentro++;
            meio = (nos[3] + nos[2]) / 2;
            if (PontoEmPoligono(ref meio.x, ref meio.y) || (pontoEmLinha(ref meio.x, ref meio.y) || (pontoFimLinha(ref meio.x, ref meio.y))))
                arestasDentro++;
            meio = (nos[0] + nos[3]) / 2;
            if (PontoEmPoligono(ref meio.x, ref meio.y) || (pontoEmLinha(ref meio.x, ref meio.y) || (pontoFimLinha(ref meio.x, ref meio.y))))
                arestasDentro++;
        }

        void Atribui_aresta_ou_fimdelinha(ref NoCelula nn)
        {
            nn.aresta = false;
            foreach (LinhaVec3 l in arestas)
            {
                if (Geom.PontoEmLinha3(nn.x, nn.y, l.p1.x, l.p1.y, l.p2.x, l.p2.y, 0.01))
                {
                    nn.aresta = true;
                    nn.id_aresta = l.id_aresta;
                }

                if (Geom.Iguais(nn.x, l.p1.x, 0.01) && Geom.Iguais(nn.y, l.p1.y, 0.01))
                {
                    nn.fim_de_linha = true;
                    nn.id_aresta = -1;
                }

                if (Geom.Iguais(nn.x, l.p2.x, 0.01) && Geom.Iguais(nn.y, l.p2.y, 0.01))
                {
                    nn.fim_de_linha = true;
                    nn.id_aresta = -1;
                }
            }
        }
        void Atribui_aresta_ou_fimdelinha(NoCelula nn)
        {
            nn.aresta = false;
     
            foreach (LinhaVec3 l in arestas)
            {     
                if (Geom.PontoEmLinha(nn.x, nn.y, l.p1.x, l.p1.y, l.p2.x, l.p2.y))
                {
                    nn.aresta = true;
                    nn.id_aresta = l.id_aresta;
                }
                if (Geom.Iguais(nn.x, l.p1.x, 0.1) && Geom.Iguais(nn.y, l.p1.y, 0.1))
                {
                    nn.fim_de_linha = true;
                    nn.id_aresta = -1;
                }
                if (Geom.Iguais(nn.x, l.p2.x, 0.1) && Geom.Iguais(nn.y, l.p2.y, 0.1))
                {
                    nn.fim_de_linha = true;
                    nn.id_aresta = -1;
                }
            }
        }

        bool pontoEmLinha(ref double x, ref double y)
        {
            foreach (LinhaVec3 l in arestas)
                if (Geom.PontoEmLinha(x, y, l.p1.x, l.p1.y, l.p2.x, l.p2.y))
                    return true;

            return false;
        }

        bool pontoFimLinha(ref double x, ref double y)
        {
            foreach (LinhaVec3 l in arestas)
            {
                if (Geom.Iguais(x, l.p1.x, 0.1) && Geom.Iguais(y, l.p1.y, 0.1))
                    return true;
                if (Geom.Iguais(x, l.p2.x, 0.1) && Geom.Iguais(y, l.p2.y, 0.1))
                    return true;
            }

            return false;
        }

        void StatusNos()
        {
            foreach (NoCelula no in nosMalha)
            {
                NoCelula _n = no;

                if (no.id == 5590)
                    no.n2_wedge = true;

                Atribui_aresta_ou_fimdelinha(ref _n);

                if (no.fim_de_linha)
                    no.status = 3;
                else
                if (no.aresta)
                    no.status = 2;
                else
                if (PontoEmPoligono(ref _n.x, ref _n.y))
                    no.status = 4;
                else
                if (!PontoEmPoligono(ref _n.x, ref _n.y))
                    no.status = 1;
            }

        }
        List<Celula> celulasIntersecao = new List<Celula>();
        void NoContorno_Dentro_da_Celula2()
        {
            int idcel = 0;
            double menor = 99999;
            double dist;
            NoCelula naux = new NoCelula();
            int nro_no = -1;
            bool tem = false;

            celulasIntersecao = new List<Celula>();

            foreach (NoCelula nn in nos_vertices)
            {
                //     if (ii == 45) break;
                //     ii++;
                celulasMalha.ForEach(o => o.sel = false);
                //  DesenhaTudo();
                //   Controle.SwapBuffers(); 

                bool dentro = false;

                // if (checkBox1.Checked)
                //     dentro = NoEmCelula2(ref nn.x, ref nn.y, ref idcel);
                //  else
                {
                    //if (!nn.aresta)
                    //  dentro = NoEmCelula3(ref nn.x, ref nn.y, ref idcel);
                    dentro = IsPointInPolygon4(nn.x, nn.y, ref idcel);
                }

                if (dentro)
                {
                    tem = true;
                    Celula cel = celulasMalha.First(o => o.id == idcel);

                //    cel.sel = true;

                    cel.dentro = true;
                    if (!celulasIntersecao.Exists(o => o.id == cel.id))
                        celulasIntersecao.Add(cel);

                    menor = 99999;
                    for (int i = 0; i < 4; i++)
                    {
                        dist = cel.nos[i].DistanciaAte(nn);
                        if (dist < menor && !cel.nos[i].moveu && cel.nos[i].status != 3)
                        {
                            menor = dist;
                            naux = cel.nos[i];
                            nro_no = i;
                        }
                    }

                    naux.status = 3;

                    //  naux.contorno_inicial = true;
                    //   if (naux.id == 104)
                    //       naux.moveu = false;

                    naux.moveu = true;
                    naux.aresta = true;

                    //  cel.avulso = true;
                    //   continue;

                    if (nro_no == 0)
                    {
                        cel.N1.x = nn.x;
                        cel.N1.y = nn.y;
                        cel.N1.id_aresta = nn.id_aresta;
                    }
                    else
                    if (nro_no == 1)
                    {
                        cel.N2.x = nn.x;
                        cel.N2.y = nn.y;
                        cel.N2.id_aresta = nn.id_aresta;
                    }
                    else
                    if (nro_no == 2)
                    {
                        cel.N3.id_aresta = nn.id_aresta;
                        cel.N3.x = nn.x;
                        cel.N3.y = nn.y;
                    }
                    else
                                if (nro_no == 3)
                    {
                        cel.N4.id_aresta = nn.id_aresta;
                        cel.N4.x = nn.x;
                        cel.N4.y = nn.y;
                    }

                    cel.atuNos();
                    //cel.AngulosInternos();
                 //   cel.sel = true;
                    //   DesenhaTudo();
                    //    Controle.SwapBuffers();
                    // break;

                }
            }
            //    if (tem)
            //  NoContorno_Dentro_da_Celula();
        }
        public bool IsPointInPolygon4(double x, double y, ref int cel)
        {
            PointF[] polygonCelula = new PointF[4];

            bool result = false;

            for (int k = 0; k < celulasMalha.Count; k++)
            {
               /* if (celulasMalha[k].id == 16) //|| i == 298)
                {
                    polygonCelula[0].X = (float)celulasMalha[k].N1.x;
                }*/

                polygonCelula[0].X = (float)celulasMalha[k].N1.x;
                polygonCelula[0].Y = (float)celulasMalha[k].N1.y;
                polygonCelula[1].X = (float)celulasMalha[k].N2.x;
                polygonCelula[1].Y = (float)celulasMalha[k].N2.y;
                polygonCelula[2].X = (float)celulasMalha[k].N3.x;
                polygonCelula[2].Y = (float)celulasMalha[k].N3.y;
                polygonCelula[3].X = (float)celulasMalha[k].N4.x;
                polygonCelula[3].Y = (float)celulasMalha[k].N4.y;

                int j = polygonCelula.Count() - 1;
                for (int i = 0; i < polygonCelula.Count(); i++)
                {
                    if (polygonCelula[i].Y < y && polygonCelula[j].Y >= y || polygonCelula[j].Y < y && polygonCelula[i].Y >= y)
                    {
                        if (polygonCelula[i].X + (y - polygonCelula[i].Y) / (polygonCelula[j].Y - polygonCelula[i].Y) * (polygonCelula[j].X - polygonCelula[i].X) < x)
                        {
                            result = !result;
                        }
                    }
                    j = i;
                }
                if (result)
                {
                    cel = celulasMalha[k].id;
                    break;
                }

            }
            return result;

        }
        void ClassificaStatusCelula()
        {
            int iperm = 0;
            foreach (Celula ce in celulasMalha)
            {
                iperm = 0;
                for (int i = 0; i < 4; i++)
                {
                    if (ce.nos[i].status == 1) // ponto fora do poligono
                        iperm += 1;
                    else
                    if (ce.nos[i].status == 2) // ponto em cima de uma aresta
                        iperm += 10;
                    else
                    if (ce.nos[i].status == 3) // vertice do contorno inicial
                        iperm += 10;
                    else
                    if (ce.nos[i].status == 4) // ponto dentro do poligono
                        iperm += 100;
                }

                ce.istat = iperm;

                ce.atuNos();
            }
        }

        bool existeAresta(NoCelula n1, NoCelula n2)
        {
            for (int i = 0; i < arestasMalha.Count; i++)
            {
                if (((Object)(n1) == (Object)arestasMalha[i].n1) &&
                    ((Object)(n2) == (Object)arestasMalha[i].n2))
                    return true;

                if (((Object)(n1) == (Object)arestasMalha[i].n2) &&
                    ((Object)(n2) == (Object)arestasMalha[i].n1))
                    return true;
            }

            return false;
        }

        ArestaCelula retAresta(NoCelula n1, NoCelula n2)
        {
            for (int i = 0; i < arestasMalha.Count; i++)
            {
                if (((Object)(n1) == (Object)arestasMalha[i].n1) &&
                    ((Object)(n2) == (Object)arestasMalha[i].n2))
                    return arestasMalha[i];

                if (((Object)(n1) == (Object)arestasMalha[i].n2) &&
                    ((Object)(n2) == (Object)arestasMalha[i].n1))
                    return arestasMalha[i];
            }

            return null;
        }

        void CorrigeCelula_1_ponto_fora() // istat = 121
        {
            List<Celula> cels_121 = celulasMalha.FindAll(o => o.istat == 121);
            foreach (Celula ce in cels_121)
            {
                if (ce.id == 274)
                {
                    //     continue;
                    ce.id = 274;
                }

                for (int i = 0; i < 4; i++)
                {
                    if (ce.nos[i].status == 1) // ponto fora do poligono
                    {
                        NoCelula nn = ce.nos[i].Clone();
                        nn.id = nosMalha.Count;
                        //  nn.status = 2;
                        nosMalha.Add(nn);
                        NoCelula _n1 = new NoCelula();
                        NoCelula _n2 = new NoCelula();

                        List<NoCelula> nos_ = ce.nos.FindAll(n_ => n_.status != 4 && n_.status != 1);

                        nn.x = ((nos_[0] + nos_[1]) / 2).x;
                        nn.y = ((nos_[0] + nos_[1]) / 2).y;

                        Atribui_aresta_ou_fimdelinha(ref nn);

                        ce.nos[i] = nn;

                        if (i == 0) ce.N1 = nn;
                        if (i == 1) ce.N2 = nn;
                        if (i == 2) ce.N3 = nn;
                        if (i == 3) ce.N4 = nn;
                    }
                }

                ce.atuNos();
            }

            StatusNos();
        }

        List<ArestaCelula> arestas_n1, arestas_n2, arestas_n3, arestas_n4;
        Celula AdicionaCelula(ref NoCelula no1, ref NoCelula no2, ref NoCelula no3, ref NoCelula no4, bool temp = false)
        {
            List<NoCelula> nos = new List<NoCelula>();
            nos.Add(no1); nos.Add(no2); nos.Add(no3); nos.Add(no4);
            if (ehHorario(nos))
            {
                NoCelula n1aux, n2aux, n3aux, n4aux;
                n1aux = no4;
                n2aux = no3;
                n3aux = no2;
                n4aux = no1;

                no1 = n1aux;
                no2 = n2aux;
                no3 = n3aux;
                no4 = n4aux;
            }

            ArestaCelula a1 = retAresta(no1, no2);
            ArestaCelula a2 = retAresta(no1, no4);
            ArestaCelula a3 = retAresta(no2, no3);
            ArestaCelula a4 = retAresta(no3, no4);

            if (a1 == null)
            {
                arestasMalha.Add(new ArestaCelula(no1, no2, arestasMalha.Count));
                a1 = arestasMalha[arestasMalha.Count - 1];
            }
            if (a2 == null)
            {
                arestasMalha.Add(new ArestaCelula(no1, no4, arestasMalha.Count));
                a2 = arestasMalha[arestasMalha.Count - 1];
            }
            if (a3 == null)
            {
                arestasMalha.Add(new ArestaCelula(no2, no3, arestasMalha.Count));
                a3 = arestasMalha[arestasMalha.Count - 1];
            }
            if (a4 == null)
            {
                arestasMalha.Add(new ArestaCelula(no3, no4, arestasMalha.Count));
                a4 = arestasMalha[arestasMalha.Count - 1];
            }
            int no1id = no1.id;
            int no2id = no1.id;
            int no3id = no1.id;
            int no4id = no1.id;

            arestas_n1 = arestasMalha.FindAll(o => o.n1.id == no1id || o.n2.id == no1id).ToList();
            arestas_n2 = arestasMalha.FindAll(o => o.n1.id == no2id || o.n2.id == no2id).ToList();
            arestas_n3 = arestasMalha.FindAll(o => o.n1.id == no3id || o.n2.id == no3id).ToList();
            arestas_n4 = arestasMalha.FindAll(o => o.n1.id == no4id || o.n2.id == no4id).ToList();

            no1.valencia = arestas_n1.Count();
            no2.valencia = arestas_n2.Count();
            no3.valencia = arestas_n3.Count();
            no4.valencia = arestas_n4.Count();

            no1.aresta = false; no2.aresta = false; no3.aresta = false; no4.aresta = false;

            Celula cel = new Celula(no1, no2, no3, no4, celulasMalha.Count);
            if (temp)
                cel.temp = true;

            cel.atuNos();

            celulasMalha.Add(cel);
            a1.celsVizinhas.Add(cel.id); a2.celsVizinhas.Add(cel.id); a3.celsVizinhas.Add(cel.id); a4.celsVizinhas.Add(cel.id);

            return cel;
        }
        void JuntarDoisTris()
        {
            if (TemTrisVizinhos())
                return;
            InsereArestas();

            ValenciaNos();
        }

        void CriarDoisTris()
        {
            if (CrarDoisTrisVizinhos())
                return;

            finalizouCriarDoisTris = true;
        }
        int total_vertice;

        void CriarDoisTris_2()
        {
            if (CriarDoisTrisVizinhos_2())
                return;

            finalizouCriarDoisTris_2 = true;
        }
        bool CriarDoisTrisVizinhos_2()
        {
            List<Celula> cel_quad_vizinha = new List<Celula>();
            Celula vizinha;
            int total_dentro = 0;
            trisVizinhos = new List<triVizinho>();
            cels_tri = celulasMalha.FindAll(o =>/*o.id == 315 &&*/ !o.avulso && !o.mesclou && (o.N1.aresta || o.N2.aresta || o.N3.aresta || o.N4.aresta) && (!Geom.Iguais(o.angulo_n1, 180, 1) && !Geom.Iguais(o.angulo_n2, 180, 1) && !Geom.Iguais(o.angulo_n3, 180, 1) && !Geom.Iguais(o.angulo_n4, 180, 1)));
            // return false;
            if (!finalizouCriarDoisTris_2)
                foreach (Celula cel_tri in cels_tri)
                {

                    // if (finalizouCriarDoisTrsi)
                    //          return;

                    total_dentro = cel_tri.nos.FindAll(o => o.status == 4).Count;

                    if (total_dentro != 3)
                        continue;

                    for (int i = 0; i < cel_tri.nos.Count; i++)
                    {

                        // if (finalizouCriarDoisTrsi)
                        //    return;
                        if (cel_tri.nos[i].valencia == 4 && (cel_tri.nos[i].status == 2))
                        {
                            cel_quad_vizinha = celulasMalha.FindAll(o => !o.avulso && !o.mesclou && (o.N1.id == cel_tri.nos[i].id || o.N2.id == cel_tri.nos[i].id ||
                            o.N3.id == cel_tri.nos[i].id || o.N4.id == cel_tri.nos[i].id)
                            && o.id != cel_tri.id
                            && (!Geom.Iguais(o.angulo_n1, 180, 1))
                            && (!Geom.Iguais(o.angulo_n2, 180, 1))
                            && (!Geom.Iguais(o.angulo_n3, 180, 1))
                            && (!Geom.Iguais(o.angulo_n4, 180, 1))
                            );

                            if (cel_quad_vizinha.Count != 2)
                                continue;

                            //    cel_tri.nos[i].sel = true;

                            //   cel_tri.sel = true;

                            Celula quad = cel_tri;
                            for (int j = 0; j < cel_quad_vizinha.Count; j++)
                            {
                                int total_na_aresta = cel_quad_vizinha[j].nos.FindAll(o => o.aresta).Count;
                                total_vertice = cel_quad_vizinha[j].nos.FindAll(o => o.fim_de_linha).Count;

                                //  if (total_vertice + total_na_aresta > 1)
                                {
                                    quad = cel_quad_vizinha[j];
                        //            quad.sel = true;
                                    //    break;
                                }

                                NoCelula n_extremo_quad = quad.nos.Find(o => o.id != cel_tri.nos[i].id && (o.aresta || o.fim_de_linha));
                                int i_extremo = quad.nos.FindIndex(o => o.id != cel_tri.nos[i].id && (o.aresta || o.fim_de_linha));
                                int i_no_comum = quad.nos.FindIndex(o => o.id == cel_tri.nos[i].id);

                                int i_op = 0;
                                NoCelula n_oposto = quad.retNoOposto(i_extremo, ref i_op);

                                NoCelula meio = (n_extremo_quad + cel_tri.nos[i]) / 2;

                                Atribui_aresta_ou_fimdelinha(ref meio);
                                if (!meio.aresta || meio.fim_de_linha)
                                {
                                    //  meio.n2_wedge = false;
                                    continue;
                                }

                                NoCelula nn = meio.Clone();
                                nn.id = nosMalha.Count;
                                nosMalha.Add(nn);
                                Atribui_aresta_ou_fimdelinha(ref nn);
                                if (nn.fim_de_linha)
                                    nn.status = 3;

                                NoCelula meio2 = (meio + cel_tri.nos[i]) / 2;
                                NoCelula nn2 = meio2.Clone();
                                nn2.id = nosMalha.Count;
                                nosMalha.Add(nn2);
                                Atribui_aresta_ou_fimdelinha(ref nn2);
                                if (nn.fim_de_linha)
                                    nn.status = 3;

                                //   cel_tri.mesclou = true;
                                quad.mesclou = true;

                                NoCelula n1 = cel_tri.nos[i];
                                NoCelula n2 = n_oposto;
                                NoCelula n3 = nn;
                                NoCelula n4 = nn2;

                                AdicionaCelula(ref n1, ref n2, ref n3, ref n4);

                                celulasMalha[celulasMalha.Count - 1].atuNos();

                                //celulasMalha[celulasMalha.Count - 1].sel = true;

                                if (i_no_comum == 0) quad.N1 = nn;
                                if (i_no_comum == 1) quad.N2 = nn;
                                if (i_no_comum == 2) quad.N3 = nn;
                                if (i_no_comum == 3) quad.N4 = nn;

                           //     cel_tri.sel = true;

                                quad.atuNos();
                            }

                            //  DesenhaTudo();
                            //   Controle.SwapBuffers();
                            //StatusNos();
                            // CrarDoisTrisVizinhos();
                            return true;
                        }
                    }
                }
            finalizouCriarDoisTris = true;
            StatusNos();
            return false;

        }
        bool CrarDoisTrisVizinhos()
        {
            List<Celula> cel_quad_vizinha = new List<Celula>();
            Celula vizinha;
            trisVizinhos = new List<triVizinho>();
            cels_tri = celulasMalha.FindAll(o =>/*o.id == 224 && */!o.avulso && !o.mesclou && (o.N1.aresta || o.N2.aresta || o.N3.aresta || o.N4.aresta) && (Geom.Iguais(o.angulo_n1, 180, 1) || Geom.Iguais(o.angulo_n2, 180, 1) || Geom.Iguais(o.angulo_n3, 180, 1) || Geom.Iguais(o.angulo_n4, 180, 1)));
            // return false;
            if (!finalizouCriarDoisTris)
                foreach (Celula cel_tri in cels_tri)
                {

                    // if (finalizouCriarDoisTrsi)
                    //          return;
                    if (cel_tri.id == 224)
                        cel_tri.id = 224;

                    for (int i = 0; i < cel_tri.nos.Count; i++)
                    {

                        // if (finalizouCriarDoisTrsi)
                        //    return;

                        if (cel_tri.nos[i].valencia == 4 && (cel_tri.nos[i].status == 2 /*|| cel_tri.nos[i].status == 3)*/))
                        {
                            //   cel_tri.nos[i].sel = true;

                            cel_quad_vizinha = celulasMalha.FindAll(o => !o.avulso && !o.mesclou && (o.N1.id == cel_tri.nos[i].id || o.N2.id == cel_tri.nos[i].id ||
                            o.N3.id == cel_tri.nos[i].id || o.N4.id == cel_tri.nos[i].id)
                            && o.id != cel_tri.id
                            && (!Geom.Iguais(o.angulo_n1, 180, 1))
                            && (!Geom.Iguais(o.angulo_n2, 180, 1))
                            && (!Geom.Iguais(o.angulo_n3, 180, 1))
                            && (!Geom.Iguais(o.angulo_n4, 180, 1))
                            );

                            Celula quad = null;
                            for (int j = 0; j < cel_quad_vizinha.Count; j++)
                            {
                                int total_na_aresta = cel_quad_vizinha[j].nos.FindAll(o => o.aresta).Count;
                                total_vertice = cel_quad_vizinha[j].nos.FindAll(o => o.fim_de_linha).Count;

                                if (total_vertice + total_na_aresta > 1)
                                {
                                    quad = cel_quad_vizinha[j];
                                    //     quad.sel = true;
                                    break;
                                }
                            }

                            if (quad == null)
                                continue;

                            NoCelula n_extremo_quad = quad.nos.Find(o => o.id != cel_tri.nos[i].id && (o.aresta || o.fim_de_linha));
                            int i_extremo = quad.nos.FindIndex(o => o.id != cel_tri.nos[i].id && (o.aresta || o.fim_de_linha));
                            int i_no_comum = quad.nos.FindIndex(o => o.id == cel_tri.nos[i].id);

                            int i_op = 0;
                            NoCelula n_oposto = quad.retNoOposto(i_extremo, ref i_op);

                            NoCelula meio = (n_extremo_quad + cel_tri.nos[i]) / 2;

                            Atribui_aresta_ou_fimdelinha(ref meio);
                            if (!meio.aresta || meio.fim_de_linha)
                            {
                                //  meio.n2_wedge = false;
                                continue;
                            }

                            NoCelula nn = meio.Clone();
                            nn.id = nosMalha.Count;
                            nosMalha.Add(nn);
                            Atribui_aresta_ou_fimdelinha(ref nn);
                            if (nn.fim_de_linha)
                                nn.status = 3;

                            NoCelula meio2 = (meio + cel_tri.nos[i]) / 2;
                            NoCelula nn2 = meio2.Clone();
                            nn2.id = nosMalha.Count;
                            nosMalha.Add(nn2);
                            Atribui_aresta_ou_fimdelinha(ref nn2);
                            if (nn.fim_de_linha)
                                nn.status = 3;

                            //   cel_tri.mesclou = true;
                            quad.mesclou = true;

                            NoCelula n1 = cel_tri.nos[i];
                            NoCelula n2 = n_oposto;
                            NoCelula n3 = nn;
                            NoCelula n4 = nn2;

                            AdicionaCelula(ref n1, ref n2, ref n3, ref n4);

                            celulasMalha[celulasMalha.Count - 1].atuNos();

                            //celulasMalha[celulasMalha.Count - 1].sel = true;

                            if (i_no_comum == 0) quad.N1 = nn;
                            if (i_no_comum == 1) quad.N2 = nn;
                            if (i_no_comum == 2) quad.N3 = nn;
                            if (i_no_comum == 3) quad.N4 = nn;

                         //   cel_tri.sel = true;
                            //this.Text = cel_tri.id.ToString();
                            quad.atuNos();

                            //  DesenhaTudo();
                            //   Controle.SwapBuffers();
                            //StatusNos();
                            // CrarDoisTrisVizinhos();
                            return true;
                        }
                    }
                }
            finalizouCriarDoisTris = true;
            StatusNos();
            return false;

        }

        List<sNosValencia> nosvalencia, nosvalenciaFrontEspecifico;

        void ValenciaNos()
        {
            nosvalenciaFrontEspecifico = new List<sNosValencia>();

            List<NoCelula> ns = nosMalha.FindAll(o => o.status != 1 && !o.avulso);
            List<ArestaCelula> ares = new List<ArestaCelula>();

            for (int i = 0; i < ns.Count; i++)
            {
                ares = arestasMalha.FindAll(o => !o.avulsa && (o.n1.id == ns[i].id || o.n2.id == ns[i].id));
                ns[i].valencia = ares.Count();
                nosvalenciaFrontEspecifico.Add(new sNosValencia(ns[i]));

                for (int j = 0; j < ares.Count; j++)
                {
                    if (ns[i].id == ares[j].n1.id)
                        nosvalenciaFrontEspecifico[nosvalenciaFrontEspecifico.Count - 1].nos.Add(ares[j].n2);
                    else
                    if (ns[i].id == ares[j].n2.id)
                        nosvalenciaFrontEspecifico[nosvalenciaFrontEspecifico.Count - 1].nos.Add(ares[j].n1);
                }
            }
        }
        void InsereNos()
        {
            nosMalha = new List<NoCelula>();
            NoCelula n1, n2, n3, n4;
            for (int i = 0; i < celulasMalha.Count; i++)
            {
                if (celulasMalha[i].avulso) continue;

                n1 = LocalizaNoQuad(celulasMalha[i].N1.x, celulasMalha[i].N1.y);
                n2 = LocalizaNoQuad(celulasMalha[i].N2.x, celulasMalha[i].N2.y);
                n3 = LocalizaNoQuad(celulasMalha[i].N3.x, celulasMalha[i].N3.y);
                n4 = LocalizaNoQuad(celulasMalha[i].N4.x, celulasMalha[i].N4.y);

                if (n1 == null)
                {
                    n1 = celulasMalha[i].N1;// new NoCelula(celulasMalha[i].N1.x, celulasMalha[i].N1.y, 0, 0);
                    n1.id = nosMalha.Count;
                    nosMalha.Add(n1);
                }
                if (n2 == null)
                {
                    n2 = celulasMalha[i].N2;//;new NoCelula(celulasMalha[i].N2.x, celulasMalha[i].N2.y, 0, 0);
                    n2.id = nosMalha.Count;
                    nosMalha.Add(n2);
                }

                if (n3 == null)
                {
                    n3 = celulasMalha[i].N3; //new NoCelula(celulasMalha[i].N3.x, celulasMalha[i].N3.y, 0, 0);
                    n3.id = nosMalha.Count;
                    nosMalha.Add(n3);
                }
                if (n4 == null)
                {
                    n4 = celulasMalha[i].N4;//new NoCelula(celulasMalha[i].N4.x, celulasMalha[i].N4.y, 0, 0);
                    n4.id = nosMalha.Count;
                    nosMalha.Add(n4);
                }
            }
        }

        void InsereArestas()
        {
            ArestaCelula a1, a2, a3, a4;
            arestasMalha = new List<ArestaCelula>();
            for (int i = 0; i < celulasMalha.Count; i++)
            {
                if (celulasMalha[i].avulso) continue;
                a1 = LocalizaArestaQuad(celulasMalha[i].N1.x, celulasMalha[i].N1.y, celulasMalha[i].N2.x, celulasMalha[i].N2.y);
                a2 = LocalizaArestaQuad(celulasMalha[i].N2.x, celulasMalha[i].N2.y, celulasMalha[i].N3.x, celulasMalha[i].N3.y);
                a3 = LocalizaArestaQuad(celulasMalha[i].N3.x, celulasMalha[i].N3.y, celulasMalha[i].N4.x, celulasMalha[i].N4.y);
                a4 = LocalizaArestaQuad(celulasMalha[i].N4.x, celulasMalha[i].N4.y, celulasMalha[i].N1.x, celulasMalha[i].N1.y);

                if (a1 == null)
                {
                    a1 = new ArestaCelula(celulasMalha[i].N1, celulasMalha[i].N2, arestasMalha.Count);
                    arestasMalha.Add(a1);
                }

                if (a2 == null)
                {
                    a2 = new ArestaCelula(celulasMalha[i].N2, celulasMalha[i].N3, arestasMalha.Count);
                    arestasMalha.Add(a2);
                }

                if (a3 == null)
                {
                    a3 = new ArestaCelula(celulasMalha[i].N3, celulasMalha[i].N4, arestasMalha.Count);
                    arestasMalha.Add(a3);
                }

                if (a4 == null)
                {
                    a4 = new ArestaCelula(celulasMalha[i].N4, celulasMalha[i].N1, arestasMalha.Count);
                    arestasMalha.Add(a4);
                }
            }
        }
        ArestaCelula LocalizaArestaQuad(double xi, double yi, double xf, double yf)
        {
            for (int jk = 0; jk < arestasMalha.Count; jk++)
            {
                if ((Geom.Iguais(xi, arestasMalha[jk].n1.x, 0.1f) &&
                     Geom.Iguais(yi, arestasMalha[jk].n1.y, 0.1f) &&
                     Geom.Iguais(xf, arestasMalha[jk].n2.x, 0.1f) &&
                     Geom.Iguais(yf, arestasMalha[jk].n2.y, 0.1f))
                ||
                   (Geom.Iguais(xi, arestasMalha[jk].n2.x, 0.1f) &&
                     Geom.Iguais(yi, arestasMalha[jk].n2.y, 0.1f) &&
                     Geom.Iguais(xf, arestasMalha[jk].n1.x, 0.1f) &&
                     Geom.Iguais(yf, arestasMalha[jk].n1.y, 0.1f)))
                {

                    return arestasMalha[jk];
                }
            }

            return null;
        }
        List<Celula> quads;
        List<triVizinho> trisVizinhos;
        List<Celula> cels_tri;
        List<NoCelula> nos_compartilhados;
        List<NoCelula> nos_compartilhados2;
        public List<NoCelula> CompartilhaUmNo(Celula c1, Celula c2)
        {
            List<NoCelula> nos = new List<NoCelula>();

            for (int i = 0; i < c1.nos.Count; i++)
            {
                if (c1.nos[i].id == c2.N1.id)
                    nos.Add(c2.N1);

                if (c1.nos[i].id == c2.N2.id)
                    nos.Add(c2.N2);

                if (c1.nos[i].id == c2.N3.id)
                    nos.Add(c2.N3);

                if (c1.nos[i].id == c2.N4.id)
                    nos.Add(c2.N4);
            }

            return nos;
        }
        List<Celula> BuscaQuadrilaterosNoMeio(int idNo, int id_tri1, int id_tri2)
        {
            List<Celula> quads = celulasMalha.FindAll(o => (o.N1.id == idNo || o.N2.id == idNo || o.N3.id == idNo || o.N4.id == idNo) && o.id != id_tri1 && o.id != id_tri2 && !o.avulso/* && !o.mesclou*/ &&
            (o.N1.aresta || o.N2.aresta || o.N3.aresta || o.N4.aresta ||
            o.N1.fim_de_linha || o.N2.fim_de_linha || o.N3.fim_de_linha || o.N4.fim_de_linha));

            return quads.ToList();
        }

        public bool PontoEmPoligono(ref double x, ref double y)
        {
            bool inside = false;
            double x_intersec;

            for (int g = 0; g < arestas.Count; g++)
            {
                //              if (!arestas[g].contornoPoligono)
                //                    continue;

                if (((arestas[g].p1.y > y) && (arestas[g].p2.y < y)) ||
                    ((arestas[g].p1.y < y) && (arestas[g].p2.y > y)))
                {
                    x_intersec = arestas[g].p1.x + (y - arestas[g].p1.y) * (arestas[g].p2.x - arestas[g].p1.x) / (arestas[g].p2.y - arestas[g].p1.y);

                    if (x_intersec > x)
                        inside = !inside;
                }
            }
            return inside;
        }

        bool TemTrisVizinhos()
        {
            List<Celula> cel_tri_vizinha = new List<Celula>();
            Celula vizinha;
            trisVizinhos = new List<triVizinho>();
            cels_tri = celulasMalha.FindAll(o =>/*o.id == 512 && */!o.avulso && !o.mesclou && (o.N1.aresta || o.N2.aresta || o.N3.aresta || o.N4.aresta) && (Geom.Iguais(o.angulo_n1, 180, 1) || Geom.Iguais(o.angulo_n2, 180, 1) || Geom.Iguais(o.angulo_n3, 180, 1) || Geom.Iguais(o.angulo_n4, 180, 1)));

            if (!finalizouJuntarTri)

                foreach (Celula cel_tri in cels_tri)
                {
                    List<Celula> vizinho = celulasMalha.FindAll(o => /*o.id == 249 &&*/ !o.avulso && !o.mesclou && o.id != cel_tri.id && (Geom.Iguais(o.angulo_n1, 180, 1) || Geom.Iguais(o.angulo_n2, 180, 1) || Geom.Iguais(o.angulo_n3, 180, 1) || Geom.Iguais(o.angulo_n4, 180, 1)));
                    for (int i = 0; i < vizinho.Count; i++)
                    {
                        vizinha = vizinho[i];

                        // if (finalizouJuntarTri)
                        //      return;

                        nos_compartilhados = CompartilhaUmNo(cel_tri, vizinha);

                        if (nos_compartilhados.Count == 1)
                        {
                            if (nos_compartilhados[0].aresta || nos_compartilhados[0].fim_de_linha)
                            {
                                NoCelula n_extremo1 = new NoCelula(), n_extremo2 = new NoCelula(), n_oposto_quad = new NoCelula(),
                                    n_oposto_180 = new NoCelula();
                                int iop1 = 0; int iop2 = 0;

                                for (int j = 0; j < cel_tri.nos.Count; j++)
                                    if (cel_tri.nos[j].id == nos_compartilhados[0].id)
                                        n_extremo1 = cel_tri.retNoOposto(j, ref iop1);

                                int no_180_1 = -1;
                                int no_180_2 = -2;

                                for (int j = 0; j < cel_tri.nos.Count; j++)
                                {
                                    if (Geom.Iguais(cel_tri.angulos_internos[j], 180, 1))
                                    {
                                        n_oposto_180 = cel_tri.retNoOposto(j, ref iop2);
                                        no_180_1 = j;
                                    }
                                }

                                for (int j = 0; j < vizinha.nos.Count; j++)
                                {
                                    if (vizinha.nos[j].id == nos_compartilhados[0].id)
                                        n_extremo2 = vizinha.retNoOposto(j, ref iop2);

                                    if (Geom.Iguais(vizinha.angulos_internos[j], 180, 1))
                                        no_180_2 = j;
                                }

                                if (cel_tri.nos[no_180_1].id_aresta == vizinha.nos[no_180_2].id_aresta)
                                {

                                    trisVizinhos.Add(new triVizinho(cel_tri, vizinha, nos_compartilhados.Count, nos_compartilhados[0], n_extremo1, n_extremo2));

                                    quads = BuscaQuadrilaterosNoMeio(nos_compartilhados[0].id, cel_tri.id, vizinha.id);

                                    if (quads.Count == 0)
                                        continue;

                                    for (int k = 0; k < quads.Count; k++)
                                    {
                                        for (int j = 0; j < quads[k].nos.Count; j++)
                                            if (quads[k].nos[j].id == nos_compartilhados[0].id)
                                                n_oposto_quad = quads[k].retNoOposto(j, ref iop1);
                                    }


                                    NoCelula meio = (nos_compartilhados[0] + n_oposto_quad) / 2;

                                    meio = (meio + nos_compartilhados[0]) / 2;

                                    NoCelula nn = meio.Clone();
                                    nn.id = nosMalha.Count;
                                    Atribui_aresta_ou_fimdelinha(ref nn);
                                    if (nn.fim_de_linha)
                                        nn.status = 3;

                                    cel_tri.N1 = n_extremo1;
                                    cel_tri.N2 = nos_compartilhados[0];
                                    cel_tri.N3 = nn;
                                    cel_tri.N4 = n_oposto_180;

                                    for (int j = 0; j < vizinha.nos.Count; j++)
                                        if (Geom.Iguais(vizinha.angulos_internos[j], 180, 1))
                                            n_oposto_180 = vizinha.retNoOposto(j, ref iop2);

                                    vizinha.N1 = n_extremo2;
                                    vizinha.N2 = nos_compartilhados[0];
                                    vizinha.N3 = nn;
                                    vizinha.N4 = n_oposto_180;

                                    for (int k = 0; k < quads.Count; k++)
                                    {
                                        if (quads[k].N1.id == nos_compartilhados[0].id) quads[k].N1 = nn;
                                        if (quads[k].N2.id == nos_compartilhados[0].id) quads[k].N2 = nn;
                                        if (quads[k].N3.id == nos_compartilhados[0].id) quads[k].N3 = nn;
                                        if (quads[k].N4.id == nos_compartilhados[0].id) quads[k].N4 = nn;

                                        quads[k].atuNos();
                                 //       quads[k].sel = true;
                                    }

                                    vizinha.atuNos();
                                    cel_tri.atuNos();

                             //       vizinha.sel = true;
                          ///          cel_tri.sel = true;

                                    /* DesenhaTudo();
                                     Controle.SwapBuffers();*/

                                    // TemTrisVizinhos();
                                    return true;
                                }
                            }
                        }
                    }
                }

            finalizouJuntarTri = true;
            StatusNos();

            return false;

        }
        struct sNosValencia
        {
            public List<NoCelula> nos;
            public NoCelula no;
            public sNosValencia(NoCelula n)
            {
                nos = new List<NoCelula>();
                no = n;
            }
        }
        struct triVizinho
        {
            Celula t1, t2;
            int nos_compartilhados;
            NoCelula no_comum, no_extremo1, no_extremo2;
            public triVizinho(Celula c1, Celula c2, int nos, NoCelula _no_comum, NoCelula _no_extremo1, NoCelula _no_extremo2)
            {
                t1 = c1;
                t2 = c2;
                nos_compartilhados = nos;
                no_comum = _no_comum;
                no_extremo1 = _no_extremo1;

                no_extremo2 = _no_extremo2;

            }
        }

        struct linhas_x_nos
        {
            public List<NoCelula> nos_ordenados;
            public LinhaVec3 linha;
            public linhas_x_nos(List<NoCelula> _nos_ordenados, LinhaVec3 _linha)
            {
                linha = _linha;
                nos_ordenados = _nos_ordenados;
            }
        }
    }
}
