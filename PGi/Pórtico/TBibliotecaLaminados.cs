using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Win32Interop.Enums;

namespace PG
{
    public interface IPropriedades_Perfil_Biblioteca
    {

    }

    [Serializable]
    public class TPropriedades_Perfil_W_Gerdau: IPropriedades_Perfil_Biblioteca
    {
        public double D, BF, TW, AREA, TF, MASSA, IX, WX, WY, RX, ZX, ZY, IY, RT, IT, CW, RY, esbeltezalma, esbeltezmesa, lambdar, H, dlinha, RAIO;
        public string nome;

        [Category("Propridades do catálogo"), DisplayName("d [mm]")]
        public double d
        {
            get
            {
                return D;
            }
            /*     set
                 { 
                     D = value; 
                 }*/
        }


        [Category("Propridades do catálogo"), DisplayName("bf [mm]")]
        public string bf { get { return BF.ToString("n2"); } }// set { BF = value; } }
        [Category("Propridades do catálogo"), DisplayName("tw [mm]")]
        public string tw { get { return TW.ToString("n2"); } }
        [Category("Propridades do catálogo"), DisplayName("área [cm²]")]
        public string area { get { return AREA.ToString("n2"); } }

        [Category("Propridades do catálogo"), DisplayName("tf [mm]")]
        public string tf { get { return TF.ToString("n2"); } }

        [Category("Propridades do catálogo"), DisplayName("massa [kg/m]")]
        public string massa { get { return MASSA.ToString("n2"); } }

        [Category("Propridades do catálogo"), DisplayName("ix [cm4]")]
        public string ix { get { return IX.ToString("n2"); } }

        [Category("Propridades do catálogo"), DisplayName("iy [cm4]")]
        public string iy { get { return IY.ToString("n2"); } }

        [Category("Propridades do catálogo"), DisplayName("rt [cm]")]
        public string rt { get { return RT.ToString("n2"); } }

        [Category("Propridades do catálogo"), DisplayName("it [cm4]"), Description("Inércia à torção")]
        public string it { get { return IT.ToString("n2"); } }

        [Category("Propridades do catálogo"), DisplayName("cw [cm6]"), Description("Const. empenamento")]
        public string cw { get { return CW.ToString("n2"); } }

        [Category("Propridades do catálogo"), DisplayName("ry [cm]")]
        public string ry { get { return RY.ToString("n2"); } }

        [Category("Propridades do catálogo"), DisplayName("rx [cm]")]
        public string rx { get { return RX.ToString("n2"); } }

        [Category("Propridades do catálogo"), DisplayName("bf/2tf [cm6]"), Description("Esbeltez da mesa")]
        public string _esbeltezmesa { get { return esbeltezmesa.ToString("n2"); } }


        [Category("Propridades do catálogo"), DisplayName("d'/tw [cm]"), Description("Esbeltez da alma")]
        public string _esbeltezalma { get { return esbeltezalma.ToString("n2"); } }

        /* [Category("Parâmetros"), DisplayName("λr [cm]")]
         public double _lambdar { get { return lambdar; } }*/

        [Category("Propridades do catálogo"), DisplayName("h [mm]")]
        public string h { get { return H.ToString("n2"); } }

        [Category("Propridades do catálogo"), DisplayName("raio [mm]"), Description("Raio entre mesa e alma")]
        public string raio { get { return (((D / 2) - (dlinha / 2) - TF)).ToString("n2"); } }

        [Category("Propridades do catálogo"), DisplayName("wx [cm³]")]
        public string wx { get { return (WX).ToString("n2"); } }

        [Category("Propridades do catálogo"), DisplayName("wy [cm³]")]
        public string wy { get { return (WY).ToString("n2"); } }

        public TPropriedades_Perfil_W_Gerdau Clone()
        {
            TPropriedades_Perfil_W_Gerdau l = new TPropriedades_Perfil_W_Gerdau();
            l.Copy(this);

            return l;
        }

