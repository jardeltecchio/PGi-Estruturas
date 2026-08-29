using PG;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Drawing;
using System.Linq;

namespace PG
{

    public partial class TBarraPortico : TObjetoDesenho
    {
        public void Preenche_MZ_Gradiente(ref List<float> coords_triangulos, ref double MultiplicadorAltura,
                      int tipocarga, int caso, int comb)
        {
            try
            {
                if (Visivel /*&& barraOriginal.Visivel && !barra_de_articulacao */&& !barraRigida)
                {
                    if (tipocarga == 0)
                    {
                        Esforcos = casos_x_esforcos[caso].Esforcos;
                    }
                    else
                    {
                        Esforcos = combinacoes_x_esforcos[comb].Esforcos;
                    }

                    if ((Geom.Iguais(Esforcos[5], 0)) && Geom.Iguais(Esforcos[11], 0))
                        return;

                    {
                        coordsFletorZ[0].x = 0;
                        coordsFletorZ[0].y = 0;
                        coordsFletorZ[0].z = 0;

                        coordsFletorZ[1].x = 0;
                        coordsFletorZ[1].y = Esforcos[5] * -1 * MultiplicadorAltura;
                        coordsFletorZ[1].z = 0;

                        if (barraOriginal_Vertical)
                            if (pIni_offset.z * -1 < pFin_offset.z * -1)
                                coordsFletorZ[1].y *= -1;

                        coordsFletorZ[2].x = comprimento;
                        coordsFletorZ[3].x = comprimento;

                        coordsFletorZ[2].y = Esforcos[11] * MultiplicadorAltura;

                        if (!Geom.Iguais(barraOriginal.pIni.x, barraOriginal.pFin.x))
                        {
                            if (barraOriginal.pIni.x > barraOriginal.pFin.x)
                            {
                                coordsFletorZ[2].x = -comprimento;
                                coordsFletorZ[3].x = -comprimento;

                                coordsFletorZ[1].y *= -1;
                                coordsFletorZ[2].y *= -1;
                            }
                        }

                        GiraDiagramaConformeAnguloAlfa(ref coordsFletorZ[1], "X");

                        if (barraOriginal_Vertical)
                        {
                            if (pIni_offset.z * -1 < pFin_offset.z * -1)
                                coordsFletorZ[2].y *= -1;
                        }
                        coordsFletorZ[2].z = 0;

                        GiraDiagramaConformeAnguloAlfa(ref coordsFletorZ[2], "X");

                        coordsFletorZ[3].y = 0;
                        coordsFletorZ[3].z = 0;

                        if (Geom.Iguais(coordsFletorZ[2].y, 0))
                            coordsFletorZ[2].y = 0;

                        if (Geom.Iguais(coordsFletorZ[1].y, 0))
                            coordsFletorZ[1].y = 0;

                       // RGB_Negativos = RGB_FletoresNegativos;
                      //  RGB_Positivos = RGB_FletoresPositivos;

                        double r1 = 0, r2 = 0, g1 = 0, g2 = 0, b1 = 0, b2 = 0;

                      //  RetRGBEsforco(sinal_mz_ini, sinal_mz_fin, 5, 11, ref r1, ref g1, ref b1, ref r2, ref g2, ref b2,
                          //   tipocarga, caso, comb);
                      
                        RetCorEsforco(sinal_mz_ini, Esforcos[5], ref r1, ref g1, ref b1);
                        RetCorEsforco(sinal_mz_fin, Esforcos[11], ref r2, ref g2, ref b2);

                        for (i = 0; i < coordsFletorZ.Count(); i++)
                        {
                            posicao[1] = coordsFletorZ[i].x;
                            posicao[2] = coordsFletorZ[i].y;
                            posicao[3] = coordsFletorZ[i].z;

                            Geom.rotY(barraOriginal.angXY, ref posicao, ref posicaoFinal);
                            Geom.rotZ(barraOriginal.angXZ, ref posicaoFinal, ref posicaoFinal2);

                            coordsFletorZ[i].x = posicaoFinal2[1] + tx;
                            coordsFletorZ[i].y = posicaoFinal2[2] + ty;
                            coordsFletorZ[i].z = posicaoFinal2[3] + tz;
                        }

                        CriarDoisTriangulos(coordsFletorZ, coords_triangulos, r1, g1, b1, r2, g2, b2);
                    }
                }
            }
            catch (Exception ms)
            {
                System.Windows.Forms.MessageBox.Show("erro ao desenhar MZ:  barra " + this.IDBarra + "  -  " + ms.Message);
            }
        }
        public void Preenche_MY_Gradiente(ref List<float> coords_triangulos, ref double MultiplicadorAltura,
                         int tipocarga, int caso, int comb)
        {
            try
            {
                if (Visivel /*&& barraOriginal.Visivel && !barra_de_articulacao*/ && !barraRigida)
                {

                    if (tipocarga == 0)
                    {
                        Esforcos = casos_x_esforcos[caso].Esforcos;
                    }
                    else
                    {
                        Esforcos = combinacoes_x_esforcos[comb].Esforcos;
                    }

                    if ((Geom.Iguais(Esforcos[6], 0)) && Geom.Iguais(Esforcos[12], 0))
                        return;

                    coordsFletorY[0].x = 0;
                    coordsFletorY[0].y = 0;
                    coordsFletorY[0].z = 0;

                    coordsFletorY[1].x = 0;
                    coordsFletorY[1].y = 0;
                    coordsFletorY[1].z = Esforcos[6] * -1 * MultiplicadorAltura;
                    if (barraOriginal_Vertical)
                        if (pIni.z * -1 < pFin.z * -1)
                            coordsFletorY[1].z *= -1;

                    GiraDiagramaConformeAnguloAlfa(ref coordsFletorY[1], "Z");

                    coordsFletorY[2].x = comprimento;
                    coordsFletorY[3].x = comprimento;

                    if (!Geom.Iguais(barraOriginal.pIni.x, barraOriginal.pFin.x))
                    {
                        if (barraOriginal.pIni.x > barraOriginal.pFin.x)
                        {
                            coordsFletorY[2].x = -comprimento;
                            coordsFletorY[3].x = -comprimento;
                        }
                    }

                    coordsFletorY[2].y = 0;
                    coordsFletorY[2].z = Esforcos[12] * MultiplicadorAltura;

                    if (barraOriginal_Vertical)
                        if (pIni.z * -1 < pFin.z * -1)
                            coordsFletorY[2].z *= -1;

                    GiraDiagramaConformeAnguloAlfa(ref coordsFletorY[2], "Z");

                    coordsFletorY[3].y = 0;
                    coordsFletorY[3].z = 0;

                    if (Geom.Iguais(coordsFletorY[2].y, 0))
                        coordsFletorY[2].y = 0;

                    if (Geom.Iguais(coordsFletorY[1].y, 0))
                        coordsFletorY[1].y = 0;

                    double r1 = 0, r2 = 0, g1 = 0, g2 = 0, b1 = 0, b2 = 0;

                    //    RetRGBEsforco(sinal_my_ini, sinal_my_fin, 6, 12, ref r1, ref g1, ref b1, ref r2, ref g2, ref b2, tipocarga, caso, comb);

                    RetCorEsforco(sinal_my_ini, Esforcos[6], ref r1, ref g1, ref b1);
                    RetCorEsforco(sinal_my_fin, Esforcos[12], ref r2, ref g2, ref b2);

                    for (i = 0; i < coordsFletorY.Count(); i++)
                    {
                        posicao[1] = coordsFletorY[i].x;
                        posicao[2] = coordsFletorY[i].y;
                        posicao[3] = coordsFletorY[i].z;

                        Geom.rotY(barraOriginal.angXY, ref posicao, ref posicaoFinal);
                        Geom.rotY(angXY, ref posicao, ref posicaoFinal);

                        Geom.rotZ(barraOriginal.angXZ, ref posicaoFinal, ref posicaoFinal2);

                        coordsFletorY[i].x = posicaoFinal2[1] + tx;
                        coordsFletorY[i].y = posicaoFinal2[2] + ty;
                        coordsFletorY[i].z = posicaoFinal2[3] + tz;
                    }

                    CriarDoisTriangulos(coordsFletorY, coords_triangulos, r1, g1, b1, r2, g2, b2);
                }
            }
            catch (Exception ms)
            {
                System.Windows.Forms.MessageBox.Show("erro ao desenhar MY:  barra " + this.IDBarra + "  -  " + ms.Message);
            }
        }
        int IndiceRGB;
        public void Preenche_MY(ref List<float> coords_arestas,
    ref double MultiplicadorAltura, ref bool gradiente,
    ref bool Diagrama_preenchido_contorno, ref bool Diagrama_somente_linhas,
    ref bool Diagrama_linha_contorno, ref Color CorArestaDiagrama,
                         int tipocarga, int caso, int comb)
        {
            try
            {
                if (Visivel && /*barraOriginal.Visivel && !barra_de_articulacao &&*/ !barraRigida)
                //  if ((!Geom.Iguais(Esforcos[6], 0)) && !Geom.Iguais(Esforcos[12], 0))
                {
                    if (tipocarga == 0)
                    {
                        Esforcos = casos_x_esforcos[caso].Esforcos;
                    }
                    else
                    {
                        Esforcos = combinacoes_x_esforcos[comb].Esforcos;
                    }

                //    if ((Geom.Iguais(Esforcos[6], 0)) && Geom.Iguais(Esforcos[12], 0))
                  //      return;

                    coordsFletorY[0].x = 0;
                    coordsFletorY[0].y = 0;
                    coordsFletorY[0].z = 0;

                    coordsFletorY[1].x = 0;
                    coordsFletorY[1].y = 0;
                    coordsFletorY[1].z = Esforcos[6] * -1 * MultiplicadorAltura;

                    if (barraOriginal_Vertical)
                        if (pIni.z * -1 < pFin.z * -1)
                            coordsFletorY[1].z *= -1;
                    GiraDiagramaConformeAnguloAlfa(ref coordsFletorY[1], "Z");

                    coordsFletorY[2].x = comprimento;
                    coordsFletorY[3].x = comprimento;

                    if (!Geom.Iguais(barraOriginal.pIni.x, barraOriginal.pFin.x))
                    {
                        if (barraOriginal.pIni.x > barraOriginal.pFin.x)
                        {
                            coordsFletorY[2].x = -comprimento;
                            coordsFletorY[3].x = -comprimento;
                        }
                    }

                    coordsFletorY[2].y = 0;
                    coordsFletorY[2].z = Esforcos[12] * MultiplicadorAltura;

                    if (barraOriginal_Vertical)
                        if (pIni.z * -1 < pFin.z * -1)
                            coordsFletorY[2].z *= -1;

                    GiraDiagramaConformeAnguloAlfa(ref coordsFletorY[2], "Z");

                    coordsFletorY[3].y = 0;
                    coordsFletorY[3].z = 0;

                    double r1 = 0, r2 = 0, g1 = 0, g2 = 0, b1 = 0, b2 = 0;

                    if (gradiente)
                    {
                    //    RetRGBEsforco(sinal_my_ini, sinal_my_fin, 6, 12, ref r1, ref g1, ref b1, ref r2, ref g2, ref b2, tipocarga, caso, comb);
                        RetCorEsforco(sinal_my_ini, Esforcos[6], ref r1, ref g1, ref b1);
                        RetCorEsforco(sinal_my_fin, Esforcos[12], ref r2, ref g2, ref b2);
                    }
                    else
                    {
                        r1 = CorArestaDiagrama.R;
                        g1 = CorArestaDiagrama.G;
                        b1 = CorArestaDiagrama.B;

                        r2 = CorArestaDiagrama.R;
                        g2 = CorArestaDiagrama.G;
                        b2 = CorArestaDiagrama.B;
                    }

                    for (i = 0; i < coordsFletorY.Count(); i++)
                    {
                        posicao[1] = coordsFletorY[i].x;
                        posicao[2] = coordsFletorY[i].y;
                        posicao[3] = coordsFletorY[i].z;

                        Geom.rotY(barraOriginal.angXY, ref posicao, ref posicaoFinal);
                        Geom.rotY(angXY, ref posicao, ref posicaoFinal);

                        Geom.rotZ(barraOriginal.angXZ, ref posicaoFinal, ref posicaoFinal2);

                        coordsFletorY[i].x = posicaoFinal2[1] + tx;
                        coordsFletorY[i].y = posicaoFinal2[2] + ty;
                        coordsFletorY[i].z = posicaoFinal2[3] + tz;
                    }

                    setLista(ref coords_arestas, coordsFletorY[0].x, coordsFletorY[0].y, coordsFletorY[0].z);
                    setLista(ref coords_arestas, r1, g1, b1);
                    setLista(ref coords_arestas, coordsFletorY[1].x, coordsFletorY[1].y, coordsFletorY[1].z);
                    setLista(ref coords_arestas, r1, g1, b1);

                    setLista(ref coords_arestas, coordsFletorY[1].x, coordsFletorY[1].y, coordsFletorY[1].z);
                    setLista(ref coords_arestas, r1, g1, b1);
                    setLista(ref coords_arestas, coordsFletorY[2].x, coordsFletorY[2].y, coordsFletorY[2].z);
                    setLista(ref coords_arestas, r2, g2, b2);

                    setLista(ref coords_arestas, coordsFletorY[2].x, coordsFletorY[2].y, coordsFletorY[2].z);
                    setLista(ref coords_arestas, r2, g2, b2);
                    setLista(ref coords_arestas, coordsFletorY[3].x, coordsFletorY[3].y, coordsFletorY[3].z);
                    setLista(ref coords_arestas, r2, g2, b2);
                }
            }
            catch (Exception ms)
            {
                System.Windows.Forms.MessageBox.Show("erro ao desenhar MY:  barra " + this.IDBarra + "  -  " + ms.Message);
            }
        }

