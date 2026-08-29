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

using OpenTK;
using OpenTK.Input;
using OpenTK.Graphics.OpenGL;
using OpenTK.Platform;


namespace PG
{
    public unsafe partial class FVisualizadorGrelha : WeifenLuo.WinFormsUI.Docking.DockContent//
    {
        public static int h, w;

        public static float r, g, b;
       
        private static float _dragPosX = (float).0, _dragPosY = (float).0, _dragPosZ = (float).0;
        double posX, posY, posZ, centroX, centroY;
        double x_angle = 15, y_angle = 15;
        double zant = 0;
        int ultx, ulty;

        double start_x = 0, start_y = 0;
        double bx, by, bz;
        float twicePi = 2.0f * 3.1415f;
        double ultZ, ultX, ultY;
        double xNo, yNo;

        public TPavimento Pavimento;

        float p_xini, p_xfin, p_yini, p_yfin;

        double xIni = 999999999;
        double yIni = 999999999;

        double xFin = -999999999;
        double yFin = -999999999;
        double FatorEscalaDeslocamento;
        double[] FatorEscalaEsforco;

        public double[] Deslocamentos;
        public double[,] RGB_Deslocamentos;
   
        public double[] MomentosNegativos;
        public double[] MomentosPositivos;
        public double[,] RGB_FletoresNegativos;
        public double[,] RGB_FletoresPositivos;

        public double[] MomentosTorcoresNegativos;
        public double[] MomentosTorcoresPositivos;
        public double[,] RGB_TorcoresNegativos;
        public double[,] RGB_TorcoresPositivos;

        public double[] CortantesNegativos;
        public double[] CortantesPositivos;
        public double[,] RGB_CortantesNegativos;
        public double[,] RGB_CortantesPositivos;
      
        double [,] ListaRGB_EsforcosNegativos;
        double [,] ListaRGB_EsforcosPositivos;
        
        //Variáveis acessadas pelo Gerenciador

        public const int iFletor = 0;
        public const int iTorcor = 1;
        public const int iCortante = 2;
        public const int iDeslocamento = 3;

        public int iEsforcoAtual   = iDeslocamento;
        public int iEscalaDiagrama = 7;
        
        int iDeslocamentos = 0, iFletoresPositivos = 0, iFletoresNegativos = 0, iTorcoresNegativos = 0, iTorcoresPositivos = 0, iCortantesNegativos, iCortantesPositivos = 0;
       
        Gerenciador              gerenciador;
        
        public FVisualizadorGrelha() // GRELHA, PORTICO, TUDO
        {

        }

        public void CarregaPavimentoSelecionado()
        {
          /*  TOpenGl.AssociateOGL(this.Controle);
            TOpenGl.InitializeOGL();
            TOpenGl.ReShapeOGL(Controle.Width, Controle.Height);*/

            centroX = Pavimento.xi + ((Pavimento.xf - Pavimento.xi) / 2);
            centroY = Pavimento.yi + ((Pavimento.yf - Pavimento.yi) / 2);

            r = 1;
            g = 1;
            b = 1;

            FatorEscalaEsforco = new double[3];
            FatorEscalaEsforco[0] = 20 / Pavimento.MaximoEsforco(2);
            FatorEscalaEsforco[1] = 20 / Pavimento.MaximoEsforco(1);
            FatorEscalaEsforco[2] = 20 / Pavimento.MaximoEsforco(3);

            FatorEscalaDeslocamento = 20 / Pavimento.MaximoDeslocamento(3);

            iDeslocamentos = 0; iFletoresPositivos = 0; iFletoresNegativos = 0; iTorcoresNegativos = 0; iTorcoresPositivos = 0; iCortantesNegativos=0; iCortantesPositivos = 0;
            lbInfoNo.Visible = true;
            CriaVetores();
            CriaFormCores();
            AtualizaDeslocamentos();
        }

        public FVisualizadorGrelha(Gerenciador gerenciador, TPavimento pav, string Modo = "GRELHA") // GRELHA, PORTICO, TUDO
        {
            InitializeComponent();

          //  Controle.InitializeContexts();

            this.gerenciador = gerenciador;
            this.Pavimento = pav;
            
         //   CarregaPavimentoSelecionado();

/*
            OrdenaEsforcos("MOMENTO FLETOR", ref MomentosPositivos, ref MomentosNegativos, ref iFletoresNegativos, ref iFletoresPositivos);
            OrdenaEsforcos("MOMENTO TORCOR", ref MomentosTorcoresPositivos, ref MomentosTorcoresNegativos, ref iTorcoresNegativos, ref iTorcoresPositivos);
            OrdenaEsforcos("ESFORCO CORTANTE", ref CortantesPositivos, ref CortantesNegativos, ref iCortantesNegativos, ref iCortantesPositivos);

            if (iFletoresNegativos > 0)
              AtribuiCoresEsforcos("FLETOR NEGATIVO", ref MomentosNegativos, ref RGB_FletoresNegativos, ref ListaRGB_EsforcosNegativos, iFletoresNegativos, 6);
            if (iFletoresPositivos > 0)
              AtribuiCoresEsforcos("FLETOR POSITIVO", ref MomentosPositivos, ref RGB_FletoresPositivos, ref ListaRGB_EsforcosPositivos, iFletoresPositivos, 4);

            if (iTorcoresNegativos > 0)
              AtribuiCoresEsforcos("TORCOR NEGATIVO", ref MomentosTorcoresNegativos, ref RGB_TorcoresNegativos, ref ListaRGB_EsforcosNegativos, iTorcoresNegativos, 6);
            if (iTorcoresPositivos > 0)
              AtribuiCoresEsforcos("TORCOR POSITIVO", ref MomentosTorcoresPositivos, ref RGB_TorcoresPositivos, ref ListaRGB_EsforcosPositivos, iTorcoresPositivos, 4);

            if (iCortantesNegativos > 0)
              AtribuiCoresEsforcos("CORTANTE NEGATIVO", ref CortantesNegativos, ref RGB_CortantesNegativos, ref ListaRGB_EsforcosNegativos, iCortantesNegativos, 6);
            if (iCortantesPositivos > 0)
              AtribuiCoresEsforcos("CORTANTE POSITIVO", ref CortantesPositivos, ref RGB_CortantesPositivos, ref ListaRGB_EsforcosPositivos, iCortantesPositivos, 4);
            */
        }

        public void AtualizaDeslocamentos()
        {
            OrdenaDeslocamentos();
            AtribuiCoresDeslocamentos();
        }

