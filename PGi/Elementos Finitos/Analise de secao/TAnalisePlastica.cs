using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Policy;
using System.Text;
using System.Threading.Tasks;

namespace PG
{
    public class TAnalisePlastica
    {
        public List<Q4_Secao> q4;
        public List<T6_Secao> t6;
        public TAnalisePlastica(List<Q4_Secao> _q4, List<T6_Secao> _t6) 
        {
            q4 = _q4;
            t6 = _t6;   
        }

        class Ponto
        {
            public double X, Y;
            public Ponto(double x, double y) { X = x; Y = y; }
        }

        class Elemento
        {
            public List<Ponto> Nos; // pode ter 3, 4 ou mais pontos
        }
        
        List<Elemento> elementos = new List<Elemento>();

        Ponto InterpolarY(Ponto p1, Ponto p2, double yLinha)
        {
            double t = (yLinha - p1.Y) / (p2.Y - p1.Y);

            return new Ponto(
                p1.X + t * (p2.X - p1.X),
                yLinha
            );
        }

        List<Ponto> ClipVertical(List<Ponto> poligono, double xLinha, bool direita)
        {
            List<Ponto> saida = new List<Ponto>();

            for (int i = 0; i < poligono.Count; i++)
            {
                Ponto atual = poligono[i];
                Ponto prox = poligono[(i + 1) % poligono.Count];

                bool dentroAtual = direita ? atual.X >= xLinha : atual.X <= xLinha;
                bool dentroProx = direita ? prox.X >= xLinha : prox.X <= xLinha;

                if (dentroAtual && dentroProx)
                {
                    saida.Add(prox);
                }
                else if (dentroAtual && !dentroProx)
                {
                    saida.Add(InterpolarX(atual, prox, xLinha));
                }
                else if (!dentroAtual && dentroProx)
                {
                    saida.Add(InterpolarX(atual, prox, xLinha));
                    saida.Add(prox);
                }
            }

            return saida;
        }
        static Ponto InterpolarX(Ponto p1, Ponto p2, double xLinha)
        {
            double t = (xLinha - p1.X) / (p2.X - p1.X);

            return new Ponto(
                xLinha,
                p1.Y + t * (p2.Y - p1.Y)
            );
        }
        List<Ponto> ClipHorizontal(List<Ponto> poligono, double yLinha, bool acima)
        {
            List<Ponto> saida = new List<Ponto>();

            for (int i = 0; i < poligono.Count; i++)
            {
                Ponto atual = poligono[i];
                Ponto prox = poligono[(i + 1) % poligono.Count];

                bool dentroAtual = acima ? atual.Y >= yLinha : atual.Y <= yLinha;
                bool dentroProx = acima ? prox.Y >= yLinha : prox.Y <= yLinha;

                if (dentroAtual && dentroProx)
                {
                    saida.Add(prox);
                }
                else if (dentroAtual && !dentroProx)
                {
                    saida.Add(InterpolarY(atual, prox, yLinha));
                }
                else if (!dentroAtual && dentroProx)
                {
                    saida.Add(InterpolarY(atual, prox, yLinha));
                    saida.Add(prox);
                }
            }

            return saida;
        }
        double AreaPoligono(List<Ponto> pts)
        {
            double A = 0;

            for (int i = 0; i < pts.Count; i++)
            {
                var p1 = pts[i];
                var p2 = pts[(i + 1) % pts.Count];

                A += (p1.X * p2.Y - p2.X * p1.Y);
            }

            return Math.Abs(A) / 2.0;
        }
        static double CentroideX(List<Ponto> pts)
        {
            double A = 0;
            double Cx = 0;

            for (int i = 0; i < pts.Count; i++)
            {
                var p1 = pts[i];
                var p2 = pts[(i + 1) % pts.Count];

                double cross = (p1.X * p2.Y - p2.X * p1.Y);

                A += cross;
                Cx += (p1.X + p2.X) * cross;
            }

            A *= 0.5;

            if (Geom.Iguais(6 * A, 0))
                Cx = 0;
            else
                Cx /= (6 * A);

            return Cx;
        }
        double CentroideY(List<Ponto> pts)
        {
            double A = 0;
            double Cy = 0;

            for (int i = 0; i < pts.Count; i++)
            {
                var p1 = pts[i];
                var p2 = pts[(i + 1) % pts.Count];

                double cross = (p1.X * p2.Y - p2.X * p1.Y);

                A += cross;
                Cy += (p1.Y + p2.Y) * cross;
            }

            A *= 0.5;
            if (Geom.Iguais(6*A,0))
                Cy = 0;
            else
                Cy /= (6 * A);

            return Cy;
        }
        double FuncaoEquilibrioVertical(double xLinha)
        {
            double direita = 0.0;
            double esquerda = 0.0;

            foreach (var elem in elementos)
            {
                var dir = ClipVertical(elem.Nos, xLinha, true);
                var esq = ClipVertical(elem.Nos, xLinha, false);

                if (dir.Count >= 3)
                    direita += AreaPoligono(dir);

                if (esq.Count >= 3)
                    esquerda += AreaPoligono(esq);
            }

            return direita - esquerda;
        }
        double FuncaoEquilibrioHorizontal(double yLinha)
        {
            double acima = 0.0;
            double abaixo = 0.0;

            foreach (var elem in elementos)
            {
                var acimaPol = ClipHorizontal(elem.Nos, yLinha, true);
                var abaixoPol = ClipHorizontal(elem.Nos, yLinha, false);

                if (acimaPol.Count >= 3)
                    acima += AreaPoligono(acimaPol);

                if (abaixoPol.Count >= 3)
                    abaixo += AreaPoligono(abaixoPol);
            }

            return acima - abaixo;
        }

