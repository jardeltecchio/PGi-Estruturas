using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PG
{
    public class TErroGrelha : Exception
    {
    }

    public class TFaltaMemoria : Exception
    {
        public TFaltaMemoria(TModeloEstrutural modelo, string msg)
        {
            modelo.HistoricoCalculo(msg);
        }
    }

    public class TErroPavimento: Exception
    {
       public TErroPavimento(TModeloEstrutural modelo, string msg)
        {
            modelo.HistoricoCalculo(msg);
        }
    }

    public class TCalculoInterrompido : Exception
    {
        public TCalculoInterrompido(TModeloEstrutural modelo, string msg)
        {
            modelo.HistoricoCalculo(msg);
        }
    }
    public class TErroPortico : Exception
    {
        public TErroPortico(TModeloEstrutural modelo, string msg)
        {
            modelo.HistoricoCalculo(msg);
        }
    }
    public class TErroInicializacaoObjeto : Exception
    {
        public string msg;
        public TErroInicializacaoObjeto(string msg)
        {
            this.msg = msg;
        }
    }

    public class TErroConcepcaoEstrutural : Exception
    {
        public TErroConcepcaoEstrutural(TModeloEstrutural modelo, string msg)
        {
            modelo.HistoricoCalculo(msg);
        }
    }
    
    public class TErroPonto : Exception
    {
    }
}
