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
    public partial class FOpcoesCaptura : Form
    {
        public FOpcoesCaptura()
        {
            InitializeComponent();
        }
        Gerenciador gerenciador;
        public FOpcoesCaptura(Gerenciador _gerenciador)
        {
            InitializeComponent();
            this.gerenciador = _gerenciador;
        }
        private void button3_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void button2_Click(object sender, EventArgs e)
        {
            gerenciador.ConfiguracoesPGi.unidadeDivisaoCotas = cbUnidade.SelectedIndex;
            gerenciador.ConfiguracoesPGi.CorCota = btCorCota.BackColor;

            if (cbUnidade.SelectedIndex == 1)
              gerenciador.ConfiguracoesPGi.divisaoCotas = (double)DivBarrasCotas.Value/100;
            else
            if (cbUnidade.SelectedIndex == 2)
                gerenciador.ConfiguracoesPGi.divisaoCotas = (double)DivBarrasCotas.Value / 1000;
            // double distCota = conv.comp((double)DivBarrasCotas.Value, gerenciador.ConfiguracoesPGi.CfgProjeto.unidadesProjeto.geo_un_comprimento.ToString(), und.m);

            //  conv.comp(distCota1, und.m, gerenciador.ConfiguracoesPGi.CfgProjeto.unidadesProjeto.geo_un_comprimento.ToString()).ToString("n" + gerenciador.ConfiguracoesPGi.CfgProjeto.unidadesProjeto.geo_casas.ToString()), fonte, Color.MediumBlue);

              gerenciador.formDesenho.divCotas = gerenciador.ConfiguracoesPGi.divisaoCotas;
            gerenciador.formDesenho.CorCota = gerenciador.ConfiguracoesPGi.CorCota;
            this.Close();
        }

        private void button5_Click_1(object sender, EventArgs e)
        {
            Angulos.Items.Add(OutroAngulo.Text);
        }

        private void button6_Click_1(object sender, EventArgs e)
        {
            Angulos.Items.Remove(Angulos.SelectedItem);
        }

        private void OpcoesCaptura_Load(object sender, EventArgs e)
        {
            cbUnidade.SelectedIndex = gerenciador.ConfiguracoesPGi.unidadeDivisaoCotas;
            int cota = 0;
            
            btCorCota.BackColor = gerenciador.ConfiguracoesPGi.CorCota;

            if (cbUnidade.SelectedIndex == 1)
              cota=  (int)(gerenciador.ConfiguracoesPGi.divisaoCotas * 100);
            else
            if (cbUnidade.SelectedIndex == 2)
              cota = (int)(gerenciador.ConfiguracoesPGi.divisaoCotas * 1000);

            DivBarrasCotas.Value = cota;
        }

        private void DivBarrasCotas_ValueChanged(object sender, EventArgs e)
        {

        }

        private void FOpcoesCaptura_FormClosed(object sender, FormClosedEventArgs e)
        {
            gerenciador.OpcoesCaptura = null;
        }

        private void btCorCima_Click(object sender, EventArgs e)
        {
            if (colorDialog1.ShowDialog() == DialogResult.OK)
                btCorCota.BackColor = colorDialog1.Color;
        }
    }
}