        public void AtualizaEsforcos(string texto)
        {
            try
            {
             //   gerenciador.cbTipoDiagramaGrelha.SelectedIndex = 0;
                gerenciador.ChamaAguardar(this, texto);

                OrdenaEsforcos("MOMENTO FLETOR", ref MomentosPositivos, ref MomentosNegativos, ref iFletoresNegativos, ref iFletoresPositivos);
                OrdenaEsforcos("MOMENTO TORCOR", ref MomentosTorcoresPositivos, ref MomentosTorcoresNegativos, ref iTorcoresNegativos, ref iTorcoresPositivos);
                OrdenaEsforcos("ESFORCO CORTANTE", ref CortantesPositivos, ref CortantesNegativos, ref iCortantesNegativos, ref iCortantesPositivos);

                if (iFletoresNegativos > 0)
                    AtribuiCoresEsforcos("FLETOR NEGATIVO", ref MomentosNegativos, ref RGB_FletoresNegativos, ref ListaRGB_EsforcosNegativos, iFletoresNegativos, 6);
                if (iFletoresPositivos > 0)
                    AtribuiCoresEsforcos("FLETOR POSITIVO", ref MomentosPositivos, ref RGB_FletoresPositivos, ref ListaRGB_EsforcosPositivos, iFletoresPositivos, 4);

                if (iTorcoresNegativos > 0)
                    AtribuiCoresEsforcos("TORCOR NEGATIVO", ref MomentosTorcoresNegativos, ref RGB_TorcoresNegativos, ref ListaRGB_EsforcosNegativos, iTorcoresNegativos, 6);
                if (iTorcoresPositivos > 0)
                    AtribuiCoresEsforcos("TORCOR POSITIVO", ref MomentosTorcoresPositivos, ref RGB_TorcoresPositivos, ref ListaRGB_EsforcosPositivos, iTorcoresPositivos, 4);

                if (iCortantesNegativos > 0)
                    AtribuiCoresEsforcos("CORTANTE NEGATIVO", ref CortantesNegativos, ref RGB_CortantesNegativos, ref ListaRGB_EsforcosNegativos, iCortantesNegativos, 6);
                if (iCortantesPositivos > 0)
                    AtribuiCoresEsforcos("CORTANTE POSITIVO", ref CortantesPositivos, ref RGB_CortantesPositivos, ref ListaRGB_EsforcosPositivos, iCortantesPositivos, 4);

                AtualizaFormCores();
            }
            catch(Exception ms)
            {
                MessageBox.Show(ms.Message);
            }

            gerenciador.FechaAguardar();

          /*  if (gerenciador.ConfiguracoesPGi.GrelhaOpcoesVisualizacao.Suavizar)
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

public void AtualizaFormCores()
{
    lbd0.Visible = false;
    try
    {
        if (iEsforcoAtual == iFletor)
        {
            lbd1.Text = "-" + (RGB_FletoresNegativos[0, 0] * 100).ToString("n3");
            lbd2.Text = "-" + (RGB_FletoresNegativos[1, 0] * 100).ToString("n3");
            lbd3.Text = "-" + (RGB_FletoresNegativos[2, 0] * 100).ToString("n3");
            lbd4.Text = "-" + (RGB_FletoresNegativos[3, 0] * 100).ToString("n3");
            lbd5.Text = "-" + (RGB_FletoresNegativos[4, 0] * 100).ToString("n3");
            lbd6.Text = "-" + (RGB_FletoresNegativos[5, 0] * 100).ToString("n3");

            lbd7.Text = (RGB_FletoresPositivos[3, 0] * 100).ToString("n3");
            lbd8.Text = (RGB_FletoresPositivos[2, 0] * 100).ToString("n3");
            lbd9.Text = (RGB_FletoresPositivos[1, 0] * 100).ToString("n3");
            lbd10.Text = (RGB_FletoresPositivos[0, 0] * 100).ToString("n3");

            lbNomeEsforco.Text = "Fletores (Kgf.m)";
        }
        if (iEsforcoAtual == iTorcor)
        {
            lbd1.Text = "-" + (RGB_TorcoresNegativos[0, 0] * 100).ToString("n3");
            lbd2.Text = "-" + (RGB_TorcoresNegativos[1, 0] * 100).ToString("n3");
            lbd3.Text = "-" + (RGB_TorcoresNegativos[2, 0] * 100).ToString("n3");
            lbd4.Text = "-" + (RGB_TorcoresNegativos[3, 0] * 100).ToString("n3");
            lbd5.Text = "-" + (RGB_TorcoresNegativos[4, 0] * 100).ToString("n3");
            lbd6.Text = "-" + (RGB_TorcoresNegativos[5, 0] * 100).ToString("n3");

            lbd7.Text = (RGB_TorcoresPositivos[3, 0] * 100).ToString("n3");
            lbd8.Text = (RGB_TorcoresPositivos[2, 0] * 100).ToString("n3");
            lbd9.Text = (RGB_TorcoresPositivos[1, 0] * 100).ToString("n3");
            lbd10.Text = (RGB_TorcoresPositivos[0, 0] * 100).ToString("n3");

            lbNomeEsforco.Text = "Torçores (Kgf.m)";
        }
        if (iEsforcoAtual == iCortante)
        {
            lbd1.Text = "-" + (RGB_CortantesNegativos[0, 0] * 100).ToString("n3");
            lbd2.Text = "-" + (RGB_CortantesNegativos[1, 0] * 100).ToString("n3");
            lbd3.Text = "-" + (RGB_CortantesNegativos[2, 0] * 100).ToString("n3");
            lbd4.Text = "-" + (RGB_CortantesNegativos[3, 0] * 100).ToString("n3");
            lbd5.Text = "-" + (RGB_CortantesNegativos[4, 0] * 100).ToString("n3");
            lbd6.Text = "-" + (RGB_CortantesNegativos[5, 0] * 100).ToString("n3");

            lbd7.Text = (RGB_CortantesPositivos[3, 0] * 100).ToString("n3");
            lbd8.Text = (RGB_CortantesPositivos[2, 0] * 100).ToString("n3");
            lbd9.Text = (RGB_CortantesPositivos[1, 0] * 100).ToString("n3");
            lbd10.Text = (RGB_CortantesPositivos[0, 0] * 100).ToString("n3");

            lbNomeEsforco.Text = "Cortantes (Kgf)";
        }
        if (iEsforcoAtual == iDeslocamento)
        {
            lbd0.Visible = true;
            lbd0.Text = (RGB_Deslocamentos[9, 1] * 1000).ToString("n3");
            lbd1.Text = (RGB_Deslocamentos[9, 0] * 1000).ToString("n3");
            lbd2.Text = (RGB_Deslocamentos[8, 0] * 1000).ToString("n3");
            lbd3.Text = (RGB_Deslocamentos[7, 0] * 1000).ToString("n3");
            lbd4.Text = (RGB_Deslocamentos[6, 0] * 1000).ToString("n3");
            lbd5.Text = (RGB_Deslocamentos[5, 0] * 1000).ToString("n3");
            lbd6.Text = (RGB_Deslocamentos[4, 0] * 1000).ToString("n3");
            lbd7.Text = (RGB_Deslocamentos[3, 0] * 1000).ToString("n3");
            lbd8.Text = (RGB_Deslocamentos[2, 0] * 1000).ToString("n3");
            lbd9.Text = (RGB_Deslocamentos[1, 0] * 1000).ToString("n3");
            lbd10.Text = (RGB_Deslocamentos[0, 0] * 1000).ToString("n3");

            lbNomeEsforco.Text = "Deslocamentos (mm)";
        }
    }
    catch( Exception e)
    {
        MessageBox.Show(e.Message);
        this.Close();
    }
}

void CriaFormCores()
{
    PanelCores.Visible = true;
}

void CriaVetores()
{
    MomentosNegativos         = new double[Pavimento.Ngl * 2];
    MomentosPositivos         = new double[Pavimento.Ngl * 2];
    MomentosTorcoresPositivos = new double[Pavimento.Ngl * 2];
    MomentosTorcoresNegativos = new double[Pavimento.Ngl * 2];
    CortantesPositivos        = new double[Pavimento.Ngl * 2];
    CortantesNegativos        = new double[Pavimento.Ngl * 2];
    Deslocamentos             = new double[Pavimento.Ngl/3];

    ListaRGB_EsforcosNegativos = new double[6,3];  //6 cores diferentes - 3 são os tons r g b
    ListaRGB_EsforcosPositivos = new double[4,3];  //4 cores diferentes - 3 são os tons r g b

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

void OrdenaEsforcos(string NomeEsforco, ref double[] EsforcoPositivo, ref double[] EsforcoNegativo, ref int iTotalNegativo, ref int iTotalPositivo)
{
    double temp; 
            
    iTotalNegativo  = 0;
    iTotalPositivo  = 0;
           
    vec3[] EsforcoCoords = new vec3[4];
 
    for (i = 1; i <= Pavimento.nBarras; i++)
    {
        if (!gerenciador.ConfiguracoesPGi.GrelhaOpcoesVisualizacao.Diagramas_DirecaoX &&
             Pavimento.barras[i].direcaoX) continue;

        if (!gerenciador.ConfiguracoesPGi.GrelhaOpcoesVisualizacao.Diagramas_DirecaoY &&
             Pavimento.barras[i].direcaoY) continue;

        if (gerenciador.ConfiguracoesPGi.GrelhaOpcoesVisualizacao.Diagramas_MostrarBarrasViga &&
           !gerenciador.ConfiguracoesPGi.GrelhaOpcoesVisualizacao.Diagramas_MostrarBarrasLaje && !Pavimento.barras[i].barraViga) continue;

        if (!gerenciador.ConfiguracoesPGi.GrelhaOpcoesVisualizacao.Diagramas_MostrarBarrasViga &&
             gerenciador.ConfiguracoesPGi.GrelhaOpcoesVisualizacao.Diagramas_MostrarBarrasLaje && Pavimento.barras[i].barraViga) continue;

        if ( !gerenciador.ConfiguracoesPGi.GrelhaOpcoesVisualizacao.Diagramas_MostrarBarrasViga &&
             !gerenciador.ConfiguracoesPGi.GrelhaOpcoesVisualizacao.Diagramas_MostrarBarrasLaje) continue;

        if (NomeEsforco == "MOMENTO FLETOR")
        {
            EsforcoCoords = Pavimento.barras[i].CoordsFletor;

            if (EsforcoCoords[1].z < 0)
              EsforcoPositivo[iTotalPositivo++] = Math.Abs(EsforcoCoords[1].z);
            else
              EsforcoNegativo[iTotalNegativo++] = EsforcoCoords[1].z;

            if (EsforcoCoords[2].z < 0)
              EsforcoPositivo[iTotalPositivo++] = Math.Abs(EsforcoCoords[2].z);
            else
              EsforcoNegativo[iTotalNegativo++] = EsforcoCoords[2].z;
        }
        else
        {

            //ESF CORTANTE E TORÇOR É AO CONTRÁRIO DO FLETOR : POSITIVO EM CIMA E NEGATIVO EMBAIXO
            if (NomeEsforco == "MOMENTO TORCOR")
                EsforcoCoords = Pavimento.barras[i].CoordsTorcor;
            else
            if (NomeEsforco == "ESFORCO CORTANTE")
                EsforcoCoords = Pavimento.barras[i].CoordsCortante;

            if (EsforcoCoords[1].z > 0)
                EsforcoPositivo[iTotalPositivo++] = EsforcoCoords[1].z;
            else
                EsforcoNegativo[iTotalNegativo++] = Math.Abs(EsforcoCoords[1].z);

            if (EsforcoCoords[2].z > 0)
                EsforcoPositivo[iTotalPositivo++] = EsforcoCoords[2].z;
            else
                EsforcoNegativo[iTotalNegativo++] = Math.Abs(EsforcoCoords[2].z);
        }
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

    for (i = 1; i <= Pavimento.nNos; i++)
      Deslocamentos[iDeslocamentos++] = Pavimento.nos[i].Deslocamento[3];
      
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
           
void AtribuiCoresEsforcos(string NomeEsforco, ref double[] Esforco, ref double[,] RGB, ref double[,] ListaRGB_Esforco, int iTotalEsforcos, int iTotalCores)
{
    RGB = new double[iTotalCores, 5];

    double max       = Esforco[0];
    double min       = Esforco[iTotalEsforcos - 1];
    double dif       = max - min;
    double intervalo = dif / iTotalCores;

    for (i = 0; i < iTotalCores; i++)
    {
       //Intervalo
       RGB[i, 0] = max - (intervalo * i);
       RGB[i, 1] = max - (intervalo * (i+1));
               
       //Cores
       RGB[i, 2] = ListaRGB_Esforco[i, 0];
       RGB[i, 3] = ListaRGB_Esforco[i, 1];
       RGB[i, 4] = ListaRGB_Esforco[i, 2];
    }

    for (i = 1; i <= Pavimento.nBarras; i++)
    {
        if (NomeEsforco == "FLETOR NEGATIVO")
        {
            Pavimento.barras[i].RGB_FletoresNegativos = new double[iTotalCores, 5];
            Pavimento.barras[i].RGB_FletoresNegativos = RGB;
         }
        else
        if (NomeEsforco == "FLETOR POSITIVO")
        {
            Pavimento.barras[i].RGB_FletoresPositivos = new double[iTotalCores, 5];
            Pavimento.barras[i].RGB_FletoresPositivos = RGB;
        }
        if (NomeEsforco == "TORCOR NEGATIVO")
        {
            Pavimento.barras[i].RGB_TorcoresNegativos = new double[iTotalCores, 5];
            Pavimento.barras[i].RGB_TorcoresNegativos = RGB;
        }
        else
        if (NomeEsforco == "TORCOR POSITIVO")
        {
            Pavimento.barras[i].RGB_TorcoresPositivos = new double[iTotalCores, 5];
            Pavimento.barras[i].RGB_TorcoresPositivos = RGB;
        }
        if (NomeEsforco == "CORTANTE NEGATIVO")
        {
            Pavimento.barras[i].RGB_CortantesNegativos = new double[iTotalCores, 5];
            Pavimento.barras[i].RGB_CortantesNegativos = RGB;
        }
        else
        if (NomeEsforco == "CORTANTE POSITIVO")
        {
            Pavimento.barras[i].RGB_CortantesPositivos = new double[iTotalCores, 5];
            Pavimento.barras[i].RGB_CortantesPositivos = RGB;
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

    for (i = 1; i <= Pavimento.nBarras; i++)
    {
        Pavimento.barras[i].RGB_Deslocamentos = new double[10, 5];
        Pavimento.barras[i].RGB_Deslocamentos = this.RGB_Deslocamentos;
    }
}

private void VisualizadorPortico_Load(object sender, EventArgs e)
{
    estavaFechado = true;
    InicializarOGL();

    /*Glut.glutInit(); // nao precisa inicializar aqui, pois ja foi inicializada no form Desenho
             
    panel1.Left = (this.Width / 2) - (panel1.Width / 2);
    panel1.Top = (this.Height / 2) - (panel1.Height / 2);*/
}

