using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using OpenTK;
using OpenTK.Graphics.OpenGL;
using OpenTK.Input;
using OpenTK.Platform;

namespace PG
{
    public class triangulo
    {
        public triangulo() { }
        public triangulo(ArestaCelula a, ArestaCelula b, ArestaCelula c, int _id)
        {
            a1 = a;
            a2 = b;
            a3 = c;
            id = _id;
            vizinhos = new List<int>();
            nos = new List<NoCelula>();
            calculaCentro();
            mesclou = false;
            pular = false;
        }
        public bool sel, avulso, mesclou, pular;
        public ArestaCelula a1, a2, a3;
        public int id;
        public List<int> vizinhos;
        public List<NoCelula> nos;
        public NoCelula centro, n1, n2, n3;
        public void calculaCentro()
        {
            vec3 p1, p2, p3, p4, p5, p6, v1, v2, v3 = new vec3(0);

            v1 = new vec3(a1.n1.x, a1.n1.y, 0);
            v2 = new vec3(a1.n2.x, a1.n2.y, 0);

            if ((a2.n1.id != a1.n1.id) && (a2.n1.id != a1.n2.id))
                v3 = new vec3(a2.n1.x, a2.n1.y, 0);
            else
            if ((a2.n2.id != a1.n1.id) && (a2.n2.id != a1.n2.id))
                v3 = new vec3(a2.n2.x, a2.n2.y, 0);

            p1 = new vec3(v1.x, v1.y, 0);
            p3 = new vec3(v2.x, v2.y, 0);
            p5 = new vec3(v3.x, v3.y, 0);
            p2 = (p3 + p1) / 2;
            p4 = (p5 + p3) / 2;
            p6 = (p5 + p1) / 2;

            vec3 centro_ = new vec3((v1.x + v2.x + v3.x) / 3, (v1.y + v2.y + v3.y) / 3, 0);
            centro = new NoCelula(centro_.x, centro_.y, 0, 0);
        }

        public void Desenha()
        {
            if (!avulso)
            {
             //   Gl.glColor3f(0, 1, 0);

                /*    if (avulso)
                    {
                        Gl.glColor3f(0, 1, 1);
                        Gl.glLineWidth(4);
                    }
                    */
                // if (sel)
                // {
               /* Gl.glBegin(Gl.GL_POLYGON);
                Gl.glColor3f(1, 0, 0);
                Gl.glLineWidth(2);
                Gl.glVertex2f(System.Convert.ToSingle(centro.x / Desenho.precisaoPixel + Desenho.ponto_zero[0]) - 10, System.Convert.ToSingle(centro.y / -Desenho.precisaoPixel + Desenho.ponto_zero[1]) - 10);
                Gl.glVertex2f(System.Convert.ToSingle(centro.x / Desenho.precisaoPixel + Desenho.ponto_zero[0]) - 10, System.Convert.ToSingle(centro.y / -Desenho.precisaoPixel + Desenho.ponto_zero[1]) + 10);
                Gl.glVertex2f(System.Convert.ToSingle(centro.x / Desenho.precisaoPixel + Desenho.ponto_zero[0]) + 10, System.Convert.ToSingle(centro.y / -Desenho.precisaoPixel + Desenho.ponto_zero[1]) + 10);
                Gl.glVertex2f(System.Convert.ToSingle(centro.x / Desenho.precisaoPixel + Desenho.ponto_zero[0]) + 10, System.Convert.ToSingle(centro.y / -Desenho.precisaoPixel + Desenho.ponto_zero[1]) - 10);
                Gl.glEnd();
              
                Gl.glLineWidth(1);


                Gl.glBegin(Gl.GL_LINES);
                Gl.glVertex2f(System.Convert.ToSingle(a1.n1.x / Desenho.precisaoPixel + Desenho.ponto_zero[0]), System.Convert.ToSingle(a1.n1.y / -Desenho.precisaoPixel + Desenho.ponto_zero[1]));
                Gl.glVertex2f(System.Convert.ToSingle(a1.n2.x / Desenho.precisaoPixel + Desenho.ponto_zero[0]), System.Convert.ToSingle(a1.n2.y / -Desenho.precisaoPixel + Desenho.ponto_zero[1]));
                Gl.glEnd();

                Gl.glBegin(Gl.GL_LINES);
                Gl.glVertex2f(System.Convert.ToSingle(a2.n1.x / Desenho.precisaoPixel + Desenho.ponto_zero[0]), System.Convert.ToSingle(a2.n1.y / -Desenho.precisaoPixel + Desenho.ponto_zero[1]));
                Gl.glVertex2f(System.Convert.ToSingle(a2.n2.x / Desenho.precisaoPixel + Desenho.ponto_zero[0]), System.Convert.ToSingle(a2.n2.y / -Desenho.precisaoPixel + Desenho.ponto_zero[1]));
                Gl.glEnd();

                Gl.glBegin(Gl.GL_LINES);
                Gl.glVertex2f(System.Convert.ToSingle(a3.n1.x / Desenho.precisaoPixel + Desenho.ponto_zero[0]), System.Convert.ToSingle(a3.n1.y / -Desenho.precisaoPixel + Desenho.ponto_zero[1]));
                Gl.glVertex2f(System.Convert.ToSingle(a3.n2.x / Desenho.precisaoPixel + Desenho.ponto_zero[0]), System.Convert.ToSingle(a3.n2.y / -Desenho.precisaoPixel + Desenho.ponto_zero[1]));
                Gl.glEnd();*/
            }

        }
    }