        public void Arestas_FZ(ref List<float> coords_arestas,
ref double MultiplicadorAltura, ref bool gradiente,
ref bool Diagrama_preenchido_contorno, ref bool Diagrama_somente_linhas,
ref bool Diagrama_linha_contorno, ref Color CorArestaDiagrama,
                    int tipocarga, int caso, int comb)
        {
            try
            {
                if (Visivel && /*barraOriginal.Visivel && !barra_de_articulacao && */!barraRigida)
                {
                    if (tipocarga == 0)
                    {
                        Esforcos = casos_x_esforcos[caso].Esforcos;
                    }
                    else
                    {
                        Esforcos = combinacoes_x_esforcos[comb].Esforcos;
                    }

                    if ((Geom.Iguais(Esforcos[2], 0)) && Geom.Iguais(Esforcos[8], 0))
                    {
                        for (int n = 0; n < 4; n++)
                            coordsForca[n] = new vec3(0);

                        return;
                    }

                    try
                    {
                        coordsForca[0].x = 0;
                        coordsForca[0].y = 0;
                        coordsForca[0].z = 0;

                        coordsForca[1].x = 0;
                        coordsForca[1].y = 0;
                        coordsForca[1].z = Esforcos[2] * -1 * MultiplicadorAltura;

                        double c1 = coordsForca[1].z;
                        GiraDiagramaConformeAnguloAlfa(ref coordsForca[1], "Z");

                        coordsForca[2].x = comprimento;
                        coordsForca[3].x = comprimento;

                        if (!Geom.Iguais(pIni.x, pFin.x, 0.00001))
                        {
                            if (pIni.x > pFin.x)
                            {
                                coordsForca[2].x = -comprimento;
                                coordsForca[3].x = -comprimento;
                            }
                        }

                        coordsForca[2].y = 0;
                        coordsForca[2].z = Esforcos[8] * MultiplicadorAltura;

                        double c2 = coordsForca[2].z;
                        GiraDiagramaConformeAnguloAlfa(ref coordsForca[2], "Z");

                        coordsForca[3].y = 0;
                        coordsForca[3].z = 0;

                        double r1 = 0, r2 = 0, g1 = 0, g2 = 0, b1 = 0, b2 = 0;

                        for (i = 0; i < coordsForca.Count(); i++)
                        {
                            posicao[1] = coordsForca[i].x;
                            posicao[2] = coordsForca[i].y;
                            posicao[3] = coordsForca[i].z;

                            Geom.rotY(barraOriginal.angXY, ref posicao, ref posicaoFinal);
                            Geom.rotZ(barraOriginal.angXZ, ref posicaoFinal, ref posicaoFinal2);

                            coordsForca[i].x = posicaoFinal2[1] + tx;
                            coordsForca[i].y = posicaoFinal2[2] + ty;
                            coordsForca[i].z = posicaoFinal2[3] + tz;
                        }

                        if (gradiente)
                        {
                          //  RetRGBEsforco(sinal_fz_ini, sinal_fz_fin, 2, 8, ref r1, ref g1, ref b1, ref r2, ref g2, ref b2, tipocarga, caso, comb);
                            RetCorEsforco(sinal_fz_ini, Esforcos[2], ref r1, ref g1, ref b1);
                            RetCorEsforco(sinal_fz_fin, Esforcos[8], ref r2, ref g2, ref b2);
                        }
                        else
                        {
                            r1 = CorArestaDiagrama.R;
                            g1 = CorArestaDiagrama.G;
                            b1 = CorArestaDiagrama.B;

                            r2 = CorArestaDiagrama.R;
                            g2 = CorArestaDiagrama.G;
                            b2 = CorArestaDiagrama.B;
                        }


                        /* r_a = r1;
                         g_a = g1;
                         b_a = b1;

                         r_b = r2;
                         g_b = g2;
                         b_b = b2;*/

                        setLista(ref coords_arestas, coordsForca[0].x, coordsForca[0].y, coordsForca[0].z);
                        setLista(ref coords_arestas, r1, g1, b1);
                        setLista(ref coords_arestas, coordsForca[1].x, coordsForca[1].y, coordsForca[1].z);
                        setLista(ref coords_arestas, r1, g1, b1);

                        setLista(ref coords_arestas, coordsForca[1].x, coordsForca[1].y, coordsForca[1].z);
                        setLista(ref coords_arestas, r1, g1, b1);
                        setLista(ref coords_arestas, coordsForca[2].x, coordsForca[2].y, coordsForca[2].z);
                        setLista(ref coords_arestas, r2, g2, b2);

                        setLista(ref coords_arestas, coordsForca[2].x, coordsForca[2].y, coordsForca[2].z);
                        setLista(ref coords_arestas, r2, g2, b2);
                        setLista(ref coords_arestas, coordsForca[3].x, coordsForca[3].y, coordsForca[3].z);
                        setLista(ref coords_arestas, r2, g2, b2);

                    }
                    catch (Exception ms)
                    {
                        System.Windows.Forms.MessageBox.Show("erro ao desenhar esforço FZ: barra " + this.IDBarra + "  -  " + ms.Message);
                    }
                }

                else
                {
                    for (int n = 0; n < 4; n++)
                        coordsForca[n] = new vec3(0);
                }

            }
            catch (Exception ms)
            {
                System.Windows.Forms.MessageBox.Show("erro ao desenhar FZ:  barra " + this.IDBarra + "  -  " + ms.Message);
            }
        }
        public void MX_Positivos_Negativos(int tipocarga, int caso, int comb)
        {
            try
            {
                if (Visivel && /*barraOriginal.Visivel && !barra_de_articulacao && */!barraRigida)
                {
                    if (tipocarga == 0)
                    {
                        Esforcos = casos_x_esforcos[caso].Esforcos;
                    }
                    else
                    {
                        Esforcos = combinacoes_x_esforcos[comb].Esforcos;
                    }
                    if ((Geom.Iguais(Esforcos[4], 0)) && Geom.Iguais(Esforcos[10], 0))
                        return;
                    {
                        try
                        {
                            coordsTorcor[0].x = 0;
                            coordsTorcor[0].y = 0;
                            coordsTorcor[0].z = 0;

                            coordsTorcor[1].x = 0;
                            coordsTorcor[1].y = 0;
                            coordsTorcor[1].z = Esforcos[4] * -1;
                            if (barraOriginal_Vertical)
                                if (pIni.z * -1 < pFin.z * -1)
                                    coordsTorcor[1].z *= -1;

                            GiraDiagramaConformeAnguloAlfa(ref coordsTorcor[1], "Z");

                            coordsTorcor[2].x = comprimento;
                            coordsTorcor[3].x = comprimento;

                            if (!Geom.Iguais(barraOriginal.pIni.x, barraOriginal.pFin.x))
                            {
                                if (barraOriginal.pIni.x > barraOriginal.pFin.x)
                                {
                                    coordsTorcor[2].x = -comprimento;
                                    coordsTorcor[3].x = -comprimento;
                                }
                            }

                            coordsTorcor[2].y = 0;
                            coordsTorcor[2].z = Esforcos[10];

                            if (barraOriginal_Vertical)
                                if (pIni.z * -1 < pFin.z * -1)
                                    coordsTorcor[2].z *= -1;

                            GiraDiagramaConformeAnguloAlfa(ref coordsTorcor[2], "Z");

                            coordsTorcor[3].y = 0;
                            coordsTorcor[3].z = 0;

                            for (i = 0; i < coordsTorcor.Count(); i++)
                            {
                                posicao[1] = coordsTorcor[i].x;
                                posicao[2] = coordsTorcor[i].y;
                                posicao[3] = coordsTorcor[i].z;

                                Geom.rotY(barraOriginal.angXY, ref posicao, ref posicaoFinal);
                                Geom.rotZ(barraOriginal.angXZ, ref posicaoFinal, ref posicaoFinal2);

                                coordsTorcor[i].x = posicaoFinal2[1] + tx;
                                coordsTorcor[i].y = posicaoFinal2[2] + ty;
                                coordsTorcor[i].z = posicaoFinal2[3] + tz;
                            }

                            y_local = Geom.CriaVetor(barraOriginal.seta_eixo_local_Z_principal.l_principal.p1, barraOriginal.seta_eixo_local_Z_principal.l_principal.p2, true);

                            vec3 vetorDiagrama;

                            if (Geom.Iguais(Esforcos[4], 0))
                                sinal_mx_ini = 0;
                            else
                            {
                                vetorDiagrama = Geom.CriaVetor(coordsTorcor[0], coordsTorcor[1], true);
                                if (Geom.Iguais(vetorDiagrama.y, 0))
                                    vetorDiagrama.y = 0;
                                if (Geom.Iguais(y_local.y, 0))
                                    y_local.y = 0;

                                angulocarga_Z = Geom.AnguloEntre2Vetores(vetorDiagrama, y_local);

                                if (Geom.Iguais(angulocarga_Z, 0, 0.01))
                                    sinal_mx_ini = 1;
                                else
                                if (Geom.Iguais(angulocarga_Z, 180, 0.01))
                                    sinal_mx_ini = -1;
                            }

                            if (Geom.Iguais(Esforcos[10], 0))
                                sinal_mx_fin = 0;
                            else
                            {
                                vetorDiagrama = Geom.CriaVetor(coordsTorcor[3], coordsTorcor[2], true);

                                if (Geom.Iguais(vetorDiagrama.y, 0))
                                    vetorDiagrama.y = 0;
                                if (Geom.Iguais(y_local.y, 0))
                                    y_local.y = 0;

                                angulocarga_Z = Geom.AnguloEntre2Vetores(vetorDiagrama, y_local);

                                if (Geom.Iguais(angulocarga_Z, 0, 0.01))
                                    sinal_mx_fin = 1;
                                else
                                if (Geom.Iguais(angulocarga_Z, 180, 0.01))
                                    sinal_mx_fin = -1;
                            }
                        }
                        catch (Exception ms)
                        {
                            System.Windows.Forms.MessageBox.Show("erro ao procurar esforço mz positivos e negativos: barra " + this.IDBarra + "  -  " + ms.Message);
                        }
                    }
                }
            }
            catch (Exception ms)
            {
                System.Windows.Forms.MessageBox.Show("erro ao desenhar mz positivo ou negativo:  barra " + this.IDBarra + "  -  " + ms.Message);
            }
        }


