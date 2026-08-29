using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


namespace PG
{
    [Serializable]
    public class TTexto : TObjetoDesenho
    {

        public string texto;

        public double x, y, z, tamx, tamy;

        public float angulo;

        [NonSerialized]
        public float y_giro, x_giro;
        [NonSerialized]
        public float r, g, b;

        public string TipoObjeto;
        [NonSerialized]
        List<TLinha> linhas_poligonal;
        [NonSerialized]
        CoordenadaD[] coord;
        [NonSerialized]
        public TPoligono Retangulo;

        [NonSerialized]
        vec3 coord1Retangulo;
        [NonSerialized]
        vec3 coord2Retangulo;
        [NonSerialized]
        vec3 coord3Retangulo;
        [NonSerialized]
        vec3 coord4Retangulo;

        [NonSerialized]
        float comp, altura;
        [NonSerialized]
        System.Drawing.SizeF size;
        [NonSerialized]
        double dist1, dist2;

        [NonSerialized]
        public System.Drawing.Pen mPen = new System.Drawing.Pen(System.Drawing.Color.White);
        [NonSerialized]
        public System.Drawing.SolidBrush Brush = new System.Drawing.SolidBrush(System.Drawing.Color.White);
        [NonSerialized]
        System.Drawing.Drawing2D.GraphicsState EstadoCad; 
        
        public TTexto() { }
        public TTexto(string texto, double posx, double posy, double tamx, double tamy, float r, float g, float b, float angulo, TLayer layer = null, string TipoObjeto = "")
        {
            this.TipoObjeto = TipoObjeto;
            base.Visivel = true;
            this.texto = texto;
            this.x     = posx;
            this.y     = posy;
            this.tamx  = tamx;
            this.tamy  = tamy;
            this.r = r;
            this.g = g;
            this.b = b;
            this.angulo = angulo;
            base.Selecionado = false;

       //     base.linhas_poligonal = new List<TLinha>();
            TPonto p1 = new TPonto(x-10, y, 0,-1, -1, -1);
            TPonto p2 = new TPonto(x + (tamx * 100), y,0, -1, -1, -1);
            TPonto p3 = new TPonto(x + (tamx * 100), y + (tamy * 50), 0,-1, -1, -1);
            TPonto p4 = new TPonto(x-10, y + (tamy * 50),0, -1, -1, -1);

        //    base.linhas_poligonal.Add(new TLinha(p1, p2, -1));
        //    base.linhas_poligonal.Add(new TLinha(p2, p3, -1));
         //   base.linhas_poligonal.Add(new TLinha(p3, p4, -1));
          //  base.linhas_poligonal.Add(new TLinha(p4, p1, -1));

            base.Tipo = "texto";
            this.layer = layer;

            AddGrips();
            base.Grips = this.Grips;
            mPen = new System.Drawing.Pen(System.Drawing.Color.White);
            Brush = new System.Drawing.SolidBrush(System.Drawing.Color.White);
        }
        public TTexto(string texto, double posx, double posy, double posz,double tamx, double tamy, float r, float g, float b, float angulo, TLayer layer = null, string TipoObjeto = "")
        {
            this.TipoObjeto = TipoObjeto;
            base.Visivel = true;
            this.texto = texto;
            this.x = posx;
            this.y = posy;
            this.z = posz;
            this.tamx = tamx;
            this.tamy = tamy;
            this.r = r;
            this.g = g;
            this.b = b;
            this.angulo = angulo;
            base.Selecionado = false;

            //     base.linhas_poligonal = new List<TLinha>();
            TPonto p1 = new TPonto(x - 10, y, 0,-1, -1, -1);
            TPonto p2 = new TPonto(x + (tamx * 100), y, 0, -1, -1, -1);
            TPonto p3 = new TPonto(x + (tamx * 100), y + (tamy * 50), 0, -1, -1, -1);
            TPonto p4 = new TPonto(x - 10, y + (tamy * 50), 0, -1, -1, -1);

            //    base.linhas_poligonal.Add(new TLinha(p1, p2, -1));
            //    base.linhas_poligonal.Add(new TLinha(p2, p3, -1));
            //   base.linhas_poligonal.Add(new TLinha(p3, p4, -1));
            //  base.linhas_poligonal.Add(new TLinha(p4, p1, -1));

            base.Tipo = "texto";
            this.layer = layer;

            AddGrips();
            base.Grips = this.Grips;
            mPen = new System.Drawing.Pen(System.Drawing.Color.White);
            Brush = new System.Drawing.SolidBrush(System.Drawing.Color.White);
        }


