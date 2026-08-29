using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Diagnostics;
using System.Threading;
using System.IO;
using rpaulo.toolbar;
using PG.Geral;

namespace PG
{
    public partial class Gerenciador : Form
    {
        public static Desenho desenho;
        public static NovoProjeto inicial;
        
        public List<TPavimento> Pavimentos;

        public EditorGrelha EdtGrelha;

        public DadosViga           dadosviga;
        public VisualizadorPortico visPortico;

        public OpcoesCaptura     captura;
        public ToolBarDockHolder holderProgresso;
        
        ToolBarManager    _toolBarManager,
                          _toolBarManager2;

        ToolBarDockHolder holder1, 
                          holder3, 
                          holder6;

        public Gerenciador()
        {
            InitializeComponent();

            dadosviga  = new DadosViga();
            captura    = new OpcoesCaptura();
  
            Configuracoes.Captura.OutrosAngulos = new List<float>();
           
            PanelCorFundo.Visible = false;

            CriaToolBars();

            Comando.Text  = "";
            Comando2.Text = "";

            CriaFormInicial();

            Pavimentos = new List<TPavimento>();
        }

        private void CriaFormInicial()
        {
            inicial = new NovoProjeto();
            DialogResult result = inicial.ShowDialog();

            if (result == DialogResult.OK)
            {
                CriaFormDesenho();
            }

        }
        
        private void CriaFormDesenho()
        {
            desenho = new Desenho(this);
            desenho.MdiParent = this;
            desenho.Show();
            desenho.WindowState = System.Windows.Forms.FormWindowState.Maximized;
            desenho.inserindo = -1;

            Desenho.r = 0;
            Desenho.g = 0;
            Desenho.b = 0;
        }

        private void CriaToolBars()
        {
            _toolBarManager = new ToolBarManager(this, this);
            this.tbVigas.Text = "Viga";
            _toolBarManager.AddControl(tbVigas, DockStyle.Top);

            this.tbCriar.Text = "Criar";
            _toolBarManager.AddControl(tbCriar, DockStyle.Left);

            this.tbSalvar.Text = "Salvar/Abrir/Novo";
            
            holder3 = _toolBarManager.AddControl(stripArco, DockStyle.Left);
            holder3.ToolbarTitle = "Arcos";
            holder3.AllowedBorders = AllowedBorders.Top | AllowedBorders.Bottom | AllowedBorders.Left | AllowedBorders.Right;

            _toolBarManager2 = new ToolBarManager(this, this);
            
            holder1 = _toolBarManager2.AddControl(tbSalvar, DockStyle.Top);
            holder1.ToolbarTitle = "Novo/Abrir/Salvar";
            holder1.AllowedBorders = AllowedBorders.Top | AllowedBorders.Bottom | AllowedBorders.Left | AllowedBorders.Right;

            holder6 = _toolBarManager2.AddControl(panel1, DockStyle.Top);
            holder6.ToolbarTitle = "Piso";
            holder6.AllowedBorders = AllowedBorders.Top | AllowedBorders.Bottom | AllowedBorders.Left | AllowedBorders.Right;

        }

        public void CancelaTudo()
        {
            btLinha.Pushed   = false;
            btRet.Pushed     = false;
            btCirculo.Pushed = false;
            btTexto.Pushed   = false;
            btLinha.Pushed   = false;
        }

        private void inícioFimECentroToolStripMenuItem_Click(object sender, EventArgs e)
        {
            desenho.SetaInsercao(Const.ID_ARCOIMF);
        }

        private void capPorQuadrante_Click_1(object sender, EventArgs e)
        {
            capPorPontoRelativo.Checked = false;
            capPorPontoMedio.Checked = false;

            if (capPorQuadrante.Checked)
            {
                if (desenho.inserindo > -1)
                {
                    Comando2.Text = "   Quadrante - Deslocamento absoluto (x,y) do ponto:";
                }
                else
                    capPorQuadrante.Checked = false;
            }
            else
                Comando2.Text = "";
        }

        private void salvarToolStripMenuItem_Click(object sender, EventArgs e)
        {        
            string folder = Directory.GetCurrentDirectory(); 

            if (!Directory.Exists(folder))
                 Directory.CreateDirectory(folder);
           
            StreamWriter ss = new StreamWriter(folder+"\\tses.txt");
            
            for (int g = 0; g < 20000; g++)
              ss.WriteLine("aa" + g.ToString());
            
            ss.Close();                  
        }

