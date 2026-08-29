using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Threading;
using System.Windows.Forms;

namespace PG
{
    [Serializable]
    public partial class TPorticoEspacial : TModeloEstrutural
    {
        public bool calculoOk = true;
        public bool Manual;
        public List<TPavimento> Pavimentos;
        [NonSerialized]
        public TBarraPortico[] barras;
        [NonSerialized]
        public TNoPortico[] nos;
        [NonSerialized]
        public TMatrizBanda MatrizRigidez;
        static int Numero;
        public Gerenciador gerenciador;

        public int Ndj = 6;  //número de vínculos possíveis em um nó

        public int nBarras, nNos;
        int i, j, k;
        public int LarguraBanda;

        public int Ngl,
                    NglBarra,
                    NLinhas,
                    NumeroDeRestricoes,
                    NumeroDeNosComRestricoes,
                    SequenciaPavimento, divBarrasPortico;

        public int NumeroRepeticoes;
        [NonSerialized]
        double[] df;
        [NonSerialized]
        double[] forcas;
        [NonSerialized]
        public int[] id;
        [NonSerialized]
        public int[] Linhas; //vetor que retorna o número da linha/coluna da matriz em que o gl se encontra
        [NonSerialized]
        double[] K_Mola;
        [NonSerialized]
        bool[] glRestrito;
        List<TTrechoViga> trechos;
        List<TPilar> pilares;
        [NonSerialized]
        List<TBarraGenerica> barrasgenericas;
        [NonSerialized]
        List<TApoio> apoios;
        [NonSerialized]
        List<TMateriais> materiais;
        List<TCargaLinear> cargaslineares;
        List<TCargaPontual> cargaspontuais;
        List<Forcas_Portico> casos_x_forcas;
        List<Forcas_Portico> combinacoes_x_forcas;
        List<Deslocamentos_Portico> casos_x_deslocamentos, combinacoes_x_deslocamentos;

        public TPorticoEspacial()
        {
            nos = new TNoPortico[1];
        }
        public TPorticoEspacial(Gerenciador _gerenciador, bool _Manual)
        {
            this.Manual = _Manual;
            this.gerenciador = _gerenciador;
            this.Progresso = gerenciador.processo.Progresso;
            this.Tipo = "PORTICO";
            InicializaNosBarras();
            Ndj = 6;
        }

        public TPorticoEspacial(List<TPavimento> _Pavimentos, List<TBarraGenerica> _barras, List<TApoio> _apoios, List<TMateriais> _materiais, List<TCargaPontual> CargasPontuais,
            List<TCargaLinear> CargasLineares, Gerenciador _gerenciador, int divisoesBarras)
        {
            this.Pavimentos = _Pavimentos;
            this.gerenciador = _gerenciador;
            this.Progresso = gerenciador.processo.Progresso;
            this.Tipo = "PORTICO";
            trechos = new List<TTrechoViga>();
            pilares = new List<TPilar>();
            barrasgenericas = new List<TBarraGenerica>();
            barrasgenericas = _barras;
            apoios = new List<TApoio>();
            apoios = _apoios;
            materiais = new List<TMateriais>();
            materiais = _materiais;
            cargaslineares = new List<TCargaLinear>();
            cargaslineares= CargasLineares;
            cargaspontuais = new List<TCargaPontual>();
            cargaspontuais = CargasPontuais;
            divBarrasPortico = divisoesBarras;
            /* foreach (TPavimento p in Pavimentos)
             {
                 foreach (TObjetoDesenho t in p.LayersByIdPrincipal[Lay.Vigas].Obj)
                 {
                     if (t.Tipo == Const.ID_TRECHOVIGA)
                         trechos.Add(t as TTrechoViga);
                 }
                 foreach (TObjetoDesenho t in p.LayersByIdPrincipal[Lay.Pilares].Obj)
                 {
                     if (t.Tipo == Const.ID_PILAR)
                         pilares.Add(t as TPilar);
                 }
             }*/

            Ndj = 6;
        }
        public double Maximo_U_Total(int tipocarga, int caso, int comb)
        {
            double max = 0;
            double utotal;
            Deslocamentos_Nos deslocamento;

            if ((tipocarga == 0 && caso > -1)
            ||
            (tipocarga == 1 && comb > -1))
            {
                for (i = 1; i <= nNos; i++)
                {
                    if (tipocarga == 0)
                    {
                        // nos[i].casos_x_deslocamentos[caso].U_Total = 1;// Math.Sqrt(Math.Pow(nos[i].casos_x_deslocamentos[caso].DeslocamentoGlobal[1], 2) + Math.Pow(nos[i].casos_x_deslocamentos[caso].DeslocamentoGlobal[2], 2) + Math.Pow(nos[i].casos_x_deslocamentos[caso].DeslocamentoGlobal[3], 2));
                        //      deslocamento = nos[i].casos_x_deslocamentos[caso];

                        utotal = nos[i].casos_x_deslocamentos[caso].U_Total;
                    }
                    else
                    {
                        utotal = nos[i].combinacoes_x_deslocamentos[comb].U_Total;
                    }

                    if (Math.Abs(utotal) > max)
                        max = Math.Abs(utotal);
                }

                return max;
            }

            return 0;
        }

        public double MaximoDeslocamento(int deslocamento, int tipocarga, int caso, int comb)
        {
            double max = 0;
            double[] p_Deslocamento;
            if ((tipocarga == 0 && caso > -1)
            ||
            (tipocarga == 1 && comb > -1))
            {
                for (i = 1; i <= nNos; i++)
                {
                    if (tipocarga == 0)
                        p_Deslocamento = nos[i].casos_x_deslocamentos[caso].DeslocamentoGlobal;
                    else
                        p_Deslocamento = nos[i].combinacoes_x_deslocamentos[comb].DeslocamentoGlobal;

                    if (Math.Abs(p_Deslocamento[deslocamento]) > max)
                        max = Math.Abs(p_Deslocamento[deslocamento]);
                }

                return max;
            }
            else
                return 0;
        }

