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

    public partial class DadosViga : Form//WeifenLuo.WinFormsUI.Docking.DockContent
    {
        Gerenciador gerenciador;
        TDadosViga Dados;
        public TPoligono Poligono;

        float fatorZoom, precisaoPixel, angulo;
        float[] ponto_zero;

        int w, h;
        bool ok;
        public bool alterando = false;
        Point Posicao;
        CoordenadaD [] CoordsSecao;

        public TSecao secao;
        int tipo;
        public DadosViga()
        {
            InitializeComponent();
        }

        public DadosViga(Gerenciador frmPai, TDadosViga Dados, int indiceTipo)
        {
            InitializeComponent();
            gerenciador = frmPai;
        
            this.Dados = Dados;

            tipo = indiceTipo;
        }

        void Carregar()
        {
             NumViga.Value = Dados.numero;
             NomeViga.Text = Dados.nome;
             edh1.Text     = Dados.h1.ToString();
             edb1.Text     = Dados.b1.ToString();
             edh2.Text     = Dados.h2.ToString();
             edb2.Text     = Dados.b2.ToString();
             tbFaces.Buttons[0].Pushed    = Dados.face_insercao == 0;
             tbFaces.Buttons[2].Pushed = Dados.face_insercao == 2;
             tbFaces.Buttons[1].Pushed = Dados.face_insercao == 1;
             redTorcao.Value = Dados.redTorcao;
             CargaExtra.Text = Dados.CargaExtra.ToString();
             CargaParede.Text = Dados.CargaParede.ToString();
             cbTipo.SelectedIndex = Dados.indiceTipo;
             RigidezEI.Text = Dados.RigidezEI.ToString();
             edE.Text = Dados.excentricidade.ToString();
             RigidezGJ.Text = Dados.RigidezGJ.ToString();
             NaoUsarPP.Checked = Dados.NaoUsarPP;
        }

        private void DadosViga_FormClosing(object sender, FormClosingEventArgs e)
        {

        }

        Graphics g;
        private void DadosViga_Load(object sender, EventArgs e)
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

            cbTipo.SelectedIndex = tipo;
            CriaSecaoPadrao(btSecao.Margin.All);
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

            for (int i = 0; i < Poligono.linhas_poligonal.Count; i++)
            {
               // Gl.glBegin(Gl.GL_LINES);
              //  Gl.glVertex2d(pixelX(Poligono.linhas_poligonal[i].pIni.x * 3), pixelY(Poligono.linhas_poligonal[i].pIni.y * 3));
              //  Gl.glVertex2d(pixelX(Poligono.linhas_poligonal[i].pFin.x * 3), pixelY(Poligono.linhas_poligonal[i].pFin.y * 3));
               // Gl.glEnd();
            };
            
        }

        private void button3_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void button2_Click(object sender, EventArgs e)
        {
         //   GravaDados();
            if (alterando)
            {
                this.DialogResult = System.Windows.Forms.DialogResult.Yes;
                GravaDados();
            }

            else
                gerenciador.NovosDadosDeViga();
        }

        private void CriaSecaoPadrao(int tipo)
        {
         //  if (btSecao.Tag < cbTipo.Items.Count - 1)
                Poligono = new TPoligono(cbTipo.Text, edh1.Text, edb1.Text, edb2.Text, edh2.Text, "","",false);
       
            lbInfo.Text = "Centroide X: " + Poligono.centroide.X.ToString("n3") + " - Centroide Y: " + Poligono.centroide.Y.ToString("n3") + " - Área: " + Poligono.area.ToString("n3")
            + "\r - Per: " + Poligono.perimetro + " - ix: " + Poligono.Ix.ToString("n3") + " - iy: " + Poligono.Iy.ToString("n3")
            + "\r - Rx: " + Poligono.rx.ToString("n3") + " - Ry: " + Poligono.ry.ToString("n3");
        
          //  DesenhaSecao();
        }

        private void edBase_Validated(object sender, EventArgs e)
        {
            CriaSecaoPadrao(btSecao.Margin.All);
        }

        private void edAltura_Validated(object sender, EventArgs e)
        {
            CriaSecaoPadrao(btSecao.Margin.All);
        }

        private void edb2_Validated(object sender, EventArgs e)
        {
            CriaSecaoPadrao(btSecao.Margin.All);
        }

        private void edh2_Validated(object sender, EventArgs e)
        {
            CriaSecaoPadrao(btSecao.Margin.All);
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
         /*   Gl.glClear(Gl.GL_COLOR_BUFFER_BIT | Gl.GL_DEPTH_BUFFER_BIT);
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

            //if (ok)
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

            edh2.Enabled = cbTipo.SelectedIndex > 0 && cbTipo.SelectedIndex != cbTipo.Items.Count - 1 && cbTipo.Text != "Circular";
            edb2.Enabled = cbTipo.SelectedIndex > 0 && cbTipo.SelectedIndex != cbTipo.Items.Count - 1 && cbTipo.Text != "Circular";

            picRet.Visible = cbTipo.Text == "Retangular";
            picT.Visible = cbTipo.Text == "T";
            picI.Visible = cbTipo.Text == "I";
            picL.Visible = cbTipo.Text == "L";
            CriaSecaoPadrao(btSecao.Margin.All);
        }

        /*public void Enquadrar()
        {

            GetMiniMaxPt(ref p_xini, ref p_xfin, ref p_yini, ref p_yfin,
                         ref xIni, ref yIni, ref xFin, ref yFin);

            ponto_zero[0] = ponto_zero[0] - p_xini + 35;
            ponto_zero[1] = ponto_zero[1] - p_yfin + 35;

            for (i = 0; i < Pontos.Count; i++)
            {
                Pontos[i].px_x = pixelX(Pontos[i].x);
                Pontos[i].px_y = pixelY(Pontos[i].y);
            };


            GetMiniMaxPt(ref p_xini, ref p_xfin, ref p_yini, ref p_yfin,
                         ref xIni, ref yIni, ref xFin, ref yFin);

            for (int c = 0; c < 40; c++)
            {
                ponto_zero[0] = p_xini + ((ponto_zero[0] - p_xini) / fatorZoom);
                ponto_zero[1] = p_yfin + ((ponto_zero[1] - p_yfin) / fatorZoom);

                for (i = 0; i < Pontos.Count; i++)
                {
                    Pontos[i].px_x = (float)(p_xini + ((Pontos[i].px_x - p_xini) / fatorZoom));
                    Pontos[i].px_y = (float)(p_yfin + ((Pontos[i].px_y - p_yfin) / fatorZoom));
                };

                precisaoPixel = (float)(precisaoPixel * fatorZoom);
            }

            for (int c = 0; c < 2000; c++)
            {
                GetMiniMaxPt(ref p_xini, ref p_xfin, ref p_yini, ref p_yfin,
                             ref xIni, ref yIni, ref xFin, ref yFin);

                if ((p_xfin < w && p_xfin > (w - 50)) ||
                    (p_yini < h && p_yini > (h - 50)))
                    break;

                ponto_zero[0] = p_xini + ((ponto_zero[0] - p_xini) * 1.01);
                ponto_zero[1] = p_yfin + ((ponto_zero[1] - p_yfin) * 1.01);

                for (i = 0; i < Pontos.Count; i++)
                {
                    Pontos[i].px_x = (float)(p_xini + ((Pontos[i].px_x - p_xini) * 1.01));
                    Pontos[i].px_y = (float)(p_yfin + ((Pontos[i].px_y - p_yfin) * 1.01));
                };

                precisaoPixel = (float)(precisaoPixel / 1.01);
            }

            g2d.ClearScreen(r, g, b);

            // DrawByLayer();
            // SwapBuffers();
        }

        private void GetMiniMaxPt(ref float p_xini, ref float p_xfin, ref float p_yini, ref float p_yfin,
                           ref double xIni, ref double yIni, ref double xFin, ref double yFin)
        {
            p_xini = 999999999;
            p_yini = 999999999;
            p_xfin = 999999999;
            p_yfin = 999999999;

            xIni = 999999999;
            yIni = 999999999;

            xFin = -999999999;
            yFin = -999999999;

            for (int i = 0; i < Pontos.Count; i++)
            {
                if (!Pontos[i].Enquadrar) continue;

                if (Pontos[i].x < xIni)
                {
                    xIni = Pontos[i].x;
                    p_xini = Pontos[i].px_x;
                };

                if (Pontos[i].y < yIni)
                {
                    yIni = Pontos[i].y;
                    p_yini = Pontos[i].px_y;
                };

                if (Pontos[i].x > xFin)
                {
                    xFin = Pontos[i].x;
                    p_xfin = Pontos[i].px_x;
                };

                if (Pontos[i].y > yFin)
                {
                    yFin = Pontos[i].y;
                    p_yfin = Pontos[i].px_y;
                };
            };

            for (int i = 0; i < Textos.Count; i++)
            {
                if (Textos[i].x < xIni)
                {
                    xIni = Textos[i].x;
                    p_xini = pixelX(Textos[i].x);
                };

                if (Textos[i].y < yIni)
                {
                    yIni = Textos[i].y;
                    p_yini = pixelY(Textos[i].y);
                };

                if (Textos[i].x > xFin)
                {
                    xFin = Textos[i].x;
                    p_xfin = pixelX(Textos[i].x);
                };

                if (Textos[i].y > yFin)
                {
                    yFin = Textos[i].y;
                    p_yfin = pixelY(Textos[i].y);
                };
            };


        }*/

        public void GravaDados()
        {
            Dados.h1 = System.Convert.ToDouble(edh1.Text);
            Dados.b1 = System.Convert.ToDouble(edb1.Text);
            Dados.h2 = System.Convert.ToDouble(edh2.Text);
            Dados.b2 = System.Convert.ToDouble(edb2.Text);
            Dados.redTorcao = (int)redTorcao.Value;
            Dados.CargaExtra  = System.Convert.ToDouble(CargaExtra.Text);
            Dados.RigidezEI = System.Convert.ToDouble(RigidezEI.Text);
            Dados.RigidezGJ = System.Convert.ToDouble(RigidezGJ.Text);
            Dados.excentricidade = System.Convert.ToDouble(edE.Text);

            Dados.NaoUsarPP = NaoUsarPP.Checked;
            Dados.CargaParede = System.Convert.ToDouble(CargaParede.Text); /*   Dados.ins_face1 = FaceUm.Checked;
            Dados.ins_face2 = FaceDois.Checked;
            Dados.ins_eixo = Eixo.Checked;*/

            if (tbFaces.Buttons[0].Pushed)
              Dados.face_insercao = 0;
            else
            if (tbFaces.Buttons[1].Pushed)
              Dados.face_insercao = 1;
            else
            if (tbFaces.Buttons[2].Pushed)
              Dados.face_insercao = 2;

            Dados.nome   = NomeViga.Text;
            Dados.numero = System.Convert.ToInt32(NumViga.Text);
            Dados.indiceTipo = cbTipo.SelectedIndex;
            Dados.Poligono = Poligono;
        }

        private void button1_Click(object sender, EventArgs e)
        {

        }

        private void Controle_MouseMove_2(object sender, MouseEventArgs e)
        {
            Posicao = e.Location;
        }

        private void tabPage2_Click(object sender, EventArgs e)
        {

        }

        private void label16_Click(object sender, EventArgs e)
        {

        }

        private void CargaExtra_KeyDown(object sender, KeyEventArgs e)
        {

        }

        private void CargaParede_KeyPress(object sender, KeyPressEventArgs e)
        {
            e.Handled = (!(e.KeyChar == 44) && !char.IsDigit(e.KeyChar) && !char.IsControl(e.KeyChar));
            if (e.KeyChar == (char)Keys.Escape)
                this.Close();
        }

        private void CargaParede_Leave(object sender, EventArgs e)
        {
            if ((sender as TextBox).Text.Trim() != "")
                (sender as TextBox).Text = double.Parse((sender as TextBox).Text).ToString("n2");
        }

        private void groupBox4_Enter(object sender, EventArgs e)
        {

        }

        private void Arco_CheckedChanged(object sender, EventArgs e)
        {
       
        }

        private void Reta_CheckedChanged(object sender, EventArgs e)
        {
      
    
        }

        private void DadosViga_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (e.KeyChar == (char)Keys.Escape)
                this.Close();
        }

        private void DadosViga_KeyDown(object sender, KeyEventArgs e)
        {
             if (e.KeyData == Keys.Escape)
                 this.Close();
          
        }

        private void edb1_KeyPress(object sender, KeyPressEventArgs e)
        {
            e.Handled = (!(e.KeyChar == 44) && !char.IsDigit(e.KeyChar) && !char.IsControl(e.KeyChar));
            if (e.KeyChar == (char)Keys.Escape)
                this.Close();
        }

        private void redTorcao_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (e.KeyChar == (char)Keys.Escape)
                this.Close();
        }

        private void tabControl3_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (e.KeyChar == (char)Keys.Escape)
                this.Close();
        }

        private void DadosViga_FormClosed(object sender, FormClosedEventArgs e)
        {
            gerenciador.FDadosViga = null;
        }

        private void toolBar1_ButtonClick(object sender, ToolBarButtonClickEventArgs e)
        {
 
        }

        private void tbFaces_ButtonClick(object sender, ToolBarButtonClickEventArgs e)
        {
            int bt = 0;
            for (int i = 0; i < tbFaces.Buttons.Count; i++)
              tbFaces.Buttons[i].Pushed = false;

            e.Button.Pushed = !e.Button.Pushed;

            if (tbFaces.Buttons[0].Pushed)
                edE.Text = (System.Convert.ToDouble(edb1.Text)*-1 / 2).ToString();
            if (tbFaces.Buttons[2].Pushed)
                edE.Text = (System.Convert.ToDouble(edb1.Text)/ 2).ToString();
            if (tbFaces.Buttons[1].Pushed)
                edE.Text = "0";
        }

        private void btSecao_Click(object sender, EventArgs e)
        {
            cbTipo.SelectedIndex = btSecao.Margin.All;
        }

        private void DadosViga_Move(object sender, EventArgs e)
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