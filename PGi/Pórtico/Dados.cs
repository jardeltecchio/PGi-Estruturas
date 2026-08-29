using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Drawing;
using System.Drawing.Drawing2D;

namespace PG
{
    [Serializable]

    /*Esses são os dados que devem ser carregados nas telas de dados dos elementos, ex: TDadosViga*/
    /*por isso que o comprimento da viga não vai aqui, poios nao se carrega isso na tela de dados*/
    public class TDadosCarga: ISettings
    {
        public double valor, d;
        public int ProjecaoGlobal, DirecaoProjecao /*0,1,2 -> x,y,z*/;
        public bool distribuida, concentrada, posicaoRelativa;
        public int idCaso;
        public bool tipoForca, tipoMomento;
        public Color cor;
        public TDadosCarga() { }
        public TDadosCarga(double val, double _d, int dp, int pg, bool _distribuida, bool _concentrada, int idcaso, Color _cor, bool posRel, bool forca, bool momento)
        {
            this.valor = val;
            this.d = _d;
            this.DirecaoProjecao = dp;
            this.ProjecaoGlobal = pg;
            this.distribuida = _distribuida;
            this.concentrada = _concentrada;
            this.posicaoRelativa = posRel;
            this.idCaso = idcaso;
            this.cor = _cor;
            this.tipoForca = forca;
            this.tipoMomento = momento;
        }

        public ISettings Clone()
        {
            TDadosCarga l = new TDadosCarga();
            l.Copy(this);
            return l;
        }

        public void Copy(TDadosCarga dad)
        {
            this.valor = dad.valor;
            this.ProjecaoGlobal = dad.ProjecaoGlobal;
            this.DirecaoProjecao = dad.DirecaoProjecao;
            this.concentrada = dad.concentrada;
            this.distribuida = dad.distribuida;
            this.d = dad.d;
            this.cor = dad.cor;
            this.idCaso = dad.idCaso;
            this.tipoForca = dad.tipoForca;
            this.tipoMomento = dad.tipoMomento;

            this.posicaoRelativa = dad.posicaoRelativa;
        }
    }

    [Serializable]
    public class TDadosViga : ISettings
    {
        public TDadosViga(){}
        public TDadosViga(double b1, double b2, double h1, double h2, double exce, 
            int indicetipo, TPoligono poligono, 
            string nome, int numero, bool ins_face1, 
            bool ins_face2, bool ins_eixo, 
            int _redTorcao, double cargaparede, 
            double cargaextra)
        {
            this.indiceTipo = indicetipo;
            this.excentricidade = exce;
            this.b1 = b1;
            this.b2 = b2;
            this.h1 = h1;
            this.h2 = h2;
            this.redTorcao = _redTorcao;
            this.Poligono  = poligono;
            this.nome = nome;
            this.numero    = numero;
            this.CargaParede = cargaparede;
            this.CargaExtra = cargaextra;
           // this.classeViga = 
            if (ins_eixo)
                this.face_insercao = 1;
            else
            if (ins_face1)
                this.face_insercao = 0;
            else
            if (ins_face2)
               this.face_insercao = 2;
        }

        public ISettings Clone()
        {
            TDadosViga l = new TDadosViga();
            l.Copy(this);
            return l;
        }