        public double MaximoDeslocamento(int tipocarga, int caso, int comb)
        {
            double max = 0;
            double[] p_Deslocamento;

            if ((tipocarga == 0 && caso > -1)
                ||
            (tipocarga == 1 && comb > -1))
            {
                for (i = 1; i <= nNos; i++)
                {
                    if (tipocarga == 0)
                        p_Deslocamento = nos[i].casos_x_deslocamentos[caso].DeslocamentoGlobal;
                    else
                        p_Deslocamento = nos[i].combinacoes_x_deslocamentos[comb].DeslocamentoGlobal;

                    if (Math.Abs(p_Deslocamento[1]) > max)
                        max = Math.Abs(p_Deslocamento[1]);
                }

                for (i = 1; i <= nNos; i++)
                {
                    if (tipocarga == 0)
                        p_Deslocamento = nos[i].casos_x_deslocamentos[caso].DeslocamentoGlobal;
                    else
                        p_Deslocamento = nos[i].combinacoes_x_deslocamentos[comb].DeslocamentoGlobal;

                    if (Math.Abs(p_Deslocamento[2]) > max)
                        max = Math.Abs(p_Deslocamento[2]);
                }


                for (i = 1; i <= nNos; i++)
                {
                    if (tipocarga == 0)
                        p_Deslocamento = nos[i].casos_x_deslocamentos[caso].DeslocamentoGlobal;
                    else
                        p_Deslocamento = nos[i].combinacoes_x_deslocamentos[comb].DeslocamentoGlobal;

                    if (Math.Abs(p_Deslocamento[3]) > max)
                        max = Math.Abs(p_Deslocamento[3]);
                }

                return max;
            }
            else
                return 0;
        }

        public double MaximoEsforco(string s, int tipocarga, int caso, int comb, bool somenteSelecionados = false)
        {
            double max = 0;
            if ((tipocarga == 0 && caso > -1)
               ||
                (tipocarga == 1 && comb > -1))
            {

                int i1 = 1, i2 = 1;
                if (s == "fx") { i1 = 1; i2 = 7; }
                else if (s == "fy") { i1 = 3; i2 = 9; }
                else if (s == "fz") { i1 = 2; i2 = 8; }
                else if (s == "mx") { i1 = 4; i2 = 10; }
                else if (s == "my") { i1 = 6; i2 = 12; }
                else if (s == "mz") { i1 = 5; i2 = 11; }

                double[] esforco;

                for (i = 1; i <= nBarras; i++)
                {
                    if ((somenteSelecionados && barras[i].barraOriginal.Selecionado) || (!somenteSelecionados))
                        if (!barras[i].barraRigida)
                        {
                            if (tipocarga == 0)
                                esforco = barras[i].casos_x_esforcos[caso].Esforcos;
                            else
                                esforco = barras[i].combinacoes_x_esforcos[comb].Esforcos;

                            if (Math.Abs(esforco[i1]) > max)
                                max = Math.Abs(esforco[i1]);
                        }
                }

                for (i = 1; i <= nBarras; i++)
                {
                    if ((somenteSelecionados && barras[i].barraOriginal.Selecionado) || (!somenteSelecionados))
                        if (!barras[i].barraRigida)
                        {
                            if (tipocarga == 0)
                                esforco = barras[i].casos_x_esforcos[caso].Esforcos;
                            else
                                esforco = barras[i].combinacoes_x_esforcos[comb].Esforcos;

                            if (Math.Abs(esforco[i2]) > max)
                                max = Math.Abs(esforco[i2]);
                        }
                }

                return max;
            }
            else
                return 0;
        }

        public double MaximaRotacao(int tipocarga, int caso, int comb)
        {
            double max = 0;
            double[] p_Deslocamento;
            if ((tipocarga == 0 && caso > -1)
                ||
            (tipocarga == 1 && comb > -1))
            {
                for (i = 1; i <= nNos; i++)
                {
                    if (tipocarga == 0)
                        p_Deslocamento = nos[i].casos_x_deslocamentos[caso].DeslocamentoGlobal;
                    else
                        p_Deslocamento = nos[i].combinacoes_x_deslocamentos[comb].DeslocamentoGlobal;

                    if (Math.Abs(p_Deslocamento[4]) > max)
                        max = Math.Abs(p_Deslocamento[4]);
                }

                for (i = 1; i <= nNos; i++)
                {
                    if (tipocarga == 0)
                        p_Deslocamento = nos[i].casos_x_deslocamentos[caso].DeslocamentoGlobal;
                    else
                        p_Deslocamento = nos[i].combinacoes_x_deslocamentos[comb].DeslocamentoGlobal;

                    if (Math.Abs(p_Deslocamento[5]) > max)
                        max = Math.Abs(p_Deslocamento[5]);
                }

                for (i = 1; i <= nNos; i++)
                {
                    if (tipocarga == 0)
                        p_Deslocamento = nos[i].casos_x_deslocamentos[caso].DeslocamentoGlobal;
                    else
                        p_Deslocamento = nos[i].combinacoes_x_deslocamentos[comb].DeslocamentoGlobal;

                    if (Math.Abs(p_Deslocamento[6]) > max)
                        max = Math.Abs(p_Deslocamento[6]);
                }

                return max;
            }
            return 0;
        }
        public override void MsgCalculo(string titulo, string texto, int max, bool fim = false, bool MostraProgresso = true)
        {
            gerenciador.processo.LabelProcesso.Text = texto;
            gerenciador.processo.PanelCalculo.Update();
            //  gerenciador.BringToFront();
            /*  gerenciador.PanelCalculo.Visible = true;
              Progresso.Visible = MostraProgresso;
              Progresso.Value = 0;
              Progresso.Maximum = System.Convert.ToInt32(max);
              gerenciador.lbProgresso.Text = texto;

              if (fim)
                Progresso.Visible = false;
              //  gerenciador.SendToBack();
              //  gerenciador.BringToFront();
              System.Windows.Forms.Application.DoEvents();
              gerenciador.Update();*/
        }
        public override void HistoricoCalculo(string texto, bool Edit = false, bool erro = false)
        {
            System.Windows.Forms.Application.DoEvents();
            if (erro)
            {
                //    gerenciador.processo.Height = 270;
                gerenciador.processo.LabelProcesso.Visible = true;
                    gerenciador.processo.LabelProcesso.Text = texto;
            }
            else
            {
                if (Edit)
                    gerenciador.processo.ListaCalculo.Items[gerenciador.processo.ListaCalculo.Items.Count - 1] = texto;
                else
                    gerenciador.processo.ListaCalculo.Items.Insert(gerenciador.processo.ListaCalculo.Items.Count, texto);

                gerenciador.processo.ListaCalculo.SelectedIndex = gerenciador.processo.ListaCalculo.Items.Count - 1;
            }

            //       Progresso.Value = 0;
            //      gerenciador.panel2.Visible = false;
        }
        public double MaximoEsforco(int indice)
        {
            double max = 0;

            for (i = 1; i <= nBarras; i++)
            {
                 if (barras[i].Esforcos != null)
                {
                    if (Math.Abs(barras[i].Esforcos[indice]) > max)
                        max = barras[i].Esforcos[indice]; //mom fletor no nó ini

                    if (Math.Abs(barras[i].Esforcos[indice + Ndj]) > max)
                        max = barras[i].Esforcos[indice + Ndj];  //mom fletor no nó fin
                }
            }

            return max;
        }

