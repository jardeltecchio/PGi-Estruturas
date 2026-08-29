using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using OpenTK;
using OpenTK.Input;
using OpenTK.Graphics.OpenGL;
using OpenTK.Platform;

namespace PG
{
    [Serializable]
    public class TArcoIMF : TObjetoDesenho
    {
        public TLinha[] trechos;
        public double raio, cx, cy, angini, angfin;
        private float rr, gg, bb;
        private int i;
        public TPonto pMedio;
        public  int CodigoObjetosTela, discretizacao;
        public bool selecaoPorProximidade, selecaoPorPontoMedio, contornoLajeLinhaEixoViga, AberturaLaje, ArcoTipoViga;

        public TArcoIMF(double raio, double cx, double cy, double angini, double angfin, TLinha[] trechos, int CodigoObjetosTela, int discretizacao)
        {
            this.raio   = raio;
            this.cx     = cx;
            this.cy     = cy;
            this.angini = angini;
            this.angfin = angfin;

            this.trechos = new TLinha[29];
            this.trechos = trechos;
            this.CodigoObjetosTela = CodigoObjetosTela;
            this.discretizacao = discretizacao;
            base.Tipo = Const.ID_ARCOIMF;
        }
        public override string PrimeiroComando()
        {
            return Const.CMD_ARCO_IMF_1_P;
        }
        public TArcoIMF() 
        {
            pIni   = new TPonto(1);
            pFin   = new TPonto(1);
            pMedio = new TPonto(1);
            base.Visivel = true;
            base.IdLayer = Lay.ElementosBasicos;
            this.Tipo    = Const.ID_ARCOIMF;
        }
        public override void Initialize(ref TPonto point, ref string command, ISettings Dados, TLayer layer, ref List<TLinha> Linhas, ref int pavimento)
        {
            discretizacao = 20;
            Definindo3Pt = false;
            Finalizado = false;
            base.pIni = new TPonto(point.x, point.y, 0);

            this.layer = layer;
            pts = new CoordenadaD[20];

            command = Const.CMD_ARCO_IMF_2_P;
            base.Initialize(ref point, ref command, Dados, layer, ref Linhas, ref pavimento);
        }
        bool Definindo3Pt, Finalizado;
        public override void OnMouseMove(ref TPonto point, bool ShowInfo, bool orto = true, double angulo = 0, bool PontoInicial = false)
        {
            if (!Definindo3Pt)
            {
                pMedio = new TPonto(point.x, point.y, 0);
            }
            else
            {
               // angini = 0;
               // angfin = -90;
                //raio = 50;
               // cx = 250;
               // cy = 250;
                base.pFin.x = point.x;
                base.pFin.y = point.y;

            }
         //   alfa = FuncoesGerais.atand(Math.Abs(pIni.y - pFin.y) / Math.Abs(pIni.x - pFin.x));

            base.angulo = ((float)(FuncoesGerais.atand((pIni.y - pFin.y) / (pIni.x - pFin.x))));
            this.angulo = base.angulo;
            base.anguloGlobal = (float)RMath.rad2deg(pIni.getAngleTo(pFin));


            //     DifCoordX = Math.Abs(pIni.x - pFin.x);
            // /       DifCoordY = Math.Abs(pIni.y - pFin.y); 

            // if (ShowInfo)
            //     ShowDynamicInfo();

            point.Snap = false;

            base.OnMouseMove(ref point, ShowInfo);
        }

        public override eObjetoDesenhoMouseDown OnMouseDown(ref TPonto point, ref string command, List<TLinha> Linhas, List<TPonto> Pontos, List<TPilar> Pilares, ref List<TObjetoDesenho> Objects, bool FromGrip = false)
        {
            if (!Definindo3Pt)
            {
                pMedio = new TPonto(point.x, point.y,0);
                Definindo3Pt = true;
                return eObjetoDesenhoMouseDown.Continue;
            }
            else
            {
               // AddGrips();
                base.pFin = new TPonto(point.x, point.y, 0);
                Definindo3Pt = false;
                Finalizado = true;
                return eObjetoDesenhoMouseDown.Done;
            }
        }

