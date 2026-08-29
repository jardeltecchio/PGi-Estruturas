using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using OpenTK;
using OpenTK.Input;
using OpenTK.Graphics.OpenGL;
using OpenTK.Platform;
using GeometryUtility;
using PolygonCuttingEar;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.IO;
using System.Drawing.Imaging;

namespace PG
{
    [Serializable]
    public class TLaje : TObjetoDesenho
    {
        public TLaje() 
        {
            base.Visivel = true;
            base.IdLayer = Lay.Lajes;
            this.Tipo    = Const.ID_LAJE;
        }

        public override TObjetoDesenho Clone()
        {
            TLaje l = new TLaje();
            l.Copy(this);
            return l;
        }

        public override void AddGrips()
        {
            Grips = new List<TGrip>();

            Grips.Add(new TGrip(this, this.Titulo.x, this.Titulo.y+20, true, false, false, false, this.layer));
        }

        public override void ShowHideGrips(bool visivel)
        {
            foreach (TGrip grip in Grips) // aqui é um, mas pode ter mais de um grip por objeto, então é melhor padronizar fazendo um foreach... 
                grip.Visivel = visivel;
        }
        public override string PrimeiroComando()
        {
            return Const.CMD_LAJE_1_P;
        }
        public override void Desenha(ref bool Unifilar, ref int transp, ref bool arestas)
        {
            //   GL.Enable(Gl.GL_LINE_STIPPLE);
            //   GL.LineStipple(4, unchecked((short)0x6666)); 

            //no primeiro clique da laje, essa nao é desenhada, apesar de passar por essa rotina
            // na verdade o layer dos vigas desenha a viga por cima, já que o poligono da laje coincide com a face interna da viga
            // é que o repaintobject é chamado antes do DrawByLayer, e o drawbylayer ainda não contem a laje no layer de Lajes
            // ai quando damos o segundo clique da laje, essa passa a ser desenhada, pois adicionou no layer e passa a ser desenhada após o layer das vigas

            // GL.Color3(.8f, .1f, .9);
            // foreach (TLinha lin in poligono.linhas_poligonal)
            //   g2d.line(Desenho.pixelX(lin.pIni.x), Desenho.pixelY(lin.pIni.y), Desenho.pixelX(lin.pFin.x), Desenho.pixelY(lin.pFin.y));
            // GL.Disable(Gl.GL_LINE_STIPPLE);

            /*    GL.Enable(Gl.GL_BLEND);
        
            
                GL.BlendFunc(Gl.GL_SRC_ALPHA, Gl.GL_ONE_MINUS_SRC_ALPHA);

                GL.Begin(PrimitiveType.Polygon);
                GL.Color4(this.layer.Rgb[0], this.layer.Rgb[1], this.layer.Rgb[2], 90);
                for (int j = 0; j < pts_poligono.Count; j++)
                  GL.Vertex2(Desenho.pixelX(pts_poligono[j].x), Desenho.pixelY(pts_poligono[j].y));
   
                GL.End();*/

            try
            {
                if (base.Selecionado)
                {


                    //       GL.Color3f(1, 0, 0);
                    //      GL.Enable(Gl.GL_LINE_STIPPLE);
                    //         GL.LineStipple(1, unchecked((short)0xF0F0));
                }

                if (!Unifilar)
                if (PoligonoLaje != null)
                {
                    for (i = 0; i < PoligonoLaje.NumberOfPolygons; i++)
                    {
                        if (this.Selecionado)
                            GL.Color4(Color.Red);
                        //    else
                        //      GL.Color4(rgb[0], rgb[1], rgb[2], transparencia);
                        GL.Color4(Color.FromArgb(transp, this.layer.Rgb[0], this.layer.Rgb[1], this.layer.Rgb[2]));

                     ///   p1 = new vec3(PoligonoLaje.Polygons(i)[0].X, PoligonoLaje.Polygons(i)[0].Y, Estrutura.pavimentos[this.Pavimento].Nivel - 1);
                     //   p2 = new vec3(PoligonoLaje.Polygons(i)[1].X, PoligonoLaje.Polygons(i)[1].Y, Estrutura.pavimentos[this.Pavimento].Nivel - 1);
                    ////    p3 = new vec3(PoligonoLaje.Polygons(i)[2].X, PoligonoLaje.Polygons(i)[2].Y, Estrutura.pavimentos[this.Pavimento].Nivel - 1);

                        ve1 = p2 - p1;
                        ve2 = p3 - p1;
                        ne1 = ve1.CrossProduct(ve2);
                        ne1.Normalize();

                        GL.Begin(PrimitiveType.Polygon);
                        GL.Normal3(-ne1.x, -ne1.y, -ne1.z);

                        for (j = 0; j < PoligonoLaje.Polygons(i).Length; j++)
                        {
                   //         GL.Vertex3(PoligonoLaje.Polygons(i)[j].X, PoligonoLaje.Polygons(i)[j].Y, Estrutura.pavimentos[this.Pavimento].Nivel - 1);
                        }

                        GL.End();

                        /* if (transparencia >= 250)
                         {
                             GL.Begin(PrimitiveType.Polygon);
                             GL.Normal3(ne1.x, ne1.y, ne1.z);
                             for (j = 0; j < PoligonoLaje.Polygons(i).Length; j++)
                             {
                                 GL.Vertex3(PoligonoLaje.Polygons(i)[j].X, PoligonoLaje.Polygons(i)[j].Y, Pavimento.Nivel - Dados.h - 1);
                             }

                             GL.End();

                             if (!MostrarVigas)
                                 foreach (TLinha Aresta in poligono.linhas_poligonal)
                                 {
                                     GL.Color4(rgb[0], rgb[1], rgb[2], transparencia);
                                     GL.Begin(PrimitiveType.Polygon);
                                     GL.Vertex3(Aresta.pIni.x - centroX, Aresta.pIni.y - centroY, h - 1);
                                     GL.Vertex3(Aresta.pIni.x - centroX, Aresta.pIni.y - centroY, h - Dados.h - 1);
                                     GL.Vertex3(Aresta.pFin.x - centroX, Aresta.pFin.y - centroY, h - Dados.h - 1);
                                     GL.Vertex3(Aresta.pFin.x - centroX, Aresta.pFin.y - centroY, h - 1);
                                     GL.End();

                                     GL.Color3(0, 0, 0);
                                     GL.Begin(PrimitiveType.LineLoop);
                                     GL.Vertex3(Aresta.pIni.x - centroX, Aresta.pIni.y - centroY, h - 1);
                                     GL.Vertex3(Aresta.pIni.x - centroX, Aresta.pIni.y - centroY, h - Dados.h - 1);
                                     GL.Vertex3(Aresta.pFin.x - centroX, Aresta.pFin.y - centroY, h - Dados.h - 1);
                                     GL.Vertex3(Aresta.pFin.x - centroX, Aresta.pFin.y - centroY, h - 1);
                                     GL.End();
                                 }

                         }*/
                    }
                }
            }
            catch (Exception e)
            {
                System.Windows.Forms.MessageBox.Show(e.Message);
            }
        }
   