        private int GetNumeroDeRestricoes()
        {
            int nr = 0;

            for (int i = 1; i <= nNos; i++)
              nr += nos[i].GetNumeroDeRestricoes();

            return nr;
        }

        private int GetNumeroDeNosComRestricoes()
        {
            int nr = 0;

            for (i = 1; i <= nNos; i++)
              if (nos[i].PossuiRestricao()) nr++;

            return nr;
        }

        private bool CriarVetores()
        {
            Ndj = 6;
            Ngl = (nNos * Ndj);
            NglBarra = Ndj * 2;
            NLinhas = Ngl - NumeroDeRestricoes;

            this.glRestrito = new bool[Ngl + 1];
            this.id = new int[Ngl + 1];
            this.Linhas = new int[Ngl + 1 ];
            this.forcas = new double[NLinhas+1];
            this.df = new double[NLinhas+1];

            return true;
        }
        private bool CriarMatrizSFF(bool UsarDll)
        {
            Atualiza(1);

            if (UsarDll && gerenciador.ConfiguracoesPGi.CfgProjeto.sistema.Solver_gradiente_conjugado)
            {
                mBanda_Dll = new double[NLinhas][];
                for (int i = 0; i < mBanda_Dll.Length; i++)
                    mBanda_Dll[i] = new double[LarguraBanda];
            }
            else
            {
                if (MatrizRigidez != null)
                  MatrizRigidez = null;

             //   if (Testes)
                  //  gerenciador.ConfiguracoesPGi.CfgProjeto.sistema.Solver1 = true;

                MatrizRigidez = null;
                MatrizRigidez = new TMatrizBanda(NLinhas, LarguraBanda, Ngl, this, gerenciador.ConfiguracoesPGi.CfgProjeto.sistema.Solver_choleskypadrao, gerenciador.ConfiguracoesPGi.CfgProjeto.sistema.UsarDll);
            }

            return true;
        }

        private bool DadosEstruturais()
        {
            Atualiza(1);
            NumeroDeRestricoes = GetNumeroDeRestricoes();
            NumeroDeNosComRestricoes = GetNumeroDeNosComRestricoes();

            if (!CriarVetores())
                throw new TErroPortico(this, "Pórtico Espacial  -  Erro na criação dos vetores.");

            int numero;
            Atualiza(1);
            //preenche o vetor glRestrito que diz para cada GL se este está restrito ou nao
            for (i = 1; i <= nNos; i++)
            {
                numero = nos[i].Numero;
                for (j = 1; j <= 6; j++)
                  glRestrito[(numero - 1) * Ndj + j] = nos[i].Restricao[j];
            }

            //indice das equações para cada grau de liberdade em cada nó na matriz guardada no formato de banda
            int n1 = 0;
            for (j = 1; j <= Ngl; j++)
            {
                //       id[j] = j - n1;

                if (glRestrito[j])
                    n1++;

                if (!glRestrito[j])
                    id[j] = j - n1;
                else
                    id[j] = NLinhas + n1;
            }
            //calcula a largura da banda
            int nbi = 0;
            LarguraBanda = 0;

            for (i = 1; i <= nBarras; i++)
            {
                nbi = Ndj * (Math.Abs(barras[i].pFin.Numero - barras[i].pIni.Numero) + 1);

                if (nbi > LarguraBanda)
                    LarguraBanda = nbi;
            }
            // LarguraBanda = NLinhas;
            return true;
        }