        private void button4_Click(object sender, EventArgs e)
        {
            PanelCorFundo.Visible = false;
        }

        private void toolStripButton4_Click(object sender, EventArgs e)
        {
            PanelCorFundo.Visible = false;
            Desenho.r = 1;
            Desenho.g = 1;
            Desenho.b = 1;
        }

        private void toolStripButton5_Click(object sender, EventArgs e)
        {
            Desenho.r = 0.75f;
            Desenho.g = 0.75f;
            Desenho.b = 0.75f; 
            
            PanelCorFundo.Visible = false;
        }

        private void toolStripButton6_Click(object sender, EventArgs e)
        {
            Desenho.r = 0;
            Desenho.g = 0;
            Desenho.b = 0; 
            
            PanelCorFundo.Visible = false;
        }

        private void toolStripButton7_Click(object sender, EventArgs e)
        {
            if (!desenho.Visible)
            {
                desenho = new Desenho(this);
                desenho.MdiParent = this;
                desenho.Show();
                desenho.WindowState = System.Windows.Forms.FormWindowState.Maximized;
            };

            desenho.SetaInsercao(Const.ID_RETANGULO);
            Comando.Text = "Retângulo - Primeiro ponto:";
        }

        private void menuItem11_Click(object sender, EventArgs e)
        {
            Application.Exit();
        }

        private void pontosToolStripMenuItem_Click_1(object sender, EventArgs e)
        {
            desenho.SetaInsercao(Const.ID_ARCO_DOIS_P);
            Comando.Text = "Arco - Centro:";
        }

        private void tbVigas_ButtonClick(object sender, ToolBarButtonClickEventArgs e)
        {
            switch (tbVigas.Buttons.IndexOf(e.Button))
            {
                case 0:
                    {
                        if (!dadosviga.Visible)
                          dadosviga = new DadosViga(this);
                        
                        dadosviga.nova = true;

                        dadosviga.ShowDialog();

                        break;
                    }
                case 1:
                    {
                        if (!dadosviga.Visible)
                            dadosviga = new DadosViga(this);

                        dadosviga.Show();
                        dadosviga.Text = "Dados de viga em arco";
                        break;
                    }
            };
        }

        private void tbCriar_ButtonClick(object sender, ToolBarButtonClickEventArgs e)
        {
            switch (tbCriar.Buttons.IndexOf(e.Button))
            {
                case 0:
                    {
                        if (!desenho.Visible)
                        {
                            desenho = new Desenho(this);
                            desenho.MdiParent = this;
                            desenho.Show();
                            desenho.WindowState = System.Windows.Forms.FormWindowState.Maximized;
                        };

                        desenho.SetaInsercao(Const.ID_LINHA);
                        Comando.Text = "Linha - Primeiro ponto:";

                        if (btLinha.Pushed == true)
                        {
                            //     btLinha.Pushed = false;
                            desenho.CancelaInsercoes();
                        }
                        else
                            btLinha.Pushed = true;

                        break;
                    }
                case 1:
                    {
                        break;
                    }
            }
        }

        private void button1_Click(object sender, EventArgs e)
        {
            if (cbPiso.SelectedIndex + 1 < cbPiso.Items.Count)
              cbPiso.SelectedIndex += 1;
        }

        private void button2_Click(object sender, EventArgs e)
        {
            if (cbPiso.SelectedIndex - 1 >= 0)
              cbPiso.SelectedIndex -= 1;
        
        }

