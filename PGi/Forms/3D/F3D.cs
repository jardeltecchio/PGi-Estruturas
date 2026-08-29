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
using GeometryUtility;
using PolygonCuttingEar;
using System.Drawing.Imaging;
using System.IO;

using OpenTK;
using OpenTK.Input;
using OpenTK.Graphics.OpenGL;
using OpenTK.Platform;

namespace PG
{
    public unsafe partial class F3D : WeifenLuo.WinFormsUI.Docking.DockContent
    {
        public static int h, w;
        public bool TresDPavimento;
        public static float r, g, b;
        public Color BackColor = Color.White;
        private static float _dragPosX = (float).0, _dragPosY = (float).0, _dragPosZ = (float).0;
        double posX, posY, posZ, centroX, centroY;
        double x_angle = 15, y_angle = 15;

        double x_rot_angle, y_rot_angle, x_trans, y_trans;
        double zant = 0;
        int ultx, ulty;

        double start_x = 0, start_y = 0;
        double bx, by, bz;

        bool Panning;

        private static int[] texture = new int[3];
        private static int filter; 

        FRotacionarElementos FOpcVis3D;
        Gerenciador gerenciador;
        public List<TPavimento> Pavimentos;
        public TPavimento Pavimento;

        public static double[] ModelViewMatrix = new double[16];
        public static double[] ModelViewMatrix2 = new double[16];
        public static double[] ModelViewMatrixCubo = new double[16];

        public static double[] ModelViewMatrixSelecao = new double[16];
        public static double[] mvm_Selecao = new double[16];

        public static double[] ProjectionMatrix = new double[16];
        public static int[] ViewPort = new int[4];

        public F3D()
        {
            InitializeComponent();
        }

        public void CarregaTexturas()
        {
          /*  bool status = false;                                                // Status Indicator
            Bitmap[] textureImage = new Bitmap[1];                              // Create Storage Space For The Texture

            textureImage[0] = LoadBMP("Solo");                // Load The Bitmap
            // Check For Errors, If Bitmap's Not Found, Quit
            if (textureImage[0] != null)
            {
                status = true;                                                  // Set The Status To True

                GL.glGenTextures(1, texture);                            // Create The Texture

                textureImage[0].RotateFlip(RotateFlipType.RotateNoneFlipY);     // Flip The Bitmap Along The Y-Axis
                // Rectangle For Locking The Bitmap In Memory
                Rectangle rectangle = new Rectangle(0, 0, textureImage[0].Width, textureImage[0].Height);
                // Get The Bitmap's Pixel Data From The Locked Bitmap
                BitmapData bitmapData = textureImage[0].LockBits(rectangle, ImageLockMode.ReadOnly, PixelFormat.Format24bppRgb);

                // Typical Texture Generation Using Data From The Bitmap
                Gl.glBindTexture(Gl.GL_TEXTURE_2D, texture[0]);
                Gl.glTexImage2D(Gl.GL_TEXTURE_2D, 0, Gl.GL_RGB8, textureImage[0].Width, textureImage[0].Height, 0, Gl.GL_BGR, Gl.GL_UNSIGNED_BYTE, bitmapData.Scan0);
                Gl.glTexParameteri(Gl.GL_TEXTURE_2D, Gl.GL_TEXTURE_MIN_FILTER, Gl.GL_NEAREST);
                Gl.glTexParameteri(Gl.GL_TEXTURE_2D, Gl.GL_TEXTURE_MAG_FILTER, Gl.GL_NEAREST);
                Gl.glTexParameteri(Gl.GL_TEXTURE_2D, Gl.GL_TEXTURE_WRAP_T, Gl.GL_REPEAT);
                Gl.glTexParameteri(Gl.GL_TEXTURE_2D, Gl.GL_TEXTURE_WRAP_S, Gl.GL_REPEAT);

                if (textureImage[0] != null)
                {                                   // If Texture Exists
                    textureImage[0].UnlockBits(bitmapData);                     // Unlock The Pixel Data From Memory
                    textureImage[0].Dispose();                                  // Dispose The Bitmap
                }
            }*/
        }
        private static Bitmap LoadBMP(string fileName)
        {
            if (fileName == null || fileName == string.Empty)
            {                  // Make Sure A Filename Was Given
                return null;                                                    // If Not Return Null
            }
            fileName = "C:\\PGi\\Solo.bmp";
            string fileName1 = string.Format("Data{0}{1}",                      // Look For Data\Filename
                Path.DirectorySeparatorChar, fileName);
            string fileName2 = string.Format("{0}{1}{0}{1}Data{1}{2}",          // Look For ..\..\Data\Filename
                "..", Path.DirectorySeparatorChar, fileName);

            // Make Sure The File Exists In One Of The Usual Directories
            if (!File.Exists(fileName) && !File.Exists(fileName1) && !File.Exists(fileName2))
            {
                return null;                                                    // If Not Return Null
            }

            if (File.Exists(fileName))
            {                                         // Does The File Exist Here?
                return new Bitmap(fileName);                                    // Load The Bitmap
            }
            else
                if (File.Exists(fileName1))
                {                                   // Does The File Exist Here?
                    return new Bitmap(fileName1);                                   // Load The Bitmap
                }
                else
                if (File.Exists(fileName2))
                {                                   // Does The File Exist Here?
                    return new Bitmap(fileName2);                                   // Load The Bitmap
                }

            return null;                                                        // If Load Failed Return Null
        }
    
        public int NumPavimento;
        Plano plano, planoSelecao, planoRotacao;
        void CriaPlano()
        {
             vec3 normXY = new vec3(0.0f,0.0f,1.0f);
		     vec3 pt = new vec3(0.0f, 0.0f, 200);
		     plano = new Plano(normXY, pt, "");

             planoSelecao = new Plano(normXY, pt, "");

             vec3 normPlanoRotacao = new vec3(0.0f, 0, 1);
             vec3 ptPlanoRotacao = new vec3(0.0f, 0.0f, Pavimentos[0].PeDireito);
             planoRotacao = new Plano(normPlanoRotacao, ptPlanoRotacao, "");
        }

        public F3D(Gerenciador owner, bool tresdpav, List<TPavimento> pavimentos, int numPavimento = -1)
        {
            InitializeComponent();
          //  Controle.InitializeContexts();

            this.TresDPavimento = tresdpav;
            this.gerenciador = owner;
            this.Pavimentos  = pavimentos;
            
            this.NumPavimento = numPavimento;

            if (this.NumPavimento > -1)
                this.Pavimento = pavimentos[this.NumPavimento];

            //cria listas de objetos caso estiverem nulas...para nao dar erro
            foreach (TPavimento PavimentoAtual in pavimentos)
            {
                if (PavimentoAtual.vigas == null)
                    PavimentoAtual.vigas = new List<TTrechoViga>();

                if (PavimentoAtual.lajes == null)
                    PavimentoAtual.lajes = new List<TLaje>();

                if (PavimentoAtual.pilares == null)
                    PavimentoAtual.pilares = new List<TPilar>();
            }

            CriaPlano();

          //  TOpenGl.AssociateOGL(this.Controle);
         //   TOpenGl.InitializeOGL();
         //   Set3DProjection(Controle.Width, Controle.Height);//    TOpenGl.ReShapeOGL(Controle.Width, Controle.Height, Perspectiva);
        }

        private static float[] LightAmb = { 0.4f, 0.4f, 0.4f, 1 };                // Ambient Light
        private static float[] LightDif = { 1, 1, 1, 1 };                         // Diffuse Light
        private static float[] LightPos = { 4, 4, 6, 1 };
        float[] position = { 20,20,50 };
        float[] ambient = { 1f, 1, 1, 1 };
        float[] diffuse = { 1.0f, 1.0f, 1.0f, 1.0f };
        float[] specular = { 1.0f, 1.0f, 1.0f, 1.0f };

