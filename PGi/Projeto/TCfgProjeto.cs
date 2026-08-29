using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Linq;
using System.Net.NetworkInformation;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;
using System.Windows.Media;
using static PG.FCalculaSecao;

namespace PG
{
    [Serializable]
    public class PontosDeslocar_x_VetorDirecao
    {
        public vec3 p;
        public vec3 vetor;
        public double angulo, fator;
    }
    
    [Serializable]
    public class cotasSecao
    {
        public bool igualOutraCota, desconsiderarNaMalha,cotaDiametro, diametroInterno;
        public int cotaIgualar1, cotaIgualar2, cotaIgualar3, cotaIgualar4;
        public List<int> cotasIgualar;
        public vec3 p1, p2;
        public bool cresceDoisSentidos;
        public List<PontosDeslocar_x_VetorDirecao> pontosDeslocar;

    }

    [Serializable]
    public class raiosSecao
    {
        public vec3 p1;
        public List<int> raios_igualar;
        public double xCota, yCota;
        public double xr1, yr1;
        public double xr2, yr2;
        public bool igualOutroRaio;
        public int raioIgualar1, raioIgualar2, raioIgualar3, raioIgualar4;
        public List<vec3> pontosRaio;
        public int outro_raio_multiplicador, id_barra1, id_barra2;
        public bool raioFuncaoDeOutroRaio, raio_dependente;
        public double ang_inicio, ang_fim, raio, multiplicador;

    }
    [Serializable]
    public class TInfoSecao
    {
        public List<cotasSecao> cotas;
        public List<raiosSecao> raios;
        public List<TPonto> nos_raios;

        public TInfoSecao(List<cotasSecao> _cotas, List<raiosSecao> _raios, List<TPonto> _nos_raios)
        {
            cotas = _cotas;
            raios = _raios;
            nos_raios = _nos_raios;
        }
    }

    public interface IConfiguracoes
    {
        IConfiguracoes Clone();
        void Copy(IConfiguracoes obj);
    }
    [Serializable]
    public class TArquivo
    {
        public TConfiguracoesPGi cfg;
        public List<TBarraGenerica> barras;
        public List<TCasosCarga> casos;
        public List<TCargaLinear> cargalinear;
        public List<TCargaPontual> cargapontual;
        public List<TApoio> apoios;
        public TEstrutura estrutura;
        public List<TSecao> secoes;
        public List<TCombinacoes> combinacoes;
      //  public string caminho;

        public TArquivo(List<TApoio> _apoios, List<TSecao> _secoes, TEstrutura _estrutura, List<TCargaPontual> _cargapontual,
            List<TCargaLinear> _cargalinear, List<TCasosCarga> _casos, List<TCombinacoes> _combinacoes, List<TBarraGenerica> _barras, TConfiguracoesPGi _cfg)
        {
            this.apoios = _apoios;
            this.secoes = _secoes;   
            cargalinear = _cargalinear; 
            cargapontual = _cargapontual;
            this.barras = _barras;  
            estrutura = _estrutura;
            cfg = _cfg; 
            casos = _casos;
            combinacoes = _combinacoes;
        }
    }

    [Serializable]
    public class SConfiguracoesProjeto
    {
        public List<SClasses> classes;
        public SMateriais material;
        public List<TMateriais> materiais;
        public TUnidadesProjeto unidadesProjeto;
        public SSistema sistema;
        public SConfiguracaoPortico portico;
        public SConfiguracoesProjeto(bool padrao = true)
        {
            double[] FckPadrao = new double[] { 200, 250, 300, 350, 400, 450, 500 };
            string[] Nomes = new string[] { "C-20", "C-25", "C-30", "C-35", "C-40", "C-45", "C-50" };
            
            classes = new List<SClasses>();
            for (int i = 0; i < 7; i++)
              classes.Add(new SClasses(i, Nomes[i], FckPadrao[i], 0));
            unidadesProjeto = new TUnidadesProjeto();
            material = new SMateriais(true);
            materiais = new List<TMateriais>();
            sistema = new SSistema(true);
            portico = new SConfiguracaoPortico(true);
            
            CriarMaterialPadrao();

        }
        /*                Cfg.materiais.Add(new TMateriais(id_, Nome.Text,
                    System.Convert.ToDouble(edE.Text),
                    System.Convert.ToDouble(edG.Text),
                    System.Convert.ToDouble(edPoisson.Text),
                    System.Convert.ToDouble(edPesoEsp.Text),
                    System.Convert.ToDouble(edCoefTermico.Text),
                     btCor.BackColor,
                     cbMaterial.SelectedIndex,
                     cbAco.SelectedIndex,
                     cbConcreto.SelectedIndex,
                     cbCoefMinoracao.SelectedIndex,
                     cbAgregado.SelectedIndex,
                     cbMaterial.SelectedIndex == 0 ? 0 : System.Convert.ToDouble(edFck.Text),
                     cbMaterial.SelectedIndex == 0 ? 0 : System.Convert.ToDouble(edFcd.Text)));*/

