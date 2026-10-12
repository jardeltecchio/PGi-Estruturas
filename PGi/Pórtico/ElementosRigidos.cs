using MathNet.Numerics;
using MathNet.Numerics.Distributions;
using OpenTK;
using OpenTK.Graphics.OpenGL;
using OpenTK.Input;
using OpenTK.Platform;
using Poly2Tri;
using PolygonCuttingEar;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TriangleNet;
using TriangleNet.Geometry;
using TriangleNet.Meshing;
using TriangleNet.Smoothing;
using TriangleNet.Topology;
using static PG.Geom;

namespace PG
{
    [Serializable]
    public class TDiafragmaRigido : TObjetoDesenho
    {
        public TDiafragmaRigido()
        {
            base.Visivel = true;
            this.Tipo = Const.ID_DIAFRAGMA_RIGIDO;
            this.Selecionado = false;
            base.Selecionado = false;

            ListaComandos = new List<string>();
            ListaComandos.Add(Const.CMD_DIAFRAGMA_RIGIDO_1_P);
            pontosContorno = new List<TPonto>();
        }

        public TDiafragmaRigido(TPonto p1, TPonto p2, TLayer lay, TDadosApoio dados, int pav)
        {
            string ss = "";

            List<TLinha> linhas = new List<TLinha>();
            this.Initialize(ref p1, ref ss, dados, lay, ref linhas, ref pav);
            pontosContorno = new List<TPonto>();

            this.pIni = (TPonto)p1.Clone();
            this.pFin = (TPonto)p2.Clone();

            this.Selecionado = false;
            base.Selecionado = false;
            base.Visivel = true;
            this.Tipo = Const.ID_DIAFRAGMA_RIGIDO;
            this.angulo = (float)(FuncoesGerais.atand((p1.y - p2.y) / (p1.x - p2.x)));
        }
        public override void Initialize(ref TPonto point, ref string command, ISettings Dados, TLayer layer, ref List<TLinha> Linhas, ref int pavimento)
        {
            base.Tipo = Const.ID_DIAFRAGMA_RIGIDO;
            pontosContorno = new List<TPonto>();

            base.pIni = (TPonto)point.Clone();
            pIni = (TPonto)point.Clone();
            base.pFin = (TPonto)point.Clone();
            pFin = (TPonto)point.Clone();

            this.layer = layer;

            base.Initialize(ref point, ref command, Dados, layer, ref Linhas, ref pavimento);
            command = Const.CMD_DIAFRAGMA_RIGIDO_1_P;
            pontosContorno.Add((TPonto)point.Clone());
        }
        public override string PrimeiroComando()
        {
            return Const.CMD_DIAFRAGMA_RIGIDO_1_P;
        }
        public string Comando;
        public List<string> ListaComandos;
        public List<TPonto> pontosContorno;
        [NonSerialized]
        public Plano plano;
        vec3 p1, p2, p3, p4, p5, p6, normal;

        void CriarPlano()
        {
            p1 = new vec3(pontosContorno[0].x, pontosContorno[0].y, pontosContorno[0].z);
            p2 = new vec3(pontosContorno[1].x, pontosContorno[1].y, pontosContorno[1].z);
            p3 = new vec3(pontosContorno[2].x, pontosContorno[2].y, pontosContorno[2].z);

            p4 = p2 - p1;
            p5 = p3 - p1;

            normal = p4.CrossProduct(p5).Normalize();
            normal.z *= -1;
            normal.y *= -1;

            vec3 posicao = (((p1 + p2) / 2) + (p1 + p3) / 2) / 2;
            posicao.z *= -1;
            posicao.y *= -1;

            plano = new Plano(normal, posicao, "", false);
        }

        bool LocalizaPonto(ref List<TPonto> lista, double x, double y, double z)
        {
            for (int jk = 0; jk < lista.Count; jk++)
                if (Geom.Iguais(x, lista[jk].x) && Geom.Iguais(y, lista[jk].y) && Geom.Iguais(z, lista[jk].z))
                    return true;
            return false;
        }

