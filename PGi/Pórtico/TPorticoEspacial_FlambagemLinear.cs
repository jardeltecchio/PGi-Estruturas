using System;

namespace PG
{
    public class TPorticoEspacial_FlambagemLinear
    {
        private readonly TPorticoEspacial portico;
        private double[] fatoresTirantesReferencia;
        public const double ToleranciaConvergencia = 1e-4;
        public event Action<string> ProgressoAlterado;
        public int LimiteVetoresLanczos { get; set; }
        public TimeSpan LimiteTempoLanczos { get; set; } = TimeSpan.FromSeconds(60);
        public System.Collections.Generic.List<int> VetoresPorTentativa { get; } = new System.Collections.Generic.List<int>();
        public System.Collections.Generic.List<double[]> ResiduosPorTentativa { get; } = new System.Collections.Generic.List<double[]>();

        /// <summary>Executa Lanczos adaptativo sobre as matrizes ja preparadas.</summary>
        public void CalcularModos(double[] vetorInicial = null)
        {
            InvalidarLanczos();

            if (!RigidezFatorada) 
                throw new InvalidOperationException("Prepare a rigidez antes de calcular os modos.");
            
            if (LimiteVetoresLanczos < 0 || LimiteTempoLanczos <= TimeSpan.Zero)
                throw new ArgumentOutOfRangeException("Limites de Lanczos invalidos.");
           
            int limite = (int)Math.Min(ReduzidoParaExpandido.Length, LimiteVetoresLanczos == 0 ? Math.Max(300L, 20L * NumeroModos) : LimiteVetoresLanczos);

            ConstruirBaseLanczosInterna(limite, vetorInicial, true);
        }
        private alglib.sparsematrix fatorCholesky;
        public alglib.sparsematrix RigidezReduzida { get; private set; }
        public alglib.sparsematrix GeometricaReduzida { get; private set; }
        // Mapas base zero entre a montagem expandida e o sistema de solucao.
        // -1 no primeiro mapa identifica um grau desconectado removido.
        public int[] ExpandidoParaReduzido { get; private set; }
        public int[] ReduzidoParaExpandido { get; private set; }
        public bool RigidezFatorada => fatorCholesky != null;
        public double ResiduoValidacaoRigidez { get; private set; } = double.NaN;
        // Vetores na numeracao reduzida, normalizados por q^T K q = 1.
        public System.Collections.Generic.IReadOnlyList<double[]> BaseLanczos { get; private set; }
        public double[] DiagonalLanczos { get; private set; }
        public double[] SubdiagonalLanczos { get; private set; }
        public double BetaFinalLanczos { get; private set; }
        // Encerramento da recorrencia; nao significa convergencia dos modos solicitados.
        public bool SubespacoLanczosEncerrado { get; private set; }
        // Aproximacoes de Ritz: mu decrescente, lambda=1/mu crescente.
        // Ainda dependem da recuperacao e verificacao do residuo no sistema completo.
        public double[] AutovaloresReduzidos { get; private set; }
        public double[,] AutovetoresReduzidos { get; private set; }
        public double[] MultiplicadoresCriticos { get; private set; }
        // Colunas = modos; normalizacao phi^T K phi = 1.
        public double[,] ModosReduzidos { get; private set; }
        // Linhas na numeracao expandida, incluindo rotacoes internas das ligacoes.
        // Graus desconectados removidos sao preenchidos com zero.
        public double[,] ModosFlambagem { get; private set; }
        public double[] ResiduosRelativos { get; private set; }
        public bool[] ModosConvergidos { get; private set; }
        public bool Convergiu { get; private set; }

        private void InvalidarModos()
        {
            ModosReduzidos = null;
            ModosFlambagem = null;
            ResiduosRelativos = null;
            ModosConvergidos = null;
            Convergiu = false;
        }

