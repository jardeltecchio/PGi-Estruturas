using System;
using System.Collections.Generic;
using System.Linq;

namespace PG
{
    public partial class TPorticoEspacial
    {
        [NonSerialized] Dictionary<int, double>[] transformacaoMPC;
        [NonSerialized] int[] independentesMPC;
        internal bool UsaEliminacaoMPC => TemMPC &&
            (gerenciador?.ConfiguracoesPGi?.CfgProjeto?.sistema?.MetodoMPC ?? MetodoVinculosMPC.Eliminacao) == MetodoVinculosMPC.Eliminacao;
        internal int OrdemSistemaMPC => UsaEliminacaoMPC ? independentesMPC.Length : NLinhas;

        void PrepararEliminacaoMPC()
        {
            transformacaoMPC = null;
            independentesMPC = null;
            if (!UsaEliminacaoMPC) return;
            var pivos = new Dictionary<int, Dictionary<int, double>>();
            var auxiliares = new HashSet<int>();
            for (int gl = nNos * 6 + 1; gl <= Ngl; gl++) auxiliares.Add(id[gl] - 1);
            foreach (var restricao in restricoesMPC)
            {
                var linha = restricao.Where(t => t.Value != 0 && !glRestrito[t.Key])
                    .ToDictionary(t => id[t.Key] - 1, t => t.Value);
                foreach (var pivo in pivos)
                {
                    double fator;
                    if (!linha.TryGetValue(pivo.Key, out fator)) continue;
                    linha.Remove(pivo.Key);
                    foreach (var t in pivo.Value) SomarTermoMPC(linha, t.Key, -fator * t.Value);
                }
                if (linha.Count == 0) continue; // Restricao redundante ou totalmente apoiada.
                // Elimina primeiro os mestres auxiliares sem massa.
                int chave = linha.Keys.OrderByDescending(k => auxiliares.Contains(k))
                    .ThenByDescending(k => Math.Abs(linha[k])).First();
                double divisor = linha[chave];
                linha.Remove(chave);
                foreach (int k in linha.Keys.ToArray()) linha[k] /= divisor;
                foreach (var anterior in pivos.Values)
                {
                    double fator;
                    if (!anterior.TryGetValue(chave, out fator)) continue;
                    anterior.Remove(chave);
                    foreach (var t in linha) SomarTermoMPC(anterior, t.Key, -fator * t.Value);
                }
                pivos.Add(chave, linha);
            }
            independentesMPC = Enumerable.Range(0, NLinhas).Where(i => !pivos.ContainsKey(i)).ToArray();
            var numero = independentesMPC.Select((gl, i) => new { gl, i }).ToDictionary(t => t.gl, t => t.i);
            transformacaoMPC = new Dictionary<int, double>[NLinhas];

            for (int i = 0; i < NLinhas; i++)
                transformacaoMPC[i] = pivos.ContainsKey(i) ? pivos[i].ToDictionary(t => numero[t.Key], t => -t.Value)
                    : new Dictionary<int, double> { { numero[i], 1 } };
        }

        static void SomarTermoMPC(Dictionary<int, double> linha, int chave, double valor)
        {
            double anterior;
            linha.TryGetValue(chave, out anterior);
            double soma = anterior + valor;
            if (Math.Abs(soma) <= 1e-12 * Math.Max(Math.Abs(anterior), Math.Abs(valor))) linha.Remove(chave);
            else linha[chave] = soma;
        }

        internal alglib.sparsematrix ReduzirMatrizMPC(alglib.sparsematrix matriz)
        {
            if (!UsaEliminacaoMPC) return matriz;
            // Graus internos das barras de flambagem permanecem independentes.
            int original = alglib.sparsegetnrows(matriz);
            int ordem = OrdemSistemaMPC + original - NLinhas;
            if (ordem == 0) return null;
            alglib.sparsematrix reduzida;
            alglib.sparsecreate(ordem, ordem, out reduzida);
            var mapa = new Dictionary<int, double>[original];
            for (int i = 0; i < original; i++)
                mapa[i] = i < NLinhas ? transformacaoMPC[i]
                    : new Dictionary<int, double> { { OrdemSistemaMPC + i - NLinhas, 1 } };
            int c1 = 0, c2 = 0, linha, coluna;
            double valor;
            while (alglib.sparseenumerate(matriz, ref c1, ref c2, out linha, out coluna, out valor))
                foreach (var a in mapa[linha])
                    foreach (var b in mapa[coluna])
                        if (a.Key <= b.Key)
                        {
                            double soma = alglib.sparseget(reduzida, a.Key, b.Key) + a.Value * valor * b.Value;
                            alglib.sparseset(reduzida, a.Key, b.Key, soma);
                            if (a.Key != b.Key) alglib.sparseset(reduzida, b.Key, a.Key, soma);
                        }
            alglib.sparseconverttocrs(reduzida);
            return reduzida;
        }

        internal double[,] ExpandirModosMPC(double[,] modos)
        {
            if (!UsaEliminacaoMPC || modos == null) return modos;
            int original = NLinhas + modos.GetLength(0) - OrdemSistemaMPC;
            var resultado = new double[original, modos.GetLength(1)];
            for (int i = 0; i < original; i++)
                for (int m = 0; m < modos.GetLength(1); m++)
                    if (i < NLinhas)
                        foreach (var t in transformacaoMPC[i]) resultado[i, m] += t.Value * modos[t.Key, m];
                    else resultado[i, m] = modos[OrdemSistemaMPC + i - NLinhas, m];
            return resultado;
        }

        internal double[] ReduzirInfluenciaMPC(double[] vetor)
        {
            return UsaEliminacaoMPC ? independentesMPC.Select(i => vetor[i]).ToArray() : vetor;
        }

        internal double[] ReduzirForcasMPC(double[] vetor)
        {
            if (!UsaEliminacaoMPC) return vetor;
            var reduzido = new double[OrdemSistemaMPC];
            for (int i = 0; i < NLinhas; i++)
                foreach (var t in transformacaoMPC[i]) reduzido[t.Key] += t.Value * vetor[i];
            return reduzido;
        }

        int ResolverEliminacaoMPC(double[] cargas, ref double[] deslocamentos)
        {
            var f = new double[OrdemSistemaMPC];
            for (int i = 0; i < NLinhas; i++)
                foreach (var t in transformacaoMPC[i]) f[t.Key] += t.Value * cargas[i + 1];
            deslocamentos = new double[NLinhas + 1];
            if (f.Length == 0) return 0;
            double[] q;
            alglib.sparsesolverreport relatorio;
            alglib.sparsespdsolvesks(MatrizRigidez.s, false, f, out q, out relatorio);
            if (relatorio.terminationtype <= 0)
                throw new InvalidOperationException("Falha na rigidez reduzida por MPC. Verifique apoios e mecanismos.");
            for (int i = 0; i < NLinhas; i++)
                foreach (var t in transformacaoMPC[i]) deslocamentos[i + 1] += t.Value * q[t.Key];
            return 0;
        }
    }
}