        public void IdentificarDiafragma(List<TPonto> ptsGeometria)
        {
            const double tolerancia = 0.01;
            string erro = "Diafragma " + ID + ": ";
            var pontos = pontosContorno;
            if (pontos == null || pontos.Count < 3)
                throw new InvalidOperationException(erro + "informe pelo menos tres vértices.");
            var vertices = pontos.Select(p => new OpenTK.Vector3d(p.x, p.y, p.z)).ToList();
            if (vertices.Any(p => double.IsNaN(p.X) || double.IsNaN(p.Y) || double.IsNaN(p.Z)
                || double.IsInfinity(p.X) || double.IsInfinity(p.Y) || double.IsInfinity(p.Z)))
                throw new InvalidOperationException(erro + "coordenadas inválidas.");
            if ((vertices[vertices.Count - 1] - vertices[0]).Length <= tolerancia)
                vertices.RemoveAt(vertices.Count - 1);
            if (vertices.Count < 3)
                throw new InvalidOperationException(erro + "contorno insuficiente.");

            var origem = vertices[0];
            
            var u = vertices.Select(p => p - origem).OrderByDescending(p => p.LengthSquared).First();
            
            if (u.Length <= tolerancia)
                throw new InvalidOperationException(erro + "vertices coincidentes.");
            u.Normalize();
           
            // Nao depende dos primeiros tres vertices serem nao colineares.
            var normal = vertices.Select(p => OpenTK.Vector3d.Cross(u, p - origem)).OrderByDescending(p => p.LengthSquared).First();
          
            if (normal.Length <= tolerancia)
                throw new InvalidOperationException(erro + "vertices colineares.");
            normal.Normalize();
            
            var v = OpenTK.Vector3d.Cross(normal, u);

            var poligono = new List<OpenTK.Vector2d>();
            foreach (var ponto in vertices)
            {
                var r = ponto - origem;
                if (Math.Abs(OpenTK.Vector3d.Dot(r, normal)) > tolerancia)
                    throw new InvalidOperationException(erro + "vértices fora do mesmo plano.");
                poligono.Add(new OpenTK.Vector2d(OpenTK.Vector3d.Dot(r, u), OpenTK.Vector3d.Dot(r, v)));
            }

            List<PolygonPoint> ptos_triangulacao = new List<PolygonPoint>();

            int quantidade = poligono.Count;
            double areaDupla = 0, centroU = 0, centroV = 0;
            for (int i = 0; i < quantidade; i++)
            {
                var a = poligono[i];
                var b = poligono[(i + 1) % quantidade];
                if ((b - a).Length <= tolerancia)
                    throw new InvalidOperationException(erro + "vertices consecutivos coincidentes.");
                // Rejeita cruzamentos e contatos entre arestas nao adjacentes.
                for (int j = i + 2; j < quantidade; j++) 
                {
                    if (i == 0 && j == quantidade - 1) continue;
                    var c = poligono[j];
                    var d = poligono[(j + 1) % quantidade];
                    double abC = (b.X - a.X) * (c.Y - a.Y) - (b.Y - a.Y) * (c.X - a.X);
                    double abD = (b.X - a.X) * (d.Y - a.Y) - (b.Y - a.Y) * (d.X - a.X);
                    double cdA = (d.X - c.X) * (a.Y - c.Y) - (d.Y - c.Y) * (a.X - c.X);
                    double cdB = (d.X - c.X) * (b.Y - c.Y) - (d.Y - c.Y) * (b.X - c.X);
                    if (abC * abD <= 0 && cdA * cdB <= 0
                        && Math.Max(a.X, b.X) >= Math.Min(c.X, d.X) && Math.Max(c.X, d.X) >= Math.Min(a.X, b.X)
                        && Math.Max(a.Y, b.Y) >= Math.Min(c.Y, d.Y) && Math.Max(c.Y, d.Y) >= Math.Min(a.Y, b.Y))
                        throw new InvalidOperationException(erro + "contorno com cruzamentos.");
                }
                double cruzado = a.X * b.Y - b.X * a.Y;
                areaDupla += cruzado;
                centroU += (a.X + b.X) * cruzado;
                centroV += (a.Y + b.Y) * cruzado;
            }
            if (Math.Abs(areaDupla) <= tolerancia * tolerancia)
                throw new InvalidOperationException(erro + "área nula ou insuficiente.");
            var centro = origem + u * (centroU / (3 * areaDupla)) + v * (centroV / (3 * areaDupla));
            centroDiafragma = new TPonto(centro.X, centro.Y, centro.Z);
            nos_contorno = new List<TPonto>();

            for (int i = 0; i < ptsGeometria.Count; i++)
            {
                if (LocalizaPonto(ref nos_contorno, ptsGeometria[i].x, ptsGeometria[i].y, ptsGeometria[i].z))
                    continue;

                var ponto = new OpenTK.Vector3d(ptsGeometria[i].x, ptsGeometria[i].y, ptsGeometria[i].z);

                var r = ponto - origem;

                if (Math.Abs(OpenTK.Vector3d.Dot(r, normal)) > tolerancia)
                    continue;

                double x = OpenTK.Vector3d.Dot(r, u);
                double y = OpenTK.Vector3d.Dot(r, v);

                bool dentro = false, borda = false;
                for (int j = 0; j < quantidade; j++)
                {
                    var a = poligono[j];
                    var b = poligono[(j + 1) % quantidade];
                    var ab = b - a;
                    double t = Math.Max(0, Math.Min(1, ((x - a.X) * ab.X + (y - a.Y) * ab.Y) / ab.LengthSquared));
                    double dx = x - a.X - t * ab.X, dy = y - a.Y - t * ab.Y;
                    if (dx * dx + dy * dy <= tolerancia * tolerancia)
                    {
                        borda = true;
                        break;
                    }

                    if ((a.Y > y) != (b.Y > y) && x < a.X + (y - a.Y) * (b.X - a.X) / (b.Y - a.Y))
                        dentro = !dentro;
                }

                if (dentro || borda)
                {
                    Vector2d plano = Geometry.ProjetarNoPlano(new Vector3d(ptsGeometria[i].x, ptsGeometria[i].y, ptsGeometria[i].z),
                                             new Vector3d(origem.X, origem.Y, origem.Z),
                                             u,
                                             v);
                    if (borda)
                      ptos_triangulacao.Add(new PolygonPoint(plano.X, plano.Y));
                    
                    nos_contorno.Add(ptsGeometria[i]);
                }
            }

         /*   tris = new List<TTrianguloDiafragma>();
            TriangulosDiafragma = new Polygon_Poly2Tri(ptos_triangulacao);
            TriangulationContext tcx = new DTSweepContext();
            tcx.PrepareTriangulation(TriangulosDiafragma);
            DTSweep.Triangulate((DTSweepContext)tcx);

            foreach (DelaunayTriangle t in TriangulosDiafragma.Triangles)
            {
                TTrianguloDiafragma tt = new TTrianguloDiafragma();
                for (int j = 0; j < t.Points.Count(); j++)
                {
                    Vector3d pt_3d = Geometry.ReprojetarDoPlano(new Vector2d(t.Points[j].X, t.Points[j].Y),
                                                                new Vector3d(origem.X, origem.Y, origem.Z), u, v);
                    if (j == 0)
                    {
                        tt.p1.x = pt_3d.X; tt.p1.y = pt_3d.Y; tt.p1.z = pt_3d.Z;
                    }
                    if (j == 1)
                    {
                        tt.p2.x = pt_3d.X; tt.p2.y = pt_3d.Y; tt.p2.z = pt_3d.Z;
                    }
                    if (j == 2)
                    {
                        tt.p3.x = pt_3d.X; tt.p3.y = pt_3d.Y; tt.p3.z = pt_3d.Z;
                    }
                    //    correspondencia_local_global.Add(new uv_XYZ(plano.X, plano.Y, ptsGeometria[i].x, ptsGeometria[i].y, ptsGeometria[i].z));
                }
                tris.Add(tt);
            }*/

        }

