using MathNet.Numerics;
using MathNet.Numerics.Distributions;
using OpenTK;
using OpenTK.Graphics.OpenGL;
using OpenTK.Input;
using OpenTK.Platform;
using OpenTK.Platform.Windows;
using Poly2Tri;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Linq;
using System.Net.NetworkInformation;
using System.Text;
using System.Threading.Tasks;
using Win32Interop.Enums;
using Win32Interop.Structs;
using static Microsoft.TeamFoundation.Client.CommandLine.Options;
using static Microsoft.WindowsAPICodePack.Shell.PropertySystem.SystemProperties.System;

namespace PG
{
    [Serializable]
    public struct ColorHSV
    {
        public double H; // 0-360
        public double S; // 0-1
        public double V; // 0-1
    }
    public struct ColorF
    {
        public double R;
        public double G;
        public double B;

        public ColorF(double r, double g, double b)
        {
            R = r;
            G = g;
            B = b;
        }
    }
    public static class ColorGenerator
    {
        private const double GoldenAngle = 137.50776405003785;
        public static void RgbToHsv(
    double r,
    double g,
    double b,
    out double h,
    out double s,
    out double v)
        {
            double max = Math.Max(r, Math.Max(g, b));
            double min = Math.Min(r, Math.Min(g, b));

            v = max;

            double delta = max - min;

            s = max == 0 ? 0 : delta / max;

            if (delta == 0)
                h = 0;
            else if (max == r)
                h = 60 * (((g - b) / delta) % 6);
            else if (max == g)
                h = 60 * (((b - r) / delta) + 2);
            else
                h = 60 * (((r - g) / delta) + 4);

            if (h < 0)
                h += 360;
        }
        public static void HsvToRgb(
    double h,
    double s,
    double v,
    out double r,
    out double g,
    out double b)
        {
            double c = v * s;
            double x = c * (1 - Math.Abs((h / 60.0) % 2 - 1));
            double m = v - c;

            if (h < 60)
                (r, g, b) = (c, x, 0);
            else if (h < 120)
                (r, g, b) = (x, c, 0);
            else if (h < 180)
                (r, g, b) = (0, c, x);
            else if (h < 240)
                (r, g, b) = (0, x, c);
            else if (h < 300)
                (r, g, b) = (x, 0, c);
            else
                (r, g, b) = (c, 0, x);

            r += m;
            g += m;
            b += m;
        }

        /*public static ColorF NextGoldenColor(ColorF color)
        {
            double h, s, v;

            RgbToHsv(color.R, color.G, color.B, out h, out s, out v);

            h = (h + 137.50776405003785) % 360.0;

            double r, g, b;
            HsvToRgb(h, s, v, out r, out g, out b);

            return new ColorF(r, g, b);
        }*/

        public static ColorF NextGoldenColor(ColorF color)
        {
            double h, s, v;

            RgbToHsv(color.R, color.G, color.B, out h, out s, out v);

            // Cor acromática: preto, branco ou cinza.
            // O Hue não possui significado nesse caso.
            if (s < 0.01)
            {
                h = 0;
                s = 0.70;

                // Evita gerar uma cor muito escura
                if (v < 0.50)
                    v = 0.80;
            }

            h = (h + 137.50776405003785) % 360.0;

            double r, g, b;

            HsvToRgb(h, s, v, out r, out g, out b);

            return new ColorF(r, g, b);
        }

    }

    public static class GeradorDeFillet
    {

        public static string QualSegmento(
                vec3 A,
                vec3 V,
                vec3 B,
                vec3 P)
        {
            vec3 d1 = (A - V).Normalize();
            vec3 d2 = (B - V).Normalize();

            vec3 dp = (P - V).Normalize();

            double dotA = d1.DotProduct(dp);
            double dotB = d2.DotProduct(dp);

            if (dotA > dotB)
                return "A";
            else
            if (dotA < dotB)
                return "B";

            return "nenhum";
        }

        public static double Clamp(double value, double min, double max)
        {
            if (value < min)
                return min;

            if (value > max)
                return max;

            return value;
        }

        public static List<vec3> CriarFillet(
            vec3 A,
            vec3 V,
            vec3 B,
            double raios,
            int segmentos)
        {
            List<vec3> pts = new List<vec3>();

            //---------------------------------------------------
            // Direções saindo do vértice
            //---------------------------------------------------

            vec3 d1 = (A - V).Normalize();
            vec3 d2 = (B - V).Normalize();

            //---------------------------------------------------
            // Ângulo interno
            //---------------------------------------------------

            double dot = d1.DotProduct(d2);

            dot = Clamp(dot, -1.0, 1.0);

            double theta = Math.Acos(dot);

            //---------------------------------------------------
            // Casos degenerados
            //---------------------------------------------------

            const double EPS = 1e-8;

            if (theta < EPS || Math.Abs(Math.PI - theta) < EPS)
                return pts;

            //---------------------------------------------------
            // Distância até tangência
            //---------------------------------------------------

            double t = raios / Math.Tan(theta / 2.0);

            //---------------------------------------------------
            // Pontos de tangência
            //---------------------------------------------------

            vec3 T1 = V + d1 * (double)t;
            vec3 T2 = V + d2 * (double)t;

            //---------------------------------------------------
            // Bissetriz
            //---------------------------------------------------

            vec3 bisector = (d1 + d2).Normalize();

            //---------------------------------------------------
            // Distância até centro
            //---------------------------------------------------

            double distToCenter = raios / Math.Sin(theta / 2.0);

            //---------------------------------------------------
            // Centro do arco
            //---------------------------------------------------

            vec3 C = V + bisector * (float)distToCenter;

            //---------------------------------------------------
            // Ângulos dos pontos tangentes
            //---------------------------------------------------

            double a1 = Math.Atan2(T1.y - C.y, T1.x - C.x);
            double a2 = Math.Atan2(T2.y - C.y, T2.x - C.x);

            //---------------------------------------------------
            // Produto vetorial 2D
            //---------------------------------------------------

            double cross = d1.x * d2.y - d1.y * d2.x;

            //---------------------------------------------------
            // Ajustar sentido corretamente
            //---------------------------------------------------

            // anti-horário
            if (cross < 0)
            {
                if (a2 < a1)
                    a2 += 2.0 * Math.PI;
            }
            // horário
            else
            {
                if (a1 < a2)
                    a1 += 2.0 * Math.PI;
            }

            //---------------------------------------------------
            // Passo angular
            //---------------------------------------------------

            double step = (a2 - a1) / segmentos;

            //---------------------------------------------------
            // Discretização
            //---------------------------------------------------

            for (int i = 0; i <= segmentos; i++)
            {
                double a = a1 + step * i;

                double x = C.x + (raios * Math.Cos(a));
                double y = C.y + (raios * Math.Sin(a));

                pts.Add(new vec3(x, y, 0));
            }

            return pts;
        }
    }
    public static class Clipper3D
    {
        static double t0;
        static double t1;

        public static bool Project3(Vector3d world, Matrix4d view_x_projection, Size viewport, out Vector3d pixel, out Vector4d clip)
        {
            pixel = Vector3d.Zero;
            clip = Vector4d.Zero;

            // Coordenada homogênea
            Vector4d p = new Vector4d(world.X, world.Y, world.Z, 1.0);

            // World -> Clip
            //Matrix4d vp = RMath.Multiply(view, projection);
            clip = RMath.Multiply(p, view_x_projection);

            // Não é possível projetar
            if (Math.Abs(clip.W) < 1e-12)
                return false;

            // NDC
            Vector4d ndc = clip / clip.W;

            // Pixel
            pixel.X = (ndc.X + 1.0) * 0.5 * viewport.Width;
            pixel.Y = (1.0 - ndc.Y) * 0.5 * viewport.Height;

            // Profundidade igual ao gluProject()
            pixel.Z = (ndc.Z + 1.0) * 0.5;

            return true;
        }
        public static bool ProjetarLinhaSelecao(Matrix4d vp, bool perspectiva,Vector3d p1,Vector3d p2,out double x1,out double y1,out double x2,out double y2)
        {
            if (perspectiva)
            {
                Vector4d clip1 = FPrincipal.WorldToClip(p1);
                Vector4d clip2 = FPrincipal.WorldToClip(p2);
                x1 = 0; y1 = 0; x2 = 0; y2 = 0;

                if (!Clipper3D.ClipSegmentoFrustum(ref clip1, ref clip2))
                    return false;
                // Agora transforma os pontos visíveis em NDC

                Vector3d ndc1 = new Vector3d(
                    clip1.X / clip1.W,
                    clip1.Y / clip1.W,
                    clip1.Z / clip1.W);

                Vector3d ndc2 = new Vector3d(
                    clip2.X / clip2.W,
                    clip2.Y / clip2.W,
                    clip2.Z / clip2.W);


                // NDC -> pixels

                 x1 = (ndc1.X * 0.5 + 0.5) * FPrincipal.w;
                 y1 = (1.0 - (ndc1.Y * 0.5 + 0.5)) * FPrincipal.h;
                 x2 = (ndc2.X * 0.5 + 0.5) * FPrincipal.w;
                 y2 = (1.0 - (ndc2.Y * 0.5 + 0.5)) * FPrincipal.h;

                /*x1 = (ndc1.X * 0.5 + 0.5) * FPrincipal.w;
                y1 = (ndc1.Y + 1.0) * 0.5 * FPrincipal.h;
                x2 = (ndc2.X * 0.5 + 0.5) * FPrincipal.w;
                y2 = (ndc2.Y + 1.0) * 0.5 * FPrincipal.h;
*/
                return true;
            }
            else
            {
                x1 = 0; y1 = 0; x2 = 0; y2 = 0; 
               // double z_clip1 = 0, z_clip2 = 0;

                Vector3d pix1, pix2;
                Vector4d clip1, clip2;

                 if (!Project3(p1, vp, new Size(FPrincipal.w, FPrincipal.h), out pix1, out clip1))
                     return false;

                 if (!Project3(p2, vp, new Size(FPrincipal.w, FPrincipal.h), out pix2, out clip2))
                    return false;

              //  FPrincipal.pixel1(ref p1.X, ref p1.Y, ref p1.Z, ref z_clip1);
              //  FPrincipal.pixel2(ref p2.X, ref p2.Y, ref p2.Z, ref z_clip2);

                x1 = pix1.X;
                y1 = pix1.Y;
                x2 = pix2.X;
                y2 = pix2.Y;

              /*  x1 = FPrincipal.px_x1[0];
                y1 = FPrincipal.px_y1[0];
                x2 = FPrincipal.px_x2[0];
                y2 = FPrincipal.px_y2[0];
                */
                return true;

            }
        }

        public static bool ClipSegmentoFrustum(ref Vector4d p0, ref Vector4d p1)
        {
            // totalmente atrás da câmera
            if (p0.W <= 0.0 && p1.W <= 0.0)
                return false;

            t0 = 0.0;
            t1 = 1.0;

            Vector4d d = p1 - p0;

            // Plano esquerdo  : x >= -w
            if (!Clip(p0.W + p0.X, -(d.W + d.X)))
                return false;

            // Plano direito   : x <= +w
            if (!Clip(p0.W - p0.X, -(d.W - d.X)))
                return false;

            // Plano inferior  : y >= -w
            if (!Clip(p0.W + p0.Y, -(d.W + d.Y)))
                return false;

            // Plano superior  : y <= +w
            if (!Clip(p0.W - p0.Y, -(d.W - d.Y)))
                return false;

            // Near
            if (!Clip(p0.Z, -d.Z))
                return false;

            // Far
            if (!Clip(p0.W - p0.Z, -(d.W - d.Z)))
                return false;

            if (t1 < 1.0)
                p1 = p0 + d * t1;

            if (t0 > 0.0)
                p0 = p0 + d * t0;

            return true;
        }

        static bool Clip(double q, double p)
        {
            const double EPS = 1e-12;

            if (Math.Abs(p) < EPS)
                return q >= 0.0;

            double r = q / p;

            if (p < 0.0)
            {
                if (r > t1)
                    return false;

                if (r > t0)
                    t0 = r;
            }
            else
            {
                if (r < t0)
                    return false;

                if (r < t1)
                    t1 = r;
            }

            return true;
        }
    }

    public static class Geometry
    {
        public static double AreaPoligono_Shoealace(List<vec3> pts)
        {
            if (pts == null || pts.Count < 3)
                return 0f;

            double area = 0f;

            for (int i = 0; i < pts.Count; i++)
            {
                vec3 a = pts[i];
                vec3 b = pts[(i + 1) % pts.Count];

                area += (a.x * b.y) - (b.x * a.y);
            }

            return Math.Abs(area) * 0.5;
        }

        public static List<vec3> RetornaContornoOrdenado(List<LinhaVec3> edges)
        {
            if (edges == null || edges.Count == 0)
                return new List<vec3>();

            List<vec3> contour = new List<vec3>();
            bool[] visited = new bool[edges.Count];

            LinhaVec3 first = edges[0];

            vec3 start = first.p1;
            vec3 current = first.p2;
            vec3 previous = first.p1;

            contour.Add(start);
            contour.Add(current);

            visited[0] = true;

            while (true)
            {
                bool found = false;

                for (int i = 0; i < edges.Count; i++)
                {
                    if (visited[i])
                        continue;

                    LinhaVec3 e = edges[i];

                    bool matchP1 = MesmoPonto(e.p1, current);
                    bool matchP2 = MesmoPonto(e.p2, current);

                    if (!matchP1 && !matchP2)
                        continue;

                    vec3 next = matchP1 ? e.p2 : e.p1;

                    // evita voltar para a aresta anterior
                    if (MesmoPonto(next, previous))
                        continue;

                    contour.Add(next);

                    previous = current;
                    current = next;

                    visited[i] = true;
                    found = true;
                    break;
                }

                // fechou o loop
                if (MesmoPonto(current, start))
                    break;

                if (!found)
                    throw new Exception("Loop inválido ou contorno aberto na seção.");
            }

            // remove ponto duplicado de fechamento
            if (MesmoPonto(contour[contour.Count - 1], start))
                contour.RemoveAt(contour.Count - 1);

            // garante anti-horário
            if (!IsCCW(contour))
                contour.Reverse();

            return contour;
        }
        public static bool IsCCW(List<vec3> pts)
        {
            double area = 0f;

            for (int i = 0; i < pts.Count; i++)
            {
                vec3 a = pts[i];
                vec3 b = pts[(i + 1) % pts.Count];

                area += (a.x * b.y) - (b.x * a.y);
            }

            return area > 0f;
        }

        const float EPS = 1e-3f;
        public static bool MesmoPonto(vec3 a, vec3 b)
        {
            return
                Math.Abs(a.x - b.x) < EPS &&
                Math.Abs(a.y - b.y) < EPS &&
                Math.Abs(a.z - b.z) < EPS;
        }

        public static List<vec3> SortCounterClockwise(List<vec3> points)
        {
            if (points == null || points.Count < 3)
                return points;

            // centroide
            double cx = 0f;
            double cy = 0f;

            foreach (var p in points)
            {
                cx += p.x;
                cy += p.y;
            }

            cx /= points.Count;
            cy /= points.Count;

            // ordenar anti-horário
            return points
                .OrderBy(p => Math.Atan2(p.y - cy, p.x - cx))
                .ToList();
        }

