using MathNet.Numerics.Distributions;
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


    
    public partial class FRotacionarElementos : Form
    {
        Gerenciador gerenciador;
        public FRotacionarElementos()
        {
            InitializeComponent();
        }
        
        F3D owner;
        public FRotacionarElementos(Gerenciador gerenciador)
        {
            this.gerenciador = gerenciador;

            InitializeComponent();
        }

        private void button3_Click(object sender, EventArgs e)
        {
            Close();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            this.DialogResult = DialogResult.OK;
        }

        private void F3DOpcoesVisualizacao_Load(object sender, EventArgs e)
        {
   
        }

        private void button6_Click_1(object sender, EventArgs e)
        {

        }


        private void F3DOpcoesVisualizacao_Move(object sender, EventArgs e)
        {
            gerenciador.AtualizaDesenho();
        }

        private void btCor_Click(object sender, EventArgs e)
        {

        }
        private void Arestas_CheckedChanged_1(object sender, EventArgs e)
        {

        }

        private void button2_Click(object sender, EventArgs e)
        {

        }

        private void ArestaConfObjeto_CheckedChanged(object sender, EventArgs e)
        {

        }
        vec3 n, v,  p, d, r, r2, p3;
        vec3 p1, p1_n;

        private void edAngulo_KeyPress(object sender, KeyPressEventArgs e)
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

        private void chCopiar_CheckedChanged(object sender, EventArgs e)
        {
            edCopias.Enabled = chCopiar.Checked;
        }

        private void FRotacionarElementos_FormClosed(object sender, FormClosedEventArgs e)
        {
            gerenciador.frot = null;
        }
        vec3 normal;
        Plano plano;
        List<TBarraGenerica> b;
        private void button11_Click(object sender, EventArgs e)
        {
           b = gerenciador.formDesenho.Estrutura.barras.FindAll(o => o.Selecionado).ToList();
            if (b.Count == 0)
              return;
            this.Text = b[0].IDBarra + " , " + b[1].IDBarra;
            vec3 p1 = new vec3(b[0].pIni.x, b[0].pIni.y, b[0].pIni.z);
            vec3 p2 = new vec3(b[0].pFin.x, b[0].pFin.y, b[0].pFin.z);
            vec3 p3 = new vec3(b[1].pIni.x, b[1].pIni.y, b[1].pIni.z);
            vec3 p4 = new vec3(b[1].pFin.x, b[1].pFin.y, b[1].pFin.z);

            vec3 p5 = p2 - p1;
            vec3 p6 = p4 - p3;

            normal = p5.CrossProduct(p6).Normalize();
            normal.z *= -1;
            normal.y *= -1;
            textBox7.Text = normal.x.ToString("n4") + ", " + normal.y.ToString("n4") + ", " + normal.z.ToString("n4");

            posicao = (((p1+p2)/2) + (p3 + p4) / 2) / 2;
            posicao.z *= -1;
            posicao.y *= -1;
            textBox10.Text = posicao.x.ToString("n4") + ", " + posicao.y.ToString("n4") + ", " + posicao.z.ToString("n4");


            plano = new Plano(normal, posicao, "", false);

            textBox11.Text = plano.a.ToString("n2") + ", " + plano.b.ToString("n2") + ", " + plano.c.ToString("n2") + ", " + plano.d.ToString("n2");
        }

        private void button12_Click(object sender, EventArgs e)
        {
           
        }
        double vdotnxn1;
        private void button14_Click(object sender, EventArgs e)
        {
            vdotnxn1 = v.DotProduct(normal);
            vec3 res = vdotnxn1 * normal;
            r = -2 * res + v;
            textBox9.Text = r.x.ToString("n2") + ", " + r.y.ToString("n2") + ", " + r.z.ToString("n2");
        }

        private void button13_Click(object sender, EventArgs e)
        {
            TBarraGenerica b = gerenciador.formDesenho.Estrutura.barras.Find(o => o.Selecionado);
            if (b == null)
                return;

            p1 = new vec3(b.pIni.x, -b.pIni.y, -b.pIni.z);
            p2 = new vec3(b.pFin.x, -b.pFin.y, -b.pFin.z);

            p1_n = new vec3(b.pIni.x, -b.pIni.y, -b.pIni.z);

            /*p1.x -= p1_n.x;
            p1.y -= p1_n.y;
            p1.z -= p1_n.z;

            p2.x -= p1_n.x;
            p2.y -= p1_n.y;
            p2.z -= p1_n.z;*/
            if (checkBox3.Checked)
                v = p1;// - posicao;
            else
                v = p2;// - posicao;
                     // v.z *= -1;
                     // v.y *= -1;

       

            textBox8.Text = v.x.ToString("n2") + ", " + v.y.ToString("n2") + ", " + v.z.ToString("n2");
        }
        double distanciaPontoZero_ao_Plano;
        vec3 vetorPontoAoPlano;
        private void button15_Click(object sender, EventArgs e)
        {
            vetorPontoAoPlano = new vec3(0);
            distanciaPontoZero_ao_Plano = Geom.DistanciaPontoPlano(normal, new vec3(0, 0, 0), posicao, ref vetorPontoAoPlano);
            textBox12.Text = distanciaPontoZero_ao_Plano.ToString("n4");
        }
        double alfaX, alfaY, alfaZ, offsetX, offsetY, offsetZ;
        private void button12_Click_1(object sender, EventArgs e)
        {
           /* alfaX = Math.Acos(normal.x);
            alfaY = Math.Acos(normal.y);
            alfaZ = Math.Acos(normal.z);

            offsetX = vetorPontoAoPlano.Magnitude();

            offsetX = 2 * (Math.Cos(alfaX) * distanciaPontoZero_ao_Plano);
            offsetY = 2 * (Math.Cos(alfaY) * distanciaPontoZero_ao_Plano);
            offsetZ = 2 * (Math.Cos(alfaZ) * distanciaPontoZero_ao_Plano);*/

            textBox13.Text = (r.x + (2*vetorPontoAoPlano.x)).ToString("n3") + ", " + (r.y + (2 * vetorPontoAoPlano.y)).ToString("n3") + ", " + (r.z + (2 * vetorPontoAoPlano.z)).ToString("n3");
            textBox14.Text = (vetorPontoAoPlano.x).ToString("n3") + ", " + (vetorPontoAoPlano.y).ToString("n3") + ", " + (vetorPontoAoPlano.z).ToString("n3");
        }

        vec3 posicao;
        vec3 p2;
        //https://github.com/EgoMoose/Articles/blob/master/Rodrigues'%20rotation/Rodrigues'%20rotation.md
        private void button4_Click(object sender, EventArgs e)
        {
            TBarraGenerica b = gerenciador.formDesenho.Estrutura.barras.Find(o => o.Selecionado);
            if (b == null)
              return;

            p1 = new vec3(b.pIni.x, -b.pIni.y, -b.pIni.z);
            p2 = new vec3(b.pFin.x, -b.pFin.y, -b.pFin.z);
            
            p1_n = new vec3(b.pIni.x, -b.pIni.y, -b.pIni.z);

            p1.x -= p1_n.x;
            p1.y -= p1_n.y;
            p1.z -= p1_n.z;
                    
            p2.x -= p1_n.x;
            p2.y -= p1_n.y;
            p2.z -= p1_n.z;

            n = p2 - p1;
            n.Normalize();
            textBox2.Text = n.x.ToString("n2") + ", " + n.y.ToString("n2") + ", " + n.z.ToString("n2");
        }
        private void button5_Click(object sender, EventArgs e)
        {
            TBarraGenerica b = gerenciador.formDesenho.Estrutura.barras.Find(o => o.Selecionado);
            if (b == null)
                return;

            //vec3 p1 = new vec3(b.pIni.x, b.pIni.y, b.pIni.z);
            if (checkBox2.Checked)
                p2 = new vec3(b.pIni.x, -b.pIni.y, -b.pIni.z);
            else
                p2 = new vec3(b.pFin.x, -b.pFin.y, -b.pFin.z);

            p2.x -= p1_n.x;
            p2.y -= p1_n.y;
            p2.z -= p1_n.z;

            p = p2 - p1;
            textBox3.Text = p.x.ToString("n2") + ", " + p.y.ToString("n2") + ", " + p.z.ToString("n2");
        }

        private void button8_Click(object sender, EventArgs e)
        {
            double teta = RMath.deg2rad((double)numericUpDown1.Value);

            r2 = r * (Math.Cos(teta)) + (n.CrossProduct(r) * Math.Sin(teta));
            p3 = d + r2;
            
            p3 += p1_n;

            textBox5.Text = r2.x.ToString("n2") + ", " + r2.y.ToString("n2") + ", " + r2.z.ToString("n2");
            textBox6.Text = p3.x.ToString("n2") + ", " + p3.y.ToString("n2") + ", " + p3.z.ToString("n2");
        }

        private void button7_Click(object sender, EventArgs e)
        {
            r = p - d;
            textBox4.Text = r.x.ToString("n2") + ", " + r.y.ToString("n2") + ", " + r.z.ToString("n2");
        }


        private void button2_Click_1(object sender, EventArgs e)
        {
            d = n.DotProduct(p)* (n);
            textBox1.Text = d.x.ToString("n2") + ", " + d.y.ToString("n2") + ", " + d.z.ToString("n2");
        }
    }
}