        [NonSerialized]
        public System.Drawing.SolidBrush sbPoligono;
        [NonSerialized]
        public System.Drawing.Point[] pts_triangulo;
        double anguloAnterior;

        public override void Mover(ref TPonto ponto1, ref TPonto ponto2, bool dinamico)
        {
            pIni.Mover(ref ponto1, ref ponto2, dinamico);
            pFin.Mover(ref ponto1, ref ponto2, dinamico);
            // pontoInicial = pontoInicial.Rotate(ponto1, (ang - anguloAnterior) * Const.PIDiv180);

            foreach (TLinha l in poligono.linhas_poligonal)
                l.Mover(ref ponto1, ref ponto2, dinamico);

            foreach (TLinha l in linhas_poligonal)
                l.Mover(ref ponto1, ref ponto2, dinamico);

            foreach (TLinha l in PoligonoSelecao.linhas_poligonal)
                l.Mover(ref ponto1, ref ponto2, dinamico);

            for (int i = 0; i < pts_poligono.Count - 1; i++)
                pts_poligono[i].Mover(ref ponto1, ref ponto2, dinamico);
        }

        public override void Rotacionar(TPonto ponto1, TPonto ponto2, double ang)
        {
            pIni = pIni.Rotate(ponto1, (ang - anguloAnterior) * Const.PIDiv180);
            pFin = pFin.Rotate(ponto1, (ang - anguloAnterior) * Const.PIDiv180);
           // pontoInicial = pontoInicial.Rotate(ponto1, (ang - anguloAnterior) * Const.PIDiv180);

            foreach (TLinha l in poligono.linhas_poligonal)
                l.Rotacionar(ponto1, ponto2, ang); 
            
            foreach (TLinha l in linhas_poligonal)
                l.Rotacionar(ponto1, ponto2, ang);

            foreach (TLinha l in PoligonoSelecao.linhas_poligonal)
                l.Rotacionar(ponto1, ponto2, ang);

            for (int i = 0; i < pts_poligono.Count-1; i++)
            {
                pts_poligono[i] = pts_poligono[i].Rotate(ponto1, (ang - anguloAnterior) * Const.PIDiv180);
            }

            anguloAnterior = ang;
        }

        public override void Atualiza(int pavimento)
        {
            this.Pavimento = pavimento;
            
            AddTitulo(Dados.nome + Dados.numero);
            AddGrips();
            AddPoligonoSelecao();
            AtualizaPoligonoSelecao();

            base.angulo = (float)(FuncoesGerais.atand((pIni.y - pFin.y) / (pIni.x - pFin.x)));
            base.anguloGlobal = (float)RMath.rad2deg(pIni.getAngleTo(pFin));
            this.angulo = base.angulo;
            TriangularizarLaje();
        }
        public override eObjetoDesenhoMouseDown OnMouseDown(ref TPonto point, ref string command, List<TLinha> Linhas, List<TPonto> Pontos, List<TPilar> Pilares, ref List<TObjetoDesenho> Objects, bool FromGrip = false)
        {
            this.pFin = new TPonto(0);
            this.pFin.x = point.x;
            this.pFin.y = point.y;
            
            AddTitulo(Dados.nome + Dados.numero);
            AddGrips();
            AddPoligonoSelecao();
            
           // this.poligono.AtualizaCoords();        
           // this.poligono.CalculaPropriedades();
            CalculaArea();
            TriangularizarLaje();

            Inicializando = false;
            sbPoligono = new System.Drawing.SolidBrush(System.Drawing.Color.FromArgb(50, this.layer.Rgb[0], this.layer.Rgb[1], this.layer.Rgb[2]));
            pts_triangulo = new System.Drawing.Point[3];
            command = "";
            return eObjetoDesenhoMouseDown.Done;
        }

        public void CalculaArea()
        {
            CoordenadaD[] c = new CoordenadaD[pts_poligono.Count+1];
            int cc =0;
            foreach (TPonto pt in pts_poligono)
                c[cc++] = new CoordenadaD(pt.x, pt.y, 0);

            c[cc++] = new CoordenadaD(pts_poligono[0].x, pts_poligono[0].y, 0);
            
            TPoligono p = new TPoligono(c, true);
            this.area = p.area/10000;
        }

        void AddPoligonoSelecao()
        {
            coord[0].X = this.Grips[0].x;
            coord[0].Y = this.Grips[0].y-20;

            coord[1].X = this.Grips[0].x + 60;
            coord[1].Y = this.Grips[0].y - 20;

            coord[2].X = this.Grips[0].x + 60;
            coord[2].Y = this.Grips[0].y + 20 - 20;

            coord[3].X = this.Grips[0].x;
            coord[3].Y = this.Grips[0].y + 20 - 20;

            coord[4].X = this.Grips[0].x;
            coord[4].Y = this.Grips[0].y - 20;

            PoligonoSelecao = new TPoligono(coord);

            linhas_poligonal.Add(new TLinha(new TPonto(coord[0].X, coord[0].Y, 0), new TPonto(coord[1].X, coord[1].Y,0),-1));
            linhas_poligonal.Add(new TLinha(new TPonto(coord[1].X, coord[1].Y, 0), new TPonto(coord[2].X, coord[2].Y, 0), -1));
            linhas_poligonal.Add(new TLinha(new TPonto(coord[2].X, coord[2].Y, 0), new TPonto(coord[3].X, coord[3].Y, 0), -1));
            linhas_poligonal.Add(new TLinha(new TPonto(coord[3].X, coord[3].Y, 0), new TPonto(coord[4].X, coord[4].Y, 0), -1));
            PoligonoSelecao.linhas_poligonal = linhas_poligonal;
        }

