using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


namespace PG
{
    public class TCirculo: TObjetoDesenho
    {
        public double x, y, raio;
        public TPoligono Poligono;

        public TCirculo()
        {
            base.Visivel = true;
            base.IdLayer = Lay.ElementosBasicos;
            this.Tipo    = Const.ID_CIRCULO;
        }
        [NonSerialized]
        public System.Drawing.Pen mPen = new System.Drawing.Pen(System.Drawing.Color.White);

        public TCirculo(double posx, double posy, double raio, TLayer layer = null)
        {
            this.x     = posx;
            this.y     = posy;
            this.raio  = raio;
            base.Selecionado = false;

            base.Tipo = Const.ID_CIRCULO;
            this.layer = layer;

            AddGrips();

            mPen = new System.Drawing.Pen(System.Drawing.Color.White);
            AddPoligonoSelecao();

            base.Grips = this.Grips;
        }
        float twicePi = 2.0f * 3.1415f;
        int j;
        public float[] dashValues = { 8, 11, 5 };
        public override void Desenha(ref System.Drawing.Graphics Cad)
        {
            if (base.Selecionado)
            {
                if (base.MostrarGrip)
                  foreach (TGrip gr in base.Grips)
                    gr.Desenha();

                mPen.DashStyle   = System.Drawing.Drawing2D.DashStyle.DashDotDot;
                mPen.DashPattern = dashValues;
            }

            mPen.Color = System.Drawing.Color.FromArgb(this.layer.Rgb[0], this.layer.Rgb[1], this.layer.Rgb[2]);
          
        //    g2d.drawCircle(Desenho.pixelX(this.x), Desenho.pixelY(this.y), (float)this.raio / Desenho.precisaoPixel, 20, this.layer.Rgb[0], this.layer.Rgb[1], this.layer.Rgb[2]);
           
          /*  if (Poligono != null)
            foreach (TLinha l in Poligono.linhas_poligonal)
                Cad.DrawLine(mPen, Desenho.pixelX(l.pIni.x), Desenho.pixelY(l.pIni.y),
                                   Desenho.pixelX(l.pFin.x), Desenho.pixelY(l.pFin.y));
            else*/
            for (j = 0; j <= 20; j++)
                Cad.DrawLine(mPen, FPrincipal.pixelX((x + (this.raio * Math.Cos(j * twicePi / 20)))), FPrincipal.pixelY((y + (this.raio * Math.Sin(j * twicePi / 20)))),
                                   FPrincipal.pixelX((x + (this.raio * Math.Cos((j + 1) * twicePi / 20)))), FPrincipal.pixelY((y + (this.raio * Math.Sin((j + 1) * twicePi / 20)))));
            
            mPen.DashStyle = System.Drawing.Drawing2D.DashStyle.Solid;    
        }

        public override bool Selecionar(float clicx, float clicy, float coordx, float coordy, TObjetoDesenho Owner = null)
        {
            if (this.layer.Congelado || this.layer.Travado)
              return false;

            bool em = (Poligono.PontoEmPoligono(coordx, coordy));

            if (em)
            {
                SetaSelecao(true, true);
                base.Selecionado = true;
                return em;
            }

            return false;
        }

        public bool TestaSelecao(float clicx, float clicy)
        {
            bool esta_no_intervalo = false;
          /*  double tol = 3.5;

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
            };*/

            return esta_no_intervalo;
        }

        public override void SetaSelecao(bool s, bool MostraGrip, bool SelecaoDeCandidato = false, TObjetoDesenho Owner = null)
        {
            if (this.layer.Congelado || this.layer.Travado)
                return;
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
            Grips = new List<TGrip>(1);

            Grips.Add(new TGrip(this, this.x, this.y, true, false, false, false, this.layer));
        }

        public override void Initialize(ref TPonto point, ref string command, ISettings Dados, TLayer layer, ref List<TLinha> Linhas, ref int pavimento)
        {
            base.pIni = new TPonto(point.x,point.y,0);
            this.x = base.pIni.x;
            this.y = base.pIni.y;

            if (!base.pIni.Incidente(this))
              base.pIni.incidencias.Add(this);

            this.layer = layer;
            base.Tipo = Const.ID_CIRCULO;
            command = Const.CMD_CIRCULO_2_P;
            base.Initialize(ref point, ref command, Dados, layer, ref Linhas, ref pavimento);
        }

        public override void OnMouseMove(ref TPonto point, bool ShowInfo, bool orto = true, double angulo = 0, bool PontoInicial = false)
        {
            this.raio = base.pIni.DistanceTo(point);
            point.Snap = false;

            base.OnMouseMove(ref point, ShowInfo);
        }        
        
        public override string PrimeiroComando()
        {
            return Const.CMD_CIRCULO_1_P;
        }
        List<TLinha> linhas_poligonal = new List<TLinha>(20);
        CoordenadaD[] coord = new CoordenadaD[40];
        public void AddPoligonoSelecao()
        {
           
            for (j = 0; j <= 20; j++)
            {
                coord[j].X = (x + (this.raio * Math.Cos(j * twicePi / 20)));
                coord[j].Y = (y + (this.raio * Math.Sin(j * twicePi / 20)));
            }

              //  Cad.DrawLine(mPen, Desenho.pixelX(), Desenho.pixelY(),
              //                     Desenho.pixelX((x + (this.raio * Math.Cos((j + 1) * twicePi / 20)))), Desenho.pixelY((y + (this.raio * Math.Sin((j + 1) * twicePi / 20)))));



            Poligono = new TPoligono(coord);
            
            for (j = 0; j < 20; j++)
            {
                linhas_poligonal.Add(new TLinha(new TPonto(coord[j].X, coord[j].Y, 0), new TPonto(coord[j + 1].X, coord[j + 1].Y, 0), -1));
            }

         //   linhas_poligonal.Add(new TLinha(new TPonto(coord[1].X, coord[1].Y, 0), new TPonto(coord[2].X, coord[2].Y, 0), -1));
         //   linhas_poligonal.Add(new TLinha(new TPonto(coord[2].X, coord[2].Y, 0), new TPonto(coord[3].X, coord[3].Y, 0), -1));
         //   linhas_poligonal.Add(new TLinha(new TPonto(coord[3].X, coord[3].Y, 0), new TPonto(coord[4].X, coord[4].Y, 0), -1));
            Poligono.linhas_poligonal = linhas_poligonal;
        }

        public override eObjetoDesenhoMouseDown OnMouseDown(ref TPonto point, ref string command, List<TLinha> Linhas, List<TPonto> Pontos, List<TPilar> Pilares, ref List<TObjetoDesenho> Objects, bool FromGrip = false)
        {
            base.pFin = point;

            if (!base.pFin.Incidente(this))
                base.pFin.incidencias.Add(this);
            
            AddGrips();
            AddPoligonoSelecao();

            return eObjetoDesenhoMouseDown.Done;
        }

        public override TObjetoDesenho Clone()
        {
            TCirculo l = new TCirculo();
            l.Copy(this);
            return l;
        }
        
        public void Copy(TCirculo obj)
        {
            base.Copy(obj);
            this.x = obj.x;
            this.y = obj.y;
            this.raio = obj.raio;
            this.layer = obj.layer;
        }

    }
}
