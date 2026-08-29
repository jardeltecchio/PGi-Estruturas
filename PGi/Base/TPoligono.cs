using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PG
{
    [Serializable]
    public class TPoligono : TObjetoDesenho
    {
        
        public TPoligono() { base.Tipo = this.Tipo; }


        public TPoligono(string tipo, int registro, bool selecionado) : base(tipo, registro, selecionado) { }
           
        public TPoligono(CoordenadaD[] coords) 
        {
          this.coords = coords;
          this.registro++;
        }

        public TPoligono(string tipo, string h1, string b1, string b2, string h2, string a1, string a2, bool vazada)
        {
            this.TipoSecao = tipo;
            if (tipo == "Retangular")
            {
                if (vazada)
                {
                    coords = new CoordenadaD[10];
                    this.linhas_poligonal = new List<TLinha>();
                 
                    for (int i = 0; i < 8; i++)
                        this.linhas_poligonal.Add(new TLinha());
                }
                else
                {
                    coords = new CoordenadaD[5];

                    this.linhas_poligonal = new List<TLinha>();
                    for (int i = 0; i < 4; i++)
                        this.linhas_poligonal.Add(new TLinha());
                }

                coords[0].X = 0;
                coords[0].Y = 0;

                coords[1].X = System.Convert.ToSingle(b1);
                coords[1].Y = 0;

                coords[2].X = System.Convert.ToSingle(b1);
                coords[2].Y = System.Convert.ToSingle(h1);

                coords[3].X = 0;
                coords[3].Y = System.Convert.ToSingle(h1);

                coords[4].X = 0;
                coords[4].Y = 0;

                if (vazada)
                {
                    coords[5].X = (System.Convert.ToSingle(b1) - System.Convert.ToSingle(b2)) / 2;
                    coords[5].Y = (System.Convert.ToSingle(h1) - System.Convert.ToSingle(h2)) / 2;

                    coords[6].X = coords[5].X + System.Convert.ToSingle(b2);
                    coords[6].Y = coords[5].Y;

                    coords[7].X = coords[6].X;
                    coords[7].Y = coords[5].Y + System.Convert.ToSingle(h2);

                    coords[8].X = coords[5].X;
                    coords[8].Y = coords[7].Y;

                    coords[9].X = coords[5].X;
                    coords[9].Y = coords[5].Y;
                }

                PreencheCoordsLinhas(vazada ? 8: 4);
            }
            else
            if (tipo == "T")
            {
                coords = new CoordenadaD[9];
                this.linhas_poligonal = new List<TLinha>();

                for (int i = 0; i < 8; i++)
                  this.linhas_poligonal.Add(new TLinha());

                coords[0].X = 0;
                coords[0].Y = 0;

                coords[1].X = System.Convert.ToSingle(b2);
                coords[1].Y = 0;

                coords[2].X = System.Convert.ToSingle(b2);
                coords[2].Y = System.Convert.ToSingle(h1) - System.Convert.ToSingle(h2);

                coords[3].X = System.Convert.ToSingle(b2) + ((System.Convert.ToSingle(b1) - System.Convert.ToSingle(b2)) / 2);
                coords[3].Y = System.Convert.ToSingle(h1) - System.Convert.ToSingle(h2);

                coords[4].X = System.Convert.ToSingle(b2) + ((System.Convert.ToSingle(b1) - System.Convert.ToSingle(b2)) / 2);
                coords[4].Y = System.Convert.ToSingle(h1);

                coords[5].X = coords[4].X - System.Convert.ToSingle(b1);
                coords[5].Y = System.Convert.ToSingle(h1);

                coords[6].X = coords[5].X;
                coords[6].Y = coords[3].Y;

                coords[7].X = 0;
                coords[7].Y = coords[3].Y;

                coords[8].X = 0;
                coords[8].Y = 0;

                PreencheCoordsLinhas(8);
            }
            else
            if (tipo == "I")
            {
                coords = new CoordenadaD[13];
                this.linhas_poligonal = new List<TLinha>();

                for (int i = 0; i < 12; i++)
                  this.linhas_poligonal.Add(new TLinha());

                coords[0].X = 0;
                coords[0].Y = 0;

                coords[1].X = System.Convert.ToSingle(b1);
                coords[1].Y = 0;

                coords[2].X = System.Convert.ToSingle(b1);
                coords[2].Y = System.Convert.ToSingle(h2);

                coords[3].X = (System.Convert.ToSingle(b1) / 2) + (System.Convert.ToSingle(b2) / 2);
                coords[3].Y = System.Convert.ToSingle(h2);

                coords[4].X = (System.Convert.ToSingle(b1) / 2) + (System.Convert.ToSingle(b2) / 2);
                coords[4].Y = System.Convert.ToSingle(h1) - System.Convert.ToSingle(h2);

                coords[5].X = System.Convert.ToSingle(b1);
                coords[5].Y = System.Convert.ToSingle(h1) - System.Convert.ToSingle(h2);

                coords[6].X = System.Convert.ToSingle(b1);
                coords[6].Y = System.Convert.ToSingle(h1);

                coords[7].X = 0;
                coords[7].Y = System.Convert.ToSingle(h1);

                coords[8].X = 0;
                coords[8].Y = System.Convert.ToSingle(h1) - System.Convert.ToSingle(h2);

                coords[9].X = (System.Convert.ToSingle(b1) / 2) - (System.Convert.ToSingle(b2) / 2);
                coords[9].Y = System.Convert.ToSingle(h1) - System.Convert.ToSingle(h2);

                coords[10].X = (System.Convert.ToSingle(b1) / 2) - (System.Convert.ToSingle(b2) / 2);
                coords[10].Y = System.Convert.ToSingle(h2);

                coords[11].X = 0;
                coords[11].Y = System.Convert.ToSingle(h2);

                coords[12].X = 0;
                coords[12].Y = 0;

                PreencheCoordsLinhas(12);
            }
            else
            if (tipo == "L")
            {
                coords = new CoordenadaD[7];

                this.linhas_poligonal = new List<TLinha>();
                for (int i = 0; i < 6; i++)
                    this.linhas_poligonal.Add(new TLinha());

                coords[0].X = 0;
                coords[0].Y = 0;

                coords[1].X = System.Convert.ToSingle(b1);
                coords[1].Y = 0;

                coords[2].X = System.Convert.ToSingle(b1);
                coords[2].Y = System.Convert.ToSingle(h2);

                coords[3].X = System.Convert.ToSingle(b2);
                coords[3].Y = System.Convert.ToSingle(h2);

                coords[4].X = System.Convert.ToSingle(b2);
                coords[4].Y = System.Convert.ToSingle(h1);

                coords[5].X = 0;
                coords[5].Y = System.Convert.ToSingle(h1);

                coords[6].X = 0;
                coords[6].Y = 0;

                PreencheCoordsLinhas(6);
            }
            else
            if (tipo == "Circular")
            {
                coords = new CoordenadaD[51];
                double raio = System.Convert.ToSingle(h1)/2;
                float twicePi = 2.0f * 3.1415f;
                
                double x = raio;
                double y = raio;

                for (int j = 0; j <= 50; j++)
                {
                    coords[j].X = (x + (raio * Math.Cos(j * twicePi / 50)));
                    coords[j].Y = (y + (raio * Math.Sin(j * twicePi / 50)));
                }

                this.linhas_poligonal = new List<TLinha>();
                for (int i = 0; i < 50; i++)
                    this.linhas_poligonal.Add(new TLinha());
                PreencheCoordsLinhas(50);
            }
        }

        public void AtualizaLinhasPoligonal()
        {
            foreach(TLinha l in linhas_poligonal)
               l.comprimento = (float)(Math.Sqrt(Math.Pow(l.pIni.x - l.pFin.x, 2) + Math.Pow(l.pIni.y - l.pFin.y, 2)));

        }
        void PreencheCoordsLinhas(int qtd)
        {
            for (int i = 0; i < qtd; i++)
            {
                linhas_poligonal[i].pIni.x = coords[i].X;
                linhas_poligonal[i].pIni.y = coords[i].Y;

                if (i == (qtd-1))
                {
                    linhas_poligonal[i].pFin.x = coords[0].X;
                    linhas_poligonal[i].pFin.y = coords[0].Y;
                }
                else
                {
                    linhas_poligonal[i].pFin.x = coords[i + 1].X;
                    linhas_poligonal[i].pFin.y = coords[i + 1].Y;
                }

                linhas_poligonal[i].comprimento = (float)(Math.Sqrt(Math.Pow(linhas_poligonal[i].pIni.x - linhas_poligonal[i].pFin.x, 2) + Math.Pow(linhas_poligonal[i].pIni.y - linhas_poligonal[i].pFin.y, 2)));
   
            }

            CalculaPropriedades();
        }

        public TPoligono(ref CoordenadaD[] coords, ref List<TLinha> linhas)
        {
            this.coords = coords;
            this.linhas_poligonal = linhas;
            this.registro++;
        }
        
        public TPoligono(CoordenadaD[] coords, bool calcular)
        {
            this.coords = coords;
            if (calcular)
                CalculaPropriedades(coords);
        }

        public void AtualizaCoords()
        {
            int nn = 0;

            int tam = linhas_poligonal.Count + 1;
            coords = new CoordenadaD[tam];

            for (int i = 0; i < linhas_poligonal.Count; i++)
            {

                if (i == 0)
                {
                    coords[nn].X = linhas_poligonal[i].pIni.x;
                    coords[nn++].Y = linhas_poligonal[i].pIni.y;

                    coords[nn].X = linhas_poligonal[i].pFin.x;
                    coords[nn++].Y = linhas_poligonal[i].pFin.y;
         
                }
                else
                {
                    if (!LocCoord(linhas_poligonal[i].pIni.x, linhas_poligonal[i].pIni.y))
                    {
                        coords[nn].X = linhas_poligonal[i].pIni.x;
                        coords[nn++].Y = linhas_poligonal[i].pIni.y;
                    }

                    if (!LocCoord(linhas_poligonal[i].pFin.x, linhas_poligonal[i].pFin.y))
                    {
                        coords[nn].X = linhas_poligonal[i].pFin.x;
                        coords[nn++].Y = linhas_poligonal[i].pFin.y;
                    }
                }
            }

            coords[nn].X = linhas_poligonal[linhas_poligonal.Count-1].pFin.x;
            coords[nn].Y = linhas_poligonal[linhas_poligonal.Count-1].pFin.y;
        }

        bool LocCoord(double x, double y)
        {
            for (int i = 0; i < coords.Length; i++)
                if (Geom.Iguais(x, coords[i].X) && Geom.Iguais(y, coords[i].Y))
                    return true;

            return false;
        }
        public double Max_X, Max_Y, Min_X, Min_Y;

        public void CalculaPropriedades(CoordenadaD[] Coords = null)  //Calcula propriedades de um polígono qualquer - Algoritmo do Andrew
        {
            int i, j, N;
            double area, Per, Ix, Iy; 
            double Ixy; //produto de inércia
            CoordenadaD C;

            if (Coords == null) 
                Coords = coords;
            double Ixcg, Iycg, rx, ry, ai,  dx, dy, P;
            area = 0;

            C.X = 0;
            C.Y = 0;
            N = Coords.Length;
            Ix = 0;
            Iy = 0;
            Ixy = 0;
            Max_X = 0; Max_Y = 0;
            Min_X = 0; Min_Y = 0;
            Per = 0;
            dx = 0; dy = 0;

            for (i = 0; i < N - 1; i++)
            {
                j = (i + 1) % N;
                area +=  (Coords[i].X * Coords[j].Y - Coords[j].X * Coords[i].Y) / 2;
                ai   = Coords[i].X * Coords[j].Y - Coords[j].X * Coords[i].Y;
                Ix  += (Math.Pow(Coords[i].Y, 2) + Coords[i].Y * Coords[j].Y + Math.Pow(Coords[j].Y, 2)) * ai / 12;
               
             //   Ix:= Ix + (Sqr(Points[i].Y)+Points[i].Y*Points[j].Y+Sqr(Points[j].Y))*ai/12;
  
                Iy  += (Math.Pow(Coords[i].X, 2) + Coords[i].X * Coords[j].X + Math.Pow(Coords[j].X, 2)) * ai / 12;
                Ixy += (Coords[i].X * Coords[j].Y + 2 * Coords[i].X * Coords[i].Y + 2 * Coords[j].X * Coords[j].Y + Coords[j].X * Coords[i].Y) * ai / 24;
                P   = Coords[i].X * Coords[j].Y - Coords[j].X * Coords[i].Y;
                C.X = (C.X + (Coords[i].X + Coords[j].X) * P);
                C.Y = (C.Y + (Coords[i].Y + Coords[j].Y) * P);

                Max_X = Math.Max(Coords[i].X, Max_X);
                Max_Y = Math.Max(Coords[i].Y, Max_Y);
                Min_X = Math.Min(Coords[i].X, Min_X);
                Min_Y = Math.Min(Coords[i].Y, Min_Y);

                dx = Math.Abs(Coords[j].X - Coords[i].X);
                dy = Math.Abs(Coords[j].Y - Coords[i].Y);
                Per += Math.Sqrt(Math.Pow(dx,2) + Math.Pow(dy,2));
            };

            C.X = System.Convert.ToSingle(C.X / (6 * area));
            C.Y = System.Convert.ToSingle(C.Y / (6 * area));

            Ixcg = Ix - Math.Pow(C.Y,2) * area;
            Iycg = Iy - Math.Pow(C.X,2) * area;
            rx = Math.Sqrt(Ixcg / area);
            ry = Math.Sqrt(Iycg / area);

            this.area = Math.Abs(area);
            this.centroide.X = C.X;
            this.centroide.Y = C.Y;
            this.perimetro = Per;

            this.Ix= Ix;
            this.Iy= Iy;
            this.Ixy= Ixy;
            this.Ixcg = Math.Abs(Ixcg);
            this.Iycg = Math.Abs(Iycg);
            this.rx   = rx;
            this.ry   = ry;

            this.fMax.X = System.Convert.ToSingle(Max_X);
            this.fMax.Y = System.Convert.ToSingle(Max_Y);
            this.fMin.X = System.Convert.ToSingle(Min_X);
            this.fMin.Y = System.Convert.ToSingle(Min_Y);

            this.WxInf = Ixcg/Math.Abs(C.Y-fMin.Y);
            this.WxSup = Ixcg/Math.Abs(fMax.Y-C.Y);
            this.WxMax = Ixcg/Math.Max(Math.Abs(C.Y-fMin.Y),Math.Abs(fMax.Y-C.Y));
            this.WyInf = Iycg/Math.Abs(C.X-fMin.X);
            this.WySup = Iycg/Math.Abs(fMax.X-C.X);
            this.WyMax = Iycg/Math.Max(Math.Abs(C.X-fMin.X),Math.Abs(fMax.X-C.X));
        }

        public bool FindLin(TLinha l)
        {
            foreach (TLinha lin in linhas_poligonal)
                if ((l.pIni == lin.pIni) && (l.pFin == lin.pFin))
                    return true;

            return false;
        }
        public bool PontoEmPoligono(double x, double y)
        {
            bool inside = false;
            double x_intersec;
            int i;

            for (i = 0; i < linhas_poligonal.Count; i++)
            {
                if (((linhas_poligonal[i].pIni.y > y) && (linhas_poligonal[i].pFin.y < y)) ||
                    ((linhas_poligonal[i].pIni.y < y) && (linhas_poligonal[i].pFin.y > y)))
                {
                    //    x_intersec = U0.x + (P.y -U0.y) * (U1.x - U0.x) / (U1.y - U0.y);

                    //livro "geometric tools for computer graphics"  pg.700
                    x_intersec = linhas_poligonal[i].pIni.x + (y - linhas_poligonal[i].pIni.y) * (linhas_poligonal[i].pFin.x - linhas_poligonal[i].pIni.x) / (linhas_poligonal[i].pFin.y - linhas_poligonal[i].pIni.y);

                    if (x_intersec > x)
                      inside = !inside;
                };
            };

            return inside;
        }
        public CoordenadaD[] coords;
        [NonSerialized]
        public List<TLinha> linhas_poligonal;

        public int registro;
        public bool externo;
        public double Ixy;        // produto de inércia
        public double rx, ry;
        public double Ixcg, Iycg; //momentos de inércia em X e Y
        public double area, perimetro;
        public CoordenadaD centroide, fMax, fMin;
        public double Ix, Iy, WxInf, WxSup, WyInf, WySup, WxMax, WyMax;
        public string Tipo;
        public string TipoSecao;

        public override TObjetoDesenho Clone()
        {
            TPoligono l = new TPoligono();
            l.Copy(this);
            return l;
        }

        public void Copy(TPoligono o)
        {
            base.Copy(o);
            this.linhas_poligonal = new List<TLinha>();
            if ((Object)o.linhas_poligonal != null)
                foreach (TLinha lin in o.linhas_poligonal)
                    linhas_poligonal.Add((TLinha)lin.Clone());

            centroide = o.centroide;
            fMax = o.fMax;
            fMin = o.fMin;
            externo = o.externo;
            if ((Object)o.coords != null)
            {
                coords = new CoordenadaD[o.coords.Count()];
                int i = -1;
                foreach (CoordenadaD c in o.coords)
                {
                    i++;
                    coords[i].X = o.coords[i].X;
                    coords[i].Y = o.coords[i].Y;
                    coords[i].pontoEmRaio = o.coords[i].pontoEmRaio;
                }
            }

           registro = o.registro;

           Ixy = o.Ixy;   
           rx = o.rx; 
            ry = o.ry;
           Ixcg = o.Ixcg; 
            Iycg = o.Iycg;
           area = o.area; 
            perimetro = o.perimetro;
           Ix = o.Ix;Iy = o.Iy; 
            WxInf = o.WxInf; 
            WxSup = o.WxSup; 
            WyInf = o.WyInf;
            WySup = o.WySup; 
            WxMax = o.WxMax; 
            WyMax = o.WyMax;
           Tipo = o.Tipo;
            if (o.TipoSecao != null)
             TipoSecao = o.TipoSecao;
         //  CalculaPropriedades();
        }

    }
}
