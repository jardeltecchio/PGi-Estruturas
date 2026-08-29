using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace PG
{
    public class TProjeto
    {
        Form gerenciador;
        List<TPorticoEspacial> Porticos;
        List<TPavimento>       Pavimentos;
        List<TLayer>           Layers;
        TEstrutura Estrutura;
        public SGrelhaOpcoesVisualizacao SOpcVisGrelha;

        public TProjeto(Form gerenciador, List<TPorticoEspacial> Porticos, List<TPavimento> Pavimentos, List<TLayer> Layers)
        {
            this.gerenciador = gerenciador;
            this.Porticos    = Porticos;
            this.Pavimentos  = Pavimentos;
            this.Layers      = Layers;

            CriaStructs();
        }

        void CriaStructs()
        {
            SOpcVisGrelha = new SGrelhaOpcoesVisualizacao(true);
        }
    }
}