        public void Copy(TDadosViga dad)
        {
            this.classeViga = dad.classeViga;
            this.face_insercao = dad.face_insercao;
            this.excentricidade = dad.excentricidade;
            this.b1 = dad.b1;
            this.h1 = dad.h1;
            this.h2 = dad.h2;
            this.b2 = dad.b2;

            this.RigidezEI = dad.RigidezEI;
            this.RigidezGJ = dad.RigidezGJ;
            this.NaoUsarPP = dad.NaoUsarPP;
            this.CargaExtra  = dad.CargaExtra;
            this.CargaParede = dad.CargaParede;
            this.redTorcao = dad.redTorcao;
            this.ins_face1 = dad.ins_face1;
            this.ins_face2 = dad.ins_face2;
            this.ins_eixo = dad.ins_eixo;
            this.nome = dad.nome;
            this.numero = dad.numero;
            if (dad.Poligono != null)
                this.Poligono = (TPoligono)dad.Poligono.Clone(); //new TPoligono(dados.Poligono.TipoSecao, dados.h1.ToString(),dados.b1.ToString(),dados.h2.ToString(),dados.b2.ToString(),"","",false);
            this.indiceTipo = dad.indiceTipo;
        }
        public SClasses classeViga;
        public int face_insercao;
        public double excentricidade, b1, h1, h2, b2, CargaParede, CargaExtra, RigidezEI, RigidezGJ;
        public bool ins_face1, ins_face2, ins_eixo, NaoUsarPP;
        public string nome;
        public int numero, redTorcao;
        [NonSerialized]
        public TPoligono Poligono;

        /* 
     indiceTipo:

    Retangular    0
    Seção T       1
    Seção I       2
    Seção L       3

    */
        public int indiceTipo;
    }

    [Serializable]
    public class TDadosPilar : ISettings
    {
     //   public double h1 { get; set; }
        public double excentricidade,h1, b1, h2,b2;
        public double altura;
        public string nome;
        public int numero;
        [NonSerialized]
        public TPoligono Poligono;
        public bool vazada;
        public TDadosPilar()
        {

        }

        /*funcoes que retornam as principais dimensoes em funcao do comprimentos das arestas*/
        public double H1
        {
            get
            {
                if (Poligono.TipoSecao == "Retangular")
                    return Math.Round(Poligono.linhas_poligonal[1].comprimento);

                if (Poligono.TipoSecao == "T")
                    return Math.Round(Poligono.linhas_poligonal[1].comprimento + Poligono.linhas_poligonal[3].comprimento);
        
                if (Poligono.TipoSecao == "I")
                    return Math.Round(Poligono.linhas_poligonal[1].comprimento + Poligono.linhas_poligonal[3].comprimento + Poligono.linhas_poligonal[5].comprimento);

                if (Poligono.TipoSecao == "L")
                    return Math.Round(Poligono.linhas_poligonal[5].comprimento);
                  
                if (Poligono.TipoSecao == "Circular")
                    return Math.Round( Geom.Comprimento(Poligono.linhas_poligonal[0].pIni, Poligono.linhas_poligonal[9].pFin));

                return 0;
            }
            set
            {
                h1 = value;
            }
        }

        public double B1
        {
            get
            {
                if (Poligono.TipoSecao == "Retangular")
                    return Math.Round(Poligono.linhas_poligonal[0].comprimento);

                if (Poligono.TipoSecao == "T")
                    return Math.Round(Poligono.linhas_poligonal[4].comprimento);

                if (Poligono.TipoSecao == "I")
                    return Math.Round(Poligono.linhas_poligonal[0].comprimento);

                if (Poligono.TipoSecao == "L")
                    return Math.Round(Poligono.linhas_poligonal[0].comprimento);

                if (Poligono.TipoSecao == "Circular")
                    return Math.Round(Geom.Comprimento(Poligono.linhas_poligonal[0].pIni, Poligono.linhas_poligonal[9].pFin));

                return 0;
            }
            set
            {
                b1 = value;
            }
        }
        public double H2
        {
            get
            {
                if (Poligono.TipoSecao == "Retangular")
                    return 0;

                if (Poligono.TipoSecao == "T")
                    return Math.Round(Poligono.linhas_poligonal[3].comprimento);

                if (Poligono.TipoSecao == "I")
                    return Math.Round(Poligono.linhas_poligonal[5].comprimento);

                if (Poligono.TipoSecao == "L")
                    return Math.Round(Poligono.linhas_poligonal[1].comprimento);

                if (Poligono.TipoSecao == "Circular")
                    return 0;
                return 0;
            }
            set
            {
                h1 = value;
            }
        }