        double Brent(double a, double b, string eixo, double tol = 1e-6, int maxIter = 100)
        {
            double fa;
            double fb;

            if (eixo == "horizontal")
            {
                fa = FuncaoEquilibrioHorizontal(a);
                fb = FuncaoEquilibrioHorizontal(b);
            }
            else // vertical
            {
               fa = FuncaoEquilibrioVertical(a);
               fb = FuncaoEquilibrioVertical(b);
            }
            
            if (fa * fb >= 0)
                throw new Exception("Raiz não está no intervalo.");

            if (Math.Abs(fa) < Math.Abs(fb))
            {
                double tmp = a; a = b; b = tmp;
                tmp = fa; fa = fb; fb = tmp;
            }

            double c = a, fc = fa;
            double d = 0;
            bool mflag = true;
            double s = b;

            for (int i = 0; i < maxIter; i++)
            {
                if (fa != fc && fb != fc)
                {
                    s = a * fb * fc / ((fa - fb) * (fa - fc))
                      + b * fa * fc / ((fb - fa) * (fb - fc))
                      + c * fa * fb / ((fc - fa) * (fc - fb));
                }
                else
                {
                    s = b - fb * (b - a) / (fb - fa);
                }

                if ((s < (3 * a + b) / 4 || s > b) ||
                    (mflag && Math.Abs(s - b) >= Math.Abs(b - c) / 2) ||
                    (!mflag && Math.Abs(s - b) >= Math.Abs(c - d) / 2))
                {
                    s = (a + b) / 2;
                    mflag = true;
                }
                else
                {
                    mflag = false;
                }

                double fs;

                if (eixo == "horizontal")
                    fs = FuncaoEquilibrioHorizontal(s);
                else
                    fs = FuncaoEquilibrioVertical(s);

                d = c;
                c = b; fc = fb;

                if (fa * fs < 0)
                {
                    b = s; fb = fs;
                }
                else
                {
                    a = s; fa = fs;
                }

                if (Math.Abs(fa) < Math.Abs(fb))
                {
                    double tmp = a; 
                    a = b; 
                    b = tmp;
                    tmp = fa; 
                    fa = fb; 
                    fb = tmp;
                }

                if (Math.Abs(b - a) < tol)
                    return b;
            }

            throw new Exception("Modulo plástico não convergiu.");
        }
        double ModuloPlasticoZ(double zPlastico)
        {
            double Z = 0;

            foreach (var elem in elementos)
            {
                var dir = ClipVertical(elem.Nos, zPlastico, true);
                var esq = ClipVertical(elem.Nos, zPlastico, false);

                if (dir.Count >= 3)
                {
                    double A = AreaPoligono(dir);
                    double Cx = CentroideX(dir);
                    Z += A * Math.Abs(Cx - zPlastico);
                }

                if (esq.Count >= 3)
                {
                    double A = AreaPoligono(esq);
                    double Cx = CentroideX(esq);
                    Z += A * Math.Abs(Cx - zPlastico);
                }
            }

            return Z;
        }
        double ModuloPlasticoY(double yPlastico)
        {
            double Z = 0;

            foreach (var elem in elementos)
            {
                var acima = ClipHorizontal(elem.Nos, yPlastico, true);
                var abaixo = ClipHorizontal(elem.Nos, yPlastico, false);

                if (acima.Count >= 3)
                {
                    double A = AreaPoligono(acima);
                    double Cy = CentroideY(acima);
                    Z += A * Math.Abs(Cy - yPlastico);
                }

                if (abaixo.Count >= 3)
                {
                    double A = AreaPoligono(abaixo);
                    double Cy = CentroideY(abaixo);
                    Z += A * Math.Abs(Cy - yPlastico);
                }
            }

            return Z;
        }

