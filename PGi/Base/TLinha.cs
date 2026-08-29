using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using System.Drawing;
using System.Drawing.Drawing2D;

namespace PG
{
    [Serializable]
    
    public class TLinha: TObjetoDesenho
    {
       
        public TPonto //pIni, 
                      //pFin, 
                      pMedio;
        
        public bool LinhaAuxCopia = false;
        public vec3 pIniAux = new vec3(0, 0, 0);
        public vec3 pFinAux = new vec3(0, 0, 0);
        
        public int IndiceArco,ID,
                   CodigoObjetosTela,
                   numViga, 
                   numLaje;

        [NonSerialized]
        public TTrechoViga TrechoViga;
        public int IDTrecho = -1;

        [NonSerialized]
        public TObjetoDesenho Barra;
        public int IDBarra = -1;

        public  float comprimento;

        public  bool  selecaoPorProximidade,
                      selecaoPorPontoMedio, 
                      contornoLajeLinhaEixoViga, 
                      invisivel,
                      permiteSnap,
                      horizontal,
                      vertical,auxiliar, barraRigida,

                      LinhaExtremoViga, LinhaTipoViga, LinhaTipoArco, LinhaEixoViga, LinhaContornoPilar;

        private bool  proxima;
        private int i;

        public TLinha (bool permiteSnap = true)
        {
            pIni = new TPonto(2);
            pFin = new TPonto(2);
            base.Visivel = true;
            base.IdLayer = Lay.ElementosBasicos;
            this.Tipo    = "linha";
            this.permiteSnap = permiteSnap;

            //LinhaEixoViga = true;
         //   AddGrips();
        }
        public TLinha(TPonto PontoIni, TPonto PontoFin, bool aux = true)
        {
            this.pIni = PontoIni;
            this.pFin = PontoFin;
            this.comprimento = (float)(Math.Sqrt(Math.Pow(PontoIni.x - PontoFin.x, 2) + Math.Pow(PontoIni.y - PontoFin.y, 2)));
            this.comprimento = (float)pFin.DistanceTo(pIni);
            base.angulo = ((float)(FuncoesGerais.atand((pIni.y - pFin.y) / (pIni.x - pFin.x))));
            base.Visivel = true;
            this.selecaoPorProximidade = false;
            this.invisivel = false;
            this.permiteSnap = true;
            this.auxiliar = aux;
            this.Tipo = "linha";
            base.IdLayer = "0";
        }
        public TLinha(TPonto PontoIni, TPonto PontoFin, int CodigoObjetosTela, TPonto pMedio = null, 
            bool LinhaTipoArco = false  , bool barra90 = false, 
            bool LinhaTipoViga = false  , bool LinhaEixoViga = false, 
            bool LinhaExtremoViga= false, bool Invisivel = false,
            bool permiteSnap =  true,
            TTrechoViga Trecho = null,
            TLayer layer       = null,
            bool criagrip = true,
            bool aux = false)
        {
            this.pIni           = PontoIni;
            this.pFin           = PontoFin;
            this.comprimento    = (float)(Math.Sqrt(Math.Pow(PontoIni.x - PontoFin.x, 2) + Math.Pow(PontoIni.y - PontoFin.y, 2)));
           // this.anguloReal     = FuncoesGerais.atand((PontoIni.y - PontoFin.y) / (PontoIni.x - PontoFin.x)); ;
            //this.anguloAbsoluto = (float)Geom.GetAnguloGlobal(PontoIni.x, PontoIni.y, PontoFin.x, PontoFin.y);
            this.comprimento = (float)pFin.DistanceTo(pIni);
            base.angulo = ((float)(FuncoesGerais.atand((pIni.y - pFin.y) / (pIni.x - pFin.x))));
            this.pMedio            = pMedio;
            this.CodigoObjetosTela = CodigoObjetosTela;
            this.LinhaTipoArco     = LinhaTipoArco;
            base.Visivel = true;
            this.LinhaTipoViga         = LinhaTipoViga;
            this.LinhaEixoViga         = LinhaEixoViga;
            this.selecaoPorProximidade = false;
            this.LinhaExtremoViga      = LinhaExtremoViga;
            this.invisivel             = Invisivel;
            this.permiteSnap           = permiteSnap;
            this.TrechoViga            = Trecho;
            this.auxiliar = aux;
            this.Tipo    = "linha";
            base.IdLayer = "0";
            if (criagrip)
              AddGrips();
         }
      