        public void MZ_Positivos_Negativos(int tipocarga, int caso, int comb)
        {
            try
            {
                if (Visivel /*&& barraOriginal.Visivel && !barra_de_articulacao*/ && !barraRigida)
                {
                    if (tipocarga == 0)
                    {
                        Esforcos = casos_x_esforcos[caso].Esforcos;
                    }
                    else
                    {
                        Esforcos = combinacoes_x_esforcos[comb].Esforcos;
                    }
                    if ((Geom.Iguais(Esforcos[5], 0)) && Geom.Iguais(Esforcos[11], 0))
                        return;
                    {
                        try
                        {
                            coordsFletorZ[0].x = 0;
                            coordsFletorZ[0].y = 0;
                            coordsFletorZ[0].z = 0;

                            coordsFletorZ[1].x = 0;
                            coordsFletorZ[1].y = Esforcos[5] * -1;
                            coordsFletorZ[1].z = 0;

                            if (barraOriginal_Vertical)
                                if (pIni.z * -1 < pFin.z * -1)
                                    coordsFletorZ[1].y *= -1;

                            coordsFletorZ[2].x = comprimento;
                            coordsFletorZ[3].x = comprimento;

                            coordsFletorZ[2].y = Esforcos[11];

                            if (!Geom.Iguais(barraOriginal.pIni.x, barraOriginal.pFin.x))
                            {
                                if (barraOriginal.pIni.x > barraOriginal.pFin.x)
                                {
                                    coordsFletorZ[2].x = -comprimento;
                                    coordsFletorZ[3].x = -comprimento;

                                    coordsFletorZ[1].y *= -1;
                                    coordsFletorZ[2].y *= -1;
                                }
                            }

                            GiraDiagramaConformeAnguloAlfa(ref coordsFletorZ[1], "X");

                            if (barraOriginal_Vertical)
                            {
                                if (pIni.z * -1 < pFin.z * -1)
                                    coordsFletorZ[2].y *= -1;
                            }

                            coordsFletorZ[2].z = 0;

                            GiraDiagramaConformeAnguloAlfa(ref coordsFletorZ[2], "X");

                            coordsFletorZ[3].y = 0;
                            coordsFletorZ[3].z = 0;

                            if (Geom.Iguais(coordsFletorZ[2].y, 0))
                                coordsFletorZ[2].y = 0;

                            if (Geom.Iguais(coordsFletorZ[1].y, 0))
                                coordsFletorZ[1].y = 0;
                            for (i = 0; i < coordsFletorZ.Count(); i++)
                            {
                                posicao[1] = coordsFletorZ[i].x;
                                posicao[2] = coordsFletorZ[i].y;
                                posicao[3] = coordsFletorZ[i].z;

                                Geom.rotY(barraOriginal.angXY, ref posicao, ref posicaoFinal);
                                Geom.rotZ(barraOriginal.angXZ, ref posicaoFinal, ref posicaoFinal2);

                                coordsFletorZ[i].x = posicaoFinal2[1] + tx;
                                coordsFletorZ[i].y = posicaoFinal2[2] + ty;
                                coordsFletorZ[i].z = posicaoFinal2[3] + tz;
                            }

                            y_local = Geom.CriaVetor(barraOriginal.seta_eixo_local_Y_principal.l_principal.p1, barraOriginal.seta_eixo_local_Y_principal.l_principal.p2, true);

                            vec3 vetorDiagrama;

                            if (Geom.Iguais(Esforcos[5], 0))
                                sinal_mz_ini = 0;
                            else
                            {
                                vetorDiagrama = Geom.CriaVetor(coordsFletorZ[0], coordsFletorZ[1], true);
                                if (Geom.Iguais(vetorDiagrama.y, 0))
                                    vetorDiagrama.y = 0;
                                if (Geom.Iguais(y_local.y, 0))
                                    y_local.y = 0;

                                angulocarga_Z = Geom.AnguloEntre2Vetores(vetorDiagrama, y_local);

                                if (Geom.Iguais(angulocarga_Z, 0, 0.01))
                                    sinal_mz_ini = -1;
                                else
                                if (Geom.Iguais(angulocarga_Z, 180, 0.01))
                                    sinal_mz_ini = 1;
                            }

                            if (Geom.Iguais(Esforcos[11], 0))
                                sinal_mz_fin = 0;
                            else
                            {
                                vetorDiagrama = Geom.CriaVetor(coordsFletorZ[3], coordsFletorZ[2], true);

                                if (Geom.Iguais(vetorDiagrama.y, 0))
                                    vetorDiagrama.y = 0;
                                if (Geom.Iguais(y_local.y, 0))
                                    y_local.y = 0;

                                angulocarga_Z = Geom.AnguloEntre2Vetores(vetorDiagrama, y_local);

                                if (Geom.Iguais(angulocarga_Z, 0, 0.01))
                                    sinal_mz_fin = -1;
                                else
                                if (Geom.Iguais(angulocarga_Z, 180, 0.01))
                                    sinal_mz_fin = 1;
                            }

                            if (!Geom.Iguais(barraOriginal.Dados.secao.propriedades.anguloEixosPrincipais, 0))
                            {
                            //    sinal_mz_fin *= -1;
                            //    sinal_mz_ini *= -1;
                            }
                        }
                        catch (Exception ms)
                        {
                            System.Windows.Forms.MessageBox.Show("erro ao procurar esforço mz positivos e negativos: barra " + this.IDBarra + "  -  " + ms.Message);
                        }
                    }
                }
            }
            catch (Exception ms)
            {
                System.Windows.Forms.MessageBox.Show("erro ao desenhar mz positivo ou negativo:  barra " + this.IDBarra + "  -  " + ms.Message);
            }
        }
        public void MY_Positivos_Negativos(int tipocarga, int caso, int comb)
        {
            try
            {
                if (Visivel /*&& barraOriginal.Visivel && !barra_de_articulacao*/ && !barraRigida)
                {
                    if (tipocarga == 0)
                    {
                        Esforcos = casos_x_esforcos[caso].Esforcos;
                    }
                    else
                    {
                        Esforcos = combinacoes_x_esforcos[comb].Esforcos;
                    }
                   // if ((Geom.Iguais(Esforcos[6], 0)) && Geom.Iguais(Esforcos[12], 0))
                   //     return;
                    {
                        try
                        {
                            coordsFletorY[0].x = 0;
                            coordsFletorY[0].y = 0;
                            coordsFletorY[0].z = 0;

                            coordsFletorY[1].x = 0;
                            coordsFletorY[1].y = 0;
                            coordsFletorY[1].z = Esforcos[6] * -1;

                            if (barraOriginal_Vertical)
                                if (pIni.z * -1 < pFin.z * -1)
                                    coordsFletorY[1].z *= -1;

                            GiraDiagramaConformeAnguloAlfa(ref coordsFletorY[1], "Z");

                            coordsFletorY[2].x = comprimento;
                            coordsFletorY[3].x = comprimento;

                            if (!Geom.Iguais(barraOriginal.pIni.x, barraOriginal.pFin.x))
                            {
                                if (barraOriginal.pIni.x > barraOriginal.pFin.x)
                                {
                                    coordsFletorY[2].x = -comprimento;
                                    coordsFletorY[3].x = -comprimento;
                                }
                            }

                            coordsFletorY[2].y = 0;
                            coordsFletorY[2].z = Esforcos[12];

                            if (barraOriginal_Vertical)
                                if (pIni.z * -1 < pFin.z * -1)
                                    coordsFletorY[2].z *= -1;

                            GiraDiagramaConformeAnguloAlfa(ref coordsFletorY[2], "Z");

                            coordsFletorY[3].y = 0;
                            coordsFletorY[3].z = 0;

                            for (i = 0; i < coordsFletorY.Count(); i++)
                            {
                                posicao[1] = coordsFletorY[i].x;
                                posicao[2] = coordsFletorY[i].y;
                                posicao[3] = coordsFletorY[i].z;

                                Geom.rotY(barraOriginal.angXY, ref posicao, ref posicaoFinal);
                                Geom.rotY(angXY, ref posicao, ref posicaoFinal);

                                Geom.rotZ(barraOriginal.angXZ, ref posicaoFinal, ref posicaoFinal2);

                                coordsFletorY[i].x = posicaoFinal2[1] + tx;
                                coordsFletorY[i].y = posicaoFinal2[2] + ty;
                                coordsFletorY[i].z = posicaoFinal2[3] + tz;
                            }

                            y_local = Geom.CriaVetor(barraOriginal.seta_eixo_local_Z_principal.l_principal.p1, barraOriginal.seta_eixo_local_Z_principal.l_principal.p2, true);

                            vec3 vetorDiagrama;

                            if (Geom.Iguais(Esforcos[6], 0))
                                sinal_my_ini = 0;
                            else
                            {
                                vetorDiagrama = Geom.CriaVetor(coordsFletorY[0], coordsFletorY[1], true);
                                if (Geom.Iguais(vetorDiagrama.y, 0))
                                    vetorDiagrama.y = 0;
                                if (Geom.Iguais(y_local.y, 0))
                                    y_local.y = 0;

                                angulocarga_Z = Geom.AnguloEntre2Vetores(vetorDiagrama, y_local);

                                if (Geom.Iguais(angulocarga_Z, 0, 0.01))
                                    sinal_my_ini = -1;
                                else
                                if (Geom.Iguais(angulocarga_Z, 180, 0.01))
                                    sinal_my_ini = 1;
                            }

                            if (Geom.Iguais(Esforcos[12], 0))
                                sinal_my_fin = 0;
                            else
                            {
                                vetorDiagrama = Geom.CriaVetor(coordsFletorY[3], coordsFletorY[2], true);

                                if (Geom.Iguais(vetorDiagrama.y, 0))
                                    vetorDiagrama.y = 0;
                                if (Geom.Iguais(y_local.y, 0))
                                    y_local.y = 0;

                                angulocarga_Z = Geom.AnguloEntre2Vetores(vetorDiagrama, y_local);

                                if (Geom.Iguais(angulocarga_Z, 0,0.01))
                                    sinal_my_fin = -1;
                                else
                                if (Geom.Iguais(angulocarga_Z, 180, 0.01))
                                    sinal_my_fin = 1;
                            }
                        }
                        catch (Exception ms)
                        {
                            System.Windows.Forms.MessageBox.Show("erro ao procurar esforço my positivos e negativos: barra " + this.IDBarra + "  -  " + ms.Message);
                        }
                    }
                }
            }
            catch (Exception ms)
            {
                System.Windows.Forms.MessageBox.Show("erro ao desenhar my positivo ou negativo:  barra " + this.IDBarra + "  -  " + ms.Message);
            }
        }
        public void FX_Positivos_Negativos(int tipocarga, int caso, int comb)
        {
            try
            {
                if (Visivel /*&& barraOriginal.Visivel && !barra_de_articulacao*/ && !barraRigida)
                {
                    if (tipocarga == 0)
                    {
                        Esforcos = casos_x_esforcos[caso].Esforcos;
                    }
                    else
                    {
                        Esforcos = combinacoes_x_esforcos[comb].Esforcos;
                    }
                    if ((Geom.Iguais(Esforcos[1], 0)) && Geom.Iguais(Esforcos[7], 0))
                        return;
                    {
                        try
                        {
                            coordsForca[0].x = 0;
                            coordsForca[0].y = 0;
                            coordsForca[0].z = 0;

                            coordsForca[1].x = 0;
                            coordsForca[1].y = 0;
                            coordsForca[1].z = Esforcos[1];

                            if (barraOriginal_Vertical)
                                if (pIni.z * -1 < pFin.z * -1)
                                    coordsForca[1].z *= -1;

                            GiraDiagramaConformeAnguloAlfa(ref coordsForca[1], "Z");

                            coordsForca[2].x = comprimento;
                            coordsForca[3].x = comprimento;

                            if (!Geom.Iguais(barraOriginal.pIni.x, barraOriginal.pFin.x, 0.00001))
                            {
                                if (barraOriginal.pIni.x > barraOriginal.pFin.x)
                                {
                                    coordsForca[2].x = -comprimento;
                                    coordsForca[3].x = -comprimento;
                                }
                            }

                            coordsForca[2].y = 0;
                            coordsForca[2].z = Esforcos[7] * -1;

                            if (barraOriginal_Vertical)
                                if (pIni.z * -1 < pFin.z * -1)
                                    coordsForca[2].z *= -1;

                            GiraDiagramaConformeAnguloAlfa(ref coordsForca[2], "Z");

                            coordsForca[3].y = 0;
                            coordsForca[3].z = 0;

                            for (i = 0; i < coordsForca.Count(); i++)
                            {
                                posicao[1] = coordsForca[i].x;
                                posicao[2] = coordsForca[i].y;
                                posicao[3] = coordsForca[i].z;

                                Geom.rotY(barraOriginal.angXY, ref posicao, ref posicaoFinal);
                                Geom.rotZ(barraOriginal.angXZ, ref posicaoFinal, ref posicaoFinal2);

                                coordsForca[i].x = posicaoFinal2[1] + tx;
                                coordsForca[i].y = posicaoFinal2[2] + ty;
                                coordsForca[i].z = posicaoFinal2[3] + tz;
                            }

                            z_local = Geom.CriaVetor(barraOriginal.seta_eixo_local_Z_principal.l_principal.p1, barraOriginal.seta_eixo_local_Z_principal.l_principal.p2, true);

                            vec3 vetorDiagrama;

                            if (Geom.Iguais(Esforcos[1], 0))
                                sinal_fx_ini = 0;
                            else
                            {
                                vetorDiagrama = Geom.CriaVetor(coordsForca[0], coordsForca[1], true);
                                if (Geom.Iguais(vetorDiagrama.y, 0))
                                    vetorDiagrama.y = 0;
                                if (Geom.Iguais(z_local.y, 0))
                                    z_local.y = 0;

                                angulocarga_Z = Geom.AnguloEntre2Vetores(vetorDiagrama, z_local);

                                if (Geom.Iguais(angulocarga_Z, 0, 0.01))
                                    sinal_fx_ini = 1;
                                else
                                if (Geom.Iguais(angulocarga_Z, 180,0.01))
                                    sinal_fx_ini = -1;
                            }

                            if (Geom.Iguais(Esforcos[7], 0))
                                sinal_fx_fin = 0;
                            else
                            {
                                vetorDiagrama = Geom.CriaVetor(coordsForca[3], coordsForca[2], true);

                                if (Geom.Iguais(vetorDiagrama.y, 0))
                                    vetorDiagrama.y = 0;
                                if (Geom.Iguais(z_local.y, 0))
                                    z_local.y = 0;

                                angulocarga_Z = Geom.AnguloEntre2Vetores(vetorDiagrama, z_local);

                                if (Geom.Iguais(angulocarga_Z, 0, 0.01))
                                    sinal_fx_fin = 1;
                                else
                                if (Geom.Iguais(angulocarga_Z, 180, 0.01))
                                    sinal_fx_fin = -1;
                            }
                        }
                        catch (Exception ms)
                        {
                            System.Windows.Forms.MessageBox.Show("erro ao procurar esforço fx positivos e negativos: barra " + this.IDBarra + "  -  " + ms.Message);
                        }
                    }
                }
            }
            catch (Exception ms)
            {
                System.Windows.Forms.MessageBox.Show("erro ao desenhar fx:  barra " + this.IDBarra + "  -  " + ms.Message);
            }
        }