        public static (vec3, vec3) OffsetLine(
            vec3 p1,
            vec3 p2,
            double offset)
        {
            double dx = p2.x - p1.x;
            double dy = p2.y - p1.y;

            double len = Math.Sqrt(dx * dx + dy * dy);

            // vetor perpendicular unitário
            double nx = -dy / len;
            double ny = dx / len;

            vec3 p1Offset = new vec3(
                p1.x + nx * offset,
                p1.y + ny * offset,
                p1.z);

            vec3 p2Offset = new vec3(
                p2.x + nx * offset,
                p2.y + ny * offset,
                p2.z);

            return (p1Offset, p2Offset);
        }
        public static vec3 CriarPontoOrtogonalApartirDoPonto2(
        vec3 p1,
        vec3 p2,
        double length)
        {
            double dx = p2.x - p1.x;
            double dy = p2.y - p1.y;

            double len = Math.Sqrt(dx * dx + dy * dy);

            // vetor perpendicular normalizado
            double nx = -dy / len;
            double ny = dx / len;

            // novo ponto
            return new vec3(
                p2.x + nx * length,
                p2.y + ny * length,
                0);
        }

        public static void CriarMarcacoesDiagonaisCota(
      vec3 p1,
      vec3 p2,
      double tickSize,
      out (vec3 a, vec3 b) tick1,
      out (vec3 a, vec3 b) tick2)
        {
            double dx = p2.x - p1.x;
            double dy = p2.y - p1.y;

            double len = Math.Sqrt(dx * dx + dy * dy);

            // direção da linha
            double ux = dx / len;
            double uy = dy / len;

            // perpendicular
            double px = -uy;
            double py = ux;

            // diagonal (45° aproximado)
            double tx = ux + px;
            double ty = uy + py;

            double tlen = Math.Sqrt(tx * tx + ty * ty);

            tx /= tlen;
            ty /= tlen;

            double half = tickSize * 0.5;

            // tick no início
            vec3 t1a = new vec3(
                p1.x - tx * half,
                p1.y - ty * half,
                p1.z);

            vec3 t1b = new vec3(
                p1.x + tx * half,
                p1.y + ty * half,
                p1.z);

            // tick no fim
            vec3 t2a = new vec3(
                p2.x - tx * half,
                p2.y - ty * half,
                p2.z);

            vec3 t2b = new vec3(
                p2.x + tx * half,
                p2.y + ty * half,
                p2.z);

            tick1 = (t1a, t1b);
            tick2 = (t2a, t2b);
        }

        public static void CriarMarcacoesCota(
            vec3 p1,
            vec3 p2,
            double tickSize,
            out (vec3 a, vec3 b) tick1,
            out (vec3 a, vec3 b) tick2)
        {
            double dx = p2.x - p1.x;
            double dy = p2.y - p1.y;

            double len = Math.Sqrt(dx * dx + dy * dy);

            // perpendicular unitária
            double nx = -dy / len;
            double ny = dx / len;

            double half = tickSize * 0.5;

            // tick em p1
            vec3 t1a = new vec3(
                p1.x - nx * half,
                p1.y - ny * half,
                p1.z);

            vec3 t1b = new vec3(
                p1.x + nx * half,
                p1.y + ny * half,
                p1.z);

            // tick em p2
            vec3 t2a = new vec3(
                p2.x - nx * half,
                p2.y - ny * half,
                p2.z);

            vec3 t2b = new vec3(
                p2.x + nx * half,
                p2.y + ny * half,
                p2.z);

            tick1 = (t1a, t1b);
            tick2 = (t2a, t2b);
        }
    }
    public struct Ponto
    {
        public float x, y;

        public Ponto(float x, float y)
        {
            this.x = x;
            this.y = y;
        }
    }
    [Serializable]
    public struct LinhaVec3
    {
        public vec3 p1, p2;
        public int raio, id_aresta;
        //usado para tubos por exemplo, para depois nao misturar os pontos na hora de triangular a seção na renderizacao
        public bool contornoInterno, linhaAuxiliarSeparadora;
        public int id_barra, tipo_barra;

        public LinhaVec3(int _id_aresta)
        {
            p1 = null;
            p2 = null;
            contornoInterno = false;
            raio = -1;
            linhaAuxiliarSeparadora = false;
            id_aresta = _id_aresta;
            id_barra = -1;
            tipo_barra = -1;
        }

        public LinhaVec3(vec3 _p1, vec3 _p2, bool cont_interno = false, int _raio = -1, bool _linhaAuxiliarSeparadora = false, int _id_aresta = -1, int id_b =-1, int tipo_b = -1)
        {
            p1 = _p1;
            p2 = _p2;
            contornoInterno = cont_interno;
            raio = _raio;
            linhaAuxiliarSeparadora = _linhaAuxiliarSeparadora;
            id_aresta = _id_aresta;
            id_barra = id_b;
            tipo_barra = tipo_b;
        }

        public void Copy(LinhaVec3 obj)
        {
            this.p1 = new vec3(obj.p1.x, obj.p1.y, obj.p1.z);
            this.p2 = new vec3(obj.p2.x, obj.p2.y, obj.p2.z);
        }

        public LinhaVec3 Clone()
        {
            LinhaVec3 s = new LinhaVec3();
            s.Copy(this);
            return s;
        }


    }
    public struct Seta
    {
        public LinhaVec3 l_principal, l1, l2, l3, l4, l5, l6;
        public Seta(LinhaVec3 _l_principal, LinhaVec3 _l1, LinhaVec3 _l2, LinhaVec3 _l3, LinhaVec3 _l4, LinhaVec3 _l5, LinhaVec3 _l6)
        {
            l_principal = _l_principal;
            l1 = _l1;
            l2 = _l2;
            l3 = _l3;   
            l4 = _l4;   
            l5 = _l5;
            l6 = _l6;
        }

        public void Copy(Seta obj)
        {
            if (obj.l_principal.p1 != null)
            this.l_principal = obj.l_principal.Clone();
        }

        public Seta Clone()
        {
            Seta s = new Seta();
            s.Copy(this);
            return s;
        }
    }
    public struct SetaMomento
    {
        public LinhaVec3 l_principal, l1, l2, l3, l4, l5, l6, 
                                      l7, l8, l9, l10, l11, l12;
        public SetaMomento(LinhaVec3 _l_principal, LinhaVec3 _l1, LinhaVec3 _l2, LinhaVec3 _l3, LinhaVec3 _l4, LinhaVec3 _l5, LinhaVec3 _l6,
            LinhaVec3 _l7, LinhaVec3 _l8, LinhaVec3 _l9, LinhaVec3 _l10, LinhaVec3 _l11, LinhaVec3 _l12)
        {
            l_principal = _l_principal;
            l1 = _l1;
            l2 = _l2;
            l3 = _l3;
            l4 = _l4;
            l5 = _l5;
            l6 = _l6;
            l7 = _l7;
            l8 = _l8;
            l9 = _l9;
            l10 = _l10;
            l11 = _l11;
            l12 = _l12;
        }

        public void Copy(SetaMomento obj)
        {
            if (obj.l_principal.p1 != null)
                this.l_principal = obj.l_principal.Clone();
        }

        public SetaMomento Clone()
        {
            SetaMomento s = new SetaMomento();
            s.Copy(this);
            return s;
        }
    }
    public struct LinhasIsovalor
    {
        public List<Linha> linhas;
        public double valor;
        public LinhasIsovalor(double val)
        {
            linhas = new List<Linha>();
            valor = val;
        }
    }

    [Serializable]
    public struct Linha
    {
        public AnyPt pIni, pFin;
        public double angulo;
        public Linha(AnyPt pIni, AnyPt pFin)
        {
            this.pIni = pIni;
            this.pFin = pFin;
            this.angulo = ((float)(FuncoesGerais.atand((pIni.y - pFin.y) / (pIni.x - pFin.x))));
        }
        public Linha(double xi, double yi, double xf, double yf)
        {
            pIni = new AnyPt(xi, yi,false);
            pFin = new AnyPt(xf, yf, false);
            this.angulo = ((float)(FuncoesGerais.atand((pIni.y - pFin.y) / (pIni.x - pFin.x))));
        }

        public void Intersec2(Linha outra, ref double x, ref double y)
        {
            if (Geom.calcIntersecEQU_RETA(this.pIni.x, this.pIni.y, this.pFin.x, this.pFin.y,
                                                        outra.pIni.x, outra.pIni.y, outra.pFin.x, outra.pFin.y, ref x, ref y))
            {
                
            }

        }

        public bool Intersec3(double x1, double y1, double x2, double y2, ref double x, ref double y)
        {
            return (Geom.calcIntersecEQU_RETA(this.pIni.x, this.pIni.y, this.pFin.x, this.pFin.y,
                                                        x1, y1, x2, y2, ref x, ref y));

        }

        public bool Intersec(Linha outra, ref double x, ref double y)
        {
            return (Geom.calcIntersecEQU_RETA(this.pIni.x, this.pIni.y, this.pFin.x, this.pFin.y,
                                                        outra.pIni.x, outra.pIni.y, outra.pFin.x, outra.pFin.y, ref x, ref y));

        }

        public bool Intersec(TLinha outra, ref double x, ref double y)
        {
            return (Geom.calcIntersecEQU_RETA(this.pIni.x, this.pIni.y, this.pFin.x, this.pFin.y,
                                                        outra.pIni.x, outra.pIni.y, outra.pFin.x, outra.pFin.y, ref x, ref y));

        }
        public AnyPt getMiddlePoint()
        {
            return (pIni + pFin) / 2;
        }
        public bool Intersec(double x1, double y1, double x2, double y2, ref double x, ref double y)
        {
            return (Geom.calcIntersecEQU_RETA(this.pIni.x, this.pIni.y, this.pFin.x, this.pFin.y,
                                                        x1, y1, x2, y2, ref x, ref y));

        }
        public bool PontoEmLinha(double p1, double p2)
        {
           return  (Geom.PontoEmLinha(p1,p2, this.pIni.x, this.pIni.y,this.pFin.x, this.pFin.y));
        }
    }


    [Serializable]
    public struct PontoD
    {
        public double x, y, z;

        public PontoD(double x, double y, double z)
        {
            this.x = x;
            this.y = y;
            this.z = z;
        }
    }
    public struct PontoAux
    {
        public double x, y;
        public TBarraGrelha b;
        public PontoAux(double x, double y, TBarraGrelha bb = null)
        {
            this.x = x;
            this.y = y;
            b = bb;
        }
    }

    [Serializable]
    public struct AnyPt
    {
        public double x, y;
        public bool continua;
        public AnyPt(double x, double y, bool cont)
        {
            this.x = x;
            this.y = y;
            continua = cont;
        }

        public static AnyPt operator -(AnyPt lhs, AnyPt rhs)
        {
            return new AnyPt(lhs.x - rhs.x, lhs.y - rhs.y, true);
        }

        public static AnyPt operator -(AnyPt lhs, double rhs)
        {
            return new AnyPt(lhs.x - rhs, lhs.y - rhs, true);
        }
        public static bool operator ==(AnyPt a, AnyPt b)
        {
            if ((Object)a == null) 
                throw new Exception("Ponto ''a'' nulo.");
            
            return Geom.Iguais(a.x, b.x, 0.01f) && Geom.Iguais(a.y, b.y, 0.01f);
        }
        public static bool operator !=(AnyPt a, AnyPt b)
        {
            return (!Geom.Iguais(a.x, b.x, 0.01f)) || (!Geom.Iguais(a.y, b.y, 0.01f));
        }
        public static AnyPt operator +(AnyPt lhs, AnyPt rhs)
        {
            return new AnyPt(lhs.x + rhs.x, lhs.y + rhs.y, false);
        }
        public static AnyPt operator /(AnyPt lhs, double rhs)
        {
            return new AnyPt(lhs.x / rhs, lhs.y / rhs, false);
        }
        public static AnyPt operator +(AnyPt lhs, double rhs)
        {
            return new AnyPt(lhs.x + rhs, lhs.y + rhs, false);
        }
        public double Magnitude()
        {
            return Math.Sqrt(x * x + y * y);
        }

        public double DistanceTo(AnyPt v)
        {
            return (this - v).Magnitude();

        }
    }

    [Serializable]
    public struct barra
    {
        public double xi,yi,xf,yf;

        public barra(double xi, double yi, double xf, double yf)
        {
            this.xi = xi;
            this.yi = yi;
            this.xf = xf;
            this.yf = yf;
        }
    }
    public static class Geom
    {
        static double cx, cy, cz;
        static double angXY, angXZ;
        static double[,] mRot = new double[4, 4];
        static double[,] mTrans = new double[5, 5];
        static double[] vecPosicao = new double[5]; 
        public static void rotZ(double a, ref double[] pos, ref double[] posFinal)
        {
            mRot[1, 1] = Math.Cos(RMath.deg2rad(a));
            mRot[1, 2] = Math.Sin(RMath.deg2rad(a));
            mRot[1, 3] = 0;

            mRot[2, 1] = -Math.Sin(RMath.deg2rad(a));
            mRot[2, 2] = Math.Cos(RMath.deg2rad(a));
            mRot[2, 3] = 0;

            mRot[3, 1] = 0;
            mRot[3, 2] = 0;
            mRot[3, 3] = 1;

            TAlgebra.Multiplica_Matriz_Vetor(ref mRot, ref pos, ref posFinal, 3, 3);
        }

        public struct poligono
        {
            public vec3[] vertices;
            int qtd;
            public poligono(int q)
            {
                qtd = q;
                vertices = new vec3[q];

                for (int i = 0; i < q; i++)
                {
                    vertices[i] = new vec3(0);
                }
            }
        }
        public struct plano
        {
            public vec3[] vertices;
            int qtd;


            public plano(int q)
            {
                qtd = q;
                vertices = new vec3[q];

                for (int i = 0; i < q; i++)
                {
                    vertices[i] = new vec3(0);
                }
            }
        }
                                                                      // 'posicao' é um ponto qualquer que deve estar contido no plano
        public static void EspelhaVetor(vec3 normal, vec3 ponto, vec3 posicao)
        {

            // aqui é um algoritmo de reflexão....pesquisar sobre reflexão de vetores
            vec3 v = ponto;

            double vdotnxn1 = v.DotProduct(normal);
            vec3 res = vdotnxn1 * normal;
            vec3 r = -2 * res + v;

            vec3 vetorPontoAoPlano = new vec3(0);
            double distanciaPontoZero_ao_Plano = Geom.DistanciaPontoPlano(normal, new vec3(0, 0, 0), posicao, ref vetorPontoAoPlano);

            ponto.x = r.x + (2 * vetorPontoAoPlano.x);
            ponto.y = r.y + (2 * vetorPontoAoPlano.y);
            ponto.z = r.z + (2 * vetorPontoAoPlano.z);

            ponto.z *= -1;
            ponto.y *= -1;

            if (Geom.Iguais(ponto.x, 0))
                ponto.x = 0;
            if (Geom.Iguais(ponto.y, 0))
                ponto.y = 0;
            if (Geom.Iguais(ponto.z, 0))
                ponto.z = 0;

        }

        //algoritmo de Rodriguez para rotacionar um vetor no espaço (Rodriguez's Formula) 
        public static void RotacionaVetor(ref vec3 ponto, double ang, vec3 pivo, vec3 vecRotacao, vec3 p1_eixorotacao)
        {
            vec3 p_ = new vec3(ponto.x, ponto.y, ponto.z);
            p_.x -= pivo.x;
            p_.y -= pivo.y;
            p_.z -= pivo.z;

            vec3 ponto_Rodar = p_ - p1_eixorotacao;

            vec3 d = vecRotacao.DotProduct(ponto_Rodar) * vecRotacao;
            vec3 r = ponto_Rodar - d;

            double teta = ang * Const.PIDiv180;// RMath.deg2rad(ang);

            vec3 r2 = r * (Math.Cos(teta)) + (vecRotacao.CrossProduct(r) * Math.Sin(teta));
            vec3 p3 = d + r2;

            p3 += pivo;

            ponto.x = p3.x;
            ponto.y = -p3.y;
            ponto.z = -p3.z;

            if (Geom.Iguais(ponto.x, 0))
                ponto.x = 0;
            if (Geom.Iguais(ponto.y, 0))
                ponto.y = 0;
            if (Geom.Iguais(ponto.z, 0))
                ponto.z = 0;
        }

