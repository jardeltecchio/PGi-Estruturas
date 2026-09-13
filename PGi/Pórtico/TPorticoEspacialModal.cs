using System;

using System.Collections.Generic;

namespace PG
{
    public class TPorticoEspacialModal
    {
        private readonly TPorticoEspacial portico;
        private readonly int numModos;
        private alglib.sparsedecompositionanalysis analiseCholesky;
        private alglib.sparsematrix matrizRigidezFatorada;
        private double[] diagonalCholesky;
        private int[] permutacaoCholesky;
        private bool rigidezFatorada;

        public double[] MassasLineares { get; private set; }
        public double[] MassasTotaisBarras { get; private set; }
        public double MassaTotalEstrutura { get; private set; }
        public alglib.sparsematrix MatrizMassa { get; private set; }
        public double ErroMaximoSimetriaRigidez { get; private set; }
        public double ErroMaximoSimetriaMassa { get; private set; }
        public List<double[]> BaseLanczos { get; private set; }
        public double[] DiagonalLanczos { get; private set; }
        public double[] SubdiagonalLanczos { get; private set; }
        // Autovalores de T (mu), em ordem decrescente; lambda = 1 / mu.
        public double[] AutovaloresReduzidos { get; private set; }
        // Cada coluna contém o autovetor correspondente, nas coordenadas da base de Lanczos.
        public double[,] AutovetoresReduzidos { get; private set; }
        // Resultados dos modos solicitados, em ordem crescente de frequência.
        public double[] Autovalores { get; private set; } // lambda = omega², em s^-2.
        public double[] FrequenciasAngulares { get; private set; } // rad/s.
        public double[] FrequenciasNaturais { get; private set; } // Hz.
        // Colunas: modos normalizados por phi^T M phi = 1.
        // Linhas: graus de liberdade livres, na numeração de K e M (id - 1).
        public double[,] ModosVibracao { get; private set; }
        // Linhas = modos; colunas = X, Y, Z estruturais. Massa modal efetiva em %.
        public double[,] PercentuaisMassaModal { get; private set; }

        private void CalcularParticipacaoModal()
        {
            double[,] percentuais = new double[numModos, 3];
            double[] massasModais = new double[numModos];
            for (int modo = 0; modo < numModos; modo++)
            {
                double[] phi = new double[portico.NLinhas];
                for (int i = 0; i < phi.Length; i++) phi[i] = ModosVibracao[i, modo];
                massasModais[modo] = ProdutoInternoMassa(phi, phi);
                if (massasModais[modo] <= 0.0)
                    throw new InvalidOperationException("Massa generalizada inválida no cálculo da participação modal.");
            }
            // Convenção de TNoPortico: GL 1 = DX, GL 2 = DZ, GL 3 = DY.
            // As colunas do relatório permanecem na ordem X, Y, Z.
            int[] glPorDirecao = { 1, 3, 2 };
            for (int direcao = 0; direcao < 3; direcao++)
            {
                double[] influencia = new double[portico.NLinhas];
                bool possuiGlLivre = false;
                for (int gl = glPorDirecao[direcao]; gl <= portico.Ngl; gl += 6)
                    if (!portico.glRestrito[gl])
                    {
                        influencia[portico.id[gl] - 1] = 1.0;
                        possuiGlLivre = true;
                    }
                if (!possuiGlLivre) 
                    continue;
                double[] massaInfluencia = MultiplicarMassa(influencia);
                double massaDirecao = ProdutoEscalar(influencia, massaInfluencia);
                
                if (massaDirecao <= 0.0 || double.IsNaN(massaDirecao) || double.IsInfinity(massaDirecao))
                    throw new InvalidOperationException("Massa de referência inválida na participação modal.");
                
                for (int modo = 0; modo < numModos; modo++)
                {
                    double projecao = 0.0;
                    for (int i = 0; i < influencia.Length; i++)
                        projecao += ModosVibracao[i, modo] * massaInfluencia[i];
                    // 100 * (phi^T M r)^2 / [(phi^T M phi) * (r^T M r)].
                    double razao = (projecao / Math.Sqrt(massasModais[modo])) / Math.Sqrt(massaDirecao);
                    double percentual = 100.0 * razao * razao;
                    if (double.IsNaN(percentual) || double.IsInfinity(percentual))
                        throw new InvalidOperationException("Percentual de massa modal inválido.");
                    percentuais[modo, direcao] = percentual;
                }
            }
            PercentuaisMassaModal = percentuais;
        }
        public const double ToleranciaConvergencia = 1e-4;
        public double[] ResiduosRelativos { get; private set; }
        public bool[] ModosConvergidos { get; private set; }
        public bool Convergiu { get; private set; }
        // Zero usa o limite automático: maior entre 300 vetores e 20 por modo.
        public int LimiteVetoresLanczos { get; set; }
        // Parada cooperativa entre vetores; não interrompe uma chamada da ALGLIB em curso.
        public TimeSpan LimiteTempoLanczos { get; set; } = TimeSpan.FromSeconds(60);
        public List<double[]> ResiduosPorTentativa { get; } = new List<double[]>();
        public List<int> VetoresPorTentativa { get; } = new List<int>();
        // Percentual da análise modal e descrição da etapa atual.
        public event Action<int, string> ProgressoAlterado;
        private int percentualProgresso;
        private string etapaProgresso;

