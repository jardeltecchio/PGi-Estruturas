using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
/*using Tao.OpenGl;
using Tao.Platform;
using Tao.FreeGlut;*/
using System.Threading;

using OpenTK;
using OpenTK.Input;
using OpenTK.Graphics.OpenGL;
using OpenTK.Platform;

namespace PG
{
    public unsafe partial class FVisualizadorPortico : WeifenLuo.WinFormsUI.Docking.DockContent//
    {
        public static int h, w;
        Cursor Cur_rotacao ;//= new Cursor(PGi.Properties.Resources.rotate1.Handle);
        public static float r, g, b;

        private static float _dragPosX = (float).0, _dragPosY = (float).0, _dragPosZ = (float).0;
        double posX, posY, posZ, centroX, centroY;
        double x_angle = 15, y_angle = 15;
        double zant = 0;
        int ultx, ulty;
        double[] FatorEscalaEsforco;
        double start_x = 0, start_y = 0;
        double bx, by, bz;
        float twicePi = 2.0f * 3.1415f;
        double ultZ, ultX, ultY;
        double xNo, yNo, zNo;
        Gerenciador gerenciador;
        public TPorticoEspacial Portico;
        float p_xini, p_xfin, p_yini, p_yfin;
        public bool NovoNo, NovaBarra;

        double FatorEscalaDeslocamento;
        public double iEscalaDiagrama = 3, iEscalaCarga = 3;
   
        public double[] Deslocamentos;
        public double[,] RGB_Deslocamentos;

        public double[] Axiais;
        public double[] MomentosNegativos;
        public double[] MomentosPositivosY;
        public double[,] RGB_FletoresNegativos;
        public double[,] RGB_FletoresPositivos;

        public double[,] RGB_Axiais;
  
        public double[] MomentosTorcoresNegativos;
        public double[] MomentosTorcoresPositivos;
        public double[,] RGB_TorcoresNegativos;
        public double[,] RGB_TorcoresPositivos;

        public double[] CortantesNegativosY;
        public double[] CortantesPositivosY;
        public double[,] RGB_CortantesNegativos;
        public double[,] RGB_CortantesPositivos;

        double[,] ListaRGB_EsforcosNegativos;
        double[,] ListaRGB_EsforcosPositivos;
        double[,] ListaRGB_Axiais;
        public List<double> CargasLineares;
        public List<double> CargasPontuais;

        public const int iFletorY = 0;
        public const int iFletorZ = 1;
        public const int iTorcor = 2;
        public const int iCortanteY = 3;
        public const int iCortanteZ = 4;
        public const int iDeslocamento = 5;


        public bool FletorY, FletorZ, CortanteY, CortanteZ, Deslocamento, Torcor, Axial; 

       /* int iDeslocamentos = 0, 
            iFletoresPositivosY = 0, iFletoresNegativosY = 0,iFletoresPositivosZ = 0, iFletoresNegativosZ = 0,
            iTorcoresNegativos = 0, iTorcoresPositivos = 0,
            iCortantesNegativosZ, iCortantesPositivosZ, iCortantesNegativosY, iCortantesPositivosY = 0;*/
     
        int iDeslocamentos = 0, iAxiais, iFletoresPositivosY = 0, iFletoresNegativos = 0, iTorcoresNegativos = 0, iTorcoresPositivos = 0, iCortantesNegativos, iCortantesPositivos = 0;
    
        public int iEsforcoAtual = iDeslocamento;
        public FVisualizadorPortico()
        {
            InitializeComponent();
        }

        public void AtualizaPorticoManual()
        {
            CriaVetores();
            CriaFormCores();
            AtualizaDeslocamentos();
            AtualizaEsforcos("");
            Fatores();
        }

        public FVisualizadorPortico(Gerenciador gerenciador, bool manual, ref TPorticoEspacial portico)
        {
            InitializeComponent();
        
           // Controle.InitializeContexts();

            this.gerenciador = gerenciador;

            this.Portico = portico; //gerenciador.PorticoEspacial;

            if (Portico != null)
            {
                if (Portico.calculoOk)
                {
                    CriaVetores();
                    CriaFormCores();
                    AtualizaDeslocamentos();
                    AtualizaEsforcos("");
                }
            }
            else
            {
                this.Portico = new TPorticoEspacial(this.gerenciador, true);
                gerenciador.PorticoEspacial = this.Portico;
                this.Portico.Manual = true;
            }

            CargasPontuais = new List<double>();
            CargasLineares = new List<double>();
        }

        public void Atualizar(bool avulso = false)
        {
            InicializarOGL();
            GerarModelo3(avulso);
        }
        Matrix4 p;
        double GLtop,GLbotton,GLleft,GLright, zNear, zFar;
        public bool Perspectiva = false;
        public void SetupCamera(bool orto = false)
        {
            GL.Viewport(0, 0, Controle.Width, Controle.Height);
            GL.MatrixMode(MatrixMode.Projection);

            if (! Perspectiva)
            {
                GLtop    = 1;
                GLbotton = -1;
                GLleft   = -(double)(this.Width) / (double)this.Height;
                GLright  = -GLleft;
                zNear    = -10000;
                zFar     = 10000;

                GL.Ortho(GLleft, GLright, GLbotton, GLtop, zNear, zFar);
            }
            else
            {
                p = Matrix4.CreatePerspectiveFieldOfView(MathHelper.PiOver4, this.Width / (float)this.Height, 0.1f, 100000f);
                GL.LoadMatrix(ref p);
            }

            //GL.MatrixMode(MatrixMode.Modelview);
            ///  GL.LoadIdentity(); 
            //          GL.Ortho(-1.0, 1.0, -1.0, 1.0, 0.0, 1000);

            //            Matrix4 lookat = Matrix4.LookAt(0, 0, 0, 0, 0, 0, 0, 0, 0);

        }
        public void GerarModelo3(bool avulso = false)
        {
            try
            {
                //  if (Portico.nNos > 0)
                {
                    //   TOpenGl.AssociateOGL(this.Controle);
                    //   TOpenGl.InitializeOGL();
                    //    TOpenGl.ReShapeOGL(Controle.Width, Controle.Height, angx, angy, angz);
                    Perspectiva = gerenciador.ConfiguracoesPGi.PorticoOpcoesVisualizacao.Perspectiva;
                    SetupCamera();

                    if (gerenciador.Pavimentos.Count < 2)
                    {
                        centroX = gerenciador.Pavimentos[0].xi + ((gerenciador.Pavimentos[0].xf - gerenciador.Pavimentos[0].xi) / 2);
                        centroY = gerenciador.Pavimentos[0].yi + ((gerenciador.Pavimentos[0].yf - gerenciador.Pavimentos[0].yi) / 2);
                    }
                    else
                    {
                        int pav = 0;

                        for (pav = gerenciador.Pavimentos.Count - 1; pav >= 0; pav--)
                            if (gerenciador.Pavimentos[pav].vigas.Count > 0)
                                break;
                        if (pav == -1)
                            pav = 0;

                        centroX = gerenciador.Pavimentos[pav].xi + ((gerenciador.Pavimentos[pav].xf - gerenciador.Pavimentos[pav].xi) / 2);
                        centroY = gerenciador.Pavimentos[pav].yi + ((gerenciador.Pavimentos[pav].yf - gerenciador.Pavimentos[pav].yi) / 2);
                    }

                    gerenciador.ChamaAguardar(this, "Gerando visualização. Aguarde...");

                    if (Portico.nBarras > 0)
                        Fatores();

                    /*   iDeslocamentos = 0; iFletoresPositivosY = 0; iFletoresNegativosY = 0;iFletoresPositivosZ = 0; iFletoresNegativosZ = 0;
                       iTorcoresNegativos = 0; iTorcoresPositivos = 0; iCortantesNegativosZ = 0; iCortantesPositivosZ = 0; iCortantesNegativosY = 0; iCortantesPositivosY = 0;
                      */
                    iDeslocamentos = 0; iFletoresPositivosY = 0; iFletoresNegativos = 0; iTorcoresNegativos = 0; iTorcoresPositivos = 0; iCortantesNegativos = 0; iCortantesPositivos = 0;

                    lbInfoNo.Visible = true;

                    gerenciador.FechaAguardar();
                    lbInfoNo.Visible = true;

                    Render();

                    GL.Flush();

                    //    Enquadrar();

                    if (Portico.Manual)
                    {
                        if (!Portico.CalculouEsforcos)
                            this.Text += " - NÃO CALCULADO.";
                        //    VistaPerspectiva(false);
                    }

                    if (!Portico.Manual)
                        Enquadrar();

                    Controle.SwapBuffers();
                    Render();
                    //  if (Portico.Manual)
                    //   VistaPerspectiva(false);
                }
            }
            catch(Exception ms)
            {
                MessageBox.Show(ms.Message);
            }
        }

        void Fatores()
        {
           if (Portico.MaximoDeslocamento(0,0,0) == 0)
               FatorEscalaDeslocamento = 1;
           else
               FatorEscalaDeslocamento = 20 / Portico.MaximoDeslocamento(0,0,0);

           FatorEscalaEsforco = new double[6];
           FatorEscalaEsforco[0] = 20 / Portico.MaximoEsforco(6); //fletor y
           FatorEscalaEsforco[1] = 20 / Portico.MaximoEsforco(1);
           FatorEscalaEsforco[2] = 20 / Portico.MaximoEsforco(3);
        }

        public void AtualizaDeslocamentos()
        {
            OrdenaDeslocamentos();
            AtribuiCoresDeslocamentos();
        }
        void AtribuiCoresAxiais(ref double[] Esforco, ref double[,] RGB, ref double[,] ListaRGB_Esforco, int iTotalEsforcos, int iTotalCores)
        {
            if (iTotalEsforcos > 0)
            {
                RGB = new double[iTotalCores, 5];

                double max = Esforco[0];
                double min = Esforco[iTotalEsforcos - 1];
                double dif = max - min;
                double intervalo = dif / iTotalCores;

                for (i = 0; i < iTotalCores; i++)
                {
                    //Intervalo
                    RGB[i, 0] = max - (intervalo * i);
                    RGB[i, 1] = max - (intervalo * (i + 1));

                    //Cores
                    RGB[i, 2] = ListaRGB_Esforco[i, 0];
                    RGB[i, 3] = ListaRGB_Esforco[i, 1];
                    RGB[i, 4] = ListaRGB_Esforco[i, 2];
                }

                for (i = 1; i <= Portico.nBarras; i++)
                {
                    if (Portico.barras[i].barraRigida) continue;
                    if (!Portico.barras[i].barraPilar) continue;

                    Portico.barras[i].RGB_Axiais = new double[iTotalCores, 5];
                    Portico.barras[i].RGB_Axiais = RGB;
                }
            }
        }

        void AtribuiCoresEsforcos(string NomeEsforco, ref double[] Esforco, ref double[,] RGB, ref double[,] ListaRGB_Esforco, int iTotalEsforcos, int iTotalCores)
        {
            if (iTotalEsforcos > 0)
            {
                RGB = new double[iTotalCores, 5];

                double max = Esforco[0];
                double min = Esforco[iTotalEsforcos - 1];
                double dif = max - min;
                double intervalo = dif / iTotalCores;

                for (i = 0; i < iTotalCores; i++)
                {
                    //Intervalo
                    RGB[i, 0] = max - (intervalo * i);
                    RGB[i, 1] = max - (intervalo * (i + 1));

                    //Cores
                    RGB[i, 2] = ListaRGB_Esforco[i, 0];
                    RGB[i, 3] = ListaRGB_Esforco[i, 1];
                    RGB[i, 4] = ListaRGB_Esforco[i, 2];
                }

                for (i = 1; i <= Portico.nBarras; i++)
                {
                    if (Portico.barras[i].barraRigida) continue;

                    /*if (NomeEsforco == "fletor z" && Portico.barras[i].barraPilar)
                    {
                        Portico.barras[i].RGB_FletoresNegativos = new double[iTotalCores, 5];
                        Portico.barras[i].RGB_FletoresNegativos = RGB;
                    }
                    else
                    if (NomeEsforco == "fletor y negativo")
                    {
                        Portico.barras[i].RGB_FletoresNegativos = new double[iTotalCores, 5];
                        Portico.barras[i].RGB_FletoresNegativos = RGB;
                    }
                    else
                    if (NomeEsforco == "fletor y positivo")
                    {
                        Portico.barras[i].RGB_FletoresPositivos = new double[iTotalCores, 5];
                        Portico.barras[i].RGB_FletoresPositivos = RGB;
                    }*/

                    if (NomeEsforco == "TORCOR NEGATIVO")
                    {
                        Portico.barras[i].RGB_TorcoresNegativos = new double[iTotalCores, 5];
                        Portico.barras[i].RGB_TorcoresNegativos = RGB;
                    }
                    else
                    if (NomeEsforco == "TORCOR POSITIVO")
                    {
                        Portico.barras[i].RGB_TorcoresPositivos = new double[iTotalCores, 5];
                        Portico.barras[i].RGB_TorcoresPositivos = RGB;
                    }

                    if (NomeEsforco == "CORTANTE NEGATIVO")
                    {
                        Portico.barras[i].RGB_CortantesNegativos = new double[iTotalCores, 5];
                        Portico.barras[i].RGB_CortantesNegativos = RGB;
                    }
                    else
                    if (NomeEsforco == "CORTANTE POSITIVO")
                    {
                        Portico.barras[i].RGB_CortantesPositivos = new double[iTotalCores, 5];
                        Portico.barras[i].RGB_CortantesPositivos = RGB;
                    }
                }
            }
        }