        public void UpdateAnguloComprimento()
        {
            this.comprimento = (float)pFin.DistanceTo(pIni);
            base.angulo = ((float)(FuncoesGerais.atand((pIni.y - pFin.y) / (pIni.x - pFin.x))));
            this.angulo = base.angulo;
            base.anguloGlobal = (float)RMath.rad2deg(pIni.getAngleTo(pFin));
        }
        vec3 p1_Rotacao = new vec3(0, 0, 0);
        vec3 p2_Rotacao = new vec3(0, 0, 0);
        vec3 p2     = new vec3(0, 0, 0);
        vec3 Centro = new vec3(0, 0, 0);
        double offsetY, offsetX, xAnt, yAnt;
        public override void Mover(ref TPonto ponto1,ref  TPonto ponto2, bool dinamico)
        {
            pIni.Mover(ref ponto1, ref ponto2, dinamico);
            pFin.Mover(ref ponto1, ref ponto2, dinamico);
        }

        double angAnterior = 0;
        double anguloAnterior = 0;
        public override void Rotacionar(TPonto ponto1, TPonto ponto2, double ang)
        {
            pIni = pIni.Rotate(ponto1, (ang - anguloAnterior) * Const.PIDiv180);
            pFin = pFin.Rotate(ponto1, (ang - anguloAnterior) * Const.PIDiv180);
           /* Centro.x   = ponto1.x;
            Centro.y   = ponto1.y;
            p2.x       = ponto2.x;
            p2.y       = ponto2.y;

            AnguloRotacao = ang; //Geom.GetAnguloGlobal(ponto1.x, ponto1.y, ponto2.x, ponto2.y);
            
            p1_Rotacao.x = pIni.x;
            p1_Rotacao.y = pIni.y;
            p1_Rotacao   = p1_Rotacao.Rotate(Centro, (AnguloRotacao - angAnterior) * Const.PIDiv180);

            pIni.x = p1_Rotacao.x;
            pIni.y = p1_Rotacao.y;

            p2_Rotacao.x = pFin.x;
            p2_Rotacao.y = pFin.y;
            p2_Rotacao = p2_Rotacao.Rotate(Centro, (AnguloRotacao - angAnterior) * Const.PIDiv180);

            pFin.x = p2_Rotacao.x;
            pFin.y = p2_Rotacao.y;
            */
            anguloAnterior = ang;
        }//

        public TLinha(string tipo, int registro, bool selecionado) : base(tipo, registro, selecionado) { }

        public bool PontoEmLinha(double x, double y)
        {
            return (Geom.PontoEmLinha2(x, y, this.pIni.x, this.pIni.y, this.pFin.x, this.pFin.y));
        }

        public bool Intersec(TLinha outra, ref double x, ref double y)
        {
            return (Geom.calcIntersecEQU_RETA(this.pIni.x, this.pIni.y, this.pFin.x, this.pFin.y,
                                                        outra.pIni.x, outra.pIni.y, outra.pFin.x, outra.pFin.y, ref x, ref y));

        }

        public void Intersec2(TLinha outra, ref double x, ref double y)
        {
            if (Geom.calcIntersecEQU_RETA(this.pIni.x, this.pIni.y, this.pFin.x, this.pFin.y,
                                                        outra.pIni.x, outra.pIni.y, outra.pFin.x, outra.pFin.y, ref x, ref y))
            {

            }

        }
        public override void Atualiza(int pavimento)
        {
            AddGrips();
        }

