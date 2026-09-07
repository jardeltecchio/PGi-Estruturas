using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Globalization;

namespace PG
{
    public class TRelatoriosResultados
    {

        public string RelatorioAnaliseModal(TPorticoEspacial portico)
        {
            if (portico == null || portico.FrequenciasNaturais == null ||
                portico.FrequenciasAngulares == null || portico.PercentuaisMassaModal == null)
                throw new InvalidOperationException("Recalcule a análise modal para gerar as participações de massa.");
            int quantidade = portico.FrequenciasNaturais.Length;
            if (quantidade == 0 || portico.FrequenciasAngulares.Length != quantidade ||
                portico.PercentuaisMassaModal.GetLength(0) != quantidade ||
                portico.PercentuaisMassaModal.GetLength(1) != 3)
                throw new InvalidOperationException("Resultados modais incompletos para o relatório.");

            var cultura = CultureInfo.GetCultureInfo("pt-BR");
            var texto = new StringBuilder();
            texto.AppendLine("RELATÓRIO DA ANÁLISE MODAL");
            texto.AppendLine();
            texto.AppendLine("Massa (%) representa o percentual da massa total efetiva que participa no modo de vibração");
            //        texto.AppendLine("Omega em rad/s; frequência em Hz; massa modal efetiva em percentual.");
            //         texto.AppendLine("X, Y e Z são os eixos globais estruturais, não os eixos gráficos.");
            //          texto.AppendLine("Referência por direção: r^T M r, usando a matriz de massa dos GL livres.");
            ///            texto.AppendLine("Participação (%) = 100 * (phi^T M r)^2 / [(phi^T M phi) * (r^T M r)].");
            //            texto.AppendLine("Direções sem translações livres apresentam 0%. Percentuais não são o fator Gamma.");
            texto.AppendLine();
            //    const string formato = "{0,6} | {1,18} | {2,18} | {3,18} | {4,14} | {5,14} | {6,14}";
            const string formato = "{0,6} | {1,18} | {2,18} | {3,14} | {4,14} | {5,14}";
            //       texto.AppendLine(string.Format(formato, "Modo", "Omega (rad/s)", "Frequência (Hz)", "Período (s)", "Massa X (%)", "Massa Y (%)", "Massa Z (%)"));
            texto.AppendLine(string.Format(formato, "Modo", "Omega (rad/s)", "Frequência (Hz)", "Massa X (%)", "Massa Y (%)", "Massa Z (%)"));
            texto.AppendLine(new string('-', 99));
            double[] acumulado = new double[3];
            for (int modo = 0; modo < quantidade; modo++)
            {
                for (int d = 0; d < 3; d++) 
                    acumulado[d] += portico.PercentuaisMassaModal[modo, d];
                
                texto.AppendLine(string.Format(cultura, formato, modo + 1,
                    portico.FrequenciasAngulares[modo].ToString("n6", cultura),
                    portico.FrequenciasNaturais[modo].ToString("n6", cultura),
                    //(1/portico.FrequenciasNaturais[modo]).ToString("n6", cultura),
                    portico.PercentuaisMassaModal[modo, 0].ToString("F4", cultura),
                    portico.PercentuaisMassaModal[modo, 1].ToString("F4", cultura),
                    portico.PercentuaisMassaModal[modo, 2].ToString("F4", cultura)));
            }
            texto.AppendLine(new string('-', 99));
        //    texto.AppendLine(string.Format(formato, "Total", "", "", acumulado[0].ToString("F4", cultura), acumulado[1].ToString("F4", cultura), acumulado[2].ToString("F4", cultura)));
            return texto.ToString();
        }
    }
}