        public void AddRetangulo()
        {
            try
            {
                //size = g.MeasureString("Hello world", font);
                linhas_poligonal = new List<TLinha>();
                comp = texto.Length * 73.5f;
                comp *= (float)tamx;

                altura = texto.Length * 53;
                altura *= -(float)tamy;

                coord1Retangulo = new vec3(x, y, 0);

                coord2Retangulo = new vec3(x + comp, y, 0);
                coord2Retangulo = coord2Retangulo.Rotate(coord1Retangulo, angulo * Const.PIDiv180);

                coord3Retangulo = new vec3(x + comp, y + altura, 0);
                coord3Retangulo = coord3Retangulo.Rotate(coord1Retangulo, angulo * Const.PIDiv180);

                coord4Retangulo = new vec3(x, y + altura, 0);
                coord4Retangulo = coord4Retangulo.Rotate(coord1Retangulo, angulo * Const.PIDiv180);

                coord = new CoordenadaD[5];
                coord[0].X = coord1Retangulo.x;
                coord[0].Y = coord1Retangulo.y;

                coord[1].X = coord2Retangulo.x;
                coord[1].Y = coord2Retangulo.y;

                coord[2].X = coord3Retangulo.x;
                coord[2].Y = coord3Retangulo.y;

                coord[3].X = coord4Retangulo.x;
                coord[3].Y = coord4Retangulo.y;

                coord[4].X = coord1Retangulo.x;
                coord[4].Y = coord1Retangulo.y;

                linhas_poligonal.Add(new TLinha(new TPonto(coord[0].X, coord[0].Y, 0), new TPonto(coord[1].X, coord[1].Y, 0), -1));
                linhas_poligonal.Add(new TLinha(new TPonto(coord[1].X, coord[1].Y, 0), new TPonto(coord[2].X, coord[2].Y, 0), -1));
                linhas_poligonal.Add(new TLinha(new TPonto(coord[2].X, coord[2].Y, 0), new TPonto(coord[3].X, coord[3].Y, 0), -1));
                linhas_poligonal.Add(new TLinha(new TPonto(coord[3].X, coord[3].Y, 0), new TPonto(coord[4].X, coord[4].Y, 0), -1));

                Retangulo = new TPoligono(ref coord, ref linhas_poligonal);
            }
            catch(Exception e)
            {
                System.Windows.Forms.MessageBox.Show(e.Message);
            }

        }