        /// <summary>
        /// Recupera phi=Q*y e verifica o problema original nas matrizes expandidas.
        /// Nao aumenta a base automaticamente; Convergiu exige todos os modos pedidos.
        /// </summary>
        public void RecuperarModosEVerificarConvergencia(double tolerancia = 1e-4)
        {
            InvalidarModos();
            if (!(tolerancia > 0 && tolerancia < 1))
                throw new ArgumentOutOfRangeException(nameof(tolerancia));
            if (!RigidezFatorada || BaseLanczos == null || MultiplicadoresCriticos == null ||
                AutovetoresReduzidos == null)
                throw new InvalidOperationException("Resolva o problema reduzido antes de recuperar os modos.");
            int quantidade = MultiplicadoresCriticos.Length;
            int n = ReduzidoParaExpandido.Length;
            int m = BaseLanczos.Count;
            if (AutovetoresReduzidos.GetLength(0) != m || AutovetoresReduzidos.GetLength(1) != quantidade)
                throw new InvalidOperationException("Dimensoes dos autovetores reduzidos incompativeis.");
            var reduzidos = new double[n, quantidade];
            var expandidos = new double[NumeroGrausLiberdade, quantidade];
            var residuos = new double[quantidade];
            var convergidos = new bool[quantidade];
            bool todos = quantidade == NumeroModos;
            for (int modo = 0; modo < quantidade; modo++)
            {
                var phi = new double[n];
                for (int j = 0; j < m; j++)
                {
                    if (BaseLanczos[j].Length != n)
                        throw new InvalidOperationException("Dimensao da base de Lanczos incompativel.");
                    double coeficiente = AutovetoresReduzidos[j, modo];
                    for (int i = 0; i < n; i++) phi[i] += BaseLanczos[j][i] * coeficiente;
                }
                double norma = Math.Sqrt(ProdutoInternoRigidez(phi, phi));
                if (!(norma > 0) || double.IsInfinity(norma))
                    throw new InvalidOperationException("Norma invalida na recuperacao do modo.");
                int maior = 0;
                for (int i = 1; i < n; i++)
                    if (Math.Abs(phi[i]) > Math.Abs(phi[maior])) maior = i;
                // Convencao de sinal para resultados reproduziveis.
                if (phi[maior] < 0) norma = -norma;
                var expandido = new double[NumeroGrausLiberdade];
                for (int i = 0; i < n; i++)
                {
                    phi[i] /= norma;
                    reduzidos[i, modo] = phi[i];
                    int gl = ReduzidoParaExpandido[i];
                    expandido[gl] = phi[i];
                    expandidos[gl, modo] = phi[i];
                }
                var kPhi = new double[NumeroGrausLiberdade];
                var gPhi = new double[NumeroGrausLiberdade];
                alglib.sparsemv(MatrizRigidez, expandido, ref kPhi);
                alglib.sparsemv(MatrizGeometrica, expandido, ref gPhi);
                double lambda = MultiplicadoresCriticos[modo];
                if (!(lambda > 0) || double.IsInfinity(lambda))
                    throw new InvalidOperationException("Multiplicador invalido na recuperacao.");
                double escala = 0;
                for (int i = 0; i < kPhi.Length; i++)
                {
                    gPhi[i] *= lambda;
                    if (double.IsNaN(kPhi[i]) || double.IsInfinity(kPhi[i]) ||
                        double.IsNaN(gPhi[i]) || double.IsInfinity(gPhi[i]))
                        throw new InvalidOperationException("Residuo modal nao finito.");
                    escala = Math.Max(escala, Math.Max(Math.Abs(kPhi[i]), Math.Abs(gPhi[i])));
                }
                if (escala == 0) throw new InvalidOperationException("Modo sem rigidez na verificacao do residuo.");
                double erro = 0, normaK = 0, normaG = 0;
                for (int i = 0; i < kPhi.Length; i++)
                {
                    double a = kPhi[i] / escala, b = gPhi[i] / escala;
                    erro += (a + b) * (a + b); normaK += a * a; normaG += b * b;
                }
                residuos[modo] = Math.Sqrt(erro) / (Math.Sqrt(normaK) + Math.Sqrt(normaG));
                convergidos[modo] = residuos[modo] <= tolerancia;
                todos &= convergidos[modo];
            }
            ModosReduzidos = reduzidos;
            ModosFlambagem = expandidos;
            ResiduosRelativos = residuos;
            ModosConvergidos = convergidos;
            Convergiu = todos;
        }
        public bool QuantidadePositivaSuficiente => MultiplicadoresCriticos != null &&
            MultiplicadoresCriticos.Length == NumeroModos;

        private void InvalidarProblemaReduzido()
        {
            InvalidarModos();
            AutovaloresReduzidos = null;
            AutovetoresReduzidos = null;
            MultiplicadoresCriticos = null;
        }

