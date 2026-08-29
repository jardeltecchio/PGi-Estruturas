using MathNet.Numerics.RootFinding;
using Microsoft.VisualBasic.Compatibility.VB6;
using MS.WindowsAPICodePack.Internal;
using OpenTK.Graphics;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web.UI.Design;
using System.Windows.Controls;
using System.Windows.Forms;
using System.Windows.Ink;
using System.Windows.Media;

namespace PG
{
    public partial class FDadosBarra : Form
    {
        public FDadosBarra()
        {
            InitializeComponent();
        }
        Gerenciador gerenciador;
        public bool alterando = false;
        TDadosBarra Dados;

        public FDadosBarra(Gerenciador _gerenciador, TDadosBarra _Dados)
        {
            this.gerenciador = _gerenciador;
            this.Dados = _Dados;
            InitializeComponent();
            cbTipo.SelectedIndex = 0;
            numero.Text = "1";
        }

        private void button3_Click(object sender, EventArgs e)
        {
            this.Close();
        }
        public void GravaDados()
        {

        }
        CoordenadaD[] coordsPoligono;
        public TPoligono poligono;
        public TSecao secaoSemRotacao;
        public void CriaSecao()
        {
            coordsPoligono = new CoordenadaD[5];
            coordsPoligono[0].X = -100;
            coordsPoligono[0].Y = -250;

            coordsPoligono[1].X = 100;
            coordsPoligono[1].Y = -250;

            coordsPoligono[2].X = 100;
            coordsPoligono[2].Y = 250;

            coordsPoligono[3].X = -100;
            coordsPoligono[3].Y = 250;

            coordsPoligono[4].X = -100;
            coordsPoligono[4].Y = -250;

            for (int i = 0; i < coordsPoligono.Count(); i++)
            {
                coordsPoligono[i].X /= 10;
                coordsPoligono[i].Y /= 10;
            }

            poligono = new TPoligono(coordsPoligono);
            poligono.CalculaPropriedades();
        }

