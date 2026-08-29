using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
namespace PG
{
    [Serializable]
    public class TModeloEstrutural
    {
        public string Descricao;
        public string Tipo;
        
        [NonSerialized] public ProgressBar Progresso;
        public TModeloEstrutural() { }

        public virtual void HistoricoCalculo(string texto, bool Edit = false, bool erro = false) { }
        public virtual void MsgCalculo(string titulo, string texto, int max, bool fim = false, bool MostraProgresso = true) { }
    }
}
