using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Globalization;
using System.Runtime.ConstrainedExecution;
using System.Runtime.InteropServices;
using System.Security;

namespace PG
{
    [Serializable]
    [ComVisible(true)]
    public struct comprimento
    {
        public double v;
    }

    public struct metros
    {
        public double v;
        public void para_cm()
        {

        }
    }
    public struct Kpa
    {
        public double v;
        public Kpa(double vv)
        {
            this.v = vv;
        }
        public Mpa para_mpa()
        {
            return new Mpa(v / 1000);
        }
    }

    public struct Mpa
    {
        public double v;
        public Mpa(double vv)
        {
            this.v = vv;
        }
        public double para_kpa()
        {
            return v * 1000;
        }
    }
    public struct Pa
    {
        public double v;
        public Pa(double vv)
        {
            this.v = vv;
        }
        public Kpa para_kpa()
        {
            return new Kpa(v / 1000);
        }
    }

    public struct tensao
    {
        public Kpa Kpa;
        public Mpa Mpa;
        public Pa Pa;
    }

}
