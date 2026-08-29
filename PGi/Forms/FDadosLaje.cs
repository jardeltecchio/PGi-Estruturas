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

    public partial class DadosLaje : Form
    {
        Gerenciador gerenciador;

        float fatorZoom, precisaoPixel, angulo, angulo2;
        float[] ponto_zero;

        TDadosLaje Dados;
        public SConfiguracaoGrelha ConfigGrelha;
        int w, h;
        Point Posicao;
        public bool alterando = false;
        public TSecao secao;
        int tipo;
        public DadosLaje()
        {
            InitializeComponent();
        }

        public DadosLaje(Gerenciador gerenciador, TDadosLaje Dados, int indiceTipo)
        {
            InitializeComponent();
            this.gerenciador = gerenciador;

            this.Dados = Dados;
            tipo = indiceTipo;
            ConfigGrelha = new SConfiguracaoGrelha(true); 
         }
        bool carregando;
        void Carregar()
        {
            carregando = true;
            NumLaje.Value = Dados.numero;
            NomeLaje.Text = Dados.nome;
            chGerarGrelha.Checked = Dados.GerarGrelha;
            rbGrelhaEspecifica.Checked = Dados.GerarGrelhaEspecifica;
            rbGrelhaPavimento.Checked  = !Dados.GerarGrelhaEspecifica;
            CargaAcidental.Text  = Dados.CargaAcidental.ToString();
            CargaPermanente.Text = Dados.CargaPermanente.ToString();
            if (rbGrelhaEspecifica.Checked)
            {
                edAnguloBarras.Value = Dados.ConfiguracaGrelha.AnguloBarras;
                edEspacamentoX.Value = Dados.ConfiguracaGrelha.EspacamentoX;
                edEspacamentoY.Value = Dados.ConfiguracaGrelha.EspacamentoY;
            }

            edh1.Text = Dados.h.ToString();
            cbTipo.SelectedIndex = Dados.indiceTipo;
            carregando = false;
        }

        private void DadosLaje_FormClosing(object sender, FormClosingEventArgs e)
        {
        
        }

        private void DadosLaje_Load(object sender, EventArgs e)
        {
       
            fatorZoom = 1.2f;

            ponto_zero = new float[2];

            ponto_zero[0] = 0;
            ponto_zero[1] = h;

            precisaoPixel = 0.8f;
            angulo        = 0;


            cbTipo.SelectedIndex = 0;

            if (Dados != null)
              Carregar();

            this.Left = 25;
        }

        public float pixelX(double coordX)
        {
            return System.Convert.ToSingle(coordX / precisaoPixel + ponto_zero[0]);
        }

        public float pixelY(double coordY)
        {
            return System.Convert.ToSingle(coordY / precisaoPixel * -1 + ponto_zero[1]);
        }

        private void button3_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void button2_Click(object sender, EventArgs e)
        {
            if (alterando)
            {
                GravaDados(); 
                this.DialogResult = System.Windows.Forms.DialogResult.Yes;
            }
            else
                gerenciador.NovosDadosDeLaje();
        }

        public void GravaDados()
        {
            try
            {
                Dados.h = System.Convert.ToDouble(edh1.Text);
                Dados.nome = NomeLaje.Text;
                Dados.numero = System.Convert.ToInt32(NumLaje.Text);
                Dados.indiceTipo = cbTipo.SelectedIndex;
                Dados.ConfiguracaGrelha = ConfigGrelha;
                Dados.GerarGrelha = chGerarGrelha.Checked;
                Dados.GerarGrelhaEspecifica = rbGrelhaEspecifica.Checked;
                Dados.CargaPermanente = System.Convert.ToDouble(CargaPermanente.Text);
                Dados.CargaAcidental = System.Convert.ToDouble(CargaAcidental.Text);
            }
            catch(Exception  e)
            {
                MessageBox.Show(e.Message);
            }
        }

        private void Controle_MouseWheel(object sender, System.Windows.Forms.MouseEventArgs e)
        {
            if (e.Delta > 0)
            {
                ponto_zero[0] = Posicao.X + ((ponto_zero[0] - Posicao.X) * fatorZoom);
                ponto_zero[1] = Posicao.Y + ((ponto_zero[1] - Posicao.Y) * fatorZoom);
                precisaoPixel = precisaoPixel / fatorZoom;
                angulo += 1;
            }
            else
            {
                ponto_zero[0] = Posicao.X + ((ponto_zero[0] - Posicao.X) / fatorZoom);
                ponto_zero[1] = Posicao.Y + ((ponto_zero[1] - Posicao.Y) / fatorZoom);
                precisaoPixel = precisaoPixel * fatorZoom;
                angulo -= 1;
            };          
        }

        private void checkBox1_CheckedChanged(object sender, EventArgs e)
        {
          
        }

        private void Controle_MouseMove_2(object sender, MouseEventArgs e)
        {
            Posicao = e.Location;
        }

        private void rbGrelhaPavimento_CheckedChanged(object sender, EventArgs e)
        {

        }

        private void rbGrelhaEspecifica_CheckedChanged_1(object sender, EventArgs e)
        {
            gbDiscretizacao.Enabled = rbGrelhaEspecifica.Checked;
        }

        private void chGerarGrelha_CheckedChanged(object sender, EventArgs e)
        {
            gbConfigGrelha.Enabled = chGerarGrelha.Checked;
 
        }

        private void rbGrelhaPavimento_CheckedChanged_1(object sender, EventArgs e)
        {
            gbDiscretizacao.Enabled = !rbGrelhaPavimento.Checked;
        }

        private void edEspacamentoX_ValueChanged(object sender, EventArgs e)
        {
            if (!carregando)
            {
                ConfigGrelha.AnguloBarras = (int)edAnguloBarras.Value;
                ConfigGrelha.EspacamentoX = (int)edEspacamentoX.Value;
                ConfigGrelha.EspacamentoY = (int)edEspacamentoY.Value;
            }
        }

        private void claspesoesp_KeyPress(object sender, KeyPressEventArgs e)
        {
           
 
        }

        private void claspesoesp_Leave(object sender, EventArgs e)
        {
   
        }

        private void CargaPermanente_Leave(object sender, EventArgs e)
        {
            if ((sender as TextBox).Text.Trim() != "")
              (sender as TextBox).Text = double.Parse((sender as TextBox).Text).ToString("n2");
        }

        private void CargaPermanente_KeyPress(object sender, KeyPressEventArgs e)
        {
            e.Handled = (!(e.KeyChar == 44) && !char.IsDigit(e.KeyChar) && !char.IsControl(e.KeyChar));
        }

        private void DadosLaje_FormClosed(object sender, FormClosedEventArgs e)
        {
            gerenciador.FDadosLaje = null;
        
        }

        private void DadosLaje_Move(object sender, EventArgs e)
        {
            gerenciador.AtualizaDesenho();
        
        }

    }

}

/* Calculate the screen point of the 3D point under the mouse cursor 
 * (the point that needs to be on fixed location) before move the camera.
 * (Say screen point as Pt2DScr and 3D point under mouse Pt3D1)
Move camera along camera Z axis. That's mean zoom to center of the viewport.
Calculate current 3D point which is under Pt2DScr, say Pt3D2.
Calculate the difference between Pt3D1 and Pt3D2 by (Pt3D1 - Pt3D2), say Pt3DOffset
Move the camera by the amount of Pt3DOffset.
Note that you will need a fixed plane to calculate Pt3D1 and Pt3D2 which normal vector of the plane parallel to the camera z axis (plane always face to the camera)
 */