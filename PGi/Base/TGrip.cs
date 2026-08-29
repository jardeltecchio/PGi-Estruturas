using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
namespace PG
{
    [Serializable]
    public class TGrip : TObjetoDesenho
    {
        public double x, y;
        public bool esta_pintado, gripFinalViga;
        public TLayer layer;
        public bool translacao, rotacao, escala, estica;
        public TObjetoDesenho ObjetoDesenho;
        public Coordenada[] Coords;
        public int codigo;
        [NonSerialized]
        public System.Drawing.Pen mPen = new System.Drawing.Pen(System.Drawing.Color.White);
        [NonSerialized]
        public System.Drawing.SolidBrush mBrush = new System.Drawing.SolidBrush(System.Drawing.Color.Blue);

        public System.Drawing.Point[] points = new System.Drawing.Point[4];
        TLinha Eixo;
        double xant, yant, beta;
        public double uxi, uyi, uxf, uyf;
        public bool movendo = false;
        
        
        public TGrip(TObjetoDesenho ObjetoDesenho, double x, double y, bool translacao = false, bool rotacao = false, bool escala = false, bool estica = false, TLayer layer = null)
        {
            this.ObjetoDesenho = ObjetoDesenho;
            this.ObjetoDesenho.Tipo = ObjetoDesenho.Tipo;
            this.x          = x;
            this.y          = y;
            base.Visivel    = false;
            base.Tipo = Const.ID_GRIP;
            this.Tipo = Const.ID_GRIP;
            this.rotacao    = rotacao;
            this.translacao = translacao;
            this.escala     = escala;
            this.estica = estica;
            this.layer = layer;
            this.Layer = layer;
        }

       
        [NonSerialized]
        System.Drawing.Graphics cad;
        public override void Desenha(ref System.Drawing.Graphics Cad)
        {
            try { 
            this.cad = Cad;
            if (base.Visivel && mBrush != null)
            {
              //  Gl.glBegin(Gl.GL_POLYGON);
               // Gl.glColor3f(0, 0, 1);

                if (Selecionado || esta_pintado)
                  mBrush.Color = System.Drawing.Color.Red;
                else
                  mBrush.Color = System.Drawing.Color.Blue;
                
                points[0].X = (int)(x / FPrincipal.precisaoPixel + FPrincipal.ponto_zero[0]) - 5;
                points[0].Y = (int)(y / -FPrincipal.precisaoPixel + FPrincipal.ponto_zero[1]) - 5;

                points[1].X = (int)(x / FPrincipal.precisaoPixel + FPrincipal.ponto_zero[0]) + 5;
                points[1].Y = (int)(y / -FPrincipal.precisaoPixel + FPrincipal.ponto_zero[1]) - 5;

                points[2].X = (int)(x / FPrincipal.precisaoPixel + FPrincipal.ponto_zero[0]) + 5;
                points[2].Y = (int)(y / -FPrincipal.precisaoPixel + FPrincipal.ponto_zero[1]) + 5;

                points[3].X = (int)(x / FPrincipal.precisaoPixel + FPrincipal.ponto_zero[0]) - 5;
                points[3].Y = (int)(y / -FPrincipal.precisaoPixel + FPrincipal.ponto_zero[1]) + 5;

                Cad.FillPolygon(mBrush, points);
           
                //Gl.glVertex2f(System.Convert.ToSingle(x / Desenho.precisaoPixel + Desenho.ponto_zero[0]) - 5, System.Convert.ToSingle(y / -Desenho.precisaoPixel + Desenho.ponto_zero[1]) - 5);
             //   Gl.glVertex2f(System.Convert.ToSingle(x / Desenho.precisaoPixel + Desenho.ponto_zero[0]) - 5, System.Convert.ToSingle(y / -Desenho.precisaoPixel + Desenho.ponto_zero[1]) + 5);
             
              //  Gl.glVertex2f(System.Convert.ToSingle(x / Desenho.precisaoPixel + Desenho.ponto_zero[0]) + 5, System.Convert.ToSingle(y / -Desenho.precisaoPixel + Desenho.ponto_zero[1]) + 5);
             //   Gl.glVertex2f(System.Convert.ToSingle(x / Desenho.precisaoPixel + Desenho.ponto_zero[0]) + 5, System.Convert.ToSingle(y / -Desenho.precisaoPixel + Desenho.ponto_zero[1]) - 5);
               // Gl.glEnd();

                mPen.Color = System.Drawing.Color.Gold;
                 
                Cad.DrawLine(mPen, points[0].X, points[0].Y,
                                   points[1].X, points[1].Y);

                Cad.DrawLine(mPen, points[1].X, points[1].Y,
                              points[2].X, points[2].Y);


                Cad.DrawLine(mPen, points[2].X, points[2].Y,
                            points[3].X, points[3].Y);


                Cad.DrawLine(mPen, points[3].X, points[3].Y,
                              points[0].X, points[0].Y);

            }

            }
            catch (Exception ms)
            {
                System.Windows.Forms.MessageBox.Show("erro ao desenhar grip: " + ms.Message);
            }
        }
        [NonSerialized]
        bool achouPontoPilar;
        [NonSerialized]
        TPonto pAux, pAux2, pAux3, pAux4;
        int verticePontoInicial;
        TPonto refPonto;
        public override void OnMouseMove(ref TPonto point, bool ShowInfo, bool orto = true, double angulo = 0, bool PontoInicial = false)
        {
            double xi, yi;
            #region pilar
            if (ObjetoDesenho.Tipo.ToString().Contains(Const.ID_PILAR))
            {
                if (escala)
                {
                    double xGrip = point.x;
                    double yGrip = point.y;
                    this.x = xGrip;
                    this.y = yGrip;

                    TPilar pilar = ObjetoDesenho as TPilar;

                    if (!achouPontoPilar)
                    {
                        foreach (TPonto p in pilar.Vertices)
                        {
                            if (Geom.Iguais(p.x, xGrip) && Geom.Iguais(p.y, yGrip))
                            {
                                pAux = p;
                                break;
                            }
                        }

                        for (int i =0; i< pilar.Vertices.Count; i++)
                        {
                            if (Geom.Iguais(pilar.Vertices[i].x, pilar.pIni.x) && Geom.Iguais(pilar.Vertices[i].y, pilar.pIni.y))
                            {
                                verticePontoInicial = i;
                                break;
                            }
                        }
                        
                        // if (Geom.Iguais(pilar.pIni.x, xGrip) && Geom.Iguais(pilar.pIni.y, yGrip))
               //         pAux4 = pilar.pIni;

                        foreach (TLinha lin in pilar.Dados.Poligono.linhas_poligonal)
                        {
                            if (Geom.Iguais(xGrip, lin.pFin.x) && Geom.Iguais(yGrip, lin.pFin.y))
                              pAux2 = lin.pFin;
                            else
                            if (Geom.Iguais(xGrip, lin.pIni.x) && Geom.Iguais(yGrip, lin.pIni.y))
                              pAux3 = lin.pIni;
                        }

                        achouPontoPilar = true;
                    }

                    if (achouPontoPilar)
                    {
                        pAux.x = xGrip;
                        pAux.y = yGrip;

                        pAux2.x = xGrip;
                        pAux2.y = yGrip;

                        pAux3.x = xGrip;
                        pAux3.y = yGrip;

                 //       if ((Object)pAux4 != null)
                        {

                        }
                    }
                }
            }
            #endregion
            #region carga linear
            if (ObjetoDesenho.Tipo.ToString().Contains(Const.ID_CARGA_LINEAR))
            {
                if (estica)
                {
                    if (x == (ObjetoDesenho as TCargaLinear).pIni.x && y == (ObjetoDesenho as TCargaLinear).pIni.y)
                    {
            //            (ObjetoDesenho as TCargaLinear).pIni.y = point.y;
            //            (ObjetoDesenho as TCargaLinear).pIni.x = point.x;
             //           (ObjetoDesenho as TCargaLinear).UpdatePixel();
                        (ObjetoDesenho as TCargaLinear).OnMouseMove(ref point, false, false, 0, true);
                    }
                    else
                    if (x == (ObjetoDesenho as TCargaLinear).pFin.x && y == (ObjetoDesenho as TCargaLinear).pFin.y)
                    {
                    //    (ObjetoDesenho as TCargaLinear).pFin.y = point.y;
                    //    (ObjetoDesenho as TCargaLinear).pFin.x = point.x;
                    //    (ObjetoDesenho as TCargaLinear).UpdatePixel();
                        (ObjetoDesenho as TCargaLinear).OnMouseMove(ref point, false, false, 0);
                    }
                    
            
                 //   (ObjetoDesenho as TCargaLinear).comprimento = (float)(ObjetoDesenho as TCargaLinear).pFin.DistanceTo((ObjetoDesenho as TCargaLinear).pIni);
                //    (ObjetoDesenho as TCargaLinear).angulo = ((float)(FuncoesGerais.atand(((ObjetoDesenho as TCargaLinear).pIni.y - (ObjetoDesenho as TCargaLinear).pFin.y) / ((ObjetoDesenho as TCargaLinear).pIni.x - (ObjetoDesenho as TCargaLinear).pFin.x))));
                //    (ObjetoDesenho as TCargaLinear).anguloGlobal = (float)RMath.rad2deg((ObjetoDesenho as TCargaLinear).pIni.getAngleTo((ObjetoDesenho as TCargaLinear).pFin));

                    (ObjetoDesenho as TCargaLinear).Grips[0].x = (ObjetoDesenho as TCargaLinear).pIni.x;
                    (ObjetoDesenho as TCargaLinear).Grips[0].y = (ObjetoDesenho as TCargaLinear).pIni.y;
                    (ObjetoDesenho as TCargaLinear).Grips[1].x = (ObjetoDesenho as TCargaLinear).pFin.x;
                    (ObjetoDesenho as TCargaLinear).Grips[1].y = (ObjetoDesenho as TCargaLinear).pFin.y;
                }
            }
            #endregion
            else
            #region linha
            if (ObjetoDesenho.Tipo.ToString().Contains(Const.ID_LINHA))
            {
                if (translacao)
                {
                    (ObjetoDesenho as TLinha).pIni.y += (point.y - uyi);
                    (ObjetoDesenho as TLinha).pIni.x += (point.x - uxi);

                    (ObjetoDesenho as TLinha).pFin.y += (point.y - uyi);
                    (ObjetoDesenho as TLinha).pFin.x += (point.x - uxi);

                    (ObjetoDesenho as TLinha).UpdatePixel();

                    uxi = point.x;
                    uyi = point.y;

                    (ObjetoDesenho as TLinha).Grips[0].x = (ObjetoDesenho as TLinha).pIni.x;
                    (ObjetoDesenho as TLinha).Grips[0].y = (ObjetoDesenho as TLinha).pIni.y;
                    
                 //   (ObjetoDesenho as TLinha).Grips[1].x = (ObjetoDesenho as TLinha).getMiddlePoint().x;
                 //   (ObjetoDesenho as TLinha).Grips[1].y = (ObjetoDesenho as TLinha).getMiddlePoint().y;

                    (ObjetoDesenho as TLinha).Grips[1].x = (ObjetoDesenho as TLinha).pFin.x;
                    (ObjetoDesenho as TLinha).Grips[1].y = (ObjetoDesenho as TLinha).pFin.y;

                  //  AplicarCoordGrips_TEXTO();
                }
                else
                if (estica)
                {
                    if (x == (ObjetoDesenho as TLinha).pIni.x && y == (ObjetoDesenho as TLinha).pIni.y)
                    {
                        (ObjetoDesenho as TLinha).pIni.y = point.y;
                        (ObjetoDesenho as TLinha).pIni.x = point.x;
                        (ObjetoDesenho as TLinha).UpdatePixel();

                      ///  AplicarCoordGrips_TEXTO();
                    }
                    else
                    if (x == (ObjetoDesenho as TLinha).pFin.x && y == (ObjetoDesenho as TLinha).pFin.y)
                    {
                        (ObjetoDesenho as TLinha).pFin.y = point.y;
                        (ObjetoDesenho as TLinha).pFin.x = point.x;
                        (ObjetoDesenho as TLinha).UpdatePixel();

                        ///  AplicarCoordGrips_TEXTO();
                    }

                    (ObjetoDesenho as TLinha).comprimento = (float)(ObjetoDesenho as TLinha).pFin.DistanceTo((ObjetoDesenho as TLinha).pIni);
                    (ObjetoDesenho as TLinha).angulo = ((float)(FuncoesGerais.atand(((ObjetoDesenho as TLinha).pIni.y - (ObjetoDesenho as TLinha).pFin.y) / ((ObjetoDesenho as TLinha).pIni.x - (ObjetoDesenho as TLinha).pFin.x))));
                    (ObjetoDesenho as TLinha).anguloGlobal = (float)RMath.rad2deg((ObjetoDesenho as TLinha).pIni.getAngleTo((ObjetoDesenho as TLinha).pFin));
                    
                    AplicarCoordGrips_LINHA();
                }
                else
                if (escala)
                {
                    // this.x = point.x;
                    // this.y = point.y;
                    xi = (ObjetoDesenho as TTexto).Grips[0].x;
                    yi = (ObjetoDesenho as TTexto).Grips[0].y;
                    ShowDynamicInfo(ref cad, xi, yi, point.x, point.y);

                    if (point.x > xant || point.y > yant)
                    {
                        (ObjetoDesenho as TTexto).tamx *= 1.01;
                        (ObjetoDesenho as TTexto).tamy *= 1.01;
                    }
                    else
                    {
                        (ObjetoDesenho as TTexto).tamx /= 1.01;
                        (ObjetoDesenho as TTexto).tamy /= 1.01;
                    }

                    xant = point.x;
                    yant = point.y;
                }
            }
            #endregion
            else
            #region laje
                if (ObjetoDesenho.Tipo.ToString().Contains(Const.ID_LAJE))
            {
                if (translacao)
                {
                    (ObjetoDesenho as TLaje).Grips[0].x = point.x;
                    (ObjetoDesenho as TLaje).Grips[0].y = point.y;

                    (ObjetoDesenho as TLaje).AtualizaPoligonoSelecao();
                }
            }
            #endregion
            else
            if (ObjetoDesenho.Tipo.ToString().Contains(Const.ID_CARGA_PONTUAL))
            {
                if (translacao)
                {
                    (ObjetoDesenho as TCargaPontual).pIni.x = point.x;
                    (ObjetoDesenho as TCargaPontual).pIni.y = point.y;
                    (ObjetoDesenho as TCargaPontual).ponto.x = point.x;
                    (ObjetoDesenho as TCargaPontual).ponto.y = point.y;
                    (ObjetoDesenho as TCargaPontual).Grips[0].x = point.x;
                    (ObjetoDesenho as TCargaPontual).Grips[0].y = point.y;
                }
            }
            else
            #region texto
            if (ObjetoDesenho.Tipo.ToString().Contains(Const.ID_TEXTO))
            {
                if (translacao)
                    (ObjetoDesenho as TTexto).OnMove(point.x, point.y);
                else
                if (rotacao)
                {
                    xi = (ObjetoDesenho as TTexto).Grips[0].x;
                    yi = (ObjetoDesenho as TTexto).Grips[0].y;

                    //  ShowDynamicInfo(ref cad, xi, yi, point.x, point.y);

                    double teta = Geom.GetAnguloGlobal((ObjetoDesenho as TTexto).Grips[0].x, (ObjetoDesenho as TTexto).Grips[0].y,
                                                        point.x, point.y);

                    (ObjetoDesenho as TTexto).angulo = (float)teta;

                    (ObjetoDesenho as TTexto).OnMove((ObjetoDesenho as TTexto).Grips[0].x, (ObjetoDesenho as TTexto).Grips[0].y);

                }
                else
                if (escala)
                {
                    xi = (ObjetoDesenho as TTexto).Grips[0].x;
                    yi = (ObjetoDesenho as TTexto).Grips[0].y;
                    //   ShowDynamicInfo(ref cad, xi, yi, point.x, point.y);

                    if (point.x > xant || point.y > yant)
                    {
                        (ObjetoDesenho as TTexto).tamx *= 1.01;
                        (ObjetoDesenho as TTexto).tamy *= 1.01;
                    }
                    else
                    {
                        (ObjetoDesenho as TTexto).tamx /= 1.01;
                        (ObjetoDesenho as TTexto).tamy /= 1.01;
                    }

                    xant = point.x;
                    yant = point.y;

                    (ObjetoDesenho as TTexto).OnMove((ObjetoDesenho as TTexto).Grips[0].x, (ObjetoDesenho as TTexto).Grips[0].y);
                }
            }
            #endregion
            else
            #region viga
            if (ObjetoDesenho.Tipo.ToString().Contains(Const.ID_TRECHOVIGA))
            {
                Eixo = (ObjetoCopia as TTrechoViga).linhas_eixo;

                (ObjetoDesenho as TTrechoViga).linha_eixo_aux.Visivel = true;

                if (estica)
                {
                    if (!gripFinalViga)
                    {
                       // Eixo.pIni.y = point.y;
                    //    Eixo.pIni.x = point.x;
                        (ObjetoCopia as TTrechoViga).OnMouseMove(ref point,true, true,0,true);

                      //  (ObjetoDesenho as TTrechoViga).OnMouseMove(ref (ObjetoDesenho as TTrechoViga).pFin, true,true);
                        
                        Eixo.UpdatePixel();

                        (ObjetoDesenho as TTrechoViga).Grips[0].x = point.x;
                        (ObjetoDesenho as TTrechoViga).Grips[0].y = point.y;
                        (ObjetoDesenho as TTrechoViga).Grips[0].Visivel = false;
                    }
                    else
                   // if (x == Eixo.pFin.x && y == Eixo.pFin.y)
                    {
                     //   Eixo.pFin.y = point.y;
                     //   Eixo.pFin.x = point.x;
                        (ObjetoCopia as TTrechoViga).OnMouseMove(ref point, true, true);
                        Eixo.UpdatePixel();

                        (ObjetoDesenho as TTrechoViga).Grips[1].x = point.x;
                        (ObjetoDesenho as TTrechoViga).Grips[1].y = point.y;
                        (ObjetoDesenho as TTrechoViga).Grips[1].Visivel = false;
                    }
                }
                else
                if (translacao)
                {
                    Eixo.pIni.y += (point.y - uyi);
                    Eixo.pIni.x += (point.x - uxi);

                    Eixo.pFin.y += (point.y - uyi);
                    Eixo.pFin.x += (point.x - uxi);

                    (ObjetoDesenho as TTrechoViga).linhas_eixo.UpdatePixel();

                    uxi = point.x;
                    uyi = point.y;
                    //  ShowDynamicInfo(ref cad, Eixo.pIni.x, Eixo.pIni.y,Eixo.pFin.x, Eixo.pFin.y, true);

                    (ObjetoDesenho as TTrechoViga).Grips[0].x = Eixo.pIni.x;
                    (ObjetoDesenho as TTrechoViga).Grips[0].y = Eixo.pIni.y;

                    (ObjetoDesenho as TTrechoViga).Grips[1].x = Eixo.pFin.x;
                    (ObjetoDesenho as TTrechoViga).Grips[1].y = Eixo.pFin.y;
                }

                //    (ObjetoDesenho as TTrechoViga).Grips[1].x = Eixo.getMiddlePoint().x;
                //   (ObjetoDesenho as TTrechoViga).Grips[1].y = Eixo.getMiddlePoint().y;
                
              //  (ObjetoDesenho as TTrechoViga).comprimento  = (float)Eixo.pFin.DistanceTo(Eixo.pIni);
              //  (ObjetoDesenho as TTrechoViga).angulo       = (float)(FuncoesGerais.atand((Eixo.pIni.y - Eixo.pFin.y) / (Eixo.pIni.x - Eixo.pFin.x)));
               // (ObjetoDesenho as TTrechoViga).anguloGlobal = (float)RMath.rad2deg(Eixo.pIni.getAngleTo(Eixo.pFin));
              //  Eixo.angulo                                 = (float)(ObjetoDesenho as TTrechoViga).angulo;
            }
            #endregion
        }

