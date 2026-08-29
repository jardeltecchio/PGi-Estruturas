using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using System.Windows.Forms;
using System.Drawing;
using System.Drawing.Drawing2D;
using GeometryUtility;
using PolygonCuttingEar;
using System.IO;
using System.Drawing.Imaging;

using OpenTK;
using OpenTK.Input;
using OpenTK.Graphics.OpenGL;
using OpenTK.Platform;

namespace PG
{
    [Serializable]

    public class TTrechoViga : TObjetoDesenho
    {
        public TLinha BarraRigida_Ini;

        public TLinha BarraRigida_Fin;

        public double xIni, yIni, zIni; /*coordenadas do eixo da seção*/
        public double xFin, yFin, zFin;
        public int face_insercao;
        public double x1, y1,z1, x2, y2, z2, x3, y3,z3, x4, y4,z4, areaSuperior;
        public double angulo,ang2, comprimento, PP;
        public Linha faceCima, faceBaixo, eixo;
        public Linha linha_face1, linha_face2;
        public TLinha linhas_facecima, linhas_facebaixo, linhas_eixo;
        public TLinha linhas_facecima_org, linhas_facebaixo_org, linhas_eixo_org;
        public TLinha linhas_facecima_longa, linhas_facebaixo_longa, linhas_eixo_longa;
        public TLinha linhas_facecima_longa2, linhas_facebaixo_longa2;
        public TLinha linhas_facecima_longa_m1, linhas_facebaixo_longa_m1;
        public TLinha linhas_faceeixo_longa2, linhas_faceeixo_longa;
        public TLinha linhas_facecima_longa_m2, linhas_facebaixo_longa_m2;
        public TLinha linha_eixo_aux;
        public TLinha linhas_faceeixo_longa_m1, linhas_faceeixo_longa_m2;
        public int NumPilar_PontoInicial;
        public int NumPilar_PontoFinal;
        public TTexto Texto1, Texto2, TextoInfo;
        public TDadosViga Dados;

        public int Pavimento;

        public TTrechoViga(TDadosViga dados = null) 
        {
            base.Visivel       = true;
            base.IdLayer       = Lay.Vigas;
            this.Tipo          = Const.ID_TRECHOVIGA;
            this.face_insercao = 1;
            this.Dados = dados;
            this.Selecionado = false;
            base.Selecionado = false;
        }
       
        public TTrechoViga(TPonto p1, TPonto p2, TDadosViga dados, TLayer lay, int pav , List<TLinha> Linhas, bool comFace = false)
        {
            string ss = "";
            double nx, ny;
            double beta;
             //    p1 = new TPonto(PontoInicial.x, PontoInicial.y, Desenho.pixelX(PontoInicial.x), Desenho.pixelY(PontoInicial.y), -1);
      //      p2 = new TPonto(point.x, point.y, Desenho.pixelX(point.x), Desenho.pixelY(point.y), -1);
            this.pIni = (TPonto)p1.Clone();
            this.pFin = (TPonto)p2.Clone();
            this.Initialize(ref p1, ref ss, dados, lay, ref Linhas, ref pav);
            this.Selecionado = false;
            base.Selecionado = false;
            base.Visivel = true;
            base.IdLayer = Lay.Vigas;
            this.Tipo    = Const.ID_TRECHOVIGA;
            this.angulo  = (float)(FuncoesGerais.atand((p1.y - p2.y) / (p1.x - p2.x)));


            beta = this.angulo;
            if (p2.x >= p1.x)
                beta -= 90;
            else
                beta += 90;

            nx = (float)((Dados.b1) * Math.Cos(beta * Const.PIDiv180));
            ny = (float)((Dados.b1) * Math.Sin(beta * Const.PIDiv180));

            if ((beta == 0 && pFin.y < pIni.y) || (beta == -180 && pFin.y > pIni.y))
               nx = -nx;

            this.comprimento = (float)pFin.DistanceTo(pIni);

            this.SetaCoords(this, comFace ? this.Dados.face_insercao : 1, p1, p2, ref nx, ref ny);
            

            this.linhas_facecima.UpdatePixel();
            this.linhas_facebaixo.UpdatePixel();
            this.linhas_eixo.UpdatePixel();

            if (!this.pFin.Incidente(this.linhas_eixo))
                this.pFin.incidencias.Add(this.linhas_eixo);

            if (!this.pIni.Incidente(this.linhas_eixo))
                this.pIni.incidencias.Add(this.linhas_eixo);

            this.linhas_facebaixo.angulo = (float)this.angulo;
            this.linhas_eixo.angulo = (float)this.angulo;
            this.linhas_facecima.angulo = (float)this.angulo;

            this.linhas_eixo.pIni.PontoEixoViga = true;
            this.linhas_eixo.pFin.PontoEixoViga = true;

            this.linhas_facebaixo.pIni.PontoEixoViga = false;
            this.linhas_facebaixo.pFin.PontoEixoViga = false;

            this.linhas_facecima.pIni.PontoEixoViga = false;
            this.linhas_facecima.pFin.PontoEixoViga = false;

            this.linhas_facebaixo_org = new TLinha(new TPonto(this.linhas_facebaixo.pIni.x, this.linhas_facebaixo.pIni.y, this.linhas_facebaixo.pIni.z),
                                                  new TPonto(this.linhas_facebaixo.pFin.x, this.linhas_facebaixo.pFin.y, this.linhas_facebaixo.pFin.z),
                                                  -1);
            this.linhas_eixo_org = new TLinha(new TPonto(this.linhas_eixo.pIni.x, this.linhas_eixo.pIni.y, this.linhas_eixo.pIni.z),
                                                  new TPonto(this.linhas_eixo.pFin.x, this.linhas_eixo.pFin.y, this.linhas_eixo.pFin.z),
                                                  -1);

            this.linhas_facecima_org = new TLinha(new TPonto(this.linhas_facecima.pIni.x, this.linhas_facecima.pIni.y, this.linhas_facecima.pIni.z),
                                                  new TPonto(this.linhas_facecima.pFin.x, this.linhas_facecima.pFin.y, this.linhas_facecima.pFin.z),
                                                  -1);
            this.AddGrips();

            this.UpdateTodas(this);

            this.UpdatePixelTodas(this);

            this.CriaTextos(this, this.Dados.b1 + "/" + this.Dados.h1, this.Dados.nome + this.Dados.numero);         
        }

        public TTrechoViga(string tipo, int registro, bool selecionado) : base(tipo, registro, selecionado) { }
      
        public override string PrimeiroComando()
        {
            return Const.CMD_TRECHOVIGA_1_P;
        }

        public override bool Command(Keys key)
        {
            if (key == Keys.A)
            {
                AlternaFace();
                base.Command(key);
                return true;
            }

            return false;
        }
        const double tam = 0.628318;
        public bool ComVigaFaceFinal, ComVigaFaceInicial;

        public override void Proximo()
        {

            base.Proximo();
        }

        public override void Atualiza(int pavimento)
        {
            //recria os vertices, pois pode ter havido rotação das barras...
            this.Pavimento = pavimento;
            this.AddGrips();
            this.comprimento = (float)pFin.DistanceTo(pIni);
            base.angulo = (float)(FuncoesGerais.atand((pIni.y - pFin.y) / (pIni.x - pFin.x)));
            base.anguloGlobal = (float)RMath.rad2deg(pIni.getAngleTo(pFin));
            this.angulo = base.angulo;

            this.UpdateTodas(this);

            this.UpdatePixelTodas(this);

            this.CriaTextos(this, this.Dados.b1 + "/" + this.Dados.h1, this.Dados.nome + this.Dados.numero);

            TriangularizaFace();

            ReposicionaTextos();
        }
        [NonSerialized]
        vec3 p1_Rotacao = new vec3(0, 0, 0);
        [NonSerialized]
        vec3 Centro = new vec3(0, 0, 0);
        double anguloAnterior = 0;
        public override void Mover(ref TPonto ponto1,ref TPonto ponto2, bool dinamico)
        {
           /* ponto1.x = 0;
            ponto1.y = 0;
            ponto1.z = 0;

            ponto2.x = 200;
            ponto2.y = 200;
            ponto2.z = 200;
            */
            pIni.Mover(ref ponto1, ref ponto2, dinamico);
            pFin.Mover(ref ponto1, ref ponto2, dinamico);

            linhas_facecima.Mover(ref ponto1, ref ponto2, dinamico);
            linhas_facebaixo.Mover(ref ponto1, ref ponto2, dinamico);
            linhas_eixo.Mover(ref ponto1, ref ponto2, dinamico);
            linhas_facecima_org.Mover(ref ponto1, ref ponto2, dinamico);
            linhas_facebaixo_org.Mover(ref ponto1, ref ponto2, dinamico);
            linhas_eixo_org.Mover(ref ponto1, ref ponto2, dinamico);
            linhas_facecima_longa.Mover(ref ponto1, ref ponto2, dinamico);
            linhas_facebaixo_longa.Mover(ref ponto1, ref ponto2, dinamico);

            if (linhas_eixo_longa != null)
                linhas_eixo_longa.Mover(ref ponto1, ref ponto2, dinamico);

            linhas_facecima_longa2.Mover(ref ponto1, ref ponto2, dinamico);
            linhas_facebaixo_longa2.Mover(ref ponto1, ref ponto2, dinamico);
            linhas_facecima_longa_m1.Mover(ref ponto1, ref ponto2, dinamico);
            linhas_facebaixo_longa_m1.Mover(ref ponto1, ref ponto2, dinamico);
            linhas_faceeixo_longa2.Mover(ref ponto1, ref ponto2, dinamico);
            linhas_faceeixo_longa.Mover(ref ponto1, ref ponto2, dinamico);
            linhas_facecima_longa_m2.Mover(ref ponto1, ref ponto2, dinamico);
            linhas_facebaixo_longa_m2.Mover(ref ponto1, ref ponto2, dinamico);
            linha_eixo_aux.Mover(ref ponto1, ref ponto2, dinamico);
            linhas_faceeixo_longa_m1.Mover(ref ponto1, ref ponto2, dinamico);
            linhas_faceeixo_longa_m2.Mover(ref ponto1, ref ponto2, dinamico);
        }

        public override void Rotacionar(TPonto ponto1, TPonto ponto2, double ang)
        {
           /* Centro.x = ponto1.x;
            Centro.y = ponto1.y;
            p1_Rotacao.x = pIni.x;
            p1_Rotacao.y = pIni.y;
            p1_Rotacao = p1_Rotacao.Rotate(Centro, (ang - anguloAnterior) * Const.PIDiv180);
            pIni.x = p1_Rotacao.x;
            pIni.y = p1_Rotacao.y;

            p1_Rotacao.x = pFin.x;
            p1_Rotacao.y = pFin.y;
            p1_Rotacao = p1_Rotacao.Rotate(Centro, (ang - anguloAnterior) * Const.PIDiv180);
            pFin.x = p1_Rotacao.x;
            pFin.y = p1_Rotacao.y;*/


            pIni = pIni.Rotate(ponto1, (ang - anguloAnterior) * Const.PIDiv180);
            pFin = pFin.Rotate(ponto1, (ang - anguloAnterior) * Const.PIDiv180);


            linhas_facecima.Rotacionar(ponto1, ponto2, ang);
            linhas_facebaixo.Rotacionar(ponto1, ponto2, ang);
            linhas_eixo.Rotacionar(ponto1, ponto2, ang);
            linhas_facecima_org.Rotacionar(ponto1, ponto2, ang);
            linhas_facebaixo_org.Rotacionar(ponto1, ponto2, ang);
            linhas_eixo_org.Rotacionar(ponto1, ponto2, ang);
            linhas_facecima_longa.Rotacionar(ponto1, ponto2, ang);
            linhas_facebaixo_longa.Rotacionar(ponto1, ponto2, ang);

            if (linhas_eixo_longa != null)
              linhas_eixo_longa.Rotacionar(ponto1, ponto2, ang);
  
            linhas_facecima_longa2.Rotacionar(ponto1, ponto2, ang);
            linhas_facebaixo_longa2.Rotacionar(ponto1, ponto2, ang);
            linhas_facecima_longa_m1.Rotacionar(ponto1, ponto2, ang);
            linhas_facebaixo_longa_m1.Rotacionar(ponto1, ponto2, ang);
            linhas_faceeixo_longa2.Rotacionar(ponto1, ponto2, ang);
            linhas_faceeixo_longa.Rotacionar(ponto1, ponto2, ang);
            linhas_facecima_longa_m2.Rotacionar(ponto1, ponto2, ang);
            linhas_facebaixo_longa_m2.Rotacionar(ponto1, ponto2, ang);
            linha_eixo_aux.Rotacionar(ponto1, ponto2, ang);
            linhas_faceeixo_longa_m1.Rotacionar(ponto1, ponto2, ang);
            linhas_faceeixo_longa_m2.Rotacionar(ponto1, ponto2, ang);

            anguloAnterior = ang;
        }
        public double transparencia = 200;
        public override void Desenha(ref bool Unifilar, ref int transp,ref  bool arestas)
        {
            if (Visivel)
            {
                try
                {
                   /* if (NumPilar_PontoInicial > 0)
                        GL.Color4(Color.FromArgb(transp, this.Estrutura.LayersByIdPrincipal[Lay.Pilares].Rgb[0], this.Estrutura.LayersByIdPrincipal[Lay.Pilares].Rgb[1], this.Estrutura.LayersByIdPrincipal[Lay.Pilares].Rgb[2]));
                    */
                    /* GL.Begin(PrimitiveType.Lines);
                     for (j = 0; j <= 7; j++)
                         GL.Vertex3(linhas_eixo.pIni.x + (1 * Math.Cos(j * 6.2831 / 7)), linhas_eixo.pIni.y + (1 * Math.Sin(j * 6.2831 / 7)),
                                    linhas_eixo.pIni.x + (1 * Math.Cos((j + 1) * 6.2831 / 7)), linhas_eixo.pIni.y + (1 * Math.Sin((j + 1) * 6.2831 / 7)));
                     */

                  /*  if (NumPilar_PontoFinal > 0)
                        GL.Color4(Color.FromArgb(transp, this.Estrutura.LayersByIdPrincipal[Lay.Pilares].Rgb[0], this.Estrutura.LayersByIdPrincipal[Lay.Pilares].Rgb[1], this.Estrutura.LayersByIdPrincipal[Lay.Pilares].Rgb[2]));
                    else
                        GL.Color4(Color.FromArgb(transp, this.layer.Rgb[0], this.layer.Rgb[1], this.layer.Rgb[2]));
*/
                    /* for (j = 0; j <= 7; j++)
                         Cad.DrawLine(mPen, Desenho.pixelX((linhas_eixo.pFin.x + (1 * Math.Cos(j * 6.2831 / 7)))), Desenho.pixelY((linhas_eixo.pFin.y + (1 * Math.Sin(j * 6.2831 / 7)))),
                                              Desenho.pixelX((linhas_eixo.pFin.x + (1 * Math.Cos((j + 1) * 6.2831 / 7)))), Desenho.pixelY((linhas_eixo.pFin.y + (1 * Math.Sin((j + 1) * 6.2831 / 7)))));
                     */

                    if (base.Selecionado)
                    {
                      /*  if (base.MostrarGrip)
                            foreach (TGrip gr in base.Grips)
                                gr.Desenha(ref Cad);*/

                        if (CandidatoSelecao)
                            mPen.Color = Color.Orange;
                        else
                            mPen.Color = Color.Red;

                        GL.Color3(Color.Red);
                        //GL.LineWidth(2);
                    }
                    else
                       GL.Color3(Color.Black);
        
                    if (Unifilar)
                    {
                        if (!base.Selecionado)
                          GL.Color4(Color.FromArgb(transp, this.layer.Rgb[0], this.layer.Rgb[1], this.layer.Rgb[2]));
                            
                        GL.Begin(PrimitiveType.Lines);
                        GL.Vertex3(linhas_eixo.pIni.x, linhas_eixo.pIni.y, linhas_eixo.pIni.z);
                        GL.Vertex3(linhas_eixo.pFin.x, linhas_eixo.pFin.y, linhas_eixo.pFin.z);
                        GL.End();
                    }
                    else
                    {
                        if (arestas || Selecionado)
                        {
                            GL.Begin(PrimitiveType.Lines);
                            GL.Vertex3(linhas_eixo.pIni.x, linhas_eixo.pIni.y, linhas_eixo.pIni.z);
                            GL.Vertex3(linhas_eixo.pFin.x, linhas_eixo.pFin.y, linhas_eixo.pFin.z);
                            GL.End(); 
                            
                            GL.Begin(PrimitiveType.LineLoop);
                            GL.Vertex3(linhas_facecima.pIni.x, linhas_facecima.pIni.y, linhas_facecima.pIni.z);
                            GL.Vertex3(linhas_facecima.pFin.x, linhas_facecima.pFin.y, linhas_facecima.pFin.z);
                            GL.Vertex3(linhas_facecima.pFin.x, linhas_facecima.pFin.y, linhas_facecima.pFin.z + Dados.h1);
                            GL.Vertex3(linhas_facecima.pIni.x, linhas_facecima.pIni.y, linhas_facecima.pIni.z + Dados.h1);
                            GL.End();

                            GL.Begin(PrimitiveType.LineLoop);
                            GL.Vertex3(linhas_facebaixo.pIni.x, linhas_facebaixo.pIni.y, linhas_facebaixo.pIni.z);
                            GL.Vertex3(linhas_facebaixo.pFin.x, linhas_facebaixo.pFin.y, linhas_facebaixo.pFin.z);
                            GL.Vertex3(linhas_facebaixo.pFin.x, linhas_facebaixo.pFin.y, linhas_facebaixo.pFin.z + Dados.h1);
                            GL.Vertex3(linhas_facebaixo.pIni.x, linhas_facebaixo.pIni.y, linhas_facebaixo.pIni.z + Dados.h1);
                            GL.End();

                            GL.Begin(PrimitiveType.LineLoop);
                            GL.Vertex3(linhas_facecima.pIni.x, linhas_facecima.pIni.y, linhas_facecima.pIni.z);
                            GL.Vertex3(linhas_facebaixo.pIni.x, linhas_facebaixo.pIni.y, linhas_facebaixo.pIni.z);
                            GL.Vertex3(linhas_facebaixo.pFin.x, linhas_facebaixo.pFin.y, linhas_facebaixo.pFin.z);
                            GL.Vertex3(linhas_facecima.pFin.x, linhas_facecima.pFin.y, linhas_facecima.pFin.z);
                            GL.End();

                            GL.Begin(PrimitiveType.LineLoop);
                            GL.Vertex3(linhas_facecima.pIni.x, linhas_facecima.pIni.y, linhas_facecima.pIni.z + Dados.h1);
                            GL.Vertex3(linhas_eixo.pIni.x, linhas_eixo.pIni.y, linhas_eixo.pIni.z + Dados.h1);
                            GL.Vertex3(linhas_facebaixo.pIni.x, linhas_facebaixo.pIni.y, linhas_facebaixo.pIni.z + Dados.h1);
                            GL.Vertex3(linhas_facebaixo.pFin.x, linhas_facebaixo.pFin.y, linhas_facebaixo.pFin.z + Dados.h1);
                            GL.Vertex3(linhas_eixo.pFin.x, linhas_eixo.pFin.y, linhas_eixo.pFin.z + Dados.h1);
                            GL.Vertex3(linhas_facecima.pFin.x, linhas_facecima.pFin.y, linhas_facecima.pFin.z + Dados.h1);
                            GL.End();
                        }

                       /* GL.Vertex3(linhas_facecima.pIni.y - centroY, h, linhas_facecima.pIni.x - centroX);
                        GL.Vertex3(linhas_facecima.pFin.y - centroY, h, linhas_facecima.pFin.x - centroX);
                        */
                        GL.Color4(Color.FromArgb(transp, this.layer.Rgb[0], this.layer.Rgb[1], this.layer.Rgb[2]));
                        
                        /*Poligonos*/
                        p1 = new vec3(linhas_facecima.pIni.x, linhas_facecima.pIni.y, linhas_facecima.pIni.z);
                        p2 = new vec3(linhas_facecima.pFin.x, linhas_facecima.pFin.y, linhas_facecima.pFin.z);
                        p3 = new vec3(linhas_facecima.pFin.x, linhas_facecima.pFin.y, linhas_facecima.pFin.z + Dados.h1);
                        v1 = p2 - p1;
                        v2 = p3 - p1;
                        n1 = v1.CrossProduct(v2);
                        n1.Normalize();
                        GL.Begin(PrimitiveType.Polygon);
                        GL.Normal3(-n1.x, -n1.y, -n1.z);
                        GL.Vertex3(linhas_facecima.pIni.x, linhas_facecima.pIni.y, linhas_facecima.pIni.z);
                        GL.Vertex3(linhas_facecima.pFin.x, linhas_facecima.pFin.y, linhas_facecima.pFin.z);
                        GL.Vertex3(linhas_facecima.pFin.x, linhas_facecima.pFin.y, linhas_facecima.pFin.z + Dados.h1);
                        GL.Vertex3(linhas_facecima.pIni.x, linhas_facecima.pIni.y, linhas_facecima.pIni.z + Dados.h1);
                        GL.End();

                        p1 = new vec3(linhas_facebaixo.pIni.x, linhas_facebaixo.pIni.y, linhas_facebaixo.pIni.z);
                        p2 = new vec3(linhas_facebaixo.pFin.x, linhas_facebaixo.pFin.y, linhas_facebaixo.pFin.z);
                        p3 = new vec3(linhas_facebaixo.pFin.x, linhas_facebaixo.pFin.y, linhas_facebaixo.pFin.z + Dados.h1);
                        v1 = p2 - p1;
                        v2 = p3 - p1;
                        n1 = v1.CrossProduct(v2);
                        n1.Normalize();
                        GL.Begin(PrimitiveType.Polygon);
                        GL.Normal3(-n1.x, -n1.y, -n1.z);
                        GL.Vertex3(linhas_facebaixo.pIni.x, linhas_facebaixo.pIni.y, linhas_facebaixo.pIni.z);
                        GL.Vertex3(linhas_facebaixo.pFin.x, linhas_facebaixo.pFin.y, linhas_facebaixo.pFin.z);
                        GL.Vertex3(linhas_facebaixo.pFin.x, linhas_facebaixo.pFin.y, linhas_facebaixo.pFin.z + Dados.h1);
                        GL.Vertex3(linhas_facebaixo.pIni.x, linhas_facebaixo.pIni.y, linhas_facebaixo.pIni.z + Dados.h1);
                        GL.End();

                        p1 = new vec3(linhas_facecima.pIni.x, linhas_facecima.pIni.y, linhas_facecima.pIni.z);
                        p2 = new vec3(linhas_facebaixo.pIni.x, linhas_facebaixo.pIni.y, linhas_facebaixo.pIni.z);
                        p3 = new vec3(linhas_facebaixo.pFin.x, linhas_facebaixo.pFin.y, linhas_facebaixo.pFin.z);
                        v1 = p2 - p1;
                        v2 = p3 - p1;
                        n1 = v1.CrossProduct(v2);
                        n1.Normalize();
                        GL.Begin(PrimitiveType.Polygon);
                        GL.Normal3(-n1.x, -n1.y, -n1.z);
                        GL.Vertex3(linhas_facecima.pIni.x, linhas_facecima.pIni.y, linhas_facecima.pIni.z);
                        GL.Vertex3(linhas_facebaixo.pIni.x, linhas_facebaixo.pIni.y, linhas_facebaixo.pIni.z);
                        GL.Vertex3(linhas_facebaixo.pFin.x, linhas_facebaixo.pFin.y, linhas_facebaixo.pFin.z);
                        GL.Vertex3(linhas_facecima.pFin.x, linhas_facecima.pFin.y, linhas_facecima.pFin.z);
                        GL.End();

                        p1 = new vec3(linhas_facecima.pIni.x, linhas_facecima.pIni.y, linhas_facecima.pIni.z + Dados.h1);
                        p2 = new vec3(linhas_facebaixo.pIni.x, linhas_facebaixo.pIni.y, linhas_facebaixo.pIni.z + Dados.h1);
                        p3 = new vec3(linhas_facebaixo.pFin.x, linhas_facebaixo.pFin.y, linhas_facebaixo.pFin.z + Dados.h1);
                        v1 = p2 - p1;
                        v2 = p3 - p1;
                        n1 = v1.CrossProduct(v2);
                        n1.Normalize();
                        GL.Begin(PrimitiveType.Polygon);
                        GL.Normal3(-n1.x, -n1.y, -n1.z);
                        GL.Vertex3(linhas_facecima.pIni.x, linhas_facecima.pIni.y, linhas_facecima.pIni.z + Dados.h1);
                        GL.Vertex3(linhas_eixo.pIni.x, linhas_eixo.pIni.y, linhas_eixo.pIni.z + Dados.h1);
                        GL.Vertex3(linhas_facebaixo.pIni.x, linhas_facebaixo.pIni.y, linhas_facebaixo.pIni.z + Dados.h1);
                        GL.Vertex3(linhas_facebaixo.pFin.x, linhas_facebaixo.pFin.y, linhas_facebaixo.pFin.z + Dados.h1);
                        GL.Vertex3(linhas_eixo.pFin.x, linhas_eixo.pFin.y, linhas_eixo.pFin.z + Dados.h1); 
                        GL.Vertex3(linhas_facecima.pFin.x, linhas_facecima.pFin.y, linhas_facecima.pFin.z + Dados.h1);
                        GL.End();

/*
 p1 = new vec3(linhas_facecima.pIni.x, linhas_facecima.pIni.y, linhas_facecima.pIni.z);
 p2 = new vec3(linhas_facecima.pFin.x, linhas_facecima.pFin.y, linhas_facecima.pFin.z);
 p3 = new vec3(linhas_facecima.pFin.x, linhas_facecima.pFin.y, linhas_facecima.pFin.z - Dados.h1);
 v1 = p2 - p1;
 v2 = p3 - p1;
 n1 = v1.CrossProduct(v2);
 n1.Normalize();
 GL.Begin(PrimitiveType.Polygon);
 GL.Normal3(-n1.x, -n1.y, -n1.z);
 GL.Vertex3(linhas_facecima.pIni.y, linhas_facecima.pIni.z, linhas_facecima.pIni.x);
 GL.Vertex3(linhas_facecima.pFin.y, linhas_facecima.pFin.z, linhas_facecima.pFin.x);
 GL.Vertex3(linhas_facecima.pFin.y, linhas_facecima.pFin.z - Dados.h1, linhas_facecima.pFin.x);
 GL.Vertex3(linhas_facecima.pIni.y, linhas_facecima.pIni.z - Dados.h1, linhas_facecima.pIni.x);
 GL.End();

 p1 = new vec3(linhas_facebaixo.pIni.x, linhas_facebaixo.pIni.y, linhas_facebaixo.pIni.z);
 p2 = new vec3(linhas_facebaixo.pFin.x, linhas_facebaixo.pFin.y, linhas_facebaixo.pFin.z);
 p3 = new vec3(linhas_facebaixo.pFin.x, linhas_facebaixo.pFin.y, linhas_facebaixo.pFin.z - Dados.h1);
 v1 = p2 - p1;
 v2 = p3 - p1;
 n1 = v1.CrossProduct(v2);
 n1.Normalize();
 GL.Begin(PrimitiveType.Polygon);
 GL.Normal3(-n1.x, -n1.y, -n1.z);
 GL.Vertex3(linhas_facebaixo.pIni.y, linhas_facebaixo.pIni.z, linhas_facebaixo.pIni.x);
 GL.Vertex3(linhas_facebaixo.pFin.y, linhas_facebaixo.pFin.z, linhas_facebaixo.pFin.x);
 GL.Vertex3(linhas_facebaixo.pFin.y, linhas_facebaixo.pFin.z - Dados.h1, linhas_facebaixo.pFin.x);
 GL.Vertex3(linhas_facebaixo.pIni.y, linhas_facebaixo.pIni.z - Dados.h1, linhas_facebaixo.pIni.x);
 GL.End();

 p1 = new vec3(linhas_facecima.pIni.x, linhas_facecima.pIni.y, linhas_facecima.pIni.z);
 p2 = new vec3(linhas_facebaixo.pIni.x, linhas_facebaixo.pIni.y, linhas_facebaixo.pIni.z);
 p3 = new vec3(linhas_facebaixo.pFin.x, linhas_facebaixo.pFin.y, linhas_facebaixo.pFin.z);
 v1 = p2 - p1;
 v2 = p3 - p1;
 n1 = v1.CrossProduct(v2);
 n1.Normalize();
 GL.Begin(PrimitiveType.Polygon);
 GL.Normal3(-n1.x, -n1.y, -n1.z);
 GL.Vertex3(linhas_facecima.pIni.y , linhas_facecima.pIni.z, linhas_facecima.pIni.x);
 GL.Vertex3(linhas_facebaixo.pIni.y, linhas_facebaixo.pIni.z, linhas_facebaixo.pIni.x);
 GL.Vertex3(linhas_facebaixo.pFin.y, linhas_facebaixo.pFin.z, linhas_facebaixo.pFin.x);
 GL.Vertex3(linhas_facecima.pFin.y , linhas_facecima.pFin.z, linhas_facecima.pFin.x);
 GL.End();

 p1 = new vec3(linhas_facecima.pIni.x, linhas_facecima.pIni.y, linhas_facecima.pIni.z - Dados.h1);
 p2 = new vec3(linhas_facebaixo.pIni.x, linhas_facebaixo.pIni.y, linhas_facebaixo.pIni.z - Dados.h1);
 p3 = new vec3(linhas_facebaixo.pFin.x, linhas_facebaixo.pFin.y, linhas_facebaixo.pFin.z - Dados.h1);
 v1 = p2 - p1;
 v2 = p3 - p1;
 n1 = v1.CrossProduct(v2);
 n1.Normalize();
 GL.Begin(PrimitiveType.Polygon);
 GL.Normal3(-n1.x, -n1.y, -n1.z);
 GL.Vertex3(linhas_facecima.pIni.y, linhas_facecima.pIni.z- Dados.h1, linhas_facecima.pIni.x );
 GL.Vertex3(linhas_facebaixo.pIni.y, linhas_facebaixo.pIni.z- Dados.h1, linhas_facebaixo.pIni.x);
 GL.Vertex3(linhas_facebaixo.pFin.y, linhas_facebaixo.pFin.z- Dados.h1, linhas_facebaixo.pFin.x);
 GL.Vertex3(linhas_facecima.pFin.y, linhas_facecima.pFin.z- Dados.h1, linhas_facecima.pFin.x);
 GL.End();*/

                        if (!ComVigaFaceInicial && BarraRigida_Ini == null)
                        {
                            GL.Begin(PrimitiveType.Polygon);
                            GL.Vertex3(linhas_facebaixo.pIni.x, linhas_facebaixo.pIni.y, linhas_facebaixo.pIni.z);
                            GL.Vertex3(linhas_facecima.pIni.x, linhas_facecima.pIni.y, linhas_facecima.pIni.z);
                            GL.Vertex3(linhas_facebaixo.pIni.x, linhas_facebaixo.pIni.y, linhas_facebaixo.pIni.z + Dados.h1);
                            GL.Vertex3(linhas_facecima.pIni.x, linhas_facecima.pIni.y, linhas_facecima.pIni.z + Dados.h1);
                            GL.Vertex3(linhas_facebaixo.pIni.x, linhas_facebaixo.pIni.y, linhas_facebaixo.pIni.z);
                            GL.End();
                        }

                        if (!ComVigaFaceFinal && BarraRigida_Fin == null)
                        {
                            GL.Begin(PrimitiveType.Polygon);
                            GL.Vertex3(linhas_facebaixo.pFin.x, linhas_facebaixo.pFin.y, linhas_facebaixo.pFin.z);
                            GL.Vertex3(linhas_facecima.pFin.x, linhas_facecima.pFin.y, linhas_facecima.pFin.z);
                            GL.Vertex3(linhas_facebaixo.pFin.x, linhas_facebaixo.pFin.y, linhas_facebaixo.pFin.z + Dados.h1);
                            GL.Vertex3(linhas_facecima.pFin.x, linhas_facecima.pFin.y, linhas_facecima.pFin.z + Dados.h1);
                            GL.Vertex3(linhas_facebaixo.pFin.x, linhas_facebaixo.pFin.y, linhas_facebaixo.pFin.z);
                            GL.End();
                        }
                    }

                    GL.LineWidth(1);
                    
                    /*=============================================*/
                }
                catch (Exception ms)
                {
                    MessageBox.Show("erro ao desenha trecho de viga: trecho" + this.Dados.numero.ToString() + "  -  " + ms.Message);
                }
            }
        }
  
