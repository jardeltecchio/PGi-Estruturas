using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

public static class RMath
{

public const double M_PI   = 3.14159265358979323846264338327950288;
public const double M_PI_2 = 1.57079632679489661923132169163975144;
public const double M_PI_4 = 0.785398163397448309615660845819875721;
public const double AngleTolerance = 1.0e-9;

    public static OpenTK.Matrix4 ToMatrix4(OpenTK.Matrix4d m)
    {
        return new OpenTK.Matrix4(
            (float)m.Row0.X, (float)m.Row0.Y, (float)m.Row0.Z, (float)m.Row0.W,
            (float)m.Row1.X, (float)m.Row1.Y, (float)m.Row1.Z, (float)m.Row1.W,
            (float)m.Row2.X, (float)m.Row2.Y, (float)m.Row2.Z, (float)m.Row2.W,
            (float)m.Row3.X, (float)m.Row3.Y, (float)m.Row3.Z, (float)m.Row3.W);
    }

    public static OpenTK.Vector4 Multiply(
    OpenTK.Vector4 vec,
    OpenTK.Matrix4 mat)
    {
        return new OpenTK.Vector4(
            vec.X * mat.Row0.X + vec.Y * mat.Row1.X + vec.Z * mat.Row2.X + vec.W * mat.Row3.X,
            vec.X * mat.Row0.Y + vec.Y * mat.Row1.Y + vec.Z * mat.Row2.Y + vec.W * mat.Row3.Y,
            vec.X * mat.Row0.Z + vec.Y * mat.Row1.Z + vec.Z * mat.Row2.Z + vec.W * mat.Row3.Z,
            vec.X * mat.Row0.W + vec.Y * mat.Row1.W + vec.Z * mat.Row2.W + vec.W * mat.Row3.W
        );
    }
    public static OpenTK.Vector4d Multiply(OpenTK.Vector4d vec, OpenTK.Matrix4d mat)
    {
        return new OpenTK.Vector4d(
            vec.X * mat.Row0.X + vec.Y * mat.Row1.X + vec.Z * mat.Row2.X + vec.W * mat.Row3.X,
            vec.X * mat.Row0.Y + vec.Y * mat.Row1.Y + vec.Z * mat.Row2.Y + vec.W * mat.Row3.Y,
            vec.X * mat.Row0.Z + vec.Y * mat.Row1.Z + vec.Z * mat.Row2.Z + vec.W * mat.Row3.Z,
            vec.X * mat.Row0.W + vec.Y * mat.Row1.W + vec.Z * mat.Row2.W + vec.W * mat.Row3.W
        );
    }
    public static OpenTK.Matrix4d Multiply(OpenTK.Matrix4d a,OpenTK.Matrix4d b)
    {
        OpenTK.Matrix4d result = new OpenTK.Matrix4d();

        result.Row0.X = a.Row0.X * b.Row0.X + a.Row0.Y * b.Row1.X + a.Row0.Z * b.Row2.X + a.Row0.W * b.Row3.X;
        result.Row0.Y = a.Row0.X * b.Row0.Y + a.Row0.Y * b.Row1.Y + a.Row0.Z * b.Row2.Y + a.Row0.W * b.Row3.Y;
        result.Row0.Z = a.Row0.X * b.Row0.Z + a.Row0.Y * b.Row1.Z + a.Row0.Z * b.Row2.Z + a.Row0.W * b.Row3.Z;
        result.Row0.W = a.Row0.X * b.Row0.W + a.Row0.Y * b.Row1.W + a.Row0.Z * b.Row2.W + a.Row0.W * b.Row3.W;

        result.Row1.X = a.Row1.X * b.Row0.X + a.Row1.Y * b.Row1.X + a.Row1.Z * b.Row2.X + a.Row1.W * b.Row3.X;
        result.Row1.Y = a.Row1.X * b.Row0.Y + a.Row1.Y * b.Row1.Y + a.Row1.Z * b.Row2.Y + a.Row1.W * b.Row3.Y;
        result.Row1.Z = a.Row1.X * b.Row0.Z + a.Row1.Y * b.Row1.Z + a.Row1.Z * b.Row2.Z + a.Row1.W * b.Row3.Z;
        result.Row1.W = a.Row1.X * b.Row0.W + a.Row1.Y * b.Row1.W + a.Row1.Z * b.Row2.W + a.Row1.W * b.Row3.W;

        result.Row2.X = a.Row2.X * b.Row0.X + a.Row2.Y * b.Row1.X + a.Row2.Z * b.Row2.X + a.Row2.W * b.Row3.X;
        result.Row2.Y = a.Row2.X * b.Row0.Y + a.Row2.Y * b.Row1.Y + a.Row2.Z * b.Row2.Y + a.Row2.W * b.Row3.Y;
        result.Row2.Z = a.Row2.X * b.Row0.Z + a.Row2.Y * b.Row1.Z + a.Row2.Z * b.Row2.Z + a.Row2.W * b.Row3.Z;
        result.Row2.W = a.Row2.X * b.Row0.W + a.Row2.Y * b.Row1.W + a.Row2.Z * b.Row2.W + a.Row2.W * b.Row3.W;

        result.Row3.X = a.Row3.X * b.Row0.X + a.Row3.Y * b.Row1.X + a.Row3.Z * b.Row2.X + a.Row3.W * b.Row3.X;
        result.Row3.Y = a.Row3.X * b.Row0.Y + a.Row3.Y * b.Row1.Y + a.Row3.Z * b.Row2.Y + a.Row3.W * b.Row3.Y;
        result.Row3.Z = a.Row3.X * b.Row0.Z + a.Row3.Y * b.Row1.Z + a.Row3.Z * b.Row2.Z + a.Row3.W * b.Row3.Z;
        result.Row3.W = a.Row3.X * b.Row0.W + a.Row3.Y * b.Row1.W + a.Row3.Z * b.Row2.W + a.Row3.W * b.Row3.W;

        return result;
    }
    public static int Clamp(int valor)
    {
        if (valor < 0)
            return 0;

        if (valor > 255)
            return 255;

        return valor;
    }
    public static OpenTK.Matrix4 Multiply(
    OpenTK.Matrix4 a,
    OpenTK.Matrix4 b)
    {
        OpenTK.Matrix4 result = new OpenTK.Matrix4();

        result.Row0.X = a.Row0.X * b.Row0.X + a.Row0.Y * b.Row1.X + a.Row0.Z * b.Row2.X + a.Row0.W * b.Row3.X;
        result.Row0.Y = a.Row0.X * b.Row0.Y + a.Row0.Y * b.Row1.Y + a.Row0.Z * b.Row2.Y + a.Row0.W * b.Row3.Y;
        result.Row0.Z = a.Row0.X * b.Row0.Z + a.Row0.Y * b.Row1.Z + a.Row0.Z * b.Row2.Z + a.Row0.W * b.Row3.Z;
        result.Row0.W = a.Row0.X * b.Row0.W + a.Row0.Y * b.Row1.W + a.Row0.Z * b.Row2.W + a.Row0.W * b.Row3.W;

        result.Row1.X = a.Row1.X * b.Row0.X + a.Row1.Y * b.Row1.X + a.Row1.Z * b.Row2.X + a.Row1.W * b.Row3.X;
        result.Row1.Y = a.Row1.X * b.Row0.Y + a.Row1.Y * b.Row1.Y + a.Row1.Z * b.Row2.Y + a.Row1.W * b.Row3.Y;
        result.Row1.Z = a.Row1.X * b.Row0.Z + a.Row1.Y * b.Row1.Z + a.Row1.Z * b.Row2.Z + a.Row1.W * b.Row3.Z;
        result.Row1.W = a.Row1.X * b.Row0.W + a.Row1.Y * b.Row1.W + a.Row1.Z * b.Row2.W + a.Row1.W * b.Row3.W;

        result.Row2.X = a.Row2.X * b.Row0.X + a.Row2.Y * b.Row1.X + a.Row2.Z * b.Row2.X + a.Row2.W * b.Row3.X;
        result.Row2.Y = a.Row2.X * b.Row0.Y + a.Row2.Y * b.Row1.Y + a.Row2.Z * b.Row2.Y + a.Row2.W * b.Row3.Y;
        result.Row2.Z = a.Row2.X * b.Row0.Z + a.Row2.Y * b.Row1.Z + a.Row2.Z * b.Row2.Z + a.Row2.W * b.Row3.Z;
        result.Row2.W = a.Row2.X * b.Row0.W + a.Row2.Y * b.Row1.W + a.Row2.Z * b.Row2.W + a.Row2.W * b.Row3.W;

        result.Row3.X = a.Row3.X * b.Row0.X + a.Row3.Y * b.Row1.X + a.Row3.Z * b.Row2.X + a.Row3.W * b.Row3.X;
        result.Row3.Y = a.Row3.X * b.Row0.Y + a.Row3.Y * b.Row1.Y + a.Row3.Z * b.Row2.Y + a.Row3.W * b.Row3.Y;
        result.Row3.Z = a.Row3.X * b.Row0.Z + a.Row3.Y * b.Row1.Z + a.Row3.Z * b.Row2.Z + a.Row3.W * b.Row3.Z;
        result.Row3.W = a.Row3.X * b.Row0.W + a.Row3.Y * b.Row1.W + a.Row3.Z * b.Row2.W + a.Row3.W * b.Row3.W;

        return result;
    }
    public static double rad2deg(double a)
{
    return (a / (2.0 * M_PI) * 360.0);
}
    /**
     * Converts grads to degrees.
     *
     * \param a angle in grad (gon)
     */
    public static double gra2deg(double a) 
{
    return a / 400.0 * 360.0;
}

/**
 * Converts degrees to radians.
 *
 * \param a angle in degrees
 */
public static double deg2rad(double a) 
{
    return ((a / 360.0) * (2.0 * M_PI));
}

/**
 * Converts radians to gradians.
 *
 * \param a angle in radians
 */
public static double rad2gra(double a) 
{
    return (a / (2.0 * M_PI) * 400.0);
}

/**
 * Finds greatest common divider using Euclid's algorithm.
 * \sa http://en.wikipedia.org/wiki/Greatest_common_divisor
 *
 * \param a the first number
 *
 * \param b the second number
 *
 * \return The greatest common divisor of \c a and \c b.
 */
public static int getGcd(int a, int b) {
    int rem;

    while (b != 0) 
    {
        rem = a % b;
        a = b;
        b = rem;
    }

    return a;
}

/**
 * Tests if angle a is between a1 and a2. a, a1 and a2 must be in the
 * range between 0 and 2*PI.
 * All angles in rad.
 *
 * \param a the test angle
 * \param a1 the lower limiting angle
 * \param a2 the upper limiting angle
 * \param reversed True for clockwise testing. False for ccw testing.
 * \return true if the angle a is between a1 and a2.
 */
public static bool isAngleBetween(double a, double a1, double a2, bool reversed) {

    a  = getNormalizedAngle(a);
    a1 = getNormalizedAngle(a1);
    a2 = getNormalizedAngle(a2);

    bool ret = false;

    if (reversed) 
    {
        double tmp = a1;
        a1 = a2;
        a2 = tmp;
    }

    if (a1 >= a2 - AngleTolerance) 
    {
        if (a >= a1 - AngleTolerance || a <= a2 + AngleTolerance) 
          ret = true;
        
    } 
    else 
    {
        if (a >= a1 - AngleTolerance && a <= a2 + AngleTolerance) 
          ret = true;
    }
    return ret;
}