        private void InformarProgresso(int percentual, string etapa)
        {
            int novoPercentual = Math.Max(percentualProgresso, Math.Min(100, percentual));
            if (novoPercentual == percentualProgresso && etapa == etapaProgresso)
                return;
            percentualProgresso = novoPercentual;
            etapaProgresso = etapa;
            ProgressoAlterado?.Invoke(percentualProgresso, etapa);
        }

        public TPorticoEspacialModal(TPorticoEspacial portico, int _numModos)
        {
            if (portico == null)
                throw new ArgumentNullException(nameof(portico));

            if (_numModos <= 0)
                throw new ArgumentOutOfRangeException(nameof(_numModos),
                    "O número de modos deve ser maior que zero.");

            this.portico = portico;
            numModos = _numModos;
        }

        private const double Gravidade = 9.80665;

        private double ObterMassaLinear(TBarraPortico barra)
        {
            if (barra == null)
                throw new InvalidOperationException(
                    "Foi encontrada uma barra nula na malha do pórtico.");

            if (barra.barraOriginal == null || barra.barraOriginal.Dados == null)
                throw new InvalidOperationException(
                    $"A barra {barra.IDBarra} não possui dados estruturais.");

            TSecao secao = barra.barraOriginal.Dados.secao;

            if (secao == null)
                throw new InvalidOperationException(
                    $"A barra {barra.IDBarra} não possui seção.");

            if (secao.PesoProprio <= 0.0)
                throw new InvalidOperationException(
                    $"A barra {barra.IDBarra} possui peso próprio inválido.");

            // Mantém na barra o peso característico, em kN/m.
            barra.pesoLinear = secao.PesoProprio;
            //barra.pesoLinear       → kN/m
         
            // Massa linear no sistema coerente com K em kN/m e deslocamentos em m.
            return barra.pesoLinear / Gravidade;
        }

        private void PrepararMassasDasBarras()
        {
            MassasLineares = new double[portico.nBarras + 1];
            MassasTotaisBarras = new double[portico.nBarras + 1];
            MassaTotalEstrutura = 0.0;

            for (int i = 1; i <= portico.nBarras; i++)
            {
                TBarraPortico barra = portico.barras[i];

                if (barra.L <= 0.0 || double.IsNaN(barra.L) || double.IsInfinity(barra.L))
                    throw new InvalidOperationException(
                        $"A barra {barra.IDBarra} possui comprimento inválido para a análise modal.");

                double massaLinear = ObterMassaLinear(barra);
                double massaTotal = massaLinear * barra.L;

                barra.SetMatrizMassaLocal(massaLinear);
                barra.SetMatrizMassaGlobal();

                MassasLineares[i] = massaLinear;
                MassasTotaisBarras[i] = massaTotal;
                MassaTotalEstrutura += massaTotal;
                InformarProgresso((int)(5L * i / portico.nBarras), "Análise modal: massas das barras");

                /*MassasLineares[i]      → kN·s²/m²
                  MassasTotaisBarras[i]  → kN·s²/m*/
            }
        }

