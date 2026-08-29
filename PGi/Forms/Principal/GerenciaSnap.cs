using OpenTK;
using OpenTK.Graphics.OpenGL;
using OpenTK.Input;
using OpenTK.Platform;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Net.NetworkInformation;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Windows.Media.Animation;

namespace PG
{
    public partial class FPrincipal
    {
        Ponto A, B, C, D, F, G;
        double cx, cy, L, cz, PontoProximo_coordX, PontoProximo_coordY;
        Ponto P2;
        float ang, co, ca;
        bool AchouPontoProximo;
        double DifCoordX, DifCoordY;
        int cota_i_x, cota_i_y, cota_f_x, cota_f_y;
        private RibbonTab ribbonTab1;
        private RibbonPanel ribbonPanel1;

        private void CapturaGrip()
        {
            for (i = 0; i < Grips.Count; i++)
            {
                Grips[i].esta_pintado = false;

                if (Grips[i].Visivel)
                    if ((Posicao.X >= (pixelX(Grips[i].x) - 15)) && (Posicao.X <= (pixelX(Grips[i].x) + 15)))
                        if ((Posicao.Y >= (pixelY(Grips[i].y) - 15)) && (Posicao.Y <= (pixelY(Grips[i].y)) + 15))
                            Grips[i].esta_pintado = true;
            }
        }

        private bool ehGrip(ref TObjetoDesenho od)
        {
            for (i = 0; i < Grips.Count; i++)
            {
                if (Grips[i].esta_pintado)
                    if ((Posicao.X >= (pixelX(Grips[i].x) - 15)) && (Posicao.X <= (pixelX(Grips[i].x) + 15)))
                        if ((Posicao.Y >= (pixelY(Grips[i].y) - 15)) && (Posicao.Y <= (pixelY(Grips[i].y)) + 15))
                        {
                            od = Grips[i];
                            Grips[i].esta_pintado = false;
                            return true;
                        }
            }
            return false;
        }

        private void ToolTip(float x, float y)
        {

        }

        double novoX, novoY, anguloLinha, angleNewObject;
        double interx = 0, intery = 0, interz = 0;
        int k;
        public static double[] px_x1 = new double[1];
        public static double[] px_y1 = new double[1];
        public static double[] px_x2 = new double[1];
        public static double[] px_y2 = new double[1];
        double[] i_x = new double[1];
        double[] i_y = new double[1];
        double[] i_z = new double[1];
        static double Z_Clip = 0;
        public static Matrix4d View_x_Proj, view, proj;
        public static void pixel1_Clipped(ref double x, ref double y, ref double z, ref double Z_Clip)
        {       
            Vector4d p1NDC = new Vector4d(x, y, z, 1);
            Vector4d clip = RMath.Multiply(p1NDC, View_x_Proj);

            p1NDC = RMath.Multiply(p1NDC, View_x_Proj);
            p1NDC /= p1NDC.W;
        }

        public static Vector4d WorldToClip(Vector3d p)
        {
            View_x_Proj = RMath.Multiply(view, proj);

            Vector4d v = new Vector4d(p.X, p.Y, p.Z, 1.0);

            return RMath.Multiply(v, View_x_Proj);
        }

        public static Vector3d ClipToNDC(Vector4d clip)
        {
            return new Vector3d(
                clip.X / clip.W,
                clip.Y / clip.W,
                clip.Z / clip.W);
        }

        public static bool DentroFrustum(Vector4d clip)
        {
            return
                clip.X >= -clip.W && clip.X <= clip.W &&
                clip.Y >= -clip.W && clip.Y <= clip.W &&
                clip.Z >= -clip.W && clip.Z <= clip.W;
        }

        public static Vector3d IntersecaoFrustum(Vector3d fora, Vector3d dentro)
        {
            Vector3d p0 = fora;
            Vector3d p1 = dentro;

            for (int i = 0; i < 25; i++)
            {
                Vector3d meio = (p0 + p1) * 0.5;

                if (DentroFrustum(WorldToClip(meio)))
                    p1 = meio;
                else
                    p0 = meio;
            }

            return p1;
        }

        public static void pixel1(ref double x, ref double y, ref double z)
        {
            Project(ref px_x1, ref px_y1, ref x, ref y, ref z, ref Mvm_Principal, ref pm_Principal, ref ViewPortPrincipal);
        }
        public static void pixel1(ref double x, ref double y, ref double z, ref double Z_Clip)
        {
            Project(ref px_x1, ref px_y1, ref x, ref y, ref z, ref Mvm_Principal, ref pm_Principal, ref ViewPortPrincipal, ref Z_Clip);
        }
        public static void pixel1(double x, double y, double z)
        {
            Project(ref px_x1, ref px_y1, ref x, ref y, ref z, ref Mvm_Principal, ref pm_Principal, ref ViewPortPrincipal);
        }
        public static void pixel2(ref double x, ref double y, ref double z, ref double Z_Clip)
        {
            Project(ref px_x2, ref px_y2, ref x, ref y, ref z, ref Mvm_Principal, ref pm_Principal, ref ViewPortPrincipal, ref Z_Clip);
        }
        public static void pixel2(ref double x, ref double y, ref double z)
        {
            Project(ref px_x2, ref px_y2, ref x, ref y, ref z, ref Mvm_Principal, ref pm_Principal, ref ViewPortPrincipal);
        }
        public bool snapEnd, snapMiddle, snapNearest, snapIntersec, snapPerpendicular;
        int hh;
        TTrechoViga trechoSnap;
        double z_pixel = 0;
        double z_pixel2 = 0;
        TLinha LinhaSnapNearest, t;
        bool ForaDaTela;