    /**
 * Gets the normalized angle from \c a.
 * Used to make sure that an angle is in the range between 0 and 2 pi.
 *
 * \param a the unnormalized angle, e.g. 8
 *
 * \return The angle \c a normalized to the range of \f$ 0 \ldots 2\pi \f$,
 * e.g. normalized angle from 8 is 1.716.
 */
public static double getNormalizedAngle(double a) 
{
    if (a >= 0.0) 
	{
        int n = (int) Math.Floor(a / (2*M_PI));
        a -= 2*M_PI * n;
    } 
	else 
	{
        int n = (int) Math.Ceiling(a / (-2*M_PI));
        a += 2*M_PI * n;
    }

    if (a>2*M_PI-AngleTolerance) 
        a = 0.0;
   
    return a;
}

    /**
 * \param a1 first angle in rad
 * \param a2 second angle in rad
 *
 * \return The angle that needs to be added to a1 to reach a2.
 *         Always positive and less than 2*pi.
 */
public static double getAngleDifference(double a1, double a2) {
    double ret;

    if (a1 >= a2) 
        a2 += 2*M_PI;

    ret = a2 - a1;

    if (ret >= 2*M_PI) 
       ret = 0.0;
    
    return ret;
}

 /**
 * \return Angle a as angle relative to baseAngle.
 *         Result is in range -PI < result < PI.
 */
public static double getRelativeAngle(double a, double baseAngle) 
{
    double ret = a - baseAngle;
   
    if (ret>M_PI) 
        ret-=2*M_PI;
    
    if (ret<-M_PI) 
       ret+=2*M_PI;
    
    return ret;
}