        public override void Desenha(ref System.Drawing.Graphics Cad)
        {
        /*    if (Visivel)
            {
                try
                {
                    if (NumPilar_PontoInicial > 0)
                      GL.Color3(Color.FromArgb(this.Pavimento.LayersByIdPrincipal[Lay.Pilares].Rgb[0], this.Pavimento.LayersByIdPrincipal[Lay.Pilares].Rgb[1], this.Pavimento.LayersByIdPrincipal[Lay.Pilares].Rgb[2]));

                   /* GL.Begin(PrimitiveType.Lines);
                    for (j = 0; j <= 7; j++)
                        GL.Vertex3(linhas_eixo.pIni.x + (1 * Math.Cos(j * 6.2831 / 7)), linhas_eixo.pIni.y + (1 * Math.Sin(j * 6.2831 / 7)),
                                   linhas_eixo.pIni.x + (1 * Math.Cos((j + 1) * 6.2831 / 7)), linhas_eixo.pIni.y + (1 * Math.Sin((j + 1) * 6.2831 / 7)));
                    */
             /*      
                    if (NumPilar_PontoFinal > 0)
                        GL.Color3(Color.FromArgb(this.Pavimento.LayersByIdPrincipal[Lay.Pilares].Rgb[0], this.Pavimento.LayersByIdPrincipal[Lay.Pilares].Rgb[1], this.Pavimento.LayersByIdPrincipal[Lay.Pilares].Rgb[2]));
                    else
                        GL.Color3(Color.FromArgb(this.layer.Rgb[0], this.layer.Rgb[1], this.layer.Rgb[2]));

                   /* for (j = 0; j <= 7; j++)
                        Cad.DrawLine(mPen, Desenho.pixelX((linhas_eixo.pFin.x + (1 * Math.Cos(j * 6.2831 / 7)))), Desenho.pixelY((linhas_eixo.pFin.y + (1 * Math.Sin(j * 6.2831 / 7)))),
                                             Desenho.pixelX((linhas_eixo.pFin.x + (1 * Math.Cos((j + 1) * 6.2831 / 7)))), Desenho.pixelY((linhas_eixo.pFin.y + (1 * Math.Sin((j + 1) * 6.2831 / 7)))));
                    */
            /*
                    if (base.Selecionado)
                    {
                        if (base.MostrarGrip)
                            foreach (TGrip gr in base.Grips)
                                gr.Desenha(ref Cad);

                        if (CandidatoSelecao)
                            mPen.Color = Color.Orange;
                        else
                            mPen.Color = Color.Red;
                    }
                    else
                      GL.Color4(this.layer.Rgb[0], this.layer.Rgb[1], this.layer.Rgb[2], transparencia);

                    /*Arestas*/
            /*        GL.Begin(PrimitiveType.LineLoop);
                    GL.Vertex3(linhas_facecima.pIni.x, linhas_facecima.pIni.y, Pavimento.Nivel);
                    GL.Vertex3(linhas_facecima.pFin.x, linhas_facecima.pFin.y, Pavimento.Nivel);
                    GL.Vertex3(linhas_facecima.pFin.x, linhas_facecima.pFin.y, Pavimento.Nivel - Dados.h1);
                    GL.Vertex3(linhas_facecima.pIni.x, linhas_facecima.pIni.y, Pavimento.Nivel - Dados.h1);
                    GL.Vertex3(linhas_facecima.pIni.x, linhas_facecima.pIni.y, Pavimento.Nivel);
                    GL.End();

                    GL.Begin(PrimitiveType.LineLoop);
                    GL.Vertex3(linhas_facebaixo.pIni.x, linhas_facebaixo.pIni.y, Pavimento.Nivel);
                    GL.Vertex3(linhas_facebaixo.pFin.x, linhas_facebaixo.pFin.y, Pavimento.Nivel);
                    GL.Vertex3(linhas_facebaixo.pFin.x, linhas_facebaixo.pFin.y, Pavimento.Nivel - Dados.h1);
                    GL.Vertex3(linhas_facebaixo.pIni.x, linhas_facebaixo.pIni.y, Pavimento.Nivel - Dados.h1);
                    GL.Vertex3(linhas_facebaixo.pIni.x, linhas_facebaixo.pIni.y, Pavimento.Nivel);
                    GL.End();

                    if (!ComVigaFaceInicial && BarraRigida_Ini == null)
                    {
                        GL.Begin(PrimitiveType.LineLoop);
                        GL.Vertex3(linhas_facebaixo.pIni.x, linhas_facebaixo.pIni.y, Pavimento.Nivel);
                        GL.Vertex3(linhas_facecima.pIni.x, linhas_facecima.pIni.y, Pavimento.Nivel);
                        GL.Vertex3(linhas_facebaixo.pIni.x, linhas_facebaixo.pIni.y, Pavimento.Nivel - Dados.h1);
                        GL.Vertex3(linhas_facecima.pIni.x, linhas_facecima.pIni.y, Pavimento.Nivel - Dados.h1);
                        GL.Vertex3(linhas_facebaixo.pIni.x, linhas_facebaixo.pIni.y, Pavimento.Nivel);
                        GL.End();
                    }

                    if (!ComVigaFaceFinal && BarraRigida_Fin == null)
                    {
                        GL.Begin(PrimitiveType.LineLoop);
                        GL.Vertex3(linhas_facebaixo.pFin.x, linhas_facebaixo.pFin.y, Pavimento.Nivel);
                        GL.Vertex3(linhas_facecima.pFin.x, linhas_facecima.pFin.y, Pavimento.Nivel);
                        GL.Vertex3(linhas_facebaixo.pFin.x, linhas_facebaixo.pFin.y, Pavimento.Nivel - Dados.h1);
                        GL.Vertex3(linhas_facecima.pFin.x, linhas_facecima.pFin.y, Pavimento.Nivel - Dados.h1);
                        GL.Vertex3(linhas_facebaixo.pFin.x, linhas_facebaixo.pFin.y, Pavimento.Nivel);
                        GL.End();
                    }


                    /*Poligonos*/
                /*    p1 = new vec3(linhas_facecima.pIni.x, linhas_facecima.pIni.y, Pavimento.Nivel);
                    p2 = new vec3(linhas_facecima.pFin.x, linhas_facecima.pFin.y, Pavimento.Nivel);
                    p3 = new vec3(linhas_facecima.pFin.x, linhas_facecima.pFin.y, Pavimento.Nivel - Dados.h1);
                    v1 = p2 - p1;
                    v2 = p3 - p1;
                    n1 = v1.CrossProduct(v2);
                    n1.Normalize();
                    GL.Begin(PrimitiveType.Polygon);
                    GL.Normal3(-n1.x, -n1.y, -n1.z); 
                    GL.Vertex3(linhas_facecima.pIni.x, linhas_facecima.pIni.y, Pavimento.Nivel);
                    GL.Vertex3(linhas_facecima.pFin.x, linhas_facecima.pFin.y, Pavimento.Nivel);
                    GL.Vertex3(linhas_facecima.pFin.x, linhas_facecima.pFin.y, Pavimento.Nivel - Dados.h1);
                    GL.Vertex3(linhas_facecima.pIni.x, linhas_facecima.pIni.y, Pavimento.Nivel - Dados.h1);
                    GL.Vertex3(linhas_facecima.pIni.x, linhas_facecima.pIni.y, Pavimento.Nivel);
                    GL.End();

                    p1 = new vec3(linhas_facebaixo.pIni.x, linhas_facebaixo.pIni.y, Pavimento.Nivel);
                    p2 = new vec3(linhas_facebaixo.pFin.x, linhas_facebaixo.pFin.y, Pavimento.Nivel);
                    p3 = new vec3(linhas_facebaixo.pFin.x, linhas_facebaixo.pFin.y, Pavimento.Nivel - Dados.h1);
                    v1 = p2 - p1;
                    v2 = p3 - p1;
                    n1 = v1.CrossProduct(v2);
                    n1.Normalize();
                    GL.Begin(PrimitiveType.Polygon);
                    GL.Normal3(-n1.x, -n1.y, -n1.z); 
                    GL.Vertex3(linhas_facebaixo.pIni.x, linhas_facebaixo.pIni.y, Pavimento.Nivel);
                    GL.Vertex3(linhas_facebaixo.pFin.x, linhas_facebaixo.pFin.y, Pavimento.Nivel);
                    GL.Vertex3(linhas_facebaixo.pFin.x, linhas_facebaixo.pFin.y, Pavimento.Nivel - Dados.h1);
                    GL.Vertex3(linhas_facebaixo.pIni.x, linhas_facebaixo.pIni.y, Pavimento.Nivel - Dados.h1);
                    GL.Vertex3(linhas_facebaixo.pIni.x, linhas_facebaixo.pIni.y, Pavimento.Nivel);
                    GL.End();

                    p1 = new vec3(linhas_facecima.pIni.x, linhas_facecima.pIni.y, Pavimento.Nivel);
                    p2 = new vec3(linhas_facebaixo.pIni.x, linhas_facebaixo.pIni.y, Pavimento.Nivel);
                    p3 = new vec3(linhas_facebaixo.pFin.x, linhas_facebaixo.pFin.y, Pavimento.Nivel);
                    v1 = p2 - p1;
                    v2 = p3 - p1;
                    n1 = v1.CrossProduct(v2);
                    n1.Normalize();
                    GL.Begin(PrimitiveType.Polygon);
                    GL.Normal3(-n1.x, -n1.y, -n1.z);
                    GL.Vertex3(linhas_facecima.pIni.x, linhas_facecima.pIni.y, Pavimento.Nivel);
                    GL.Vertex3(linhas_facebaixo.pIni.x, linhas_facebaixo.pIni.y, Pavimento.Nivel);
                    GL.Vertex3(linhas_facebaixo.pFin.x, linhas_facebaixo.pFin.y, Pavimento.Nivel);
                    GL.Vertex3(linhas_facecima.pFin.x, linhas_facecima.pFin.y, Pavimento.Nivel);
                    GL.Vertex3(linhas_facecima.pIni.x, linhas_facecima.pIni.y, Pavimento.Nivel);
                    GL.End();

                    p1 = new vec3(linhas_facecima.pIni.x, linhas_facecima.pIni.y, Pavimento.Nivel - Dados.h1);
                    p2 = new vec3(linhas_facebaixo.pIni.x, linhas_facebaixo.pIni.y, Pavimento.Nivel - Dados.h1);
                    p3 = new vec3(linhas_facebaixo.pFin.x, linhas_facebaixo.pFin.y, Pavimento.Nivel - Dados.h1);
                    v1 = p2 - p1;
                    v2 = p3 - p1;
                    n1 = v1.CrossProduct(v2);
                    n1.Normalize();
                    GL.Begin(PrimitiveType.Polygon);
                    GL.Normal3(-n1.x, -n1.y, -n1.z);
                    GL.Vertex3(linhas_facecima.pIni.x, linhas_facecima.pIni.y, Pavimento.Nivel - Dados.h1);
                    GL.Vertex3(linhas_facebaixo.pIni.x, linhas_facebaixo.pIni.y, Pavimento.Nivel - Dados.h1);
                    GL.Vertex3(linhas_facebaixo.pFin.x, linhas_facebaixo.pFin.y, Pavimento.Nivel - Dados.h1);
                    GL.Vertex3(linhas_facecima.pFin.x, linhas_facecima.pFin.y, Pavimento.Nivel - Dados.h1);
                    GL.Vertex3(linhas_facecima.pIni.x, linhas_facecima.pIni.y, Pavimento.Nivel - Dados.h1);
                    GL.End();

                    if (!ComVigaFaceInicial && BarraRigida_Ini == null)
                    {
                        GL.Begin(PrimitiveType.Polygon);
                        GL.Vertex3(linhas_facebaixo.pIni.x, linhas_facebaixo.pIni.y, Pavimento.Nivel);
                        GL.Vertex3(linhas_facecima.pIni.x, linhas_facecima.pIni.y, Pavimento.Nivel);
                        GL.Vertex3(linhas_facebaixo.pIni.x, linhas_facebaixo.pIni.y, Pavimento.Nivel - Dados.h1);
                        GL.Vertex3(linhas_facecima.pIni.x, linhas_facecima.pIni.y, Pavimento.Nivel - Dados.h1);
                        GL.Vertex3(linhas_facebaixo.pIni.x, linhas_facebaixo.pIni.y, Pavimento.Nivel);
                        GL.End();
                    }

                    if (!ComVigaFaceFinal && BarraRigida_Fin == null)
                    {
                        GL.Begin(PrimitiveType.Polygon);
                        GL.Vertex3(linhas_facebaixo.pFin.x, linhas_facebaixo.pFin.y, Pavimento.Nivel);
                        GL.Vertex3(linhas_facecima.pFin.x, linhas_facecima.pFin.y, Pavimento.Nivel);
                        GL.Vertex3(linhas_facebaixo.pFin.x, linhas_facebaixo.pFin.y, Pavimento.Nivel - Dados.h1);
                        GL.Vertex3(linhas_facecima.pFin.x, linhas_facecima.pFin.y, Pavimento.Nivel - Dados.h1);
                        GL.Vertex3(linhas_facebaixo.pFin.x, linhas_facebaixo.pFin.y, Pavimento.Nivel);
                        GL.End();
                    }
                    /*=============================================*/


                  /*  mPen.DashStyle   = DashStyle.DashDotDot;
                    mPen.DashPattern = dashValues;
                    mPen.DashStyle = DashStyle.Solid;

                    if (NumPilar_PontoInicial > 0)
                        mPen.Color = Color.FromArgb(this.Pavimento.LayersByIdPrincipal[Lay.Pilares].Rgb[0], this.Pavimento.LayersByIdPrincipal[Lay.Pilares].Rgb[1], this.Pavimento.LayersByIdPrincipal[Lay.Pilares].Rgb[2]);

                    for (j = 0; j <= 7; j++)
                        Cad.DrawLine(mPen, Desenho.pixelX((linhas_eixo.pIni.x + (1 * Math.Cos(j * 6.2831 / 7)))), Desenho.pixelY((linhas_eixo.pIni.y + (1 * Math.Sin(j * 6.2831 / 7)))),
                                            Desenho.pixelX((linhas_eixo.pIni.x + (1 * Math.Cos((j + 1) * 6.2831 / 7)))), Desenho.pixelY((linhas_eixo.pIni.y + (1 * Math.Sin((j + 1) * 6.2831 / 7)))));

                    if (NumPilar_PontoFinal > 0)
                        mPen.Color = Color.FromArgb(this.Pavimento.LayersByIdPrincipal[Lay.Pilares].Rgb[0], this.Pavimento.LayersByIdPrincipal[Lay.Pilares].Rgb[1], this.Pavimento.LayersByIdPrincipal[Lay.Pilares].Rgb[2]);
                    else
                        mPen.Color = Color.FromArgb(this.layer.Rgb[0], this.layer.Rgb[1], this.layer.Rgb[2]);

                    for (j = 0; j <= 7; j++)
                        Cad.DrawLine(mPen, Desenho.pixelX((linhas_eixo.pFin.x + (1 * Math.Cos(j * 6.2831 / 7)))), Desenho.pixelY((linhas_eixo.pFin.y + (1 * Math.Sin(j * 6.2831 / 7)))),
                                             Desenho.pixelX((linhas_eixo.pFin.x + (1 * Math.Cos((j + 1) * 6.2831 / 7)))), Desenho.pixelY((linhas_eixo.pFin.y + (1 * Math.Sin((j + 1) * 6.2831 / 7)))));

                    if (base.Selecionado)
                    {
                        if (base.MostrarGrip)
                            foreach (TGrip gr in base.Grips)
                                gr.Desenha(ref Cad);

                        if (CandidatoSelecao)
                            mPen.Color = Color.Orange;
                        else
                            mPen.Color = Color.Red;
                    }
                    else
                        mPen.Color = Color.FromArgb(this.layer.Rgb[0], this.layer.Rgb[1], this.layer.Rgb[2]);

                    Cad.DrawLine(mPen, Desenho.pixelX(linhas_facecima.pIni.x), Desenho.pixelY(linhas_facecima.pIni.y), Desenho.pixelX(linhas_facecima.pFin.x), Desenho.pixelY(linhas_facecima.pFin.y));
                    Cad.DrawLine(mPen, Desenho.pixelX(linhas_facebaixo.pIni.x), Desenho.pixelY(linhas_facebaixo.pIni.y), Desenho.pixelX(linhas_facebaixo.pFin.x), Desenho.pixelY(linhas_facebaixo.pFin.y));

                    if (!ComVigaFaceInicial && BarraRigida_Ini == null)
                        Cad.DrawLine(mPen, Desenho.pixelX(linhas_facecima.pIni.x), Desenho.pixelY(linhas_facecima.pIni.y), Desenho.pixelX(linhas_facebaixo.pIni.x), Desenho.pixelY(linhas_facebaixo.pIni.y));
                    if (!ComVigaFaceFinal && BarraRigida_Fin == null)
                        Cad.DrawLine(mPen, Desenho.pixelX(linhas_facecima.pFin.x), Desenho.pixelY(linhas_facecima.pFin.y), Desenho.pixelX(linhas_facebaixo.pFin.x), Desenho.pixelY(linhas_facebaixo.pFin.y));
                }
                catch (Exception ms)
                {
                    MessageBox.Show("erro ao desenha trecho de viga: trecho" +this.Dados.numero.ToString()+"  -  " +ms.Message);
                }
            }*/
        }
        
        public void ReposicionaTextos()
        {
            float ang2 = (float)this.angulo;
            PontoD pt;

            if (linhas_eixo.pFin.x >= linhas_eixo.pIni.x)
                ang2 -= 90;
            else
                ang2 += 90;

            float xx6 = (float)((30) * Math.Cos(ang2 * Const.PIDiv180));
            float yy6 = (float)((30) * Math.Sin(ang2 * Const.PIDiv180));

            if (Math.Abs(this.angulo) == 90)
            {
                if (this.angulo == 90)
                {
                    pt = Geom.FracaoDeLinha(
                                this.linhas_eixo.pIni.x,
                                this.linhas_eixo.pIni.y,
                                this.linhas_eixo.pFin.x,
                                this.linhas_eixo.pFin.y,
                                  2);

                    Texto1.x =  pt.x - 12.0;
                    Texto1.y =  pt.y + yy6;
                    Texto1.angulo = (float)this.angulo;

                    pt = Geom.FracaoDeLinha(
                    this.linhas_eixo.pIni.x,
                    this.linhas_eixo.pIni.y,
                    this.linhas_eixo.pFin.x,
                    this.linhas_eixo.pFin.y,
                    2);

                    Texto2.x = pt.x + xx6 / 2.0;
                    Texto2.y = pt.y + yy6;
                    Texto2.angulo = (float)this.angulo;
                }
                else
                {
                    pt = Geom.FracaoDeLinha(
                        this.linhas_eixo.pIni.x,
                        this.linhas_eixo.pIni.y,
                        this.linhas_eixo.pFin.x,
                        this.linhas_eixo.pFin.y,
                        2);

                    Texto1.x = pt.x + 12.0;
                    Texto1.y = pt.y + yy6;
                    Texto1.angulo = (float)this.angulo;

                    pt = Geom.FracaoDeLinha(
                    this.linhas_eixo.pIni.x,
                    this.linhas_eixo.pIni.y,
                    this.linhas_eixo.pFin.x,
                    this.linhas_eixo.pFin.y,
                    2);

                    Texto2.x = pt.x + xx6 / 2.0;
                    Texto2.y = pt.y + yy6;
                    Texto2.angulo = (float)this.angulo;
                }
            }
            else
            {
                if (this.linhas_facecima.pIni.y < this.linhas_facebaixo.pIni.y)
                {
                    pt = Geom.FracaoDeLinha(
                        this.linhas_facecima.pIni.x,
                        this.linhas_facecima.pIni.y,
                        this.linhas_facecima.pFin.x,
                        this.linhas_facecima.pFin.y, 2);

                    Texto1.x = pt.x - xx6 / 1.9;
                    Texto1.y =  pt.y - yy6 / 1.9;
                    Texto1.angulo = (float)this.angulo;

                    pt = Geom.FracaoDeLinha(
                    this.linhas_eixo.pIni.x,
                    this.linhas_eixo.pIni.y,
                    this.linhas_eixo.pFin.x,
                    this.linhas_eixo.pFin.y,
                    2);

                    Texto2.x = pt.x + xx6 / 1.9;
                    Texto2.y =  pt.y + yy6 / 1.9;
                    Texto2.angulo = (float)this.angulo;
                }
                else
                {
                    pt = Geom.FracaoDeLinha(
                    this.linhas_facebaixo.pIni.x,
                    this.linhas_facebaixo.pIni.y,
                    this.linhas_facebaixo.pFin.x,
                    this.linhas_facebaixo.pFin.y,
                    2);
                  
                    Texto1.x = pt.x + xx6 / 1.9;
                    Texto1.y = pt.y + yy6 / 1.9;
                    Texto1.angulo = (float)this.angulo;

                    pt = Geom.FracaoDeLinha(
                    this.linhas_facecima.pIni.x,
                    this.linhas_facecima.pIni.y,
                    this.linhas_facecima.pFin.x,
                    this.linhas_facecima.pFin.y,
                    2);

                    Texto2.x = pt.x - (xx6 / 3.3);
                    Texto2.y = pt.y - (yy6 / 3.3);
                    Texto2.angulo = (float)this.angulo;
                }
            }

            Texto1.OnMove(Texto1.x, Texto1.y);
            Texto2.OnMove(Texto2.x, Texto2.y);
        }

