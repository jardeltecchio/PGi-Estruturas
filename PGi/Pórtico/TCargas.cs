using OpenTK;
using OpenTK.Graphics.OpenGL;
using OpenTK.Input;
using OpenTK.Platform;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Win32Interop.Enums;
using static Microsoft.WindowsAPICodePack.Shell.PropertySystem.SystemProperties.System;

namespace PG
{
    public enum TipoCasoCarga
    {
        [Description("Permanente")]
        Permanente = 1,
        [Description("Variável")]
        Variável = 2,
    }
    public enum SimNao
    {
        [Description("Sim")]
        Sim = 1,
        [Description("Não")]
        Não = 2,
    }
    [Serializable]
    public class TCasosCarga
    {
        public int ID;
        public string Nome;
        public string Descricao;
        public TipoCasoCarga Tipo;
        public Color Cor;
        public SimNao atuante;
        public TCasosCarga(int id, string nome, string descricao, TipoCasoCarga tipo, Color cor, SimNao _atuante)
        {
            this.ID = id;
            this.atuante = _atuante;
            this.Nome = nome;
            this.Descricao = descricao;
            this.Tipo = tipo;
            this.Cor = cor;
        }

        //[Category(), DisplayName("Id")] public int ID { get { return Id; } set { Id = value; } }
        [Category("Casos de carga"), DisplayName("Id")] public int ID_ { get { return ID; } }
        [Category("Casos de carga"), DisplayName("Nome")] public string NOME { get { return Nome; } set { Nome = value; } }
        [Category("Casos de carga"), DisplayName("Descrição")] public string DESC { get { return Descricao; } set { Descricao = value; } }
        [Category("Casos de carga"), DisplayName("Tipo")] public TipoCasoCarga TIPO { get { return Tipo; } set { Tipo = value; } }

        [Category("Casos de carga"), DisplayName("Cor")] public System.Drawing.Color COR { get { return Cor; } set { Cor = value; } }
        [Category("Casos de carga"), DisplayName("Atuante")] public SimNao ATUANTE { get { return atuante; } set { atuante = value; } }
    }
    [Serializable]
    public class CoeficientesCombinacao
    {
        public int caso { get; set; }
        public double coef { get; set; }
        public CoeficientesCombinacao(int idcaso, double _coef)
        {
            caso = idcaso;
            coef = _coef;
        }
    }
    [Serializable]
    public enum TipoEstadoLimite
    {
        [Description("ELU")]
        ELU = 1,
        [Description("ELS")]
        ELS = 2,
    }
    
    [Serializable]
    public enum CategoriaCombinacao
    {
        [Description("Linear")]
        Linear = 0,
        [Description("Estabilidade")]
        Estabilidade = 1,
    }
    [Serializable]
    public class TCombinacoes
    {
        public int Id;
        public string Descricao, Nome;
        public CategoriaCombinacao categoriaCombinacao; // 0 linear  1 estabildiade
        public List<CoeficientesCombinacao> Coeficientes;

        public TipoEstadoLimite EstadoLimite;


        public TCombinacoes(int id,string descricao, string nome, TipoEstadoLimite tipoEL, CategoriaCombinacao categoria)
        {
            this.Id = id;
            this.categoriaCombinacao = categoria;
            this.Descricao = descricao;

            this.Nome = nome;
            this.EstadoLimite = tipoEL;
            Coeficientes = new List<CoeficientesCombinacao>();
        }

        [Category("Combinação"), DisplayName("Nome")] public string NOME { get { return Nome; } set { Nome = value; } }
        [Category("Combinação"), DisplayName("Descrição")] public string DESC { get { return Descricao; } set { Descricao = value; } }
        [Category("Combinação"), DisplayName("Estado Limite")] public TipoEstadoLimite ESTADOLIMITE { get { return EstadoLimite; } set { EstadoLimite = value; } }

    }

    [Serializable]
    public class TCargaLinear : TObjetoDesenho
    {
        public double anguloRotacao;
        public int idBarra;
        public double valor;
        public TPonto ponto;
        public TDadosCarga Dados;
        public int ID;
        public double comprimento;

        [NonSerialized]
        OpenTK.Graphics.TextPrinter texto;
        [NonSerialized]
        Font fonte;
        public TCargaLinear()
        {
            base.Visivel = true;
            base.IdLayer = Lay.CargaLinear;
            this.Tipo = Const.ID_CARGA_LINEAR;
            NaoPermiteMoverOuCopiar = true;
        }

        public TCargaLinear(double v, TPonto p1, TLayer _layer = null)
        {
            this.valor = v;

            if (Geom.Iguais(p1.x, 0))
                p1.x = 0;

            if (Geom.Iguais(p1.y, 0))
                p1.y = 0;

            if (Geom.Iguais(p1.z, 0))
                p1.z = 0;

            this.ponto = p1;
            this.pIni = p1;
            this.pFin = p1;

            base.Selecionado = false;

            base.Tipo = Const.ID_CARGA_LINEAR;
            NaoPermiteMoverOuCopiar = true;
            this.layer = _layer;
        }
        public TCargaLinear(TPonto p1, TPonto p2, TDadosCarga dados,double angRotacao, int id_barra, TLayer _layer)
        {
            this.ponto = p1;
            base.pIni = new TPonto(p1.x, p1.y, p1.z);
            base.pFin = new TPonto(p2.x, p2.y, p2.z);
            ObjetoPrimario = true;
            this.anguloRotacao = angRotacao;
            this.idBarra = id_barra;
            this.Initialize(ref p1, dados as TDadosCarga, _layer);
            base.Selecionado = false;
            base.Visivel = true;
            base.Tipo = Const.ID_CARGA_LINEAR;
            this.layer = _layer;
            NaoPermiteMoverOuCopiar = true;
            OnMouseMove(ref p2,false);
        }

        public TCargaLinear(TPonto p1, TPonto p2, TLayer lay, TDadosBarra dados, int pav)
        {
            string ss = "";
            NaoPermiteMoverOuCopiar = false;
            List<TLinha> linhas = new List<TLinha>();
            this.Initialize(ref p1, ref ss, dados, lay, ref linhas, ref pav);
            pIni = (TPonto)p1.Clone();
            pFin = (TPonto)p2.Clone();

            this.Selecionado = false;
            base.Selecionado = false;
            base.Visivel = true;
            base.IdLayer = Lay.Barras;
            this.Tipo = Const.ID_BARRAGENERICA;
            this.angulo = (float)(FuncoesGerais.atand((p1.y - p2.y) / (p1.x - p2.x)));

        }

        /* public override void Mover(TPonto ponto1, TPonto ponto2)
         {
             pIni.primeiraVezMover = true;

             pIni.Mover(ponto1, ponto2);
             pFin.Mover(ponto1, ponto2);

             RotacionaDiagramaCarga(true,1,1,1);
         }*/

        public override string PrimeiroComando()
        {
            return Const.CMD_CARGA_LINEAR_1_P /*+ "("+this.Dados.valor+")"*/;
        }

        public override void Initialize(ref TPonto point, ISettings Dados, TLayer layer)
        {
            ponto = new TPonto(point.x, point.y, 0);
            base.pIni = new TPonto(point.x, point.y, point.z);
            base.pFin = new TPonto(point.x, point.y, point.z);
            texto = new OpenTK.Graphics.TextPrinter(OpenTK.Graphics.TextQuality.Low);
            fonte = new Font("Arial", 10);
            base.Selecionado = false;
            this.Selecionado = false;

            this.layer = layer;
            this.Dados = Dados as TDadosCarga;
            base.Tipo = Const.ID_CARGA_LINEAR;
            NaoPermiteMoverOuCopiar = true;

            base.Initialize(ref point, Dados, layer);
            ApagarSelecionados = false;
        }