        List<TTrianguloDiafragma> tris;

        class TTrianguloDiafragma
        {
            public TPonto p1;
            public TPonto p2;
            public TPonto p3;
            public TTrianguloDiafragma()
            {
                p1 = new TPonto(0, 0, 0);
                p2 = new TPonto(0, 0, 0);
                p3 = new TPonto(0, 0, 0);
            }
        }
        public List<TPonto> nos_contorno;

        public TPonto centroDiafragma;
        public override eObjetoDesenhoMouseDown OnMouseDown(ref TPonto point, ref string command, List<TLinha> Linhas, List<TPonto> Pontos, List<TPilar> Pilares, ref List<TObjetoDesenho> Objects, bool FromGrip = false)
        {
            if (command == "finalizar")
            {
                if (!Geom.Iguais(pontosContorno[pontosContorno.Count-1].x, pontosContorno[0].x) 
                 || !Geom.Iguais(pontosContorno[pontosContorno.Count-1].y, pontosContorno[0].y)
                 || !Geom.Iguais(pontosContorno[pontosContorno.Count-1].z, pontosContorno[0].z))
                {
                    pontosContorno.Add((TPonto)pontosContorno[0].Clone());
                }

                CriarPlano();

                return eObjetoDesenhoMouseDown.DoneRepeat;

            }
            else
            {
                if ((Geom.Iguais(point.x, pIni.x)) && (Geom.Iguais(point.y, pIni.y)) && (Geom.Iguais(point.z, pIni.z)))
                {
                    pontosContorno.Add((TPonto)point.Clone());

                    CriarPlano();

                    return eObjetoDesenhoMouseDown.DoneRepeat;
                }

                NaoPermiteMoverOuCopiar = false;

                pFin = (TPonto)point.Clone();
                //    }
                if (Geom.Iguais(pFin.x, 0))
                    pFin.x = 0;
                if (Geom.Iguais(pFin.y, 0))
                    pFin.y = 0;
                if (Geom.Iguais(pFin.z, 0))
                    pFin.z = 0;

                if (Geom.Iguais(pIni.x, 0))
                    pIni.x = 0;
                if (Geom.Iguais(pIni.y, 0))
                    pIni.y = 0;
                if (Geom.Iguais(pIni.z, 0))
                    pIni.z = 0;

                command = Const.CMD_DIAFRAGMA_RIGIDO_1_P;

                this.comprimento = (float)pFin.DistanceTo(pIni);

                TPonto pp = (TPonto)point.Clone();
                pontosContorno.Add(new TPonto(pp.x, pp.y, pp.z));

                return eObjetoDesenhoMouseDown.Continue;
            }
        }