        void AtribuiCoresDeslocamentos()
        {
            RGB_Deslocamentos = new double[10, 5];

            double max = Deslocamentos[0];
            double min = Deslocamentos[iDeslocamentos - 1];
            double dif = max - min;
            double intervalo = dif / 10;

            RGB_Deslocamentos[0, 0] = max;
            RGB_Deslocamentos[0, 1] = max - intervalo;
            RGB_Deslocamentos[0, 2] = pnd10.BackColor.R;  //RGB's    azul
            RGB_Deslocamentos[0, 3] = pnd10.BackColor.G;
            RGB_Deslocamentos[0, 4] = pnd10.BackColor.B;

            RGB_Deslocamentos[1, 0] = max - intervalo;
            RGB_Deslocamentos[1, 1] = max - intervalo * 2;
            RGB_Deslocamentos[1, 2] = pnd9.BackColor.R;  //RGB's    azul claro
            RGB_Deslocamentos[1, 3] = pnd9.BackColor.G;
            RGB_Deslocamentos[1, 4] = pnd9.BackColor.B;

            RGB_Deslocamentos[2, 0] = max - intervalo * 2;
            RGB_Deslocamentos[2, 1] = max - intervalo * 3;
            RGB_Deslocamentos[2, 2] = pnd8.BackColor.R;  //RGB's   azul mais claro
            RGB_Deslocamentos[2, 3] = pnd8.BackColor.G;
            RGB_Deslocamentos[2, 4] = pnd8.BackColor.B;

            RGB_Deslocamentos[3, 0] = max - intervalo * 3;
            RGB_Deslocamentos[3, 1] = max - intervalo * 4;
            RGB_Deslocamentos[3, 2] = pnd7.BackColor.R;//RGB's   amarelo claro
            RGB_Deslocamentos[3, 3] = pnd7.BackColor.G;
            RGB_Deslocamentos[3, 4] = pnd7.BackColor.B;

            RGB_Deslocamentos[4, 0] = max - intervalo * 4;
            RGB_Deslocamentos[4, 1] = max - intervalo * 5;
            RGB_Deslocamentos[4, 2] = pnd6.BackColor.R; //RGB's   amarelo
            RGB_Deslocamentos[4, 3] = pnd6.BackColor.G;
            RGB_Deslocamentos[4, 4] = pnd6.BackColor.B;

            RGB_Deslocamentos[5, 0] = max - intervalo * 5;
            RGB_Deslocamentos[5, 1] = max - intervalo * 6;
            RGB_Deslocamentos[5, 2] = pnd5.BackColor.R;  //RGB's    laranja claro
            RGB_Deslocamentos[5, 3] = pnd5.BackColor.G;
            RGB_Deslocamentos[5, 4] = pnd5.BackColor.B;

            RGB_Deslocamentos[6, 0] = max - intervalo * 6;
            RGB_Deslocamentos[6, 1] = max - intervalo * 7;
            RGB_Deslocamentos[6, 2] = pnd4.BackColor.R;  //RGB's    laranja
            RGB_Deslocamentos[6, 3] = pnd4.BackColor.G;
            RGB_Deslocamentos[6, 4] = pnd4.BackColor.B;

            RGB_Deslocamentos[7, 0] = max - intervalo * 7;
            RGB_Deslocamentos[7, 1] = max - intervalo * 8;
            RGB_Deslocamentos[7, 2] = pnd3.BackColor.R; //RGB's    vermelho
            RGB_Deslocamentos[7, 3] = pnd3.BackColor.G;
            RGB_Deslocamentos[7, 4] = pnd3.BackColor.B;

            RGB_Deslocamentos[8, 0] = max - intervalo * 8;
            RGB_Deslocamentos[8, 1] = max - intervalo * 9;
            RGB_Deslocamentos[8, 2] = pnd2.BackColor.R; //RGB's    vermelho
            RGB_Deslocamentos[8, 3] = pnd2.BackColor.G;
            RGB_Deslocamentos[8, 4] = pnd2.BackColor.B;

            RGB_Deslocamentos[9, 0] = max - intervalo * 9;
            RGB_Deslocamentos[9, 1] = max - intervalo * 10;
            RGB_Deslocamentos[9, 2] = pnd1.BackColor.R; //RGB's    vermelho
            RGB_Deslocamentos[9, 3] = pnd1.BackColor.G;
            RGB_Deslocamentos[9, 4] = pnd1.BackColor.B;

            for (i = 1; i <= Portico.nBarras; i++)
            {
                Portico.barras[i].RGB_Deslocamentos = new double[10, 5];
                Portico.barras[i].RGB_Deslocamentos = this.RGB_Deslocamentos;
            }
        }
        void CriaFormCores()
        {
            PanelCores.Visible = true;
        }
        public void AtualizaEsforcos(string texto)
        {
            try
            {
                //preenche as duas listas dos positovos e negativos e ordena os valores
                
             //   Preenche_e_Ordena_Esforcos("fletor y", ref MomentosPositivosY, ref MomentosNegativos, ref iFletoresNegativos, ref iFletoresPositivosY);
                Preenche_e_Ordena_Esforcos_Fletor_Y_Vigas();
                Preenche_e_Ordena_Esforcos_Fletor_Y_Pilares();
                Preenche_e_Ordena_Esforcos_Fletor_Z_Pilares();
                Preenche_e_Ordena_Esforcos_Axial_Pilares();

                //  OrdenaEsforcos("MOMENTO TORCOR", ref MomentosTorcoresPositivos, ref MomentosTorcoresNegativos, ref iTorcoresNegativos, ref iTorcoresPositivos);
              //  OrdenaEsforcos("ESFORCO CORTANTE", ref CortantesPositivosY, ref CortantesNegativosY, ref iCortantesNegativos, ref iCortantesPositivos);

                if (iFletoresNegativos > 0)
                    AtribuiCoresEsforcos("fletor y negativo", ref MomentosNegativos, ref RGB_FletoresNegativos, ref ListaRGB_EsforcosNegativos, iFletoresNegativos, 6);
                if (iFletoresPositivosY > 0)
                    AtribuiCoresEsforcos("fletor y positivo", ref MomentosPositivosY, ref RGB_FletoresPositivos, ref ListaRGB_EsforcosPositivos, iFletoresPositivosY, 4);

                if (iFletoresNegativos > 0)
                    AtribuiCoresEsforcos("fletor z", ref MomentosNegativos, ref RGB_FletoresNegativos, ref ListaRGB_EsforcosNegativos, iFletoresNegativos, 6);

                AtribuiCoresAxiais(ref Axiais, ref RGB_Axiais, ref ListaRGB_EsforcosNegativos, iAxiais, 6);
                
                /*  if (iTorcoresNegativos > 0)
                      AtribuiCoresEsforcos("TORCOR NEGATIVO", ref MomentosTorcoresNegativos, ref RGB_TorcoresNegativos, ref ListaRGB_EsforcosNegativos, iTorcoresNegativos, 6);
                  if (iTorcoresPositivos > 0)
                      AtribuiCoresEsforcos("TORCOR POSITIVO", ref MomentosTorcoresPositivos, ref RGB_TorcoresPositivos, ref ListaRGB_EsforcosPositivos, iTorcoresPositivos, 4);

                  if (iCortantesNegativos > 0)
                      AtribuiCoresEsforcos("CORTANTE NEGATIVO", ref CortantesNegativosY, ref RGB_CortantesNegativos, ref ListaRGB_EsforcosNegativos, iCortantesNegativos, 6);
                  if (iCortantesPositivos > 0)
                      AtribuiCoresEsforcos("CORTANTE POSITIVO", ref CortantesPositivosY, ref RGB_CortantesPositivos, ref ListaRGB_EsforcosPositivos, iCortantesPositivos, 4);
                  */
                AtualizaFormCores();

                CancelaOperacoes();
            }
            catch (Exception ms)
            {
                MessageBox.Show("AtualizaEsforcos: " + ms.Message);
            }

            gerenciador.FechaAguardar();

         /*   if (gerenciador.ConfiguracoesPGi.GrelhaOpcoesVisualizacao.Suavizar)
            {
                GL.Enable(Gl.GL_LINE_SMOOTH);
                GL.Enable(Gl.GL_BLEND);
                GL.BlendFunc(Gl.GL_SRC_ALPHA, Gl.GL_ONE_MINUS_SRC_ALPHA);
                GL.Hint(Gl.GL_LINE_SMOOTH_HINT, Gl.GL_DONT_CARE);
            }
            else
            {
                GL.Disable(Gl.GL_LINE_SMOOTH);
                GL.Disable(Gl.GL_BLEND);
                GL.BlendFunc(Gl.GL_SRC_ALPHA, Gl.GL_ONE_MINUS_SRC_ALPHA);
                GL.Hint(Gl.GL_LINE_SMOOTH_HINT, Gl.GL_DONT_CARE);
            }*/
        }
        void Preenche_e_Ordena_Esforcos_Fletor_Z_Pilares()
        {
            double temp;
            var barras = (from b in Portico.barras where b != null && b.barraPilar && !b.barraRigida select b);
            TBarraPortico bar;
            foreach (var b in barras)
            {
                bar = b as TBarraPortico;
                //   if (!Portico.barras[i].barraViga) continue;
                /* if (!gerenciador.ConfiguracoesPGi.GrelhaOpcoesVisualizacao.Diagramas_DirecaoX &&
                      Portico.barras[i].direcaoX) continue;

                 if (!gerenciador.ConfiguracoesPGi.GrelhaOpcoesVisualizacao.Diagramas_DirecaoY &&
                      Pavimento.barras[i].direcaoY) continue;*/

                /*if (gerenciador.ConfiguracoesPGi.GrelhaOpcoesVisualizacao.Diagramas_MostrarBarrasViga &&
                   !gerenciador.ConfiguracoesPGi.GrelhaOpcoesVisualizacao.Diagramas_MostrarBarrasLaje && !Pavimento.barras[i].barraViga) continue;

                if (!gerenciador.ConfiguracoesPGi.GrelhaOpcoesVisualizacao.Diagramas_MostrarBarrasViga &&
                     gerenciador.ConfiguracoesPGi.GrelhaOpcoesVisualizacao.Diagramas_MostrarBarrasLaje && Pavimento.barras[i].barraViga) continue;

                if (!gerenciador.ConfiguracoesPGi.GrelhaOpcoesVisualizacao.Diagramas_MostrarBarrasViga &&
                     !gerenciador.ConfiguracoesPGi.GrelhaOpcoesVisualizacao.Diagramas_MostrarBarrasLaje) continue;*/

                MomentosNegativos[iFletoresNegativos++] = Math.Abs(bar.Esforcos[5]);
                MomentosNegativos[iFletoresNegativos++] = Math.Abs(bar.Esforcos[11]);
            }

            for (j = 0; j < iFletoresNegativos; j++)
            {
                for (k = j; k < iFletoresNegativos; k++)
                {
                    if (MomentosNegativos[k] > MomentosNegativos[j])
                    {
                        temp = MomentosNegativos[j];
                        MomentosNegativos[j] = MomentosNegativos[k];
                        MomentosNegativos[k] = temp;
                    };
                };
            };
        }
    
        void Preenche_e_Ordena_Esforcos_Fletor_Y_Vigas()
        {
            double temp;
            var barras = (from b in Portico.barras where b!=null && b.barraViga /*&& !b.barraRigida*/ select b);
            TBarraPortico bar;
            foreach (var b in barras)
            {
                bar = b as TBarraPortico;

                //   if (!Portico.barras[i].barraViga) continue;
                /* if (!gerenciador.ConfiguracoesPGi.GrelhaOpcoesVisualizacao.Diagramas_DirecaoX &&
                      Portico.barras[i].direcaoX) continue;

                 if (!gerenciador.ConfiguracoesPGi.GrelhaOpcoesVisualizacao.Diagramas_DirecaoY &&
                      Pavimento.barras[i].direcaoY) continue;*/

                /*if (gerenciador.ConfiguracoesPGi.GrelhaOpcoesVisualizacao.Diagramas_MostrarBarrasViga &&
                   !gerenciador.ConfiguracoesPGi.GrelhaOpcoesVisualizacao.Diagramas_MostrarBarrasLaje && !Pavimento.barras[i].barraViga) continue;

                if (!gerenciador.ConfiguracoesPGi.GrelhaOpcoesVisualizacao.Diagramas_MostrarBarrasViga &&
                     gerenciador.ConfiguracoesPGi.GrelhaOpcoesVisualizacao.Diagramas_MostrarBarrasLaje && Pavimento.barras[i].barraViga) continue;

                if (!gerenciador.ConfiguracoesPGi.GrelhaOpcoesVisualizacao.Diagramas_MostrarBarrasViga &&
                     !gerenciador.ConfiguracoesPGi.GrelhaOpcoesVisualizacao.Diagramas_MostrarBarrasLaje) continue;*/

                if (bar.fletorY_inicial_viga > bar.pIni.z)
                    MomentosNegativos[iFletoresNegativos++] = Math.Abs(bar.Esforcos[6]);
                else
                    MomentosPositivosY[iFletoresPositivosY++] = Math.Abs(bar.Esforcos[6]);

                if (bar.fletorY_final_viga < bar.pIni.z)
                    MomentosPositivosY[iFletoresPositivosY++] = Math.Abs(bar.Esforcos[12]);
                else
                    MomentosNegativos[iFletoresNegativos++] = Math.Abs(bar.Esforcos[12]);
            }

            for (j = 0; j < iFletoresPositivosY; j++)
            {
                for (k = j; k < iFletoresPositivosY; k++)
                {
                    if (MomentosPositivosY[k] > MomentosPositivosY[j])
                    {
                        temp = MomentosPositivosY[j];
                        MomentosPositivosY[j] = MomentosPositivosY[k];
                        MomentosPositivosY[k] = temp;
                    };
                };
            };

            for (j = 0; j < iFletoresNegativos; j++)
            {
                for (k = j; k < iFletoresNegativos; k++)
                {
                    if (MomentosNegativos[k] > MomentosNegativos[j])
                    {
                        temp = MomentosNegativos[j];
                        MomentosNegativos[j] = MomentosNegativos[k];
                        MomentosNegativos[k] = temp;
                    };
                };
            };
        }
        void Preenche_e_Ordena_Esforcos_Fletor_Y_Pilares()
        {
            double temp;

            var barras = (from b in Portico.barras where b != null && b.barraPilar && !b.barraRigida select b);
            TBarraPortico bar;
            foreach (var b in barras)
            {
                bar = b as TBarraPortico;
                //   if (!Portico.barras[i].barraViga) continue;
                /* if (!gerenciador.ConfiguracoesPGi.GrelhaOpcoesVisualizacao.Diagramas_DirecaoX &&
                      Portico.barras[i].direcaoX) continue;

                 if (!gerenciador.ConfiguracoesPGi.GrelhaOpcoesVisualizacao.Diagramas_DirecaoY &&
                      Pavimento.barras[i].direcaoY) continue;*/

                /*if (gerenciador.ConfiguracoesPGi.GrelhaOpcoesVisualizacao.Diagramas_MostrarBarrasViga &&
                   !gerenciador.ConfiguracoesPGi.GrelhaOpcoesVisualizacao.Diagramas_MostrarBarrasLaje && !Pavimento.barras[i].barraViga) continue;

                if (!gerenciador.ConfiguracoesPGi.GrelhaOpcoesVisualizacao.Diagramas_MostrarBarrasViga &&
                     gerenciador.ConfiguracoesPGi.GrelhaOpcoesVisualizacao.Diagramas_MostrarBarrasLaje && Pavimento.barras[i].barraViga) continue;

                if (!gerenciador.ConfiguracoesPGi.GrelhaOpcoesVisualizacao.Diagramas_MostrarBarrasViga &&
                     !gerenciador.ConfiguracoesPGi.GrelhaOpcoesVisualizacao.Diagramas_MostrarBarrasLaje) continue;*/

                    //pilar é tudo nos tons de esforço negativo, nao tem tom azul e por isso nao precisa de separação de cores que nem viga, 
                    //é tudo numa lista só- ver arquivo diagrama na pasta raciocinios
               MomentosNegativos[iFletoresNegativos++] = Math.Abs(bar.Esforcos[6]);
               MomentosNegativos[iFletoresNegativos++] = Math.Abs(bar.Esforcos[12]);
            }

            for (j = 0; j < iFletoresNegativos; j++)
            {
                for (k = j; k < iFletoresNegativos; k++)
                {
                    if (MomentosNegativos[k] > MomentosNegativos[j])
                    {
                        temp = MomentosNegativos[j];
                        MomentosNegativos[j] = MomentosNegativos[k];
                        MomentosNegativos[k] = temp;
                    };
                };
            };
        }
        void Preenche_e_Ordena_Esforcos_Axial_Pilares()
        {
            double temp;

            var barras = (from b in Portico.barras where b != null && b.barraPilar && !b.barraRigida select b);
            TBarraPortico bar;
            foreach (var b in barras)
            {
                bar = b as TBarraPortico;
                //   if (!Portico.barras[i].barraViga) continue;
                /* if (!gerenciador.ConfiguracoesPGi.GrelhaOpcoesVisualizacao.Diagramas_DirecaoX &&
                      Portico.barras[i].direcaoX) continue;

                 if (!gerenciador.ConfiguracoesPGi.GrelhaOpcoesVisualizacao.Diagramas_DirecaoY &&
                      Pavimento.barras[i].direcaoY) continue;*/

                /*if (gerenciador.ConfiguracoesPGi.GrelhaOpcoesVisualizacao.Diagramas_MostrarBarrasViga &&
                   !gerenciador.ConfiguracoesPGi.GrelhaOpcoesVisualizacao.Diagramas_MostrarBarrasLaje && !Pavimento.barras[i].barraViga) continue;

                if (!gerenciador.ConfiguracoesPGi.GrelhaOpcoesVisualizacao.Diagramas_MostrarBarrasViga &&
                     gerenciador.ConfiguracoesPGi.GrelhaOpcoesVisualizacao.Diagramas_MostrarBarrasLaje && Pavimento.barras[i].barraViga) continue;

                if (!gerenciador.ConfiguracoesPGi.GrelhaOpcoesVisualizacao.Diagramas_MostrarBarrasViga &&
                     !gerenciador.ConfiguracoesPGi.GrelhaOpcoesVisualizacao.Diagramas_MostrarBarrasLaje) continue;*/

                //pilar é tudo nos tons de esforço negativo, nao tem tom azul e por isso nao precisa de separação de cores que nem viga, 
                //é tudo numa lista só- ver arquivo diagrama na pasta raciocinios
                Axiais[iAxiais++] = Math.Abs(bar.Esforcos[1]);
                Axiais[iAxiais++] = Math.Abs(bar.Esforcos[7]);
            }

            for (j = 0; j < iAxiais; j++)
            {
                for (k = j; k < iAxiais; k++)
                {
                    if (Axiais[k] > Axiais[j])
                    {
                        temp = Axiais[j];
                        Axiais[j] = Axiais[k];
                        Axiais[k] = temp;
                    };
                };
            };
        }