        private void ObtemSnap()
        {
            try
            {

                // if (GripAtual != null)
                //   if (GripAtual.translacao) return;
                snapEnd = false;
                snapMiddle = false;
                snapNearest = false;
                snapIntersec = false;
                snapPerpendicular = false;
                if (rodando || FazendoZoom || Panning)
                    return;

                if (snap_PontoFinal)
                {
                    if (ModelagemEmPlano)
                    {
                        double min_z = 99999;
                        for (hh = 0; hh < max_snap_extremo + 1; hh++)
                        {
                            interx = Pontos_SnapPontoExtremo[hh].x;
                            intery = Pontos_SnapPontoExtremo[hh].y;
                            interz = Pontos_SnapPontoExtremo[hh].z;
                            pixel1(ref interx, ref intery, ref interz, ref z_pixel);

                            ForaDaTela = ((px_x1[0] < 0)) ||
                                         ((px_x1[0] > w)) ||
                                         ((px_y1[0] < 0)) ||
                                         ((px_y1[0] > h));
                            if (ForaDaTela)
                                continue;

                            if (z_pixel < 1 && z_pixel > 0)
                                if ((mouseX >= (px_x1[0] - 10)) && (mouseX <= (px_x1[0] + 10)))
                                {
                                    if ((mouseY >= (px_y1[0] - 10)) && (mouseY <= (px_y1[0] + 10)))
                                    {
                                        if (z_pixel < min_z)
                                        {
                                            AtualizaPontoSnap(ref interx, ref intery, ref interz, ref px_x1[0], ref px_y1[0]);
                                            snapEnd = true;
                                            min_z = z_pixel;    
                                        }
                                    }
                                }
                        }

                        if (snapEnd) return;
                    }
                    else
                    {
                        for (hh = 0; hh < max_snap_extremo + 1; hh++)
                        {
                            interx = Pontos_SnapPontoExtremo[hh].x;
                            intery = Pontos_SnapPontoExtremo[hh].y;
                            interz = Pontos_SnapPontoExtremo[hh].z;
                            pixel1(ref interx, ref intery, ref interz, ref z_pixel);

                            ForaDaTela = ((px_x1[0] < 0)) ||
                                         ((px_x1[0] > w)) ||
                                         ((px_y1[0] < 0)) ||
                                         ((px_y1[0] > h));
                            if (ForaDaTela)
                                continue;

                            if (z_pixel < 1 && z_pixel > 0)
                                if ((mouseX >= (px_x1[0] - 10)) && (mouseX <= (px_x1[0] + 10)))
                                {
                                    if ((mouseY >= (px_y1[0] - 10)) && (mouseY <= (px_y1[0] + 10)))
                                    {
                                        AtualizaPontoSnap(ref interx, ref intery, ref interz, ref px_x1[0], ref px_y1[0]);
                                        snapEnd = true;
                                        return;
                                    }
                                }
                        }
                    }
                }

                if (snap_Perpendicular)
                {
                    if (ModelagemEmPlano)
                    {
                        double min_z = 99999;
                        for (hh = 0; hh < max_PerpBarras + 1; hh++)
                        {
                            interx = PerpBarras[hh].ponto.x;
                            intery = PerpBarras[hh].ponto.y;
                            interz = PerpBarras[hh].ponto.z;
                            pixel1(ref interx, ref intery, ref interz, ref z_pixel);

                            ForaDaTela = ((px_x1[0] < 0)) ||
                                         ((px_x1[0] > w)) ||
                                         ((px_y1[0] < 0)) ||
                                        ((px_y1[0] > h));
                            if (ForaDaTela)
                                continue;

                            if (z_pixel < 1 && z_pixel > 0)
                                if ((mouseX >= (px_x1[0] - 10)) && (mouseX <= (px_x1[0] + 10)))
                                {
                                    if ((mouseY >= (px_y1[0] - 10)) && (mouseY <= (px_y1[0] + 10)))
                                    {
                                        if (z_pixel < min_z)
                                        {
                                            AtualizaPontoSnap(ref interx, ref intery, ref interz, ref px_x1[0], ref px_y1[0]);
                                            snapPerpendicular = true; 
                                            min_z = z_pixel;
                                            LinhaSnapNearest = PerpBarras[hh].linha_nearest;
                                        }
                                    }
                                }
                        }

                        if (snapPerpendicular) return;
                    }
                    else
                    {
                        for (hh = 0; hh < max_PerpBarras + 1; hh++)
                        {
                            interx = PerpBarras[hh].ponto.x;
                            intery = PerpBarras[hh].ponto.y;
                            interz = PerpBarras[hh].ponto.z;
                            pixel1(ref interx, ref intery, ref interz, ref z_pixel);

                            ForaDaTela = ((px_x1[0] < 0)) ||
                                         ((px_x1[0] > w)) ||
                                         ((px_y1[0] < 0)) ||
                                        ((px_y1[0] > h));
                            if (ForaDaTela)
                                continue;

                            if (z_pixel < 1 && z_pixel > 0)
                                if ((mouseX >= (px_x1[0] - 10)) && (mouseX <= (px_x1[0] + 10)))
                                {
                                    if ((mouseY >= (px_y1[0] - 10)) && (mouseY <= (px_y1[0] + 10)))
                                    {
                                        AtualizaPontoSnap(ref interx, ref intery, ref interz, ref px_x1[0], ref px_y1[0]);
                                        snapPerpendicular = true;
                                        LinhaSnapNearest = PerpBarras[hh].linha_nearest;

                                        return;
                                    }
                                }
                        }
                    }
                }

                if (snap_Intersecao)
                {
                    for (hh = 0; hh < Pontos_SnapIntersecao.Count; hh++)
                    {
                        interx = Pontos_SnapIntersecao[hh].x;
                        intery = Pontos_SnapIntersecao[hh].y;
                        interz = Pontos_SnapIntersecao[hh].z;
                        pixel1(ref interx, ref intery, ref interz, ref z_pixel);

                        ForaDaTela = ((px_x1[0] < 0)) ||
                                     ((px_x1[0] > w)) ||
                                     ((px_y1[0] < 0)) ||
                                    ((px_y1[0] > h));
                        if (ForaDaTela)
                            continue;

                        if (z_pixel < 1 && z_pixel > 0)
                            if ((mouseX >= (px_x1[0] - 10)) && (mouseX <= (px_x1[0] + 10)))
                            {
                                if ((mouseY >= (px_y1[0] - 10)) && (mouseY <= (px_y1[0] + 10)))
                                {
                                    /*  foreach (IntersecBarras item in IntersecoesBarras)
                                      {
                                          if ((Geom.Iguais(item.ponto.x, interx) && Geom.Iguais(item.ponto.y, intery) && Geom.Iguais(item.ponto.z, interz)))
                                          {
                                              item.b1.pinta = true;
                                              item.b2.pinta = true;
                                          }
                                      }*/

                                    AtualizaPontoSnap(ref interx, ref intery, ref interz, ref px_x1[0], ref px_y1[0]);
                                    snapIntersec = true;
                                    return;
                                }
                            }
                    }
                }

                if (snap_PontoMeio)
                {
                    if (ModelagemEmPlano)
                    {
                        double min_z = 99999;
                        for (hh = 0; hh < s_PontosMedio.Count; hh++)
                        {
                            interx = s_PontosMedio[hh].ponto.x;
                            intery = s_PontosMedio[hh].ponto.y;
                            interz = s_PontosMedio[hh].ponto.z;
                            pixel1(ref interx, ref intery, ref interz, ref z_pixel);

                            ForaDaTela = ((px_x1[0] < 0)) ||
                                         ((px_x1[0] > w)) ||
                                         ((px_y1[0] < 0)) ||
                                         ((px_y1[0] > h));

                            if (ForaDaTela)
                                continue;

                            if (z_pixel < 1 && z_pixel > 0)
                                if ((mouseX >= (px_x1[0] - 10)) && (mouseX <= (px_x1[0] + 10)))
                                {
                                    if ((mouseY >= (px_y1[0] - 10)) && (mouseY <= (px_y1[0] + 10)))
                                    {
                                        if (z_pixel < min_z)
                                        {
                                            AtualizaPontoSnap(ref interx, ref intery, ref interz, ref px_x1[0], ref px_y1[0]);
                                            snapMiddle = true;
                                            min_z = z_pixel;
                                            LinhaSnapNearest = s_PontosMedio[hh].linha;
                                        }
                                    }
                                }
                        }

                        if (snapMiddle) return;
                    }
                    else
                    {
                        for (hh = 0; hh < s_PontosMedio.Count; hh++)
                        {
                            interx = s_PontosMedio[hh].ponto.x;
                            intery = s_PontosMedio[hh].ponto.y;
                            interz = s_PontosMedio[hh].ponto.z;
                            pixel1(ref interx, ref intery, ref interz, ref z_pixel);

                            ForaDaTela = ((px_x1[0] < 0)) ||
                                         ((px_x1[0] > w)) ||
                                         ((px_y1[0] < 0)) ||
                                         ((px_y1[0] > h));

                            if (ForaDaTela)
                                continue;

                            if (z_pixel < 1 && z_pixel > 0)
                                if ((mouseX >= (px_x1[0] - 10)) && (mouseX <= (px_x1[0] + 10)))
                                {
                                    if ((mouseY >= (px_y1[0] - 10)) && (mouseY <= (px_y1[0] + 10)))
                                    {
                                        AtualizaPontoSnap(ref interx, ref intery, ref interz, ref px_x1[0], ref px_y1[0]);
                                        LinhaSnapNearest = s_PontosMedio[hh].linha;
                                        snapMiddle = true;
                                        return;
                                    }
                                }
                        }
                    }
                }

                if (InsercaoPorCotas)
                {
                    CoordsSubdvisao.Clear();
                    for (hh = 0; hh < LinhasProlongamento.Count ; hh++)
                    {
                      //  if (Linhas[hh].auxiliar && Linhas[hh].Visivel)
                        {

                            t = LinhasProlongamento[hh];
                            pixel1(ref t.pIni.x, ref t.pIni.y, ref t.pIni.z, ref z_pixel);
                            pixel2(ref t.pFin.x, ref t.pFin.y, ref t.pFin.z, ref z_pixel2);

                            // if (!(z_pixel < 1 && z_pixel > 0))
                            //     SubdivideBarra(divbarras, t.pIni.x, t.pIni.y, t.pIni.z, t.pFin.x, t.pFin.y, t.pFin.z, false,"F", t);
                            // else
                            // if (!(z_pixel2 < 1 && z_pixel2 > 0))
                            SubdivideBarra(divCotas, t.pIni.x, t.pIni.y, t.pIni.z, t.pFin.x, t.pFin.y, t.pFin.z, t, false, "F", false);
                        }
                    }

                    for (i = 0; i < CoordsSubdvisao.Count; i++)
                    {
                        pixel1(ref CoordsSubdvisao[i].x, ref CoordsSubdvisao[i].y, ref CoordsSubdvisao[i].z, ref z_pixel);

                        /* ForaDaTela = ((px_x1[0] < 0)) ||
                                      ((px_x1[0] > w)) ||
                                      ((px_y1[0] < 0)) ||
                                      ((px_y1[0] > h));

                         if (ForaDaTela)
                             continue;*/

                        //       if (z_pixel < 1 && z_pixel > 0)
                        if ((mouseX >= (px_x1[0] - 10)) && (mouseX <= px_x1[0] + 10))
                        {
                            if ((mouseY >= (px_y1[0] - 10)) && (mouseY <= (px_y1[0]) + 10))
                            {
                                AtualizaPontoSnap(ref CoordsSubdvisao[i].x, ref CoordsSubdvisao[i].y, ref CoordsSubdvisao[i].z, ref px_x1[0], ref px_y1[0]);
                                snapNearest = true;
                                LinhaSnapNearest = CoordsSubdvisao[i].linhaNearest;
                                return;
                            }
                        }
                    }

                    for (hh = Estrutura.barras.Count - 1; hh > -1; hh--)
                    {
                        t = Estrutura.barras[hh].Linha_Eixo;

                        if (!Estrutura.barras[hh].Visivel) continue;
                        /*if (gerenciador.ModoPiso)
                            if (t.Pavimento != gerenciador.cbPiso.SelectedIndex)
                                continue;*/
                        //    if (t.TrechoViga != null)
                        //     continue;

                        /*   if (snap_PontoMeio && !t.auxiliar)
                           {
                               interx = t.getMiddlePoint().x;
                               intery = t.getMiddlePoint().y;
                               interz = t.getMiddlePoint().z;

                               pixel1(ref interx, ref intery, ref interz);
                               //    pixel2(ref t.linhas_eixo.pFin.x, ref  t.linhas_eixo.pFin.y, ref t.linhas_eixo.pFin.z);

                               if ((mouseX >= (px_x1[0] - 30)) && (mouseX <= (px_x1[0] + 30)))
                               {
                                   if ((mouseY >= (px_y1[0] - 30)) && (mouseY <= (px_y1[0] + 30)))
                                   {
                                       UpdateMousePoint(ref interx, ref intery, ref interz, ref px_x1[0], ref px_y1[0]);
                                       snapMiddle = true;
                                       return;
                                   }
                               }
                           }*/

                        //   pixel1(ref t.pIni.x, ref  t.pIni.y, ref t.pIni.z);
                        //   pixel2(ref t.pFin.x, ref  t.pFin.y, ref t.pFin.z);

                        /*    if (snap_PontoFinal )
                            {
                                if ((mouseX >= (px_x1[0] - 30)) && (mouseX <= px_x1[0] + 30))
                                {
                                    if ((mouseY >= (px_y1[0] - 30)) && (mouseY <= (px_y1[0]) + 30))
                                    {
                                        UpdateMousePoint(ref t.pIni.x, ref t.pIni.y, ref t.pIni.z, ref px_x1[0], ref px_y1[0]);
                                        snapEnd = true;

                                        return;
                                    }
                                }

                                if ((mouseX >= (px_x2[0] - 30)) && (mouseX <= px_x2[0] + 30))
                                {
                                    if ((mouseY >= (px_y2[0] - 30)) && (mouseY <= (px_y2[0]) + 30))
                                    {
                                        UpdateMousePoint(ref t.pFin.x, ref t.pFin.y, ref t.pFin.z, ref px_x2[0], ref px_y2[0]);
                                        snapEnd = true;

                                        return;
                                    }
                                }
                            }*/

                        //           LinhaSnapNearest = null;
                        /*  for (i = 0; i < CoordsSubdvisao.Count; i++)
                          {
                              pixel1(ref CoordsSubdvisao[i].x, ref  CoordsSubdvisao[i].y, ref CoordsSubdvisao[i].z, ref z_pixel);

                              ForaDaTela = ((px_x1[0] < 0)) ||
                                           ((px_x1[0] > w)) ||
                                           ((px_y1[0] < 0)) ||
                                           ((px_y1[0] > h));

                              if (ForaDaTela)
                                  continue;

                              if (z_pixel < 1 && z_pixel > 0)                                    
                              if ((mouseX >= (px_x1[0] - 10)) && (mouseX <= px_x1[0] + 10))
                              {
                                  if ((mouseY >= (px_y1[0] - 10)) && (mouseY <= (px_y1[0]) + 10))
                                  {
                                      UpdateMousePoint(ref CoordsSubdvisao[i].x, ref CoordsSubdvisao[i].y, ref CoordsSubdvisao[i].z, ref px_x1[0], ref px_y1[0]);
                                      snapNearest = true;

                                      return;
                                  }
                              }
                          }*/

                        pixel1(ref t.pIni.x, ref t.pIni.y, ref t.pIni.z, ref z_pixel);
                        pixel2(ref t.pFin.x, ref t.pFin.y, ref t.pFin.z, ref z_pixel2);

                        if (!(z_pixel < 1 && z_pixel > 0))
                            continue;

                        if (!(z_pixel2 < 1 && z_pixel2 > 0))
                            continue;

                        ForaDaTela = ((px_x1[0] < 0) && (px_x2[0] < 0)) ||
                                         ((px_x1[0] > w) && (px_x2[0] > w)) ||
                                         ((px_y1[0] < 0) && (px_y2[0] < 0)) ||
                                         ((px_y1[0] > h) && (px_y2[0] > h));
                        if (ForaDaTela)
                            continue;

                        // Ponto_Projetado = Vector3.Project(new Vector3((float)t.pIni.x, (float)t.pIni.y, (float)t.pIni.z), 0, ViewPortPrincipal[3], ViewPortPrincipal[2], -ViewPortPrincipal[3], -1, 1, viewMatrix * Projecao);


                        A.x = (float)px_x1[0];
                        A.y = (float)(ViewPortPrincipal[3] - px_y1[0]);
                        B.x = (float)px_x2[0];
                        B.y = (float)(ViewPortPrincipal[3] - px_y2[0]);
                        //  this.Text = "yi " + A.y.ToString("n2") + " yf " + B.y.ToString("n2");

                        ang = ((float)(FuncoesGerais.atand((A.y - B.y) / (A.x - B.x))));
                        ang += 90;
                        //      this.Text += " ang: " + ang.ToString("n2");
                        A.y = (float)px_y1[0];
                        B.y = (float)px_y2[0];

                        C.x = mouseX - 10;
                        C.y = mouseY;

                        D.x = mouseX + 10;
                        D.y = mouseY;

                        F.x = mouseX;
                        F.y = mouseY - 10;

                        G.x = mouseX;
                        G.y = mouseY + 10;

                        // this.Text += " my: " + mouseY;

                        if (Geom.calcIntersecEQU_RETA(ref A, ref B, ref C, ref D, ref P2) ||
                           (Geom.calcIntersecEQU_RETA(ref A, ref B, ref F, ref G, ref P2)))
                        {
                            //  ang = (float)Linhas[i].angulo + 90;
                            co = (float)(Math.Sin((float)(ang * Const.PIDiv180)) * 10);
                            ca = (float)(Math.Cos((float)(ang * Const.PIDiv180)) * 10);

                            C.x = mouseX - ca;
                            C.y = mouseY + co;

                            D.x = mouseX + ca;
                            D.y = mouseY - co;

                            if (ObjetoNovo != null)
                            {
                                Ponto1_Coord.X = ObjetoNovo.pIni.x;
                                Ponto1_Coord.Y = ObjetoNovo.pIni.y;
                                angleNewObject = (float)(ObjetoNovo.angulo);
                            }

                            anguloLinha = Math.Abs(ang - 90);

                            if (Geom.calcIntersecEQU_RETA(ref A, ref B, ref C, ref D, ref P2))
                            {
                                snapNearest = true;

                               /* if ((Object)t != (Object)LinhaSnapNearest)
                                {
                                    LinhaSnapNearest = t;

                                    SubdivideBarra(divbarras, LinhaSnapNearest.pIni.x, LinhaSnapNearest.pIni.y, LinhaSnapNearest.pIni.z, LinhaSnapNearest.pFin.x, LinhaSnapNearest.pFin.y, LinhaSnapNearest.pFin.z, false,"F", LinhaSnapNearest);
                                    break;
                                }
                                else*/
                                {
                                    LinhaSnapNearest = t;

                                    SubdivideBarra(divCotas, LinhaSnapNearest.pIni.x, LinhaSnapNearest.pIni.y, LinhaSnapNearest.pIni.z, LinhaSnapNearest.pFin.x, LinhaSnapNearest.pFin.y, LinhaSnapNearest.pFin.z, LinhaSnapNearest, false, "F");
                                    for (i = 0; i < CoordsSubdvisao.Count; i++)
                                    {
                                        pixel1(ref CoordsSubdvisao[i].x, ref CoordsSubdvisao[i].y, ref CoordsSubdvisao[i].z, ref z_pixel);

                                        ForaDaTela = ((px_x1[0] < 0)) ||
                                                   ((px_x1[0] > w)) ||
                                                   ((px_y1[0] < 0)) ||
                                                   ((px_y1[0] > h));

                                        if (ForaDaTela)
                                            continue;

                                        if (z_pixel < 1 && z_pixel > 0)
                                            if ((mouseX >= (px_x1[0] - 10)) && (mouseX <= px_x1[0] + 10))
                                            {
                                                if ((mouseY >= (px_y1[0] - 10)) && (mouseY <= (px_y1[0]) + 10))
                                                {
                                                    AtualizaPontoSnap(ref CoordsSubdvisao[i].x, ref CoordsSubdvisao[i].y, ref CoordsSubdvisao[i].z, ref px_x1[0], ref px_y1[0]);
                                                    snapNearest = true;
                                                    LinhaSnapNearest = t;
                                                    return;
                                                }
                                            }
                                    }
                                }
                            }
                        }
                        snapNearest = false;
                    }
                }
            }
            catch (Exception e)
            {
                MessageBox.Show(e.Message);
            }
        }