        public void AtualizaPoligonoSelecao()
        {
            coord[0].X = this.Grips[0].x;
            coord[0].Y = this.Grips[0].y - 20;

            coord[1].X = this.Grips[0].x + 60;
            coord[1].Y = this.Grips[0].y - 20;

            coord[2].X = this.Grips[0].x + 60;
            coord[2].Y = this.Grips[0].y + 20 - 20;

            coord[3].X = this.Grips[0].x;
            coord[3].Y = this.Grips[0].y + 20 - 20;

            coord[4].X = this.Grips[0].x;
            coord[4].Y = this.Grips[0].y - 20;

            PoligonoSelecao.linhas_poligonal[0].pIni.x = coord[0].X;
            PoligonoSelecao.linhas_poligonal[0].pIni.y = coord[0].Y;
            PoligonoSelecao.linhas_poligonal[0].pFin.x = coord[1].X;
            PoligonoSelecao.linhas_poligonal[0].pFin.y = coord[1].Y;
        
            PoligonoSelecao.linhas_poligonal[1].pIni.x = coord[1].X;
            PoligonoSelecao.linhas_poligonal[1].pIni.y = coord[1].Y;
            PoligonoSelecao.linhas_poligonal[1].pFin.x = coord[2].X;
            PoligonoSelecao.linhas_poligonal[1].pFin.y = coord[2].Y;
       
            PoligonoSelecao.linhas_poligonal[2].pIni.x = coord[2].X;
            PoligonoSelecao.linhas_poligonal[2].pIni.y = coord[2].Y;
            PoligonoSelecao.linhas_poligonal[2].pFin.x = coord[3].X;
            PoligonoSelecao.linhas_poligonal[2].pFin.y = coord[3].Y;
           
            PoligonoSelecao.linhas_poligonal[3].pIni.x = coord[3].X;
            PoligonoSelecao.linhas_poligonal[3].pIni.y = coord[3].Y;
            PoligonoSelecao.linhas_poligonal[3].pFin.x = coord[4].X;
            PoligonoSelecao.linhas_poligonal[3].pFin.y = coord[4].Y;

            Titulo.OnMove(this.Grips[0].x,this.Grips[0].y-20);

            if (Titulo2 != null)
             Titulo2.OnMove(this.Grips[0].x+5, this.Grips[0].y-35);
        }

        public void AddTitulo(string nome)
        {
           /* Titulo = new TTexto(nome, this.pFin.x, this.pFin.y, (15.0 / 60),
                                      -(15.0 / 60),
                                      0,1,0,
                                      0, this.Estrutura.LayersByIdPrincipal[Lay.TextosLajes], this.Tipo);
            Titulo2 = new TTexto("h:" + this.Dados.h.ToString(), this.pFin.x+5, this.pFin.y-15, (15.0 / 150),
                                      -(15.0 / 150),
                                      0, 1, 0,
                                      0, this.Estrutura.LayersByIdPrincipal[Lay.TextosLajes], this.Tipo);*/
        }

        public override void Initialize(ref TPonto point, ref string command, ISettings Dados, TLayer layer, ref List<TLinha> Linhas, ref int pavimento)
        {
            this.Dados = Dados as TDadosLaje;
           
            this.Layer     = layer;
            this.Pavimento = pavimento;
            
            this.Linhas = Linhas;
    //        this.Pilares = estrutura.pilares;
            //mPen aqui só serve pra pintar o contorno na hora de inicializar
            mPen.Width = 4; 
            
            Inicializando = true;
            lastX = point.x;
            lastY = point.y;
            bar = new barra[7000];
            last_pts = new AnyPt[1000];
            pts = new AnyPt[1000];
            i_pts = 0;
            i_last_pts = 0;
            i_bar = 0;

            last_pts[i_last_pts++] = new AnyPt(point.x, point.y, true);
            poligono.linhas_poligonal = new List<TLinha>();
            poligono_eixo.linhas_poligonal = new List<TLinha>();

            this.pIni = new TPonto(0);
            this.pIni.x = point.x;
            this.pIni.y = point.y;
       
            erroInicializacao = false;
/*
            foreach (TLaje laje in estrutura.lajes)
            {
                if (laje.PontoEmPoligono(ref point.x, ref point.y))
                {
                    command = Const.CMD_LAJE_1_P;
                    Inicializando = false;
                    throw new TErroInicializacaoObjeto("Já existe uma laje nesse local.");
                }
            }*/

            FloodFillMesh();

            if (!erroInicializacao)
            {
                ProcuraBarrasRigidasFaltantesNoContorno();
                DefinePontosPoligono();
                OrdenaPontosAntiHorario(ref pt2);

                command = Const.CMD_LAJE_2_P;
                base.Initialize(ref point, ref command, Dados, layer, ref Linhas, ref pavimento);
            }
            else
            {
                command = Const.CMD_LAJE_1_P;
                Inicializando = false;
                throw new TErroInicializacaoObjeto("O sistema não conseguiu detectar um contorno fechado para a laje. Tente novamente.");
            }
        }

        void ProcuraBarrasRigidasFaltantesNoContorno()
        {
            List<TLinha> LinhasAdicionais = new List<TLinha>();
         
            /*Adiciona barra rigidas como eixo*/
            TTrechoViga trecho = null;
            foreach (TLinha lin in poligono_eixo.linhas_poligonal)
            {
                if (!lin.barraRigida)
                {
                    trecho = lin.TrechoViga;
                    TLinha br = trecho.BarraRigida_Ini;

                    if (br != null)
                      if (!poligono_eixo.FindLin(br))
                      {
                          LinhasAdicionais.Add(br);
                      }

                    br = trecho.BarraRigida_Fin;

                    if (br != null)
                      if (!poligono_eixo.FindLin(br))
                      {
                          LinhasAdicionais.Add(br);
                      }
                }
            }

            foreach (TLinha lin in LinhasAdicionais)
              poligono_eixo.linhas_poligonal.Add(lin);


            /*Adiciona barra rigidas como contorno de desenho*/
            poligono.linhas_poligonal.RemoveAll(obj => obj.barraRigida); //remove se houve barras rigidas como contorno de desenho, pois vai recriar a partir de agora...
           
            LinhasAdicionais.Clear();

            trecho = null;

            foreach (TLinha lin in poligono.linhas_poligonal)
            {
                if (!lin.barraRigida)
                {
                    trecho = lin.TrechoViga;
                                        
                    TLinha br = trecho.BarraRigida_Ini;
                    
                    if (br != null)
                    {
                        LinhasAdicionais.Add(new TLinha(new TPonto(lin.pIni.x, lin.pIni.y, 0), new TPonto(br.pIni.x, br.pIni.y, 0), -1));
                        LinhasAdicionais.Add(new TLinha(new TPonto(br.pIni.x, br.pIni.y, 0), new TPonto(br.pFin.x, br.pFin.y, 0), -1));
                    }

                    br = trecho.BarraRigida_Fin;

                    if (br != null)
                    {
                        LinhasAdicionais.Add(new TLinha(new TPonto(lin.pFin.x, lin.pFin.y, 0), new TPonto(br.pIni.x, br.pIni.y, 0), -1));
                        LinhasAdicionais.Add(new TLinha(new TPonto(br.pIni.x, br.pIni.y, 0), new TPonto(br.pFin.x, br.pFin.y, 0), -1));
                    }
                }
            }

            foreach (TLinha lin in LinhasAdicionais)
            {
                lin.barraRigida = true;
                poligono.linhas_poligonal.Add(lin);
            }
        }