        private void MontarMatrizMassa()
        {
            if (portico.NLinhas <= 0)
                throw new InvalidOperationException(
                    "O pórtico não possui graus de liberdade livres.");

            if (portico.id == null || portico.glRestrito == null)
                throw new InvalidOperationException(
                    "A numeração dos graus de liberdade do pórtico não foi inicializada.");

            // Matriz reduzida esparsa, com a mesma dimensão e numeração de K.
            alglib.sparsematrix matrizMassa;
            alglib.sparsecreate(portico.NLinhas, portico.NLinhas, out matrizMassa);

            for (int i = 1; i <= portico.nBarras; i++)
            {
                TBarraPortico barra = portico.barras[i];

                if (barra.MatrizMassaGlobal == null)
                    throw new InvalidOperationException(
                        $"A matriz de massa global da barra {barra.IDBarra} não foi montada.");

                for (int j = 1; j <= portico.NglBarra; j++)
                {
                    int glLinha = barra.GlGlobal[j];

                    if (portico.glRestrito[glLinha])
                        continue;

                    int linha = portico.id[glLinha] - 1;

                    for (int k = j; k <= portico.NglBarra; k++)
                    {
                        int glColuna = barra.GlGlobal[k];

                        if (portico.glRestrito[glColuna])
                            continue;

                        int coluna = portico.id[glColuna] - 1;
                        double coeficienteMassa = barra.MatrizMassaGlobal[j, k];

                        if (double.IsNaN(coeficienteMassa) || double.IsInfinity(coeficienteMassa))
                            throw new InvalidOperationException(
                                $"A matriz de massa da barra {barra.IDBarra} possui coeficiente inválido.");

                        double coeficienteGlobal = alglib.sparseget(matrizMassa, linha, coluna);
                        double novoCoeficiente = coeficienteGlobal + coeficienteMassa;

                        alglib.sparseset(matrizMassa, linha, coluna, novoCoeficiente);
                        alglib.sparseset(matrizMassa, coluna, linha, novoCoeficiente);
                    }
                }
            }

            alglib.sparseconverttocrs(matrizMassa);
            MatrizMassa = matrizMassa;
        }

        private double VerificarMatrizSimetrica(
            alglib.sparsematrix matriz,
            string nome,
            double toleranciaRelativa)
        {
            if (matriz == null)
                throw new InvalidOperationException(
                    $"A matriz {nome} não foi inicializada.");

            int numeroLinhas = alglib.sparsegetnrows(matriz);
            int numeroColunas = alglib.sparsegetncols(matriz);

            if (numeroLinhas != portico.NLinhas || numeroColunas != portico.NLinhas)
                throw new InvalidOperationException(
                    $"A matriz {nome} possui dimensão {numeroLinhas}x{numeroColunas}; " +
                    $"era esperada a dimensão {portico.NLinhas}x{portico.NLinhas}.");

            double maiorErroAbsoluto = 0.0;
            int cursor1 = 0;
            int cursor2 = 0;
            int linha = 0;
            int coluna = 0;
            double valor = 0.0;
            int numeroCoeficientes = 0;

            while (alglib.sparseenumerate(
                matriz, ref cursor1, ref cursor2,
                out linha, out coluna, out valor))
            {
                numeroCoeficientes++;

                if (double.IsNaN(valor) || double.IsInfinity(valor))
                    throw new InvalidOperationException(
                        $"A matriz {nome} possui coeficiente inválido em [{linha},{coluna}].");

                double valorTransposto = alglib.sparseget(matriz, coluna, linha);
                double erroAbsoluto = Math.Abs(valor - valorTransposto);
                double escala = Math.Max(1.0,
                    Math.Max(Math.Abs(valor), Math.Abs(valorTransposto)));

                if (erroAbsoluto > maiorErroAbsoluto)
                    maiorErroAbsoluto = erroAbsoluto;

                if (erroAbsoluto > toleranciaRelativa * escala)
                    throw new InvalidOperationException(
                        $"A matriz {nome} não é simétrica em [{linha},{coluna}]. " +
                        $"Valores: {valor} e {valorTransposto}.");
            }

            if (numeroCoeficientes == 0)
                throw new InvalidOperationException(
                    $"A matriz {nome} não possui coeficientes não nulos.");

            for (int i = 0; i < portico.NLinhas; i++)
            {
                double diagonal = alglib.sparsegetdiagonal(matriz, i);

                if (double.IsNaN(diagonal) || double.IsInfinity(diagonal))
                    throw new InvalidOperationException(
                        $"A matriz {nome} possui diagonal inválida em [{i},{i}].");

                if (diagonal <= 0.0)
                    throw new InvalidOperationException(
                        $"A matriz {nome} possui diagonal não positiva em [{i},{i}]: {diagonal}.");
            }

            return maiorErroAbsoluto;
        }

        private void VerificarMatrizesModais()
        {
            if (portico.MatrizRigidez == null || portico.MatrizRigidez.s == null)
                throw new InvalidOperationException(
                    "A matriz de rigidez esparsa do pórtico não foi montada.");

            if (numModos > portico.NLinhas)
                throw new InvalidOperationException(
                    $"Foram solicitados {numModos} modos, mas o sistema possui apenas " +
                    $"{portico.NLinhas} graus de liberdade livres.");

            const double toleranciaSimetria = 1e-10;

            ErroMaximoSimetriaRigidez = VerificarMatrizSimetrica(
                portico.MatrizRigidez.s, "de rigidez", toleranciaSimetria);

            ErroMaximoSimetriaMassa = VerificarMatrizSimetrica(
                MatrizMassa, "de massa", toleranciaSimetria);
        }