        public bool estavaFechado = true;
        private void FVisualizadorGrelha_Activated(object sender, EventArgs e)
        {
            if (PavAntes != gerenciador.cbPiso.SelectedIndex && !estavaFechado)
                gerenciador.cbPiso.SelectedIndex = PavAntes;

                //Pavimento = gerenciador.Pavimentos[PavAntes];


            estavaFechado = false;
          /*  if (gerenciador.recalculou)
            {
                Atualizar();

                estavaFechado          = false;
                gerenciador.recalculou = false;
            }*/
        }
        public void InicializarOGL()
        {
        /*    GL.Hint(Gl.GL_PERSPECTIVE_CORRECTION_HINT, Gl.GL_NICEST);
            //glHint(GL_LINE_SMOOTH_HINT, GL_NICEST);
            //     GL.Hint(GL_POLYGON_SMOOTH_HINT, GL_NICEST);
            GL.Enable(Gl.GL_DEPTH_TEST);
            //   GL.Enable(Gl.GL_LIGHTING);
            //   GL.Enable(Gl.GL_TEXTURE_2D);
            //    GL.Enable(Gl.GL_CULL_FACE);

            // track material ambient and diffuse from surface color, call it before glEnable(GL_COLOR_MATERIAL)
            GL.ColorMaterial(Gl.GL_FRONT_AND_BACK, Gl.GL_AMBIENT_AND_DIFFUSE);
            GL.Enable(Gl.GL_COLOR_MATERIAL);

            GL.ClearColor(0, 0, 0, 0);                   // background color
            GL.ClearStencil(0);                          // clear stencil buffer
            GL.ClearDepth(1.0f);                         // 0 is near, 1 is far
            GL.DepthFunc(Gl.GL_LEQUAL);

            float[] lightKa = { .2f, .2f, .2f, 1.0f };  // ambient light
            float[] lightKd = { .7f, .7f, .7f, 1.0f };  // diffuse light
            float[] lightKs = { 1, 1, 1, 1 };           // specular light
            GL.Lightfv(Gl.GL_LIGHT0, Gl.GL_AMBIENT, lightKa);
            GL.Lightfv(Gl.GL_LIGHT0, Gl.GL_DIFFUSE, lightKd);
            GL.Lightfv(Gl.GL_LIGHT0, Gl.GL_SPECULAR, lightKs);

            // position the light
            float[] lightPos = { 0, 0, 20, 1 }; // positional light
            GL.Lightfv(Gl.GL_LIGHT0, Gl.GL_POSITION, lightPos);*/
        }
  