        void Preenche_e_Ordena_Esforcos(string NomeEsforco, ref double[] EsforcoPositivo, ref double[] EsforcoNegativo, ref int iTotalNegativo, ref int iTotalPositivo)
        {
            double temp;

            iTotalNegativo = 0;
            iTotalPositivo = 0;

            vec3[] EsforcoCoords = new vec3[4];

            for (i = 1; i <= Portico.nBarras; i++)
            {
                if (Portico.barras[i].barraRigida) continue;
                //   if (!Portico.barras[i].barraViga) continue;
                /* if (!gerenciador.ConfiguracoesPGi.GrelhaOpcoesVisualizacao.Diagramas_DirecaoX &&
                      Portico.barras[i].direcaoX) continue;

                 if (!gerenciador.ConfiguracoesPGi.GrelhaOpcoesVisualizacao.Diagramas_DirecaoY &&
                      Pavimento.barras[i].direcaoY) continue;*/

                /*if (gerenciador.ConfiguracoesPGi.GrelhaOpcoesVisualizacao.Diagramas_MostrarBarrasViga &&
                   !gerenciador.ConfiguracoesPGi.GrelhaOpcoesVisualizacao.Diagramas_MostrarBarrasLaje && !Pavimento.barras[i].barraViga) continue;

                if (!gerenciador.ConfiguracoesPGi.GrelhaOpcoesVisualizacao.Diagramas_MostrarBarrasViga &&
                     gerenciador.ConfiguracoesPGi.GrelhaOpcoesVisualizacao.Diagramas_MostrarBarrasLaje && Pavimento.barras[i].barraViga) continue;

                if (!gerenciador.ConfiguracoesPGi.GrelhaOpcoesVisualizacao.Diagramas_MostrarBarrasViga &&
                     !gerenciador.ConfiguracoesPGi.GrelhaOpcoesVisualizacao.Diagramas_MostrarBarrasLaje) continue;*/

                if (NomeEsforco == "fletor z")
                {
                    if (Portico.barras[i].barraPilar)
                    {
                        EsforcoNegativo[iTotalNegativo++] = Math.Abs(Portico.barras[i].Esforcos[5]);
                        EsforcoNegativo[iTotalNegativo++] = Math.Abs(Portico.barras[i].Esforcos[11]);
                    }
                }
                else
                if (NomeEsforco == "fletor y")
                {
                    if (Portico.barras[i].barraPilar)
                    {
                        //pilar é tudo nos tons de esforço negativo, nao tem tom azul e por isso nao precisa de separação de cores que nem viga, 
                        //é tudo numa lista só- ver arquivo diagrama na pasta raciocinios
                        EsforcoNegativo[iTotalNegativo++] = Math.Abs(Portico.barras[i].Esforcos[6]);
                        EsforcoNegativo[iTotalNegativo++] = Math.Abs(Portico.barras[i].Esforcos[12]);
                    }
                    else
                    if (Portico.barras[i].barraViga)
                    {
                        if (NomeEsforco == "fletor y")
                        {
                            if (Portico.barras[i].fletorY_inicial_viga > Portico.barras[i].pIni.z)
                                EsforcoNegativo[iTotalNegativo++] = Math.Abs(Portico.barras[i].Esforcos[6]);
                            else
                                EsforcoPositivo[iTotalPositivo++] = Math.Abs(Portico.barras[i].Esforcos[6]);

                            if (Portico.barras[i].fletorY_final_viga < Portico.barras[i].pIni.z)
                                EsforcoPositivo[iTotalPositivo++] = Math.Abs(Portico.barras[i].Esforcos[12]);
                            else
                                EsforcoNegativo[iTotalNegativo++] = Math.Abs(Portico.barras[i].Esforcos[12]);

                            /* if (EsforcoCoords[1].z < EsforcoCoords[0].z)
                                 EsforcoPositivo[iTotalPositivo++] = Math.Abs(EsforcoCoords[1].z);
                             else
                   

                             if (EsforcoCoords[2].z < EsforcoCoords[0].z)
                                 EsforcoPositivo[iTotalPositivo++] = Math.Abs(EsforcoCoords[2].z);
                             else
                                 EsforcoNegativo[iTotalNegativo++] = EsforcoCoords[2].z;*/
                        }
                    }
                }

                   /* else
                    {

                        //ESF CORTANTE E TORÇOR É AO CONTRÁRIO DO FLETOR : POSITIVO EM CIMA E NEGATIVO EMBAIXO
                        if (NomeEsforco == "MOMENTO TORCOR")
                            EsforcoCoords = Portico.barras[i].CoordsTorcor;
                        else
                            if (NomeEsforco == "ESFORCO CORTANTE")
                                EsforcoCoords = Portico.barras[i].CoordsCortante;

                        if (EsforcoCoords[1].z > EsforcoCoords[0].z)
                            EsforcoPositivo[iTotalPositivo++] = EsforcoCoords[1].z;
                        else
                            EsforcoNegativo[iTotalNegativo++] = Math.Abs(EsforcoCoords[1].z);

                        if (EsforcoCoords[2].z > EsforcoCoords[0].z)
                            EsforcoPositivo[iTotalPositivo++] = EsforcoCoords[2].z;
                        else
                            EsforcoNegativo[iTotalNegativo++] = Math.Abs(EsforcoCoords[2].z);
                    }*/
                
            }

            for (j = 0; j < iTotalPositivo; j++)
            {
                for (k = j; k < iTotalPositivo; k++)
                {
                    if (EsforcoPositivo[k] > EsforcoPositivo[j])
                    {
                        temp = EsforcoPositivo[j];
                        EsforcoPositivo[j] = EsforcoPositivo[k];
                        EsforcoPositivo[k] = temp;
                    };
                };
            };

            for (j = 0; j < iTotalNegativo; j++)
            {
                for (k = j; k < iTotalNegativo; k++)
                {
                    if (EsforcoNegativo[k] > EsforcoNegativo[j])
                    {
                        temp = EsforcoNegativo[j];
                        EsforcoNegativo[j] = EsforcoNegativo[k];
                        EsforcoNegativo[k] = temp;
                    };
                };
            };
        }


        private void OrdenaDeslocamentos()
        {
            double temp;
            MaxCargaPontual = 0;
            iDeslocamentos = 0;
            for (i = 1; i <= Portico.nNos; i++)
            {
                if (Math.Abs(Portico.nos[i].Carga[2]) > Math.Abs(MaxCargaPontual))
                   MaxCargaPontual = Math.Abs(Portico.nos[i].Carga[2]);

                Deslocamentos[iDeslocamentos++] = Portico.nos[i].Deslocamento[2];
            }

            for (j = 0; j < iDeslocamentos; j++)
            {
                for (k = j; k < iDeslocamentos; k++)
                {
                    if (Deslocamentos[k] > Deslocamentos[j])
                    {
                        temp = Deslocamentos[j];
                        Deslocamentos[j] = Deslocamentos[k];
                        Deslocamentos[k] = temp;
                    };
                };
            };
        }
        void CriaVetores()
        {
            MomentosNegativos = new double[Portico.Ngl * 2];
            Axiais            = new double[Portico.Ngl * 2];
            MomentosPositivosY = new double[Portico.Ngl * 2];
            MomentosTorcoresPositivos = new double[Portico.Ngl * 2];
            MomentosTorcoresNegativos = new double[Portico.Ngl * 2];
            CortantesPositivosY = new double[Portico.Ngl * 2];
            CortantesNegativosY = new double[Portico.Ngl * 2];
            Deslocamentos = new double[Portico.Ngl/3];

            ListaRGB_EsforcosNegativos = new double[6, 3];  //6 cores diferentes - 3 são os tons r g b
            ListaRGB_EsforcosPositivos = new double[4, 3];  //4 cores diferentes - 3 são os tons r g b
           // ListaRGB_Axiais = new double[6, 3];  //6 cores diferentes - 3 são os tons r g b

            #region Cores - Positivos
            ListaRGB_EsforcosPositivos[0, 0] = pnd10.BackColor.R;  //RGB's    azul
            ListaRGB_EsforcosPositivos[0, 1] = pnd10.BackColor.G;
            ListaRGB_EsforcosPositivos[0, 2] = pnd10.BackColor.B;

            ListaRGB_EsforcosPositivos[1, 0] = pnd9.BackColor.R;  //RGB's    azul claro
            ListaRGB_EsforcosPositivos[1, 1] = pnd9.BackColor.G;
            ListaRGB_EsforcosPositivos[1, 2] = pnd9.BackColor.B;

            ListaRGB_EsforcosPositivos[2, 0] = pnd8.BackColor.R;  //RGB's   azul mais claro
            ListaRGB_EsforcosPositivos[2, 1] = pnd8.BackColor.G;
            ListaRGB_EsforcosPositivos[2, 2] = pnd8.BackColor.B;

            ListaRGB_EsforcosPositivos[3, 0] = pnd7.BackColor.R;  //RGB's     branco
            ListaRGB_EsforcosPositivos[3, 1] = pnd7.BackColor.G;
            ListaRGB_EsforcosPositivos[3, 2] = pnd7.BackColor.B;
            #endregion

            #region Cores - Negativos
            ListaRGB_EsforcosNegativos[0, 0] = pnd1.BackColor.R;  //RGB's    vermelho
            ListaRGB_EsforcosNegativos[0, 1] = pnd1.BackColor.G;
            ListaRGB_EsforcosNegativos[0, 2] = pnd1.BackColor.B;

            ListaRGB_EsforcosNegativos[1, 0] = pnd2.BackColor.R;  //RGB's    laranja
            ListaRGB_EsforcosNegativos[1, 1] = pnd2.BackColor.G;
            ListaRGB_EsforcosNegativos[1, 2] = pnd2.BackColor.B;

            ListaRGB_EsforcosNegativos[2, 0] = pnd3.BackColor.R;  //RGB's    laranja claro
            ListaRGB_EsforcosNegativos[2, 1] = pnd3.BackColor.G;
            ListaRGB_EsforcosNegativos[2, 2] = pnd3.BackColor.B;

            ListaRGB_EsforcosNegativos[3, 0] = pnd4.BackColor.R; //RGB's   amarelo
            ListaRGB_EsforcosNegativos[3, 1] = pnd4.BackColor.G;
            ListaRGB_EsforcosNegativos[3, 2] = pnd4.BackColor.B;

            ListaRGB_EsforcosNegativos[4, 0] = pnd5.BackColor.R; //RGB's   amarelo claro
            ListaRGB_EsforcosNegativos[4, 1] = pnd5.BackColor.G;
            ListaRGB_EsforcosNegativos[4, 2] = pnd5.BackColor.B;

            ListaRGB_EsforcosNegativos[5, 0] = pnd6.BackColor.R;  //RGB's     branco
            ListaRGB_EsforcosNegativos[5, 1] = pnd6.BackColor.G;
            ListaRGB_EsforcosNegativos[5, 2] = pnd6.BackColor.B;
            #endregion

        }

        double[] px_x_i2 = new double[1];
        double[] px_y_i2 = new double[1];
        
        double[] px_x_i = new double[1];
        double[] px_y_i = new double[1];
        double[] px_x_f = new double[1];
        double[] px_y_f = new double[1];

