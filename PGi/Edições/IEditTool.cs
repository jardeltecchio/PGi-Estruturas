using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PG
{

    public interface IEditTool
    {
        IEditTool Clone();
        void Initialize(TObjetoDesenho objeto, ref TPonto point, ref string command, ISettings settings, TLayer layer);
    
        eEditToolTipoSelecao editToolTipoSelecao { get; }
        bool ApagarSelecionados { get; }
        string TipoObjetoParaSelecionar { get; }
        string LayerObjetoParaSelecionar { get; }
        string GetComando(int numeroComando);
        void Continue();
        void OnMouseMove(ref TPonto point, bool ShowInfo, bool orto = true, double angulo = 0, bool PontoInicial = false);
         eObjetoDesenhoMouseDown OnMouseDown(Gerenciador gerenciador, FPrincipal desenho, ref List<TObjetoDesenho> ObjetosSelecionados, ref List<TObjetoDesenho> ObjetosResultado, ref TPonto point, ref string command, List<TLinha> Linhas, List<TPonto> Pontos, bool FromGrip = false);
        void OnMouseUp(ref TPonto point);
        void OnKeyDown(System.Windows.Forms.KeyEventArgs e);
        void Finished();
        void Undo();
        void Redo();
    }
}
