using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using OpenTK;
using OpenTK.Input;
using OpenTK.Graphics.OpenGL;
using OpenTK.Platform;
using System.Drawing.Imaging;
using System.Drawing;

using GeometryUtility;
using PolygonCuttingEar;
using System.Windows.Media.Media3D;
using OpenTK.Platform.Windows;
using Win32Interop.Enums;
using System.Web.Security;

namespace PG
{
    public class Camera3D
    {
        public bool Ortogonal { get; set; }
        // Matrizes em double (fonte matematica)
        public Matrix4d viewMatrix;
        public Matrix4d viewMatrix_temp;
        public Matrix4d Projecao;

        //matriz usada para calcular pivo de rotacao e
        //projecão utilizada para fazer o perfect panning...usa um znear bem  pequeno para melhorar a precisao da distancia do plano fixo do panning
        //pois senao ocorrem pequenos saltos se usar a matiz de projecao oficial com znear de 0.1 m, lembrado que na matriz oficial nao posso
        //usar znear mto pequeno, pois a renderização fica esquisita
        public Matrix4d Projecao_zNear_zero;

        // Matrizes em float (cache para OpenGL)
        public Matrix4 viewMatrix_f;
        public Matrix4 viewMatrix_temp_f;
        public Matrix4 Projecao_f;

        // Estado da câmera
        public double x_trans;
        public double y_trans;
        public double z_trans;
        public double y_rot_angle, x_rot_angle;
        public double x_ang_rad;
        public double z_ang_rad;

        public double pivoX;
        public double pivoY;
        public double pivoZ;

        public bool Dirty = true;
        public double zNear, zFar;
        public bool ViewDirty = true;
        public bool ProjecaoDirty = true;
        double width_tela, height_tela;

        public Camera3D(double w, double h)
        {
            width_tela = w;
            height_tela = h;
        }

        //    Projecao = Matrix4d.CreatePerspectiveFieldOfView((45 * Math.PI/180), this.Width / this.Height, zNear, zFar);
        public void Projecao_Perspectiva(double ang, double _zNear, double _zFar)
        {
            zNear = _zNear;
            zFar  = _zFar;

            Projecao   = Matrix4d.CreatePerspectiveFieldOfView((ang * Math.PI / 180), width_tela / height_tela, zNear, zFar);
            Projecao_f = RMath.ToMatrix4(Projecao);

            Projecao_zNear_zero = Matrix4d.CreatePerspectiveFieldOfView((45 * Math.PI / 180), width_tela / height_tela, 0.00001, 300);

            ProjecaoDirty = true;
            Ortogonal = false;
        }

        public void Projecao_Ortografica(double leftOrtoZoom, double rightOrtoZoom, double bottonOrtoZoom, double topOrtoZoom, double _zNear, double _zFar)
        {
            zNear = _zNear;
            zFar  = _zFar;

            Projecao = Matrix4d.CreateOrthographicOffCenter(leftOrtoZoom, rightOrtoZoom, bottonOrtoZoom, topOrtoZoom, _zNear, _zFar);
            Projecao_f = RMath.ToMatrix4(Projecao);

            Projecao_zNear_zero = Matrix4d.CreatePerspectiveFieldOfView((45 * Math.PI / 180), width_tela / height_tela, 0.00001, 300);

            ProjecaoDirty = true;
            Ortogonal = true;
        }

        void Recalcular()
        {
            if (Ortogonal)
            {
                viewMatrix = Matrix4d.CreateTranslation(-pivoX, -pivoY, -pivoZ) *
                             (Matrix4d.CreateRotationZ(-z_ang_rad) * Matrix4d.CreateRotationX(-x_ang_rad)) *
                              Matrix4d.CreateTranslation(-x_trans, -y_trans, 0);

                viewMatrix_temp = Matrix4d.CreateTranslation(-pivoX, -pivoY, -pivoZ) *
                         (Matrix4d.CreateRotationZ(-z_ang_rad) * Matrix4d.CreateRotationX(-x_ang_rad)) *
                           Matrix4d.CreateTranslation(-x_trans, -y_trans, -200);

                viewMatrix_f      = RMath.ToMatrix4(viewMatrix);
                viewMatrix_temp_f = RMath.ToMatrix4(viewMatrix_temp);
            }
            else
            {
                viewMatrix = Matrix4d.CreateTranslation(-pivoX, -pivoY, -pivoZ) *
                                 ((Matrix4d.CreateRotationZ(-z_ang_rad) * Matrix4d.CreateRotationX(x_ang_rad)) *
                                 Matrix4d.CreateTranslation(x_trans, y_trans, -z_trans));

                viewMatrix_temp = Matrix4d.CreateTranslation(-pivoX, -pivoY, -pivoZ) *
                               ((Matrix4d.CreateRotationZ(-z_ang_rad) * Matrix4d.CreateRotationX(x_ang_rad)) *
                               Matrix4d.CreateTranslation(x_trans, y_trans, -z_trans));

                viewMatrix_f       = RMath.ToMatrix4(viewMatrix);
                viewMatrix_temp_f = RMath.ToMatrix4(viewMatrix_temp);
            }
        }
        public void Atualizar()
        {
            // Recalcula View
            if (ViewDirty)
            {
                Recalcular();
                ViewDirty = false;
            }

            if (ProjecaoDirty)
              ProjecaoDirty = false;
        }
    }

    public class Triangulo
    {
        public vec3 p0, p1, p2, _normal;
        public Plano plano; // plano que contém o triangulo
        public int id, idBarra, idCarga, idApoio, idBarraPortico;
        public string nome;
      //  public TObjetoDesenho objeto;
        public Triangulo()
        {

        }

        public Triangulo(int _id, string _nome)
        {
            p0 = new vec3(0);
            p1 = new vec3(0);
            p2 = new vec3(0);

            _p0 = new vec3(0);
            _p1 = new vec3(0);
            _p2 = new vec3(0);
            v1 = new vec3(0);
            v2 = new vec3(0);
            
            _normal = new vec3(0);
            id = _id;
            plano = new Plano(new vec3(0),new vec3(0),"");
            nome = _nome;
        }
        vec3 v1, v2, _p0, _p1, _p2;
        public void CriaNormal()
        {
            v1 = new vec3(0);
            v2 = new vec3(0);

            v1 = p1 - p0;
            v2 = p2 - p0;
            _normal.x = v1.CrossProduct(v2).x;
            _normal.y = v1.CrossProduct(v2).y;
            _normal.z = v1.CrossProduct(v2).z;

            _normal.Normalize();
        }

        public void CriaPlano(vec3 p)
        {
            plano = new Plano(_normal, p, "");
            plano.Normal.x = _normal.x;
            plano.Normal.y = _normal.y;
            plano.Normal.z = _normal.z;

            plano.Posicao.x = p.x;
            plano.Posicao.y = p.y;
            plano.Posicao.z = p.z;
        }

        public void Aplica(double x0, double y0,
            double x1, double y1,
            double x2, double y2,
            double coordFixa, string plano)
        {
            if (plano == "xy") // fixa em z
            {
                p0.x = x0;
                p0.y = y0;
                p1.x = x1;
                p1.y = y1;
                p2.x = x2;
                p2.y = y2;

                p0.z = coordFixa;
                p1.z = coordFixa;
                p2.z = coordFixa;
            }
            if (plano == "xz") // fixa em y
            {
                p0.x = x0;
                p0.z = y1;
                p1.x = x1;
                p1.z = y1;
                p2.x = x2;
                p2.z = y2;

                p0.y = coordFixa;
                p1.y = coordFixa;
                p2.y = coordFixa;
            }
            if (plano == "yz") // fixa em x
            {
                p0.z = y0;
                p0.y = x0;

                p1.z = y1;
                p1.y = x1;

                p2.z = y2;
                p2.y = x2;

                p0.x = coordFixa;
                p1.x = coordFixa;
                p2.x = coordFixa;
            }
        }
    }
    struct TrianguloSelecao
    {
        public string nome;
        public double z;
        public TrianguloSelecao(string _nome, double _z)
        {
            this.nome = _nome;
            this.z = _z;
        }
    }
    public class CuboRotacao
    {
        public int w, h;
        public double[] Mvm_Cubo;
        public static double[] ProjectionMatrix = new double[16];
        public static int[] _ViewPort = new int[4];

        double tamCubo = 0;
        vec3 p1, p2, p3, n1, v1, v2;

        Raio raio = new Raio(0, 0);
        vec3 IntersecRaioPlano = new vec3(0, 0, 0);

     /*  void CriaNormal(vec3 _p1, vec3 _p2, vec3 _p3)
        {
            _p1 = new vec3(-1.0f, -1.0f, 1.0f);
            _p2 = new vec3(1.0f, -1.0f, 1.0f);
            _p3 = new vec3(1.0f, 1.0f, 1.0f);
            v1 = _p2 - p1;
            v2 = _p3 - p1;
            n1 = v1 * v2;
            n1.Normalize();
        }*/

        [NonSerializedAttribute]
        public CPolygonShape top_cubo, top_canto1, left_cubo, right_cubo, botton_cubo, front_cubo, back_cubo;
        List<Triangulo> TriangulosCubo;

        void TriangularizaFaceCubo(ref CPolygonShape shape, string nome, double coordFixa, string plano, ref int cont)
        {
            shape = new CPolygonShape(vertices);
            shape.CutEar();
   
            for (int i = 0; i < shape.NumberOfPolygons; i++)
            {
                cont++;
                TriangulosCubo.Add(new Triangulo(i, nome));
                TriangulosCubo[cont].Aplica(shape.Polygons(i)[0].X, shape.Polygons(i)[0].Y,
                                         shape.Polygons(i)[1].X, shape.Polygons(i)[1].Y,
                                         shape.Polygons(i)[2].X, shape.Polygons(i)[2].Y,
                                         coordFixa,
                                         plano);

                TriangulosCubo[cont].CriaNormal();
                TriangulosCubo[cont].CriaPlano(TriangulosCubo[cont].p0);
            }

        }

