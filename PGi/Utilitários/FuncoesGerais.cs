using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Security.Cryptography;
using System.IO;
using System.Windows.Forms;
using System.Drawing;

namespace PG
{

    public class TransparentPanel : Panel
    {
        protected override CreateParams CreateParams
        {
            get
            {
                var cp = base.CreateParams;
                cp.ExStyle |= 0x00000020; // WS_EX_TRANSPARENT

                return cp;
            }
        }

        protected override void OnPaint(PaintEventArgs e) =>
            e.Graphics.FillRectangle(new SolidBrush(this.BackColor), this.ClientRectangle);
    }

    public class CustomPanel : System.Windows.Forms.Panel
    {
        private System.Windows.Forms.Panel panel;

        private System.Drawing.Color borderColor = System.Drawing.Color.MediumSlateBlue;
        private System.Drawing.Color borderFocusColor = System.Drawing.Color.Red;
        private int borderSize = 2;
        private bool underlinedStyle = false;
        private bool isFocused = false;

        private int borderRadius = 0;

       /* const int WS_EX_TRANSPARENT = 0x20;

        int opacity = 50;

        public int Opacity
        {
            get
            {
                return opacity;
            }
            set
            {
                if (value < 0 || value > 100) throw new ArgumentException("Value must be between 0 and 100");
                opacity = value;
            }
        }

        protected override CreateParams CreateParams
        {
            get
            {
                var cp = base.CreateParams;
                cp.ExStyle = cp.ExStyle | WS_EX_TRANSPARENT;

                return cp;
            }
        }

        protected override void OnPaint(PaintEventArgs e)
        {
            using (var b = new SolidBrush(Color.FromArgb(opacity * 255 / 100, BackColor)))
            {
                e.Graphics.FillRectangle(b, ClientRectangle);
            }

            base.OnPaint(e);
        }
        */
        public System.Drawing.Color BorderColor
        {
            get { return borderColor; }
            set
            {
                borderColor = value;
                this.Invalidate();
            }
        }

        public System.Drawing.Color BorderFocusColor
        {
            get { return borderFocusColor; }
            set { borderFocusColor = value; }
        }

        public int BorderSize
        {
            get { return borderSize; }
            set
            {
                if (value >= 1)
                {
                    borderSize = value;
                    this.Invalidate();
                }
            }
        }

        public bool UnderlinedStyle
        {
            get { return underlinedStyle; }
            set
            {
                underlinedStyle = value;
                this.Invalidate();
            }
        }

        public override System.Drawing.Color ForeColor
        {
            get { return base.ForeColor; }
            set
            {
                base.ForeColor = value;
                panel.ForeColor = value;
            }
        }


        public int BorderRadius
        {
            get { return borderRadius; }
            set
            {
                if (value >= 0)
                {
                    borderRadius = value;
                    this.Invalidate();//Redraw control
                }
            }
        }