        /// <summary>
        /// Resolve T*y=mu*y e seleciona ate NumeroModos multiplicadores positivos.
        /// Poucos positivos na base atual nao comprovam ausencia de outros modos.
        /// </summary>
        public void ResolverProblemaReduzido()
        {
            InvalidarProblemaReduzido();
            if (BaseLanczos == null || DiagonalLanczos == null || DiagonalLanczos.Length == 0 ||
                BaseLanczos.Count != DiagonalLanczos.Length || SubdiagonalLanczos == null ||
                SubdiagonalLanczos.Length != DiagonalLanczos.Length - 1)
                throw new InvalidOperationException("Construa a base de Lanczos antes de resolver o problema reduzido.");
            int n = DiagonalLanczos.Length;
            var diagonal = (double[])DiagonalLanczos.Clone();
            var subdiagonal = (double[])SubdiagonalLanczos.Clone();
            double escala = 0;
            foreach (double valor in diagonal)
            {
                if (double.IsNaN(valor) || double.IsInfinity(valor))
                    throw new InvalidOperationException("Diagonal de Lanczos nao finita.");
                escala = Math.Max(escala, Math.Abs(valor));
            }
            foreach (double valor in subdiagonal)
            {
                if (double.IsNaN(valor) || double.IsInfinity(valor))
                    throw new InvalidOperationException("Subdiagonal de Lanczos nao finita.");
                escala = Math.Max(escala, Math.Abs(valor));
            }
            // Escalar T melhora a robustez quando as unidades mudam muito seu tamanho.
            if (escala > 0)
            {
                for (int i = 0; i < n; i++) diagonal[i] /= escala;
                for (int i = 0; i < n - 1; i++) subdiagonal[i] /= escala;
            }
            int solicitados = Math.Min(NumeroModos, n);
            double[,] vetores = new double[0, 0];
            if (!alglib.smatrixtdevdi(ref diagonal, subdiagonal, n, 2,
                n - solicitados, n - 1, ref vetores))
                throw new InvalidOperationException("O solver da matriz tridiagonal nao convergiu.");
            // Autovalores proximos de zero nao podem ser invertidos com confianca.
            double limiar = 64 * 2.2204460492503131e-16 * n;
            int quantidade = 0;
            for (int i = solicitados - 1; i >= 0; i--)
            {
                if (double.IsNaN(diagonal[i]) || double.IsInfinity(diagonal[i]))
                    throw new InvalidOperationException("Autovalor reduzido nao finito.");
                if (escala > 0 && diagonal[i] > limiar) quantidade++;
            }
            var valores = new double[quantidade];
            var lambdas = new double[quantidade];
            var modos = new double[n, quantidade];
            for (int modo = 0; modo < quantidade; modo++)
            {
                int origem = solicitados - 1 - modo;
                double mu = diagonal[origem] * escala;
                double lambda = 1.0 / mu;
                if (!(mu > 0) || double.IsInfinity(mu) || !(lambda > 0) || double.IsInfinity(lambda))
                    throw new InvalidOperationException("Multiplicador fora da faixa numerica representavel.");
                valores[modo] = mu;
                lambdas[modo] = lambda;
                for (int i = 0; i < n; i++)
                {
                    double valor = vetores[i, origem];
                    if (double.IsNaN(valor) || double.IsInfinity(valor))
                        throw new InvalidOperationException("Autovetor reduzido nao finito.");
                    modos[i, modo] = valor;
                }
            }

            AutovaloresReduzidos = valores;
            AutovetoresReduzidos = modos;
            MultiplicadoresCriticos = lambdas;

        }

        private void InvalidarLanczos()
        {
            VetoresPorTentativa.Clear();
            ResiduosPorTentativa.Clear();
            InvalidarProblemaReduzido();
            BaseLanczos = null;
            DiagonalLanczos = null;
            SubdiagonalLanczos = null;
            BetaFinalLanczos = double.NaN;
            SubespacoLanczosEncerrado = false;
        }

        /// <summary>
        /// Constroi ate maximoVetores da base de Lanczos de K^-1*(-Kg).
        /// Duas passagens de reortogonalizacao completa na metrica K.
        /// Esta etapa nao calcula autovalores nem declara convergencia modal.
        /// </summary>
        public void ConstruirBaseLanczos(int maximoVetores, double[] vetorInicial = null)
        {
            ConstruirBaseLanczosInterna(maximoVetores, vetorInicial, false);
        }