        public void OnMove(double x, double y)
        {
            this.x = x;
            this.y = y;

            comp = texto.Length * 73.5f;
            comp *= (float)tamx;

            altura = texto.Length * 53f;
            altura *= -(float)tamy;          
            
            Grips[0].x = x;
            Grips[0].y = y;

            coord1Retangulo.x = x;
            coord1Retangulo.y = y;

            coord2Retangulo.x = x + comp;
            coord2Retangulo.y = y;
            coord2Retangulo   = coord2Retangulo.Rotate(coord1Retangulo, angulo * Const.PIDiv180);

            coord3Retangulo.x = x + comp;
            coord3Retangulo.y = y + altura;
            coord3Retangulo   = coord3Retangulo.Rotate(coord1Retangulo, angulo * Const.PIDiv180);

            coord4Retangulo.x = x;
            coord4Retangulo.y = y + altura;
            coord4Retangulo   = coord4Retangulo.Rotate(coord1Retangulo, angulo * Const.PIDiv180);

            coord[0].X = coord1Retangulo.x;
            coord[0].Y = coord1Retangulo.y;

            coord[1].X = coord2Retangulo.x;
            coord[1].Y = coord2Retangulo.y;

            coord[2].X = coord3Retangulo.x;
            coord[2].Y = coord3Retangulo.y;

            coord[3].X = coord4Retangulo.x;
            coord[3].Y = coord4Retangulo.y;

            coord[4].X = coord1Retangulo.x;
            coord[4].Y = coord1Retangulo.y;
           
            linhas_poligonal[0].pIni.x = coord[0].X;
            linhas_poligonal[0].pIni.y = coord[0].Y;
            linhas_poligonal[0].pFin.x = coord[1].X;
            linhas_poligonal[0].pFin.y = coord[1].Y;

            linhas_poligonal[1].pIni.x = coord[1].X;
            linhas_poligonal[1].pIni.y = coord[1].Y;
            linhas_poligonal[1].pFin.x = coord[2].X;
            linhas_poligonal[1].pFin.y = coord[2].Y;

            linhas_poligonal[2].pIni.x = coord[2].X;
            linhas_poligonal[2].pIni.y = coord[2].Y;
            linhas_poligonal[2].pFin.x = coord[3].X;
            linhas_poligonal[2].pFin.y = coord[3].Y;

            linhas_poligonal[3].pIni.x = coord[3].X;
            linhas_poligonal[3].pIni.y = coord[3].Y;
            linhas_poligonal[3].pFin.x = coord[4].X;
            linhas_poligonal[3].pFin.y = coord[4].Y;

            Grips[1].x = coord2Retangulo.x;
            Grips[1].y = coord2Retangulo.y;

            Grips[2].x = coord3Retangulo.x;
            Grips[2].y = coord3Retangulo.y;       
        }

        public void DesenhaSemZoom(double x, double y, string texto)
        {
            try { 
            using (System.Drawing.Font Fonte = new System.Drawing.Font("Tahoma", (float)(tamx * 100), System.Drawing.FontStyle.Italic))
            {

            }
            }
            catch (Exception ms)
            {
                System.Windows.Forms.MessageBox.Show("erro ao desenhar texto sem zoom " + ms.Message);
            }
        }