        CPoint2D[] vertices;
        public void TriangularizarLados()
        {
            int ii;
            TriangulosCubo = new List<Triangulo>();
            
            int iTriangulo = -1;
            double tam = 15;
            /*TOP*/
            ii = 0;
            vertices = new CPoint2D[4];
            vertices[ii++] = new CPoint2D(-tam, -tam);
            vertices[ii++] = new CPoint2D(tam, -tam);
            vertices[ii++] = new CPoint2D(tam, tam);
            vertices[ii++] = new CPoint2D(-tam, tam);

            TriangularizaFaceCubo(ref top_cubo, "top", -26, "xy", ref iTriangulo);


            /*LEFT*/
            ii = 0;
            vertices = new CPoint2D[4];
            vertices[ii++] = new CPoint2D(-tam, -tam);
            vertices[ii++] = new CPoint2D(-tam, tam);
            vertices[ii++] = new CPoint2D(tam, tam);
            vertices[ii++] = new CPoint2D(tam, -tam);
            TriangularizaFaceCubo(ref left_cubo, "left", 26, "yz", ref iTriangulo);

            /*RIGHT*/
            ii = 0;
            vertices = new CPoint2D[4];
            vertices[ii++] = new CPoint2D(-tam, -tam);
            vertices[ii++] = new CPoint2D(-tam, tam);
            vertices[ii++] = new CPoint2D(tam, tam);
            vertices[ii++] = new CPoint2D(tam, -tam);
            TriangularizaFaceCubo(ref right_cubo, "right", -26, "yz", ref iTriangulo);

            /*- BOTTON -*/
            ii = 0;
            vertices = new CPoint2D[4];
            vertices[ii++] = new CPoint2D(-tam, -tam);
            vertices[ii++] = new CPoint2D(-tam, tam);
            vertices[ii++] = new CPoint2D(tam, tam);
            vertices[ii++] = new CPoint2D(tam, -tam);
            TriangularizaFaceCubo(ref botton_cubo, "botton", 26, "xy", ref iTriangulo);

            ii = 0;
            vertices = new CPoint2D[4];
            vertices[ii++] = new CPoint2D(-tam, -tam);
            vertices[ii++] = new CPoint2D(-tam, tam);
            vertices[ii++] = new CPoint2D(tam, tam);
            vertices[ii++] = new CPoint2D(tam, -tam);
            TriangularizaFaceCubo(ref front_cubo, "front", 26, "xz", ref iTriangulo);

            ii = 0;
            vertices = new CPoint2D[4];
            vertices[ii++] = new CPoint2D(-tam, -tam);
            vertices[ii++] = new CPoint2D(-tam, tam);
            vertices[ii++] = new CPoint2D(tam, tam);
            vertices[ii++] = new CPoint2D(tam, -tam);
            TriangularizaFaceCubo(ref back_cubo, "back", -26, "xz", ref iTriangulo);
         }
        
        void CriaTriangulos()
        {
           /* vec3 normalTopo = new vec3(0);
            p1 = new vec3(-15, -15, 15);
            p2 = new vec3(15, -15, 15);
            p3 = new vec3(15, 15, 15);

            v1 = p2 - p1;
            v2 = p3 - p1;
            normalTopo = v1.CrossProduct(v2);
            normalTopo.Normalize();*/
            
            TriangularizarLados();

           /* t1_topo.p0.x = p1.x;
            t1_topo.p0.y = p1.y;
            t1_topo.p0.z = tamCubo+1;

            t1_topo.p1.x = p2.x;
            t1_topo.p1.y = p2.y;
            t1_topo.p1.z = tamCubo+1;

            t1_topo.p2.x = p3.x;
            t1_topo.p2.y = p3.y;
            t1_topo.p2.z = tamCubo+1;
            t1

            t1_topo.plano = new Plano(normalTopo, t1_topo.p0, -1);*/

          /*  vec3 normalEsquerda = new vec3(0);
            p1 = new vec3(-15, -15, -15);
            p2 = new vec3(-15, -15, 15);
            p3 = new vec3(-15, 15, 15);
            v1 = p2 - p1;
            v2 = p3 - p1;
            normalEsquerda = v1.CrossProduct(v2);
            normalEsquerda.Normalize();

            t1_esquerdo.p0.x = tamCubo + 1;
            t1_esquerdo.p0.y = p1.y;
            t1_esquerdo.p0.z = p1.z;

            t1_esquerdo.p1.x = tamCubo + 1;
            t1_esquerdo.p1.y = p2.y;
            t1_esquerdo.p1.z = p2.z;

            t1_esquerdo.p2.x = tamCubo + 1;
            t1_esquerdo.p2.y = p3.y;
            t1_esquerdo.p2.z = p3.z;
            t1_esquerdo.plano = new Plano(normalEsquerda, t1_esquerdo.p0, -1);*/

         /*   p1 = new vec3(linhas_facecima.pIni.x, linhas_facecima.pIni.y, Pavimento.Nivel);
            p2 = new vec3(linhas_facecima.pFin.x, linhas_facecima.pFin.y, Pavimento.Nivel);
            p3 = new vec3(linhas_facecima.pFin.x, linhas_facecima.pFin.y, Pavimento.Nivel - Dados.h1);
            v1 = p2 - p1;
            v2 = p3 - p1;
            n1 = v1.CrossProduct(v2);
            n1.Normalize();
            GL.Begin(PrimitiveType.Polygon);
            GL.Normal3(-n1.x, -n1.y, -n1.z);*/
        }

        public CuboRotacao( int w, int h)
        {
            this.w = w;
            this.h = h;
            tamCubo = 25;
            Mvm_Cubo = new double[16];
            pfin_x  = new vec3(50, tamCubo + 4, tamCubo + 2);
            //   t1_topo = new Triangulo(0); 
            //// t1_esquerdo = new Triangulo(0);

            CriaTriangulos();
            CarregaTextura();
        }

        public void CarregaTextura()
        {
            try
            {
                //Bitmap bitmap = new Bitmap("Data/Textures/logo.jpg");
                Bitmap bitmap = new Bitmap(System.IO.Path.GetDirectoryName(System.Windows.Forms.Application.ExecutablePath) + "\\cima.bmp");
                if (!CameraOrto)
                    bitmap = new Bitmap(System.IO.Path.GetDirectoryName(System.Windows.Forms.Application.ExecutablePath) + "\\cima2.bmp"); //System.IO.Path.GetDirectoryName(Application.ExecutablePath) + "\\perfil_w_laminados.txt";
                GL.GenTextures(1, out textureCima);
                GL.BindTexture(TextureTarget.Texture2D, textureCima);
                GL.TexParameter(TextureTarget.Texture2D, TextureParameterName.TextureMinFilter, (int)TextureMinFilter.Linear);
                GL.TexParameter(TextureTarget.Texture2D, TextureParameterName.TextureMagFilter, (int)TextureMagFilter.Linear);

                BitmapData data = bitmap.LockBits(new System.Drawing.Rectangle(0, 0, bitmap.Width, bitmap.Height),
                    ImageLockMode.ReadOnly, System.Drawing.Imaging.PixelFormat.Format32bppArgb);

                GL.TexImage2D(TextureTarget.Texture2D, 0, PixelInternalFormat.Rgba, data.Width, data.Height, 0,
                    OpenTK.Graphics.OpenGL.PixelFormat.Bgra, PixelType.UnsignedByte, data.Scan0);

                bitmap.UnlockBits(data);
                bitmap = new Bitmap(System.IO.Path.GetDirectoryName(System.Windows.Forms.Application.ExecutablePath) + "\\frente.bmp");
                if (!CameraOrto)
                    bitmap = new Bitmap(System.IO.Path.GetDirectoryName(System.Windows.Forms.Application.ExecutablePath) + "\\frente2.bmp");

                GL.GenTextures(1, out textureFrente);
                GL.BindTexture(TextureTarget.Texture2D, textureFrente);
                GL.TexParameter(TextureTarget.Texture2D, TextureParameterName.TextureMinFilter, (int)TextureMinFilter.Linear);
                GL.TexParameter(TextureTarget.Texture2D, TextureParameterName.TextureMagFilter, (int)TextureMagFilter.Linear);

                data = bitmap.LockBits(new System.Drawing.Rectangle(0, 0, bitmap.Width, bitmap.Height),
                    ImageLockMode.ReadOnly, System.Drawing.Imaging.PixelFormat.Format32bppArgb);

                GL.TexImage2D(TextureTarget.Texture2D, 0, PixelInternalFormat.Rgba, data.Width, data.Height, 0,
                    OpenTK.Graphics.OpenGL.PixelFormat.Bgra, PixelType.UnsignedByte, data.Scan0);
                bitmap.UnlockBits(data);

                bitmap = new Bitmap(System.IO.Path.GetDirectoryName(System.Windows.Forms.Application.ExecutablePath) + "\\esquerda.bmp");
                if (!CameraOrto)
                    bitmap = new Bitmap(System.IO.Path.GetDirectoryName(System.Windows.Forms.Application.ExecutablePath) + "\\esquerda2.bmp");
                GL.GenTextures(1, out textureEsquerda);
                GL.BindTexture(TextureTarget.Texture2D, textureEsquerda);
                GL.TexParameter(TextureTarget.Texture2D, TextureParameterName.TextureMinFilter, (int)TextureMinFilter.Linear);
                GL.TexParameter(TextureTarget.Texture2D, TextureParameterName.TextureMagFilter, (int)TextureMagFilter.Linear);

                data = bitmap.LockBits(new System.Drawing.Rectangle(0, 0, bitmap.Width, bitmap.Height),
                    ImageLockMode.ReadOnly, System.Drawing.Imaging.PixelFormat.Format32bppArgb);

                GL.TexImage2D(TextureTarget.Texture2D, 0, PixelInternalFormat.Rgba, data.Width, data.Height, 0,
                    OpenTK.Graphics.OpenGL.PixelFormat.Bgra, PixelType.UnsignedByte, data.Scan0);

                bitmap.UnlockBits(data);

                bitmap = new Bitmap(System.IO.Path.GetDirectoryName(System.Windows.Forms.Application.ExecutablePath) + "\\direita.bmp");
                if (!CameraOrto)
                    bitmap = new Bitmap(System.IO.Path.GetDirectoryName(System.Windows.Forms.Application.ExecutablePath) + "\\direita2.bmp");

                GL.GenTextures(1, out textureDireita);
                GL.BindTexture(TextureTarget.Texture2D, textureDireita);
                GL.TexParameter(TextureTarget.Texture2D, TextureParameterName.TextureMinFilter, (int)TextureMinFilter.Linear);
                GL.TexParameter(TextureTarget.Texture2D, TextureParameterName.TextureMagFilter, (int)TextureMagFilter.Linear);

                data = bitmap.LockBits(new System.Drawing.Rectangle(0, 0, bitmap.Width, bitmap.Height),
                    ImageLockMode.ReadOnly, System.Drawing.Imaging.PixelFormat.Format32bppArgb);

                GL.TexImage2D(TextureTarget.Texture2D, 0, PixelInternalFormat.Rgba, data.Width, data.Height, 0,
                    OpenTK.Graphics.OpenGL.PixelFormat.Bgra, PixelType.UnsignedByte, data.Scan0);

                bitmap.UnlockBits(data);

                bitmap = new Bitmap(System.IO.Path.GetDirectoryName(System.Windows.Forms.Application.ExecutablePath) + "\\tras.bmp");
                if (!CameraOrto)
                    bitmap = new Bitmap(System.IO.Path.GetDirectoryName(System.Windows.Forms.Application.ExecutablePath) + "\\tras2.bmp");

                GL.GenTextures(1, out textureTras);
                GL.BindTexture(TextureTarget.Texture2D, textureTras);
                GL.TexParameter(TextureTarget.Texture2D, TextureParameterName.TextureMinFilter, (int)TextureMinFilter.Linear);
                GL.TexParameter(TextureTarget.Texture2D, TextureParameterName.TextureMagFilter, (int)TextureMagFilter.Linear);

                data = bitmap.LockBits(new System.Drawing.Rectangle(0, 0, bitmap.Width, bitmap.Height),
                    ImageLockMode.ReadOnly, System.Drawing.Imaging.PixelFormat.Format32bppArgb);

                GL.TexImage2D(TextureTarget.Texture2D, 0, PixelInternalFormat.Rgba, data.Width, data.Height, 0,
                    OpenTK.Graphics.OpenGL.PixelFormat.Bgra, PixelType.UnsignedByte, data.Scan0);

                bitmap.UnlockBits(data);

                bitmap = new Bitmap(System.IO.Path.GetDirectoryName(System.Windows.Forms.Application.ExecutablePath) + "\\baixo.bmp");
                GL.GenTextures(1, out textureBaixo);
                GL.BindTexture(TextureTarget.Texture2D, textureBaixo);
                GL.TexParameter(TextureTarget.Texture2D, TextureParameterName.TextureMinFilter, (int)TextureMinFilter.Linear);
                GL.TexParameter(TextureTarget.Texture2D, TextureParameterName.TextureMagFilter, (int)TextureMagFilter.Linear);

                data = bitmap.LockBits(new System.Drawing.Rectangle(0, 0, bitmap.Width, bitmap.Height),
                    ImageLockMode.ReadOnly, System.Drawing.Imaging.PixelFormat.Format32bppArgb);

                GL.TexImage2D(TextureTarget.Texture2D, 0, PixelInternalFormat.Rgba, data.Width, data.Height, 0,
                    OpenTK.Graphics.OpenGL.PixelFormat.Bgra, PixelType.UnsignedByte, data.Scan0);

                bitmap.UnlockBits(data);
            }
            catch(Exception ee)
            {
                System.Windows.Forms.MessageBox.Show("Erro ao carregar textura do cubo: " + ee.Message);
            }
        }
        void DesenhaTriangulosCubo(string nome)
        {
            GL.Color4(Color.FromArgb(180, Color.Gray));
            GL.Begin(PrimitiveType.TriangleFan);
            foreach (Triangulo t in TriangulosCubo)
            {
                if (t.nome == nome)
                {
                    GL.Vertex3(t.p0.x, t.p0.y, t.p0.z);
                    GL.Vertex3(t.p1.x, t.p1.y, t.p1.z);
                    GL.Vertex3(t.p2.x, t.p2.y, t.p2.z);
                }
            }
            GL.End();
        }