        public static double DistanciaPontoPlano(vec3 normalPlano, vec3 ponto, vec3 posicao, ref vec3 vetorPontoAoPlano)
        {
            vec3 v = ponto - posicao;
            double distancia = Math.Abs( v.DotProduct(normalPlano));

            double alfaX = Math.Acos(Math.Abs(normalPlano.x));
            double alfaY = Math.Acos(Math.Abs(normalPlano.y));
            double alfaZ = Math.Acos(Math.Abs(normalPlano.z));

            double sinalX = 1;
            if (!Geom.Iguais(normalPlano.x , 0))
                if (normalPlano.x < 0)
                  sinalX = -1;

            double sinalY = 1;
            if (!Geom.Iguais(normalPlano.y, 0))
                if (normalPlano.y < 0)
                    sinalY = -1;
            
            double sinalZ = 1;
            if (!Geom.Iguais(normalPlano.z, 0))
                if (normalPlano.z < 0)
                    sinalZ = -1;

            vec3 pontoPlano = new vec3(ponto.x + (Math.Cos(alfaX) * distancia * sinalX),
                                       ponto.y + (Math.Cos(alfaY) * distancia * sinalY),
                                       ponto.z + (Math.Cos(alfaZ) * distancia * sinalZ));

            vetorPontoAoPlano = pontoPlano - ponto;
          //  vetorPontoAoPlano *= -1;
         //   vetorPontoAoPlano.x *= -1;

            return distancia;
        }

        public static Seta CriarSetaEixoGlobalY(double tamanho, TPonto posicao)
        {
            double tamSetaXHoriz = 0.07 * tamanho;
            double tamSetaVert = 0.3 * tamanho;
            LinhaVec3 l_principal;
            vec3 p2_l1, p2_l2, p2_l3, p2_l4;
            tamanho *= -1;

            l_principal = new LinhaVec3(new vec3(posicao.x, posicao.y, posicao.z), new vec3(posicao.x, posicao.y + tamanho, posicao.z));

            p2_l1 = new vec3(l_principal.p2.x + tamSetaXHoriz, l_principal.p2.y + tamSetaVert, l_principal.p2.z + tamSetaXHoriz);
            p2_l2 = new vec3(l_principal.p2.x - tamSetaXHoriz, l_principal.p2.y + tamSetaVert, l_principal.p2.z + tamSetaXHoriz);
            p2_l3 = new vec3(l_principal.p2.x - tamSetaXHoriz, l_principal.p2.y + tamSetaVert, l_principal.p2.z - tamSetaXHoriz);
            p2_l4 = new vec3(l_principal.p2.x + tamSetaXHoriz, l_principal.p2.y + tamSetaVert, l_principal.p2.z - tamSetaXHoriz);

            LinhaVec3 l1 = new LinhaVec3(new vec3(l_principal.p2.x, l_principal.p2.y, l_principal.p2.z), p2_l1);
            LinhaVec3 l2 = new LinhaVec3(new vec3(l_principal.p2.x, l_principal.p2.y, l_principal.p2.z), p2_l2);
            LinhaVec3 l3 = new LinhaVec3(new vec3(l_principal.p2.x, l_principal.p2.y, l_principal.p2.z), p2_l3);
            LinhaVec3 l4 = new LinhaVec3(new vec3(l_principal.p2.x, l_principal.p2.y, l_principal.p2.z), p2_l4);
            LinhaVec3 l5 = new LinhaVec3(l1.p2, l3.p2);
            LinhaVec3 l6 = new LinhaVec3(l2.p2, l4.p2);

            return new Seta(l_principal, l1, l2, l3, l4, l5, l6);
        }
        public static Seta CriarSetaEixoLocalY(double tamanho, double local, TPonto pIni, TPonto pFin, double alfa)
        {
            double comprimento = (Math.Sqrt(Math.Pow(pIni.x - pFin.x, 2) + Math.Pow(pIni.y - pFin.y, 2) + Math.Pow(pIni.z - pFin.z, 2)));
            double tamSetaXHoriz = 0.07 * tamanho;
            double tamSetaVert = 0.3 * tamanho;

            double cx = (((pFin.x - pIni.x)) / comprimento);
            double cy = ((((pFin.y * -1) - (pIni.y * -1))) / comprimento);

            if (Geom.Iguais(cx, 0))
                cx = 0;
            if (Geom.Iguais(cy, 0))
                cy = 0;
            double xf, yf, zf;
            LinhaVec3 l_principal;
            string eixoRodar;
            double ang = alfa;
            vec3 p2_l1, p2_l2, p2_l3, p2_l4;
            if (Geom.Iguais(cx, 0) && Geom.Iguais(cy, 0))
            {
                if (pIni.z * -1 < pFin.z * -1)
                    tamanho *= -1;

                xf = local;
                yf = 0;
                zf = -tamanho;

                l_principal = new LinhaVec3(new vec3(local, 0, 0), new vec3(xf, yf, zf));
                eixoRodar = "Z";

                if (pIni.z * -1 < pFin.z * -1)
                {
                    tamSetaVert *= -1;
                    tamSetaXHoriz *= -1;
                }

                p2_l1 = new vec3(l_principal.p2.x + tamSetaXHoriz, l_principal.p2.y + tamSetaXHoriz, l_principal.p2.z + tamSetaVert);
                p2_l2 = new vec3(l_principal.p2.x - tamSetaXHoriz, l_principal.p2.y + tamSetaXHoriz, l_principal.p2.z + tamSetaVert);
                p2_l3 = new vec3(l_principal.p2.x - tamSetaXHoriz, l_principal.p2.y - tamSetaXHoriz, l_principal.p2.z + tamSetaVert);
                p2_l4 = new vec3(l_principal.p2.x + tamSetaXHoriz, l_principal.p2.y - tamSetaXHoriz, l_principal.p2.z + tamSetaVert);
            }
            else
            {
                if (Geom.Iguais(pIni.x, pFin.x))
                    tamanho *= -1;
                else
                if (pIni.x < pFin.x)
                {
                    tamanho *= -1;
                }
                else
                if (pIni.x > pFin.x)
                {
                    local *= -1;
                    ang *= -1;

                    tamSetaVert *= -1;
                    tamSetaXHoriz *= -1;
                }

                xf = local;
                yf = tamanho;
                zf = 0;

                l_principal = new LinhaVec3(new vec3(local, 0, 0), new vec3(xf, yf, zf));
                eixoRodar = "Y";

                p2_l1 = new vec3(l_principal.p2.x + tamSetaXHoriz, l_principal.p2.y + tamSetaVert, l_principal.p2.z + tamSetaXHoriz);
                p2_l2 = new vec3(l_principal.p2.x - tamSetaXHoriz, l_principal.p2.y + tamSetaVert, l_principal.p2.z + tamSetaXHoriz);
                p2_l3 = new vec3(l_principal.p2.x - tamSetaXHoriz, l_principal.p2.y + tamSetaVert, l_principal.p2.z - tamSetaXHoriz);
                p2_l4 = new vec3(l_principal.p2.x + tamSetaXHoriz, l_principal.p2.y + tamSetaVert, l_principal.p2.z - tamSetaXHoriz);
            }

            LinhaVec3 l1 = new LinhaVec3(new vec3(l_principal.p2.x, l_principal.p2.y, l_principal.p2.z), p2_l1);

            LinhaVec3 l2 = new LinhaVec3(new vec3(l_principal.p2.x, l_principal.p2.y, l_principal.p2.z), p2_l2);

            LinhaVec3 l3 = new LinhaVec3(new vec3(l_principal.p2.x, l_principal.p2.y, l_principal.p2.z), p2_l3);

            LinhaVec3 l4 = new LinhaVec3(new vec3(l_principal.p2.x, l_principal.p2.y, l_principal.p2.z), p2_l4);

            Geom.GiraConformeAnguloAlfa(ref l_principal.p2, eixoRodar, ang);

            Geom.RodarSeta(ref l1.p1, eixoRodar, ang);
            Geom.RodarSeta(ref l2.p1, eixoRodar, ang);
            Geom.RodarSeta(ref l3.p1, eixoRodar, ang);
            Geom.RodarSeta(ref l4.p1, eixoRodar, ang);
            Geom.RodarSeta(ref l1.p2, eixoRodar, ang);
            Geom.RodarSeta(ref l2.p2, eixoRodar, ang);
            Geom.RodarSeta(ref l3.p2, eixoRodar, ang);
            Geom.RodarSeta(ref l4.p2, eixoRodar, ang);

            LinhaVec3 lin = new LinhaVec3();

            for (int i = 0; i < 5; i++)
            {
                if (i == 0)
                    lin = l_principal;
                else if (i == 1)
                    lin = l1;
                else if (i == 2)
                    lin = l2;
                else if (i == 3)
                    lin = l3;
                else if (i == 4)
                    lin = l4;
                Geom.RotacionaLinhaNoEspaco(pIni.x, pIni.y, pIni.z, pFin.x, pFin.y, pFin.z, ref lin.p1, ref lin.p2);
            }
            LinhaVec3 l5 = new LinhaVec3(l1.p2, l3.p2);
            LinhaVec3 l6 = new LinhaVec3(l2.p2, l4.p2);

            return new Seta(l_principal, l1, l2, l3, l4, l5, l6);
        }
        public static Seta CriarSetaEixoLocalZ(double tamanho, double local, TPonto pIni, TPonto pFin, double alfa, double offset = 0)
        {
            double tamSetaXHoriz = 0.07 * tamanho;
            double tamSetaVert = 0.3 * tamanho;
            double comprimento = (Math.Sqrt(Math.Pow(pIni.x - pFin.x, 2) + Math.Pow(pIni.y - pFin.y, 2) + Math.Pow(pIni.z - pFin.z, 2)));
            double cx = (((pFin.x - pIni.x)) / comprimento);
            double cy = ((((pFin.y * -1) - (pIni.y * -1))) / comprimento);

            if (Geom.Iguais(cx, 0))
                cx = 0;
            if (Geom.Iguais(cy, 0))
                cy = 0;

            double ang = alfa;
            string eixoRodar;
            double xf, yf, zf;
            LinhaVec3 l_principal;
            vec3 p2_l1, p2_l2, p2_l3, p2_l4;

            if (Geom.Iguais(cx, 0) && Geom.Iguais(cy, 0))
            {
                if (pIni.z * -1 < pFin.z * -1)
                {
                    tamanho *= -1;
                    //   tamSetaVert *= -1;
                    //     tamSetaXHoriz *= -1;
                }

                if (pIni.z * -1 > pFin.z * -1)
                {

                    tamSetaVert *= -1;
                    tamSetaXHoriz *= -1;
                }

                xf = local;
                yf = tamanho;
                zf = 0;

                eixoRodar = "Y";

                l_principal = new LinhaVec3(new vec3(local, offset, 0), new vec3(xf, yf, zf + offset));

                p2_l1 = new vec3(l_principal.p2.x + tamSetaXHoriz, l_principal.p2.y + tamSetaVert, l_principal.p2.z + tamSetaXHoriz);
                p2_l2 = new vec3(l_principal.p2.x - tamSetaXHoriz, l_principal.p2.y + tamSetaVert, l_principal.p2.z + tamSetaXHoriz);
                p2_l3 = new vec3(l_principal.p2.x - tamSetaXHoriz, l_principal.p2.y + tamSetaVert, l_principal.p2.z - tamSetaXHoriz);
                p2_l4 = new vec3(l_principal.p2.x + tamSetaXHoriz, l_principal.p2.y + tamSetaVert, l_principal.p2.z - tamSetaXHoriz);
            }
            else
            {
                tamanho *= -1;

                if (!Geom.Iguais(pIni.x, pFin.x))
                    if (pIni.x > pFin.x)
                    {
                        local *= -1;
                        ang *= -1;
                    }

                xf = local;
                yf = 0;
                zf = tamanho;

                l_principal = new LinhaVec3(new vec3(local, 0, offset), new vec3(xf, yf, zf + offset));

                eixoRodar = "Z";

                p2_l1 = new vec3(l_principal.p2.x + tamSetaXHoriz, l_principal.p2.y + tamSetaXHoriz, l_principal.p2.z + tamSetaVert);
                p2_l2 = new vec3(l_principal.p2.x - tamSetaXHoriz, l_principal.p2.y + tamSetaXHoriz, l_principal.p2.z + tamSetaVert);
                p2_l3 = new vec3(l_principal.p2.x - tamSetaXHoriz, l_principal.p2.y - tamSetaXHoriz, l_principal.p2.z + tamSetaVert);
                p2_l4 = new vec3(l_principal.p2.x + tamSetaXHoriz, l_principal.p2.y - tamSetaXHoriz, l_principal.p2.z + tamSetaVert);
            }

            LinhaVec3 l1 = new LinhaVec3(new vec3(l_principal.p2.x, l_principal.p2.y, l_principal.p2.z), p2_l1);
            LinhaVec3 l2 = new LinhaVec3(new vec3(l_principal.p2.x, l_principal.p2.y, l_principal.p2.z), p2_l2);
            LinhaVec3 l3 = new LinhaVec3(new vec3(l_principal.p2.x, l_principal.p2.y, l_principal.p2.z), p2_l3);
            LinhaVec3 l4 = new LinhaVec3(new vec3(l_principal.p2.x, l_principal.p2.y, l_principal.p2.z), p2_l4);

            Geom.GiraConformeAnguloAlfa(ref l_principal.p2, eixoRodar, ang);
            Geom.RodarSeta(ref l1.p1, eixoRodar, ang);
            Geom.RodarSeta(ref l2.p1, eixoRodar, ang);
            Geom.RodarSeta(ref l3.p1, eixoRodar, ang);
            Geom.RodarSeta(ref l4.p1, eixoRodar, ang);
            Geom.RodarSeta(ref l1.p2, eixoRodar, ang);
            Geom.RodarSeta(ref l2.p2, eixoRodar, ang);
            Geom.RodarSeta(ref l3.p2, eixoRodar, ang);
            Geom.RodarSeta(ref l4.p2, eixoRodar, ang);
            
            LinhaVec3 lin = new LinhaVec3();

            for (int i = 0; i < 5; i++)
            {
                if (i == 0)
                    lin = l_principal;
                else if (i == 1)
                    lin = l1;
                else if (i == 2)
                    lin = l2;
                else if (i == 3)
                    lin = l3;
                else if (i == 4)
                    lin = l4;

                Geom.RotacionaLinhaNoEspaco(pIni.x, pIni.y, pIni.z, pFin.x, pFin.y, pFin.z, ref lin.p1, ref lin.p2);
            }

            LinhaVec3 l5 = new LinhaVec3(l1.p2, l3.p2);
            LinhaVec3 l6 = new LinhaVec3(l2.p2, l4.p2);

            return new Seta(l_principal, l1, l2, l3, l4, l5, l6);
        }
        public static Seta CriarSetaEixoGlobalZ(double tamanho, TPonto posicao, double offset = 0)
        {
            double tamSetaXHoriz = 0.03 * tamanho;
            double tamSetaVert = 0.156 * tamanho;
            LinhaVec3 l_principal;
            vec3 p2_l1, p2_l2, p2_l3, p2_l4;
            tamanho *= -1;

            l_principal = new LinhaVec3(new vec3(posicao.x, posicao.y, posicao.z+offset), new vec3(posicao.x, posicao.y, posicao.z + tamanho + offset));

            p2_l1 = new vec3(l_principal.p2.x + tamSetaXHoriz, l_principal.p2.y + tamSetaXHoriz, l_principal.p2.z + tamSetaVert);
            p2_l2 = new vec3(l_principal.p2.x - tamSetaXHoriz, l_principal.p2.y + tamSetaXHoriz, l_principal.p2.z + tamSetaVert);
            p2_l3 = new vec3(l_principal.p2.x - tamSetaXHoriz, l_principal.p2.y - tamSetaXHoriz, l_principal.p2.z + tamSetaVert);
            p2_l4 = new vec3(l_principal.p2.x + tamSetaXHoriz, l_principal.p2.y - tamSetaXHoriz, l_principal.p2.z + tamSetaVert);         

            LinhaVec3 l1 = new LinhaVec3(new vec3(l_principal.p2.x, l_principal.p2.y, l_principal.p2.z), p2_l1);
            LinhaVec3 l2 = new LinhaVec3(new vec3(l_principal.p2.x, l_principal.p2.y, l_principal.p2.z), p2_l2);
            LinhaVec3 l3 = new LinhaVec3(new vec3(l_principal.p2.x, l_principal.p2.y, l_principal.p2.z), p2_l3);
            LinhaVec3 l4 = new LinhaVec3(new vec3(l_principal.p2.x, l_principal.p2.y, l_principal.p2.z), p2_l4);
            LinhaVec3 l5 = new LinhaVec3(l1.p2, l3.p2);
            LinhaVec3 l6 = new LinhaVec3(l2.p2, l4.p2);

            return new Seta(l_principal, l1, l2, l3, l4, l5, l6);
        }