        public bool Intersec(double x1, double y1, double x2, double y2, ref double x, ref double y)
        {
            return (Geom.calcIntersecEQU_RETA(this.pIni.x, this.pIni.y, this.pFin.x, this.pFin.y,
                                                        x1, y1, x2, y2, ref x, ref y));

        }
        public bool Intersec(Linha outra, ref double x, ref double y)
        {
            return (Geom.calcIntersecEQU_RETA(this.pIni.x, this.pIni.y, this.pFin.x, this.pFin.y,
                                                        outra.pIni.x, outra.pIni.y, outra.pFin.x, outra.pFin.y, ref x, ref y));

        }
        

        public TPonto getMiddlePoint()
        {
            return (pIni + pFin) /2;
        }
        public override string PrimeiroComando()
        {
            return Const.CMD_LINHA_1_P;
        }

  
        public float[] dashValues = { 3, 8, 5 };
        public override void Desenha(ref System.Drawing.Graphics Cad)
        {
          /*  proxima = false;

           // Gl.glDisable(Gl.GL_LINE_STIPPLE);
            if (auxiliar)
                proxima = false;
           if (barraRigida) return;

            if ((!LinhaTipoArco && !LinhaTipoViga && !LinhaEixoViga) || (LinhaEixoViga && barraRigida))
            {
                if (Math.Min(Desenho.pixelX(pIni.x), Desenho.pixelX(pFin.x)) < 0 && Math.Max(Desenho.pixelX(pIni.x), Desenho.pixelX(pFin.x)) < 0) return;
                else
                if (Math.Min(Desenho.pixelX(pIni.x), Desenho.pixelX(pFin.x)) > Desenho.w && Math.Max(Desenho.pixelX(pIni.x), Desenho.pixelX(pFin.x)) > Desenho.h) return;
                else
                if (Math.Min(Desenho.pixelY(pIni.y), Desenho.pixelY(pFin.y)) < 0 && Math.Max(Desenho.pixelY(pIni.y), Desenho.pixelY(pFin.y)) < 0) return;
                else
                if (Math.Min(Desenho.pixelY(pIni.y), Desenho.pixelY(pFin.y)) > Desenho.h && Math.Max(Desenho.pixelY(pIni.y), Desenho.pixelY(pFin.y)) > Desenho.h) return;
                
                if (!base.Visivel) return;

               // Gl.glColor3f(this.layer.Rgb[0], this.layer.Rgb[1], this.layer.Rgb[2]);

                if (base.Selecionado)
                {
                    if (base.MostrarGrip && (Object)Grips != null)
                      for (i = 0; i < Grips.Count(); i++)
                        Grips[i].Desenha(ref Cad);

                //    Gl.glLineStipple(3, 0xAAAA);
                //    Gl.glEnable(Gl.GL_LINE_STIPPLE);
                };

                mPen.Color = Color.FromArgb(this.layer.Rgb[0], this.layer.Rgb[1], this.layer.Rgb[2]);
                
                if (selecaoPorProximidade)
                {
                    mPen.DashStyle = DashStyle.Dash;
                    mPen.DashPattern = dashValues;  
                    mPen.Color = Color.LimeGreen;
                 //   Gl.glLineStipple(5, 0xAAAA);
                 //   Gl.glEnable(Gl.GL_LINE_STIPPLE);
                }
                else
                  mPen.DashStyle = DashStyle.Solid;
       

                if (selecaoPorPontoMedio)
                {
                    mPen.Color = Color.LimeGreen;
                    //   Gl.glLineStipple(5, 0xAAAA);
                 //   Gl.glEnable(Gl.GL_LINE_STIPPLE);
                };

                try
                {
               //  if (PaintArgs != null)
                  Cad.DrawLine(mPen, Desenho.pixelX(pIni.x), Desenho.pixelY(pIni.y), Desenho.pixelX(pFin.x), Desenho.pixelY(pFin.y));
                }
                catch(Exception ms)
                {
                    System.Windows.Forms.MessageBox.Show(ms.Message);
                }
                //  g2d.line(Desenho.pixelX(pIni.x), Desenho.pixelY(pIni.y), Desenho.pixelX(pFin.x), Desenho.pixelY(pFin.y));
                
             //  g2d.line((float)pIni.x, (float)(pIni.y), (float)(pFin.x), (float)(pFin.y));
               
               selecaoPorPontoMedio = false;
               selecaoPorProximidade = false;

            //   Gl.glDisable(Gl.GL_LINE_STIPPLE);
            };*/
        }