        /*Esse nome PrepararOperadorShiftInvert descreve melhor sua finalidade na análise modal:
\
        A=K^-1 * M

        A fatoração de Cholesky de K continua acontecendo internamente, mas é apenas um recurso para aplicar o operador shift-invert durante o Lanczos.

        */
        private void PrepararOperadorShiftInvert()
        {
            alglib.sparsematrix copiaRigidez;
            alglib.sparsecopy(portico.MatrizRigidez.s, out copiaRigidez);

            alglib.sparsedecompositionanalysis analise;
            bool decomposicaoOk = alglib.sparsecholeskyanalyze(
                copiaRigidez,
                false,
                0,
                -1,
                out analise);

            if (!decomposicaoOk)
                throw new InvalidOperationException(
                    "Não foi possível analisar a matriz de rigidez para a fatoração de Cholesky.");

            double[] diagonal = new double[portico.NLinhas];
            int[] permutacao = new int[portico.NLinhas];

            for (int i = 0; i < portico.NLinhas; i++)
            {
                diagonal[i] = 1.0;
                permutacao[i] = i;
            }

            decomposicaoOk = alglib.sparsecholeskyfactorize(
                analise,
                false,
                out copiaRigidez,
                out diagonal,
                out permutacao);

            if (!decomposicaoOk)
                throw new InvalidOperationException(
                    "A matriz de rigidez não é positiva definida. " +
                    "Verifique vínculos, articulações e possíveis mecanismos na estrutura.");

            analiseCholesky = analise;
            matrizRigidezFatorada = copiaRigidez;
            diagonalCholesky = diagonal;
            permutacaoCholesky = permutacao;
            rigidezFatorada = true;
        }

        private double[] ResolverComRigidez(double[] ladoDireito)
        {
            if (!rigidezFatorada || matrizRigidezFatorada == null)
                throw new InvalidOperationException(
                    "A matriz de rigidez deve ser fatorada antes da solução.");

            if (ladoDireito == null || ladoDireito.Length != portico.NLinhas)
                throw new ArgumentException(
                    $"O vetor deve possuir {portico.NLinhas} componentes.",
                    nameof(ladoDireito));

            double[] resultado;
            alglib.sparsesolverreport relatorio;

            alglib.sparsespdcholeskysolve(
                matrizRigidezFatorada,
                false,
                ladoDireito,
                out resultado,
                out relatorio);

            if (relatorio == null || relatorio.terminationtype <= 0)
                throw new InvalidOperationException(
                    "Falha ao resolver o sistema com a matriz de rigidez fatorada.");

            for (int i = 0; i < resultado.Length; i++)
            {
                if (double.IsNaN(resultado[i]) || double.IsInfinity(resultado[i]))
                    throw new InvalidOperationException(
                        $"A solução com a matriz de rigidez possui valor inválido na posição {i}.");
            }

            return resultado;
        }

        private void ValidarVetorModal(double[] vetor, string nome)
        {
            if (vetor == null || vetor.Length != portico.NLinhas)
                throw new ArgumentException(
                    $"O vetor {nome} deve possuir {portico.NLinhas} componentes.",
                    nameof(vetor));

            for (int i = 0; i < vetor.Length; i++)
            {
                if (double.IsNaN(vetor[i]) || double.IsInfinity(vetor[i]))
                    throw new InvalidOperationException(
                        $"O vetor {nome} possui valor inválido na posição {i}.");
            }
        }

        private double[] MultiplicarMassa(double[] vetor)
        {
            ValidarVetorModal(vetor, "de entrada da matriz de massa");

            if (MatrizMassa == null)
                throw new InvalidOperationException(
                    "A matriz de massa deve ser montada antes da multiplicação.");

            double[] resultado = new double[portico.NLinhas];
            alglib.sparsemv(MatrizMassa, vetor, ref resultado);

            ValidarVetorModal(resultado, "resultante da matriz de massa");
            return resultado;
        }

        private double ProdutoInternoMassa(double[] vetor1, double[] vetor2)
        {
            ValidarVetorModal(vetor1, "1 do produto interno");
            ValidarVetorModal(vetor2, "2 do produto interno");

            double[] massaVetor2 = MultiplicarMassa(vetor2);
            double produto = 0.0;

            for (int i = 0; i < portico.NLinhas; i++)
                produto += vetor1[i] * massaVetor2[i];

            if (double.IsNaN(produto) || double.IsInfinity(produto))
                throw new InvalidOperationException(
                    "O produto interno na métrica de massa resultou em valor inválido.");

            return produto;
        }

        private double NormaMassa(double[] vetor)
        {
            double normaQuadrada = ProdutoInternoMassa(vetor, vetor);

            if (normaQuadrada <= 0.0)
                throw new InvalidOperationException(
                    "Não foi possível normalizar o vetor na métrica de massa. " +
                    "A matriz de massa pode não ser positiva definida.");

            return Math.Sqrt(normaQuadrada);
        }