        public void Atualizar()
        {
            InicializarOGL(); 
            CarregaPavimentoSelecionado();
            GerarModelo3D(true);
          //  Enquadrar();
        }

        private void FVisualizadorGrelha_Resize(object sender, EventArgs e)
        {
         //   SetupViewPort();
            ogl.ReShapeOGL(Controle.Width, Controle.Height); 
            CarregaPavimentoSelecionado();
            Enquadrar();
            // Atualizar();
        }
        public bool Isovalor;
        public int[] ViewPort = new int[4];
        public static double OGLleft = .0, OGLright = .0, OGLtop = .0, OGLbotton = .0;
        public static double OGLzNear = -1, OGLzFar = 1;
        public void Render()
        {
            try
            {
               // TOpenGl.ReShapeOGL(Controle.Width, Controle.Height);
                OGLtop = 1;
                OGLbotton = -1;
                OGLleft = -(double)(Controle.Width) / (double)(Controle.Height);
                OGLright = -OGLleft;
                
               // GL.PushMatrix();
             //   GL.LoadIdentity();
                
             //   GL.MatrixMode(Gl.GL_PROJECTION); // Tell opengl that we are doing project matrix work
               /* GL.Disable(Gl.GL_DEPTH_TEST);
                GL.LoadIdentity(); // Clear the matrix
               */
                GL.Clear(ClearBufferMask.ColorBufferBit | ClearBufferMask.DepthBufferBit);
               // GL.ClearColor(0, 0, 0, 0);    // This Will Clear The Background Color To Black
  /*
                GL.Ortho(OGLleft, OGLright, -1, 1, -10000, 1000);
                //      GL.PopMatrix();
                GL.MatrixMode(Gl.GL_MODELVIEW); // Tell opengl that we are doing model matrix work. (drawing)
                GL.LoadIdentity(); // Clear the model matrix
                
                GL.Color3f(0, 1, 1);
                GL.Begin(Gl.GL_LINES);
                GL.Vertex3d(0, 0, 0);
                GL.Vertex3d(0-centroX, 0-centroY,0);
                //  GL.Vertex2d(px_x_i, -px_y_i);
                GL.End();*/

                for (i = 1; i <= Pavimento.nBarras; i++)
                {
                    if (Pavimento.barras[i].barraViga)
                        GL.LineWidth(2);
                    else
                        GL.LineWidth(1);

                    GL.Begin(PrimitiveType.Lines);
                    if ((gerenciador.ConfiguracoesPGi.GrelhaOpcoesVisualizacao.BarrasLajeVisiveis && !Pavimento.barras[i].barraViga) || (Pavimento.barras[i].barraViga))
                        Pavimento.barras[i].Desenha3D(centroX,
                                                       centroY,
                                                       FatorEscalaDeslocamento * (double)iEscalaDiagrama,
                                                       gerenciador.ConfiguracoesPGi.GrelhaOpcoesVisualizacao.Diagramas_DeslocamentoGradiente /*&& Isovalor == false*/,
                                                       iEsforcoAtual == iDeslocamento/* && Isovalor == false*/);
                    GL.End();
                }

                //  Pavimento.DesenhaCelulas(centroX, centroY, FatorEscalaDeslocamento * (double)iEscalaDiagrama);

                GL.LineWidth(1);

                if (!Isovalor)
                {
                    if (iEsforcoAtual != iDeslocamento)
                    {
                        GL.Color3(.1, .9, .8);
                        for (i = 1; i <= Pavimento.nBarras; i++)
                        {
                            Pavimento.barras[i].DesenhaDiagramas(iEsforcoAtual,
                                                               centroX,
                                                               centroY,
                                                               FatorEscalaEsforco[iEsforcoAtual] * iEscalaDiagrama,
                                                               gerenciador.ConfiguracoesPGi.GrelhaOpcoesVisualizacao.Diagramas_Gradiente,
                                                               gerenciador.ConfiguracoesPGi.GrelhaOpcoesVisualizacao.Diagramas_DuasCores,
                                                               true,
                                                               false,
                                                               gerenciador.ConfiguracoesPGi.GrelhaOpcoesVisualizacao.Diagramas_MostrarBarrasLaje,
                                                               gerenciador.ConfiguracoesPGi.GrelhaOpcoesVisualizacao.Diagramas_MostrarBarrasViga,
                                                               gerenciador.ConfiguracoesPGi.GrelhaOpcoesVisualizacao.Diagramas_MostrarBarrasRigidas,
                                                               gerenciador.ConfiguracoesPGi.GrelhaOpcoesVisualizacao.Diagramas_DirecaoX,
                                                               gerenciador.ConfiguracoesPGi.GrelhaOpcoesVisualizacao.Diagramas_DirecaoY);
                        }
                    }
                }
                else
                    DesenhaIsovalores();

                #region Molas
                /* Desenha as molas*/
                for (i = 1; i <= Pavimento.nNos; i++)
                {
                    ultZ = 0;
                    ultX = 0;
                    ultY = 0;

                    GL.Color3(50, 100, 0);

                    if (Pavimento.nos[i].vinculo == 3)
                    {
                        xNo = Pavimento.nos[i].x - (centroX);
                        yNo = Pavimento.nos[i].y - (centroY);

                        /*      Mola de translação  */
                        GL.Begin(PrimitiveType.LineStrip);
                        GL.Vertex3(xNo, yNo, 0);
                        GL.Vertex3(xNo, yNo, -1);

                        for (k = 0; k < 2; k++)
                        {
                            for (j = 0; j <= 10; j++)
                            {
                                GL.Vertex3((xNo + (0.2 * Math.Cos(j * twicePi / 10))),
                                              (yNo + (0.2 * Math.Sin(j * twicePi / 10))), ultZ - 1);
                                ultZ = ultZ - .05;
                            };
                            ultZ += .05;
                        }
                        GL.End();

                        /*      Mola de rotação em x  */
                        GL.Begin(PrimitiveType.LineStrip);
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
                        GL.Begin(PrimitiveType.LineStrip);
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
                        GL.End();
                        /* ----------------------- */

                    }
                }
                #endregion
             //   GL.PopMatrix();
              //  GL.Flush();
               /* if (checkBox1.Checked)
                {
                    int sx = 0, sy = 0, ex = 350, ey = 89;
                    g2d.Project(ref px_x_i, ref px_y_i, posX - centroX, posY - centroY, posZ);
                    // g2d.Project(ref px_x_f, ref px_y_f, linha.pFin.x - centroX, linha.pFin.y - centroY, 0);
                    //     g2d.UnProject(e.X, e.Y, ref posX, ref posY, ref posZ);                            

                    //Set ortho view
                   // GL.PushMatrix();
                    GL.glMatrixMode(Gl.GL_PROJECTION); // Tell opengl that we are doing project matrix work
                    GL.glDisable(Gl.GL_DEPTH_TEST);
                    GL.glLoadIdentity(); // Clear the matrix
                    GL.glOrtho(0, Controle.Width, Controle.Height, 0, -1, 1);
                    
                    GL.MatrixMode(Gl.GL_MODELVIEW); // Tell opengl that we are doing model matrix work. (drawing)
                    GL.LoadIdentity(); // Clear the model matrix

//Draw_2d();


                    GL.Color3f(1, 0, 0);
                    GL.Begin(Gl.GL_LINES);
                    GL.Vertex3d(0,0, 0);
                    GL.Vertex2d(TOpenGl.MouseX, TOpenGl.MouseY);
                  //  GL.Vertex2d(px_x_i, -px_y_i);
                    GL.End();
                  //  GL.PopMatrix();
                }*/
                Controle.SwapBuffers();
            }
            catch (Exception e)
            {
                MessageBox.Show(e.Message);
                this.Close();
            }
        }

