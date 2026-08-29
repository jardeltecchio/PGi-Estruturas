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
    public partial class FFontes : Form
    {
        Gerenciador gerenciador;
        public FFontes(Gerenciador gerenciador)
        {
            InitializeComponent();
            this.gerenciador = gerenciador;
        }
        public FFontes()
        {
            InitializeComponent();
        }

        private void button7_Click(object sender, EventArgs e)
        {
 
        }

        private void button1_Click(object sender, EventArgs e)
        {
            if (fontDialog1.ShowDialog() == DialogResult.OK)
            {
                NomeViga.Font = fontDialog1.Font;
                NomeViga.Text = fontDialog1.Font.Name;
            }
        }

        private void button7_Click_1(object sender, EventArgs e)
        {
            this.Close();
        }

    }
}
