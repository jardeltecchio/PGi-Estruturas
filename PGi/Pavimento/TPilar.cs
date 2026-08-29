using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Drawing;
using System.Drawing.Drawing2D;
using GeometryUtility;
using PolygonCuttingEar;
using System.IO;
using System.Drawing.Imaging;

using OpenTK;
using OpenTK.Input;
using OpenTK.Graphics.OpenGL;
using OpenTK.Platform;

namespace PG
{
    [Serializable]

    public class TPilar : TObjetoDesenho
    {
        public void AddTextos()
        {
            double xmax = -99999999999;
            double ymax = -99999999999;
            foreach (TLinha aresta in this.Dados.Poligono.linhas_poligonal)
            {
                if (aresta.pIni.y > ymax)
                {
                    ymax = aresta.pIni.y;
                }
                if (aresta.pFin.y > ymax)
                {
                    ymax = aresta.pFin.y;
                } 
                
                if (aresta.pIni.x > xmax)
                {
                    xmax = aresta.pIni.x;
                }
                if (aresta.pFin.x > xmax)
                {
                    xmax = aresta.pFin.x;
                }
            }

            ymax += 15;
            xmax += 2;

            Texto1 = new TTexto(Dados.nome + Dados.numero, xmax, ymax, (15.0 / 100),
                          -(15.0 / 100),
                          0, 1, 0,
                          0, this.Estrutura.LayersByIdPrincipal[Lay.TextosPilares], this.Tipo);

            Texto2 = new TTexto(Dados.B1 + "/" + Dados.H1 + (Dados.B2 > 0 ? "/" + Dados.B2 + "/" + Dados.H2 : ""), xmax + 5, ymax - 10, (15.0 / 300),
                          -(15.0 / 300),
                          0, 1, 0,
                          0, this.Estrutura.LayersByIdPrincipal[Lay.TextosPilares], this.Tipo);
        }

        public override string PrimeiroComando()
        {
            return Const.CMD_PILAR_1_P;
        }

        public TPilar(TDadosPilar dados = null) 
        {
            base.Visivel       = true;
            base.IdLayer       = Lay.Pilares;
            this.Tipo          = Const.ID_PILAR;
            this.Dados = dados;
        }

        public TPilar(TPonto p1, TPonto p2, TDadosPilar dados, TLayer lay, int pav , List<TLinha> Linhas, int verticeAtual)
        {
            string arg = "";
            List<TObjetoDesenho> obj = new List<TObjetoDesenho>();
            
            this.VerticeAtual = verticeAtual;

            Initialize(ref p1, ref arg, dados, lay, ref Linhas, ref pav); // inicializa 
          
            DefinindoAngulo = false;

            OnMouseMove(ref p1, false,false, 0, false);

            /*olhar raciocinios - pilares - ponto fixo*/
            /* o vertice inicial vai comecar no p1 (que é o ponto fixo), ai se alterna todos os vertices para ficar ok*/
            for (int k = 0; k < Vertices.Count; k++) 
              AlternaVertice();
            AddLinhasPoligonalAux();

            OnMouseDown(ref p1, ref arg, null, null, null, ref obj, false);
          
            DefinindoAngulo = true;
            OnMouseMove(ref p2, false, false, 0, false); 
         //   OnMouseDown(ref p2, ref arg,null,null,  ref obj, false);
        }

        public override void Initialize(ref TPonto point, ref string command, ISettings Dados, TLayer layer, ref List<TLinha> Linhas, ref int pavimento)
        {
            this.pIni = new TPonto(point.x, point.y, 0);
            this.pFin = new TPonto(point.x, point.y, 0);
            base.Visivel = true;
            this.Tipo = Const.ID_PILAR;
            base.IdLayer = Lay.Pilares;
            this.layer = layer;
            this.Dados = Dados as TDadosPilar;
            this.angulo = 0;

            mPen          = new Pen(Color.White);
            sbPoligono    = new System.Drawing.SolidBrush(System.Drawing.Color.FromArgb(50, this.layer.Rgb[0], this.layer.Rgb[1], this.layer.Rgb[2]));
            pts_triangulo = new System.Drawing.Point[3];

            command = Const.CMD_PILAR_1_P;
            base.Initialize(ref point, ref command, Dados, layer, ref Linhas, ref pavimento);
                     
            CriaListaVertices();

            //posiciona o pilar no vertice do centro geometrico, que sempre é o último vértice na lista de vértices
            if ((Dados as TDadosPilar).Poligono.TipoSecao == "Circular")
              for (int k = 0; k< 40; k++)
                AlternaVertice();       //-->>   alternar para comecar no vertice central*/

            foreach (TLinha lin in (Dados as TDadosPilar).Poligono.linhas_poligonal)
              lin.layer = this.layer;

            this.Pavimento = pavimento;

            AddLinhasPoligonalAux();

            TextoInfo = new TTexto("Comp: ", point.x, point.y, 0.08, 0.08, 1, 1, 0, 0, Estrutura.LayersByIdPrincipal[Lay.Pilares], Const.ID_PILAR);

        }
        [NonSerializedAttribute]
        public CPolygonShape TriangulosPilar;
        public void CriaListaVertices()
        {
            Vertices = new List<TPonto>();

            foreach (TLinha lin in Dados.Poligono.linhas_poligonal)
            {
                lin.LinhaContornoPilar = true;

                if (!LocVertice(lin.pIni))
                    Vertices.Add(new TPonto(lin.pIni.x, lin.pIni.y, 0));

                if (!LocVertice(lin.getMiddlePoint()))
                    Vertices.Add(new TPonto(lin.getMiddlePoint().x, lin.getMiddlePoint().y, -2)); //-2 significa vertice do meio da linha

                if (!LocVertice(lin.pFin))
                    Vertices.Add(new TPonto(lin.pFin.x, lin.pFin.y, 0));
            }

            Dados.Poligono.AtualizaCoords();
            Dados.Poligono.CalculaPropriedades();

            //
            //z = -1 sinaliza que é um vértice no vazio do perfil
            //esses vertices podem ser o centroide da secao ou outro ponto na secao
            //
            Vertices.Add(new TPonto(Dados.Poligono.centroide.X, Dados.Poligono.centroide.Y, -1));

            TPonto v1, v2;
            TLinha lAux = null;

            if (Dados.Poligono.TipoSecao == "L")
            {
                v1 = Dados.Poligono.linhas_poligonal[0].pIni;
                v2 = Dados.Poligono.linhas_poligonal[2].pFin;
                lAux = new TLinha(v1,v2,-1,null,false,false,false,false,false,false,true,null,null,false);
                Vertices.Add(lAux.getMiddlePoint());
                Vertices[Vertices.Count-1].z = -1;
            }
            else
            if (Dados.Poligono.TipoSecao == "I")
            {
                v1 = Dados.Poligono.linhas_poligonal[0].pIni;
                v2 = Dados.Poligono.linhas_poligonal[1].pFin;
                lAux = new TLinha(v1, v2, -1, null, false, false, false, false, false, false, true, null, null, false);
                Vertices.Add(lAux.getMiddlePoint());
                Vertices[Vertices.Count - 1].z = -1;

                v1 = Dados.Poligono.linhas_poligonal[6].pIni;
                v2 = Dados.Poligono.linhas_poligonal[8].pIni;
                lAux = new TLinha(v1, v2, -1, null, false, false, false, false, false, false, true, null, null, false);
                Vertices.Add(lAux.getMiddlePoint());
                Vertices[Vertices.Count - 1].z = -1;

            }
            if (Dados.Poligono.TipoSecao == "T")
            {
                v1 = Dados.Poligono.linhas_poligonal[0].pIni;
                v2 = Dados.Poligono.linhas_poligonal[1].pFin;
                lAux = new TLinha(v1, v2, -1, null, false, false, false, false, false, false, true, null, null, false);
                Vertices.Add(lAux.getMiddlePoint());
                Vertices[Vertices.Count - 1].z = -1;

                v1 = Dados.Poligono.linhas_poligonal[2].pFin;
                v2 = Dados.Poligono.linhas_poligonal[4].pFin;
                lAux = new TLinha(v1, v2, -1, null, false, false, false, false, false, false, true, null, null, false);
                Vertices.Add(lAux.getMiddlePoint());
                Vertices[Vertices.Count - 1].z = -1;
            }
           /* if (Dados.Poligono.TipoSecao == "Circular")
            {
                v1 = new TPonto(Dados.Poligono.centroide.X,Dados.Poligono.centroide.Y,0);
                TPonto pMeio = new TPonto(v1.x, v1.y,0);
                Vertices.Add(pMeio);
                Vertices[Vertices.Count - 1].z = -1;
            }*/

            //lista de linhas que tem tamanho zero, só servem para dar a captura
            VerticesNosVazios = new List<TLinha>();
            
            for (int k = 0; k< Vertices.Count; k++)
            {
                if (Vertices[k].z == -1)
                {
                    VerticesNosVazios.Add(new TLinha(Vertices[k], Vertices[k], -1, null, false, false, false, false, false, false, true, null, null, false));
                    VerticesNosVazios[VerticesNosVazios.Count - 1].layer = this.layer;
                }
            }
       
        }