        protected override void OnPaint(System.Windows.Forms.PaintEventArgs e)
        {
            base.OnPaint(e);
            System.Drawing.Graphics graph = e.Graphics;

            if (borderRadius > 1)//Rounded TextBox
            {
                //-Fields
                var rectBorderSmooth = this.ClientRectangle;
                var rectBorder = System.Drawing.Rectangle.Inflate(rectBorderSmooth, -borderSize, -borderSize);
                int smoothSize = borderSize > 0 ? borderSize : 1;
                graph.SmoothingMode = System.Drawing.Drawing2D.SmoothingMode.AntiAlias;
                using (System.Drawing.Drawing2D.GraphicsPath pathBorderSmooth = GetFigurePath(rectBorderSmooth, borderRadius))
                using (System.Drawing.Drawing2D.GraphicsPath pathBorder = GetFigurePath(rectBorder, borderRadius - borderSize))
                using (System.Drawing.Pen penBorderSmooth = new System.Drawing.Pen(System.Drawing.Color.FromArgb(64,64,64), smoothSize))
                using (System.Drawing.Pen penBorder = new System.Drawing.Pen(borderColor, borderSize))
                {
                    //-Drawing
                    this.Region = new System.Drawing.Region(pathBorderSmooth);//Set the rounded region of UserControl
                   
                    if (borderRadius > 15) 
                        SetTextBoxRoundedRegion();//Set the rounded region of TextBox component
                 
                    graph.SmoothingMode = System.Drawing.Drawing2D.SmoothingMode.AntiAlias;
                    penBorder.Alignment = System.Drawing.Drawing2D.PenAlignment.Center;
                   
                    if (isFocused) 
                        penBorder.Color = borderFocusColor;

                    if (underlinedStyle) //Line Style
                    {
                        //Draw border smoothing
                      // graph.DrawPath(penBorderSmooth, pathBorderSmooth);
                        //Draw border
                      //  graph.SmoothingMode = System.Drawing.Drawing2D.SmoothingMode.AntiAlias;
                        graph.DrawLine(penBorder, 0, this.Height + 5, this.Width, this.Height + 1);
                    }
                    else //Normal Style
                    {
                        //Draw border smoothing
                        graph.DrawPath(penBorderSmooth, pathBorderSmooth);
                        //Draw border
                        graph.DrawPath(penBorder, pathBorder);
                    }
                }
            }
        }
        private void SetTextBoxRoundedRegion()
        {
            System.Drawing.Drawing2D.GraphicsPath pathTxt;


            pathTxt = GetFigurePath(panel.ClientRectangle, borderSize * 2);
            panel.Region = new System.Drawing.Region(pathTxt);

            pathTxt.Dispose();
        }
        private System.Drawing.Drawing2D.GraphicsPath GetFigurePath(System.Drawing.Rectangle rect, int radius)
        {
            System.Drawing.Drawing2D.GraphicsPath path = new System.Drawing.Drawing2D.GraphicsPath();
            float curveSize = radius * 2.5f;

            path.StartFigure();
            path.AddArc(rect.X, rect.Y, curveSize, curveSize, 180, 90);

            path.AddArc(rect.Right - curveSize, rect.Y, curveSize, curveSize, 270, 90);

            path.AddArc(rect.Right - curveSize, rect.Bottom - curveSize, curveSize, curveSize, 0, 90);

            path.AddArc(rect.X, rect.Bottom - curveSize, curveSize, curveSize, 90, 90);

            path.CloseFigure();
            return path;
        }
    }

    [Serializable]
   public struct Coordenada
     {
        public float X, Y;
        public Coordenada(float X, float Y)
        {
            this.X = X;
            this.Y = Y;
        }
     }
   [Serializable]
   public struct CoordenadaD
    {
        public double X, Y, Z;
        public bool pontoEmRaio;

       
        public CoordenadaD(double X, double Y, double Z)
        {
            this.X = X;
            this.Y = Y;
            this.Z = Z;
            pontoEmRaio = false;
 
        }
        public CoordenadaD(double X, double Y, bool pt_raio)
        {
            this.X = X;
            this.Y = Y;
            this.Z = 0;
            pontoEmRaio = pt_raio;
        }
    }

    public static class FuncoesDesenho
     {

     } 

     public static class FuncoesGerais
     {
        static public float sind(double angle)
             {
                 return System.Convert.ToSingle(Math.Sin(angle * Math.PI / 180.0f) * Math.PI / 180.0f);
             }
        static public float atand(double angle)
             {
                 return System.Convert.ToSingle(Math.Atan(angle) / Math.PI * 180.0f);
             }
        static public float cosd(double angle)
             {
                 return System.Convert.ToSingle(Math.Cos(angle * Math.PI / 180.0f) * Math.PI / 180.0f);
             }
        static public float tand(double angle)
             {
                 return System.Convert.ToSingle(Math.Tan(angle * Math.PI / 180.0f) * Math.PI / 180.0f);
             }
        static public string FormataString(double value, string casasDecimais)//  casas decimais F0/F2 ..F3...F4
             {
                 return value.ToString(value % 1 == 0 ? "F0" : casasDecimais).Replace(",",".");
             }
        static public string FormataStringSeparadorMilhar(double value, string casasDecimais)
             {
                 return value.ToString(value % 1 == 0 ? "N0" : casasDecimais); //  casas decimais N0/N2 ..N3..N4
             }

        public static float Frac(float value)
        {
            return value - (float)Math.Truncate(value);
        }

        public static double Frac(double value)
        {
            return value - (double)Math.Truncate(value);
        }

        public static System.Drawing.Pen mPen = new System.Drawing.Pen(System.Drawing.Color.White);
     }

     public class Criptografia
     {
         /// <summary>     
         /// Vetor de bytes utilizados para a criptografia (Chave Externa)     
         /// </summary>     
         private static byte[] bIV = 
    { 0x50, 0x08, 0xF1, 0xDD, 0xDE, 0x3C, 0xF2, 0x18,
        0x44, 0x74, 0x19, 0x2C, 0x53, 0x49, 0xAB, 0xBC };