        /*
                 void InicializaOGL()
        {
            Gl.glShadeModel(Gl.GL_SMOOTH);
            Glut.glutInitDisplayMode(Glut.GLUT_SINGLE | Glut.GLUT_RGB | Glut.GLUT_DEPTH);

            Gl.glEnable(Gl.GL_TEXTURE_3D);
            Gl.glEnable(Gl.GL_DOUBLEBUFFER);

            Gl.glEnable(Gl.GL_BLEND);
            Gl.glBlendFunc(Gl.GL_SRC_ALPHA, Gl.GL_ONE_MINUS_SRC_ALPHA);

            Gl.glColorMaterial(Gl.GL_FRONT_AND_BACK, Gl.GL_AMBIENT_AND_DIFFUSE);
            Gl.glEnable(Gl.GL_COLOR_MATERIAL);

            Gl.glClearColor(1f, 1.0f, 1.0f, 0.0f);    // This Will Clear The Background Color To Black
            Gl.glClearDepth(1);            // Enables Clearing Of The Depth Buffer
            Gl.glEnable(Gl.GL_DEPTH_TEST);        // Enables Depth Testing
            Gl.glEnable(Gl.GL_NORMALIZE);
            Gl.glDisable(Gl.GL_CULL_FACE);
            Gl.glCullFace(Gl.GL_BACK);

            Gl.glDepthFunc(Gl.GL_LESS);

            Gl.glEnable(Gl.GL_POLYGON_OFFSET_FILL);
            Gl.glPolygonMode(Gl.GL_FRONT_AND_BACK, Gl.GL_FILL);


            x_rot_angle = 15;
            y_rot_angle = 90;
            cameraDistance = 3000;

            Gl.glEnable(Gl.GL_MULTISAMPLE);
            Gl.glEnable(Gl.GL_POLYGON_SMOOTH);
            Gl.glHint(Gl.GL_POLYGON_SMOOTH_HINT, Gl.GL_NICEST);
            //     Gl.glEnable(Gl.GL_LINE_SMOOTH);
            Gl.glEnable(Gl.GL_BLEND);
            Gl.glBlendFunc(Gl.GL_SRC_ALPHA, Gl.GL_ONE_MINUS_SRC_ALPHA);
        }
         */
        void InicializaOGL()
        {
            GL.ClearColor(Color.Black);

            GL.Enable(EnableCap.DepthTest);
            GL.Enable(EnableCap.Lighting);
            GL.Enable(EnableCap.Blend);
            GL.Enable(EnableCap.Light1);
            GL.Enable(EnableCap.ColorMaterial);
            GL.Enable(EnableCap.PolygonOffsetFill); 
            GL.Disable(EnableCap.CullFace);
            GL.Enable(EnableCap.Normalize);
            GL.Enable(EnableCap.AutoNormal);
            GL.Enable(EnableCap.Multisample);
            GL.Enable(EnableCap.PolygonSmooth);

            GL.Light(LightName.Light1, LightParameter.Position, new float[] { (float)(600-centroX), (float)(600-centroY), 1100 });
            GL.Light(LightName.Light1, LightParameter.Ambient, ambient);
            GL.Light(LightName.Light1, LightParameter.Diffuse, diffuse);
            GL.Light(LightName.Light1, LightParameter.Specular, specular);
            GL.LightModel(LightModelParameter.LightModelAmbient, new float[] { 0.2f, 0.2f, 0.2f, 1.0f });
            GL.LightModel(LightModelParameter.LightModelTwoSide, 1);
            GL.LightModel(LightModelParameter.LightModelLocalViewer, 1);
            GL.Hint(HintTarget.PerspectiveCorrectionHint, HintMode.Nicest);         // Really Nice Perspective Calculations

            GL.ShadeModel(ShadingModel.Smooth);

            GL.BlendFunc(BlendingFactor.SrcAlpha, BlendingFactor.OneMinusSrcAlpha);
            GL.ColorMaterial(MaterialFace.FrontAndBack, ColorMaterialParameter.AmbientAndDiffuse);

            GL.ClearDepth(1.0);
            GL.DepthFunc(DepthFunction.Less);
            GL.FrontFace(FrontFaceDirection.Cw);
            GL.CullFace(CullFaceMode.Back);

            GL.PolygonOffset(2, 1);
            GL.PolygonMode(MaterialFace.FrontAndBack, PolygonMode.Fill);
            GL.Hint(HintTarget.PolygonSmoothHint, HintMode.Nicest);
          //  GL.Hint(HintTarget.PerspectiveCorrectionHint, HintMode.Nicest);
            
            //    GL.FrontFace(FrontFaceDirection.Ccw);
         //   GL.Enable(EnableCap.AutoNormal);

            //GL.Enable(EnableCap.StencilTest);
            //GL.ClearStencil(0);
           // GL.StencilMask(0xFFFFFFFF); // read&write



       //     GL.Hint(HintTarget.MultisampleFilterHintNv, HintMode.Nicest);


            x_rot_angle = 0;
            y_rot_angle = 0;
            cameraDistance = 3000;

            // shader = new Shader("shader.vert", "shader.frag");
            ///   shader.Use();
            //   _loaded = true;

            ViewPort = new int[4];
        //    ModelViewMatrix = new double[16];
            ProjectionMatrix = new double[16];

            /*
            GL.Light(LightName.Light1, LightParameter.Position, position);
            GL.Light(LightName.Light1, LightParameter.Ambient, ambient);
            GL.Light(LightName.Light1, LightParameter.Diffuse, diffuse);
            GL.Light(LightName.Light1, LightParameter.Specular, specular);
            GL.Light(LightName.Light1, LightParameter.SpotExponent, new float[] { 1.0f, 1.0f, 1.0f, 1.0f });
            GL.LightModel(LightModelParameter.LightModelAmbient, new float[] { 0.2f, 0.2f, 0.2f, 1.0f });
            GL.LightModel(LightModelParameter.LightModelTwoSide, 1);
            GL.LightModel(LightModelParameter.LightModelLocalViewer, 1);

            GL.Enable(EnableCap.Light1);*/
/*
            GL.Material(MaterialFace.Front, MaterialParameter.Ambient, new float[] { 0.3f, 0.3f, 0.3f, 1.0f });
            GL.Material(MaterialFace.Front, MaterialParameter.Diffuse, new float[] { 1.0f, 1.0f, 1.0f, 1.0f });
            GL.Material(MaterialFace.Front, MaterialParameter.Specular, specular);
            GL.Material(MaterialFace.Front, MaterialParameter.Emission, new float[] { 0.0f, 0.0f, 0.0f, 1.0f });*/
            
            GL.Viewport(0, 0, Controle.Width, Controle.Height);
            Render();
  
            Controle.SwapBuffers();
            /*GL.ShadeModel(Gl.GL_SMOOTH);
            Glut.glutInitDisplayMode(Glut.GLUT_SINGLE | Glut.GLUT_RGB | Glut.GLUT_DEPTH);

            Gl.glEnable(Gl.GL_TEXTURE_2D);
            Gl.glEnable(Gl.GL_DOUBLEBUFFER);

            Gl.glEnable(Gl.GL_BLEND);
            Gl.glBlendFunc(Gl.GL_SRC_ALPHA, Gl.GL_ONE_MINUS_SRC_ALPHA);

            Gl.glColorMaterial(Gl.GL_FRONT_AND_BACK, Gl.GL_AMBIENT_AND_DIFFUSE);
            Gl.glEnable(Gl.GL_COLOR_MATERIAL);

            Gl.glClearColor(1f, 1.0f, 1.0f, 0.0f);    // This Will Clear The Background Color To Black
            Gl.glClearDepth(1);            // Enables Clearing Of The Depth Buffer
            Gl.glEnable(Gl.GL_DEPTH_TEST);        // Enables Depth Testing
       //     Gl.glEnable(Gl.GL_NORMALIZE);
            Gl.glDisable(Gl.GL_CULL_FACE);
            Gl.glCullFace(Gl.GL_BACK);

            Gl.glDepthFunc(Gl.GL_LESS);

            Gl.glEnable(Gl.GL_POLYGON_OFFSET_FILL);
            Gl.glPolygonMode(Gl.GL_FRONT_AND_BACK, Gl.GL_FILL);

            x_rot_angle = 15;
            y_rot_angle = 90;
            cameraDistance = 3000;

            Gl.glEnable(Gl.GL_POLYGON_SMOOTH);
            Gl.glHint(Gl.GL_POLYGON_SMOOTH_HINT, Gl.GL_NICEST);

            Gl.glFrontFace(Gl.GL_CW);
            Gl.glEnable(Gl.GL_LIGHTING);
            Gl.glEnable(Gl.GL_AUTO_NORMAL);
            Gl.glEnable(Gl.GL_NORMALIZE);
            Gl.glEnable(Gl.GL_DEPTH_TEST);

            float[] ambient = { 1f, 1, 1, 1 };
            float[] diffuse = { 1.0f, 1.0f, 1.0f, 1.0f };
            float[] specular = { 1.0f, 1.0f, 1.0f, 1.0f };
            //    float[] position = { 110000, 10000, 4800, 1 };
            //   float[] position = { 140000, 140000, 140000, 1 };

            float[] SHININESS = { 150 };

            float[] lmodel_ambient = { .2f, .2f, .2f, 1.0f };
            float[] local_view = { 0 };

            float[] mostAmbient = { .3f, .3f, .3f, 1.0f };
            float[] spot_direction = { 0, -1, 0 };

            Gl.glEnable(Gl.GL_LIGHT1);
            Gl.glLightfv(Gl.GL_LIGHT1, Gl.GL_AMBIENT, ambient);
            Gl.glLightfv(Gl.GL_LIGHT1, Gl.GL_DIFFUSE, diffuse);
            Gl.glLightfv(Gl.GL_LIGHT1, Gl.GL_POSITION, position);
            Gl.glLightfv(Gl.GL_LIGHT1, Gl.GL_SHININESS, SHININESS);
            Gl.glLightfv(Gl.GL_LIGHT1, Gl.GL_SPECULAR, specular);

            Gl.glMaterialfv(Gl.GL_FRONT, Gl.GL_SPECULAR, specular);
            Gl.glMaterialfv(Gl.GL_FRONT, Gl.GL_SHININESS, SHININESS);

           Gl.glLightModelfv(Gl.GL_LIGHT_MODEL_AMBIENT, lmodel_ambient);
           Gl.glLightModelfv(Gl.GL_LIGHT_MODEL_LOCAL_VIEWER, local_view);*/
        }
        private void F3D_Load(object sender, EventArgs e)
        {
     //       InicializaOGL();
        }

        private void F3D_Resize(object sender, EventArgs e)
        {

        }

        bool wheel = false;

        vec3 pt2D = new vec3(0, 0, 0);
        vec3 pt3D_1 = new vec3(0, 0, 0);
        vec3 pt3D_2 = new vec3(0, 0, 0);
        vec3 pt3D_OffSet = new vec3(0, 0, 0);
        double s;

        struct TrechosXPoligonos
        {
            public TTrechoViga trecho;
            public List<TPoligono> poligonos;

            public TrechosXPoligonos(TTrechoViga t)
            {
                this.trecho = t;
                poligonos = new List<TPoligono>();
            }
        }

