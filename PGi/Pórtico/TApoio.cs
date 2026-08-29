using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using OpenTK;
using OpenTK.Input;
using OpenTK.Graphics.OpenGL;
using OpenTK.Platform;
using System.Drawing;

namespace PG
{
    [Serializable]
    public class TApoio : TObjetoDesenho
    {
     //   public TPonto pIni;
        public TDadosApoio Dados;
        public int Pavimento, IDApoio;
        public TApoio()
        {

        }
        public TApoio(TDadosApoio dados = null)
        {
            base.Visivel = true;
            base.IdLayer = Lay.Barras;
            this.Tipo = Const.ID_APOIO;
            this.Selecionado = false;
            base.Selecionado = false;
        }

        public TApoio(TPonto pInicial)
        {
            if (Geom.Iguais(pInicial.x, 0))
                pInicial.x = 0;

            if (Geom.Iguais(pInicial.y, 0))
                pInicial.y = 0;

            if (Geom.Iguais(pInicial.z, 0))
                pInicial.z = 0;

            this.pIni = pInicial;
        }

        public TApoio(TPonto p1, TPonto p2, TLayer lay, TDadosApoio dados, int pav)
        {
            string ss = "";

            List<TLinha> linhas = new List<TLinha>();
            this.Initialize(ref p1, ref ss, dados, lay, ref linhas, ref pav);
            InsercaoIndividual = true;
            this.pIni = (TPonto)p1.Clone();
            this.pFin = (TPonto)p2.Clone();

            this.Selecionado = false;
            base.Selecionado = false;
            base.Visivel = true;
            base.IdLayer = Lay.Barras;
            this.Tipo = Const.ID_APOIO;
            this.angulo = (float)(FuncoesGerais.atand((p1.y - p2.y) / (p1.x - p2.x)));
        }
        public override void Initialize(ref TPonto point, ref string command, ISettings Dados, TLayer layer, ref List<TLinha> Linhas, ref int pavimento)
        {
            this.Dados = Dados as TDadosApoio;
            base.Tipo = Const.ID_APOIO;

            base.pIni = (TPonto)point.Clone();
            pIni = (TPonto)point.Clone();
            base.pFin = (TPonto)point.Clone();
            pFin = (TPonto)point.Clone();
            InsercaoIndividual = true;
            this.layer = layer;
            this.Pavimento = pavimento;
            base.Initialize(ref point, ref command, Dados, layer, ref Linhas, ref pavimento);
            command = Const.CMD_APOIO_1_P;
        }
        public override string PrimeiroComando()
        {
            return Const.CMD_APOIO_1_P;
        }
        public override void Proximo()
        {

            base.Proximo();
        }

        [NonSerialized]
        public List<TPoligono> poligonosApoio;
          

        double z_pixel1;
        bool ForaDaTela;
        public override void SetaSelecao(bool s, bool MostraGrip, bool SelecaoDeCandidato = false, TObjetoDesenho Owner = null)
        {
            base.Selecionado = false;
            if (!this.Visivel)
                return;
            FPrincipal.pixel1(ref  pIni.x, ref  pIni.y, ref  pIni.z, ref z_pixel1);
            ForaDaTela = ((FPrincipal.px_x1[0] < 0)) ||
                              ((FPrincipal.px_x1[0] > FPrincipal.w)) ||
                              ((FPrincipal.px_y1[0] < 0)) ||
                              ((FPrincipal.px_y1[0] > FPrincipal.h));
            if (ForaDaTela)
                return;

            ForaDaTela = !(z_pixel1 < 1 && z_pixel1 > 0);

            if (ForaDaTela)
                return;

            Soma_Z = z_pixel1;

            base.Selecionado = s;

        }

        public override bool Selecionar(float clicx, float clicy, float coordx, float coordy, TObjetoDesenho Owner = null)
        {
            try
            {
               // GeraPoligonosSelecao();
              
            /*    z_clip = 0;
                Desenho.pixel1(ref  pIni.x, ref  pIni.y, ref  pIni.z, ref z_clip);
              
                bool ForaDaTela = ((Desenho.px_x1[0] < 0)) ||
                  ((Desenho.px_x1[0] > Desenho.w)) ||
                  ((Desenho.px_y1[0] < 0)) ||
                  ((Desenho.px_y1[0] > Desenho.h));
              
                if (ForaDaTela)
                    return false;

                if (z_clip < 1 && z_clip > 0)*/
          /*      {
                    foreach (TPoligono p1 in poligonosApoio)
                    {
                        if (p1.PontoEmPoligono(clicx, clicy))
                        {
                            SetaSelecao(true, true);
                            return base.Selecionado;
                        }
                    }
                }*/
                return false;

            }
            catch (Exception)
            {

            }
            return false;
        }
        public bool InsercaoIndividual = true;
        public override eObjetoDesenhoMouseDown OnMouseDown(ref TPonto point, ref string command, List<TLinha> Linhas, List<TPonto> Pontos, List<TPilar> Pilares, ref List<TObjetoDesenho> Objects, bool FromGrip = false)
        {
          /*  if ((Geom.Iguais(point.x, this.pIni.x)) && (Geom.Iguais(point.y, this.pIni.y)) && (Geom.Iguais(point.z, this.pIni.z)))
            {
                return eDrawObjectMouseDown.Continue;
            }
            */
            if (InsercaoIndividual)
                return eObjetoDesenhoMouseDown.DoneRepeat;
            else
                return eObjetoDesenhoMouseDown.Done;
        }