        public TArcoIMF(string tipo, int registro, bool selecionado) : base(tipo, registro, selecionado) { }
        public override TObjetoDesenho Clone()
        {
            TArcoIMF l = new TArcoIMF();
            l.Copy(this);
            return l;
        }

        public void Copy(TArcoIMF obj)
        {
            pIni.x = obj.pIni.x;
            pIni.y = obj.pIni.y;

            pFin.x = obj.pFin.x;
            pFin.y = obj.pFin.y;

            pMedio.x = obj.pMedio.x;
            pMedio.y = obj.pMedio.y;
             raio = obj.raio;
             cx = obj.cx;
             cy = obj.cy;
            angini = obj.angini;
            angfin = obj.angfin;
            discretizacao = obj.discretizacao;
        }

        [NonSerialized]
        public System.Drawing.Pen mPen = new System.Drawing.Pen(System.Drawing.Color.White);
        CoordenadaD[] pts;
        public override void Desenha(ref bool Unifilar, ref int transp, ref bool arestas)
        {
            if (base.Selecionado)
            {
               // Gl.glLineStipple(3, 0xAAAA);
               /// Gl.glEnable(Gl.GL_LINE_STIPPLE);
            };

            if (selecaoPorProximidade)
            {
              //  Gl.glLineStipple(5, 0xAAAA);
             //   Gl.glEnable(Gl.GL_LINE_STIPPLE);
            };

            if (!Finalizado)
            {
                GL.Begin(PrimitiveType.Lines);
                GL.Vertex3(pIni.x, pIni.y, pIni.z);
                GL.Vertex3(pMedio.x, pMedio.y, pMedio.z);
                GL.End();

                // Cad.DrawLine(mPen, FPrincipal.pixelX(pIni.x), FPrincipal.pixelY(pIni.y), FPrincipal.pixelX(pMedio.x), FPrincipal.pixelY(pMedio.y));

                theta = angfin / (double)(discretizacao - 1);//theta is now calculated from the arc angle instead, the - 1 bit comes from the fact that the arc is open

                tangetial_factor = System.Convert.ToSingle(Math.Tan(theta * Const.PIDiv180));

                radial_factor = System.Convert.ToSingle(Math.Cos(theta * Const.PIDiv180));

                x = raio * System.Convert.ToSingle(Math.Cos(angini * Const.PIDiv180));//we now start at the start angle
                y = raio * System.Convert.ToSingle(Math.Sin(angini * Const.PIDiv180));
                for (int ii = 0; ii < discretizacao; ii++)
                {
                    pixx = FPrincipal.pixelX(System.Convert.ToSingle(x + cx));
                    pixy = FPrincipal.pixelY(System.Convert.ToSingle(y + cy));

                    pts[ii].X = pixx;
                    pts[ii].Y = pixy;

                    tx = -y;
                    ty = x;

                    x += tx * tangetial_factor;
                    y += ty * tangetial_factor;

                    x *= radial_factor;
                    y *= radial_factor;
                }

                 for (int ii = 0; ii < discretizacao - 1; ii++)
                {
                    GL.Begin(PrimitiveType.Lines);
                    GL.Vertex3(pts[ii].X, pts[ii].Y, pts[ii].Z);
                    GL.Vertex3(pts[ii+1].X, pts[ii+1].Y, pts[ii+1].Z);
                    GL.End();
                }

             //     Cad.DrawLine(mPen, FPrincipal.pixelX(pts[ii].X), FPrincipal.pixelY(pts[ii].Y), FPrincipal.pixelX(pts[ii + 1].X), FPrincipal.pixelY(pts[ii + 1].Y));

                if (Definindo3Pt)
                {
                    pm1 = (pIni + pMedio) / 2;
                    theta = pIni.getAngleTo(pMedio) / Const.PIDiv180;
                    hip = 1000;
                    senO = Math.Sin(theta * Const.PIDiv180);
                    cosO = Math.Cos(theta * Const.PIDiv180);
                    nx = cosO * hip;
                    ny = senO * hip;
                    p1.x = nx;
                    p1.y = ny;
                    p1 = pm1 + p1;
                    centro.x = pm1.x;
                    centro.y = pm1.y;

                    /**/
                    rot1.x = p1.x;
                    rot1.y = p1.y;
                    rot1 = rot1.Rotate(centro, 90 * Const.PIDiv180);

                    /**/
                    rot2.x = p1.x;
                    rot2.y = p1.y;
                    rot2 = rot2.Rotate(centro, -90 * Const.PIDiv180);

                    GL.Begin(PrimitiveType.Lines);
                    GL.Vertex3(rot2.x, rot2.y, rot2.z);
                    GL.Vertex3(rot2.x, rot2.y, rot2.z);
                    GL.End();

                    GL.Begin(PrimitiveType.Lines);
                    GL.Vertex3(pMedio.x, pMedio.y, pMedio.z);
                    GL.Vertex3(pFin.x, pFin.y, pFin.z);
                    GL.End();


                    //  Cad.DrawLine(mPen, Desenho.pixelX(rot2.x), Desenho.pixelY(rot2.y), Desenho.pixelX(rot1.x), Desenho.pixelY(rot1.y));
                    ///   Cad.DrawLine(mPen, FPrincipal.pixelX(pMedio.x), FPrincipal.pixelY(pMedio.y), FPrincipal.pixelX(pFin.x), FPrincipal.pixelY(pFin.y));

                    /**/
                    pm1 = (pMedio + pFin) / 2;
                    theta = pFin.getAngleTo(pMedio) / Const.PIDiv180;
                    senO = Math.Sin(theta * Const.PIDiv180);
                    cosO = Math.Cos(theta * Const.PIDiv180);
                    nx = cosO * hip;
                    ny = senO * hip;
                    p1.x = nx;
                    p1.y = ny;
                    p1 = pm1 + p1;
                    centro.x = pm1.x;
                    centro.y = pm1.y;

                    /**/
                    rot3.x = p1.x;
                    rot3.y = p1.y;
                    rot3 = rot3.Rotate(centro, 90 * Const.PIDiv180);

                    /**/
                    rot4.x = p1.x;
                    rot4.y = p1.y;
                    rot4 = rot4.Rotate(centro, -90 * Const.PIDiv180);

                    if (Geom.calcIntersecEQU_RETA(rot1.x, rot1.y, rot2.x, rot2.y, rot3.x, rot3.y, rot4.x, rot4.y, ref inx, ref iny))
                    {
                        GL.Begin(PrimitiveType.Lines);
                        GL.Vertex3(pm1.x, pm1.y, pm1.z);
                        GL.Vertex3(inx, iny, pm1.z);
                        GL.End();

                        //  Cad.DrawLine(mPen, FPrincipal.pixelX(pm1.x), FPrincipal.pixelY(pm1.y), FPrincipal.pixelX(inx), FPrincipal.pixelY(iny));
                        pm1 = (pIni + pMedio) / 2;


                        GL.Begin(PrimitiveType.Lines);
                        GL.Vertex3(pm1.x, pm1.y, pm1.z);
                        GL.Vertex3(inx, iny, pm1.z);
                        GL.End();
                        //   Cad.DrawLine(mPen, FPrincipal.pixelX(pm1.x), FPrincipal.pixelY(pm1.y), FPrincipal.pixelX(inx), FPrincipal.pixelY(iny));
                        cx = inx;
                        cy = iny;
                        pCentro.x = inx;
                        pCentro.y = iny;
                        raio = pCentro.DistanceTo(pFin);

                        angini = pIni.getAngleTo(pCentro) / Const.PIDiv180;
                        angfin = pFin.getAngleTo(pCentro) / Const.PIDiv180;
                    }
                }
            }
            else
            {
                for (int ii = 0; ii < discretizacao - 1; ii++)
                {

                }
            }
             // Cad.DrawLine(mPen, FPrincipal.pixelX(pts[ii].X), FPrincipal.pixelY(pts[ii].Y), FPrincipal.pixelX(pts[ii + 1].X), FPrincipal.pixelY(pts[ii + 1].Y));
            
            this.selecaoPorProximidade = false; 
        }
        double nx, ny, hip, senO, cosO, inx,iny;
        double theta, tangetial_factor, tx, ty,radial_factor, x, y, start_angle;
        TPonto pm1 = new TPonto(0), pm2 = new TPonto(0), p1 = new TPonto(0), p2 = new TPonto(0);
        TPonto pCentro = new TPonto(0);
        vec3 rot1 = new vec3(0);
        vec3 rot2 = new vec3(0);
        vec3 rot3 = new vec3(0);
        vec3 rot4 = new vec3(0);
        vec3 centro = new vec3(0);

