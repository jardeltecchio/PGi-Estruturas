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
    public partial class FConfiguracaoPrograma : Form
    {
        public FConfiguracaoPrograma()
        {
            InitializeComponent();
        }

        public FConfiguracaoPrograma(Gerenciador _gerenciador)
        {
            this.gerenciador = _gerenciador;
            InitializeComponent();
            ShowInTaskbar = false;
        }
        Gerenciador gerenciador;
        void GravaItemCheckBox(CheckBox ch, string item )
        {
            string linha;
            if (ch.Checked)
                linha = item +"=S";
            else
                linha = item + "=N";

            sb.AppendLine(linha);
        }
        StringBuilder sb;
        private void button1_Click(object sender, EventArgs e)
        {
            try
            {
                sb = new StringBuilder();

                GravaItemCheckBox(chSuavizacao, "SUAVIZACAO_OPENGL");
                GravaItemCheckBox(cbIrParaResultados, "PAGINA_RESULTADOS");
                GravaItemCheckBox(cbFecharJanelaMensagensAposCalculo,"FECHAR_JANELA_MENSAGENS_APOS_CALCULO");
                GravaItemCheckBox(chAbrirUltimoProjeto, "ABRIR_ULTIMO_PROJETO");

                string path = System.IO.Path.GetDirectoryName(Application.ExecutablePath) + "\\preferencias_programa.txt";
                using (System.IO.StreamWriter w = new System.IO.StreamWriter(path, false))
                {
                    w.Write(sb.ToString());
                }

                gerenciador.CarregaConfiguracaoPrograma();

                Close();
            }
            catch (Exception ee)
            {
                
            }
        }

        private void button3_Click(object sender, EventArgs e)
        {
            Close();
        }

        private void FConfiguracaoPrograma_Move(object sender, EventArgs e)
        {
            gerenciador.AtualizaDesenho();
        }

        private void FConfiguracaoPrograma_FormClosed(object sender, FormClosedEventArgs e)
        {
            gerenciador.fConfiguracaoPrograma = null;
        }
        bool carregando = true;
        void Carregar()
        {
         /*   string arq = System.IO.Path.GetDirectoryName(Application.ExecutablePath) + "\\preferencias_programa.txt";
            string item, texto, linha, suavizacao;
      

            linha = System.IO.File.ReadLines(arq).ToString();
            carregando = true;
            foreach (string line in System.IO.File.ReadLines(arq))
            {         
                if (line.Trim() == string.Empty)
                    break;
                linha = line.Trim();
                texto = linha.Substring(0, linha.IndexOf("=") + 1);
                item = linha.Substring(linha.IndexOf("=") + 1, linha.Length - texto.Length);
                suavizacao = item;

                if (item == "S")
                    chSuavizacao.Checked = true;
                else
                    chSuavizacao.Checked = false;
            }*/
            carregando = true;
            chSuavizacao.Checked = gerenciador.ConfiguracaoPrograma.SuavizacaoOpenGl;
            chAbrirUltimoProjeto.Checked = gerenciador.ConfiguracaoPrograma.AbrirUltimoProjeto;
            cbIrParaResultados.Checked = gerenciador.ConfiguracaoPrograma.IrParaPaginaResultados;
            cbFecharJanelaMensagensAposCalculo.Checked = gerenciador.ConfiguracaoPrograma.FecharJanelaResultadosAposCalculo;
            carregando = false;
        }

        private void FConfiguracaoPrograma_Load(object sender, EventArgs e)
        {
            Carregar();
            tabControl1.TabIndex = 0;
        }

        private void chSuavizacao_CheckedChanged(object sender, EventArgs e)
        {
            if (!carregando)
             MessageBox.Show("O programa deve ser reiniciado para habilitar/desabilitar essa opção.");
        }
    }
}
