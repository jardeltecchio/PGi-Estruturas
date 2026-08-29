using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace PG
{
    public partial class FCombinacoes : Form
    {
        public FCombinacoes()
        {
            InitializeComponent();
        }

        public FCombinacoes(Gerenciador _gerenciador)
        {
            this.gerenciador = _gerenciador;
            InitializeComponent();
            ShowInTaskbar = false;
        }
        Gerenciador gerenciador;
        void SetWidthPG(PropertyGrid grid, int width)
        {
            Control view = (Control)grid.GetType().GetField("gridView", System.Reflection.BindingFlags.Instance | System.Reflection.BindingFlags.NonPublic).GetValue(grid);
            FieldInfo fi = view.GetType().GetField("labelWidth", System.Reflection.BindingFlags.Instance |  System.Reflection.BindingFlags.NonPublic);
            fi.SetValue(view,width);
            view.Invalidate();
        }
        private void FCombinacoes_Load(object sender, EventArgs e)
        {
            GridPrincipal.AutoGenerateColumns = false;
            
            var combobox = (DataGridViewComboBoxColumn)GridPrincipal.Columns["Caso"];
            combobox.DisplayMember = "NOME";
            combobox.ValueMember = "ID_";
            combobox.DataSource = gerenciador.formDesenho.CasosCarga;
            AtualizaListBox(0);
            SetWidthPG(pg, 20);
            /*    DataGridViewComboBoxColumn colCaso = new DataGridViewComboBoxColumn();
                colCaso.HeaderText = "Caso";
                colCaso.DataSource = gerenciador.formDesenho.CasosCarga;
                colCaso.DisplayMember = "Nome";
                colCaso.ValueMember = "Id";
                /*  foreach (TCasosCarga c in gerenciador.formDesenho.CasosCarga)
                  {
                      colCaso.Items.Add(c.Nome);
                  }*/

            //    colCaso.DataPropertyName = "caso";

            /*  GridPrincipal.Columns.Add(colCaso);


              DataGridViewTextBoxColumn colCoef = new DataGridViewTextBoxColumn();
              colCoef.HeaderText = "Coeficiente";
              colCoef.DataPropertyName = "coef";
              GridPrincipal.Columns.Add(colCoef);*/

            //      GridPrincipal.DataSource = gerenciador.formDesenho.Estrutura.combinacoes;
            // Grid_DataBindingComplete(Grid, null);
        }
        void AtualizaListBox(int itemSelecionado)
        {
            lbComb.Items.Clear();
            if (gerenciador.formDesenho.Estrutura.combinacoes.Count > 0)
                foreach (TCombinacoes c in gerenciador.formDesenho.Estrutura.combinacoes)
                    lbComb.Items.Add(c.Nome +" - "+c.Descricao);
           
            if (itemSelecionado > -1)
            {
                pg.SelectedObject = null;
                if (lbComb.Items.Count > 0)
                {
                    ultSelecao = itemSelecionado;
                    lbComb.SelectedIndex = itemSelecionado;
                    pg.SelectedObject = gerenciador.formDesenho.Estrutura.combinacoes[lbComb.SelectedIndex];
                    bindingSource1.DataSource = gerenciador.formDesenho.Estrutura.combinacoes[lbComb.SelectedIndex].Coeficientes;
                    GridPrincipal.DataSource = bindingSource1;// gerenciador.formDesenho.Estrutura.combinacoes[lbComb.SelectedIndex].Coeficientes;
                }
                else
                    GridPrincipal.DataSource = null;
            }
        }

        private void FCombinacoes_Move(object sender, EventArgs e)
        {
            gerenciador.AtualizaDesenho();
        }



        private void button3_Click(object sender, EventArgs e)
        {
            Close();
        }

        private void FCombinacoes_FormClosed(object sender, FormClosedEventArgs e)
        {
            gerenciador.AtualizaComboCombinacoes();
            gerenciador.Combinacoes = null;
        }

        private void textBox2_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (!char.IsControl(e.KeyChar) && !char.IsDigit(e.KeyChar) && (e.KeyChar != '.') && (e.KeyChar != '-'))
            {
                e.Handled = true;
            }
            if (!char.IsControl(e.KeyChar) && !char.IsDigit(e.KeyChar) && (e.KeyChar != '.') && (e.KeyChar != '-'))
            {
                e.Handled = true;
            }

            if (e.KeyChar == (char)Keys.Enter || e.KeyChar == (char)Keys.Escape)
            {
                e.Handled = true;   // stop annoying beep
            }
            base.OnKeyPress(e);
        }

        private void textBox2_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
              SendKeys.Send("{TAB}");
        }
        int ultSelecao = -1;
        private void lbComb_Click(object sender, EventArgs e)
        {
            if (lbComb.Items.Count > 0)
            if (lbComb.SelectedIndex > -1)
            {
                    int ite = lbComb.SelectedIndex;
                    GridPrincipal.DataSource = gerenciador.formDesenho.Estrutura.combinacoes[ite].Coeficientes;
                    pg.SelectedObject = gerenciador.formDesenho.Estrutura.combinacoes[lbComb.SelectedIndex];

                    bindingSource1.DataSource = gerenciador.formDesenho.Estrutura.combinacoes[lbComb.SelectedIndex].Coeficientes;
                    GridPrincipal.DataSource = bindingSource1;

                    ultSelecao = ite;

                    if (gerenciador.formDesenho.Estrutura.combinacoes[lbComb.SelectedIndex].Coeficientes.Count == 0)
                    {
                        MessageBox.Show("Essa combinação deve conter ao menos um coeficiente para combinar!", "Atenção", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    }
                }
        }

        private void GridPrincipal_KeyPress(object sender, KeyPressEventArgs e)
        {

        }

        private void GridPrincipal_KeyDown(object sender, KeyEventArgs e)
        {

        }

        private void button1_Click(object sender, EventArgs e)
        {
            if (lbComb.SelectedIndex > -1) 
               bindingSource1.Add(new CoeficientesCombinacao(1,1));
        }

        private void button2_Click(object sender, EventArgs e)
        {
            if (ultSelecao != -1)
                bindingSource1.RemoveCurrent();
        }

        private void button6_Click(object sender, EventArgs e)
        {
            int contagem = 0;
            if (gerenciador.formDesenho.Estrutura.combinacoes.Count == 0)
                contagem = 1;
            else
                contagem = gerenciador.formDesenho.Estrutura.combinacoes.Max(o => o.Id) + 1;
            
            gerenciador.formDesenho.Estrutura.combinacoes.Add(new TCombinacoes(contagem, "Combinação " + contagem.ToString(), "C" + contagem.ToString(), TipoCombinacao.ELU));
            AtualizaListBox(gerenciador.formDesenho.Estrutura.combinacoes.Count - 1);
            ultSelecao = gerenciador.formDesenho.Estrutura.combinacoes.Count - 1;
            lbComb.SelectedIndex = lbComb.Items.Count - 1;

            int ite = lbComb.SelectedIndex;
            GridPrincipal.DataSource = gerenciador.formDesenho.Estrutura.combinacoes[ite].Coeficientes;
            pg.SelectedObject = gerenciador.formDesenho.Estrutura.combinacoes[lbComb.SelectedIndex];

            bindingSource1.DataSource = gerenciador.formDesenho.Estrutura.combinacoes[lbComb.SelectedIndex].Coeficientes;
            GridPrincipal.DataSource = bindingSource1;

            ultSelecao = ite;
        }

        private void pg_PropertyValueChanged(object s, PropertyValueChangedEventArgs e)
        {
            AtualizaListBox(ultSelecao);
        }
        void AtuDescricao()
        {
            if (ultSelecao > -1)
            {
                string desc = "";
                for (int cc = 0; cc < gerenciador.formDesenho.Estrutura.combinacoes[ultSelecao].Coeficientes.Count; cc++)
                {
                    if (cc == gerenciador.formDesenho.Estrutura.combinacoes[ultSelecao].Coeficientes.Count - 1)
                        desc += gerenciador.formDesenho.Estrutura.combinacoes[ultSelecao].Coeficientes[cc].coef.ToString()
                             + "*" + gerenciador.formDesenho.CasosCarga.Find(o => o.ID == gerenciador.formDesenho.Estrutura.combinacoes[ultSelecao].Coeficientes[cc].caso).Nome.ToString();
                    else
                        desc += gerenciador.formDesenho.Estrutura.combinacoes[ultSelecao].Coeficientes[cc].coef.ToString()
                             + "*" + gerenciador.formDesenho.CasosCarga.Find(o => o.ID == gerenciador.formDesenho.Estrutura.combinacoes[ultSelecao].Coeficientes[cc].caso).Nome.ToString() + " + ";

                }
                gerenciador.formDesenho.Estrutura.combinacoes[ultSelecao].Descricao = desc;
                AtualizaListBox(ultSelecao);
                pg.SelectedObject = gerenciador.formDesenho.Estrutura.combinacoes[ultSelecao];


            }
        }

        private void button7_Click(object sender, EventArgs e)
        {
            if (lbComb.SelectedIndex > -1)
                if (gerenciador.formDesenho.Estrutura.combinacoes.Count > 0)
                {

                    DialogResult dlgresult = MessageBox.Show("Deseja apagar a combinação selecionada?"
                                    , "Apagar", MessageBoxButtons.YesNo, MessageBoxIcon.Question);
                    if (dlgresult == DialogResult.Yes)
                    {
                        List<int> tmp = new List<int>();
                        gerenciador.formDesenho.Estrutura.combinacoes.RemoveAt(lbComb.SelectedIndex);

                        if (gerenciador.formDesenho.Estrutura.combinacoes.Count > 0)
                        {
                            AtualizaListBox(0);
                        }
                        else
                            AtualizaListBox(0);
                    }
                }
        }

        private void button4_Click(object sender, EventArgs e)
        {

        }

        private void GridPrincipal_CellEndEdit(object sender, DataGridViewCellEventArgs e)
        {
            AtuDescricao();
        }

        private void lbComb_SelectedIndexChanged(object sender, EventArgs e)
        {

        }

        private void button4_Click_1(object sender, EventArgs e)
        {
            int contagem = 0;
            if (gerenciador.formDesenho.Estrutura.combinacoes.Count == 0)
              return;

            int combSelecionada = lbComb.SelectedIndex;

            contagem = gerenciador.formDesenho.Estrutura.combinacoes.Max(o => o.Id) + 1;

            gerenciador.formDesenho.Estrutura.combinacoes.Add(new TCombinacoes(contagem, "Combinação " + contagem.ToString(), "C" + contagem.ToString(), TipoCombinacao.ELU));
          
            for (int i = 0; i < gerenciador.formDesenho.Estrutura.combinacoes[combSelecionada].Coeficientes.Count; i++)
            {
                int caso = gerenciador.formDesenho.Estrutura.combinacoes[combSelecionada].Coeficientes[i].caso;
                double coef = gerenciador.formDesenho.Estrutura.combinacoes[combSelecionada].Coeficientes[i].coef;
                gerenciador.formDesenho.Estrutura.combinacoes[gerenciador.formDesenho.Estrutura.combinacoes.Count - 1].Coeficientes.Add(new CoeficientesCombinacao(caso, coef));
            }
            
            AtualizaListBox(gerenciador.formDesenho.Estrutura.combinacoes.Count - 1);
            ultSelecao = gerenciador.formDesenho.Estrutura.combinacoes.Count - 1;
            lbComb.SelectedIndex = lbComb.Items.Count - 1;

            int ite = lbComb.SelectedIndex;
            GridPrincipal.DataSource = gerenciador.formDesenho.Estrutura.combinacoes[ite].Coeficientes;
            pg.SelectedObject = gerenciador.formDesenho.Estrutura.combinacoes[lbComb.SelectedIndex];

            bindingSource1.DataSource = gerenciador.formDesenho.Estrutura.combinacoes[lbComb.SelectedIndex].Coeficientes;
            GridPrincipal.DataSource = bindingSource1;

            ultSelecao = ite;

            AtuDescricao();
        }
    }
}
