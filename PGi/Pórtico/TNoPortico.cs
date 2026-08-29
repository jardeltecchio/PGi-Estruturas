using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using OpenTK;
using OpenTK.Input;
using OpenTK.Graphics.OpenGL;
using OpenTK.Platform;

namespace PG
{
    [Serializable]

    public struct Deslocamentos_Nos
    {
        public double[] DeslocamentoGlobal, DeslocamentoLocal;
        public int id;
        public double U_Total { get; set; }
        public Deslocamentos_Nos(int _id)
        {
            DeslocamentoGlobal = new double[7];
            DeslocamentoLocal = new double[7];
            id = _id;
            U_Total = 0;
        }
    }

 /*   public struct Deslocamentos
    {
        public List<Deslocamentos_Nos> deslocamentos;
        public Deslocamentos(int _id)
        {

        }
    }*/

    public class TNoPortico : TObjetoDesenho
    {
        
        public List<Deslocamentos_Nos> combinacoes_x_deslocamentos;
        public List<Deslocamentos_Nos> casos_x_deslocamentos;
       // public List<Deslocamentos> deslocamentos;

        public float px_y, px_x;
        public double coordx_tela, coordy_tela, coordz_tela;
        public double x,
                      y,
                      z,
                      x_offset, y_offset, z_offset, // coordenadas se houver offset na barra, crio um par, uma coordenada na barra original sem offset e outra com offset
                      carga, momento;
        public bool PreSelecionado;
        public int Numero, NAR, indiceMatriz;
        public int nivelMax, nivelMed;
        public int direcaoSentidoMomento //esq dir bai cim   caso seja momento
                  , primeiroGL
                  , unCarga
                  , vinculo; //  0- livre    1- apoio simples    2- engaste    3- mola 

        public bool vertice, CentroidePilar, noContorno, Enquadrar,
                    restrDZ, restrDX, restrDY, restrRX, restrRZ, restrRY, //restrição
                    PossuiMolaDZ, PossuiMolaDY, PossuiMolaDX, PossuiMolaRX, PossuiMolaRY,PossuiMolaRZ,
                    Reordenado;

        public double[] Deslocamento, DeslocamentoLocal, Carga;

        public double K_Mola_DZ, K_Mola_RY, K_Mola_RX,
         K_Mola_DY, K_Mola_DX, K_Mola_RZ, U_Total;

        public bool[] Restricao;

        public List<TBarraPortico> barrasIncidentes;  //barras que incidem no nó
        [NonSerialized]
        public TTrechoViga TrechoViga;

        [NonSerialized]
        public TPilar Pilar;

        [NonSerialized]
        public double[,] RGB_Deslocamentos;

        public int[] GlGlobal;
        int i = 0;

        public TNoPortico(double x, double y, double z)
		{
			this.x = x;
			this.y = y;
            this.z = z;
            this.GlGlobal = new int[7];
            this.barrasIncidentes = new List<TBarraPortico>();
            combinacoes_x_deslocamentos = new List<Deslocamentos_Nos>();
            casos_x_deslocamentos = new List<Deslocamentos_Nos>();
        }

        public TNoPortico(double x_, double y_, double z, float px_x, float px_y, int vinculo, int Numero,TTrechoViga TrechoViga, TPilar Pilar, bool vertice = false, bool noContorno = false)
        {
            this.TrechoViga = TrechoViga;
            this.Pilar = Pilar;
            
            this.z                = z;
            this.x                = x_;
            this.y                = y_;
            this.vinculo          = vinculo;
            this.px_x             = px_x;
            this.px_y             = px_y;
            this.vertice          = vertice;
            this.noContorno       = noContorno;
            this.barrasIncidentes = new List<TBarraPortico>();
            this.Numero = Numero - 1;
            this.Restricao        = new bool[7];
            this.Deslocamento      = new double[7];
            this.DeslocamentoLocal = new double[7];
            combinacoes_x_deslocamentos = new List<Deslocamentos_Nos>();
            casos_x_deslocamentos = new List<Deslocamentos_Nos>();

            this.Carga            = new double[7];
            this.GlGlobal         = new int[7];
            this.Carga            = new double[7];
          //  this.Carga[3] = -0.04;
         //   this.Carga[4] = -0.14;
            this.Enquadrar = true;
        }
         public TNoPortico(double x_, double y_, double z_, double xoffset_, double yoffset_, double zoffset_, int vinculo, int Numero, bool vertice = false)
         {

            if (Geom.Iguais(x_, 0))
                x_ = 0;

            if (Geom.Iguais(y_, 0))
                y_ = 0;
            
            if (Geom.Iguais(z_, 0))
                z_ = 0;

            if (Geom.Iguais(xoffset_, 0))
                xoffset_ = 0;

            if (Geom.Iguais(yoffset_, 0))
                yoffset_ = 0;

            if (Geom.Iguais(zoffset_, 0))
                zoffset_ = 0;

             this.z = z_;
             this.x = x_;
             this.y = y_;

            this.z_offset = zoffset_;
            this.x_offset = xoffset_;
            this.y_offset = yoffset_;

             this.vinculo = vinculo;
             this.vertice = vertice;
             this.barrasIncidentes = new List<TBarraPortico>();
             this.Numero = Numero - 1;
             this.Restricao = new bool[7];
             this.Deslocamento = new double[7];
             this.DeslocamentoLocal = new double[7];
             combinacoes_x_deslocamentos = new List<Deslocamentos_Nos>();
             casos_x_deslocamentos = new List<Deslocamentos_Nos>();

            this.Carga = new double[7];
             this.GlGlobal = new int[7];
             this.Carga = new double[7];
             //  this.Carga[3] = -0.04;
             //   this.Carga[4] = -0.14;
             this.Enquadrar = true;
         }