        int mousex_, iIntersec, iii;
        int textureCima, textureFrente, textureEsquerda, textureDireita, textureTras, textureBaixo;
        double zNear = -70;
        double difAnt;
        double zFar = 10000;
        Matrix4 persp;
        public string FaceClique;
        public bool CameraOrto;
        Triangulo trianguloRef;
        TrianguloSelecao[] IntersecoesRaioPlano = new TrianguloSelecao[30];

        public vec3 pfin_x, pfin_y, pfin_z;
        double Z_Clip = 0;

        public void Project(ref double[] pixelX, ref double[] pixelY, ref double worldX, ref double worldY, ref double worldZ, ref double[] model, ref double[] proj, ref int[] viewport, ref double Z)
        {
            OpenTK.Graphics.Glu.Project(worldX, worldY, worldZ, model, proj, viewport, pixelX, pixelY, pixelZ);

            pixelY[0] = viewport[3] - pixelY[0];
            Z = pixelZ[0];
        }



        double[] pixelZ = new double[1];
        public double[] px_x1 = new double[1];
        public double[] px_y1 = new double[1];
        public double[] py_x1 = new double[1];
        public double[] py_y1 = new double[1];
        public double[] pz_x1 = new double[1];
        public double[] pz_y1 = new double[1];
        public void Desenha(float x_rot_angle, float y_rot_angle, ref int mouseX, ref int mouseY)
        {
            GL.PushMatrix();
          //  DesenhaEixosPrincipais(CameraOrto);

            GL.Viewport(w - 100, h - 100, 100, 100);
            GL.MatrixMode(MatrixMode.Projection);
            GL.LoadIdentity();
            GL.Ortho(75, -75, 75, -75, zNear, zFar);
           
            GL.Translate(0, 0, 0);

            GL.Rotate(-x_rot_angle, 1, 0, 0);
            GL.Rotate(-y_rot_angle, 0, 0, 1);

            GL.GetDouble(GetPName.ModelviewMatrix, Mvm_Cubo);
            GL.GetDouble(GetPName.ProjectionMatrix, ProjectionMatrix);
            GL.GetInteger(GetPName.Viewport, _ViewPort);

            GL.Disable(EnableCap.Lighting);

            GL.LineWidth(2);

            pfin_x = new vec3(48, tamCubo + 4, tamCubo + 2);
            pfin_y = new vec3(-tamCubo - 4, -48, tamCubo + 2);
            pfin_z = new vec3(-tamCubo, tamCubo + 4, -48);
            
            if (CameraOrto)
            {
                //x
             GL.Color3(Color.Red);
             GL.Begin(PrimitiveType.Lines);
             GL.Vertex3(-tamCubo, tamCubo+4, tamCubo+2);
             GL.Vertex3(35, tamCubo + 4, tamCubo + 2);
             GL.End();

                //y
             GL.Color3(Color.Green);
             GL.Begin(PrimitiveType.Lines);
             GL.Vertex3(-tamCubo-4, tamCubo+2, tamCubo + 2);
             GL.Vertex3(-tamCubo-4, -35, tamCubo + 2);
             GL.End();

                //z
             GL.Color3(Color.Blue);
             GL.Begin(PrimitiveType.Lines);
             GL.Vertex3(-tamCubo, tamCubo + 4, tamCubo + 2);
             GL.Vertex3(-tamCubo, tamCubo + 4, -35);
             GL.End();
          }
          else
          {
                pfin_x *= -1;
                pfin_y *= -1;
                pfin_z *= -1;

                pfin_x.z *= -1;
                pfin_y.z *= -1;
                pfin_z.z *= -1;

                //x
            //    pfin_x = new vec3(-48, (tamCubo + 4) * -1, tamCubo + 2);
              
                GL.Color3(Color.Red);
                GL.Begin(PrimitiveType.Lines);
                GL.Vertex3(tamCubo, -(tamCubo + 4), tamCubo + 2);
                GL.Vertex3(-35, -(tamCubo + 4), tamCubo + 2);
                GL.End();


               // pfin_y = new vec3(tamCubo + 4, 48, tamCubo + 2);
                //y
                GL.Color3(Color.Green);
                GL.Begin(PrimitiveType.Lines);
                GL.Vertex3(tamCubo + 4, -tamCubo, tamCubo + 2);
                GL.Vertex3(tamCubo + 4, 35, tamCubo + 2);
                GL.End();

                //z
              //  pfin_z = new vec3(-tamCubo, tamCubo + 4, -48);
                GL.Color3(Color.Blue);
                GL.Begin(PrimitiveType.Lines);
                GL.Vertex3(tamCubo, -(tamCubo + 4), tamCubo + 2);
                GL.Vertex3(tamCubo, -(tamCubo + 4), -35);
                GL.End();
            }

          Project(ref px_x1, ref px_y1, ref pfin_x.x, ref pfin_x.y, ref pfin_x.z, ref Mvm_Cubo, ref ProjectionMatrix, ref _ViewPort, ref Z_Clip);
          Project(ref py_x1, ref py_y1, ref pfin_y.x, ref pfin_y.y, ref pfin_y.z, ref Mvm_Cubo, ref ProjectionMatrix, ref _ViewPort, ref Z_Clip);
          Project(ref pz_x1, ref pz_y1, ref pfin_z.x, ref pfin_z.y, ref pfin_z.z, ref Mvm_Cubo, ref ProjectionMatrix, ref _ViewPort, ref Z_Clip);

            #region desenha cubo

            GL.Enable(EnableCap.Texture2D);
            GL.LineWidth(2);
            GL.Begin(PrimitiveType.LineLoop);
            GL.Color3(Color.Orange);

        //    if (CameraOrto)
       //     {
                GL.Vertex3(-tamCubo, -tamCubo, -tamCubo);
                GL.Vertex3(-tamCubo, tamCubo, -tamCubo);
                GL.Vertex3(tamCubo, tamCubo, -tamCubo);
                GL.Vertex3(tamCubo, -tamCubo, -tamCubo);
                GL.End();

                GL.Begin(PrimitiveType.LineLoop);
                GL.Vertex3(-tamCubo, -tamCubo, -tamCubo);
                GL.Vertex3(tamCubo, -tamCubo, -tamCubo);
                GL.Vertex3(tamCubo, -tamCubo, tamCubo);
                GL.Vertex3(-tamCubo, -tamCubo, tamCubo);
                GL.End();

                GL.Begin(PrimitiveType.LineLoop);
                GL.Vertex3(-tamCubo, -tamCubo, -tamCubo);
                GL.Vertex3(-tamCubo, -tamCubo, tamCubo);
                GL.Vertex3(-tamCubo, tamCubo, tamCubo);
                GL.Vertex3(-tamCubo, tamCubo, -tamCubo);
                GL.End();

                GL.Begin(PrimitiveType.LineLoop);
                GL.Vertex3(-tamCubo, -tamCubo, tamCubo);
                GL.Vertex3(tamCubo, -tamCubo, tamCubo);
                GL.Vertex3(tamCubo, tamCubo, tamCubo);
                GL.Vertex3(-tamCubo, tamCubo, tamCubo);
                GL.End();

                GL.Begin(PrimitiveType.LineLoop);
                GL.Vertex3(-tamCubo, tamCubo, -tamCubo);
                GL.Vertex3(-tamCubo, tamCubo, tamCubo);
                GL.Vertex3(tamCubo, tamCubo, tamCubo);
                GL.Vertex3(tamCubo, tamCubo, -tamCubo);
                GL.End();

                GL.Begin(PrimitiveType.LineLoop);
                GL.Vertex3(tamCubo, -tamCubo, -tamCubo);
                GL.Vertex3(tamCubo, tamCubo, -tamCubo);
                GL.Vertex3(tamCubo, tamCubo, tamCubo);
                GL.Vertex3(tamCubo, -tamCubo, tamCubo);
                GL.End();
                /*----------------------------------------*/

                GL.Color3(Color.White);

                GL.BindTexture(TextureTarget.Texture2D, textureCima);
                GL.Begin(PrimitiveType.Quads);
                GL.TexCoord2(1, 0); GL.Vertex3(-tamCubo, -tamCubo, -tamCubo);
                GL.TexCoord2(0, 0); GL.Vertex3(-tamCubo, tamCubo, -tamCubo);
                GL.TexCoord2(0, 1); GL.Vertex3(tamCubo, tamCubo, -tamCubo);
                GL.TexCoord2(1, 1); GL.Vertex3(tamCubo, -tamCubo, -tamCubo);
                GL.End();

                GL.BindTexture(TextureTarget.Texture2D, textureTras);
                GL.Begin(PrimitiveType.Quads);
                GL.TexCoord2(1, 0); GL.Vertex3(-tamCubo, -tamCubo, -tamCubo);
                GL.TexCoord2(0, 0); GL.Vertex3(tamCubo, -tamCubo, -tamCubo);
                GL.TexCoord2(0, 1); GL.Vertex3(tamCubo, -tamCubo, tamCubo);
                GL.TexCoord2(1, 1); GL.Vertex3(-tamCubo, -tamCubo, tamCubo);
                GL.End();

                GL.BindTexture(TextureTarget.Texture2D, textureDireita);
                GL.Begin(PrimitiveType.Quads);
                GL.TexCoord2(1, 0); GL.Vertex3(-tamCubo, -tamCubo, -tamCubo);
                GL.TexCoord2(0, 0); GL.Vertex3(-tamCubo, -tamCubo, tamCubo);
                GL.TexCoord2(0, 1); GL.Vertex3(-tamCubo, tamCubo, tamCubo);
                GL.TexCoord2(1, 1); GL.Vertex3(-tamCubo, tamCubo, -tamCubo);
                GL.End();

                GL.BindTexture(TextureTarget.Texture2D, textureBaixo);
                GL.Begin(PrimitiveType.Quads);
                GL.TexCoord2(1, 0); GL.Vertex3(-tamCubo, -tamCubo, tamCubo);
                GL.TexCoord2(0, 0); GL.Vertex3(tamCubo, -tamCubo, tamCubo);
                GL.TexCoord2(0, 1); GL.Vertex3(tamCubo, tamCubo, tamCubo);
                GL.TexCoord2(1, 1); GL.Vertex3(-tamCubo, tamCubo, tamCubo);
                GL.End();

                //  GL.Color4(Color.DarkGray);
                GL.BindTexture(TextureTarget.Texture2D, textureFrente);
                GL.Begin(PrimitiveType.Quads);
                GL.TexCoord2(1, 0); GL.Vertex3(-tamCubo, tamCubo, -tamCubo);
                GL.TexCoord2(0, 0); GL.Vertex3(-tamCubo, tamCubo, tamCubo);
                GL.TexCoord2(0, 1); GL.Vertex3(tamCubo, tamCubo, tamCubo);
                GL.TexCoord2(1, 1); GL.Vertex3(tamCubo, tamCubo, -tamCubo);
                GL.End();

                GL.BindTexture(TextureTarget.Texture2D, textureEsquerda);             
                GL.Begin(PrimitiveType.Quads);
                GL.TexCoord2(1, 0); GL.Vertex3(tamCubo, -tamCubo, -tamCubo);
                GL.TexCoord2(0, 0); GL.Vertex3(tamCubo, tamCubo, -tamCubo);
                GL.TexCoord2(0, 1); GL.Vertex3(tamCubo, tamCubo, tamCubo);
                GL.TexCoord2(1, 1); GL.Vertex3(tamCubo, -tamCubo, tamCubo);

                GL.End();



            GL.LineWidth(1);

            #endregion

            mousex_ = mouseX-(w - 100);
            FaceClique = "";        
            if (mousex_ > 0 && mouseY < 101)
            {
                _ViewPort[0] = 0;
                _ViewPort[1] = 0;
                _ViewPort[2] = 100;
                _ViewPort[3] = 100;

                iIntersec = -1;
                raio.GerarRaio3D(ref mousex_, ref mouseY, ref zNear, ref zFar, ref _ViewPort, ref Mvm_Cubo, ref ProjectionMatrix);
                IntersecoesRaioPlano = new TrianguloSelecao[30];

                foreach (Triangulo t in TriangulosCubo)
                {
                    trianguloRef = t;

                    if (raio.CalculaIntersecao_Raio_x_Triangulo(ref trianguloRef, ref IntersecRaioPlano))
                    {
                        IntersecoesRaioPlano[++iIntersec].z = IntersecRaioPlano.z;
                        IntersecoesRaioPlano[iIntersec].nome = t.nome;
                        FaceClique = t.nome;
                    }
                }

                if (iIntersec > 0)
                {
                    difAnt = Math.Abs((raio.p0.z) - (IntersecoesRaioPlano[0].z));
                    FaceClique = IntersecoesRaioPlano[0].nome;
                    for (iii = 0; iii <= iIntersec; iii++)
                    {
                        if (Math.Abs((raio.p0.z) - (IntersecoesRaioPlano[iii].z)) < Math.Abs(difAnt))
                            FaceClique = IntersecoesRaioPlano[iii].nome;
      
                        difAnt = (raio.p0.z) - (IntersecoesRaioPlano[iii].z);
                    }
                }

                if (FaceClique != "")
                  DesenhaTriangulosCubo(FaceClique);
            }
            GL.Enable(EnableCap.Lighting);


            GL.PopMatrix();

            GL.Viewport(0, 0, w, h);
            GL.MatrixMode(MatrixMode.Projection);
            GL.LoadIdentity();
            GL.Ortho(0, w, h, 0, zNear+50, zFar);

            GL.PushMatrix();
            GL.Color3(Color.Red);
            g2d.drawFilledCircle(px_x1[0]-1, h + px_y1[0] - 101, 8, 11);          
            GL.Translate(px_x1[0]-4, h + px_y1[0] - 109, 0);//1274 
            textoLetra.Print("X", fonteLetra, Color.White);
            GL.PopMatrix();

            GL.PushMatrix(); 
            GL.Color3(Color.DarkGreen);
            g2d.drawFilledCircle(py_x1[0] - 1, h + py_y1[0] - 101, 8, 11);
            GL.Translate(py_x1[0] - 4, h + py_y1[0] - 108, 0);//1274 
            textoLetra.Print("Y", fonteLetra, Color.White);
            GL.PopMatrix();

            GL.PushMatrix();
            GL.Color3(Color.Blue);
            g2d.drawFilledCircle(pz_x1[0] - 1, h + pz_y1[0] - 101, 8, 11);
            GL.Translate(pz_x1[0] - 4, h + pz_y1[0] - 109, 0);//1274 
            textoLetra.Print("Z", fonteLetra, Color.White);
            GL.PopMatrix();

        }
        double ee, idv, rr;
  
