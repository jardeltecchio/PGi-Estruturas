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
    public partial class ChecarConectividades : Form
    {
        Gerenciador gerenciador;
        public ChecarConectividades()
        {
            InitializeComponent();
        }


        public ChecarConectividades(Gerenciador gerenciador_)
        {
            InitializeComponent();
            gerenciador = gerenciador_;
            this.Left = gerenciador.Width - this.Width - 50;
            this.Top = gerenciador.Height / 2 - 50;
            ShowInTaskbar = false;
        }
        private void button2_Click(object sender, EventArgs e)
        {
            
        }

        private void button2_Click_1(object sender, EventArgs e)
        {
            this.DialogResult = System.Windows.Forms.DialogResult.Yes;
        }

        private void button3_Click_1(object sender, EventArgs e)
        {
            
        }

        private void ChecarConectividades_FormClosed(object sender, FormClosedEventArgs e)
        {
            gerenciador.checarconectividade = null;
            
        }

        private void button3_Click(object sender, EventArgs e)
        {
            Close();
        }

        private void btSalvar_Click(object sender, EventArgs e)
        {
            resultado.Text = "<>";
            List<TBarraGenerica> barras = new List<TBarraGenerica>();
            foreach (TBarraGenerica b in gerenciador.formDesenho.Barras)
            {               
                barras.Add(b);
            }

            if (checarConectividade.Checked)
            {
                if (barras.FindAll(o => o.Selecionado == true).Count == 0)
                {
                    MessageBox.Show("Deve-se selecionar um elemento para esta checagem.", "Atenção", MessageBoxButtons.OK, MessageBoxIcon.Stop);
                    return;
                }
                if (barras.FindAll(o => o.Selecionado == true).Count > 1)
                {
                    MessageBox.Show("Deve-se selecionar apenas um elemento por vez para esta checagem.", "Atenção", MessageBoxButtons.OK, MessageBoxIcon.Stop);
                    return;
                }
                //                List<TObjetoDesenho> retorno = TIntersecoes.RetornaConexoes(ref barras);

                List<vec3> pontos = new List<vec3>();
                List<TBarraGenerica> retorno = TIntersecoes.RetornaConexoes2(ref barras,gerenciador.formDesenho.Estrutura.apoios, true, barras.Find(o=>o.Selecionado), ref pontos, false);

                foreach (TBarraGenerica o in retorno)
                {
                    {
                        gerenciador.formDesenho.ObjetosSelecionados.Add(o);
                        o.SetaSelecao(true, true, true);
                        o.realcar = true;
                    }
                }

                if (retorno.Count > 0)
                {
                    resultado.Text = "O objeto selecionado está conectado em " + retorno.Count.ToString() + (retorno.Count > 1 ? " elementos" : " elemento");
                }
                else
                    resultado.Text = "Nenhuma conexão encontrada";

            }
            else
            if (conexoesPerdidas.Checked)
            {
                foreach (TBarraGenerica b in gerenciador.formDesenho.Barras)
                {
                    b.SetaSelecao(false, false);
                }

                double tol = double.Parse(tolerancia.Text);

                List<TPonto> nos = new List<TPonto>();
                foreach (TPonto b in gerenciador.formDesenho.Estrutura.nos)
                {
                    b.SetaSelecao(false,false);
                    nos.Add(b);
                }
                List<TPonto> retorno = TIntersecoes.RetornaNosProximos(ref nos, ref barras, tol);

                foreach (TPonto o in retorno)
                {
                    gerenciador.formDesenho.NosSelecionados.Add(o);
                    o.SetaSelecao(true, true);
                }

                double tol2 = Const.Tol;

                int quantidade = retorno
                    .Select(n => new
                    {
                        x = Math.Round(n.x / tol2),
                        y = Math.Round(n.y / tol2),
                        z = Math.Round(n.z / tol2)
                    })
                    .Distinct()
                    .Count();

                if (quantidade > 0)
                {
                    gerenciador.formDesenho.gerenciador.ConfiguracoesPGi.F3DOpcoesVisualizacao.Nos = true;
                    gerenciador.formDesenho.MostrarNos = true;

                    gerenciador.btMostrarNos.FlatAppearance.BorderColor = Color.Cyan;
                    gerenciador.btMostrarNos.Tag = "0";

                    resultado.Text = quantidade.ToString() + (quantidade > 1 ? " nós encontrados" : " nó encontrado");
                }
                else
                    resultado.Text = "Nenhum nó encontrado";
            }

            gerenciador.formDesenho.AtualizarDesenho(false,true);
            gerenciador.formDesenho.glControl.SwapBuffers();
        }

        private void conexoesPerdidas_CheckedChanged(object sender, EventArgs e)
        {
            
        }

        private void checarConectividade_CheckedChanged(object sender, EventArgs e)
        {
            conexoesPerdidas.Checked = !checarConectividade.Checked;
        }

        private void conexoesPerdidas_CheckedChanged_1(object sender, EventArgs e)
        {
            checarConectividade.Checked = !conexoesPerdidas.Checked;
        }

        private void tolerancia_KeyPress(object sender, KeyPressEventArgs e)
        {
            char a = Convert.ToChar(System.Threading.Thread.CurrentThread.CurrentCulture.NumberFormat.NumberDecimalSeparator);
            if (!char.IsControl(e.KeyChar) && !char.IsDigit(e.KeyChar) && (e.KeyChar != a) && (e.KeyChar != '-'))
            {
                e.Handled = true;
            }

            if (e.KeyChar == (char)Keys.Enter || e.KeyChar == (char)Keys.Escape)
            {
                e.Handled = true;   // stop annoying beep
            }
            base.OnKeyPress(e);
        }
    }
}