        private double[] NormalizarPelaMassa(double[] vetor)
        {
            ValidarVetorModal(vetor, "a normalizar");

            double norma = NormaMassa(vetor);
            double[] normalizado = new double[portico.NLinhas];

            for (int i = 0; i < portico.NLinhas; i++)
                normalizado[i] = vetor[i] / norma;

            return normalizado;
        }

        private double[] AplicarOperadorShiftInvert(double[] vetor)
        {
            ValidarVetorModal(vetor, "de entrada do operador shift-invert");

            double[] massaVetor = MultiplicarMassa(vetor);
            return ResolverComRigidez(massaVetor);
        }

        private static double ProdutoEscalar(double[] vetor1, double[] vetor2)
        {
            double resultado = 0.0;

            for (int i = 0; i < vetor1.Length; i++)
                resultado += vetor1[i] * vetor2[i];

            return resultado;
        }

        private static void SomarMultiplo(double[] destino, double[] vetor, double fator)
        {
            for (int i = 0; i < destino.Length; i++)
                destino[i] += fator * vetor[i];
        }

        private double[] CriarVetorInicialLanczos()
        {
            // Semente fixa: resultados reproduzíveis e baixa chance de o vetor
            // ser ortogonal a algum dos primeiros modos.
            Random gerador = new Random(19790619);
            double[] vetorInicial = new double[portico.NLinhas];

            for (int i = 0; i < vetorInicial.Length; i++)
                vetorInicial[i] = gerador.NextDouble() - 0.5;

            return NormalizarPelaMassa(vetorInicial);
        }

        private double[] ReortogonalizarLanczos(
            double[] vetor,
            List<double[]> baseLanczos,
            List<double[]> massasBaseLanczos)
        {
            double[] massaVetor = MultiplicarMassa(vetor);
            // Duas passagens de Gram-Schmidt modificado reduzem a perda de
            // ortogonalidade causada por autovalores próximos ou repetidos.
            for (int passagem = 0; passagem < 2; passagem++)
            {
                for (int i = 0; i < baseLanczos.Count; i++)
                {
                    double coeficiente = ProdutoEscalar(baseLanczos[i], massaVetor);

                    SomarMultiplo(vetor, baseLanczos[i], -coeficiente);
                    SomarMultiplo(massaVetor, massasBaseLanczos[i], -coeficiente);
                }
                // Corrigir a deriva acumulada antes da próxima passagem e da norma.
                // Nunca usar a imagem atualizada por subtrações para calcular beta.
                massaVetor = MultiplicarMassa(vetor);
            }
            return massaVetor;
        }