        void CriarMaterialPadrao()
        {
            materiais.Add(new TMateriais(1, "C-20", 25043.96, 21287.37, 8514.95, 0.2, 2500, 0.00005,
                System.Drawing.Color.Gray, 1, -1,  0, 1, 0, 200, 142.86,0,0,0));

            materiais.Add(new TMateriais(1, "C-25", 28000, 24150.00, 9660.00, 0.2, 2500, 0.00005,
                System.Drawing.Color.Gray, 1, -1, 0, 1, 0, 250, 178.57, 0, 0, 0));

            materiais.Add(new TMateriais(1, "C-30", 30672.46, 26838.41, 10735.36, 0.2, 2500, 0.00005,
                System.Drawing.Color.Gray, 1, -1, 0, 1, 0, 300, 214.28, 0, 0, 0));

            materiais.Add(new TMateriais(2, "A36", 200000, 0, 79000, 0.3, 7850, 0.00012,
                System.Drawing.Color.Gray, 0, 0, -1, 0, 0, 0, 0,250,400,0));

            materiais.Add(new TMateriais(3, "A572 Gr42", 200000, 0, 80000, 0.3, 7850, 0.00012,
                System.Drawing.Color.Gray, 0, 1, -1, 0, 0, 0, 0, 290, 413, 0));

            materiais.Add(new TMateriais(4, "A572 Gr45", 200000, 0, 80000, 0.3, 7850, 0.00012,
               System.Drawing.Color.Gray, 0, 2, -1, 0, 0, 0, 0, 310, 415, 0));

            materiais.Add(new TMateriais(5, "A572 Gr50", 200000, 0, 80000, 0.3, 7850, 0.00012,
                System.Drawing.Color.Gray, 0, 3, -1, 0, 0, 0, 0, 345, 450, 0));
            //    materiais[materiais.Count - 1].fu = 400;
            //       materiais[materiais.Count - 1].fy = 250;

        }
    }

    [Serializable]
    public class SConfiguracaoPortico
    {
        public int tambarrapilar, tambarraviga, qtdbarrapilar, qtdbarrasPortico;
        public bool usatampilar, usatamviga, usaqtdpilar, usaqtdvigas, refinar;
        public bool calcularEsforcos;
        public SConfiguracaoPortico(bool padrao = true)
        {
            usatampilar = true;
            usatamviga = true;
            usaqtdpilar = false;
            usaqtdvigas = false;
            tambarrapilar = 80;
            tambarraviga  = 80;
            qtdbarrapilar = 6;
            qtdbarrasPortico  = 6;
            calcularEsforcos = true;
            refinar = false;
        }
    }

    [Serializable]
    public class SSistema
    {
        public bool Solver_cholesky_supernodal, Solver_gradiente_conjugado, Solver_choleskypadrao,UsarDll,
            Interromper_calculo_elementos_sobrepostos,Interromper_calculo_conexao_perdida;
        public int toleranciaConexaoPerdida;
        public SSistema(bool padrao = true)
        {
            Solver_cholesky_supernodal = true;
            Solver_choleskypadrao = false;
            Solver_gradiente_conjugado = false;
            UsarDll = false;
            Interromper_calculo_conexao_perdida = true;
            toleranciaConexaoPerdida = 20;
            Interromper_calculo_elementos_sobrepostos = true;
        }
    }

[Serializable]
    public class SConfiguracaoGrelha
    {
        public int AnguloBarras, EspacamentoX, EspacamentoY;
        public bool GerarNovaGrelha, ConsiderarGrelhaEditada;
        public SConfiguracaoGrelha(bool padrao = true)
        {
            AnguloBarras = 0;
            EspacamentoX = 35;
            EspacamentoY = 35;
            GerarNovaGrelha = true;
            ConsiderarGrelhaEditada = false;
        }

        public SConfiguracaoGrelha(int angulo, int espx, int espy, bool gerarnova, bool consideraredicao)
        {
            AnguloBarras = angulo;
            EspacamentoX = espx;
            EspacamentoY = espy;
            GerarNovaGrelha = gerarnova;
            ConsiderarGrelhaEditada = consideraredicao;
        }

        public SConfiguracaoGrelha Clone()
        {
            SConfiguracaoGrelha s = new SConfiguracaoGrelha(this.AnguloBarras, this.EspacamentoX, this.EspacamentoY, this.GerarNovaGrelha, this.ConsiderarGrelhaEditada);
            return s;
        }
    }

    [Serializable]
    public class SClasses
    {
        public double pesoEspecifico;
        public double fck, fcd, poisson, eci, ecs, gc;
        public string tipoAgregado, nome;
        public int classe, gamaC;
        public bool ConformeFck;

