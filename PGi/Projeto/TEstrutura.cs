using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TriangleNet.Topology;

namespace PG
{
          [Serializable]
    public class TEstrutura
    {
        public bool calculoOk;
        public SClasses classeViga, classePilar, classeLaje;
        public float x_rot_angle, y_rot_angle, x_trans, y_trans, z_trans;
        public bool cameraOrto;
        public byte[] Rgb_Barras = new byte[3] { 0, 0, 0 };
        public List<TLayer> layers;
              [NonSerialized]
        public TPorticoEspacial PorticoEspacial;

              [NonSerialized]
              public List<TLaje> lajes;
              [NonSerialized]
              public List<TTrechoViga> vigas;
              [NonSerialized]
              public List<TBarraGenerica> barras;
              [NonSerialized]
              public List<TPilar> pilares;
              [NonSerialized]
              public List<TPavimento> pavimentos;
              [NonSerialized]
              public List<TApoio> apoios;
              [NonSerialized]
              public List<TPonto> nos;
              [NonSerialized]
              public List<TCargaLinear> cargaLinear;
              [NonSerialized]
              public List<TCargaPontual> cargaPontual;
              [NonSerialized]
              public List<TCombinacoes> combinacoes;

        public Dictionary<string, TLayer> LayersByIdPrincipal, LayersByIdArquitetura;
        [NonSerialized]
        public Gerenciador gerenciador;
        public TEstrutura(ref Gerenciador gerenciador)
        {
            objetos = new List<TObjetoDesenho>();
            lajes = new List<TLaje>();
            vigas   = new List<TTrechoViga>();
            pilares    = new List<TPilar>();
            pavimentos = new List<TPavimento>();
            barras = new List<TBarraGenerica>();
            apoios = new List<TApoio>();
            nos = new List<TPonto>(); 
            cargaLinear  = new List<TCargaLinear>();
            cargaPontual = new List<TCargaPontual>();
            combinacoes = new List<TCombinacoes>();
            PorticoEspacial = new TPorticoEspacial();
            this.gerenciador = gerenciador;
            this.CreateStandardLayers();
        }

        public bool TemElementoAvulso(ref string textoretorno)
        {
            List<TObjetoDesenho> _barras = new List<TObjetoDesenho>();

            foreach (TBarraGenerica b in barras)
            {
               _barras.Add(b);
                b.SetaSelecao(false, false, false);
            }
            int BarrasAvulsas = 0;
            List<TObjetoDesenho> retorno;
            foreach (TBarraGenerica b in barras)
            {
                b.SetaSelecao(true,true);
                retorno = TIntersecoes.RetornaConexoes(ref _barras);        

                if (retorno.Count > 0)
                {
                    b.SetaSelecao(false, false);
                }
                else
                {
                    bool temApoio = false;
                    foreach (TApoio a in apoios)
                    {
                        if ((Geom.Iguais(a.pIni.x, b.pIni.x) &&
                           Geom.Iguais(a.pIni.y, b.pIni.y) &&
                           Geom.Iguais(a.pIni.z, b.pIni.z))

                           ||

                           (Geom.Iguais(a.pIni.x, b.pFin.x) &&
                           Geom.Iguais(a.pIni.y, b.pFin.y) &&
                           Geom.Iguais(a.pIni.z, b.pFin.z)))
                        {
                            temApoio = true;
                            break;
                        }

                    }

                    if (!temApoio)
                    {
                        b.SetaSelecao(true, true, false);

                        gerenciador.formDesenho.ObjetosSelecionados.Add(b);

                        BarrasAvulsas++;
                    }
                    else
                        b.SetaSelecao(false, false, false);
                }
            }

            if (BarrasAvulsas > 0)
            {
                gerenciador.formDesenho.AtualizarDesenho(true, true);
                gerenciador.formDesenho.glControl.SwapBuffers();

                textoretorno = "Atenção! Existem elementos que não estão conectados com a estrutura! Verifique os elementos que foram realçados na tela";
                return true;
            }

            return false;
        }
        public bool TemElementoSobreposto(ref string textoretorno)
        {
            List<TBarraGenerica> _barras = new List<TBarraGenerica>();

            foreach (TBarraGenerica b in barras)
            {
                _barras.Add(b);
                b.SetaSelecao(false, false);
            }

            List<TBarraGenerica> retorno = TIntersecoes.RetornaElementosSobrepostos(ref barras);

            foreach (TBarraGenerica o in retorno)
            {
                gerenciador.formDesenho.ObjetosSelecionados.Add(o);
                o.SetaSelecao(true, true);
            }

            if (retorno.Count > 0)
            {
                textoretorno = "Atenção!" +
                    (retorno.Count > 1 ? " Existem " + retorno.Count.ToString() + " elementos sobrepostos um ao outro. Verifique os elementos que foram realçados na tela!" :
                    " Existe um elemento sobreposto ao outro. Verifique o elemento que foi realçado na tela!");

                gerenciador.formDesenho.AtualizarDesenho(false, true);
                gerenciador.formDesenho.glControl.SwapBuffers();

                return true;
            }
            else
                return false;
        }

