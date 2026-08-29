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
    public struct SConfiguracaoGrelha1
    {
        public int AnguloBarras, EspacamentoX, EspacamentoY;
        public bool GerarNovaGrelha, ConsiderarGrelhaEditada;
        public SConfiguracaoGrelha1(bool padrao = true)
        {
            AnguloBarras = 0;
            EspacamentoX = 35;
            EspacamentoY = 35;
            GerarNovaGrelha = true;
            ConsiderarGrelhaEditada = false;
        }

        public SConfiguracaoGrelha1(int angulo, int espx, int espy, bool gerarnova, bool consideraredicao)
        {
            AnguloBarras = angulo;
            EspacamentoX = espx;
            EspacamentoY = espy;
            GerarNovaGrelha = gerarnova;
            ConsiderarGrelhaEditada = consideraredicao;
        }

        public SConfiguracaoGrelha1 Clone()
        {
            SConfiguracaoGrelha1 s = new SConfiguracaoGrelha1(this.AnguloBarras, this.EspacamentoX,this.EspacamentoY,this.GerarNovaGrelha,this.ConsiderarGrelhaEditada);
            return s;
        }
    }

    public partial class FGrelhaConfiguracao : Form
    {
        Gerenciador gerenciador;
        bool carregando;
        void LerDados()
        {
            carregando = true;
            edAnguloBarras.Value = gerenciador.Pavimentos[lbPavimentos.SelectedIndex].ConfiguracaGrelha.AnguloBarras;
            edEspacamentoX.Value = gerenciador.Pavimentos[lbPavimentos.SelectedIndex].ConfiguracaGrelha.EspacamentoX;
            edEspacamentoY.Value = gerenciador.Pavimentos[lbPavimentos.SelectedIndex].ConfiguracaGrelha.EspacamentoY;
            rbGerarNovaGrelha.Checked = gerenciador.Pavimentos[lbPavimentos.SelectedIndex].ConfiguracaGrelha.GerarNovaGrelha;
            rbConsiderarGrelhaEditada.Checked = gerenciador.Pavimentos[lbPavimentos.SelectedIndex].ConfiguracaGrelha.ConsiderarGrelhaEditada;

            carregando = false;
        }

        void GravarDados(int piso)
        {
            gerenciador.Pavimentos[piso].ConfiguracaGrelha.AnguloBarras = (int)edAnguloBarras.Value;
            gerenciador.Pavimentos[piso].ConfiguracaGrelha.EspacamentoX = (int)edEspacamentoX.Value;
            gerenciador.Pavimentos[piso].ConfiguracaGrelha.EspacamentoY = (int)edEspacamentoY.Value;
            gerenciador.Pavimentos[piso].ConfiguracaGrelha.GerarNovaGrelha = rbGerarNovaGrelha.Checked;
            gerenciador.Pavimentos[piso].ConfiguracaGrelha.ConsiderarGrelhaEditada = rbConsiderarGrelhaEditada.Checked;
        }

  
        public FGrelhaConfiguracao()
        {
            InitializeComponent();
        }
        public FGrelhaConfiguracao(Gerenciador gerenciador)
        {
            InitializeComponent();
            this.gerenciador = gerenciador;

            if (gerenciador.Pavimentos.Count > 0)
            {
                foreach (TPavimento pav in gerenciador.Pavimentos)
                    lbPavimentos.Items.Add(pav.Descricao);
            }
            lbPavimentos.SelectedIndex = 0;
        }

        private void button1_Click(object sender, EventArgs e)
        {
            GravarDados(lbPavimentos.SelectedIndex);
         //   this.DialogResult = DialogResult.OK;
        }

        private void button3_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void FGrelhaConfiguracao_Load(object sender, EventArgs e)
        {
            LerDados();
        }

        private void groupBox3_Enter(object sender, EventArgs e)
        {

        }
        int PavAnterior;
        private void lbPavimentos_SelectedIndexChanged(object sender, EventArgs e)
        {
            labelPiso.Text = "Piso: " + lbPavimentos.Items[lbPavimentos.SelectedIndex];
        }

        private void checkBox1_CheckedChanged(object sender, EventArgs e)
        {

        }

        private void edEspacamentoX_ValueChanged(object sender, EventArgs e)
        {
            if (!carregando)
                GravarDados(lbPavimentos.SelectedIndex);
        }

        private void edEspacamentoY_ValueChanged(object sender, EventArgs e)
        {
            if (!carregando)
                GravarDados(lbPavimentos.SelectedIndex);
        }

        private void edAnguloBarras_ValueChanged(object sender, EventArgs e)
        {
            if (!carregando)
                GravarDados(lbPavimentos.SelectedIndex);
        }

        private void lbPavimentos_Click(object sender, EventArgs e)
        {
            LerDados();

         //   if (PavAnterior != lbPavimentos.SelectedIndex)
       //       GravarDados(PavAnterior);

            PavAnterior = lbPavimentos.SelectedIndex;
        }

        private void button2_Click(object sender, EventArgs e)
        {
            foreach (TPavimento pav in gerenciador.Pavimentos)
            {
                pav.ConfiguracaGrelha.EspacamentoX = (int)edEspacamentoX.Value;
                pav.ConfiguracaGrelha.EspacamentoY = (int)edEspacamentoY.Value;
                pav.ConfiguracaGrelha.AnguloBarras = (int)edAnguloBarras.Value;
            }
        }

        private void rbGerarNovaGrelha_CheckedChanged(object sender, EventArgs e)
        {
            gbDiscretizacao.Enabled = true;
            if (!carregando)
                GravarDados(lbPavimentos.SelectedIndex); 
        }

        private void rbConsiderarGrelhaEditada_CheckedChanged(object sender, EventArgs e)
        {
            gbDiscretizacao.Enabled = false;
            if (!carregando)
                GravarDados(lbPavimentos.SelectedIndex); 
        }
    }
}