        private void DefinePontosPoligono()
        {
            double xIni = 99999999999;
            pts_poligono.Clear();

            for (int j = 0; j < poligono.linhas_poligonal.Count; j++)
            {
                if (!locPonto(poligono.linhas_poligonal[j].pIni))
                {
                    pts_poligono.Add(poligono.linhas_poligonal[j].pIni);
                    pts_poligono[pts_poligono.Count - 1].codigo = 1;
                }

                if (!locPonto(poligono.linhas_poligonal[j].pFin))
                {
                    pts_poligono.Add(poligono.linhas_poligonal[j].pFin);
                    pts_poligono[pts_poligono.Count - 1].codigo = 2;
                }
            }

            for (int i = 0; i < pts_poligono.Count; i++)
            {
                if (pts_poligono[i].x < xIni)
                {
                    pt1 = pts_poligono[i];
                    xIni = pt1.x;
                }
            };

            for (int j = 0; j < poligono.linhas_poligonal.Count; j++)
            {
                if ((Object)pt1 == null) throw new Exception("Ponto ''pt1'' nulo.");

                if ((Object)poligono.linhas_poligonal[j].pIni == null) throw new Exception("Ponto ''poligono.linhas_poligonal[j].pIni'' nulo.");

                if (pt1 == poligono.linhas_poligonal[j].pIni)
                {
                    l1 = poligono.linhas_poligonal[j];
                    linhasAdd.Add(l1);

                    if (pt1.y < poligono.linhas_poligonal[j].pFin.y)
                    {
                        pt1 = poligono.linhas_poligonal[j].pFin;
                        pt2 = poligono.linhas_poligonal[j].pIni;
                    }
                    else
                        if (pt1.y > poligono.linhas_poligonal[j].pFin.y)
                    {
                        pt1 = poligono.linhas_poligonal[j].pIni;
                        pt2 = poligono.linhas_poligonal[j].pFin;
                    }

                    break;
                }

                if ((Object)pt1 == null) throw new Exception("Ponto ''pt1'' nulo.");

                if ((Object)poligono.linhas_poligonal[j].pIni == null) throw new Exception("Ponto ''poligono.linhas_poligonal[j].pIni'' nulo.");

                if (pt1 == poligono.linhas_poligonal[j].pFin)
                {
                    l1 = poligono.linhas_poligonal[j];

                    if (pt1.y < poligono.linhas_poligonal[j].pIni.y)
                    {
                        pt1 = poligono.linhas_poligonal[j].pIni;
                        pt2 = poligono.linhas_poligonal[j].pFin;
                    }
                    else
                        if (pt1.y > poligono.linhas_poligonal[j].pIni.y)
                    {
                        pt1 = poligono.linhas_poligonal[j].pFin;
                        pt2 = poligono.linhas_poligonal[j].pIni;
                    }

                    break;
                }
            }

            pts_poligono.Clear();

            if (Geom.Iguais(l1.pIni.y, l1.pFin.y))
            {
                pt1 = l1.pIni;
                pt2 = l1.pFin;
            }
            else
            {
                if (l1.pIni.y > l1.pFin.y)
                {
                    if (l1.pIni.x < l1.pFin.x)
                    {
                    //    pt1 = l1.pIni;
                        pt2 = l1.pFin;
                    }
                    else
                    if (l1.pIni.x > l1.pFin.x)
                    {
                  //      pt1 = l1.pFin;
                        pt2 = l1.pIni;
                    }
                }
                else
                if (l1.pIni.y < l1.pFin.y)
                {
                    if (l1.pIni.x < l1.pFin.x)
                    {
               //         pt1 = l1.pFin;
                        pt2 = l1.pIni;
                    }
                    else
                    if (l1.pIni.x > l1.pFin.x)
                    {
               //         pt1 = l1.pFin;
                        pt2 = l1.pIni;
                    }
                }
            }

           // pts_poligono.Add(poligono.linhas_poligonal[2].pIni);
           // pts_poligono.Add(poligono.linhas_poligonal[2].pFin);
            linhasAdd.Clear();
            pt1 = poligono.linhas_poligonal[2].pIni;

            pontoInicial = pt1;

            pt2 = poligono.linhas_poligonal[2].pFin;
            linhasAdd.Add(poligono.linhas_poligonal[2]);

            pts_poligono.Add(pt1);
            pts_poligono.Add(pt2);       
        }


        bool FindBar(double xi, double yi, double xf, double yf)
        {
            foreach (barra b in bar)
                if ((Geom.Iguais(b.xi, xi) && Geom.Iguais(b.yi, yi) && Geom.Iguais(b.xf, xf) && Geom.Iguais(b.yf, yf))
                 || (Geom.Iguais(b.xf, xi) && Geom.Iguais(b.yf, yi) && Geom.Iguais(b.xi, xf) && Geom.Iguais(b.yi, yf)))
                    return true;

            return false;
        }

        Linha outra = new Linha(0, 0, 0, 0);

        int ii, jk;
        double lx, ly, inx, iny;
        int inc = 35;
        bool intersec;

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
                        l1 = null;

                        outra.pFin.x = last_pts[ii].x + inc;
                        outra.pFin.y = last_pts[ii].y;

                        foreach (TLinha lin in Linhas)
                        {
                            if (!lin.auxiliar)
                              if  (((Object)lin.TrechoViga != null && !lin.LinhaEixoViga && !lin.auxiliar)/* || lin.barraRigida*/)
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
                                }
                        }

                        if (intersec)
                        {
                            if (!poligono.FindLin(l1))
                            {
                                //    poligono.linhas_poligonal.Add(new TLinha(l1.pIni, l1.pFin,-1));
                                poligono.linhas_poligonal.Add(l1.Clone() as TLinha);
                                poligono.linhas_poligonal[poligono.linhas_poligonal.Count - 1].TrechoViga = l1.TrechoViga;
                            }

                            if (!poligono_eixo.FindLin(l1.TrechoViga.linhas_eixo))
                                poligono_eixo.linhas_poligonal.Add(l1.TrechoViga.linhas_eixo);
                        }
                        else
                        foreach (TPilar PilarAtual in Pilares)
                        {
                            foreach (TLinha ArestaAtual in PilarAtual.Dados.Poligono.linhas_poligonal)
                            {
                                if (ArestaAtual.Intersec(outra, ref inx, ref iny))
                                {
                                    pts[i_pts - 1].x = inx;
                                    pts[i_pts - 1].y = iny;
                                    pts[i_pts - 1].continua = false;

                                    outra.pFin.x = inx;
                                    outra.pFin.y = iny;
                                    break;
                                }
                            }
                        }