        public override void Initialize(ref TPonto point, ref string command, ISettings Dados, TLayer layer, ref List<TLinha> Linhas, ref int pavimento)
        {
            ponto = new TPonto(point.x, point.y, 0);
            base.pIni = new TPonto(point.x, point.y, point.z);
            base.pFin = new TPonto(point.x, point.y, point.z);
            texto = new OpenTK.Graphics.TextPrinter(OpenTK.Graphics.TextQuality.Low);
            fonte = new Font("Arial", 10);
            base.Selecionado = false;
            this.Selecionado = false;
            NaoPermiteMoverOuCopiar = true;

            this.layer = layer;
            this.Dados = Dados as TDadosCarga;
            base.Tipo = Const.ID_CARGA_LINEAR;
            base.Initialize(ref point, ref command, Dados, layer, ref Linhas, ref pavimento);
            command = PrimeiroComando();
            ApagarSelecionados = false;
        }
        [NonSerialized]
        CoordenadaD[] coord = new CoordenadaD[5];
       
        int j;

   
        public override eObjetoDesenhoMouseDown OnMouseDown(ref TPonto point, ref string command, List<TLinha> Linhas, List<TPonto> Pontos, List<TPilar> Pilares, ref List<TObjetoDesenho> Objects, bool FromGrip = false)
        {
            try
            {
                base.pFin = point.Clone() as TPonto;
                
                return eObjetoDesenhoMouseDown.Done;
            }
            catch (Exception e)
            {
                System.Windows.Forms.MessageBox.Show(e.Message.ToString(), "Erro mousedown carga linear", System.Windows.Forms.MessageBoxButtons.OK, System.Windows.Forms.MessageBoxIcon.Error);
                return eObjetoDesenhoMouseDown.Continue;
            }
        }

        int i;
        void setLista(ref List<float> coord_objeto, double x, double y, double z, double r, double g, double b)
        {
            coord_objeto.Add((float)x);
            coord_objeto.Add((float)y);
            coord_objeto.Add((float)z);

            coord_objeto.Add((float)r);
            coord_objeto.Add((float)g);
            coord_objeto.Add((float)b);
        }
        [NonSerialized]
        double r, g, b;
        public int qtdSetas;
        public void Preenche_Arestas(ref List<float> coords_arestas)
        {
            if (Visivel /*&& Estrutura.barras[idBarra].Visivel*/)
            {
                if (Selecionado)
                {
                    r = Color.Aqua.R / 255;
                    g = Color.Aqua.G / 255;
                    b = Color.Aqua.B / 255;
                }
                else
                {
                    r = (double)Dados.cor.R / 255;
                    g = (double)Dados.cor.G / 255;
                    b = (double)Dados.cor.B / 255;
                }

                if (Dados.concentrada)
                {
                    LinhaVec3 lin;

                    lin = new LinhaVec3();
                    for (int j = 0; j < 7; j++)
                    {
                        if (j == 0) lin = setas[0].l1;
                        else if (j == 1) lin = setas[0].l2;
                        else if (j == 2) lin = setas[0].l3;
                        else if (j == 3) lin = setas[0].l4;
                        else if (j == 4) lin = setas[0].l5;
                        else if (j == 5) lin = setas[0].l6;
                        else if (j == 6) lin = setas[0].l_principal;

                        setLista(ref coords_arestas, lin.p1.x, lin.p1.y, lin.p1.z, r, g, b);
                        setLista(ref coords_arestas, lin.p2.x, lin.p2.y, lin.p2.z, r, g, b);
                    }                                      
                }
                else
                if (Dados.distribuida)
                {
                    LinhaVec3 lin;                  

                    for (int i = 0; i < setas.Count; i++)
                    {
                        lin = new LinhaVec3();
                        for (int j = 0; j < 7; j++)
                        {
                            if (j == 0) lin = setas[i].l1;
                            else if (j == 1) lin = setas[i].l2;
                            else if (j == 2)lin = setas[i].l3;
                            else if (j == 3)lin = setas[i].l4;
                            else if (j == 4)lin = setas[i].l5;
                            else if (j == 5)lin = setas[i].l6;
                            else if (j == 6)lin = setas[i].l_principal;

                            setLista(ref coords_arestas, lin.p1.x, lin.p1.y, lin.p1.z, r, g, b);
                            setLista(ref coords_arestas, lin.p2.x, lin.p2.y, lin.p2.z, r, g, b);
                        }
                    }

                    setLista(ref coords_arestas, setas[0].l_principal.p1.x, setas[0].l_principal.p1.y, setas[0].l_principal.p1.z, r, g, b);
                    setLista(ref coords_arestas, setas[setas.Count-1].l_principal.p1.x, setas[setas.Count - 1].l_principal.p1.y, setas[setas.Count - 1].l_principal.p1.z, r, g, b);
                }
            }
        }
        [NonSerialized]
        public vec3[] CoordsCarga;
        [NonSerialized]
        vec3 u1, u2, u, normxy, normxz;
        [NonSerialized] double NdotU, ndotu_mod, cos_alfa, angXY, angXZ, divisoes, divisoesFrac, xAnt;
        [NonSerialized]
        List<vec3> CoordsSubdvisao = new List<vec3>();
        [NonSerialized]
        double[] posicao = new double[4];
        [NonSerialized] double[] posicaoFinal = new double[4];
        [NonSerialized] double[] posicaoFinal2 = new double[4];
        [NonSerialized] double tx, ty, tz;
        [NonSerialized] bool zi_maior_que_zf, xi_igual_xf;
        [NonSerialized] vec3 pos;
        [NonSerialized] double cx, cy, L, cz, xi, yi, xf, yf, zi, zf;
        [NonSerialized] double tamSetaXHoriz = 2;
        [NonSerialized] double tamSetaVert = 1;