        private void DigitarComando_KeyDown(object sender, KeyEventArgs e)
        {

            if (e.KeyData == Keys.Escape)
                desenho.CancelaInsercoes();
            else
            if (e.KeyData == Keys.Enter && DigitarComando.Text == "" && desenho.inserindo == Const.ID_VIGA)
            {
                desenho.CriaViga();
            }
            else
            if (e.KeyData == Keys.Enter)
            {
                try
                {
                    if (DigitarComando.Text.Contains("@") && !DigitarComando.Text.Contains("<")) // coordenada relativa simples
                    {
                        string x = DigitarComando.Text.Substring(1, DigitarComando.Text.IndexOf(",") - 1).Replace(".", ",");
                        string y = DigitarComando.Text.Substring(DigitarComando.Text.IndexOf(",") + 1, DigitarComando.Text.Length - (x.Length) - 2).Trim().Replace(".", ",");

                        if (!capPorPontoRelativo.Checked)
                        {
                            desenho.clicouPrimeiro = true;
                            desenho.clicouSegundo = false;

                            if (desenho.inserindo == Const.ID_LINHA)
                                desenho.TestaCliqueLinha(Desenho.pixelX(desenho.Ponto1_Coord.X + System.Convert.ToSingle(x)),
                                                           Desenho.pixelY(desenho.Ponto1_Coord.Y + System.Convert.ToSingle(y)),
                                                           desenho.Ponto1_Coord.X + System.Convert.ToSingle(x),
                                                           desenho.Ponto1_Coord.Y + System.Convert.ToSingle(y));
                            else
                                if (desenho.inserindo == Const.ID_ARCO_DOIS_P)
                                    desenho.TestaCliqueArco(Desenho.pixelX(desenho.Ponto1_Coord.X + System.Convert.ToSingle(x)),
                                                            Desenho.pixelY(desenho.Ponto1_Coord.Y + System.Convert.ToSingle(y)),
                                                            desenho.Ponto1_Coord.X + System.Convert.ToSingle(x),
                                                            desenho.Ponto1_Coord.Y + System.Convert.ToSingle(y));
                                else
                                    if (desenho.inserindo == Const.ID_VIGA)
                                        desenho.TestaCliqueViga(Desenho.pixelX(desenho.Ponto1_Coord.X + System.Convert.ToSingle(x)),
                                                                Desenho.pixelY(desenho.Ponto1_Coord.Y + System.Convert.ToSingle(y)),
                                                                desenho.Ponto1_Coord.X + System.Convert.ToSingle(x),
                                                                desenho.Ponto1_Coord.Y + System.Convert.ToSingle(y));
                        }
                        else
                        {
                            if (desenho.inserindo == Const.ID_LINHA)
                              desenho.TestaCliqueLinha(Desenho.pixelX(System.Convert.ToSingle(x)),
                                                       Desenho.pixelY(System.Convert.ToSingle(y)),
                                                       System.Convert.ToSingle(x),
                                                       System.Convert.ToSingle(y));
                            else
                            if (desenho.inserindo == Const.ID_ARCO_DOIS_P)
                              desenho.TestaCliqueArco(Desenho.pixelX(System.Convert.ToSingle(x)),
                                                      Desenho.pixelY(System.Convert.ToSingle(y)),
                                                      System.Convert.ToSingle(x),
                                                      System.Convert.ToSingle(y));
                            else
                            if (desenho.inserindo == Const.ID_VIGA)
                              desenho.TestaCliqueViga(Desenho.pixelX(System.Convert.ToSingle(x)),
                                                      Desenho.pixelY(System.Convert.ToSingle(y)),
                                                      System.Convert.ToSingle(x),
                                                      System.Convert.ToSingle(y));
                        };
                    }
                    if (DigitarComando.Text.Contains("@") && DigitarComando.Text.Contains("<")) // coordenada relativa polar
                    {
                        string tamanho = DigitarComando.Text.Substring(1, DigitarComando.Text.IndexOf("<") - 1).Replace(".", ",");
                        string angulo = DigitarComando.Text.Substring(DigitarComando.Text.IndexOf("<") + 1, DigitarComando.Text.Length - (tamanho.Length) - 2).Trim().Replace(".", ",");

                        float ca = System.Convert.ToSingle(tamanho) * System.Convert.ToSingle(Math.Cos(System.Convert.ToDouble(angulo) * Const.PIDiv180));
                        float co = System.Convert.ToSingle(tamanho) * System.Convert.ToSingle(Math.Sin(System.Convert.ToDouble(angulo) * Const.PIDiv180));

                        if (!capPorPontoRelativo.Checked)
                        {
                            desenho.clicouPrimeiro = true;
                            desenho.clicouSegundo = false;

                            if (desenho.inserindo == Const.ID_LINHA)
                              desenho.TestaCliqueLinha(Desenho.pixelX(desenho.Ponto1_Coord.X + System.Convert.ToSingle(ca)),
                                                         Desenho.pixelY(desenho.Ponto1_Coord.Y + System.Convert.ToSingle(co)),
                                                         desenho.Ponto1_Coord.X + System.Convert.ToSingle(ca),
                                                         desenho.Ponto1_Coord.Y + System.Convert.ToSingle(co));
                            else
                            if (desenho.inserindo == Const.ID_ARCO_DOIS_P)
                              desenho.TestaCliqueArco(Desenho.pixelX(desenho.Ponto1_Coord.X + System.Convert.ToSingle(ca)),
                                                             Desenho.pixelY(desenho.Ponto1_Coord.Y + System.Convert.ToSingle(co)),
                                                             desenho.Ponto1_Coord.X + System.Convert.ToSingle(ca),
                                                             desenho.Ponto1_Coord.Y + System.Convert.ToSingle(co));
                            else
                            if (desenho.inserindo == Const.ID_VIGA)
                              desenho.TestaCliqueViga(Desenho.pixelX(desenho.Ponto1_Coord.X + System.Convert.ToSingle(ca)),
                                                             Desenho.pixelY(desenho.Ponto1_Coord.Y + System.Convert.ToSingle(co)),
                                                             desenho.Ponto1_Coord.X + System.Convert.ToSingle(ca),
                                                             desenho.Ponto1_Coord.Y + System.Convert.ToSingle(co));
                        }
                        else
                        {
                            if (desenho.inserindo == Const.ID_LINHA)
                              desenho.TestaCliqueLinha(Desenho.pixelX(System.Convert.ToSingle(ca)),
                                                       Desenho.pixelY(System.Convert.ToSingle(co)),
                                                       System.Convert.ToSingle(ca),
                                                       System.Convert.ToSingle(co));
                            else
                            if (desenho.inserindo == Const.ID_ARCO_DOIS_P)
                              desenho.TestaCliqueArco(Desenho.pixelX(System.Convert.ToSingle(ca)),
                                                      Desenho.pixelY(System.Convert.ToSingle(co)),
                                                      System.Convert.ToSingle(ca),
                                                      System.Convert.ToSingle(co));
                            else
                            if (desenho.inserindo == Const.ID_VIGA)
                              desenho.TestaCliqueViga(Desenho.pixelX(System.Convert.ToSingle(ca)),
                                                      Desenho.pixelY(System.Convert.ToSingle(co)),
                                                      System.Convert.ToSingle(ca),
                                                      System.Convert.ToSingle(co));
                        };
                    }
                    else  //coordenada simples
                    if (!DigitarComando.Text.Contains("@") && !DigitarComando.Text.Contains("<")) // coordenada simples
                    {
                        string x = DigitarComando.Text.Substring(0, DigitarComando.Text.IndexOf(",")).Replace(".", ",");
                        string y = DigitarComando.Text.Substring(DigitarComando.Text.IndexOf(",") + 1, DigitarComando.Text.Length - (x.Length) - 1).Trim().Replace(".", ",");

                        if (desenho.inserindo == Const.ID_LINHA)
                          desenho.TestaCliqueLinha(Desenho.pixelX(System.Convert.ToSingle(x)),
                                                   Desenho.pixelY(System.Convert.ToSingle(y)),
                                                   System.Convert.ToSingle(x),
                                                   System.Convert.ToSingle(y));
                        else
                        if (desenho.inserindo == Const.ID_ARCO_DOIS_P)
                          desenho.TestaCliqueArco(Desenho.pixelX(System.Convert.ToSingle(x)),
                                                  Desenho.pixelY(System.Convert.ToSingle(y)),
                                                  System.Convert.ToSingle(x),
                                                  System.Convert.ToSingle(y));
                        else
                        if (desenho.inserindo == Const.ID_VIGA)
                          desenho.TestaCliqueViga(Desenho.pixelX(System.Convert.ToSingle(x)),
                                                  Desenho.pixelY(System.Convert.ToSingle(y)),
                                                  System.Convert.ToSingle(x),
                                                  System.Convert.ToSingle(y));

                    }
                }

                catch (System.ArgumentOutOfRangeException ex)
                {
                    MessageBox.Show("Comando inválido!\n\n Possíveis motivos: Usou vírgula como separdor decimal ? Nesse caso, use ponto ''.'' " +
                                    "\n Usou comando alfanumérico? ");
                }
                catch (System.FormatException)
                {
                    MessageBox.Show("Comando inválido!\n\n Possíveis motivos: \n Usou vírgula como separdor decimal ? Nesse caso, use ponto '.' " +
                                    "\n Usou comando alfanumérico? Nesse caso, use somente números. ");
                }
            };
        }