        TBarraPortico BarraTemp;
        string ss;
        private void GetInfoBarra()
        {
            lbInfoBarra.Text = "";
            BarraTemp = null;
            for (i = 1; i <= Portico.nBarras; i++)
            {
                if (Math.Abs(Portico.barras[i].CargaDistribuida) > Math.Abs(MaxCargaDistribuida))
                   MaxCargaDistribuida = Math.Abs(Portico.barras[i].CargaDistribuida);
             // if (Portico.barras[i].barraViga)
                {
                    Project(ref px_x_i, ref px_y_i, Portico.barras[i].pIni.x - centroX, Portico.barras[i].pIni.y - centroY, Portico.barras[i].pIni.z,ModelViewMatrix,ProjectionMatrix);
                    Project(ref px_x_f, ref px_y_f, Portico.barras[i].pFin.x - centroX, Portico.barras[i].pFin.y - centroY, Portico.barras[i].pFin.z,ModelViewMatrix,ProjectionMatrix);
                    Portico.barras[i].PreSelecionada = false;
                    if (Geom.PontoEmLinha2(mouseX, mouseY, px_x_i[0], px_y_i[0], px_x_f[0], px_y_f[0], 2))
                    {
                        lbInfoBarra.Visible = true;
                        lbInfoBarra.Left = (int)mouseX + 10;
                        lbInfoBarra.Top     = (int)mouseY -25;
                         
                        if (Portico.barras[i].barraRigida)
                            lbInfoBarra.Text = "Barra rígida";
                        else
                            if (Portico.barras[i].barraViga && Portico.barras[i].TrechoViga != null)
                            lbInfoBarra.Text = "Viga " + Portico.barras[i].TrechoViga.Dados.nome + Portico.barras[i].TrechoViga.Dados.numero+
                                            " - Barra " + Portico.barras[i].IDBarra.ToString();
                        else
                        if (Portico.barras[i].barraPilar && Portico.barras[i].Pilar != null)
                        {
                            ss = " - "+ Portico.barras[i].Pilar.Dados.b1.ToString()+"/"
                                       +  Portico.barras[i].Pilar.Dados.h1.ToString();
                           
                            if (Portico.barras[i].Pilar.Dados.Poligono.TipoSecao != "Retangular" && Portico.barras[i].Pilar.Dados.Poligono.TipoSecao != "Circular")
                               ss += "/" + Portico.barras[i].Pilar.Dados.b2.ToString() + "/"
                                         + Portico.barras[i].Pilar.Dados.h2.ToString();

                            lbInfoBarra.Text = "Pilar " + Portico.barras[i].Pilar.Dados.nome + Portico.barras[i].Pilar.Dados.numero +
                                             " - " + Portico.barras[i].Pilar.Dados.Poligono.TipoSecao.ToString() + ss;
                        }

                        if (SelecionandoBarra)
                        {
                            Portico.barras[i].PreSelecionada = true;
                        }

                        BarraTemp = Portico.barras[i];
                        lbInfoBarra.Refresh();
                    }
                }
            }
        }
        private void GetFletorZ()
        {
            for (i = 1; i <= Portico.nBarras; i++)
            {
                if (Portico.barras[i].Poligono1_FletorZ_Pilar != null)
                    for (j = 0; j < Portico.barras[i].Poligono1_FletorZ_Pilar.Count; j++)
                    {
                        if ((mouseX >= (Portico.barras[i].Poligono1_FletorZ_Pilar[j].px_x - 12)) && (mouseX <= (Portico.barras[i].Poligono1_FletorZ_Pilar[j].px_x + 12)))
                        {
                            if ((mouseY >= (Portico.barras[i].Poligono1_FletorZ_Pilar[j].px_y - 12)) && (mouseY <= (Portico.barras[i].Poligono1_FletorZ_Pilar[j].px_y + 12)))
                            {
                                if (Portico.barras[i].Poligono1_FletorZ_Pilar[j].vv != 0)
                                {
                                    lbInfoNo.Visible = true;
                                    lbInfoNo.Left = mouseX + 10;
                                    lbInfoNo.Top = mouseY - 10;
                                    lbInfoNo.Text = Math.Abs(Portico.barras[i].Poligono1_FletorZ_Pilar[j].vv * 0.1).ToString("n3") + " tf.m";
                                    lbInfoNo.Refresh();
                                }
                            }
                        }
                    }

                if (Portico.barras[i].Poligono2_FletorZ_Pilar != null)
                    for (j = 0; j < Portico.barras[i].Poligono2_FletorZ_Pilar.Count; j++)
                    {

                        if ((mouseX >= (Portico.barras[i].Poligono2_FletorZ_Pilar[j].px_x - 12)) && (mouseX <= (Portico.barras[i].Poligono2_FletorZ_Pilar[j].px_x + 12)))
                        {
                            if ((mouseY >= (Portico.barras[i].Poligono2_FletorZ_Pilar[j].px_y - 12)) && (mouseY <= (Portico.barras[i].Poligono2_FletorZ_Pilar[j].px_y + 12)))
                            {
                                if (Portico.barras[i].Poligono2_FletorZ_Pilar[j].vv != 0)
                                {
                                    lbInfoNo.Visible = true;
                                    lbInfoNo.Left = mouseX + 10;
                                    lbInfoNo.Top = mouseY - 10;
                                    lbInfoNo.Text = Math.Abs(Portico.barras[i].Poligono2_FletorZ_Pilar[j].vv * 0.1).ToString("n3") + " tf.m";
                                    lbInfoNo.Refresh();
                                }
                            }
                        }
                    }
            }
        }
        private void GetCortanteY()
        {
            lbInfoNo.Visible = false;

            for (i = 1; i <= Portico.nBarras; i++)
            {
                //   if (Portico.barras[i].barraRigida) continue;
                //  if (Portico.barras[i].barraPilar) continue;
                /*if (!gerenciador.ConfiguracoesPGi.GrelhaOpcoesVisualizacao.Diagramas_DirecaoX &&
                     Pavimento.barras[i].direcaoX) continue;

                if (!gerenciador.ConfiguracoesPGi.GrelhaOpcoesVisualizacao.Diagramas_DirecaoY &&
                     Pavimento.barras[i].direcaoY) continue;*/

                //if (!gerenciador.ConfiguracoesPGi.GrelhaOpcoesVisualizacao.Diagramas_MostrarBarrasViga && Portico.barras[i].barraViga) continue;
                // if (!gerenciador.ConfiguracoesPGi.GrelhaOpcoesVisualizacao.Diagramas_MostrarBarrasViga && Portico.barras[i].barraViga) continue;


                // if (!gerenciador.ConfiguracoesPGi.GrelhaOpcoesVisualizacao.Diagramas_MostrarBarrasViga &&
                //       !gerenciador.ConfiguracoesPGi.GrelhaOpcoesVisualizacao.Diagramas_MostrarBarrasLaje) continue;

                if (Portico.barras[i].Poligono1_CortanteY_Viga != null)
                    for (j = 0; j < Portico.barras[i].Poligono1_CortanteY_Viga.Count; j++)
                    {

                        if ((mouseX >= (Portico.barras[i].Poligono1_CortanteY_Viga[j].px_x - 12)) && (mouseX <= (Portico.barras[i].Poligono1_CortanteY_Viga[j].px_x + 12)))
                        {
                            if ((mouseY >= (Portico.barras[i].Poligono1_CortanteY_Viga[j].px_y - 12)) && (mouseY <= (Portico.barras[i].Poligono1_CortanteY_Viga[j].px_y + 12)))
                            {
                                if (Portico.barras[i].Poligono1_CortanteY_Viga[j].vv != 0)
                                {
                                    lbInfoNo.Visible = true;
                                    lbInfoNo.Left = mouseX + 10;
                                    lbInfoNo.Top = mouseY - 10;
                                    lbInfoNo.Text = Math.Abs(Portico.barras[i].Poligono1_CortanteY_Viga[j].vv * 0.1).ToString("n3") + " tf";
                                    lbInfoNo.Refresh();
                                }
                            }
                        }
                    }


                if (Portico.barras[i].Poligono1_CortanteY_Pilar != null)
                    for (j = 0; j < Portico.barras[i].Poligono1_CortanteY_Pilar.Count; j++)
                    {

                        if ((mouseX >= (Portico.barras[i].Poligono1_CortanteY_Pilar[j].px_x - 12)) && (mouseX <= (Portico.barras[i].Poligono1_CortanteY_Pilar[j].px_x + 12)))
                        {
                            if ((mouseY >= (Portico.barras[i].Poligono1_CortanteY_Pilar[j].px_y - 12)) && (mouseY <= (Portico.barras[i].Poligono1_CortanteY_Pilar[j].px_y + 12)))
                            {
                                if (Portico.barras[i].Poligono1_CortanteY_Pilar[j].vv != 0)
                                {
                                    lbInfoNo.Visible = true;
                                    lbInfoNo.Left = mouseX + 10;
                                    lbInfoNo.Top = mouseY - 10;
                                    lbInfoNo.Text = Math.Abs(Portico.barras[i].Poligono1_CortanteY_Pilar[j].vv * 0.1).ToString("n3") + " tf";
                                    lbInfoNo.Refresh();
                                }
                            }
                        }
                    }

            }
        }
        private void GetFletorY()
        {
            lbInfoNo.Visible = false;

            for (i = 1; i <= Portico.nBarras; i++)
            {
             //   if (Portico.barras[i].barraRigida) continue;
              //  if (Portico.barras[i].barraPilar) continue;
                /*if (!gerenciador.ConfiguracoesPGi.GrelhaOpcoesVisualizacao.Diagramas_DirecaoX &&
                     Pavimento.barras[i].direcaoX) continue;

                if (!gerenciador.ConfiguracoesPGi.GrelhaOpcoesVisualizacao.Diagramas_DirecaoY &&
                     Pavimento.barras[i].direcaoY) continue;*/

                //if (!gerenciador.ConfiguracoesPGi.GrelhaOpcoesVisualizacao.Diagramas_MostrarBarrasViga && Portico.barras[i].barraViga) continue;
               // if (!gerenciador.ConfiguracoesPGi.GrelhaOpcoesVisualizacao.Diagramas_MostrarBarrasViga && Portico.barras[i].barraViga) continue;

          
               // if (!gerenciador.ConfiguracoesPGi.GrelhaOpcoesVisualizacao.Diagramas_MostrarBarrasViga &&
              //       !gerenciador.ConfiguracoesPGi.GrelhaOpcoesVisualizacao.Diagramas_MostrarBarrasLaje) continue;

                if (Portico.barras[i].Poligono1_FletorY_Viga != null)
                    for (j = 0; j < Portico.barras[i].Poligono1_FletorY_Viga.Count; j++)
                {

                    if ((mouseX >= (Portico.barras[i].Poligono1_FletorY_Viga[j].px_x - 12)) && (mouseX <= (Portico.barras[i].Poligono1_FletorY_Viga[j].px_x + 12)))
                    {
                        if ((mouseY >= (Portico.barras[i].Poligono1_FletorY_Viga[j].px_y - 12)) && (mouseY <= (Portico.barras[i].Poligono1_FletorY_Viga[j].px_y + 12)))
                        {
                            if (Portico.barras[i].Poligono1_FletorY_Viga[j].vv != 0)
                            {
                                lbInfoNo.Visible = true;
                                lbInfoNo.Left = mouseX + 10;
                                lbInfoNo.Top = mouseY - 10;
                                lbInfoNo.Text = Math.Abs(Portico.barras[i].Poligono1_FletorY_Viga[j].vv /** 0.1*/).ToString("n3") + " kN.m";//tf.m
                                lbInfoNo.Refresh();
                            }
                        }
                    }
                }

                if (Portico.barras[i].Poligono2_FletorY_Viga != null)
                    for (j = 0; j < Portico.barras[i].Poligono2_FletorY_Viga.Count; j++)
                {

                    if ((mouseX >= (Portico.barras[i].Poligono2_FletorY_Viga[j].px_x - 12)) && (mouseX <= (Portico.barras[i].Poligono2_FletorY_Viga[j].px_x + 12)))
                    {
                        if ((mouseY >= (Portico.barras[i].Poligono2_FletorY_Viga[j].px_y - 12)) && (mouseY <= (Portico.barras[i].Poligono2_FletorY_Viga[j].px_y + 12)))
                        {
                            if (Portico.barras[i].Poligono2_FletorY_Viga[j].vv != 0)
                            {
                                lbInfoNo.Visible = true;
                                lbInfoNo.Left = mouseX + 10;
                                lbInfoNo.Top = mouseY - 10;
                                lbInfoNo.Text = Math.Abs(Portico.barras[i].Poligono2_FletorY_Viga[j].vv ).ToString("n3") + " kN.m";
                                lbInfoNo.Refresh();
                            }
                        }
                    }
                }



                if (Portico.barras[i].Poligono1_FletorY_Pilar != null)
                    for (j = 0; j < Portico.barras[i].Poligono1_FletorY_Pilar.Count; j++)
                    {

                        if ((mouseX >= (Portico.barras[i].Poligono1_FletorY_Pilar[j].px_x - 12)) && (mouseX <= (Portico.barras[i].Poligono1_FletorY_Pilar[j].px_x + 12)))
                        {
                            if ((mouseY >= (Portico.barras[i].Poligono1_FletorY_Pilar[j].px_y - 12)) && (mouseY <= (Portico.barras[i].Poligono1_FletorY_Pilar[j].px_y + 12)))
                            {
                                if (Portico.barras[i].Poligono1_FletorY_Pilar[j].vv != 0)
                                {
                                    lbInfoNo.Visible = true;
                                    lbInfoNo.Left = mouseX + 10;
                                    lbInfoNo.Top = mouseY - 10;
                                    lbInfoNo.Text = Math.Abs(Portico.barras[i].Poligono1_FletorY_Pilar[j].vv).ToString("n3") + " kN.m";
                                    lbInfoNo.Refresh();
                                }
                            }
                        }
                    }

                if (Portico.barras[i].Poligono2_FletorY_Pilar != null)
                    for (j = 0; j < Portico.barras[i].Poligono2_FletorY_Pilar.Count; j++)
                    {

                        if ((mouseX >= (Portico.barras[i].Poligono2_FletorY_Pilar[j].px_x - 12)) && (mouseX <= (Portico.barras[i].Poligono2_FletorY_Pilar[j].px_x + 12)))
                        {
                            if ((mouseY >= (Portico.barras[i].Poligono2_FletorY_Pilar[j].px_y - 12)) && (mouseY <= (Portico.barras[i].Poligono2_FletorY_Pilar[j].px_y + 12)))
                            {
                                if (Portico.barras[i].Poligono2_FletorY_Pilar[j].vv != 0)
                                {
                                    lbInfoNo.Visible = true;
                                    lbInfoNo.Left = mouseX + 10;
                                    lbInfoNo.Top = mouseY - 10;
                                    lbInfoNo.Text = Math.Abs(Portico.barras[i].Poligono2_FletorY_Pilar[j].vv).ToString("n3") + " kN.m";
                                    lbInfoNo.Refresh();
                                }
                            }
                        }
                    }
    
            }
        }
        private void GetTorcor()
        {
            lbInfoNo.Visible = false;

            for (i = 1; i <= Portico.nBarras; i++)
            {
                //   if (Portico.barras[i].barraRigida) continue;
                //  if (Portico.barras[i].barraPilar) continue;
                /*if (!gerenciador.ConfiguracoesPGi.GrelhaOpcoesVisualizacao.Diagramas_DirecaoX &&
                     Pavimento.barras[i].direcaoX) continue;

                if (!gerenciador.ConfiguracoesPGi.GrelhaOpcoesVisualizacao.Diagramas_DirecaoY &&
                     Pavimento.barras[i].direcaoY) continue;*/

                //if (!gerenciador.ConfiguracoesPGi.GrelhaOpcoesVisualizacao.Diagramas_MostrarBarrasViga && Portico.barras[i].barraViga) continue;
                // if (!gerenciador.ConfiguracoesPGi.GrelhaOpcoesVisualizacao.Diagramas_MostrarBarrasViga && Portico.barras[i].barraViga) continue;


                // if (!gerenciador.ConfiguracoesPGi.GrelhaOpcoesVisualizacao.Diagramas_MostrarBarrasViga &&
                //       !gerenciador.ConfiguracoesPGi.GrelhaOpcoesVisualizacao.Diagramas_MostrarBarrasLaje) continue;
                if (Portico.barras[i].Poligono1_Torcor_Viga != null)
                    for (j = 0; j < Portico.barras[i].Poligono1_Torcor_Viga.Count; j++)
                    {
                        if ((mouseX >= (Portico.barras[i].Poligono1_Torcor_Viga[j].px_x - 12)) && (mouseX <= (Portico.barras[i].Poligono1_Torcor_Viga[j].px_x + 12)))
                        {
                            if ((mouseY >= (Portico.barras[i].Poligono1_Torcor_Viga[j].px_y - 12)) && (mouseY <= (Portico.barras[i].Poligono1_Torcor_Viga[j].px_y + 12)))
                            {
                                if (Portico.barras[i].Poligono1_Torcor_Viga[j].vv != 0)
                                {
                                    lbInfoNo.Visible = true;
                                    lbInfoNo.Left = mouseX + 10;
                                    lbInfoNo.Top = mouseY - 10;
                                    lbInfoNo.Text = Math.Abs(Portico.barras[i].Poligono1_Torcor_Viga[j].vv * 0.1).ToString("n3") + " tf.m";
                                    lbInfoNo.Refresh();
                                }
                            }
                        }
                    }
            }
        } 
        private void GetAxial()
        {
            lbInfoNo.Visible = false;

            for (i = 1; i <= Portico.nBarras; i++)
            {
                //   if (Portico.barras[i].barraRigida) continue;
                //  if (Portico.barras[i].barraPilar) continue;
                /*if (!gerenciador.ConfiguracoesPGi.GrelhaOpcoesVisualizacao.Diagramas_DirecaoX &&
                     Pavimento.barras[i].direcaoX) continue;

                if (!gerenciador.ConfiguracoesPGi.GrelhaOpcoesVisualizacao.Diagramas_DirecaoY &&
                     Pavimento.barras[i].direcaoY) continue;*/

                //if (!gerenciador.ConfiguracoesPGi.GrelhaOpcoesVisualizacao.Diagramas_MostrarBarrasViga && Portico.barras[i].barraViga) continue;
                // if (!gerenciador.ConfiguracoesPGi.GrelhaOpcoesVisualizacao.Diagramas_MostrarBarrasViga && Portico.barras[i].barraViga) continue;


                // if (!gerenciador.ConfiguracoesPGi.GrelhaOpcoesVisualizacao.Diagramas_MostrarBarrasViga &&
                //       !gerenciador.ConfiguracoesPGi.GrelhaOpcoesVisualizacao.Diagramas_MostrarBarrasLaje) continue;
                if (Portico.barras[i].Poligono1_Axial_Pilar != null)
                    for (j = 0; j < Portico.barras[i].Poligono1_Axial_Pilar.Count; j++)
                    {
                        if ((mouseX >= (Portico.barras[i].Poligono1_Axial_Pilar[j].px_x - 12)) && (mouseX <= (Portico.barras[i].Poligono1_Axial_Pilar[j].px_x + 12)))
                        {
                            if ((mouseY >= (Portico.barras[i].Poligono1_Axial_Pilar[j].px_y - 12)) && (mouseY <= (Portico.barras[i].Poligono1_Axial_Pilar[j].px_y + 12)))
                            {
                                if (Portico.barras[i].Poligono1_Axial_Pilar[j].vv != 0)
                                {
                                    lbInfoNo.Visible = true;
                                    lbInfoNo.Left = mouseX + 10;
                                    lbInfoNo.Top = mouseY - 10;
                                    lbInfoNo.Text = Math.Abs(Portico.barras[i].Poligono1_Axial_Pilar[j].vv * 0.1).ToString("n3") + " tf";
                                    lbInfoNo.Refresh();
                                }
                            }
                        }
                    }
            }
        }
        private void GetDeslocamentoNo(int Deslocamento)
        {
            lbInfoNo.Visible = false;
            for (i = 1; i <= Portico.nNos; i++)
            {
                if ((mouseX >= (Portico.nos[i].px_x - 8)) && (mouseX <= (Portico.nos[i].px_x + 8)))
                {
                    if ((mouseY >= (Portico.nos[i].px_y - 8)) && (mouseY <= (Portico.nos[i].px_y + 8)))
                    {
                        lbInfoNo.Visible = true;
                        lbInfoNo.Left = mouseX + 10;
                        lbInfoNo.Top = mouseY - 10;
                        lbInfoNo.Text = "Nó " + i.ToString() + "\r dx: " + ((Portico.nos[i].Deslocamento[1] * 1000)).ToString("n3") +
                                        "\r dy: " + ((Portico.nos[i].Deslocamento[3] * 1000)).ToString("n3") +
                                        "\r dz: " + ((Portico.nos[i].Deslocamento[2] * 1000)).ToString("n3") + " (mm)";
                                      /*  "\r rx: " + ((Portico.nos[i].Deslocamento[4])).ToString("e") +
                                        " ry: " + ((Portico.nos[i].Deslocamento[6])).ToString("e") +
                                        " rz: " + ((Portico.nos[i].Deslocamento[5])).ToString("e") + " (rad)";*/
                        lbInfoNo.Refresh();
                    }
                }
            }
        }
       