        public void FY_Positivos_Negativos(int tipocarga, int caso, int comb)
        {
            try
            {
                if (Visivel && /*barraOriginal.Visivel && !barra_de_articulacao &&*/ !barraRigida)
                {
                    if (tipocarga == 0)
                    {
                        Esforcos = casos_x_esforcos[caso].Esforcos;
                    }
                    else
                    {
                        Esforcos = combinacoes_x_esforcos[comb].Esforcos;
                    }
                    if ((Geom.Iguais(Esforcos[3], 0)) && Geom.Iguais(Esforcos[9], 0))
                        return;
                    {
                        try
                        {
                            coordsForca[0].x = 0;
                            coordsForca[0].y = 0;
                            coordsForca[0].z = 0;

                            coordsForca[1].x = 0;
                            coordsForca[1].y = Esforcos[3];
                            coordsForca[1].z = 0;

                            GiraDiagramaConformeAnguloAlfa(ref coordsForca[1], "X");

                            coordsForca[2].x = comprimento;
                            coordsForca[3].x = comprimento;

                            if (!Geom.Iguais(pIni.x, pFin.x))
                            {
                                if (pIni.x > pFin.x)
                                {
                                    coordsForca[2].x = -comprimento;
                                    coordsForca[3].x = -comprimento;
                                }
                            }

                            coordsForca[2].y = Esforcos[9];
                            coordsForca[2].z = 0;

                            GiraDiagramaConformeAnguloAlfa(ref coordsForca[2], "X");

                            coordsForca[3].y = 0;
                            coordsForca[3].z = 0;

                            for (i = 0; i < coordsForca.Count(); i++)
                            {
                                posicao[1] = coordsForca[i].x;
                                posicao[2] = coordsForca[i].y;
                                posicao[3] = coordsForca[i].z;

                                Geom.rotY(barraOriginal.angXY, ref posicao, ref posicaoFinal);
                                Geom.rotZ(barraOriginal.angXZ, ref posicaoFinal, ref posicaoFinal2);

                                coordsForca[i].x = posicaoFinal2[1] + tx;
                                coordsForca[i].y = posicaoFinal2[2] + ty;
                                coordsForca[i].z = posicaoFinal2[3] + tz;
                            }

                            y_local = Geom.CriaVetor(barraOriginal.seta_eixo_local_Y_principal.l_principal.p1, barraOriginal.seta_eixo_local_Y_principal.l_principal.p2, true);

                            vec3 vetorDiagrama;

                            if (Geom.Iguais(Esforcos[3], 0))
                                sinal_fy_ini = 0;
                            else
                            {
                                vetorDiagrama = Geom.CriaVetor(coordsForca[0], coordsForca[1], true);
                                if (Geom.Iguais(vetorDiagrama.y, 0))
                                    vetorDiagrama.y = 0;
                                if (Geom.Iguais(y_local.y, 0))
                                    y_local.y = 0;

                                angulocarga_Z = Geom.AnguloEntre2Vetores(vetorDiagrama, y_local);

                                if (Geom.Iguais(angulocarga_Z, 0, 0.01))
                                    sinal_fy_ini = 1;
                                else
                                if (Geom.Iguais(angulocarga_Z, 180, 0.01))
                                    sinal_fy_ini = -1;
                            }

                            if (Geom.Iguais(Esforcos[9], 0))
                                sinal_fy_fin = 0;
                            else
                            {
                                vetorDiagrama = Geom.CriaVetor(coordsForca[2], coordsForca[3], true);

                                if (Geom.Iguais(vetorDiagrama.y, 0))
                                    vetorDiagrama.y = 0;
                                if (Geom.Iguais(y_local.y, 0))
                                    y_local.y = 0;

                                angulocarga_Z = Geom.AnguloEntre2Vetores(vetorDiagrama, y_local);

                                if (Geom.Iguais(angulocarga_Z, 0, 0.01))
                                    sinal_fy_fin = 1;
                                else
                                if (Geom.Iguais(angulocarga_Z, 180, 0.01))
                                    sinal_fy_fin = -1;
                            }
                        }
                        catch (Exception ms)
                        {
                            System.Windows.Forms.MessageBox.Show("erro ao procurar esforço fy positivos e negativos: barra " + this.IDBarra + "  -  " + ms.Message);
                        }
                    }
                }
            }
            catch (Exception ms)
            {
                System.Windows.Forms.MessageBox.Show("erro ao desenhar fy positivo ou negativo:  barra " + this.IDBarra + "  -  " + ms.Message);
            }
        }
        public void FZ_Positivos_Negativos(int tipocarga, int caso, int comb)
        {
            try
            {
                if (Visivel /*&& barraOriginal.Visivel && !barra_de_articulacao */&& !barraRigida)
                {
                    if (tipocarga == 0)
                    {
                        Esforcos = casos_x_esforcos[caso].Esforcos;
                    }
                    else
                    {
                        Esforcos = combinacoes_x_esforcos[comb].Esforcos;
                    }
                    if ((Geom.Iguais(Esforcos[2], 0)) && Geom.Iguais(Esforcos[8], 0))
                        return;
                    //  if ((!Geom.Iguais(Esforcos[2], 0)) && !Geom.Iguais(Esforcos[8], 0))
                    {
                        try
                        {
                            coordsForca[0].x = 0;
                            coordsForca[0].y = 0;
                            coordsForca[0].z = 0;

                            coordsForca[1].x = 0;
                            coordsForca[1].y = 0;
                            coordsForca[1].z = Esforcos[2] * -1;

                            GiraDiagramaConformeAnguloAlfa(ref coordsForca[1], "Z");

                            coordsForca[2].x = comprimento;
                            coordsForca[3].x = comprimento;

                            if (!Geom.Iguais(barraOriginal.pIni.x, barraOriginal.pFin.x))
                            {
                                if (barraOriginal.pIni.x > barraOriginal.pFin.x)
                                {
                                    coordsForca[2].x = -comprimento;
                                    coordsForca[3].x = -comprimento;
                                }
                            }

                            coordsForca[2].y = 0;
                            coordsForca[2].z = Esforcos[8];

                            GiraDiagramaConformeAnguloAlfa(ref coordsForca[2], "Z");

                            coordsForca[3].y = 0;
                            coordsForca[3].z = 0;

                            for (i = 0; i < coordsForca.Count(); i++)
                            {
                                posicao[1] = coordsForca[i].x;
                                posicao[2] = coordsForca[i].y;
                                posicao[3] = coordsForca[i].z;

                                Geom.rotY(barraOriginal.angXY, ref posicao, ref posicaoFinal);
                                Geom.rotZ(barraOriginal.angXZ, ref posicaoFinal, ref posicaoFinal2);

                                coordsForca[i].x = posicaoFinal2[1] + tx;
                                coordsForca[i].y = posicaoFinal2[2] + ty;
                                coordsForca[i].z = posicaoFinal2[3] + tz;
                            }

                            z_local = Geom.CriaVetor(barraOriginal.seta_eixo_local_Z_principal.l_principal.p1, barraOriginal.seta_eixo_local_Z_principal.l_principal.p2, true);

                            if (Geom.Iguais(Esforcos[2], 0))
                                sinal_fz_ini = 0;
                            else
                            {
                                vec3 vetorDiagrama = Geom.CriaVetor(coordsForca[0], coordsForca[1], true);
                                if (Geom.Iguais(vetorDiagrama.y, 0))
                                    vetorDiagrama.y = 0;
                                if (Geom.Iguais(z_local.y, 0))
                                    z_local.y = 0;
                                angulocarga_Z = Geom.AnguloEntre2Vetores(vetorDiagrama, z_local);

                                if (Geom.Iguais(angulocarga_Z, 0, 0.01))
                                    sinal_fz_ini = 1;
                                else
                                if (Geom.Iguais(angulocarga_Z, 180, 0.01))
                                    sinal_fz_ini = -1;
                            }

                            if (Geom.Iguais(Esforcos[8], 0))
                                sinal_fz_fin = 0;
                            else
                            {
                                vec3 vetorDiagrama = Geom.CriaVetor(coordsForca[3], coordsForca[2], true);
                                if (Geom.Iguais(vetorDiagrama.y, 0))
                                    vetorDiagrama.y = 0;
                                if (Geom.Iguais(z_local.y, 0))
                                    z_local.y = 0;
                                angulocarga_Z = Geom.AnguloEntre2Vetores(vetorDiagrama, z_local);

                                if (Geom.Iguais(angulocarga_Z, 0, 0.01))
                                    sinal_fz_fin = 1;
                                else
                                if (Geom.Iguais(angulocarga_Z, 180, 0.01))
                                    sinal_fz_fin = -1;
                            }
                        }
                        catch (Exception ms)
                        {
                            System.Windows.Forms.MessageBox.Show("erro ao procurar esforço fz positivos e negativos: barra " + this.IDBarra + "  -  " + ms.Message);
                        }
                    }
                }
            }
            catch (Exception ms)
            {
                System.Windows.Forms.MessageBox.Show("erro ao desenhar fz:  barra " + this.IDBarra + "  -  " + ms.Message);
            }
        }
        double angulocarga_Z;
        public void Preenche_FZ_Gradiente(ref List<float> coords_triangulos, ref double MultiplicadorAltura,
                         int tipocarga, int caso, int comb)
        {
            try
            {
                if (Visivel /*&& barraOriginal.Visivel && !barra_de_articulacao*/ && !barraRigida)
                {
                    if (tipocarga == 0)
                    {
                        Esforcos = casos_x_esforcos[caso].Esforcos;
                    }
                    else
                    {
                        Esforcos = combinacoes_x_esforcos[comb].Esforcos;
                    }

                    if ((Geom.Iguais(Esforcos[2], 0)) && Geom.Iguais(Esforcos[8], 0))
                        return;


                    {
                        try
                        {
                            coordsForca[0].x = 0;
                            coordsForca[0].y = 0;
                            coordsForca[0].z = 0;

                            coordsForca[1].x = 0;
                            coordsForca[1].y = 0;
                            coordsForca[1].z = Esforcos[2] * -1 * MultiplicadorAltura;

                            double c1 = coordsForca[1].z;
                            GiraDiagramaConformeAnguloAlfa(ref coordsForca[1], "Z");

                            coordsForca[2].x = comprimento;
                            coordsForca[3].x = comprimento;

                            if (!Geom.Iguais(barraOriginal.pIni.x, barraOriginal.pFin.x))
                            {
                                if (barraOriginal.pIni.x > barraOriginal.pFin.x)
                                {
                                    coordsForca[2].x = -comprimento;
                                    coordsForca[3].x = -comprimento;
                                }
                            }

                            coordsForca[2].y = 0;
                            coordsForca[2].z = Esforcos[8] * MultiplicadorAltura;

                            double c2 = coordsForca[2].z;

                            GiraDiagramaConformeAnguloAlfa(ref coordsForca[2], "Z");

                            coordsForca[3].y = 0;
                            coordsForca[3].z = 0;

                            double r1 = 0, r2 = 0, g1 = 0, g2 = 0, b1 = 0, b2 = 0;

                            //     RetRGB(c1, c2, 2, 8, ref r1, ref g1, ref b1, ref r2, ref g2, ref b2, tipocarga, caso, comb);
                         //   RetRGBEsforco(sinal_fz_ini, sinal_fz_fin, 2, 8, ref r1, ref g1, ref b1, ref r2, ref g2, ref b2, tipocarga, caso, comb);
                            RetCorEsforco(sinal_fz_ini, Esforcos[2], ref r1, ref g1, ref b1);
                            RetCorEsforco(sinal_fz_fin, Esforcos[8], ref r2, ref g2, ref b2);
                          
                            for (i = 0; i < coordsForca.Count(); i++)
                            {
                                posicao[1] = coordsForca[i].x;
                                posicao[2] = coordsForca[i].y;
                                posicao[3] = coordsForca[i].z;

                                Geom.rotY(barraOriginal.angXY, ref posicao, ref posicaoFinal);
                                Geom.rotZ(barraOriginal.angXZ, ref posicaoFinal, ref posicaoFinal2);

                                coordsForca[i].x = posicaoFinal2[1] + tx;
                                coordsForca[i].y = posicaoFinal2[2] + ty;
                                coordsForca[i].z = posicaoFinal2[3] + tz;
                            }

                            CriarDoisTriangulos(coordsForca, coords_triangulos, r1, g1, b1, r2, g2, b2);
                        }
                        catch (Exception ms)
                        {
                            System.Windows.Forms.MessageBox.Show("erro ao desenhar esforço fz: barra " + this.IDBarra + "  -  " + ms.Message);
                        }
                    }
                }
            }
            catch (Exception ms)
            {
                System.Windows.Forms.MessageBox.Show("erro ao desenhar fz:  barra " + this.IDBarra + "  -  " + ms.Message);
            }
        }