        private void ConstruirBaseLanczosInterna(int maximoVetores, double[] vetorInicial, bool adaptativo)
        {
            InvalidarLanczos();
            if (!RigidezFatorada) throw new InvalidOperationException("Prepare a rigidez antes do Lanczos.");
            if (maximoVetores <= 0) throw new ArgumentOutOfRangeException(nameof(maximoVetores));
            int n = ReduzidoParaExpandido.Length;
            int limite = Math.Min(maximoVetores, n);
            double[] q;
           
            if (vetorInicial != null)
            {
                ValidarVetorOperador(vetorInicial, nameof(vetorInicial));
                q = (double[])vetorInicial.Clone();
            }
            else
            {
                var random = new Random(1729);
                q = new double[n];
                for (int i = 0; i < n; i++) q[i] = random.NextDouble() - 0.5;
            }

            double escala = 0;
            foreach (double v in q) 
                escala = Math.Max(escala, Math.Abs(v));

            if (escala == 0) 
                throw new ArgumentException("Vetor inicial nulo.", nameof(vetorInicial));
           
            for (int i = 0; i < n; i++) 
                q[i] /= escala;
           
            double[] kq = MultiplicarRigidezLanczos(q);
            double norma = NormaRigidezLanczos(q, kq);
            if (norma == 0) 
                throw new InvalidOperationException("Norma inicial nula na metrica K.");
           
            for (int i = 0; i < n; i++) 
            { 
                q[i] /= norma; 
                kq[i] /= norma; 
            }

            var baseLocal = new System.Collections.Generic.List<double[]>();
            var baseK = new System.Collections.Generic.List<double[]>();
            var diagonal = new System.Collections.Generic.List<double>();
            var subdiagonal = new System.Collections.Generic.List<double>();
            double beta = 0;
            bool encerrado = false;
            int proximaVerificacao = (int)Math.Min(limite, Math.Max(NumeroModos + 8L, 2L * NumeroModos));
            var cronometro = System.Diagnostics.Stopwatch.StartNew();
            for (int passo = 0; passo < limite; passo++)
            {
                ProgressoAlterado?.Invoke($"Lanczos: vetor {passo + 1} de até {limite} — tentativa de convergência {VetoresPorTentativa.Count + 1}");
                if (adaptativo && cronometro.Elapsed >= LimiteTempoLanczos)
                    throw new InvalidOperationException("Lanczos interrompido pelo limite de tempo; modos ainda nao convergidos.");
                baseLocal.Add(q);
                baseK.Add(kq);
                double[] w = AplicarOperadorFlambagem(q);
                double[] kw = MultiplicarRigidezLanczos(w);
                double normaOperador = NormaRigidezLanczos(w, kw);
                double alfa = ProdutoLanczos(q, kw);
                diagonal.Add(alfa);
                for (int i = 0; i < n; i++)
                {
                    w[i] -= alfa * q[i];
                    if (passo > 0) 
                        w[i] -= beta * baseLocal[passo - 1][i];
                }
                // Kq em cache evita um produto esparso para cada projecao.
                // Recalcula Kw entre passagens para limitar cancelamento acumulado.
                for (int passagem = 0; passagem < 2; passagem++)
                {
                    kw = MultiplicarRigidezLanczos(w);
                    for (int j = 0; j < baseLocal.Count; j++)
                    {
                        double projecao = ProdutoLanczos(baseLocal[j], kw);
                        for (int i = 0; i < n; i++)
                        {
                            w[i] -= projecao * baseLocal[j][i];
                            kw[i] -= projecao * baseK[j][i];
                        }
                    }
                }
                kw = MultiplicarRigidezLanczos(w);
                double proximoBeta = NormaRigidezLanczos(w, kw);
                // Limiar relativo de arredondamento, independente da escala de Kg.
                double escalaRecorrencia = Math.Max(normaOperador, Math.Max(Math.Abs(alfa), beta));
                encerrado = proximoBeta == 0 || proximoBeta <= 64 * 2.2204460492503131e-16 * escalaRecorrencia;
                beta = proximoBeta;
                if (adaptativo && (passo + 1 >= proximaVerificacao || encerrado || passo + 1 == limite))
                {
                    BaseLanczos = baseLocal.AsReadOnly();
                    DiagonalLanczos = diagonal.ToArray();
                    SubdiagonalLanczos = subdiagonal.ToArray();
                    BetaFinalLanczos = beta;
                    SubespacoLanczosEncerrado = encerrado;
                    ResolverProblemaReduzido();
                    ProgressoAlterado?.Invoke("Recuperação");
                    RecuperarModosEVerificarConvergencia(ToleranciaConvergencia);
                    VetoresPorTentativa.Add(baseLocal.Count);
                    ResiduosPorTentativa.Add((double[])ResiduosRelativos.Clone());
                    if (Convergiu) return;
                    if (encerrado || passo + 1 == limite)
                        throw new InvalidOperationException(
                            $"Flambagem sem convergencia dos {NumeroModos} modos com {baseLocal.Count} vetores " +
                            $"(tolerancia {ToleranciaConvergencia:E3}). " +
                            (encerrado ? "A recorrencia encerrou em um subespaco; pode ser necessario outro vetor inicial ou tratamento de multiplicidades. " : "Limite de vetores atingido. ") +
                            "Consulte ResiduosPorTentativa.");
                    proximaVerificacao = (int)Math.Min(limite, 2L * proximaVerificacao);
                }
                if (encerrado || passo + 1 == limite) break;
                subdiagonal.Add(beta);
                q = w; kq = kw;
                for (int i = 0; i < n; i++) { q[i] /= beta; kq[i] /= beta; }
            }
            BaseLanczos = baseLocal.AsReadOnly();
            DiagonalLanczos = diagonal.ToArray();
            SubdiagonalLanczos = subdiagonal.ToArray();
            BetaFinalLanczos = beta;
            SubespacoLanczosEncerrado = encerrado;
        }

        private double[] MultiplicarRigidezLanczos(double[] v)
        {
            double[] resultado = new double[v.Length];
            alglib.sparsemv(RigidezReduzida, v, ref resultado);
            return resultado;
        }

        private static double ProdutoLanczos(double[] u, double[] v)
        {
            double soma = 0, compensacao = 0;
            for (int i = 0; i < u.Length; i++)
            {
                double parcela = u[i] * v[i] - compensacao;
                double total = soma + parcela;
                compensacao = (total - soma) - parcela;
                soma = total;
            }
            if (double.IsNaN(soma) || double.IsInfinity(soma))
                throw new InvalidOperationException("Produto nao finito durante o Lanczos.");
            return soma;
        }

        private static double NormaRigidezLanczos(double[] v, double[] kv)
        {
            double quadrado = ProdutoLanczos(v, kv);
            if (quadrado < 0)
                throw new InvalidOperationException("Norma negativa na metrica K; verifique o condicionamento da rigidez.");
            return Math.Sqrt(quadrado);
        }

        private void InvalidarFatoracao()
        {
            InvalidarLanczos();
            fatorCholesky = null;
            RigidezReduzida = null;
            GeometricaReduzida = null;
            ExpandidoParaReduzido = null;
            ReduzidoParaExpandido = null;
            ResiduoValidacaoRigidez = double.NaN;
        }