        void GiraCargaConformeAnguloAlfa(ref vec3 coord, string eixo)
        {
            vec3 vetorRotacao = new vec3(0);
            vec3 centroRotacao = new vec3(0);
            if (eixo == "Z")
            {
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
        void RodarSeta(ref vec3 coord, string eixo)
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
        [NonSerialized] double alturaCarga = -0.2;
        double  escala;


        /* struct seta
         {
             seta()
             {

             }
             public vec3 [] CoordsCarga = new vec3[qtd_CoordsSecao];
         }*/
        [NonSerialized]
        public List<Seta> setas;

        public void CriaSetas(bool unifilar, double alturaVert, double alturaHoriz, double  escala)
        {
            NaoPermiteMoverOuCopiar = true;
            this.escala = escala;
            double offset = 0;
            if (!unifilar)
            {
                double sinal = 0;
                if (Dados.DirecaoProjecao == 0)
                    offset = 0;
                else
                if (Dados.DirecaoProjecao == 1) //y
                {
                    sinal = (Dados.valor > 0 ? -1 : 1);
                    offset = (alturaHoriz*sinal) / 2;
                }
                else
                if (Dados.DirecaoProjecao == 2) // z
                {
                    sinal = (Dados.valor > 0 ? 1 : -1);
                    offset = ((alturaVert * (sinal)) / 2);
                }

                offset += (offset * 0.05);

                if (anguloRotacao != 0 && anguloRotacao != 180)
                   offset = 0;
            }

            alturaCarga = Dados.valor * escala;

            tamSetaVert = (Math.Abs( Dados.valor)) * escala * 0.05;
            tamSetaXHoriz = (Math.Abs(Dados.valor)) * escala * 0.04;

            int qtd_CoordsSecao = 1;
            
            if (Dados.distribuida)
              qtd_CoordsSecao = 22;
            if (Dados.concentrada)
              qtd_CoordsSecao = 6;

            posicao = new double[4];
            posicaoFinal = new double[4];
            posicaoFinal2 = new double[4];

            CoordsCarga = new vec3[qtd_CoordsSecao];
            setas = new List<Seta>();
            LinhaVec3 lin = new LinhaVec3();
            vec3 vetorDirecao;
            Seta saux;
            try
            {
                    xi = pIni.x;
                    xf = pFin.x;
                    yi = pIni.y;
                    yf = pFin.y;
                    zi = pIni.z;
                    zf = pFin.z;

                    comprimento = (Math.Sqrt(Math.Pow(xi - xf, 2) + Math.Pow(yi - yf, 2) + Math.Pow(zi - zf, 2)));

                    if (Dados.concentrada)
                    {
                        TPonto posicao = new TPonto(0);
                        #region x
                        if (Dados.DirecaoProjecao == 0) // x
                        {
                            if (Dados.ProjecaoGlobal == 1) // x local
                            {
                                if (Dados.posicaoRelativa) 
                                  setas.Add(Geom.CriarSetaEixoLocalX(alturaCarga, Dados.d * comprimento, pIni, pFin, anguloRotacao));
                                else 
                                  setas.Add(Geom.CriarSetaEixoLocalX(alturaCarga, Dados.d, pIni, pFin, anguloRotacao));
                            }
                            else
                            if (Dados.ProjecaoGlobal == 0) // x global
                            {
                                if (Dados.posicaoRelativa) saux = Geom.CriarSetaEixoLocalX(alturaCarga, Dados.d * comprimento, pIni, pFin, 0);
                                else saux = Geom.CriarSetaEixoLocalX(alturaCarga, Dados.d, pIni, pFin, anguloRotacao);

                                posicao.x = saux.l_principal.p1.x; posicao.y = saux.l_principal.p1.y; posicao.z = saux.l_principal.p1.z;
                                setas.Add(Geom.CriarSetaEixoGlobalX(alturaCarga, posicao));
                            }
                        }
                        #endregion 
                        else
                        #region z
                        if (Dados.DirecaoProjecao == 2) // z
                        {
                            if (Dados.ProjecaoGlobal == 1) // z local
                            {
                                if (Dados.posicaoRelativa)
                                    setas.Add(Geom.CriarSetaEixoLocalZ(alturaCarga, Dados.d * comprimento, pIni, pFin, anguloRotacao, offset));
                                else
                                    setas.Add(Geom.CriarSetaEixoLocalZ(alturaCarga, Dados.d, pIni, pFin, anguloRotacao, offset));
                            }
                            else
                            if (Dados.ProjecaoGlobal == 0) // z global
                            {
                                if (Dados.posicaoRelativa) saux = Geom.CriarSetaEixoLocalZ(alturaCarga, Dados.d * comprimento, pIni, pFin, 0);
                                else saux = Geom.CriarSetaEixoLocalZ(alturaCarga, Dados.d, pIni, pFin, anguloRotacao);

                                posicao.x = saux.l_principal.p1.x; posicao.y = saux.l_principal.p1.y; posicao.z = saux.l_principal.p1.z;
                                setas.Add(Geom.CriarSetaEixoGlobalZ(alturaCarga, posicao, offset));
                            }
                        }
                        #endregion 
                        else
                        #region y
                        if (Dados.DirecaoProjecao == 1) // y
                        {
                            if (Dados.ProjecaoGlobal == 1) // y local
                            {
                                if (Dados.posicaoRelativa)
                                    setas.Add(Geom.CriarSetaEixoLocalY(alturaCarga, Dados.d * comprimento, pIni, pFin, anguloRotacao));
                                else
                                    setas.Add(Geom.CriarSetaEixoLocalY(alturaCarga, Dados.d, pIni, pFin, anguloRotacao));
                            }
                            else
                            if (Dados.ProjecaoGlobal == 0) // y global
                            {
                                if (Dados.posicaoRelativa) saux = Geom.CriarSetaEixoLocalY(alturaCarga, Dados.d * comprimento, pIni, pFin, 0);
                                else saux = Geom.CriarSetaEixoLocalY(alturaCarga, Dados.d, pIni, pFin, anguloRotacao);

                                posicao.x = saux.l_principal.p1.x; posicao.y = saux.l_principal.p1.y; posicao.z = saux.l_principal.p1.z;
                                setas.Add(Geom.CriarSetaEixoGlobalY(alturaCarga, posicao));
                            }
                        }
                        #endregion
                    }
                    else
                    if (Dados.distribuida)
                    {
                        #region X
                        if (Dados.DirecaoProjecao == 0) // X
                        {
                            if (Dados.ProjecaoGlobal == 1) // y local
                            {
                                for (int i = 0; i < 10; i++)
                                    setas.Add(Geom.CriarSetaEixoLocalX(alturaCarga, (0.11111 * i) * comprimento, pIni, pFin, anguloRotacao));
                            }
                            else
                            if (Dados.ProjecaoGlobal == 0) // x global
                            {
                                setas.Add(Geom.CriarSetaEixoGlobalX(alturaCarga, pIni));
                                TPonto pp = (pIni + pFin) / 2;
                                setas.Add(Geom.CriarSetaEixoGlobalX(alturaCarga, pp));
                                TPonto pp2 = ((pIni + pp) / (2));
                                setas.Add(Geom.CriarSetaEixoGlobalX(alturaCarga, pp2));
                                TPonto pp3 = ((pIni + pp2) / (2));
                                setas.Add(Geom.CriarSetaEixoGlobalX(alturaCarga, pp3));
                                TPonto pp4 = ((pp2 + pp) / (2));
                                setas.Add(Geom.CriarSetaEixoGlobalX(alturaCarga, pp4));

                                pp2 = ((pFin + pp) / (2));
                                setas.Add(Geom.CriarSetaEixoGlobalX(alturaCarga, pp2));
                                pp3 = ((pFin + pp2) / (2));
                                setas.Add(Geom.CriarSetaEixoGlobalX(alturaCarga, pp3));
                                pp4 = ((pp2 + pp) / (2));
                                setas.Add(Geom.CriarSetaEixoGlobalX(alturaCarga, pp4));

                                setas.Add(Geom.CriarSetaEixoGlobalX(alturaCarga, pFin));
                            /*setas.Add(Geom.CriarSetaEixoGlobalX(alturaCarga, pIni));
                            setas.Add(Geom.CriarSetaEixoGlobalX(alturaCarga, (pIni + (pIni + pFin) / 2) / 2));
                            setas.Add(Geom.CriarSetaEixoGlobalX(alturaCarga, (pFin + (pFin + pIni) / 2) / 2));
                            setas.Add(Geom.CriarSetaEixoGlobalX(alturaCarga, (pIni + pFin) / 2));
                            setas.Add(Geom.CriarSetaEixoGlobalX(alturaCarga, pFin));*/
                        }
                        }
                        #endregion
                        #region z
                        if (Dados.DirecaoProjecao == 2) // z
                        {
                            if (Dados.ProjecaoGlobal == 1) // z local
                            {
                                for (int i = 0; i < 10; i++)
                                  setas.Add(Geom.CriarSetaEixoLocalZ(alturaCarga, (0.11111 * i) * comprimento, pIni, pFin, anguloRotacao, offset));
                            }
                            else
                            if (Dados.ProjecaoGlobal == 0) // z global
                            {
                                    setas.Add(Geom.CriarSetaEixoGlobalZ(alturaCarga, pIni));
                                    TPonto pp = (pIni + pFin) / 2;
                                    setas.Add(Geom.CriarSetaEixoGlobalZ(alturaCarga, pp));
                                    TPonto pp2 = ((pIni + pp) / (2));
                                    setas.Add(Geom.CriarSetaEixoGlobalZ(alturaCarga, pp2));
                                    TPonto pp3 = ((pIni + pp2) / (2));
                                    setas.Add(Geom.CriarSetaEixoGlobalZ(alturaCarga, pp3));
                                    TPonto pp4 = ((pp2 + pp) / (2));
                                    setas.Add(Geom.CriarSetaEixoGlobalZ(alturaCarga, pp4));

                                    pp2 = ((pFin + pp) / (2));
                                    setas.Add(Geom.CriarSetaEixoGlobalZ(alturaCarga, pp2));
                                    pp3 = ((pFin + pp2) / (2));
                                    setas.Add(Geom.CriarSetaEixoGlobalZ(alturaCarga, pp3));
                                    pp4 = ((pp2 + pp) / (2));
                                    setas.Add(Geom.CriarSetaEixoGlobalZ(alturaCarga, pp4));

                                    setas.Add(Geom.CriarSetaEixoGlobalZ(alturaCarga, pFin));
                            /* setas.Add(Geom.CriarSetaEixoGlobalZ(alturaCarga, pIni, offset));
                             setas.Add(Geom.CriarSetaEixoGlobalZ(alturaCarga, (pIni + (pIni + pFin) / 2)/2, offset));
                             setas.Add(Geom.CriarSetaEixoGlobalZ(alturaCarga, (pFin + (pFin + pIni) / 2) / 2, offset));
                             setas.Add(Geom.CriarSetaEixoGlobalZ(alturaCarga, (pIni + pFin) / 2, offset));
                             setas.Add(Geom.CriarSetaEixoGlobalZ(alturaCarga, pFin, offset));*/
                        }
                        }
                        #endregion
                        #region y
                        if (Dados.DirecaoProjecao == 1) // y
                        {
                            if (Dados.ProjecaoGlobal == 1) // y local
                            {
                                for (int i = 0; i < 10; i++)
                                    setas.Add(Geom.CriarSetaEixoLocalY(alturaCarga, (0.11111 * i) * comprimento, pIni, pFin, anguloRotacao));
                            }
                            else
                            if (Dados.ProjecaoGlobal == 0) // y global
                            {
                                setas.Add(Geom.CriarSetaEixoGlobalY(alturaCarga, pIni));
                                TPonto pp = (pIni + pFin) / 2;
                                setas.Add(Geom.CriarSetaEixoGlobalY(alturaCarga, pp));
                                TPonto pp2 = ((pIni + pp) / (2));
                                setas.Add(Geom.CriarSetaEixoGlobalY(alturaCarga, pp2));
                                TPonto pp3 = ((pIni + pp2) / (2));
                                setas.Add(Geom.CriarSetaEixoGlobalY(alturaCarga, pp3));
                                TPonto pp4 = ((pp2+ pp) / (2));
                                setas.Add(Geom.CriarSetaEixoGlobalY(alturaCarga, pp4));

                                pp2 = ((pFin + pp) / (2));
                                setas.Add(Geom.CriarSetaEixoGlobalY(alturaCarga, pp2));
                                pp3 = ((pFin + pp2) / (2));
                                setas.Add(Geom.CriarSetaEixoGlobalY(alturaCarga, pp3));
                                pp4 = ((pp2 + pp) / (2));
                                setas.Add(Geom.CriarSetaEixoGlobalY(alturaCarga, pp4));

                                setas.Add(Geom.CriarSetaEixoGlobalY(alturaCarga, pFin));
                         
                            /*setas.Add(Geom.CriarSetaEixoGlobalY(alturaCarga, pIni));

                             setas.Add(Geom.CriarSetaEixoGlobalY(alturaCarga, (pIni + (pIni + pFin) / 2) / 2));
                             setas.Add(Geom.CriarSetaEixoGlobalY(alturaCarga, (pFin + (pFin + pIni) / 2) / 2));
                             setas.Add(Geom.CriarSetaEixoGlobalY(alturaCarga, (pIni + pFin) / 2));
                             setas.Add(Geom.CriarSetaEixoGlobalY(alturaCarga, pFin));*/
                        }

                    }
                        #endregion
                    }

                    vetorDirecao = new vec3(setas[0].l_principal.p1.x - setas[0].l_principal.p2.x, setas[0].l_principal.p1.y - setas[0].l_principal.p2.y, setas[0].l_principal.p1.z - setas[0].l_principal.p2.z);

                    for (int i = 0; i < setas.Count; i++)
                    {
                        for (int j = 0; j < 5; j++)
                        {
                            if (j == 1) lin = setas[i].l1;
                            else
                            if (j == 2) lin = setas[i].l2;
                            else
                            if (j == 3) lin = setas[i].l3;
                            else
                            if (j == 4) lin = setas[i].l4;
                            else
                            if (j == 0) lin = setas[i].l_principal;

                            lin.p2.x += vetorDirecao.x;
                            lin.p2.y += vetorDirecao.y;
                            lin.p2.z += vetorDirecao.z;

                            lin.p1.x += vetorDirecao.x;
                            lin.p1.y += vetorDirecao.y;
                            lin.p1.z += vetorDirecao.z;
                        }
                    }
                    qtdSetas = setas.Count-1;
            }

            catch (Exception e)
            {
                System.Windows.Forms.MessageBox.Show(e.Message);
            }
        }

        [NonSerialized]
        vec3 n1 = new vec3(0, 0, 0), pi = new vec3(0, 0, 0), pf = new vec3(0, 0, 0), n2 = new vec3(0, 0, 0), n3 = new vec3(0, 0, 0), n4 = new vec3(0, 0, 0), nm = new vec3(0, 0, 0), nOffset;
        [NonSerialized]
        public System.Drawing.Point[] points = new System.Drawing.Point[4];
        public override void OnMouseMove(ref TPonto point, bool ShowInfo, bool orto = true, double angulo = 0, bool PontoInicial = false)
        {
            if (PontoInicial)
            {
                this.pIni.x = point.x;
                this.pIni.y = point.y;
            }
            else
            {
                this.pFin.x = point.x;
                this.pFin.y = point.y;
            }

            point.Snap = false;
            base.anguloGlobal = (float)RMath.rad2deg(pIni.getAngleTo(pFin));
            base.angulo = ((float)(FuncoesGerais.atand((pIni.y - pFin.y) / (pIni.x - pFin.x))));

            pi.x = pIni.x;
            pi.y = pIni.y;

            n1.x = pi.x + 1;
            n1.y = pi.y;
            n1 = n1.Rotate(pi, anguloGlobal * Const.PIDiv180);
            n1 = n1.Rotate(pi, 90 * Const.PIDiv180);

            n2.x = pi.x + 1;
            n2.y = pi.y;
            n2 = n2.Rotate(pi, anguloGlobal * Const.PIDiv180);
            n2 = n2.Rotate(pi, -90 * Const.PIDiv180);

            pf.x = pFin.x;
            pf.y = pFin.y;
            
            n3.x = pf.x + 1;
            n3.y = pf.y;
            n3 = n3.Rotate(pf, anguloGlobal * Const.PIDiv180);
            n3 = n3.Rotate(pf, 90 * Const.PIDiv180);

            n4.x = pf.x + 1;
            n4.y = pf.y;
            n4 = n4.Rotate(pf, anguloGlobal * Const.PIDiv180);
            n4 = n4.Rotate(pf, -90 * Const.PIDiv180);

            base.OnMouseMove(ref point, ShowInfo);
        }
        double z_pixel1,z_pixel2;


        public override bool Selecionar(float clicx, float clicy, float coordx, float coordy, TObjetoDesenho Owner = null)
        {
            if (!this.Visivel)
                return false;

            if (Dados.idCaso == 1)
               return false;
           
            double x1 = 0, y1 = 0, x2 = 0, y2 = 0;

            LinhaVec3 lin;
            Vector3d p1, p2;
            for (int i = 0; i < setas.Count; i++)
            {
                lin = new LinhaVec3();
                for (int j = 0; j < 7; j++)
                {
                    if (j == 0) lin = setas[i].l1;
                    else if (j == 1) lin = setas[i].l2;
                    else if (j == 2) lin = setas[i].l3;
                    else if (j == 3) lin = setas[i].l4;
                    else if (j == 4) lin = setas[i].l5;
                    else if (j == 5) lin = setas[i].l6;
                    else if (j == 6) lin = setas[i].l_principal;

                 //   FPrincipal.pixel1(ref lin.p1.x, ref lin.p1.y, ref lin.p1.z);
                //    FPrincipal.pixel2(ref lin.p2.x, ref lin.p2.y, ref lin.p2.z);

                    p1 = new Vector3d(lin.p1.x, lin.p1.y, lin.p1.z);
                    p2 = new Vector3d(lin.p2.x, lin.p2.y, lin.p2.z);

                    if (!Clipper3D.ProjetarLinhaSelecao(FPrincipal.View_x_Proj, FPrincipal.cameraPerspectiva, p1, p2, out x1, out y1, out x2, out y2))
                        continue;
                    if (Geom.PontoEmLinha2(clicx, clicy, x1, y1, x2, y2, 0.3))
                    {
                        SetaSelecao(true, true);
                        return base.Selecionado;
                    }

                   /* if (Geom.PontoEmLinha2(clicx, clicy, FPrincipal.px_x1[0], FPrincipal.px_y1[0], FPrincipal.px_x2[0], FPrincipal.px_y2[0], 0.4))
                    {
                        SetaSelecao(true, true);
                        return base.Selecionado;
                    }*/
                }
            }

            p1 = new Vector3d(setas[0].l_principal.p1.x, setas[0].l_principal.p1.y, setas[0].l_principal.p1.z);
            p2 = new Vector3d(setas[setas.Count - 1].l_principal.p1.x, setas[setas.Count - 1].l_principal.p1.y, setas[setas.Count - 1].l_principal.p1.z);

            if (!Clipper3D.ProjetarLinhaSelecao(FPrincipal.View_x_Proj, FPrincipal.cameraPerspectiva, p1, p2, out x1, out y1, out x2, out y2))
                return false;
            if (Geom.PontoEmLinha2(clicx, clicy, x1, y1, x2, y2, 0.3))
            {
                SetaSelecao(true, true);
                return base.Selecionado;
            }

            /*FPrincipal.pixel1(ref setas[0].l_principal.p1.x, ref setas[0].l_principal.p1.y, ref setas[0].l_principal.p1.z);
            FPrincipal.pixel2(ref setas[setas.Count-1].l_principal.p1.x, ref setas[setas.Count - 1].l_principal.p1.y, ref setas[setas.Count - 1].l_principal.p1.z);
            if (Geom.PontoEmLinha2(clicx, clicy, FPrincipal.px_x1[0], FPrincipal.px_y1[0], FPrincipal.px_x2[0], FPrincipal.px_y2[0], 0.4))
            {
                SetaSelecao(true, true);
                return base.Selecionado;
            }*/


            /////////////////
            ///



          /*  FPrincipal.pixel1(ref CoordsCarga[1].x, ref CoordsCarga[1].y, ref CoordsCarga[1].z);

          //  if (z_clip < 1 && z_clip > 0)
           // {
                FPrincipal.pixel2(ref CoordsCarga[2].x, ref CoordsCarga[2].y, ref CoordsCarga[2].z);
                
          /*      bool ForaDaTela = ((Desenho.px_x1[0] < 0) && (Desenho.px_x2[0] < 0)) ||
                                  ((Desenho.px_x1[0] > Desenho.w) && (Desenho.px_x2[0] > Desenho.w)) ||
                                  ((Desenho.px_y1[0] < 0) && (Desenho.px_y2[0] < 0)) ||
                                  ((Desenho.px_y1[0] > Desenho.h) && (Desenho.px_y2[0] > Desenho.h));

                if (ForaDaTela)
                    return false;*/
                
             //   if (z_clip < 1 && z_clip > 0)
      /*            if (Geom.PontoEmLinha2(clicx, clicy, FPrincipal.px_x1[0], FPrincipal.px_y1[0], FPrincipal.px_x2[0], FPrincipal.px_y2[0], 0.5))
                  {
                    SetaSelecao(true, true);
                    return base.Selecionado;
                  }

            FPrincipal.pixel1(ref CoordsCarga[4].x, ref CoordsCarga[4].y, ref CoordsCarga[4].z);
            FPrincipal.pixel2(ref CoordsCarga[5].x, ref CoordsCarga[5].y, ref CoordsCarga[5].z);
            if (Geom.PontoEmLinha2(clicx, clicy, FPrincipal.px_x1[0], FPrincipal.px_y1[0], FPrincipal.px_x2[0], FPrincipal.px_y2[0], 0.5))
            {
                SetaSelecao(true, true);
                return base.Selecionado;
            }

            FPrincipal.pixel1(ref CoordsCarga[0].x, ref CoordsCarga[0].y, ref CoordsCarga[0].z);
            FPrincipal.pixel2(ref CoordsCarga[1].x, ref CoordsCarga[1].y, ref CoordsCarga[1].z);
            if (Geom.PontoEmLinha2(clicx, clicy, FPrincipal.px_x1[0], FPrincipal.px_y1[0], FPrincipal.px_x2[0], FPrincipal.px_y2[0], 0.5))
            {
                SetaSelecao(true, true);
                return base.Selecionado;
            }

            FPrincipal.pixel1(ref CoordsCarga[2].x, ref CoordsCarga[2].y, ref CoordsCarga[2].z);
            FPrincipal.pixel2(ref CoordsCarga[3].x, ref CoordsCarga[3].y, ref CoordsCarga[3].z);
            if (Geom.PontoEmLinha2(clicx, clicy, FPrincipal.px_x1[0], FPrincipal.px_y1[0], FPrincipal.px_x2[0], FPrincipal.px_y2[0], 0.5))
            {
                SetaSelecao(true, true);
                return base.Selecionado;
            }*/

            return false;
        }

        public bool TestaSelecao(float clicx, float clicy)
        {
            bool esta_no_intervalo = false;

            return esta_no_intervalo;
        }

        public override void SetaSelecao(bool s, bool MostraGrip, bool SelecaoDeCandidato = false, TObjetoDesenho Owner = null)
        {
            try
            {
                if (CoordsCarga == null)
                    return;

                base.Selecionado = false;

                if (!this.Visivel)
                    return;

                if (Dados.idCaso == 1)
                    return;
                LinhaVec3 lin = new LinhaVec3(); 
             
                bool ForaDaTela;

                for (int i = 0; i < setas.Count; i++)
                {
                        lin = setas[i].l_principal;

                        FPrincipal.pixel1(ref lin.p1.x, ref lin.p1.y, ref lin.p1.z, ref z_pixel1);
                        FPrincipal.pixel2(ref lin.p2.x, ref lin.p2.y, ref lin.p2.z, ref z_pixel2);

                        ForaDaTela = ((FPrincipal.px_x1[0] < 0) && (FPrincipal.px_x2[0] < 0)) ||
                                          ((FPrincipal.px_x1[0] > FPrincipal.w) && (FPrincipal.px_x2[0] > FPrincipal.w)) ||
                                          ((FPrincipal.px_y1[0] < 0) && (FPrincipal.px_y2[0] < 0)) ||
                                          ((FPrincipal.px_y1[0] > FPrincipal.h) && (FPrincipal.px_y2[0] > FPrincipal.h));
                        if (ForaDaTela)
                            continue;

                        ForaDaTela = !(z_pixel1 < 1 && z_pixel1 > 0 && z_pixel2 < 1 && z_pixel2 > 0);

                        if (ForaDaTela)
                            continue;
                    
                    Soma_Z = z_pixel1 + z_pixel2;
                }

                base.Selecionado = s;
            }
            catch(Exception e)
            {

            }
        }

        public override void ShowHideGrips(bool visivel)
        {
            if (Grips != null)
                foreach (TGrip grip in Grips)
                    grip.Visivel = visivel;
        }
        public TPonto getMiddlePoint()
        {
            return (pIni + pFin) / 2;
        }
        public void UpdatePixel()
        {
            pIni.px_x = FPrincipal.pixelX(pIni.x);
            pIni.px_y = FPrincipal.pixelY(pIni.y);

            pFin.px_x = FPrincipal.pixelX(pFin.x);
            pFin.px_y = FPrincipal.pixelY(pFin.y);
        }
        public override void AddGrips()
        {
            Grips = new List<TGrip>(2);

            Grips.Add(new TGrip(this, this.pIni.x, this.pIni.y, false, false, false, true, this.layer));

      //      Grips.Add(new TGrip(this, this.getMiddlePoint().x, this.getMiddlePoint().y, true, false, false, false, this.layer));

            Grips.Add(new TGrip(this, this.pFin.x, this.pFin.y, false, false, false, true, this.layer));
        }
        public override TObjetoDesenho Clone()
        {
            TCargaLinear l = new TCargaLinear();
            l.Copy(this);
            return l;
        }
        public void Copy(TCargaLinear obj)
        {
            base.Copy(obj);
            angulo = obj.angulo;
            valor = obj.valor;
            comprimento = obj.comprimento;
            NaoPermiteMoverOuCopiar = true;
            anguloGlobal = obj.anguloGlobal;
            anguloRotacao = obj.anguloRotacao;
            Selecionado = obj.Selecionado;
            if ((TDadosCarga)obj.Dados != null)
                Dados = (TDadosCarga)obj.Dados.Clone();
            base.Selecionado = false;
            if ((Object)obj.pIni != null)
                pIni = obj.pIni.Clone() as TPonto;

            if ((Object)obj.pFin != null)
                pFin = obj.pFin.Clone() as TPonto;
        //    CoordsCarga = new 
            layer = obj.layer;

        }

    }

    [Serializable]
    public class TCargaPontual: TObjetoDesenho
    {
        public TPonto ponto;
        public int idPonto;
        public TDadosCarga Dados;
        public double anguloRotacao;
        public bool apagarAposCalculo;
        [NonSerialized]
        OpenTK.Graphics.TextPrinter texto;
        [NonSerialized]
        Font fonte;
        public TCargaPontual()
        {
            base.Visivel = true;
            base.IdLayer = Lay.CargaPontual;
            this.Tipo    = Const.ID_CARGA_PONTUAL;
            
            ObjetoPrimario = true;
            NaoPermiteMoverOuCopiar = false;
        }
        [NonSerialized]
        double  escala;
        public TCargaPontual(TDadosCarga dados, TPonto p1, TLayer lay)
        {
            this.ponto = (TPonto)p1.Clone();
            base.Selecionado = false;

            base.Tipo = Const.ID_CARGA_PONTUAL;
            this.layer = lay;
            NaoPermiteMoverOuCopiar = true;
            ObjetoPrimario = true;
            this.Initialize(ref p1, dados as TDadosCarga, lay);
        }

        public override string PrimeiroComando()
        {
            return  Const.CMD_CARGA_PONTUAL_1_P;
        }

        public override void Initialize(ref TPonto point, ISettings Dados, TLayer layer)
        {
            ponto = new TPonto(point.x, point.y, point.z);
            base.pIni = new TPonto(point.x, point.y, point.z);
            this.layer = layer;
            this.Dados = Dados as TDadosCarga;
            base.Tipo = Const.ID_CARGA_PONTUAL;
            ObjetoPrimario = true;
            base.Selecionado = false;
            this.Selecionado = false;
            NaoPermiteMoverOuCopiar = true;
            base.Initialize(ref point, Dados, layer);
        }

        public override void Initialize(ref TPonto point, ref string command, ISettings Dados, TLayer layer, ref List<TLinha> Linhas, ref int pavimento)
        {
            ObjetoPrimario = true;
            ponto = new TPonto(point.x, point.y, point.z);
            base.pIni = new TPonto(point.x, point.y, point.z);
            this.layer = layer;
            this.Dados = Dados as TDadosCarga;
            base.Tipo  = Const.ID_CARGA_PONTUAL;
            command    = Const.CMD_CARGA_PONTUAL_1_P;
            base.Selecionado = false;
            this.Selecionado = false;
            NaoPermiteMoverOuCopiar = true;
            base.Initialize(ref point, ref command, Dados, layer, ref Linhas, ref pavimento);
        }
        [NonSerialized] public vec3[] CoordsCarga;

        [NonSerialized] double alturaCarga = -0.2;
        [NonSerialized]
        vec3 u1, u2, u, normxy, normxz;
        double NdotU, ndotu_mod, cos_alfa, angXY, angXZ, divisoes, divisoesFrac, xAnt;

        List<vec3> CoordsSubdvisao = new List<vec3>();
        [NonSerialized]
        double[] posicao = new double[4];
        double[] posicaoFinal = new double[4];
        double[] posicaoFinal2 = new double[4];
        double[] posicaoFinal_Trans = new double[5];
        [NonSerialized]
        double tx, ty, tz;
        bool zi_maior_que_zf, xi_igual_xf;
        vec3 pos;
        double cx, cy, L, cz, xi, yi, xf, yf, zi, zf;
        [NonSerialized]
        double tamSetaXHoriz = 0.008;
        double tamSetaVert = 0.02;

        [NonSerialized]
        public List<Seta> setas;
        [NonSerialized]
        public List<SetaMomento> setasMomento;
        public vec3 p1_vetor;
        public void CriaSeta(bool unifilar, double escala)
        {
            // tamSetaVert = Math.Abs(Dados.valor) * 1;
            // tamSetaXHoriz = Math.Abs(Dados.valor) * 1;
            NaoPermiteMoverOuCopiar = true;
            this.escala = escala;
           // CoordsSecao_f.Clear();
            int qtd_CoordsSecao = 1;

            if (Dados.distribuida)
                qtd_CoordsSecao = 18;
            if (Dados.concentrada)
                qtd_CoordsSecao = 6;
            vec3 centroRotacao = new vec3(0);
            vec3 vetorRotacao = new vec3(0);
            posicaoFinal = new double[4];
            posicaoFinal2 = new double[4];
            posicaoFinal_Trans = new double[5];
            p1_vetor = new vec3(pIni.x,pIni.y, pIni.z);
            CoordsCarga = new vec3[qtd_CoordsSecao];
            alturaCarga += (alturaCarga * 0.0005);
            tamSetaVert = (Math.Abs(Dados.valor)) * escala * 0.4;
            tamSetaXHoriz = (Math.Abs(Dados.valor)) * escala * 0.07;
            //pos = new vec3(xi > xf ? xAnt - div : xAnt + div, yi, zi);
            //  escala *= 30; 
            LinhaVec3 lin = new LinhaVec3();
            alturaCarga = Dados.valor * escala;
            try
            {
                if (Dados.tipoForca)
                {
                    setas = new List<Seta>();
                    #region x
                    if (Dados.DirecaoProjecao == 0) // x
                    {
                       setas.Add(Geom.CriarSetaEixoGlobalX(alturaCarga, pIni));
                    }
                    #endregion
                    else
                        #region z
                    if (Dados.DirecaoProjecao == 2) // z
                    {
                        setas.Add(Geom.CriarSetaEixoGlobalZ(alturaCarga, pIni));
                    }
                    #endregion
                    else
                    #region y
                    if (Dados.DirecaoProjecao == 1) // y
                    {
                        setas.Add(Geom.CriarSetaEixoGlobalY(alturaCarga, pIni));
                    }

                    vec3 vetorDirecao = new vec3(setas[0].l_principal.p1.x - setas[0].l_principal.p2.x, setas[0].l_principal.p1.y - setas[0].l_principal.p2.y, setas[0].l_principal.p1.z - setas[0].l_principal.p2.z);

                    for (int i = 0; i < setas.Count; i++)
                    {
                        for (int j = 0; j < 5; j++)
                        {
                            if (j == 1) lin = setas[i].l1;
                            else
                            if (j == 2) lin = setas[i].l2;
                            else
                            if (j == 3) lin = setas[i].l3;
                            else
                            if (j == 4) lin = setas[i].l4;
                            else
                            if (j == 0) lin = setas[i].l_principal;

                            lin.p2.x += vetorDirecao.x;
                            lin.p2.y += vetorDirecao.y;
                            lin.p2.z += vetorDirecao.z;

                            lin.p1.x += vetorDirecao.x;
                            lin.p1.y += vetorDirecao.y;
                            lin.p1.z += vetorDirecao.z;
                        }
                    }

                    p1_vetor = setas[0].l_principal.p1;
                    #endregion                    
                }
                else
                if (Dados.tipoMomento)
                {
                    setasMomento = new List<SetaMomento>();
                    #region x
                    if (Dados.DirecaoProjecao == 0) // x
                    {
                        setasMomento.Add(Geom.CriarSetaEixoGlobalX_DUPLA(alturaCarga, pIni, Dados.valor / Math.Abs(Dados.valor)));
                    }
                    #endregion
                    else
                    #region z
                    if (Dados.DirecaoProjecao == 2) // z
                    {
                        setas.Add(Geom.CriarSetaEixoGlobalZ(alturaCarga, pIni));
                    }
                    #endregion
                    else
                    #region y
                    if (Dados.DirecaoProjecao == 1) // y
                    {
                        setas.Add(Geom.CriarSetaEixoGlobalY(alturaCarga, pIni));
                    }
                    #endregion

                    vec3 vetorDirecao = new vec3(setasMomento[0].l_principal.p1.x - setasMomento[0].l_principal.p2.x, setasMomento[0].l_principal.p1.y - setasMomento[0].l_principal.p2.y, setasMomento[0].l_principal.p1.z - setasMomento[0].l_principal.p2.z);

                    for (int i = 0; i < setasMomento.Count; i++)
                    {
                        for (int j = 0; j < 9; j++)
                        {
                            if (j == 1) lin = setasMomento[i].l1;
                            else
                            if (j == 2) lin = setasMomento[i].l2;
                            else
                            if (j == 3) lin = setasMomento[i].l3;
                            else
                            if (j == 4) lin = setasMomento[i].l4;

                            else
                            if (j == 5) lin = setasMomento[i].l7;
                            else
                            if (j == 6) lin = setasMomento[i].l8;
                            else
                            if (j == 7) lin = setasMomento[i].l9;
                            else
                            if (j == 8) lin = setasMomento[i].l10;
                            else
                            if (j == 0) lin = setasMomento[i].l_principal;

                            lin.p2.x += vetorDirecao.x;
                            lin.p2.y += vetorDirecao.y;
                            lin.p2.z += vetorDirecao.z;

                            lin.p1.x += vetorDirecao.x;
                            lin.p1.y += vetorDirecao.y;
                            lin.p1.z += vetorDirecao.z;
                        }
                    }


                    p1_vetor = setasMomento[0].l_principal.p1;
                }
            }
            catch (Exception e)
            {
                System.Windows.Forms.MessageBox.Show(e.Message);
            }
        }

        public override eObjetoDesenhoMouseDown OnMouseDown(ref TPonto point, ref string command, List<TLinha> Linhas, List<TPonto> Pontos, List<TPilar> Pilares, ref List<TObjetoDesenho> Objects, bool FromGrip = false)
        {
            try
            {
                ponto = point.Clone() as TPonto;
                pIni = point.Clone() as TPonto;
               
              //  AddGrips();
              //  AddCirculo();

                return eObjetoDesenhoMouseDown.Done;
            }
            catch (Exception e)
            {
                System.Windows.Forms.MessageBox.Show(e.Message.ToString(), "Erro", System.Windows.Forms.MessageBoxButtons.OK, System.Windows.Forms.MessageBoxIcon.Error);
                return eObjetoDesenhoMouseDown.Continue;
            }
        }

        [NonSerialized]
        public Pen mPen = new Pen(Color.Magenta);
        [NonSerialized]
        public float[] dashValues = { 8, 10, 11 };

        void setLista(ref List<float> coord_objeto, double x, double y, double z, double r, double g, double b)
        {
            coord_objeto.Add((float)x);
            coord_objeto.Add((float)y);
            coord_objeto.Add((float)z);

            coord_objeto.Add((float)r);
            coord_objeto.Add((float)g);
            coord_objeto.Add((float)b);
        }
        [NonSerialized]
        double r, g, b;

        public void Preenche_Arestas(ref List<float> coords_arestas)
        {
            if (Visivel)
            {
                if (Selecionado)
                {
                    r = Color.Aqua.R / 255;
                    g = Color.Aqua.G / 255;
                    b = Color.Aqua.B / 255;
                }
                else
                {
                    r = (double)Dados.cor.R / 255;
                    g = (double)Dados.cor.G / 255;
                    b = (double)Dados.cor.B / 255;
                }

                if (Dados.tipoForca)
                {
                    LinhaVec3 lin;

                    lin = new LinhaVec3();
                    for (int j = 0; j < 7; j++)
                    {
                        if (j == 0) lin = setas[0].l1;
                        else if (j == 1) lin = setas[0].l2;
                        else if (j == 2) lin = setas[0].l3;
                        else if (j == 3) lin = setas[0].l4;
                        else if (j == 4) lin = setas[0].l5;
                        else if (j == 5) lin = setas[0].l6;
                        else if (j == 6) lin = setas[0].l_principal;

                        setLista(ref coords_arestas, lin.p1.x, lin.p1.y, lin.p1.z, r, g, b);
                        setLista(ref coords_arestas, lin.p2.x, lin.p2.y, lin.p2.z, r, g, b);
                    }
                }
                else
                if (Dados.tipoMomento)
                {
                    LinhaVec3 lin;

                    lin = new LinhaVec3();
                    for (int j = 0; j < 13; j++)
                    {
                        if (j == 0) lin = setasMomento[0].l1;
                        else if (j == 1) lin = setasMomento[0].l2;
                        else if (j == 2) lin = setasMomento[0].l3;
                        else if (j == 3) lin = setasMomento[0].l4;
                        else if (j == 4) lin = setasMomento[0].l5;
                        else if (j == 5) lin = setasMomento[0].l6;
                        else if (j == 6) lin = setasMomento[0].l_principal;
                        else if (j == 7) lin = setasMomento[0].l7;
                        else if (j == 8) lin = setasMomento[0].l8;
                        else if (j == 9) lin = setasMomento[0].l9;
                        else if (j == 10) lin = setasMomento[0].l10;
                        else if (j == 11) lin = setasMomento[0].l11;
                        else if (j == 12) lin = setasMomento[0].l12;

                        setLista(ref coords_arestas, lin.p1.x, lin.p1.y, lin.p1.z, r, g, b);
                        setLista(ref coords_arestas, lin.p2.x, lin.p2.y, lin.p2.z, r, g, b);
                    }
                }
            }
        }

        public override void Mover(ref TPonto ponto1, ref TPonto ponto2, bool dinamico)
        {

           // pIni.Mover(ref ponto1, ref ponto2, dinamico); 
         //   ponto.Mover(ref ponto1, ref ponto2, dinamico);

        }
        public override void OnMouseMove(ref TPonto point, bool ShowInfo, bool orto = true, double angulo = 0, bool PontoInicial = false)
        {
            this.ponto.x = point.x;
            this.ponto.y = point.y;
            this.ponto.z = point.z;

            this.pIni.x = point.x;
            this.pIni.y = point.y;
            this.pIni.z = point.z;


            point.Snap = false;

            base.OnMouseMove(ref point, ShowInfo);
        }
        [NonSerialized] double z_pixel1;     
        [NonSerialized] double z_pixel2;
        public override bool Selecionar(float clicx, float clicy, float coordx, float coordy, TObjetoDesenho Owner = null)
        {
            if (!this.Visivel)
                return false ;

            if (Dados.idCaso == 1)
                return false;
            Vector3d p1, p2;
            double x1 = 0, y1 = 0, x2 = 0, y2 = 0;

            LinhaVec3 lin;
            if (Dados.tipoForca)
            {
                for (int i = 0; i < setas.Count; i++)
                {
                    lin = new LinhaVec3();
                    for (int j = 0; j < 6; j++)
                    {
                        if (j == 0) lin = setas[i].l1;
                        else if (j == 1) lin = setas[i].l2;
                        else if (j == 2) lin = setas[i].l3;
                        else if (j == 3) lin = setas[i].l4;
                        else if (j == 4) lin = setas[i].l5;
                        // else if (j == 5) lin = setas[i].l6;
                        else if (j == 5) lin = setas[i].l_principal;

                      /*  FPrincipal.pixel1(ref lin.p1.x, ref lin.p1.y, ref lin.p1.z);
                        FPrincipal.pixel2(ref lin.p2.x, ref lin.p2.y, ref lin.p2.z);
                        if (Geom.PontoEmLinha2(clicx, clicy, FPrincipal.px_x1[0], FPrincipal.px_y1[0], FPrincipal.px_x2[0], FPrincipal.px_y2[0], 0.4))
                        {
                            SetaSelecao(true, true);
                            return base.Selecionado;
                        }*/

                        p1 = new Vector3d(lin.p1.x, lin.p1.y, lin.p1.z);
                        p2 = new Vector3d(lin.p2.x, lin.p2.y, lin.p2.z);

                        if (!Clipper3D.ProjetarLinhaSelecao(FPrincipal.View_x_Proj, FPrincipal.cameraPerspectiva, p1, p2, out x1, out y1, out x2, out y2))
                            continue;
                        if (Geom.PontoEmLinha2(clicx, clicy, x1, y1, x2, y2, 0.3))
                        {
                            SetaSelecao(true, true);
                            return base.Selecionado;
                        }

                    }
                }
            }

            return false;
        }

        public bool TestaSelecao(float clicx, float clicy)
        {
            bool esta_no_intervalo = false;

            return esta_no_intervalo;
        }

        public override void SetaSelecao(bool s, bool MostraGrip, bool SelecaoDeCandidato=false, TObjetoDesenho Owner = null)
        {
            base.Selecionado = false;

            if (!this.Visivel)
                return;
            if (Dados.idCaso == 1)
                return;
            LinhaVec3 lin = new LinhaVec3();

            bool ForaDaTela;
            if (Dados.tipoForca && setas != null)
            {
                for (int i = 0; i < setas.Count; i++)
                {
                    lin = setas[i].l_principal;

                    FPrincipal.pixel1(ref lin.p1.x, ref lin.p1.y, ref lin.p1.z, ref z_pixel1);
                    FPrincipal.pixel2(ref lin.p2.x, ref lin.p2.y, ref lin.p2.z, ref z_pixel2);

                    ForaDaTela = ((FPrincipal.px_x1[0] < 0) && (FPrincipal.px_x2[0] < 0)) ||
                                      ((FPrincipal.px_x1[0] > FPrincipal.w) && (FPrincipal.px_x2[0] > FPrincipal.w)) ||
                                      ((FPrincipal.px_y1[0] < 0) && (FPrincipal.px_y2[0] < 0)) ||
                                      ((FPrincipal.px_y1[0] > FPrincipal.h) && (FPrincipal.px_y2[0] > FPrincipal.h));
                    if (ForaDaTela)
                        continue;

                    ForaDaTela = !(z_pixel1 < 1 && z_pixel1 > 0 && z_pixel2 < 1 && z_pixel2 > 0);

                    if (ForaDaTela)
                        continue;

                    Soma_Z = z_pixel1 + z_pixel2;
                }
            }

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

            Grips.Add(new TGrip(this, this.ponto.x, this.ponto.y, true, false, false, false, this.layer));
        }  
        public override TObjetoDesenho Clone()
        {
            TCargaPontual l = new TCargaPontual();
            l.Copy(this);
            return l;
        }

        public void Copy(TCargaPontual obj)
        {
            base.Copy(obj);
            layer = obj.layer;
            anguloRotacao =  obj.anguloRotacao;
            idPonto = obj.idPonto;
            NaoPermiteMoverOuCopiar = true;
            if ((Object)obj.pIni != null)
                pIni = obj.pIni.Clone() as TPonto;

            if ((Object)obj.ponto != null)
              ponto  = obj.ponto.Clone() as TPonto;
            if ((Object)obj.Dados != null)
                Dados = (TDadosCarga)obj.Dados.Clone();
        }

    }
}