        public double divCotas; // 15 cm é o padrão
        vec3 u1, u2, u, normxy, normxz;
        double NdotU, ndotu_mod, cos_alfa, angXY, angXZ, divisoes, divisoesFrac, xAnt;
        int divint;
        List<vec3> CoordsSubdvisao = new List<vec3>();
        double[] posicao = new double[4];
        double[] posicaoFinal = new double[4];
        double[] posicaoFinal2 = new double[4];
        double[] posicaoFinal_Trans = new double[5];

        double tx, ty, tz;
        bool zi_maior_que_zf, xi_igual_xf;
        double y__, z__, x__;
        vec3 pos;
        vec3 pinicial = new vec3(0);
        vec3 pfinal = new vec3(0);
        vec3 pmeio = new vec3(0);

        List<vec3> Pontos_SnapIntersecao = new List<vec3>();

        List<IntersecBarras> IntersecoesBarras = new List<IntersecBarras>();

        PerpendicularBarras[] PerpBarras = new PerpendicularBarras[5000];
        

        List<vec3> Pontos_SnapPontoMedio = new List<vec3>();
        //  List<vec3> Pontos_SnapPontoExtremo = new List<vec3>();

        vec3[] Pontos_SnapPontoExtremo = new vec3[5000];

        List<sPontosMeio> s_PontosMedio = new List<sPontosMeio>();
        struct sPontosMeio
        {
            public vec3 ponto;
            public TLinha linha;
        }
        public struct IntersecBarras
        {
            public vec3 ponto;
            public TBarraGenerica b1, b2;
        }
        struct PerpendicularBarras
        {
            public vec3 ponto;
            public TBarraGenerica b1;
            public TLinha linha_nearest;