        public override void Mover(ref TPonto ponto1,ref  TPonto ponto2, bool dinamico)
        {

            pIni.Mover(ref ponto1, ref ponto2, dinamico);

        }


        [NonSerialized]
        OpenTK.Graphics.TextPrinter textoCota = new OpenTK.Graphics.TextPrinter(OpenTK.Graphics.TextQuality.High);


        public override void OnMouseMove(ref TPonto point, bool ShowInfo, bool orto = true,
                                         double angulo = 0, bool PontoInicial = false)
        {
            //se está movendo a viga a partir do ponto inicial ou final
         /*   if (PontoInicial)
          {
                pIni = (TPonto)point.Clone();
             //   pIni.z *=-1;
            }
            else
            {
            }*/

            pIni = (TPonto)point.Clone();
            pFin = (TPonto)point.Clone();

        }
        int qtd_CoordsSecao = 0;
        double r, g, b;
        void setLista(ref List<float> coord_objeto, double x, double y, double z)
        {
            coord_objeto.Add((float)x);
            coord_objeto.Add((float)y);
            coord_objeto.Add((float)z);
        }

        void setLista(ref List<float> coord_objeto, double x, double y, double z,double r, double g, double b)
        {
            coord_objeto.Add((float)x);
            coord_objeto.Add((float)y);
            coord_objeto.Add((float)z);

            coord_objeto.Add((float)r);
            coord_objeto.Add((float)g);
            coord_objeto.Add((float)b);
        }

        void setListaTextura(ref List<float> coords_triangulos, double x, double y)
        {
            coords_triangulos.Add((float)x);
            coords_triangulos.Add((float)y);
        }