         public TNoPortico(TNoPortico outro)
        {
            this.x = outro.x;
            this.y = outro.y;
            this.px_x = outro.px_x;
            this.px_y = outro.px_y;
            this.barrasIncidentes = new List<TBarraPortico>();
            this.GlGlobal = new int[7];
            combinacoes_x_deslocamentos = new List<Deslocamentos_Nos>();
            casos_x_deslocamentos       = new List<Deslocamentos_Nos>();
        }
        public bool PossuiRestricao()
        {
            return (restrDY || restrDX || restrDZ || restrRY || restrRX || restrRZ);
        }
        public int GetNumeroDeRestricoes()
        {
            i = 0;

            if (restrDZ) i++;
            if (restrDX) i++;
            if (restrDY) i++;
            if (restrRX) i++;
            if (restrRY) i++;
            if (restrRZ) i++;

            //   2 dz
            //          3 dy          translacoes
            //    |   / 
            //    |  /
            //    | /
            //      ------  1 dx

            //    5 rz
            //    ^     6  ry
            //    ^   /               rotacoes
            //    |  /
            //    | /
            //      ------>>  4  rx

            //preenche o vetor que diz se cada GL está restrito ou nao

       //     if (restrDX)
     //         Restricao[1] = restrDX;
            
            Restricao[1] = restrDX;
            Restricao[2] = restrDZ;         
            Restricao[3] = restrDY;

            Restricao[4] = restrRX;
            Restricao[5] = restrRZ;
            Restricao[6] = restrRY;

            return i;
        }

        public static TNoPortico operator +(TNoPortico lhs, TNoPortico rhs)
        {
            return new TNoPortico(lhs.x + rhs.x, lhs.y + rhs.y, lhs.z + rhs.z);
        }

        public static TNoPortico operator +(TNoPortico lhs, double rhs)
        {
            return new TNoPortico(lhs.x + rhs, lhs.y + rhs, lhs.z + rhs);
        }

        public static TNoPortico operator -(TNoPortico lhs, TNoPortico rhs)
        {
            return new TNoPortico(lhs.x - rhs.x, lhs.y - rhs.y, lhs.z - rhs.z);
        }

        public static TNoPortico operator -(TNoPortico lhs, double rhs)
        {

            return new TNoPortico(lhs.x - rhs, lhs.y - rhs, lhs.z - rhs);
        }

        public static TNoPortico operator *(TNoPortico self, double s)
        {
            return new TNoPortico(self.x * s, self.y * s, self.z * s);
        }
        public static TNoPortico operator *(double lhs, TNoPortico rhs)
        {
            return new TNoPortico(rhs.x * lhs, rhs.y * lhs, rhs.z * lhs);
        }

        public static TNoPortico operator /(TNoPortico lhs, double rhs)
        {
            return new TNoPortico(lhs.x / rhs, lhs.y / rhs, lhs.z / rhs);
        }

        public static bool operator ==(TNoPortico np, TPonto lhs)
        {
            return (Geom.Iguais(lhs.x, np.x) && Geom.Iguais(lhs.y, np.y) && Geom.Iguais(lhs.z, np.z));
        }

        public static bool operator !=(TNoPortico np, TPonto lhs)
        {
            return (!Geom.Iguais(lhs.x, np.x) || !Geom.Iguais(lhs.y, np.y) || !Geom.Iguais(lhs.z, np.z));
        }

        public static TNoPortico operator *(TNoPortico lhs, TNoPortico rhs)
        {
            return new TNoPortico(rhs.x * lhs.x, rhs.y * lhs.y, rhs.z * lhs.z);
        }