        private void AplicarCoordGrips_LINHA()
        {
            (ObjetoDesenho as TLinha).Grips[0].x = (ObjetoDesenho as TLinha).pIni.x;
            (ObjetoDesenho as TLinha).Grips[0].y = (ObjetoDesenho as TLinha).pIni.y;

            (ObjetoDesenho as TLinha).Grips[1].x = (ObjetoDesenho as TLinha).getMiddlePoint().x;
            (ObjetoDesenho as TLinha).Grips[1].y = (ObjetoDesenho as TLinha).getMiddlePoint().y;

            (ObjetoDesenho as TLinha).Grips[2].x = (ObjetoDesenho as TLinha).pFin.x;
            (ObjetoDesenho as TLinha).Grips[2].y = (ObjetoDesenho as TLinha).pFin.y;
        }

        public void ShowDynamicInfo(ref System.Drawing.Graphics Cad, double xini, double yini, double xfin, double yfin, bool showline = true)
        {
            double alfa, DifCoordX, DifCoordY, Comp, angFin, angIni, dx, dy, inicioAnguloX, inicioAnguloY;
            float PontoY;
            int i;
            alfa = FuncoesGerais.atand(Math.Abs(yini - yfin) / Math.Abs(xini - xfin));
            #region Ortogonal e magnetismo por ângulos

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

                    //   Posicao.Y = pixelY(yfin);
                };
            };