    public class Celula : TObjetoDesenho
    {
        public int bn_j; // nó de contorno anterior
        public int bn_k; // no de contorno posterior
        public int in_j; // nó interno anterior
        public int in_k; // nó interno posterior
        public int id, iperm, istat;
        public bool temp, dentro, sel, deletar, avulso, front, nao_colapsar, mesclou, canto_faltante;
        public NoCelula N1, N2, N3, N4;
        public double area;
        public List<NoCelula> nos;
        public List<double> angulos_internos;
        public ArestaCelula a1, a2, a3, a4;
        public int qtd_arestas_fora;

        public void atuNos()
        {
            nos.Clear();
            nos.Add(N1); nos.Add(N2); nos.Add(N3); nos.Add(N4);

            AngulosInternos();

            for (int i = 0; i < 4; i++)
                angulos_internos[i] = angNo(i);
        }
        public void CalculaArea()
        {
            double aa = 0f;

            List<vec3> pts = new List<vec3>();
            pts.Add(new vec3(N1.x, N1.y, 0));
            pts.Add(new vec3(N2.x, N2.y,0));
            pts.Add(new vec3(N3.x, N3.y,0));
            pts.Add(new vec3(N4.x, N4.y,0));

            for (int i = 0; i < pts.Count; i++)
            {
                vec3 a = pts[i];
                vec3 b = pts[(i + 1) % pts.Count];

                aa += (a.x * b.y) - (b.x * a.y);
            }

            area = Math.Abs(aa) * 0.5;
        }
        public Celula(NoCelula n1, NoCelula n2, NoCelula n3, NoCelula n4, int _id)
        {
            N1 = n1;
            N2 = n2;
            N3 = n3;
            N4 = n4;
            this.id = _id;
            front = true;
            nos = new List<NoCelula>();
            nos.Add(n1); nos.Add(n2); nos.Add(n3); nos.Add(n4);

            angulos_internos = new List<double>() { 0, 0, 0, 0 };

            if (ehHorario(nos))
            {
                NoCelula n1aux, n2aux, n3aux, n4aux;
                n1aux = n4;
                n2aux = n3;
                n3aux = n2;
                n4aux = n1;

                n1 = n1aux;
                n2 = n2aux;
                n3 = n3aux;
                n4 = n4aux;
            }

            AngulosInternos();
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

        public NoCelula retNoOposto(int j, ref int i_noOposto)
        {
            if (j == 0)
            {
                i_noOposto = 2;
                return nos[2];
            }
            else
            if (j == 1)
            {
                i_noOposto = 3;
                return nos[3];
            }

            else
            if (j == 2)
            {
                i_noOposto = 0;
                return nos[0];
            }

            else
            if (j == 3)
            {
                i_noOposto = 1;
                return nos[1];
            }

            return null;
        }

        public int retIndiceNoOposto(int j)
        {
            if (j == 0)
                return 2;
            else
                if (j == 1)
                return 3;
            else
                    if (j == 2)
                return 0;
            else
                        if (j == 3)
                return 1;

            return -1;
        }

        public double angNo(int ni)
        {
            vec3 no_j, no_k, no_i, vetor_u, vetor_v;

            int nj = 0, nk = 0;
            if (ni == 0)
            {
                nj = 3; nk = 1;
            }
            else
            if (ni == 1)
            {
                nj = 0; nk = 2;
            }
            else
            if (ni == 2)
            {
                nj = 1; nk = 3;
            }
            else
            if (ni == 3)
            {
                nj = 2; nk = 0;
            }
            ;

            no_k = new vec3(nos[nk].x, nos[nk].y, 0);

            no_i = new vec3(nos[ni].x, nos[ni].y, 0);

            no_j = new vec3(nos[nj].x, nos[nj].y, 0);

            vetor_u = no_k - no_i;
            vetor_v = no_j - no_i;

            double magnitude = vetor_u.Magnitude() * vetor_v.Magnitude();
            double dot = vetor_u.DotProduct(vetor_v);

            double dot_div_mag = dot / magnitude;
            if (Geom.Iguais(dot_div_mag, -1, 0.00001))
                dot_div_mag = -1;
            double acos = Math.Acos(dot_div_mag);
            double angulo = acos / Const.PIDiv180;

            double x2 = no_k.x - no_i.x; //Vector 1 - x
            double y2 = no_k.y - no_i.y; //Vector 1 - y

            double x1 = no_j.x - no_i.x; //Vector 2 - x
            double y1 = no_j.y - no_i.y; //Vector 2 - y

            double angle = Math.Atan2(y1, x1) - Math.Atan2(y2, x2);
            angle = angle * 360 / (2 * Math.PI);

            if (angle < 0)
                angle += 360;

            angulo = angle;

            return angulo;
        }
        public double angulo_n1, angulo_n2, angulo_n3, angulo_n4;

        public void AngulosInternos()
        {
            angulo_n1 = angNo(0);
            angulo_n2 = angNo(1);
            angulo_n3 = angNo(2);
            angulo_n4 = angNo(3);

            for (int i = 0; i < 4; i++)
                angulos_internos[i] = angNo(i);
        }
        public override void Desenha()
        {
            if (!avulso)
            {
            /*    Gl.glLineWidth(1);
                Gl.glColor3f(0.4f, 0.2f, 0.5f);

                /*    if (avulso)
                    {
                        Gl.glColor3f(0, 1, 1);
                        Gl.glLineWidth(4);
                    }
                    */
                /*   if (istat == 31 || istat == 40 || istat == 130)
                   {
                        Gl.glColor3f(00.3f, 0.5f, 0);
                        Gl.glLineWidth(3);
                   }*/

           /*     if (avulso)
                {
                    Gl.glColor3f(0.5f, 0.5f, 0.1f);
                    Gl.glLineWidth(2);
                }

                /* if (temp)
                 {
                     Gl.glColor3f(0.5f, 0.5f, 0.5f);
                     Gl.glLineWidth(1);
                 }*/
            /*    if (sel)
                {
                    Gl.glColor3f(1, 0, 0);
                    Gl.glLineWidth(6);
                }



                if (N3 != null)
                {
                    Gl.glBegin(Gl.GL_LINE_LOOP);

                    Gl.glVertex2f(System.Convert.ToSingle(N1.x / Desenho.precisaoPixel + Desenho.ponto_zero[0]), System.Convert.ToSingle(N1.y / -Desenho.precisaoPixel + Desenho.ponto_zero[1]));
                    Gl.glVertex2f(System.Convert.ToSingle(N2.x / Desenho.precisaoPixel + Desenho.ponto_zero[0]), System.Convert.ToSingle(N2.y / -Desenho.precisaoPixel + Desenho.ponto_zero[1]));
                    Gl.glVertex2f(System.Convert.ToSingle(N3.x / Desenho.precisaoPixel + Desenho.ponto_zero[0]), System.Convert.ToSingle(N3.y / -Desenho.precisaoPixel + Desenho.ponto_zero[1]));
                    Gl.glVertex2f(System.Convert.ToSingle(N4.x / Desenho.precisaoPixel + Desenho.ponto_zero[0]), System.Convert.ToSingle(N4.y / -Desenho.precisaoPixel + Desenho.ponto_zero[1]));
                    Gl.glEnd();
                }*/
            }
            /*  else
              {
                  Gl.glColor3f(0, 0, 1);
                  Gl.glBegin(Gl.GL_LINE_LOOP);

                  Gl.glVertex2f(System.Convert.ToSingle(N1.x / Desenho.precisaoPixel + Desenho.ponto_zero[0]), System.Convert.ToSingle(N1.y / -Desenho.precisaoPixel + Desenho.ponto_zero[1]));
                  Gl.glVertex2f(System.Convert.ToSingle(N2.x / Desenho.precisaoPixel + Desenho.ponto_zero[0]), System.Convert.ToSingle(N2.y / -Desenho.precisaoPixel + Desenho.ponto_zero[1]));
                  Gl.glVertex2f(System.Convert.ToSingle(N3.x / Desenho.precisaoPixel + Desenho.ponto_zero[0]), System.Convert.ToSingle(N3.y / -Desenho.precisaoPixel + Desenho.ponto_zero[1]));
                  Gl.glVertex2f(System.Convert.ToSingle(N4.x / Desenho.precisaoPixel + Desenho.ponto_zero[0]), System.Convert.ToSingle(N4.y / -Desenho.precisaoPixel + Desenho.ponto_zero[1]));
                  Gl.glEnd();
              }*/

        }
    }

