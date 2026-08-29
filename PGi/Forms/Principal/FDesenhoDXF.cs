using System;
using System.IO;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace PG
{
    public partial class FPrincipal
    {

        private FileInfo theSourceFile;
        private double XMax, XMin;
        private double YMax, YMin;

        private double scaleX = 1;
        private double scaleY = 1;
        private double mainScale = 1;
        
        public void LerDXF(string arquivo)
        {
            string line1, line2;							//these line1 and line2 is used for getting the a/m data groups...

            line1 = "0";									//line1 and line2 are are initialized here...
            line2 = "0";

            long position = 0;

            theSourceFile = new FileInfo(arquivo);		//the sourceFile is set.

            StreamReader reader = null;						//a reader is prepared...

            reader = theSourceFile.OpenText();			//the reader is set ...

            do
            {
                ////////////////////////////////////////////////////////////////////
                //This part interpretes the drawing objects found in the DXF file...
                ////////////////////////////////////////////////////////////////////
              //  if (line1.Trim() != "")
                {
                    if (line1.Contains("$LAYER$"))
                      LayersModule(reader);
                    else
                    if (line1.Contains("name=LINE"))
                      LineModule(reader);
                    else
                    if (line1.Contains("name=POLYLINE") || (line1.Contains("name=LWPOLYLINE")))
                      PolylineModule(reader);
                   /* else
                    if (line1.Contains("name=TEXT"))
                      TextModule(reader);*/
                  //  else
                 //   if (line1.Contains("name=CIRCLE"))
                   //   CircleModule(reader);

                    /*
                                                    else if (line1 == "0" && line2 == "ARC")
                                                       ArcModule(reader);*/

                    ////////////////////////////////////////////////////////////////////
                    ////////////////////////////////////////////////////////////////////


                    PegaProximaLinha(reader, out line1);		//the related method is called for iterating through the text file and assigning values to line1 and line2...
                }
            }
            while (line1 != "EOF");



            reader.DiscardBufferedData();							//reader is cleared...
            theSourceFile = null;
 
            reader.Close();											//...and closed.
            reader.Dispose();

            AtualizarPixels();

        }
        List<TPonto> pointList = new List<TPonto>();

        private void LineModule(StreamReader reader)		//Interpretes line objects in the DXF file
        {
            
            string line1, line2;
            line1 = "0";
            line2 = "0";
            string lay = "";

            double x1 = 0;
            double y1 = 0;
            double x2 = 0;
            double y2 = 0;
            string valor;
            try
            {
                PegaProximaLinha(reader, out line1);

                int indiceInicial = line1.IndexOf("X") + 2;
                int indiceFinal = line1.IndexOf("Y") - 3;

                valor = line1.Substring(indiceInicial, indiceFinal).Trim();
                x1 = Convert.ToDouble(valor);

                indiceInicial = line1.IndexOf("Y") + 2;
                indiceFinal = line1.IndexOf("Z");

                valor = line1.Substring(indiceInicial, indiceFinal - indiceInicial).Trim();
                y1 = Convert.ToDouble(valor);

                /* ============================== */

                PegaProximaLinha(reader, out line1);

                indiceInicial = line1.IndexOf("X") + 2;
                indiceFinal = line1.IndexOf("Y") - 3;

                valor = line1.Substring(indiceInicial, indiceFinal).Trim();
                x2 = Convert.ToDouble(valor);

                indiceInicial = line1.IndexOf("Y") + 2;
                indiceFinal = line1.IndexOf("Z");

                valor = line1.Substring(indiceInicial, indiceFinal - indiceInicial).Trim();
                y2 = Convert.ToDouble(valor);

                PegaProximaLinha(reader, out line1);
                lay = line1;
            }
            catch (Exception m)
            {
                MessageBox.Show("Um erro ocorreu na leitura das linhas do arquivo DXF." + m.Message);
                return;
            }
            ///////////////////////////////////////////////////////////////////////////////////////////////////////
            ///////////////////////////////////////////////////////////////////////////////////////////////////////
            try
            {

                TPonto pini = new TPonto(x1 * 1, y1 * 1, pixelX((float)x1 * 1), pixelY((float)y1 * 1), 0, -1);
                TPonto pfin = new TPonto(x2 * 1, y2 * 1, pixelX((float)x2 * 1), pixelY((float)y2 * 1), 0, -1);

                pini.px_x = pixelX(pini.x);
                pini.px_y = pixelX(pini.y);

                pfin.px_x = pixelX(pfin.x);
                pfin.px_y = pixelX(pfin.y);

                TLinha lin = new TLinha(pini, pfin, -1);
                lin.layer = Estrutura.LayersByIdArquitetura[lay];

                AddPoint(ref pini);
                AddPoint(ref pfin);
                AddLinha(lin, PavimentoAtual, true, false, false, null);
                lin.AddGrips();

                AddGrip(lin.Grips[0], PavimentoAtual, Estrutura.LayersByIdArquitetura);
                AddGrip(lin.Grips[1], PavimentoAtual, Estrutura.LayersByIdArquitetura);
                AddGrip(lin.Grips[2], PavimentoAtual, Estrutura.LayersByIdArquitetura);
            }
            catch (Exception m)
            {
                MessageBox.Show("Um erro ocorreu na leitura das linhas do arquivo DXF." + lay +"  "+ m.Message);
                return;
            }
        }
        
        private void LayersModule(StreamReader reader)		//Interpretes line objects in the DXF file
        {
            string line1,s, nome = "";
            byte r, g, b;
            bool ligado, congelado,travado;

            PegaProximaLinha(reader, out line1);
            nome = line1;

            PegaProximaLinha(reader, out line1);
            r = Convert.ToByte(line1);

            PegaProximaLinha(reader, out line1);
            g = Convert.ToByte(line1);

            PegaProximaLinha(reader, out line1);
            b = Convert.ToByte(line1);

            PegaProximaLinha(reader, out line1);
            s = line1.Trim();

            congelado = (s == "S" ? true: false);
            ligado    = !congelado;
            travado   = false;

            TLayer l;
            try
            {
                foreach (TPavimento pav in gerenciador.Pavimentos)
                {
                    l = new TLayer(nome, ligado, congelado, new byte[3] { r, g, b }, -1, travado, GrupoLay.Arquitetura);
                    pav.LayersByIdArquitetura[l.nome] = l;
                    pav.layers.Add(l);
                }
            }
            catch (Exception m)
            {
                MessageBox.Show("Um erro ocorreu na leitura do arquivo DXF." + m.Message);
                return;
            }
        }
        
        private void CircleModule(StreamReader reader)		//Interpretes circle objects in the DXF file
        {
            string line1, line2;
            line1 = "0";
            line2 = "0";

            double x1 = 0;
            double y1 = 0;

            double radius = 0;

            do
            {
                GetLineCouple(reader, out line1, out line2);

                if (line1 == "10")
                {
                    x1 = Convert.ToDouble(line2);

                }


                if (line1 == "20")
                {
                    y1 = Convert.ToDouble(line2);

                }


                if (line1 == "40")
                {
                    radius = Convert.ToDouble(line2);

                    if ((x1 + radius) > XMax)
                        XMax = x1 + radius;

                    if ((x1 - radius) < XMin)
                        XMin = x1 - radius;

                    if (y1 + radius > YMax)
                        YMax = y1 + radius;

                    if ((y1 - radius) < YMin)
                        YMin = y1 - radius;

                }



            }
            while (line1 != "40");

            //****************************************************************************************************//
            //***************This Part is related with the drawing editor...the data taken from the dxf file******//
            //***************is interpreted hereinafter***********************************************************//


           /* if ((Math.Abs(XMax) - Math.Abs(XMin)) > this.pictureBox1.Size.Width)
            {
                scaleX = (double)(this.pictureBox1.Size.Width) / (double)(Math.Abs(XMax) - Math.Abs(XMin));
            }
            else
                scaleX = 1;


            if ((Math.Abs(YMax) - Math.Abs(YMin)) > this.pictureBox1.Size.Height)
            {
                scaleY = (double)(this.pictureBox1.Size.Height) / (double)(Math.Abs(YMax) - Math.Abs(YMin));
            }
            else
                scaleY = 1;

            mainScale = Math.Min(scaleX, scaleY);


            int ix = drawingList.Add(new circle(new Point((int)x1, (int)-y1), radius, Color.White, Color.Red, 1));
            objectIdentifier.Add(new DrawingObject(4, ix));*/

            //////////////////////////////////////////////////////////////////////////////////////////////////////
            //////////////////////////////////////////////////////////////////////////////////////////////////////

            TCirculo obj = new TCirculo(x1, y1, radius, Estrutura.LayersByIdPrincipal[Lay.ElementosBasicos]);

            AddCirculo(ref obj, PavimentoAtual, Estrutura.LayersByIdPrincipal[Lay.ElementosBasicos]);
        }
        
        
        private void TextModule(StreamReader reader)		//Interpretes line objects in the DXF file
        {
            string line1, lay;
            string texto = "";
            line1 = "0";

            double x1 = 0;
            double y1 = 0;
            double z1 = 0;
            double altura = 0;
            double angulo = 0;
            string valor;
            int indiceInicial, indiceFinal;
            
            PegaProximaLinha(reader, out line1);
     
            indiceInicial = line1.IndexOf("X") + 2;
            indiceFinal = line1.IndexOf("Y") - 3;
            valor = line1.Substring(indiceInicial, indiceFinal).Trim();
            x1 = Convert.ToDouble(valor);

            indiceInicial = line1.IndexOf("Y") + 2;
            indiceFinal = line1.IndexOf("Z");
            valor = line1.Substring(indiceInicial, indiceFinal - indiceInicial).Trim();
            y1 = Convert.ToDouble(valor);

            indiceInicial = line1.IndexOf("Z") + 2;
            indiceFinal = line1.IndexOf("A");
            valor = line1.Substring(indiceInicial, indiceFinal - indiceInicial).Trim();
            z1 = Convert.ToDouble(valor);

            indiceInicial = line1.IndexOf("A") + 2;
            indiceFinal = line1.Length;
            valor = line1.Substring(indiceInicial, indiceFinal - indiceInicial).Trim();
            angulo = Convert.ToDouble(valor);

/*
X=267.297168713855 Y=3465.41617853434 Z=0 A=0.0057295779697597
Arial; 
H=-11; 
C=$2FFFFFFF; 
Style:
Thickness=-1
Generation=0
VAlign=4
HAlign=0
T=60 */

            PegaProximaLinha(reader, out line1);
            PegaProximaLinha(reader, out line1);

            indiceInicial = line1.IndexOf("H") + 2;
            indiceFinal = line1.IndexOf(";") ;
            valor = line1.Substring(indiceInicial, indiceFinal - indiceInicial).Trim();
            altura = Math.Abs(Convert.ToDouble(valor)) * 3;

            PegaProximaLinha(reader, out line1);
            PegaProximaLinha(reader, out line1);
            PegaProximaLinha(reader, out line1);
        //    PegaProximaLinha(reader, out line1);
       //     PegaProximaLinha(reader, out line1);
       //     PegaProximaLinha(reader, out line1);

            PegaProximaLinha(reader, out line1);
            indiceInicial = line1.IndexOf("T") + 2;
            indiceFinal = line1.Length;
            valor = line1.Substring(indiceInicial, indiceFinal - indiceInicial).Trim();
            texto = valor;
 //           if (texto.Contains("Ç"))
   //             MessageBox.Show(texto);
            PegaProximaLinha(reader, out line1);
            lay = line1;

            double tam = altura / 64;


            TTexto t = new TTexto(texto, x1, y1, tam, -tam, 255, 255, 255, (float)angulo, Estrutura.LayersByIdArquitetura[lay]);

            AddGrip(t.Grips[0], PavimentoAtual, Estrutura.LayersByIdArquitetura);
            AddGrip(t.Grips[1], PavimentoAtual, Estrutura.LayersByIdArquitetura);
            AddGrip(t.Grips[2], PavimentoAtual, Estrutura.LayersByIdArquitetura);

            AddTexto(ref t, PavimentoAtual, Estrutura.LayersByIdArquitetura);
        }
        
        private void PolylineModule(StreamReader reader)	//Interpretes polyline objects in the DXF file
        {
            string line1, lay = "";
            line1 = "0";

            double x1 = 0;
            double y1 = 0;

          //  thePolyLine = new polyline(Color.White, 1);

         //   int ix = drawingList.Add(thePolyLine);
         //   objectIdentifier.Add(new DrawingObject(5, ix));

            int counter = 0;
            int numberOfVertices = 0;
            int openOrClosed = 0;
            string valor;
       //     ArrayList pointList = new ArrayList();
            int indiceInicial, indiceFinal;
            pointList.Clear();
            do
            {
                PegaProximaLinha(reader, out line1);
                if (!line1.Contains("L="))
                {
                    numberOfVertices++;

                    indiceInicial = line1.IndexOf("X") + 2;
                    indiceFinal = line1.IndexOf("Y") - 3;

                    valor = line1.Substring(indiceInicial, indiceFinal).Trim();
                    x1 = Convert.ToDouble(valor);

                    indiceInicial = line1.IndexOf("Y") + 2;
                    indiceFinal = line1.IndexOf("Z");

                    valor = line1.Substring(indiceInicial, indiceFinal - indiceInicial).Trim();
                    y1 = Convert.ToDouble(valor);

                    pointList.Add(new TPonto(x1, y1,0, pixelX((float)x1), pixelY((float)y1), -1));
               }
             //   else
              //  {
              //      indiceInicial = line1.IndexOf("L") + 2;
              //      indiceFinal =  line1.Length;
              //      lay = line1.Substring(indiceInicial, indiceFinal - indiceInicial);
              //  }

            }
            while (!line1.Contains("L="));

            indiceInicial = line1.IndexOf("L") + 2;
            indiceFinal = line1.Length;
            lay = line1.Substring(indiceInicial, indiceFinal - indiceInicial);

            //*************************************************************************//


            for (int i = 1; i < numberOfVertices; i++)
            {
                TPonto pini = pointList[i - 1];
                TPonto pfin = pointList[i];
            
                pini.px_x = pixelX(pini.x);
                pini.px_y = pixelX(pini.y);

                pfin.px_x = pixelX(pfin.x);
                pfin.px_y = pixelX(pfin.y);

                TLinha lin = new TLinha(pini, pfin, -1);
                lin.layer = Estrutura.LayersByIdArquitetura[lay];

                AddPoint(ref pini);
                AddPoint(ref pfin);
                AddLinha(lin, PavimentoAtual, true, false, false, Estrutura.LayersByIdArquitetura);
                lin.AddGrips();

                AddGrip(lin.Grips[0], PavimentoAtual, Estrutura.LayersByIdArquitetura);
                AddGrip(lin.Grips[1], PavimentoAtual, Estrutura.LayersByIdArquitetura);
                AddGrip(lin.Grips[2], PavimentoAtual, Estrutura.LayersByIdArquitetura);
            }

        }

        private void PegaProximaLinha(StreamReader theReader, out string line1)		//this method is used to iterate through the text file and assign values to line1 and line2
        {
            System.Globalization.CultureInfo ci = System.Threading.Thread.CurrentThread.CurrentCulture;
            string decimalSeparator             = ci.NumberFormat.CurrencyDecimalSeparator;

            line1 = "";

            if (theReader == null)
                return;

            line1 = theReader.ReadLine();
            if (line1 != null)
            {
                line1 = line1.Trim();
                line1 = line1.Replace('.', decimalSeparator[0]);

            }
        }

        private void GetLineCouple(StreamReader theReader, out string line1, out string line2)		//this method is used to iterate through the text file and assign values to line1 and line2
        {
            System.Globalization.CultureInfo ci = System.Threading.Thread.CurrentThread.CurrentCulture;
            string decimalSeparator = ci.NumberFormat.CurrencyDecimalSeparator;

            line1 = line2 = "";

            if (theReader == null)
                return;

            line1 = theReader.ReadLine();
            if (line1 != null)
            {
                line1 = line1.Trim();
                line1 = line1.Replace('.', decimalSeparator[0]);

            }
            line2 = theReader.ReadLine();
            if (line2 != null)
            {
                line2 = line2.Trim();
                line2 = line2.Replace('.', decimalSeparator[0]);
            }
        }

    }
}