        /// <summary>Verifica, remove graus vazios, fatora e valida Kx=b.</summary>
        public void PrepararRigidez()
        {
            InvalidarFatoracao();
            int n = NumeroGrausLiberdade;
            var ocupados = new bool[n];
            VerificarMatriz(MatrizRigidez, n, ocupados, "K");
            VerificarMatriz(MatrizGeometrica, n, ocupados, "Kg");
            var mapa = new int[n];
            var inverso = new System.Collections.Generic.List<int>();
            for (int i = 0; i < n; i++)
            {
                mapa[i] = ocupados[i] ? inverso.Count : -1;
                if (ocupados[i]) inverso.Add(i);
            }
            if (inverso.Count == 0)
                throw new InvalidOperationException("Todas as equacoes estao desconectadas.");
            var k = ReduzirMatriz(MatrizRigidez, mapa, inverso.Count);
            var g = ReduzirMatriz(MatrizGeometrica, mapa, inverso.Count);
            for (int i = 0; i < inverso.Count; i++)
                if (alglib.sparseget(k, i, i) <= 0)
                    throw new InvalidOperationException($"Rigidez nao positiva no grau expandido {inverso[i] + 1}. Verifique mecanismos e vinculos.");
            alglib.sparsematrix copia;
            alglib.sparsecopy(k, out copia);
            alglib.sparsedecompositionanalysis analise;
            if (!alglib.sparsecholeskyanalyze(copia, false, 0, -1, out analise))
                throw new InvalidOperationException("Falha na analise simbolica de Cholesky.");
            alglib.sparsematrix fator;
            double[] diagonal;
            int[] permutacao;
            if (!alglib.sparsecholeskyfactorize(analise, false, out fator, out diagonal, out permutacao))
                throw new InvalidOperationException("K nao e positiva definida. Verifique mecanismos, vinculos ou condicionamento da estrutura.");

            // Vetor conhecido, nao uniforme: b=K*x. A verificacao usa K original,
            // nunca a matriz sobrescrita pela fatoracao.
            var conhecido = new double[inverso.Count];
            for (int i = 0; i < conhecido.Length; i++) conhecido[i] = 1.0 + (i % 7) * 0.125;
            double[] b = new double[conhecido.Length];
            alglib.sparsemv(k, conhecido, ref b);
            var x = ResolverFator(fator, b);
            double residuo = CalcularResiduo(k, x, b);
            if (double.IsNaN(residuo) || double.IsInfinity(residuo) || residuo > 1e-10)
                throw new InvalidOperationException($"Validacao de Kx=b falhou: residuo relativo {residuo:E3}.");
            RigidezReduzida = k;
            GeometricaReduzida = g;
            ExpandidoParaReduzido = mapa;
            ReduzidoParaExpandido = inverso.ToArray();
            ResiduoValidacaoRigidez = residuo;
            fatorCholesky = fator;
        }

        private static void VerificarMatriz(alglib.sparsematrix matriz, int n, bool[] ocupados, string nome)
        {
            if (matriz == null || n <= 0 || alglib.sparsegetnrows(matriz) != n || alglib.sparsegetncols(matriz) != n)
                throw new InvalidOperationException($"Matriz {nome} ausente ou com dimensao invalida.");
            int c1 = 0, c2 = 0, i, j;
            double v;
            while (alglib.sparseenumerate(matriz, ref c1, ref c2, out i, out j, out v))
            {
                double t = alglib.sparseget(matriz, j, i);
                if (double.IsNaN(v) || double.IsInfinity(v) || double.IsNaN(t) || double.IsInfinity(t))
                    throw new InvalidOperationException($"Valor nao finito em {nome}[{i + 1},{j + 1}].");
                double escala = Math.Max(Math.Abs(v), Math.Abs(t));
                if (escala > 0 && Math.Abs(v / escala - t / escala) > 1e-12)
                    throw new InvalidOperationException($"Matriz {nome} assimetrica em [{i + 1},{j + 1}].");
                // Zero exato: nao elimina rigidezes pequenas de tirantes ou molas.
                if (v != 0) { ocupados[i] = true; ocupados[j] = true; }
            }
        }

        private static alglib.sparsematrix ReduzirMatriz(alglib.sparsematrix origem, int[] mapa, int n)
        {
            alglib.sparsematrix destino;
            alglib.sparsecreate(n, n, out destino);
            int c1 = 0, c2 = 0, i, j;
            double v;
            while (alglib.sparseenumerate(origem, ref c1, ref c2, out i, out j, out v))
                if (mapa[i] >= 0 && mapa[j] >= 0 && v != 0)
                    alglib.sparseset(destino, mapa[i], mapa[j], v);
            alglib.sparseconverttocrs(destino);
            return destino;
        }

        /// <summary>Resolve na numeracao reduzida, reutilizando a fatoracao.</summary>
        public double[] ResolverComRigidez(double[] b)
        {
            if (!RigidezFatorada) throw new InvalidOperationException("Prepare a rigidez antes da solucao.");
            if (b == null || b.Length != ReduzidoParaExpandido.Length)
                throw new ArgumentException("Dimensao do vetor incompativel com a rigidez reduzida.", nameof(b));
            return ResolverFator(fatorCholesky, b);
        }