        public void Copy(TPropriedades_Perfil_W_Gerdau obj)
        {
            D = obj.D;
            BF = obj.BF;
            TW = obj.TW;
            AREA = obj.AREA;
            TF = obj.TF;
            MASSA = obj.MASSA;
            IX = obj.IX;
            WX = obj.WX;
            WY = obj.WY;
            RX = obj.RX;
            ZX = obj.ZX;
            ZY = obj.ZY;
            IY = obj.IY;
            RT = obj.RT;
            IT = obj.IT;
            CW = obj.CW;
            RY = obj.RY;
            esbeltezalma = obj.esbeltezalma;
            esbeltezmesa = obj.esbeltezmesa;
            lambdar = obj.lambdar;
            H = obj.H;
            dlinha = obj.dlinha;
            RAIO = obj.RAIO;
            nome = obj.nome;
        }
    }
    [Serializable]
    public class TPropriedades_Perfil_u_Gerdau: IPropriedades_Perfil_Biblioteca
    {
        public double massa, d, tw, bf, tf, area, Ix, Wx, rx, Iy, Wy, ry, x;
        public string nome;

        public TPropriedades_Perfil_u_Gerdau Clone()
        {
            TPropriedades_Perfil_u_Gerdau l = new TPropriedades_Perfil_u_Gerdau();
            l.Copy(this);

            return l;
        }

        public void Copy(TPropriedades_Perfil_u_Gerdau obj)
        {
            massa = obj.massa;
            d = obj.d;
            tw = obj.d;
            bf = obj.bf;
            tf = obj.tf;
            area = obj.area;
            Ix = obj.Ix;
            Wx = obj.Wx;
            rx = obj.rx;
            Iy = obj.Iy;
            Wy = obj.Wy;
            ry = obj.ry;
            x = obj.x;
            nome = obj.nome;
        }
    }

    [Serializable]
    public class TPropriedades_Perfil_i_Gerdau: IPropriedades_Perfil_Biblioteca
    {
        public double massa, d, tw, bf, tf, area, Ix, Wx, rx, Iy, Wy, ry, rt;
        public string nome;

        public TPropriedades_Perfil_i_Gerdau Clone()
        {
            TPropriedades_Perfil_i_Gerdau l = new TPropriedades_Perfil_i_Gerdau();
            l.Copy(this);

            return l;
        }

        public void Copy(TPropriedades_Perfil_i_Gerdau obj)
        {
            massa = obj.massa;
            d = obj.d;
            tw= obj.d;
            bf= obj.bf;
            tf = obj.tf;
            area = obj.area;
            Ix = obj.Ix;
            Wx= obj.Wx;
            rx= obj.rx;
            Iy= obj.Iy;
            Wy= obj.Wy;
            ry= obj.ry;
            rt = obj.rt;
            nome = obj.nome;
        }
    }
    [Serializable]
    public class TPropriedades_Perfil_t_Gerdau: IPropriedades_Perfil_Biblioteca
    {
        public double massa, d, tw, area, Ix, Wx, rx, Iy, Wy, ry, x;
        public string nome;

        public TPropriedades_Perfil_t_Gerdau Clone()
        {
            TPropriedades_Perfil_t_Gerdau l = new TPropriedades_Perfil_t_Gerdau();
            l.Copy(this);

            return l;
        }

        public void Copy(TPropriedades_Perfil_t_Gerdau obj)
        {
            massa = obj.massa;
            d = obj.d;
            tw = obj.d;
            area = obj.area;
            Ix = obj.Ix;
            Wx = obj.Wx;
            rx = obj.rx;
            Iy = obj.Iy;
            Wy = obj.Wy;
            ry = obj.ry;
            x = obj.x;
            nome = obj.nome;
        }
    }
    public class TPropriedades_Cantoneiras_Gerdau : IPropriedades_Perfil_Biblioteca
    {
        public double b, t;
        public string nome;

        public TPropriedades_Cantoneiras_Gerdau Clone()
        {
            TPropriedades_Cantoneiras_Gerdau l = new TPropriedades_Cantoneiras_Gerdau();
            l.Copy(this);

            return l;
        }

        public void Copy(TPropriedades_Cantoneiras_Gerdau obj)
        {
            b = obj.b;
            t = obj.t;
            nome = obj.nome;
        }
    }

}