            #endregion

            Comp = (float)(Math.Sqrt(Math.Pow(xfin - xini, 2) + Math.Pow(yfin - yini, 2)));

            if (yfin > yini)
                PontoY = FPrincipal.pixelY(yini);
            else
                PontoY = FPrincipal.pixelY(yfin);

            #region Valores nas cotas

            dy = 0;
            dx = 0;

            angFin = alfa;
            angIni = 0;

            inicioAnguloX = 0;
            inicioAnguloY = 0;

            if (xfin >= xini)
            {
                dx = (float)(Comp / 2.4);
                dy = (float)(Math.Tan((float)((alfa / 3.3f) * Const.PIDiv180))) * dx;  //co = tan°.ca

                if (yfin > yini)
                {
                    angIni = alfa / 3.3f;
                    inicioAnguloY = FPrincipal.pixelY(yini + dy);
                }
                else
                {
                    angIni = -alfa / 3.3f;
                    inicioAnguloY = FPrincipal.pixelY(yini - dy);
                };

                inicioAnguloX = FPrincipal.pixelX(xini + dx) + 3;
            }
            else
            {
                dx = (float)(Comp / 2.4);
                dy = (float)(Math.Tan((float)((alfa / 3.3f) * Const.PIDiv180))) * dx;  //co = tan°.ca

                inicioAnguloX = FPrincipal.pixelX(xini - dx) - 44;

                if (yfin > yini)
                {
                    angIni = -alfa / 3.3f;
                    inicioAnguloY = FPrincipal.pixelY(yini + dy);
                }
                else
                {
                    angIni = alfa / 3.3f;
                    inicioAnguloY = FPrincipal.pixelY(yini - dy);
                };
            };