    public class ArestaCelula : TObjetoDesenho
    {
        public NoCelula n1, n2, centro;
        public string estado; //1_1  0_0   1_0   0_1
        public bool sel, avulsa;
        public List<int> celsVizinhas;
        public int id;
        public ArestaCelula()
        {
            n1 = new NoCelula();
            n2 = new NoCelula();
            celsVizinhas = new List<int>();
        }

        public ArestaCelula(NoCelula _n1, NoCelula _n2, int _id)
        {
            n1 = _n1;
            n2 = _n2;
            calculaCentro();
            celsVizinhas = new List<int>();
            id = _id;
        }

        public void calculaCentro()
        {
            centro = new NoCelula(((n1 + n2) / 2).x, ((n1 + n2) / 2).y, 0, 0);
        }

        public override void Desenha()
        {
            /*Gl.glLineWidth(3);
            Gl.glColor3f(0, 1, 0);

            if (sel)
            {
                Gl.glColor3f(1, 0, 0);
                Gl.glLineWidth(4);
                Gl.glBegin(Gl.GL_LINES);
                Gl.glVertex2f(System.Convert.ToSingle(n1.x / Desenho.precisaoPixel + Desenho.ponto_zero[0]), System.Convert.ToSingle(n1.y / -Desenho.precisaoPixel + Desenho.ponto_zero[1]));
                Gl.glVertex2f(System.Convert.ToSingle(n2.x / Desenho.precisaoPixel + Desenho.ponto_zero[0]), System.Convert.ToSingle(n2.y / -Desenho.precisaoPixel + Desenho.ponto_zero[1]));
                Gl.glEnd();
            }*/
        }
    }
    public class NoCelula : TObjetoDesenho
    {