        private void btSalvar_Click(object sender, EventArgs e)
        {
            Salvar();
        }
        void AlterarElementos()
        {
            bool alterouAngulo = edAngulo.Text.Trim() != string.Empty;

            if (multi)
            {
                foreach (TObjetoDesenho o in gerenciador.formDesenho.ObjetosSelecionados)
                {
                    if (o.Tipo == Const.ID_BARRAGENERICA)
                    {
                        if ((o as TBarraGenerica).Dados.Tipo == 4 && gerenciador.formDesenho.Estrutura.barras.Exists(bb => bb.id_barra_rigida_1 == (o as TBarraGenerica).IDBarra || bb.id_barra_rigida_2 == (o as TBarraGenerica).IDBarra))
                            continue;

                        if (ArtMy.SelectedIndex > 0)
                            (o as TBarraGenerica).Dados.Articulacao_my = ArtMy.SelectedIndex-1;
                        if (ArtMz.SelectedIndex > 0)
                            (o as TBarraGenerica).Dados.Articulacao_mz = ArtMz.SelectedIndex-1;
                        if (cbTipo.SelectedIndex > 0)
                            (o as TBarraGenerica).Dados.Tipo = cbTipo.SelectedIndex - 1;

                        if (edEy.Text != string.Empty)
                            (o as TBarraGenerica).Dados.ey = Convert.ToDouble(edEy.Text);
                        if (edEz.Text != string.Empty)
                            (o as TBarraGenerica).Dados.ez = Convert.ToDouble(edEz.Text);
                        if (edEx_i.Text != string.Empty)
                            (o as TBarraGenerica).Dados.ex_i = Convert.ToDouble(edEx_i.Text);
                        if (edEx_f.Text != string.Empty)
                            (o as TBarraGenerica).Dados.ex_f = Convert.ToDouble(edEx_f.Text);

                        (o as TBarraGenerica).temOffset = 
                            (!Geom.Iguais((o as TBarraGenerica).Dados.ez, 0)
                             || !Geom.Iguais((o as TBarraGenerica).Dados.ex_i, 0)
                             || !Geom.Iguais((o as TBarraGenerica).Dados.ex_f, 0)
                             || !Geom.Iguais((o as TBarraGenerica).Dados.ey, 0));

                        if (edRigMY.Text != string.Empty)
                            (o as TBarraGenerica).Dados.k_my = System.Convert.ToDouble(edRigMY.Text);

                        if (edRigMZ.Text != string.Empty)
                            (o as TBarraGenerica).Dados.k_mz = System.Convert.ToDouble(edRigMZ.Text);

                        if (cbTipo.SelectedIndex == 4 || (o as TBarraGenerica).Dados.Tipo == 4)
                        {
                            (o as TBarraGenerica).Dados.Articulacao_my = 0;
                            (o as TBarraGenerica).Dados.Articulacao_mz = 0;
                        }
                    }
                }
            }
            else
            {
                foreach (TObjetoDesenho o in gerenciador.formDesenho.ObjetosSelecionados)
                {
                    if (o.Tipo == Const.ID_BARRAGENERICA)
                    {
                        if ((o as TBarraGenerica).Dados.Tipo == 4 && gerenciador.formDesenho.Estrutura.barras.Exists(bb => bb.id_barra_rigida_1 == (o as TBarraGenerica).IDBarra || bb.id_barra_rigida_2 == (o as TBarraGenerica).IDBarra))
                            continue;

                       (o as TBarraGenerica).Dados.Articulacao_my = ArtMy.SelectedIndex;
                       (o as TBarraGenerica).Dados.Articulacao_mz = ArtMz.SelectedIndex;
                       (o as TBarraGenerica).Dados.Tipo = cbTipo.SelectedIndex;

                       (o as TBarraGenerica).Dados.k_my = System.Convert.ToDouble(edRigMY.Text);
                       (o as TBarraGenerica).Dados.k_mz = System.Convert.ToDouble(edRigMZ.Text);

                        if (edEy.Text != string.Empty)
                            (o as TBarraGenerica).Dados.ey = Convert.ToDouble(edEy.Text);

                        if (edEz.Text != string.Empty)
                            (o as TBarraGenerica).Dados.ez = Convert.ToDouble(edEz.Text);

                        if (edEx_i.Text != string.Empty)
                            (o as TBarraGenerica).Dados.ex_i = Convert.ToDouble(edEx_i.Text);

                        if (edEx_f.Text != string.Empty)
                            (o as TBarraGenerica).Dados.ex_f = Convert.ToDouble(edEx_f.Text);

                        (o as TBarraGenerica).temOffset =
                            (!Geom.Iguais((o as TBarraGenerica).Dados.ez, 0)
                            || !Geom.Iguais((o as TBarraGenerica).Dados.ex_i, 0)
                            || !Geom.Iguais((o as TBarraGenerica).Dados.ex_f, 0)
                            || !Geom.Iguais((o as TBarraGenerica).Dados.ey, 0));


                        if ((o as TBarraGenerica).Dados.ey != 0 || (o as TBarraGenerica).Dados.ez != 0)
                        {
                           /* if ((o as TBarraGenerica).id_barra_rigida_1 != -1)
                            {
                                if (gerenciador.formDesenho.Estrutura.barras.Exists(bb => bb.IDBarra == ((o as TBarraGenerica).id_barra_rigida_1)))
                                    gerenciador.formDesenho.Estrutura.barras.Find(bb => bb.IDBarra == (o as TBarraGenerica).id_barra_rigida_1).Selecionado = true;
                                
                                gerenciador.formDesenho.Estrutura.barras.RemoveAll(obj => obj.Selecionado && obj.Dados.Tipo == 4);
                            }*/

                          //  gerenciador.formDesenho.CriarOffsets((o as TBarraGenerica));
                        }

                        if (cbTipo.SelectedIndex == 4)
                        {
                            (o as TBarraGenerica).Dados.Articulacao_my = 0;
                            (o as TBarraGenerica).Dados.Articulacao_mz = 0;
                        }
                    }
                }
            }


            if (secao_click != null)
            {
                if (!multi)
                {
                    foreach (TObjetoDesenho o in gerenciador.formDesenho.ObjetosSelecionados)
                    {
                        if (o.Tipo == Const.ID_BARRAGENERICA)
                        {
                            (o as TBarraGenerica).Dados.secao = (TSecao)secao_click.Clone();
                            (o as TBarraGenerica).Dados.secaoSemRotacao = (TSecao)secaoSemRotacao.Clone();
                            (o as TBarraGenerica).Dados.idsecao = idSecao;
                            (o as TBarraGenerica).Dados.anguloRotacao = anguloRotacao;
                        }
                    }
                }
                else
                if (multi)
                {
                    if (alterouAngulo)
                    {
                        foreach (TObjetoDesenho o in gerenciador.formDesenho.ObjetosSelecionados)
                        {
                            if (o.Tipo == Const.ID_BARRAGENERICA)
                            {
                                if ((o as TBarraGenerica).Dados.Tipo == 4 && gerenciador.formDesenho.Estrutura.barras.Exists(bb => bb.id_barra_rigida_1 == (o as TBarraGenerica).IDBarra || bb.id_barra_rigida_2 == (o as TBarraGenerica).IDBarra))
                                    continue;

                                (o as TBarraGenerica).Dados.secao = (TSecao)secao_click.Clone();
                                (o as TBarraGenerica).Dados.secaoSemRotacao = (TSecao)secaoSemRotacao.Clone();
                                (o as TBarraGenerica).Dados.idsecao = idSecao;
                                (o as TBarraGenerica).Dados.anguloRotacao = anguloRotacao;
                            }
                        }
                    }
                    else
                    {
                        TSecao secaux = new TSecao();
                        foreach (TObjetoDesenho o in gerenciador.formDesenho.ObjetosSelecionados)
                        {
                            if (o.Tipo == Const.ID_BARRAGENERICA)
                            {
                                if ((o as TBarraGenerica).Dados.Tipo == 4 && gerenciador.formDesenho.Estrutura.barras.Exists(bb => bb.id_barra_rigida_1 == (o as TBarraGenerica).IDBarra || bb.id_barra_rigida_2 == (o as TBarraGenerica).IDBarra))
                                    continue;

                                if ((Object)secao_click != null)
                                {
                                    secaux = (TSecao)secao_click.Clone();

                                    centroX = pixelX(0);
                                    centroY = pixelY(0);

                               //     secaux.poligono.CalculaPropriedades();

                                    centroRotacao.y = 0;
                                    centroRotacao.x = 0;

                                  /*  for (int i = 0; i < (secaux.poligono.coords.Count()); i++)
                                    {
                                        coord1 = new vec3(secaux.poligono.coords[i].X, secaux.poligono.coords[i].Y, 0);
                                        coord1 = coord1.Rotate(centroRotacao, ((o as TBarraGenerica).Dados.anguloRotacao) * Const.PIDiv180);

                                        secaux.poligono.coords[i].X = coord1.x;
                                        secaux.poligono.coords[i].Y = coord1.y;
                                    }
                                    */
                                    for (int j = 0; j < (secaux.poligonos.Count()); j++)
                                    {
                                        for (int i = 0; i < (secaux.poligonos[j].coords.Count()); i++)
                                        {
                                            coord1 = new vec3(secaux.poligonos[j].coords[i].X, secaux.poligonos[j].coords[i].Y, 0);
                                            coord1 = coord1.Rotate(centroRotacao, ((o as TBarraGenerica).Dados.anguloRotacao) * Const.PIDiv180);

                                            secaux.poligonos[j].coords[i].X = coord1.x;
                                            secaux.poligonos[j].coords[i].Y = coord1.y;
                                        }
                                    }
                                }

                                (o as TBarraGenerica).Dados.secao = (TSecao)secaux.Clone();
                                (o as TBarraGenerica).Dados.secaoSemRotacao = (TSecao)secaoSemRotacao.Clone();
                                (o as TBarraGenerica).Dados.idsecao = idSecao;
                            }
                        }
                    }
                }
            }
            else
            {
                TBarraGenerica bar;
                if (alterouAngulo)
                {
                    TSecao secaux = new TSecao();
                    foreach (TObjetoDesenho o in gerenciador.formDesenho.ObjetosSelecionados)
                    {
                        if (o.Tipo == Const.ID_BARRAGENERICA)
                        {
                            if ((o as TBarraGenerica).Dados.Tipo == 4 && gerenciador.formDesenho.Estrutura.barras.Exists(bb => bb.id_barra_rigida_1 == (o as TBarraGenerica).IDBarra || bb.id_barra_rigida_2 == (o as TBarraGenerica).IDBarra))
                                continue;

                            bar = (o as TBarraGenerica);
                            //  secaux = gerenciador.formDesenho.Secoes.Find(se => se.id == bar.Dados.idsecao);

                            if ((Object)bar.Dados.secaoSemRotacao != null)
                            {
                                secaux = (TSecao)bar.Dados.secaoSemRotacao.Clone();

                                centroX = pixelX(0);
                                centroY = pixelY(0);

                            //    secaux.poligono.CalculaPropriedades();

                                centroRotacao.y = 0;
                                centroRotacao.x = 0;

                              /*  for (int i = 0; i < (secaux.poligono.coords.Count()); i++)
                                {
                                    coord1 = new vec3(secaux.poligono.coords[i].X, secaux.poligono.coords[i].Y, 0);
                                    coord1 = coord1.Rotate(centroRotacao, anguloRotacao * Const.PIDiv180);

                                    secaux.poligono.coords[i].X = coord1.x;
                                    secaux.poligono.coords[i].Y = coord1.y;
                                }*/

                                for (int j = 0; j < (secaux.poligonos.Count()); j++)
                                {
                                    for (int i = 0; i < (secaux.poligonos[j].coords.Count()); i++)
                                    {
                                        coord1 = new vec3(secaux.poligonos[j].coords[i].X, secaux.poligonos[j].coords[i].Y, 0);
                                        coord1 = coord1.Rotate(centroRotacao, ((o as TBarraGenerica).Dados.anguloRotacao) * Const.PIDiv180);

                                        secaux.poligonos[j].coords[i].X = coord1.x;
                                        secaux.poligonos[j].coords[i].Y = coord1.y;
                                    }
                                }
                            }

                            (o as TBarraGenerica).Dados.secao = (TSecao)secaux.Clone();
                            (o as TBarraGenerica).Dados.anguloRotacao = anguloRotacao;
                        }
                    }
                }
            }
        }