            #endregion}


            #region Arco e hipotenusa

            angFin = alfa;
            angIni = 0;

            if (yfin < yini) angFin *= -1;

            if (xfin < xini)
            {
                angIni = 180;
                angFin = alfa;

                if (yfin > yini)
                    angFin = -alfa;
            };


          /*  g2d.DrawArc(xini,
                                   yini,
                                   Comp / 2.4f,
                                   angIni,
                                   angFin,
                                   15, 0, 0, 0,
                                   0.2f, 0.2f, 0.5f);*/

            if (yfin < yini) alfa *= -1;
            if (xfin < xini) alfa *= -1;

            //hipotenusa
            g2d.StrokeText(Comp.ToString("f2").Replace(",", "."),
                        xfin < xini ? FPrincipal.pixelX(xini - (DifCoordX / 1.4f)) :
                                                           FPrincipal.pixelX(xini + (DifCoordX / 1.4f)),

                        yfin > yini ? FPrincipal.pixelY(yini + (DifCoordY / 1.4f)) - 10 :
                                                           FPrincipal.pixelY(yini - (DifCoordY / 1.4f)) - 10,
                        0.08f, -0.08f,
                        106, 90, 205,
                        (float)alfa);

            #endregion

            if (showline) //nao desenhar a linha se estiver desenhando a viga, pois nao preciso dela
            {
                //   mPen.Color = System.Drawing.Color.FromArgb(this.layer.Rgb[0], this.layer.Rgb[1], this.layer.Rgb[2]);
                //  if (PaintArgs != null)
                cad.DrawLine(mPen, FPrincipal.pixelX(xini), FPrincipal.pixelY(yini), FPrincipal.pixelX(xfin), FPrincipal.pixelY(yfin));

                
                /* Gl.glBegin(Gl.GL_LINES);

                 Gl.glColor3f(1, 1, 1);
                 if (Desenho.r == 1)
                     Gl.glColor3f(0, 0, 0);

                 Gl.glVertex2f(Desenho.pixelX(xini), Desenho.pixelY(yini));
                 Gl.glVertex2f(Desenho.pixelX(xfin), Desenho.pixelY(yfin));
                 Gl.glEnd();*/
            };

        }

        public override eObjetoDesenhoMouseDown OnMouseDown(ref TPonto point, ref string command, List<TLinha> Linhas, List<TPonto> Pontos, List<TPilar> Pilares, ref List<TObjetoDesenho> Objects, bool FromGrip = false)
        {
            achouPontoPilar = false;
            if (ObjetoDesenho.Tipo.ToString().Contains(Const.ID_PILAR))
            {
               (ObjetoDesenho as TPilar).CriaListaVertices();
               (ObjetoDesenho as TPilar).AddLinhasPoligonalAux();
               (ObjetoDesenho as TPilar).TriangularizaFace();
               (ObjetoDesenho as TPilar).Dados.Poligono.AtualizaLinhasPoligonal();
               (ObjetoDesenho as TPilar).Texto2.texto = (ObjetoDesenho as TPilar).Dados.B1 + "/" + (ObjetoDesenho as TPilar).Dados.H1 + ((ObjetoDesenho as TPilar).Dados.B2 > 0 ? "/" + (ObjetoDesenho as TPilar).Dados.B2 + "/" + (ObjetoDesenho as TPilar).Dados.H2 : "");

               (ObjetoDesenho as TPilar).pIni.x = (ObjetoDesenho as TPilar).Vertices[verticePontoInicial].x;
               (ObjetoDesenho as TPilar).pIni.y = (ObjetoDesenho as TPilar).Vertices[verticePontoInicial].y;
        //       (ObjetoDesenho as TPilar).AtualizaArestasAuxiliares((ObjetoDesenho as TPilar).pIni);
            }
            if (ObjetoDesenho.Tipo.ToString().Contains(Const.ID_CARGA_LINEAR))
            {              
              //  (ObjetoDesenho as TCargaLinear).pFin = point.Clone() as TPonto;

              //  (ObjetoDesenho as TCargaLinear).AddGrips();

                (ObjetoDesenho as TCargaLinear).Grips[0].x = (ObjetoDesenho as TCargaLinear).pIni.x;
                (ObjetoDesenho as TCargaLinear).Grips[0].y = (ObjetoDesenho as TCargaLinear).pIni.y;
                (ObjetoDesenho as TCargaLinear).Grips[1].x = (ObjetoDesenho as TCargaLinear).pFin.x;
                (ObjetoDesenho as TCargaLinear).Grips[1].y = (ObjetoDesenho as TCargaLinear).pFin.y;
            }

            if (ObjetoDesenho.Tipo.ToString().Contains(Const.ID_CARGA_PONTUAL))
            {

            }
            
            if (ObjetoDesenho.Tipo.ToString().Contains(Const.ID_TRECHOVIGA))
            {
                TTrechoViga New = new TTrechoViga((TPonto)(ObjetoCopia as TTrechoViga).pIni.Clone(),
                                                  (TPonto)(ObjetoCopia as TTrechoViga).pFin.Clone(),
                                                  ((TDadosViga)(ObjetoCopia as TTrechoViga).Dados.Clone()),
                                                  ObjetoCopia.layer, (ObjetoCopia as TTrechoViga).Pavimento, Linhas, true);
                Objects.Add(New);
               /* TTrechoViga Trecho = (ObjetoDesenho as TTrechoViga);
                
                TLinha Eixo_Aux = Trecho.linha_eixo_aux;

                beta = Eixo.angulo;
                if (Eixo.pFin.x >= Eixo.pIni.x)
                  beta -= 90;
                else
                  beta += 90;

                double xx10 = (float)(Trecho.Dados.b1 * Math.Cos(beta * Const.PIDiv180));
                double yy10 = (float)(Trecho.Dados.b1 * Math.Sin(beta * Const.PIDiv180));

                if ((beta == 0 && Eixo_Aux.pFin.y < Eixo_Aux.pIni.y) || (beta == -180 && Eixo_Aux.pFin.y > Eixo_Aux.pIni.y))
                  xx10 = -xx10;

                Trecho.comprimento  = (float)Eixo_Aux.pFin.DistanceTo(Eixo_Aux.pIni);
                Trecho.angulo       = (float)(FuncoesGerais.atand((Eixo_Aux.pIni.y - Eixo_Aux.pFin.y) / (Eixo_Aux.pIni.x - Eixo_Aux.pFin.x)));
                Trecho.anguloGlobal = (float)RMath.rad2deg(Eixo_Aux.pIni.getAngleTo(Eixo_Aux.pFin));
               
                Trecho.linhas_eixo.pIni.x = Eixo_Aux.pIni.x;
                Trecho.linhas_eixo.pIni.y = Eixo_Aux.pIni.y;
                Trecho.linhas_eixo.pFin.x = Eixo_Aux.pFin.x;
                Trecho.linhas_eixo.pFin.y = Eixo_Aux.pFin.y;

                Trecho.linhas_eixo_org.pIni.x = Eixo_Aux.pIni.x;
                Trecho.linhas_eixo_org.pIni.y = Eixo_Aux.pIni.y;
                Trecho.linhas_eixo_org.pFin.x = Eixo_Aux.pFin.x;
                Trecho.linhas_eixo_org.pFin.y = Eixo_Aux.pFin.y;

                Trecho.SetaCoords(Trecho, 1, Trecho.linhas_eixo.pIni, Trecho.linhas_eixo.pFin, ref xx10, ref yy10);
                
                if (Trecho.Dados.face_insercao == 1)
                {
                    Trecho.pIni.x = Trecho.linhas_eixo.pIni.x;
                    Trecho.pIni.y = Trecho.linhas_eixo.pIni.y;
                    Trecho.pFin.x = Trecho.linhas_eixo.pFin.x;
                    Trecho.pFin.y = Trecho.linhas_eixo.pFin.y;
                }
                else
                if (Trecho.Dados.face_insercao == 0)
                {
                    Trecho.pIni.x = Trecho.linhas_facecima.pIni.x;
                    Trecho.pIni.y = Trecho.linhas_facecima.pIni.y;
                    Trecho.pFin.x = Trecho.linhas_facecima.pFin.x;
                    Trecho.pFin.y = Trecho.linhas_facecima.pFin.y;
                }
                else
                if (Trecho.Dados.face_insercao == 2)
                {
                    Trecho.pIni.x = Trecho.linhas_facebaixo.pIni.x;
                    Trecho.pIni.y = Trecho.linhas_facebaixo.pIni.y;
                    Trecho.pFin.x = Trecho.linhas_facebaixo.pFin.x;
                    Trecho.pFin.y = Trecho.linhas_facebaixo.pFin.y;
                }

                Trecho.linhas_facebaixo_org.pIni.x = Trecho.linhas_facebaixo.pIni.x;
                Trecho.linhas_facebaixo_org.pIni.y = Trecho.linhas_facebaixo.pIni.y;
                Trecho.linhas_facebaixo_org.pFin.x = Trecho.linhas_facebaixo.pFin.x;
                Trecho.linhas_facebaixo_org.pFin.y = Trecho.linhas_facebaixo.pFin.y;
              
                Trecho.linhas_facecima_org.pIni.x = Trecho.linhas_facecima.pIni.x;
                Trecho.linhas_facecima_org.pIni.y = Trecho.linhas_facecima.pIni.y;
                Trecho.linhas_facecima_org.pFin.x = Trecho.linhas_facecima.pFin.x;
                Trecho.linhas_facecima_org.pFin.y = Trecho.linhas_facecima.pFin.y;

                Trecho.ReposicionaTextos();

                Trecho.UpdateTodas(Trecho);

                Trecho.UpdatePixelTodas(Trecho);

                Trecho.Grips[1].x = Trecho.linhas_eixo.pFin.x;
                Trecho.Grips[1].y = Trecho.linhas_eixo.pFin.y;
                Trecho.Grips[0].x = Trecho.linhas_eixo.pIni.x;
                Trecho.Grips[0].y = Trecho.linhas_eixo.pIni.y;
                Trecho.UpdateTrechosDasLinhas();
                Trecho.UpdateAnguloTodas2();

                Trecho.linha_eixo_aux.Visivel = false;*/
             //   Trecho.pFin.x = point.x;
             //  Trecho.pFin.y = point.y;
                //----------------------------------------------------------------------------------------------
                if (!translacao)
                {
              //      point.x = Trecho.pFin.x;
             //       point.y = Trecho.pFin.y;

               //     Trecho.OnMouseDown(ref point, ref command, Linhas, Pontos, null,ref Objects, FromGrip);
                  //  if (Objects.Count > 1)
                 //   {
                 //       Trecho.SelecionaTodosElementos(Trecho);
                      /*  Trecho.Texto1.Grips[0].Selecionado = true;
                        Trecho.Texto1.Grips[1].Selecionado = true;
                      //  Trecho.Texto1.Grips[2].Selecionado = true;

                        Trecho.Grips[0].Selecionado = true;
                        Trecho.Grips[1].Selecionado = true;
                       // Trecho.Grips[2].Selecionado = true;

                        Trecho.Texto1.Selecionado = true;
                        Trecho.Texto2.Selecionado = true;

                        Trecho.Texto2.Grips[0].Selecionado = true;
                        Trecho.Texto2.Grips[1].Selecionado = true;*/
                      //  Trecho.Texto2.Grips[2].Selecionado = true;
                 //   }
                }

            }

            return eObjetoDesenhoMouseDown.Continue;
        }

        public override bool Selecionar(float clicx, float clicy, float coordx, float coordy, TObjetoDesenho Owner = null)
        {
            if (this.layer.Congelado || this.layer.Travado)
                return false;
            bool selecionou = false;

            if ((clicx >= (FPrincipal.pixelX(this.x) - 15)) && (clicx <= (FPrincipal.pixelX(this.x) + 15)))
                if ((clicy >= (FPrincipal.pixelY(this.y) - 15)) && (clicy <= (FPrincipal.pixelY(this.y)) + 15))
                    {
                        selecionou = true;
                    }

            //if (esta_pintado) //quando pintou de vermelho ao passar o mouse lá na tela de desenho
             //  SetaSelecao(true, true);


            if (selecionou) //quando pintou de vermelho ao passar o mouse lá na tela de desenho
            {
                ObjetoCopia = ObjetoDesenho.Clone();
                ObjetoCopia.ObjetoCopia = true;
             //   ObjetoCopia.Selecionado = false;
               // ObjetoDesenho.Visivel = false;
                SetaSelecao(true, true);
            }
            return base.Selecionado;
        }
        public TObjetoDesenho ObjetoCopia;
        public override void SetaSelecao(bool s, bool visivel = false,  bool SelecaoDeCandidato = false, TObjetoDesenho Owner = null)
        {
            if (ObjetoDesenho.Tipo == Const.ID_TEXTO && (ObjetoDesenho as TTexto).TipoObjeto == Const.ID_LAJE)
                return; 

            //if (ObjetoDesenho.Tipo == Const.ID_PILAR)
            //    return;

            if (this.ObjetoDesenho.layer.Congelado || this.ObjetoDesenho.layer.Travado)
                return;
            //  if (this.layer.Congelado || this.layer.Travado)
       //         return;

            if (ObjetoDesenho.Tipo == Const.ID_BARRAGRELHA && (ObjetoDesenho as TBarraGrelha).barraViga)
              return;

            base.Selecionado = s;
            base.Visivel = visivel;

            ObjetoDesenho.SetaSelecao(s, visivel, SelecaoDeCandidato);
            //ObjetoDesenho.Selecionado = s;
        }
        public void Copy(TGrip obj)
        {           
            layer = obj.layer;
            Layer = obj.Layer; 
            Tipo = obj.Tipo;
            Visivel = obj.Visivel;
            this.x = obj.x;
            this.y = obj.y;
            this.esta_pintado = obj.esta_pintado;
            this.layer = obj.layer;;
            this.translacao = obj.translacao;
            this.rotacao = obj.rotacao;
            this.escala = obj.escala;
            this.estica = obj.estica;
            this.ObjetoDesenho = obj.ObjetoDesenho;
            this.Coords = obj.Coords;
            this.codigo = obj.codigo;
            mPen = new System.Drawing.Pen(System.Drawing.Color.White);
            mBrush = new System.Drawing.SolidBrush(System.Drawing.Color.Blue);
            points = new System.Drawing.Point[4];
        }

        public override TObjetoDesenho Clone()
        {
            TGrip l = new TGrip();
            l.Copy(this);
            return l;
        }

        public TGrip() : base() { }


    }
}