        public double B2
        {
            get
            {
                if (Poligono.TipoSecao == "Retangular")
                    return 0;

                if (Poligono.TipoSecao == "T")
                    return Math.Round(Poligono.linhas_poligonal[0].comprimento);

                if (Poligono.TipoSecao == "I")
                    return Math.Round(Poligono.linhas_poligonal[0].comprimento - (Poligono.linhas_poligonal[2].comprimento*2));

                if (Poligono.TipoSecao == "L")
                    return Math.Round(Poligono.linhas_poligonal[4].comprimento);

                if (Poligono.TipoSecao == "Circular")
                    return 0;
                return 0;
            }
            set
            {
                h1 = value;
            }
        }
        public ISettings Clone()
        {
            TDadosPilar l = new TDadosPilar();
            l.Copy(this);
            return l;
        }

        public void Copy(TDadosPilar dados)
        {
            excentricidade = dados.excentricidade;
            b1 = dados.b1;
            h1 = dados.h1;
            h2 = dados.h2;
            b2 = dados.b2;
            altura = dados.altura;
            nome = dados.nome;
            numero = dados.numero;
            if (dados.Poligono != null)
                Poligono = (TPoligono)dados.Poligono.Clone(); //new TPoligono(dados.Poligono.TipoSecao, dados.h1.ToString(), dados.b1.ToString(), dados.h2.ToString(), dados.b2.ToString(), "", "", false);
            indiceTipo = dados.indiceTipo;
        }
     
        public TDadosPilar(double _altura, double h1, double b1, double h2, double b2, int indicetipo, double rev, string nome, int numero, TPoligono poligono)
        {
            this.indiceTipo = indicetipo;
            this.excentricidade = rev;
            this.b1 = b1;
            this.b2 = b2;
            this.H1 = h1;
            this.h2 = h2;
            this.Poligono = poligono;
            this.nome = nome;
            this.numero = numero;
            this.altura = _altura;
        }
        /* 
       indiceTipo:
Retangular
T
I
L
Circular
      */
        public int indiceTipo;
    }
    [Serializable]
    public class TDadosApoio : ISettings
    {
        public TDadosApoio() { }
        public TDadosApoio(int indicetipo, double ang)
        {
            this.Tipo = indicetipo;
            this.anguloRotacao = ang;
        }

        public ISettings Clone()
        {
            TDadosApoio l = new TDadosApoio();
            l.Copy(this);
            return l;
        }

        public void Copy(TDadosApoio dados)
        {
            Tipo = dados.Tipo;
            anguloRotacao = dados.anguloRotacao;
            restringe_rx = dados.restringe_rx;
            restringe_ry = dados.restringe_ry;
            restringe_rz = dados.restringe_rz;
            restringe_dx = dados.restringe_dx;
            restringe_dy = dados.restringe_dy;
            restringe_dz = dados.restringe_dz;
            mola_dx = dados.mola_dx;
            mola_dy = dados.mola_dy;
            mola_dz = dados.mola_dz;
            mola_rx = dados.mola_rx;
            mola_ry = dados.mola_ry;
            mola_rz =  dados.mola_rz;
        }
        public double anguloRotacao;
        public bool restringe_rx, restringe_ry, restringe_rz, restringe_dx, restringe_dy, restringe_dz;
        public double mola_dx, mola_dy, mola_dz;
        public double mola_rx, mola_ry, mola_rz;

        public int Tipo; // 1 apoio simples, 2 engaste
    }

