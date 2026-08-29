using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PG
{
    [Serializable]
    public class TObjetoDesenho
    {
        private bool selecionado,
                     visivel,
                     mostrargrip;
        public bool ObjetoPrimario, CandidatoSelecao, ObjetoCopia;
        public bool ApagarSelecionados = true;
        public string NomePavimento;
        private string tipo;
        public int Piso;
        public int   ID;
        public float  angulo, anguloGlobal;
        public double Soma_Z;
        public List<TGrip> Grips; //retirar isso ao serializar
        public bool NaoPermiteMoverOuCopiar;
        public TLayer layer; // retirar isso ao serializar
        public string IdLayer;
        public TPonto pIni, pFin;
         public bool MostrarGrip { get { return mostrargrip; } set { mostrargrip = value; } }
        public TLayer Layer { get { return layer; } set { layer = value; } }
        public bool Visivel { get { return visivel; } set { visivel = value; } }
        public bool Selecionado { get { return selecionado; } set { selecionado = value; }}
        public string Tipo{get { return tipo; } set { tipo = value; }}
        public int Registro { get { return ID; } set { ID = value; } }
        public virtual void Atualiza(int pavimento) { }
        public virtual void Cancel(bool cmd, ref string msg,ref List<TPonto> points) {  }
        public virtual void Continue() { }
        public byte[] Rgb = new byte[3] { 0, 0, 0 }; 
        public TObjetoDesenho() { }

        public TObjetoDesenho(string tipo, int Id, bool selecionado = false, TLayer layer = null) 
        { 
            this.tipo        = tipo; 
            this.selecionado = selecionado;
            this.ID    = Id;
            this.visivel     = false;
            this.mostrargrip = false;
            this.layer       = layer;
            NaoPermiteMoverOuCopiar = false;
        }
        public virtual void Proximo(){}

        public virtual TObjetoDesenho Clone() { return null; }
        public virtual TPonto LastPoint(){return null;}
        public virtual TPonto FirstPoint(){return null;}
        public virtual void ShowDynamicInfo() { }
        public virtual void Copy(TObjetoDesenho obj) {}

        public virtual bool Command(System.Windows.Forms.Keys key) { return false; }

        public virtual void Initialize(ref TPonto point, ref string command, ISettings settings, TLayer layer, ref List<TLinha> Linhas, ref int Pavimento) { }
        public virtual void Initialize(ref TPonto point, ISettings settings, TLayer layer ) { }

        public virtual void OnMouseMove(ref TPonto point, bool ShowInfo, bool orto = true, double angulo = 0, bool PontoInicial = false) { }

        public virtual eObjetoDesenhoMouseDown OnMouseDown(ref TPonto point, ref string command, List<TLinha> Linhas, List<TPonto> Pontos, List<TPilar> Pilares, ref List<TObjetoDesenho> Objects, bool FromGrip = false) { return 0; }
        public virtual void Rotacionar(TPonto ponto1, TPonto ponto2, double ang) { }
        public virtual void Mover(ref TPonto ponto1,ref  TPonto ponto2, bool dinamico) { }

        public virtual void OnMove() { }

        public virtual void Desenha(ref System.Drawing.Graphics Cad) { }
        public virtual void Desenha(ref bool Unifilar, ref int transp, ref bool arestas) { }
        public virtual void Desenha() { }
        public virtual void Desenha(float x, float y) { }

        public virtual bool Selecionar(float clicx, float clicy, float coordx, float coordy, TObjetoDesenho Owner = null) { return true; }

        public virtual void PreSelecionar(ref int clicx, ref int clicy) { }

        public virtual void AddGrips() { }

        public virtual void ShowHideGrips(bool visivel) { }
        
        public virtual void RemoveGrips() { }

        public virtual void SetaSelecao(bool s, bool MostrarGrip, bool SelecaoDeCandidato=false, TObjetoDesenho Owner = null) { }
   
        public virtual string PrimeiroComando() {return "";}
 }
}