        private void capPorPontoRelativo_Click_1(object sender, EventArgs e)
        {
            capPorPontoMedio.Checked = false;
            capPorQuadrante.Checked = false;

            if (capPorPontoRelativo.Checked)
            {
                if (desenho.inserindo > -1)
                {
                    Comando2.Text = " Desloc. relativo - Ponto base:";
                }
                else
                    capPorPontoRelativo.Checked = false;
            }
            else
                Comando2.Text = "";
        }

        private void capPorPontoMedio_Click(object sender, EventArgs e)
        {
            capPorPontoRelativo.Checked = false;
            capPorQuadrante.Checked = false;

            if (capPorPontoMedio.Checked)
            {
                if (desenho.inserindo > -1)
                {
                    Comando2.Text = " Ponto médio a dois pontos - Primeiro ponto:";
                }
                else
                    capPorPontoMedio.Checked = false;
            }
            else
                Comando2.Text = "";
        }

        private void toolStripButton3_Click(object sender, EventArgs e)
        {
            PanelCorFundo.Left = (this.Width) - (PanelCorFundo.Width) - 20;
            PanelCorFundo.Top = (this.Height) - (PanelCorFundo.Height) - 90;
            PanelCorFundo.Visible = true;
        }

        private void button3_Click(object sender, EventArgs e)
        {
                EdtGrelha = new EditorGrelha();
                EdtGrelha.MdiParent = this;
                EdtGrelha.Show();
                EdtGrelha.WindowState = System.Windows.Forms.FormWindowState.Maximized;
        }