         /// <summary>     
         /// Representação de valor em base 64 (Chave Interna)    
         /// O Valor representa a transformação para base64 de     
         /// um conjunto de 32 caracteres (8 * 32 = 256bits)    
         /// A chave é: "Criptografias com Rijndael / AES"     
         /// </summary>     
         private const string cryptoKey =
             "J3JpcHRvZ3JhZmlhcyBjb20gUlluamRhZWwgLyBBR12=";

         /// <summary>     
         /// Metodo de criptografia de valor     
         /// </summary>     
         /// <param name="text">valor a ser criptografado</param>     
         /// <returns>valor criptografado</returns>
         public static string Encrypt(string text)
         {
             try
             {
                 // Se a string não está vazia, executa a criptografia
                 if (!string.IsNullOrEmpty(text))
                 {
                     // Cria instancias de vetores de bytes com as chaves                
                     byte[] bKey = Convert.FromBase64String(cryptoKey);
                     byte[] bText = new UTF8Encoding().GetBytes(text);

                     // Instancia a classe de criptografia Rijndael
                     Rijndael rijndael = new RijndaelManaged();

                     // Define o tamanho da chave "256 = 8 * 32"                
                     // Lembre-se: chaves possíves:                
                     // 128 (16 caracteres), 192 (24 caracteres) e 256 (32 caracteres)                
                     rijndael.KeySize = 256;

                     // Cria o espaço de memória para guardar o valor criptografado:                
                     MemoryStream mStream = new MemoryStream();
                     // Instancia o encriptador                 
                     CryptoStream encryptor = new CryptoStream(
                         mStream,
                         rijndael.CreateEncryptor(bKey, bIV),
                         CryptoStreamMode.Write);

                     // Faz a escrita dos dados criptografados no espaço de memória
                     encryptor.Write(bText, 0, bText.Length);
                     // Despeja toda a memória.                
                     encryptor.FlushFinalBlock();
                     // Pega o vetor de bytes da memória e gera a string criptografada                
                     return Convert.ToBase64String(mStream.ToArray());
                 }
                 else
                 {
                     // Se a string for vazia retorna nulo                
                     return null;
                 }
             }
             catch (Exception ex)
             {
                 // Se algum erro ocorrer, dispara a exceção            
                 throw new ApplicationException("Erro ao criptografar", ex);
             }
         }

         /// <summary>     
         /// Pega um valor previamente criptografado e retorna o valor inicial 
         /// </summary>     
         /// <param name="text">texto criptografado</param>     
         /// <returns>valor descriptografado</returns>     
         public static string Decrypt(string text)
         {
             try
             {
                 // Se a string não está vazia, executa a criptografia           
                 if (!string.IsNullOrEmpty(text))
                 {
                     // Cria instancias de vetores de bytes com as chaves                
                     byte[] bKey = Convert.FromBase64String(cryptoKey);
                     byte[] bText = Convert.FromBase64String(text);

                     // Instancia a classe de criptografia Rijndael                
                     Rijndael rijndael = new RijndaelManaged();

                     // Define o tamanho da chave "256 = 8 * 32"                
                     // Lembre-se: chaves possíves:                
                     // 128 (16 caracteres), 192 (24 caracteres) e 256 (32 caracteres)                
                     rijndael.KeySize = 256;

                     // Cria o espaço de memória para guardar o valor DEScriptografado:               
                     MemoryStream mStream = new MemoryStream();

                     // Instancia o Decriptador                 
                     CryptoStream decryptor = new CryptoStream(
                         mStream,
                         rijndael.CreateDecryptor(bKey, bIV),
                         CryptoStreamMode.Write);

                     // Faz a escrita dos dados criptografados no espaço de memória   
                     decryptor.Write(bText, 0, bText.Length);
                     // Despeja toda a memória.                
                     decryptor.FlushFinalBlock();
                     // Instancia a classe de codificação para que a string venha de forma correta         
                     UTF8Encoding utf8 = new UTF8Encoding();
                     // Com o vetor de bytes da memória, gera a string descritografada em UTF8       
                     return utf8.GetString(mStream.ToArray());
                 }
                 else
                 {
                     // Se a string for vazia retorna nulo                
                     return null;
                 }
             }
             catch (Exception ex)
             {
                 // Se algum erro ocorrer, dispara a exceção            
                 throw new ApplicationException("Erro ao descriptografar", ex);
             }
         }
     }
    
}