        void Salvar()
        {
            if (edAngulo.Text.Trim() != string.Empty)
              anguloRotacao = System.Convert.ToDouble(edAngulo.Text);

            if (alterando)
            {
                //    AtualizaSecoesEstrutura(-1);
                this.DialogResult = System.Windows.Forms.DialogResult.Yes;
                /*foreach (TObjetoDesenho o in ObjetosSelecionados)
                {
                    if (o.Tipo == Const.ID_BARRAGENERICA)
                    {
                        (o as TBarraGenerica).Dados.secao = (TSecao)gerenciador.DadosBarra.secaoCopia.Clone();
                        (o as TBarraGenerica).Dados.secaoSemRotacao = (TSecao)gerenciador.DadosBarra.secao.Clone();
                        (o as TBarraGenerica).Dados.idsecao = gerenciador.DadosBarra.idSecao;
                        (o as TBarraGenerica).Dados.anguloRotacao = gerenciador.DadosBarra.anguloRotacao;
                    }
                }*/

                /* foreach (TSecao sec in Secoes)
                 {
                     secaoCopia = (TSecao)sec.Clone();
                     secaoCopia.poligono.CalculaPropriedades();

                     foreach (TBarraGenerica b in Barras)
                     {
                         if (b.Dados.secao.id == sec.id)
                         {
                         }
                     }
                 }*/
                AlterarElementos();
                gerenciador.formDesenho.Alterou(true);

                /*         gerenciador.formDesenho.CancelaInsercoes();
                         gerenciador.formDesenho.SetaSelecionados(false, -1);
                         gerenciador.formDesenho.Atualiza_Segmentos_e_Snap();*/
                //     AtualizaCargas();

                // gerenciador.DadosBarra = null;
                //gerenciador.formDesenho.CancelaInsercoes();
            }
            else
            {
                if ((Object)secao_click != null)
                {
                    gerenciador.formDesenho.DadosBarra = new TDadosBarra(
                                           (int.Parse)(numero.Text),
                                            System.Convert.ToInt32(cbTipo.SelectedIndex),
                                            (TSecao)secao_click.Clone(), (TSecao)secaoSemRotacao.Clone(), idSecao, anguloRotacao);

                    gerenciador.formDesenho.DadosBarra.Articulacao_my = ArtMy.SelectedIndex;
                    gerenciador.formDesenho.DadosBarra.Articulacao_mz = ArtMz.SelectedIndex;
                    gerenciador.formDesenho.DadosBarra.k_my           = System.Convert.ToDouble(edRigMY.Text);
                    gerenciador.formDesenho.DadosBarra.k_mz           = System.Convert.ToDouble(edRigMZ.Text);

                    if (cbTipo.SelectedIndex == 4)
                    {
                        gerenciador.formDesenho.DadosBarra.Articulacao_my = 0;
                        gerenciador.formDesenho.DadosBarra.Articulacao_mz = 0;
                    }

                    gerenciador.formDesenho.DadosBarra.ey = System.Convert.ToDouble(edEy.Text);
                    gerenciador.formDesenho.DadosBarra.ez = System.Convert.ToDouble(edEz.Text);
                    gerenciador.formDesenho.DadosBarra.ex_i = System.Convert.ToDouble(edEx_i.Text);
                    gerenciador.formDesenho.DadosBarra.ex_f = System.Convert.ToDouble(edEx_f.Text);

                    gerenciador.ComandoNovaBarra();
                    gerenciador.formDesenho.Comando.Text = "Novo elemento - " + secaoSemRotacao.descricao + " - Selecione o primeiro ponto:";

                }
            }

            foreach (TSecao sec in gerenciador.formDesenho.Secoes)
                sec.alterou = false;
       
            /*if (alterando)
            {
                GC.Collect();
              //  gerenciador.DadosBarra = null;
                Close();

            }*/
        }
        double precisaoPixel;
        public bool multi;

        private void FDadosBarra_Load(object sender, EventArgs e)
        {
            if (gerenciador.x_ult_formBarras == 0 && gerenciador.y_ult_formBarras == 0)
            {
                this.Left = 25;
                this.Top = (int)((gerenciador.Height - this.Height) / 2);
            }
            else
            {
                this.Left = gerenciador.x_ult_formBarras;
                this.Top = gerenciador.y_ult_formBarras;
            }

         //   this.Left = gerenciador.Width - (this.Width);

            ponto_zero = new double[2];
            ponto_zero[0] = 0;
            ponto_zero[1] = pnDesenhoSecao.Height;

            if ((Dados != null && !multi))
              idSecao = Dados.secao.id;

            precisaoPixel = 0.8f;

            if (alterando)
            {
                gbSecoes.Text = "Seções do projeto";

                if (!multi)
                {
                    edAngulo.Text = Dados.anguloRotacao.ToString("n2");
                    anguloRotacao = Dados.anguloRotacao;
                    ArtMy.SelectedIndex = Dados.Articulacao_my;
                    ArtMz.SelectedIndex = Dados.Articulacao_mz;
                    cbTipo.SelectedIndex = Dados.Tipo;
                    edRigMY.Text = Dados.k_my.ToString("n2");
                    edRigMZ.Text = Dados.k_mz.ToString("n2");
                    edEy.Text = Dados.ey.ToString("n2");
                    edEz.Text = Dados.ez.ToString("n2");
                    edEx_i.Text = Dados.ex_i.ToString("n2");
                    edEx_f.Text = Dados.ex_f.ToString("n2");
                }
                else
                {
                    ArtMy.Items.Insert(0,"");
                    ArtMz.Items.Insert(0, "");
                    cbTipo.Items.Insert(0, "");

                    edEy.Clear();
                    edEz.Clear();
                    edEx_i.Clear();
                    edEx_f.Clear();

                    edRigMY.Clear();
                    edRigMZ.Clear();

                    ArtMy.SelectedIndex = 0;
                    ArtMz.SelectedIndex = 0;
                    cbTipo.SelectedIndex = 0;
                    edAngulo.Clear();
                }
            }
            else
            {
                this.Text = "Novo elemento";
                gbSecoes.Text = "Seções do projeto";
                ArtMy.SelectedIndex = 0;
                ArtMz.SelectedIndex = 0;
                cbTipo.SelectedIndex = 0;
            }

            pnInfoBarra.Height = 13;
            CarregaSecoes();

            this.DoubleBuffered = true;
            this.SetStyle(ControlStyles.OptimizedDoubleBuffer, true);

            if (gerenciador.formDesenho.Secoes.Count > 0 && !alterando)
            {
                secaoSemRotacao = gerenciador.formDesenho.Secoes[0];
                secao_click = (TSecao)secaoSemRotacao.Clone();
                //   RodarSecao();
                //   Enquadrar();
                RodarSecao();
            }
            if (alterando)
                RodarSecao(true);

            Enquadrar();

            if (!alterando) //seleciona o tipo de seção que esta selecionada. Isso facilita p usuario
            {
                if (gerenciador.formDesenho.Barras.Exists(o => o.Selecionado))
                {
                    TBarraGenerica bar = gerenciador.formDesenho.Barras.First(o => o.Selecionado);
                    if (bar != null)
                    {
                        int idsec = bar.Dados.secao.id;
                        idSecao = idsec;
                        int ii = gerenciador.formDesenho.Secoes.FindIndex(o => o.id == idsec);
                        cbSecoes.SelectedIndex = ii;
                        cbTipo.SelectedIndex = bar.Dados.Tipo;
                        cbSecoes_SelectedIndexChanged(cbSecoes, null);
                        RodarSecao(true);
                    }
                }
            }
        }

        public void CarregaSecoes()
        {
            int ii = -1;
            cbSecoes.Items.Clear();
            if (alterando && multi)
            {
                //    var item1 = new ListBoxItem("<>");
                //  ComboBoxItem it = new ComboBoxItem();
                //     it. = (string)"<>";
                string itemm = "<>";
                cbSecoes.Items.Add(itemm);

                ii = 0;
            }

            foreach (TSecao sec in gerenciador.formDesenho.Secoes)
            {
                ii++;
                cbSecoes.Items.Add(sec.descricao);

                if (alterando)
                    if (sec.id == idSecao)
                    {
                        cbSecoes.SelectedIndex = ii;
                        lbDescricaoSecao.Text = "Seção: " + sec.id + " - " + sec.descricao;
                        pnCorSecao.BackColor = System.Drawing.Color.FromArgb(sec.Rgb[0], sec.Rgb[1], sec.Rgb[2]);
                    }
            }

            if (alterando && multi && cbSecoes.SelectedIndex < 1)
                cbSecoes.SelectedIndex = 0;

            cbSecoes_SelectedIndexChanged(cbSecoes, null);
        }