        public void DeletaElementosComTamanhoZero()
        {
            gerenciador.formDesenho.SetaSelecionados(false,-1, false);
            bool tem = false;
            for (int i = 0; i < barras.Count; i++)
            {
                if (Geom.Iguais(barras[i].pIni.DistanceTo(barras[i].pFin), 0, 0.001))
                {
                    barras[i].SetaSelecao(true, true);

                    tem = true;
                }

            }

            if (tem)
              gerenciador.formDesenho.DeletaSelecionados(-1);
        }
        public void VerificaValencia_pIni_pFin(TBarraGenerica bar, ref int ini, ref int fin)
        {
            List<TPonto> pontos = new List<TPonto>();
            vec3 p_1 = new vec3(bar.pIni.x, bar.pIni.y, bar.pIni.z);
            vec3 p_2 = new vec3(bar.pFin.x, bar.pFin.y, bar.pFin.z);
            vec3 p_3;
            vec3 p_4;
            vec3 pontoToque = new vec3(0);

            foreach (TApoio a in apoios)
            {
                if (Geom.Iguais(bar.pIni.x, a.pIni.x) &&
                   Geom.Iguais(bar.pIni.y, a.pIni.y) &&
                   Geom.Iguais(bar.pIni.z, a.pIni.z))
                    ini++;

                if (Geom.Iguais(bar.pFin.x, a.pIni.x) &&
                   Geom.Iguais(bar.pFin.y, a.pIni.y) &&
                   Geom.Iguais(bar.pFin.z, a.pIni.z))
                    fin++;
            }

            foreach (TBarraGenerica b in barras)
            {
                if (b.IDBarra != bar.IDBarra)
                {
                    p_3 = new vec3(b.pIni.x, b.pIni.y, b.pIni.z);
                    p_4 = new vec3(b.pFin.x, b.pFin.y, b.pFin.z);

                    if (Geom.PontoTocaAresta(p_1, p_3, p_4, ref pontoToque, true))
                        ini++;

                    if (Geom.PontoTocaAresta(p_2, p_3, p_4, ref pontoToque, true))
                        fin++;
                }
            }
        }

        public bool TemElementosArticuladosEmBalanco(ref string textoretorno)
        {
            int cc = 0;
            foreach (TBarraGenerica b in barras)
            {
                bool art_ini = (b.Dados.Articulacao_my == 1 || b.Dados.Articulacao_my == 2 ||
                                b.Dados.Articulacao_mz == 1 || b.Dados.Articulacao_mz == 2);

                bool art_fim = (b.Dados.Articulacao_my == 1 || b.Dados.Articulacao_my == 3 ||
                                b.Dados.Articulacao_mz == 1 || b.Dados.Articulacao_mz == 3);

                int val_ini = 1;
                int val_fin = 1;

                if (art_ini || art_fim)
                {
                    VerificaValencia_pIni_pFin(b, ref val_ini, ref val_fin);

                    if (art_fim && val_ini == 1)
                    {
                        gerenciador.formDesenho.ObjetosSelecionados.Add(b);
                        b.SetaSelecao(true, true);
                        cc++;
                    }

                    if (art_ini && val_fin == 1)
                    {
                        gerenciador.formDesenho.ObjetosSelecionados.Add(b);
                        b.SetaSelecao(true, true);
                        cc++;
                    }
                }
            }

            if (cc > 0)
            {
                textoretorno = "Atenção! \r Existe/m " + cc.ToString() + " elemento/s articulado/s em uma extremidade mas em balanço na outra extremidade, gerando assim um mecanismo instável. \r Verifique os elementos que foram realçados na tela!";

                gerenciador.formDesenho.AtualizarDesenho(false, true);
                gerenciador.formDesenho.glControl.SwapBuffers();

                return true;
            }
            else
                return false;
            
        }

