using FlatTabControl;
using MathNet.Numerics.Integration;
using MathNet.Numerics.LinearAlgebra;
using MathNet.Numerics.LinearAlgebra.Double;
using Microsoft.TeamFoundation.Framework.Client;
using Microsoft.VisualBasic.Devices;
using OpenTK;
using OpenTK.Graphics.OpenGL;
using OpenTK.Input;
using OpenTK.Platform;

using Poly2Tri;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Net.NetworkInformation;
using System.Runtime.Serialization.Formatters.Binary;
using System.Text;
using System.Threading.Tasks;
using System.Web.UI.Design.WebControls;
using System.Windows.Annotations;
using System.Windows.Automation.Peers;
using System.Windows.Controls;
using System.Windows.Documents;
using System.Windows.Forms;
using System.Windows.Media;
using System.Windows.Media.TextFormatting;
using System.Windows.Shapes;
using TriangleNet;
using TriangleNet.Geometry;
using TriangleNet.Meshing;
using TriangleNet.Meshing.Iterators;
using TriangleNet.Smoothing;
using TriangleNet.Topology;
using TriangleNet.Topology.DCEL;
using Win32Interop.Enums;
using Win32Interop.Structs;
using static PG.Geom;
using static PG.GeracaoLoopsGeometria;
using static System.Windows.Forms.VisualStyles.VisualStyleElement;

namespace PG
{

    public partial class FCalculaSecao : Form
    {
        Gerenciador gerenciador;
        public FCalculaSecao()
        {
            InitializeComponent();
        }
        public vec3[] coords;
        double areaSecao;
        public bool testes;

        public bool alterando;
        public TSecao secao;
        private void button3_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void FCalculaSecao_Move(object sender, EventArgs e)
        {
            //gerenciador.AtualizaDesenho();
        }
        bool enquadrando = false;
        float pivoX, pivoY, pivoZ, centroX, centroY, centroZ;
        void calculaMinMaxCoordendas()
        {
            double xMin = 99999999999;
            double yMin = 99999999999;
            double xMax = -99999999999;
            double yMax = -99999999999;

            foreach (LinhaVec3 obj in arestasFinalGeometria)
            {
                 if (obj.p1.x < xMin)
                    xMin = obj.p1.x;

                // if (obj.pFin.z == zIni)
                if (obj.p2.x < xMin)
                    xMin = obj.p2.x;

                //  if (obj.pIni.z == zIni)
                if (obj.p1.x > xMax)
                    xMax = obj.p1.x;

                //  if (obj.pFin.z == zIni)
                if (obj.p2.x > xMax)
                    xMax = obj.p2.x;

                ///
                // if (obj.pIni.z == zIni)
                if (obj.p1.y < yMin)
                    yMin = obj.p1.y;

                //  if (obj.pFin.z == zIni)
                if (obj.p2.y < yMin)
                    yMin = obj.p2.y;

                // if (obj.pIni.z == zIni)
                if (obj.p1.y > yMax)
                    yMax = obj.p1.y;

                // if (obj.pFin.z == zIni)
                if (obj.p2.y > yMax)
                    yMax = obj.p2.y;
            }
            this.xMin = xMin;
            this.xMax = xMax;
            this.yMin = yMin;
            this.yMax = yMax;
        }
        public double xMin, xMax, yMin, yMax;
        public static double[] px_x1 = new double[1];
        public static double[] px_y1 = new double[1];
        public static double[] px_x2 = new double[1];
        public static double[] px_y2 = new double[1];
        void Pivo_no_centro()
        {
            calculaMinMaxCoordendas();
            pivoX = (float)(xMin + ((xMax - xMin) / 2));
            pivoY = (float)(yMin + ((yMax - yMin) / 2));

           // centrox_ant = (float)pivoX;
           // centroy_ant = (float)pivoY;
        }
        void CalculaCentroAproximado()
        {
            calculaMinMaxCoordendas();
            centroX = (float)(xMin + ((xMax - xMin) / 2));
            centroY = (float)(yMin + ((yMax - yMin) / 2));
        }

        public void Enquadrar()
        {
            DesenhaObjetos();
            double folga = 150;
            try
            {
                enquadrando = true;
                Pivo_no_centro();

                double box_x = 0;
                double box_y = 0;

                List<vec3> nos = new List<vec3>();
                foreach (LinhaVec3 b in arestasFinalGeometria)
                {
                    if (LocalizaPonto(ref nos, b.p1.x, b.p1.y, 0) == -1)
                        nos.Add(b.p1);
                    if (LocalizaPonto(ref nos, b.p2.x, b.p2.y, 0) == -1)
                        nos.Add(b.p2);
                }
                if (nos.Count == 0)
                {
                    enquadrando = false;
                    return;
                }
                double x_min = 99999999;
                double y_min = 99999999;
                double x_max = -99999999;
                double y_max = -99999999;

                bool necessario_menos_zoom = false;
                bool necessario_mais_zoom;
                bool frente_da_tela = false;
  
                    foreach (vec3 b in nos)
                    {
                        Vector3 p2d = new Vector3((float)b.x, (float)b.y,0);
                        Vector3 proj =  Vector3.Project(p2d, ViewPortPrincipal[0], ViewPortPrincipal[1], ViewPortPrincipal[2], ViewPortPrincipal[3],0,0, viewMatrix * Projecao);
                        px_x1[0] = proj.X;
                        px_y1[0] = proj.Y;

                    //    pixel1(ref b.x, ref b.y, ref b.z);

                    if (px_x1[0] < x_min)
                            x_min = px_x1[0];
                        if (px_y1[0] < y_min)
                            y_min = px_y1[0];

                        if (px_x1[0] > x_max)
                            x_max = px_x1[0];
                        if (px_y1[0] > y_max)
                            y_max = px_y1[0];
                    }
                


                box_x = x_max - x_min;
                box_y = y_max - y_min;

                if (((box_x) > (w - folga)) || ((box_y) > (h - folga)))
                    necessario_menos_zoom = true;

                mouseX = w / 2;
                mouseY = h / 2;

                if (necessario_menos_zoom)
                {
                    int num = 0;
                    while (necessario_menos_zoom)
                    {
                        DesenhaObjetos();
                        glControl.SwapBuffers();
                        num++;
                        if (num > 70)
                        {

                        }
                        Wheel(-120);
                        x_min = 99999999;
                        y_min = 99999999;
                        x_min = 99999999;
                        x_max = -99999999;
                        y_max = -99999999;

                        frente_da_tela = false;
                        foreach (vec3 b in nos)
                        {
                            Vector3 p2d = new Vector3((float)b.x, (float)b.y, 0);
                            Vector3 proj = Vector3.Project(p2d, ViewPortPrincipal[0], ViewPortPrincipal[1], ViewPortPrincipal[2], ViewPortPrincipal[3], 0, 0, viewMatrix * Projecao);
                            px_x1[0] = proj.X;
                            px_y1[0] = proj.Y;

                            //   pixel1(ref b.x, ref b.y, ref b.z);

                            if (px_x1[0] < x_min)
                                x_min = px_x1[0];
                            if (px_y1[0] < y_min)
                                y_min = px_y1[0];

                            if (px_x1[0] > x_max)
                                x_max = px_x1[0];
                            if (px_y1[0] > y_max)
                                y_max = px_y1[0];
                        }

                        box_x = x_max - x_min;
                        box_y = y_max - y_min;

                        if (((box_x) < (w - folga)) && ((box_y) < (h - folga)))
                        {
                            necessario_menos_zoom = false;
                        }
                        else
                        {
                            necessario_menos_zoom = true;
                            continue;
                        }
                    }
                }
                else
                {
                    x_min = 99999999;
                    y_min = 99999999;
                    x_min = 99999999;
                    x_max = -99999999;
                    y_max = -99999999;
                    int num = 0;
                    foreach (vec3 b in nos)
                    {
                        //     pixel1(ref b.x, ref b.y, ref b.z);
                        Vector3 p2d = new Vector3((float)b.x, (float)b.y, 0);
                        Vector3 proj = Vector3.Project(p2d, ViewPortPrincipal[0], ViewPortPrincipal[1], ViewPortPrincipal[2], ViewPortPrincipal[3], 0, 0, viewMatrix * Projecao);
                        px_x1[0] = proj.X;
                        px_y1[0] = proj.Y;

                        if (px_x1[0] < x_min)
                            x_min = px_x1[0];
                        if (px_y1[0] < y_min)
                            y_min = px_y1[0];

                        if (px_x1[0] > x_max)
                            x_max = px_x1[0];
                        if (px_y1[0] > y_max)
                            y_max = px_y1[0];
                    }

                    box_x = x_max - x_min;
                    box_y = y_max - y_min;
                    necessario_mais_zoom = false;
                    if (((box_x) < (w - folga)) && ((box_y) < (h - folga)))
                        necessario_mais_zoom = true;

                    while (necessario_mais_zoom)
                    {
                        DesenhaObjetos();
                        glControl.SwapBuffers();

                        x_min = 99999999;
                        y_min = 99999999;
                        x_min = 99999999;
                        x_max = -99999999;
                        y_max = -99999999;

                        num++;
                        if (num > 500)
                        {
                            Wheel(-120);
                            break;
                        }

                        Wheel(120);
                        foreach (vec3 b in nos)
                        {
                            //pixel1(ref b.x, ref b.y, ref b.z);
                            Vector3 p2d = new Vector3((float)b.x, (float)b.y, 0);
                            Vector3 proj = Vector3.Project(p2d, ViewPortPrincipal[0], ViewPortPrincipal[1], ViewPortPrincipal[2], ViewPortPrincipal[3], 0, 0, viewMatrix * Projecao);
                            px_x1[0] = proj.X;
                            px_y1[0] = proj.Y;

                            if (px_x1[0] < x_min)
                                x_min = px_x1[0];
                            if (px_y1[0] < y_min)
                                y_min = px_y1[0];

                            if (px_x1[0] > x_max)
                                x_max = px_x1[0];
                            if (px_y1[0] > y_max)
                                y_max = px_y1[0];
                        }

                        box_x = x_max - x_min;
                        box_y = y_max - y_min;

                        if (Geom.Iguais(box_x, 0) || Geom.Iguais(box_y, 0))
                            necessario_mais_zoom = false;
                        else
                        if (((box_x) < (w - folga)) && ((box_y) < (h - folga)))
                        {
                            necessario_mais_zoom = true;
                        }
                        else
                        {
                            necessario_mais_zoom = false;
                            Wheel(-120);
                            foreach (vec3 b in nos)
                            {
                                //      pixel1(ref b.x, ref b.y, ref b.z);
                                Vector3 p2d = new Vector3((float)b.x, (float)b.y, 0);
                                Vector3 proj = Vector3.Project(p2d, ViewPortPrincipal[0], ViewPortPrincipal[1], ViewPortPrincipal[2], ViewPortPrincipal[3], 0, 0, viewMatrix * Projecao);
                                px_x1[0] = proj.X;
                                px_y1[0] = proj.Y;

                                if (px_x1[0] < x_min)
                                    x_min = px_x1[0];
                                if (px_y1[0] < y_min)
                                    y_min = px_y1[0];

                                if (px_x1[0] > x_max)
                                    x_max = px_x1[0];
                                if (px_y1[0] > y_max)
                                    y_max = px_y1[0];
                            }

                            if (((box_x) < (w - folga)) && ((box_y) < (h - folga)))
                            {
                                continue;
                            }
                            else
                            {
                                Wheel(-120);
                            }
                        }
                    }
                }

                CalculaCentroAproximado();

                double cx = centroX;
                double cy = centroY;
                double cz = centroZ;
               
                //pixel1(ref cx, ref cy, ref cz);

                Vector3 p2d_ = new Vector3((float)cx, (float)cy, 0);
                Vector3 proj_ = Vector3.Project(p2d_, ViewPortPrincipal[0], ViewPortPrincipal[1], ViewPortPrincipal[2], ViewPortPrincipal[3], 0, 1, viewMatrix * Projecao);
                px_x1[0] = proj_.X;
                px_y1[0] = proj_.Y;

                int mx = (int)px_x1[0];
                int my = (int)px_y1[0];
                raioZoom.GerarRaio3D(ref mx, ref my, ref ViewPortPrincipal, ref viewMatrix, ref Projecao);

                Ponto_zNear = new vec3(raioZoom.p0.x, raioZoom.p0.y, raioZoom.p0.z);

                double dist_near_HitObjeto = (Ponto_zNear - new vec3(centroX, centroY, centroZ)).Magnitude();

                /*raio partindo do meio da tela*/
                int meio_x = (int)(w / 2);
                int meio_y = (int)(h / 2);
                raioZoom.GerarRaio3D(ref meio_x, ref meio_y, ref ViewPortPrincipal, ref viewMatrix, ref Projecao);

                double distancia_znear_ate_plano = 100;
                vec3 normalPlano = new vec3(0, 0, 1);
                PlanoPanning = new Plano(normalPlano, new vec3(0, 0, -distancia_znear_ate_plano), "xy", false);
                PlanoPanning.xmin = -2; PlanoPanning.ymin = -2; PlanoPanning.xmax = 2; PlanoPanning.ymax = 2;

                viewMatrix_Panning = Matrix4.CreateTranslation(0, 0, 0);
                raioZoom.GerarRaio3D(ref mx, ref my, ref ViewPortPrincipal, ref viewMatrix_Panning, ref Projecao);

                raioZoom.CalculaIntersecao_Raio_x_Plano(ref PlanoPanning, ref Coord2_PlanoPanning);

                raioZoom.GerarRaio3D(ref meio_x, ref meio_y, ref ViewPortPrincipal, ref viewMatrix_Panning, ref Projecao);

                raioZoom.CalculaIntersecao_Raio_x_Plano(ref PlanoPanning, ref Coord_PlanoPanning);
                vec3 dif = Coord_PlanoPanning - Coord2_PlanoPanning;

                dif *= -1;

                //return;

                x_trans += (float)dif.x;
                y_trans += (float)dif.y*-1;

                y_trans *= -1;
                x_trans *= -1;

                DesenhaObjetos();
                glControl.SwapBuffers();
                enquadrando = false;
            }
            catch (Exception ee)
            {
                enquadrando = false;
                MessageBox.Show("erro ao enqudrar -> " + ee.Message);
            }
        }
        vec3 Coord_PlanoPanning = new vec3(0, 0, 0);


        private void FCalculaSecao_FormClosed(object sender, FormClosedEventArgs e)
        {
            if (gerenciador.DadosBarra.SecaoLaminada != null)
                gerenciador.DadosBarra.SecaoLaminada.calculasecao = null;
            if (gerenciador.DadosBarra.SecaoSolida != null)
                gerenciador.DadosBarra.SecaoSolida.calculasecao = null;
            gerenciador.formDesenho.glControl.MakeCurrent();
            /*if (shader_triangulos_program != 0)
                GL.DeleteProgram(shader_triangulos_program);
            if (fragment_shader_object_triangulos != 0)
                GL.DeleteShader(fragment_shader_object_triangulos);
            if (vertex_shader_object_triangulos != 0)
                GL.DeleteShader(vertex_shader_object_triangulos);
            if (vertex_buffer_triangulos_principal != 0)
                GL.DeleteBuffers(1, ref vertex_buffer_triangulos_principal);
            if (vertex_buffer_triangulos_deformacao != 0)
                GL.DeleteBuffers(1, ref vertex_buffer_triangulos_deformacao);

            if (shader_arestas_program != 0)
                GL.DeleteProgram(shader_arestas_program);
            if (fragment_shader_object_arestas != 0)
                GL.DeleteShader(fragment_shader_object_arestas);
            if (vertex_shader_object_arestas != 0)
                GL.DeleteShader(vertex_shader_object_arestas);
            if (vertex_buffer_arestas != 0)
                GL.DeleteBuffers(1, ref vertex_buffer_arestas);

            if (index_buffer_object != 0)
                GL.DeleteBuffers(1, ref index_buffer_object);*/

          /*  gerenciador.formDesenho.IniciaOGL();
            gerenciador.formDesenho.PreencheBatchTriangulos();
            gerenciador.formDesenho.PreencheBatchArestas();
            gerenciador.formDesenho.AtualizaVBO();
            gerenciador.formDesenho.DesenhaObjetos();
            gerenciador.formDesenho.glControl.SwapBuffers();*/
        }
        bool calculoOK;
        float offset_x, offset_y, fatorzoom_orto = -5;
        public static OpenTK.Matrix4 Projecao, Projecao_Panning, proj_Cubo, ViewMatrix, Projecao2;
        double zNear, zFar;
        double GLtop, GLbotton, GLleft, GLright;
        int VAO_triangulos_principal, VAO_triangulos_deformacoes, VAO_arestas;
        public static OpenTK.Matrix4 viewMatrix_Panning, viewMatrix, viewMatrixTextos,viewMatrix_temp, modelViewMatrix_Zoom;
        public float x_trans, y_trans, z_trans;
        public int mouseX = 0, mouseY = 0;
        bool Panning;
        Raio raioZoom = new Raio(0, 0);
        int qtd_coords_triangulos_principal = 0;
        int qtd_coords_triangulos_deformacao = 0;
        int qtd_coords_arestas = 0;
        double leftOrtoZoom, rightOrtoZoom, topOrtoZoom, bottonOrtoZoom;
        double leftOrtoZoom2, rightOrtoZoom2, topOrtoZoom2, bottonOrtoZoom2;
        public static int[] ViewPortPrincipal = new int[4];
        vec3 Coord2_PlanoPanning = new vec3(0, 0, 0);
        vec3 Coord_PlanoZoom = new vec3(0, 0, 0);
        Plano PlanoPanning, planoZoom;
        float start_x = 0, start_y = 0;
        vec3 pb, pd, Ponto_zNear, Ponto_HitObjeto, Ponto_zFar;
        double xant, yant = 0, zant = 0;
        vec3 normXY;
        private void glControl_MouseDown(object sender, System.Windows.Forms.MouseEventArgs e)
        {
            if (e.Button == System.Windows.Forms.MouseButtons.Middle)
            {
                Panning = true;

                Ponto_zNear = new vec3(raioZoom.p0.x, raioZoom.p0.y, raioZoom.p0.z);
     
                double distancia_znear_ate_plano = 1;

                vec3 normalPlano = new vec3(0, 0, 1);
                PlanoPanning = new Plano(normalPlano, new vec3(0, 0, -distancia_znear_ate_plano), "xy", false);
                PlanoPanning.xmin = -2; PlanoPanning.ymin = -2; PlanoPanning.xmax = 2; PlanoPanning.ymax = 2;

                viewMatrix_Panning = Matrix4.CreateTranslation(0, 0, 0);
                raioZoom.GerarRaio3D(ref mouseX, ref mouseY, ref ViewPortPrincipal, ref viewMatrix_Panning, ref Projecao);

                raioZoom.CalculaIntersecao_Raio_x_Plano(ref PlanoPanning, ref Coord2_PlanoPanning);
                start_x = (float)Coord2_PlanoPanning.x;
                start_y = (float)Coord2_PlanoPanning.y;
            }
        }
        public void Desenho_MouseWheel(object sender, System.Windows.Forms.MouseEventArgs e)
        {
            Wheel(e.Delta);

            //  AtualizaDisplayList_Nos();
            DesenhaObjetos();
            glControl.SwapBuffers();

        }
        private void glControl_MouseUp(object sender, System.Windows.Forms.MouseEventArgs e)
        {
            Panning = false;

            if (e.Button == System.Windows.Forms.MouseButtons.Middle)
                Panning = false;

        }
        float z_trans_orto;