        float pixx, pixy;
        public override bool Selecionar(float clicx, float clicy, float coordx, float coordy, TObjetoDesenho Owner = null)
        {
            bool esta_no_intervalo = false;
            double tol = 3.5;

            for (i = 0; i < 29; i++)
            {
                if (clicx - Math.Max(trechos[i].pIni.px_x, trechos[i].pFin.px_x) > tol ||
                                        Math.Min(trechos[i].pIni.px_x, trechos[i].pFin.px_x) - clicx > tol ||

                clicy - Math.Max(trechos[i].pIni.px_y, trechos[i].pFin.px_y) > tol ||
                                    Math.Min(trechos[i].pIni.px_y, trechos[i].pFin.px_y) - clicy > tol)

                esta_no_intervalo = false;

                if (Math.Abs(trechos[i].pFin.px_x - trechos[i].pIni.px_x) < tol)
                    esta_no_intervalo = Math.Abs(trechos[i].pIni.px_x - clicx) < tol || Math.Abs(trechos[i].pFin.px_x - clicx) < tol;

                if (Math.Abs(trechos[i].pFin.px_y - trechos[i].pIni.px_y) < tol)
                    esta_no_intervalo = Math.Abs(trechos[i].pIni.px_y - clicy) < tol || Math.Abs(trechos[i].pFin.px_y - clicy) < tol;

                double x = trechos[i].pIni.px_x + (clicy - trechos[i].pIni.px_y) * (trechos[i].pFin.px_x - trechos[i].pIni.px_x) / (trechos[i].pFin.px_y - trechos[i].pIni.px_y);
                double y = trechos[i].pIni.px_y + (clicx - trechos[i].pIni.px_x) * (trechos[i].pFin.px_y - trechos[i].pIni.px_y) / (trechos[i].pFin.px_x - trechos[i].pIni.px_x);

                float x1 = Math.Min(trechos[i].pIni.px_x, trechos[i].pFin.px_x);
                float y1 = Math.Min(trechos[i].pIni.px_y, trechos[i].pFin.px_y);
                float x2 = Math.Max(trechos[i].pIni.px_x, trechos[i].pFin.px_x);
                float y2 = Math.Max(trechos[i].pIni.px_y, trechos[i].pFin.px_y);

                esta_no_intervalo = Math.Abs(clicx - x) < tol || Math.Abs(clicy - y) < tol;
                if (esta_no_intervalo)
                {
                    if (y1 == y2)
                        esta_no_intervalo = ((clicx >= x1) && (clicx <= x2));
                    else
                        if (x1 == x2)
                            esta_no_intervalo = ((clicy >= y1) && (clicy <= y2));
                        else
                            esta_no_intervalo = ((clicx >= x1) && (clicx <= x2)) && ((clicy >= y1) && (clicy <= y2));
                };

                if (esta_no_intervalo)
                {
                    SetaSelecao(true,true);
                    
                    return base.Selecionado;  //seleciona o Arco em si...
                }
            };

            return false;
        }

        public override void SetaSelecao(bool s, bool MostrarGrip, bool SelecaoDeCandidato = false, TObjetoDesenho Owner = null)
        {
            base.Selecionado = s;
            base.MostrarGrip = MostrarGrip;
            
           // for (int hh = 0; hh < 29; hh++)
          //     trechos[hh].Selecionado = s;  //seleciona os trechos do tipo TLinha

            if (MostrarGrip)
                AddGrips();
        }
    }
}
