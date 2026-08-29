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
    public partial class FMoverCopiar : Form
    {
        public FMoverCopiar()
        {
            InitializeComponent();
        }

        Gerenciador owner;
        public FMoverCopiar(Gerenciador ow)
        {
            InitializeComponent();
            owner = ow;
            edDx.BackColor = Color.Gray;
            edDy.BackColor = Color.Gray;
            edDz.BackColor = Color.Gray;
            edDx.ReadOnly = true;
            edDx.ReadOnly = true;
            edDx.ReadOnly = true;

        }

        private void button2_Click(object sender, EventArgs e)
        {
            this.Close();
        }
        public TPonto pontoRef = new TPonto(0);
        private void edDx_TextChanged(object sender, EventArgs e)
        {
            if (chManual.Checked)
            {
                pontoRef.x = (owner.formDesenho.FerramentaEdicao as TCopiarElementos).ponto1.x + System.Convert.ToSingle(edDx.Text);
                pontoRef.y = (owner.formDesenho.FerramentaEdicao as TCopiarElementos).ponto1.y + System.Convert.ToSingle(edDy.Text);
                pontoRef.z = (owner.formDesenho.FerramentaEdicao as TCopiarElementos).ponto1.z + (System.Convert.ToSingle(edDz.Text)*-1);
                owner.AtualizaDesenho();
            }
        }

        private void checkBox1_CheckedChanged(object sender, EventArgs e)
        {
            if (chManual.Checked)
            {
                edDx.BackColor = Color.White;
                edDy.BackColor = Color.White;
                edDz.BackColor = Color.White;
                edDx.ReadOnly = false;
                edDx.ReadOnly = false;
                edDx.ReadOnly = false;
            }
            else
            {
                edDx.BackColor = Color.Gray;
                edDy.BackColor = Color.Gray;
                edDz.BackColor = Color.Gray;
                edDx.ReadOnly = true;
                edDx.ReadOnly = true;
                edDx.ReadOnly = true;
            }

        }

        private void button1_Click(object sender, EventArgs e)
        {
            owner.formDesenho.MouseEdit(pontoRef.x, pontoRef.y, pontoRef.z,0,"",0);
        }
    }
}
