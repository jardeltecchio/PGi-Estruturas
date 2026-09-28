using System;
using System.Collections.Generic;
using System.Diagnostics;

namespace PG
{
    // Iteracao simultanea com Rayleigh-Ritz e duas passagens de Gram-Schmidt.
    // O operador deve ser autoadjunto no produto interno fornecido.
    internal static class TIteracaoSubespaco
    {
        internal static void Resolver(int n, int modos, int limiteVetores, int limiteIteracoes,
            TimeSpan limiteTempo, Func<double[], double[]> aplicar,
            Func<double[], double[]> metrica,
            Func<List<double[]>, double[], double[,], bool> verificar,
            Action<int, int> progresso)
        {
            if (modos <= 0 || modos > n || limiteVetores < modos || limiteIteracoes <= 0 || limiteTempo <= TimeSpan.Zero)
                throw new ArgumentException("Dimensoes ou limites invalidos para iteracao por subespaco.");
            int maximo = Math.Min(n, limiteVetores);
            int tamanho = (int)Math.Min(maximo, Math.Max(2L * modos, modos + 8L));
            var tempo = Stopwatch.StartNew();
            var aleatorio = new Random(1729);
            Action prazo = () =>
            {
                if (tempo.Elapsed >= limiteTempo)
                    throw new InvalidOperationException("Iteracao por subespaco interrompida pelo limite de tempo.");
            };
            Func<double[]> random = () =>
            {
                var v = new double[n];
                for (int i = 0; i < n; i++) v[i] = aleatorio.NextDouble() - 0.5;
                return v;
            };
            var q = new List<double[]>();
            for (int j = 0; j < tamanho; j++) q.Add(random());
            for (int iteracao = 1; iteracao <= limiteIteracoes; iteracao++)
            {
                prazo();
                progresso?.Invoke(iteracao, tamanho);
                var baseNova = new List<double[]>();
                var baseMetrica = new List<double[]>();
                for (int j = 0; j < tamanho; j++)
                {
                    prazo();
                    var w = aplicar(q[j]);
                    // Reposicao em caso de nucleo do operador ou dependencia linear.
                    bool aceito = false;
                    for (int tentativa = 0; tentativa < 12 && !aceito; tentativa++)
                    {
                        if (tentativa > 0) w = random();
                        double escala = 0;
                        foreach (double valor in w) escala = Math.Max(escala, Math.Abs(valor));
                        if (!(escala > 0) || double.IsInfinity(escala)) continue;
                        for (int i = 0; i < n; i++) w[i] /= escala;
                        double inicial = Dot(w, metrica(w));
                        for (int passagem = 0; passagem < 2; passagem++)
                        {
                            var mw = metrica(w);
                            for (int k = 0; k < baseNova.Count; k++)
                            {
                                double coef = Dot(baseNova[k], mw);
                                for (int i = 0; i < n; i++)
                                {
                                    w[i] -= coef * baseNova[k][i];
                                    mw[i] -= coef * baseMetrica[k][i];
                                }
                            }
                        }
                        var produto = metrica(w);
                        double norma2 = Dot(w, produto);
                        if (!(norma2 > 1e-24 * Math.Max(inicial, 0))) continue;
                        double norma = Math.Sqrt(norma2);
                        for (int i = 0; i < n; i++) { w[i] /= norma; produto[i] /= norma; }
                        baseNova.Add(w); baseMetrica.Add(produto); aceito = true;
                    }
                    if (!aceito) throw new InvalidOperationException("Nao foi possivel completar a base do subespaco na metrica adotada.");
                }
                var h = new double[tamanho, tamanho];
                for (int j = 0; j < tamanho; j++)
                {
                    prazo();
                    var aq = aplicar(baseNova[j]);
                    for (int i = 0; i < tamanho; i++) h[i, j] = Dot(baseMetrica[i], aq);
                }
                for (int i = 0; i < tamanho; i++)
                    for (int j = i + 1; j < tamanho; j++)
                        h[i, j] = h[j, i] = 0.5 * h[i, j] + 0.5 * h[j, i];
                double[] valores;
                double[,] vetores;
                if (!alglib.smatrixevd(h, tamanho, 1, true, out valores, out vetores))
                    throw new InvalidOperationException("O problema de Rayleigh-Ritz nao convergiu.");
                prazo();
                bool convergiu = verificar(baseNova, valores, vetores);
                // A flambagem admite autovalores negativos. A verificacao exige
                // a quantidade pedida de modos positivos e seus residuos completos.
                if (convergiu) return;
                double limiar = 64 * 2.2204460492503131e-16 * tamanho *
                    Math.Max(Math.Abs(valores[0]), Math.Abs(valores[tamanho - 1]));
                int positivos = 0;
                foreach (double valor in valores) if (valor > limiar) positivos++;
                q = new List<double[]>();
                for (int j = 0; j < tamanho; j++)
                {
                    var v = new double[n];
                    for (int k = 0; k < tamanho; k++)
                        for (int i = 0; i < n; i++) v[i] += baseNova[k][i] * vetores[k, tamanho - 1 - j];
                    q.Add(v);
                }
                if (positivos < modos || iteracao % 20 == 0)
                {
                    int novo = (int)Math.Min(maximo, 2L * tamanho);
                    // No limite de vetores, continua refinando a base existente.
                    while (q.Count < novo) q.Add(random());
                    tamanho = novo;
                }
            }
            throw new InvalidOperationException("Iteracao por subespaco atingiu o limite de iteracoes sem convergencia.");
        }

        private static double Dot(double[] a, double[] b)
        {
            double soma = 0, correcao = 0;
            for (int i = 0; i < a.Length; i++)
            {
                double parcela = a[i] * b[i] - correcao;
                double total = soma + parcela;
                correcao = (total - soma) - parcela;
                soma = total;
            }
            if (double.IsNaN(soma) || double.IsInfinity(soma)) throw new InvalidOperationException("Produto nao finito na iteracao por subespaco.");
            return soma;
        }
    }
}