        OpenTK.Graphics.TextPrinter textoLetra = new OpenTK.Graphics.TextPrinter(OpenTK.Graphics.TextQuality.Low);
        Font fonteLetra = new Font("Tahoma", 8, FontStyle.Bold);
        public void TestaSelecaoCubo()
        {
            if (FaceClique != "")
            {

            }

        }
    }

    public class Raio
    {
        public vec3 p0, p1;
        
        public int winx, winy;

        public Raio(vec3 P0, vec3 P1)
        {
            p0.x = P0.x;
            p0.y = P0.y;
            p0.z = P0.z;
            p1.x = P1.x;
            p1.y = P1.y;
            p1.z = P1.z;
        }
        double[] winZ0;
        double[] winZ1;
        public Raio(int x2D, int y2D)
        {
            p0 = new vec3(0, 0, 0);
            p1 = new vec3(0, 0, 0);

            winZ1 = new double[1];
            winZ0 = new double[1];
            posX0 = new double[1];
            posY0 = new double[1];
            posZ0 = new double[1];

            posX1 = new double[1];
            posY1 = new double[1];
            posZ1 = new double[1];
            // GerarRaio3D(x2D, y2D);		
        }
        double winY;

        double[] posX0 = new double[1];
        double[] posY0 = new double[1];
        double[] posZ0 = new double[1];

        double[] posX1 = new double[1];
        double[] posY1 = new double[1];
        double[] posZ1 = new double[1];

