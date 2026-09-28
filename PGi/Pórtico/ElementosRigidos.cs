using MathNet.Numerics.Distributions;
using OpenTK;
using OpenTK.Graphics.OpenGL;
using OpenTK.Input;
using OpenTK.Platform;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

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

                pontosContorno.Add((TPonto)point.Clone());

                return eObjetoDesenhoMouseDown.Continue;
            }
        }

        public override void Desenha(ref bool Unifilar, ref int transp, ref bool arestas)
        {
            try
            {
                GL.Color4(System.Drawing.Color.FromArgb(50, 0, 200, 100));

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

                for (int i = 0; i < pontosContorno.Count-1; i++)
                {
                    int next = (i + 1) % pontosContorno.Count;

                    setLista(ref coords_arestas, pontosContorno[i].x, pontosContorno[i].y, pontosContorno[i].z);
                    setLista(ref coords_arestas, r, g, b);
                    setLista(ref coords_arestas, pontosContorno[i+1].x, pontosContorno[i+1].y, pontosContorno[i+1].z);
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
