//...............................................................................//
//         Procedimentos Globais                                                 //
//...............................................................................//

unit FH;

interface

{$WARN UNIT_PLATFORM OFF}
{$WARN SYMBOL_PLATFORM OFF}
{$WARN SYMBOL_DEPRECATED OFF}

uses
  Messages, SysUtils, Classes, Controls, StdCtrls, ExtCtrls, Jpeg, Windows, Graphics, Dialogs, Forms,
  DB, DBGrids, ShellAPI, Menus, ComCtrls, DateUtils, StrUtils, Math, DbClient, SqlTimSt, Consts,
  CheckLst, TlHelp32, Spin, Buttons, Grids, Variants, FileCtrl, Mask, DBCtrls, CommCtrl, ValEdit,
  ComObj, Registry, UrlMon, Clipbrd, typInfo;


const
  NFe_finNFe_Ajuste                     = '3 - NFe de ajuste';
  NFe_finNFe_Complementar               = '2 - NFe complementar';
  NFe_finNFe_Normal                     = '1 - NFe normal';
  NFe_modFrete_Dest                     = '1 - por conta do destinatário';
  NFe_modFrete_Emitente                 = '0 - por conta do emitente';
  NFe_procEmi_Avulsa_Contribuinte_Fisco = '2 - Avulsa, pelo contribuinte com seu certificado digital, através do site do Fisco';
  NFe_procEmi_Avulsa_Fisco              = '1 - Avulsa pelo Fisco';
  NFe_procEmi_ComAplicativo             = '0 - Com aplicativo do contribuinte';
  NFe_procEmi_Contribuinte_Fisco        = '3 - Pelo contribuinte com aplicativo fornecido pelo Fisco';
  NFe_tpEmis_ContDPEC                   = '4 - Contingência DPEC';
  NFe_tpEmis_ContFS                     = '2 - Contingência FS';
  NFe_tpEmis_ContFSDA                   = '5 - Contingência FSDA';
  NFe_tpEmis_ContSCAN                   = '3 - Contingência SCAN';
  NFe_tpEmis_Normal                     = '1 - Normal';
  NFe_tpNF_Entrada                      = '0 - Entrada';
  NFe_tpNF_Saida                        = '1 - Saida';

  DefErrMsg  = 'Conteúdo inválido ou não informado para o campo.';
  PixelMM    : double = 3.50;
  cngStrMeses: array[1..12] of string = ('01', '02', '03', '04', '05', '06',
                                         '07', '08', '09', '10', '11', '12');

var
  // registro
  vrgRoot        : string = 'pressier';
  { para ficar compatível com o prERP/prMAG, mas se usar em outra
    aplicação é melhor trocar }
  vrgSubRoot     : string = 'ERP Comercial';
  vrgDefErrMsg   : string = DefErrMsg;
  vrgHKeyDefault1: HKEY = HKEY_USERS;
  vrgHKeyDefault2: HKEY = HKEY_CURRENT_USER;

  // grids
  vrgGridDefSubDir   : string = 'sis\grid';
  vrgGridDefExtension: string = '.cfg';

  //
  vrgDefaultDir: string = 'sis\';
  vrgDirCache  : string = 'Sel';

  //
  vrgTrueStr : string = 'S';
  vrgFalseStr: string = 'N';





{----------------------------------------------------------------------------------------------}
//    Preenche uma string com um determinado símbolo. Se o tamanho da string
//    for maior que PTam a string será truncada.
//      PDirecao -> indica o lado do preenchimento:
//                  -1 à esquerda, 0 no centro e 1 à direita
//      PTam  -> define o tamanho do resultado.
//      PChr -> não é obrigatório. Define o preenchimento
function PreencheStr(const PDirecao: Shortint; const S: string; const PTam: SmallInt;
  const PChr: string = #32): string; overload;

//    Preenche uma string com um determinado símbolo à esquerda.
//    Se o tamanho da string for maior que PTam a string será truncada.
function PreencheStr(const S: string; const PTam: SmallInt; const PChr: string = #32): string; overload;

//    Preenche uma string com brancos à direita
function PreencheDir(const S: string; const PTam: SmallInt): string;

//    Preenche uma string com brancos à esquerda
function PreencheEsq(const S: string; const PTam: SmallInt): string;

//    Preenche uma string com zeros à esquerda
function StrZero(const S: string; const PTam: SmallInt): string; overload;

function StrZero(const N: integer; const PTam: SmallInt): string; overload;

//  Completa uma string com um símbolo. É diferente de PreencheStr porque
//  esta função não trunca a string se ela for maior que PTam
procedure PadR(var PString: string; const PTam: integer; const PChr: string = ' ');

procedure PadL(var PString: string; const PTam: integer; const PChr: string = ' ');

procedure PadZeroR(var PString: string; const PTam: integer);

procedure PadZeroL(var PString: string; const PTam: integer);

//    Conta as ocorrências de PSubStr dentro de S
function ContaStr(S, PSubStr: string; PIndex, PTotal: integer): integer; overload;

function ContaStr(S, PSubStr: string): integer; overload;

function TrimX(const S: string; A: string): string;

//     Retorna a parte inteira de uma string
function StrInteiro(pNumero: string): string;

//     Retorna a parte não numérica de uma string
function RetParteNaoNumerica(pTexto: string): string;

//     Retorna a parte inteira de uma string em inteiro
function GetInteiro(const pNumero: string; const PDefault: integer = 0): integer;

//     Remove os zeros à esquerda da string
function LimpaZero(Texto: string): string;

function GeraEspc(PTam: integer): string;

function LimpaChrEspecial(Str: string): string;

function LimpaAaZ(PStr: string): string;

function AspaDup(const S: string): string;

function AspaSimp(const S: string): string;

function LimpaAspas(const S: string): string;

//  Junta os dois parâmetros ou formata S com o parâmentro I.
//  Usa-se ^ em vez de %d na formatação
function Juntar(S: string; I: integer): string;

function RetCodigoStr(const S: string; const PSep: string = ' '): string;

function RetDescriStr(const S: string; const PSep: string = ' '): string;

function RetSubCodigoStr(const S: string; const PSep: string = ' '): string;

//    gera uma lista de strings a partir de uma string simples com o
//  seguinte formato: S1,S2,S3,..,Sn
function StrParaLista(POrigem: string): TStrings;

function ListaParaStr(ALista: TStrings): string;

function CarregaSeExiste(L: TStrings; const pArq: string): Boolean;

procedure InsereNaLista(Lista: TStrings; const Linhas: string);

procedure FormaLista(var V: string; const S: string; const Espacado: Boolean = True);

function TemAaZ(const PStr: string): Boolean;

function NaoTemAaZ(const PStr: string): Boolean;

function SeStrVazia(const PStr, PDefault: string): string;

function TemInteiro(const PStr: string): Boolean;

function NaoTemInteiro(const PStr: string): Boolean;

//     É o mesmo Pos mas faz a procura de trás para frente, sendo que se o
//  resultado for 1 na verdade será o último caractere da string
function RetroPos(PSubStr: string; S: string): integer;

function RetroCopy(S: string; Index, Count: integer): string;

procedure RetroInsert(Substr: string; var Dest: string; Index: integer);

//     Executa múltiplas chamadas da procedure Insert
procedure MultiInsert(var Dest: string; const Substr: array of string; const Index: array of integer);

function EntreChr(const PStr: string; const PC1, PC2: Char; const PEspc: Cardinal = 0): string;

function EntreCol(const PStr: string; const PEspc: Cardinal = 0): string;

function EntrePar(const PStr: string; const PEspc: Cardinal = 0): string;

//    Converte as propriedades da fonte informada em uma string.
//    Cada propriedade é separada por uma quebra de linha, ficando
//    no formato das linhas de um TStrings
function FonteParaStr(const F: TFont): string;

function StrParaFonte(const S: string; PDefault: TFont): TFont;

function MontaFonte(const PNomeFone: string; const PTam: integer;
                    const PColor: TColor; const PEstiloFonte: TFontStyles): TFont;

procedure CriaLista(var L: TStrings; const pTexto: string = '');

procedure GravaLista(var L: TStrings; const Arquivo: string);

function LimpaString(const S: string; PChr: array of string): string;

function LimpaEspc(const S: string): string;

function LimpaPonto(const S: string): string;

function LimpaVrgl(const S: string): string;

// Elimina todos "#$D#$A" em uma string
function LimpaBreak(const S: string; const PSubChar: string = ' '): string;

//     Substitui/insere as quebras no texto.
//     TrocaQuebra = true  : substitui a quebra '#$D#$A' pelo '<BR>'
//     TrocaQuebra = false : substitui o '<BR>' pela quebra '#$D#$A'
function TrocaBreaks(const pTexto: string; const TrocaQuebra: Boolean): string;

// Remove os pontos de uma string e a converte para numérico
// Se S não for um valor válido então será devolvido PDefault
// Se PDefault for igual a 3.4e-4932 (MinExtended) e S for inválido então
// uma exceção será disparada
function StrParaNum(const S: string; PDefault: Extended = MinExtended): Extended;

function CopyPos(const PStr, PSubStr: string; const PTrim: Boolean = true): string;

//     Funciona como um Copy(PStr, PIndex, Length(PStr) - PTotal)
//     Ou seja, quanto maior for PTotal menor será o resultado
function CopyMin(const PStr: string; const PTotal: integer; const PIndex: integer = 1): string;

//     Funciona como um Copy(PStr, PIndex, MaxInt)
//     Ou seja, copia uma string a partir de um ponto até o final
function CopyMax(const PStr: string; const PIndex: integer): string;

function CopyPosMax(const PStr, PSubStr: string; const PTrim: Boolean = true): string;

//     Faz uma comparação 'case-insensitive' entre PVar e PFixo
//     PVar é ajustada para não ser maior que PFixo
function ComparaTextoFixo(const PVar, PFixo: string): Boolean;

function ComparaTextoRapido(const S1, S2: string): Boolean;

//  Substitui a função SysUtils.StrScan que exige parâmetros PChar
//  PInit indica em que ponto da string de resultado será copiado
//  o resultado final. Por padrão é 1, ou seja, será repassado o resultado
//  da função SysUtils.StrScan na íntegra, senão será como
//  um Copy(Resultado, PInit, 2147483647)
function StrScan(const PStr: string; Chr: Char; PInit: integer = 1): string;

procedure CopiaParaAreaTransf(const S: string);

procedure GeraArqTexto(const pNomeArq, pTexto: string);

function ExcLinhasEmBranco(const pLinhas: string): string;

function FmtNum(pVal: Extended; pDec: Byte = 2): string;
function FmtFix(pVal: Extended; pDec: Byte = 2): string;

function FmtNumStr(pVal: string; pDec: Byte = 2; PDefault: Extended = 0): string;
function FmtFixStr(pVal: string; pDec: Byte = 2; PDefault: Extended = 0): string;

function FmtInt(pVal: integer): string;

//     gera uma string formatada com zeros à esquerda, útil para exportações para
//     arquivo texto
function FmtFloatStr(pVal: Extended; PTamString: integer; pDec: Byte = 2): string; overload;
function FmtFloatStr(pVal: string; PTamString: integer; pDec: Byte = 2): string; overload;

function FmtNumParaFix(pVal: string; pDec: Byte = 2; PDefault: Extended = 0): string;

function StrToFloatFix(pVal: string): Extended;

function FmtValSQL(pVal: Extended): string;

//    Funciona como um Pos "Case-Insensitive"
function GetPos(Substr, Str: string): integer;

function StrInvalida(const S: string): Boolean;

function StrOk(const S: string): Boolean;

function StringListOk(SL: TStrings): Boolean;

function RetMaskNum(const PQtdDec: integer; const PPrefixo: string = '###,###,##0.'): string;

//     Retorna um valor escrito por extenso em formato monetário
function RetValorExtenso(pValor: double; const FormatoNF: Boolean = False; const PMonetario: Boolean = True;
  const PGenFem: Boolean = False): string;

//     Retorna um código formatado conforme a máscara do CNPJ/CPF
//        CPF  = 999.999.999-99
//        CNPJ = 99.999.999/9999-99
function FormataCNPJCPF(const PCode: string): string;

//   Retorna um código formatado conforme a máscara do CNPJ/CPF
//        CPF  = 999999999-99
//        CNPJ = 99.999.999/9999-99
function MontaCNPJCPF(const Codigo: string): string;

//     Torna maiúscula toda a string pTexto ou somente uma parte dela
//     quando a PTotal for maior que zero
function SetaUpper(const pTexto: string; const PTotal: integer = -1): string;

//  muda a 1ª letra para maiúscula
function InitCap(const pTexto: string): string;

function MultiStringReplace(const S: string; const OldPattern, NewPattern: array of string;
  Flags: TReplaceFlags): string;

//     Retorna um determinado número de palavras da string passada
function RetPalavra(const S: string; const PQuant: integer): string;

procedure IniGera(var L: TStrings; const PConteudo: string);
procedure IniGrava(var L: TStrings; const pNomeArq: string);
procedure IniDeletaSec(var L: TStrings; const PSecao: string);
procedure IniDeletaIdent(var L: TStrings; const PSecao, PIdent: string);
function IniExisteSec(var L: TStrings; const PSecao: string): Boolean;
procedure IniGravaStr(var L: TStrings; const PSecao, PIdent, pValor: string);
procedure IniGravaInt(var L: TStrings; const PSecao, PIdent: string; const pValor: integer);
procedure IniGravaNum(var L: TStrings; const PSecao, PIdent: string; const pValor: Extended);
procedure IniGravaBln(var L: TStrings; const PSecao, PIdent: string; const pValor: Boolean);
procedure IniGravaData(var L: TStrings; const PSecao, PIdent: string; const pValor: TDateTime);
function IniLeStr(var L: TStrings; const PSecao, PIdent: string; const PDefault: string = ''): string;
function IniLeInt(var L: TStrings; const PSecao, PIdent: string; const PDefault: integer): integer;
function IniLeNum(var L: TStrings; const PSecao, PIdent: string; const PDefault: Extended): Extended;
function IniLeBln(var L: TStrings; const PSecao, PIdent: string; const PDefault: Boolean): Boolean;
function IniLeData(var L: TStrings; const PSecao, PIdent: string; const PDefault: TDateTime): TDateTime;

function LeIniControls(const PSecao: string; PContainer: TWinControl): string;
procedure GravaIniControls(const PConteudo, PSecao: string; PContainer: TWinControl);

function RetBreak(const PTotal: Cardinal = 1): string;

//  retorna o índice se algum dos valores passado pelo vetor PValores está contido ou é
//  igual ao parâmetro pTexto. O teste é 'case-insensitive'
function ContemTextoIndex(const pTexto: string; const PValores: array of string): integer;

//    verifica se algum dos valores passado pelo vetor PValores está contido ou é
//    igual ao parâmetro pTexto. O teste é 'case-insensitive'
function ContemTexto(const pTexto: string; const PValores: array of string): Boolean;

function Match(const pTexto: string; const pValores: array of string): Boolean;

function Contem(const pTexto, pSubtexto: string): Boolean;

function NaoContem(const pTexto, pSubtexto: string): Boolean;

function Igual(const pTexto, pSubtexto: string): Boolean;

function NaoIgual(const pTexto, pSubtexto: string): Boolean;

//  retorna o número ordinal. PFeminino for 'true' então retorna a string no gênero
//  feminino
function GetNumeroOrdinal(const PNro: ShortInt; const PFeminino: Boolean = false): string;

function PreparaValorValido(const PStr: string; const pDec: SmallInt; const PSepMilhar: Boolean): string;

function BoolParaStr(const pValor: Boolean): string;
function StrParaBool(const pValor: string; const PDefault: Boolean = false): Boolean;

function SeTemInteiro(const PStr: string; const SenaoRetorne: string = ''): string;

function VerSQLLike(const PStr: string; const NaEsquerda, NaDireita: Boolean): string;

function WhereAnd(const PSql: string; const VerificarApos: string = ''): string;

function AdicionaBreak(const pTexto: string): string;

procedure AdicionaLinha(var S: string; const Linha: string; const Quebras: Integer = 1);

function RetCodigoCombo(CB: TComboBox; const PCodigos: array of string): string;

//    retorna o valor de uma determinada posição do array Valores
//    é util para retornar valores de um combo, lista etc...
//    ex.: situação [ítens         : |TODOS|OK  |Cancelado|Estornado|
//                   corresponde a : |''   |'OK'|'CA'     |'ES'     |]
//         poupa de fazer um "case" testando índice por índice para
//         fazer, por ex., um script sql
function RetValIndex(const Index: integer; const Valores: array of string;
  const Quoted: Boolean = false): string;

function CaseOf(const Valor: string; const IDs, Results: array of string): string;

//  muda uma string para o formato de nome próprio
function ProperCase(sBuffer: string): string;

function SentenceCase(str: string): string;

function IncEllipsis(PStr: string; const PTam: Integer): string;

function StrParaVar(const PStr: string): Variant;

function ContaPalavra(const PStr: string): Integer;

procedure PreparaPChar(var P: PChar; const Org: string; const TamMax, TamAloca: Cardinal);

function MixString(const TamMax: Integer; const Full: Boolean): string;

function EmailOK(const PEmail: string): Boolean;

function RetTag(const Tag, Texto: string): string;

function MontaTag(const Tag, Texto: string): string;

function SeTemConteudo(const Texto, TxIni, TxFin: string): string;

{----------------------------------------------------------------------------------------------}
function DataOk(const S: string): Boolean;

function DataHoraOk(const S: string): Boolean;

function DataInvalida(const S: string): Boolean;

//     Assegura que a string S seja uma data válida
function TrataData(const S: string; PDef: string = ''): string;

function HoraOk(const S: string): Boolean;

//     Assegura que a string S seja uma hora válida
function TrataHora(const S: string; PDef: string = ''): string;

function GetDataExtenso(PStrData: string; PDiaPorExtenso: Boolean): string;

function ComprimeData(const PData: string): string;

function ComprimeDataHora(const PDataHora: string): string;

function IsDate(PStr: string): Boolean;

function GetDiaSemanaExtenso(PData: Tdatetime): string; overload;

function GetDiaSemanaExtenso(const PDia: integer): string; overload;

function DiaUtil(PData: TDatetime): Boolean;

function DiaUtilProximo(PData: TDatetime): TDatetime;

function DiaUtilAnterior(PData: TDatetime): TDateTime;

function DiaBaseProximo(PData: TDatetime; PDia: Integer): TDateTime;

function TotalDiasUteis(PData: string; const ConsideraSabados: Boolean): Integer;

function GetNomeMes(const PMes: integer; const PDisplayFormat: string = 'mmmm'): string;

function FmtDataSql(PStrData: string): string; overload;
function FmtDataSql(PData: TDateTime): string; overload;

function FmtDataSqlSimples(PStrData: string): string;

function GetDataArq(const pNome: string): TDateTime;

//     verifica a formatação de datas do windows 
procedure VerificaFmtDataWindows;

function HojeStr: string;

function OntemStr(Retrocesso: integer = 1): string;

//     Retorna um mês anterior a data atual 
function UltimoMes: string;

function HoraStr(const PHora: TTime = 1): string; overload;
function HoraStr(const PHora: string): string; overload;

//    retorna a hora atual em formato hh:mm
function NowStr: string;

function TimeStamp: string;

//     Retorna a hora atual em string no formato hh:mm:ss
function HoraExata: string;

function MontaHoraExtensa(const T: string; const PAbrev: Boolean = false): string;

//     Retorna o mês/ano da data passada. Se não for informada a data na versão
//     com parâmetro TDateTime será considerada a data atual
function GetMesAno(const PData: TDateTime = 0): string; overload;
function GetMesAno(const PData: string): string; overload;

//     Codifica uma data conforme o MesAno passado (ex.: '10/2003'). Se PDia for
//     -1 então será atribuído o último dia do mês correspondente
function MontaDataMesAno(const PMesAno: string; const PDia: Smallint = 1): TDateTime;
function MontaDataStrMesAno(const PMesAno: string; const PDia: Smallint = 1): string;

function MontaMesAnoExtenso(const DataOuMesAno: string; const AnoComDoisDigitos: Boolean = false): string;

function InverteMesAno(const PMesAno: string; const SemBarra: Boolean = False): string;
function InverteAnoMes(const PAnoMes: string): string;

function IncMesAno(const PMesAno: string; const Incremento: Integer): string;

function MesAnoValido(const pValor: string): Boolean;
function AnoMesValido(const pValor: string): Boolean;

//     Retorna o mês da data (TDateTime ou String) em formato string
function RetMesStr(const PData: TDateTime = 0): string; overload;
function RetMesStr(const PData: string): string; overload;

function DiaAtual: integer;

function MesAtual: integer;

function AnoAtual: integer;

function AnoAtualStr: string;

function MesAtualStr: string;

function MesAtualExtenso: string;

function DiaAtualStr: string;

//    converte um número em milisegundos para um padrão de hora válido
function FmtMilesimos(const Z: int64; const MostraMilesimos: Boolean = true): string;

// converte um número a diferença de milisegundos para um padrão de hora válido
// esta função diminui o resultado de GetTickCount do parâmetro BaseZ, ou
// seja, ela retorna a diferença entre as duas a fim de mostrar um determinado
// espaço de tempo
function MontaHoraDelta(const BaseZ: int64): string;

//    Retorna o AnoMês da data passada. Se não for informada a data na versão
//    com parâmetro TDateTime será considerada a data atual
function ExtraiAnoMesCR(const PData: TDateTime = 0): string; overload
function ExtraiAnoMesCR(const PData: string): string; overload;

function MontaDataAnoMesCR(const PAnoMes: string; const PDia: Smallint = 1): TDateTime;
function MontaDataStrAnoMesCR(const PAnoMes: string; const PDia: Smallint = 1): string;

function TrocaDiaStr(const PData: TDateTime = 0; const PDia: Smallint = 1): string; overload;
function TrocaDiaStr(const PData: string; const PDia: Smallint = 1): string; overload;

function UltimoDia(const PData: TDateTime = 0): integer; overload;
function UltimoDia(const PData: string): integer; overload;
function UltimoDia(PAno, PMes: SmallInt): SmallInt; overload;

function PrimeiroDiaMes(const PData: TDateTime = 0): TDateTime;
function PrimeiroDiaMesStr(const PData: TDateTime = 0): string;

//     Retorna a data com último dia do mês
function UltimoDiaMes(const PData: TDateTime = 0): TDateTime;
function UltimoDiaMesStr(const PData: TDateTime = 0): string;

//     Codifica uma data a partir da data original cujo formato deve ser informado
function MontaDataFmt(const PDataOriginal, PDisplayFormat: string): string;

function DataNoIntervalo(const PData, PMin, PMax: TDateTime): Boolean;

function DatasIguais(const D1, D2: TDateTime): Boolean;

function TrataMesAno(PMaskEdit: TMaskEdit): Boolean;

function PrimeiroDiaMesAno(const PMesAno: string): string;

function UltimoDiaMesAno(const PMesAno: string): string;

function ScanDateStr(const Format, S: string; var D, M, Y: Integer): Boolean;

{----------------------------------------------------------------------------------------------}
function TruncVal(const V: Extended): Extended;

function IsFloat(PStr: string): Boolean;

function IsFloatMaiorQue(PStr: string; PMaiorQue: double): Boolean;

function IsInt(PStr: string): Boolean;

function FloatOk(const S: string): Boolean;

function IntOk(const S: string): Boolean;
function Int64Ok(const S: string): Boolean;

function IntInvalido(const S: string): Boolean;

function TrataInt(const S: string; PDef: string = '0'): string;

function ZeroOk(const F: Extended): Boolean;

function ValoresIdenticos(const PValor1, PValor2: extended; const pDec: Integer = 2): Boolean;

//Checa se pValor é zero e passa então o PDefault para o resultado
function VerZero(const pValor: Extended; const PDefault: Extended = 1): Extended;

function MaiorQue(const PValor1, PValor2: Extended; const PDelta: Extended = 0): Extended;

//     Faz uma divisão segura evitando a divisão por zero
function SafeDiv(const PDividendo, PDivisor: Extended; const PDivisorDefault: Extended = 1): Extended;

function TrataDivZero(const PDividendo, PDivisor: Extended; const ResultadoSeZero: Extended = 0): Extended;

function CalculaIdade(const PNascimento, PDataAtual: string): integer;

//     Executa Arredondarmento e devolve float
//     Utilizar em somatórios
function ArredondaValor(const pVal: Extended; const PDecimal: integer = 2): Extended;

function ArredondaValorStr(const pVal: Extended; const PDecimal: integer = 2): string; 

function CalcPercentual(const PBase, PPercentual: double): double;

//     ex.: Qual seria o imposto sobre R$900,00, considerando a aliquota de 8.75%
//
function CalcPorcentagem(const X, Y: double): double;

//       ex.: Se a população dos EUA é de 250.000.000 de pessoas e a população da
//       California é de 30.000.000, qual a porcentagem do total de pessoas nos
//       EUA que vive na California?
function CalcPorcentagemTotal(const X, Y: double): double;

//   ex.: a população do estado de Nevada era de 800.000 em 1980 e em 1989 era de
//   1.111.000. Qual foi a mudança percentual na população?
function CalcPorcentagemDelta(const X, Y: double): double;

function TrataFloat(const S: string; PDef: string = ''): string;

//     Retorna a parte fracionária de um número multiplicada por um pivô
//     Ex.: FracInt => Frac(1.435) => 0.435 * Pivo => 0.435 * 1000 => 435
function FracInt(const X: Extended; const Pivo: integer = 1000): Int64;

function PixelParaMm(const pValor: integer): double;

function MmParaPixel(const pValor: double): integer;

//     Verifica se o valor passado em PNum está contido no array passado
function BuscaArrayInt(const PNum: integer; const PValores: array of integer): Boolean;

//     Retorna a posição do array na qual o valor passado em PNum está contido
function PosArrayInt(const PNum: integer; const PValores: array of integer): integer;

function StrToWord(const Value: String): Word;

function WordToStr(const Value: Word): string;

function VarParaFloat(const V: Variant): Extended;

procedure QuickSort(var A: array of Integer);

function GetTickCountStr: string;

function Iff(const aValor: Boolean; const aTrue: string; const aFalse: string = ''): string; overload;

function Iff(const aValor: Boolean; const aTrue: Int64; const aFalse: Int64 = 0): Int64; overload;

function StrDiferentes(const aStr1, aStr2: string): Boolean;

{----------------------------------------------------------------------------------------------}
procedure CriptoArq(ArqOrg, ArqDst: string; pKey: Word);

//     Assegura que um delimitador seja incluído no final da string
//     Obs.: é uma abreviação da função IncludeTrailingPathDelimiter
function IncPathDel(const S: string): string;

//     Inclui uma extensão no arquivo passado
function IncExt(const S, PExt: string): string;

function DeletaExt(const S: string): string;

function TrocaPathArq(const POrigem, PDestino: string): string;

function TrocaNomeArq(const POld, PNew: string): string;

procedure DeletaArq(PDir, PArquivo: string);

//abre o arquivo passado em F com o seu respectivo programa
function ChamaExec(const F: string; FR: TForm): string;

procedure ExecutaArq(const F: string; FR: TForm = nil);

function GetTipoArq(const PArquivo: string): string;

function ClonaArq(ArqOrg, ArqDst: string; ASobrepor: Boolean = false): Boolean;

//     Rertorna o diretorio para criacao de arquivos temporarios
//  Resultado: '<unidade>:\<diretório_da_aplicação>\<dir_especificado>\'
function SetaLocal(const PDir: string): string;

function SetaLocalTmp: string;

function RetDirSis: string;

function RetDataSis: TDateTime;

function TerminaProcesso(PNomeExe: string): integer;

function ProcessoRodando(PNomeExe: string): Boolean;

function ContaProcessosRodando(PNomeExe: string): integer;

function BlockInput(fBlockIt: Boolean): Boolean;

//  Monta um nome de arquivo com o caminho e uma extensão válida
//  Se a PExtensao for branca a extensão de pNome não é modificada
//  PPath não precisa incluir um delimitador e pode ser um caminho com
//  nome de arquivo pois será extraído somente o diretório
//  pNome pode ser um caminho completo com o nome do arquivo pois será
//  extraído somente o nome com a extensão 
function MontaNomeArq(const PPath, pNome: string; PExtensao: string = ''): string;

//     Delete múltiplos arquivos de um diretório conforme a máscara PMask
procedure DeletaArqEx(FR: TForm; const PDir, PMask: string);

//     copia um arquivo de um diretório para outro. PArquivo pode ser uma máscara
procedure ClonaArqEx(FR: TForm; const PDirOrigem, PDirDestino, PArquivo: string);

function DeletaDiretorio(const PDir: string): Boolean;

function TrocaNomeArqEx(const PDirArqOrigem, PDirArqDestino: string): Boolean;

function GetConteudoArq(const PArquivo: string): string;

function GetTamDetalhadoArq(const PExec: string): string;

function DeletaUltimoSubdir(const PDir: string): string;

//  ajusta o nome do arquivo para um padrão válido no windows
function SetaNomeArqValido(const pNomeArq: string): string;

{----------------------------------------------------------------------------------------------}
procedure GravaRegistry(const pKey, pNome, PConteudo: string; const PSubKeys: string = '');
procedure GravaRegistryGeral(pKey, pNome, PConteudo: string);
procedure GravaRegistryStr(pKey: TForm; pNome, PConteudo: string); overload;
procedure GravaRegistryStr(pKey, pNome, PConteudo: string); overload;
procedure GravaRegistryInt(pKey: TForm; pNome: string; PConteudo: integer); overload;
procedure GravaRegistryInt(pKey, pNome: string; PConteudo: integer); overload;
procedure GravaRegistryBool(pKey: TForm; pNome: string; PConteudo: Boolean); overload;
procedure GravaRegistryBool(pKey, pNome: string; PConteudo: Boolean); overload;
procedure GravaRegistryReal(pKey: TForm; pNome: string; PConteudo: Extended); overload;
procedure GravaRegistryReal(pKey, pNome: string; PConteudo: Extended); overload;
procedure GravaRegistryDate(pKey: TForm; pNome: string; PConteudo: TDateTime); overload;
procedure GravaRegistryDate(pKey, pNome: string; PConteudo: TDateTime); overload;

function LeRegistry(const pKey, pNome: string; const PSubKeys: string = ''): string;
function LeRegistryGeral(pKey, pNome: string): string;
function LeRegistryStr(pKey: TForm; pNome: string; PDef: string = ''): string; overload;
function LeRegistryStr(pKey, pNome: string; PDef: string = ''): string; overload;
function LeRegistryInt(pKey: TForm; pNome: string; PDef: integer = 0): integer; overload;
function LeRegistryInt(pKey, pNome: string; PDef: integer = 0): integer; overload;
function LeRegistryBool(pKey: TForm; pNome: string; PDef: Boolean = false): Boolean; overload;
function LeRegistryBool(pKey, pNome: string; PDef: Boolean = false): Boolean; overload;
function LeRegistryReal(pKey: TForm; pNome: string; PDef: Extended = 0): Extended; overload;
function LeRegistryReal(pKey, pNome: string; PDef: Extended = 0): Extended; overload;
function LeRegistryDate(pKey: TForm; pNome: string; PDef: TDateTime = 0): TDateTime; overload;
function LeRegistryDate(pKey, pNome: string; PDef: TDateTime = 0): TDateTime; overload;

function VerificaRegistry(const pKey, pNome: string; const PSubKeys: string = ''): Boolean;

//     exclui os valores de uma chave do registry
//     Se PLimpaTudo for false será considerado o parâmetro pVal que especifica
//     um valor de seqüência da chave passada
function LimpaRegistry(pKey: string; PLimpaTudo: Boolean; pVal: string = '';
  const PSubKeys: string = ''): Boolean;

function DeletaRegistry(pKey: string; const PDeletarValoresSeExistir: Boolean;
  const PSubKeys: string = ''): Boolean;

function TrocaChaveRegistry(PVlKey, PNvKey: string): Boolean;

procedure CarregaRegistry(pKey: string; Lista: TStrings; const PSubKeys: string = '';
  const CarregaKeys: Boolean = False);

procedure TrocaRegistry(const pKey, PNomeVl, PNomeNv: string; const PSubKeys: string = '');

type
  TPropRegistry = -1..4;

//     Grava ou retorna as propriedades básicas do tcontrol passado em AControl
//     a partir do registry. O parâmetro PCarrega determina se a propriedade vai
//     ser lida(true) ou gravada(false). PDefault é por default -1
procedure ControlaRegistryControl(pKey: TForm; AControl: TControl; PCarrega: Boolean;
  PDefault: TPropRegistry = -1; const PSubKeys: string = '');

//  Controles suportados:
//
//     **************** t p r o p r e g i s t r y **********************************
//     -----------------------------------------------------------------------------
//     control          -1        0       1        2          3         4
//     -----------------------------------------------------------------------------
//     tedit            text      text    text     text       text      text
//     tmemo            text      text    text     text       text      text
//     tcheckbox        checked   s/n     checked  checked    checked   -
//     tspinedit        value     value   value    value      value     -
//     tcombobox        index     index   text     items.txt  2+index   -
//     tradiobutton     checked   s/n     checked  checked    checked   -
//     tlistbox         index     index   index    items.txt  2+index   items.txt
//     tradiogroup      index     index   index    index      index     -
//     tcustommaskedit  text      text    text     text       text      -
//     -----------------------------------------------------------------------------
//
//      4 = não muda a propriedade do componente quando não há nada no registry
procedure GravaRegistryControl(pKey: TForm; AControl: TControl; PDefault: TPropRegistry = -1;
  const PSubKeys: string = '');
procedure LeRegistryControl(pKey: TForm; AControl: TControl; PDefault: TPropRegistry = -1;
  const PSubKeys: string = '');
procedure GravaRegistryMultiControl(pKey: TForm; PContainer: TWinControl; PDefault: TPropRegistry = -1;
  const PSubKeys: string = '');
procedure LeRegistryMultiControl(pKey: TForm; PContainer: TWinControl; AUseDefault: Boolean = true;
  PDefault: TPropRegistry = -1; const PSubKeys: string = '');

//  Escreve um valor qualquer no canvas especificado obedecendo a um alinhamento
//  específico. Para escrever em um dbgrid use DrawText uma vez que
//  DsnCanvas é de baixo nível
procedure DsnCanvas(PCanvas: TCanvas; PRect: TRect; DX, DY: integer; const PStr: string;
  PAlinhamento: TAlignment);

//     Modifica a fonte de uma célula do DBGrid. Isto inclui estilo e cor
procedure GridDsnFntCel(GR: TDBGrid; const R: TRect; DataCol: integer; PColuna: TColumn;
  PState: TGridDrawState; PColor: TColor; PEstiloFonte: TFontStyles; PVerificaFoco: Boolean = true);

//     Modifica a fonte e o preenchimento de uma célula do DBGrid.
//     Isto inclui estilo e cor da fonte e cor do preechimento
procedure GridDsnCel(GR: TDBGrid; const R: TRect; DataCol: integer; PColuna: TColumn;
  PState: TGridDrawState; PCorFonte, PCorBrush: TColor; PEstiloFonte: TFontStyles;
  PVerificaFoco: Boolean = true; PCorSelecionada: TColor = clHighlightText);

//     Escreve uma string qualquer em uma célula do DBGrid. O texto é alinhado
//     conforme o alinhamento da coluna. A cor e a fonte são preservadas
procedure GridDsnStr(GR: TDBGrid; const R: TRect; PColuna: TColumn; const PStr: string);

//     Escreve uma string qualquer em uma célula do StringGrid. A cor e a fonte
//     são preservadas. PAlinhamento é: -1 à esquerda, 0 ao centro e 1 à direita
procedure StringGridDsnStr(SG: TStringGrid; const R: TRect; PAlinhamento: integer; const PStr: string);

procedure GravaColGrid(FR: TForm; GR: TDBGrid; const PArquivo: string;
  const PShowMsg: Boolean);
procedure LeColGrid(FR: TForm; GR: TDBGrid; const PArquivo: string);
procedure DeletaColGrid(FR: TForm; GR: TDBGrid; const PArquivo: string;
  const PShowMsg: Boolean);

function RetColumn(GR: TDBGrid; const PNomeCampo: string): TColumn;

{----------------------------------------------------------------------------------------------}
function ArrayPos(PArray: array of Boolean; pValor: Boolean): integer; overload;
function ArrayPos(PArray: array of double; pValor: double): integer; overload;
function ArrayPos(PArray: array of integer; pValor: integer): integer; overload;
function ArrayPos(PArray: array of string; pValor: string): integer; overload;

procedure LimpaArray(PArray: array of string; pValor: string = ''); overload;
procedure LimpaArray(PArray: array of integer; pValor: integer = 0); overload;
procedure LimpaArray(PArray: array of double; pValor: double = 0); overload;

//     Localiza uma determinada string "PIdent" dentro do array PMap e passa sua
//     posição para APos
function IdentPos(const PIdent: string; var PPos: Longint; const PMap: array of TIdentMapEntry): Boolean;

{----------------------------------------------------------------------------------------------}
function MsgBox(const pTexto, PCaption: string; AFlags: Longint = 0): integer;

procedure MsgA(const S: string); overload;
procedure MsgA(const S: string; const Args: array of const); overload;
procedure MsgE(const S: string); overload;
procedure MsgE(const S: string; const Args: array of const); overload;
procedure MsgI(const S: string); overload;
procedure MsgI(const S: string; const Args: array of const); overload;
function MsgP(const Msg: string; DlgType: integer): integer;

{ PFlag é um ícone que pode ser:
    0 : questão
    1 : erro
    2 : atenção
    3 : informação }
type
  TFlagMsgSN = 0..3;

function MsgSN(const S: string; PFlag: TFlagMsgSN = 0): integer; overload;
function MsgSN(const S: string; const Args: array of const; PFlag: TFlagMsgSN = 0): integer; overload;

//     Dispara uma mensagem conforme o valor de pValor
//     Os flags são 1 = erro, 2 = atençã e 3 = informação
function MsgSe(pValor: Boolean; const PTrue, PFalse: string;
  const PFlagTrue: integer = 3; const PFlagFalse: integer = 1): Boolean;

{ cria uma mensagem personalizada }
type
  TIconeMsg = ( imAten, imErro, imInfo, imConf, imPers );
  TBotaoMsg = ( mbtnYes, mbtnNo, mbtnOk, mbtnCancel, mbtnAbort, mbtnRetry,
                mbtnIgnore, mbtnAll, mbtnNoForAll, mbtnYesForAll,
                mbtnHelp );
  TGrupoBotaoMsg = set of TBotaoMsg;
const
  mbtnYesNoCancel = [mbtnYes, mbtnNo, mbtnCancel];
  mbtnYesAllNoAllCancel = [mbtnYes, mbtnYesForAll, mbtnNo, mbtnNoForAll, mbtnCancel];
  mbtnOkCancel = [mbtnOk, mbtnCancel];
  mbtnAbortRetryIgnore = [mbtnAbort, mbtnRetry, mbtnIgnore];
  mbtnAbortIgnore = [mbtnAbort, mbtnIgnore];

function MontaMsgPersonalizada(const Msg: string; DlgType: TIconeMsg; Buttons: TGrupoBotaoMsg;
  const PFonteMsg, PFonteBtn: TFont): TForm;
function DispMsgPersonalizada(const Msg: string; DlgType: TIconeMsg; Buttons: TGrupoBotaoMsg;
  const PFonteMsg, PFonteBtn: TFont ): integer;
procedure MsgPersonalizada(const Msg: string; const PFonteMsg, PFonteBtn: TFont);


{----------------------------------------------------------------------------------------------}
//  mensagens stay-on-top
function PrMsg_Standard(const Msg: string; DlgType: TMsgDlgType; Buttons: TMsgDlgButtons): Integer;
procedure PrMsg_Exit(pCampo: TWinControl; const S: string); overload;
procedure PrMsg_Exit(pCampo: TWinControl; const S: string; const Args: array of const); overload;
procedure PrMsg_AT(const S: string); overload;
procedure PrMsg_AT(const S: string; const Args: array of const); overload;
procedure PrMsg_ER(const S: string); overload;
procedure PrMsg_ER(const S: string; const Args: array of const); overload;
procedure PrMsg_IN(const S: string); overload;
procedure PrMsg_IN(const S: string; const Args: array of const); overload;
function PrMsg_CF(const S: string; PFlag: TFlagMsgSN = 0): integer; overload;
function PrMsg_CF(const S: string; const Args: array of const; PFlag: TFlagMsgSN = 0): integer; overload;
function PrMsg_Focalize(pCampo: TWinControl; const PMsg: string = ''; const MsgCustomizada: Boolean = false): Boolean;
function PrMsg_FocalizeSe(const PCondicao: Boolean; pCampo: TWinControl; const PMsg: string = ''; const MsgCustomizada: Boolean = false): Boolean;

{----------------------------------------------------------------------------------------------}
function BuscaTreeView(PTreeView: TTreeView; PBusca: string): Boolean;

function BuscaTreeNode(PTreeView: TTreeView; S: string; PBuscaParcial: Boolean;
  PExpandir: Boolean = true): TTreeNode;

procedure DsnRichedit(FR: TForm; PRichEdit: TRichedit; PFormat: Boolean); overload
procedure DsnRichedit(FR: TForm; PRichEdit: TRichedit; PFormat: Boolean;
  PReservedMatrix: array of string; FonteMonospaced: Boolean = true); overload;

procedure LimpaValueListEditor(PVlLstEdt: TValueListEditor);

procedure InverteStatusCheckListBox(CB: TCheckListBox);

procedure InverteStatusListView(LV: TListView);

procedure SetaStatusCheckListBox(PStatus: Boolean; CB: TCheckListBox);

procedure SetaStatusListView(PStatus: Boolean; LV: TListView);

{ TTipoVar
    tvNul = nada é testado
    tvInt = somente valores inteiros
    tvFlo = somente valores flutuantes
    tvDat = somente datas }
type
  TTipoVar = (tvNul, tvInt, tvFlo, tvDat);

function VerConteudo(pCampo: TControl; PTipoCampo: TTipoVar = tvNul;
  const PDisparaMsg: Boolean = true; const MsgCustomizada: Boolean = false): Boolean;

function MultiVerConteudo(PCampos: array of TControl; PTipoCampo: array of TTipoVar;
  const PDisparaMsg: Boolean = true; const MsgCustomizada: Boolean = false): Boolean;

//     Testa o conteúdo padrão para TEdit, TComboBox, TMaskEdit...
//     >>> PTipoCampo: 0 = texto, 1 = inteiros, 2 = flutuantes, 3 = datas
function ValidaCampo(pCampo: TControl; PTipoCampo: Integer = 0;
  const PDisparaMsg: Boolean = true; const MsgCustomizada: Boolean = false): Boolean;

//     Testa o conteúdo padrão para múltiplos TEdit, TComboBox, TMaskEdit...
//     >>> PTipoCampo: 0 = texto, 1 = inteiros, 2 = flutuantes, 3 = datas
function ValidaMultiCampo(PCampos: array of TControl; PTipoCampo: array of Integer;
  const PDisparaMsg: Boolean = true; const MsgCustomizada: Boolean = false): Boolean;

function VerificaEntradaArq(const pNomeArq: string; pCampo: TWinControl): Boolean;

function VerificaEntradaDir(const PNomeDir: string; pCampo: TWinControl): Boolean;

//  Dispara uma mensagem de erro, envia o foco para o componente passado e
//  muda o cursor da tela para default. Sempre resultará true
//  Se a PMsg não for informada ou for branca será utilizada a mensagem da
//  variável global FH.vrgDefErrMsg
function MsgFocalize(pCampo: TWinControl; const PMsg: string = '';
  const MsgCustomizada: Boolean = false): Boolean;

//     faz a mesma coisa que a função MsgFocalize porém baseada numa condição.
//     Se PCondicao for 'true' executa senão, não faz nada
function MsgFocalizeSe(const PCondicao: Boolean; pCampo: TWinControl;
  const PMsg: string = ''; const MsgCustomizada: Boolean = false): Boolean;

//     Deleta um item qualquer de um listbox, com tratamento de índice não
//     selecionado e re-seleção de índice. Se PIndex for -1 então será
//     atribuído o índice ativo (ItemIndex)
procedure ListboxDeletaItem(PListBox: TListBox; PIndex: integer = -1);

procedure ListboxCopiaItem(PListBoxOrigem, PListBoxDestino: TListBox; PIndex: integer = -1); overload;
procedure ListboxCopiaItem(DS: TDataSet; PListBoxDestino: TListBox; PCampos: string = 'CODIGO;DESCRICAO'); overload;

procedure SetaComboItemIndex(CB: TComboBox; const PIndex: integer);

// Verifica se um item do checklist passado está marcado
// Se PFullItem = true então será procurado a string pTexto inteira nos ítens
// senão, será pego somente até a posição de PDivisor em pTexto
function VerItemCL(CL: TCheckListBox; const pTexto: string;
  const PFullItem: Boolean; const PDivisor: string = #32): Boolean;

function VerItemLV(LV: TListView; const pTexto: string): Boolean;

//     Localiza uma string no checklist
//     Se PFullItem = true então será procurado a string pTexto inteira nos ítens
//     senão, será pego somente até a posição de PDivisor em pTexto
function BuscaChecklist(CL: TCheckListBox; const pTexto: string;
  const PFullItem: Boolean; const PDivisor: string = #32): Boolean;

//     Verifica se o checklist tem ítens selecionados
function VerCheckedCL(CL: TCheckListBox): Boolean;

//     Verifica se o listview tem ítens selecionados
function VerCheckedLV(LV: TListView): Boolean;

//     Verifica se todos os ítens do listview estão selecionados
function VerCheckedAllLV(LV: TListView): Boolean;

function VerCheckedAllCL(CL: TCheckListBox): Boolean;

procedure ForceClick(PComp: TControl; PSender: TControl = nil);

procedure ForceExit(PComp: TWinControl; PSender: TWinControl = nil);

//     Dispara um evento sem parâmetros do TControl
procedure ForceEvent(PComp: TControl; PEvento: TNotifyEvent; PSender: TControl = nil);

//     envia o foco para o WC somente se ele pode ser focalizado
function Focalize(WC: TWinControl): Boolean; overload;
function Focalize(AHandle: HWnd): Boolean; overload;

function FocalizeSe(const PCondicao: Boolean; PWnCtrTrue, PWnCtrFalse: TWinControl): Boolean;

procedure FocalizeFirst(PContainer: TObject);

procedure FocalizeFirstIF(const Valor: Boolean; PContainerT, PContainerF: TObject);

procedure FocalizeFirstIn(PSender: TObject);

function ContainerFocalizado(PContainer: TWinControl): Boolean;

procedure PreparaProcPgb(var PProgBar: TProgressBar; const PMax: integer; const PVisible: Boolean;
  const AStep: integer = 1);

procedure PreparaProcLbl(FR: TForm; var PLabel: TLabel; const PCaption: string;
  PVisible: Boolean = true);

procedure PreparaProcPnl(var APanel: TPanel; PVisible: Boolean; PProgBar: TProgressBar = nil;
  PMax: integer = 0; PLabel: TLabel = nil; PCaption: string = '');

//     seta o enabled de um conjunto de componentes
procedure SetaEnabled(const PEnabled: Boolean; AControls: array of TControl);

//     seta o enabled de um conjunto de componentes dentro do container
procedure SetaEnabledContainer(PContainer: TWinControl; const PEnabled: Boolean);

//        Limpa os sub-componentes do container passado
//        Obs.: Labels com Tag 9 = caption nulo
//        Labels com Tag 8 = serão limpas com a procedure SetaCaptionLbl
procedure LimpaContainer(PContainer: TWinControl);

procedure LimpaComps(PCampos: array of TControl); overload;

//        Limpa as labels do container passado
//   Obs.: Labels com Tag = 9 ficarão com o caption nulo
//        Labels com Tag = 8 serão limpas com a procedure SetaCaptionLbl
procedure LimpaContainerLabel(PContainer: TWinControl);

procedure BloqUpdate(WC: TWinControl);
procedure DesbloqUpdate;

procedure TremeJanela(wHandle: THandle);

procedure IncExclusivo(Lista: TStrings; const S: string); overload;
procedure IncExclusivo(CB: TComboBox; const S: string); overload;

function ProcuraNaLista(Lista: TStrings; const Texto: string): Integer;

function VerEditText(PEdit: TEdit): Boolean;

procedure SetaCaptionLbl(PLabel: TLabel; const PCaption: string; const PEspc: integer = 2);

procedure FmtFloatLabel(var PLabel: TLabel; const pValor: Extended; const PCaption: string = '';
  const PThousandSeparator: Boolean = true; const pDec: Cardinal = 2);

procedure FmtIntLabel(var PLabel: TLabel; const pValor: integer; const PCaption: string = '';
  const PThousandSeparator: Boolean = true);

procedure GeraToolTip(FR: TForm; WC: TWinControl; const PIconKind: Cardinal;
  const PTitle, pTexto: string);

procedure MultiHide(FR: TForm);

procedure MultiShow(FR: TForm);

procedure AtuForms;

procedure HideFR(FR: TForm);

procedure ShowFR(FR: TForm);

function BuscaComp(FR: TForm; const pNome: string; const PIndex: integer): TComponent;

function RetForm(const FR: string): TForm;
function RetFormComp(const FR, PCtr: string): TComponent;
function RetFormControl(const FR, PCtr: string): TControl; overload
function RetFormControl(FR: TForm; const PCtr: string; const PIndex: integer): TControl; overload
function RetFormWinControl(const FR, PCtr: string): TWinControl;
function RetFormEdit(const FR, PCtr: string): TEdit; overload;
function RetFormEdit(FR: TForm; const PCtr: string): TEdit; overload
function RetFormEdit(FR: TForm; const PCtr: string; const PIndex: integer): TEdit; overload
function RetFormButton(const FR, PCtr: string): TButton;
function RetFormPageControl(const FR, PCtr: string): TPageControl;
function RetFormImage(const FR, PCtr: string): TImage;
function RetFormLabel(const FR, PCtr: string): TLabel;
function RetFormComboBox(const FR, PCtr: string): TComboBox;
function RetFormMaskEdit(const FR, PCtr: string): TMaskEdit;
function RetFormCds(const FR, PCtr: string): TClientDataSet;

function RetAtvFormComp(const PCtr: string): TComponent;
function RetAtvFormControl(const PCtr: string): TControl;
function RetAtvFormWinControl(const PCtr: string): TWinControl;
function RetAtvFormEdit(const PCtr: string): TEdit;
function RetAtvFormButton(const PCtr: string): TButton;
function RetAtvFormPageControl(const PCtr: string): TPageControl;

function VerOwner(FR: TForm; const POwner: string): Boolean;

procedure DestroiObj(var PObj);

procedure DestroiMultiObj(Objs: array of TObject);

procedure ReposCompsHrz(PComps: array of TWinControl);

procedure LimpaBufferTeclado(FR: TForm);

procedure LimpaBufferMouse(FR: TForm);

procedure SetaComboDes(CB: TComboBox; S: string; PSep: string = ' ');

procedure SetaComboCod(CB: TComboBox; S: string; PSep: string = ' ');

procedure SetaComboIndex(CB: TComboBox; const S: string);

procedure RetBitmap(PImgLst: TImageList; const PIndex: Integer; ABmp: TBitmap);

function RetIcon(PImgLst: TImageList; const PIndex: integer): TIcon;

procedure GravaImagemTab(PC: TPicture; DS: TDataSet; const pCampo: string);

procedure LeImagemTab(PC: TPicture; DS: TDataSet; const pCampo, PExtensao: string);

function CapturaTela: TBitmap;

procedure GravaArqTab(const PArq: string; DS: TDataSet; const pCampo: string);

procedure GeraArqTab(DS: TDataSet; const pCampo, PDestino: string; const AbreArq: Boolean);

{----------------------------------------------------------------------------------------------}
function VerificaCNPJ(Codigo: string): Boolean;

function VerificaCPF(Codigo: string): Boolean;

//     consiste um número de cartão de crédito
function VerificaCartaoCredito(const pNumero: string): Boolean;

function VerFone(PDdd, PFone: TEdit; PFocus: Word): Boolean;

function Modulo10(const S: string): string;

function Modulo11(const S: string): string;

function FormataCNPJ(Codigo: string): string;

procedure DesmontaCMC7(const CMC7: string; out Banco, Agencia, Conta, Cheque: string);

{----------------------------------------------------------------------------------------------}
function GetPrimeiroCampoIndex(const PIdxFlNm: string): string;

procedure HabiliaDatasets(PDataSets: array of TDataSet; PEnabled: Boolean);

//  Aqui o parâmetro PCampos deve conter os nome dos campos a serem carregados
//  separados por um ponto-e-vírgula. Ex.: 'CODIGO;NOME;ESTADO' e geran-
//  do o resultado '001 PRESSIER SISTEMAS E INFORMÁTICA RS'. PSep infor-
//  ma qual caractere será inserido entre o resultado dos campos - neste
//  caso ' '
function CarregaStringList(PTab: TDataSet; PCampos: string; PSep: string = ' '): TStrings;

//     converte uma (string/tipo de campo) para um (tipo de campo/string).
//     Ex.: ftString
function StrParaFldtp(AFType: string): TFieldType;
function FldtpParaStr(AFType: TFieldType): string;

//     Gera um CDS a partir da estrutura de um dataset qualquer e
//     não o ativa para que outros campos possam ser adicionados
procedure GeraCDSInativo(POrigem: TDataSet; CDS: TClientDataSet);

procedure GeraCDS(POrigem: TDataSet; CDS: TClientDataSet);

procedure AbreCDS(CDS: TClientDataSet; const PLogChanges: Boolean = False);

//     Copia um registro de um dataset para outro.
//     Obs.: a procedure somente move o registro mas não o grava
procedure ClonaUm(POrigem, PDestino: TDataSet; PAppend: Boolean; PIniciarEm: integer = 0);

procedure ClonaTudo(POrigem, PDestino: TDataSet; PAppend: Boolean;
  PIniciarEm: integer = 0; PProgBar: TProgressBar = nil);

procedure SetaBmark(var P: Pointer; DS: TDataSet; PGet, PGoto: Boolean;
  PFree: Boolean = false);
procedure GetBmark(var P: Pointer; DS: TDataSet);
procedure GotoBmark(var P: Pointer; DS: TDataSet; PFree: Boolean = false);

procedure SetaFmtNum(DS: TDataSet; pCampo: string; PMask: string = '###,###,###,##0.00');

procedure SetaGridDispFormat(GR: TDBGrid; const PDecs: integer = 2);

procedure SetaGridMskData(GR: TDBGrid);

//  Salva o conteúdo do ClientDataSet num arquivo XML
//  Se pNome não contiver o caminho, somente o nome, o arquivo
//  será salvo no diretório: '<diretório_do_programa>\sis\sel'
//  Se o nome estiver em branco, o nome do arquivo será o nome do
//  form e o nome clientdataset concatenados
procedure GravaXML(FR: TForm; CDS: TClientDataSet; pNome: string = ''); overload;

function LeXML(FR: TForm; CDS: TClientDataSet; pNome: string = ''): Boolean; overload;

//     carrega um clientdataset a partir do xml especificado e seta o primeiro campo como indice
function LeXMLSetaIndex(FR: TForm; CDS: TClientDataSet; pNome: string = ''): Boolean;

procedure ClonaDisplays(DSOrg, DSDst: TDataSet; const PDisplayLabel,
  PDisplayFormat: Boolean);

function RetDisplayFormat(DS: TDataSet; const pCampo: string): string;

function RetValSql(const pValor: string; const pUpperCase: Boolean): string; overload;
function RetValSql(const pValor: string): string; overload;
function RetValSql(const pValor: Extended): string; overload;
function RetValSql(const pValor: TDateTime): string; overload;

//     gera uma instrução IN válida
function GeraSQLIN(DS: TDataSet; const pCampo: string; const EntrePar: Boolean = true; const Espacado: Boolean = false): string;

function GeraSQLOR(DS: TDataSet; const pCampo: string; const pAlias: string = ''): string;

function DsParaLinha(DS: TDataSet; const pCampo: string; const Espacado: Boolean = True): string;

//     faz uma procura incremental no dataset passado como se fosse um locate simples
function Locate(DS: TDataSet; const PCampos: string;
  const PValores: array of string; const PInsensitive: Boolean;
  const PDisableControls: Boolean = false): Boolean;

//   verifica se o campo é numérico
function VerCampoNum(FL: TField): Boolean; overload;
function VerCampoNum(FT: TFieldType): Boolean; overload;

{----------------------------------------------------------------------------------------------}
function GetNumeroSerial(Drive: string): string;

function GetInfoVersao(const FileName: string): string;

{ TTipoAmpulheta
    0: crHourGlass
    1: crAppStart
    2: crSqlWait }
type
  TTipoAmpulheta = 0..2;

procedure SetaCursorAmp(const PTipo: TTipoAmpulheta = 0);
procedure SetaCursorDef;

procedure Navegar(const Endereco: string); overload;
procedure Navegar(FR: TForm; const Endereco: string); overload;

procedure Explorar(const Caminho: string);

procedure EnviaEmail(const Endereco: string);

function GetClockCPU: double;

function TerminaWindows(RebootParam: Longword): Boolean;
function DesligaWindows: Boolean;

//     retorna o nome do usuário logado no sistema operacional
function RetUsuarioWindows: string;

function FonteExiste(const PNomeFone: string): Boolean;

function TemaXpAtivo: Boolean;

function GeraCripto(const PWD: string; const Decrypt: Boolean;
  const TamRslt: integer = -1): string;

procedure GeraCodBarEAN13(PCanvas: TCanvas; pNumero: string;
  PPosX, PPosY: integer; PAlt: double);

function VerificaTeclaMov(const pKey: Word): Boolean;

procedure ControlaTeclaMesAno(var Key: Char; PMaskEdit: TMaskEdit);

procedure ExportaDOC(DS: TDataSet; const pNomeArq: string);

function IfThenColor(const PCondic: Boolean; const PTrue, PFalse: TColor): TColor;

// verifica se está rodando numa sessão remota (terminal server)
function VerSessaoRemota: Boolean;

{----------------------------------------------------------------------------------------------}
function PreencherEsq(Str, PFilling: string; PTam: integer): string;

//     Retornar a string com a 1º Letra de cadas substring Maiscula
function StrConverteIni(PStr: string): string;

//     exclui uma substring dentro de uma string
function StrRemove(PStrSub, PStr: string): string;

//    retorna somente a parte string de PStr até que um caracter numérico apareça
function RetStrChar(const PStr: string): string;

function RetStrPonto(PSubStr, PStr: string): string;

function RetCharPonto(PSubStr, PStr: string): string;

function RetLinhaPonto(PSubStr, PStr: string): string;

function RetStrArray(PStr, pKey: string): string;

function LocTStrings(PItem: TStrings; PStr: string): integer;

function RetNomeComputador: string;

function FullTimeToStr(SUMTime: TDateTime): string;









implementation

type
  THookWinControl = class(TWinControl);
  THookControl = class(TControl);
  TDateOrder = (doMDY, doDMY, doYMD);

const
  DefaultDateOrder = doDMY;
  CenturyOffset: Byte = 60;
  FieldTypeNames: array[TFieldType] of string = (
    'DESCONHECIDO', 'STRING', 'SMALLINT', 'INTEGER', 'WORD', 'BOOLEAN', 'FLOAT',
    'CURRENCY', 'BCD', 'DATE', 'TIME', 'DATETIME', 'BYTES', 'VARBYTES',
    'AUTOINC', 'BLOB', 'MEMO', 'GRAPHIC', 'FMTMEMO', 'PARADOXOLE',
    'DBASEOLE', 'TYPEDBINARY', 'CURSOR', 'FIXEDCHAR', 'WIDESTRING',
    'LARGEINT', 'ADT', 'ARRAY', 'REFERENCE', 'DATASET', 'HUGEBLOB', 'HUGECLOB',
    'VARIANT', 'INTERFACE', 'DISPATCH', 'GUID', 'SQLTIMESTAMP', 'FMTBCDFIELD');





{----------------------------------------------------------------------------------------------}
function VerificaCNPJ(Codigo: string): Boolean;
var
  tx: string;
  ii: Shortint;
  sm, dg: integer;
begin
  Result := true;
  if (Codigo = '  .   .   /    -  ')
  or (Codigo = '')
  or (Codigo = '00.000.000/0000-00')
  or (Codigo = '00000000000000') then
    exit;

  Codigo := FH.StrInteiro(Codigo);
  Codigo := FH.FormataCNPJCPF(Codigo);
  try
    Codigo := Copy(Codigo, 1, 2) + Copy(Codigo, 4, 3) + Copy(Codigo, 8, 3) +
             Copy(Codigo, 12, 4) + Copy(Codigo, 17, 2);
    tx := Copy(Codigo, 1, 12);
    sm := 0;

    for ii := 1 to 4 do
      sm := sm + StrToInt(Copy(tx, ii, 1)) * (6 - ii);
    for ii := 1 to 8 do
      sm := sm + StrToInt(Copy(tx, ii + 4, 1)) * (10 - ii);
    dg := 11 - sm mod 11;
    if dg in [10, 11] then
      tx := tx + '0'
    else
      tx := tx + IntToStr(dg);
    sm := 0;
    for ii := 1 to 5 do
      sm := sm + StrToInt(Copy(tx, ii, 1)) * (7 - ii);
    for ii := 1 to 8 do
      sm := sm + StrToInt(Copy(tx, ii + 5, 1)) * (10 - ii);
    dg := 11 - sm mod 11;
    if dg in [10, 11] then
      tx := tx + '0'
    else
      tx := tx + IntToStr(dg);
    if Codigo <> tx then
      Result := false
    else
      Result := true;
  except
    Result := false;
  end;
end;

{----------------------------------------------------------------------------------------------}
function VerificaCPF(Codigo: string): Boolean;
var
  tx: string;
  ii: Shortint;
  sm, dg, jj, nn: integer;
begin
  try
    Result := true;
    Codigo := FH.LimpaString(Codigo, ['.']);
    if (Codigo = '         -  ') or (Trim(Codigo) = '-') then
      exit;
    if (Length(Trim(Codigo)) < 11) then
    begin
      Result := false;
      exit;
    end;

    for jj := 1 to Length(Codigo) do
    begin
      try
        nn := StrToInt(Copy(Codigo, jj, 1));
        tx := tx + IntToStr(nn);
      except
        Delete(Codigo, jj, 1);
      end;
    end;
    Delete(tx, 10, 2);
    sm := 0;
    for ii := 1 to 9 do
      sm := sm + StrToInt(Copy(tx, ii, 1)) * (11 - ii);
    dg := 11 - sm mod 11;
    if dg in [10, 11] then
      tx := tx + '0'
    else
      tx := tx + IntToStr(dg);
    sm := 0;
    for ii := 1 to 10 do
      sm := sm + StrToInt(Copy(tx, ii, 1)) * (12 - ii);
    dg := 11 - sm mod 11;
    if dg in [10, 11] then
      tx := tx + '0'
    else
      tx := tx + IntToStr(dg);
    if Codigo <> tx then
      Result := false
    else
      Result := true;
  except
    Result := false
  end;
end;

{----------------------------------------------------------------------------------------------}
function VerFone(PDdd, PFone: TEdit; PFocus: Word): Boolean;
label
  Terminador;
var
  S: string;
begin
  Result := true;
  if PFone = nil then
    goto Terminador;

  S := StrInteiro(PFone.Text);
  if S = '' then
  begin
    try PDdd.Clear; except end;
    exit;
  end;

  Terminador:
  if PDdd <> nil then
  begin
    PDdd.Text := Trim(PDdd.Text);
    if (Length(PDdd.Text) > 0) and (not FH.IntOk(PDdd.Text)) then
    begin
      FH.MsgE('Informe apenas números para este campo.');
      FH.Focalize(PDdd);
      Result := false;
      exit;
    end;
    if PFone = nil then
      exit;
  end;

  if Length(S) < 7 then
  begin
    Result := false;
    FH.MsgE('Telefone inválido.');
    if PFocus = 0 then
    try
      PFone.SetFocus
    except
    end;
    exit;
  end;

  Insert('-', S, Length(S) - 3);
  PFone.Text := S;
end;    

{----------------------------------------------------------------------------------------------}
function GetDataExtenso(PStrData: string; PDiaPorExtenso: Boolean): string;
var
  vDia, vMes, vAno: string;
begin
  try
    vDia := copy(PStrData, 1, 2);
  except
  end;
  try
    vMes := GetNomeMes(StrToInt(copy(PStrData, 4, 2)));
  except
  end;
  try
    vAno := copy(PStrData, 7, 4);
  except
  end;

  if PDiaPorExtenso = true then
    vDia := GetDiaSemanaExtenso(StrToDate(PStrData)) + ', ' + vDia;

  Result := vDia + ' de ' + vMes + ' de ' + vAno;
end;

{----------------------------------------------------------------------------------------------}
function GetDiaSemanaExtenso(PData: TDatetime): string;
begin
  Result := FH.SetaUpper(FormatDateTime('dddd', PData), 1);
end;

{----------------------------------------------------------------------------------------------}
function BuscaTreeView(PTreeView: TTreeView; PBusca: string): Boolean;
var
  cIt: SmallInt;
  vItem: string;
begin
  PBusca := Trim(AnsiUpperCase(Trim(PBusca)));
  Result := false;
  cIt := 0;
  while cIt < PTreeView.Items.Count do
  begin
    vItem := Trim(AnsiUpperCase(PTreeView.Items.Item[cIt].Text));
    if PBusca = vItem then
    begin
      PTreeView.Items.Item[cIt].MakeVisible;
      PTreeView.Selected := PTreeView.Items[cIt];
      Result := true;
      exit;
    end;
    Inc(cIt);
  end;

  cIt := 0;
  while cIt < PTreeView.Items.Count do
  begin
    vItem := Trim(AnsiUpperCase(PTreeView.Items.Item[cIt].Text));
    vItem := copy(vItem, 1, Length(PBusca));
    if Pos(PBusca, vItem) > 0 then
    begin
      PTreeView.Items.Item[cIt].MakeVisible;
      PTreeView.Selected := PTreeView.Items[cIt];
      Result := true;
      exit;
    end;
    Inc(cIt);
  end;
end;

{----------------------------------------------------------------------------------------------}
function BuscaTreeNode(PTreeView: TTreeView; S: string; PBuscaParcial: Boolean;
  PExpandir: Boolean = true): TTreeNode;
var
  N: TTreeNode;
  A, B: string;

  function Retorno: Boolean;
  begin
    if PBuscaParcial then
      Result := AnsiContainsText(B, A)
    else
      Result := AnsiSameText(A, B);
  end;

begin
  Result := nil;
  N := PTreeView.Items.GetFirstNode;
  while N <> nil do
  begin
    A := FH.LimpaChrEspecial(AnsiLowerCase(S));
    B := FH.LimpaChrEspecial(AnsiLowerCase(N.Text));
    if Retorno then
    begin
      if PExpandir then
      begin
        N.MakeVisible;
        PTreeView.Selected := N;
      end;
      Result := N;
      Break;
    end;
    N := N.GetNext;
  end;
end;

{----------------------------------------------------------------------------------------------}
function IsFloat(PStr: string): Boolean;
begin
  Result := FH.FloatOk(PStr);
end;

{----------------------------------------------------------------------------------------------}
function IsFloatMaiorQue(PStr: string; PMaiorQue: double): Boolean;
var
  E: Extended;
begin
  Result := TryStrToFloat(PStr, E);
  if Result then
    Result := E > PMaiorQue;
end;

{----------------------------------------------------------------------------------------------}
function IsInt(PStr: string): Boolean;
begin
  Result := FH.IntOk(PStr);
end;

{----------------------------------------------------------------------------------------------}
function IsDate(PStr: string): Boolean;
begin
  Result := FH.DataOk(PStr);
end;

{----------------------------------------------------------------------------------------------}
function GetNomeMes(const PMes: integer; const PDisplayFormat: string = 'mmmm'): string;
begin
  Result := FH.SetaUpper(FormatDateTime(PDisplayFormat, EncodeDate(2003, PMes, 1)), 1);
end;

{----------------------------------------------------------------------------------------------}
function PreencherEsq(Str, PFilling: string; PTam: integer): string;
var
  J: integer;
begin
  for J := 1 to (PTam - Length(Str)) do
    Str := PFilling + Str;

  Str := copy(Str, 1, PTam);
  Result := Str;
end;

{----------------------------------------------------------------------------------------------}
function SetaLocal(const PDir: string): string;
var
  tmpStr, tmpDir: string;
begin
  tmpDir := Trim(PDir);
  while IsDelimiter('/:', tmpDir, 1) do
    Delete(tmpDir, 1, 1);
  tmpStr := FH.IncPathDel(ExtractFilePath(Application.ExeName));
  Result := FH.IncPathDel(tmpStr + ExcludeTrailingPathDelimiter(tmpDir));
  if not SysUtils.DirectoryExists(Result) then
    if not SysUtils.ForceDirectories(Result) then
    begin
      Result := tmpStr;
      exit;
    end;
  Result := FH.IncPathDel(Result);
end;

{----------------------------------------------------------------------------------------------}
function SetaLocalTmp: string;
begin
  Result := FH.SetaLocal('Tmp');
end;

{----------------------------------------------------------------------------------------------}
function StrConverteIni(PStr: string): string;
var
  vStr, vRes: string;
  vPos: Smallint;

  function VerSeConverte: Boolean;
  var
    vconteudo: string;
  begin
    Result := true;

    vPos := Pos(' ', vStr);
    if vPos = 0 then
      vPos := 99;

    if (vPos > 4) or (Length(vRes) = 0) or (vStr[vPos - 1] = '.') then
      exit;

    if (vPos = 4) then
    begin
      vconteudo := AnsiUpperCase(copy(vStr, 1, vPos - 1));
      if not (vConteudo = 'DOS') and not (vConteudo = 'DAS') then
        exit;
    end;

    Result := false;
  end;

begin
  vStr := PStr;
  vRes := '';
  vPos := 0;
  while Length(vRes) < Length(PStr) do
  begin
    if VerSeConverte = true then
    begin
      vRes := vRes + AnsiUpperCase(Copy(vStr, 1, 1));
      Delete(vStr, 1, 1);
      Dec(vPos);
    end;
    vRes := vRes + AnsiLowerCase(Copy(vStr, 1, vPos));
    Delete(vStr, 1, vPos);
  end;

  Result := vRes;
end;

{----------------------------------------------------------------------------------------------}
function StrInteiro(pNumero: string): string;
var
  ii: integer;
begin
  Result := '';
  for ii := 1 to Length(pNumero) do
    if FH.IntOk(pNumero[ii]) then
      Result := Result + pNumero[ii];
end;

{----------------------------------------------------------------------------------------------}
function RetParteNaoNumerica(pTexto: string): string;
const
  Num = ['0'..'9'];
var
  I: integer;
begin
  Result := '';
  for I := 1 to Length(pTexto) do
    if not (pTexto[I] in Num) then
      Result := Result + pTexto[I];
end;

{----------------------------------------------------------------------------------------------}
function GetInteiro(const pNumero: string; const PDefault: integer = 0): integer;
begin
  Result := StrToIntDef(FH.StrInteiro(pNumero), PDefault);
end;

{----------------------------------------------------------------------------------------------}
function StrRemove(PStrSub, PStr: string): string;
begin
  PStrSub := AnsiUpperCase(PStrSub);
  PStr := AnsiUpperCase(PStr);
  while Pos(PStrSub, PStr) > 0 do
    Delete(PStr, Pos(PStrSub, PStr), Length(PStrSub));
  Result := PStr;
end;

{----------------------------------------------------------------------------------------------}
procedure VerificaFmtDataWindows;
var
  vShell: OleVariant;
begin
  if Uppercase(SysUtils.ShortDateFormat) = 'DD/MM/YYYY' then
    exit;

  FH.MsgA('Este sistema requer formatação de data no formato DD/MM/AAAA.'#13 +
          'Altere as Configurações Regionais do Painel de Controle.');

  vShell := CreateOleObject('Shell.Application');
  vShell.ControlPanelItem('intl.cpl');
end;

{----------------------------------------------------------------------------------------------}
function RetStrChar(const PStr: string): string;
var
  ii: Smallint;
  vStr: string;
begin
  Result := '';
  if FH.StrInvalida(PStr) then
    exit;
  vStr := FH.LimpaChrEspecial(PStr);
  ii := 1;
  while vStr[ii] in ['A'..'Z', 'a'..'z', '_', #32] do
  begin
    Result := Result + vStr[ii];
    Inc(ii);
  end;
  Result := TrimRight(Result);
end;

{----------------------------------------------------------------------------------------------}
function RetStrPonto(PSubStr, PStr: string): string;
var
  vStrAnt, vStrPos, vStr1, vStr2: string;
  vCo: Smallint;
begin
  vStr1 := '';
  vStr2 := '';
  vStrAnt := copy(PStr, 1, Pos(PSubStr, PStr) - 1);
  vStrPos := copy(PStr, Pos(PSubStr, PStr) + 1, Length(PStr));
  vCo := Length(vStrAnt);
  for vCo := Length(vStrAnt) downto 1 do
  begin
    if (Trim(vStrAnt[vCo]) = '') or (vStrAnt[vCo] = ',') then
      break
    else
      vStr1 := vStrAnt[vCo] + vStr1;
  end;
  vCo := 1;
  for vCo := 1 to Length(vStrPos) do
  begin
    if (Trim(vStrPos[vCo]) = '')
    or AnsiMatchText(vStrPos[vCo], [',']) then
      break
    else
      vStr2 := vStr2 + vStrPos[vCo];
  end;
  Result := vStr1 + PSubStr + vStr2;
end;

{----------------------------------------------------------------------------------------------}
function RetCharPonto(PSubStr, PStr: string): string;
var
  vStrAnt, vStrPos, vStr1, vStr2: string;
  vCo: Smallint;
begin
  vStr1 := '';
  vStr2 := '';
  vStrAnt := copy(PStr, 1, Pos(PSubStr, PStr) - 1);
  vStrPos := copy(PStr, Pos(PSubStr, PStr) + 1, Length(PStr));
  vCo := Length(vStrAnt);
  for vCo := Length(vStrAnt) downto 1 do
  begin
    if (vStrAnt[vCo] in ['a'..'z'])
    or (vStrAnt[vCo] in ['0'..'9'])
    or (vStrAnt[vCo] in ['A'..'Z']) then
      vStr1 := vStrAnt[vCo] + vStr1
    else
      break;
  end;
  vCo := 1;
  for vCo := 1 to Length(vStrPos) do
  begin
    if (vStrPos[vCo] in ['a'..'z'])
    or (vStrPos[vCo] in ['0'..'9'])
    or (vStrPos[vCo] in ['A'..'Z']) then
      vStr2 := vStr2 + vStrPos[vCo]
    else
      break;
  end;
  Result := vStr1 + PSubStr + vStr2;
end;

{----------------------------------------------------------------------------------------------}
function RetLinhaPonto(PSubStr, PStr: string): string;
var
  vStrAnt, vStrPos, vStr1, vStr2: string;
  vCo: Smallint;
begin
  vStr1 := '';
  vStr2 := '';
  vStrAnt := copy(PStr, 1, Pos(PSubStr, PStr) - 1);
  vStrPos := copy(PStr, Pos(PSubStr, PStr) + 1, Length(PStr));
  vCo := Length(vStrAnt);
  for vCo := Length(vStrAnt) downto 1 do
  begin
    if (vStrAnt[vCo] <> #13) and (vStrAnt[vCo] <> #10) and (vStrAnt[vCo] <> #$D#$A) then
      vStr1 := vStrAnt[vCo] + vStr1
    else
      break;
  end;
  vCo := 1;
  for vCo := 1 to Length(vStrPos) do
  begin
    if (vStrPos[vCo] <> #13) and (vStrPos[vCo] <> #10) and (vStrPos[vCo] <> #$D#$A) then
      vStr2 := vStr2 + vStrPos[vCo]
    else
      break;
  end;
  Result := vStr1 + PSubStr + vStr2;
end;

{----------------------------------------------------------------------------------------------}
function RetStrArray(PStr, pKey: string): string;
var
  vStr, vChave: string;
begin
  while PStr <> vStr do
  begin
    vStr := PStr;
    if Pos(';', vStr) > 0 then
      vStr := copy(vStr, 1, Pos(';', vStr) - 1);

    vChave := Trim(copy(vStr, 1, Pos('=', vStr) - 1));
    if vChave = pKey then
    begin
      Result := trim(vStr);
      exit;
    end;

    Delete(PStr, 1, Length(vStr) + 1);
  end;
end;

{----------------------------------------------------------------------------------------------}
function Modulo10(const S: string): string;
var
  Indice: ShortInt;
  Peso: ShortInt;
  Soma: integer;
  Total: ShortInt;
begin
  Soma := 0;
  Peso := 2;
  for Indice := Length(S) downto 1 do
  begin
    Total := StrToInt(Copy(S, Indice, 1)) * Peso;
    if Total > 9 then
      Soma := Soma + 1 + (Total - 10)
    else
      Soma := Soma + Total;

    if Peso = 1 then
      Peso := 2
    else
      Peso := 1;
  end;

  Soma := 10 - Soma mod 10;
  if Soma = 10 then
    Soma := 0;

  Result := IntToStr(Soma);
end;

{----------------------------------------------------------------------------------------------}
function Modulo11(const S: string): string;
var
  ii, Peso, Soma: integer;
begin
  Result := '';
  Soma := 0;
  Peso := 2;
  for ii := Length(S) downto 1 do
  begin
    Soma := Soma + StrToInt(Copy(S, ii, 1)) * Peso;
    if Peso = 9 then
      Peso := 2
    else
      Inc(Peso);
  end;
  Soma := 11 - (Soma mod 11);
  if Soma > 9 then
    Soma := 0;
  Result := IntToStr(Soma);
end;

{----------------------------------------------------------------------------------------------}
procedure DsnRichedit(FR: TForm; PRichEdit: TRichedit; PFormat: Boolean);
const
  vWord: array[0..63] of string =
  (':', ',',
    'Select', 'and', 'as', 'Where', 'from', 'for',
    'null', 'or', 'to', 'do', 'until', 'of',
    'in', 'Order', 'by', 'Group', 'Left', 'Outer',
    'Right', 'Outer', 'Inner', 'Join', 'Having', 'On',
    'Distinct', 'Sum', 'Max', 'Min', 'Avarage', 'Count',
    'Asc', 'Desc', 'Not', 'Is', 'Between', 'Month',
    'Year', 'Day', 'Extract', 'Full', 'AT', 'AVG',
    'Substring', 'Cast', 'Any', 'Date', 'Exists', 'Hour',
    'Minute', 'Insert', 'Delete', 'Update', 'Like', 'NO',
    'Only', 'Set', 'then', 'Trim', 'Upper', 'Lower',
    'Starting', 'Union');
var
  FoundAt, vTWord, StartPos, ToEnd, I, PosInicial: integer;
begin
  { Deve-se desligar onChange do RichEdit antes de chamar }
  { esta procedure e ligar novamente após }

  vTWord := High(vWord);

  try
    FH.SetaCursorAmp;
    // tratamento para não "tremer" ao atualizar.
    LockWindowUpdate(PRichEdit.Handle);
    //LockWindowUpdate(FR.Handle);

    // guarda posição atual do Cursor.
    PosInicial := PRichEdit.SelStart;

    // Faz a formatação
    PRichEdit.SelectAll;
    PRichEdit.Color := clWindow;
    PRichEdit.Font.Name := 'Courier New';
    PRichEdit.Font.Size := 10;
    PRichEdit.Font.Style := [];
    PRichEdit.SelAttributes.color := clBlack;
    PRichEdit.SelAttributes.Name := PRichEdit.Font.Name;
    PRichEdit.SelAttributes.Style := PRichEdit.Font.Style;
    PRichEdit.SelAttributes.Size := PRichEdit.Font.Size;

    if PFormat then
    begin
      for I := 0 to vTWord do
      begin
        with PRichEdit do
        begin
          StartPos := 0;
          ToEnd := Length(Text) - StartPos;
          FoundAt := FindText(vWord[i], StartPos, ToEnd, [stWholeWord]);
          while (FoundAt <> -1) do
          begin
            SelStart := FoundAt;
            SelLength := Length(vWord[i]);
            SelAttributes.Style := [fsBold];
            StartPos := FoundAt + Length(vWord[i]);
            FoundAt := FindText(vWord[i], StartPos, ToEnd, [stWholeWord]);
          end;
        end;
      end;
    end;
  finally
    PRichEdit.SelStart := PosInicial;
    PRichEdit.SelAttributes.Color := clBlack;
    PRichEdit.SelAttributes.Style := [];
    LockWindowUpdate(0);
    FH.SetaCursorDef;
  end;
end;

{----------------------------------------------------------------------------------------------}
procedure DsnRichedit(FR: TForm; PRichEdit: TRichedit; PFormat: Boolean;
  PReservedMatrix: array of string; FonteMonospaced: Boolean = true);
var
  FoundAt, vTWord, StartPos, ToEnd, I, PosInicial: integer;
begin
  vTWord := High(PReservedMatrix);
  try
    FH.SetaCursorAmp;

    LockWindowUpdate(PRichEdit.Handle);
    LockWindowUpdate(FR.Handle);

    // guarda posição atual do Cursor.
    PosInicial := PRichEdit.SelStart;

    // Faz a formatação
    PRichEdit.SelectAll;
    PRichEdit.Color := clWindow;
    if FonteMonospaced then
    begin
      PRichEdit.Font.Name  := 'Courier New';
      PRichEdit.Font.Size  := 10;
      PRichEdit.Font.Style := [];
    end;
    PRichEdit.SelAttributes.Color := clBlack;
    PRichEdit.SelAttributes.Name  := PRichEdit.Font.Name;
    PRichEdit.SelAttributes.Style := PRichEdit.Font.Style;
    PRichEdit.SelAttributes.Size  := PRichEdit.Font.Size;

    if PFormat then
    begin
      for I := 0 to vTWord do
      begin
        with PRichEdit do
        begin
          StartPos := 0;
          ToEnd := Length(Text) - StartPos;
          FoundAt := FindText(PReservedMatrix[I], StartPos, ToEnd, [stWholeWord]);
          while (FoundAt <> -1) do
          begin
            SelStart := FoundAt;
            SelLength := Length(PReservedMatrix[i]);
            SelAttributes.Style := [fsBold];
            StartPos := FoundAt + Length(PReservedMatrix[i]);
            FoundAt := FindText(PReservedMatrix[i], StartPos, ToEnd, [stWholeWord]);
          end;
        end;
      end;
    end;
  finally
    PRichEdit.SelStart := PosInicial;
    PRichEdit.SelAttributes.Color := clBlack;
    PRichEdit.SelAttributes.Style := [];
    LockWindowUpdate(0);
    FH.SetaCursorDef;
  end;
end;

{----------------------------------------------------------------------------------------------}
procedure LimpaValueListEditor(PVlLstEdt: TValueListEditor);
begin
  while PVlLstEdt.VisibleRowCount > 0 do
    try
      PVlLstEdt.DeleteRow(PVlLstEdt.VisibleRowCount);
    except
      try
        PVlLstEdt.Cells[0, 0] := '';
        PVlLstEdt.Cells[1, 0] := '';
      except

      end;
      break;
    end;
end;

{----------------------------------------------------------------------------------------------}
procedure DeletaArq(PDir, PArquivo: string);
var
  vSearchRec: TSearchRec;
  vRes: integer;
begin
  chDir(PDir);
  vRes := FindFirst(PArquivo, faAnyFile, vSearchRec);
  while vRes = 0 do
  begin
    PArquivo := vSearchRec.FindData.cFileName;
    DeleteFile(PChar(PArquivo));
    vRes := FindNext(vSearchRec);
  end;
end;

{----------------------------------------------------------------------------------------------}
function LocTStrings(PItem: TStrings; PStr: string): integer;
begin
  PStr := UpperCase(PStr);
  Result := 0;
  while Result < PItem.Count do
  begin
    if pos(PStr, UpperCase(PItem.Strings[Result])) > 0 then
      exit;
    Inc(Result);
  end;
  Result := -1;
end;

{----------------------------------------------------------------------------------------------}
function PreencheStr(const PDirecao: Shortint; const S: string; const PTam: SmallInt;
  const PChr: string = #32): string;
var
  tx: string;
begin
  tx := S;
  if Length(tx) > PTam then
    tx := Copy(tx, 1, PTam);
  Result := tx;
  case PDirecao of
    -1:
      begin
        while Length(Result) < PTam do
          Result := PChr + Result;
      end;
    1:
      begin
        while Length(Result) < PTam do
          Result := Result + PChr;
      end;
    0:
      begin
        while Length(Result) < (((PTam div 2) - (Length(tx) div 2)) + Length(tx)) do
          Result := PChr + Result;
        while Length(Result) < PTam do
          Result := Result + PChr;
      end;
  end;
end;

{----------------------------------------------------------------------------------------------}
function PreencheStr(const S: string; const PTam: SmallInt; const PChr: string = #32): string;
begin
  Result := FH.PreencheStr(-1, S, PTam, PChr);
end;

{----------------------------------------------------------------------------------------------}
function StrZero(const S: string; const PTam: SmallInt): string;
begin
  Result := FH.PreencheStr(-1, S, PTam, '0');
end;

{----------------------------------------------------------------------------------------------}
function StrZero(const N: integer; const PTam: SmallInt): string;
begin
  Result := FH.PreencheStr(-1, FloatToStr(N), PTam, '0');
end;

{----------------------------------------------------------------------------------------------}
function PreencheDir(const S: string; const PTam: SmallInt): string;
begin
  Result := FH.PreencheStr(1, S, PTam, #32);
end;

{----------------------------------------------------------------------------------------------}
function PreencheEsq(const S: string; const PTam: SmallInt): string;
begin
  Result := FH.PreencheStr(-1, S, PTam, #32);
end;

{----------------------------------------------------------------------------------------------}
procedure PadR(var PString: string; const PTam: integer; const PChr: string = ' ');
begin
  while Length(PString) < PTam do
    PString := PString + PChr;
end;

{----------------------------------------------------------------------------------------------}
procedure PadL(var PString: string; const PTam: integer; const PChr: string = ' ');
begin
  while Length(PString) < PTam do
    PString := PChr + PString;
end;

{----------------------------------------------------------------------------------------------}
procedure PadZeroL(var PString: string; const PTam: integer);
begin
  while Length(PString) < PTam do
    PString := '0' + PString;
end;

{----------------------------------------------------------------------------------------------}
procedure PadZeroR(var PString: string; const PTam: integer);
begin
  while Length(PString) < PTam do
    PString := PString + '0';
end;

{----------------------------------------------------------------------------------------------}
function ContaStr(S, PSubStr: string): integer;
begin
  Result := ContaStr(S, PSubStr, -1, -1);
end;

{----------------------------------------------------------------------------------------------}
function ContaStr(S, PSubStr: string; PIndex, PTotal: integer): integer; overload;
var
  vInd, I: integer;
  A: string;
begin
  vInd := 0;
  I := 0;
  if (PIndex < 0) and (PTotal < 0) then
  begin
    A := S;
    for vInd := 1 to Length(S) do
      if AnsiSameText(Copy(S, vInd, Length(PSubStr)), PSubStr) then
        Inc(I);
  end
  else
  begin
    if PTotal > Length(S) then
      PTotal := Length(S);
    if PIndex <= PTotal then
    begin
      for vInd := PIndex to PTotal do
        if AnsiSameText(Copy(S, vInd, Length(PSubStr)), PSubStr) then
          Inc(I);
    end;
  end;
  Result := I;
end;

{----------------------------------------------------------------------------------------------}
function GeraEspc(PTam: integer): string;
begin
  Result := DupeString(#32, PTam);
end;

{----------------------------------------------------------------------------------------------}
function LimpaChrEspecial(Str: string): string;
const
  cS1 = 'àâäãáêéëèíïìîôöõóòûúüùçñýÿÀÂÄÃÁÊÉËÈÍÏÌÎÔÖÕÓÒÛÚÜÙÇÑÝŸ';
  cS2 = 'aaaaaeeeeiiiiooooouuuucnyyAAAAAEEEEIIIIOOOOOUUUUCNYY';
var
  I: integer;
begin
  for I := 1 to Length(Str) do
    if Pos(Str[I], cS1) <> 0 then
      Str[I] := cS2[Pos(Str[I], cS1)];
  Result := Str;
end;

{----------------------------------------------------------------------------------------------}
function LimpaAaZ(PStr: string): string;
var
  ii: integer;
begin
  Result := '';
  for ii := 1 to Length(PStr) do
    if (FH.IntOk(PStr[ii])) or (PStr[ii] = ',') then
      Result := Result + PStr[ii];
end;

{----------------------------------------------------------------------------------------------}
function FloatOk(const S: string): Boolean;
var
  E: Extended;
begin
  Result := TryStrToFloat(S, E);
end;

{----------------------------------------------------------------------------------------------}
function IntOk(const S: string): Boolean;
var
  I: int64;
begin
  Result := TryStrToInt64(S, I);
end;

{----------------------------------------------------------------------------------------------}
function Int64Ok(const S: string): Boolean;
var
  I: Int64;
begin
  Result := TryStrToInt64(S, I);
end;

{----------------------------------------------------------------------------------------------}
function IntInvalido(const S: string): Boolean;
begin
  Result := not FH.IntOk(S);
end;

{----------------------------------------------------------------------------------------------}
function DataOk(const S: string): Boolean;
var
  D: TDateTime;
begin
  Result := TryStrToDate(S, D);
end;

{----------------------------------------------------------------------------------------------}
function DataHoraOk(const S: string): Boolean;
var
  D: TDateTime;
begin
  Result := TryStrToDateTime(S, D);
end;

{----------------------------------------------------------------------------------------------}
function DataInvalida(const S: string): Boolean;
begin
  Result := not FH.DataOk(S);
end;

{----------------------------------------------------------------------------------------------}
function HoraOk(const S: string): Boolean;
var
  T: TDateTime;
begin
  Result := TryStrToTime(S, T);
end;

{----------------------------------------------------------------------------------------------}
function LimpaZero(Texto: string): string;
var
  sTemp: string;
  ii, jj: integer;
begin
  ii := 0;
  jj := Length(Texto);
  while jj > ii do
  begin
    sTemp := Copy(Texto, 0, 1);
    if sTemp = '0' then
    begin
      Delete(Texto, 1, 1);
      Inc(ii);
    end
    else
    begin
      Result := Texto;
      exit;
    end;
  end;
  Result := Texto;
end;

{----------------------------------------------------------------------------------------------}
function TrimX(const S: string; A: string): string;
var
  I, L: integer;
begin
  L := Length(S);
  I := 1;
  A := Copy(A, 1, 1);
  if A = '' then
    A := ' ';
  while (I <= L) and (S[I] <= A) do
    Inc(I);
  if I > L then
    Result := ''
  else
  begin
    while S[L] <= A do
      Dec(L);
    Result := Copy(S, I, L - I + 1);
  end;
end;

{----------------------------------------------------------------------------------------------}
function UltimoDia(PAno, PMes: SmallInt): SmallInt;
begin
  Result := MonthDays[IsLeapYear(PAno), PMes];
end;

{----------------------------------------------------------------------------------------------}
function TerminaProcesso(PNomeExe: string): integer;
const
  PROCESS_TERMINATE = $0001;
var
  bTemp: BOOL;
  hnd  : THandle;
  tar  : TProcessEntry32;
begin
  Result     := 0;
  hnd        := CreateToolhelp32Snapshot(TH32CS_SNAPPROCESS, 0);
  tar.dwSize := Sizeof(tar);
  bTemp      := Process32First(hnd, tar);
  while integer(bTemp) <> 0 do
  begin
    if AnsiSameText(ExtractFileName(tar.szExeFile), PNomeExe)
    or AnsiSameText(tar.szExeFile, PNomeExe) then
      Result := integer(TerminateProcess(OpenProcess(PROCESS_TERMINATE, BOOL(0), tar.th32ProcessID), 0));
    bTemp := Process32Next(hnd, tar);
  end;
  CloseHandle(hnd);
end;

{----------------------------------------------------------------------------------------------}
function ProcessoRodando(PNomeExe: string): Boolean;
var
  bTemp: BOOL;
  hnd  : THandle;
  tar  : TProcessEntry32;
begin
  Result     := false;
  hnd        := CreateToolhelp32Snapshot(TH32CS_SNAPPROCESS, 0);
  tar.dwSize := Sizeof(tar);
  bTemp      := Process32First(hnd, tar);
  while integer(bTemp) <> 0 do
  begin
    if AnsiSameText(ExtractFileName(tar.szExeFile), PNomeExe) or
       AnsiSameText(tar.szExeFile, PNomeExe) then
    begin
      Result := true;
      Break;
    end;
    bTemp := Process32Next(hnd, tar);
  end;
  CloseHandle(hnd);
end;

{----------------------------------------------------------------------------------------------}
function ContaProcessosRodando(PNomeExe: string): integer;
var
  bTemp: BOOL;
  hnd  : THandle;
  tar  : TProcessEntry32;
begin
  Result     := 0;
  hnd        := CreateToolhelp32Snapshot(TH32CS_SNAPPROCESS, 0);
  tar.dwSize := Sizeof(tar);
  bTemp      := Process32First(hnd, tar);
  while integer(bTemp) <> 0 do
  begin
    if AnsiSameText(ExtractFileName(tar.szExeFile), PNomeExe) or
       AnsiSameText(tar.szExeFile, PNomeExe) then
      Inc(Result);
    bTemp := Process32Next(hnd, tar);
  end;
  CloseHandle(hnd);
end;

{----------------------------------------------------------------------------------------------}
function GetInfoVersao(const FileName: string): string;
const
  KeyBr: array[1..9] of string =
   ('Empresa', 'Descricao', 'Versao do Arquivo', 'Nome Interno', 'Copyright',
    'Nome Original do Arquivo', 'Nome do Produto', 'Versao do Produto',
    'Comentarios');
var
  Dummy           : THandle;
  BufferSize,
  Len             : integer;
  Buffer          : PChar;
  LoCharSet,
  HiCharSet       : Word;
  Translate,
  Return          : Pointer;
  StrFileInfo     : string;
begin
  Result := '';
  BufferSize := GetFileVersionInfoSize(PChar(FileName), Dummy);
  if BufferSize = 0 then
    exit;

  GetMem(Buffer, Succ(BufferSize));
  try
    if GetFileVersionInfo(PChar(FileName), 0, BufferSize, Buffer) then
      if VerQueryValue(Buffer, '\VarFileInfo\Translation', Translate, UINT(Len)) then
      begin
        LoCharSet := LoWord(Longint(Translate^));
        HiCharSet := HiWord(Longint(Translate^));
        StrFileInfo := Format('\StringFileInfo\0%x0%x\%s', [LoCharSet, HiCharSet, 'FileVersion']);
        if VerQueryValue(Buffer, PChar(StrFileInfo), Return, UINT(Len)) then
          Result := PChar(Return);
      end;
  finally
    FreeMem(Buffer, Succ(BufferSize));
  end;
end;

{----------------------------------------------------------------------------------------------}
function GetNumeroSerial(Drive: string): string;
var
  Serial, DirLen, Flags: DWord;
  DLabel: array[0..11] of Char;
begin
  Drive := ExtractFileDrive(Drive);
  if Pos(':', Drive) = 0 then
    Drive := Drive + ':';
  if Pos('\', Drive) = 0 then
    Drive := Drive + '\';
  try
    GetVolumeInformation(PChar(Drive), dLabel, 12, @Serial, DirLen,
      Flags, nil, 0);
    Result := IntToHex(Serial, 8);
  except
    Result := '';
  end;
end;

{----------------------------------------------------------------------------------------------}
function MsgBox(const pTexto, PCaption: string; AFlags: Longint = 0): integer;
var
  S: string;
begin
  S := PCaption;
  if S = '' then
    S := Application.Title;
  Result := Application.MessageBox(PChar(pTexto), PChar(S), AFlags);
  Screen.ActiveForm.Repaint;
end;

{----------------------------------------------------------------------------------------------}
procedure MsgA(const S: string);
begin
  FH.BlockInput(false);
  MessageDlg(S, mtWarning, [mbOK], -1);
end;

{----------------------------------------------------------------------------------------------}
procedure MsgA(const S: string; const Args: array of const);
begin
  FH.BlockInput(false);
  MessageDlg(Format(S, Args), mtWarning, [mbOK], -1);
end;

{----------------------------------------------------------------------------------------------}
procedure MsgE(const S: string);
begin
  FH.BlockInput(false);
  MessageDlg(S, mtError, [mbOK], -1);
end;

{----------------------------------------------------------------------------------------------}
procedure MsgE(const S: string; const Args: array of const);
begin
  FH.BlockInput(false);
  MessageDlg(Format(S, Args), mtError, [mbOK], -1);
end;

{----------------------------------------------------------------------------------------------}
procedure MsgI(const S: string);
begin
  FH.BlockInput(false);
  MessageDlg(S, mtInformation, [mbOK], -1);
end;

{----------------------------------------------------------------------------------------------}
procedure MsgI(const S: string; const Args: array of const);
begin
  FH.BlockInput(false);
  MessageDlg(Format(S, Args), mtInformation, [mbOK], -1);
end;

{----------------------------------------------------------------------------------------------}
function MsgSN(const S: string; PFlag: TFlagMsgSN = 0): integer;
var
  T: TMsgDlgType;
begin
  FH.BlockInput(false);

  case PFlag of
    0: T := mtConfirmation;
    1: T := mtError;
    2: T := mtWarning;
    3: T := mtInformation;
  else
    T := mtConfirmation;
  end;

  Result := MessageDlg(S, T, [mbYes, mbNo], -1);

  if Result <> mrYes then
    Result := 0
  else
    Result := 1;
end;

{----------------------------------------------------------------------------------------------}
function MsgSN(const S: string; const Args: array of const; PFlag: TFlagMsgSN = 0): integer;
begin
  Result := FH.MsgSN(Format(S, Args), PFlag);
end;

{----------------------------------------------------------------------------------------------}
function MsgP(const Msg: string; DlgType: integer): integer;
begin
  case dlgType of
    0: Result := FH.MsgSN(Msg);
    1: FH.MsgE(Msg);
    2: FH.MsgA(Msg);
    3: FH.MsgI(Msg);
  end;
end;

{----------------------------------------------------------------------------------------------}
procedure SetaStatusCheckListBox(PStatus: Boolean; CB: TCheckListBox);
var
  I: integer;
begin
  for I := 0 to CB.Items.Count - 1 do
    CB.Checked[I] := PStatus;
end;

{----------------------------------------------------------------------------------------------}
procedure InverteStatusCheckListBox(CB: TCheckListBox);
var
  I: integer;
begin
  for I := 0 to CB.Items.Count - 1 do
     CB.Checked[I] := not CB.Checked[I];
end;

{----------------------------------------------------------------------------------------------}
procedure InverteStatusListView(LV: TListView);
var
  I: integer;
begin
  if not LV.Checkboxes then
    exit;
  for I := 0 to LV.Items.Count - 1 do
    LV.Items[I].Checked := not LV.Items[I].Checked;
end;

{----------------------------------------------------------------------------------------------}
procedure SetaStatusListView(PStatus: Boolean; LV: TListView);
var
  I: integer;
begin
  if not LV.Checkboxes then
    exit;
  for I := 0 to LV.Items.Count - 1 do
    LV.Items[I].Checked := PStatus;
end;

{----------------------------------------------------------------------------------------------}
procedure LimpaArray(PArray: array of string; pValor: string = '');
var
  I: integer;
begin
  for I := Low(PArray) to High(PArray) do
    PArray[I] := pValor;
end;

{----------------------------------------------------------------------------------------------}
procedure LimpaArray(PArray: array of integer; pValor: integer = 0);
var
  I: integer;
begin
  for I := Low(PArray) to High(PArray) do
    PArray[I] := pValor;
end;

{----------------------------------------------------------------------------------------------}
procedure LimpaArray(PArray: array of double; pValor: double = 0);
var
  I: integer;
begin
  for I := Low(PArray) to High(PArray) do
    PArray[I] := pValor;
end;

{----------------------------------------------------------------------------------------------}
function ArrayPos(PArray: array of Boolean; pValor: Boolean): integer;
var
  I: integer;
begin
  Result := Low(PArray) - 1;
  for I := Low(PArray) to High(PArray) do
  begin
    if PArray[I] = pValor then
    begin
      Result := I;
      Break;
    end;
  end;
end;

{----------------------------------------------------------------------------------------------}
function ArrayPos(PArray: array of double; pValor: double): integer;
var
  I: integer;
begin
  Result := Low(PArray) - 1;
  for I := Low(PArray) to High(PArray) do
  begin
    if PArray[I] = pValor then
    begin
      Result := I;
      Break;
    end;
  end;
end;

{----------------------------------------------------------------------------------------------}
function ArrayPos(PArray: array of integer; pValor: integer): integer;
var
  I: integer;
begin
  Result := Low(PArray) - 1;
  for I := Low(PArray) to High(PArray) do
  begin
    if PArray[I] = pValor then
    begin
      Result := I;
      Break;
    end;
  end;
end;

{----------------------------------------------------------------------------------------------}
function ArrayPos(PArray: array of string; pValor: string): integer;
var
  I: integer;
begin
  Result := Low(PArray) - 1;
  for I := Low(PArray) to High(PArray) do
  begin
    if PArray[I] = pValor then
    begin
      Result := I;
      Break;
    end;
  end;
end;

{----------------------------------------------------------------------------------------------}
function FmtDataSqlSimples(PStrData: string): string;
begin
  Result := QuotedStr(SqlTimeStampToStr('mm/dd/yyyy', DateTimeToSqlTimeStamp(StrToDate(PStrData))));
end;

{----------------------------------------------------------------------------------------------}
function FmtDataSql(PStrData: string): string;
begin
  Result := FH.AspaDup(SqlTimeStampToStr('mm/dd/yyyy', DateTimeToSqlTimeStamp(StrToDate(PStrData))));
end;

{----------------------------------------------------------------------------------------------}
function FmtDataSql(PData: TDateTime): string;
begin
  Result := FH.FmtDataSql(DateToStr(PData));
end;

{----------------------------------------------------------------------------------------------}
function AspaDup(const S: string): string;
begin
  Result := AnsiQuotedStr(S, '"');
end;

{----------------------------------------------------------------------------------------------}
function AspaSimp(const S: string): string;
begin
  Result := AnsiQuotedStr(S, '''');
end;

{----------------------------------------------------------------------------------------------}
function FormataCNPJ(Codigo: string): string;
begin
  if Pos('.', Codigo) = 0 then
  begin
    if Length(Codigo) < 14 then
      Codigo := FH.StrZero(Codigo, 14);
    Insert('.', Codigo, 3);
    Insert('.', Codigo, 7);
    Insert('/', Codigo, 11);
    Insert('-', Codigo, 16);
  end;
  Result := Codigo;
end;

{----------------------------------------------------------------------------------------------}
function VerificaRegistry(const pKey, pNome: string; const PSubKeys: string = ''): Boolean;
var
  Reg: TRegistry;
  Local: string;
begin
  Local := Format('\%s\%s\%s%s', [vrgRoot, vrgSubRoot, pKey, PSubKeys]);
  Reg := TRegistry.Create;
  with TRegistry.Create do
  begin
    try
      RootKey := vrgHKeyDefault1;
      if OpenKey(Local, false) = false then
      begin
        RootKey := vrgHKeyDefault2;
        if not OpenKey(Local, false) then
          exit;
      end;
      if pNome = '' then
        Result := KeyExists(Local)
      else
        Result := ValueExists(pNome);
    finally
      Free;
    end;
  end;
end;

{----------------------------------------------------------------------------------------------}
procedure TrocaRegistry(const pKey, PNomeVl, PNomeNv: string; const PSubKeys: string = '');
var
  Reg: TRegistry;
  Local: string;
begin
  Local := Format('\%s\%s\%s%s', [vrgRoot, vrgSubRoot, pKey, PSubKeys]);
  Reg := TRegistry.Create;
  with TRegistry.Create do
  begin
    try
      RootKey := vrgHKeyDefault1;
      if OpenKey(Local, false) = false then
      begin
        RootKey := vrgHKeyDefault2;
        if not OpenKey(Local, false) then
          exit;
      end;
      RenameValue(PNomeVl, PNomeNv);
    finally
      Free;
    end;
  end;
end;

{----------------------------------------------------------------------------------------------}
procedure GravaRegistry(const pKey, pNome, PConteudo: string; const PSubKeys: string = '');
var
  Reg: TRegistry;
  Local: string;
begin
  Local := Format('\%s\%s\%s%s', [vrgRoot, vrgSubRoot, pKey, PSubKeys]);
  try
    Reg := TRegistry.Create;
    try
      with Reg do
      begin
        RootKey := vrgHKeyDefault1;
        OpenKey(Local, true);
        WriteString(pNome, PConteudo);
      end;
    except
      with Reg do
      begin
        RootKey := vrgHKeyDefault2;
        OpenKey(Local, true);
        WriteString(pNome, PConteudo);
      end;
    end;
  finally
    Reg.Free;
  end;
end;

{----------------------------------------------------------------------------------------------}
function LeRegistry(const pKey, pNome: string; const PSubKeys: string = ''): string;
var
  Reg: TRegistry;
  Local: string;
begin
  Result := '';
  Local := Format('\%s\%s\%s%s', [vrgRoot, vrgSubRoot, pKey, PSubKeys]);
  try
    Reg := TRegistry.Create;
    with Reg do
    begin
      RootKey := vrgHKeyDefault1;
      if OpenKey(Local, false) then
        Result := ReadString(pNome)
      else
      begin
        RootKey := vrgHKeyDefault2;
        if OpenKey(Local, false) then
          Result := ReadString(pNome);
      end;
    end;
  finally
    Reg.Free;
  end;
end;

{----------------------------------------------------------------------------------------------}
procedure GravaRegistryInt(pKey: TForm; pNome: string; PConteudo: integer);
begin
  FH.GravaRegistry(pKey.Name, pNome, IntToStr(PConteudo));
end;

{----------------------------------------------------------------------------------------------}
procedure GravaRegistryStr(pKey: TForm; pNome, PConteudo: string);
begin
  FH.GravaRegistry(pKey.Name, pNome, PConteudo);
end;

{----------------------------------------------------------------------------------------------}
procedure GravaRegistryBool(pKey: TForm; pNome: string; PConteudo: Boolean);
begin
  FH.GravaRegistry(pKey.Name, pNome, BoolToStr(PConteudo));
end;

{----------------------------------------------------------------------------------------------}
procedure GravaRegistryInt(pKey, pNome: string; PConteudo: integer);
begin
  FH.GravaRegistry(pKey, pNome, IntToStr(PConteudo));
end;

{----------------------------------------------------------------------------------------------}
procedure GravaRegistryStr(pKey, pNome, PConteudo: string);
begin
  FH.GravaRegistry(pKey, pNome, PConteudo);
end;

{----------------------------------------------------------------------------------------------}
procedure GravaRegistryBool(pKey, pNome: string; PConteudo: Boolean);
begin
  FH.GravaRegistry(pKey, pNome, BoolToStr(PConteudo));
end;

{----------------------------------------------------------------------------------------------}
procedure GravaRegistryReal(pKey: TForm; pNome: string; PConteudo: Extended);
begin
  FH.GravaRegistry(pKey.Name, pNome, FloatToStr(PConteudo));
end;

{----------------------------------------------------------------------------------------------}
procedure GravaRegistryReal(pKey, pNome: string; PConteudo: Extended);
begin
  FH.GravaRegistry(pKey, pNome, FloatToStr(PConteudo));
end;

{----------------------------------------------------------------------------------------------}
procedure GravaRegistryDate(pKey: TForm; pNome: string; PConteudo: TDateTime);
begin
  FH.GravaRegistry(pKey.Name, pNome, DateToStr(PConteudo));
end;

{----------------------------------------------------------------------------------------------}
procedure GravaRegistryDate(pKey, pNome: string; PConteudo: TDateTime);
begin
  FH.GravaRegistry(pKey, pNome, DateToStr(PConteudo));
end;

{----------------------------------------------------------------------------------------------}
function LeRegistryStr(pKey: TForm; pNome: string; PDef: string = ''): string;
begin
  Result := FH.LeRegistry(pKey.Name, pNome);
  if Result = '' then
    Result := PDef;
end;

{----------------------------------------------------------------------------------------------}
function LeRegistryInt(pKey: TForm; pNome: string; PDef: integer = 0): integer;
begin
  Result := StrToIntDef(FH.LeRegistry(pKey.Name, pNome), PDef);
end;

{----------------------------------------------------------------------------------------------}
function LeRegistryBool(pKey: TForm; pNome: string; PDef: Boolean = false):
  Boolean;
begin
  Result := StrToBoolDef(FH.LeRegistry(pKey.Name, pNome), PDef);
end;

{----------------------------------------------------------------------------------------------}
function LeRegistryStr(pKey, pNome: string; PDef: string = ''): string;
begin
  Result := FH.LeRegistry(pKey, pNome);
  if Result = '' then
    Result := PDef;
end;

{----------------------------------------------------------------------------------------------}
function LeRegistryInt(pKey, pNome: string; PDef: integer = 0): integer;
begin
  Result := StrToIntDef(FH.LeRegistry(pKey, pNome), PDef);
end;

{----------------------------------------------------------------------------------------------}
function LeRegistryBool(pKey, pNome: string; PDef: Boolean = false): Boolean;
begin
  Result := StrToBoolDef(FH.LeRegistry(pKey, pNome), PDef);
end;

{----------------------------------------------------------------------------------------------}
function LeRegistryReal(pKey: TForm; pNome: string; PDef: Extended = 0): Extended;
begin
  Result := StrToFloatDef(FH.LeRegistry(pKey.Name, pNome), PDef);
end;

{----------------------------------------------------------------------------------------------}
function LeRegistryReal(pKey, pNome: string; PDef: Extended = 0): Extended;
begin
  Result := StrToFloatDef(FH.LeRegistry(pKey, pNome), PDef);
end;

{----------------------------------------------------------------------------------------------}
function LeRegistryDate(pKey: TForm; pNome: string; PDef: TDateTime = 0): TDateTime;
begin
  if PDef = 0 then
    PDef := Date;
  Result := StrToDateDef(FH.LeRegistry(pKey.Name, pNome), PDef);
end;

{----------------------------------------------------------------------------------------------}
function LeRegistryDate(pKey, pNome: string; PDef: TDateTime = 0): TDateTime;
begin
  if PDef = 0 then
    PDef := Date;
  Result := StrToDateDef(FH.LeRegistry(pKey, pNome), PDef);
end;

{----------------------------------------------------------------------------------------------}
function LeRegistryGeral(pKey, pNome: string): string;
var
  Reg: TRegistry;
  Local: string;
begin
  Result := '';
  Local := Format('\%s\%s', [vrgRoot, pKey]);
  Reg := TRegistry.Create;
  with TRegistry.Create do
  begin
    try
      RootKey := vrgHKeyDefault1;
      if OpenKey(Local, false) then
        Result := ReadString(pNome)
      else
      begin
        RootKey := vrgHKeyDefault2;
        if OpenKey(Local, false) then
          Result := ReadString(pNome);
      end;
    finally
      Free;
    end;
  end;
end;

{----------------------------------------------------------------------------------------------}
procedure GravaRegistryGeral(pKey, pNome, PConteudo: string);
var
  Reg: TRegistry;
  Local: string;
begin
  Local := Format('\%s\%s', [vrgRoot, pKey]);
  Reg := TRegistry.Create;
  try
    with TRegistry.Create do
    begin
      RootKey := vrgHKeyDefault1;
      OpenKey(Local, true);
      WriteString(pNome, PConteudo);
      Free;
    end;
  except
    with TRegistry.Create do
    begin
      RootKey := vrgHKeyDefault2;
      OpenKey(Local, true);
      WriteString(pNome, PConteudo);
      Free;
    end;
  end;
end;

{----------------------------------------------------------------------------------------------}
function LimpaRegistry(pKey: string; PLimpaTudo: Boolean; pVal: string = '';
  const PSubKeys: string = ''): Boolean;
var
  Reg: TRegistry;
  Local: string;
  L: TStrings;
  I: integer;
begin
  Local := Format('\%s\%s\%s%s', [vrgRoot, vrgSubRoot, pKey, PSubKeys]);
  Reg := TRegistry.Create;
  with TRegistry.Create do
  begin
    try
      RootKey := vrgHKeyDefault1;
      if not OpenKey(Local, false) then
      begin
        RootKey := vrgHKeyDefault2;
        if not OpenKey(Local, false) then
          exit;
      end;

      if not PLimpaTudo then
        Result := DeleteValue(pVal)
      else
      begin
        try
          L := TStringList.Create;
          Result := true;
          GetValueNames(L);
          try
            for I := 0 to L.Count - 1 do
              if L[I] <> '' then
                DeleteValue(L[I]);
          except
            Result := false;
          end;
        finally
          L.Free
        end;
      end;
    finally
      Free;
    end;
  end;
end;

{----------------------------------------------------------------------------------------------}
function DeletaRegistry(pKey: string; const PDeletarValoresSeExistir: Boolean;
  const PSubKeys: string = ''): Boolean;
var
  Reg: TRegistry;
  Local: string;
begin
  Local := Format('\%s\%s\%s%s', [vrgRoot, vrgSubRoot, pKey, PSubKeys]);
  Reg := TRegistry.Create;
  with TRegistry.Create do
  begin
    try
      RootKey := vrgHKeyDefault1;
      if PDeletarValoresSeExistir then FH.LimpaRegistry(pKey, true);
      Result := DeleteKey(Local);
      if not Result then
      begin
        RootKey := vrgHKeyDefault2;
        if PDeletarValoresSeExistir then FH.LimpaRegistry(pKey, true);
        Result := DeleteKey(Local);
      end;
    finally
      Free;
    end;
  end;
end;

{----------------------------------------------------------------------------------------------}
function TrocaChaveRegistry(PVlKey, PNvKey: string): Boolean;
var
  Reg: TRegistry;
  OldLocal, NewLocal: string;
begin
  OldLocal := Format('\%s\%s\%s', [vrgRoot, vrgSubRoot, PVlKey]);
  NewLocal := Format('\%s\%s\%s', [vrgRoot, vrgSubRoot, PNvKey]);
  Reg := TRegistry.Create;
  with TRegistry.Create do
  begin
    try
      RootKey := vrgHKeyDefault1;
      MoveKey(OldLocal, NewLocal, true);
      Result := OpenKey(OldLocal, false);
      RootKey := vrgHKeyDefault2;
      MoveKey(OldLocal, NewLocal, true);
      Result := OpenKey(OldLocal, false);
    finally
      Free;
    end;
  end;
end;

{----------------------------------------------------------------------------------------------}
procedure CarregaRegistry(pKey: string; Lista: TStrings; const PSubKeys: string = '';
  const CarregaKeys: Boolean = False);
var
  Reg: TRegistry;
  Local: string;
begin
  Local := Format('\%s\%s\%s%s', [vrgRoot, vrgSubRoot, pKey, PSubKeys]);
  Reg := TRegistry.Create;
  with TRegistry.Create do
  begin
    try
      RootKey := vrgHKeyDefault1;
      if not OpenKey(Local, false) then
      begin
        RootKey := vrgHKeyDefault2;
        if not OpenKey(Local, false) then
          exit;
      end;
      if CarregaKeys then
        GetKeyNames(Lista)
      else
        GetValueNames(Lista);
    finally
      Free;
    end;
  end;
end;

{----------------------------------------------------------------------------------------------}
function TrataData(const S: string; PDef: string = ''): string;
begin
  Result := S;
  if not FH.DataOk(Result) then
    Result := PDef;
end;

{----------------------------------------------------------------------------------------------}
function TrataInt(const S: string; PDef: string = '0'): string;
begin
  Result := S;
  if not FH.IntOk(Result) then
    Result := PDef;
end;

{----------------------------------------------------------------------------------------------}
function TrataHora(const S: string; PDef: string = ''): string;
begin
  Result := S;
  if not FH.HoraOk(Result) then
    Result := PDef;
end;

{----------------------------------------------------------------------------------------------}
function Juntar(S: string; I: integer): string;
begin
  if Pos('^', S) > 0 then
  begin
    S := StringReplace(S, '^', '%d', [rfReplaceAll]);
    Result := Format(S, [I]);
  end
  else
    Result := S + IntToStr(I);
end;

{----------------------------------------------------------------------------------------------}
function GetDataArq(const pNome: string): TDateTime;
begin
  if not FileExists(pNome) then
    raise Exception.CreateFmt('O arquivo ''%s'' não existe', [pNome]);

  Result := FileDateToDateTime(FileAge(pNome));
end;

{----------------------------------------------------------------------------------------------}
function IncPathDel(const S: string): string;
begin
  Result := IncludeTrailingPathDelimiter(S);
end;

{----------------------------------------------------------------------------------------------}
function ZeroOk(const F: Extended): Boolean;
begin
  Result := FloatToStrF(F, ffFixed, 14, 2) = '0,00';
end;

{----------------------------------------------------------------------------------------------}
procedure SetaCursorAmp(const PTipo: TTipoAmpulheta = 0);
const
  C: array[TTipoAmpulheta] of TCursor = (crHourGlass, crAppStart, crSQLWait);
begin
  Screen.Cursor := C[PTipo];
end;

{----------------------------------------------------------------------------------------------}
procedure SetaCursorDef;
begin
  Screen.Cursor := crDefault;
end;

{----------------------------------------------------------------------------------------------}
function HojeStr: string;
begin
  Result := DateToStr(Date);
end;

{----------------------------------------------------------------------------------------------}
function OntemStr(Retrocesso: integer = 1): string;
begin
  Result := DateToStr(IncDay(Date, Retrocesso * -1))
end;

{----------------------------------------------------------------------------------------------}
function HoraExata: string;
begin
     Result := TimeToStr(SysUtils.Time);
end;

{----------------------------------------------------------------------------------------------}
function HoraStr(const PHora: TTime = 1): string;
var
  H: TTime;
begin
  if PHora = 1 then
    H := SysUtils.Time
  else
    H := PHora;
  Result := LeftStr(TimeToStr(H), 5);
end;

{----------------------------------------------------------------------------------------------}
function HoraStr(const PHora: string): string;
begin
     Result := FH.HoraStr(StrToTime(PHora));
end;

{----------------------------------------------------------------------------------------------}
function NowStr: string;
begin
     Result := FH.HoraStr;
end;

{----------------------------------------------------------------------------------------------}
function TimeStamp: string;
begin
     Result := DateTimeToStr(Now);
end;

{----------------------------------------------------------------------------------------------}
function UltimoMes: string;
begin
     Result := DateToStr(IncMonth(Date, -1));
end;

{----------------------------------------------------------------------------------------------}
function MsgFocalize(pCampo: TWinControl; const PMsg: string = ''; const MsgCustomizada: Boolean = false): Boolean;
var
  vMsg: string;
begin
  vMsg := IfThen(PMsg = '', FH.vrgDefErrMsg, PMsg);
  if vMsg = FH.DefErrMsg then
    if MsgCustomizada then
      if RightStr(vMsg, 1) = '.' then
        vMsg := FH.CopyMin(vMsg, 1); // remove o ponto final
  if MsgCustomizada then
  begin
    if pCampo.Hint <> '' then
      vMsg := vMsg + #32 + QuotedStr(pCampo.Hint);
  end;
  FH.Focalize(pCampo);
  FH.MsgA(vMsg);
  FH.SetaCursorDef;
  Result := true;
end;

{----------------------------------------------------------------------------------------------}
function MsgFocalizeSe(const PCondicao: Boolean; pCampo: TWinControl;
  const PMsg: string = ''; const MsgCustomizada: Boolean = false): Boolean;
begin
  Result := PCondicao;
  if Result then
    FH.MsgFocalize(pCampo, PMsg, MsgCustomizada);
end;

{----------------------------------------------------------------------------------------------}
function VerConteudo(pCampo: TControl; PTipoCampo: TTipoVar = tvNul;
  const PDisparaMsg: Boolean = true; const MsgCustomizada: Boolean = false): Boolean;
var
  I: integer;
  vMsg: string;
begin
  vMsg := vrgDefErrMsg;
  vrgDefErrMsg := DefErrMsg;

  if pCampo is TEdit then
  begin
    Result := true;
    if TEdit(pCampo).Text = '' then
    begin
      if PDisparaMsg then
        FH.MsgFocalize(TEdit(pCampo), vMsg, MsgCustomizada);
      Result := false;
    end
    else if (PTipoCampo = tvInt) or (PTipoCampo = tvFlo) then
    begin
      if ((PTipoCampo = tvInt) and
         (not FH.IntOk(TEdit(pCampo).Text))) or ((PTipoCampo = tvFlo) and
         (not FH.FloatOk(FH.limpaPonto(TEdit(pCampo).Text)))) then
      begin
        if PDisparaMsg then
          FH.MsgFocalize(TEdit(pCampo), vMsg, MsgCustomizada);
        Result := false;
      end;
    end;
  end
  else if pCampo is TCustomMaskEdit then
  begin
    Result := true;
    if (TCustomMaskEdit(pCampo).Text = '  /  /    ') or
       (TCustomMaskEdit(pCampo).Text = '') then
    begin
      if PDisparaMsg then
        FH.MsgFocalize(TCustomMaskEdit(pCampo), vMsg, MsgCustomizada);
      Result := false;
    end
    else if PTipoCampo = tvDat then
    begin
      if not FH.DataOk(TCustomMaskEdit(pCampo).Text) then
      begin
        if PDisparaMsg then
          FH.MsgFocalize(TCustomMaskEdit(pCampo), vMsg, MsgCustomizada);
        Result := false;
      end;
    end
    else if PTipoCampo = tvFlo then
    begin
      if not FH.FloatOk(FH.limpaPonto(TCustomMaskEdit(pCampo).Text)) then
      begin
        if PDisparaMsg then
          FH.MsgFocalize(TCustomMaskEdit(pCampo), vMsg, MsgCustomizada);
        Result := false;
      end;
    end;
  end
  else if pCampo is TComboBox then
  begin
    Result := true;
    if TComboBox(pCampo).ItemIndex = -1 then
    begin
      if PDisparaMsg then
        FH.MsgFocalize(TComboBox(pCampo), vMsg, MsgCustomizada);
      Result := false;
    end;
  end
  else if pCampo is TLabeledEdit then
  begin
    Result := true;
    if TLabeledEdit(pCampo).Text = '' then
    begin
      if PDisparaMsg then
        FH.MsgFocalize(TLabeledEdit(pCampo), vMsg, MsgCustomizada);
      Result := false;
    end
    else if (PTipoCampo = tvInt) or (PTipoCampo = tvFlo) then
    begin
      if ((PTipoCampo = tvInt) and
         (not FH.IntOk(TLabeledEdit(pCampo).Text))) or
         ((PTipoCampo = tvFlo) and
         (not FH.FloatOk(FH.limpaPonto(TLabeledEdit(pCampo).Text)))) then
      begin
        if PDisparaMsg then
          FH.MsgFocalize(TLabeledEdit(pCampo), vMsg, MsgCustomizada);
        Result := false;
      end;
    end;
  end
  else if pCampo is TDBEdit then
  begin
    Result := true;
    if TDBEdit(pCampo).Text = '' then
    begin
      if PDisparaMsg then
        FH.MsgFocalize(TDBEdit(pCampo), vMsg, MsgCustomizada);
      Result := false;
    end
    else if (PTipoCampo = tvInt) or (PTipoCampo = tvFlo) then
    begin
      if ((PTipoCampo = tvInt) and
         (not FH.IntOk(TDBEdit(pCampo).Text))) or
         ((PTipoCampo = tvFlo) and
         (not FH.FloatOk(TDBEdit(pCampo).Text))) then
      begin
        if PDisparaMsg then
          FH.MsgFocalize(TDBEdit(pCampo), vMsg, MsgCustomizada);
        Result := false;
      end;
    end;
  end
  else if pCampo is TCheckListBox then
  begin
    Result := true;
    for I := 0 to TCheckListBox(pCampo).Items.Count - 1 do
    begin
      Result := TCheckListBox(pCampo).Checked[I];
      if Result then
        Break;
    end;
    if (not Result) and (PDisparaMsg) then
    begin
      FH.MsgA('Não há ítens selecionados.');
      FH.Focalize(TCheckListBox(pCampo));
    end;
  end
  else if pCampo is TCustomMemo then
  begin
    Result := true;
    if TCustomMemo(pCampo).Lines.Text = '' then
    begin
      if PDisparaMsg then
        FH.MsgFocalize(TCustomMemo(pCampo), vMsg, MsgCustomizada);
      Result := false;
    end;
  end
end;

{----------------------------------------------------------------------------------------------}
function MultiVerConteudo(PCampos: array of TControl; PTipoCampo: array of TTipoVar;
  const PDisparaMsg: Boolean = true; const MsgCustomizada: Boolean = false): Boolean;
var
  I: integer;
  V: TTipoVar;
begin
  Result := true;
  for I := Low(PCampos) to High(PCampos) do
  begin
    if (I < Low(PTipoCampo)) or (I > High(PTipoCampo)) then
      V := tvNul
    else
      V := PTipoCampo[I];
    Result := FH.VerConteudo(PCampos[I], V, PDisparaMsg, MsgCustomizada);
    if not Result then
      exit;
  end;
end;

{----------------------------------------------------------------------------------------------}
procedure ListboxCopiaItem(PListBoxOrigem, PListBoxDestino: TListBox; PIndex: integer = -1);
begin
  with PListBoxOrigem do
  begin
    if PIndex = -1 then
      PIndex := ItemIndex;
    if Items.Count = 0 then
      exit;
    if PIndex = -1 then
      ItemIndex := TopIndex
    else
    begin
      if PListBoxDestino.Items.IndexOf(Items[PIndex]) = -1 then
        PListBoxDestino.Items.Add(Items[PIndex]);
      PListBoxDestino.ItemIndex := PListBoxDestino.Items.IndexOf(Items[PIndex]);
    end;
  end;
end;

{----------------------------------------------------------------------------------------------}
procedure ListboxCopiaItem(DS: TDataSet; PListBoxDestino: TListBox;
  PCampos: string = 'CODIGO;DESCRICAO');
var
  S: string;
  vCampo: array of string;
  I, P: integer;
begin
  if Length(PCampos) = 0 then
    exit;
  S := PCampos + ';';
  SetLength(vCampo, FH.ContaStr(S, ';', -1, -1));
  I := 0;
  while Pos(';', S) > 0 do
  begin
    P := Pos(';', S);
    vCampo[I] := Copy(S, 1, P - 1);
    Delete(S, 1, P);
    Inc(I);
  end;
  S := '';
  for I := 0 to High(vCampo) do
    S := S + DS.FieldByName(vCampo[I]).AsString + ' ';
  S := Trim(S);
  with PListBoxDestino do
  begin
    if Items.IndexOf(S) = -1 then
      Items.Add(S);
    ItemIndex := Items.IndexOf(S);
  end;
end;

{----------------------------------------------------------------------------------------------}
procedure ListboxDeletaItem(PListBox: TListBox; PIndex: integer = -1);
begin
  with PListBox do
  begin
    if PIndex = -1 then
      PIndex := ItemIndex;
    if Items.Count = 0 then
      exit;
    if PIndex = -1 then
      ItemIndex := TopIndex
    else
    begin
      Items.Delete(PIndex);
      if Items.Count > 0 then
      begin
        ItemIndex := PIndex - 1;
        if ItemIndex = -1 then
          ItemIndex := TopIndex;
      end;
    end;
  end;
end;

{----------------------------------------------------------------------------------------------}
procedure ForceEvent(PComp: TControl; PEvento: TNotifyEvent; PSender: TControl = nil);
begin
  if PSender = nil then
    PSender := PComp;
  with THookControl(PComp) do
    PEvento(PSender);
end;

{----------------------------------------------------------------------------------------------}
procedure ForceClick(PComp: TControl; PSender: TControl = nil);
begin
  FH.ForceEvent(PComp, THookControl(PComp).OnClick, PSender);
end;

{----------------------------------------------------------------------------------------------}
procedure ForceExit(PComp: TWinControl; PSender: TWinControl = nil);
begin
  FH.ForceEvent(PComp, THookWinControl(PComp).OnExit, PSender);
end;

{----------------------------------------------------------------------------------------------}
function Focalize(WC: TWinControl): Boolean;
var
  vParent: TCustomForm;
begin
  if WC = nil then
    exit;
  with WC do
  begin
    Result := CanFocus;
    if Result then
    begin
      vParent := GetParentForm(WC);
      if vParent <> nil then
      try
        vParent.FocusControl(WC);
      except
      end
      else if ParentWindow <> 0 then
        Windows.SetFocus(Handle)
      else
        //ValidParentForm(WC);
    end;
  end;
end;

{----------------------------------------------------------------------------------------------}
function Focalize(AHandle: HWnd): Boolean;
begin
  Result := Windows.SetFocus(AHandle) <> 0;
end;

{----------------------------------------------------------------------------------------------}
function FocalizeSe(const PCondicao: Boolean; PWnCtrTrue,
  PWnCtrFalse: TWinControl): Boolean;
begin
  Result := PCondicao;
  if Result then
    FH.Focalize(PWnCtrTrue)
  else
    FH.Focalize(PWnCtrFalse);
end;

{----------------------------------------------------------------------------------------------}
function RetForm(const FR: string): TForm;
var
  I: integer;
begin
  Result := nil;
  for I := 0 to Screen.FormCount - 1 do
    if AnsiSameText((Screen.Forms[I] as TForm).Name, FR) then
    begin
      Result := (Screen.Forms[I] as TForm);
      Break;
    end;
end;

{----------------------------------------------------------------------------------------------}
function RetFormComp(const FR, PCtr: string): TComponent;
begin
  Result := nil;
  try
    Result := FH.RetForm(FR).FindComponent(PCtr);
  except
    Result := nil;
  end;
end;

{----------------------------------------------------------------------------------------------}
function RetFormCds(const FR, PCtr: string): TClientDataSet;
begin
    Result := FH.RetFormComp(FR, PCtr) as TClientDataSet;
end;

{----------------------------------------------------------------------------------------------}
function RetFormControl(const FR, PCtr: string): TControl;
begin
  Result := TControl(FH.RetFormComp(FR, PCtr));
end;

{----------------------------------------------------------------------------------------------}
function RetFormControl(FR: TForm; const PCtr: string; const PIndex: integer): TControl;
begin
  Result := TControl(FH.RetFormComp(FR.Name, PCtr + IntToStr(PIndex)));
end;

{----------------------------------------------------------------------------------------------}
function RetFormWinControl(const FR, PCtr: string): TWinControl;
begin
  Result := TWinControl(FH.RetFormComp(FR, PCtr));
end;

{----------------------------------------------------------------------------------------------}
function RetFormEdit(const FR, PCtr: string): TEdit;
begin
  Result := TEdit(FH.RetFormComp(FR, PCtr));
end;

{----------------------------------------------------------------------------------------------}
function RetFormEdit(FR: TForm; const PCtr: string): TEdit;
begin
  Result := TEdit(FH.RetFormComp(FR.Name, PCtr));
end;

{----------------------------------------------------------------------------------------------}
function RetFormEdit(FR: TForm; const PCtr: string; const PIndex: integer): TEdit;
begin
  Result := TEdit(FH.RetFormComp(FR.Name, PCtr + IntToStr(PIndex)));
end;

{----------------------------------------------------------------------------------------------}
function RetFormButton(const FR, PCtr: string): TButton;
begin
  Result := TButton(FH.RetFormComp(FR, PCtr));
end;

{----------------------------------------------------------------------------------------------}
function RetFormPageControl(const FR, PCtr: string): TPageControl;
begin
  Result := TPageControl(FH.RetFormComp(FR, PCtr));
end;

{----------------------------------------------------------------------------------------------}
function RetFormImage(const FR, PCtr: string): TImage;
begin
  Result := TImage(FH.RetFormComp(FR, PCtr));
end;

{----------------------------------------------------------------------------------------------}
function RetFormLabel(const FR, PCtr: string): TLabel;
begin
  Result := TLabel(FH.RetFormComp(FR, PCtr));
end;

{----------------------------------------------------------------------------------------------}
function RetFormComboBox(const FR, PCtr: string): TComboBox;
begin
  Result := TComboBox(FH.RetFormComp(FR, PCtr));
end;

{----------------------------------------------------------------------------------------------}
function RetFormMaskEdit(const FR, PCtr: string): TMaskEdit;
begin
  Result := TMaskEdit(FH.RetFormComp(FR, PCtr));
end;

{----------------------------------------------------------------------------------------------}
function RetAtvFormComp(const PCtr: string): TComponent;
begin
  Result := nil;
  if Screen.ActiveForm = nil then
    exit;
  Result := TComponent(Screen.ActiveForm.FindComponent(PCtr));
end;

{----------------------------------------------------------------------------------------------}
function RetAtvFormEdit(const PCtr: string): TEdit;
begin
  Result := TEdit(FH.RetAtvFormComp(PCtr));
end;

{----------------------------------------------------------------------------------------------}
function RetAtvFormButton(const PCtr: string): TButton;
begin
  Result := TButton(FH.RetAtvFormComp(PCtr));
end;

{----------------------------------------------------------------------------------------------}
function RetAtvFormControl(const PCtr: string): TControl;
begin
  Result := TControl(FH.RetAtvFormComp(PCtr));
end;

{----------------------------------------------------------------------------------------------}
function RetAtvFormWinControl(const PCtr: string): TWinControl;
begin
  Result := TWinControl(FH.RetAtvFormControl(PCtr));
end;

{----------------------------------------------------------------------------------------------}
function RetAtvFormPageControl(const PCtr: string): TPageControl;
begin
  Result := TPageControl(FH.RetAtvFormControl(PCtr));
end;

{----------------------------------------------------------------------------------------------}
procedure GravaRegistryControl(pKey: TForm; AControl: TControl; PDefault: TPropRegistry = -1;
  const PSubKeys: string = '');
begin
  FH.ControlaRegistryControl(pKey, AControl, false, PDefault, PSubKeys);
end;

{----------------------------------------------------------------------------------------------}
procedure LeRegistryControl(pKey: TForm; AControl: TControl; PDefault: TPropRegistry = -1;
  const PSubKeys: string = '');
begin
  FH.ControlaRegistryControl(pKey, AControl, true, PDefault, PSubKeys);
end;

{----------------------------------------------------------------------------------------------}
procedure GravaRegistryMultiControl(pKey: TForm; PContainer: TWinControl; PDefault: TPropRegistry = -1;
  const PSubKeys: string = '');
var
  I: integer;
begin
  for I := 0 to PContainer.ControlCount - 1 do
    FH.GravaRegistryControl(pKey, TControl(PContainer.Controls[I]), PDefault, PSubKeys);
end;

{----------------------------------------------------------------------------------------------}
procedure LeRegistryMultiControl(pKey: TForm; PContainer: TWinControl; AUseDefault: Boolean = true;
  PDefault: TPropRegistry = -1; const PSubKeys: string = '');
var
  I: integer;
begin
  for I := 0 to PContainer.ControlCount - 1 do
    if (AUseDefault) or ((not AUseDefault) and
       (FH.VerificaRegistry(pKey.Name, TControl(PContainer.Controls[I]).Name, PSubKeys))) then
      FH.LeRegistryControl(pKey, TControl(PContainer.Controls[I]), PDefault, PSubKeys);
end;

{----------------------------------------------------------------------------------------------}
procedure ControlaRegistryControl(pKey: TForm; AControl: TControl; PCarrega: Boolean;
  PDefault: TPropRegistry = -1; const PSubKeys: string = '');
var
  S: string;
begin
  if AControl is TEdit then
  begin
    with TEdit(AControl) do
    begin
      if PCarrega then
      begin
        if PDefault = 4 then
        begin
          S := FH.LeRegistry(pKey.Name, Name, PSubKeys);
          if S <> '' then
            Text := S;
        end
        else
          Text := FH.LeRegistry(pKey.Name, Name, PSubKeys);
      end
      else
        FH.GravaRegistry(pKey.Name, Name, Text, PSubKeys);
    end;
  end
  else if AControl is TMemo then
  begin
    with TMemo(AControl) do
    begin
      if PCarrega then
      begin
        if PDefault = 4 then
        begin
          S := FH.LeRegistry(pKey.Name, Name, PSubKeys);
          if S <> '' then
            Lines.Text := S;
        end
        else
          Lines.Text := FH.LeRegistry(pKey.Name, Name, PSubKeys);
      end
      else
        FH.GravaRegistry(pKey.Name, Name, Lines.Text, PSubKeys);
    end;
  end
  else if AControl is TSpinEdit then
  begin
    with TSpinEdit(AControl) do
    begin
      if PCarrega then
      try
        Value := StrToInt(FH.LeRegistry(pKey.Name, Name, PSubKeys));
      except
      end
      else
        FH.GravaRegistry(pKey.Name, Name, IntToStr(Value), PSubKeys);
    end;
  end
  else if AControl is TCheckBox then
  begin
    with TCheckBox(AControl) do
    begin
      if PCarrega then
      begin
        S := FH.LeRegistry(pKey.Name, Name, PSubKeys);
        if S = '' then
          exit;
        if PDefault = 0 then
        try
          Checked := S = 'S';
        except
        end
        else
          Checked := StrToBoolDef(S, false);
      end
      else
      begin
        if PDefault = 0 then
          FH.GravaRegistry(pKey.Name, Name, IfThen(Checked, 'S', 'N'), PSubKeys)
        else
          FH.GravaRegistry(pKey.Name, Name, BoolToStr(Checked), PSubKeys);
      end;
    end;
  end
  else if AControl is TComboBox then
  begin
    with TComboBox(AControl) do
    begin
      if PCarrega then
      begin
        if PDefault = 1 then
        begin
          if Style = csDropDownList then
            FH.SetaComboIndex(TComboBox(AControl), FH.LeRegistry(pKey.Name, Name, PSubKeys))
          else
            Text := FH.LeRegistry(pKey.Name, Name, PSubKeys);
        end
        else
        if PDefault = 2 then
          Items.Text := FH.LeRegistry(pKey.Name, Name, PSubKeys)
        else
        if PDefault = 3 then
        begin
          S := FH.LeRegistry(pKey.Name, Name, PSubKeys);
          if S = '' then
            exit;
          Items.Text := Copy(S, Pos('Items.Text=', S) + 11, Length(S));
          S := Copy(S, Pos('ItemIndex=', S) + 10, Pos(';Items.Text=', S) - (Pos('ItemIndex=', S) + 10));
          ItemIndex := StrToIntDef(S, -1);
        end
        else
        begin
          S := FH.LeRegistry(pKey.Name, Name, PSubKeys);
          if (S = '') or (not FH.IntOk(S)) then
            exit;
          ItemIndex := StrToIntDef(S, -1);
        end;
      end
      else
      begin
        if PDefault = 1 then
          FH.GravaRegistry(pKey.Name, Name, Text, PSubKeys)
        else if PDefault = 2 then
          FH.GravaRegistry(pKey.Name, Name, Items.Text, PSubKeys)
        else if PDefault = 3 then
          FH.GravaRegistry(pKey.Name, Name, 'ItemIndex=' + IntToStr(ItemIndex) + ';Items.Text=' + Items.Text, PSubKeys)
        else
          FH.GravaRegistry(pKey.Name, Name, IntToStr(ItemIndex), PSubKeys);
      end;
    end;
  end
  else if AControl is TRadioButton then
  begin
    with TRadioButton(AControl) do
    begin
      if PCarrega then
      begin
        S := FH.LeRegistry(pKey.Name, Name, PSubKeys);
        if S = '' then
          exit;
        if PDefault = 0 then
        try
          Checked := S = 'S';
        except
        end
        else
          Checked := StrToBoolDef(S, false);
      end
      else
      begin
        if PDefault = 0 then
          FH.GravaRegistry(pKey.Name, Name, IfThen(Checked, 'S', 'N'), PSubKeys)
        else
          FH.GravaRegistry(pKey.Name, Name, BoolToStr(Checked), PSubKeys);
      end;
    end;
  end
  else if AControl is TListBox then
  begin
    with TListBox(AControl) do
    begin
      if PCarrega then
      begin
        if PDefault = 2 then
        begin
          Items.Text := FH.LeRegistry(pKey.Name, Name, PSubKeys);
        end
        else if PDefault = 4 then
        begin
          S := FH.LeRegistry(pKey.Name, Name, PSubKeys);
          if S <> '' then
            Items.Text := S;
        end
        else if PDefault = 3 then
        begin
          S := FH.LeRegistry(pKey.Name, Name, PSubKeys);
          if S = '' then
            exit;
          Items.Text := Copy(S, Pos('Items.Text=', S) + 11, Length(S));
          S := Copy(S, Pos('ItemIndex=', S) + 10, Pos(';Items.Text=', S) - (Pos('ItemIndex=', S) + 10));
          ItemIndex := StrToIntDef(S, -1);
        end
        else
        begin
          S := FH.LeRegistry(pKey.Name, Name, PSubKeys);
          if (S = '') or (not FH.IntOk(S)) then
            exit;
          ItemIndex := StrToIntDef(S, -1);
        end;
      end
      else
      begin
        if (PDefault = 2) or (PDefault = 4) then
          FH.GravaRegistry(pKey.Name, Name, Items.Text, PSubKeys)
        else if PDefault = 3 then
          FH.GravaRegistry(pKey.Name, Name,
            'ItemIndex=' + IntToStr(ItemIndex) + ';Items.Text=' + Items.Text, PSubKeys)
        else
          FH.GravaRegistry(pKey.Name, Name, IntToStr(ItemIndex), PSubKeys);
      end;
    end;
  end
  else if AControl is TRadioGroup then
  begin
    with TRadioGroup(AControl) do
    begin
      if PCarrega then
      try
        ItemIndex := StrToInt(FH.LeRegistry(pKey.Name, Name, PSubKeys));
      except
      end
      else
        FH.GravaRegistry(pKey.Name, Name, IntToStr(ItemIndex), PSubKeys);
    end;
  end
  else if AControl is TCustomMaskEdit then
  begin
    try
      with TCustomMaskEdit(AControl) do
      begin
        if PCarrega then
          Text := FH.LeRegistry(pKey.Name, Name, PSubKeys)
        else
          FH.GravaRegistry(pKey.Name, Name, Text, PSubKeys);
      end;
    except
      //
    end;
  end;
end;

{----------------------------------------------------------------------------------------------}
function RetCodigoStr(const S: string; const PSep: string = ' '): string;
begin
  Result := Trim(LeftStr(S, Pos(PSep, S) - Length(PSep)));
end;
 
{----------------------------------------------------------------------------------------------}
function RetDescriStr(const S: string; const PSep: string = ' '): string;
begin
  Result := Trim(Copy(S, Pos(PSep, S) + Length(PSep), MaxInt));
end;

{----------------------------------------------------------------------------------------------}
function RetSubCodigoStr(const S: string; const PSep: string = ' '): string;
begin
  Result := Trim(Copy(S, Pos(PSep, S) + Length(PSep), MaxInt));
  Result := Trim(LeftStr(Result, Pos(PSep, Result) - Length(PSep)));
end;

{----------------------------------------------------------------------------------------------}
procedure FocalizeFirst(PContainer: TObject);
begin
     THookWinControl(PContainer).SelectFirst;
end;

{----------------------------------------------------------------------------------------------}
procedure FocalizeFirstIF(const Valor: Boolean; PContainerT, PContainerF: TObject);
begin
     if Valor then
        FH.FocalizeFirst(PContainerT)
     else
        FH.FocalizeFirst(PContainerF);
end;

{----------------------------------------------------------------------------------------------}
procedure FocalizeFirstIn(PSender: TObject);
begin
  THookWinControl(TControl(PSender).Parent).SelectFirst;
end;

{----------------------------------------------------------------------------------------------}
function StrParaLista(POrigem: string): TStrings;
begin
  Result := TStringList.Create;
  POrigem := Trim(POrigem);
  if POrigem = '' then
    exit;

  ExtractStrings([','], [' '], PChar(POrigem), Result);
end;

{----------------------------------------------------------------------------------------------}
function ChamaExec(const F: string; FR: TForm): string;
begin
  Result := '';
  case ShellExecute(FR.Handle, nil, PChar(F), nil, nil, SW_SHOWNORMAL) of
    ERROR_FILE_NOT_FOUND   : Result := 'Arquivo especificado não encontrado.';
    ERROR_PATH_NOT_FOUND   : Result := 'Caminho especificado não encontrado.';
    SE_ERR_ASSOCINCOMPLETE : Result := 'A associação do nome de arquivo está incompleta ou é inválida.';
    SE_ERR_NOASSOC         : Result := 'Não há aplicação associada com a extensão do arquivo especificado.';
  end;
end;

{----------------------------------------------------------------------------------------------}
procedure ExecutaArq(const F: string; FR: TForm = nil);
var
  R: string;
begin
  if FR = nil then FR := Application.MainForm;
  R := ChamaExec(F, FR);
  if R <> '' then FH.MsgE(R);
end;

{----------------------------------------------------------------------------------------------}
function TemAaZ(const PStr: string): Boolean;
const
  Alpha = ['A'..'Z', 'a'..'z'];
var
  I: integer;
begin
  Result := Length(PStr) > 0;
  if not Result then
    exit;
  for I := 1 to Length(PStr) do
    if PStr[I] in Alpha then
      exit;
  Result := false;
end;

{----------------------------------------------------------------------------------------------}
function NaoTemAaZ(const PStr: string): Boolean;
begin
  Result := not FH.TemAaZ(PStr);
end;

{----------------------------------------------------------------------------------------------}
function RetroPos(PSubStr: string; S: string): integer;
begin
  Result := AnsiPos(ReverseString(PSubStr), ReverseString(S));
end;

{----------------------------------------------------------------------------------------------}
function RetroCopy(S: string; Index, Count: integer): string;
begin
  Result := ReverseString(Copy(ReverseString(S), Index, Count));
end;

{----------------------------------------------------------------------------------------------}
procedure RetroInsert(Substr: string; var Dest: string; Index: integer);
var
  D: string;
begin
  D := ReverseString(Dest);
  Insert(ReverseString(Substr), D, Index);
  Dest := ReverseString(D);
end;

{----------------------------------------------------------------------------------------------}
procedure DsnCanvas(PCanvas: TCanvas; PRect: TRect; DX, DY: integer; const PStr: string;
  PAlinhamento: TAlignment);
const
  AlinFlag: array[TAlignment] of integer =
  (DT_LEFT or DT_WORDBREAK or DT_EXPANDTABS or DT_NOPREFIX,
   DT_RIGHT or DT_WORDBREAK or DT_EXPANDTABS or DT_NOPREFIX,
   DT_CENTER or DT_WORDBREAK or DT_EXPANDTABS or DT_NOPREFIX);

  RTL: array[Boolean] of integer = (0, DT_RTLREADING);
  cReverseWriting: Boolean = false;
var
  B, R: TRect;
  vHld, Esquerda: integer;
  vRef: TColorRef;
  vBmp: TBitmap;
begin
  vRef := ColorToRGB(PCanvas.Brush.Color);
  if GetNearestColor(PCanvas.Handle, vRef) = vRef then
  begin
    if (PCanvas.CanvasOrientation = coRightToLeft) and (not cReverseWriting) then
      ChangeBiDiModeAlignment(PAlinhamento);
    case PAlinhamento of
      taLeftJustify: Esquerda := PRect.Left + DX;
      taRightJustify: Esquerda := PRect.Right - PCanvas.TextWidth(PStr) - 3;
    else { taCenter }
      Esquerda := PRect.Left + (PRect.Right - PRect.Left) shr 1 - (PCanvas.TextWidth(PStr) shr 1);
    end;
    PCanvas.TextRect(PRect, Esquerda, PRect.Top + DY, PStr);
  end
  else
  begin
    vBmp := TBitmap.Create;
    vBmp.Canvas.Lock;
    try
      with vBmp, PRect do
      begin
        Width := Max(Width, Right - Left);
        Height := Max(Height, Bottom - Top);

        R := Rect(DX, DY, Right - Left - 1, Bottom - Top - 1);
        B := Rect(0, 0, Right - Left, Bottom - Top);
      end;
      with vBmp.Canvas do
      begin
        Font := PCanvas.Font;
        Font.Color := PCanvas.Font.Color;
        Brush := PCanvas.Brush;
        Brush.Style := bsSolid;
        FillRect(B);
        SetBkMode(Handle, TRANSPARENT);

        if (PCanvas.CanvasOrientation = coRightToLeft) then
          ChangeBiDiModeAlignment(PAlinhamento);

        DrawText(Handle, PChar(PStr), Length(PStr), R, AlinFlag[PAlinhamento] or RTL[cReverseWriting]);
      end;
      if PCanvas.CanvasOrientation = coRightToLeft then
      begin
        vHld := PRect.Left;
        PRect.Left := PRect.Right;
        PRect.Right := vHld;
      end;
      PCanvas.CopyRect(PRect, vBmp.Canvas, B);
    finally
      vBmp.Canvas.Unlock;
      vBmp.Free;
    end;
  end;
end;

{----------------------------------------------------------------------------------------------}
procedure GridDsnStr(GR: TDBGrid; const R: TRect; PColuna: TColumn; const PStr: string);
begin
  FH.DsnCanvas(GR.Canvas, R, 2, 2, PStr, PColuna.Alignment);
end;

{----------------------------------------------------------------------------------------------}
procedure StringGridDsnStr(SG: TStringGrid; const R: TRect; PAlinhamento: integer; const PStr: string);
var
  A: TAlignment;
begin
  case PAlinhamento of
    0: A := taCenter;
    1: A := taRightJustify;
    -1: A := taLeftJustify;
  end;
  FH.DsnCanvas(SG.Canvas, R, 2, 2, PStr, A);
end;

{----------------------------------------------------------------------------------------------}
procedure GridDsnFntCel(GR: TDBGrid; const R: TRect; DataCol: integer; PColuna: TColumn;
  PState: TGridDrawState; PColor: TColor; PEstiloFonte: TFontStyles; PVerificaFoco: Boolean = true);
begin
  with GR.Canvas do
  begin
    Font.Style := PEstiloFonte;
    Font.Color := PColor;
    if gdSelected in PState then
      Font.Color := clHighlightText;
    if (PVerificaFoco) and (not GR.Focused) then
      Font.Color := PColor;
    FillRect(R);
  end;
  GR.DefaultDrawColumnCell(R, DataCol, PColuna, PState);
end;

{----------------------------------------------------------------------------------------------}
procedure GridDsnCel(GR: TDBGrid; const R: TRect; DataCol: integer; PColuna: TColumn;
  PState: TGridDrawState; PCorFonte, PCorBrush: TColor; PEstiloFonte: TFontStyles;
  PVerificaFoco: Boolean = true; PCorSelecionada: TColor = clHighlightText);
begin
  with GR.Canvas do
  begin
    Font.Style := PEstiloFonte;
    Font.Color := PCorFonte;
    Brush.Color := PCorBrush;
    if gdSelected in PState then
    begin
      Font.Color := PCorSelecionada;
      if PVerificaFoco then
        Brush.Color := clHighlight;
    end;
    if (PVerificaFoco) and (not GR.Focused) and (not (gdSelected in PState)) then
      Font.Color := PCorFonte;
    FillRect(R);
  end;
  GR.DefaultDrawColumnCell(R, DataCol, PColuna, PState);
end;

{----------------------------------------------------------------------------------------------}
function GetTipoArq(const PArquivo: string): string;
var
  vInfo: TSHFileInfo;
begin
  if SHGetFileInfo(PChar(PArquivo), 0, vInfo, Sizeof(vInfo), SHGFI_TYPENAME) <> 0 then
    Result := StrPas(vInfo.szTypeName)
  else
  begin
    Result := ExtractFileExt(PArquivo);
    Delete(Result, 1, 1);
    Result := Result + ' File';
  end;
  if Result = '' then
    Result := 'Arquivo ' + ExtractFileExt(PArquivo);
end;

{----------------------------------------------------------------------------------------------}
procedure CriptoArq(ArqOrg, ArqDst: string; pKey: Word);
var
  msOrg, msDes: TMemoryStream;
  I: integer;
  C: Byte;
begin
  msOrg := TMemoryStream.Create;
  msDes := TMemoryStream.Create;
  try
    msOrg.LoadFromFile(ArqOrg);
    msOrg.Position := 0;
    for I := 0 to msOrg.Size - 1 do
    begin
      msOrg.Read(C, 1);
      C := C xor not (Ord(pKey shr I));
      msDes.Write(C, 1);
    end;
    msDes.SaveToFile(ArqDst);
  finally
    msOrg.Free;
    msDes.Free;
  end;
end;

{----------------------------------------------------------------------------------------------}
function FonteParaStr(const F: TFont): string;

  function RetStyle: string;
  begin
    Result := '';
    if fsBold in F.Style then
      Result := Result + 'fsBold,';
    if fsItalic in F.Style then
      Result := Result + 'fsItalic,';
    if fsUnderline in F.Style then
      Result := Result + 'fsUnderline,';
    if fsStrikeout in F.Style then
      Result := Result + 'fsStrikeout,';
    if Result = '' then
      exit;
    Delete(Result, Length(Result), 1);
  end;
  function RetPitch: string;
  begin
    Result := 'fpDefault';
    if F.Pitch = fpVariable then
      Result := 'fpVariable'
    else if F.Pitch = fpFixed then
      Result := 'fpFixed';
  end;

var
  L: TStrings;
begin
  L := TStringList.Create;
  with F do
  begin
    L.Add(Format('Charset:%d', [Charset]));
    L.Add(Format('Color:%s', [ColorToString(Color)]));
    L.Add(Format('Height:%d', [Height]));
    L.Add(Format('Name:%s', [Name]));
    L.Add(Format('Pitch:%s', [RetPitch]));
    L.Add(Format('Size:%d', [Size]));
    L.Add(Format('Style:%s', [RetStyle]));
  end;
  Result := L.Text;
end;

{----------------------------------------------------------------------------------------------}
function StrParaFonte(const S: string; PDefault: TFont): TFont;
var
  L: TStrings;
  I: integer;

  function RetVal(S: string): string;
  begin
    Result := RightStr(S, FH.RetroPos(':', S) - 1);
  end;
  function RetID(S: string): string;
  begin
    Result := LeftStr(S, Pos(':', S) - 1);
  end;
  function RetPitch: TFontPitch;
  var
    S: string;
  begin
    S := RetVal(L[I]);
    if S = 'fpVariable' then
      Result := fpVariable
    else if S = 'fpFixed' then
      Result := fpFixed
    else
      Result := fpDefault;
  end;
  function RetStyle: TFontStyles;
  var
    S: string;
  begin
    S := RetVal(L[I]);
    Result := [];
    if Pos('fsBold', S) > 0 then
      Result := Result + [fsBold];
    if Pos('fsItalic', S) > 0 then
      Result := Result + [fsItalic];
    if Pos('fsUnderline', S) > 0 then
      Result := Result + [fsUnderline];
    if Pos('fsStrikeout', S) > 0 then
      Result := Result + [fsStrikeout];
  end;

begin
  L := TStringList.Create;
  L.Text := S;
  Result := TFont.Create;
  for I := 0 to L.Count - 1 do
  begin
    if RetID(L[I]) = 'Charset' then
      Result.Charset := StrToIntDef(RetVal(L[I]), 0)
    else if RetID(L[I]) = 'Color' then
    begin
      try
        Result.Color := StringToColor(RetVal(L[I]));
      except
        Result.Color := clWindowText;
      end;
    end
    else if RetID(L[I]) = 'Height' then
      Result.Height := StrToIntDef(RetVal(L[I]), -11)
    else if RetID(L[I]) = 'Name' then
    begin
      Result.Name := RetVal(L[I]);
      if Result.Name = '' then
        Result.Name := 'MS Sans Serif';
    end
    else if RetID(L[I]) = 'Pitch' then
      Result.Pitch := RetPitch
    else if RetID(L[I]) = 'Size' then
      Result.Size := StrToIntDef(RetVal(L[I]), 8)
    else if RetID(L[I]) = 'Style' then
      Result.Style := RetStyle;
  end;
  if (Result = nil) or (L.Count = 0) then
    Result := PDefault;
end;

{----------------------------------------------------------------------------------------------}
function RetDirSis: string;
begin
  Result := FH.IncPathDel(ExtractFilePath(Application.ExeName));
end;

{----------------------------------------------------------------------------------------------}
function RetDataSis: TDateTime;
begin
  Result := FileDateToDateTime(FileAge(Application.ExeName));
end;

{----------------------------------------------------------------------------------------------}
function ClonaArq(ArqOrg, ArqDst: string; ASobrepor: Boolean = false): Boolean;
begin
  if (FileExists(ArqDst)) and (not ASobrepor) then
    raise Exception.CreateFmt('Não foi possível criar o arquivo %s', [ArqDst]);

  Result := Windows.CopyFile(PChar(ArqOrg), PChar(ArqDst), not ASobrepor);
end;

{----------------------------------------------------------------------------------------------}
function DeletaExt(const S: string): string;
const
  E = '.MIB';
begin
  Result := ChangeFileExt(S, E);
  Result := LeftStr(Result, Pos(E, Result) - 1);
end;

{----------------------------------------------------------------------------------------------}
procedure Navegar(const Endereco: string);
var
  W: array[0..100] of WideChar;
begin
  if Trim(Endereco) = '' then
    exit;
  hLinkNavigateString(nil, StringToWideChar(Endereco, W, Length(Endereco) + 1));
end;

{----------------------------------------------------------------------------------------------}
procedure Navegar(FR: TForm; const Endereco: string);
begin
  if Trim(Endereco) = '' then
    exit;
  try
    FH.SetaCursorAmp;
    ShellExecute(FR.Handle, 'open', PChar(Endereco), nil, nil, SW_SHOWNORMAL);
  finally
    FH.SetaCursorDef;
  end;
end;

{----------------------------------------------------------------------------------------------}
procedure Explorar(const Caminho: string);
begin
     WinExec(PChar('explorer.exe ' + Caminho), SW_SHOWMAXIMIZED);
end;

{----------------------------------------------------------------------------------------------}
procedure EnviaEmail(const Endereco: string);
var
  E: PChar;
begin
  try
    FH.SetaCursorAmp;
    E := PChar('mailto:' + Trim(Endereco));
    ShellExecute(Application.Handle, 'Open', E, nil, nil, SW_SHOWNORMAL);
  finally
    FH.SetaCursorDef;
  end;
end;

{----------------------------------------------------------------------------------------------}
function ContainerFocalizado(PContainer: TWinControl): Boolean;
var
  I: integer;
begin
  Result := false;
  for I := 0 to PContainer.ControlCount - 1 do
    if PContainer.Controls[I] is TWinControl then
      if TWinControl(PContainer.Controls[I]).Focused then
      begin
        Result := true;
        Break;
      end;
end;

{----------------------------------------------------------------------------------------------}
function GetClockCPU: double;
const
  cInt = 500;
  cBit = $200000;
var
  vHigh, vLow: DWORD;
  vPrdClasse, vPrd: integer;
begin
  try
    vPrdClasse := GetPriorityClass(GetCurrentProcess);
    vPrd := GetThreadPriority(GetCurrentThread);
    SetPriorityClass(GetCurrentProcess, REALTIME_PRIORITY_CLASS);
    SetThreadPriority(GetCurrentThread, THREAD_PRIORITY_TIME_CRITICAL);

    Sleep(10);
    asm
          DW 310Fh
          MOV vLow, EAX
          MOV vHigh, EDX
    end;
    Sleep(cInt);
    asm
          DW 310Fh
          SUB EAX, vLow
          SBB EDX, vHigh
          MOV vLow, EAX
          MOV vHigh, EDX
    end;
    SetThreadPriority(GetCurrentThread, vPrd);
    SetPriorityClass(GetCurrentProcess, vPrdClasse);
    Result := vLow / (1000.0 * cInt);
  except
  end;
end;

{----------------------------------------------------------------------------------------------}
procedure CriaLista(var L: TStrings; const pTexto: string = '');
begin
  L      := TStringList.Create;
  L.Text := pTexto;
end;

{----------------------------------------------------------------------------------------------}
function LimpaString(const S: string; PChr: array of string): string;
var
  I: integer;
begin
  Result := S;
  for I := Low(PChr) to High(PChr) do
    while Pos(PChr[I], Result) > 0 do
      Delete(Result, Pos(PChr[I], Result), Length(PChr[I]));
end;

{----------------------------------------------------------------------------------------------}
function StrParaNum(const S: string; PDefault: Extended = MinExtended): Extended;
begin
  if not TextToFloat(PChar(FH.LimpaString(Trim(S), ['.'])), Result, fvExtended) then
    if PDefault = MinExtended then
      raise Exception.CreateFmt('''%s'' não é um valor válido', [S])
    else
      Result := PDefault;
end;

{----------------------------------------------------------------------------------------------}
function LimpaEspc(const S: string): string;
begin
  Result := FH.LimpaString(S, [' ']);
end;

{----------------------------------------------------------------------------------------------}
function LimpaPonto(const S: string): string;
begin
  Result := FH.LimpaString(S, ['.']);
end;

{----------------------------------------------------------------------------------------------}
function LimpaVrgl(const S: string): string;
begin
  Result := FH.LimpaString(S, [',']);
end;

{----------------------------------------------------------------------------------------------}
function LimpaBreak(const S: string; const PSubChar: string = ' '): string;
begin
  Result := StringReplace(StringReplace(S, #$D, PSubChar, [rfReplaceAll]), #$A, PSubChar, [rfReplaceAll]);
end;

{----------------------------------------------------------------------------------------------}
function TrocaBreaks(const pTexto: string; const TrocaQuebra: Boolean): string;
begin
  if TrocaQuebra then
    Result := StringReplace(pTexto, #$D#$A, '<BR>', [rfReplaceAll])
  else
    Result := StringReplace(pTexto, '<BR>', #$D#$A, [rfReplaceAll]);
end;

{----------------------------------------------------------------------------------------------}
function TrocaPathArq(const POrigem, PDestino: string): string;
var
  N: string;
begin
  Result := '';
  Result := ExtractFileName(PDestino);
  if Result = '' then
    Result := ExtractFileName(POrigem);
  N := ExtractFilePath(PDestino);
  if N = '' then
    Result := POrigem
  else
    Result := FH.IncPathDel(N) + Result;
end;

{----------------------------------------------------------------------------------------------}
function TrocaNomeArq(const POld, PNew: string): string;
var
  N: string;
begin
  Result := '';
  N := ExtractFilePath(POld);
  if N <> '' then
    N := FH.IncPathDel(N);
  Result := N + ExtractFileName(PNew);
end;

{----------------------------------------------------------------------------------------------}
function CopyPos(const PStr, PSubStr: string; const PTrim: Boolean = true): string;
begin
  if FH.GetPos(PSubStr, PStr) > 0 then
    Result := Copy(PStr, 1, FH.GetPos(PSubStr, PStr)-1)
  else
    Result := PStr;
  if PTrim then
    Result := Trim(Result);
end;

{----------------------------------------------------------------------------------------------}
function StrScan(const PStr: string; Chr: Char; PInit: integer = 1): string;
var
  P: PChar;
begin
  Result := '';
  P := SysUtils.StrScan(PChar(PStr), Chr);
  if P <> nil then
  begin
    Result := string(P);
    if PInit > 1 then
      Result := Copy(Result, PInit, MaxInt);
  end;
end;

{----------------------------------------------------------------------------------------------}
function FmtNum(pVal: Extended; pDec: Byte = 2): string;
begin
  Result := FloatToStrF(pVal, ffNumber, 14, pDec);
end;

{----------------------------------------------------------------------------------------------}
function FmtFix(pVal: Extended; pDec: Byte = 2): string;
begin
  Result := FloatToStrF(pVal, ffFixed, 14, pDec);
end;

{----------------------------------------------------------------------------------------------}
function FmtNumStr(pVal: string; pDec: Byte = 2; PDefault: Extended = 0): string;
begin
  Result := FloatToStrF(StrToFloatDef(pVal, PDefault), ffNumber, 14, pDec);
end;

{----------------------------------------------------------------------------------------------}
function FmtFixStr(pVal: string; pDec: Byte = 2; PDefault: Extended = 0): string;
begin
  Result := FloatToStrF(StrToFloatDef(pVal, PDefault), ffFixed, 14, pDec);
end;

{----------------------------------------------------------------------------------------------}
function FmtNumParaFix(pVal: string; pDec: Byte = 2; PDefault: Extended = 0): string;
begin
  Result := FloatToStrF(StrToFloatDef(FH.LimpaString(pVal, [ThousandSeparator, #$D#$A]), PDefault),
                        ffFixed, 14, pDec);
end;

{----------------------------------------------------------------------------------------------}
function StrToFloatFix(pVal: string): Extended;
begin
  Result := StrToFloat(FmtNumParaFix(pVal));
end;

{----------------------------------------------------------------------------------------------}
function FmtValSQL(pVal: Extended): string;
var
  oldSep: Char;
begin
  oldSep := DecimalSeparator;
  try
    DecimalSeparator := '.';
    Result := FloatToStr(pVal);
  finally
    DecimalSeparator := oldSep;
  end;
end;

{----------------------------------------------------------------------------------------------}
function RetValSql(const pValor: string; const pUpperCase: Boolean): string;
begin
     Result := quotedStr(pValor);
     if pUpperCase then
        Result := AnsiUpperCase(Result);
end;

{----------------------------------------------------------------------------------------------}
function RetValSql(const pValor: string): string;
begin
     Result := quotedStr(pValor);
end;

{----------------------------------------------------------------------------------------------}
function RetValSql(const pValor: Extended): string;
begin
     Result := FH.FmtValSQL(pValor);
end;

{----------------------------------------------------------------------------------------------}
function RetValSql(const pValor: TDateTime): string;
begin
     Result := QuotedStr(stringReplace(datetostr(pValor),'/','.',[rfReplaceAll]));
end;

{----------------------------------------------------------------------------------------------}
function FmtInt(pVal: integer): string;
begin
  Result := FloatToStrF(pVal, ffNumber, 14, 0);
end;

{----------------------------------------------------------------------------------------------}
function DiaAtual: integer;
begin
  Result := DayOf(Today);
end;

{----------------------------------------------------------------------------------------------}
function MesAtual: integer;
begin
  Result := MonthOf(Today);
end;

{----------------------------------------------------------------------------------------------}
function AnoAtual: integer;
begin
  Result := CurrentYear;
end;

{----------------------------------------------------------------------------------------------}
function DiaAtualStr: string;
begin
  Result := FH.StrZero(DayOf(Today), 2);
end;

{----------------------------------------------------------------------------------------------}
function MesAtualStr: string;
begin
  Result := FH.StrZero(MonthOf(Today), 2);
end;

{----------------------------------------------------------------------------------------------}
function AnoAtualStr: string;
begin
  Result := FH.StrZero(CurrentYear, 4);
end;

{----------------------------------------------------------------------------------------------}
function MesAtualExtenso: string;
begin
  Result := FH.SetaUpper(FormatDateTime('mmmm', Today), 1);
end;

{----------------------------------------------------------------------------------------------}
function RetMesStr(const PData: TDateTime = 0): string;
var
  D: TDateTime;
begin
  D := PData;
  if D = 0 then
    D := Date;
  Result := FormatDateTime('mm', D);
end;

{----------------------------------------------------------------------------------------------}
function RetMesStr(const PData: string): string;
begin
  Result := FormatDateTime('mm', StrToDate(PData));
end;

{----------------------------------------------------------------------------------------------}
function GetMesAno(const PData: TDateTime = 0): string;
var
  D: TDateTime;
begin
  D := PData;
  if D = 0 then
    D := Date;
  Result := FormatDateTime('mm/yyyy', D);
end;

{----------------------------------------------------------------------------------------------}
function GetMesAno(const PData: string): string;
begin
  Result := FormatDateTime('mm/yyyy', StrToDate(PData));
end;

{----------------------------------------------------------------------------------------------}
function MontaDataMesAno(const PMesAno: string; const PDia: Smallint = 1): TDateTime;
var
  M, A, D: integer;
begin
  M := StrToInt(FH.CopyPos(PMesAno, '/'));
  A := StrToInt(Copy(PMesAno, Pos('/', PMesAno) + 1, 4));
  D := PDia;
  if D = -1 then
    D := FH.UltimoDia(A, M);
  Result := EncodeDate(A, M, D);
end;

{----------------------------------------------------------------------------------------------}
function MontaDataStrMesAno(const PMesAno: string; const PDia: Smallint = 1): string;
begin
  Result := DateToStr(FH.MontaDataMesAno(PMesAno, PDia));
end;

{----------------------------------------------------------------------------------------------}
function ExtraiAnoMesCR(const PData: TDateTime = 0): string;
var
  D: TDateTime;
begin
  D := PData;
  if D = 0 then
    D := Date;
  Result := FormatDateTime('yyyymm', D);
end;

{----------------------------------------------------------------------------------------------}
function ExtraiAnoMesCR(const PData: string): string;
begin
  Result := FormatDateTime('yyyymm', StrToDate(PData));
end;

{----------------------------------------------------------------------------------------------}
function MontaDataAnoMesCR(const PAnoMes: string; const PDia: Smallint = 1): TDateTime;
begin
  Result := EncodeDate(StrToInt(LeftStr(PAnoMes, 4)), StrToInt(FH.RetroCopy(PAnoMes, 1, 2)), PDia);
end;

{----------------------------------------------------------------------------------------------}
function MontaDataStrAnoMesCR(const PAnoMes: string; const PDia: Smallint = 1): string;
begin
  Result := DateToStr(FH.MontaDataAnoMesCR(PAnoMes, PDia));
end;

{----------------------------------------------------------------------------------------------}
function TrocaDiaStr(const PData: TDateTime = 0; const PDia: Smallint = 1): string;
var
  D: TDateTime;
begin
  D := PData;
  if D = 0 then
    D := Date;
  Result := DateToStr(EncodeDate(YearOf(D), MonthOf(D), PDia));
end;

{----------------------------------------------------------------------------------------------}
function TrocaDiaStr(const PData: string; const PDia: Smallint = 1): string;
var
  D: TDateTime;
begin
  D := StrToDate(PData);
  Result := DateToStr(EncodeDate(YearOf(D), MonthOf(D), PDia));
end;

{----------------------------------------------------------------------------------------------}
function UltimoDia(const PData: TDateTime = 0): integer;
var
  D: TDateTime;
begin
  D := PData;
  if D = 0 then
    D := Date;
  Result := FH.UltimoDia(YearOf(D), MonthOf(D));
end;

{----------------------------------------------------------------------------------------------}
function UltimoDia(const PData: string): integer;
var
  D: TDateTime;
begin
  D := StrToDate(PData);
  Result := FH.UltimoDia(YearOf(D), MonthOf(D));
end;

{----------------------------------------------------------------------------------------------}
function GetPos(Substr, Str: string): integer;
begin
  Result := AnsiPos(AnsiLowerCase(Substr), AnsiLowerCase(Str));
end;

{----------------------------------------------------------------------------------------------}
function MontaNomeArq(const PPath, pNome: string; PExtensao: string = ''): string;
var
  C, N: string;
begin
  C := FH.IncPathDel(ExtractFilePath(PPath));
  N := ExtractFileName(pNome);
  if PExtensao <> '' then
    N := FH.IncExt(N, PExtensao);
  Result := C + N;
end;

{----------------------------------------------------------------------------------------------}
procedure PreparaProcPgb(var PProgBar: TProgressBar; const PMax: integer;
  const PVisible: Boolean; const AStep: integer = 1);
begin
  with PProgBar do
  begin
    Max := PMax;
    Step := AStep;
    Position := 0;
    Visible := PVisible;
  end;
end;

{----------------------------------------------------------------------------------------------}
procedure PreparaProcLbl(FR: TForm; var PLabel: TLabel; const PCaption: string;
  PVisible: Boolean = true);
begin
  with PLabel do
  begin
    if PCaption <> '-' then
      Caption := PCaption;
    Visible := PVisible;
  end;
  if FR <> nil then
    FR.Repaint;
end;

{----------------------------------------------------------------------------------------------}
procedure PreparaProcPnl(var APanel: TPanel; PVisible: Boolean; PProgBar: TProgressBar = nil;
  PMax: integer = 0; PLabel: TLabel = nil; PCaption: string = '');
begin
  APanel.Visible := PVisible;
  if PProgBar <> nil then
    FH.PreparaProcPgb(PProgBar, PMax, true, 1);
  if PLabel <> nil then
    FH.PreparaProcLbl(TForm(APanel.Owner), PLabel, PCaption, true);
end;

{----------------------------------------------------------------------------------------------}
procedure LimpaContainer(PContainer: TWinControl);
var
  I: integer;
begin
  try
    with PContainer do
    begin
      for I := 0 to ControlCount - 1 do
      begin
        if (Controls[I] is TLabel) then
        begin
          case Controls[I].Tag of
            9: TLabel(Controls[I]).Caption := '';
            8: FH.SetaCaptionLbl(TLabel(Controls[I]), '', 1);
          end;
        end;
             if Controls[I] is TEdit then TEdit(Controls[I]).Clear
        else if Controls[I] is TCustomMaskEdit then TCustomMaskEdit(Controls[I]).Clear
        else if Controls[I] is TMemo then TMemo(Controls[I]).Lines.Clear
        else if Controls[I] is TRichEdit then TRichEdit(Controls[I]).Lines.Clear
        else if Controls[I] is TCheckBox then TCheckBox(Controls[I]).Checked := false
        else if Controls[I] is TImage then TImage(Controls[I]).Picture.Assign(nil)
        else if Controls[I] is TComboBox then TComboBox(Controls[I]).ItemIndex := -1
        else if Controls[I] is TCheckListBox then FH.SetaStatusCheckListBox(false, TCheckListBox(Controls[I]));
      end;
    end;
  except
  end;
end;

{----------------------------------------------------------------------------------------------}
procedure LimpaContainerLabel(PContainer: TWinControl);
var
  I: integer;
begin
  try
    with PContainer do
    begin
      for I := 0 to ControlCount - 1 do
      begin
        if (Controls[I] is TLabel) then
        begin
          case Controls[I].Tag of
            9: TLabel(Controls[I]).Caption := '';
            8: FH.SetaCaptionLbl(TLabel(Controls[I]), '', 1);
          end;
        end;
      end;
    end;
  except
  end;
end;

{----------------------------------------------------------------------------------------------}
function IdentPos(const PIdent: string; var PPos: Longint; const PMap: array of TIdentMapEntry): Boolean;
var
  I: integer;
begin
  for I := Low(PMap) to High(PMap) do
    if AnsiSameText(PMap[I].Name, PIdent) then
    begin
      Result := true;
      PPos := I;
      exit;
    end;
  Result := false;
end;

{----------------------------------------------------------------------------------------------}
function StrInvalida(const S: string): Boolean;
begin
  Result := Length(Trim(S)) = 0;
end;

{----------------------------------------------------------------------------------------------}
function StrOk(const S: string): Boolean;
begin
  Result := Length(Trim(S)) > 0;
end;

{----------------------------------------------------------------------------------------------}
function StringListOk(SL: TStrings): Boolean;
begin
    if SL = nil then
       Result := False
    else
       Result := FH.StrOk(FH.LimpaBreak(SL.Text));
end;

{----------------------------------------------------------------------------------------------}
function VerEditText(PEdit: TEdit): Boolean;
begin
  Result := Length(Trim(PEdit.Text)) > 0;
end;

{----------------------------------------------------------------------------------------------}
procedure BloqUpdate(WC: TWinControl);
begin
  if WC <> nil then
    LockWindowUpdate(WC.Handle)
  else
    LockWindowUpdate(0);
end;

{----------------------------------------------------------------------------------------------}
procedure DesbloqUpdate;
begin
  LockWindowUpdate(0);
end;

{----------------------------------------------------------------------------------------------}
function RetMaskNum(const PQtdDec: integer; const PPrefixo: string = '###,###,##0.'): string;
begin
  Result := PPrefixo + DupeString('0', PQtdDec);
end;

{----------------------------------------------------------------------------------------------}
function ArredondaValor(const pVal: Extended; const PDecimal: integer = 2): Extended;
begin
  Result := StrToFloat(FloatToStrF(pVal, ffFixed, 15, PDecimal));
end;

{----------------------------------------------------------------------------------------------}
function ArredondaValorStr(const pVal: Extended; const PDecimal: integer = 2): string;
begin
  Result := FloatToStr(FH.ArredondaValor(pVal, PDecimal));
end;

{----------------------------------------------------------------------------------------------}
procedure SetaCaptionLbl(PLabel: TLabel; const PCaption: string; const PEspc: integer = 2);
var
  M: string;
begin
  M := FH.GeraEspc(PEspc);
  with PLabel do
  begin
    if Pos(':', HelpKeyword) = 0 then
      M := ':' + M;
    M := '%s' + M + '%s';
    if not FH.StrInvalida(HelpKeyword) then
      Caption := Format(M, [HelpKeyword, PCaption])
    else
      Caption := PCaption;
  end;
end;

{----------------------------------------------------------------------------------------------}
function CalcPercentual(const PBase, PPercentual: double): double;
begin
  Result := ((PBase * PPercentual) / 100);
end;

{----------------------------------------------------------------------------------------------}
function MsgSe(pValor: Boolean; const PTrue, PFalse: string;
  const PFlagTrue: integer = 3; const PFlagFalse: integer = 1): Boolean;

  procedure Disp(S: string; K: integer);
  begin
    case K of
      1: FH.MsgE(S);
      2: FH.MsgA(S);
      3: FH.MsgI(S);
    end;
  end;

begin
  Result := pValor;
  if Result and (PTrue <> '') then
    Disp(PTrue, PFlagTrue)
  else if (not Result) and (PFalse <> '') then
    Disp(PFalse, PFlagFalse)
end;

{----------------------------------------------------------------------------------------------}
function RetValorExtenso(pValor: double; const FormatoNF: Boolean = False; const PMonetario: Boolean = True;
  const PGenFem: Boolean = False): string;
const
  cUnidade: array[1..9] of string = ('um', 'dois', 'três', 'quatro', 'cinco', 'seis',
                                     'sete', 'oito', 'nove');
  cDezVinte: array[1..9] of string = ('onze', 'doze', 'treze', 'quatorze', 'quinze',
                                      'dezesseis', 'dezessete', 'dezoito', 'dezenove');
  cDezena: array[1..9] of string = ('dez', 'vinte', 'trinta', 'quarenta',
                                    'cinqüenta', 'sessenta', 'setenta', 'oitenta', 'noventa');
  cCentena: array[1..9] of string = ('cem', 'duzentos', 'trezentos', 'quatrocentos',
                                     'quinhentos', 'seiscentos', 'setecentos', 'oitocentos',
                                     'novecentos');

var
  vValorS, vBilhao, vMilhao, vMilhar, vCentena, vCentavo, auxStr: string;

  function RetDigitos(Digitos: string): string;
  begin
    Result := '';
    if (StrToInt(Digitos) > 100) and (StrToInt(Digitos) < 200) then
      Result := 'cento e '
    else
    if StrToInt(Digitos) > 99 then
    begin
      Result := cCentena[StrToInt(copy(Digitos, 1, 1))];
      if Copy(Digitos, 2, 2) = '00' then
        exit
      else
        Result := Result + ' e ';
    end;
    if (StrToInt(copy(Digitos, 2, 2)) > 10) and (StrToInt(copy(Digitos, 2, 2)) < 20) then
    begin
      Result := Result + cDezVinte[StrToInt(copy(Digitos, 3, 1))];
      exit;
    end;
    if (StrToInt(copy(Digitos, 2, 2)) > 9) then
      Result := Result + cDezena[StrToInt(copy(Digitos, 2, 1))];
    if (StrToInt(copy(Digitos, 3, 1)) = 0) then
      exit;
    if  (Copy(Result, (Length(Result) - 1), 1) <> 'e')
    and (Result <> '') then
      Result := Result + ' e ';

    auxStr := cUnidade[StrToInt(Copy(Digitos, 3, 1))];
    if PGenFem then
    begin
      if SameText(auxStr, 'um') then
        auxStr := 'uma'
      else
      if SameText(auxStr, 'dois') then
        auxStr := 'duas';
    end;
    Result := Result + auxStr;
  end;

begin
  Result := '';
  pValor := Abs(pValor);
  vValorS := Format('%15.2f', [pValor]);
  while Pos(' ', vValorS) > 0 do
    vValorS[Pos(' ', vValorS)] := '0';

  vBilhao := Copy(vValorS, 1, 3);
  if vBilhao <> '000' then
  begin
    if vBilhao = '001' then
      Result := 'Um bilhão '
    else
      Result := RetDigitos(vBilhao) + ' bilhões '
  end;

  vMilhao := Copy(vValorS, 4, 3);
  if vMilhao <> '000' then
  begin
    if vMilhao = '001' then
      Result := 'Um milhão '
    else
      Result := Result + RetDigitos(vMilhao) + ' milhões ';
  end;

  vMilhar := Copy(vValorS, 7, 3);
  if vMilhar <> '000' then
  begin
    if Result = '' then
      Result := RetDigitos(vMilhar) + ' mil '
    else
      Result := Result + 'e ' + RetDigitos(vMilhar) + ' mil ';
  end;

  vCentena := Copy(vValorS, 10, 3);
  if vCentena <> '000' then
  begin
    if Result = '' then
    begin
      if vCentena = '001' then
      begin
        if PMonetario then
          Result := 'Um real '
        else
        begin
          if PGenFem then
            Result := 'Uma '
          else
            Result := 'Um ';
        end;
      end
      else
      begin
        if PMonetario then
          Result := RetDigitos(vCentena) + ' reais '
        else
          Result := RetDigitos(vCentena) + ' ';
      end;
    end
    else
    begin
      if PMonetario then
        Result := Result + 'e ' + RetDigitos(vCentena) + ' reais '
      else
        Result := Result + 'e ' + RetDigitos(vCentena) + ' ';
    end;
  end
  else
  if Result <> '' then
  begin
    if PMonetario then
    begin
      if pValor > 999999.99 then
        Result := Result + 'de reais '
      else
        Result := Result + 'reais ';
    end
    else
      Result := Result + ' ';
  end;

  vCentavo := '0' + Copy(vValorS, 14, 2);
  if vCentavo <> '000' then
  begin
    if Result = '' then
      Result := RetDigitos(vCentavo) + ' centavos'
    else
      Result := Result + 'e ' + RetDigitos(vCentavo) + ' centavos';
    if vCentavo = '001' then
      Delete(Result, Length(Result), 1);
  end;
  Result := Trim(Result);
  try
    Result := UpperCase(Result[1]) + Copy(Result, 2, MaxInt);
  except
    //
  end;
  if FormatoNF then
    Result := '(*** ' + Result + ' ***)';
end;

{----------------------------------------------------------------------------------------------}
function TrataFloat(const S: string; PDef: string = ''): string;
begin
  Result := S;
  if not FH.FloatOk(Result) then
    Result := PDef;
end;

{----------------------------------------------------------------------------------------------}
function CarregaStringList(PTab: TDataSet; PCampos: string; PSep: string = ' '): TStrings;
var
  I, P: SmallInt;
  vCampo: array of string;
  S: string;
begin
  Result := TStringList.Create;
  Result.Text := '';
  if Length(PCampos) = 0 then
    exit;
  S := PCampos + ';';
  SetLength(vCampo, FH.ContaStr(S, ';', -1, -1));
  I := 0;
  while Pos(';', S) > 0 do
  begin
    P := Pos(';', S);
    vCampo[I] := Copy(S, 1, P - 1);
    Delete(S, 1, P);
    Inc(I);
  end;
  PTab.First;
  while not PTab.Eof do
  begin
    S := '';
    for I := 0 to High(vCampo) do
      S := S + PTab.FieldByName(vCampo[I]).AsString + PSep;
    Result.Add(FH.TrimX(S, PSep));
    PTab.Next;
  end;
end;

{----------------------------------------------------------------------------------------------}
procedure SetaComboDes(CB: TComboBox; S: string; PSep: string = ' ');
var
  I: integer;
  A, B: string;
begin
  S := AnsiLowerCase(Trim(S));
  with CB do
  begin
    ItemIndex := -1;
    if S = '' then
      exit;
    B := '';
    for I := 0 to Items.Count - 1 do
    begin
      A := Copy(Items[I], Pos(PSep, Items[I]) + Length(PSep), MaxInt);
      A := AnsiLowerCase(Trim(A));
      if Length(A) = 0 then
        Continue;
      B := S;
      if (AnsiSameText(A, B)) or (LeftStr(B, Length(B)) = LeftStr(A, Length(B))) then
      begin
        ItemIndex := I;
        Break;
      end;
    end;
  end;
end;

{----------------------------------------------------------------------------------------------}
procedure SetaComboCod(CB: TComboBox; S: string; PSep: string = ' ');
var
  I: integer;
  A, B: string;
begin
  S := AnsiLowerCase(Trim(S));
  with CB do
  begin
    ItemIndex := -1;
    if S = '' then
      exit;
    B := '';
    for I := 0 to Items.Count - 1 do
    begin
      if  (Pos(PSep, Items[I]) = 0)
      and (Length(S) = Length(Items[I])) then
        A := Items[I]
      else
        A := Copy(Items[I], 1, Pos(PSep, Items[I]) - Length(PSep));
      A := AnsiLowerCase(Trim(A));
      if Length(A) = 0 then
        Continue;
      B := S;
      if AnsiSameText(A, B) then
      begin
        ItemIndex := I;
        Break;
      end;
    end;
  end;
end;

{----------------------------------------------------------------------------------------------}
function FracInt(const X: Extended; const Pivo: integer = 1000): Int64;
begin
  Result := Round(Frac(X) * Pivo);
end;

{----------------------------------------------------------------------------------------------}
function FormataCNPJCPF(const PCode: string): string;
begin
  Result := StrInteiro(PCode);
  while (Length(Result) < 14) and (Length(Result) > 11) do
    Result := '0' + Result;
  if Length(Result) = 14 then
  begin
    Insert('.', Result, 3);
    Insert('.', Result, 7);
    Insert('/', Result, 11);
    Insert('-', Result, 16);
  end
  else
  if Length(Result) = 11 then
  begin
    Insert('.', Result, 4);
    Insert('.', Result, 8);
    Insert('-', Result, 12);
  end;
end;

{----------------------------------------------------------------------------------------------}
function MontaCNPJCPF(const Codigo: string): string;
begin
  Result := StrInteiro(Codigo);
  while (Length(Result) < 14) and (Length(Result) > 11) do
    Result := '0' + Result;
  if Length(Result) = 14 then
  begin
    Insert('.', Result, 3);
    Insert('.', Result, 7);
    Insert('/', Result, 11);
    Insert('-', Result, 16);
  end
  else
  if Length(Result) = 11 then
  begin
    Insert('-', Result, 10);
  end;
end;

{----------------------------------------------------------------------------------------------}
procedure GravaColGrid(FR: TForm; GR: TDBGrid; const PArquivo: string;
  const PShowMsg: Boolean);
var
  temp, fname: string;
begin
  FH.SetaCursorAmp;
  try
    temp := PArquivo;
    if temp = '' then
      temp := FH.SetaLocal(vrgGridDefSubDir) +
           ChangeFileExt(FR.Name +
                         FH.SetaUpper(GR.Name, 1), vrgGridDefExtension)
    else
    begin
      fname := ExtractFilePath(temp);
      if fname = '' then
        fname := FH.SetaLocal(vrgGridDefSubDir)
      else
        fname := FH.IncPathDel(fname);
      if ExtractFileName(temp) = '' then
        temp := FR.Name +
                FH.SetaUpper(GR.Name, 1);
      temp := fname + ChangeFileExt(temp, vrgGridDefExtension);
    end;
    try
      GR.Columns.SaveToFile(temp);
      if PShowMsg then FH.MsgI('As colunas foram salvas.');
    except
    end;
  finally
    FH.SetaCursorDef;
  end;
end;

{----------------------------------------------------------------------------------------------}
procedure LeColGrid(FR: TForm; GR: TDBGrid; const PArquivo: string);
var
  temp, fname: string;
begin
  temp := PArquivo;
  if temp = '' then
    temp := FH.SetaLocal(vrgGridDefSubDir) +
         ChangeFileExt(FR.Name +
                       FH.SetaUpper(GR.Name, 1), vrgGridDefExtension)
  else
  begin
    fname := ExtractFilePath(temp);
    if fname = '' then
      fname := FH.SetaLocal(vrgGridDefSubDir)
    else
      fname := FH.IncPathDel(fname);
    if ExtractFileName(temp) = '' then
      temp := FR.Name +
              FH.SetaUpper(GR.Name, 1);
    temp := fname + ChangeFileExt(temp, vrgGridDefExtension);
  end;
  if FileExists(temp) then
  begin
    try
      GR.Columns.LoadFromFile(temp);
    except
      try SysUtils.DeleteFile(temp); except end;
    end;
  end;
end;

{----------------------------------------------------------------------------------------------}
procedure DeletaColGrid(FR: TForm; GR: TDBGrid; const PArquivo: string;
  const PShowMsg: Boolean);
var
  temp, fname: string;
begin
  if PShowMsg then
    if FH.MsgSN('Deseja resetar a disposição das colunas?') = 0 then
      exit;
  temp := PArquivo;
  if temp = '' then
    temp := FH.SetaLocal(vrgGridDefSubDir) +
         ChangeFileExt(FR.Name +
                       FH.SetaUpper(GR.Name, 1), vrgGridDefExtension)
  else
  begin
    fname := ExtractFilePath(temp);
    if fname = '' then
      fname := FH.SetaLocal(vrgGridDefSubDir)
    else
      fname := FH.IncPathDel(fname);
    if ExtractFileName(temp) = '' then
      temp := FR.Name +
              FH.SetaUpper(GR.Name, 1);
    temp := fname + ChangeFileExt(temp, vrgGridDefExtension);
  end;
  SysUtils.DeleteFile(temp);
end;

{----------------------------------------------------------------------------------------------}
function VerZero(const pValor: Extended; const PDefault: Extended = 1): Extended;
begin
  Result := pValor;
  if FH.FmtFix(Result)='0,00' then
    Result := PDefault;
end;

{----------------------------------------------------------------------------------------------}
function SafeDiv(const PDividendo, PDivisor: Extended; const PDivisorDefault: Extended = 1): Extended;
begin
  Result := PDividendo / IfThen(IsZero(PDivisor), PDivisorDefault, PDivisor);
end;

{----------------------------------------------------------------------------------------------}
function TrataDivZero(const PDividendo, PDivisor: Extended; const ResultadoSeZero: Extended = 0): Extended;
begin
    if PDivisor = 0 then
       Result := ResultadoSeZero
    else
       Result := PDividendo / PDivisor;
end;

{----------------------------------------------------------------------------------------------}
function SetaUpper(const pTexto: string; const PTotal: integer = -1): string;
begin
  Result := TrimLeft(pTexto);
  if PTotal < 0 then
    Result := AnsiUpperCase(Result)
  else
  if PTotal > 0 then
    Result := AnsiUpperCase(Copy(Result, 1, PTotal)) + Copy(Result, PTotal + 1, MaxInt);
end;

{----------------------------------------------------------------------------------------------}
function InitCap(const pTexto: string): string;
begin
  Result := FH.SetaUpper(pTexto, 1);
end;

{----------------------------------------------------------------------------------------------}
function FmtFloatStr(pVal: Extended; PTamString: integer; pDec: Byte = 2): string;
begin
  if pVal < 0 then
  begin
      Result := FH.LimpaString(FH.FmtFix(Abs(pVal), pDec), [DecimalSeparator]);
      Result := '-' + FH.StrZero(Result, PTamString - 1);
  end
  else
  begin
      Result := FH.LimpaString(FH.FmtFix(pVal, pDec), [DecimalSeparator]);
      Result := FH.StrZero(Result, PTamString);
  end;
end;

{----------------------------------------------------------------------------------------------}
function FmtFloatStr(pVal: string; PTamString: integer; pDec: Byte = 2): string;
begin
  Result := FH.LimpaString(pVal, [ThousandSeparator]);
  Result := FH.FmtFloatStr(StrToFloatDef(Result, 0), PTamString, pDec);
end;

{----------------------------------------------------------------------------------------------}
function PrimeiroDiaMes(const PData: TDateTime = 0): TDateTime;
var
  D: TDateTime;
begin
  D := Date;
  if PData <> 0 then
    D := PData;
  Result := EncodeDate(YearOf(D), MonthOf(D), 1);
end;

{----------------------------------------------------------------------------------------------}
function PrimeiroDiaMesStr(const PData: TDateTime = 0): string;
var
  D: TDateTime;
begin
  D := Now;
  if PData <> 0 then
    D := PData;
  Result := FormatDateTime('01/mm/yyyy', D);
end;

{----------------------------------------------------------------------------------------------}
function MultiStringReplace(const S: string; const OldPattern, NewPattern: array of string;
  Flags: TReplaceFlags): string;
var
  I: integer;
begin
  Result := S;
  for I := Low(OldPattern) to High(OldPattern) do
    Result := StringReplace(Result, OldPattern[I], NewPattern[I], Flags);
end;

{----------------------------------------------------------------------------------------------}
function RetBreak(const PTotal: Cardinal = 1): string;
var
  I: integer;
begin
  Result := '';
  for I := 1 to PTotal do
    Result := Result + Chr(13);
end;

{----------------------------------------------------------------------------------------------}
procedure FmtFloatLabel(var PLabel: TLabel; const pValor: Extended;
  const PCaption: string = ''; const PThousandSeparator: Boolean = true;
  const pDec: Cardinal = 2);
var
  F: TFloatFormat;
begin
  F := ffNumber;
  if not PThousandSeparator then
    F := ffFixed;
  if PCaption = '' then
    PLabel.Caption := FloatToStrF(pValor, F, 14, pDec)
  else
    PLabel.Caption := Format(PCaption, [FloatToStrF(pValor, F, 14, pDec)]);
end;

{----------------------------------------------------------------------------------------------}
procedure FmtIntLabel(var PLabel: TLabel; const pValor: integer;
  const PCaption: string = ''; const PThousandSeparator: Boolean = true);
var
  F: TFloatFormat;
begin
  F := ffNumber;
  if not PThousandSeparator then
    F := ffFixed;
  if PCaption = '' then
    PLabel.Caption := FloatToStrF(pValor, F, 14, 0)
  else
    PLabel.Caption := Format(PCaption, [FloatToStrF(pValor, F, 14, 0)]);
end;

{----------------------------------------------------------------------------------------------}
procedure IniGera(var L: TStrings; const PConteudo: string);
var
  I: integer;
begin
  L := nil;
  FH.CriaLista(L);
  L.Text := PConteudo;
  I := 0;
  while I < L.Count do
  begin
    if (not AnsiContainsText(L[I], '=')) and
       (not AnsiContainsText(L[I], ']')) and
       (not AnsiContainsText(L[I], '[')) then
      L.Delete(I)
    else
      Inc(I);
  end;
end;

{----------------------------------------------------------------------------------------------}
procedure IniGrava(var L: TStrings; const pNomeArq: string);
begin
  L.SaveToFile(pNomeArq);
end;

{----------------------------------------------------------------------------------------------}
procedure IniDeletaSec(var L: TStrings; const PSecao: string);
var
  I, J: integer;
begin
  J := L.IndexOf('[' + PSecao + ']');
  if J > -1 then
  begin
    I := J + 1;
    while I < L.Count do
    begin
      // não há chaves dentro da seção
      if (Pos('[', L[I]) > 0) and (Pos(']', L[I]) > 0) then
        Break;
      // a seção é a última da lista
      if I = (L.Count - 1) then
      begin
        L.Delete(I);
        Break;
      end;
      L.Delete(I);
    end;
    L.Delete(J);
  end;
end;

{----------------------------------------------------------------------------------------------}
procedure IniDeletaIdent(var L: TStrings; const PSecao, PIdent: string);
var
  I, J: integer;
begin
  J := L.IndexOf('[' + PSecao + ']');
  if J = -1 then
    exit // seção não existe
  else
  begin
    for I := J + 1 to L.Count - 1 do
    begin
      // fim da seção
      if (Pos('[', L[I]) > 0) and (Pos(']', L[I]) > 0) then
        exit;
      if AnsiSameText(PIdent, Trim(Copy(L[I], 1, Pos('=', L[I]) - 1))) then
      begin
        // chave encontrada dentro da seção
        L.Delete(I);
        exit;
      end;
      // fim da lista
      if I = (L.Count - 1) then
        exit;
    end;
  end;
end;

{----------------------------------------------------------------------------------------------}
function IniExisteSec(var L: TStrings; const PSecao: string): Boolean;
begin
  Result := L.IndexOf('[' + PSecao + ']') > -1;
end;

{----------------------------------------------------------------------------------------------}
procedure IniGravaStr(var L: TStrings; const PSecao, PIdent, pValor: string);
var
  I, J, cc: integer;
begin
  J := L.IndexOf('[' + PSecao + ']');
  if J = -1 then
  begin
    // a seção e a chave não existem
    L.Add('[' + PSecao + ']');
    L.Add(PIdent + '=' + pValor);
  end
  else
  begin
    cc := 0;
    for I := J + 1 to L.Count - 1 do
    begin
      Inc(cc);
      if (Pos('[', L[I]) > 0) and (Pos(']', L[I]) > 0) then
      begin
        if I = J + 1 then // não há chaves dentro da seção
          L.Add(PIdent + '=' + pValor)
        else
          L.Insert(I, PIdent + '=' + pValor);
        Break;
      end;
      // a chave já existe dentro da seção
      if AnsiSameText(PIdent, Trim(Copy(L[I], 1, Pos('=', L[I]) - 1))) then
      begin
        L.Strings[I] := PIdent + '=' + pValor;
        Break;
      end;
      // a seção é a última da lista
      if I = (L.Count - 1) then
      begin
        L.Add(PIdent + '=' + pValor);
        Break;
      end;
    end;
    if cc = 0 then
    begin
        L.Add(PIdent + '=' + pValor);
    end;
  end;
end;

{----------------------------------------------------------------------------------------------}
procedure IniGravaInt(var L: TStrings; const PSecao, PIdent: string;
  const pValor: integer);
begin
  FH.IniGravaStr(L, PSecao, PIdent, IntToStr(pValor));
end;

{----------------------------------------------------------------------------------------------}
procedure IniGravaNum(var L: TStrings; const PSecao, PIdent: string;
  const pValor: Extended);
begin
  FH.IniGravaStr(L, PSecao, PIdent, FloatToStr(pValor));
end;

{----------------------------------------------------------------------------------------------}
procedure IniGravaBln(var L: TStrings; const PSecao, PIdent: string;
  const pValor: Boolean);
begin
  FH.IniGravaStr(L, PSecao, PIdent, BoolToStr(pValor));
end;

{----------------------------------------------------------------------------------------------}
procedure IniGravaData(var L: TStrings; const PSecao, PIdent: string;
  const pValor: TDateTime);
begin
  FH.IniGravaStr(L, PSecao, PIdent, DateToStr(pValor));
end;

{----------------------------------------------------------------------------------------------}
function IniLeStr(var L: TStrings; const PSecao, PIdent: string;
  const PDefault: string = ''): string;
var
  I, J: integer;
begin
  Result := PDefault;
  J := L.IndexOf('[' + PSecao + ']');
  if J = -1 then
    exit // seção não existe
  else
  begin
    for I := J + 1 to L.Count - 1 do
    begin
      // fim da seção
      if (LeftStr(Trim(L[I]), 1) = '[') and (Pos(']', L[I]) > 0) then
        exit;
      if AnsiSameText(PIdent, Trim(Copy(L[I], 1, Pos('=', L[I]) - 1))) then
      begin
        // chave encontrada dentro da seção
        Result := Trim(Copy(L[I], Pos('=', L[I]) + 1, MaxInt));
        exit;
      end;
      // fim da lista
      if I = (L.Count - 1) then
        exit;
    end;
  end;
end;

{----------------------------------------------------------------------------------------------}
function IniLeInt(var L: TStrings; const PSecao, PIdent: string;
  const PDefault: integer): integer;
begin
  Result := StrToIntDef(FH.IniLeStr(L, PSecao, PIdent), PDefault);
end;

{----------------------------------------------------------------------------------------------}
function IniLeNum(var L: TStrings; const PSecao, PIdent: string;
  const PDefault: Extended): Extended;
begin
  Result := StrToFloatDef(FH.IniLeStr(L, PSecao, PIdent), PDefault);
end;

{----------------------------------------------------------------------------------------------}
function IniLeBln(var L: TStrings; const PSecao, PIdent: string;
  const PDefault: Boolean): Boolean;
begin
  Result := StrToBoolDef(FH.IniLeStr(L, PSecao, PIdent), PDefault);
end;

{----------------------------------------------------------------------------------------------}
function IniLeData(var L: TStrings; const PSecao, PIdent: string;
  const PDefault: TDateTime): TDateTime;
begin
  Result := StrToDateDef(FH.IniLeStr(L, PSecao, PIdent), PDefault);
end;

{----------------------------------------------------------------------------------------------}
procedure GeraToolTip(FR: TForm; WC: TWinControl;
  const PIconKind: Cardinal; const PTitle, pTexto: string);
const
  Tts_Balloon = $40;
  Ttm_SetTitle = (Wm_User + 32);
var
  vRes: Cardinal;
  vInfo: TToolInfo;
  vBuffer: array[0..255] of char;


  procedure Gerar(hWnd: Cardinal);
  begin
    vRes := CreateWindowEx(Ws_Ex_ContextHelp, 'Tooltips_Class32', nil, Tts_AlwaysTip or Tts_Balloon,
      integer(Cw_UseDefault), integer(Cw_UseDefault), integer(Cw_UseDefault),
      integer(Cw_UseDefault), hWnd, 0, hInstance, nil);
    if vRes <> 0 then
    begin
      SetWindowPos(vRes, Hwnd_TopMost, 0, 0, 0, 0, Swp_NoMove or Swp_NoSize or Swp_NoActivate);
      vInfo.cbSize := SizeOf(TToolInfo);
      vInfo.uFlags := Ttf_SubClass;
      vInfo.hInst := hInstance;
    end;
  end;


  procedure Adicionar(hwnd: dword; lpti: PToolInfo; IconType: integer;
                      Text, Title: PChar);
  var
    Item: THandle;
    Rect: TRect;
  begin
    Item := hWnd;
    if (Item <> 0) and (GetClientRect(Item, Rect)) then
    begin
      lpti.hwnd := Item;
      lpti.Rect := Rect;
      lpti.lpszText := Text;
      SendMessage(vRes, Ttm_AddTool, 0, integer(lpti));
      FillChar(vBuffer, sizeof(vBuffer), #0);
      LStrCpy(vBuffer, Title);
      if (IconType > 3) or (IconType < 0) then
        IconType := 0;
      SendMessage(vRes, Ttm_SetTitle, IconType, integer(@vBuffer));
      SendMessage(vRes, Ttm_SetDelayTime, TTDT_AUTOMATIC, 5000000);
      SendMessage(vRes, TTM_SETDELAYTIME, TTDT_INITIAL, 1);
    end;
  end;


begin
  Gerar(FR.Handle);
  Adicionar(WC.Handle, @vInfo, PIconKind, PChar(pTexto), PChar(PTitle));
end;

{----------------------------------------------------------------------------------------------}
procedure SetaComboIndex(CB: TComboBox; const S: string);
var
  I: integer;
begin
  with CB do
    for I := 0 to Items.Count - 1 do
      if AnsiSameText(Items[I], S) then
      begin
        ItemIndex := I;
        exit;
      end;
end;

{----------------------------------------------------------------------------------------------}
procedure DeletaArqEx(FR: TForm; const PDir, PMask: string);
var
  F: TFileListBox;
  I: integer;
begin
  try
    F := TFileListBox.Create(F);
    F.Parent := FR;
    F.Visible := false;
    F.Directory := ExtractFilePath(PDir);
    F.Mask := PMask;
    for I := 0 to F.Items.Count - 1 do
      SysUtils.DeleteFile(FH.IncPathDel(F.Directory) + F.Items[I]);
  finally
    F.Free;
  end;
end;

{----------------------------------------------------------------------------------------------}
procedure ClonaArqEx(FR: TForm; const PDirOrigem, PDirDestino, PArquivo: string);
var
  F: TFileListBox;
  I: integer;
  SO, SD: string;
begin
  try
    F := TFileListBox.Create(FR);
    F.Parent := FR;
    F.Visible := false;
    F.Directory := PDirOrigem;
    F.Mask := PArquivo;
    for I := 0 to F.Items.Count - 1 do
    begin
      SO := FH.IncPathDel(PDirOrigem) + F.Items[I];
      SD := FH.IncPathDel(PDirDestino) + F.Items[I];
      Windows.CopyFile(PChar(SO), PChar(SD), false);
    end;
  finally
    F.Free;
  end;
end;

{----------------------------------------------------------------------------------------------}
procedure MultiHide(FR: TForm);
var
  F: TForm;
begin
  FR.Hide;
  F := TForm(FR.Owner);
  while F <> Application.MainForm do
  begin
    F.Hide;
    F := TForm(F.Owner);
  end;
end;

{----------------------------------------------------------------------------------------------}
procedure MultiShow(FR: TForm);
var
  F: TForm;
begin
  F := TForm(FR.Owner);
  while F <> Application.MainForm do
  begin
    F.Show;
    F := TForm(F.Owner);
  end;
  FR.Show;
end;

{----------------------------------------------------------------------------------------------}
procedure HideFR(FR: TForm);
var
  ii: Integer;
begin
  if FR.Visible then
    FR.Hide;
  for ii := 0 to Screen.FormCount-1 do
    if  (Screen.Forms[ii]       <> Application.MainForm)
    and (Screen.Forms[ii].Owner <> nil                 ) then
      if Screen.Forms[ii].Visible then
        Screen.Forms[ii].Hide;
end;

{----------------------------------------------------------------------------------------------}
procedure ShowFR(FR: TForm);
var
  ii: Integer;
begin
  for ii := 0 to Screen.FormCount-1 do
    if  (Screen.Forms[ii]       <> Application.MainForm)
    and (Screen.Forms[ii].Owner <> nil                 ) then
      Screen.Forms[ii].Show;
  FR.Show;
end;

{----------------------------------------------------------------------------------------------}
function PixelParaMm(const pValor: integer): double;
begin
  Result := pValor / FH.PixelMM;
end;

{----------------------------------------------------------------------------------------------}
function MmParaPixel(const pValor: double): integer;
begin
  Result := Round(pValor * FH.PixelMM);
end;

{----------------------------------------------------------------------------------------------}
function RetPalavra(const S: string; const PQuant: integer): string;
var
  P: string;
  I, T: integer;
begin
  Result := '';
  P := Trim(S) + #32;
  T := 0;
  if Length(P) < 2 then
    exit;
  for I := 1 to Length(P) do
  begin
    if IsValidIdent(P[I])
    or (P[I] in ['0'..'9'])
    or (P[I] in [#192..#255]) then
      Result := Result + P[I]
    else
    if RightStr(Result, 1) <> #32 then
    begin
      Result := Result + #32;
      Inc(T);
      if T = PQuant then
        Break;
    end;
  end;
  Result := Trim(Result);
end;

{----------------------------------------------------------------------------------------------}
function BuscaArrayInt(const PNum: integer; const PValores: array of integer): Boolean;
begin
  Result := FH.PosArrayInt(PNum, PValores) <> -1;
end;

{----------------------------------------------------------------------------------------------}
function PosArrayInt(const PNum: integer; const PValores: array of integer): integer;
var
  I: integer;
begin
  Result := -1;
  for I := Low(PValores) to High(PValores) do
    if PNum = PValores[I] then
    begin
      Result := I;
      Break;
    end;
end;

{----------------------------------------------------------------------------------------------}
procedure MultiInsert(var Dest: string; const Substr: array of string;
  const Index: array of integer);
var
  I, J: integer;
begin
  J := Low(Index);
  for I := Low(Substr) to High(Substr) do
  begin
    if I <= High(Index) then
      J := I;
    System.Insert(Substr[I], Dest, Index[J]);
  end;
end;

{----------------------------------------------------------------------------------------------}
function MontaDataFmt(const PDataOriginal, PDisplayFormat: string): string;
var
  D: string;
begin
  D := PDataOriginal;

  if AnsiSameText(PDisplayFormat, 'ddmmyyyy') then
    FH.MultiInsert(D, ['/', '/'], [3, 6])
  else
  if AnsiSameText(PDisplayFormat, 'ddmmyy') then
    FH.MultiInsert(D, ['/', '/'], [3, 6]);

  if not FH.DataOk(D) then
    D := '';
  Result := D;
end;

{----------------------------------------------------------------------------------------------}
procedure GeraCDSInativo(POrigem: TDataSet; CDS: TClientDataSet);
var
  I: integer;
  t: TFieldType;
begin
  CDS.Close;
  CDS.FieldDefs.Clear;
  with POrigem.FieldDefs do
    for I := 0 to Count - 1 do
    begin
      if Items[I].DataType = ftLargeInt then
      begin
        t := ftLargeint;
        CDS.FieldDefs.Add(Items[I].Name, t, Items[I].Size);
      end
      else
      if Items[I].DataType = ftBCD then
      begin
        t := ftFloat;
        CDS.FieldDefs.Add(Items[I].Name, t);
      end
      else
      begin
        t := Items[I].DataType;
        CDS.FieldDefs.Add(Items[I].Name, t, Items[I].Size);
      end;
    end;
end;

{----------------------------------------------------------------------------------------------}
procedure GeraCDS(POrigem: TDataSet; CDS: TClientDataSet);
begin
  FH.GeraCDSInativo(POrigem, CDS);
  CDS.CreateDataSet;
end;

{----------------------------------------------------------------------------------------------}
procedure ClonaUm(POrigem, PDestino: TDataSet; PAppend: Boolean; PIniciarEm: integer = 0);
var
  I: integer;
  n: string;
begin
  if PAppend then
    PDestino.Insert;
  with POrigem do
  begin
    for I := 0 to FieldDefs.Count - 1 do
    begin
      n := FieldDefs.Items[I].Name;
      try
        if FieldDefs.Items[I].DataType = ftLargeInt then
        begin
          if (FieldByName(n) as TLargeintField).IsNull then
            PDestino.FieldByName(n).Value := null
          else
            (PDestino.FieldByName(n) as TLargeintField).Value := (FieldByName(n) as TLargeintField).Value;
        end
        else
          PDestino.FieldByName(n).Value := FieldByName(n).Value;
      except
        //on e:Exception do
        //ShowMessage(e.Message);
      end;
    end;
  end;
end;

{----------------------------------------------------------------------------------------------}
procedure ClonaTudo(POrigem, PDestino: TDataSet; PAppend: Boolean;
  PIniciarEm: integer = 0; PProgBar: TProgressBar = nil);
var
  vStep, vMax: integer;
begin
  POrigem.DisableControls;
  POrigem.First;
  if PProgBar <> nil then
  begin
    with PProgBar do
    begin
      Position := 0;
      vStep := Step;
      Step := 1;
      vMax := Max;
      Max := POrigem.RecordCount;
    end;
  end;
  while not POrigem.Eof do
  begin
    FH.ClonaUm(POrigem, PDestino, PAppend, PIniciarEm);
    try
      PDestino.Post;
    except
      raise;
    end;
    POrigem.Next;
    if PProgBar <> nil then
      PProgBar.StepIt;
  end;
  if PProgBar <> nil then
  begin
    with PProgBar do
    begin
      Position := 0;
      Step := vStep;
      Max := vMax;
    end;
  end;
  POrigem.First;
  POrigem.EnableControls;
end;

{----------------------------------------------------------------------------------------------}
procedure SetaBmark(var P: Pointer; DS: TDataSet; PGet, PGoto: Boolean;
  PFree: Boolean = false);
begin
  if PGet then
  try
    P := DS.GetBookmark;
  except
  end;
  if PGoto then
  try
    DS.GotoBookmark(P);
  except
  end;
  if PFree then
  try
    DS.FreeBookmark(P);
  except
  end;
end;

{----------------------------------------------------------------------------------------------}
procedure GetBmark(var P: Pointer; DS: TDataSet);
begin
  try
    P := DS.GetBookmark;
  except
  end;
end;

{----------------------------------------------------------------------------------------------}
procedure GotoBmark(var P: Pointer; DS: TDataSet; PFree: Boolean = false);
begin
  try
    DS.GotoBookmark(P);
  except
  end;
  if PFree then
  try
    DS.FreeBookmark(P);
  except
  end;
end;

{----------------------------------------------------------------------------------------------}
procedure SetaFmtNum(DS: TDataSet; pCampo: string; PMask: string = '###,###,###,##0.00');
var
  F: TField;
begin
  F := DS.Fields.FindField(pCampo);
  if F = nil then
    exit;
  if not (F is TNumericField) then
    exit;
  (F as TNumericField).DisplayFormat := PMask;
end;

{----------------------------------------------------------------------------------------------}
function FldtpParaStr(AFType: TFieldType): string;
begin
  Result := FH.FieldTypeNames[AFType];
end;

{----------------------------------------------------------------------------------------------}
function StrParaFldtp(AFType: string): TFieldType;
begin
  Result := ftUnknown;
  if AnsiSameText(AFType, 'CHAR')     then Result := ftString else
  if AnsiSameText(AFType, 'STRING')   then Result := ftString else
  if AnsiSameText(AFType, 'SMALLINT') then Result := ftSmallInt else
  if AnsiSameText(AFType, 'integer')  then Result := ftInteger else
  if AnsiSameText(AFType, 'WORD')     then Result := ftWord else
  if AnsiSameText(AFType, 'Boolean')  then Result := ftBoolean else
  if AnsiSameText(AFType, 'FLOAT')    then Result := ftFloat else
  if AnsiSameText(AFType, 'CURRENCY') then Result := ftCurrency else
  if AnsiSameText(AFType, 'FLOAT')    then Result := ftFloat else
  if AnsiSameText(AFType, 'DATE')     then Result := ftDate else
  if AnsiSameText(AFType, 'TIME')     then Result := ftTime else
  if AnsiSameText(AFType, 'DATETIME') then Result := ftDateTime else
  if AnsiSameText(AFType, 'BLOB')     then Result := ftBlob else
  if AnsiSameText(AFType, 'AUTOINC')  then Result := ftAutoInc else
  if AnsiSameText(AFType, 'MEMO')     then Result := ftMemo else
  if AnsiSameText(AFType, 'GRAPHIC')  then Result := ftGraphic else
  if AnsiSameText(AFType, 'FMTMEMO')  then Result := ftFmtMemo;
end;

{----------------------------------------------------------------------------------------------}
procedure GravaXML(FR: TForm; CDS: TClientDataSet; pNome: string = '');
var
  vName: string;
begin
  if ExtractFileName(pNome) = '' then
  begin
    vName := '';
    if FR <> nil then
      vName := FR.Name;
    pNome := vName + FH.SetaUpper(CDS.Name, 1);
  end;

  if ExtractFilePath(pNome) = '' then
    pNome := FH.SetaLocal(FH.vrgDefaultDir + FH.vrgDirCache) + pNome;

  if ExtractFileExt(pNome) = '' then
    pNome := ChangeFileExt(pNome, '.xml');

  CDS.SaveToFile(pNome, dfXML);
end;

{----------------------------------------------------------------------------------------------}
function LeXML(FR: TForm; CDS: TClientDataSet; pNome: string = ''): Boolean;
begin
  if ExtractFileName(pNome) = '' then
    pNome := FR.Name + FH.SetaUpper(CDS.Name, 1);

  if ExtractFilePath(pNome) = '' then
    pNome := FH.SetaLocal(FH.vrgDefaultDir + FH.vrgDirCache) + pNome;

  if ExtractFileExt(pNome) = '' then
    pNome := ChangeFileExt(pNome, '.xml');

  Result := FileExists(pNome);
  if Result then
    CDS.LoadFromFile(pNome);
end;

{----------------------------------------------------------------------------------------------}
function LeXMLSetaIndex(FR: TForm; CDS: TClientDataSet; pNome: string = ''): Boolean;
begin
   result := false;
   if LeXML(FR,CDS,pNome) then
     if CDS.Fields.Count > 0 then
        CDS.IndexFieldNames := CDS.Fields.Fields[0].FieldName;
   result := true;
end;

{----------------------------------------------------------------------------------------------}
procedure HabiliaDatasets(PDataSets: array of TDataSet; PEnabled: Boolean);
var
  I: integer;
begin
  for I := Low(PDataSets) to High(PDataSets) do
    if PEnabled then
      PDataSets[I].EnableControls
    else
      PDataSets[I].DisableControls;
end;

{----------------------------------------------------------------------------------------------}
procedure ClonaDisplays(DSOrg, DSDst: TDataSet; const PDisplayLabel, PDisplayFormat: Boolean);
var
  I: integer;
  F: TField;
begin
  with DSDst do
    for I := 0 to Fields.Count - 1 do
    begin
      F := DSOrg.Fields.FindField(Fields[I].FieldName);
      if F = nil then
        Continue;
      if PDisplayLabel then
        Fields[I].DisplayLabel := F.DisplayLabel;
      if (Fields[I] is TFloatField) and (F is TFloatField) and (PDisplayFormat) then
        TFloatField(Fields[I]).DisplayFormat := TFloatField(F).DisplayFormat;
    end;
end;

{----------------------------------------------------------------------------------------------}
function CopyMin(const PStr: string; const PTotal: integer; const PIndex: integer = 1): string;
begin
  Result := Copy(PStr, PIndex, Length(PStr) - PTotal);
end;

{----------------------------------------------------------------------------------------------}
function BuscaComp(FR: TForm; const pNome: string; const PIndex: integer): TComponent;
begin
  Result := FR.FindComponent(pNome + IntToStr(PIndex));
end;

{----------------------------------------------------------------------------------------------}
function LimpaAspas(const S: string): string;
begin
  Result := FH.LimpaString(S, [#34, #39]);
end;

{----------------------------------------------------------------------------------------------}
function CopyMax(const PStr: string; const PIndex: integer): string;
begin
  Result := Copy(PStr, PIndex, MaxInt);
end;

{----------------------------------------------------------------------------------------------}
function GetConteudoArq(const PArquivo: string): string;
var
  L: TStrings;
begin
  Result := '';
  if (PArquivo = '') or (not FileExists(PArquivo)) then
    exit;
  try
    FH.CriaLista(L);
    L.LoadFromFile(PArquivo);
    Result := L.Text;
  finally
    L.Free;
  end;
end;

{----------------------------------------------------------------------------------------------}
function ComparaTextoFixo(const PVar, PFixo: string): Boolean;
begin
  Result := AnsiSameText(Copy(PVar, 1, Length(PFixo)), PFixo);
end;

{----------------------------------------------------------------------------------------------}
function RetUsuarioWindows: string;
var
  buffer: array[0..20] of Char;
  max: dWord;
begin
  Result := '';
  max := 120;
  if GetUserName(buffer, max) then
    Result := buffer
  else
    Result := 'Erro: ' + IntToStr(GetLastError);
end;

{----------------------------------------------------------------------------------------------}
function EntreChr(const PStr: string; const PC1, PC2: Char; const PEspc: Cardinal = 0): string;
begin
  Result := DupeString(#32, PEspc);
  Result := PC1 + Result + PStr + Result + PC2;
end;

{----------------------------------------------------------------------------------------------}
function EntreCol(const PStr: string; const PEspc: Cardinal = 0): string;
begin
  Result := FH.EntreChr(PStr, #91, #93, PEspc);
end;

{----------------------------------------------------------------------------------------------}
function EntrePar(const PStr: string; const PEspc: Cardinal = 0): string;
begin
  Result := FH.EntreChr(PStr, #40, #41, PEspc);
end;

{----------------------------------------------------------------------------------------------}
function RetDisplayFormat(DS: TDataSet; const pCampo: string): string;
var
  F: TField;
begin
  Result := '';
  F := DS.FindField(pCampo);
  if F = nil then
    exit;
  if F is TFloatField then
    Result := TFloatField(F).DisplayFormat;
end;

{----------------------------------------------------------------------------------------------}
function ProcuraNaLista(Lista: TStrings; const Texto: string): Integer;
var
    c, j, i : Integer;
    m       : TStrings;
begin
    Result := -1;
    if AnsiContainsText(Texto, #$D#$A) then
    begin
        try
            FH.CriaLista(m);
            m.Text := FH.TrimX(Texto, #$D#$A);
            if m.Count > 0 then
            begin
                if m.Count > Lista.Count then
                   Exit
                else
                begin
                    for i := 0 to Lista.Count-1 do
                    begin
                        c := -1;
                        for j := 0 to m.Count-1 do
                        begin
                            if i+j > Lista.Count then
                               Break
                            else
                            if AnsiSameText(m[j], Lista[i+j]) then
                               inc(c)
                            else
                               Break;
                        end;
                        if c = m.Count-1 then
                        begin
                           Result := i;
                           Exit;
                        end;
                    end;
                end;
            end
            else
            begin
                for i := 0 to Lista.Count-1 do
                    if AnsiSameText(Texto, Lista[i]) then
                    begin
                        Result := i;
                        Exit;
                    end;
            end;
        finally
            FH.DestroiObj(m);
        end;
    end
    else
    begin
        for i := 0 to Lista.Count-1 do
            if AnsiSameText(Texto, Lista[i]) then
            begin
                Result := i;
                Exit;
            end;
    end;
end;

{----------------------------------------------------------------------------------------------}
procedure IncExclusivo(Lista: TStrings; const S: string);
begin
    if Lista.IndexOf(S) = -1 then
       Lista.Add(S);
end;
procedure IncExclusivo(CB: TComboBox; const S: string);
begin
    FH.IncExclusivo(CB.Items, S);
end;

{----------------------------------------------------------------------------------------------}
procedure SetaComboItemIndex(CB: TComboBox; const PIndex: integer);
var
  A: integer;
begin
  A := CB.Items.Count;
  if A = 0 then
    CB.ItemIndex := -1
  else
  begin
    Dec(A);
    if PIndex > A then
      CB.ItemIndex := A
    else
      CB.ItemIndex := PIndex;
  end;
end;

{----------------------------------------------------------------------------------------------}
function Match(const pTexto: string; const pValores: array of string): Boolean;
begin
     Result := AnsiMatchText(pTexto, pValores);
end;

{----------------------------------------------------------------------------------------------}
function Contem(const pTexto, pSubtexto: string): Boolean;
begin
     Result := AnsiContainsText(pTexto, pSubtexto);
end;

{----------------------------------------------------------------------------------------------}
function NaoContem(const pTexto, pSubtexto: string): Boolean;
begin
     Result := not AnsiContainsText(pTexto, pSubtexto);
end;

{----------------------------------------------------------------------------------------------}
function Igual(const pTexto, pSubtexto: string): Boolean;
begin
     Result := AnsiSameText(pTexto, pSubtexto);
end;

{----------------------------------------------------------------------------------------------}
function NaoIgual(const pTexto, pSubtexto: string): Boolean;
begin
     Result := not AnsiSameText(pTexto, pSubtexto);
end;

{----------------------------------------------------------------------------------------------}
function ContemTextoIndex(const pTexto: string; const PValores: array of string): integer;
var
  I: integer;
begin
  Result := -1;
  for I := Low(PValores) to High(PValores) do
    if AnsiContainsText(pTexto, PValores[I]) then
    begin
      Result := I;
      Break;
    end;
end;

{----------------------------------------------------------------------------------------------}
function ContemTexto(const pTexto: string; const PValores: array of string): Boolean;
begin
  Result := FH.ContemTextoIndex(pTexto, PValores) <> -1;
end;

{----------------------------------------------------------------------------------------------}
function IncExt(const S, PExt: string): string;
begin
  Result := S;
  if ExtractFileExt(Result) <> '' then
    Result := FH.DeletaExt(Result);
  if RightStr(Result, 1) <> '.' then
    Result := Result + IfThen(Pos('.', PExt) = 0, '.') + PExt
  else
    Result := Result + IfThen(Pos('.', PExt) = 0, PExt, RightStr(PExt, Length(PExt) - 1));
end;

{----------------------------------------------------------------------------------------------}
procedure SetaEnabled(const PEnabled: Boolean; AControls: array of TControl);
var
  i: integer;
begin
  for i := low(AControls) to high(AControls) do
    AControls[i].Enabled := PEnabled;
end;

{----------------------------------------------------------------------------------------------}
procedure SetaEnabledContainer(PContainer: TWinControl; const PEnabled: Boolean);
var
  i: integer;
begin
  for i := 0 to PContainer.ControlCount - 1 do
    PContainer.Controls[i].Enabled := PEnabled;
end;

{----------------------------------------------------------------------------------------------}
function DatasIguais(const D1, D2: TDateTime): Boolean;
begin
     Result := DateOf(D1) = DateOf(D2);
end;

{----------------------------------------------------------------------------------------------}
function DataNoIntervalo(const PData, PMin, PMax: TDateTime): Boolean;
begin
     Result := (PData >= PMin) and (PData <= PMax);
end;

{----------------------------------------------------------------------------------------------}
function UltimoDiaMes(const PData: TDateTime = 0): TDateTime;
var
  D: TDateTime;
begin
  D := Date;
  if PData <> 0 then
    D := PData;
  Result := EncodeDate(YearOf(D), MonthOf(D), FH.UltimoDia(D));
end;

{----------------------------------------------------------------------------------------------}
function UltimoDiaMesStr(const PData: TDateTime = 0): string;
var
  D: TDateTime;
begin
  D := Date;
  if PData <> 0 then
    D := PData;
  Result := DateToStr(EncodeDate(YearOf(D), MonthOf(D), FH.UltimoDia(D)));
end;

{----------------------------------------------------------------------------------------------}
function PrimeiroDiaMesAno(const PMesAno: string): string;
begin
  if FH.MesAnoValido(PMesAno) then
    Result := '01/' + PMesAno
  else
    Result := '';
end;

{----------------------------------------------------------------------------------------------}
function UltimoDiaMesAno(const PMesAno: string): string;
begin
  if FH.MesAnoValido(PMesAno) then
    Result := FH.UltimoDiaMesStr(StrToDate('01/' + PMesAno))
  else
    Result := '';
end;

{----------------------------------------------------------------------------------------------}
function GeraSQLIN(DS: TDataSet; const pCampo: string; const EntrePar: Boolean = true; const Espacado: Boolean = false): string;
var
  tmp: string;
begin
  Result := '';
  with DS do
  begin
    first;
    while not eof do
    begin
      tmp := FieldByName(pCampo).AsString;
      if FieldByName(pCampo) is TStringField then
        tmp := QuotedStr(tmp);
      Result := Result + ifthen(Result > '', ',' +
                                ifthen(Espacado, #32)) + tmp;
      next;
    end;
    if EntrePar then
      Result := FH.EntrePar(Result);
  end;
end;

{----------------------------------------------------------------------------------------------}
function GeraSQLOR(DS: TDataSet; const pCampo: string; const pAlias: string = ''): string;
var
  tmp, vapl, sTemp, camDS, camRF: string;
  ls: TStrings;
  ii: Integer;
begin
  Result := '';
  try
    FH.CriaLista(ls);
    sTemp := pCampo;
    sTemp := StringReplace(sTemp, ';', ',', [rfReplaceAll]);
    if AnsiContainsText(pCampo, ',') then
      ls.Text := FH.StrParaLista(sTemp).Text
    else
      ls.Add(sTemp);

    vapl := pAlias;
    if vapl > '' then
      vapl := vapl + '.';

    with DS do
    begin
      if IsEmpty then
        Exit;

      first;
      while not eof do
      begin
        if Result > '' then
          Result := Result + #13' or ';

        Result := Result + '(';

        for ii := 0 to ls.Count-1 do
        begin
          if AnsiContainsText(ls[ii], '=') then
          begin
            camDS := FH.CopyPos   (ls[ii], '=');
            camRF := FH.CopyPosMax(ls[ii], '=');
          end
          else
          begin
            camDS := ls[ii];
            camRF := ls[ii];
          end;

          tmp := FieldByName(camDS).AsString;
          if FieldByName(camDS) is TStringField then
            tmp := QuotedStr(tmp);

          if ii > 0 then
            Result := Result + ' and ';

          Result := Result + vapl + camRF + ' = ' + tmp;
        end;

        Result := Result + ')';
        next;
      end;
      Result := '( ' + Result + ' )';
    end;
  finally
    FH.DestroiObj(ls);
  end;
end;

{----------------------------------------------------------------------------------------------}
function GetNumeroOrdinal(const PNro: ShortInt; const PFeminino: Boolean = false): string;
const
  cUni: array[1..9] of string = ('primeiro', 'segundo', 'terceiro', 'quarto', 'quinto', 'sexto', 'sétimo', 'oitavo', 'nono');
  cDec: array[1..9] of string = ('décimo', 'vigésimo', 'trigésimo', 'quadragésimo', 'quinquagésimo', 'sextuagésimo', 'septuagésimo', 'octagésimo', 'nonagésimo');
  cCen: array[1..1] of string = ('centésimo');
var
  S: string;
  I: integer;
begin
  Result := '';
  if PNro < 1 then
    exit;
  S := IntToStr(PNro);
  if Length(S) = 3 then
  begin
    I := StrToInt(Copy(S, 1, 1));
    if InRange(I, Low(cCen), High(cCen)) then
    begin
      Result := cCen[I];
      if PFeminino then
      begin
        Delete(Result, Length(Result), 1);
        Result := Result + 'a';
      end;
    end;
    Delete(S, 1, 1);
    if Copy(S, 1, 1) = '0' then
      Delete(S, 1, 1);
  end;
  if (Length(S) = 2) and (S <> '00') then
  begin
    if Result <> '' then
      Result := Result + '-';
    Result := Result + cDec[StrToInt(Copy(S, 1, 1))];
    if PFeminino then
    begin
      Delete(Result, Length(Result), 1);
      Result := Result + 'a';
    end;
    Delete(S, 1, 1);
  end;
  if (Length(S) = 1) and (S <> '0') then
  begin
    if Result <> '' then
      Result := Result + '-';
    Result := Result + cUni[StrToInt(Copy(S, 1, 1))];
    if PFeminino then
    begin
      Delete(Result, Length(Result), 1);
      Result := Result + 'a';
    end;
    Delete(S, 1, 1);
  end;
end;

{----------------------------------------------------------------------------------------------}
function FonteExiste(const PNomeFone: string): Boolean;
begin
  Result := Screen.Fonts.IndexOf(PNomeFone) > -1;
end;

{----------------------------------------------------------------------------------------------}
procedure RetBitmap(PImgLst: TImageList; const PIndex: Integer; ABmp: TBitmap);
begin
  PImgLst.GetBitmap(PIndex, ABmp);
end;

{----------------------------------------------------------------------------------------------}
function RetIcon(PImgLst: TImageList; const PIndex: integer): TIcon;
begin
  Result := TIcon.Create;
  PImgLst.GetIcon(PIndex, Result);
end;

{----------------------------------------------------------------------------------------------}
function RetColumn(GR: TDBGrid; const PNomeCampo: string): TColumn;
var
  i: integer;
begin
  Result := nil;
  for i := 0 to GR.Columns.Count - 1 do
    if AnsiSameText(GR.Columns[i].FieldName, PNomeCampo) then
    begin
      Result := GR.Columns[i];
      exit;
    end;
end;

{----------------------------------------------------------------------------------------------}
procedure LimpaBufferTeclado(FR: TForm);
var
  Msg: TMsg;
begin
  if FR = nil then
    exit;
  while PeekMessage(Msg, FR.Handle, WM_KEYFIRST, WM_KEYLAST, PM_REMOVE or PM_NOYIELD) do
    ;
end;

{----------------------------------------------------------------------------------------------}
function CopyPosMax(const PStr, PSubStr: string; const PTrim: Boolean = true): string;
var
  i: integer;
begin
  i := FH.GetPos(PSubStr, PStr);
  if i > 0 then
    Result := copy(PStr, i + Length(PSubStr), MaxInt)
  else
    Result := '';
  if PTrim then
    Result := Trim(Result);
end;

{----------------------------------------------------------------------------------------------}
procedure LimpaBufferMouse(FR: TForm);
var
  Msg: TMsg;
begin
  if FR = nil then
    exit;
  while PeekMessage(Msg, FR.Handle, WM_MOUSEFIRST, WM_MOUSELAST, PM_REMOVE or PM_NOYIELD) do
    ;
end;

{----------------------------------------------------------------------------------------------}
procedure AtuForms;
var
  I: integer;
begin
    for I := 0 to Screen.FormCount - 1 do
        if Screen.Forms[i].ClassName = 'TForm' then
           Screen.Forms[I].Repaint;
end;

{----------------------------------------------------------------------------------------------}
function MontaFonte(const PNomeFone: string; const PTam: integer;
  const PColor: TColor; const PEstiloFonte: TFontStyles): TFont;
begin
  Result := TFont.Create;
  with Result do
  begin
    Name := PNomeFone;
    Size := PTam;
    Color := PColor;
    Style := PEstiloFonte;
  end;
end;

{----------------------------------------------------------------------------------------------}
function TemaXpAtivo: Boolean;
const
  themelib = 'uxtheme.dll';
type
  TIsThemeActive = function: BOOL; stdcall;
var
  IsThemeActive: TIsThemeActive;
  hUxTheme: HINST;
begin
  Result := false;
  if (Win32Platform = VER_PLATFORM_WIN32_NT) and
    (((Win32MajorVersion = 5) and (Win32MinorVersion >= 1)) or
    (Win32MajorVersion > 5)) then
  begin
    hUxTheme := LoadLibrary(themelib);
    if hUxTheme <> 0 then
    begin
      try
        IsThemeActive := GetProcAddress(hUxTheme, 'IsThemeActive');
        Result := IsThemeActive;
      finally
        if hUxTheme > 0 then
          FreeLibrary(hUxTheme);
      end;
    end;
  end;
end;

{----------------------------------------------------------------------------------------------}
function GeraCripto(const PWD: string; const Decrypt: Boolean;
  const TamRslt: integer = -1): string;
const
  cNum = '0123456789';
  cCod = 'AJFEGXQLIS';
var
  I, J: integer;
  S, M: string;
begin
  if not Decrypt then
  begin
    J := 0;
    S := '';
    Result := '';
    for I := 1 to Length(PWD) do
    begin
      J := Ord(PWD[I]);
      J := ((J * 9) - 7) * 3;
      S := IntToStr(J);
      S := StringOfChar('0', 4 - Length(S)) + S;
      Result := Result + S;
    end;
    for I := 1 to Length(Result) do
      Result[I] := cCod[Pos(Result[I], cNum)];
    while Length(Result) < TamRslt do
    begin
      J := RandomRange(1, Length(Result) + 1);
      if Odd(Length(Result)) then
      begin
        S := Chr(65);
        while FH.GetPos(S, cCod) > 0 do
          S := Chr(RandomRange(65, 90));
        Insert(S, Result, J);
      end
      else
        Insert(IntToStr(RandomRange(0, 9)), Result, J);
    end;
  end
  else
  begin
    J := 0;
    S := '';
    M := '';
    Result := '';
    M := PWD;
    for I := 0 to 9 do
      M := StringReplace(M, IntToStr(I), '', [rfReplaceAll]);
    for I := 65 to 90 do
      if FH.GetPos(Chr(I), cCod) = 0 then
        M := StringReplace(M, Chr(I), '', [rfReplaceAll]);
    I := 1;
    M := Trim(M);
    while I <= Length(M) do
    begin
      S := Copy(M, I, 4);
      for J := 1 to Length(S) do
        S[J] := cNum[FH.GetPos(S[J], cCod)];
      J := StrToInt(S);
      J := ((J div 3) + 7) div 9;
      Result := Result + Chr(J);
      Inc(I, 4);
    end;
  end;
end;

{----------------------------------------------------------------------------------------------}
function GetTamDetalhadoArq(const PExec: string): string;
var
  vAtr: TSearchRec;


  function RetTam(P: Byte): string;
  var
    R: double;
  begin
    case P of
      1:
        begin
          R := vAtr.Size / 1024;
          R := Round(R);
          Result := FloatToStrF(R, ffNumber, 11, 0) + ' KB';
        end;
      2:
        begin
          R := (vAtr.Size / 1024) / 1024;
          Result := FloatToStrF(R, ffNumber, 11, 1) + ' MB';
        end;
      3:
        begin
          R := vAtr.Size;
          R := Round(R);
          Result := FloatToStrF(R, ffNumber, 11, 0) + ' bytes';
        end;
    end;
  end;


begin
  FindFirst(PExec, faAnyFile, vAtr);
  Result := RetTam(2) + ' (' + RetTam(1) + '/' + RetTam(3) + ')';
end;

{----------------------------------------------------------------------------------------------}
function VerificaCartaoCredito(const pNumero: string): Boolean;
var
  vVlr, vSoma, vMltp, T, J: integer;
begin
  Result := false;
  vMltp := 2;
  vSoma := 0;
  T := Length(Trim(pNumero));
  for J := 1 to T - 1 do
  begin
    try
      vVlr := StrToInt(Copy(pNumero, J, 1)) * vMltp;
    except
      vVlr := 0;
    end;
    vSoma := vSoma + (vVlr div 10) + (vVlr mod 10);
    if vMltp = 1 then
      vMltp := 2
    else
      vMltp := 1;
  end;
  if IntToStr((10 - (vSoma mod 10)) mod 10) = Copy(pNumero, T, 1) then
    Result := true;
end;

{----------------------------------------------------------------------------------------------}
function VerItemCL(CL: TCheckListBox; const pTexto: string;
  const PFullItem: Boolean; const PDivisor: string = #32): Boolean;
var
  I: integer;
begin
  Result := false;
  for I := 0 to CL.Items.Count - 1 do
  begin
    if PFullItem then
    begin
      if AnsiSameText(pTexto, CL.Items[I]) then
        if CL.Checked[I] then
        begin
          Result := true;
          exit;
        end;
    end
    else
    begin
      if AnsiSameText(pTexto, FH.RetCodigoStr(CL.Items[I], PDivisor)) then
        if CL.Checked[I] then
        begin
          Result := true;
          exit;
        end;
    end;
  end;
end;

{----------------------------------------------------------------------------------------------}
function VerItemLV(LV: TListView; const pTexto: string): Boolean;
var
  I: integer;
begin
  Result := false;
  for I := 0 to LV.Items.Count - 1 do
  begin
    if AnsiSameText(pTexto, LV.Items[I].Caption) then
      if LV.Items[I].Checked then
      begin
        Result := true;
        exit;
      end;
  end;
end;

{----------------------------------------------------------------------------------------------}
function BuscaChecklist(CL: TCheckListBox; const pTexto: string;
  const PFullItem: Boolean; const PDivisor: string = #32): Boolean;
var
  I: integer;
  temp: string;
begin
  Result := false;
  for I := 0 to CL.Items.Count - 1 do
  begin
    if PFullItem then
    begin
      if AnsiSameText(pTexto, CL.Items[I]) then
      begin
        CL.ItemIndex := I;
        Result := true;
        exit;
      end;
    end
    else
    begin
      if (PDivisor = #0) or (not AnsiContainsText(CL.Items[I], PDivisor)) then
        temp := CL.Items[I]
      else
        temp := FH.RetCodigoStr(CL.Items[I], PDivisor);
      if AnsiSameText(pTexto, LeftStr(temp, Length(pTexto))) then
      begin
        CL.ItemIndex := I;
        Result := true;
        exit;
      end;
    end;
  end;
  CL.ItemIndex := -1;
end;

{----------------------------------------------------------------------------------------------}
function VerCheckedCL(CL: TCheckListBox): Boolean;
var
  I: integer;
begin
  Result := false;
  for I := 0 to CL.Items.Count - 1 do
  begin
    if CL.Checked[I] then
    begin
      Result := true;
      exit;
    end;
  end;
end;

{----------------------------------------------------------------------------------------------}
function VerCheckedLV(LV: TListView): Boolean;
var
  I: integer;
begin
  Result := false;
  if not LV.Checkboxes then
    exit;
  if LV.Items.Count = 0 then
    exit;
  for I := 0 to LV.Items.Count - 1 do
  begin
    if LV.Items[I].Checked then
    begin
      Result := true;
      exit;
    end;
  end;
end;

{----------------------------------------------------------------------------------------------}
function VerCheckedAllLV(LV: TListView): Boolean;
var
  I: integer;
begin
  Result := false;
  if not LV.Checkboxes then
    exit;
  if LV.Items.Count = 0 then
    exit;
  for I := 0 to LV.Items.Count - 1 do
  begin
    if not LV.Items[I].Checked then
      exit;
  end;
  Result := true;
end;

{----------------------------------------------------------------------------------------------}
function VerCheckedAllCL(CL: TCheckListBox): Boolean;
var
  I: integer;
begin
  Result := false;
  if CL.Items.Count = 0 then
    exit;
  for I := 0 to CL.Items.Count - 1 do
  begin
    if not CL.Checked[I] then
      exit;
  end;
  Result := true;
end;

{----------------------------------------------------------------------------------------------}
function DeletaDiretorio(const PDir: string): Boolean;
var
  fos: TSHFileOpStruct;
begin
  ZeroMemory(@fos, SizeOf(fos));
  with fos do
  begin
    wFunc := FO_DELETE;
    fFlags := FOF_SILENT or FOF_NOCONFIRMATION;
    pFrom := PChar(PDir + #0);
  end;
  Result := (0 = ShFileOperation(fos));
end;

{----------------------------------------------------------------------------------------------}
function TrocaNomeArqEx(const PDirArqOrigem, PDirArqDestino: string): Boolean;
begin
  Result := MoveFile(PChar(PDirArqOrigem), PChar(PDirArqDestino));
end;

{----------------------------------------------------------------------------------------------}
function GetPrimeiroCampoIndex(const PIdxFlNm: string): string;
begin
  if Pos(';', PIdxFlNm) = 0 then
    Result := PIdxFlNm
  else
    Result := FH.CopyPos(PIdxFlNm, ';');
end;

{----------------------------------------------------------------------------------------------}
function BlockInput(fBlockIt: Boolean): Boolean;
type
  TCacheFunction = function (fBlockIt: BOOL): Boolean; stdcall;
const
  ND = 'USER32.DLL';
var
  Hnd   : THandle;
  Fpr   : TFarProc;
  fCache: TCacheFunction;
begin
  Result := true;
  Hnd := SafeLoadLibrary(nd);
  if Hnd > 0 then
  begin
    Fpr := GetProcAddress(Hnd, 'BlockInput');
    if Fpr <> nil then
    begin
      fCache := TCacheFunction(Fpr);
      Result := fCache(fBlockIt);
    end;
  end;
end;

{----------------------------------------------------------------------------------------------}
function ValoresIdenticos(const PValor1, PValor2: extended; const pDec: Integer = 2): Boolean;
begin
  Result := FH.FmtFix(PValor1, pDec) = FH.FmtFix(PValor2, pDec);
end;

{----------------------------------------------------------------------------------------------}
procedure DestroiObj(var PObj);
var
  temp: TObject;
begin
  temp := TObject(PObj);
  if temp <> nil then
    try
      FreeAndNil(temp);
    except
    end;
end;

{----------------------------------------------------------------------------------------------}
procedure DestroiMultiObj(Objs: array of TObject);
var
  ii: Integer;
begin
  for ii := Low(Objs) to High(Objs) do
    FH.DestroiObj(Objs[ii]);
end;

{----------------------------------------------------------------------------------------------}
procedure GeraCodBarEAN13(PCanvas: TCanvas; pNumero: string;
  PPosX, PPosY: integer; PAlt: double);
const
  cnBarVector: array[1..3] of array[0..9] of string =
               (('0001101', '0011001', '0010011', '0111101', '0100011',
                 '0110001', '0101111', '0111011', '0110111', '0001011'),
                 ('0100111', '0110011', '0011011', '0100001', '0011101',
                 '0111001', '0000101', '0010001', '0001001', '0010111'),
                 ('1110010', '1100110', '1101100', '1000010', '1011100',
                 '1001110', '1010000', '1000100', '1001000', '1110100'));
  cnSelectionTable: array[0..9] of string =
                    ('111111', '112122', '112212', '112221', '121122',
                      '122112', '122211', '121212', '121221', '122121');
  cnStartEnd: string = '101';
  cnMiddle: string = '01010';

var
  i, j: integer;
  vBarSize: integer;
  vCurTable: integer;
  vSwappedX, vSwappedY: integer;

  function SwapX(Medida: double): integer;
  begin
    Result := Trunc(vSwappedX / 25.4 * Medida);
  end;

  function SwapY(Medida: double): integer;
  begin
    Result := Trunc(vSwappedY / 25.4 * Medida);
  end;

  function CalcDigit(PSequence: string): Char;
  var
    ii: integer;
    vAmount: integer;
    vPivot: integer;
  begin
    vAmount := 0;
    vPivot := 3;
    for ii := Length(PSequence) downto 1 do
    begin
      vAmount := vAmount + (Ord(PSequence[ii]) - Ord('0')) * vPivot;
      if vPivot = 3 then
        vPivot := 1
      else
        vPivot := 3;
    end;
    if vAmount mod 10 <> 0 then
      Result := Chr(10 - vAmount mod 10 + Ord('0'))
    else
      Result := '0';
  end;

begin
  vSwappedX := GetDeviceCaps(PCanvas.Handle, LogPixelsX);
  vSwappedY := GetDeviceCaps(PCanvas.Handle, LogPixelsY);

  PCanvas.Pen.Color := clBlack;
  PCanvas.Pen.Style := psInsideFrame;
  PCanvas.Brush.Color := clBlack;
  while length(pNumero) < 12 do
    pNumero := '0' + pNumero;
  if Length(pNumero) = 12 then
    pNumero := pNumero + CalcDigit(pNumero);
  if Length(pNumero) <> 13 then
    raise Exception.CreateFmt('O número informado excede o tamanho do padrão EAN-13: ''%s''', [pNumero]);
  vBarSize := SwapX(0.3);
  for i := 1 to Length(cnStartEnd) do
  begin
    if cnStartEnd[i] = '1' then
      PCanvas.FillRect(Rect(PPosX, PPosY, PPosX + vBarSize, PPosY + SwapY(PAlt + 3)));
    PPosX := PPosX + vBarSize;
  end;
  for i := 2 to length(pNumero) do
  begin
    if i <= 7 then
      vCurTable := Ord(cnSelectionTable[Ord(pNumero[1]) - Ord('0')][i - 1]) - Ord('0')
    else
      vCurTable := 3;
    for j := 1 to 7 do
    begin
      if cnBarVector[vCurTable, Ord(pNumero[i]) - Ord('0')][j] = '1' then
        PCanvas.FillRect(Rect(PPosX, PPosY, PPosX + vBarSize, PPosY + SwapY(PAlt)));
      PPosX := PPosX + vBarSize;
    end;
    if i = 7 then
      for j := 1 to Length(cnMiddle) do
      begin
        if cnMiddle[j] = '1' then
          PCanvas.FillRect(Rect(PPosX, PPosY, PPosX + vBarSize, PPosY + SwapY(PAlt + 3)));
        PPosX := PPosX + vBarSize;
      end;
  end;
  for i := 1 to Length(cnStartEnd) do
  begin
    if cnStartEnd[i] = '1' then
      PCanvas.FillRect(Rect(PPosX, PPosY, PPosX + vBarSize, PPosY + SwapY(PAlt + 3)));
    PPosX := PPosX + vBarSize;
  end;
end;

{----------------------------------------------------------------------------------------------}
function GetDiaSemanaExtenso(const PDia: integer): string;
begin
  Result := '<Dia inválido>';
  case PDia of
    1: Result := 'Domingo';
    2: Result := 'Segunda-feira';
    3: Result := 'Terça-feira';
    4: Result := 'Quarta-feira';
    5: Result := 'Quinta-feira';
    6: Result := 'Sexta-feira';
    7: Result := 'Sábado';
  end;
end;

{----------------------------------------------------------------------------------------------}
function DiaUtil(PData: TDatetime): Boolean;
begin
  Result := DayOfTheWeek(PData) < 6;
end;

{----------------------------------------------------------------------------------------------}
function DiaUtilProximo(PData: TDatetime): TDatetime;
begin
   while not FH.DiaUtil(PData) do
     PData := IncDay(PData, 1);

   Result := PData;
end;

{----------------------------------------------------------------------------------------------}
function DiaUtilAnterior(PData: TDatetime): TDateTime;
begin
   while not FH.DiaUtil(PData) do
     PData := IncDay(PData, -1);

   Result := PData;
end;

{----------------------------------------------------------------------------------------------}
function DiaBaseProximo(PData: TDatetime; PDia: Integer): TDatetime;
var
   vDia : Integer;
begin
   // 1=segunda,2=terça...
   IF PDia > 0 then
   begin
     vDia := DayOfTheWeek(PData);
     while vDia <> PDia do
     begin
       PData := PData + 1;
       vDia  := DayOfTheWeek(PData);
     end;
   end;

   Result := PData;
end;

{----------------------------------------------------------------------------------------------}
function VerificaEntradaArq(const pNomeArq: string; pCampo: TWinControl): Boolean;
begin
  Result := false;
  if FH.StrInvalida(pNomeArq) then
  begin
    FH.PrMsg_Focalize(pCampo, 'Arquivo inválido');
    exit;
  end;
  if not FileExists(pNomeArq) then
  begin
    FH.PrMsg_Focalize(pCampo, Format('O arquivo %s não existe.', [QuotedStr(pNomeArq)]));
    exit;
  end;
  Result := true;
end;

{----------------------------------------------------------------------------------------------}
function VerificaEntradaDir(const PNomeDir: string; pCampo: TWinControl): Boolean;
begin
  Result := false;
  if FH.StrInvalida(PNomeDir) then
  begin
    FH.PrMsg_Focalize(pCampo, 'Diretório inválido');
    exit;
  end;
  if not DirectoryExists(PNomeDir) then
  begin
    FH.PrMsg_Focalize(pCampo, Format('O diretório %s não existe.', [QuotedStr(PNomeDir)]));
    exit;
  end;
  Result := true;
end;

{----------------------------------------------------------------------------------------------}
procedure CopiaParaAreaTransf(const S: string);
begin
  Clipboard.SetTextBuf(PChar(S));
end;

{----------------------------------------------------------------------------------------------}
function DeletaUltimoSubdir(const PDir: string): string;
var
  L: string;
begin
  L := ExcludeTrailingPathDelimiter(ExtractFilePath(PDir));
  Result := FH.IncPathDel(LeftStr(L, LastDelimiter('\.:', L) - 1));
end;

{----------------------------------------------------------------------------------------------}
function PreparaValorValido(const PStr: string; const pDec: SmallInt;
  const PSepMilhar: Boolean): string;
var
  vDec: SmallInt;
begin
  Result := PStr;
  if FH.StrInvalida(Result) then
  begin
    Result := FH.FmtFix(0, pDec);
    exit;
  end;
  vDec := pDec;
  if vDec < 1 then vDec := 0;
  if vDec > 0 then
    System.Insert(DecimalSeparator, Result, Length(Result) - (vDec - 1));
  Result := FH.LimpaZero(Result);
  if FH.FloatOk(Result) then
  begin
    Result := FH.FmtFix(StrToFloat(Result), vDec);
    if PSepMilhar then
      Result := FH.FmtNum(StrToFloat(Result), vDec);
  end;
end;

{----------------------------------------------------------------------------------------------}
function TerminaWindows(RebootParam: Longword): Boolean;
var
  TTokenHd: THandle;
  TTokenPvg: TTokenPrivileges;
  cbtpPrevious: DWORD;
  rTTokenPvg: TTokenPrivileges;
  pcbtpPreviousRequired: DWORD;
  tpResult: Boolean;
const
  SE_SHUTDOWN_NAME = 'SeShutdownPrivilege';
begin
   if Win32Platform = VER_PLATFORM_WIN32_NT then
   begin
     tpResult := OpenProcessToken(GetCurrentProcess(),
                                  TOKEN_ADJUST_PRIVILEGES or TOKEN_QUERY,
                                  TTokenHd);
     if tpResult then
     begin
       tpResult := LookupPrivilegeValue(nil,
                                        SE_SHUTDOWN_NAME,
                                        TTokenPvg.Privileges[0].Luid);
       TTokenPvg.PrivilegeCount := 1;
       TTokenPvg.Privileges[0].Attributes := SE_PRIVILEGE_ENABLED;
       cbtpPrevious := SizeOf(rTTokenPvg);
       pcbtpPreviousRequired := 0;
       if tpResult then
         Windows.AdjustTokenPrivileges(TTokenHd,
                                       false,
                                       TTokenPvg,
                                       cbtpPrevious,
                                       rTTokenPvg,
                                       pcbtpPreviousRequired);
     end;
   end;
   Result := ExitWindowsEx(RebootParam, 0);
end;

{----------------------------------------------------------------------------------------------}
procedure GeraArqTexto(const pNomeArq, pTexto: string);
var
  L: TStrings;
begin
  try
    FH.CriaLista(L);
    L.Text := pTexto;
    L.SaveToFile(pNomeArq);
  finally
    FH.DestroiObj(L);
  end;
end;

{----------------------------------------------------------------------------------------------}
function ExcLinhasEmBranco(const pLinhas: string): string;
var
  L: TStrings;
  i: Integer;
begin
  try
    FH.CriaLista(L);
    L.Text := pLinhas;
    i := 0;
    while i <= L.Count-1 do
    begin
         if FH.StrInvalida(L[i]) then
            L.Delete(i)
         else
            inc(i);
    end;
    Result := L.Text;
  finally
    FH.DestroiObj(L);
  end;
end;

{----------------------------------------------------------------------------------------------}
function DesligaWindows: Boolean;
begin
  Result := FH.TerminaWindows(EWX_SHUTDOWN or EWX_FORCE);
end;

{----------------------------------------------------------------------------------------------}
function VerificaTeclaMov(const pKey: Word): Boolean;
begin
  Result := FH.BuscaArrayInt(pKey, [VK_UP, VK_DOWN, VK_PRIOR, VK_NEXT, VK_HOME,
                                    VK_END, VK_LEFT, VK_RIGHT]);
end;

{----------------------------------------------------------------------------------------------}
procedure ReposCompsHrz(PComps: array of TWinControl);
var
  leftEdge, rightEdge, i, delta, beta: integer;
begin
  with PComps[low(PComps)] do
    leftEdge := Left + (Width div 2);
  with PComps[high(PComps)] do
    rightEdge := Left + (Width div 2);
  if high(PComps) < 2 then
    exit;
  beta  := ((rightEdge - leftEdge) div high(PComps));
  delta := beta+5;
  for i := (low(PComps) + 1) to (high(PComps) - 1) do
  begin
    PComps[i].Left := delta;
    inc(delta, beta);
  end;
end;

{----------------------------------------------------------------------------------------------}
function GetAveCharSize( Canvas: TCanvas ): TPoint;
var
  I: integer;
  Buffer: array[0..51] of Char;
begin
  for I := 0 to 25 do
    Buffer[I] := Chr( I + Ord( 'A' ) );
  for I := 0 to 25 do
    Buffer[I + 26] := Chr( I + Ord( 'a' ) );
  GetTextExtentPoint( Canvas.Handle, Buffer, 52, TSize( Result ) );
  Result.X := Result.X div 52;
end;

{----------------------------------------------------------------------------------------------}
type
  TMessageForm = class( TForm )
  private
    Message: TLabel;
    procedure HelpButtonClick( Sender: TObject );
  protected
    procedure CustomKeyDown( Sender: TObject; var Key: Word; Shift: TShiftState );
    procedure WriteToClipBoard( Text: string );
    function GetFormText: string;
  public
    constructor CreateNew( POwner: TComponent ); reintroduce;
  end;

constructor TMessageForm.CreateNew( POwner: TComponent );
var
  NonClientMetrics: TNonClientMetrics;
begin
  inherited CreateNew( POwner );
  NonClientMetrics.cbSize := sizeof( NonClientMetrics );
  if SystemParametersInfo( SPI_GETNONCLIENTMETRICS, 0, @NonClientMetrics, 0 ) then
    Font.Handle := CreateFontIndirect( NonClientMetrics.lfMessageFont );
end;

{----------------------------------------------------------------------------------------------}
procedure TMessageForm.HelpButtonClick( Sender: TObject );
begin
  Application.HelpContext( HelpContext );
end;

{----------------------------------------------------------------------------------------------}
procedure TMessageForm.CustomKeyDown( Sender: TObject;
                                      var Key: Word;
                                      Shift: TShiftState );
begin
  if ( Shift = [ssCtrl] ) and ( Key = Word( 'C' ) ) then
  begin
    SysUtils.Beep;
    WriteToClipBoard( GetFormText );
  end;
end;

{----------------------------------------------------------------------------------------------}
procedure TMessageForm.WriteToClipBoard( Text: string );
var
  Data: THandle;
  DataPtr: Pointer;
begin
  if OpenClipBoard( 0 ) then
  begin
    try
      Data := GlobalAlloc( GMEM_MOVEABLE + GMEM_DDESHARE, Length( Text ) + 1 );
      try
        DataPtr := GlobalLock( Data );
        try
          Move( PChar( Text )^, DataPtr^, Length( Text ) + 1 );
          EmptyClipBoard;
          SetClipboardData( CF_TEXT, Data );
        finally
          GlobalUnlock( Data );
        end;
      except
        GlobalFree( Data );
        raise;
      end;
    finally
      CloseClipBoard;
    end;
  end
  else
    raise Exception.CreateRes( @SCannotOpenClipboard );
end;

{----------------------------------------------------------------------------------------------}
function TMessageForm.GetFormText: string;
var
  DividerLine, ButtonCaptions: string;
  I: integer;
begin
  DividerLine := StringOfChar( '-', 27 ) + sLineBreak;
  for I := 0 to ComponentCount - 1 do
    if Components[I] is TButton then
      ButtonCaptions := ButtonCaptions + TButton( Components[I] ).Caption + StringOfChar( ' ', 3 );
  ButtonCaptions := StringReplace( ButtonCaptions, '&', '', [rfReplaceAll] );
  Result := Format( '%s%s%s%s%s%s%s%s%s%s', [DividerLine, Caption, sLineBreak,
                                             DividerLine, Message.Caption,
                                             sLineBreak, DividerLine,
                                             ButtonCaptions, sLineBreak,
                                             DividerLine] );
end;

var
  sLgnd: array[TIconeMsg] of string = (
                                           'Atenção',
                                           'Erro',
                                           'Informação',
                                           'Confirmação',
                                           ''
                                          );
  idsIcones: array[TIconeMsg] of PChar = (
                                        IDI_EXCLAMATION, IDI_HAND,
                                        IDI_ASTERISK, IDI_QUESTION, nil
                                       );
  sNomeBtns: array[TBotaoMsg] of string = (
                                              'Yes', 'No', 'OK', 'Cancel',
                                              'Abort', 'Retry', 'Ignore',
                                              'All', 'NoToAll', 'YesToAll',
                                              'Help'
                                             );
  sLgndBtns: array[TBotaoMsg] of string =  (
                                                  'Sim',
                                                  'Não',
                                                  'OK',
                                                  'Cancelar',
                                                  'Abortar',
                                                  'Refazer',
                                                  'Ignorar',
                                                  'Todos',
                                                  'Não para todos',
                                                  'Sim para todos',
                                                  'Ajuda'
                                                 );
  MdlRslt: array[TBotaoMsg] of integer = (
                                                mrYes, mrNo, mrOk, mrCancel,
                                                mrAbort, mrRetry, mrIgnore,
                                                mrAll, mrNoToAll, mrYesToAll, 0
                                               );
var
  iLarBtns: array[TBotaoMsg] of integer;

{----------------------------------------------------------------------------------------------}
function MontaMsgPersonalizada(const Msg: string; DlgType: TIconeMsg; Buttons: TGrupoBotaoMsg;
  const PFonteMsg, PFonteBtn: TFont): TForm;
const
  mcHorzMargin = 8;
  mcVertMargin = 8;
  mcHorzSpacing = 10;
  mcVertSpacing = 10;
  mcButtonWidth = 50;
  mcButtonHeight = 14;
  mcButtonSpacing = 4;
var
  DialogUnits: TPoint;

  HorzMargin, VertMargin, HorzSpacing, VertSpacing, ButtonWidth,
    ButtonHeight, ButtonSpacing, ButtonCount, ButtonGroupWidth,
    IconTextWidth, IconTextHeight, X, ALeft: integer;

  B, DefaultButton, CancelButton: TBotaoMsg;
  IconID: PChar;
  TextRect: TRect;
begin
  Result := TMessageForm.CreateNew( Application );
  with Result do
  begin
    BiDiMode := Application.BiDiMode;
    BorderStyle := bsDialog;
    Canvas.Font := PFonteMsg;
    KeyPreview := true;
    OnKeyDown := TMessageForm( Result ).CustomKeyDown;
    DialogUnits := GetAveCharSize( Canvas );
    BorderIcons := [];

    HorzMargin  := MulDiv( mcHorzMargin, DialogUnits.X, 4 );
    VertMargin  := MulDiv( mcVertMargin, DialogUnits.Y, 8 );
    HorzSpacing := MulDiv( mcHorzSpacing, DialogUnits.X, 4 );
    VertSpacing := MulDiv( mcVertSpacing, DialogUnits.Y, 8 );
    ButtonWidth := MulDiv( mcButtonWidth, DialogUnits.X, 4 );

    for B := Low( TBotaoMsg ) to High( TBotaoMsg ) do
    begin
      if B in Buttons then
      begin
        if iLarBtns[B] = 0 then
        begin
          TextRect := Rect( 0, 0, 0, 0 );
          Windows.DrawText( Canvas.handle,
                            PChar( sLgndBtns[B] ), -1,
                            TextRect, DT_CALCRECT or DT_LEFT or DT_SINGLELINE or
                            DrawTextBiDiModeFlagsReadingOnly );
          with TextRect do
            iLarBtns[B] := Right - Left + 8;
        end;
        if iLarBtns[B] > ButtonWidth then
          ButtonWidth := iLarBtns[B];
      end;
    end;

    ButtonHeight := MulDiv( mcButtonHeight, DialogUnits.Y, 8 );
    ButtonSpacing := MulDiv( mcButtonSpacing, DialogUnits.X, 4 );

    SetRect( TextRect, 0, 0, Screen.Width div 2, 0 );

    DrawText( Canvas.Handle, PChar( Msg ), Length( Msg ) + 1, TextRect,
              DT_EXPANDTABS or DT_CALCRECT or DT_WORDBREAK or
              DrawTextBiDiModeFlagsReadingOnly );

    IconID := idsIcones[DlgType];
    IconTextWidth := TextRect.Right;
    IconTextHeight := TextRect.Bottom;
    if IconID <> nil then
    begin
      Inc( IconTextWidth, 32 + HorzSpacing );
      if IconTextHeight < 32 then
        IconTextHeight := 32;
    end;
    ButtonCount := 0;
    for B := Low( TBotaoMsg ) to High( TBotaoMsg ) do
      if B in Buttons then
        Inc( ButtonCount );
    ButtonGroupWidth := 0;
    if ButtonCount <> 0 then
      ButtonGroupWidth := ButtonWidth * ButtonCount + ButtonSpacing * ( ButtonCount - 1 );
    ClientWidth := Max( IconTextWidth, ButtonGroupWidth ) + HorzMargin * 2;
    ClientHeight := IconTextHeight + ButtonHeight + VertSpacing + VertMargin * 2;
    Left := ( Screen.Width div 2 ) - ( Width div 2 );
    Top := ( Screen.Height div 2 ) - ( Height div 2 );
    if DlgType <> imPers then
      Caption := sLgnd[DlgType]
    else
      Caption := Application.Title;
    if IconID <> nil then
      with TImage.Create( Result ) do
      begin
        Name := 'Image';
        Parent := Result;
        Picture.Icon.Handle := LoadIcon( 0, IconID );
        SetBounds( HorzMargin, VertMargin, 32, 32 );
      end;
    TMessageForm( Result ).Message := TLabel.Create( Result );
    with TMessageForm( Result ).Message do
    begin
      Name := 'Message';
      Parent := Result;
      WordWrap := true;
      Caption := Msg;
      BoundsRect := TextRect;
      BiDiMode := Result.BiDiMode;
      ALeft := IconTextWidth - TextRect.Right + HorzMargin;
      if UseRightToLeftAlignment then
        ALeft := Result.ClientWidth - ALeft - Width;
      SetBounds( ALeft, VertMargin, TextRect.Right, TextRect.Bottom );
      Font := PFonteMsg;
    end;

    if mbtnOk in Buttons then
      DefaultButton := mbtnOk
    else if mbtnYes in Buttons then
      DefaultButton := mbtnYes
    else
      DefaultButton := mbtnRetry;

    if mbtnCancel in Buttons then
      CancelButton := mbtnCancel
    else if mbtnNo in Buttons then
      CancelButton := mbtnNo
    else
      CancelButton := mbtnOk;

    X := ( ClientWidth - ButtonGroupWidth ) div 2;

    for B := Low( TBotaoMsg ) to High( TBotaoMsg ) do
      if B in Buttons then
        with TButton.Create( Result ) do
        begin
          Name := sNomeBtns[B];
          Parent := Result;
          Font := PFonteBtn;
          Caption := sLgndBtns[B];
          ModalResult := MdlRslt[B];
          if B = DefaultButton then
            Default := true;
          if B = CancelButton then
            Cancel := true;
          SetBounds( X, IconTextHeight + VertMargin + VertSpacing, ButtonWidth, ButtonHeight );
          Inc( X, ButtonWidth + ButtonSpacing );
          Height := 25;
          if B = mbtnHelp then
            OnClick := TMessageForm( Result ).HelpButtonClick;
        end;
  end;
end;

{----------------------------------------------------------------------------------------------}
function DispMsgPersonalizada(const Msg: string; DlgType: TIconeMsg; Buttons: TGrupoBotaoMsg;
  const PFonteMsg, PFonteBtn: TFont ): integer;
begin
  with FH.MontaMsgPersonalizada( Msg, DlgType, Buttons, PFonteMsg, PFonteBtn ) do
    try
      Position := poScreenCenter;
      Result := ShowModal;
    finally
      Free;
    end;
end;

{----------------------------------------------------------------------------------------------}
procedure MsgPersonalizada(const Msg: string; const PFonteMsg, PFonteBtn: TFont);
begin
  FH.DispMsgPersonalizada( Msg, imPers, [mbtnOk], PFonteMsg, PFonteBtn );
end;

{----------------------------------------------------------------------------------------------}
var
  Captions: array[TMsgDlgType] of string = (SMsgDlgWarning, SMsgDlgError,
    SMsgDlgInformation, SMsgDlgConfirm, '');
  IconIDs: array[TMsgDlgType] of PChar = (IDI_EXCLAMATION, IDI_HAND,
    IDI_ASTERISK, IDI_QUESTION, nil);
  ButtonNames: array[TMsgDlgBtn] of string = (
    'Yes', 'No', 'OK', 'Cancel', 'Abort', 'Retry', 'Ignore', 'All', 'NoToAll',
    'YesToAll', 'Help');
  ButtonCaptions: array[TMsgDlgBtn] of string = (
    SMsgDlgYes, SMsgDlgNo, SMsgDlgOK, SMsgDlgCancel, SMsgDlgAbort,
    SMsgDlgRetry, SMsgDlgIgnore, SMsgDlgAll, SMsgDlgNoToAll, SMsgDlgYesToAll,
    SMsgDlgHelp);
  ModalResults: array[TMsgDlgBtn] of Integer = (
    mrYes, mrNo, mrOk, mrCancel, mrAbort, mrRetry, mrIgnore, mrAll, mrNoToAll,
    mrYesToAll, 0);
var
  ButtonWidths : array[TMsgDlgBtn] of integer;  // initialized to zero

{----------------------------------------------------------------------------------------------}
function GeraMsStandard(const Msg: string; DlgType: TMsgDlgType;
  Buttons: TMsgDlgButtons): TForm;
const
  mcHorzMargin = 8;
  mcVertMargin = 8;
  mcHorzSpacing = 10;
  mcVertSpacing = 10;
  mcButtonWidth = 50;
  mcButtonHeight = 14;
  mcButtonSpacing = 4;
var
  DialogUnits: TPoint;
  HorzMargin, VertMargin, HorzSpacing, VertSpacing, ButtonWidth,
  ButtonHeight, ButtonSpacing, ButtonCount, ButtonGroupWidth,
  IconTextWidth, IconTextHeight, X, ALeft: Integer;
  B, DefaultButton, CancelButton: TMsgDlgBtn;
  IconID: PChar;
  TextRect: TRect;
begin
  Result := TMessageForm.CreateNew(Application);
  with Result do
  begin
    BiDiMode := Application.BiDiMode;
    BorderStyle := bsDialog;
    Canvas.Font := Font;
    KeyPreview := True;
    OnKeyDown := TMessageForm(Result).CustomKeyDown;
    FormStyle := fsStayOnTop;
    DialogUnits := GetAveCharSize(Canvas);
    HorzMargin := MulDiv(mcHorzMargin, DialogUnits.X, 4);
    VertMargin := MulDiv(mcVertMargin, DialogUnits.Y, 8);
    HorzSpacing := MulDiv(mcHorzSpacing, DialogUnits.X, 4);
    VertSpacing := MulDiv(mcVertSpacing, DialogUnits.Y, 8);
    ButtonWidth := MulDiv(mcButtonWidth, DialogUnits.X, 4);
    for B := Low(TMsgDlgBtn) to High(TMsgDlgBtn) do
    begin
      if B in Buttons then
      begin
        if ButtonWidths[B] = 0 then
        begin
          TextRect := Rect(0,0,0,0);
          Windows.DrawText( canvas.handle,
            PChar(ButtonCaptions[B]), -1,
            TextRect, DT_CALCRECT or DT_LEFT or DT_SINGLELINE or
            DrawTextBiDiModeFlagsReadingOnly);
          with TextRect do ButtonWidths[B] := Right - Left + 8;
        end;
        if ButtonWidths[B] > ButtonWidth then
          ButtonWidth := ButtonWidths[B];
      end;
    end;
    ButtonHeight := MulDiv(mcButtonHeight, DialogUnits.Y, 8);
    ButtonSpacing := MulDiv(mcButtonSpacing, DialogUnits.X, 4);
    SetRect(TextRect, 0, 0, Screen.Width div 2, 0);
    DrawText(Canvas.Handle, PChar(Msg), Length(Msg)+1, TextRect,
      DT_EXPANDTABS or DT_CALCRECT or DT_WORDBREAK or
      DrawTextBiDiModeFlagsReadingOnly);
    IconID := IconIDs[DlgType];
    IconTextWidth := TextRect.Right;
    IconTextHeight := TextRect.Bottom;
    if IconID <> nil then
    begin
      Inc(IconTextWidth, 32 + HorzSpacing);
      if IconTextHeight < 32 then IconTextHeight := 32;
    end;
    ButtonCount := 0;
    for B := Low(TMsgDlgBtn) to High(TMsgDlgBtn) do
      if B in Buttons then Inc(ButtonCount);
    ButtonGroupWidth := 0;
    if ButtonCount <> 0 then
      ButtonGroupWidth := ButtonWidth * ButtonCount +
        ButtonSpacing * (ButtonCount - 1);
    ClientWidth := Max(IconTextWidth, ButtonGroupWidth) + HorzMargin * 2;
    ClientHeight := IconTextHeight + ButtonHeight + VertSpacing +
      VertMargin * 2;
    Left := (Screen.Width div 2) - (Width div 2);
    Top := (Screen.Height div 2) - (Height div 2);
    if DlgType <> mtCustom then
      Caption := Captions[DlgType] else
      Caption := Application.Title;
    if IconID <> nil then
      with TImage.Create(Result) do
      begin
        Name := 'Image';
        Parent := Result;
        Picture.Icon.Handle := LoadIcon(0, IconID);
        SetBounds(HorzMargin, VertMargin, 32, 32);
      end;
    TMessageForm(Result).Message := TLabel.Create(Result);
    with TMessageForm(Result).Message do
    begin
      Name := 'Message';
      Parent := Result;
      WordWrap := True;
      Caption := Msg;
      BoundsRect := TextRect;
      BiDiMode := Result.BiDiMode;
      ALeft := IconTextWidth - TextRect.Right + HorzMargin;
      if UseRightToLeftAlignment then
        ALeft := Result.ClientWidth - ALeft - Width;
      SetBounds(ALeft, VertMargin,
        TextRect.Right, TextRect.Bottom);
    end;
    if mbOk in Buttons then DefaultButton := mbOk else
      if mbYes in Buttons then DefaultButton := mbYes else
        DefaultButton := mbRetry;
    if mbCancel in Buttons then CancelButton := mbCancel else
      if mbNo in Buttons then CancelButton := mbNo else
        CancelButton := mbOk;
    X := (ClientWidth - ButtonGroupWidth) div 2;
    for B := Low(TMsgDlgBtn) to High(TMsgDlgBtn) do
      if B in Buttons then
        with TButton.Create(Result) do
        begin
          Name := ButtonNames[B];
          Parent := Result;
          Caption := ButtonCaptions[B];
          ModalResult := ModalResults[B];
          if B = DefaultButton then Default := True;
          if B = CancelButton then Cancel := True;
          SetBounds(X, IconTextHeight + VertMargin + VertSpacing,
            ButtonWidth, ButtonHeight);
          Inc(X, ButtonWidth + ButtonSpacing);
          if B = mbHelp then
            OnClick := TMessageForm(Result).HelpButtonClick;
        end;
  end;
end;

{----------------------------------------------------------------------------------------------}
function PrMsg_Standard(const Msg: string; DlgType: TMsgDlgType; Buttons: TMsgDlgButtons): Integer;
begin
  with GeraMsStandard(Msg, DlgType, Buttons) do
    try
      Position := poScreenCenter;
      Result   := ShowModal;
    finally
      Free;
    end;
end;

{----------------------------------------------------------------------------------------------}
procedure PrMsg_AT(const S: string);
begin
  FH.PrMsg_Standard(S, mtWarning, [mbOK]);
end;

{----------------------------------------------------------------------------------------------}
procedure PrMsg_AT(const S: string; const Args: array of const);
begin
  FH.PrMsg_Standard(Format(S, Args), mtWarning, [mbOK]);
end;

{----------------------------------------------------------------------------------------------}
procedure PrMsg_ER(const S: string);
begin
  FH.PrMsg_Standard(S, mtError, [mbOK]);
end;

{----------------------------------------------------------------------------------------------}
procedure PrMsg_ER(const S: string; const Args: array of const);
begin
  FH.PrMsg_Standard(Format(S, Args), mtError, [mbOK]);
end;

{----------------------------------------------------------------------------------------------}
procedure PrMsg_Exit(pCampo: TWinControl; const S: string);
begin
  FH.PrMsg_Standard(S, mtWarning, [mbOK]);
  Focalize(pCampo);
  Abort;
end;

{----------------------------------------------------------------------------------------------}
procedure PrMsg_Exit(pCampo: TWinControl; const S: string; const Args: array of const);
begin
  FH.PrMsg_Standard(Format(S, Args), mtWarning, [mbOK]);
  Focalize(pCampo);
  Abort;
end;

{----------------------------------------------------------------------------------------------}
procedure PrMsg_IN(const S: string);
begin
  FH.PrMsg_Standard(S, mtInformation, [mbOK]);
end;

{----------------------------------------------------------------------------------------------}
procedure PrMsg_IN(const S: string; const Args: array of const);
begin
  FH.PrMsg_Standard(Format(S, Args), mtInformation, [mbOK]);
end;

{----------------------------------------------------------------------------------------------}
function PrMsg_CF(const S: string; PFlag: TFlagMsgSN = 0): integer;
var
  T: TMsgDlgType;
begin
  case PFlag of
    0: T := mtConfirmation;
    1: T := mtError;
    2: T := mtWarning;
    3: T := mtInformation;
  else
    T := mtConfirmation;
  end;

  Result := FH.PrMsg_Standard(S, T, [mbYes, mbNo]);

  if Result <> mrYes then
    Result := 0
  else
    Result := 1;
end;

{----------------------------------------------------------------------------------------------}
function PrMsg_CF(const S: string; const Args: array of const; PFlag: TFlagMsgSN = 0): integer;
begin
  Result := FH.PrMsg_CF(Format(S, Args), PFlag);
end;

{----------------------------------------------------------------------------------------------}
function PrMsg_Focalize(pCampo: TWinControl; const PMsg: string = ''; const MsgCustomizada: Boolean = false): Boolean;
var
    ss : string;
    vr : integer;
    ii : Integer;
    F  : TForm;
    L  : TLabel;
    pp : PPropInfo;
begin
    if PMsg = '' then
       ss := FH.vrgDefErrMsg
    else
       ss := PMsg;

    if ss = FH.DefErrMsg then
       if MsgCustomizada then
          if RightStr(ss, 1) = '.' then
             ss := FH.CopyMin(ss, 1); // remove o ponto final

    if MsgCustomizada then
    begin
        if pCampo.Hint <> '' then
           ss := ss + ' ' + QuotedStr(pCampo.Hint) + '.'
        else
        if pCampo.Owner is TForm then
        begin
            F := pCampo.Owner as TForm;
            for ii := 0 to F.ComponentCount-1 do
            begin
                if F.Components[ii] is TLabel then
                begin
                    L := F.Components[ii] as TLabel;
                    if L.FocusControl <> nil then
                       if L.FocusControl.Name = pCampo.Name then
                       begin
                           ss := ss + ' ' + QuotedStr(L.Caption) + '.';
                           Break;
                       end;
                end;
            end;
        end;
    end;

    if pCampo <> nil then
    begin
         FH.Focalize(pCampo);
    end;

    FH.PrMsg_ER(ss);

    FH.SetaCursorDef;
    Result := true;
end;

{----------------------------------------------------------------------------------------------}
function PrMsg_FocalizeSe(const PCondicao: Boolean; pCampo: TWinControl; const PMsg: string = ''; const MsgCustomizada: Boolean = false): Boolean;
begin
  Result := PCondicao;
  if Result then
    FH.PrMsg_Focalize(pCampo, PMsg, MsgCustomizada);
end;

{----------------------------------------------------------------------------------------------}
procedure LimpaComps(PCampos: array of TControl);
var
  ii: integer;
begin
  for ii := low(PCampos) to high(PCampos) do
  begin
    if PCampos[ii] is TEdit then (PCampos[ii] as TEdit).Clear else
    if PCampos[ii] is TMemo then (PCampos[ii] as TMemo).Clear else
    if PCampos[ii] is TRichEdit then (PCampos[ii] as TRichEdit).Clear else
    if PCampos[ii] is TLabel then (PCampos[ii] as TLabel).Caption := '' else
    if PCampos[ii] is TMaskEdit then (PCampos[ii] as TMaskEdit).Clear else
    if PCampos[ii] is TCustomMaskEdit then (PCampos[ii] as TCustomMaskEdit).Clear else
    if PCampos[ii] is TComboBox then (PCampos[ii] as TComboBox).ItemIndex := -1 else
    if PCampos[ii] is TCheckBox then (PCampos[ii] as TCheckBox).Checked := false else
    if PCampos[ii] is TCheckListBox then FH.SetaStatusCheckListBox(false, (PCampos[ii] as TCheckListBox)) else
  end;
end;

{----------------------------------------------------------------------------------------------}
function ValidaCampo(pCampo: TControl; PTipoCampo: Integer = 0;
  const PDisparaMsg: Boolean = true; const MsgCustomizada: Boolean = false): Boolean;
var
  I   : integer;
  vMsg,
  ss  : string;
  pp  : PPropInfo;
begin
    vMsg         := vrgDefErrMsg;
    vrgDefErrMsg := DefErrMsg;

    { *** CheckListBox *** }
    if pCampo is TCheckListBox then
    begin
        Result := true;
        for I := 0 to TCheckListBox(pCampo).Items.Count - 1 do
        begin
            Result := TCheckListBox(pCampo).Checked[I];
            if Result then
              Break;
        end;
        if (not Result) and (PDisparaMsg) then
        begin
            FH.PrMsg_ER('Não há ítens selecionados.');
            FH.Focalize(TCheckListBox(pCampo));
        end;
    end
    else
    { *** ComboBox *** }
    if pCampo is TComboBox then
    begin
        Result := true;
        if TComboBox(pCampo).ItemIndex = -1 then
        begin
            if PDisparaMsg then
               FH.PrMsg_Focalize(TComboBox(pCampo), vMsg, MsgCustomizada);
            Result := false;
        end;
    end
    else
    { *** Edits, etc... *** }
    begin
        if AnsiSameText(RightStr(pCampo.ClassName, 4), 'EDIT') then
        begin
            // ------------------------------
            // texto normal
            // ------------------------------
            if PTipoCampo = 0 then
            begin
                pp := typInfo.GetPropInfo(pCampo.ClassInfo, 'Text');
                if Assigned(pp) then
                begin
                    ss := typInfo.GetStrProp(pCampo, pp);
                    if ss = '' then
                    begin
                        if PDisparaMsg then
                           FH.PrMsg_Focalize(TWinControl(pCampo), vMsg, MsgCustomizada);
                        Result := false;
                        Exit;
                    end
                    else
                    if AnsiContainsText(pCampo.ClassName, 'DATEEDIT') then
                    begin
                        if FH.DataInvalida(ss) then
                        begin
                            if PDisparaMsg then
                               FH.PrMsg_Focalize(TWinControl(pCampo), vMsg, MsgCustomizada);
                            Result := false;
                            Exit;
                        end;
                    end;
                end;
            end
            else
            // ------------------------------
            // inteiro ou numérico
            // ------------------------------
            if (PTipoCampo = 1) or (PTipoCampo = 2) then
            begin
                pp := typInfo.GetPropInfo(pCampo.ClassInfo, 'Value');
                if Assigned(pp) then
                begin
                    if AnsiSameText(pCampo.ClassName, 'TcxSpinEdit') then
                       ss := FloatToStr(typInfo.GetVariantProp(pCampo, pp))
                    else
                       ss := FloatToStr(typInfo.GetFloatProp(pCampo, pp));

                    if PTipoCampo = 1 then
                    begin
                        if FH.IntInvalido(ss) then
                        begin
                            if PDisparaMsg then
                               FH.PrMsg_Focalize(TWinControl(pCampo), vMsg, MsgCustomizada);
                            Result := false;
                            Exit;
                        end;
                    end
                    else
                    if PTipoCampo = 2 then
                    begin
                        if (not FH.FloatOk(ss)) or (StrToFloatDef(ss, 0) = 0) then
                        begin
                            if PDisparaMsg then
                               FH.PrMsg_Focalize(TWinControl(pCampo), vMsg, MsgCustomizada);
                            Result := false;
                            Exit;
                        end;
                    end;
                end
                else
                begin
                    pp := typInfo.GetPropInfo(pCampo.ClassInfo, 'Text');
                    if Assigned(pp) then
                    begin
                        ss := typInfo.GetStrProp(pCampo, pp);
                        if PTipoCampo = 1 then
                        begin
                            if FH.IntInvalido(ss) then
                            begin
                                if PDisparaMsg then
                                   FH.PrMsg_Focalize(TWinControl(pCampo), vMsg, MsgCustomizada);
                                Result := false;
                                Exit;
                            end;
                        end
                        else
                        if PTipoCampo = 2 then
                        begin
                            if not FH.FloatOk(ss) then
                            begin
                                if PDisparaMsg then
                                   FH.PrMsg_Focalize(TWinControl(pCampo), vMsg, MsgCustomizada);
                                Result := false;
                                Exit;
                            end;
                        end;
                    end;
                end;
            end
            else
            // ------------------------------
            // data
            // ------------------------------
            if PTipoCampo = 3 then
            begin
                pp := typInfo.GetPropInfo(pCampo.ClassInfo, 'Text');
                if Assigned(pp) then
                begin
                    ss := typInfo.GetStrProp(pCampo, pp);
                    if FH.DataInvalida(ss) then
                    begin
                        if PDisparaMsg then
                           FH.PrMsg_Focalize(TWinControl(pCampo), vMsg, MsgCustomizada);
                        Result := false;
                        Exit;
                    end;
                end;
            end;
        end;

        if pCampo is TCustomMemo then
        begin
            if TCustomMemo(pCampo).Lines.Text = '' then
            begin
                if PDisparaMsg then
                   FH.PrMsg_Focalize(TWinControl(pCampo), vMsg, MsgCustomizada);
                Result := false;
                Exit;
            end;
        end;
        
        Result := True;
    end;
end;

{----------------------------------------------------------------------------------------------}
function ValidaMultiCampo(PCampos: array of TControl; PTipoCampo: array of Integer;
  const PDisparaMsg: Boolean = true; const MsgCustomizada: Boolean = false): Boolean;
var
  I: integer;
  V: Integer;
begin
    Result := true;

    for I := Low(PCampos) to High(PCampos) do
    begin
        if (I < Low(PTipoCampo)) or (I > High(PTipoCampo)) then
           V := 0
        else
           V := PTipoCampo[I];

        Result := FH.ValidaCampo(PCampos[I], V, PDisparaMsg, MsgCustomizada);

        if not Result then
           exit;
    end;
end;

{----------------------------------------------------------------------------------------------}
function BoolParaStr(const pValor: Boolean): string;
begin
  if pValor then
    Result := FH.vrgTrueStr
  else
    Result := FH.vrgFalseStr;
end;

{----------------------------------------------------------------------------------------------}
function StrParaBool(const pValor: string; const PDefault: Boolean = false): Boolean;
begin
  if pValor = FH.vrgTrueStr then
    Result := true
  else
  if pValor = FH.vrgFalseStr then
    Result := false
  else
    Result := PDefault;
end;

{----------------------------------------------------------------------------------------------}
function LeIniControls(const PSecao: string; PContainer: TWinControl): string;
var
  ii: integer;
  ls: TStrings;
begin
  Result := '';
  try
    FH.CriaLista(ls);
    for ii := 0 to PContainer.ControlCount - 1 do
    begin
      if PContainer.Controls[ii] is TEdit then
        with PContainer.Controls[ii] as TEdit do FH.IniGravaStr(ls, PSecao, Name, Text) else
      if PContainer.Controls[ii] is TComboBox then
        with PContainer.Controls[ii] as TComboBox do FH.IniGravaInt(ls, PSecao, Name, ItemIndex) else
      if PContainer.Controls[ii] is TSpinEdit then
        with PContainer.Controls[ii] as TSpinEdit do FH.IniGravaInt(ls, PSecao, Name, Value) else
    end;
    Result := ls.Text;
  finally
    FH.DestroiObj(ls);
  end;
end;

{----------------------------------------------------------------------------------------------}
procedure GravaIniControls(const PConteudo, PSecao: string; PContainer: TWinControl);
var
  ii: integer;
  ls: TStrings;
begin
  try
    FH.IniGera(ls, PConteudo);
    for ii := 0 to PContainer.ControlCount - 1 do
    begin
      if PContainer.Controls[ii] is TEdit then
        with PContainer.Controls[ii] as TEdit do Text := FH.IniLeStr(ls, PSecao, Name) else
      if PContainer.Controls[ii] is TComboBox then
        with PContainer.Controls[ii] as TComboBox do ItemIndex := FH.IniLeInt(ls, PSecao, Name, 0) else
      if PContainer.Controls[ii] is TSpinEdit then
        with PContainer.Controls[ii] as TSpinEdit do Value := FH.IniLeInt(ls, PSecao, Name, 0) else
    end;
  finally
    FH.DestroiObj(ls);
  end;
end;

{----------------------------------------------------------------------------------------------}
function RetCodigoCombo(CB: TComboBox; const PCodigos: array of string): string;
var
  ii, jj: integer;
begin
  Result := '';
  jj := 0;
  for ii := Low(PCodigos) to High(PCodigos) do
  begin
    if jj = CB.ItemIndex then
    begin
      Result := PCodigos[ii];
      exit;
    end;
    inc(jj);
  end;
end;

{----------------------------------------------------------------------------------------------}
function SeTemInteiro(const PStr: string; const SenaoRetorne: string = ''): string;
begin
  Result := PStr;
  if FH.StrInteiro(PStr) = '' then
    Result := SenaoRetorne;
end;

{----------------------------------------------------------------------------------------------}
function TemInteiro(const PStr: string): Boolean;
begin
  Result := FH.StrInteiro(PStr) <> '';
end;

{----------------------------------------------------------------------------------------------}
function NaoTemInteiro(const PStr: string): Boolean;
begin
  Result := not FH.TemInteiro(PStr);
end;

{----------------------------------------------------------------------------------------------}
function VerSQLLike(const PStr: string; const NaEsquerda, NaDireita: Boolean): string;
begin
  Result := PStr;
  if AnsiContainsText(PStr, '%') then
    exit;
  Result := AnsiDequotedStr(AnsiDequotedStr(Result, '"'), '''');
  if NaDireita then Result := Result + '%';
  if NaEsquerda then Result := '%' + Result;
  Result := QuotedStr(Result);
end;

{----------------------------------------------------------------------------------------------}
function WhereAnd(const PSql: string; const VerificarApos: string = ''): string;
begin
    if (VerificarApos <> '') and AnsiContainsText(PSql, VerificarApos) then
    begin
        if AnsiContainsText(FH.CopyPosMax(PSql, VerificarApos), 'WHERE') then
           Result := ' AND '
        else
           Result := 'WHERE ';
    end
    else
    if AnsiContainsText(PSql, '/*INI_CRITERIOS*/') then
    begin
        if AnsiContainsText(FH.CopyPosMax(PSql, '/*INI_CRITERIOS*/'), 'WHERE') then
           Result := ' AND '
        else
           Result := 'WHERE ';
    end
    else
    begin
        if AnsiContainsText(PSql, 'WHERE') then
           Result := ' AND '
        else
           Result := 'WHERE ';
    end;
end;

{----------------------------------------------------------------------------------------------}
function AdicionaBreak(const pTexto: string): string;
begin
  if Length(pTexto) > 0 then
    Result := #13
  else
    Result := '';
end;

{----------------------------------------------------------------------------------------------}
procedure AdicionaLinha(var S: string; const Linha: string; const Quebras: Integer = 1);
begin
     if S > '' then
        S := S + FH.RetBreak(Quebras);
     S := S + Linha;
end;

{----------------------------------------------------------------------------------------------}
function InverteMesAno(const PMesAno: string; const SemBarra: Boolean = False): string;
begin
  Result := FH.CopyPosMax(PMesAno, DateSeparator) +
            FH.Iff(SemBarra, '', DateSeparator) +
            FH.CopyPos(PMesAno, DateSeparator);
end;

{----------------------------------------------------------------------------------------------}
function InverteAnoMes(const PAnoMes: string): string;
begin
  Result := FH.CopyPosMax(PAnoMes, DateSeparator) + DateSeparator +
            FH.CopyPos(PAnoMes, DateSeparator);
end;

{----------------------------------------------------------------------------------------------}
function IncMesAno(const PMesAno: string; const Incremento: Integer): string;
begin
    Result := PMesAno;
    if FH.MesAnoValido(Result) then
    begin
        Result := FH.GetMesAno(DateToStr(IncMonth(RecodeDay(StrToDate('01/' + Result), 1), Incremento)));
    end;
end;

{----------------------------------------------------------------------------------------------}
function MesAnoValido(const pValor: string): Boolean;
begin
  Result := false;

  if FH.StrInvalida(pValor)
  or FH.NaoTemInteiro(pValor)
  or (not AnsiContainsText(pValor, DateSeparator))
  or (Length(pValor) <> 7)
  or (not AnsiMatchText(LeftStr(pValor, 2), FH.cngStrMeses))
  or FH.IntInvalido(copy(pValor, 1, 1))
  or FH.IntInvalido(copy(pValor, 2, 1))
  or FH.IntInvalido(copy(pValor, 4, 1))
  or FH.IntInvalido(copy(pValor, 5, 1))
  or FH.IntInvalido(copy(pValor, 6, 1))
  or FH.IntInvalido(copy(pValor, 7, 1)) then
    exit;

  Result := true;
end;

{----------------------------------------------------------------------------------------------}
function AnoMesValido(const pValor: string): Boolean;
begin
  Result := false;

  if FH.StrInvalida(pValor)
  or FH.NaoTemInteiro(pValor)
  or (not AnsiContainsText(pValor, DateSeparator))
  or (Length(pValor) <> 7)
  or (not AnsiMatchText(Copy(pValor, 6, 2), FH.cngStrMeses))
  or FH.IntInvalido(copy(pValor, 1, 1))
  or FH.IntInvalido(copy(pValor, 2, 1))
  or FH.IntInvalido(copy(pValor, 3, 1))
  or FH.IntInvalido(copy(pValor, 4, 1))
  or FH.IntInvalido(copy(pValor, 6, 1))
  or FH.IntInvalido(copy(pValor, 7, 1)) then
    exit;

  Result := true;
end;

{----------------------------------------------------------------------------------------------}
function TrataMesAno(PMaskEdit: TMaskEdit): Boolean;
var
  temp, vAno: string;
begin
  Result := true;
  temp := PMaskEdit.Text;
  if FH.NaoTemInteiro(temp) then
    exit;
  if not AnsiMatchText(FH.CopyPos(temp, '/'),
                      ['01', '02', '03', '04', '05', '06',
                       '07', '08', '09', '10', '11', '12']) then
  begin
    Result := false;
    FH.MsgFocalize(PMaskEdit, 'Referência inválida.');
    exit;
  end;
  vAno := FH.CopyMax(temp, 4);
  if Pos(#32, vAno) = 0 then
    exit;
  Result := false;
  vAno := FH.LimpaEspc(vAno);
  if FH.MsgFocalizeSe(FH.StrInvalida(vAno), PMaskEdit, 'Referência inválida.') then
    exit;
  if StrToInt(vAno) < 50 then
    vAno := '2'  + FH.StrZero(vAno, 3)
  else
    vAno := '19' + FH.StrZero(vAno, 2);
  temp := FH.CopyPos(temp, '/') + '/' + vAno;
  PMaskEdit.Text := temp;
  Result := true;
end;

{----------------------------------------------------------------------------------------------}
procedure ExportaDOC(DS: TDataSet; const pNomeArq: string);
var
  A, D, T, R: Variant;
  rr, cc: integer;
begin
  A := CreateOleObject('Word.Basic');
  A.AppShow;
  D := CreateOleObject('Word.Document');
  R := D.Range;
  T := D.Tables.Add(D.Range, 1, DS.FieldCount);
  for cc := 0 to DS.FieldCount - 1 do
    T.Cell(1, cc + 1).Range.Text := DS.Fields[cc].FieldName;
  rr := 2;
  DS.First;
  while not DS.Eof do
  begin
    T.Rows.Add;
    for cc := 0 to DS.FieldCount - 1 do
      T.Cell(rr, cc + 1).Range.Text := DS.Fields[cc].DisplayText;
    DS.Next;
    inc(rr);
  end;
  D.SaveAs(pNomeArq);
  D := UnAssigned;
end;

{----------------------------------------------------------------------------------------------}
procedure ControlaTeclaMesAno(var Key: Char; PMaskEdit: TMaskEdit);
begin
  if (Key <> '+') and (Key <> '-') and (Key <> 't') and (Key <> 'T') then
    exit;
  with PMaskEdit do
  begin
    if (FH.NaoTemInteiro(text)) or (not FH.MesAnoValido(text)) then
      text := FH.GetMesAno(Now);
    case Key of
      '+': Text := FH.GetMesAno(IncMonth(FH.MontaDataMesAno(Text)));
      '-': Text := FH.GetMesAno(IncMonth(FH.MontaDataMesAno(Text), -1));
      't', 'T':
           Text := FH.GetMesAno(Now);
    end;
    Key := #0;
  end;
end;

{----------------------------------------------------------------------------------------------}
function MontaHoraExtensa(const T: string; const PAbrev: Boolean = false): string;
const
  cFmtHr: array[Boolean] of string = (' hora', ' hor');
  cFmtMn: array[Boolean] of string = (' minuto', ' min');
  cFmtSg: array[Boolean] of string = (' segundo', ' seg');
  cFmtMs: array[Boolean] of string = (' milésimo', ' mseg');
var
  h, m, s, z: word;
  tx: string;
begin
  Result := '';
  tx     := T;
  if FH.ContaStr(tx, ':') < 2 then exit;
  h := StrToInt(FH.CopyPos(tx, ':'));
  tx := FH.CopyPosMax(tx, ':');
  m := StrToInt(FH.CopyPos(tx, ':'));
  tx := FH.CopyPosMax(tx, ':');
  if Pos(':', tx) = 0 then
  begin
    s := StrToInt(tx);
    z := 0;
  end
  else
  begin
    s := StrToInt(FH.CopyPos(tx, ':'));
    tx := FH.CopyPosMax(tx, ':');
    z := StrToInt(tx);
  end;
  if h > 0 then Result := Result + IntToStr(h) + cFmtHr[PAbrev] + ifthen((h > 1) and (not PAbrev), 's') + #32;
  if m > 0 then Result := Result + IntToStr(m) + cFmtMn[PAbrev] + ifthen((m > 1) and (not PAbrev), 's') + #32;
  if s > 0 then Result := Result + IntToStr(s) + cFmtSg[PAbrev] + ifthen((s > 1) and (not PAbrev), 's') + #32;
  if z > 0 then Result := Result + IntToStr(z) + cFmtMs[PAbrev] + ifthen((z > 1) and (not PAbrev), 's') + #32;
  Result := trim(Result);
end;

{----------------------------------------------------------------------------------------------}
function SeStrVazia(const PStr, PDefault: string): string;
begin
  Result := PStr;
  if FH.StrInvalida(Result) then Result := PDefault;
end;

{----------------------------------------------------------------------------------------------}
function CalcPorcentagem(const X, Y: double): double;
begin
  Result := (Y * X) / 100;
end;

{----------------------------------------------------------------------------------------------}
function CalcPorcentagemTotal(const X, Y: double): double;
begin
  Result := 100 * (X / Y);
end;

{----------------------------------------------------------------------------------------------}
function CalcPorcentagemDelta(const X, Y: double): double;
begin
  Result := 100 * ((X - Y) / Y);
end;

{----------------------------------------------------------------------------------------------}
function MaiorQue(const PValor1, PValor2: Extended; const PDelta: Extended = 0): Extended;
begin
  if PValor1 > PDelta then
    Result := PValor1
  else
    Result := PValor2;
end;

{----------------------------------------------------------------------------------------------}
function ComprimeData(const PData: string): string;
begin
  Result := PData;
  if FH.DataOk(PData) then
    Result := FormatDateTime('dd/mm/yy', StrToDate(PData));
end;

{----------------------------------------------------------------------------------------------}
function ComprimeDataHora(const PDataHora: string): string;
begin
  Result := PDataHora;
  if FH.DataHoraOk(PDataHora) then
    Result := FormatDateTime('dd/mm/yy hh:mm', StrToDateTime(PDataHora));
end;

{----------------------------------------------------------------------------------------------}
function MontaMesAnoExtenso(const DataOuMesAno: string; const AnoComDoisDigitos: Boolean = false): string;
begin
  Result := DataOuMesAno;
  if FH.MesAnoValido(Result) then
    Result := '01/' + Result
  else
  if not FH.DataOk(Result) then
    exit;
  Result := FormatDateTime('mmmm/' + IfThen(AnoComDoisDigitos, 'yy', 'yyyy'),
                           StrToDate(Result));
  Result := FH.SetaUpper(Result, 1);
end;

{----------------------------------------------------------------------------------------------}
function ListaParaStr(ALista: TStrings): string;
var
  ii: integer;
begin
  Result := '';
  for ii := 0 to ALista.Count - 1 do
    Result := Result + ifthen(Result > '', ', ') + ALista[ii];
end;

{----------------------------------------------------------------------------------------------}
procedure InsereNaLista(Lista: TStrings; const Linhas: string);
var
  i: integer;
  m: TStrings;
begin
    try
      FH.CriaLista(m);
      m.Text := Linhas;
      for i := 0 to m.Count-1 do
         Lista.Add(m[i]);
    finally
      FH.DestroiObj(m);
    end;
end;

{----------------------------------------------------------------------------------------------}
procedure FormaLista(var V: string; const S: string; const Espacado: Boolean = True);
begin
     V := V + IfThen(V > '', IfThen(Espacado, ', ', ',')) + S;
end;

{----------------------------------------------------------------------------------------------}
function FmtMilesimos(const Z: int64; const MostraMilesimos: Boolean = true): string;
var
  H, M, S, L: integer;
  T: double;
begin
  T := Z / 1000;
  H := Round(Int(T / 3600));
  M := Round(Int((T - H * 3600) / 60));
  if MostraMilesimos then
  begin
    S := Round(int(T - H * 3600 - M * 60));
    L := Z - ((S * 1000) + (M * 60000) + (H * 3600000));
    Result := Format('%s:%s:%s:%s', [FH.StrZero(H, 2), FH.StrZero(M, 2),
                                     FH.StrZero(S, 2), FH.StrZero(L, 4)]);
  end
  else
  begin
    S := Round(T - H * 3600 - M * 60);
    Result := Format('%s:%s:%s', [FH.StrZero(H, 2), FH.StrZero(M, 2), FH.StrZero(S, 2)]);
  end;
end;

{----------------------------------------------------------------------------------------------}
function MontaHoraDelta(const BaseZ: int64): string;
begin
  Result := FH.FmtMilesimos(GetTickCount - BaseZ, false);
end;
//Button1.Caption := SecToTime(Round(1000 / MSecsPerSec));
{
function SecToTime(Sec: Integer): string;
var
   H, M, S: string;
   ZH, ZM, ZS: Integer;
begin
   ZH := Sec div 3600;
   ZM := Sec div 60 - ZH * 60;
   ZS := Sec - (ZH * 3600 + ZM * 60) ;
   H := IntToStr(ZH) ;
   M := IntToStr(ZM) ;
   S := IntToStr(ZS) ;
   Result := H + ':' + M + ':' + S;
end;
}

{----------------------------------------------------------------------------------------------}
function Locate(DS: TDataSet; const PCampos: string;
  const PValores: array of string; const PInsensitive: Boolean;
  const PDisableControls: Boolean = false): Boolean;
var
  ii: integer;
  ls: TStrings;
  rp: Boolean;
  pt: pointer;
begin
  Result := false;
  if DS.IsEmpty then
    exit;
  try
    FH.GetBmark(pt, DS);
    FH.CriaLista(ls);
    ls := FH.StrParaLista(StringReplace(PCampos, ';', ',', [rfReplaceAll]));
    DS.First;
    if PDisableControls then DS.DisableControls;
    while not DS.Eof do
    begin
      for ii := 0 to ls.Count - 1 do
      begin
        if ii <= high(PValores) then
        begin
          if PInsensitive then
            rp := AnsiSameText(PValores[ii], DS.FieldByName(ls[ii]).AsString)
          else
            rp := AnsiSameStr(PValores[ii], DS.FieldByName(ls[ii]).AsString);
        end
        else
          rp := false;
        if not rp then break;
      end;
      Result := rp;
      if Result then
        exit;
      DS.Next;
    end;
    FH.GotoBmark(pt, DS);
  finally
    if pt <> nil then DS.FreeBookmark(pt);
    if PDisableControls then DS.EnableControls;
    FH.DestroiObj(ls);
  end;
end;

{----------------------------------------------------------------------------------------------}
function IfThenColor(const PCondic: Boolean; const PTrue, PFalse: TColor): TColor;
begin
  if PCondic then
    Result := PTrue
  else
    Result := PFalse;
end;

{----------------------------------------------------------------------------------------------}
function CalculaIdade(const PNascimento, PDataAtual: string): integer;
var
  D: TDateTime;
begin
  Result := 0;
  if FH.DataInvalida(PNascimento) then
    exit;
  D := Now;
  if FH.DataOk(PDataAtual) then
    D := StrToDate(PDataAtual);

  Result := round(int(YearSpan(D, StrToDate(PNascimento))));
end;

{----------------------------------------------------------------------------------------------}
function ComparaTextoRapido(const S1, S2: string): Boolean;
begin
  Result := AnsiSameText(FH.LimpaChrEspecial(S1), FH.LimpaChrEspecial(S2));
end;

{----------------------------------------------------------------------------------------------}
procedure SetaGridMskData(GR: TDBGrid);
var
  ii: integer;
  DS: TDataSet;
  F: TField;
begin
  if GR.DataSource = nil then
    exit;
  DS := GR.DataSource.DataSet;
  if DS = nil then
    exit;
  for ii := 0 to pred(GR.Columns.Count) do
  begin
    F := DS.Fields.FindField(GR.Columns[ii].FieldName);
    if F = nil then
      continue;
    if not (F is TDateField) then
      continue;
    with (F as TDateField) do
      if DisplayFormat <> '' then
        EditMask := DisplayFormat
      else
        EditMask := '99/99/9999';
  end;
end;

{----------------------------------------------------------------------------------------------}
procedure SetaGridDispFormat(GR: TDBGrid; const PDecs: integer = 2);
var
  ii: integer;
  DS: TDataSet;
  F: TField;
begin
  if GR.DataSource = nil then
    exit;
  DS := GR.DataSource.DataSet;
  if DS = nil then
    exit;
  for ii := 0 to pred(GR.Columns.Count) do
  begin
    F := DS.Fields.FindField(GR.Columns[ii].FieldName);
    if F = nil then
      continue;
    if  (not (F is TFloatField))
    and (not (F is TBCDField)) then
      continue;
    (F as TNumericField).DisplayFormat := ',0' +
                                          ifthen(PDecs > 0,
                                                 '.' + StringOfChar('0', PDecs));
  end;
end;

{----------------------------------------------------------------------------------------------}
function RetValIndex(const Index: integer; const Valores: array of string;
  const Quoted: Boolean = false): string;
begin
  Result := '';
  if (Index > high(Valores)) or (Index < low(Valores)) then
    exit;
  Result := Valores[Index];
  if Quoted then Result := QuotedStr(Result);
end;

{----------------------------------------------------------------------------------------------}
function CaseOf(const Valor: string; const IDs, Results: array of string): string;
var
  ii: integer;
begin
  Result := Valor;
  ii     := AnsiIndexText(Valor, IDs);
  if ii < 0 then
    exit;
  if (ii > high(Results)) or (ii < low(Results)) then
    exit;
  Result := Results[ii];
end;

{----------------------------------------------------------------------------------------------}
procedure GravaArqTab(const PArq: string; DS: TDataSet; const pCampo: string);
var
    MS : TMemoryStream;
begin
    try
        MS := TMemoryStream.Create;
        MS.LoadFromFile(PArq);
        (DS.FieldByName(pCampo) as TBlobField).LoadFromStream(MS);
    finally
        MS.Destroy;
    end;
end;

{----------------------------------------------------------------------------------------------}
procedure GeraArqTab(DS: TDataSet; const pCampo, PDestino: string; const AbreArq: Boolean);
var
    ST : TStream;
    MS : TMemoryStream;
begin
    try
        MS := TMemoryStream.Create;
        ST := DS.CreateBlobStream(DS.FieldByName(pCampo), bmRead);
        MS.LoadFromStream(ST);
        MS.SaveToFile(PDestino);
        if AbreArq then
           FH.ExecutaArq(PDestino);
    finally
        MS.Destroy;
        ST.Free;
    end;
end;

{----------------------------------------------------------------------------------------------}
procedure GravaImagemTab(PC: TPicture; DS: TDataSet; const pCampo: string);
var
    MS : TMemoryStream;
begin
    try
         MS := TMemoryStream.Create;
         if PC.Width > 0 then
            PC.Graphic.SaveToStream(MS);
         (DS.FieldByName(pCampo) as TBlobField).LoadFromStream(MS);
    finally
        MS.Destroy;
    end;
end;

{----------------------------------------------------------------------------------------------}
procedure LeImagemTab(PC: TPicture; DS: TDataSet; const pCampo, PExtensao: string);
var
    ST  : TStream;
    JPG : TJPEGImage;
    BTM : TBitmap;
begin
    if not DS.Active then
       Exit;
    if DS.IsEmpty then
       Exit;
    if DS.FindField(pCampo) = nil then
       raise Exception.CreateFmt('O campo %s não localizado no dataset %s.', [pCampo, DS.Name]);
    try
        JPG := nil;
        BTM := nil;
        ST  := DS.CreateBlobStream(DS.FieldByName(pCampo), bmRead);
        if ST.Size > 0 then
        begin
            if AnsiContainsText(PExtensao, 'JPG') then
            begin
                try
                    JPG := TJPEGImage.Create;
                    JPG.LoadFromStream(ST);
                    PC.Assign(JPG)
                except
                    //
                end;
            end
            else
            begin
                try
                    BTM := TBitmap.Create;
                    BTM.LoadFromStream(ST);
                    PC.Bitmap.Assign(BTM);
                except
                    //
                end;
            end;
        end
        else
            PC.Assign(nil);
    finally
        JPG.Free;
        BTM.Free;
        ST.Free;
    end;
end;

{----------------------------------------------------------------------------------------------}
procedure AbreCDS(CDS: TClientDataSet; const PLogChanges: Boolean = False);
begin
  CDS.Close;
  CDS.CreateDataSet;
  CDS.LogChanges := PLogChanges;
end;

{----------------------------------------------------------------------------------------------}
function VerOwner(FR: TForm; const POwner: string): Boolean;
begin
  Result := False;
  if FH.RetForm(POwner) <> nil then
    if FR.Owner is TForm then
      if SameText((FR.Owner as TForm).Name, FH.RetForm(POwner).Name) then
        Result := True;
end;

{----------------------------------------------------------------------------------------------}
function VerCampoNum(FL: TField): Boolean;
begin
  Result := (FL.DataType = ftBCD) or (FL.DataType = ftFloat);
end;

function VerCampoNum(FT: TFieldType): Boolean;
begin
  Result := (FT = ftBCD) or (FT = ftFloat);
end;

{----------------------------------------------------------------------------------------------}
function ProperCase(sBuffer: string): string;
var
  iLen, iIndex: integer;
begin
  Result  := '';
  iLen    := Length(sBuffer);
  sBuffer := AnsiUpperCase(MidStr(sBuffer, 1, 1)) + AnsiLowerCase(MidStr(sBuffer, 2, iLen));

  for iIndex := 0 to iLen do
  begin
    if (MidStr(sBuffer, iIndex, 1) = #32)
    or (MidStr(sBuffer, iIndex, 1) = '-')
    or (MidStr(sBuffer, iIndex, 1) = '/') then
      sBuffer := MidStr(sBuffer, 0, iIndex) +
                 AnsiUpperCase(MidStr(sBuffer, iIndex + 1, 1)) +
                 AnsiLowerCase(MidStr(sBuffer, iIndex + 2, iLen));
  end;

  Result := sBuffer;
end;

{----------------------------------------------------------------------------------------------}
function SentenceCase(str: string): string;
begin
  Result := str;
end;

{----------------------------------------------------------------------------------------------}
function IncEllipsis(PStr: string; const PTam: Integer): string;
begin
  Result := PStr;
  if (Length(Result) <= PTam) or (Length(Result) = 0) then
    Exit;
  Result := Copy(Result, 1, PTam) + '...';
end;

{----------------------------------------------------------------------------------------------}
function StrToWord(const Value: String): Word;
begin
  Result := Word(pointer(@Value[1])^);
end;

{----------------------------------------------------------------------------------------------}
function WordToStr(const Value: Word): string;
begin
  SetLength(Result, SizeOf(Value));
  Move(Value, Result[1], SizeOf(Value));
end;

{----------------------------------------------------------------------------------------------}
function StrParaVar(const PStr: string): Variant;
begin
  Result := null;
  if PStr > '' then
    Result := PStr;
end;

{----------------------------------------------------------------------------------------------}
function ContaPalavra(const PStr: string): Integer;
var
   PChr  : char;
   IsWord : Boolean;
   i      : integer;
   sTemp  : string;
begin
  Result := 0;
  IsWord := False;
  sTemp  := FH.LimpaChrEspecial(PStr);
  if Length(sTemp) < 1 then
    Exit;
  for i := 0 to Length(sTemp) do
  begin
    PChr := sTemp[i];
    if PChr in ['a'..'z', 'A'..'Z', '0'..'9', '''', '-'] then
    begin
      if not IsWord then
        Inc(Result);
      IsWord := True;
    end
    else
    if PChr = '\' then
      IsWord := True
    else
      IsWord := False
  end;
end;

{----------------------------------------------------------------------------------------------}
function SetaNomeArqValido(const pNomeArq: string): string;
begin
  Result := pNomeArq;
  Result := StringReplace(Result, '\', '_', [rfReplaceAll]);
  Result := StringReplace(Result, '/', '_', [rfReplaceAll]);
  Result := StringReplace(Result, ':', '_', [rfReplaceAll]);
  Result := StringReplace(Result, '*', '_', [rfReplaceAll]);
  Result := StringReplace(Result, '?', '_', [rfReplaceAll]);
  Result := StringReplace(Result, '"', '_', [rfReplaceAll]);
  Result := StringReplace(Result, '>', '_', [rfReplaceAll]);
  Result := StringReplace(Result, '<', '_', [rfReplaceAll]);
  Result := StringReplace(Result, '|', '_', [rfReplaceAll]);
end;

{----------------------------------------------------------------------------------------------}
procedure GravaLista(var L: TStrings; const Arquivo: string);
var
  Stream: TStream;
  auxStr: string;
begin
  Stream := TFileStream.Create(Arquivo, fmCreate);
  auxStr := L.Text;

  if Copy(auxStr, Length(auxStr) - 1, 4) = #$D#$A then
    auxStr := Copy(auxStr, 1, Length(auxStr) - 2);

  try
    Stream.WriteBuffer(Pointer(auxStr)^, Length(auxStr));
  finally
    Stream.Free;
  end;
end;

{----------------------------------------------------------------------------------------------}
function CapturaTela: TBitmap;
var
  dc : HDC;
  cv : TCanvas;
begin
     Result := TBitmap.Create;
     Result.Width  := Screen.Width;
     Result.Height := Screen.Height;
     dc        := GetDC(0);
     cv        := TCanvas.Create;
     cv.Handle := dc;
     Result.Canvas.CopyRect(Rect(0, 0, Screen.Width, Screen.Height), cv,
                            Rect(0, 0, Screen.Width, Screen.Height));
     cv.Free;
     ReleaseDC(0, DC);
end;

{----------------------------------------------------------------------------------------------}
function VarParaFloat(const V: Variant): Extended;
begin
     Result := 0;
     if not VarIsNull(V) then
        if VarIsNumeric(V) then
           Result := FindVarData(V)^.VDouble;
end;

{----------------------------------------------------------------------------------------------}
procedure TremeJanela(wHandle: THandle);
const MAXDELTA   = 2;
      SHAKETIMES = 300;
var   oRect, wRect : TRect;
      deltax       : integer;
      deltay       : integer;
      cnt          : integer;
      dx, dy       : integer;
begin
     GetWindowRect(wHandle,wRect);
     oRect := wRect;
     Randomize;
     for cnt := 0 to SHAKETIMES do
     begin
          deltax := Round(Random(MAXDELTA));
          deltay := Round(Random(MAXDELTA));
          dx     := Round(1 + Random(2));
          if dx = 2 then
             dx := -1;
          dy := Round(1 + Random(2));
          if dy = 2 then
             dy := -1;
          OffsetRect(wRect, dx * deltax, dy * deltay);
          MoveWindow(wHandle, wRect.Left, wRect.Top, wRect.Right - wRect.Left, wRect.Bottom - wRect.Top, true);
     end;
     MoveWindow(wHandle, oRect.Left, oRect.Top, oRect.Right - oRect.Left, oRect.Bottom - oRect.Top, true);
end;

{----------------------------------------------------------------------------------------------}
function RetNomeComputador: string;
var
  buf   : array[0..255] of char;
  nSize : Cardinal;
begin
  nSize := sizeof(buf);
  if Windows.GetComputerName(buf, nSize) then
    Result := buf
  else
    Result := buf;
end;

{----------------------------------------------------------------------------------------------}
procedure PreparaPChar(var P: PChar; const Org: string; const TamMax, TamAloca: Cardinal);
begin
  P := StrAlloc(TamAloca);
  StrPLCopy(P, Org, TamMax);
  P[TamMax] := #0;
end;

{----------------------------------------------------------------------------------------------}
function TotalDiasUteis(PData: string; const ConsideraSabados: Boolean): Integer;
var
  dtAux : TDate;
  dtUlt : TDate;
  cc    : Integer;
begin
  Result := 0;
  dtAux  := StrToDate(PData);
  dtUlt  := FH.UltimoDiaMes(dtAux);
  while dtAux <= dtUlt do
  begin
    cc := DayOfTheWeek(dtAux);
    if (cc < 6) or ((cc = 6) and ConsideraSabados) then
      Inc(Result);
    dtAux := IncDay(dtAux, 1);
  end;
end;

{----------------------------------------------------------------------------------------------}
function FullTimeToStr(SUMTime: TDateTime): string;
var
  StrHor,
  StrMin :string;
  TotHor :double;
begin
  TotHor := SUMTime *24;
  StrHor := FormatFloat('##0:', Int(TotHor));
  StrMin := FormatDateTime('nn:ss', Frac(TotHor)/24);
  Result := StrHor +StrMin;
end;

{----------------------------------------------------------------------------------------------}
procedure DesmontaCMC7(const CMC7: string; out Banco, Agencia, Conta, Cheque: string);
var
    ss, tmpStr : string;
begin
    Banco   := '';
    Agencia := '';
    Conta   := '';
    Cheque  := '';

    ss      := CMC7;
    if Length(ss) < 34 then
    begin
       FH.PrMsg_ER('CMC7 incompleto.');
       Exit;
    end;
    if Pos('?', ss) > 0 then
    begin
       FH.PrMsg_ER('Não foi possível reconhecer todos caractéres.');
       Exit;
    end;

    tmpStr  := FH.StrInteiro(Copy(ss, 1, 4));
    Banco   := tmpStr;

    ss      := Copy(ss, 5, 60);
    tmpStr  := FH.StrInteiro(copy(ss, 1, 4));
    Agencia := tmpStr;

    ss     := Copy(ss, 7, 60);
    tmpStr := FH.StrInteiro(copy(ss, 3, 7));
    if FH.Int64Ok(tmpStr) then
       Cheque := IntToStr(StrToInt64Def(tmpStr, 0))
    else
       Cheque := '';

    ss     := Copy(ss, 12, 60);
    tmpStr := FH.StrInteiro(copy(ss, 2, 10));
    if FH.Int64Ok(tmpStr) then
    begin
       Conta := IntToStr(StrToInt64Def(tmpStr, 0));
       if FH.IntOk(Banco) then
       begin
           case StrToInt(Banco) of
                // Unibanco
                409 : Conta := Copy(Conta, 4, 15);
           end;
       end;
    end
    else
       Conta := '';
end;

{----------------------------------------------------------------------------------------------}
procedure QuickSort(var A: array of Integer);

 procedure QuickSort(var A: array of Integer; iLo, iHi: Integer);
 var
   Lo, Hi, Mid, T: Integer;
 begin
   Lo := iLo;
   Hi := iHi;
   Mid := A[(Lo + Hi) div 2];
   repeat
     while A[Lo] < Mid do Inc(Lo);
     while A[Hi] > Mid do Dec(Hi);
     if Lo <= Hi then
     begin
       T := A[Lo];
       A[Lo] := A[Hi];
       A[Hi] := T;
       Inc(Lo);
       Dec(Hi);
     end;
   until Lo > Hi;
   if Hi > iLo then QuickSort(A, iLo, Hi);
   if Lo < iHi then QuickSort(A, Lo, iHi);
 end;

begin
   QuickSort(A, Low(A), High(A));
end;

{----------------------------------------------------------------------------------------------}
function GetTickCountStr: string;
begin
    Result := IntToStr(GetTickCount);
end;

{----------------------------------------------------------------------------------------------}
function MixString(const TamMax: Integer; const Full: Boolean): string;
const
   m = 'ABCDEFGHIJKLMNOPQRSTUVWXYZabcdefghijklmnopqrstuvwxyz0123456789';
var
   i, k : Integer;
begin
     Result := '';
     if Full then
        k := TamMax
     else
        k := RandomRange(1, TamMax);
     while Length(Result) < k do
     begin
          i := RandomRange(1, 62);
          Result := Result + m[i];
     end;
end;

{----------------------------------------------------------------------------------------------}
function EmailOK(const PEmail: string): Boolean;
begin
     Result := False;
     if PEmail = '' then
     begin
          Result := True;
          Exit;
     end;
     if Pos('@',PEmail) = 0 then
        Exit;
     if FH.CopyPos(PEmail, '@') = '' then
        Exit;
     if FH.CopyPosMax(PEmail, '@') = '' then
        Exit;
     Result := True;
end;

{----------------------------------------------------------------------------------------------}
function Iff(const aValor: Boolean; const aTrue: string; const aFalse: string = ''): string;
begin
     if aValor then
        Result := aTrue
     else
        Result := aFalse;
end;

{----------------------------------------------------------------------------------------------}
function Iff(const aValor: Boolean; const aTrue: Int64; const aFalse: Int64 = 0): Int64;
begin
     if aValor then
        Result := aTrue
     else
        Result := aFalse;
end;

{----------------------------------------------------------------------------------------------}
function StrDiferentes(const aStr1, aStr2: string): Boolean;
begin
     Result := AnsiCompareText(aStr1, aStr2) <> 0;
end;

{----------------------------------------------------------------------------------------------}
function RetTag(const Tag, Texto: string): string;
var p : Integer;
    s : string;
begin
     Result := '';
     p      := FH.GetPos('<'+Tag+'>',Texto);
     if p > 0 then
     begin
          s := FH.CopyPosMax(Texto, '<'+Tag+'>');
          p := FH.GetPos('</'+Tag+'>',s);
          if p > 0 then
          begin
               Result := FH.CopyPos(s, '</'+Tag+'>', false);
          end;
     end;
end;

{----------------------------------------------------------------------------------------------}
function MontaTag(const Tag, Texto: string): string;
begin
     Result := '';
     if Tag > '' then
        Result := '<'+Tag+'>'+Texto+'</'+Tag+'> ';
end;

{----------------------------------------------------------------------------------------------}
function SeTemConteudo(const Texto, TxIni, TxFin: string): string;
begin
     Result := Texto;
     if FH.StrOk(Result) then
        Result := TxIni+Result+TxFin;
end;

{----------------------------------------------------------------------------------------------}
function CarregaSeExiste(L: TStrings; const pArq: string): Boolean;
begin
     Result := FileExists(pArq);
     if Result then
        L.LoadFromFile(pArq);
end;

{----------------------------------------------------------------------------------------------}
function DsParaLinha(DS: TDataSet; const pCampo: string; const Espacado: Boolean = True): string;
begin
     Result := '';
     DS.first;
     while not DS.eof do
     begin
          FH.FormaLista(Result, DS.FieldByName(pCampo).AsString, Espacado);
          DS.next;
     end;
end;

{----------------------------------------------------------------------------------------------}
function MonthFromName(const S: string; MaxLen: Byte): Byte;
begin
  if Length(S) > 0 then
    for Result := 1 to 12 do
    begin
      if (Length(LongMonthNames[Result]) > 0) and
        (AnsiCompareText(Copy(S, 1, MaxLen),
        Copy(LongMonthNames[Result], 1, MaxLen)) = 0) then
        Exit;
    end;
  Result := 0;
end;

{----------------------------------------------------------------------------------------------}
function MakeStr(C: Char; N: Integer): string;
begin
  if N < 1 then
    Result := ''
  else
  begin
    {$IFNDEF WIN32}
    if N > 255 then
      N := 255;
    {$ENDIF WIN32}
    SetLength(Result, N);
    FillChar(Result[1], Length(Result), C);
  end;
end;

{----------------------------------------------------------------------------------------------}
procedure ExtractMask(const Format, S: string; Ch: Char; Cnt: Integer;
  var I: Integer; Blank, Default: Integer);
var
  Tmp: string[20];
  J, L: Integer;
begin
  I := Default;
  Ch := UpCase(Ch);
  L := Length(Format);
  if Length(S) < L then
    L := Length(S)
  else if Length(S) > L then
    Exit;
  J := Pos(MakeStr(Ch, Cnt), AnsiUpperCase(Format));
  if J <= 0 then
    Exit;
  Tmp := '';
  while (UpCase(Format[J]) = Ch) and (J <= L) do
  begin
    if S[J] <> ' ' then
      Tmp := Tmp + S[J];
    Inc(J);
  end;
  if Tmp = '' then
    I := Blank
  else if Cnt > 1 then
  begin
    I := MonthFromName(Tmp, Length(Tmp));
    if I = 0 then
      I := -1;
  end
  else
    I := StrToIntDef(Tmp, -1);
end;

{----------------------------------------------------------------------------------------------}
function CurrentMonth: Word;
var
  SystemTime: TSystemTime;
begin
  GetLocalTime(SystemTime);
  Result := SystemTime.wMonth;
end;

{----------------------------------------------------------------------------------------------}
function ExpandYear(Year: Integer): Integer;
var
  N: Longint;
begin
  if Year = -1 then
    Result := CurrentYear
  else
  begin
    Result := Year;
    if Result < 100 then
    begin
      N := CurrentYear - CenturyOffset;
      Inc(Result, N div 100 * 100);
      if (CenturyOffset > 0) and (Result < N) then
        Inc(Result, 100);
    end;
  end;
end;

{----------------------------------------------------------------------------------------------}
function GetDateOrder(const DateFormat: string): TDateOrder;
var
  I: Integer;
begin
  Result := DefaultDateOrder;
  I := 1;
  while I <= Length(DateFormat) do
  begin
    case Chr(Ord(DateFormat[I]) and $DF) of
{$IFDEF COMPILER3_UP}
      'E':
        Result := doYMD;
{$ENDIF}
      'Y':
        Result := doYMD;
      'M':
        Result := doMDY;
      'D':
        Result := doDMY;
    else
      Inc(I);
      Continue;
    end;
    Exit;
  end;
  Result := DefaultDateOrder; { default }
end;

{----------------------------------------------------------------------------------------------}
procedure ScanBlanks(const S: string; var Pos: Integer);
var
  I: Integer;
begin
  I := Pos;
  while (I <= Length(S)) and (S[I] = ' ') do
    Inc(I);
  Pos := I;
end;

{----------------------------------------------------------------------------------------------}
function ScanNumber(const S: string; MaxLength: Integer; var Pos: Integer; var Number: Longint): Boolean;
var
  I: Integer;
  N: Word;
begin
  Result := False;
  ScanBlanks(S, Pos);
  I := Pos;
  N := 0;
  while (I <= Length(S)) and (Longint(I - Pos) < MaxLength) and
    (S[I] in ['0'..'9']) and (N < 1000) do
  begin
    N := N * 10 + (Ord(S[I]) - Ord('0'));
    Inc(I);
  end;
  if I > Pos then
  begin
    Pos := I;
    Number := N;
    Result := True;
  end;
end;

{----------------------------------------------------------------------------------------------}
function ScanChar(const S: string; var Pos: Integer; Ch: Char): Boolean;
begin
  Result := False;
  ScanBlanks(S, Pos);
  if (Pos <= Length(S)) and (S[Pos] = Ch) then
  begin
    Inc(Pos);
    Result := True;
  end;
end;

{----------------------------------------------------------------------------------------------}
function ScanDate(const S, DateFormat: string; var Pos: Integer; var Y, M, D: Integer): Boolean;
var
  DateOrder: TDateOrder;
  N1, N2, N3: Longint;
begin
  Result := False;
  Y := 0;
  M := 0;
  D := 0;
  DateOrder := GetDateOrder(DateFormat);
{$IFDEF COMPILER3_UP}
  if ShortDateFormat[1] = 'g' then { skip over prefix text }
    ScanToNumber(S, Pos);
{$ENDIF COMPILER3_UP}
  if not (ScanNumber(S, MaxInt, Pos, N1) and ScanChar(S, Pos, DateSeparator) and
    ScanNumber(S, MaxInt, Pos, N2)) then
    Exit;
  if ScanChar(S, Pos, DateSeparator) then
  begin
    if not ScanNumber(S, MaxInt, Pos, N3) then
      Exit;
    case DateOrder of
      doMDY:
        begin
          Y := N3;
          M := N1;
          D := N2;
        end;
      doDMY:
        begin
          Y := N3;
          M := N2;
          D := N1;
        end;
      doYMD:
        begin
          Y := N1;
          M := N2;
          D := N3;
        end;
    end;
    Y := ExpandYear(Y);
  end
  else
  begin
    Y := CurrentYear;
    if DateOrder = doDMY then
    begin
      D := N1;
      M := N2;
    end
    else
    begin
      M := N1;
      D := N2;
    end;
  end;
  ScanChar(S, Pos, DateSeparator);
  ScanBlanks(S, Pos);
{$IFDEF COMPILER3_UP}
  if SysLocale.FarEast and (System.Pos('ddd', ShortDateFormat) <> 0) then
  begin { ignore trailing text }
    if ShortTimeFormat[1] in ['0'..'9'] then { stop at time digit }
      ScanToNumber(S, Pos)
    else { stop at time prefix }
      repeat
        while (Pos <= Length(S)) and (S[Pos] <> ' ') do
          Inc(Pos);
        ScanBlanks(S, Pos);
      until (Pos > Length(S)) or
        (AnsiCompareText(TimeAMString, Copy(S, Pos, Length(TimeAMString))) = 0) or
        (AnsiCompareText(TimePMString, Copy(S, Pos, Length(TimePMString))) = 0);
  end;
{$ENDIF COMPILER3_UP}
  Result := IsValidDate(Y, M, D) and (Pos > Length(S));
end;

{----------------------------------------------------------------------------------------------}
function ScanDateStr(const Format, S: string; var D, M, Y: Integer): Boolean;
var
  Pos: Integer;
begin
  ExtractMask(Format, S, 'm', 3, M, -1, 0); { short month name? }
  if M = 0 then ExtractMask(Format, S, 'm', 1, M, -1, 0);
  ExtractMask(Format, S, 'd', 1, D, -1, 1);
  ExtractMask(Format, S, 'y', 1, Y, -1, CurrentYear);
  if M = -1 then
    M := CurrentMonth;
  Y := ExpandYear(Y);
  Result := IsValidDate(Y, M, D);
  if not Result then
  begin
    Pos := 1;
    Result := ScanDate(S, Format, Pos, Y, M, D);
  end;
end;

{----------------------------------------------------------------------------------------------}
function VerSessaoRemota: Boolean;
const
     sm_RemoteSession = $1000; { from WinUser.h }
begin
     Result := (GetSystemMetrics(sm_RemoteSession) <> 0);
end;

{----------------------------------------------------------------------------------------------}
function TruncVal(const V: Extended): Extended;
var s: string;
begin
     Result := V;
     if frac(V) > 0 then
     begin
          s := FH.FmtFix(V, 8);
          s := FH.CopyPos(s, DecimalSeparator)+DecimalSeparator+
               copy(FH.CopyPosMax(s, DecimalSeparator),1,2);
          Result := StrToFloat(s);
     end;
end;

end.