        private void FDadosBarra_FormClosed(object sender, FormClosedEventArgs e)
        {
            if (!alterando)
            {
                gerenciador.y_ult_formBarras = this.Top;
                gerenciador.x_ult_formBarras = this.Left;

                gerenciador.DadosBarra = null;
                if (!gerenciador.formDesenho.chamouEdicaoElemento)
                    gerenciador.formDesenho.CancelaInsercoes();
            }
            else
            {
                gerenciador.y_ult_formBarras = this.Top;
                gerenciador.x_ult_formBarras = this.Left;
                gerenciador.formDesenho.chamouEdicaoElemento = false;
            }
           /* if (SecaoGenerica != null)
                SecaoGenerica.Close();

            if (SecaoLaminada != null)
                SecaoLaminada.Close();
            if (SecaoSolida != null)
                SecaoSolida.Close();*/
            FecharJanelas();
            this.gerenciador = null;
            mPen.Dispose();
            PenVerde.Dispose();
            PenAzul.Dispose();
            GC.Collect();
        }
        void FecharJanelas()
        {
            FormCollection fc = Application.OpenForms;
            List<Form> forms = new List<Form>();

            foreach (Form frm in fc)
                if (frm != null)
                    if ((string)frm.Tag == "Secoes")
                        forms.Add(frm);

            foreach (Form frm in forms)
                frm.Close();
        }

        private void FDadosBarra_Move(object sender, EventArgs e)
        {
        //    gerenciador.AtualizaDesenho();
        }
        public FSecaoSolida SecaoSolida;
        public FEscolheSecao SecaoLaminada;
        public FSecaoGenerica SecaoGenerica;
        public int id;

        private void FDadosBarra_Activated(object sender, EventArgs e)
        {
     //       gerenciador.AtualizaDesenho();
        }

        public int idSecao = -1;
        TSecao secaoCopia2;
        public void AtualizaSecoesEstrutura(int id)
        { 
            pnDesenhoSecao.Focus();
            //refaz a geometria da seçao secoes em toda as estrutura, caso haja modificação do usuário na
            foreach (TBarraGenerica b in gerenciador.formDesenho.Barras)
            {
                if (b.Dados.secao.id == secaoSemRotacao.id)
                {
                    secaoCopia2 = (TSecao)secaoSemRotacao.Clone();
                    b.Dados.secaoSemRotacao = (TSecao)secaoCopia2.Clone();

                  /*  if (b.Dados.secao.poligono != null)
                    {
                        secaoCopia2.poligono.CalculaPropriedades();
                        centroRotacao.x = secaoCopia2.poligono.centroide.X;
                        centroRotacao.y = secaoCopia2.poligono.centroide.Y;

                        for (int i = 0; i < (secaoCopia2.poligono.coords.Count()); i++)
                        {
                            coord1 = new vec3(secaoCopia2.poligono.coords[i].X, secaoCopia2.poligono.coords[i].Y, 0);
                            coord1 = coord1.Rotate(centroRotacao, (b.Dados.anguloRotacao) * Const.PIDiv180);

                            secaoCopia2.poligono.coords[i].X = coord1.x;
                            secaoCopia2.poligono.coords[i].Y = coord1.y;
                        }
                    }*/


                    if (b.Dados.secao.poligonos != null)
                    {
                        centroRotacao.x = 0;
                        centroRotacao.y = 0;

                        for (int j = 0; j < (secaoCopia2.poligonos.Count()); j++)
                        {
                            for (int i = 0; i < (secaoCopia2.poligonos[j].coords.Count()); i++)
                            {
                                coord1 = new vec3(secaoCopia2.poligonos[j].coords[i].X, secaoCopia2.poligonos[j].coords[i].Y, 0);
                                coord1 = coord1.Rotate(centroRotacao, (b.Dados.anguloRotacao) * Const.PIDiv180);

                                secaoCopia2.poligonos[j].coords[i].X = coord1.x;
                                secaoCopia2.poligonos[j].coords[i].Y = coord1.y;
                            }
                        }
                    }
           //         b.DirtyTriangulos = true;
              //      b.DirtyArestas = true;
                    b.Dados.secao = (TSecao)secaoCopia2.Clone();

                    if (b.Dados.secao.poligonos != null)
                      b.OrientaSecaoNoEspaco();
                }
            }
            gerenciador.formDesenho.AtualizaShaders();
            gerenciador.formDesenho.DesenhaObjetos();
            gerenciador.formDesenho.glControl.SwapBuffers();
        }

        public void RodarSecao(bool carregando = false)
        {
            if ((Object)secaoSemRotacao != null)
            {
                Pontos.Clear();
                secao_click = (TSecao)secaoSemRotacao.Clone();

                centroX = pixelX(0);
                centroY = pixelY(0);

                if (secao_click.poligonos != null)
                {
                    //secao_click.poligono.CalculaPropriedades();

                    centroRotacao.y = 0;
                    centroRotacao.x = 0;

                    for (int j = 0; j < secao_click.poligonos.Count; j++)
                    {
                        for (int i = 0; i < (secao_click.poligonos[j].coords.Count()); i++)
                        {
                            coord1 = new vec3(secao_click.poligonos[j].coords[i].X, secao_click.poligonos[j].coords[i].Y, 0);
                            coord1 = coord1.Rotate(centroRotacao, (anguloRotacao) * Const.PIDiv180);

                            secao_click.poligonos[j].coords[i].X = coord1.x;
                            secao_click.poligonos[j].coords[i].Y = coord1.y;
                        }

                        for (int i = 0; i < (secao_click.poligonos[j].coords.Count()); i++)
                        {
                            Pontos.Add(new TPonto(secao_click.poligonos[j].coords[i].X, secao_click.poligonos[j].coords[i].Y, 0));
                            Pontos[Pontos.Count - 1].px_x = pixelX(Pontos[Pontos.Count - 1].x);
                            Pontos[Pontos.Count - 1].px_y = pixelY(Pontos[Pontos.Count - 1].y);
                        }
                    }

                    centroX = pixelX(0);
                    centroY = pixelY(0);
                    GetMiniMaxPt();
                }
            }

           /*  if ((Object)secaoSemRotacao != null)
             {
                 Pontos.Clear();
                 secao_click = (TSecao)secaoSemRotacao.Clone();

                 centroX = pixelX(secao_click.poligono.centroide.X);
                 centroY = pixelY(secao_click.poligono.centroide.Y);

                 if (secao_click.poligono != null)
                 {
                     secao_click.poligono.CalculaPropriedades();

                     centroRotacao.y = secao_click.poligono.centroide.Y;
                     centroRotacao.x = secao_click.poligono.centroide.X;

                     if (Geom.Iguais(secao_click.poligono.centroide.X, 0))
                         centroRotacao.x = 0;
                     if (Geom.Iguais(secao_click.poligono.centroide.Y, 0))
                         centroRotacao.y = 0;

                     for (int i = 0; i < (secao_click.poligono.coords.Count()); i++)
                     {
                         coord1 = new vec3(secao_click.poligono.coords[i].X, secao_click.poligono.coords[i].Y, 0);
                         coord1 = coord1.Rotate(centroRotacao, (anguloRotacao) * Const.PIDiv180);

                         secao_click.poligono.coords[i].X = coord1.x;
                         secao_click.poligono.coords[i].Y = coord1.y;
                     }
                     for (int i = 0; i < (secao_click.poligono.coords.Count()); i++)
                     {
                         Pontos.Add(new TPonto(secao_click.poligono.coords[i].X, secao_click.poligono.coords[i].Y, 0));
                         Pontos[Pontos.Count - 1].px_x = pixelX(Pontos[Pontos.Count - 1].x);
                         Pontos[Pontos.Count - 1].px_y = pixelY(Pontos[Pontos.Count - 1].y);
                     }

                     centroX = pixelX(secao_click.poligono.centroide.X);
                     centroY = pixelY(secao_click.poligono.centroide.Y);
                     GetMiniMaxPt();

                 }
             }*/

            if (carregando)
              Enquadrar();
        }