        public override void Desenha(ref System.Drawing.Graphics Cad)
        {
            try
            { 
            if (FPrincipal.pixelX(x) < 0) return;
            else
            if (FPrincipal.pixelX(x) > FPrincipal.w) return;
            else
            if (FPrincipal.pixelY(y) < 0) return;
            else
            if (FPrincipal.pixelY(y) > FPrincipal.h) return;

            coord1Retangulo.px_x = FPrincipal.pixelX(coord1Retangulo.x);
            coord1Retangulo.px_y = FPrincipal.pixelY(coord1Retangulo.y);

            coord2Retangulo.px_x = FPrincipal.pixelX(coord2Retangulo.x);
            coord2Retangulo.px_y = FPrincipal.pixelY(coord2Retangulo.y);

            dist1 = coord1Retangulo.DistanceBetweenPixels(coord2Retangulo);

            coord2Retangulo.px_x = FPrincipal.pixelX(coord2Retangulo.x);
            coord2Retangulo.px_y = FPrincipal.pixelY(coord2Retangulo.y);

            coord3Retangulo.px_x = FPrincipal.pixelX(coord3Retangulo.x);
            coord3Retangulo.px_y = FPrincipal.pixelY(coord3Retangulo.y);

            dist2 = coord2Retangulo.DistanceBetweenPixels(coord3Retangulo);

            if (dist1 < 5.5 || dist2 < 5.5)
            {
                mPen.Color = System.Drawing.Color.FromArgb(this.layer.Rgb[0], this.layer.Rgb[1], this.layer.Rgb[2]);
                
                Cad.DrawLine(mPen, FPrincipal.pixelX(linhas_poligonal[0].pIni.x), FPrincipal.pixelY(linhas_poligonal[0].pIni.y),
                                    FPrincipal.pixelX(linhas_poligonal[0].pFin.x), FPrincipal.pixelY(linhas_poligonal[0].pFin.y));
                Cad.DrawLine(mPen, FPrincipal.pixelX(linhas_poligonal[1].pIni.x), FPrincipal.pixelY(linhas_poligonal[1].pIni.y),
                                    FPrincipal.pixelX(linhas_poligonal[1].pFin.x), FPrincipal.pixelY(linhas_poligonal[1].pFin.y));
                Cad.DrawLine(mPen, FPrincipal.pixelX(linhas_poligonal[2].pIni.x), FPrincipal.pixelY(linhas_poligonal[2].pIni.y),
                                    FPrincipal.pixelX(linhas_poligonal[2].pFin.x), FPrincipal.pixelY(linhas_poligonal[2].pFin.y));
                Cad.DrawLine(mPen, FPrincipal.pixelX(linhas_poligonal[3].pIni.x), FPrincipal.pixelY(linhas_poligonal[3].pIni.y),
                                    FPrincipal.pixelX(linhas_poligonal[3].pFin.x), FPrincipal.pixelY(linhas_poligonal[3].pFin.y));
                
            /*    Gl.glColor3ub(this.layer.Rgb[0], this.layer.Rgb[1], this.layer.Rgb[2]);
                Gl.glBegin(Gl.GL_LINE_LOOP);
                Gl.glVertex2d(Desenho.pixelX(linhas_poligonal[0].pIni.x), Desenho.pixelY(linhas_poligonal[0].pIni.y));
                Gl.glVertex2d(Desenho.pixelX(linhas_poligonal[0].pFin.x), Desenho.pixelY(linhas_poligonal[0].pFin.y));

                Gl.glVertex2d(Desenho.pixelX(linhas_poligonal[1].pIni.x), Desenho.pixelY(linhas_poligonal[1].pIni.y));
                Gl.glVertex2d(Desenho.pixelX(linhas_poligonal[1].pFin.x), Desenho.pixelY(linhas_poligonal[1].pFin.y));

                Gl.glVertex2d(Desenho.pixelX(linhas_poligonal[2].pIni.x), Desenho.pixelY(linhas_poligonal[2].pIni.y));
                Gl.glVertex2d(Desenho.pixelX(linhas_poligonal[2].pFin.x), Desenho.pixelY(linhas_poligonal[2].pFin.y));
                Gl.glEnd();*/
            }
            else
            {
                /*
    System.Drawing.Drawing2D.GraphicsState g = PaintArgs.Graphics.Save();
    PaintArgs.Graphics.ResetTransform();
    PaintArgs.Graphics.RotateTransform(80);
    Cad.TranslateTransform(x,y,MatrixOrder.Append);
    Cad.DrawString("teste", fonte ,sol,x+200,y);*/

                using (System.Drawing.Font Fonte = new System.Drawing.Font("Arial", (float)(tamx * 100 / FPrincipal.precisaoPixel), System.Drawing.FontStyle.Regular))
                {
                   // Fonte = new System.Drawing.Font("Tahoma", (float)(tamx * 100 / Desenho.precisaoPixel), System.Drawing.FontStyle.Regular);
                //    System.Drawing.SizeF sz = Cad.VisibleClipBounds.Size;

                    Brush.Color = System.Drawing.Color.FromArgb(this.layer.Rgb[0], this.layer.Rgb[1], this.layer.Rgb[2]);
                    
                    EstadoCad = Cad.Save();

                    Cad.ResetTransform();

                    Cad.TranslateTransform(FPrincipal.pixelX(coord[3].X), FPrincipal.pixelY(coord[3].Y));
                    Cad.RotateTransform(-this.angulo);
           
                    //Cad.TranslateTransform(-Desenho.pixelX(x), -Desenho.pixelY(y), System.Drawing.Drawing2D.MatrixOrder.Append);

                    //  System.Drawing.SizeF size = Cad.MeasureString(texto, Fonte); // Get size of rotated text (bounding box)

                    Cad.DrawString(texto, Fonte, Brush, 0, 0);
                   
                    //Cad.RotateTransform(this.angulo);
                    Cad.TranslateTransform(-FPrincipal.pixelX(coord[3].X), -FPrincipal.pixelY(coord[3].Y));

                    // Cad.RotateTransform(-10, System.Drawing.Drawing2D.MatrixOrder.Append);
                    // Cad.TranslateTransform(-Desenho.pixelX(x), -Desenho.pixelY(y));


                    Cad.Restore(EstadoCad);
                    //    Cad.ResetTransform();
                }

               // Fonte.Dispose();
            }
// }
//     g2d.StrokeText(texto, Desenho.pixelX(x), Desenho.pixelY(y), tamx / Desenho.precisaoPixel, tamy / Desenho.precisaoPixel, this.layer.Rgb[0], this.layer.Rgb[1], this.layer.Rgb[2], angulo);
            
     if (base.Selecionado)
     {
        if (base.MostrarGrip)
            foreach (TGrip gr in base.Grips)
                gr.Desenha(ref Cad);
     };

//    Gl.glColor3f(this.layer.Rgb[0], this.layer.Rgb[1], this.layer.Rgb[2]);
//   float w = Glut.glutStrokeLength(Glut.GLUT_STROKE_MONO_ROMAN, texto);
//   w *= (float)tamx;

//    float h = Glut.glutStrokeHeight(Glut.GLUT_STROKE_MONO_ROMAN);
//    h *= -(float)tamy;



/*
Gl.glBegin(Gl.GL_LINES);
Gl.glVertex2d(Desenho.pixelX(x), Desenho.pixelY(y));
Gl.glVertex2d(Desenho.pixelX(x), Desenho.pixelY(y + h));
Gl.glEnd();*/
            
            /*
                        Gl.glBegin(Gl.GL_LINE_LOOP);
                        Gl.glVertex2d(Desenho.pixelX(base.linhas_poligonal[1].pIni.x), Desenho.pixelY(base.linhas_poligonal[1].pIni.y));
                        Gl.glVertex2d(Desenho.pixelX(base.linhas_poligonal[1].pFin.x), Desenho.pixelY(base.linhas_poligonal[1].pFin.y));
                        Gl.glEnd();

                        Gl.glBegin(Gl.GL_LINE_LOOP);
                        Gl.glVertex2d(Desenho.pixelX(base.linhas_poligonal[2].pIni.x), Desenho.pixelY(base.linhas_poligonal[2].pIni.y));
                        Gl.glVertex2d(Desenho.pixelX(base.linhas_poligonal[2].pFin.x), Desenho.pixelY(base.linhas_poligonal[2].pFin.y));
                        Gl.glEnd();

                        Gl.glBegin(Gl.GL_LINE_LOOP);
                        Gl.glVertex2d(Desenho.pixelX(base.linhas_poligonal[3].pIni.x), Desenho.pixelY(base.linhas_poligonal[3].pIni.y));
                        Gl.glVertex2d(Desenho.pixelX(base.linhas_poligonal[3].pFin.x), Desenho.pixelY(base.linhas_poligonal[3].pFin.y));
                        Gl.glEnd();*/

            }
            catch (Exception ms)
            {
                System.Windows.Forms.MessageBox.Show("erro ao desenhar texto: " + ms.Message);
            }
        }