                     /*   for (int j = 0; j < i_bar; j++)
                        {
                            GL.Color3f(0, 1, 0);
                            GL.Begin(Gl.GL_LINES);
                            GL.Vertex2d(Desenho.pixelX(bar[j].xi), Desenho.pixelY(bar[j].yi));
                            GL.Vertex2d(Desenho.pixelX(bar[j].xf), Desenho.pixelY(bar[j].yf));
                            GL.End();
                            Pavimento.desenho.Controle.SwapBuffers();
                        } 
                        */
                        if (i_bar + 1 > 4700)
                        {
                            erroInicializacao = true;
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

                        foreach (TLinha lin in Linhas)
                        {
                            if (!lin.auxiliar)
                                if (((Object)lin.TrechoViga != null && !lin.LinhaEixoViga && !lin.auxiliar)/* || lin.barraRigida*/)
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
                                }
                        }

                        if (intersec)
                        {
                            if (!poligono.FindLin(l1))
                            {
                                //    poligono.linhas_poligonal.Add(new TLinha(l1.pIni, l1.pFin,-1));
                                poligono.linhas_poligonal.Add(l1.Clone() as TLinha);
                                poligono.linhas_poligonal[poligono.linhas_poligonal.Count - 1].TrechoViga = l1.TrechoViga;
                            }

                            if (!poligono_eixo.FindLin(l1.TrechoViga.linhas_eixo))
                                poligono_eixo.linhas_poligonal.Add(l1.TrechoViga.linhas_eixo);
                        }
                        else
                            foreach (TPilar PilarAtual in Pilares)
                            {
                                foreach (TLinha ArestaAtual in PilarAtual.Dados.Poligono.linhas_poligonal)
                                {
                                    if (ArestaAtual.Intersec(outra, ref inx, ref iny))
                                    {
                                        pts[i_pts - 1].x = inx;
                                        pts[i_pts - 1].y = iny;
                                        pts[i_pts - 1].continua = false;

                                        outra.pFin.x = inx;
                                        outra.pFin.y = iny;
                                        break;
                                    }
                                }
                            }
                     /*   for (int j = 0; j < i_bar; j++)
                        {
                            GL.Color3f(0, 1, 0);
                            GL.Begin(Gl.GL_LINES);
                            GL.Vertex2d(Desenho.pixelX(bar[j].xi), Desenho.pixelY(bar[j].yi));
                            GL.Vertex2d(Desenho.pixelX(bar[j].xf), Desenho.pixelY(bar[j].yf));
                            GL.End();
                            Pavimento.desenho.Controle.SwapBuffers();
                        } */

                        if (i_bar + 1 > 4700)
                        {
                            erroInicializacao = true;
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

                        foreach (TLinha lin in Linhas)
                        {
                            if (!lin.auxiliar)
                                if (((Object)lin.TrechoViga != null && !lin.LinhaEixoViga && !lin.auxiliar) /*|| lin.barraRigida*/)
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
                                }
                        }

                        if (intersec)
                        {
                            if (!poligono.FindLin(l1))
                            {
                                //    poligono.linhas_poligonal.Add(new TLinha(l1.pIni, l1.pFin,-1));
                                poligono.linhas_poligonal.Add(l1.Clone() as TLinha);
                                poligono.linhas_poligonal[poligono.linhas_poligonal.Count - 1].TrechoViga = l1.TrechoViga;
                            }

                            if (!poligono_eixo.FindLin(l1.TrechoViga.linhas_eixo))
                                poligono_eixo.linhas_poligonal.Add(l1.TrechoViga.linhas_eixo);
                        }
                        else
                            foreach (TPilar PilarAtual in Pilares)
                            {
                                foreach (TLinha ArestaAtual in PilarAtual.Dados.Poligono.linhas_poligonal)
                                {
                                    if (ArestaAtual.Intersec(outra, ref inx, ref iny))
                                    {
                                        pts[i_pts - 1].x = inx;
                                        pts[i_pts - 1].y = iny;
                                        pts[i_pts - 1].continua = false;

                                        outra.pFin.x = inx;
                                        outra.pFin.y = iny;
                                        break;
                                    }
                                }
                            }
                      /*  for (int j = 0; j < i_bar; j++)
                        {
                            GL.Color3f(0, 1, 0);
                            GL.Begin(Gl.GL_LINES);
                            GL.Vertex2d(Desenho.pixelX(bar[j].xi), Desenho.pixelY(bar[j].yi));
                            GL.Vertex2d(Desenho.pixelX(bar[j].xf), Desenho.pixelY(bar[j].yf));
                            GL.End();
                            Pavimento.desenho.Controle.SwapBuffers();
                        } */

                        if (i_bar + 1 > 4700)
                        {
                            erroInicializacao = true;
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
                        foreach (TLinha lin in Linhas)
                        {
                            if (!lin.auxiliar)
                                if (((Object)lin.TrechoViga != null && !lin.LinhaEixoViga && !lin.auxiliar) /*|| lin.barraRigida*/)
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
                                }
                        }

                        if (intersec)
                        {
                            if (!poligono.FindLin(l1))
                            {
                                //    poligono.linhas_poligonal.Add(new TLinha(l1.pIni, l1.pFin,-1));
                                poligono.linhas_poligonal.Add(l1.Clone() as TLinha);
                                poligono.linhas_poligonal[poligono.linhas_poligonal.Count - 1].TrechoViga = l1.TrechoViga;
                            }

                            if (!poligono_eixo.FindLin(l1.TrechoViga.linhas_eixo))
                                poligono_eixo.linhas_poligonal.Add(l1.TrechoViga.linhas_eixo);
                        }
                        else
                            foreach (TPilar PilarAtual in Pilares)
                            {
                                foreach (TLinha ArestaAtual in PilarAtual.Dados.Poligono.linhas_poligonal)
                                {
                                    if (ArestaAtual.Intersec(outra, ref inx, ref iny))
                                    {
                                        pts[i_pts - 1].x = inx;
                                        pts[i_pts - 1].y = iny;
                                        pts[i_pts - 1].continua = false;

                                        outra.pFin.x = inx;
                                        outra.pFin.y = iny;
                                        break;
                                    }
                                }
                            }
                     /*   for (int j = 0; j < i_bar; j++)
                        {
                            GL.Color3f(0, 1, 0);
                            GL.Begin(Gl.GL_LINES);
                            GL.Vertex2d(Desenho.pixelX(bar[j].xi), Desenho.pixelY(bar[j].yi));
                            GL.Vertex2d(Desenho.pixelX(bar[j].xf), Desenho.pixelY(bar[j].yf));
                            GL.End();
                            Pavimento.desenho.Controle.SwapBuffers();
                        } */