        public void CriaTextos(TTrechoViga New, string info1, string info2)
        {
          /*  PontoD pt;

            string texto = info1;

            float ang2 = (float)New.angulo;

            if (New.pFin.x >= New.pIni.x)
                ang2 -= 90;
            else
                ang2 += 90;

            float xx = (float)((30) * Math.Cos(ang2 * Const.PIDiv180));
            float yy = (float)((30) * Math.Sin(ang2 * Const.PIDiv180));

            TTexto Texto1, Texto2;

            float r = 0.0f;
            float g = .5f;
            float b = .0f;

            if (Math.Abs(New.angulo) == 90)
            {
                if (New.angulo == 90)
                {
                    pt = Geom.FracaoDeLinha(
                                New.linhas_eixo.pIni.x,
                                New.linhas_eixo.pIni.y,
                                New.linhas_eixo.pFin.x,
                                New.linhas_eixo.pFin.y,
                                  2);

                    Texto1 = new TTexto(info2,
                                        pt.x - 12.0,
                                        pt.y + yy,pt.z,
                                       (15.0 / 80),
                                      -(15.0 / 80),
                                      r,g,b,
                                      (float)New.angulo, Estrutura.LayersByIdPrincipal[Lay.TextosVigas], Const.ID_TRECHOVIGA);

                    pt = Geom.FracaoDeLinha(
                    New.linhas_eixo.pIni.x,
                    New.linhas_eixo.pIni.y,
                    New.linhas_eixo.pFin.x,
                    New.linhas_eixo.pFin.y,
                    2);

                    Texto2 = new TTexto(texto,
                                        pt.x + xx / 2.0, pt.y + yy,pt.z,
                                       (15.0 / 200),
                                      -(15.0 / 200),
                                      r, g, b,
                                        (float)New.angulo, Estrutura.LayersByIdPrincipal[Lay.TextosVigas], Const.ID_TRECHOVIGA);
                }
                else
                {
                    pt = Geom.FracaoDeLinha(
                        New.linhas_eixo.pIni.x,
                        New.linhas_eixo.pIni.y,
                        New.linhas_eixo.pFin.x,
                        New.linhas_eixo.pFin.y,
                        2);

                    Texto1 = new TTexto(info2,
                                        pt.x + 12.0,
                                        pt.y + yy,pt.z,
                                       (15.0 / 80),
                                      -(15.0 / 80),
                              r, g, b,
                                       (float)New.angulo, Estrutura.LayersByIdPrincipal[Lay.TextosVigas], Const.ID_TRECHOVIGA);

                    pt = Geom.FracaoDeLinha(
                    New.linhas_eixo.pIni.x,
                    New.linhas_eixo.pIni.y,
                    New.linhas_eixo.pFin.x,
                    New.linhas_eixo.pFin.y,
                    2);

                    Texto2 = new TTexto(texto,
                                        pt.x + xx/2, pt.y + yy,pt.z,
                                       (15.0 / 200),
                                      -(15.0 / 200),
                                        r, g, b,
                                        (float)New.angulo, Estrutura.LayersByIdPrincipal[Lay.TextosVigas], Const.ID_TRECHOVIGA);

                }
            }
            else
            {
                if (New.linhas_facecima.pIni.y < New.linhas_facebaixo.pIni.y)
                {
                    pt = Geom.FracaoDeLinha(
                        New.linhas_facecima.pIni.x,
                        New.linhas_facecima.pIni.y,
                        New.linhas_facecima.pFin.x,
                        New.linhas_facecima.pFin.y, 2);

                    Texto1 = new TTexto(info2,
                                        pt.x - xx / 1.3,
                                        pt.y - yy / 1.3,pt.z,
                                       (15.0 / 80),
                                      -(15.0 / 80),
                                     r, g, b,
                                        (float)New.angulo, Estrutura.LayersByIdPrincipal[Lay.TextosVigas], Const.ID_TRECHOVIGA);

                    pt = Geom.FracaoDeLinha(
                    New.linhas_eixo.pIni.x,
                    New.linhas_eixo.pIni.y,
                    New.linhas_eixo.pFin.x,
                    New.linhas_eixo.pFin.y,
                    2);

                    Texto2 = new TTexto(texto,
                                        pt.x + xx / 1.9, pt.y + yy / 1.9,pt.z,
                                       (15.0 / 200),
                                      -(15.0 / 200),
                                     r, g, b,
                                        (float)New.angulo, Estrutura.LayersByIdPrincipal[Lay.TextosVigas], Const.ID_TRECHOVIGA);
                }
                else
                {
                    pt = Geom.FracaoDeLinha(
                    New.linhas_facebaixo.pIni.x,
                    New.linhas_facebaixo.pIni.y,
                    New.linhas_facebaixo.pFin.x,
                    New.linhas_facebaixo.pFin.y,
                    2);

                    Texto1 = new TTexto(info2,
                                        pt.x + xx / 1.3,
                                        pt.y + yy / 1.3,pt.z,
                                       (15.0 / 80),
                                      -(15.0 / 80),
                                     r, g, b,
                                        (float)New.angulo, Estrutura.LayersByIdPrincipal[Lay.TextosVigas], Const.ID_TRECHOVIGA);

                    pt = Geom.FracaoDeLinha(
                    New.linhas_facecima.pIni.x,
                    New.linhas_facecima.pIni.y,
                    New.linhas_facecima.pFin.x,
                    New.linhas_facecima.pFin.y,
                    2);

                    Texto2 = new TTexto(texto,
                                        pt.x - (xx / 3.3), pt.y - (yy / 3.3),pt.z,
                                       (15.0 / 200),
                                      -(15.0 / 200),
                                  r, g, b,
                                        (float)New.angulo, Estrutura.LayersByIdPrincipal[Lay.TextosVigas], Const.ID_TRECHOVIGA);
                }
            }
            New.Texto1 = Texto1;
            New.Texto2 = Texto2;*/
        }

        public override bool Selecionar(float clicx, float clicy, float coordx, float coordy, TObjetoDesenho Owner = null)
        {
            try
            {
              if (this.layer.Congelado || this.layer.Travado) 
                return false;

              if (linhas_facebaixo.Selecionar(clicx, clicy, coordx, coordy))
              {
                  SetaSelecao(true, true);
                  return true;
              }
              if (linhas_facecima.Selecionar(clicx, clicy, coordx, coordy))
              {
                  SetaSelecao(true, true);
                  return true;
              }
              if (linhas_eixo.Selecionar(clicx, clicy, coordx, coordy))
              {
                  SetaSelecao(true,true);
                  return true;
              }
            }
            catch (Exception)
            {
                MessageBox.Show("Erro ao selecionar a viga + " + this.Texto1.texto);
            }
            return false;
        }

        public override TPonto LastPoint()
        {
            return pFin;
        }

        public override TPonto FirstPoint()
        {
            return pIni;
        }
        
        public override void Cancel(bool cmd, ref string msg, ref List<TPonto> points)
        {
            if (cmd)
              msg = Const.CMD_TRECHOVIGA_1_P;

            points.Add(linhas_facebaixo.pIni);
            points.Add(linhas_facebaixo.pFin);
            points.Add(linhas_eixo.pIni);
            points.Add(linhas_eixo.pFin);
            points.Add(linhas_facecima.pIni);
            points.Add(linhas_facecima.pFin);

            points[0].incidencias.Remove(linhas_facebaixo);
            points[1].incidencias.Remove(linhas_facebaixo);

            points[2].incidencias.Remove(linhas_eixo);
            points[3].incidencias.Remove(linhas_eixo);

            points[2].incidencias.Remove(this);
            points[3].incidencias.Remove(this);

            points[4].incidencias.Remove(linhas_facecima);
            points[5].incidencias.Remove(linhas_facecima);
        }

        float beta;

        [Serializable]
        public struct TrechosQuebra
        {
            public TTrechoViga trecho;
            public double x, y, z;