        double[][] mBanda_Dll;
        private bool MatrizDeRigidez(bool UsarDll)
        {
            try
            {
                Atualiza(1);
                int ir, ic, i1, i2, linAux;
                double coef1, coef2;
                // MsgCalculo("", "Pórtico - Gerando sistema de equações...", nBarras, false, false);
                HistoricoCalculo("      > Gerando o sistema de equações...");
                #region solver2
                if (!gerenciador.ConfiguracoesPGi.CfgProjeto.sistema.Solver_choleskypadrao)
                {
                    for (i = 1; i <= nBarras; i++)
                    {
                        for (j = 1; j <= NglBarra; j++)
                        {
                            i1 = barras[i].GlGlobal[j];

                            if (!glRestrito[i1])
                            {
                                for (k = j; k <= NglBarra; k++)
                                {
                                    i2 = barras[i].GlGlobal[k];

                                    if (!glRestrito[i2])
                                    {
                                        ir = id[i1] - 1;
                                        ic = id[i2] - 1;

                                        coef1 = alglib.sparseget(MatrizRigidez.s, ir, ic);
                                        coef2 = barras[i].MatrizGlobal[j, k];

                                        alglib.sparseset(MatrizRigidez.s, ir, ic, coef1 + coef2);
                                        alglib.sparseset(MatrizRigidez.s, ic, ir, coef1 + coef2);
                                    }
                                }
                            }
                        }
                    }
                }
                #endregion
                else
                {
                    for (i = 1; i <= nBarras; i++)
                    {
                        for (j = 1; j <= NglBarra; j++)
                        {
                            i1 = barras[i].GlGlobal[j];

                            if (!glRestrito[i1])
                            {
                                for (k = j; k <= NglBarra; k++)
                                {
                                    i2 = barras[i].GlGlobal[k];

                                    if (!glRestrito[i2])
                                    {
                                        ir = id[i1];
                                        ic = id[i2];

                                        if (ir >= ic)
                                        {
                                            linAux = ir;

                                            ir = ic;
                                            ic = linAux;
                                        }

                                        ic = ic - ir + 1;

                                        /* Zeros e um*/
                                        if (glRestrito[i1])
                                        {
                                            if (i1 == i2)
                                                MatrizRigidez.Sff[ir * LarguraBanda + ic] = 1;
                                            else
                                                MatrizRigidez.Sff[ir * LarguraBanda + ic] = 0;
                                        }
                                        else
                                        if (glRestrito[i2])
                                            MatrizRigidez.Sff[ir * LarguraBanda + ic] = 0;
                                        else
                                            MatrizRigidez.Sff[ir * LarguraBanda + ic] += barras[i].MatrizGlobal[j, k];

                                        /* if (Testes)
                                         {
                                             lin = ir - 1;
                                             col = (ic - 1) + lin;
                                             MatrizRigidez.Sff_[lin][col] += barras[i].MatrizGlobal[j, k];
                                         }*/
                                    }
                                }
                            }
                        }
                    }
                }

                /*for (int i = 1; i <= NLinhas; i++)
                {
                    int li = i - 1;
                    int co = li;
                    double valk = alglib.sparseget(MatrizRigidez.s, li, co);
                    if (Geom.Iguais(valk, 0,0.00001) || valk < 3)
                    {
                        HistoricoCalculo("Estrutura é instável");
                        return false;
                    }
                }*/

                int gl;
                /*Insere coef. de mola*/
                for (int i = 0; i < Ngl; i++)
                {

                    if (K_Mola[i] != 0)
                    {
                        gl = id[i + 1] - 1;

                        if (!glRestrito[gl])
                        {
                            double coef = alglib.sparseget(MatrizRigidez.s, gl, gl);
                            coef += K_Mola[i];
                            alglib.sparseset(MatrizRigidez.s, gl, gl, coef);
                        }
                    }
                }

                alglib.sparseconverttocrs(MatrizRigidez.s);

                HistoricoCalculo("      - Geração do sistema de equações [" + NLinhas + "] - [Ok]", true);
                MostraMatrizRigidez();
                return true;
            }
            catch (Exception ee)
            {
                MessageBox.Show("erro ao montar matriz de rigidez: elemento: " + barras[i].barraOriginal.IDBarra);
                return false;
            }
        
        }

        private bool SetCoeficientesMola()
        {
            K_Mola = new double[Ngl];
            int gl;
/*
            for (i = 1; i <= nNos; i++)
            {
                numero = nos[i].Numero;
                for (j = 1; j <= Ndj; j++)  
                {
                    gl = (numero - 1) * Ndj + j;

                    if (!glRestrito[gl])
                    {
                        jr = id[gl];
                        aj[jr] = nos[i].Carga[j];
                    }
                }
            }*/

            for (i = 1; i <= nNos; i++)
            {
                gl = ((nos[i].Numero - 1) * Ndj);
             //   if (!glRestrito[gl])
                {

                    if (nos[i].PossuiMolaDX)
                        K_Mola[gl] = nos[i].K_Mola_DX;

                    if (nos[i].PossuiMolaDY)
                        K_Mola[gl+2] = nos[i].K_Mola_DY;

                    if (nos[i].PossuiMolaDZ)
                        K_Mola[gl+1] = nos[i].K_Mola_DZ;

                    if (nos[i].PossuiMolaRX)
                        K_Mola[gl + 3] = nos[i].K_Mola_RX;

                    if (nos[i].PossuiMolaRY)
                        K_Mola[gl + 5] = nos[i].K_Mola_RY;

                    if (nos[i].PossuiMolaRZ)
                        K_Mola[gl+4] = nos[i].K_Mola_RZ;
            }
            }

            return true;
        }



        private bool AplicarVinculos()
        {
            Atualiza(1);
            for (j = 1; j <= nNos; j++)
            {
                if (nos[j].vinculo == 1)
                {
                    nos[j].restrDX = true;
                    nos[j].restrDY = true;
                    nos[j].restrDZ = true;
                    //         nos[j].restrDY = true;
                }
                else
                    if (nos[j].vinculo == 2)
                    {
                        nos[j].restrDX = true;
                        nos[j].restrDY = true;
                        nos[j].restrDZ = true;

                        nos[j].restrRX = true;
                        nos[j].restrRY = true;
                        nos[j].restrRZ = true;
                    }
                    else
                        if (nos[j].vinculo == 3)
                        {
                            nos[j].PossuiMolaRX = true;
                            nos[j].PossuiMolaRY = true;
                            nos[j].PossuiMolaDZ = true;

                            nos[j].K_Mola_DZ = 6;
                            nos[j].K_Mola_RX = 1;
                            nos[j].K_Mola_RY = 1;
                        }
            }

            return true;
        }

