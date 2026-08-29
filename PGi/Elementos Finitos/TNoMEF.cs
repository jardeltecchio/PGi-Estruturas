using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PG
{
    public class TNoMEF
    {
        public double x, y,z;
        public int numero;

        [NonSerialized]
        public bool[] Restricao;

        public TNoMEF(double _x, double _y, double _z) 
        {
            x = _x;
            y = _y;
            z = _z;
        }

        public static TNoMEF operator +(TNoMEF lhs, TNoMEF rhs)
        {
            return new TNoMEF(lhs.x + rhs.x, lhs.y + rhs.y, lhs.z + rhs.z);
        }

        public static TNoMEF operator +(TNoMEF lhs, double rhs)
        {
            return new TNoMEF(lhs.x + rhs, lhs.y + rhs, lhs.z + rhs);
        }

        public static TNoMEF operator -(TNoMEF lhs, TNoMEF rhs)
        {
            return new TNoMEF(lhs.x - rhs.x, lhs.y - rhs.y, lhs.z - rhs.z);
        }

        public static TNoMEF operator -(TNoMEF lhs, double rhs)
        {

            return new TNoMEF(lhs.x - rhs, lhs.y - rhs, lhs.z - rhs);
        }

        public static TNoMEF operator *(TNoMEF self, double s)
        {
            return new TNoMEF(self.x * s, self.y * s, self.z * s);
        }
        public static TNoMEF operator *(double lhs, TNoMEF rhs)
        {
            return new TNoMEF(rhs.x * lhs, rhs.y * lhs, rhs.z * lhs);
        }

        public static TNoMEF operator /(TNoMEF lhs, double rhs)
        {
            return new TNoMEF(lhs.x / rhs, lhs.y / rhs, lhs.z / rhs);
        }
        public static TNoMEF operator /(TNoMEF lhs, TNoMEF rhs)
        {
            return new TNoMEF(lhs.x / rhs.x, lhs.y / rhs.y, lhs.z / rhs.z);
        }
        public static TNoMEF operator *(TNoMEF lhs, TNoMEF rhs)
        {
            return new TNoMEF(rhs.x * lhs.x, rhs.y * lhs.y, rhs.z * lhs.z);
        }
    }
}