            public PerpendicularBarras(vec3 p, TBarraGenerica b, TLinha l)
            {
                ponto = p;
                b1 = b;
                linha_nearest = l;
            }
        }

        bool PontoExiste_SnapIntersecao(double x_, double y_, double z_)
        {
            foreach (vec3 l in Pontos_SnapIntersecao)
            {
                if ((Geom.Iguais(l.x, x_)) && (Geom.Iguais(l.y, y_) && (Geom.Iguais(l.z, z_))))
                    return true;
            }
            return false;
        }

        bool PontoExiste_SnapPontoExtremo(double x_, double y_, double z_)
        {
            for (int i = 0; i < max_snap_extremo+1; i++)
            {
                if ((Geom.Iguais(Pontos_SnapPontoExtremo[i].x, x_)) && (Geom.Iguais(Pontos_SnapPontoExtremo[i].y, y_) &&
                    (Geom.Iguais(Pontos_SnapPontoExtremo[i].z, z_))))
                    return true;

            }
            return false;
        }
        bool PontoExiste_SnapPontoMedio(double x_, double y_, double z_)
        {
            for (int i = 0; i < s_PontosMedio.Count; i++)
            {
                if ((Geom.Iguais(Pontos_SnapPontoMedio[i].x, x_)) && (Geom.Iguais(Pontos_SnapPontoMedio[i].y, y_) &&
                    (Geom.Iguais(Pontos_SnapPontoMedio[i].z, z_))))
                    return true;

            }
            return false;
        }
        int max_PerpBarras = -1;
        public void AtualizaListaPerpendicular()
        {
            PerpBarras = new PerpendicularBarras[5000];
             max_PerpBarras = -1;
            vec3 p_avaliar = new vec3(0), p_1,p_2, p_3, perpendicular;
            vec3 p_r = new vec3(0, 0, 0);
            vec3 ponto = new vec3(0, 0, 0);
            TBarraGenerica  b2;
            TLinha lin;
            bool habilita = false;

            if (ObjetoNovo != null)
            {
                if (ObjetoNovo.Tipo == Const.ID_BARRAGENERICA)
                {
                    p_avaliar = new vec3(ObjetoNovo.pIni.x, ObjetoNovo.pIni.y, ObjetoNovo.pIni.z);
                    habilita = true;
                }
            }
            else
            if (FerramentaEdicao != null)
            {
                if (IdFerramentaEdicao == Const.ID_COPIAR_ELEMENTOS)
                {
                    if ((FerramentaEdicao as TCopiarElementos).Comando == Const.COPIAR_COMANDO_3)
                    {
                        if ((Object)(FerramentaEdicao as TCopiarElementos).ponto1 != null)
                        {
                            p_avaliar = new vec3((FerramentaEdicao as TCopiarElementos).ponto1.x, (FerramentaEdicao as TCopiarElementos).ponto1.y, (FerramentaEdicao as TCopiarElementos).ponto1.z);
                            habilita = true;
                        }
                    }
                }
                else
                if (IdFerramentaEdicao == Const.ID_MOVER_ELEMENTOS)
                {
                    if ((FerramentaEdicao as TMoverElementos).Comando == Const.MOVER_COMANDO_3)
                    {
                        if ((Object)(FerramentaEdicao as TMoverElementos).ponto1 != null)
                        {
                            p_avaliar = new vec3((FerramentaEdicao as TMoverElementos).ponto1.x, (FerramentaEdicao as TMoverElementos).ponto1.y, (FerramentaEdicao as TMoverElementos).ponto1.z);
                            habilita = true;
                        }
                    }
                }
                else
                if (IdFerramentaEdicao == Const.ID_MOVER_EXTREMO_ELEMENTOS)
                {
                    if ((FerramentaEdicao as TMoverExtremoElemento).Comando == Const.MOVER_EXTREMO_COMANDO_4)
                    {
                        if ((Object)(FerramentaEdicao as TMoverExtremoElemento).ponto1 != null)
                        {
                            p_avaliar = new vec3((FerramentaEdicao as TMoverExtremoElemento).ponto1.x, (FerramentaEdicao as TMoverExtremoElemento).ponto1.y, (FerramentaEdicao as TMoverExtremoElemento).ponto1.z);
                            habilita = true;
                        }
                    }
                }
            }

            //for (j = 0; j < LinhasProlongamento.Count; j++)
            {
                //  lin = LinhasProlongamento[j];
                // p_1 = new vec3(lin.pIni.x, lin.pIni.y, lin.pIni.z);
                if (habilita)
                {
                   // if (ObjetoNovo.Tipo == Const.ID_BARRAGENERICA)
                    {
                      //  p_avaliar = new vec3(ObjetoNovo.pIni.x, ObjetoNovo.pIni.y, ObjetoNovo.pIni.z);
                        for (i = 0; i < Barras.Count; i++)
                        {
                            b2 = Barras[i];

                            if (!b2.Visivel) continue;

                            p_1 = new vec3(b2.pIni.x, b2.pIni.y, b2.pIni.z);
                            p_2 = new vec3(b2.pFin.x, b2.pFin.y, b2.pFin.z);

                            perpendicular = Geom.PontoPerpendicular(p_1, p_2, p_avaliar);

                            if (Geom.Iguais(perpendicular.x, p_avaliar.x) &&
                                Geom.Iguais(perpendicular.y, p_avaliar.y) &&
                                Geom.Iguais(perpendicular.z, p_avaliar.z))
                                continue;

                            double dist1 = perpendicular.DistanceTo(p_1);
                            double dist2 = perpendicular.DistanceTo(p_2);

                            if (dist1 < dist2)
                                lin = new TLinha(new TPonto(p_1.x, p_1.y, p_1.z), new TPonto(perpendicular.x, perpendicular.y, perpendicular.z));
                            else
                                lin = new TLinha(new TPonto(p_2.x, p_2.y, p_2.z), new TPonto(perpendicular.x, perpendicular.y, perpendicular.z));

                            lin.Barra = (TBarraGenerica)b2;
                            PerpBarras[++max_PerpBarras] = new PerpendicularBarras(perpendicular, b2, lin);
                        }
                    }
                }
            }
        }

