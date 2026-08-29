using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.IO;


namespace PG
{
    public partial class Inicial : Form
    {
        Gerenciador gerenciador;
      //  NovoProjeto novoproj;

        string DiretorioSelecionado, substringDirectory;

        public Inicial()
        {
            InitializeComponent();
        }
        private void tv_NodeMouseClick(object sender, TreeNodeMouseClickEventArgs e)
        {
            if (e.Node.IsExpanded)
                e.Node.Collapse();
            else
                e.Node.Expand();
        }               

        private void AtualizarDiretorios()
        {
            Diretorios.Nodes.Clear();
           // MessageBox.Show(Environment.);

            if ( Directory.Exists(@"C:\" ))
            {
                Diretorios.Nodes.Add(@"C:\");
                Diretorios.Nodes[0].ToolTipText = @"C:\";
                PopulateTreeView(@"C:\", Diretorios.Nodes[0]);
            }
            else
                MessageBox.Show( "Diretório não econtrado", "Directory Not Found", MessageBoxButtons.OK,
                               MessageBoxIcon.Error );
        }
        
        List<string> dirs;
        List<string> dirs_pavimentos;
        private void AtualizarProjetos(string diretorio)
        {
            tv.Nodes.Clear();
            string dirPath = @diretorio;

            dirs            = new List<string>(Directory.EnumerateDirectories(dirPath));
            dirs_pavimentos = new List<string>();

            TreeNode n1;
            TreeNode[] nodes      = new TreeNode[dirs.Count];
            TreeNode[] pavimentos = new TreeNode[1];
            bool tem = false;
            for (int i = 0; i < dirs.Count; i++)
            {
                string nomeDir = dirs[i].Substring(dirs[i].LastIndexOf("\\") + 1);
                if (nomeDir.ToString().Contains(".pgproj"))
                {
                    nomeDir = nomeDir.Replace(".pgproj", "");

                    pavimentos[0] = new TreeNode("Pavimentos", 14, 15);
                   
                    tem = true;

                    nodes[i]             = new TreeNode(nomeDir, 13, 13, pavimentos);
                    nodes[i].ToolTipText = dirs[i];
                }
            }

            if (tem)
            {
                n1 = new TreeNode(@"Projetos", 12, 12, nodes);

                n1.ExpandAll();
                tv.Nodes.Add(n1);
            }
        }
        
        public void PopulateTreeView(string directoryValue, TreeNode parentNode )
        {
           // if (Directory.)
            string[] directoryArray = Directory.GetDirectories( directoryValue );

            try
            {
                if ( directoryArray.Length != 0 )
                {
                    foreach ( string directory in directoryArray )
                    {
                        if (!directory.ToString().Contains("$") && (!directory.ToString().Contains(".pgproj")))
                        {
                            substringDirectory = Path.GetFileNameWithoutExtension(directory);

                            TreeNode myNode = new TreeNode(substringDirectory);
                            myNode.ToolTipText = directory.ToString();
                            parentNode.Nodes.Add(myNode);

                            PopulateTreeView(directory, myNode);
                        }
                   } 
               } 
            }
            catch ( UnauthorizedAccessException )
            {
              parentNode.Nodes.Add( "Erro TreeView!" );
            } 
        }

        public Inicial(Gerenciador frmPai)
        {
            InitializeComponent();
            gerenciador = frmPai;

            AtualizarDiretorios();
        
            this.tv.NodeMouseClick += new TreeNodeMouseClickEventHandler(this.tv_NodeMouseClick);
        }

        private void button2_Click(object sender, EventArgs e)
        {
            DialogResult result;
            string diretorio;
            using (OpenFileDialog fileChooser = new OpenFileDialog())
            {
                result = fileChooser.ShowDialog();
                diretorio = fileChooser.FileName;
            }
        }

        private void button4_Click(object sender, EventArgs e)
        {
            AtualizarDiretorios();
        }

        private void tv_AfterSelect(object sender, TreeViewEventArgs e)
        {
            
        }

        private void tv_MouseDoubleClick(object sender, MouseEventArgs e)
        {
            MessageBox.Show(DiretorioSelecionado);   
        }

        private void btNovo_Click(object sender, EventArgs e)
        {
         //   novoproj = new NovoProjeto();
         //   novoproj.ShowDialog();
        }

        private void Diretorios_AfterSelect(object sender, TreeViewEventArgs e)
        {
            DiretorioSelecionado = e.Node.ToolTipText;

            AtualizarProjetos(DiretorioSelecionado);
        }

    }
}