        private void Controle_Resize(object sender, EventArgs e)
        {
          //  if (gerenciador.abrindoPortico)
           //   Controle.InitializeContexts();
            //   GerarModelo3();
            //  Enquadrar();

           // SetupCamera();
        }

        private void FVisualizadorPortico_Activated(object sender, EventArgs e)
        {
        
         //        Controle.InitializeContexts();
         //        GerarModelo3();
        }

        private void FVisualizadorPortico_Shown(object sender, EventArgs e)
        {

           // TOpenGl.ReShapeOGL(Controle.Width, Controle.Height, angx, angy, angz);
        
            //   GerarModelo3();
        }
        private void FVisualizadorPortico_DockChanged(object sender, EventArgs e)
        {
   
        }

        private void FVisualizadorPortico_Validated(object sender, EventArgs e)
        {
            
          

        }

        private void Controle_Layout(object sender, LayoutEventArgs e)
        {
   
        }

        private void FVisualizadorPortico_SizeChanged(object sender, EventArgs e)
        {
          
        }

        private void Controle_Validated(object sender, EventArgs e)
        {
           
        }
        private void Controle_Paint(object sender, PaintEventArgs e)
        {
            Controle.MakeCurrent();
       //     Enquadrar();

        }

        private void FVisualizadorPortico_Paint(object sender, PaintEventArgs e)
        {
           
        }

        private static float[] LightAmb = { 0.4f, 0.4f, 0.4f, 1 };                // Ambient Light
        private static float[] LightDif = { 1, 1, 1, 1 };                         // Diffuse Light
        private static float[] LightPos = { 4, 4, 6, 1 };
        float[] position = { 20, 20, 50 };
        float[] ambient = { 1f, 1, 1, 1 };
        float[] diffuse = { 1.0f, 1.0f, 1.0f, 1.0f };
        float[] specular = { 1.0f, 1.0f, 1.0f, 1.0f };
        
        public void InicializarOGL()
        {
            GL.Enable(EnableCap.DepthTest);
            GL.Enable(EnableCap.Lighting);
            GL.Enable(EnableCap.Blend);
            GL.Enable(EnableCap.Light0);
            GL.Enable(EnableCap.ColorMaterial);
            GL.Enable(EnableCap.PolygonOffsetFill);
            GL.Disable(EnableCap.CullFace);
            GL.Enable(EnableCap.Normalize);
            GL.Enable(EnableCap.AutoNormal);
            GL.Enable(EnableCap.Multisample);
            GL.Enable(EnableCap.PolygonSmooth);

            GL.Light(LightName.Light0, LightParameter.Position, new float[] { 0 - (float)centroY, 140000, 0 - (float)centroX, 1 });
            GL.Light(LightName.Light0, LightParameter.Ambient, ambient);
            GL.Light(LightName.Light0, LightParameter.Diffuse, diffuse);
            GL.Light(LightName.Light0, LightParameter.Specular, specular);
            GL.LightModel(LightModelParameter.LightModelAmbient, new float[] { 0.2f, 0.2f, 0.2f, 1.0f });
            GL.LightModel(LightModelParameter.LightModelTwoSide, 1);
            GL.LightModel(LightModelParameter.LightModelLocalViewer, 1);

            GL.ShadeModel(ShadingModel.Smooth);

            GL.BlendFunc(BlendingFactor.SrcAlpha, BlendingFactor.OneMinusSrcAlpha);
            GL.ColorMaterial(MaterialFace.FrontAndBack, ColorMaterialParameter.AmbientAndDiffuse);

            GL.ClearDepth(1.0);
            GL.DepthFunc(DepthFunction.Lequal);
            GL.FrontFace(FrontFaceDirection.Cw);
            GL.CullFace(CullFaceMode.Back);

            GL.PolygonMode(MaterialFace.FrontAndBack, PolygonMode.Fill);
            GL.Hint(HintTarget.PolygonSmoothHint, HintMode.Nicest);
        }
 
        public void InicializarOGL2()
        {
            GL.Hint(HintTarget.PerspectiveCorrectionHint, HintMode.Nicest);
            //glHint(GL_LINE_SMOOTH_HINT, GL_NICEST);
            //     GL.Hint(GL_POLYGON_SMOOTH_HINT, GL_NICEST);
            GL.Enable(EnableCap.DepthTest);
            //   GL.Enable(Gl.GL_LIGHTING);
            //   GL.Enable(Gl.GL_TEXTURE_2D);
//                GL.Enable(Gl.GL_CULL_FACE);

            // track material ambient and diffuse from surface color, call it before glEnable(GL_COLOR_MATERIAL)
            GL.ColorMaterial(MaterialFace.FrontAndBack, ColorMaterialParameter.AmbientAndDiffuse);
            GL.Enable(EnableCap.ColorMaterial);

            GL.ClearColor(1,1,1,1);                   // background color
            GL.ClearStencil(0);                          // clear stencil buffer
            GL.ClearDepth(1.0f);                         // 0 is near, 1 is far
            GL.DepthFunc(DepthFunction.Lequal);

          /*  GL.Light(LightName.Light0, LightParameter.Position, new float[] { 0 - (float)centroY, 140000, 0 - (float)centroX, 1 });
            GL.Light(LightName.Light0, LightParameter.Ambient, ambient);
            GL.Light(LightName.Light0, LightParameter.Diffuse, diffuse);
            GL.Light(LightName.Light0, LightParameter.Specular, specular);
            GL.LightModel(LightModelParameter.LightModelAmbient, new float[] { 0.2f, 0.2f, 0.2f, 1.0f });
            GL.LightModel(LightModelParameter.LightModelTwoSide, 1);
            GL.LightModel(LightModelParameter.LightModelLocalViewer, 1);*/

            if (gerenciador.ConfiguracoesPGi.PorticoOpcoesVisualizacao.Suavizar)
            {
               /* GL.Enable(Gl.GL_LINE_SMOOTH);
                GL.Enable(Gl.GL_BLEND);
                GL.BlendFunc(Gl.GL_SRC_ALPHA, Gl.GL_ONE_MINUS_SRC_ALPHA);
                GL.Hint(Gl.GL_LINE_SMOOTH_HINT, Gl.GL_DONT_CARE);*/
            }
            else
            {
              /*  GL.Disable(Gl.GL_LINE_SMOOTH);
                GL.Disable(Gl.GL_BLEND);
                GL.BlendFunc(Gl.GL_SRC_ALPHA, Gl.GL_ONE_MINUS_SRC_ALPHA);
                GL.Hint(Gl.GL_LINE_SMOOTH_HINT, Gl.GL_DONT_CARE);*/
            }
        }
        private void FVisualizadorPortico_Load(object sender, EventArgs e)
        {
            try
            {
                InicializarOGL();
            }
            catch(Exception)
            {

            }
/*
            if (gerenciador.ConfiguracoesPGi.GrelhaOpcoesVisualizacao.Suavizar)
            {
                GL.Enable(Gl.GL_LINE_SMOOTH);
                GL.Enable(Gl.GL_BLEND);
                GL.BlendFunc(Gl.GL_SRC_ALPHA, Gl.GL_ONE_MINUS_SRC_ALPHA);
                GL.Hint(Gl.GL_LINE_SMOOTH_HINT, Gl.GL_DONT_CARE);
            }
            else
            {
                GL.Disable(Gl.GL_LINE_SMOOTH);
                GL.Disable(Gl.GL_BLEND);
                GL.BlendFunc(Gl.GL_SRC_ALPHA, Gl.GL_ONE_MINUS_SRC_ALPHA);
                GL.Hint(Gl.GL_LINE_SMOOTH_HINT, Gl.GL_DONT_CARE);
            }*/
          //  Controle.InitializeContexts();
      //      GerarModelo3();
        }
        private void Controle_MouseClick(object sender, System.Windows.Forms.MouseEventArgs e)
        {
            start_x = e.X;
            start_y = e.Y;

            if (NovaBarra || NovoNo)
                if (CapturouPonto1)
                {
                  if (!clic1)
                  {
                    clic1 = true;
                    clic2 = false;
                  }
                  else
                  {
                    clic1 = false;
                    clic2 = true;
                  }
                }
        }

        double[] px_x = new double[1];
        double[] px_y = new double[1];
        TNoPortico FindPt(double x, double y, double z)
        {
            int jk;
            for (jk = 0; jk < nosTemp.Count; jk++)
              if (Geom.Iguais(x, nosTemp[jk].x) && Geom.Iguais(y, nosTemp[jk].y) && Geom.Iguais(z, nosTemp[jk].z))
                 return nosTemp[jk];

            return null;
        }

        List<TNoPortico> nosTemp = new List<TNoPortico>();
        List<TBarraPortico> barrasTemp = new List<TBarraPortico>();
        TBarraPortico BarraNova;
        TNoPortico NoNovo;

        FProcessoCalculo fSubdividir;
        string TipoCarga;
        bool InserindoCarga, DividindoBarra, SelecionandoBarra;
        double ValorCarga;
        int sentidoCarga;
        public void NovaCarga(string tipo, double valor, int sentido)
        {
            CancelaOperacoes();
            sentidoCarga = sentido;
            InserindoCarga = true;
            SelecionandoBarra = true;
            ValorCarga = valor;
            TipoCarga = tipo;
        }

        double MaxCargaDistribuida, MaxCargaPontual;
        public void HandleNovaCarga()
        {
            if (TipoCarga == Const.ID_CARGA_LINEAR)
            {
                MaxCargaDistribuida = Math.Abs(ValorCarga);
                for (i = 1; i <= Portico.nBarras; i++)
                {
                    if (Math.Abs(Portico.barras[i].CargaDistribuida) > Math.Abs(MaxCargaDistribuida))
                      MaxCargaDistribuida = Math.Abs(Portico.barras[i].CargaDistribuida); 
                    
                    if (Portico.barras[i].PreSelecionada)
                    {
                        Portico.barras[i].CargaDistribuida = ValorCarga;
                        break;
                    }
                }
            }
            else
            if (TipoCarga == Const.ID_CARGA_PONTUAL)
            {
                MaxCargaPontual = Math.Abs(ValorCarga);
                for (i = 1; i <= Portico.nNos; i++)
                {
                    if (Math.Abs(Portico.nos[i].Carga[sentidoCarga]) > Math.Abs(MaxCargaPontual))
                        MaxCargaDistribuida = Math.Abs(Portico.nos[i].Carga[sentidoCarga]);

                    if (Portico.nos[i].PreSelecionado)
                    {
                        Portico.nos[i].Carga[sentidoCarga] = ValorCarga;
                        break;
                    }
                }
            }
        }

