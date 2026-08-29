using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PG
{
    namespace ConfiguracoesCaptura
    {
        public static class Captura
        {
            public static List<float> OutrosAngulos;
            public static bool Ponto = true;
            public static bool PontoMedio = true;
            public static bool Interesecao = true;
            public static bool Perpendicular = true;
            public static bool MaisProximo = true;
            public static bool OrtogonalLigado = true;
            public static double DivisoesCota = 0.1;
            public static int unidadeCota = 1;


        }
    }

    [Serializable]

    public struct CfgTela
    {
        public double precisaoPixel;
        public double[] ponto_zero;
        public double fatorZoom;
    }

    [Serializable]
    public class TConfiguracoesVisualizacao
    {
        public byte[] Cor_Apoio = new byte[3] { 0, 0, 0 }; /*RGB*/
        public TConfiguracoesVisualizacao()
        {

        }
    }

    [Serializable]
    public struct SF3DOpcoesVisualizacao
    {
        public byte Transparencia;
        public Color CorFundoCima, CorFundoBaixo;
        public bool EixosCentrais, UsarPlanoFundoGradiente, MostrarNumeroElementos, MostrarDescricaoElementos, MostrarEixosLocais, OpenGlSuavizacao, Arestas, ArestasConformeObjetos, Nos, Unifilar, Perspectiva, CorPorTipoDeElemento, CorPorSecao, CorPorMaterial, MostrarTexturas, MostrarChao;
        public byte[] RgbArestas;
        public double tamanhoNo;
        public Color CorNo, CorDescElementos;
        public int TamApoios, TamArt;
        public SF3DOpcoesVisualizacao(byte transp = 255)
        {
            UsarPlanoFundoGradiente = false;
            Transparencia = 255;
            tamanhoNo = 3;
            MostrarChao = false;
            Perspectiva = true;
            Unifilar = false;
            MostrarEixosLocais = false;
            EixosCentrais = false;
            this.RgbArestas = new byte[3];
            CorFundoBaixo = Color.FromArgb(64, 64, 64);
            CorFundoCima = Color.FromArgb(64, 64, 64);
            CorNo = Color.Blue;
            CorDescElementos = Color.Purple;
            MostrarDescricaoElementos = false;
            MostrarNumeroElementos = false;
            RgbArestas[0] = 0;
            RgbArestas[1] = 0;
            RgbArestas[2] = 0;
            TamApoios = 30;//30 cm
            TamArt = 8;// 8 cm de diametro
            CorPorTipoDeElemento = false;
            CorPorMaterial = false;
            CorPorSecao = true;
            Nos = true;
            Arestas = true;
            ArestasConformeObjetos = false;
            MostrarTexturas = false;
            OpenGlSuavizacao = false;
        }
    }

    [Serializable]
    public class TConfiguracoesPGi
    {
        public double precisaoPixel, divisaoCotas;
        public double[] ponto_zero;
        public double fatorZoom, CotaFundacao;
        public string NomeProjeto, DescricaoProjeto;
        public bool snap_PontoFinal, snap_PontoMedio, snap_Intersecao, snap_Perpendicular;
        public SGrelhaOpcoesVisualizacao GrelhaOpcoesVisualizacao; 
        public SPorticoOpcoesVisualizacao PorticoOpcoesVisualizacao;
        public SDiagramaOpcoesVisualizacao DiagramaOpcoesVisualizacao;
        public int unidadeDivisaoCotas;
        public Color CorCota;
        public SF3DOpcoesVisualizacao F3DOpcoesVisualizacao;
        public SConfiguracoesProjeto  CfgProjeto;
        public List<TLayer> layers; // essa lista de layer serve só para guardar informções dos layers, como cor e etc, nao os objetos dos layers
        public Dictionary<string, TLayer> LayersByIdPrincipal; 
        public void CreateStandardLayers()
        {
            this.LayersByIdPrincipal = new Dictionary<string, TLayer>();
            this.layers = new List<TLayer>();
           
            cl = -1;

            CriaLayer(Lay.Zero,true, false, new byte[3] { 255, 0, 0 }, 0, false);
            CriaLayer(Lay.ElementosBasicos, true, false, new byte[3] { 214, 214, 214 }, 0, false);
            CriaLayer(Lay.Vigas, true, false, new byte[3] { 62, 64, 250}, 0, false);
            CriaLayer(Lay.Pilares, true, false, new byte[3] { 0, 0, 70 }, 0, false);
            CriaLayer(Lay.Lajes, true, false, new byte[3] { 169,169,169 }, 0, false);
            CriaLayer(Lay.Barras, true, false, new byte[3] { 0, 71, 170 }, 0, false);
            CriaLayer(Lay.GrelhaLajes, false, true, new byte[3] { 112, 128, 144 }, 0, false);
            CriaLayer(Lay.TextosVigas, true, false, new byte[3] { 65, 127, 255 }, 0, false);
            CriaLayer(Lay.TextosLajes, true, false, new byte[3] { 64, 128, 128 }, 0, false);
            CriaLayer(Lay.TextosPilares, true, false, new byte[3] { 160, 160, 160 }, 0, false);
            CriaLayer(Lay.CargaPontual, true, false, new byte[3] { 0, 255, 0 }, 0, false);
            CriaLayer(Lay.CargaLinear, true, false, new byte[3] { 153, 50, 204 }, 0, false);
        }

        int cl;
        public void CriaLayer(string Nome, bool ligado, bool congelado, byte[] rgb, int tipolinha, bool travado)
        {
            cl++;
            layers.Add(new TLayer(Nome, ligado, congelado, rgb, 0, travado, GrupoLay.Principal));

            LayersByIdPrincipal[Nome] = layers[cl];
        }

        public TConfiguracoesPGi()
        {
            GrelhaOpcoesVisualizacao = new SGrelhaOpcoesVisualizacao(true);
            PorticoOpcoesVisualizacao = new SPorticoOpcoesVisualizacao(true);
            F3DOpcoesVisualizacao = new SF3DOpcoesVisualizacao(0);
            CfgProjeto               = new SConfiguracoesProjeto(true);
            DiagramaOpcoesVisualizacao = new SDiagramaOpcoesVisualizacao(true);
         //   CreateStandardLayers();
            snap_Intersecao = true;
            snap_PontoFinal = true;
            snap_PontoMedio = true;
            snap_Perpendicular = false;
            unidadeDivisaoCotas = 1; // cm
            CorCota = Color.Green;
            divisaoCotas = 0.1;
            NomeProjeto = "PGi - [Não salvo]";
            DescricaoProjeto = "";
        }
    }

    public class TConfiguracoesPrograma
    {
        public bool SuavizacaoOpenGl, AbrirUltimoProjeto, IrParaPaginaResultados, FecharJanelaResultadosAposCalculo;
        public TConfiguracoesPrograma()
        {
            SuavizacaoOpenGl = false;
            IrParaPaginaResultados = true;
            FecharJanelaResultadosAposCalculo = true;
            AbrirUltimoProjeto = true;
        }


    }

}