        public static Seta CriarSetaEixoGlobalX(double tamanho, TPonto posicao)
        {
            double tamSetaXHoriz = 0.07 * tamanho;
            double tamSetaVert = 0.3 * tamanho;
            LinhaVec3 l_principal;
            vec3 p2_l1, p2_l2, p2_l3, p2_l4;
         //   tamanho *= -1;

            l_principal = new LinhaVec3(new vec3(posicao.x, posicao.y, posicao.z), new vec3(posicao.x + tamanho, posicao.y, posicao.z));

            p2_l1 = new vec3(l_principal.p2.x - tamSetaVert,l_principal.p2.y - tamSetaXHoriz,l_principal.p2.z - tamSetaXHoriz);
            p2_l2 = new vec3(l_principal.p2.x - tamSetaVert,l_principal.p2.y - tamSetaXHoriz,l_principal.p2.z + tamSetaXHoriz);
            p2_l3 = new vec3(l_principal.p2.x - tamSetaVert,l_principal.p2.y + tamSetaXHoriz,l_principal.p2.z + tamSetaXHoriz);
            p2_l4 = new vec3(l_principal.p2.x - tamSetaVert,l_principal.p2.y + tamSetaXHoriz,l_principal.p2.z - tamSetaXHoriz);

            LinhaVec3 l1 = new LinhaVec3(new vec3(l_principal.p2.x, l_principal.p2.y, l_principal.p2.z), p2_l1);
            LinhaVec3 l2 = new LinhaVec3(new vec3(l_principal.p2.x, l_principal.p2.y, l_principal.p2.z), p2_l2);
            LinhaVec3 l3 = new LinhaVec3(new vec3(l_principal.p2.x, l_principal.p2.y, l_principal.p2.z), p2_l3);
            LinhaVec3 l4 = new LinhaVec3(new vec3(l_principal.p2.x, l_principal.p2.y, l_principal.p2.z), p2_l4);

            LinhaVec3 l5 = new LinhaVec3(l1.p2, l3.p2);
            LinhaVec3 l6 = new LinhaVec3(l2.p2, l4.p2);

            return new Seta(l_principal, l1, l2, l3, l4, l5, l6);
        }

        public static SetaMomento CriarSetaEixoGlobalX_DUPLA(double tamanho, TPonto posicao, double sentido)
        {
            double tamSetaXHoriz = 0.07 * tamanho;
            double tamSetaVert = 0.3 * tamanho;
            LinhaVec3 l_principal, l_principal_2;
            vec3 p2_l1, p2_l2, p2_l3, p2_l4, p2_l7, p2_l8, p2_l9, p2_l10, p2_l11, p2_l12;

            l_principal = new LinhaVec3(new vec3(posicao.x, posicao.y, posicao.z), new vec3(posicao.x + tamanho, posicao.y, posicao.z));
          
            l_principal_2 = new LinhaVec3(new vec3(posicao.x + (0.3 * sentido * -1), posicao.y, posicao.z), new vec3(posicao.x + tamanho - 0.3, posicao.y, posicao.z));

            p2_l1 = new vec3(l_principal.p2.x - tamSetaVert, l_principal.p2.y - tamSetaXHoriz, l_principal.p2.z - tamSetaXHoriz);
            p2_l2 = new vec3(l_principal.p2.x - tamSetaVert, l_principal.p2.y - tamSetaXHoriz, l_principal.p2.z + tamSetaXHoriz);
            p2_l3 = new vec3(l_principal.p2.x - tamSetaVert, l_principal.p2.y + tamSetaXHoriz, l_principal.p2.z + tamSetaXHoriz);
            p2_l4 = new vec3(l_principal.p2.x - tamSetaVert, l_principal.p2.y + tamSetaXHoriz, l_principal.p2.z - tamSetaXHoriz);

            LinhaVec3 l1 = new LinhaVec3(new vec3(l_principal.p2.x, l_principal.p2.y, l_principal.p2.z), p2_l1);
            LinhaVec3 l2 = new LinhaVec3(new vec3(l_principal.p2.x, l_principal.p2.y, l_principal.p2.z), p2_l2);
            LinhaVec3 l3 = new LinhaVec3(new vec3(l_principal.p2.x, l_principal.p2.y, l_principal.p2.z), p2_l3);
            LinhaVec3 l4 = new LinhaVec3(new vec3(l_principal.p2.x, l_principal.p2.y, l_principal.p2.z), p2_l4);

            LinhaVec3 l5 = new LinhaVec3(l1.p2, l3.p2);
            LinhaVec3 l6 = new LinhaVec3(l2.p2, l4.p2);

            ////////////////segunda seta
            p2_l7  = new vec3(l_principal_2.p2.x - tamSetaVert, l_principal_2.p2.y - tamSetaXHoriz, l_principal_2.p2.z - tamSetaXHoriz);
            p2_l8  = new vec3(l_principal_2.p2.x - tamSetaVert, l_principal_2.p2.y - tamSetaXHoriz, l_principal_2.p2.z + tamSetaXHoriz);
            p2_l9  = new vec3(l_principal_2.p2.x - tamSetaVert, l_principal_2.p2.y + tamSetaXHoriz, l_principal_2.p2.z + tamSetaXHoriz);
            p2_l10 = new vec3(l_principal_2.p2.x - tamSetaVert, l_principal_2.p2.y + tamSetaXHoriz, l_principal_2.p2.z - tamSetaXHoriz);

            LinhaVec3 l7 = new LinhaVec3(new vec3(l_principal_2.p2.x, l_principal_2.p2.y, l_principal_2.p2.z), p2_l7);
            LinhaVec3 l8 = new LinhaVec3(new vec3(l_principal_2.p2.x, l_principal_2.p2.y, l_principal_2.p2.z), p2_l8);
            LinhaVec3 l9 = new LinhaVec3(new vec3(l_principal_2.p2.x, l_principal_2.p2.y, l_principal_2.p2.z), p2_l9);
            LinhaVec3 l10 = new LinhaVec3(new vec3(l_principal_2.p2.x, l_principal_2.p2.y, l_principal_2.p2.z), p2_l10);

            LinhaVec3 l11 = new LinhaVec3(l7.p2, l9.p2);
            LinhaVec3 l12 = new LinhaVec3(l7.p2, l10.p2);

            return new SetaMomento(l_principal, l1, l2, l3, l4, l5, l6, l7, l8, l9, l10, l11, l12);
        }


        public static Seta CriarSetaEixoLocalX(double tamanho, double local, TPonto pIni, TPonto pFin, double alfa)
        {
          //  tamanho = 0.05 * comprimento;
          //  local = 0.5 * comprimento;
            double tamSetaXHoriz = 0.07 * tamanho;
            double tamSetaVert = 0.3 * tamanho;
            double comprimento = (Math.Sqrt(Math.Pow(pIni.x - pFin.x, 2) + Math.Pow(pIni.y - pFin.y, 2) + Math.Pow(pIni.z - pFin.z, 2)));

            double cx = (((pFin.x - pIni.x)) / comprimento);
            double cy = ((((pFin.y * -1) - (pIni.y * -1))) / comprimento);
            double cz = ((((pFin.z * -1) - (pIni.z * -1))) / comprimento);

            if (!Geom.Iguais(pIni.x, pFin.x))
            {
                if (pIni.x > pFin.x)
                {
                    local *= -1;
                    tamanho *= -1;

                    tamSetaVert *= -1;
                    tamSetaXHoriz *= -1;
                }
            }

            LinhaVec3 l_principal = new LinhaVec3(new vec3(local, 0, 0), new vec3(local + tamanho, 0, 0));
            //    Geom.GiraConformeAnguloAlfa(ref l_principal.p1, "X", Dados.anguloRotacao);
            Geom.GiraConformeAnguloAlfa(ref l_principal.p2, "X", alfa);

            LinhaVec3 l1 = new LinhaVec3(new vec3(l_principal.p2.x, l_principal.p2.y, l_principal.p2.z),
                                         new vec3(l_principal.p2.x - tamSetaVert,
                                                  l_principal.p2.y - tamSetaXHoriz,
                                                  l_principal.p2.z - tamSetaXHoriz));

            LinhaVec3 l2 = new LinhaVec3(new vec3(l_principal.p2.x, l_principal.p2.y, l_principal.p2.z),
                                         new vec3(l_principal.p2.x - tamSetaVert,
                                         l_principal.p2.y - tamSetaXHoriz,
                                         l_principal.p2.z + tamSetaXHoriz));

            LinhaVec3 l3 = new LinhaVec3(new vec3(l_principal.p2.x, l_principal.p2.y, l_principal.p2.z),
                             new vec3(l_principal.p2.x - tamSetaVert,
                             l_principal.p2.y + tamSetaXHoriz,
                             l_principal.p2.z + tamSetaXHoriz));

            LinhaVec3 l4 = new LinhaVec3(new vec3(l_principal.p2.x, l_principal.p2.y, l_principal.p2.z),
                             new vec3(
                             l_principal.p2.x - tamSetaVert,
                             l_principal.p2.y + tamSetaXHoriz,
                             l_principal.p2.z - tamSetaXHoriz));

            Geom.RodarSeta(ref l1.p2, "X", alfa);
            Geom.RodarSeta(ref l2.p2, "X", alfa);
            Geom.RodarSeta(ref l3.p2, "X", alfa);
            Geom.RodarSeta(ref l4.p2, "X", alfa);

            LinhaVec3 lin = new LinhaVec3();

            for (int i = 0; i < 5; i++)
            {
                if (i == 0)
                    lin = l_principal;
                else if (i == 1)
                    lin = l1;
                else if (i == 2)
                    lin = l2;
                else if (i == 3)
                    lin = l3;
                else if (i == 4)
                    lin = l4;

                Geom.RotacionaLinhaNoEspaco(pIni.x, pIni.y, pIni.z, pFin.x, pFin.y, pFin.z, ref lin.p1, ref lin.p2);
            }

            LinhaVec3 l5 = new LinhaVec3(l1.p2, l3.p2);
            LinhaVec3 l6 = new LinhaVec3(l2.p2, l4.p2);

            return new Seta(l_principal, l1, l2, l3, l4, l5, l6);
        }


        public static void RodarSeta(ref vec3 coord, string eixo, double anguloRotacao)
        {
            vec3 vetorRotacao = new vec3(0);
            vec3 centroRotacao = new vec3(0);
            // centroRotacao.x = coord.x;
            //      centroRotacao.y = coord.y;
            // centroRotacao.z = coord.z;

            if (eixo == "Z")
            {
                vetorRotacao = new vec3(coord.y, coord.z, 0);
                vetorRotacao = vetorRotacao.Rotate(centroRotacao, (anguloRotacao) * Const.PIDiv180);
                coord.y = vetorRotacao.x;
                coord.z = vetorRotacao.y;
            }

            if (eixo == "Y")
            {
                vetorRotacao = new vec3(coord.y, coord.z, 0);
                vetorRotacao = vetorRotacao.Rotate(centroRotacao, (anguloRotacao) * Const.PIDiv180);
                coord.y = vetorRotacao.x;
                coord.z = vetorRotacao.y;
            }

            if (eixo == "X")
            {
                vetorRotacao = new vec3(coord.y, coord.z, 0);
                vetorRotacao = vetorRotacao.Rotate(centroRotacao, (-anguloRotacao) * Const.PIDiv180);
                coord.y = vetorRotacao.x;
                coord.z = vetorRotacao.y;
            }
        }
        public static void GiraConformeAnguloAlfa(ref vec3 coord, string eixo, double anguloRotacao)
        {
            vec3 vetorRotacao = new vec3(0);
            vec3 centroRotacao = new vec3(0);
            if (eixo == "Z")
            {
              //  centroRotacao.z = coord.z;
                vetorRotacao = new vec3(0, coord.z, 0);
                vetorRotacao = vetorRotacao.Rotate(centroRotacao, (anguloRotacao) * Const.PIDiv180);
                coord.y = vetorRotacao.x;
                coord.z = vetorRotacao.y;
            }
            else
            if (eixo == "Y")
            {
                vetorRotacao = new vec3(coord.y, 0, 0);
                vetorRotacao = vetorRotacao.Rotate(centroRotacao, (anguloRotacao) * Const.PIDiv180);
                coord.y = vetorRotacao.x;
                coord.z = vetorRotacao.y;
            }
            else
            if (eixo == "X")
            {
                vetorRotacao = new vec3(coord.y, 0, 0);
                vetorRotacao = vetorRotacao.Rotate(centroRotacao, (anguloRotacao) * Const.PIDiv180);
                coord.y = vetorRotacao.x;
                coord.z = vetorRotacao.y;
            }
        }