        public override void Desenha(ref bool Unifilar, ref int transp, ref bool arestas)
        {
            try
            {
                GL.Color3(System.Drawing.Color.FromArgb(0, 0, 200));

               /* GL.Begin(PrimitiveType.Polygon);
                for (int i = 0; i < pontosContorno.Count; i++)
                    GL.Vertex3(pontosContorno[i].x, pontosContorno[i].y, pontosContorno[i].z);
                GL.End();*/

              //  GL.Color4(System.Drawing.Color.FromArgb(200, 0, 200, 100));
                GL.Begin(PrimitiveType.LineLoop);
                for (int i = 0; i < pontosContorno.Count; i++)
                    GL.Vertex3(pontosContorno[i].x, pontosContorno[i].y, pontosContorno[i].z);
                GL.End();
            }
            catch (Exception ms)
            {
            }
        }
        public override void SetaSelecao(bool s, bool MostraGrip, bool SelecaoDeCandidato = false, TObjetoDesenho Owner = null)
        {
            base.Selecionado = false;
            if (!this.Visivel)
                return;

            base.Selecionado = s;
            DirtyArestas = true;
        }
        public override bool Selecionar(float clicx, float clicy, float coordx, float coordy, TObjetoDesenho Owner = null)
        {
            try
            {
                if (!this.Visivel)
                    return false;

                Vector3d p1 = new Vector3d(pIni.x, pIni.y, pIni.z);
                Vector3d p2 = new Vector3d(pFin.x, pFin.y, pFin.z);

                double x1 = 0, y1 = 0, x2 = 0, y2 = 0;

                if (!Clipper3D.ProjetarLinhaSelecao(FPrincipal.View_x_Proj, FPrincipal.cameraPerspectiva, p1, p2, out x1, out y1, out x2, out y2))
                    return false;

                if (Geom.PontoEmLinha2(clicx, clicy, x1, y1, x2, y2, 0.05))
                {
                    DirtyArestas = true;
                    SetaSelecao(true, true);
                    return base.Selecionado;
                }

                return false;
            }
            catch (Exception)
            {

            }
            return false;
        }

        double comprimento;
        public override void Proximo()
        {

            base.Proximo();
        }
        public void Copy(TBracoRigido obj)
        {
            base.Copy(obj);

            if ((Object)obj.pIni != null)
                pIni = (TPonto)obj.pIni.Clone();
            Tipo = obj.Tipo;
            IdLayer = obj.IdLayer;
            Layer = obj.Layer;
            layer = obj.layer;
            Visivel = obj.Visivel;

            ObjetoCopia = obj.ObjetoCopia;
        }

        public override TObjetoDesenho Clone()
        {
            TDiafragmaRigido l = new TDiafragmaRigido();
            l.Copy(this);
            return l;
        }
        [NonSerialized]
        public float[] BatchArestas;
        public bool DirtySelecao = true;

