using System;

namespace PG
{
    /// <summary>
    /// Represents a three dimensional vector.
    /// </summary>
    /// 
    [Serializable]
	public class vec3
	{
		public double x,x2;
        public double y,y2;
        public double z, z2;
        public int id;
        public double vv;
        public double px_x, px_y;
        public bool pontoEmRaio = false;
        [NonSerialized]
        public TLinha linhaNearest;
        [NonSerialized]
        public bool c1_box, c2_box, c3_box, c4_box, c5_box, c6_box, c7_box, c8_box;
        public vec3(double s)
		{
			x = y = z = s;
		}
        public static vec3 MoverNaDirecao(vec3 ponto, vec3 direcao, double distancia)
        {
            vec3 dir = direcao.Normalize();

            return ponto + dir * distancia;
        }

        public vec3(double x, double y, double z, bool sohPixels = false, double v= 0)
		{
            if (sohPixels)
            {
                this.px_x = x;
                this.px_y = y;
            }
            else
            {
                this.x = x;
                this.y = y;
                this.z = z;

            }

            this.vv = v;
        }


        public vec3(vec3 v)
        {
            this.x = v.x;
            this.y = v.y;
            this.z = v.z;
        }


        #region Operators
        public static vec3 operator + (vec3 lhs, vec3 rhs)
		{
			return new vec3(lhs.x + rhs.x, lhs.y + rhs.y, lhs.z + rhs.z);
		}

        public static vec3 operator +(vec3 lhs, double rhs)
        {
            return new vec3(lhs.x + rhs, lhs.y + rhs, lhs.z + rhs);
        }

        public static vec3 operator -(vec3 lhs, vec3 rhs)
        {
            return new vec3(lhs.x - rhs.x, lhs.y - rhs.y, lhs.z - rhs.z);
        }

        public static vec3 operator -(vec3 lhs, double rhs)
        {
            
            return new vec3(lhs.x - rhs, lhs.y - rhs, lhs.z - rhs);
        }

        public static vec3 operator *(vec3 self, double s)
		{
			return new vec3(self.x * s, self.y * s, self.z * s);
		}
        public static vec3 operator *(double lhs, vec3 rhs)
        {
            return new vec3(rhs.x * lhs, rhs.y * lhs, rhs.z * lhs);
        }

        public static vec3 operator /(vec3 lhs, double rhs)
        {
            return new vec3(lhs.x / rhs, lhs.y / rhs, lhs.z / rhs);
        }
        public static vec3 operator /(vec3 lhs, vec3  rhs)
        {
            return new vec3(lhs.x / rhs.x, lhs.y / rhs.y, lhs.z / rhs.z);
        }
        public static vec3 operator * (vec3 lhs, vec3 rhs)
        {
            return new vec3(rhs.x * lhs.x, rhs.y * lhs.y, rhs.z * lhs.z);
        }


        #endregion

    // returns the euclidean norm of the vector
	    public double Magnitude() 
    	{
    	    return Math.Sqrt( x * x + y * y + z * z );
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
        public double SquaredTo(vec3 v1) 
        {
            return (this - v1).SquaredLength();
        }

        public double DistanceTo(vec3 v) 
        {
          return (this-v).Magnitude();

        }

        public double DistanceBetweenPixels(vec3 v)
        {
         //   return (this - v).MagnitudePixels();

            return new vec3(v.px_x - this.px_x, v.px_y - this.px_y, 0, true).MagnitudePixels();
   
        }

        /**
        * Rotates this vector around 0/0 by the given angle.
        */
        public vec3 Rotate(double ang) 
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
       public vec3 Rotate(vec3 angleVector) 
       {
	       double x0 = x * angleVector.x - y * angleVector.y;
	        
           y = x * angleVector.y + y * angleVector.x;
	        
           x = x0;

	       return this;
       }

        /**
        * Rotates this vector around the given center by the given angle.
        */
        public vec3 Rotate(vec3 center, double ang) 
        {
            try
            {
                return (center + (this - center).Rotate(ang));
            }
            catch(StackOverflowException e)
            {
                System.Windows.Forms.MessageBox.Show(e.Message);
                return null;
            }
        }

        public vec3 Rotate(vec3 center,vec3 angleVector) 
        {
            this.x = (center + (this - center).Rotate(angleVector)).x;
            this.y = (center + (this - center).Rotate(angleVector)).y;
            this.z = (center + (this - center).Rotate(angleVector)).z;

            return this;
        }

        // Cross product
	    public vec3 CrossProduct( vec3 p ) 
	    {
		   return new vec3(y * p.z - z * p.y,
		                   z * p.x - x * p.z,
			               x * p.y - y * p.x);
	    }

	  // Dot product
	   public double DotProduct(vec3 p )    
       {
		   return x * p.x + y * p.y + z * p.z;
       }

	  // normalizes the vector magnitude to unity
	   public vec3 Normalize()
    	{
     		double length = Magnitude();

            if (length > 0)
            {
                x /= length;
                y /= length;
                z /= length;
                return this;
            }
            else
                return new vec3(0,0,0);

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

     /*   public double angleTo(vec3 v)
        {
            double cosA = (this.x * v.x + this.y * v.y + this.z * v.z);
        }*/
      
 /*
 * return The angle from zero to this vector (in rad).
 */
public double getAngle() 
{
            double ret = 0.0;
            double m   = getMagnitude2D();

            if (m > 1.0e-6) 
            {
                 double dp = DotProduct(new vec3(1, 0, 0));
   
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
    vec3 n = new vec3(0, 0, 1);

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
public double getAngleTo(vec3 v) 
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
    try{
    return Math.Sqrt(x*x + y*y);
                }
            catch(StackOverflowException e)
            {
                System.Windows.Forms.MessageBox.Show(e.Message);
                return 0;
            }
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
public vec3 getLerp(vec3  v, double t)
{
    return new vec3(x + (v.x - x) * t, y + (v.y - y) * t, z + (v.z - z) * t);
}

/**
 * \return Unit vector for this vector.
 */
public vec3 Unitario()  
{

    return this / Magnitude();
}


}
}