        public static void RotacionaLinhaNoEspaco(double xi, double yi, double zi, double xf, double yf, double zf, ref vec3 pIni, ref vec3 pFin)
        {
            /* - faço translação da barra para o ponto zero usando tx ty  tz
               - encontro os angulos com os planos xy e xz
               - crio a barra no eixo global x, a partir do ponto zero ( x=0 y=0 z=0) e faço a subdivisao nos tamanhos
                  que eu quero criando a lista de pontos nesse eixo global x
               - rotaciono esses pontos em torno do ponto zero de acordo com os angulos que achei nos planos xy e xz
               - faço a translação dos pontos com sinal contrário ( -tx -ty -tz)  para voltar para a posição original 
             */

            try
            {
                if (Geom.Iguais(xi, 0))
                    xi = 0;
                if (Geom.Iguais(yi, 0))
                    yi = 0;
                if (Geom.Iguais(zi, 0))
                    zi = 0;

                if (Geom.Iguais(xf, 0))
                    xf = 0;
                if (Geom.Iguais(yf, 0))
                    yf = 0;
                if (Geom.Iguais(zf, 0))
                    zf = 0;
                /*---------------*/
                vec3 u1, u2, u, normxy, normxz;
                double NdotU, ndotu_mod, cos_alfa, L, angXY, angXZ, divisoes, divisoesFrac, xAnt;
                int divint;
                List<vec3> CoordsSubdvisao = new List<vec3>();
                double[] posicao = new double[4];
                double[] posicaoFinal = new double[4];
                double[] posicaoFinal2 = new double[4];
                double[] posicaoFinal_Trans = new double[5];

                double tx, ty, tz;
                bool zi_maior_que_zf, xi_igual_xf;
                vec3 pinicial = new vec3(0);
                vec3 pfinal = new vec3(0);
                /*---------------*/

                L = (Math.Sqrt(Math.Pow(xi - xf, 2) + Math.Pow(yi - yf, 2) + Math.Pow(zi - zf, 2)));          
                zi_maior_que_zf = false;
                zi_maior_que_zf = ((zi * -1) > (zf * -1));

                xi_igual_xf = Geom.Iguais(xi, xf);
                pinicial.x = xi; pinicial.y = yi; pinicial.z = zi;
                pfinal.x = xi; pfinal.y = yi; pfinal.z = zi;

                tx = xi;
                ty = yi;
                tz = zi;

                xi -= tx;
                yi -= ty;
                zi -= tz;

                xf -= tx;
                yf -= ty;
                zf -= tz;

                if (Geom.Iguais(xi, 0))
                    xi = 0;
                if (Geom.Iguais(yi, 0))
                    yi = 0;
                if (Geom.Iguais(zi, 0))
                    zi = 0;

                if (Geom.Iguais(xf, 0))
                    xf = 0;
                if (Geom.Iguais(yf, 0))
                    yf = 0;
                if (Geom.Iguais(zf, 0))
                    zf = 0;

                u1 = new vec3(xi, yi, zi * -1);
                u2 = new vec3(xf, yf, zf * -1);

                //Encontrar angulo que a linha faz com os planos XY e XZ
                normxy = new vec3(0, 0, 1);
                u = u1 - u2;

                NdotU = (normxy.DotProduct(u));
                ndotu_mod = normxy.Magnitude() * u.Magnitude();
                cos_alfa = Math.Abs(NdotU / ndotu_mod);
                angXY = RMath.rad2deg(Math.Acos(cos_alfa));
                angXY =(90 - angXY) * 1;

                //PLANO XZ
                normxz = new vec3(0, 1, 0);
                u = u1 - u2;

                NdotU = (normxz.DotProduct(u));
                ndotu_mod = normxz.Magnitude() * u.Magnitude();
                cos_alfa = Math.Abs(NdotU / ndotu_mod);
                angXZ = RMath.rad2deg(Math.Acos(cos_alfa));

                if (!Geom.Iguais(u1.x - u2.x, 0))
                    angXZ = RMath.rad2deg(Math.Atan(u1.y - u2.y / (u1.x - u2.x)));
                else
                {
                    if (yi < yf)
                        angXZ = -90;
                    else
                        angXZ = 90;
                }

                if ((xi < xf) && (!xi_igual_xf))
                    angXY *= -1;

                if (!Geom.Iguais(Math.Abs(angXZ), 90))
                    angXZ *= -1;

                if (xi_igual_xf)
                    angXY *= -1;

                if (zi_maior_que_zf)
                    angXY *= -1;

                posicao[1] = pIni.x;
                posicao[2] = pIni.y;
                posicao[3] = pIni.z;

                Geom.rotY(angXY, ref posicao, ref posicaoFinal);
                Geom.rotZ(angXZ, ref posicaoFinal, ref posicaoFinal2);

                pIni.x = posicaoFinal2[1];
                pIni.y = posicaoFinal2[2];
                pIni.z = posicaoFinal2[3];

                pIni.x += tx;
                pIni.y += ty;
                pIni.z += tz;

                posicao[1] = pFin.x;
                posicao[2] = pFin.y;
                posicao[3] = pFin.z;

                Geom.rotY(angXY, ref posicao, ref posicaoFinal);
                Geom.rotZ(angXZ, ref posicaoFinal, ref posicaoFinal2);

                pFin.x = posicaoFinal2[1];
                pFin.y = posicaoFinal2[2];
                pFin.z = posicaoFinal2[3];

                pFin.x += tx;
                pFin.y += ty;
                pFin.z += tz;
            }

            catch (Exception e)
            {

            }
        }
        public static void RotacionaObjeto(vec3 vetor, double angulo, ref TPonto pi, ref TPonto pf)
        {
            /* - faço translação da barra para o ponto zero usando tx ty  tz
               - encontro os angulos com os planos xy e xz
               - crio a barra no eixo global x, a partir do ponto zero ( x=0 y=0 z=0) e faço a subdivisao nos tamanhos
                  que eu quero criando a lista de pontos nesse eixo global x
               - rotaciono esses pontos em torno do ponto zero de acordo com os angulos que achei nos planos xy e xz
               - faço a translação dos pontos com sinal contrário ( -tx -ty -tz)  para voltar para a posição original 
             */

            try
            {
                vec3 pIni, pFin;
                        /*---------------*/
                vec3 u1, u2, u, normxy, normxz;
                double NdotU, ndotu_mod, cos_alfa, L, angXY, angXZ;
                List<vec3> CoordsSubdvisao = new List<vec3>();
                double[] posicao = new double[4];
                double[] posicaoFinal = new double[4];
                double[] posicaoFinal2 = new double[4];
                double[] posicaoFinal_Trans = new double[5];

                double tx, ty, tz;
                bool zi_maior_que_zf, xi_igual_xf;
                vec3 pinicial = new vec3(0);
                vec3 pfinal = new vec3(0);
                /*---------------*/
                double xi = pi.x;
                double xf = pf.x;

                double yi = pi.y;
                double yf = pf.y;

                double zi = pi.z;
                double zf = pf.z;

                if (Geom.Iguais(xi, 0))
                    xi = 0;
                if (Geom.Iguais(yi, 0))
                    yi = 0;
                if (Geom.Iguais(zi, 0))
                    zi = 0;

                if (Geom.Iguais(xf, 0))
                    xf = 0;
                if (Geom.Iguais(yf, 0))
                    yf = 0;
                if (Geom.Iguais(zf, 0))
                    zf = 0;


                L = (Math.Sqrt(Math.Pow(xi - xf, 2) + Math.Pow(yi - yf, 2) + Math.Pow(zi - zf, 2)));

                pIni = new vec3(0, 0, 0);
                pFin = new vec3(L, 0, 0);

                if (!Geom.Iguais(xi, xf))
                    if (xi > xf)
                        pFin = new vec3(-L, 0, 0);

                zi_maior_que_zf = false;
                zi_maior_que_zf = ((zi * -1) > (zf * -1));

                xi_igual_xf = Geom.Iguais(xi, xf);
                pinicial.x = xi; pinicial.y = yi; pinicial.z = zi;
                pfinal.x = xi; pfinal.y = yi; pfinal.z = zi;

                tx = vetor.x;
                ty = vetor.y;
                tz = vetor.z;

                pIni.x += tx;
                pIni.y += ty;
                pIni.z += tz;

                pFin.x += tx;
                pFin.y += ty;
                pFin.z += tz;

                posicao[1] = pIni.x;
                posicao[2] = pIni.y;
                posicao[3] = pIni.z;

               // Geom.rotY(angulo, ref posicao, ref posicaoFinal);
                Geom.rotZ(angulo, ref posicao, ref posicaoFinal);

                pIni.x = posicaoFinal[1];
                pIni.y = posicaoFinal[2];
                pIni.z = posicaoFinal[3];

                pIni.x += tx;
                pIni.y += ty;
                pIni.z += tz;

                posicao[1] = pFin.x;
                posicao[2] = pFin.y;
                posicao[3] = pFin.z;

         //       Geom.rotY(angXY, ref posicao, ref posicaoFinal);
                Geom.rotZ(angulo, ref posicao, ref posicaoFinal);

                pFin.x = posicaoFinal[1];
                pFin.y = posicaoFinal[2];
                pFin.z = posicaoFinal[3];

                pFin.x += tx;
                pFin.y += ty;
                pFin.z += tz;              
                
                pi.x = pIni.x; pi.y = pIni.y; pi.z = pIni.z;
                pf.x = pFin.x; pf.y = pFin.y; pf.z = pFin.z;
               
                return;

                u1 = new vec3(xi, yi, zi * -1);
                u2 = new vec3(xf, yf, zf * -1);

                //Encontrar angulo que a linha faz com os planos XY e XZ
                normxy = new vec3(0, 0, 1);
                u = u1 - u2;

                NdotU = (normxy.DotProduct(u));
                ndotu_mod = normxy.Magnitude() * u.Magnitude();
                cos_alfa = Math.Abs(NdotU / ndotu_mod);
                angXY = RMath.rad2deg(Math.Acos(cos_alfa));
                angXY = (90 - angXY) * 1;

                //PLANO XZ
                normxz = new vec3(0, 1, 0);
                u = u1 - u2;

                NdotU = (normxz.DotProduct(u));
                ndotu_mod = normxz.Magnitude() * u.Magnitude();
                cos_alfa = Math.Abs(NdotU / ndotu_mod);
                angXZ = RMath.rad2deg(Math.Acos(cos_alfa));

                if (!Geom.Iguais(u1.x - u2.x, 0))
                    angXZ = RMath.rad2deg(Math.Atan(u1.y - u2.y / (u1.x - u2.x)));
                else
                {
                    if (yi < yf)
                        angXZ = -90;
                    else
                        angXZ = 90;
                }

                if ((xi < xf) && (!xi_igual_xf))
                    angXY *= -1;

                if (!Geom.Iguais(Math.Abs(angXZ), 90))
                    angXZ *= -1;

                if (xi_igual_xf)
                    angXY *= -1;

                if (zi_maior_que_zf)
                    angXY *= -1;


               /* posicao[1] = pIni.x;
                posicao[2] = pIni.y;
                posicao[3] = pIni.z;

                Geom.rotY(angXY, ref posicao, ref posicaoFinal);
                Geom.rotZ(angXZ, ref posicaoFinal, ref posicaoFinal2);

                pIni.x = posicaoFinal2[1];
                pIni.y = posicaoFinal2[2];
                pIni.z = posicaoFinal2[3];

                pIni.x += tx;
                pIni.y += ty;
                pIni.z += tz;

                posicao[1] = pFin.x;
                posicao[2] = pFin.y;
                posicao[3] = pFin.z;

                Geom.rotY(angXY, ref posicao, ref posicaoFinal);
                Geom.rotZ(angXZ, ref posicaoFinal, ref posicaoFinal2);

                pFin.x = posicaoFinal2[1];
                pFin.y = posicaoFinal2[2];
                pFin.z = posicaoFinal2[3];

                pFin.x += tx;
                pFin.y += ty;
                pFin.z += tz;*/
            }

            catch (Exception e)
            {

            }
        }

        public static void InterceptaAABB(Raio raioZoom, ref List<TBarraGenerica> barras, ref List<int> ids)
        {
            vec3 lb = new vec3(0);
            vec3 rt = new vec3(0);

            vec3 rdir = (raioZoom.p1 - raioZoom.p0);
            vec3 rorg = raioZoom.p0;
            vec3 dirfrac = new vec3(0);
            dirfrac.x = 1.0f / rdir.x;
            dirfrac.y = 1.0f / rdir.y;
            dirfrac.z = 1.0f / rdir.z;
            double len;
            double t1, t2, t3, t4, t5, t6, tmin, tmax;
            for (int i = 0; i < barras.Count; i++)
            {
                // r.dir is unit direction vector of ray

                // lb is the corner of AABB with minimal coordinates - left bottom, rt is maximal corner
                // r.org is origin of ray
            
                //  if (!barras[i].NaTela()) 
                 //   continue;

                lb.x = barras[i].min_x_AABB;
                lb.y = -barras[i].min_y_AABB;
                lb.z = -barras[i].min_z_AABB;

                rt.x = barras[i].max_x_AABB;
                rt.y = Geom.Iguais(barras[i].max_y_AABB, 0) ? 0 : barras[i].max_y_AABB * -1;
                rt.z = -barras[i].max_z_AABB;

                t1 = (lb.x - rorg.x) * dirfrac.x;
                t2 = (rt.x - rorg.x) * dirfrac.x;
                t3 = (lb.y - rorg.y) * dirfrac.y;
                t4 = (rt.y - rorg.y) * dirfrac.y;
                t5 = (lb.z - rorg.z) * dirfrac.z;
                t6 = (rt.z - rorg.z) * dirfrac.z;

                tmin = Math.Max(Math.Max(Math.Min(t1, t2), Math.Min(t3, t4)), Math.Min(t5, t6));
                tmax = Math.Min(Math.Min(Math.Max(t1, t2), Math.Max(t3, t4)), Math.Max(t5, t6));
                tmin = Math.Abs(tmin);

                // if tmax < 0, ray (line) is intersecting AABB, but the whole AABB is behind us
                /* if (tmax < 0) 
                 {
                     len = tmax;
                     return;
                 }

                 // if tmin > tmax, ray doesn't intersect AABB
                 if (tmin > tmax) continue;
                 {
                     len = tmax;
                    return;
                 }
                 len = tmin;*/ // len é a distancia da origem do raio até a interseção. Por enquanto nao tem utilidade, mas vou deixar...

                if (tmax < 0 || tmin > tmax) continue;

                ids.Add(barras[i].IDBarra);
            }
        }

        public static void Trans3D(ref double[] pos, double tx, double ty, double tz, ref double[] posFinal)
        {
            vecPosicao[1] = pos[1];
            vecPosicao[2] = pos[2];
            vecPosicao[3] = pos[3];
            vecPosicao[4] = 1;

            mTrans[1, 1] = 1;
            mTrans[1, 2] = 0;
            mTrans[1, 3] = 0; 
            mTrans[1, 4] = 0;

            mTrans[2, 1] = 0;
            mTrans[2, 2] = 1;
            mTrans[2, 3] = 0;
            mTrans[2, 4] = 0;

            mTrans[3, 1] = 0;
            mTrans[3, 2] = 0;
            mTrans[3, 3] = 1;
            mTrans[3, 4] = 0;

            mTrans[4, 1] = tx;
            mTrans[4, 2] = ty;
            mTrans[4, 3] = tz;
            mTrans[4, 4] = 1;

            TAlgebra.Multiplica_Matriz_Vetor(ref mTrans, ref vecPosicao, ref posFinal, 4, 4);
        }

        public static void rotY(double a, ref double[] pos, ref double[] posFinal)
        {
            mRot[1, 1] = Math.Cos(RMath.deg2rad(a));
            mRot[1, 2] = 0;
            mRot[1, 3] = -Math.Sin(RMath.deg2rad(a));

            mRot[2, 1] = 0;
            mRot[2, 2] = 1;
            mRot[2, 3] = 0;

            mRot[3, 1] = Math.Sin(RMath.deg2rad(a));
            mRot[3, 2] = 0;
            mRot[3, 3] = Math.Cos(RMath.deg2rad(a));

            TAlgebra.Multiplica_Matriz_Vetor(ref mRot, ref pos, ref posFinal, 3, 3);
        }