        public void GerarRaio3D(ref int x2D, ref int y2D, ref double znear, ref double zfar, ref int[] ViewPort, ref double[] model, ref double[] proj)
        {
            winZ0[0] = znear;

            UnProject(x2D, y2D, winZ0, ViewPort, model, proj, posX0, posY0, posZ0);
           // UnProject3(new Vector3d(x2D, y2D,) ,winZ0, ViewPort, model, proj, posX0, posY0, posZ0);
            p0.x = posX0[0];
            p0.z = posZ0[0];
            p0.y = posY0[0];

            winZ1[0] = zfar;

            UnProject(x2D, y2D, winZ1, ViewPort, model, proj, posX0, posY0, posZ0);
            p1.x = posX0[0];
            p1.z = posZ0[0];
            p1.y = posY0[0];
        }
        Vector3d pontoMundo = new Vector3d();
       // Vector3d pontoMundo_d = new Vector3d();
        public void GerarRaio3D(ref int x2D, ref int y2D, ref int[] ViewPort, ref Matrix4d modelview,ref  Matrix4d projection)
        {
//            pontoMundo = UnProject2(new Vector3d(x2D, y2D, -1), projection, modelview, new Size(ViewPort[2], ViewPort[3]));

            pontoMundo = UnProject3(new Vector3d(x2D, y2D, -1), projection, modelview, new Size(ViewPort[2], ViewPort[3]));
            p0.x = pontoMundo.X;
            p0.z = pontoMundo.Z;
            p0.y = pontoMundo.Y;

            // pontoMundo = UnProject2(new Vector3d(x2D, y2D, 1), projection, modelview, new Size(ViewPort[2], ViewPort[3]));
            pontoMundo = UnProject3(new Vector3d(x2D, y2D, 1), projection, modelview, new Size(ViewPort[2], ViewPort[3]));

            p1.x = pontoMundo.X;
            p1.z = pontoMundo.Z;
            p1.y = pontoMundo.Y;
        }
        public void GerarRaio3D(ref int x2D, ref int y2D, ref int[] ViewPort, ref Matrix4 modelview, ref Matrix4 projection)
        {
            Vector3 pMundo = new Vector3();

            pMundo = UnProject2(new Vector3(x2D, y2D, -1), projection, modelview, new Size(ViewPort[2], ViewPort[3]));

            p0.x = pMundo.X;
            p0.z = pMundo.Z;
            p0.y = pMundo.Y;

            pMundo = UnProject2(new Vector3(x2D, y2D, 1), projection, modelview, new Size(ViewPort[2], ViewPort[3]));

            p1.x = pMundo.X;
            p1.z = pMundo.Z;
            p1.y = pMundo.Y;
        }
        /*   public void GerarRaio3D_d(ref int x2D, ref int y2D, ref int[] ViewPort, ref Matrix4d modelview, ref Matrix4d projection)
           {
               pontoMundo_d = UnProject2_d(new Vector3d(x2D, y2D, -1), projection, modelview, new Size(ViewPort[2], ViewPort[3]));

               p0.x = pontoMundo_d.X;
               p0.z = pontoMundo_d.Z;
               p0.y = pontoMundo_d.Y;

               pontoMundo_d = UnProject2_d(new Vector3d(x2D, y2D, 1), projection, modelview, new Size(ViewPort[2], ViewPort[3]));

               p1.x = pontoMundo_d.X;
               p1.z = pontoMundo_d.Z;
               p1.y = pontoMundo_d.Y;
           }*/
        /* public Vector3d UnProject2_d(Vector3d mouse, Matrix4d projection, Matrix4d view, Size viewport)
         {
             Vector4d vec;

             vec.X = 2.0f * mouse.X / (float)viewport.Width - 1;

             vec.Y = -(2.0f * mouse.Y / (float)viewport.Height - 1);
             vec.Z = mouse.Z;
             vec.W = 1.0f;

             /*  Vector3 ray_nds =  new Vector3(vec.X, vec.Y, 1);
               Vector4 ray_clip = new Vector4(vec.X, vec.Y, 1.0f, 1.0f);
               Vector4 ray_eye = Matrix4.Invert(projection) * ray_clip;
               ray_eye.Z = -1.0f;
               ray_eye.W = 0.0f;
               Vector3 ray_wor = (Matrix4.Invert(view) * ray_eye).Xyz;
               ray_wor.Normalize();*/

        /*      Matrix4d viewInv = Matrix4d.Invert(view);
              Matrix4d projInv = Matrix4d.Invert(projection);

              Vector4d.Transform(ref vec, ref projInv, out vec);
              Vector4d.Transform(ref vec, ref viewInv, out vec);

              if (vec.W > 0.000001f || vec.W < -0.000001f)
              {
                  vec.X /= vec.W;
                  vec.Y /= vec.W;
                  vec.Z /= vec.W;
              }
              else
              {
                  return vec.Xyz;
              }

              return vec.Xyz;
          }*/
        public Vector3 UnProject2(Vector3 mouse, Matrix4 projection, Matrix4 view, Size viewport)
        {
            Vector4 vec;

            vec.X = 2.0f * mouse.X / (float)viewport.Width - 1;

            vec.Y = -(2.0f * mouse.Y / (float)viewport.Height - 1);
            vec.Z = mouse.Z;
            vec.W = 1.0f;

            /*  Vector3 ray_nds =  new Vector3(vec.X, vec.Y, 1);
              Vector4 ray_clip = new Vector4(vec.X, vec.Y, 1.0f, 1.0f);
              Vector4 ray_eye = Matrix4.Invert(projection) * ray_clip;
              ray_eye.Z = -1.0f;
              ray_eye.W = 0.0f;
              Vector3 ray_wor = (Matrix4.Invert(view) * ray_eye).Xyz;
              ray_wor.Normalize();*/

            Matrix4 viewInv = Matrix4.Invert(view);
            Matrix4 projInv = Matrix4.Invert(projection);

            Vector4.Transform(ref vec, ref projInv, out vec);
            Vector4.Transform(ref vec, ref viewInv, out vec);

            if (vec.W > 0.000001f || vec.W < -0.000001f)
            {
                vec.X /= vec.W;
                vec.Y /= vec.W;
                vec.Z /= vec.W;
            }
            else
            {
                return vec.Xyz;
            }

            return vec.Xyz;
        }
        public Vector3d UnProject2(Vector3d mouse, Matrix4d projection, Matrix4d view, Size viewport)
        {
            Vector4d vec;

            vec.X = 2.0f * mouse.X / viewport.Width - 1;

            vec.Y = -(2.0f * mouse.Y / viewport.Height - 1);
            vec.Z = mouse.Z;
            vec.W = 1.0f;

          /*  Vector3 ray_nds =  new Vector3(vec.X, vec.Y, 1);
            Vector4 ray_clip = new Vector4(vec.X, vec.Y, 1.0f, 1.0f);
            Vector4 ray_eye = Matrix4.Invert(projection) * ray_clip;
            ray_eye.Z = -1.0f;
            ray_eye.W = 0.0f;
            Vector3 ray_wor = (Matrix4.Invert(view) * ray_eye).Xyz;
            ray_wor.Normalize();*/

            Matrix4d viewInv = Matrix4d.Invert(view);
            Matrix4d projInv = Matrix4d.Invert(projection);

            Vector4d.Transform(ref vec, ref projInv, out vec);
            Vector4d.Transform(ref vec, ref viewInv, out vec);

            if (vec.W > 0.000001f || vec.W < -0.000001f)
            {
                vec.X /= vec.W;
                vec.Y /= vec.W;
                vec.Z /= vec.W;
            }
            else
            {
                return vec.Xyz;
            }

            return vec.Xyz;
        }

        public Vector3d UnProject3(Vector3d mouse,Matrix4d projection,Matrix4d view,Size viewport)
        {
            Vector4d p;

            p.X = 2.0 * mouse.X / viewport.Width - 1.0;
            p.Y = 1.0 - 2.0 * mouse.Y / viewport.Height;
            p.Z = mouse.Z;
            p.W = 1.0;

            Matrix4d inv = Matrix4d.Invert(view * projection);

            Vector4d world = RMath.Multiply(p, inv);//Vector4d.Transform(p, inv);

            world /= world.W;

            return world.Xyz;
        }


        public bool CalculaIntersecao_Raio_x_Triangulo(ref Triangulo triangulo, ref vec3 I)
        {
            CalculaIntersecao_Raio_x_Plano(ref triangulo.plano, ref I);

            return RaioPassaNoTriangulo(ref triangulo, ref I);
        }


        vec3 vp0, vp1, vp2,edge0, edge1, edge2, C, N, v0v2, v0v1;
        public bool RaioPassaNoTriangulo(ref Triangulo triangulo, ref vec3 pI)
        {
            v0v1 = triangulo.p1 - triangulo.p0;
            v0v2 = triangulo.p2 - triangulo.p0; 
            N = v0v1.CrossProduct(v0v2); // N 

            // edge 0
            edge0 = triangulo.p1 - triangulo.p0; //v1 - v0;
            vp0 = pI - triangulo.p0;
            C = edge0.CrossProduct(vp0);
            if (N.DotProduct(C) < 0) return false; // P is on the right side 

            // edge 1
            edge1 = triangulo.p2 - triangulo.p1;
            vp1 = pI - triangulo.p1;
            C = edge1.CrossProduct(vp1);
            if (N.DotProduct(C) < 0) return false; // P is on the right side 

            // edge 2
            edge2 = triangulo.p0 - triangulo.p2;//v0 - v2;
            vp2 = pI - triangulo.p2;
            C = edge2.CrossProduct(vp2);
            if (N.DotProduct(C) < 0) return false; // P is on the right side;

            return true;
        }
        
        public vec3 PontoIntersec = new vec3(0, 0, 0);
        vec3 u, w;
        double _D, _N, _s;
        public int CalculaIntersecao_Raio_x_Plano(ref Plano p, ref vec3 I)
        {
            u = p1 - p0;
            w = p0 - p.Posicao;

            _D = p.Normal.DotProduct(u);
            _N = -p.Normal.DotProduct(w);

            if (Math.Abs(_D) < 0)			// segment is parallel to plane
            {
                if (_N == 0)                     // segment lies in plane
                    return 2;
                else
                    return 0;                   // no intersection
            }

            _s = _N / _D;

            //if (s < 0 || s > 1)
            //	return 0;                       // no intersection

            PontoIntersec = p0 + u * _s;         // compute segment intersect point

            I.x = PontoIntersec.x;
            I.y = PontoIntersec.y;
            I.z = PontoIntersec.z;

            return 1;
        }

        public void UnProject(int mx, int my, double[] _winZ, int[] ViewPort, double[] model, double[] proj, double[] x_, double[] y_, double[] z_)
        {
            GL.ReadBuffer(ReadBufferMode.Back);
            winY = ViewPort[3] - (int)my;
            OpenTK.Graphics.Glu.UnProject(mx, winY, _winZ[0], model, proj, ViewPort, x_, y_, z_);
        }
    }

    public class Plano
    {
        public vec3 Normal,u,v, p1,p2;
        public vec3 Posicao;
        public string plano;
        public double d, a,b,c, l, xmin, ymin, xmax, ymax, zmin, zmax;
        public Plano() { Normal = new vec3(0); }
        public Plano(ref double posicao_x, ref double posicao_y, ref double posicao_z)
        {
          //  Normal = new vec3(normal_x, normal_y, normal_z);
            Posicao = new vec3(posicao_x, posicao_y, posicao_z);

            p1 = new vec3(posicao_x, posicao_y+100, posicao_z);
            p2 = new vec3(posicao_x+100, posicao_y, posicao_z);
            v = p1 - Posicao;
            u = p2 - Posicao;
            Normal = v.CrossProduct(u);
            Normal = Normal.Unitario();

            a = Normal.x;
            b = Normal.y;
            c = Normal.z;
            d = (-Normal.DotProduct(Posicao));
        }