        public bool TemNosPerdidos(ref string textoretorno, double tol)
        {
            List<TBarraGenerica> _barras = new List<TBarraGenerica>();
            
            foreach (TBarraGenerica b in barras)
            {
               _barras.Add(b);
                b.SetaSelecao(false, false);
            }

          //  List<TPonto> nos = new List<TPonto>();
            foreach (TPonto b in nos)
            {
                b.Selecionado = false;
            //    nos.Add(b);
            }
            List<TPonto> retorno = TIntersecoes.ConectarNosPerdidos(ref nos, ref _barras, tol);
            //    List<TPonto> retorno = TIntersecoes.RetornaNosProximos(ref nos, ref _barras, tol);
            gerenciador.formDesenho.Atualiza_pIni_pFin_das_Barras_e_Cargas();
            foreach (TPonto o in retorno)
            {
                o.Selecionado = true;
            }

            if (retorno.Count > 0)
            {
                gerenciador.ConfiguracoesPGi.F3DOpcoesVisualizacao.Nos = true;
                gerenciador.formDesenho.MostrarNos = true;
                textoretorno = "Atenção!" +
                    (retorno.Count > 1 ? " Existem "+ retorno.Count.ToString() + " nós a menos de "+tol.ToString()+" mm de outro elemento/nó. Verifique os nós que foram realçados na tela!" :
                    " Existe um nó a menos de "+tol.ToString()+" mm de outro elemento/nó. Verifique o nó que foi realçado na tela!");

                gerenciador.formDesenho.AtualizarDesenho(false, true);
                gerenciador.formDesenho.glControl.SwapBuffers();

                return true;
            }
            else
                return false;
        }

        public void CriaListas(Gerenciador gerenciador)
        {
            lajes = new List<TLaje>();
            vigas = new List<TTrechoViga>();
            pilares = new List<TPilar>();
            pavimentos = new List<TPavimento>();
            barras = new List<TBarraGenerica>();
            apoios = new List<TApoio>();
            nos = new List<TPonto>();
            cargaLinear = new List<TCargaLinear>();
            cargaPontual = new List<TCargaPontual>();
            combinacoes = new List<TCombinacoes>();

            this.gerenciador = gerenciador;
        }


        TLayer RetLayerConfigurado(int indice)
        {
            return gerenciador.ConfiguracoesPGi.layers[indice];
        }

        int cl;

        public void CreateStandardLayers()
        {
            this.LayersByIdPrincipal = new Dictionary<string, TLayer>();
            this.LayersByIdArquitetura = new Dictionary<string, TLayer>();
            this.layers = new List<TLayer>();

            cl = -1;

            CriaLayer(Lay.Zero, true, false, new byte[3] { 255, 0, 0 }, 0, false);
            CriaLayer(Lay.ElementosBasicos, true, false, new byte[3] { 214, 214, 214 }, 0, false);
            CriaLayer(Lay.Vigas, true, false, new byte[3] { 62, 64, 250 }, 0, false);
            CriaLayer(Lay.Pilares, true, false, new byte[3] { 0, 0, 70 }, 0, false);
            CriaLayer(Lay.Lajes, true, false, new byte[3] { 169, 169, 169 }, 0, false);
            CriaLayer(Lay.Barras, true, false, new byte[3] { 0, 71, 170 }, 0, false);
            CriaLayer(Lay.GrelhaLajes, false, true, new byte[3] { 112, 128, 144 }, 0, false);
            CriaLayer(Lay.TextosVigas, true, false, new byte[3] { 65, 127, 255 }, 0, false);
            CriaLayer(Lay.TextosLajes, true, false, new byte[3] { 64, 128, 128 }, 0, false);
            CriaLayer(Lay.TextosPilares, true, false, new byte[3] { 160, 160, 160 }, 0, false);
            CriaLayer(Lay.CargaPontual, true, false, new byte[3] { 0, 255, 0 }, 0, false);
            CriaLayer(Lay.CargaLinear, true, false, new byte[3] { 153, 50, 204 }, 0, false);
        }
        public void CriaLayer(string Nome, bool ligado, bool congelado, byte[] rgb, int tipolinha, bool travado)
        {
            cl++;
            layers.Add(new TLayer(Nome, ligado, congelado, rgb, 0, travado, GrupoLay.Principal));

            LayersByIdPrincipal[Nome] = layers[cl];
        }

        int kk;

        [NonSerialized]
        public List<TObjetoDesenho> objetos;
        public void AtualizaListaObjetos()
        {
            objetos = new List<TObjetoDesenho>();
            foreach (TTrechoViga o in vigas)
                objetos.Add(o);
            foreach (TPilar o in pilares)
                objetos.Add(o);
            foreach (TLaje o in lajes)
                objetos.Add(o);
            foreach (TBarraGenerica o in barras)
                objetos.Add(o);
            foreach (TApoio o in apoios)
                objetos.Add(o);
            foreach (TPonto o in nos)
                objetos.Add(o);
            foreach (TCargaLinear o in cargaLinear)
                objetos.Add(o);
            foreach (TCargaPontual o in cargaPontual)
                objetos.Add(o);
        }