        private void toolStripDropDownButton3_Click(object sender, EventArgs e)
        {
            if (!captura.Visible)
                captura = new OpcoesCaptura();
            captura.ShowDialog();
        }

        private void DigitarComando_KeyPress(object sender, KeyPressEventArgs e)
        {

            if (e.KeyChar == (char)Keys.Enter || e.KeyChar == (char)Keys.Escape)
            {
                e.Handled = true;   // stop annoying beep
            }

            // call base handler...
            base.OnKeyPress(e);

            if (desenho.inserindo == Const.ID_VIGA && e.KeyChar == 102)//alterna face de inserção da viga
            {
                desenho.AlternarFaceViga();
                DigitarComando.Text = "";
            }
        }

        private void DigitarComando_KeyUp(object sender, KeyEventArgs e)
        {
            if (desenho.inserindo == Const.ID_VIGA)
            {
               if (e.KeyData == Keys.F)//alterna face de inserção da viga
                 DigitarComando.Clear();
               
               if (e.KeyData == Keys.Enter)
               {
                  if (DigitarComando.Text.Contains("b="))
                  {
                      string tamanho = DigitarComando.Text.Substring(DigitarComando.Text.IndexOf("=") + 1, 10).Replace(".", ",");
                      double b1      = System.Convert.ToDouble(DigitarComando.Text.Substring(DigitarComando.Text.IndexOf("=") + 1, DigitarComando.Text.Length - (tamanho.Length) - 2).Trim().Replace(".", ","));
                      desenho.DadosTrecho.b1 = b1;
                  }
               }
            }
        }

        private void toolBar1_ButtonClick(object sender, ToolBarButtonClickEventArgs e)
        {
            switch (tbSalvar.Buttons.IndexOf(e.Button))
            {
                case 6:
                    {
                        CriaFormInicial();
                        break;
                    }
                case 5:
                    {
                        if (desenho.Visible)
                            desenho.Enquadrar();
                        break;
                    }
                case 2:
                    {
                        if (desenho.Visible)
                            desenho.Salvar();
                        break;
                    }
                case 1:
                    {
                       
                        desenho.Abrir();
                        break;
                    }
                case 0:
                    {

                        CriaFormInicial();
                        break;
                    }
            }
        }

        private void sairToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Environment.Exit(0);
        }
    }
}



/*        try 
        {
            // Get the current directory.
            string path = Directory.GetCurrentDirectory();
            string target = @"c:\temp";
            Console.WriteLine("The current directory is {0}", path);
            if (!Directory.Exists(target)) 
            {
                Directory.CreateDirectory(target);
            }

            // Change the current directory.
            Environment.CurrentDirectory = (target);
            if (path.Equals(Directory.GetCurrentDirectory())) 
            {
                Console.WriteLine("You are in the temp directory.");
            } 
            else 
            {
                Console.WriteLine("You are not in the temp directory.");
            }
        } 
        catch (Exception e) 
        {
            Console.WriteLine("The process failed: {0}", e.ToString());
        }
 */