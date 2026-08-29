using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PG
{
    public class TVetor
    {
        public float x,y,z;
        public TVetor(float x, float y, float z)
        {
            this.x = x; this.y = y; this.z = z;
        }

        public float Magnitude()
        {
           return System.Convert.ToSingle(Math.Sqrt(x*x+y*y+z*z));
        }

        public TVetor NormaliseVector()
        {
           TVetor vec = new TVetor(0,0,0);
           float temp = Magnitude();
           vec.x = x / temp;
           vec.y = y / temp;
           vec.z = z / temp;
           return vec;
        }
        
        float DotProduct(TVetor vect)
        {
           float dot;
           dot = vect.x * x + vect.y * y + vect.z * z;
           return dot;
        }

        public TVetor CrossProduct(TVetor vect)
        {
             TVetor temp = this;
             x = vect.y * temp.z - vect.z * temp.y;
             y = vect.z * temp.x - vect.x * temp.z;
             z = vect.x * temp.y - vect.y * temp.x;
            return temp;
        }

        public float AnguloEntreOutroVetor(TVetor vect)
        {
            float soma_dos_quadrados1,
                  soma_dos_quadrados2,
                  norma1,
                  norma2,
                  prod_escalar,
                  prod_normas;

            prod_escalar = (this.x * vect.x) + (this.y * vect.y) + +(this.z * vect.z);

            soma_dos_quadrados1 = (this.x * this.x) + (this.y * this.y) + (this.z * this.z);
            soma_dos_quadrados2 = (vect.x * vect.x) + (vect.y * vect.y) + (vect.z * vect.z);

            norma1 = System.Convert.ToSingle(Math.Sqrt(soma_dos_quadrados1));
            norma2 = System.Convert.ToSingle(Math.Sqrt(soma_dos_quadrados2));
            prod_normas = norma1 * norma2;

            return System.Convert.ToSingle(Math.Acos(prod_escalar / prod_normas)) / Const.PIDiv180;
        }

        public TVetor Unitario()
        {
            float soma_dos_quadrados = (this.x * this.x) + (this.y * this.y) + (this.z * this.z);
            float norma              = System.Convert.ToSingle(Math.Sqrt(soma_dos_quadrados));

            TVetor vec = this;
            
            vec.x = (this.x / norma);
            vec.y = (this.y / norma);
            vec.z = (this.z / norma);

            return vec;
        }
    }
}