       /*/ public static float pixelX(double coordX)
        {
            return (float)(coordX / precisaoPixel + ponto_zero[0]);
        }

        public static float pixelY(double coordY)
        {
            return (float)(coordY / -precisaoPixel + ponto_zero[1]);
        }*/

        void DesenhaIsovalores()
        {
            if (iEsforcoAtual == iDeslocamento)
            {
                for (i = 0; i < Pavimento.LinhasIsoDeslocamento.Count; i++ )
                {
                    if (i == Pavimento.LinhasIsoDeslocamento.Count-1)
                        GL.Color3(255, 0, 0);
                    else
                        GL.Color3((byte)RGB_Deslocamentos[i, 2], (byte)RGB_Deslocamentos[i, 3], (byte)RGB_Deslocamentos[i, 4]);

                    foreach (Linha linha in Pavimento.LinhasIsoDeslocamento[i].linhas)
                    {
                        GL.Begin(PrimitiveType.Lines);
                        GL.Vertex3(linha.pIni.x - centroX, linha.pIni.y - centroY, 0);
                        GL.Vertex3(linha.pFin.x - centroX, linha.pFin.y - centroY, 0);
                        GL.End();
                    }
                }
            }
            else
            if (iEsforcoAtual == iFletor)
            {
                for (i = 0; i < Pavimento.LinhasIsoFletor.Count; i++)
                {
                    if (i == Pavimento.LinhasIsoFletor.Count - 1)
                        GL.Color3(255, 0, 0);
                    else
                        GL.Color3((byte)RGB_Deslocamentos[i, 2], (byte)RGB_Deslocamentos[i, 3], (byte)RGB_Deslocamentos[i, 4]);

                    foreach (Linha linha in Pavimento.LinhasIsoFletor[i].linhas)
                    {
                        GL.Begin(PrimitiveType.Lines);
                        GL.Vertex3(linha.pIni.x - centroX, linha.pIni.y - centroY, 0);
                        GL.Vertex3(linha.pFin.x - centroX, linha.pFin.y - centroY, 0);
                        GL.End();
                    }
                }
            }
        }

        double s;
        private void ApplyZoom(int mousex, int mousey, int delta)
        {
            s = System.Math.Exp((double)delta * 0.0014);

            GL.Translate((float)posX, (float)posY, (float)posZ);
            GL.Scale((float)s, (float)s, (float)s);
            GL.Translate(-(float)posX, -(float)posY, -(float)posZ);

            Render();
           // TOpenGl.OGL.Refresh();
        }

        public void Controle_MouseWheel(object sender, System.Windows.Forms.MouseEventArgs e)
        {
            ApplyZoom(e.X, e.Y, e.Delta);
        }

        public void SetupViewPort()
        {
            w = Controle.Width;
            h = Controle.Height;

            double left = -(double)(w) / (double)h;
            double rigth = -left;

            GL.Viewport(0, 0, w, h);
        //    GL.MatrixMode(Gl.GL_PROJECTION);
            GL.LoadIdentity();
            
            GL.ClearColor(0,0,0, 0);

            GL.Ortho(left, rigth, -1, 1, -1, 100);
           // GL.MatrixMode(Gl.GL_MODELVIEW);                                
            GL.LoadIdentity();
            Enquadrar();
        }

        private void Controle_Resize(object sender, EventArgs e)
        {
            
        }

        private void Controle_SizeChanged(object sender, EventArgs e)
        {
         //   TOpenGl.ReShapeOGL();
         //   Render();
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

            for (i = 1; i <= Pavimento.nNos; i++)
            {
                if (!Pavimento.nos[i].Enquadrar) continue;

                if (Pavimento.nos[i].px_x < p_xini)
                {
                    xIni = Pavimento.nos[i].x;
                    p_xini = Pavimento.nos[i].px_x;
                    p_yini = Pavimento.nos[i].px_y;        
                };
            };
        }