        void SetaTriangulo(double x1, double y1, double z1,
                           double x2, double y2, double z2,
                           double x3, double y3, double z3,
                           ref List<float> coords_triangulos, ref List<Triangulo> triangulos_selecao)
        {
            p1 = new vec3(x1,y1, z1);
            p2 = new vec3(x2,y2, z2);
            p3 = new vec3(x3,y3, z3);
            v1 = p2 - p1;
            v2 = p3 - p1;
            n1 = v1.CrossProduct(v2);
            n1.Normalize();
            //n1 *= -1;
            // TRIANGULO 1 //
            setLista(ref coords_triangulos, x1, y1, z1);
            setLista(ref coords_triangulos, n1.x, n1.y, n1.z);
            setLista(ref coords_triangulos, r, g, b);
            setListaTextura(ref coords_triangulos, 1, 1);

            setLista(ref coords_triangulos, x2, y2, z2);
            setLista(ref coords_triangulos, n1.x, n1.y, n1.z);
            setLista(ref coords_triangulos, r, g, b);
            setListaTextura(ref coords_triangulos, 1, 1);

            setLista(ref coords_triangulos, x3, y3, z3);
            setLista(ref coords_triangulos, n1.x, n1.y, n1.z);
            setLista(ref coords_triangulos, r, g, b);
            setListaTextura(ref coords_triangulos, 1, 1);
            
            Triangulo tr = new Triangulo(-1, "apoio");
            tr.p0 = new vec3(x1, y1, z1);
            tr.p1 = new vec3(x2, y2, z2);
            tr.p2 = new vec3(x3, y3, z3);
            tr._normal.x = -n1.x;
            tr._normal.y = -n1.y;
            tr._normal.z = -n1.z;
            tr.CriaPlano(tr.p0);
            tr.idApoio = IDApoio;
            triangulos_selecao.Add(tr);
        }
        [NonSerialized]
        List<LinhaVec3> linhas_engaste;
        public void Preenche_Arestas(ref List<float> coords_arestas, double TamApoio,ref bool arestas, ref bool arestasConfObjeto, ref bool unifilar)
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
                    r = 0.1;
                    g =0.9;
                    b = 0.6;
                }
                if (Dados.Tipo == 1 || Dados.Tipo == -1) // apoio simples
                {
                    setLista(ref coords_arestas, pIni.x, pIni.y, pIni.z, r, g, b);
                    setLista(ref coords_arestas, pIni.x + TamApoio, pIni.y - TamApoio, pIni.z + TamApoio, r, g, b);

                    setLista(ref coords_arestas, pIni.x, pIni.y, pIni.z, r, g, b);
                    setLista(ref coords_arestas, pIni.x - TamApoio, pIni.y - TamApoio, pIni.z + TamApoio, r, g, b);

                    setLista(ref coords_arestas, pIni.x, pIni.y, pIni.z, r, g, b);
                    setLista(ref coords_arestas, pIni.x + TamApoio, pIni.y + TamApoio, pIni.z + TamApoio, r, g, b);

                    setLista(ref coords_arestas, pIni.x, pIni.y, pIni.z, r, g, b);
                    setLista(ref coords_arestas, pIni.x - TamApoio, pIni.y + TamApoio, pIni.z + TamApoio, r, g, b);
                    
                    //// fundo
                    setLista(ref coords_arestas, pIni.x + TamApoio, pIni.y - TamApoio, pIni.z + TamApoio, r, g, b);
                    setLista(ref coords_arestas, pIni.x - TamApoio, pIni.y - TamApoio, pIni.z + TamApoio, r, g, b);

                    setLista(ref coords_arestas, pIni.x + TamApoio, pIni.y - TamApoio, pIni.z + TamApoio, r, g, b);
                    setLista(ref coords_arestas, pIni.x + TamApoio, pIni.y + TamApoio, pIni.z + TamApoio, r, g, b);

                    setLista(ref coords_arestas, pIni.x + TamApoio, pIni.y + TamApoio, pIni.z + TamApoio, r, g, b);
                    setLista(ref coords_arestas, pIni.x - TamApoio, pIni.y + TamApoio, pIni.z + TamApoio, r, g, b);

                    setLista(ref coords_arestas, pIni.x - TamApoio, pIni.y + TamApoio, pIni.z + TamApoio, r, g, b);
                    setLista(ref coords_arestas, pIni.x - TamApoio, pIni.y - TamApoio, pIni.z + TamApoio, r, g, b);
                }
                else
                if (Dados.Tipo == 2)
                {
                    linhas_engaste = new List<LinhaVec3>();
                    linhas_engaste.Add(new LinhaVec3(new vec3(pIni.x, pIni.y, pIni.z), new vec3(pIni.x - TamApoio, pIni.y, pIni.z)));
                    linhas_engaste.Add(new LinhaVec3(new vec3(pIni.x, pIni.y, pIni.z), new vec3(pIni.x, pIni.y - TamApoio, pIni.z)));
                    linhas_engaste.Add(new LinhaVec3(new vec3(pIni.x, pIni.y, pIni.z), new vec3(pIni.x, pIni.y, pIni.z + TamApoio)));
                    
                    ///////////////////////
                    linhas_engaste.Add(new LinhaVec3(new vec3(pIni.x - TamApoio / 2, pIni.y - TamApoio / 2, pIni.z + TamApoio), new vec3(pIni.x + TamApoio / 2, pIni.y - TamApoio / 2, pIni.z + TamApoio)));
                    linhas_engaste.Add(new LinhaVec3(new vec3(pIni.x + TamApoio / 2, pIni.y - TamApoio / 2, pIni.z + TamApoio), new vec3(pIni.x + TamApoio / 2, pIni.y + TamApoio / 2, pIni.z + TamApoio)));
                    linhas_engaste.Add(new LinhaVec3(new vec3(pIni.x + TamApoio / 2, pIni.y + TamApoio / 2, pIni.z + TamApoio), new vec3(pIni.x - TamApoio / 2, pIni.y + TamApoio / 2, pIni.z + TamApoio)));
                    linhas_engaste.Add(new LinhaVec3(new vec3(pIni.x - TamApoio / 2, pIni.y + TamApoio / 2, pIni.z + TamApoio), new vec3(pIni.x - TamApoio / 2, pIni.y - TamApoio / 2, pIni.z + TamApoio)));

                    ///////////////////////
                    linhas_engaste.Add(new LinhaVec3(new vec3(pIni.x - TamApoio, pIni.y - TamApoio / 2, pIni.z - TamApoio / 2), new vec3(pIni.x - TamApoio, pIni.y - TamApoio / 2, pIni.z + TamApoio / 2)));
                    linhas_engaste.Add(new LinhaVec3(new vec3(pIni.x - TamApoio, pIni.y - TamApoio / 2, pIni.z + TamApoio / 2), new vec3(pIni.x - TamApoio, pIni.y + TamApoio / 2, pIni.z + TamApoio / 2)));
                    linhas_engaste.Add(new LinhaVec3(new vec3(pIni.x - TamApoio, pIni.y + TamApoio / 2, pIni.z - TamApoio / 2), new vec3(pIni.x - TamApoio, pIni.y + TamApoio / 2, pIni.z + TamApoio / 2)));
                    linhas_engaste.Add(new LinhaVec3(new vec3(pIni.x - TamApoio, pIni.y + TamApoio / 2, pIni.z - TamApoio / 2), new vec3(pIni.x - TamApoio, pIni.y - TamApoio / 2, pIni.z - TamApoio / 2)));

                    ///////////////////////
                    linhas_engaste.Add(new LinhaVec3(new vec3(pIni.x - TamApoio / 2, pIni.y - TamApoio, pIni.z + TamApoio / 2), new vec3(pIni.x + TamApoio / 2, pIni.y - TamApoio, pIni.z + TamApoio / 2)));
                    linhas_engaste.Add(new LinhaVec3(new vec3(pIni.x + TamApoio / 2, pIni.y - TamApoio, pIni.z + TamApoio / 2), new vec3(pIni.x + TamApoio / 2, pIni.y - TamApoio, pIni.z - TamApoio / 2)));
                    linhas_engaste.Add(new LinhaVec3(new vec3(pIni.x + TamApoio / 2, pIni.y - TamApoio, pIni.z - TamApoio / 2), new vec3(pIni.x - TamApoio / 2, pIni.y - TamApoio, pIni.z - TamApoio / 2)));
                    linhas_engaste.Add(new LinhaVec3(new vec3(pIni.x - TamApoio / 2, pIni.y - TamApoio, pIni.z - TamApoio / 2), new vec3(pIni.x - TamApoio / 2, pIni.y - TamApoio, pIni.z + TamApoio / 2)));

                    for (int i = 0; i < linhas_engaste.Count; i++)
                    {
                        setLista(ref coords_arestas, linhas_engaste[i].p1.x, linhas_engaste[i].p1.y, linhas_engaste[i].p1.z, r, g, b);
                        setLista(ref coords_arestas, linhas_engaste[i].p2.x, linhas_engaste[i].p2.y, linhas_engaste[i].p2.z, r, g, b);
                    }

                    /*  setLista(ref coords_arestas, pIni.x, pIni.y, pIni.z, r, g, b);
                      setLista(ref coords_arestas, pIni.x - TamApoio, pIni.y, pIni.z, r, g, b);

                      setLista(ref coords_arestas, pIni.x, pIni.y, pIni.z, r, g, b);
                      setLista(ref coords_arestas, pIni.x, pIni.y - TamApoio, pIni.z, r, g, b);

                      setLista(ref coords_arestas, pIni.x, pIni.y, pIni.z, r, g, b);
                      setLista(ref coords_arestas, pIni.x, pIni.y, pIni.z + TamApoio, r, g, b);



                      setLista(ref coords_arestas, pIni.x - TamApoio/2, pIni.y - TamApoio / 2, pIni.z + TamApoio, r, g, b);
                      setLista(ref coords_arestas, pIni.x + TamApoio / 2, pIni.y - TamApoio / 2, pIni.z + TamApoio, r, g, b);

                      setLista(ref coords_arestas, pIni.x + TamApoio / 2, pIni.y - TamApoio / 2, pIni.z + TamApoio , r, g, b);
                      setLista(ref coords_arestas, pIni.x + TamApoio / 2, pIni.y + TamApoio / 2, pIni.z + TamApoio , r, g, b);

                      setLista(ref coords_arestas, pIni.x + TamApoio / 2, pIni.y + TamApoio / 2, pIni.z + TamApoio , r, g, b);
                      setLista(ref coords_arestas, pIni.x - TamApoio / 2, pIni.y + TamApoio / 2, pIni.z + TamApoio , r, g, b);

                      setLista(ref coords_arestas, pIni.x - TamApoio / 2, pIni.y + TamApoio / 2, pIni.z + TamApoio, r, g, b);
                      setLista(ref coords_arestas, pIni.x - TamApoio / 2, pIni.y - TamApoio / 2, pIni.z + TamApoio, r, g, b);
                      ///////////////////////
                      setLista(ref coords_arestas, pIni.x - TamApoio, pIni.y - TamApoio / 2, pIni.z - TamApoio/2, r, g, b);
                      setLista(ref coords_arestas, pIni.x - TamApoio, pIni.y - TamApoio / 2, pIni.z + TamApoio / 2, r, g, b);

                      setLista(ref coords_arestas, pIni.x - TamApoio, pIni.y - TamApoio / 2, pIni.z + TamApoio / 2, r, g, b);
                      setLista(ref coords_arestas, pIni.x - TamApoio, pIni.y + TamApoio / 2, pIni.z + TamApoio / 2, r, g, b);

                      setLista(ref coords_arestas, pIni.x - TamApoio, pIni.y + TamApoio / 2, pIni.z - TamApoio / 2, r, g, b);
                      setLista(ref coords_arestas, pIni.x - TamApoio, pIni.y + TamApoio / 2, pIni.z + TamApoio / 2, r, g, b);

                      setLista(ref coords_arestas, pIni.x - TamApoio, pIni.y + TamApoio / 2, pIni.z - TamApoio / 2, r, g, b);
                      setLista(ref coords_arestas, pIni.x - TamApoio, pIni.y - TamApoio / 2, pIni.z - TamApoio / 2, r, g, b);
                      ///////////////////////

                      setLista(ref coords_arestas, pIni.x - TamApoio / 2, pIni.y - TamApoio, pIni.z + TamApoio / 2, r, g, b);
                      setLista(ref coords_arestas, pIni.x + TamApoio / 2, pIni.y - TamApoio, pIni.z + TamApoio / 2, r, g, b);

                      setLista(ref coords_arestas, pIni.x + TamApoio / 2, pIni.y - TamApoio, pIni.z + TamApoio / 2, r, g, b);
                      setLista(ref coords_arestas, pIni.x + TamApoio / 2, pIni.y - TamApoio, pIni.z - TamApoio / 2, r, g, b);

                      setLista(ref coords_arestas, pIni.x + TamApoio / 2, pIni.y - TamApoio, pIni.z - TamApoio / 2, r, g, b);
                      setLista(ref coords_arestas, pIni.x - TamApoio / 2, pIni.y - TamApoio, pIni.z - TamApoio / 2, r, g, b);

                      setLista(ref coords_arestas, pIni.x - TamApoio / 2, pIni.y - TamApoio, pIni.z - TamApoio / 2, r, g, b);
                      setLista(ref coords_arestas, pIni.x - TamApoio / 2, pIni.y - TamApoio, pIni.z + TamApoio / 2, r, g, b);*/


                    /* setLista(ref coords_arestas, pIni.x + TamApoio, pIni.y - TamApoio, pIni.z + TamApoioZ, r, g, b);
                     setLista(ref coords_arestas, pIni.x - TamApoio, pIni.y - TamApoio, pIni.z + TamApoioZ, r, g, b);

                     setLista(ref coords_arestas, pIni.x + TamApoio, pIni.y - TamApoio, pIni.z + TamApoioZ, r, g, b);
                     setLista(ref coords_arestas, pIni.x + TamApoio, pIni.y + TamApoio, pIni.z + TamApoioZ, r, g, b);

                     setLista(ref coords_arestas, pIni.x + TamApoio, pIni.y + TamApoio, pIni.z + TamApoioZ, r, g, b);
                     setLista(ref coords_arestas, pIni.x - TamApoio, pIni.y + TamApoio, pIni.z + TamApoioZ, r, g, b);

                     setLista(ref coords_arestas, pIni.x - TamApoio, pIni.y + TamApoio, pIni.z + TamApoioZ, r, g, b);
                     setLista(ref coords_arestas, pIni.x - TamApoio, pIni.y - TamApoio, pIni.z + TamApoioZ, r, g, b);

                     ///


                     setLista(ref coords_arestas, pIni.x + TamApoio, pIni.y - TamApoio, pIni.z, r, g, b);
                     setLista(ref coords_arestas, pIni.x - TamApoio, pIni.y - TamApoio, pIni.z, r, g, b);

                     setLista(ref coords_arestas, pIni.x + TamApoio, pIni.y - TamApoio, pIni.z, r, g, b);
                     setLista(ref coords_arestas, pIni.x + TamApoio, pIni.y + TamApoio, pIni.z, r, g, b);

                     setLista(ref coords_arestas, pIni.x + TamApoio, pIni.y + TamApoio, pIni.z, r, g, b);
                     setLista(ref coords_arestas, pIni.x - TamApoio, pIni.y + TamApoio, pIni.z, r, g, b);

                     setLista(ref coords_arestas, pIni.x - TamApoio, pIni.y + TamApoio, pIni.z, r, g, b);
                     setLista(ref coords_arestas, pIni.x - TamApoio, pIni.y - TamApoio, pIni.z, r, g, b);*/
                }
                ;



            }
        }

        public void Preenche_Triangulos(ref List<float> coords_triangulos,double TamApoio, ref List<Triangulo> triangulos_selecao)
        {
            if (Visivel)
            {
                /* if (Selecionado)
                 {
                     r = 0.8;
                     g = 0.4;
                     b = 0.3;
                 }
                 else
                 {*/
                r = 0.1;
                g = 0.3;
                b = 0.6;
                //    }

                if (Dados.Tipo == 1 || Dados.Tipo == -1) // apoio simples
                {
                    SetaTriangulo(pIni.x, pIni.y, pIni.z,
                                   pIni.x + TamApoio, pIni.y - TamApoio, pIni.z + TamApoio,
                                   pIni.x - TamApoio, pIni.y - TamApoio, pIni.z + TamApoio,
                                   ref coords_triangulos, ref triangulos_selecao);

                    SetaTriangulo(pIni.x, pIni.y, pIni.z,
                                  pIni.x + TamApoio, pIni.y + TamApoio, pIni.z + TamApoio,
                                  pIni.x + TamApoio, pIni.y - TamApoio, pIni.z + TamApoio,
                                  ref coords_triangulos, ref triangulos_selecao);

                    SetaTriangulo(pIni.x, pIni.y, pIni.z,
                                  pIni.x - TamApoio, pIni.y + TamApoio, pIni.z + TamApoio,
                                  pIni.x + TamApoio, pIni.y + TamApoio, pIni.z + TamApoio,
                                  ref coords_triangulos, ref triangulos_selecao);

                    SetaTriangulo(pIni.x - TamApoio, pIni.y - TamApoio, pIni.z + TamApoio,
                                  pIni.x - TamApoio, pIni.y + TamApoio, pIni.z + TamApoio,
                                  pIni.x, pIni.y, pIni.z,
                                  ref coords_triangulos, ref triangulos_selecao);

                    SetaTriangulo(pIni.x - TamApoio, pIni.y - TamApoio, pIni.z + TamApoio,
                                  pIni.x + TamApoio, pIni.y - TamApoio, pIni.z + TamApoio,
                                  pIni.x + TamApoio, pIni.y + TamApoio, pIni.z + TamApoio,
                                  ref coords_triangulos, ref triangulos_selecao);
                    SetaTriangulo(pIni.x + TamApoio, pIni.y + TamApoio, pIni.z + TamApoio,
                                  pIni.x - TamApoio, pIni.y + TamApoio, pIni.z + TamApoio,
                                  pIni.x - TamApoio, pIni.y - TamApoio, pIni.z + TamApoio,
                                  ref coords_triangulos, ref triangulos_selecao);
                }
                else
                if (Dados.Tipo == 2)
                {
                    if (linhas_engaste != null)
                    {
                        SetaTriangulo(linhas_engaste[3].p2.x, linhas_engaste[3].p2.y, linhas_engaste[3].p2.z,
                                      linhas_engaste[3].p1.x, linhas_engaste[3].p1.y, linhas_engaste[3].p1.z,
                                      linhas_engaste[5].p1.x, linhas_engaste[5].p1.y, linhas_engaste[5].p1.z,
                                      ref coords_triangulos, ref triangulos_selecao);

                        SetaTriangulo(linhas_engaste[5].p1.x, linhas_engaste[5].p1.y, linhas_engaste[5].p1.z,
                            linhas_engaste[3].p1.x, linhas_engaste[3].p1.y, linhas_engaste[3].p1.z,
                            linhas_engaste[5].p2.x, linhas_engaste[5].p2.y, linhas_engaste[5].p2.z,
                            ref coords_triangulos, ref triangulos_selecao);

                        SetaTriangulo(linhas_engaste[7].p2.x, linhas_engaste[7].p2.y, linhas_engaste[7].p2.z,
                                      linhas_engaste[7].p1.x, linhas_engaste[7].p1.y, linhas_engaste[7].p1.z,
                                      linhas_engaste[8].p2.x, linhas_engaste[8].p2.y, linhas_engaste[8].p2.z,
                                      ref coords_triangulos, ref triangulos_selecao);

                        SetaTriangulo(linhas_engaste[10].p2.x, linhas_engaste[10].p2.y, linhas_engaste[10].p2.z,
                                        linhas_engaste[9].p1.x, linhas_engaste[9].p1.y, linhas_engaste[9].p1.z,
                                        linhas_engaste[9].p2.x, linhas_engaste[9].p2.y, linhas_engaste[9].p2.z,
                                        ref coords_triangulos, ref triangulos_selecao);


                        SetaTriangulo(linhas_engaste[11].p2.x, linhas_engaste[11].p2.y, linhas_engaste[11].p2.z,
                                      linhas_engaste[11].p1.x, linhas_engaste[11].p1.y, linhas_engaste[11].p1.z,
                                      linhas_engaste[12].p2.x, linhas_engaste[12].p2.y, linhas_engaste[12].p2.z,
                                      ref coords_triangulos, ref triangulos_selecao);

                        SetaTriangulo(linhas_engaste[14].p2.x, linhas_engaste[14].p2.y, linhas_engaste[14].p2.z,
                                        linhas_engaste[13].p2.x, linhas_engaste[13].p2.y, linhas_engaste[13].p2.z,
                                        linhas_engaste[13].p1.x, linhas_engaste[13].p1.y, linhas_engaste[13].p1.z,
                                        ref coords_triangulos, ref triangulos_selecao);
                    }
                }              
            }
        }
        double tamTemp = 0.05;
        double tamTempZ = 0.05;

        public override void Desenha(ref bool Unifilar, ref int transp, ref  bool arestas)
        {
            if (Visivel)
            {
                try
                {
                  /*  GL.Color4(Color.Green);//100 100 255

                    if (base.Selecionado)
                        GL.Color3(Color.Red);

                    if (Dados.Tipo == 1 || Dados.Tipo == -1) // apoio simples
                    {
                        GL.Begin(PrimitiveType.Polygon);
                        GL.Vertex3(pIni.x, pIni.y, pIni.z);
                        GL.Vertex3(pIni.x + tamTemp, pIni.y - tamTemp, pIni.z + tamTemp);
                        GL.Vertex3(pIni.x - tamTemp, pIni.y - tamTemp, pIni.z + tamTemp);
                        GL.Vertex3(pIni.x, pIni.y, pIni.z);
                        GL.End();

                        GL.Begin(PrimitiveType.Polygon);
                        GL.Vertex3(pIni.x, pIni.y, pIni.z);
                        GL.Vertex3(pIni.x + tamTemp, pIni.y + tamTemp, pIni.z + tamTemp);
                        GL.Vertex3(pIni.x + tamTemp, pIni.y - tamTemp, pIni.z + tamTemp);
                        GL.Vertex3(pIni.x, pIni.y, pIni.z);
                        GL.End();

                        GL.Begin(PrimitiveType.Polygon);
                        GL.Vertex3(pIni.x, pIni.y, pIni.z);
                        GL.Vertex3(pIni.x - tamTemp, pIni.y + tamTemp, pIni.z + tamTempZ);
                        GL.Vertex3(pIni.x + tamTemp, pIni.y + tamTemp, pIni.z + tamTempZ);
                        GL.Vertex3(pIni.x, pIni.y, pIni.z);
                        GL.End();


                        GL.Begin(PrimitiveType.Polygon);
                        GL.Vertex3(pIni.x, pIni.y, pIni.z);
                        GL.Vertex3(pIni.x - tamTemp, pIni.y + tamTemp, pIni.z + tamTempZ);
                        GL.Vertex3(pIni.x - tamTemp, pIni.y - tamTemp, pIni.z + tamTempZ);
                        GL.Vertex3(pIni.x, pIni.y, pIni.z);
                        GL.End();

                        GL.Begin(PrimitiveType.Polygon);
                        GL.Vertex3(pIni.x - tamTemp, pIni.y - tamTemp, pIni.z + tamTempZ);
                        GL.Vertex3(pIni.x + tamTemp, pIni.y - tamTemp, pIni.z + tamTempZ);
                        GL.Vertex3(pIni.x + tamTemp, pIni.y + tamTemp, pIni.z + tamTempZ);
                        GL.Vertex3(pIni.x - tamTemp, pIni.y + tamTemp, pIni.z + tamTempZ);
                        GL.Vertex3(pIni.x - tamTemp, pIni.y - tamTemp, pIni.z + tamTempZ);
                        GL.End();

                        ////arestas
                        GL.Color4(Color.Blue);
                        GL.Begin(PrimitiveType.LineLoop);
                        GL.Vertex3(pIni.x, pIni.y, pIni.z);
                        GL.Vertex3(pIni.x + tamTemp, pIni.y - tamTemp, pIni.z + tamTempZ);
                        GL.Vertex3(pIni.x - tamTemp, pIni.y - tamTemp, pIni.z + tamTempZ);
                        GL.End();

                        GL.Begin(PrimitiveType.LineLoop);
                        GL.Vertex3(pIni.x, pIni.y, pIni.z);
                        GL.Vertex3(pIni.x - tamTemp, pIni.y + tamTemp, pIni.z + tamTempZ);
                        GL.Vertex3(pIni.x + tamTemp, pIni.y + tamTemp, pIni.z + tamTempZ);
                        GL.End();

                        GL.Begin(PrimitiveType.LineLoop);
                        GL.Vertex3(pIni.x - tamTemp, pIni.y - tamTemp, pIni.z + tamTempZ);
                        GL.Vertex3(pIni.x + tamTemp, pIni.y - tamTemp, pIni.z + tamTempZ);
                        GL.Vertex3(pIni.x + tamTemp, pIni.y + tamTemp, pIni.z + tamTempZ);
                        GL.Vertex3(pIni.x - tamTemp, pIni.y + tamTemp, pIni.z + tamTempZ);
                      //  GL.Vertex3(pIni.x - tamTemp, pIni.y - tamTemp, pIni.z + tamTemp);
                        GL.End();
                    }
                    else
                    if (Dados.Tipo == 2)
                    {
                        GL.Begin(PrimitiveType.Polygon);
                        GL.Vertex3(pIni.x-tamTemp, pIni.y-tamTemp, pIni.z);
                        GL.Vertex3(pIni.x + tamTemp, pIni.y - tamTemp, pIni.z);
                        GL.Vertex3(pIni.x + tamTemp, pIni.y + tamTemp, pIni.z);
                        GL.Vertex3(pIni.x - tamTemp, pIni.y + tamTemp, pIni.z);
                        GL.Vertex3(pIni.x - tamTemp, pIni.y - tamTemp, pIni.z); 
                        GL.End();

                        GL.Begin(PrimitiveType.Polygon);
                        GL.Vertex3(pIni.x - tamTemp, pIni.y + tamTemp, pIni.z);
                        GL.Vertex3(pIni.x - tamTemp, pIni.y + tamTemp, pIni.z + tamTempZ);
                        GL.Vertex3(pIni.x + tamTemp, pIni.y + tamTemp, pIni.z + tamTempZ);
                        GL.Vertex3(pIni.x + tamTemp, pIni.y + tamTemp, pIni.z);
                        GL.Vertex3(pIni.x - tamTemp, pIni.y + tamTemp, pIni.z);
                        GL.End();

                        GL.Begin(PrimitiveType.Polygon);
                        GL.Vertex3(pIni.x - tamTemp, pIni.y - tamTemp, pIni.z);
                        GL.Vertex3(pIni.x - tamTemp, pIni.y - tamTemp, pIni.z + tamTempZ);
                        GL.Vertex3(pIni.x + tamTemp, pIni.y - tamTemp, pIni.z + tamTempZ);
                        GL.Vertex3(pIni.x + tamTemp, pIni.y - tamTemp, pIni.z);
                        GL.Vertex3(pIni.x - tamTemp, pIni.y - tamTemp, pIni.z);
                        GL.End();

                        GL.Begin(PrimitiveType.Polygon);
                        GL.Vertex3(pIni.x - tamTemp, pIni.y - tamTemp, pIni.z);
                        GL.Vertex3(pIni.x - tamTemp, pIni.y - tamTemp, pIni.z + tamTempZ);
                        GL.Vertex3(pIni.x - tamTemp, pIni.y + tamTemp, pIni.z + tamTempZ);
                        GL.Vertex3(pIni.x - tamTemp, pIni.y + tamTemp, pIni.z);
                        GL.Vertex3(pIni.x - tamTemp, pIni.y - tamTemp, pIni.z);
                        GL.End();

                        GL.Begin(PrimitiveType.Polygon);
                        GL.Vertex3(pIni.x + tamTemp, pIni.y - tamTemp, pIni.z);
                        GL.Vertex3(pIni.x + tamTemp, pIni.y - tamTemp, pIni.z + tamTempZ);
                        GL.Vertex3(pIni.x + tamTemp, pIni.y + tamTemp, pIni.z + tamTempZ);
                        GL.Vertex3(pIni.x + tamTemp, pIni.y + tamTemp, pIni.z);
                        GL.Vertex3(pIni.x + tamTemp, pIni.y - tamTemp, pIni.z);
                        GL.End();

                        GL.Begin(PrimitiveType.Polygon);
                        GL.Vertex3(pIni.x - tamTemp, pIni.y - tamTemp, pIni.z + tamTempZ);
                        GL.Vertex3(pIni.x + tamTemp, pIni.y - tamTemp, pIni.z + tamTempZ);
                        GL.Vertex3(pIni.x + tamTemp, pIni.y + tamTemp, pIni.z + tamTempZ);
                        GL.Vertex3(pIni.x - tamTemp, pIni.y + tamTemp, pIni.z + tamTempZ);
                        GL.Vertex3(pIni.x - tamTemp, pIni.y - tamTemp, pIni.z + tamTempZ);
                        GL.End();

                        ////arestas
                        GL.Color4(Color.Blue);
                        GL.Begin(PrimitiveType.LineLoop);
                        GL.Vertex3(pIni.x - tamTemp, pIni.y + tamTemp, pIni.z);
                        GL.Vertex3(pIni.x - tamTemp, pIni.y + tamTemp, pIni.z + tamTempZ);
                        GL.Vertex3(pIni.x + tamTemp, pIni.y + tamTemp, pIni.z + tamTempZ);
                        GL.Vertex3(pIni.x + tamTemp, pIni.y + tamTemp, pIni.z);
                        GL.End();

                        GL.Begin(PrimitiveType.LineLoop);
                        GL.Vertex3(pIni.x - tamTemp, pIni.y - tamTemp, pIni.z);
                        GL.Vertex3(pIni.x - tamTemp, pIni.y - tamTemp, pIni.z + tamTempZ);
                        GL.Vertex3(pIni.x + tamTemp, pIni.y - tamTemp, pIni.z + tamTempZ);
                        GL.Vertex3(pIni.x + tamTemp, pIni.y - tamTemp, pIni.z);
                        GL.End();

                        GL.Begin(PrimitiveType.LineLoop);
                        GL.Vertex3(pIni.x - tamTemp, pIni.y - tamTemp, pIni.z);
                        GL.Vertex3(pIni.x - tamTemp, pIni.y - tamTemp, pIni.z + tamTempZ);
                        GL.Vertex3(pIni.x - tamTemp, pIni.y + tamTemp, pIni.z + tamTempZ);
                        GL.Vertex3(pIni.x - tamTemp, pIni.y + tamTemp, pIni.z);
                        GL.End();

                        GL.Begin(PrimitiveType.LineLoop);
                        GL.Vertex3(pIni.x + tamTemp, pIni.y - tamTemp, pIni.z);
                        GL.Vertex3(pIni.x + tamTemp, pIni.y - tamTemp, pIni.z + tamTempZ);
                        GL.Vertex3(pIni.x + tamTemp, pIni.y + tamTemp, pIni.z + tamTempZ);
                        GL.Vertex3(pIni.x + tamTemp, pIni.y + tamTemp, pIni.z);
                        GL.End();    
                    };*/

                    /* if (Unifilar)
                     {
                         if (!base.Selecionado)
                           GL.Color4(Color.FromArgb(transp, Rgb[0], Rgb[1], Rgb[2]));

                         // pinta = false;
                         GL.Begin(PrimitiveType.Lines);
                         GL.Vertex3(pIni.x, pIni.y, pIni.z);
                         GL.Vertex3(pFin.x, pFin.y, pFin.z);
                         GL.End();
                     }
                     else
                     {


                         if (Selecionado)
                             GL.Color3(Color.Red);
                         else
                             if (arestas)
                                 GL.Color3(Color.Black);


                         GL.Color4(Color.FromArgb(transp, Rgb[0], Rgb[1], Rgb[2]));

                     }
                 }*/

                }

                catch (Exception ms)
                {
                    System.Windows.Forms.MessageBox.Show("erro ao desenhar apoio: " + ms.Message);
                }
            }
        }
        int i;
        [NonSerialized]
        vec3 n1, v1, v2, p1, p2, p3;
        public void Copy(TApoio obj)
        {
            base.Copy(obj);
            if ((Object)obj.Dados != null)
            {
                this.Dados = (TDadosApoio)obj.Dados.Clone();
                /*  if ((Object)Dados.Poligono != null)
                    foreach (TLinha lin in Dados.Poligono.linhas_poligonal)
                        lin.layer = this.layer;*/
            }

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
            TApoio l = new TApoio();
            l.Copy(this);
            return l;
        }

    }
}