        public void AtualizaListaIntersecoes(TBarraGenerica bar, bool prolongamentos = false)
        {
            IntersecoesBarras.Clear();
            Pontos_SnapIntersecao.Clear();
            double tt = 0, uu = 0;
            vec3 p_1, p_2, p_3, p_4;
            vec3 p_r = new vec3(0, 0, 0);
            vec3 ponto = new vec3(0, 0, 0);
            TBarraGenerica b1, b2;
            TLinha lin;
            
          //  bar = null;
            
            if (prolongamentos)
            {
                for (i = 0; i < Barras.Count; i++)
                {
                    b1 = Barras[i];
                    if (!b1.Visivel) continue;

                    p_1 = new vec3(b1.pIni.x, b1.pIni.y, b1.pIni.z);
                    p_2 = new vec3(b1.pFin.x, b1.pFin.y, b1.pFin.z);

                    for (j = 0; j < LinhasProlongamento.Count; j++)
                    {
                        lin = LinhasProlongamento[j];
                        p_3 = new vec3(lin.pIni.x, lin.pIni.y, lin.pIni.z);
                        p_4 = new vec3(lin.pFin.x, lin.pFin.y, lin.pFin.z);
                        if (Geom.TemInterseccao3D(ref p_1, ref p_2, ref p_3, ref p_4, ref ponto, ref tt, ref uu))
                        {
                            vec3 pontoToque = new vec3(0);
                            if (Geom.PontoTocaAresta(ponto, p_1, p_2, ref pontoToque, false) &&
                                Geom.PontoTocaAresta(ponto, p_3, p_4, ref pontoToque, false))
                            {
                                if (!PontoExiste_SnapIntersecao(ponto.x, ponto.y, ponto.z))
                                    Pontos_SnapIntersecao.Add(new vec3(ponto.x, ponto.y, ponto.z));
                            }
                        }
                    }
                }
            }
            else
            if (bar != null)
            {
                b1 = bar;

                p_1 = new vec3(b1.pIni.x, b1.pIni.y, b1.pIni.z);
                p_2 = new vec3(b1.pFin.x, b1.pFin.y, b1.pFin.z);

                for (i = 0; i < Barras.Count; i++)
                {
                    b1 = Barras[i];
                    if (!b1.Visivel) continue;

                    p_1 = new vec3(b1.pIni.x, b1.pIni.y, b1.pIni.z);
                    p_2 = new vec3(b1.pFin.x, b1.pFin.y, b1.pFin.z);

                    for (j = 0; j < LinhasProlongamento.Count; j++)
                    {
                        lin = LinhasProlongamento[j];
                        p_3 = new vec3(lin.pIni.x, lin.pIni.y, lin.pIni.z);
                        p_4 = new vec3(lin.pFin.x, lin.pFin.y, lin.pFin.z);
                        if (Geom.TemInterseccao3D(ref p_1, ref p_2, ref p_3, ref p_4, ref ponto, ref tt, ref uu))
                        {
                            vec3 pontoToque = new vec3(0);
                            if (Geom.PontoTocaAresta(ponto, p_1, p_2, ref pontoToque, false) &&
                                Geom.PontoTocaAresta(ponto, p_3, p_4, ref pontoToque, false))
                            {
                                if (!PontoExiste_SnapIntersecao(ponto.x, ponto.y, ponto.z))
                                    Pontos_SnapIntersecao.Add(new vec3(ponto.x, ponto.y, ponto.z));
                            }
                        }
                    }
                }

                    /* for (j = 0; j < Barras.Count; j++)
                     {
                         b2 = Barras[j];

                         if (!b2.Visivel) continue;

                         if (b1.IDBarra != b2.IDBarra)
                         {
                             p_3 = new vec3(b2.pIni.x, b2.pIni.y, b2.pIni.z);
                             p_4 = new vec3(b2.pFin.x, b2.pFin.y, b2.pFin.z);

                             if (Geom.TemInterseccao3D(ref p_1, ref p_2, ref p_3, ref p_4, ref ponto, ref tt, ref uu))
                             {
                                 vec3 pontoToque = new vec3(0);
                                 if (Geom.PontoTocaAresta(ponto, p_1, p_2, ref pontoToque, false) &&
                                     Geom.PontoTocaAresta(ponto, p_3, p_4, ref pontoToque, false))
                                 {
                                     if (!PontoExiste_SnapIntersecao(ponto.x, ponto.y, ponto.z))
                                     {
                                         IntersecBarras ib;
                                         ib.ponto = new vec3(ponto.x, ponto.y, ponto.z);
                                         ib.b1 = b1;
                                         ib.b2 = b2;
                                         IntersecoesBarras.Add(ib);
                                         Pontos_SnapIntersecao.Add(new vec3(ponto.x, ponto.y, ponto.z));
                                     }
                                 }
                             }
                         }
                     }*/
                }
            else
            {
                for (i = 0; i < Barras.Count; i++)
                {
                    b1 = Barras[i];
                    if (!b1.Visivel) continue;

                    p_1 = new vec3(b1.pIni.x, b1.pIni.y, b1.pIni.z);
                    p_2 = new vec3(b1.pFin.x, b1.pFin.y, b1.pFin.z);

                    for (j = 0; j < LinhasProlongamento.Count; j++)
                    {
                        lin = LinhasProlongamento[j];
                        p_3 = new vec3(lin.pIni.x, lin.pIni.y, lin.pIni.z);
                        p_4 = new vec3(lin.pFin.x, lin.pFin.y, lin.pFin.z);
                        if (Geom.TemInterseccao3D(ref p_1, ref p_2, ref p_3, ref p_4, ref ponto, ref tt, ref uu))
                        {
                            vec3 pontoToque = new vec3(0);
                            if (Geom.PontoTocaAresta(ponto, p_1, p_2, ref pontoToque, false) &&
                                Geom.PontoTocaAresta(ponto, p_3, p_4, ref pontoToque, false))
                            {
                                if (!PontoExiste_SnapIntersecao(ponto.x, ponto.y, ponto.z))
                                    Pontos_SnapIntersecao.Add(new vec3(ponto.x, ponto.y, ponto.z));
                            }
                        }
                    }

                    for (j = 0; j < Barras.Count; j++)
                    {
                        b2 = Barras[j];

                        if (!b2.Visivel) continue;

                        if (b1.IDBarra != b2.IDBarra)
                        {
                            p_3 = new vec3(b2.pIni.x, b2.pIni.y, b2.pIni.z);
                            p_4 = new vec3(b2.pFin.x, b2.pFin.y, b2.pFin.z);

                            if (Geom.TemInterseccao3D(ref p_1, ref p_2, ref p_3, ref p_4, ref ponto, ref tt, ref uu))
                            {
                                vec3 pontoToque = new vec3(0);
                                if (Geom.PontoTocaAresta(ponto, p_1, p_2, ref pontoToque, false) &&
                                    Geom.PontoTocaAresta(ponto, p_3, p_4, ref pontoToque, false))
                                {
                                    if (!PontoExiste_SnapIntersecao(ponto.x, ponto.y, ponto.z))
                                    {
                                        IntersecBarras ib;
                                        ib.ponto = new vec3(ponto.x, ponto.y, ponto.z);
                                        ib.b1 = b1;
                                        ib.b2 = b2;
                                        IntersecoesBarras.Add(ib);
                                        Pontos_SnapIntersecao.Add(new vec3(ponto.x, ponto.y, ponto.z));
                                    }
                                }
                            }
                        }
                    }

                }
            }
        }

