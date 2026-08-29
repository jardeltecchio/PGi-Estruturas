using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Media.Media3D;
using static alglib;
using static PG.Geom;

namespace PG
{
    [Serializable]
    public class TPropriedades_Perfil_Retangulo
    {
        public double B,H, AREA, IX, WX, WY, RX, ZX, ZY, IY, IT, RY;
        public string nome;


        [Category("Propridades"), DisplayName("base [cm]") ]
        public string b { get { return B.ToString("n2"); } }// set { BF = value; } }

        [Category("Propridades"), DisplayName("altura [cm]")]
        public string h { get { return H.ToString("n2"); } }
        public string area { get { return AREA.ToString("n2"); } }
        public string ix { get { return IX.ToString("n2"); } }

        [Category("Propridades"), DisplayName("iy [cm4]")]
        public string iy { get { return IY.ToString("n2"); } }

        [Category("Propridades"), DisplayName("it [cm4]"), Description("Inércia à torção")]
        public string it { get { return IT.ToString("n2"); } }

        [Category("Propridades"), DisplayName("ry [cm]")]
        public string ry { get { return RY.ToString("n2"); } }

        [Category("Propridades"), DisplayName("rx [cm]")]
        public string rx { get { return RX.ToString("n2"); } }

        [Category("Propridades"), DisplayName("wx [cm³]")]
        public string wx { get { return (WX).ToString("n2"); } }

        [Category("Propridades"), DisplayName("wy [cm³]")]
        public string wy { get { return (WY).ToString("n2"); } }

        public TPropriedades_Perfil_Retangulo Clone()
        {
            TPropriedades_Perfil_Retangulo l = new TPropriedades_Perfil_Retangulo();
            l.Copy(this);

            return l;
        }

        public void Copy(TPropriedades_Perfil_Retangulo obj)
        {
            AREA = obj.AREA;
            IX = obj.IX;
            WX = obj.WX;
            WY = obj.WY;
            RX = obj.RX;
            ZX = obj.ZX;
            ZY = obj.ZY;
            IY = obj.IY;
            IT = obj.IT;
            RY = obj.RY;
            H = obj.H;
            nome = obj.nome;
        }
    }

    public class Propriedades_Catalogados
    {
        public TPropriedades_Perfil_W_Gerdau Prop_Perfil_W_Gerdau;

        public Propriedades_Catalogados()
        {

        }

    }

    [Serializable]
    public class TSecao : TObjetoDesenho
    {
        public TSecao(CoordenadaD[] Coords, bool catalogada = false, int tipo = 0)
        {
          //  if (!catalogada)
          //    this.CalculaPropriedades(Coords);
       }
        public TSecao(TPoligono pol, string desc, string _tipo)
        {
            this.poligono = pol;
            this.descricao = desc;
            this.tipo = _tipo;
            PropriedadesManuais = false;
            //  if (!catalogada)
            //    this.CalculaPropriedades(Coords);
        }
        public TSecao(List<TPoligono> pols, string desc, string _template)
        {
            this.poligonos = pols;
            this.descricao = desc;
            valoresCotas = new List<double>();
            valoresRaios = new List<double>();
            template = _template;
        }

        public TSecao() : base() { base.Tipo = this.Tipo; }
        public double PesoProprio;
        public string descricao;
        public int idSecaoLaminada;
        public TPoligono poligono;
        public List<TPoligono> poligonos;
        public int id, idMaterial;
        public TPropriedades_Secao propriedades, propriedades_eixos_principais;
        public TMateriais material;
        public List<double> valoresCotas, valoresRaios;
        public string tipo, template;
        public bool alterou, PropriedadesManuais;
        public byte[] Rgb = new byte[3] { 0, 0, 0 };

        public TPropriedades_Perfil_W_Gerdau Prop_Perfil_W_Gerdau;

        /*Propriedades utilizadas no solver e no dimensionamento final*/
        public double area, anguloEixosPrincipais, areaCisalhamentoY, areaCisalhamentoZ, areaCisalhamentoYZ;
        public double inercia_torcao; // it - torção de saint venant
        public double raio_giracao_y, raio_giracao_z; //rx,ry
        public double prod_inercia;//ixy
        public double cy, cz;//Distância Z,Y do centróide até a fibra mais baixa/ mais esquerda da seção
        public double primeiro_momento_area_y, primeiro_momento_area_z;//qy, qz
        public double inercia_flexao_z, inercia_flexao_y;//ix,iy -> segundos momentos de area
        public double ModuloPlastico_y, ModuloPlastico_z;//Zy, Zz; 

        public double ModuloElastico_y_sup, ModuloElastico_z_dir; // wy,wz
        public double ModuloElastico_y_inf, ModuloElastico_z_esq; // wy,wz

        public double e,g,dist_modulo_plastico_z_do_centroide; //d_Zz 
        public double dist_modulo_plastico_y_do_centroide;// d_Zy
        public double centro_cis_y, centro_cis_z;//coordenadas do centro de cisalhamento
        public double prod_setorial_area_y, prod_setorial_area_z;  //iyw, izw
        public double Coef_Deformacao_Corte_y, Coef_Deformacao_Corte_z, Coef_Deformacao_Corte_yz;//kz,ky,kzy
        public double ConstanteEmpenamento; //cw
        public string SimetriaY, SimetriaZ;

        public double b1, h1, hf, d1; //refazer em outra classe

        public void Copy(TSecao obj)
        {
            base.Copy(obj);
            h1 = obj.h1;
            b1 = obj.b1;
            area = obj.area;
            template = obj.template;
            g = obj.g;
            e = obj.e;
            inercia_flexao_z = obj.inercia_flexao_z; 
            inercia_flexao_y = obj.inercia_flexao_y;
            inercia_torcao = obj.inercia_torcao;    
           
            PesoProprio = obj.PesoProprio;
            material = obj.material;
            idMaterial = obj.idMaterial;
            if ((TPropriedades_Perfil_W_Gerdau)obj.Prop_Perfil_W_Gerdau!= null)
                Prop_Perfil_W_Gerdau = (TPropriedades_Perfil_W_Gerdau)obj.Prop_Perfil_W_Gerdau.Clone();
            if ((TPoligono)obj.poligono != null)
              poligono = (TPoligono)obj.poligono.Clone();

            if (obj.valoresCotas != null)
                valoresCotas = obj.valoresCotas.ToList();

            if (obj.valoresRaios != null)
                valoresRaios = obj.valoresRaios.ToList();

            if (obj.poligonos != null)
            {
                poligonos = new List<TPoligono>();
                for (int i = 0; i < obj.poligonos.Count; i++)
                {
                    poligonos.Add((TPoligono)obj.poligonos[i].Clone());
                }
            }
            Rgb[0] = obj.Rgb[0];
            Rgb[1] = obj.Rgb[1];
            Rgb[2] = obj.Rgb[2];
            idSecaoLaminada = obj.idSecaoLaminada;
            id = obj.id;
            Tipo = obj.Tipo;
            tipo = obj.tipo;
            descricao = obj.descricao;

            propriedades = obj.propriedades;
            if (obj.propriedades_eixos_principais!= null)
              propriedades_eixos_principais = obj.propriedades_eixos_principais;

        }

        public override TObjetoDesenho Clone()
        {
            TSecao l = new TSecao();
            l.Copy(this);
            return l;
        }
   }
}
      
      
      
      
      
      
      
      
      
      
      