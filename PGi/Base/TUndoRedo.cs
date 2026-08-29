using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PG
{
    class EditCommandBase
    {
        public virtual bool DoUndo()
        {
            return false;
        }
        public virtual bool DoRedo()
        {
            return false;
        }
    }

    class EditCommand_Add : EditCommandBase
    {
        List<TObjetoDesenho> m_objects = null;
        TObjetoDesenho m_object;
        TLayer m_layer;

        public EditCommand_Add(TLayer layer, TObjetoDesenho obj)
		{
			m_object = obj;
			m_layer = layer;
		}
		public EditCommand_Add(TLayer layer, List<TObjetoDesenho> objects)
		{
			m_objects = new List<TObjetoDesenho>(objects);
			m_layer = layer;
		}

        public override bool DoUndo()
        {
           // if (m_object != null)
         //       data.DeleteObjects(new IDrawObject[] { m_object });
         //   if (m_objects != null)
         //       data.DeleteObjects(m_objects);
            return true;
        }
        public override bool DoRedo()
        {
         //   if (m_object != null)
         //       data.AddObject(m_layer, m_object);
         //   if (m_objects != null)
         //   {
         //       foreach (IDrawObject obj in m_objects)
         //           data.AddObject(m_layer, obj);
        //    }
            return true;
        }

    }

    class EditCommand_Remove : EditCommandBase
    {
    }

    class EditCommand_Move : EditCommandBase
    {
    }

    class TUndoRedo
    {
        List<EditCommandBase> m_undoBuffer = new List<EditCommandBase>();
        List<EditCommandBase> m_redoBuffer = new List<EditCommandBase>();
        
        public TUndoRedo()
        {

        }
        public void Clear()
        {
            m_undoBuffer.Clear();
            m_redoBuffer.Clear();
        }

        public void AddCommand(EditCommandBase command)
        {
            if (command != null)
            {
                m_undoBuffer.Add(command);
                m_redoBuffer.Clear();
            }
        }

        public bool DoUndo()
        {
            if (m_undoBuffer.Count == 0)
                return false;

            EditCommandBase command = m_undoBuffer[m_undoBuffer.Count - 1];
            
            bool result = command.DoUndo();
            
            m_undoBuffer.RemoveAt(m_undoBuffer.Count - 1);
            m_redoBuffer.Add(command);
 
            return result;
        }
        public bool DoRedo()
        {
            if (m_redoBuffer.Count == 0)
                return false;
 
            EditCommandBase command = m_redoBuffer[m_redoBuffer.Count - 1];
            
            bool result = command.DoRedo();
            
            m_redoBuffer.RemoveAt(m_redoBuffer.Count - 1);
            m_undoBuffer.Add(command);

            return result;
        }
    }

}