        public bool PontoEmAresta(double x, double y, double tol = 0.01)
        {
            foreach (TLinha Aresta in this.Dados.Poligono.linhas_poligonal)
                if (Geom.PontoEmLinha2(x, y, Aresta.pIni.x, Aresta.pIni.y, Aresta.pFin.x, Aresta.pFin.y, tol))
                    return true;
            
            return false;
        }

        bool LocVertice(TPonto p1)
        {
            foreach (TPonto p2 in Vertices)
                if (Geom.Iguais(p1.x, p2.x) && Geom.Iguais(p1.y, p2.y))
                    return true;
            
            return false;
        }

        public void AddLinhasPoligonalAux()
        {
            laux = new TLinha(new TPonto(0, 0, 0), new TPonto(0, 0, 0), -1);
            linhas_poligonal_aux = new List<TLinha>();
          
            for (int i = 0; i < Dados.Poligono.linhas_poligonal.Count*2; i++)
              linhas_poligonal_aux.Add(new TLinha(new TPonto(0, 0, 0), new TPonto(0, 0, 0),-1));
        }

        public override void OnMouseMove(ref TPonto point, bool ShowInfo,bool orto = true, double ang = 0, bool PontoInicial = false)
        {
            if (!DefinindoAngulo)
            {
                this.pIni = point;
                this.pFin.x = point.x;
                this.pFin.y = point.y;
                this.pFin.z = point.z + Dados.altura;

                foreach (TLinha lin in Dados.Poligono.linhas_poligonal)
                {
                    centroRotacao.x = this.pIni.x;
                    centroRotacao.y = this.pIni.y;

                    coord1.x = (lin.pIni.x - ultPonto.x) + this.pIni.x;
                    coord1.y = (lin.pIni.y - ultPonto.y) + this.pIni.y;

                    coord2.x = (lin.pFin.x - ultPonto.x) + this.pIni.x;
                    coord2.y = (lin.pFin.y - ultPonto.y) + this.pIni.y;

                    lin.pIni.x = coord1.x;
                    lin.pIni.y = coord1.y;

                    lin.pFin.x = coord2.x;
                    lin.pFin.y = coord2.y;

                    //eu preciso dessas coordenadas originais para fazer a rotação
                    lin.pIniAux.x = coord1.x;
                    lin.pIniAux.y = coord1.y;
                    lin.pFinAux.x = coord2.x;
                    lin.pFinAux.y = coord2.y;
                }

                ultPonto.x = point.x;
                ultPonto.y = point.y;
            }
            else
            {
             //   if (DefinindoAngulo)
                {
                    xini = pIni.x;
                    yini = pIni.y;

                   // this.pFin = point.Clone() as TPonto;

                    xfin = point.x;
                    yfin = point.y;

                    alfa = FuncoesGerais.atand(Math.Abs(yini - yfin) / Math.Abs(xini - xfin));

                    if (ConfiguracoesCaptura.Captura.OrtogonalLigado)
                    {
                        if (alfa > 88)
                        {
                            alfa = 90;
                            xfin = xini;
                        }
                        else
                        if (alfa > 0 && alfa < 2)
                        {
                            alfa = 0;
                            yfin = yini;
                        };
                    };

                    DifCoordX = Math.Abs(xini - xfin);
                    DifCoordY = Math.Abs(yini - yfin);

                    for (i = 0; i < ConfiguracoesCaptura.Captura.OutrosAngulos.Count; i++)
                    {
                        if (alfa >= (ConfiguracoesCaptura.Captura.OutrosAngulos[i] - 2) && alfa <= (ConfiguracoesCaptura.Captura.OutrosAngulos[i] + 2))
                        {
                            alfa = ConfiguracoesCaptura.Captura.OutrosAngulos[i];

                            if (yfin > yini)
                                yfin = (yini + (float)(Math.Tan(alfa * Const.PIDiv180) * DifCoordX));
                            else
                                yfin = (yini - (float)(Math.Tan(alfa * Const.PIDiv180) * DifCoordX));
                        };
                    };

                    this.pFin.x = xfin;
                    this.pFin.y = yfin;

               //     Cad.DrawLine(Pens.Red, Desenho.pixelX(xini), Desenho.pixelY(yini), Desenho.pixelX(xfin), Desenho.pixelY(yfin));
                }

                angulo = Geom.GetAnguloGlobal(pIni.x, pIni.y, this.pFin.x, this.pFin.y);

                angAnterior = angulo;

                this.pFin = new TPonto(point.x, point.y, point.z + Dados.altura);
                centroRotacao = new vec3(this.pIni.x, this.pIni.y, 0);

                i = -1;
                iAux = -1;
                foreach (TLinha lin in Dados.Poligono.linhas_poligonal)
                {
                    //pego as coordenadas originais, sem rotação
                    coord1 = new vec3(lin.pIniAux.x, lin.pIniAux.y, 0);
                    coord2 = new vec3(lin.pFinAux.x, lin.pFinAux.y, 0);

                    //rotaciono elas com relação ao vértice escolhido
                    coord1 = coord1.Rotate(centroRotacao, angulo * Const.PIDiv180);
                    coord2 = coord2.Rotate(centroRotacao, angulo * Const.PIDiv180);

                    lin.pIni.x = coord1.x;
                    lin.pIni.y = coord1.y;

                    lin.pFin.x = coord2.x;
                    lin.pFin.y = coord2.y;

                    lin.comprimento = (float)lin.pFin.DistanceTo(lin.pIni);
                    lin.angulo = ((float)(FuncoesGerais.atand((lin.pIni.y - lin.pFin.y) / (lin.pIni.x - lin.pFin.x))));
                    lin.anguloGlobal = (float)lin.pIni.getAngleTo(lin.pFin);

                    /* Arestas Auxiliares */
                    i++;
                    for (j = 0; j < 2; j++)
                    {
                        iAux++;

                        if (j == 0)
                            UmOitoZero_Ou_TresSeisZero = 360;
                        else
                            UmOitoZero_Ou_TresSeisZero = 180;

                        linhas_poligonal_aux[iAux].pIni = Dados.Poligono.linhas_poligonal[i].getMiddlePoint();

                        hip = .6 + Dados.Poligono.linhas_poligonal[i].comprimento / 2;

                        senO = Math.Sin(Dados.Poligono.linhas_poligonal[i].anguloGlobal + UmOitoZero_Ou_TresSeisZero * Const.PIDiv180);
                        cosO = Math.Cos(Dados.Poligono.linhas_poligonal[i].anguloGlobal + UmOitoZero_Ou_TresSeisZero * Const.PIDiv180);

                        nx = cosO * hip;
                        ny = senO * hip;

                        pFinAux = new TPonto(nx, ny, 0);
                        linhas_poligonal_aux[iAux].pFin = linhas_poligonal_aux[iAux].pIni + pFinAux;
                    }
                }


              /*  i = 0;

                linhas_poligonal_aux[i].pIni = Dados.Poligono.linhas_poligonal[i].getMiddlePoint();
                double nx, ny;
                double hip = 5 + Dados.Poligono.linhas_poligonal[i].comprimento / 2;

                double senO = Math.Sin(Dados.Poligono.linhas_poligonal[i].anguloGlobal + 360 * Const.PIDiv180);
                double cosO = Math.Cos(Dados.Poligono.linhas_poligonal[i].anguloGlobal + 360 * Const.PIDiv180);

                nx = cosO * hip;
                ny = senO * hip;

                TPonto pfin = new TPonto(nx, ny, 0);
                linhas_poligonal_aux[i].pFin = linhas_poligonal_aux[i].pIni + pfin;*/
              //  DefinindoAngulo = false;
            }

            base.OnMouseMove(ref pIni, ShowInfo);
        }

        public void AtualizaArestasAuxiliares()
        {
        }
        double AnguloRotacao;
        double anguloAnterior = 0;

        public override void Mover(ref TPonto ponto1,ref  TPonto ponto2, bool dinamico)
        {
            pIni.Mover(ref ponto1,ref ponto2, dinamico);
            pFin.Mover(ref ponto1,ref ponto2, dinamico);

            i = -1;
            iAux = -1;

            foreach (TLinha lin in Dados.Poligono.linhas_poligonal)
            {
                lin.Mover(ref   ponto1, ref ponto2, dinamico);

                lin.comprimento = (float)lin.pFin.DistanceTo(lin.pIni);
                lin.angulo = ((float)(FuncoesGerais.atand((lin.pIni.y - lin.pFin.y) / (lin.pIni.x - lin.pFin.x))));
                lin.anguloGlobal = (float)lin.pIni.getAngleTo(lin.pFin);

                /* Arestas Auxiliares */
                i++;
                for (j = 0; j < 2; j++)
                {
                    iAux++;

                    if (j == 0)
                        UmOitoZero_Ou_TresSeisZero = 360;
                    else
                        UmOitoZero_Ou_TresSeisZero = 180;

                    linhas_poligonal_aux[iAux].pIni = Dados.Poligono.linhas_poligonal[i].getMiddlePoint();

                    hip = .6 + Dados.Poligono.linhas_poligonal[i].comprimento / 2;

                    senO = Math.Sin(Dados.Poligono.linhas_poligonal[i].anguloGlobal + UmOitoZero_Ou_TresSeisZero * Const.PIDiv180);
                    cosO = Math.Cos(Dados.Poligono.linhas_poligonal[i].anguloGlobal + UmOitoZero_Ou_TresSeisZero * Const.PIDiv180);

                    nx = cosO * hip;
                    ny = senO * hip;

                    pFinAux = new TPonto(nx, ny, 0);
                    linhas_poligonal_aux[iAux].pFin = linhas_poligonal_aux[iAux].pIni + pFinAux;
                }
            }
        }