        public void DesenhaSecao()
        {
            try
            {
                //   pnDesenhoSecao_Paint(pnDesenhoSecao, ControleCAD);
                //  ControleCAD.SmoothingMode = SmoothingMode.AntiAlias;
                this.Refresh();

                /* for (int i = 0; i < (Pontos.Count()) - 1; i++)
                 {
                     //  if (i + 1 > secaoCopia.poligono.coords.Count())
                     //     break;
                     ControleCAD.DrawLine(mPen, (int)(Pontos[i].px_x), (int)(Pontos[i].px_y),
                               (int)(Pontos[i + 1].px_x), (int)(Pontos[i + 1].px_y));
                 }
                 ControleCAD.DrawRectangle(mPen, (int)(centroX - 2), (int)(centroY - 2),
                           2, 2);*/

                /* if ((Object)secao != null)
                     if (ControleCAD != null)
                     {
                         for (int i = 0; i < (Pontos.Count()) - 1; i++)
                         {
                             //  if (i + 1 > secaoCopia.poligono.coords.Count())
                             //     break;
                             ControleCAD.DrawLine(mPen, (int)(Pontos[i].px_x), (int)(Pontos[i].px_y),
                                       (int)(Pontos[i + 1].px_x), (int)(Pontos[i + 1].px_y));
                         }
                     }*/
                //    timer1.Enabled = false;
            }
            catch (Exception e)
            {
                MessageBox.Show(e.Message);
            }
        }
        System.Drawing.Drawing2D.GraphicsState EstadoCad;
        private void pnDesenhoSecao_Paint(object sender, PaintEventArgs e)
        {
            //  ControleCAD = e.Graphics;
            //      ControleCAD.DrawLine(mPen, 0, 0, 50, 50);
            //   e.Graphics.SmoothingMode = SmoothingMode.AntiAlias;

            //        RodarSecao();
            try
            {
                if (secao_click != null && idSecao > -1)
                {
                    /*centroX = pixelX(secaoCopia.poligono.centroide.X);
                    centroY = pixelY(secaoCopia.poligono.centroide.Y);*/

                    //  ControleCAD = e.Graphics;

                    for (int j = 0; j < secao_click.poligonos.Count; j++)
                    {
                        for (int i = 0; i < (secao_click.poligonos[j].coords.Count() - 1); i++)
                        {
                            e.Graphics.DrawLine(mPen, (int)(pixelX(secao_click.poligonos[j].coords[i].X)), (int)(pixelY(secao_click.poligonos[j].coords[i].Y)),
                                                      (int)(pixelX(secao_click.poligonos[j].coords[i + 1].X)), (int)(pixelY(secao_click.poligonos[j].coords[i + 1].Y)));
                        }
                    }

                    //      for (int i = 0; i < (Pontos.Count()) - 1; i++)
                    //          e.Graphics.DrawLine(mPen, (int)(Pontos[i].px_x), (int)(Pontos[i].px_y), (int)(Pontos[i + 1].px_x), (int)(Pontos[i + 1].px_y));

                    e.Graphics.DrawRectangle(mPen, (int)(centroX - 2), (int)(centroY - 2), 2, 2);
                    double px = 0, py = 0;

                    px = centroX + (30 * Math.Cos(anguloRotacao * Const.PIDiv180));
                    py = centroY - (30 * Math.Sin(anguloRotacao * Const.PIDiv180));
                    e.Graphics.DrawLine(PenVerde, (int)(centroX), (int)(centroY), (int)(px), (int)py);

                    px = centroX + (35 * Math.Cos(anguloRotacao * Const.PIDiv180));
                    py = centroY - (35 * Math.Sin(anguloRotacao * Const.PIDiv180));
                    e.Graphics.DrawString("Y", Fonte, BrushVerde, (int)(px), (int)py);

                    px = centroX + (33 * Math.Cos((90 + anguloRotacao) * Const.PIDiv180));
                    py = centroY - (33 * Math.Sin((90 + anguloRotacao) * Const.PIDiv180));
                    e.Graphics.DrawLine(PenAzul, (int)(centroX), (int)(centroY), (int)(px), (int)py);

                    px = centroX + (50 * Math.Cos((90 + anguloRotacao) * Const.PIDiv180));
                    py = centroY - (50 * Math.Sin((90 + anguloRotacao) * Const.PIDiv180));
                    e.Graphics.DrawString("Z", Fonte, BrushAzul, (int)px, (int)py);
                }
                else
                    e.Graphics.Clear(System.Drawing.Color.White);

            }
            catch (Exception exc)
            {
                MessageBox.Show("erro onpaint:" + centroX.ToString() + " , " + centroY.ToString() + " - " + exc.Message);
            }
            //  enquadrar();
        }
        System.Drawing.SolidBrush BrushVerde = new System.Drawing.SolidBrush(System.Drawing.Color.Green);
        System.Drawing.SolidBrush BrushAzul = new System.Drawing.SolidBrush(System.Drawing.Color.Blue);

        System.Drawing.Font Fonte = new System.Drawing.Font("Arial", (float)(10), System.Drawing.FontStyle.Bold);
        System.Drawing.Graphics ControleCAD;
        private void edX_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (e.KeyChar == (char)Keys.Enter || e.KeyChar == (char)Keys.Escape)
            {
                e.Handled = true;   // stop annoying beep
            }

            if (!char.IsControl(e.KeyChar) && !char.IsDigit(e.KeyChar) && (e.KeyChar != '.') && (e.KeyChar != '-'))
            {
                e.Handled = true;
            }

            // only allow one decimal point
            if ((e.KeyChar == '.') && ((sender as System.Windows.Forms.TextBox).Text.IndexOf('.') > -1))
            {
                e.Handled = true;
            }

        }

        System.Drawing.Pen mPen = new System.Drawing.Pen(System.Drawing.Color.Black);
        System.Drawing.Pen PenVerde = new System.Drawing.Pen(System.Drawing.Color.Green,3);
        System.Drawing.Pen PenAzul = new System.Drawing.Pen(System.Drawing.Color.Blue,3);

        double offset_x_1, offset_y_1, offset_x_ant, offset_y_ant, centroX, centroY;
        vec3 coord1 = new vec3(0, 0, 0);
        vec3 coord2 = new vec3(0, 0, 0);
        vec3 centroRotacao = new vec3(0, 0, 0);
        double angAnterior;
        public TSecao secao_click;
        System.Drawing.Graphics drawing;


        private void timer1_Tick(object sender, EventArgs e)
        {
            // pnDesenhoSecao.Refresh();
            Enquadrar();
            timer1.Enabled = false;
            //this.pnDesenhoSecao.Refresh();
            //  pnDesenhoSecao.Invalidate();
        }

        public float pixelX(float coordX)
        {
            if (!Geom.Iguais(precisaoPixel, 0, 0.000001))
            {
                if (precisaoPixel > 0)
                    return (float)(coordX / precisaoPixel + ponto_zero[0]);
                else
                    return 0;
            }
            else
                return 0;
        }

        public float pixelY(float coordY)
        {
            if (!Geom.Iguais(precisaoPixel, 0, 0.000001))
            {
                if (precisaoPixel > 0)
                    return (float)(coordY / -precisaoPixel + ponto_zero[1]);
                else
                    return 0;
            }
            else
                return 0;
        }

        public float pixelX(double coordX)
        {
            if (!Geom.Iguais(precisaoPixel, 0, 0.000001))
            {
                if (precisaoPixel > 0)
                    return (float)(coordX / precisaoPixel + ponto_zero[0]);
                else
                    return 0;
            }
            else
                return 0;
        }

        public float pixelY(double coordY)
        {
            if (!Geom.Iguais(precisaoPixel, 0, 0.000001))
            {
                if (precisaoPixel > 0)
                    return (float)(coordY / -precisaoPixel + ponto_zero[1]);
                else
                    return 0;
            }
            else
                return 0;
        }