        public Plano(double a, double b, double c, double d_)
        {
            Normal = new vec3(0);
            Normal.x = a;
            Normal.y = b;
            Normal.z = c;

            this.a = a;
            this.b = b;
            this.c = c;

            l =  Normal.getMagnitude();
            Normal.x = a / l;
            Normal.y =  b / l;
            Normal.z =  c / l;

            this.d = d_/l;

           /* Posicao = new vec3(0,0,50);
            Normal.x = 0;
            Normal.y = 0;
            Normal.z = 1;*/
          }
/*Ax + By + Cz + D = 0
Assuming three points p0, p1, and p2 the coefficients A, B, C and D can be computed as follows:

Compute vectors v = p1 – p0, and u = p2 – p0;
Compute n = v x u (cross product)
Normalize n
Assuming n = (xn,yn,zn) is the normalized normal vector then
A = xn
B = yn
C = zn
To compute the value of D we just use the equation above, hence -D = Ax + By + Cz. Replacing (x,y,z) for a point in the plane (for instance p0), we get D = – n . p0 (dot product).*/
        public Plano(ref double Coord_z_Plano)
        {
            a = 0;
            b = 0;
            c = 1;
            d = -Coord_z_Plano;
        //    Posicao = new vec3(0, 0, CoordPlano);
          //  d = (z * p.z)*-1;

            //produto escalar
          //  d = x * Coord_x_Plano + y * Coord_y_Plano + z * Coord_z_Plano;

           // d = -(Normal.DotProduct(Posicao));
        }

        double dd;
        public double Distancia(vec3 P)
        {

            dd = Normal.DotProduct(P) + d;
            return dd;
            /*double sb, sn, sd;
           
            sn = -(Normal.DotProduct(P - Posicao));
            sd = (Normal.DotProduct(Normal));
            sb = sn / sd;
            B = P + sb * Normal;
            sd = (P - B).Magnitude();


            sd = ((P.x - 0) * Normal.x + (P.y - 0) * Normal.y + (P.z - 50) * Normal.z);
            return sd;*/
          /*  sn = -dot(PL.n, (P - PL.V0));
            sd = dot(PL.n, PL.n);
            sb = sn / sd;

            *B = P + sb * PL.n;
            return d(P, *B);*/

        //    return 0;

        }

        public double Distancia(ref double x, ref double y, ref double z)
        {
            return (Normal.x * x + y * Normal.y + z * Normal.z) + d;
        }
        int transparencia;
        Color CorPlano;
        
        public Plano(vec3 norm, vec3 pos, string _plano, bool _PlanoCorte = false)
        {
            Normal = new vec3(0);
            Normal.x = norm.x;
            Normal.y = norm.y;
            Normal.z = norm.z;

            Posicao = new vec3(pos.x, pos.y, pos.z);
            a = Normal.x;
            b = Normal.y;
            c = Normal.z;
            d = (-Normal.DotProduct(Posicao));

            plano = _plano;
            PlanoCorte = _PlanoCorte;
            if (PlanoCorte)
            {
                transparencia = 50;
                CorPlano = Color.LightGray;
            }
            else
            {
                CorPlano = Color.CadetBlue;
                transparencia = 200;
            }
         }

        bool PlanoCorte;

        public void DesenhaNormal()
        {
            GL.Disable(EnableCap.Lighting);
            GL.Enable(EnableCap.LineSmooth);
           // GL.Disable(EnableCap.Texture2D);
            GL.LineWidth(2);
            GL.Begin(PrimitiveType.Lines);
            GL.Color3(Color.Yellow);
            GL.Vertex3(Posicao.x, Posicao.y, Posicao.z);
            GL.Vertex3(Normal.x, Normal.y, Normal.z);
            GL.End();
            GL.Enable(EnableCap.Lighting);
            GL.Disable(EnableCap.LineSmooth);
            GL.LineWidth(1);
            //  GL.Enable(EnableCap.Texture2D);
        }
        public void desenha()
        {
            // GL.Disable(EnableCap.Lighting);
            // GL.Enable(EnableCap.LineSmooth);
            // GL.Disable(EnableCap.Texture2D); ;
            // GL.Enable(EnableCap.ColorMaterial);

            //   float size = 1000;
            // xy plane default
              
                
           float x = (float)Posicao.x;
            float y = (float)Posicao.y;
            float z = (float)Posicao.z;


            // GL.PushMatrix();
            // if (axesAlignment == XYTRANS)
            //  {
            x = (float)Posicao.x;
            y = (float)Posicao.y;
            z = (float)Posicao.z;
            //  }
            /*else if (axesAlignment == ZXTRANS)
            {
                glRotatef(90.0f, 1.0f, 0.0f, 0.0f);
                x = Position->X;
                y = Position->Y;
                z = -Position->Z;
            }
            else if (axesAlignment == YZTRANS)
            {
                glRotatef(-90.0f, 0.0f, 0.0f, 1.0f);
                x = -Position->Z;
                y = Position->X;
                z = Position->Y;
            }*/
            x = (float)Posicao.x;
            y = (float)Posicao.y;
            z = (float)Posicao.z;

            if (plano == "panning")
            {
                 GL.Color4(Color.FromArgb(transparencia, CorPlano));
                 GL.Begin(PrimitiveType.Quads);

                 GL.Vertex3(x-xmin, y-ymin, z);
                 GL.Vertex3(x + xmin, y - ymin, z);
                 GL.Vertex3(x + xmin, y + ymin, z);
                 GL.Vertex3(x - xmin, y + ymin, z);
                 GL.End();
            }
            else
            if (plano == "xy")
            {
                if (!PlanoCorte)
                {
                    GL.Color4(Color.FromArgb(transparencia, CorPlano));
                    GL.Begin(PrimitiveType.Quads);

                    /*   GL.Vertex3(x-xmin,y-ymin,z);
                       GL.Vertex3(x + size, y - size, z);
                       GL.Vertex3(x + size, y + size, z);
                       GL.Vertex3(x - size, y + size, z);
                       GL.End();*/

                    GL.Vertex3(xmin, ymin, z);
                    GL.Vertex3(xmax, ymin, z );
                    GL.Vertex3(xmax, ymax, z );
                    GL.Vertex3(xmin, ymax, z);
                    GL.End();
                }

                GL.Color3(Color.Blue);
                GL.Begin(PrimitiveType.LineLoop);
                GL.Vertex3(xmin, ymin, z);
                GL.Vertex3(xmax, ymin, z);
                GL.Vertex3(xmax, ymax, z);
                GL.Vertex3(xmin, ymax, z);
                GL.End();
            }
            else
            if (plano == "yz")
            {
                if (!PlanoCorte)
                {
                    GL.Color4(Color.FromArgb(transparencia, CorPlano));
                    GL.Begin(PrimitiveType.Quads);
                    GL.Vertex3(x, ymin, -zmin);
                    GL.Vertex3(x, ymax, -zmin);
                    GL.Vertex3(x, ymax, -zmax);
                    GL.Vertex3(x, ymin, -zmax);
                    GL.End();
                }

                GL.Color3(Color.Blue);
                GL.Begin(PrimitiveType.LineLoop);
                GL.Vertex3(x, ymin, -zmin);
                GL.Vertex3(x, ymax, -zmin);
                GL.Vertex3(x, ymax, -zmax);
                GL.Vertex3(x, ymin, -zmax);
                GL.End();
            }
            else
            if (plano == "xz")
            {
                if (!PlanoCorte)
                {
                    GL.Color4(Color.FromArgb(transparencia, CorPlano));
                    GL.Begin(PrimitiveType.Quads);
                    GL.Vertex3(xmin, y, -zmin);
                    GL.Vertex3(xmax, y, -zmin);
                    GL.Vertex3(xmax, y, -zmax);
                    GL.Vertex3(xmin, y, -zmax);
                    GL.End();
                }
                GL.Color3(Color.Blue);
                GL.Begin(PrimitiveType.LineLoop);
                GL.Vertex3(xmin, y, -zmin);
                GL.Vertex3(xmax, y, -zmin);
                GL.Vertex3(xmax, y, -zmax);
                GL.Vertex3(xmin, y, -zmax);
                GL.End();
            }
            //GL.PopMatrix();

            // GL.Enable(EnableCap.Lighting);
            // GL.Disable(EnableCap.LineSmooth);
            // GL.Enable(EnableCap.Texture2D); ;
            // GL.Disable(EnableCap.ColorMaterial);
        }

    }
    public static class ogl
    {
        public static int h = 0, w = 0;
      //  public static SimpleOpenGlControl OGL = null;
        public static double xMin = -2, xMax = 6;
        public static double yMin = -1, yMax = 1;
        public static double zMin = -100, zMax = 1;
        public static double OGLleft = .0, OGLright = .0, OGLtop = .0, OGLbotton = .0;
        public static double OGLzNear = -1, OGLzFar = 1;

        public static double[] ProjectionMatrix   = new double[16];
        public static double[] InvModelViewMatrix = new double[16];
        public static double[] ModelViewMatrix = new double[16];
        public static double[] Mvm_Principal      = new double[16];
        public static double[] Mvm_Zoom           = new double[16];
        public static double[] Mvm_Cubo           = new double[16];
        public static double[] Mvm_Selecao        = new double[16];

        public static int[] ViewPort = new int[4];
        public static int MouseX = 0, MouseY = 0;
        public static double x_rot_angle, y_rot_angle, x_trans, y_trans, z_trans;

public readonly static string vertex_shader = @"
#version 330 core

layout (location = 0) in vec3 aPos;
layout (location = 1) in vec3 aNormal;
layout (location = 2) in vec3 aColor;
layout (location = 3) in vec2 aTexCoords;

out VS_OUT {
    vec3 FragPos;
    vec3 Normal;
    vec3 Color;
    vec2 TexCoords;
} vs_out;

uniform mat4 projection;
uniform mat4 modelview;

void main()
{
    vs_out.FragPos  = vec3(modelview * vec4(aPos, 1.0));

    vs_out.Normal   = transpose(inverse(mat3(modelview))) * aNormal;

    vs_out.Color    = aColor;                 
    vs_out.TexCoords = aTexCoords;
    gl_Position     = projection * modelview* vec4(aPos, 1.0f); //vec4(vPos,1);   

} ";

public readonly static string fragment_shader = @"
#version 330 core

out vec4 FragColor;

in VS_OUT {
    vec3 FragPos;
    vec3 Normal;
    vec3 Color; 
    vec2 TexCoords; 
} fs_in;

//uniform vec3 lightPos;
//uniform vec3 viewPos;
uniform sampler2D Texture;

void main()
{
   // vec3 color = texture(Texture, fs_in.TexCoords).rgb ;
  //  vec3 viewPos = vec3(0,0,0);
    vec3 lightPos = vec3(100,500,1000);
    vec3 color = fs_in.Color.rgb;
    // ambient
    vec3 ambient = 0.5 * color;
    // diffuse
    vec3 lightDir = normalize(lightPos - fs_in.FragPos);
    vec3 normal   = normalize(fs_in.Normal);
    float diff    = max(dot(lightDir,normal), 0.0);
    vec3 diffuse  = diff * color;
    // specular
    vec3 viewDir = normalize(lightPos - fs_in.FragPos);

    float spec = 0.0;

    //blinn
    vec3 halfwayDir = normalize(lightDir + viewDir);  
    spec = pow(max(dot(normal, halfwayDir), 0.0), 1.0);

    vec3 specular = vec3(0.01) * spec; // assuming bright white light color
    FragColor =/*vec4(fs_in.Color, 1);*/ vec4(ambient + diffuse + specular, 1); 
}";