        public int GetCountElementosSelecionados()
        {
            kk = 0;
            foreach (TTrechoViga obj in vigas)
                if (obj.Selecionado)
                    if (obj.ObjetoPrimario)
                        kk++;
            foreach (TPilar obj in pilares)
                if (obj.Selecionado)
                    if (obj.ObjetoPrimario)
                        kk++;
            foreach (TLaje  obj in lajes)
                if (obj.Selecionado)
                    if (obj.ObjetoPrimario)
                        kk++;

            foreach (TBarraGenerica obj in barras)
                if (obj.Selecionado)
                   // if (obj.ObjetoPrimario)
                        kk++;

            foreach (TApoio obj in apoios)
                if (obj.Selecionado)
                    if (obj.ObjetoPrimario)
                        kk++;

            foreach (TCargaLinear obj in cargaLinear)
                if (obj.Selecionado)
                    if (obj.ObjetoPrimario)
                        kk++;

            foreach (TCargaPontual obj in cargaPontual)
                if (obj.Selecionado)
                    if (obj.ObjetoPrimario)
                        kk++;

            foreach (TPonto obj in nos)
            {
                if (obj.Selecionado)
                    //if (obj.ObjetoPrimario)
                    kk++;
            }

            return kk;
        }
        [NonSerialized]
        public double xMin, xMax, yMin, yMax, zMin, zMax;
        [NonSerialized]
        public int xMin_Pixel, xMax_Pixel, yMin_Pixel, yMax_Pixel;
        public void CalculaMinMaxCoordenadas(bool deformada)
        {
            double xMin = 99999999999;
            double yMin = 99999999999;
            double zMin = 99999999999;

            double xMax = -99999999999;
            double yMax = -99999999999;
            double zMax = -99999999999;

            foreach (TBarraGenerica obj in barras)
            {
                if (!deformada) 
                  if (!obj.Visivel) continue;

               // if (obj.pIni.z == zIni)
                    if (obj.pIni.x < xMin)
                        xMin = obj.pIni.x;

               // if (obj.pFin.z == zIni)
                    if (obj.pFin.x < xMin)
                        xMin = obj.pFin.x;

              //  if (obj.pIni.z == zIni)
                    if (obj.pIni.x > xMax)
                        xMax = obj.pIni.x;

              //  if (obj.pFin.z == zIni)
                    if (obj.pFin.x > xMax)
                        xMax = obj.pFin.x;

                /*z*/
                    if (obj.pIni.z*-1 > zMax)
                        zMax = obj.pIni.z*-1;

                    if (obj.pFin.z * -1 > zMax)
                        zMax = obj.pFin.z * -1;

                    if (obj.pIni.z * -1 < zMin)
                        zMin = obj.pIni.z * -1;

                    if (obj.pFin.z * -1 < zMin)
                        zMin = obj.pFin.z * -1;
                /*z*/

                ///
               // if (obj.pIni.z == zIni)
                    if (obj.pIni.y < yMin)
                        yMin = obj.pIni.y;

              //  if (obj.pFin.z == zIni)
                    if (obj.pFin.y < yMin)
                        yMin = obj.pFin.y;

               // if (obj.pIni.z == zIni)
                    if (obj.pIni.y > yMax)
                        yMax = obj.pIni.y;

               // if (obj.pFin.z == zIni)
                    if (obj.pFin.y > yMax)
                        yMax = obj.pFin.y;
            }
            this.xMin = xMin;
            this.xMax = xMax;
            this.yMin = yMin;
            this.yMax = yMax;
            this.zMin = -zMin;
            this.zMax = -zMax;
        }

        public void Desenha3D(ref bool unifilar,ref int transparencia, ref bool arestas)
        {
            foreach (TLaje o in lajes)
            {
                o.Desenha(ref unifilar,  ref transparencia, ref arestas);   
            }
            foreach (TTrechoViga o in vigas)
            {
                o.Desenha(ref unifilar, ref transparencia, ref arestas);
            }
            foreach (TPilar o in pilares)
            {
                o.Desenha(ref unifilar, ref transparencia, ref arestas);
            }
            foreach (TBarraGenerica o in barras)
            {
                o.Desenha(ref unifilar, ref transparencia, ref arestas);
            }
            foreach (TApoio o in apoios)
            {
                o.Desenha(ref unifilar, ref transparencia, ref arestas);
            }
            foreach (TPonto o in nos)
                o.Desenha(ref unifilar, ref transparencia, ref arestas);
        
            foreach (TCargaLinear o in cargaLinear)
                o.Desenha(ref unifilar, ref transparencia, ref arestas);
           
            foreach (TCargaPontual o in cargaPontual)
                o.Desenha(ref unifilar, ref transparencia, ref arestas);
        }
    }
}