        void AtualizaListaPontoMedio(TBarraGenerica barra)
        {
            sPontosMeio spn;
            if (barra != null)
            {
                t = barra.Linha_Eixo;

                interx = t.getMiddlePoint().x;
                intery = t.getMiddlePoint().y;
                interz = t.getMiddlePoint().z;
                if (!PontoExiste_SnapPontoMedio(interx, intery, interz))
                {
                    spn = new sPontosMeio();
                    spn.linha = t;
                    spn.ponto = new vec3(interx, intery, interz);
                    s_PontosMedio.Add(spn);
                    Pontos_SnapPontoMedio.Add(new vec3(interx, intery, interz));
                }
            }
            else
            {
                Pontos_SnapPontoMedio.Clear();
                s_PontosMedio.Clear();

                for (i = 0; i < Estrutura.barras.Count; i++)
                {
                    t = Estrutura.barras[i].Linha_Eixo;

                    if (Estrutura.barras[i].Visivel)
                    {
                        interx = t.getMiddlePoint().x;
                        intery = t.getMiddlePoint().y;
                        interz = t.getMiddlePoint().z;
                        if (!PontoExiste_SnapPontoMedio(interx, intery, interz))
                        {
                            spn = new sPontosMeio();
                            spn.linha = t;
                            spn.ponto = new vec3(interx, intery, interz);
                            s_PontosMedio.Add(spn);
                            Pontos_SnapPontoMedio.Add(new vec3(interx, intery, interz));
                        }
                    }
                }
            }
        }

        int max_snap_extremo = -1;
        void AtualizaListaPontoExtremo(TBarraGenerica barra)
        {


            vec3 ponto = new vec3(0, 0, 0);
            TApoio apoio;
            TNoPortico noP;
            if (barra != null)
            { 
                t =barra.Linha_Eixo;
                ponto.x = t.pIni.x; ponto.y = t.pIni.y; ponto.z = t.pIni.z;
                if (!PontoExiste_SnapPontoExtremo(ponto.x, ponto.y, ponto.z))
                    Pontos_SnapPontoExtremo[++max_snap_extremo] = new vec3(t.pIni.x, t.pIni.y, t.pIni.z);

                ponto.x = t.pFin.x; ponto.y = t.pFin.y; ponto.z = t.pFin.z;
                if (!PontoExiste_SnapPontoExtremo(ponto.x, ponto.y, ponto.z))
                    Pontos_SnapPontoExtremo[++max_snap_extremo] = new vec3(t.pFin.x, t.pFin.y, t.pFin.z);               
            }
            else
            {
                Pontos_SnapPontoExtremo = new vec3[5000];
                max_snap_extremo = -1;

                for (i = 0; i < Estrutura.barras.Count; i++)
                {
                    t = Estrutura.barras[i].Linha_Eixo;

                    if (Estrutura.barras[i].Visivel)
                    {
                        ponto.x = t.pIni.x; ponto.y = t.pIni.y; ponto.z = t.pIni.z;
                        if (!PontoExiste_SnapPontoExtremo(ponto.x, ponto.y, ponto.z))
                            Pontos_SnapPontoExtremo[++max_snap_extremo] = new vec3(t.pIni.x, t.pIni.y, t.pIni.z);

                        ponto.x = t.pFin.x; ponto.y = t.pFin.y; ponto.z = t.pFin.z;
                        if (!PontoExiste_SnapPontoExtremo(ponto.x, ponto.y, ponto.z))
                            Pontos_SnapPontoExtremo[++max_snap_extremo] = new vec3(t.pFin.x, t.pFin.y, t.pFin.z);
                    }
                }
            }

            for (i = 0; i < Estrutura.apoios.Count; i++)
            {
                apoio = Estrutura.apoios[i];

                if (apoio.Visivel)
                {
                    ponto.x = apoio.pIni.x; ponto.y = apoio.pIni.y; ponto.z = apoio.pIni.z;
                    if (!PontoExiste_SnapPontoExtremo(ponto.x, ponto.y, ponto.z))
                        Pontos_SnapPontoExtremo[++max_snap_extremo] = new vec3(apoio.pIni.x, apoio.pIni.y, apoio.pIni.z);
                }
            }

          //  if (Datum)
        //      Pontos_SnapPontoExtremo.Add(new vec3(0,0,0));

         /*   if (Estrutura.PorticoEspacial != null)
            {
                for (i = 0; i < Estrutura.PorticoEspacial.nos.Count(); i++)
                {
                    noP = Estrutura.PorticoEspacial.nos[i];
                    if (noP != null)
                    {
                        ponto.x = noP.coordx_tela; ponto.y = noP.coordy_tela; ponto.z = noP.coordz_tela;
                        if (!PontoExiste_SnapPontoExtremo(ponto.x, ponto.y, ponto.z))
                            Pontos_SnapPontoExtremo.Add(new vec3(ponto.x, ponto.y, ponto.z));
                    }
                }
            }*/

            foreach (TPonto p in  Estrutura.nos)
            {
                if (p.habilitado)
                {
                    ponto.x = p.x; ponto.y = p.y; ponto.z = p.z;

                    if (!PontoExiste_SnapPontoExtremo(ponto.x, ponto.y, ponto.z))
                        Pontos_SnapPontoExtremo[++max_snap_extremo] = new vec3(p.x, p.y, p.z);
                }
            }
        }