        List<TrechosXPoligonos> PoligonosTrechos;
        double []px_x = new double[1];
        double[] px_y = new double[1];
        void GeraPoligonosSelecao()
        {
            PoligonosTrechos = new List<TrechosXPoligonos>();

            for (i = Pavimentos.Count - 1; i >= 0; i--)
            {
                foreach (TTrechoViga Trecho in Pavimentos[i].vigas)
                {
                    nivel = Pavimentos[i].Nivel;
                    PoligonosTrechos.Add(new TrechosXPoligonos(Trecho));
                    CoordenadaD []coord = new CoordenadaD[4];
                    List<TLinha> lin = new List<TLinha>();
                    TrechosXPoligonos ult = PoligonosTrechos[PoligonosTrechos.Count - 1];
                    int uu =0;
                    Project(ref px_x, ref px_y, Trecho.linhas_facecima.pIni.x - centroX, Trecho.linhas_facecima.pIni.y - centroY, nivel, ModelViewMatrix, ProjectionMatrix);
                    coord[uu++] = new CoordenadaD(px_x[0], px_y[0], 0);

                    Project(ref px_x, ref px_y, Trecho.linhas_facecima.pIni.x - centroX, Trecho.linhas_facecima.pIni.y - centroY, nivel - Trecho.Dados.h1, ModelViewMatrix, ProjectionMatrix);
                    coord[uu++] = new CoordenadaD(px_x[0], px_y[0], 0);

                    Project(ref px_x, ref px_y, Trecho.linhas_facecima.pFin.x - centroX, Trecho.linhas_facecima.pFin.y - centroY, nivel - Trecho.Dados.h1, ModelViewMatrix, ProjectionMatrix);
                    coord[uu++] = new CoordenadaD(px_x[0], px_y[0], 0);

                    Project(ref px_x, ref px_y, Trecho.linhas_facecima.pFin.x - centroX, Trecho.linhas_facecima.pFin.y - centroY, nivel, ModelViewMatrix, ProjectionMatrix);
                    coord[uu++] = new CoordenadaD(px_x[0], px_y[0],0);

                    lin.Add(new TLinha(new TPonto(coord[0].X, coord[0].Y,0), new TPonto(coord[1].X, coord[1].Y,0),-1));
                    lin.Add(new TLinha(new TPonto(coord[1].X, coord[1].Y, 0), new TPonto(coord[2].X, coord[2].Y, 0), -1));
                    lin.Add(new TLinha(new TPonto(coord[2].X, coord[2].Y, 0), new TPonto(coord[3].X, coord[3].Y, 0), -1));
                    lin.Add(new TLinha(new TPonto(coord[3].X, coord[3].Y, 0), new TPonto(coord[0].X, coord[0].Y, 0), -1));
                    ult.poligonos.Add(new TPoligono(ref coord, ref lin));
                }
//                foreach (TPilar Pilar in Pavimentos[i].pilares)
  //              foreach (TLaje Laje in Pavimentos[i].lajes)
            }

        }
        float zoomP = 40;
        double offset_x, offset_y;
        double xant, yant = 0;
        public void Controle_MouseWheel(object sender, System.Windows.Forms.MouseEventArgs e)
        {
            if (!Perspectiva)
                ApplyZoomOrtho(e.X, e.Y, e.Delta);
            else
            {
                if (e.Delta < 0)
                  zoomP -= 1;
                else
                  zoomP += 1;

                cameraDistance -= (e.Delta * .7);

                Render();

                xant = IntersecRaioPlano.x;
                yant = IntersecRaioPlano.y;
                zant = IntersecRaioPlano.z;

                raio.GerarRaio3D(ref mouseX, ref  mouseY, ref zNear, ref zFar, ref  ViewPort, ref ModelViewMatrix2, ref ProjectionMatrix);
                raio.CalculaIntersecao_Raio_x_Plano(ref plano, ref IntersecRaioPlano);

                offset_x = (IntersecRaioPlano.x - xant);
                offset_y = (IntersecRaioPlano.y - yant);

                x_trans += offset_x;
                y_trans += offset_y;

                wheel = true;

                Render();
                Controle.SwapBuffers(); 

              // GL.MatrixMode(MatrixMode.Projection);
              // OpenTK.Graphics.Glu.Perspective(45, Controle.Width / (float)Controle.Height, 0.1f, 10000f);
             //   p = Matrix4.CreatePerspectiveFieldOfView(zoomP, Controle.Width / (float)Controle.Height, 0.1f, 10000f);
             //  GL.LoadMatrix(ref p);

              //  realY = ViewPort[3] - (int)e.Y;

             //   winZ = new double[1];
             //   posX3d = new double[1];
             //   posY3d = new double[1];
             //   posZ3d = new double[1];
             //   GL.ReadPixels(e.X, e.Y, 1, 1, OpenTK.Graphics.OpenGL.PixelFormat.DepthComponent, PixelType.Float, winZ);
                //  g2d.UnProject(e.X, e.Y, ref posX, ref posY, ref posZ);
            //    OpenTK.Graphics.Glu.UnProject(e.X, realY, winZ[0], ModelViewMatrix, ProjectionMatrix, ViewPort, posX3d, posY3d, posZ3d);

              //  Project(ref px_x, ref px_y, posX3d[0], posY3d[0], posZ3d[0]);

       //         gerenciador.Text = e.X + " - " + e.Y + " x:" + px_x[0].ToString("n5") + "y:" + px_y[0].ToString("n5");
 //               gerenciador.Text = e.X + " - " + e.Y + " x:" + posX3d[0].ToString("n5") + "y:" + posY3d[0].ToString("n5") + "z:" + posZ3d[0].ToString("n5");
            }

            GeraPoligonosSelecao();
        }

     /*   private Vector2 planeUnproject(Vector2 win)
        {
            vec3 win1 = new vec3(0,0,0);
            UnProject(mouseX, mouseY, ModelViewMatrix, ProjectionMatrix, posX3d, posY3d, posZ3d);
        }*/

        double cameraDistance;
        private void ApplyZoomFustrum(int mouseX, int mouseY, int delta)
        {
             cameraDistance -= delta;
        }

        int mouseX, mouseY;
        double cameraAngleY,cameraAngleX;
        void rotateCamera(int x, int y)
        {
           cameraAngleY += (x - mouseX);
           cameraAngleX += (y - mouseY);
           mouseX = x;
           mouseY = y;
        }

        void zoomCamera(int y)
        {
           cameraDistance -= (y - mouseY) * 0.1f;
           mouseY = y;
        }

        void zoomCameraDelta(float delta)
        {
            cameraDistance -= delta;
            GL.Translate(0,0, cameraDistance);

            Render();
           // TOpenGl.OGL.Refresh();
        }

        private void Controle_Paint(object sender, PaintEventArgs e)
        {
            Controle.MakeCurrent();
        }

        int pav = 0;
        public void GerarModelo3D(bool enquadrar)
        {
            try
            {
                //Perspectiva = false;// gerenciador.ConfiguracoesPGi.F3DOpcoesVisualizacao.Perspectiva;
                SetPerspective(Controle.Width, Controle.Height);

            //    Gl.glEnable(Gl.GL_BLEND);
             //   Gl.glBlendFunc(Gl.GL_SRC_ALPHA, Gl.GL_ONE_MINUS_SRC_ALPHA); 
            //    if (gerenciador.ConfiguracoesPGi.F3DOpcoesVisualizacao.RgbLajes == null)
             //   {
              //      gerenciador.ConfiguracoesPGi.F3DOpcoesVisualizacao = new SF3DOpcoesVisualizacao(255, 255, 255);
              //  }

                gerenciador.ChamaAguardar(this, "Atualizando a estrutura 3D. Aguarde...");

                if (NumPavimento > -1)
                {
                    centroX = Pavimentos[NumPavimento].xi + ((Pavimentos[NumPavimento].xf - Pavimentos[NumPavimento].xi) / 2);
                    centroY = Pavimentos[NumPavimento].yi + ((Pavimentos[NumPavimento].yf - Pavimentos[NumPavimento].yi) / 2);
                }
                else
                {                  
                    for (pav = gerenciador.Pavimentos.Count - 1; pav >= 0; pav--)
                      if (gerenciador.Pavimentos[pav].vigas.Count > 0)
                         break;
                    if (pav == -1)
                        pav = 0;

                    centroX = gerenciador.Pavimentos[pav].xi + ((gerenciador.Pavimentos[pav].xf - gerenciador.Pavimentos[pav].xi) / 2);
                    centroY = gerenciador.Pavimentos[pav].yi + ((gerenciador.Pavimentos[pav].yf - gerenciador.Pavimentos[pav].yi) / 2);
                }

             //   Gl.glMaterialfv(Gl.GL_FRONT_AND_BACK, Gl.GL_EMISSION, mostAmbient);

                /* foreach(TPavimento pav in Pavimentos)
                  foreach (TPilar Pilar in pav.pilares)
                  {
                      Pilar.ht = new double[2];

                      if (pav.SequenciaPavimento > 1)
                        Pilar.AlturaLance = Pavimentos[pav.SequenciaPavimento-2].PeDireito;
                  }*/
                  //Pilar.TriangularizaFace();*/

                Triangularizacoes();

                InicializaOGL();
              //  GL.Translate(0, 0, -1100);
                GL.Rotate(-45,0, 1, 0);
                Render();
                
                GL.Flush();

                Controle.SwapBuffers();

                if ((Object)Pavimento != null)
                  this.Text = "PGi - Vizualização 3D - "+ Pavimento.Descricao;
    
                gerenciador.FechaAguardar();

                GeraPoligonosSelecao();

                CarregaTexturas();

                VistaCima();

            }
            catch (Exception ms)
            {
                MessageBox.Show(ms.Message);
                gerenciador.FechaAguardar();
            }
        }

        public void Triangularizacoes()
        {
            foreach (TPavimento pav in Pavimentos)
            {
                foreach (TLaje Laje in pav.lajes)
                {
                    Laje.TriangularizarLaje();
                    //      Laje.CarregaTexturas();
                }

                foreach (TTrechoViga Trecho in pav.vigas)
                {
                    Trecho.CarregaTexturas();
                    Trecho.TriangularizaFaceCima();
                    //    Trecho.TriangularizaLado1();
                    //      Trecho.TriangularizaLado2();
                }

                foreach (TPilar Pilar in pav.pilares)
                    Pilar.CarregaTexturas();
            }
        }

        Matrix4 p;
        int i;
        double GLtop,GLbotton,GLleft,GLright, zNear, zFar;
        void SetupCamera(bool orto = true)
        {
            GL.Viewport(0, 0, Controle.Width, Controle.Height);
            GL.MatrixMode(MatrixMode.Projection); 

            if (!Perspectiva)
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
                zNear = 70;
                zFar = 10000f;
                 // OpenTK.Graphics.Glu.Perspective(MathHelper.PiOver4, Controle.Width / (float)Controle.Height, 0.1f, 10000f); 
                p = Matrix4.CreatePerspectiveFieldOfView(45 * Const.PIDiv180, Controle.Width / (float)Controle.Height, (float)zNear, (float)zFar);
               GL.LoadMatrix(ref p);
            }
            //GL.MatrixMode(MatrixMode.Modelview);
            ///  GL.LoadIdentity(); 
   //          GL.Ortho(-1.0, 1.0, -1.0, 1.0, 0.0, 1000);

