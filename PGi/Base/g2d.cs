using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using OpenTK;
using OpenTK.Input;
using OpenTK.Graphics.OpenGL;
using OpenTK.Platform;
using System.Drawing.Drawing2D;
using System.Drawing;

namespace PG
{

    static class GraphicsExtension
    {
        private static GraphicsPath GenerateRoundedRectangle(
            this Graphics graphics,
            RectangleF rectangle,
            float radius)
        {
            float diameter;
            GraphicsPath path = new GraphicsPath();
            if (radius <= 0.0F)
            {
                path.AddRectangle(rectangle);
                path.CloseFigure();
                return path;
            }
            else
            {
                if (radius >= (Math.Min(rectangle.Width, rectangle.Height)) / 2.0)
                    return graphics.GenerateCapsule(rectangle);
                diameter = radius * 2.0F;
                SizeF sizeF = new SizeF(diameter, diameter);
                RectangleF arc = new RectangleF(rectangle.Location, sizeF);
                path.AddArc(arc, 180, 90);
                arc.X = rectangle.Right - diameter;
                path.AddArc(arc, 270, 90);
                arc.Y = rectangle.Bottom - diameter;
                path.AddArc(arc, 0, 90);
                arc.X = rectangle.Left;
                path.AddArc(arc, 90, 90);
                path.CloseFigure();
            }
            return path;
        }
        private static GraphicsPath GenerateCapsule(
            this Graphics graphics,
            RectangleF baseRect)
        {
            float diameter;
            RectangleF arc;
            GraphicsPath path = new GraphicsPath();
            try
            {
                if (baseRect.Width > baseRect.Height)
                {
                    diameter = baseRect.Height;
                    SizeF sizeF = new SizeF(diameter, diameter);
                    arc = new RectangleF(baseRect.Location, sizeF);
                    path.AddArc(arc, 90, 180);
                    arc.X = baseRect.Right - diameter;
                    path.AddArc(arc, 270, 180);
                }
                else if (baseRect.Width < baseRect.Height)
                {
                    diameter = baseRect.Width;
                    SizeF sizeF = new SizeF(diameter, diameter);
                    arc = new RectangleF(baseRect.Location, sizeF);
                    path.AddArc(arc, 180, 180);
                    arc.Y = baseRect.Bottom - diameter;
                    path.AddArc(arc, 0, 180);
                }
                else path.AddEllipse(baseRect);
            }
            catch { path.AddEllipse(baseRect); }
            finally { path.CloseFigure(); }
            return path;
        }

        /// <summary>
        /// Draws a rounded rectangle specified by a pair of coordinates, a width, a height and the radius 
        /// for the arcs that make the rounded edges.
        /// </summary>
        /// <param name="brush">System.Drawing.Pen that determines the color, width and style of the rectangle.</param>
        /// <param name="x">The x-coordinate of the upper-left corner of the rectangle to draw.</param>
        /// <param name="y">The y-coordinate of the upper-left corner of the rectangle to draw.</param>
        /// <param name="width">Width of the rectangle to draw.</param>
        /// <param name="height">Height of the rectangle to draw.</param>
        /// <param name="radius">The radius of the arc used for the rounded edges.</param>

        public static void DrawRoundedRectangle(
            this Graphics graphics,
            Pen pen,
            float x,
            float y,
            float width,
            float height,
            float radius)
        {
            RectangleF rectangle = new RectangleF(x, y, width, height);
            GraphicsPath path = graphics.GenerateRoundedRectangle(rectangle, radius);
            SmoothingMode old = graphics.SmoothingMode;
            graphics.SmoothingMode = SmoothingMode.AntiAlias;
            graphics.DrawPath(pen, path);
            graphics.SmoothingMode = old;
        }

        /// <summary>
        /// Draws a rounded rectangle specified by a pair of coordinates, a width, a height and the radius 
        /// for the arcs that make the rounded edges.
        /// </summary>
        /// <param name="brush">System.Drawing.Pen that determines the color, width and style of the rectangle.</param>
        /// <param name="x">The x-coordinate of the upper-left corner of the rectangle to draw.</param>
        /// <param name="y">The y-coordinate of the upper-left corner of the rectangle to draw.</param>
        /// <param name="width">Width of the rectangle to draw.</param>
        /// <param name="height">Height of the rectangle to draw.</param>
        /// <param name="radius">The radius of the arc used for the rounded edges.</param>

