using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PG
{
    public class TAdicionaCargaPontual: IEditTool
    {
        List<string> ListaComandos;
         bool DefinindoPonto = false;

        public TAdicionaCargaPontual()
        {
            //lista de comandos em ordem de execução
            ListaComandos = new List<string>();
            ListaComandos.Add("");
        }
        
        public void Initialize(TObjetoDesenho objeto, ref TPonto point, ref string command, ISettings settings, TLayer layer) {}
        public eObjetoDesenhoMouseDown OnMouseDown(Gerenciador gerenciador, FPrincipal desenho, ref List<TObjetoDesenho> ObjetosSelecionados, ref List<TObjetoDesenho> ObjetosResultado, ref TPonto point, ref string command, List<TLinha> Linhas, List<TPonto> Pontos, bool FromGrip = false)
        {
            if (!DefinindoPonto)
            {
   
   
                return eObjetoDesenhoMouseDown.Continue;
            }
            else
            {
           //     desenho.PavimentoAtual.CargasAplicadas.Add(new TCargaPontual(Const.ID_CARGA_PONTUAL, System.Convert.ToDouble(gerenciador.Cargas.Valor.Text), point,null,null,null));
                return eObjetoDesenhoMouseDown.Done;
            }
        }

        public IEditTool Clone()
        {
            return new TAdicionaCargaPontual();
        }
        public bool ApagarSelecionados
        {
             get { return false; }
        }

        public void Continue()
        {
            DefinindoPonto = true;
        }

        public string GetComando(int numeroComando)
        {
            return ListaComandos[numeroComando];
        }

        public string TipoObjetoParaSelecionar
        {
            get { return Const.ID_TRECHOVIGA; }
        }
        public string LayerObjetoParaSelecionar
        {
            get { return null; }
        }

        public eEditToolTipoSelecao editToolTipoSelecao
        {
            get { return eEditToolTipoSelecao.selecaoUnica; }
        }

        public void OnMouseMove(ref TPonto point, bool ShowInfo, bool orto = true, double angulo = 0, bool PontoInicial = false)
        {
        }
        public void OnMouseUp(ref TPonto point)
        {
        }
        public void OnKeyDown(System.Windows.Forms.KeyEventArgs e)
        {
        }
        public void Finished()
        {
        }
        public void Undo()
        {
        }
        public void Redo()
        {
        }
    }
}
