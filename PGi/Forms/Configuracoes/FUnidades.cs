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
    [Serializable]
    public struct SUnidadesd
    {
        public string Forca, Comprimento, Armadura, Secao;
        public SUnidadesd(bool padrao = true)
        {
            Forca       = und.kN;
            Comprimento = und.m;
            Armadura    = und.mm;
            Secao       = und.cm;
        }
    }

    public partial class FUnidades : Form
    {
        Gerenciador gerenciador;
        public FUnidades()
        {
            InitializeComponent();
        }
        public FUnidades(Gerenciador gerenciador)
        {
            this.gerenciador = gerenciador;
    
            InitializeComponent();
        }

        private void button3_Click(object sender, EventArgs e)
        {
            this.DialogResult = System.Windows.Forms.DialogResult.Cancel;
        }

        void GravaDados()
        {
           /* gerenciador.ConfiguracoesPGi.Unidades.Armadura    = cbArmadura.Text;
            gerenciador.ConfiguracoesPGi.Unidades.Forca       = cbForca.Text;
            gerenciador.ConfiguracoesPGi.Unidades.Comprimento = cbComp.Text;
            gerenciador.ConfiguracoesPGi.Unidades.Secao       = cbSecao.Text;*/
        }

        void Carrega()
        {
           /* cbArmadura.Text = gerenciador.ConfiguracoesPGi.Unidades.Armadura;
            cbForca.Text    = gerenciador.ConfiguracoesPGi.Unidades.Forca;
            cbComp.Text     = gerenciador.ConfiguracoesPGi.Unidades.Comprimento;
            cbSecao.Text    = gerenciador.ConfiguracoesPGi.Unidades.Secao;*/
        }

        private void button1_Click(object sender, EventArgs e)
        {
            GravaDados();
            this.DialogResult = System.Windows.Forms.DialogResult.OK;
        }

        private void FUnidades_SizeChanged(object sender, EventArgs e)
        {

        }

        private void FUnidades_Shown(object sender, EventArgs e)
        {
            Carrega();
        }   
    }
}