        void HandleDividirBarra()
        {
            double comp, tambarra;
            List<vec3> nos = new List<vec3>();

            for (i = 1; i <= Portico.nBarras; i++)
            {
                if (Portico.barras[i].PreSelecionada)
                {
                    comp = Portico.barras[i].L;
                    tambarra = comp / Divisoesbarra;
                  //  nos.Add(new vec3(Portico.barras[i].pIni.x, Portico.barras[i].pIni.y, Portico.barras[i].pIni.z));
                  //  nos.Add(new vec3(Portico.barras[i].pFin.x, Portico.barras[i].pFin.y, Portico.barras[i].pFin.z));

                    vec3 p1 = new vec3(Portico.barras[i].pIni.x, Portico.barras[i].pIni.y, Portico.barras[i].pIni.z);
                    vec3 p2 = new vec3(Portico.barras[i].pFin.x, Portico.barras[i].pFin.y, Portico.barras[i].pFin.z);
                    vec3 pMeio = (p1 + p2) / Divisoesbarra;
                    
                    double difx = Portico.barras[i].pIni.x;
                    double dify = Portico.barras[i].pIni.y;
                    double difz = Portico.barras[i].pIni.z;
                    
                    nos.Add(new vec3(pMeio.x, pMeio.y, pMeio.z));
                    vec3 pAnterior;

                    double distancia1 = pMeio.DistanceTo(p1);
                    double distancia2 = pMeio.DistanceTo(p2);
                    
                    pAnterior = new vec3(pMeio.x, pMeio.y, pMeio.z);
                    
                    if (distancia1 <= distancia2) // vai do pmeio ate o pfin
                    {
                        for (int j = 0; j < Divisoesbarra-2; j++)
                        {
                           // nos.Add(new vec3(pAnterior.x + pMeio.x, pAnterior.y + pMeio.y, pAnterior.z + pMeio.z));
                            pAnterior = new vec3(pAnterior.x + pMeio.x, pAnterior.y + pMeio.y, pAnterior.z + pMeio.z);
                        }
                    }
                    else
                    {
                        for (int j = 0; j < Divisoesbarra - 2; j++)
                        {
                    //       nos.Add(new vec3(pAnterior.x - pMeio.x, pAnterior.y - pMeio.y, pAnterior.z - pMeio.z));
                           pAnterior = new vec3(pAnterior.x - pMeio.x, pAnterior.y - pMeio.y, pAnterior.z - pMeio.z);
                        }
                    }

                   /* for (int j = 0; j < nos.Count; j++)
                    {
                        ++Portico.nNos;
                        Portico.nos[Portico.nNos] = new TNoPortico(nos[j].x, nos[j].y, nos[j].z,
                                                 0, 0, fSubdividir.cbApoio.SelectedIndex, Portico.nNos + 1, null, null);                    
                    }*/
                    TNoPortico pf = Portico.barras[i].pFin;

                    Portico.barras[i].pFin = Portico.nos[Portico.nNos];
                    Portico.barras[i].comprimento = (Math.Sqrt(Math.Pow(Portico.barras[i].pIni.x - Portico.barras[i].pFin.x, 2) + Math.Pow(Portico.barras[i].pIni.y - Portico.barras[i].pFin.y, 2) + Math.Pow(Portico.barras[i].pIni.z - Portico.barras[i].pFin.z, 2)));
                    Portico.barras[i].L = Portico.barras[i].comprimento / 100;


                    ++Portico.nBarras;
                    TBarraPortico BarraNova_ = new TBarraPortico(Portico.barras[i].pFin, pf, null, null, Portico.barras[i].E1, 1, Portico.barras[i].G1, 1, (float)Portico.barras[i].CargaDistribuida, 0, 0, 0, 0, 0, null, false, false, false, false, -1, false);
                    BarraNova_.InseridaManualmente = true;
                    Portico.barras[Portico.nBarras] = BarraNova_;
                    Portico.barras[Portico.nBarras].pIni = LocNo(BarraNova_.pIni.x, BarraNova_.pIni.y, BarraNova_.pIni.z);
                    Portico.barras[Portico.nBarras].pFin = LocNo(BarraNova_.pFin.x, BarraNova_.pFin.y, BarraNova_.pFin.z);
                    Portico.barras[Portico.nBarras].comprimento = (Math.Sqrt(Math.Pow(Portico.barras[Portico.nBarras].pIni.x - Portico.barras[Portico.nBarras].pFin.x, 2) + Math.Pow(Portico.barras[Portico.nBarras].pIni.y - Portico.barras[Portico.nBarras].pFin.y, 2) + Math.Pow(Portico.barras[Portico.nBarras].pIni.z - Portico.barras[Portico.nBarras].pFin.z, 2)));
                    Portico.barras[Portico.nBarras].L = Portico.barras[Portico.nBarras].comprimento / 100;
                    BarraNova_ = null;
                    Portico.barras[i].PreSelecionada = false;
               //     MessageBox.Show(pMeio.x + " " + pMeio.y + " " +pMeio.z);

                    break;
                }
            }
        }

        public void CancelaOperacoes()
        {
            lbInfoNo.Visible = false;
            NovaBarra = false;
            NovoNo = false;
            InserindoCarga = false;
            DividindoBarra = false;
            SelecionandoBarra = false;
        }

        public void ChamaSubdivirBarra()
        {
            /*fSubdividir = new FDividirBarraPortico(this);
            fSubdividir.Show();*/
        }
        int Divisoesbarra;

        public void DividirBarra(int n)
        {
            CancelaOperacoes();
            Divisoesbarra = n;
            DividindoBarra = true;
            SelecionandoBarra = true;
        }

        public void ControlesElemento(string tipo)
        {
            lbInfoNo.Visible = false;
            CancelaOperacoes();

            if (tipo == "barra")
            {
               NovoNo = false;
               Deslocamento = false;
               FletorY = false;
               FletorZ = false;
               CortanteY = false;
               CortanteZ = false;

            }

            if (tipo == "no")
            {
              NovoNo = true;
              NovaBarra = false;
              Deslocamento = false;
              FletorY = false;
              FletorZ = false;
              CortanteY = false;
              CortanteZ = false;
            }

            if (gerenciador.EsforcoPortico != null)
              gerenciador.EsforcoPortico.Close();
            gerenciador.ConfiguracoesPGi.PorticoOpcoesVisualizacao.MostrarNos = true;
        }

        public TNoPortico LocNo(double x, double y, double z)
        {
            int jk;
            for (jk = 1; jk <= Portico.nNos; jk++)
                if ((Object)Portico.nos[jk] != null)
                    if (Geom.Iguais(x, Portico.nos[jk].x, 0.1) && Geom.Iguais(y, Portico.nos[jk].y, 0.1) && Geom.Iguais(z, Portico.nos[jk].z, 0.1))
                        return Portico.nos[jk];

            return null;
        }
        public double EI, GJ;
        void HandleElementoNovo(double x, double y, double z)
        {
            TNoPortico novo = new TNoPortico(x,y,z);
            nosTemp.Add(novo);

            if (NovaBarra)
            {
                if (BarraNova == null)
                {
                    BarraNova = new TBarraPortico(new TNoPortico(x, y, z),new TNoPortico(0, 0, 0),null,null,EI,1,GJ,1,0,0,0,0,0,0,null,false,false,false,false,-1,false);
                    BarraNova.InseridaManualmente = true;
                }
                else
                {
                    barrasTemp.Add(BarraNova);
                 //   Portico.nBarras = barrasTemp.Count();
                    Portico.nBarras ++;
                    Portico.barras[Portico.nBarras]      = BarraNova;
                    Portico.barras[Portico.nBarras].pIni = LocNo(BarraNova.pIni.x,BarraNova.pIni.y,BarraNova.pIni.z);
                    Portico.barras[Portico.nBarras].pFin = LocNo(BarraNova.pFin.x, BarraNova.pFin.y, BarraNova.pFin.z);
                    Portico.barras[Portico.nBarras].comprimento = (Math.Sqrt(Math.Pow(Portico.barras[Portico.nBarras].pIni.x - Portico.barras[Portico.nBarras].pFin.x, 2) + Math.Pow(Portico.barras[Portico.nBarras].pIni.y - Portico.barras[Portico.nBarras].pFin.y, 2) + Math.Pow(Portico.barras[Portico.nBarras].pIni.z - Portico.barras[Portico.nBarras].pFin.z, 2)));
                    Portico.barras[Portico.nBarras].L           = Portico.barras[Portico.nBarras].comprimento / 100;

                    // MessageBox.Show(gerenciador.PorticoEspacial.nBarras.ToString());
                  //  MessageBox.Show(Portico.nBarras.ToString());
                    BarraNova = null;
                }
            }
            else
            if (NovoNo)
            {

            }
        }
        bool capPonto;
        void CapturaPonto()
        {
            xPontoCaptura = -1;
            yPontoCaptura = -1;
            zPontoCaptura = -1;

            capPonto = false;

            for (i = 1; i <= Portico.nNos; i++)
            {
                ultZ = 0;
                ultX = 0;
                ultY = 0;
                Portico.nos[i].PreSelecionado = false;
                        
                GL.Color3(0, 0, 250);
                GL.LineWidth(3);
                xNo = Portico.nos[i].x - (centroX);
                yNo = Portico.nos[i].y - (centroY);
                zNo = Portico.nos[i].z;
                GL.LineWidth(1);
                Project(ref px_x, ref px_y, xNo, yNo, zNo,ModelViewMatrix,ProjectionMatrix);
                Portico.nos[i].px_x = (int)px_x[0];
                Portico.nos[i].px_y = (int)px_y[0];

                if ((mouseX >= (px_x[0] - 17)) && (mouseX <= (px_x[0] + 17)))
                {
                    if ((mouseY >= (px_y[0] - 17) && (mouseY <= (px_y[0] + 17))))
                    {

                       // GL.Translate(34, 18, -50);


                        GL.PushMatrix();
                        GL.Rotate(90, 1, 0.0, 0.0);
                        GL.Rotate(90, 0.0, 0, 1); 
                        
                        GL.Color3(255, 0, 0);
                        GL.Begin(PrimitiveType.LineLoop);
                        GL.Vertex3((xNo - 5), (yNo - 5), zNo);
                        GL.Vertex3((xNo + 5), (yNo - 5), zNo);
                        GL.Vertex3((xNo + 5), (yNo + 5), zNo);
                        GL.Vertex3((xNo - 5), (yNo + 5), zNo);
                        GL.End();
                        GL.PopMatrix();

                        Portico.nos[i].PreSelecionado = true;

                        xPontoCaptura = Portico.nos[i].x;
                        yPontoCaptura = Portico.nos[i].y;
                        zPontoCaptura = Portico.nos[i].z;
                        capPonto = true;
                        break;
                    }
                }
            }


        }

        double xPontoCaptura, yPontoCaptura, zPontoCaptura; 
        double xPonto2, yPonto2, zPonto2;
        bool CapturouPonto1, CapturouPonto2,clic1, clic2;
        private void FVisualizadorPortico_Resize(object sender, EventArgs e)
        {
    //        TOpenGl.ReShapeOGL(Controle.Width, Controle.Height, angx, angy, angz); 
        }
        int i, k, j;
        public double MaximoEsforco, tamNo = 1;
        public bool CargaLinear, CargaPontual;
        public Color backColor = Color.White;
        double x_rot_angle, y_rot_angle, x_trans, y_trans;
        int incAnimar = 0;
        bool decrescer = false;
        void RetanguloSelecao(int x1, int y1)
        {
            GL.Color3(Color.Red);
            GL.LineWidth(2);
            GL.Begin(PrimitiveType.Lines);

            UnProject(100, 100, ModelViewMatrix, ProjectionMatrix, posX3d, posY3d, posZ3d);
            GL.Vertex3(posX3d[0], posY3d[0], posZ3d[0]);
            UnProject(5000, 500, ModelViewMatrix, ProjectionMatrix, posX3d, posY3d, posZ3d);
            GL.Vertex3(posX3d[0], posY3d[0], posZ3d[0]);

            //            GL.Vertex3(posX3d[0], posY3d[0], posZ3d[0]);
            GL.End();
        }