        //sinaliza se o esforço é positivo, negativo ou zero: -1, +1, 0
        public double sinal_fz_ini, sinal_fz_fin;
        public double sinal_fy_ini, sinal_fy_fin;
        public double sinal_fx_ini, sinal_fx_fin;
        public double sinal_mz_ini, sinal_mz_fin;
        public double sinal_my_ini, sinal_my_fin;
        public double sinal_mx_ini, sinal_mx_fin;


        public void Preenche_FX_Gradiente(ref List<float> coords_triangulos, ref double MultiplicadorAltura,
                         int tipocarga, int caso, int comb)
        {
            try
            {
                if (Visivel /*&& barraOriginal.Visivel && !barra_de_articulacao*/ && !barraRigida)
                {
                    if (tipocarga == 0)
                    {
                        Esforcos = casos_x_esforcos[caso].Esforcos;
                    }
                    else
                    {
                        Esforcos = combinacoes_x_esforcos[comb].Esforcos;
                    }

                    if ((Geom.Iguais(Esforcos[1], 0)) && Geom.Iguais(Esforcos[7], 0))
                        return;


                    {
                        try
                        {
                            coordsForca[0].x = 0;
                            coordsForca[0].y = 0;
                            coordsForca[0].z = 0;

                            coordsForca[1].x = 0;
                            coordsForca[1].y = 0;
                            coordsForca[1].z = Esforcos[1] * MultiplicadorAltura;

                            if (barraOriginal_Vertical)
                                if (pIni.z * -1 < pFin.z * -1)
                                    coordsForca[1].z *= -1;

                            GiraDiagramaConformeAnguloAlfa(ref coordsForca[1], "Z");

                            coordsForca[2].x = comprimento;
                            coordsForca[3].x = comprimento;

                            if (!Geom.Iguais(barraOriginal.pIni.x, barraOriginal.pFin.x))
                            {
                                if (barraOriginal.pIni.x > barraOriginal.pFin.x)
                                {
                                    coordsForca[2].x = -comprimento;
                                    coordsForca[3].x = -comprimento;
                                }
                            }

                            coordsForca[2].y = 0;
                            coordsForca[2].z = Esforcos[7] * -1 * MultiplicadorAltura;

                            if (barraOriginal_Vertical)
                                if (pIni.z * -1 < pFin.z * -1)
                                    coordsForca[2].z *= -1;

                            GiraDiagramaConformeAnguloAlfa(ref coordsForca[2], "Z");

                            coordsForca[3].y = 0;
                            coordsForca[3].z = 0;

                            double r1 = 0, r2 = 0, g1 = 0, g2 = 0, b1 = 0, b2 = 0;

                       //     RetRGBEsforco(sinal_fx_ini, sinal_fx_fin, 1, 7, ref r1, ref g1, ref b1, ref r2, ref g2, ref b2, tipocarga, caso, comb);
                            RetCorEsforco(sinal_fx_ini, Esforcos[1], ref r1, ref g1, ref b1);
                            RetCorEsforco(sinal_fx_fin, Esforcos[7], ref r2, ref g2, ref b2);
                            
                            for (i = 0; i < coordsForca.Count(); i++)
                            {
                                posicao[1] = coordsForca[i].x;
                                posicao[2] = coordsForca[i].y;
                                posicao[3] = coordsForca[i].z;

                                Geom.rotY(barraOriginal.angXY, ref posicao, ref posicaoFinal);
                                Geom.rotZ(barraOriginal.angXZ, ref posicaoFinal, ref posicaoFinal2);

                                coordsForca[i].x = posicaoFinal2[1] + tx;
                                coordsForca[i].y = posicaoFinal2[2] + ty;
                                coordsForca[i].z = posicaoFinal2[3] + tz;
                            }

                            CriarDoisTriangulos(coordsForca, coords_triangulos, r1, g1, b1, r2, g2, b2);
                        }
                        catch (Exception ms)
                        {
                            System.Windows.Forms.MessageBox.Show("erro ao desenhar esforço axial: barra " + this.IDBarra + "  -  " + ms.Message);
                        }
                    }
                }
            }
            catch (Exception ms)
            {
                System.Windows.Forms.MessageBox.Show("erro ao desenhar fx:  barra " + this.IDBarra + "  -  " + ms.Message);
            }
        }