        public double x,
                      y,
                      alfa, alfaOriginal, alfa2;
        public vec3 turn;
        public float px_y,
                     px_x;
        public bool deletar, aresta, contorno_inicial, moveu, no_de_intersecao, eh_mae, n2_wedge, delimitante_front, n4_wedge, renumerar, refazer_nk_nj, nao_alterar_no_k, fora_de_ordem, sel, avulso, completar_celula_wedge, vl, corrigiu_concavidade, wedge;
        public int id, id_aresta, id_celula_completar, status; // se alfa <=135 entao 1, senao 0
        public int no_j, no_k, valencia; //nó anterior e nó posterior na ordem da geração do front
        public int[] nks;
        public int[] njs;
        public bool no_fixo; // nó do contorno original
        public bool no_de_front, ultimo, fim_de_linha;
        public string tipo; // (a) end node; (b) side node; (c) corner node; (d) reversal node
        //end (0 < alfa <= 135)     side (135 < alfa <= 225)      corner (225 < alfa <=315)    reversal (315 < alfa <= 360)//
        public int no_mae; // nó de contorno de onde este se originou

        public List<int> linhas_incidentes;
        public int no_projetado; // nó que foi projetado a partir desse
        public NoCelula Clone()
        {
            NoCelula l = new NoCelula(0, 0, 0, 0);
            l.Copiar(this);
            return l;
        }