        public static void rotX(double a, ref double[] pos, ref double[] posFinal)
        {
            mRot[1, 1] = 1;
            mRot[1, 2] = 0;
            mRot[1, 3] = 0;

            mRot[2, 1] = 0;
            mRot[2, 2] = Math.Cos(RMath.deg2rad(a));
            mRot[2, 3] = Math.Sin(RMath.deg2rad(a));

            mRot[3, 1] = 0;
            mRot[3, 2] = -Math.Sin(RMath.deg2rad(a));
            mRot[3, 3] = Math.Cos(RMath.deg2rad(a));

            TAlgebra.Multiplica_Matriz_Vetor(ref mRot, ref pos, ref posFinal, 3, 3);
        }


        public static void RotacionarSecaoBarra(vec3 pIniBarra, vec3 pFinBarra, double[] coordSecao)
        {
            double L = (Math.Sqrt(Math.Pow(pIniBarra.x - pFinBarra.x, 2) + Math.Pow(pIniBarra.y - pFinBarra.y, 2)));

            double cx = ((pIniBarra.x) -
                 (pFinBarra.x)) / L;

            double cy = ((pIniBarra.y) -
                 (pFinBarra.y)) / L;

            double cz = ((pIniBarra.z) -
                 (pFinBarra.z)) / L;

            vec3 u1 = new vec3(pIniBarra.x, pIniBarra.y, pIniBarra.z);
            vec3 u2 = new vec3(pFinBarra.x, pFinBarra.y, pFinBarra.z);
            vec3 u;

            //Encontrar angulo que a barra faz com os planos XY e XZ

            //PLANO XY
            vec3 normxy = new vec3(0, 0, 1);

            if (pIniBarra.x < pFinBarra.x)
                u = u1 - u2;
            else
                u = u2 - u1;

            double NdotU = (normxy.DotProduct(u));
            double ndotu_mod = normxy.Magnitude() * u.Magnitude();
            double cos_alfa = NdotU / ndotu_mod;
            angXY = RMath.rad2deg(Math.Acos(cos_alfa));
            angXY = (90 - angXY) * -1;


            //PLANO XZ
            vec3 normxz = new vec3(0, 1, 0);
            if (pIniBarra.x < pFinBarra.x)
                u = u2 - u1;
            else
                u = u1 - u2;

            NdotU = (normxz.DotProduct(u));
            ndotu_mod = normxz.Magnitude() * u.Magnitude();
            cos_alfa = NdotU / ndotu_mod;
            angXZ = RMath.rad2deg(Math.Acos(cos_alfa));
            angXZ = (90 - angXZ) * -1;


            //desenha a seção e o objeto no plano YZ

        }
        /*
          
// Assume Coord has members x(), y() and z() and supports arithmetic operations	   
    // that is Coord u + Coord v = u.x() + v.x(), u.y() + v.y(), u.z() + v.z()	   
           
    inline Point	   
    dot(const Coord& u, const Coord& v) 	   
    {	   
        return u.x() * v.x() + u.y() * v.y() + u.z() * v.z();   	   
    }	   
           
    inline Point	   
    norm2( const Coord& v )	   
    {	   
        return v.x() * v.x() + v.y() * v.y() + v.z() * v.z();	   
    }	   
           
    inline Point	   
    norm( const Coord& v ) 	   
    {	   
        return sqrt(norm2(v));	   
    }	   
           
    inline	   
    Coord	   
    cross( const Coord& b, const Coord& c) // cross product	   
    {	   
        return Coord(b.y() * c.z() - c.y() * b.z(), b.z() * c.x() - c.z() * b.x(), b.x() *  c.y() - c.x() * b.y());	   
    }	   
           
    bool 	   
    intersection(const Line& a, const Line& b, Coord& ip)	   
    // TODO: To work in 2D set z components to zero	   
    {	   
    Coord da = a.second - a.first; 	   
    Coord db = b.second - b.first;	   
    Coord dc = b.first - a.first;	   
           
    if (dot(dc, cross(da,db)) != 0.0) // lines are not coplanar	   
        return false;	   
           
        Point s = dot(cross(dc,db),cross(da,db)) / norm2(cross(da,db));	   
        if (s >= 0.0 && s <= 1.0)	   
        {	   
            ip = a.first + da * Coord(s,s,s);	   
            return true;	   
        }	   
           
        return false;	   
    }	 

         
         */


        public static double Comprimento(TPonto p1, TPonto p2)
        {
            return (Math.Sqrt(Math.Pow(p1.x - p2.x, 2) + Math.Pow(p1.y - p2.y, 2)));
        }



        public static void ReordenaPontos(double yini, double yfin, double xini, double xfin, ref List<TPonto> pontos)
        {
            //se for trecho horizontal, reordena os nós em ordem crescente de X
            int kk;
            TPonto temp;
            if (Geom.Iguais(yini, yfin))
            {
                if (xini > xfin)
                {
                    for (kk = 0; kk < pontos.Count; kk++)
                    {
                        for (int j = kk; j < pontos.Count; j++)
                        {
                            if (pontos[j].x > pontos[kk].x)
                            {
                                temp = pontos[kk];
                                pontos[kk] = pontos[j];
                                pontos[j] = temp;
                            }
                        }
                    }
                }
                else
                {
                    for (kk = 0; kk < pontos.Count; kk++)
                    {
                        for (int j = kk; j < pontos.Count; j++)
                        {
                            if (pontos[j].x < pontos[kk].x)
                            {
                                temp = pontos[kk];
                                pontos[kk] = pontos[j];
                                pontos[j] = temp;
                            }
                        }
                    }
                }
            }
            else
            {
                //senao reordena os nós em ordem crescente de Y, independente se for trecho vertical ou oblíquo
                if (yini > yfin)
                {
                    for (kk = 0; kk < pontos.Count; kk++)
                    {
                        for (int j = kk; j < pontos.Count; j++)
                        {
                            if (pontos[j].y > pontos[kk].y)
                            {
                                temp = pontos[kk];
                                pontos[kk] = pontos[j];
                                pontos[j] = temp;
                            }
                        }
                    }
                }
                else
                {
                    for (kk = 0; kk < pontos.Count; kk++)
                    {
                        for (int j = kk; j < pontos.Count; j++)
                        {
                            if (pontos[j].y < pontos[kk].y)
                            {
                                temp = pontos[kk];
                                pontos[kk] = pontos[j];
                                pontos[j] = temp;
                            }
                        }
                    }
                }
            };
        }
 
        public static bool Iguais(float a, float b, float tolerancia)
        {
            return ((Math.Abs(a - b) < tolerancia));
        }
        public static bool Iguais(float a, float b)
        {
            return ((Math.Abs(a - b) < 0.001f));
        }

        public static bool Iguais(double a, double b, double tolerancia)
        {
            return ((Math.Abs(a - b) < tolerancia));
        }

        public static bool Iguais(double a, double b)
        {
            return ((Math.Abs(a - b) < 0.001f));
        }
        public static bool MaiorOuIgual(double a, double b)
        {
            return a > b || Iguais(a, b);
        }

        public static bool MenorOuIgual(double a, double b)
        {
            return a < b || Iguais(a, b);
        }

        public static bool SaoIguais(double a, double b, double eps = 1e-9)
        {
            double aa = Math.Max(Math.Abs(a), Math.Abs(b));
            double ii = Math.Max(1.0, aa);
            return Math.Abs(a - b) <= eps * ii;
        }

        public static double GetAnguloGlobal(double xi, double yi, double xf, double yf)
        {
            if ((xf > xi) && Geom.Iguais(yf, yi))
                return  0;
            if ((xf < xi) && Geom.Iguais(yf, yi))
                return  180;
            if (Geom.Iguais(xf, xi) && (yf > yi))
                return  90;
            if (Geom.Iguais(xf, xi) && (yf < yi))
                return 270;

            if ((xf > xi) && (yf > yi))
                return (Math.Atan((yf - yi) / (xf - xi)) * 57.2958);

            if ((xf < xi) && (yf > yi))
                return 180 + (Math.Atan((yf - yi) / (xf - xi)) * 57.2958);

            if ((xf < xi) && (yf < yi))
                return 180 + (Math.Atan((yf - yi) / (xf - xi)) * 57.2958);

            if ((xf > xi) && (yf < yi))
            {
                double ata = Math.Atan((yf - yi) / (xf - xi));
                return 360 + (ata * 57.2958); // aqui era (360 - ...) na dissertação, acho q estava errado
            }

            return 0;
        }
        static double ab, ap, pb;
        static bool em;
        public static bool PontoEmLinha2(double posX, double posY, double xi, double yi, double xf, double yf, double tol = 0.1)
        {
            em = false;
            ab = Math.Sqrt((xf - xi) * (xf - xi) + (yf - yi) * (yf - yi));
            ap = Math.Sqrt((posX - xi) * (posX - xi) + (posY - yi) * (posY - yi));
            pb = Math.Sqrt((xf - posX) * (xf - posX) + (yf - posY) * (yf - posY));

            if (Geom.Iguais(ab, (ap + pb), tol)) 
                return true;
            else 
                return false;
        }

        public static bool PontoEmLinha3D(double posX, double posY, double posZ, double xi, double yi, double zi, double xf, double yf, double zf, 
                                          double tol = 0.1)
        {
            
            return false;
        }
        public static bool PontoEmLinha3(double posX, double posY, double xi, double yi, double xf, double yf, double tol = 0.01)
        {
            bool esta_no_intervalo = false;

            if (posX - Math.Max(xi, xf) > tol || Math.Min(xi, xf) - posX > tol ||
                posY - Math.Max(yi, yf) > tol || Math.Min(yi, yf) - posY > tol)

                esta_no_intervalo = false;

            if (Math.Abs(xf - xi) < tol)
                esta_no_intervalo = Math.Abs(xi - posX) < tol || Math.Abs(xf - posX) < tol;

            if (Math.Abs(yf - yi) < tol)
                esta_no_intervalo = Math.Abs(yi - posY) < tol || Math.Abs(yf - posY) < tol;

            double x, y;

            if (Geom.Iguais(yf, yi, tol))
                x = 0;
            else
                x = xi + (posY - yi) * (xf - xi) / (yf - yi);

            if (Geom.Iguais(xf, xi, tol))
                y = 0;
            else
                y = yi + (posX - xi) * (yf - yi) / (xf - xi);

            double x1 = Math.Min(xi, xf);
            double y1 = Math.Min(yi, yf);
            double x2 = Math.Max(xi, xf);
            double y2 = Math.Max(yi, yf);

            esta_no_intervalo = Math.Abs(posX - x) < tol || Math.Abs(posY - y) < tol;

            if (esta_no_intervalo)
            {
                float tolerancia = (float)tol;

                if (Geom.Iguais(y1, y2, tolerancia))
                    esta_no_intervalo = (((posX >= x1) && (posX <= x2)) && (Geom.Iguais(posY, y1, tol)));
                else
                if (Geom.Iguais(x1, x2, tolerancia))
                    esta_no_intervalo = (((posY >= y1) && (posY <= y2)) && (Geom.Iguais(posX, x1, tol)));
                else
                    esta_no_intervalo = ((posX >= x1) && (posX <= x2)) && ((posY >= y1) && (posY <= y2));


                /*   if (Geom.Iguais(y1, y2, tol))
                   {
                   //    float tolerancia = 0.1f;
                  //     if (posX > x1 || (Geom.Iguais(posX, x1)))
                //         esta_no_intervalo = true;
                 //      else
             //          if (posX < x2 || (Geom.Iguais(posX, x2)))
                           esta_no_intervalo = ((posX > x1 || (Geom.Iguais(posX, x1))) && (posX < x2 || (Geom.Iguais(posX, x2))));
                   }
                   else
                   if (Geom.Iguais(x1, x2, tol))
                   {
                       esta_no_intervalo = ((posY > y1 || (Geom.Iguais(posY, y1))) && (posY < y2 || (Geom.Iguais(posY, y2))));

                    /*   if (Geom.Iguais(x1, 257.197657))
                           esta_no_intervalo = true;

                       if (posY > y1 || (Geom.Iguais(posY, y1)))
                         esta_no_intervalo = true;
                       else
                       if (posY < y2 || (Geom.Iguais(posY, y2)))
                         esta_no_intervalo = true;*/
                /*  }
                  else
                    esta_no_intervalo = ((posX >= x1) && (posX <= x2)) && ((posY >= y1) && (posY <= y2));*/
            }
            ;

            return esta_no_intervalo;
        }

        public static bool PontoEmLinha(double posX, double posY, double xi, double yi, double xf, double yf, double tol = 0.01)
        {
            bool esta_no_intervalo = false;

            if (posX - Math.Max(xi, xf) > tol || Math.Min(xi, xf) - posX > tol ||
                posY - Math.Max(yi, yf) > tol || Math.Min(yi, yf) - posY > tol)

            esta_no_intervalo = false;

            if (Math.Abs(xf - xi) < tol)
                esta_no_intervalo = Math.Abs(xi - posX) < tol || Math.Abs(xf - posX) < tol;

            if (Math.Abs(yf - yi) < tol)
                esta_no_intervalo = Math.Abs(yi - posY) < tol || Math.Abs(yf - posY) < tol;

            double x, y;

            if (Geom.Iguais(yf, yi, 0.1))
                x = 0;
            else
                x = xi + (posY - yi) * (xf - xi) / (yf - yi);

            if (Geom.Iguais(xf, xi, 0.1))
                y = 0;
            else
                y = yi + (posX - xi) * (yf - yi) / (xf - xi);

            double x1 = Math.Min(xi, xf);
            double y1 = Math.Min(yi, yf);
            double x2 = Math.Max(xi, xf);
            double y2 = Math.Max(yi, yf);

            esta_no_intervalo = Math.Abs(posX - x) < tol || Math.Abs(posY - y) < tol;

            if (esta_no_intervalo)
            {
                float tolerancia = 0.1f;
               
                if (Geom.Iguais(y1, y2, tolerancia))
                    esta_no_intervalo = (((posX >= x1) && (posX <= x2)) && (Geom.Iguais(posY, y1, 0.0001)));
                else
                if (Geom.Iguais(x1, x2, tolerancia))
                    esta_no_intervalo = (((posY >= y1) && (posY <= y2)) && (Geom.Iguais(posX, x1, 0.0001)));
                else
                    esta_no_intervalo = ((posX >= x1) && (posX <= x2)) && ((posY >= y1) && (posY <= y2));


             /*   if (Geom.Iguais(y1, y2, tol))
                {
                //    float tolerancia = 0.1f;
               //     if (posX > x1 || (Geom.Iguais(posX, x1)))
             //         esta_no_intervalo = true;
              //      else
          //          if (posX < x2 || (Geom.Iguais(posX, x2)))
                        esta_no_intervalo = ((posX > x1 || (Geom.Iguais(posX, x1))) && (posX < x2 || (Geom.Iguais(posX, x2))));
                }
                else
                if (Geom.Iguais(x1, x2, tol))
                {
                    esta_no_intervalo = ((posY > y1 || (Geom.Iguais(posY, y1))) && (posY < y2 || (Geom.Iguais(posY, y2))));
                    
                 /*   if (Geom.Iguais(x1, 257.197657))
                        esta_no_intervalo = true;

                    if (posY > y1 || (Geom.Iguais(posY, y1)))
                      esta_no_intervalo = true;
                    else
                    if (posY < y2 || (Geom.Iguais(posY, y2)))
                      esta_no_intervalo = true;*/
              /*  }
                else
                  esta_no_intervalo = ((posX >= x1) && (posX <= x2)) && ((posY >= y1) && (posY <= y2));*/
            };

            return esta_no_intervalo;
        }
        public static vec3 CriaVetor(vec3 ponto1, vec3 ponto2, bool unitario = false)
        {
            vec3 v = new vec3(ponto2 - ponto1);
            v.z *= -1; v.y *= -1;

            if (unitario)
              v = v.Unitario();

            return v;
        }
        static double mod1, mod2, dot, angto, anguloGrau;
        public static double AnguloEntre2Vetores2(vec3 v1, vec3 v2)
        {
            mod1 = v1.Magnitude();
            mod2 = v2.Magnitude();

            dot = v1.DotProduct(v2);
            angto = dot / (mod1 * mod2);

            if (Geom.Iguais(angto, 1)) angto = 1; else
            if (Geom.Iguais(angto, -1)) angto = -1;

            anguloGrau = System.Math.Acos(angto) * 57.2958;
            if (double.IsNaN(anguloGrau))
              anguloGrau = 0;

            return anguloGrau;
        }
        public static double AnguloEntre2Vetores(vec3 v1, vec3 v2)
        {
            double mod1 = v1.Magnitude();
            double mod2 = v2.Magnitude();

            // Evita divisão por zero
            if (Geom.Iguais(mod1, 0) || Geom.Iguais(mod2, 0))
                return 0;

            double cosTheta = v1.DotProduct(v2) / (mod1 * mod2);

            // Corrige pequenos erros numéricos
            cosTheta = Math.Max(-1.0, Math.Min(1.0, cosTheta));
            double ang = Math.Acos(cosTheta) * (180.0 / Math.PI);
            // Retorna o menor ângulo entre os vetores (0° a 180°)
            return ang;
        }