        /// <summary>
        /// Aplica A*v = K^-1*(-Kg)*v na numeracao reduzida.
        /// Nao forma a inversa nem refatora K. Para A*phi=mu*phi, lambda=1/mu.
        /// </summary>
        public double[] AplicarOperadorFlambagem(double[] vetor)
        {
            ValidarVetorOperador(vetor, nameof(vetor));
            double[] ladoDireito = new double[vetor.Length];
            alglib.sparsemv(GeometricaReduzida, vetor, ref ladoDireito);
            for (int i = 0; i < ladoDireito.Length; i++) ladoDireito[i] = -ladoDireito[i];
            return ResolverComRigidez(ladoDireito);
        }

        /// <summary>Produto interno u^T K v para a ortogonalizacao do Lanczos.</summary>
        public double ProdutoInternoRigidez(double[] u, double[] v)
        {
            ValidarVetorOperador(u, nameof(u));
            ValidarVetorOperador(v, nameof(v));
            double[] kv = new double[v.Length];
            alglib.sparsemv(RigidezReduzida, v, ref kv);
            double soma = 0, compensacao = 0;
            for (int i = 0; i < u.Length; i++)
            {
                double parcela = u[i] * kv[i] - compensacao;
                double novaSoma = soma + parcela;
                compensacao = (novaSoma - soma) - parcela;
                soma = novaSoma;
            }
            if (double.IsNaN(soma) || double.IsInfinity(soma))
                throw new InvalidOperationException("Produto interno de rigidez nao finito.");
            return soma;
        }

        private void ValidarVetorOperador(double[] vetor, string nome)
        {
            if (!RigidezFatorada)
                throw new InvalidOperationException("Prepare a rigidez antes de aplicar o operador de flambagem.");
            if (vetor == null || vetor.Length != ReduzidoParaExpandido.Length)
                throw new ArgumentException("O vetor deve usar a numeracao reduzida da flambagem.", nome);
            foreach (double valor in vetor)
                if (double.IsNaN(valor) || double.IsInfinity(valor))
                    throw new ArgumentException("O vetor possui valor nao finito.", nome);
        }

        private static double[] ResolverFator(alglib.sparsematrix fator, double[] b)
        {
            foreach (double v in b)
                if (double.IsNaN(v) || double.IsInfinity(v)) throw new ArgumentException("Vetor nao finito.");
            double[] x;
            alglib.sparsesolverreport relatorio;
            alglib.sparsespdcholeskysolve(fator, false, b, out x, out relatorio);
            if (relatorio == null || relatorio.terminationtype <= 0)
                throw new InvalidOperationException("Falha na solucao com Cholesky.");
            foreach (double v in x)
                if (double.IsNaN(v) || double.IsInfinity(v)) throw new InvalidOperationException("Solucao nao finita.");
            return x;
        }

        private static double CalcularResiduo(alglib.sparsematrix k, double[] x, double[] b)
        {
            double[] produto = new double[b.Length];
            alglib.sparsemv(k, x, ref produto);
            double escala = 0;
            for (int i = 0; i < b.Length; i++) escala = Math.Max(escala, Math.Max(Math.Abs(produto[i]), Math.Abs(b[i])));
            if (escala == 0) return 0;
            double erro = 0, normaB = 0, normaProduto = 0;
            for (int i = 0; i < b.Length; i++)
            {
                double p = produto[i] / escala, q = b[i] / escala;
                erro += (p - q) * (p - q); normaB += q * q; normaProduto += p * p;
            }
            return Math.Sqrt(erro) / (Math.Sqrt(normaB) + Math.Sqrt(normaProduto));
        }

        public int NumeroModos { get; }
        public int IdReferencia { get; private set; }
        public bool ReferenciaEhCombinacao { get; private set; }
        public bool ReferenciaPreparada { get; private set; }
        public alglib.sparsematrix MatrizGeometrica { get; private set; }
        public alglib.sparsematrix MatrizRigidez { get; private set; }
        public int NumeroGrausLiberdade { get; private set; }
        // Equacoes base zero; -1 indica rotacao rigidamente conectada.
        // Colunas: ry inicial, rz inicial, ry final, rz final (GL 5,6,11,12).
        public int[,] GrausInternosBarras { get; private set; }