        public override TPonto LastPoint()
        {
            return pFin;
        }

        public override TPonto FirstPoint()
        {
            return pIni;
        }

        public void UpdatePixel()
        {
            pIni.px_x = FPrincipal.pixelX(pIni.x);
            pIni.px_y = FPrincipal.pixelY(pIni.y);

            pFin.px_x = FPrincipal.pixelX(pFin.x);
            pFin.px_y = FPrincipal.pixelY(pFin.y);
        }

        public override bool Selecionar(float clicx, float clicy, float coordx, float coordy, TObjetoDesenho Owner = null)
        {
            if (this.layer.Congelado || this.layer.Travado)
              return false;
            
            if (Geom.PontoEmLinha2(clicx, clicy, FPrincipal.pixelX(pIni.x), FPrincipal.pixelY(pIni.y), FPrincipal.pixelX(pFin.x), FPrincipal.pixelY(pFin.y)))
            {
                SetaSelecao(true, true);
                return base.Selecionado;
            }
            return false;

              bool esta_no_intervalo = false;
              double tol = 3.5;

              if (!LinhaTipoArco)
              {
                  if (clicx - Math.Max(FPrincipal.pixelX(pIni.x), FPrincipal.pixelX(pFin.x)) > tol ||
                                          Math.Min(FPrincipal.pixelX(pIni.x), FPrincipal.pixelX(pFin.x)) - clicx > tol ||

                  clicy - Math.Max(FPrincipal.pixelY(pIni.y), FPrincipal.pixelY(pFin.y)) > tol ||
                                      Math.Min(FPrincipal.pixelY(pIni.y), FPrincipal.pixelY(pFin.y)) - clicy > tol)

                      esta_no_intervalo = false;

                  if (Math.Abs(FPrincipal.pixelX(pFin.x) - FPrincipal.pixelX(pIni.x)) < tol)
                      esta_no_intervalo = Math.Abs(FPrincipal.pixelX(pIni.x) - clicx) < tol || Math.Abs(FPrincipal.pixelX(pFin.x) - clicx) < tol;

                  if (Math.Abs(FPrincipal.pixelY(pFin.y) - FPrincipal.pixelY(pIni.y)) < tol)
                      esta_no_intervalo = Math.Abs(FPrincipal.pixelY(pIni.y) - clicy) < tol || Math.Abs(FPrincipal.pixelY(pFin.y) - clicy) < tol;

                  double x, y;

                  if ((FPrincipal.pixelY(pFin.y) - FPrincipal.pixelY(pIni.y)) == 0)
                      x = 0;
                  else
                      x = FPrincipal.pixelX(pIni.x) + (clicy - FPrincipal.pixelY(pIni.y)) * (FPrincipal.pixelX(pFin.x) - FPrincipal.pixelX(pIni.x)) / (FPrincipal.pixelY(pFin.y) - FPrincipal.pixelY(pIni.y));

                  if ((FPrincipal.pixelX(pFin.x) - FPrincipal.pixelX(pIni.x)) == 0)
                      y = 0;
                  else
                      y = FPrincipal.pixelY(pIni.y) + (clicx - FPrincipal.pixelX(pIni.x)) * (FPrincipal.pixelY(pFin.y) - FPrincipal.pixelY(pIni.y)) / (FPrincipal.pixelX(pFin.x) - FPrincipal.pixelX(pIni.x));

                  float x1 = Math.Min(FPrincipal.pixelX(pIni.x), FPrincipal.pixelX(pFin.x));
                  float y1 = Math.Min(FPrincipal.pixelY(pIni.y), FPrincipal.pixelY(pFin.y));
                  float x2 = Math.Max(FPrincipal.pixelX(pIni.x), FPrincipal.pixelX(pFin.x));
                  float y2 = Math.Max(FPrincipal.pixelY(pIni.y), FPrincipal.pixelY(pFin.y));

                  esta_no_intervalo = Math.Abs(clicx - x) < tol || Math.Abs(clicy - y) < tol;
                  if (esta_no_intervalo)
                  {
                      float tolerancia = 0.1f;

                      if (Geom.Iguais(y1, y2, tolerancia))
                        esta_no_intervalo = ((clicx >= x1) && (clicx <= x2));
                      else
                      if (Geom.Iguais(x1, x2, tolerancia))
                        esta_no_intervalo = ((clicy >= y1) && (clicy <= y2));
                      else
                        esta_no_intervalo = ((clicx >= x1) && (clicx <= x2)) && ((clicy >= y1) && (clicy <= y2));
                  };

                  if (esta_no_intervalo)
                  {
                     // if ((Object)TrechoViga != null)
                     // {
                      //    System.Windows.Forms.MessageBox.Show(this.TrechoViga.Dados.numero.ToString());
                        //  this.TrechoViga.SetaSelecao(s, MostraGrip);
                   //   }

                      SetaSelecao(true, true);
                      return base.Selecionado;
                  }
              };

              return false;
        }

