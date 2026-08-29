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
    public class TPonto : TObjetoDesenho
    {
        public double x,
                      y,
                      z;

        public float px_y,  //posição em pixel na tela
                     px_x;

        public bool PontoMedio, habilitado = true,
                    PontoIntersecao,
                    PontoEixoViga,PontoFinal,PontoInicial,
                    Snap,Enquadrar, PontoAuxiliar, PontoCentroidePilar;
        public double[] Carga;
                [NonSerialized]
        public TLinha LinhaDoPontoMedio;
                [NonSerialized]
        public TLinha LinhaIntersecao1,
                      LinhaIntersecao2;

        public int  codigo;
        public bool permiteSnap;

        [NonSerialized]
        public List<TObjetoDesenho> incidencias;

        public bool Incidente(TObjetoDesenho o)
        {
            foreach(TObjetoDesenho obj in incidencias)
            {
                if ((Object)o == (Object)obj)
                    return true;
            }

            return false;
        }

        public TPonto(double x_, double y_, double z_, float px_x, float px_y, int codigo,  
                                                                    bool PontoMedio          = false, 
                                                                    bool PontoIntersecao     = false,
                                                                    TLinha LinhaDoPontoMedio = null,
                                                                    TLinha LinhaIntersecao1  = null,
                                                                    TLinha LinhaIntersecao2  = null,
                                                                    bool permiteSnap         = true,
                                                                    bool Snap = false)
        {
            if (Geom.Iguais(x_, 0))
                x_ = 0;

            if (Geom.Iguais(y_, 0))
                y_ = 0;

            if (Geom.Iguais(z_, 0))
                z_ = 0;

            this.x      = x_;
            this.y      = y_;
            this.z      = z_;
            this.px_x = px_x;
            this.px_y   = px_y;
            this.codigo = codigo;
            base.Visivel = true;
            this.PontoMedio        = PontoMedio;
            this.PontoIntersecao   = PontoIntersecao;
            this.LinhaDoPontoMedio = LinhaDoPontoMedio;
            this.Snap = Snap;
            // o ponto de intersecao é definido por 2 linhas
            this.LinhaIntersecao1 = LinhaIntersecao1;
            this.LinhaIntersecao2 = LinhaIntersecao2;
            base.Selecionado = false;
            this.permiteSnap = permiteSnap;
            base.Tipo = "ponto";
            Enquadrar = true;
            incidencias = new List<TObjetoDesenho>();
            Carga = new double[4];
        }
        public int index, _id;
        public void SetIndex(int _index)
        {
            this.index = _index;
        }
        public void SetID(int id)
        {
            this._id = id;
        }

        public int GetID()
        {
            return _id;
        }
        
        public static int CompareOrder(TPonto p1, TPonto p2)
{
	
	if (p1.y < p2.y)
		return -1;
	else
		if (Geom.Iguais(p1.y,p2.y)) {
			if (p1.x < p2.x)
				return -1;
			else 
				if (Geom.Iguais(p1.x,p2.x))
					return 0;
		}
	
	// p1 is greater than p2
	return 1;

}
        public override TObjetoDesenho Clone()
        {
            TPonto l = new TPonto(1);
            l.Copy(this);
            return l;
        }

        public override void Desenha(ref bool Unifilar, ref int transp, ref  bool arestas)
        {
            
           /* double TamanhoNo = 0.02;
            GL.Begin(PrimitiveType.Polygon);
            GL.Vertex3(x - TamanhoNo, y - TamanhoNo, z + TamanhoNo);
            GL.Vertex3(x + TamanhoNo, y - TamanhoNo, z + TamanhoNo);
            GL.Vertex3(x + TamanhoNo, y + TamanhoNo, z + TamanhoNo);
            GL.Vertex3(x - TamanhoNo, y + TamanhoNo, z + TamanhoNo);
            GL.Vertex3(x - TamanhoNo, y - TamanhoNo, z + TamanhoNo);
            GL.End();*/
        }

        public void Desenha(ref double TamanhoNo)
        {
            if (Visivel)
            {
            //    GL.Color4(Color.Blue);
                GL.Disable(EnableCap.Lighting);
                if (base.Selecionado)
                    GL.Color4(Color.Red);

                GL.Begin(PrimitiveType.Polygon);
                GL.Vertex3(x - TamanhoNo, y - TamanhoNo, z + TamanhoNo);
                GL.Vertex3(x + TamanhoNo, y - TamanhoNo, z + TamanhoNo);
                GL.Vertex3(x + TamanhoNo, y + TamanhoNo, z + TamanhoNo);
                GL.Vertex3(x - TamanhoNo, y + TamanhoNo, z + TamanhoNo);
                GL.Vertex3(x - TamanhoNo, y - TamanhoNo, z + TamanhoNo);
                GL.End();

                GL.Begin(PrimitiveType.Polygon);
                GL.Vertex3(x - TamanhoNo, y + TamanhoNo, z + TamanhoNo);
                GL.Vertex3(x - TamanhoNo, y + TamanhoNo, z - TamanhoNo);
                GL.Vertex3(x + TamanhoNo, y + TamanhoNo, z - TamanhoNo);
                GL.Vertex3(x + TamanhoNo, y + TamanhoNo, z + TamanhoNo);
                GL.Vertex3(x - TamanhoNo, y + TamanhoNo, z + TamanhoNo);
                GL.End();

                GL.Begin(PrimitiveType.Polygon);
                GL.Vertex3(x - TamanhoNo, y - TamanhoNo, z + TamanhoNo);
                GL.Vertex3(x - TamanhoNo, y - TamanhoNo, z - TamanhoNo);
                GL.Vertex3(x + TamanhoNo, y - TamanhoNo, z - TamanhoNo);
                GL.Vertex3(x + TamanhoNo, y - TamanhoNo, z + TamanhoNo);
                GL.Vertex3(x - TamanhoNo, y - TamanhoNo, z + TamanhoNo);
                GL.End();

                GL.Begin(PrimitiveType.Polygon);
                GL.Vertex3(x - TamanhoNo, y - TamanhoNo, z + TamanhoNo);
                GL.Vertex3(x - TamanhoNo, y - TamanhoNo, z - TamanhoNo);
                GL.Vertex3(x - TamanhoNo, y + TamanhoNo, z - TamanhoNo);
                GL.Vertex3(x - TamanhoNo, y + TamanhoNo, z + TamanhoNo);
                GL.Vertex3(x - TamanhoNo, y - TamanhoNo, z + TamanhoNo);
                GL.End();

                GL.Begin(PrimitiveType.Polygon);
                GL.Vertex3(x + TamanhoNo, y - TamanhoNo, z + TamanhoNo);
                GL.Vertex3(x + TamanhoNo, y - TamanhoNo, z - TamanhoNo);
                GL.Vertex3(x + TamanhoNo, y + TamanhoNo, z - TamanhoNo);
                GL.Vertex3(x + TamanhoNo, y + TamanhoNo, z + TamanhoNo);
                GL.Vertex3(x + TamanhoNo, y - TamanhoNo, z + TamanhoNo);
                GL.End();

                GL.Begin(PrimitiveType.Polygon);
                GL.Vertex3(x - TamanhoNo, y - TamanhoNo, z - TamanhoNo);
                GL.Vertex3(x + TamanhoNo, y - TamanhoNo, z - TamanhoNo);
                GL.Vertex3(x + TamanhoNo, y + TamanhoNo, z - TamanhoNo);
                GL.Vertex3(x - TamanhoNo, y + TamanhoNo, z - TamanhoNo);
                GL.Vertex3(x - TamanhoNo, y - TamanhoNo, z - TamanhoNo);
                GL.End();

                GL.Enable(EnableCap.Lighting);
            }
        }
        public void Copy(TPonto obj)
        {
            base.Copy(obj);

            Enquadrar = obj.Enquadrar;

            if (Geom.Iguais(obj.x, 0))
                obj.x = 0;

            if (Geom.Iguais(obj.y, 0))
                obj.y = 0;

            if (Geom.Iguais(obj.z, 0))
                obj.z = 0;

            x    = obj.x;
            y    = obj.y;
            z    = obj.z;
            Tipo = obj.Tipo;
            px_x = FPrincipal.pixelX(obj.x);
            px_y = FPrincipal.pixelY(obj.y);
            Tipo = obj.Tipo;
            Visivel = obj.Visivel;

       /*     xAnt = obj.xAnt;
            xAnt2 = obj.xAnt2;

            yAnt = obj.yAnt;
            yAnt2 = obj.yAnt2;

            zAnt = obj.zAnt;
            zAnt2 = obj.zAnt2;*/
        //    primeiraVezMover = obj.primeiraVezMover;
            PontoMedio = obj.PontoMedio;
            PontoIntersecao     = obj.PontoIntersecao;
            PontoEixoViga       = obj.PontoEixoViga;
            PontoFinal          = obj.PontoFinal;
            PontoInicial        = obj.PontoInicial;
            Snap                = obj.Snap;
            PontoAuxiliar       = obj.PontoAuxiliar;
            PontoCentroidePilar = obj.PontoCentroidePilar;

            if ((Object)obj.LinhaDoPontoMedio != null) 
                LinhaDoPontoMedio = (TLinha)obj.LinhaDoPontoMedio.Clone();
            if ((Object)obj.LinhaIntersecao1 != null) 
                LinhaIntersecao1 = (TLinha)obj.LinhaIntersecao1.Clone();
            if ((Object)obj.LinhaIntersecao2 != null) 
                LinhaIntersecao2 = (TLinha)obj.LinhaIntersecao2.Clone();

            codigo              = obj.codigo;
            permiteSnap         = obj.permiteSnap;
        }
        double xAnt, yAnt, zAnt, xAnt2, yAnt2, zAnt2;
        public bool primeiraVezMover = true;
        vec3 vetorDirecao, p1Mover, p2Mover;
        public override void Mover(ref TPonto ponto1,ref TPonto ponto2, bool dinamico)
        {
            /*
         x' = x + Tx
	     y' = y + Ty
	     z' = z + Tz
             */
            if (dinamico)
            {
               if (primeiraVezMover)
               {
                   xAnt = ponto1.x;
                   yAnt = ponto1.y;
                   zAnt = ponto1.z;
                   p1Mover = new vec3(ponto1.x, ponto1.y, ponto1.z);
                   p2Mover = new vec3(ponto2.x, ponto2.y, ponto2.z);

                   vetorDirecao = new vec3(ponto1.x, ponto1.y, ponto1.z);
                   primeiraVezMover = false;
                }

                p2Mover.x = ponto2.x; 
                p2Mover.y = ponto2.y; 
                p2Mover.z = ponto2.z;
                vetorDirecao  = new vec3( p2Mover - p1Mover);

                this.x += (vetorDirecao.x - xAnt2);
                this.y += (vetorDirecao.y - yAnt2);                                              
                this.z += (vetorDirecao.z - zAnt2);

                xAnt = this.x;
                yAnt = this.y;
                zAnt = this.z;

                xAnt2 = ponto2.x - ponto1.x;
                yAnt2 = ponto2.y - ponto1.y;
                zAnt2 = ponto2.z - ponto1.z;
            }
            else
            { 
                double offset_x = (float)(ponto2.x - ponto1.x);
                double offset_y = (float)(ponto2.y - ponto1.y);
                double offset_z = (float)(ponto2.z - ponto1.z);

                this.x += offset_x;
                this.y += offset_y;
                this.z += offset_z;
            }

            base.Mover(ref ponto1, ref ponto2, dinamico);
        }

        public TPonto(double s)
		{
			x = y = z = s;
            incidencias = new List<TObjetoDesenho>();
            base.Tipo = "ponto";
            Enquadrar = true;         
		}

        [NonSerialized]
        public List<TBarraGenerica> BarrasConectadas = new List<TBarraGenerica>();
        
        public TPonto(double x_, double y_, double z_)
		{
            if (Geom.Iguais(x_, 0))
                x_ = 0;

            if (Geom.Iguais(y_, 0))
                y_ = 0;

            if (Geom.Iguais(z_, 0))
                z_ = 0;
            
            this.x = x_;
			this.y = y_;
            this.z = z_;
            incidencias = new List<TObjetoDesenho>();
            base.Tipo = "ponto";
            Enquadrar = true;
		}

        public TPonto(TPonto v)
        {

            if (Geom.Iguais(v.x, 0))
                v.x = 0;

            if (Geom.Iguais(v.y, 0))
                v.y = 0;

            if (Geom.Iguais(v.z, 0))
                v.z = 0;

            this.x = v.x;
            this.y = v.y;
            this.z = v.z;
            this.index = v.index;
            this._id = v._id;
            base.Tipo = "ponto";
            Enquadrar = true;
            
        }
        [NonSerialized]
        public List<TPoligono> poligonoPonto;
        const double TamanhoPonto =3;
        public void GeraPoligonosSelecao()
        {
            List<TLinha> lin = new List<TLinha>();
            CoordenadaD[] coord1;
            poligonoPonto = new List<TPoligono>();
            int uu = 0;

            coord1 = new CoordenadaD[5];
            
            //FPrincipal.pixel1(ref interx, ref intery, ref interz, ref z_pixel);

            uu = 0;
            FPrincipal.pixel1(x, y, z);
            coord1[uu++] = new CoordenadaD(FPrincipal.px_x1[0] - TamanhoPonto, FPrincipal.px_y1[0] - TamanhoPonto, 0);
            coord1[uu++] = new CoordenadaD(FPrincipal.px_x1[0] - TamanhoPonto, FPrincipal.px_y1[0] + TamanhoPonto, 0);
            coord1[uu++] = new CoordenadaD(FPrincipal.px_x1[0] + TamanhoPonto, FPrincipal.px_y1[0] + TamanhoPonto, 0);
            coord1[uu++] = new CoordenadaD(FPrincipal.px_x1[0] + TamanhoPonto, FPrincipal.px_y1[0] - TamanhoPonto, 0);
            coord1[uu++] = new CoordenadaD(FPrincipal.px_x1[0] - TamanhoPonto, FPrincipal.px_y1[0] - TamanhoPonto, 0);

            lin.Add(new TLinha(new TPonto(coord1[0].X, coord1[0].Y, 0), new TPonto(coord1[1].X, coord1[1].Y, 0), -1));
            lin.Add(new TLinha(new TPonto(coord1[1].X, coord1[1].Y, 0), new TPonto(coord1[2].X, coord1[2].Y, 0), -1));
            lin.Add(new TLinha(new TPonto(coord1[2].X, coord1[2].Y, 0), new TPonto(coord1[3].X, coord1[3].Y, 0), -1));
            lin.Add(new TLinha(new TPonto(coord1[3].X, coord1[3].Y, 0), new TPonto(coord1[0].X, coord1[0].Y, 0), -1));

            poligonoPonto.Add(new TPoligono(ref coord1, ref lin));
        }
        double z_clip;

       public override void SetaSelecao(bool s, bool MostraGrip, bool SelecaoDeCandidato = false, TObjetoDesenho Owner = null)
       {
           base.Selecionado = false;
           if (!this.Visivel)
               return;

           FPrincipal.pixel1(ref x, ref  y, ref  z, ref z_clip);
           ForaDaTela = ((FPrincipal.px_x1[0] < 0)) ||
                             ((FPrincipal.px_x1[0] > FPrincipal.w)) ||
                             ((FPrincipal.px_y1[0] < 0)) ||
                             ((FPrincipal.px_y1[0] > FPrincipal.h));
           if (ForaDaTela)
               return;

           ForaDaTela = !(z_clip < 1 && z_clip > 0);

           if (ForaDaTela)
               return;

           Soma_Z = z_clip;

           base.Selecionado = s;

       }
        bool ForaDaTela;
        public override bool Selecionar(float clicx, float clicy, float coordx, float coordy, TObjetoDesenho Owner = null)
        {
            try
            {
                if (!this.habilitado || !Visivel)
                    return false;

                FPrincipal.pixel1(ref x, ref y, ref z, ref z_clip);
/*
                bool ForaDaTela = ((Desenho.px_x1[0] < 0)) ||
                                  ((Desenho.px_x1[0] > Desenho.w)) ||
                                  ((Desenho.px_y1[0] < 0)) ||
                                  ((Desenho.px_y1[0] > Desenho.h));
                if (ForaDaTela)
                  return false;

                if (z_clip < 1 && z_clip > 0)
                {*/
                    GeraPoligonosSelecao();
                    foreach (TPoligono p1 in poligonoPonto)
                    {
                        if (p1.PontoEmPoligono(clicx, clicy))
                        {
                            SetaSelecao(true, true);
                            return true;
                        }
                    }

                return false;

            }
            catch (Exception)
            {

            }
            return false;
        }

        public static bool operator ==(TPonto a, TPonto b)
        {
            if ((Object)a == null) throw new Exception("Ponto ''a'' nulo.");
            if ((Object)b == null) throw new Exception("Ponto ''b'' nulo.");

            return Geom.Iguais(a.x, b.x,Const.Tol) && Geom.Iguais(a.y, b.y, Const.Tol) && Geom.Iguais(a.z, b.z, Const.Tol);
        }

        public bool Igual(double xp, double yp)
        {
            return (Geom.Iguais(this.x, xp) && (Geom.Iguais(y, yp)));
        }

        public static bool operator !=(TPonto a, TPonto b)
        {
            return (!Geom.Iguais(a.x, b.x, Const.Tol)) || (!Geom.Iguais(a.y, b.y, Const.Tol));
        }

        /*Daqui p baixo sao rotinas destinadas ao tratamento do ponto como sendo um vetor de 3 dim*/
        public static TPonto operator + (TPonto lhs, TPonto rhs)
		{
			return new TPonto(lhs.x + rhs.x, lhs.y + rhs.y, lhs.z + rhs.z);
		}

        public static TPonto operator +(TPonto lhs, double rhs)
        {
            return new TPonto(lhs.x + rhs, lhs.y + rhs, lhs.z + rhs);
        }

        public static TPonto operator -(TPonto lhs, TPonto rhs)
        {
            return new TPonto(lhs.x - rhs.x, lhs.y - rhs.y, lhs.z - rhs.z);
        }

        public static TPonto operator -(TPonto lhs, double rhs)
        {
            return new TPonto(lhs.x - rhs, lhs.y - rhs, lhs.z - rhs);
        }

        public static TPonto operator *(TPonto self, double s)
		{
			return new TPonto(self.x * s, self.y * s, self.z * s);
		}
        public static TPonto operator *(double lhs, TPonto rhs)
        {
            return new TPonto(rhs.x * lhs, rhs.y * lhs, rhs.z * lhs);
        }

        public static TPonto operator /(TPonto lhs, double rhs)
        {
            return new TPonto(lhs.x / rhs, lhs.y / rhs, lhs.z / rhs);
        }

        public static TPonto operator * (TPonto lhs, TPonto rhs)
        {
            return new TPonto(rhs.x * lhs.x, rhs.y * lhs.y, rhs.z * lhs.z);
        }

    // returns the euclidean norm of the vector
	    public double Magnitude() 
    	{
    	    return Math.Sqrt( x * x + y * y + z * z );
    	}   
		
	// returns the squared euclidean norm of the vector
    	public double SquaredLength() 
    	{
	        return x * x + y * y + z * z;
    	}

        
   /**
  * @return square of vector length
  */
        public double SquaredTo(TPonto v1) 
        {
            return (this - v1).SquaredLength();
        }

        public double DistanceTo(TPonto v) 
        {
          return (this-v).Magnitude();

        }

        /**
        * Rotates this vector around 0/0 by the given angle.
        */
        public TPonto Rotate(double ang)
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
       public TPonto Rotate(TPonto angleVector) 
       {
	       double x0 = x * angleVector.x - y * angleVector.y;
	        
           y = x * angleVector.y + y * angleVector.x;
	        
           x = x0;

	       return this;
       }

        /**
        * Rotates this vector around the given center by the given angle.
        */
        public TPonto Rotate(TPonto center, double ang) 
        {
            return (center + (this - center).Rotate(ang));
        }
        public TPonto Rotate(TPonto center,TPonto angleVector) 
        {
            this.x = (center + (this - center).Rotate(angleVector)).x;
            this.y = (center + (this - center).Rotate(angleVector)).y;
            this.z = (center + (this - center).Rotate(angleVector)).z;

            return this;
        }

       // Cross product
	    public TPonto CrossProduct( TPonto p ) 
	    {
		   return new TPonto(y * p.z - z * p.y,
		                   z * p.x - x * p.z,
			               x * p.y - y * p.x);
	    }

	  // Dot product
	   public double DotProduct(TPonto p )    
       {
		   return x * p.x + y * p.y + z * p.z;
       }

	  // normalizes the vector magnitude to unity
	   public TPonto Normalize()
    	{
     		double length = Magnitude();

		    x /= length;
		    y /= length;
		    z /= length;

		    return this;
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
            double m   = getMagnitude2D();

            if (m > 1.0e-6) 
            {
                 double dp = DotProduct(new TPonto(1, 0, 0));
   
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
    TPonto n = new TPonto(0, 0, 1);

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
public double getAngleTo(TPonto v) 
{
    return (v - this).getAngle();
}

        
/**
 * Sets the vector magnitude without chaning the direction.
 */
public void setMagnitude2D(double m) {
    double a = getAngle();
    setPolar(m, a);
}

/**
 * \return Magnitude (length) of the vector.
 */
public double getMagnitude() {

    // Note that the z coordinate is also needed for 2d
    //   (due to definition of crossP())
    return Math.Sqrt(x*x + y*y + z*z);
}

/**
 * \return Magnitude (length) of the vector projected to the x/y plane (2d).
 */
public double getMagnitude2D()
{
    return Math.Sqrt(x*x + y*y);
}

        
/**
 * \return Square of magnitude (length).
 */
public double getSquaredMagnitude()  
{
    return x*x + y*y + z*z;
}

/**
 * Linear interpolation between this and \c v by fraction 't'.
 */
public TPonto getLerp(TPonto  v, double t)
{
    return new TPonto(x + (v.x - x) * t, y + (v.y - y) * t, z + (v.z - z) * t);
}

/**
 * \return Unit vector for this vector.
 */
public TPonto getUnitVector()  
{

    return this / Magnitude();
}
    }
}
