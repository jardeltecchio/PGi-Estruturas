using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PG
{


    public static class Lay
    {
         public const string Zero             = "0";

         public const string Barras = "Barras"; 
         public const string Vigas = "Vigas";
         public const string ElementosBasicos = "Elementos Básicos";
         public const string TextosVigas      = "Textos Vigas";
       
         public const string Pilares          = "Pilares";
         public const string TextosPilares    = "Textos Pilares";

         public const string CargaPontual = "Carga Pontual";
         public const string CargaLinear  = "Carga Linear";
  
         public const string Lajes = "Lajes";
         public const string TextosLajes       = "Textos Lajes";
         public const string CarregamentoLajes = "Carregamento Adicional Lajes";

         public const string GrelhaLajes       = "Grelhas de Lajes";
         public const string GrelhaVigas       = "Grelhas de Vigas";
    }

    public static class GrupoLay
    {
        public const string Principal   = "Principal";
        public const string Arquitetura = "Arquitetura";
    }

    public static class conv
    {

        public static double comp(double valor, string de, string para)
        {
            if (de == und.cm)
            {
                if (para == und.m)
                    return valor/100;

                if (para == und.mm)
                    return valor *10;

                if (para == und.cm)
                    return valor;
            }
            if (de == und.m)
            {
                if (para == und.m)
                    return valor;

                if (para == und.mm)
                    return valor * 1000;

                if (para == und.cm)
                    return valor*100;
            }
            if (de == und.mm)
            {
                if (para == und.m)
                    return valor / 1000;

                if (para == und.mm)
                    return valor;

                if (para == und.cm)
                    return valor / 10;
            }
            return 0;
        
        }

        public static double forca(double valor, string de, string para)
        {
            if (de == und.kN)
            {
                if (para == und.kN)
                    return valor;

                if (para == und.N)
                    return valor * 1000;

                if (para == und.kgf)
                    return valor * 100;

                if (para == und.tonf)
                    return valor /10;
            }
            
            if (de == und.N)
            {
                if (para == und.kN)
                    return valor/1000;

                if (para == und.N)
                    return valor;

                if (para == und.kgf)
                    return valor / 10;
                
                if (para == und.tonf)
                    return valor / 10000;
            }

            if (de == und.kgf)
            {
                if (para == und.kN)
                    return valor / 100;

                if (para == und.N)
                    return valor * 10;

                if (para == und.kgf)
                    return valor;

                if (para == und.tonf)
                    return valor / 1000;
            }

            if (de == und.tonf)
            {
                if (para == und.kN)
                    return valor * 10;

                if (para == und.N)
                    return valor * 10;

                if (para == und.kgf)
                    return valor * 1000;

                if (para == und.tonf)
                    return valor;
            }

            return 0;
        }
    }

    public static class und
    {
        public const string N = "N";
        public const string kN = "kN";
        public const string kgf = "kgf";
        public const string tonf = "tonf";

        public const string kNm2 = "kN/m²";
        public const string kNcm2 = "kN/cm²";
        public const string tonfm2 = "tonf/m²";         
        public const string kgfm2 = "kgf/m²";         
        public const string kgfcm2 = "kgf/cm²";
        public const string mpa = "MPa";

        public const string m = "m";
        public const string cm = "cm";
        public const string mm = "mm";
    }

    public static class Const
    {
        public const int totCoresTensao = 7;
        public const double Tol = 1e-4;
        public const float PIDiv180 = 0.017453292f;
        public static string CANCELOU_CALCULO = "Cálculo cancelado.";
        public const string ID_INSERCAO_INDIVIDUAL = "insercao individual";
        public const string ID_INSERCAO_SELECAO = "insercao selecao";

        public const string ID_LINHA = "linha";
        public const string ID_PONTO = "ponto";
        public const string SECAO_SOLIDO_RET = "conc ret";
        public const string SECAO_SOLIDO_CIRC = "conc circ";
        public const string SECAO_W_I_LAMINADO = "w_i laminado";
        public const string SECAO_CANT_LAMINADO = "cant laminado";
        public const string SECAO_RED_LAMINADO = "red laminado";
        public const string SECAO_U_DOBRADO_ENRIJ = "u dobrado enrij";
        public const string SECAO_U_DOBRADO = "u dobrado";
        public const string SECAO_GENERICA = "generica";

        public const string COPIAR_COMANDO_1 = "Copiar - Selecione os objetos que deseja copiar e clique com o botão direito do mouse para confirmar";
        public const string COPIAR_COMANDO_2 = "Copiar - Indique o ponto base";
        public const string COPIAR_COMANDO_3 = "Copiar - Indique a posição";

        public const string MOVER_COMANDO_1 = "Mover - Selecione os objetos que deseja mover e clique com o botão direito do mouse para confirmar";
        public const string MOVER_COMANDO_2 = "Mover - Indique o ponto base";
        public const string MOVER_COMANDO_3 = "Mover - Indique a posição";

        public const string MOVER_EXTREMO_COMANDO_1 = "Mover extremidade - Selecione o(s) elemento(s) que deseja mover a extremidade e clique com o botão direito do mouse para confirmar";
        public const string MOVER_EXTREMO_COMANDO_2 = "Mover extremidade - Selecione o(s) nó(s) e clique com o botão direito do mouse para confirmar";
        public const string MOVER_EXTREMO_COMANDO_3 = "Mover extremidade - Indique o ponto base";
        public const string MOVER_EXTREMO_COMANDO_4 = "Mover extremidade - Indique a posição";

        public const string ROTACIONAR_COMANDO_1 = "Rotacionar - Selecione os objetos e clique com o botão direito do mouse para confirmar";
        public const string ROTACIONAR_COMANDO_2 = "Rotacionar - Indique o ponto 1 do eixo de rotação";
        public const string ROTACIONAR_COMANDO_3 = "Rotacionar - Indique o ponto 2 do eixo de rotação";

        public const string ESPELHAR_COMANDO_1 = "Espelhar - Selecione os objetos e clique com o botão direito do mouse para confirmar";
        public const string ESPELHAR_COMANDO_2 = "Espelhar - Indique o ponto 1 do plano de espelhamento";
        public const string ESPELHAR_COMANDO_3 = "Espelhar - Indique o ponto 2 do plano de espelhamento";
        public const string ESPELHAR_COMANDO_4 = "Espelhar - Indique o ponto 3 do plano de espelhamento";

        public const string ROTULAR_COMANDO_1 = "Articular - Selecione o(s) elemento(s) que deseja articular e clique com o botão direito do mouse para confirmar";

        public const string VersaoPGi = "PGi - Versão 2.0";

        public const string ID_ARCOIMF = "arco imf";
        public const int ID_VIGA      = 2;
        public const int ID_VIGAARCO  = 3;
        public const string ID_PILAR     = "pilar";
        public const string ID_LAJE      = "laje";
        public const string ID_TRECHOVIGA = "trecho viga";
        public const string ID_BARRAGENERICA = "barra generica"; 
        public const string ID_APOIO = "apoio";
        public const string ID_COTA = "cota";

        public const int ID_CAPTURAPONTOMEDIO = 7;
        public const int ID_RETANGULO   = 8;
        public const int ID_ARCO_DOIS_P = 9;
        public const int ID_ARCO_TRES_P = 10;
        public const string ID_TEXTO       = "texto";
        public const string ID_CIRCULO     = "circulo";
        public const string ID_GRIP = "Grip";
        public const string ID_BARRAGRELHA = "Barra Grelha";
        public const string ID_CARGA_PONTUAL = "carga pontual";
        public const string ID_CARGA_LINEAR = "carga linear";
        public const string ID_CARGA_MOMENTO = "carga momento";
        public const string ID_CARGA_AREA = "carga area";

        public const string ID_TIPO_CASO = "caso";
        public const string ID_TIPO_COMBINACAO = "combinacao";


        public const string ID_ARCO  = "Arco";

        public const string DR_PB = " Desloc. relativo - Ponto base:";
        public const string DR_D = " Desloc. relativo - Deslocamento (x,y):";
        public const string PMDP_PP = " Ponto médio a dois pontos - Primeiro ponto:";
        public const string PMDP_SP = " Ponto médio a dois pontos - Segundo ponto:";
        
        
        /*Constantes de nomes de ferramentas de edição*/
        public const string ID_ROTACIONAR_PILAR = "rotacionar pilar";
        public const string ID_DIVIDIR_VIGA = "dividir trecho viga";
        public const string ID_RENOMEAR_VIGA = "renomear trecho viga";
        public const string ID_COPIAR_ELEMENTOS = "copiar elementos";
        public const string ID_ROTACIONAR_ELEMENTOS = "rotacionar elementos";
        public const string ID_ESPELHAR_ELEMENTOS = "espelhar elementos";
        public const string ID_GIRAR_ELEMENTOS = "girar elementos";
        public const string ID_MOVER_ELEMENTOS = "mover elementos";
        public const string ID_MOVER_EXTREMO_ELEMENTOS = "mover extremidade elementos";
        public const string ID_COPIA_PADRAO = "cópia padrão";
        public const string ID_DIVIDIR_NAS_INTERSECOES = "dividir nas interseções";

        public const string ID_ARTICULAR_ELEMENTO = "articular elemento";


        /*Comandos de desenho*/
        public const string CMD_CARGA_PONTUAL_1_P = "Carga nodal - Selecione o ponto de aplicação da carga e clique com o botão direito do mouse para confirmar";
        public const string CMD_CARGA_LINEAR_1_P = "Carga distribuída - Selecione as barras e clique com o botão direito do mouse para confirmar";
        public const string CMD_CARGA_LINEAR_2_P = "Carga Linear - Selecione o segundo ponto:";
        
        public const string CMD_ARCO_IMF_1_P = "Arco - Primeiro ponto:";
        public const string CMD_ARCO_IMF_2_P = "Arco - Segundo ponto:";
        public const string CMD_ARCO_IMF_3_P = "Arco - Terceiro ponto:";

        public const string CMD_LINHA_1_P = "Linha - Primeiro ponto:";
        public const string CMD_LINHA_2_P = "Linha - Segundo ponto:";
        public const string CMD_CIRCULO_1_P = "Círculo - Primeiro ponto:";
        public const string CMD_CIRCULO_2_P = "Círculo - Segundo ponto:";
        public const string CMD_TRECHOVIGA_1_P = "Viga - Primeiro ponto";
        public const string CMD_TRECHOVIGA_2_P = "Viga - Segundo ponto (A = Alterna face)";
        
        public const string CMD_BARRA_1_P = "Novo membro - Selecione o primeiro ponto";
        public const string CMD_BARRA_2_P = "Novo membro - Selecione o segundo ponto";
        
        public const string CMD_APOIO_1_P = "Apoio - Selecione o(s) nó(s)";

        public const string CMD_LAJE_1_P = "Laje - Clique em um ponto interno à laje";
        public const string CMD_LAJE_2_P = "Laje - Indique a posição do título";
        public const string CMD_TEXTO_1_P = "Texto - Indique a posição do texto";

        public const string CMD_PILAR_1_P = "Pilar - Clique no local do pilar (A = Alterna vértice)";
        public const string CMD_PILAR_2_P = "Pilar - Indique o ângulo de rotação ou aperte enter para finalizar";

        /*Comandos de botoes de edição*/
        public const string CMD_PILAR_ROTACIONAR_1_P = "Selecione o pilar";
 

    }
}