    /**
 * \param a1 first angle in rad
 *
 * \param a2 s second angle in rad
 *
 * \return The angle that needs to be added to a1 to reach a2.
 *         Always between -pi and pi.
 */
public static double getAngleDifference180(double a1, double a2) {
    double ret;

    ret = a2 - a1;
    if (ret > M_PI) 
       ret = -(2*M_PI - ret);
    
    if (ret < -M_PI) 
        ret = 2*M_PI + ret;
    
    return ret;
}


    /**
 * Tests if two angles point approximately in the same direction.
 *
 * \param dir1 first direction
 *
 * \param dir2 second direction
 *
 * \param tolerance Tolerance in rad.
 *
 * \retval true The two angles point in the same direction.
 *
 * \retval false The difference between the two angles is at
 * least \c tolerance radians.
 */
public static bool isSameDirection(double dir1, double dir2, double tolerance) 
{
    double diff = Math.Abs(dir1 - dir2);
    if (diff < tolerance || diff > 2*M_PI - tolerance) {
        return true;
    } else {
        return false;
    }
}
//                                        padrao-> double[][5] r 5 colunas 
public static void getQuadRoots(double[] p, double[][] r)
{
    /*
    Array r[3][5] p[5]
    Roots of poly p[0]*x^2 + p[1]*x + p[2]=0
    x=r[1][k] + i r[2][k] k=1,2
    */
    double b,c,d;
    b=-p[1]/(2.0*p[0]);
    c=p[2]/p[0];
    d=b*b-c;
    if(d>=0.0) {
        if(b>0.0)
            b=(r[1][2]=(Math.Sqrt(d)+b));
        else
            b = (r[1][2] = (-Math.Sqrt(d) + b));
        r[1][1]=c/b;
        r[2][1]=(r[2][2]=0.0);
    }
    else {
        d = (r[2][1] = Math.Sqrt(-d));
        r[2][2]=-d;
        r[1][1]=(r[1][2]=b);
    }
} 
//                                           padrao-> double[][5] r 5 colunas 
public static void getCubicRoots(double[] p, double[][] r) {
    /*
    Array r[3][5] p[5]
    Roots of poly p[0]*x^3 + p[1]*x^2 + p[2]*x + p[3] = 0
    x=r[1][k] + i r[2][k] k=1,...,3
    Assumes 0<arctan(x)<pi/2 for x>0
    */
    double s,t,b,c,d;
    int k;
    if(p[0]!=1.0)
    {
        for(k=1;k<4;k++)
            p[k]=p[k]/p[0];
        p[0]=1.0;
    }
    s=p[1]/3.0;
    t=s*p[1];
    b=0.5*(s*(t/1.5-p[2])+p[3]);
    t=(t-p[2])/3.0;
    c=t*t*t;
    d=b*b-c;
    if(d>=0.0) {
        d = Math.Pow((Math.Sqrt(d) + Math.Abs(b)), 1.0 / 3.0);
        if(d!=0.0) {
            if(b>0.0)
                b=-d;
            else
                b=d;
            c=t/b;
        }
        d = r[2][2] = Math.Sqrt(0.75) * (b - c);
        b=b+c;
        c=r[1][2]=-0.5*b-s;
        if((b>0.0 && s<=0.0) || (b<0.0 && s>0.0)) {
            r[1][1]=c;
            r[2][1]=-d;
            r[1][3]=b-s;
            r[2][3]=0.0;
        }
        else {
            r[1][1]=b-s;
            r[2][1]=0.0;
            r[1][3]=c;
            r[2][3]=-d;
        }

    } /* end 2 equal or complex roots */
    else 
    {
        if(b==0.0)
            d=Math.Atan(1.0)/1.5;
        else
            d = Math.Atan(Math.Sqrt(-d) / Math.Abs(b)) / 3.0;
        if(b<0.0)
            b = 2.0 * Math.Sqrt(t);
        else
            b = -2.0 * Math.Sqrt(t);
        c=Math.Cos(d)*b;
        t = -Math.Sqrt(0.75) * Math.Sin(d) * b - 0.5 * c;
        d=-t-c-s;
        c=c-s;
        t=t-s;
        if (Math.Abs(c) > Math.Abs(t))
        {
            r[1][3]=c;
        }
        else {
            r[1][3]=t;
            t=c;
        }
        if (Math.Abs(d) > Math.Abs(t))
        {
            r[1][2]=d;
        }
        else {
            r[1][2]=t;
            t=d;
        }
        r[1][1]=t;
        for(k=1;k<4;k++)
            r[2][k]=0.0;
    }
    return;
}


}