        public void Enquadrar()
        {

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

            _dragPosX = 0;
            _dragPosY = 0;
            _dragPosZ = 0;

            int x = (int)p_xini, y = (int)p_yini;
            double s = System.Math.Exp((double)-120 * 0.001);
 
            for (int i = 0; i < 65; i++)
            {
                g2d.UnProject(580, 286, ref posX, ref posY, ref posZ);

                GL.Translate((float)posX, (float)posY, (float)posZ);
                GL.Scale((float)s, (float)s, (float)s);
                GL.Translate(-(float)posX, -(float)posY, -(float)posZ);
                
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
            //    Render();

                GetMiniMaxPt(ref p_xini, ref p_xfin, ref p_yini, ref p_yfin,
                             ref xIni, ref yIni, ref xFin, ref yFin);

                if (p_xini < 100 && p_xini > 0 && i > 0)
                    break;
             //   Controle.SwapBuffers();
            }
        }
        double clicx, clicy, clicz;
        private void Controle_MouseDown(object sender, System.Windows.Forms.MouseEventArgs e)
        {
            ultx = e.X;
            ulty = e.Y;
            clicx = posX;
            clicy = posY;
            clicz = posZ;
     //       TOpenGl.OGL.Refresh();
        }
        int dx, dy, dz, x, y;
        double angle, ax, ay, az;
        double[] _MatrixInverse = new double[16];
        double x_rot_angle, y_rot_angle, x_trans, y_trans;
        private void Controle_MouseMove(object sender, System.Windows.Forms.MouseEventArgs e)
        {
          /*  if (gerenciador.ActiveMdiChild != this) return;
            double px, py, pz;
            x = e.X;
            y = e.Y;

            dx = x - ogl.MouseX;
            dy = y - ogl.MouseY;
            dz = (int)(posZ - zant);

            ogl.UpdateOGLMatrix();
            ogl.pos(&px, &py, &pz, x, y, ogl.ViewPort);

            angle = 0;

            if (e.Button == System.Windows.Forms.MouseButtons.Middle)
            {
                //GL.MatrixMode(GL.MatrixMode.ModelView);
                GL.LoadIdentity();

                GL.Translate((float)px - _dragPosX, (float)py - _dragPosY, (float)pz - _dragPosZ);

                //GL.MultMatrixd(TOpenGl.ModelViewMatrix);
            }
            else if (e.Button == System.Windows.Forms.MouseButtons.Left)
            {
                //if (!Isovalor)
                {                   
                    ogl.InvertMatrixd(ogl.ModelViewMatrix, _MatrixInverse);

                    ax = dy;
                    ay = dx;
                    az = dy;
                    angle = ogl.vlen(ax, ay, az) / (double)(ogl.ViewPort[2] + 1) * 360.0;

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

                    x_rot_angle += ((e.Y - start_y) * .5);
                    y_rot_angle += ((e.X - start_x) * .5);

                    start_x = e.X;
                    start_y = e.Y;
                }
            }
            else
            {
                ogl.MouseX = x;
                ogl.MouseY = y;

                if (iEsforcoAtual == iFletor)
                {
                   GetFletorNo();
                   if (Isovalor)
                      GetIsoFletorBarra();
                }
                else
                if (iEsforcoAtual == iTorcor)
                  GetTorcorNo();
                else
                if (iEsforcoAtual == iCortante)
                  GetCortanteNo();
                else
                if (iEsforcoAtual == iDeslocamento)
                {
                   GetDeslocamentoNo(3);
                   
                   if (Isovalor)
                      GetIsoDeslocamentoBarra();
                }
                GetInfoBarra();
                ultx = e.X;
                ulty = e.Y;
            }
            start_x = e.X;
            start_y = e.Y; 

            ogl.MouseX = x;
            ogl.MouseY = y;

            _dragPosX = (float)px;
            _dragPosY = (float)py;
            _dragPosZ = (float)pz;
 
            Render();
          //  TOpenGl.OGL.Refresh();

           this.Text = e.X.ToString() + "  " + e.Y.ToString();
            zant = posZ;

            g2d.UnProject(e.X,e.Y,ref posX, ref posY, ref posZ);      */                      
         }

        int i, j, k;
        private void GetDeslocamentoNo(int Deslocamento)
        {
            lbInfoNo.Visible = false;
            for (i = 1; i <= Pavimento.nNos; i++)
            {
                if ((ogl.MouseX >= (Pavimento.nos[i].px_x - 4)) && (ogl.MouseX <= (Pavimento.nos[i].px_x + 4)))
                {
                    if ((ogl.MouseY >= (Pavimento.nos[i].px_y - 4)) && (ogl.MouseY <= (Pavimento.nos[i].px_y + 4)))
                    {
                        lbInfoNo.Visible = true;
                        lbInfoNo.Left = ogl.MouseX+10;
                        lbInfoNo.Top  = ogl.MouseY-10;
                        lbInfoNo.Text = ((Pavimento.nos[i].Deslocamento[Deslocamento]*1000)).ToString("n3") + " (mm)";
                        lbInfoNo.Refresh();
                    }
                }
            }
        }
        double px_x_i, px_y_i;
        double px_x_f, px_y_f;
        Label lb;
        public void HabilitaIsovalores(bool habilita)
        {

              /*  lbIso1.Visible = habilita;
                lbIso2.Visible = habilita;
                lbIso3.Visible = habilita;
                lbIso4.Visible = habilita;
                lbIso5.Visible = habilita;
                lbIso6.Visible = habilita;
                lbIso7.Visible = habilita;
                lbIso8.Visible = habilita;
                lbIso9.Visible = habilita;*/

                Isovalor = habilita;
        }

        private void GetIsoDeslocamentoBarra()
        {

                lbIso1.Visible = false;

                for (i = 0; i < Pavimento.LinhasIsoDeslocamento.Count; i++)
                {
                  /* if (i == 0) lb = lbIso1;
                   if (i == 1) lb = lbIso2;
                   if (i == 2) lb = lbIso3;
                   if (i == 3) lb = lbIso4;
                   if (i == 4) lb = lbIso5;
                   if (i == 5) lb = lbIso6;
                   if (i == 6) lb = lbIso7;
                   if (i == 7) lb = lbIso8;
                   if (i == 8) lb = lbIso9;*/

                   if (i == Pavimento.LinhasIsoDeslocamento.Count - 1)
                       lbIso1.ForeColor = Color.FromArgb(255, 0, 0);
                   else
                       lbIso1.ForeColor = Color.FromArgb((byte)RGB_Deslocamentos[i, 2], (byte)RGB_Deslocamentos[i, 3], (byte)RGB_Deslocamentos[i, 4]);
                   
 //                   linha = Pavimento.LinhasIsoDeslocamento[i].linhas[i];

                    foreach (Linha linha in Pavimento.LinhasIsoDeslocamento[i].linhas)
                    {

                        g2d.Project(ref px_x_i, ref px_y_i, linha.pIni.x - centroX, linha.pIni.y - centroY, 0);
                        g2d.Project(ref px_x_f, ref px_y_f, linha.pFin.x - centroX, linha.pFin.y - centroY, 0);

                        if (Geom.PontoEmLinha2(ogl.MouseX, ogl.MouseY, px_x_i, px_y_i, px_x_f, px_y_f, 5))
                        {
                            lbIso1.Visible = true;
                            lbIso1.Left = (int)ogl.MouseX + 20;
                            lbIso1.Top = (int)ogl.MouseY - 10;
                            lbIso1.Text    = (Pavimento.LinhasIsoDeslocamento[i].valor*100).ToString("n3");
                            lbIso1.Refresh();
                        }
                    }
                  
                }
        }
   //     Linha linha;
        private void GetIsoFletorBarra()
        {

            lbIso1.Visible = false;

            for (i = 0; i < Pavimento.LinhasIsoFletor.Count; i++)
            {
                /* if (i == 0) lb = lbIso1;
                 if (i == 1) lb = lbIso2;
                 if (i == 2) lb = lbIso3;
                 if (i == 3) lb = lbIso4;
                 if (i == 4) lb = lbIso5;
                 if (i == 5) lb = lbIso6;
                 if (i == 6) lb = lbIso7;
                 if (i == 7) lb = lbIso8;
                 if (i == 8) lb = lbIso9;*/

                if (i == Pavimento.LinhasIsoFletor.Count - 1)
                    lbIso1.ForeColor = Color.FromArgb(255, 0, 0);
                else
                    lbIso1.ForeColor = Color.FromArgb((byte)RGB_Deslocamentos[i, 2], (byte)RGB_Deslocamentos[i, 3], (byte)RGB_Deslocamentos[i, 4]);

                //                   linha = Pavimento.LinhasIsoDeslocamento[i].linhas[i];

                foreach (Linha linha in Pavimento.LinhasIsoFletor[i].linhas)
                {

                    g2d.Project(ref px_x_i, ref px_y_i, linha.pIni.x - centroX, linha.pIni.y - centroY, 0);
                    g2d.Project(ref px_x_f, ref px_y_f, linha.pFin.x - centroX, linha.pFin.y - centroY, 0);

                    if (Geom.PontoEmLinha2(ogl.MouseX, ogl.MouseY, px_x_i, px_y_i, px_x_f, px_y_f, 5))
                    {
                        lbIso1.Visible = true;
                        lbIso1.Left = (int)ogl.MouseX + 20;
                        lbIso1.Top = (int)ogl.MouseY - 10;
                        lbIso1.Text = Pavimento.LinhasIsoFletor[i].valor.ToString("n3");
                        lbIso1.Refresh();
                    }
                }

            }
        }
        
