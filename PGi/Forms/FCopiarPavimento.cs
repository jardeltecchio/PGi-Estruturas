using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace PG
{
    
    public partial class FCopiarPavimento : Form
    {
        Gerenciador gerenciador;
        public FCopiarPavimento()
        {
            InitializeComponent();
        }
        public FCopiarPavimento(Gerenciador gerenciador)
        {
            InitializeComponent();

            this.gerenciador = gerenciador;

            if (gerenciador.Pavimentos.Count > 0)
            {
                foreach (TPavimento pav in gerenciador.Pavimentos)
                    lbDe.Items.Add(pav.Descricao);
            }

            lbDe.SelectedIndex = gerenciador.cbPiso.SelectedIndex;
        }

        List<TPavimento> Destino = new List<TPavimento>();
        private void lbDe_SelectedIndexChanged(object sender, EventArgs e)
        {
            TPavimento pav = gerenciador.Pavimentos[lbDe.SelectedIndex];
            lbPara.Items.Clear();
            Destino.Clear();
            foreach (TPavimento pav2 in gerenciador.Pavimentos)
                if ((Object)pav != (Object)pav2)
                {
                    lbPara.Items.Add(pav2.Descricao);
                    Destino.Add(pav2);
                }
        }

        private void button6_Click(object sender, EventArgs e)
        {
            TPavimento Origem = gerenciador.Pavimentos[lbDe.SelectedIndex];
            TLayer layOrigem = Origem.layers[0];
            TLayer layDestino = Origem.layers[0];

            foreach (TPavimento dest in Destino)
            {
                if (chCopiarVigas.Checked)
                {
                    layDestino = dest.LayersByIdPrincipal["Vigas"];
                    layOrigem = Origem.LayersByIdPrincipal["Vigas"];

           //         layDestino.Obj.Clear();
      
                 //   foreach (TLayer lay in Origem.layers[])
                  //  lay.AddObject();
            
                }
            }

        }

        private void chCopiarVigas_CheckedChanged(object sender, EventArgs e)
        {
            chCopiarLajes.Checked = chCopiarVigas.Checked;
            chCopiarLajes.Enabled = chCopiarVigas.Checked; 
        }

        private void button7_Click(object sender, EventArgs e)
        {
            this.Close();
        }

    }
}