        int LocalizaPonto(ref List<vec3> lista, double x, double y, double z)
        {
            int jk;
            for (jk = 0; jk < lista.Count; jk++)
                if (Geom.Iguais(x, lista[jk].x) && Geom.Iguais(y, lista[jk].y) && Geom.Iguais(z, lista[jk].z))
                    return jk;

            return -1;
        }
        string caminho_templateAtual, template;
        List<double> valorCotasAlterar;
        List<double> valorRaiosAlterar;
        bool biblioteca;
        public FCalculaSecao(Gerenciador ger, string _template, string caminho_template, string nome, bool testasecao, ref List<double> valorCotas, ref List<double> valorRaios, bool _biblioteca = false, bool _alterando = false, TSecao _secao = null)
        {
            try
            {
                this.alterando = _alterando;
                this.gerenciador = ger;
                if (gerenciador.ConfiguracaoPrograma.SuavizacaoOpenGl)
                    this.glControl = new OpenTK.GLControl(new OpenTK.Graphics.GraphicsMode(32, 24, 0, 8));
                else
                    this.glControl = new OpenTK.GLControl(new OpenTK.Graphics.GraphicsMode(32, 24, 0, 0));

                caminho_templateAtual = caminho_template;
                template = _template;

                AbrirTemplate(caminho_templateAtual);
                CriarArestasTemplateOriginal();
                CriarCotas(true);
                CriarArestasFinalGeometria_SemRaio();
                CriarRaios(true);
                GeometriaFinalOrdenadaCCW_ComRaios();

                InitializeComponent();

                testes = testasecao;

                using (Font font = new Font(grid.DefaultCellStyle.Font.FontFamily, 8, FontStyle.Regular))
                {
                    grid.Columns[0].DefaultCellStyle.Font = font;
                    grid.Columns[1].DefaultCellStyle.Font = font;
                }
                using (Font font = new Font(gridEixosPrincipais.DefaultCellStyle.Font.FontFamily, 8, FontStyle.Regular))
                {
                    gridEixosPrincipais.Columns[0].DefaultCellStyle.Font = font;
                    gridEixosPrincipais.Columns[1].DefaultCellStyle.Font = font;
                }

                paginas = new List<TabPage>();
                paginas.Add(flatTabControl1.TabPages[0]);
                paginas.Add(flatTabControl1.TabPages[1]);

                this.Left = gerenciador.Width - (this.Width / 4);
               
                pnCota.Visible = false;

                AtualizaMateriais(true, true);

                if (alterando)
                {
                    secao = _secao;
                    Carregar();
                    valorCotasAlterar = valorCotas;
                    valorRaiosAlterar = valorRaios;
                    this.Text = "Alterar seção transversal";
                }
                else
                {
                    this.Text = "Nova seção transversal";

                    this.edDescricao.Text = nome;
                    if (gerenciador.formDesenho.Secoes.Count > 0)
                    {
                        ColorF cor = new ColorF((double)gerenciador.formDesenho.Secoes[gerenciador.formDesenho.Secoes.Count - 1].Rgb[0] / 255,
                                                (double)gerenciador.formDesenho.Secoes[gerenciador.formDesenho.Secoes.Count - 1].Rgb[1] / 255,
                                                (double)gerenciador.formDesenho.Secoes[gerenciador.formDesenho.Secoes.Count - 1].Rgb[2] / 255);
                        cor = ColorGenerator.NextGoldenColor(cor);
                        //                    btCor.BackColor = System.Drawing.Color.FromArgb((int)(cor.R * 255), (int)(cor.G * 255), (int)(cor.B * 255));
                        btCor.BackColor = System.Drawing.Color.FromArgb(
                        RMath.Clamp((int)Math.Round(cor.R * 255.0)),
                        RMath.Clamp((int)Math.Round(cor.G * 255.0)),
                        RMath.Clamp((int)Math.Round(cor.B * 255.0)));
                    }
                    else
                        btCor.BackColor = System.Drawing.Color.CadetBlue;

                    if (caminho_templateAtual.Contains("Concreto"))
                    {
                         cbMaterial.SelectedIndex = gerenciador.ConfiguracoesPGi.CfgProjeto.materiais.IndexOf( gerenciador.ConfiguracoesPGi.CfgProjeto.materiais.First(o=>o.Tipo == 1));
                    }
                    else
                    {
                        cbMaterial.SelectedIndex = gerenciador.ConfiguracoesPGi.CfgProjeto.materiais.IndexOf(gerenciador.ConfiguracoesPGi.CfgProjeto.materiais.First(o => o.Tipo == 0));
                    }
                }

                this.biblioteca = _biblioteca;
                if (biblioteca)
                {
                    valorCotasAlterar = valorCotas;
                    valorRaiosAlterar = valorRaios;
                }

                tuboRedondoVazado = caminho_templateAtual.Contains("Diversos\\sec3");

            }
            catch (Exception ee)
            {
                MessageBox.Show(ee.Message);
            }
            // AtualizaShaders(false);
        }
        void Carregar()
        {
            calculoSecao = new TCalculoSecao();
            calculoSecao.propriedades = secao.propriedades;
            if (secao.propriedades_eixos_principais != null)
            {
               // calculoSecao.propriedades.inercia_flexao_y = secao.inercia_flexao_y;
              //  calculoSecao.propriedades.inercia_flexao_z = secao.inercia_flexao_z;
                
                calculoSecao_EixosLocais = new TCalculoSecao();
                calculoSecao_EixosLocais.propriedades = secao.propriedades_eixos_principais;
            }

            edDescricao.Text = secao.descricao;
            cbMaterial.SelectedIndex = secao.idMaterial;
            btCor.BackColor = System.Drawing.Color.FromArgb(secao.Rgb[0], secao.Rgb[1], secao.Rgb[2]);
        }

        bool tuboRedondoVazado = false;
        List<vec3> coordenadas;

        public List<cotasSecao> cotas = new List<cotasSecao>();
        public void AtualizaMateriais(bool concreto, bool aco)
        {
            bool temAco = false;
            int ultimoconc = -1;

            cbMaterial.Items.Clear();

            for (int i = 0; i < gerenciador.ConfiguracoesPGi.CfgProjeto.materiais.Count; i++)
            {
                if (gerenciador.ConfiguracoesPGi.CfgProjeto.materiais[i].Tipo == 1) // se for concreto
                    ultimoconc = i;
            }

            foreach (TMateriais m in gerenciador.ConfiguracoesPGi.CfgProjeto.materiais)
            {
                // if (m.Tipo == 0 && aco)  // aço
                //  cbMaterial.Items.Add(m.Descricao);

                //  if (m.Tipo == 1 && concreto)  // concreto
                cbMaterial.Items.Add(m.Descricao);
            }

            if (ultimoconc != -1)
                cbMaterial.SelectedIndex = ultimoconc;
        }
        double cy_carregado, cz_carregado;
        private void FCalculaSecao_Shown(object sender, EventArgs e)
        {
            glControl.MakeCurrent();
            AtualizaShaders(false);
            DesenhaObjetos();
            glControl.SwapBuffers();

            glControl.Focus();

            if (alterando)
            {
                for (int i = 0; i < valorCotasAlterar.Count; i++)
                    AlterarCota(i, valorCotasAlterar[i]);

                for (int i = 0; i < valorRaiosAlterar.Count; i++)
                    AlterarRaio(i, valorRaiosAlterar[i]);

                AtualizaGrid();

                alterouGeometria = false;


                if (calculoSecao_EixosLocais != null)
                {
                    flatTabControl1.TabPages.Add(paginas[1]);
                    AtualizaGrid_EixosLocais();
                }

                Recalcular = false;
             /*   centralizado = true;

                cy_carregado = calculoSecao.propriedades.cy;
                cz_carregado = calculoSecao.propriedades.cz;

                rot_eixo_u = (new vec3(15 + cy_carregado, cz_carregado, 0)).Rotate(-anguloEixosPrincipais);
                rot_eixo_v = (new vec3(cy_carregado, -15 + cz_carregado, 0)).Rotate(-anguloEixosPrincipais);*/

                AtualizaShaders();

                Enquadrar();
            }
            else
            {
                if (biblioteca)
                {
                    for (int i = 0; i < valorCotasAlterar.Count; i++)
                    {
                        AlterarCota(i, valorCotasAlterar[i]);
                    }

                    for (int i = 0; i < valorRaiosAlterar.Count; i++)
                    {
                        AlterarRaio(i, valorRaiosAlterar[i]);
                    }

                    Enquadrar();
                }
            }
        }

        private void button2_Click(object sender, EventArgs e)
        {

        }

        public List<raiosSecao> raios = new List<raiosSecao>();
        List<TBarraGenerica> bars;
        TInfoSecao infoSecao;

        List<LinhaVec3> arestasTemplateOriginal, arestasRaios, arestasFinalGeometria_SemRaio, arestasFinalGeometria;

        private void glControl_Enter(object sender, EventArgs e)
        {

        }

        List<LinhaVec3> arestasCotas, marcacoesCotas;
        List<double> angCotas;
        List<double> valCotas;
        List<double> valRaios;
        bool alterouGeometria = false;
        void AlterarDiametro(int cota, double novaDim)
        {
            alterouGeometria = true;

            float twicePi = 2.0f * 3.1415f;
            List<vec3> pts = new List<vec3>();
            double raio_ = novaDim / 2;
            int discret = 20;
            for (int j = 0; j < discret; j++)
              pts.Add(new vec3(raio_ * Math.Cos(j * twicePi / discret), raio_ * Math.Sin(j * twicePi / discret), 0));
            
            vec3 zero = new vec3(pts[0].x, -pts[0].y, 0);
            if (!infoSecao.cotas[cota].diametroInterno)
            {
                for (int i = 0; i < discret; i++)
                {
                    int prox = (i + 1) % pts.Count;
                    arestasTemplateOriginal[i].p1.x = pts[i].x;// - zero.x;
                    arestasTemplateOriginal[i].p1.y = pts[i].y;// - zero.y;

                    arestasTemplateOriginal[i].p2.x = pts[prox].x;// - zero.x;
                    arestasTemplateOriginal[i].p2.y = pts[prox].y;// - zero.y;
                }

                infoSecao.cotas[cota].p2.x = pts[0].x;//- zero.x;
                infoSecao.cotas[cota].p2.y = pts[0].y;// - zero.y;
                arestasCotas[cota].p2.x = infoSecao.cotas[cota].p2.x;
                arestasCotas[cota].p2.y = infoSecao.cotas[cota].p2.y;

                infoSecao.cotas[cota].p1.x = pts[(discret / 2)].x;//- zero.x;
                infoSecao.cotas[cota].p1.y = pts[(discret / 2)].y;//- zero.y;
                arestasCotas[cota].p1.x = infoSecao.cotas[cota].p1.x;
                arestasCotas[cota].p1.y = infoSecao.cotas[cota].p1.y;
            }
            else
            if (infoSecao.cotas[cota].diametroInterno)
            {
                for (int i = 0; i < discret; i++)
                {
                    int prox = (i + 1) % pts.Count;
                    arestasTemplateOriginal[i + discret].p1.x = pts[i].x;// - zero.x;
                    arestasTemplateOriginal[i + discret].p1.y = pts[i].y;// - zero.y;

                    arestasTemplateOriginal[i + discret].p2.x = pts[prox].x;// - zero.x;
                    arestasTemplateOriginal[i + discret].p2.y = pts[prox].y;// - zero.y;
                }

                infoSecao.cotas[cota].p2.x = pts[3].x;//- zero.x;
                infoSecao.cotas[cota].p2.y = pts[3].y;// - zero.y;
                arestasCotas[cota].p2.x = infoSecao.cotas[cota].p2.x;
                arestasCotas[cota].p2.y = infoSecao.cotas[cota].p2.y;

                infoSecao.cotas[cota].p1.x = pts[(discret / 2) +3].x;//- zero.x;
                infoSecao.cotas[cota].p1.y = pts[(discret / 2) + 3].y;//- zero.y;
                arestasCotas[cota].p1.x = infoSecao.cotas[cota].p1.x;
                arestasCotas[cota].p1.y = infoSecao.cotas[cota].p1.y;


            }

            CalculaValorCotas();
            CriarMarcacoesCotas();
            CalculaAnguloCotas();
            CriarArestasFinalGeometria_SemRaio();
            GeometriaFinalOrdenadaCCW_ComRaios();

            apagarCelulas();

            AtualizaShaders(false);

            if (enquadrarAposCota.Checked)
                Enquadrar();

            DesenhaObjetos();
            glControl.SwapBuffers();

        }