        public void MontarMatrizes()
        {
            InvalidarFatoracao();
            MatrizRigidez = null;
            MatrizGeometrica = null;
            GrausInternosBarras = null;
            NumeroGrausLiberdade = 0;
            if (!ReferenciaPreparada || portico.id == null || portico.glRestrito == null)
                throw new InvalidOperationException("Prepare a referencia e a numeracao dos graus livres antes da montagem.");
            
            int ordem = portico.NLinhas;
            var internos = new int[portico.nBarras + 1, 4];
            for (int b = 1; b <= portico.nBarras; b++)
            {
                var barra = portico.barras[b];
                bool[] libera = { barra.articulacao_mz_ini, barra.articulacao_my_ini,
                    barra.articulacao_mz_fin, barra.articulacao_my_fin };
               
                for (int r = 0; r < 4; r++) 
                    internos[b, r] = libera[r] ? ordem++ : -1;
            }

            if (ordem <= 0) throw new InvalidOperationException("Nao existem graus livres.");
            alglib.sparsematrix ke, kg;
            alglib.sparsecreate(ordem, ordem, out ke);
            alglib.sparsecreate(ordem, ordem, out kg);
            int[] rotacoes = { 5, 6, 11, 12 };
            for (int b = 1; b <= portico.nBarras; b++)
            {
                var barra = portico.barras[b];
               
                if (barra.SomenteTracao && fatoresTirantesReferencia == null)
                    throw new InvalidOperationException("Prepare novamente a referencia apos alterar os tirantes.");
                
                double fator = barra.SomenteTracao ? fatoresTirantesReferencia[b] : 1.0;
              
                if (!(barra.L > 0) || !(barra.A1 > 0) || barra.MatrizRotacao == null || barra.GlGlobal == null)
                    throw new InvalidOperationException($"Geometria ou numeracao incompleta na barra {b}.");
              
                var elastica = barra.CriarMatrizElasticaFlambagem(fator);
                // Mantem a regularizacao elastica da estatica, mas um tirante
                // inativo nao fornece rigidez geometrica de compressao.
                // O criterio estatico de ativacao admite pequeno encurtamento;
                // mesmo ativo, o tirante nao pode contribuir com P negativo em Kg.
                double normal = EsforcosNormaisReferencia[b];
                if (barra.SomenteTracao) normal = fator < 1.0 ? 0.0 : Math.Max(0.0, normal);
                barra.SetMatrizGeometrica_2(normal);
                // q_barra = H q_global. As rotacoes internas sao escalares locais;
                // apenas os graus nodais recebem a rotacao R e o offset O.
                var h = new double[13, 17];
                var mapa = new int[17];

                for (int c = 1; c <= 16; c++) 
                    mapa[c] = -1;

                for (int i = 1; i <= 12; i++)
                {
                    int gl = barra.GlGlobal[i];
                    mapa[i] = portico.glRestrito[gl] ? -1 : portico.id[gl] - 1;

                    if (!portico.glRestrito[gl] && (mapa[i] < 0 || mapa[i] >= portico.NLinhas))
                        throw new InvalidOperationException("Numeracao dos graus livres invalida.");
                    
                    for (int j = 1; j <= 12; j++)
                    {
                        if (!barra.temOffset) h[i, j] = barra.MatrizRotacao[i, j];
                        else
                        {
                            if (barra.MatrizOffset == null) throw new InvalidOperationException("Offset ausente.");
                            for (int k = 1; k <= 12; k++)
                                h[i, j] += barra.MatrizOffset[i, k] * barra.MatrizRotacao[k, j];
                        }
                    }
                }

                double[] molas = { barra.KMz_Inicio, barra.KMy_Inicio, barra.KMz_Final, barra.KMy_Final };
               
                for (int r = 0; r < 4; r++)
                {
                    if (internos[b, r] < 0) 
                        continue;
                    int gl = rotacoes[r], interno = 13 + r;
                    mapa[interno] = internos[b, r];

                    // Energia da mola: k/2 * (theta_no - theta_barra)^2.
                    var diferenca = new double[17];
                    for (int j = 1; j <= 12; j++)
                    {
                        diferenca[j] = h[gl, j];
                        h[gl, j] = 0;
                    }

                    diferenca[interno] = -1;
                    h[gl, interno] = 1;
                    if (molas[r] < 0 || double.IsNaN(molas[r]) || double.IsInfinity(molas[r]))
                        throw new InvalidOperationException($"Rigidez de ligacao invalida na barra {b}.");

                    for (int i = 1; i <= 16; i++)
                        for (int j = 1; j <= 16; j++)
                            Somar(ke, mapa[i], mapa[j], molas[r] * diferenca[i] * diferenca[j]);
                }
                MontarContribuicao(ke, elastica, h, mapa);
                MontarContribuicao(kg, barra.MatrizGeometrica, h, mapa);
            }
            // Molas de apoio: mesma ordem global DX,DZ,DY,RX,RZ,RY do portico.
            for (int n = 1; n <= portico.nNos; n++)
            {
                var no = portico.nos[n];
                bool[] possui = { no.PossuiMolaDX, no.PossuiMolaDZ, no.PossuiMolaDY,
                    no.PossuiMolaRX, no.PossuiMolaRZ, no.PossuiMolaRY };
                double[] valores = { no.K_Mola_DX, no.K_Mola_DZ, no.K_Mola_DY,
                    no.K_Mola_RX, no.K_Mola_RZ, no.K_Mola_RY };
                for (int d = 0; d < 6; d++)
                {
                    int gl = (no.Numero - 1) * 6 + d + 1;
                    if (possui[d] && !portico.glRestrito[gl])
                        Somar(ke, portico.id[gl] - 1, portico.id[gl] - 1, valores[d]);
                }
            }
            alglib.sparseconverttocrs(ke);
            alglib.sparseconverttocrs(kg);
            MatrizRigidez = ke;
            MatrizGeometrica = kg;
            GrausInternosBarras = internos;
            NumeroGrausLiberdade = ordem;
        }