            //            Matrix4 lookat = Matrix4.LookAt(0, 0, 0, 0, 0, 0, 0, 0, 0);

        }
        void Draw()
        {
            try
            {
          /*      if (gerenciador.ConfiguracoesPGi.F3DOpcoesVisualizacao.MostrarTexturas)
                {
                    if (NumPavimento == -1)
                    {
                        for (i = Pavimentos.Count - 1; i >= 0; i--)
                        {
                            nivel = Pavimentos[i].Nivel;
                            //   GL.PolygonOffset(2, 1);
                            //  GL.PolygonMode(MaterialFace.FrontAndBack, PolygonMode.Fill);

                            //  Gl.glPolygonOffset(2, 1);
                            //    Gl.glPolygonMode(Gl.GL_FRONT_FACE, Gl.GL_FILL);

                            foreach (TTrechoViga Trecho in Pavimentos[i].vigas)
                                Trecho.Render3DTextura(centroX, centroY, nivel,
                                    gerenciador.ConfiguracoesPGi.F3DOpcoesVisualizacao.TranspVigas,
                                    gerenciador.ConfiguracoesPGi.F3DOpcoesVisualizacao.RgbVigas,
                                    gerenciador.ConfiguracoesPGi.F3DOpcoesVisualizacao.ArestasVigas);

                            foreach (TPilar Pilar in Pavimentos[i].pilares)
                                Pilar.Render3DTextura(centroX, centroY, nivel,
                                    gerenciador.ConfiguracoesPGi.F3DOpcoesVisualizacao.TranspPilares,
                                    gerenciador.ConfiguracoesPGi.F3DOpcoesVisualizacao.RgbPilares,
                                    gerenciador.ConfiguracoesPGi.F3DOpcoesVisualizacao.Arestas);

                            foreach (TLaje Laje in Pavimentos[i].lajes)
                                Laje.Render3DTextura(centroX, centroY, nivel,
                                    gerenciador.ConfiguracoesPGi.F3DOpcoesVisualizacao.TranspLajes,
                                    gerenciador.ConfiguracoesPGi.F3DOpcoesVisualizacao.RgbLajes);
                        }
                    }
                    else
                    {
                        //  Gl.glPolygonOffset(2, 1);
                        //  Gl.glPolygonMode(Gl.GL_FRONT_FACE, Gl.GL_FILL);

                        foreach (TTrechoViga Trecho in Pavimentos[NumPavimento].vigas)
                            Trecho.Render3DTextura(centroX, centroY, 0, gerenciador.ConfiguracoesPGi.F3DOpcoesVisualizacao.TranspVigas,
                                gerenciador.ConfiguracoesPGi.F3DOpcoesVisualizacao.RgbVigas,
                                gerenciador.ConfiguracoesPGi.F3DOpcoesVisualizacao.ArestasVigas);

                        foreach (TPilar Pilar in Pavimentos[NumPavimento].pilares)
                            Pilar.Render3DTextura(centroX, centroY, 0, gerenciador.ConfiguracoesPGi.F3DOpcoesVisualizacao.TranspPilares,
                                gerenciador.ConfiguracoesPGi.F3DOpcoesVisualizacao.RgbPilares,
                                gerenciador.ConfiguracoesPGi.F3DOpcoesVisualizacao.Arestas);

                        foreach (TLaje Laje in Pavimentos[NumPavimento].lajes)
                            Laje.Render3DTextura(centroX, centroY, 0, gerenciador.ConfiguracoesPGi.F3DOpcoesVisualizacao.TranspLajes,
                                gerenciador.ConfiguracoesPGi.F3DOpcoesVisualizacao.RgbLajes);
                    }
                }
                else
                {
                    if (NumPavimento == -1)
                    {
                        for (i = Pavimentos.Count - 1; i >= 0; i--)
                        {
                            nivel = Pavimentos[i].Nivel;

                            //  Gl.glPolygonOffset(2, 1);
                            //    Gl.glPolygonMode(Gl.GL_FRONT_FACE, Gl.GL_FILL);

                            foreach (TTrechoViga Trecho in Pavimentos[i].vigas)
                                Trecho.Render3D(centroX, centroY, nivel,
                                    gerenciador.ConfiguracoesPGi.F3DOpcoesVisualizacao.TranspVigas,
                                    gerenciador.ConfiguracoesPGi.F3DOpcoesVisualizacao.RgbVigas,
                                    gerenciador.ConfiguracoesPGi.F3DOpcoesVisualizacao.ArestasVigas);

                            foreach (TPilar Pilar in Pavimentos[i].pilares)
                                Pilar.Render3D(centroX, centroY, nivel,
                                    gerenciador.ConfiguracoesPGi.F3DOpcoesVisualizacao.TranspPilares,
                                    gerenciador.ConfiguracoesPGi.F3DOpcoesVisualizacao.RgbPilares,
                                    gerenciador.ConfiguracoesPGi.F3DOpcoesVisualizacao.Arestas);

                            foreach (TLaje Laje in Pavimentos[i].lajes)
                                Laje.Render3D(centroX, centroY, nivel,
                                    gerenciador.ConfiguracoesPGi.F3DOpcoesVisualizacao.TranspLajes,
                                    gerenciador.ConfiguracoesPGi.F3DOpcoesVisualizacao.RgbLajes);
                        }
                    }
                    else
                    {
                        //  Gl.glPolygonOffset(2, 1);
                        //  Gl.glPolygonMode(Gl.GL_FRONT_FACE, Gl.GL_FILL);

                        foreach (TTrechoViga Trecho in Pavimentos[NumPavimento].vigas)
                            Trecho.Render3D(centroX, centroY, 0, gerenciador.ConfiguracoesPGi.F3DOpcoesVisualizacao.TranspVigas,
                                gerenciador.ConfiguracoesPGi.F3DOpcoesVisualizacao.RgbVigas,
                                gerenciador.ConfiguracoesPGi.F3DOpcoesVisualizacao.ArestasVigas);

                        foreach (TPilar Pilar in Pavimentos[NumPavimento].pilares)
                            Pilar.Render3D(centroX, centroY, 0, gerenciador.ConfiguracoesPGi.F3DOpcoesVisualizacao.TranspPilares,
                                gerenciador.ConfiguracoesPGi.F3DOpcoesVisualizacao.RgbPilares,
                                gerenciador.ConfiguracoesPGi.F3DOpcoesVisualizacao.Arestas);

                        foreach (TLaje Laje in Pavimentos[NumPavimento].lajes)
                            Laje.Render3D(centroX, centroY, 0, gerenciador.ConfiguracoesPGi.F3DOpcoesVisualizacao.TranspLajes,
                                gerenciador.ConfiguracoesPGi.F3DOpcoesVisualizacao.RgbLajes);
                    }
                }*/
            }
            catch( Exception excep)
            {
                MessageBox.Show("Erro Draw:" + excep.Message);
            }
        }

        void HandleMouseDrawing()
        {

        }

        int j;
        void DrawSky()
        {
          //  GL.Color3(Color.Red);
            GL.Begin(PrimitiveType.Polygon);
            GL.Color3(Color.CadetBlue);
            GL.Vertex3(-10000, 8000, -5500);
            GL.Color3(Color.White);
            GL.Vertex3(-10000, -8000, -5500);
            GL.Vertex3(10000, -8000, -5500);
            GL.Color3(Color.CadetBlue);
            GL.Vertex3(10000, 8000, -5500);
            GL.Vertex3(-10000, 8000, -5500);
            GL.End();
        }
        public bool shift;

            double[]winZ = new double[1];
            double realY;