        void AlterarCota(int cota, double novaDim, bool recursiva = false)
        {
            pnCota.Visible = false;

            if (cota != -1)
            {
                alterouGeometria = true;

                cotasSecao cotaAlterando = infoSecao.cotas[cota];

                if (cotaAlterando.cotaDiametro)
                {
                    if (cotaAlterando.diametroInterno)
                    {
                        double cotaexterna = valCotas[cota - 1];

                        if (cotaexterna < novaDim || (Geom.Iguais(cotaexterna,novaDim)))
                        {
                            MessageBox.Show("A cota interna não pode ser maior ou igual à cota externa." , "Atenção", MessageBoxButtons.OK, MessageBoxIcon.Information);
                            return;
                        }
                    }
                    else
                    {
                        if (valCotas.Count > 1) // se é um tubo circular vazado...tem duas cotas
                        {
                            double cotainterna = valCotas[cota + 1];

                            if (cotainterna > novaDim || (Geom.Iguais(cotainterna, novaDim)))
                            {
                                MessageBox.Show("A cota externa não pode ser menor ou igual à cota interna.", "Atenção", MessageBoxButtons.OK, MessageBoxIcon.Information);
                                return;
                            }
                        }
                    }

                    AlterarDiametro(cota, novaDim);
                }
                else
                {
                    if (cotaAlterando.pontosDeslocar == null)
                    {
                        MessageBox.Show("Essa cota não possui vértices para deslocar! Contate o desenvolvedor. \r\rTemplate: '" + caminho_templateAtual + "'", "Atenção", MessageBoxButtons.OK, MessageBoxIcon.Information);
                        return;
                    }

                    if (cotaAlterando.pontosDeslocar.Count == 0)
                        return;

                    double dif = valCotas[cota] - novaDim;

                //   if (Geom.Iguais( dif,0))
                //        return;

                    valCotas[cota] = novaDim;
                    vec3 vetor_desloc, novo;

                    int max_tot_pontos_deslocar = -99999;
                    for (int i = 0; i < infoSecao.cotas.Count; i++)
                        if (infoSecao.cotas[i].pontosDeslocar != null)
                            if (infoSecao.cotas[i].pontosDeslocar.Count > max_tot_pontos_deslocar)
                                max_tot_pontos_deslocar = infoSecao.cotas[i].pontosDeslocar.Count;

                    if (cotaAlterando.cresceDoisSentidos)
                        dif /= 2;

                    double dif_original = dif;

                    bool[] movimentou_p1_arestas = new bool[arestasTemplateOriginal.Count];
                    bool[] movimentou_p2_arestas = new bool[arestasTemplateOriginal.Count];
                    bool[] movimentou_raios = new bool[infoSecao.raios.Count];
                    bool[] movimentou_p1_outrasCotas = new bool[infoSecao.cotas.Count];
                    bool[] movimentou_p2_outrasCotas = new bool[infoSecao.cotas.Count];
                    bool[,] movimentou_pontosDeslocar_outrasCotas = new bool[infoSecao.cotas.Count, max_tot_pontos_deslocar];

                    movimentou_p1_outrasCotas[cota] = true;
                    movimentou_p2_outrasCotas[cota] = true;

                    raiosSecao raioAtual;
                    for (int i = 0; i < cotaAlterando.pontosDeslocar.Count; i++)
                    {
                        //vetor_desloc = cotaAlterando.pontosDeslocar[i].p;
                        vetor_desloc = cotaAlterando.pontosDeslocar[i].p - new vec3(cotaAlterando.pontosDeslocar[i].vetor.x, cotaAlterando.pontosDeslocar[i].vetor.y, 0);

                        if (cotaAlterando.pontosDeslocar[i].angulo != 0)
                        {
                            double cos_anrad = Math.Cos(cotaAlterando.pontosDeslocar[i].angulo * Const.PIDiv180);
                            double novaDif = dif / cos_anrad;
                            dif = novaDif;
                        }
                        else
                            dif = dif_original;

                        //altera o p1 e o p2 da aresta[j] da lista de arestas do template original da seção que tem o mesmo ponto de deslocamento[i], para enviar depois a aresta corrigida p/ funcao preencheBatchArestas()
                        for (int j = 0; j < arestasTemplateOriginal.Count; j++)
                        {
                            //testar se o p1 ja movimentou 
                            if (!movimentou_p1_arestas[j])
                                if (Geom.Iguais(arestasTemplateOriginal[j].p1.x, cotaAlterando.pontosDeslocar[i].p.x) &&
                                Geom.Iguais(arestasTemplateOriginal[j].p1.y, cotaAlterando.pontosDeslocar[i].p.y))
                                {
                                    novo = vec3.MoverNaDirecao(arestasTemplateOriginal[j].p1, vetor_desloc, dif);
                                    arestasTemplateOriginal[j].p1.x = novo.x;
                                    arestasTemplateOriginal[j].p1.y = novo.y;
                                    movimentou_p1_arestas[j] = true;
                                }

                            //testar se o p2 ja movimentou 
                            if (!movimentou_p2_arestas[j])
                                if (Geom.Iguais(arestasTemplateOriginal[j].p2.x, cotaAlterando.pontosDeslocar[i].p.x) &&
                                Geom.Iguais(arestasTemplateOriginal[j].p2.y, cotaAlterando.pontosDeslocar[i].p.y))
                                {
                                    novo = vec3.MoverNaDirecao(arestasTemplateOriginal[j].p2, vetor_desloc, dif);
                                    arestasTemplateOriginal[j].p2.x = novo.x;
                                    arestasTemplateOriginal[j].p2.y = novo.y;
                                    movimentou_p2_arestas[j] = true;
                                }
                        }

                        //procuro outras cotas que tenham o mesmo ponto para deslocar e desloco esse ponto e seu vetor tbm...
                        for (int j = 0; j < infoSecao.cotas.Count; j++)
                        {
                            if (j != cota)
                            {
                                cotasSecao outraCota = infoSecao.cotas[j];

                                //o p1 e o p2 da cota é o que eu uso pra desenhar a cota...
                                if (!movimentou_p1_outrasCotas[j] && !movimentou_p2_outrasCotas[j])

                                    if ((Geom.Iguais(outraCota.p1.x, cotaAlterando.pontosDeslocar[i].p.x) &&
                                        Geom.Iguais(outraCota.p1.y, cotaAlterando.pontosDeslocar[i].p.y))
                                        ||
                                        (Geom.Iguais(outraCota.p2.x, cotaAlterando.pontosDeslocar[i].p.x) &&
                                        Geom.Iguais(outraCota.p2.y, cotaAlterando.pontosDeslocar[i].p.y)))
                                    {
                                        novo = vec3.MoverNaDirecao(outraCota.p1, vetor_desloc, dif_original);
                                        outraCota.p1.x = novo.x;
                                        outraCota.p1.y = novo.y;

                                        novo = vec3.MoverNaDirecao(arestasCotas[j].p1, vetor_desloc, dif_original);
                                        arestasCotas[j].p1.x = novo.x;
                                        arestasCotas[j].p1.y = novo.y;

                                        novo = vec3.MoverNaDirecao(outraCota.p2, vetor_desloc, dif_original);
                                        outraCota.p2.x = novo.x;
                                        outraCota.p2.y = novo.y;

                                        novo = vec3.MoverNaDirecao(arestasCotas[j].p2, vetor_desloc, dif_original);
                                        arestasCotas[j].p2.x = novo.x;
                                        arestasCotas[j].p2.y = novo.y;

                                        movimentou_p1_outrasCotas[j] = true;
                                        movimentou_p2_outrasCotas[j] = true;
                                    }

                                if (outraCota.pontosDeslocar != null)
                                {
                                    for (int k = 0; k < outraCota.pontosDeslocar.Count; k++)
                                    {

                                        if (!movimentou_pontosDeslocar_outrasCotas[j, k])

                                            if (Geom.Iguais(cotaAlterando.pontosDeslocar[i].p.x, outraCota.pontosDeslocar[k].p.x) &&
                                                Geom.Iguais(cotaAlterando.pontosDeslocar[i].p.y, outraCota.pontosDeslocar[k].p.y))
                                            {
                                                novo = vec3.MoverNaDirecao(outraCota.pontosDeslocar[k].p, vetor_desloc, dif);
                                                outraCota.pontosDeslocar[k].p.x = novo.x;
                                                outraCota.pontosDeslocar[k].p.y = novo.y;

                                                novo = vec3.MoverNaDirecao(outraCota.pontosDeslocar[k].vetor, vetor_desloc, dif);
                                                outraCota.pontosDeslocar[k].vetor.x = novo.x;
                                                outraCota.pontosDeslocar[k].vetor.y = novo.y;

                                                movimentou_pontosDeslocar_outrasCotas[j, k] = true;
                                            }
                                    }
                                }
                            }
                        }

                        // desloca o ponto principal do raio e depois recria os demais pontos desse raio ali embaixo com CriarRaios(false)
                        for (int i_raio = 0; i_raio < infoSecao.raios.Count; i_raio++)
                        {
                            raioAtual = infoSecao.raios[i_raio];

                            if (!movimentou_raios[i_raio])
                                if (Geom.Iguais(raioAtual.p1.x, cotaAlterando.pontosDeslocar[i].p.x) &&
                                    Geom.Iguais(raioAtual.p1.y, cotaAlterando.pontosDeslocar[i].p.y))
                                {
                                    novo = vec3.MoverNaDirecao(raioAtual.p1, vetor_desloc, dif);
                                    raioAtual.p1.x = novo.x;
                                    raioAtual.p1.y = novo.y;

                                    movimentou_raios[i_raio] = true;

                                    for (int m = 0; m < arestasRaios.Count; m++)
                                    {
                                        if (arestasRaios[m].p1.vv == i_raio && arestasRaios[m].p2.vv == i_raio) //vv é uma tag que eu uso no CriaRaios() para gravar o indice do raio nas arestas desse raio
                                        {
                                            //   novo = vec3.MoverNaDirecao(arestasRaios[m].p1, vetor_desloc, dif);
                                            // arestasRaios[m].p1.x = novo.x;
                                            // arestasRaios[m].p1.y = novo.y;

                                            // novo = vec3.MoverNaDirecao(arestasRaios[m].p2, vetor_desloc, dif);
                                            // arestasRaios[m].p2.x = novo.x;
                                            // arestasRaios[m].p2.y = novo.y;
                                        }
                                    }

                                    ///    arestasCotas[j].p1.x = novo.x;
                                    //         arestasCotas[j].p1.y = novo.y;
                                }
                        }
                    }

                    for (int i = 0; i < cotaAlterando.pontosDeslocar.Count; i++)
                    {
                        vetor_desloc = cotaAlterando.pontosDeslocar[i].p - new vec3(cotaAlterando.pontosDeslocar[i].vetor.x, cotaAlterando.pontosDeslocar[i].vetor.y, 0);
                       
                        if (cotaAlterando.pontosDeslocar[i].angulo != 0)
                        {
                            double cos_anrad = Math.Cos(cotaAlterando.pontosDeslocar[i].angulo * Const.PIDiv180);
                            double novaDif = dif / cos_anrad;
                            dif = novaDif;
                        }
                        else
                            dif = dif_original;

                        //altera ponto de deslocamento[i] da cota
                        novo = vec3.MoverNaDirecao(cotaAlterando.pontosDeslocar[i].p, vetor_desloc, dif);
                        cotaAlterando.pontosDeslocar[i].p.x = novo.x;
                        cotaAlterando.pontosDeslocar[i].p.y = novo.y;

                        //altera vetor do ponto de deslocamento[i] da cota
                        novo = vec3.MoverNaDirecao(cotaAlterando.pontosDeslocar[i].vetor, vetor_desloc, dif);
                        cotaAlterando.pontosDeslocar[i].vetor.x = novo.x;
                        cotaAlterando.pontosDeslocar[i].vetor.y = novo.y;
                    }

                    //o p1 e o p2 da cota é o que eu uso pra desenhar a cota...
                    vec3 p2_antes = new vec3(cotaAlterando.p2.x, cotaAlterando.p2.y, 0);
                    vetor_desloc = cotaAlterando.p1 - new vec3(cotaAlterando.p2.x, cotaAlterando.p2.y, 0);
                    novo = vec3.MoverNaDirecao(cotaAlterando.p2, vetor_desloc, dif_original);
                    cotaAlterando.p2.x = novo.x;
                    cotaAlterando.p2.y = novo.y;

                    novo = vec3.MoverNaDirecao(arestasCotas[cota].p2, vetor_desloc, dif_original);
                    arestasCotas[cota].p2.x = novo.x;
                    arestasCotas[cota].p2.y = novo.y;

                    if (cotaAlterando.cresceDoisSentidos)
                    {
                        vetor_desloc = p2_antes - new vec3(cotaAlterando.p1.x, cotaAlterando.p1.y, 0);
                        novo = vec3.MoverNaDirecao(cotaAlterando.p1, vetor_desloc, dif_original);

                        cotaAlterando.p1.x = novo.x;
                        cotaAlterando.p1.y = novo.y;

                        novo = vec3.MoverNaDirecao(arestasCotas[cota].p1, vetor_desloc, dif_original);
                        arestasCotas[cota].p1.x = novo.x;
                        arestasCotas[cota].p1.y = novo.y;
                    }

                    //  CriarCotas(false);
                    CriarMarcacoesCotas();
                    CalculaAnguloCotas();
                    CriarArestasFinalGeometria_SemRaio();
                    CriarRaios(false);
                    GeometriaFinalOrdenadaCCW_ComRaios();

                    apagarCelulas();

                    AtualizaShaders(false);

                    /* if (infoSecao.cotas.Exists(p => p.igualOutraCota == true && p.cotaIgualar == cota))
                     {
                             if (!alterouOutraCota)
                             {
                                 alterouOutraCota = true;
                                 int ci = infoSecao.cotas.IndexOf(infoSecao.cotas.First(p => p.igualOutraCota == true && p.cotaIgualar == cota));
                                 AlterarCota(ci, novaDim);
                             }
                         
                     }*/

                    if (!recursiva)
                    {
                        if (cotaAlterando.igualOutraCota)
                        {
                            for (int hh = 0; hh < 4; hh++)
                            {
                                int ci = -1;
                                if (hh == 0 && cotaAlterando.cotaIgualar1 != -1)
                                  ci = cotaAlterando.cotaIgualar1;
                                else
                                if (hh == 1 && cotaAlterando.cotaIgualar2 != -1)
                                    ci = cotaAlterando.cotaIgualar2;
                                else
                                if (hh == 2 && cotaAlterando.cotaIgualar3 != -1)
                                    ci = cotaAlterando.cotaIgualar3;
                                else
                                if (hh == 3 && cotaAlterando.cotaIgualar4 != -1)
                                    ci = cotaAlterando.cotaIgualar4;

                                if (ci != -1)
                                  AlterarCota(ci, novaDim, true);
                            }

                            //  while (!alterouTodas)
                            {
                                alterouOutraCota = true;
                            }
                        }
                    }


                    alterouOutraCota = false;

                    if (enquadrarAposCota.Checked)
                        Enquadrar();

                    DesenhaObjetos();
                    glControl.SwapBuffers();
                }
            }
        }
        bool Recalcular = true;
        bool alterouTodas = false;
        void apagarCelulas()
        {
            Recalcular = true;
            props = null;
            bindingSource1.DataSource = props;
            grid.DataSource = bindingSource1;
            gridEixosPrincipais.DataSource = bindingSource2;

            celulas = null;

            lbTotCelulas.Text = "";
            centralizado = false;
            edTamCelula.Text = "";
            edDescricaoProp.Clear();
        }
        bool alterouOutraCota = false;
        void CriarRaios(bool primeiraVez)
        {
            raiosSecao raioAtual;
            vec3 ponto_que_contem_o_raio = new vec3(0);
            arestasRaios = new List<LinhaVec3>();

            arestaVisivel = new bool[arestasTemplateOriginal.Count];
            for (int j = 0; j < arestasTemplateOriginal.Count; j++)
              arestaVisivel[j] = true;
            
            raiosMarcar = new bool[infoSecao.raios.Count];

            raiosInvisiveis = new List<int>();

            for (int i = 0; i < infoSecao.raios.Count; i++)
            {
                raioAtual = infoSecao.raios[i];

                if (primeiraVez)
                {
                    raioAtual.p1.x -= zero.x;
                    raioAtual.p1.y -= zero.y;
                    raioAtual.p1.x *= 10;
                    raioAtual.p1.y *= 10;

                   // raioAtual.raio *= 10;
                }

                if (raioAtual.igualOutroRaio)
                {
                    if (raioAtual.raioIgualar1 != -1)
                        raiosInvisiveis.Add(raioAtual.raioIgualar1);
                    if (raioAtual.raioIgualar2 != -1)
                        raiosInvisiveis.Add(raioAtual.raioIgualar2);
                    if (raioAtual.raioIgualar3 != -1)
                        raiosInvisiveis.Add(raioAtual.raioIgualar3);
                    if (raioAtual.raioIgualar4 != -1)
                        raiosInvisiveis.Add(raioAtual.raioIgualar4);
                }

                ponto_que_contem_o_raio = new vec3(infoSecao.raios[i].p1.x, infoSecao.raios[i].p1.y, 0);
                int aresta_p2 = -1, aresta_p3 = -1;
                vec3 PA = new vec3(0, 0, 0, false, 2), PB = new vec3(0,0,0,false,2);
                
                bool ponto_raio_contorno_interno = false;
              
          //      List<LinhaVec3> arestas_1_e_2_raio = arestasTemplateOriginal.FindAll(o=>o.id_aresta == raioAtual.id_barra1 || o.id_aresta == raioAtual.id_barra2).ToList();

                for (int cc = 0; cc < arestasTemplateOriginal.Count; cc++)
                {
                    if (arestasTemplateOriginal[cc].id_aresta == raioAtual.id_barra1 || arestasTemplateOriginal[cc].id_aresta == raioAtual.id_barra2)
                    {
                        if (Geom.Iguais(ponto_que_contem_o_raio.x, arestasTemplateOriginal[cc].p1.x)
                         && Geom.Iguais(ponto_que_contem_o_raio.y, arestasTemplateOriginal[cc].p1.y))
                        {
                            if (PA.vv == 1)
                            {
                                ponto_raio_contorno_interno = arestasTemplateOriginal[cc].contornoInterno;

                                PB = new vec3(arestasTemplateOriginal[cc].p2.x, arestasTemplateOriginal[cc].p2.y, 0, false, 1);
                                aresta_p3 = arestasTemplateOriginal[cc].id_aresta;
                            }
                            else
                            {
                                ponto_raio_contorno_interno = arestasTemplateOriginal[cc].contornoInterno;

                                PA = new vec3(arestasTemplateOriginal[cc].p2.x, arestasTemplateOriginal[cc].p2.y, 0, false, 1);
                                aresta_p2 = arestasTemplateOriginal[cc].id_aresta;
                            }
                        }
                        else
                        if (Geom.Iguais(ponto_que_contem_o_raio.x, arestasTemplateOriginal[cc].p2.x)
                         && Geom.Iguais(ponto_que_contem_o_raio.y, arestasTemplateOriginal[cc].p2.y))
                        {
                            if (PA.vv == 1)
                            {
                                ponto_raio_contorno_interno = arestasTemplateOriginal[cc].contornoInterno;

                                PB = new vec3(arestasTemplateOriginal[cc].p1.x, arestasTemplateOriginal[cc].p1.y, 0, false, 1);
                                aresta_p3 = arestasTemplateOriginal[cc].id_aresta;
                            }
                            else
                            {
                                ponto_raio_contorno_interno = arestasTemplateOriginal[cc].contornoInterno;

                                PA = new vec3(arestasTemplateOriginal[cc].p1.x, arestasTemplateOriginal[cc].p1.y, 0, false, 1);
                                aresta_p2 = arestasTemplateOriginal[cc].id_aresta;
                            }
                        }
                    }
                }

                if ( PB.vv != 2 )
                {
                    List<vec3> pts;

                    pts = GeradorDeFillet.CriarFillet(PA, ponto_que_contem_o_raio, PB, raioAtual.raio, 3);

                    int aresta_p_fim_raio = 0;
                    int aresta_p_inicio_raio = 0;

                    string segmento_pIni_Raio = GeradorDeFillet.QualSegmento(PA, ponto_que_contem_o_raio, PB, pts[0]);
                    if (segmento_pIni_Raio == "A")
                        aresta_p_inicio_raio = aresta_p2;
                    else
                    if (segmento_pIni_Raio == "B")
                        aresta_p_inicio_raio = aresta_p3;

                    string segmento_pFin_Raio = GeradorDeFillet.QualSegmento(PA, ponto_que_contem_o_raio, PB, pts[pts.Count - 1]);
                    if (segmento_pFin_Raio == "A")
                        aresta_p_fim_raio = aresta_p2;
                    else
                    if (segmento_pFin_Raio == "B")
                        aresta_p_fim_raio = aresta_p3;

                    /*   for (int j = 0; j < pts.Count; j++)
                           {
                               pts[j] -= zero;
                               pts[j] *= 10;
                           }*/

                    for (int j = 0; j < pts.Count - 1; j++)
                    {
                        arestasRaios.Add(new LinhaVec3(new vec3(pts[j].x, pts[j].y, 0, false , i), new vec3(pts[j + 1].x, pts[j + 1].y, 0, false, i), ponto_raio_contorno_interno, i));
                    }

                    infoSecao.raios[i].xCota = pts[2].x + 0.1;
                    infoSecao.raios[i].yCota = pts[2].y + 0.1;

                    infoSecao.raios[i].xr1 = pts[1].x; 
                    infoSecao.raios[i].yr1 = pts[1].y;
                    infoSecao.raios[i].xr2 = pts[2].x; 
                    infoSecao.raios[i].yr2 = pts[2].y;

                    for (int k = 0; k < arestasTemplateOriginal.Count; k++)
                    {
                        if (arestasTemplateOriginal[k].id_aresta == raioAtual.id_barra1 || arestasTemplateOriginal[k].id_aresta == raioAtual.id_barra2)
                        {
                            if (Geom.Iguais(arestasTemplateOriginal[k].p1.x, ponto_que_contem_o_raio.x) && Geom.Iguais(arestasTemplateOriginal[k].p1.y, ponto_que_contem_o_raio.y))
                            {
                                if (arestasTemplateOriginal[k].id_aresta == aresta_p_inicio_raio)
                                {
                                    arestasFinalGeometria_SemRaio[k].p1.x = pts[0].x;
                                    arestasFinalGeometria_SemRaio[k].p1.y = pts[0].y;
                                }
                                else
                                if (arestasTemplateOriginal[k].id_aresta == aresta_p_fim_raio)
                                {
                                    arestasFinalGeometria_SemRaio[k].p1.x = pts[pts.Count - 1].x;
                                    arestasFinalGeometria_SemRaio[k].p1.y = pts[pts.Count - 1].y;
                                }
                            }

                            if (Geom.Iguais(arestasTemplateOriginal[k].p2.x, ponto_que_contem_o_raio.x) && Geom.Iguais(arestasTemplateOriginal[k].p2.y, ponto_que_contem_o_raio.y))
                            {
                                if (arestasTemplateOriginal[k].id_aresta == aresta_p_inicio_raio)
                                {
                                    arestasFinalGeometria_SemRaio[k].p2.x = pts[0].x;
                                    arestasFinalGeometria_SemRaio[k].p2.y = pts[0].y;
                                }
                                else
                                if (arestasTemplateOriginal[k].id_aresta == aresta_p_fim_raio)
                                {
                                    arestasFinalGeometria_SemRaio[k].p2.x = pts[pts.Count - 1].x;
                                    arestasFinalGeometria_SemRaio[k].p2.y = pts[pts.Count - 1].y;
                                }
                            }
                        }
                    }
                }
            }
        }

        void CriarArestasFinalGeometria_SemRaio()
        {
            arestasFinalGeometria_SemRaio = new List<LinhaVec3>();
            for (int i = 0; i < arestasTemplateOriginal.Count; i++)
            {
                arestasFinalGeometria_SemRaio.Add(new LinhaVec3(new vec3(arestasTemplateOriginal[i].p1.x, arestasTemplateOriginal[i].p1.y, 0), 
                                                        new vec3(arestasTemplateOriginal[i].p2.x, arestasTemplateOriginal[i].p2.y, 0),
                                                        arestasTemplateOriginal[i].contornoInterno, 
                                                        arestasTemplateOriginal[i].raio, 
                                                        arestasTemplateOriginal[i].linhaAuxiliarSeparadora,
                                                        arestasTemplateOriginal[i].id_aresta));
            }
        }

        List<vec3> pontosArestasFinal;

        List<vec3> nos_ordenados, nos_ordenados_final = new List<vec3>();

        void GeometriaFinalOrdenadaCCW_ComRaios()
        {
            vec3 ultNo = new vec3(arestasFinalGeometria_SemRaio[0].p1.x, arestasFinalGeometria_SemRaio[0].p1.y, 0);
            vec3 NoAtual = new vec3(arestasFinalGeometria_SemRaio[0].p1.x, arestasFinalGeometria_SemRaio[0].p1.y, 0); 
            vec3 temp = new vec3(0);
            nos_ordenados = new List<vec3>();

            List<LinhaVec3> arestas_temp = new List<LinhaVec3>();
            for (int i = 0; i < arestasFinalGeometria_SemRaio.Count; i++)           
              arestas_temp.Add(new LinhaVec3(new vec3(arestasFinalGeometria_SemRaio[i].p1.x, arestasFinalGeometria_SemRaio[i].p1.y, 0),
                                                  new vec3(arestasFinalGeometria_SemRaio[i].p2.x, arestasFinalGeometria_SemRaio[i].p2.y, 0), 
                                             arestasFinalGeometria_SemRaio[i].contornoInterno, 
                                             arestasFinalGeometria_SemRaio[i].raio,
                                             arestasFinalGeometria_SemRaio[i].linhaAuxiliarSeparadora,
                                             arestasFinalGeometria_SemRaio[i].id_aresta));
            

            for (int i = 0; i < arestasRaios.Count; i++)
              arestas_temp.Add(new LinhaVec3(new vec3(arestasRaios[i].p1.x, arestasRaios[i].p1.y, 0),
                                                                new vec3(arestasRaios[i].p2.x, arestasRaios[i].p2.y, 0),
                                             arestasRaios[i].contornoInterno,
                                             arestasRaios[i].raio,
                                             arestasRaios[i].linhaAuxiliarSeparadora,
                                             arestasRaios[i].id_aresta));

            //------------------------//---------------//---------------------------//-----------------------
            List<LinhaVec3> arestas_externas = arestas_temp.FindAll(o=>o.contornoInterno == false && o.linhaAuxiliarSeparadora == false).ToList();

            arestasFinalGeometria = new List<LinhaVec3>();

            nos_ordenados = Geometry.RetornaContornoOrdenado(arestas_externas);
            nos_ordenados_final = nos_ordenados.ToList();
            for (int i = 0; i < nos_ordenados.Count; i++)
            {
                int prox = (i + 1) % nos_ordenados.Count;
                arestasFinalGeometria.Add(new LinhaVec3(new vec3(nos_ordenados[i].x, nos_ordenados[i].y, 0),
                                                        new vec3(nos_ordenados[prox].x, nos_ordenados[prox].y, 0),
                                                  false));
            }

            //adiciona arestas do contorno interno, se houver
            //(parede interna de tubos, perfil U duplo Caixão, viga I dupla etc) para ser utlizado na renderização depois,
            //ja que sao arestas separadas das outras arestas externas
            if (arestas_temp.Exists(o => o.contornoInterno == true))
            {
                List<LinhaVec3> arestas_internas = arestas_temp.FindAll(o => o.contornoInterno == true && o.linhaAuxiliarSeparadora == false).ToList();
                if (arestas_internas.Count > 0)
                {
                    nos_ordenados = Geometry.RetornaContornoOrdenado(arestas_internas);

                    nos_ordenados_final.AddRange(nos_ordenados);
                    for (int i = 0; i < nos_ordenados.Count; i++)
                    {
                        int prox = (i + 1) % nos_ordenados.Count;
                        arestasFinalGeometria.Add(new LinhaVec3(new vec3(nos_ordenados[i].x, nos_ordenados[i].y, 0),
                                                                        new vec3(nos_ordenados[prox].x, nos_ordenados[prox].y, 0),
                                                          true));
                    }
                }
            }

            List<LinhaVec3> arestas_auxliares_ou_separadoras = arestas_temp.FindAll(o => o.linhaAuxiliarSeparadora == true).ToList();
            for (int i = 0; i < arestas_auxliares_ou_separadoras.Count; i++)
                arestasFinalGeometria.Add(new LinhaVec3(new vec3(arestas_auxliares_ou_separadoras[i].p1.x, arestas_auxliares_ou_separadoras[i].p1.y, 0),
                                                                new vec3(arestas_auxliares_ou_separadoras[i].p2.x, arestas_auxliares_ou_separadoras[i].p2.y, 0),
                                                  false,-1,true,
                                                  arestas_auxliares_ou_separadoras[i].id_aresta));
            
        }

        bool[] arestaVisivel;
        List<int> cotasInvisiveis;
        List<int> raiosInvisiveis;