        private void ConstruirBaseLanczos()
        {
            Convergiu = false;
            int proximaVerificacao = Math.Min(
                portico.NLinhas,
                Math.Max(numModos + 8, 2 * numModos));
            if (LimiteVetoresLanczos < 0 ||
                (LimiteVetoresLanczos > 0 && LimiteVetoresLanczos < numModos))
                throw new InvalidOperationException("O limite de vetores deve ser zero (automático) ou pelo menos o número de modos.");
            if (LimiteTempoLanczos <= TimeSpan.Zero)
                throw new InvalidOperationException("O limite de tempo do Lanczos deve ser positivo.");
            int numeroMaximoVetores = (int)Math.Min(portico.NLinhas,
                LimiteVetoresLanczos == 0 ? Math.Max(300L, 20L * numModos) : LimiteVetoresLanczos);
            proximaVerificacao = Math.Min(proximaVerificacao, numeroMaximoVetores);
            var cronometro = System.Diagnostics.Stopwatch.StartNew();
            int tentativaConvergencia = 1;
            ResiduosPorTentativa.Clear();
            VetoresPorTentativa.Clear();

            List<double[]> baseLanczos = new List<double[]>();
            List<double[]> massasBaseLanczos = new List<double[]>();
            List<double> diagonal = new List<double>();
            List<double> subdiagonal = new List<double>();

            double[] qAtual = CriarVetorInicialLanczos();
            double[] massaQAtual = MultiplicarMassa(qAtual);
            double[] qAnterior = null;
            double betaAnterior = 0.0;

            for (int iteracao = 0; iteracao < numeroMaximoVetores; iteracao++)
            {
                if (cronometro.Elapsed >= LimiteTempoLanczos)
                    throw new InvalidOperationException(
                        $"Análise modal interrompida pelo limite de tempo de {LimiteTempoLanczos.TotalSeconds:G} s " +
                        $"após {baseLanczos.Count} vetores. Os modos não foram aceitos como convergidos. " +
                        "Consulte os resíduos por tentativa e verifique o modelo antes de ampliar os limites.");
                InformarProgresso(20 + (int)(75L * iteracao / numeroMaximoVetores),
                    $"Análise modal: Lanczos, vetor {iteracao + 1} de até {numeroMaximoVetores} — tentativa de convergência {tentativaConvergencia}");
                baseLanczos.Add(qAtual);
                massasBaseLanczos.Add(massaQAtual);

                // z = K^-1 M q
                double[] z = ResolverComRigidez(massaQAtual);

                if (qAnterior != null)
                    SomarMultiplo(z, qAnterior, -betaAnterior);

                double[] massaZ = MultiplicarMassa(z);
                double alfa = ProdutoEscalar(qAtual, massaZ);

                SomarMultiplo(z, qAtual, -alfa);
                massaZ = ReortogonalizarLanczos(z, baseLanczos, massasBaseLanczos);

                diagonal.Add(alfa);

                double betaQuadrado = ProdutoEscalar(z, massaZ);
                double escala = Math.Max(1.0, Math.Abs(alfa));
                double toleranciaQuebra = 1e-12 * escala;

                if (double.IsNaN(betaQuadrado) || double.IsInfinity(betaQuadrado) || betaQuadrado < 0.0)
                    throw new InvalidOperationException(
                        $"Norma de massa inválida na iteração {iteracao + 1} de Lanczos: " +
                        $"z^T M z = {betaQuadrado:E16}, com M z recalculado. " +
                        "Verifique a definição positiva e o condicionamento da matriz de massa.");

                double beta = Math.Sqrt(betaQuadrado);

                bool fimDaBase = iteracao + 1 >= numeroMaximoVetores || beta <= toleranciaQuebra;
                if (baseLanczos.Count >= numModos &&
                    (baseLanczos.Count >= proximaVerificacao || fimDaBase))
                {
                    BaseLanczos = baseLanczos;
                    DiagonalLanczos = diagonal.ToArray();
                    SubdiagonalLanczos = subdiagonal.ToArray();
                    
                    InformarProgresso(percentualProgresso, "Análise modal: solução do problema reduzido");
                    ResolverProblemaReduzido();
                    
                    InformarProgresso(percentualProgresso, "Análise modal: recuperação dos modos e frequências");
                    RecuperarModosEFrequencias();
                    
                    InformarProgresso(percentualProgresso, "Análise modal: verificação da convergência");
                    VerificarConvergencia(false);
                   
                    ResiduosPorTentativa.Add((double[])ResiduosRelativos.Clone());
                    
                    VetoresPorTentativa.Add(baseLanczos.Count);
                    
                    if (Convergiu)
                        return;
                    
                    if (baseLanczos.Count >= numeroMaximoVetores && numeroMaximoVetores < portico.NLinhas)
                    {
                        double maiorResiduo = 0.0;
                        foreach (double residuo in ResiduosRelativos)
                            maiorResiduo = Math.Max(maiorResiduo, residuo);
                        throw new InvalidOperationException(
                            $"Análise modal sem convergência após atingir o limite de {numeroMaximoVetores} vetores " +
                            $"para {numModos} modos. Maior resíduo: {maiorResiduo:E3}; tolerância: {ToleranciaConvergencia:E3}. " +
                            "Consulte ResiduosPorTentativa; " +
                            "verifique o modelo ou ajuste LimiteVetoresLanczos explicitamente.");
                    }
                    if (fimDaBase)
                        VerificarConvergencia(); // Não aceitar modos sem convergência.

                    // Continuar a mesma recorrência, preservando base e fatoração.
                    proximaVerificacao = (int)Math.Min(numeroMaximoVetores,
                        2L * proximaVerificacao);
                    tentativaConvergencia++;
                }

                if (fimDaBase)
                    break;

                subdiagonal.Add(beta);

                double[] proximoQ = new double[portico.NLinhas];

                for (int i = 0; i < portico.NLinhas; i++)
                {
                    proximoQ[i] = z[i] / beta;
                }

                qAnterior = qAtual;
                qAtual = proximoQ;
                massaQAtual = MultiplicarMassa(qAtual);
                betaAnterior = beta;
            }

            if (baseLanczos.Count < numModos)
                throw new InvalidOperationException(
                    $"O processo de Lanczos gerou apenas {baseLanczos.Count} vetores " +
                    $"para os {numModos} modos solicitados.");

            BaseLanczos = baseLanczos;
            DiagonalLanczos = diagonal.ToArray();
            SubdiagonalLanczos = subdiagonal.ToArray();
        }