        public bool DirtyArestas = true;
        public bool DirtySelecaoArestas = true;
        public void AtualizaArestas()
        {
            List<float> coords = new List<float>();

            Preenche_Arestas(ref coords);

            BatchArestas = coords.ToArray();

            DirtyArestas = false;
        }
        [NonSerialized]
        double r, g, b;
        public void Preenche_Arestas(ref List<float> coords_arestas)
        {
            if (Visivel)
            {
                if (Selecionado)
                {
                    r = 1;
                    g = 0;
                    b = 0;
                }
                else
                {
                    r = 0;
                    g = 0;
                    b = 1;
                }

              /*  for (int i = 0; i < pontosContorno.Count - 1; i++)
                {
                    setLista(ref coords_arestas, pontosContorno[i].x, pontosContorno[i].y, pontosContorno[i].z);
                    setLista(ref coords_arestas, r, g, b);
                    setLista(ref coords_arestas, pontosContorno[i + 1].x, pontosContorno[i + 1].y, pontosContorno[i + 1].z);
                    setLista(ref coords_arestas, r, g, b);
                }*/

                for (int i = 0; i < nos_contorno.Count; i++)
                {
                    setLista(ref coords_arestas, nos_contorno[i].x, nos_contorno[i].y, nos_contorno[i].z);
                    setLista(ref coords_arestas, r, g, b);
                    setLista(ref coords_arestas, centroDiafragma.x, centroDiafragma.y, centroDiafragma.z);
                    setLista(ref coords_arestas, r, g, b);
                }

                if (Selecionado)
                {
                   for (int i = 0; i < pontosContorno.Count - 1; i++)
                   {
                       setLista(ref coords_arestas, pontosContorno[i].x, pontosContorno[i].y, pontosContorno[i].z);
                       setLista(ref coords_arestas, r, g, b);
                       setLista(ref coords_arestas, pontosContorno[i + 1].x, pontosContorno[i + 1].y, pontosContorno[i + 1].z);
                       setLista(ref coords_arestas, r, g, b);
                   }
                }
            }
            GL.LineWidth(1);
        }
        void setLista(ref List<float> coord_objeto, double x, double y, double z)
        {
            coord_objeto.Add((float)x);
            coord_objeto.Add((float)y);
            coord_objeto.Add((float)z);
        }
    }
    [Serializable]
    public class TBracoRigido: TObjetoDesenho
    {
        public TBracoRigido()
        {
            base.Visivel = true;
            this.Tipo = Const.ID_BRACO_RIGIDO;
            this.Selecionado = false;
            base.Selecionado = false;

            ListaComandos = new List<string>();
            ListaComandos.Add(Const.CMD_BRACORIGIDO_1_P);
            ListaComandos.Add(Const.CMD_BRACORIGIDO_2_P);
        }