        void CriarCotas(bool abrindoTemplate)
        {
            arestasCotas = new List<LinhaVec3>();

            List<cotasSecao> infoCotas = infoSecao.cotas;
            cotasInvisiveis = new List<int>();

            for (int i = 0; i < infoCotas.Count; i++)
            {
                if (infoCotas[i].igualOutraCota)
                {
                    if (infoCotas[i].cotaIgualar1 != -1)
                        cotasInvisiveis.Add(infoCotas[i].cotaIgualar1);
                    if (infoCotas[i].cotaIgualar2 != -1)
                        cotasInvisiveis.Add(infoCotas[i].cotaIgualar2);
                    if (infoCotas[i].cotaIgualar3 != -1)
                        cotasInvisiveis.Add(infoCotas[i].cotaIgualar3);
                    if (infoCotas[i].cotaIgualar4 != -1)
                        cotasInvisiveis.Add(infoCotas[i].cotaIgualar4);
                }
            }

            if (abrindoTemplate)
            {
                for (int i = 0; i < infoCotas.Count; i++)
                {
                 //   if (cotasInvisiveis.Exists(o => o == i))
                 //       continue;

                    if (infoCotas[i].pontosDeslocar != null)
                    {
                        for (int j = 0; j < infoCotas[i].pontosDeslocar.Count; j++)
                        {
                            infoCotas[i].pontosDeslocar[j].p.x -= zero.x;
                            infoCotas[i].pontosDeslocar[j].p.y -= zero.y;
                            infoCotas[i].pontosDeslocar[j].p.x *= 10;
                            infoCotas[i].pontosDeslocar[j].p.y *= 10;

                            infoCotas[i].pontosDeslocar[j].vetor.x -= zero.x;
                            infoCotas[i].pontosDeslocar[j].vetor.y -= zero.y;
                            infoCotas[i].pontosDeslocar[j].vetor.x *= 10;
                            infoCotas[i].pontosDeslocar[j].vetor.y *= 10;
                        }
                    }

                    infoCotas[i].p1.x -= zero.x;
                    infoCotas[i].p1.y -= zero.y;
                    infoCotas[i].p2.x -= zero.x;
                    infoCotas[i].p2.y -= zero.y;
                    infoCotas[i].p1.x *= 10;
                    infoCotas[i].p1.y *= 10;
                    infoCotas[i].p2.x *= 10;
                    infoCotas[i].p2.y *= 10;

                    arestasCotas.Add(new LinhaVec3(new vec3(infoCotas[i].p1.x, infoCotas[i].p1.y, 0), new vec3(infoCotas[i].p2.x, infoCotas[i].p2.y, 0)));
                }
            }
            else
            {
                for (int i = 0; i < infoCotas.Count; i++)
                {
              //      if (cotasInvisiveis.Exists(o => o == i))
                 //      continue;
                    arestasCotas.Add(new LinhaVec3(new vec3(infoCotas[i].p1.x, infoCotas[i].p1.y, 0), new vec3(infoCotas[i].p2.x, infoCotas[i].p2.y, 0)));
                }
            }

            for (int i = 0; i < arestasCotas.Count; i++)
            {
                /*arestasCotas[i].p1.x -= zero.x;
                arestasCotas[i].p1.y -= zero.y;
                arestasCotas[i].p2.x -= zero.x;
                arestasCotas[i].p2.y -= zero.y;

                arestasCotas[i].p1.x *= 10;
                arestasCotas[i].p1.y *= 10;
                //  arestas[i].p1.x *= -1;

                arestasCotas[i].p2.x *= 10;
                arestasCotas[i].p2.y *= 10;*/
                //     arestas[i].p2.x *= -1;

                if (Geom.Iguais(arestasCotas[i].p1.x, 0))
                    arestasCotas[i].p1.x = 0;
                if (Geom.Iguais(arestasCotas[i].p1.y, 0))
                    arestasCotas[i].p1.y = 0;
                if (Geom.Iguais(arestasCotas[i].p2.x, 0))
                    arestasCotas[i].p2.x = 0;
                if (Geom.Iguais(arestasCotas[i].p2.y, 0))
                    arestasCotas[i].p2.y = 0;
            }


            CalculaValorCotas();


            cotasMarcar = new bool[arestasCotas.Count];

            //offset na cota para um lado que nao intercepte as arestas da secao

            PontoD ptb = new PontoD(), pta = new PontoD(), ptc = new PontoD(), ptd = new PontoD(), intersec = new PontoD();
            vec3 vecOrto = new vec3(0);
            vec3 pa, pb;
            double offset = 5;
            for (int i = 0; i < arestasCotas.Count; i++)
            {
                if (valCotas[i] > 150)
                    offset = 20;
                (pa, pb) = Geometry.OffsetLine(arestasCotas[i].p1, arestasCotas[i].p2, offset);

                for (int p = 0; p < arestasTemplateOriginal.Count; p++)
                {
                    pta.x = arestasTemplateOriginal[p].p1.x;
                    pta.y = arestasTemplateOriginal[p].p1.y;
                    ptb.x = arestasTemplateOriginal[p].p2.x;
                    ptb.y = arestasTemplateOriginal[p].p2.y;

                    ptc.x = pa.x;
                    ptc.y = pa.y;
                    ptd.x = pb.x;
                    ptd.y = pb.y;

                    if (Geom.calcIntersecEQU_RETA(ref pta, ref ptb, ref ptc, ref ptd, ref intersec))
                    {
                        (pa, pb) = Geometry.OffsetLine(arestasCotas[i].p1, arestasCotas[i].p2, -offset);
                        break;
                    }
                }

                arestasCotas[i].p1.x = pa.x;
                arestasCotas[i].p1.y = pa.y;

                arestasCotas[i].p2.x = pb.x;
                arestasCotas[i].p2.y = pb.y;
            }
           

            CriarMarcacoesCotas();

            CalculaAnguloCotas();

        }

        void CriarMarcacoesCotas()
        {
            marcacoesCotas = new List<LinhaVec3>();
            int maxCotas = arestasCotas.Count;
            //marcacoes das cotas
            for (int i = 0; i < maxCotas; i++)
            {
                if (cotasInvisiveis.Exists(o => o == i))
                  continue;
                
                double tamTick1 = 3;

                if (valCotas[i] < 5)
                    tamTick1 = 2;
               /* Geometry.CriarMarcacoesDiagonaisCota(arestasCotas[i].p1, arestasCotas[i].p2, tamTick1, out var tick1, out var tick2);

                marcacoesCotas.Add(new LinhaVec3(tick1.a, tick1.b));
                marcacoesCotas.Add(new LinhaVec3(tick2.a, tick2.b));*/

                Geometry.CriarMarcacoesCota(arestasCotas[i].p1, arestasCotas[i].p2, tamTick1, out var tick3, out var tick4);
                marcacoesCotas.Add(new LinhaVec3(tick3.a, tick3.b));
                marcacoesCotas.Add(new LinhaVec3(tick4.a, tick4.b));
            }
        }
        
        int cotaSelecionada, raioSelecionado;
        private void glControl_DoubleClick(object sender, EventArgs e)
        {
            cotaSelecionada = -1;

            if (!cotasMarcar.All(o => o == false))
            {
                for (int i = 0; i < arestasCotas.Count; i++)
                {
                    if (cotasInvisiveis.Exists(o => o == i))
                      continue;
                   
                    if (cotasMarcar[i])
                    {
                        cotaSelecionada = i;
                        pnCota.Visible = true;
                        pnCota.Left = mouseX - 15; pnCota.Top = mouseY;
                        glControl.SwapBuffers();
                        edCota.Focus();
                        edCota.Text = valCotas[i].ToString("n1");
                        edCota.SelectAll();
                    }
                }

                DesenhaObjetos();
                glControl.SwapBuffers();
            }

            if (!raiosMarcar.All(o => o == false))
            {
                for (int i = 0; i < infoSecao.raios.Count; i++)
                {
                    if (raiosMarcar[i])
                    {
                        raioSelecionado = i;
                        pnCota.Visible = true;
                        pnCota.Left = mouseX - 15; pnCota.Top = mouseY;
                        glControl.SwapBuffers();
                        edCota.Focus();
                        edCota.Text = (infoSecao.raios[i].raio).ToString("n1");
                        edCota.SelectAll();
                    }
                }

                DesenhaObjetos();
                glControl.SwapBuffers();
            }
        }

        private void button10_Click(object sender, EventArgs e)
        {
            pnCota.Visible = false;
        }

        private void edCota_KeyPress(object sender, System.Windows.Forms.KeyPressEventArgs e)
        {
            char a = Convert.ToChar(System.Threading.Thread.CurrentThread.CurrentCulture.NumberFormat.NumberDecimalSeparator);
            if (!char.IsControl(e.KeyChar) && !char.IsDigit(e.KeyChar) && (e.KeyChar != a) && (e.KeyChar != '-'))
            {
                e.Handled = true;
            }

            if (e.KeyChar == (char)Keys.Enter || e.KeyChar == (char)Keys.Escape)
            {
                e.Handled = true;   // parar barulho chato
            }
            base.OnKeyPress(e);
        }


        private void edCota_KeyUp(object sender, KeyEventArgs e)
        {
            if (edCota.Text.Trim() != string.Empty && edCota.Text.Trim() != "-")
            {
                double val = double.Parse(edCota.Text);

                if (e.KeyData == Keys.Enter && val > 0)
                {
                    if (cotaSelecionada != -1)
                      AlterarCota(cotaSelecionada, val);
                    else 
                    if (raioSelecionado != -1)
                      AlterarRaio(raioSelecionado, val);
                }
            }
        }

        void AlterarRaio(int raio, double val)
        {
            pnCota.Visible = false;
            alterouGeometria = true;

            infoSecao.raios[raio].raio = val;
            
            if (infoSecao.raios[raio].raios_igualar != null)
            {
                for (int i = 0; i < infoSecao.raios[raio].raios_igualar.Count; i++)
                {
                    infoSecao.raios[infoSecao.raios[raio].raios_igualar[i]].raio = val;
                }
            }
            
            if (infoSecao.raios[raio].igualOutroRaio)
            {
                for (int hh = 0; hh < 4; hh++)
                {
                    int ci = -1;
                    if (hh == 0 && infoSecao.raios[raio].raioIgualar1 != -1)
                        ci = infoSecao.raios[raio].raioIgualar1;
                    else
                    if (hh == 1 && infoSecao.raios[raio].raioIgualar2 != -1)
                        ci = infoSecao.raios[raio].raioIgualar2;
                    else
                    if (hh == 2 && infoSecao.raios[raio].raioIgualar3 != -1)
                        ci = infoSecao.raios[raio].raioIgualar3;
                    else
                    if (hh == 3 && infoSecao.raios[raio].raioIgualar4 != -1)
                        ci = infoSecao.raios[raio].raioIgualar4;

                    if (ci != -1)
                        infoSecao.raios[ci].raio = val;
                }
            }

            CriarRaios(false);
            GeometriaFinalOrdenadaCCW_ComRaios();

            apagarCelulas();

            AtualizaShaders(false);

            DesenhaObjetos();
            glControl.SwapBuffers();
        }

        private void btConfirmaCota_Click(object sender, EventArgs e)
        {
            if (edCota.Text.Trim() != string.Empty)
            {
                double val = double.Parse(edCota.Text);

                if (val > 0 && (cotaSelecionada != -1))
                    AlterarCota(cotaSelecionada, val);
                else
                if (val > 0 && (raioSelecionado != -1))
                    AlterarRaio(raioSelecionado, val);
            }
        }