        private void ResolverProblemaReduzido()
        {
            AutovaloresReduzidos = null;
            AutovetoresReduzidos = null;

            if (DiagonalLanczos == null || DiagonalLanczos.Length == 0 ||
                SubdiagonalLanczos == null ||
                SubdiagonalLanczos.Length != DiagonalLanczos.Length - 1)
                throw new InvalidOperationException(
                    "A matriz tridiagonal de Lanczos não foi construída corretamente.");

            int ordem = DiagonalLanczos.Length;
            if (ordem < numModos)
                throw new InvalidOperationException("A base reduzida possui menos vetores que os modos solicitados.");
            // O solver sobrescreve a diagonal; preservar os coeficientes de T.
            double[] valores = (double[])DiagonalLanczos.Clone();
            double[] subdiagonal = (double[])SubdiagonalLanczos.Clone();
            for (int i = 0; i < ordem; i++)
            {
                if (double.IsNaN(valores[i]) || double.IsInfinity(valores[i]) ||
                    (i < ordem - 1 && (double.IsNaN(subdiagonal[i]) ||
                                      double.IsInfinity(subdiagonal[i]))))
                    throw new InvalidOperationException(
                        "A matriz tridiagonal de Lanczos possui coeficiente inválido.");
            }

            double[,] vetores = new double[0, 0];
            // Somente os maiores mu, equivalentes às menores frequências.
            // zneeded = 2; índices inclusivos, base zero; saída em ordem crescente.
            if (!alglib.smatrixtdevdi(ref valores, subdiagonal, ordem, 2,
                ordem - numModos, ordem - 1, ref vetores))
                throw new InvalidOperationException(
                    "O cálculo dos autovalores da matriz reduzida não convergiu.");

            double[] valoresOrdenados = new double[numModos];
            double[,] vetoresOrdenados = new double[ordem, numModos];
            for (int modo = 0; modo < numModos; modo++)
            {
                int origem = numModos - 1 - modo;
                double valor = valores[origem];
                if (double.IsNaN(valor) || double.IsInfinity(valor) || valor <= 0.0)
                    throw new InvalidOperationException(
                        "A matriz reduzida apresentou autovalor inválido ou não positivo.");

                valoresOrdenados[modo] = valor;
                for (int linha = 0; linha < ordem; linha++)
                    vetoresOrdenados[linha, modo] = vetores[linha, origem];
            }

            AutovaloresReduzidos = valoresOrdenados;
            AutovetoresReduzidos = vetoresOrdenados;
        }

        private void RecuperarModosEFrequencias()
        {
            if (BaseLanczos == null || AutovaloresReduzidos == null ||
                AutovetoresReduzidos == null || BaseLanczos.Count < numModos ||
                AutovaloresReduzidos.Length != numModos ||
                AutovetoresReduzidos.GetLength(0) != BaseLanczos.Count ||
                AutovetoresReduzidos.GetLength(1) != numModos)
                throw new InvalidOperationException(
                    "O problema reduzido deve ser resolvido antes de recuperar os modos.");

            foreach (double[] vetor in BaseLanczos)
                ValidarVetorModal(vetor, "da base de Lanczos");

            double[] autovalores = new double[numModos];
            double[] angulares = new double[numModos];
            double[] frequencias = new double[numModos];
            double[,] modos = new double[portico.NLinhas, numModos];

            for (int modo = 0; modo < numModos; modo++)
            {
                double mu = AutovaloresReduzidos[modo];
                double lambda = 1.0 / mu;
                if (mu <= 0.0 || double.IsNaN(mu) || double.IsInfinity(mu) ||
                    lambda <= 0.0 || double.IsNaN(lambda) || double.IsInfinity(lambda))
                    throw new InvalidOperationException(
                        "Foi obtido autovalor inválido ao recuperar os modos estruturais.");

                // phi = Q y: combinação dos vetores da base de Lanczos.
                double[] phi = new double[portico.NLinhas];
                for (int j = 0; j < BaseLanczos.Count; j++)
                    SomarMultiplo(phi, BaseLanczos[j], AutovetoresReduzidos[j, modo]);

                phi = NormalizarPelaMassa(phi);
                ValidarVetorModal(phi, "modal normalizado");
                for (int linha = 0; linha < portico.NLinhas; linha++)
                    modos[linha, modo] = phi[linha];

                autovalores[modo] = lambda;
                angulares[modo] = Math.Sqrt(lambda);
                frequencias[modo] = angulares[modo] / (2.0 * Math.PI);
            }

            Autovalores = autovalores;
            FrequenciasAngulares = angulares;
            FrequenciasNaturais = frequencias;
            ModosVibracao = modos;
        }