        private void GetFletorNo()
        {
            lbInfoNo.Visible = false;

            for (i = 1; i <= Pavimento.nBarras; i++)
            {
                if (!gerenciador.ConfiguracoesPGi.GrelhaOpcoesVisualizacao.Diagramas_DirecaoX &&
                     Pavimento.barras[i].direcaoX) continue;

                if (!gerenciador.ConfiguracoesPGi.GrelhaOpcoesVisualizacao.Diagramas_DirecaoY &&
                     Pavimento.barras[i].direcaoY) continue;

                if (gerenciador.ConfiguracoesPGi.GrelhaOpcoesVisualizacao.Diagramas_MostrarBarrasViga &&
                   !gerenciador.ConfiguracoesPGi.GrelhaOpcoesVisualizacao.Diagramas_MostrarBarrasLaje && !Pavimento.barras[i].barraViga) continue;

                if (!gerenciador.ConfiguracoesPGi.GrelhaOpcoesVisualizacao.Diagramas_MostrarBarrasViga &&
                     gerenciador.ConfiguracoesPGi.GrelhaOpcoesVisualizacao.Diagramas_MostrarBarrasLaje && Pavimento.barras[i].barraViga) continue;

                if (!gerenciador.ConfiguracoesPGi.GrelhaOpcoesVisualizacao.Diagramas_MostrarBarrasViga &&
                     !gerenciador.ConfiguracoesPGi.GrelhaOpcoesVisualizacao.Diagramas_MostrarBarrasLaje) continue; 
                
                for (j = 1; j <= 2; j++)
                {
                   if ((ogl.MouseX >= (Pavimento.barras[i].CoordsFletor[j].px_x - 9)) && (ogl.MouseX <= (Pavimento.barras[i].CoordsFletor[j].px_x + 9)))
                   {
                      if ((ogl.MouseY >= (Pavimento.barras[i].CoordsFletor[j].px_y - 9)) && (ogl.MouseY <= (Pavimento.barras[i].CoordsFletor[j].px_y + 9)))
                      {
                         lbInfoNo.Visible = true;
                         lbInfoNo.Left = ogl.MouseX + 10;
                         lbInfoNo.Top = ogl.MouseY - 10;
                         lbInfoNo.Text = (Math.Abs(Pavimento.barras[i].CoordsFletor[j].z) * .1).ToString("n3") + " tf.m";
                         lbInfoNo.Refresh();
                      }
                   }
                }
            }
        }
        private void GetTorcorNo()
        {
            lbInfoNo.Visible = false;
            
            for (i = 1; i <= Pavimento.nBarras; i++)
            {
                if (!gerenciador.ConfiguracoesPGi.GrelhaOpcoesVisualizacao.Diagramas_DirecaoX &&
                     Pavimento.barras[i].direcaoX) continue;

                if (!gerenciador.ConfiguracoesPGi.GrelhaOpcoesVisualizacao.Diagramas_DirecaoY &&
                     Pavimento.barras[i].direcaoY) continue;

                if (gerenciador.ConfiguracoesPGi.GrelhaOpcoesVisualizacao.Diagramas_MostrarBarrasViga &&
                   !gerenciador.ConfiguracoesPGi.GrelhaOpcoesVisualizacao.Diagramas_MostrarBarrasLaje && !Pavimento.barras[i].barraViga) continue;

                if (!gerenciador.ConfiguracoesPGi.GrelhaOpcoesVisualizacao.Diagramas_MostrarBarrasViga &&
                     gerenciador.ConfiguracoesPGi.GrelhaOpcoesVisualizacao.Diagramas_MostrarBarrasLaje && Pavimento.barras[i].barraViga) continue;

                if (!gerenciador.ConfiguracoesPGi.GrelhaOpcoesVisualizacao.Diagramas_MostrarBarrasViga &&
                     !gerenciador.ConfiguracoesPGi.GrelhaOpcoesVisualizacao.Diagramas_MostrarBarrasLaje) continue; 
                
                for (j = 1; j <= 2; j++)
                {
                    if ((ogl.MouseX >= (Pavimento.barras[i].CoordsTorcor[j].px_x - 9)) && (ogl.MouseX <= (Pavimento.barras[i].CoordsTorcor[j].px_x + 9)))
                    {
                        if ((ogl.MouseY >= (Pavimento.barras[i].CoordsTorcor[j].px_y - 9)) && (ogl.MouseY <= (Pavimento.barras[i].CoordsTorcor[j].px_y + 9)))
                        {
                            lbInfoNo.Visible = true;
                            lbInfoNo.Left = ogl.MouseX + 10;
                            lbInfoNo.Top = ogl.MouseY - 10;
                            lbInfoNo.Text = (Pavimento.barras[i].CoordsTorcor[j].z*.1).ToString("n3") + " tf.m";
                            lbInfoNo.Refresh();
                        }
                    }
                }
            }
        }

        private void GetCortanteNo()
        {
            lbInfoNo.Visible = false;

            for (i = 1; i <= Pavimento.nBarras; i++)
            {
                for (j = 1; j <= 2; j++)
                {
                    if ((ogl.MouseX >= (Pavimento.barras[i].CoordsCortante[j].px_x - 9)) && (ogl.MouseX <= (Pavimento.barras[i].CoordsCortante[j].px_x + 9)))
                    {
                        if ((ogl.MouseY >= (Pavimento.barras[i].CoordsCortante[j].px_y - 9)) && (ogl.MouseY <= (Pavimento.barras[i].CoordsCortante[j].px_y + 9)))
                        {
                            lbInfoNo.Visible = true;
                            lbInfoNo.Left = ogl.MouseX + 10;
                            lbInfoNo.Top = ogl.MouseY - 10;
                            lbInfoNo.Text = (Pavimento.barras[i].CoordsCortante[j].z * .1).ToString("n3") + " tf";
                            lbInfoNo.Refresh();
                        }
                    }
                }
            }
        }
        
        private void numericUpDown1_ValueChanged(object sender, EventArgs e)
        {
            Render();
            Controle.SwapBuffers();
        }

        private void VisualizadorGrelha_Shown(object sender, EventArgs e)
        {
            ogl.ReShapeOGL(Controle.Width, Controle.Height);
            GerarModelo3D(true);
 
         //   gerenciador.holderEsforcoEspacialCombinacao.Left = this.Width / 2;
      //      gerenciador.holderEsforcoEspacialCombinacao.Top = this.Height / 2;

        }