        public static void DrawRoundedRectangle(
            this Graphics graphics,
            Pen pen,
            int x,
            int y,
            int width,
            int height,
            int radius)
        {
            graphics.DrawRoundedRectangle(
                pen,
                Convert.ToSingle(x),
                Convert.ToSingle(y),
                Convert.ToSingle(width),
                Convert.ToSingle(height),
                Convert.ToSingle(radius));
        }

        /// <summary>
        /// Fills the interior of a rounded rectangle specified by a pair of coordinates, a width, a height
        /// and the radius for the arcs that make the rounded edges.
        /// </summary>
        /// <param name="brush">System.Drawing.Brush that determines the characteristics of the fill.</param>
        /// <param name="x">The x-coordinate of the upper-left corner of the rectangle to fill.</param>
        /// <param name="y">The y-coordinate of the upper-left corner of the rectangle to fill.</param>
        /// <param name="width">Width of the rectangle to fill.</param>
        /// <param name="height">Height of the rectangle to fill.</param>
        /// <param name="radius">The radius of the arc used for the rounded edges.</param>

        public static void FillRoundedRectangle(
            this Graphics graphics,
            Brush brush,
            float x,
            float y,
            float width,
            float height,
            float radius)
        {
            RectangleF rectangle = new RectangleF(x, y, width, height);
            GraphicsPath path = graphics.GenerateRoundedRectangle(rectangle, radius);
            SmoothingMode old = graphics.SmoothingMode;
            graphics.SmoothingMode = SmoothingMode.AntiAlias;
            graphics.FillPath(brush, path);
            graphics.SmoothingMode = old;
        }

        /// <summary>
        /// Fills the interior of a rounded rectangle specified by a pair of coordinates, a width, a height
        /// and the radius for the arcs that make the rounded edges.
        /// </summary>
        /// <param name="brush">System.Drawing.Brush that determines the characteristics of the fill.</param>
        /// <param name="x">The x-coordinate of the upper-left corner of the rectangle to fill.</param>
        /// <param name="y">The y-coordinate of the upper-left corner of the rectangle to fill.</param>
        /// <param name="width">Width of the rectangle to fill.</param>
        /// <param name="height">Height of the rectangle to fill.</param>
        /// <param name="radius">The radius of the arc used for the rounded edges.</param>

        public static void FillRoundedRectangle(
            this Graphics graphics,
            Brush brush,
            int x,
            int y,
            int width,
            int height,
            int radius)
        {
            graphics.FillRoundedRectangle(
                brush,
                Convert.ToSingle(x),
                Convert.ToSingle(y),
                Convert.ToSingle(width),
                Convert.ToSingle(height),
                Convert.ToSingle(radius));
        }
    }
    public static class g2d
    {
        static double[] winZ;

        public static void Linha3D()
        {

        }
        public static void UnProject(int mouseX, int mouseY, ref double worldX, ref double worldY, ref double worldZ)
        {
            
            winZ = new double[1];
/*
            Gl.glGetDoublev(Gl.GL_MODELVIEW_MATRIX, TOpenGl.ModelViewMatrix);
            Gl.glGetDoublev(Gl.GL_PROJECTION_MATRIX, TOpenGl.ProjectionMatrix);
            Gl.glGetIntegerv(Gl.GL_VIEWPORT, TOpenGl.ViewPort);

            double realY = TOpenGl.ViewPort[3] - (int)mouseY;
            Gl.glReadPixels(mouseX, mouseY, 1, 1, Gl.GL_DEPTH_COMPONENT, Gl.GL_FLOAT, winZ);
            Glu.gluUnProject(mouseX, realY, winZ[0], TOpenGl.ModelViewMatrix, TOpenGl.ProjectionMatrix, TOpenGl.ViewPort,
                out worldX, out worldY, out worldZ);*/
        }

        public static void Project(ref double pixelX, ref double pixelY, double worldX, double worldY, double worldZ)
        {
            double pixelZ;

         /*   Gl.glGetDoublev(Gl.GL_MODELVIEW_MATRIX, TOpenGl.ModelViewMatrix);
            Gl.glGetDoublev(Gl.GL_PROJECTION_MATRIX, TOpenGl.ProjectionMatrix);
            Gl.glGetIntegerv(Gl.GL_VIEWPORT, TOpenGl.ViewPort);
            Glu.gluProject(worldX, worldY, worldZ, TOpenGl.ModelViewMatrix, TOpenGl.ProjectionMatrix, TOpenGl.ViewPort, out pixelX, out pixelY, out pixelZ);
            pixelY = TOpenGl.ViewPort[3] - pixelY;*/
        }
        public static void Project(ref double[] pixelX, ref double[] pixelY, double worldX, double worldY, double worldZ, double[] model, double[] proj, int[] ViewPort)
        {
            double[] pixelZ = new double[1];

            OpenTK.Graphics.Glu.Project(worldX, worldY, worldZ, model, proj, ViewPort, pixelX, pixelY, pixelZ);
            pixelY[0] = ViewPort[3] - pixelY[0];
        }