        private void VerificarConvergencia(bool lancarExcecao = true)
        {
            Convergiu = false;
            ResiduosRelativos = null;
            ModosConvergidos = null;
            if (Autovalores == null || Autovalores.Length != numModos ||
                ModosVibracao == null || ModosVibracao.GetLength(0) != portico.NLinhas ||
                ModosVibracao.GetLength(1) != numModos)
                throw new InvalidOperationException("Os modos devem ser recuperados antes de verificar a convergência.");

            double[] residuos = new double[numModos];
            bool[] convergidos = new bool[numModos];
            int primeiroNaoConvergido = -1;
            for (int modo = 0; modo < numModos; modo++)
            {
                double[] phi = new double[portico.NLinhas];
                for (int i = 0; i < phi.Length; i++)
                    phi[i] = ModosVibracao[i, modo];
                ValidarVetorModal(phi, "da verificação de convergência");

                double[] kPhi = new double[phi.Length];
                alglib.sparsemv(portico.MatrizRigidez.s, phi, ref kPhi);
                ValidarVetorModal(kPhi, "K phi");
                double[] lambdaMPhi = MultiplicarMassa(phi);
                double lambda = Autovalores[modo];
                if (lambda <= 0.0 || double.IsNaN(lambda) || double.IsInfinity(lambda))
                    throw new InvalidOperationException("Autovalor inválido na verificação de convergência.");

                double escala = 0.0;
                for (int i = 0; i < phi.Length; i++)
                {
                    lambdaMPhi[i] *= lambda;
                    escala = Math.Max(escala, Math.Max(Math.Abs(kPhi[i]), Math.Abs(lambdaMPhi[i])));
                }
                ValidarVetorModal(lambdaMPhi, "lambda M phi");
                if (escala == 0.0)
                    throw new InvalidOperationException("Resíduo modal sem escala: K phi e lambda M phi são nulos.");

                // Escala comum evita overflow/underflow nas normas euclidianas.
                // eta = ||K phi - lambda M phi|| / (||K phi|| + ||lambda M phi||).
                double normaK2 = 0.0, normaM2 = 0.0, normaResiduo2 = 0.0;
                for (int i = 0; i < phi.Length; i++)
                {
                    double k = kPhi[i] / escala;
                    double m = lambdaMPhi[i] / escala;
                    normaK2 += k * k;
                    normaM2 += m * m;
                    normaResiduo2 += (k - m) * (k - m);
                }
                residuos[modo] = Math.Sqrt(normaResiduo2) / (Math.Sqrt(normaK2) + Math.Sqrt(normaM2));
                convergidos[modo] = residuos[modo] <= ToleranciaConvergencia;
                if (!convergidos[modo] && primeiroNaoConvergido < 0)
                    primeiroNaoConvergido = modo;
            }

            ResiduosRelativos = residuos;
            ModosConvergidos = convergidos;
            Convergiu = primeiroNaoConvergido < 0;
            if (!Convergiu && lancarExcecao)
                throw new InvalidOperationException(
                    $"O modo {primeiroNaoConvergido + 1} não convergiu: resíduo relativo " +
                    $"{residuos[primeiroNaoConvergido]:E3}, tolerância {ToleranciaConvergencia:E3}. " +
                    $"Base com {BaseLanczos?.Count ?? 0} vetores para {portico.NLinhas} graus de liberdade; " +
                    "o Lanczos esgotou a base disponível ou atingiu uma quebra numérica.");
        }

        public void Calcular(int numeroDeModos)
        {
            if (numeroDeModos != numModos)
                throw new ArgumentException(
                    "O número de modos informado difere do definido no construtor.",
                    nameof(numeroDeModos));

            AutovaloresReduzidos = null;
            AutovetoresReduzidos = null;
            Autovalores = null;
            FrequenciasAngulares = null;
            FrequenciasNaturais = null;
            ModosVibracao = null;
            PercentuaisMassaModal = null;
            ResiduosRelativos = null;
            ModosConvergidos = null;
            Convergiu = false;
            percentualProgresso = 0;
            etapaProgresso = null;
            InformarProgresso(0, "Análise modal: massas das barras");
            PrepararMassasDasBarras();
            InformarProgresso(5, "Análise modal: montagem da matriz de massa");
            MontarMatrizMassa();
            InformarProgresso(10, "Análise modal: verificação das matrizes");
            VerificarMatrizesModais();
            InformarProgresso(15, "Análise modal: fatoração da rigidez");
            PrepararOperadorShiftInvert();
            ConstruirBaseLanczos();
            CalcularParticipacaoModal();
            InformarProgresso(100, "Análise modal concluída");

            // K vem do pórtico, já montada e com os vínculos aplicados
            // M é montada especificamente para a análise modal
            // Resolve K φ = λ M φ
        }
    }
}
