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
using Win32Interop.Enums;
using static Microsoft.TeamFoundation.Client.CommandLine.Options;

namespace PG
{
    public partial class FCasosCarga : Form
    {
        public FCasosCarga()
        {
            InitializeComponent();
        }
        public FCasosCarga(Gerenciador _gerenciador)
        {
            this.gerenciador = _gerenciador;
            InitializeComponent();
            ShowInTaskbar = false;

        }
        Gerenciador gerenciador;
        
        private void FCasosCarga_Load(object sender, EventArgs e)
        {
            Carregar();
            
        }
        TCasosCarga casosdecarga;
        private void SetLabelColumnWidth(int width)
        {
            FieldInfo fi = this.GetType().BaseType.GetField("gridView", BindingFlags.Instance | BindingFlags.NonPublic);
            object view = fi.GetValue(this);
            MethodInfo mi = view.GetType().GetMethod("MoveSplitterTo", BindingFlags.Instance | BindingFlags.NonPublic);

            mi.Invoke(view, new object[] { width });
        }

        public void Carregar()
        {
            AtualizaListBox(gerenciador.formDesenho.CasosCarga.Count-1);
        
        }

        private void FCasosCarga_Move(object sender, EventArgs e)
        {
            gerenciador.AtualizaDesenho();
        }

        private void button3_Click(object sender, EventArgs e)
        {
            Close();
        }

        private void FCasosCarga_FormClosed(object sender, FormClosedEventArgs e)
        {
            int ind = gerenciador.cbCasoCarga.SelectedIndex;
            gerenciador.AtualizaComboCarga();
            gerenciador.AtualizaComboCombinacoes();
     
            gerenciador.CasosCarga = null;
            if (gerenciador.CargaBarra != null)
            {
                gerenciador.CargaBarra.AtualizaCasos();
                if (incluiu)
                    gerenciador.CargaBarra.cbCasos.SelectedIndex = gerenciador.formDesenho.CasosCarga.Count -2;
                gerenciador.CargaBarra.Valor.Focus();
            }

            if (gerenciador.CargaNodal != null)
            {
                gerenciador.CargaNodal.AtualizaCasos();
                if (incluiu)
                    gerenciador.CargaNodal.cbCasos.SelectedIndex = gerenciador.formDesenho.CasosCarga.Count - 2;
                gerenciador.CargaNodal.Valor.Focus();
            }

            gerenciador.formDesenho.AtualizaCargas();
            gerenciador.formDesenho.Alterou(true);
            gerenciador.cbCasoCarga.SelectedIndex = ind;
            // gerenciador.AtualizaComboCarga();     
        }

        bool alterando, incluiu;

        void AtualizaListBox(int itemSelecionado)
        {
            lbCasos.Items.Clear();
            if (gerenciador.formDesenho.CasosCarga.Count > 0)
                foreach (TCasosCarga c in gerenciador.formDesenho.CasosCarga)
                    lbCasos.Items.Add(c.Nome);

            lbCasos.SelectedIndex = itemSelecionado;
            pg.SelectedObject = gerenciador.formDesenho.CasosCarga[lbCasos.SelectedIndex];
        }

        private void button2_Click(object sender, EventArgs e)
        {
            alterando = false;
            incluiu = true;
            int contagem = gerenciador.formDesenho.CasosCarga.Max(o => o.ID) + 1;
            Color corCaso;
            if (gerenciador.formDesenho.CasosCarga.Count > 0)
            {
                ColorF cor = new ColorF((double)gerenciador.formDesenho.CasosCarga[gerenciador.formDesenho.CasosCarga.Count - 1].Cor.R,
                                        (double)gerenciador.formDesenho.CasosCarga[gerenciador.formDesenho.CasosCarga.Count - 1].Cor.G,
                                        (double)gerenciador.formDesenho.CasosCarga[gerenciador.formDesenho.CasosCarga.Count - 1].Cor.B);
                cor = ColorGenerator.NextGoldenColor(cor);
                corCaso = System.Drawing.Color.FromArgb((int)(cor.R), (int)(cor.G), (int)(cor.B));
            }
            else
                corCaso = System.Drawing.Color.CadetBlue;

            gerenciador.formDesenho.CasosCarga.Add(new TCasosCarga(contagem,"Caso " + (contagem+1).ToString(),"",TipoCasoCarga.Permanente, corCaso, SimNao.Sim));
            AtualizaListBox(gerenciador.formDesenho.CasosCarga.Count-1);
            ultSelecao = gerenciador.formDesenho.CasosCarga.Count - 1;
        }
     
        private void button1_Click(object sender, EventArgs e)
        {
            if (gerenciador.formDesenho.CasosCarga.Count > 0)
            {
                if (gerenciador.formDesenho.CasosCarga[lbCasos.SelectedIndex].ID == 1)
                {
                    MessageBox.Show("Não é possível apagar o caso de carga para peso próprio.", "Atenção", System.Windows.Forms.MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return;
                }

                if (gerenciador.formDesenho.Estrutura.combinacoes.Exists(o => o.Coeficientes.Exists(p => p.caso == gerenciador.formDesenho.CasosCarga[lbCasos.SelectedIndex].ID)))
                {
                    MessageBox.Show("Existem combinações de carga associados com ese caso de carga, portanto não é possível apagar.", "Atenção", System.Windows.Forms.MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return;
                }


                DialogResult dlgresult = MessageBox.Show("Deseja apagar o caso de carga? \n\n As cargas desse caso também serão apagadas."
                                    , "Apagar", MessageBoxButtons.YesNo, MessageBoxIcon.Question);
                    if (dlgresult == DialogResult.Yes)
                    {
                        List<int> tmp = new List<int>();
                        gerenciador.formDesenho.Estrutura.cargaPontual.RemoveAll(x => x.Dados.idCaso == gerenciador.formDesenho.CasosCarga[lbCasos.SelectedIndex].ID);
                        gerenciador.formDesenho.Estrutura.cargaLinear.RemoveAll(x => x.Dados.idCaso == gerenciador.formDesenho.CasosCarga[lbCasos.SelectedIndex].ID);

                        gerenciador.formDesenho.CargasPontuais.RemoveAll(x => x.Dados.idCaso == gerenciador.formDesenho.CasosCarga[lbCasos.SelectedIndex].ID);
                        gerenciador.formDesenho.CargasLineares.RemoveAll(x => x.Dados.idCaso == gerenciador.formDesenho.CasosCarga[lbCasos.SelectedIndex].ID);
                        gerenciador.formDesenho.CasosCarga.RemoveAt(lbCasos.SelectedIndex);

                        Carregar();
                    }                
            }
        }
        int ri;

        private void pg_CausesValidationChanged(object sender, EventArgs e)
        {

        }
        int ultSelecao = 0;

        private void pg_PropertyValueChanged(object s, PropertyValueChangedEventArgs e)
        {
            AtualizaListBox(ultSelecao);
        }

        private void dataGridView1_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {

        }

        private void lbCasos_SelectedIndexChanged(object sender, EventArgs e)
        {

        }

        private void lbCasos_Click(object sender, EventArgs e)
        {
            int ite = lbCasos.SelectedIndex;
            pg.SelectedObject = gerenciador.formDesenho.CasosCarga[ite];
            ultSelecao = ite;
        }
    }
}