        public override void Rotacionar(TPonto ponto1, TPonto ponto2, double ang)
        {
            AnguloRotacao = ang;

            pFin = pFin.Rotate(ponto1, (AnguloRotacao - anguloAnterior) * Const.PIDiv180);
            pIni = pIni.Rotate(ponto1, (AnguloRotacao - anguloAnterior) * Const.PIDiv180);
            
            i    = -1;
            iAux = -1;
    
            foreach (TLinha lin in Dados.Poligono.linhas_poligonal)
            {
                lin.Rotacionar(ponto1, ponto2, AnguloRotacao);

                lin.comprimento  = (float)lin.pFin.DistanceTo(lin.pIni);
                lin.angulo       = ((float)(FuncoesGerais.atand((lin.pIni.y - lin.pFin.y) / (lin.pIni.x - lin.pFin.x))));
                lin.anguloGlobal = (float)lin.pIni.getAngleTo(lin.pFin);

                /* Arestas Auxiliares */
                i++;
                for (j = 0; j < 2; j++)
                {
                    iAux++;

                    if (j == 0)
                        UmOitoZero_Ou_TresSeisZero = 360;
                    else
                        UmOitoZero_Ou_TresSeisZero = 180;

                    linhas_poligonal_aux[iAux].pIni = Dados.Poligono.linhas_poligonal[i].getMiddlePoint();

                    hip = .6 + Dados.Poligono.linhas_poligonal[i].comprimento / 2;

                    senO = Math.Sin(Dados.Poligono.linhas_poligonal[i].anguloGlobal + UmOitoZero_Ou_TresSeisZero * Const.PIDiv180);
                    cosO = Math.Cos(Dados.Poligono.linhas_poligonal[i].anguloGlobal + UmOitoZero_Ou_TresSeisZero * Const.PIDiv180);

                    nx = cosO * hip;
                    ny = senO * hip;

                    pFinAux = new TPonto(nx, ny, 0);
                    linhas_poligonal_aux[iAux].pFin = linhas_poligonal_aux[iAux].pIni + pFinAux;
                }
            }

            anguloAnterior = AnguloRotacao;

        }//

        public override void Atualiza(int pavimento)
        {
            //recria os vertices, pois pode ter havido rotação das barras...
            this.Pavimento = pavimento;
            
            mPen = new Pen(Color.White);
            sbPoligono = new System.Drawing.SolidBrush(System.Drawing.Color.FromArgb(50, this.layer.Rgb[0], this.layer.Rgb[1], this.layer.Rgb[2]));
            pts_triangulo = new System.Drawing.Point[3];
            //pFin.z = pIni.z + Dados.altura;

            CriaListaVertices();
            TriangularizaFace();

            foreach (TLinha lin in Dados.Poligono.linhas_poligonal)
                lin.angulo = ((float)(FuncoesGerais.atand((lin.pIni.y - lin.pFin.y) / (lin.pIni.x - lin.pFin.x))));

            LinhaEixo = new TLinha(new TPonto(Dados.Poligono.centroide.X, Dados.Poligono.centroide.Y, pIni.z), new TPonto(Dados.Poligono.centroide.X, Dados.Poligono.centroide.Y, this.pFin.z), -1);
            LinhaEixo.layer = this.layer;
            LinhaEixo.Barra = this;

            AddGrips();
            AddTextos();
        }

        public override eObjetoDesenhoMouseDown OnMouseDown(ref TPonto point, ref string command, List<TLinha> Linhas, List<TPonto> Pontos, List<TPilar> Pilares, ref List<TObjetoDesenho> Objects, bool FromGrip = false)
        {
            this.OnMouseMove(ref point, false);

            if (!DefinindoAngulo)
            {
                this.pIni = new TPonto(point.x, point.y, point.z);
                this.pFin = new TPonto(point.x, point.y, point.z + Dados.altura);
                command = Const.CMD_PILAR_2_P;
                return eObjetoDesenhoMouseDown.Continue;
            }
            else
            {
                DefinindoAngulo = false;

                //recria os vertices, pois pode ter havido rotação das barras...
                Atualiza(this.Pavimento);

                return eObjetoDesenhoMouseDown.DoneRepeat;
            }
        }

        public override void Continue()
        {
            DefinindoAngulo = true;
        }

        public override void Desenha(ref System.Drawing.Graphics Cad)
        {
            try
            {
                mPen.Color   = Color.FromArgb(this.layer.Rgb[0], this.layer.Rgb[1], this.layer.Rgb[2]);
                mBrush.Color = Color.FromArgb(this.layer.Rgb[0], this.layer.Rgb[1], this.layer.Rgb[2]);
                sbPoligono.Color = Color.FromArgb(50, this.layer.Rgb[0], this.layer.Rgb[1], this.layer.Rgb[2]);

                if (Selecionado)
                {
                    mPen.Color = Color.Red;
                    mBrush.Color = Color.Red;
                    sbPoligono.Color = Color.Red;
                }

                foreach (TLinha lin in Dados.Poligono.linhas_poligonal)
                  Cad.DrawLine(mPen, FPrincipal.pixelX(lin.pIni.x), FPrincipal.pixelY(lin.pIni.y), FPrincipal.pixelX(lin.pFin.x), FPrincipal.pixelY(lin.pFin.y));
           
                foreach (TLinha lin in VerticesNosVazios)
                {
                    points[0].X = (int)FPrincipal.pixelX(lin.pIni.x - .1);
                    points[0].Y = (int)FPrincipal.pixelY(lin.pIni.y - .1);

                    points[1].X = (int)FPrincipal.pixelX(lin.pIni.x + .1);
                    points[1].Y = (int)FPrincipal.pixelY(lin.pIni.y - .1);

                    points[2].X = (int)FPrincipal.pixelX(lin.pIni.x + .1);
                    points[2].Y = (int)FPrincipal.pixelY(lin.pIni.y + .1);

                    points[3].X = (int)FPrincipal.pixelX(lin.pIni.x - .1);
                    points[3].Y = (int)FPrincipal.pixelY(lin.pIni.y + .1);

                    Cad.FillPolygon(mBrush, points);
                }

             //   mPen.Color = Color.White;
             //   if (linhas_poligonal_aux != null)
            //        foreach (TLinha lin in linhas_poligonal_aux)
            ////            Cad.DrawLine(mPen, Desenho.pixelX(lin.pIni.x), Desenho.pixelY(lin.pIni.y), Desenho.pixelX(lin.pFin.x), Desenho.pixelY(lin.pFin.y));
   
                //ponto fixo
                points[0].X = (int)FPrincipal.pixelX(pIni.x - 1);
                points[0].Y = (int)FPrincipal.pixelY(pIni.y - 1);

                points[1].X = (int)FPrincipal.pixelX(pIni.x + 1);
                points[1].Y = (int)FPrincipal.pixelY(pIni.y - 1);

                points[2].X = (int)FPrincipal.pixelX(pIni.x + 1);
                points[2].Y = (int)FPrincipal.pixelY(pIni.y + 1);

                points[3].X = (int)FPrincipal.pixelX(pIni.x - 1);
                points[3].Y = (int)FPrincipal.pixelY(pIni.y + 1);
                Cad.FillPolygon(mBrush, points);

                if (DefinindoAngulo)
                {
                    Cad.DrawLine(Pens.Red, FPrincipal.pixelX(xini), FPrincipal.pixelY(yini), FPrincipal.pixelX(xfin), FPrincipal.pixelY(yfin));
                }

                if (PoligonoFace!=null)
                  for (i = 0; i < PoligonoFace.NumberOfPolygons; i++)
                  {
                      for (j = 0; j < PoligonoFace.Polygons(i).Length; j++)
                      {
                          pts_triangulo[j].X = (int)FPrincipal.pixelX(PoligonoFace.Polygons(i)[j].X);
                          pts_triangulo[j].Y = (int)FPrincipal.pixelY(PoligonoFace.Polygons(i)[j].Y);
                      }
                      Cad.FillPolygon(sbPoligono, pts_triangulo);
                  }

                if (PoligonoFace2 != null)
                    for (i = 0; i < PoligonoFace2.NumberOfPolygons; i++)
                    {
                        for (j = 0; j < PoligonoFace2.Polygons(i).Length; j++)
                        {
                            pts_triangulo[j].X = (int)FPrincipal.pixelX(PoligonoFace2.Polygons(i)[j].X);
                            pts_triangulo[j].Y = (int)FPrincipal.pixelY(PoligonoFace2.Polygons(i)[j].Y);
                        }
                        Cad.FillPolygon(sbPoligono, pts_triangulo);
                    }                
            }
            catch (Exception ms)
            {
                System.Windows.Forms.MessageBox.Show(ms.Message);
            }

        }
        [NonSerialized]
        public System.Drawing.SolidBrush sbPoligono;
        [NonSerialized]
        public System.Drawing.Point[] pts_triangulo;
        public override bool Command(System.Windows.Forms.Keys key)
        {
            if (key == System.Windows.Forms.Keys.A)
            {
                AlternaVertice();
                base.Command(key);
                return true;
            }

            return false;
        }

