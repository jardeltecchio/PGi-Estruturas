using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using Win32Interop.Enums;

namespace PG
{
    public class ComandoEdicaoBase
    {
        public virtual bool DoUndo(FPrincipal desenho)
        {
            return false;
        }
        public virtual bool DoRedo(FPrincipal desenho)
        {
            return false;
        }
    }
    public class ComandoMover : ComandoEdicaoBase
    {
        TObjetoDesenho m_objeto;
        List<TObjetoDesenho> m_objetos = null;
        TPonto p1 = new TPonto(0);
        TPonto p2 = new TPonto(0);
        string qual_ponto;
        public ComandoMover(List<TObjetoDesenho> objetos, TPonto p1_, TPonto p2_)
        {
            m_objetos = objetos.ToList();
            p1 = new TPonto(p1_.x, p1_.y, p1_.z);
            p2 = new TPonto(p2_.x, p2_.y, p2_.z);
        }

        public ComandoMover(TObjetoDesenho obj,string _qual_ponto, TPonto p1_, TPonto p2_)
        {
            m_objeto = obj;
            p1 = new TPonto(p1_.x, p1_.y, p1_.z);
            p2 = new TPonto(p2_.x, p2_.y, p2_.z);
            qual_ponto = _qual_ponto;
        }
        public ComandoMover(TObjetoDesenho obj, TPonto p1_, TPonto p2_)
        {
            m_objeto = obj;
            p1 = new TPonto(p1_.x, p1_.y, p1_.z);
            p2 = new TPonto(p2_.x, p2_.y, p2_.z);
        }
        public override bool DoUndo(FPrincipal desenho)
        {
            TPonto p_offset = new TPonto(p2.x - p1.x, p2.y - p1.y, p2.z - p1.z);

            if (qual_ponto == string.Empty)
            {
                if (m_objetos != null)
                    foreach (TObjetoDesenho obj in m_objetos)
                    {
                        //TPonto offset = new TPonto[(-p_offset.x, -p_offset.y, -p_offset.z)];
                        obj.Mover(ref p2,ref  p1, true);
                    }

                if (m_objeto != null)
                    m_objeto.Mover(ref p2,ref  p1, true);
            }
            else
            {
                if (m_objetos != null)
                    foreach (TObjetoDesenho obj in m_objetos)
                    {
                        //TPonto offset = new TPonto[(-p_offset.x, -p_offset.y, -p_offset.z)];
                        if (qual_ponto == "pFin")
                          obj.pFin.Mover(ref p2, ref p1, true);
                        else
                        if (qual_ponto == "pIni")
                          obj.pIni.Mover(ref p2, ref p1,true);
                    }

                if (m_objeto != null)
                {
                    if (qual_ponto == "pFin")
                      m_objeto.pFin.Mover(ref p2, ref p1, true);
                    else
                    if (qual_ponto == "pIni")
                      m_objeto.pIni.Mover(ref p2, ref p1, true);
                }
            }
            return true;
        }
        public override bool DoRedo(FPrincipal desenho)
        {
           // foreach (TObjetoDesenho obj in m_objetos)
             //   obj.Mover(p_offset,);
            return true;
        }
    }
    public class ComandoAdicionar: ComandoEdicaoBase
    {
        public List<TObjetoDesenho> m_objetos = null;
        public List<TObjetoDesenho> m_objetos_movidos = null;
        public List<ComandoMover> comandosMover = null;
        TObjetoDesenho m_objeto;
        public ComandoAdicionar(TObjetoDesenho obj)
        {
            m_objeto = obj;
        }

        public ComandoAdicionar(List<TObjetoDesenho> objetos)
        {
            m_objetos = objetos.ToList();
        }

        public ComandoAdicionar(List<TObjetoDesenho> objetos, List<ComandoMover> _comandosMover)
        {
            m_objetos = objetos.ToList();
            comandosMover = _comandosMover.ToList();
        }

