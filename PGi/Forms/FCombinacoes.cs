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
            AtualizaListBox(-1, CategoriaAtual);
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
        CategoriaCombinacao CategoriaAtual => tbTipoCombinacao.SelectedIndex == 1
            ? CategoriaCombinacao.Estabilidade : CategoriaCombinacao.Linear;

        List<TCombinacoes> combinacoesFiltradas = new List<TCombinacoes>();

        TCombinacoes CombinacaoSelecionada => lbComb.SelectedIndex >= 0 ? combinacoesFiltradas[lbComb.SelectedIndex] : null;

        void AtualizaListBox(int itemSelecionado, CategoriaCombinacao categoria = CategoriaCombinacao.Linear)
        {
            lbComb.Items.Clear();
            combinacoesFiltradas = gerenciador.formDesenho.Estrutura.combinacoes.Where(c => c.categoriaCombinacao == categoria).ToList();
            foreach (TCombinacoes c in combinacoesFiltradas)
                lbComb.Items.Add(c.Nome +" - "+c.Descricao);

            int indice = combinacoesFiltradas.FindIndex(c => c.Id == itemSelecionado);
            lbComb.SelectedIndex = indice >= 0 ? indice : (lbComb.Items.Count > 0 ? 0 : -1);
            AtualizaSelecao();
        }

        void AtualizaSelecao()
        {
            var combinacao = CombinacaoSelecionada;
            ultSelecao = combinacao == null ? -1 : combinacao.Id;
            pg.SelectedObject = combinacao;
            bindingSource1.DataSource = combinacao?.Coeficientes;
            GridPrincipal.DataSource = combinacao == null ? null : bindingSource1;
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
            var combinacao = CombinacaoSelecionada;
            if (combinacao != null && combinacao.Coeficientes.Count == 0)
                MessageBox.Show("Essa combinação deve conter ao menos um coeficiente para combinar!", "Atenção", MessageBoxButtons.OK, MessageBoxIcon.Warning);
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

            CategoriaCombinacao categoria = CategoriaCombinacao.Linear;
            if (tbTipoCombinacao.SelectedIndex == 0)
                categoria = CategoriaCombinacao.Linear;
            else
            if (tbTipoCombinacao.SelectedIndex == 1)
                categoria = CategoriaCombinacao.Estabilidade;

            gerenciador.formDesenho.Estrutura.combinacoes.Add(new TCombinacoes(contagem, "Combinação " + contagem.ToString(), "C" + contagem.ToString(), TipoEstadoLimite.ELU, categoria));
            AtualizaListBox(contagem, categoria);
        }

        private void pg_PropertyValueChanged(object s, PropertyValueChangedEventArgs e)
        {
            AtualizaListBox(ultSelecao, CategoriaAtual);
        }
        void AtuDescricao()
        {
            var combinacao = gerenciador.formDesenho.Estrutura.combinacoes.Find(o => o.Id == ultSelecao);
            if (combinacao != null)
            {
                string desc = "";
                for (int cc = 0; cc < combinacao.Coeficientes.Count; cc++)
                {
                    if (cc == combinacao.Coeficientes.Count - 1)
                        desc += combinacao.Coeficientes[cc].coef.ToString()
                             + "*" + gerenciador.formDesenho.CasosCarga.Find(o => o.ID == combinacao.Coeficientes[cc].caso).Nome.ToString();
                    else
                        desc += combinacao.Coeficientes[cc].coef.ToString()
                             + "*" + gerenciador.formDesenho.CasosCarga.Find(o => o.ID == combinacao.Coeficientes[cc].caso).Nome.ToString() + " + ";

                }
                combinacao.Descricao = desc;
                AtualizaListBox(ultSelecao, CategoriaAtual);
                pg.SelectedObject = combinacao;


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
                        gerenciador.formDesenho.Estrutura.combinacoes.Remove(CombinacaoSelecionada);

                        if (gerenciador.formDesenho.Estrutura.combinacoes.Count > 0)
                        {
                            AtualizaListBox(-1, CategoriaAtual);
                        }
                        else
                            AtualizaListBox(-1, CategoriaAtual);
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
            AtualizaSelecao();
        }

        private void button4_Click_1(object sender, EventArgs e)
        {
            int contagem = 0;
            if (gerenciador.formDesenho.Estrutura.combinacoes.Count == 0)
              return;

            var combSelecionada = CombinacaoSelecionada;
            if (combSelecionada == null)
                return;

            CategoriaCombinacao categoria = CategoriaCombinacao.Linear;
            if (tbTipoCombinacao.SelectedIndex == 0)
                categoria = CategoriaCombinacao.Linear;
            else
            if (tbTipoCombinacao.SelectedIndex == 1)
                categoria = CategoriaCombinacao.Estabilidade;

            contagem = gerenciador.formDesenho.Estrutura.combinacoes.Max(o => o.Id) + 1;

            gerenciador.formDesenho.Estrutura.combinacoes.Add(new TCombinacoes(contagem, "Combinação " + contagem.ToString(), "C" + contagem.ToString(), TipoEstadoLimite.ELU, categoria));

            for (int i = 0; i < combSelecionada.Coeficientes.Count; i++)
            {
                int caso = combSelecionada.Coeficientes[i].caso;
                double coef = combSelecionada.Coeficientes[i].coef;
                gerenciador.formDesenho.Estrutura.combinacoes[gerenciador.formDesenho.Estrutura.combinacoes.Count - 1].Coeficientes.Add(new CoeficientesCombinacao(caso, coef));
            }

            AtualizaListBox(contagem, categoria);

            AtuDescricao();
        }
        void FiltrarCategoria(CategoriaCombinacao categoria)
        {
            AtualizaListBox(ultSelecao, categoria);
        }

        private void tbTipoCombinacao_SelectedIndexChanged(object sender, EventArgs e)
        {
            CategoriaCombinacao categoria = CategoriaCombinacao.Linear;
            if (tbTipoCombinacao.SelectedIndex == 0)
                categoria = CategoriaCombinacao.Linear;
            else
            if (tbTipoCombinacao.SelectedIndex == 1)
                categoria = CategoriaCombinacao.Estabilidade;

            FiltrarCategoria(categoria);
        }
    }
}