        private static void Somar(alglib.sparsematrix matriz, int i, int j, double valor)
        {
            if (double.IsNaN(valor) || double.IsInfinity(valor))
                throw new InvalidOperationException("Coeficiente nao finito na montagem de flambagem.");
            if (i < 0 || j < 0 || valor == 0) return;
            alglib.sparseset(matriz, i, j, alglib.sparseget(matriz, i, j) + valor);
        }

        private static void MontarContribuicao(alglib.sparsematrix global, double[,] local, double[,] h, int[] mapa)
        {
            var produto = new double[13, 17];
            for (int i = 1; i <= 12; i++)
                for (int j = 1; j <= 16; j++)
                    for (int k = 1; k <= 12; k++) produto[i, j] += local[i, k] * h[k, j];

            for (int i = 1; i <= 16; i++)
                for (int j = i; j <= 16; j++)
                {
                    double valor = 0;

                    for (int k = 1; k <= 12; k++) 
                        valor += h[k, i] * produto[k, j];

                    Somar(global, mapa[i], mapa[j], valor);

                    if (i != j) 
                        Somar(global, mapa[j], mapa[i], valor);
                }
        }

        /// <summary>Compatibilidade: monta K e Kg na mesma base expandida.</summary>
        public void MontarMatrizGeometrica()
        {
            MontarMatrizes();
        }

        // Indice da barra, de 1 a nBarras. Tracao positiva, compressao negativa.
        // Quando o axial varia, adota a media dos valores internos nas extremidades.
        public double[] EsforcosNormaisReferencia { get; private set; }

        public TPorticoEspacial_FlambagemLinear(TPorticoEspacial portico, int numeroModos)
        {
            if (portico == null)
                throw new ArgumentNullException(nameof(portico));
            if (numeroModos <= 0)
                throw new ArgumentOutOfRangeException(nameof(numeroModos));

            this.portico = portico;
            NumeroModos = numeroModos;
        }

        /// <summary>
        /// Etapa 1: captura os axiais de uma analise estatica ja concluida.
        /// idReferencia e o ID armazenado em Esforcos_Barra, nao o indice da lista.
        /// O carregamento selecionado sera escalado proporcionalmente por lambda.
        /// </summary>
        public void PrepararReferencia(int idReferencia, bool ehCombinacao)
        {
            InvalidarFatoracao();
            ReferenciaPreparada = false;
            fatoresTirantesReferencia = null;
            MatrizRigidez = null;
            GrausInternosBarras = null;
            NumeroGrausLiberdade = 0;
            MatrizGeometrica = null;
            EsforcosNormaisReferencia = null;
            if (portico.barras == null || portico.nBarras <= 0)
                throw new InvalidOperationException("O portico nao possui barras para a analise de flambagem.");

            double[] fatores = null;
            for (int b = 1; b <= portico.nBarras; b++)
                if (portico.barras[b] != null && portico.barras[b].SomenteTracao)
                {
                    fatores = portico.ObterFatoresTirantesReferencia(idReferencia, ehCombinacao);
                    break;
                }

            var normais = new double[portico.nBarras + 1];
            
            for (int indice = 1; indice <= portico.nBarras; indice++)
            {
                var barra = portico.barras[indice];
                if (barra == null)
                    throw new InvalidOperationException($"Barra {indice} inexistente.");
                if (barra.SomenteTracao && fatores[indice] != 1.0 && fatores[indice] != 1e-8)
                    throw new InvalidOperationException($"Estado do tirante {indice} invalido na referencia.");
                var resultados = ehCombinacao ? barra.combinacoes_x_esforcos : barra.casos_x_esforcos;
                Esforcos_Barra referencia = null;
                if (resultados != null)
                    foreach (var resultado in resultados)
                        if (resultado != null && resultado.id == idReferencia)
                        {
                            if (referencia != null)
                                throw new InvalidOperationException($"Referencia {idReferencia} duplicada na barra {indice}.");
                            referencia = resultado;
                        }

                if (referencia == null || referencia.Esforcos == null || referencia.Esforcos.Length < 13)
                    throw new InvalidOperationException($"Resultados estáticos da referencia {idReferencia} ausentes na barra {indice}.");

                // Forcas nodais locais: em tracao, F1 e negativa e F7 positiva.
                double inicial = -referencia.Esforcos[1];
                double final = referencia.Esforcos[7];
                double normal = 0.5 * inicial + 0.5 * final;
                if (double.IsNaN(normal) || double.IsInfinity(normal))
                    throw new InvalidOperationException($"Esforco normal invalido na barra {indice}.");
                normais[indice] = normal;
            }

            IdReferencia = idReferencia;
            ReferenciaEhCombinacao = ehCombinacao;
            EsforcosNormaisReferencia = normais;
            fatoresTirantesReferencia = fatores;
            ReferenciaPreparada = true;
        }
    }
}