        public readonly static string vertex_shader_sem_iluminacao = @"
            #version 330 core
            layout (location = 0) in vec3 aPos;
            layout (location = 1) in vec3 aColor;
        
            out float alpha; 
            out vec3 Color;

            uniform mat4 projection;
            uniform mat4 modelview;

            void main()
            {
               alpha = 1;
               Color       = aColor;                 
               gl_Position = projection * modelview * vec4(aPos, 1.0f);  
            } ";

        public readonly static string fragment_shader_sem_iluminacao = @"
            #version 330 core
            in vec3 Color; 
            in float alpha;

            out vec4 FragColor;
            void main()
            {
              FragColor  = vec4(Color.rgb, alpha);
            }";

        public readonly static string vertex_shader_com_transparencia = @"
            #version 330 core
            layout (location = 0) in vec3 aPos;
            layout (location = 1) in vec3 aColor;
        
            out float alpha; 
            out vec3 Color;

            uniform mat4 projection;
            uniform mat4 modelview;

            void main()
            {
               alpha = 0.8f;
               Color       = aColor;                 
               gl_Position = projection * modelview * vec4(aPos, 1.0f);  
            } ";

        public readonly static string fragment_shader_com_transparencia = @"
            #version 330 core
            in vec3 Color; 
            in float alpha;

            out vec4 FragColor;
            void main()
            {
              FragColor  = vec4(Color.rgb, alpha);
            }";

        public static void Inicializa(int w, int h, float luz_x = 1, float luz_y = -1, float luz_z = 1)
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
            GL.Enable(EnableCap.Texture2D);
            //  GL.Enable(EnableCap.LineSmooth);
            float[] direcao = { 1f, 0, 0, 0 };
            float[] ambient = { 1f, 1, 1, 1 };
            float[] diffuse = { 0.9f, 0.9f, 0.9f, 1.0f };
            float[] specular = { 1.0f, 1.0f, 1.0f, 1.0f };

            GL.Light(LightName.Light1, LightParameter.Position, new float[] { luz_x, luz_y, luz_z });
            GL.Light(LightName.Light1, LightParameter.Ambient, ambient);
            GL.Light(LightName.Light1, LightParameter.Diffuse, diffuse);
            GL.Light(LightName.Light1, LightParameter.Specular, specular);
            GL.Light(LightName.Light1, LightParameter.SpotDirection, direcao);

            /* GL.LightModel(LightModelParameter.LightModelAmbient, new float[] { 0.2f, 0.2f, 0.2f, 1.0f });
             GL.LightModel(LightModelParameter.LightModelTwoSide, 1);
             GL.LightModel(LightModelParameter.LightModelLocalViewer, 1);
             GL.Hint(HintTarget.PerspectiveCorrectionHint, HintMode.Nicest);         // Really Nice Perspective Calculations
             */
            GL.ShadeModel(ShadingModel.Smooth);

            GL.BlendFunc(BlendingFactor.SrcAlpha, BlendingFactor.OneMinusSrcAlpha);
            //   GL.ColorMaterial(MaterialFace.FrontAndBack, ColorMaterialParameter.AmbientAndDiffuse);
            GL.Hint(HintTarget.LineSmoothHint, HintMode.Nicest);
            GL.ClearDepth(1.0);
            GL.DepthFunc(DepthFunction.Lequal);
            GL.FrontFace(FrontFaceDirection.Cw);
            GL.CullFace(CullFaceMode.FrontAndBack);

            GL.PolygonOffset(2, 1);
            GL.PolygonMode(MaterialFace.FrontAndBack, PolygonMode.Fill);
            GL.Hint(HintTarget.PolygonSmoothHint, HintMode.Nicest);
            GL.Enable(EnableCap.StencilTest);
            GL.ClearStencil(1);
            GL.StencilMask(0xFFFFFFFF); // read&write

            // GL.Viewport(0, 0, w,h);
        }
        static Vector3d p1, p2, p3,n1,v1,v2;
       // static float tamCubo = 30;
        static double dist = 9990;


        public static void DrawSky(bool degrade_azul)
        {
            //  GL.Color3(Color.Red);
            if (!degrade_azul)
            {
                GL.Begin(PrimitiveType.Polygon);
                GL.Color3(Color.White);
                GL.Vertex3(-1000, 600, -dist);
                GL.Color3(Color.Black);
                GL.Vertex3(-1000, -600, -dist);
                GL.Vertex3(1500, -600, -dist);
                GL.Color3(Color.White);
                GL.Vertex3(1500, 600, -dist);
                GL.Vertex3(-1500, 600, -dist);
                GL.End();
            }
            else
            {
                GL.Begin(PrimitiveType.Polygon);
                GL.Color3(Color.White);
                GL.Vertex3(-1000, 600, -dist);
                GL.Color3(Color.Blue);
                GL.Vertex3(-1000, -300, -dist);
                GL.Vertex3(1500, -300, -dist);
                GL.Color3(Color.White);
                GL.Vertex3(1500, 600, -dist);
                GL.Vertex3(-1500, 600, -dist);
                GL.End();
            }
        }
        public static void DesenhaEixosPrincipais(ref bool orto)
        {
            GL.Color3(Color.Red);
            //x
  
            GL.Begin(PrimitiveType.Lines);
            GL.Vertex3(0, 0, 0);
            if (orto)
            {
                //x
                GL.Vertex3(-40, 0, 0);
                GL.End();
                GL.Begin(PrimitiveType.Lines);
                GL.Vertex3(-45, -7, 0);
                GL.Vertex3(-55, 7, 0);
                GL.End();
                GL.Begin(PrimitiveType.Lines);
                GL.Vertex3(-45, 7, 0);
                GL.Vertex3(-55, -7, 0);
                GL.End();


                //y
                GL.Color3(Color.Green);
                GL.Begin(PrimitiveType.Lines);
                GL.Vertex3(0,0,0);
                GL.Vertex3(0, 40, 0);
                GL.End();

                GL.Begin(PrimitiveType.LineStrip);
                GL.Vertex3(-5, 45, 0);
                GL.Vertex3(-10, 55, 0);
                GL.Vertex3(-15, 45, 0);
                GL.End();

                GL.Begin(PrimitiveType.Lines);
                GL.Vertex3(-10, 55, 0);
                GL.Vertex3(-10, 65, 0);
                GL.End();

                //z
                GL.Color3(Color.Blue);
                GL.Begin(PrimitiveType.Lines);
                GL.Vertex3(0, 0, 0);
                GL.Vertex3(0, 0, -40);
                GL.End();

                GL.Begin(PrimitiveType.LineStrip);
                GL.Vertex3(5, 0, -55);
                GL.Vertex3(-5, 0, -55);
                GL.Vertex3(5, 0, -45);
                GL.Vertex3(-5, 0, -45);
                GL.End();
            }
            else
            {
                GL.Vertex3(40, 0, 0);
                GL.End();

                GL.Begin(PrimitiveType.Lines);
                GL.Vertex3(45, -7, 0);
                GL.Vertex3(55, 7, 0);
                GL.End();
                GL.Begin(PrimitiveType.Lines);
                GL.Vertex3(45, 7, 0);
                GL.Vertex3(55, -7, 0);
                GL.End();

                //y
                GL.Color3(Color.Green);
                GL.Begin(PrimitiveType.Lines);
                GL.Vertex3(0, 0, 0);
                GL.Vertex3(0, -40, 0);
                GL.End();

                GL.Begin(PrimitiveType.LineStrip);
                GL.Vertex3(5, -45, 0);
                GL.Vertex3(10, -55, 0);
                GL.Vertex3(15, -45, 0);
                GL.End();

                GL.Begin(PrimitiveType.Lines);
                GL.Vertex3(10, -55, 0);
                GL.Vertex3(10, -65, 0);
                GL.End();

                //z
                GL.Color3(Color.Blue);
                GL.Begin(PrimitiveType.Lines);
                GL.Vertex3(0, 0, 0);
                GL.Vertex3(0, 0, -40);
                GL.End();

                GL.Begin(PrimitiveType.LineStrip);
                GL.Vertex3(-5, 0, -55);
                GL.Vertex3(5, 0, -55);
                GL.Vertex3(-5, 0, -45);
                GL.Vertex3(5, 0, -45);
                GL.End();
            }
        }

        public static void centro(double x, double y, double z)
        {
            double[] ColRed = new double[4] { 1.0, 0.0, 0.0, 0 };
            //    ClearScreen();         

           /* Gl.glColor4dv(ColRed);

            Gl.glBegin(Gl.GL_LINES);
            Gl.glVertex3d(x, y, z);
            Gl.glVertex3d(x, y, z + .2);
            Gl.glEnd();

            Gl.glColor4dv(ColRed);
            Gl.glBegin(Gl.GL_LINES);
            Gl.glVertex3d(x, y, z);
            Gl.glVertex3d(x, y + .2, z);
            Gl.glEnd();

            Gl.glColor4dv(ColRed);
            Gl.glBegin(Gl.GL_LINES);
            Gl.glVertex3d(x, y, z);
            Gl.glVertex3d(x + .2, y, z);
            Gl.glEnd();
            Gl.glFinish();*/
        }

        /// </summary>
 