        public override string PrimeiroComando()
        {
            return Const.CMD_TEXTO_1_P;
        }
        public override bool Selecionar(float clicx, float clicy, float coordx, float coordy, TObjetoDesenho Owner = null)
        {
            if (this.layer.Congelado || this.layer.Travado)
                return false;
    
            if (TipoObjeto == Const.ID_LAJE)
                return false;

            if (Retangulo.PontoEmPoligono(coordx, coordy))
            {
              SetaSelecao(true, true, false, Owner);
              return true;
            }

            return false;
        }

        public override void SetaSelecao(bool s, bool MostraGrip, bool SelecaoDeCandidato = false,TObjetoDesenho Owner = null)
        {
            if (this.layer == null)
                return;

            if (this.layer.Congelado || this.layer.Travado)
                return;

        //    if (TipoObjeto == Const.ID_LAJE)
       //         return; 

            base.Selecionado = s;
            base.MostrarGrip = MostraGrip;

            ShowHideGrips(MostraGrip);
        }

        public override void ShowHideGrips(bool visivel)
        {
            foreach (TGrip grip in Grips)
              grip.Visivel = visivel;
        }
        
        public override void AddGrips()
        {
            AddRetangulo();
                     
            Grips = new List<TGrip>();
            Grips.Add(new TGrip(this, this.x, this.y, true,false,false,false,this.layer));

            Grips.Add(new TGrip(this, coord2Retangulo.x, coord2Retangulo.y, false, true, false, false, this.layer));
            Grips.Add(new TGrip(this, coord3Retangulo.x, coord3Retangulo.y, false, false, true, false, this.layer));
        }