            public TrechosQuebra(TTrechoViga trecho, double x, double y, double z)
            {
                this.trecho = trecho;
                this.x = x;
                this.y = y;
                this.z = z;
            }
        }
        int LocalizaPontoEmLinha(double x, double y)
        {
            for (int jk = 0; jk < PontosEmLinha.Count; jk++)
                if (Geom.Iguais(x, PontosEmLinha[jk].x) && Geom.Iguais(y, PontosEmLinha[jk].y))
                    return jk;

            return -1;
        }
        private bool EhPontoExtremoLinha(TLinha lin, double x, double y)
        {
            if ((Geom.Iguais(x, lin.pIni.x) && Geom.Iguais(y, lin.pIni.y)) ||
                (Geom.Iguais(x, lin.pFin.x) && Geom.Iguais(y, lin.pFin.y)))
                return true;

            return false;
        }
        private void ReordenaPontos(double yini, double yfin, double xini, double xfin)
        {
            //se for trecho horizontal, reordena os nós em ordem crescente de X
            int kk;
            TPonto temp;
            if (Geom.Iguais(yini, yfin))
            {
                if (xini > xfin)
                {
                    for (kk = 0; kk < PontosEmLinha.Count; kk++)
                    {
                        for (int j = kk; j < PontosEmLinha.Count; j++)
                        {
                            if (PontosEmLinha[j].x > PontosEmLinha[kk].x)
                            {
                                temp = PontosEmLinha[kk];
                                PontosEmLinha[kk] = PontosEmLinha[j];
                                PontosEmLinha[j] = temp;
                            }
                        }
                    }
                }
                else
                {
                    for (kk = 0; kk < PontosEmLinha.Count; kk++)
                    {
                        for (int j = kk; j < PontosEmLinha.Count; j++)
                        {
                            if (PontosEmLinha[j].x < PontosEmLinha[kk].x)
                            {
                                temp = PontosEmLinha[kk];
                                PontosEmLinha[kk] = PontosEmLinha[j];
                                PontosEmLinha[j] = temp;
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
                    for (kk = 0; kk < PontosEmLinha.Count; kk++)
                    {
                        for (int j = kk; j < PontosEmLinha.Count; j++)
                        {
                            if (PontosEmLinha[j].y > PontosEmLinha[kk].y)
                            {
                                temp = PontosEmLinha[kk];
                                PontosEmLinha[kk] = PontosEmLinha[j];
                                PontosEmLinha[j] = temp;
                            }
                        }
                    }
                }
                else
                {
                    for (kk = 0; kk < PontosEmLinha.Count; kk++)
                    {
                        for (int j = kk; j < PontosEmLinha.Count; j++)
                        {
                            if (PontosEmLinha[j].y < PontosEmLinha[kk].y)
                            {
                                temp = PontosEmLinha[kk];
                                PontosEmLinha[kk] = PontosEmLinha[j];
                                PontosEmLinha[j] = temp;
                            }
                        }
                    }
                }
            };
        }

        public void AlternaFace()
        {
            if (Dados.face_insercao == 0)
              Dados.face_insercao = 1;
            else
            if (Dados.face_insercao == 1)
              Dados.face_insercao = 2;
            else
            if (Dados.face_insercao == 2)
              Dados.face_insercao = 0;
        }

        [NonSerialized]
        TLinha main = null;
        [NonSerialized]
        public float[] dashValuesOnMouseMove = {2, 5, 2 };
        public override void OnMouseMove(ref TPonto point, bool ShowInfo, bool orto = true, 
            double angulo = 0, 
            bool PontoInicial = false)
        {
            //se está movendo a viga a partir do ponto inicial ou final
            if (PontoInicial)
            {
                base.pIni = point;
                this.pIni = point;
            }
            else
            {
                base.pFin = point;
                this.pFin = point;
                pFin.z = pIni.z;
            }
 
            this.comprimento   = (float)pFin.DistanceTo(pIni);
            base.angulo        = (float)(FuncoesGerais.atand((pIni.y - pFin.y) / (pIni.x - pFin.x)));
            base.anguloGlobal  = (float)RMath.rad2deg(pIni.getAngleTo(pFin));
            this.angulo        = base.angulo;

            beta = (float)this.angulo;
            if (pFin.x >= pIni.x)
              beta -= 90;
            else
              beta += 90;

            double nx = (float)((Dados.b1) * Math.Cos(beta * Const.PIDiv180));
            double ny = (float)((Dados.b1) * Math.Sin(beta * Const.PIDiv180));

            //se for vertical
            if ((beta == 0 && pFin.y < pIni.y) || (beta == -180 && pFin.y > pIni.y))
              nx = -nx;
            
            SetaCoords(this, Dados.face_insercao, pIni, pFin, ref main, ref nx, ref ny);

            if (!PontoInicial)
            {
                linhas_facecima.OnMouseMove(ref linhas_facecima.pFin, Dados.face_insercao == 0,  false, 0, PontoInicial);
                linhas_facecima.pFin.z = point.z;
                linhas_eixo.OnMouseMove(ref linhas_eixo.pFin, Dados.face_insercao == 1, false, 0, PontoInicial);
                linhas_eixo.pFin.z = point.z;

                linhas_facebaixo.OnMouseMove(ref linhas_facebaixo.pFin, Dados.face_insercao == 2, false, 0, PontoInicial);
                linhas_facebaixo.pFin.z = point.z;
            }
            else
            {
                linhas_facecima.OnMouseMove(ref linhas_facecima.pIni, Dados.face_insercao == 0, false, 0, PontoInicial);
                linhas_eixo.OnMouseMove(ref linhas_eixo.pIni, Dados.face_insercao == 1, false, 0, PontoInicial);
                linhas_facebaixo.OnMouseMove(ref linhas_facebaixo.pIni, Dados.face_insercao == 2, false, 0, PontoInicial);
            }
          //  Orto();

            linhas_facecima.pFin.Snap = false;
            linhas_facebaixo.pFin.Snap = false;
            linhas_eixo.pFin.Snap = false;

            linhas_facecima.UpdatePixel();
            linhas_facebaixo.UpdatePixel();
            linhas_eixo.UpdatePixel();
            pFin.Snap = false;

            this.pFin.x = point.x;
            this.pFin.y = point.y;
            this.pFin.z = point.z;

            base.OnMouseMove(ref pFin, ShowInfo);

  
            {
                TextoInfo.DesenhaSemZoom( point.x + 10, point.y + 10, "L: " + (this.comprimento / 100).ToString("n2") + " m, Δx: " + Math.Abs(this.pFin.x - this.pIni.x).ToString("n2") + " , Δy:  " + Math.Abs(this.pFin.y - this.pIni.y).ToString("n2"));
             //   mPen.DashStyle = DashStyle.Dash;
             //   mPen.DashPattern = dashValuesOnMouseMove;
           //     Cad.DrawLine(mPen, Desenho.pixelX((float)pIni.x), Desenho.pixelY((float)pIni.y), Desenho.pixelX((float)(pIni.x + (this.pFin.x - this.pIni.x)/2)), Desenho.pixelY((float)pIni.y));
             //   Cad.DrawArc(mPen, Desenho.pixelX((float)pIni.x), Desenho.pixelY((float)pIni.y), 150, 150, 0, (float)this.angulo);
            }
            // Cad.DrawLine(mPen,0,50,250,960);
        }
        double DifCoordX, DifCoordY, alfa;
        public void Orto()
        {
            //     if (pFin.Snap)
            //     MessageBox.Show("");
            alfa = (float)(FuncoesGerais.atand((pIni.y - pFin.y) / (pIni.x - pFin.x)));

            if (!pFin.Snap)
            {
                if (ConfiguracoesCaptura.Captura.OrtogonalLigado)
                {
                    if (alfa > 88)
                    {
                        alfa = 90;
                        this.angulo = 90;
                        pFin.x = pIni.x;
                    }
                    else
                        if (alfa > 0 && alfa < 2)
                        {
                            alfa = 0;
                            this.angulo = 0;
                            pFin.y = pIni.y;
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

        public void SetaCoords(TTrechoViga New, int face, TPonto pi, TPonto pf, ref TLinha linmain, ref double xx10, ref double yy10)
        {

            if (face == 0)
            {
                /*      
                x1,y1_______________x4,y4
                x2,y2_______________x3,y3             
                */
                New.linhas_eixo.pIni.x = (pi.x + (xx10 / 2));
                New.linhas_eixo.pIni.y = (pi.y + (yy10 / 2));
                New.linhas_eixo.pFin.x = (pf.x + (xx10 / 2));
                New.linhas_eixo.pFin.y = (pf.y + (yy10 / 2));

                New.linhas_facecima.pIni.x = (pi.x);
                New.linhas_facecima.pIni.y = (pi.y);
                New.linhas_facecima.pFin.x = (pf.x);
                New.linhas_facecima.pFin.y = (pf.y);

                New.linhas_facebaixo.pIni.x = (pi.x + xx10);
                New.linhas_facebaixo.pIni.y = (pi.y + yy10);
                New.linhas_facebaixo.pFin.x = (pf.x + xx10);
                New.linhas_facebaixo.pFin.y = (pf.y + yy10);

                New.linhas_facecima.pFin.Snap = pFin.Snap;
                
                linmain = linhas_facecima;
            }
            else
            if (face == 1)
            {
                New.linhas_facecima.pIni.x = (pi.x - (xx10 / 2));
                New.linhas_facecima.pIni.y = (pi.y - (yy10 / 2));
                New.linhas_facecima.pFin.x = (pf.x - (xx10 / 2));
                New.linhas_facecima.pFin.y = (pf.y - (yy10 / 2));

                New.linhas_facebaixo.pIni.x = (pi.x + (xx10 / 2));
                New.linhas_facebaixo.pIni.y = (pi.y + (yy10 / 2));
                New.linhas_facebaixo.pFin.x = (pf.x + (xx10 / 2));
                New.linhas_facebaixo.pFin.y = (pf.y + (yy10 / 2));
 
                New.linhas_eixo.pIni.x = (pi.x);
                New.linhas_eixo.pIni.y = (pi.y);
                New.linhas_eixo.pFin.x = (pf.x);
                New.linhas_eixo.pFin.y = (pf.y);
                New.linhas_eixo.pFin.Snap = pFin.Snap;
                linmain = linhas_eixo;
            }
            else
            if (face == 2)
            {

                New.linhas_facecima.pIni.x = (pi.x - xx10);
                New.linhas_facecima.pIni.y = (pi.y - yy10);

                New.linhas_eixo.pIni.x = (pi.x - (xx10 / 2));
                New.linhas_eixo.pIni.y = (pi.y - (yy10 / 2));

                New.linhas_facecima.pFin.x = (pf.x - xx10);
                New.linhas_facecima.pFin.y = (pf.y - yy10);

                New.linhas_eixo.pFin.x = (pf.x - (xx10 / 2));
                New.linhas_eixo.pFin.y = (pf.y - (yy10 / 2));

                New.linhas_facebaixo.pFin.x = (pf.x);
                New.linhas_facebaixo.pFin.y = (pf.y);
                New.linhas_facebaixo.pIni.x = (pi.x);
                New.linhas_facebaixo.pIni.y = (pi.y);
                New.linhas_facebaixo.pFin.Snap = pFin.Snap;
                
                linmain = linhas_facebaixo;
            }

            New.linhas_facebaixo.pFin.Snap = pFin.Snap;
            New.linhas_eixo.pFin.Snap      = pFin.Snap;
            New.linhas_facecima.pFin.Snap = pFin.Snap;
        }

        public void SetaCoords(TTrechoViga New, int face, TPonto pi, TPonto pf, ref double xx2, ref double yy2)
        {
            if (face == 0)
            {
                /*      
                x1,y1_______________x4,y4
                x2,y2_______________x3,y3             
                */
                New.linhas_eixo.pIni.x = (pi.x + (xx2 / 2));
                New.linhas_eixo.pIni.y = (pi.y + (yy2 / 2));
                New.linhas_eixo.pFin.x = (pf.x + (xx2 / 2));
                New.linhas_eixo.pFin.y = (pf.y + (yy2 / 2));

                New.linhas_facecima.pIni.x = (pi.x);
                New.linhas_facecima.pIni.y = (pi.y);
                New.linhas_facecima.pFin.x = (pf.x);
                New.linhas_facecima.pFin.y = (pf.y);

                New.linhas_facebaixo.pIni.x = (pi.x + xx2);
                New.linhas_facebaixo.pIni.y = (pi.y + yy2);
                New.linhas_facebaixo.pFin.x = (pf.x + xx2);
                New.linhas_facebaixo.pFin.y = (pf.y + yy2);

                New.linhas_facecima.pFin.Snap = pFin.Snap;

            } 
            else
            if (face == 1)
            {
                New.linhas_facecima.pIni.x = (pi.x - (xx2 / 2));
                New.linhas_facecima.pIni.y = (pi.y - (yy2 / 2));

                New.linhas_facebaixo.pIni.x = (pi.x + (xx2 / 2));
                New.linhas_facebaixo.pIni.y = (pi.y + (yy2 / 2));

                New.linhas_facebaixo.pFin.x = (pf.x + (xx2 / 2));
                New.linhas_facebaixo.pFin.y = (pf.y + (yy2 / 2));

                New.linhas_facecima.pFin.x = (pf.x - (xx2 / 2));
                New.linhas_facecima.pFin.y = (pf.y - (yy2 / 2));

                New.linhas_eixo.pIni.x = (pi.x);
                New.linhas_eixo.pIni.y = (pi.y);
                New.linhas_eixo.pFin.x = (pf.x);
                New.linhas_eixo.pFin.y = (pf.y);
                New.linhas_eixo.pFin.Snap = pFin.Snap;
            }
            else
            if (face == 2)
            {

                New.linhas_facecima.pIni.x = (pi.x - xx2);
                New.linhas_facecima.pIni.y = (pi.y - yy2);

                New.linhas_eixo.pIni.x = (pi.x - (xx2 / 2));
                New.linhas_eixo.pIni.y = (pi.y - (yy2 / 2));

                New.linhas_facecima.pFin.x = (pf.x - xx2);
                New.linhas_facecima.pFin.y = (pf.y - yy2);

                New.linhas_eixo.pFin.x = (pf.x - (xx2 / 2));
                New.linhas_eixo.pFin.y = (pf.y - (yy2 / 2));

                New.linhas_facebaixo.pFin.x = (pf.x);
                New.linhas_facebaixo.pFin.y = (pf.y);
                New.linhas_facebaixo.pIni.x = (pi.x);
                New.linhas_facebaixo.pIni.y = (pi.y);
                New.linhas_facebaixo.pFin.Snap = pFin.Snap;
            }

            New.linhas_facebaixo.pFin.Snap = pFin.Snap;
            New.linhas_eixo.pFin.Snap = pFin.Snap;
            New.linhas_facecima.pFin.Snap = pFin.Snap;
        }

        public override void Initialize(ref TPonto point, ref string command, ISettings Dados, TLayer layer, ref List<TLinha> Linhas, ref int pavimento)
        {
            TPonto point1 = new TPonto(point.x, point.y, point.z, point.px_x, point.px_y, -1);
            TPonto point2 = new TPonto(point.x, point.y, point.z, point.px_x, point.px_y, -1);
            TPonto point3 = new TPonto(point.x, point.y, point.z, point.px_x, point.px_y, -1);

            if (!point2.Incidente(this))
              point2.incidencias.Add(this);

            if (!point1.Incidente(this))
              point1.incidencias.Add(this);

            if (!point3.Incidente(this))
              point3.incidencias.Add(this);

            this.Dados = Dados as TDadosViga;

            linhas_facecima  = new TLinha(false);
            linhas_eixo      = new TLinha();
            linhas_facebaixo = new TLinha(false);

            linhas_eixo.LinhaEixoViga = true;

            linhas_eixo.TrechoViga      = this;
            linhas_facebaixo.TrechoViga = this;
            linhas_facecima.TrechoViga  = this;
           
            base.Tipo = Const.ID_TRECHOVIGA;

            linhas_facecima.Initialize(ref point1, ref command, null, layer, ref Linhas, ref pavimento);
            linhas_eixo.Initialize(ref point2, ref command, null, layer, ref Linhas, ref pavimento);
            linhas_facebaixo.Initialize(ref point3, ref command, null, layer, ref Linhas, ref pavimento);

            base.pIni  = point;
            this.layer = layer;
            this.Pavimento = pavimento;
            command = Const.CMD_TRECHOVIGA_2_P;
            base.Initialize(ref point, ref command, Dados, layer, ref Linhas, ref pavimento);


         /*   Texto1 = new TTexto(info2,
                                pt.x - 12.0,
                                pt.y + yy,
                               (15.0 / 80),
                              -(15.0 / 80),
                              r, g, b,
                              (float)New.angulo, New.Pavimento.LayersByIdPrincipal[Lay.TextosVigas], Const.ID_TRECHOVIGA);*/

            TextoInfo = new TTexto("Comp: ", point.x, point.y, point.z, 0.08, 0.08, 1, 1, 0, 0, layer, Const.ID_TRECHOVIGA);
        }

        public void SelecionaTodosElementos(TTrechoViga trecho)
        {
            trecho.SetaSelecao(true, false);

            trecho.Texto1.Grips[0].Selecionado = true;
            trecho.Texto1.Grips[1].Selecionado = true;
          //  trecho.Texto1.Grips[2].Selecionado = true;

            trecho.Grips[0].Selecionado = true;
            trecho.Grips[1].Selecionado = true;
           // trecho.Grips[2].Selecionado = true;
            
         //   trecho.linhas_facebaixo.TrechoViga = null;
         //   trecho.linhas_facecima.TrechoViga  = null;
         //   trecho.linhas_eixo.TrechoViga      = null;

            trecho.Texto2.Grips[0].Selecionado = true;
            trecho.Texto2.Grips[1].Selecionado = true;

            trecho.Texto1.Selecionado = true;
            trecho.Texto2.Selecionado = true;

          //  trecho.Texto2.Grips[2].Selecionado = true;
        }

        public override eObjetoDesenhoMouseDown OnMouseDown(ref TPonto point, ref string command, List<TLinha> Linhas, List<TPonto> Pontos, List<TPilar> Pilares, ref List<TObjetoDesenho> Objects, bool FromGrip = false)
        {

            if ((Geom.Iguais(point.x, this.pIni.x)) && (Geom.Iguais(point.y, this.pIni.y)) && (Geom.Iguais(point.z, this.pIni.z)))
            {
                return eObjetoDesenhoMouseDown.Continue;
            }

            double x = 0, y = 0, z = 0;

            double nx, ny;

            try
            {
                try
                {
                    int kk;

                    if (!point.Snap)
                        this.OnMouseMove(ref point, false);

                    if (!point.Incidente(this))
                        point.incidencias.Add(this);

                    this.pFin = (TPonto)point.Clone(); // new TPonto(point.x, point.y,0);
                    Piso = 1;
                    List<TLinha> Linhas2 = new List<TLinha>();
                    List<TTrechoViga> todos = new List<TTrechoViga>();
                    List<TPonto> tmpPt = new List<TPonto>();

                    TLinha main = null;

                    if (Geom.Iguais(pIni.z, pFin.z))  //só faz os testes de quebra se a viga for horizontal obviamente, e se os outros trechos forem no mesma cota
                    {
                        foreach (TLinha lin in Linhas)
                          if (!lin.auxiliar)
                            if (lin.TrechoViga != null)
                            {
                               if (Geom.Iguais(lin.pIni.z,pIni.z))
                                 Linhas2.Add(lin);
                            }
                    }

                    bool haIntersecao = false;

                    PontosEmLinha  = new List<TPonto>();
                    TrechosQuebrar = new List<TrechosQuebra>();

                    if (this.Dados.face_insercao == 0) main = this.linhas_eixo;
                    if (this.Dados.face_insercao == 1) main = this.linhas_eixo;
                    if (this.Dados.face_insercao == 2) main = this.linhas_eixo;

                    //procura pontos de interseções e grava na lista PontosEmLinha
                    //adiciona os trechos das outras vigas que houve as interseções e grava na lista TrechosQuebrar (trechos que serão quebrados em 2)
                    bool emLinhaIni, emLinhaFin = false;
                    foreach (TLinha lin in Linhas2)
                    {
                        emLinhaIni = (Geom.PontoEmLinha(FPrincipal.pixelX(main.pIni.x), FPrincipal.pixelY(main.pIni.y),
                                               FPrincipal.pixelX(lin.pIni.x), FPrincipal.pixelY(lin.pIni.y),
                                               FPrincipal.pixelX(lin.pFin.x), FPrincipal.pixelY(lin.pFin.y),2));

                        emLinhaFin = (Geom.PontoEmLinha(FPrincipal.pixelX(main.pFin.x), FPrincipal.pixelY(main.pFin.y),
                                               FPrincipal.pixelX(lin.pIni.x), FPrincipal.pixelY(lin.pIni.y),
                                               FPrincipal.pixelX(lin.pFin.x), FPrincipal.pixelY(lin.pFin.y), 2));

                        if (!lin.auxiliar && !lin.barraRigida)
                            if (lin.LinhaEixoViga)
                            {
                                if (lin.Intersec(main.pIni.x, main.pIni.y, main.pFin.x, main.pFin.y, ref x, ref y) || emLinhaIni || emLinhaFin)
                                {
                               //     if (lin.TrechoViga.NumViga == this.NumViga &&(!EhPontoExtremoLinha(lin, x, y)))
                                //        throw new Exception("A viga " + this.NumViga + " que você está inserindo está sobreposta com outro trecho dela mesma");
                                    z = main.pFin.z;

                                    if (lin.TrechoViga.Dados.numero != Dados.numero)
                                    {
                                        if (emLinhaIni)
                                        {
                                            x = main.pIni.x;
                                            y = main.pIni.y;
                                            z = main.pIni.z;
                                        }

                                        if (emLinhaFin)
                                        {
                                            x = main.pFin.x;
                                            y = main.pFin.y;
                                            z = main.pFin.z;
                                        }

                                        if (!EhPontoExtremoLinha(lin, x, y))
                                            TrechosQuebrar.Add(new TrechosQuebra(lin.TrechoViga, x, y,z));

                                        if (LocalizaPontoEmLinha(x, y) == -1)
                                            PontosEmLinha.Add(new TPonto(x, y, z, FPrincipal.pixelX(x), FPrincipal.pixelY(y), 0));

                                        haIntersecao = true;
                                    }
                                }
                            }
                    }

                    if (haIntersecao)
                    {
                        //verificar se os dois pontos extremos do trecho ja foram criados os nós, se nao foi, então 
                        //adiciona os trechos em que os pontos estão contidos no seu eixo e grava na lista TrechosQuebrar (trechos que serão divididos em 2)
                        if (LocalizaPontoEmLinha(main.pIni.x, main.pIni.y) == -1)
                        {
                            PontosEmLinha.Add(new TPonto(main.pIni.x, main.pIni.y, main.pIni.z, FPrincipal.pixelX(main.pIni.x), FPrincipal.pixelY(main.pIni.y), 0));

                            foreach (TLinha lin in Linhas2)
                            {
                                if (!lin.auxiliar && !lin.barraRigida)
                                    if (lin.LinhaEixoViga)
                                    {
                                        if (lin.TrechoViga.Dados.numero != this.Dados.numero &&
                                            Geom.PontoEmLinha(FPrincipal.pixelX(main.pIni.x), FPrincipal.pixelY(main.pIni.y),
                                                              FPrincipal.pixelX(lin.pIni.x), FPrincipal.pixelY(lin.pIni.y),
                                                              FPrincipal.pixelX(lin.pFin.x), FPrincipal.pixelY(lin.pFin.y),2))
                                        {
                                            if (!EhPontoExtremoLinha(lin, main.pIni.x, main.pIni.y))
                                                TrechosQuebrar.Add(new TrechosQuebra(lin.TrechoViga, main.pIni.x, main.pIni.y, main.pIni.z));
                                        }
                                    }
                            }
                        }

                        if (LocalizaPontoEmLinha(main.pFin.x, main.pFin.y) == -1)
                        {
                            PontosEmLinha.Add(new TPonto(main.pFin.x, main.pFin.y, main.pFin.z, FPrincipal.pixelX(main.pFin.x), FPrincipal.pixelY(main.pFin.y), 0));

                            foreach (TLinha lin in Linhas2)
                            {
                                if (!lin.auxiliar && !lin.barraRigida)
                                    if (lin.LinhaEixoViga)
                                    {
                                        if (lin.TrechoViga.Dados.numero != this.Dados.numero &&
                                            Geom.PontoEmLinha(FPrincipal.pixelX(main.pFin.x), FPrincipal.pixelY(main.pFin.y),
                                            FPrincipal.pixelX(lin.pIni.x), FPrincipal.pixelY(lin.pIni.y),
                                            FPrincipal.pixelX(lin.pFin.x), FPrincipal.pixelY(lin.pFin.y),2))
                                        {
                                            if (!EhPontoExtremoLinha(lin, main.pFin.x, main.pFin.y))
                                                TrechosQuebrar.Add(new TrechosQuebra(lin.TrechoViga, main.pFin.x, main.pFin.y, main.pFin.z));
                                        }
                                    }
                            }
                        }

                        ReordenaPontos(main.pIni.y, main.pFin.y, main.pIni.x, main.pFin.x);

                        beta = main.angulo;
                        if (linhas_eixo.pFin.x >= linhas_eixo.pIni.x)
                            beta -= 90;
                        else
                            beta += 90;

                        nx = (float)((Dados.b1) * Math.Cos(beta * Const.PIDiv180));
                        ny = (float)((Dados.b1) * Math.Sin(beta * Const.PIDiv180));

                        if ((beta == 0 && linhas_eixo.pFin.y < linhas_eixo.pIni.y) || (beta == -180 && linhas_eixo.pFin.y > linhas_eixo.pIni.y))
                            nx = -nx; 

                        /*Adiciona os trechos novos*/
                        for (kk = 0; kk < PontosEmLinha.Count - 1; kk++)
                        {
                            TTrechoViga New = new TTrechoViga();
                            TPonto p1, p2;

                            string ss = string.Empty;

                            if (this.Dados.face_insercao == 0) main = this.linhas_eixo;
                            if (this.Dados.face_insercao == 1) main = this.linhas_eixo;
                            if (this.Dados.face_insercao == 2) main = this.linhas_eixo;

                            if (kk == 0)
                                p1 = new TPonto(main.pIni.x, main.pIni.y, main.pIni.z, FPrincipal.pixelX(main.pIni.x), FPrincipal.pixelY(main.pIni.y), Pontos.Count);
                            else
                                p1 = new TPonto(PontosEmLinha[kk].x, PontosEmLinha[kk].y, PontosEmLinha[kk].z, FPrincipal.pixelX(PontosEmLinha[kk].x), FPrincipal.pixelY(PontosEmLinha[kk].y), Pontos.Count);
                            //    PontosEmLinha[kk];

                            tmpPt.Add(p1);

                            New.pIni = p1;
                            New.Initialize(ref p1, ref ss, this.Dados, this.layer, ref Linhas, ref Pavimento);

                            if (kk == (PontosEmLinha.Count - 2))
                                p2 = new TPonto(main.pFin.x, main.pFin.y, main.pFin.z, FPrincipal.pixelX(main.pFin.x), FPrincipal.pixelY(main.pFin.y), Pontos.Count);//this.pFin;
                            else
                                // p2 = PontosEmLinha[kk + 1];
                                p2 = new TPonto(PontosEmLinha[kk + 1].x, PontosEmLinha[kk + 1].y,PontosEmLinha[kk + 1].z, FPrincipal.pixelX(PontosEmLinha[kk + 1].x), FPrincipal.pixelY(PontosEmLinha[kk + 1].y), Pontos.Count);

                            tmpPt.Add(p2);

                            New.pFin = p2;

                            //   New.linhas_eixo.pIni = new TPonto(p1.x, p1.y, Desenho.pixelX(p1.x), Desenho.pixelY(p1.y), p1.codigo);
                            //   Pontos.Add(New.linhas_eixo.pIni); 
                            //      New.linhas_eixo.pFin = new TPonto(p2.x, p2.y, Desenho.pixelX(p2.x), Desenho.pixelY(p2.y), p2.codigo);
                            //  Pontos.Add(New.linhas_eixo.pFin);

                            SetaCoords(New, 1, p1, p2, ref nx, ref ny);

                            //  New.pFin.x = main.pFin.x;
                            //  New.pFin.y = main.pFin.y;

                            New.linhas_facecima.UpdatePixel();
                            New.linhas_facebaixo.UpdatePixel();
                            New.linhas_eixo.UpdatePixel();

                            if (!New.pFin.Incidente(New.linhas_eixo))
                                New.pFin.incidencias.Add(New.linhas_eixo);

                            if (!New.pIni.Incidente(New.linhas_eixo))
                                New.pIni.incidencias.Add(New.linhas_eixo);

                            New.angulo = (float)(FuncoesGerais.atand((linhas_eixo.pIni.y - linhas_eixo.pFin.y) / (linhas_eixo.pIni.x - linhas_eixo.pFin.x)));

                            New.linhas_facebaixo.angulo = (float)New.angulo;
                            New.linhas_eixo.angulo = (float)New.angulo;
                            New.linhas_facecima.angulo = (float)New.angulo;

                            New.linhas_eixo.pIni.PontoEixoViga = true;
                            New.linhas_eixo.pFin.PontoEixoViga = true;

                            New.linhas_facebaixo.pIni.PontoEixoViga = false;
                            New.linhas_facebaixo.pFin.PontoEixoViga = false;

                            New.linhas_facecima.pIni.PontoEixoViga = false;
                            New.linhas_facecima.pFin.PontoEixoViga = false;

                            New.linhas_facebaixo_org = new TLinha(new TPonto(New.linhas_facebaixo.pIni.x, New.linhas_facebaixo.pIni.y, New.linhas_facebaixo.pIni.z),
                                                                  new TPonto(New.linhas_facebaixo.pFin.x, New.linhas_facebaixo.pFin.y, New.linhas_facebaixo.pFin.z),
                                                                  -1);
                            New.linhas_eixo_org = new TLinha(new TPonto(New.linhas_eixo.pIni.x, New.linhas_eixo.pIni.y, New.linhas_eixo.pIni.z),
                                                                  new TPonto(New.linhas_eixo.pFin.x, New.linhas_eixo.pFin.y, New.linhas_eixo.pFin.z),
                                                                  -1);

                            New.linhas_facecima_org = new TLinha(new TPonto(New.linhas_facecima.pIni.x, New.linhas_facecima.pIni.y, New.linhas_facecima.pIni.z),
                                                                  new TPonto(New.linhas_facecima.pFin.x, New.linhas_facecima.pFin.y, New.linhas_facecima.pFin.z),
                                                                  -1);
                            New.AddGrips();

                            UpdateTodas(New);

                            UpdatePixelTodas(New);

                            New.CriaTextos(New, New.Dados.b1 + "/" + New.Dados.h1, New.Dados.nome + New.Dados.numero);
                            Objects.Add(New);
                            todos.Add(New);
                        }

                        /*Quebra em 2 os trechos em que houve interseção (na verdade, é apagado o trecho e criado dois novos...)*/
                        List<TTrechoViga> tmp = new List<TTrechoViga>();
                        TPonto ptIntersec = null;

                        for (kk = 0; kk < TrechosQuebrar.Count; kk++)
                        {
                            foreach (TPonto pt in tmpPt)
                                if (Geom.Iguais(pt.x, TrechosQuebrar[kk].x) && Geom.Iguais(pt.y, TrechosQuebrar[kk].y))
                                    ptIntersec = new TPonto(TrechosQuebrar[kk].x, TrechosQuebrar[kk].y, TrechosQuebrar[kk].z, FPrincipal.pixelX(TrechosQuebrar[kk].x), FPrincipal.pixelY(TrechosQuebrar[kk].y), 0);

                            ptIntersec.codigo = 666;
                            ////  if ((Object)ptIntersec == null)
                            //      ptIntersec = new TPonto(TrechosQuebrar[kk].x, TrechosQuebrar[kk].y, Desenho.pixelX(TrechosQuebrar[kk].x), Desenho.pixelY(TrechosQuebrar[kk].y), Pontos.Count);

                            tmpPt.Add(ptIntersec);

                            tmp.Add(TrechosQuebrar[kk].trecho);

                            SelecionaTodosElementos(TrechosQuebrar[kk].trecho);

                            PontosEmLinha.Clear();
                            PontosEmLinha.Add(new TPonto(ptIntersec.x, ptIntersec.y, ptIntersec.z, FPrincipal.pixelX(ptIntersec.x), FPrincipal.pixelY(ptIntersec.y), 0));

                            PontosEmLinha.Add(new TPonto(tmp[0].linhas_eixo.pIni.x, tmp[0].linhas_eixo.pIni.y, tmp[0].linhas_eixo.pIni.z, FPrincipal.pixelX(tmp[0].linhas_eixo.pIni.x), FPrincipal.pixelY(tmp[0].linhas_eixo.pIni.y), Pontos.Count));
                            tmpPt.Add(PontosEmLinha[PontosEmLinha.Count - 1]);

                            PontosEmLinha.Add(new TPonto(tmp[0].linhas_eixo.pFin.x, tmp[0].linhas_eixo.pFin.y, tmp[0].linhas_eixo.pFin.z, FPrincipal.pixelX(tmp[0].linhas_eixo.pFin.x), FPrincipal.pixelY(tmp[0].linhas_eixo.pFin.y), Pontos.Count));
                            tmpPt.Add(PontosEmLinha[PontosEmLinha.Count - 1]);

                            ReordenaPontos(tmp[0].linhas_eixo.pIni.y, tmp[0].linhas_eixo.pFin.y, tmp[0].linhas_eixo.pIni.x, tmp[0].linhas_eixo.pFin.x);

                            for (int jj = 0; jj < PontosEmLinha.Count - 1; jj++)
                            {
                      
                                beta = (float)(FuncoesGerais.atand((tmp[0].linhas_eixo.pIni.y - tmp[0].linhas_eixo.pFin.y) / (tmp[0].linhas_eixo.pIni.x - tmp[0].linhas_eixo.pFin.x)));
                                if (PontosEmLinha[jj + 1].x >= PontosEmLinha[jj].x)
                                    beta -= 90;
                                else
                                    beta += 90;
                                double seca = (tmp[0].Dados.b1) + tmp[0].Dados.excentricidade;

                                nx = (float)(seca * Math.Cos(beta * Const.PIDiv180));
                                ny = (float)(seca * Math.Sin(beta * Const.PIDiv180));

                                if ((beta == 0 && TrechosQuebrar[kk].trecho.pFin.y < TrechosQuebrar[kk].trecho.pIni.y) || (beta == -180 && TrechosQuebrar[kk].trecho.pFin.y > TrechosQuebrar[kk].trecho.pIni.y))
                                    nx = -nx; 
                                
                                TTrechoViga New = new TTrechoViga();
                                TPonto p1, p2;

                                string ss = string.Empty;

                                p1 = new TPonto(PontosEmLinha[jj].x, PontosEmLinha[jj].y, PontosEmLinha[jj].z, FPrincipal.pixelX(PontosEmLinha[jj].x), FPrincipal.pixelY(PontosEmLinha[jj].y), Pontos.Count);

                                New.pIni = p1;
                                New.Initialize(ref p1, ref ss, TrechosQuebrar[kk].trecho.Dados, TrechosQuebrar[kk].trecho.layer, ref Linhas, ref Pavimento);

                                p2 = new TPonto(PontosEmLinha[jj + 1].x, PontosEmLinha[jj + 1].y, PontosEmLinha[jj + 1].z, FPrincipal.pixelX(PontosEmLinha[jj + 1].x), FPrincipal.pixelY(PontosEmLinha[jj + 1].y), Pontos.Count);

                                New.pFin = p2;
                                SetaCoords(New, 1, p1, p2, ref nx, ref ny);

                                //   New.pFin.x = main.pFin.x;
                                //   New.pFin.y = main.pFin.y;

                                New.linhas_facecima.UpdatePixel();
                                New.linhas_facebaixo.UpdatePixel();
                                New.linhas_eixo.UpdatePixel();

                                if (!New.pFin.Incidente(New.linhas_eixo))
                                    New.pFin.incidencias.Add(New.linhas_eixo);

                                if (!New.pIni.Incidente(New.linhas_eixo))
                                    New.pIni.incidencias.Add(New.linhas_eixo);

                                New.angulo = (float)(FuncoesGerais.atand((New.linhas_eixo.pIni.y - New.linhas_eixo.pFin.y) / (New.linhas_eixo.pIni.x - New.linhas_eixo.pFin.x)));

                                New.linhas_facebaixo.angulo = (float)New.angulo;
                                New.linhas_eixo.angulo = (float)New.angulo;
                                New.linhas_facecima.angulo = (float)New.angulo;

                                New.linhas_facebaixo_org = new TLinha(new TPonto(New.linhas_facebaixo.pIni.x, New.linhas_facebaixo.pIni.y, New.linhas_facebaixo.pIni.z),
                                                                      new TPonto(New.linhas_facebaixo.pFin.x, New.linhas_facebaixo.pFin.y, New.linhas_facebaixo.pFin.z),
                                                                      -1);
                                New.linhas_eixo_org = new TLinha(new TPonto(New.linhas_eixo.pIni.x, New.linhas_eixo.pIni.y, New.linhas_eixo.pIni.z),
                                                                      new TPonto(New.linhas_eixo.pFin.x, New.linhas_eixo.pFin.y, New.linhas_eixo.pFin.z),
                                                                      -1);

                                New.linhas_facecima_org = new TLinha(new TPonto(New.linhas_facecima.pIni.x, New.linhas_facecima.pIni.y, New.linhas_facecima.pIni.z),
                                                                      new TPonto(New.linhas_facecima.pFin.x, New.linhas_facecima.pFin.y, New.linhas_facecima.pFin.z),
                                                                      -1);

                                New.AddGrips();

                                UpdateTodas(New);

                                UpdatePixelTodas(New);

                                New.CriaTextos(New, New.Dados.b1 + "/" + New.Dados.h1, New.Dados.nome + New.Dados.numero);
                                Objects.Add(New);
                                todos.Add(New);
                            }
                            tmp.Clear();
                        }

                        foreach (TLinha lin in Linhas2)
                        {
                            if ((Object)lin.TrechoViga != null)
                            {
                                bool achou = false;
                                foreach (TTrechoViga tr in todos)
                                    if ((Object)tr == (Object)lin.TrechoViga)
                                        achou = true;

                                if (!achou)
                                    todos.Add(lin.TrechoViga);
                            }
                        }

                    }
                    else
                    {
                        if (FromGrip)
                        {
                            this.SelecionaTodosElementos(this);
                            return eObjetoDesenhoMouseDown.Continue;
                        }

                        base.pFin = (TPonto)point.Clone();

                        if (!linhas_eixo.pFin.Incidente(linhas_eixo))
                            linhas_eixo.pFin.incidencias.Add(linhas_eixo);

                        if (!linhas_facecima.pFin.Incidente(linhas_facecima))
                            linhas_facecima.pFin.incidencias.Add(linhas_facecima);

                        if (!linhas_facebaixo.pFin.Incidente(linhas_facebaixo))
                            linhas_facebaixo.pFin.incidencias.Add(linhas_facebaixo);

                        //   linhas_eixo.pFin = point;

                        point.PontoEixoViga = true;


                        this.linhas_eixo.UpdatePixel();
                        this.linhas_facecima.UpdatePixel();
                        this.linhas_facebaixo.UpdatePixel();

                        base.angulo = (float)(FuncoesGerais.atand((linhas_eixo.pIni.y - linhas_eixo.pFin.y) / (linhas_eixo.pIni.x - linhas_eixo.pFin.x)));
                        base.anguloGlobal = (float)RMath.rad2deg(linhas_eixo.pIni.getAngleTo(linhas_eixo.pFin));
                        this.angulo = base.angulo;

                        beta = (float)this.angulo;
                        if (linhas_eixo.pFin.x >= linhas_eixo.pIni.x)
                            beta -= 90;
                        else
                            beta += 90;

                        nx = (float)((Dados.b1) * Math.Cos(beta * Const.PIDiv180));
                        ny = (float)((Dados.b1) * Math.Sin(beta * Const.PIDiv180));

                        if ((beta == 0 && linhas_eixo.pFin.y < linhas_eixo.pIni.y) || (beta == -180 && linhas_eixo.pFin.y > linhas_eixo.pIni.y))
                          nx = -nx; 

                        SetaCoords(this, Dados.face_insercao, pIni, pFin, ref main, ref nx, ref ny);


                        AddGrips();

                        CriaTextos(this, this.Dados.b1 + "/" + this.Dados.h1, this.Dados.nome + this.Dados.numero);

                        UpdateFCL(this);
                        UpdateFCL2(this);
                        UpdateFBL(this);
                        UpdateFBL2(this);

                        this.linhas_facebaixo_org = new TLinha(new TPonto(this.linhas_facebaixo.pIni.x, this.linhas_facebaixo.pIni.y, this.linhas_facebaixo.pIni.z),
                                              new TPonto(this.linhas_facebaixo.pFin.x, this.linhas_facebaixo.pFin.y, this.linhas_facebaixo.pFin.z),
                                              -1);
                        this.linhas_eixo_org = new TLinha(new TPonto(this.linhas_eixo.pIni.x, this.linhas_eixo.pIni.y, this.linhas_eixo.pIni.z),
                                                              new TPonto(this.linhas_eixo.pFin.x, this.linhas_eixo.pFin.y, this.linhas_eixo.pFin.z),
                                                              -1);

                        this.linhas_facecima_org = new TLinha(new TPonto(this.linhas_facecima.pIni.x, this.linhas_facecima.pIni.y, this.linhas_facecima.pIni.z),
                                                              new TPonto(this.linhas_facecima.pFin.x, this.linhas_facecima.pFin.y, this.linhas_facecima.pFin.z),
                                                              -1);

                        UpdateFEL(this);
                        UpdateFEL2(this);

                        UpdateFCM1(this);
                        UpdateFCM2(this);
                        UpdateFBM1(this);
                        UpdateFBM2(this);

                        UpdateFEM1(this);
                        UpdateFEM2(this);

                       
                        CriaLinhaEixoAux(this);

                        UpdateAnguloTodas(this);
                        
                        this.linhas_eixo.pIni.PontoEixoViga = true;
                        this.linhas_eixo.pFin.PontoEixoViga = true;

                        this.linhas_facebaixo.pIni.PontoEixoViga = false;
                        this.linhas_facebaixo.pFin.PontoEixoViga = false;

                        this.linhas_facecima.pIni.PontoEixoViga = false;
                        this.linhas_facecima.pFin.PontoEixoViga = false;

                        UpdatePixelTodas(this);

                        foreach (TLinha lin in Linhas2)
                        {
                            if ((Object)lin.TrechoViga != null)
                            {
                                bool achou = false;
                                foreach (TTrechoViga tr in todos)
                                    if ((Object)tr == (Object)lin.TrechoViga)
                                        achou = true;

                                if (!achou)
                                    todos.Add(lin.TrechoViga);
                            }
                        }

                        todos.Add(this);
                    }
                }
                catch (NullReferenceException e)
                {
                    MessageBox.Show("Existe algum trecho nullo \r" + e.Message.ToString(), "Erro", System.Windows.Forms.MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return eObjetoDesenhoMouseDown.Continue;
                }
            }
            catch (Exception e)
            {
                MessageBox.Show(e.Message.ToString(), "Erro", System.Windows.Forms.MessageBoxButtons.OK, MessageBoxIcon.Error);
                return eObjetoDesenhoMouseDown.Continue;
            }
            return eObjetoDesenhoMouseDown.DoneRepeat;
        }

        public override void OnMove() 
        { 

        }

        public void CalculaArea()
        {
            CoordenadaD[] c = new CoordenadaD[5];
            c[0].X = linhas_facebaixo.pIni.x;
            c[0].Y = linhas_facebaixo.pIni.y;
            c[1].X = linhas_facebaixo.pFin.x;
            c[1].Y = linhas_facebaixo.pFin.y;
            c[2].X = linhas_facecima.pFin.x;
            c[2].Y = linhas_facecima.pFin.y;
            c[3].X = linhas_facecima.pIni.x;
            c[3].Y = linhas_facecima.pIni.y;
            c[4].X = linhas_facebaixo.pIni.x;
            c[4].Y = linhas_facebaixo.pIni.y;

            TPoligono p = new TPoligono(c, true);
            this.areaSuperior = p.area / 10000;
        }

        public void UpdateTodas(TTrechoViga New)
        {
            UpdateFCL(New);
            UpdateFCL2(New);
            UpdateFBL(New);
            UpdateFBL2(New);
            UpdateFCM1(New);
            UpdateFCM2(New);
            UpdateFBM1(New);
            UpdateFBM2(New);

            UpdateFEM1(New);
            UpdateFEM2(New);
            
            UpdateFEL(New);
            UpdateFEL2(New);

            CriaLinhaEixoAux(New);

            UpdateAnguloTodas(New);
        }

        public void UpdateTrechosDasLinhas()
        {
            if (BarraRigida_Ini != null)  
              BarraRigida_Ini.TrechoViga = this;
            if (BarraRigida_Fin != null)
                BarraRigida_Fin.TrechoViga = this;
            linhas_facecima.TrechoViga = this;
            linhas_facebaixo.TrechoViga = this;
            linhas_eixo.TrechoViga = this;
            linhas_facecima_org.TrechoViga = this;
            linhas_facebaixo_org.TrechoViga = this;
            linhas_eixo_org.TrechoViga = this;
            linhas_facecima_longa.TrechoViga = this;
            linhas_facebaixo_longa.TrechoViga = this;
            if (linhas_eixo_longa != null)
              linhas_eixo_longa.TrechoViga = this;
            linhas_facecima_longa2.TrechoViga = this;
            linhas_facebaixo_longa2.TrechoViga = this;
            linhas_facecima_longa_m1.TrechoViga = this;
            linhas_facebaixo_longa_m1.TrechoViga = this;
            linhas_faceeixo_longa2.TrechoViga = this;
            linhas_faceeixo_longa.TrechoViga = this;
            linhas_facecima_longa_m2.TrechoViga = this;
            linhas_facebaixo_longa_m2.TrechoViga = this;
            linha_eixo_aux.TrechoViga = this;
            linhas_faceeixo_longa_m1.TrechoViga = this;
            linhas_faceeixo_longa_m2.TrechoViga = this;
        }

        private void CriaLinhaEixoAux(TTrechoViga New)
        {
            New.linha_eixo_aux = new TLinha(false);
            New.linha_eixo_aux.pIni.x = New.linhas_eixo_org.pIni.x;
            New.linha_eixo_aux.pIni.y = New.linhas_eixo_org.pIni.y;
            New.linha_eixo_aux.pFin.x = New.linhas_eixo_org.pFin.x;
            New.linha_eixo_aux.pFin.y = New.linhas_eixo_org.pFin.y;
            New.linha_eixo_aux.TrechoViga = New;
            New.linha_eixo_aux.Visivel = false;
        }

        public void UpdateAnguloTodas(TTrechoViga New)
        {
            New.linhas_facecima.angulo = (float)New.angulo;
            New.linhas_facebaixo.angulo = (float)New.angulo;
            New.linhas_eixo.angulo = (float)New.angulo;

            New.linhas_facecima_org.angulo = (float)New.angulo;
            New.linhas_facebaixo_org.angulo = (float)New.angulo;
            New.linhas_eixo_org.angulo = (float)New.angulo;

            New.linhas_facebaixo_longa.angulo = (float)New.angulo;
            New.linhas_facebaixo_longa2.angulo = (float)New.angulo;
            New.linhas_facebaixo_longa_m1.angulo = (float)New.angulo;
            New.linhas_facebaixo_longa_m2.angulo = (float)New.angulo;

            New.linhas_facecima_longa.angulo = (float)New.angulo;
            New.linhas_facecima_longa2.angulo = (float)New.angulo;
            New.linhas_facecima_longa_m2.angulo = (float)New.angulo;
            New.linhas_facecima_longa_m1.angulo = (float)New.angulo;

            New.linhas_faceeixo_longa2.angulo = (float)New.angulo;
            New.linha_eixo_aux.angulo = (float)New.angulo;
            New.linhas_faceeixo_longa.angulo = (float)New.angulo;
            New.linhas_faceeixo_longa_m2.angulo = (float)New.angulo;
            New.linhas_faceeixo_longa_m1.angulo = (float)New.angulo;
        }

        public void UpdateAnguloTodas2()
        {
            linhas_facecima.UpdateAnguloComprimento();
            linhas_facebaixo.UpdateAnguloComprimento();
            linhas_eixo.UpdateAnguloComprimento();

            linhas_facecima_org.UpdateAnguloComprimento();
            linhas_facebaixo_org.UpdateAnguloComprimento();
            linhas_eixo_org.UpdateAnguloComprimento();

            linhas_facebaixo_longa.UpdateAnguloComprimento();
            linhas_facebaixo_longa2.UpdateAnguloComprimento();
            linhas_facebaixo_longa_m1.UpdateAnguloComprimento();
            linhas_facebaixo_longa_m2.UpdateAnguloComprimento();

            linhas_facecima_longa.UpdateAnguloComprimento();
            linhas_facecima_longa2.UpdateAnguloComprimento();
            linhas_facecima_longa_m2.UpdateAnguloComprimento();
            linhas_facecima_longa_m1.UpdateAnguloComprimento();

            linhas_faceeixo_longa2.UpdateAnguloComprimento();
            linha_eixo_aux.UpdateAnguloComprimento();
            linhas_faceeixo_longa.UpdateAnguloComprimento();
            linhas_faceeixo_longa_m2.UpdateAnguloComprimento();
            linhas_faceeixo_longa_m1.UpdateAnguloComprimento();
        }

        public void UpdatePixelTodas(TTrechoViga New)
        {
            New.linhas_facecima.UpdatePixel();
            New.linhas_facebaixo.UpdatePixel();
            New.linhas_eixo.UpdatePixel();

            New.linhas_facecima_org.UpdatePixel();
            New.linhas_facebaixo_org.UpdatePixel();
            New.linhas_eixo_org.UpdatePixel();

            New.linhas_facebaixo_longa.UpdatePixel();
            New.linhas_facebaixo_longa2.UpdatePixel();
            New.linhas_facebaixo_longa_m1.UpdatePixel();
            New.linhas_facebaixo_longa_m2.UpdatePixel();

            New.linhas_facecima_longa.UpdatePixel();
            New.linhas_facecima_longa2.UpdatePixel();
            New.linhas_facecima_longa_m2.UpdatePixel();
            New.linhas_facecima_longa_m1.UpdatePixel();

            New.linhas_faceeixo_longa2.UpdatePixel();
            New.linhas_faceeixo_longa.UpdatePixel();
            New.linhas_faceeixo_longa_m2.UpdatePixel();
            New.linhas_faceeixo_longa_m1.UpdatePixel();

            UpdatePosicaoPontos(New.linhas_facecima_org);
            UpdatePosicaoPontos(New.linhas_eixo_org);
            UpdatePosicaoPontos(New.linhas_facebaixo_org);
        }

        public void UpdatePosicaoPontos(TLinha lin)
        {
            lin.pIni.PontoInicial = true;
            lin.pIni.PontoFinal   = false;
            lin.pFin.PontoInicial = false;
            lin.pFin.PontoFinal   = true;
        }

        public void UpdateFBM2(TTrechoViga New)
        {
            TPonto pm = New.linhas_facebaixo.getMiddlePoint();

            x1 = pm.x;
            y1 = pm.y;
            z1 = pm.z;

            x4 = New.linhas_facebaixo.pIni.x;
            y4 = New.linhas_facebaixo.pIni.y;
            z4 = New.linhas_facebaixo.pIni.z;

            if (Math.Abs(x4 - x1) != 0)
              tg0 = (y4 - y1) / (x4 - x1);

            novox2 = x1 + prolongamento;
            novoy2 = y1 + (prolongamento * tg0);

            if (x4 - x1 == 0)
            {
                novox2 = x1;
                if (New.linhas_eixo.pFin.y < New.linhas_eixo.pIni.y)
                    novoy2 = y1 - prolongamento;
            }
            if (New.linhas_eixo.pFin.x < New.linhas_eixo.pIni.x)
            {
                if (x4 - x1 != 0)
                    tg0 = (y4 - y1) / (x4 - x1);

                novox2 = x1 - prolongamento;
                novoy2 = y1 - (prolongamento * tg0);

                if (x4 - x1 == 0)
                    novox2 = x1;
            }

            TPonto pt7 = new TPonto(x1, y1, z1);
            TPonto pt8 = new TPonto(novox2, novoy2, z1);

            if (New.linhas_facebaixo_longa_m2 == null)
                New.linhas_facebaixo_longa_m2 = new TLinha(pt7, pt8, -1);
            else
            {
                New.linhas_facebaixo_longa_m2.pIni.x = x1;
                New.linhas_facebaixo_longa_m2.pIni.y = y1;
                New.linhas_facebaixo_longa_m2.pIni.z = z1;
                New.linhas_facebaixo_longa_m2.pFin.x = novox2;
                New.linhas_facebaixo_longa_m2.pFin.y = novoy2;
                New.linhas_facebaixo_longa_m2.pFin.z = z1;
                New.linhas_facebaixo_longa_m2.UpdatePixel();
            }

            New.linhas_facebaixo_longa_m2.TrechoViga = New;
        }

        public void UpdateFBM1(TTrechoViga New)
        {
            TPonto pm = New.linhas_facebaixo.getMiddlePoint();

            x1 = pm.x;
            y1 = pm.y;
            z1 = pm.z;

            x4 = New.linhas_facebaixo.pFin.x;
            y4 = New.linhas_facebaixo.pFin.y;
            z4 = New.linhas_facebaixo.pFin.z;

            if (x4 - x1 != 0)
                tg0 = (y4 - y1) / (x4 - x1);

            novox2 = x1 - prolongamento;//x4 + prolongamento;
            novoy2 = y1 - (prolongamento * tg0);//y4 + (prolongamento * tg0);

            if (x4 - x1 == 0)
            {
                novox2 = x1;
                if (New.linhas_eixo.pFin.y < New.linhas_eixo.pIni.y)
                    novoy2 = y1 + prolongamento;
            }
            if (New.linhas_eixo.pFin.x < New.linhas_eixo.pIni.x)
            {
                if (x4 - x1 != 0)
                    tg0 = (y4 - y1) / (x4 - x1);

                novox2 = x1 + prolongamento;
                novoy2 = y1 + (prolongamento * tg0);

                if (x4 - x1 == 0)
                    novox2 = x1;
            }

            TPonto pt3 = new TPonto(x1, y1, z1);
            TPonto pt4 = new TPonto(novox2, novoy2, z1);

            if (New.linhas_facebaixo_longa_m1 == null)
                New.linhas_facebaixo_longa_m1 = new TLinha(pt3, pt4, -1);
            else
            {
                New.linhas_facebaixo_longa_m1.pIni.x = x1;
                New.linhas_facebaixo_longa_m1.pIni.y = y1;
                New.linhas_facebaixo_longa_m1.pIni.z = z1;

                New.linhas_facebaixo_longa_m1.pFin.x = novox2;
                New.linhas_facebaixo_longa_m1.pFin.y = novoy2;
                New.linhas_facebaixo_longa_m1.pFin.z = z1;
                New.linhas_facebaixo_longa_m1.UpdatePixel();
            }

            New.linhas_facebaixo_longa_m1.TrechoViga = New;
        }

        public void UpdateFBL2(TTrechoViga New)
        {
            x1 = New.linhas_facebaixo.pFin.x;
            y1 = New.linhas_facebaixo.pFin.y;
            z1 = New.linhas_facebaixo.pFin.z;

            x4 = New.linhas_facebaixo.pIni.x;
            y4 = New.linhas_facebaixo.pIni.y;
            z4 = New.linhas_facebaixo.pIni.z;

            if (Math.Abs(x4 - x1) != 0)
                tg0 = (y4 - y1) / (x4 - x1);

            novox2 = x1 + prolongamento;
            novoy2 = y1 + (prolongamento * tg0);

            if (x4 - x1 == 0)
            {
                novox2 = x1;
                if (New.linhas_eixo.pFin.y < New.linhas_eixo.pIni.y)
                    novoy2 = y1 - prolongamento;
            }
            if (New.linhas_eixo.pFin.x < New.linhas_eixo.pIni.x)
            {
                if (x4 - x1 != 0)
                    tg0 = (y4 - y1) / (x4 - x1);

                novox2 = x1 - prolongamento;
                novoy2 = y1 - (prolongamento * tg0);

                if (x4 - x1 == 0)
                    novox2 = x1;
            }

            TPonto pt7 = new TPonto(x1, y1,z1);
            TPonto pt8 = new TPonto(novox2, novoy2, z1);

            if (New.linhas_facebaixo_longa2 == null)
                New.linhas_facebaixo_longa2 = new TLinha(pt7, pt8, -1);
            else
            {
                New.linhas_facebaixo_longa2.pIni.x = x1;
                New.linhas_facebaixo_longa2.pIni.y = y1;
                New.linhas_facebaixo_longa2.pIni.z = z1;
                New.linhas_facebaixo_longa2.pFin.x = novox2;
                New.linhas_facebaixo_longa2.pFin.y = novoy2;
                New.linhas_facebaixo_longa2.pFin.z = z1;
                New.linhas_facebaixo_longa2.UpdatePixel();
            }

            New.linhas_facebaixo_longa2.TrechoViga = New;
        }

        public void UpdateFBL(TTrechoViga New)
        {
            x1 = New.linhas_facebaixo.pIni.x;
            y1 = New.linhas_facebaixo.pIni.y;
            z1 = New.linhas_facebaixo.pIni.z;

            x4 = New.linhas_facebaixo.pFin.x;
            y4 = New.linhas_facebaixo.pFin.y;
            z4 = New.linhas_facebaixo.pFin.z;

            if (x4 - x1 != 0)
                tg0 = (y4 - y1) / (x4 - x1);

            novox2 = x1 - prolongamento;//x4 + prolongamento;
            novoy2 = y1 - (prolongamento * tg0);//y4 + (prolongamento * tg0);

            if (x4 - x1 == 0)
            {
                novox2 = x1;
                if (New.linhas_eixo.pFin.y < New.linhas_eixo.pIni.y)
                    novoy2 = y1 + prolongamento;
            }
            if (New.linhas_eixo.pFin.x < New.linhas_eixo.pIni.x)
            {
                if (x4 - x1 != 0)
                    tg0 = (y4 - y1) / (x4 - x1);

                novox2 = x1 + prolongamento;
                novoy2 = y1 + (prolongamento * tg0);

                if (x4 - x1 == 0)
                    novox2 = x1;
            }

            TPonto pt3 = new TPonto(x1, y1, z1);
            TPonto pt4 = new TPonto(novox2, novoy2, z1);

            if (New.linhas_facebaixo_longa == null)
                New.linhas_facebaixo_longa = new TLinha(pt3, pt4, -1);
            else
            {
                New.linhas_facebaixo_longa.pIni.x = x1;
                New.linhas_facebaixo_longa.pIni.y = y1;
                New.linhas_facebaixo_longa.pIni.z = z1;
                New.linhas_facebaixo_longa.pFin.x = novox2;
                New.linhas_facebaixo_longa.pFin.y = novoy2;
                New.linhas_facebaixo_longa.pFin.z = z1;
                New.linhas_facebaixo_longa.UpdatePixel();
            }

            New.linhas_facebaixo_longa.TrechoViga = New;
        }

        public void UpdateFCL2(TTrechoViga New)
        {
             x1 = New.linhas_facecima.pFin.x;
             y1 = New.linhas_facecima.pFin.y;
             z1 = New.linhas_facecima.pFin.z;

             x4 = New.linhas_facecima.pIni.x;
             y4 = New.linhas_facecima.pIni.y;
             z4 = New.linhas_facecima.pIni.z;

             if (Math.Abs(x4 - x1) != 0)
                 tg0 = (y4 - y1) / (x4 - x1);

             novox2 = x1 + prolongamento;//x4 + prolongamento;
             novoy2 = y1 + (prolongamento * tg0);//y4 + (prolongamento * tg0);

             if (x4 - x1 == 0)
             {
                 novox2 = x1;
                 if (New.linhas_eixo.pFin.y < New.linhas_eixo.pIni.y)
                     novoy2 = y1 - prolongamento;
             }
             if (New.linhas_eixo.pFin.x < New.linhas_eixo.pIni.x)
             {
                 if (x4 - x1 != 0)
                     tg0 = (y4 - y1) / (x4 - x1);

                 novox2 = x1 - prolongamento;
                 novoy2 = y1 - (prolongamento * tg0);

                 if (x4 - x1 == 0)
                     novox2 = x1;
             }

             TPonto pt5 = new TPonto(x1, y1, z1);
             TPonto pt6 = new TPonto(novox2, novoy2, z1);

             if (New.linhas_facecima_longa2 == null)
                 New.linhas_facecima_longa2 = new TLinha(pt5, pt6, -1);
             else
             {
                 New.linhas_facecima_longa2.pIni.x = x1;
                 New.linhas_facecima_longa2.pIni.y = y1;
                 New.linhas_facecima_longa2.pIni.z = z1;
                 New.linhas_facecima_longa2.pFin.x = novox2;
                 New.linhas_facecima_longa2.pFin.y = novoy2;
                 New.linhas_facecima_longa2.pFin.z = z1;
                 New.linhas_facecima_longa2.UpdatePixel();
             }

             New.linhas_facecima_longa2.TrechoViga = New;
        }

        public void UpdateFCL(TTrechoViga New)
        {
            //face de cima 1
            x1 = New.linhas_facecima.pIni.x;
            y1 = New.linhas_facecima.pIni.y;
            z1 = New.linhas_facecima.pIni.z;

            x4 = New.linhas_facecima.pFin.x;
            y4 = New.linhas_facecima.pFin.y;
            z4 = New.linhas_facecima.pFin.z;

            if (x4 - x1 != 0)
                tg0 = (y4 - y1) / (x4 - x1);

            novox2 = x1 - prolongamento;//x4 + prolongamento;
            novoy2 = y1 - (prolongamento * tg0);//y4 + (prolongamento * tg0);

            if (x4 - x1 == 0)
            {
                novox2 = x1;
                if (New.linhas_eixo.pFin.y < New.linhas_eixo.pIni.y)
                    novoy2 = y1 + prolongamento;
            }
            if (New.linhas_eixo.pFin.x < New.linhas_eixo.pIni.x)
            {
                if (x4 - x1 != 0)
                    tg0 = (y4 - y1) / (x4 - x1);

                novox2 = x1 + prolongamento;
                novoy2 = y1 + (prolongamento * tg0);

                if (x4 - x1 == 0)
                    novox2 = x1;
            }

            TPonto pt1 = new TPonto(x1, y1, z1);
            TPonto pt2 = new TPonto(novox2, novoy2, z1);

            if (New.linhas_facecima_longa == null)
                New.linhas_facecima_longa = new TLinha(pt1, pt2, -1);
            else
            {
                New.linhas_facecima_longa.pIni.x = x1;
                New.linhas_facecima_longa.pIni.y = y1;
                New.linhas_facecima_longa.pIni.z = z1;
                New.linhas_facecima_longa.pFin.x = novox2;
                New.linhas_facecima_longa.pFin.y = novoy2;
                New.linhas_facecima_longa.pFin.z = z1;
                New.linhas_facecima_longa.UpdatePixel();
            }

            New.linhas_facecima_longa.TrechoViga = New;
        }

        public void UpdateFEL2(TTrechoViga New)
        {
            x1 = New.linhas_eixo.pFin.x;
            y1 = New.linhas_eixo.pFin.y;
            z1 = New.linhas_eixo.pFin.z;

            x4 = New.linhas_eixo.pIni.x;
            y4 = New.linhas_eixo.pIni.y;
            z4 = New.linhas_eixo.pIni.z;

            if (Math.Abs(x4 - x1) != 0)
                tg0 = (y4 - y1) / (x4 - x1);

            novox2 = x1 + prolongamento;//x4 + prolongamento;
            novoy2 = y1 + (prolongamento * tg0);//y4 + (prolongamento * tg0);

            if (x4 - x1 == 0)
            {
                novox2 = x1;
                if (New.linhas_eixo.pFin.y < New.linhas_eixo.pIni.y)
                    novoy2 = y1 - prolongamento;
            }
            if (New.linhas_eixo.pFin.x < New.linhas_eixo.pIni.x)
            {
                if (x4 - x1 != 0)
                    tg0 = (y4 - y1) / (x4 - x1);

                novox2 = x1 - prolongamento;
                novoy2 = y1 - (prolongamento * tg0);

                if (x4 - x1 == 0)
                    novox2 = x1;
            }

            TPonto pt5 = new TPonto(x1, y1, z1);
            TPonto pt6 = new TPonto(novox2, novoy2, z1);

            if (New.linhas_faceeixo_longa2 == null)
                New.linhas_faceeixo_longa2 = new TLinha(pt5, pt6, -1);
            else
            {
                New.linhas_faceeixo_longa2.pIni.x = x1;
                New.linhas_faceeixo_longa2.pIni.y = y1;
                New.linhas_faceeixo_longa2.pIni.z = z1;
                New.linhas_faceeixo_longa2.pFin.x = novox2;
                New.linhas_faceeixo_longa2.pFin.y = novoy2;
                New.linhas_faceeixo_longa2.pFin.z = z1;
                New.linhas_faceeixo_longa2.UpdatePixel();
            }

            New.linhas_faceeixo_longa2.TrechoViga = New;
        }

        public void UpdateFEL(TTrechoViga New)
        {
            //face de cima 1
            x1 = New.linhas_eixo.pIni.x;
            y1 = New.linhas_eixo.pIni.y;
            z1 = New.linhas_eixo.pIni.z;

            x4 = New.linhas_eixo.pFin.x;
            y4 = New.linhas_eixo.pFin.y;
            z4 = New.linhas_eixo.pFin.z;

            if (x4 - x1 != 0)
                tg0 = (y4 - y1) / (x4 - x1);

            novox2 = x1 - prolongamento;//x4 + prolongamento;
            novoy2 = y1 - (prolongamento * tg0);//y4 + (prolongamento * tg0);

            if (x4 - x1 == 0)
            {
                novox2 = x1;
                if (New.linhas_eixo.pFin.y < New.linhas_eixo.pIni.y)
                    novoy2 = y1 + prolongamento;
            }
            if (New.linhas_eixo.pFin.x < New.linhas_eixo.pIni.x)
            {
                if (x4 - x1 != 0)
                    tg0 = (y4 - y1) / (x4 - x1);

                novox2 = x1 + prolongamento;
                novoy2 = y1 + (prolongamento * tg0);

                if (x4 - x1 == 0)
                    novox2 = x1;
            }

            TPonto pt1 = new TPonto(x1, y1, z1);
            TPonto pt2 = new TPonto(novox2, novoy2, z1);

            if (New.linhas_faceeixo_longa == null)
                New.linhas_faceeixo_longa = new TLinha(pt1, pt2, -1);
            else
            {
                New.linhas_faceeixo_longa.pIni.x = x1;
                New.linhas_faceeixo_longa.pIni.y = y1;
                New.linhas_faceeixo_longa.pIni.z = z1;
                New.linhas_faceeixo_longa.pFin.x = novox2;
                New.linhas_faceeixo_longa.pFin.y = novoy2;
                New.linhas_faceeixo_longa.pFin.z = z1;
                New.linhas_faceeixo_longa.UpdatePixel();
            }

            New.linhas_faceeixo_longa.TrechoViga = New;
        }
        
        public void UpdateFCM1(TTrechoViga New)
        {
            TPonto pm = New.linhas_facecima_org.getMiddlePoint();

            x1 = pm.x;
            y1 = pm.y;
            z1 = pm.z;

            x4 = New.linhas_facecima.pFin.x;
            y4 = New.linhas_facecima.pFin.y;
            z4 = New.linhas_facecima.pFin.z;

            if (x4 - x1 != 0)
                tg0 = (y4 - y1) / (x4 - x1);

            novox2 = x1 - prolongamento;//x4 + prolongamento;
            novoy2 = y1 - (prolongamento * tg0);//y4 + (prolongamento * tg0);

            if (x4 - x1 == 0)
            {
                novox2 = x1;
                if (New.linhas_eixo.pFin.y < New.linhas_eixo.pIni.y)
                    novoy2 = y1 + prolongamento;
            }
            if (New.linhas_eixo.pFin.x < New.linhas_eixo.pIni.x)
            {
                if (x4 - x1 != 0)
                    tg0 = (y4 - y1) / (x4 - x1);

                novox2 = x1 + prolongamento;
                novoy2 = y1 + (prolongamento * tg0);

                if (x4 - x1 == 0)
                    novox2 = x1;
            }

            TPonto pt1 = new TPonto(x1, y1, z1);
            TPonto pt2 = new TPonto(novox2, novoy2, z1);

            if (New.linhas_facecima_longa_m1 == null)
                New.linhas_facecima_longa_m1 = new TLinha(pt1, pt2, -1);
            else
            {
                New.linhas_facecima_longa_m1.pIni.x = x1;
                New.linhas_facecima_longa_m1.pIni.y = y1;
                New.linhas_facecima_longa_m1.pIni.z = z1;
                New.linhas_facecima_longa_m1.pFin.x = novox2;
                New.linhas_facecima_longa_m1.pFin.y = novoy2;
                New.linhas_facecima_longa_m1.pFin.z = z1;
                New.linhas_facecima_longa_m1.UpdatePixel();
            }

            New.linhas_facecima_longa_m1.TrechoViga = New;
        }

        public void UpdateFCM2(TTrechoViga New)
        {
            TPonto pm = New.linhas_facecima.getMiddlePoint();

            x1 = pm.x;
            y1 = pm.y;
            z1 = pm.z;

            x4 = New.linhas_facecima.pIni.x;
            y4 = New.linhas_facecima.pIni.y;
            z4 = New.linhas_facecima.pIni.z;

            if (Math.Abs(x4 - x1) != 0)
                tg0 = (y4 - y1) / (x4 - x1);

            novox2 = x1 + prolongamento;//x4 + prolongamento;
            novoy2 = y1 + (prolongamento * tg0);//y4 + (prolongamento * tg0);

            if (x4 - x1 == 0)
            {
                novox2 = x1;
                if (New.linhas_eixo.pFin.y < New.linhas_eixo.pIni.y)
                    novoy2 = y1 - prolongamento;
            }
            if (New.linhas_eixo.pFin.x < New.linhas_eixo.pIni.x)
            {
                if (x4 - x1 != 0)
                    tg0 = (y4 - y1) / (x4 - x1);

                novox2 = x1 - prolongamento;
                novoy2 = y1 - (prolongamento * tg0);

                if (x4 - x1 == 0)
                    novox2 = x1;
            }

            TPonto pt5 = new TPonto(x1, y1, z1);
            TPonto pt6 = new TPonto(novox2, novoy2, z1);

            if (New.linhas_facecima_longa_m2 == null)
                New.linhas_facecima_longa_m2 = new TLinha(pt5, pt6, -1);
            else
            {
                New.linhas_facecima_longa_m2.pIni.x = x1;
                New.linhas_facecima_longa_m2.pIni.y = y1;
                New.linhas_facecima_longa_m2.pIni.z = z1;
                New.linhas_facecima_longa_m2.pFin.x = novox2;
                New.linhas_facecima_longa_m2.pFin.y = novoy2;
                New.linhas_facecima_longa_m2.pFin.z = z1;
                New.linhas_facecima_longa_m2.UpdatePixel();
            }

            New.linhas_facecima_longa_m2.TrechoViga = New;
        }
        //
        public void UpdateFEM1(TTrechoViga New)
        {
            TPonto pm = New.linhas_eixo_org.getMiddlePoint();

            x1 = pm.x;
            y1 = pm.y;
            z1 = pm.z;

            x4 = New.linhas_eixo.pFin.x;
            y4 = New.linhas_eixo.pFin.y;
            z4 = New.linhas_eixo.pFin.z;

            if (x4 - x1 != 0)
                tg0 = (y4 - y1) / (x4 - x1);

            novox2 = x1 - prolongamento;//x4 + prolongamento;
            novoy2 = y1 - (prolongamento * tg0);//y4 + (prolongamento * tg0);

            if (x4 - x1 == 0)
            {
                novox2 = x1;
                if (New.linhas_eixo.pFin.y < New.linhas_eixo.pIni.y)
                    novoy2 = y1 + prolongamento;
            }
            if (New.linhas_eixo.pFin.x < New.linhas_eixo.pIni.x)
            {
                if (x4 - x1 != 0)
                    tg0 = (y4 - y1) / (x4 - x1);

                novox2 = x1 + prolongamento;
                novoy2 = y1 + (prolongamento * tg0);

                if (x4 - x1 == 0)
                    novox2 = x1;
            }

            TPonto pt666 = new TPonto(x1, y1, z1);
            TPonto pt667 = new TPonto(novox2, novoy2, z1);

            if (New.linhas_faceeixo_longa_m1 == null)
              New.linhas_faceeixo_longa_m1 = new TLinha(pt666, pt667, -1);
            else
            {
                New.linhas_faceeixo_longa_m1.pIni.x = x1;
                New.linhas_faceeixo_longa_m1.pIni.y = y1;
                New.linhas_faceeixo_longa_m1.pIni.z = z1;
                New.linhas_faceeixo_longa_m1.pFin.x = novox2;
                New.linhas_faceeixo_longa_m1.pFin.y = novoy2;
                New.linhas_faceeixo_longa_m1.pFin.z = z1;
                New.linhas_faceeixo_longa_m1.UpdatePixel();
            }

            New.linhas_faceeixo_longa_m1.TrechoViga = New;
            New.linhas_faceeixo_longa_m1.layer = New.layer;
        }

        public void UpdateFEM2(TTrechoViga New)
        {
            TPonto pm = New.linhas_eixo.getMiddlePoint();

            x1 = pm.x;
            y1 = pm.y;
            z1 = pm.z;

            x4 = New.linhas_eixo.pIni.x;
            y4 = New.linhas_eixo.pIni.y;
            z4 = New.linhas_eixo.pIni.z;

            if (Math.Abs(x4 - x1) != 0)
                tg0 = (y4 - y1) / (x4 - x1);

            novox2 = x1 + prolongamento;//x4 + prolongamento;
            novoy2 = y1 + (prolongamento * tg0);//y4 + (prolongamento * tg0);

            if (x4 - x1 == 0)
            {
                novox2 = x1;
                if (New.linhas_eixo.pFin.y < New.linhas_eixo.pIni.y)
                    novoy2 = y1 - prolongamento;
            }
            if (New.linhas_eixo.pFin.x < New.linhas_eixo.pIni.x)
            {
                if (x4 - x1 != 0)
                    tg0 = (y4 - y1) / (x4 - x1);

                novox2 = x1 - prolongamento;
                novoy2 = y1 - (prolongamento * tg0);

                if (x4 - x1 == 0)
                    novox2 = x1;
            }

            TPonto pt555 = new TPonto(x1, y1, z1);
            TPonto pt668 = new TPonto(novox2, novoy2, z1);

            if (New.linhas_faceeixo_longa_m2 == null)
                New.linhas_faceeixo_longa_m2 = new TLinha(pt555, pt668, -1);
            else
            {
                New.linhas_faceeixo_longa_m2.pIni.x = x1;
                New.linhas_faceeixo_longa_m2.pIni.y = y1;
                New.linhas_faceeixo_longa_m2.pIni.z = z1;
                New.linhas_faceeixo_longa_m2.pFin.x = novox2;
                New.linhas_faceeixo_longa_m2.pFin.y = novoy2;
                New.linhas_faceeixo_longa_m2.pFin.z = z1;
                New.linhas_faceeixo_longa_m2.UpdatePixel();
            }

            New.linhas_faceeixo_longa_m2.TrechoViga = New;
            New.linhas_faceeixo_longa_m2.layer = New.layer;
        }
        //
        public override void SetaSelecao(bool s, bool MostraGrip,bool SelecaoDeCandidato=false, TObjetoDesenho Owner = null)
        {
            if (this.layer.Congelado || this.layer.Travado)
              return;
            base.CandidatoSelecao = SelecaoDeCandidato;
            base.MostrarGrip = MostraGrip;

            if ((Object)Texto1 != null)
                Texto1.SetaSelecao(s, MostraGrip, SelecaoDeCandidato);
            if ((Object)Texto2 != null)
                Texto2.SetaSelecao(s, MostraGrip, SelecaoDeCandidato);
            
            base.Selecionado = s;

            linhas_facecima.Selecionado = s;
            linhas_eixo.Selecionado = s;
            linhas_facebaixo.Selecionado = s;

            linhas_facecima_org.Selecionado       = s; 
            linhas_facebaixo_org.Selecionado      = s; 
            linhas_eixo_org.Selecionado           = s;
            
            linhas_facebaixo_longa.Selecionado    = s;
            linhas_facebaixo_longa2.Selecionado   = s;
            linhas_facebaixo_longa_m1.Selecionado = s;
            linhas_facebaixo_longa_m2.Selecionado = s;

            linhas_facecima_longa.Selecionado     = s;
            linhas_facecima_longa2.Selecionado    = s;
            linhas_facecima_longa_m2.Selecionado  = s;
            linhas_facecima_longa_m1.Selecionado  = s; 
    
            linhas_faceeixo_longa2.Selecionado    = s;
            linhas_faceeixo_longa.Selecionado     = s;
            linhas_faceeixo_longa_m2.Selecionado  = s;
            linhas_faceeixo_longa_m1.Selecionado  = s;
            
            //   linhas_facecima.pIni.Selecionado = true;
         //   linhas_facecima.pFin.Selecionado = true;
         //   linhas_eixo.pIni.Selecionado = true;
         //   linhas_eixo.pFin.Selecionado = true;
         //   linhas_facebaixo.pIni.Selecionado = true;
         //   linhas_facebaixo.pFin.Selecionado = true;
            base.MostrarGrip = MostraGrip;
            ShowHideGrips(MostraGrip);

        }

        public override void ShowHideGrips(bool visivel)
        {
            foreach (TGrip grip in Grips)  
              grip.Visivel = visivel;
        }

        public override void AddGrips()
        {
            Grips = new List<TGrip>();

            Grips.Add(new TGrip(this, this.linhas_eixo.pIni.x, this.linhas_eixo.pIni.y, false, false, false, true, this.layer));
            Grips[0].gripFinalViga = false;
            //  Grips.Add(new TGrip(this, this.linhas_eixo.getMiddlePoint().x, this.linhas_eixo.getMiddlePoint().y, true, false, false, false, this.layer));
            Grips.Add(new TGrip(this, this.linhas_eixo.pFin.x, this.linhas_eixo.pFin.y, false, false, false, true, this.layer));
            Grips[1].gripFinalViga = true;
        }

        public void UpdateGrips()
        {
            Grips[0].x = this.linhas_eixo.pIni.x;
            Grips[0].y = this.linhas_eixo.pIni.y;
         //   Grips[1].x = this.linhas_eixo.getMiddlePoint().x;
         //   Grips[1].y = this.linhas_eixo.getMiddlePoint().y;
            Grips[1].x = this.linhas_eixo.pFin.x;
            Grips[1].y = this.linhas_eixo.pFin.y;
        }
        double cenx;
        int texture;                             // Storage For One Texture ( NEW )
        public void CarregaTexturas()
        {
         /*   bool status = false;                                                // Status Indicator
            Bitmap[] textureImage = new Bitmap[1];                              // Create Storage Space For The Texture

            textureImage[0] = LoadBMP("NeHe");                // Load The Bitmap
            // Check For Errors, If Bitmap's Not Found, Quit
            if (textureImage[0] != null)
            {
                status = true;                                                  // Set The Status To True

                GL.GenTextures(1, texture);                            // Create The Texture

                textureImage[0].RotateFlip(RotateFlipType.RotateNoneFlipY);     // Flip The Bitmap Along The Y-Axis
                // Rectangle For Locking The Bitmap In Memory
                Rectangle rectangle = new Rectangle(0, 0, textureImage[0].Width, textureImage[0].Height);
                // Get The Bitmap's Pixel Data From The Locked Bitmap
                BitmapData bitmapData = textureImage[0].LockBits(rectangle, ImageLockMode.ReadOnly, PixelFormat.Format24bppRgb);

                // Typical Texture Generation Using Data From The Bitmap
                GL.BindTexture(Gl.GL_TEXTURE_2D, texture[0]);
                GL.TexImage2D(Gl.GL_TEXTURE_2D, 0, Gl.GL_RGB8, textureImage[0].Width, textureImage[0].Height, 0, Gl.GL_BGR, Gl.GL_UNSIGNED_BYTE, bitmapData.Scan0);
                GL.TexParameteri(Gl.GL_TEXTURE_2D, Gl.GL_TEXTURE_MIN_FILTER, Gl.GL_NEAREST);
                GL.TexParameteri(Gl.GL_TEXTURE_2D, Gl.GL_TEXTURE_MAG_FILTER, Gl.GL_NEAREST);
                GL.TexParameteri(Gl.GL_TEXTURE_2D, Gl.GL_TEXTURE_WRAP_T, Gl.GL_REPEAT);
                GL.TexParameteri(Gl.GL_TEXTURE_2D, Gl.GL_TEXTURE_WRAP_S, Gl.GL_REPEAT);


                if (textureImage[0] != null)
                {                                   // If Texture Exists
                    textureImage[0].UnlockBits(bitmapData);                     // Unlock The Pixel Data From Memory
                    textureImage[0].Dispose();                                  // Dispose The Bitmap
                }
            }              */ 
        }
        private static Bitmap LoadBMP(string fileName)
        {
            if (fileName == null || fileName == string.Empty)
            {                  // Make Sure A Filename Was Given
                return null;                                                    // If Not Return Null
            }
            fileName = "C:\\PGi\\conc.bmp";
            string fileName1 = string.Format("Data{0}{1}",                      // Look For Data\Filename
                Path.DirectorySeparatorChar, fileName);
            string fileName2 = string.Format("{0}{1}{0}{1}Data{1}{2}",          // Look For ..\..\Data\Filename
                "..", Path.DirectorySeparatorChar, fileName);

            // Make Sure The File Exists In One Of The Usual Directories
            if (!File.Exists(fileName) && !File.Exists(fileName1) && !File.Exists(fileName2))
            {
                return null;                                                    // If Not Return Null
            }

            if (File.Exists(fileName))
            {                                         // Does The File Exist Here?
                return new Bitmap(fileName);                                    // Load The Bitmap
            }
            else
            if (File.Exists(fileName1))
            {                                   // Does The File Exist Here?
                return new Bitmap(fileName1);                                   // Load The Bitmap
            }
            else
                if (File.Exists(fileName2))
                {                                   // Does The File Exist Here?
                    return new Bitmap(fileName2);                                   // Load The Bitmap
                }

            return null;                                                        // If Load Failed Return Null
        }
        public void Render3DTextura(double centroX, double centroY, double h, byte transparencia, byte[] rgb, bool Arestas)
        {
            //face lateral de cima
            /*     GL.Color3(0, 0, 0);
                 GL.Begin(Gl.GL_LINE_LOOP);
                 GL.Vertex3(linhas_facecima.pIni.y - centroY, 0         , linhas_facecima.pIni.x - centroX);
                 GL.Vertex3(linhas_facecima.pIni.y - centroY, -Dados.h1 , linhas_facecima.pIni.x - centroX);
                 GL.Vertex3(linhas_facecima.pFin.y - centroY, -Dados.h1 , linhas_facecima.pFin.x - centroX);
                 GL.Vertex3(linhas_facecima.pFin.y - centroY, 0         , linhas_facecima.pFin.x - centroX);
                 GL.End();

                 //face lateral de baixo
                 GL.Begin(Gl.GL_LINE_LOOP);
                 GL.Vertex3(linhas_facebaixo.pIni.y - centroY, 0, linhas_facebaixo.pIni.x - centroX);
                 GL.Vertex3(linhas_facebaixo.pIni.y - centroY, -Dados.h1, linhas_facebaixo.pIni.x - centroX);
                 GL.Vertex3(linhas_facebaixo.pFin.y - centroY, -Dados.h1, linhas_facebaixo.pFin.x - centroX);
                 GL.Vertex3(linhas_facebaixo.pFin.y - centroY, 0, linhas_facebaixo.pFin.x - centroX);
                 GL.End();*/
            if (Arestas || Selecionado)
            {
                // GL.Color3(rgb[0], rgb[1], rgb[2]);
                //  if (Arestas)
                if (this.Selecionado)
                {
                    GL.LineWidth(2);
                    GL.Color3(255, 0, 0);
                }
                else
                    GL.Color3(rgb[0], rgb[1], rgb[2]);

                /*face cima*/
                GL.Begin(PrimitiveType.Lines);
                GL.Vertex3(linhas_facecima.pIni.y - centroY, h, linhas_facecima.pIni.x - centroX);
                GL.Vertex3(linhas_facecima.pFin.y - centroY, h, linhas_facecima.pFin.x - centroX);
                GL.End();

                GL.Begin(PrimitiveType.Lines);
                GL.Vertex3(linhas_facecima.pIni.y - centroY, h - Dados.h1, linhas_facecima.pIni.x - centroX);
                GL.Vertex3(linhas_facecima.pFin.y - centroY, h - Dados.h1, linhas_facecima.pFin.x - centroX);
                GL.End();

                /*face baixo*/
                GL.Begin(PrimitiveType.Lines);
                GL.Vertex3(linhas_facebaixo.pIni.y - centroY, h, linhas_facebaixo.pIni.x - centroX);
                GL.Vertex3(linhas_facebaixo.pFin.y - centroY, h, linhas_facebaixo.pFin.x - centroX);
                GL.End();

                GL.Begin(PrimitiveType.Lines);
                GL.Vertex3(linhas_facebaixo.pIni.y - centroY, h - Dados.h1, linhas_facebaixo.pIni.x - centroX);
                GL.Vertex3(linhas_facebaixo.pFin.y - centroY, h - Dados.h1, linhas_facebaixo.pFin.x - centroX);
                GL.End();

                /*face frente*/
                GL.Begin(PrimitiveType.Lines);
                GL.Vertex3(linhas_facecima.pIni.y - centroY, h, linhas_facecima.pIni.x - centroX);
                GL.Vertex3(linhas_facecima.pIni.y - centroY, h - Dados.h1, linhas_facecima.pIni.x - centroX);
                GL.End();

                GL.Begin(PrimitiveType.Lines);
                GL.Vertex3(linhas_facebaixo.pIni.y - centroY, h, linhas_facebaixo.pIni.x - centroX);
                GL.Vertex3(linhas_facebaixo.pIni.y - centroY, h - Dados.h1, linhas_facebaixo.pIni.x - centroX);
                GL.End();

                /*face tras*/
                GL.Begin(PrimitiveType.Lines);
                GL.Vertex3(linhas_facecima.pFin.y - centroY, h, linhas_facecima.pFin.x - centroX);
                GL.Vertex3(linhas_facecima.pFin.y - centroY, h - Dados.h1, linhas_facecima.pFin.x - centroX);
                GL.End();

                GL.Begin(PrimitiveType.Lines);
                GL.Vertex3(linhas_facebaixo.pFin.y - centroY, h, linhas_facebaixo.pFin.x - centroX);
                GL.Vertex3(linhas_facebaixo.pFin.y - centroY, h - Dados.h1, linhas_facebaixo.pFin.x - centroX);
                GL.End();

                GL.LineWidth(1);
            }
            GL.Color4(rgb[0], rgb[1], rgb[2], transparencia);

            p1 = new vec3(linhas_facecima.pIni.y - centroY, h, linhas_facecima.pIni.x - centroX);
            p2 = new vec3(linhas_facebaixo.pIni.y - centroY, h, linhas_facebaixo.pIni.x - centroX);
            p3 = new vec3(linhas_facebaixo.pFin.y - centroY, h, linhas_facebaixo.pFin.x - centroX);

            v1 = p2 - p1;
            v2 = p3 - p1;
            n1 = v1 * v2;
            n1.Normalize();

             GL.BindTexture(TextureTarget.Texture2D, texture);

            GL.Begin(PrimitiveType.Polygon);
            //  GL.Normal3(n1.x, n1.y, n1.z);
            GL.Normal3(0, 1, 0);

            GL.TexCoord2(0, 0);
            GL.Vertex3(linhas_facecima.pIni.y - centroY, h, linhas_facecima.pIni.x - centroX);

            GL.TexCoord2(0, 1);
            GL.Vertex3(linhas_facebaixo.pIni.y - centroY, h, linhas_facebaixo.pIni.x - centroX);

            GL.TexCoord2(1, 1);
            GL.Vertex3(linhas_facebaixo.pFin.y - centroY, h, linhas_facebaixo.pFin.x - centroX);

            GL.TexCoord2(1, 0);
            GL.Vertex3(linhas_facecima.pFin.y - centroY, h, linhas_facecima.pFin.x - centroX);
            GL.End();

            /*  GL.Begin(PrimitiveType.Lines);
              GL.LineWidth(5);
              GL.Color3(255, 0, 0);
              GL.Vertex3(linhas_facecima.pIni.y - centroY, h, linhas_facecima.pIni.x - centroX);
              GL.Vertex3(n1.x, n1.y, n1.z);
              GL.End();
              GL.Color4(rgb[0], rgb[1], rgb[2], transparencia);
              GL.Color3(255, 0, 0);
              GL.Begin(PrimitiveType.Polygon);
              GL.Vertex3(p1.x - 2, p1.y - 2, p1.z);
              GL.Vertex3(p1.x + 2, p1.y - 2, p1.z);
              GL.Vertex3(p1.x + 2, p1.y + 2, p1.z);
              GL.Vertex3(p1.x - 2, p1.y + 2, p1.z);
              GL.End();


              GL.Color3(0, 255, 0);
              GL.Begin(PrimitiveType.Polygon);
              GL.Vertex3(p2.x - 2, p2.y - 2, p2.z);
              GL.Vertex3(p2.x + 2, p2.y - 2, p2.z);
              GL.Vertex3(p2.x + 2, p2.y + 2, p2.z);
              GL.Vertex3(p2.x - 2, p2.y + 2, p2.z);
              GL.End();

            
              GL.Color3(0, 0, 255);
              GL.Begin(PrimitiveType.Polygon);
              GL.Vertex3(p3.x - 2, p3.y - 2, p3.z);
              GL.Vertex3(p3.x + 2, p3.y - 2, p3.z);
              GL.Vertex3(p3.x + 2, p3.y + 2, p3.z);
              GL.Vertex3(p3.x - 2, p3.y + 2, p3.z);
              GL.End();*/


            p1 = new vec3(linhas_facecima.pIni.y, h - Dados.h1, linhas_facecima.pIni.x);
            p2 = new vec3(linhas_facebaixo.pIni.y, h - Dados.h1, linhas_facebaixo.pIni.x);
            p3 = new vec3(linhas_facebaixo.pFin.y, h - Dados.h1, linhas_facebaixo.pFin.x);

            v1 = p2 - p1;
            v2 = p3 - p1;
            n1 = v1 * v2;
            n1.Normalize();

            GL.Begin(PrimitiveType.Polygon);
            GL.Normal3(n1.x, n1.y, n1.z);

            GL.TexCoord2(0, 0);
            GL.Vertex3(linhas_facecima.pIni.y - centroY, h - Dados.h1, linhas_facecima.pIni.x - centroX);

            GL.TexCoord2(1, 0);
            GL.Vertex3(linhas_facebaixo.pIni.y - centroY, h - Dados.h1, linhas_facebaixo.pIni.x - centroX);

            GL.TexCoord2(1, 1);
            GL.Vertex3(linhas_facebaixo.pFin.y - centroY, h - Dados.h1, linhas_facebaixo.pFin.x - centroX);

            GL.TexCoord2(0, 1);
            GL.Vertex3(linhas_facecima.pFin.y - centroY, h - Dados.h1, linhas_facecima.pFin.x - centroX);
            GL.End();


            p1 = new vec3(linhas_facebaixo.pIni.y - centroY, h, linhas_facebaixo.pIni.x - centroX);
            p2 = new vec3(linhas_facebaixo.pIni.y - centroY, h - Dados.h1, linhas_facebaixo.pIni.x - centroX);
            p3 = new vec3(linhas_facebaixo.pFin.y - centroY, h - Dados.h1, linhas_facebaixo.pFin.x - centroX);

            v1 = p2 - p1;
            v2 = p3 - p1;
            n1 = v1 * v2;
            n1.Normalize();

            GL.Begin(PrimitiveType.Polygon);
            GL.Normal3(n1.x, n1.y, n1.z);
            GL.TexCoord2(0, 0); GL.Vertex3(linhas_facebaixo.pIni.y - centroY, h, linhas_facebaixo.pIni.x - centroX);
            GL.TexCoord2(1, 0); GL.Vertex3(linhas_facebaixo.pIni.y - centroY, h - Dados.h1, linhas_facebaixo.pIni.x - centroX);
            GL.TexCoord2(1, 1); GL.Vertex3(linhas_facebaixo.pFin.y - centroY, h - Dados.h1, linhas_facebaixo.pFin.x - centroX);
            GL.TexCoord2(0, 1); GL.Vertex3(linhas_facebaixo.pFin.y - centroY, h, linhas_facebaixo.pFin.x - centroX);
            GL.End();


            p1 = new vec3(linhas_facecima.pIni.y, h, linhas_facecima.pIni.x);
            p2 = new vec3(linhas_facecima.pIni.y, h - Dados.h1, linhas_facecima.pIni.x);
            p3 = new vec3(linhas_facecima.pFin.y, h, linhas_facecima.pFin.x);

            v1 = p2 - p1;
            v2 = p3 - p1;
            n1 = v1 * v2;
            n1.Normalize();


            GL.Begin(PrimitiveType.Polygon);
            GL.Normal3(n1.x, n1.y, n1.z);
            GL.TexCoord2(0, 0); GL.Vertex3(linhas_facecima.pIni.y - centroY, h, linhas_facecima.pIni.x - centroX);
            GL.TexCoord2(1, 0); GL.Vertex3(linhas_facecima.pIni.y - centroY, h - Dados.h1, linhas_facecima.pIni.x - centroX);
            GL.TexCoord2(1, 1); GL.Vertex3(linhas_facecima.pFin.y - centroY, h - Dados.h1, linhas_facecima.pFin.x - centroX);
            GL.TexCoord2(0, 1); GL.Vertex3(linhas_facecima.pFin.y - centroY, h, linhas_facecima.pFin.x - centroX);
            GL.End();

            /*Poligonos das faces de tras e da frente*/


            /*float[] materialDiffuse = { 0.7f, 0.7f, 0.7f, 1.0f };
            float[] materialSpecular = { 1.0f, 1.0f, 1.0f, 1.0f };
            float[] materialShininess = { 100.0f };

            float[] lowAmbient = { 0.1f, 0.1f, 0.1f, 1.0f };
            float[] moreAmbient = { 0.4f, 0.4f, 0.4f, 1.0f };
            float[] mostAmbient = { 1.0f, 1.0f, 1.0f, 1.0f };
            
            GL.Materialfv(Gl.GL_FRONT_AND_BACK, Gl.GL_DIFFUSE, materialDiffuse);
            GL.Materialfv(Gl.GL_FRONT_AND_BACK, Gl.GL_SPECULAR, materialSpecular);
            GL.Materialfv(Gl.GL_FRONT_AND_BACK, Gl.GL_SHININESS, materialShininess);
            GL.Materialfv(Gl.GL_FRONT_AND_BACK, Gl.GL_AMBIENT, moreAmbient);*/

            // material has small ambient reflection

            /*face inicial*/



            // GL.Color4(rgb[0], rgb[1], rgb[2], transparencia);
            /*   GL.Begin(PrimitiveType.Polygon);
               GL.Normal3f(0, 0, 1);
               GL.Vertex3(linhas_facebaixo_org.pIni.y - centroY, h, linhas_facebaixo_org.pIni.x - centroX);
               GL.Vertex3(linhas_facebaixo_org.pIni.y - centroY, h - Dados.h1, linhas_facebaixo_org.pIni.x - centroX);
               GL.Vertex3(linhas_facebaixo.pIni.y - centroY, h - Dados.h1, linhas_facebaixo.pIni.x - centroX);
               GL.Vertex3(linhas_facebaixo.pIni.y - centroY, h, linhas_facebaixo.pIni.x - centroX);
               GL.End();*/


            /*  p1 = new vec3(linhas_facecima.pFin.x - centroX, linhas_facecima.pFin.y - centroY, h - Dados.h1);
              p2 = new vec3(linhas_facecima.pIni.x - centroX, linhas_facecima.pIni.y - centroY, h);
              p3 = new vec3(linhas_facecima.pFin.x - centroX, linhas_facecima.pFin.y - centroY, h);
              v1 = p2 - p1;
              v2 = p3 - p1;
              n1 = v1 * v2;
              n1.Normalize();

              /*  GL.Begin(PrimitiveType.Polygon);
        
                GL.Normal3(n1.x, n1.y, n1.z);  
                GL.Vertex3(linhas_facecima_org.pIni.y - centroY, h, linhas_facecima_org.pIni.x - centroX);
                GL.Vertex3(linhas_facecima_org.pIni.y - centroY, h - Dados.h1, linhas_facecima_org.pIni.x - centroX);
                GL.Vertex3(linhas_facecima.pIni.y - centroY, h - Dados.h1, linhas_facecima.pIni.x - centroX);
                GL.Vertex3(linhas_facecima.pIni.y - centroY, h, linhas_facecima.pIni.x - centroX);
                GL.End();*/
            /********************/
            /*face inicial*/
            /*  GL.Begin(PrimitiveType.Polygon);
              GL.Normal3(n1.x, n1.y, n1.z);

              if (linhas_facecima_org.comprimento > linhas_facecima.comprimento)
                  lin = linhas_facecima_org;
              else
                  lin = linhas_facecima;
              GL.Vertex3(lin.pIni.y - centroY, h, lin.pIni.x - centroX);
              GL.Vertex3(lin.pIni.y - centroY, h - Dados.h1, lin.pIni.x - centroX);

              if (linhas_facebaixo_org.comprimento > linhas_facebaixo.comprimento)
                  lin = linhas_facebaixo_org;
              else
                  lin = linhas_facebaixo;
              GL.Vertex3(lin.pIni.y - centroY, h - Dados.h1, lin.pIni.x - centroX);
              GL.Vertex3(lin.pIni.y - centroY, h, lin.pIni.x - centroX);
              GL.End();

              //////////////////////////////////////////
              /*face final*/
            // GL.Color3(0, 0, 255); 
            //  GL.Color3(0, 0, 255);
            /* GL.Begin(PrimitiveType.Polygon);
             GL.Normal3(n1.x, n1.y, n1.z);

             if (linhas_facecima_org.comprimento > linhas_facecima.comprimento)
                 lin = linhas_facecima_org;
             else
                 lin = linhas_facecima;
             GL.Vertex3(lin.pFin.y - centroY, h, lin.pFin.x - centroX);
             GL.Vertex3(lin.pFin.y - centroY, h - Dados.h1, lin.pFin.x - centroX);

             if (linhas_facebaixo_org.comprimento > linhas_facebaixo.comprimento)
                 lin = linhas_facebaixo_org;
             else
                 lin = linhas_facebaixo;
             GL.Vertex3(lin.pFin.y - centroY, h - Dados.h1, lin.pFin.x - centroX);
             GL.Vertex3(lin.pFin.y - centroY, h, lin.pFin.x - centroX);
             GL.End();

             GL.Color4(rgb[0], rgb[1], rgb[2], transparencia);
             /*face final*/
            /* p1 = new vec3(linhas_facebaixo_org.pFin.x - centroX, linhas_facebaixo_org.pFin.y - centroY, h - Dados.h1);
             p2 = new vec3(linhas_facebaixo_org.pIni.x - centroX, linhas_facebaixo_org.pIni.y - centroY, h);
             p3 = new vec3(linhas_facebaixo_org.pFin.x - centroX, linhas_facebaixo_org.pFin.y - centroY, h);
             v1 = p2 - p1;
             v2 = p3 - p1;
             n1 = v1 * v2;
             n1.Normalize();
             GL.Begin(PrimitiveType.Polygon);
             GL.Normal3(n1.x, n1.y, n1.z);
             //GL.Normal3f(0, 0, 0);
             GL.Vertex3(linhas_facebaixo_org.pFin.y - centroY, h, linhas_facebaixo_org.pFin.x - centroX);
             GL.Vertex3(linhas_facebaixo_org.pFin.y - centroY, h - Dados.h1, linhas_facebaixo_org.pFin.x - centroX);
             GL.Vertex3(linhas_facebaixo.pFin.y - centroY, h - Dados.h1, linhas_facebaixo.pFin.x - centroX);
             GL.Vertex3(linhas_facebaixo.pFin.y - centroY, h, linhas_facebaixo.pFin.x - centroX);
             GL.End();
             /********************/


            /* GL.Begin(PrimitiveType.Polygon);
             GL.Normal3(n1.x, n1.y, n1.z);
             GL.Vertex3(linhas_facecima.pFin.y - centroY, h, linhas_facecima.pFin.x - centroX);
             GL.Vertex3(linhas_facecima.pFin.y - centroY, h - Dados.h1, linhas_facecima.pFin.x - centroX);
             GL.Vertex3(linhas_facebaixo_org.pFin.y - centroY, h - Dados.h1, linhas_facebaixo_org.pFin.x - centroX);
             GL.Vertex3(linhas_facebaixo_org.pFin.y - centroY, h, linhas_facebaixo_org.pFin.x - centroX);
             GL.End();

             p1 = new vec3(linhas_facebaixo_org.pFin.x - centroX, linhas_facebaixo_org.pFin.y - centroY, h - Dados.h1);
             p2 = new vec3(linhas_facebaixo_org.pIni.x - centroX, linhas_facebaixo_org.pIni.y - centroY, h);
             p3 = new vec3(linhas_facebaixo_org.pFin.x - centroX, linhas_facebaixo_org.pFin.y - centroY, h);
             v1 = p2 - p1;
             v2 = p3 - p1;
             n1 = v1 * v2;
             n1.Normalize();

             GL.Begin(PrimitiveType.Polygon);
             GL.Normal3(n1.x, n1.y, n1.z);
             GL.Vertex3(linhas_facecima_org.pFin.y - centroY, h, linhas_facecima_org.pFin.x - centroX);
             GL.Vertex3(linhas_facecima_org.pFin.y - centroY, h - Dados.h1, linhas_facecima_org.pFin.x - centroX);
             GL.Vertex3(linhas_facecima.pFin.y - centroY, h - Dados.h1, linhas_facecima.pFin.x - centroX);
             GL.Vertex3(linhas_facecima.pFin.y - centroY, h, linhas_facecima.pFin.x - centroX);
             GL.End();

             /*face lateral de baixo*/
            /*  GL.Begin(PrimitiveType.Polygon);
              GL.Normal3(n1.x, n1.y, n1.z);
              GL.TexCoord3(linhas_facebaixo.pIni.y - centroY, h, linhas_facebaixo.pIni.x - centroX);
              GL.Vertex3(linhas_facebaixo.pIni.y - centroY, h, linhas_facebaixo.pIni.x - centroX);
              GL.TexCoord3(linhas_facebaixo.pIni.y - centroY, h - Dados.h1, linhas_facebaixo.pIni.x - centroX);
              GL.Vertex3(linhas_facebaixo.pIni.y - centroY, h - Dados.h1, linhas_facebaixo.pIni.x - centroX);
              GL.TexCoord3(linhas_facebaixo.pFin.y - centroY, h - Dados.h1, linhas_facebaixo.pFin.x - centroX);
              GL.Vertex3(linhas_facebaixo.pFin.y - centroY, h - Dados.h1, linhas_facebaixo.pFin.x - centroX);
              GL.TexCoord3(linhas_facebaixo.pFin.y - centroY, h, linhas_facebaixo.pFin.x - centroX);
              GL.Vertex3(linhas_facebaixo.pFin.y - centroY, h, linhas_facebaixo.pFin.x - centroX);
              GL.End();


              /*face lateral de cima*/
            /*  p1 = new vec3(linhas_facecima.pFin.x - centroX, linhas_facecima.pFin.y - centroY, h - Dados.h1);
              p2 = new vec3(linhas_facecima.pIni.x - centroX, linhas_facecima.pIni.y - centroY, h);
              p3 = new vec3(linhas_facecima.pFin.x - centroX, linhas_facecima.pFin.y - centroY, h);
              v1 = p2 - p1;
              v2 = p3 - p1;
              n1 = v1 * v2;
              n1.Normalize();

              GL.Begin(PrimitiveType.Polygon);
              GL.Normal3(n1.x, n1.y, n1.z);    //     GL.Normal3f(0, 0, -1);
              GL.Vertex3(linhas_facecima.pIni.y - centroY, h, linhas_facecima.pIni.x - centroX);
              GL.Vertex3(linhas_facecima.pIni.y - centroY, h - Dados.h1, linhas_facecima.pIni.x - centroX);
              GL.Vertex3(linhas_facecima.pFin.y - centroY, h - Dados.h1, linhas_facecima.pFin.x - centroX);
              GL.Vertex3(linhas_facecima.pFin.y - centroY, h, linhas_facecima.pFin.x - centroX);
              GL.End();

              /* GL.Begin(PrimitiveType.Polygon);
               GL.Normal3f(-1, 0, 0); //  GL.Normal3(n1.x, n1.y, n1.z);
               GL.Vertex3(linhas_facecima_org.pIni.y - centroY, h, linhas_facecima_org.pIni.x - centroX);
               GL.Vertex3(linhas_facecima_org.pIni.y - centroY, h - Dados.h1, linhas_facecima_org.pIni.x - centroX);
               GL.Vertex3(linhas_facebaixo_org.pIni.y - centroY, h - Dados.h1, linhas_facebaixo_org.pIni.x - centroX);
               GL.Vertex3(linhas_facebaixo_org.pIni.y - centroY, h, linhas_facebaixo_org.pIni.x - centroX);
               GL.End();*/

            /* GL.Begin(PrimitiveType.Polygon);
             GL.Normal3f(0, -1, 0);    //     GL.Normal3(n1.x, n1.y, n1.z);
             GL.Vertex3(linhas_facecima_org.pFin.y - centroY, h, linhas_facecima_org.pFin.x - centroX);
             GL.Vertex3(linhas_facecima_org.pFin.y - centroY, h - Dados.h1, linhas_facecima_org.pFin.x - centroX);
             GL.Vertex3(linhas_facebaixo_org.pFin.y - centroY, h - Dados.h1, linhas_facebaixo_org.pFin.x - centroX);
             GL.Vertex3(linhas_facebaixo_org.pFin.y - centroY, h, linhas_facebaixo_org.pFin.x - centroX);
             GL.End();*/

            /*       GL.Begin(PrimitiveType.Polygon);
                   GL.Vertex3(linhas_facecima_org.pIni.y - centroY, h, linhas_facecima_org.pIni.x - centroX);
                   GL.Vertex3(linhas_facecima_org.pIni.y - centroY,h -Dados.h1, linhas_facecima_org.pIni.x - centroX);
                   GL.Vertex3(linhas_facecima_org.pFin.y - centroY, h-Dados.h1, linhas_facecima_org.pFin.x - centroX);
                   GL.Vertex3(linhas_facecima_org.pFin.y - centroY, h, linhas_facecima_org.pFin.x - centroX);
                   GL.End();     */


            // GL.Color3(169, 169, 169);

            //face de cima e de baixo da viga
            //  GL.Color4(rgb[0], rgb[1], rgb[2], transparencia);

            /*face de baixo*/
            /*  GL.Begin(PrimitiveType.Polygon);

              /*v1 = new vec3(linhas_facebaixo.pFin.y - centroY, h - Dados.h1, linhas_facebaixo.pFin.x - centroX);
              v2 = new vec3(linhas_facecima.pIni.y - centroY, h - Dados.h1, linhas_facecima.pIni.x - centroX);
              n1 = v2 * v1;*/

            /*   if (Dados.numero == 17)
                   Dados.numero = 17;
               p1 = new vec3(linhas_facecima.pFin.x - centroX, linhas_facecima.pFin.y - centroY, h - Dados.h1);
               p2 = new vec3(linhas_facebaixo.pIni.x + (linhas_facebaixo.pIni.x / 2) - centroX, linhas_facebaixo.pIni.y + (linhas_facebaixo.pIni.y / 2) - centroY, h - Dados.h1);
               p3 = new vec3(linhas_facebaixo.pFin.x - centroX, linhas_facebaixo.pFin.y - centroY, h - Dados.h1);

               v1 = p2 - p1;
               v2 = p3 - p1;

               n1 = v1 * v2;

               n1.Normalize();
               ///  GL.Normal3(n1.y, n1.z, n1.x);
               GL.Normal3f(0, 1, 0);
               GL.Vertex3(linhas_facecima_org.pIni.y - centroY, h - Dados.h1, linhas_facecima_org.pIni.x - centroX);
               //     GL.Vertex3(linhas_eixo_org.pIni.y - centroY, h - Dados.h1, linhas_eixo_org.pIni.x - centroX);
               GL.Vertex3(linhas_facebaixo_org.pIni.y - centroY, h - Dados.h1, linhas_facebaixo_org.pIni.x - centroX);
               GL.Vertex3(linhas_facebaixo_org.pFin.y - centroY, h - Dados.h1, linhas_facebaixo_org.pFin.x - centroX);
               //  GL.Vertex3(linhas_eixo_org.pFin.y - centroY, h - Dados.h1, linhas_eixo_org.pFin.x - centroX);
               GL.Vertex3(linhas_facecima_org.pFin.y - centroY, h - Dados.h1, linhas_facecima_org.pFin.x - centroX);
               GL.End();

               if (linhas_facebaixo.comprimento > linhas_facecima.comprimento)
                   lin = linhas_facebaixo;
               else
                   lin = linhas_facecima;

               /*   v1 = new vec3(linhas_facebaixo.pFin.y - centroY, h, linhas_facebaixo.pFin.x - centroX);
                  v2 = new vec3(linhas_facecima.pIni.y - centroY, h, linhas_facecima.pIni.x - centroX);
                  n1 = v1 * v2;
                  n1.Normalize();*/

            /*face de cima*/

            /*   p1 = new vec3(linhas_facecima.pIni.x - centroX, linhas_facecima.pIni.y - centroY, h);
               p2 = new vec3(linhas_facebaixo.pFin.x - centroX, linhas_facebaixo.pFin.y - centroY, h);
               p3 = new vec3(linhas_facebaixo.pIni.x - centroX, linhas_facebaixo.pIni.y - centroY, h);

               v1 = p2 - p1;
               v2 = p3 - p1;

               n1 = v1 * v2;
               n1.Normalize();

               GL.Begin(PrimitiveType.Polygon);
               //    GL.Normal3(n1.x, n1.y, n1.z);

               GL.Normal3f(0, 1, 0);
               GL.Vertex3(linhas_facecima.pIni.y - centroY, h, linhas_facecima.pIni.x - centroX);
               GL.Vertex3(linhas_eixo.pIni.y - centroY, h, linhas_eixo.pIni.x - centroX);
               GL.Vertex3(linhas_facebaixo.pIni.y - centroY, h, linhas_facebaixo.pIni.x - centroX);
               GL.Vertex3(linhas_facebaixo.pFin.y - centroY, h, linhas_facebaixo.pFin.x - centroX);
               GL.Vertex3(linhas_eixo.pFin.y - centroY, h, linhas_eixo.pFin.x - centroX);
               GL.Vertex3(linhas_facecima.pFin.y - centroY, h, linhas_facecima.pFin.x - centroX);
               GL.End();*/
        }
        public void TriangularizaFaceCima()
        {
            CPoint2D[] vertices;
            vertices = new CPoint2D[4];
 
            vertices[0] = new CPoint2D(linhas_facecima.pIni.y, linhas_facecima.pIni.x);
            vertices[1] = new CPoint2D(linhas_facebaixo.pIni.y, linhas_facebaixo.pIni.x);
            vertices[2] = new CPoint2D(linhas_facebaixo.pFin.y, linhas_facebaixo.pFin.x);
            vertices[3] = new CPoint2D(linhas_facecima.pFin.y, linhas_facecima.pFin.x);

            PoligonoFaceCima = new CPolygonShape(vertices);
            PoligonoFaceCima.CutEar();
        }
        public void TriangularizaLado1()
        {
            CPoint2D[] vertices;
            vertices = new CPoint2D[4];

            vertices[0] = new CPoint2D(linhas_facebaixo.pIni.y, linhas_facebaixo.pIni.x);
            vertices[1] = new CPoint2D(linhas_facebaixo.pIni.y, linhas_facebaixo.pIni.x);
            vertices[2] = new CPoint2D(linhas_facebaixo.pFin.y, linhas_facebaixo.pFin.x);
            vertices[3] = new CPoint2D(linhas_facebaixo.pFin.y, linhas_facebaixo.pFin.x);

            PoligonoFaceCima = new CPolygonShape(vertices);
            PoligonoFaceCima.CutEar();
        }

        public void TriangularizaLado2()
        {
            CPoint2D[] vertices;
            vertices = new CPoint2D[4];

            vertices[0] = new CPoint2D(linhas_facecima.pIni.y, linhas_facecima.pIni.x);
            vertices[1] = new CPoint2D(linhas_facebaixo.pIni.y, linhas_facebaixo.pIni.x);
            vertices[2] = new CPoint2D(linhas_facebaixo.pFin.y, linhas_facebaixo.pFin.x);
            vertices[3] = new CPoint2D(linhas_facecima.pFin.y, linhas_facecima.pFin.x);

            PoligonoFaceCima = new CPolygonShape(vertices);
            PoligonoFaceCima.CutEar();
        }
        public void Render3D(double centroX, double centroY, double h, byte transparencia, byte[] rgb, bool Arestas)
        {
            //face lateral de cima
            /*     GL.Color3(0, 0, 0);
                 GL.Begin(Gl.GL_LINE_LOOP);
                 GL.Vertex3(linhas_facecima.pIni.y - centroY, 0         , linhas_facecima.pIni.x - centroX);
                 GL.Vertex3(linhas_facecima.pIni.y - centroY, -Dados.h1 , linhas_facecima.pIni.x - centroX);
                 GL.Vertex3(linhas_facecima.pFin.y - centroY, -Dados.h1 , linhas_facecima.pFin.x - centroX);
                 GL.Vertex3(linhas_facecima.pFin.y - centroY, 0         , linhas_facecima.pFin.x - centroX);
                 GL.End();

                 //face lateral de baixo
                 GL.Begin(Gl.GL_LINE_LOOP);
                 GL.Vertex3(linhas_facebaixo.pIni.y - centroY, 0, linhas_facebaixo.pIni.x - centroX);
                 GL.Vertex3(linhas_facebaixo.pIni.y - centroY, -Dados.h1, linhas_facebaixo.pIni.x - centroX);
                 GL.Vertex3(linhas_facebaixo.pFin.y - centroY, -Dados.h1, linhas_facebaixo.pFin.x - centroX);
                 GL.Vertex3(linhas_facebaixo.pFin.y - centroY, 0, linhas_facebaixo.pFin.x - centroX);
                 GL.End();*/

          //  draw1();
          //  return;

            if (Arestas || Selecionado)
            {
                // GL.Color3(rgb[0], rgb[1], rgb[2]);
                //  if (Arestas)
                if (this.Selecionado)
                {
                    GL.LineWidth(2);
                    GL.Color3(Color.Red);
                }
                else
                  GL.Color3(Color.Black);
                   //  GL.Color3ui(rgb[0]+10, rgb[1], rgb[2]+2);

                /*face cima*/

             //   GL.LineWidth(2);
                GL.Begin(PrimitiveType.Lines);
                GL.Vertex3(linhas_facecima.pIni.x - centroX,linhas_facecima.pIni.y - centroY, h);
                GL.Vertex3(linhas_facecima.pFin.x - centroX,linhas_facecima.pFin.y - centroY, h);
                GL.End();

                GL.Begin(PrimitiveType.Lines);
                GL.Vertex3(linhas_facecima.pIni.x - centroX,linhas_facecima.pIni.y - centroY, h - Dados.h1);
                GL.Vertex3(linhas_facecima.pFin.x - centroX,linhas_facecima.pFin.y - centroY, h - Dados.h1);
                GL.End();

                /*face baixo*/
                GL.Begin(PrimitiveType.Lines);
                GL.Vertex3(linhas_facebaixo.pIni.x - centroX,linhas_facebaixo.pIni.y - centroY, h);
                GL.Vertex3(linhas_facebaixo.pFin.x - centroX,linhas_facebaixo.pFin.y - centroY, h);
                GL.End();

                GL.Begin(PrimitiveType.Lines);
                GL.Vertex3(linhas_facebaixo.pIni.x - centroX,linhas_facebaixo.pIni.y - centroY, h - Dados.h1);
                GL.Vertex3(linhas_facebaixo.pFin.x - centroX,linhas_facebaixo.pFin.y - centroY, h - Dados.h1);
                GL.End();

                /*face frente*/
                GL.Begin(PrimitiveType.Lines);
                GL.Vertex3(linhas_facecima.pIni.x - centroX,linhas_facecima.pIni.y - centroY, h);
                GL.Vertex3(linhas_facecima.pIni.x - centroX,linhas_facecima.pIni.y - centroY, h - Dados.h1);
                GL.End();

                GL.Begin(PrimitiveType.Lines);
                GL.Vertex3(linhas_facebaixo.pIni.x - centroX,linhas_facebaixo.pIni.y - centroY, h);
                GL.Vertex3(linhas_facebaixo.pIni.x - centroX,linhas_facebaixo.pIni.y - centroY, h - Dados.h1);
                GL.End();

                /*face tras*/
                GL.Begin(PrimitiveType.Lines);
                GL.Vertex3(linhas_facecima.pFin.x - centroX,linhas_facecima.pFin.y - centroY, h);
                GL.Vertex3(linhas_facecima.pFin.x - centroX,linhas_facecima.pFin.y - centroY, h - Dados.h1);
                GL.End();

                GL.Begin(PrimitiveType.Lines);
                GL.Vertex3(linhas_facebaixo.pFin.x - centroX,linhas_facebaixo.pFin.y - centroY, h);
                GL.Vertex3(linhas_facebaixo.pFin.x - centroX,linhas_facebaixo.pFin.y - centroY, h - Dados.h1);
                GL.End();

                GL.LineWidth(1);
            }

            GL.Color4(rgb[0], rgb[1], rgb[2], transparencia);

            /**/
            p1 = new vec3(linhas_facecima.pIni.x - centroX, linhas_facecima.pIni.y - centroY, h);
            p2 = new vec3(linhas_facebaixo.pIni.x - centroX, linhas_facebaixo.pIni.y - centroY, h);
            p3 = new vec3(linhas_facebaixo.pFin.x - centroX, linhas_facebaixo.pFin.y - centroY, h);
            v1 = p2 - p1;
            v2 = p3 - p1;
            n1 = v1.CrossProduct(v2);
            n1.Normalize();
            
            GL.Begin(PrimitiveType.Polygon);
            GL.Normal3(-n1.x, -n1.y, -n1.z); 
            GL.Vertex3(linhas_facecima.pIni.x - centroX, linhas_facecima.pIni.y - centroY, h);
            GL.Vertex3(linhas_facebaixo.pIni.x - centroX,linhas_facebaixo.pIni.y - centroY, h);
            GL.Vertex3(linhas_facebaixo.pFin.x - centroX,linhas_facebaixo.pFin.y - centroY, h);
            GL.Vertex3(linhas_facecima.pFin.x - centroX,linhas_facecima.pFin.y - centroY, h);
            GL.End();

            /**/
            p1 = new vec3(linhas_facecima.pIni.y - centroY, h - Dados.h1, linhas_facecima.pIni.x - centroX);
            p2 = new vec3(linhas_facebaixo.pIni.y - centroY, h - Dados.h1, linhas_facebaixo.pIni.x - centroX);
            p3 = new vec3(linhas_facebaixo.pFin.y - centroY, h - Dados.h1, linhas_facebaixo.pFin.x - centroX);
            v1 = p2 - p1;
            v2 = p3 - p1;
            n1 = v1.CrossProduct(v2);
            n1.Normalize();

            GL.Begin(PrimitiveType.Polygon);
            GL.Normal3(-n1.x,- n1.y, -n1.z);
            GL.Normal3(0, 0, 1);
            GL.Vertex3(linhas_facecima.pIni.x - centroX,linhas_facecima.pIni.y - centroY, h - Dados.h1 );            
            GL.Vertex3(linhas_facebaixo.pIni.x - centroX,linhas_facebaixo.pIni.y - centroY, h - Dados.h1 ); 
            GL.Vertex3(linhas_facebaixo.pFin.x - centroX,linhas_facebaixo.pFin.y - centroY, h - Dados.h1 ); 
            GL.Vertex3(linhas_facecima.pFin.x - centroX,linhas_facecima.pFin.y - centroY, h - Dados.h1 );
            GL.End();

            /**/
            p1 = new vec3(linhas_facebaixo.pIni.x - centroX, linhas_facebaixo.pIni.y - centroY, h);
            p2 = new vec3(linhas_facebaixo.pIni.x - centroX, linhas_facebaixo.pIni.y - centroY, h - Dados.h1);
            p3 = new vec3(linhas_facebaixo.pFin.x - centroX, linhas_facebaixo.pFin.y - centroY, h - Dados.h1);
            v1 = p2 - p1;
            v2 = p3 - p1;
            n1 = v1.CrossProduct(v2);
            n1.Normalize();

            GL.Begin(PrimitiveType.Polygon);
            GL.Normal3(-n1.x, -n1.y, -n1.z);
            GL.Vertex3(linhas_facebaixo.pIni.x - centroX,linhas_facebaixo.pIni.y - centroY, h );
            GL.Vertex3(linhas_facebaixo.pIni.x - centroX,linhas_facebaixo.pIni.y - centroY, h - Dados.h1 );
            GL.Vertex3(linhas_facebaixo.pFin.x - centroX,linhas_facebaixo.pFin.y - centroY, h - Dados.h1 );
            GL.Vertex3(linhas_facebaixo.pFin.x - centroX,linhas_facebaixo.pFin.y - centroY, h );
            GL.End();
            /**/


            p1 = new vec3(linhas_facecima.pIni.x - centroX, linhas_facecima.pIni.y - centroY, h           );
            p2 = new vec3(linhas_facecima.pIni.x - centroX, linhas_facecima.pIni.y - centroY, h - Dados.h1);
            p3 = new vec3(linhas_facecima.pFin.x - centroX, linhas_facecima.pFin.y - centroY, h - Dados.h1);

            v1 = p2 - p1;
            v2 = p3 - p1;
            n1 = v1.CrossProduct(v2);
            n1.Normalize();
           
            GL.Begin(PrimitiveType.Polygon);
            GL.Normal3(-n1.x, -n1.y, -n1.z);
            GL.Vertex3(linhas_facecima.pIni.x - centroX,linhas_facecima.pIni.y - centroY, h );
            GL.Vertex3(linhas_facecima.pIni.x - centroX,linhas_facecima.pIni.y - centroY, h - Dados.h1 );
            GL.Vertex3(linhas_facecima.pFin.x - centroX,linhas_facecima.pFin.y - centroY, h - Dados.h1 );
            GL.Vertex3(linhas_facecima.pFin.x - centroX,linhas_facecima.pFin.y - centroY, h );
            GL.End();
            /*
            GL.LineWidth(2);
            GL.Color3(Color.Red);
            GL.Begin(PrimitiveType.LineLoop);
            GL.Vertex3(p1.x, p1.y, p1.z);
            GL.Vertex3(p2.x, p2.y, p2.z);
            GL.Vertex3(p3.x, p3.y, p3.z);
            GL.End();
            GL.LineWidth(1);

            GL.LineWidth(2);
            GL.Color3(Color.Red);
            GL.Begin(PrimitiveType.Lines);
            GL.Vertex3(p1.x, p1.y, p1.z);
            GL.Vertex3(p1.x + (n1.x * 10), p1.y + (n1.y * 10), p1.z + (n1.z * 10));
            GL.End();
            GL.LineWidth(1);*/
            /*Poligonos das faces de tras e da frente*/


            /*float[] materialDiffuse = { 0.7f, 0.7f, 0.7f, 1.0f };
            float[] materialSpecular = { 1.0f, 1.0f, 1.0f, 1.0f };
            float[] materialShininess = { 100.0f };

            float[] lowAmbient = { 0.1f, 0.1f, 0.1f, 1.0f };
            float[] moreAmbient = { 0.4f, 0.4f, 0.4f, 1.0f };
            float[] mostAmbient = { 1.0f, 1.0f, 1.0f, 1.0f };
            
            GL.Materialfv(Gl.GL_FRONT_AND_BACK, Gl.GL_DIFFUSE, materialDiffuse);
            GL.Materialfv(Gl.GL_FRONT_AND_BACK, Gl.GL_SPECULAR, materialSpecular);
            GL.Materialfv(Gl.GL_FRONT_AND_BACK, Gl.GL_SHININESS, materialShininess);
            GL.Materialfv(Gl.GL_FRONT_AND_BACK, Gl.GL_AMBIENT, moreAmbient);*/

            // material has small ambient reflection

            /*face inicial*/



            // GL.Color4(rgb[0], rgb[1], rgb[2], transparencia);
            /*   GL.Begin(PrimitiveType.Polygon);
               GL.Normal3f(0, 0, 1);
               GL.Vertex3(linhas_facebaixo_org.pIni.y - centroY, h, linhas_facebaixo_org.pIni.x - centroX);
               GL.Vertex3(linhas_facebaixo_org.pIni.y - centroY, h - Dados.h1, linhas_facebaixo_org.pIni.x - centroX);
               GL.Vertex3(linhas_facebaixo.pIni.y - centroY, h - Dados.h1, linhas_facebaixo.pIni.x - centroX);
               GL.Vertex3(linhas_facebaixo.pIni.y - centroY, h, linhas_facebaixo.pIni.x - centroX);
               GL.End();*/


          /*  p1 = new vec3(linhas_facecima.pFin.x - centroX, linhas_facecima.pFin.y - centroY, h - Dados.h1);
            p2 = new vec3(linhas_facecima.pIni.x - centroX, linhas_facecima.pIni.y - centroY, h);
            p3 = new vec3(linhas_facecima.pFin.x - centroX, linhas_facecima.pFin.y - centroY, h);
            v1 = p2 - p1;
            v2 = p3 - p1;
            n1 = v1 * v2;
            n1.Normalize();

            /*  GL.Begin(PrimitiveType.Polygon);
        
              GL.Normal3(n1.x, n1.y, n1.z);  
              GL.Vertex3(linhas_facecima_org.pIni.y - centroY, h, linhas_facecima_org.pIni.x - centroX);
              GL.Vertex3(linhas_facecima_org.pIni.y - centroY, h - Dados.h1, linhas_facecima_org.pIni.x - centroX);
              GL.Vertex3(linhas_facecima.pIni.y - centroY, h - Dados.h1, linhas_facecima.pIni.x - centroX);
              GL.Vertex3(linhas_facecima.pIni.y - centroY, h, linhas_facecima.pIni.x - centroX);
              GL.End();*/
            /********************/
            /*face inicial*/
          /*  GL.Begin(PrimitiveType.Polygon);
            GL.Normal3(n1.x, n1.y, n1.z);

            if (linhas_facecima_org.comprimento > linhas_facecima.comprimento)
                lin = linhas_facecima_org;
            else
                lin = linhas_facecima;
            GL.Vertex3(lin.pIni.y - centroY, h, lin.pIni.x - centroX);
            GL.Vertex3(lin.pIni.y - centroY, h - Dados.h1, lin.pIni.x - centroX);

            if (linhas_facebaixo_org.comprimento > linhas_facebaixo.comprimento)
                lin = linhas_facebaixo_org;
            else
                lin = linhas_facebaixo;
            GL.Vertex3(lin.pIni.y - centroY, h - Dados.h1, lin.pIni.x - centroX);
            GL.Vertex3(lin.pIni.y - centroY, h, lin.pIni.x - centroX);
            GL.End();

            //////////////////////////////////////////
            /*face final*/
            // GL.Color3(0, 0, 255); 
            //  GL.Color3(0, 0, 255);
           /* GL.Begin(PrimitiveType.Polygon);
            GL.Normal3(n1.x, n1.y, n1.z);

            if (linhas_facecima_org.comprimento > linhas_facecima.comprimento)
                lin = linhas_facecima_org;
            else
                lin = linhas_facecima;
            GL.Vertex3(lin.pFin.y - centroY, h, lin.pFin.x - centroX);
            GL.Vertex3(lin.pFin.y - centroY, h - Dados.h1, lin.pFin.x - centroX);

            if (linhas_facebaixo_org.comprimento > linhas_facebaixo.comprimento)
                lin = linhas_facebaixo_org;
            else
                lin = linhas_facebaixo;
            GL.Vertex3(lin.pFin.y - centroY, h - Dados.h1, lin.pFin.x - centroX);
            GL.Vertex3(lin.pFin.y - centroY, h, lin.pFin.x - centroX);
            GL.End();

            GL.Color4(rgb[0], rgb[1], rgb[2], transparencia);
            /*face final*/
           /* p1 = new vec3(linhas_facebaixo_org.pFin.x - centroX, linhas_facebaixo_org.pFin.y - centroY, h - Dados.h1);
            p2 = new vec3(linhas_facebaixo_org.pIni.x - centroX, linhas_facebaixo_org.pIni.y - centroY, h);
            p3 = new vec3(linhas_facebaixo_org.pFin.x - centroX, linhas_facebaixo_org.pFin.y - centroY, h);
            v1 = p2 - p1;
            v2 = p3 - p1;
            n1 = v1 * v2;
            n1.Normalize();
            GL.Begin(PrimitiveType.Polygon);
            GL.Normal3(n1.x, n1.y, n1.z);
            //GL.Normal3f(0, 0, 0);
            GL.Vertex3(linhas_facebaixo_org.pFin.y - centroY, h, linhas_facebaixo_org.pFin.x - centroX);
            GL.Vertex3(linhas_facebaixo_org.pFin.y - centroY, h - Dados.h1, linhas_facebaixo_org.pFin.x - centroX);
            GL.Vertex3(linhas_facebaixo.pFin.y - centroY, h - Dados.h1, linhas_facebaixo.pFin.x - centroX);
            GL.Vertex3(linhas_facebaixo.pFin.y - centroY, h, linhas_facebaixo.pFin.x - centroX);
            GL.End();
            /********************/


           /* GL.Begin(PrimitiveType.Polygon);
            GL.Normal3(n1.x, n1.y, n1.z);
            GL.Vertex3(linhas_facecima.pFin.y - centroY, h, linhas_facecima.pFin.x - centroX);
            GL.Vertex3(linhas_facecima.pFin.y - centroY, h - Dados.h1, linhas_facecima.pFin.x - centroX);
            GL.Vertex3(linhas_facebaixo_org.pFin.y - centroY, h - Dados.h1, linhas_facebaixo_org.pFin.x - centroX);
            GL.Vertex3(linhas_facebaixo_org.pFin.y - centroY, h, linhas_facebaixo_org.pFin.x - centroX);
            GL.End();

            p1 = new vec3(linhas_facebaixo_org.pFin.x - centroX, linhas_facebaixo_org.pFin.y - centroY, h - Dados.h1);
            p2 = new vec3(linhas_facebaixo_org.pIni.x - centroX, linhas_facebaixo_org.pIni.y - centroY, h);
            p3 = new vec3(linhas_facebaixo_org.pFin.x - centroX, linhas_facebaixo_org.pFin.y - centroY, h);
            v1 = p2 - p1;
            v2 = p3 - p1;
            n1 = v1 * v2;
            n1.Normalize();

            GL.Begin(PrimitiveType.Polygon);
            GL.Normal3(n1.x, n1.y, n1.z);
            GL.Vertex3(linhas_facecima_org.pFin.y - centroY, h, linhas_facecima_org.pFin.x - centroX);
            GL.Vertex3(linhas_facecima_org.pFin.y - centroY, h - Dados.h1, linhas_facecima_org.pFin.x - centroX);
            GL.Vertex3(linhas_facecima.pFin.y - centroY, h - Dados.h1, linhas_facecima.pFin.x - centroX);
            GL.Vertex3(linhas_facecima.pFin.y - centroY, h, linhas_facecima.pFin.x - centroX);
            GL.End();

            /*face lateral de baixo*/
          /*  GL.Begin(PrimitiveType.Polygon);
            GL.Normal3(n1.x, n1.y, n1.z);
            GL.TexCoord3(linhas_facebaixo.pIni.y - centroY, h, linhas_facebaixo.pIni.x - centroX);
            GL.Vertex3(linhas_facebaixo.pIni.y - centroY, h, linhas_facebaixo.pIni.x - centroX);
            GL.TexCoord3(linhas_facebaixo.pIni.y - centroY, h - Dados.h1, linhas_facebaixo.pIni.x - centroX);
            GL.Vertex3(linhas_facebaixo.pIni.y - centroY, h - Dados.h1, linhas_facebaixo.pIni.x - centroX);
            GL.TexCoord3(linhas_facebaixo.pFin.y - centroY, h - Dados.h1, linhas_facebaixo.pFin.x - centroX);
            GL.Vertex3(linhas_facebaixo.pFin.y - centroY, h - Dados.h1, linhas_facebaixo.pFin.x - centroX);
            GL.TexCoord3(linhas_facebaixo.pFin.y - centroY, h, linhas_facebaixo.pFin.x - centroX);
            GL.Vertex3(linhas_facebaixo.pFin.y - centroY, h, linhas_facebaixo.pFin.x - centroX);
            GL.End();


            /*face lateral de cima*/
          /*  p1 = new vec3(linhas_facecima.pFin.x - centroX, linhas_facecima.pFin.y - centroY, h - Dados.h1);
            p2 = new vec3(linhas_facecima.pIni.x - centroX, linhas_facecima.pIni.y - centroY, h);
            p3 = new vec3(linhas_facecima.pFin.x - centroX, linhas_facecima.pFin.y - centroY, h);
            v1 = p2 - p1;
            v2 = p3 - p1;
            n1 = v1 * v2;
            n1.Normalize();

            GL.Begin(PrimitiveType.Polygon);
            GL.Normal3(n1.x, n1.y, n1.z);    //     GL.Normal3f(0, 0, -1);
            GL.Vertex3(linhas_facecima.pIni.y - centroY, h, linhas_facecima.pIni.x - centroX);
            GL.Vertex3(linhas_facecima.pIni.y - centroY, h - Dados.h1, linhas_facecima.pIni.x - centroX);
            GL.Vertex3(linhas_facecima.pFin.y - centroY, h - Dados.h1, linhas_facecima.pFin.x - centroX);
            GL.Vertex3(linhas_facecima.pFin.y - centroY, h, linhas_facecima.pFin.x - centroX);
            GL.End();

            /* GL.Begin(PrimitiveType.Polygon);
             GL.Normal3f(-1, 0, 0); //  GL.Normal3(n1.x, n1.y, n1.z);
             GL.Vertex3(linhas_facecima_org.pIni.y - centroY, h, linhas_facecima_org.pIni.x - centroX);
             GL.Vertex3(linhas_facecima_org.pIni.y - centroY, h - Dados.h1, linhas_facecima_org.pIni.x - centroX);
             GL.Vertex3(linhas_facebaixo_org.pIni.y - centroY, h - Dados.h1, linhas_facebaixo_org.pIni.x - centroX);
             GL.Vertex3(linhas_facebaixo_org.pIni.y - centroY, h, linhas_facebaixo_org.pIni.x - centroX);
             GL.End();*/

            /* GL.Begin(PrimitiveType.Polygon);
             GL.Normal3f(0, -1, 0);    //     GL.Normal3(n1.x, n1.y, n1.z);
             GL.Vertex3(linhas_facecima_org.pFin.y - centroY, h, linhas_facecima_org.pFin.x - centroX);
             GL.Vertex3(linhas_facecima_org.pFin.y - centroY, h - Dados.h1, linhas_facecima_org.pFin.x - centroX);
             GL.Vertex3(linhas_facebaixo_org.pFin.y - centroY, h - Dados.h1, linhas_facebaixo_org.pFin.x - centroX);
             GL.Vertex3(linhas_facebaixo_org.pFin.y - centroY, h, linhas_facebaixo_org.pFin.x - centroX);
             GL.End();*/

            /*       GL.Begin(PrimitiveType.Polygon);
                   GL.Vertex3(linhas_facecima_org.pIni.y - centroY, h, linhas_facecima_org.pIni.x - centroX);
                   GL.Vertex3(linhas_facecima_org.pIni.y - centroY,h -Dados.h1, linhas_facecima_org.pIni.x - centroX);
                   GL.Vertex3(linhas_facecima_org.pFin.y - centroY, h-Dados.h1, linhas_facecima_org.pFin.x - centroX);
                   GL.Vertex3(linhas_facecima_org.pFin.y - centroY, h, linhas_facecima_org.pFin.x - centroX);
                   GL.End();     */


            // GL.Color3(169, 169, 169);

            //face de cima e de baixo da viga
            //  GL.Color4(rgb[0], rgb[1], rgb[2], transparencia);

            /*face de baixo*/
          /*  GL.Begin(PrimitiveType.Polygon);

            /*v1 = new vec3(linhas_facebaixo.pFin.y - centroY, h - Dados.h1, linhas_facebaixo.pFin.x - centroX);
            v2 = new vec3(linhas_facecima.pIni.y - centroY, h - Dados.h1, linhas_facecima.pIni.x - centroX);
            n1 = v2 * v1;*/

         /*   if (Dados.numero == 17)
                Dados.numero = 17;
            p1 = new vec3(linhas_facecima.pFin.x - centroX, linhas_facecima.pFin.y - centroY, h - Dados.h1);
            p2 = new vec3(linhas_facebaixo.pIni.x + (linhas_facebaixo.pIni.x / 2) - centroX, linhas_facebaixo.pIni.y + (linhas_facebaixo.pIni.y / 2) - centroY, h - Dados.h1);
            p3 = new vec3(linhas_facebaixo.pFin.x - centroX, linhas_facebaixo.pFin.y - centroY, h - Dados.h1);

            v1 = p2 - p1;
            v2 = p3 - p1;

            n1 = v1 * v2;

            n1.Normalize();
            ///  GL.Normal3(n1.y, n1.z, n1.x);
            GL.Normal3f(0, 1, 0);
            GL.Vertex3(linhas_facecima_org.pIni.y - centroY, h - Dados.h1, linhas_facecima_org.pIni.x - centroX);
            //     GL.Vertex3(linhas_eixo_org.pIni.y - centroY, h - Dados.h1, linhas_eixo_org.pIni.x - centroX);
            GL.Vertex3(linhas_facebaixo_org.pIni.y - centroY, h - Dados.h1, linhas_facebaixo_org.pIni.x - centroX);
            GL.Vertex3(linhas_facebaixo_org.pFin.y - centroY, h - Dados.h1, linhas_facebaixo_org.pFin.x - centroX);
            //  GL.Vertex3(linhas_eixo_org.pFin.y - centroY, h - Dados.h1, linhas_eixo_org.pFin.x - centroX);
            GL.Vertex3(linhas_facecima_org.pFin.y - centroY, h - Dados.h1, linhas_facecima_org.pFin.x - centroX);
            GL.End();

            if (linhas_facebaixo.comprimento > linhas_facecima.comprimento)
                lin = linhas_facebaixo;
            else
                lin = linhas_facecima;

            /*   v1 = new vec3(linhas_facebaixo.pFin.y - centroY, h, linhas_facebaixo.pFin.x - centroX);
               v2 = new vec3(linhas_facecima.pIni.y - centroY, h, linhas_facecima.pIni.x - centroX);
               n1 = v1 * v2;
               n1.Normalize();*/

            /*face de cima*/

         /*   p1 = new vec3(linhas_facecima.pIni.x - centroX, linhas_facecima.pIni.y - centroY, h);
            p2 = new vec3(linhas_facebaixo.pFin.x - centroX, linhas_facebaixo.pFin.y - centroY, h);
            p3 = new vec3(linhas_facebaixo.pIni.x - centroX, linhas_facebaixo.pIni.y - centroY, h);

            v1 = p2 - p1;
            v2 = p3 - p1;

            n1 = v1 * v2;
            n1.Normalize();

            GL.Begin(PrimitiveType.Polygon);
            //    GL.Normal3(n1.x, n1.y, n1.z);

            GL.Normal3f(0, 1, 0);
            GL.Vertex3(linhas_facecima.pIni.y - centroY, h, linhas_facecima.pIni.x - centroX);
            GL.Vertex3(linhas_eixo.pIni.y - centroY, h, linhas_eixo.pIni.x - centroX);
            GL.Vertex3(linhas_facebaixo.pIni.y - centroY, h, linhas_facebaixo.pIni.x - centroX);
            GL.Vertex3(linhas_facebaixo.pFin.y - centroY, h, linhas_facebaixo.pFin.x - centroX);
            GL.Vertex3(linhas_eixo.pFin.y - centroY, h, linhas_eixo.pFin.x - centroX);
            GL.Vertex3(linhas_facecima.pFin.y - centroY, h, linhas_facecima.pFin.x - centroX);
            GL.End();*/
        }
        [NonSerialized]
        vec3 n1, v1, v2, p1, p2, p3;
        [NonSerialized]
        TLinha lin;
        /* 
indiceTipo:

Retangular    0
Seção T       1
Seção I       3
Seção L       4

*/
        public void TriangularizaFace()
        {
            CPoint2D[] vertices;
            vertices = new CPoint2D[this.Dados.Poligono.coords.Length-1];

            for (int i = 0; i < this.Dados.Poligono.coords.Length-1; i++)
              vertices[i] = new CPoint2D(this.Dados.Poligono.coords[i].X, this.Dados.Poligono.coords[i].Y);           

            PoligonoFace1 = new CPolygonShape(vertices);
            PoligonoFace1.CutEar();
        }

        public override TObjetoDesenho Clone()
        {
            TTrechoViga l = new TTrechoViga();
            l.Copy(this);
            return l;
        }

        public void Copy(TTrechoViga obj)
        {
            base.Copy(obj);
     
            if ((Object)obj.pIni != null)
              pIni = (TPonto)obj.pIni.Clone();
            if ((Object)obj.pFin != null)
              pFin = (TPonto)obj.pFin.Clone();
            Tipo = obj.Tipo;
            areaSuperior = obj.areaSuperior;
            PP = obj.PP;
            this.Pavimento = obj.Pavimento;

            this.comprimento      = obj.comprimento;
            if ((Object)obj.linhas_facecima != null)
                this.linhas_facecima = (TLinha)obj.linhas_facecima.Clone();
            if ((Object)obj.linhas_facebaixo != null)
                this.linhas_facebaixo = (TLinha)obj.linhas_facebaixo.Clone();
            if ((Object)obj.linhas_eixo != null)
                this.linhas_eixo = (TLinha)obj.linhas_eixo.Clone();

            if ((Object)obj.linhas_facecima_org != null)
                this.linhas_facecima_org = (TLinha)obj.linhas_facecima_org.Clone();
            if ((Object)obj.linhas_facebaixo_org != null)
                this.linhas_facebaixo_org = (TLinha)obj.linhas_facebaixo_org.Clone();
            if ((Object)obj.linhas_eixo_org != null)
                this.linhas_eixo_org = (TLinha)obj.linhas_eixo_org.Clone();

            if ((Object)obj.linhas_facecima_longa != null)
                this.linhas_facecima_longa = (TLinha)obj.linhas_facecima_longa.Clone();
            if ((Object)obj.linhas_facebaixo_longa != null)
                this.linhas_facebaixo_longa = (TLinha)obj.linhas_facebaixo_longa.Clone();
            if ((Object)obj.linhas_eixo_longa != null)
                this.linhas_eixo_longa = (TLinha)obj.linhas_eixo_longa.Clone();

            if ((Object)obj.linhas_facecima_longa2 != null)
                this.linhas_facecima_longa2 = (TLinha)obj.linhas_facecima_longa2.Clone();
            if ((Object)obj.linhas_facebaixo_longa2 != null)
                this.linhas_facebaixo_longa2 = (TLinha)obj.linhas_facebaixo_longa2.Clone();

            if ((Object)obj.linhas_facecima_longa_m1 != null)
                this.linhas_facecima_longa_m1 = (TLinha)obj.linhas_facecima_longa_m1.Clone();
            if ((Object)obj.linhas_facebaixo_longa_m1 != null)
                this.linhas_facebaixo_longa_m1 = (TLinha)obj.linhas_facebaixo_longa_m1.Clone();

            if ((Object)obj.linhas_facecima_longa_m2 != null)
                this.linhas_facecima_longa_m2 = (TLinha)obj.linhas_facecima_longa_m2.Clone();
            if ((Object)obj.linhas_facebaixo_longa_m2 != null)
                this.linhas_facebaixo_longa_m2 = (TLinha)obj.linhas_facebaixo_longa_m2.Clone();

            if ((Object)obj.linhas_faceeixo_longa2 != null)
                this.linhas_faceeixo_longa2 = (TLinha)obj.linhas_faceeixo_longa2.Clone();

            if ((Object)obj.linhas_faceeixo_longa != null)
                this.linhas_faceeixo_longa = (TLinha)obj.linhas_faceeixo_longa.Clone();

            if ((Object)obj.linhas_faceeixo_longa_m2 != null)
                this.linhas_faceeixo_longa_m2 = (TLinha)obj.linhas_faceeixo_longa_m2.Clone();

            if ((Object)obj.linhas_faceeixo_longa_m1 != null)
                this.linhas_faceeixo_longa_m1 = (TLinha)obj.linhas_faceeixo_longa_m1.Clone();
                       
            if ((Object)obj.linha_eixo_aux != null)
                linha_eixo_aux = (TLinha)obj.linha_eixo_aux.Clone();
            if ((Object)obj.linhas_faceeixo_longa_m1 != null)
                linhas_faceeixo_longa_m1 = (TLinha)obj.linhas_faceeixo_longa_m1.Clone();
            if ((Object)obj.linhas_faceeixo_longa_m2 != null)
                linhas_faceeixo_longa_m2 = (TLinha)obj.linhas_faceeixo_longa_m2.Clone();

            if ((Object)obj.BarraRigida_Ini != null)
                this.BarraRigida_Ini = (TLinha)obj.BarraRigida_Ini.Clone();
            if ((Object)obj.BarraRigida_Fin != null)
                this.BarraRigida_Fin = (TLinha)obj.BarraRigida_Fin.Clone();

            if ((Object)obj.Texto1 != null)
                this.Texto1 = (TTexto)obj.Texto1.Clone();
            if ((Object)obj.Texto2 != null)
                this.Texto2 = (TTexto)obj.Texto2.Clone();
            if ((Object)obj.TextoInfo != null)
                this.TextoInfo = (TTexto)obj.TextoInfo.Clone();

            this.ComVigaFaceFinal   = obj.ComVigaFaceFinal;
            this.ComVigaFaceInicial = obj.ComVigaFaceInicial;

            Grips = new List<TGrip>();

            if (obj.Grips != null)
              foreach (TGrip g in obj.Grips)
                 Grips.Add(new TGrip(this,g.x,g.y,g.translacao, g.rotacao, g.escala, g.estica, g.layer));

            Selecionado = obj.Selecionado;
            
            Layer       = obj.Layer;
            layer       = obj.layer; 

            Visivel = obj.Visivel;
            mPen   = new Pen(Color.White);
            points = new System.Drawing.Point[4];
            mBrush = new System.Drawing.SolidBrush(System.Drawing.Color.BlueViolet);
            xIni= obj.xIni;
            yIni= obj.yIni;
            zIni= obj.zIni; 
            xFin= obj.xFin;
            yFin = obj.yFin;
            zFin = obj.zFin;
            angulo = obj.angulo;
            ang2 = obj.ang2;
            NumPilar_PontoInicial = obj.NumPilar_PontoInicial;
            NumPilar_PontoFinal = obj.NumPilar_PontoFinal;

            if ((Object)obj.Dados != null)
            {
                this.Dados = (TDadosViga)obj.Dados.Clone();
                if ((Object)Dados.Poligono != null)
                  foreach (TLinha lin in Dados.Poligono.linhas_poligonal)
                      lin.layer = this.layer;
            }
        }


        [NonSerialized]
        CPolygonShape PoligonoFace1, PoligonoFaceCima;
        [NonSerialized]
        public Pen mPen = new Pen(Color.White);
        [NonSerialized]
        public System.Drawing.Point[] points = new System.Drawing.Point[4];
        [NonSerialized]
        public System.Drawing.SolidBrush mBrush = new System.Drawing.SolidBrush(System.Drawing.Color.BlueViolet);

        [NonSerialized]
        public float[] dashValues = { 3, 8, 5 };
        [NonSerialized]
        List<TPonto> PontosEmLinha;
        [NonSerialized]
        List<TrechosQuebra> TrechosQuebrar;
        int prolongamento = 2000;
        double novox2, novoy2;
        double tg0 = 1;
        int i, j;
    }
}