        private void MensagemPortico()
        {

        }

        public bool ReordenarNos()
        {
            Atualiza(1);
            // MsgCalculo("", "Pórtico - Reordenação nodal...", 0);

            HistoricoCalculo("      > Renumeração dos nós...");
            TReordenaNosPortico.ReordenaNos(ref nos, barras, nNos, nBarras, this);
            HistoricoCalculo("      - Renumeração dos nós [Largura da semi-banda: " + maxBandaReordenacao.ToString() + "] - [Ok]", true);

            return true;
        }
        private bool SetMatrizesBarras()
        {
            Atualiza(1);
            for (int i = 1; i <= nBarras; i++)
            {
                barras[i].SetGlGlobal();
                barras[i].SetMatrizLocal();

                if (barras[i].articulacao_my_ini || 
                    barras[i].articulacao_my_fin || 
                    barras[i].articulacao_mz_ini || 
                    barras[i].articulacao_mz_fin)
                    barras[i].SetMatrizLocalSemiRigida(/*25, 25, 1e17, 1e17*/);

                barras[i].SetMatrizRotacao(); 

                barras[i].SetMatrizGlobal();
            }

            return true;
        }

        struct Forcas_Portico
        {
            public double[] forcasBarras;
            public int id;//id da combinacao ou do caso 
            public Forcas_Portico(double[] forcas, int _id)
            {
                forcasBarras = forcas;
                this.id = _id;
            }
        }
        public struct Deslocamentos_Portico
        {
            public double[] df;
            public int id; //id da combinacao ou do caso 
            public Deslocamentos_Portico(double[] _df, int _id)
            {
                df = _df;
                this.id = _id;
            }
        }

        private bool Resultados()
        {
            try
            {
                CalculaPesoTotal();

                int glGlobal, jr, gl, numero;

                double[] dj = new double[NLinhas + 1];

                HistoricoCalculo("      > Calculando esforços..");
                Application.DoEvents();

                for (int caso = 0; caso < casos_x_deslocamentos.Count; caso++)
                {
                    Atualiza(1);

                    for (j = 1; j <= nNos; j++)
                    {
                        numero = nos[j].Numero;

                        nos[j].casos_x_deslocamentos.Add(new Deslocamentos_Nos(caso));

                        int cc = nos[j].casos_x_deslocamentos.Count;

                        for (int k = 1; k <= Ndj; k++)
                        {
                            gl = (numero - 1) * Ndj + k;

                            if (!glRestrito[gl])
                            {
                                glGlobal = nos[j].GlGlobal[k];

                                jr = id[glGlobal];
                                nos[j].casos_x_deslocamentos[caso].DeslocamentoGlobal[k] = casos_x_deslocamentos[caso].df[jr];
                            }
                        }
                    }

                    for (i = 1; i <= nBarras; i++)
                        barras[i].CalcularEsforcos(Const.ID_TIPO_CASO, caso);
                }

                TSuavizacaoDiagramas suavizar1;

                for (int caso = 0; caso < casos_x_deslocamentos.Count; caso++)
                    suavizar1 = new TSuavizacaoDiagramas(barrasDividas_X_barrasPortico, Const.ID_TIPO_CASO, caso);


                //  ------   COMBINACOES  --------
           //     Atualiza(1);
                for (int comb = 0; comb < combinacoes_x_deslocamentos.Count; comb++)
                {
                    Atualiza(1);

                    for (j = 1; j <= nNos; j++)
                    {
                        numero = nos[j].Numero;

                        nos[j].combinacoes_x_deslocamentos.Add(new Deslocamentos_Nos(comb));

                        int cc = nos[j].combinacoes_x_deslocamentos.Count;

                        for (int k = 1; k <= Ndj; k++)
                        {
                            gl = (numero - 1) * Ndj + k;

                            if (!glRestrito[gl])
                            {
                                glGlobal = nos[j].GlGlobal[k];

                                jr = id[glGlobal];
                                nos[j].combinacoes_x_deslocamentos[comb].DeslocamentoGlobal[k] = combinacoes_x_deslocamentos[comb].df[jr];
                            }
                        }
                    }

                    for (i = 1; i <= nBarras; i++)
                        barras[i].CalcularEsforcos(Const.ID_TIPO_COMBINACAO, comb);
                }

             //   for (int comb = 0; comb < combinacoes_x_deslocamentos.Count; comb++)
                 //   suavizar1 = new TSuavizacaoDiagramas(barrasDividas_X_barrasPortico, Const.ID_TIPO_COMBINACAO, comb);

              //  Atualiza(1);
                gerenciador.processo.ListaCalculo.Items[gerenciador.processo.ListaCalculo.Items.Count - 1] = "      - Cálculo de esforços [Ok]";

                CalcularTensoes();


                /* for (int k = 1; k <= NLinhas; k++)
                 {
                     //  dj[k] = df[id[k]]; //a linha da matriz referente ao grau de liberdade é id[k]
                     dj[k] = df[k];
                 }

                 for (j = 1; j <= nNos; j++)
                 {
                     Atualiza(1);
                     numero = nos[j].Numero;
                     for (int k = 1; k <= Ndj; k++)
                     {
                         gl = (numero - 1) * Ndj + k;

                         if (!glRestrito[gl])
                         {
                             glGlobal = nos[j].GlGlobal[k];

                             jr = id[glGlobal];
                             nos[j].Deslocamento[k] = dj[jr];
                         }
                     }
                 }

                 for (i = 1; i <= nBarras; i++)
                     barras[i].CalcularEsforcos("caso", 1);

                 TSuavizacaoDiagramas suavizar = new TSuavizacaoDiagramas(barrasDividas_X_barrasPortico,1);

                 HistoricoCalculo("      > Cálculo de esforços - [Ok]");
                 */
                return true;
            }
            catch(Exception ee)
            {
                Progresso.Value = 0;
                MessageBox.Show("erro ao calcular os esforços");
                return false;   
            }
            
        }