            double []posX3d = new double[1];
            double []posY3d = new double[1];
            double [] posZ3d = new double[1];
        void UnProject(int mx, int my, double [] model, double [] proj, double []x_, double[] y_, double []z_)
        {
            realY = ViewPort[3] - (int)my;


            GL.ReadPixels(mx, my, 1, 1, OpenTK.Graphics.OpenGL.PixelFormat.DepthComponent, PixelType.Float, winZ);

          //  g2d.UnProject(mouseX, mouseY, ref posX, ref posY, ref posZ);

            OpenTK.Graphics.Glu.UnProject(mx, realY, winZ[0], model, proj, ViewPort, x_, y_, z_);
          //  Project(ref px_x, ref px_y, posX3d[0], posY3d[0], posZ3d[0], ModelViewMatrix, ProjectionMatrix);
            // gerenciador.Text = e.X + " - " + e.Y + " x:" + px_x[0].ToString("n5") + "y:" +(ViewPort[3] -  px_y[0]).ToString("n5");
        }
        bool Perspectiva = true;
        double sz = 0;
        private void ApplyZoomOrtho(int mouseX, int mouseY, int delta)
        {
            
            sz = System.Math.Exp((double)delta * 0.001);

            GL.Translate((float)posX3d[0], (float)posY3d[0], (float)posZ3d[0]);
            GL.Scale((float)sz, (float)sz, (float)sz);
            GL.Translate(-(float)posX3d[0], -(float)posY3d[0], -(float)posZ3d[0]);

            Render();
            Controle.SwapBuffers();
            //TOpenGl.OGL.Refresh();
        }
        Raio raio = new Raio(0,0);
        vec3 IntersecRaioPlano = new vec3(0, 0, 0);
        vec3 IntersecRaioPlanoSelecao = new vec3(0, 0, 0);
        vec3 IntersecRaioPlanoRotacao = new vec3(0, 0, 0);
        public void Render()
        {
            try
            {
                GL.Clear(ClearBufferMask.ColorBufferBit | ClearBufferMask.DepthBufferBit);
                GL.MatrixMode(MatrixMode.Modelview);
                GL.ClearColor(BackColor);

                if (Perspectiva)
                {
                    GL.LoadIdentity();
                    GL.PushMatrix();

                    if (rodando)
                    {
                        GL.Translate(x_trans, y_trans, -cameraDistance);

                      //  GL.Translate(IntersecRaioPlanoRotacao.x, IntersecRaioPlanoRotacao.y, 0);
                        GL.Rotate(x_rot_angle, 1, 0.0, 0.0);
                        GL.Rotate(y_rot_angle, 0.0, 0, 1);
                     //   GL.Translate(-IntersecRaioPlanoRotacao.x, -IntersecRaioPlanoRotacao.y, 0);
                        /*   GL.Translate(x_trans, y_trans, -cameraDistance);


                           GL.Translate(IntersecRaioPlano2.x, IntersecRaioPlano2.y, 0);
                           //  GL.Translate(-10, -20, 0);
                           GL.Rotate(x_rot_angle, 1, 0.0, 0.0);
                           GL.Rotate(y_rot_angle, 0.0, 0, 1);
                           //  GL.Translate(10, 20, 0); 
                           GL.Translate(-IntersecRaioPlano2.x, -IntersecRaioPlano2.y, 0);*/
                    }
                    else
                    {
                        GL.Translate(x_trans, y_trans, -cameraDistance);
                        GL.Rotate(x_rot_angle, 1, 0.0, 0.0);
                        GL.Rotate(y_rot_angle, 0.0, 0, 1);
                    }
                }
                else
                {
                  //  GL.LoadIdentity(); 
                 //   GL.PushMatrix();

                  /*  GL.Translate((float)posX3d[0], (float)posY3d[0], (float)posZ3d[0]);
                    GL.Scale((float)sz, (float)sz, (float)sz);
                    GL.Translate(-(float)posX3d[0], -(float)posY3d[0], -(float)posZ3d[0]);*/

                  /*  GL.Rotate(x_rot_angle, 1, 0.0, 0.0);
                    GL.Rotate(y_rot_angle, 0.0, 1, 0.0);*/
                }
               
               // GL.Enable(EnableCap.PolygonOffsetFill);
                //GL.PolygonMode(MaterialFace.FrontAndBack, PolygonMode.Fill);

                Draw();
               // planoRotacao.desenha();
             //   planoRotacao.DesenhaNormal();
                UpdateOGLMatrix(ModelViewMatrix);

                UnProject(mouseX, mouseY, ModelViewMatrix, ProjectionMatrix, posX3d, posY3d, posZ3d);
                gerenciador.Text = mouseX + " - " + mouseY + " x:" + posX3d[0].ToString("n5") + "y:" + posY3d[0].ToString("n5") + "z:" + posZ3d[0].ToString("n5");
                GL.PopMatrix();

                /*------------------------------------------------------------*/

                GL.PushMatrix();
                GL.Translate(x_trans, y_trans, -cameraDistance);
               //  plano.desenha();
               //  plano.DesenhaNormal();

                raio.GerarRaio3D(ref mouseX, ref mouseY, ref zNear, ref zFar, ref  ViewPort, ref  ModelViewMatrix2, ref ProjectionMatrix);
                raio.CalculaIntersecao_Raio_x_Plano(ref plano, ref IntersecRaioPlano);
                UpdateOGLMatrix(ModelViewMatrix2);
                GL.PopMatrix();


                /*----------------------------*/                
                GL.PushMatrix();
                GL.Translate(x_trans, y_trans, -300);
             //   planoSelecao.desenha();
            //    planoSelecao.DesenhaNormal();
                raio.GerarRaio3D(ref mouseX, ref mouseY, ref zNear, ref zFar, ref  ViewPort, ref  ModelViewMatrixSelecao, ref ProjectionMatrix);
                raio.CalculaIntersecao_Raio_x_Plano(ref planoSelecao, ref IntersecRaioPlanoSelecao);
               
                GL.Disable(EnableCap.Lighting);
                GL.Begin(PrimitiveType.LineLoop);
                GL.Color3(Color.Red);
                GL.Vertex3(IntersecRaioPlanoSelecao.x-1, IntersecRaioPlanoSelecao.y-1, IntersecRaioPlanoSelecao.z);
                GL.Vertex3(IntersecRaioPlanoSelecao.x+1, IntersecRaioPlanoSelecao.y-1, IntersecRaioPlanoSelecao.z);
                GL.Vertex3(IntersecRaioPlanoSelecao.x+1, IntersecRaioPlanoSelecao.y+1, IntersecRaioPlanoSelecao.z);
                GL.Vertex3(IntersecRaioPlanoSelecao.x-1, IntersecRaioPlanoSelecao.y+1, IntersecRaioPlanoSelecao.z);
           //     GL.Vertex3(IntersecRaioPlanoSelecao.x, IntersecRaioPlanoSelecao.y + 1, IntersecRaioPlanoSelecao.z);
                GL.End();
                GL.Enable(EnableCap.Lighting); 
                
                UpdateOGLMatrix(ModelViewMatrixSelecao);
                GL.PopMatrix();
                /*----------------------------*/


                GL.PushMatrix();
                GL.Translate(34, 18, -50);
             //   GL.Translate(x_trans, y_trans, -cameraDistance);
                GL.Rotate(x_rot_angle, 1, 0.0, 0.0);
                GL.Rotate(y_rot_angle, 0.0, 0, 1);
                DrawCube();
                UpdateOGLMatrix(ModelViewMatrixCubo);
                GL.PopMatrix();


              /*  GL.PushMatrix();
                GL.Translate(34, 18, -50);
                GL.Rotate(x_rot_angle, 1, 0.0, 0.0);
                GL.Rotate(y_rot_angle, 0.0, 0, 1);
                DesenhaEixos();
                GL.PopMatrix();*/

              //  drawAxis();

                /*----------------------------*/

         /*       if (gerenciador.ConfiguracoesPGi.F3DOpcoesVisualizacao.FundoDegrade2)
                {
                    GL.PushMatrix();
                    DrawSky();
                    GL.PopMatrix();
                }
                */

             //   if (selecionando)
                {
                   /* GL.PushMatrix();
                    UpdateOGLMatrix(mvm_Selecao);
                    UnProject(mouseX, mouseY, mvm_Selecao, ProjectionMatrix, posX3d, posY3d, posZ3d);
                    this.Text = mouseX + " - " + mouseY + " x:" + posX3d[0].ToString("n5") + "y:" + posY3d[0].ToString("n5") + "z:" + posZ3d[0].ToString("n5");


                    RetanguloSelecao(cli1xSelecao, cli1ySelecao);
                    GL.PopMatrix();*/
                }
                //    Gl.glClearColor(r, g, b, 0.0f);    // This Will Clear The Background Color To Black

                //    Gl.glMatrixMode(Gl.GL_MODELVIEW);
             //   Gl.glLoadIdentity();
           //     Gl.glLightfv(Gl.GL_LIGHT1, Gl.GL_POSITION, position);
                //    float[] lightPosition = { (float)numericUpDown1.Value / 10, (float)numericUpDown2.Value / 10, (float)numericUpDown3.Value / 10, 0.0f };

                //Gl.glLightfv(Gl.GL_LIGHT0, Gl.GL_POSITION, lightPosition); 

                //   Set3DProjection(Controle.Width, Controle.Height);


                /*  if (checkBox1.Checked )
                  {
                      double w = Controle.Width;
                      double h = Controle.Height;
                      double wdiv2 = w / 2;
                      double hdiv2 = h / 2;
                      double ox = mouseX - wdiv2;
                      double oy = mouseY - hdiv2;

                    //  Gl.glTranslated(ox, oy, 0);
                      Gl.glTranslated(posX, posY, -cameraDistance);
                  //     Gl.glTranslated(x_trans, y_trans, -cameraDistance);
                //           Gl.glTranslated(-posX, -posY, -posZ);
                  }*/
                //  else
                //   Gl.glTranslated(0, 0, -cameraDistance);

                /*  g2d.UnProject((int)pt2D.x, (int)pt2D.y, ref posX, ref posY, ref posZ);
                  pt3D_2.x = posX;
                  pt3D_2.y = posY;
                  pt3D_2.z = posZ;

                  pt3D_OffSet = pt3D_1 - pt3D_2;*/

                //Gl.glTranslated(pt3D_OffSet.x, pt3D_OffSet.y, pt3D_OffSet.y);

                // Gl.glRotated(x_rot_angle, 1, 0.0, 0.0);
                //  Gl.glRotated(y_rot_angle, 0.0, 1, 0.0);


                //   foreach (TPilar Pilar in Pavimento.pilares)
                ///       Pilar.Render3D(centroX, centroY);
                /*    float[] lowAmbient = { 0.1f, 0.1f, 0.1f, 1.0f };
                    float[] moreAmbient = { 0.4f, 0.4f, 0.4f, 1.0f };
                    float[] mostAmbient = { 1.0f, 1.0f, 1.0f, 1.0f };
                    */


                 // SetPerspective(Controle.Width, Controle.Height);


             //   this.Text = x_trans + " - " + y_trans;

                       // Gl.glPolygonOffset(-15, -1);
                       //     Gl.glEnable(Gl.GL_POLYGON_OFFSET_FILL);
                         //   Gl.glPolygonMode(Gl.GL_FRONT_FACE, Gl.GL_FILL);

              //    Gl.glMaterialfv(Gl.GL_FRONT, Gl.GL_AMBIENT, LightAmb);
                //  Gl.glMaterialf(Gl.GL_FRONT, Gl.GL_SHININESS, 40.0f);

                nivel = 0;
                if (gerenciador.ConfiguracoesPGi.F3DOpcoesVisualizacao.MostrarChao && Pavimentos.Count>1)
                {
                /*    Gl.glBindTexture(Gl.GL_TEXTURE_2D, texture[0]);

                    Gl.glColor4ub(189, 183, 107, 100);
                    Gl.glBegin(Gl.GL_POLYGON);
                    Gl.glTexCoord2f(0, 0); Gl.glVertex3d(Pavimentos[pav].yi * 20 - centroY * 20, Pavimentos[Pavimentos.Count - 2].Nivel - 5, Pavimentos[pav].xi * 20 - centroX * 20);
                    Gl.glTexCoord2f(1, 0); Gl.glVertex3d(Pavimentos[pav].yi * 20 - centroY * 20, Pavimentos[Pavimentos.Count - 2].Nivel - 5, Pavimentos[pav].xf * 20 - centroX * 20);
                    Gl.glTexCoord2f(1, 1); Gl.glVertex3d(Pavimentos[pav].yf * 20 - centroY * 20, Pavimentos[Pavimentos.Count - 2].Nivel - 5, Pavimentos[pav].xf * 20 - centroX * 20);
                    Gl.glTexCoord2f(0, 1); Gl.glVertex3d(Pavimentos[pav].yf * 20 - centroY * 20, Pavimentos[Pavimentos.Count - 2].Nivel - 5, Pavimentos[pav].xi * 20 - centroX * 20);
                    Gl.glTexCoord2f(0, 0); Gl.glVertex3d(Pavimentos[pav].yi * 20 - centroY * 20, Pavimentos[Pavimentos.Count - 2].Nivel - 5, Pavimentos[pav].xi * 20 - centroX * 20);
                    Gl.glEnd();*/
                }

                /*face de baixo*/
                /* Gl.glBegin(Gl.GL_POLYGON);
                 Gl.glColor3ub(0, 0, 255);
                 Gl.glVertex3d(-10000, -10000, 10000);
                 Gl.glVertex3d(10000, -10000, 10000);
                 Gl.glVertex3d(10000, -10000, -10000);
                 Gl.glVertex3d(10000, -10000, -10000);
                 Gl.glVertex3d(-10000, -10000, -10000);
                 Gl.glEnd();*/
       

              //  Controle.SwapBuffers();

                //  Gl.glPushMatrix();
                // Gl.glLoadIdentity();
                //  Gl.glPopMatrix();
                /*
                            Gl.glColor3ub(255,0,0);
                            Gl.glBegin(Gl.GL_LINES);
                            Gl.glVertex3d(1000      , 4000, 1000);
                            Gl.glVertex3d(1000 + 500, 4000, 1000);
                            Gl.glEnd();

                            Gl.glBegin(Gl.GL_LINES);
                            Gl.glVertex3d(1000, 4000    , 1000);
                            Gl.glVertex3d(1000, 4000+200, 1000);
                            Gl.glEnd();

                            Gl.glBegin(Gl.GL_LINES);
                            Gl.glVertex3d(1000, 4000, 1000);
                            Gl.glVertex3d(1000, 4000, 1000 + 500);
                            Gl.glEnd();*/
            }
            catch (Exception e)
            {
                MessageBox.Show("Erro ao desenhar estrutura: " + e.Message);
                this.Close();
            }
        }

