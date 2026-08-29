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
   
    public partial class FNovoPavimento : Form
    {
        Gerenciador gerenciador;
        bool novo;
        public FNovoPavimento()
        {
            InitializeComponent();
        }
        bool teste;
        public FNovoPavimento(Gerenciador gerenciador, bool Novo)
        {
            InitializeComponent();
            this.gerenciador = gerenciador;
            this.novo = Novo;

            if (gerenciador.Pavimentos.Count > 0 && !Novo)
            {
                foreach (TPavimento pav in gerenciador.Pavimentos)
                {
                    lb.Items.Add(pav.Descricao);
                    lbPeDireito.Items.Add(pav.PeDireito);
                    lbNivel.Items.Add(pav.Nivel);
                    lbSequencia.Items.Add(pav.SequenciaPavimento);
                }

                edNomeProjeto.Text      = gerenciador.ConfiguracoesPGi.NomeProjeto;
                edDescricaoProjeto.Text = gerenciador.ConfiguracoesPGi.DescricaoProjeto;
                edCotaFundacao.Text = gerenciador.ConfiguracoesPGi.CotaFundacao.ToString();

                edRepeticoes.Text = "";
                edPeDireito.Text  = "";
            }
            else
            {
                lb.Items.Add("Fundação");
                lbPeDireito.Items.Add("0");
                lbNivel.Items.Add("0");

                lb.SelectedIndex = 0;
                lbPeDireito.SelectedIndex = 0;
                lbNivel.SelectedIndex = 0;
            }

        }

        private void Atualiza()
        {
            lb.Items.Clear();
            foreach (TPavimento pav in gerenciador.Pavimentos)
               lb.Items.Add(pav.Descricao);
        }

        int ii;

        private void button7_Click(object sender, EventArgs e)
        {
            this.DialogResult = System.Windows.Forms.DialogResult.No;
            this.Close();
        }

        private void edNome_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (e.KeyChar == (char)Keys.Enter || e.KeyChar == (char)Keys.Escape)
            {
                e.Handled = true;   // stop annoying beep
            }

            // call base handler...
            base.OnKeyPress(e);
        }

        private void edNome_KeyDown(object sender, KeyEventArgs e)
        {
             if (e.KeyData == Keys.Enter)
             {
                 edPeDireito.Focus();
             }
        }

        private void edPeDireito_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyData == Keys.Enter)
            {
                edRepeticoes.Focus();
            }

        }

        private void edRepeticoes_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyData == Keys.Enter)
            {
               btGravar_Click(sender, null); 
            }

        }

        private void btGravar_Click(object sender, EventArgs e)
        {
            try
            {
                try
                {
                    string descricao;

                    if ((edNome.ToString() != "") && (edPeDireito.ToString() != ""))
                    {
                        int repeticoes = int.Parse(edRepeticoes.Text);

                        int ii = lb.SelectedIndex;

                        for (int i = 0; i < repeticoes; i++)
                        {

                            //gerenciador.Pavimentos.Insert(ii, new TPavimento(edNome.Text, double.Parse(edPeDireito.Text),(int)edRepeticoes.Value));

                            descricao = edNome.Text;

                            if (i > 0)
                            {
                                lb.Items.Insert(i + ii, descricao);
                                lbPeDireito.Items.Insert(i + ii, edPeDireito.Text.Trim());
                                lbNivel.Items.Insert(i + ii, edPeDireito.Text.Trim());
                                lbSequencia.Items.Insert(i + ii, "");
                            }
                            else
                            {
                                lb.Items[ii] = descricao;
                                lbPeDireito.Items[ii] = edPeDireito.Text.Trim();
                                lbNivel.Items[ii] = edPeDireito.Text.Trim();
                                lbSequencia.Items[ii] = lbSequencia.Text.Trim();
                            }
                        }

                        double nivelAnt = double.Parse(edCotaFundacao.Text.ToString());
                        double nivel;
                        for (int i = lbPeDireito.Items.Count - 1; i >= 0; i--)
                        {
                            nivel = nivelAnt + double.Parse(lbPeDireito.Items[i].ToString());
                            lbNivel.Items[i] = nivel.ToString();
                            nivelAnt = nivel;
                        }

                        btInsereAcima.Focus();
                        edNome.Clear();
                        edRepeticoes.Text = "1";
                        lb.SelectedIndex = ii;
                        lbPeDireito.SelectedIndex = ii;
                        lbSequencia.SelectedIndex = ii;
                    }
                }

                catch (System.FormatException)
                {
                    MessageBox.Show("Use apenas números na altura ou na quantidade de repetições!");
                }
            }

            catch (Exception ee)
            {
                MessageBox.Show(ee.Message);
            }
        }

        private void button6_Click(object sender, EventArgs e)
        {
        }

        private void button4_Click(object sender, EventArgs e)
        {
            int i =lb.SelectedIndex;

         //   Pavimentos.Insert(i, new TPavimento("", 0));
           // Pavimentos.
     //       lb.Items.Insert(i, lb.Items[lb.SelectedIndex - 1].ToString());
      //      lb.Items.RemoveAt(lb.SelectedIndex - 1);
            
            Atualiza();
           // lb.Items.Insert(lb.SelectedIndex, lb.Items[lb.SelectedIndex].ToString());
        }

        private void edPeDireito_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (e.KeyChar == (char)Keys.Enter || e.KeyChar == (char)Keys.Escape)
            {
                e.Handled = true;   // stop annoying beep
            }

            // call base handler...
            base.OnKeyPress(e);
        }

        private void edRepeticoes_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (e.KeyChar == (char)Keys.Enter || e.KeyChar == (char)Keys.Escape)
            {
                e.Handled = true;   // stop annoying beep
            }

            // call base handler...
            base.OnKeyPress(e);
        }

        private void button3_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (e.KeyChar == (char)Keys.Enter || e.KeyChar == (char)Keys.Escape)
            {
                e.Handled = true;   // stop annoying beep
            }

            // call base handler...
            base.OnKeyPress(e);
        }

        private void btInsereAcima_Click(object sender, EventArgs e)
        {
            if (lb.SelectedIndex == -1) return;
            ii = lb.SelectedIndex;//lb.Items.IndexOf(lb.SelectedItem.ToString());

            //       gerenciador.Pavimentos.Insert(ii, new TPavimento("", 0,1));

            lb.Items.Insert(ii, "");
            lbPeDireito.Items.Insert(ii, "");
            lbNivel.Items.Insert(ii, "");
            lbSequencia.Items.Insert(ii, "");

            //Atualiza();

            //    lb.Items.Insert(ii,"");
            lb.SelectedIndex = ii;
            lbPeDireito.SelectedIndex = ii;
            lbNivel.SelectedIndex = ii;
            lbSequencia.SelectedIndex = ii;

            HabilitaAlturaeNome();
        //    edPeDireito.Text = lbPeDireito.Items[ii+1].ToString();
        }

        void HabilitaAlturaeNome()
        {
            edNome.Focus();
            edNome.BackColor = Color.White;
            edNome.ReadOnly = false;
            edPeDireito.BackColor = Color.White;
            edPeDireito.ReadOnly = false;
            btGravar.Enabled = true;
            btExcluir.Enabled = true;
            btInsereAbaixo.Enabled = true;
        }
        private void btInsereAbaixo_Click(object sender, EventArgs e)
        {
            if (lb.SelectedIndex == -1) return;
            ii = lb.SelectedIndex;// lb.Items.IndexOf(lb.SelectedItem.ToString());

            //gerenciador.Pavimentos.Insert(ii + 1, new TPavimento("", 0,1));

            lb.Items.Insert(ii + 1, "");
            lbPeDireito.Items.Insert(ii + 1, "");
            lbNivel.Items.Insert(ii + 1, "");
            lbSequencia.Items.Insert(ii + 1, "");
            // Atualiza();

            //    lb.Items.Insert(ii,"");
            lb.SelectedIndex = ii + 1;
            lbPeDireito.SelectedIndex = ii + 1;
            lbNivel.SelectedIndex = ii + 1;
            lbSequencia.SelectedIndex = ii + 1;
            edNome.Focus();

            HabilitaAlturaeNome();
        }

        private void btExcluir_Click(object sender, EventArgs e)
        {
            if (lb.SelectedIndex == -1) return;
            lb.Items.RemoveAt(lb.SelectedIndex);
            lbPeDireito.Items.RemoveAt(lbPeDireito.SelectedIndex);
            lbNivel.Items.RemoveAt(lbNivel.SelectedIndex);
            lbSequencia.Items.RemoveAt(lbSequencia.SelectedIndex);
            lbSequencia.SelectedIndex = 0;
        }

        private void edNomeProjeto_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyData == Keys.Enter)
            {
                edDescricaoProjeto.Focus();
            }
        }

        private void edNomeProjeto_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (e.KeyChar == (char)Keys.Enter || e.KeyChar == (char)Keys.Escape)
            {
                e.Handled = true;   // stop annoying beep
            }

            // call base handler...
            base.OnKeyPress(e);

        }

        private void btMoverCima_Click(object sender, EventArgs e)
        {
            if (lb.SelectedIndex == -1) return;
            string anterior = lb.Items[lb.SelectedIndex - 1].ToString();
            string atual    = lb.Items[lb.SelectedIndex].ToString();

            lb.Items[lb.SelectedIndex - 1] = atual;
            lb.Items[lb.SelectedIndex]     = anterior;
            lb.SelectedIndex -=1;

            lbPeDireito.Items[lbPeDireito.SelectedIndex - 1] = atual;
            lbPeDireito.Items[lbPeDireito.SelectedIndex] = anterior;
            lbPeDireito.SelectedIndex -= 1;

            lbNivel.Items[lbNivel.SelectedIndex - 1] = atual;
            lbNivel.Items[lbNivel.SelectedIndex] = anterior;
            lbNivel.SelectedIndex -= 1;

            lbSequencia.Items[lbSequencia.SelectedIndex - 1] = atual;
            lbSequencia.Items[lbSequencia.SelectedIndex] = anterior;
            lbSequencia.SelectedIndex -= 1;

            HabilitaAlturaeNome();
        }

        private void btMoverBaixo_Click(object sender, EventArgs e)
        {
            if (lb.SelectedIndex == -1) return;
            string proximo = lb.Items[lb.SelectedIndex + 1].ToString();
            string atual    = lb.Items[lb.SelectedIndex].ToString();

            lb.Items[lb.SelectedIndex + 1] = atual;
            lb.Items[lb.SelectedIndex] = proximo;
            lb.SelectedIndex += 1;

            lbPeDireito.Items[lbPeDireito.SelectedIndex + 1] = atual;
            lbPeDireito.Items[lbPeDireito.SelectedIndex] = proximo;
            lbPeDireito.SelectedIndex += 1;

            lbNivel.Items[lbNivel.SelectedIndex + 1] = atual;
            lbNivel.Items[lbNivel.SelectedIndex] = proximo;
            lbNivel.SelectedIndex += 1;

            lbSequencia.Items[lbSequencia.SelectedIndex + 1] = atual;
            lbSequencia.Items[lbSequencia.SelectedIndex] = proximo;
            lbSequencia.SelectedIndex += 1;

            HabilitaAlturaeNome();
        }

        private void lb_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (lb.SelectedIndex == lb.Items.Count - 1 || lb.SelectedIndex == lb.Items.Count - 2)
              btMoverBaixo.Enabled = false;
            else
              btMoverBaixo.Enabled = true;

            if (lb.SelectedIndex == 0 || lb.SelectedIndex == lb.Items.Count - 1)
            {
                btMoverCima.Enabled = false;
            }
            else
            {
                btMoverCima.Enabled = true;
            }
        }
        void carrega()
        {
            string n = lb.Items[lb.SelectedIndex].ToString();
            n = n.ToUpper();
            if (!n.Contains("FUNDA"))
            {
              edNome.Text = lb.Items[lb.SelectedIndex].ToString();
              edPeDireito.Text = lbPeDireito.Items[lbPeDireito.SelectedIndex].ToString();
              edRepeticoes.Text = "1";
              btGravar.Enabled = true;
              btExcluir.Enabled = true;
              btInsereAbaixo.Enabled = true;
            }
            else
            {
                edNome.Clear();
                edPeDireito.Clear();
                edRepeticoes.Value = 1;
                btGravar.Enabled = false;
                btExcluir.Enabled = false;
                btInsereAbaixo.Enabled = false;
            }

        }

        private void lb_MouseDoubleClick(object sender, MouseEventArgs e)
        {
           
       //     if (lb.Items[lb.SelectedIndex].ToString().Contains("["))
          //  {

       //     }
         //   else
         //   {
         //       edNome.Text = lb.Items[lb.SelectedIndex].ToString();
         //       edPeDireito.Text = "0";
         //   }

        }

        private void FNovoPavimento_Shown(object sender, EventArgs e)
        {
            if (novo)
            {
       /*         btInsereAcima_Click(btInsereAcima, null);
                edNome.Text = "pav 1";
                edPeDireito.Text = "300";
                btGravar_Click(btGravar, null);

                btInsereAcima_Click(btInsereAcima, null);
                edNome.Text = "pav 2";
                edPeDireito.Text = "300";
                btGravar_Click(btGravar, null);*/

                button6_Click(button6, null);
            }
        }

        private void lb_Click(object sender, EventArgs e)
        {
            if (lb.SelectedIndex == -1) return;
            lbPeDireito.SelectedIndex = lb.SelectedIndex;
            lbNivel.SelectedIndex = lb.SelectedIndex;
            lbSequencia.SelectedIndex = lb.SelectedIndex;
            carrega();
        }

        private void lbPeDireito_Click(object sender, EventArgs e)
        {
            if (lbPeDireito.SelectedIndex == -1) return;
            lb.SelectedIndex = lbPeDireito.SelectedIndex;
            lbNivel.SelectedIndex = lbPeDireito.SelectedIndex;
            lbSequencia.SelectedIndex = lb.SelectedIndex;
            carrega();
        }

        private void lbNivel_Click(object sender, EventArgs e)
        {
            if (lbNivel.SelectedIndex == -1) return;
            lb.SelectedIndex = lbNivel.SelectedIndex;
            lbPeDireito.SelectedIndex = lbNivel.SelectedIndex;
            lbSequencia.SelectedIndex = lbNivel.SelectedIndex;
            carrega();
        }

        private void lbPeDireito_MouseDoubleClick(object sender, MouseEventArgs e)
        {
    
        }

        private void lbNivel_MouseDoubleClick(object sender, MouseEventArgs e)
        {

        }

        private void lbNivel_RightToLeftChanged(object sender, EventArgs e)
        {

        }

        private void lbSequencia_Click(object sender, EventArgs e)
        {
            if (lbSequencia.SelectedIndex == -1) return;
            lb.SelectedIndex = lbSequencia.SelectedIndex;
            lbNivel.SelectedIndex = lbSequencia.SelectedIndex;
            lbPeDireito.SelectedIndex = lbSequencia.SelectedIndex;
            carrega();
        }

        private void groupBox1_Enter(object sender, EventArgs e)
        {

        }

        private void edCotaFundacao_KeyPress(object sender, KeyPressEventArgs e)
        {

        }

        private void edCotaFundacao_Leave(object sender, EventArgs e)
        {
            double nivelAnt = double.Parse(edCotaFundacao.Text.ToString());
            double nivel;
            for (int i = lbPeDireito.Items.Count - 1; i >= 0; i--)
            {
                nivel = nivelAnt + double.Parse(lbPeDireito.Items[i].ToString());
                lbNivel.Items[i] = nivel.ToString();
                nivelAnt = nivel;
            }
        }

        private void FNovoPavimento_Move(object sender, EventArgs e)
        {
            gerenciador.AtualizaDesenho();
        
        }

    }
}
