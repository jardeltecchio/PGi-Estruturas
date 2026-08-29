using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PG
{
        [Serializable]
    public class TCota : TObjetoDesenho
    {
            public TTexto Texto;
            public TPonto pIni, pFin;
            public int Pavimento;
            public double comprimento;
            public TCota(double x1)
            {
                
            }

            public override void Initialize(ref TPonto point, ref string command, ISettings Dados, TLayer layer, ref List<TLinha> Linhas, ref int pavimento)
            {
                base.Tipo = Const.ID_COTA;

                base.pIni = (TPonto)point.Clone();
                pIni = (TPonto)point.Clone();
                base.pFin = (TPonto)point.Clone();
                pFin = (TPonto)point.Clone();

                this.layer = layer;
                this.Pavimento = pavimento;

                base.Initialize(ref point, ref command, Dados, layer, ref Linhas, ref pavimento);
                command = "";
            }
    }
}
