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
    public partial class FDividirElementos : Form
    {
        public FDividirElementos()
        {
            InitializeComponent();
        }
        Gerenciador gerenciador;
        public FDividirElementos(Gerenciador _gerenciador)
        {
            gerenciador = _gerenciador;

            InitializeComponent();

            ShowInTaskbar = false;
        }

        private void FDividirElementos_Load(object sender, EventArgs e)
        {
            this.Left = 25;
            this.Top = (int)((gerenciador.Height - this.Height) / 2);
        }

        private void button3_Click(object sender, EventArgs e)
        {
            Close();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            gerenciador.formDesenho.RemoverDosObjetosSelecionados_Nao_Copiaveis();
            
            if (gerenciador.formDesenho.ObjetosSelecionados.Count > 0)
            {
                gerenciador.formDesenho.ComandoEdicao(Const.ID_DIVIDIR_NAS_INTERSECOES, eTipoComando.edit);
                gerenciador.formDesenho.MouseEdit(0, 0, 0, 0, "", 0);
            }
            else
                gerenciador.formDesenho.ComandoEdicao(Const.ID_DIVIDIR_NAS_INTERSECOES);
        }

        private void FDividirElementos_FormClosed(object sender, FormClosedEventArgs e)
        {
            gerenciador.dividirElementos = null;
        }
    }
}
