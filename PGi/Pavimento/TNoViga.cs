using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PG
{
    [Serializable]
    public class TNoViga : TObjetoDesenho
    {

        public TNoViga(string tipo, int registro, bool selecionado) : base(tipo, registro, selecionado) { }
        

        public TNoViga(double x, double y, double z)
        {
        }

        public override void Desenha(ref bool Unifilar, ref int transp,ref  bool Arestas)
        {
           // FuncoesDesenho.drawCircle();
        }
    }
}