        void CalculaModulos(double fck, int tipoAgregado, ref double eci, ref double ecs, ref double gc)
        {
            double gama1, coefAgregado;

            /*   fck em kgf/cm²   */

            /*
            granito
            calcário
            arenito
            basalto
            */
            fck /= 10;
            coefAgregado = 0;
            ecs = 0;
            switch (tipoAgregado)
            {
                case 0: coefAgregado = 1; break;
                case 1: coefAgregado = 0.9; break;
                case 2: coefAgregado = 0.7; break;
                case 3: coefAgregado = 1.2; break;
            };

            eci = coefAgregado * 5600 * Math.Sqrt(fck);
            gama1 = 0.8 + (0.2 * (fck / 80));

            ecs = (gama1 * eci);
            gc = 0.4 * ecs;
        }

        public SClasses(int _classe, string _nome, double _fck, int agregado)
        {
            classe = _classe;
            tipoAgregado = "granito";
            nome = _nome;
            pesoEspecifico = 2500;
            fck = _fck;
            fcd = fck / 1.4;

            gamaC = 1;
            poisson = 0.2;
            ConformeFck = true;
            ecs = 0; eci = 0; gc = 0;
            CalculaModulos(fck, 0, ref eci, ref ecs, ref gc);
        }
    }

    [Serializable]
    public class TMateriais
    {
        public int Id { get; set; }
        public string Descricao { get; set; }
        public double E { get; set; }
        public double Ecs { get; set; }
        public double G { get; set; }
        public double V { get; set; }
        public double PesoEspecifico { get; set; }
        public double CoefTermico { get; set; }

        public string Cor { get; set; }

        public System.Drawing.Color Cor_;

        public int Tipo, indiceTipo;
        public int tipoAco, tipoConcreto, coefMin, Agregado;
        public double fck, fcd, fu,fy;
        public TMateriais(int id, 
            string descricao, double e, double ecs,
            double g, double v,
            double pesoesp, 
            double coeftermico, System.Drawing.Color cor, int tipo,
            int tipoaco, int tipoconcreto, int coefmin, int agregado,
            double _fck, double _fcd, double _fy,double _fu, int indice)
        {
            this.Id = id;
            this.Descricao = descricao;
            this.G = g;
            this.E = e;
            this.Ecs = ecs;
            this.V = v;
            this.Cor_ = cor;
            this.CoefTermico = coeftermico;
            this.PesoEspecifico = pesoesp;
            this.Tipo = tipo;
            this.tipoAco = tipoaco;
            this.tipoConcreto = tipoconcreto;
            this.coefMin = coefmin;
            this.Agregado = agregado;
            this.fcd = _fcd;
            this.fck = _fck;
            this.fu = _fu;
            this.fy = _fy;
            this.indiceTipo = indice;
        }
    }

    [Serializable]
    public class SMateriais
    {
        public List<TPavimento> pavs;
        public bool AplicarEmTodos;

        public SMateriais(bool padrao = true)
        {
            pavs = new List<TPavimento>();
            AplicarEmTodos = true;
        }
    }

    [Serializable]
    public class TUnidadesGeometria: IConfiguracoes
    {
        public int casas;
        public IConfiguracoes Clone()
        {
            TUnidadesGeometria l = new TUnidadesGeometria();
            l.Copy(this);
            return l;
        }
        public void Copy(IConfiguracoes obj)
        {
       
            casas = (obj as TUnidadesGeometria).casas;
        }
    }

    [Serializable]
    public class TUnidadesProjeto : IConfiguracoes
    {
        public int res_casas, geo_casas, sec_dim_casas, sec_prop_casas;
        public string res_un_comprimento, res_un_deformacao,geo_un_comprimento,sec_dimensoes, sec_propriedades;
        public string res_un_forca;
        public string res_un_tensao;


        public TUnidadesProjeto()
        {
            res_casas = 2;
            geo_casas = 2;
            sec_dim_casas = 2;
            sec_prop_casas = 2;

            geo_un_comprimento = und.m;

            res_un_comprimento = und.m;
            res_un_deformacao  = und.mm;
            res_un_forca = und.kgf;
            res_un_tensao = und.kgfcm2;

            sec_dimensoes    = und.mm;
            sec_propriedades = und.mm;
        }

        public IConfiguracoes Clone()
        {
            TUnidadesProjeto l = new TUnidadesProjeto();
            l.Copy(this);
            return l;
        }
        public void Copy(IConfiguracoes obj)
        {
            res_casas = (obj as TUnidadesProjeto).res_casas;
            geo_casas = (obj as TUnidadesProjeto).geo_casas;
            res_un_comprimento = (obj as TUnidadesProjeto).res_un_comprimento;
            res_un_deformacao = (obj as TUnidadesProjeto).res_un_deformacao;
            geo_un_comprimento = (obj as TUnidadesProjeto).geo_un_comprimento;
            res_un_forca = (obj as TUnidadesProjeto).res_un_forca;
            res_un_tensao = (obj as TUnidadesProjeto).res_un_tensao;

        }
    }
}