        public override bool DoUndo(FPrincipal desenho)
        {
            desenho.SetaSelecionados(false, -1, true);

            if (m_objeto != null)
                m_objeto.SetaSelecao(true, false);
            if (m_objetos != null)
                m_objetos.ForEach(obj => obj.SetaSelecao(true,false));

            desenho.DeletaSelecionados(-1);

            if (comandosMover != null)
            {
                foreach (ComandoMover cm in comandosMover)
                {
                    cm.DoUndo(desenho);
                } 
            }

            return true;
        }
        public override bool DoRedo(FPrincipal desenho)
        {
            if (m_objeto != null)
                desenho.AdicionaObjeto(m_objeto,-1);
            if (m_objetos != null)
              foreach (TObjetoDesenho obj in m_objetos)
                 desenho.AdicionaObjeto(obj,-1);

            return true;
        }
    }
    class ComandoRemover : ComandoEdicaoBase
    {
        List<TObjetoDesenho> m_objetos = new List<TObjetoDesenho>();
        public ComandoRemover()
        {
        }
        public void AicionaObjetosDeletados(List<TObjetoDesenho> objetos)
        {
            m_objetos = objetos.ToList();
        }
        public override bool DoUndo(FPrincipal desenho)
        {
              foreach (TObjetoDesenho obj in m_objetos)
                 desenho.AdicionaObjeto(obj, -1);

            return true;
        }
        public override bool DoRedo(FPrincipal desenho)
        {
            desenho.SetaSelecionados(false, -1, true);

            desenho.DeletaSelecionados(-1);
            return true;
        }
    }
    class ComandoEdicaoFerramentaEdicao : ComandoEdicaoBase
    {
        IEditTool m_tool;
        public ComandoEdicaoFerramentaEdicao(IEditTool tool)
        {
            m_tool = tool;
        }
        public override bool DoUndo(FPrincipal desenho)
        {
            m_tool.Undo();
            return true;
        }
        public override bool DoRedo(FPrincipal desenho)
        {
            m_tool.Redo();
            return true;
        }
    }
    public class UndoRedoBuffer
    {
        public List<ComandoEdicaoBase> m_undoBuffer = new List<ComandoEdicaoBase>();
        public List<ComandoEdicaoBase> m_redoBuffer = new List<ComandoEdicaoBase>();
        bool m_canCapture = true;
        public UndoRedoBuffer()
        {
        }
        public void Clear()
        {
            m_undoBuffer.Clear();
            m_redoBuffer.Clear();
        }
        public bool CanCapture
        {
            get { return m_canCapture; }
        }
        public bool CanUndo
        {
            get { return m_undoBuffer.Count > 0; }
        }
        public bool CanRedo
        {
            get { return m_redoBuffer.Count > 0; }
        }
        public void AdicionaComando(ComandoEdicaoBase comando)
        {
            if (m_canCapture && comando != null)
            {
                m_undoBuffer.Add(comando);
                m_redoBuffer.Clear();
            }
        }
        public bool DoUndo(FPrincipal desenho)
        {
            if (m_undoBuffer.Count == 0)
                return false;
            m_canCapture = false;
            ComandoEdicaoBase comando = m_undoBuffer[m_undoBuffer.Count - 1];
            bool result = comando.DoUndo(desenho);
            m_undoBuffer.RemoveAt(m_undoBuffer.Count - 1);
            m_redoBuffer.Add(comando);
            m_canCapture = true;
            return result;
        }
        public bool DoRedo(FPrincipal desenho)
        {
            if (m_redoBuffer.Count == 0)
                return false;
            m_canCapture = false;
            ComandoEdicaoBase comando = m_redoBuffer[m_redoBuffer.Count - 1];
            bool result = comando.DoRedo(desenho);
            m_redoBuffer.RemoveAt(m_redoBuffer.Count - 1);
            m_undoBuffer.Add(comando);
            m_canCapture = true;
            return result;
        }
    }

}
