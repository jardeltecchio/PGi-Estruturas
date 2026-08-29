using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PG
{

    public class TRenomeiaTrechosVigas : IEditTool
    {
        public IEditTool Clone()
        {
            return new TRenomeiaTrechosVigas();
        }

        List<string> ListaComandos;
        public TRenomeiaTrechosVigas()
        {
            //lista de comandos em ordem de execução
            ListaComandos = new List<string>();
            ListaComandos.Add("Selecione os trechos de viga.");
            ListaComandos.Add("OK - Vigas renomeadas.");
        }

        public void Initialize(TObjetoDesenho objeto, ref TPonto point, ref string command, ISettings settings, TLayer layer)
        {
            // command = Const.CMD_PILAR_ROTACIONAR_1_P;

            // this.trecho = objeto as TTrechoViga;
        }

        TLinha Eixo;

        bool Definindo;
        FRenomearElementos FRenomear;
        public eObjetoDesenhoMouseDown OnMouseDown(Gerenciador gerenciador, FPrincipal desenho, ref List<TObjetoDesenho> ObjetosSelecionados, ref List<TObjetoDesenho> ObjetosResultado, ref TPonto point, ref string command, List<TLinha> Linhas, List<TPonto> Pontos, bool FromGrip = false)
        {
            if (Definindo)
            {
                FRenomear = new FRenomearElementos();
                FRenomear.ShowDialog();

                return eObjetoDesenhoMouseDown.Done;
            }
            else
            {
                FRenomear = new FRenomearElementos();
                System.Windows.Forms.DialogResult result = FRenomear.ShowDialog();

                foreach (TObjetoDesenho o in ObjetosSelecionados)
                {
                    o.Selecionado = false;
                    o.SetaSelecao(false, false);
                    if ((Object)o.Grips != null)
                      foreach (TGrip grip in o.Grips)
                        grip.SetaSelecao(false,false);
                } 

                if (result == System.Windows.Forms.DialogResult.OK)
                {
                    foreach (TObjetoDesenho trecho in ObjetosSelecionados)
                    {
                        if (trecho.Tipo == Const.ID_TRECHOVIGA)
                        {
                            (trecho as TTrechoViga).SetaSelecao(false,false);
                            
                            int num = -1;
                            string prefixo = "";

                            if (FRenomear.NumeroInicial.Text.Trim() != "")
                              num = int.Parse(FRenomear.NumeroInicial.Text.ToString() + (trecho as TTrechoViga).Dados.numero.ToString());

                            if (FRenomear.NovoPrefixo.Text.Trim() != "")
                                prefixo = FRenomear.NovoPrefixo.Text;

                            if (num != -1)
                            {
                                (trecho as TTrechoViga).Dados.numero = num;
                                (trecho as TTrechoViga).Texto1.texto = (trecho as TTrechoViga).Dados.nome + num.ToString();
                            }

                            if (prefixo != "")
                            {
                                (trecho as TTrechoViga).Dados.nome = prefixo;
                                (trecho as TTrechoViga).Texto1.texto = (trecho as TTrechoViga).Dados.nome + (trecho as TTrechoViga).Dados.numero;
                            }
                            (trecho as TTrechoViga).Texto1.OnMove((trecho as TTrechoViga).Texto1.x, (trecho as TTrechoViga).Texto1.y);

                            foreach (TGrip grip in (trecho as TTrechoViga).Texto1.Grips)
                                grip.SetaSelecao(false, false);
                            foreach (TGrip grip in (trecho as TTrechoViga).Texto2.Grips)
                                grip.SetaSelecao(false, false);
                        }
                    }


                    return eObjetoDesenhoMouseDown.Done;

                }
            }

            return eObjetoDesenhoMouseDown.Done;
        }

        public void Continue()
        {
      
        }
        public bool ApagarSelecionados
        {
              get { return true; }
        }
        public string GetComando(int numeroComando)
        {
            return ListaComandos[numeroComando];
        }

        public string TipoObjetoParaSelecionar
        {
            get { return Const.ID_TRECHOVIGA; }
        }
        public string LayerObjetoParaSelecionar
        {
            get { return Lay.Vigas; }
        }
        public eEditToolTipoSelecao editToolTipoSelecao
        {
            get { return eEditToolTipoSelecao.multiSelecao; }
        }

        public void OnMouseMove(ref TPonto point, bool ShowInfo, bool orto = true, double angulo = 0, bool PontoInicial = false)
        {
        }
        public void OnMouseUp(ref TPonto point)
        {
        }
        public void OnKeyDown(System.Windows.Forms.KeyEventArgs e)
        {
        }
        public void Finished()
        {
        }
        public void Undo()
        {
        }
        public void Redo()
        {
        }
 
    }

    public class TDivideTrechoViga : IEditTool
    {
        List<string> ListaComandos;
        bool DefinindoPonto = false;

        public TDivideTrechoViga()
        {
            //lista de comandos em ordem de execução
            ListaComandos = new List<string>();
            ListaComandos.Add("Selecione o trecho de viga.");
            ListaComandos.Add("Indique o ponto de divisão");
        }

        public IEditTool Clone()
        {
            return new TDivideTrechoViga();
        }

        public void Initialize(TObjetoDesenho objeto, ref TPonto point, ref string command, ISettings settings, TLayer layer)
        {
            // command = Const.CMD_PILAR_ROTACIONAR_1_P;

            // this.trecho = objeto as TTrechoViga;
        }

        TLinha Eixo;
        double xant, yant, beta, nx, ny;
        public eObjetoDesenhoMouseDown OnMouseDown(Gerenciador gerenciador, FPrincipal desenho, ref List<TObjetoDesenho> ObjetosSelecionados, ref List<TObjetoDesenho> ObjetosResultado, ref TPonto point, ref string command, List<TLinha> Linhas, List<TPonto> Pontos, bool FromGrip = false)
        {
            if (DefinindoPonto)
            {
          
                /*Redefine o ponto inicial como sendo o ponto que clicou*/
                Eixo = (ObjetosSelecionados[0] as TTrechoViga).linha_eixo_aux;
           
                TPonto PontoInicial = new TPonto(Eixo.pIni.x, Eixo.pIni.y, 0);
                
                Eixo.pIni.y = point.y;
                Eixo.pIni.x = point.x;
                Eixo.UpdatePixel();

                (ObjetosSelecionados[0] as TTrechoViga).Grips[0].x = point.x;
                (ObjetosSelecionados[0] as TTrechoViga).Grips[0].y = point.y;

                (ObjetosSelecionados[0] as TTrechoViga).comprimento = (float)Eixo.pFin.DistanceTo(Eixo.pIni);
                (ObjetosSelecionados[0] as TTrechoViga).angulo = (float)(FuncoesGerais.atand((Eixo.pIni.y - Eixo.pFin.y) / (Eixo.pIni.x - Eixo.pFin.x)));
                (ObjetosSelecionados[0] as TTrechoViga).anguloGlobal = (float)RMath.rad2deg(Eixo.pIni.getAngleTo(Eixo.pFin));
                Eixo.angulo = (float)(ObjetosSelecionados[0] as TTrechoViga).angulo;

                TTrechoViga Trecho = (ObjetosSelecionados[0] as TTrechoViga);

                beta = Trecho.angulo;
                if (Trecho.linha_eixo_aux.pFin.x >= Trecho.linha_eixo_aux.pIni.x)
                    beta -= 90;
                else
                    beta += 90;

                double xx10 = (float)(Trecho.Dados.b1 * Math.Cos(beta * Const.PIDiv180));
                double yy10 = (float)(Trecho.Dados.b1 * Math.Sin(beta * Const.PIDiv180));

                if (beta == 0 && Trecho.linha_eixo_aux.pFin.y < Trecho.linha_eixo_aux.pIni.y)
                    xx10 = -xx10;

                Trecho.linhas_eixo.pIni.x = Trecho.linha_eixo_aux.pIni.x;
                Trecho.linhas_eixo.pIni.y = Trecho.linha_eixo_aux.pIni.y;
                Trecho.linhas_eixo.pFin.x = Trecho.linha_eixo_aux.pFin.x;
                Trecho.linhas_eixo.pFin.y = Trecho.linha_eixo_aux.pFin.y;

                Trecho.SetaCoords(Trecho, 1, Trecho.linhas_eixo.pIni, Trecho.linhas_eixo.pFin, ref xx10, ref yy10);

                Trecho.linhas_facebaixo_org.pIni.x = Trecho.linhas_facebaixo.pIni.x;
                Trecho.linhas_facebaixo_org.pIni.y = Trecho.linhas_facebaixo.pIni.y;
                Trecho.linhas_facebaixo_org.pFin.x = Trecho.linhas_facebaixo.pFin.x;
                Trecho.linhas_facebaixo_org.pFin.y = Trecho.linhas_facebaixo.pFin.y;

                Trecho.linhas_eixo_org.pIni.x = Trecho.linhas_eixo.pIni.x;
                Trecho.linhas_eixo_org.pIni.y = Trecho.linhas_eixo.pIni.y;
                Trecho.linhas_eixo_org.pFin.x = Trecho.linhas_eixo.pFin.x;
                Trecho.linhas_eixo_org.pFin.y = Trecho.linhas_eixo.pFin.y;

                Trecho.linhas_facecima_org.pIni.x = Trecho.linhas_facecima.pIni.x;
                Trecho.linhas_facecima_org.pIni.y = Trecho.linhas_facecima.pIni.y;
                Trecho.linhas_facecima_org.pFin.x = Trecho.linhas_facecima.pFin.x;
                Trecho.linhas_facecima_org.pFin.y = Trecho.linhas_facecima.pFin.y;

                Trecho.ReposicionaTextos();

                Trecho.UpdateTodas(Trecho);

                Trecho.UpdatePixelTodas(Trecho);

                Trecho.Grips[1].x = Trecho.linhas_eixo.pFin.x;
                Trecho.Grips[1].y = Trecho.linhas_eixo.pFin.y;
                Trecho.Grips[0].x = Trecho.linhas_eixo.pIni.x;
                Trecho.Grips[0].y = Trecho.linhas_eixo.pIni.y;


           /* Cria o trecho novo - Ponto inicial vai ser o ponto inicial do trecho original, e o ponto final vai ser em cima do ponto que clicou*/
               
                TTrechoViga New = new TTrechoViga();
                TPonto p1, p2;
                string ss = "";

                p1 = new TPonto(PontoInicial.x, PontoInicial.y, PontoInicial.z, FPrincipal.pixelX(PontoInicial.x), FPrincipal.pixelY(PontoInicial.y), -1);
                p2 = new TPonto(point.x, point.y, point.z,FPrincipal.pixelX(point.x), FPrincipal.pixelY(point.y), -1);
               
                New.pIni = p1;
                New.pFin = p2;       
                New.Initialize(ref p1, ref ss, Trecho.Dados, Trecho.layer, ref Linhas, ref Trecho.Pavimento);

                New.angulo = (float)(FuncoesGerais.atand((p1.y - p2.y) / (p1.x - p2.x)));


                beta = New.angulo;
                if (p2.x >= p1.x)
                  beta -= 90;
                else
                  beta += 90;

                nx = (float)(New.Dados.b1 * Math.Cos(beta * Const.PIDiv180));
                ny = (float)(New.Dados.b1 * Math.Sin(beta * Const.PIDiv180));

                if (beta == 0 && New.linhas_eixo.pFin.y < New.linhas_eixo.pIni.y)
                  nx = -nx;

                New.SetaCoords(New, 1, p1, p2, ref nx, ref ny);
                
                New.linhas_facecima.UpdatePixel();
                New.linhas_facebaixo.UpdatePixel();
                New.linhas_eixo.UpdatePixel();

                if (!New.pFin.Incidente(New.linhas_eixo))
                    New.pFin.incidencias.Add(New.linhas_eixo);

                if (!New.pIni.Incidente(New.linhas_eixo))
                    New.pIni.incidencias.Add(New.linhas_eixo);

 
                New.linhas_facebaixo.angulo = (float)New.angulo;
                New.linhas_eixo.angulo = (float)New.angulo;
                New.linhas_facecima.angulo = (float)New.angulo;

                New.linhas_eixo.pIni.PontoEixoViga = true;
                New.linhas_eixo.pFin.PontoEixoViga = true;

                New.linhas_facebaixo.pIni.PontoEixoViga = false;
                New.linhas_facebaixo.pFin.PontoEixoViga = false;

                New.linhas_facecima.pIni.PontoEixoViga = false;
                New.linhas_facecima.pFin.PontoEixoViga = false;

                New.linhas_facebaixo_org = new TLinha(new TPonto(New.linhas_facebaixo.pIni.x, New.linhas_facebaixo.pIni.y, 0),
                                                      new TPonto(New.linhas_facebaixo.pFin.x, New.linhas_facebaixo.pFin.y, 0),
                                                      -1);
                New.linhas_eixo_org = new TLinha(new TPonto(New.linhas_eixo.pIni.x, New.linhas_eixo.pIni.y, 0),
                                                      new TPonto(New.linhas_eixo.pFin.x, New.linhas_eixo.pFin.y, 0),
                                                      -1);

                New.linhas_facecima_org = new TLinha(new TPonto(New.linhas_facecima.pIni.x, New.linhas_facecima.pIni.y, 0),
                                                      new TPonto(New.linhas_facecima.pFin.x, New.linhas_facecima.pFin.y, 0),
                                                      -1);
                New.AddGrips();

                New.UpdateTodas(New);

                New.UpdatePixelTodas(New);

                New.CriaTextos(New, New.Dados.b1 + "/" + New.Dados.h1, New.Dados.nome + New.Dados.numero);
              
                ObjetosResultado.Add(New);

                DefinindoPonto = false;

                Trecho.SetaSelecao(false,false);
                New.SetaSelecao(false, false);
                
                return eObjetoDesenhoMouseDown.Done;
            }
            else
            {
                command = GetComando(1);
                return eObjetoDesenhoMouseDown.Continue;
            }
        }

        public void Continue()
        {
            DefinindoPonto = true;
        }
        public bool ApagarSelecionados
        {
           get { return true; }
        }

        public string GetComando(int numeroComando)
        {
            return ListaComandos[numeroComando];
        }

        public string TipoObjetoParaSelecionar
        {
            get { return Const.ID_TRECHOVIGA; }
        }

        public string LayerObjetoParaSelecionar
        {
            get { return Lay.Vigas; }
        }

        public eEditToolTipoSelecao editToolTipoSelecao
        {
            get { return eEditToolTipoSelecao.selecaoUnica; }
        }

        public  void OnMouseMove(ref TPonto point, bool ShowInfo,  bool orto = true, double angulo = 0, bool PontoInicial = false)
        {
        }
        public void OnMouseUp(ref TPonto point)
        {
        }
        public void OnKeyDown(System.Windows.Forms.KeyEventArgs e)
        {
        }
        public void Finished()
        {
        }
        public void Undo()
        {
        }
        public void Redo()
        {
        }
    }

}