        public void Copiar(NoCelula obj)
        {
            id = obj.id;
            tipo = obj.tipo;
            eh_mae = obj.eh_mae;
            id_celula_completar = obj.id_celula_completar;
            alfa = obj.alfa;
            alfa2 = obj.alfa2;
            alfaOriginal = obj.alfaOriginal;
            x = obj.x;
            y = obj.y;
            no_de_front = obj.no_de_front;
            no_fixo = obj.no_fixo;
            no_mae = obj.no_mae;
            no_projetado = obj.no_projetado;
            ultimo = obj.ultimo;
            no_mae = obj.no_mae;
            no_k = obj.no_k;
            no_j = obj.no_j;
            n2_wedge = obj.n2_wedge;
            n4_wedge = obj.n4_wedge;
            renumerar = obj.renumerar;
            refazer_nk_nj = obj.refazer_nk_nj;
            nao_alterar_no_k = obj.nao_alterar_no_k;
            fora_de_ordem = obj.fora_de_ordem;
            sel = obj.sel;
            avulso = obj.avulso;
            completar_celula_wedge = obj.completar_celula_wedge;
            vl = obj.vl;
            corrigiu_concavidade = obj.corrigiu_concavidade;
            wedge = obj.wedge;
            status = obj.status;
            delimitante_front = obj.delimitante_front;
            id_aresta = obj.id_aresta;
        }

        public NoCelula()
        {
            no_j = -1;
            no_k = -1;
            ultimo = false;
            wedge = false;
            vl = false;
            completar_celula_wedge = false;
            n2_wedge = false;
            n4_wedge = false;
            nao_alterar_no_k = false;
            fora_de_ordem = false;
            refazer_nk_nj = false;
            renumerar = false;
            valencia = 0;
            nks = new int[100];
            njs = new int[100];
            eh_mae = false;
            id_aresta = -1;
            for (int i = 0; i < 100; i++)
            {
                nks[i] = -1;
                njs[i] = -1;
            }
        }

        public NoCelula(double x_, double y_, float px_x, float px_y)
        {
            this.x = x_;
            this.y = y_;
            eh_mae = false;
            no_fixo = false;
            base.Selecionado = false;
            this.no_de_front = false;
            this.no_mae = -1;
            corrigiu_concavidade = false;
            ultimo = false;
            wedge = false;
            completar_celula_wedge = false;
            vl = false;
            n2_wedge = false;
            nao_alterar_no_k = false;
            n4_wedge = false;
            refazer_nk_nj = false;
            fora_de_ordem = false;
            renumerar = false;
            valencia = 0;
            nks = new int[100];
            njs = new int[100];
            id_aresta = -1;
            for (int i = 0; i < 100; i++)
            {
                nks[i] = -1;
                njs[i] = -1;
            }
        }
        public NoCelula(double x_, double y_, int _id)
        {
            eh_mae = false;
            ultimo = false;
            this.x = x_;
            this.y = y_;
            no_fixo = false;
            base.Selecionado = false;
            this.no_fixo = false;
            this.id = _id;
            no_j = -1;
            no_k = -1;
            this.no_mae = -1;
            corrigiu_concavidade = false;
            wedge = false;
            completar_celula_wedge = false;
            vl = false;
            n2_wedge = false;
            n4_wedge = false;
            nao_alterar_no_k = false;
            fora_de_ordem = false;
            refazer_nk_nj = false;
            renumerar = false;
            id_aresta = -1;
            valencia = 0;
        }

        public NoCelula(double x_, double y_, float px_x, float px_y, bool nofront, int _id)
        {
            eh_mae = false;
            ultimo = false;
            this.x = x_;
            this.y = y_;
            no_fixo = false;
            base.Selecionado = false;
            this.no_de_front = nofront;
            this.no_fixo = false;
            this.id = _id;
            no_j = -1;
            no_k = -1;
            this.no_mae = -1;
            corrigiu_concavidade = false;
            wedge = false;
            completar_celula_wedge = false;
            vl = false;
            n2_wedge = false;
            n4_wedge = false;
            nao_alterar_no_k = false;
            fora_de_ordem = false;
            refazer_nk_nj = false;
            renumerar = false;
            valencia = 0;
            nks = new int[100];
            njs = new int[100];
            id_aresta = -1;
            for (int i = 0; i < 100; i++)
            {
                nks[i] = -1;
                njs[i] = -1;
            }
        }
        public static NoCelula operator /(NoCelula lhs, double rhs)
        {
            return new NoCelula(lhs.x / rhs, lhs.y / rhs, 0, 0);
        }
        public static NoCelula operator /(NoCelula lhs, NoCelula rhs)
        {
            return new NoCelula(lhs.x / rhs.x, lhs.y / rhs.y, 0, 0);
        }

