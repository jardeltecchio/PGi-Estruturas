using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Windows.Media;

namespace PG
{
    public partial class FProcessoCalculo : Form
    {
        Gerenciador owner;
        bool vis;
        public FProcessoCalculo()
        {
            InitializeComponent();
        }
        public FProcessoCalculo(Gerenciador ow, bool visivel)
        {
            InitializeComponent();
          //  vis = true;
            this.owner = ow;
            LabelProcesso.Visible = false;
            Application.DoEvents();
        }

        private void button3_Click(object sender, EventArgs e)
        {
            Close();
        }
        void DesenhaMatriz()
        {
            int tamQuadrado = 1;
          //  if (mostrarMatrizDeRigidezDuranteCalculo.Checked)
            {
                int Tam = owner.formDesenho.Estrutura.PorticoEspacial.NLinhas;
                tamQuadrado = pnMatrizRigidez.Width-80;
                int tamBlocos = (int)(Tam / tamQuadrado);

                try
                {
                    int i = 0, j = 0, k = 0, l = 0;
                    int lin = 0, col = 0;
                    bool achou = false;
                    if (owner.formDesenho.Estrutura.PorticoEspacial.MatrizRigidez != null)
                    {
                        for (i = 0; i < tamQuadrado; i++)
                        {
                            lin = (i * tamBlocos);
                            ///           pnMatrizRigidez.Update();

                            try
                            {
                                for (j = 0; j < tamQuadrado; j++)
                                {
                                    col = (j * tamBlocos) - 1;
                                    achou = false;
                                    for (k = 0; k < tamBlocos; k++)
                                    {
                                        lin += k;
                                        for (l = 0; l < tamBlocos; l++)
                                        {
                                            col += 1;

                                            if (!Geom.Iguais(alglib.sparseget(owner.formDesenho.Estrutura.PorticoEspacial.MatrizRigidez.s, lin, col), 0))
                                            {
                                                GraphicsMatrizRigidez.FillRectangle(aBrush, i + 1, j + 1, 1, 1);
                                                achou = true;
                                                break;
                                            }
                                        }
                                        if (achou)
                                            break;

                                        col = (j * tamBlocos) - 1;
                                    }

                                    lin = (i * tamBlocos);
                                }
                            }
                            catch (Exception)
                            {
                                //      MessageBox.Show("i:" + i.ToString() + " j:" + j.ToString() + " k:" + k.ToString() + " l:" + l.ToString()
                                //     + " lin:" + lin.ToString() + " col:" +col.ToString());
                            }

                        }
                    }
                }

                catch (Exception e)
                {
                    MessageBox.Show(e.Message);
                }
            }
        }
        private void FProcessoCalculo_FormClosed(object sender, FormClosedEventArgs e)
        {
            if (owner.processo != null)
                owner.processo.Dispose();   
        }

        private void btInterromper_Click(object sender, EventArgs e)
        {
            owner.CalculoCancelado = true;
        }

        private void FProcessoCalculo_Shown(object sender, EventArgs e)
        {
            this.Visible = vis;
        }
        System.Drawing.Graphics GraphicsMatrizRigidez;
        System.Drawing.Pen mPen = new System.Drawing.Pen(System.Drawing.Color.Black);
        System.Drawing.Brush aBrush = (System.Drawing.Brush)System.Drawing.Brushes.Black;

        private void pnMatrizRigidez_Paint(object sender, PaintEventArgs e)
        {
            GraphicsMatrizRigidez = e.Graphics;
            //      ControleCAD.DrawLine(mPen, 0, 0, 50, 50);


           // if (owner.formDesenho.Estrutura.PorticoEspacial != null)
       //         DesenhaMatriz();
        }

        private void FProcessoCalculo_Load(object sender, EventArgs e)
        {

        }
    }
}