        public void Arestas_FX(ref List<float> coords_arestas,
                    ref double MultiplicadorAltura, ref bool gradiente,
                    ref bool Diagrama_preenchido_contorno, ref bool Diagrama_somente_linhas,
                    ref bool Diagrama_linha_contorno, ref Color CorArestaDiagrama,
                         int tipocarga, int caso, int comb)
        {
            try
            {
                if (Visivel /*&& barraOriginal.Visivel && !barra_de_articulacao*/ && !barraRigida)
                {
                    if (tipocarga == 0)
                    {
                        Esforcos = casos_x_esforcos[caso].Esforcos;
                    }
                    else
                    {
                        Esforcos = combinacoes_x_esforcos[comb].Esforcos;
                    }

                    if ((Geom.Iguais(Esforcos[1], 0)) && Geom.Iguais(Esforcos[7], 0))
                    {
                        for (int n = 0; n < 4; n++)
                            coordsForca[n] = new vec3(0);

                        return;
                    }

                    {
                        try
                        {
                            coordsForca[0].x = 0;
                            coordsForca[0].y = 0;
                            coordsForca[0].z = 0;

                            coordsForca[1].x = 0;
                            coordsForca[1].y = 0;
                            coordsForca[1].z = Esforcos[1] * MultiplicadorAltura;

                            if (barraOriginal_Vertical)
                                if (pIni.z * -1 < pFin.z * -1)
                                    coordsForca[1].z *= -1;

                            GiraDiagramaConformeAnguloAlfa(ref coordsForca[1], "Z");

                            coordsForca[2].x = comprimento;
                            coordsForca[3].x = comprimento;

                            if (!Geom.Iguais(barraOriginal.pIni.x, barraOriginal.pFin.x, 0.00001))
                            {
                                if (barraOriginal.pIni.x > barraOriginal.pFin.x)
                                {
                                    coordsForca[2].x = -comprimento;
                                    coordsForca[3].x = -comprimento;
                                }
                            }

                            coordsForca[2].y = 0;
                            coordsForca[2].z = Esforcos[7] * -1 * MultiplicadorAltura;

                            if (barraOriginal_Vertical)
                                if (pIni.z * -1 < pFin.z * -1)
                                    coordsForca[2].z *= -1;

                            GiraDiagramaConformeAnguloAlfa(ref coordsForca[2], "Z");

                            coordsForca[3].y = 0;
                            coordsForca[3].z = 0;

                            double r1 = 0, r2 = 0, g1 = 0, g2 = 0, b1 = 0, b2 = 0;

                            if (gradiente)
                            {
                              //  RetRGBEsforco(sinal_fx_ini, sinal_fx_fin, 1, 7, ref r1, ref g1, ref b1, ref r2, ref g2, ref b2, tipocarga, caso, comb);
                               
                                RetCorEsforco(sinal_fx_ini, Esforcos[1], ref r1, ref g1, ref b1);
                                RetCorEsforco(sinal_fx_fin, Esforcos[7], ref r2, ref g2, ref b2);
                            }
                            else
                            {
                                r1 = CorArestaDiagrama.R;
                                g1 = CorArestaDiagrama.G;
                                b1 = CorArestaDiagrama.B;

                                r2 = CorArestaDiagrama.R;
                                g2 = CorArestaDiagrama.G;
                                b2 = CorArestaDiagrama.B;
                            }

                            for (i = 0; i < coordsForca.Count(); i++)
                            {
                                posicao[1] = coordsForca[i].x;
                                posicao[2] = coordsForca[i].y;
                                posicao[3] = coordsForca[i].z;

                                Geom.rotY(barraOriginal.angXY, ref posicao, ref posicaoFinal);
                                Geom.rotZ(barraOriginal.angXZ, ref posicaoFinal, ref posicaoFinal2);

                                coordsForca[i].x = posicaoFinal2[1] + tx;
                                coordsForca[i].y = posicaoFinal2[2] + ty;
                                coordsForca[i].z = posicaoFinal2[3] + tz;
                            }

                            setLista(ref coords_arestas, coordsForca[0].x, coordsForca[0].y, coordsForca[0].z);
                            setLista(ref coords_arestas, r1, g1, b1);
                            setLista(ref coords_arestas, coordsForca[1].x, coordsForca[1].y, coordsForca[1].z);
                            setLista(ref coords_arestas, r1, g1, b1);

                            setLista(ref coords_arestas, coordsForca[1].x, coordsForca[1].y, coordsForca[1].z);
                            setLista(ref coords_arestas, r1, g1, b1);
                            setLista(ref coords_arestas, coordsForca[2].x, coordsForca[2].y, coordsForca[2].z);
                            setLista(ref coords_arestas, r2, g2, b2);

                            setLista(ref coords_arestas, coordsForca[2].x, coordsForca[2].y, coordsForca[2].z);
                            setLista(ref coords_arestas, r2, g2, b2);
                            setLista(ref coords_arestas, coordsForca[3].x, coordsForca[3].y, coordsForca[3].z);
                            setLista(ref coords_arestas, r2, g2, b2);
                        }
                        catch (Exception ms)
                        {
                            System.Windows.Forms.MessageBox.Show("erro ao desenhar esforço axial: barra " + this.IDBarra + "  -  " + ms.Message);
                        }
                    }

                }
                else
                {
                    for (int n = 0; n < 4; n++)
                        coordsForca[n] = new vec3(0);
                }
            }
            catch (Exception ms)
            {
                System.Windows.Forms.MessageBox.Show("erro ao desenhar fx:  barra " + this.IDBarra + "  -  " + ms.Message);
            }
        }

        public void Preenche_FY_Gradiente(ref List<float> coords_triangulos, ref double MultiplicadorAltura,
                         int tipocarga, int caso, int comb)
        {
            try
            {
                if (Visivel /*&& barraOriginal.Visivel && !barra_de_articulacao*/ && !barraRigida)
                {
                    if (tipocarga == 0)
                    {
                        Esforcos = casos_x_esforcos[caso].Esforcos;
                    }
                    else
                    {
                        Esforcos = combinacoes_x_esforcos[comb].Esforcos;
                    }

                    if ((Geom.Iguais(Esforcos[3], 0)) && Geom.Iguais(Esforcos[9], 0))
                        return;


                    {
                        try
                        {
                            coordsForca[0].x = 0;
                            coordsForca[0].y = 0;
                            coordsForca[0].z = 0;

                            coordsForca[1].x = 0;
                            coordsForca[1].y = Esforcos[3] * MultiplicadorAltura;
                            coordsForca[1].z = 0;

                            //   double c1 = coordsForca[1].y;
                            GiraDiagramaConformeAnguloAlfa(ref coordsForca[1], "X");

                            coordsForca[2].x = comprimento;
                            coordsForca[3].x = comprimento;

                            if (!Geom.Iguais(barraOriginal.pIni.x, barraOriginal.pFin.x))
                            {
                                if (barraOriginal.pIni.x > barraOriginal.pFin.x)
                                {
                                    coordsForca[2].x = -comprimento;
                                    coordsForca[3].x = -comprimento;
                                }
                            }

                            coordsForca[2].y = Esforcos[9] * -1 * MultiplicadorAltura;
                            coordsForca[2].z = 0;

                            //   double c2 = coordsForca[2].y;

                            GiraDiagramaConformeAnguloAlfa(ref coordsForca[2], "X");

                            coordsForca[3].y = 0;
                            coordsForca[3].z = 0;

                            double r1 = 0, r2 = 0, g1 = 0, g2 = 0, b1 = 0, b2 = 0;

//                            RetRGBEsforco(sinal_fy_ini, sinal_fy_fin, 3, 9, ref r1, ref g1, ref b1, ref r2, ref g2, ref b2, tipocarga, caso, comb);
                            RetCorEsforco(sinal_fy_ini, Esforcos[3], ref r1, ref g1, ref b1);
                            RetCorEsforco(sinal_fy_fin, Esforcos[9], ref r2, ref g2, ref b2);
                           
                            for (i = 0; i < coordsForca.Count(); i++)
                            {
                                posicao[1] = coordsForca[i].x;
                                posicao[2] = coordsForca[i].y;
                                posicao[3] = coordsForca[i].z;

                                Geom.rotY(barraOriginal.angXY, ref posicao, ref posicaoFinal);
                                Geom.rotZ(barraOriginal.angXZ, ref posicaoFinal, ref posicaoFinal2);

                                coordsForca[i].x = posicaoFinal2[1] + tx;
                                coordsForca[i].y = posicaoFinal2[2] + ty;
                                coordsForca[i].z = posicaoFinal2[3] + tz;
                            }

                            CriarDoisTriangulos(coordsForca, coords_triangulos, r1, g1, b1, r2, g2, b2);
                        }
                        catch (Exception ms)
                        {
                            System.Windows.Forms.MessageBox.Show("erro ao desenhar esforço fy: barra " + this.IDBarra + "  -  " + ms.Message);
                        }
                    }
                }

            }
            catch (Exception ms)
            {
                System.Windows.Forms.MessageBox.Show("erro ao desenhar fz:  barra " + this.IDBarra + "  -  " + ms.Message);
            }
        }

        public void Arestas_FY(ref List<float> coords_arestas,
    ref double MultiplicadorAltura, ref bool gradiente,
    ref bool Diagrama_preenchido_contorno, ref bool Diagrama_somente_linhas,
    ref bool Diagrama_linha_contorno, ref Color CorArestaDiagrama,
                         int tipocarga, int caso, int comb)
        {
            try
            {
                if (Visivel /*&& barraOriginal.Visivel && !barra_de_articulacao */&& !barraRigida)
                {

                    if (tipocarga == 0)
                    {
                        Esforcos = casos_x_esforcos[caso].Esforcos;
                    }
                    else
                    {
                        Esforcos = combinacoes_x_esforcos[comb].Esforcos;
                    }

                    if ((Geom.Iguais(Esforcos[3], 0)) && Geom.Iguais(Esforcos[9], 0))
                    {
                        for (int n = 0; n < 4; n++)
                            coordsForca[n] = new vec3(0);
                        return;
                    }
                    {
                        try
                        {
                            coordsForca[0].x = 0;
                            coordsForca[0].y = 0;
                            coordsForca[0].z = 0;

                            coordsForca[1].x = 0;
                            coordsForca[1].y = Esforcos[3] * MultiplicadorAltura;
                            coordsForca[1].z = 0;


                            GiraDiagramaConformeAnguloAlfa(ref coordsForca[1], "X");

                            coordsForca[2].x = comprimento;
                            coordsForca[3].x = comprimento;

                            if (!Geom.Iguais(pIni.x, pFin.x))
                            {
                                if (pIni.x > pFin.x)
                                {
                                    coordsForca[2].x = -comprimento;
                                    coordsForca[3].x = -comprimento;
                                }
                            }

                            coordsForca[2].y = Esforcos[9] * -1 * MultiplicadorAltura;
                            coordsForca[2].z = 0;

                            GiraDiagramaConformeAnguloAlfa(ref coordsForca[2], "X");

                            coordsForca[3].y = 0;
                            coordsForca[3].z = 0;

                            for (i = 0; i < coordsForca.Count(); i++)
                            {
                                posicao[1] = coordsForca[i].x;
                                posicao[2] = coordsForca[i].y;
                                posicao[3] = coordsForca[i].z;

                                Geom.rotY(barraOriginal.angXY, ref posicao, ref posicaoFinal);
                                Geom.rotZ(barraOriginal.angXZ, ref posicaoFinal, ref posicaoFinal2);

                                coordsForca[i].x = posicaoFinal2[1] + tx;
                                coordsForca[i].y = posicaoFinal2[2] + ty;
                                coordsForca[i].z = posicaoFinal2[3] + tz;
                            }

                            double r1 = 0, r2 = 0, g1 = 0, g2 = 0, b1 = 0, b2 = 0;

                            if (gradiente)
                            {
                                //    RetRGBEsforco(sinal_fy_ini, sinal_fy_fin, 3, 9, ref r1, ref g1, ref b1, ref r2, ref g2, ref b2, tipocarga, caso, comb);
                                RetCorEsforco(sinal_fy_ini, Esforcos[3], ref r1, ref g1, ref b1);
                                RetCorEsforco(sinal_fy_fin, Esforcos[9], ref r2, ref g2, ref b2);
                            }
                            else
                            {
                                r1 = CorArestaDiagrama.R;
                                g1 = CorArestaDiagrama.G;
                                b1 = CorArestaDiagrama.B;

                                r2 = CorArestaDiagrama.R;
                                g2 = CorArestaDiagrama.G;
                                b2 = CorArestaDiagrama.B;
                            }

                            /* r_a = r1;
                             g_a = g1;
                             b_a = b1;

                             r_b = r2;
                             g_b = g2;
                             b_b = b2;*/

                            setLista(ref coords_arestas, coordsForca[0].x, coordsForca[0].y, coordsForca[0].z);
                            setLista(ref coords_arestas, r1, g1, b1);
                            setLista(ref coords_arestas, coordsForca[1].x, coordsForca[1].y, coordsForca[1].z);
                            setLista(ref coords_arestas, r1, g1, b1);

                            setLista(ref coords_arestas, coordsForca[1].x, coordsForca[1].y, coordsForca[1].z);
                            setLista(ref coords_arestas, r1, g1, b1);
                            setLista(ref coords_arestas, coordsForca[2].x, coordsForca[2].y, coordsForca[2].z);
                            setLista(ref coords_arestas, r2, g2, b2);

                            setLista(ref coords_arestas, coordsForca[2].x, coordsForca[2].y, coordsForca[2].z);
                            setLista(ref coords_arestas, r2, g2, b2);
                            setLista(ref coords_arestas, coordsForca[3].x, coordsForca[3].y, coordsForca[3].z);
                            setLista(ref coords_arestas, r2, g2, b2);
                        }
                        catch (Exception ms)
                        {
                            System.Windows.Forms.MessageBox.Show("erro ao desenhar esforço FZ: barra " + this.IDBarra + "  -  " + ms.Message);
                        }
                    }
                }
                else
                {
                    for (int n = 0; n < 4; n++)
                        coordsForca[n] = new vec3(0);
                }


            }
            catch (Exception ms)
            {
                System.Windows.Forms.MessageBox.Show("erro ao desenhar FZ:  barra " + this.IDBarra + "  -  " + ms.Message);
            }
        }

