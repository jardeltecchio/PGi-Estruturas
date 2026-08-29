using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace PG
{
    public partial class TPavimento: TModeloEstrutural
    {
        [NonSerialized] public List<TLaje> lajes;
        [NonSerialized] public List<TTrechoViga> vigas;
        [NonSerialized] public List<TPilar> pilares;
        [NonSerialized] public TLinha[] linhas;
        [NonSerialized] public TPonto[] pontos;

        [NonSerialized]
        public List<TLayer> layers;
        [NonSerialized]
        public Dictionary<string, TLayer> LayersByIdPrincipal, LayersByIdArquitetura;

        [NonSerialized] public Gerenciador gerenciador;
        [NonSerialized] public TBarraGrelha[] barras;
        [NonSerialized] public TNoGrelha[] nos;
        public SConfiguracaoGrelha ConfiguracaGrelha;

        #region Variáveis
        public SClasses classeViga, classePilar, classeLaje;
        public List<TCargaPontual> CargasAplicadas;

        public int nBarras, nNos;

        int i, j, k;
        public int LarguraBanda;

        int linhasCount, pontosCount = 0;
        
        public double PeDireito, Nivel, PP;
      
        public double espacamentoX, espacamentoY, area;

        public double xi, xf, yi, yf;

        public int Numero;

        int Ndj = 3;  //número de vínculos possíveis em um nó
        
        public int  Ngl,
                    NglBarra,
                    NLinhas,
                    NumeroDeRestricoes,
                    NumeroDeNosComRestricoes,
                    SequenciaPavimento;

        public int NumeroRepeticoes;
        [NonSerialized]
        double[] df;
        [NonSerialized]
        double[] ac;
        [NonSerialized] public int[] id;
        [NonSerialized]
        public int[] Linhas; //vetor que retorna o número da linha/coluna da matriz em que o gl se encontra
        [NonSerialized]
        double[] K_Mola;
        [NonSerialized] bool[] glRestrito;

        /*Configuracoes da tela para carregar corretamente o pavimento na viewport*/
        public double precisaoPixel;
        public double[] ponto_zero;
        public double fatorZoom; 

        [NonSerialized] 
        public TMatrizBanda MatrizRigidez;
        
        public bool   calculoOk;
        #endregion
        public TPavimento() : base() { }

        public TPavimento(ref Gerenciador gerenciador, string descricao, double pedireito, double nivel)
        {
            this.PeDireito        = pedireito;
            this.Descricao        = descricao;
            this.Nivel            = nivel;
            this.gerenciador      = gerenciador;

            this.Tipo = "GRELHA";
            this.CreateStandardLayers();
            this.vigas = new List<TTrechoViga>();
            this.pilares = new List<TPilar>();
            this.lajes = new List<TLaje>();
            this.CargasAplicadas = new List<TCargaPontual>();

//            this.espacamentoX = (double)malhaX;
       //     this.espacamentoY = (double)malhaY;

            this.ConfiguracaGrelha = new SConfiguracaoGrelha(true);
        }

        TLayer RetLayerConfigurado(int indice)
        {
            return gerenciador.ConfiguracoesPGi.layers[indice];
        }

        int cl;

        public void CreateStandardLayers()
        {
            this.LayersByIdPrincipal   = new Dictionary<string, TLayer>();
            this.LayersByIdArquitetura = new Dictionary<string, TLayer>();
            this.layers                = new List<TLayer>();
             
            cl = -1;
            CriaLayer(Lay.Zero);
            CriaLayer(Lay.ElementosBasicos);
            CriaLayer(Lay.Vigas);
            CriaLayer(Lay.Pilares);
            CriaLayer(Lay.Lajes);
            CriaLayer(Lay.GrelhaLajes);
            CriaLayer(Lay.TextosVigas);
            CriaLayer(Lay.TextosLajes);
            CriaLayer(Lay.TextosPilares);


            CriaLayer(Lay.CargaPontual);
            CriaLayer(Lay.CargaLinear);
        }

        void CriaLayer(string Nome)
        {
            cl++;
            layers.Add(new TLayer(Nome, 
                                 RetLayerConfigurado(cl).Ligado,
                                 RetLayerConfigurado(cl).Congelado, 
                                 new byte[3] { RetLayerConfigurado(cl).Rgb[0], RetLayerConfigurado(cl).Rgb[1], RetLayerConfigurado(cl).Rgb[2] }, 0, 
                                 RetLayerConfigurado(cl).Travado, 
                                 GrupoLay.Principal));

            LayersByIdPrincipal[Nome] = layers[cl]; 
        }

        public int GetCountElementosSlecionados()
        {
            kk = 0;
            /*foreach (TLayer lay in layers)
                foreach (TObjetoDesenho obj in lay.Obj)
                    if (obj != null)
                        if (obj.layer != null)
                            if (obj.Selecionado && obj.layer.Grupo == GrupoLay.Principal)
                                if (obj.ObjetoPrimario)
                                    kk++;*/
            return kk;
        }

        public void GetCountLinhas_e_Pontos()
        {
            linhasCount = 0;
            pontosCount = 0;

            foreach (TLinha ob in linhas)
                linhasCount++;

            foreach (TPonto ob in pontos)
                pontosCount++;

            double xIni = 99999999999;
            double yIni = 99999999999;

            double xFin = -99999999999;
            double yFin = -99999999999;

            for (int i = 0; i < pontosCount; i++)
            {
              //  if (!pontos[i].PontoEixoViga) continue;
                if (pontos[i].PontoAuxiliar) continue;

                if (pontos[i].x < xIni) 
                    xIni = pontos[i].x;

                if (pontos[i].y < yIni)                    
                    yIni = pontos[i].y;

                if (pontos[i].x > xFin)
                    xFin = pontos[i].x;

                if (pontos[i].y > yFin)
                    yFin = pontos[i].y;

            };

            xi = xIni;
            xf = xFin;
            yi = yIni;
            yf = yFin;
        }

        public TPavimento(ref Gerenciador gerenciador, string descricao, List<TLaje> _lajes, List<TTrechoViga> _vigas, List<TPilar> _pilares, TLinha[] _linhas, TPonto[] _pontos, double malhaX, double malhaY)
        {
            Numero++;
            this.espacamentoX = (double)malhaX;
            this.espacamentoY = (double)malhaY;
            this.Tipo = "GRELHA";

            this.Descricao   = descricao;
            this.gerenciador = gerenciador;

            this.lajes = _lajes;
            this.pilares = _pilares;
            this.vigas   = _vigas;
            this.pontos  = _pontos;
            this.linhas  = _linhas;

            GetCountLinhas_e_Pontos();

            this.nNos    = 0;
            this.nBarras = 0;

            calculoOk   = false;
        }

        [NonSerialized]
        public List<BarrasIsovalor> BarrasIsoDeslocamento, BarrasIsoFletor, BarrasIsoTorcor, BarrasIsoCortante;
        [NonSerialized]
        public List<LinhasIsovalor> LinhasIsoDeslocamento, LinhasIsoFletor, LinhasIsoTorcor, LinhasIsoCortante;
        [NonSerialized]
        public double[] ValoresIsoDeslocamentos;
        [NonSerialized]
        public double[] ValoresIsoFletores;

        public struct BarrasxCoord
        {
            public int numBarra;
            public vec3 CoordIsovalor;
            public BarrasxCoord(int n )
            {
                CoordIsovalor = new vec3(0, 0, 0);
                numBarra = n;
            }
        }

        public struct BarrasIsovalor
        {
            public List<TBarraGrelha> barras;
            public List<BarrasxCoord> barrasxCoord;

            public double valor;
            public BarrasIsovalor(double val)
            {
                barras       = new List<TBarraGrelha>();
                barrasxCoord = new List<BarrasxCoord>();
                valor = val;
            }
        }
        
        public void GeraIsovaloresDeslocamentos()
        {
            LinhasIsoDeslocamento = new List<LinhasIsovalor>();

            BarrasIsoDeslocamento = new List<BarrasIsovalor>();

            AnyPt p1;
            double a, b, x, y, isoValor, dIni, dFin;
            vec3 centroRotacao   = new vec3(0, 0, 0);

            /*
             y = ax + b
             
             b = desloc. esquerda
             a = ((b *-1)  + desloc.direita) / comp.barra
             
             x = (b*-1 + y ) / a   -> posição x em que o deslocamento está na barra deitada, sem rotação
             */
            for (int i = 0; i < 9; i++)
            {
                BarrasIsoDeslocamento.Add(new BarrasIsovalor(ValoresIsoDeslocamentos[i])); // uma lista de linhas para cada valor 
                LinhasIsoDeslocamento.Add(new LinhasIsovalor(ValoresIsoDeslocamentos[i])); // uma lista de linhas para cada valor 
            }
            vec3 coordIsoDeslocamento = new vec3(0,0,0);
            TBarraGrelha barra;
            for (int d = 0; d < 9; d++)
            {
                isoValor = ValoresIsoDeslocamentos[d];

                /*grava numa lista as barras que contém cada isovalor, tambem grava a posicao do isovalor na barra*/
                for (int i = 1; i < barras.Count(); i++)
                {
                   // if (barras[i].barraViga) continue;
                    barra = barras[i];
                    dIni = barra.pIni.Deslocamento[3];
                    dFin = barra.pFin.Deslocamento[3];

                    if ((dIni >= isoValor && dFin <= isoValor) || (dFin >= isoValor && dIni <= isoValor))
                    {
                    //    if (barras[i].codBarra == 304)
                      //      barras[i].codBarra = 304;

                        b = 0;
                        a = 0;

                        BarrasIsoDeslocamento[d].barras.Add(barra);

                    //    if (Geom.Iguais(barra.pIni.x, barra.pFin.x,0.001))
                        //{
                            b = dIni;
                            a = (-b + dFin) / barra.comprimento;
                      //  }
                    /*    else
                        if (barra.pIni.x < barra.pFin.x)
                        {
                            b = dIni;
                            a = (-b + dFin) / barra.comprimento;
                        }
                        else
                        if (barra.pIni.x > barra.pFin.x)
                        {
                            b = dFin;
                            a = (-b + dIni) / barra.comprimento;
                        }*/

                        y = isoValor;
                        x = ((-b) + y) / a;

                        p1 = new AnyPt(barra.pIni.x + x, barra.pIni.y, false);

                        centroRotacao = new vec3(barra.pIni.x, barra.pIni.y, 0);
                        //aqui no 'x' é o pfin, que é o ponto onde tem o isovalor
                        //faço a coordenada do deslocamento sem rotacao, com y = 0  (barra deitada)    --->    pini O-----x-----------------O //pfinal da barra, mas esse nao interessa, só o ponto do isovalor
                        coordIsoDeslocamento = new vec3(p1.x, p1.y, 0);

                        //rotaciono a coordenada no angulo da barra
                        coordIsoDeslocamento = coordIsoDeslocamento.Rotate(centroRotacao, barra.anguloGlobal * Const.PIDiv180);

                        BarrasIsoDeslocamento[d].barrasxCoord.Add(new BarrasxCoord(barra.codBarra));
                        BarrasIsoDeslocamento[d].barrasxCoord[BarrasIsoDeslocamento[d].barrasxCoord.Count - 1].CoordIsovalor.x = coordIsoDeslocamento.x;
                        BarrasIsoDeslocamento[d].barrasxCoord[BarrasIsoDeslocamento[d].barrasxCoord.Count - 1].CoordIsovalor.y = coordIsoDeslocamento.y;

                    }
                }
            }

            /*Cria as linhas de isovalor*/
            TBarraGrelha bar1, bar2;
            Linha linha;
            int numBarra1, numBarra2;
     
            for (int i = 0; i < BarrasIsoDeslocamento.Count; i++)
            {
                for (int j = 0; j < BarrasIsoDeslocamento[i].barras.Count; j++)
               {

                  bar1 = BarrasIsoDeslocamento[i].barras[j];

                  for (int k = 0; k < BarrasIsoDeslocamento[i].barras.Count; k++)
                  {
                      bar2 = BarrasIsoDeslocamento[i].barras[k];

                      if ((Object)bar1 != (Object)bar2)
                      {
                          if (BarraConectada(bar1, bar2)) // faz uma verificação meia-boca da conectividade ds nós...tenho q pensar melhor nessa solução
                          {
                              numBarra1 = RetNumeroBarra(BarrasIsoDeslocamento[i],bar1.codBarra); 
                              numBarra2 = RetNumeroBarra(BarrasIsoDeslocamento[i],bar2.codBarra);

                              linha = new Linha(new AnyPt(BarrasIsoDeslocamento[i].barrasxCoord[numBarra1].CoordIsovalor.x, BarrasIsoDeslocamento[i].barrasxCoord[numBarra1].CoordIsovalor.y, false),
                                                new AnyPt(BarrasIsoDeslocamento[i].barrasxCoord[numBarra2].CoordIsovalor.x, BarrasIsoDeslocamento[i].barrasxCoord[numBarra2].CoordIsovalor.y, false));
                              
                              if (!CortaMaisDeDuasBarras(linha, bar1, bar2))
                                if (!LocLinhaIsovalor(linha, LinhasIsoDeslocamento[i].linhas))
                                    LinhasIsoDeslocamento[i].linhas.Add(linha);
                          }
                      }

                  }
               }
            }
        }
        public void GeraIsovaloresFletor()
        {
            LinhasIsoFletor = new List<LinhasIsovalor>();

            BarrasIsoFletor = new List<BarrasIsovalor>();

            AnyPt p1;
            double a, b, x, y, isoValor, dIni, dFin;
            vec3 centroRotacao = new vec3(0, 0, 0);

            /*
             y = ax + b
             
             b = desloc. esquerda
             a = ((b *-1)  + desloc.direita) / comp.barra
             
             x = (b*-1 + y ) / a   -> posição x em que o deslocamento está na barra deitada, sem rotação
             */

            for (int i = 0; i < 9; i++)
            {
                BarrasIsoFletor.Add(new BarrasIsovalor(ValoresIsoFletores[i])); // uma lista de linhas para cada valor 
                LinhasIsoFletor.Add(new LinhasIsovalor(ValoresIsoFletores[i])); // uma lista de linhas para cada valor 
            }
            vec3 coordIso = new vec3(0, 0, 0);
            TBarraGrelha barra;
            for (int d = 0; d < 9; d++)
            {
                isoValor = ValoresIsoFletores[d];

                /*grava numa lista as barras que contém cada isovalor, tambem grava a posicao do isovalor na barra nessa mesma lista*/
                for (int i = 1; i < barras.Count(); i++)
                {
                    if (barras[i].direcaoY) continue;
                    if (barras[i].barraRigida) continue;
                    if (barras[i].barraViga) continue;


                    barra = barras[i];
                    dIni = Math.Abs(barra.Esforcos[2]);
                    dFin = Math.Abs(barra.Esforcos[5]);

                    if ((dIni >= isoValor && dFin <= isoValor) || (dFin >= isoValor && dIni <= isoValor))
                    {
                        //    if (barras[i].codBarra == 77)
                        //       barras[i].codBarra = 77;

                        b = 0;
                        a = 0;

                        BarrasIsoFletor[d].barras.Add(barra);

                     //   if (Geom.Iguais(barra.pIni.x, barra.pFin.x, 0.001))
                     //   {
                            b = dIni;
                            a = (-b + dFin) / barra.comprimento;
                     /*   }
                        else
                        if (barra.pIni.x < barra.pFin.x)
                        {
                            b = dIni;
                            a = (-b + dFin) / barra.comprimento;
                        }
                        else
                        if (barra.pIni.x > barra.pFin.x)
                        {
                            b = dFin;
                            a = (-b + dIni) / barra.comprimento;
                        }*/

                        y = isoValor;
                        x = ((-b) + y) / a;

                        p1 = new AnyPt(barra.pIni.x + x, barra.pIni.y, false);

                        centroRotacao = new vec3(barra.pIni.x, barra.pIni.y, 0);
                        //aqui no 'x' é o pfin, que é o ponto onde tem o isovalor
                        //faço a coordenada do deslocamento sem rotacao, com y = 0  (barra deitada)    --->    pini O-----x-----------------O //pfinal da barra, mas esse nao interessa, só o ponto do isovalor
                        coordIso = new vec3(p1.x, p1.y, 0);

                        //rotaciono a coordenada no angulo da barra
                        coordIso = coordIso.Rotate(centroRotacao, barra.anguloGlobal * Const.PIDiv180);

                        BarrasIsoFletor[d].barrasxCoord.Add(new BarrasxCoord(barra.codBarra));
                        BarrasIsoFletor[d].barrasxCoord[BarrasIsoFletor[d].barrasxCoord.Count - 1].CoordIsovalor.x = coordIso.x;
                        BarrasIsoFletor[d].barrasxCoord[BarrasIsoFletor[d].barrasxCoord.Count - 1].CoordIsovalor.y = coordIso.y;

                    }
                }
            }

            /*Cria as linhas de isovalor*/
            TBarraGrelha bar1, bar2;
            Linha linha;
            int numBarra1, numBarra2;
            for (int i = 0; i < BarrasIsoFletor.Count; i++)
            {
                for (int j = 0; j < BarrasIsoFletor[i].barras.Count; j++)
                {
                    bar1 = BarrasIsoFletor[i].barras[j];

                    for (int k = 0; k < BarrasIsoFletor[i].barras.Count; k++)
                    {
                        bar2 = BarrasIsoFletor[i].barras[k];

                        if ((Object)bar1 != (Object)bar2)
                        {
                            if (BarraConectada(bar1, bar2)) // faz uma verificação meia-boca da conectividade ds nós...tenho q pensar melhor nessa solução
                            {
                                numBarra1 = RetNumeroBarra(BarrasIsoFletor[i], bar1.codBarra);
                                numBarra2 = RetNumeroBarra(BarrasIsoFletor[i], bar2.codBarra);

                                linha = new Linha(new AnyPt(BarrasIsoFletor[i].barrasxCoord[numBarra1].CoordIsovalor.x, BarrasIsoFletor[i].barrasxCoord[numBarra1].CoordIsovalor.y, false),
                                                  new AnyPt(BarrasIsoFletor[i].barrasxCoord[numBarra2].CoordIsovalor.x, BarrasIsoFletor[i].barrasxCoord[numBarra2].CoordIsovalor.y, false));

                                if (!CortaMaisDeDuasBarras(linha, bar1, bar2))
                                    if (!LocLinhaIsovalor(linha, LinhasIsoFletor[i].linhas))
                                        LinhasIsoFletor[i].linhas.Add(linha);
                            }
                        }

                    }
                }
            }
        }
  
        int RetNumeroBarra(BarrasIsovalor barraIsoValor, int num)
        {
            for (int j = 0; j < barraIsoValor.barrasxCoord.Count; j++)
            {
                if (barraIsoValor.barrasxCoord[j].numBarra == num)
                    return j;
            }
            return 0;
        }
        double ix__ = 0;
        double iy__ = 0;
        bool CortaMaisDeDuasBarras(Linha lin, TBarraGrelha b1, TBarraGrelha b2)
        {
            for (k = 1; k < barras.Count(); k++)
                if (((Object)barras[k] != (Object)b1) && ((Object)barras[k] != (Object)b2))
                    if (lin.Intersec3(barras[k].pIni.x, barras[k].pIni.y, barras[k].pFin.x, barras[k].pFin.y, ref  ix__, ref iy__))
                        return true;

            return false;
        }

        bool BarraConectada(TBarraGrelha b1, TBarraGrelha b2)
        {
            if ((b1.pFin.barrasIncidentes.Contains(b2)) || (b1.pIni.barrasIncidentes.Contains(b2))) 
                return true;
           
            foreach (TBarraGrelha a in b1.pFin.barrasIncidentes)
            {
                if ((Object)a != (Object)b1)
                {
                    if ((Object)a.pIni == (Object)b1.pFin)
                    {
                        foreach (TBarraGrelha b in a.pFin.barrasIncidentes)
                        {
                            if ((Object)b == (Object)b2)
                              return true;
                        }
                    }
                    else
                    if ((Object)a.pFin == (Object)b1.pFin)
                    {
                        foreach (TBarraGrelha b in a.pIni.barrasIncidentes)
                        {
                            if ((Object)b == (Object)b2)
                                return true;
                        }
                    }
                }

            }

            foreach (TBarraGrelha a in b1.pIni.barrasIncidentes)
            {
                if ((Object)a != (Object)b1)
                {
                    if ((Object)a.pIni == (Object)b1.pIni)
                    {
                        foreach (TBarraGrelha b in a.pFin.barrasIncidentes)
                        {
                            if ((Object)b == (Object)b2)
                                return true;
                        }
                    }
                    else
                    if ((Object)a.pFin == (Object)b1.pIni)
                    {
                        foreach (TBarraGrelha b in a.pIni.barrasIncidentes)
                        {
                            if ((Object)b == (Object)b2)
                                return true;
                        }
                    }
                }

            }

            return false;
        }

        bool LocLinhaIsovalor(Linha lin, List<Linha> linhas)
        {
            foreach (Linha l in linhas)
                if (((Object)l.pFin == (Object)lin.pFin && (Object)l.pIni == (Object)lin.pIni) ||
                    ((Object)l.pIni == (Object)lin.pFin && (Object)l.pFin == (Object)lin.pIni))
                    return true;
          
            return false;
        }

        public override void MsgCalculo(string titulo, string texto, int max, bool fim = false, bool MostraProgresso = true)
        {
          //  gerenciador.BringToFront();
         //   gerenciador.PanelCalculo.Visible = true;
          //  gerenciador.lbProgresso.Text = texto;
            //gerenciador.panel2.Visible = true;
            gerenciador.processo.PanelCalculo.Update();
            gerenciador.processo.LabelProcesso.Text = texto;
            /*   Progresso.Visible = MostraProgresso;
            Progresso.Value = 0;
            Progresso.Maximum = System.Convert.ToInt32(max);
            gerenciador.lbProgresso.Text = texto;

            if (fim)
              Progresso.Visible = false;
          //  gerenciador.SendToBack();
          //  gerenciador.BringToFront();*/
        //    Application.DoEvents();
            gerenciador.Update();
         //   gerenciador.BringToFront();

        }

        public override void HistoricoCalculo(string texto, bool Edit = false, bool erro =false)
        {
          /*  if (Edit)
              gerenciador.ListaCalculo.Items[gerenciador.ListaCalculo.Items.Count - 1] = texto;
            else
              gerenciador.ListaCalculo.Items.Insert(gerenciador.ListaCalculo.Items.Count, texto);

            gerenciador.ListaCalculo.SelectedIndex = gerenciador.ListaCalculo.Items.Count - 1;*/

      //      Progresso.Value = 0;
        }

        public double MaximoEsforco(int indice)
        {
            double max = 0;

            for (i = 1; i <= nBarras; i++)
            {
                if (Math.Abs(barras[i].Esforcos[indice]) > max)
                    max = barras[i].Esforcos[indice]; //mom fletor no nó ini

                if (Math.Abs(barras[i].Esforcos[indice+3]) > max)
                    max = barras[i].Esforcos[indice+3];  //mom fletor no nó fin
            }

            return max;
        }

        public void LocNo()
        {


        }

        public double MaximoDeslocamento(int Deslocamento)
        {
            double max = 0;
            
            for (i = 1; i <= nNos; i++)
                if (Math.Abs(nos[i].Deslocamento[Deslocamento]) > max)
                    max = Math.Abs(nos[i].Deslocamento[Deslocamento]);

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
            Ndj      = 3;
            Ngl      = nNos * Ndj;
            NglBarra = Ndj * 2;
            NLinhas  = Ngl - NumeroDeRestricoes;

            this.glRestrito = new bool[Ngl + 1];
            this.id         = new int[Ngl + 1];
            this.Linhas     = new int[Ngl + 1];
            this.ac         = new double[Ngl + 1];
            this.df         = new double[Ngl + 1];
  
            return true;
        }

        private bool CriarMatrizSFF()
        {
            Atualiza(1);
            
            if (MatrizRigidez != null)
              MatrizRigidez = null;
            
         //   if (LarguraBanda >= .5 * NLinhas)
        //        return false;

            MatrizRigidez = new TMatrizBanda(NLinhas, LarguraBanda, Ngl, this, gerenciador.ConfiguracoesPGi.CfgProjeto.sistema.Solver_cholesky_supernodal);

            return true;
        }
        
        private bool DadosEstruturais()
        {
            Atualiza(1);
            
            NumeroDeRestricoes       = GetNumeroDeRestricoes();
            NumeroDeNosComRestricoes = GetNumeroDeNosComRestricoes();

            if (!CriarVetores())
              throw new TErroPavimento(this, "Pavimento: " + Descricao + "  -  Erro dados estruturais.");
           
            int numero;

            //preenche o vetor glRestrito que diz para cada GL se este está restrito ou nao
            for (i = 1; i <= nNos; i++)
            {
                numero = nos[i].Numero;
                for (j = 1; j <= 3; j++)
                  glRestrito[(numero-1) * 3 +j] = nos[i].Restricao[j];
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
            int nbi=0;
            LarguraBanda = 0;

            for (i = 1; i <= nBarras; i++ )
            {
                nbi = 3 * (Math.Abs(barras[i].pFin.Numero - barras[i].pIni.Numero) + 1);
                
                if (nbi > LarguraBanda)
                  LarguraBanda = nbi;
            }

         //   LarguraBanda -= (NumeroDeRestricoes);

            return true;
        }

        private int RetGL(int Coluna)
        {
            for (int i = 1; i <= Ngl; i++)
                if (id[i] == Coluna)
                    return i;

            return -1;
        }

        private bool MatrizDeRigidez()
        {
            Atualiza(1);
            
            /*Preenche SOMENTE o triangulo superior da matriz*/
            
            int ir, ic, i1, i2,lin, col, linAux;

            MsgCalculo("Pavimento: " + this.Descricao, "Pavimento: " + this.Descricao + " - Gerando sistema de equações...", nBarras, false, false);
           #region solver2
            if (gerenciador.ConfiguracoesPGi.CfgProjeto.sistema.Solver_choleskypadrao)
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

                                   lin = ir - 1;
                                    col = (ic - 1) + lin;

                                //    if (lin == 6)// && col < 9)
                                   //     lin = 6;

                                    if (TestesManuais)
                                      MatrizRigidez.Sff_[lin][col]     += barras[i].MatrizGlobal[j, k];

                                    MatrizRigidez.Banda[ir - 1][ic - 1] += barras[i].MatrizGlobal[j, k];

                                    //   MatrizRigidez.Sff[ir * LarguraBanda + ic] += barras[i].MatrizGlobal[j, k];
                                }
                            }
                        }
                    }
                }
                /*Insere coef. de mola*/
                if (!Testes)
                  for (int i = 0; i < NLinhas; i++)
                      if (K_Mola[i + 1] != 0)
                          MatrizRigidez.Banda[i][0] += K_Mola[i + 1];
            }
            #endregion
            else
            #region solver 1
            if (gerenciador.ConfiguracoesPGi.CfgProjeto.sistema.Solver_cholesky_supernodal)
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
                                }
                            }
                        }
                    }
                }

                /*Insere coef. de mola*/
                for (int i = 1; i <= Ngl; i++)
                    if (K_Mola[i] != 0)
                        MatrizRigidez.Sff[i * LarguraBanda + 1] += K_Mola[i];
            }
            #endregion

           
            HistoricoCalculo("   Geração do sistema de equações [" + NLinhas + " x " + LarguraBanda + "]");
            
            return true;
        }
        

        //preenche o vetor molas com os respectivos coeficientes K para os graus de liberdade em ordem. Se nao tem, fica zero
        private bool SetCoeficientesMola()
        {
            Atualiza(1);
            
            K_Mola = new double[Ngl + 1];

            for (i = 1; i <= nNos; i++)
            {
                if (nos[i].PossuiMolaDZ) 
                  K_Mola[nos[i].Numero * 3] = nos[i].K_Mola_DZ;
               
                if (nos[i].PossuiMolaRX) 
                  K_Mola[(nos[i].Numero * 3) - 1] = nos[i].K_Mola_RY;
               
                if (nos[i].PossuiMolaRY) 
                  K_Mola[(nos[i].Numero * 3) - 2] = nos[i].K_Mola_RX;
            }

            return true;
        }

        private bool Resultados()
        {
        //    Application.DoEvents(); 
            MsgCalculo("Pavimento: " + this.Descricao, "Pavimento: " + this.Descricao + " - Resultados finais...", nBarras, false, false);
            
            Atualiza(1);
            
            int glGlobal;

            double[] dj = new double[Ngl +1];

            for (int k = 1; k <= Ngl; k++)
            {
                dj[k] = df[id[k]]; //a linha da matriz referente ao grau de liberdade é id[k]
  
            }
           
            for (j = 1; j <= nNos; j++)
            {
              for (int k = 1; k <= 3; k++)
              {
                 glGlobal = nos[j].GlGlobal[k];
                 nos[j].Deslocamento[k] = dj[glGlobal];           
              }
            }

            for (i = 1; i <= nBarras; i++)
              barras[i].CalcularEsforcos();

            HistoricoCalculo("   Cálculo de esforços - [Ok]");

            return true;
        }

        private bool AplicarVinculos()
        {
            Atualiza(1);
            
            for (j = 1; j <= nNos; j++)
            {
              if (nos[j].vinculo == 1)
              {
                  nos[j].restrDZ = true;
              }
              else
              if (nos[j].vinculo == 2)
              {
                 nos[j].restrRX = true;
                 nos[j].restrRY = true;
                 nos[j].restrDZ = true;
              }
              else
              if (nos[j].vinculo == 3)
              {
                 nos[j].restrRX = false;
                 nos[j].restrRY = false;
                 nos[j].restrDZ = false; 
                  
                 nos[j].PossuiMolaRX = true;
                 nos[j].PossuiMolaRY = true;
                 nos[j].PossuiMolaDZ = true;

              //   nos[j].K_Mola_DZ = 6;
               //  nos[j].K_Mola_RX = 1;
                // nos[j].K_Mola_RY = 1;
              }
            }          

            return true;
        }

        private void MensagemGrelha()
        {
           HistoricoCalculo("   Geração de grelha [" + max_sBarra.ToString() +
                               " barras, " + max_sNo +
                               " nós, "+
                               " Malha ajustada para " + malha_horiz.ToString("n2") + " x " + malha_vert.ToString("n2") + "] - " + span2.Subtract(span1).ToString("mm") + ":" + span2.Subtract(span1).ToString("ss"));
        }

        public bool ReordenarNos()
        {
           // Application.DoEvents(); 
            MsgCalculo("Pavimento: " + this.Descricao, "Pavimento: " + this.Descricao + " - Reordenação nodal...", 0);
            Atualiza(1);
            TReordenaNosGrelha.ReordenaNos(ref nos, barras, nNos, nBarras, this);
            HistoricoCalculo("   Renumeração dos nós [Largura da semi-banda: " + maxBandaReordenacao.ToString() + "]");

            return true;
        }


        private bool SetMatrizesBarras()
        {
            Atualiza(1);
            for (int i = 1; i <= max_sBarra; i++)
            {
                barras[i].SetGlGlobal();
                barras[i].SetAnguloGlobal();
                barras[i].SetMatrizLocal();
                barras[i].SetMatrizRotacao();
                barras[i].SetMatrizGlobal();
            }

            return true;
        }
        
        public bool Carregamentos()
        {
            Atualiza(1);

            if (!TestesManuais)
            {
                CalculaPP();
                CalculaCarregamentoBarras();
            }

            double[] aj = new double[Ngl + 1];
            double[] ae = new double[Ngl + 1];
           
            int numero;
    
            for (i = 1; i <= nNos; i++)
            {
             //  if (Testes)
            
               // if (nos[i].vertice && nos[i].vinculo == 0 && nos[i].x == 0)
               //   nos[i].Carga[3] = -30;
              //  else
             //   if (nos[i].vertice && nos[i].vinculo == 0)
              //    nos[i].Carga[3] = -5;
           
                numero = nos[i].Numero;  
                for (j = 1; j <= 3; j++)  /*Aqui é o caso de uma carga ser aplicada diretamente no nó*/
                  aj[(numero - 1) * Ndj + j] = nos[i].Carga[j];
            }

            for (i = 1; i <= nBarras; i++)   /*Aqui é o caso de carga distribuida na barra. Isso converte a carga distribuida em ações nos nós (duas cargas concentradas e dois momentos)*/
              barras[i].PreencheAcoesEngPerf(ref ae);
            
            int jr;
            for (j = 1; j <= Ngl; j++)
            {
                jr = id[j];

                if (!glRestrito[j])
                    ac[jr] = aj[j] + ae[j];
            }
            

        /*    for (i = 1; i <= nNos; i++)
            {
              //  for (j = 1; j <= 3; j++)
                    if (! glRestrito[nos[i].GlGlobal[3]])
                  //    if (nos[i].GlGlobal[j])
                       ac[nos[i].GlGlobal[3]] = -0.00004;
            }*/

            return true;
        }

        bool VerificaSeHaConfiguracaoGrelha()
        {
            if (ConfiguracaGrelha.EspacamentoX == 0)
                return false;

            espacamentoX = ConfiguracaGrelha.EspacamentoX;
            espacamentoY = ConfiguracaGrelha.EspacamentoY;

            return true; 
        }
        bool RefazerGrelha;
        void Atualiza(int inc)
        {
            Progresso.Increment(inc);
          //  Application.DoEvents();
        //    gerenciador.Update();
         //   gerenciador.Refresh();
        }

        bool Testes = false;
        bool TestesManuais = false;
        void Inicializar()
        {
            ContaErro = 0;
            calculoOk = false;

            Progresso.Visible = true;
            Progresso.Value = 0;
            Progresso.Maximum = 19;
        }

        public bool Calcula()
        {
          //  gerenciador.ConfiguracoesPGi.CfgProjeto.sistema.Solver_cholesky = true;
         //   gerenciador.ConfiguracoesPGi.CfgProjeto.sistema.Solver_cg = false;
            
            Inicializar();
            TestesManuais = false;

            Testes = false;
        
            if (!VerificaSeHaConfiguracaoGrelha())
                throw new TErroConcepcaoEstrutural(this, ">Erro: Pavimento " + this.Descricao + " não possui grelha configurada. Cálculo interrompido.");

            HistoricoCalculo("Pavimento '" + this.Descricao + "':");
        
            if (ConfiguracaGrelha.GerarNovaGrelha)
            {
                RefazerGrelha = true;

                if (!TestesManuais)
                {
                    if (pilares.Count == 0)
                        throw new TErroConcepcaoEstrutural(this, ">Erro: Pavimento " + this.Descricao + " não possui pilares. Cálculo interrompido.");

                    if (pilares == null)
                        throw new TErroConcepcaoEstrutural(this, ">Erro: Pavimento " + this.Descricao + " não possui pilares. Cálculo interrompido.");

                    if (vigas.Count > 0)
                    {
                        while (RefazerGrelha)
                        {
                            if (!GerarGrelha2())
                                throw new TErroPavimento(this, "Pavimento: " + Descricao + "  -  Grelha não foi criada");
                        }
                    }
                    else
                    {
                        calculoOk = true;
                        return true;
                    }
                }
                else
                    GerarGrelha_Teste("sussekind_pg40");

                if (nBarras < 1 || nNos < 1)
                    throw new TErroPavimento(this, "Pavimento: " + Descricao + "  -  Grelha sem nenhum nó o barra.");
              
                MensagemGrelha();
                
                Atualiza(2);
            }

            if (!TestesManuais)
              if (!ReordenarNos())
                 throw new TErroPavimento(this, "Pavimento: " + Descricao + "  -  Erro na reordenação dos nós.");

            if (!SetMatrizesBarras())
                throw new TErroPavimento(this, "Pavimento: " + Descricao + "  -  Matrizes locais não puderam ser criadas.");

            if (!AplicarVinculos())
                throw new TErroPavimento(this, "Pavimento: " + Descricao + "  -  Erro na aplicação dos vínculos.");

            DadosEstruturais();

            if (!CriarMatrizSFF())
                throw new TErroPavimento(this, "Pavimento: " + Descricao + "  -  Reveja o lançamento do pavimento. Banda da matriz excede capacidade.");

            SetCoeficientesMola();

            if (!MatrizDeRigidez())
                throw new TErroPavimento(this, "Pavimento: " + Descricao + "  -  Erro na montagem do sistema de equações.");

          //  if (TestesManuais)
        //        gerenciador.ShowMatriz(-1, true); 

            VerificaSeCancelou();
           
            Atualiza(1);
            
            if (gerenciador.ConfiguracoesPGi.CfgProjeto.sistema.Solver_cholesky_supernodal)
              if (!MatrizRigidez.FatoraMatrizBanda(this.Descricao, "Pavimento: " + this.Descricao + " - Resolvendo matriz...", false))
                 throw new TErroPavimento(this, "Pavimento: " + Descricao + "  -  Erro na solução da matriz de rigidez. Estrutura inconsistente. Verifique!");

            if (!Carregamentos())
                return false;

            Atualiza(1);
          //  MatrizRigidez.ResolveMatrizBanda(ref ac, ref df, this.Descricao, "Pavimento: " + this.Descricao + " - Resolvendo matriz...");

            Atualiza(1);
            
     //       if (gerenciador.ConfiguracoesPGi.CfgProjeto.sistema.Solver1 && !MatrizRigidez.VerificarPrecisao(ref ac, ref df))
   //            throw new TErroPavimento(this, "Pavimento: " + Descricao + "  - Erro de precisão numérica.");

            if (!Resultados())
                return false;

            calculoOk = true;

            return calculoOk;
        }
    }
}