        public double Magnitude()
        {
            return Math.Sqrt(x * x + y * y + z * z);
        }

        public double MagnitudePixels()
        {
            return Math.Sqrt(px_x * px_x + px_y * px_y);
        }

        // returns the squared euclidean norm of the vector
        public double SquaredLength()
        {
            return x * x + y * y + z * z;
        }


        /**
       * @return square of vector length
       */
        public double SquaredTo(TNoPortico v1)
        {
            return (this - v1).SquaredLength();
        }

        public double DistanceTo(TNoPortico v)
        {
            return (this - v).Magnitude();

        }
        /**
        * Rotates this vector around 0/0 by the given angle.
        */
        public TNoPortico Rotate(double ang)
        {
            //    Rotate(ang);

            double r = getMagnitude2D();
            double a = getAngle() + ang;

            x = Math.Cos(a) * r;
            y = Math.Sin(a) * r;

            return this;
        }

        /**
       * Rotates this vector around 0/0 by the given vector
       * if the vector is a unit, then, it's the same as rotating around
       * 0/0 by the angle of the vector
       */
        public TNoPortico Rotate(TNoPortico angleVector)
        {
            double x0 = x * angleVector.x - y * angleVector.y;

            y = x * angleVector.y + y * angleVector.x;

            x = x0;

            return this;
        }

        /**
        * Rotates this vector around the given center by the given angle.
        */
        public TNoPortico Rotate(TNoPortico center, double ang)
        {
            return (center + (this - center).Rotate(ang));
        }

        public TNoPortico Rotate(TNoPortico center, TNoPortico angleVector)
        {
            this.x = (center + (this - center).Rotate(angleVector)).x;
            this.y = (center + (this - center).Rotate(angleVector)).y;
            this.z = (center + (this - center).Rotate(angleVector)).z;

            return this;
        }

        // Cross product
        public TNoPortico CrossProduct(TNoPortico p)
        {
            return new TNoPortico(y * p.z - z * p.y,
                            z * p.x - x * p.z,
                            x * p.y - y * p.x);
        }

        // Dot product
        public double DotProduct(TNoPortico p)
        {
            return x * p.x + y * p.y + z * p.z;
        }

        // normalizes the vector magnitude to unity
        public TNoPortico Normalize()
        {
            double length = Magnitude();

            x /= length;
            y /= length;
            z /= length;

            return this;
        }

        public double norm2()
        {
            return x * x + y * y + z * z;
        }

        /**
       * Sets a new position for the vector in polar coordinates.
       *
       * param radius the radius or the distance
       *
       * param angle the angle in rad
       */
        public void setPolar(double radius, double angle)
        {
            x = radius * Math.Cos(angle);
            y = radius * Math.Sin(angle);
            z = 0.0;
        }

        public void setAngle(double a)
        {
            double m = Magnitude();
            setPolar(m, a);
        }

        /*
        * return The angle from zero to this vector (in rad).
        */
        public double getAngle()
        {
            double ret = 0.0;
            double m = getMagnitude2D();

            if (m > 1.0e-6)
            {
                double dp = DotProduct(new TNoPortico(1, 0, 0));

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

        /**
      * \return Angle between this vector and XY plane (horizontal plane).
      */
        public double getAngleToPlaneXY()
        {
            TNoPortico n = new TNoPortico(0, 0, 1);

            if (Magnitude() < 1.0e-4)
                return Math.PI / 2;
            else
                if ((DotProduct(n) / (Magnitude() * 1)) > 1.0)
                    return 0.0;
                else
                    return Math.PI / 2 - Math.Acos(DotProduct(n) / (Magnitude() * 1));

        }

        /**
 * \return The angle from this and the given coordinate (in rad).
 */
        public double getAngleTo(TNoPortico v)
        {
            return (v - this).getAngle();
        }


        /**
         * Sets the vector magnitude without chaning the direction.
         */
        public void setMagnitude2D(double m)
        {
            double a = getAngle();
            setPolar(m, a);
        }

        /**
         * \return Magnitude (length) of the vector.
         */
        public double getMagnitude()
        {

            // Note that the z coordinate is also needed for 2d
            //   (due to definition of crossP())
            return Math.Sqrt(x * x + y * y + z * z);
        }

        /**
         * \return Magnitude (length) of the vector projected to the x/y plane (2d).
         */
        public double getMagnitude2D()
        {
            return Math.Sqrt(x * x + y * y);
        }
        double coordx, coordy, coordz, p_x, p_y;
        string cargatxt;
        int w;

   }
}
