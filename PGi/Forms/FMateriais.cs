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
    public partial class FMateriais : Form
    {
        Gerenciador gerenciador;
        SConfiguracoesProjeto Cfg;
        public FMateriais()
        {
            InitializeComponent();
        }
        public FMateriais(Gerenciador ger)
        {
            this.gerenciador = ger;
            Cfg = gerenciador.ConfiguracoesPGi.CfgProjeto;
          
            InitializeComponent();
            ShowInTaskbar = false;
        }

        public void Carrega()
        {
            if (Cfg.materiais != null)
            if (Cfg.materiais.Count > 0)
            {
                GridMateriais.DataSource = Cfg.materiais.ToArray();
                GridMateriais_DataBindingComplete(GridMateriais, null);
            }
        }
        
        private void GridMateriais_DataBindingComplete(object sender, DataGridViewBindingCompleteEventArgs e)
        {
            if (GridMateriais.Rows.Count > 0)
            {
                {
                    for (int i = 0; i < GridMateriais.Rows.Count; i++)
                    {
                        GridMateriais[8, i].Style.BackColor = Cfg.materiais[i].Cor_;
                    }
                }
            }
            this.DoubleBuffered = true;
            GridMateriais.Columns[1].HeaderText = "Descrição";
            GridMateriais.Columns[4].HeaderText = "G";

            GridMateriais.Columns[5].HeaderText = "C.Poisson";
            GridMateriais.Columns[6].HeaderText = "P.Esp.";
            GridMateriais.Columns[5].Width = 5;
            GridMateriais.Columns[7].HeaderText = "C.térmico";
        }

        private void FMateriais_FormClosed(object sender, FormClosedEventArgs e)
        {
            gerenciador.Materiais = null;

            if (gerenciador.DadosBarra != null)
            {
                if (gerenciador.DadosBarra.SecaoSolida != null)
                    gerenciador.DadosBarra.SecaoSolida.AtualizaMateriais();
            }
        }

        private void FMateriais_Move(object sender, EventArgs e)
        {
            gerenciador.AtualizaDesenho();
        }

        private void FMateriais_Load(object sender, EventArgs e)
        {      
            Carrega();

            this.Left = 0;
        }

        private void button3_Click(object sender, EventArgs e)
        {
            Close();
        }
        bool alterando;
        public FDadosMaterial dadosmaterial;
        private void button6_Click(object sender, EventArgs e)
        {
            dadosmaterial = new FDadosMaterial(this.gerenciador);
            dadosmaterial.alterando = false;
            dadosmaterial.ShowDialog();
        }

        int ri;

        int id;
        private void GridMateriais_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            if (Cfg.materiais.Count > 0 && (e.RowIndex) > -1)
            {
                id = Cfg.materiais[(e.RowIndex)].Id;
                ri = (e.RowIndex);
            }
        }

        private void GridMateriais_CellDoubleClick(object sender, DataGridViewCellEventArgs e)
        {
            alterando = true;
            id = Cfg.materiais[ri].Id;
            
            dadosmaterial = new FDadosMaterial(this.gerenciador);
            dadosmaterial.ri = ri;
            dadosmaterial.Cfg = Cfg;
            dadosmaterial.alterando = true;
            dadosmaterial.ShowDialog();
        }

        private void btExcluir_Click(object sender, EventArgs e)
        {
            if (Cfg.materiais.Count > 0)
            {
               foreach (TSecao b in gerenciador.formDesenho.Secoes)
               {
                   if (b.idMaterial == ri)
                   {
                       MessageBox.Show("Existem elementos associados com esse material. \rAntes de apagar, modifique o material dos elementos.","Atenção", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                       return;
                   }
               }

                DialogResult dlgresult = MessageBox.Show("Deseja apagar o material?"
                                , "Apagar", MessageBoxButtons.YesNo, MessageBoxIcon.Question);
                if (dlgresult == DialogResult.Yes)
                {
                    if (GridMateriais.Rows[ri].Selected)
                    {
                        foreach (TSecao b in gerenciador.formDesenho.Secoes)
                          if (b.idMaterial == ri)
                            b.idMaterial = -1;
                        Cfg.materiais.RemoveAt(ri);
                    }

                    Carrega();
                }
            }
        }

    }
}