        void drawAxis()
        {
            GL.Viewport(0, 0, 50, 50);

            GL.MatrixMode(MatrixMode.Projection);
            GL.PushMatrix();

            //      GL.Ortho(GLleft, GLright, GLbotton, GLtop, zNear, zFar);

            p = Matrix4.CreatePerspectiveFieldOfView(MathHelper.PiOver4, Controle.Width / (float)Controle.Height, 0.1f, 10000f);
            GL.LoadMatrix(ref p);

            //GL.gluPerspective(45.0f, 1.0f, 0.1f, 20.0f);

            GL.MatrixMode(MatrixMode.Modelview);
            GL.PushMatrix();
            //This really has to come from your camera....
           // gluLookAt(10.0f, 10.0f, 10.0f, 0.0f, 0.0f, 0.0f, 0.0f, 0.1f, 0.0f);

            GL.LineWidth(2);
            GL.Begin(PrimitiveType.Lines);
            GL.Color3(Color.Red);
            GL.Vertex3(-1000, 0, 0);
            GL.Vertex3(1000, 0, 0);
            GL.End();

            //Restore View
            GL.MatrixMode(MatrixMode.Modelview);
            GL.PopMatrix();
            GL.MatrixMode(MatrixMode.Projection);
            GL.PopMatrix();

          //  SetupCamera();
          //  glViewport(0, 0, 960, 600);
        }

        void DesenhaEixos()
        {
            GL.LineWidth(2);
            GL.Begin(PrimitiveType.Lines);
            GL.Color3(Color.Red);
            GL.Vertex3(-1.0f, -1.0f, -1.0f);
            GL.Vertex3(-1.0f, 1.0f, -1.0f);
            GL.End();

            GL.Begin(PrimitiveType.Lines);
            GL.Color3(Color.Blue);
            GL.Vertex3(-1.0f, -1.0f, -1.0f);
            GL.Vertex3(1.0f, -1.0f, -1.0f);
            GL.End();

            GL.Begin(PrimitiveType.Lines);
            GL.Color3(Color.Green);
            GL.Vertex3(-1.0f, -1.0f, -1.0f);
            GL.Vertex3(-1.0f, -1.0f, 1.0f);
            GL.End();
        }

        Vector3d p1, p2, p3, n1, v1, v2;
        void RetanguloSelecao(int x1, int y1)
        {
            GL.Color3(Color.Red);
            GL.LineWidth(2);
            GL.Begin(PrimitiveType.Lines);

            UnProject(100,100,ModelViewMatrix, ProjectionMatrix, posX3d, posY3d, posZ3d);
            GL.Vertex3(posX3d[0], posY3d[0], posZ3d[0]);
            UnProject(5000, 500, ModelViewMatrix, ProjectionMatrix, posX3d, posY3d, posZ3d);
            GL.Vertex3(posX3d[0], posY3d[0], posZ3d[0]);
            
//            GL.Vertex3(posX3d[0], posY3d[0], posZ3d[0]);
            GL.End();
        }
        double tamCubo = 500;
        private void DrawCube()
        {
            GL.Begin(PrimitiveType.LineLoop);
            GL.Color3(1, 1, 1);

            GL.Vertex3(-1.0f, -1.0f, -1.0f);
            GL.Vertex3(-1.0f, 1.0f, -1.0f);
            GL.Vertex3(1.0f, 1.0f, -1.0f);
            GL.Vertex3(1.0f, -1.0f, -1.0f);
            GL.End();

            GL.Begin(PrimitiveType.LineLoop);
            GL.Vertex3(-1.0f, -1.0f, -1.0f);
            GL.Vertex3(1.0f, -1.0f, -1.0f);
            GL.Vertex3(1.0f, -1.0f, 1.0f);
            GL.Vertex3(-1.0f, -1.0f, 1.0f);
            GL.End();

            GL.Begin(PrimitiveType.LineLoop);
            GL.Vertex3(-1.0f, -1.0f, -1.0f);
            GL.Vertex3(-1.0f, -1.0f, 1.0f);
            GL.Vertex3(-1.0f, 1.0f, 1.0f);
            GL.Vertex3(-1.0f, 1.0f, -1.0f);
            GL.End();


            /*----------------------------------------*/

            GL.Begin(PrimitiveType.Quads);
            GL.Color3(Color.Gray);


            //FUNDO
            p1 = new Vector3d(-1.0f, -1.0f, -1.0f);
            p2 = new Vector3d(-1.0f, 1.0f, -1.0f);
            p3 = new Vector3d(1.0f, 1.0f, -1.0f);
            v1 = p2 - p1;
            v2 = p3 - p1;
            n1 = v1 * v2;
            n1.Normalize();
            GL.Normal3(0, 0, -1);

            GL.Vertex3(-1, -1.0f, -1.0f);
            GL.Vertex3(-1.0f, 1.0f, -1.0f);
            GL.Vertex3(1.0f, 1.0f, -1.0f);
            GL.Vertex3(1.0f, -1.0f, -1.0f);
            GL.End();

            GL.Begin(PrimitiveType.Quads);
            //BAIXO
            p1 = new Vector3d(-1.0f, -1.0f, -1.0f);
            p2 = new Vector3d(1.0f, -1.0f, -1.0f);
            p3 = new Vector3d(1.0f, -1.0f, 1.0f);
            v1 = p2 - p1;
            v2 = p3 - p1;
            n1 = v1 * v2;
            n1.Normalize();

            GL.Normal3(0, 1, 0);
            GL.Vertex3(-1.0f, -1.0f, -1.0f);
            GL.Vertex3(1.0f, -1.0f, -1.0f);
            GL.Vertex3(1.0f, -1.0f, 1.0f);
            GL.Vertex3(-1.0f, -1.0f, 1.0f);
            GL.End();


            GL.Begin(PrimitiveType.Quads);
            //ESQUERDO
            p1 = new Vector3d(-1.0f, -1.0f, -1.0f);
            p2 = new Vector3d(-1.0f, -1.0f, 1.0f);
            p3 = new Vector3d(-1.0f, 1.0f, 1.0f);
            v1 = p2 - p1;
            v2 = p3 - p1;
            n1 = v1 * v2;
            n1.Normalize();
            GL.Normal3(-1, 0, 0);

            GL.Vertex3(-1.0f, -1.0f, -1.0f);
            GL.Vertex3(-1.0f, -1.0f, 1.0f);
            GL.Vertex3(-1.0f, 1.0f, 1.0f);
            GL.Vertex3(-1.0f, 1.0f, -1.0f);
            GL.End();

            GL.Begin(PrimitiveType.Quads);
            // FRENTE
            p1 = new Vector3d(-1.0f, -1.0f, 1.0f);
            p2 = new Vector3d(1.0f, -1.0f, 1.0f);
            p3 = new Vector3d(1.0f, 1.0f, 1.0f);
            v1 = p2 - p1;
            v2 = p3 - p1;
            n1 = v1 * v2;
            n1.Normalize();
            GL.Normal3(0, 0, 1);


            GL.Vertex3(-1.0f, -1.0f, 1.0f);
            GL.Vertex3(1.0f, -1.0f, 1.0f);
            GL.Vertex3(1.0f, 1.0f, 1.0f);
            GL.Vertex3(-1.0f, 1.0f, 1.0f);
            GL.End();

            GL.Begin(PrimitiveType.Quads);
            // CIMA
            p1 = new Vector3d(-1.0f, 1.0f, -1.0f);
            p2 = new Vector3d(-1.0f, 1.0f, 1.0f);
            p3 = new Vector3d(1.0f, 1.0f, 1.0f);
            v1 = p2 - p1;
            v2 = p3 - p1;
            n1 = v1 * v2;
            n1.Normalize();
            GL.Normal3(0, 1, 0);

            GL.Vertex3(-1.0f, 1.0f, -1.0f);
            GL.Vertex3(-1.0f, 1.0f, 1.0f);
            GL.Vertex3(1.0f, 1.0f, 1.0f);
            GL.Vertex3(1.0f, 1.0f, -1.0f);
            GL.End();

            GL.Begin(PrimitiveType.Quads);
            // DIREITO
            GL.Normal3(1, 0, 0);
            GL.Vertex3(1.0f, -1.0f, -1.0f);
            GL.Vertex3(1.0f, 1.0f, -1.0f);
            GL.Vertex3(1.0f, 1.0f, 1.0f);
            GL.Vertex3(1.0f, -1.0f, 1.0f);

            GL.End();
        }

        private static int sphereList, cubeList;
        double nivel = 0;
        float tq = 500000;
        private void F3D_Shown(object sender, EventArgs e)
        {
         //   Set3DProjection(Controle.Width, Controle.Height);

       //     TOpenGl.ReShapeOGL(Controle.Width, Controle.Height, Perspectiva);

           /* Gl.glViewport(0, 0, Controle.Width, Controle.Height);
            Gl.glMatrixMode(Gl.GL_PROJECTION);
            Gl.glLoadIdentity();
            double left = -(Controle.Width / Controle.Height);
            double rigth = -left;

           // Glu.gluPerspective(45.0, (float)Controle.Width / (float)Controle.Height, 3, 100000.0);

            if (!Perspectiva)
            {
                TOpenGl.AssociateOGL(this.Controle);
                TOpenGl.InitializeOGL(); 
                TOpenGl.ReShapeOGL(Controle.Width, Controle.Height);
            }*/
//
            //Gl.glMatrixMode(Gl.GL_MODELVIEW);
          //  Gl.glLoadIdentity();
            
         //   GerarModelo3D(true);

            if (gerenciador.TresDPavimento)
                WindowState = System.Windows.Forms.FormWindowState.Normal;
        }