        public static float AnguloEntre2Vetores(float x1, float y1, float x2, float y2)
        {
            float soma_dos_quadrados1,
                  soma_dos_quadrados2,
                  norma1,
                  norma2,
                  prod_escalar,
                  prod_normas;
            
            prod_escalar        = (x1 * x2) + (y1 * y2);

            soma_dos_quadrados1 = (x1 * x1) + (y1 * y1);
            soma_dos_quadrados2 = (x2 * x2) + (y2 * y2);

            norma1      = System.Convert.ToSingle(Math.Sqrt(soma_dos_quadrados1));
            norma2      = System.Convert.ToSingle(Math.Sqrt(soma_dos_quadrados2));
            prod_normas = norma1* norma2;

            return System.Convert.ToSingle(Math.Acos(prod_escalar / prod_normas)) / Const.PIDiv180;
        }

        public static Ponto Meio(ref Ponto P1, ref Ponto P2)
        {
            Ponto P;
            P.x = (P1.x + P2.x) / 2;
            P.y = (P1.y + P2.y) / 2;
            return P;
        }

        public static void MeioDaLinha(float x1, float y1, float x2, float y2, ref float MeioX, ref float MeioY)
        {
            if (x2 < x1)
              MeioX = x1 - (Math.Abs(x1 - x2) / 2);
            else
              MeioX = x1 + (Math.Abs(x1 - x2) / 2);

            if (y2 > y1)
              MeioY = y1 + (Math.Abs(y1 - y2) / 2);
            else
              MeioY = y1 - (Math.Abs(y1 - y2) / 2);      
        }

        public static PontoD MeioDaLinha(double x1, double y1, double x2, double y2)
        {
            PontoD meio = new PontoD(0,0,0);

            if (x2 < x1)
                meio.x = x1 - (Math.Abs(x1 - x2) / 2);
            else
                meio.x = x1 + (Math.Abs(x1 - x2) / 2);

            if (y2 > y1)
                meio.y = y1 + (Math.Abs(y1 - y2) / 2);
            else
                meio.y = y1 - (Math.Abs(y1 - y2) / 2);

            return meio;
        }

        public static PontoD FracaoDeLinha(double x1, double y1, double x2, double y2, double fracao)
        {
            PontoD meio = new PontoD(0, 0, 0);

            if (x2 < x1)
                meio.x = x1 - (Math.Abs(x1 - x2) / fracao);
            else
                meio.x = x1 + (Math.Abs(x1 - x2) / fracao);

            if (y2 > y1)
                meio.y = y1 + (Math.Abs(y1 - y2) / fracao);
            else
                meio.y = y1 - (Math.Abs(y1 - y2) / fracao);

            return meio;
        }

        public static int Lado(ref Ponto P1, ref Ponto P2, ref Ponto P)
        {
            Ponto V1;
            Ponto V2;

            V1.x = P2.x - P1.x;
            V1.y = P2.y - P1.y;
            V2.x = P.x - P1.x;
            V2.y = P.y - P1.y;

            double k = V1.x * V2.y - V1.y * V2.x;
            if (Math.Abs(k) < 0.0001)
                return 0;
            if (k > 0)
                return -1;
            else return 1;
        }

        public static int existeIntersec(ref Ponto P1, ref Ponto P2, ref Ponto PA, ref Ponto PB)
        {
            int La, Lb, L1, L2;
            La = Lado(ref P1, ref P2, ref PA);

            Lb = Lado(ref P1, ref P2, ref PB);

            if (La == Lb) return 0;
            L1 = Lado(ref PA, ref PB, ref P1);
            L2 = Lado(ref PA, ref PB, ref P2);
            if (L1 == L2) return 0;
            return 1;
        }
       
        public static bool intersec2d(ref Ponto k, ref Ponto l, ref Ponto m, ref Ponto n, ref float s, ref float t)
        {
            float det;
            det = (n.x - m.x) * (l.y - k.y) - (n.y - m.y) * (l.x - k.x);
            if (det == 0.0)
                return false; // nao ha intersecao
            s = ((n.x - m.x) * (m.y - k.y) - (n.y - m.y) * (m.x - k.x)) / det;
            t = ((l.x - k.x) * (m.y - k.y) - (l.y - k.y) * (m.x - k.x)) / det;
            return true; // ha intersecao
        }

        public static bool calcIntersecEQU_RETA(ref Ponto P1, ref Ponto P2, ref Ponto PA, ref Ponto PB, ref Ponto Pm)
        {
            float s = 0;
            float t = 0;

            if (! intersec2d(ref P1, ref P2, ref PA, ref PB, ref s,ref t))
                return false;

            if ((s < 0) || (s > 1) || (t < 0) || (t > 1))
                return false;

            Pm.x = P1.x + (P2.x - P1.x) * s;
            Pm.y = P1.y + (P2.y - P1.y) * s;

            return true;
        }

        public static bool intersec2d(ref PontoD k, ref PontoD l, ref PontoD m, ref PontoD n, ref double s, ref double t)
        {
            double det;
            det = (n.x - m.x) * (l.y - k.y) - (n.y - m.y) * (l.x - k.x);
            if (det == 0.0)
                return false; // nao ha intersecao
            s = ((n.x - m.x) * (m.y - k.y) - (n.y - m.y) * (m.x - k.x)) / det;
            t = ((l.x - k.x) * (m.y - k.y) - (l.y - k.y) * (m.x - k.x)) / det;
            return true; // ha intersecao
        }

        public static bool calcIntersecEQU_RETA(ref PontoD P1, ref PontoD P2, ref PontoD PA, ref PontoD PB, ref PontoD Pm)
        {
            double s = 0;
            double t = 0;

            if (!intersec2d(ref P1, ref P2, ref PA, ref PB, ref s, ref t))
                return false;

            if ((s < 0) || (s > 1) || (t < 0) || (t > 1))
                return false;

            Pm.x = P1.x + (P2.x - P1.x) * s;
            Pm.y = P1.y + (P2.y - P1.y) * s;

            return true;
        }

        public static bool calcIntersecEQU_RETA(PontoAux P1, PontoAux P2, PontoAux PA, PontoAux PB, ref PontoAux Pm)
        {
            double s = 0;
            double t = 0;

            if (!intersec2d(P1, P2, PA, PB, ref s, ref t))
                return false;

            if ((s < 0) || (s > 1) || (t < 0) || (t > 1))
                return false;

            Pm.x = P1.x + (P2.x - P1.x) * s;
            Pm.y = P1.y + (P2.y - P1.y) * s;

            return true;
        }

        public static bool intersec2d(PontoAux k, PontoAux l, PontoAux m, PontoAux n, ref double s, ref double t)
        {
            double det;
            det = (n.x - m.x) * (l.y - k.y) - (n.y - m.y) * (l.x - k.x);
            if (det == 0.0)
                return false; // nao ha intersecao
            s = ((n.x - m.x) * (m.y - k.y) - (n.y - m.y) * (m.x - k.x)) / det;
            t = ((l.x - k.x) * (m.y - k.y) - (l.y - k.y) * (m.x - k.x)) / det;
            return true; // ha intersecao
        }
        //
        //
        public static bool intersec2d(PontoD k, PontoD l, PontoD m, PontoD n, ref double s, ref double t)
        {
            double det;
            det = (n.x - m.x) * (l.y - k.y) - (n.y - m.y) * (l.x - k.x);
            if (det == 0.0)
                return false; // nao ha intersecao
            s = ((n.x - m.x) * (m.y - k.y) - (n.y - m.y) * (m.x - k.x)) / det;
            t = ((l.x - k.x) * (m.y - k.y) - (l.y - k.y) * (m.x - k.x)) / det;
            return true; // ha intersecao
        }

        public static bool calcIntersecEQU_RETA(PontoD P1, PontoD P2, PontoD PA, PontoD PB, ref PontoD Pm)
        {
            double s = 0;
            double t = 0;

            if (!intersec2d(P1, P2, PA, PB, ref s, ref t))
                return false;

            if ((s < 0) || (s > 1) || (t < 0) || (t > 1))
                return false;

            Pm.x = P1.x + (P2.x - P1.x) * s;
            Pm.y = P1.y + (P2.y - P1.y) * s;

            return true;
        }
//

        public static bool calcIntersecEQU_RETA(ref double P1x, ref double P1y, 
                                                ref double P2x, ref double P2y,
                                                ref double PAx, ref double PAy,
                                                ref double PBx, ref double PBy,
                                                ref double Pmx, ref double Pmy)
        {
            double s = 0;
            double t = 0;

            if (!intersec2d(ref P1x,ref P1y, ref P2x, ref P2y, ref PAx, ref PAy, ref PBx, ref PBy, ref s, ref t))
                return false;

            if (!Geom.Iguais(s, 0))
            {
                if ((s < 0) || (s > 1) || (t < 0) || (t > 1))
                    return false;
            }

            Pmx = P1x + (P2x - P1x) * s;
            Pmy = P1y + (P2y - P1y) * s;

            return true;
        }


        public static bool intersec2d(ref double kx, ref double ky, 
                                      ref double lx,ref double ly,
                                      ref double mx, ref double my,
                                      ref double nx, ref double ny,
                                      ref double s, ref double t)
        {
            double det;
            det = (nx - mx) * (ly - ky) - (ny - my) * (lx - kx);
            if (det == 0.0)
                return false; // nao ha intersecao
            s = ((nx - mx) * (my - ky) - (ny - my) * (mx - kx)) / det;
            t = ((lx - kx) * (my - ky) - (ly - ky) * (mx - kx)) / det;
            return true; // ha intersecao
        }

        //
        public static bool calcIntersecEQU_RETA(double P1x,  double P1y,
                                         double P2x,  double P2y,
                                         double PAx,  double PAy,
                                         double PBx,  double PBy,
                                        ref double Pmx, ref double Pmy)
        {
            double s = 0;
            double t = 0;

            if (!intersec2d( P1x,  P1y,  P2x,  P2y,  PAx,  PAy,  PBx, PBy, ref s, ref t))
                return false;

            if ((s < 0) || (s > 1) || (t < 0) || (t > 1))
                return false;

            Pmx = P1x + (P2x - P1x) * s;
            Pmy = P1y + (P2y - P1y) * s;

            return true;
        }


        public static bool intersec2d( double kx,  double ky,
                                       double lx,  double ly,
                                       double mx,  double my,
                                       double nx,  double ny,
                                      ref double s, ref double t)
        {
            double det;
            det = (nx - mx) * (ly - ky) - (ny - my) * (lx - kx);
            if (det == 0.0)
                return false; // nao ha intersecao
            s = ((nx - mx) * (my - ky) - (ny - my) * (mx - kx)) / det;
            t = ((lx - kx) * (my - ky) - (ly - ky) * (mx - kx)) / det;
            return true; // ha intersecao
        }
        static vec3 P, Q, DP, DQ, PQ, cp1, cp2, Quu, Ptt;

        static double m1,m2, a, b, c, d, e, DD, div_, s, Dist, pqdotcp2;
       
        //https://mathworld.wolfram.com/Point-LineDistance3-Dimensional.html
        public static double DistanciaPerpendicular(vec3 p1, vec3 p2, vec3 pontoAvaliar)
        {
            vec3 v1 = (pontoAvaliar - p1).CrossProduct(pontoAvaliar - p2);
            vec3 v2 = p2 - p1;
            m1 = v1.Magnitude();
            m2 = v2.Magnitude(); //m2 é duas vezes a área do triangulo formado

            vec3 pPerp = PontoPerpendicular(p1,p2, pontoAvaliar);
            m1 += pPerp.Magnitude();
            m1 -= (pPerp.Magnitude()*1);

            return m1 / m2;
        }

        public class ResultadoSegmento
        {
            public PosicaoNoSegmento Posicao;
            public double T;
            public ResultadoSegmento (PosicaoNoSegmento p, double _t)
            {
                this.Posicao = p;   
                this.T = _t;    
            }
        }

        public static ResultadoSegmento ClassificarPontoNoSegmento(vec3 ponto, vec3 p1, vec3 p2)
        {
            vec3 v = p2 - p1;

            double vv = v.DotProduct(v);

            // Barra degenerada
            if (vv < Const.Tol * Const.Tol)
                return new ResultadoSegmento(PosicaoNoSegmento.Fora, -1);

            // Coordenada paramétrica
            double t = (ponto - p1).DotProduct(v) / vv;

            // Projeção ortogonal na reta
            vec3 proj = p1 + t * v;

            double distancia = (ponto - proj).Magnitude();

            // Não pertence à reta da barra
            if (distancia > Const.Tol)
                return new ResultadoSegmento(PosicaoNoSegmento.Fora,-1);

            // Extremidade inicial
            if (Math.Abs(t) <= Const.Tol)
                return new ResultadoSegmento(PosicaoNoSegmento.ExtremidadeInicial, t);

            // Extremidade final
            if (Math.Abs(t - 1.0) <= Const.Tol)
                return new ResultadoSegmento(PosicaoNoSegmento.ExtremidadeFinal, t);

            // Interior
            if (t > Const.Tol && t < 1.0 - Const.Tol)
                return new ResultadoSegmento(PosicaoNoSegmento.Interior, t);

            // Está na reta mas fora do segmento
            return new ResultadoSegmento(PosicaoNoSegmento.Fora,-1);
        }
        public enum PosicaoNoSegmento
        {
            Fora,
            ExtremidadeInicial,
            ExtremidadeFinal,
            Interior
        }

        public static vec3 PontoPerpendicular(vec3 p1, vec3 p2, vec3 pontoAvaliar)
        {
            vec3 d = (p2 - p1) / ((p2-p1).Magnitude());
            vec3 v = (p1 - pontoAvaliar);
            double t = v.DotProduct(d);
            vec3 pontoPerpendicular = p1 - (t * d);

            return pontoPerpendicular;
        }