        void GeraIsovalores()
        {
            gerenciador.ChamaAguardar(this, "Gerando isolinhas. Aguarde...");

            Pavimento.ValoresIsoDeslocamentos = new double[9];
            Pavimento.ValoresIsoDeslocamentos[0] = RGB_Deslocamentos[1, 0];
            Pavimento.ValoresIsoDeslocamentos[1] = RGB_Deslocamentos[2, 0];
            Pavimento.ValoresIsoDeslocamentos[2] = RGB_Deslocamentos[3, 0];
            Pavimento.ValoresIsoDeslocamentos[3] = RGB_Deslocamentos[4, 0];
            Pavimento.ValoresIsoDeslocamentos[4] = RGB_Deslocamentos[5, 0];
            Pavimento.ValoresIsoDeslocamentos[5] = RGB_Deslocamentos[6, 0];
            Pavimento.ValoresIsoDeslocamentos[6] = RGB_Deslocamentos[7, 0];
            Pavimento.ValoresIsoDeslocamentos[7] = RGB_Deslocamentos[8, 0];
            Pavimento.ValoresIsoDeslocamentos[8] = RGB_Deslocamentos[9, 0];

            if (RGB_FletoresPositivos != null)
            {
                Pavimento.ValoresIsoFletores = new double[9];
                Pavimento.ValoresIsoFletores[0] = RGB_FletoresPositivos[1, 0];
                Pavimento.ValoresIsoFletores[1] = RGB_FletoresPositivos[2, 0];
                Pavimento.ValoresIsoFletores[2] = RGB_FletoresPositivos[3, 0];

                Pavimento.ValoresIsoFletores[3] = RGB_FletoresNegativos[5, 0];
                Pavimento.ValoresIsoFletores[4] = RGB_FletoresNegativos[4, 0];
                Pavimento.ValoresIsoFletores[5] = RGB_FletoresNegativos[3, 0];
                Pavimento.ValoresIsoFletores[6] = RGB_FletoresNegativos[2, 0];
                Pavimento.ValoresIsoFletores[7] = RGB_FletoresNegativos[1, 0];
                Pavimento.ValoresIsoFletores[8] = RGB_FletoresNegativos[0, 0];
                Pavimento.GeraIsovaloresFletor();

            }
            Pavimento.GeraIsovaloresDeslocamentos();

            gerenciador.FechaAguardar();
        }

        public void GerarModelo3D(bool enquadrar)
        {
            AtualizaEsforcos("Gerando visualização. Aguarde...");
            
            AtualizaFormCores();
            gerenciador.FechaAguardar();
    
            GeraIsovalores();

            Render();

            GL.Flush();

            if (enquadrar) 
              Enquadrar();

            Controle.SwapBuffers();
            Render();
            //TOpenGl.OGL.Refresh();
            this.Text = "Grelha - " + Pavimento.Descricao;
        }

        public void FVisualizadorGrelha_FormClosing(object sender, FormClosingEventArgs e)
        {

            Dispose();
            gerenciador.FVisGrelha = null;
            gerenciador.VoltaParaPavimento();
        }

        private void Controle_Paint(object sender, PaintEventArgs e)
        {
            Controle.MakeCurrent();           
        }

        double zz = 0;

        public void VistaCima()
        {
       /*     GL.Clear(Gl.GL_COLOR_BUFFER_BIT | Gl.GL_DEPTH_BUFFER_BIT);
            GL.MatrixMode(Gl.GL_MODELVIEW);
            GL.LoadIdentity();
            Enquadrar();*/
        }

        public void VistaPerspectiva()
        {
          /*  GL.Clear(Gl.GL_COLOR_BUFFER_BIT | Gl.GL_DEPTH_BUFFER_BIT);
            GL.MatrixMode(Gl.GL_MODELVIEW);
            GL.LoadIdentity();
            Enquadrar();
            GL.Rotated(-55, 1, 0, 0);
            GL.Rotated(45, 0, 0, 1);    */     
        }

        private void button1_Click(object sender, EventArgs e)
        {

        }

        private void FVisualizadorGrelha_GiveFeedback(object sender, GiveFeedbackEventArgs e)
        {

        }

        private void FVisualizadorGrelha_MouseDown(object sender, System.Windows.Forms.MouseEventArgs e)
        {
        }

        public int PavAntes;
        private void FVisualizadorGrelha_Deactivate(object sender, EventArgs e)
        {
            PavAntes = gerenciador.cbPiso.SelectedIndex;
        }

        private void numericUpDown4_ValueChanged(object sender, EventArgs e)
        {
          /*  GL.Clear(Gl.GL_COLOR_BUFFER_BIT | Gl.GL_DEPTH_BUFFER_BIT);
            GL.MatrixMode(Gl.GL_MODELVIEW);
            GL.LoadIdentity();
            Enquadrar();
            GL.Rotated((float)numericUpDown4.Value, 1, 0, 0);
            GL.Rotated((float)numericUpDown1.Value, 0, 1, 0);
            GL.Rotated((float)numericUpDown3.Value, 0, 0, 1);
            Render();
            Controle.SwapBuffers();/*
        }

        private void numericUpDown1_ValueChanged_1(object sender, EventArgs e)
        {
         /*   GL.Clear(Gl.GL_COLOR_BUFFER_BIT | Gl.GL_DEPTH_BUFFER_BIT);
            GL.MatrixMode(Gl.GL_MODELVIEW);
            GL.LoadIdentity();
            Enquadrar();
            GL.Rotated((float)numericUpDown4.Value, 1, 0, 0);
            GL.Rotated((float)numericUpDown1.Value, 0, 1, 0);
            GL.Rotated((float)numericUpDown3.Value, 0, 0, 1);
            Render();
            Controle.SwapBuffers();/*
        }

        private void numericUpDown3_ValueChanged(object sender, EventArgs e)
        {
           /* GL.Clear(Gl.GL_COLOR_BUFFER_BIT | Gl.GL_DEPTH_BUFFER_BIT);
            GL.MatrixMode(Gl.GL_MODELVIEW);
            GL.LoadIdentity();
            Enquadrar();
            GL.Rotated((float)numericUpDown4.Value, 1, 0, 0);
            GL.Rotated((float)numericUpDown1.Value, 0, 1, 0);
            GL.Rotated((float)numericUpDown3.Value, 0, 0, 1);
            Render();
            Controle.SwapBuffers();*/
        }
        private void GetInfoBarra()
        {
            lbInfoBarra.Visible = false;

            for (i = 1; i < Pavimento.barras.Count(); i++)
            {
                if (Pavimento.barras[i].barraViga)
                {
                  g2d.Project(ref px_x_i, ref px_y_i, Pavimento.barras[i].pIni.x - centroX, Pavimento.barras[i].pIni.y - centroY, 0);
                  g2d.Project(ref px_x_f, ref px_y_f, Pavimento.barras[i].pFin.x - centroX, Pavimento.barras[i].pFin.y - centroY, 0);

                  if (Geom.PontoEmLinha2(ogl.MouseX, ogl.MouseY, px_x_i, px_y_i,px_x_f, px_y_f, 2))
                  {
                      lbInfoBarra.Visible = true;
                      lbInfoBarra.Left = (int)ogl.MouseX - 20;
                      lbInfoBarra.Top  = (int)ogl.MouseY + 20;
                      lbInfoBarra.Text = "Barra " + Pavimento.barras[i].codBarra.ToString() + " - Viga " + Pavimento.barras[i].TrechoViga.Texto1.texto + " "+Pavimento.barras[i].TrechoViga.Texto2.texto;
                      lbInfoBarra.Refresh();
                  }
                }
            }
        }
        private void Controle_Click(object sender, EventArgs e)
        {
           
        }

        private void Controle_MouseEnter(object sender, EventArgs e)
        {

        }

        private void Controle_Resize_1(object sender, EventArgs e)
        {
          //  Controle.InitializeContexts();
        }

        private void Controle_MouseClick(object sender, System.Windows.Forms.MouseEventArgs e)
        {
            start_x = e.X;
            start_y = e.Y;
        }
    }
}