        public void Render(bool Animar = false)
        {
            try
            {
                GL.Clear(ClearBufferMask.ColorBufferBit | ClearBufferMask.DepthBufferBit);
                GL.MatrixMode(MatrixMode.Modelview);
                GL.ClearColor(backColor);

                if (Perspectiva)
                {
                    GL.LoadIdentity();
                    GL.PushMatrix();
                    GL.Translate(x_trans, y_trans, -cameraDistance);
                    GL.Rotate(x_rot_angle, 1, 0.0, 0.0);
                    GL.Rotate(y_rot_angle, 0.0, 0, 1.0);
                }
                      //GL.LineWidth(1);               
                   /*   GL.Color3(105, 105, 105);
                      GL.Begin(PrimitiveType.Lines);
                      GL.Vertex3(-1000000 - centroX, centroY,0);
                      GL.Vertex3( 1000000 - centroX, centroY, 0);
                      GL.Vertex3(centroX, -1000000 - centroY, 0);
                      GL.Vertex3(centroX, 1000000 - centroY, 0);
                      GL.End();*/
                /*
                      CapturaPonto();

                     /* if (NovaBarra)
                     {
                          GL.LineWidth(1); 
                          GL.Color3(0, 50, 200);
                          GL.Begin(PrimitiveType.Lines);
                          GL.Vertex3(xPontoCaptura - centroX, yPontoCaptura - centroY, zPontoCaptura);
                          GL.Vertex3(xPonto2 - centroX, yPonto2 - centroY, zPonto2);
                          GL.End();
                      }*/
                //  }
                //else
                {
                    if (Deslocamento == false && Portico.CalculouEsforcos)
                    {
                        for (i = 1; i <= Portico.nBarras; i++)
                        {
                        }
                    }
                }

                for (i = 1; i <= Portico.nBarras; i++)
                {
                    GL.Begin(PrimitiveType.Lines);

                    //           if ((gerenciador.ConfiguracoesPGi.GrelhaOpcoesVisualizacao.BarrasLajeVisiveis && !Pavimento.barras[i].barraViga) || (Pavimento.barras[i].barraViga))

                    if (Deslocamento &&  animar)
                    {

                        this.Text = incAnimar.ToString();
                    }
                    else
                    {

                    }

                    GL.End();

                    if (Portico.barras[i].CargaDistribuida != 0 && CargaLinear)
                    {

                    }
                }

               /* for (i = 0; i < barrasTemp.Count; i++)
                {
                    GL.Begin(PrimitiveType.Lines);
                    //           if ((gerenciador.ConfiguracoesPGi.GrelhaOpcoesVisualizacao.BarrasLajeVisiveis && !Pavimento.barras[i].barraViga) || (Pavimento.barras[i].barraViga))
                    barrasTemp[i].Desenha3(centroX,
                                                centroY,
                                                FatorEscalaDeslocamento * (double)iEscalaDiagrama,
                                                gerenciador.ConfiguracoesPGi.PorticoOpcoesVisualizacao.Diagramas_DeslocamentoGradiente,
                                                Deslocamento);
                    GL.End();
                }*/
/*
                foreach (TPavimento p in Portico.Pavimentos)
                {
          
                    foreach (TTrechoViga Trecho in p.vigas)
                        Trecho.Render3(centroX, centroY, p.Nivel, gerenciador.ConfiguracoesPGi.F3OpcoesVisualizacao.TranspVigas, gerenciador.ConfiguracoesPGi.F3OpcoesVisualizacao.RgbVigas, gerenciador.ConfiguracoesPGi.F3OpcoesVisualizacao.ArestasVigas);
                }*/

                #region Molas
                /* Desenha as molas*/
                for (i = 1; i <= Portico.nNos; i++)
                {
                    ultZ = 0;
                    ultX = 0;
                    ultY = 0;

                    GL.Color3(0, 100, 250);

                    xNo = Portico.nos[i].x - (centroX);
                    yNo = Portico.nos[i].y - (centroY);
                    zNo = Portico.nos[i].z;
                    
                 //   if (CargaPontual)
                  //    Portico.nos[i].DesenhaCargaPontual(centroX, centroY, (10 / MaxCargaPontual) * (double)iEscalaCarga);

                    if (gerenciador.ConfiguracoesPGi.PorticoOpcoesVisualizacao.MostrarNos)
                    {
                        if (Deslocamento)
                        {
                            xNo = Portico.nos[i].x - centroX + (Portico.nos[i].Deslocamento[1] * FatorEscalaDeslocamento * (double)iEscalaDiagrama);
                            yNo = Portico.nos[i].y - centroY + (Portico.nos[i].Deslocamento[3] * FatorEscalaDeslocamento * (double)iEscalaDiagrama);
                            zNo = Portico.nos[i].z + (Portico.nos[i].Deslocamento[2] * FatorEscalaDeslocamento * (double)iEscalaDiagrama);
                        }

                        GL.Begin(PrimitiveType.LineLoop);
                        GL.Vertex3(xNo - tamNo, yNo - tamNo, zNo + tamNo);
                        GL.Vertex3(xNo + tamNo, yNo - tamNo, zNo + tamNo);
                        GL.Vertex3(xNo + tamNo, yNo + tamNo, zNo + tamNo);
                        GL.Vertex3(xNo - tamNo, yNo + tamNo, zNo + tamNo);
                        GL.End();

                        GL.Begin(PrimitiveType.LineLoop);
                        GL.Vertex3(xNo - tamNo, yNo - tamNo, zNo - tamNo);
                        GL.Vertex3(xNo + tamNo, yNo - tamNo, zNo - tamNo);
                        GL.Vertex3(xNo + tamNo, yNo + tamNo, zNo - tamNo);
                        GL.Vertex3(xNo - tamNo, yNo + tamNo, zNo - tamNo);
                        GL.End();

                        GL.Begin(PrimitiveType.Lines);
                        GL.Vertex3(xNo - tamNo, yNo - tamNo, zNo - tamNo);
                        GL.Vertex3(xNo - tamNo, yNo - tamNo, zNo + tamNo);
                        GL.End();
                        GL.Begin(PrimitiveType.Lines);
                        GL.Vertex3(xNo + tamNo, yNo - tamNo, zNo - tamNo);
                        GL.Vertex3(xNo + tamNo, yNo - tamNo, zNo + tamNo);
                        GL.End();
                        GL.Begin(PrimitiveType.Lines);
                        GL.Vertex3(xNo - tamNo, yNo + tamNo, zNo - tamNo);
                        GL.Vertex3(xNo - tamNo, yNo + tamNo, zNo + tamNo);
                        GL.End();
                        GL.Begin(PrimitiveType.Lines);
                        GL.Vertex3(xNo + tamNo, yNo + tamNo, zNo - tamNo);
                        GL.Vertex3(xNo + tamNo, yNo + tamNo, zNo + tamNo);
                        GL.End();
                    }
                    
                    if (Portico.nos[i].vinculo == 1)
                    {
                        GL.Begin(PrimitiveType.LineLoop);
                        GL.Vertex3(xNo - 10, yNo + 10, zNo - 10);
                        GL.Vertex3(xNo + 10, yNo + 10, zNo - 10);
                        GL.Vertex3(xNo, yNo , zNo);
                        GL.End();
                        GL.Begin(PrimitiveType.LineLoop);
                        GL.Vertex3(xNo - 10, yNo - 10, zNo - 10);
                        GL.Vertex3(xNo + 10, yNo - 10, zNo - 10);
                        GL.Vertex3(xNo, yNo, zNo);
                        GL.End();
                        GL.Begin(PrimitiveType.LineLoop);
                        GL.Vertex3(xNo - 10, yNo + 10, zNo - 10);
                        GL.Vertex3(xNo + 10, yNo + 10, zNo - 10);
                        GL.Vertex3(xNo + 10, yNo - 10, zNo - 10);
                        GL.Vertex3(xNo - 10, yNo - 10, zNo - 10);
                        GL.End();
                    }

                    if (Portico.nos[i].vinculo == 2)
                    {
                       // xNo = Portico.nos[i].x - (centroX);
                      //  yNo = Portico.nos[i].y - (centroY);
                     //   zNo = Portico.nos[i].z;

                        GL.Begin(PrimitiveType.LineLoop);
                        GL.Vertex3(xNo - 10, yNo - 10, zNo);
                        GL.Vertex3(xNo + 10, yNo - 10, zNo);
                        GL.Vertex3(xNo + 10, yNo + 10, zNo);
                        GL.Vertex3(xNo - 10, yNo + 10, zNo);
                        GL.End();

                        GL.Begin(PrimitiveType.Lines);
                        GL.Vertex3(xNo - 10, yNo - 10, zNo);
                        GL.Vertex3(xNo - 10, yNo - 10, zNo - 10);
                        GL.End();

                        GL.Begin(PrimitiveType.Lines);
                        GL.Vertex3(xNo + 10, yNo - 10, zNo);
                        GL.Vertex3(xNo + 10, yNo - 10, zNo - 10);
                        GL.End();
                        GL.Begin(PrimitiveType.Lines);
                        GL.Vertex3(xNo + 10, yNo + 10, zNo);
                        GL.Vertex3(xNo + 10, yNo + 10, zNo - 10);
                        GL.End();

                        GL.Begin(PrimitiveType.Lines);
                        GL.Vertex3(xNo - 10, yNo + 10, zNo);
                        GL.Vertex3(xNo - 10, yNo + 10, zNo - 10);
                        GL.End();

                        GL.Begin(PrimitiveType.Lines);
                        GL.Vertex3(xNo, yNo - 10, zNo);
                        GL.Vertex3(xNo, yNo - 10, zNo - 10);
                        GL.End();
                        GL.Begin(PrimitiveType.Lines);
                        GL.Vertex3(xNo - 10, yNo, zNo);
                        GL.Vertex3(xNo - 10, yNo, zNo - 10);
                        GL.End();
                        GL.Begin(PrimitiveType.Lines);
                        GL.Vertex3(xNo + 10, yNo, zNo);
                        GL.Vertex3(xNo + 10, yNo, zNo - 10);
                        GL.End();
                        GL.Begin(PrimitiveType.Lines);
                        GL.Vertex3(xNo, yNo + 10, zNo);
                        GL.Vertex3(xNo, yNo + 10, zNo - 10);
                        GL.End();


                        /*      Mola de translação  */
                        /* GL.Begin(Gl.GL_LINE_STRIP);
                         GL.Vertex3(xNo, yNo, 0);
                         GL.Vertex3(xNo, yNo, -1);

                         for (k = 0; k < 2; k++)
                         {
                             for (j = 0; j <= 10; j++)
                             {
                                 GL.Vertex3((xNo + (1 * Math.Cos(j * twicePi / 10))),
                                               (yNo + (1 * Math.Sin(j * twicePi / 10))), ultZ - 1);
                                 ultZ = ultZ - .05;
                             };
                             ultZ += .05;
                         }
                         GL.End();*/

                        /*      Mola de rotação em x  */
                        /*  GL.Begin(Gl.GL_LINE_STRIP);
                          GL.Vertex3(xNo, yNo, 0);
                          GL.Vertex3(xNo + 0.5, yNo, 0);
                          for (k = 0; k < 2; k++)
                          {
                              for (j = 0; j <= 10; j++)
                              {
                                  GL.Vertex3(xNo - ultX + 0.5,
                                                yNo + (0.2 * Math.Cos(j * twicePi / 10)),
                                                (0.2 * Math.Sin(j * twicePi / 10)));
                                  ultX = ultX - .02;
                              };
                              ultX += .02;
                          }
                          GL.End();

                          /*      Mola de rotação em y  */
                        /*  GL.Begin(Gl.GL_LINE_STRIP);
                          GL.Vertex3(xNo, yNo, 0);
                          GL.Vertex3(xNo, yNo + 0.5, 0);
                          for (k = 0; k < 2; k++)
                          {
                              for (j = 0; j <= 10; j++)
                              {
                                  GL.Vertex3((xNo + (.2 * Math.Cos(j * twicePi / 10))),
                                                yNo - ultY + 0.5,
                                                (.2 * Math.Sin(j * twicePi / 10)));
                                  ultY = ultY - .02;
                              };
                              ultY += .02;
                          }
                          GL.End();*/
                        /* ----------------------- */

                    }
                }
                #endregion
                UpdateOGLMatrix(ModelViewMatrix);

                GL.PopMatrix();

                if (Portico.nos[1] != null)
                {
                    GL.PushMatrix();
                   // GL.Rotate(90, 0, 1, 0.0);
                    GL.Color3(Color.Red);
                    GL.Begin(PrimitiveType.LineLoop);
                    GL.Vertex3((Portico.nos[1].x - 10), (Portico.nos[1].y - 10), Portico.nos[1].z);
                    GL.Vertex3((Portico.nos[1].x + 10), (Portico.nos[1].y - 10), Portico.nos[1].z);
                    GL.Vertex3((Portico.nos[1].x + 10), (Portico.nos[1].y + 10), Portico.nos[1].z);
                    GL.Vertex3((Portico.nos[1].x - 10), (Portico.nos[1].y + 10), Portico.nos[1].z);
                    GL.End();
                    GL.PopMatrix();
                }

                if (NovoNo || SelecionandoBarra)
                    CapturaPonto();

                if (NovaBarra && !rodando_movendo)
                {
                    CapturaPonto();
                    if (BarraNova != null)
                    {
                        if (capPonto)
                        {
                            if (xPontoCaptura != -1 && yPontoCaptura != -1 && zPontoCaptura != -1)
                            {
                                BarraNova.MouseMove(xPontoCaptura, yPontoCaptura, zPontoCaptura);
                               // BarraNova.DesenhaTemp(centroX, centroY);
                            }
                        }
                        else
                        {// senao pega o ponto que o mouse esta, na cota z do ponto incial

                            /* UnProject(mouseX, mouseY, ModelViewMatrix, ProjectionMatrix, posX3d, posY3d, posZ3d);
                             zPontoCaptura = BarraNova.pIni.z;
                             xPontoCaptura = posX3d[0];
                             yPontoCaptura = posY3d[0];
                             */
                            Project(ref px_x_i2, ref px_y_i2, posX3d[0], posY3d[0], BarraNova.pIni.z, ModelViewMatrix, ProjectionMatrix);


                            UnProject((int)(px_x_i2[0]), (int)(px_y_i2[0]), ModelViewMatrix, ProjectionMatrix, posX3d, posY3d, posZ3d);
                            this.Text = posX3d[0].ToString("n2") + " , " + posY3d[0].ToString("n2") + " , " + posZ3d[0].ToString("n2");

                            zPontoCaptura = BarraNova.pIni.z;
                            xPontoCaptura = posX3d[0];
                            yPontoCaptura = posY3d[0];

                            gerenciador.Text = px_x_i[0].ToString() + " , " + px_y_i[0].ToString();
                            BarraNova.MouseMove(xPontoCaptura, yPontoCaptura, zPontoCaptura);
                           // BarraNova.DesenhaTemp(centroX, centroY);
                        }

                    }
                }

                Controle.SwapBuffers();
            }
            catch(Exception e)
            { 
                MessageBox.Show("Erro ao desenhar pórtico: " + e.Message);
                this.Close();
            }
        }
        double fatorzoom;
        private void ApplyZoom(int mousex, int mousey, int delta)
        {
            fatorzoom = System.Math.Exp((double)delta * 0.0014);


            GL.Translate((float)posX3d[0], (float)posY3d[0], (float)posZ3d[0]);
            GL.Scale((float)fatorzoom, (float)fatorzoom, (float)fatorzoom);
            GL.Translate(-(float)posX3d[0], -(float)posY3d[0], -(float)posZ3d[0]);

            Render();
     //       TOpenGl.OGL.Refresh();
        }
        double cameraDistance;
        bool wheel;
        public void Controle_MouseWheel(object sender, System.Windows.Forms.MouseEventArgs e)
        {
            if (!Perspectiva)
              ApplyZoom(e.X, e.Y, e.Delta);
            else
            {
                wheel = true;
                cameraDistance -= (e.Delta * .5);
                Render();
                wheel = false;

                double xa = posX;
                double ya = posY;
                double za = posZ;

                zant = posZ;
            }
        }

        private void Controle_MouseDown(object sender, System.Windows.Forms.MouseEventArgs e)
        {
            ultx = e.X;
            ulty = e.Y;

            if ((NovaBarra || NovoNo) && (xPontoCaptura != -1 && yPontoCaptura != -1 && zPontoCaptura != -1))
              HandleElementoNovo(xPontoCaptura, yPontoCaptura, zPontoCaptura);

            if (InserindoCarga)
            {
              //  HandleNovaCarga(xPontoCaptura, yPontoCaptura, zPontoCaptura);
            }
            else
            if (DividindoBarra)
            {
                HandleDividirBarra();
            }

            if (e.Button == System.Windows.Forms.MouseButtons.Middle)
            {
                if (Perspectiva)
                {
                    this.Controle.Cursor = System.Windows.Forms.Cursors.Hand;
                    Panning = true;

                    start_x = e.X;
                    start_y = e.Y;

                    //      x_trans = start_x;
                    //     y_trans = start_y;
                }
            }

        }

        int dx, dy, dz, x, y;
        double angle, ax, ay, az;
        double[] _MatrixInverse = new double[16];
        bool rodando_movendo;
        public bool shift;
        public static double[] ModelViewMatrix = new double[16];
        public static double[] mvm_Selecao = new double[16];

