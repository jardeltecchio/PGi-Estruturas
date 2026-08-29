using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PG
{
    [Serializable]
    public class TRetangulo : TPoligono
    {
        TLinha lin1, lin2, lin3, lin4;
        public TRetangulo(TLinha lin1, TLinha lin2, TLinha lin3,TLinha lin4)
        {
            this.lin1 = lin1;
            this.lin2 = lin2;
            this.lin3 = lin3;
            this.lin4 = lin4;
        }

        public TRetangulo(string tipo, int registro, bool selecionado) : base(tipo, registro, selecionado) { }
    }
}