        public void CalcularModulos()
        {
            yPlastico = Brent(yMin, yMax, "horizontal");
            moduloPlasticoY = ModuloPlasticoY(yPlastico);

            zPlastico = Brent(xMin, xMax, "vertical");
            moduloPlasticoZ = ModuloPlasticoZ(zPlastico);

        }

        public double yPlastico, zPlastico, moduloPlasticoZ, moduloPlasticoY;

        double yMin = 999999;
        double yMax = -99999;

        double xMin = 999999;
        double xMax = -99999;

        public void CriarElementos()
        {
            elementos = new List<Elemento>();

           /* elementos.Add(new Elemento
            {
                Nos = new List<Ponto> {  new Ponto(0,2),  new Ponto(2,2), new Ponto(2,4), new Ponto(0,4)}
            });

            xMin = 0;
            xMax = 2;
            yMin = 2;
            yMax = 4;

            return;*/

            for (int i = 0; i < q4.Count; i++)
            {
                Q4_Secao quad = q4[i];

                if (Geom.Iguais(quad.area, 0))
                {
                    quad.id *= 1;
                    continue;
                }

                if (quad.n1.y < yMin)
                    yMin = quad.n1.y;
                if (quad.n2.y < yMin)
                    yMin = quad.n2.y;
                if (quad.n3.y < yMin)
                    yMin = quad.n3.y;
                if (quad.n4.y < yMin)
                    yMin = quad.n4.y;

                if (quad.n1.y > yMax)
                    yMax = quad.n1.y;
                if (quad.n2.y > yMax)
                    yMax = quad.n2.y;
                if (quad.n3.y > yMax)
                    yMax = quad.n3.y;
                if (quad.n4.y > yMax)
                    yMax = quad.n4.y;

                if (quad.n1.x < xMin)
                    xMin = quad.n1.x;
                if (quad.n2.x < xMin)
                    xMin = quad.n2.x;
                if (quad.n3.x < xMin)
                    xMin = quad.n3.x;
                if (quad.n4.x < xMin)
                    xMin = quad.n4.x;

                if (quad.n1.x > xMax)
                    xMax = quad.n1.x;
                if (quad.n2.x > xMax)
                    xMax = quad.n2.x;
                if (quad.n3.x > xMax)
                    xMax = quad.n3.x;
                if (quad.n4.x > xMax)
                    xMax = quad.n4.x;

                elementos.Add(
               new Elemento
               {
                   Nos = new List<Ponto>
                    {
                        new Ponto(quad.n1.x,quad.n1.y ),
                        new Ponto(quad.n2.x,quad.n2.y ),
                        new Ponto(quad.n3.x,quad.n3.y ),
                        new Ponto(quad.n4.x,quad.n4.y )
                    }
               });
            }

            for (int i = 0; i < t6.Count; i++)
            {
                T6_Secao tri = t6[i];

                if (Geom.Iguais(tri.area, 0))
                {
                    tri.id *= 1;
                    continue;
                }

                if (tri.n1.y < yMin)
                    yMin = tri.n1.y;
                if (tri.n2.y < yMin)
                    yMin = tri.n2.y;
                if (tri.n3.y < yMin)
                    yMin = tri.n3.y;

                if (tri.n1.y > yMax)
                    yMax = tri.n1.y;
                if (tri.n2.y > yMax)
                    yMax = tri.n2.y;
                if (tri.n3.y > yMax)
                    yMax = tri.n3.y;

                if (tri.n1.x < xMin)
                    xMin = tri.n1.x;
                if (tri.n2.x < xMin)
                    xMin = tri.n2.x;
                if (tri.n3.x < xMin)
                    xMin = tri.n3.x;

                if (tri.n1.x > xMax)
                    xMax = tri.n1.x;
                if (tri.n2.x > xMax)
                    xMax = tri.n2.x;
                if (tri.n3.x > xMax)
                    xMax = tri.n3.x;

                elementos.Add(
               new Elemento
               {
                   Nos = new List<Ponto>
                    {
                        new Ponto(tri.n1.x,tri.n1.y ),
                        new Ponto(tri.n2.x,tri.n2.y ),
                        new Ponto(tri.n3.x,tri.n3.y )
                    }
               });
            }
        }
    }
}
