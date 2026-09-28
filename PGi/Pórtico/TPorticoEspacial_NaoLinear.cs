using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PG
{
    public class TPorticoEspacial_NaoLinear
    {
        private readonly TPorticoEspacial portico;
        private readonly int numModos;
        private alglib.sparsedecompositionanalysis analiseCholesky;
        private alglib.sparsematrix matrizRigidezFatorada;
    }
}
