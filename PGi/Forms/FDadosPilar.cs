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

    public partial class DadosPilar : Form
    {
        Gerenciador gerenciador;

        float fatorZoom, precisaoPixel, angulo, angulo2;
        float[] ponto_zero;

        TDadosPilar Dados;
        public bool alterando = false;
        int w, h;
        Point Posicao;

        public TPoligono Poligono;

        int tipo;
        public DadosPilar()
        {
            InitializeComponent();
        }

        public DadosPilar(Gerenciador gerenciador, TDadosPilar Dados, int indiceTipo)
        {
            InitializeComponent();
            this.gerenciador = gerenciador;

            this.Dados = Dados;

            tipo = indiceTipo;
        }

        void Carregar()
        {
            Num.Value = Dados.numero;
            Nome.Text = Dados.nome;
            try
            {
              cbTipo.SelectedIndex = Dados.indiceTipo;
              edh1.Text = Dados.h1.ToString();
              edb1.Text = Dados.b1.ToString();
              edAltura.Text = Dados.altura.ToString();
            }
            catch (Exception e)
            {
                MessageBox.Show(e.Message);
            }
        }

        public void GravaDados()
        {
           /*if (Dados == null)
              Dados = new TDadosPilar();
           */ 
            Dados.h1 = System.Convert.ToDouble(edh1.Text);
            Dados.b1 = System.Convert.ToDouble(edb1.Text);
            Dados.indiceTipo = cbTipo.SelectedIndex;
            Dados.altura = System.Convert.ToDouble(edAltura.Text);

            Dados.excentricidade = System.Convert.ToDouble(edE.Text);
            Dados.nome = Nome.Text;
            Dados.numero = System.Convert.ToInt32(Num.Text);
            Dados.Poligono = Poligono.Clone() as TPoligono;

            /*   Gerenciador.desenho.DadosTrecho = new TDadosLaje(); 
               Gerenciador.desenho.DadosTrecho.secao = secao;
               Gerenciador.desenho.DadosTrecho.secao.h1 = System.Convert.ToDouble(edAltura.Text);
               Gerenciador.desenho.DadosTrecho.secao.b1 = System.Convert.ToDouble(edBase.Text);
               Gerenciador.desenho.DadosTrecho.revestimento = System.Convert.ToDouble(Revestimento.Text);
               Gerenciador.desenho.DadosTrecho.ins_face1    = FaceUm.Checked;
               Gerenciador.desenho.DadosTrecho.ins_face2    = FaceDois.Checked;
               Gerenciador.desenho.DadosTrecho.ins_eixo     = Eixo.Checked;
               Gerenciador.desenho.DadosTrecho.nome      = "V" + NumViga.Text;
               Gerenciador.desenho.DadosTrecho.numero    = System.Convert.ToInt32(NumViga.Text);
               Gerenciador.desenho.DadosTrecho.secao     = secao;*/
        }

        private void DadosLaje_FormClosing(object sender, FormClosingEventArgs e)
        {


        //    this.Hide();
         //   e.Cancel = true;

            
           // Gerenciador.desenho.WindowState = System.Windows.Forms.FormWindowState.Normal;
          //  Gerenciador.desenho.Width  = 1000;
          //  Gerenciador.desenho.Height = 1000;
          //  Gerenciador.desenho.WindowState = System.Windows.Forms.FormWindowState.Maximized;
        }

        private void DadosLaje_Load(object sender, EventArgs e)
        {

            fatorZoom = 1.2f;


            ponto_zero = new float[2];

            ponto_zero[0] = 0;
            ponto_zero[1] = h;

            precisaoPixel = 0.8f;
            angulo        = 0;
                 
            if (Dados != null)
              Carregar();

            cbTipo.SelectedIndex = tipo;
            CriaSecaoPadrao(cbTipo.SelectedIndex);
            DesenhaSecao();

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

        private void DesenhaSecao()
        {
            /*if (cbTipo.SelectedIndex == cbTipo.Items.Count - 1)
            {
                Gl.glClear(Gl.GL_COLOR_BUFFER_BIT | Gl.GL_DEPTH_BUFFER_BIT);
                Gl.glMatrixMode(Gl.GL_MODELVIEW);
                Gl.glLoadIdentity();
                Gl.glClearColor(0, 0, 0, 0);
                Controle.SwapBuffers(); 
                return;
            }
            Gl.glClear(Gl.GL_COLOR_BUFFER_BIT | Gl.GL_DEPTH_BUFFER_BIT);
            Gl.glMatrixMode(Gl.GL_MODELVIEW);
            Gl.glLoadIdentity();
            Gl.glClearColor(0, 0, 0, 0);
            
            Gl.glColor3f(1,1,1);

            for (int i = 0; i < Poligono.linhas_poligonal.Count; i++)
            {
                Gl.glBegin(Gl.GL_LINES);
                Gl.glVertex2d(pixelX(Poligono.linhas_poligonal[i].pIni.x * 3), pixelY(Poligono.linhas_poligonal[i].pIni.y * 3));
                Gl.glVertex2d(pixelX(Poligono.linhas_poligonal[i].pFin.x * 3), pixelY(Poligono.linhas_poligonal[i].pFin.y * 3));
                Gl.glEnd();
            };

            Controle.SwapBuffers();*/
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
                gerenciador.NovosDadosDePilar();
          
        }

        private void CriaSecaoPadrao(int tipo)
        {
            if (cbTipo.SelectedIndex < cbTipo.Items.Count - 1)
              Poligono = new TPoligono(cbTipo.Text, edh1.Text, edb1.Text,
                  "","", "","", false);
                 
          /*  this.centroide.X = C.X;
            this.centroide.Y = C.Y;
            this.perimetro = Per;

            this.Ix= Ix;
            this.Iy= Iy;
            this.Ixy= Ixy;
            this.Ixcg = Math.Abs(Ixcg);
            this.Iycg = Math.Abs(Iycg);
            this.rx   = rx;
            this.ry   = ry;

            this.fMax.X = System.Convert.ToSingle(Max_X);
            this.fMax.Y = System.Convert.ToSingle(Max_Y);
            this.fMin.X = System.Convert.ToSingle(Min_X);
            this.fMin.Y = System.Convert.ToSingle(Min_Y);

            this.WxInf = Ixcg/Math.Abs(C.Y-fMin.Y);
            this.WxSup = Ixcg/Math.Abs(fMax.Y-C.Y);
            this.WxMax = Ixcg/Math.Max(Math.Abs(C.Y-fMin.Y),Math.Abs(fMax.Y-C.Y));
            this.WyInf = Iycg/Math.Abs(C.X-fMin.X);
            this.WySup = Iycg/Math.Abs(fMax.X-C.X);
            this.WyMax = Iycg/Math.Max(Math.Abs(C.X-fMin.X),Math.Abs(fMax.X-C.X));
            */

          /*  lbInfo.Text = "Centroide X: " + Poligono.centroide.X.ToString("n3") + " - Centroide Y: " + Poligono.centroide.Y.ToString("n3") + " - Área: " + Poligono.area.ToString("n3")
            +"\r - Per: " + Poligono.perimetro + " - ix: " + Poligono.Ix.ToString("n3") + " - iy: " + Poligono.Iy.ToString("n3")
            + "\r - Rx: " + Poligono.rx.ToString("n3") + " - Ry: " + Poligono.ry.ToString("n3");
            */
            
            /*
                case 1:
                    {
                        CoordsSecao[0].X = 0;
                        CoordsSecao[0].Y = 0;

                        CoordsSecao[1].X = System.Convert.ToSingle(edb1.Text);
                        CoordsSecao[1].Y = 0;

                        CoordsSecao[2].X = System.Convert.ToSingle(edb1.Text);
                        CoordsSecao[2].Y = System.Convert.ToSingle(edh1.Text) - System.Convert.ToSingle(edh2.Text);

                        CoordsSecao[3].X = System.Convert.ToSingle(edb1.Text) + ((System.Convert.ToSingle(edb2.Text) - System.Convert.ToSingle(edb1.Text)) / 2);
                        CoordsSecao[3].Y = System.Convert.ToSingle(edh1.Text) - System.Convert.ToSingle(edh2.Text);

                        CoordsSecao[4].X = System.Convert.ToSingle(edb1.Text) + ((System.Convert.ToSingle(edb2.Text) - System.Convert.ToSingle(edb1.Text)) / 2);
                        CoordsSecao[4].Y = System.Convert.ToSingle(edh1.Text);

                        CoordsSecao[5].X = 0-((System.Convert.ToSingle(edb2.Text) - System.Convert.ToSingle(edb1.Text)) / 2);
                        CoordsSecao[5].Y = System.Convert.ToSingle(edh1.Text);

                        CoordsSecao[6].X = 0 - ((System.Convert.ToSingle(edb2.Text) - System.Convert.ToSingle(edb1.Text)) / 2);
                        CoordsSecao[6].Y = System.Convert.ToSingle(edh1.Text) - System.Convert.ToSingle(edh2.Text);

                        CoordsSecao[7].X = 0;
                        CoordsSecao[7].Y = System.Convert.ToSingle(edh1.Text) - System.Convert.ToSingle(edh2.Text);

                        CoordsSecao[8].X = 0;
                        CoordsSecao[8].Y = 0;

                        break;
                    };

                case 2:
                    {
                        CoordsSecao[0].X = 0;
                        CoordsSecao[0].Y = 0;

                        CoordsSecao[1].X = System.Convert.ToSingle(edb2.Text);
                        CoordsSecao[1].Y = 0;

                        CoordsSecao[2].X = System.Convert.ToSingle(edb2.Text);
                        CoordsSecao[2].Y = System.Convert.ToSingle(edh1.Text) - System.Convert.ToSingle(edh2.Text);

                        CoordsSecao[3].X = System.Convert.ToSingle(edb2.Text) - (Math.Abs((System.Convert.ToSingle(edb2.Text) - System.Convert.ToSingle(edb1.Text))) / 2);
                        CoordsSecao[3].Y = System.Convert.ToSingle(edh1.Text) - System.Convert.ToSingle(edh2.Text);

                        CoordsSecao[4].X = System.Convert.ToSingle(edb2.Text) - (Math.Abs((System.Convert.ToSingle(edb2.Text) - System.Convert.ToSingle(edb1.Text))) / 2);
                        CoordsSecao[4].Y = System.Convert.ToSingle(edh1.Text);

                        CoordsSecao[5].X = System.Convert.ToSingle(edb2.Text) - (Math.Abs((System.Convert.ToSingle(edb2.Text) - System.Convert.ToSingle(edb1.Text))) / 2)- System.Convert.ToSingle(edb2.Text);
                        CoordsSecao[5].Y = System.Convert.ToSingle(edh1.Text);

                        CoordsSecao[6].X = System.Convert.ToSingle(edb2.Text) - (Math.Abs((System.Convert.ToSingle(edb2.Text) - System.Convert.ToSingle(edb1.Text))) / 2) - System.Convert.ToSingle(edb2.Text);
                        CoordsSecao[6].Y = System.Convert.ToSingle(edh1.Text) - System.Convert.ToSingle(edh2.Text);

                        CoordsSecao[7].X = 0;
                        CoordsSecao[7].Y = System.Convert.ToSingle(edh1.Text) - System.Convert.ToSingle(edh2.Text);

                        CoordsSecao[8].X = 0;
                        CoordsSecao[8].Y = 0;

                        break;
                    };
            };

            ok    = true;
            secao = new TSecao(CoordsSecao, false, cbTipo.SelectedIndex);
        */
         //   BestFit();    
            if (cbTipo.Text != "Circular")
            DesenhaSecao();
        }

        private void edBase_Validated(object sender, EventArgs e)
        {
            CriaSecaoPadrao(cbTipo.SelectedIndex);
        }


        private void edAltura_Validated(object sender, EventArgs e)
        {
            CriaSecaoPadrao(cbTipo.SelectedIndex);
        }

        private void edb2_Validated(object sender, EventArgs e)
        {
            CriaSecaoPadrao(cbTipo.SelectedIndex);
        }

        private void edh2_Validated(object sender, EventArgs e)
        {
            CriaSecaoPadrao(cbTipo.SelectedIndex);
        }

        private void Controle_MouseClick(object sender, MouseEventArgs e)
        {
           /* unsafe
            {
                viewport = new int[4];
                modelview = new double[16];
                projection = new double[16];

                //glGetIntegerv(GL_VIEWPORT, viewport);
                viewport[0] = 0;
                viewport[1] = 0;
                viewport[2] = w;
                viewport[3] = h; 
                
                winX = Posicao.X;
                winY = Posicao.Y;
                double []winZ;
                winZ = new double[1];

                Gl.glGetDoublev(Gl.GL_MODELVIEW_MATRIX, modelview);
                Gl.glGetDoublev(Gl.GL_PROJECTION_MATRIX, projection);
                Gl.glGetIntegerv(Gl.GL_VIEWPORT, viewport);

                Gl.glReadPixels(winX, winY, 1, 1, Gl.GL_DEPTH_COMPONENT, Gl.GL_FLOAT, winZ);

                double posX, posY, posZ;
    
                Glu.gluUnProject(winX, winY, winZ[0], modelview, projection, viewport, out posX, out posY, out posZ);


                MessageBox.Show(posX.ToString() + " -" + posY.ToString() + " " + posZ.ToString());
            };*/
        }

        private void button5_Click_1(object sender, EventArgs e)
        {
         /*   Gl.glClear(Gl.GL_COLOR_BUFFER_BIT | Gl.GL_DEPTH_BUFFER_BIT);
            Gl.glMatrixMode(Gl.GL_MODELVIEW);
            Gl.glLoadIdentity();
            Gl.glClearColor(0, 0, 0, 0);
            
            Gl.glPushMatrix();
            angulo += 1;
            Gl.glTranslatef(0,0, 0);
            Gl.glScalef(2,2 ,2);
            Gl.glRotatef(angulo, 0, 1, 0);
            
            Gl.glBegin(Gl.GL_LINES);
            Gl.glColor3f(1, 1, 1);
            
            Gl.glVertex3f(150, 50,0);
            Gl.glVertex3f(150, 150,0);

            Gl.glVertex3f(200, 50, 0);
            Gl.glVertex3f(350, 60, -100);

            Gl.glEnd();
          
            Gl.glBegin(Gl.GL_LINE_STRIP);
            Gl.glColor3f(1, 1, 1);

            double x, y;
            double angle = 25;
            double z = -10.0f;
            for (angle = 0.0f; angle <= (2.0f * Math.PI) * 3.0f; angle += 0.1f)
            {
                x = 20.0f * Math.Sin(System.Convert.ToSingle(angle));
                y = 20.0f * Math.Cos(System.Convert.ToSingle(angle));

                // Specify the point and move the z value up a little 
                Gl.glVertex3f(System.Convert.ToSingle(x), System.Convert.ToSingle(y), System.Convert.ToSingle(z));
                z += 0.5f;
            }

            // Done drawing points
            Gl.glEnd();

            Gl.glPopMatrix();
            Gl.glFlush();
         

            Controle.SwapBuffers();*/
        }

        private void button5_Click(object sender, EventArgs e)
        {
          /*  Gl.glClear(Gl.GL_COLOR_BUFFER_BIT | Gl.GL_DEPTH_BUFFER_BIT);
            Gl.glMatrixMode(Gl.GL_MODELVIEW);
            Gl.glLoadIdentity();
            Gl.glClearColor(0, 0, 0, 0); 
            
            angulo2 += 5;

            Gl.glPushMatrix();
            Gl.glRotatef(angulo2, 0, 0, 1);

            Gl.glBegin(Gl.GL_LINE_STRIP);
            Gl.glColor3f(1, 1, 1);
            Gl.glVertex3f(200, 200, 0);
            Gl.glVertex3f(400, 200, 0);
            Gl.glVertex3f(400, 200, -100);
            Gl.glVertex3f(200, 200, -100);
            Gl.glEnd();

        //    Gl.glPopMatrix();

            Controle.SwapBuffers();*/
        }

        private void button6_Click(object sender, EventArgs e)
        {

         /*   Gl.glClear(Gl.GL_COLOR_BUFFER_BIT | Gl.GL_DEPTH_BUFFER_BIT);
            Gl.glMatrixMode(Gl.GL_MODELVIEW);
            Gl.glLoadIdentity();
            Gl.glClearColor(0, 0, 0, 0);

           angulo2 -= 5;
         
           Gl.glPushMatrix();
           Gl.glRotatef(angulo2, 0, 1, 0);

           Gl.glBegin(Gl.GL_LINE_STRIP);
           Gl.glColor3f(1, 1, 1);
           Gl.glVertex3f(200, 200, 0);
           Gl.glVertex3f(400, 200, 0);
           Gl.glVertex3f(400, 200, -100);
           Gl.glVertex3f(200, 200, -100);
           Gl.glEnd();

       //    Gl.glPopMatrix();
           Controle.SwapBuffers();*/
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

           // if (ok)
              DesenhaSecao();

        //    Controle.SwapBuffers();

        /*    unsafe
            {
                viewport = new int[4];
                modelview = new double[16];
                projection = new double[16];

                //glGetIntegerv(GL_VIEWPORT, viewport);
                viewport[0] = 0;
                viewport[1] = 0;
                viewport[2] = w;
                viewport[3] = h;

                winX = Posicao.X;
                winY = Posicao.Y;
                double[] winZ;
                winZ = new double[1];

                Gl.glGetDoublev(Gl.GL_MODELVIEW_MATRIX, modelview);
                Gl.glGetDoublev(Gl.GL_PROJECTION_MATRIX, projection);
                Gl.glGetIntegerv(Gl.GL_VIEWPORT, viewport);

                Gl.glReadPixels(winX, winY, 1, 1, Gl.GL_DEPTH_COMPONENT, Gl.GL_FLOAT, winZ);

                double posX, posY, posZ;

                Glu.gluUnProject(winX, winY, winZ[0], modelview, projection, viewport, out posX, out posY, out posZ);


                MessageBox.Show(posX.ToString() + " -" + posY.ToString() + " " + posZ.ToString());
            };*/
            
        }

        private void cbTipo_SelectedIndexChanged(object sender, EventArgs e)
        {
            edh1.Enabled = cbTipo.SelectedIndex != cbTipo.Items.Count - 1;
            edb1.Enabled = cbTipo.SelectedIndex != cbTipo.Items.Count - 1;

          /*  edh2.Enabled = cbTipo.SelectedIndex > 0 && cbTipo.SelectedIndex != cbTipo.Items.Count - 1 && cbTipo.Text != "Circular";
            edb2.Enabled = cbTipo.SelectedIndex > 0 && cbTipo.SelectedIndex != cbTipo.Items.Count - 1 && cbTipo.Text != "Circular";
            edAngulo.Enabled = cbTipo.Text == "L" || cbTipo.Text == "U" || cbTipo.SelectedIndex != cbTipo.Items.Count - 1;
            edAngulo2.Enabled = cbTipo.Text == "U" || cbTipo.SelectedIndex != cbTipo.Items.Count - 1;
            */
            picRet.Visible   = cbTipo.Text == "Retangular";
            picU.Visible = cbTipo.Text == "U";
            picT.Visible = cbTipo.Text == "T";
            picI.Visible = cbTipo.Text == "I";
            picCirc.Visible = cbTipo.Text == "Circular";
            picL.Visible = cbTipo.Text == "L";

            if (cbTipo.SelectedIndex == cbTipo.Items.Count - 1)
            {
        //        lbInfo.Text = "Clique em inserir para desenhar um polígono fechado na tela do pavimento. Após o definição, o programa retorna para essa tela.";
  //              lbInfo.Text += " Importante: defina em sentido anti-horário os pontos do polígono.";
            }
            else
              CriaSecaoPadrao(cbTipo.SelectedIndex);

        }

        private void chVazada_CheckedChanged(object sender, EventArgs e)
        {

        }

        private void Controle_MouseMove_1(object sender, MouseEventArgs e)
        {

        }

        private void button1_Click(object sender, EventArgs e)
        {
        }

        private void Controle_MouseMove_2(object sender, MouseEventArgs e)
        {
            Posicao = e.Location;
        }

        private void tabPage3_Validated(object sender, EventArgs e)
        {
            CriaSecaoPadrao(cbTipo.SelectedIndex);
        }

        private void DadosPilar_FormClosed(object sender, FormClosedEventArgs e)
        {
            gerenciador.FDadosPilar = null;
        }

        private void DadosPilar_Move(object sender, EventArgs e)
        {
            gerenciador.AtualizaDesenho();
        
        }

    }

}