        public void AtualizaListaSnap(bool intersecao = true, TBarraGenerica bar = null, bool sohProlongamentos = false)
        {
            if (intersecao)
            {
                if (gerenciador.Intersecao.Checked)
                    AtualizaListaIntersecoes(bar, sohProlongamentos);
            }
            AtualizaListaPontoMedio(bar);
            AtualizaListaPontoExtremo(bar);
            AtualizaListaPerpendicular();
        }

        void SubdivideBarra(double div, double xi, double yi, double zi, double xf, double yf, double zf, TLinha linhaNearest, bool InserindoCota = false, string PontoMaisProximo = "F", bool limpa = true)
        {
            /* - faço translação da barra para o ponto zero usando tx ty  tz
               - encontro os angulos com os planos xy e xz
               - crio a barra no eixo global x, a partir do ponto zero ( x=0 y=0 z=0) e faço a subdivisao nos tamanhos
                  que eu quero criando a lista de pontos nesse eixo global x
               - rotaciono esses pontos em torno do ponto zero de acordo com os angulos que achei nos planos xy e xz
               - faço a translação dos pontos com sinal contrário ( -tx -ty -tz)  para voltar para a posição original 
             */

            try
            {

                if (Geom.Iguais(xi, 0))
                    xi = 0;
                if (Geom.Iguais(yi, 0))
                    yi = 0;
                if (Geom.Iguais(zi, 0))
                    zi = 0;

                if (Geom.Iguais(xf, 0))
                    xf = 0;
                if (Geom.Iguais(yf, 0))
                    yf = 0;
                if (Geom.Iguais(zf, 0))
                    zf = 0;

                L = (Math.Sqrt(Math.Pow(xi - xf, 2) + Math.Pow(yi - yf, 2) + Math.Pow(zi - zf, 2)));
                
              //  if (!checkBox1.Checked)
               // {               
                zi_maior_que_zf = false;

                 //   if (!Geom.Iguais(Math.Abs(zf), Math.Abs(zi))) 
                      zi_maior_que_zf = ((zi * -1) > (zf * -1));

                    xi_igual_xf = Geom.Iguais(xi, xf);
                /*
                    if (zi_maior_que_zf)
                    {
                        x__ = xi;
                        y__ = yi;
                        z__ = zi;

                        xi = xf;
                        yi = yf;
                        zi = zf;

                        xf = x__;
                        yf = y__;
                        zf = z__;
                    }*/

                    /*faço uma translação da barra para o ponto zero, como se eu fizesse o comando mover do programa
                     * para o ponto zero pegando o pIni como pivo */
                
                    pinicial.x = xi; pinicial.y = yi; pinicial.z = zi;
                    pfinal.x = xi; pfinal.y = yi; pfinal.z = zi;
                    pmeio = (pinicial+pfinal)/2;

                    tx = xi;
                    ty = yi;
                    tz = zi; 
                    
                    xi -= tx;
                    yi -= ty;
                    zi -= tz;

                    xf -= tx;
                    yf -= ty;
                    zf -= tz;

                if (Geom.Iguais(xi, 0))
                    xi = 0;
                if (Geom.Iguais(yi, 0))
                    yi = 0;
                if (Geom.Iguais(zi, 0))
                    zi = 0;

                if (Geom.Iguais(xf, 0))
                    xf = 0;
                if (Geom.Iguais(yf, 0))
                    yf = 0;
                if (Geom.Iguais(zf, 0))
                    zf = 0;

                /**/
                if (limpa)
                      CoordsSubdvisao.Clear();
                //    L = (Math.Sqrt(Math.Pow(xi - xf, 2) + Math.Pow(yi - yf, 2)));

                    u1 = new vec3(xi, yi, zi*-1);
                    u2 = new vec3(xf, yf, zf*-1);

                    //Encontrar angulo que a barra faz com os planos XY e XZ

                    //PLANO XY
                    normxy = new vec3(0, 0, 1);

                    //  if (xi < xf)
                    u = u1 - u2;
                    //else
                    //    u = u2 - u1;

                    NdotU = (normxy.DotProduct(u));
                    ndotu_mod = normxy.Magnitude() * u.Magnitude();
                    cos_alfa =Math.Abs( NdotU / ndotu_mod);
                    angXY = RMath.rad2deg(Math.Acos(cos_alfa));
                    angXY =/* RMath.rad2deg(Math.Atan(u1.z - u2.z / (u1.y - u2.y)));*/(90-angXY) * 1;

                    //PLANO XZ
                    normxz = new vec3(0, 1, 0);
                    //  if (xi< xf)
                    u = u1 - u2;
                    //  else
                    //     u = u1 - u2;

                    NdotU = (normxz.DotProduct(u));
                    ndotu_mod = normxz.Magnitude() * u.Magnitude();
                    cos_alfa = Math.Abs( NdotU / ndotu_mod);
                    angXZ = RMath.rad2deg(Math.Acos(cos_alfa));

                    if (!Geom.Iguais(u1.x - u2.x, 0))
                      angXZ = RMath.rad2deg(Math.Atan(u1.y - u2.y / (u1.x - u2.x)));/* (90-angXZ) * 1;*/
                    else
                    {
                        if (!Geom.Iguais(yi, yf))
                        {
                           if (yi < yf)
                               angXZ = -90;
                           else
                               angXZ = 90;
                        }
                        else
                           angXZ = 90; 
                    }

                    divisoes = (L / div);
                    divisoesFrac = FuncoesGerais.Frac(L / div);
                    divint = (int)(divisoes - divisoesFrac);

                    pos = new vec3(0, 0, 0);
                    xAnt = xi;
                    
                    if ((xi < xf) && (!xi_igual_xf))
                      angXY *= -1;

                    if (!Geom.Iguais(Math.Abs( angXZ),90))
                      angXZ *= -1;

                    if (xi_igual_xf)
                      angXY *=-1;

                    if (zi_maior_que_zf)
                        angXY *= -1;  

             //       textBox5.Text = angXY.ToString("n2");
           //         textBox6.Text = angXZ.ToString("n2");

                    pos.x = xi;
                    pos.y = yi;
                    pos.z = zi;
                    xAnt = pos.x;

                    pos.x = 0;
                    pos.y = 0;
                    pos.z = 0;
                    xAnt = 0;

                  //  CoordsSubdvisao.Add(pos);
                 /*   if (InserindoCota)
                    {
                        for (int i = 0; i < divint; i++)
                        {
     //                       pos = new vec3(xi > xf ? xAnt - div : xAnt + div, yi, zi);
                            if ((xi > xf) && PontoMaisProximo == "F")
                              pos = new vec3(xAnt + div, yi, zi);
                            else
                            if ((xi > xf) && PontoMaisProximo == "I")
                              pos = new vec3(xAnt - div, yi, zi);
     
                            CoordsSubdvisao.Add(pos);
                            xAnt = pos.x;
                        }
                    }
                    else*/
                    {
                        for (int i = 1; i < divint ; i++)
                        {
                            pos = new vec3(((xi > xf) && !xi_igual_xf) ? xAnt - div : xAnt + div, yi, zi);
                         /*   if ((pos.DistanceTo(pinicial)) > (pmeio.DistanceTo(pinicial)))
                            {
                                pos = new vec3(xi > xf ? xAnt - div : xAnt + div, yi, xi);
                            }*/

                            CoordsSubdvisao.Add(pos);
                            xAnt = pos.x;
                        }
                    }

                    pos = new vec3(xi > xf ? xAnt - (div * divisoesFrac) : xAnt + (div * divisoesFrac), yi, zi);
                    CoordsSubdvisao.Add(pos);
                  //  if (checkBox2.Checked)
                    {
                        for (int i = CoordsSubdvisao.Count - divint; i < CoordsSubdvisao.Count; i++)
                        {
                            posicao[1] = CoordsSubdvisao[i].x;
                            posicao[2] = CoordsSubdvisao[i].y;
                            posicao[3] = CoordsSubdvisao[i].z;

                            Geom.rotY(angXY, ref posicao, ref posicaoFinal);
                            Geom.rotZ(angXZ, ref posicaoFinal, ref posicaoFinal2);

                            CoordsSubdvisao[i].x = posicaoFinal2[1];
                            CoordsSubdvisao[i].y = posicaoFinal2[2];
                            CoordsSubdvisao[i].z = posicaoFinal2[3];

                            CoordsSubdvisao[i].linhaNearest = linhaNearest;
                            CoordsSubdvisao[i].x += tx;
                            CoordsSubdvisao[i].y += ty;
                            CoordsSubdvisao[i].z += tz;
                        }
                    }

                      //Translado pra posição original
                   // for (int i = 0; i < CoordsSubdvisao.Count; i++)
                   // {

                    //}
                } 

            catch(Exception e)
            {
                MessageBox.Show(e.Message);
            }
        }

