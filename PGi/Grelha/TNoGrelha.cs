using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PG
{
    [Serializable]

    public struct Sincidencias
    {
        public TBarraGrelha l;
        public bool final;
        public Sincidencias(TBarraGrelha _l, bool _final)
        {
            this.l = _l;
            this.final = _final;
        }
    }

    [Serializable]
    public class TNoGrelha : TObjetoDesenho
    {
        public float px_y, px_x;

        public double x,
                      y,
                      carga, momento;
        
        public List<Sincidencias> s_incidencias;

        public int Numero;
        public int nivel, indiceMatriz, NAR;
        public int direcaoSentidoMomento //esq dir bai cim   caso seja momento
                  ,primeiroGL
                  ,unCarga
                  ,vinculo; //  0- livre    1- apoio simples    2- engaste    3- mola 

        public bool vertice, CentroidePilar, noContorno,Enquadrar,
                    restrDZ, restrRX, restrRY, //restrição
                    PossuiMolaDZ, PossuiMolaRX, PossuiMolaRY,
                    Reordenado;
        
        public double[] Deslocamento, Carga;

        public double K_Mola_DZ, K_Mola_RY, K_Mola_RX;

        public bool[] Restricao;

        public List<TBarraGrelha> barrasIncidentes;  //barras que incidem no nó
        [NonSerialized]
        public TLaje Laje;
        [NonSerialized]
        public TTrechoViga TrechoViga;
        [NonSerialized]
        public TPilar Pilar;

        public int[] GlGlobal;
 
        public TNoGrelha(string tipo, int registro, bool selecionado) : base(tipo, registro, selecionado) { }
        
      /*  public override void Desenha()
        {
            FuncoesDesenho.StrokeText(System.Convert.ToString(NEN), px_x, px_y, 0.02f / Desenho.precisaoPixel, -0.02f / Desenho.precisaoPixel, 1, 0, 0);
        }*/
        public TNoGrelha(double x, double y)
		{
			this.x = x;
			this.y = y;
            this.GlGlobal = new int[4];
            this.barrasIncidentes = new List<TBarraGrelha>();
        }

        public TNoGrelha(double x_, double y_, float px_x, float px_y, int vinculo, int Numero,TTrechoViga TrechoViga, TLaje Laje, bool vertice = false, bool noContorno = false, double[] _Carga = null)
        {
            this.TrechoViga = TrechoViga;
            this.Laje       = Laje;
              
            this.x                = x_;
            this.y                = y_;
            this.vinculo          = vinculo;
            this.px_x             = px_x;
            this.px_y             = px_y;
            this.vertice          = vertice;
            this.noContorno       = noContorno;
            this.barrasIncidentes = new List<TBarraGrelha>();
            this.Numero = Numero - 1;
            this.Restricao        = new bool[4];
            this.Deslocamento     = new double[4];
            this.Carga            = new double[4];
            this.GlGlobal         = new int[4];
            this.Carga            = new double[4];

            if (_Carga == null)
            {
           //     this.Carga[3] = -.5;
                this.Carga[1] = 0;
            }
            else
                Carga = _Carga;

            this.Enquadrar = true;
        }

        public TNoGrelha(TNoGrelha outro)
        {
            this.x = outro.x;
            this.y = outro.y;
            this.px_x = outro.px_x;
            this.px_y = outro.px_y;
            this.barrasIncidentes = new List<TBarraGrelha>();
            this.GlGlobal = new int[4];
        }

        public bool PossuiRestricao()
        {
            return (restrDZ || restrRY || restrRX);
        }
        public static TNoGrelha operator +(TNoGrelha lhs, TNoGrelha rhs)
        {
            return new TNoGrelha(lhs.x + rhs.x, lhs.y + rhs.y);
        }

        public static TNoGrelha operator +(TNoGrelha lhs, double rhs)
        {
            return new TNoGrelha(lhs.x + rhs, lhs.y + rhs);
        }
        
        public static TNoGrelha operator -(TNoGrelha lhs, TNoGrelha rhs)
        {
            return new TNoGrelha(lhs.x - rhs.x, lhs.y - rhs.y);
        }

        public static TNoGrelha operator -(TNoGrelha lhs, double rhs)
        {
            return new TNoGrelha(lhs.x - rhs, lhs.y - rhs);
        }
        public static TNoGrelha operator /(TNoGrelha lhs, double rhs)
        {
            return new TNoGrelha(lhs.x / rhs, lhs.y / rhs);
        }
        public double DistanceTo(TNoGrelha v)
        {
            return (this - v).Magnitude();
        }
        public double Magnitude()
        {
            return Math.Sqrt(x * x + y * y);
        }   
        public double getMagnitude2D()
        {
            return Math.Sqrt(x * x + y * y);
        }

        public double DotProduct(TNoGrelha p)
        {
            return x * p.x + y * p.y;
        }

        public double getAngleTo(TNoGrelha v)
        {
            return (v - this).getAngle();
        }

        public double getAngle()
        {
            double ret = 0.0;
            double m = getMagnitude2D();

            if (m > 1.0e-6)
            {
                double dp = DotProduct(new TNoGrelha(1, 0));

                if (dp / m >= 1.0)
                    ret = 0.0;
                else
                    if (dp / m < -1.0)
                        ret = Math.PI;
                    else
                        ret = Math.Acos(dp / m);

                if (y < 0.0)
                    ret = 2 * Math.PI - ret;
            }
            return ret;
        }

        int i = 0;
        public int GetNumeroDeRestricoes()
        {
            i = 0;

            if (restrDZ) i++;
            if (restrRX) i++;
            if (restrRY) i++;

            //    2
            //    ^    3
            //    ^   / 
            //    |  /
            //    | /
            //      ------>>  1

            //preenche o vetor que diz se cada GL está restrito ou nao
            Restricao[1] = restrRX;
            Restricao[2] = restrRY;
            Restricao[3] = restrDZ;

            return i;
        }

        public void UpdatePixel()
        {
            this.px_x = FPrincipal.pixelX(this.x);
            this.px_y = FPrincipal.pixelY(this.y);
        }
    }
}
