using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PG
{
    public partial class TPorticoEspacial
    {
        class mestre_x_escravo
        {
            public TNoPortico mestre, escravo;
            public int[] gl_mestre, gl_escravo;
        }

        [NonSerialized]
        List<mestre_x_escravo> nos_mestre_x_escravo;

        [NonSerialized]
        List<Dictionary<int, double>> restricoesMPC;
        int numeroGrausAuxiliaresMPC;
        internal bool TemMPC { get { return restricoesMPC != null && restricoesMPC.Count > 0; } }

        void PrepararPenalidadesMPC()
        {
            restricoesMPC = new List<Dictionary<int, double>>();
            numeroGrausAuxiliaresMPC = 0;
            ObterNosMestres_x_NosEscravos();
            OnterDiafragmasRigidos();
            foreach (var par in nos_mestre_x_escravo)
            {
                if (ReferenceEquals(par.mestre, par.escravo))
                    throw new InvalidOperationException("Braco rigido: mestre e escravo coincidem.");
                double x = par.escravo.x - par.mestre.x;
                double y = par.mestre.z - par.escravo.z;
                double z = par.mestre.y - par.escravo.y;
                for (int g = 1; g <= 6; g++)
                {
                    var c = new Dictionary<int, double> { { par.gl_escravo[g], 1 }, { par.gl_mestre[g], -1 } };
                    if (g == 1) { c[par.gl_mestre[5]] = -z; c[par.gl_mestre[6]] = y; }
                    if (g == 2) { c[par.gl_mestre[4]] = z; c[par.gl_mestre[6]] = -x; }
                    if (g == 3) { c[par.gl_mestre[4]] = -y; c[par.gl_mestre[5]] = x; }
                    restricoesMPC.Add(c);
                }
            }
            foreach (var diafragma in diafragmasIdentificados)
            {
                // Coordenadas globais do solver: X, -Z, -Y.
                var u = new OpenTK.Vector3d(diafragma.eixoU.X, -diafragma.eixoU.Z, -diafragma.eixoU.Y);
                var v = new OpenTK.Vector3d(diafragma.eixoV.X, -diafragma.eixoV.Z, -diafragma.eixoV.Y);
                var normal = OpenTK.Vector3d.Cross(u, v);
                var mestre = diafragma.mestre;
                int auxiliar = nNos * 6 + numeroGrausAuxiliaresMPC;
                if (diafragma.mestreAuxiliar)
                {
                    if (diafragma.escravos.Count < 2)
                        throw new InvalidOperationException("Diafragma " + diafragma.origem.ID + ": sao necessarios pelo menos dois nos distintos.");
                    double separacao = 0;
                    var primeiro = diafragma.escravos[0];
                    foreach (var no in diafragma.escravos)
                        separacao = Math.Max(separacao, new OpenTK.Vector3d(no.x-primeiro.x, no.y-primeiro.y, no.z-primeiro.z).Length);
                    if (separacao <= 0.001)
                        throw new InvalidOperationException("Diafragma: nos escravos coincidentes.");
                    numeroGrausAuxiliaresMPC += 3;
                }
                foreach (var escravo in diafragma.escravos)
                {
                    var r = new OpenTK.Vector3d(escravo.x-mestre.x, mestre.z-escravo.z, mestre.y-escravo.y);
                    for (int direcao = 0; direcao < 2; direcao++)
                    {
                        var eixo = direcao == 0 ? u : v;
                        double rotacao = direcao == 0 ? OpenTK.Vector3d.Dot(r, v) : -OpenTK.Vector3d.Dot(r, u);
                        double[] e = { eixo.X, eixo.Y, eixo.Z }, n = { normal.X, normal.Y, normal.Z };
                        var c = new Dictionary<int, double>();
                        for (int g = 0; g < 3; g++)
                        {
                            c[escravo.GlGlobal[g+1]] = e[g];
                            if (!diafragma.mestreAuxiliar)
                            {
                                c[mestre.GlGlobal[g+1]] = -e[g];
                                c[mestre.GlGlobal[g+4]] = rotacao * n[g];
                            }
                        }
                        if (diafragma.mestreAuxiliar)
                        {
                            c[auxiliar+direcao+1] = -1;
                            c[auxiliar+3] = rotacao;
                        }
                        restricoesMPC.Add(c);
                    }
                }
            }
        }

        // Acrescenta w C^T C antes da fatoracao. Tambem usado pela flambagem.
        internal void AplicarPenalidadesMPC(alglib.sparsematrix matriz, double[] banda = null)
        {
            if (!TemMPC || UsaEliminacaoMPC) return;
            double maior = 0;
            if (banda != null)
            {
                foreach (double valor in banda) maior = Math.Max(maior, Math.Abs(valor));
            }
            else
            {
                int c1 = 0, c2 = 0, linha, coluna;
                double valor;
                while (alglib.sparseenumerate(matriz, ref c1, ref c2, out linha, out coluna, out valor))
                    maior = Math.Max(maior, Math.Abs(valor));
            }
            double peso = maior * 1e3; // Felippa: double, aproximadamente 16 digitos.
            //double peso = maior * Math.Sqrt(Math.Pow(10.0, 16.0)); 
            if (!(peso > 0) || double.IsInfinity(peso))
                throw new InvalidOperationException("MPC: escala de rigidez invalida para a penalidade.");
            foreach (var c in restricoesMPC)
                foreach (var a in c)
                {
                    if (a.Value == 0 || glRestrito[a.Key]) continue;
                    int ir = id[a.Key] - 1;
                    foreach (var b in c)
                    {
                        if (b.Value == 0 || glRestrito[b.Key]) continue;
                        int ic = id[b.Key] - 1;
                        if (ir > ic) continue;
                        double valor = peso * a.Value * b.Value;
                        if (double.IsNaN(valor) || double.IsInfinity(valor))
                            throw new InvalidOperationException("MPC: coeficiente de penalidade inválido.");
                        if (banda == null)
                        {
                            double soma = alglib.sparseget(matriz, ir, ic) + valor;
                            alglib.sparseset(matriz, ir, ic, soma);
                            if (ir != ic) alglib.sparseset(matriz, ic, ir, soma);
                        }
                        else
                            banda[(ir+1)*LarguraBanda + ic-ir+1] += valor;
                    }
                }
        }

        internal bool EquacaoAuxiliarMPC(int equacao)
        {
            if (UsaEliminacaoMPC) equacao = independentesMPC[equacao];
            for (int gl = nNos*6+1; gl <= Ngl; gl++)
                if (id[gl]-1 == equacao) return true;
            return false;
        }

        void OnterDiafragmasRigidos()
        {
            diafragmasIdentificados = new List<DiafragmaPortico>();
            var diafragmas = gerenciador.formDesenho.Estrutura.diafragmasRigidos;
            if (diafragmas == null) return;
            foreach (var diafragma in diafragmas)
                diafragmasIdentificados.Add(IdentificarDiafragma(diafragma, nos, nNos));
        }

        [NonSerialized]
        List<DiafragmaPortico> diafragmasIdentificados;

        #region Identificacao geometrica dos diafragmas
        class DiafragmaPortico
        {
            public TDiafragmaRigido origem;
            public TNoPortico mestre;
            public bool mestreAuxiliar;
            public List<TNoPortico> escravos = new List<TNoPortico>();
            // Eixos nas coordenadas da modelagem; converter ao montar as penalidades.
            public OpenTK.Vector3d eixoU, eixoV, normal;
        }

        static DiafragmaPortico IdentificarDiafragma(TDiafragmaRigido diafragma, TNoPortico[] nos, int nNos)
        {
            const double tolerancia = 0.01;
            string erro = "Diafragma " + diafragma.ID + ": ";
            var pontos = diafragma.pontosContorno;
            if (pontos == null || pontos.Count < 3)
                throw new InvalidOperationException(erro + "informe pelo menos tres vertices.");
            var vertices = pontos.Select(p => new OpenTK.Vector3d(p.x, p.y, p.z)).ToList();
            if (vertices.Any(p => double.IsNaN(p.X) || double.IsNaN(p.Y) || double.IsNaN(p.Z)
                || double.IsInfinity(p.X) || double.IsInfinity(p.Y) || double.IsInfinity(p.Z)))
                throw new InvalidOperationException(erro + "coordenadas invalidas.");
            if ((vertices[vertices.Count - 1] - vertices[0]).Length <= tolerancia)
                vertices.RemoveAt(vertices.Count - 1);
            if (vertices.Count < 3)
                throw new InvalidOperationException(erro + "contorno insuficiente.");

            var origem = vertices[0];
            var u = vertices.Select(p => p - origem).OrderByDescending(p => p.LengthSquared).First();
            if (u.Length <= tolerancia)
                throw new InvalidOperationException(erro + "vertices coincidentes.");
            u.Normalize();
            // Nao depende dos primeiros tres vertices serem nao colineares.
            var normal = vertices.Select(p => OpenTK.Vector3d.Cross(u, p - origem))
                .OrderByDescending(p => p.LengthSquared).First();
            if (normal.Length <= tolerancia)
                throw new InvalidOperationException(erro + "vertices colineares.");
            normal.Normalize();
            var v = OpenTK.Vector3d.Cross(normal, u);
            var poligono = new List<OpenTK.Vector2d>();
            foreach (var ponto in vertices)
            {
                var r = ponto - origem;
                if (Math.Abs(OpenTK.Vector3d.Dot(r, normal)) > tolerancia)
                    throw new InvalidOperationException(erro + "vertices fora do mesmo plano.");
                poligono.Add(new OpenTK.Vector2d(OpenTK.Vector3d.Dot(r, u), OpenTK.Vector3d.Dot(r, v)));
            }

            int quantidade = poligono.Count;
            double areaDupla = 0, centroU = 0, centroV = 0;
            for (int i = 0; i < quantidade; i++)
            {
                var a = poligono[i];
                var b = poligono[(i + 1) % quantidade];
                if ((b - a).Length <= tolerancia)
                    throw new InvalidOperationException(erro + "vértices consecutivos coincidentes.");
                // Rejeita cruzamentos e contatos entre arestas nao adjacentes.
                for (int j = i + 2; j < quantidade; j++)
                {
                    if (i == 0 && j == quantidade - 1) continue;
                    var c = poligono[j];
                    var d = poligono[(j + 1) % quantidade];
                    double abC = (b.X-a.X)*(c.Y-a.Y)-(b.Y-a.Y)*(c.X-a.X);
                    double abD = (b.X-a.X)*(d.Y-a.Y)-(b.Y-a.Y)*(d.X-a.X);
                    double cdA = (d.X-c.X)*(a.Y-c.Y)-(d.Y-c.Y)*(a.X-c.X);
                    double cdB = (d.X-c.X)*(b.Y-c.Y)-(d.Y-c.Y)*(b.X-c.X);
                    if (abC * abD <= 0 && cdA * cdB <= 0
                        && Math.Max(a.X,b.X) >= Math.Min(c.X,d.X) && Math.Max(c.X,d.X) >= Math.Min(a.X,b.X)
                        && Math.Max(a.Y,b.Y) >= Math.Min(c.Y,d.Y) && Math.Max(c.Y,d.Y) >= Math.Min(a.Y,b.Y))
                        throw new InvalidOperationException(erro + "contorno com cruzamentos.");
                }
                double cruzado = a.X * b.Y - b.X * a.Y;
                areaDupla += cruzado;
                centroU += (a.X + b.X) * cruzado;
                centroV += (a.Y + b.Y) * cruzado;
            }
            if (Math.Abs(areaDupla) <= tolerancia * tolerancia)
                throw new InvalidOperationException(erro + "área nula ou insuficiente.");
            var centro = origem + u * (centroU / (3 * areaDupla)) + v * (centroV / (3 * areaDupla));
         
            var resultado = new DiafragmaPortico { origem = diafragma, eixoU = u, eixoV = v, normal = normal };
            for (int i = 1; i <= nNos; i++)
            {
                var ponto = new OpenTK.Vector3d(nos[i].x, nos[i].y, nos[i].z);
                if ((ponto - centro).Length <= tolerancia)
                {
                    if (!ReferenceEquals(resultado.mestre, null))
                        throw new InvalidOperationException(erro + "mais de um nó no centroide.");
                    resultado.mestre = nos[i];
                }
                var r = ponto - origem;
               
                if (Math.Abs(OpenTK.Vector3d.Dot(r, normal)) > tolerancia) 
                    continue;
               
                double x = OpenTK.Vector3d.Dot(r, u), y = OpenTK.Vector3d.Dot(r, v);
                bool dentro = false, borda = false;
                for (int j = 0; j < quantidade; j++)
                {
                    var a = poligono[j];
                    var b = poligono[(j + 1) % quantidade];
                    var ab = b - a;
                    double t = Math.Max(0, Math.Min(1, ((x-a.X)*ab.X + (y-a.Y)*ab.Y) / ab.LengthSquared));
                    double dx = x - a.X - t*ab.X, dy = y - a.Y - t*ab.Y;
                    if (dx*dx + dy*dy <= tolerancia*tolerancia) 
                    { 
                        borda = true; 
                        break; 
                    }

                    if ((a.Y > y) != (b.Y > y) && x < a.X + (y-a.Y)*(b.X-a.X)/(b.Y-a.Y))
                        dentro = !dentro;
                }
                if (dentro || borda)
                    resultado.escravos.Add(nos[i]);
            }

            resultado.mestreAuxiliar = ReferenceEquals(resultado.mestre, null);
            if (resultado.mestreAuxiliar)
                resultado.mestre = new TNoPortico(centro.X, centro.Y, centro.Z);
            resultado.escravos.RemoveAll(no => ReferenceEquals(no, resultado.mestre));
            if (resultado.escravos.Count == 0)
                throw new InvalidOperationException(erro + "nenhum no escravo encontrado no contorno.");
            return resultado;
        }
        #endregion
        void ObterNosMestres_x_NosEscravos()
        {
            nos_mestre_x_escravo = new List<mestre_x_escravo>();
            for (int i = 0; i < bracosRigidos.Count; i++)
            {
                nos_mestre_x_escravo.Add(new mestre_x_escravo());
                int cc = nos_mestre_x_escravo.Count - 1;

                for (int barra = 1; barra <= nBarras; barra++)
                {
                    if (Geom.Iguais(barras[barra].pIni.x, bracosRigidos[i].pIni.x)
                        && Geom.Iguais(barras[barra].pIni.y, bracosRigidos[i].pIni.y)
                        && Geom.Iguais(barras[barra].pIni.z, bracosRigidos[i].pIni.z))
                        nos_mestre_x_escravo[cc].mestre = barras[barra].pIni;

                    if (Geom.Iguais(barras[barra].pFin.x, bracosRigidos[i].pIni.x)
                        && Geom.Iguais(barras[barra].pFin.y, bracosRigidos[i].pIni.y)
                        && Geom.Iguais(barras[barra].pFin.z, bracosRigidos[i].pIni.z))
                        nos_mestre_x_escravo[cc].mestre = barras[barra].pFin;

                    if (Geom.Iguais(barras[barra].pIni.x, bracosRigidos[i].pFin.x)
                            && Geom.Iguais(barras[barra].pIni.y, bracosRigidos[i].pFin.y)
                            && Geom.Iguais(barras[barra].pIni.z, bracosRigidos[i].pFin.z))
                        nos_mestre_x_escravo[cc].escravo = barras[barra].pIni;

                    if (Geom.Iguais(barras[barra].pFin.x, bracosRigidos[i].pFin.x)
                        && Geom.Iguais(barras[barra].pFin.y, bracosRigidos[i].pFin.y)
                        && Geom.Iguais(barras[barra].pFin.z, bracosRigidos[i].pFin.z))
                        nos_mestre_x_escravo[cc].escravo = barras[barra].pFin;
                }

                if ((Object)nos_mestre_x_escravo[cc].mestre == null || (Object)nos_mestre_x_escravo[cc].escravo == null)
                    throw new InvalidOperationException("O braço rígido não encontrou um nó de pórtico correspondente.");

                nos_mestre_x_escravo[cc].gl_mestre = nos_mestre_x_escravo[cc].mestre.GlGlobal;
                nos_mestre_x_escravo[cc].gl_escravo = nos_mestre_x_escravo[cc].escravo.GlGlobal;
            }
        }
    }
}