        public void AlternaVertice()
        {
            if (VerticeAtual+1 == Vertices.Count)
              VerticeAtual = -1;                   //volta para o primeiro vertice
            
            ++VerticeAtual;
            
            double novoX = Vertices[VerticeAtual].x;
            double novoY = Vertices[VerticeAtual].y;
            
            foreach (TLinha lin in Dados.Poligono.linhas_poligonal)
            {
                lin.pIni.x -= novoX - ultX;
                lin.pIni.y -= novoY - ultY;

                lin.pFin.x -= novoX - ultX;
                lin.pFin.y -= novoY - ultY;
            }

            ultX = novoX;
            ultY = novoY;
        }

        public override bool Selecionar(float clicx, float clicy, float coordx, float coordy, TObjetoDesenho Owner = null)
        {
            if (this.layer.Congelado || this.layer.Travado)
                return false;

           bool em = (Dados.Poligono.PontoEmPoligono(coordx, coordy));
           
           if (em)
           {
               SetaSelecao(true, true);
               base.Selecionado = true;
               return em;
           }

           return false;
        }
        public override void SetaSelecao(bool s, bool MostraGrip, bool SelecaoDeCandidato = false, TObjetoDesenho Owner = null)
        {
            if (this.layer.Congelado || this.layer.Travado)
                return;

            foreach (TLinha lin in this.Dados.Poligono.linhas_poligonal)
              lin.Selecionado = s;

            Texto2.SetaSelecao(s, s, SelecaoDeCandidato);
            Texto1.SetaSelecao(s, s, SelecaoDeCandidato);

            base.Selecionado = s;
            base.MostrarGrip = MostraGrip;
            ShowHideGrips(MostraGrip);
        }