        double fatorZoom = 1.28f;


        public void panel_MouseWheel(object sender, System.Windows.Forms.MouseEventArgs e)
        {
            if (e.Delta > 0)
            {
                ponto_zero[0] = Posicao.X + ((ponto_zero[0] - Posicao.X) * fatorZoom);
                ponto_zero[1] = Posicao.Y + ((ponto_zero[1] - Posicao.Y) * fatorZoom);

                for (int i = 0; i < Pontos.Count; i++)
                {
                    Pontos[i].px_x = (float)(Posicao.X + ((Pontos[i].px_x - Posicao.X) * fatorZoom));
                    Pontos[i].px_y = (float)(Posicao.Y + ((Pontos[i].px_y - Posicao.Y) * fatorZoom));
                };
                centroX = (Posicao.X + ((centroX - Posicao.X) * fatorZoom));

                centroY = (Posicao.Y + ((centroY - Posicao.Y) * fatorZoom));

                precisaoPixel = (float)(precisaoPixel / fatorZoom);
            }
            else
            {
                ponto_zero[0] = Posicao.X + ((ponto_zero[0] - Posicao.X) / fatorZoom);
                ponto_zero[1] = Posicao.Y + ((ponto_zero[1] - Posicao.Y) / fatorZoom);

                for (int i = 0; i < Pontos.Count; i++)
                {
                    Pontos[i].px_x = (float)(Posicao.X + ((Pontos[i].px_x - Posicao.X) / fatorZoom));
                    Pontos[i].px_y = (float)(Posicao.Y + ((Pontos[i].px_y - Posicao.Y) / fatorZoom));
                };

                centroX = (Posicao.X + ((centroX - Posicao.X) / fatorZoom));

                centroY = (Posicao.Y + ((centroY - Posicao.Y) / fatorZoom));

                precisaoPixel = (float)(precisaoPixel * fatorZoom);
            };
            pnDesenhoSecao.Invalidate();
            Invalidate();
            //   DesenhaSecao2();
        }
        public static double[] ponto_zero;
        List<TPonto> Pontos = new List<TPonto>();
        double p_xini, p_yini, p_xfin, p_yfin, xIni, yIni, xFin, yFin;
        void GetMiniMaxPt()
        {
            p_xini = 999999999;
            p_yini = 999999999;
            p_xfin = 999999999;
            p_yfin = 999999999;

            xIni = 999999999;
            yIni = 999999999;

            xFin = -999999999;
            yFin = -999999999;

            for (int i = 0; i < Pontos.Count; i++)
            {
                if (Pontos[i].x < xIni)
                {
                    xIni = Pontos[i].x;
                    p_xini = Pontos[i].px_x;
                };

                if (Pontos[i].y < yIni)
                {
                    yIni = Pontos[i].y;
                    p_yini = Pontos[i].px_y;
                };

                if (Pontos[i].x > xFin)
                {
                    xFin = Pontos[i].x;
                    p_xfin = Pontos[i].px_x;
                };

                if (Pontos[i].y > yFin)
                {
                    yFin = Pontos[i].y;
                    p_yfin = Pontos[i].px_y;
                };
            };
        }

        private void button7_Click(object sender, EventArgs e)
        {
            if (SecaoLaminada == null)
            {
                SecaoLaminada = new FEscolheSecao(this, this.gerenciador);
                DialogResult result = SecaoLaminada.ShowDialog();
             //   SecaoLaminada.Show();
            }
        }

        private void button8_Click(object sender, EventArgs e)
        {
            if (multi && cbSecoes.SelectedIndex == 0) // primeiro item selecionado '<>'
                return;

            if (cbSecoes.Items.Count > 0)
            {
                int sel = cbSecoes.SelectedIndex;
                if (gerenciador.formDesenho.Barras.Exists(p => p.Dados.secao.id == secao_click.id))
                {
                    MessageBox.Show("Não é possível deletar essa seção. Existem elementos associados à ela.", "Atenção", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                    return;
                }

                DialogResult dlgresult = MessageBox.Show("Deseja apagar a seção?"
                                , "Apagar", MessageBoxButtons.YesNo, MessageBoxIcon.Question);
                if (dlgresult == DialogResult.Yes)
                {
                    if (multi)
                    {
                        gerenciador.formDesenho.Secoes.RemoveAt(cbSecoes.SelectedIndex - 1);
                        cbSecoes.Items.RemoveAt(cbSecoes.SelectedIndex);
                    }
                    else
                    {
                        gerenciador.formDesenho.Secoes.RemoveAt(cbSecoes.SelectedIndex);
                        cbSecoes.Items.RemoveAt(cbSecoes.SelectedIndex);

                    }

                    gerenciador.formDesenho.Alterou(true,false);
                    //         CarregaSecoes();

                    if ((!multi && cbSecoes.Items.Count > 0) || (multi && cbSecoes.Items.Count > 1))
                    {
                            cbSecoes.SelectedIndex = 0;

                  //      cbSecoes.SelectedIndex = (sel - 1 == 0 ? sel : sel + 1);
                    }
                    else
                    if ((!multi && cbSecoes.Items.Count == 0) || (multi && cbSecoes.Items.Count == 1))
                    {   
                        secao_click = null;
                        secaoSemRotacao = null;
                        RodarSecao();
                        DesenhaSecao();
                        
                    }
                    //cbSecoes.SelectedIndex = cbSecoes.SelectedIndex - 1;
                }
            }
        }

        private void chExcentricidades_CheckedChanged(object sender, EventArgs e)
        {

        }

        private void chArticulacoes_CheckedChanged(object sender, EventArgs e)
        {

        }

        private void lbSecoes_SelectedIndexChanged(object sender, EventArgs e)
        {

        }
        List<TBarraGenerica> bars;

        private void tabPage2_Click(object sender, EventArgs e)
        {

        }

        private void edEy_KeyUp(object sender, KeyEventArgs e)
        {
            if ((sender as System.Windows.Forms.TextBox).Text.Trim() == "" && !multi)
            {
                (sender as System.Windows.Forms.TextBox).Text = "0";
            }
        }

        private void tabControl1_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (tabControl1.SelectedIndex == 0)
                edAngulo.Focus();
            else
            if (tabControl1.SelectedIndex == 1)
            {
                edEy.Focus();
                edEy.SelectAll();   
            }
            else
            if (tabControl1.SelectedIndex == 2)
                ArtMy.Focus();
        }

        void CalculaExcentricidades(ref double maxy, ref double maxz)
        {
            TSecao secaux;
            maxy = -99999;
            maxz = -99999;

            if ((Object)secao_click != null)
            {
                maxy = secao_click.propriedades.cy;
                maxz = secao_click.propriedades.cz;

              /*  secaux = (TSecao)secao_click.Clone();

                double centroX = pixelX(secaux.poligono.centroide.X);
                double centroY = pixelY(secaux.poligono.centroide.Y);

                secaux.poligono.CalculaPropriedades();

                centroRotacao.y = secaux.poligono.centroide.Y;
                centroRotacao.x = secaux.poligono.centroide.X;
                for (int i = 0; i < (secaux.poligono.coords.Count()); i++)
                {
                    if (secaux.poligono.coords[i].Y > maxz)
                        maxz = secaux.poligono.coords[i].Y;

                    if (secaux.poligono.coords[i].X > maxy)
                        maxy = secaux.poligono.coords[i].X;
                }*/
            }
            else
            {
                maxy *= 0;
                maxz *= 0;
            }
          //  maxy *= 1000;
       //     maxz *= 1000;

        }

        private void topoesq_CheckedChanged(object sender, EventArgs e)
        {

        }

        private void topodir_CheckedChanged(object sender, EventArgs e)
        {

        }