        public static void glutInit()
        {
            //Glut.glutInit();
        }
        public static void Color3d(double r, double g, double b)
        {
       //     Gl.glColor3d(r,g,b);
        }

        public static void beginline()
        {
       //     Gl.glBegin(Gl.GL_LINES);
        }

        public static void endline()
        {
         //   Gl.glEnd();
        }

        public static void ClearScreen(double r, double g, double b)
        {
            /*Gl.glClear(Gl.GL_COLOR_BUFFER_BIT | Gl.GL_DEPTH_BUFFER_BIT);
            Gl.glMatrixMode(Gl.GL_MODELVIEW);
            Gl.glLoadIdentity();
            Gl.glClearColor((float)r, (float)g, (float)b, 0);*/
        }

        public static void line(float xi, float yi, float xf, float yf)
        {
          /*  Gl.glBegin(Gl.GL_LINES);
            Gl.glVertex2f(xi, yi);
            Gl.glVertex2f(xf, yf);
            Gl.glEnd();*/
        }

        public static void CotaCruz(float x, float y, float r, float g, float b)
        {
        /*    Gl.glColor3f(r, g, b);
            Gl.glBegin(Gl.GL_LINES);
            Gl.glVertex2f(x-4, y - 4);
            Gl.glVertex2f(x+4, y+4);
            Gl.glEnd();
            */
         //   Gl.glBegin(Gl.GL_LINES);
         //   Gl.glVertex2f(x - 4, y - 4);
         //   Gl.glVertex2f(x + 4, y + 4);
         //   Gl.glEnd();

          /*  Gl.glBegin(Gl.GL_LINE_LOOP);
            Gl.glVertex2f(x - 6, y - 6);
            Gl.glVertex2f(x, y);
            Gl.glVertex2f(x - 6, y + 6);
            Gl.glEnd();*/

         //   StrokeText("o", x - 3, y + 3,
         //               0.07f, -0.07f,
          //              r, g, b);
        }

        public static void Cruz(float x, float y, float r, float g, float b)
        {
         /*   Gl.glColor3f(r, g, b);
            Gl.glBegin(Gl.GL_LINES);
            Gl.glVertex2f(x - 5, y);
            Gl.glVertex2f(x + 5, y);
            Gl.glEnd();

            Gl.glBegin(Gl.GL_LINES);
            Gl.glVertex2f(x, y + 5);
            Gl.glVertex2f(x, y - 5);
            Gl.glEnd();*/
        }

        public static int StrokeWidth(string str)
        {
            int width = 0;
          //  for (int i = 0; i < str.Length; ++i)
          //      width += Glut.glutStrokeWidth(Glut.GLUT_STROKE_MONO_ROMAN, str[i]);
            return width;
        }

        public static void StrokeText(string str, double x, double y, double escalaX, double escalaY, byte r, byte g, byte b, float rotacao = 0)
        {
          /*  Gl.glColor3ub(r,g,b);

            Gl.glPushMatrix();
           // Gl.glRasterPos2d(x, y); 
            Gl.glTranslated(x, y, 0);
            Gl.glScaled(escalaX, escalaY, 0);
        
            if (rotacao != 0)
                Gl.glRotatef(rotacao, 0, 0, 1);

          //  float w = -StrokeWidth(str);

            for (int i = 0; i < str.Length; i++)
             //   Glut.glutBitmapCharacter(Glut.GLUT_BITMAP_TIMES_ROMAN_10,str[i]);
                 Glut.glutStrokeCharacter(Glut.GLUT_STROKE_MONO_ROMAN, str[i]);

            Gl.glPopMatrix();
            Gl.glFlush();*/
        }