        public override void ShowHideGrips(bool visivel)
        {
            if (Grips != null)
                foreach (TGrip grip in Grips)
                    grip.Visivel = visivel;
        }
        public override void AddGrips()
        {
            Grips = new List<TGrip>();
            for (int k = 0; k < Vertices.Count; k++)
                if (Vertices[k].z == 0)
                  Grips.Add(new TGrip(this, Vertices[k].x, Vertices[k].y, false, false, true, false, this.layer));

       //     Grips.Add(new TGrip(this, VerticesNosVazios[0].pIni.x, VerticesNosVazios[0].pIni.y, false, false, true, false, this.layer));
        }
        int texture;                             // Storage For One Texture ( NEW )
        public void CarregaTexturas()
        {
        /*    bool status = false;                                                // Status Indicator
            Bitmap[] textureImage = new Bitmap[1];                              // Create Storage Space For The Texture

            textureImage[0] = LoadBMP("NeHe");                // Load The Bitmap
            // Check For Errors, If Bitmap's Not Found, Quit
            if (textureImage[0] != null)
            {
                status = true;                                                  // Set The Status To True

                GL.GenTextures(1, texture);                            // Create The Texture

                textureImage[0].RotateFlip(RotateFlipType.RotateNoneFlipY);     // Flip The Bitmap Along The Y-Axis
                // Rectangle For Locking The Bitmap In Memory
                Rectangle rectangle = new Rectangle(0, 0, textureImage[0].Width, textureImage[0].Height);
                // Get The Bitmap's Pixel Data From The Locked Bitmap
                BitmapData bitmapData = textureImage[0].LockBits(rectangle, ImageLockMode.ReadOnly, PixelFormat.Format24bppRgb);

                // Typical Texture Generation Using Data From The Bitmap
                GL.BindTexture(Gl.GL_TEXTURE_2D, texture[0]);
                GL.TexImage2D(Gl.GL_TEXTURE_2D, 0, Gl.GL_RGB8, textureImage[0].Width, textureImage[0].Height, 0, Gl.GL_BGR, Gl.GL_UNSIGNED_BYTE, bitmapData.Scan0);
                GL.TexParameteri(Gl.GL_TEXTURE_2D, Gl.GL_TEXTURE_MIN_FILTER, Gl.GL_LINEAR);
                GL.TexParameteri(Gl.GL_TEXTURE_2D, Gl.GL_TEXTURE_MAG_FILTER, Gl.GL_LINEAR);

                if (textureImage[0] != null)
                {                                   // If Texture Exists
                    textureImage[0].UnlockBits(bitmapData);                     // Unlock The Pixel Data From Memory
                    textureImage[0].Dispose();                                  // Dispose The Bitmap
                }
            }*/
        }
        private static Bitmap LoadBMP(string fileName)
        {
            if (fileName == null || fileName == string.Empty)
            {                  // Make Sure A Filename Was Given
                return null;                                                    // If Not Return Null
            }
            fileName = "C:\\PGi\\concreto.bmp";
            string fileName1 = string.Format("Data{0}{1}",                      // Look For Data\Filename
                Path.DirectorySeparatorChar, fileName);
            string fileName2 = string.Format("{0}{1}{0}{1}Data{1}{2}",          // Look For ..\..\Data\Filename
                "..", Path.DirectorySeparatorChar, fileName);

            // Make Sure The File Exists In One Of The Usual Directories
            if (!File.Exists(fileName) && !File.Exists(fileName1) && !File.Exists(fileName2))
            {
                return null;                                                    // If Not Return Null
            }

            if (File.Exists(fileName))
            {                                         // Does The File Exist Here?
                return new Bitmap(fileName);                                    // Load The Bitmap
            }
            else
                if (File.Exists(fileName1))
                {                                   // Does The File Exist Here?
                    return new Bitmap(fileName1);                                   // Load The Bitmap
                }
                else
                    if (File.Exists(fileName2))
                    {                                   // Does The File Exist Here?
                        return new Bitmap(fileName2);                                   // Load The Bitmap
                    }

            return null;                                                        // If Load Failed Return Null
        }
        public void Render3DTextura(double centroX, double centroY, double h, byte transparencia, byte[] rgb, bool Arestas)
        {
            if (Morre) return;

            ht[0] = h;
            ht[1] = h + AlturaLance;

            if (Arestas)
            {
                GL.Color3(rgb[0], rgb[1], rgb[2]);
                foreach (TLinha aresta in Dados.Poligono.linhas_poligonal)
                {
                    GL.Begin(PrimitiveType.LineLoop);
                    GL.Vertex3(aresta.pIni.y - centroY, ht[0], aresta.pIni.x - centroX);
                    GL.Vertex3(aresta.pIni.y - centroY, ht[1], aresta.pIni.x - centroX);
                    GL.Vertex3(aresta.pFin.y - centroY, ht[1], aresta.pFin.x - centroX);
                    GL.Vertex3(aresta.pFin.y - centroY, ht[0], aresta.pFin.x - centroX);
                    GL.End();
                }
            }
            GL.BindTexture(TextureTarget.Texture2D, texture);

            if (this.Selecionado)
                GL.Color4(255, 0, 0, (int)transparencia);
            else
                GL.Color4(rgb[0], rgb[1], rgb[2], transparencia);

            foreach (TLinha aresta in Dados.Poligono.linhas_poligonal)
            {
                p1 = new vec3(aresta.pIni.x - centroX, aresta.pIni.y - centroY, ht[1]);
                p2 = new vec3(aresta.pFin.x - centroX, aresta.pFin.y - centroY, ht[0]);
                p3 = new vec3(aresta.pFin.x - centroX, aresta.pFin.y - centroY, ht[1]);

                v1 = p2 - p1;
                v2 = p3 - p1;

                n1 = v1 * v2;
                n1.Normalize();

                /*face lateral de cima*/
                GL.Begin(PrimitiveType.Polygon);
                GL.Normal3(n1.x, n1.y, n1.z);

                GL.TexCoord2(0, 0);
                GL.Vertex3(aresta.pIni.y - centroY, ht[0], aresta.pIni.x - centroX);

                GL.TexCoord2(1, 0);
                GL.Vertex3(aresta.pIni.y - centroY, ht[1], aresta.pIni.x - centroX);

                GL.TexCoord2(1, 1);
                GL.Vertex3(aresta.pFin.y - centroY, ht[1], aresta.pFin.x - centroX);

                GL.TexCoord2(0, 1);
                GL.Vertex3(aresta.pFin.y - centroY, ht[0], aresta.pFin.x - centroX);
                GL.End();
            }
            /* 
            Retangular
            T
            I
            L
            Circular
            */

            if (Dados.indiceTipo == 1) // T
            {
                for (i = 0; i < 2; i++)
                {
                    //retangulo de baixo                   
                    GL.Begin(PrimitiveType.Polygon);
                    GL.Normal3(0, 1, 0);
                    GL.Vertex3(Dados.Poligono.linhas_poligonal[0].pIni.y - centroY, ht[i], Dados.Poligono.linhas_poligonal[0].pIni.x - centroX);
                    GL.Vertex3(Dados.Poligono.linhas_poligonal[1].pIni.y - centroY, ht[i], Dados.Poligono.linhas_poligonal[1].pIni.x - centroX);
                    GL.Vertex3(Dados.Poligono.linhas_poligonal[2].pIni.y - centroY, ht[i], Dados.Poligono.linhas_poligonal[2].pIni.x - centroX);
                    GL.Vertex3(Dados.Poligono.linhas_poligonal[7].pIni.y - centroY, ht[i], Dados.Poligono.linhas_poligonal[7].pIni.x - centroX);
                    GL.End();

                    //retangulo de cima
                    GL.Begin(PrimitiveType.Polygon);
                    GL.Normal3(0, 1, 0);
                    GL.Vertex3(Dados.Poligono.linhas_poligonal[3].pIni.y - centroY, ht[i], Dados.Poligono.linhas_poligonal[3].pIni.x - centroX);
                    GL.Vertex3(Dados.Poligono.linhas_poligonal[4].pIni.y - centroY, ht[i], Dados.Poligono.linhas_poligonal[4].pIni.x - centroX);
                    GL.Vertex3(Dados.Poligono.linhas_poligonal[5].pIni.y - centroY, ht[i], Dados.Poligono.linhas_poligonal[5].pIni.x - centroX);
                    GL.Vertex3(Dados.Poligono.linhas_poligonal[6].pIni.y - centroY, ht[i], Dados.Poligono.linhas_poligonal[6].pIni.x - centroX);
                    GL.End();
                }
            }
            else
                if (Dados.indiceTipo == 2) // I
                {
                    for (i = 0; i < 2; i++)
                    {
                        //retangulo de baixo
                        GL.Begin(PrimitiveType.Polygon);

                        GL.Normal3(0, 1, 0);
                        GL.Vertex3(Dados.Poligono.linhas_poligonal[0].pIni.y - centroY, ht[i], Dados.Poligono.linhas_poligonal[0].pIni.x - centroX);
                        GL.Vertex3(Dados.Poligono.linhas_poligonal[1].pIni.y - centroY, ht[i], Dados.Poligono.linhas_poligonal[1].pIni.x - centroX);
                        GL.Vertex3(Dados.Poligono.linhas_poligonal[2].pIni.y - centroY, ht[i], Dados.Poligono.linhas_poligonal[2].pIni.x - centroX);
                        GL.Vertex3(Dados.Poligono.linhas_poligonal[Dados.Poligono.linhas_poligonal.Count - 1].pIni.y - centroY, ht[i], Dados.Poligono.linhas_poligonal[Dados.Poligono.linhas_poligonal.Count - 1].pIni.x - centroX);
                        GL.End();

                        //retangulo do meio
                        GL.Begin(PrimitiveType.Polygon);
                        GL.Normal3(0, 1, 0);
                        GL.Vertex3(Dados.Poligono.linhas_poligonal[3].pIni.y - centroY, ht[i], Dados.Poligono.linhas_poligonal[3].pIni.x - centroX);
                        GL.Vertex3(Dados.Poligono.linhas_poligonal[4].pIni.y - centroY, ht[i], Dados.Poligono.linhas_poligonal[4].pIni.x - centroX);
                        GL.Vertex3(Dados.Poligono.linhas_poligonal[9].pIni.y - centroY, ht[i], Dados.Poligono.linhas_poligonal[9].pIni.x - centroX);
                        GL.Vertex3(Dados.Poligono.linhas_poligonal[10].pIni.y - centroY, ht[i], Dados.Poligono.linhas_poligonal[10].pIni.x - centroX);
                        GL.End();

                        //retangulo de cima
                        GL.Begin(PrimitiveType.Polygon);
                        GL.Normal3(0, 1, 0);
                        GL.Vertex3(Dados.Poligono.linhas_poligonal[5].pIni.y - centroY, ht[i], Dados.Poligono.linhas_poligonal[5].pIni.x - centroX);
                        GL.Vertex3(Dados.Poligono.linhas_poligonal[6].pIni.y - centroY, ht[i], Dados.Poligono.linhas_poligonal[6].pIni.x - centroX);
                        GL.Vertex3(Dados.Poligono.linhas_poligonal[7].pIni.y - centroY, ht[i], Dados.Poligono.linhas_poligonal[7].pIni.x - centroX);
                        GL.Vertex3(Dados.Poligono.linhas_poligonal[8].pIni.y - centroY, ht[i], Dados.Poligono.linhas_poligonal[8].pIni.x - centroX);
                        GL.End();
                    }
                }
                else
                {
                    GL.Begin(PrimitiveType.Polygon);
                    GL.Normal3(0, -1, 0);
                    kk = -1;
                    foreach (TLinha aresta in Dados.Poligono.linhas_poligonal)
                    {
                        kk++;
                        if (kk == 0)
                            GL.TexCoord2(0, 0);
                        else
                            if (kk == 1)
                                GL.TexCoord2(1, 0);
                            else
                                if (kk == 2)
                                    GL.TexCoord2(1, 1);
                                else
                                    if (kk == 3)
                                        GL.TexCoord2(0, 1);

                        GL.Vertex3(aresta.pIni.y - centroY, ht[0], aresta.pIni.x - centroX);
                        GL.Vertex3(aresta.pFin.y - centroY, ht[0], aresta.pFin.x - centroX);
                    }
                    GL.End();

                    GL.Begin(PrimitiveType.Polygon);
                    GL.Normal3(0, 1, 0);
                    kk = -1;
                    foreach (TLinha aresta in Dados.Poligono.linhas_poligonal)
                    {
                        kk++;
                        if (kk == 0)
                            GL.TexCoord2(0, 0);
                        else
                            if (kk == 1)
                                GL.TexCoord2(1, 0);
                            else
                                if (kk == 2)
                                    GL.TexCoord2(1, 1);
                                else
                                    if (kk == 3)
                                        GL.TexCoord2(0, 1);

                        GL.Vertex3(aresta.pIni.y - centroY, ht[1], aresta.pIni.x - centroX);
                        GL.Vertex3(aresta.pFin.y - centroY, ht[1], aresta.pFin.x - centroX);
                    }
                    GL.End();
                }
        }
        public override void Desenha(ref bool Unifilar,ref  int transp, ref bool Arestas)
        {
            ht[0] = pIni.z-1;
            ht[1] = pFin.z;

            if (DefinindoAngulo)
            {
                GL.Begin(PrimitiveType.Lines);
                GL.Vertex3(xini, yini, pIni.z);
                GL.Vertex3(xfin, yfin, pIni.z);
                GL.End();
            }

            if (this.Selecionado)
               GL.Color3(Color.Red);
            else
                GL.Color3(Color.Black);

            if (Unifilar)
            {
                GL.Begin(PrimitiveType.Lines);
                GL.Vertex3(Dados.Poligono.centroide.X, Dados.Poligono.centroide.Y, pIni.z);
                GL.Vertex3(Dados.Poligono.centroide.X, Dados.Poligono.centroide.Y, pFin.z);
                GL.End();
            }
            else
            {
                if (Arestas || Selecionado)
                {
                    foreach (TLinha aresta in Dados.Poligono.linhas_poligonal)
                    {
                        GL.Begin(PrimitiveType.LineLoop);
                        GL.Vertex3(aresta.pIni.x, aresta.pIni.y, ht[0]);
                        GL.Vertex3(aresta.pIni.x, aresta.pIni.y, ht[1]);
                        GL.Vertex3(aresta.pFin.x, aresta.pFin.y, ht[1]);
                        GL.Vertex3(aresta.pFin.x, aresta.pFin.y, ht[0]);
                        GL.End();
                    }
                }

                GL.Color4(Color.FromArgb(transp, this.Estrutura.LayersByIdPrincipal[Lay.Pilares].Rgb[0], this.Estrutura.LayersByIdPrincipal[Lay.Pilares].Rgb[1], this.Estrutura.LayersByIdPrincipal[Lay.Pilares].Rgb[2]));

                foreach (TLinha aresta in Dados.Poligono.linhas_poligonal)
                {
                    p1 = new vec3(aresta.pIni.x, aresta.pIni.y, ht[0]);
                    p2 = new vec3(aresta.pIni.x, aresta.pIni.y, ht[1]);
                    p3 = new vec3(aresta.pFin.x, aresta.pFin.y, ht[1]);

                    v1 = p2 - p1;
                    v2 = p3 - p1;

                    n1 = v1.CrossProduct(v2);

                    /*      GL.Begin(Gl.GL_LINES);
                          GL.Vertex3(aresta.pIni.x - centroX, aresta.pIni.y - centroY, ht[1]);
                          GL.Vertex3(aresta.pIni.x + n1.x, aresta.pIni.y - centroY + n1.y, ht[1] + n1.z);
                          GL.End();*/

                    n1.Normalize();

                    GL.Begin(PrimitiveType.Polygon);
                    GL.Normal3(-n1.x, -n1.y, -n1.z);
                    GL.Vertex3(aresta.pIni.x, aresta.pIni.y, ht[0]);
                    GL.Vertex3(aresta.pIni.x, aresta.pIni.y, ht[1]);
                    GL.Vertex3(aresta.pFin.x, aresta.pFin.y, ht[1]);
                    GL.Vertex3(aresta.pFin.x, aresta.pFin.y, ht[0]);
                    GL.End();

                    /*
                                    GL.LineWidth(1);
                                    GL.Color3(Color.Red);
                                    GL.Begin(PrimitiveType.LineLoop);
                                    GL.Vertex3(p1.x, p1.y, p1.z);
                                    GL.Vertex3(p2.x, p2.y, p2.z);
                                    GL.Vertex3(p3.x, p3.y, p3.z);
                                    GL.End();
                                    GL.LineWidth(1);*/

                }
                /* 
                Retangular
                T
                I
                L
                Circular
                */

                if (Dados.indiceTipo == 1) // T
                {
                    for (i = 0; i < 2; i++)
                    {
                        //retangulo de baixo                   
                        GL.Begin(PrimitiveType.Polygon);
                        GL.Normal3(0, 1, 0);
                        GL.Vertex3(Dados.Poligono.linhas_poligonal[0].pIni.x, Dados.Poligono.linhas_poligonal[0].pIni.y, ht[i]);
                        GL.Vertex3(Dados.Poligono.linhas_poligonal[1].pIni.x, Dados.Poligono.linhas_poligonal[1].pIni.y, ht[i]);
                        GL.Vertex3(Dados.Poligono.linhas_poligonal[2].pIni.x, Dados.Poligono.linhas_poligonal[2].pIni.y, ht[i]);
                        GL.Vertex3(Dados.Poligono.linhas_poligonal[7].pIni.x, Dados.Poligono.linhas_poligonal[7].pIni.y, ht[i]);
                        GL.End();

                        //retangulo de cima
                        GL.Begin(PrimitiveType.Polygon);
                        GL.Normal3(0, 1, 0);
                        GL.Vertex3(Dados.Poligono.linhas_poligonal[3].pIni.x, Dados.Poligono.linhas_poligonal[3].pIni.y, ht[i]);
                        GL.Vertex3(Dados.Poligono.linhas_poligonal[4].pIni.x, Dados.Poligono.linhas_poligonal[4].pIni.y, ht[i]);
                        GL.Vertex3(Dados.Poligono.linhas_poligonal[5].pIni.x, Dados.Poligono.linhas_poligonal[5].pIni.y, ht[i]);
                        GL.Vertex3(Dados.Poligono.linhas_poligonal[6].pIni.x, Dados.Poligono.linhas_poligonal[6].pIni.y, ht[i]);
                        GL.End();
                    }
                }
                else
                    if (Dados.indiceTipo == 2) // I
                    {
                        for (i = 0; i < 2; i++)
                        {
                            //retangulo de baixo
                            GL.Begin(PrimitiveType.Polygon);

                            GL.Normal3(0, 1, 0);
                            GL.Vertex3(Dados.Poligono.linhas_poligonal[0].pIni.y, ht[i], Dados.Poligono.linhas_poligonal[0].pIni.x);
                            GL.Vertex3(Dados.Poligono.linhas_poligonal[1].pIni.y, ht[i], Dados.Poligono.linhas_poligonal[1].pIni.x);
                            GL.Vertex3(Dados.Poligono.linhas_poligonal[2].pIni.y, ht[i], Dados.Poligono.linhas_poligonal[2].pIni.x);
                            GL.Vertex3(Dados.Poligono.linhas_poligonal[Dados.Poligono.linhas_poligonal.Count - 1].pIni.y, ht[i], Dados.Poligono.linhas_poligonal[Dados.Poligono.linhas_poligonal.Count - 1].pIni.x);
                            GL.End();

                            //retangulo do meio
                            GL.Begin(PrimitiveType.Polygon);
                            GL.Normal3(0, 1, 0);
                            GL.Vertex3(Dados.Poligono.linhas_poligonal[3].pIni.y, ht[i], Dados.Poligono.linhas_poligonal[3].pIni.x);
                            GL.Vertex3(Dados.Poligono.linhas_poligonal[4].pIni.y, ht[i], Dados.Poligono.linhas_poligonal[4].pIni.x);
                            GL.Vertex3(Dados.Poligono.linhas_poligonal[9].pIni.y, ht[i], Dados.Poligono.linhas_poligonal[9].pIni.x);
                            GL.Vertex3(Dados.Poligono.linhas_poligonal[10].pIni.y, ht[i], Dados.Poligono.linhas_poligonal[10].pIni.x);
                            GL.End();

                            //retangulo de cima
                            GL.Begin(PrimitiveType.Polygon);
                            GL.Normal3(0, 1, 0);
                            GL.Vertex3(Dados.Poligono.linhas_poligonal[5].pIni.y, ht[i], Dados.Poligono.linhas_poligonal[5].pIni.x);
                            GL.Vertex3(Dados.Poligono.linhas_poligonal[6].pIni.y, ht[i], Dados.Poligono.linhas_poligonal[6].pIni.x);
                            GL.Vertex3(Dados.Poligono.linhas_poligonal[7].pIni.y, ht[i], Dados.Poligono.linhas_poligonal[7].pIni.x);
                            GL.Vertex3(Dados.Poligono.linhas_poligonal[8].pIni.y, ht[i], Dados.Poligono.linhas_poligonal[8].pIni.x);
                            GL.End();
                        }
                    }
                    else
                    {
                        GL.Begin(PrimitiveType.Polygon);
                        GL.Normal3(0, -1, 0);
                        foreach (TLinha aresta in Dados.Poligono.linhas_poligonal)
                        {
                            GL.Vertex3(aresta.pIni.x, aresta.pIni.y, ht[0]);
                            GL.Vertex3(aresta.pFin.x, aresta.pFin.y, ht[0]);
                        }
                        GL.End();

                        GL.Begin(PrimitiveType.Polygon);
                        GL.Normal3(0, 1, 0);
                        foreach (TLinha aresta in Dados.Poligono.linhas_poligonal)
                        {
                            GL.Vertex3(aresta.pIni.x, aresta.pIni.y, ht[1]);
                            GL.Vertex3(aresta.pFin.x, aresta.pFin.y, ht[1]);
                        }
                        GL.End();
                    }
            }
        }