    [Serializable]
    public class TDadosBarra : ISettings
    {
        public TDadosBarra() 
        {
            Articulacao_my = 0;
            Articulacao_mz = 0;
            ez = 0;
            ey = 0;
            ex_f = 0;
            ex_i = 0;
        }
        public TDadosBarra(int numero, int indicetipo, TSecao _secao, TSecao secaosemrotacao, int _idsecao, double ang)
        {
            this.numero = numero;
            this.Tipo = indicetipo;
            this.secao = _secao;
            this.anguloRotacao = ang;
            this.idsecao = _idsecao;
            this.secaoSemRotacao = secaosemrotacao;
        }

        public ISettings Clone()
        {
            TDadosBarra l = new TDadosBarra();
            l.Copy(this);
            return l;
        }

        public void Copy(TDadosBarra dados)
        {
            numero = dados.numero;
            Tipo = dados.Tipo;
            idsecao = dados.idsecao;
            secao = dados.secao;
            secaoSemRotacao = dados.secaoSemRotacao;
            anguloRotacao = dados.anguloRotacao;

            Articulacao_mz =  dados.Articulacao_mz;
            Articulacao_my = dados.Articulacao_my;
            k_my = dados.k_my;  
            k_mz = dados.k_mz;  
            ez = dados.ez;
            ey = dados.ey;
            ex_i = dados.ex_i;
            ex_f = dados.ex_f;
        }

        public int Articulacao_my;
        public int Articulacao_mz;
        public int numero, idsecao;
        public TSecao secao, secaoSemRotacao;
        public double anguloRotacao, ey, ez, ex_i,ex_f, k_my, k_mz;
        /* 
      indiceTipo:

     Retangular    0
     Seção T       1
     Seção T inv.  2
     Seção I       3
     Seção L       4
     Seção L inv.  5
     */
        public int Tipo;
    }
    class LigacaoRotacional
    {
        public bool SemiRigidaY;
        public bool SemiRigidaZ;

        public double KRy;
        public double KRz;
    }

    [Serializable]
    public class TDadosLaje : ISettings
    {
        public TDadosLaje() {}
        public TDadosLaje(double h, string nome, int numero, int indicetipo, SConfiguracaoGrelha _ConfiguracaGrelha, bool _GerarGrelha, bool _GerarGrelhaEsp, double permanente, double acidental)
        {
            this.nome = nome;
            this.numero = numero;
            this.h = h;
            this.indiceTipo = indicetipo;
            this.CargaPermanente = permanente;
            this.CargaAcidental = acidental;
            this.GerarGrelha = _GerarGrelha;
            this.GerarGrelhaEspecifica = _GerarGrelhaEsp;
            this.ConfiguracaGrelha = new SConfiguracaoGrelha(_ConfiguracaGrelha.AnguloBarras, _ConfiguracaGrelha.EspacamentoX, _ConfiguracaGrelha.EspacamentoY, _ConfiguracaGrelha.GerarNovaGrelha, _ConfiguracaGrelha.ConsiderarGrelhaEditada);         
        }

        public ISettings Clone()
        {
            TDadosLaje l = new TDadosLaje();
            l.Copy(this);
            return l;
        }

        public void Copy(TDadosLaje dados)
        {
            nome = dados.nome;
            numero = dados.numero;
    
            h = dados.h;
            CargaAcidental = dados.CargaAcidental;
            CargaPermanente = dados.CargaPermanente;
            GerarGrelha = dados.GerarGrelha;
            GerarGrelhaEspecifica = dados.GerarGrelhaEspecifica;
            ConfiguracaGrelha = dados.ConfiguracaGrelha.Clone();
            indiceTipo = dados.indiceTipo;
        }

        public double h, CargaAcidental, CargaPermanente;
        public string nome;
        public int numero;
        public bool GerarGrelhaEspecifica, GerarGrelha;
        public SConfiguracaoGrelha ConfiguracaGrelha;
        /* 
      indiceTipo:

     Retangular    0
     Seção T       1
     Seção T inv.  2
     Seção I       3
     Seção L       4
     Seção L inv.  5
     */
        public int indiceTipo;
    }

}