        public bool Carregamentos(List<TCombinacoes> combinacoes, List<TCasosCarga> casos)
        {
            Atualiza(1);
            /*double[] aj = new double[Ngl + 1];
            double[] ae = new double[Ngl + 1];
            */

            double[] forcasNos = new double[NLinhas+1];
            double[] forcasNos_Casos = new double[NLinhas + 1];

            double[] ae = new double[NLinhas+1];
            double[] forcasBarras_Casos = new double[NLinhas + 1];
            double[] forcasBarras_Combinacoes = new double[NLinhas + 1];

            int numero;
           // for (i = 1; i <= nBarras; i++)   /*Aqui é o caso de carga distribuida na barra. Isso converte a carga distribuida em ações nos nós (duas cargas concentradas e dois momentos)*/
          //  {
                /*   if (barras[i].pFin.z != barras[i].pIni.z)
                   if (barras[i].Pilar.Dados.numero == 1 || 
                       barras[i].Pilar.Dados.numero == 14 ||
                       barras[i].Pilar.Dados.numero == 23||
                       barras[i].Pilar.Dados.numero == 15)
                   {
                       barras[i].pFin.Carga[1] = 4;
                   }
                 */
                // if (barras[i].barraPilar)
                //     barras[i].pFin.Carga[2] = -2.5;
                //     if (barras[i].barraPilar)
                //      barras[i].pFin.Carga[1] = -3;
                //   if (barras[i].barraPilar)
                //        if (barras[i].Pilar.Dados.numero == 1 || barras[i].Pilar.Dados.numero == 4)
                //       barras[i].pFin.Carga[1] = 3;
                ////
                // if (barras[i].barraViga)
                //   barras[i].CargaDistribuida = -1;
          //  }

            //   if (!Manual)
            //     CalculaCarregamentoBarrasVigas();

            HistoricoCalculo("      > Gerando os carregamentos...");
            Application.DoEvents();

            for (i = 1; i <= nBarras; i++)
            {
                for (int j = 0; j < casos.Count; j++)
                    barras[i].casos_x_forcasLocais.Add(new ForcasLocais_Barra(j));
                
                for (int j = 0; j < combinacoes.Count; j++)               
                    barras[i].combinacoes_x_forcasLocais.Add(new ForcasLocais_Barra(j));
            }

            TCarregamentoPortico geracaoCarregamento;
            geracaoCarregamento = new TCarregamentoPortico(gerenciador.formDesenho.Estrutura);
            Application.DoEvents();

            casos_x_forcas = new List<Forcas_Portico>();
            for (int i = 0; i < casos.Count; i++)
            {
                forcasBarras_Casos = new double[NLinhas + 1];

                geracaoCarregamento.GerarCargasLineares_Casos(ref forcasBarras_Casos, i,casos[i], ref glRestrito, ref id);
                geracaoCarregamento.GerarCargasNos_Casos(ref forcasBarras_Casos, i, casos[i], ref glRestrito, ref id);

                casos_x_forcas.Add(new Forcas_Portico(forcasBarras_Casos.ToArray(), casos[i].ID));
            }
            Application.DoEvents();
            combinacoes_x_forcas = new List<Forcas_Portico>();
            for (int i = 0; i < combinacoes.Count; i++)
            {
                forcasBarras_Combinacoes = new double[NLinhas + 1];

                geracaoCarregamento.GerarCargasLineares_Combinacoes(ref forcasBarras_Combinacoes, i, combinacoes[i], ref glRestrito, ref id);
                geracaoCarregamento.GerarCargasNos_Combinacoes(ref forcasBarras_Combinacoes,i, combinacoes[i], ref glRestrito, ref id);

                combinacoes_x_forcas.Add(new Forcas_Portico(forcasBarras_Combinacoes.ToArray(), combinacoes[i].Id));
            }

            gerenciador.formDesenho.Estrutura.cargaPontual.RemoveAll(o=>o.apagarAposCalculo);
            Application.DoEvents();

            // for (i = 1; i <= nBarras; i++)
            // {
            //   barras[i].PreencheAcoesEngPerf(ref forcasBarras_Casos);
            //  jr = id[j];

            //    if (!glRestrito[j])
            //    }

            int jr, gl;
            /* for (i = 1; i <= nNos; i++)
             {
                 numero = nos[i].Numero;
                 for (j = 1; j <= Ndj; j++) 
                 {
                     gl = (numero - 1) * Ndj + j;

                     if (!glRestrito[gl])
                     {
                         jr = id[gl];
                         forcasNos[jr] = nos[i].Carga[j];
                     }
                 }
             }
             */
          
        /*    for (int i = 0; i < casos_x_forcas.Count; i++)
            {
                for (i = 1; i <= nNos; i++)
                {
                    numero = nos[i].Numero;
                    for (j = 1; j <= Ndj; j++)  
                    {
                        gl = (numero - 1) * Ndj + j;

                        if (!glRestrito[gl])
                        {
                            jr = id[gl];
                            forcasNos[jr] = nos[i].Carga[j];
                        }
                    }
                }
            }*/
            // forcasBarras_Casos[12] = -0.65;
            // forcasBarras_Casos[18] = -0.65;
            // forcasBarras_Casos[24] = -0.65;

         /*   for (int i = 0; i < combinacoes_x_forcas.Count; i++)
            {
                for (j = 1; j <= Ngl; j++)
                {
                    jr = id[j];

                    if (!glRestrito[j])
                        forcas[jr] = forcasNos[jr] + combinacoes_x_forcas[i].forcasBarras[jr]; //forcasBarras_Casos[j];

                    //   if (!glRestrito[j])
                    //        ac[j] = aj[j] + ae[j];
                    // else
                    //   ac[j] = 0;
                }
            }*/
            /*    for (i = 1; i <= nNos; i++)
                {
                  //  for (j = 1; j <= 3; j++)
                        if (! glRestrito[nos[i].GlGlobal[3]])
                      //    if (nos[i].GlGlobal[j])
                           ac[nos[i].GlGlobal[3]] = -0.00004;
                }*/

            HistoricoCalculo("      - Geração dos carregamentos - [Ok]", true);

            return true;
        }
        bool RefazerMalha, Testes;
        void Inicializar()
        {
            Testes = false;
            calculoOk = false;
            Progresso.Visible = true;
           // gerenciador.processo.LabelProcesso.Visible = true;
            gerenciador.processo.PanelCalculo.Update();
            //     Progresso.Visible = true;
            
            Progresso.Value = 0;
            Progresso.Maximum = 20 + nNos + (gerenciador.formDesenho.Estrutura.combinacoes.Count*3) + (gerenciador.formDesenho.CasosCarga.Count*3);
        }
        public void Atualiza(int inc)
        {
            Progresso.Increment(inc);
        
         //   Application.DoEvents();
        }