                        if (i_bar + 1 > 4700)
                        {
                            erroInicializacao = true;
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

        private bool locLinha(TLinha l)
        {
            foreach (TLinha lin in linhasAdd)
                if ((Object)l == (Object)lin)
                    return true;
            return false;
        }

        private bool locPonto(TPonto p)
        {
            foreach (TPonto pt in pts_poligono)
                if (Geom.Iguais(p.x, pt.x) && Geom.Iguais(p.y, pt.y))
                    return true;
            return false;
        }

        TPonto pt1, pt2 = null;
        private void OrdenaPontosAntiHorario(ref TPonto pontoFinal)
        {
            for (int j = 0; j < poligono.linhas_poligonal.Count; j++)
            {
                if (!locLinha(poligono.linhas_poligonal[j]))
                {
                    if (Geom.Iguais(poligono.linhas_poligonal[j].pFin.x, pontoFinal.x) && Geom.Iguais(poligono.linhas_poligonal[j].pFin.y, pontoFinal.y))
                    {
                        pontoFinal = poligono.linhas_poligonal[j].pIni;

                        if (Geom.Iguais(pontoInicial.x, pontoFinal.x) && Geom.Iguais(pontoInicial.y, pontoFinal.y))
                          return;
                      
                        if (!locPonto(pontoFinal))
                          pts_poligono.Add(pontoFinal);

                        linhasAdd.Add(poligono.linhas_poligonal[j]);

                        OrdenaPontosAntiHorario(ref pontoFinal);
                    }
                    else
                    if (Geom.Iguais(poligono.linhas_poligonal[j].pIni.x, pontoFinal.x) && Geom.Iguais(poligono.linhas_poligonal[j].pIni.y, pontoFinal.y))
                    {
                        pontoFinal = poligono.linhas_poligonal[j].pFin;

                        if (Geom.Iguais(pontoInicial.x, pontoFinal.x) && Geom.Iguais(pontoInicial.y, pontoFinal.y))
                          return;

                        if (!locPonto(pontoFinal))
                          pts_poligono.Add(pontoFinal);

                        linhasAdd.Add(poligono.linhas_poligonal[j]);
                        OrdenaPontosAntiHorario(ref pontoFinal);
                    }                    
                }
            }
        }

        [NonSerializedAttribute]
        public CPolygonShape PoligonoLaje;
        public void TriangularizarLaje()
        {
            CPoint2D[] vertices;
            int ii;

            vertices = new CPoint2D[this.pts_poligono.Count];
                
            ii = 0;

            foreach (TPonto Ponto in this.pts_poligono)
              vertices[ii++] = new CPoint2D(Ponto.x, Ponto.y);

            PoligonoLaje = new CPolygonShape(vertices);
            PoligonoLaje.CutEar();
        }
        bool MostrarVigas = true;
        [NonSerialized]
        vec3 ne1, ve1, ve2, p1, p2, p3;
        private static int[] texture = new int[1];                              // Storage For One Texture ( NEW )
        public void CarregaTexturas()
        {
         /*   bool status = false;                                                // Status Indicator
            Bitmap[] textureImage = new Bitmap[1];                              // Create Storage Space For The Texture

            textureImage[0] = LoadBMP("Solo");                // Load The Bitmap
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
        public void Render3DTextura(double centroX, double centroY, double h, byte transparencia, byte[] rgb)
        {
            if (PoligonoLaje != null)
            {
                for (i = 0; i < PoligonoLaje.NumberOfPolygons; i++)
                {
                    if (this.Selecionado)
                        GL.Color3(255, 0, 0);
                    else
                        GL.Color4(rgb[0], rgb[1], rgb[2], transparencia);

                    p1 = new vec3(PoligonoLaje.Polygons(i)[0].Y - centroY, h - 1, PoligonoLaje.Polygons(i)[0].X - centroX);
                    p2 = new vec3(PoligonoLaje.Polygons(i)[1].Y - centroY, h - 1, PoligonoLaje.Polygons(i)[1].X - centroX);
                    p3 = new vec3(PoligonoLaje.Polygons(i)[2].Y - centroY, h - 1, PoligonoLaje.Polygons(i)[2].X - centroX);

                    ve1 = p2 - p1;
                    ve2 = p3 - p1;
                    ne1 = ve1.CrossProduct(ve2);
                    ne1.Normalize();

                    GL.Begin(PrimitiveType.Polygon);
                    GL.Normal3(0, -1, 0);
                //    GL.BindTexture(Gl.GL_TEXTURE_2D, texture[0]);

                    for (j = 0; j < PoligonoLaje.Polygons(i).Length; j++)
                    {
                        if (j == 0)
                            GL.TexCoord2(0, 0);
                        else
                            if (j == 1)
                                GL.TexCoord2(1, 0);
                            else
                                if (j == 2)
                                    GL.TexCoord2(0, 1);

                        GL.Vertex3(PoligonoLaje.Polygons(i)[j].Y - centroY, h - 1, PoligonoLaje.Polygons(i)[j].X - centroX);
                    }

                    GL.End();

                    if (transparencia >= 250)
                    {
                        GL.Begin(PrimitiveType.Polygon);
                        GL.Normal3(0, -1, 0);
                        for (j = 0; j < PoligonoLaje.Polygons(i).Length; j++)
                        {
                            if (j == 0)
                                GL.TexCoord2(0, 0);
                            else
                                if (j == 1)
                                    GL.TexCoord2(1, 0);
                                else
                                    if (j == 2)
                                        GL.TexCoord2(0, 1);
                            GL.Vertex3(PoligonoLaje.Polygons(i)[j].Y - centroY, h - Dados.h - 1, PoligonoLaje.Polygons(i)[j].X - centroX);
                        }

                        GL.End();

                        if (!MostrarVigas)
                            foreach (TLinha Aresta in poligono.linhas_poligonal)
                            {
                                GL.Color4(rgb[0], rgb[1], rgb[2], transparencia);
                                GL.Begin(PrimitiveType.Polygon);
                                GL.Vertex3(Aresta.pIni.y - centroY, h - 1, Aresta.pIni.x - centroX);
                                GL.Vertex3(Aresta.pIni.y - centroY, h - Dados.h - 1, Aresta.pIni.x - centroX);
                                GL.Vertex3(Aresta.pFin.y - centroY, h - Dados.h - 1, Aresta.pFin.x - centroX);
                                GL.Vertex3(Aresta.pFin.y - centroY, h - 1, Aresta.pFin.x - centroX);
                                GL.End();

                                GL.Color3(0, 0, 0);
                                GL.Begin(PrimitiveType.LineLoop);
                                GL.Vertex3(Aresta.pIni.y - centroY, h - 1, Aresta.pIni.x - centroX);
                                GL.Vertex3(Aresta.pIni.y - centroY, h - Dados.h - 1, Aresta.pIni.x - centroX);
                                GL.Vertex3(Aresta.pFin.y - centroY, h - Dados.h - 1, Aresta.pFin.x - centroX);
                                GL.Vertex3(Aresta.pFin.y - centroY, h - 1, Aresta.pFin.x - centroX);
                                GL.End();
                            }

                    }
                }
            }

            //   GL.Color3(0, 0, 0); 
            // GL.Begin(PrimitiveType.LineLoop);


            /*    GL.Color4(0, 0, 0, 100);
                GL.Begin(PrimitiveType.LineLoop);
                GL.Vertex3(Aresta.pIni.y - centroY, -1, Aresta.pIni.y - centroX);
                GL.Vertex3(Aresta.pIni.y - centroY, -Dados.h, Aresta.pIni.x - centroX);
                GL.Vertex3(Aresta.pFin.y - centroY, -Dados.h, Aresta.pFin.x - centroX);
                GL.Vertex3(Aresta.pFin.y - centroY, -1, Aresta.pFin.x - centroX);
                GL.End();*/
            //  }
            // GL.End();
        }

        public void Render3D(double centroX, double centroY, double h, byte transparencia, byte[] rgb)
        {
            if (PoligonoLaje != null)
            {
                for (i = 0; i < PoligonoLaje.NumberOfPolygons; i++)
                {
                    if (this.Selecionado)
                        GL.Color4(Color.Red);
                    else
                        GL.Color4(rgb[0], rgb[1], rgb[2], transparencia);

                    p1 = new vec3(PoligonoLaje.Polygons(i)[0].X - centroX, PoligonoLaje.Polygons(i)[0].Y - centroY, h - 1);
                    p2 = new vec3(PoligonoLaje.Polygons(i)[1].X - centroX, PoligonoLaje.Polygons(i)[1].Y - centroY, h - 1);
                    p3 = new vec3(PoligonoLaje.Polygons(i)[2].X - centroX, PoligonoLaje.Polygons(i)[2].Y - centroY, h - 1);

                    ve1 = p2 - p1;
                    ve2 = p3 - p1;
                    ne1 = ve1.CrossProduct(ve2);
                    ne1.Normalize();

                    GL.Begin(PrimitiveType.Polygon);
                    GL.Normal3(-ne1.x, -ne1.y, -ne1.z);

                    for (j = 0; j < PoligonoLaje.Polygons(i).Length; j++)
                    {
                        GL.Vertex3(PoligonoLaje.Polygons(i)[j].X - centroX,PoligonoLaje.Polygons(i)[j].Y - centroY, h - 1);
                    }
                    
                    GL.End();
                    
                    if (transparencia >= 250)
                    {
                        GL.Begin(PrimitiveType.Polygon);
                        GL.Normal3(ne1.x, ne1.y, ne1.z);
                        for (j = 0; j < PoligonoLaje.Polygons(i).Length; j++)
                        {
                            GL.Vertex3(PoligonoLaje.Polygons(i)[j].X - centroX,PoligonoLaje.Polygons(i)[j].Y - centroY, h - Dados.h - 1);
                        }

                        GL.End();

                        if (!MostrarVigas)
                        foreach (TLinha Aresta in poligono.linhas_poligonal)
                        {
                            GL.Color4(rgb[0], rgb[1], rgb[2], transparencia);
                            GL.Begin(PrimitiveType.Polygon);
                            GL.Vertex3(Aresta.pIni.x - centroX, Aresta.pIni.y - centroY, h - 1);
                            GL.Vertex3(Aresta.pIni.x - centroX, Aresta.pIni.y - centroY, h - Dados.h - 1);
                            GL.Vertex3(Aresta.pFin.x - centroX, Aresta.pFin.y - centroY, h - Dados.h - 1);
                            GL.Vertex3(Aresta.pFin.x - centroX, Aresta.pFin.y - centroY, h - 1);
                            GL.End();
                            
                            GL.Color3(0, 0, 0); 
                            GL.Begin(PrimitiveType.LineLoop);
                            GL.Vertex3(Aresta.pIni.x - centroX, Aresta.pIni.y - centroY, h - 1);
                            GL.Vertex3(Aresta.pIni.x - centroX, Aresta.pIni.y - centroY, h - Dados.h - 1);
                            GL.Vertex3(Aresta.pFin.x - centroX, Aresta.pFin.y - centroY, h - Dados.h - 1);
                            GL.Vertex3(Aresta.pFin.x - centroX, Aresta.pFin.y - centroY, h - 1);
                            GL.End();
                        }
                    
                    }
                }
            }

         //   GL.Color3(0, 0, 0); 
           // GL.Begin(PrimitiveType.LineLoop);

            
            /*    GL.Color4(0, 0, 0, 100);
                GL.Begin(PrimitiveType.LineLoop);
                GL.Vertex3(Aresta.pIni.y - centroY, -1, Aresta.pIni.y - centroX);
                GL.Vertex3(Aresta.pIni.y - centroY, -Dados.h, Aresta.pIni.x - centroX);
                GL.Vertex3(Aresta.pFin.y - centroY, -Dados.h, Aresta.pFin.x - centroX);
                GL.Vertex3(Aresta.pFin.y - centroY, -1, Aresta.pFin.x - centroX);
                GL.End();*/
          //  }
           // GL.End();
        }

        public bool PontoEmPoligono(ref double x, ref double y)
        {
            bool inside = false;
            double x_intersec;

            foreach (TLinha lin in poligono_eixo.linhas_poligonal)
            {
                if (!lin.LinhaEixoViga || lin.auxiliar) continue;

                if (((lin.pIni.y > y) && (lin.pFin.y < y)) ||
                    ((lin.pIni.y < y) && (lin.pFin.y > y)))
                {
                    //    x_intersec = U0.x + (P.y -U0.y) * (U1.x - U0.x) / (U1.y - U0.y);

                    //livro "geometric tools for computer graphics"  pg.700
                    x_intersec = lin.pIni.x + (y - lin.pIni.y) * (lin.pFin.x - lin.pIni.x) / (lin.pFin.y - lin.pIni.y);

                    if (x_intersec > x)
                        inside = !inside;
                };
            };

            return inside;
        }

        public override bool Selecionar(float clicx, float clicy, float coordx, float coordy, TObjetoDesenho Owner = null)
        {
            try
            {
                if (this.layer.Congelado || this.layer.Travado)
                    return false;

                if (PoligonoSelecao != null)
                    if (PoligonoSelecao.PontoEmPoligono(coordx, coordy))
                    {
                        SetaSelecao(true, true);
                        return true;
                    }
            }
            catch(Exception)
            {
                MessageBox.Show("Erro ao selecionar a laje + " + this.Titulo.texto);
            }
            return false;
        }
        public override void SetaSelecao(bool s, bool MostraGrip,bool SelecaoDeCandidato=false, TObjetoDesenho Owner = null)
        {
            if (this.layer.Congelado || this.layer.Travado)
                return;

            base.Selecionado = s;

            if ((Object)Titulo != null)
            {
                Titulo.SetaSelecao(s, false, SelecaoDeCandidato);
                Titulo2.SetaSelecao(s, false, SelecaoDeCandidato);
            }
            base.MostrarGrip = MostraGrip;
            ShowHideGrips(MostraGrip);
        }

        [NonSerialized]
        public System.Drawing.Pen mPen = new System.Drawing.Pen(System.Drawing.Color.Red);
        public TTexto Titulo, Titulo2;
        public TPoligono poligono = new TPoligono();
        public TPoligono poligono_eixo = new TPoligono();
        public TDadosLaje Dados;
        public double PP, area;

        public int Pavimento;

        public List<TPonto> pts_poligono = new List<TPonto>();
        [NonSerialized]
        List<TLinha> linhasAdd = new List<TLinha>();
        public double espacamentoRealX, espacamentoRealY;
        [NonSerialized]
        TLinha l1 = null;
        TPoligono PoligonoSelecao;
        List<TLinha> linhas_poligonal = new List<TLinha>();
        CoordenadaD[] coord = new CoordenadaD[5];
        bool Inicializando;
        [NonSerialized]
        TPonto pontoInicial;
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
        [NonSerialized]
        List<TLinha> Linhas = new List<TLinha>();
        [NonSerialized]
        List<TPilar> Pilares = new List<TPilar>();
        bool erroInicializacao;
        [NonSerialized]
        vec3 n1, v1, v2;
        int i, j;
        public void Copy(TLaje obj)
        {
            base.Copy(obj);

            if ((Object)obj.pIni != null)
                pIni = (TPonto)obj.pIni.Clone();

            if ((Object)obj.pFin != null)
                pFin = (TPonto)obj.pFin.Clone();
            
            PP = obj.PP;
            area = obj.area;
            espacamentoRealX = obj.espacamentoRealX;
            espacamentoRealY = obj.espacamentoRealY;

            mPen = new System.Drawing.Pen(System.Drawing.Color.Red);
          
            if ((Object)obj.n1 != null)
                n1 = new vec3(obj.n1.x, obj.n1.y, obj.n1.z);
            if ((Object)obj.v1 != null)
                v1 = new vec3(obj.v1.x, obj.v1.y, obj.v1.z);
            if ((Object)obj.v2 != null)
                v2 = new vec3(obj.v2.x, obj.v2.y, obj.v2.z);
          
            Grips = new List<TGrip>();
            if (obj.Grips != null)
                foreach (TGrip g in obj.Grips)
                    Grips.Add(new TGrip(this, g.x, g.y, g.translacao, g.rotacao, g.escala, g.estica, g.layer));
           
            Tipo = obj.Tipo;

            if ((Object)obj.Titulo != null)
              this.Titulo = (TTexto)obj.Titulo.Clone();

            if ((Object)obj.Titulo2 != null)
                this.Titulo2 = (TTexto)obj.Titulo2.Clone();

            this.layer = obj.layer;
            Visivel = obj.Visivel;

            Selecionado = obj.Selecionado;
            Layer = obj.Layer;
            layer = obj.layer;

            if ((Object)obj.Dados != null)
                this.Dados = (TDadosLaje)obj.Dados.Clone();

            if ((Object)obj.l1 != null)
                this.l1 = (TLinha)obj.l1.Clone();

            if ((Object)obj.linhas_poligonal != null)
                foreach (TLinha lin in obj.linhas_poligonal)
                {
                    linhas_poligonal.Add((TLinha)lin.Clone());
                    linhas_poligonal[linhas_poligonal.Count-1].UpdateAnguloComprimento();
                }
            if ((Object)obj.linhasAdd != null)
                foreach (TLinha lin in obj.linhasAdd)
                {
                    linhasAdd.Add((TLinha)lin.Clone()); 
                    linhasAdd[linhasAdd.Count-1].UpdateAnguloComprimento();
                }
            
            if ((Object)obj.pts_poligono != null)
                foreach (TPonto lin in obj.pts_poligono)
                    pts_poligono.Add((TPonto)lin.Clone());
            
            Inicializando = obj.Inicializando;

            if ((Object)obj.pontoInicial != null)
                this.pontoInicial = (TPonto)obj.pontoInicial.Clone();

            lastX = obj.lastX;
            lastY = obj.lastY;

            coord = new CoordenadaD[5];
            for (int i = 0; i < 5; i++)
            {
                coord[i].X = obj.coord[i].X;
                coord[i].Y = obj.coord[i].Y;
            }

            if ((Object)obj.Linhas != null)
                foreach (TLinha lin in obj.Linhas)
                    Linhas.Add((TLinha)lin.Clone());

            if ((Object)obj.Pilares != null)
                foreach (TPilar p in obj.Pilares)
                    Pilares.Add((TPilar)p.Clone());

            if ((Object)obj.poligono != null)
                poligono = (TPoligono)obj.poligono.Clone();
            if ((Object)obj.PoligonoSelecao != null)
                PoligonoSelecao = (TPoligono)obj.PoligonoSelecao.Clone();
            if ((Object)obj.poligono_eixo != null)
                poligono_eixo = (TPoligono)obj.poligono_eixo.Clone();

            if (this.Grips != null)
              if (this.Grips.Count > 0)
                 this.AtualizaPoligonoSelecao(); 
        }
    }
}