        bool ClicouObjeto;
        private void Controle_MouseClick(object sender, System.Windows.Forms.MouseEventArgs e)
        {

        }

        private void button1_Click(object sender, EventArgs e)
        {
            x_trans = double.Parse(textBox1.Text);
            y_trans = double.Parse(textBox2.Text);
            cameraDistance -= double.Parse(textBox3.Text);
            Render();
            Controle.SwapBuffers();
           /* float[] mat = { 0.1745f, 0.01175f, 0.01175f };
            Gl.glMaterialfv(Gl.GL_FRONT, Gl.GL_AMBIENT, mat);
            mat[0] = 0.61424f;
            mat[1] = 0.04136f;
            mat[2] = 0.04136f;
            Gl.glMaterialfv(Gl.GL_FRONT, Gl.GL_DIFFUSE, mat);
            mat[0] = 0.727811f;
            mat[1] = 0.626959f;
            mat[2] = 0.626959f;
            Gl.glMaterialfv(Gl.GL_FRONT, Gl.GL_SPECULAR, mat);
            Gl.glMaterialf(Gl.GL_FRONT, Gl.GL_SHININESS, 0.6f * 128.0f);

              /*SetPerspective(Controle.Width, Controle.Height);
              TOpenGl.InitializeOGL();

              InicializaOGL();

              Gl.glViewport(0, 0, Controle.Width, Controle.Height);
              Gl.glMatrixMode(Gl.GL_PROJECTION);
              Gl.glLoadIdentity();
              double left = -(Controle.Width / Controle.Height);
              double rigth = -left;
       
              GerarModelo3D(true);*/

            // gerenciador.Chama3D(gerenciador.TresDPavimento);
           /* return;
            
            if (z)
            {
                SetPerspective(Controle.Width, Controle.Height);
           //     Gl.glTranslated(pt3D_1.x, pt3D_1.y, pt3D_1.z); 
                Render();
                Controle.SwapBuffers();
                z = false;
            }

            g2d.UnProject((int)edZoom.Value, (int)numericUpDown2.Value, ref posX, ref posY, ref posZ);
            */
            //MessageBox.Show("x: " + posX.ToString() + "   y: " + posY.ToString() + "   z: " + posZ.ToString());

        }
        int cli1xSelecao,cli1ySelecao;
        double x1selecao, y1selecao, z1selecao;
        bool selecionando;
        private void Controle_MouseDown(object sender, System.Windows.Forms.MouseEventArgs e)
        {
            ultx = e.X;
            ulty = e.Y;

            x1selecao = posX3d[0];
            y1selecao = posY3d[0];
            z1selecao = posZ3d[0]; 
            
            if (e.Button == System.Windows.Forms.MouseButtons.Left && !shift)
            {
                cli1xSelecao = e.X;
                cli1ySelecao = e.Y;
                selecionando = true;
                raio.GerarRaio3D(ref mouseX, ref  mouseY, ref zNear, ref zFar, ref  ViewPort, ref ModelViewMatrix, ref  ProjectionMatrix);
                raio.CalculaIntersecao_Raio_x_Plano(ref planoRotacao, ref IntersecRaioPlanoRotacao);

                this.Text = IntersecRaioPlanoRotacao.x.ToString("n2") + " " + IntersecRaioPlanoRotacao.y.ToString("n2");
     
            }
            
            if (e.Button == System.Windows.Forms.MouseButtons.Middle)
            {
                //if (Perspectiva)
                {
                    this.Controle.Cursor = System.Windows.Forms.Cursors.Hand;
                    Panning = true;

                    start_x = e.X;
                    start_y = e.Y;

              //      x_trans = start_x;
               //     y_trans = start_y;

                }
            }

            if (e.Button == System.Windows.Forms.MouseButtons.Right)
            {
                pt3D_1.x = posX;
                pt3D_1.y = posY;
                pt3D_1.z = posZ;

              //  Gl.glTranslated(posX, posY, posZ);
                Render();
                Controle.SwapBuffers();
                z = true;
            }
        }

        bool z;
        double rigth, left;
        public void SetPerspective(int iWidth, int iHeight)
        {
         //   SetOrtho(iWidth, iHeight);
          //  return;

           /* if (iHeight == 0)
                iHeight = 1;

            Gl.glViewport(0, 0, iWidth, iHeight);
            Gl.glMatrixMode(Gl.GL_PROJECTION);
            Gl.glLoadIdentity();
            left = -(iWidth / iHeight);
            rigth = -left;

            Glu.gluPerspective(60, (float)iWidth / (float)iHeight,50, 1000000.0);
           
            Gl.glMatrixMode(Gl.GL_MODELVIEW);
            Gl.glLoadIdentity();*/
        }

        public void SetOrtho(int iWidth, int iHeight)
        {
           /* if (iHeight == 0)
                iHeight = 1;

            Gl.glViewport(0, 0, iWidth, iHeight);
            Gl.glMatrixMode(Gl.GL_PROJECTION);
            Gl.glLoadIdentity();
            left = -(iWidth / iHeight);
            rigth = -left;
            Gl.glOrtho(left, rigth,0, 100, 50, 1000000.0);
           // Gl.glOrtho(left, rigth, -1, 1, -10000, 10000);

            Gl.glMatrixMode(Gl.GL_MODELVIEW);
            Gl.glLoadIdentity();*/
        }
        private void glControl_MouseMove(object sender, System.Windows.Forms.MouseEventArgs e)
        {

        }
        double angle;
    //    Cursor Cur_rotacao = new Cursor(PGi.Properties.Resources.rotate1.Handle);
        double z_trans;
        public void Controle_MouseMove(object sender, System.Windows.Forms.MouseEventArgs e)
        {
     //       if (gerenciador.ActiveMdiChild != this) return;
       /*     int x = e.X, y = e.Y;
            double px, py, pz; 
         //   mouseX = e.X;
         //   mouseY = e.Y;
            
            int dx = x - mouseX;
            int dy = y - mouseY;
            int dz = (int)(posZ - zant);
            
          //  UpdateOGLMatrix(ModelViewMatrix);
        
          //  ogl.pos(&px, &py, &pz, x, y, ViewPort, (int)GLright, (int)GLleft, (int)GLtop, (int)GLbotton, (int)zNear);
            
            angle = 0;

            if (e.Button == System.Windows.Forms.MouseButtons.Middle)
            {
                if (!Perspectiva)
                {
                    GL.MatrixMode(MatrixMode.Modelview);
                    GL.LoadIdentity();

                    GL.Translate((float)px - _dragPosX, (float)py - _dragPosY, (float)pz - _dragPosZ);
                    
                //    x_trans = x_trans + (e.X - start_x) / 1;
           //         y_trans = y_trans - (e.Y - start_y) / 1;

                    GL.MultMatrix(ModelViewMatrix);
                }
                else
                {
                    if (Panning)
                    {
                        x_trans = x_trans + (e.X - start_x)/1;
                        y_trans = y_trans - (e.Y - start_y)/1;

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
                rodando = true;
                if (!Perspectiva)
                {
                    double ax, ay, az;

                    double[] _MatrixInverse = new double[16];
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


                    x_rot_angle += ((e.Y - start_y) * .3);
                    y_rot_angle += ((e.X - start_x) * .3);


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

                ultx = e.X;
                ulty = e.Y;
            }

            start_x = e.X;
            start_y = e.Y; 
            
            mouseX = x;
            mouseY = y;

           // if (!Perspectiva)
           // {
                _dragPosX = (float)px;
                _dragPosY = (float)py;
                _dragPosZ = (float)pz;
         //   }
            TestaSelecao(e.X, e.Y);
            Render();
            Controle.SwapBuffers(); 

            //TOpenGl.OGL.Refresh();

            //   this.Text = e.X.ToString() + "  " + e.Y.ToString();
          /*  zant = posZ;
            realY = ViewPort[3] - (int)e.Y;
           
            winZ = new double[1];
            posX3d = new double[1];
            posY3d = new double[1];
            posZ3d = new double[1];*/
           // GL.ReadPixels(e.X, e.Y, 1, 1, OpenTK.Graphics.OpenGL.PixelFormat.DepthComponent, PixelType.Float, winZ);
          //  g2d.UnProject(e.X, e.Y, ref posX, ref posY, ref posZ);
          // OpenTK.Graphics.Glu.UnProject(e.X, realY, winZ[0], ModelViewMatrix, ProjectionMatrix, ViewPort, posX3d, posY3d, posZ3d);
          //  Project(ref px_x, ref px_y, posX3d[0], posY3d[0], posZ3d[0]);
           // gerenciador.Text = e.X + " - " + e.Y + " x:" + px_x[0].ToString("n5") + "y:" +(ViewPort[3] -  px_y[0]).ToString("n5");
         //   gerenciador.Text = e.X + " - " + e.Y + " x:" + posX3d[0].ToString("n5") + "y:" + posY3d[0].ToString("n5") + "z:" + posZ3d[0].ToString("n5");
        }

        public static void Project(ref double []pixelX, ref double []pixelY, double worldX, double worldY, double worldZ, double []model, double[] proj)
        {
            double []pixelZ = new double[1];
        //    UpdateOGLMatrix();
            OpenTK.Graphics.Glu.Project(worldX, worldY, worldZ, model, proj, ViewPort, pixelX, pixelY, pixelZ);

            pixelY[0] = ViewPort[3] - pixelY[0];
            /* 
               Glu.gluProject(worldX, worldY, worldZ, TOpenGl.ModelViewMatrix, TOpenGl.ProjectionMatrix, TOpenGl.ViewPort, out pixelX, out pixelY, out pixelZ);
               pixelY = TOpenGl.ViewPort[3] - pixelY;*/
        }


        private void F3D_FormClosing(object sender, FormClosingEventArgs e)
        {
       //     gerenciador.formDesenho.WindowState = System.Windows.Forms.FormWindowState.Maximized;
            Dispose();
            gerenciador.F3d = null;
        }
        public bool rodando = false;
        private void Controle_MouseUp(object sender, System.Windows.Forms.MouseEventArgs e)
        {
            if (e.Button == System.Windows.Forms.MouseButtons.Middle)
            {
              //  this.Cursor = System.Windows.Forms.Cursors.Cross;
                Panning = false;
            };
            if (e.Button == System.Windows.Forms.MouseButtons.Left && selecionando && !shift)
            {
                selecionando = false;
            }

            this.Controle.Cursor = System.Windows.Forms.Cursors.Cross;
            
            rodando = false;

            GeraPoligonosSelecao();
        }