        public void Preenche_MX(ref List<float> coords_arestas,
     ref double MultiplicadorAltura, ref bool gradiente,
     ref bool Diagrama_preenchido_contorno, ref bool Diagrama_somente_linhas,
     ref bool Diagrama_linha_contorno, ref Color CorArestaDiagrama,
                 int tipocarga, int caso, int comb)
        {
            try
            {
                if (Visivel /*&& barraOriginal.Visivel && !barra_de_articulacao */&& !barraRigida)
                {
                    if (tipocarga == 0)
                    {
                        Esforcos = casos_x_esforcos[caso].Esforcos;
                    }
                    else
                    {
                        Esforcos = combinacoes_x_esforcos[comb].Esforcos;
                    }

                    if ((Geom.Iguais(Esforcos[4], 0)) && Geom.Iguais(Esforcos[10], 0))
                        return;

                    {
                        coordsTorcor[0].x = 0;
                        coordsTorcor[0].y = 0;
                        coordsTorcor[0].z = 0;

                        coordsTorcor[1].x = 0;
                        coordsTorcor[1].y = 0;
                        coordsTorcor[1].z = Esforcos[4] * -1 * MultiplicadorAltura;
                        if (barraOriginal_Vertical)
                            if (pIni.z * -1 < pFin.z * -1)
                                coordsTorcor[1].z *= -1;

                        GiraDiagramaConformeAnguloAlfa(ref coordsTorcor[1], "Z");

                        coordsTorcor[2].x = comprimento;
                        coordsTorcor[3].x = comprimento;

                        if (!Geom.Iguais(barraOriginal.pIni.x, barraOriginal.pFin.x))
                        {
                            if (barraOriginal.pIni.x > barraOriginal.pFin.x)
                            {
                                coordsTorcor[2].x = -comprimento;
                                coordsTorcor[3].x = -comprimento;
                            }
                        }

                        coordsTorcor[2].y = 0;
                        coordsTorcor[2].z = Esforcos[10] * MultiplicadorAltura;

                        if (barraOriginal_Vertical)
                            if (pIni.z * -1 < pFin.z * -1)
                                coordsTorcor[2].z *= -1;

                        GiraDiagramaConformeAnguloAlfa(ref coordsTorcor[2], "Z");

                        coordsTorcor[3].y = 0;
                        coordsTorcor[3].z = 0;

                        double r1 = 0, r2 = 0, g1 = 0, g2 = 0, b1 = 0, b2 = 0;

                        if (gradiente)
                        {
                          //  RetRGBEsforco(sinal_mx_ini, sinal_mx_fin, 4, 10, ref r1, ref g1, ref b1, ref r2, ref g2, ref b2, tipocarga, caso, comb);
                            RetCorEsforco(sinal_mx_ini, Esforcos[4], ref r1, ref g1, ref b1);
                            RetCorEsforco(sinal_mx_fin, Esforcos[10], ref r2, ref g2, ref b2);
                        }
                        else
                        {
                            r1 = CorArestaDiagrama.R;
                            g1 = CorArestaDiagrama.G;
                            b1 = CorArestaDiagrama.B;

                            r2 = CorArestaDiagrama.R;
                            g2 = CorArestaDiagrama.G;
                            b2 = CorArestaDiagrama.B;
                        }

                        for (i = 0; i < coordsTorcor.Count(); i++)
                        {
                            posicao[1] = coordsTorcor[i].x;
                            posicao[2] = coordsTorcor[i].y;
                            posicao[3] = coordsTorcor[i].z;

                            Geom.rotY(barraOriginal.angXY, ref posicao, ref posicaoFinal);
                            Geom.rotZ(barraOriginal.angXZ, ref posicaoFinal, ref posicaoFinal2);

                            coordsTorcor[i].x = posicaoFinal2[1] + tx;
                            coordsTorcor[i].y = posicaoFinal2[2] + ty;
                            coordsTorcor[i].z = posicaoFinal2[3] + tz;
                        }

                        setLista(ref coords_arestas, coordsTorcor[0].x, coordsTorcor[0].y, coordsTorcor[0].z);
                        setLista(ref coords_arestas, r1, g1, b1);
                        setLista(ref coords_arestas, coordsTorcor[1].x, coordsTorcor[1].y, coordsTorcor[1].z);
                        setLista(ref coords_arestas, r1, g1, b1);

                        setLista(ref coords_arestas, coordsTorcor[1].x, coordsTorcor[1].y, coordsTorcor[1].z);
                        setLista(ref coords_arestas, r1, g1, b1);
                        setLista(ref coords_arestas, coordsTorcor[2].x, coordsTorcor[2].y, coordsTorcor[2].z);
                        setLista(ref coords_arestas, r2, g2, b2);

                        setLista(ref coords_arestas, coordsTorcor[2].x, coordsTorcor[2].y, coordsTorcor[2].z);
                        setLista(ref coords_arestas, r2, g2, b2);
                        setLista(ref coords_arestas, coordsTorcor[3].x, coordsTorcor[3].y, coordsTorcor[3].z);
                        setLista(ref coords_arestas, r2, g2, b2);
                    }
                }
            }
            catch (Exception ms)
            {
                System.Windows.Forms.MessageBox.Show("erro ao desenhar MX:  barra " + this.IDBarra + "  -  " + ms.Message);
            }
        }

        public void Preenche_MX_Gradiente(ref List<float> coords_triangulos, ref double MultiplicadorAltura,
                         int tipocarga, int caso, int comb)
        {
            try
            {
                if (Visivel/* && barraOriginal.Visivel && !barra_de_articulacao*/ && !barraRigida)
                {
                    if (tipocarga == 0)
                    {
                        Esforcos = casos_x_esforcos[caso].Esforcos;
                    }
                    else
                    {
                        Esforcos = combinacoes_x_esforcos[comb].Esforcos;
                    }

                    if ((Geom.Iguais(Esforcos[4], 0)) && Geom.Iguais(Esforcos[10], 0))
                        return;

                    {
                        coordsTorcor[0].x = 0;
                        coordsTorcor[0].y = 0;
                        coordsTorcor[0].z = 0;

                        coordsTorcor[1].x = 0;
                        coordsTorcor[1].y = 0;
                        coordsTorcor[1].z = Esforcos[4] * -1 * MultiplicadorAltura;
                        if (barraOriginal_Vertical)
                            if (pIni.z * -1 < pFin.z * -1)
                                coordsTorcor[1].z *= -1;

                        GiraDiagramaConformeAnguloAlfa(ref coordsTorcor[1], "Z");

                        coordsTorcor[2].x = comprimento;
                        coordsTorcor[3].x = comprimento;

                        if (!Geom.Iguais(barraOriginal.pIni.x, barraOriginal.pFin.x))
                        {
                            if (barraOriginal.pIni.x > barraOriginal.pFin.x)
                            {
                                coordsTorcor[2].x = -comprimento;
                                coordsTorcor[3].x = -comprimento;
                            }
                        }

                        coordsTorcor[2].y = 0;
                        coordsTorcor[2].z = Esforcos[10] * MultiplicadorAltura;

                        if (barraOriginal_Vertical)
                            if (pIni.z * -1 < pFin.z * -1)
                                coordsTorcor[2].z *= -1;

                        GiraDiagramaConformeAnguloAlfa(ref coordsTorcor[2], "Z");

                        coordsTorcor[3].y = 0;
                        coordsTorcor[3].z = 0;

                        double r1 = 0, r2 = 0, g1 = 0, g2 = 0, b1 = 0, b2 = 0;

                      //  RetRGBEsforco(sinal_mx_ini, sinal_mx_fin, 4, 10, ref r1, ref g1, ref b1, ref r2, ref g2, ref b2, tipocarga, caso, comb);
                        RetCorEsforco(sinal_mx_ini, Esforcos[4], ref r1, ref g1, ref b1);
                        RetCorEsforco(sinal_mx_fin, Esforcos[10], ref r2, ref g2, ref b2);
                        /* RGB_Negativos = RGB_FletoresNegativos;
                         RGB_Positivos = RGB_FletoresPositivos;

                         if (coordsTorcor[1].z < 0)
                         {
                             IndiceRGB = RetIndiceRGB_Negativos(ref RGB_Negativos, Math.Abs(Esforcos[4]));
                             r1 = RGB_Negativos[IndiceRGB, 2];
                             g1 = RGB_Negativos[IndiceRGB, 3];
                             b1 = RGB_Negativos[IndiceRGB, 4];
                         }
                         else
                         {
                             IndiceRGB = RetIndiceRGB_Positivos(ref RGB_Positivos, Math.Abs(Esforcos[4]));
                             r1 = RGB_Positivos[IndiceRGB, 2];
                             g1 = RGB_Positivos[IndiceRGB, 3];
                             b1 = RGB_Positivos[IndiceRGB, 4];
                         }

                         if (coordsTorcor[2].z < 0)
                         {
                             IndiceRGB = RetIndiceRGB_Negativos(ref RGB_Negativos, Math.Abs(Esforcos[10]));
                             r2 = RGB_Negativos[IndiceRGB, 2];
                             g2 = RGB_Negativos[IndiceRGB, 3];
                             b2 = RGB_Negativos[IndiceRGB, 4];
                         }
                         else
                         {
                             IndiceRGB = RetIndiceRGB_Positivos(ref RGB_Positivos, Math.Abs(Esforcos[10]));
                             r2 = RGB_Positivos[IndiceRGB, 2];
                             g2 = RGB_Positivos[IndiceRGB, 3];
                             b2 = RGB_Positivos[IndiceRGB, 4];
                         }
                         */
                        for (i = 0; i < coordsTorcor.Count(); i++)
                        {
                            posicao[1] = coordsTorcor[i].x;
                            posicao[2] = coordsTorcor[i].y;
                            posicao[3] = coordsTorcor[i].z;

                            Geom.rotY(barraOriginal.angXY, ref posicao, ref posicaoFinal);
                            Geom.rotZ(barraOriginal.angXZ, ref posicaoFinal, ref posicaoFinal2);

                            coordsTorcor[i].x = posicaoFinal2[1] + tx;
                            coordsTorcor[i].y = posicaoFinal2[2] + ty;
                            coordsTorcor[i].z = posicaoFinal2[3] + tz;
                        }

                        CriarDoisTriangulos(coordsTorcor, coords_triangulos, r1, g1, b1, r2, g2, b2);
                    }
                }
            }
            catch (Exception ms)
            {
                System.Windows.Forms.MessageBox.Show("erro ao desenhar MX:  barra " + this.IDBarra + "  -  " + ms.Message);
            }
        }

