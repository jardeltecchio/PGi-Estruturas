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
    public partial class FNovoNo : Form
    {
        FVisualizadorPortico owner;
        public FNovoNo(FVisualizadorPortico ow)
        {
            InitializeComponent();
            this.owner = ow;
        }

        public FNovoNo()
        {
            InitializeComponent();
        }
        
        private void FNovoNo_Load(object sender, EventArgs e)
        {

        }

        private void FNovoNo_Shown(object sender, EventArgs e)
        {
            this.Left = 10;
            this.Top = 200;
        }

        private void edb1_KeyPress(object sender, KeyPressEventArgs e)
        {
            //e.Handled = (!(e.KeyChar == 44) && !char.IsDigit(e.KeyChar) && !char.IsControl(e.KeyChar));
        }

        private void btSalvar_Click(object sender, EventArgs e)
        {
            try
            {
                int max_no = owner.Portico.nNos;
                int vinculo = 0;
                if (Engaste.Checked) vinculo = 2;
                if (Apoio.Checked) vinculo = 1;

                if (owner.LocNo(double.Parse(edx.Text), double.Parse(edy.Text), double.Parse(edz.Text)) != null)
                {
                    MessageBox.Show("Já existe um nó nessa posição", "Atenção", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                    return;
                }

                owner.Portico.nos[++max_no] = new TNoPortico(double.Parse(edx.Text),
                                                             double.Parse(edy.Text),
                                                             double.Parse(edz.Text),
                                                             0, 0, vinculo, max_no + 1, null, null);
                owner.Portico.nNos++;
                owner.Render();
                owner.Controle.SwapBuffers();
            }
            catch(Exception)
            {

            }
        }

        private void button3_Click(object sender, EventArgs e)
        {
            owner.CancelaOperacoes();
            Close();
        }

        private void FNovoNo_FormClosed(object sender, FormClosedEventArgs e)
        {
            owner.fNovoNo = null;
        }
    }
}