        private void UpdateMousePoint(TPonto pt)
        {
         /*   mousepoint.x    = pt.x;
            mousepoint.y    = pt.y;
            mousepoint.z    = pt.z;
            mousepoint.px_x = pixelX(pt.x);
            mousepoint.px_y = pixelY(pt.y);
            mousepoint.Snap = true;
            */
      //      SwapBuffers();
         //   gerenciador.CoordenadaX.Text = pt.x.ToString("n2").Replace(",", ".") + ", " + pt.y.ToString("n2").Replace(",", ".");
            
        }
        double alfa;
        double ortox, ortoy;
        public void Orto(bool PontoInicial)
        {
            //     if (pFin.Snap)
            //     MessageBox.Show("");
               alfa = FuncoesGerais.atand(Math.Abs(ObjetoNovo.pIni.y - mousepoint.y) / Math.Abs(ObjetoNovo.pIni.x - mousepoint.x));

                if (ConfiguracoesCaptura.Captura.OrtogonalLigado)
                {
                    if (alfa > 88)
                    {
                        alfa = 90;

                        //if (!PontoInicial)
                            ortox = ObjetoNovo.pIni.x;
                            AtualizaPontoSnap(ref ortox, ref mousepoint.y, ref ObjetoNovo.pIni.z, ref ortox, ref ortoy);
                        //  else
                     //       pIni.x = pFin.x;

                    }
                    else
                    if (alfa > 0 && alfa < 2)
                    {
                        alfa = 0;
                        //this.angulo = 0;

                       // if (!PontoInicial)
                        ortoy = ObjetoNovo.pIni.y;
                        AtualizaPontoSnap(ref mousepoint.x, ref ortoy, ref ObjetoNovo.pIni.z, ref ortox, ref ortoy);
                        //  else
                      //      pIni.y = pFin.y;
                    };

                };

                DifCoordX = Math.Abs(ObjetoNovo.pIni.x - mousepoint.x);
                DifCoordY = Math.Abs(ObjetoNovo.pIni.y - mousepoint.y);

                for (i = 0; i < ConfiguracoesCaptura.Captura.OutrosAngulos.Count; i++)
                {
                    if (alfa >= (ConfiguracoesCaptura.Captura.OutrosAngulos[i] - 2) && alfa <= (ConfiguracoesCaptura.Captura.OutrosAngulos[i] + 2))
                    {
                        alfa = ConfiguracoesCaptura.Captura.OutrosAngulos[i];
                       // this.angulo = (float)alfa;

                        if (mousepoint.y > ObjetoNovo.pIni.y)
                            mousepoint.y = (ObjetoNovo.pIni.y + (float)(Math.Tan(alfa * Const.PIDiv180) * DifCoordX));
                        else
                            mousepoint.y = (ObjetoNovo.pIni.y - (float)(Math.Tan(alfa * Const.PIDiv180) * DifCoordX));

                        //   Posicao.Y = pixelY(pFin.y);
                    };
                };
        }

        private void AtualizaPontoSnap(ref double x, ref double y, ref double z, ref double pxx, ref double pxy)
        {

            mousepoint.x = x;
            mousepoint.y = y;
            mousepoint.z = z;

            mousepoint.px_x = (float)pxx;
            mousepoint.px_y = (float)pxy;
            mousepoint.Snap = true;


          //  SwapBuffers();
            gerenciador.CoordenadaX.Text = "x: " + x.ToString("n2").Replace(",", ".") + " y: " + y.ToString("n2").Replace(",", ".") + " z: " + z.ToString("n2").Replace(",", ".");
        }
        private void AtualizaPontoSnap(double x, double y,  float pxx, float pxy)
        {
        /*    mousepoint.x = x;
            mousepoint.y = y;

            mousepoint.px_x = pixelX(x);
            mousepoint.px_y = pixelY(y);
            mousepoint.Snap = true;*/


            //  SwapBuffers();
      //      gerenciador.CoordenadaX.Text = x.ToString("n2").Replace(",", ".") + ", " + y.ToString("n2").Replace(",", ".") + z.ToString("n2").Replace(",", ".");
        }
        private bool getNearestEdge(ref TPonto pt)
        {
            //if (AchouPontoProximo)
            {
                PontoProximo_coordX = 0;
                PontoProximo_coordY = 0;

                Coord((double)(P2.x), (double)(P2.y), ref PontoProximo_coordX, ref (PontoProximo_coordY));

                pt = new TPonto(PontoProximo_coordX, PontoProximo_coordY, 0, P2.x, P2.y, Pontos.Count + 1);
                return true;
            };

            return false;
        }

        private bool getSnapPoint(ref TPonto pt, float x, float y)
        {
                //retorna ponto notável
            for (i = 0; i < Pontos.Count; i++)
            {
               if ((x >= (Pontos[i].px_x - 20)) && (x <= (Pontos[i].px_x + 20)))
               {
                  if ((y >= (Pontos[i].px_y - 20)) && (y <= (Pontos[i].px_y + 20)))
                  {
                      pt = Pontos[i];

                     return true;
                  }
                }
            };
           
             return false;
        }

      /*  private void InitializeComponent()
        {
            this.ribbonTab1 = new System.Windows.Forms.RibbonTab();
            this.ribbonPanel1 = new System.Windows.Forms.RibbonPanel();
            this.SuspendLayout();
            // 
            // ribbonTab1
            // 
            this.ribbonTab1.Name = "ribbonTab1";
            this.ribbonTab1.Panels.Add(this.ribbonPanel1);
            this.ribbonTab1.Text = "ribbonTab1";
            this.ribbonTab1.Visible = false;
            // 
            // ribbonPanel1
            // 
            this.ribbonPanel1.Name = "ribbonPanel1";
            this.ribbonPanel1.Text = "ribbonPanel1";
            this.ribbonPanel1.Visible = false;
            // 
            // Desenho
            // 
            this.ClientSize = new System.Drawing.Size(553, 413);
            this.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.KeyPreview = true;
            this.Name = "Desenho";
            this.ResumeLayout(false);

        }*/

        private void ribbon2_OrbDropDown_Click(object sender, EventArgs e)
        {

        }
    }
}
