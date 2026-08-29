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
    public partial class FLayers : Form
    {
        public struct Layers
        {

            public string Nome;
            public int tipolinha;
            public int[] Cor;

            public Layers(string nome, int tipolin, int[] rgb)
            {
                rgb = new int[3];
                Cor = rgb;
                this.Nome = nome;
                this.tipolinha = tipolin;
            }
        }

        List<LayersTemp> layersPrincipal;

        Gerenciador gerenciador;
        public FLayers(Gerenciador gerenciador)
        {
            InitializeComponent();
            this.gerenciador = gerenciador;

            layersPrincipal = new List<LayersTemp>();

            Carregar();
        }

        public FLayers()
        {
            InitializeComponent();
        }

        void Carregar()
        {
            foreach (TLayer lay in gerenciador.formDesenho.Estrutura.layers)
                if (lay.nome != Lay.Zero && (lay.Grupo == GrupoLay.Principal || lay.Grupo == null))
                    layersPrincipal.Add(new LayersTemp(lay.nome, lay.Rgb));

            GridTipoElemento.DataSource = layersPrincipal;

            btCorCima.BackColor = gerenciador.ConfiguracoesPGi.F3DOpcoesVisualizacao.CorFundoCima;
            btCorBaixo.BackColor = gerenciador.ConfiguracoesPGi.F3DOpcoesVisualizacao.CorFundoBaixo;
        }

        private void button7_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        List<LayersTemp> layAux;

        private void GridLayers_DataBindingComplete(object sender, DataGridViewBindingCompleteEventArgs e)
        {
            if ((sender as DataGridView) == GridTipoElemento)
                layAux = layersPrincipal;

            if ((sender as DataGridView).Rows.Count > 0)
            {

                // if ((sender as DataGridView).Rows[1].Cells.Count == 5)
                {
                    for (int i = 0; i < (sender as DataGridView).Rows.Count; i++)
                    {
                        int r = layAux[i].Rgb[0];
                        int g = layAux[i].Rgb[1];
                        int b = layAux[i].Rgb[2];

                        (sender as DataGridView)[1, i].Style.BackColor = Color.FromArgb(r, g, b);
                    }
                }
            }
        }

        private void GridLayers_CellDoubleClick(object sender, DataGridViewCellEventArgs e)
        {
            if ((sender as DataGridView) == GridTipoElemento)
                layAux = layersPrincipal;

            if (colorDialog1.ShowDialog() == DialogResult.OK)
            {
                layAux[e.RowIndex].Rgb[0] = colorDialog1.Color.R;
                layAux[e.RowIndex].Rgb[1] = colorDialog1.Color.G;
                layAux[e.RowIndex].Rgb[2] = colorDialog1.Color.B;
            }

            for (int i = 0; i < (sender as DataGridView).Rows.Count; i++)
            {
                int r = layAux[i].Rgb[0];
                int g = layAux[i].Rgb[1];
                int b = layAux[i].Rgb[2];

                (sender as DataGridView)[1, i].Style.BackColor = Color.FromArgb(r, g, b);

            }

            (sender as DataGridView).Refresh();
        }

        DataGridView grid;


        void GravaDados()
        {
            //atribui para todos os pavimentos as mesmas caracteristicas, pois os layers são por projeto, e nao por pavimento
            // eu fiz os layers dentro de cada pavimento para facilitar a gravação e leitura de objetos contidos nesses layers


            /* for (int j = 1; j < gerenciador.Pavimentos[0].layers.Count; j++)
             {
                 if (gerenciador.Pavimentos[0].layers[j].Grupo == GrupoLay.Principal)
                 {
                     gerenciador.ConfiguracoesPGi.layers[j].Congelado = layersPrincipal[j - 1].Congelado;
                     gerenciador.ConfiguracoesPGi.layers[j].Ligado = layersPrincipal[j - 1].Ligado;
                     gerenciador.ConfiguracoesPGi.layers[j].Travado = layersPrincipal[j - 1].Travado;
                     gerenciador.ConfiguracoesPGi.layers[j].Rgb[0] = layersPrincipal[j - 1].Rgb[0];
                     gerenciador.ConfiguracoesPGi.layers[j].Rgb[1] = layersPrincipal[j - 1].Rgb[1];
                     gerenciador.ConfiguracoesPGi.layers[j].Rgb[2] = layersPrincipal[j - 1].Rgb[2];
                 }
             } */

            
            //começo a partir do layer 1, pois o layer 0 nao consta na lista
            for (int j = 1; j < gerenciador.formDesenho.Estrutura.layers.Count; j++)
            {
                if (gerenciador.formDesenho.Estrutura.layers[j].Grupo == GrupoLay.Principal)
                {
                    gerenciador.formDesenho.Estrutura.layers[j].Rgb[0] = layersPrincipal[j - 1].Rgb[0];
                    gerenciador.formDesenho.Estrutura.layers[j].Rgb[1] = layersPrincipal[j - 1].Rgb[1];
                    gerenciador.formDesenho.Estrutura.layers[j].Rgb[2] = layersPrincipal[j - 1].Rgb[2];
                }
            }

            gerenciador.ConfiguracoesPGi.F3DOpcoesVisualizacao.CorFundoCima = btCorCima.BackColor;
            gerenciador.ConfiguracoesPGi.F3DOpcoesVisualizacao.CorFundoBaixo = btCorBaixo.BackColor;
            gerenciador.formDesenho.Alterou(true);
            gerenciador.formDesenho.AtualizaConfiguracoes3D();
            /* int la;
             for (int i = 0; i < gerenciador.Pavimentos.Count; i++)
             {
                 la = -1;
                 //começo a partir do layer 1, pois o layer 0 nao consta na lista
                 for (int j = 0; j < gerenciador.Pavimentos[i].layers.Count; j++)
                 {
                     if (gerenciador.Pavimentos[i].layers[j].Grupo == GrupoLay.Arquitetura)
                     {
                         la++;
                         gerenciador.Pavimentos[i].layers[j].Congelado = layersArquiteura[la].Congelado;
                         gerenciador.Pavimentos[i].layers[j].Ligado    = layersArquiteura[la].Ligado;
                         gerenciador.Pavimentos[i].layers[j].Travado   = layersArquiteura[la].Travado;
                         gerenciador.Pavimentos[i].layers[j].Rgb[0]    = layersArquiteura[la].Rgb[0];
                         gerenciador.Pavimentos[i].layers[j].Rgb[1]    = layersArquiteura[la].Rgb[1];
                         gerenciador.Pavimentos[i].layers[j].Rgb[2]    = layersArquiteura[la].Rgb[2];
                     }
                 }
             }*/

        }

        private void button8_Click(object sender, EventArgs e)
        {
            GravaDados();
            Close();
        }

        private void button9_Click(object sender, EventArgs e)
        {

        }

        private void FLayers_Move(object sender, EventArgs e)
        {
            gerenciador.AtualizaDesenho();

        }

        private void treeView1_AfterSelect(object sender, TreeViewEventArgs e)
        {
            for (int i = 0; i < tabPage1.Controls.Count; i++)
            {
                if (tabPage1.Controls[i] is Panel)
                {
                    (tabPage1.Controls[i] as Panel).Visible = false;
                    if ((tabPage1.Controls[i] as Panel).Name == (string)treeView1.SelectedNode.Tag)
                    {
                        (tabPage1.Controls[i] as Panel).Visible = true;
                        (tabPage1.Controls[i] as Panel).Dock = DockStyle.Fill;
                    }
                }
            }
        }

        private void FLayers_Shown(object sender, EventArgs e)
        {
            treeView1.SelectedNode = treeView1.Nodes[0];

            treeView1_AfterSelect(treeView1, null);
        }

        private void btCorCima_Click(object sender, EventArgs e)
        {
            if (colorDialog1.ShowDialog() == DialogResult.OK)
                btCorCima.BackColor = colorDialog1.Color;
        }

        private void btCorBaixo_Click(object sender, EventArgs e)
        {
            if (colorDialog1.ShowDialog() == DialogResult.OK)
                btCorBaixo.BackColor = colorDialog1.Color;
        }

        private void FLayers_FormClosed(object sender, FormClosedEventArgs e)
        {
            gerenciador.fLayers = null;
        }

        private void label1_Click(object sender, EventArgs e)
        {

        }
    }
}