        private void meioesq_CheckedChanged(object sender, EventArgs e)
        {

        }

        private void meio_CheckedChanged(object sender, EventArgs e)
        {


        }

        private void baixoesq_CheckedChanged(object sender, EventArgs e)
        {

        }

        private void baixomeio_CheckedChanged(object sender, EventArgs e)
        {

        }

        private void baixodir_CheckedChanged(object sender, EventArgs e)
        {

        }

        private void topomeio_CheckedChanged(object sender, EventArgs e)
        {

        }

        private void topoesq_Click(object sender, EventArgs e)
        {

        }

        private void topomeio_Click(object sender, EventArgs e)
        {

        }

        private void topodir_Click(object sender, EventArgs e)
        {

        }

        private void meioesq_Click(object sender, EventArgs e)
        {

        }

        private void meio_Click(object sender, EventArgs e)
        {

        }

        private void meiodir_Click(object sender, EventArgs e)
        {

        }

        private void baixoesq_Click(object sender, EventArgs e)
        {

        }

        private void baixomeio_Click(object sender, EventArgs e)
        {

        }

        private void baixodir_Click(object sender, EventArgs e)
        {

        }

        private void checkBox2_CheckedChanged(object sender, EventArgs e)
        {

        }

        private void chFlexMZ_CheckedChanged(object sender, EventArgs e)
        {
        }