        public void Render3D(double centroX, double centroY, double h, byte transparencia, byte[] rgb, bool Arestas)
        {
              if (Morre) return;
             
              ht[0] = h;
              ht[1] = h + AlturaLance;

          /*    if (ht[0] == 1 && ht[1] == 0)
              {
                  ht[0] = 1;
                  ht[1] = -50;
              } */

      //          GL.Color3(rgb[0], rgb[1], rgb[2]);
              if (Arestas)
              {
                  GL.Color3(Color.Black);

                  if (this.Selecionado)
                  {
                      GL.LineWidth(2);
                      GL.Color3(Color.Red);
                  }
                  //    GL.Color3(0, 0, 0);

                  //GL.Color3(rgb[0], rgb[1], rgb[2]);
                  foreach (TLinha aresta in Dados.Poligono.linhas_poligonal)
                  {
                      GL.Begin(PrimitiveType.LineLoop);
                      GL.Vertex3(aresta.pIni.x - centroX,aresta.pIni.y - centroY, ht[0]);
                      GL.Vertex3(aresta.pIni.x - centroX,aresta.pIni.y - centroY, ht[1]);
                      GL.Vertex3(aresta.pFin.x - centroX,aresta.pFin.y - centroY, ht[1]);
                      GL.Vertex3(aresta.pFin.x - centroX,aresta.pFin.y - centroY, ht[0]);
                      GL.End();
                  }
              }
              GL.LineWidth(1);
            GL.Color4(rgb[0], rgb[1], rgb[2], transparencia);

            foreach (TLinha aresta in Dados.Poligono.linhas_poligonal)
            {
                p1 = new vec3(aresta.pIni.x - centroX, aresta.pIni.y - centroY, ht[0]);
                p2 = new vec3(aresta.pIni.x - centroX, aresta.pIni.y - centroY, ht[1]);
                p3 = new vec3(aresta.pFin.x - centroX, aresta.pFin.y - centroY, ht[1]);
                
                v1 = p2-p1;
                v2 = p3-p1;

                n1 = v1.CrossProduct(v2);
                
          /*      GL.Begin(Gl.GL_LINES);
                GL.Vertex3(aresta.pIni.x - centroX, aresta.pIni.y - centroY, ht[1]);
                GL.Vertex3(aresta.pIni.x + n1.x, aresta.pIni.y - centroY + n1.y, ht[1] + n1.z);
                GL.End();*/ 
                
                n1.Normalize();
             
                GL.Begin(PrimitiveType.Polygon);
                GL.Normal3(-n1.x, -n1.y, -n1.z);
                GL.Vertex3(aresta.pIni.x - centroX, aresta.pIni.y - centroY, ht[0]);
                GL.Vertex3(aresta.pIni.x - centroX, aresta.pIni.y - centroY, ht[1]);
                GL.Vertex3(aresta.pFin.x - centroX, aresta.pFin.y - centroY, ht[1]);
                GL.Vertex3(aresta.pFin.x - centroX, aresta.pFin.y - centroY, ht[0]);
                GL.End();

/*
                GL.LineWidth(1);
                GL.Color3(Color.Red);
                GL.Begin(PrimitiveType.LineLoop);
                GL.Vertex3(p1.x, p1.y, p1.z);
                GL.Vertex3(p2.x, p2.y, p2.z);
                GL.Vertex3(p3.x, p3.y, p3.z);
                GL.End();
                GL.LineWidth(1);*/

            }
/* 
Retangular
T
I
L
Circular
*/
           
            if (Dados.indiceTipo == 1) // T
            {
                for (i = 0; i < 2; i++)
               {
                    //retangulo de baixo                   
                    GL.Begin(PrimitiveType.Polygon);
                    GL.Normal3(0, 1, 0); 
                    GL.Vertex3(Dados.Poligono.linhas_poligonal[0].pIni.x - centroX,Dados.Poligono.linhas_poligonal[0].pIni.y - centroY, ht[i] );
                    GL.Vertex3(Dados.Poligono.linhas_poligonal[1].pIni.x - centroX,Dados.Poligono.linhas_poligonal[1].pIni.y - centroY, ht[i] );
                    GL.Vertex3(Dados.Poligono.linhas_poligonal[2].pIni.x - centroX,Dados.Poligono.linhas_poligonal[2].pIni.y - centroY, ht[i] );
                    GL.Vertex3(Dados.Poligono.linhas_poligonal[7].pIni.x - centroX,Dados.Poligono.linhas_poligonal[7].pIni.y - centroY, ht[i] );
                    GL.End();

                    //retangulo de cima
                    GL.Begin(PrimitiveType.Polygon);
                    GL.Normal3(0, 1, 0);
                    GL.Vertex3(Dados.Poligono.linhas_poligonal[3].pIni.x - centroX, Dados.Poligono.linhas_poligonal[3].pIni.y - centroY, ht[i]);
                    GL.Vertex3(Dados.Poligono.linhas_poligonal[4].pIni.x - centroX, Dados.Poligono.linhas_poligonal[4].pIni.y - centroY, ht[i]);
                    GL.Vertex3(Dados.Poligono.linhas_poligonal[5].pIni.x - centroX, Dados.Poligono.linhas_poligonal[5].pIni.y - centroY, ht[i]);
                    GL.Vertex3(Dados.Poligono.linhas_poligonal[6].pIni.x - centroX, Dados.Poligono.linhas_poligonal[6].pIni.y - centroY, ht[i]);
                    GL.End();
               }
            }
            else
            if (Dados.indiceTipo == 2) // I
            {
                for (i = 0; i < 2; i++)
                {
                    //retangulo de baixo
                    GL.Begin(PrimitiveType.Polygon);

                    GL.Normal3(0, 1, 0); 
                    GL.Vertex3(Dados.Poligono.linhas_poligonal[0].pIni.y - centroY, ht[i], Dados.Poligono.linhas_poligonal[0].pIni.x - centroX);
                    GL.Vertex3(Dados.Poligono.linhas_poligonal[1].pIni.y - centroY, ht[i], Dados.Poligono.linhas_poligonal[1].pIni.x - centroX);
                    GL.Vertex3(Dados.Poligono.linhas_poligonal[2].pIni.y - centroY, ht[i], Dados.Poligono.linhas_poligonal[2].pIni.x - centroX);
                    GL.Vertex3(Dados.Poligono.linhas_poligonal[Dados.Poligono.linhas_poligonal.Count - 1].pIni.y - centroY, ht[i], Dados.Poligono.linhas_poligonal[Dados.Poligono.linhas_poligonal.Count - 1].pIni.x - centroX);
                    GL.End();

                    //retangulo do meio
                    GL.Begin(PrimitiveType.Polygon);
                    GL.Normal3(0, 1, 0); 
                    GL.Vertex3(Dados.Poligono.linhas_poligonal[3].pIni.y - centroY, ht[i], Dados.Poligono.linhas_poligonal[3].pIni.x - centroX);
                    GL.Vertex3(Dados.Poligono.linhas_poligonal[4].pIni.y - centroY, ht[i], Dados.Poligono.linhas_poligonal[4].pIni.x - centroX);
                    GL.Vertex3(Dados.Poligono.linhas_poligonal[9].pIni.y - centroY, ht[i], Dados.Poligono.linhas_poligonal[9].pIni.x - centroX);
                    GL.Vertex3(Dados.Poligono.linhas_poligonal[10].pIni.y - centroY, ht[i], Dados.Poligono.linhas_poligonal[10].pIni.x - centroX);
                    GL.End();

                    //retangulo de cima
                    GL.Begin(PrimitiveType.Polygon);
                    GL.Normal3(0, 1, 0); 
                    GL.Vertex3(Dados.Poligono.linhas_poligonal[5].pIni.y - centroY, ht[i], Dados.Poligono.linhas_poligonal[5].pIni.x - centroX);
                    GL.Vertex3(Dados.Poligono.linhas_poligonal[6].pIni.y - centroY, ht[i], Dados.Poligono.linhas_poligonal[6].pIni.x - centroX);
                    GL.Vertex3(Dados.Poligono.linhas_poligonal[7].pIni.y - centroY, ht[i], Dados.Poligono.linhas_poligonal[7].pIni.x - centroX);
                    GL.Vertex3(Dados.Poligono.linhas_poligonal[8].pIni.y - centroY, ht[i], Dados.Poligono.linhas_poligonal[8].pIni.x - centroX);
                    GL.End();
                }
            }
            else
            {                
                GL.Begin(PrimitiveType.Polygon);
                GL.Normal3(0, -1, 0);
                foreach (TLinha aresta in Dados.Poligono.linhas_poligonal)
                {
                    GL.Vertex3(aresta.pIni.x - centroX, aresta.pIni.y - centroY, ht[0]);
                    GL.Vertex3(aresta.pFin.x - centroX, aresta.pFin.y - centroY, ht[0]);
                }
                GL.End();

               GL.Begin(PrimitiveType.Polygon);
                GL.Normal3(0, 1, 0);
                foreach (TLinha aresta in Dados.Poligono.linhas_poligonal)
                {
                    GL.Vertex3(aresta.pIni.x - centroX, aresta.pIni.y - centroY, ht[1]);
                    GL.Vertex3(aresta.pFin.x - centroX, aresta.pFin.y - centroY, ht[1]);
                }
                GL.End();
            }
        }