        public override TObjetoDesenho Clone()
        {
            TTexto l = new TTexto();
            l.Copy(this);
            return l;
        }

        public void Copy(TTexto obj)
        {
            base.Copy(obj);

            if ((Object)obj.pIni != null) pIni = (TPonto)obj.pIni.Clone();
            if ((Object)obj.pFin != null) pFin = (TPonto)obj.pFin.Clone();

            Selecionado = obj.Selecionado;
            Layer = obj.Layer;
            z = obj.z;
            texto  = obj.texto;
            x      = obj.x;
            y      = obj.y;
            y_giro = obj.y_giro;
            x_giro = obj.x_giro;

            tamx = obj.tamx;
            tamy = obj.tamy;

            angulo = obj.angulo;
            linhas_poligonal = new List<TLinha>();

            foreach(TLinha lin in obj.linhas_poligonal)
              linhas_poligonal.Add((TLinha)lin.Clone());
        
            coord = new CoordenadaD[5];
            for (int i = 0; i < 5; i++)
			{
    	       coord[i].X = obj.coord[i].X;
               coord[i].Y = obj.coord[i].Y;
			}
            Visivel = obj.Visivel;

            TipoObjeto = obj.TipoObjeto;
            Retangulo = obj.Retangulo;
            
            coord1Retangulo  = new vec3(obj.coord1Retangulo.x,obj.coord1Retangulo.y,obj.coord1Retangulo.z);
            coord2Retangulo  = new vec3(obj.coord2Retangulo.x,obj.coord2Retangulo.y,obj.coord2Retangulo.z);
            coord3Retangulo  = new vec3(obj.coord3Retangulo.x,obj.coord3Retangulo.y,obj.coord3Retangulo.z);
            coord4Retangulo  = new vec3(obj.coord4Retangulo.x,obj.coord4Retangulo.y,obj.coord4Retangulo.z);

            comp = obj.comp;
            altura    = obj.altura;
            dist1 = obj.dist1;
            dist2  = obj.dist2;
            Tipo = obj.Tipo;

            mPen = new System.Drawing.Pen(System.Drawing.Color.White);
            Brush = new System.Drawing.SolidBrush(System.Drawing.Color.White);

            Grips = new List<TGrip>();

            if (obj.Grips != null)
                foreach (TGrip g in obj.Grips)
                    Grips.Add(new TGrip(this, g.x, g.y, g.translacao, g.rotacao, g.escala, g.estica, g.layer));
        }
    }
}