        public static void UpdateOGLMatrix()
        {
         //   Gl.glGetIntegerv(Gl.GL_VIEWPORT, ViewPort);
         //   Gl.glGetDoublev(Gl.GL_MODELVIEW_MATRIX, ModelViewMatrix);
          //  Gl.glGetDoublev(Gl.GL_PROJECTION_MATRIX, ProjectionMatrix);
        }
        public static void ReShapeOGL(int w, int h, double ax = -55, double ay = -10, double az = 30, bool Perspectiva = false)
        {
            /*
                        float[] lightAmbient = { 0.0f, 0.0f, 0.0f, 1.0f };
                        float[] lightDiffuse = { 1.0f, 0.0f, 1.0f, 1.0f };
                        float[] lightSpecular = { 1.0f, 0.0f, 1.0f, 1.0f };
                        // light_position is NOT default value
                        float[] lightPosition = { 1.0f, 1.0f, 1.0f, 0.0f };

                        Gl.glLightfv(Gl.GL_LIGHT0, Gl.GL_AMBIENT, lightAmbient);
                        Gl.glLightfv(Gl.GL_LIGHT0, Gl.GL_DIFFUSE, lightDiffuse);
                        Gl.glLightfv(Gl.GL_LIGHT0, Gl.GL_SPECULAR, lightSpecular);
                        Gl.glLightfv(Gl.GL_LIGHT0, Gl.GL_POSITION, lightPosition);

                        Gl.glEnable(Gl.GL_LIGHTING);
                        Gl.glEnable(Gl.GL_LIGHT0);
                        Gl.glDepthFunc(Gl.GL_LESS);
                        Gl.glEnable(Gl.GL_DEPTH_TEST);

                        Gl.glViewport(0, 0, w, h);
                        Gl.glMatrixMode(Gl.GL_PROJECTION);
                        Gl.glLoadIdentity();
                        if (w <= h)
                        {
                            Gl.glOrtho(-4.0, 4.0, -4.0 * h / w, 4.0 * h / w, -4.0, 4.0);
                        }
                        else
                        {
                            Gl.glOrtho(-4.0 * w / h, 4.0 * w / h, -4.0, 4.0, -4.0, 4.0);
                        }
                        Gl.glMatrixMode(Gl.GL_MODELVIEW);
                        Gl.glLoadIdentity();
                        */
            /* Gl.glViewport(0, 0, w, h);
              // set the matrix mode to project
              Gl.glMatrixMode(Gl.GL_PROJECTION);
              // load the identity matrix
              Gl.glLoadIdentity();
              // create the viewing frustum
              Glu.gluPerspective(45.0, (float)w / (float)h, 1, 100);
              // set the matrix mode to modelview
              Gl.glMatrixMode(Gl.GL_MODELVIEW);
              // load the identity matrix
              Gl.glLoadIdentity();
              // position the view point
              Glu.gluLookAt(0, 0, .1,
                            1,0,0,
                            0, 1, 0);
            
              */
            //Gl.glOrtho(0, w, h, 0, -1, 1);

           /* if (!Perspectiva)
            {
                Gl.glViewport(0, 0, w, h);

                OGLtop = 1;
                OGLbotton = -1;
                OGLleft = -(double)(w) / (double)h;
                OGLright = -OGLleft;

                Gl.glPushMatrix();
                Gl.glLoadIdentity();

                Gl.glMatrixMode(Gl.GL_PROJECTION);
                Gl.glPushMatrix();                     // save current modelview matrix
                Gl.glLoadIdentity();

                Gl.glOrtho(OGLleft, OGLright, -1, 1, -100000, 1000);
                Gl.glMatrixMode(Gl.GL_MODELVIEW);
                Gl.glLoadIdentity();

                Gl.glRotated(ax, 1, 0, 0);
                Gl.glRotated(ay, 0, 1, 0);
                Gl.glRotated(az, 0, 0, 1);
            }
            else
            {
                Gl.glViewport(0, 0, w, h);

                OGLtop = 1;
                OGLbotton = -1;
                OGLleft = -(double)(w) / (double)h;
                OGLright = -OGLleft;

                Gl.glPushMatrix();
                Gl.glLoadIdentity();

                Gl.glMatrixMode(Gl.GL_PROJECTION);
                Gl.glPushMatrix();                     // save current modelview matrix
                Gl.glLoadIdentity();

                //Glu.gluPerspective(45, w  / h, 0.1, 10000.0f);
                Glu.gluPerspective(45, w / h, 0.1, 10000.0f);
                Gl.glMatrixMode(Gl.GL_MODELVIEW);
                Gl.glLoadIdentity();

                Gl.glRotated(-55, 1, 0, 0);
                Gl.glRotated(-10, 0, 1, 0);
                Gl.glRotated(30, 0, 0, 1); 
            }*/
        }

        public static void ReShapeOGL(double x, double y, double z)
        {
           /* Gl.glViewport(0, 0, OGL.Size.Width, OGL.Size.Height);

            OGLtop = 1;
            OGLbotton = -1;
            OGLleft = -(double)(OGL.Size.Width) / (double)OGL.Size.Height;
            OGLright = -OGLleft;

            Gl.glMatrixMode(Gl.GL_PROJECTION);
            Gl.glPushMatrix(); 
            Gl.glLoadIdentity();

            Glu.gluPerspective(45, (double)(OGL.Size.Width) / (double)OGL.Size.Height, 0, 1000);

            Gl.glMatrixMode(Gl.GL_MODELVIEW);
            Gl.glLoadIdentity();*/
        }

      /*  public static void pos(double* px, double* py, double* pz, int x, int y, int[] viewport, int right, int left, int top, int botton, int znear)
        {
            *px = (double)(x - viewport[0]) / (double)(viewport[2]);
            *py = (double)(y - viewport[1]) / (double)(viewport[3]);

            *px = left + (*px) * (right - left);
            *py = top + (*py) * (botton - top);
            *pz = znear;
        }

        public static void pos(double* px, double* py, double* pz, int x, int y, int[] viewport)
        {
            *px = (double)(x - viewport[0]) / (double)(viewport[2]);
            *py = (double)(y - viewport[1]) / (double)(viewport[3]);

            *px = OGLleft + (*px) * (OGLright - OGLleft);
            *py = OGLtop + (*py) * (OGLbotton - OGLtop);
            *pz = OGLzNear;
        }
        */
        public static double vlen(double x, double y, double z)
        {
            return Math.Sqrt(x * x + y * y + z * z);
        }

        public static bool InvertMatrixd(double[] m, double[] invOut)
        {
            double[] inv = new double[16];
            double det;
            int i;

            inv[0] = m[5] * m[10] * m[15] - m[5] * m[11] * m[14] - m[9] * m[6] * m[15]
                + m[9] * m[7] * m[14] + m[13] * m[6] * m[11] - m[13] * m[7] * m[10];

            inv[4] = -m[4] * m[10] * m[15] + m[4] * m[11] * m[14] + m[8] * m[6] * m[15]
               - m[8] * m[7] * m[14] - m[12] * m[6] * m[11] + m[12] * m[7] * m[10];

            inv[8] = m[4] * m[9] * m[15] - m[4] * m[11] * m[13] - m[8] * m[5] * m[15]
               + m[8] * m[7] * m[13] + m[12] * m[5] * m[11] - m[12] * m[7] * m[9];

            inv[12] = -m[4] * m[9] * m[14] + m[4] * m[10] * m[13] + m[8] * m[5] * m[14]
               - m[8] * m[6] * m[13] - m[12] * m[5] * m[10] + m[12] * m[6] * m[9];

            inv[1] = -m[1] * m[10] * m[15] + m[1] * m[11] * m[14] + m[9] * m[2] * m[15]
               - m[9] * m[3] * m[14] - m[13] * m[2] * m[11] + m[13] * m[3] * m[10];

            inv[5] = m[0] * m[10] * m[15] - m[0] * m[11] * m[14] - m[8] * m[2] * m[15]
               + m[8] * m[3] * m[14] + m[12] * m[2] * m[11] - m[12] * m[3] * m[10];

            inv[9] = -m[0] * m[9] * m[15] + m[0] * m[11] * m[13] + m[8] * m[1] * m[15]
               - m[8] * m[3] * m[13] - m[12] * m[1] * m[11] + m[12] * m[3] * m[9];

            inv[13] = m[0] * m[9] * m[14] - m[0] * m[10] * m[13] - m[8] * m[1] * m[14]
               + m[8] * m[2] * m[13] + m[12] * m[12] * m[1] - m[12] * m[2] * m[9];

            inv[2] = m[1] * m[6] * m[15] - m[1] * m[7] * m[14] - m[5] * m[2] * m[15]
               + m[5] * m[3] * m[14] + m[13] * m[2] * m[7] - m[13] * m[3] * m[6];

            inv[6] = -m[0] * m[6] * m[15] + m[0] * m[7] * m[14] + m[4] * m[2] * m[15]
               - m[4] * m[3] * m[14] - m[12] * m[2] * m[7] + m[12] * m[3] * m[6];

            inv[10] = m[0] * m[5] * m[15] - m[0] * m[7] * m[13] - m[4] * m[1] * m[15]
               + m[4] * m[3] * m[13] + m[12] * m[1] * m[7] - m[12] * m[3] * m[5];

            inv[14] = -m[0] * m[5] * m[14] + m[0] * m[6] * m[13] + m[4] * m[1] * m[14]
               - m[4] * m[2] * m[13] - m[12] * m[1] * m[6] + m[12] * m[2] * m[5];

            inv[3] = -m[1] * m[6] * m[11] + m[1] * m[7] * m[10] + m[5] * m[2] * m[11]
               - m[5] * m[3] * m[10] - m[9] * m[2] * m[7] + m[9] * m[3] * m[6];

            inv[7] = m[0] * m[6] * m[11] - m[0] * m[7] * m[10] - m[4] * m[2] * m[11]
               + m[4] * m[3] * m[10] + m[8] * m[2] * m[7] - m[8] * m[3] * m[6];

            inv[11] = -m[0] * m[5] * m[11] + m[0] * m[7] * m[9] + m[4] * m[1] * m[11]
               - m[4] * m[3] * m[9] - m[8] * m[1] * m[7] + m[8] * m[3] * m[5];

            inv[15] = m[0] * m[5] * m[10] - m[0] * m[6] * m[9] - m[4] * m[1] * m[10]
               + m[4] * m[2] * m[9] + m[8] * m[1] * m[6] - m[8] * m[2] * m[5];

            det = m[0] * inv[0] + m[1] * inv[4] + m[2] * inv[8] + m[3] * inv[12];

            if (det == 0)
                return false;

            det = 1.0 / det;

            for (i = 0; i < 16; i++)
                invOut[i] = inv[i] * det;

            return true;
        }

    }
}