        public void TriangularizaFace()
        {
            CPoint2D[] vertices;
            CPoint2D[] vertices2;

            vertices  = new CPoint2D[this.Dados.Poligono.coords.Length - 1];
            
            if (Dados.Poligono.TipoSecao == "T")
            {
                vertices = new CPoint2D[4];
                vertices2 = new CPoint2D[4];

                vertices[0] = new CPoint2D(this.Dados.Poligono.coords[0].X, this.Dados.Poligono.coords[0].Y);
                vertices[1] = new CPoint2D(this.Dados.Poligono.coords[1].X, this.Dados.Poligono.coords[1].Y);
                vertices[2] = new CPoint2D(this.Dados.Poligono.coords[2].X, this.Dados.Poligono.coords[2].Y);
                vertices[3] = new CPoint2D(this.Dados.Poligono.coords[7].X, this.Dados.Poligono.coords[7].Y);

                vertices2[0] = new CPoint2D(this.Dados.Poligono.coords[6].X, this.Dados.Poligono.coords[6].Y);
                vertices2[1] = new CPoint2D(this.Dados.Poligono.coords[3].X, this.Dados.Poligono.coords[3].Y);
                vertices2[2] = new CPoint2D(this.Dados.Poligono.coords[4].X, this.Dados.Poligono.coords[4].Y);
                vertices2[3] = new CPoint2D(this.Dados.Poligono.coords[5].X, this.Dados.Poligono.coords[5].Y);

                PoligonoFace2 = new CPolygonShape(vertices2);
                PoligonoFace2.CutEar();
            }
            else
            {
                for (int i = 0; i < this.Dados.Poligono.coords.Length - 1; i++)
                    vertices[i] = new CPoint2D(this.Dados.Poligono.coords[i].X, this.Dados.Poligono.coords[i].Y);
            }

            PoligonoFace = new CPolygonShape(vertices);
            PoligonoFace.CutEar();
        }
        [NonSerialized]
        public double[] ht = new double[2];
        int kk;
        public TTexto Titulo;
        public TDadosPilar Dados;
        public List<TPonto> Vertices;
        public List<TLinha> VerticesNosVazios;
        public List<TLinha> BarrasRigidasDeViga;
        public double angulo;
        public TTexto Texto1, Texto2,TextoInfo;

