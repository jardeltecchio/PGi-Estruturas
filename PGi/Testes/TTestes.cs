using PG;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PGi.Testes
{
    public class TTestes
    {
        public Gerenciador ger;
        public TTestes(Gerenciador _ger)
        {
            ger = _ger; 
        }    
        public void testaSecao()
        {
            if (ger.DadosBarra == null)
                ger.DadosBarra = new FDadosBarra(ger, null);
            ger.DadosBarra.Show();
            ger.DadosBarra.SecaoLaminada = new FEscolheSecao(ger.DadosBarra, ger);
            ger.DadosBarra.SecaoLaminada.Show();
            
            ger.DadosBarra.Visible = false;
            ger.DadosBarra.SecaoLaminada.Left = ger.Width;
            ger.DadosBarra.SecaoLaminada.testasecao = true;

        }

    }
}