        public bool TestaSelecao(float clicx, float clicy)
        {
            bool esta_no_intervalo = false;
            double tol = 3.5;

                if (clicx - Math.Max(pIni.px_x, pFin.px_x) > tol ||
                                        Math.Min(pIni.px_x, pFin.px_x) - clicx > tol ||

                clicy - Math.Max(pIni.px_y, pFin.px_y) > tol ||
                                    Math.Min(pIni.px_y, pFin.px_y) - clicy > tol)

                    esta_no_intervalo = false;

                if (Math.Abs(pFin.px_x - pIni.px_x) < tol)
                    esta_no_intervalo = Math.Abs(pIni.px_x - clicx) < tol || Math.Abs(pFin.px_x - clicx) < tol;

                if (Math.Abs(pFin.px_y - pIni.px_y) < tol)
                    esta_no_intervalo = Math.Abs(pIni.px_y - clicy) < tol || Math.Abs(pFin.px_y - clicy) < tol;

                double x, y;

                if ((pFin.px_y - pIni.px_y) == 0)
                    x = 0;
                else
                    x = pIni.px_x + (clicy - pIni.px_y) * (pFin.px_x - pIni.px_x) / (pFin.px_y - pIni.px_y);

                if ((pFin.px_x - pIni.px_x) == 0)
                    y = 0;
                else
                    y = pIni.px_y + (clicx - pIni.px_x) * (pFin.px_y - pIni.px_y) / (pFin.px_x - pIni.px_x);

                float x1 = Math.Min(pIni.px_x, pFin.px_x);
                float y1 = Math.Min(pIni.px_y, pFin.px_y);
                float x2 = Math.Max(pIni.px_x, pFin.px_x);
                float y2 = Math.Max(pIni.px_y, pFin.px_y);

                esta_no_intervalo = Math.Abs(clicx - x) < tol || Math.Abs(clicy - y) < tol;
                if (esta_no_intervalo)
                {
                    float tolerancia = 0.01f;

                    if (Geom.Iguais(y1, y2, tolerancia))
                        esta_no_intervalo = ((clicx >= x1) && (clicx <= x2));
                    else
                        if (Geom.Iguais(x1, x2, tolerancia))
                            esta_no_intervalo = ((clicy >= y1) && (clicy <= y2));
                        else
                            esta_no_intervalo = ((clicx >= x1) && (clicx <= x2)) && ((clicy >= y1) && (clicy <= y2));
                };

                return esta_no_intervalo;
        }