        public static NoCelula operator +(NoCelula lhs, NoCelula rhs)
        {
            return new NoCelula(lhs.x + rhs.x, lhs.y + rhs.y, 0, 0);
        }

        public static NoCelula operator +(NoCelula lhs, double rhs)
        {
            return new NoCelula(lhs.x + rhs, lhs.y + rhs, 0, 0);
        }

        public static NoCelula operator -(NoCelula lhs, NoCelula rhs)
        {
            return new NoCelula(lhs.x - rhs.x, lhs.y - rhs.y, 0, 0);
        }

        public static NoCelula operator -(NoCelula lhs, double rhs)
        {

            return new NoCelula(lhs.x - rhs, lhs.y - rhs, 0, 0);
        }

        public double Magnitude()
        {
            return Math.Sqrt(x * x + y * y);
        }

        public double DistanciaAte(NoCelula v)
        {
            return (this - v).Magnitude();
        }

        public override void Desenha()
        {

            // Gl.glBegin(Gl.GL_POLYGON);
            //Gl.glColor3f(0, 0, 1);
            /*  if (id_aresta == 0)
                {
                    Gl.glBegin(Gl.GL_POLYGON);
                    // Gl.glColor3f(0, 0, 1);
                    Gl.glColor3f(0, 1, 0);
                    Gl.glVertex2f(System.Convert.ToSingle(x / Desenho.precisaoPixel + Desenho.ponto_zero[0]) - 10, System.Convert.ToSingle(y / -Desenho.precisaoPixel + Desenho.ponto_zero[1]) - 10);
                    Gl.glVertex2f(System.Convert.ToSingle(x / Desenho.precisaoPixel + Desenho.ponto_zero[0]) - 10, System.Convert.ToSingle(y / -Desenho.precisaoPixel + Desenho.ponto_zero[1]) + 10);
                    Gl.glVertex2f(System.Convert.ToSingle(x / Desenho.precisaoPixel + Desenho.ponto_zero[0]) + 10, System.Convert.ToSingle(y / -Desenho.precisaoPixel + Desenho.ponto_zero[1]) + 10);
                    Gl.glVertex2f(System.Convert.ToSingle(x / Desenho.precisaoPixel + Desenho.ponto_zero[0]) + 10, System.Convert.ToSingle(y / -Desenho.precisaoPixel + Desenho.ponto_zero[1]) - 10);
                    Gl.glEnd();

                }
              */
            if (sel)
            {
               /* GL.glBegin(Gl.GL_POLYGON);
             
                Gl.glColor3f(1, 0, 0);
                Gl.glVertex2f(System.Convert.ToSingle(x / Desenho.precisaoPixel + Desenho.ponto_zero[0]) - 2, System.Convert.ToSingle(y / -Desenho.precisaoPixel + Desenho.ponto_zero[1]) - 2);
                Gl.glVertex2f(System.Convert.ToSingle(x / Desenho.precisaoPixel + Desenho.ponto_zero[0]) - 2, System.Convert.ToSingle(y / -Desenho.precisaoPixel + Desenho.ponto_zero[1]) + 2);
                Gl.glVertex2f(System.Convert.ToSingle(x / Desenho.precisaoPixel + Desenho.ponto_zero[0]) + 2, System.Convert.ToSingle(y / -Desenho.precisaoPixel + Desenho.ponto_zero[1]) + 2);
                Gl.glVertex2f(System.Convert.ToSingle(x / Desenho.precisaoPixel + Desenho.ponto_zero[0]) + 2, System.Convert.ToSingle(y / -Desenho.precisaoPixel + Desenho.ponto_zero[1]) - 2);
                Gl.glEnd();
                */
            }
            /*
                        if ((tipo == Const.reversal_node || tipo == Const.corner_node) && alfa > 45 && FuncoesGeometricas.Iguais(alfaOriginal, alfa, 0.01))
                     //   if (/*avulso*/ //alfa > 260 && (alfaOriginal < 0 && alfaOriginal > -89))
                                         // if (alfaOriginal < 0 && (tipo == Const.reversal_node || tipo == Const.corner_node))
            /*   {
                   Gl.glBegin(Gl.GL_POLYGON);
                   // Gl.glColor3f(0, 0, 1);
                   Gl.glColor3f(0, 0, 1);
                   Gl.glVertex2f(System.Convert.ToSingle(x / Desenho.precisaoPixel + Desenho.ponto_zero[0]) - 6, System.Convert.ToSingle(y / -Desenho.precisaoPixel + Desenho.ponto_zero[1]) - 6);
                   Gl.glVertex2f(System.Convert.ToSingle(x / Desenho.precisaoPixel + Desenho.ponto_zero[0]) - 6, System.Convert.ToSingle(y / -Desenho.precisaoPixel + Desenho.ponto_zero[1]) + 6);
                   Gl.glVertex2f(System.Convert.ToSingle(x / Desenho.precisaoPixel + Desenho.ponto_zero[0]) + 6, System.Convert.ToSingle(y / -Desenho.precisaoPixel + Desenho.ponto_zero[1]) + 6);
                   Gl.glVertex2f(System.Convert.ToSingle(x / Desenho.precisaoPixel + Desenho.ponto_zero[0]) + 6, System.Convert.ToSingle(y / -Desenho.precisaoPixel + Desenho.ponto_zero[1]) - 6);
                   Gl.glEnd();
               } */
            /*else
            if ( (alfaOriginal < 0 && alfaOriginal > -89))
            {
                Gl.glBegin(Gl.GL_POLYGON);
                // Gl.glColor3f(0, 0, 1);
                Gl.glColor3f(1, 0, 1);
                Gl.glVertex2f(System.Convert.ToSingle(x / Desenho.precisaoPixel + Desenho.ponto_zero[0]) - 6, System.Convert.ToSingle(y / -Desenho.precisaoPixel + Desenho.ponto_zero[1]) - 6);
                Gl.glVertex2f(System.Convert.ToSingle(x / Desenho.precisaoPixel + Desenho.ponto_zero[0]) - 6, System.Convert.ToSingle(y / -Desenho.precisaoPixel + Desenho.ponto_zero[1]) + 6);
                Gl.glVertex2f(System.Convert.ToSingle(x / Desenho.precisaoPixel + Desenho.ponto_zero[0]) + 6, System.Convert.ToSingle(y / -Desenho.precisaoPixel + Desenho.ponto_zero[1]) + 6);
                Gl.glVertex2f(System.Convert.ToSingle(x / Desenho.precisaoPixel + Desenho.ponto_zero[0]) + 6, System.Convert.ToSingle(y / -Desenho.precisaoPixel + Desenho.ponto_zero[1]) - 6);
                Gl.glEnd();
            }*/

            /*  }
              else
              {
                  Gl.glVertex2f(System.Convert.ToSingle(x / Desenho.precisaoPixel + Desenho.ponto_zero[0]) - 3, System.Convert.ToSingle(y / -Desenho.precisaoPixel + Desenho.ponto_zero[1]) - 3);
                  Gl.glVertex2f(System.Convert.ToSingle(x / Desenho.precisaoPixel + Desenho.ponto_zero[0]) - 3, System.Convert.ToSingle(y / -Desenho.precisaoPixel + Desenho.ponto_zero[1]) + 3);
                  Gl.glVertex2f(System.Convert.ToSingle(x / Desenho.precisaoPixel + Desenho.ponto_zero[0]) + 3, System.Convert.ToSingle(y / -Desenho.precisaoPixel + Desenho.ponto_zero[1]) + 3);
                  Gl.glVertex2f(System.Convert.ToSingle(x / Desenho.precisaoPixel + Desenho.ponto_zero[0]) + 3, System.Convert.ToSingle(y / -Desenho.precisaoPixel + Desenho.ponto_zero[1]) - 3);
                  Gl.glEnd();
              }*/
        }
    }
}