        public void Preenche_MZ(ref List<float> coords_arestas,
          ref double MultiplicadorAltura, ref bool gradiente,
          ref bool Diagrama_preenchido_contorno, ref bool Diagrama_somente_linhas,
          ref bool Diagrama_linha_contorno, ref Color CorArestaDiagrama,
                 int tipocarga, int caso, int comb)
        {
            try
            {
                if (Visivel /*&& barraOriginal.Visivel && !barra_de_articulacao */&& !barraRigida)
                //    if ((!Geom.Iguais(Esforcos[5],0)) && !Geom.Iguais(Esforcos[11], 0))
                {

                    if (tipocarga == 0)
                    {
                        Esforcos = casos_x_esforcos[caso].Esforcos;
                    }
                    else
                    {
                        Esforcos = combinacoes_x_esforcos[comb].Esforcos;
                    }

                    if ((Geom.Iguais(Esforcos[5], 0)) && Geom.Iguais(Esforcos[11], 0))
                        return;

                    {
                        coordsFletorZ[0].x = 0;
                        coordsFletorZ[0].y = 0;
                        coordsFletorZ[0].z = 0;

                        coordsFletorZ[1].x = 0;
                        coordsFletorZ[1].y = Esforcos[5] * -1 * MultiplicadorAltura;
                        coordsFletorZ[1].z = 0;

                        if (barraOriginal_Vertical)
                            if (pIni_offset.z * -1 < pFin_offset.z * -1)
                                coordsFletorZ[1].y *= -1;

                        //GiraDiagramaConformeAnguloAlfa(ref coordsFletorZ[1], "X");

                        coordsFletorZ[2].x = comprimento;
                        coordsFletorZ[3].x = comprimento;
                        coordsFletorZ[2].y = Esforcos[11] * MultiplicadorAltura;

                        if (!Geom.Iguais(barraOriginal.pIni.x, barraOriginal.pFin.x))
                        {
                            if (barraOriginal.pIni.x > barraOriginal.pFin.x)
                            {
                                coordsFletorZ[2].x = -comprimento;
                                coordsFletorZ[3].x = -comprimento;

                                coordsFletorZ[1].y *= -1;
                                coordsFletorZ[2].y *= -1;
                            }
                        }
                        GiraDiagramaConformeAnguloAlfa(ref coordsFletorZ[1], "X");

                        if (barraOriginal_Vertical)
                        {
                            if (pIni_offset.z * -1 < pFin_offset.z * -1)
                                coordsFletorZ[2].y *= -1;
                        }

                        coordsFletorZ[2].z = 0;

                        GiraDiagramaConformeAnguloAlfa(ref coordsFletorZ[2], "X");

                        coordsFletorZ[3].y = 0;
                        coordsFletorZ[3].z = 0;

                        double r1 = 0, r2 = 0, g1 = 0, g2 = 0, b1 = 0, b2 = 0;

                        if (gradiente)
                        {
                          // RetRGBEsforco(sinal_mz_ini, sinal_mz_fin, 5, 11, ref r1, ref g1, ref b1, ref r2, ref g2, ref b2, tipocarga, caso, comb);
                            RetCorEsforco(sinal_mz_ini, Esforcos[5], ref r1, ref g1, ref b1);
                            RetCorEsforco(sinal_mz_fin, Esforcos[11], ref r2, ref g2, ref b2);
                        }
                        else
                        {
                            r1 = CorArestaDiagrama.R;
                            g1 = CorArestaDiagrama.G;
                            b1 = CorArestaDiagrama.B;

                            r2 = CorArestaDiagrama.R;
                            g2 = CorArestaDiagrama.G;
                            b2 = CorArestaDiagrama.B;
                        }

                        for (i = 0; i < coordsFletorZ.Count(); i++)
                        {
                            posicao[1] = coordsFletorZ[i].x;
                            posicao[2] = coordsFletorZ[i].y;
                            posicao[3] = coordsFletorZ[i].z;

                            Geom.rotY(barraOriginal.angXY, ref posicao, ref posicaoFinal);
                            Geom.rotZ(barraOriginal.angXZ, ref posicaoFinal, ref posicaoFinal2);

                            coordsFletorZ[i].x = posicaoFinal2[1] + tx;
                            coordsFletorZ[i].y = posicaoFinal2[2] + ty;
                            coordsFletorZ[i].z = posicaoFinal2[3] + tz;
                        }

                        setLista(ref coords_arestas, coordsFletorZ[0].x, coordsFletorZ[0].y, coordsFletorZ[0].z);
                        setLista(ref coords_arestas, r1, g1, b1);
                        setLista(ref coords_arestas, coordsFletorZ[1].x, coordsFletorZ[1].y, coordsFletorZ[1].z);
                        setLista(ref coords_arestas, r1, g1, b1);

                        setLista(ref coords_arestas, coordsFletorZ[1].x, coordsFletorZ[1].y, coordsFletorZ[1].z);
                        setLista(ref coords_arestas, r1, g1, b1);
                        setLista(ref coords_arestas, coordsFletorZ[2].x, coordsFletorZ[2].y, coordsFletorZ[2].z);
                        setLista(ref coords_arestas, r2, g2, b2);

                        setLista(ref coords_arestas, coordsFletorZ[2].x, coordsFletorZ[2].y, coordsFletorZ[2].z);
                        setLista(ref coords_arestas, r2, g2, b2);
                        setLista(ref coords_arestas, coordsFletorZ[3].x, coordsFletorZ[3].y, coordsFletorZ[3].z);
                        setLista(ref coords_arestas, r2, g2, b2);
                    }
                }
            }
            catch (Exception ms)
            {
                System.Windows.Forms.MessageBox.Show("erro ao desenhar MZ:  barra " + this.IDBarra + "  -  " + ms.Message);
            }
        }

        public int RetRGBTensao(string inicio_fim_barra, int coord_secao,
                ref double r1, ref double g1, ref double b1,
                int tipocarga, int caso, int comb)
        {
                if (inicio_fim_barra == "i")
                {
                   /* if (tipocarga == 0)
                        tensoes_ini = casos_x_tensoes[caso].tensoes_i;
                    else
                    if (tipocarga == 1)
                        tensoes_ini = combinacoes_x_tensoes[comb].tensoes_i;*/

                    if (Geom.SaoIguais(tensoes_ini[coord_secao], 0, 1e-9))
                    {
                        r1 = (double)Gerenciador.corSemTensao.R / 255;
                        g1 = (double)Gerenciador.corSemTensao.G / 255;
                        b1 = (double)Gerenciador.corSemTensao.B / 255;
                    }
                    else
                    if (tensoes_ini[coord_secao] < 0)
                    {
                        IndiceRGB = RetIndiceRGB_TensaoNegativa(ref tensoes_ini[coord_secao]);
                        r1 = Gerenciador.RGB_TensoesNormaisNegativas[IndiceRGB, 2];
                        g1 = Gerenciador.RGB_TensoesNormaisNegativas[IndiceRGB, 3];
                        b1 = Gerenciador.RGB_TensoesNormaisNegativas[IndiceRGB, 4];
                    }
                    else
                    if (tensoes_ini[coord_secao] > 0)
                    {
                        IndiceRGB = RetIndiceRGB_TensaoPositiva(ref tensoes_ini[coord_secao]);
                        r1 = Gerenciador.RGB_TensoesNormaisPositivas[IndiceRGB, 2];
                        g1 = Gerenciador.RGB_TensoesNormaisPositivas[IndiceRGB, 3];
                        b1 = Gerenciador.RGB_TensoesNormaisPositivas[IndiceRGB, 4];
                    }
                }
                else
                if (inicio_fim_barra == "f")
                {
                   /* if (tipocarga == 0)
                        tensoes_fin = casos_x_tensoes[caso].tensoes_f;
                    else
                    if (tipocarga == 1)
                        tensoes_fin = combinacoes_x_tensoes[comb].tensoes_f;*/

                    if (Geom.SaoIguais(tensoes_fin[coord_secao], 0, 1e-9))
                    {
                        r1 = (double)Gerenciador.corSemTensao.R / 255;
                        g1 = (double)Gerenciador.corSemTensao.G / 255;
                        b1 = (double)Gerenciador.corSemTensao.B / 255;
                    }
                    else
                    if (tensoes_fin[coord_secao] < 0)
                    {
                        IndiceRGB = RetIndiceRGB_TensaoNegativa(ref tensoes_fin[coord_secao]);
                        r1 = Gerenciador.RGB_TensoesNormaisNegativas[IndiceRGB, 2];
                        g1 = Gerenciador.RGB_TensoesNormaisNegativas[IndiceRGB, 3];
                        b1 = Gerenciador.RGB_TensoesNormaisNegativas[IndiceRGB, 4];
                    }
                    else
                    if (tensoes_fin[coord_secao] >= 0)
                    {
                        IndiceRGB = RetIndiceRGB_TensaoPositiva(ref tensoes_fin[coord_secao]);
                        r1 = Gerenciador.RGB_TensoesNormaisPositivas[IndiceRGB, 2];
                        g1 = Gerenciador.RGB_TensoesNormaisPositivas[IndiceRGB, 3];
                        b1 = Gerenciador.RGB_TensoesNormaisPositivas[IndiceRGB, 4];
                    }
                }

            return 0;
        }
        private int RetIndiceRGB_TensaoPositiva(ref double tensao)
        {
            /* double max = Gerenciador.RGB_TensoesNormaisPositivas[0, 0];
             double min = Gerenciador.RGB_TensoesNormaisPositivas[6, 1];

             if (tensao >= max)
                 return 0;

             if (tensao <= min)
                 return 6;

             double intervalo = (max - min) / 7.0;

             int indice = (int)((max - tensao) / intervalo);

             if (indice < 0)
                 indice = 0;

             if (indice > 6)
                 indice = 6;

             return indice;*/

            double maxTracao = Gerenciador.RGB_TensoesNormaisPositivas[0, 0];

            int indice =
                (int)(((maxTracao - tensao) / maxTracao) * 7.0);

            if (indice < 0)
                indice = 0;

            if (indice > 6)
                indice = 6;

            return indice;
        }
        private int RetIndiceRGB_TensaoNegativa(ref double tensao)
        {
            /*  double t = Math.Abs(tensao);

              double min = Math.Abs(Gerenciador.RGB_TensoesNormaisNegativas[0, 0]);
              double max = Math.Abs(Gerenciador.RGB_TensoesNormaisNegativas[6, 1]);

              if (t <= min)
                  return 0;

              if (t >= max)
                  return 6;

              double intervalo = (max - min) / 7.0;

              int indice = (int)((t - min) / intervalo);

              if (indice < 0)
                  indice = 0;

              if (indice > 6)
                  indice = 6;

              return indice;*/

            double t = Math.Abs(tensao);

            double maxComp = Math.Abs(Gerenciador.RGB_TensoesNormaisNegativas[6, 1]);

            int indice = (int)((t / maxComp) * 7.0);

            if (indice < 0)
                indice = 0;

            if (indice > 6)
                indice = 6;

            return indice;

        }
        public void RetCorDeslocamento(ref double r1, ref double g1, ref double b1, ref double r2, ref double g2, ref double b2,
            int tipocarga, int caso, int comb)
        {
            double[,] tabela = Gerenciador.RGB_Deslocamentos;

            if (tabela == null)
                return;

            double uIni, uFin;

            if (tipocarga == 0)
            {
                uIni = pIni.casos_x_deslocamentos[caso].U_Total;
                uFin = pFin.casos_x_deslocamentos[caso].U_Total;
            }
            else
            {
                uIni = pIni.combinacoes_x_deslocamentos[comb].U_Total;
                uFin = pFin.combinacoes_x_deslocamentos[comb].U_Total;
            }

            double[] deslocamentos = { uIni, uFin };

            for (int extremidade = 0; extremidade < 2; extremidade++)
            {
                double desloc = deslocamentos[extremidade];

                for (int i = 0; i < tabela.GetLength(0); i++)
                {
                    if ((desloc >= tabela[i, 1]) &&
                        (desloc <= tabela[i, 0]))
                    {
                        if (extremidade == 0)
                        {
                            r1 = tabela[i, 2];
                            g1 = tabela[i, 3];
                            b1 = tabela[i, 4];
                        }
                        else
                        {
                            r2 = tabela[i, 2];
                            g2 = tabela[i, 3];
                            b2 = tabela[i, 4];
                        }

                        break;
                    }
                }
            }
        }

        private void RetCorEsforco(double sinal,double esforco,ref double r,ref double g,ref double b)
        {
            double[,] tabela = (sinal < 0)
                ? Gerenciador.RGB_EsforcosNegativos
                : Gerenciador.RGB_EsforcosPositivos;

            if (tabela == null)
                return;

            esforco = Math.Abs(esforco);

            for (int i = 0; i < tabela.GetLength(0); i++)
            {
                if (Geom.MaiorOuIgual(esforco, tabela[i, 1]) &&
                    Geom.MenorOuIgual(esforco, tabela[i, 0]))
                {
                    r = tabela[i, 2];
                    g = tabela[i, 3];
                    b = tabela[i, 4];
                    return;
                }
            }

            // Caso não encontre nenhuma faixa
            r = tabela[0, 2];
            g = tabela[0, 3];
            b = tabela[0, 4];
        }

        public double esfTemp1, esfTemp2;


        public double r_a, g_a, b_a, r_b, g_b, b_b;
    }
}