        private void numericUpDown1_ValueChanged(object sender, EventArgs e)
        {

        }

        private void opçõesDeVisualizaçãoToolStripMenuItem_Click(object sender, EventArgs e)
        {
          //  bool TelaLiberadaAnterior = gerenciador.ConfiguracoesPGi.F3DOpcoesVisualizacao.FundoDegrade1;

            DialogResult result = DialogResult.Yes;
            {
                FOpcVis3D = new FRotacionarElementos( gerenciador);

                result = FOpcVis3D.ShowDialog(this);
            }
            
            if (result == DialogResult.OK)
            {
              /*  if (gerenciador.ConfiguracoesPGi.F3DOpcoesVisualizacao.FundoDegrade1 != TelaLiberadaAnterior)
                {
                    this.Close();
                    gerenciador.Chama3D(gerenciador.TresDPavimento);
                }*/

                if (gerenciador.ConfiguracoesPGi.F3DOpcoesVisualizacao.MostrarTexturas)
                {
                    CarregaTexturas();
                }

               // Perspectiva = gerenciador.ConfiguracoesPGi.F3DOpcoesVisualizacao.Perspectiva;

               // SetupCamera();
            }
        }

        private void numericUpDown4_ValueChanged(object sender, EventArgs e)
        {
       /*     x_trans = (double)numericUpDown4.Value / 10;
    
            Render();
            Controle.SwapBuffers();

            return;
            SetPerspective(Controle.Width, Controle.Height);


        //    this.Text = x_trans + " - " + y_trans;
            Gl.glTranslated((double)numericUpDown4.Value, (double)numericUpDown1.Value, -cameraDistance);

            Gl.glRotated(x_rot_angle, 1, 0.0, 0.0);
            Gl.glRotated(y_rot_angle, 0.0, 1, 0.0);
            Render();
            Controle.SwapBuffers();

            return;


            float[] lightPosition = { (float)numericUpDown4.Value / 10, (float)numericUpDown1.Value / 10, (float)numericUpDown3.Value / 10, 1 };

            Gl.glLightfv(Gl.GL_LIGHT1, Gl.GL_POSITION, lightPosition);

            Render();
            Controle.SwapBuffers();*/
        }

        private void numericUpDown1_ValueChanged_1(object sender, EventArgs e)
        {
          /*  x_trans = (double)numericUpDown4.Value / 10;            y_trans = (double)numericUpDown1.Value / 10;
            Render();
            Controle.SwapBuffers();

          //  SetPerspective(Controle.Width, Controle.Height);


            //    this.Text = x_trans + " - " + y_trans;
            Gl.glTranslated((double)numericUpDown4.Value, (double)numericUpDown1.Value, -cameraDistance);

            Gl.glRotated(x_rot_angle, 1, 0.0, 0.0);
            Gl.glRotated(y_rot_angle, 0.0, 1, 0.0);
            Render();
            Controle.SwapBuffers();
            return;

            float[] lightPosition = { (float)numericUpDown4.Value / 10, (float)numericUpDown1.Value / 10, (float)numericUpDown3.Value / 10, 1 };

            Gl.glLightfv(Gl.GL_LIGHT1, Gl.GL_POSITION, lightPosition);

            Render();
            Controle.SwapBuffers();*/
        }

        private void numericUpDown3_ValueChanged(object sender, EventArgs e)
        {
            /*float[] lightPosition = { (float)numericUpDown4.Value / 10, (float)numericUpDown1.Value / 10, (float)numericUpDown3.Value / 10, 1 };

            Gl.glLightfv(Gl.GL_LIGHT1, Gl.GL_POSITION, lightPosition);

            Render();
            Controle.SwapBuffers();*/
        }

        private void desbloquearTelaToolStripMenuItem_Click(object sender, EventArgs e)
        {

        }

        private void Controle_Resize(object sender, EventArgs e)
        {
            try
            {
                Perspectiva = gerenciador.ConfiguracoesPGi.F3DOpcoesVisualizacao.Perspectiva;
                SetupCamera();
                    //   SetupCamera();
                    /*
                                     GL.Viewport(0, 0, glControl1.Width, glControl1.Height);

                                     GL.MatrixMode(MatrixMode.Projection);
                                     GL.LoadIdentity();
                                     GL.Ortho(-1.0, 1.0, -1.0, 1.0, 0.0, 1);*/

                //      Set3DProjection(Controle.Width, Controle.Height);
/*
                Controle.InitializeContexts();
                // TOpenGl.ReShapeOGL(Controle.Width, Controle.Height);

                /*
                TOpenGl.AssociateOGL(this.Controle);

                SetPerspective(Controle.Width, Controle.Height);
                Controle.SwapBuffers();
                Render();
                Controle.SwapBuffers();*/
            /*    SetPerspective(Controle.Width, Controle.Height);
                TOpenGl.InitializeOGL();

                InicializaOGL();

                Gl.glViewport(0, 0, Controle.Width, Controle.Height);
                Gl.glMatrixMode(Gl.GL_PROJECTION);
                Gl.glLoadIdentity();

                double left = 0; //prevent division by zero
                if (Controle.Height == 0)
                    left = 0;
                else
                    left = -(Controle.Width / Controle.Height);

                double rigth = -left;

                if (!gerenciador.abrindo3D)
                  GerarModelo3D(true);

                Render();
                // Gl.glClear(Gl.GL_COLOR_BUFFER_BIT | Gl.GL_DEPTH_BUFFER_BIT);
                //  Gl.glClearColor(r, g, b, 0.0f);    // This Will Clear The Background Color To Black
                TOpenGl.OGL.Refresh();*/

              //  MouseEventArgs me = new MouseEventArgs(MouseButtons.None, -1, 0, 0, 0);

              //  Controle_MouseMove(Controle, me);
            }
            catch(Exception)
            {

            }
            //  TOpenGl.InitializeOGL();
            // gerenciador.Chama3D(gerenciador.TresDPavimento);
        }

        private void F3D_FormClosed(object sender, FormClosedEventArgs e)
        {

        }

        private void button2_Click(object sender, EventArgs e)
        {
            MessageBox.Show(PanelPane.Width.ToString());
        }

        private void Controle_DoubleClick(object sender, EventArgs e)
        {
           // GeraPoligonosSelecao();
            TTrechoViga tSelecionado = null;
            ClicouObjeto = false;
            foreach (TrechosXPoligonos p in PoligonosTrechos)
            {
                //  p.trecho.Selecionado = false;
                foreach (TPoligono p1 in p.poligonos)
                {
                    if (p1.PontoEmPoligono(mouseX, mouseY))
                    {
                        tSelecionado = p.trecho;
                        ClicouObjeto = true;
                        //   MessageBox.Show(p.trecho.Dados.numero.ToString());
                    }
                }
            }

            if (ClicouObjeto)
            {
                foreach (TrechosXPoligonos p in PoligonosTrechos)
                  tSelecionado.SetaSelecao(false, false);

                tSelecionado.SetaSelecao(true,true);
            }

            if (ClicouObjeto)
            {
                gerenciador.formDesenho.ChamaEditObjeto();
            }
            else
            {
               // foreach(TPavimento p in Pavimentos)
                  gerenciador.formDesenho.SetaSelecionados(false, -1);
            
            }
        }

        private void glControl_Resize(object sender, EventArgs e)
        {

        }

        public static void UpdateOGLMatrix(double [] model)
        {
            GL.GetDouble(GetPName.ModelviewMatrix, model);
            GL.GetDouble(GetPName.ProjectionMatrix, ProjectionMatrix);
            GL.GetInteger(GetPName.Viewport, ViewPort);
        }

        private void Controle_Resize_1(object sender, EventArgs e)
        {
            try
            {
                Perspectiva = gerenciador.ConfiguracoesPGi.F3DOpcoesVisualizacao.Perspectiva;
                SetupCamera();
                /*
                                 GL.Viewport(0, 0, glControl1.Width, glControl1.Height);

                                 GL.MatrixMode(MatrixMode.Projection);
                                 GL.LoadIdentity();
                                 GL.Ortho(-1.0, 1.0, -1.0, 1.0, 0.0, 1);*/

            }
            catch (Exception m)
            {
                MessageBox.Show(m.Message);
            }
        }

        private void Controle_Load(object sender, EventArgs e)
        {
      //      InicializaOGL();
        }

        TTrechoViga tSelecionado = null;
        void TestaSelecao(int mx, int my)
        {
            if (!rodando)
            {
                tSelecionado = null;
                ClicouObjeto = false;
                foreach (TrechosXPoligonos p in PoligonosTrechos)
                {
                   p.trecho.SetaSelecao(false, false);
                    foreach (TPoligono p1 in p.poligonos)
                    {
                        if (p1.PontoEmPoligono(mx, my))
                        {
                            tSelecionado = p.trecho;
                            ClicouObjeto = true;
                            //   MessageBox.Show(p.trecho.Dados.numero.ToString());
                        }
                    }
                }

                if (ClicouObjeto)
                {
                    foreach (TrechosXPoligonos p in PoligonosTrechos)
                        tSelecionado.SetaSelecao(false, false);

                    foreach (TPavimento p in Pavimentos)
                        gerenciador.formDesenho.SetaSelecionados(false, -1);

                    tSelecionado.SetaSelecao(true, true);
                }
            }
        }
        public void VistaCima()
        {
            GL.Clear(ClearBufferMask.ColorBufferBit | ClearBufferMask.DepthBufferBit);
            GL.MatrixMode(MatrixMode.Modelview);
            GL.LoadIdentity();
        }

        private void Controle_MouseClick_1(object sender, System.Windows.Forms.MouseEventArgs e)
        {
            start_x = e.X;
            start_y = e.Y;

            //GeraPoligonosSelecao();
            TestaSelecao(e.X, e.Y);

        }

        private void button2_Click_1(object sender, EventArgs e)
        {
            textBox4.Text = x_trans.ToString();
            textBox5.Text = y_trans.ToString();
            textBox6.Text = cameraDistance.ToString();
        }

        private void button3_Click(object sender, EventArgs e)
        {
            x_trans = (double.Parse)(textBox4.Text);
            y_trans = (double.Parse)(textBox5.Text);
        }

        //new OpenTK.Graphics.GraphicsMode(32, 24, 0, 8)
    }
}
