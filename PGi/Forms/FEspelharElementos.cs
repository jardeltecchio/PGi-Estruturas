using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace PG
{
    public partial class FEspelharElementos : Form
    {
        public List<TObjetoDesenho> objetos;
        Gerenciador gerenciador;
        public FEspelharElementos(Gerenciador _gerenciador)
        {
            InitializeComponent();
            this.gerenciador = _gerenciador;
        }


        public FEspelharElementos()
        {
            InitializeComponent();
        }
        public string Comando;
        public TPonto p1, p2;

        private void FEspelharElementos_FormClosed(object sender, FormClosedEventArgs e)
        {
            gerenciador.fEspelharelementos= null;
        }

        private void FEspelharElementos_Move(object sender, EventArgs e)
        {
            gerenciador.AtualizaDesenho();
        }

        private void FEspelharElementos_Load(object sender, EventArgs e)
        {
            this.Left = 25;
            this.Top = 200;
            objetos = new List<TObjetoDesenho>();
        }
    }
}