        public static double[] ProjectionMatrix = new double[16];
        public static int[] ViewPort = new int[4];
        public static void UpdateOGLMatrix(double[] model)
        {
            GL.GetDouble(GetPName.ModelviewMatrix, model);
            GL.GetDouble(GetPName.ProjectionMatrix, ProjectionMatrix);
            GL.GetInteger(GetPName.Viewport, ViewPort);
        }
        int mouseX, mouseY;
        bool Panning;
        private void Controle_MouseMove(object sender,System.Windows.Forms.MouseEventArgs e)
        {
           /* if (gerenciador.ActiveMdiChild != this) 
                return;
            rodando_movendo = false;
            
            if (Portico.nNos > 0)
            {
                double px, py, pz;
                x = e.X;
                y = e.Y;

                dx = x - mouseX;
                dy = y - mouseY;
                dz = (int)(posZ - zant);

                UpdateOGLMatrix(ModelViewMatrix);

              //  ogl.pos(&px, &py, &pz, x, y, ViewPort, (int)GLright, (int)GLleft, (int)GLtop, (int)GLbotton, (int)zNear);
            //    this.Text = "";
                angle = 0;

                if (e.Button == System.Windows.Forms.MouseButtons.Middle)
                {
                    rodando_movendo = true;

                    if (!Perspectiva)
                    {

                        GL.MatrixMode(MatrixMode.Modelview);
                        GL.LoadIdentity();

                        GL.Translate((float)px - _dragPosX, (float)py - _dragPosY, (float)pz - _dragPosZ);

                        GL.MultMatrix(ModelViewMatrix);
                    }
                    else
                    {
                        if (Panning)
                        {
                            x_trans = x_trans + (e.X - start_x) / 1;
                            y_trans = y_trans - (e.Y - start_y) / 1;

                            //    start_x = e.X;
                            //    start_y = e.Y;
                        }
                    }
                }
                else 
                if (e.Button == System.Windows.Forms.MouseButtons.Left && shift)
                {
                    this.Controle.Cursor = Cur_rotacao;
                    this.Cursor = Cur_rotacao;

                    if (!Perspectiva)
                    {
                        rodando_movendo = true;
                        ogl.InvertMatrixd(ModelViewMatrix, _MatrixInverse);

                        ax = dy;
                        ay = dx;
                        az = dy;
                        angle = ogl.vlen(ax, ay, az) / (double)(ViewPort[2] + 1) * 360.0;

                        bx = _MatrixInverse[0] * ax + _MatrixInverse[4] * ay + _MatrixInverse[8] * az;
                        by = _MatrixInverse[1] * ax + _MatrixInverse[5] * ay + _MatrixInverse[9] * az;
                        bz = _MatrixInverse[2] * ax + _MatrixInverse[6] * ay + _MatrixInverse[10] * az;

                        x_angle = x_angle + (e.Y - start_y) / 180;
                        y_angle = y_angle + (e.X - start_x) / 180;

                        start_x = e.X;
                        start_y = e.Y;

                        GL.Rotate(angle, bx, by, bz);

                        ultx = e.X;
                        ulty = e.Y;
                    }
                    else
                    {
                        x_rot_angle += ((e.Y - start_y) * .3);
                        y_rot_angle += ((e.X - start_x) * .3);

                        start_x = e.X;
                        start_y = e.Y;
                    }
                }
                else
                {
                    mouseX = x;
                    mouseY = y;

                    GetInfoBarra();
                    lbInfoNo.Visible = false;
                    if (Deslocamento)
                      GetDeslocamentoNo(-1);
                    else
                    if (FletorY || FletorZ)
                    {
                        if (FletorY)
                            GetFletorY();
                        if (FletorZ)
                            GetFletorZ();
                    }
                    else
                    if (Axial)
                        GetAxial();
                    else
                    if (Torcor)
                        GetTorcor();
                    else
                    if (CortanteY || CortanteZ)
                    {
                        if (CortanteY)
                          GetCortanteY();
                        if (FletorZ)
                          GetFletorZ();
                    }

                    ultx = e.X;
                    ulty = e.Y;
                }

                start_x = e.X;
                start_y = e.Y; 

                mouseX = x;
                mouseY = y;

                _dragPosX = (float)px;
                _dragPosY = (float)py;
                _dragPosZ = (float)pz;*/

               /* if (!animar)
                 Render();
               // TOpenGl.OGL.Refresh();

                 //  this.Text = e.X.ToString() + "  " + e.Y.ToString();
                zant = posZ;

           //     UpdateOGLMatrix(ModelViewMatrix);
                UnProject(e.X, e.Y, ModelViewMatrix, ProjectionMatrix, posX3d, posY3d, posZ3d);*/
          //  }      

        }
        double[] winZ = new double[1];
        double realY;

        double[] posX3d = new double[1];
        double[] posY3d = new double[1];
        double[] posZ3d = new double[1];
        void UnProject(int mx, int my, double[] model, double[] proj, double[] x_, double[] y_, double[] z_, bool z_zero = false)
        {
            realY = ViewPort[3] - (int)my;
          //  GL.ReadPixels(mx, my, 1, 1, OpenTK.Graphics.OpenGL.PixelFormat.DepthComponent, PixelType.Float, winZ);
            winZ[0] = 0;

            OpenTK.Graphics.Glu.UnProject(mx, realY, winZ[0], model, proj, ViewPort, x_, y_, z_);
        }

        public static void Project(ref double[] pixelX, ref double[] pixelY, double worldX, double worldY, double worldZ, double[] model, double[] proj)
        {
            double[] pixelZ = new double[1];
            OpenTK.Graphics.Glu.Project(worldX, worldY, worldZ, model, proj, ViewPort, pixelX, pixelY, pixelZ);
            pixelY[0] = ViewPort[3] - pixelY[0];
        }

        double angx = -55, angy = 0, angz = 30;


        public void ChamaInfoBarra()
        {
            if (BarraTemp != null)
            {
                FInfoBarraPortico info = new FInfoBarraPortico(BarraTemp, gerenciador);
                info.Show();
            }
        }

        private void FVisualizadorPortico_FormClosing(object sender, FormClosingEventArgs e)
        {
           // gerenciador.cbTipoDiagramaGrelha.SelectedIndex = 0;
            Dispose();

            gerenciador.EsforcoPortico.Close();

            gerenciador.FVisPortico = null;
            gerenciador.VoltaParaPavimento();
        }

        public void Enquadrar()
        {
            return;
            // glOrtho(left, right, bottom, top, zNear, zFar);

            /*
             zNear = 1.0; zFar = zNear + diam; 
            Structure your matrix calls in this order (for an Orthographic projection):
            */
            //   double left = c.x - diam; 
            //   double right = c.x + diam;
            //    double bottom c.y - diam; 
            //   double top = c.y + diam; 


            /*    double aspect = (double)Controle.Size.Width / Controle.Size.Height;
                if (aspect < 1.0)
                {
                    // window taller than wide 
                    bottom /= aspect;
                    top /= aspect;
                }
                else
                {
                    left *= aspect;
                    right *= aspect;
                } 
                */

            /* double OGLtop = 1;
             double OGLbotton = -1;
             double OGLleft = -(double)(Controle.Size.Width) / (double)Controle.Size.Height;
             double OGLright = -OGLleft;

             GL.MatrixMode(Gl.GL_PROJECTION);
             GL.LoadIdentity();
             GL.Ortho(OGLleft, OGLright, OGLbotton, OGLtop, -100000, 1000);
             GL.MatrixMode(Gl.GL_MODELVIEW);
             GL.LoadIdentity();

             return;*/
           // return;
            _dragPosX = 0;
            _dragPosY = 0;
            _dragPosZ = 0;

            int x = (int)p_xini, y = (int)p_yini;
            double s = System.Math.Exp((double)-120 * 0.001);
            if (Portico.nNos> 0)
            for (int i = 0; i < 170; i++)
            {
               g2d.UnProject(580, 286, ref posX, ref posY, ref posZ);
 
               GL.Translate((float)posX, (float)posY, (float)posZ);
               GL.Scale((float)s, (float)s, (float)s);
               GL.Translate(-(float)posX, -(float)posY, -(float)posZ);

                Render();
               // Controle_MouseMove(Controle, null);
                //  ApplyZoom(580, 286, -120);
                /*  x -= i;

                  TOpenGl.UpdateOGLMatrix();
                  TOpenGl.pos(&px, &py, &pz, x, y, TOpenGl.ViewPort);

                  GL.MatrixMode(Gl.GL_MODELVIEW);
                  GL.LoadIdentity();

                  GL.Translatef((float)px - _dragPosX, (float)py - _dragPosY, (float)pz - _dragPosZ);

                  GL.MultMatrixd(TOpenGl.ModelViewMatrix);

                  xant = x;
                  yant = y; 
                
                  _dragPosX = (float)px;
                  _dragPosY = (float)py;
                  _dragPosZ = (float)pz;
                  */

                GetMiniMaxPt(ref p_xini, ref p_xfin, ref p_yini, ref p_yfin,
                             ref xIni, ref yIni, ref xFin, ref yFin);
              
              //  GetMiniMaxPt(ref p_xini, ref p_xfin, ref p_yini, ref p_yfin,
              //   ref xIni, ref yIni, ref xFin, ref yFin);

              //  MessageBox.Show("p_xini:" + p_xini.ToString() + " p_xfin:" + p_xfin.ToString() + "  p_yini:" + p_yini.ToString() + " p_yfin:" + p_yfin.ToString());
 
                if ((p_xfin < Controle.Width)&&// && p_xfin > (Controle.Width - 50))&&
                    (p_yfin < Controle.Height) &&
                    (p_yini > 150 && p_yini < 200))// && p_yfin > (Controle.Height - 50)))
                    break;
                //   Controle.SwapBuffers();
            }
        }

        double xIni = 999999999;
        double yIni = 999999999;

        double xFin = -999999999;
        double yFin = -999999999;
        private void GetMiniMaxPt(ref float p_xini, ref float p_xfin, ref float p_yini, ref float p_yfin,
                  ref double xIni, ref double yIni, ref double xFin, ref double yFin)
        {
            p_xini = 999999999;
            p_yini = 999999999;
            p_xfin = -999999999;
            p_yfin = -999999999;

            xIni = 999999999;
            yIni = 999999999;

            xFin = -999999999;
            yFin = -999999999;

            for (i = 1; i <= Portico.nNos; i++)
            {
                if (!Portico.nos[i].Enquadrar) continue;

                if (Portico.nos[i].px_x < p_xini)
                {
                    p_xini = Portico.nos[i].px_x;
                };

                if (Portico.nos[i].px_y < p_yini)
                {
                   p_yini = Portico.nos[i].px_y;
                };

                if (Portico.nos[i].px_x > p_xfin)
                {
                    p_xfin = Portico.nos[i].px_x;
                };

                if (Portico.nos[i].px_y > p_yfin)
                {
                    p_yfin = Portico.nos[i].px_y;
                };
            };
        }
        public void AtualizaFormCores()
        {
        }

        public void VistaPerspectiva(bool enquadar = true)
        {
            return;
            GL.Clear(ClearBufferMask.ColorBufferBit | ClearBufferMask.DepthBufferBit);
            GL.MatrixMode(MatrixMode.Modelview);
            GL.LoadIdentity();
            if (enquadar)
               Enquadrar();
            GL.Rotate(-55, 1, 0, 0);
            GL.Rotate(45, 0, 0, 1);  
        }

        public void VistaCima()
        {
            GL.Clear(ClearBufferMask.ColorBufferBit | ClearBufferMask.DepthBufferBit);
            GL.MatrixMode(MatrixMode.Modelview);
            GL.LoadIdentity();
            Enquadrar();
        }

        public void VistaLado1()
        {
            GL.Clear(ClearBufferMask.ColorBufferBit | ClearBufferMask.DepthBufferBit);
            GL.MatrixMode(MatrixMode.Modelview);
            GL.LoadIdentity();
            Enquadrar();

            GL.Rotate(-90, 1, 0, 0);
            Render();
            Controle.SwapBuffers();
        }

        public void VistaLado2()
        {
            GL.Clear(ClearBufferMask.ColorBufferBit | ClearBufferMask.DepthBufferBit);
            GL.MatrixMode(MatrixMode.Modelview);
            GL.LoadIdentity();
            Enquadrar();

            GL.Rotate(-90, 1, 0, 0);
            GL.Rotate(90, 0, 0, 1);

            Render();
            Controle.SwapBuffers();
        }

        public void VistaLado3()
        {
            GL.Clear(ClearBufferMask.ColorBufferBit | ClearBufferMask.DepthBufferBit);
            GL.MatrixMode(MatrixMode.Modelview);
            GL.LoadIdentity();
            Enquadrar();

            GL.Rotate(-90, 1, 0, 0);
            GL.Rotate(180, 0, 0, 1);
            Render();
            Controle.SwapBuffers();
        }

        public void VistaLado4()
        {
            GL.Clear(ClearBufferMask.ColorBufferBit | ClearBufferMask.DepthBufferBit);
            GL.MatrixMode(MatrixMode.Modelview);
            GL.LoadIdentity();
            Enquadrar();

            GL.Rotate(-90, 1, 0, 0);
            GL.Rotate(270, 0, 0, 1);
            Render();
            Controle.SwapBuffers();
        }
        
        private void Controle_MouseEnter(object sender, EventArgs e)
        {

        }

        private void cancelarToolStripMenuItem_Click(object sender, EventArgs e)
        {
            NovoNo    = false;
            NovaBarra = false;
            BarraNova = null;
            NoNovo    = null;
            CancelaOperacoes();
        }

        private void AnimarToolStripMenuItem_Click(object sender, EventArgs e)
        {
            animar = animarToolStripMenuItem.Checked;
            timer1.Enabled = animarToolStripMenuItem.Checked;
           /* if (Deslocamento)
            {
                if (gerenciador.EsforcoPortico!= null)
                {
                    gerenciador.EsforcoPortico.trackEscalaDiagrama.Value = 0;

                    for (int i = 0; i < 8; i++)
                    {
                        Thread.Sleep(150);
                        gerenciador.EsforcoPortico.trackEscalaDiagrama.Value += 1;
                        gerenciador.EsforcoPortico.AtualizaEscala();
                    //    Render();
                    //    Controle.SwapBuffers();
                    }
                    for (int i = 0; i < 8; i++)
                    {
                        Thread.Sleep(150);
                        gerenciador.EsforcoPortico.trackEscalaDiagrama.Value -= 1;
                        gerenciador.EsforcoPortico.AtualizaEscala();
                   //     Render();
                   //     Controle.SwapBuffers();
                    }
                }
            }*/

        //    Render()
        }
        bool animar = false;

        private void Controle_MouseUp(object sender, System.Windows.Forms.MouseEventArgs e)
        {
            if (e.Button == System.Windows.Forms.MouseButtons.Middle)
            {
                Panning = false;
            };
            this.Controle.Cursor = System.Windows.Forms.Cursors.Default;
            this.Cursor = System.Windows.Forms.Cursors.Default;
        }
        double iEscalaDiagramaAnimacao;
        private void timer1_Tick(object sender, EventArgs e)
        {
            if (iEscalaDiagramaAnimacao > 8 && decrescer == false)
            {
                decrescer = true;
            }

            if (decrescer)
                iEscalaDiagramaAnimacao--;
            else
                iEscalaDiagramaAnimacao++;

            if (iEscalaDiagramaAnimacao == 0 && decrescer)
                decrescer = false;
            
            Render(animar);
        }

        private void button1_Click(object sender, EventArgs e)
        {
            double cx, cy, cz;
           double cos_alpha,  sen_alpha;

           cx = ((barrasTemp[0].pFin.x - barrasTemp[0].pIni.x) / 100) / barrasTemp[0].L;
           cy = ((barrasTemp[0].pFin.y - barrasTemp[0].pIni.y) / 100) / barrasTemp[0].L;
           cz = ((barrasTemp[0].pFin.z - barrasTemp[0].pIni.z) / 100) / barrasTemp[0].L;
           double alfa2 = 0;

            cos_alpha = Math.Cos(alfa2 * Const.PIDiv180);
            sen_alpha = Math.Sin(alfa2 * Const.PIDiv180);

            button1.Text = "cx: " + cx.ToString("n2") + "cy: " + cy.ToString("n2") + "cz: " + cz.ToString("n2");
        }
    }
}