        void AtualizaNos()
        {
            for (int i = 1; i <= nNos; i++)
                if ((Object)nos[i] != null)
                    nos[i].Numero = i;
        }

        public bool CalculouEsforcos;
        public bool CalculaNLG(bool UsarDll)
        {
            try
            {
                //   gerenciador.ConfiguracoesPGi.CfgProjeto.sistema.Solver_cholesky = false;
                //  gerenciador.ConfiguracoesPGi.CfgProjeto.sistema.Solver_cg = true;

                Inicializar();
                // return true;
                // Testes = true;


                //           if (!VerificaSeHaConfiguracaoGrelha())
                //              throw new TErroConcepcaoEstrutural(this, "Erro: Pavimento " + this.Descricao + " não possui grelha configurada. Cálculo interrompido.");

                //     HistoricoCalculo("Pórtico Espacial");


                Atualiza(1);

                RefazerMalha = true;

                //   if (!Manual)
                while (RefazerMalha)
                    if (!GerarMalha())
                        throw new TErroPortico(this, "   >ERRO: MALHA NÃO PODE SER CRIADA.");

                if (Manual)
                {
                    Finaliza();
                }

                Atualiza(1);

                if (nBarras < 1 || nNos < 1)
                    throw new TErroPavimento(this, "   >ERRO: NÃO FOI CRIADA NENHUMA BARRA DE PÓRTICO ESPACIAL.");

                MensagemPortico();

                if (!Testes)
                    if (!ReordenarNos())
                        throw new TErroPavimento(this, "   >ERRO: PROBLEMA NA REORDENAÇÃO DOS NÓS");

                if (!SetMatrizesBarras())
                    throw new TErroPavimento(this, "   >ERRO: MATRIZES LOCAIS NÃO PUDERAM SER CRIADAS");

                DadosEstruturais();

                CalculouEsforcos = true;

                CriarMatrizSFF(UsarDll);
                SetCoeficientesMola();

                //   calculoOk = true;
                // return true;
                if (!MatrizDeRigidez(UsarDll))
                    throw new TErroPavimento(this, "   >ERRO: ERRO NA MONTAGEM DO SISTEMA DE EQUAÇÕES.");

                //  gerenciador.ShowMatriz(-1, true,false); 
                Atualiza(1);

                calculoOk = ResolveEquacoes(UsarDll);

                //  if (!MatrizRigidez.VerificarPrecisao(ref ac, ref df))
                //     throw new TErroPavimento(this, "Pórtico espacial - Erro de precisão numérica.");

                if (!Resultados())
                    return false;

                // HistoricoCalculo((PesoTotal * 100 / 1000).ToString("n2"));

                //   calculoOk = true;
            }
            catch
            {

            }
            //  MatrizRigidez = null;
            glRestrito = null;
            id = null;
            Linhas = null;
            forcas = null;
            df = null;
            return calculoOk;
        }

        public bool Calcular(bool calculaEsforco, bool UsarDll)
        {
            try
            {
                Inicializar();

                RefazerMalha = true;

                Atualiza(1);

                while (RefazerMalha)
                    if (!GerarMalha())
                        throw new TErroPortico(this, "   >ERRO: MALHA NÃO PODE SER CRIADA.");

                if (Manual)
                {
                    Finaliza();
                }

                Atualiza(1);

                if (nBarras < 1 || nNos < 1)
                    throw new TErroPavimento(this, "   >ERRO: NÃO FOI CRIADA NENHUMA BARRA DE PÓRTICO ESPACIAL.");

                if (!Testes)
                  if (!ReordenarNos())
                     throw new TErroPavimento(this, "   >ERRO: PROBLEMA NA REORDENAÇÃO DOS NÓS");

                if (!SetMatrizesBarras())
                    throw new TErroPavimento(this, "   >ERRO: MATRIZES LOCAIS NÃO PUDERAM SER CRIADAS");

                DadosEstruturais();

                CalculouEsforcos = true;
                if (!calculaEsforco)
                {
                    CalculouEsforcos = false;
                    calculoOk = true;
                    return calculoOk;
                }

                CriarMatrizSFF(UsarDll);
                SetCoeficientesMola();

                if (!MatrizDeRigidez(UsarDll))
                    throw new TErroPavimento(this, "   >ERRO: ERRO NA MONTAGEM DO SISTEMA DE EQUAÇÕES.");

                Atualiza(1);

                calculoOk = ResolveEquacoes(UsarDll);

                if (calculoOk)
                {
                    Atualiza(1);

                    if (!Resultados())
                        return false;
                }
                Atualiza(1);
                Progresso.Value = 0;
            }
            catch
            {

            }
          //  MatrizRigidez = null;
            glRestrito = null;
            id = null;
            Linhas = null;
            forcas = null;
            df = null;
            return calculoOk;
        }

        public Thread t;
        System.Threading.Timer timerCalculo;