        public TBracoRigido(TPonto p1, TPonto p2, TLayer lay, TDadosApoio dados, int pav)
        {
            string ss = "";

            List<TLinha> linhas = new List<TLinha>();
            this.Initialize(ref p1, ref ss, dados, lay, ref linhas, ref pav);
     
            this.pIni = (TPonto)p1.Clone();
            this.pFin = (TPonto)p2.Clone();

            this.Selecionado = false;
            base.Selecionado = false;
            base.Visivel = true;
            this.Tipo = Const.ID_BRACO_RIGIDO;
            this.angulo = (float)(FuncoesGerais.atand((p1.y - p2.y) / (p1.x - p2.x)));
        }
        public override void Initialize(ref TPonto point, ref string command, ISettings Dados, TLayer layer, ref List<TLinha> Linhas, ref int pavimento)
        {
            base.Tipo = Const.ID_BRACO_RIGIDO;

            base.pIni = (TPonto)point.Clone();
            pIni = (TPonto)point.Clone();
            base.pFin = (TPonto)point.Clone();
            pFin = (TPonto)point.Clone();

            this.layer = layer;

            base.Initialize(ref point, ref command, Dados, layer, ref Linhas, ref pavimento);
            command = Const.CMD_BRACORIGIDO_2_P;
        }
        public override string PrimeiroComando()
        {
            return Const.CMD_BRACORIGIDO_1_P;
        }
        public string Comando;
        public List<string> ListaComandos;
        public override eObjetoDesenhoMouseDown OnMouseDown(ref TPonto point, ref string command, List<TLinha> Linhas, List<TPonto> Pontos, List<TPilar> Pilares, ref List<TObjetoDesenho> Objects, bool FromGrip = false)
        {
                if ((Geom.Iguais(point.x, pIni.x)) && (Geom.Iguais(point.y, pIni.y)) && (Geom.Iguais(point.z, pIni.z)))
                {
                    return eObjetoDesenhoMouseDown.Continue;
                }
                NaoPermiteMoverOuCopiar = false;

                pFin = (TPonto)point.Clone();
                //    }
                if (Geom.Iguais(pFin.x, 0))
                    pFin.x = 0;
                if (Geom.Iguais(pFin.y, 0))
                    pFin.y = 0;
                if (Geom.Iguais(pFin.z, 0))
                    pFin.z = 0;

                if (Geom.Iguais(pIni.x, 0))
                    pIni.x = 0;
                if (Geom.Iguais(pIni.y, 0))
                    pIni.y = 0;
                if (Geom.Iguais(pIni.z, 0))
                    pIni.z = 0;

                this.comprimento = (float)pFin.DistanceTo(pIni);

                return eObjetoDesenhoMouseDown.Done;

        }
        public override void SetaSelecao(bool s, bool MostraGrip, bool SelecaoDeCandidato = false, TObjetoDesenho Owner = null)
        {
            base.Selecionado = false;
            if (!this.Visivel)
                return;

            base.Selecionado = s;
            DirtyArestas = true;
        }
        public override bool Selecionar(float clicx, float clicy, float coordx, float coordy, TObjetoDesenho Owner = null)
        {
            try
            {
                if (!this.Visivel)
                    return false;

                Vector3d p1 = new Vector3d(pIni.x, pIni.y, pIni.z);
                Vector3d p2 = new Vector3d(pFin.x, pFin.y, pFin.z);

                double x1 = 0, y1 = 0, x2 = 0, y2 = 0;

                if (!Clipper3D.ProjetarLinhaSelecao(FPrincipal.View_x_Proj, FPrincipal.cameraPerspectiva, p1, p2, out x1, out y1, out x2, out y2))
                    return false;

                if (Geom.PontoEmLinha2(clicx, clicy, x1, y1, x2, y2, 0.05))
                {
                    DirtyArestas = true;
                    SetaSelecao(true, true);
                    return base.Selecionado;
                }

                return false;
            }
            catch (Exception)
            {

            }
            return false;
        }

        double comprimento;
        public override void Proximo()
        {

            base.Proximo();
        }
        public void Copy(TBracoRigido obj)
        {
            base.Copy(obj);

            if ((Object)obj.pIni != null)
                pIni = (TPonto)obj.pIni.Clone();
            Tipo = obj.Tipo;
            IdLayer = obj.IdLayer;
            Layer = obj.Layer;
            layer = obj.layer;
            Visivel = obj.Visivel;

            ObjetoCopia = obj.ObjetoCopia;
        }

        public override TObjetoDesenho Clone()
        {
            TBracoRigido l = new TBracoRigido();
            l.Copy(this);
            return l;
        }
        [NonSerialized]
        public float[] BatchArestas;
        public bool DirtySelecao = true;

        public bool DirtyArestas = true;
        public bool DirtySelecaoArestas = true;
        public void AtualizaArestas()
        {
            List<float> coords = new List<float>();

            Preenche_Arestas(ref coords);

            BatchArestas = coords.ToArray();

            DirtyArestas = false;
        }
        [NonSerialized]
        double r, g, b;
        public void Preenche_Arestas(ref List<float> coords_arestas)
        {
            if (Visivel)
            {
                if (Selecionado)
                {
                    r = 1;
                    g = 0;
                    b = 0;
                }
                else
                {
                    r = 0;
                    g = 0;
                    b = 1;
                }

                //  if (unifilar || Dados.Tipo == 4)
                {
                    setLista(ref coords_arestas, pIni.x, pIni.y, pIni.z);
                    setLista(ref coords_arestas, r, g, b);
                    setLista(ref coords_arestas, pFin.x, pFin.y, pFin.z);
                    setLista(ref coords_arestas, r, g, b);

                }
            }
            GL.LineWidth(1);
        }
        void setLista(ref List<float> coord_objeto, double x, double y, double z)
        {
            coord_objeto.Add((float)x);
            coord_objeto.Add((float)y);
            coord_objeto.Add((float)z);
        }
    }
}
