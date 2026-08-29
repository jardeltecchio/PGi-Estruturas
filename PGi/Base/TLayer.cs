using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace PG
{

    public class LayersTemp
    {
        public string Nome { get; set; }
        public string Cor { get; set; }

        public byte[] Rgb;
        public LayersTemp(string nome, byte[] rgb)
        {
            this.Nome = nome;
            this.Rgb = new byte[3];
            this.Rgb = rgb;
        }
    }

     [Serializable]    
    public class TLayer : TObjetoDesenho
    {
         public string nome; 
         public int    tipolinha;

        public string Grupo;
        public bool   Ligado {get;set;}
        public bool  Congelado{get;set;}
        public bool  Travado{get;set;}

        /*Ligado: objetos ficam invisíveis, porém deixa selecionar ,editar e rastrear*/
        /*Congelado: objetos ficam ivisíveis e não deixa selecionar, editar e nem rastrear*/
        /*Travado: objetos ficam visíveis, mas não deixa selecionar nem editar*/


        public byte[] Rgb = new byte[3] { 0, 0, 0 }; /*RGB*/
         
     //   public List<TObjetoDesenho> Obj;               /*Objetos do layer*/

        public TLayer(string nome, bool ligado, bool congelado, byte[] Rgb, int tipolinha, bool travado, string grupo = "", List<TObjetoDesenho> objs = null)
        {
            this.nome       = nome;
            this.Rgb        = Rgb;
            this.Grupo      = grupo;

            this.Ligado     = ligado;
            this.Congelado  = congelado;

            this.tipolinha  = tipolinha;
            this.Travado    = travado;
        }

        public void Copy(TLayer Obj)
        {       
            this.nome = Obj.nome;
            this.Rgb = Obj.Rgb;
            this.Ligado = Obj.Ligado;
            this.Congelado = Obj.Congelado;
            this.tipolinha = Obj.tipolinha;
            this.Travado = Obj.Travado;
           // this.Obj = new List<TObjetoDesenho>();
        }

        public void Draw(ref System.Drawing.Graphics Cad)
        {
           /*    foreach (TObjetoDesenho obj in Obj)
               try
               {
                  obj.Desenha(ref Cad);
               }
               catch(Exception e)
               {
                 System.Windows.Forms.MessageBox.Show("erro no objeto " +obj.Tipo +"  ---  " +e.Message);
               }*/
        }

        public void Desenha3D(ref bool Unifilar, ref int transp, ref bool Arestas)
        {
          /*  foreach (TObjetoDesenho obj in Obj)
                try
                {
                    obj.Desenha(ref Unifilar, ref transp, ref Arestas);
                }
                catch (Exception e)
                {
                    System.Windows.Forms.MessageBox.Show("TLayer.Desenha3D - erro no objeto " + obj.Tipo + "  ---  " + e.Message);
                }*/
        }

    }
}