        public int Pavimento;
        [NonSerialized]
        public TEstrutura Estrutura;
        [NonSerializedAttribute]
        CPolygonShape PoligonoFace, PoligonoFace2;
        [NonSerialized]
        vec3 n1, v1, v2, p1, p2, p3;
        double alfa, xini, yini, xfin, yfin, DifCoordX, DifCoordY, Comp, angFin, angIni, dx, dy, inicioAnguloX, inicioAnguloY;
        float PontoY;
        int i;

        public int VerticeAtual = 0;
        double ultX = 0;
        double ultY = 0;
        public bool DefinindoAngulo;
        double angAnterior = 0;
        double UmOitoZero_Ou_TresSeisZero;
        public double PesoDoLance,AlturaLance, AlturaAcima, AlturaAbaixo;
        int iAux;
        [NonSerialized]
        TPonto pFinAux;
        vec3 ultPonto = new vec3(0, 0, 0);

        public List<TLinha> linhas_poligonal_aux;
        public TLinha laux, LinhaEixo;
        double nx, ny, hip, senO, cosO;
        [NonSerialized]
        public Pen mPen = new Pen(Color.Red);
        [NonSerialized]
        public System.Drawing.SolidBrush mBrush = new System.Drawing.SolidBrush(System.Drawing.Color.Blue);
        [NonSerialized]
        public System.Drawing.Point[] points = new System.Drawing.Point[4];

        vec3 centroRotacao = new vec3(0, 0, 0);
        vec3 coord1 = new vec3(0, 0, 0);
        vec3 coord2 = new vec3(0, 0, 0);
        int j;
        public bool Morre, Passa, Nasce;
        
        public void Copy(TPilar obj)
        {
            base.Copy(obj);
            LinhaEixo = obj.LinhaEixo;
            Morre = obj.Morre;
            Passa = obj.Passa;
            Nasce = obj.Nasce;
            AlturaLance = obj.AlturaLance;
            PesoDoLance = obj.PesoDoLance;
            AlturaAcima = obj.AlturaAcima;
            AlturaAbaixo = obj.AlturaAbaixo;
            Estrutura = obj.Estrutura;
            VerticesNosVazios    = new List<TLinha>();
            linhas_poligonal_aux = new List<TLinha>();
            Vertices             = new List<TPonto>();
            BarrasRigidasDeViga  = new List<TLinha>();
            
            if ((Object)obj.linhas_poligonal_aux != null) 
                foreach (TLinha lin in obj.linhas_poligonal_aux)
                  linhas_poligonal_aux.Add((TLinha)lin.Clone());

            if ((Object)obj.VerticesNosVazios != null) 
                foreach (TLinha lin in obj.VerticesNosVazios)
                VerticesNosVazios.Add((TLinha)lin.Clone());

            if ((Object)obj.BarrasRigidasDeViga != null) 
                foreach (TLinha lin in obj.BarrasRigidasDeViga)
                  BarrasRigidasDeViga.Add((TLinha)lin.Clone());

            if ((Object)obj.Vertices != null) 
                foreach (TPonto pt in obj.Vertices)
                  Vertices.Add((TPonto)pt.Clone()); 
            
            if ((Object)obj.Titulo != null)
                this.Titulo = (TTexto)obj.Titulo.Clone();

            if ((Object)obj.Texto1 != null)
                this.Texto1 = (TTexto)obj.Texto1.Clone();
            if ((Object)obj.Texto2 != null)
                this.Texto2 = (TTexto)obj.Texto2.Clone();
            if ((Object)obj.TextoInfo != null)
                this.TextoInfo = (TTexto)obj.TextoInfo.Clone();
            
            if ((Object)obj.pIni != null)
                this.pIni = (TPonto)obj.pIni.Clone();
            if ((Object)obj.pFin != null)
                this.pFin = (TPonto)obj.pFin.Clone();

            Tipo = obj.Tipo;

            this.layer = obj.layer;
            Visivel    = obj.Visivel;

            Grips = new List<TGrip>();
            if (obj.Grips != null)
              foreach (TGrip g in obj.Grips)
                Grips.Add(new TGrip(this, g.x, g.y, g.translacao, g.rotacao, g.escala, g.estica, g.layer));

            if ((Object)obj.centroRotacao != null)
               centroRotacao = new vec3(obj.centroRotacao.x, obj.centroRotacao.y, obj.centroRotacao.z);
            if ((Object)obj.coord1 != null)
                coord1 = new vec3(obj.coord1.x, obj.coord1.y, obj.coord1.z);
            if ((Object)obj.coord2 != null)
                coord2 = new vec3(obj.coord2.x, obj.coord2.y, obj.coord2.z);

            mPen   = new Pen(Color.White);
            mBrush = new System.Drawing.SolidBrush(System.Drawing.Color.Blue);
            points = new System.Drawing.Point[4];
            if (obj.layer == null)
              sbPoligono = new System.Drawing.SolidBrush(System.Drawing.Color.FromArgb(50, 0,255,0));
            else
               sbPoligono = new System.Drawing.SolidBrush(System.Drawing.Color.FromArgb(50, obj.layer.Rgb[0], obj.layer.Rgb[1], obj.layer.Rgb[2]));

            if ((Object)obj.pFinAux != null)
                pFinAux = (TPonto)obj.pFinAux.Clone();
            if ((Object)obj.ultPonto != null)
                ultPonto = new vec3(obj.ultPonto.x, obj.ultPonto.y, obj.ultPonto.z);

            if ((Object)obj.n1 != null)
                n1 = new vec3(obj.n1.x, obj.n1.y, obj.n1.z);
            if ((Object)obj.v1 != null)
                v1 = new vec3(obj.v1.x, obj.v1.y, obj.v1.z);
            if ((Object)obj.v2 != null)
                v2 = new vec3(obj.v2.x, obj.v2.y, obj.v2.z);
            if ((Object)obj.p1 != null)
                p1 = new vec3(obj.p1.x, obj.p1.y, obj.p1.z);
            if ((Object)obj.p2 != null)
                p2 = new vec3(obj.p2.x, obj.p2.y, obj.p2.z);
            if ((Object)obj.p3 != null)
                p3 = new vec3(obj.p3.x, obj.p3.y, obj.p3.z);
            
            alfa = obj.alfa;
            xini = obj.xini;
            yini = obj.yini;
            xfin = obj.xfin;
            yfin = obj.yfin;
            DifCoordX = obj.DifCoordX;
            DifCoordY = obj.DifCoordY;
            Comp = obj.Comp;
            angFin = obj.angFin;
            angIni = obj.angIni;
            dx = obj.dx;
            dy = obj.dy;
            inicioAnguloX = obj.inicioAnguloX;
            inicioAnguloY = obj.inicioAnguloY;
            PontoY = obj.PontoY;
            layer = obj.layer;
            VerticeAtual = obj.VerticeAtual;
            ultX = obj.ultX;
            ultY = obj.ultY; ;
            DefinindoAngulo = obj.DefinindoAngulo; ;
            angAnterior = obj.angAnterior;
            UmOitoZero_Ou_TresSeisZero = obj.UmOitoZero_Ou_TresSeisZero; ;

            if ((Object)obj.Dados != null)
            {
                this.Dados = (TDadosPilar)obj.Dados.Clone();

                for (int i = 0; i < Dados.Poligono.linhas_poligonal.Count; i++)
                {
                    Dados.Poligono.linhas_poligonal[i].LinhaContornoPilar = true;
                    Dados.Poligono.linhas_poligonal[i].layer = this.layer;
                
             //       Dados.Poligono.linhas_poligonal[i].pIni.x = obj.Dados.Poligono.linhas_poligonal[i].pIni.x;
             //       Dados.Poligono.linhas_poligonal[i].pIni.y = obj.Dados.Poligono.linhas_poligonal[i].pIni.y;

            //        Dados.Poligono.linhas_poligonal[i].pFin.x = obj.Dados.Poligono.linhas_poligonal[i].pFin.x;
           //         Dados.Poligono.linhas_poligonal[i].pFin.y = obj.Dados.Poligono.linhas_poligonal[i].pFin.y;
                }

              //  Dados.Poligono.AtualizaCoords();
              //  Dados.Poligono.CalculaPropriedades();
            }
        }

        public bool PontoDentroOuNaAresta(double x, double y, double tol = 0.01)
        {
            if (Dados.Poligono.PontoEmPoligono(x, y)) 
                return true;

            if (PontoEmAresta(x, y, tol))
                return true;

            return false;
        }

        public override TObjetoDesenho Clone()
        {
            TPilar l = new TPilar();
            l.Copy(this);
            return l;
        }
    }
}