        public override void SetaSelecao(bool s, bool MostraGrip, bool SelecaoDeCandidato = false, TObjetoDesenho Owner = null)
        {
          //  if (this.layer.Congelado || this.layer.Travado)
           //    return ;

            if ((Object)TrechoViga != null)
            //{
       //         System.Windows.Forms.MessageBox.Show(this.TrechoViga.Dados.numero.ToString());
                this.TrechoViga.SetaSelecao(s, MostraGrip,  SelecaoDeCandidato);
           // }


            base.Selecionado = s;
            base.MostrarGrip = MostraGrip;
            ShowHideGrips(MostraGrip);
      /*      if (MostraGrip)
              AddGrips();
            else
              RemoveGrips();*/
        }
        public override void ShowHideGrips(bool visivel)
        {
            if (Grips != null)
            foreach (TGrip grip in Grips)
                grip.Visivel = visivel;
        }
        public override void AddGrips()
        {
            Grips = new List<TGrip>(3);

            Grips.Add(new TGrip(this, this.pIni.x, this.pIni.y, false, false, false, true, this.layer));

            Grips.Add(new TGrip(this, this.getMiddlePoint().x, this.getMiddlePoint().y, true, false, false, false, this.layer));

            Grips.Add(new TGrip(this, this.pFin.x, this.pFin.y, false, false, false, true, this.layer));

        }

        public override void Initialize(ref TPonto point, ref string command, ISettings Dados, TLayer layer, ref List<TLinha> Linhas, ref int pavimento)
        {
            base.pIni = new TPonto(point.x, point.y, point.z);
            base.pFin = new TPonto(point.x, point.y, point.z);

            if (!base.pIni.Incidente(this))
              base.pIni.incidencias.Add(this);
            
            this.layer = layer;
    
            command = Const.CMD_LINHA_2_P;
            base.Initialize(ref point, ref command, Dados, layer, ref Linhas, ref pavimento);
        }

        public override void Cancel(bool cmd, ref string msg, ref List<TPonto> points)
        {
            if (cmd)
              msg = Const.CMD_LINHA_1_P;

            points.Add(pIni);
            points.Add(pFin);
        }

        public override void OnMouseMove(ref TPonto point, bool ShowInfo, bool orto = true, double angulo = 0, bool PontoInicial = false)
        {
            if (PontoInicial)
            {
                base.pIni = point;
                this.pIni = point;
            }
            else
            {
                base.pFin = point;
                this.pFin = point;
            }

            if (PontoInicial)
              alfa = FuncoesGerais.atand(Math.Abs(pFin.y - pIni.y) / Math.Abs(pFin.x - pIni.x));
            else
              alfa = FuncoesGerais.atand(Math.Abs(pIni.y - pFin.y) / Math.Abs(pIni.x - pFin.x));
      
            this.comprimento  = (float)pFin.DistanceTo(pIni);
            base.angulo       = ((float)(FuncoesGerais.atand((pIni.y - pFin.y) / (pIni.x - pFin.x))));
            this.angulo       = base.angulo;
            base.anguloGlobal = (float)RMath.rad2deg(pIni.getAngleTo(pFin));

           // if (orto)
            //  Orto(PontoInicial);

       //     DifCoordX = Math.Abs(pIni.x - pFin.x);
    // /       DifCoordY = Math.Abs(pIni.y - pFin.y); 

           // if (ShowInfo)
             // ShowDynamicInfo();

            point.Snap = false;

            base.OnMouseMove(ref point, ShowInfo);
        }

        public override eObjetoDesenhoMouseDown OnMouseDown(ref TPonto point, ref string command, List<TLinha> Linhas, List<TPonto> Pontos, List<TPilar> Pilares, ref List<TObjetoDesenho> Objects, bool FromGrip = false)
        {
            base.pFin = new TPonto(point.x, point.y, point.z);
        
            if (!base.pFin.Incidente(this))
              base.pFin.incidencias.Add(this);
            AddGrips();

            return eObjetoDesenhoMouseDown.DoneRepeat;
        }

        double alfa, DifCoordX, DifCoordY, Comp, angFin, angIni, dx, dy, inicioAnguloX, inicioAnguloY;
        float PontoY;