        private void FCalculaSecao_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyData == Keys.Escape && pnCota.Visible)
            { 
                pnCota.Visible = false;
                glControl.Focus();
            }
            else
            if (e.KeyData == Keys.Escape && !pnCota.Visible)
                button3_Click_1(button3, null);
        }

        private void edCota_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyData == Keys.Escape)
            {
                pnCota.Visible = false;
                glControl.Focus();
            }
        }

        private void glControl_Click(object sender, EventArgs e)
        {
          
            glControl.MakeCurrent();

        }

        private void glControl_Paint(object sender, PaintEventArgs e)
        {

        }

        private void chOcultarCotas_CheckedChanged(object sender, EventArgs e)
        {
            AtualizaShaders(false);
            DesenhaObjetos();
            glControl.SwapBuffers();
        }

        private void button3_Click_1(object sender, EventArgs e)
        {
            if (calculoOK)
            {

            }
            this.Close();
        }
        System.Windows.Forms.ToolTip tipBotoes = new System.Windows.Forms.ToolTip();
        private void edA_MouseEnter(object sender, EventArgs e)
        {
            System.Windows.Forms.TextBox txt = (System.Windows.Forms.TextBox)sender;

            tipBotoes.SetToolTip(txt, txt.AccessibleName);
        }

        private void button2_Click_1(object sender, EventArgs e)
        {
            glControl.MakeCurrent();
            Enquadrar();
        }

        void Salvar()
        {
            ExtrairLoops();

            if (alterando)
            {
               // secao.propriedades = calculoSecao.propriedades;
                secao.descricao = edDescricao.Text;
                secao.alterou = true;
                if (alterouGeometria)
                  secao.poligonos = poligonos;

                dadosBarra.alterando = true;

            }
            else
            {
                secao = new TSecao(poligonos, edDescricao.Text, template);

                if (gerenciador.formDesenho.Secoes.Count == 0)
                    secao.id = 0;
                else
                    secao.id = gerenciador.formDesenho.Secoes.Max(o => o.id) + 1;
            }

                secao.propriedades = calculoSecao.propriedades;
                
                //inercia usada no calculo
                secao.inercia_flexao_y = calculoSecao.propriedades.inercia_flexao_y; 
                secao.inercia_flexao_z = calculoSecao.propriedades.inercia_flexao_z;

                if (calculoSecao_EixosLocais != null)
                {
                    secao.propriedades_eixos_principais = calculoSecao_EixosLocais.propriedades;

                    //inercia usada no calculo
                    secao.inercia_flexao_y = calculoSecao_EixosLocais.propriedades.inercia_flexao_y;
                    secao.inercia_flexao_z = calculoSecao_EixosLocais.propriedades.inercia_flexao_z;

                    // secao.inercia_flexao_y = calculoSecao.propriedades.inercia_flexao_y;
                    // secao.inercia_flexao_z = calculoSecao.propriedades.inercia_flexao_z;

                    // secao.propriedades.inercia_flexao_y = calculoSecao_EixosLocais.propriedades.inercia_flexao_y;
                    //     secao.propriedades.inercia_flexao_z = calculoSecao_EixosLocais.propriedades.inercia_flexao_z;
                }
            

            secao.valoresCotas = valCotas.ToList();
            secao.valoresRaios = new List<double>();
            for (int i = 0; i < infoSecao.raios.Count; i++)
              secao.valoresRaios.Add(infoSecao.raios[i].raio);

            secao.Rgb[0] = (btCor.BackColor.R);
            secao.Rgb[1] = (btCor.BackColor.G);
            secao.Rgb[2] = (btCor.BackColor.B);
            secao.idMaterial = cbMaterial.SelectedIndex;
            secao.material = gerenciador.ConfiguracoesPGi.CfgProjeto.materiais[cbMaterial.SelectedIndex];
            secao.PesoProprio = (secao.material.PesoEspecifico * (secao.propriedades.area / 1000000)) / 100; //kf/f para kn/m
            dadosBarra.secaoSemRotacao = secao;
  
            if (!alterando)
            {
                gerenciador.formDesenho.Secoes.Add(secao);
                dadosBarra.secaoSemRotacao = secao;
            }
            else
            {
                int indice = gerenciador.formDesenho.Secoes.IndexOf(secao);
                dadosBarra.cbSecoes.SelectedIndex = indice;
                dadosBarra.secaoSemRotacao = secao;
            }

            dadosBarra.CarregaSecoes();

            if (!alterando)
            {
                dadosBarra.cbSecoes.Select();
                dadosBarra.secaoSemRotacao = (TSecao)secao.Clone();
                dadosBarra.cbSecoes.SelectedIndex = dadosBarra.cbSecoes.Items.Count - 1;
                dadosBarra.lbSecoes_Click(dadosBarra.cbSecoes, null);
            }

            dadosBarra.AtualizaSecoesEstrutura(secao.id);

            gerenciador.formDesenho.AtualizaBarras();
            gerenciador.formDesenho.Alterou(true, false);

            dadosBarra.RodarSecao();
            dadosBarra.DesenhaSecao();

            if (escolheSecao != null)
            {
                escolheSecao.lbSecoes.Items.Clear();
                for (int i = 0; i < dadosBarra.cbSecoes.Items.Count; i++)
                    escolheSecao.lbSecoes.Items.Add(dadosBarra.cbSecoes.Items[i]);
            }
        }
        public FDadosBarra dadosBarra;
        public FEscolheSecao escolheSecao;
        void ExtrairLoops()
        {
            List<LinhaVec3> linhas = new List<LinhaVec3>();
            List<vec3> nos = new List<vec3>();
            for (int i = 0; i < arestasFinalGeometria.Count; i++)
            {

                if (arestasFinalGeometria[i].linhaAuxiliarSeparadora)
                    continue;

                int p1 = LocalizaPonto(ref nos, arestasFinalGeometria[i].p1.x, arestasFinalGeometria[i].p1.y, 0);
                int p2 = LocalizaPonto(ref nos, arestasFinalGeometria[i].p2.x, arestasFinalGeometria[i].p2.y, 0);
                if ( p1== -1)
                {
                    arestasFinalGeometria[i].p1.id = nos.Count + 1;
                    p1 = nos.Count;
                    nos.Add(arestasFinalGeometria[i].p1);
                }

                if (p2 == -1)
                {
                    arestasFinalGeometria[i].p2.id = nos.Count + 1;
                    p2 = nos.Count;
                    nos.Add(arestasFinalGeometria[i].p2);
                }

                linhas.Add(new LinhaVec3(nos[p1],nos[p2],false,-1,false,linhas.Count+1,-1,-1));
            }

            List<List<GeracaoLoopsGeometria.HalfEdge>> loops = LoopExtractor.ExtrairLoops(linhas);

            Dictionary<int, vec3> nodes = new Dictionary<int, vec3>();

            foreach (LinhaVec3 linha in linhas)
            {
                if (!nodes.ContainsKey(linha.p1.id))
                    nodes.Add(linha.p1.id, linha.p1);

                if (!nodes.ContainsKey(linha.p2.id))
                    nodes.Add(linha.p2.id, linha.p2);
            }

            int contador = 1;
            poligonos = new List<TPoligono>();

            List<LoopInfo> infos = new List<LoopInfo>();

            foreach (var loop in loops)
            {
                infos.Add(new LoopInfo
                {
                    edges = loop,
                    area = LoopExtractor.CalcularArea(loop,nodes)
                });
            }

            LoopInfo externo = infos.OrderByDescending(x => x.area).First();
            externo.externo = true;
            bool Secao_com_diametro = infoSecao.cotas.Exists(o => o.cotaDiametro);

            for (int i = 0; i < infos.Count; i++)
            {
                if (infos[i].area > 0)
                {
                    coordsPoligono = new CoordenadaD[infos[i].edges.Count+1];
                    int l = 0;
                    bool pt_raio = false;
                    foreach (GeracaoLoopsGeometria.HalfEdge he in infos[i].edges)
                    {
                        pt_raio = false;

                        if (Secao_com_diametro)
                            pt_raio = true;
                        else
                        {
                            for (int r = 0; r < infoSecao.raios.Count; r++)
                            {
                                //marco como ponto em raio as coordendas interna do raio, menos as extremas, pq eu nao quero arestas no meio do raio, senao fica poluída a renderização
                                if (Geom.Iguais(nodes[he.origem].x, infoSecao.raios[r].xr1) || Geom.Iguais(nodes[he.origem].y, infoSecao.raios[r].yr1))
                                {
                                    pt_raio = true;
                                    break;
                                }
                                if (Geom.Iguais(nodes[he.origem].x, infoSecao.raios[r].xr2) || Geom.Iguais(nodes[he.origem].y, infoSecao.raios[r].yr2))
                                {
                                    pt_raio = true;
                                    break;
                                }
                            }
                        }
                        coordsPoligono[l++] = new CoordenadaD(nodes[he.origem].x / 1000, nodes[he.origem].y / 1000, pt_raio);
                    }

                    if (coordsPoligono.Count() > 0) //adicionar o primeiro nó como ultimo para fechar o poligono
                        coordsPoligono[l++] = new CoordenadaD(coordsPoligono[0].X, coordsPoligono[0].Y, coordsPoligono[0].pontoEmRaio);

                    poligonos.Add(new TPoligono(coordsPoligono));
                    poligonos[poligonos.Count - 1].externo = infos[i].externo;
                }
            }
        }

        CoordenadaD[] coordsPoligono;
        List<TPoligono> poligonos;
        
        bool Verificacoes()
        {
            if (cbMaterial.SelectedIndex == -1)
            {
                MessageBox.Show("O material não foi informado.", "Atenção", MessageBoxButtons.OK, MessageBoxIcon.Stop);
                return false;
            }
            bool concreto = (caminho_templateAtual.Contains("Concreto"));

            if (!concreto)
            {
                if (gerenciador.ConfiguracoesPGi.CfgProjeto.materiais[cbMaterial.SelectedIndex].Tipo == 1)
                {
                    MessageBox.Show("Selecione um material de aço na opção 'Material'", "Atenção", MessageBoxButtons.OK, MessageBoxIcon.Stop);
                    cbMaterial.Focus();
                    return false ;
                }
            }
            else
            {
                if (gerenciador.ConfiguracoesPGi.CfgProjeto.materiais[cbMaterial.SelectedIndex].Tipo == 0)
                {
                    MessageBox.Show("Selecione um material de concreto na opção 'Material'.", "Atenção", MessageBoxButtons.OK, MessageBoxIcon.Stop);
                    cbMaterial.Focus();
                    return false;
                }
            }

            return true;
        }

        private void button4_Click(object sender, EventArgs e)
        {
            if (!Verificacoes())
                return;

            if (Recalcular)
              CalculaSecao();

            Salvar();

            DialogResult = DialogResult.OK;
        }

        private void button2_MouseEnter(object sender, EventArgs e)
        {
            System.Windows.Forms.Button btn = (System.Windows.Forms.Button)sender;

            tipBotoes.SetToolTip(btn, btn.AccessibleName);
        }

        private void btCor_Click(object sender, EventArgs e)
        {
            if (colorDialog1.ShowDialog() == DialogResult.OK)
                btCor.BackColor = colorDialog1.Color;
        }
       
        double TamanhoCelula(int qtd_desejadas_celulas)
        {
            double areaAproxPoligono = Geometry.AreaPoligono_Shoealace(nos_ordenados_final);

            if (tuboRedondoVazado)
                areaAproxPoligono = ((3.1415*Math.Pow(valCotas[0]/2,2)) - (3.1415 * Math.Pow(valCotas[1]/2, 2)));

            double menorCota = double.MaxValue;
            for (int i = 0; i < valCotas.Count; i++)
                if (!infoSecao.cotas[i].desconsiderarNaMalha)
                  if (valCotas[i] < menorCota)
                     menorCota = valCotas[i];

            double tamCelulaAprox = Math.Sqrt(areaAproxPoligono / qtd_desejadas_celulas);
            double tamCelulaMenorCota = menorCota / 2.4;

            if (template == "Diversos\\sec1" || template == "Diversos\\sec2") //tubo retangular vazado
                tamCelulaMenorCota /= 1.5;

       //     if ()
            if (infoSecao.cotas.Exists(o => o.cotaDiametro))
                return tamCelulaAprox;


            /*if (templateAtual.Contains("Dobrado"))
            {
                return ;
            }
            else
            {
*/
             return Math.Min(tamCelulaAprox, tamCelulaMenorCota);
           // }
        }
        List<Celula> celulas;
        void GerarMalha()
        {
            double tam = TamanhoCelula(1400);

            TGeracaoMalha2D gerMalha = new TGeracaoMalha2D(arestasFinalGeometria, nos_ordenados_final, tam);
          
            gerMalha.Malha(ref celulas);
            nosMalha = gerMalha.nosMalha;

            lbTotCelulas.Text = "Total elementos: " + celulas.Count.ToString();
            edTamCelula.Text  = tam.ToString("n2");
        }

        public List<Q4_Secao> quad4;
        public List<T6_Secao> tris6;
        public List<TNoMEF> nosElementos;
        List<NoCelula> nosMalha;
        public void CriarElementos()
        {
            TConverterMalhaEmElementosFinitos geraElem = new TConverterMalhaEmElementosFinitos(celulas);
            geraElem.Gerar();

            quad4        = geraElem.quad4.ToList();
            tris6        = geraElem.tris6.ToList();
            nosElementos = geraElem.nosElementos;
        }

        TCalculoSecao calculoSecao, calculoSecao_EixosLocais;
        void chamaAguardar(string texto)
        {
            PanelAguarde.Left = this.Width / 2 - groupBox1.Width;
            PanelAguarde.Top = this.Height - PanelAguarde.Height*2 - panel1.Height*2;
            
            PanelAguarde.Refresh();
            PanelAguarde.Visible = true;
            PanelAguarde.Refresh();
            LabelAguarde.Text = texto;
            PanelAguarde.Refresh();

        }
        void CalculaSecao()
        {
            try 
            {
                chamaAguardar("Gerando malha de elementos. Aguarde...");
                GerarMalha();

                chamaAguardar("Calculando seção. Aguarde...");
                CriarElementos();
                ResolveSecao();

                PanelAguarde.Visible = false;
            }
            catch (Exception e)
            {
                PanelAguarde.Visible = false;
            }

        }
        T6_Secao[] _tris6;
        Q4_Secao[] _quad4;
        TNoMEF[] noselem;
        void ResolveSecao()
        {
            _tris6 = new T6_Secao[tris6.Count + 1];
            for (int i = 0; i < tris6.Count; i++)
            {
                _tris6[i + 1] = tris6[i];
                _tris6[i + 1].id = tris6[i].id + 1;
            }

            _quad4 = new Q4_Secao[quad4.Count + 1];
            for (int i = 0; i < quad4.Count; i++)
            {
                _quad4[i + 1] = quad4[i];
                _quad4[i + 1].id = quad4[i].id + 1;
            }

            noselem = new TNoMEF[nosElementos.Count + 1];
            for (int i = 0; i < nosElementos.Count; i++)
            {
                noselem[i + 1] = nosElementos[i];
                noselem[i + 1].numero = nosElementos[i].numero;
            }

            calculoSecao = new TCalculoSecao(gerenciador, _tris6, _quad4, true, noselem);

            //  calculoSecao.ixx = ixx;
            //   calculoSecao.iyy = iyy;
            calculoSecao.Calcula_Area_Qx_Qy();

            xc = calculoSecao.propriedades.primeiro_momento_area_z / calculoSecao.propriedades.area;
            yc = calculoSecao.propriedades.primeiro_momento_area_y / calculoSecao.propriedades.area;
         
            CentralizarSecaoCalculada(xc, yc);

            for (int i = 1; i <= calculoSecao.nosElementos.Count()-1; i++)
            {
                calculoSecao.nosElementos[i].x -= xc; calculoSecao.nosElementos[i].y -= yc;
            }

            calculoSecao.Calcula_Area_Qx_Qy();

            calculoSecao.Calcula(true, true, true);

            AvaliaSimetria();

            AtualizaGrid();

            if (flatTabControl1.TabPages.Count == 2)
                flatTabControl1.TabPages.RemoveAt(0);

            flatTabControl1.TabPages.RemoveAt(0);

            flatTabControl1.TabPages.Add(paginas[0]);

            if (!Geom.Iguais(calculoSecao.propriedades.prod_inercia,0))
            {
                CalculaSecao_EixosPrincipais();

                flatTabControl1.TabPages.Add(paginas[1]);
            }
        }
        List<TabPage> paginas;

        void CalculaSecao_EixosPrincipais()
        {
           /* for (int i = 0; i < nosMalha.Count; i++)
            {
                vec3 vr = new vec3(nosMalha[i].x, nosMalha[i].y,0);
                vr.Rotate(-calculoSecao.propriedades.anguloEixosPrincipais);
                nosMalha[i].x = vr.x; nosMalha[i].y = vr.y;
            }*/

            for (int i = 0; i < nosElementos.Count; i++)
            {
                vec3 vr = new vec3(nosElementos[i].x, nosElementos[i].y, 0);
                vr.Rotate(calculoSecao.propriedades.anguloEixosPrincipais);

                nosElementos[i].x = vr.x; nosElementos[i].y = vr.y;
            }

            noselem = new TNoMEF[nosElementos.Count + 1];
            for (int i = 0; i < nosElementos.Count; i++)
            {
                noselem[i + 1] = nosElementos[i];
                noselem[i + 1].numero = nosElementos[i].numero;
            }

            calculoSecao_EixosLocais = new TCalculoSecao(gerenciador, _tris6, _quad4, true, noselem);

            calculoSecao_EixosLocais.Calcula_Area_Qx_Qy();

            calculoSecao_EixosLocais.Calcula(true, true, true);
            calculoSecao_EixosLocais.propriedades.SimetriaY = calculoSecao.propriedades.SimetriaY;
            calculoSecao_EixosLocais.propriedades.SimetriaZ = calculoSecao.propriedades.SimetriaZ;
            calculoSecao_EixosLocais.propriedades.inercia_torcao = calculoSecao.propriedades.inercia_torcao;

            AtualizaGrid_EixosLocais();
        }

        void AtualizaGrid_EixosLocais()
        {
            props_eixosPrincipais = new List<Linha_propriedades>();
            props_eixosPrincipais.Add(new Linha_propriedades("Iu [cm4]", (calculoSecao_EixosLocais.propriedades.inercia_flexao_y / 10000).ToString("n3"), "Momento de inércia em torno do eixo principal u"));
            props_eixosPrincipais.Add(new Linha_propriedades("Iv [cm4]", (calculoSecao_EixosLocais.propriedades.inercia_flexao_z / 10000).ToString("n3"), "Momento de inércia em torno do eixo principal v"));
            props_eixosPrincipais.Add(new Linha_propriedades("Ru [mm]", (calculoSecao_EixosLocais.propriedades.raio_giracao_y).ToString("n1"), "Raio de giração em torno do eixo principal u"));
            props_eixosPrincipais.Add(new Linha_propriedades("Rv [mm]", (calculoSecao_EixosLocais.propriedades.raio_giracao_z).ToString("n1"), "Raio de giração em torno do eixo principal v"));
            props_eixosPrincipais.Add(new Linha_propriedades("Wu inf. [cm³]", (calculoSecao_EixosLocais.propriedades.ModuloElastico_y_inf / 1000).ToString("n2"), "Módulo elástico u - inferior"));
            props_eixosPrincipais.Add(new Linha_propriedades("Wu sup. [cm³]", (calculoSecao_EixosLocais.propriedades.ModuloElastico_y_sup / 1000).ToString("n2"), "Módulo elástico u - superior"));
            props_eixosPrincipais.Add(new Linha_propriedades("Wv dir. [cm³]", (calculoSecao_EixosLocais.propriedades.ModuloElastico_z_dir / 1000).ToString("n2"), "Módulo elástico v - direita"));
            props_eixosPrincipais.Add(new Linha_propriedades("Wv esq. [cm³]", (calculoSecao_EixosLocais.propriedades.ModuloElastico_z_esq / 1000).ToString("n2"), "Módulo elástico v - esquerda"));
            props_eixosPrincipais.Add(new Linha_propriedades("Zu [cm³]", (calculoSecao_EixosLocais.propriedades.ModuloPlastico_y / 1000).ToString("n2"), "Módulo plástico u"));
            props_eixosPrincipais.Add(new Linha_propriedades("Zv [cm³]", (calculoSecao_EixosLocais.propriedades.ModuloPlastico_z / 1000).ToString("n2"), "Módulo plástico v"));
            props_eixosPrincipais.Add(new Linha_propriedades("Acu [cm²]", (calculoSecao_EixosLocais.propriedades.areaCisalhamentoY / 100).ToString("n2"), "Área de cisalhamento na direção u"));
            props_eixosPrincipais.Add(new Linha_propriedades("Acv [cm²]", (calculoSecao_EixosLocais.propriedades.areaCisalhamentoZ / 100).ToString("n2"), "Área de cisalhamento na direção v"));

            gridEixosPrincipais.Columns[0].DataPropertyName = "propriedade";
            gridEixosPrincipais.Columns[0].HeaderText = "Propriedade";
            gridEixosPrincipais.Columns[1].HeaderText = "Valor";

            gridEixosPrincipais.Columns[1].DataPropertyName = "val";

            bindingSource2.DataSource = props_eixosPrincipais;
            gridEixosPrincipais.DataSource = bindingSource2;// gerenciador.formDesenho.Estrutura.combinacoes[lbComb.SelectedIndex].Coeficientes;
        }

        double xc, yc;
        bool sy, sz;
        List<vec3> nos_Rotacionados;
        void AvaliaSimetria()
        {
            LinhaVec3 [] contornoRotacionado = new LinhaVec3[arestasFinalGeometria.Count];
            nos_Rotacionados = new List<vec3>();
            contornoRotacionado = arestasFinalGeometria.ToArray();

            for (int i = 0; i < contornoRotacionado.Count(); i++)
            {
                vec3 p1 = new vec3(arestasFinalGeometria[i].p1.x, arestasFinalGeometria[i].p1.y, 0);
                vec3 p2 = new vec3(arestasFinalGeometria[i].p2.x, arestasFinalGeometria[i].p2.y, 0);

                if (!nos_Rotacionados.Exists(o=>o.x == p1.x && o.y == p1.y))
                    nos_Rotacionados.Add(new vec3(p1.x, p1.y, 0));

                if (!nos_Rotacionados.Exists(o => o.x == p2.x && o.y == p2.y))
                    nos_Rotacionados.Add(new vec3(p2.x, p2.y, 0));
            }

            calculoSecao.AnguloEixosPrincipais();

            for (int i = 0; i < nos_Rotacionados.Count; i++)
                nos_Rotacionados[i].Rotate(-calculoSecao.propriedades.anguloEixosPrincipais);

            sy = SimetriaY(nos_Rotacionados);
            sz = SimetriaZ(nos_Rotacionados);

            if (sy)
                calculoSecao.propriedades.SimetriaY = "Sim"; else calculoSecao.propriedades.SimetriaY = "Não";
            
            if (sz)
                calculoSecao.propriedades.SimetriaZ = "Sim"; else calculoSecao.propriedades.SimetriaZ = "Não";

        }

        public static bool SimetriaY(List<vec3> nodes, double EPS = 0.1)
        {
            foreach (var p in nodes)
            {
                bool found = false;

                foreach (var q in nodes)
                {
                    if (Math.Abs(q.x - p.x) < EPS &&
                        Math.Abs(q.y + p.y) < EPS)
                    {
                        found = true;
                        break;
                    }
                }

                if (!found)
                    return false;
            }

            return true;
        }

        public static bool SimetriaZ(List<vec3> nodes, double EPS = 0.1)
        {
            foreach (var p in nodes)
            {
                bool found = false;

                foreach (var q in nodes)
                {
                    if (Math.Abs(q.x + p.x) < EPS &&
                        Math.Abs(q.y - p.y) < EPS)
                    {
                        found = true;
                        break;
                    }
                }

                if (!found)
                    return false;
            }

            return true;
        }

        List<Linha_propriedades> props, props_eixosPrincipais;
        TPropriedades_Secao propriedades_calculadas;
        double anguloEixosPrincipais;
        void AtualizaGrid()
        {
            props = new List<Linha_propriedades>();
            props.Add(new Linha_propriedades("Área [cm²]", (calculoSecao.propriedades.area / 100).ToString("n2"),""));
            props.Add(new Linha_propriedades("cY [mm]", (calculoSecao.propriedades.cy).ToString("n1"), "Distância Y do centróide até a fibra mais a esquerda da seção"));
            props.Add(new Linha_propriedades("cZ [mm]", (calculoSecao.propriedades.cz).ToString("n1"), "Distância Z do centróide até a fibra mais baixa da seção"));

            props.Add(new Linha_propriedades("Iy [cm4]", (calculoSecao.propriedades.inercia_flexao_y / 10000).ToString("n3"), "Momento de inércia em torno de y"));
            props.Add(new Linha_propriedades("Iz [cm4]", (calculoSecao.propriedades.inercia_flexao_z / 10000).ToString("n3"), "Momento de inércia em torno de z"));
            props.Add(new Linha_propriedades("It [cm4]", (calculoSecao.propriedades.inercia_torcao / 10000).ToString("n2"), "Momento de inércia a torção"));
            props.Add(new Linha_propriedades("Iyz [cm4]", (calculoSecao.propriedades.prod_inercia /10000).ToString("n2"), "Produto de inércia"));
            props.Add(new Linha_propriedades("Ry [mm]", (calculoSecao.propriedades.raio_giracao_y).ToString("n1"), "Raio de giração em torno de y"));
            props.Add(new Linha_propriedades("Rz [mm]", (calculoSecao.propriedades.raio_giracao_z).ToString("n1"), "Raio de giração em torno de z"));

            props.Add(new Linha_propriedades("Wy inf. [cm³]", (calculoSecao.propriedades.ModuloElastico_y_inf / 1000).ToString("n2"), "Módulo elástico y - inferior"));
            props.Add(new Linha_propriedades("Wy sup. [cm³]", (calculoSecao.propriedades.ModuloElastico_y_sup / 1000).ToString("n2"), "Módulo elástico y - superior"));

            props.Add(new Linha_propriedades("Wz dir. [cm³]", (calculoSecao.propriedades.ModuloElastico_z_dir / 1000).ToString("n2"), "Módulo elástico z - direita"));
            props.Add(new Linha_propriedades("Wz esq. [cm³]", (calculoSecao.propriedades.ModuloElastico_z_esq / 1000).ToString("n2"), "Módulo elástico z - esquerda"));

            props.Add(new Linha_propriedades("Zy [cm³]", (calculoSecao.propriedades.ModuloPlastico_y / 1000).ToString("n2"), "Módulo plástico y"));
            props.Add(new Linha_propriedades("Zz [cm³]", (calculoSecao.propriedades.ModuloPlastico_z / 1000).ToString("n2"), "Módulo plástico z"));
            props.Add(new Linha_propriedades("D. eixo pl. centróide-y [mm]", (calculoSecao.propriedades.dist_modulo_plastico_y_do_centroide).ToString("n1"), "Deslocamento do eixo neutro plástico até o centróide na direção y"));
            props.Add(new Linha_propriedades("D. eixo pl. centróide-z [mm]", (calculoSecao.propriedades.dist_modulo_plastico_z_do_centroide).ToString("n1"), "Deslocamento do eixo neutro plástico até o centróide na direção z"));

            anguloEixosPrincipais = calculoSecao.AnguloEixosPrincipais();
            
            if (Geom.Iguais(anguloEixosPrincipais, 0, 0.1))
                anguloEixosPrincipais = 0;

            rot_eixo_u = (new vec3(15, 0, 0)).Rotate(-anguloEixosPrincipais);
            rot_eixo_v = (new vec3(0, -15, 0)).Rotate(-anguloEixosPrincipais);

            props.Add(new Linha_propriedades("α u,v [graus]", (anguloEixosPrincipais /Const.PIDiv180).ToString("n1"), "Ângulo de rotação dos eixos principais u,v"));

            props.Add(new Linha_propriedades("Desloc. y até c.c [mm]", (calculoSecao.propriedades.centro_cis_y).ToString("n1"), "Deslocamento Y do centróide até o centro de cisalhamento"));
            props.Add(new Linha_propriedades("Desloc. z até c.c [mm]", (calculoSecao.propriedades.centro_cis_z).ToString("n1"), "Deslocamento Z do centróide até o centro de cisalhamento"));

            props.Add(new Linha_propriedades("Cw [cm6]", (calculoSecao.propriedades.ConstanteEmpenamento /1000000).ToString("e3"), "Constante de empenamento"));

            props.Add(new Linha_propriedades("Acy [cm²]", (calculoSecao.propriedades.areaCisalhamentoY / 100).ToString("n2"), "Área de cisalhamento na direção y"));
            props.Add(new Linha_propriedades("Acz [cm²]", (calculoSecao.propriedades.areaCisalhamentoZ / 100).ToString("n2"), "Área de cisalhamento na direção z"));

            props.Add(new Linha_propriedades("Simetria - eixo u", (calculoSecao.propriedades.SimetriaY), "Avalia se a seção é simétrica/espelhada no eixo local u"));
            props.Add(new Linha_propriedades("Simetria - eixo v", (calculoSecao.propriedades.SimetriaZ), "Avalia se a seção é simétrica/espelhada no eixo local v"));

            grid.Columns[0].DataPropertyName = "propriedade";
            grid.Columns[0].HeaderText = "Propriedade";
            grid.Columns[1].HeaderText = "Valor";

            grid.Columns[1].DataPropertyName = "val";

            bindingSource1.DataSource = props;
            grid.DataSource = bindingSource1;// gerenciador.formDesenho.Estrutura.combinacoes[lbComb.SelectedIndex].Coeficientes;
        }
        bool centralizado = false;
        void CentralizarSecaoCalculada(double xc, double yc)
        {
            for (int i = 0; i < arestasFinalGeometria.Count(); i++)
            {
                arestasFinalGeometria[i].p1.x -= xc;
                arestasFinalGeometria[i].p1.y -= yc;
                arestasFinalGeometria[i].p2.x -= xc;
                arestasFinalGeometria[i].p2.y -= yc;
            }
            for (int i = 0; i < arestasFinalGeometria_SemRaio.Count(); i++)
            {
                arestasFinalGeometria_SemRaio[i].p1.x -= xc;
                arestasFinalGeometria_SemRaio[i].p1.y -= yc;
                arestasFinalGeometria_SemRaio[i].p2.x -= xc;
                arestasFinalGeometria_SemRaio[i].p2.y -= yc;
            }

            for (int i = 0; i < arestasTemplateOriginal.Count(); i++)
            {
                arestasTemplateOriginal[i].p1.x -= xc;
                arestasTemplateOriginal[i].p1.y -= yc;
                arestasTemplateOriginal[i].p2.x -= xc;
                arestasTemplateOriginal[i].p2.y -= yc;
            }

            if (nosMalha != null)
            {
                for (int i = 0; i < nosMalha.Count(); i++)
                {
                    nosMalha[i].x -= xc; nosMalha[i].y -= yc;
                }
            }

            if (nos_ordenados_final != null)
            {
                for (int i = 0; i < nos_ordenados_final.Count(); i++)
                {
                    nos_ordenados_final[i].x -= xc; nos_ordenados_final[i].y -= yc;
                }
            }

            for (int i = 0; i < marcacoesCotas.Count(); i++)
            {
                marcacoesCotas[i].p1.x -= xc;
                marcacoesCotas[i].p1.y -= yc;
                marcacoesCotas[i].p2.x -= xc;
                marcacoesCotas[i].p2.y -= yc;
            }

            for (int i = 0; i < arestasCotas.Count(); i++)
            {
                arestasCotas[i].p1.x -= xc;
                arestasCotas[i].p1.y -= yc;
                arestasCotas[i].p2.x -= xc;
                arestasCotas[i].p2.y -= yc;
            }

            for (int i = 0; i < arestasRaios.Count(); i++)
            {
                arestasRaios[i].p1.x -= xc;
                arestasRaios[i].p1.y -= yc;
                arestasRaios[i].p2.x -= xc;
                arestasRaios[i].p2.y -= yc;
            }

            List<cotasSecao> infoCotas = infoSecao.cotas;
            for (int i = 0; i < infoCotas.Count; i++)
            {
                if (infoCotas[i].pontosDeslocar != null)
                {
                    for (int j = 0; j < infoCotas[i].pontosDeslocar.Count; j++)
                    {
                        infoCotas[i].pontosDeslocar[j].p.x -= xc;
                        infoCotas[i].pontosDeslocar[j].p.y -= yc;

                        infoCotas[i].pontosDeslocar[j].vetor.x -= xc;
                        infoCotas[i].pontosDeslocar[j].vetor.y -= yc;
                    }
                }

                infoCotas[i].p1.x -= xc;
                infoCotas[i].p1.y -= yc;
                infoCotas[i].p2.x -= xc;
                infoCotas[i].p2.y -= yc;
            }

            for (int i = 0; i < infoSecao.raios.Count; i++)
            {
                infoSecao.raios[i].p1.x -= xc;
                infoSecao.raios[i].p1.y -= yc;

                infoSecao.raios[i].xCota -= xc;
                infoSecao.raios[i].yCota -= yc;

                infoSecao.raios[i].xr1 -= xc;
                infoSecao.raios[i].yr1 -= yc;
                infoSecao.raios[i].xr2 -= xc;
                infoSecao.raios[i].yr2 -= yc;
            }

            Enquadrar();
            centralizado = true;
            AtualizaShaders(false);
            DesenhaObjetos();
            glControl.SwapBuffers();
        }

        private void numericUpDown3_ValueChanged(object sender, EventArgs e)
        {
            celulas.ForEach(o => o.sel = false);
            celulas[(int)numericUpDown3.Value].sel = true;

            AtualizaShaders(false);
            DesenhaObjetos();
            glControl.SwapBuffers();
        }

        private void chMostrarMalha_CheckedChanged(object sender, EventArgs e)
        {
            AtualizaShaders(false);
            DesenhaObjetos();
            glControl.SwapBuffers();
        }

        private void gridEixosPrincipais_Click(object sender, EventArgs e)
        {
            if (props_eixosPrincipais != null)
            {
                int it = gridEixosPrincipais.CurrentRow.Index;
                edDescricaoProp.Text = props_eixosPrincipais[it].descricao;
            }
        }

        private void grid_DoubleClick(object sender, EventArgs e)
        {

        }

        private void grid_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {

        }

        private void grid_CellContentDoubleClick(object sender, DataGridViewCellEventArgs e)
        {

        }

        private void grid_CellDoubleClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex == 5)
            {
                pnEditaPropr.Left = mouseX;
                pnEditaPropr.Top = mouseY;

                edNovaPropr.Text = (calculoSecao.propriedades.inercia_torcao / 10000).ToString("n2");
                edNovaPropr.Focus();
                edNovaPropr.SelectAll();
                pnEditaPropr.Visible = true;
            }
            //     
        }

        private void button6_Click(object sender, EventArgs e)
        {
           
        }

        private void button6_Click_1(object sender, EventArgs e)
        {
            calculoSecao.propriedades.inercia_torcao = double.Parse(edNovaPropr.Text) * 10000;
            AtualizaGrid();
            pnEditaPropr.Visible = false;
        }

        private void button1_Click_2(object sender, EventArgs e)
        {
            pnEditaPropr.Visible = false;
        }

        private void grid_MouseMove(object sender, System.Windows.Forms.MouseEventArgs e)
        {

        }

        private void grid_RowEnter(object sender, DataGridViewCellEventArgs e)
        {

        }

        private void grid_Click(object sender, EventArgs e)
        {
            if (props != null)
            {
                int it = grid.CurrentRow.Index;
                edDescricaoProp.Text = props[it].descricao;
            }
        }

        private void button1_Click_1(object sender, EventArgs e)
        {
            CalculaSecao();

            cy_carregado = 0;
            cz_carregado = 0;
            Recalcular = false;
            //alterouGeometria = false;

            AtualizaShaders(false);
            DesenhaObjetos();
            glControl.SwapBuffers();
        }

        private void chOcultarCotas_CheckedChanged_1(object sender, EventArgs e)
        {

        }

        private void glControl_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyData == Keys.Escape && !pnCota.Visible)
                button3_Click_1(button3, null);
        }

        void CalculaAnguloCotas()
        {
            angCotas = new List<double>();

            int maxCotas = arestasCotas.Count;
            for (int i = 0; i < maxCotas; i++)
            {
                p1x = arestasCotas[i].p1.x; p1y = arestasCotas[i].p1.y;
                p2x = arestasCotas[i].p2.x; p2y = arestasCotas[i].p2.y;

                angCota = Math.Atan(((p1y - p2y) / (p1x - p2x)));
                angCotas.Add(-angCota);
            }
        }
        void CalculaValorCotas()
        {
            valCotas = new List<double>();

            for (int i = 0; i < infoSecao.cotas.Count; i++)
                valCotas.Add(infoSecao.cotas[i].p1.DistanceTo(infoSecao.cotas[i].p2));
        }

        vec3 zero;
        void CriarArestasTemplateOriginal()
        {
            coordenadas = new List<vec3>();
            arestasTemplateOriginal = new List<LinhaVec3>();
            foreach (LinhaVec3 b in linhas_geom_temp)
            {
                if (b.tipo_barra == 0 || b.tipo_barra == 3 || b.tipo_barra == 1) // simples tipo 0 (só contorno externo) ou tipo 3 (contorno interno no caso de tubos por exemplo) ou tipo 1 (linha auxiliar)
                {
                    arestasTemplateOriginal.Add(new LinhaVec3( new vec3(b.p1.x, -b.p1.y, 0), new vec3(b.p2.x, -b.p2.y, 0), b.tipo_barra == 3, -1 , b.tipo_barra == 1, b.id_barra, b.id_barra, b.tipo_barra));
                    //if (LocalizaPonto(ref coordenadas, b.pIni.x, b.pIni.y, 0) == -1)
                    {
                  //      coordenadas.Add(new vec3(b.pIni.x, b.pIni.y, 0));
                    }
                 //   if (LocalizaPonto(ref coordenadas, b.pFin.x, b.pFin.y, 0) == -1)
                    {
             //           coordenadas.Add(new vec3(b.pFin.x, b.pFin.y, 0));
                    }
                }
            }

            if (infoSecao.cotas[0].cotaDiametro)
            {
               // CalculaValorCotas();
                zero = new vec3(0,0,0);
            }
            else
                zero = new vec3(arestasTemplateOriginal[0].p1.x, arestasTemplateOriginal[0].p1.y, 0);

            for (int i = 0; i < arestasTemplateOriginal.Count; i++)
            {
                arestasTemplateOriginal[i].p1.x -= zero.x;
                arestasTemplateOriginal[i].p1.y -= zero.y;
                arestasTemplateOriginal[i].p2.x -= zero.x;
                arestasTemplateOriginal[i].p2.y -= zero.y;

                arestasTemplateOriginal[i].p1.x *= 10;
                arestasTemplateOriginal[i].p1.y *= 10;

              //  arestas[i].p1.x *= -1;

                arestasTemplateOriginal[i].p2.x *= 10;
                arestasTemplateOriginal[i].p2.y *= 10;

           //     arestas[i].p2.x *= -1;

                if (Geom.Iguais(arestasTemplateOriginal[i].p1.x, 0))
                    arestasTemplateOriginal[i].p1.x = 0;
                if (Geom.Iguais(arestasTemplateOriginal[i].p1.y, 0))
                    arestasTemplateOriginal[i].p1.y = 0;
                if (Geom.Iguais(arestasTemplateOriginal[i].p2.x, 0))
                    arestasTemplateOriginal[i].p2.x = 0;
                if (Geom.Iguais(arestasTemplateOriginal[i].p2.y, 0))
                    arestasTemplateOriginal[i].p2.y = 0;
                // coordenadas[i].y *= -1;

            }
        }
        List<LinhaVec3> linhas_geom_temp;

        void AbrirTemplate(string arq)
        {
            try 
            {
                string arq_vec = arq + "_vec";
                if (File.Exists(arq_vec))
                {
                    FileStream arquivo;
                    BinaryFormatter arqBin = new BinaryFormatter();
                    arquivo = new FileStream(arq_vec, FileMode.Open, FileAccess.Read);

                    linhas_geom_temp = (List<LinhaVec3>)arqBin.Deserialize(arquivo);

                    // TArquivo Template = (TArquivo)arqBin.Deserialize(arquivo);

                    // contorno externo (que é o mais utilizado) e contorno interno no caso da parede interna de tubos por exemplo
                    //faço essa distinção para depois renderizar corretamente, senao a triangulação se perde na ordem nos pontos
                    //nao tem serventia na geração de malha e nem na edição de cotas
                    //tipo 1 

                    //bars = Template.barras.FindAll(o=>o.Dados.Tipo == 0 || o.Dados.Tipo == 3 || o.Dados.Tipo == 1);

                    arquivo.Close();

                    string arquivo_cotas_e_raios = arq + "_info";
                    if (File.Exists(arquivo_cotas_e_raios))
                    {
                        arquivo = new FileStream(arquivo_cotas_e_raios, FileMode.Open, FileAccess.Read);

                        infoSecao = (TInfoSecao)arqBin.Deserialize(arquivo);
                    }
                }
            }
            catch(Exception ee)
            {

            }
        }

        private void FCalculaSecao_Load(object sender, EventArgs e)
        {
            grid.AutoGenerateColumns = false;
            gridEixosPrincipais.AutoGenerateColumns = false;
            
            //   AtualizaMateriais(true, true);
            PanelAguarde.Visible = false;

            flatTabControl1.TabPages.RemoveAt(1);
            /*  for (int i = 0; i < gbValoresProp.Controls.Count; i++)
              {
                  if (gbValoresProp.Controls[i] is System.Windows.Forms.TextBox)
                  {
                      System.Windows.Forms.TextBox tc = (System.Windows.Forms.TextBox)(gbValoresProp.Controls[i]);
                      //   MessageBox.Show(tc.Name.ToString());
                      ((System.Windows.Forms.TextBox)(gbValoresProp.Controls[i])).MouseEnter += new EventHandler(edA_MouseEnter);
                  }
              }*/
        }

        void CriaPlanos()
        {
            normXY = new vec3(0.0f, 0.0f, 1.0f);
            vec3 pt = new vec3(0.0f, 0.0f, 200);
            planoZoom = new Plano(normXY, pt, "xy");
        }
        void Wheel(int Delta)
        {
            try
            {

                z_trans_orto -= (Delta * 0.00055f);


                if (Delta > 0)
                {
                    fatorzoom_orto -= (fatorzoom_orto * 0.2f);

                }
                else
                {

                    fatorzoom_orto += (fatorzoom_orto * 0.2f);

                }
                modelViewMatrix_Zoom = Matrix4.CreateTranslation(-x_trans, -y_trans, -z_trans);
                raioZoom.GerarRaio3D(ref mouseX, ref mouseY, ref ViewPortPrincipal, ref modelViewMatrix_Zoom, ref Projecao);
                raioZoom.CalculaIntersecao_Raio_x_Plano(ref planoZoom, ref Coord_PlanoZoom);
                xant = Coord_PlanoZoom.x;
                yant = Coord_PlanoZoom.y;
                z_trans -= (Delta * 0.00035f);
                leftOrtoZoom = GLleft * fatorzoom_orto;
                rightOrtoZoom = GLright * fatorzoom_orto;
                topOrtoZoom = GLtop * fatorzoom_orto;
                bottonOrtoZoom = GLbotton * fatorzoom_orto;

                Projecao = Matrix4.CreateOrthographicOffCenter((float)leftOrtoZoom, (float)rightOrtoZoom, (float)bottonOrtoZoom, (float)topOrtoZoom, (float)zNear, (float)zFar);

                modelViewMatrix_Zoom = Matrix4.CreateTranslation(-x_trans, -y_trans, -z_trans);
                raioZoom.GerarRaio3D(ref mouseX, ref mouseY, ref ViewPortPrincipal, ref modelViewMatrix_Zoom, ref Projecao);
                raioZoom.CalculaIntersecao_Raio_x_Plano(ref planoZoom, ref Coord_PlanoZoom);

                offset_x = (float)(Coord_PlanoZoom.x - xant);
                offset_y = (float)(Coord_PlanoZoom.y - yant);

                x_trans += offset_x * -1;
                y_trans += offset_y * -1;

                DesenhaObjetos();
            }

            catch (Exception ex)
            {
                MessageBox.Show("erro mousewheel: " + ex.Message);
            }
        }
        Vector3 p_projetado, p1_projetado;
        bool [] cotasMarcar;
        bool[] raiosMarcar;
        private void glControl_MouseMove(object sender, System.Windows.Forms.MouseEventArgs e)
        {
            mouseX = e.X;
            mouseY = e.Y;
            
            if (e.Button == System.Windows.Forms.MouseButtons.Middle)
            {
                if (Panning)
                {
                    raioZoom.GerarRaio3D(ref mouseX, ref mouseY, ref ViewPortPrincipal, ref viewMatrix_Panning, ref Projecao);
                    raioZoom.CalculaIntersecao_Raio_x_Plano(ref PlanoPanning, ref Coord2_PlanoPanning);

                    x_trans = x_trans - ((float)Coord2_PlanoPanning.x - start_x);
                    y_trans = y_trans - ((float)Coord2_PlanoPanning.y - start_y);

                    start_x = (float)Coord2_PlanoPanning.x;
                    start_y = (float)Coord2_PlanoPanning.y;
                }
            }


            for (int i = 0; i < arestasCotas.Count; i++)
            {
              //  if (infoSecao.cotas[i].igualOutraCota) continue;
                p1_projetado = new Vector3(-(float)(arestasCotas[i].p1.x), -(float)(arestasCotas[i].p1.y), 0);
                p_projetado = Vector3.Project(p1_projetado, ViewPortPrincipal[0], ViewPortPrincipal[1], ViewPortPrincipal[2], ViewPortPrincipal[3], -100, 200, viewMatrix * Projecao);
                p_projetado.Y = ViewPortPrincipal[3] - p_projetado.Y;
                p1x = p_projetado.X; p1y = p_projetado.Y;

                p1_projetado = new Vector3(-(float)(arestasCotas[i].p2.x), -(float)(arestasCotas[i].p2.y), 0);
                p_projetado = Vector3.Project(p1_projetado, ViewPortPrincipal[0], ViewPortPrincipal[1], ViewPortPrincipal[2], ViewPortPrincipal[3], -100, 200, viewMatrix * Projecao);
                p_projetado.Y = ViewPortPrincipal[3] - p_projetado.Y;
                p2x = p_projetado.X; p2y = p_projetado.Y;

                /* p2d = new Vector3(-(float)(arestasCotas[i].p2.x), -(float)(arestasCotas[i].p2.y), 0);
                 proj = Vector3.Project(p2d, ViewPortPrincipal[0], ViewPortPrincipal[1], ViewPortPrincipal[2], ViewPortPrincipal[3], -100, 200, viewMatrix * Projecao);
                 proj.Y = ViewPortPrincipal[3] - proj.Y;
                 p2x = proj.X; p2y = proj.Y;*/

                cotasMarcar[i] = false;

                if (Geom.PontoEmLinha2(mouseX, mouseY, p1x, p1y, p2x, p2y, 2))
                {
                    cotasMarcar[i] = true;
                }
            }

            for (int i = 0; i < infoSecao.raios.Count; i++)
            {
                if (infoSecao.raios[i].raio_dependente)
                    continue;

                if (raiosInvisiveis.Exists(o => o == i))
                    continue;

                // if (infoSecao.cotas[i].igualOutraCota) continue;
                p1_projetado = new Vector3(-(float)(infoSecao.raios[i].xCota), -(float)(infoSecao.raios[i].yCota), 0);
                p_projetado = Vector3.Project(p1_projetado, ViewPortPrincipal[0], ViewPortPrincipal[1], ViewPortPrincipal[2], ViewPortPrincipal[3], -100, 200, viewMatrix * Projecao);
                p_projetado.Y = ViewPortPrincipal[3] - p_projetado.Y;
                p1x = p_projetado.X; p1y = p_projetado.Y;

            /*    p1_projetado = new Vector3(-(float)(infoSecao.raios[i].p1.x), -(float)(infoSecao.raios[i].p1.y), 0);
                p_projetado = Vector3.Project(p1_projetado, ViewPortPrincipal[0], ViewPortPrincipal[1], ViewPortPrincipal[2], ViewPortPrincipal[3], -100, 200, viewMatrix * Projecao);
                p_projetado.Y = ViewPortPrincipal[3] - p_projetado.Y;
                p2x = p_projetado.X+15; p2y = p_projetado.Y+15;*/

                raiosMarcar[i] = false;

                if (Geom.PontoEmLinha2(mouseX, mouseY, p1x-15, p1y-15, p1x+15, p1y+15, 2))
                {
                    raiosMarcar[i] = true;
                }
            }

            //    PreencheBatchArestas();
            DesenhaObjetos();
            glControl.SwapBuffers();
        }
        public void SetupCamera2(bool mudancaManual = false, double zfar = 300)
        {
            try
            {

                GL.ClearColor(gerenciador.ConfiguracoesPGi.F3DOpcoesVisualizacao.CorFundoCima);
                
                GL.Viewport(0, 0, glControl.Width, glControl.Height);

                lbGeometriaSecao.BackColor = gerenciador.ConfiguracoesPGi.F3DOpcoesVisualizacao.CorFundoCima;
              
                fatorzoom_orto = -5;

                GL.MatrixMode(MatrixMode.Projection);

                GLtop = 1;
                GLbotton = -1;

                if (this.Height == 0)
                {
                    GLleft = 0;
                    GLright = 0;
                }
                else
                {
                    GLleft = -(double)(glControl.Width) / (double)glControl.Height;
                    GLright = -GLleft;
                }

                zNear = -100;
                zFar = 300;

                // GL.Ortho(GLleft, GLright, GLbotton, GLtop, zNear, zFar);
                leftOrtoZoom2 = GLleft * 1;
                rightOrtoZoom2 = GLright * 1;
                topOrtoZoom2 = GLtop * 1;
                bottonOrtoZoom2 = GLbotton * 1;

                Projecao2 = Matrix4.CreateOrthographicOffCenter((float)leftOrtoZoom, (float)rightOrtoZoom, (float)bottonOrtoZoom, (float)topOrtoZoom, (float)zNear, (float)zFar);


            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        public void SetupCamera(bool mudancaManual = false, double zfar = 300)
        {
            try
            {

                if (gerenciador.ConfiguracoesPGi.F3DOpcoesVisualizacao.CorFundoCima == System.Drawing.Color.White ||
                    gerenciador.ConfiguracoesPGi.F3DOpcoesVisualizacao.CorFundoCima == System.Drawing.Color.Silver)
                    CorCota = System.Drawing.Color.Black;
                else
                    CorCota = System.Drawing.Color.Orange;

                lbGeometriaSecao.ForeColor = CorCota;

                GL.ClearColor(gerenciador.ConfiguracoesPGi.F3DOpcoesVisualizacao.CorFundoCima);
                GL.Viewport(0, 0, glControl.Width, glControl.Height);

                fatorzoom_orto = -5;

                GL.MatrixMode(MatrixMode.Projection);

                    GLtop = 1;
                    GLbotton = -1;

                    if (this.Height == 0)
                    {
                        GLleft = 0;
                        GLright = 0;
                    }
                    else
                    {
                        GLleft = -(double)(glControl.Width) / (double)glControl.Height;
                        GLright = -GLleft;
                    }

                    zNear = -100;
                    zFar = 300;

                    // GL.Ortho(GLleft, GLright, GLbotton, GLtop, zNear, zFar);
                    leftOrtoZoom = GLleft * fatorzoom_orto;
                    rightOrtoZoom = GLright * fatorzoom_orto;
                    topOrtoZoom = GLtop * fatorzoom_orto;
                    bottonOrtoZoom = GLbotton * fatorzoom_orto;

                    Projecao = Matrix4.CreateOrthographicOffCenter((float)leftOrtoZoom, (float)rightOrtoZoom, (float)bottonOrtoZoom, (float)topOrtoZoom, (float)zNear, (float)zFar);
                    GL.LoadMatrix(ref Projecao);
                    double near = 0.00001;
                    double far = 300;
                    Projecao_Panning = Matrix4.CreatePerspectiveFieldOfView((float)(45 * Math.PI / 180), glControl.Width / (float)glControl.Height, (float)near, (float)far);
                    
                CriaPlanos();

                AtualizaShaders(false);
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        public void PreencheBatchTriangulos()
        {
            try
            {
                List<float> coords_triangulos = new List<float>();

                List<Triangulo> triangulos_selecao = new List<Triangulo>();

                List<float> coords_triangulos_deformacao = new List<float>();

            //    foreach (TBarraGenerica b in Estrutura.barras)
              //      b.Preenche_Triangulos(ref coords_triangulos, ref triangulos_selecao);

                BatchTriangulos_Principal = new float[coords_triangulos.Count];
                BatchTriangulos_Principal = coords_triangulos.ToArray();
            }
            catch (Exception ee)
            {
                MessageBox.Show("erro PreencheBatchTriangulos: " + ee.Message);
            }
        }
        float[] BatchTriangulos_Principal;
        float[] BatchArestas;
        void setLista(ref List<float> coord_objeto, double x, double y, double z)
        {
            coord_objeto.Add((float)x);
            coord_objeto.Add((float)y);
            coord_objeto.Add((float)z);
        }
        TriangleNet.Mesh mesh;
        void TriangularSecao()
        {
            /*var pol = new Polygon_TriangleNET();
           
            int max = coords.Count();
            if (!testes)
            {
                for (int k = 0; k < max; k++)
                {

                    if (k == max - 1)
                        pol.Add(new Segment(new TriangleNet.Geometry.Vertex(coords[k].X, coords[k].Y),
                                            new TriangleNet.Geometry.Vertex(coords[0].X, coords[0].Y), 1), 0);
                    else 
                        pol.Add(new Segment(new TriangleNet.Geometry.Vertex(coords[k].X, coords[k].Y),
                                            new TriangleNet.Geometry.Vertex(coords[k + 1].X, coords[k + 1].Y), 1), 0);
                }
            }
            else
            {
                pol.Add(new Segment(new TriangleNet.Geometry.Vertex(-0.1, -0.2),
                                     new TriangleNet.Geometry.Vertex(0.2, -0.2), 1), 0);

                pol.Add(new Segment(new TriangleNet.Geometry.Vertex(0.2, -0.2),
                        new TriangleNet.Geometry.Vertex(0.2, 0.1), 1), 0);

                pol.Add(new Segment(new TriangleNet.Geometry.Vertex(0.2, 0.1),
                        new TriangleNet.Geometry.Vertex(-0.1, 0.1), 1), 0);

                pol.Add(new Segment(new TriangleNet.Geometry.Vertex(-0.1, 0.1),
                        new TriangleNet.Geometry.Vertex(-0.1, -0.2), 1), 0);
            }
            
            var options = new ConstraintOptions() { ConformingDelaunay = true };

            // var qual = new QualityOptions() { MaximumArea = areaSecao * 0.00000008 };
            var qual = new QualityOptions() { MaximumArea = areaSecao * 0.00000015 };
            if (testes)
               qual = new QualityOptions() { MaximumArea = 0.5 };
          
            mesh = (Mesh)pol.Triangulate(options, qual);
            var smoother = new SimpleSmoother();
            smoother.Smooth(mesh);*/
        }
        public System.Drawing.Color CorCelulas = System.Drawing.Color.Gray;
        vec3 rot_eixo_u, rot_eixo_v;
        public void PreencheBatchArestas()
        {
            try
            {
                List<float> coords_arestas = new List<float>();
                //   foreach (TBarraGenerica b in Estrutura.barras)
                //    b.Preenche_Arestas(ref coords_arestas, ref Arestas, ref ArestasConfObjeto, ref Unifilar);

                if (chMostrarMalha.Checked)
                {
                    if (celulas != null)
                    {
                        for (int i = 0; i < celulas.Count; i++)
                        {
                            if (celulas[i].sel)
                                CorCelulas = System.Drawing.Color.Red;
                            else
                                CorCelulas = System.Drawing.Color.Gray;

                            if (celulas[i].N3 != null)
                            {
                                setLista(ref coords_arestas, -celulas[i].N1.x, -celulas[i].N1.y, 0);
                                setLista(ref coords_arestas, (double)CorCelulas.R / 255, (double)CorCelulas.G / 255, (double)CorCelulas.B / 255);
                                setLista(ref coords_arestas, -celulas[i].N2.x, -celulas[i].N2.y, 0);
                                setLista(ref coords_arestas, (double)CorCelulas.R / 255, (double)CorCelulas.G / 255, (double)CorCelulas.B / 255);

                                setLista(ref coords_arestas, -celulas[i].N2.x, -celulas[i].N2.y, 0);
                                setLista(ref coords_arestas, (double)CorCelulas.R / 255, (double)CorCelulas.G / 255, (double)CorCelulas.B / 255);
                                setLista(ref coords_arestas, -celulas[i].N3.x, -celulas[i].N3.y, 0);
                                setLista(ref coords_arestas, (double)CorCelulas.R / 255, (double)CorCelulas.G / 255, (double)CorCelulas.B / 255);

                                setLista(ref coords_arestas, -celulas[i].N3.x, -celulas[i].N3.y, 0);
                                setLista(ref coords_arestas, (double)CorCelulas.R / 255, (double)CorCelulas.G / 255, (double)CorCelulas.B / 255);
                                setLista(ref coords_arestas, -celulas[i].N4.x, -celulas[i].N4.y, 0);
                                setLista(ref coords_arestas, (double)CorCelulas.R / 255, (double)CorCelulas.G / 255, (double)CorCelulas.B / 255);

                                setLista(ref coords_arestas, -celulas[i].N4.x, -celulas[i].N4.y, 0);
                                setLista(ref coords_arestas, (double)CorCelulas.R / 255, (double)CorCelulas.G / 255, (double)CorCelulas.B / 255);
                                setLista(ref coords_arestas, -celulas[i].N1.x, -celulas[i].N1.y, 0);
                                setLista(ref coords_arestas, (double)CorCelulas.R / 255, (double)CorCelulas.G / 255, (double)CorCelulas.B / 255);
                            }
                        }

                      /*  for (int i = 1; i < _quad4.Count(); i++)
                        {
                            CorCelulas = System.Drawing.Color.Red;
                            {
                                setLista(ref coords_arestas, -_quad4[i].n1.x, -_quad4[i].n1.y, 0);
                                setLista(ref coords_arestas, (double)CorCelulas.R / 255, (double)CorCelulas.G / 255, (double)CorCelulas.B / 255);
                                setLista(ref coords_arestas, -_quad4[i].n2.x, -_quad4[i].n2.y, 0);
                                setLista(ref coords_arestas, (double)CorCelulas.R / 255, (double)CorCelulas.G / 255, (double)CorCelulas.B / 255);

                                setLista(ref coords_arestas, -_quad4[i].n2.x, -_quad4[i].n2.y, 0);
                                setLista(ref coords_arestas, (double)CorCelulas.R / 255, (double)CorCelulas.G / 255, (double)CorCelulas.B / 255);
                                setLista(ref coords_arestas, -_quad4[i].n3.x, -_quad4[i].n3.y, 0);
                                setLista(ref coords_arestas, (double)CorCelulas.R / 255, (double)CorCelulas.G / 255, (double)CorCelulas.B / 255);

                                setLista(ref coords_arestas, -_quad4[i].n3.x, -_quad4[i].n3.y, 0);
                                setLista(ref coords_arestas, (double)CorCelulas.R / 255, (double)CorCelulas.G / 255, (double)CorCelulas.B / 255);
                                setLista(ref coords_arestas, -_quad4[i].n4.x, -_quad4[i].n4.y, 0);
                                setLista(ref coords_arestas, (double)CorCelulas.R / 255, (double)CorCelulas.G / 255, (double)CorCelulas.B / 255);

                                setLista(ref coords_arestas, -_quad4[i].n4.x, -_quad4[i].n4.y, 0);
                                setLista(ref coords_arestas, (double)CorCelulas.R / 255, (double)CorCelulas.G / 255, (double)CorCelulas.B / 255);
                                setLista(ref coords_arestas, -_quad4[i].n1.x, -_quad4[i].n1.y, 0);
                                setLista(ref coords_arestas, (double)CorCelulas.R / 255, (double)CorCelulas.G / 255, (double)CorCelulas.B / 255);
                            }
                        }*/
                    }
                }

                if (centralizado)
                {
                    setLista(ref coords_arestas, -cy_carregado, cz_carregado, 0);
                    setLista(ref coords_arestas, 1, 0, 0);
                    setLista(ref coords_arestas, -25 - cy_carregado, cz_carregado, 0);
                    setLista(ref coords_arestas, 1, 0, 0);

                    setLista(ref coords_arestas, -cy_carregado, cz_carregado, 0);
                    setLista(ref coords_arestas, 0, 1, 0);
                    setLista(ref coords_arestas, -cy_carregado, -25 + cz_carregado, 0);
                    setLista(ref coords_arestas, 0, 1, 0);

                    if (rot_eixo_u != null)
                    {
                        setLista(ref coords_arestas, -cy_carregado, cz_carregado, 0);
                        setLista(ref coords_arestas, 1, 0, 0);
                        setLista(ref coords_arestas, -rot_eixo_u.x, rot_eixo_u.y, 0);
                        setLista(ref coords_arestas, 1, 0, 0);

                        setLista(ref coords_arestas, -cy_carregado, cz_carregado, 0);
                        setLista(ref coords_arestas, 0, 1, 0);
                        setLista(ref coords_arestas, -rot_eixo_v.x, rot_eixo_v.y, 0);
                        setLista(ref coords_arestas, 0, 1, 0);
                    }
                }


                if (!testes)
                {
                    int max = arestasTemplateOriginal.Count();
                    int prox;
                    for (int i = 0; i < arestasTemplateOriginal.Count(); i++)
                    {
                        /*   if (infoSecao.raios.Exists(r=>Geom.Iguais(r.p1.x, arestas[i].p1.x) && Geom.Iguais(r.p1.y, arestas[i].p1.y)))
                           {
                               continue;
                           }

                           if (infoSecao.raios.Exists(r => Geom.Iguais(r.p1.x, arestas[i].p2.x) && Geom.Iguais(r.p1.y, arestas[i].p2.y)))
                           {
                               continue;
                           }*/
                     /*   if (arestaVisivel[i])
                        {
                            setLista(ref coords_arestas, -arestasFinalGeometria_SemRaio[i].p1.x, -arestasFinalGeometria_SemRaio[i].p1.y, 0);
                            setLista(ref coords_arestas, 0.1, 0.5, 0.8);
                            setLista(ref coords_arestas, -arestasFinalGeometria_SemRaio[i].p2.x, -arestasFinalGeometria_SemRaio[i].p2.y, 0);
                            setLista(ref coords_arestas, 0.1, 0.5, 0.8);
                        }*/
                    }

                    for (int i = 0; i < arestasFinalGeometria.Count(); i++)
                    {
                       setLista(ref coords_arestas, -arestasFinalGeometria[i].p1.x, -arestasFinalGeometria[i].p1.y, 0);
                       setLista(ref coords_arestas, 0.1, 0.5, 0.8);
                       setLista(ref coords_arestas, -arestasFinalGeometria[i].p2.x, -arestasFinalGeometria[i].p2.y, 0);
                       setLista(ref coords_arestas, 0.1, 0.5, 0.8);
                    }

                    if (chMostrarCotas.Checked)
                    {
                        for (int i = 0; i < marcacoesCotas.Count(); i++)
                        {
                            setLista(ref coords_arestas, -marcacoesCotas[i].p1.x, -marcacoesCotas[i].p1.y, 0);
                            setLista(ref coords_arestas, (double)CorCota.R / 255, (double)CorCota.G / 255, (double)CorCota.B / 255);
                            setLista(ref coords_arestas, -marcacoesCotas[i].p2.x, -marcacoesCotas[i].p2.y, 0);
                            setLista(ref coords_arestas, (double)CorCota.R / 255, (double)CorCota.G / 255, (double)CorCota.B / 255);
                        }

                        for (int i = 0; i < arestasCotas.Count(); i++)
                        {
                               if (cotasInvisiveis.Exists(o => o == i))
                                   continue;

                            setLista(ref coords_arestas, -arestasCotas[i].p1.x, -arestasCotas[i].p1.y, 0);
                            setLista(ref coords_arestas, (double)CorCota.R / 255, (double)CorCota.G / 255, (double)CorCota.B / 255);
                            setLista(ref coords_arestas, -arestasCotas[i].p2.x, -arestasCotas[i].p2.y, 0);
                            setLista(ref coords_arestas, (double)CorCota.R / 255, (double)CorCota.G / 255, (double)CorCota.B / 255);
                        }
                    }

                    for (int i = 0; i < arestasRaios.Count(); i++)
                    {
                        //   prox = (i + 1) % arestas.Count;

                       /* setLista(ref coords_arestas, -arestasRaios[i].p1.x, -arestasRaios[i].p1.y, 0);
                        setLista(ref coords_arestas, 0.1,1, 0.8);
                        setLista(ref coords_arestas, -arestasRaios[i].p2.x, -arestasRaios[i].p2.y, 0);
                        setLista(ref coords_arestas, 0.1, 1, 0.8);*/

                        //                        setLista(ref coords_arestas, arestas[prox].p1.x, arestas[prox].p1.y, 0);
                        //                      setLista(ref coords_arestas, 0, 1, 1);
                    }
                }

           //     TriangleNet.Geometry.Vertex v1, v2, v3;

              //  TriangularSecao();
                /*
                setLista(ref coords_arestas, 0.3,0, 1);
                setLista(ref coords_arestas, 1, 0.4, 0.4);

                setLista(ref coords_arestas, 0, 0.3, -1);
                setLista(ref coords_arestas, 0, 0, 1);

                setLista(ref coords_arestas, 0, 0, 1);
                setLista(ref coords_arestas, 0, 1, 0);
                */
              /*  foreach (var tri in mesh.Triangles)
                {
                    v1 = tri.GetVertex(0);
                    v2 = tri.GetVertex(1);
                    v3 = tri.GetVertex(2);
                   
                    //if (tri.Bounds)
                    setLista(ref coords_arestas, v1.X, v1.Y, 0);
                    setLista(ref coords_arestas, 0.5, 0.5, 0.5);
                    setLista(ref coords_arestas, v2.X, v2.Y, 0);
                    setLista(ref coords_arestas, 0.5, 0.5, 0.5);

                    setLista(ref coords_arestas, v2.X, v2.Y, 0);
                    setLista(ref coords_arestas, 0.5, 0.5, 0.5);
                    setLista(ref coords_arestas, v3.X, v3.Y, 0);
                    setLista(ref coords_arestas, 0.5, 0.5, 0.5);

                    setLista(ref coords_arestas, v3.X, v3.Y, 0);
                    setLista(ref coords_arestas, 0.5, 0.5, 0.5);
                    setLista(ref coords_arestas, v1.X, v1.Y, 0);
                    setLista(ref coords_arestas, 0.5, 0.5, 0.5);
                }*/


 ///deslocar o ponto zero para o centroide
 
                /* for (int i = 0; i < mesh.triangles.Count; i++)
                 {
                     var p0 = mesh.triangles.ElementAt(i).GetVertex(0);

                     var p1 = mesh.triangles.ElementAt(i).GetVertex(1);
                     setLista(ref coords_arestas, p1.x, p1.y, 0);
                     setLista(ref coords_arestas, 0, 1, 1);
                     var p2 = mesh.triangles.ElementAt(i).GetVertex(2);
                     setLista(ref coords_arestas, p2.x, p2.y, 0);
                     setLista(ref coords_arestas, 0, 1, 1);

                     //var star = circulator.EnumerateTriangles(mesh.vertices[i]);
                     // foreach (var triangle in star)
                     // {
                     //  triangle.GetVertex(0);
                     //    
                     //    ;
                     //  }


                     // setLista(ref coords_arestas, coords.coordenadas[i + 1].X, coords.coordenadas[i + 1].Y, 0);

                     //  setLista(ref coords_arestas, 0, 1, 1);
                 }
                 */
                BatchArestas = new float[coords_arestas.Count];
                BatchArestas = coords_arestas.ToArray();
            }
            catch (Exception ee)
            {
                MessageBox.Show("erro PreencheBatchArestas: " + ee.Message);
            }
        }

        public void AtualizaVBO(bool atualizaTriangulos = true)
        {
            int size = 0;

            if (atualizaTriangulos)
            {
                if (VAO_triangulos_principal != 0)
                {
                    GL.DeleteBuffers(1, ref VAO_triangulos_principal);
                    GL.DeleteVertexArray(VAO_triangulos_principal);
                }

                VAO_triangulos_principal = GL.GenVertexArray();
                GL.BindVertexArray(VAO_triangulos_principal);
                GL.GenBuffers(1, out vertex_buffer_triangulos_principal);
                GL.BindBuffer(BufferTarget.ArrayBuffer, vertex_buffer_triangulos_principal);
                GL.BufferData(BufferTarget.ArrayBuffer, (IntPtr)(BatchTriangulos_Principal.Length * sizeof(float)), BatchTriangulos_Principal, BufferUsageHint.StaticDraw);
                qtd_coords_triangulos_principal = BatchTriangulos_Principal.Length / 11;
                //coord                normal             cor                           textura
                GL.VertexAttribPointer(0, 3, VertexAttribPointerType.Float, false, 11 * sizeof(float), IntPtr.Zero);
                GL.VertexAttribPointer(1, 3, VertexAttribPointerType.Float, false, 11 * sizeof(float), 3 * sizeof(float));
                GL.VertexAttribPointer(2, 3, VertexAttribPointerType.Float, false, 11 * sizeof(float), 6 * sizeof(float));
                GL.VertexAttribPointer(3, 2, VertexAttribPointerType.Float, false, 11 * sizeof(float), 9 * sizeof(float));
                GL.EnableVertexAttribArray(0);
                GL.EnableVertexAttribArray(1);
                GL.EnableVertexAttribArray(2);
                GL.EnableVertexAttribArray(3);
                GL.GetBufferParameter(BufferTarget.ArrayBuffer, BufferParameterName.BufferSize, out size);

                if (size != BatchTriangulos_Principal.Length * sizeof(float))
                    throw new ApplicationException(string.Format(
                        "Problema ao atualizar VBO dos triângulos principais. Foi tentado carregar {0} bytes, carregado {1}.",
                        BatchTriangulos_Principal.Length * sizeof(float), size));
                /*-------------------------*/

            }

            GL.EnableClientState(ArrayCap.VertexArray);

            if (VAO_arestas != 0)
            {
                GL.DeleteBuffers(1, ref VAO_arestas);
                GL.DeleteVertexArray(VAO_arestas);
            }
            VAO_arestas = GL.GenVertexArray();
            GL.BindVertexArray(VAO_arestas);
            GL.GenBuffers(1, out vertex_buffer_arestas);
            GL.BindBuffer(BufferTarget.ArrayBuffer, vertex_buffer_arestas);
            GL.BufferData(BufferTarget.ArrayBuffer, (IntPtr)(BatchArestas.Length * sizeof(float)), BatchArestas, BufferUsageHint.StaticDraw);
            qtd_coords_arestas = BatchArestas.Length / 6;
            //coord                cor                          
            GL.VertexAttribPointer(0, 3, VertexAttribPointerType.Float, false, 6 * sizeof(float), IntPtr.Zero);
            GL.VertexAttribPointer(1, 3, VertexAttribPointerType.Float, false, 6 * sizeof(float), 3 * sizeof(float));
            GL.EnableVertexAttribArray(0);
            GL.EnableVertexAttribArray(1);
            GL.GetBufferParameter(BufferTarget.ArrayBuffer, BufferParameterName.BufferSize, out size);

            if (size != BatchArestas.Length * sizeof(float))
                throw new ApplicationException(string.Format(
                    "Problema ao atualizar VBO das arestas. Foi tentado carregar {0} bytes, carregado {1}.",
                    BatchArestas.Length * sizeof(float), size));
        }
        public void AtualizaShaders(bool atualizaTriangulos = true)
        {
            try
            {
                if (atualizaTriangulos)
                    PreencheBatchTriangulos();

                PreencheBatchArestas();

                AtualizaVBO(atualizaTriangulos);

            }
            catch (Exception ee)
            {
                MessageBox.Show("Erro ao atualizar os shaders: " + ee.Message);
            }
        }

        public void DesenhaObjetos()
        {
            try
            {

                //   this.Text = m_undoBuffer.m_undoBuffer[(ComandoAdicionar)m_undoBuffer.m_undoBuffer.Count-1]._
                GL.Clear(ClearBufferMask.ColorBufferBit | ClearBufferMask.DepthBufferBit);
                GL.Viewport(0, 0, glControl.Width, glControl.Height);
                GL.MatrixMode(MatrixMode.Projection);
                GL.LoadMatrix(ref Projecao);

                /*só tenho viewmatrix, pois só trabalho na posição da "camera (x,y,z)"
                * modelmatrix nao é necessário, pois não altero a posição dos objetos na cena*/
                GL.MatrixMode(MatrixMode.Modelview);
                viewMatrix = Matrix4.CreateTranslation(-x_trans, -y_trans, 0);
                viewMatrix_temp = Matrix4.CreateTranslation(-x_trans, -y_trans, -200);

                GL.LoadMatrix(ref viewMatrix);

                GL.UseProgram(gerenciador.formDesenho.shader_triangulos_program);
                proj_location = GL.GetUniformLocation(gerenciador.formDesenho.shader_triangulos_program, "projection");
                modeViewlLocation = GL.GetUniformLocation(gerenciador.formDesenho.shader_triangulos_program, "modelview");
                GL.UniformMatrix4(modeViewlLocation, false, ref viewMatrix_temp);
                GL.UniformMatrix4(proj_location, false, ref Projecao);
                GL.BindVertexArray(VAO_triangulos_principal);
                GL.DrawArrays(PrimitiveType.Triangles, 0, qtd_coords_triangulos_principal);

                GL.UseProgram(gerenciador.formDesenho.shader_arestas_program);
                proj_location = GL.GetUniformLocation(gerenciador.formDesenho.shader_arestas_program, "projection");
                modeViewlLocation = GL.GetUniformLocation(gerenciador.formDesenho.shader_arestas_program, "modelview");
                GL.UniformMatrix4(modeViewlLocation, false, ref viewMatrix_temp);
                GL.UniformMatrix4(proj_location, false, ref Projecao);
                GL.BindVertexArray(VAO_arestas);
                GL.DrawArrays(PrimitiveType.Lines, 0, qtd_coords_arestas);

                GL.UseProgram(0);

                GL.GetInteger(GetPName.Viewport, ViewPortPrincipal);

                /*plano textos das cotas*/
                /*  GL.Viewport(0, 0, glControl.Width, glControl.Height);

                  GL.MatrixMode(MatrixMode.Projection);
                  GL.LoadIdentity();
                  GL.Ortho(0, glControl.Width, glControl.Height, 0, zNear, zFar);
                  GL.MatrixMode(MatrixMode.Modelview);*/

                /* GL.Viewport(0, 0, glControl.Width, glControl.Height);
                 GL.MatrixMode(MatrixMode.Projection);
                 GL.LoadMatrix(ref Projecao2);
                 GL.MatrixMode(MatrixMode.Modelview);
                 viewMatrixTextos = Matrix4.CreateTranslation((float)numericUpDown1.Value, (float)numericUpDown2.Value, -200);
                 GL.LoadMatrix(ref viewMatrixTextos);*/

                GL.PopMatrix();

                GL.Viewport(0, 0, glControl.Width, glControl.Height);
                GL.MatrixMode(MatrixMode.Projection);
                GL.LoadIdentity();
                GL.Ortho(0, glControl.Width, glControl.Height, 0, -0.069, zFar);
                GL.MatrixMode(MatrixMode.Modelview);

                viewMatrixTextos = Matrix4.CreateTranslation(0, 0, 0);

                GL.LoadMatrix(ref viewMatrixTextos);

                Vector3 proj, p2d, p1d;
                if (chMostrarCotas.Checked)
                {
                    for (int i = 0; i < arestasCotas.Count; i++)
                    {
                        if (cotasInvisiveis.Exists(o => o == i))
                          continue;

                            /* p1d = new Vector3(-(float)(arestasCotas[i].p1.x), -(float)(arestasCotas[i].p1.y), 0);
                             proj = Vector3.Project(p1d, ViewPortPrincipal[0], ViewPortPrincipal[1], ViewPortPrincipal[2], ViewPortPrincipal[3], -100, 200, viewMatrix * Projecao);
                             proj.Y = ViewPortPrincipal[3] - proj.Y;
                             p1x = proj.X; p1y = proj.Y;*/

                            /* p2d = new Vector3(-(float)(arestasCotas[i].p2.x), -(float)(arestasCotas[i].p2.y), 0);
                             proj = Vector3.Project(p2d, ViewPortPrincipal[0], ViewPortPrincipal[1], ViewPortPrincipal[2], ViewPortPrincipal[3], -100, 200, viewMatrix * Projecao);
                             proj.Y = ViewPortPrincipal[3] - proj.Y;
                             p2x = proj.X; p2y = proj.Y;*/

                        angCota = angCotas[i];

                        p2d = new Vector3(-(float)((arestasCotas[i].p1 + arestasCotas[i].p2) / 2).x, -(float)((arestasCotas[i].p1 + arestasCotas[i].p2) / 2).y, 0);
                        proj = Vector3.Project(p2d, ViewPortPrincipal[0], ViewPortPrincipal[1], ViewPortPrincipal[2], ViewPortPrincipal[3], -100, 200, viewMatrix * Projecao);
                        proj.Y = ViewPortPrincipal[3] - proj.Y;

                        viewMatrixTextos = Matrix4.CreateRotationZ((float)(angCota)) * Matrix4.CreateTranslation(proj.X, proj.Y, 0);

                        GL.LoadMatrix(ref viewMatrixTextos);
                        //       valorCota = infoSecao.cotas[i].p1.DistanceTo(infoSecao.cotas[i].p2) *10;

                        if (cotasMarcar[i])
                            textoCota.Print(valCotas[i].ToString("n2"), fonte, System.Drawing.Color.FromArgb(255, 0, 0));
                        else
                            textoCota.Print(valCotas[i].ToString("n2"), fonte, CorCota);
                    }

                    for (int i = 0; i < infoSecao.raios.Count; i++)
                    {
                        /* p2d = new Vector3(-(float)(infoSecao.raios[i].p1.x), -(float)(infoSecao.raios[i].p1.y), 0);
                         proj = Vector3.Project(p2d, ViewPortPrincipal[0], ViewPortPrincipal[1], ViewPortPrincipal[2], ViewPortPrincipal[3], -100, 200, viewMatrix * Projecao);
                         proj.Y = ViewPortPrincipal[3] - proj.Y;*/
                        if (infoSecao.raios[i].raio_dependente)
                            continue;

                        if (raiosInvisiveis.Exists(o => o == i))
                            continue;

                        p2d = new Vector3(-(float)(infoSecao.raios[i].xCota), -(float)(infoSecao.raios[i].yCota), 0);
                        proj = Vector3.Project(p2d, ViewPortPrincipal[0], ViewPortPrincipal[1], ViewPortPrincipal[2], ViewPortPrincipal[3], -100, 200, viewMatrix * Projecao);
                        proj.Y = ViewPortPrincipal[3] - proj.Y;

                        viewMatrixTextos = Matrix4.CreateTranslation(proj.X, proj.Y, 0);

                        GL.LoadMatrix(ref viewMatrixTextos);

                        if (raiosMarcar[i])
                            textoCota.Print("R" + infoSecao.raios[i].raio.ToString("n1"), fonte, System.Drawing.Color.FromArgb(255, 0, 0));
                        else
                            textoCota.Print("R" + infoSecao.raios[i].raio.ToString("n1"), fonte, CorCota);
                    }
                }

                if (celulas!= null)
                {
                    p2d = new Vector3(-(float)(25), (float)(0), 0);
                    proj = Vector3.Project(p2d, ViewPortPrincipal[0], ViewPortPrincipal[1], ViewPortPrincipal[2], ViewPortPrincipal[3], -100, 200, viewMatrix * Projecao);
                    proj.Y = ViewPortPrincipal[3] - proj.Y;
                    viewMatrixTextos = Matrix4.CreateTranslation(proj.X, proj.Y, 0);
                    GL.LoadMatrix(ref viewMatrixTextos);
                    textoCota.Print("Y", fonte, System.Drawing.Color.FromArgb(255, 0, 0));

                    p2d = new Vector3((float)(0), -(float)(26), 0);
                    proj = Vector3.Project(p2d, ViewPortPrincipal[0], ViewPortPrincipal[1], ViewPortPrincipal[2], ViewPortPrincipal[3], -100, 200, viewMatrix * Projecao);
                    proj.Y = ViewPortPrincipal[3] - proj.Y;
                    viewMatrixTextos = Matrix4.CreateTranslation(proj.X, proj.Y, 0);
                    GL.LoadMatrix(ref viewMatrixTextos);
                    textoCota.Print("Z", fonte, System.Drawing.Color.FromArgb(0, 255, 0));

                    if (rot_eixo_u != null)
                    {
                        p2d = new Vector3(-(float)(rot_eixo_u.x), (float)(rot_eixo_u.y), 0);
                        proj = Vector3.Project(p2d, ViewPortPrincipal[0], ViewPortPrincipal[1], ViewPortPrincipal[2], ViewPortPrincipal[3], -100, 200, viewMatrix * Projecao);
                        proj.Y = ViewPortPrincipal[3] - proj.Y;
                        viewMatrixTextos = Matrix4.CreateTranslation(proj.X, proj.Y, 0);
                        GL.LoadMatrix(ref viewMatrixTextos);
                        textoCota.Print("U", fonte, System.Drawing.Color.FromArgb(255, 0, 0));
                        
                        p2d = new Vector3(-(float)(rot_eixo_v.x), (float)(rot_eixo_v.y), 0);
                        proj = Vector3.Project(p2d, ViewPortPrincipal[0], ViewPortPrincipal[1], ViewPortPrincipal[2], ViewPortPrincipal[3], -100, 200, viewMatrix * Projecao);
                        proj.Y = ViewPortPrincipal[3] - proj.Y;
                        viewMatrixTextos = Matrix4.CreateTranslation(proj.X, proj.Y, 0);
                        GL.LoadMatrix(ref viewMatrixTextos);
                        textoCota.Print("V", fonte, System.Drawing.Color.FromArgb(0, 255, 0));
                    }

                    if (calculoSecao.propriedades.centro_cis_y != 0  || calculoSecao.propriedades.centro_cis_z != 0)
                    {
                        p2d = new Vector3(-(float)(calculoSecao.propriedades.centro_cis_y), -(float)(calculoSecao.propriedades.centro_cis_z), 0);
                        proj = Vector3.Project(p2d, ViewPortPrincipal[0], ViewPortPrincipal[1], ViewPortPrincipal[2], ViewPortPrincipal[3], -100, 200, viewMatrix * Projecao);
                        proj.Y = ViewPortPrincipal[3] - proj.Y;
                        viewMatrixTextos = Matrix4.CreateTranslation(proj.X, proj.Y-10, 0);
                        GL.LoadMatrix(ref viewMatrixTextos);
                        textoCota.Print("c.c", fonte, System.Drawing.Color.FromArgb(255, 0, 0));

                    }
                }
            }
            catch (Exception e)
            {
                MessageBox.Show("erro ao desenhar objetos: " + e.Message);
            }
        }


        public System.Drawing.Color CorCota = System.Drawing.Color.Green;
        Font fonte = new Font("Tahoma", 9);
        double angCota, coca, p1x, p1y, p2x, p2y, dist1, dist2;
        OpenTK.Graphics.TextPrinter textoCota = new OpenTK.Graphics.TextPrinter(OpenTK.Graphics.TextQuality.Medium);
        vec3 pontoMedioCota = new vec3(0, 0, 0);
        public static int h, w;
        private void glControl_Resize(object sender, EventArgs e)
        {
            try
            {
                try
                {
                    SetupCamera(false);
                    SetupCamera2();

                    h = this.glControl.Height;
                    w = this.glControl.Width;

                    Enquadrar();
                    Enquadrar();

                }
                catch (ArgumentException err)
                {
                    MessageBox.Show(err.Message);
                }
            }

            catch (Exception err2)
            {
                MessageBox.Show(err2.Message);
            }
        }
      //  List<TTrianguloQuadratico> triangulos = new List<TTrianguloQuadratico>();


        void integrarGauss(ref double[][] eqs, int pontos, int ordem)
        {
            if (ordem == 3)
            {
                if (pontos == 3)
                {

                }
            }
                
        }
        Matrix<double> Btf;
         void Calcular()
        {
            TriangleNet.Geometry.Vertex v1, v2, v3;
            vec3 p1, p2, p3, p4, p5, p6;
            double area = 0;
            double determintanteJacobiana = 0;
          //  triangulos.Clear();
            MessageBox.Show(mesh.Triangles.Count().ToString());
            foreach (var tri in mesh.Triangles)
            {
                v1 = tri.GetVertex(0);
                v2 = tri.GetVertex(1);
                v3 = tri.GetVertex(2);
                p1 = new vec3(v1.X, v1.Y, 0); 
                p3 = new vec3(v2.X, v2.Y, 0);
                p5 = new vec3(v3.X, v3.Y, 0);
                p2 = (p3+p1)/2; 
                p4 = (p5+ p3)/2;
                p6 = (p5+p1)/2;
                
                vec3 centro = new vec3((v1.X + v2.X + v3.X) / 3, (v1.Y + v2.Y + v3.Y) / 3, 0);

            //    triangulos.Add(new TTrianguloQuadratico( new TNoFEM(p1.x, p1.y), new TNoFEM(p2.x, p2.y), new TNoFEM(p3.x, p3.y),
              //                 new TNoFEM(p4.x, p4.y), new TNoFEM(p5.x, p5.y), new TNoFEM(p6.x, p6.y),area,centro));


            }

            double[,] matriz_jacobiana = new double[2,2];
            double qx = 0;
          /*  for (int i = 0; i < triangulos.Count; i++)
            {
                matriz_jacobiana[0, 0] = triangulos[i].n3.x - triangulos[i].n1.x;
                matriz_jacobiana[0, 1] = triangulos[i].n3.y - triangulos[i].n1.y;
                matriz_jacobiana[1, 0] = triangulos[i].n5.x - triangulos[i].n1.x;
                matriz_jacobiana[1, 1] = triangulos[i].n5.y - triangulos[i].n1.y;

                determintanteJacobiana = (triangulos[i].n3.x - triangulos[i].n1.x) * (triangulos[i].n5.y - triangulos[i].n1.y) -
                                         (triangulos[i].n5.x - triangulos[i].n1.x) * (triangulos[i].n3.y - triangulos[i].n1.y);
                area += (determintanteJacobiana / 2);
                triangulos[i].matriz_jacobiana = matriz_jacobiana;
                triangulos[i].det_jacobiana    = determintanteJacobiana;
                triangulos[i].Area             = (determintanteJacobiana / 2);

               // for (int j = 0; j < 3; j++)
               // {

                // }
            }*/
            double u = 0, v = 0;

            //funçoes forma triangulo quadratido subparametrico
            double n1 = 1 - u - v;
            double n3 = u;
            double n5 = v;

           // label1.Text = ((area*10000).ToString("n2")+ " cm²");
            double[] coefs = new double[4];
            coefs[0] = 2;
            coefs[1] = 1;
            coefs[2] = -3;
            coefs[3] = 4;


              Matrix<double> B = Matrix<double>.Build.Random(3, 8);
             Matrix<double> D  = Matrix<double>.Build.Random(3, 3);
              D[2, 2] = 20;
              MessageBox.Show(D[2,2].ToString());
             Matrix<double> Bt = B.Transpose(); //8x3
             Btf = Bt.Inverse();
             Matrix<double> Bt_x_D = (Bt * D);//8x3
             Matrix<double> mult = Bt * D * B;
            
             GaussLegendreRule rule = new GaussLegendreRule(-1, 1, 2);


            double[][] eqs;
            eqs = new double[1][];
            for (int i = 0; i < 1; i++)
            {
                eqs[i] = new double[4];
            }

            for (int i = 0; i < 1; i++)
            {
                for (int j = 0; j < 4; j++)
                {
                    eqs[i][j] = coefs[j];
                }
            }


          //  integrarGauss(ref eqs);

            Mostrar();
        }

        void Mostrar()
        {
          //  for (int i = 0; i < triangulos.Count; i++)
           // {

           // }
           // label1.Text=
        }

        private void button1_Click(object sender, EventArgs e)
        {
            Calcular();
            calculoOK = true;
        }
        void CriarShaders(string vs, string fs,
           out int vertexObject, out int fragmentObject,
           out int program)
        {
            int status_code;
            string info;

            vertexObject =  GL.CreateShader(ShaderType.VertexShader);
            fragmentObject = GL.CreateShader(ShaderType.FragmentShader);

            // compila vertex shader
            GL.ShaderSource(vertexObject, vs);
            GL.CompileShader(vertexObject);
            GL.GetShaderInfoLog(vertexObject, out info);
            GL.GetShader(vertexObject, ShaderParameter.CompileStatus, out status_code);

            if (status_code != 1)
                throw new ApplicationException(info);

            // compila fragment shader
            GL.ShaderSource(fragmentObject, fs);
            GL.CompileShader(fragmentObject);
            GL.GetShaderInfoLog(fragmentObject, out info);
            GL.GetShader(fragmentObject, ShaderParameter.CompileStatus, out status_code);

            if (status_code != 1)
                throw new ApplicationException(info);

            program = GL.CreateProgram();
            GL.AttachShader(program, fragmentObject);
            GL.AttachShader(program, vertexObject);

            GL.LinkProgram(program);
            GL.UseProgram(program);

            GL.DetachShader(program, vertexObject);
            GL.DetachShader(program, fragmentObject);
            GL.DeleteShader(fragmentObject);
            GL.DeleteShader(vertexObject);
        }
        int vertex_buffer_triangulos_principal, vertex_buffer_triangulos_deformacao, vertex_buffer_arestas;
        uint index_buffer_object;
        int vertex_shader_object_triangulos, fragment_shader_object_triangulos, shader_triangulos_program;
        int vertex_shader_object_arestas, fragment_shader_object_arestas, shader_arestas_program;
        int modeViewlLocation = 0, proj_location, modelLocation, LightPos_location, mat_translacao_location;

        private void glControl_Load(object sender, EventArgs e)
        {
            try
            {
                //ogl.Inicializa(glControl.Width, glControl.Height);
                //CriaPlanos();
                glControl.MakeCurrent();
                //  Preenche_Batch_Triangulos();
                //  AtualizaVBO();
              //  CriarShaders(ogl.vertex_shader, ogl.fragment_shader, out vertex_shader_object_triangulos, out fragment_shader_object_triangulos, out shader_triangulos_program);
               // CriarShaders(ogl.vertex_shader_arestas, ogl.fragment_shader_arestas, out vertex_shader_object_arestas, out fragment_shader_object_arestas, out shader_arestas_program);
            }
            catch (Exception er)
            {
                MessageBox.Show(er.Message);
            }
        }
    }
}