        public void CalcularTensoes()
        {
            HistoricoCalculo("      > Calculando tensões...");
            Application.DoEvents();

          //  Atualiza(1);

            for (int i = 0; i < gerenciador.formDesenho.CasosCarga.Count; i++)
            {
                Atualiza(1);
                for (int j = 1; j <= nBarras; j++)
                    barras[j].CalcularTensoes(Const.ID_TIPO_CASO, i);
            }

            Application.DoEvents();

            for (int i = 0; i < gerenciador.formDesenho.Estrutura.combinacoes.Count; i++)
            {
                Atualiza(1);
                for (int j = 1; j <= nBarras; j++)
                    barras[j].CalcularTensoes(Const.ID_TIPO_COMBINACAO, i);
            }

            Atualiza(1);

            HistoricoCalculo("      - Cálculo de tensões [Ok]", true);
        }

        public void MostraMatrizRigidez()
        {

        }

        bool ResolveEquacoes(bool UsarDll)
        {
            TCasosCarga caso;
            TCombinacoes combinacao;
            try
            {
                // if (gerenciador.ConfiguracoesPGi.CfgProjeto.sistema.Solver_cholesky)
                Atualiza(1);

                if (!MatrizRigidez.FatoraMatrizBanda(this.Descricao, "Pórtico - Resolvendo matriz...", UsarDll))
                    throw new TErroPavimento(this, "Pórtico - Erro ao fatorar a matriz de rigidez!");

                if (!Carregamentos(gerenciador.formDesenho.Estrutura.combinacoes, gerenciador.formDesenho.CasosCarga))
                    return false;

                span1 = DateTime.Now;

                HistoricoCalculo("      > Resolvendo o sistema de equações...");
                Application.DoEvents();

                casos_x_deslocamentos = new List<Deslocamentos_Portico>();
                combinacoes_x_deslocamentos = new List<Deslocamentos_Portico>();

                for (int i = 0; i < casos_x_forcas.Count; i++)
                {
                    Atualiza(1);
                    caso = gerenciador.formDesenho.CasosCarga.Find(o => o.ID == casos_x_forcas[i].id);

                    //  MsgCalculo(this.Descricao, "Resolvendo para carregamento ", 0, false, false);

                  //  if (gerenciador.ConfiguracoesPGi.CfgProjeto.sistema.Solver_cholesky_supernodal)
                  //      HistoricoCalculo("      > Resolvendo matriz (Cholesky)...");
                  //  else
                 //   if (gerenciador.ConfiguracoesPGi.CfgProjeto.sistema.Solver_gradiente_conjugado)
                 //       HistoricoCalculo("      > Resolvendo matriz (GC Iterativo)...");

                    double[] forcas_caso = casos_x_forcas[i].forcasBarras;

                    int resultado = MatrizRigidez.ResolveMatrizBanda(this, ref forcas_caso/*ref forcas*/,
                        ref df,
                        this.Descricao, "Pórtico - Resolvendo matriz - Caso: " + caso.Nome,
                        gerenciador.ConfiguracoesPGi.CfgProjeto.sistema.Solver_cholesky_supernodal, 
                        gerenciador.ConfiguracoesPGi.CfgProjeto.sistema.Solver_gradiente_conjugado,
                        i==0);

                    if (resultado == -6)
                    {
                        HistoricoCalculo("CÁLCULO INTERROMPIDO");
                    }
                    else
                    {
                        span2 = DateTime.Now;

                        casos_x_deslocamentos.Add(new Deslocamentos_Portico(df.ToArray(), caso.ID));
                    }

                    if (resultado == 1)
                        throw new TFaltaMemoria(this, "Memória insuficiente. Vá em 'Configurações de análise->Solver' e marque a opção 'iterativo (gradiente conjugado)' e calcule novamente.");
                    if (resultado == 2)
                        throw new System.Exception();

                      /* gerenciador.processo.ListaCalculo.Items[gerenciador.processo.ListaCalculo.Items.Count - 1] =
                         "      > Resolução das equações lineares - [Ok] - " + 
                          span2.Subtract(span1).ToString("mm") + ":" +
                          span2.Subtract(span1).ToString("ss");*/
                }

                for (int i = 0; i < combinacoes_x_forcas.Count; i++)
                {
                    Atualiza(1);
                    combinacao = gerenciador.formDesenho.Estrutura.combinacoes.Find(o => o.Id == combinacoes_x_forcas[i].id);

                    double[] forcas_combinacao = combinacoes_x_forcas[i].forcasBarras;

                    int resultado = MatrizRigidez.ResolveMatrizBanda(this, ref forcas_combinacao/*ref forcas*/,
                        ref df,
                        this.Descricao, "Pórtico - Resolvendo matriz",
                        gerenciador.ConfiguracoesPGi.CfgProjeto.sistema.Solver_cholesky_supernodal,
                        gerenciador.ConfiguracoesPGi.CfgProjeto.sistema.Solver_gradiente_conjugado,
                        false);

                    if (resultado == -6)
                    {
                        HistoricoCalculo("CÁLCULO INTERROMPIDO");
                    }
                    else
                    {
                        span2 = DateTime.Now;

                        combinacoes_x_deslocamentos.Add(new Deslocamentos_Portico(df.ToArray(), combinacao.Id));
                    }

                    if (resultado == 1)
                        throw new TFaltaMemoria(this, "Memória insuficiente. Vá em 'Configurações de análise->Solver' e marque a opção 'iterativo (gradiente conjugado)' e calcule novamente.");
                    if (resultado == 2)
                        throw new System.Exception();


                }

                HistoricoCalculo("      - Resolução das equações lineares - [Ok] - " +
                                                span2.Subtract(span1).ToString("mm") + ":" +
                                                span2.Subtract(span1).ToString("ss"), true);
                MatrizRigidez.s = null;
                return true;
                
            }
            catch(Exception ex)
            {
                HistoricoCalculo("ERRO: " + ex.Message);
                return false;
            }
        }

    }
}