        //testa se pontoAvaliar toca na reta formada por p1 e p2
        //com exceção das extremidades.. no meio vai ser sempre dot1 = 0 e dot2 < 0
        public static bool TocaNoMeio2(vec3 pontoAvaliar, vec3 p1, vec3 p2)
        {
            vec3 v1 = (p2 - p1).Normalize();
            vec3 v2 = (pontoAvaliar - p1).Normalize();
            vec3 v3 = (pontoAvaliar - p2);
            double tol = Const.Tol;
            /*if (fabs(dot(v2,v1)-1.0)<EPS and dot(v3,v1)<0) return between
else return not between*/
            double dot1 = Math.Abs(v2.DotProduct(v1) - 1);
            double dot2 = v3.DotProduct(v1);

            if (!Geom.Iguais(dot2, 0, tol))
                if (Geom.Iguais(dot1, 0, tol) && (dot2 < 0))
                    return true;

            return false;
        }
   
        public static bool TocaNoMeio(vec3 pontoAvaliar, vec3 p1, vec3 p2)
        {
            vec3 v = p2 - p1;

            double vv = v.DotProduct(v);

            if (vv < Const.Tol * Const.Tol)
                return false; // barra degenerada

            double t = (pontoAvaliar - p1).DotProduct(v) / vv;

            vec3 proj = p1 + t * v;

            double distancia = (pontoAvaliar - proj).Magnitude();

            if (distancia > Const.Tol)
                return false;

            if (t <= Const.Tol)
                return false;

            if (t >= 1.0 - Const.Tol)
                return false;

            return true;
        }
        public static bool TocaNaExtremidade(vec3 pontoAvaliar, vec3 p1, vec3 p2)
        {
            if ((pontoAvaliar - p1).Magnitude() < Const.Tol)
                return true;

            if ((pontoAvaliar - p2).Magnitude() < Const.Tol)
                return true;

            return false;
        }

        //testa se pontoAvaliar é igual à uma extremidade da reta formada por p1 e p2
        public static bool TocaNaExtremidade2(vec3 pontoAvaliar, vec3 p1, vec3 p2)
        {
            if ((Geom.Iguais(p1.x, pontoAvaliar.x) && Geom.Iguais(p1.y, pontoAvaliar.y) && Geom.Iguais(p1.z, pontoAvaliar.z)) ||
                (Geom.Iguais(p2.x, pontoAvaliar.x) && Geom.Iguais(p2.y, pontoAvaliar.y) && Geom.Iguais(p2.z, pontoAvaliar.z)))
               return true;

            return false;
        }
        static double distancia_perpendicular;

        public static bool PontoTocaAresta(vec3 pontoAvaliar, vec3 p1, vec3 p2, ref vec3 pontoToque, bool TestarToqueExtremidade)
        {
            distancia_perpendicular = DistanciaPerpendicular(p1, p2, pontoAvaliar);
            bool setocam = false;
            if (Geom.Iguais(distancia_perpendicular, 0, 0.001))
            {
                bool seTocamNoMeio = TocaNoMeio(pontoAvaliar, p1, p2);

                if (!seTocamNoMeio)
                {
                    if (TestarToqueExtremidade)
                        setocam = TocaNaExtremidade(pontoAvaliar, p1, p2);
                }
                else
                    setocam = true;

                if (setocam)
                    pontoToque = pontoAvaliar;

                return setocam;
            }

            return false;
        }

        public static bool DuasArestasSeTocam(ref vec3 p1, ref vec3 p2, ref vec3 q1, ref vec3 q2, ref vec3 pontoToque, bool TestarToqueExtremidade)
        {
            vec3 pontoAvaliar;
            
            pontoAvaliar = p1;
            if (PontoTocaAresta(pontoAvaliar, q1, q2, ref pontoToque, TestarToqueExtremidade))
                return true;

            pontoAvaliar = p2;
            if (PontoTocaAresta(pontoAvaliar, q1, q2, ref pontoToque, TestarToqueExtremidade))
                return true;

            pontoAvaliar = q1;
            if (PontoTocaAresta(pontoAvaliar, p1, p2, ref pontoToque, TestarToqueExtremidade))
                return true;

            pontoAvaliar = q2;
            if (PontoTocaAresta(pontoAvaliar, p1, p2, ref pontoToque, TestarToqueExtremidade))
                return true;

            return false;
        }
        public static bool TemInterseccao3D(ref vec3 P0, ref vec3 P1, ref vec3 Q0, ref vec3 Q1, ref vec3 ip, ref double tt, ref double uu, double tolerancia = 0.001)
        {

            // se S = 0 ou S = 1 , entao tem intersecaono final das barras, se S= 0.5 ou 0.1 0.2 etc entao
            //intersecao dentro de uma barra, que é o mais apropriado , se for 0.5 entao é bem no meio da barra

            if (Geom.Iguais(P0.y, 0)) P0.y = 0;
            if (Geom.Iguais(P1.y, 0)) P1.y = 0;
            if (Geom.Iguais(Q0.y, 0)) Q0.y = 0;
            if (Geom.Iguais(Q1.y, 0)) Q1.y = 0;
            /*    P0.z *= -1;
                P1.z *= -1;
                Q0.z *= -1;
                Q1.z *= -1;*/

            DP = P1 - P0;
            DQ = Q1 - Q0;

            PQ = Q0 - P0;

            tt = 0;
            uu = 0;

            cp1 = PQ.CrossProduct(DQ);
            cp2 = DP.CrossProduct(DQ);
            div_ = cp2.norm2();

            if (Geom.Iguais(div_, 0, tolerancia))
                return false;

          //  if (Geom.Iguais(div_, 0, 0.0000001))
           //     return false;

            s = (cp1.DotProduct(cp2) / div_);
            ip = P0 + DP * s;

            /* if (Geom.Iguais(Math.Abs(s), 1))
                 return 1;
             if (Geom.Iguais(s, 0))
                 return 0;*/

            if (s < 0 || s > 1)
                return false;

            a = DP.DotProduct(DP);
            b = DP.DotProduct(DQ);
            c = DQ.DotProduct(DQ);
            d = DP.DotProduct(PQ);
            e = DQ.DotProduct(PQ);

            DD = a * c - b * b;
            tt = (b * e - c * d) / DD;
            uu = (a * e - b * d) / DD;

            Ptt = P0 + s * DP;
            Quu = Q0 + s * DQ;
            Dist = (Quu - Ptt).Magnitude();

            /* Ptt = P0 + tt * DP;
             Quu = Q0 + uu * DQ;
             Dist = (Quu - Ptt).Magnitude();*/

            if (Geom.Iguais(Dist, 0))
            {
                ip = Ptt;
                //      return s;
            }

            // return 10;

            if (Geom.Iguais(uu, 0)) uu = 0;
            if (Geom.Iguais(tt, 0)) tt = 0;

            if (tt > 0 && uu < 0)
                return false;

            if (tt < 0 && uu > 0)
                return false;

            //  if (Math.Abs(tt) > 1 || Math.Abs(uu) > 1) /*comentado dia 17/07/22 -> tinham casos em que tinha iterseccao e retornava 10, ou seja, retornava falso ...avaliar melhor essa funcao*/
            //    return 10;

            pqdotcp2 = PQ.DotProduct(cp2);
            if (!Geom.Iguais(pqdotcp2, 0)) // linhas não são coplanares
                return false;


            if (Geom.Iguais(Math.Abs(tt), 1))
                return false;
            if (Geom.Iguais(tt, 0))
                return false;

            if (Geom.Iguais(Math.Abs(uu), 1))
                return false;
            if (Geom.Iguais(uu, 0))
                return false;

            return (s > 0 && s < 1);


            //  if (dc.DotProduct(cp2) != 0.0) // linhas não são coplanares
            //   return false;


            //  ip = l1_p1 + da * Math.Abs( s);

            /*  if (Math.Abs(s) >= 0.0 && Math.Abs(s) <= 1.0)
              {
                  ip = l1_p1 + da * Math.Abs(s); 
                  //ip = l1_p1 + da * s;

                  return true;
              }*/


            //  else
            /* if (s >= -1 && s <= 0)
              {
                  ip = l1_p1 + da * s;

                  return true;
              }*/
            /*if (s >= 0 && s <= 1)
            {
                ip = P0 + DP * s;

                return true;
            }
            return false;*/
        }

        public static double Intersec3D(ref vec3 P0, ref vec3 P1, ref vec3 Q0, ref vec3 Q1, ref vec3 ip, ref double tt, ref double uu )
        {
            // se S = 0 ou S = 1 , entao tem intersecaono final das barras, se S= 0.5 ou 0.1 0.2 etc entao
            //intersecao dentro de uma barra, que é o mais apropriado , se for 0.5 entao é bem no meio da barra
            
            if (Geom.Iguais(P0.y, 0)) P0.y = 0;
            if (Geom.Iguais(P1.y, 0)) P1.y = 0;
            if (Geom.Iguais(Q0.y, 0)) Q0.y = 0;
            if (Geom.Iguais(Q1.y, 0)) Q1.y = 0;
        /*    P0.z *= -1;
            P1.z *= -1;
            Q0.z *= -1;
            Q1.z *= -1;*/

            DP = P1 - P0;
            DQ = Q1 - Q0;

            PQ = Q0 - P0;
            
            tt = 0;
            uu = 0;

            cp1 = PQ.CrossProduct(DQ);
            cp2 = DP.CrossProduct(DQ);
            div_ =  cp2.norm2();

            if (Geom.Iguais(div_, 0))
                return 10;

            s = (cp1.DotProduct(cp2) / div_);
            ip = P0 + DP * s;

           /* if (Geom.Iguais(Math.Abs(s), 1))
                return 1;
            if (Geom.Iguais(s, 0))
                return 0;*/

            if (s < 0 || s > 1)
                return 10;

            a = DP.DotProduct(DP);
            b = DP.DotProduct(DQ);
            c = DQ.DotProduct(DQ);
            d = DP.DotProduct(PQ);
            e = DQ.DotProduct(PQ);

            DD = a * c - b * b;
            tt = (b * e - c * d) / DD;
            uu = (a * e - b * d) / DD;

            Ptt = P0 + s * DP;
            Quu = Q0 + s * DQ;
            Dist = (Quu- Ptt).Magnitude();

           /* Ptt = P0 + tt * DP;
            Quu = Q0 + uu * DQ;
            Dist = (Quu - Ptt).Magnitude();*/

           if (Geom.Iguais(Dist, 0))
           {
                ip = Ptt;
           //      return s;
            }

           // return 10;

           if (Geom.Iguais(uu, 0)) uu = 0;
           if (Geom.Iguais(tt, 0)) tt = 0;

            if (tt > 0 && uu < 0)
               return 10;

            if (tt < 0 && uu > 0)
                return 10;

          //  if (Math.Abs(tt) > 1 || Math.Abs(uu) > 1) /*comentado dia 17/07/22 -> tinham casos em que tinha iterseccao e retornava 10, ou seja, retornava falso ...avaliar melhor essa funcao*/
            //    return 10;

             pqdotcp2 = PQ.DotProduct(cp2);
            if (!Geom.Iguais(pqdotcp2,0)) // linhas não são coplanares
                return 10;


            if (Geom.Iguais(Math.Abs(tt), 1))
                return 1;
            if (Geom.Iguais(tt, 0))
                return 0;

            if (Geom.Iguais(Math.Abs(uu), 1))
                return 1;
            if (Geom.Iguais(uu, 0))
                return 0;

            return s;


          //  if (dc.DotProduct(cp2) != 0.0) // linhas não são coplanares
           //   return false;


          //  ip = l1_p1 + da * Math.Abs( s);

          /*  if (Math.Abs(s) >= 0.0 && Math.Abs(s) <= 1.0)
            {
                ip = l1_p1 + da * Math.Abs(s); 
                //ip = l1_p1 + da * s;

                return true;
            }*/


          //  else
          /* if (s >= -1 && s <= 0)
            {
                ip = l1_p1 + da * s;

                return true;
            }*/
            /*if (s >= 0 && s <= 1)
            {
                ip = P0 + DP * s;

                return true;
            }
            return false;*/
        }

        /* ********************************************************************** */
/*                                                                        */
/*  Calcula a interseccao entre 2 retas (no espaco)                       */
/*                                                                        */
/* k : ponto inicial da reta 1                                            */
/* l : ponto final da reta 1                                              */
/* m : ponto inicial da reta 2                                            */
/* n : ponto final da reta 2                                              */
/*                                                                        */
/* s: valor do parâmetro no ponto de interseção (sobre a reta KL)         */
/* t: valor do parâmetro no ponto de interseção (sobre a reta MN)         */
/*                                                                        */
/* ********************************************************************** */
public static int intersec3d(TPonto k, TPonto l, TPonto m, TPonto n, ref double s, ref double t)
/*XYZR *k, *l, *m, *n;
double *t, *s ;*/
{
  double det;


 if (((k.x != l.x) || (k.y != l.y))  &&
     ((m.x != n.x) || (m.y != n.y)) )  /* se nao e' paralela ao plano XY*/
 {
  det = (n.x - m.x) * (l.y - k.y)  -  (n.y - m.y) * (l.x - k.x);

  if (det == 0.0)
   return 0 ;

  s = ((n.x - m.x) * (m.y - k.y) - (n.y - m.y) * (m.x - k.x))/ det ;
  t = ((l.x - k.x) * (m.y - k.y) - (l.y - k.y) * (m.x - k.x))/ det ;

  return 1;
 }

 if (((k.x != l.x) || (k.z != l.z))  &&
     ((m.x != n.x) || (m.z != n.z)) )  /* se nao e' paralela ao plano XZ*/
 {
  det = (n.x - m.x) * (l.z - k.z)  -  (n.z - m.z) * (l.x - k.x);

  if (det == 0.0)
   return 0 ;

  s = ((n.x - m.x) * (m.z - k.z) - (n.z - m.z) * (m.x - k.x))/ det ;
  t = ((l.x - k.x) * (m.z - k.z) - (l.z - k.z) * (m.x - k.x))/ det ;

  return 1;
 }

 if (((k.y != l.y) || (k.z != l.z))  &&
     ((m.y != n.y) || (m.z != n.z)) )  /* se nao e' paralela ao plano YZ*/
 {
  det = (n.y - m.y) * (l.z - k.z)  -  (n.z - m.z) * (l.y - k.y);

  if (det == 0.0)
   return 0 ;

  s = ((n.y - m.y) * (m.z - k.z) - (n.z - m.z) * (m.y - k.y))/ det ;
  t = ((l.y - k.y) * (m.z - k.z) - (l.z - k.z) * (m.y - k.y))/ det ;

  return 1;
 }

 return 1;
}

public static vec3 intersec3d(vec3 P0, vec3 Q0, vec3 P1, vec3 Q1, ref double tt, ref double uu, ref double distancia)
/*XYZR *k, *l, *m, *n;
double *t, *s ;*/
{
    vec3 DP = P1 - P0;
    vec3 DQ = Q1 - Q0;
    vec3 PQ = Q0 - P0;

  //  vec3 P = P0 + t * DP;
   // vec3 Q = Q0 + u * DQ;

double a = DP.DotProduct(DP);
double b = DP.DotProduct(DQ);
double c = DQ.DotProduct(DQ);
double d = DP.DotProduct(PQ);
double e = DQ.DotProduct(PQ);
double DD = (a * c) - (b * b);

 tt = (b * e - c * d) / DD;
 uu = (a * e - b * d) / DD;

 vec3 pi = P0 + tt * DP;
 vec3 qi = Q0 + uu * DQ;

 distancia = (qi - pi).Magnitude();

 return pi;
}

}
}