        private void ArtMz_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (ArtMz.SelectedIndex == 0)
            {
                edRigMZ.Text = "0";
                edRigMZ.Enabled = false;
            }
            else
                edRigMZ.Enabled = true;

        }

        private void ArtMy_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (ArtMy.SelectedIndex == 0)
            {
                edRigMY.Text = "0";
                edRigMY.Enabled = false;
            }
            else
                edRigMY.Enabled = true;
        }

        private void tabPage1_Click(object sender, EventArgs e)
        {

        }

        private void button1_Click(object sender, EventArgs e)
        {
            double maxy = 0, maxz = 0;
            CalculaExcentricidades(ref maxy, ref maxz);

            edEz.Text = (maxz).ToString("n2");
            edEy.Text = (maxy).ToString("n2");
        }

        private void button2_Click(object sender, EventArgs e)
        {
            double maxy = 0, maxz = 0;
            CalculaExcentricidades(ref maxy, ref maxz);

            edEz.Text = (maxz).ToString("n2");
            edEy.Text = (0).ToString("n2");
        }

        private void button4_Click(object sender, EventArgs e)
        {
            double maxy = 0, maxz = 0;
            CalculaExcentricidades(ref maxy, ref maxz);

            edEz.Text = (maxz).ToString("n2");
            edEy.Text = (-maxy).ToString("n2");
        }

        private void button12_Click(object sender, EventArgs e)
        {
            double maxy = 0, maxz = 0;
            CalculaExcentricidades(ref maxy, ref maxz);

            edEz.Text = (-maxz).ToString("n2");
            edEy.Text = (0).ToString("n2");
        }

        private void button11_Click(object sender, EventArgs e)
        {
            double maxy = 0, maxz = 0;
            CalculaExcentricidades(ref maxy, ref maxz);

            edEz.Text = (-maxz).ToString("n2");
            edEy.Text = (maxy).ToString("n2");
        }

        private void button9_Click(object sender, EventArgs e)
        {
            double maxy = 0, maxz = 0;
            CalculaExcentricidades(ref maxy, ref maxz);

            edEz.Text = (-maxz).ToString("n2");
            edEy.Text = (-maxy).ToString("n2");
        }

        private void button5_Click(object sender, EventArgs e)
        {
            double maxy = 0, maxz = 0;
            CalculaExcentricidades(ref maxy, ref maxz);

            edEz.Text = (0).ToString("n2");
            edEy.Text = (maxy).ToString("n2");
        }

        private void button10_Click(object sender, EventArgs e)
        {
            double maxy = 0, maxz = 0;
            CalculaExcentricidades(ref maxy, ref maxz);

            edEz.Text = (0).ToString("n2");
            edEy.Text = (-maxy).ToString("n2");
        }

        private void button13_Click(object sender, EventArgs e)
        {
            edEz.Text = (0).ToString("n2");
            edEy.Text = (0).ToString("n2");
        }

        public  void cbSecoes_SelectedIndexChanged(object sender, EventArgs e)
        {
            if ((cbSecoes.SelectedIndex > -1 && !multi) || (cbSecoes.SelectedIndex > 0 && multi))
            {
                if ((alterando && cbSecoes.SelectedIndex == 0 && multi))
                    idSecao = -1;
                else
                if ((alterando && cbSecoes.SelectedIndex > 0 && multi))
                    idSecao = gerenciador.formDesenho.Secoes[cbSecoes.SelectedIndex - 1].id;
                else
                    idSecao = gerenciador.formDesenho.Secoes[cbSecoes.SelectedIndex].id;

                secao_click = null;
                secaoSemRotacao = null;
                foreach (TSecao sec in gerenciador.formDesenho.Secoes)
                {
                    if (sec.id == idSecao)
                    {
                        secaoSemRotacao = sec;

                        secao_click = (TSecao)secaoSemRotacao.Clone();

                        lbDescricaoSecao.Text = "Seção: " + secaoSemRotacao.id + " - " + secaoSemRotacao.descricao;
                        pnCorSecao.BackColor = System.Drawing.Color.FromArgb(secaoSemRotacao.Rgb[0], secaoSemRotacao.Rgb[1], secaoSemRotacao.Rgb[2]);

                        break;
                    }
                }

                if (!alterando)
                    Salvar();
                gerenciador.formDesenho.Focus();

                timer1.Enabled = true;

                RodarSecao();
                //  Enquadrar();
                DesenhaSecao();
            }
            else
            {
                idSecao = -1;
                secao_click = null;
                secaoSemRotacao = null;
                DesenhaSecao();
            }
        }

        private void chMostrar_CheckedChanged(object sender, EventArgs e)
        {

        }
        System.Windows.Forms.ToolTip tipBotoes = new System.Windows.Forms.ToolTip();
        private void button14_MouseEnter(object sender, EventArgs e)
        {
            System.Windows.Forms.Button btn = (System.Windows.Forms.Button)sender;

            tipBotoes.SetToolTip(btn, btn.AccessibleName);
        }

        private void button14_Click(object sender, EventArgs e)
        {
            if (idSecao > -1)
            {
                if (gerenciador.formDesenho.Barras.Exists(o => o.Dados.secao.id == idSecao))
                {
                    gerenciador.formDesenho.Barras.ForEach(o => o.SetaSelecao(false, false));
                    gerenciador.formDesenho.Barras.FindAll(o => o.Dados.secao.id == idSecao).ForEach(o => o.SetaSelecao(true, false));
                    List<TBarraGenerica> bars = gerenciador.formDesenho.Barras.FindAll(o => o.Dados.secao.id == idSecao);
                    gerenciador.formDesenho.ObjetosSelecionados.AddRange(bars);

                    gerenciador.formDesenho.AtualizaShaders();
                    gerenciador.formDesenho.DesenhaObjetos();
                    gerenciador.formDesenho.glControl.SwapBuffers();
                }
                else
                {
                    MessageBox.Show("Essa seção não é utilizada em nenhum elemento.", "Informação", MessageBoxButtons.OK);
                }
            }
        }

        private void btAlterarSecao_Click(object sender, EventArgs e)
        {
            /*    if (secaoSemRotacao == null) return;

                if (SecaoLaminada == null)
                    SecaoLaminada = new FEscolheSecao(this, this.gerenciador);

                SecaoLaminada.Show();
                SecaoLaminada.id = idSecao;
                SecaoLaminada.secao = secaoSemRotacao;

                SecaoLaminada.Carrega(secaoSemRotacao.tipo);*/

            if (cbSecoes.SelectedIndex == -1) // primeiro item selecionado '<>'
                return;

            if (multi && cbSecoes.SelectedIndex == 0) // primeiro item selecionado '<>'
                return;

            string template = secaoSemRotacao.template;
            string CaminhoTemplate = Directory.GetCurrentDirectory() + @"\TemplateSec\" + template;

            if (File.Exists(CaminhoTemplate + "_vec"))
            {
                AbrirSecao(false, template, CaminhoTemplate, secaoSemRotacao.descricao, ref secaoSemRotacao.valoresCotas, ref secaoSemRotacao.valoresRaios);
            }
            else
            {
                MessageBox.Show("O template da seção não existe! Contate o desenvolvedor.\r Template: " + CaminhoTemplate, "Atenção", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
        }
        public FCalculaSecao calculasecao;
        void AbrirSecao(bool editar, string template, string caminho_template, string nome, ref List<double> valorCotas, ref List<double> valorRaios, bool biblioteca = false)
        {
            calculasecao = new FCalculaSecao(gerenciador, template, caminho_template, nome, false, 
                ref valorCotas, ref valorRaios, 
                biblioteca, true,
                secaoSemRotacao);
            calculasecao.dadosBarra = this;

            DialogResult result = calculasecao.ShowDialog();

            if (result == DialogResult.OK)
            {
                gerenciador.formDesenho.Alterou(true, false);

                //     gerenciador.formDesenho.AtualizaShaders(true);
           //     gerenciador.formDesenho.DesenhaObjetos();
           //     gerenciador.formDesenho.glControl.SwapBuffers();
                //        gerenciador.formDesenho.glControl.MakeCurrent();
                
                //atualizar as geometrias no 3d
                gerenciador.formDesenho.AtualizaShaders();
                gerenciador.formDesenho.DesenhaObjetos();
                gerenciador.formDesenho.glControl.SwapBuffers();
            }
        }

        private void cbTipo_SelectedIndexChanged(object sender, EventArgs e)
        {

        }

        private void FDadosBarra_Shown(object sender, EventArgs e)
        {

        }

        private void edAngulo_TextChanged(object sender, EventArgs e)
        {

        }

        public void lbSecoes_Click(object sender, EventArgs e)
        {

        }

        private void lbSecoes_MouseDoubleClick(object sender, MouseEventArgs e)
        {

        }

        public void Enquadrar()
        {
            if (secao_click != null)
            {
                if (Pontos.Count > 0)
                {
                    GetMiniMaxPt();

                    ponto_zero[0] = ponto_zero[0] - p_xini + 20;
                    ponto_zero[1] = ponto_zero[1] - p_yfin + 20;

                    for (int i = 0; i < Pontos.Count; i++)
                    {
                        Pontos[i].px_x = pixelX(Pontos[i].x);
                        Pontos[i].px_y = pixelY(Pontos[i].y);
                    };

                    GetMiniMaxPt();

                    //      ponto_zero[0] = ponto_zero[0] - p_xini + 10;
                    //   ponto_zero[1] = ponto_zero[1] - p_yfin + 10;



                    //  GetMiniMaxPt();

                    for (int c = 0; c < 9; c++)
                    {
                        ponto_zero[0] = p_xini + ((ponto_zero[0] - p_xini) / fatorZoom);
                        ponto_zero[1] = p_yfin + ((ponto_zero[1] - p_yfin) / fatorZoom);

                        for (int i = 0; i < Pontos.Count; i++)
                        {
                            Pontos[i].px_x = (float)(p_xini + ((Pontos[i].px_x - p_xini) / fatorZoom));
                            Pontos[i].px_y = (float)(p_yfin + ((Pontos[i].px_y - p_yfin) / fatorZoom));
                        };

                        precisaoPixel = (float)(precisaoPixel * fatorZoom);
                    }
                    /*         
                     *  ponto_zero[0] = Posicao.X + ((ponto_zero[0] - Posicao.X) * fatorZoom);
                        ponto_zero[1] = Posicao.Y + ((ponto_zero[1] - Posicao.Y) * fatorZoom);
                     * 
                     * Pontos[i].px_x = (float)(Posicao.X + ((Pontos[i].px_x - Posicao.X) * fatorZoom));
                       Pontos[i].px_y = (float)(Posicao.Y + ((Pontos[i].px_y - Posicao.Y) * fatorZoom));*/

                    for (int c = 0; c < 500; c++)
                    {
                        GetMiniMaxPt();

                        if ((p_xfin < pnDesenhoSecao.Width && p_xfin > (pnDesenhoSecao.Width - 20)) ||
                            (p_yini < pnDesenhoSecao.Height && p_yini > (pnDesenhoSecao.Height - 20)))
                            break;

                        ponto_zero[0] = p_xini + ((ponto_zero[0] - p_xini) * 1.1);
                        ponto_zero[1] = p_yfin + ((ponto_zero[1] - p_yfin) * 1.1);

                        for (int i = 0; i < Pontos.Count; i++)
                        {
                            Pontos[i].px_x = (float)(p_xini + ((Pontos[i].px_x - p_xini) * 1.1));
                            Pontos[i].px_y = (float)(p_yfin + ((Pontos[i].px_y - p_yfin) * 1.1));
                        };
                        //  if ((precisaoPixel / 1.01) == 0) break;

                        precisaoPixel = (float)(precisaoPixel / 1.1);

                    }
                }

                centroX = pixelX(0);
                centroY = pixelY(0);
                //RodarSecao();
                DesenhaSecao();
            }
            // g2d.ClearScreen(r, g, b);

            //  DrawByLayer();
            // Controle.SwapBuffers();
        }

        public double anguloRotacao = 0;
        private void edAngulo_Validated(object sender, EventArgs e)
        {
            if (edAngulo.Text.Trim() != string.Empty)
            {
                anguloRotacao = System.Convert.ToDouble(edAngulo.Text);
                RodarSecao();
                DesenhaSecao();
                Enquadrar();
            }
        }

        private void edAngulo_KeyPress(object sender, KeyPressEventArgs e)
        {
            char a = Convert.ToChar(System.Threading.Thread.CurrentThread.CurrentCulture.NumberFormat.NumberDecimalSeparator);
            if (!char.IsControl(e.KeyChar) && !char.IsDigit(e.KeyChar) && (e.KeyChar != a) && (e.KeyChar != '-'))
            {
                e.Handled = true;
            }

            if (e.KeyChar == (char)Keys.Enter || e.KeyChar == (char)Keys.Escape)
            {
                e.Handled = true;   // stop annoying beep
            }
            base.OnKeyPress(e);
        }

        private void edAngulo_KeyUp(object sender, KeyEventArgs e)
        {
            if (edAngulo.Text.Trim() == "" && !multi)
            {
                edAngulo.Text = "0";
              //  MessageBox.Show("Ângulo não deve ser nulo");
            }
            else
            if ((edAngulo.Text.Trim() != "-") && (edAngulo.Text.Trim() != ",") && (edAngulo.Text.Trim() != "."))
            {
                if (edAngulo.Text.Trim() != string.Empty)
                    anguloRotacao = System.Convert.ToDouble(edAngulo.Text);
                //  angAnterior = anguloRotacao;
                RodarSecao();
                DesenhaSecao();
                Enquadrar();

            }
        }
        private void edAngulo_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
            {
                btSalvar.Focus();
                btSalvar_Click(btSalvar, null);
            }
        }
        public static Coordenada Posicao;
        private void pnDesenhoSecao_MouseMove(object sender, MouseEventArgs e)
        {
            Posicao.X = e.X;
            Posicao.Y = e.Y;
        }

        private void FDadosBarra_Paint(object sender, PaintEventArgs e)
        {
            //         RodarSecao();
        }

        private void button6_Click(object sender, EventArgs e)
        {
            Enquadrar();
        }

    }
}