        public void Orto(bool PontoInicial)
        {
       //     if (pFin.Snap)
         //     MessageBox.Show("");
            //alfa = FuncoesGerais.atand(Math.Abs(pIni.y - pFin.y) / Math.Abs(pIni.x - pFin.x));

            if (!pFin.Snap)
            {
                if (ConfiguracoesCaptura.Captura.OrtogonalLigado)
                {
                    if (alfa > 88)
                    {
                        alfa = 90;
                        this.angulo = 90;
                        
                        if (!PontoInicial)
                          pFin.x = pIni.x;
                        else
                          pIni.x = pFin.x;
                      
                    }
                    else
                    if (alfa > 0 && alfa < 2)
                    {
                        alfa = 0;
                        this.angulo = 0;

                        if (!PontoInicial)
                            pFin.y = pIni.y;
                        else
                            pIni.y = pFin.y;
                    };
                };
            }

            DifCoordX = Math.Abs(pIni.x - pFin.x);
            DifCoordY = Math.Abs(pIni.y - pFin.y); 
            
            if (!pFin.Snap)
            {
                for (i = 0; i < ConfiguracoesCaptura.Captura.OutrosAngulos.Count; i++)
                {
                    if (alfa >= (ConfiguracoesCaptura.Captura.OutrosAngulos[i] - 2) && alfa <= (ConfiguracoesCaptura.Captura.OutrosAngulos[i] + 2))
                    {
                        alfa = ConfiguracoesCaptura.Captura.OutrosAngulos[i];
                        this.angulo = (float)alfa;

                        if (pFin.y > pIni.y)
                            pFin.y = (pIni.y + (float)(Math.Tan(alfa * Const.PIDiv180) * DifCoordX));
                        else
                            pFin.y = (pIni.y - (float)(Math.Tan(alfa * Const.PIDiv180) * DifCoordX));

                        //   Posicao.Y = pixelY(pFin.y);
                    };
                };
            }
        }

        double xi,yi;

        public override TObjetoDesenho Clone()
        {
            TLinha l = new TLinha();
            l.Copy(this);
            return l;
        }

        public void Copy(TLinha obj)
        {
            base.Copy(obj);
            
            pIni.x = obj.pIni.x;
            pIni.y = obj.pIni.y;
            pIni.z = obj.pIni.z;
            
            pFin.y = obj.pFin.y;
            pFin.x = obj.pFin.x;
            pFin.z = obj.pFin.z;

            pMedio     = obj.pMedio;
            IndiceArco = obj.IndiceArco;
            CodigoObjetosTela = obj.CodigoObjetosTela;
            numLaje     = obj.numLaje;
            numViga     = obj.numViga;
           
        //    if (obj.TrechoViga!= null)
        //      TrechoViga  = (TTrechoViga)obj.TrechoViga.Clone();

            if (obj.Barra != null)
                Barra = obj.Barra;

            layer = obj.layer;
            Tipo = obj.Tipo;
            Grips = new List<TGrip>();
            if (obj.Grips != null)
              foreach (TGrip g in obj.Grips)
                Grips.Add(new TGrip(this, g.x, g.y, g.translacao, g.rotacao, g.escala, g.estica, g.layer));
           
            comprimento = obj.comprimento;
            anguloGlobal = obj.anguloGlobal;
            angulo = obj.angulo;
            invisivel = obj.invisivel;
            permiteSnap = obj.permiteSnap;
            horizontal = obj.horizontal;
            vertical = obj. vertical;
            LinhaExtremoViga = obj.LinhaExtremoViga;
            LinhaTipoViga = obj.LinhaTipoViga;
            LinhaTipoArco = obj.LinhaTipoArco;
            LinhaContornoPilar = obj.LinhaContornoPilar;
            LinhaEixoViga = obj.LinhaEixoViga;
            Selecionado = obj.Selecionado;
            Layer = obj.Layer;
            contornoLajeLinhaEixoViga = obj.contornoLajeLinhaEixoViga;
            Visivel = obj.Visivel;


            pIniAux = new vec3(obj.pIniAux.x, obj.pIniAux.y, obj.pIniAux.z);
            pFinAux = new vec3(obj.pFinAux.x, obj.pFinAux.y, obj.pFinAux.z);
        
            IDTrecho    = obj.IDTrecho;
            IDBarra = obj.IDBarra;

            barraRigida = obj.barraRigida;
            auxiliar = obj.auxiliar;

        }

    }
}