        /*
        * Function that handles the drawing of a circle using the triangle fan
        * method. This will create a filled circle.
        *
        * Params:
       *	x (GLFloat) - the x position of the center point of the circle
       *	y (GLFloat) - the y position of the center point of the circle
       *	radius (GLFloat) - the radius that the painted circle will have
       */
        public static void drawFilledCircle(double x, double y, double radius, int lineAmount)
        {
            GL.Begin(PrimitiveType.Polygon);
            for (int i = 0; i <= lineAmount; i++)
            {
                GL.Vertex2(System.Convert.ToSingle(x + (radius * Math.Cos(i * 6.2831 / lineAmount))),
                              System.Convert.ToSingle(y + (radius * Math.Sin(i * 6.2831 / lineAmount))));
            };
            GL.End();
        }

        /*
         * Function that handles the drawing of a circle using the line loop
         * method. This will create a hollow circle.
         *
         * Params:
         *	x (GLFloat) - the x position of the center point of the circle
         *	y (GLFloat) - the y position of the center point of the circle
         *	radius (GLFloat) - the radius that the painted circle will have
         */

        public static void drawCircle(double x, double y, double radius, int lineAmount)
        {
      

            //GLfloat radius = 0.8f; //radius

           // GL.Color3(cor);

            GL.Begin(PrimitiveType.LineLoop);
            for (int i = 0; i <= lineAmount; i++)
            {
                GL.Vertex2(System.Convert.ToSingle(x + (radius * Math.Cos(i * 6.2831 / lineAmount))),
                              System.Convert.ToSingle(y + (radius * Math.Sin(i * 6.2831 / lineAmount))));
            };
            GL.End();
            /*for (int j = 0; j <= lineAmount; j++)
            {
                double hh = j;
                GL.Begin(PrimitiveType.Lines);
                GL.Vertex2((x + (radius * Math.Cos(hh * 6.2831 / lineAmount))), (y + (radius * Math.Sin(hh * 6.2831 / lineAmount))));
                GL.Vertex2((x + (radius * Math.Cos((hh +1) * 6.2831 / lineAmount))), (y + (radius * Math.Sin((hh+1) * 6.2831 / lineAmount))));
                //  Cad.DrawLine(mPen, Desenho.pixelX((x + (this.raio * Math.Cos(j * twicePi / 20)))), Desenho.pixelY((y + (this.raio * Math.Sin(j * twicePi / 20)))),
                //                    Desenho.pixelX((x + (this.raio * Math.Cos((j + 1) * twicePi / 20)))), Desenho.pixelY((y + (this.raio * Math.Sin((j + 1) * twicePi / 20)))));
                GL.End();
            }*/


        }

        public static float pixelX(float coordX, float precisaoPixel, float pontozeroX) { return coordX / precisaoPixel + pontozeroX; }
        public static float pixelY(float coordY, float precisaoPixel, float pontozeroY) { return coordY / precisaoPixel * -1 + pontozeroY; }
        
 
        public static void DrawArc(double cx, double cy, double r, double start_angle, double arc_angle, int num_segments,
                                    float precisaoPixel, double pontozeroX, double pontozeroY,
                                    byte R, byte G, byte B)
        {
            double theta = arc_angle / (double)(num_segments - 1);//theta is now calculated from the arc angle instead, the - 1 bit comes from the fact that the arc is open

            double tangetial_factor = System.Convert.ToSingle(Math.Tan(theta * Const.PIDiv180));

            double radial_factor = System.Convert.ToSingle(Math.Cos(theta * Const.PIDiv180));

            double x = r * System.Convert.ToSingle(Math.Cos(start_angle * Const.PIDiv180));//we now start at the start angle
            double y = r * System.Convert.ToSingle(Math.Sin(start_angle * Const.PIDiv180));

            precisaoPixel = FPrincipal.precisaoPixel;
            pontozeroX = FPrincipal.ponto_zero[0];
            pontozeroY = FPrincipal.ponto_zero[1];

           // Gl.glColor3ub(R, G, B);
           // Gl.glBegin(Gl.GL_LINE_STRIP);//since the arc is not a closed curve, this is a strip now

            for (int ii = 0; ii < num_segments; ii++)
            {
                float pixx = pixelX(System.Convert.ToSingle(x + cx), System.Convert.ToSingle(precisaoPixel), System.Convert.ToSingle(pontozeroX));
                float pixy = pixelY(System.Convert.ToSingle(y + cy), System.Convert.ToSingle(precisaoPixel), System.Convert.ToSingle(pontozeroY));

          //      Gl.glVertex2f(pixx, pixy);

                double tx = -y;
                double ty = x;

                x += tx * tangetial_factor;
                y += ty * tangetial_factor;

                x *= radial_factor;
                y *= radial_factor;
            }
           // GL.End();
        }
    }
}
