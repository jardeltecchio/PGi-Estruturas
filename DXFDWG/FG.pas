unit FG;

interface

uses
  Messages, SysUtils, Classes, Controls, StdCtrls, ExtCtrls, Jpeg, Windows, Graphics, Dialogs,
  Printers, Forms, DBGrids, ShellAPI, JvSpin, Menus, ComCtrls, StrUtils, Math, db, Mask, DbClient,
  Consts, Checklst, Spin, Buttons, Grids, Variants, DBCommon, JvToolEdit, ValEdit, MidasLib, JvMail,
  DbCtrls, JvGroupHeader, JvEdit, JclFileUtils, JclSysInfo, FH, JvCtrls, IBODataSet, JvBitBtn,
  inifiles, JvFormPlacement, JclDateTime, IB_Components, cxExportGrid4Link, Excel97, OleServer,
  cxCustomData, cxGraphics, cxFilter, cxData, cxDataStorage, cxEdit, cxDBData, cxGridLevel,
  cxClasses, cxControls, cxGridCustomView, cxGridCustomTableView, cxGridTableView, cxButtons,
  cxGridDBTableView, cxGrid, cxLookAndFeels, cxCheckComboBox, cxCheckBox, cxGridDBBandedTableView,
  cxPC, cxLookupEdit, cxDBLookupEdit, cxDBLookupComboBox, typInfo, frxClass, frxDBSet, cxspinEdit,
  cxLabel, JvExButtons, JvExControls, JvExMask, jvBaseEdits, JvAppStorage, JvAppIniStorage,
  cxGridDBDataDefinitions, DateUtils, cxImage, Chart, DBChart, Series, TeEngine, JvTypes, Registry,
  cxLookAndFeelPainters;

const
  // FTP __________________________________________
  CLI_DEFAULT        = '$Cli_Default';
  COR_AGENDA_PAL     = clRed;
  COR_AGENDA_PBX     = clBlue;
  COR_AGENDA_PMD     = clBlack;
  DEF_ANOTA          = '$Form_Anota';
  FONTE_ARIAL        = 'Arial';
  FONTE_COURIER      = 'Courier New';
  FONTE_TAHOMA       = 'Tahoma';
  FOR_DEFAULT        = '$For_Default';
  FTP_ENDERECO       = 'ftp.pressier.com.br';
  FTP_SENHA          = 'presweb1841';
  FTP_USUARIO        = 'pressier';
  LIBERA_USU         = '$Libera_Usu';
  LICENCA_MAGNABOSCO = 'MAGNABOSCO';
  LICENCA_PRESSIER   = 'PRESSIER';
  MASK_CNPJ          = '99.999.999/9999-99';
  MASK_CPF           = '999.999.999-99';
  MASK_DATA          = '99/99/9999';
  NOME_INTERNO       = 'prERP2';
  NOME_SIS           = 'ERP/2';
  RECADO_HORA_VERIF  = '$Sis_Recado_HoraVer';
  RECADO_MSG         = '$Sis_Recado_Msg';
  REG_CAIXA_LOCAL    = '$Caixa_Local';
  REG_CHAVE_FILTRO   = 'Geral.Filtro';
  REG_CHAVE_GERAL    = 'Geral.Sistema';
  REG_ID_DEFAULT     = -56970017;
  SECAO_GERAL        = 'Geral';
  SENHAS_DEFAULT     : array[1..3] of string = ('SSERP2', '[][]', '||');
  SIMBOLO_REAL       = 'R$';
  TAM_PWD            = 40;


  // Geração de Contratos _________________________
  RecMovContrato = 'Rec_Mov_Contrato';


  // Filtros ______________________________________
  FILTRO_CLIENTE = 'FCliFiltro';
  FILTRO_COMPRA  = 'FComNotaFiltro';
  FILTRO_FORNEC  = 'FForFiltro';
  FILTRO_PEDCOM  = 'FComPedFiltro';
  FILTRO_PEDVEN  = 'FVenPedFiltro';
  FILTRO_PRODUTO = 'FProFiltro';
  FILTRO_VENDA   = 'FVenNotaFiltro';


  // Configuração do Estabelecimento ______________________
  CfgEstPerDesAniv           = 'Per_Des_Aniv';
  CfgEstPerDesLiqGer         = 'Per_Des_LiqGer';
  CfgEstDtIniDesLiqGer       = 'Ini_Des_LiqGer';
  CfgEstDtFimDesLiqGer       = 'Fim_Des_LiqGer';
  CfgEstCodContabilista      = 'Cod_Contabilista';
  CfgEstCodCoordTI           = 'Coord_TI';
  CfgEstStqMovTipo_Troca     = 'StqMovTipo_Troca';
  CfgEstStqMovTipo_VenPed    = 'StqMovTipo_VenPed';
  CfgEstStqMovTipo_Req       = 'StqMovTipo_Req';
  CfgEstDirDllTef            = 'Dir_Dll_Tef';
  CfgEstAcumulaProduto       = 'Acu_Produto';
  CfgEstAtrAviso             = 'Atr_Aviso';
  CfgEstAtuCustoStq          = 'Atu_Custo_Stq';
  CfgEstAvisDifRec           = 'Avis_Dif_Rec';
  CfgEstAvisPtEnc            = 'Avis_Pt_Enc';
  CfgEstBloqueia             = 'Bloqueia';
  CfgEstBloqueiaVp           = 'Bloqueia_Vp';
  CfgEstBloqVendaConjAuto    = 'Bloq_Venda_Conj_Auto';
  CfgEstCartao               = 'Cartao';
  CfgEstTranspPadrao         = 'Transp_Padrao';
  CfgEstChrCartao            = 'Chr_Cartao';
  CfgEstCliGeral             = 'Cli_Geral';
  CfgEstComissaoNF           = 'Comissao_NF';
  CfgEstContatoProrroga      = 'Contato_Prorroga';
  CfgEstCupomSemCliente      = 'CF_S_Cli';
  CfgEstDiaBase              = 'Dia_Base';
  CfgEstDiasBlqCliInat       = 'Dias_Blq_Cli_Inat';
  CfgEstDigCodigo            = 'Dig_Codigo';
  CfgEstDigPreco             = 'Dig_Preco';
  CfgEstDirErroServ          = 'Dir_Erro_Serv';
  CfgEstDirParam             = 'Dir_Param';
  CfgEstDirRAT               = 'Dir_RAT';
  CfgEstCamUniversal         = 'Cam_Universal';
  CfgEstDirTef               = 'Dir_Tef';
  CfgEstDirNFeGera           = 'Dir_NFe_Gera';
  CfgEstDirNFeLeitura        = 'Dir_NFe_Leitura';
  CfgEstEcfNaoIncRet         = 'Ecf_Nao_Inc_Ret';
  CfgEstEcfResVenCrt         = 'Ecf_Res_Ven_Crt';
  CfgEstEcfViasRec           = 'Ecf_Vias_Rec';
  CfgEstEmiEstorno           = 'Emi_Estorno';
  CfgEstEcfNaoImpReserva     = 'Ecf_Nao_Imp_Reserva';
  CfgEstHistoricoPagNFE      = 'Historico_Pag_NFE';
  CfgEstContaPagNFE          = 'Cont_Pag_NFE';
  CfgEstPortadorPagNFE       = 'Portador_Pag_NFE';
  CfgEstExtLogo              = 'Ext_Logo';
  CfgEstExtMiniLogo          = 'Ext_MiniLogo';
  CfgEstExtPapelParede       = 'Ext_Papel_Parede';
  CfgEstFicha                = 'Ficha';
  CfgEstFixaDiaVen           = 'Fixa_Dia_Ven';
  CfgEstForGeral             = 'For_Geral';
  CfgEstFormaTroco           = 'Forma_Troco';
  CfgEstGravaReserva         = 'Grava_Reserva';
  CfgEstHisTroco             = 'His_Troco';
  CfgEstIdePesado            = 'Ide_Pesado';
  CfgEstLePesado             = 'Le_Pesado';
  CfgEstLimiteOct            = 'Limite_Oct';
  CfgEstLmcDiaBase           = 'Lmc_Dia_Base';
  CfgEstLmcPerCoj            = 'Lmc_Per_Coj';
  CfgEstLmcPeriodo           = 'Lmc_Periodo';
  CfgEstLmcPerTit            = 'Lmc_Per_Tit';
  CfgEstLmcTipo              = 'Lmc_Tipo';
  CfgEstLocPapelParede       = 'Loc_Papel_Parede';
  CfgEstMargem               = 'Margem';
  CfgEstMarkup               = 'Markup';
  CfgEstMsgCupom             = 'Msg_Cupom';
  CfgEstMsgDemo              = 'Msg_Demo';
  CfgEstMsgDivida            = 'Msg_Divida';
  CfgEstNaoDuplicaRef        = 'Nao_Duplica_Ref';
  CfgEstDuplicaAlt           = 'Duplica_Alt';
  CfgEstNaoMostraStqAmx      = 'Nao_Mostaa_StqAmx';
  CfgEstNaoVisAbaCli         = 'Nao_Vis_Aba_Cli';
  CfgEstNaoSolCodCli         = 'Nao_Sol_Cod_Cli';
  CfgEstNatureza             = 'Natureza';
  CfgEstNaturezaNf           = 'Natureza_Nf';
  CfgEstNaturezaOrc          = 'Natureza_Orc';
  CfgEstNfSemPreco           = 'Nf_Sem_Preco';
  CfgEstNfSemQuantidade      = 'Nf_Sem_Quantidade';
  CfgEstNota1                = 'Nota_1';
  CfgEstNota2                = 'Nota_2';
  CfgEstObrAltEntNF          = 'Obr_Alt_Ent_NF';
  CfgEstPortador             = 'Portador';
  CfgEstPortCob              = 'Port_Cob';
  CfgEstProdutoAcesso        = 'Produto_Acesso';
  CfgEstPvPadrao             = 'Pv_Padrao';
  CfgEstQtdEmb               = 'Qtd_Emb';
  CfgEstReservaVenda         = 'Reserva_Venda';
  CfgEstSer1                 = 'Ser_1';
  CfgEstSer2                 = 'Ser_2';
  CfgEstSerNFEnt             = 'Ser_NF_Ent';
  CfgEstSerNFSai             = 'Ser_NF_Sai';
  CfgEstSolMoeda             = 'Sol_Moeda';
  CfgEstStqTipoComp          = 'Stq_Tipo_Compos';
  CfgEstTextoCli             = 'Texto_Cli';
  CfgEstTrabalhaProPesado    = 'Trabalha_Pro_Pesado';
  CfgEstUpper                = 'Upper';
  CfgEstUsaIPI               = 'Usa_IPI';
  CfgEstUtilizaTef           = 'Utiliza_Tef';
  CfgEstValDecimal           = 'Val_Decimal';
  CfgEstValPre               = 'Val_Pre';
  CfgEstVendGeral            = 'Vend_Geral';
  CfgEstCaixaGeral           = 'Caixa_Geral';
  CfgEstVisUltimoPreco       = 'Vis_Ultimo_Preco';
  CfgEstSimples              = 'Simples_Nacional';
  CfgEstRetBancoLyt          = 'Ret_Banco_Lyt';
  CfgEstRetBancoFormaPag     = 'Ret_Banco_FormaPag';
  CfgEstComNtaCxFormaPg      = 'Com_Nta_Cx_FormaPg';
  CfgEstComNtaCxHis          = 'Com_Nta_Cx_His';
  CfgEstOcoTipos             = 'Oco_Tipos';
  CfgEstTipoVisita           = 'Tipo_Visita';
  CfgEstValidadeTroca        = 'Validade_Troca';
  CfgEstValidadeDemo         = 'Validade_Demo';
  CfgEstAuditaDataPos        = 'Audita_DataPos';
  CfgEstAuditaDataTam        = 'Audita_DataTam';
  CfgEstAuditaFoneTam        = 'Audita_FoneTam';
  CfgEstAuditaFonePos        = 'Audita_FonePos';
  CfgEstAuditaArquivos       = 'Audita_Arquivos';
  CfgEstAuditaDurLigPos      = 'Audita_DurLigPos';
  CfgEstAuditaDurLigTam      = 'Audita_DurLigTam';
  CfgEstAuditaHrLigPos       = 'Audita_HrLigPos';
  CfgEstAuditaHrLigTam       = 'Audita_HrLigTam';
  CfgEstAuditaDurSupMin      = 'Audita_DurSupMin';
  CfgEstAuditaDurSupSeg      = 'Audita_DurSupSeg';
  CfgEstPerMetaCli           = 'Per_Meta_Cli';
  CfgEstUsaDesFiscalPro      = 'Usa_DesFiscal_Pro';
  CfgEstSomRecado            = 'Som_Recado';
  CfgEstFormaContrato        = 'Forma_Contrato';
  CfgEstFormaContratoDefault = 'Forma_Contrato_Default';
  CfgEstPadraoFrete          = 'Padrao_Frete';
  CfgEstNaoSolOperador       = 'Nao_Sol_Operador';
  CfgEstVendGeralCliente     = 'Vend_Geral_Cliente';
  CfgEstAvisaMesAniversario  = 'Avisa_Mes_Aniversario';
  CfgEstAliqIcmsSimples      = 'Aliq_Icms_Simples';
  CfgEstTextoNfSimples       = 'Texto_Nf_Simples';
  CfgEstTextoNfSubst         = 'Texto_Nf_Subst';
  CfgEstLocalTextoNfSimples  = 'Local_Texto_Nf_Simples';
  CfgEstLocAvanCli           = 'Loc_Avan_Cli';
  CfgEstLocAvanFor           = 'Loc_Avan_For';
  CfgEstLocAvanPro           = 'Procura_Parcial';
  CfgEstBloqNroDocNf         = 'Bloq_NroDoc_Nf';
  CfgEstEstenderPapelParede  = 'Estender_Papel_Parede';
  CfgEstImpressoraTroca      = 'Impressora_Troca';
  CfgEstTipoVendaIniCF       = 'TipoVenda_Inicio_Cupom';
  CfgEstNaoSolLibPed         = 'Nao_Sol_Lib_Ped';
  CfgEstCmsCliParceiro       = 'Cms_Cli_Parceiro';
  CfgEstSeqIncCli            = 'Seq_Inc_Cli';
  CfgEstDescPontaStq         = 'Desc_Ponta_Stq';
  CfgEstCenMatExp            = 'Cen_Mat_Exp';
  CfgEstUsuRespCobranca      = 'Usu_Resp_Cobranca';
  CfgEstUsuRespCobrancaSPC   = 'Usu_Resp_Cobranca_SPC';
  CfgEstUsuSenhaSPC          = 'Usu_Senha_SPC';
  CfgEstUsuLogonSPC          = 'Usu_Logon_SPC';
  CfgEstServidorSPC          = 'Servidor_SPC';
  CfgEstNFeNumCert           = 'NFe_Num_Cert';
  CfgEstNFeAmbiente          = 'NFe_Ambiente';
  CfgEstLibStqPed            = 'Lib_Stq_Ped';
  CfgEstNFeServidor          = 'NFe_Servidor';
  CfgEstTipoDanfe            = 'Tipo_Danfe';
  CfgEstNFePorta             = 'NFe_Porta';
  CfgEstPerIniPPRTerminal    = 'Per_Ini_PPR_Terminal';
  CfgEstPerFinPPRTerminal    = 'Per_Fin_PPR_Terminal';
  CfgEstExtLogoDanfe         = 'Ext_Logo_Danfe';
  CfgEstNfeRefFor            = 'Nfe_Ref_For';
  CfgEstNfeVendedor          = 'Nfe_Vendedor';
  CfgEstNfeUsaRef            = 'Nfe_Usa_Ref';
  CfgEstNumLivroEnt          = 'Num_Livro_Ent';
  CfgEstNumLivroSai          = 'Num_Livro_Sai';


  // Configuração do tipo de entrada _________________________
  CfgTipEntCustoAdicional = 'Custo_Adicional';
  CfgTipObrigaPedNt       = 'Obriga_Ped_Nt';


  // Liberação de usuário _________________________
  CfgUsuVenTefInoperante      = 'Ven_Tef_Inoperante';
  CfgUsuCancelaTroca          = 'Cancela_Troca';
  CfgUsuLiquidaTroca          = 'Liquida_Troca';
  CfgUsuAbreAgenda            = 'Abre_Agenda';
  CfgUsuVisEventoAge          = 'Vis_Agenda';
  CfgUsuAltAgenda             = 'Alt_Agenda';
  CfgUsuAbreLoja              = 'Abre_Loja';
  CfgUsuAltCfgLocOct          = 'Alt_Cfg_Loc_Oct';
  CfgUsuAltDadoContrato       = 'Alt_Dado_Contrato';
  CfgUsuAltDadPar             = 'Alt_Dad_Parcelamento';
  CfgUsuAlteraNota            = 'Altera_Nota';
  CfgUsuAlteraPrecoCompra     = 'Altera_Preco_Compra';
  CfgUsuAltLimite             = 'Alt_Limite';
  CfgUsuAltPrcCFNFOCT         = 'Alt_Prc_CFNFOCT';
  CfgUsuAtuStqManNtEnt        = 'Atu_Stq_Man_Nt_Ent';
  CfgUsuCancelaItem           = 'Cancela_Item';
  CfgUsuCancelaNFCF           = 'Cancela_NFCF';
  CfgUsuDataServDif           = 'Data_Serv_Dif';
  CfgUsuVenPedidoAtu          = 'VenPedido_Atu';
  CfgUsuEcfOpe                = 'Ecf_Ope';
  CfgUsuEstornaNC             = 'Estorna_NC';
  CfgUsuExcContrato           = 'Exc_Contrato';
  CfgUsuExcFormaNF            = 'Exc_Forma_NF';
  CfgUsuExcNFCF               = 'Exc_NFCF';
  CfgUsuExcRecLoja            = 'Exc_Rec_Loja';
  CfgUsuFinResDataDif         = 'Fin_Res_Data_Dif';
  CfgUsuIncBaiCli             = 'Inc_Bai_Cli';
  CfgUsuIncProfCli            = 'Inc_Prof_Cli';
  CfgUsuIncBaiFor             = 'Inc_Bai_For';
  CfgUsuIncCidCli             = 'Inc_Cid_Cli';
  CfgUsuIncCidFor             = 'Inc_Cid_For';
  CfgUsuIncContAvu            = 'Inc_Cont_Avu';
  CfgUsuIncMovLoja            = 'Inc_Mov_Loja';
  CfgUsuLibCxDifCtb           = 'Lib_Cx_Dif_Ctb';
  CfgUsuLiberaAtrasoDev       = 'Orc_Atr';
  CfgUsuLiberaQtdDev          = 'Orc_Dev';
  CfgUsuLiberaQtdDevSai       = 'Orc_Dev_Sai';
  CfgUsuManNtEntStq           = 'Man_Nt_Ent_Stq';
  CfgUsuManProFin             = 'Man_Pro_Fin';
  CfgUsuMovDia                = 'Mov_Dia';
  CfgUsuParConPag             = 'Par_Con_Pag';
  CfgUsuProrrogaDia           = 'Prorroga_Dia';
  CfgUsuRecCliBlq             = 'Rec_Cli_Blq';
  CfgUsuRecDescMax            = 'Rec_Desc_Max';
  CfgUsuRecDiasAtr            = 'Rec_Dias_Atr';
  CfgUsuRecJurMulMax          = 'Rec_Jur_Mul_Max';
  CfgUsuRecParNovaAnt         = 'Rec_Par_Nova_Ant';
  CfgUsuVenCliAtraso          = 'Ven_Cli_Atraso';
  CfgUsuVenCliInativo         = 'Ven_Cli_Inativo';
  CfgUsuVenDescMax            = 'Ven_Desc_Max';
  CfgUsuVenLimiteEx           = 'Ven_Limite_Ex';
  CfgUsuVenProSalNgt          = 'Ven_Pro_Sal_Ngt';
  CfgUsuVisRsmCax             = 'Vis_Rsm_Cax';
  CfgUsuVisRsmDia             = 'Vis_Rsm_Dia';
  CfgUsuVenCliEndAtr          = 'Ven_Cli_End_atr';
  CfgUsuOpeCliRestrito        = 'Ope_Cli_Restrito';
  CfgUsuImpPedCom             = 'Imp_Ped_Com';
  CfgUsuDupPedCom             = 'Imp_Dup_Com';
  CfgUsuIncDadObrCli          = 'Inc_Dad_Obr_Cli';
  CfgUsuMarkupFora            = 'Markup_Fora';
  CfgUsuListaAtalho           = 'Lista_Atalho';
  CfgUsuAltDesctoDpl          = 'Alt_Descto_Dpl';
  CfgUsuRecadosOnline         = 'Recados_Online';
  CfgUsuAltMovLoja            = 'Alt_Mov_Loja';
  CfgUsuGravaCfgLote          = 'Grava_Cfg_Lote';
  CfgUsuMovProNegativo        = 'Mov_Pro_Negativo';
  CfgUsuExcMovStq             = 'Exc_Mov_Stq';
  CfgUsuEnvEmailDefault       = 'Env_Email_Default';
  CfgUsuIncNFSemPedCom        = 'Inc_NF_Sem_Pedcom';
  CfgUsuAltPedVen             = 'Alt_Ped_Ven';
  CfgUsuAltPedVenCons         = 'Alt_Ped_Ven_Cons';
  CfgUsuAltPedOrc             = 'Alt_Ped_Orc';
  CfgUsuPressierNoteDiasSinc  = 'Pressier_Note_Dias_Sinc';
  CfgUsuPressierNoteBloqueado = 'Pressier_Note_Bloqueado';
  CfgUsuAcessarCliVinc        = 'Acessar_Cli_Vinc';
  CfgUsuAltCliOutEstabe       = 'Alt_Cli_Out_Estabe';
  CfgUsuNaoDescGerIte         = 'NAO_DESC_GER_ITE';
  CfgUsuNaoDescItem           = 'NAO_DESC_ITEM';
  CfgUsuAltTipoCli            = 'Alt_Tipo_Cli';
  CfgUsuAltPercDescVenCli     = 'Alt_Perc_Desc_VenCli';
  CfgUsuRecebeReq             = 'Recebe_Req';
  CfgUsuNaoVisCustoPro        = 'NAO_VIS_CUSTO';
  CfgUsuVisAgendaFone         = 'VIS_AGENDA_FONE';
  CfgUsuExcPedVenCons         = 'Exc_Ped_Ven_Cons';
  CfgUsuExcPedVen             = 'Exc_Ped_Ven';
  CfgUsuExcOrcVen             = 'Exc_Orc_Ven';
  CfgUsuSPCProd               = 'SPC_Prod';
  CfgUsuVisCustoConsultaPro   = 'VIS_CUSTO_CONSULTA_PRO';
  CfgUsuRecChequeParcial      = 'REC_CHEQUE_PARCIAL';

  // Configuração Local ___________________________
  CfgLocOctPortaMI      = 'Oct_Porta_MI';
  CfgLocOctNroVias      = 'Oct_Vias_JT';
  CfgLocOctImpPadrao    = 'Oct_Imp_Padrao';
  CfgLocOctImp          = 'Oct_Imp';
  CfgLocOctColunas      = 'Oct_Colunas';


  // Configuração de pedidos de compra _____________
  CfgComPedNaoImpClas1  = 'Nao_Imp_Clas1';
  CfgComPedNaoImpClas2  = 'Nao_Imp_Clas2';
  CfgComPedNaoImpClas3  = 'Nao_Imp_Clas3';
  CfgComPedNaoImpConPag = 'Nao_Imp_ConPag';
  CfgComPedObs          = 'Obs';
  CfgComPedFonteObs     = 'Fonte_Obs';
  CfgComPedAjuObs       = 'Aju_Obs';
  CfgComPedTituloRel    = 'Titulo_Rel';
  CfgComPedListaDtFat   = 'Lista_DtFat';
  CfgComPedNaoImpVenctos= 'Nao_Imp_Venctos';

  
  // Configuração de pedidos de venda _____________
  CfgVenPedDesabEntrega = 'Oct_Desab_Entrega';
  CfgVenPedImpLogo      = 'Oct_Imp_Logo';
  CfgVenPedMoeda        = 'Oct_Moeda';
  CfgVenPedNaoSolCli    = 'Oct_Nao_Sol_Cli';
  CfgVenPedNaoSolPrc    = 'Oct_Nao_Sol_Prc';
  CfgVenPedNaoSolQtd    = 'Oct_Nao_Sol_Qtd';
  CfgVenPedNaoSolQtdDev = 'Oct_Nao_Sol_Qt_Dev';
  CfgVenPedNaoSolCfop   = 'Oct_Nao_Sol_Cfop';
  CfgVenPedTipoOct      = 'Oct_Tipo_Oct';
  CfgVenPedTipoPed      = 'Oct_Tipo_Ped';
  CfgVenPedTipoCon      = 'Oct_Tipo_Con';
  CfgVenPedModelo       = 'Modelo';
  CfgVenPedOrigem       = 'Origem';
  CfgVenPedTipo         = 'Tipo';
  CfgVenPedImpMultiLinha= 'Imp_Multi_Linha';
  CfgVenPedImpEntDias   = 'Imp_Ent_Dias';
  CfgVenPedStqPed       = 'Nao_Stq_Ped';
  CfgVenPedStqOrc       = 'Nao_Stq_Orc';
  CfgVenPedStqCons      = 'Nao_Stq_Cons';
  CfgVenPedCompItem     = 'Comp_Item';
  CfgVenPedSolOrigem    = 'Sol_Origem';
  CfgVenPedSolIndice    = 'Sol_Indice';
  CfgVenPedUsuLibera    = 'Usu_Libera';
  CfgVenPedSolObs       = 'Sol_Obs';
  CfgVenPedAtuEmissao   = 'Atu_Emissao';
  CfgVenPedNaoListaRef  = 'Nao_Lista_Ref';
  CfgVenPedNaoGeraRec   = 'Nao_Gera_Rec';
  CfgVenPedObsCons      = 'Obs_Cons';
  CfgVenPedObsOrc       = 'Obs_Orc';
  CfgVenPedObsPed       = 'Obs_Ped';
  CfgVenPedAumentaPreco = 'Aumenta_Preco';
  CfgVenPedNaoAltVen    = 'Nao_Alt_Ven_Cli';
  CfgVenPedAcaoFinal    = 'Acao_Finalizar';
  CfgVenPedCliPosPed    = 'CliPos_Ped';

  // Configuração de Notas ________________________
  CfgNfiCodPro         = 'Cod_Pro';
  CfgNfiConVctAV       = 'Con_Vct_AV';
  CfgNfiDesSrvIte      = 'Des_Srv_Ite';
  CfgNfiImpDef         = 'Imp_Def';
  CfgNfiLinha          = 'Linha';
  CfgNfiObsIte         = 'Obs_Ite';
  CfgNfiPedObs         = 'Ped_Obs';
  CfgNfiSitICMObs      = 'Sit_ICM_Obs';
  CfgNfiSolTraVen      = 'Sol_Tra_Ven';
  CfgNfiTipoPap        = 'Tipo_Pap';
  CfgNfiVctAA          = 'Vct_AA';
  CfgNfiDesPer         = 'Des_Per';
  CfgNfiNaoImpObsIte   = 'Nao_Imp_Obs_Ite';
  CfgNfiCompItem       = 'Comp_Item';
  CfgNfiObsPedido      = 'Obs_Pedido';
  CfgNfiObsItensPedido = 'Obs_Item_Pedido';
  CfgNfiNaoSugDtHr     = 'Nao_Sug_DtHr';
  CfgNfiClasFiscal     = 'Clas_Fiscal';
  CfgNfiQtdClasFiscal  = 'Qtd_Clas_Fiscal';


  // Configuração de Bloquetos ____________________
  CfgBloqAtuNumero    = 'Atu_Nosso_Numero';
  CfgBloqAcuDebito    = 'Acu_Debito';
  CfgBloqCobRegistro  = 'Cob_Registro';
  CfgBloqMovMes       = 'Mov_Mes';
  CfgBloqObsItNf      = 'Obs_I_Nf';
  CfgBloqObsCon       = 'Obs_Con';
  CfgBloqExtBco       = 'Ext_Bco';
  CfgBloqExtEnt       = 'Ext_Ent';
  CfgBloqLocalPagto   = 'LOCAL_PAGTO';
  CfgBloqTipoPapel    = 'TIPO_PAPEL';


  // tipos de movimentação do estoque _____________
  StqMovCompra   = 0;
  StqMovVenda    = 1;
  StqMovTransSai = 2;
  StqMovTransEnt = 3;
  StqMovDevCli   = 4;
  StqMovDevFor   = 5;
  StqMovAcerto   = 6;
  StqMovOutSai   = 7;
  StqMovOutEnt   = 8;


  // Consultas ____________________________________
  CnsAlteraCh             = '*Cns_Altera_Ch';
  CnsCampoCh              = '*Cns_Campo_Ch';
  CnsFormOrg              = '*Cns_Form_Org';
  CnsIndexRet             = '*Cns_Index_Ret';
  CnsIndexCEP             = '*Cns_Index_CEP';

  CnsCepLog               = '*Cns_Cep_Log';
  CnsCepCEP               = '*Cns_Cep_CEP';
  CnsCepBai               = '*Cns_Cep_Bai';
  CnsCepCid               = '*Cns_Cep_Cid';
  CnsCepEst               = '*Cns_Cep_Est';
  CnsCepCEPCorresp        = '*Cns_Cep_CEP_Corresp';

  CnsComCotaTipo          = '*Cns_Com_Cota_Tipo';
  CnsComCotaMesAno        = '*Cns_Com_Cota_Mes_Ano';
  CnsComCotaEstabe        = '*Cns_Com_Cota_Estabe';
  CnsComCotaCodigo        = '*Cns_Com_Cota_Codigo';

  CnsVenCotaTipo          = '*Cns_Ven_Cota_Tipo';
  CnsVenCotaMesAno        = '*Cns_Ven_Cota_Mes_Ano';
  CnsVenCotaEstabe        = '*Cns_Ven_Cota_Estabe';
  CnsVenCotaCodigo        = '*Cns_Ven_Cota_Codigo';

  CnsProdutoCodigo        = '*Cns_Produto_Codigo';
  CnsProdutoDescricao     = '*Cns_Produto_Descricao';

  CnsClienteCodigo        = '*Cns_Cliente_Codigo';
  CnsClienteOrdem         = '*Cns_Cliente_Ordem';
  CnsClienteCodigoOrdem   = '*Cns_Cliente_Codigo_Ordem';
  CnsClienteNome          = '*Cns_Cliente_Nome';

  CnsFornecedorCodigo     = '*Cns_Fornecedor_Codigo';
  CnsFornecedorNome       = '*Cns_Fornecedor_Nome';

  CnsVenPedNumero         = '*Cns_Ven_Ped_Numero';

  CnsContatoID            = '*Cns_Contato_ID';
  CnsContatoNome          = '*Cns_Contato_Nome';


  // Atributos ____________________________________

  // Constante pública usada pela nota fiscal eletronica Marcopolo
  // onde é associado ao produto para obter o código interno do produto na Marcopolo
  AtributoPro_CodMarcopolo = 'COD_MARCOPOLO';

  // Constante pública usada pela Mastersul para "trocar" o código da movimentação do estoque
  // Exemplo: vende PICANHA mas da saida no estoque de DIANTEIRO
  // Ver: FGStq.RetCodEstoque
  AtributoPro_CodEstoque = 'COD_ESTOQUE';

  // Constante pública usada pela Mastersul para reduzir a quantidade do produto na entrada da NF de compra
  // Exemplo: compra DIANTEIRO DE BOI e tem uma perda de 20% após ser cortado nas partes (PICANHA,ETC).
  AtributoPro_PerPerda = 'PER_PERDA';

  // Constante pública usada para habilitar a digitação
  // do nome do cliente na emissão do cupom fiscal 
  AtributoCli_AlteraNome = 'CONVENIO_ALTERANOME';

  // Constante pública usada para informar o código de grade usado por um produto
  // É utilizado no Magnabosco para definir a grade do pedido de compra
  AtributoPro_CodGrade = 'COD_GRADE';

  // Constante pública usada para informar o CRC do contador
  // É utilizado na geração do SPED Fiscal/Contábil
  AtributoFor_CrcContador = 'CRC_CONTADOR';

  // Arquivos _____________________________________
  ArqCaixa        = 'Caixa.ini';
  ArqLogErro      = 'LogErro.txt';
  ArqScript       = 'Scripts.xml';
  ArqCfgLocal     = 'CfgLocal.ini';


  // Usadas pela função ChamaInputBox ___________
  IBoxPasw   = -1;
  IBoxDef    = 0;
  IBoxDate   = 1;
  IBoxNum    = 2;
  IBoxInt    = 3;
  IBoxMesAno = 4;
  IBoxFmt    = ',0.00';
  IBoxDecs   = 2;
  IBoxTexto  = 8;


  // tipos de natureza de operação ________________
  CfopVen = 'V';
  CfopTrf = 'T';
  CfopCom = 'C';
  CfopDvc = 'D';
  CfopDvf = 'F';
  CfopOut = 'O';
  CfopIcm = 'I';
  CfopIpi = 'P';
  CfopRet = 'R';
  CfopSub = 'S';


  // tipos de estabelecimento _____________________
  TpEstMov = 'M';
  TpEstStq = 'E';
  TpEstAmx = 'A';


  // NFE _____________________________________________
  // Modalidade Definição da BC ICMS NOR
  NFE_ModBC                                 = 'DEFINIÇÃO_BC_ICMS_NOR';
  NFE_ModBC_MrgVlrAgregado                  = '0';
  NFE_ModBC_MrgVlrAgregado_Des              = 'Margem Valor Agregado (%)';
  NFE_ModBC_Pauta                           = '1';
  NFE_ModBC_Pauta_Des                       = 'Pauta (valor)';
  NFE_ModBC_PrcTabelado                     = '2';
  NFE_ModBC_PrcTabelado_Des                 = 'Preço Tabelado Máximo (valor)';
  NFE_ModBC_VlrOperacao                     = '3';
  NFE_ModBC_VlrOperacao_Des                 = 'Valor da Operação';
  // Tributação do PIS
  NFE_PIS_CST                               = 'TRIBUTACAO_PIS';
  NFE_PIS_CST_TribMonofasica                = '04';
  NFE_PIS_CST_TribMonofasica_Des            = 'Operação Tributável - Tributação Monofásica - (Alíquota Zero)';
  NFE_PIS_CST_TribAliqZero                  = '06';
  NFE_PIS_CST_TribAliqZero_Des              = 'Operação Tributável - Alíquota Zero';
  NFE_PIS_CST_InsetoContrib                 = '07';
  NFE_PIS_CST_InsetoContrib_Des             = 'Operação Isenta da contribuição';
  NFE_PIS_CST_SemIncidContrib               = '08';
  NFE_PIS_CST_SemIncidContrib_Des           = 'Operação Sem Incidência da contribuição';
  NFE_PIS_CST_SuspContrib                   = '09';
  NFE_PIS_CST_SuspContrib_Des               = 'Operação com suspensão da contribuição';
  // Tributação do COFINS
  NFE_COFINS_CST                            = 'TRIBUTACAO_COFINS';
  NFE_COFINS_CST_TribMonofasica             = '04';
  NFE_COFINS_CST_TribMonofasica_Des         = 'Operação Tributável - Tributação Monofásica - (Alíquota Zero)';
  NFE_COFINS_CST_TribAliqZero               = '06';
  NFE_COFINS_CST_TribAliqZero_Des           = 'Operação Tributável - Alíquota Zero';
  NFE_COFINS_CST_InsetoContrib              = '07';
  NFE_COFINS_CST_InsetoContrib_Des          = 'Operação Isenta da contribuição';
  NFE_COFINS_CST_SemIncidContrib            = '08';
  NFE_COFINS_CST_SemIncidContrib_Des        = 'Operação Sem Incidência da contribuição';
  NFE_COFINS_CST_SuspContrib                = '09';
  NFE_COFINS_CST_SuspContrib_Des            = 'Operação com suspensão da contribuição';
  // Tributação do ICMS
  NFE_ICMS_CST                              = 'TRIBUTACAO_ICMS';
  NFE_ICMS_CST_Tributado                    = '00';
  NFE_ICMS_CST_Tributado_Des                = 'Tributada integralmente';
  // Origem da mercadoria
  NFE_ICMS_Orig                             = 'ORIGEM_MERCADORIA';
  NFE_ICMS_Orig_Nacional                    = '0';
  NFE_ICMS_Orig_Nacional_Des                = 'Nacional';
  NFE_ICMS_Orig_EstrangImpDir               = '1';
  NFE_ICMS_Orig_EstrangImpDir_Des           = 'Estrangeira - Importação direta';
  NFE_ICMS_Orig_EstrangMercadoInt           = '2';
  NFE_ICMS_Orig_EstrangMercadoInt_Des       = 'Adquirida no mercado interno';
  // Forma de processo
  NFE_procEmi                               = 'FORMA_PROCESSO';
  NFE_procEmi_ComAplicativo                 = '0';
  NFE_procEmi_ComAplicativo_Des             = 'Com aplicativo do contribuinte';
  NFE_procEmi_Avulsa_Fisco                  = '1';
  NFE_procEmi_Avulsa_Fisco_Des              = 'Avulsa pelo Fisco';
  NFE_procEmi_Avulsa_Contribuinte_Fisco     = '2';
  NFE_procEmi_Avulsa_Contribuinte_Fisco_Des = 'Avulsa, pelo contribuinte com seu certificado digital, através do site do Fisco';
  NFE_procEmi_Contribuinte_Fisco            = '3';
  NFE_procEmi_Contribuinte_Fisco_Des        = 'Pelo contribuinte com aplicativo fornecido pelo Fisco';
  // Finalidade NFE
  NFE_finNFe                                = 'FINALIDADE_NFE';
  NFE_finNFe_Normal                         = '1';
  NFE_finNFe_Normal_Des                     = 'Normal';
  NFE_finNFe_Complementar                   = '2';
  NFE_finNFe_Complementar_Des               = 'Complementar';
  NFE_finNFe_Ajuste                         = '3';
  NFE_finNFe_Ajuste_Des                     = 'Ajuste';
  // Tipo de operacao
  NFE_tpNF                                  = 'TIPO_OPERACAO';
  NFE_tpNF_Entrada                          = '0';
  NFE_tpNF_Entrada_Des                      = 'Entrada';
  NFE_tpNF_Saida                            = '1';
  NFE_tpNF_Saida_Des                        = 'Saída';
  // Forma de emissao
  NFE_tpEmis                                = 'FORMA_EMISSAO';
  NFE_tpEmis_Normal                         = '1';
  NFE_tpEmis_Normal_Des                     = 'Normal';
  NFE_tpEmis_ContFS                         = '2';
  NFE_tpEmis_ContFS_Des                     = 'Contingência FS';
  NFE_tpEmis_ContSCAN                       = '3';
  NFE_tpEmis_ContSCAN_Des                   = 'Contingência SCAN';
  NFE_tpEmis_ContDPEC                       = '4';
  NFE_tpEmis_ContDPEC_Des                   = 'Contingência DEPC';
  NFE_tpEmis_ContFSDA                       = '5';
  NFE_tpEmis_ContFSDA_Des                   = 'Contingência FSDA';
  //Modalidade frete
  NFE_modFrete                              = 'MODALIDADE_FRETE';
  NFE_modFrete_Emitente                     = '0';
  NFE_modFrete_Emitente_Des                 = 'Por conta do emitente';
  NFE_modFrete_Dest                         = '1';
  NFE_modFrete_Dest_Des                     = 'Por conta do destinatário';
  //pais do destinatario
  NFE_enderDest_Pais                        = 'PAIS_DESTINATARIO';
  //Endereco do destinatario
  NFE_enderDest_Endereco                    = 'ENDERECO_DESTINATARIO';
  //municipio do destinatario
  NFE_enderDest_Municipio                   = 'MUNICIPIO_DESTINATARIO';
  //pais do emitente
  NFE_enderEmit_Pais                        = 'PAIS_EMITENTE';
  //Endereco do emitente
  NFE_enderEmit_Endereco                    = 'ENDERECO_EMITENTE';
  //municipio do emitente
  NFE_enderEmit_Municipio                   = 'MUNICIPIO_EMITENTE';
  //cnpj do destinatario
  NFE_dest_CNPJ                             = 'CNPJ_DESTINATARIO';
  //cnpj do emitente
  NFE_emit_CNPJ                             = 'CNPJ_EMITENTE';
  //cnpj do transportador
  NFE_transp_CNPJCPF                        = 'CNPJ_TRANSPORTADOR';
  //data emissao
  NFE_dEmi                                  = 'DATA_EMISSAO';
  //data vencimento
  NFE_dVenc                                 = 'DATA_VENCIMENTO';







var
  vrgIBoxFmt       : string  = ',0.00';
  vrgIBoxDecs      : Integer = 2;
  vrgIBOProcErro   : boolean = false;
  vrgRetornoTrends : string;

  { Flag para identificar se a Empresa usa atributo de produto para especificar código de baixa do estoque
    Exemplo: vende PICANHA mas da saida no estoque de DIANTEIRO
    Ver: FGStq.RetCodEstoque }
  vrgUsaCodEstoque : boolean = false;



type
  TStatusAcesso = record
    staStatus, staIncluir, staAlterar, staExcluir: boolean;
  end;

  TNumeroClas = 1..3;
  TNumeroEnd  = 1..2;



procedure ChamaForm(      FRPai               : TForm;
                    const FR                  ,
                          pFRCaption          : string;
                          pPosicao            : Integer;
                    const pFormModal          : Boolean = False;
                    const pMultiplasInstancias: Boolean = False;
                    const pChecarPermissao    : Boolean = True;
                    const pMostraForm         : Boolean = True);

function AbreTab(PTabela: TDataSet; const AIndexFieldNames: string = ''): boolean;

procedure CriaItemPopupMenu(pControl: TWinControl);

procedure SetaTopMost(const AHWnd: HWND);

procedure CancelaTopMost(const AHWnd: HWND);

procedure FocalizeGrid(GR: TcxGrid; ATecla: Word);

procedure GeraAtalhoMenu;

procedure RetFormsAtivos(Lista: TStrings; vNome: TStrings = nil);

function ContaFormsAtivos: Integer;

procedure ExecFormClose(FR: TForm = nil);

procedure GravaCacheStr(const PChave: string; PValor: string);
procedure GravaCacheInt(const PChave: string; PValor: Integer);
procedure GravaCacheBln(const PChave: string; PValor: boolean);
procedure GravaCacheNum(const PChave: string; PValor: double);

function LeCacheStr(const PChave: string): string; overload;
function LeCacheStr(const PChave, ANovoValor: string): string; overload;
function LeCacheInt(const PChave: string; PDef: Integer = 0): Integer;
function LeCacheBln(const PChave: string; PDef: boolean = false): boolean;
function LeCacheNum(const PChave: string; PDef: double = 0): double;

function ChamaInputBox(const PTipo: Smallint; const pFRCaption, PTexto: string;
  var PValor: string; const UpperCase: boolean = false): boolean;

function RetEstabeAtivo: string;
function RetEstabeAtivoNome: string;
function RetEstabeAtivoRazao: string;
function RetEstabeAtivoEndereco: string;
function RetEstabeAtivoNumero: string;
function RetEstabeAtivoBairro: string;
function RetEstabeAtivoCidade: string;
function RetEstabeAtivoEstado: string;
function RetEstabeAtivoCnpj: string;
function RetEstabeAtivoInscEst: string;
function RetEstabeAtivoInscMun: string;
function RetEstabeAtivoCep: string;
function RetEstabeAtivoFone: string;
function RetEstabeAtivoFax: string;
function RetEstabeAtivoEmail: string;
function RetEstabeAtivoCidadeIBGE: string;
function RetEstabeAtivoSite: string;

function RetLicenca: string;

function RetAmxEstabe(const PEstabe: string): string;

function RetUsuAtivo: string;
function RetUsuOperador: string;
function RetUsuNomeOperador: string;
function RetUsuCaixa: string;
function RetUsuEcfCaixa: Integer;
function RetUsuNomeEcfCaixa: string;
function RetUsuNomeCaixa: string;
function RetUsuPortaCaixa: Integer;
function RetUsuPdvCaixa: string;
function RetUsuAmxCaixa: string;
function RetUsuGavetaCaixa: string;
function RetUsuVerRecadosOnline: string;
function RetUsuLiberaCartaoMagnetico: string;
function VerOperadorInformado: Boolean;
function RetUsuEstabe: string;

//    Rertorna o diretório para criação de arquivos temporários
//    Resultado: '<unidade>:\<dir_da_aplicação>\sistema\<dir_especificado>\'
function SetaLocalIni: string;
function SetaLocalRelatorio: string;
function SetaLocalDownloads: string;
function SetaLocalEcf: string;
function SetaLocalNfe: string;

function RetDM(ADataModule: string): TDataModule;
function RetDMComp(ADataModule, PCtr: string): TComponent;

function RetFormCaption(FR: string): string;

function RetArqCfgLocal: string;

function VerModoLogin: boolean;

function VerModoDebug: boolean;

function RetCaixaLocal: string;

function ChamaCaxMov(FR: TForm; const pCliente, ANomeCli, ACpfCpnf, ACaixa, AData, AOrg, AIDVen, ADesHis, ACupomTef: string;
  const PValor: Extended; TR: TIB_Transaction = nil): Boolean;

function ChamaRecMov(FR: TForm; const pCliente, PNome, AIdentidade, ACpf, ACaixa, AData, AIDVen, AOrg, ACupomTef: string;
                     const PValor: Extended; const AFatura, ATipoVenda: string; const ASugereCndPgt: string = '';
                     TR: TIB_Transaction = nil): Boolean;

procedure ChamaAtributos(FR: TForm; const pFRCaption, TabelaAtrib, Codigo, Descricao: string);

procedure ChamaCnsSis(      FR                      : TForm;
                      const PTabela,
                            PCampoChave             : string;
                      const PIndiceLocaliza         : SmallInt;
                            PCampos                 : array of string;
                      const PIdentificadorRetorno,
                            PCaptionForm            : string;
                      const PFiltro                 : string = '';
                      const PIndiceColOrdena        : Integer = 0);

procedure ChamaCnsCep(FR: TForm; const AEndereco, ACEP, ACidade: string; const PIdentificadorRetorno: string = '');
procedure ChamaCnsComCota(FR: TForm);
procedure ChamaCnsProStq(FR: TForm; const ACodPro: string);
procedure ChamaGerPreco(FR: TForm; const ACodPro: string);
procedure ChamaCnsVenCota(FR: TForm);
procedure ChamaCnsPro(FR: TForm; const AProduto: string; const pEditar: Boolean = False;
                      const CarregaUltimoLocalizado: Boolean = False; const BuscaAdicional: string = '');
procedure ChamaCnsCli(FR: TForm; const pCliente: string = ''; const PIdentificadorRetorno: string = ''; const pMultiplasInstancias: Boolean = False);
procedure ChamaCnsFor(FR: TForm; const AFornecedor: string; const PIdentificadorRetorno: string = '');
procedure ChamaCnsStq(FR: TForm; const AProduto: string);
procedure ChamaCnsVenPed(FR: TForm; const pNumero: string; const pEditar: Boolean = False);
procedure ChamaCnsRec(FR: TForm; const AContrato: string; const pEditar: Boolean = False);
procedure ChamaCnsPag(FR: TForm; const AFor, AFatura, ADupl: string; const pEditar: Boolean = False);

procedure ChamaCnsVen(FR: TForm; const pDataIni, pDataFin, pNumero, pSerie, pCliente: string; const pEditar: Boolean = False; const pBloqueio: Boolean = False); overload;
procedure ChamaCnsVen(FR: TForm; const pID: string; const pNumero: string = ''; const pSerie: string = ''; const pEditar: Boolean = False; const pBloqueio: Boolean = False); overload;

procedure ChamaCnsFlx(FR: TForm; const pID: string; const pEditar: Boolean = False);
procedure ChamaCnsCom(FR: TForm; const pID: Int64; const pEditar: Boolean = False);
procedure ChamaCnsNotaDev(FR: TForm; const pID: Int64);
procedure ChamaCnsCliOco(FR: TForm; const pCliente: string);
procedure ChamaCnsForOco(FR: TForm; const AFor: string);
procedure ChamaCnsContato(FR: TForm; const pCliente: string);
procedure ChamaCnsTroca(FR: TForm; const pCliente, PNome, pDataIni, pDataFin, pNumero: string; const ALocaliza: Boolean = False);
procedure ChamaCnsComPed(FR: TForm; const pNumero: string; const pEditar: Boolean = False);
procedure ChamaCnsValePresente(FR: TForm; const PClienteVale, PIdFlx: string);
procedure ChamaCnsCidIbge(FR: TForm);

procedure ChamaOcorrenciaCli(FR: TForm; const pID: Int64; const PCodCliente, PNome, PData, PHora, PUsuario: string); overload;
procedure ChamaOcorrenciaCli(FR: TForm; const pID: Int64); overload;

procedure ChamaOcorrenciaFor(FR: TForm; const pID: Int64; const PCodFor, PNome, PData, PHora, PUsuario: string); overload;
procedure ChamaOcorrenciaFor(FR: TForm; const pID: Int64); overload;

function PrimeiroCodigo(const PTabela: string; const PCampo: string = 'CODIGO';
  const ACondicao: string = ''): string;

function ProximoCodigo(const PTabela: string; const PCampo: string = 'CODIGO';
  TR: TIB_Transaction = nil; ACast: boolean = false): string;
function ProximoID(const PTabela: string; const PCampo: string;
  TR: TIB_Transaction = nil): Int64;

function Lookup(const PTab, PCampoRslt: string; const PCampos: array of string;
  const PValores: array of const): string; overload;
function Lookup(const PTab, PCampoRslt: string; const PCampo, PValor: string): string; overload;
function Lookup(const PTab, PCampoRslt: string; const PCampo: string; const PValor: integer): string; overload;

function LocCampo(PQuery: TIBOQuery; const PTab, PCampoRslt: string;
  const PCampos: array of string; const PValores: array of const): string; overload;
function LocCampo(PQuery: TIBOQuery; const PTab, PCampoRslt: string;
  const PCampo, PValor: string): string; overload;
function LocCampo(PQuery: TIBOQuery; const PTab, PCampoRslt: string;
  const PCampo: string; const PValor: integer): string; overload;

function RegistroExiste(const PTab, PCampo, PValor: string): boolean;

function FindKey(PQuery: TIBOQuery; const AParams: string; const PValores: array of const): boolean;
function FindKeyLivre(PQuery: TIBOQuery; const PTabela, PCampos: string; const PValores: array of const): boolean;

function ExeStoredProc(PQuery: TIBOQuery; const AParams: string; const PValores: array of const): boolean;

function CarregaLista(const PTabela, ACondicao: string;
  const PCampos: string = 'CODIGO,DESCRICAO'; const AOrderBy: string = 'CODIGO'): TStrings; overload;
function CarregaLista(DS: TDataSet; const PCampos: string): TStrings; overload;

function CarregaStringListFmt(const PTab: string; const AFields: string = ' ';
  pFormat: string = '%-6s   %-30s '; ASep: string = '-'): TStrings;

procedure CarregaListView(LV: TListView; const PTabela, ACondicao: string;
  const PCampos: string = 'CODIGO,DESCRICAO'; const AOrderBy: string = 'CODIGO');

function CriptografaSenhaUsu(const APwd: string): string;
function RevelaSenhaUsu(const APwd: string): string;
function RetSenhaMontada: string;

procedure GravaLogErrosDB(const PMensagem, PForm, PSql: string);

procedure GravaTabLog(FR: TForm; const PMensagem: string; const POrigem: string='');

procedure SetaVisual(FR: TForm);

function RetDataCompila: string;

function RetMemFis: string;

function RetCopyright: string;

function RetVersaoSis(const SomenteBuild: Boolean): string;

function LeQuery(pCons: integer): string;
procedure GravaLogSQL(const PTexto: string; const ForcePost: Boolean = False); overload;
procedure GravaLogSQL(PQuery: TIBOQuery); overload;

function  RetDisplayImp: string;

procedure SetaDisplayImp;

procedure RetTabelasAtivas(Lista: TStrings);

function RetArqScripts: string;

procedure CancelaAcesso(FR: TWinControl; AUsuario, AOpcao: string);

procedure CancelaAcessoMenu(const PUsuario: string);

function RetStatusAcesso(FR: string): TStatusAcesso;

function VerAcessoGravar(FR: string; Acao: string; AMsg: boolean = true): boolean; overload;
function VerAcessoGravar(FR: TForm; Acao: string; AMsg: boolean = true): boolean; overload;

procedure GetDDDFone(const AOrigem: string; AEditDDD, AEditFone: TEdit);

function RetDDDFone(AEditDDD, AEditFone : TEdit): string;

function VerFoco(FR: TForm; Sender: TObject; AField: TWinControl): boolean;

function RetAtvFormJvBitBtn(PCtr: string): TJvBitBtn;
function RetFormJvBitBtn(FR, PCtr: string): TJvBitBtn;

function RetAtvFormJvDateEdit(PCtr: string): TJvDateEdit;
function RetFormJvDateEdit(FR, PCtr: string): TJvDateEdit;

function RetAtvFormJvComboEdit(PCtr: string): TJvComboEdit;
function RetFormJvComboEdit(FR, PCtr: string): TJvComboEdit;

function RetAtvFormCxSpinEdit(PCtr: string): TCxSpinEdit;
function RetFormCxSpinEdit(FR, PCtr: string): TCxSpinEdit;

procedure EnviaEmail(const Destinatario, CopiaOculta, Assunto, Texto: string; const Anexos: array of string; const EnvioDireto: Boolean = False);

procedure SetaImp;
procedure SetaNovaImp(const PNome: string; const APadrao: integer);
procedure VoltaImpDef(const APadrao: integer);
function RetImpDef: integer;
function RetNomeImpDef: string;

procedure SetaPapelCustom(ALargura, AAltura: Integer);

procedure ChamaEditorTxt1(const ALinhas: string; FR: TForm);

function ChamaEditorTxt2(FR: TForm; const FRAltura: Integer; const TextoEnt: string; var TextoSai: string): Boolean;

procedure SetaOpe(const PControlador: TWinControl; const ASinal: string);
procedure SetaOpeINC(const PControlador: TWinControl);
procedure SetaOpeALT(const PControlador: TWinControl);
function VerOpe(const PControlador: TWinControl; const ASinal: string): boolean;
function VerOpeINC(const PControlador: TWinControl): boolean;
function VerOpeALT(const PControlador: TWinControl): boolean;

function PreparaAnoMes(const Data_Ou_MesAno: string): string;
function PreparaMesAno(const Data_Ou_AnoMes: string): string;

function VerificaMesAno(AEdit: TWinControl; const AMesAno: string): boolean;

function RetColunaExcel(const AIndex: Integer): string;

procedure LimpaComps(PContainer: TWinControl); overload;

function VerificaComps(PContainer: TWinControl): boolean;

function RetEstabeRel: string;

function LeConfigEstabe(const PEstabe: string): string;

function LeIniBool(const PTexto, pID: string; const PDefault: boolean = false): boolean;
function LeIniStr(const PTexto, pID: string; const PDefault: string = ''): string;
function LeIniInt(const PTexto, pID: string; const PDefault: integer = 0): integer;
function LeIniNum(const PTexto, pID: string; const PDefault: double = 0): double;
function LeIniStrEx(const PTexto, PSecao, pID: string; const PDefault: string = ''): string;
function LeIniIntEx(const PTexto, PSecao, pID: string; const PDefault: integer = 0): integer;

function LeConfigEstabeStr(const pID: string; const PDefault: string = ''): string;
function LeConfigEstabeBool(const pID: string; const PDefault: boolean = false): boolean;
function LeConfigEstabeInt(const pID: string; const PDefault: integer = 0): integer;
function LeConfigEstabeNum(const pID: string; const PDefault: double = 0): double;
procedure GravaConfigEstabeStr(const pID, PValor: string; TR: TIB_Transaction = nil);

procedure RecarregaConfigEstabe;

function LeConfigLocalStr(const pID: string; const PDefault: string = ''): string;
function LeConfigLocalBool(const pID: string; const PDefault: boolean = false): boolean;
function LeConfigLocalInt(const pID: string; const PDefault: integer = 0): integer;
function LeConfigLocalNum(const pID: string; const PDefault: double = 0): double;
procedure GravaConfigLocalStr(const pID, PValor: string);
procedure GravaConfigLocalBool(const pID: string; const PValor: boolean);
procedure GravaConfigLocalInt(const pID: string; const PValor: integer);
procedure GravaConfigLocalNum(const pID: string; const PValor: double);

procedure ChamaFotoProduto(FR: TForm; const PCodigo, PDescricao: string);

function LeConfigUsu(const AUsuario: string): string;
function LeConfigUsuStr(const pID: string; const PDefault: string = ''): string;
function LeConfigUsuBool(const pID: string; const PDefault: boolean): boolean;
function LeConfigUsuInt(const pID: string; const PDefault: integer = 0): integer;
function LeConfigUsuNum(const pID: string; const PDefault: double = 0): double;
procedure GravaConfigUsuStr(const pID, PValor: string; TR: TIB_Transaction = nil);

function LeConfigNFStr(const pID: string; const PDefault: string = ''): string;
function LeConfigNFBool(const pID: string; const PDefault: boolean = false): boolean;
function LeConfigNFInt(const pID: string; const PDefault: integer = 0): integer;
function LeConfigNFNum(const pID: string; const PDefault: double = 0): double;
procedure GravaConfigNFStr(const PEstabe, PNome, pID, PValor: string; TR: TIB_Transaction = nil);

function LeParEstabeStr(const PEstabe, pID: string; const PDefault: string = ''): string;
function LeParEstabeBool(const PEstabe, pID: string; const PDefault: boolean = false): boolean;
function LeParEstabeInt(const PEstabe, pID: string; const PDefault: integer = 0): integer;
function LeParEstabeNum(const PEstabe, pID: string; const PDefault: double = 0): double;
procedure GravaParEstabeStr(const PEstabe, pID, PValor: string; TR: TIB_Transaction = nil);

function LeConfigVenPedido: string;
function LeConfigVenPedidoStr(const pID: string; const PDefault: string = ''): string;
function LeConfigVenPedidoBool(const pID: string; const PDefault: boolean): boolean;
function LeConfigVenPedidoInt(const pID: string; const PDefault: integer = 0): integer;
function LeConfigVenPedidoNum(const pID: string; const PDefault: double = 0): double;
procedure GravaConfigVenPedidoStr(const pID, PValor: string; TR: TIB_Transaction = nil);

function LeConfigComPedido: string;
function LeConfigComPedidoStr(const pID: string; const PDefault: string = ''): string;
function LeConfigComPedidoBool(const pID: string; const PDefault: boolean): boolean;
function LeConfigComPedidoInt(const pID: string; const PDefault: integer = 0): integer;
function LeConfigComPedidoNum(const pID: string; const PDefault: double = 0): double;
procedure GravaConfigComPedidoStr(const pID, PValor: string; TR: TIB_Transaction = nil);

procedure ChamaBuscaCL(CL: TCheckListBox; const PTipo: SmallInt = 0);
procedure ChamaCnsSat(FR: TForm);

procedure ChamaBuscaLV(LV: TListView; const PTipo: SmallInt = 0);

procedure FocalizeListView(LV: TListView; const AIndex: integer);

function GeraSQLINCL(CL: TCheckListBox; const ComAspas: boolean = false): string;

function GeraSQLINLV(LV: TListView; const ComAspas: boolean = false): string;

function GeraSQLINCheckComboBox(CX: TcxCheckComboBox; const ComAspas, ExtraiCodigo: Boolean; const TamCod: Integer = -1; const Separador: string = ' '): string; overload;
function GeraSQLINCheckComboBox(CX: TcxCheckComboBox): string; overload;

function verCheckComboBox(CX: TcxCheckComboBox; Index: Integer): Boolean; overload;
function verCheckComboBox(CX: TcxCheckComboBox): Boolean; overload;

procedure setaCheckComboBox(CX: TcxCheckComboBox; Index: Integer; Status: Boolean); overload;
procedure setaCheckComboBox(CX: TcxCheckComboBox; Status: Boolean); overload;

function GeraSICN010(var   aStringConexao: string;
                     const aProduto      ,
                           aNSU          ,
                           aCPFCNPJ      ,
                           aBanco        ,
                           aAgencia      ,
                           aConta        ,
                           aContaDv      ,
                           aCheque       ,
                           aChequeDv     ,
                           aChequeQtd    ,
                           aDDDFone      ,
                           aFone         : string) : Boolean;

procedure CarregaCheckComboBox(CX: TcxCheckComboBox; DS: TDataSet; const PCampos: string = 'CODIGO,DESCRICAO'; const PSeparador: string = ' ');

function MsgMono(const AMsg: string; const Msg_ID: TIconeMsg; const ATamFonte: Integer = 10;
  const ANegrito: boolean = false): Integer;

procedure OrdenaGrid(GR: TDBGrid; AColuna: TColumn; const PintarColuna: boolean = true); overload;
procedure OrdenaGrid(AColuna: TColumn; const PintarColuna: boolean = true); overload;

procedure SetaPropColOrdena(AColuna: TColumn);

procedure SetaQueryOrder(PQuery: TIBOQuery);

function QualificaSQL(const ASQL: string): string;

function DataSql(ADataStr: string): string; overload;
function DataSql(AData: TDateTime): string; overload;

function RetAnoMesSQL(const Data_Ou_MesAno: string): string;

function FormataCHR(const ACodigo: string): string;
function MontaBarra25(AUsaTab: boolean; ABan, AMoe, AVen, AVal, AVar: string;
  var ABar: string): string;

function Modulo11Bradesco(const S: string): string;

procedure AtivaAnotacao(const FR: string);

procedure SetaJFS(FR: TForm; AJfs: TJvFormStorage; const Sufixo: string = '');

function RetValorGenerator(const AGenerator: string): integer;

function TabelaExiste(const Nome: string): Boolean;

function LeHoraServSQL: string;

function LeDataHoraServSQL: string;

function LeDataServSQL: string;
function DataLocal: TDateTime;
function DataStrLocal: string;

procedure StrConvIniFile(var AIniFile: TINIFile; AStrings: TStrings; AStr: string);

procedure GeraIniFile(ACriar: boolean; var AIniFile : TINIFile);

function IniFileConvStr(var AIniFile: TINIFile; AStringList: TStrings): string;

function RetProClas(const ANumeroClas: TNumeroClas; const PEstabe: string = ''): string;

function VerProClas(const ANumeroClas: TNumeroClas; const PEstabe: string = ''): boolean;

function RetProEnd(const ANumeroEnd: TNumeroEnd): string;

function VerProEnd(const ANumeroEnd: TNumeroEnd): boolean;

function RetMaskProEnd(const ANumeroEnd: TNumeroEnd): string;

function TemProEnd: Boolean;

function UsuAtivoExiste: boolean;

procedure TabStopControl(pObj: TWinControl; pHabilita: boolean);

procedure LocalizaCombo(pControl: TWinControl; pEdit: TEdit; pParcial, pSemEspaco: boolean;
  pStart: string = '');

procedure ChamaCfgRelFast(FR: TForm; DS: TDataSet; const Modulo: string; const Nome: string = '');

procedure ImprimeCfgRelFast(FR: TForm; DS: TDataSet; const Modulo, Nome: string);

procedure SetaCtrSel(DSOrg, DSDst: TDataSet;
  AGridOrg, AGridDst: TDBGrid; const AbreOrg: boolean = true); overload;

procedure ExeSel(const Fun: Char; AGridOrg, AGridDst: TDBGrid;
  const DstChave: string = 'CODIGO'); overload;

procedure ExeSel_ProEndereco(const Fun: Char; AGridOrg, AGridDst: TcxGridDBTableView;
  const DstChave: string = 'ENDERECO');

procedure SetaCtrSel(DSOrg, DSDst: TDataSet;
  AGridOrg, AGridDst: TcxGridDBTableView; const AbreOrg: boolean = true); overload;

procedure ExeSel(const Fun: Char; AGridOrg, AGridDst: TcxGridDBTableView;
  const DstChave: string = 'CODIGO'); overload;

procedure LocSel(const ALocalizar: string; GR: TDBGrid); overload;

procedure LocSel(const ALocalizar: string; GR: TcxGridDBTableView; const SubTipo: string=''); overload;

procedure SetaClasSel(TC: TTabControl); overload;
procedure SetaClasSel(TC: TcxTabControl); overload;
procedure SetaEndSel(TC: TcxTabControl); overload;

function RetSQLCliente(const AVerFiltro: Boolean; const AExtraSelect, AOrdenacao: string): string;

procedure PreparaFiltroCli(PQuery: TIBOQuery);

function RetSQLFornecedor(const AVerFiltro: Boolean; const AOrdenacao: string): string;

procedure PreparaFiltroFor(PQuery: TIBOQuery);

procedure GravaInfoFiltro(FR: TForm; AContainers: array of TWinControl;
  ACdsSel: array of TClientDataSet);
procedure LeInfoFiltro(FR: TForm; AContainers: array of TWinControl;
  ACdsSel: array of TClientDataSet);
procedure RemoveInfoFiltro(const AFormSel: string);

procedure CriaIB_Cursor(var ACursor: TIB_Cursor; const ASQL: string);

procedure CriaIBOQuery(var PQuery: TIBOQuery; const ASQL: string = '');

function MontaSQLInsert(const PTabela: string): string;

function LocCotacao(PQuery: TIBOQuery; const AMoeda, AData: string;
  const AVerMoeda: boolean): double; overload
function LocCotacao(const AMoeda: string; const AData: string = ''): double; overload;

function MoedaEstrangeira(const AMoeda: string): boolean; overload;
function MoedaEstrangeira(PQuery: TIBOQuery; const AMoeda: string): boolean; overload;

function VerInscricaoEst(const AInscricao, AUF: string): boolean;

function RetDataNull: string;

function VerVendedor(const AModulo, ACodigo: string; ALabel: TLabel; const PMsg: Boolean = True): Boolean;

function VerLocate(DS: TDataSet; const ALocaliza: string; const PChave: string = 'CODIGO'): boolean;

procedure SetaPapelParede;

procedure ZeraValorDataSet(DS: TDataSet);

procedure ChamaGridPreview(arq: string);

procedure ExportaQuantumGrid(cxGR: TcxGrid; const PTipo: string;
                             const L1C1: string = ''; const L1C2: string = '';
                             const L2C1: string = ''; const L2C2: string = '';
                             const L3C1: string = ''; const L3C2: string = '';
                             const FormatoFloats: string = '';
                             const MostraCabecalho: Boolean = True);

function ContratoTemPag(const ID: Int64): Boolean;

procedure ChamaCadastroCli(FR: TForm; const ACodCli: string);
procedure ChamaCadastroFor(FR: TForm; const ACodFor: string);

procedure GravaCliDefault(const ACli: string);

function LeCliDefault: string;

procedure GravaForDefault(const AFor: string);

function LeForDefault: string;

procedure DeletaFluxoRec(const ID: Int64; TR: TIB_Transaction = nil);

function DeletaFluxoRecPag(const ID: Int64; TR: TIB_Transaction = nil): Boolean;

procedure DeletaFluxoVen(const ID: Int64; TR: TIB_Transaction = nil);

procedure CancelaFluxoRec(const ID: Int64; const ASit: string; TR: TIB_Transaction = nil);

procedure CancelaFluxoRecPag(const ID: Int64; const ASit: string; TR: TIB_Transaction = nil);

procedure CancelaFluxoVen(const ID: Int64; const ASit: string; TR: TIB_Transaction = nil);

procedure ChamaDica(FR: TForm; const PTitulo, PTexto: string);

procedure ChamaSisAnota(TFR: string);

function RetCabHTMLQuantumGrid: string;

procedure AtuInfoUsuCaixa;

function VerVinculosCaixa: Boolean;

function ProcuraValorDS(DS: TDataSet; const PCampo: string; const PValor: Variant): Boolean;

procedure RefreshFR;

function RetValLookupComboBox(LCB: TcxLookupComboBox; const PCampo: string): Variant;

function RetStrLookupComboBox(LCB: TcxLookupComboBox; const PCampo: string = 'CODIGO'): string;

function RetDescLookupComboBox(LCB: TcxLookupComboBox; const PCampo: string = 'DESCRICAO'): string;

function ChecaLookupComboBox(LCB: TcxLookupComboBox; Lbl: TcxLabel): Boolean;

procedure SetaLookupComboBox(LCB: TcxLookupComboBox; const PValor: string; PIndexDefault: Integer);

function RetUltPrc(const ACodCliente, ACodProduto: string): Extended;

function VerUsuEstabe(const PEstabe: string): Boolean;

procedure ChamaMsg(const aMsg: string; const aTipo:Integer = 0);

procedure ChamaMsgDt(const AMsg, ADetalhes: string);

procedure ChamaMsgECF(const aMsg: string);

function ChamaMsgCf(const AMsg: string; const ADefBtn: Smallint = 1): Integer;

procedure ImprimeFichaPag(FR: TForm; const APerIni, APerFin: TDate; const PClientes: string);

procedure ImprimeFichaFin(FR: TForm; const PDtBase: TDate; const PClientes: string;
  const AAtrIni, AAtrFin: Integer);

procedure ImprimeCartaCob(FR: TForm; const PDtBase: TDate; const PClientes: string;
  const AAtrIni, AAtrFin: Integer);


procedure SetaFrxExports(FR: TForm; const ANomeRel: string = '');

function TCPServerAtivo : Boolean;

procedure SetaCamposFrx(DS: TDataSet; FxDS: TfrxDBDataset);

procedure ExecSQL(const SQL: string; TR: TIB_Transaction = nil);

function RetMemoView(RPT: TfrxReport; const Obj: string): TfrxMemoView;

function RetLineView(RPT: TfrxReport; const Obj: string): TfrxLineView;

function RetGroupHeader(RPT: TfrxReport; const Obj: string): TfrxGroupHeader;

function RetGroupFooter(RPT: TfrxReport; const Obj: string): TfrxGroupFooter;

function ProximoVago(const Tabela: string; var Resultado: Int64; const Campo: string = 'CODIGO'): Boolean;

procedure AtuDataEstat(const Tipo: string; TR: TIB_Transaction = nil);

function LeDataEstat(const Tipo: string): string;

procedure TravaMouseTeclado;
procedure DestravaMouseTeclado;

function RetFundoPanel: TImage;

function RetImGeral: TcxImage;

function RetNomeEcf(const PCodigoECF: Integer): string;

procedure AtuPanelCaixaMenu(const PDataHora: string);

function RetMascaraPreco: string;

function RetDecsPreco: Integer;

procedure GravaUsuRegistroStr(const PChave, PNome, PValor: string);
procedure GravaUsuRegistroBool(const PChave, PNome: string; const PValor: Boolean);
procedure GravaUsuRegistroInt(const PChave, PNome: string; const PValor: Integer);
procedure GravaUsuRegistroNum(const PChave, PNome: string; const PValor: Extended);

function LeUsuRegistroStr(const PChave, PNome: string; const PDefault: string = ''): string;
function LeUsuRegistroBool(const PChave, PNome: string; const PDefault: Boolean = False): Boolean;
function LeUsuRegistroInt(const PChave, PNome: string; const PDefault: Integer = 0): Integer;
function LeUsuRegistroNum(const PChave, PNome: string; const PDefault: Extended = 0): Extended;
procedure LeUsuRegistroCds(const PChave, PNome: string; CDS: TClientDataSet);

procedure DeletaUsuRegistro(const PChave, PNome: string);

procedure ChamaGrafico(FR: TForm; DS: TDataSet; const PTitulo: TStrings; const PCampoX, PCampoY, PFmt: string);

procedure ChamaGraficoEmpilhado(FR: TForm; DS: TDataSet; const PTitulo: TStrings; const PCampoX: string;
                                const PCampoY, PLegenda: array of string; const PFmt: string);

procedure ReiniciarSistema;

function RetVersaoReqDB: string;

function RetVersaoAtuDB(const StringErro: Boolean = True): string;

procedure LimpaCacheConsultas;

procedure RealinhaBtns(P: TPanel);

procedure ProcessaSelConsulta(FR: TForm; const ManterAberto: Boolean = True);

function RetDataValidadeDemo: string;

function ClasseFormPermitida(const PClasse: string): Boolean;

function verTabAtributo(ATab_Atributo : string) : Boolean;

function ExisteAtributo(Tipo : string) : Boolean;

function MontaEndereco(const PLogradouro, PNumero, PComplemento: string): string;

function RetInfoPro(const aCodProduto: string; out aHeader, aTexto: string): string;
procedure GravaDicaProduto(aEdit: TCustomEdit; const aCodProduto: string);
procedure AtivaBalaoDicaProduto(aEdit: TCustomEdit);
procedure FechaBalaoDicaProduto;
procedure AcessaDicaProduto;

function CodIBGEParaUF(const aCodigo: string; const aUFDefault: string = 'RS'): string;
function UFParaCodIBGE(const aUF: string; const aCodigoDefault: string = '43'): string;

procedure ChamaBalaoDica(CT: TControl; const PTitulo, PTexto: string);
procedure FechaBalaoDica;

procedure LayoutGridView(GR: TcxGridDBTableView; const TipoAcao: Integer; const Arquivo: string=''); overload;
procedure LayoutGridView(GR: TcxGridDBBandedTableView; const TipoAcao: Integer; const Arquivo: string=''); overload;

procedure SetaPrimeiroValido(E1, E2 : TJvDateEdit);

function MontaNossoNumeroPortador(const PPortador, PNumero: string) : string;

function MontaNossoNumero(const PBanco, PNumero: string; const PAgenciaSicredi: string = ''): string;

function CalculaDV(const PBanco, PNumero: string; PRecalculaBanrisul: Boolean = True): string;

procedure SetaValorGroupFooter(GR: TcxGridDBTableView; const IDX: Integer; const Campo: string; const Valor: Variant);
function LeValorGroupFooter(GR: TcxGridDBTableView; const IDX: Integer; const Campo: string): Variant;

function LeValorFooter(GR: TcxGridDBTableView; DS: TcxDataSummary; const Campo: string): Variant;
procedure SetaValorFooter(GR: TcxGridDBTableView; DS: TcxDataSummary; const Campo: string; const Valor: Variant);

procedure AlteraColunasGridView(GR: TcxGridDBTableView; const Incremento: Integer);

function VerVendedorAusente(const pCodigo: string): Boolean;

procedure ApplyBestFitViewDetalhe(pDataController: TcxCustomDataController; pRecordIndex: Integer);

function RetSQLRAT(const pSelect, pOrderBy: Boolean): string;

function MontaCam(C : TComponent; const Extensao: string = '.ini'): string;

procedure SetaEditPreco(CalcEdit: TJvCalcEdit);

function VerReferenciaEspecial(DS: TDataSet; const pFrete  : Boolean = True;
                                             const pSeguro : Boolean = True;
                                             const pDespesa: Boolean = True): Boolean;

function RetNFe_CodigoDesc(const pValor, pCampo: string): string;

function LocalizaDescricao(const aTabela, aDescricao: string; const aCampoDescricao: string = 'descricao'): Boolean;

function EAD_GeraChave(var pChavePublica, pChavePrivada: string): Boolean;

function EAD_Assina(var pNomeArq, pChavePublica, pChavePrivada: string): Boolean;

procedure CarregaFrxImagem(pReport: TfrxReport; const pPictureView: string; pDataSet: TDataSet; const pCampo, pChaveExtensao: string);

function BuildFileList(const Path: string; const Attr: Integer; const List: TStrings): Boolean;


implementation

uses
  ADMG, Menu, Libera, ADMC, ADMS, ADMP, ADMI, CaxMov, RecMov, FGCli, SisDica, FGLib, EcfGer,
  MsgCf, Msg, RecCobrancaFicha, CfgRelFast, CfgRelFastQ, ProgGer, RecCobrancaCarta, EditorTxt1, EditorTxt2,
  SisGraf, MsgDt, AtributoC01, CliOcorrencia, ForOcorrencia, RecPagFichaQ, RecPagFicha,
  ProImagem, MsgInput, cxTextEdit;


const
  Inicio: string[1] = '<';
  Final: string[1] = '>';
  TabConv: array[0..99] of string[5] =
   ('nnWWn', 'NnwwN', 'nNwwN', 'NNwwn', 'nnWwN',
    'NnWwn', 'nNWwn', 'nnwWN', 'NnwWn', 'nNwWn',
    'wnNNw', 'WnnnW', 'wNnnW', 'WNnnw', 'wnNnW',
    'WnNnw', 'wNNnw', 'wnnNW', 'WnnNw', 'wNnNw',
    'nwNNw', 'NwnnW', 'nWnnW', 'NWnnw', 'nwNnW',
    'NwNnw', 'nWNnw', 'nwnNW', 'NwnNw', 'nWnNw',
    'wwNNn', 'WwnnN', 'wWnnN', 'WWnnn', 'wwNnN',
    'WwNnn', 'wWNnn', 'wwnNN', 'WwnNn', 'wWnNn',
    'nnWNw', 'NnwnW', 'nNwnW', 'NNwnw', 'nnWnW',
    'NnWnw', 'nNWnw', 'nnwNW', 'NnwNw', 'nNwNw',
    'wnWNn', 'WnwnN', 'wNwnN', 'WNwnn', 'wnWnN',
    'WnWnn', 'wNWnn', 'wnwNN', 'WnwNn', 'wNwNn',
    'nwWNn', 'NwwnN', 'nWwnN', 'NWwnn', 'nwWnN',
    'NwWnn', 'nWWnn', 'nwwNN', 'NwwNn', 'nWwNn',
    'nnNWw', 'NnnwW', 'nNnwW', 'NNnww', 'nnNwW',
    'NnNww', 'nNNww', 'nnnWW', 'NnnWw', 'nNnWw',
    'wnNWn', 'WnnwN', 'wNnwN', 'WNnwn', 'wnNwN',
    'WnNwn', 'wNNwn', 'wnnWN', 'WnnWn', 'wNnWn',
    'nwNWn', 'NwnwN', 'nWnwN', 'NWnwn', 'nwNwN',
    'NwNwn', 'nWNwn', 'nwnWN', 'NwnWn', 'nWnWn');

  TabChr: array[0..99] of string =
   ('48', '49', '50', '51', '52', '53', '54', '55', '56', '57',
    '58', '59', '60', '61', '62', '63', '64', '65', '66', '67',
    '68', '69', '70', '71', '72', '73', '74', '75', '76', '77',
    '78', '79', '80', '81', '82', '83', '84', '85', '86', '87',
    '88', '89', '90', '91', '92', '93', '94', '95', '96', '97',
    '192', '193', '194', '195', '196', '197', '198', '199', '200', '201',
    '202', '203', '204', '205', '206', '207', '208', '209', '210', '211',
    '212', '213', '214', '215', '216', '217', '218', '219', '220', '221',
    '222', '223', '224', '225', '226', '227', '228', '229', '230', '231',
    '232', '233', '234', '235', '236', '237', '238', '239', '240', '241');

{------------------------------------------------------------------------------}
//procedure SetaInfoDebug(F: TForm);
//begin
//    if not FMenu.pnDebug.Visible then
//      Exit;
//    FMenu.lbChamaForm.Caption := Format('%s [ %s ]: H %d W %d L %d T %d',
//                                        [F.Name, F.Owner.Name, F.Height, F.Width, F.Left, F.Top]);
//end;

{------------------------------------------------------------------------------}
//procedure GravaLogAcesso(const aForm: string);
//var w : TStrings;
//    ss: string;
//begin
//    try
//       FH.CriaLista(w);
//       try
//          ss := FH.RetDirSis+'Acessos.log';
//          if FileExists(ss) then
//             w.LoadFromFile(ss);
//          w.Add(DateTimeToStr(Now) + ' | ' + FG.RetUsuAtivo + ' | ' + aForm);
//          w.SaveToFile(ss);
//       except
//       end;
//    finally
//           FH.DestroiObj(w);
//    end;
//end;

{------------------------------------------------------------------------------}
procedure ChamaForm(      FRPai              : TForm;
                    const FR                 ,
                          pFRCaption          : string;
                          pPosicao            : Integer;
                    const pFormModal          : Boolean = False;
                    const pMultiplasInstancias: Boolean = False;
                    const pChecarPermissao    : Boolean = True;
                    const pMostraForm        : Boolean = True);
var
    ClasseForm : TFormClass;
    TabNome    ,
    FrmNome    ,
    vCaption   : string;
    vMenuItem  : TMenuItem;
    vForm      : TForm;

    
    { posição 0 = centro
      posição 1 = rodapé do chamador
      posição 2 = mesmo top do form chamador }


    function MostraFrm(vform: string): boolean;
    var
        I: Integer;
    begin
        Result := false;
        if pMultiplasInstancias then
           Exit;
        for I := 0 to Screen.FormCount - 1 do
            if AnsiSameText(Screen.Forms[I].Name, Copy(vForm, 2, 40)) then
            begin
                 if pMostraForm then
                    Screen.Forms[I].Show;
                 Result := true;
            end;
    end;


begin
    FH.SetaCursorAmp;
    try
        FrmNome := Trim(Copy(FR, 2, 100));

        if Pos('_', FrmNome) > 0 then
        begin
            TabNome := FH.CopyPosMax(FrmNome, '_');
            FrmNome := FH.CopyPos(FrmNome, '_');
            if TabNome <> '' then
               FH.GravaRegistry(FG.REG_CHAVE_GERAL, 'Tabela', TabNome);
        end
        else
            FH.GravaRegistry(FG.REG_CHAVE_GERAL, 'Tabela', '');

        if MostraFrm(FrmNome) then
           Exit;
           
        if (not SameText(FR, '_TFCliCadastro')) then
        begin
             if pChecarPermissao then
                if not FG.RetStatusAcesso(FR).staStatus then
                begin
                     FG.ChamaMsgDt('Acesso negado.'#13 +
                                   'Usuário não possui permissão para acessar este programa.',
                                   'Nome do programa: ' + FR);
                     Exit;
                end;
        end;

        FG.LimpaCacheConsultas;

        try
          ClasseForm := TFormClass(FindClass(FrmNome));
          if ClasseForm = nil then
          begin
              FG.ChamaMsgDt('Programa não encontrado.', FrmNome);
              exit;
          end;
        except
          on E: EClassNotFound do
          begin
              FG.ChamaMsgDt('Programa não encontrado.', 'Nome: ' + FrmNome + #13 + 'Mensagem: ' + E.Message);
              exit;
          end
          else
              exit;
        end;

        vCaption := pFRCaption;

        if vCaption = '' then
        begin
            vMenuItem := TMenuItem(Application.MainForm.FindComponent('_' + FrmNome));
            if vMenuItem <> nil then
               vCaption := vMenuItem.Hint
            else
               vCaption := '[]';
        end;

        try
            vForm := ClasseForm.Create(FRPai);
            with vForm do
            begin
                if vCaption = '[]' then
                   Caption := '[' + Caption + ']'
                else
                   Caption := vCaption;

                if not AnsiMatchText(FrmNome, ['TFSisProgresso']) then
                begin
                    if SameText(FrmNome, 'TFRelToolBar') then
                       Top := Top + (GetSystemMetrics(SM_CYCAPTION) - 19)
                end;

                case pPosicao of
                  0, 55: // <<<<----- 55 é exclusivo para a atualização do banco
                    begin
                      // no centro da tela se height < 500

                      if ((Height > 500) and (Height < 600)) then
                        if FMenu.WindowState = wsMaximized then
                          if Screen.Width < 1280 then
                          begin
                            Position := poDesigned;
                            Left     := (Screen.Width div 2) - (Width div 2);
                            Top      := GetSystemMetrics(SM_CYCAPTION) +
                                        FMenu.clbHead.Top +
                                        FMenu.tlbMenu.Height;
                          end;
                    end;
                  1:
                    begin
                      Top  := Trunc((FRPai.Height + FRPai.Top) - Height);
                      Left := FRPai.Left + Trunc((FRPai.Width / 2) - (Width / 2));
                    end;
                  2:
                    begin
                      Position := poDesigned;
                      Top      := FRPai.Top;
                      Left     := FRPai.Left;
                    end;
                end;

                // obrigar ajuste de forms  para o tamanho grande padrão
                if ((Height > 500) and (Height < 600) and (Width > 700)) then
                begin
                    Height := 530;
                    Width  := 799;
                end;

                //FG.SetaInfoDebug(vForm);

                if pFormModal then
                begin
                    FG.AtivaAnotacao(FrmNome);
                    FG.CancelaAcesso(vForm, FG.RetUsuAtivo, FR);
                    FG.CriaItemPopupMenu(vForm);

                    if pMostraForm then
                       ShowModal;
                end
                else
                begin
                    FG.AtivaAnotacao(FrmNome);
                    FG.CancelaAcesso(vForm, FG.RetUsuAtivo, FR);
                    FG.CriaItemPopupMenu(vForm);

                    if (pPosicao <> 3) and (pMostraForm) then
                       Show;

                    if FG.VerModoDebug and (FormStyle = fsStayOnTop) then
                       if (FG.LeCacheStr('$Sis_Stay') <> 'S') and (pPosicao <> 55) then
                          FG.CancelaTopMost(Handle);
                end;
            end;
        except
            on E: Exception do
               FG.ChamaMsgDt('Erro na criação do programa.', E.Message);
        end;
    finally
      FH.SetaCursorDef;
    end;
end;

{------------------------------------------------------------------------------}
procedure CriaItemPopupMenu(pControl: TWinControl);
var
  I, J, P, P1, vTag : Integer;
  vCaption, vNome   : string;
  vItens            : array[0..99] of TMenuItem;
  ppm               : TPopupMenu;
begin
  I  := 0;
  J  := 0;
  P  := 0;
  P1 := 0;

  with pControl do
  begin
    FG.SetaVisual(TForm(pControl));

    // testa se já não existe um popup no form para não criar duplicado
    for I := 0 to ComponentCount - 1 do
      if Components[I] is TPopupMenu then
        if Components[I].Tag <> -5 then // só cria outro se o tag do ppm for -5
          exit;

    ppm := TPopupMenu.Create(pControl);

    ppm.AutoHotKeys := maManual;
    ppm.AutoPopup   := false;

    TForm(pControl).popupMenu := ppm;
    for I := 0 to ComponentCount - 1 do
    begin
      vCaption := '';

      if Components[I] is TJvBitBtn then
        vCaption := TJvBitBtn(Components[I]).Caption
      else
      if Components[I] is TBitBtn then
        vCaption := TBitBtn(Components[I]).Caption
      else
      if Components[I] is TSpeedButton then
        vCaption := TSpeedButton(Components[I]).Caption
      else
      if Components[I] is TButton then
        vCaption := TButton(Components[I]).Caption
      else
      if Components[I] is TJvComboEdit then
        vCaption := TJvComboEdit(Components[I]).ButtonHint;

      if vCaption <> '' then
      begin
        vNome := Components[I].Name;
        vTag  := Components[I].Tag;
        P     := Pos('(', vCaption);
        P1    := Pos(')', vCaption) - P - 1;

        if P > 0 then
        begin
          vItens[J]         := TMenuItem.Create(ppm);
          vItens[J].Caption := Copy(vCaption, 1, P - 1);
          vItens[J].Name    := vNome;
          vItens[J].Tag     := vTag;

          if Components[I] is TJvComboEdit then
          begin
            if Assigned(TJvComboEdit(Components[I]).OnButtonClick) then
            begin
              vItens[J].onClick  := TJvComboEdit(Components[I]).OnButtonClick;
              vCaption           := Copy(vCaption, P + 1, P1);
              vItens[J].ShortCut := TextToShortCut(vCaption);
            end;
          end
          else
          begin
            try
              vItens[J].onClick := TBitBtn(Components[I]).onClick;
            except
              try
                vItens[J].onClick := TSpeedButton(Components[I]).onClick;
              except
                vItens[J].onClick := TButton(Components[I]).onClick;
              end;
            end;
            vCaption           := Copy(vCaption, P + 1, P1);
            vItens[J].ShortCut := TextToShortCut(vCaption);
          end;
          ppm.Items.Add(vItens[J]);
          Inc(J);
        end;
      end;
    end;
  end;
end;

{------------------------------------------------------------------------------}
procedure SetaTopMost(const AHWnd: HWND);
begin
  SetWindowPos(AHWnd, HWND_TOPMOST, 0, 0, 0, 0,
               SWP_NOMOVE or SWP_NOSIZE or SWP_NOACTIVATE or SWP_NOOWNERZORDER);
end;

{------------------------------------------------------------------------------}
procedure CancelaTopMost(const AHWnd: HWND);
begin
  SetWindowPos(AHWnd, HWND_NOTOPMOST, 0, 0, 0, 0,
               SWP_NOMOVE or SWP_NOSIZE or SWP_NOACTIVATE or SWP_NOOWNERZORDER);
end;

{------------------------------------------------------------------------------}
procedure GravaCacheStr(const PChave: string; PValor: string);
begin
  try
    if DMG.cdsCache.Locate('CHAVE', PChave, [loCaseInsensitive]) then
      DMG.cdsCache.Edit
    else
    begin
      DMG.cdsCache.Append;
      DMG.cdsCacheCHAVE.AsString := PChave;
    end;
    DMG.cdsCacheVALOR.AsString := PValor;
    DMG.cdsCache.Post;
  except
  end;
end;

{------------------------------------------------------------------------------}
procedure GravaCacheInt(const PChave: string; PValor: Integer);
begin
  FG.GravaCacheStr(PChave, IntToStr(PValor));
end;

{------------------------------------------------------------------------------}
procedure GravaCacheBln(const PChave: string; PValor: boolean);
begin
  FG.GravaCacheStr(PChave, BoolToStr(PValor));
end;

{------------------------------------------------------------------------------}
procedure GravaCacheNum(const PChave: string; PValor: double);
begin
  FG.GravaCacheStr(PChave, FloatToStr(PValor));
end;

{------------------------------------------------------------------------------}
function LeCacheStr(const PChave: string): string; overload;
begin
  try
    Result := '';
    if DMG.cdsCache.Locate('CHAVE', PChave, [loCaseInsensitive]) then
      Result := DMG.cdsCacheVALOR.AsString;
  except
  end;
end;

{------------------------------------------------------------------------------}
function LeCacheStr(const PChave, ANovoValor: string): string; overload;
begin
  Result := FG.LeCacheStr(PChave);
  FG.GravaCacheStr(PChave, ANovoValor);
end;

{------------------------------------------------------------------------------}
function LeCacheInt(const PChave: string; PDef: Integer = 0): Integer;
begin
  Result := StrToIntDef(FG.LeCacheStr(PChave), PDef);
end;

{------------------------------------------------------------------------------}
function LeCacheBln(const PChave: string; PDef: boolean = false): boolean;
begin
  Result := StrToBoolDef(FG.LeCacheStr(PChave), PDef);
end;

{------------------------------------------------------------------------------}
function LeCacheNum(const PChave: string; PDef: double = 0): double;
begin
  Result := StrToFloatDef(FG.LeCacheStr(PChave), PDef);
end;

{------------------------------------------------------------------------------}
function SetaLocalIni: string;
begin
  Result := FH.SetaLocal('Sistema\INI\' + FH.RetNomeComputador);
end;

{------------------------------------------------------------------------------}
function SetaLocalRelatorio: string;
begin
  Result := FH.SetaLocal('Sistema\Relatorio\' + FH.RetNomeComputador);
end;

{------------------------------------------------------------------------------}
function SetaLocalDownloads: string;
begin
  Result := FH.SetaLocal('Sistema\Downloads\' + FH.RetNomeComputador);
end;

{------------------------------------------------------------------------------}
function SetaLocalEcf: string;
begin
  Result := FH.SetaLocal('Sistema\ECF\' + FH.RetNomeComputador);
end;

{------------------------------------------------------------------------------}
function SetaLocalNfe: string;
begin
     Result := FH.IncPathDel(FG.LeConfigEstabeStr(FG.CfgEstDirNFeGera));
     if not DirectoryExists(Result) then
        ForceDirectories(Result);
end;

{------------------------------------------------------------------------------}
function RetFormCaption(FR: string): string;
var
  M: TMenuItem;
begin
  Result := '';
  M := (FH.RetFormComp(Application.MainForm.Name, FR) as TMenuItem);
  if M = nil then
    exit;
  Result := M.Hint;
  if Result = '' then
    Result := FH.LimpaString(M.Caption, ['&']);
end;

{------------------------------------------------------------------------------}
function RetCaixaLocal: string;
begin
  Result := FH.LeRegistryStr(FG.REG_CHAVE_GERAL, FG.REG_CAIXA_LOCAL);
end;

{------------------------------------------------------------------------------}
function RetArqCfgLocal: string;
begin
  Result := FG.SetaLocalIni + FG.ArqCfgLocal;
end;

{------------------------------------------------------------------------------}
function ChamaInputBox(const PTipo: Smallint; const pFRCaption, PTexto: string;
  var PValor: string; const UpperCase: boolean = false): boolean;
begin
     Result := False;

     try
        Application.CreateForm(TFMsgInput, FMsgInput);

        FMsgInput.Caption          := pFRCaption;
        FMsgInput.lbPrompt.Caption := PTexto;

        if PTipo = FG.IBoxDate then
        begin
             if FH.DataOk(PValor) then
                FMsgInput.edData.Text := PValor;
        end
        else
        if PTipo = FG.IBoxNum then
        begin
             if FH.FloatOk(PValor) then
                FMsgInput.edValor.Text := PValor;
        end
        else
        if PTipo = FG.IBoxInt    then
        begin
             if FH.IntOk(PValor) then
                FMsgInput.edInt.Text := PValor;
        end
        else
        if PTipo = FG.IBoxMesAno then
        begin
             if FH.MesAnoValido(PValor) then
                FMsgInput.edMesAno.Text := PValor;
        end
        else
        if PTipo = FG.IBoxTexto  then
        begin
             FMsgInput.mmTexto.Lines.Text := PValor;
        end
        else
        begin
             FMsgInput.edDefault.Text := PValor;
        end;

        if PTipo = FG.IBoxDate   then FMsgInput.pgForm.ActivePage := FMsgInput.tsData   else
        if PTipo = FG.IBoxNum    then
        begin
             FMsgInput.pgForm.ActivePage     := FMsgInput.tsNum;
             FMsgInput.edValor.DisplayFormat := FG.vrgIBoxFmt;
             FMsgInput.edValor.DecimalPlaces := FG.vrgIBoxDecs;
        end
        else
        if PTipo = FG.IBoxInt    then FMsgInput.pgForm.ActivePage := FMsgInput.tsInt    else
        if PTipo = FG.IBoxMesAno then FMsgInput.pgForm.ActivePage := FMsgInput.tsMesAno else
        if PTipo = FG.IBoxTexto  then
        begin
             FMsgInput.Width  := 450;
             FMsgInput.Height := 350;
             FMsgInput.pgForm.ActivePage := FMsgInput.tsMemo;
        end
        else
        begin
             FMsgInput.pgForm.ActivePage := FMsgInput.tsDef;
             if PTipo = FG.IBoxPasw then
                FMsgInput.edDefault.PasswordChar := '*'
             else
                FMsgInput.Width  := 300;
             if UpperCase then
                FMsgInput.edDefault.CharCase := ecUpperCase;
        end;

        if FMsgInput.ShowModal = mrOk then
        begin
             case PTipo of
                  FG.IBoxMesAno : PValor := FMsgInput.edMesAno.Text;
                  FG.IBoxDate   : PValor := DateToStr(FMsgInput.edData.Date);
                  FG.IBoxNum    : PValor := FloatToStr(FMsgInput.edValor.Value);
                  FG.IBoxInt    : PValor := IntToStr(StrToIntDef(FMsgInput.edInt.Text,0));
                  FG.IBoxTexto  : PValor := FMsgInput.mmTexto.Lines.Text;
                  else
                                  PValor := FMsgInput.edDefault.Text;
             end;
             Result := true;
        end;
     finally
            FreeAndNil(FMsgInput);
     end;
end;

{------------------------------------------------------------------------------}
procedure ExecFormClose(FR: TForm = nil);
var
    F : TForm;
begin
     if FR <> nil then
        if FR.Owner <> nil then
        begin
             try
                if FR.Owner <> Application.MainForm then
                begin
                     TForm(FR.Owner).Show;
                     FG.AtivaAnotacao(TForm(FR.Owner).Name);
                     exit;
                end;
             except
             end;
        end;

     try
        F := Screen.Forms[1];
        if not FG.ClasseFormPermitida(F.ClassName) then
           Exit;
        if F = nil then
           Exit;
        F.Show;
        if F <> nil then
           FG.AtivaAnotacao(F.Name);
     except
     end;
end;

{------------------------------------------------------------------------------}
function AbreTab(PTabela: TDataSet; const AIndexFieldNames: string = ''): boolean;
begin
  if PTabela is TIBOTable then
  begin
    with PTabela as TIBOTable do
      if not Active then
      begin
        Open;
        if AIndexFieldNames <> '' then
          IndexFieldNames := AIndexFieldNames;
      end;
  end
  else
  if PTabela is TIBOQuery then
  begin
    with PTabela as TIBOQuery do
      if not Active then
        Open;
  end
  else
  if PTabela is TClientDataSet then
  begin
    with PTabela as TClientDataSet do
    begin
      if not Active then
      begin
        CreateDataSet;
        if AIndexFieldNames <> '' then
          IndexFieldNames := AIndexFieldNames;
      end;
    end;
  end;
end;

{------------------------------------------------------------------------------}
function VerModoLogin: boolean;
begin
  Result := FH.LeRegistry(FG.REG_CHAVE_GERAL, 'LOGIN') = '1';
end;

{------------------------------------------------------------------------------}
function VerModoDebug: boolean;
begin
  Result := FH.LeRegistry(FG.REG_CHAVE_GERAL, 'DEBUG') = '1';
end;

{------------------------------------------------------------------------------}
function PrimeiroCodigo(const PTabela: string; const PCampo: string = 'CODIGO';
  const ACondicao: string = ''): string;
const
  cResultField = 'min_reg_tab';
var
  tmpCursor: TIB_Cursor;
begin
  Result := '0';
  try
    tmpCursor := TIB_Cursor.Create(nil);
    tmpCursor.IB_Connection := DMG.IB_Connection;
    with tmpCursor.SQL do
    begin
      Text := Format('select min(%s) as %s from %s', [PCampo, cResultField, PTabela]);
      if ACondicao > '' then
        Text := Text + #13 + ACondicao;
    end;
    tmpCursor.Open;
    if FH.Int64Ok(tmpCursor.FieldByName(cResultField).AsString) then
      Result := IntToStr(tmpCursor.FieldByName(cResultField).AsInteger);
  finally
    tmpCursor.Close;
    FH.DestroiObj(tmpCursor);
  end;
end;

{------------------------------------------------------------------------------}
function ProximoCodigo(const PTabela: string; const PCampo: string = 'CODIGO';
  TR: TIB_Transaction = nil; ACast: boolean = false): string;
const
  cResultField = 'max_reg_tab';
var
  tmpCursor: TIB_Cursor;
begin
  Result := '';
  try
    tmpCursor := TIB_Cursor.Create(nil);
    tmpCursor.IB_Connection  := DMG.IB_Connection;
    tmpCursor.IB_Transaction := TR;
    with tmpCursor.SQL do
     if ACast then
      Text := Format('select max(cast(%s as integer)) as %s from %s', [PCampo, cResultField, PTabela])
     else
      Text := Format('select max(%s) as %s from %s', [PCampo, cResultField, PTabela]);
    Try
      tmpCursor.Open;
      if tmpCursor.FieldByName(cResultField).IsNull then
        Result := '1'
      else
      if FH.Int64Ok(tmpCursor.FieldByName(cResultField).AsString) then
        Result := IntToStr(1 + tmpCursor.FieldByName(cResultField).AsInteger);
    except
        Result := '';
    end;
  finally
    tmpCursor.Close;
    FH.DestroiObj(tmpCursor);
  end;
end;

{------------------------------------------------------------------------------}
function ProximoID(const PTabela: string; const PCampo: string;
  TR: TIB_Transaction = nil): Int64;
begin
  Result := StrToInt64Def(FG.ProximoCodigo(PTabela, PCampo, TR), 1);
end;

{------------------------------------------------------------------------------}
function Lookup(const PTab, PCampoRslt: string; const PCampos: array of string;
  const PValores: array of const): string;
var
  tmpCursor                : TIB_Cursor;
  vRes, vWhere, vVal, vAux : string;
  ii, jj                   : integer;
begin
  Result := '';
  try
    tmpCursor := TIB_Cursor.Create(nil);
    tmpCursor.IB_Connection := DMG.IB_Connection;

    vRes := PCampoRslt;
    if Pos(';', vRes) > 0 then
      vRes := StringReplace(vRes, ';', ',', [rfReplaceAll]);

    jj := 0;
    vWhere := '';
    for ii := low(PCampos) to high(PCampos) do
    begin
      if AnsiContainsText(vWhere, 'where') then
        vWhere := vWhere + #13 + '  and '
      else
        vWhere := vWhere + 'where ';

      vVal := '';
      with System.TVarRec(PValores[ii]) do
      begin
        case VType of
          System.vtInteger:
            begin
              vVal := vVal + IntToStr(VInteger);
              inc(jj);
            end;
          System.vtInt64:
            begin
              vVal := vVal + IntToStr(VInt64^);
              inc(jj);
            end;
          System.vtChar:
            begin
              jj   := jj + Length(VChar);
              vVal := vVal + QuotedStr(VChar);
            end;
          System.vtExtended:
            begin
              vVal := vVal + StringReplace(FloatToStr(VExtended^), ',', '.', [rfReplaceAll]);
              inc(jj);
            end;
          System.vtString:
            begin
              jj   := jj + Length(vString^);
              vVal := vVal + QuotedStr(vString^);
            end;
          System.vtPChar:
            begin
              jj   := jj + Length(VPChar);
              vVal := vVal + QuotedStr(VPChar);
            end;
          System.vtAnsiString:
            begin
              vAux := string(VAnsiString);
              jj   := jj + Length(vAux);
              if Length(vAux) = 10 then
                if  SameText(copy(vAux, 3, 1), DateSeparator)
                and SameText(copy(vAux, 6, 1), DateSeparator) then
                  vAux := AnsiDequotedStr(FH.FmtDataSql(vAux), '"');
              vVal := vVal + QuotedStr(vAux);
            end;
          System.vtCurrency:
            begin
              vVal := vVal + StringReplace(FloatToStr(VCurrency^), ',', '.', [rfReplaceAll]);
              inc(jj);
            end;
          System.vtVariant:
            begin
              if not VarIsClear(VVariant^) then
              begin
                vVal := vVal + VVariant^;
                inc(jj);
              end;
            end;
          System.vtWideString:
            begin
              jj   := jj + Length(WideString(VWideString));
              vVal := vVal + QuotedStr(WideString(VWideString));
            end;
        end;
      end;
      vAux := TrimRight(PCampos[ii]);
      if not AnsiMatchText(RightStr(vAux, 1), ['=', '>', '<']) then
        vAux := vAux + ' = ';
      vWhere := vWhere + vAux + vVal;
    end;

    if jj = 0 then
      exit;

    with tmpCursor.SQL do
    begin
      Add(Format('select %s from %s', [vRes, PTab]));
      Add(vWhere);
    end;

    tmpCursor.Open;
    tmpCursor.First;

    vRes := vRes + ',';
    while Pos(',', vRes) > 0 do
    begin
      if Result <> '' then
        Result := Result + #32;
      Result := Result + tmpCursor.FieldByName(FH.CopyPos(vRes, ',')).AsString;
      vRes := FH.CopyPosMax(vRes, ',');
    end;
  finally
    tmpCursor.Close;
    FH.DestroiObj(tmpCursor);
  end;
end;

{------------------------------------------------------------------------------}
function Lookup(const PTab, PCampoRslt: string; const PCampo, PValor: string): string;
begin
  Result := FG.Lookup(PTab, PCampoRslt, [PCampo], [PValor]);
end;

{------------------------------------------------------------------------------}
function Lookup(const PTab, PCampoRslt: string; const PCampo: string; const PValor: integer): string;
begin
  Result := FG.Lookup(PTab, PCampoRslt, [PCampo], [PValor]);
end;

{------------------------------------------------------------------------------}
function LocCampo(PQuery: TIBOQuery; const PTab, PCampoRslt: string;
  const PCampos: array of string; const PValores: array of const): string;
var
  vRes, vWhere, vVal, vAux : string;
  ii, jj                   : integer;
begin
  Result := '';

  vRes := PCampoRslt;
  if Pos(';', vRes) > 0 then
    vRes := StringReplace(vRes, ';', ',', [rfReplaceAll]);

  jj := 0;
  vWhere := '';
  for ii := low(PCampos) to high(PCampos) do
  begin
    if AnsiContainsText(vWhere, 'where') then
      vWhere := vWhere + #13 + '  and '
    else
      vWhere := vWhere + 'where ';

    vVal := '';
    if ii <= high(PValores) then
      with System.TVarRec(PValores[ii]) do
      begin
        case VType of
          System.vtInteger:
            begin
              vVal := vVal + IntToStr(VInteger);
              inc(jj);
            end;
          System.vtInt64:
            begin
              vVal := vVal + IntToStr(VInt64^);
              inc(jj);
            end;
          System.vtChar:
            begin
              jj   := jj + Length(VChar);
              vVal := vVal + QuotedStr(VChar);
            end;
          System.vtExtended:
            begin
              vVal := vVal + StringReplace(FloatToStr(VExtended^), ',', '.', [rfReplaceAll]);
              inc(jj);
            end;
          System.vtString:
            begin
              jj   := jj + Length(vString^);
              vVal := vVal + QuotedStr(vString^);
            end;
          System.vtPChar:
            begin
              jj   := jj + Length(VPChar);
              vVal := vVal + QuotedStr(VPChar);
            end;
          System.vtAnsiString:
            begin
              vAux := string(VAnsiString);
              jj   := jj + Length(vAux);
              if Length(vAux) = 10 then
                if  SameText(copy(vAux, 3, 1), DateSeparator)
                and SameText(copy(vAux, 6, 1), DateSeparator) then
                  vAux := AnsiDequotedStr(FH.FmtDataSql(vAux), '"');
              if vAux = '' then
                vVal := '%NULL%'
              else
                vVal := vVal + QuotedStr(vAux);
            end;
          System.vtCurrency:
            begin
              vVal := vVal + StringReplace(FloatToStr(VCurrency^), ',', '.', [rfReplaceAll]);
              inc(jj);
            end;
          System.vtVariant:
            begin
              if not VarIsClear(VVariant^) then
              begin
                vVal := vVal + VVariant^;
                inc(jj);
              end;
            end;
          System.vtWideString:
            begin
              jj   := jj + Length(WideString(VWideString));
              vVal := vVal + QuotedStr(WideString(VWideString));
            end;
        end;
      end;

    if vVal = '%NULL%' then
    begin
      vAux := TrimRight(PCampos[ii]);
      if not AnsiMatchText(RightStr(vAux, 1), ['=', '>', '<']) then
        vAux := vAux + ' IS NULL '
      else
      if AnsiMatchText(RightStr(vAux, 2), ['>=', '<='])
      or AnsiMatchText(RightStr(vAux, 1), ['>', '<']) then
        vAux := vAux + ' IS NOT NULL '
      else
        vAux := vAux + ' IS NULL ';
      vVal := '';
    end
    else
    begin
      vAux := TrimRight(PCampos[ii]);
      if not AnsiMatchText(RightStr(vAux, 1), ['=', '>', '<']) then
        vAux := vAux + ' = ';
    end;

    vWhere := vWhere + vAux + vVal;
  end;

  if jj = 0 then
    exit;

  with PQuery.SQL do
  begin
    Clear;
    Add(Format('select %s from %s', [vRes, PTab]));
    Add(vWhere);
  end;

  PQuery.Open;
  PQuery.First;

  vRes := vRes + ',';
  while Pos(',', vRes) > 0 do
  begin
    if Result <> '' then
      Result := Result + #32;
    Result := Result + PQuery.FieldByName(FH.CopyPos(vRes, ',')).AsString;
    vRes := FH.CopyPosMax(vRes, ',');
  end;
end;

{------------------------------------------------------------------------------}
function LocCampo(PQuery: TIBOQuery; const PTab, PCampoRslt: string;
  const PCampo, PValor: string): string;
begin
  Result := FG.LocCampo(PQuery, PTab, PCampoRslt, [PCampo], [PValor]);
end;

{------------------------------------------------------------------------------}
function LocCampo(PQuery: TIBOQuery; const PTab, PCampoRslt: string;
  const PCampo: string; const PValor: integer): string;
begin
  Result := FG.LocCampo(PQuery, PTab, PCampoRslt, [PCampo], [PValor]);
end;

{------------------------------------------------------------------------------}
function RegistroExiste(const PTab, PCampo, PValor: string): boolean;
begin
  if PValor = '' then
    Result := false
  else
    Result := FG.Lookup(PTab, PCampo, PCampo, PValor) = PValor;
end;

{------------------------------------------------------------------------------}
function FindKey(PQuery: TIBOQuery; const AParams: string; const PValores: array of const): boolean;
var
  vCam   : string;
  ii, jj : integer;
  lst    : TStrings;
begin
  Result := false;
  if PQuery = nil then
    exit;
  PQuery.Close;
  try
    FH.CriaLista(lst);
    vCam := AParams;
    if vCam = '' then vCam := 'CODIGO';
    if Pos(';', vCam) > 0 then
      vCam := StringReplace(vCam, ';', ',', [rfReplaceAll]);
    lst := FH.StrParaLista(vCam);

    jj := 0;
    for ii := 0 to pred(lst.Count) do
    begin
      if ii <= high(PValores) then
        with System.TVarRec(PValores[ii]) do
        begin
          case VType of
            System.vtInteger:
              begin
                PQuery.ParamByName(lst[ii]).AsInteger := VInteger;
                inc(jj);
              end;
            System.vtChar:
              begin
                PQuery.ParamByName(lst[ii]).AsString := VChar;
                inc(jj);
              end;
            System.vtExtended:
              begin
                PQuery.ParamByName(lst[ii]).AsFloat := VExtended^;
                inc(jj);
              end;
            System.vtString:
              begin
                PQuery.ParamByName(lst[ii]).AsString := vString^;
                inc(jj);
              end;
            System.vtPChar:
              begin
                PQuery.ParamByName(lst[ii]).AsString := VPChar;
                inc(jj);
              end;
            System.vtAnsiString:
              begin
                PQuery.ParamByName(lst[ii]).AsString := string(VAnsiString);
                inc(jj);
              end;
            System.vtCurrency:
              begin
                PQuery.ParamByName(lst[ii]).AsCurrency := VCurrency^;
                inc(jj);
              end;
            System.vtVariant:
              begin
                if not VarIsClear(VVariant^) then
                begin
                  PQuery.ParamByName(lst[ii]).Value := VVariant^;
                  inc(jj);
                end;
              end;
            System.vtWideString:
              begin
                PQuery.ParamByName(lst[ii]).AsString := WideString(VWideString);
                inc(jj);
              end;
            System.vtInt64:
              begin
                PQuery.ParamByName(lst[ii]).AsInteger := VInt64^;
                inc(jj);
              end;
          end;
        end;
    end;
    if jj = 0 then
      exit;
    PQuery.Open;
    Result := not PQuery.IsEmpty;
  finally
    FH.DestroiObj(lst);
  end;
end;

{------------------------------------------------------------------------------}
function FindKeyLivre(PQuery: TIBOQuery; const PTabela, PCampos: string; const PValores: array of const): boolean;
var
  vCam, vAux, vWhere, vVal : string;
  ii, jj                   : integer;
  lst                      : TStrings;
begin
  Result := false;
  if (PQuery = nil) or FH.StrInvalida(PTabela) or FH.StrInvalida(PCampos) then
    exit;
  PQuery.Close;
  try
    FH.CriaLista(lst);
    vCam := PCampos;
    if Pos(';', vCam) > 0 then
      vCam := StringReplace(vCam, ';', ',', [rfReplaceAll]);
    lst := FH.StrParaLista(vCam);

    jj := 0;
    vWhere := '';
    for ii := 0 to pred(lst.Count) do
    begin
      if ii <= high(PValores) then
      begin
        if AnsiContainsText(vWhere, 'where') then
          vWhere := vWhere + #13 + '  and '
        else
          vWhere := vWhere + 'where ';

        vVal := '';
        with System.TVarRec(PValores[ii]) do
        begin
          case VType of
            System.vtInteger:
              begin
                vVal := vVal + IntToStr(VInteger);
                inc(jj);
              end;
            System.vtInt64:
              begin
                vVal := vVal + IntToStr(VInt64^);
                inc(jj);
              end;
            System.vtChar:
              begin
                jj   := jj + Length(VChar);
                vVal := vVal + QuotedStr(VChar);
              end;
            System.vtExtended:
              begin
                vVal := vVal + StringReplace(FloatToStr(VExtended^), ',', '.', [rfReplaceAll]);
                inc(jj);
              end;
            System.vtString:
              begin
                jj   := jj + Length(vString^);
                vVal := vVal + QuotedStr(vString^);
              end;
            System.vtPChar:
              begin
                jj   := jj + Length(VPChar);
                vVal := vVal + QuotedStr(VPChar);
              end;
            System.vtAnsiString:
              begin
                vAux := string(VAnsiString);
                jj   := jj + Length(vAux);
                if Length(vAux) = 10 then
                  if  SameText(copy(vAux, 3, 1), DateSeparator)
                  and SameText(copy(vAux, 6, 1), DateSeparator) then
                    vAux := AnsiDequotedStr(FG.DataSql(vAux), '"');
                vVal := vVal + QuotedStr(vAux);
              end;
            System.vtCurrency:
              begin
                vVal := vVal + StringReplace(FloatToStr(VCurrency^), ',', '.', [rfReplaceAll]);
                inc(jj);
              end;
            System.vtVariant:
              begin
                if not VarIsClear(VVariant^) then
                begin
                  vVal := vVal + VVariant^;
                  inc(jj);
                end;
              end;
            System.vtWideString:
              begin
                jj   := jj + Length(WideString(VWideString));
                vVal := vVal + QuotedStr(WideString(VWideString));
              end;
          end;
        end;
        vAux := TrimRight(lst[ii]);
        if not AnsiMatchText(RightStr(vAux, 1), ['=', '>', '<']) then
          vAux := vAux + ' = ';
        vWhere := vWhere + vAux + vVal;
      end;
    end;

    if jj = 0 then
      exit;

    with PQuery.SQL do
    begin
      Clear;
      Add(Format('select * from %s', [PTabela]));
      Add(vWhere);
    end;
    PQuery.Open;
    Result := not PQuery.IsEmpty;
  finally
    FH.DestroiObj(lst);
  end;
end;

{------------------------------------------------------------------------------}
function ExeStoredProc(PQuery: TIBOQuery; const AParams: string; const PValores: array of const): boolean;
var
  vCam   : string;
  ii, jj : integer;
  lst    : TStrings;
begin
  Result := false;
  if PQuery = nil then
    exit;
  PQuery.Close;
  PQuery.Unprepare;
  try
    FH.CriaLista(lst);
    vCam := AParams;
    if Pos(';', vCam) > 0 then
      vCam := StringReplace(vCam, ';', ',', [rfReplaceAll]);
    lst := FH.StrParaLista(vCam);

    jj := 0;
    for ii := 0 to pred(lst.Count) do
    begin
      if ii <= high(PValores) then
        with System.TVarRec(PValores[ii]) do
        begin
          case VType of
            System.vtInteger:
              begin
                PQuery.ParamByName(lst[ii]).AsInteger := VInteger;
                inc(jj);
              end;
            System.vtChar:
              begin
                PQuery.ParamByName(lst[ii]).AsString := VChar;
                inc(jj);
              end;
            System.vtExtended:
              begin
                PQuery.ParamByName(lst[ii]).AsFloat := VExtended^;
                inc(jj);
              end;
            System.vtString:
              begin
                PQuery.ParamByName(lst[ii]).AsString := vString^;
                inc(jj);
              end;
            System.vtPChar:
              begin
                PQuery.ParamByName(lst[ii]).AsString := VPChar;
                inc(jj);
              end;
            System.vtAnsiString:
              begin
                PQuery.ParamByName(lst[ii]).AsString := string(VAnsiString);
                inc(jj);
              end;
            System.vtCurrency:
              begin
                PQuery.ParamByName(lst[ii]).AsCurrency := VCurrency^;
                inc(jj);
              end;
            System.vtVariant:
              begin
                if not VarIsClear(VVariant^) then
                begin
                  PQuery.ParamByName(lst[ii]).Value := VVariant^;
                  inc(jj);
                end;
              end;
            System.vtWideString:
              begin
                PQuery.ParamByName(lst[ii]).AsString := WideString(VWideString);
                inc(jj);
              end;
            System.vtInt64:
              begin
                PQuery.ParamByName(lst[ii]).AsInteger := VInt64^;
                inc(jj);
              end;
          end;
        end;
    end;
    if jj = 0 then
      exit;
    PQuery.Prepare;
    PQuery.Open;
    Result := not PQuery.IsEmpty;
  finally
    FH.DestroiObj(lst);
  end;
end;

{------------------------------------------------------------------------------}
function CarregaLista(const PTabela, ACondicao: string;
  const PCampos: string = 'CODIGO,DESCRICAO'; const AOrderBy: string = 'CODIGO'): TStrings;
var
  tmpCursor                 : TIB_Cursor;
  ii                        : smallint;
  tmpStr, vCampos, vOrderBy : string;
  L                         : TStrings;
begin
  FH.CriaLista(Result);
  try
    FH.CriaLista(L);
    tmpCursor := TIB_Cursor.Create(nil);
    tmpCursor.IB_Connection := DMG.IB_Connection;
    vCampos := PCampos;
    if Pos(';', vCampos) > 0 then
      vCampos := StringReplace(vCampos, ';', ',', [rfReplaceAll]);
    L := FH.StrParaLista(vCampos);
    vOrderBy := AOrderBy;
    if Pos(';', vOrderBy) > 0 then
      vOrderBy := StringReplace(vOrderBy, ';', ',', [rfReplaceAll]);
    with tmpCursor.SQL do
    begin
      Add(Format('select %s from %s', [vCampos, PTabela]));
      if ACondicao <> '' then
        Add(ACondicao);
      if AOrderBy <> '' then
        Add('order by ' + vOrderBy);
    end;
    tmpCursor.Open;
    tmpCursor.First;
    while not tmpCursor.Eof do
    begin
      tmpStr := '';
      for ii := 0 to L.Count - 1 do
        tmpStr := tmpStr + tmpCursor.FieldByName(Trim(L[ii])).AsString + #32;
      Result.Add(TrimRight(tmpStr));
      tmpCursor.Next;
    end;
  finally
    tmpCursor.Close;
    FH.DestroiObj(L);
    FH.DestroiObj(tmpCursor);
  end;
end;

{------------------------------------------------------------------------------}
function CarregaLista(DS: TDataSet; const PCampos: string): TStrings;
var
  ii              : smallint;
  tmpStr, vCampos : string;
  L               : TStrings;
begin
  FH.CriaLista(Result);
  if DS.IsEmpty then
    Exit;
  try
    FH.CriaLista(L);
    vCampos := PCampos;
    if Pos(';', vCampos) > 0 then
      vCampos := StringReplace(vCampos, ';', ',', [rfReplaceAll]);
    L := FH.StrParaLista(vCampos);
    DS.First;
    while not DS.Eof do
    begin
      tmpStr := '';
      for ii := 0 to L.Count - 1 do
        tmpStr := tmpStr + DS.FieldByName(Trim(L[ii])).AsString + #32;
      Result.Add(TrimRight(tmpStr));
      DS.Next;
    end;
    DS.First;
  finally
    FH.DestroiObj(L);
  end;
end;

{------------------------------------------------------------------------------}
function CarregaStringListFmt(const PTab: string; const AFields: string = ' ';
  pFormat: string = '%-6s   %-30s '; ASep: string = '-'): TStrings;
var
  vCa1, vCa2: string;
  tmpCursor : TIB_Cursor;
begin
  Result := TStringList.Create;
  Result.Text := '';
  if Pos(';', AFields) = 0 then
  begin
    vCa1 := 'CODIGO';
    vCa2 := 'DESCRICAO';
  end
  else
  begin
    vCa1 := Copy(AFields, 1, Pos(';', AFields) - 1);
    vCa2 := Copy(AFields, Pos(';', AFields) + 1, 30);
  end;
  try
    tmpCursor := TIB_Cursor.Create(nil);
    tmpCursor.IB_Connection := DMG.IB_Connection;
    with tmpCursor.SQL do
      Text := Format('select %s, %s from %s', [vCa1, vCa2, PTab]);
    tmpCursor.Open;
    tmpCursor.First;
    while not tmpCursor.Eof do
    begin
      Result.Add(Format(pFormat, [tmpCursor.FieldByName(vCa1).AsString,
                                  ASep + tmpCursor.FieldByName(vCa2).AsString]));
      tmpCursor.Next;
    end;
  finally
    tmpCursor.Close;
    FH.DestroiObj(tmpCursor);
  end;
end;

{------------------------------------------------------------------------------}
procedure CarregaListView(LV: TListView; const PTabela, ACondicao: string;
  const PCampos: string = 'CODIGO,DESCRICAO'; const AOrderBy: string = 'CODIGO');
var
  tmpCursor                 : TIB_Cursor;
  ii                        : smallint;
  tmpStr, vCampos, vOrderBy : string;
  L                         : TStrings;
  li                        : TListItem;
begin
  try
    FH.CriaLista(L);
    tmpCursor := TIB_Cursor.Create(nil);
    tmpCursor.IB_Connection := DMG.IB_Connection;
    vCampos := PCampos;
    if Pos(';', vCampos) > 0 then
      vCampos := StringReplace(vCampos, ';', ',', [rfReplaceAll]);
    L := FH.StrParaLista(vCampos);
    vOrderBy := AOrderBy;
    if Pos(';', vOrderBy) > 0 then
      vOrderBy := StringReplace(vOrderBy, ';', ',', [rfReplaceAll]);
    with tmpCursor.SQL do
    begin
      Add(Format('select %s from %s', [vCampos, PTabela]));
      if ACondicao <> '' then
        Add(ACondicao);
      if AOrderBy <> '' then
        Add('order by ' + vOrderBy);
    end;
    tmpCursor.Open;
    tmpCursor.First;
    while not tmpCursor.Eof do
    begin
      li := LV.Items.Add;
      for ii := 0 to L.Count - 1 do
      begin
        tmpStr := tmpCursor.FieldByName(Trim(L[ii])).AsString;
        if ii = 0 then
          li.Caption := tmpStr
        else
          li.SubItems.Add(tmpStr);
      end;
      tmpCursor.Next;
    end;
  finally
    tmpCursor.Close;
    FH.DestroiObj(L);
    FH.DestroiObj(tmpCursor);
  end;
end;

{------------------------------------------------------------------------------}
procedure GravaTabLog(FR: TForm; const PMensagem: string; const POrigem: string='');
var  tmpLs ,
     priLs : TStrings;
     frm   ,
     ss    : string;
begin
     try
        FH.CriaLista(tmpLs);
        FH.CriaLista(priLs);
        FG.RetFormsAtivos(tmpLs, priLs);
        ss  := FH.ListaParaStr(priLs);
        frm := '';
        if FR <> nil then
           frm := FR.Name;

        DMG.qrTabLog.Close;
        DMG.qrTabLog.Open;
        DMG.qrTabLog.Insert;
        DMG.qrTablog.FieldByName(           'ID').AsInteger :=  DMG.qrTablog.GeneratorValue('GEN_TABLOG_ID',1);
        DMG.qrTablog.FieldByName(      'USUARIO').AsString  :=  FG.RetUsuAtivo;
        DMG.qrTablog.FieldByName(   'COMPUTADOR').AsString  :=  FH.RetNomeComputador;
        DMG.qrTablog.FieldByName(         'FORM').AsString  :=  frm;
        DMG.qrTablog.FieldByName( 'FORMSABERTOS').AsString  :=  ss;
        DMG.qrTablog.FieldByName(       'ORIGEM').AsString  :=  FH.Iff(POrigem='','Forms',POrigem);
        DMG.qrTablog.FieldByName(     'MENSAGEM').AsString  :=  PMensagem;
        DMG.qrTablog.FieldByName('DATAHORALOCAL').AsString  :=  FH.TimeStamp;
        DMG.qrTablog.Post;
        DMG.qrTabLog.Close;
     finally
            FH.DestroiObj(priLs);
            FH.DestroiObj(tmpLs);
     end;
end;

{------------------------------------------------------------------------------}
procedure GravaLogErrosDB(const PMensagem, PForm, PSql: string);
var
    tmpLs : TStrings;
    priLs : TStrings;
    ss    : string;
    ii    : Integer;
begin
    try
        FH.CriaLista(tmpLs);
        FH.CriaLista(priLs);

        FG.RetFormsAtivos(tmpLs, priLs);
        ss := '';
        for ii := 0 to priLs.Count - 1 do
            ss := ss + IfThen(ss>'',', ') + priLs[ii];

        tmpLs.Clear;
        tmpLs.Add(DupeString('-', 80));
        tmpLs.Add('Computador   : ' + FH.RetNomeComputador);
        tmpLs.Add('Usuário Sist.: ' + FG.RetUsuAtivo      );
        tmpLs.Add('Data/Hora    : ' + DateTimeToStr(Now)  );
        tmpLs.Add('Programa     : ' + PForm               );
        tmpLs.Add('Forms abertos: ' + ss                  );
        tmpLs.Add('   ');
        tmpLs.Add('  >>>> SQL EXECUTADO <<<< ');
        tmpLs.Add(PSql);
        tmpLs.Add('   ');
        tmpLs.Add('  >>>> MENSAGEM <<<<<<<<< ');
        tmpLs.Add(PMensagem);

        try
            ss := FG.SetaLocalRelatorio + FG.ArqLogErro;
            if FileExists(ss) then
               priLs.LoadFromFile(ss);
            priLs.Add(tmpLs.Text);
            priLs.SaveToFile(ss);
        except
        end;
    finally
        FH.DestroiObj(priLs);
        FH.DestroiObj(tmpLs);
    end;
end;

{------------------------------------------------------------------------------}
procedure SetaVisual(FR: TForm);
var I, A, tot       : Integer;
    vUpper, frmFlex : boolean;
    mniLoc          : TMenuItem;
    ppmChk          : TPopupMenu;
    vComponente     : TComponent;
    vCXBtn          : TcxButton;
    vJVBtn          : TJvBitBtn;
    vJVCEdit        : TJvComboEdit;
    vSPBtn          : TSpeedButton;
begin
    vUpper   := false;
    ppmChk := nil;

    { usamos a propriedade ALPHABLENDVALUE para saber quando testar o form }
    if FR.AlphaBlendValue = 50 then
    begin
        if  (not SameText(FR.Name, 'FICM'))
        and (not SameText(FR.Name, 'FATRIBUTO')) then
           vUpper := FG.LeConfigEstabeBool(FG.CfgEstUpper, false);
    end;

    { faço este teste para verificar se o form é flexível, ou seja,
      se pode ser redimensionado. Caso seja, é executado o teste para setar o
      evento de resize no panel de botões. }
    frmFlex :=     (FR.BorderStyle = bsSizeable)
               and ((FR.Constraints.MaxWidth = 0) or (FR.Constraints.MinWidth = 0));

    for I := 0 to FR.ComponentCount - 1 do
    begin
        vComponente := FR.Components[I];
        { configura os jvcomboedits com função de pesquisa }
        if vComponente is TJvComboEdit then
        begin
            vJVCEdit := TJvComboEdit(vComponente);
            if vJVCEdit.Glyph.Empty then
            begin
                if FH.ComparaTextoFixo(vJVCEdit.Name, 'EDLOCORIGEM' )
                or FH.ComparaTextoFixo(vJVCEdit.Name, 'EDLOCDESTINO') then
                begin
                    vJVCEdit.Glyph := FMenu.imLocSel.Picture.Bitmap;
                    vJVCEdit.ButtonHint := 'Localizar (Enter)';
                    vJVCEdit.ShowHint := true;
                end
                else
                begin
                    vJVCEdit.Glyph := FMenu.imPesquisar.Picture.Bitmap;
                    if vJVCEdit.ButtonHint = '' then
                    begin
                        vJVCEdit.ButtonHint := 'Consultar (F10)';
                        vJVCEdit.ShowHint := true;
                    end;
                end;
            end;
        end
        else
        {if vComp is TJvCalcEdit then
        begin
             (vComp as TJvCalcEdit).Glyph := FMenu.imCalc.Picture.Bitmap;
        end
        else}
        if vComponente is TSpeedButton then
        begin
            { configura os botões de navegação }
            if FH.ComparaTextoFixo(vComponente.Name, 'sbNext') then
               (vComponente as TSpeedButton).Glyph := FMenu.imNext.Picture.Bitmap
            else
            if FH.ComparaTextoFixo(vComponente.Name, 'sbPrev') then
               (vComponente as TSpeedButton).Glyph := FMenu.imPrev.Picture.Bitmap
            else
            { configura os botões de dicas }
            if FH.ComparaTextoFixo(vComponente.Name, 'sbDica') then
            begin
                (vComponente as TSpeedButton).Glyph    := FMenu.imDica.Picture.Bitmap;
                (vComponente as TSpeedButton).Hint     := 'Clique aqui para visualizar uma dica!';
                (vComponente as TSpeedButton).ShowHint := True;
            end
            else
            begin
                vSPBtn := (vComponente as TSpeedButton);

                { configura a imagem dos botões }
                if  (FH.IntOk(vSPBtn.HelpKeyword)) and (vSPBtn.Glyph.Empty) then
                begin
                   if FR.Name = FMenu.Name then
                     DMG.imMenu.GetBitmap(StrToInt(vSPBtn.HelpKeyword), vSPBtn.Glyph)
                   else
                     DMG.ImgDefault.GetBitmap(StrToInt(vSPBtn.HelpKeyword), vSPBtn.Glyph);
                end;
            end;
        end
        else
        if vComponente is TComboBox then
        begin
            if TComboBox(vComponente).DropDownCount < 16 then
               TComboBox(vComponente).DropDownCount := 16;
        end
        else
        { configura a imagem dos botões }
        if vComponente is TJvBitBtn then
        begin
            vJVBtn := (vComponente as TJvBitBtn);
            vJVBtn.HotTrack := False;
            if FH.IntOk(vJVBtn.HelpKeyword) then
            begin
                if FR.Name = FMenu.Name then
                   DMG.imMenu.GetBitmap(StrToInt(vJVBtn.HelpKeyword), vJVBtn.Glyph)
                else
                   DMG.ImgDefault.GetBitmap(StrToInt(vJVBtn.HelpKeyword), vJVBtn.Glyph);
            end;
        end
        else
        { teste para setar o OnResize dos panels de botões }
        {
        if frmFlex and
           (vComponente is TPanel) and
           (vComponente.Tag <> 777)  then
        begin
            if FH.ComparaTextoFixo((vComponente as TPanel).Name, 'PNBTN') then
            begin
               if not Assigned((vComponente as TPanel).OnResize) then
                  (vComponente as TPanel).OnResize := FMenu.pnBtn_Resize;
            end
            else
            if ((vComponente as TPanel).Align = alBottom) and
               ((vComponente as TPanel).Height < 37     ) then
            begin
               tot := 0;

               for A := 0 to (vComponente as TPanel).ControlCount - 1 do
                   if (vComponente as TPanel).Controls[A] is TJvBitBtn then
                      if ((vComponente as TPanel).Controls[A] as TJvBitBtn).Visible then
                         Inc(tot);

               if tot > 1 then
                  if not Assigned((vComponente as TPanel).OnResize) then
                     (vComponente as TPanel).OnResize := FMenu.pnBtn_Resize;
            end;
        end
        else
        }
        { configura as imagens dos botões }
        if vComponente is TcxButton then
        begin
            vCXBtn := (vComponente as TcxButton);
            if FH.IntOk(vCXBtn.HelpKeyword) then
            begin
                if FR.Name = FMenu.Name then
                   DMG.imMenu.GetBitmap(StrToInt(vCXBtn.HelpKeyword), vCXBtn.Glyph)
                else
                   DMG.ImgDefault.GetBitmap(StrToInt(vCXBtn.HelpKeyword), vCXBtn.Glyph);
            end;
        end
        else
        { configura o KeyUp dos TableViews }
        if vComponente is TcxGridDBTableView then
        begin
            if not Assigned((vComponente as TcxGridDBTableView).OnKeyUp) then
               (vComponente as TcxGridDBTableView).OnKeyUp := DMG.QuantumGrid_KeyUp;

            { configura o grid para considerar as strings nulas }
            with (vComponente as TcxGridDBTableView).DataController do
                if not DataModeController.GridMode then
                    Filter.Options := Filter.Options + [fcoSoftNull];
        end
        else
        { configura o KeyUp dos BandedTableViews }
        if vComponente is TcxGridDBBandedTableView then
        begin
            if not Assigned((vComponente as TcxGridDBBandedTableView).OnKeyUp) then
               (vComponente as TcxGridDBBandedTableView).OnKeyUp := DMG.QuantumGrid_KeyUp;

            { configura o grid para considerar as strings nulas }
            with (vComponente as TcxGridDBBandedTableView).DataController do
                if not DataModeController.GridMode then
                    Filter.Options := Filter.Options + [fcoSoftNull];
        end
        else
        {}
        if vComponente is TcxCheckComboBox then
        begin
            if ppmChk = nil then
            begin
               ppmChk          := TPopupMenu.Create(FR);
               ppmChk.Tag      := -5;
               ppmChk.Name     := 'ppm' + FH.GetTickCountStr;
               mniLoc          := TMenuItem.Create(FR);
               mniLoc.Caption  := 'Localizar na lista';
               mniLoc.ShortCut := TextToShortCut('Ctrl+F');
               mniLoc.OnClick  := FMenu.mniLoc_CheckComboBox;
               ppmChk.Items.Add(mniLoc);
            end;
            (vComponente as TcxCheckComboBox).PopupMenu := ppmChk;
        end
        else
        { [spRight]}
        if vComponente is TcxPageControl then
        begin
             if (vComponente as TcxPageControl).Style = 9 then
                (vComponente as TcxPageControl).TabSlants.Positions := [spRight];
        end
        { teste para não modificar o estilo do grid }
        else
        if vComponente is TcxGrid then
        begin
            if (vComponente as TcxGrid).HelpKeyword <> 'PRESERVAR' then
            begin
                (vComponente as TcxGrid).LookAndFeel.NativeStyle    := True;
                (vComponente as TcxGrid).LookAndFeel.Kind           := lfOffice11;
                (vComponente as TcxGrid).LookAndFeel.AssignedValues := [lfvNativeStyle, lfvKind];
            end;
        end;

        if vUpper then
        begin
            if vComponente is TEdit then
            begin
                if (vComponente as TEdit).CharCase = ecNormal then
                   (vComponente as TEdit).CharCase := ecUpperCase;
            end
            else
            if vComponente is TJvComboEdit then
            begin
                if (vComponente as TJvComboEdit).CharCase = ecNormal then
                   (vComponente as TJvComboEdit).CharCase := ecUpperCase;
            end;
        end;
    end;
end;

{------------------------------------------------------------------------------}
function RetDataCompila: string;
var
  FHandle: Integer;
  S: string;
begin
  Result := '';
  try
    ChDir(ExtractFilePath(Application.ExeName));
    FHandle := FileOpen(Application.ExeName, fmShareDenyNone);
    FileGetDate(FHandle);
    S := FormatDateTime('dddd, dd/mm/yyyy hh:mm', FileDateToDateTime(FileGetDate(FHandle)));
    Result := FH.SetaUpper(S, 1);
  except
    Result := '';
  end;
end;

{------------------------------------------------------------------------------}
function RetMemFis: string;
begin
  Result := FH.FmtINT(Round((jclSysInfo.GetTotalPhysicalMemory / 1024) / 1024)) + ' MB';
end;

{------------------------------------------------------------------------------}
function RetCopyright: string;
begin
  Result := Chr(169) + '2009 Pressier Informática Ltda.';
end;

{------------------------------------------------------------------------------}
function RetVersaoSis(const SomenteBuild: Boolean): string;
begin
    Result := FH.GetInfoVersao(Application.ExeName);
    if SomenteBuild then
    begin
        while Pos('.', Result) > 0 do
            Result := FH.CopyPosMax(Result, '.');
    end;
end;

{------------------------------------------------------------------------------}
procedure RetFormsAtivos(Lista: TStrings; vNome: TStrings = nil);
var
  I: Integer;
begin
  Lista.Clear;
  I := 0;
  for I := 0 to Screen.FormCount - 1 do
  begin
    if not FG.ClasseFormPermitida(Screen.Forms[I].ClassName) then
      Continue;
    Lista.Add(Screen.Forms[I].Caption);
    if vNome <> nil then
      vNome.Add(Screen.Forms[I].Name);
  end;
end;

{------------------------------------------------------------------------------}
function ContaFormsAtivos: Integer;
var
  I: Integer;
begin
  Result := 0;
  for I := 0 to Screen.FormCount - 1 do
  begin
      if FG.ClasseFormPermitida(Screen.Forms[I].ClassName) then
         Inc(Result);
  end;
end;

{------------------------------------------------------------------------------}
procedure FocalizeGrid(GR: TcxGrid; ATecla: Word);
begin
  if (ATecla <> VK_DOWN) and (ATecla <> VK_UP) then
    exit;
  FH.Focalize(GR);
end;

{------------------------------------------------------------------------------}
procedure ChamaCnsCep(FR: TForm; const AEndereco, ACEP, ACidade: string;
  const PIdentificadorRetorno: string = '');
const
  NOME_FORM = 'FCepConsulta';
  CLAS_FORM = '_TFCepConsulta';
var
  temp: string;
begin
  if FH.RetForm(NOME_FORM) <> nil then
  begin
    FH.RetForm(NOME_FORM).Show;
    exit;
  end;

  temp := AEndereco;
  if AnsiMatchText(FH.CopyPos(temp, #32), ['RUA', 'AVENIDA', 'AV', 'R.', 'AV.', 'R']) then
    temp := FH.CopyPosMax(temp, #32)
  else
    temp := Trim(temp);
  if AnsiMatchText(FH.CopyPos(temp, #32), ['DOS', 'DAS']) then
    temp := FH.CopyPosMax(temp, #32);
  temp := FH.RetStrChar(temp);

  FG.GravaCacheStr(FG.CnsIndexCEP, PIdentificadorRetorno);
  FG.ChamaForm(FR, CLAS_FORM, 'Consulta - CEP', 0);

  FH.RetFormEdit(NOME_FORM, 'edLog').Text := AnsiUpperCase(temp);
  if SameText(FH.LimpaEspc(ACidade), '<NENHUM>')
  or FH.StrInvalida(FH.LimpaEspc(ACidade)) then
    FH.RetFormEdit(NOME_FORM, 'edCid').Text := AnsiUpperCase(FG.Lookup('TABESTABE', 'CIDADE', 'CODIGO', FG.RetEstabeAtivo))
  else
    FH.RetFormEdit(NOME_FORM, 'edCid').Text := AnsiUpperCase(ACidade);
  FH.RetFormMaskEdit(NOME_FORM, 'edCEP').Text := AnsiUpperCase(ACEP);
  FG.RetAtvFormJvBitBtn('btLocalizar').Click;
end;

{------------------------------------------------------------------------------}
function CriptografaSenhaUsu(const APwd: string): string;
begin
  if Length(APwd) <= (FG.TAM_PWD div 4) then
    Result := FH.GeraCripto(APwd, false, FG.TAM_PWD)
  else
    Result := APwd;
end;

{------------------------------------------------------------------------------}
function RevelaSenhaUsu(const APwd: string): string;
begin
  if Length(APwd) = FG.TAM_PWD then
    Result := FH.GeraCripto(APwd, true, FG.TAM_PWD)
  else
    Result := APwd;
end;

{------------------------------------------------------------------------------}
function RetUsuAtivo: string;
begin
     Result := FG.LeCacheStr('$Sis_Usuario');
end;

{------------------------------------------------------------------------------}
function RetUsuEstabe: string;
begin
     Result := FG.LeCacheStr('$Sis_Usu_Estabe');
end;

{------------------------------------------------------------------------------}
function RetUsuOperador: string;
begin
     Result := FG.LeCacheStr('$Sis_Operador');
end;

{------------------------------------------------------------------------------}
function VerOperadorInformado: Boolean;
begin
     Result := FG.LeCacheStr('$Sis_Operador_Informado') = 'SIM';
end;

{------------------------------------------------------------------------------}
function RetUsuNomeOperador: string;
begin
     Result := FG.LeCacheStr('$Sis_Nome_Operador');
end;

{------------------------------------------------------------------------------}
function RetUsuCaixa: string;
begin
     Result := FG.LeCacheStr('$Sis_Caixa');
end;

{------------------------------------------------------------------------------}
function RetUsuNomeCaixa: string;
begin
     Result := FG.LeCacheStr('$Sis_Nome_Caixa');
end;

{------------------------------------------------------------------------------}
function RetUsuVerRecadosOnline: string;
begin
     Result := FG.LeCacheStr('$Sis_Recados_Online');
end;

{------------------------------------------------------------------------------}
function RetUsuPortaCaixa: Integer;
begin
  Result := FG.LeCacheInt('$Sis_Porta_Caixa');
end;

{------------------------------------------------------------------------------}
function RetUsuPdvCaixa: string;
begin
  Result := FG.LeCacheStr('$Sis_Pdv_Caixa');
end;

{------------------------------------------------------------------------------}
function RetUsuGavetaCaixa: string;
begin
  Result := FG.LeCacheStr('$Sis_Gaveta_Caixa');
end;

{------------------------------------------------------------------------------}
function RetUsuLiberaCartaoMagnetico: string;
begin
  Result := FG.LeCacheStr('$Sis_Libera_Cartao_Caixa');
end;

{------------------------------------------------------------------------------}
function RetNomeEcf(const PCodigoECF: Integer): string;
begin
  case PCodigoECF of
    EcfGer.ID_Itautec1EBR   : Result := 'Itautec 1EBR';
    EcfGer.ID_Itautec1EII   : Result := 'Itautec 1EII';
    EcfGer.ID_Trends10E     : Result := 'Trends 1.0E';
    EcfGer.ID_MIParalela    : Result := 'MI-Paralela';
    EcfGer.ID_Virtual       : Result := 'Imp. Virtual';
    EcfGer.ID_LoggerII      : Result := 'Loger II';
    EcfGer.ID_Schalter30    : Result := 'Schalter 3.0';
    EcfGer.ID_SwedaST120    : Result := 'Sweda ST120';
    EcfGer.ID_Sweda7000     : Result := 'Sweda 7000';
    EcfGer.ID_BematechMP2100: Result := 'Bematech MP-2100';
    else
      Result := '< NÃO USA ECF >';
  end;
end;

{------------------------------------------------------------------------------}
function RetUsuEcfCaixa: Integer;
begin
  if FG.LeCacheStr('VenPedFatura_DEMO') = 'SIM' then
    Result := EcfGer.ID_Virtual
  else
  if FG.LeCacheStr('VenCupom_MI') = 'SIM' then
    Result := EcfGer.ID_MIParalela
  else
  if FG.LeCacheStr('VenCupom_Virtual') = 'SIM' then
    Result := EcfGer.ID_Virtual
  else
  if FG.LeCacheStr('RecBaixa_NaoUsaECF') = 'SIM' then
    Result := 99
  else
  if FG.LeCacheStr('RecInc_NaoUsaECF') = 'SIM' then
    Result := 99
  else
    Result := FG.LeCacheInt('$Sis_Ecf_Caixa');
end;

{------------------------------------------------------------------------------}
function RetUsuNomeEcfCaixa: string;
begin
  Result := FG.RetNomeEcf(FG.RetUsuEcfCaixa);
end;

{------------------------------------------------------------------------------}
function RetEstabeAtivo: string;
begin
  Result := FG.LeCacheStr('$Sis_Estabe');
end;

{------------------------------------------------------------------------------}
function RetLicenca: string;
begin
  Result := FG.LeCacheStr('$Sis_Licenca');
end;

{------------------------------------------------------------------------------}
function RetAmxEstabe(const PEstabe: string): string;
begin
  if FG.FindKey(DMG.qrAmxPadraoEstabe, 'ESTABE', [IfThen(PEstabe = '', FG.RetEstabeAtivo, PEstabe)]) then
  begin
    Result := DMG.qrAmxPadraoEstabe.FieldByName('CODIGO').AsString;
  end
  else
  if FG.FindKey(DMG.qrAmxEstabe, 'ESTABE', [IfThen(PEstabe = '', FG.RetEstabeAtivo, PEstabe)]) then
  begin
    DMG.qrAmxEstabe.First;
    Result := DMG.qrAmxEstabe.FieldByName('CODIGO').AsString;
  end
  else
    Result := '';
end;

{------------------------------------------------------------------------------}
function RetUsuAmxCaixa: string;
begin
  Result := FG.LeCacheStr('$Sis_Amx_Caixa');
end;

{------------------------------------------------------------------------------}
function RetEstabeAtivoNome: string;
begin
  Result := FG.LeCacheStr('$Sis_Estabe_Nome');
end;

{------------------------------------------------------------------------------}
function RetEstabeAtivoRazao: string;
begin
  Result := FG.LeCacheStr('$Sis_Estabe_Razao');
end;

{------------------------------------------------------------------------------}
function RetEstabeAtivoEndereco: string;
begin
  Result := FG.LeCacheStr('$Sis_Estabe_Endereco');
end;

{------------------------------------------------------------------------------}
function RetEstabeAtivoNumero: string;
begin
  Result := FG.LeCacheStr('$Sis_Estabe_Numero');
end;

{------------------------------------------------------------------------------}
function RetEstabeAtivoBairro: string;
begin
  Result := FG.LeCacheStr('$Sis_Estabe_Bairro');
end;

{------------------------------------------------------------------------------}
function RetEstabeAtivoCidade: string;
begin
  Result := FG.LeCacheStr('$Sis_Estabe_Cidade');
end;

{------------------------------------------------------------------------------}
function RetEstabeAtivoCidadeIBGE: string;
begin
  Result := FG.LeCacheStr('$Sis_Estabe_CidIbge');
end;

{------------------------------------------------------------------------------}
function RetEstabeAtivoEstado: string;
begin
  Result := FG.LeCacheStr('$Sis_Estabe_Estado');
end;

{------------------------------------------------------------------------------}
function RetEstabeAtivoCnpj: string;
begin
  Result := FG.LeCacheStr('$Sis_Estabe_Cnpj');
end;

{------------------------------------------------------------------------------}
function RetEstabeAtivoInscEst: string;
begin
  Result := FG.LeCacheStr('$Sis_Estabe_InscEst');
end;

function RetEstabeAtivoInscMun: string;
begin
  Result := FG.LeCacheStr('$Sis_Estabe_InscMun');
end;

{------------------------------------------------------------------------------}
function RetEstabeAtivoCep: string;
begin
  Result := FG.LeCacheStr('$Sis_Estabe_Cep');
end;

{------------------------------------------------------------------------------}
function RetEstabeAtivoFone: string;
begin
  Result := FG.LeCacheStr('$Sis_Estabe_Fone');
end;

{------------------------------------------------------------------------------}
function RetEstabeAtivoFax: string;
begin
  Result := FG.LeCacheStr('$Sis_Estabe_Fax');
end;

{------------------------------------------------------------------------------}
function RetEstabeAtivoEmail: string;
begin
  Result := FG.LeCacheStr('$Sis_Estabe_Email');
end;

{------------------------------------------------------------------------------}
function RetEstabeAtivoSite: string;
begin
  Result := FG.LeCacheStr('$Sis_Estabe_Site');
end;

{------------------------------------------------------------------------------}
function LeQuery(pCons: integer): string;
begin
  if DMG.cdsScript.Locate('ID', pCons, []) then
    Result := DMG.cdsScriptSQL.AsString;
end;

{------------------------------------------------------------------------------}
procedure GravaLogSQL(const PTexto: string; const ForcePost: Boolean = False);
const
  MaxReg = 25;
var
  I: Integer;
begin
  if not ForcePost then
    if not FG.VerModoDebug then
      Exit;

  if not DMG.cdsScript.Active then
  begin
    // ??????
  end;
  DMG.cdsScript.Last;
  if (DMG.cdsScriptSQL.AsString = PTexto) or FH.StrInvalida(PTexto) then
    exit;
  if DMG.cdsScript.RecordCount >= MaxReg then
  begin
    DMG.cdsScript.First;
    while DMG.cdsScript.RecordCount >= MaxReg do
      DMG.cdsScript.Delete;
    DMG.cdsScript.Last;
  end;
  I := DMG.cdsScriptID.AsInteger + 1;
  DMG.cdsScript.Append;
  DMG.cdsScriptID.AsInteger    := i;
  DMG.cdsScriptSQL.AsString    := PTexto;
  DMG.cdsScriptDATA.AsDateTime := Now;
  DMG.cdsScript.Post;
  DMG.cdsScript.SaveToFile(FG.RetArqScripts);
end;

{------------------------------------------------------------------------------}
procedure GravaLogSQL(PQuery: TIBOQuery);
var
  ii: Integer;
  tx: string;
begin
  tx := PQuery.SQL.Text;
  for ii := 0 to PQuery.Params.Count-1 do
    tx := StringReplace(tx,
                        ':'     + PQuery.Params[ii].Name,
                        ':'     + PQuery.Params[ii].Name +
                        '  /* ' + PQuery.Params[ii].AsString + ' */',
                        [rfReplaceAll, rfIgnoreCase]);

  FG.GravaLogSQL(tx);
end;

{------------------------------------------------------------------------------}
function RetDM(ADataModule: string): TDataModule;
var
  I: Integer;
begin
  Result := nil;
  for I := 0 to Screen.DataModuleCount - 1 do
    if (Screen.DataModules[I] as TDataModule).Name = ADataModule then
    begin
      Result := (Screen.DataModules[I] as TDataModule);
      Break;
    end;
end;

{------------------------------------------------------------------------------}
function RetDMComp(ADataModule, PCtr: string): TComponent;
begin
  Result := nil;
  try
    Result := (FG.RetDM(ADataModule).FindComponent(PCtr) as TComponent);
  except
    Result := nil;
  end;
end;

{------------------------------------------------------------------------------}
function RetDisplayImp: string;
const
  Cn = '< Sem Impressora >';
begin
  Result := Cn;
  try
    if Printer.PrinterIndex = -1 then
      Result := Cn
    else
    begin
      Result := Printer.Printers[Printer.PrinterIndex];
      if Result = '' then Result := Cn;
    end;
  except
    Result := Cn;
  end;
end;

{------------------------------------------------------------------------------}
procedure SetaDisplayImp;
var
  tmpStr: string;
begin
  tmpStr := FG.RetDisplayImp;

  FMenu.lbStatusImp.Caption := tmpStr;

  if FH.RetForm('FCOMNOTAEMI') <> nil then
    if FH.RetFormLabel('FCOMNOTAEMI', 'LBIMPPADRAO') <> nil then
       FH.RetFormLabel('FCOMNOTAEMI', 'LBIMPPADRAO').Caption := tmpStr;

  if FH.RetForm('FVENNOTAEMI') <> nil then
    if FH.RetFormLabel('FVENNOTAEMI', 'LBIMPPADRAO') <> nil then
       FH.RetFormLabel('FVENNOTAEMI', 'LBIMPPADRAO').Caption := tmpStr;
end;

{------------------------------------------------------------------------------}
procedure RetTabelasAtivas(Lista: TStrings);
var
  ii: integer;

  procedure Scan(AOwner: TComponent);
  var
    jj: integer;
  begin
    with AOwner do
      for jj := 0 to ComponentCount - 1 do
        if Components[jj] is TIBOQuery then
        begin
          if (Components[jj] as TIBOQuery).Active then
            Lista.Add((Components[jj] as TIBOQuery).Name + ' - Query');
        end
        else
        if Components[jj] is TIB_Cursor then
        begin
          if (Components[jj] as TIB_Cursor).Active then
            Lista.Add((Components[jj] as TIB_Cursor).Name + ' - Cursor');
        end
        else
        if Components[jj] is TIBOTable then
        begin
          if (Components[jj] as TIBOTable).Active then
            Lista.Add((Components[jj] as TIBOTable).Name + ' - Query Table (' +
                            (Components[jj] as TIBOTable).TableName + ')');
        end;
  end;

begin
  Lista.Clear;
  for ii := 0 to Screen.DataModuleCount - 1 do Scan(Screen.DataModules[ii]);
  for ii := 0 to Screen.FormCount - 1 do Scan(Screen.Forms[ii]);
end;

{------------------------------------------------------------------------------}
procedure GeraAtalhoMenu;
var
  vIco     : TIcon;
  J        : Integer;
  M        : TMenuItem;
  tmpQuery : TIBOQuery;
begin
  try
    tmpQuery := TIBOQuery.Create(nil);
    tmpQuery.IB_Connection := DMG.IB_Connection;
    tmpQuery.SQL.Text := 'SELECT * FROM USUATALHO' +
                      #13'WHERE UPPER(USUARIO) = ' + QuotedStr(AnsiUpperCase(FG.RetUsuAtivo)) +
                      #13'ORDER BY ATALHO';
    tmpQuery.Open;

    while FMenu.imAtalho.Count > 0 do
      FMenu.imAtalho.Delete(0);

    while FMenu.tlbAtalho.ButtonCount > 0 do
      FMenu.tlbAtalho.Buttons[0].Free;

    FMenu.cdsAtalho.Close;
    FMenu.cdsAtalho.CreateDataSet;
    FMenu.cdsAtalho.IndexFieldNames := 'ID';

    J := -1;
    tmpQuery.Last;
    while not tmpQuery.Bof do
    begin
      // se não for separador
      if LeftStr(tmpQuery.FieldByName('FORMULARIO').AsString, 1) <> '|' then
      begin
        vIco := nil;
        vIco := ticon.Create;
        if (tmpQuery.FieldByName('IMAGEM') as TBlobField).BlobSize > 0 then
          vIco.Assign(tmpQuery.FieldByName('IMAGEM'))
        else
          vIco.Assign(FMenu.imAux.Picture.Icon);
        J := FMenu.imAtalho.AddIcon(vIco);
      end;
      with TToolButton.Create(FMenu.tlbAtalho) do
      begin
        Parent := FMenu.tlbAtalho;
        //Cursor := crHandPoint;
        if LeftStr(tmpQuery.FieldByName('FORMULARIO').AsString, 1) <> '|' then
        begin
          Caption := tmpQuery.FieldByName('FORMULARIO').AsString;
          OnClick := FMenu.tbAtalhoClick;
          M := TMenuItem(FMenu.FindComponent(tmpQuery.FieldByName('FORMULARIO').AsString));
          if M <> nil then // se é "dropped-down-menu"
            if (Pos('_', M.Name) = 0) and (Pos('$', tmpQuery.FieldByName('FORMULARIO').AsString) = 0) then
            begin
              Style := tbsDropDown;
              MenuItem := M;
            end;
          ImageIndex := J;
          Hint := tmpQuery.FieldByName('OPCAO').AsString;

          FMenu.cdsAtalho.AppendRecord([tmpQuery.FieldByName('OPCAO').AsString,
                                        tmpQuery.FieldByName('FORMULARIO').AsString,
                                        tmpQuery.FieldByName('ATALHO').AsInteger]);
        end
        else
        begin
          Style := tbsSeparator;
          Width := 10;
        end;
      end;
      tmpQuery.Prior;
    end;

    if FG.LeConfigUsuBool(FG.CfgUsuListaAtalho, False) then
    begin
        FMenu.tlbAtalho.Visible := False;
        FMenu.grAtalhoDBTableView1.DataController.GotoFirst;
        FMenu.grAtalho.Visible  := not tmpQuery.IsEmpty;
        if FMenu.grAtalho.Visible then
        begin
            FMenu.grAtalho.Width  := 370;
            FMenu.grAtalho.Height := (FMenu.grAtalhoDBTableView1.DataController.RecordCount *
                                      FMenu.grAtalhoDBTableView1.OptionsView.DataRowHeight) + 40;
            FMenu.grAtalhoDBTableView1OPCAO.Width := 350;
            FMenu.PosicionaGridAtalhos;
        end;
    end
    else
    begin
        FMenu.grAtalho.Visible  := False;
        FMenu.tlbAtalho.Visible := not tmpQuery.IsEmpty;
    end;
  finally
    FH.DestroiObj(tmpQuery);
  end;
end;

{------------------------------------------------------------------------------}
function RetArqScripts: string;
begin
  Result := FG.SetaLocalRelatorio + FG.ArqScript;
end;

{------------------------------------------------------------------------------}
procedure CancelaAcesso(FR: TWinControl; AUsuario, AOpcao: string);
var
    I : Integer;
    S : string;
begin
    try
        DMG.qrCancelaAcesso.Close;
        DMG.qrCancelaAcesso.ParamByName('USUARIO').AsString    := AnsiUpperCase(AUsuario);
        DMG.qrCancelaAcesso.ParamByName('FORMULARIO').AsString := AnsiUpperCase(AOpcao);
        DMG.qrCancelaAcesso.Open;

        if DMG.qrCancelaAcesso.IsEmpty then
           Exit;

        for I := 0 to FR.ComponentCount - 1 do
        begin
            if (not (FR.Components[I] is TJvBitBtn)) and
               (not (FR.Components[I] is TButton)) and
               (not (FR.Components[I] is TBitBtn)) and
               (not (FR.Components[I] is TMenuItem)) then
              Continue;

            if FR.Components[I] is TMenuItem then
            begin
                S := (FR.Components[I] as TMenuItem).Caption;
                S := LowerCase(Copy(S, 3, 15));
                if (S <> 'incluir') and (S <> 'alterar') and (S <> 'excluir') then
                  Continue;
                (FR.Components[I] as TMenuItem).Enabled := DMG.qrCancelaAcesso.FieldByName(UpperCase(S)).AsString <> 'S';
            end
            else
            begin
                S := (FR.Components[I] as TControl).Name;
                S := LowerCase(Copy(S, 3, 15));
                if (S <> 'incluir') and (S <> 'alterar') and (S <> 'excluir') then
                  Continue;
                (FR.Components[I] as TControl).Enabled := DMG.qrCancelaAcesso.FieldByName(UpperCase(S)).AsString <> 'S';
            end;

            if (not (FR.Components[I] is TMenuItem)) then
               if (not (FR.Components[I] as TControl).Enabled) then
               begin
                   if FR.Components[I] is TJvBitBtn then
                      S := (FR.Components[I] as TJvBitBtn).Caption
                   else
                   if FR.Components[I] is TButton then
                      S := (FR.Components[I] as TButton).Caption
                   else
                   if FR.Components[I] is TBitBtn then
                      S := (FR.Components[I] as TBitBtn).Caption;
                   if (Pos('(', S) > 0) and (Pos(')', S) > 0) then
                      S := LeftStr(S, Pos('(', S) - 1);
                   if FR.Components[I] is TJvBitBtn then
                      (FR.Components[I] as TJvBitBtn).Caption := S
                   else
                   if FR.Components[I] is TButton then
                      (FR.Components[I] as TButton).Caption := S
                   else
                   if FR.Components[I] is TBitBtn then
                      (FR.Components[I] as TBitBtn).Caption := S;
               end;
        end;
    finally
        DMG.qrCancelaAcesso.Close;
    end;
end;

{------------------------------------------------------------------------------}
function VerAcessoGravar(FR: string; Acao: string; AMsg: boolean = true): boolean;
begin
  Result := true;
  DMG.qrUsuPermissao.Close;
  DMG.qrUsuPermissao.ParamByName('USUARIO').AsString    := FG.RetUsuAtivo;
  DMG.qrUsuPermissao.ParamByName('FORMULARIO').AsString := '_T' + FR;
  DMG.qrUsuPermissao.Open;
  if DMG.qrUsuPermissao.IsEmpty then
  begin
    DMG.qrUsuPermissao.Close;
    exit;
  end;
  Acao := AnsiLowerCase(Acao);
  if Pos('incl', Acao) > 0 then
  begin
    Result := DMG.qrUsuPermissao.FieldByName('INCLUIR').AsString <> 'S';
    if not Result then
    begin
      if AMsg then
        FH.PrMsg_ER('Usuário não possui permissão para "Incluir".');
    end;
  end
  else
  if Pos('alte', Acao) > 0 then
  begin
    Result := DMG.qrUsuPermissao.FieldByName('ALTERAR').AsString <> 'S';
    if not Result then
    begin
      if AMsg then
        FH.PrMsg_ER('Usuário não possui permissão para "Alterar".');
    end;
  end
  else
  if Pos('gerar', Acao) > 0 then
  begin
    Result :=   (DMG.qrUsuPermissao.FieldByName('ALTERAR').AsString <> 'S')
             or (DMG.qrUsuPermissao.FieldByName('INCLUIR').AsString <> 'S');
    if not Result then
    begin
      if AMsg then
        FH.PrMsg_ER('Usuário não possui permissão para "Gerar".');
    end;
  end
  else
  if Pos('excl', Acao) > 0 then
  begin
    Result := DMG.qrUsuPermissao.FieldByName('EXCLUIR').AsString <> 'S';
    if not Result then
    begin
      if AMsg then
        FH.PrMsg_ER('Usuário não possui permissão para "Excluir".');
    end;
  end;
  DMG.qrUsuPermissao.Close;
end;

{------------------------------------------------------------------------------}
function VerAcessoGravar(FR: TForm; Acao: string; AMsg: boolean = true): boolean;
begin
  Result := FG.VerAcessoGravar(FR.Name, Acao, AMsg);
  if not Result then
    FH.FocalizeFirst(FR);
end;

{------------------------------------------------------------------------------}
procedure CancelaAcessoMenu(const PUsuario: string);
var
    M : TMenuItem;
begin
    try
        DMG.qrCancelaAcessoMenu.Close;
        DMG.qrCancelaAcessoMenu.ParamByName('USUARIO').AsString := AnsiUpperCase(PUsuario);
        DMG.qrCancelaAcessoMenu.Open;
        if not DMG.qrCancelaAcessoMenu.IsEmpty then
        begin
            DMG.qrCancelaAcessoMenu.First;
            while not DMG.qrCancelaAcessoMenu.Eof do
            begin
                M := (FMenu.FindComponent(DMG.qrCancelaAcessoMenu.FieldByName('FORMULARIO').AsString) as TMenuItem);
                if M <> nil then
                begin
                    M.Enabled := true;
                    M.Visible := true;
                    if DMG.qrCancelaAcessoMenu.FieldByName('STATUS').AsString = 'S' then
                        M.Enabled := false
                    else
                    if DMG.qrCancelaAcessoMenu.FieldByName('STATUS').AsString = 'I' then
                    begin
                        M.Enabled := false;
                        M.Visible := false;
                    end;

                    if AnsiMatchText(M.Name, ['mniAdm', 'mniCRM', 'mniFat', 'mniFin', 'mniGCQ',
                                              'mniFis', 'mniGer', 'mniLoj', 'mniSup']) then
                    begin
                        (FMenu.FindComponent('tbt' + Copy(M.Name, 4, 3)) as TToolButton).Enabled := M.Enabled;
                        (FMenu.FindComponent('tbt' + Copy(M.Name, 4, 3)) as TToolButton).Visible := M.Visible;
                    end;
                end;
                DMG.qrCancelaAcessoMenu.Next;
            end;
        end;
    finally
        DMG.qrCancelaAcessoMenu.Close;
    end;

    try
        DMG.qrTrataLicencaMenu.Close;
        DMG.qrTrataLicencaMenu.Open;
        if not DMG.qrTrataLicencaMenu.IsEmpty then
        begin
            DMG.qrTrataLicencaMenu.First;
            while not DMG.qrTrataLicencaMenu.Eof do
            begin
                M := (FMenu.FindComponent(DMG.qrTrataLicencaMenu.FieldByName('OPCAO').AsString) as TMenuItem);
                if M <> nil then
                begin
                    if DMG.qrTrataLicencaMenu.FieldByName('MOSTRAR').AsString = 'S' then
                    begin
                        if M.Enabled then
                        begin
                            M.Enabled := True;
                            M.Visible := True;
                        end;
                    end
                    else
                    if DMG.qrTrataLicencaMenu.FieldByName('MOSTRAR').AsString = 'N' then
                    begin
                        M.Enabled := false;
                        M.Visible := false;
                    end;

                    if AnsiMatchText(M.Name, ['mniAdm', 'mniCRM', 'mniFat', 'mniFin',
                                              'mniFis', 'mniGer', 'mniLoj', 'mniSup']) then
                    begin
                        (FMenu.FindComponent('tbt' + Copy(M.Name, 4, 3)) as TToolButton).Enabled := M.Enabled;
                        (FMenu.FindComponent('tbt' + Copy(M.Name, 4, 3)) as TToolButton).Visible := M.Visible;
                    end;
                end;
                DMG.qrTrataLicencaMenu.Next;
            end;
        end;
    finally
        DMG.qrTrataLicencaMenu.Close;
    end;
end;

{------------------------------------------------------------------------------}
function RetStatusAcesso(FR: string): TStatusAcesso;
begin
    Result.staStatus  := true;
    Result.staIncluir := true;
    Result.staAlterar := true;
    Result.staExcluir := true;
    try
        DMG.qrRetStatusAcesso.Close;
        DMG.qrRetStatusAcesso.ParamByName('USUARIO').AsString    := AnsiUpperCase(FG.RetUsuAtivo);
        DMG.qrRetStatusAcesso.ParamByName('FORMULARIO').AsString := AnsiUpperCase(FR);
        DMG.qrRetStatusAcesso.Open;
        if not DMG.qrRetStatusAcesso.IsEmpty then
        begin
            Result.staStatus := (DMG.qrRetStatusAcesso.FieldByName('STATUS').AsString <> 'S')
                            and (DMG.qrRetStatusAcesso.FieldByName('STATUS').AsString <> 'I');
            Result.staIncluir := DMG.qrRetStatusAcesso.FieldByName('INCLUIR').AsString <> 'S';
            Result.staAlterar := DMG.qrRetStatusAcesso.FieldByName('ALTERAR').AsString <> 'S';
            Result.staExcluir := DMG.qrRetStatusAcesso.FieldByName('EXCLUIR').AsString <> 'S';
        end;
    finally
        DMG.qrRetStatusAcesso.Close;
    end;
end;

{------------------------------------------------------------------------------}
procedure GetDDDFone(const AOrigem: string; AEditDDD, AEditFone: TEdit);
var
  S, Z: string;
begin
  S := Trim(AOrigem);
  if Length(S) = 0 then
  begin
    AEditDDD.Clear;
    AEditFone.Clear;
    exit;
  end;

  if (Pos('(', S) > 0) and (Pos(')', S) > 0) and (AEditDDD <> nil) then
  begin
    Z := LeftStr(S, (Pos(')', S)));
    Z := FH.StrInteiro(Z);
    AEditDDD.Text := Z;
  end;
  if AEditFone = nil then
    exit;
  if Pos(')', S) > 0 then
    S := Copy(S, Pos(')', S) + 1, MaxInt);
  S := FH.StrInteiro(S);
  if Length(S) >= 7 then
    Insert('-', S, Length(S) - 3);
  AEditFone.Text := S;
end;

{------------------------------------------------------------------------------}
Function RetDDDFone(AEditDDD, AEditFone : TEdit): string;
begin
  if Length(Trim(AEditFone.Text)) = 0 then
    Result := ''
  else
    Result := Format('(%s)%s', [AEditDDD.Text, AEditFone.Text]);
end;

{------------------------------------------------------------------------------}
function VerFoco(FR: TForm; Sender: TObject; AField: TWinControl): boolean;
begin
  Result := true;
  if SameText(AField.Name, FR.ActiveControl.Name) then
  begin
    if AField is TJvComboEdit then
    begin
      if not (AField as TJvComboEdit).ReadOnly then
        exit;
    end
    else if AField is TEdit then
    begin
      if not (AField as TEdit).ReadOnly then
        exit;
    end
    else if AField is TMaskEdit then
    begin
      if not (AField as TMaskEdit).ReadOnly then
        exit;
    end
    else
      exit;
  end;
  if not (Sender is TMenuItem) then
    if SameText(AField.Name, (Sender as TControl).Name) then
      exit;
  Result := false;
end;

{------------------------------------------------------------------------------}
function RetAtvFormJvBitBtn(PCtr: string): TJvBitBtn;
begin
  Result := (FH.RetAtvFormComp(PCtr) as TJvBitBtn);
end;

{------------------------------------------------------------------------------}
function RetFormJvBitBtn(FR, PCtr: string): TJvBitBtn;
begin
  Result := (FH.RetFormComp(FR, PCtr) as TJvBitBtn);
end;

{------------------------------------------------------------------------------}
function RetAtvFormJvDateEdit(PCtr: string): TJvDateEdit;
begin
  Result := (FH.RetAtvFormWinControl(PCtr) as TJvDateEdit);
end;

{------------------------------------------------------------------------------}
function RetFormJvDateEdit(FR, PCtr: string): TJvDateEdit;
begin
  Result := (FH.RetFormWinControl(FR, PCtr) as TJvDateEdit);
end;

{------------------------------------------------------------------------------}
function RetAtvFormJvComboEdit(PCtr: string): TJvComboEdit;
begin
  Result := (FH.RetAtvFormWinControl(PCtr) as TJvComboEdit);
end;

{------------------------------------------------------------------------------}
function RetFormJvComboEdit(FR, PCtr: string): TJvComboEdit;
begin
  Result := (FH.RetFormWinControl(FR, PCtr) as TJvComboEdit);
end;

{------------------------------------------------------------------------------}
function RetAtvFormCxSpinEdit(PCtr: string): TCxSpinEdit;
begin
  Result := (FH.RetAtvFormWinControl(PCtr) as TCxSpinEdit);
end;

{------------------------------------------------------------------------------}
function RetFormCxSpinEdit(FR, PCtr: string): TCxSpinEdit;
begin
  Result := (FH.RetFormWinControl(FR, PCtr) as TCxSpinEdit);
end;

{------------------------------------------------------------------------------}
procedure EnviaEmail(const Destinatario, CopiaOculta, Assunto, Texto: string; const Anexos: array of string; const EnvioDireto: Boolean = False);
var
    JM : TJvMail;
    ii : integer;
begin
    try
        JM := TJvMail.Create(nil);

        JM.Subject   := Assunto;
        JM.Body.Text := Texto;

        for ii := Low(Anexos) to High(Anexos) do
            if FileExists(Anexos[ii]) then
               JM.Attachment.Add(Anexos[ii]);

        if Destinatario <> '' then
           JM.Recipient.AddRecipient(Destinatario);

        if CopiaOculta <> '' then
           JM.BlindCopy.AddRecipient(CopiaOculta);

        JM.SendMail( not EnvioDireto );
    finally
        JM.Free;
    end;
end;

{------------------------------------------------------------------------------}
procedure SetaImp;
var
  Res: DWORD;
  Device, Driver, Port, WindowsStr: array[0..255] of Char;
  HDeviceMode: THandle;
begin
  Printer.GetPrinter(Device, Driver, Port, Hdevicemode);
  StrCat(device, ',');
  StrCat(device, Driver);
  StrCat(device, ',');
  StrCat(device, Port);
  StrCopy(WindowsStr, 'Windows');

  WriteProfileString(WindowsStr, 'device', Device);

  SendMessageTimeout(HWND_BROADCAST, WM_WININICHANGE, 0, DWORD(@WindowsStr),
                     SMTO_NORMAL, 1000, Res);

  FG.SetaDisplayImp;
end;

{------------------------------------------------------------------------------}
procedure SetaNovaImp(const PNome: string; const APadrao: integer);
var
  ii: integer;
begin
  if FH.StrInvalida(PNome) then
    exit;
  ii := Printer.Printers.IndexOf(PNome);
  if (ii > -1) and (ii <> APadrao) then
  begin
    Printer.PrinterIndex := ii;
    FG.SetaImp;
  end;
end;

{------------------------------------------------------------------------------}
procedure VoltaImpDef(const APadrao: integer);
begin
  if Printer.PrinterIndex <> APadrao then
  begin
    Printer.PrinterIndex := APadrao;
    FG.SetaImp;
  end;
end;

{------------------------------------------------------------------------------}
procedure SetaPapelCustom(ALargura, AAltura: Integer);
var
  ADevice, ADriver, APort: array[0..255] of Char;
  vDevHandle: THandle;
  vDevPoint: PDevMode;
  vStr: string;
begin
  vStr := Printer.Printers[Printer.PrinterIndex]; { Bug da VCL - Não remover}
  Printer.GetPrinter(ADevice, ADriver, APort, vDevHandle);
  vDevPoint := GlobalLock(vDevHandle);
  try
    if not (vDevPoint = nil) then
    begin
      vDevPoint^.dmFields := DM_PAPERSIZE;
      vDevPoint^.dmFields := vDevPoint^.dmFields or DM_PAPERLENGTH or DM_PAPERWIDTH;
      vDevPoint^.dmPaperWidth := ALargura;
      vDevPoint^.dmPaperLength := AAltura;
      vDevPoint^.dmPaperSize := DMPAPER_USER;
      Printer.SetPrinter(ADevice, ADriver, APort, vDevHandle);
    end;
  finally
    GlobalUnlock(vDevHandle);
  end;
end;

{------------------------------------------------------------------------------}
procedure ChamaEditorTxt1(const ALinhas: string; FR: TForm);
begin
     FEditorTxt1 := TFEditorTxt1.Create(FR);
     FEditorTxt1.mmTexto.Lines.Text := ALinhas;
     FEditorTxt1.Caption            := FR.Caption;
     FEditorTxt1.ShowModal;
end;

{------------------------------------------------------------------------------}
function ChamaEditorTxt2(FR: TForm; const FRAltura: Integer; const TextoEnt: string; var TextoSai: string): Boolean;
begin
     TextoSai := '';

     FEditorTxt2 := TFEditorTxt2.Create(FR);
     FEditorTxt2.chGravaobs.Visible := false;
     FEditorTxt2.Height             := FRAltura;
     FEditorTxt2.Memo.Lines.Text    := TextoEnt;
     FEditorTxt2.MemoChange(FEditorTxt2.Memo);
     FEditorTxt2.ShowModal;

     Result := FG.LeCacheBln('$Editor2Txt_Resp');

     if Result then
        TextoSai := FG.LeCacheStr('$Editor2Txt_Texto');
end;

{------------------------------------------------------------------------------}
function VerOpe(const PControlador: TWinControl; const ASinal: string): boolean;
var
  temp: string;
begin
  Result := false;
  if PControlador is TGroupBox   then
    temp := (PControlador as TGroupBox).Caption
  else
  if PControlador is TStaticText then
    temp := (PControlador as TStaticText).Caption
  else
    exit;
  if AnsiContainsText(temp, ASinal) then
    Result := true;
end;

{------------------------------------------------------------------------------}
function VerOpeINC(const PControlador: TWinControl): boolean;
begin
  Result := FG.VerOpe(PControlador, 'INCL');
end;

{------------------------------------------------------------------------------}
function VerOpeALT(const PControlador: TWinControl): boolean;
begin
  Result := FG.VerOpe(PControlador, 'ALTE');
end;

{------------------------------------------------------------------------------}
procedure SetaOpe(const PControlador: TWinControl; const ASinal: string);
begin
  if PControlador is TGroupBox   then
    (PControlador as TGroupBox).Caption   := ASinal
  else
  if PControlador is TStaticText then
    (PControlador as TStaticText).Caption := ASinal
  else
    exit;
end;

{------------------------------------------------------------------------------}
procedure SetaOpeINC(const PControlador: TWinControl);
begin
  FG.SetaOpe(PControlador, 'Incluir');
end;

{------------------------------------------------------------------------------}
procedure SetaOpeALT(const PControlador: TWinControl);
begin
  FG.SetaOpe(PControlador, 'Alterar');
end;

{------------------------------------------------------------------------------}
function PreparaAnoMes(const Data_Ou_MesAno: string): string;
begin
  if FH.DataOk(Data_Ou_MesAno) then
    Result := FormatDateTime('yyyy/mm', StrToDate(Data_Ou_MesAno))
  else
    Result := FH.InverteMesAno(Data_Ou_MesAno);
end;

{------------------------------------------------------------------------------}
function PreparaMesAno(const Data_Ou_AnoMes: string): string;
begin
  if FH.DataOk(Data_Ou_AnoMes) then
    Result := FormatDateTime('mm/yyyy', StrToDate(Data_Ou_AnoMes))
  else
    Result := FH.InverteAnoMes(Data_Ou_AnoMes);
end;

{------------------------------------------------------------------------------}
function RetAnoMesSQL(const Data_Ou_MesAno: string): string;
begin
  if FH.DataOk(Data_Ou_MesAno) then
    Result := FormatDateTime('yyyy/mm', StrToDate(Data_Ou_MesAno))
  else
    Result := FH.InverteMesAno(Data_Ou_MesAno);
  Result := QuotedStr(Result);
end;

{------------------------------------------------------------------------------}
function VerificaMesAno(AEdit: TWinControl; const AMesAno: string): boolean;
begin
  Result := not FH.PrMsg_FocalizeSe(FH.NaoTemInteiro(AMesAno) or
                                   (not FH.IntOk(copy(AMesAno, 1, 1))) or
                                   (not FH.IntOk(copy(AMesAno, 2, 1))) or
                                   (not FH.IntOk(copy(AMesAno, 4, 1))) or
                                   (not FH.IntOk(copy(AMesAno, 5, 1))) or
                                   (not FH.IntOk(copy(AMesAno, 6, 1))) or
                                   (not FH.IntOk(copy(AMesAno, 7, 1))) or
                                   (not InRange(StrToInt(FH.CopyPos(AMesAno, DateSeparator)), 1, 12)),
                                   AEdit, '', true);
end;

{------------------------------------------------------------------------------}
function RetColunaExcel(const AIndex: Integer): string;
var
  I, J: integer;
  S: string;
  C: char;
begin
  I := Ord('A'); Dec(I);
  S := '';
  C := #0;
  for J := 1 to AIndex do
  begin
    Inc(I);
    S := Chr(I);
    if I = Ord('Z') then
    begin
      if J = AIndex then break;
      I := Ord('A'); Dec(I);
      if C = #0 then
        C := 'A' else C := Chr(Ord(C) + 1)
    end;
  end;
  if C <> #0 then S := C + S;
  Result := S;
end;

{------------------------------------------------------------------------------}
procedure LimpaComps(PContainer: TWinControl);
var
  I, J: Integer;
begin
  try
    with PContainer do
    begin
      for I := 0 to ControlCount - 1 do
      begin
        if Controls[I] is TLabel then
        begin
          if Controls[I].Tag = 9 then
            TLabel(Controls[I]).Caption := '';
        end
        else if Controls[I] is TStaticText then
        begin
          if Controls[I].Tag <> 9 then
            TStaticText(Controls[I]).Caption := '';
        end
        else if Controls[I] is TcxPageControl then
        begin
          for J := 0 to TcxPageControl(Controls[I]).PageCount - 1 do
            FG.LimpaComps(TcxPageControl(Controls[I]).Pages[J]);
        end
        else if Controls[I] is TPageControl then
        begin
          for J := 0 to TPageControl(Controls[I]).PageCount - 1 do
            FG.LimpaComps(TPageControl(Controls[I]).Pages[J]);
        end
        else if Controls[I] is TGroupBox       then FG.LimpaComps(TGroupBox(Controls[I]))
        else if Controls[I] is TScrollBox      then FG.LimpaComps(TScrollBox(Controls[I]))
        else if Controls[I] is TPanel          then FG.LimpaComps(TPanel(Controls[I]))
        else if Controls[I] is TTabSheet       then FG.LimpaComps(TTabSheet(Controls[I]))
        else if Controls[I] is TcxTabSheet     then FG.LimpaComps(TcxTabSheet(Controls[I]))
        else if Controls[I] is TEdit           then TEdit(Controls[I]).Clear
        else if Controls[I] is TSpinEdit       then TSpinEdit(Controls[I]).Value := 0
        else if Controls[I] is TJvSpinEdit     then TJvSpinEdit(Controls[I]).Value := 0
        else if Controls[I] is TcxSpinEdit     then TcxSpinEdit(Controls[I]).Value := 0
        else if Controls[I] is TCustomMaskEdit then TCustomMaskEdit(Controls[I]).Clear
        else if Controls[I] is TMemo           then TMemo(Controls[I]).Lines.Clear
        else if Controls[I] is TRichEdit       then TRichEdit(Controls[I]).Lines.Clear
        //else if Controls[I] is TJvMemo         then TJvMemo(Controls[I]).Lines.Clear
        else if Controls[I] is TImage          then TImage(Controls[I]).Picture.Assign(nil)
        else if Controls[I] is TCheckBox       then
        begin
          if Controls[I].Tag <> -9 then
            TCheckBox(Controls[I]).Checked := false;
        end
        else if Controls[I] is TComboBox       then
        begin
          if Controls[I].Tag = 9 then
            TComboBox(Controls[I]).Clear
          else
            TComboBox(Controls[I]).ItemIndex := -1
        end
        else if Controls[I] is TListBox       then
        begin
          if Controls[I].Tag <> 9 then
            TListBox(Controls[I]).Clear
          else
            TListBox(Controls[I]).ItemIndex := -1
        end
        else
        if Controls[I] is TCheckListBox   then
          FH.SetaStatusCheckListBox(false, TCheckListBox(Controls[I]))
        else
        if Controls[I] is TListView   then
          FH.SetaStatusListView(false, TListView(Controls[I]))
        else
      end;
    end;
  except
    //
  end;
end;

{------------------------------------------------------------------------------}
function LeConfigEstabe(const PEstabe: string): string;
var
  est: string;
begin
  Result := '';
  est    := PEstabe;
  if est  = '' then est := FG.RetEstabeAtivo;
  try
    if not FG.FindKey(DMG.qrLeConfigEstabe, 'ESTABE', [est]) then
      Exit;
    Result := DMG.qrLeConfigEstabe.FieldByName('CONFIG').AsString;
  finally
    DMG.qrLeConfigEstabe.Close;
  end;
end;

{------------------------------------------------------------------------------}
function VerificaComps(PContainer: TWinControl): boolean;
var
  I, J: Integer;
begin
  Result := true;
  try
    with PContainer do
    begin
      for I := 0 to ControlCount - 1 do
      begin
        if Controls[I] is TPageControl then
        begin
          for J := 0 to TPageControl(Controls[I]).PageCount - 1 do
            FG.VerificaComps(TPageControl(Controls[I]).Pages[J]);
        end
        else
        if Controls[I] is TcxPageControl then
        begin
          for J := 0 to TcxPageControl(Controls[I]).PageCount - 1 do
            FG.VerificaComps(TcxPageControl(Controls[I]).Pages[J]);
        end
        else
        if Controls[I] is TGroupBox then
          FG.VerificaComps(TGroupBox(Controls[I]))
        else
        if Controls[I] is TScrollBox then
          FG.VerificaComps(TScrollBox(Controls[I]))
        else
        if Controls[I] is TPanel then
          FG.VerificaComps(TPanel(Controls[I]))
        else
        if Controls[I] is TTabSheet then
          FG.VerificaComps(TTabSheet(Controls[I]))
        else
        if Controls[I] is TcxTabSheet then
          FG.VerificaComps(TcxTabSheet(Controls[I]))
        else
        if Controls[I] is TEdit then
        begin
          if TEdit(Controls[I]).Text = '' then
          begin
            FH.PrMsg_Focalize(TEdit(Controls[I]), FH.vrgDefErrMsg, true);
            Result := false;
            exit;
          end;
        end
        else
        if Controls[I] is TJvComboEdit then
        begin
          if TJvComboEdit(Controls[I]).Text = '' then
          begin
            FH.PrMsg_Focalize(TJvComboEdit(Controls[I]), FH.vrgDefErrMsg, true);
            Result := false;
            exit;
          end;
        end
        else
        if Controls[I] is TMaskEdit then
        begin
          if FH.NaoTemInteiro(TMaskEdit(Controls[I]).Text) then
          begin
            FH.PrMsg_Focalize(TMaskEdit(Controls[I]), FH.vrgDefErrMsg, true);
            Result := false;
            exit;
          end;
        end
        else
        if Controls[I] is TJvDateEdit then
        begin
          if FH.DataInvalida(TJvDateEdit(Controls[I]).Text) then
          begin
            FH.PrMsg_Focalize(TJvDateEdit(Controls[I]), FH.vrgDefErrMsg, true);
            Result := false;
            exit;
          end;
        end
        else
        if Controls[I] is TComboBox then
        begin
          if TComboBox(Controls[I]).ItemIndex < 0 then
          begin
            FH.PrMsg_Focalize(TComboBox(Controls[I]), FH.vrgDefErrMsg, true);
            Result := false;
            exit;
          end;
        end;
      end;
    end;
  except

  end;
end;

{------------------------------------------------------------------------------}
function LeIniStr(const PTexto, pID: string; const PDefault: string = ''): string;
var
  tmpLst: TStrings;
begin
  try
    FH.IniGera(tmpLst, PTexto);
    Result := FH.IniLeStr(tmpLst, FG.SECAO_GERAL, pID, PDefault);
  finally
    tmpLst.Free;
  end;
end;

{------------------------------------------------------------------------------}
function LeIniStrEx(const PTexto, PSecao, pID: string; const PDefault: string = ''): string;
var
  tmpLst: TStrings;
begin
  try
    FH.IniGera(tmpLst, PTexto);
    Result := FH.IniLeStr(tmpLst, PSecao, pID, PDefault);
  finally
    tmpLst.Free;
  end;
end;

{------------------------------------------------------------------------------}
function LeIniInt(const PTexto, pID: string; const PDefault: integer = 0): integer;
var
  tmpLst: TStrings;
begin
  try
    FH.IniGera(tmpLst, PTexto);
    Result := FH.IniLeInt(tmpLst, FG.SECAO_GERAL, pID, PDefault);
  finally
    tmpLst.Free;
  end;
end;

{------------------------------------------------------------------------------}
function LeIniIntEx(const PTexto, PSecao, pID: string; const PDefault: integer = 0): integer;
var
  tmpLst: TStrings;
begin
  try
    FH.IniGera(tmpLst, PTexto);
    Result := FH.IniLeInt(tmpLst, PSecao, pID, PDefault);
  finally
    tmpLst.Free;
  end;
end;

{------------------------------------------------------------------------------}
function LeIniNum(const PTexto, pID: string; const PDefault: double = 0): double;
var
  tmpLst: TStrings;
begin
  try
    FH.IniGera(tmpLst, PTexto);
    Result := FH.IniLeNum(tmpLst, FG.SECAO_GERAL, pID, PDefault);
  finally
    tmpLst.Free;
  end;
end;

{------------------------------------------------------------------------------}
function LeIniBool(const PTexto, pID: string; const PDefault: boolean = false): boolean;
var
  tmpLst: TStrings;
begin
  try
    FH.IniGera(tmpLst, PTexto);
    Result := FH.IniLeInt(tmpLst, FG.SECAO_GERAL, pID, Ord(PDefault)) <> 0;
  finally
    tmpLst.Free;
  end;
end;

{------------------------------------------------------------------------------}
//    Retorna a configuração do usuário
function LeConfigUsu(const AUsuario: string): string;
var
  tmpQuery: TIBODataSet;
  tx: string;
begin
  Result := '';
  tx := AUsuario;
  if tx = '' then tx := FG.RetUsuAtivo;
  try
    tmpQuery := TIBODataSet.Create(nil);
    with tmpQuery do
    begin
      IB_Connection := DMG.IB_Connection;
      SQL.Text := 'select config from usucadastro ' +
                  'where usuario = ' + QuotedStr(tx);
      Open;
      if IsEmpty then
        exit;
      Result := FieldByName('CONFIG').AsString;
    end;
  finally
    FH.DestroiObj(tmpQuery);
  end;
end;

{------------------------------------------------------------------------------}
procedure ChamaBuscaCL(CL: TCheckListBox; const PTipo: SmallInt = 0);
const
  cPrompt = 'Digite um código ou descrição para pesquisar na lista de marcações';
var
  S, vDes: string;
  I : Integer;
  C: TCheckListBox;

  function BuscaDesc: boolean;
  var
    jj: integer;
  begin
    Result := false;
    for jj := 0 to C.Items.Count - 1 do
    begin
      vDes := FH.RetDescriStr(C.Items[jj]);
      {
      vPos := Min(Length(S), Length(vDes));
      if (AnsiSameText(LeftStr(vDes, vPos), LeftStr(S, vPos)))
      or (AnsiSameText(vDes, S)) then
      }
      if AnsiContainsText(vDes, S) then
      begin
        C.ItemIndex := jj;
        Result := true;
        exit;
      end;
    end;
  end;

begin
  C := CL;
  if not FG.ChamaInputBox(FG.IBoxDef, 'Localizar', cPrompt, S) then
    exit;
  if FH.IntOk(S) then
  begin
    for I := 0 to C.Items.Count - 1 do
    begin
      case PTipo of
        2: // contas condominiais
          begin
            if Trim(FH.CopyPos(C.Items[I], '.')) = FH.StrZero(S, 3) then
            begin
              C.ItemIndex := I;
              exit;
            end;
          end;
        else // outros
          begin
            if Trim(FH.RetCodigoStr(C.Items[I])) = S then
            begin
              C.ItemIndex := I;
              exit;
            end;
          end;
      end;
    end;
  end
  else
  begin
    case PTipo of
      2: // conta condominial
        begin
          if Pos('.', S) > 0 then
          begin
            if Length(FH.CopyPos(S, '.')) < 3 then S := FH.StrZero(FH.CopyPos(S, '.'), 3) + '.' + FH.CopyPosMax(S, '.');
            if Length(FH.CopyPosMax(S, '.'))  < 3 then S := FH.CopyPos(S, '.') + '.' + FH.StrZero(FH.CopyPosMax(S, '.'), 3);
            for I := 0 to C.Items.Count - 1 do
            begin
              if Trim(FH.RetCodigoStr(C.Items[I])) = S then
              begin
                C.ItemIndex := I;
                exit;
              end;
            end;
          end
          else
            if BuscaDesc then exit;
        end;
      else // outros
        begin
          if BuscaDesc then exit;
        end;
    end;
  end;
  C.ItemIndex := -1;
end;

{------------------------------------------------------------------------------}
procedure FocalizeListView(LV: TListView; const AIndex: integer);
begin
  FH.Focalize(LV);
  if LV.Items.Count = 0 then
    Exit;
  if AIndex > (LV.Items.Count-1) then
    Exit;
  LV.ItemIndex := AIndex;
  LV.Items.Item[AIndex].MakeVisible(false);
  LV.Items.Item[AIndex].Selected := true;
  LV.Items.Item[AIndex].Focused  := true;
end;

{------------------------------------------------------------------------------}
procedure ChamaBuscaLV(LV: TListView; const PTipo: SmallInt = 0);
const
  cPrompt = 'Digite um código ou descrição para pesquisar na lista de marcações';
var
  S, vDes: string;
  I : Integer;
  tmpLv: TListView;

  procedure SetaFoco(index: integer);
  begin
    FG.FocalizeListView(tmpLv, index);
  end;

  function BuscaDesc: boolean;
  var
    jj: integer;
  begin
    Result := false;
    for jj := 0 to tmpLv.Items.Count - 1 do
    begin
      vDes := tmpLv.Items[jj].SubItems[0];
      {
      vPos := Min(Length(S), Length(vDes));
      if (AnsiSameText(LeftStr(vDes, vPos), LeftStr(S, vPos)))
      or (AnsiSameText(vDes, S)) then
      }
      if AnsiContainsText(vDes, S) then
      begin
        SetaFoco(jj);
        Result := true;
        exit;
      end;
    end;
  end;

begin
  tmpLv := LV;
  if not FG.ChamaInputBox(FG.IBoxDef, 'Localizar', cPrompt, S) then
    exit;
  if FH.IntOk(S) then
  begin
    for I := 0 to tmpLv.Items.Count - 1 do
    begin
      case PTipo of
        2: // contas condominiais
          begin
            if Trim(FH.CopyPos(tmpLv.Items[I].Caption, '.')) = FH.StrZero(S, 3) then
            begin
              SetaFoco(I);
              exit;
            end;
          end;
        else // outros
          begin
            if Trim(tmpLv.Items[I].Caption) = S then
            begin
              SetaFoco(I);
              exit;
            end;
          end;
      end;
    end;
  end
  else
  begin
    case PTipo of
      2: // conta condominial
        begin
          if Pos('.', S) > 0 then
          begin
            if Length(FH.CopyPos(S, '.')) < 3 then S := FH.StrZero(FH.CopyPos(S, '.'), 3) + '.' + FH.CopyPosMax(S, '.');
            if Length(FH.CopyPosMax(S, '.'))  < 3 then S := FH.CopyPos(S, '.') + '.' + FH.StrZero(FH.CopyPosMax(S, '.'), 3);
            for I := 0 to tmpLv.Items.Count - 1 do
            begin
              if Trim(tmpLv.Items[I].Caption) = S then
              begin
                SetaFoco(I);
                exit;
              end;
            end;
          end
          else
            if BuscaDesc then
              exit;
        end;
      else // outros
        begin
          if BuscaDesc then
            exit;
        end;
    end;
  end;
  tmpLv.ItemIndex := -1;
end;

{------------------------------------------------------------------------------}
function verCheckComboBox(CX: TcxCheckComboBox; Index: Integer): Boolean;
begin
     Result := CX.GetItemState(Index) = cbsChecked;
end;

{------------------------------------------------------------------------------}
function verCheckComboBox(CX: TcxCheckComboBox): Boolean;
var i : Integer;
begin
     Result := False;
     for i := 0 to CX.Properties.Items.Count -1 do
     begin
          Result := CX.GetItemState(i) = cbsChecked;
          if Result then
             Exit;
     end;
end;

{------------------------------------------------------------------------------}
procedure setaCheckComboBox(CX: TcxCheckComboBox; Index: Integer; Status: Boolean);
begin
     if Status then
        CX.SetItemState(Index, cbsChecked)
     else
        CX.SetItemState(Index, cbsUnchecked);
end;

{------------------------------------------------------------------------------}
procedure setaCheckComboBox(CX: TcxCheckComboBox; Status: Boolean);
var i : Integer;
begin
     for i := 0 to CX.Properties.Items.Count -1 do
         if Status then
            CX.SetItemState(i, cbsChecked)
         else
            CX.SetItemState(i, cbsUnchecked);
end;

{------------------------------------------------------------------------------}
function GeraSQLINCheckComboBox(CX: TcxCheckComboBox; const ComAspas, ExtraiCodigo: Boolean; const TamCod: Integer = -1; const Separador: string = ' '): string;
var
    jj : integer;
    ss : string;
begin
    Result    := '';
    for jj := 0 to CX.Properties.Items.Count - 1 do
    begin
        if CX.GetItemState(jj) <> cbsChecked then
           Continue;
        if Result <> '' then
           Result := Result + ',';
        ss := CX.Properties.Items[jj].Description;
        if ExtraiCodigo then
        begin
            ss := FH.RetCodigoStr(ss,Separador);
        end;
        if TamCod > 0 then
           ss := Copy(ss, 1, TamCod);
        if ComAspas then
           Result := Result + QuotedStr(ss)
        else
           Result := Result + ss;
    end;
    if Result <> '' then
       Result := FH.EntrePar(Result);
end;

function GeraSQLINCheckComboBox(CX: TcxCheckComboBox): string;
begin
    Result := FG.GeraSQLINCheckComboBox(CX, False, True);
end;

{------------------------------------------------------------------------------}
procedure CarregaCheckComboBox(CX: TcxCheckComboBox; DS: TDataSet; const PCampos: string = 'CODIGO,DESCRICAO'; const PSeparador: string = ' ');
var
    ii      : Integer;
    tmpStr  ,
    vCampos : string;
    L       : TStrings;
begin
    CX.Properties.Items.Clear;
    if DS.IsEmpty then
       Exit;
    try
        FH.CriaLista(L);
        vCampos := PCampos;
        if Pos(';', vCampos) > 0 then
           vCampos := StringReplace(vCampos, ';', ',', [rfReplaceAll]);
        L := FH.StrParaLista(vCampos);
        DS.First;
        while not DS.Eof do
        begin
            tmpStr := '';
            for ii := 0 to L.Count - 1 do
            begin
                 if ii > 0 then
                    tmpstr := tmpstr + PSeparador;
                 tmpStr := tmpStr + DS.FieldByName(Trim(L[ii])).AsString;
            end;
            CX.Properties.Items.AddCheckItem(TrimRight(tmpStr));
            DS.Next;
        end;
        DS.First;
    finally
        FH.DestroiObj(L);
    end;
end;

{-----------------------------------------------------------------------------}
function GeraSQLINCL(CL: TCheckListBox; const ComAspas: boolean = false): string;
var
  jj: integer;
begin
  Result := '';
  for jj := 0 to CL.Items.Count - 1 do
  begin
    if CL.Checked[jj] then
    begin
      if Result <> '' then Result := Result + ',';
      if ComAspas then
        Result := Result + QuotedStr(FH.RetCodigoStr(CL.Items[jj]))
      else
        Result := Result + FH.RetCodigoStr(CL.Items[jj]);
    end;
  end;
  if Result <> '' then
    Result := FH.EntrePar(Result);
end;

{------------------------------------------------------------------------------}
function GeraSQLINLV(LV: TListView; const ComAspas: boolean = false): string;
var
  jj: integer;
begin
  Result := '';
  for jj := 0 to LV.Items.Count - 1 do
  begin
    if LV.Items[jj].Checked then
    begin
      if Result <> '' then Result := Result + ',';
      if ComAspas then
        Result := Result + QuotedStr(LV.Items[jj].Caption)
      else
        Result := Result + LV.Items[jj].Caption;
    end;
  end;
  if Result <> '' then
    Result := FH.EntrePar(Result);
end;

{------------------------------------------------------------------------------}
function MsgMono(const AMsg: string; const Msg_ID: TIconeMsg; const ATamFonte: Integer = 10;
  const ANegrito: boolean = false): Integer;
var
  tmpMsg: TGrupoBotaoMsg;
  tmpStyle: TFontStyles;
begin
  tmpMsg := [mbtnOk];
  if Msg_ID = imConf then
    tmpMsg := [mbtnYes, mbtnNo];
  if ANegrito then
    tmpStyle := [fsBold]
  else
    tmpStyle := [];
  Result := FH.DispMsgPersonalizada(
                    AMsg, Msg_ID, tmpMsg,
                    FH.MontaFonte('Courier', ATamFonte, clBlack, tmpStyle),
                    FH.MontaFonte(FG.FONTE_TAHOMA, 8, clBlack, []));
end;

{------------------------------------------------------------------------------}
procedure SetaPropColOrdena(AColuna: TColumn);
begin
  if AColuna <> nil then
  begin
    AColuna.Title.Font.Color := $00A00000;
    AColuna.Title.Font.Style := [fsUnderline, fsBold];
    AColuna.Color            := $00E8FFFF;
  end;
end;

{------------------------------------------------------------------------------}
procedure OrdenaGrid(GR: TDBGrid; AColuna: TColumn; const PintarColuna: boolean = true);
var
  ii: Integer;
  D: TDataSet;
begin
  try
    D := GR.DataSource.DataSet;
    if D is TClientDataSet then
      (D as TClientDataSet).IndexFieldNames := AColuna.FieldName
    else
    if D is TIBOTable then
      (D as TIBOTable).IndexFieldNames := AColuna.FieldName
    else
    if D is TIBOQuery then
    begin
      with (D as TIBOQuery) do
        if OrderingItems.Count > 0 then
        begin
          for ii := 0 to Fields.Count - 1 do
            if SameText(AColuna.FieldName, Fields[ii].FieldName) then
            begin
              try
                FH.SetaCursorAmp;
                OrderingItemNo := ii + 1;
                break;
              finally
                FH.SetaCursorDef;
              end;
            end;
        end;
    end;
    if PintarColuna then
      try
        for ii := 0 to GR.Columns.Count - 1 do
        begin
          with GR.Columns[ii].Title.Font do
          begin
            Color := clWindowText;
            Style := [];
          end;
          GR.Columns[ii].Color := GR.Color;
        end;

        FG.SetaPropColOrdena(AColuna);
      except
        //
      end;
  except
    //
  end;
end;

{------------------------------------------------------------------------------}
procedure OrdenaGrid(AColuna: TColumn; const PintarColuna: boolean = true);
begin
  FG.OrdenaGrid(TDBGrid(AColuna.Grid), AColuna, PintarColuna);
end;

{------------------------------------------------------------------------------}
procedure SetaQueryOrder(PQuery: TIBOQuery);
var
  ii: integer;
  lsi, lsl: TStrings;
begin
  with PQuery do
  begin
    OrderingItems.Clear;
    OrderingLinks.Clear;
    if FieldDefs.Count > 0 then
    begin
      try
        FH.CriaLista(lsi);
        FH.CriaLista(lsl);
        for ii := 0 to pred(FieldDefs.Count) do
        begin
          // referencia a coluna pelo número // + NULLS FIRST para colocar os nulos primeiro
          lsi.Add(FieldDefs[ii].Name + '=' + intToStr(ii + 1));
          lsl.Add(intToStr(ii + 1) + '=' + intToStr(ii + 1));
        end;
        OrderingItems.Text := lsi.Text;
        OrderingLinks.Text := lsl.Text;
      finally
        FH.DestroiObj(lsi);
        FH.DestroiObj(lsl);
      end;
    end;
  end;
end;

{------------------------------------------------------------------------------}
function DataSql(ADataStr: string): string;
begin
  Result := QuotedStr(StringReplace(ADataStr, '/', '.', [rfReplaceAll]));
end;

{------------------------------------------------------------------------------}
function DataSql(AData: TDateTime): string;
begin
  Result := FG.DataSql(DateToStr(AData));
end;

{------------------------------------------------------------------------------}
function FormataCHR(const ACodigo: string): string;
var
  I, K: Byte;
  Barra: string;
begin
  I := 1;
  Barra := '';
  while I < 44 do
  begin
    K := StrToInt(Copy(ACodigo, I, 2));
    Barra := Barra + chr(StrToInt(TabChr[K]));
    Inc(I, 2);
  end;
  Result := Chr(40) + Barra + Chr(41);
end;

{------------------------------------------------------------------------------}
function MontaBarra25(AUsaTab: boolean; ABan, AMoe, AVen, AVal, AVar: string;
  var ABar: string): string;
var
  vBarTxt,  vStrAux: string;
  vDatAux: TDate;
  vNumAux: double;
const
  vFatVen: string = '07/10/1997';

  procedure FormataComTabela;
  var
    I, J, K: Byte;
    Barra: string[120];
  begin
    Barra[0] := #0;
    I := 1;
    J := 1;
    while I < 44 do
    begin
      K := StrToInt(Copy(vBarTxt, I, 2));
      Insert(TabConv[K], Barra, ((J - 1) * 5) + 1);
      Inc(I, 2);
      Inc(J);
    end;
    Result := Inicio + Barra + Final;
  end;

begin
  {-- Formação da barra
       ABan = (3 dig) banco  = codigo do banco sem DV
       AMoe = (1 dig) Moeda  = 9 é REAL e se # valor deve ser zerado
       AVen = (4 dig) Fator vencto  = zeros ou diferenca de dias apartir de 03/07/2000
       AVal = (10dig) valor  = zeros ou valor sem virgula com dois decimais
       AVar = (24Dig) Variavel para cada banco (produto+agencia+nossNro etc)
       ABar = Retorna a Barra completa com 44 pos ou "" se ocorrer erro
   --}
  ABar := '';

  if not Length(ABan) = 3 then
  begin
    Result := 'ERRO1 = Banco Invalido';
    exit;
  end;
  if not Length(AMoe) = 1 then
  begin
    Result := 'ERRO2 = Moeda Invalida';
    exit;
  end;
  if not Length(AVar) = 25 then
  begin
    Result := 'ERRO3 = Digitos do campo livre menor que 25 = ' + IntToStr(Length(AVar));
    exit;
  end;

  try
    vDatAux := StrToDate(AVen);
    AVen := '0000';
    if vDatAux > StrToDate(vFatVen) then
      AVen := FloatToStr(vDatAux - StrToDate(vFatVen));
    AVen := FH.PreencherEsq(AVen, '0', 4);
  except
    AVen := '0000';
  end;

  try
    vNumAux := StrToFloat(AVal);
    AVal := FH.PreencherEsq(AVal, '0', 10);
  except
    if AMoe = '9' then
    begin
      Result := 'ERRO4 = Valor Invalido';
      exit;
    end;
    AVal := '0000000000';
  end;


  vBarTxt := ABan + AMoe + AVen + AVal + AVar;

  if Length(vBarTxt) < 43 then
  begin
    Result := 'ERRO5 = Digitos da barra menor que 44 = ' + IntToStr(Length(vBarTxt));
    exit;
  end;

  vStrAux := FH.Modulo11(vBarTxt);
  if vStrAux = '0' then { DAC Não pode ser = zero}
    insert('1', vBarTxt, 5)
  else
    insert(vStrAux, vBarTxt, 5);

  ABar   := vBarTxt;
  
  Result := '';

(* OBSOLETO

  {  Intervaled 2 de 5 }
  if AUsaTab then
    formataComTabela
  { fuga Intervaled 2 de 5 }
  else
    Result := FormataCHR(vBarTxt);
*)

end;

{------------------------------------------------------------------------------}
function LeConfigLocalStr(const pID: string; const PDefault: string = ''): string;
begin
  Result := FH.IniLeStr(DMG.vpbConfigLocal, FG.SECAO_GERAL, pID, PDefault);
end;

{------------------------------------------------------------------------------}
function LeConfigLocalBool(const pID: string; const PDefault: boolean = false): boolean;
begin
  Result := FH.IniLeBln(DMG.vpbConfigLocal, FG.SECAO_GERAL, pID, PDefault);
end;

{------------------------------------------------------------------------------}
function LeConfigLocalInt(const pID: string; const PDefault: integer = 0): integer;
begin
  Result := FH.IniLeInt(DMG.vpbConfigLocal, FG.SECAO_GERAL, pID, PDefault);
end;

{------------------------------------------------------------------------------}
function LeConfigLocalNum(const pID: string; const PDefault: double = 0): double;
begin
  Result := FH.IniLeNum(DMG.vpbConfigLocal, FG.SECAO_GERAL, pID, PDefault);
end;

{------------------------------------------------------------------------------}
procedure GravaConfigLocalStr(const pID, PValor: string);
var
  lst: TStrings;
begin
  try
    FH.IniGera(lst, DMG.vpbConfigLocal.Text);
    FH.IniGravaStr(lst, FG.SECAO_GERAL, pID, PValor);

    lst.SaveToFile(FG.RetArqCfgLocal);
    DMG.vpbConfigLocal.Text := lst.Text;
  finally
    FH.DestroiObj(lst);
  end;
end;

{------------------------------------------------------------------------------}
procedure ChamaCadastroCli(FR: TForm; const ACodCli: string);
var
 aqry: TIBOQuery;
begin
   try
     fg.criaiboquery(aqry);
     aqry.SQL.Text := 'SELECT CODIGO, '+
                      '       ORDEM, '+
                      '       NOME,  '+
                      '       TIPO,  '+
                      '       PESSOA '+
                      ' FROM CLICADASTRO '+
                      ' WHERE CODIGO = ' + fgcli.RetCliCodigo(ACodCli) +
                      '   AND ORDEM = ' + QuotedStr(fgcli.RetCliOrdem(ACodCli));
     aqry.Open;

     FG.GravaCacheStr('Cli_Cadastro_Ope', 'A');
     FG.GravaCacheInt('Cli_Cadastro_Cod', aQry.FieldByName('CODIGO').AsInteger);
     FG.GravaCacheStr('Cli_Cadastro_Tip', aQry.FieldByName('TIPO').AsString);
     FG.GravaCacheStr('Cli_Cadastro_Ord', aQry.FieldByName('ORDEM').AsString);

     if SameText(aQry.FieldByName('PESSOA').AsString, 'J') then
       FG.ChamaForm(fr, '_TFCliCadastroJur', FG.RetFormCaption('_TFCliCadastro') + ' ( Jurídico )', 0)
     else
       FG.ChamaForm(fr, '_TFCliCadastroPF', FG.RetFormCaption('_TFCliCadastro') + ' ( Físico )', 0);
  finally
    fh.DestroiObj(aqry);
  end;
end;

{------------------------------------------------------------------------------}
procedure ChamaCadastroFor(FR: TForm; const ACodFor: string);
var
 aqry: TIBOQuery;
begin
   try
     fg.criaiboquery(aqry);
     aqry.SQL.Text := 'SELECT CODIGO, '+
                      '       NOME  '+
                      ' FROM FORCADASTRO '+
                      ' WHERE CODIGO = ' + ACodFor;
     AQry.Open;

     FG.GravaCacheInt('[ForCadastro]:Codigo', aQry.FieldByName('CODIGO').AsInteger);
     FG.ChamaForm(FR, '_TFForCadastro', FG.RetFormCaption('_TFForCadastro'), 0)
  finally
    fh.DestroiObj(aqry);
  end;
end;

{------------------------------------------------------------------------------}
procedure GravaConfigLocalBool(const pID: string; const PValor: boolean);
var
  lst: TStrings;
begin
  try
    FH.IniGera(lst, DMG.vpbConfigLocal.Text);
    FH.IniGravaBln(lst, FG.SECAO_GERAL, pID, PValor);

    lst.SaveToFile(FG.RetArqCfgLocal);
    DMG.vpbConfigLocal.Text := lst.Text;
  finally
    FH.DestroiObj(lst);
  end;
end;

{------------------------------------------------------------------------------}
procedure GravaConfigLocalInt(const pID: string; const PValor: integer);
var
  lst: TStrings;
begin
  try
    FH.IniGera(lst, DMG.vpbConfigLocal.Text);
    FH.IniGravaInt(lst, FG.SECAO_GERAL, pID, PValor);

    lst.SaveToFile(FG.RetArqCfgLocal);
    DMG.vpbConfigLocal.Text := lst.Text;
  finally
    FH.DestroiObj(lst);
  end;
end;

{------------------------------------------------------------------------------}
procedure GravaConfigLocalNum(const pID: string; const PValor: double);
var
  lst: TStrings;
begin
  try
    FH.IniGera(lst, DMG.vpbConfigLocal.Text);
    FH.IniGravaNum(lst, FG.SECAO_GERAL, pID, PValor);

    lst.SaveToFile(FG.RetArqCfgLocal);
    DMG.vpbConfigLocal.Text := lst.Text;
  finally
    FH.DestroiObj(lst);
  end;
end;

{------------------------------------------------------------------------------}
function LeConfigEstabeStr(const pID: string; const PDefault: string = ''): string;
begin
  Result := FH.IniLeStr(DMG.vpbConfigEstabe, FG.SECAO_GERAL, pID, PDefault);
end;

{------------------------------------------------------------------------------}
function LeConfigEstabeBool(const pID: string; const PDefault: boolean = false): boolean;
begin
  Result := FH.IniLeBln(DMG.vpbConfigEstabe, FG.SECAO_GERAL, pID, PDefault);
end;

{------------------------------------------------------------------------------}
function LeConfigEstabeInt(const pID: string; const PDefault: integer = 0): integer;
begin
  Result := FH.IniLeInt(DMG.vpbConfigEstabe, FG.SECAO_GERAL, pID, PDefault);
end;

{------------------------------------------------------------------------------}
function LeConfigEstabeNum(const pID: string; const PDefault: double = 0): double;
begin
  Result := FH.IniLeNum(DMG.vpbConfigEstabe, FG.SECAO_GERAL, pID, PDefault);
end;

{------------------------------------------------------------------------------}
procedure GravaConfigEstabeStr(const pID, PValor: string; TR: TIB_Transaction = nil);
var
  lst: TStrings;
  tmpQuery: TIBOQuery;
begin
  try
    tmpQuery := TIBOQuery.Create(nil);
    tmpQuery.IB_Connection := DMG.IB_Connection;

    if TR <> nil then
      tmpQuery.IB_Transaction := TR;

    tmpQuery.SQL.Text := 'select * from cfgestabe where estabe = ' + QuotedStr(FG.RetEstabeAtivo);
    tmpQuery.Open;

    FH.IniGera(lst, tmpQuery.FieldByName('CONFIG').AsString);
    FH.IniGravaStr(lst, FG.SECAO_GERAL, pID, PValor);

    tmpQuery.Close;
    tmpQuery.SQL.Text := 'update cfgestabe set config = ' + QuotedStr(lst.Text) +
                         ' where estabe = ' + QuotedStr(FG.RetEstabeAtivo);
    tmpQuery.ExecSQL;

    DMG.vpbConfigEstabe.Text := lst.Text;
  finally
    FH.DestroiObj(lst);
    FH.DestroiObj(tmpQuery);
  end;
end;

{------------------------------------------------------------------------------}
procedure RecarregaConfigEstabe;
var
  lst: TStrings;
  tmpQuery: TIBOQuery;
begin
  try
    tmpQuery := TIBOQuery.Create(nil);
    tmpQuery.IB_Connection := DMG.IB_Connection;

    tmpQuery.SQL.Text := 'select * from cfgestabe where estabe = ' + QuotedStr(FG.RetEstabeAtivo);
    tmpQuery.Open;

    FH.IniGera(lst, tmpQuery.FieldByName('CONFIG').AsString);

    DMG.vpbConfigEstabe.Text := lst.Text;
  finally
    FH.DestroiObj(lst);
    FH.DestroiObj(tmpQuery);
  end;
end;

{------------------------------------------------------------------------------}
function LeConfigUsuStr(const pID: string; const PDefault: string = ''): string;
begin
  Result := FH.IniLeStr(DMG.vpbConfigUsuario, FG.SECAO_GERAL, pID, PDefault);
end;

{------------------------------------------------------------------------------}
function LeConfigUsuBool(const pID: string; const PDefault: boolean): boolean;
begin
  Result := FH.IniLeBln(DMG.vpbConfigUsuario, FG.SECAO_GERAL, pID, PDefault);
end;

{------------------------------------------------------------------------------}
function LeConfigUsuInt(const pID: string; const PDefault: integer = 0): integer;
begin
  Result := FH.IniLeInt(DMG.vpbConfigUsuario, FG.SECAO_GERAL, pID, PDefault);
end;

{------------------------------------------------------------------------------}
function LeConfigUsuNum(const pID: string; const PDefault: double = 0): double;
begin
  Result := FH.IniLeNum(DMG.vpbConfigUsuario, FG.SECAO_GERAL, pID, PDefault);
end;

{------------------------------------------------------------------------------}
procedure GravaConfigUsuStr(const pID, PValor: string; TR: TIB_Transaction = nil);
var
  lst: TStrings;
  tmpQuery: TIBOQuery;
begin
  try
    tmpQuery := TIBOQuery.Create(nil);
    tmpQuery.IB_Connection := DMG.IB_Connection;

    if TR <> nil then
      tmpQuery.IB_Transaction := TR;

    tmpQuery.SQL.Text := 'select * from usucadastro where usuario = ' +
                         QuotedStr(FG.RetUsuAtivo);
    tmpQuery.Open;

    FH.IniGera(lst, tmpQuery.FieldByName('CONFIG').AsString);
    FH.IniGravaStr(lst, FG.SECAO_GERAL, pID, PValor);

    tmpQuery.Close;
    tmpQuery.SQL.Text := 'update usucadastro set config = ' + QuotedStr(lst.Text) +
                         ' where usuario = ' + QuotedStr(FG.RetUsuAtivo);
    tmpQuery.ExecSQL;

    DMG.vpbConfigUsuario.Text := lst.Text;
  finally
    FH.DestroiObj(lst);
    FH.DestroiObj(tmpQuery);
  end;
end;

{------------------------------------------------------------------------------}
function LeConfigVenPedido: string;
var
  tmpQuery: TIBODataSet;
begin
  Result := '';
  try
    tmpQuery := TIBODataSet.Create(nil);
    with tmpQuery do
    begin
      IB_Connection := DMG.IB_Connection;
      SQL.Text := 'select config from venpedidocfg';
      Open;
      if IsEmpty then
        exit;
      Result := FieldByName('CONFIG').AsString;
    end;
  finally
    FH.DestroiObj(tmpQuery);
  end;
end;

{------------------------------------------------------------------------------}
function LeConfigVenPedidoStr(const pID: string; const PDefault: string = ''): string;
begin
  Result := FH.IniLeStr(DMG.vpbConfigVenPedido, FG.SECAO_GERAL, pID, PDefault);
end;

{------------------------------------------------------------------------------}
function LeConfigVenPedidoBool(const pID: string; const PDefault: boolean): boolean;
begin
  Result := FH.IniLeBln(DMG.vpbConfigVenPedido, FG.SECAO_GERAL, pID, PDefault);
end;

{------------------------------------------------------------------------------}
function LeConfigVenPedidoInt(const pID: string; const PDefault: integer = 0): integer;
begin
  Result := FH.IniLeInt(DMG.vpbConfigVenPedido, FG.SECAO_GERAL, pID, PDefault);
end;

{------------------------------------------------------------------------------}
function LeConfigVenPedidoNum(const pID: string; const PDefault: double = 0): double;
begin
  Result := FH.IniLeNum(DMG.vpbConfigVenPedido, FG.SECAO_GERAL, pID, PDefault);
end;

{------------------------------------------------------------------------------}
procedure GravaConfigVenPedidoStr(const pID, PValor: string; TR: TIB_Transaction = nil);
var
  lst: TStrings;
  tmpQuery: TIBOQuery;
begin
  try
    tmpQuery := TIBOQuery.Create(nil);
    tmpQuery.IB_Connection := DMG.IB_Connection;

    if TR <> nil then
      tmpQuery.IB_Transaction := TR;

    tmpQuery.SQL.Text := 'select * from venpedidocfg';
    tmpQuery.Open;

    FH.IniGera(lst, tmpQuery.FieldByName('CONFIG').AsString);
    FH.IniGravaStr(lst, FG.SECAO_GERAL, pID, PValor);

    tmpQuery.Close;
    tmpQuery.SQL.Text := 'update venpedidocfg set config = ' + QuotedStr(lst.Text);
    tmpQuery.ExecSQL;

    DMG.vpbConfigVenPedido.Text := lst.Text;
  finally
    FH.DestroiObj(lst);
    FH.DestroiObj(tmpQuery);
  end;
end;

{------------------------------------------------------------------------------}
function LeConfigNFStr(const pID: string; const PDefault: string = ''): string;
begin
  Result := FH.IniLeStr(DMG.vpbConfigNotaFiscal, FG.SECAO_GERAL, pID, PDefault);
end;

{------------------------------------------------------------------------------}
function LeConfigNFBool(const pID: string; const PDefault: boolean = false): boolean;
begin
  Result := FH.IniLeBln(DMG.vpbConfigNotaFiscal, FG.SECAO_GERAL, pID, PDefault);
end;

{------------------------------------------------------------------------------}
function LeConfigNFInt(const pID: string; const PDefault: integer = 0): integer;
begin
  Result := FH.IniLeInt(DMG.vpbConfigNotaFiscal, FG.SECAO_GERAL, pID, PDefault);
end;

{------------------------------------------------------------------------------}
function LeConfigNFNum(const pID: string; const PDefault: double = 0): double;
begin
  Result := FH.IniLeNum(DMG.vpbConfigNotaFiscal, FG.SECAO_GERAL, pID, PDefault);
end;

{------------------------------------------------------------------------------}
procedure GravaConfigNFStr(const PEstabe, PNome, pID, PValor: string; TR: TIB_Transaction = nil);
var
  lst: TStrings;
  tmpQuery: TIBOQuery;
begin
  try
    tmpQuery := TIBOQuery.Create(nil);
    tmpQuery.IB_Connection := DMG.IB_Connection;

    if TR <> nil then
      tmpQuery.IB_Transaction := TR;

    tmpQuery.SQL.Text := ' select * from cfgnf ' +
                         ' where estabe = ' + QuotedStr(PEstabe) +
                         '   and nome   = ' + QuotedStr(PNome);
    tmpQuery.Open;
    if tmpQuery.IsEmpty then
      exit;

    FH.IniGera(lst, tmpQuery.FieldByName('CONFIG').AsString);
    FH.IniGravaStr(lst, FG.SECAO_GERAL, pID, PValor);

    tmpQuery.Close;
    tmpQuery.SQL.Text := ' update cfgnf set config = ' + QuotedStr(lst.Text) +
                         ' where estabe = ' + QuotedStr(PEstabe) +
                         '   and nome   = ' + QuotedStr(PNome);
    tmpQuery.ExecSQL;

    DMG.vpbConfigNotaFiscal.Text := lst.Text;
  finally
    FH.DestroiObj(lst);
    FH.DestroiObj(tmpQuery);
  end;
end;

{------------------------------------------------------------------------------}
function Modulo11Bradesco(const S: string): string;
var
  ii, Peso, Soma: Integer;
begin
  Result := '';
  Soma := 0;
  Peso := 2;
  for ii := Length(S) downto 1 do
  begin
    Soma := Soma + StrToInt(Copy(S, ii, 1)) * Peso;
    if Peso = 7 then
      Peso := 2
    else
      Inc(Peso);
  end;
  Soma := 11 - (Soma mod 11);

       if Soma = 10 then Result := 'P'
  else if Soma = 11 then Result := '0'
                    else Result := IntToStr(Soma);
end;

{------------------------------------------------------------------------------}
procedure AtivaAnotacao(const FR: string);
var
  S,vCaption: string;
begin
  S := FR;
  if AnsiContainsText(S, 'SISANOTA')
  or AnsiContainsText(S, 'FSISDICA') then
     Exit;

  if AnsiContainsText(S, 'FMENU') then
  begin
    FMenu.btMenuAnota.Enabled := False;
    FMenu.btMenuAnota.Hint    := '';
    Exit;
  end;

  if Pos('_T', S) = 0 then
  begin
    S := IfThen(LeftStr(S, 1) <> 'T', 'T') + S;
    S := IfThen(LeftStr(S, 1) <> '_', '_') + S;
  end;
  FG.GravaCacheStr(FG.DEF_ANOTA, S);

  if AnsiContainsText(S, 'SISANOTA') then
    Exit;

  vCaption := FG.RetFormCaption(FG.LeCacheStr(FG.DEF_ANOTA));
  if AnsiContainsText(vCaption, '-') then
     vCaption := Trim(FH.CopyPosMax(vCaption, '-'));
  vCaption := QuotedStr(vCaption);

  if S = '_TFAtributo' then
    vCaption := QuotedStr('Atributo');
  try
    FMenu.btMenuAnota.Enabled := True;
    DMG.qrSisAnota.Close;
    DMG.qrSisAnota.ParamByName('FORM').AsString := S;
    DMG.qrSisAnota.Open;
    FMenu.btMenuAnota.ShowHint := True;

    if DMG.qrSisAnota.IsEmpty then
    begin
      FMenu.btMenuAnota.ImageIndex := 25;
      FMenu.btMenuAnota.Hint       := 'Não há anotações para ' + vCaption;
    end
    else
    begin
      FMenu.btMenuAnota.ImageIndex := 12;
      FMenu.btMenuAnota.Hint       := 'Há anotações para ' + vCaption;
    end;
  finally
    DMG.qrSisAnota.Close;
  end;
end;

{------------------------------------------------------------------------------}
procedure SetaJFS(FR: TForm; AJfs: TJvFormStorage; const Sufixo: string = '');
begin
  if AJfs.AppStorage <> nil then
  begin
    (AJfs.AppStorage as TJvAppIniFileStorage).Location := flCustom;
    (AJfs.AppStorage as TJvAppIniFileStorage).FileName := FG.SetaLocalIni + FR.Name + Sufixo + '.ini';
  end;
end;

{------------------------------------------------------------------------------}
function RetValorGenerator(const AGenerator: string): integer;
var
  tmpQr: TIBOQuery;
begin
  Result := 0;
  try
    tmpQr := TIBOQuery.Create(nil);
    with tmpQr do
    begin
      IB_Connection := DMG.IB_Connection;
      Sql.Text      := 'select gen_id("' + AGenerator + '", 1) as valor ' +
                       'from rdb$database';
      open;
      Result := FieldByName('VALOR').AsInteger;
      close;
    end;
  finally
    FH.DestroiObj(tmpQr);
  end;
end;

{------------------------------------------------------------------------------}
function TabelaExiste(const Nome: string): Boolean;
var
  tmpQr: TIBOQuery;
begin
  Result := false;
  try
    tmpQr := TIBOQuery.Create(nil);
    with tmpQr do
    begin
      IB_Connection := DMG.IB_Connection;
      Sql.Text      := 'select 1 from rdb$relations where rdb$relation_name = '+FH.RetValSql(Nome);
      open;
      Result := not isempty;
      close;
    end;
  finally
    FH.DestroiObj(tmpQr);
  end;
end;

{------------------------------------------------------------------------------}
function RetEstabeRel: string;
begin
  Result := FG.RetEstabeAtivoNome;
end;

{------------------------------------------------------------------------------}
function LeDataServSQL: string;
var
  tmpCursor: TIB_Cursor;
begin
  Result := '';
  try
    tmpCursor := TIB_Cursor.Create(nil);
    with tmpCursor do
    begin
      IB_Connection := DMG.IB_Connection;
      SQL.Text := 'select current_date from rdb$database';
      Open;
      if RecordCount > 0 then
        Result := FieldByName('current_date').AsString;
    end;
  finally
    FH.DestroiObj(tmpCursor);
  end;
end;

{------------------------------------------------------------------------------}
function DataStrLocal: string;
begin
  Result := DateToStr(SysUtils.Date);
end;

{------------------------------------------------------------------------------}
function DataLocal: TDateTime;
begin
  Result := DateOf(SysUtils.Date);
end;

{------------------------------------------------------------------------------}
function LeHoraServSQL: string;
var
  tmpCursor: TIB_Cursor;
begin
  Result := '';
  try
    tmpCursor := TIB_Cursor.Create(nil);
    with tmpCursor do
    begin
      IB_Connection := DMG.IB_Connection;
      SQL.Text := 'select current_time from rdb$database';
      Open;
      if RecordCount > 0 then
        Result := FieldByName('current_time').AsString;
    end;
  finally
    FH.DestroiObj(tmpCursor);
  end;
end;

{------------------------------------------------------------------------------}
function LeDataHoraServSQL: string;
var
  tmpCursor: TIB_Cursor;
begin
  Result := '';
  try
    tmpCursor := TIB_Cursor.Create(nil);
    with tmpCursor do
    begin
      IB_Connection := DMG.IB_Connection;
      SQL.Text := 'select current_timestamp from rdb$database';
      Open;
      if RecordCount > 0 then
        Result := FieldByName('current_timestamp').AsString;
    end;
  finally
    FH.DestroiObj(tmpCursor);
  end;
end;

{------------------------------------------------------------------------------}
procedure StrConvIniFile(var AIniFile: TINIFile; AStrings: TStrings; AStr: string);
var
  A: string;
begin
  A := FG.SetaLocalIni + 'tempERP.ini';
  if AStrings = nil then
    AStrings := TStringList.Create;
  AStrings.Text := AStr;
  if FileExists(A) then
    SysUtils.DeleteFile(A);
  try
    AStrings.SaveToFile(A);
  except
    if FileExists(A) then
      SysUtils.DeleteFile(A);
    AStrings.SaveToFile(A);
  end;
  AIniFile := TINIFile.Create(A);
end;

{------------------------------------------------------------------------------}
procedure GeraIniFile(ACriar: boolean; var AIniFile : TINIFile);
var
  A: string;
begin
  A := FG.SetaLocalini + 'tempERP.ini';
  if ACriar then
  begin
    try
      SysUtils.DeleteFile(A);
    except
    end;
    AIniFile := TIniFile.Create(A);
  end
  else
  begin
    AIniFile.Free;
    try
      SysUtils.DeleteFile(A);
    except
    end;
  end;
end;

{------------------------------------------------------------------------------}
function IniFileConvStr(var AIniFile: TINIFile; AStringList: TStrings): string;
var
  A: string;
begin
  AIniFile.Free;
  A := FG.SetaLocalIni + 'tempERP.ini';
  if AStringList = nil then
    AStringList := TStringList.Create;
  AStringList.LoadFromFile(A);
  try
    SysUtils.DeleteFile(A);
  except
  end;
  Result := AStringList.Text;
end;

{------------------------------------------------------------------------------}
procedure ChamaCnsSis(      FR                      : TForm;
                      const PTabela,
                            PCampoChave             : string;
                      const PIndiceLocaliza         : SmallInt;
                            PCampos                 : array of string;
                      const PIdentificadorRetorno,
                            PCaptionForm            : string;
                      const PFiltro                 : string = '';
                      const PIndiceColOrdena        : Integer = 0);
const
  NOME_FORM = 'FSisConsulta';
  CLAS_FORM = '_TFSisConsulta';
var
  ii           : integer;
  temp         : string;
  vField       : string;
  vDisp        : string;
  vSize        : string;
  vFmt         : string;
  vTab         : TIBOQuery;
  vGrid        : TcxGridDBTableView;
  vCombo1      : TComboBox;
  vCombo2      : TComboBox;
  vEdit        : TEdit;
  testaFooter  : Boolean;
begin
     FG.GravaCacheStr(FG.CnsCampoCh , PCampoChave);
     FG.GravaCacheStr(FG.CnsIndexRet, PIdentificadorRetorno);

     if FH.RetForm(NOME_FORM) <> nil then
     begin
       FH.RetForm(NOME_FORM).Show;
       exit;
     end;
     FG.GravaCacheStr(FG.CnsFormOrg, FR.Caption);

     if PCaptionForm = '' then
       temp := Format('Consulta (%s)', [FR.Caption])
     else
       temp := 'Consulta - ' + PCaptionForm;
     FG.ChamaForm(FR, CLAS_FORM, temp, 0);

     vTab    := (FH.RetFormComp(NOME_FORM, 'qrForm') as TIBOQuery);
     vGrid   := (FH.RetFormComp(NOME_FORM, 'grFormDBTableView1') as TcxGridDBTableView);
     vCombo1 := FH.RetFormComboBox(NOME_FORM, 'cbIndex');
     vCombo2 := FH.RetFormComboBox(NOME_FORM, 'cbIndexDes');
     vEdit   := FH.RetFormEdit(NOME_FORM, 'edLocalizar');

     vTab.Close;
  
     if  AnsiContainsText(PTabela, 'SELECT')
     and AnsiContainsText(PTabela, #32) then
        vTab.SQL.Text := PTabela
     else
        vTab.SQL.Text := 'SELECT * FROM ' + PTabela;

     if PFiltro <> '' then
       vTab.SQL.Add(PFiltro);
     vTab.Open;
     vTab.First;
     while vGrid.ColumnCount > 0 do
       vGrid.Columns[0].Destroy;
     Application.ProcessMessages;
     vCombo1.Clear;
     vCombo2.Clear;
     testaFooter := False;
     for ii := low(PCampos) to high(PCampos) do
     begin
          vFmt   := '';
          temp   := PCampos[ii];
          vField := FH.CopyPos(temp, ':');
          temp   := FH.CopyPosMax(temp, ':');
          vSize  := FH.CopyPos(temp, ':');
          temp   := FH.CopyPosMax(temp, ':');
          if Pos(':', temp) = 0 then
            vDisp := temp
          else
          begin
            vDisp := FH.CopyPos(temp, ':');
            temp  := FH.CopyPosMax(temp, ':');
            vFmt  := temp;
          end;

          if vFmt <> '' then
            if vTab.Fields.FindField(vField) <> nil then
              if vTab.FieldByName(vField) is TNumericField then
                (vTab.FieldByName(vField) as TNumericField).DisplayFormat := vFmt;

          with vGrid.CreateColumn do
          begin
            DataBinding.FieldName := vField;
            Caption := vDisp;
            Width   := StrToInt(vSize);

            if vTab.Fields.FindField(vField) <> nil then
               if vTab.FieldByName(vField) is TMemoField then
               begin
                    PropertiesClassName := 'TcxMemoProperties';
                    vGrid.OptionsView.CellAutoHeight := True;
               end;

            if not testaFooter then
              if Width >= 70 then
              begin
                Summary.FooterKind        := skCount;
                Summary.FooterFormat      := 'Total: ,0';
                Summary.GroupFooterKind   := skCount;
                Summary.GroupFooterFormat := 'Total: ,0';
                testaFooter := True;
              end;
          end;

          vCombo1.Items.Add(vField);
          vCombo2.Items.Add(vDisp);
     end;

     if not testaFooter then // se não conseguiu adicionar um sumário fecha a barra
     begin
       vGrid.OptionsView.Footer       := False;
       vGrid.OptionsView.GroupFooters := gfInvisible;
     end;

     with vGrid do
     begin
          for ii := 0 to ColumnCount-1 do
          begin
            Columns[ii].SortIndex := -1;
            Columns[ii].SortOrder := soNone;
          end;
          if ColumnCount > 0 then
          begin
            Columns[PIndiceColOrdena].SortIndex := 0;
            Columns[PIndiceColOrdena].SortOrder := soAscending;
          end;
          DataController.GotoFirst;
          Application.ProcessMessages;
          ApplyBestFit;
     end;

     FH.SetaComboItemIndex(vCombo1, PIndiceLocaliza);
     FH.SetaComboItemIndex(vCombo2, PIndiceLocaliza);
     FH.Focalize(vEdit);
end;

{------------------------------------------------------------------------------}
procedure ChamaCnsPro(FR: TForm; const AProduto: string; const pEditar: Boolean = False;
                      const CarregaUltimoLocalizado: Boolean = False; const BuscaAdicional: string = '');
const
     CLAS_FORM   = '_TFProConsulta';
     PREFIXO_CONS  = 'Pro_Consulta';
var
     temp: string;
begin
     temp := 'Produto - Consulta';

     FG.GravaCacheStr(PREFIXO_CONS + '_UltLocalizado', '');

     if pEditar then
     begin
          FG.GravaCacheStr('ChamaCnsPro_CodPro', AProduto);
          FG.ChamaForm(FR, '_TFProCadastro', temp, 0);
     end
     else
     begin
          FG.GravaCacheStr(PREFIXO_CONS + '_Codigo'        , AProduto);
          FG.GravaCacheStr(PREFIXO_CONS + '_BuscaAdicional', BuscaAdicional);

          if CarregaUltimoLocalizado then
             FG.GravaCacheStr(PREFIXO_CONS + '_UltLocalizado', 'SIM');

          FG.ChamaForm(FR, CLAS_FORM, temp, 0);
     end;
end;

{------------------------------------------------------------------------------}
procedure ChamaCnsCliOco(FR: TForm; const pCliente: string);
const
  NOME_FORM = 'FCliOcorrenciaConsulta';
  CLAS_FORM   = '_TFCliOcorrenciaConsulta';
var
  temp: string;
begin
  temp := 'C.R.M. - Consulta Ocorrência';
  FG.ChamaForm(FR, CLAS_FORM, temp, 0);
end;

{------------------------------------------------------------------------------}
procedure ChamaCnsForOco(FR: TForm; const AFor: string);
const
  NOME_FORM = 'FForOcorrenciaConsulta';
  CLAS_FORM   = '_TFForOcorrenciaConsulta';
var
  temp: string;
begin
  temp := 'Fornecedor - Consulta Ocorrência';
  FG.ChamaForm(FR, CLAS_FORM, temp, 0);
end;

{------------------------------------------------------------------------------}
procedure ChamaCnsContato(FR: TForm; const pCliente: string);
const
  NOME_FORM = 'FCliContatoConsulta';
  CLAS_FORM   = '_TFCliContatoConsulta';
var
  temp: string;
begin
  temp := 'Contato de Clientes - Consulta';
  FG.ChamaForm(FR, CLAS_FORM, temp, 0);
end;

{------------------------------------------------------------------------------}
procedure ChamaCnsValePresente(FR: TForm; const PClienteVale, PIdFlx: string);
const
  NOME_FORM = 'FVenNotaVale';
  CLAS_FORM   = '_TFVenNotaVale';
var
  temp: string;
begin
  FG.ChamaForm(FR, CLAS_FORM, '', 0);

  FG.RetFormJvDateEdit(NOME_FORM, 'edLocIni').Clear;
  FG.RetFormJvDateEdit(NOME_FORM, 'edLocFin').Clear;
  FH.RetFormComboBox(NOME_FORM, 'cbSit').ItemIndex := 0;
  FG.RetFormJvComboEdit(NOME_FORM, 'edLocCli').Text := PClienteVale;
  FH.ForceExit(FG.RetFormJvComboEdit(NOME_FORM, 'edLocCli'));

  if PIdFlx > '' then
     FH.RetFormEdit(NOME_FORM, 'edIdFlx').Text := PIdFlx;

  FH.ForceClick(FG.RetFormJvBitBtn(NOME_FORM, 'btLocalizar'));
end;

{------------------------------------------------------------------------------}
procedure ChamaCnsTroca(FR: TForm; const pCliente, PNome, pDataIni, pDataFin, pNumero: string; const ALocaliza: Boolean = False);
const
  NOME_FORM = 'FVenTrocaConsulta';
  CLAS_FORM   = '_TFVenTrocaConsulta';
var
  temp: string;
begin
  temp := 'Troca - Consulta';
  FG.ChamaForm(FR, CLAS_FORM, temp, 0);

  if pCliente > '' then
  begin
    FG.RetFormJvComboEdit(NOME_FORM, 'edCliente').Text := pCliente;

    if PNome = '' then
      FH.RetFormLabel(NOME_FORM, 'lbNome').Caption := FG.Lookup('CLIDADO', 'NOME', 'CODIGO', FGCli.RetCliCodigo(pCliente))
    else
      FH.RetFormLabel(NOME_FORM, 'lbNome').Caption := PNome;
  end;

  FG.RetFormJvDateEdit(NOME_FORM, 'edDataIni').Text := pDataIni;
  FG.RetFormJvDateEdit(NOME_FORM, 'edDataFin').Text := pDataFin;

  if FH.IntOk(pNumero) then
    TJvSpinEdit(FH.RetFormComp(NOME_FORM, 'edNumero')).Value := StrToInt(pNumero);

  if ALocaliza then
    FG.RetFormJvBitBtn(NOME_FORM, 'btLocalizar').Click;
end;

{------------------------------------------------------------------------------}
procedure ChamaCnsStq(FR: TForm; const AProduto: string);
const
  NOME_FORM = 'FStqConsulta';
  CLAS_FORM   = '_TFStqConsulta';
var
  temp: string;
begin
  temp := 'Estoque - Consulta';
  FG.ChamaForm(FR, CLAS_FORM, temp, 0);
  FH.RetFormMaskEdit(NOME_FORM, 'edProduto').Text := AProduto;
end;

{------------------------------------------------------------------------------}
procedure ChamaCnsFor(FR: TForm; const AFornecedor: string; const PIdentificadorRetorno: string = '');
const
  NOME_FORM = 'FForConsulta';
  CLAS_FORM   = '_TFForConsulta';
var
  temp: string;
begin
  temp := 'Fornecedor - Consulta';
  FG.GravaCacheStr(FG.CnsIndexRet, PIdentificadorRetorno);
  FG.ChamaForm(FR, CLAS_FORM, temp, 0);
  { tiago, 21/02/08, desabilitado }
  {FH.RetFormMaskEdit(NOME_FORM, 'edLocalizar').Text := AFornecedor;}
end;

{------------------------------------------------------------------------------}
procedure ChamaCnsCli(FR: TForm; const pCliente: string = ''; const PIdentificadorRetorno: string = ''; const pMultiplasInstancias: Boolean = False);
const
  NOME_FORM = 'FCliCadastro';
  CLAS_FORM   = '_TFCliCadastro';
  PREFIXO_CONS  = 'Cli_Cadastro_Consulta';
var
  temp: string;
begin
  temp := 'Cliente - Consulta';

  FG.GravaCacheStr(PREFIXO_CONS            , 'ATIVA');
  FG.GravaCacheStr(PREFIXO_CONS + '_Codigo', FGCli.RetCliCodigo(pCliente));
  FG.GravaCacheStr(FG.CnsIndexRet, PIdentificadorRetorno);

  FG.ChamaForm(FR, CLAS_FORM, temp, 0, False, pMultiplasInstancias);
end;

{------------------------------------------------------------------------------}
procedure ChamaCnsComCota(FR: TForm);
const
  NOME_FORM = 'FComCotaConsulta';
  CLAS_FORM   = '_TFComCotaConsulta';
var
  temp: string;
begin
  temp := 'Cotas de Compra - Consulta';
  FG.ChamaForm(FR, CLAS_FORM, temp, 0);
end;

{------------------------------------------------------------------------------}
procedure ChamaCnsVenCota(FR: TForm);
const
  NOME_FORM = 'FVenCotaConsulta';
  CLAS_FORM   = '_TFVenCotaConsulta';
var
  temp: string;
begin
  temp := 'Cotas de Venda - Consulta';
  FG.ChamaForm(FR, CLAS_FORM, temp, 0);
end;

{------------------------------------------------------------------------------}
procedure ChamaCnsSat(FR: TForm);
const
  NOME_FORM = 'FSatCns';
  CLAS_FORM   = '_TFSatCns';
var
  temp: string;
begin
  temp := 'Serviço de Assistência Técnica - Consulta';
  FG.ChamaForm(FR, CLAS_FORM, temp, 0);
end;

{------------------------------------------------------------------------------}
procedure ChamaCnsRec(FR: TForm; const AContrato: string; const pEditar: Boolean = False);
const
  NOME_FORM = 'FRecMan';
  CLAS_FORM   = '_T' + NOME_FORM;
  PREFIXO_CONS  = 'Rec_Man_Consulta';
var
  temp: string;
begin
  temp := 'A Receber - Consulta';

  FG.GravaCacheStr(PREFIXO_CONS              , 'ATIVA');
  FG.GravaCacheStr(PREFIXO_CONS + '_Contrato', AContrato);
  FG.GravaCacheBln(PREFIXO_CONS + '_Editar'  , pEditar);
  try
    FG.ChamaForm(FR, CLAS_FORM, temp, 0);
  finally
    FG.GravaCacheStr(PREFIXO_CONS, '');
  end;
end;

{------------------------------------------------------------------------------}
procedure ChamaCnsCom(FR: TForm; const pID: Int64; const pEditar: Boolean = False);
const
  NOME_FORM = 'FComNotaMan';
  CLAS_FORM   = '_T' + NOME_FORM;
  PREFIXO_CONS  = '[ComNotaMan-Consulta]';
var
  temp: string;
begin
  temp := 'Compra - Manutenção - Consulta';

  FG.GravaCacheStr(PREFIXO_CONS             , 'ATIVA');
  FG.GravaCacheInt(PREFIXO_CONS + ':ID'     , pID);
  FG.GravaCacheBln(PREFIXO_CONS + ':Editar' , pEditar);
  try
    FG.ChamaForm(FR, CLAS_FORM, temp, 0);
  finally
    FG.GravaCacheStr(PREFIXO_CONS, '');
  end;
end;

{------------------------------------------------------------------------------}
procedure ChamaCnsNotaDev(FR: TForm; const pID: Int64);
const
  NOME_FORM = 'FComNotaMan';
  CLAS_FORM   = '_T' + NOME_FORM;
  PREFIXO_CONS  = '[ComNotaMan-Consulta]';
var
  temp: string;
begin
  temp := 'Compra - Manutenção - Consulta';

  FG.GravaCacheStr(PREFIXO_CONS                    , 'ATIVA');
  FG.GravaCacheInt(PREFIXO_CONS + ':ID'            , pID);
  FG.GravaCacheStr(PREFIXO_CONS + ':ConsultaNtDev' , 'SIM');
  try
    FG.ChamaForm(FR, CLAS_FORM, temp, 0);
  finally
    FG.GravaCacheStr(PREFIXO_CONS, '');
  end;
end;

{------------------------------------------------------------------------------}
procedure ChamaCnsProStq(FR: TForm; const ACodPro: string);
const
  NOME_FORM = 'FStqPos';
  CLAS_FORM   = '_TFStqPos';
  PREFIXO_CONS  = 'Stq_Pos_Consulta';
var
  temp: string;
begin
  temp := 'Produto - Posição';

  FG.GravaCacheStr(PREFIXO_CONS + '_Codigo', ACodPro);
  try
    FG.ChamaForm(FR, CLAS_FORM, temp, 0);
  finally
    FG.GravaCacheStr(PREFIXO_CONS, '');
  end;
end;

{------------------------------------------------------------------------------}
procedure ChamaGerPreco(FR: TForm; const ACodPro: string);
const
  NOME_FORM = 'FGerPreco';
  CLAS_FORM   = '_TFGerPreco';
  PREFIXO_CONS  = 'GerPreco_Pro';
var
  temp: string;
begin
  temp := 'Gerencial - Análise de preço';

  FG.GravaCacheStr(PREFIXO_CONS + '_Codigo', ACodPro);
  try
    FG.ChamaForm(FR, CLAS_FORM, temp, 0);
  finally
    FG.GravaCacheStr(PREFIXO_CONS, '');
  end;
end;

{------------------------------------------------------------------------------}
procedure ChamaCnsVen(FR: TForm; const pID       : string;
                                 const pNumero   : string  = '';
                                 const pSerie    : string  = '';
                                 const pEditar   : Boolean = False;
                                 const pBloqueio : Boolean = False);
const
  NOME_FORM = 'FVenNotaMan';
  CLAS_FORM   = '_T' + NOME_FORM;
  PREFIXO_CONS  = '[VenNotaMan-Consulta]';
var
  temp: string;
begin
  temp := 'Venda - Manutenção - Consulta';

  FG.GravaCacheStr(PREFIXO_CONS               , 'ATIVA');
  FG.GravaCacheStr(PREFIXO_CONS + ':ID'       , pID);
  FG.GravaCacheStr(PREFIXO_CONS + ':DataIni'  , '');
  FG.GravaCacheStr(PREFIXO_CONS + ':DataFin'  , '');
  FG.GravaCacheStr(PREFIXO_CONS + ':Numero'   , pNumero);
  FG.GravaCacheStr(PREFIXO_CONS + ':Serie'    , pSerie);
  FG.GravaCacheStr(PREFIXO_CONS + ':Cliente'  , '');
  FG.GravaCacheBln(PREFIXO_CONS + ':Editar'   , pEditar);
  FG.GravaCacheBln(PREFIXO_CONS + ':BloqDatas', pBloqueio);
  try
    FG.ChamaForm(FR, CLAS_FORM, temp, 0);
  finally
    FG.GravaCacheStr(PREFIXO_CONS, '');
  end;
end;

{------------------------------------------------------------------------------}
procedure ChamaCnsVen(FR: TForm; const pDataIni  ,
                                       pDataFin  ,
                                       pNumero   ,
                                       pSerie    ,
                                       pCliente  : string;
                                 const pEditar   : Boolean = False;
                                 const pBloqueio : Boolean = False);
const
  NOME_FORM = 'FVenNotaMan';
  CLAS_FORM   = '_T' + NOME_FORM;
  PREFIXO_CONS  = '[VenNotaMan-Consulta]';
var
  temp: string;
begin
  temp := 'Venda - Manutenção - Consulta';

  FG.GravaCacheStr(PREFIXO_CONS               , 'ATIVA');
  FG.GravaCacheStr(PREFIXO_CONS + ':ID'       , '');
  FG.GravaCacheStr(PREFIXO_CONS + ':DataIni'  , pDataIni);
  FG.GravaCacheStr(PREFIXO_CONS + ':DataFin'  , pDataFin);
  FG.GravaCacheStr(PREFIXO_CONS + ':Numero'   , pNumero);
  FG.GravaCacheStr(PREFIXO_CONS + ':Serie'    , pSerie);
  FG.GravaCacheStr(PREFIXO_CONS + ':Cliente'  , pCliente);
  FG.GravaCacheBln(PREFIXO_CONS + ':Editar'   , pEditar);
  FG.GravaCacheBln(PREFIXO_CONS + ':BloqDatas', pBloqueio);
  try
    FG.ChamaForm(FR, CLAS_FORM, temp, 0);
  finally
    FG.GravaCacheStr(PREFIXO_CONS, '');
  end;
end;

{------------------------------------------------------------------------------}
procedure ChamaCnsVenPed(FR: TForm; const pNumero: string; const pEditar: Boolean = False);
const
  NOME_FORM = 'FVenPedMan';
  CLAS_FORM   = '_T' + NOME_FORM;
  PREFIXO_CONS  = 'VEN_PEDMAN_CONSULTA';
var
  temp: string;
begin
  temp := 'Pedidos de Venda - Consulta';
  if pEditar then
  begin
    FG.GravaCacheStr(PREFIXO_CONS             , 'ATIVA');
    FG.GravaCacheStr(PREFIXO_CONS + '_Numero' , pNumero);
    try
      FG.ChamaForm(FR, CLAS_FORM, temp, 0);
    finally
      FG.GravaCacheStr(PREFIXO_CONS, '');
    end;
  end
  else
  begin
       FG.GravaCacheStr('FVenPedConsulta_Numero', pNumero);
       FG.ChamaForm(FR, '_TFVenPedConsulta', temp, 0);
  end;
end;

{------------------------------------------------------------------------------}
procedure ChamaCnsPag(FR: TForm; const AFor, AFatura, ADupl: string; const pEditar: Boolean = False);
const
  NOME_FORM = 'FPagMan';
  CLAS_FORM   = '_T' + NOME_FORM;
  PREFIXO_CONS  = 'Pag_Man_Consulta';
var
  temp: string;
begin
  temp := 'A Pagar - Consulta';

  FG.GravaCacheStr(PREFIXO_CONS             , 'ATIVA');
  FG.GravaCacheStr(PREFIXO_CONS + '_For'    , AFor);
  FG.GravaCacheStr(PREFIXO_CONS + '_Fatura' , AFatura);
  FG.GravaCacheStr(PREFIXO_CONS + '_Dupl'   , ADupl);
  FG.GravaCacheBln(PREFIXO_CONS + '_Editar' , pEditar);
  try
    FG.ChamaForm(FR, CLAS_FORM, temp, 0);
  finally
    FG.GravaCacheStr(PREFIXO_CONS, '');
  end;
end;

{------------------------------------------------------------------------------}
procedure ChamaCnsCidIbge(FR: TForm);
const
  NOME_FORM = 'FCidadeIbgeCns';
  CLAS_FORM   = '_TFCidadeIbgeCns';
var
  temp: string;
begin
  temp := 'Cidade IBGE - Consulta';
  FG.ChamaForm(FR, CLAS_FORM, temp, 0);
end;

{------------------------------------------------------------------------------}
procedure ChamaCnsComPed(FR: TForm; const pNumero: string; const pEditar: Boolean = False);
const
  NOME_FORM = 'FComPedMan';
  CLAS_FORM   = '_T' + NOME_FORM;
  PREFIXO_CONS  = 'Com_Ped_Man_Consulta';
var
  temp: string;
begin
  temp := 'Suprimento - Pedido de Compra Manutenção - Consulta';

  FG.GravaCacheStr(PREFIXO_CONS            , 'ATIVA');
  FG.GravaCacheStr(PREFIXO_CONS + '_Numero', pNumero);
  FG.GravaCacheBln(PREFIXO_CONS + '_Editar', pEditar);
  try
    FG.ChamaForm(FR, CLAS_FORM, temp, 0);
  finally
    FG.GravaCacheStr(PREFIXO_CONS, '');
  end;
end;

{------------------------------------------------------------------------------}
procedure ChamaCnsFlx(FR: TForm; const pID: string; const pEditar: Boolean = False);
const
  NOME_FORM = 'FFlxMan';
  CLAS_FORM   = '_T' + NOME_FORM;
  PREFIXO_CONS  = 'Flx_Man_Consulta';
var
  temp: string;
begin
  temp := 'Financeiro - Fluxo Manutenção - Consulta';

  FG.GravaCacheStr(PREFIXO_CONS            , 'ATIVA');
  FG.GravaCacheStr(PREFIXO_CONS + '_ID'    , pID);
  FG.GravaCacheBln(PREFIXO_CONS + '_Editar', pEditar);
  try
    FG.ChamaForm(FR, CLAS_FORM, temp, 0);
  finally
    FG.GravaCacheStr(PREFIXO_CONS, '');
  end;
end;

{------------------------------------------------------------------------------}
function RetProClas(const ANumeroClas: TNumeroClas; const PEstabe: string = ''): string;
begin
  Result := FG.Lookup('PROCLASCFG', FH.Juntar('CLAS', ANumeroClas), 'ESTABE',
                      IfThen(PEstabe = '', FG.RetEstabeAtivo, PEstabe));
  if FH.StrInvalida(Result) then
    Result := '[Clas.' + inttostr(ANumeroClas) + ']';
end;

{------------------------------------------------------------------------------}
function VerProClas(const ANumeroClas: TNumeroClas; const PEstabe: string = ''): boolean;
begin
  Result := FH.StrOk(FG.Lookup('PROCLASCFG', FH.Juntar('CLAS', ANumeroClas), 'ESTABE',
                                   IfThen(PEstabe = '', FG.RetEstabeAtivo, PEstabe)));
end;

{------------------------------------------------------------------------------}
function RetProEnd(const ANumeroEnd: TNumeroEnd): string;
begin
     Result := FG.Lookup('proendereco', 'denominacao', 'numero', ANumeroEnd);
     if FH.StrInvalida(Result) then
        Result := 'End. '+IntToStr(ANumeroEnd);
end;

{------------------------------------------------------------------------------}
function RetMaskProEnd(const ANumeroEnd: TNumeroEnd): string;
const subX = #222;
      subA = #240;
      subN = #254;
var e : string;
    i : Integer;
begin
     Result := '';
     e := FG.Lookup('proendereco', 'estrutura', 'numero', ANumeroEnd);
     if FH.StrOK(e) then
     begin
          e := stringReplace(e, 'X', subX, [rfReplaceAll]);
          e := stringReplace(e, 'A', subA, [rfReplaceAll]);
          e := stringReplace(e, 'N', subN, [rfReplaceAll]);
          i := 1;
          while i <= Length(e) do
          begin
               if AnsiMatchStr(e[i], [subX,subA,subN,'.','-']) then
                  inc(i)
               else
               begin
                    System.Insert('\',e,i);
                    inc(i,2);
               end;

          end;
          e := stringReplace(e, subX, 'a', [rfReplaceAll]);
          e := stringReplace(e, subA, 'l', [rfReplaceAll]);
          e := stringReplace(e, subN, '9', [rfReplaceAll]);
          
          Result := e + ';1;_';
     end;
end;

{------------------------------------------------------------------------------}
function VerProEnd(const ANumeroEnd: TNumeroEnd): boolean;
begin
     Result := FH.StrOk(FG.Lookup('proendereco', 'denominacao', 'numero', ANumeroEnd));
end;

{------------------------------------------------------------------------------}
function TemProEnd: Boolean;
var q:TIBOQuery;
begin
     try
       FG.CriaIboQuery(q, 'select count(*) as tot from proendereco');
       q.Open;
       Result := q.FieldByName('tot').AsInteger > 0;
     finally
            FH.DestroiObj(q);
     end;
end;

{------------------------------------------------------------------------------}
function UsuAtivoExiste: boolean;
begin
     Result := FG.RegistroExiste('USUCADASTRO', 'USUARIO', FG.RetUsuAtivo);
end;

{------------------------------------------------------------------------------}
procedure TabStopControl(pObj: TWinControl; pHabilita: boolean);
var
  ii: Smallint;
begin
  with pObj do
  begin
    for ii := 0 to ControlCount - 1 do
      if ((Controls[ii]).ClassType <> TLabel) and
         ((Controls[ii]).ClassType <> TGroupBox) then
      try
        TWinControl(Controls[ii]).TabStop := pHabilita;
      except
      end;
  end;
end;

{------------------------------------------------------------------------------}
procedure LocalizaCombo(pControl: TWinControl; pEdit: TEdit; pParcial, pSemEspaco: boolean;
  pStart: string = '');
var
  I, vTam: Integer;
  S, vAux: string;
  vCombo: TComboBox;
begin
  with pControl do
  begin
    vAux := 'Cb' + Copy(pEdit.Name, 3, 40);
    vCombo := (FindComponent(vAux) as TComboBox);
    try
      vCombo.ItemIndex := -1;
      I := 0;

      S := AnsiUpperCase(pEdit.Text);
      if pSemEspaco then
        S := FH.LimpaEspc(S);

      vTam := Length(S);

      if vTam = 0 then
      begin
        vCombo.ItemIndex := -1;
        exit;
      end;

      while I < vCombo.Items.Count do
      begin
        vAux := AnsiUpperCase(vCombo.Items[I]);

        if (pParcial) and (Pos('=', S) = 0) then
        begin
          if pSemEspaco then
            vAux := FH.LimpaEspc(vAux);
          if pStart = '' then
            vAux := Copy(vAux, 1, vTam)
          else
            vAux := Copy(vAux, Pos(pStart, vAux) + 1, vTam);
        end
        else
        begin
          if Pos('=', S) > 0 then
            Delete(S, Pos('=', S), 1);
          vAux := Copy(vAux, Pos(pStart, vAux) + 1, MaxInt);
        end;

        vAux := AnsiUpperCase(vAux);

        if pSemEspaco then
          vAux := FH.LimpaEspc(vAux);

        if SameText(S, vAux) then
        begin
          vCombo.ItemIndex := I;
          I := vCombo.Items.Count;
        end;
        Inc(I);
      end;
    except
    end;
  end;
end;

{------------------------------------------------------------------------------}
procedure SetaCtrSel(DSOrg, DSDst: TDataSet;
  AGridOrg, AGridDst: TDBGrid; const AbreOrg: boolean = true);
var
  CD: TClientDataSet;
begin
     try
        Application.ProcessMessages;
        FH.SetaCursorAmp;

        AGridOrg.DataSource.DataSet := DSOrg;
        AGridDst.DataSource.DataSet := DSDst;

        if AbreOrg then
        begin
          if (DSOrg is TIBOTable) and (not DSOrg.Active) then (DSOrg as TIBOTable).Open;
          if (DSOrg is TIBOQuery) and (not DSOrg.Active) then (DSOrg as TIBOQuery).Open;
        end;
        if (DSDst is TClientDataSet) and (not DSDst.Active) then
        begin
          CD := DSDst as TClientDataSet;
          CD.CreateDataSet;
          if AGridDst.Columns.Count > 0 then CD.IndexFieldNames := AGridDst.Columns[0].FieldName;
          CD.LogChanges := false;
        end;

        if AGridOrg.Columns.Count > 0 then
          if DSOrg.FindField(AGridOrg.Columns[0].FieldName) = nil then
          begin
            if SameText(AGridOrg.Columns[0].FieldName, 'CODIGO') then
              AGridOrg.Columns[0].FieldName := 'CODCONTA'
            else
            if SameText(AGridOrg.Columns[0].FieldName, 'CODCONTA') then
              AGridOrg.Columns[0].FieldName := 'CODIGO';
          end;

        if AGridOrg.Columns.Count > 1 then
          if DSOrg.FindField(AGridOrg.Columns[1].FieldName) = nil then
          begin
            if SameText(AGridOrg.Columns[1].FieldName, 'DESCRICAO') then
              AGridOrg.Columns[1].FieldName := 'NOME'
            else
            if SameText(AGridOrg.Columns[1].FieldName, 'NOME') then
              AGridOrg.Columns[1].FieldName := 'DESCRICAO';
          end;

        if AGridDst.Columns.Count > 1 then
          if DSDst.FindField(AGridDst.Columns[1].FieldName) = nil then
          begin
            if SameText(AGridDst.Columns[1].FieldName, 'DESCRICAO') then
              AGridDst.Columns[1].FieldName := 'NOME'
            else
            if SameText(AGridDst.Columns[1].FieldName, 'NOME') then
              AGridDst.Columns[1].FieldName := 'DESCRICAO';
          end;
     finally
            FH.SetaCursorDef;
     end;
end;

{------------------------------------------------------------------------------}
procedure SetaCtrSel(DSOrg, DSDst: TDataSet;
  AGridOrg, AGridDst: TcxGridDBTableView; const AbreOrg: boolean = true);
var
   CD: TClientDataSet;
begin
     try
        Application.ProcessMessages;
        FH.SetaCursorAmp;

        AGridOrg.DataController.DataSource.DataSet := nil;
        AGridDst.DataController.DataSource.DataSet := nil;

        AGridOrg.DataController.DataModeController.GridMode := False;

        AGridDst.OptionsBehavior.IncSearch := True;
        AGridOrg.OptionsBehavior.IncSearch := True;

        AGridOrg.OptionsCustomize.ColumnSorting := True;

        if (DSOrg is TIBOTable) then
           if AnsiMatchText((DSOrg as TIBOTable).TableName, ['PROCADASTRO', 'FORCADASTRO', 'TABBAIRRO']) then
           begin
                AGridOrg.DataController.DataModeController.GridMode := True;
                AGridDst.DataController.DataModeController.GridMode := True;

                AGridDst.OptionsBehavior.IncSearch := False;
                AGridOrg.OptionsBehavior.IncSearch := False;

                AGridDst.OptionsCustomize.ColumnSorting := False;
                AGridOrg.OptionsCustomize.ColumnSorting := False;
           end;

        AGridOrg.DataController.DataSource.DataSet := DSOrg;
        AGridDst.DataController.DataSource.DataSet := DSDst;

        if AbreOrg then
        begin
             if (DSOrg is TIBOTable) and (not DSOrg.Active) then
             begin
                  (DSOrg as TIBOTable).Open;
                  (DSOrg as TIBOTable).First;
                  AGridOrg.DataController.GotoFirst;
             end;
             if (DSOrg is TIBOQuery) and (not DSOrg.Active) then
             begin
                  (DSOrg as TIBOQuery).Open;
                  AGridOrg.DataController.GotoFirst;
             end;
        end;
        if (DSDst is TClientDataSet) and (not DSDst.Active) then
        begin
             CD := DSDst as TClientDataSet;
             CD.CreateDataSet;
             if AGridDst.ColumnCount > 0 then
                CD.IndexFieldNames := AGridDst.Columns[0].DataBinding.FieldName;
             AGridDst.DataController.GotoFirst;
        end;

        if AGridOrg.ColumnCount > 0 then
           if DSOrg.FindField(AGridOrg.Columns[0].DataBinding.FieldName) = nil then
           begin
                if SameText(AGridOrg.Columns[0].DataBinding.FieldName, 'CODIGO') then
                   AGridOrg.Columns[0].DataBinding.FieldName := 'CODCONTA'
                else
                if SameText(AGridOrg.Columns[0].DataBinding.FieldName, 'CODCONTA') then
                   AGridOrg.Columns[0].DataBinding.FieldName := 'CODIGO';
           end;

        if AGridOrg.ColumnCount > 1 then
           if DSOrg.FindField(AGridOrg.Columns[1].DataBinding.FieldName) = nil then
           begin
                if SameText(AGridOrg.Columns[1].DataBinding.FieldName, 'DESCRICAO') then
                   AGridOrg.Columns[1].DataBinding.FieldName := 'NOME'
                else
                if SameText(AGridOrg.Columns[1].DataBinding.FieldName, 'NOME') then
                   AGridOrg.Columns[1].DataBinding.FieldName := 'DESCRICAO';
           end;

        if AGridDst.ColumnCount > 1 then
           if DSDst.FindField(AGridDst.Columns[1].DataBinding.FieldName) = nil then
           begin
                if SameText(AGridDst.Columns[1].DataBinding.FieldName, 'DESCRICAO') then
                   AGridDst.Columns[1].DataBinding.FieldName := 'NOME'
                else
                if SameText(AGridDst.Columns[1].DataBinding.FieldName, 'NOME') then
                   AGridDst.Columns[1].DataBinding.FieldName := 'DESCRICAO';
           end;
     finally
            FH.SetaCursorDef;
     end;
end;

{------------------------------------------------------------------------------}
procedure LocSel(const ALocalizar: string; GR: TDBGrid);
var
  DS: TDataSet;
begin
  if FH.StrInvalida(ALocalizar) then
    exit;
  DS := GR.DataSource.DataSet;
  if DS = nil then
    exit;
  if copy(ALocalizar, 1, 1) = '=' then
  begin
    if copy(ALocalizar, 2, MaxInt) = '' then
      exit;

    if GR.Columns.Count > 1 then
      DS.Locate(GR.Columns[1].FieldName,
                copy(ALocalizar, 2, MaxInt),
                [loCaseInsensitive, loPartialKey]);
  end
  else
  begin
    if GR.Columns.Count > 0 then
    begin
      if (DS.FieldByName(GR.Columns[0].FieldName).DataType = ftInteger)
      or (DS.FieldByName(GR.Columns[0].FieldName).DataType = ftLargeInt)
      or (DS.FieldByName(GR.Columns[0].FieldName).DataType = ftSmallInt)
      or (DS.FieldByName(GR.Columns[0].FieldName).DataType = ftString) then

      if FH.IntInvalido(ALocalizar) then
        exit;

      if (DS.FieldByName(GR.Columns[0].FieldName).DataType = ftFloat) then
        if not FH.FloatOk(ALocalizar) then
          exit;

      DS.Locate(GR.Columns[0].FieldName,
                ALocalizar, [loCaseInsensitive, loPartialKey]);
    end;
  end;
end;

{------------------------------------------------------------------------------}
procedure LocSel(const ALocalizar: string; GR: TcxGridDBTableView; const SubTipo: string='');
var
   DS: TDataSet;
   ii: Integer;
begin
     if FH.StrInvalida(ALocalizar) then
        exit;
     DS := GR.DataController.DataSet;
     if DS = nil then
        exit;

     if SubTipo = 'CLASFISCAL' then
     begin
          DS.Locate(GR.Columns[1].DataBinding.FieldName,
                    ALocalizar, [loCaseInsensitive, loPartialKey]);
          for ii := 0 to GR.ColumnCount - 1 do
          begin
               GR.Columns[ii].SortIndex := -1;
               GR.Columns[ii].SortOrder := soNone;
          end;
          GR.Columns[1].SortOrder := soAscending;
     end
     else
     if FH.IntInvalido(ALocalizar) then
     begin
          if  (GR.ColumnCount > 1)
          and (copy(ALocalizar, 1, 1) <> '=') then
              if (DS.FieldByName(GR.Columns[1].DataBinding.FieldName).DataType = ftString) then
              begin
                   DS.Locate(GR.Columns[1].DataBinding.FieldName,
                             ALocalizar, [loCaseInsensitive, loPartialKey]);
                   for ii := 0 to GR.ColumnCount - 1 do
                   begin
                        GR.Columns[ii].SortIndex := -1;
                        GR.Columns[ii].SortOrder := soNone;
                   end;
                   GR.Columns[1].SortOrder := soAscending;
              end;

          if GR.ColumnCount > 0 then
          begin
               if (DS.FieldByName(GR.Columns[0].DataBinding.FieldName).DataType = ftString) then
               begin
                    DS.Locate(GR.Columns[0].DataBinding.FieldName,
                                IfThen(leftstr(ALocalizar, 1) = '=', copy(ALocalizar, 2, MaxInt) ,ALocalizar),
                                   [loCaseInsensitive, loPartialKey]);
                    for ii := 0 to GR.ColumnCount - 1 do
                    begin
                         GR.Columns[ii].SortIndex := -1;
                         GR.Columns[ii].SortOrder := soNone;
                    end;
                    GR.Columns[0].SortOrder := soAscending;
                    Exit;
               end;
          end;
     end
     else
     begin
          if GR.ColumnCount > 0 then
          begin
               if (DS.FieldByName(GR.Columns[0].DataBinding.FieldName).DataType = ftInteger )
               or (DS.FieldByName(GR.Columns[0].DataBinding.FieldName).DataType = ftLargeInt)
               or (DS.FieldByName(GR.Columns[0].DataBinding.FieldName).DataType = ftSmallInt)
               or (DS.FieldByName(GR.Columns[0].DataBinding.FieldName).DataType = ftString  ) then
               begin
                    DS.Locate(GR.Columns[0].DataBinding.FieldName,
                              ALocalizar, [loCaseInsensitive, loPartialKey]);
                    for ii := 0 to GR.ColumnCount - 1 do
                    begin
                         GR.Columns[ii].SortIndex := -1;
                         GR.Columns[ii].SortOrder := soNone;
                    end;
                    GR.Columns[0].SortOrder := soAscending;
               end;
          end;
     end;
end;

{------------------------------------------------------------------------------}
procedure ExeSel(const Fun: Char; AGridOrg, AGridDst: TDBGrid;
  const DstChave: string = 'CODIGO');
var
  C2Org, C2Dst: string;
  DSD, DSO: TDataSet;
  CD: TClientDataSet;

  procedure CopiaUm;
  begin
    if CD.Locate('CODIGO', DSO.FieldByName(DstChave).AsString, []) then
      exit;
    CD.Insert;
    CD.FieldByName('CODIGO').AsString := DSO.FieldByName(DstChave).AsString;
    C2Org := 'DESCRICAO';
    C2Dst := 'DESCRICAO';
    if CD.FindField(C2Dst) = nil then
    begin
      if DSO.FindField(C2Org) = nil then
      begin
        C2Org := 'NOME';
        C2Dst := 'NOME';
      end
      else
        C2Dst := 'NOME';
    end
    else
    begin
      if DSO.FindField(C2Org) = nil then
        C2Org := 'NOME';
    end;
    CD.FieldByName(C2Dst).AsString := DSO.FieldByName(C2Org).AsString;
    CD.Post;
  end;

begin
  DSD := AGridDst.DataSource.DataSet;
  if DSD is TClientDataSet then
    CD := (DSD as TClientDataSet)
  else
    CD := nil;
  DSO := AGridOrg.DataSource.DataSet;

  try
    FH.SetaCursorAmp;

    case Fun of
      'L':
        begin
          if CD <> nil then
            CD.EmptyDataSet;
        end;
      'E':
        begin
          if not DSD.IsEmpty then
          begin
            if DSD is TClientDataSet then
              (DSD as TClientDataSet).Delete
            else
              DSD.Delete;
          end;
        end;
      'S':
        begin
          if CD <> nil then
            CopiaUm;
        end;
      'T':
        begin
          if CD <> nil then
          begin
            CD.EmptyDataSet;
            try
              DSO.DisableControls;
              DSD.DisableControls;
              DSO.First;
              while not DSO.Eof do
              begin
                CopiaUm;
                DSO.Next;
              end;
            finally
              DSO.First;
              DSD.First;
              DSO.EnableControls;
              DSD.EnableControls;
            end;
          end;
        end;
    end;
  finally
    FH.SetaCursorDef;
  end;
end;

{------------------------------------------------------------------------------}
procedure ExeSel(const Fun: Char; AGridOrg, AGridDst: TcxGridDBTableView;
  const DstChave: string = 'CODIGO');
var
  C2Org,
  C2Dst  : string;
  DSD,
  DSO    : TDataSet;
  CD     : TClientDataSet;

  procedure CopiaUm(const ALoc: Boolean);
  begin
    if ALoc then
      if CD.Locate('CODIGO', DSO.FieldByName(DstChave).AsString, []) then
        exit;

    CD.Append;
    CD.FieldByName('CODIGO').AsString := DSO.FieldByName(DstChave).AsString;
    C2Org := 'DESCRICAO';
    C2Dst := 'DESCRICAO';
    if CD.FindField(C2Dst) = nil then
    begin
      if DSO.FindField(C2Org) = nil then
      begin
        C2Org := 'NOME';
        C2Dst := 'NOME';
      end
      else
        C2Dst := 'NOME';
    end
    else
    begin
      if DSO.FindField(C2Org) = nil then
        C2Org := 'NOME';
    end;

    if DSD.FindField('CLASSIFICACAO') <> nil  then
      CD.FieldByName('CLASSIFICACAO').AsString := DSO.FieldByName('CLASSIFICACAO').AsString
    else
      CD.FieldByName(C2Dst).AsString := DSO.FieldByName(C2Org).AsString;
      
    CD.Post;
  end;

begin
  DSD := AGridDst.DataController.DataSet;
  if DSD is TClientDataSet then
    CD := (DSD as TClientDataSet)
  else
    CD := nil;
  DSO := AGridOrg.DataController.DataSet;

  try
    FH.SetaCursorAmp;

    case Fun of
      'L':
        begin
          if CD <> nil then
            CD.EmptyDataSet;
        end;
      'E':
        begin
            if not CD.IsEmpty then
            begin
                CD.Delete;
            end;
        end;
      'S':
        begin
          if CD <> nil then
            CopiaUm(True);
        end;
      'T':
        begin
          if CD <> nil then
          begin
            AGridDst.DataController.DataSource.DataSet := nil;
            CD.EmptyDataSet;
            try
              DSO.DisableControls;
              DSD.DisableControls;
              DSO.First;
              while not DSO.Eof do
              begin
                CopiaUm(False);
                DSO.Next;
              end;
            finally
              DSD.First;
              DSO.First;
              DSO.EnableControls;
              DSD.EnableControls;
              AGridDst.DataController.DataSource.DataSet := DSD;
              AGridOrg.DataController.GotoFirst;
            end;
          end;
        end;
    end;
  finally
    FH.SetaCursorDef;
  end;
end;

{------------------------------------------------------------------------------}
procedure ExeSel_ProEndereco(const Fun: Char; AGridOrg, AGridDst: TcxGridDBTableView;
  const DstChave: string = 'ENDERECO');
var
  C2Org,
  C2Dst  : string;
  DSD,
  DSO    : TDataSet;
  CD     : TClientDataSet;

  procedure CopiaUm(const ALoc: Boolean);
  begin
    if ALoc then
      if CD.Locate('ENDERECO', DSO.FieldByName(DstChave).AsString, []) then
        exit;
    CD.Append;
    CD.FieldByName('ENDERECO').AsString := DSO.FieldByName(DstChave).AsString;
    CD.Post;
  end;

begin
  DSD := AGridDst.DataController.DataSet;
  if DSD is TClientDataSet then
    CD := (DSD as TClientDataSet)
  else
    CD := nil;
  DSO := AGridOrg.DataController.DataSet;

  try
    FH.SetaCursorAmp;

    case Fun of
      'L':
        begin
          if CD <> nil then
            CD.EmptyDataSet;
        end;
      'E':
        begin
            if not CD.IsEmpty then
            begin
                CD.Delete;
            end;
        end;
      'S':
        begin
          if CD <> nil then
            CopiaUm(True);
        end;
      'T':
        begin
          if CD <> nil then
          begin
            AGridDst.DataController.DataSource.DataSet := nil;
            CD.EmptyDataSet;
            try
              DSO.DisableControls;
              DSD.DisableControls;
              DSO.First;
              while not DSO.Eof do
              begin
                CopiaUm(False);
                DSO.Next;
              end;
            finally
              DSD.First;
              DSO.First;
              DSO.EnableControls;
              DSD.EnableControls;
              AGridDst.DataController.DataSource.DataSet := DSD;
              AGridOrg.DataController.GotoFirst;
            end;
          end;
        end;
    end;
  finally
    FH.SetaCursorDef;
  end;
end;

{------------------------------------------------------------------------------}
procedure SetaClasSel(TC: TTabControl);
var
  ii: integer;
begin
  ii := 0;
  while ii <= pred(TC.Tabs.Count) do
  begin
         if SameText('CLAS1', TC.Tabs[ii]) then TC.Tabs[ii] := FG.RetProClas(1)
    else if SameText('CLAS2', TC.Tabs[ii]) then TC.Tabs[ii] := FG.RetProClas(2)
    else if SameText('CLAS3', TC.Tabs[ii]) then TC.Tabs[ii] := FG.RetProClas(3);

    if FH.ComparaTextoFixo(TC.Tabs[ii], '[CLAS.') then
      TC.Tabs.Delete(ii)
    else
      inc(ii);
  end;
end;

{------------------------------------------------------------------------------}
procedure SetaClasSel(TC: TcxTabControl);
var
  ii: integer;
begin
  ii := 0;
  while ii <= pred(TC.Tabs.Count) do
  begin
         if SameText('CLAS1', TC.Tabs[ii].Caption) then TC.Tabs[ii].Caption := FG.RetProClas(1)
    else if SameText('CLAS2', TC.Tabs[ii].Caption) then TC.Tabs[ii].Caption := FG.RetProClas(2)
    else if SameText('CLAS3', TC.Tabs[ii].Caption) then TC.Tabs[ii].Caption := FG.RetProClas(3);

    if FH.ComparaTextoFixo(TC.Tabs[ii].Caption, '[CLAS.') then
      TC.Tabs.Delete(ii)
    else
      inc(ii);
  end;
end;

{------------------------------------------------------------------------------}
procedure SetaEndSel(TC: TcxTabControl);
var
  ii: integer;
begin
  ii := 0;
  while ii <= pred(TC.Tabs.Count) do
  begin
         if SameText('END1', TC.Tabs[ii].Caption) then TC.Tabs[ii].Caption := FG.RetProClas(1)
    else if SameText('END2', TC.Tabs[ii].Caption) then TC.Tabs[ii].Caption := FG.RetProClas(2);

    if FH.ComparaTextoFixo(TC.Tabs[ii].Caption, '[END.') then
      TC.Tabs.Delete(ii)
    else
      inc(ii);
  end;
end;

{------------------------------------------------------------------------------}
function RetSQLCliente(const AVerFiltro: Boolean; const AExtraSelect, AOrdenacao: string): string;
var
  vFrom  ,
  vWhere ,
  auxStr ,
  ss     : string;
  bTemp  : Boolean;
  lsAux  : TStrings;
  ii     ,
  jj     : Integer;
begin
  if AVerFiltro then
  begin
    Result := DMC.ClienteSQL_Select + AExtraSelect;

    vFrom := ADMC.ClienteSQL_From;

    if DMS.cdsSelMetas.Active then
       if not DMS.cdsSelMetas.IsEmpty then
           vFrom  := ADMC.ClienteSQL_From +
                     #13'     LEFT JOIN CLIMETA        META ON CLIC.CODIGO = META.CODCLI ';

    vWhere := ADMC.ClienteSQL_Where;

    if DMS.cdsCliFiltro.Active then
    begin
      if not DMS.cdsCliFiltro.IsEmpty then
      begin
        if FH.DataOk(DMS.cdsCliFiltroDAVISO.AsString) then
        begin
          vFrom := vFrom + #13 +
                   ', CLIAVISOHIS AVIS';

          vWhere := vWhere + #13 +
                    '  AND DADO.CODIGO = AVIS.CLIENTE '#13 +
                    '  AND AVIS.DATA   = ' + FG.DataSql(DMS.cdsCliFiltroDAVISO.AsString);
        end;

        if FH.DataOk(DMS.cdsCliFiltroDTCOB.AsString)
        or (    (not AnsiContainsText(DMS.cdsCliFiltroSITCOB.AsString, 'T'))
            and (DMS.cdsCliFiltroSITCOB.AsString <> '')) then
        begin
          vFrom := vFrom + #13 +
                   ', CLICOBHIS COBR';

          vWhere := vWhere + #13 +
                    '  AND DADO.CODIGO = COBR.CLIENTE ';

          if FH.DataOk(DMS.cdsCliFiltroDTCOB.AsString) then
            vWhere := vWhere + #13 +
                      '  AND COBR.DATA   = ' + FG.DataSql(DMS.cdsCliFiltroDTCOB.AsString);

          if  (not AnsiContainsText(DMS.cdsCliFiltroSITCOB.AsString, 'T'))
          and (DMS.cdsCliFiltroSITCOB.AsString <> '') then
            vWhere := vWhere + #13 +
                      '  AND STRPOS(COBR.SITUACAO, ' + QuotedStr(DMS.cdsCliFiltroSITCOB.AsString) + ') > 0';
        end;

        if  (    FH.IntOk(DMS.cdsCliFiltroQTDCOMINI.AsString)
             and FH.IntOk(DMS.cdsCliFiltroQTDCOMFIN.AsString))
        or  (    FH.FloatOk(DMS.cdsCliFiltroVLRCOMINI.AsString)
             and FH.FloatOk(DMS.cdsCliFiltroVLRCOMFIN.AsString))
        or  (    FH.DataOk(DMS.cdsCliFiltroDTULTCOMINI.AsString)
             and FH.DataOk(DMS.cdsCliFiltroDTULTCOMFIN.AsString)) then
        begin
          vFrom := vFrom + #13 +
                   ', CLIESTATISTICA ESTA';

          vWhere := vWhere + #13 +
                    '  AND DADO.CODIGO = ESTA.CODIGO ';

          if  FH.IntOk(DMS.cdsCliFiltroQTDCOMINI.AsString)
          and FH.IntOk(DMS.cdsCliFiltroQTDCOMFIN.AsString) then
            vWhere := vWhere + #13 +
                      '  AND ESTA.QTTOTCOM BETWEEN ' + DMS.cdsCliFiltroQTDCOMINI.AsString +
                                             ' AND ' + DMS.cdsCliFiltroQTDCOMFIN.AsString;

          if  FH.FloatOk(DMS.cdsCliFiltroVLRCOMINI.AsString)
          and FH.FloatOk(DMS.cdsCliFiltroVLRCOMFIN.AsString) then
            vWhere := vWhere + #13 +
                      '  AND ESTA.VLTOTCOM BETWEEN ' + FH.FmtValSQL(DMS.cdsCliFiltroVLRCOMINI.AsFloat) +
                                             ' AND ' + FH.FmtValSQL(DMS.cdsCliFiltroVLRCOMFIN.AsFloat);

          if  FH.DataOk(DMS.cdsCliFiltroDTULTCOMINI.AsString)
          and FH.DataOk(DMS.cdsCliFiltroDTULTCOMFIN.AsString)
          and (DMS.cdsCliFiltroSELDTULTCOM.AsString <> 'T') then
          begin
            auxStr := '';
            if DMS.cdsCliFiltroSELDTULTCOM.AsString = 'I' then // inativos
              auxStr := 'NOT';

            vWhere := vWhere + #13 +
                      '  AND ESTA.DTULTCOM ' + auxStr +
                      ' BETWEEN ' + FG.DataSql(DMS.cdsCliFiltroDTULTCOMINI.AsString) +
                          ' AND ' + FG.DataSql(DMS.cdsCliFiltroDTULTCOMFIN.AsString);
          end;
        end;

        if  FH.DataOk(DMS.cdsCliFiltroDATACADAINI.AsString)
        and FH.DataOk(DMS.cdsCliFiltroDATACADAFIN.AsString) then
        begin
          vWhere := vWhere + #13 +
                    '  AND DADO.DATACADASTRO BETWEEN ' + FG.DataSql(DMS.cdsCliFiltroDATACADAINI.AsString) +
                                               ' AND ' + FG.DataSql(DMS.cdsCliFiltroDATACADAFIN.AsString);
        end;

        if FH.IntOk(DMS.cdsCliFiltroANIVERSARIO.AsString) then
        begin
          vWhere := vWhere + #13 +
                    '  AND EXTRACT (MONTH FROM CLIC.NASCIMENTO) = ' + DMS.cdsCliFiltroANIVERSARIO.AsString;
        end;

        if FH.SeStrVazia(DMS.cdsCliFiltroEMAIL.AsString, 'T') <> 'T' then
        begin
          if DMS.cdsCliFiltroEMAIL.AsString = 'I' then
          begin
            vWhere := vWhere + #13 +
                      '  AND DADO.EMAILINVALIDO = ''S'' ';
          end
          else
          if DMS.cdsCliFiltroEMAIL.AsString = 'V' then
          begin
            vWhere := vWhere + #13 +
                      '  AND STRLEN(DADO.EMAIL) > 0 ';
          end
          else
          if DMS.cdsCliFiltroEMAIL.AsString = 'S' then
          begin
            vWhere := vWhere + #13 +
                      '  AND STRLEN(DADO.EMAIL) < 1 ';
          end;
        end;

        if  FH.IntOk(DMS.cdsCliFiltroCODINI.AsString)
        and FH.IntOk(DMS.cdsCliFiltroCODFIN.AsString) then
        begin
          vWhere := vWhere + #13 +
                    '  AND CLIC.CODIGO BETWEEN ' + DMS.cdsCliFiltroCODINI.AsString +
                                         ' AND ' + DMS.cdsCliFiltroCODFIN.AsString;
        end;

        if DMS.cdsSelCliente.Active then
          if not DMS.cdsSelCliente.IsEmpty then
            vWhere := vWhere + #13 +
                      '  AND ' + FH.GeraSQLOR(DMS.cdsSelCliente, 'CODIGO,ORDEM', 'CLIC');

        if  FH.DataOk(DMS.cdsCliFiltroDATANASCINI.AsString)
        and FH.DataOk(DMS.cdsCliFiltroDATANASCFIN.AsString) then
        begin
          vWhere := vWhere + #13 +
                    '  AND CLIC.NASCIMENTO BETWEEN ' + FG.DataSql(DMS.cdsCliFiltroDATANASCINI.AsString) +
                                             ' AND ' + FG.DataSql(DMS.cdsCliFiltroDATANASCFIN.AsString);
        end;

        if  FH.DataOk(DMS.cdsCliFiltroDATAMANINI.AsString)
        and FH.DataOk(DMS.cdsCliFiltroDATAMANFIN.AsString) then
        begin
          vWhere := vWhere + #13 +
                    '  AND CLIC.DATA BETWEEN ' + FG.DataSql(DMS.cdsCliFiltroDATAMANINI.AsString) +
                                       ' AND ' + FG.DataSql(DMS.cdsCliFiltroDATAMANFIN.AsString);
        end;

        if  FH.DataOk(DMS.cdsCliFiltroDATADESCINI.AsString)
        and FH.DataOk(DMS.cdsCliFiltroDATADESCFIN.AsString) then
        begin
          vWhere := vWhere + #13 +
                    '  AND DADO.DTLIMITEDESC BETWEEN ' + FG.DataSql(DMS.cdsCliFiltroDATADESCINI.AsString) +
                                               ' AND ' + FG.DataSql(DMS.cdsCliFiltroDATADESCFIN.AsString);
        end;

        if FH.SeStrVazia(DMS.cdsCliFiltroPESSOA.AsString, 'T') <> 'T' then
        begin
          vWhere := vWhere + #13 +
                    '  AND CLIC.PESSOA = ' + QuotedStr(DMS.cdsCliFiltroPESSOA.AsString);
        end;

        if FH.SeStrVazia(DMS.cdsCliFiltroENDERECO.AsString, 'T') <> 'T' then
        begin
          vWhere := vWhere + #13 +
                    '  AND DADO.SITUACAO = ' + QuotedStr(DMS.cdsCliFiltroENDERECO.AsString);
        end;

        if  FH.floatOk(DMS.cdsCliFiltroSALARIOINI.AsString)
        and FH.floatOk(DMS.cdsCliFiltroSALARIOFIN.AsString) then
        begin
          vWhere := vWhere + #13 +
                    '  AND CLFI.EMPSALARIO BETWEEN ' + FH.FmtValSQL(DMS.cdsCliFiltroSALARIOINI.AsFloat) +
                                             ' AND ' + FH.FmtValSQL(DMS.cdsCliFiltroSALARIOFIN.AsFloat);
        end;
        if FH.SeStrVazia(DMS.cdsCliFiltroSALCOMPROVADO.AsString, 'T') <> 'T' then
        begin
          vWhere := vWhere + #13 +
                    '  AND CLFI.SALCOMPROVADO = ' + QuotedStr(DMS.cdsCliFiltroSALCOMPROVADO.AsString);
        end;

        if DMS.cdsSelOcorrencia.Active then
        begin
            if not DMS.cdsSelOcorrencia.IsEmpty then
            begin
                ss    := '';
                bTemp := False;
                DMS.cdsSelOcorrencia.First;
                while not DMS.cdsSelOcorrencia.Eof do
                begin
                    bTemp := DMS.cdsSelOcorrenciaSEL.AsString = 'S';
                    if bTemp then Break;
                    DMS.cdsSelOcorrencia.Next;
                end;

                if bTemp then
                begin
                    vFrom  := vFrom +
                              #13', CLICONTATOHIS CHIS ';

                    vWhere := vWhere + #13 +
                              '  AND CHIS.CLIENTE = CLIC.CODIGO  ' + #13 +
                              '  AND CHIS.CLIORDEM = CLIC.ORDEM ';

                    DMS.cdsSelOcorrencia.First;
                    while not DMS.cdsSelOcorrencia.Eof do
                    begin
                         if  (DMS.cdsSelOcorrenciaSEL.AsString = 'S')
                         and (SameText(DMS.cdsSelOcorrenciaLISTAR.AsString , 'Sim'))
                         and FH.DataOk(DMS.cdsSelOcorrenciaDATAINI.AsString)
                         and FH.DataOk(DMS.cdsSelOcorrenciaDATAFIN.AsString) then
                         begin
                              if ss = '' then
                                 ss := #13'  AND ( '
                              else
                                 ss := ss + ' OR ';

                              ss := ss +
                                 #13'     (CHIS.DATA BETWEEN ' + FG.DataSql(DMS.cdsSelOcorrenciaDATAINI.AsString) +
                                 #13'                    AND ' + FG.DataSql(DMS.cdsSelOcorrenciaDATAFIN.AsString) +
                                 #13'  AND CHIS.CODCONTATO = ' + DMS.cdsSelOcorrenciaTIPO.AsString + ')';
                         end;
                         DMS.cdsSelOcorrencia.Next;
                    end;

                    if ss > '' then
                       ss := ss + ' )';

                    vWhere := vWhere + ss;
                    ss     := '';

                    DMS.cdsSelOcorrencia.First;
                    while not DMS.cdsSelOcorrencia.Eof do
                    begin
                       if DMS.cdsSelOcorrenciaSEL.AsString = 'S'  then
                          if not SameText(DMS.cdsSelOcorrenciaLISTAR.AsString , 'Sim')   then
                          begin
                             ss := #13' AND not exists (select chis.cliente from CLICONTATOHIS CHIS '  +
                                   #13' where CHIS.CLIENTE = CLIC.CODIGO and CHIS.CLIORDEM = CLIC.ORDEM ';

                             if  FH.DataOk(DMS.cdsSelOcorrenciaDATAINI.AsString)
                             and FH.DataOk(DMS.cdsSelOcorrenciaDATAFIN.AsString) then
                             begin
                                 ss := ss +
                                      #13'   AND CHIS.DATA BETWEEN ' + FG.DataSql(DMS.cdsSelOcorrenciaDATAINI.AsString) +
                                      #13'                     AND ' + FG.DataSql(DMS.cdsSelOcorrenciaDATAFIN.AsString) +
                                      #13'   AND CHIS.CODCONTATO = ' + DMS.cdsSelOcorrenciaTIPO.AsString;
                             end;

                             ss := ss + ')'

                          end;

                       DMS.cdsSelOcorrencia.Next;
                    end;

                    vWhere := vWhere + ss;

                end;
            end;
        end;

        if DMS.cdsSelCartao.Active then
        begin
            if not DMS.cdsSelCartao.IsEmpty then
            begin
                bTemp := False;
                DMS.cdsSelCartao.First;
                while not DMS.cdsSelCartao.Eof do
                begin
                    bTemp := DMS.cdsSelCartaoSELECIONADO.AsString = 'S';
                    if bTemp then Break;
                    DMS.cdsSelCartao.Next;
                end;
                if bTemp then
                begin
                    vFrom  := vFrom + #13 +
                              '   , CLICARTAO CRTA';
                    auxStr := '';
                    DMS.cdsSelCartao.First;
                    while not DMS.cdsSelCartao.Eof do
                    begin
                        if  FH.ComparaTextoRapido(DMS.cdsSelCartaoSITUACAO.AsString, 'SOLICITADO')
                        and (DMS.cdsSelCartaoSELECIONADO.AsString = 'S')  then
                        begin
                            auxStr := auxStr + IfThen(auxStr > '', #13'  OR ') +
                                               '(    CRTA.DTSOLICITADO IS NOT NULL ' +
                                    #13'         AND CRTA.DTCONFECCIONADO  IS NULL ' +
                                    #13'         AND CRTA.DTAGUARDO        IS NULL ' +
                                    #13'         AND CRTA.DTENVIADO        IS NULL ' +
                                    #13'         AND CRTA.DTLIBERADO       IS NULL ' +
                                    #13'         AND CRTA.DTBLOQUEADO      IS NULL ';

                            if  FH.DataOk(DMS.cdsSelCartaoDATAINI.AsString)
                            and FH.DataOk(DMS.cdsSelCartaoDATAFIN.AsString) then
                                auxStr := auxStr +
                                      #13'         AND CRTA.DTSOLICITADO BETWEEN ' + FG.DataSql(DMS.cdsSelCartaoDATAINI.AsString) +
                                                                           ' AND ' + FG.DataSql(DMS.cdsSelCartaoDATAFIN.AsString);
                            auxStr := auxStr + ' )';
                        end;
                        if  FH.ComparaTextoRapido(DMS.cdsSelCartaoSITUACAO.AsString, 'CONFECCAO')
                        and (DMS.cdsSelCartaoSELECIONADO.AsString = 'S')  then
                        begin
                            auxStr := auxStr + IfThen(auxStr > '', #13'  OR ') +
                                               '(    CRTA.DTCONFECCIONADO  IS NOT NULL ' +
                                    #13'         AND CRTA.DTAGUARDO        IS NULL ' +
                                    #13'         AND CRTA.DTENVIADO        IS NULL ' +
                                    #13'         AND CRTA.DTLIBERADO       IS NULL ' +
                                    #13'         AND CRTA.DTBLOQUEADO      IS NULL ';

                            if  FH.DataOk(DMS.cdsSelCartaoDATAINI.AsString)
                            and FH.DataOk(DMS.cdsSelCartaoDATAFIN.AsString) then
                                auxStr := auxStr +
                                      #13'         AND CRTA.DTCONFECCIONADO BETWEEN ' + FG.DataSql(DMS.cdsSelCartaoDATAINI.AsString) +
                                                                              ' AND ' + FG.DataSql(DMS.cdsSelCartaoDATAFIN.AsString);
                            auxStr := auxStr + ' )';
                        end;
                        if  FH.ComparaTextoRapido(DMS.cdsSelCartaoSITUACAO.AsString, 'AGUARDO')
                        and (DMS.cdsSelCartaoSELECIONADO.AsString = 'S')  then
                        begin
                            auxStr := auxStr + IfThen(auxStr > '', #13'  OR ') +
                                               '(    CRTA.DTAGUARDO        IS NOT NULL ' +
                                    #13'         AND CRTA.DTENVIADO        IS NULL ' +
                                    #13'         AND CRTA.DTLIBERADO       IS NULL ' +
                                    #13'         AND CRTA.DTBLOQUEADO      IS NULL ';

                            if  FH.DataOk(DMS.cdsSelCartaoDATAINI.AsString)
                            and FH.DataOk(DMS.cdsSelCartaoDATAFIN.AsString) then
                                auxStr := auxStr +
                                      #13'         AND CRTA.DTAGUARDO BETWEEN ' + FG.DataSql(DMS.cdsSelCartaoDATAINI.AsString) +
                                                                        ' AND ' + FG.DataSql(DMS.cdsSelCartaoDATAFIN.AsString);
                            auxStr := auxStr + ' )';
                        end;
                        if  FH.ComparaTextoRapido(DMS.cdsSelCartaoSITUACAO.AsString, 'ENVIADO')
                        and (DMS.cdsSelCartaoSELECIONADO.AsString = 'S')  then
                        begin
                            auxStr := auxStr + IfThen(auxStr > '', #13'  OR ') +
                                               '(    CRTA.DTENVIADO        IS NOT NULL ' +
                                    #13'         AND CRTA.DTLIBERADO       IS NULL ' +
                                    #13'         AND CRTA.DTBLOQUEADO      IS NULL ';

                            if  FH.DataOk(DMS.cdsSelCartaoDATAINI.AsString)
                            and FH.DataOk(DMS.cdsSelCartaoDATAFIN.AsString) then
                                auxStr := auxStr +
                                      #13'         AND CRTA.DTENVIADO BETWEEN ' + FG.DataSql(DMS.cdsSelCartaoDATAINI.AsString) +
                                                                        ' AND ' + FG.DataSql(DMS.cdsSelCartaoDATAFIN.AsString);
                            auxStr := auxStr + ' )';
                        end;
                        if  FH.ComparaTextoRapido(DMS.cdsSelCartaoSITUACAO.AsString, 'LIBERADO')
                        and (DMS.cdsSelCartaoSELECIONADO.AsString = 'S')  then
                        begin
                            auxStr := auxStr + IfThen(auxStr > '', #13'  OR ') +
                                               '(    CRTA.DTLIBERADO       IS NOT NULL ' +
                                    #13'         AND CRTA.DTBLOQUEADO      IS NULL ';

                            if  FH.DataOk(DMS.cdsSelCartaoDATAINI.AsString)
                            and FH.DataOk(DMS.cdsSelCartaoDATAFIN.AsString) then
                                auxStr := auxStr +
                                      #13'         AND CRTA.DTLIBERADO BETWEEN ' + FG.DataSql(DMS.cdsSelCartaoDATAINI.AsString) +
                                                                         ' AND ' + FG.DataSql(DMS.cdsSelCartaoDATAFIN.AsString);
                            auxStr := auxStr + ' )';
                        end;
                        if  FH.ComparaTextoRapido(DMS.cdsSelCartaoSITUACAO.AsString, 'BLOQUEADO')
                        and (DMS.cdsSelCartaoSELECIONADO.AsString = 'S')  then
                        begin
                            auxStr := auxStr + IfThen(auxStr > '', #13'  OR ') +
                                               '(    CRTA.DTBLOQUEADO      IS NOT NULL ';

                            if  FH.DataOk(DMS.cdsSelCartaoDATAINI.AsString)
                            and FH.DataOk(DMS.cdsSelCartaoDATAFIN.AsString) then
                                auxStr := auxStr +
                                      #13'         AND CRTA.DTBLOQUEADO BETWEEN ' + FG.DataSql(DMS.cdsSelCartaoDATAINI.AsString) +
                                                                          ' AND ' + FG.DataSql(DMS.cdsSelCartaoDATAFIN.AsString);
                            auxStr := auxStr + ' )';
                        end;
                        DMS.cdsSelCartao.Next;
                    end;

                    vWhere := vWhere +
                              #13'  AND CRTA.CLICODIGO = CLIC.CODIGO ' +
                              #13'  AND CRTA.CLIORDEM  = CLIC.ORDEM ' +
                              #13'  AND ( ' + auxStr + ' )';
                end;
            end;
        end;

        if DMS.cdsSelCliTipo.Active then
          if not DMS.cdsSelCliTipo.IsEmpty then
            vWhere := vWhere + #13 +
                      '  AND DADO.TIPOCLIENTE IN ' + FH.GeraSQLIN(DMS.cdsSelCliTipo, 'CODIGO');

        if DMS.cdsSelSpc.Active then
          if not DMS.cdsSelSpc.IsEmpty then
            vWhere := vWhere + #13 +
                      '  AND DADO.SPC IN ' + FH.GeraSQLIN(DMS.cdsSelSpc, 'CODIGO');

        if DMS.cdsSelVendedor.Active then
           if not DMS.cdsSelVendedor.IsEmpty then
              vWhere := vWhere + #13 +
                        '  AND DADO.VENDEDOR IN ' + FH.GeraSQLIN(DMS.cdsSelVendedor, 'CODIGO');
      end;
    end;

    if DMS.cdsSelMetas.Active then
    begin
      if not DMS.cdsSelMetas.IsEmpty then
      begin
        vWhere := vWhere + #13 +
                '  AND META.ANO IN ' + FH.GeraSQLIN(DMS.cdsSelMetas,'ANO',True);
      end;
    end;

    if not AnsiContainsText(vFrom, 'CLIESTATISTICA') then
    begin
         vFrom := StringReplace(vFrom,
                                '/*ESTATISTICA*/',
                                'LEFT JOIN CLIESTATISTICA ESTA ON (DADO.CODIGO = ESTA.CODIGO) ', [rfIgnoreCase]);
    end;

    Result := Result      + #13 +
              vFrom       + #13 +
              vWhere      + #13 +
              '/* FIM */' + #13;

    if AOrdenacao = '' then
      Result := Result +
                ADMC.ClienteSQL_OrderBy
    else
    begin
      try
        FH.CriaLista(lsAux);
        lsAux  := FH.StrParaLista(AOrdenacao);
        auxStr := '';
        for ii := 0 to lsAux.Count-1 do
        begin
          jj := AnsiIndexText(lsAux[ii], ADMC.CampoUsu) + 1;
          if jj > 0 then
            auxStr := auxStr + IfThen(auxStr > '', ',') + ADMC.CampoTab[jj];
        end;
        if auxStr > '' then
          auxStr := #13'ORDER BY ' + auxStr;
        Result := Result + auxStr;
      finally
        FH.DestroiObj(lsAux);
      end;
    end;
  end
  else
  begin
    vFrom := ADMC.ClienteSQL_From;
    
    if not AnsiContainsText(vFrom, 'CLIESTATISTICA') then
    begin
         vFrom := StringReplace(vFrom,
                                '/*ESTATISTICA*/',
                                'LEFT JOIN CLIESTATISTICA ESTA ON (DADO.CODIGO = ESTA.CODIGO) ', [rfIgnoreCase]);
    end;

    Result := DMC.ClienteSQL_Select   + #13 +
              vFrom                   + #13 +
              ADMC.ClienteSQL_Where   + #13 +
              '/* FIM */'             + #13 +
              ADMC.ClienteSQL_OrderBy;
  end;
end;

{------------------------------------------------------------------------------}
procedure PreparaFiltroCli(PQuery: TIBOQuery);
begin
  PQuery.Filtered       := false;
  PQuery.OnFilterRecord := nil;
  if not DMS.cdsCliFiltro.Active then
    exit;
  if DMS.cdsCliFiltro.IsEmpty then
    exit;
  PQuery.OnFilterRecord := DMC.Cliente_FilterRecord;
  PQuery.Filtered       := true;
  PQuery.First;
  while not PQuery.Eof do
  begin
    PQuery.Next;
  end;
  PQuery.First;
end;

{------------------------------------------------------------------------------}
function RetSQLFornecedor(const AVerFiltro: Boolean; const AOrdenacao: string): string;
var
  vFrom  : string;
  vWhere : string;
  auxStr : string;
  lsAux  : TStrings;
  ii     : Integer;
  jj     : Integer;
begin
  if AVerFiltro then
  begin
    Result := DMP.FornecedorSQL_Select;
    vFrom  := ADMP.FornecedorSQL_From;
    vWhere := ADMP.FornecedorSQL_Where;

    if DMS.cdsForFiltro.Active then
    begin
      if not DMS.cdsForFiltro.IsEmpty then
      begin
        //
      end;
    end;

    Result := Result      + #13 +
              vFrom       + #13 +
              vWhere      + #13 +
              '/* FIM */' + #13;

    if AOrdenacao = '' then
      Result := Result +
                ADMP.FornecedorSQL_OrderBy
    else
    begin
      try
        FH.CriaLista(lsAux);
        lsAux  := FH.StrParaLista(AOrdenacao);
        auxStr := '';
        for ii := 0 to lsAux.Count-1 do
        begin
          jj := AnsiIndexText(lsAux[ii], ADMP.CampoUsu) + 1;
          if jj > 0 then
            auxStr := auxStr + IfThen(auxStr > '', ',') + ADMP.CampoTab[jj];
        end;
        if auxStr > '' then
          auxStr := #13'ORDER BY ' + auxStr;
        Result := Result + auxStr;
      finally
        FH.DestroiObj(lsAux);
      end;
    end;
  end
  else
  begin
    Result := DMP.FornecedorSQL_Select   + #13 +
              ADMP.FornecedorSQL_From    + #13 +
              ADMP.FornecedorSQL_Where   + #13 +
              '/* FIM */'                + #13 +
              ADMP.FornecedorSQL_OrderBy;
  end;
end;

{------------------------------------------------------------------------------}
procedure PreparaFiltroFor(PQuery: TIBOQuery);
begin
  PQuery.Filtered       := false;
  PQuery.OnFilterRecord := nil;
  if not DMS.cdsForFiltro.Active then
    exit;
  if DMS.cdsForFiltro.IsEmpty then
    exit;
  PQuery.OnFilterRecord := DMP.qrFornecedorRelFilterRecord;
  PQuery.Filtered       := true;
  PQuery.First;
  while not PQuery.Eof do
  begin
    PQuery.Next;
  end;
  PQuery.First;
end;

{------------------------------------------------------------------------------}
function QualificaSQL(const ASQL: string): string;
var
  tx,
  token,
  Tab    : string;
  lc,
  lr,
  ls     : TStrings;
  ii,
  jj,
  cc,
  blk    : integer;
  cr     : TIB_Cursor;
begin
  Result := ASQL;
  if not AnsiContainsText(Result, '/*REF') then
    exit;
  tx := FH.CopyPosMax(Result, '/*REF');
  tx := FH.CopyPos(tx, '*/');
  if FH.StrInvalida(tx) then
    exit;
  try
    FH.CriaLista(lc);
    FH.CriaLista(lr);
    FH.CriaLista(ls);
    cr := TIB_Cursor.Create(nil);
    cr.IB_Connection := DMG.IB_Connection;

    lc.Text := tx;
    for ii := 0 to lc.Count - 1 do
    begin
      token := lc[ii];
      tab   := FH.CopyPosMax(token, '=');
      token := FH.CopyPos(token, '=');
      if  FH.StrOk(tab)
      and AnsiContainsText(Result, token + '.*') then
      begin
        blk := 0;
        ls.Text := Result;
        for jj := 0 to ls.Count - 1 do
          if AnsiContainsText(ls[jj], token + '.*') then
          begin
            for cc := 1 to Length(ls[jj]) do
              if ls[jj][cc] <> #32 then
                break
              else
                inc(blk);
            break;
          end;

        cr.Close;
        cr.SQL.Text := ' SELECT RDB$FIELD_POSITION, RDB$FIELD_NAME AS NOME_CAMPO ' +
                       '   FROM RDB$RELATION_FIELDS ' +
                       '  WHERE UPPER(RDB$RELATION_NAME) = UPPER(' + QuotedStr(tab) + ') ' +
                       ' ORDER BY 1';
        cr.Open;
        cr.First;
        lr.Clear;
        while not cr.Eof do
        begin
          lr.Add(StringOfChar(#32, blk) + token + '.' +
                 cr.FieldByName('NOME_CAMPO').AsString);
          cr.Next;
        end;
        for jj := 0 to lr.Count - 2 do // ... não coloca ',' no último campo
        begin
          lr[jj] := lr[jj] + ',';
          if jj = 0 then
            lr[jj] := trim(lr[jj]);
        end;

        Result := StringReplace(Result, token + '.*', lr.Text, [rfIgnoreCase]);
      end;
    end;
  finally
    FH.DestroiObj(lc);
    FH.DestroiObj(lr);
    FH.DestroiObj(ls);
    FH.DestroiObj(cr);
  end;
end;

{------------------------------------------------------------------------------}
procedure GravaInfoFiltro(FR: TForm; AContainers: array of TWinControl;
  ACdsSel: array of TClientDataSet);
var
  ii,
  jj,
  zz,
  xa      : integer;
  tmpCtr  : TControl;
  tmpStr  : string;
  tmpCds  : TClientDataSet;
begin
  for jj := Low(AContainers) to High(AContainers) do
    for ii := 0 to AContainers[jj].ControlCount - 1 do
    begin
      tmpCtr := AContainers[jj].Controls[ii];

      if tmpCtr is TEdit then
        FH.GravaRegistry(FG.REG_CHAVE_FILTRO, TEdit(tmpCtr).Name, TEdit(tmpCtr).Text, PathDelim + FR.Name)
      else
      if tmpCtr is TMemo then
        FH.GravaRegistry(FG.REG_CHAVE_FILTRO, TMemo(tmpCtr).Name, TMemo(tmpCtr).Lines.Text, PathDelim + FR.Name)
      else
      if tmpCtr is TMaskEdit then
        FH.GravaRegistry(FG.REG_CHAVE_FILTRO, TMaskEdit(tmpCtr).Name, TMaskEdit(tmpCtr).Text, PathDelim + FR.Name)
      else
      if tmpCtr is TJvDateEdit then
      begin
        if FH.DataOk(TJvDateEdit(tmpCtr).Text) then
          FH.GravaRegistry(FG.REG_CHAVE_FILTRO, TJvDateEdit(tmpCtr).Name, TJvDateEdit(tmpCtr).Text, PathDelim + FR.Name)
        else
          FH.GravaRegistry(FG.REG_CHAVE_FILTRO, TJvDateEdit(tmpCtr).Name, '', PathDelim + FR.Name);
      end
      else
      if tmpCtr is TJvCalcEdit then
        FH.GravaRegistry(FG.REG_CHAVE_FILTRO, TJvCalcEdit(tmpCtr).Name, FloatToStr(TJvCalcEdit(tmpCtr).Value), PathDelim + FR.Name)
      else
      if tmpCtr is TJvSpinEdit then
        FH.GravaRegistry(FG.REG_CHAVE_FILTRO, TJvSpinEdit(tmpCtr).Name, IntToStr(TJvSpinEdit(tmpCtr).AsInteger), PathDelim + FR.Name)
      else
      if tmpCtr is TComboBox then
        FH.GravaRegistry(FG.REG_CHAVE_FILTRO, TComboBox(tmpCtr).Name, IntToStr(TComboBox(tmpCtr).ItemIndex), PathDelim + FR.Name)
      else
      if tmpCtr is TListBox then
        FH.GravaRegistry(FG.REG_CHAVE_FILTRO, TListBox(tmpCtr).Name, IntToStr(TListBox(tmpCtr).ItemIndex), PathDelim + FR.Name)
      else
      if tmpCtr is TGroupBox then
        FG.GravaInfoFiltro(FR, [(tmpCtr as TGroupBox)], [])
      else
      if tmpCtr is TCheckBox then
        FH.GravaRegistry(FG.REG_CHAVE_FILTRO, tmpCtr.Name, FH.BoolParaStr(TCheckBox(tmpCtr).Checked), PathDelim + FR.Name)
      else
      if tmpCtr is TListView then
      begin
        if TListView(tmpCtr).Checkboxes then
          if TListView(tmpCtr).ViewStyle = vsReport then
            for zz := 0 to pred(TListView(tmpCtr).Items.Count) do
            begin
              if TListView(tmpCtr).HelpKeyword = 'SALVA_TUDO' then
              begin
                tmpStr := '';
                tmpStr := tmpStr + #156 + TListView(tmpCtr).Items[zz].Caption;
                tmpStr := tmpStr + #156 + FH.BoolParaStr(TListView(tmpCtr).Items[zz].Checked);
                tmpStr := tmpStr + #156;
                for xa := 0 to pred(TListView(tmpCtr).Items[zz].SubItems.Count) do
                  tmpStr := tmpStr +
                            TListView(tmpCtr).Items[zz].SubItems[xa] + #215;
                tmpStr := tmpStr + #156;

                FH.GravaRegistry(FG.REG_CHAVE_FILTRO,
                                 tmpCtr.Name + '_' +
                                 FH.StrZero(TListView(tmpCtr).Items[zz].Index, 10),
                                 tmpStr, PathDelim + FR.Name);
              end
              else
                FH.GravaRegistry(FG.REG_CHAVE_FILTRO,
                                 tmpCtr.Name + '_' +
                                 TListView(tmpCtr).Items[zz].Caption,
                                 FH.BoolParaStr(TListView(tmpCtr).Items[zz].Checked),
                                 PathDelim + FR.Name);
            end;
      end
      else
      if tmpCtr is TCheckListBox then
      begin
        for zz := 0 to pred(TCheckListBox(tmpCtr).Items.Count) do
        begin
            FH.GravaRegistry(FG.REG_CHAVE_FILTRO,
                             tmpCtr.Name + '_' +
                             TCheckListBox(tmpCtr).Items[zz],
                             FH.BoolParaStr(TCheckListBox(tmpCtr).Checked[zz]),
                             PathDelim + FR.Name);
        end;
      end
      else
      if tmpCtr is TPanel then
        FG.GravaInfoFiltro(FR, [(tmpCtr as TPanel)], []);
    end;
  for jj := Low(ACdsSel) to High(ACdsSel) do
  begin
    tmpCds := ACdsSel[jj];
    tmpStr := FG.SetaLocalRelatorio + FR.Name + tmpCds.Name + '.xml';
    if tmpCds.RecordCount > 0 then
      FH.GravaXML(FR, tmpCds, tmpStr)
    else
    if FileExists(tmpStr) then
      SysUtils.DeleteFile(tmpStr);
  end;
end;

{------------------------------------------------------------------------------}
procedure LeInfoFiltro(FR: TForm; AContainers: array of TWinControl;
  ACdsSel: array of TClientDataSet);
var
  ii,
  jj,
  tt,
  zz       : integer; 
  tmpCtr   : TControl;
  tmpStr,
  S        : string;
  vl       : Double;
  tmpCds,
  vCDS     : TClientDataSet;
  ls       : TStrings;
  li       : TListItem;
begin
  FH.CriaLista(ls);
  for jj := Low(AContainers) to High(AContainers) do
    for ii := 0 to AContainers[jj].ControlCount - 1 do
    begin
      tmpCtr := AContainers[jj].Controls[ii];

      if tmpCtr is TEdit then
      begin
        tmpStr := FH.LeRegistry(FG.REG_CHAVE_FILTRO, TEdit(tmpCtr).Name, PathDelim + FR.Name);
        if tmpStr <> '' then
          TEdit(tmpCtr).Text := tmpStr;
      end
      else
      if tmpCtr is TMemo then
      begin
        tmpStr := FH.LeRegistry(FG.REG_CHAVE_FILTRO, TMemo(tmpCtr).Name, PathDelim + FR.Name);
        if tmpStr <> '' then
          TMemo(tmpCtr).Lines.Text := tmpStr;
      end
      else
      if tmpCtr is TMaskEdit then
      begin
        tmpStr := FH.LeRegistry(FG.REG_CHAVE_FILTRO, TMaskEdit(tmpCtr).Name, PathDelim + FR.Name);
        if tmpStr <> '' then
          TMaskEdit(tmpCtr).Text := tmpStr;
      end
      else
      if tmpCtr is TJvDateEdit then
      begin
        tmpStr := FH.LeRegistry(FG.REG_CHAVE_FILTRO, TJvDateEdit(tmpCtr).Name, PathDelim + FR.Name);
        if FH.DataOk(tmpStr) then
          TJvDateEdit(tmpCtr).Text := tmpStr;
      end
      else
      if tmpCtr is TJvCalcEdit then
      begin
        vl := StrToFloatDef(FH.LeRegistry(FG.REG_CHAVE_FILTRO, TJvCalcEdit(tmpCtr).Name, PathDelim + FR.Name), FG.REG_ID_DEFAULT);
        if vl <> FG.REG_ID_DEFAULT then
          TJvCalcEdit(tmpCtr).Value := vl;
      end
      else
      if tmpCtr is TJvSpinEdit then
      begin
        zz := StrToIntDef(FH.LeRegistry(FG.REG_CHAVE_FILTRO, TJvSpinEdit(tmpCtr).Name, PathDelim + FR.Name), FG.REG_ID_DEFAULT);
        if zz <> FG.REG_ID_DEFAULT then
          TJvSpinEdit(tmpCtr).AsInteger := zz;
      end
      else
      if tmpCtr is TCheckBox then
      begin
        tmpStr := FH.LeRegistry(FG.REG_CHAVE_FILTRO, tmpCtr.Name, PathDelim + FR.Name);
        if tmpStr <> '' then
          TCheckBox(tmpCtr).Checked := FH.StrParaBool(tmpStr);
      end
      else
      if tmpCtr is TListView then
      begin
        if TListView(tmpCtr).Checkboxes then
          if TListView(tmpCtr).ViewStyle = vsReport then
            if TListView(tmpCtr).HelpKeyword = 'SALVA_TUDO' then
            begin
              FH.CarregaRegistry(FG.REG_CHAVE_FILTRO, ls, PathDelim + FR.Name);
              for zz := 0 to pred(ls.Count) do
              begin
                if FH.ComparaTextoFixo(ls[zz], TListView(tmpCtr).Name + '_') then
                  if FH.IntOk(Copy(FH.CopyPosMax(ls[zz], '_'), 1, 10)) then
                  begin
                    tmpStr := FH.LeRegistry(FG.REG_CHAVE_FILTRO,
                                            ls[zz],
                                            PathDelim + FR.Name);
                    li := TListView(tmpCtr).Items.Add;
                    if tmpStr > '' then
                    begin
                      tmpStr := FH.CopyPosMax(tmpStr, #156);
                      S := FH.CopyPos(tmpStr, #156);
                      li.Caption := S;
                      tmpStr := FH.CopyPosMax(tmpStr, #156);
                      S := FH.CopyPos(tmpStr, #156);
                      li.Checked := FH.StrParaBool(S);
                      tmpStr := FH.CopyPosMax(tmpStr, #156);
                      S := FH.CopyPos(tmpStr, #156);
                      while Pos(#215, S) > 0 do
                      begin
                        li.SubItems.Add(FH.CopyPos(S, #215));
                        S := FH.CopyPosMax(S, #215);
                        if Trim(S) = #215 then
                          S := '';
                      end;
                    end;
                  end;
              end;
            end
            else
            begin
              for zz := 0 to pred(TListView(tmpCtr).Items.Count) do
              begin
                tmpStr := FH.LeRegistry(FG.REG_CHAVE_FILTRO,
                                        tmpCtr.Name + '_' +
                                        TListView(tmpCtr).Items[zz].Caption,
                                        PathDelim + FR.Name);
                if tmpStr <> '' then
                  TListView(tmpCtr).Items[zz].Checked := FH.StrParaBool(tmpStr);
              end;
            end;
      end
      else
      if tmpCtr is TCheckListBox then
      begin
        for zz := 0 to pred(TCheckListBox(tmpCtr).Items.Count) do
        begin
          tmpStr := FH.LeRegistry(FG.REG_CHAVE_FILTRO,
                                  tmpCtr.Name + '_' +
                                  TCheckListBox(tmpCtr).Items[zz],
                                  PathDelim + FR.Name);
          if tmpStr <> '' then
            TCheckListBox(tmpCtr).Checked[zz] := FH.StrParaBool(tmpStr);
        end;
      end
      else
      if tmpCtr is TComboBox then
      begin
        tt := StrToIntDef(FH.LeRegistry(FG.REG_CHAVE_FILTRO, TComboBox(tmpCtr).Name, PathDelim + FR.Name), FG.REG_ID_DEFAULT);
        if tt <> FG.REG_ID_DEFAULT then
          TComboBox(tmpCtr).ItemIndex := tt;
      end
      else
      if tmpCtr is TListBox then
      begin
        tt := StrToIntDef(FH.LeRegistry(FG.REG_CHAVE_FILTRO, TListBox(tmpCtr).Name, PathDelim + FR.Name), FG.REG_ID_DEFAULT);
        if tt <> FG.REG_ID_DEFAULT then
          TListBox(tmpCtr).ItemIndex := tt;
      end
      else
      if tmpCtr is TGroupBox then
        FG.LeInfoFiltro(FR, [(tmpCtr as TGroupBox)], [])
      else
      if tmpCtr is TPanel then
        FG.LeInfoFiltro(FR, [(tmpCtr as TPanel)], []);
    end;
  try
    tmpCds := TClientDataSet.Create(nil);
    for jj := Low(ACdsSel) to High(ACdsSel) do
    begin
      vCDS := ACdsSel[jj];
      tmpStr := FG.SetaLocalRelatorio + FR.Name + vCDS.Name + '.xml';
      if FileExists(tmpStr) then
      begin
        tmpCds.Close;
        tmpCds.LoadFromFile(tmpStr);
        if tmpCds.Active and vCDS.Active then
        begin
          FH.ClonaTudo(tmpCds, vCDS, true);
          vCds.First;
          tmpCds.First;
        end;
      end;
    end;
  finally
    tmpCds.Free;
  end;
end;

{------------------------------------------------------------------------------}
procedure RemoveInfoFiltro(const AFormSel: string);
begin
  FH.LimpaRegistry(FG.REG_CHAVE_FILTRO, true, '', PathDelim + AFormSel);
  FH.DeletaArq(FG.SetaLocalRelatorio, AFormSel + '*.xml');
end;

{------------------------------------------------------------------------------}
procedure CriaIB_Cursor(var ACursor: TIB_Cursor; const ASQL: string);
begin
  ACursor := TIB_Cursor.Create(nil);
  ACursor.IB_Connection := DMG.IB_Connection;
  ACursor.SQL.Text      := ASQL;
end;

{------------------------------------------------------------------------------}
procedure CriaIBOQuery(var PQuery: TIBOQuery; const ASQL: string = '');
begin
  PQuery := TIBOQuery.Create(nil);
  PQuery.IB_Connection := DMG.IB_Connection;
  PQuery.SQL.Text      := ASQL;
end;

{------------------------------------------------------------------------------}
function MoedaEstrangeira(const AMoeda: string): boolean;
var
  qrBusca: TIBOQuery;
begin
  Result := false;
  if FH.StrInvalida(AMoeda) then
    exit;
  try
    FG.CriaIBOQuery(qrBusca, '');
    qrBusca.SQL.Text := 'select * from tabmoeda where codigo = ' + QuotedStr(AMoeda);
    qrBusca.Open;
    if qrBusca.IsEmpty then
      exit;
    if SameText('R', LeftStr(Trim(qrBusca.FieldbyName('SIMBOLO').AsString), 1)) then
      exit;
    Result := true;
  finally
    FH.DestroiObj(qrBusca);
  end;
end;

{------------------------------------------------------------------------------}
function MoedaEstrangeira(PQuery: TIBOQuery; const AMoeda: string): boolean;
begin
  Result := false;
  if FH.StrInvalida(AMoeda) then
    exit;
  if not PQuery.Active then
  begin
    if PQuery.SQL.Text = '' then
      PQuery.SQL.Text := 'select * from tabmoeda';
    PQuery.Open;
  end;
  if not PQuery.Locate('CODIGO', AMoeda, [loCaseInsensitive]) then
    exit;
  if SameText('R', LeftStr(Trim(PQuery.FieldbyName('SIMBOLO').AsString), 1)) then
    exit;
  Result := true;
end;

{------------------------------------------------------------------------------}
function LocCotacao(PQuery: TIBOQuery; const AMoeda, AData: string;
  const AVerMoeda: boolean): double;
var
  vData: string;
begin
  Result := 0;
  vData  := AData;

  if vData = '' then
    vData := FG.DataStrLocal
  else
  if FH.DataInvalida(vData) then
    raise exception.CreateFmt('Data de Cotação inválida: %s', [QuotedStr(vData)]);

  if AVerMoeda then
  begin
    if not FG.MoedaEstrangeira(PQuery, AMoeda) then
      exit;
  end;

  PQuery.Close;
  PQuery.SQL.Text := 'select * from tabmoedaval ' +
                      'where codigo = ' + QuotedStr(AMoeda) +
                      '  and data   = ' + FG.DataSql(vData);
  PQuery.Open;
  if PQuery.IsEmpty then
    exit;

  Result := PQuery.FieldByName('VALOR').AsFloat;
end;

{------------------------------------------------------------------------------}
function LocCotacao(const AMoeda: string; const AData: string = ''): double;
var
  qrBusca: TIBOQuery;
begin
  Result := 0;
  try
    FG.CriaIBOQuery(qrBusca, '');

    Result := FG.LocCotacao(qrBusca, AMoeda, AData, true);
  finally
    FH.DestroiObj(qrBusca);
  end;
end;

{------------------------------------------------------------------------------}
function VerInscricaoEst(const AInscricao, AUF: string): boolean;
type
  TConsisteInscricaoEstadual = function(const Insc, UF: string): Integer; stdcall;
var
  vRet : Integer;
  H    : THandle;
  S    : string;
  F    : TConsisteInscricaoEstadual;
begin
  try
    Result := true;

    S := FH.StrInteiro(AInscricao);
    if FH.StrInvalida(S) then
      exit;

    H := LoadLibrary(PChar(Trim('DllInscE32.Dll')));
    if H <= HINSTANCE_ERROR then
      exit;
    @F := GetProcAddress(H, 'ConsisteInscricaoEstadual');
    if @F = nil then
      exit;

    Result := false;

    vRet := F(S, AUF);

    Result := vRet = 0;
  finally
    FreeLibrary(H);
  end;
end;

{------------------------------------------------------------------------------}
function RetDataNull: string;
begin
  Result := StringReplace(FG.MASK_DATA, '9', #32, [rfReplaceAll]);
end;

{------------------------------------------------------------------------------}
function VerVendedor(const AModulo, ACodigo: string; ALabel: TLabel; const PMsg: Boolean = True): Boolean;
var
  tmpQr: TIBOQuery;
begin
  Result := True;

  if ALabel <> nil then
    ALabel.Caption := '';

  if ACodigo = '' then
    Exit;

  Result := False;

  try
    FG.CriaIBOQuery(tmpQr, '');

    if FH.IntInvalido(ACodigo)
    or (not FG.FindKeyLivre(tmpQr, 'TABFUNCIONARIO', 'CODIGO', [ACodigo])) then
    begin
      if PMsg then
         FH.PrMsg_ER('Vendedor não cadastrado.');
      Exit;
    end;

    if ALabel <> nil then
      ALabel.Caption := tmpQr.FieldByName('NOME').AsString;

    if FH.DataOk(tmpQr.FieldByName('DTDEMISSAO').AsString) then
    begin
      if PMsg then
         FH.PrMsg_ER('O vendedor %s foi demitido.', [QuotedStr(tmpQr.FieldByName('NOME').AsString)]);
      Exit;
    end;

    if ((AModulo = 'VENDA' ) and (tmpQr.FieldByName('PERMITEVENDA' ).AsString <> 'S'))
    or ((AModulo = 'COMPRA') and (tmpQr.FieldByName('PERMITECOMPRA').AsString <> 'S')) then
    begin
      if PMsg then
         FH.PrMsg_AT('O funcionário %s não possui permissão para efetuar %s.',
                     [QuotedStr(tmpQr.FieldByName('NOME').AsString), AnsiLowerCase(AModulo)]);
      Exit;
    end;

    Result := True;
  finally
    FH.DestroiObj(tmpQr);
  end;
end;

{------------------------------------------------------------------------------}
procedure ChamaFotoProduto(FR: TForm; const PCodigo, PDescricao: string);
begin
    if FProImagem = nil then
       FProImagem := TFProImagem.Create(FR);
    FProImagem.CarregaTela(PCodigo, PDescricao);
    FProImagem.Caption := FR.Caption + ' - Imagem';
    FG.CriaItemPopupMenu(FProImagem);
    FProImagem.Show;
end;

{------------------------------------------------------------------------------}
function VerLocate(DS: TDataSet; const ALocaliza: string; const PChave: string = 'CODIGO'): boolean;
begin
  Result := true;
  if DS.Active then
    if not DS.IsEmpty then
    begin
      if ALocaliza = '' then
        Result := False
      else
        Result := DS.Locate(PChave, ALocaliza, [loCaseInsensitive]);
    end;
end;

{------------------------------------------------------------------------------}
procedure SetaPapelParede;
var
  qr: TIBOQuery;
begin
  FMenu.imGeral.Visible   := false;
  FMenu.imGeral.Align     := alClient;

  try
    FG.CriaIBOQuery(qr, 'SELECT PAPELPAREDE FROM CFGESTABE ' +
                        ' WHERE ESTABE = ' + QuotedStr(FG.RetEstabeAtivo));
    qr.Open;
    try
      FH.LeImagemTab(FMenu.imGeral.Picture, qr, 'PAPELPAREDE',
                     FG.LeConfigEstabeStr(FG.CfgEstExtPapelParede, 'BMP'));
    except
      FMenu.imGeral.Picture := FMenu.imPressier.Picture;
    end;

    if FMenu.imGeral.Picture.Width < 1 then
    begin
         FMenu.imGeral.Picture := FMenu.imPressier.Picture;

         FMenu.imGeral.Properties.Stretch := False;
         FMenu.imGeral.Properties.Center  := True;
    end
    else
    begin
         FMenu.imGeral.Properties.Stretch := FG.LeConfigEstabeBool(FG.CfgEstEstenderPapelParede);
         FMenu.imGeral.Properties.Center  := not FG.LeConfigEstabeBool(FG.CfgEstEstenderPapelParede);
    end;
  finally
    FH.DestroiObj(qr);
  end;

  if FH.VerSessaoRemota then
  begin
       FMenu.imGeral.Visible        := False;
       FMenu.imSessaoRemota.Visible := True;
       FMenu.imSessaoRemota.Align   := alClient;
  end
  else
  begin
       FMenu.imGeral.Visible        := true;
       FMenu.imSessaoRemota.Visible := False;
  end;
end;

{------------------------------------------------------------------------------}
function RetImpDef: integer;
begin
    Result := Printer.PrinterIndex;
end;

{------------------------------------------------------------------------------}
function RetNomeImpDef: string;
begin
    try
       if Printer.PrinterIndex = -1 then
          Result := ''
       else
          Result := Printer.Printers[Printer.PrinterIndex];
    except
       Result := '';
    end;
end;

{------------------------------------------------------------------------------}
procedure ChamaCfgRelFast(FR: TForm; DS: TDataSet; const Modulo: string; const Nome: string = '');
begin
  if FCfgRelFast = nil then
    FCfgRelFast := TFCfgRelFast.Create(FR);

  FG.FindKey(FCfgRelFast.qrCfgRelFast, 'MODULO', [Modulo]);
  if Nome > '' then
    FCfgRelFast.qrCfgRelFast.Locate('NOME', Nome, [loCaseInsensitive]);

  FCfgRelFast.Dados.DataSet := DS;
  FCfgRelFast.Dados.GetFieldList(FCfgRelFast.lbCampos.Items);
  FCfgRelFast.Caption := FR.Caption + ' (Layout)';
  FCfgRelFast.Show;
end;

{------------------------------------------------------------------------------}
procedure ImprimeCfgRelFast(FR: TForm; DS: TDataSet; const Modulo, Nome: string);
var
  arq ,
  tx  : string;
  ii  : Integer;
  qcf : TIBOQuery;
  pic : TPicture;
begin
  try
    pic := TPicture.Create;
    FG.CriaIBOQuery(qcf);
    qcf.SQL.Text := 'SELECT * FROM CFGESTABE WHERE ESTABE = ' + quotedStr(FG.RetEstabeAtivo);
    qcf.Open;
    FH.LeImagemTab(pic, qcf, 'MINILOGO', FG.LeConfigEstabeStr(FG.CfgEstExtMiniLogo, 'BMP'));

    try
      if FCfgRelFastQ = nil then
        FCfgRelFastQ := TFCfgRelFastQ.Create(FR);

      FG.FindKey(FCfgRelFastQ.qrCfgRelFast, 'MODULO,NOME', [Modulo, Nome]);
      if FCfgRelFastQ.qrCfgRelFast.IsEmpty then
      begin
        FH.PrMsg_ER('Impossível localizar o relatório.');
        Exit;
      end;

      arq := FG.SetaLocalRelatorio + 'RELFAST_IMPRESSAO.fr3';
      FH.GeraArqTexto(arq, FCfgRelFastQ.qrCfgRelFast.FieldByName('LAYOUT').AsString);

      if FCfgRelFastQ.qrCfgRelFast.FieldByName('ORDENACAO').AsString > '' then
        if DS is TClientDataSet then
          (DS as TClientDataSet).IndexFieldNames := StringReplace(FCfgRelFastQ.qrCfgRelFast.FieldByName('ORDENACAO').AsString, ',', ';', [rfReplaceAll]);

      FCfgRelFastQ.Dados.DataSet := DS;
      FCfgRelFastQ.Dados.GetData;
      FCfgRelFastQ.frxRep.Clear;
      FCfgRelFastQ.frxRep.LoadFromFile(arq);
      FCfgRelFastQ.frxRep.DataSets.Clear;
      FCfgRelFastQ.frxRep.DataSets.Add(FCfgRelFastQ.Dados);

      for ii := 0 to FCfgRelFastQ.frxRep.AllObjects.Count-1 do
      begin
          if TfrxComponent(FCfgRelFastQ.frxRep.AllObjects[ii]) is TfrxMemoView then
          begin
              with (TfrxComponent(FCfgRelFastQ.frxRep.AllObjects[ii]) as TfrxMemoView) do
              begin
                  tx := Trim(FH.LimpaBreak(Memo.Text));

                  if SameText(tx, '$ESTABE') then
                     Memo.Text := FG.RetEstabeRel
                  else
                  if SameText(tx, '$USUARIO') then
                     Memo.Text := FG.RetUsuAtivo
                  else
                  if SameText(tx, '$TITULO') then
                     Memo.Text := Nome;
              end;
          end
          else
          if TfrxComponent(FCfgRelFastQ.frxRep.AllObjects[ii]) is TfrxPictureView then
          begin
              with (TfrxComponent(FCfgRelFastQ.frxRep.AllObjects[ii]) as TfrxPictureView) do
              begin
                  if SameText(TagStr, '$LOGO') then
                  begin
                     Picture := pic;
                  end;
              end;
          end;
      end;

      FG.SetaFrxExports(FCfgRelFastQ, FH.SetaNomeArqValido(Nome));

      FCfgRelFastQ.frxRep.ShowReport;
    finally
      FCfgRelFastQ.Close;
    end;
  finally
    FH.DestroiMultiObj([qcf, pic]);
  end;
end;

{------------------------------------------------------------------------------}
function LeParEstabeStr(const PEstabe, pID: string; const PDefault: string = ''): string;
begin
  Result := FG.LeIniStr(FG.LeConfigEstabe(PEstabe), pID, PDefault);
end;

{------------------------------------------------------------------------------}
function LeParEstabeBool(const PEstabe, pID: string; const PDefault: boolean = false): boolean;
begin
  Result := FG.LeIniBool(FG.LeConfigEstabe(PEstabe), pID, PDefault);
end;

{------------------------------------------------------------------------------}
function LeParEstabeInt(const PEstabe, pID: string; const PDefault: integer = 0): integer;
begin
  Result := FG.LeIniInt(FG.LeConfigEstabe(PEstabe), pID, PDefault);
end;

{------------------------------------------------------------------------------}
function LeParEstabeNum(const PEstabe, pID: string; const PDefault: double = 0): double;
begin
  Result := FG.LeIniNum(FG.LeConfigEstabe(PEstabe), pID, PDefault);
end;

{------------------------------------------------------------------------------}
procedure GravaParEstabeStr(const PEstabe, pID, PValor: string; TR: TIB_Transaction = nil);
var
  lst: TStrings;
  tmpQuery: TIBOQuery;
begin
  try
    tmpQuery := TIBOQuery.Create(nil);
    tmpQuery.IB_Connection := DMG.IB_Connection;

    if TR <> nil then
      tmpQuery.IB_Transaction := TR;

    tmpQuery.SQL.Text := 'SELECT * FROM CFGESTABE WHERE ESTABE = ' + QuotedStr(PEstabe);
    tmpQuery.Open;

    FH.IniGera(lst, tmpQuery.FieldByName('CONFIG').AsString);
    FH.IniGravaStr(lst, FG.SECAO_GERAL, pID, PValor);

    tmpQuery.Close;
    tmpQuery.SQL.Text := 'UPDATE CFGESTABE SET CONFIG= ' + QuotedStr(lst.Text) +
                         ' WHERE ESTABE = ' + QuotedStr(PEstabe);
    tmpQuery.ExecSQL;
  finally
    FH.DestroiObj(lst);
    FH.DestroiObj(tmpQuery);
  end;
end;

{------------------------------------------------------------------------------}
procedure ZeraValorDataSet(DS: TDataSet);
var
   ii : Integer;
begin
   for ii := 0 to DS.Fields.Count-1 do
   begin
     try
       if FH.VerCampoNum(DS.Fields.Fields[ii]) then
         DS.Fields.Fields[ii].Value := 0;
     except
       //
     end;
   end;
end;

{------------------------------------------------------------------------------}
function ChamaCaxMov(FR: TForm; const pCliente, ANomeCli, ACpfCpnf, ACaixa, AData, AOrg, AIDVen, ADesHis, ACupomTef: string;
  const PValor: Extended; TR: TIB_Transaction = nil): Boolean;
begin
     Result             := False;
     CaxMov.vrgStatus   := '';
     CaxMov.vrgCliente  := FGCli.RetCliCodigo(pCliente);
     CaxMov.vrgOrdem    := FGCli.RetCliOrdem (pCliente);
     CaxMov.vrgNomeCli  := ANomeCli;
     CaxMov.vrgCpfCnpj  := ACpfCpnf;
     CaxMov.vrgCaixa    := ACaixa;
     CaxMov.vrgValor    := PValor;
     CaxMov.vrgTroco    := PValor;
     CaxMov.vrgData     := AData;
     CaxMov.vrgOrigem   := AOrg;
     CaxMov.vrgIDVen    := AIDVen;
     CaxMov.vrgDesHis   := ADesHis;
     CaxMov.vrgTrans    := TR;
     CaxMov.vrgCupomTef := ACupomTef;

     try
        FCaxMov := TFCaxMov.Create(Application);
        FCaxMov.Width  := 800;
        FCaxMov.Height := 600;

        if FG.VerModoDebug then
           if FCaxMov.FormStyle = fsStayOnTop then
              FG.CancelaTopMost(FCaxMov.Handle);

        if (Screen.Height > 700) and (not FMenu.pnLoja.Visible) then
        begin
             FCaxMov.Left := FR.Left;
             FCaxMov.Top  := FR.Top;
        end
        else
        begin
             FCaxMov.Left := Round(Screen.Width  / 2) - Round(FCaxMov.Width  / 2);
             FCaxMov.Top  := Round(Screen.Height / 2) - Round(FCaxMov.Height / 2);
        end;

        FCaxMov.ShowModal;
        Result := CaxMov.vrgStatus = 'FINALIZADO';
     finally
            FCaxMov.Free;
     end;
     Application.ProcessMessages;
end;

{------------------------------------------------------------------------------}
function ChamaRecMov(FR: TForm; const pCliente, PNome, AIdentidade, ACpf, ACaixa, AData, AIDVen, AOrg, ACupomTef: string;
                     const PValor: Extended; const AFatura, ATipoVenda: string; const ASugereCndPgt: string = '';
                     TR: TIB_Transaction = nil): Boolean;
begin
     Result := False;

     FG.GravaCacheStr(FG.RecMovContrato, '');

     RecMov.vrgStatus      := '';
     RecMov.vrgContrato    := '';
     RecMov.vrgCliente     := FGCli.RetCliCodigo(pCliente);
     RecMov.vrgOrdem       := FGCli.RetCliOrdem (pCliente);
     RecMov.vrgNome        := PNome;
     RecMov.vrgCaixa       := ACaixa;
     RecMov.vrgValor       := PValor;
     RecMov.vrgFatura      := AFatura;
     RecMov.vrgData        := AData;
     RecMov.vrgTrans       := TR;
     RecMov.vrgSugereCnPgt := ASugereCndPgt;
     RecMov.vrgIDVen       := AIDVen;
     RecMov.vrgOrg         := AOrg;
     RecMov.vrgCupomTef    := ACupomTef;
     RecMov.vrgIdentidade  := AIdentidade;
     RecMov.vrgCPF         := ACpf;
     RecMov.vrgTipoVenda   := ATipoVenda;

     try
        FRecMov := TFRecMov.Create(Application);
        FRecMov.Width  := 800;
        FRecMov.Height := 600;

        if (Screen.Height > 700) and (not FMenu.pnLoja.Visible) then
        begin
             FRecMov.Left := FR.Left;
             FRecMov.Top  := FR.Top;
        end
        else
        begin
             FRecMov.Left := Round(Screen.Width  / 2) - Round(FRecMov.Width  / 2);
             FRecMov.Top  := Round(Screen.Height / 2) - Round(FRecMov.Height / 2);
        end;

        FRecMov.ShowModal;
        Result := RecMov.vrgStatus = 'FINALIZADO';
        if Result then
           FG.GravaCacheStr(FG.RecMovContrato, RecMov.vrgContrato);
     finally
            FRecMov.Free;
     end;
     Application.ProcessMessages;
end;

{------------------------------------------------------------------------------}
function MontaSQLInsert(const PTabela: string): string;
const
  StructSQL = 'SELECT RF.RDB$RELATION_NAME, ' +
              '       RF.RDB$FIELD_NAME AS NOME_CAMPO, ' +
              '       RF.RDB$FIELD_SOURCE AS NOME_DOMINIO, ' +
              '       FL.RDB$FIELD_TYPE, ' +
              '       FL.RDB$FIELD_PRECISION, ' +
              '       FL.RDB$FIELD_LENGTH AS TAMANHO ' +
              '  FROM RDB$RELATION_FIELDS RF, ' +
              '       RDB$FIELDS FL, ' +
              '       RDB$TYPES TP ' +
              'WHERE RF.RDB$RELATION_NAME = :TABELA ' +
              '  AND TP.RDB$TYPE          = FL.RDB$FIELD_TYPE ' +
              '  AND TP.RDB$FIELD_NAME    = ''RDB$FIELD_TYPE'' ' +
              '  AND RF.RDB$FIELD_SOURCE  = FL.RDB$FIELD_NAME ' +
              'ORDER BY RF.RDB$FIELD_POSITION ';
var
  qr: TIBOQuery;
  ls: TStrings;
  tx: string;
begin
  Result := '';
  try
    FH.CriaLista(ls);
    FG.CriaIBOQuery(qr, StructSQL);
    if not FG.FindKey(qr, 'TABELA', [PTabela]) then
      Exit;

    ls.Clear;
    ls.Add('insert into ' + PTabela + '(');
    qr.First;
    while not qr.Eof do
    begin
      if qr.RecNo = qr.RecordCount then
        tx := ')'
      else
        tx := ',';
      ls.Add(qr.FieldByName('NOME_CAMPO').AsString + tx);
      qr.Next;
    end;
    ls.Add('values (');
    qr.First;
    while not qr.Eof do
    begin
      if qr.RecNo = qr.RecordCount then
        tx := ')'
      else
        tx := ',';
      ls.Add(':' + qr.FieldByName('NOME_CAMPO').AsString + tx);
      qr.Next;
    end;
    Result := ls.Text;
  finally
    FH.DestroiMultiObj([ls, qr]);
  end;
end;

{------------------------------------------------------------------------------}
procedure ChamaGridPreview(arq: string);
begin
  FG.GravaCacheStr('FSisGridPreview_Arq', arq);
  FG.ChamaForm(Application.MainForm, '_TFSisGridPreview', 'Visualizador', 0);
end;

{------------------------------------------------------------------------------}
function RetCabHTMLQuantumGrid: string;
begin
  Result :=  '<table border="1" cellpadding="0" width="100%" id="table1" style=' +
             '"border-collapse: collapse; " bordercolor="#C0C0C0"> ' +
          #13'	<tr> '+
          #13'		<td style="border-left-color: #C0C0C0; border-left-width: 1px; ' +
             'border-bottom-style: solid; border-bottom-width: 1px; border-right-style:' +
             'none; border-right-width:medium" height="25"> ' +
          #13'		<font face="Tahoma" style="font-size: 8pt"> ' +
          #13'		<b>&nbsp; NOME_ESTABE</b></font></td> ' +
          #13'		<td style="border-bottom-style: solid; border-bottom-width: 1px; ' +
             'border-left-style:none; border-left-width:medium; border-right-style:none; ' +
             'border-right-width:medium" height="25"> ' +
          #13'		<p align="center"><font face="Tahoma" style="font-size: 8pt"> ' +
          #13'		<b>NOME_REL</b></font></td> ' +
          #13'		<td style="border-right-color: #C0C0C0; border-right-width: 1px; ' +
             'border-bottom-style: solid; border-bottom-width: 1px; border-left-style:none; ' +
             'border-left-width:medium" height="25"> ' +
          #13'		<p align="center"><font face="Tahoma" style="font-size: 8pt"> ' +
          #13'		<b>DATA_REL</b></font></td> ' +
          #13'	</tr> ' +
          #13'	</table> ' +
          #13'<font face="Tahoma"> ' +
          #13'<br>';
end;

{------------------------------------------------------------------------------}
procedure ExportaQuantumGrid(cxGR: TcxGrid; const PTipo: string;
                             const L1C1: string = ''; const L1C2: string = '';
                             const L2C1: string = ''; const L2C2: string = '';
                             const L3C1: string = ''; const L3C2: string = '';
                             const FormatoFloats: string = '';
                             const MostraCabecalho: Boolean = True);
var
  arqTmp   ,
  arqDest  ,
  vHtml    ,
  ultCel   ,
  vNome    ,
  vSubCab  : string;
  cc       ,
  ll       ,
  psBody   : Integer;
  vTmpApp  ,
  vExcelApp: TExcelApplication;
  vTmpWrk  ,
  vExcelWrk: TExcelWorksheet;
  ls       : TStrings;
  existe   : Boolean;
  r1       ,
  r2       : ExcelRange;

  function RetCab: string;
  begin
    Result := FG.RetCabHTMLQuantumGrid;
    Result := StringReplace(Result, 'NOME_ESTABE', FG.RetEstabeRel, [rfIgnoreCase]);
    Result := StringReplace(Result, 'NOME_REL'   , TForm(cxGR.Owner).Caption, [rfIgnoreCase]);
    Result := StringReplace(Result, 'DATA_REL'   , FG.DataStrLocal + #32 + FH.HoraExata, [rfIgnoreCase]);

    if (L1C1 + L1C2 + L2C1 + L2C2 + L3C1 + L3C2) > '' then
    begin
      vSubCab := #13'<table border="1" cellpadding="0" width="100%" id="table2" ' +
                    'style="border-collapse: collapse; " bordercolor="#C0C0C0"> ' +
                 #13'	<tr> ' +
                 #13'		<td style="border-left-color: #C0C0C0; border-left-width: ' +
                    '1px; border-bottom-style: solid; border-bottom-width: 1px; ' +
                    'border-right-style:none; border-right-width:medium; ' +
                    'border-top-style:solid; border-top-width:1px" width="50%"> ' +
                 #13'		<font face="Tahoma" style="font-size: 8pt"> ' +
                 #13'		<b>&nbsp; LINHA_COLUNA_1</b></font></td> ' +
                 #13'		<td style="border-bottom-style: solid; border-bottom-width: 1px; ' +
                    'border-left-style:none; border-left-width:medium; ' +
                    'border-right-style:none; border-right-width:medium; ' +
                    'border-top-style:solid; border-top-width:1px" height="25" width="50%"> ' +
                 #13'<font face="Tahoma"> ' +
                 #13'		<p align="center"><font face="Tahoma" style="font-size: 8pt"> ' +
                 #13'		<b>LINHA_COLUNA_2</b></font></td> ' +
                 #13'	</tr> ' +
                 #13' ' +
                 #13'	</table> ';
      if (L1C1 + L1C2) > '' then
      begin
        Result := Result +
                  FH.MultiStringReplace(vSubCab,
                                        ['LINHA_COLUNA_1', 'LINHA_COLUNA_2'],
                                        [L1C1, L1C2], [rfIgnoreCase]);
      end;
      if (L2C1 + L2C2) > '' then
      begin
        Result := Result +
                  FH.MultiStringReplace(vSubCab,
                                        ['LINHA_COLUNA_1', 'LINHA_COLUNA_2'],
                                        [L2C1, L2C2], [rfIgnoreCase]);
      end;
      if (L3C1 + L3C2) > '' then
      begin
        Result := Result +
                  FH.MultiStringReplace(vSubCab,
                                        ['LINHA_COLUNA_1', 'LINHA_COLUNA_2'],
                                        [L3C1, L3C2], [rfIgnoreCase]);
      end;
    end;

    if FH.RetroCopy(Result, 1, 4) <> '<br>' then
      Result := Result + #13'<br>';
  end;

begin
  try
    FH.SetaCursorAmp;
    FH.CriaLista(ls);

    if SameText(PTipo, 'HTML') then
    begin
      arqTmp := FG.SetaLocalRelatorio + cxGR.Owner.Name + '.html';
      ExportGrid4ToHTML(arqTmp, cxGR);
      if FileExists(arqTmp) then
      begin
        vHtml := FH.GetConteudoArq(arqTmp);
        psBody := FH.GetPos('<BODY>', vHtml);
        if psBody > 0 then
        begin
          System.Insert(RetCab, vHtml, psBody);
          FH.GeraArqTexto(arqTmp, vHtml);
        end;
      end;
    end
    else
    if FH.ComparaTextoFixo(PTipo, 'XLS') then
    begin
      vNome  := cxGR.Owner.Name;
      if Length(PTipo) >= 4 then
      begin
          vNome := Copy(PTipo, 5, 300);
      end;
      arqTmp  := FG.SetaLocalRelatorio + vNome + '.xls';
      arqDest := FG.SetaLocalRelatorio + vNome + '(001).xls';

      ExportGrid4ToExcel(arqTmp, cxGR, True, True);

      Application.ProcessMessages;

      if FileExists(arqTmp) then
      begin
        cc := 1;
        while (FileExists(arqDest)) and (not SysUtils.DeleteFile(arqDest) )  do
        begin
            Inc(cc);
            arqDest := FG.SetaLocalRelatorio + vNome + '(' + FH.StrZero(cc, 3) + ').xls';
        end;

        CopyFile(PChar(arqTmp), PChar(arqDest), False);
        DeleteFile(PChar(arqTmp));
        if not MostraCabecalho then
           FH.ExecutaArq(arqDest, Application.MainForm)
        else
        begin
            try
              vExcelApp := TExcelApplication.Create(nil);
              vExcelApp.AutoConnect := True;
              vExcelApp.ConnectKind := ckNewInstance;
              vExcelWrk := TExcelWorksheet.Create(nil);
              try
                vExcelApp.Disconnect;
                vExcelApp.Connect;
                vExcelApp.Workbooks.Open(arqDest, null, null, null, null, null, null, null, null, True, null, null, null, 0);
                vExcelWrk.ConnectTo(vExcelApp.Sheets[1] as _WorkSheet);

                vExcelWrk.Range['A1', 'A1'].EntireRow.Insert(null);
                vExcelWrk.Range['A1', 'A1'].EntireRow.Insert(null);
                vExcelWrk.Range['A1', 'A1'].EntireRow.Insert(null);
                vExcelWrk.Range['A1', 'A1'].EntireRow.Insert(null);

                ls.Clear;
                if (L1C1 + L1C2) > '' then ls.Add(L1C1 + #191 + L1C2);
                if (L2C1 + L2C2) > '' then ls.Add(L2C1 + #191 + L2C2);
                if (L3C1 + L3C2) > '' then ls.Add(L3C1 + #191 + L3C2);

                if ls.Count > 0 then vExcelWrk.Range['A1', 'A1'].EntireRow.Insert(null);
                if ls.Count > 1 then vExcelWrk.Range['A1', 'A1'].EntireRow.Insert(null);
                if ls.Count > 2 then vExcelWrk.Range['A1', 'A1'].EntireRow.Insert(null);

                vExcelWrk.Range['A1', 'A1'].Value := FG.RetEstabeRel;
                vExcelWrk.Range['A2', 'A2'].Value := TForm(cxGR.Owner).Caption;
                vExcelWrk.Range['A3', 'A3'].Value := FG.DataStrLocal + #32 + FH.HoraExata;

                if ls.Count > 0 then
                begin
                  vExcelWrk.Range['A5', 'A5'].Value := FH.CopyPos   (ls[0], #191);
                  vExcelWrk.Range['C5', 'C5'].Value := FH.CopyPosMax(ls[0], #191);
                end;
                if ls.Count > 1 then
                begin
                  vExcelWrk.Range['A6', 'A6'].Value := FH.CopyPos   (ls[1], #191);
                  vExcelWrk.Range['C6', 'C6'].Value := FH.CopyPosMax(ls[1], #191);
                end;
                if ls.Count > 2 then
                begin
                  vExcelWrk.Range['A7', 'A7'].Value := FH.CopyPos   (ls[2], #191);
                  vExcelWrk.Range['C7', 'C7'].Value := FH.CopyPosMax(ls[2], #191);
                end;

                ultCel := 'A3';

                if ls.Count > 0 then ultCel := 'C5';
                if ls.Count > 1 then ultCel := 'C6';
                if ls.Count > 2 then ultCel := 'C7';

                vExcelWrk.Range['A1', ultCel].EntireRow.Font.Name := 'Arial';
                vExcelWrk.Range['A1', ultCel].EntireRow.Font.Size := 8;
                vExcelWrk.Range['A1', ultCel].EntireRow.Font.Bold := true;

                vExcelWrk.Range['A1', 'A1'].Select;
                vExcelWrk.ConnectTo(vExcelApp.Sheets[vExcelApp.Sheets.Count] as _WorkSheet);
                vExcelWrk.Activate;
                vExcelApp.Visible[0] := true;
              except
                on E: Exception do
                begin
                  FH.PrMsg_ER('Erro na planilha.'#13#13 + E.Message);
                  vExcelApp.Quit;
                end;
              end;
            finally
              FreeAndNil(vExcelWrk);
              FreeAndNil(vExcelApp);
            end;
        end;
      end;
    end;
    if FileExists(arqTmp) then
      FH.ExecutaArq(arqTmp);
  finally
    FH.DestroiObj(ls);
    FH.SetaCursorDef;
  end;
end;

{------------------------------------------------------------------------------}
procedure GravaCliDefault(const ACli: string);
begin
  FH.GravaRegistryStr(FG.REG_CHAVE_GERAL, FG.CLI_DEFAULT, ACli);
end;

{------------------------------------------------------------------------------}
function LeCliDefault: string;
begin
  Result := FH.LeRegistryStr(FG.REG_CHAVE_GERAL, FG.CLI_DEFAULT);
end;

{------------------------------------------------------------------------------}
procedure GravaForDefault(const AFor: string);
begin
  FH.GravaRegistryStr(FG.REG_CHAVE_GERAL, FG.FOR_DEFAULT, AFor);
end;

{------------------------------------------------------------------------------}
function LeForDefault: string;
begin
  Result := FH.LeRegistryStr(FG.REG_CHAVE_GERAL, FG.FOR_DEFAULT);
end;

{------------------------------------------------------------------------------}
function ContratoTemPag(const ID: Int64): Boolean;
var
  qrPag : TIBOQuery;
begin
  if ID < 1 then
    Exit;
  try
    FG.CriaIBOQuery(qrPag, 'SELECT PGT.ID ' +
                           '  FROM RECCONTRATOPREPAG PGT ' +
                           '  JOIN RECCONTRATOPRE PRE ON (PGT.IDPRE = PRE.ID) ' +
                           'WHERE PGT.IDCON = ' + IntToStr(ID) +
                           '  AND PRE.PRESTACAO > 0');
    qrPag.Open;
    Result := not qrPag.IsEmpty;
  finally
    FH.DestroiMultiObj([qrPag]);
  end;
end;

{------------------------------------------------------------------------------}
function DeletaFluxoRecPag(const ID: Int64; TR: TIB_Transaction = nil): Boolean;
var
  qrLnk  : TIBOQuery;
  qrFlx  : TIBOQuery;
  qrBan  : TIBOQuery;
  qrPag  : TIBOQuery;
  vMsg   : string;
begin
  Result := False;
  try
    FG.CriaIBOQuery(qrLnk, 'SELECT * FROM BANLNK WHERE IDMOV = ' + IntToStr(ID));
    FG.CriaIBOQuery(qrFlx, '');
    FG.CriaIBOQuery(qrPag, '');
    FG.CriaIBOQuery(qrBan, '');
    qrLnk.IB_Transaction := TR;
    qrFlx.IB_Transaction := TR;
    qrPag.IB_Transaction := TR;
    qrBan.IB_Transaction := TR;

    qrLnk.Open;

    vMsg := '';
    qrPag.SQL.Text := 'SELECT PAG.ID, PRE.IDCON, ' +
                      '       PRE.IDCON || ''/'' || PRE.PRESTACAO AS CONTRATO_PRE ' +
                      '  FROM RECCONTRATOPREPAG PAG ' +
                      '  JOIN RECCONTRATOPRE    PRE ON (PAG.IDPRE = PRE.ID) ' +
                      ' WHERE PAG.ID = ' + IntToStr(ID);
    qrPag.Open;
    // pega o pagamento original
    if not qrPag.IsEmpty then
      vMsg := qrPag.FieldByName('CONTRATO_PRE').AsString;
    qrPag.Close;

    if not qrLnk.IsEmpty then
    begin
      qrBan.SQL.Text := 'SELECT DISTINCT IDMOV FROM BANLNK ' +
                        ' WHERE IDBAN IN ' + FH.GeraSQLIN(qrLnk, 'IDBAN') +
                        '   AND IDMOV <> ' + IntToStr(ID);
      qrBan.Open;

      if not qrBan.IsEmpty then
      begin
        qrPag.SQL.Text := 'SELECT PAG.ID, PRE.IDCON, ' +
                          '       PRE.IDCON || ''/'' || PRE.PRESTACAO AS CONTRATO_PRE ' +
                          '  FROM RECCONTRATOPREPAG PAG ' +
                          '  JOIN RECCONTRATOPRE    PRE ON (PAG.IDPRE = PRE.ID) ' +
                          ' WHERE PAG.ID IN ' + FH.GeraSQLIN(qrBan, 'IDMOV');
        qrPag.Open;
        if not qrPag.IsEmpty then
        begin
          qrPag.First;
          while not qrPag.Eof do
          begin
            vMsg := vMsg + #13 + qrPag.FieldByName('CONTRATO_PRE').AsString;
            qrPag.Next;
          end;
        end;
      end;
    end;

    if vMsg > '' then
      if FH.PrMsg_CF('Serão excluídos os seguintes pagamentos:'#13#13 +
                     vMsg + #13#13'Deseja prosseguir?', 2) = 0 then
      Exit;

    qrLnk.First;
    while not qrLnk.Eof do
    begin
      qrFlx.Close;
      qrFlx.SQL.Text := 'DELETE FROM BANMOVIMENTO WHERE ID = ' +
                        qrLnk.FieldByName('IDBAN').AsString;
      qrFlx.ExecSQL;
      qrLnk.Next;
    end;

    if qrPag.Active then
      if not qrPag.IsEmpty then
      begin
        qrPag.First;
        while not qrPag.Eof do
        begin
          qrFlx.Close;
          qrFlx.SQL.Text := 'DELETE FROM RECCONTRATOPREPAG WHERE ID = ' +
                            qrPag.FieldByName('ID').AsString;
          qrFlx.ExecSQL;
          qrPag.Next;
        end;
      end;

    Result := True;
  finally
    FH.DestroiMultiObj([qrLnk, qrFlx]);
  end;
end;

{------------------------------------------------------------------------------}
procedure DeletaFluxoRec(const ID: Int64; TR: TIB_Transaction = nil);
var
  qrPag : TIBOQuery;
begin
  try
    FG.CriaIBOQuery(qrPag, 'SELECT ID FROM RECCONTRATOPREPAG WHERE IDCON = ' + IntToStr(ID));
    qrPag.IB_Transaction  := TR;
    qrPag.Open;
    qrPag.First;
    while not qrPag.Eof do
    begin
      FG.DeletaFluxoRecPag(qrPag.FieldByName('ID').AsInteger, TR);
      qrPag.Next;
    end;
  finally
    FH.DestroiMultiObj([qrPag]);
  end;
end;

{------------------------------------------------------------------------------}
procedure DeletaFluxoVen(const ID: Int64; TR: TIB_Transaction = nil);
var
  qrFlx : TIBOQuery;
  qrVen : TIBOQuery;
  qrLnk : TIBOQuery;
begin
  try
    FG.CriaIBOQuery(qrLnk, 'SELECT * FROM BANLNKVEN WHERE IDVEN = ' + IntToStr(ID));
    FG.CriaIBOQuery(qrFlx, 'DELETE FROM BANMOVIMENTO WHERE ID = :ID');
    FG.CriaIBOQuery(qrVen, 'SELECT IDCON FROM VENNOTA WHERE ID = ' + IntToStr(ID));

    qrFlx.IB_Transaction  := TR;
    qrVen.IB_Transaction  := TR;
    qrLnk.IB_Transaction  := TR;

    qrVen.Open;
    qrLnk.Open;

    if qrVen.FieldByName('IDCON').AsString <> '' then
      FG.DeletaFluxoRec(qrVen.FieldByName('IDCON').AsInteger, TR);

    qrLnk.First;
    while not qrLnk.Eof do
    begin
      qrFlx.Close;
      qrFlx.ParamByName('ID').AsString := qrLnk.FieldByName('IDBAN').AsString;
      qrFlx.ExecSQL;
      qrLnk.Next;
    end;
  finally
    FH.DestroiMultiObj([qrFlx, qrVen, qrLnk]);
  end;
end;

{------------------------------------------------------------------------------}
procedure CancelaFluxoRecPag(const ID: Int64; const ASit: string; TR: TIB_Transaction = nil);
var
  qrLnk : TIBOQuery;
  qrFlx : TIBOQuery;
  tmp   : string;
begin
  if ASit = 'OK' then
    tmp := 'OK'
  else
    tmp := 'CA';
  try
    FG.CriaIBOQuery(qrLnk, 'SELECT * FROM BANLNK WHERE IDMOV = ' + IntToStr(ID));
    FG.CriaIBOQuery(qrFlx, '');
    qrLnk.IB_Transaction := TR;
    qrFlx.IB_Transaction  := TR;
    qrLnk.Open;
    qrLnk.First;
    while not qrLnk.Eof do
    begin
      qrFlx.Close;
      qrFlx.SQL.Text := 'UPDATE BANMOVIMENTO ' +
                        ' SET SITUACAO = ' + QuotedStr(tmp) +
                        ' WHERE ID = ' + qrLnk.FieldByName('IDBAN').AsString;
      qrFlx.ExecSQL;
      qrLnk.Next;
    end;
  finally
    FH.DestroiMultiObj([qrLnk, qrFlx]);
  end;
end;

{------------------------------------------------------------------------------}
procedure CancelaFluxoRec(const ID: Int64; const ASit: string; TR: TIB_Transaction = nil);
var
  qrPag : TIBOQuery;
begin
  try
    FG.CriaIBOQuery(qrPag, 'SELECT ID FROM RECCONTRATOPREPAG WHERE IDCON = ' + IntToStr(ID));
    qrPag.IB_Transaction  := TR;
    qrPag.Open;
    qrPag.First;
    while not qrPag.Eof do
    begin
      FG.CancelaFluxoRecPag(qrPag.FieldByName('ID').AsInteger, ASit, TR);
      qrPag.Next;
    end;
  finally
    FH.DestroiMultiObj([qrPag]);
  end;
end;

{------------------------------------------------------------------------------}
procedure CancelaFluxoVen(const ID: Int64; const ASit: string; TR: TIB_Transaction = nil);
var
  qrVen : TIBOQuery;
  qrFlx : TIBOQuery;
  qrLnk : TIBOQuery;
  tmp   : string;
begin
  try
    if ASit = 'OK' then
      tmp := 'OK'
    else
      tmp := 'CA';

    FG.CriaIBOQuery(qrLnk, 'SELECT * FROM BANLNKVEN WHERE IDVEN = ' + IntToStr(ID));
    FG.CriaIBOQuery(qrFlx, 'UPDATE BANMOVIMENTO SET SITUACAO = '    + QuotedStr(tmp) + ' WHERE ID = :ID');
    FG.CriaIBOQuery(qrVen, 'SELECT IDCON FROM VENNOTA WHERE ID = '  + IntToStr(ID));
    
    qrFlx.IB_Transaction  := TR;
    qrVen.IB_Transaction  := TR;
    qrLnk.IB_Transaction  := TR;

    qrLnk.Open;
    qrVen.Open;

    if qrVen.FieldByName('IDCON').AsString <> '' then
      FG.CancelaFluxoRec(qrVen.FieldByName('IDCON').AsInteger, tmp, TR);

    qrLnk.First;
    while not qrLnk.Eof do
    begin
      qrFlx.Close;
      qrFlx.ParamByName('ID').AsString := qrLnk.FieldByName('IDBAN').AsString;
      qrFlx.ExecSQL;
      qrLnk.Next;
    end;
  finally
    FH.DestroiMultiObj([qrFlx, qrVen, qrLnk]);
  end;
end;

{------------------------------------------------------------------------------}
procedure ChamaDica(FR: TForm; const PTitulo, PTexto: string);
begin
  if FSisDica = nil then
    FSisDica := TFSisDica.Create(Application);

  FSisDica.Constraints.MinHeight := 0;
  FSisDica.lbTexto.Anchors       := FSisDica.lbTexto.Anchors - [akRight];
  FSisDica.lbTexto.AutoSize      := True;
  FSisDica.lbTexto.Properties.WordWrap := False;
  FSisDica.lbTexto.Caption       := '';
  FSisDica.lbTexto.Caption       := PTexto;
  FSisDica.Height                := 196 + FSisDica.lbTexto.Height;
  FSisDica.Width                 := FSisDica.lbTexto.Width + 37;
  FSisDica.Constraints.MinHeight := FSisDica.Height;
  FSisDica.lbTitulo.Caption      := PTitulo;

  if FR = Application.MainForm then
  begin
    FSisDica.Left := (FMenu.Width + FMenu.Left) - FSisDica.Width  - 2;
    if FSisDica.Left + FSisDica.Width > Screen.Width then
      FSisDica.Left := Screen.Width - FSisDica.Width;
    FSisDica.Top  := (FMenu.Height + FMenu.Top)
                     - FSisDica.Height - FMenu.pnStatus.Height - 2;
  end
  else
  begin
    if FR.Left >= 0 then
      FSisDica.Left := FR.Left;
    if  (FR.Top + FR.Height <= Screen.Height)
    and (FR.Top >= 0) then
      FSisDica.Top := (FR.Top + FR.Height) - FSisDica.Height;
  end;

  FSisDica.lbTexto.Anchors := FSisDica.lbTexto.Anchors + [akRight];
  FSisDica.lbTexto.Properties.WordWrap := True;
  FSisDica.lbTexto.AutoSize            := False;

  FSisDica.Show;
end;

{------------------------------------------------------------------------------}
procedure ChamaSisAnota(TFR: string);
var
  qrAux: TIBOQuery;
begin
  try
    FG.CriaIBOQuery(qrAux, 'SELECT * FROM SISANOTA WHERE FORM = ' + QuotedStr(TFR));
    qrAux.Open;
    if qrAux.IsEmpty then
      Exit;
    FG.ChamaDica(Application.MainForm,
                 qrAux.FieldByName('TITULO').AsString,
                 qrAux.FieldByName('TEXTO').AsString);
  finally
    FH.DestroiObj(qrAux);
  end;
end;

{------------------------------------------------------------------------------}
procedure AtuInfoUsuCaixa;
var
  auxQr : TIBOQuery;
  vSelect, vFrom : string;
begin
  FG.GravaCacheStr('$Sis_Caixa'               , '');
  FG.GravaCacheStr('$Sis_Nome_Caixa'          , '');
  FG.GravaCacheStr('$Sis_Ecf_Caixa'           , '');
  FG.GravaCacheStr('$Sis_Amx_Caixa'           , '');
  FG.GravaCacheInt('$Sis_Porta_Caixa'         , -1);
  FG.GravaCacheStr('$Sis_Pdv_Caixa'           , '');
  FG.GravaCacheStr('$Sis_Gaveta_Caixa'        , '');
  FG.GravaCacheStr('$Sis_Libera_Cartao_Caixa' , '');

  try
    FG.CriaIBOQuery(auxQr);
    if FH.IntOk(FG.RetCaixaLocal) then
    begin
      vSelect := 'SELECT CODIGO, DESCRICAO, PORTA, TIMPRESSORA, CODAMX, PDV, GAVETA ';
      vFrom   := 'FROM CAXCADASTRO ' +
                 'WHERE CODIGO = ' + FG.RetCaixaLocal;
      try
         auxQr.SQL.Text := vSelect + ', LIBERACARTAO ' + vFrom;
         auxQr.Open;
      except
         auxQr.Close;
         auxQr.SQL.Text := vSelect + vFrom;
         auxQr.Open;
      end;
      if auxQr.IsEmpty then
      begin
        FH.PrMsg_ER('O caixa informado no arquivo de configuração CAIXA.INI não existe.'#13'Reporte-se ao administrador do sistema para maiores detalhes.');
        Exit;
      end;
    end
    else
    begin
      vSelect := 'SELECT CAX.CODIGO, CAX.DESCRICAO, CAX.PORTA, ' +
                 '       CAX.TIMPRESSORA, CAX.CODAMX, CAX.PDV, CAX.GAVETA ';
      vFrom   := 'FROM USUCADASTRO USU ' +
                 '     JOIN CAXCADASTRO CAX ON USU.CAIXA = CAX.CODIGO ' +
                 'WHERE USU.USUARIO = ' + QuotedStr(FG.RetUsuAtivo);
      try
         auxQr.SQL.Text := vSelect + ', CAX.LIBERACARTAO ' + vFrom;
         auxQr.Open;
      except
         auxQr.Close;
         auxQr.SQL.Text := vSelect + vFrom;
         auxQr.Open;
      end;
      if auxQr.FieldByName('CODIGO').AsString = '' then
         if FH.IntOk(FG.LeConfigEstabeStr(FG.CfgEstCaixaGeral)) then
         begin
              auxQr.Close;
              vSelect := 'SELECT CODIGO, DESCRICAO, PORTA, TIMPRESSORA, CODAMX, PDV, GAVETA ';
              vFrom   := 'FROM CAXCADASTRO ' +
                         'WHERE CODIGO = ' + FG.LeConfigEstabeStr(FG.CfgEstCaixaGeral);
              try
                 auxQr.SQL.Text := vSelect + ', LIBERACARTAO ' + vFrom;
                 auxQr.Open;
              except
                 auxQr.Close;
                 auxQr.SQL.Text := vSelect + vFrom;
                 auxQr.Open;
              end;
         end;
    end;
    FG.GravaCacheStr('$Sis_Caixa'               , auxQr.FieldByName('CODIGO').AsString);
    FG.GravaCacheStr('$Sis_Nome_Caixa'          , auxQr.FieldByName('DESCRICAO').AsString);
    FG.GravaCacheStr('$Sis_Amx_Caixa'           , auxQr.FieldByName('CODAMX').AsString);
    FG.GravaCacheInt('$Sis_Porta_Caixa'         , StrToIntDef(FH.StrInteiro(auxQr.FieldByName('PORTA').AsString), 0));
    FG.GravaCacheStr('$Sis_Pdv_Caixa'           , auxQr.FieldByName('PDV').AsString);
    FG.GravaCacheStr('$Sis_Gaveta_Caixa'        , auxQr.FieldByName('GAVETA').AsString);
    try
      FG.GravaCacheStr('$Sis_Libera_Cartao_Caixa' , auxQr.FieldByName('LIBERACARTAO').AsString);
    except
      FG.GravaCacheStr('$Sis_Libera_Cartao_Caixa' , 'N');
    end;

    if FileExists(FH.RetDirSis + 'CONFIGSIS.TXT') then
       FG.GravaCacheInt('$Sis_Ecf_Caixa'  , EcfGer.ID_Virtual)
    else
       FG.GravaCacheStr('$Sis_Ecf_Caixa'  , auxQr.FieldByName('TIMPRESSORA').AsString);
  finally
    FH.DestroiObj(auxQr);
  end;
end;

{------------------------------------------------------------------------------}
function ProcuraValorDS(DS: TDataSet; const PCampo: string; const PValor: Variant): Boolean;
begin
   try
     Result := False;
     DS.DisableControls;
     DS.First;
     while not DS.Eof do
     begin
       if DS.FieldByName(PCampo).Value = PValor then
       begin
         Result := True;
         Exit;
       end;
       DS.Next;
     end;
   finally
     DS.EnableControls;
     DS.First;
   end;
end;

{------------------------------------------------------------------------------}
procedure RefreshFR;
var
  I: integer;
begin
    for I := 0 to Screen.FormCount - 1 do
        if FG.ClasseFormPermitida(Screen.Forms[I].ClassName) then
           Screen.Forms[I].Repaint;
end;

{------------------------------------------------------------------------------}
function RetSenhaMontada: string;
begin
  Result := FG.SENHAS_DEFAULT[1] + IntToStr(DayOf(FG.DataLocal));
end;

{------------------------------------------------------------------------------}
function RetValLookupComboBox(LCB: TcxLookupComboBox; const PCampo: string): Variant;
begin
  if (LCB.ItemIndex < 0) or (LCB.Text = '') then
    Result := null
  else
    Result := LCB.Properties.DataController.Values[LCB.ItemIndex,
                                                   LCB.Properties.Grid.Columns.ColumnByFieldName(PCampo).Index];
end;

{------------------------------------------------------------------------------}
function RetStrLookupComboBox(LCB: TcxLookupComboBox; const PCampo: string = 'CODIGO'): string;
begin
  Result := VarToStr(FG.RetValLookupComboBox(LCB, PCampo));
end;

{------------------------------------------------------------------------------}
function RetDescLookupComboBox(LCB: TcxLookupComboBox; const PCampo: string = 'DESCRICAO'): string;
begin
  if (LCB.ItemIndex < 0) or (LCB.Text = '') then
    Result := ''
  else
  begin
    Result := VarToStr(LCB.Properties.DataController.GetValue(LCB.ItemIndex,
                                                              LCB.Properties.Grid.Columns.ColumnByFieldName(PCampo).Index));
  end;
end;

{------------------------------------------------------------------------------}
function ChecaLookupComboBox(LCB: TcxLookupComboBox; Lbl: TcxLabel): Boolean;
begin
  Result := True;

  if LCB.Text > '' then
    if not LCB.Properties.ListSource.DataSet.Locate(LCB.Properties.KeyFieldNames,
                                                    LCB.Text, [loCaseInsensitive]) then
    begin
      Lbl.Caption := 'INVÁLIDO!';
      Result      := False;
      FH.PrMsg_Focalize(LCB, '', True);
      Exit;
    end;
end;

{------------------------------------------------------------------------------}
procedure SetaLookupComboBox(LCB: TcxLookupComboBox; const PValor: string; PIndexDefault: Integer);
begin
    if PValor = '' then
       LCB.ItemIndex := PIndexDefault
    else
       LCB.ItemIndex := LCB.Properties.DataController.FindRecordIndexByKey(PValor);
end;

{------------------------------------------------------------------------------}
function RetUltPrc(const ACodCliente, ACodProduto: string): Extended;
begin
  Result := 0;
  if FGCli.VerClienteGeral(ACodCliente) then
    Exit;
  try
    if FG.FindKey(DMI.qrSpVenUltPrc, 'CODCLIENTE,CODPRODUTO', [ACodCliente, ACodProduto]) then
      Result := DMI.qrSpVenUltPrc.FieldByName('PRECO').AsFloat;
  finally
    DMI.qrSpVenUltPrc.Close;
  end;
end;

{------------------------------------------------------------------------------}
function VerUsuEstabe(const PEstabe: string): Boolean;
var
  qr: TIBOQuery;
begin
  Result := True;
  try
    FG.CriaIBOQuery(qr, 'SELECT 1 FROM USUESTABE ' +
                        ' WHERE USUARIO = ' + QuotedStr(FG.RetUsuAtivo) +
                        '   AND ESTABE  = ' + QuotedStr(PEstabe));
    qr.Open;
    if qr.IsEmpty then
    begin
      Result := False;
      FH.PrMsg_ER('Usuário não está vinculado ao estabelecimento %s %s.',
                  [PEstabe, FG.Lookup('TABESTABE', 'NOME', 'CODIGO', PEstabe)]);
      Exit;
    end;
  finally
    FH.DestroiMultiObj([qr]);
  end;
end;

{------------------------------------------------------------------------------}
function ChamaMsgCf(const AMsg: string; const ADefBtn: Smallint = 1): Integer;
begin
  Result := 0;
  try
    Application.CreateForm(TFMsgCf, FMsgCf);
    FMsgCf.lbMsg.Caption := AMsg;
    FMsgCf.btSim.Default := False;
    FMsgCf.btNao.Default := False;
    case ADefBtn of
      1:
        begin
          FMsgCf.btSim.TabOrder := 0;
          FMsgCf.btSim.Default  := True;
          FMsgCf.btNao.Default  := False;
        end;
      2:
        begin
          FMsgCf.btSim.TabOrder := 1;
          FMsgCf.btSim.Default  := False;
          FMsgCf.btNao.Default  := True;
        end;
    end;
    FMsgCf.Caption := 'Confirmação';
    if FMsgCf.ShowModal = mrYes then
      Result := 1;
  finally
    FreeAndNil(FMsgCf);
  end;
end;

{------------------------------------------------------------------------------}
procedure ChamaMsg(const aMsg: string; const aTipo:Integer = 0);
var  m:string;
begin
     try
        Application.CreateForm(TFMsg, FMsg);
        m := 'Mensagem';
        case aTipo of
          1:begin
                 m := 'Aviso';
                 FMsg.lbMsg.Properties.Alignment.Horz := taCenter;
                 FMsg.lbMsg.Style.Color               := $00F7DFCA;
                 FMsg.lbMsg.Style.Font.Size           := 12;
                 FMsg.lbMsg.Style.Font.Style          := [fsBold];
                 FMsg.lbMsg.Style.TextColor           := clBlack;
            end;
          2:begin
                 m := 'Erro';
                 FMsg.lbMsg.Properties.Alignment.Horz := taCenter;
                 FMsg.lbMsg.Style.Color               := $00BF6000;
                 FMsg.lbMsg.Style.Font.Size           := 8;
                 FMsg.lbMsg.Style.Font.Style          := [fsBold];
                 FMsg.lbMsg.Style.TextColor           := clWhite;
            end;
        end;

        FMsg.lbMsg.Caption := aMsg;
        FMsg.Caption       := m;
        FMsg.ShowModal;
     finally
            FreeAndNil(FMsg);
     end;
end;

{------------------------------------------------------------------------------}
procedure ChamaMsgECF(const aMsg: string);
begin
     FG.GravaTabLog(nil,aMsg,'ECF');
     FG.ChamaMsg(aMsg, 2);
end;

{------------------------------------------------------------------------------}
procedure ChamaMsgDt(const AMsg, ADetalhes: string);
begin
     try
        Application.CreateForm(TFMsgDt, FMsgDt);
        FMsgDt.lbMsg.Caption    := AMsg;
        FMsgDt.mmDet.Lines.Text := ADetalhes;
        FMsgDt.Caption          := 'Mensagem';
        FMsgDt.ShowModal;
     finally
            FreeAndNil(FMsgDt);
     end;
end;

{------------------------------------------------------------------------------}
procedure ImprimeFichaFin(FR: TForm; const PDtBase: TDate; const PClientes: string;
  const AAtrIni, AAtrFin: Integer);
begin
  if FRecCobrancaFicha = nil then
     FRecCobrancaFicha := TFRecCobrancaFicha.Create(FR);
  FRecCobrancaFicha.Caption := 'Ficha financeira';
  FRecCobrancaFicha.ImprimeFicha(True, PDtBase, PClientes, AAtrIni, AAtrFin, True);
end;

{------------------------------------------------------------------------------}
procedure ImprimeFichaPag(FR: TForm; const APerIni, APerFin: TDate; const PClientes: string);
begin
  if FRecPagFicha = nil then
     FRecPagFicha := TFRecPagFicha.Create(FR);
  FRecPagFicha.Caption := 'Ficha financeira';
  FRecPagFicha.ImprimeFicha(True, APerIni, APerFin, PClientes);
end;

{------------------------------------------------------------------------------}
procedure ImprimeCartaCob(FR: TForm; const PDtBase: TDate; const PClientes: string;
  const AAtrIni, AAtrFin: Integer);
begin
  if FRecCobrancaCarta = nil then
     FRecCobrancaCarta := TFRecCobrancaCarta.Create(FR);
  FRecCobrancaCarta.ImprimeFichaCobranca(True, PDtBase, PClientes, AAtrIni, AAtrFin);
end;

{------------------------------------------------------------------------------}
procedure SetaFrxExports(FR: TForm; const ANomeRel: string = '');
var
   ii  : Integer;
   lst : TStrings;
   PI  : PPropInfo;
   ss  : string;
   vRgt: TRegistry;
begin
    if ANomeRel > '' then
       ss := FH.SetaNomeArqValido(ANomeRel)
    else
       ss := FH.SetaNomeArqValido((FR.Owner as TForm).Caption);

    for ii := 0 to FR.ComponentCount-1 do
    begin
        if  AnsiContainsText(FR.Components[ii].ClassName, 'TFRX')
        and AnsiContainsText(FR.Components[ii].ClassName, 'EXPORT') then
        begin
            PI := typInfo.GetPropInfo(FR.Components[ii].ClassInfo, 'FileName');
            if Assigned(PI) then
               typInfo.SetStrProp(FR.Components[ii], PI, ss);
        end;
    end;

    try
        FH.CriaLista(lst);
        FG.FindKey(DMG.qrUsuEmails, 'USUARIO', [AnsiUpperCase(FG.RetUsuAtivo)]);

        vRgt         := TRegistry.Create;
        vRgt.RootKey := HKEY_CURRENT_USER;
        vRgt.OpenKey('\Software\Fast Reports\EmailExport.RecentAddresses', True);
        vRgt.GetValueNames(lst);
        for ii := 0 to lst.Count -1 do
            vRgt.DeleteValue(lst[ii]);

        if DMG.qrUsuEmails.FieldByName('EMAILS').AsString > '' then
        begin
            lst.Text := DMG.qrUsuEmails.FieldByName('EMAILS').AsString;
            for ii := 0 to lst.Count -1 do
                if (lst[ii] > '') and AnsiContainsText(lst[ii], '@') then
                begin
                    vRgt         := TRegistry.Create;
                    vRgt.RootKey := HKEY_CURRENT_USER;
                    vRgt.OpenKey('\Software\Fast Reports\EmailExport.RecentAddresses', True);
                    vRgt.WriteString(trim(lst[ii]), '');
                end;
        end;
    finally
        DMG.qrUsuEmails.Close;
        FH.DestroiObj(lst);
        if vRgt <> nil then FH.DestroiObj(vRgt);
    end;
end;

{------------------------------------------------------------------------------}
function TCPServerAtivo : Boolean;
begin
    result := FMenu.IdTCPServer1.Active;
end;

{------------------------------------------------------------------------------}
procedure SetaCamposFrx(DS: TDataSet; FxDS: TfrxDBDataset);
var
  ii: Integer;
begin
  FxDS.FieldAliases.Clear;
  for ii := 0 to DS.FieldDefs.Count-1 do
    FxDS.FieldAliases.Add(DS.FieldDefs[ii].Name + '=' + DS.FieldDefs[ii].Name);
end;

{------------------------------------------------------------------------------}
procedure AtuDataEstat(const Tipo: string; TR: TIB_Transaction = nil);
var
  QR: TIBOQuery;
begin
  try
    QR := TIBOQuery.Create(nil);
    QR.IB_Connection  := DMG.IB_Connection;
    QR.IB_Transaction := TR;
    QR.SQL.Text       := 'SELECT * FROM TABDATAESTAT WHERE TIPO = ' + QuotedStr(Tipo);
    QR.Open;
    if QR.IsEmpty then
    begin
      QR.Close;
      QR.SQL.Text := 'INSERT INTO TABDATAESTAT (TIPO,DATAGERA) VALUES (' + QuotedStr(Tipo) + ', CURRENT_TIMESTAMP)';
      QR.ExecSQL;
    end
    else
    begin
      QR.Close;
      QR.SQL.Text := 'UPDATE TABDATAESTAT SET DATAGERA = CURRENT_TIMESTAMP WHERE TIPO = ' + QuotedStr(Tipo);
      QR.ExecSQL;
    end;
  finally
    QR.Close;
    FH.DestroiObj(QR);
  end;
end;

{------------------------------------------------------------------------------}
function LeDataEstat(const Tipo: string): string;
var
  QR: TIBOQuery;
begin
  Result := '';
  try
    QR := TIBOQuery.Create(nil);
    QR.IB_Connection  := DMG.IB_Connection;
    QR.SQL.Text       := 'SELECT DATAGERA FROM TABDATAESTAT WHERE TIPO = ' + QuotedStr(Tipo);
    QR.Open;
    Result            := QR.FieldByName('DATAGERA').AsString;
  finally
    QR.Close;
    FH.DestroiObj(QR);
  end;
end;

{------------------------------------------------------------------------------}
procedure ExecSQL(const SQL: string; TR: TIB_Transaction = nil);
var
  tmpCursor: TIB_Cursor;
begin
  try
    tmpCursor := TIB_Cursor.Create(nil);
    tmpCursor.IB_Connection  := DMG.IB_Connection;
    tmpCursor.IB_Transaction := TR;
    tmpCursor.SQL.Text       := SQL;
    tmpCursor.ExecSQL;
  finally
    tmpCursor.Close;
    FH.DestroiObj(tmpCursor);
  end;
end;

{------------------------------------------------------------------------------}
function RetMemoView(RPT: TfrxReport; const Obj: string): TfrxMemoView;
begin
  Result := (RPT.FindObject(Obj) as TfrxMemoView);
end;

{------------------------------------------------------------------------------}
function RetLineView(RPT: TfrxReport; const Obj: string): TfrxLineView;
begin
  Result := (RPT.FindObject(Obj) as TfrxLineView);
end;

{------------------------------------------------------------------------------}
function RetGroupHeader(RPT: TfrxReport; const Obj: string): TfrxGroupHeader;
begin
  Result := (RPT.FindObject(Obj) as TfrxGroupHeader);
end;

{------------------------------------------------------------------------------}
function RetGroupFooter(RPT: TfrxReport; const Obj: string): TfrxGroupFooter;
begin
  Result := (RPT.FindObject(Obj) as TfrxGroupFooter);
end;

{------------------------------------------------------------------------------}
function ProximoVago(const Tabela: string; var Resultado: Int64; const Campo: string = 'CODIGO'): Boolean;
var
  tmpCursor : TIB_Cursor;
  ii        : Int64;
  cc        : Int64;
  start     : Int64;
  ss        : string;
begin
  ss        := '1';
  Result    := False;
  Resultado := -1;
  if not FG.ChamaInputBox(FG.IBoxInt, 'Próximo vago', 'Informe o código inicial:', ss) then
    Exit;
  if FH.IntInvalido(ss) then
  begin
    FH.PrMsg_ER('Valor inválido.');
    Exit;
  end;
  start := StrToInt64(ss);
  try
    tmpCursor := TIB_Cursor.Create(nil);
    tmpCursor.IB_Connection  := DMG.IB_Connection;
    tmpCursor.SQL.Text       := Format('SELECT %s FROM %s WHERE %s = :%s', [Campo, Tabela, Campo, Campo]);
    ii := start;
    cc := 0;
    while ii <= High(Int64) do
    begin
      tmpCursor.Close;
      tmpCursor.ParamByName(Campo).AsInt64 := ii;
      tmpCursor.Open;
      if tmpCursor.RecordCount = 0 then
      begin
        Resultado := ii;
        Result    := True;
        Exit;
      end;
      Inc(ii);
      Inc(cc);
      if cc = 50 then
        ProgGer.ChamaStat(Application.MainForm, 'Procurando um código vago...');
    end;
  finally
    FH.SetaCursorDef;
    ProgGer.FechaStat;
    tmpCursor.Close;
    FH.DestroiObj(tmpCursor);
  end;
end;

{------------------------------------------------------------------------------}
procedure TravaMouseTeclado;
begin
  FH.BlockInput(True);
end;

{------------------------------------------------------------------------------}
procedure DestravaMouseTeclado;
begin
  FH.BlockInput(False);
end;

{------------------------------------------------------------------------------}
function RetFundoPanel: TImage;
begin
  Result := FMenu.imFundoPanel;
end;

{------------------------------------------------------------------------------}
function RetImGeral: TCxImage;
begin
  Result := FMenu.imGeral;
end;

{------------------------------------------------------------------------------}
procedure AtuPanelCaixaMenu(const PDataHora: string);
begin
    if FG.RetUsuCaixa > '' then
    begin
      if FG.RetUsuEcfCaixa <> 99 then
      begin
        if PDataHora = '' then
            FMenu.lbStatusCax.Caption := 'Caixa: '  + FG.RetUsuCaixa + ' - ' + FG.RetUsuNomeEcfCaixa
        else
            FMenu.lbStatusCax.Caption := 'Caixa: '  + FG.RetUsuCaixa + ' - ' + FG.RetUsuNomeEcfCaixa + #13 +
                                         'Abe.: '   + PDataHora;
      end
      else
      if FG.RetUsuEcfCaixa = 99 then
      begin
        if PDataHora = '' then
            FMenu.lbStatusCax.Caption := 'Caixa: '  + FG.RetUsuCaixa
        else
            FMenu.lbStatusCax.Caption := 'Caixa: '  + FG.RetUsuCaixa + #13 +
                                         'Abe.: '   + PDataHora;
      end;
    end
    else
        FMenu.lbStatusCax.Caption := '';
end;

{------------------------------------------------------------------------------}
function RetMascaraPreco: string;
begin
  Result := FH.RetMaskNum(FG.LeConfigEstabeInt(FG.CfgEstValDecimal, 4), ',0.');
end;

{------------------------------------------------------------------------------}
function RetDecsPreco: Integer;
begin
  Result := FG.LeConfigEstabeInt(FG.CfgEstValDecimal, 4);
end;

{------------------------------------------------------------------------------}
procedure GravaUsuRegistroStr(const PChave, PNome, PValor: string);
begin
  DMG.qrUsuRegistro.Close;
  DMG.qrUsuRegistro.ParamByName('USUARIO').AsString := FG.RetUsuAtivo;
  DMG.qrUsuRegistro.ParamByName('CHAVE').AsString   := PChave;
  DMG.qrUsuRegistro.ParamByName('NOME').AsString    := PNome;
  DMG.qrUsuRegistro.Open;
  if DMG.qrUsuRegistro.IsEmpty then
  begin
    DMG.qrUsuRegistro.Insert;
    DMG.qrUsuRegistro.FieldByName('USUARIO').AsString := FG.RetUsuAtivo;
    DMG.qrUsuRegistro.FieldByName('CHAVE').AsString   := PChave;
    DMG.qrUsuRegistro.FieldByName('NOME').AsString    := PNome;
  end
  else
    DMG.qrUsuRegistro.Edit;
  DMG.qrUsuRegistro.FieldByName('VALOR').AsString     := PValor;
  DMG.qrUsuRegistro.Post;
end;

{------------------------------------------------------------------------------}
function LeUsuRegistroStr(const PChave, PNome: string; const PDefault: string): string;
begin
  DMG.qrUsuRegistro.Close;
  DMG.qrUsuRegistro.ParamByName('USUARIO').AsString := FG.RetUsuAtivo;
  DMG.qrUsuRegistro.ParamByName('CHAVE').AsString   := PChave;
  DMG.qrUsuRegistro.ParamByName('NOME').AsString    := PNome;
  DMG.qrUsuRegistro.Open;
  if DMG.qrUsuRegistro.IsEmpty then
    Result := PDefault
  else
    Result := DMG.qrUsuRegistro.FieldByName('VALOR').AsString;
end;

{------------------------------------------------------------------------------}
procedure DeletaUsuRegistro(const PChave, PNome: string);
begin
     DMG.qrUsuRegistro.Close;
     DMG.qrUsuRegistro.ParamByName('USUARIO').AsString := FG.RetUsuAtivo;
     DMG.qrUsuRegistro.ParamByName('CHAVE').AsString   := PChave;
     DMG.qrUsuRegistro.ParamByName('NOME').AsString    := PNome;
     DMG.qrUsuRegistro.Open;
     if not DMG.qrUsuRegistro.IsEmpty then
        DMG.qrUsuRegistro.Delete;
end;

{------------------------------------------------------------------------------}
procedure LeUsuRegistroCds(const PChave, PNome: string; CDS: TClientDataSet);
var
    memAux : TMemoryStream;
    bloAux : TStream;
begin
     DMG.qrUsuRegistro.Close;
     DMG.qrUsuRegistro.ParamByName('USUARIO').AsString := FG.RetUsuAtivo;
     DMG.qrUsuRegistro.ParamByName('CHAVE').AsString   := PChave;
     DMG.qrUsuRegistro.ParamByName('NOME').AsString    := PNome;
     DMG.qrUsuRegistro.Open;
     if not DMG.qrUsuRegistro.IsEmpty then
     begin
          try
             memAux := TMemoryStream.Create;
             bloAux := DMG.qrUsuRegistro.CreateBlobStream(DMG.qrUsuRegistro.FieldByName('VALOR'), bmRead);
             memAux.CopyFrom(bloAux, bloAux.Size);
             memAux.Position := 0;
             if memAux.Size > 0 then
             begin
                  CDS.Close;
                  CDS.LoadFromStream(memAux);
             end;
          finally
                 FH.DestroiObj(memAux);
          end;
     end;
end;

{------------------------------------------------------------------------------}
procedure GravaUsuRegistroBool(const PChave, PNome: string; const PValor: Boolean);
begin
  FG.GravaUsuRegistroStr(PChave, PNome, FH.BoolParaStr(PValor));
end;

{------------------------------------------------------------------------------}
procedure GravaUsuRegistroInt(const PChave, PNome: string; const PValor: Integer);
begin
  FG.GravaUsuRegistroStr(PChave, PNome, IntToStr(PValor));
end;

{------------------------------------------------------------------------------}
procedure GravaUsuRegistroNum(const PChave, PNome: string; const PValor: Extended);
begin
  FG.GravaUsuRegistroStr(PChave, PNome, FloatToStr(PValor));
end;

{------------------------------------------------------------------------------}
function LeUsuRegistroBool(const PChave, PNome: string; const PDefault: Boolean = False): Boolean;
begin
  Result := FH.StrParaBool(FG.LeUsuRegistroStr(PChave, PNome), PDefault);
end;

{------------------------------------------------------------------------------}
function LeUsuRegistroInt(const PChave, PNome: string; const PDefault: Integer = 0): Integer;
begin
  Result := StrToIntDef(FG.LeUsuRegistroStr(PChave, PNome), PDefault);
end;

{------------------------------------------------------------------------------}
function LeUsuRegistroNum(const PChave, PNome: string; const PDefault: Extended = 0): Extended;
begin
  Result := StrToFloatDef(FG.LeUsuRegistroStr(PChave, PNome), PDefault);
end;

{------------------------------------------------------------------------------}
procedure ChamaGrafico(FR: TForm; DS: TDataSet; const PTitulo: TStrings; const PCampoX, PCampoY, PFmt: string);
begin
  if FSisGraf = nil then
    FSisGraf := TFSisGraf.Create(FR);

  FSisGraf.Caption := FR.Caption + ' (Gráfico)';
  FSisGraf.Height  := FR.Height;
  FSisGraf.Width   := FR.Width;
  FSisGraf.Show;

  FSisGraf.MontaGrafico(DS, False, PTitulo, PCampoX, [PCampoY], [], PFmt);
end;

{------------------------------------------------------------------------------}
procedure ChamaGraficoEmpilhado(FR: TForm; DS: TDataSet; const PTitulo: TStrings; const PCampoX: string;
                                const PCampoY, PLegenda: array of string; const PFmt: string);
begin
  if FSisGraf = nil then
    FSisGraf := TFSisGraf.Create(FR);

  FSisGraf.Caption := FR.Caption + ' (Gráfico)';
  FSisGraf.Height  := FR.Height;
  FSisGraf.Width   := FR.Width;
  FSisGraf.Show;

  FSisGraf.MontaGrafico(DS, True, PTitulo, PCampoX, PCampoY, PLegenda, PFmt);
end;

{------------------------------------------------------------------------------}
procedure ReiniciarSistema;
var
    AppName: PChar;
begin
    AppName := PChar(Application.ExeName);
    Application.Terminate;
    ShellExecute(Application.MainForm.Handle, 'open', AppName, nil, nil, SW_SHOWNORMAL);
end;

{------------------------------------------------------------------------------}
function RetVersaoReqDB: string;
begin
    Result := IntToStr(FMenu.edVersaoDB.Value);
end;

{------------------------------------------------------------------------------}
function RetVersaoAtuDB(const StringErro: Boolean = True): string;
var
    qr : TIBOQuery;
begin
    try
        FG.CriaIBOQuery(qr, 'SELECT RDB$GENERATOR_NAME FROM RDB$GENERATORS WHERE RDB$GENERATOR_NAME = ''GEN_VERSAODB''');
        qr.Open;
        if qr.IsEmpty then
        begin
           if StringErro then
              Result := 'OBJETO DE CONTROLE DE VERSÃO NÃO EXISTE!'
           else
              Result := '';
        end
        else
           Result := IntToStr(qr.GeneratorValue('GEN_VERSAODB', 0));
        qr.Close;
    finally
        FH.DestroiObj(qr);
    end;
end;

{------------------------------------------------------------------------------}
procedure ChamaAtributos(FR: TForm; const pFRCaption, TabelaAtrib, Codigo, Descricao: string);
begin
  AtributoC01.vrgTabelaAtrib := TabelaAtrib;
  AtributoC01.vrgCodigo      := Codigo;
  AtributoC01.vrgDescricao   := Descricao;

  try
    FAtributoC01 := TFAtributoC01.Create(FR);
    FAtributoC01.Caption := pFRCaption;
    FAtributoC01.ShowModal;
  finally
    FAtributoC01.Free;
  end;
end;

{------------------------------------------------------------------------------}
procedure LimpaCacheConsultas;
begin
    FG.GravaCacheStr(FG.CnsProdutoCodigo   , '');
    FG.GravaCacheStr(FG.CnsProdutoDescricao, '');
end;

{------------------------------------------------------------------------------}
procedure RealinhaBtns(P: TPanel);
var
   tot, soma, e, i : Integer;
   bt              : TJvBitBtn;
   ac              : array of TWinControl;
   ps              : array of Integer;
begin
    e    := -500;
    tot  := 0;
    bt   := nil;
    soma := 0;

    for i := 0 to P.ControlCount-1 do
        if P.Controls[i] is TJvBitBtn then
           with (P.Controls[i] as TJvBitBtn) do
              if Visible and (Width > 0) and (Height > 0) then
              begin
                 Inc(tot);
                 soma := soma + Width + 2;
                 if Left > e then
                 begin
                    e  := Left;
                    bt := P.Controls[i] as TJvBitBtn;
                 end;
              end;

    if P.Width <= soma then
       Exit;

    if tot < 1 then
       Exit;

    SetLength(ac, tot);
    SetLength(ps, tot);

    tot := -1;
    for i := 0 to P.ControlCount-1 do
        if P.Controls[i] is TJvBitBtn then
           with (P.Controls[i] as TJvBitBtn) do
              if Visible and (Width > 0) and (Height > 0) then
              begin
                 Inc(tot);
                 ps[tot] := Left;
              end;

    FH.QuickSort(ps);

    for e := Low(ps) to High(ps) do
    begin
        for i := 0 to P.ControlCount-1 do
            if P.Controls[i] is TJvBitBtn then
               with (P.Controls[i] as TJvBitBtn) do
                  if Visible and (Width > 0) and (Height > 0) then
                     if Left = ps[e] then
                     begin
                         ac[e] := P.Controls[i] as TJvBitBtn;
                         Break;
                     end;
    end;

    for e := Low(ac) to High(ac) do
       if ac[e] = nil then
          Exit;

    bt.Left := P.Width - bt.Width - 4;

    FH.ReposCompsHrz(ac);
end;

{------------------------------------------------------------------------------}
procedure ChamaOcorrenciaCli(FR: TForm; const pID: Int64; const PCodCliente, PNome, PData, PHora, PUsuario: string);
begin
    if FCliOcorrencia = nil then
       FCliOcorrencia := TFCliOcorrencia.Create(FR);

    FCliOcorrencia.vpbID      := pID;
    FCliOcorrencia.vpbCliente := PCodCliente;
    FCliOcorrencia.vpbNome    := PNome;
    FCliOcorrencia.vpbData    := PData;
    FCliOcorrencia.vpbHora    := PHora;
    FCliOcorrencia.vpbUsuario := PUsuario;

    FCliOcorrencia.Caption := FG.RetFormCaption('_T' + FCliOcorrencia.Name);

    FG.CriaItemPopupMenu(FCliOcorrencia);

    FCliOcorrencia.Show;
    FCliOcorrencia.CarregaTela;
end;

procedure ChamaOcorrenciaCli(FR: TForm; const pID: Int64);
begin
    FG.ChamaOcorrenciaCli(FR, pID, '', '', '', '', '');
end;

{------------------------------------------------------------------------------}
procedure ChamaOcorrenciaFor(FR: TForm; const pID: Int64; const PCodFor, PNome, PData, PHora, PUsuario: string);
begin
    if FForOcorrencia = nil then
       FForOcorrencia := TFForOcorrencia.Create(FR);

    FForOcorrencia.vpbID      := pID;
    FForOcorrencia.vpbFor     := PCodFor;
    FForOcorrencia.vpbNome    := PNome;
    FForOcorrencia.vpbData    := PData;
    FForOcorrencia.vpbHora    := PHora;
    FForOcorrencia.vpbUsuario := PUsuario;

    FForOcorrencia.Caption := FG.RetFormCaption('_T' + FForOcorrencia.Name);

    FG.CriaItemPopupMenu(FForOcorrencia);

    FForOcorrencia.Show;
    FForOcorrencia.CarregaTela;
end;

procedure ChamaOcorrenciaFor(FR: TForm; const pID: Int64);
begin
    FG.ChamaOcorrenciaFor(FR, pID, '', '', '', '', '');
end;

{------------------------------------------------------------------------------}
procedure ProcessaSelConsulta(FR: TForm; const ManterAberto: Boolean = True);
begin
    if FR.Owner is TForm then
       if (FR.Owner as TForm).Name <> Application.MainForm.Name then
       begin
            if FSMODAL in (FR.Owner as TForm).FormState then
               FR.Close
            else
            begin
                 if ManterAberto then
                    FH.FocalizeFirst(FR)
                 else
                    FR.Close;
                 (FR.Owner as TForm).Show;
            end;
       end;
end;

{------------------------------------------------------------------------------}
function RetDataValidadeDemo: string;
var
    a      : Integer;
    d1, d2 : TDateTime;
begin
    Result := '';
    a      := Abs(FG.LeConfigEstabeInt(FG.CfgEstValidadeDemo));

    if a < 1 then
       Exit;

    d1 := IncDay(FG.DataLocal, a);
    d2 := FH.UltimoDiaMes(FG.DataLocal);

    if DayOfWeek(d1) = 1 then
       d1 := IncDay(d1, -1);
    if DayOfWeek(d2) = 1 then
       d2 := IncDay(d2, -1);

    if d1 > d2 then
       Result := DateToStr(d2)
    else
       Result := DateToStr(d1);
end;

{------------------------------------------------------------------------------}
function ClasseFormPermitida(const PClasse: string): Boolean;
begin
    Result := False;
    { este teste evita a contagem dos forms que o QuantumGrid cria }
    if FH.ComparaTextoFixo(PClasse, 'Tcx') then
      Exit;
    { este teste evita a contagem dos forms que o FastReport cria }
    if FH.ComparaTextoFixo(PClasse, 'TParentForm') then
      Exit;
    Result := True;
end;

{------------------------------------------------------------------------------}
function VerVinculosCaixa: Boolean;
var
    s: string;
begin
    Result := False;
    if FG.RetUsuCaixa = '' then
    begin
        FH.PrMsg_ER('Usuário não está vinculado a nenhum caixa. Não é possível realizar movimentações.');
        Exit;
    end;
    if FG.RetUsuAmxCaixa = '' then
    begin
        FG.ChamaMsgDt('Caixa do usuário não está vinculado a nenhum almoxarifado. Não é possível realizar movimentações.',
                      'Caixa : ' + FG.RetUsuCaixa);
        Exit;
        Exit;
    end;
    s := FG.Lookup('CAXCADASTRO', 'ESTABE', 'CODIGO', FG.RetUsuCaixa);
    if FG.RetEstabeAtivo <> s then
    begin
        FG.ChamaMsgDt('Caixa do usuário está vinculado a um estabelecimento diferente do estabelecimento ativo.',
                      'Caixa                   : ' + FG.RetUsuCaixa + #13 +
                      'Estabelecimento do caixa: ' + s + #13 +
                      'Estabelecimento ativo   : ' + FG.RetEstabeAtivo);
        Exit;
    end;
    Result := True;
end;

{------------------------------------------------------------------------------}
function verTabAtributo(ATab_Atributo : string) : Boolean;
begin
     //**************
     // funciona só para o tipo 'P'
     //**************
     DMG.qrTabAtributo.Close;
     DMG.qrTabAtributo.ParamByName('NOME').AsString := ATab_Atributo;
     DMG.qrTabAtributo.Open;
     Result := not DMG.qrTabAtributo.IsEmpty;
     DMG.qrTabAtributo.Close;
end;

{------------------------------------------------------------------------------}
function ExisteAtributo(Tipo : string) : Boolean;
begin
     DMG.qrTipoAtributo.Close;
     DMG.qrTipoAtributo.ParamByName('tipo').AsString := Tipo;
     DMG.qrTipoAtributo.Open;
     Result := not DMG.qrTipoAtributo.IsEmpty;
     DMG.qrTipoAtributo.Close;
end;

{------------------------------------------------------------------------------}
function LeConfigComPedido: string;
var
  tmpQuery: TIBODataSet;
begin
  Result := '';
  try
    tmpQuery := TIBODataSet.Create(nil);
    with tmpQuery do
    begin
      IB_Connection := DMG.IB_Connection;
      SQL.Text := 'select config from compedidocfg';
      Open;
      if IsEmpty then
        exit;
      Result := FieldByName('CONFIG').AsString;
    end;
  finally
    FH.DestroiObj(tmpQuery);
  end;
end;

{------------------------------------------------------------------------------}
function LeConfigComPedidoStr(const pID: string; const PDefault: string = ''): string;
begin
  Result := FH.IniLeStr(DMG.vpbConfigComPedido, FG.SECAO_GERAL, pID, PDefault);
end;

{------------------------------------------------------------------------------}
function LeConfigComPedidoBool(const pID: string; const PDefault: boolean): boolean;
begin
  Result := FH.IniLeBln(DMG.vpbConfigComPedido, FG.SECAO_GERAL, pID, PDefault);
end;

{------------------------------------------------------------------------------}
function LeConfigComPedidoInt(const pID: string; const PDefault: integer = 0): integer;
begin
  Result := FH.IniLeInt(DMG.vpbConfigComPedido, FG.SECAO_GERAL, pID, PDefault);
end;

{------------------------------------------------------------------------------}
function LeConfigComPedidoNum(const pID: string; const PDefault: double = 0): double;
begin
  Result := FH.IniLeNum(DMG.vpbConfigComPedido, FG.SECAO_GERAL, pID, PDefault);
end;

{------------------------------------------------------------------------------}
procedure GravaConfigComPedidoStr(const pID, PValor: string; TR: TIB_Transaction = nil);
var
  lst: TStrings;
  tmpQuery: TIBOQuery;
begin
  try
    tmpQuery := TIBOQuery.Create(nil);
    tmpQuery.IB_Connection := DMG.IB_Connection;

    if TR <> nil then
      tmpQuery.IB_Transaction := TR;

    tmpQuery.SQL.Text := 'select * from Compedidocfg';
    tmpQuery.Open;

    FH.IniGera(lst, tmpQuery.FieldByName('CONFIG').AsString);
    FH.IniGravaStr(lst, FG.SECAO_GERAL, pID, PValor);

    tmpQuery.Close;
    tmpQuery.SQL.Text := 'update Compedidocfg set config = ' + QuotedStr(lst.Text);
    tmpQuery.ExecSQL;

    DMG.vpbConfigComPedido.Text := lst.Text;
  finally
    FH.DestroiObj(lst);
    FH.DestroiObj(tmpQuery);
  end;
end;

{------------------------------------------------------------------------------}
function MontaEndereco(const PLogradouro, PNumero, PComplemento: string): string;
begin
     Result := Trim(PLogradouro);
     if PNumero > '' then
     begin
          if Result > '' then
             Result := Result + ', ';
          Result := Result + PNumero;
     end;
     if PComplemento > '' then
     begin
          if Result > '' then
             Result := Result + ' ';
          Result := Result + PComplemento;
     end;
end;

{------------------------------------------------------------------------------}
function RetInfoPro(const aCodProduto: string; out aHeader, aTexto: string): string;
var
   qr: TIBOQuery;
   tmp: string;
begin
     aHeader := '';
     aTexto  := '';
     tmp     := '';
     try
        FG.CriaIBOQuery(qr);
        qr.SQL.Text := ' select pro.*, ' +
                       '        fam.grupo, ' +
                       '        fam.descricao as desfam, ' +
                       '        mar.descricao as desmar, ' +
                       '        cen.descricao as descen, ' +
                       '        grp.descricao as desgru, ' +
                       '        cfi.classificacao, ' +
                       '        cl1.descricao as desclas1, ' +
                       '        cl2.descricao as desclas2, ' +
                       '        cl3.descricao as desclas3 ' +
                       '  from procadastro pro ' +
                       '       left join profamilia     fam on fam.codigo     = pro.familia ' +
                       '       left join promarca       mar on mar.codigo     = pro.marca ' +
                       '       left join tabcentrocusto cen on pro.codcentro  = cen.codigo ' +
                       '       left join progrupo       grp on fam.grupo      = grp.codigo ' +
                       '       left join proclas1       cl1 on cl1.codigo     = pro.clas1 ' +
                       '       left join proclas2       cl2 on cl2.codigo     = pro.clas2 ' +
                       '       left join proclas3       cl3 on cl3.codigo     = pro.clas3 ' +
                       '       left join tabclasfiscal  cfi on cfi.codigo     = pro.codclasfiscal ' +
                       ' where pro.codigo = ' + aCodProduto;
        qr.Open;

        aHeader := qr.FieldByName('descricao').AsString+' ('+qr.FieldByName('codigo').AsString+')';

        tmp     := 'Código' +#9': '+qr.FieldByName('codigo').AsString + #13 +
                   'Desc.'  +#9': '+qr.FieldByName('descricao').AsString + #13;

        Result  := 'Família'+#9': '+qr.FieldByName('desfam').AsString+' ('+qr.FieldByName('familia').AsString+')'+ #13;

        if qr.FieldByName('marca').AsString > '' then
           Result := Result +'Marca'#9': '+qr.FieldByName('desmar').AsString+' ('+qr.FieldByName('marca').AsString+')'+ #13;

        if qr.FieldByName('codcentro').AsString > '' then
           Result := Result +'Centro'#9': '+qr.FieldByName('descen').AsString+' ('+qr.FieldByName('codcentro').AsString+')'+ #13;

        if qr.FieldByName('grupo').AsString > '' then
           Result := Result +'Grupo'#9': '+qr.FieldByName('desgru').AsString+' ('+qr.FieldByName('grupo').AsString+')'+ #13;

        if qr.FieldByName('classificacao').AsString > '' then
           Result := Result +'Clas.Fiscal'#9': '+qr.FieldByName('classificacao').AsString+ #13;

        if qr.FieldByName('clas1').AsString > '' then
           if FG.VerProClas(1) then
              Result := Result+FG.RetProClas(1)+#9': '+qr.FieldByName('desclas1').AsString+' ('+qr.FieldByName('clas1').AsString+')'+#13;

        if qr.FieldByName('clas2').AsString > '' then
           if FG.VerProClas(2) then
              Result := Result+FG.RetProClas(2)+#9': '+qr.FieldByName('desclas2').AsString+' ('+qr.FieldByName('clas2').AsString+')'+ #13;

        if qr.FieldByName('clas3').AsString > '' then
           if FG.VerProClas(3) then
              Result := Result+FG.RetProClas(3)+#9': '+qr.FieldByName('desclas3').AsString+' ('+qr.FieldByName('clas3').AsString+')'+ #13;

        Result := Result + #13 +
                  'Refer.'#9': '+qr.FieldByName('referencia').AsString + #13 +
                  'Unid.'#9': ' +qr.FieldByName('unidade').AsString + #13 +
                  'Preço'#9': ' +FH.FmtNum(qr.FieldByName('preco').AsFloat);

        aTexto := Result;
        Result := tmp + Result;
     finally
        FH.DestroiObj(qr);
     end;
end;

{------------------------------------------------------------------------------}
procedure GravaDicaProduto(aEdit: TCustomEdit; const aCodProduto: string);
var h, t: string;
begin
     DMG.vpbProdutoDica := aCodProduto;

     if aCodProduto = '' then
     begin
          FG.GravaCacheStr(aEdit.Owner.Name + aEdit.Name + '_DicaPro_HD', '');
          FG.GravaCacheStr(aEdit.Owner.Name + aEdit.Name + '_DicaPro_TX', '');
          FG.GravaCacheStr(aEdit.Owner.Name + aEdit.Name + '_DicaPro_CD', '');
     end
     else
     begin
          FG.RetInfoPro(aCodProduto, h, t);
          FG.GravaCacheStr(aEdit.Owner.Name + aEdit.Name + '_DicaPro_HD', h);
          FG.GravaCacheStr(aEdit.Owner.Name + aEdit.Name + '_DicaPro_TX', t);
          FG.GravaCacheStr(aEdit.Owner.Name + aEdit.Name + '_DicaPro_CD', aCodProduto);
     end;
end;

{------------------------------------------------------------------------------}
procedure AtivaBalaoDicaProduto(aEdit: TCustomEdit);
begin
     if FG.LeCacheStr(aEdit.Owner.Name + aEdit.Name + '_DicaPro_HD') = '' then
        Exit;
     DMG.balloonInfoPro.ActivateHint( aEdit,
                                      FG.LeCacheStr(aEdit.Owner.Name + aEdit.Name + '_DicaPro_TX'),
                                      'Produto '+FG.LeCacheStr(aEdit.Owner.Name + aEdit.Name + '_DicaPro_HD'),
                                      10000 );
end;

{------------------------------------------------------------------------------}
procedure FechaBalaoDicaProduto;
begin
     DMG.balloonInfoPro.CancelHint;
end;

{------------------------------------------------------------------------------}
procedure AcessaDicaProduto;
var c: string;
begin
     c := DMG.vpbProdutoDica;
     if c > '' then
        FG.ChamaCnsProStq(Application.MainForm, c);
end;

{------------------------------------------------------------------------------}
procedure ChamaBalaoDica(CT: TControl; const PTitulo, PTexto: string);
begin
     DMG.balloonDica.ActivateHint( CT, PTexto, PTitulo, 20000 );
end;

{------------------------------------------------------------------------------}
procedure FechaBalaoDica;
begin
     DMG.balloonDica.CancelHint;
end;

{------------------------------------------------------------------------------}
function CodIBGEParaUF(const aCodigo: string; const aUFDefault: string = 'RS'): string;
begin
     Result := aCodigo;
     if aCodigo = '11' then Result := 'RO' else
     if aCodigo = '12' then Result := 'AC' else
     if aCodigo = '13' then Result := 'AM' else
     if aCodigo = '14' then Result := 'RR' else
     if aCodigo = '15' then Result := 'PA' else
     if aCodigo = '16' then Result := 'AP' else
     if aCodigo = '17' then Result := 'TO' else
     if aCodigo = '21' then Result := 'MA' else
     if aCodigo = '22' then Result := 'PI' else
     if aCodigo = '23' then Result := 'CE' else
     if aCodigo = '24' then Result := 'RN' else
     if aCodigo = '25' then Result := 'PB' else
     if aCodigo = '26' then Result := 'PE' else
     if aCodigo = '27' then Result := 'AL' else
     if aCodigo = '28' then Result := 'SE' else
     if aCodigo = '29' then Result := 'BA' else
     if aCodigo = '31' then Result := 'MG' else
     if aCodigo = '32' then Result := 'ES' else
     if aCodigo = '33' then Result := 'RJ' else
     if aCodigo = '35' then Result := 'SP' else
     if aCodigo = '41' then Result := 'PR' else
     if aCodigo = '42' then Result := 'SC' else
     if aCodigo = '43' then Result := 'RS' else
     if aCodigo = '50' then Result := 'MS' else
     if aCodigo = '51' then Result := 'MT' else
     if aCodigo = '52' then Result := 'GO' else
     if aCodigo = '53' then Result := 'DF' else
                            Result := aUFDefault;
end;

{------------------------------------------------------------------------------}
function UFParaCodIBGE(const aUF: string; const aCodigoDefault: string = '43'): string;
begin
     Result := aUF;
     if aUF = 'RO' then Result := '11' else
     if aUF = 'AC' then Result := '12' else
     if aUF = 'AM' then Result := '13' else
     if aUF = 'RR' then Result := '14' else
     if aUF = 'PA' then Result := '15' else
     if aUF = 'AP' then Result := '16' else
     if aUF = 'TO' then Result := '17' else
     if aUF = 'MA' then Result := '21' else
     if aUF = 'PI' then Result := '22' else
     if aUF = 'CE' then Result := '23' else
     if aUF = 'RN' then Result := '24' else
     if aUF = 'PB' then Result := '25' else
     if aUF = 'PE' then Result := '26' else
     if aUF = 'AL' then Result := '27' else
     if aUF = 'SE' then Result := '28' else
     if aUF = 'BA' then Result := '29' else
     if aUF = 'MG' then Result := '31' else
     if aUF = 'ES' then Result := '32' else
     if aUF = 'RJ' then Result := '33' else
     if aUF = 'SP' then Result := '35' else
     if aUF = 'PR' then Result := '41' else
     if aUF = 'SC' then Result := '42' else
     if aUF = 'RS' then Result := '43' else
     if aUF = 'MS' then Result := '50' else
     if aUF = 'MT' then Result := '51' else
     if aUF = 'GO' then Result := '52' else
     if aUF = 'DF' then Result := '53' else
                        Result := aCodigoDefault;
end;

{------------------------------------------------------------------------------}
procedure LayoutGridView(GR: TcxGridDBTableView; const TipoAcao: Integer; const Arquivo: string='');
var c : TcxControl;
    a : string;
begin
     c := GR.Site.Container;
     a := Arquivo;

     if a = '' then
        a := FG.SetaLocalIni+'Layout_'+c.Owner.Name+'_'+c.Name+'.ini'
     else
     if SameText(a,'ANT') then
        a := FG.SetaLocalIni+c.Owner.Name+c.Name+'.ini'
     else
     if SameText(a,'FORM') then
        a := FG.SetaLocalIni+c.Owner.Name+'.ini';

     case TipoAcao of
          0 : GR.RestoreFromIniFile(a);
          1 : begin
                   GR.StoreToIniFile(a);
                   try try ProgGer.ChamaStat((c.Owner as TForm), 'Layout gravado.');
                           Sleep(500);                   except end;
                   finally ProgGer.FechaStat; FH.SetaCursorDef; end;
              end;
          2 : begin
                   SysUtils.DeleteFile(a);
                   try try ProgGer.ChamaStat((c.Owner as TForm), 'Layout revertido.');
                           Sleep(500);                   except end;
                   finally ProgGer.FechaStat; FH.SetaCursorDef; end;
              end;
     end;
end;

{------------------------------------------------------------------------------}
procedure LayoutGridView(GR: TcxGridDBBandedTableView; const TipoAcao: Integer; const Arquivo: string='');
var c : TcxControl;
    a : string;
begin
     c := GR.Site.Container;
     a := Arquivo;

     if a = '' then
        a := FG.SetaLocalIni+'Layout_'+c.Owner.Name+'_'+c.Name+'.ini'
     else
     if SameText(a,'ANT') then
        a := FG.SetaLocalIni+c.Owner.Name+c.Name+'.ini'
     else
     if SameText(a,'FORM') then
        a := FG.SetaLocalIni+c.Owner.Name+'.ini';

     case TipoAcao of
          0 : GR.RestoreFromIniFile(a);
          1 : begin
                   GR.StoreToIniFile(a);
                   try try ProgGer.ChamaStat((c.Owner as TForm), 'Layout gravado.');
                           Sleep(500);                   except end;
                   finally ProgGer.FechaStat; FH.SetaCursorDef; end;
              end;
          2 : begin
                   SysUtils.DeleteFile(a);
                   try try ProgGer.ChamaStat((c.Owner as TForm), 'Layout revertido.');
                           Sleep(500);                   except end;
                   finally ProgGer.FechaStat; FH.SetaCursorDef; end;
              end;
     end;
end;

{------------------------------------------------------------------------------}
procedure SetaPrimeiroValido(E1, E2 : TJvDateEdit);
begin
     if FH.DataInvalida(E1.Text) and FH.DataOk(E2.Text) then
        E1.Text := E2.Text
     else
     if FH.DataInvalida(E2.Text) and FH.DataOk(E1.Text) then
        E2.Text := E1.Text;
end;


{------------------------------------------------------------------------------}
function CalculaDV(const PBanco, PNumero: string; PRecalculaBanrisul: Boolean = True): string;

var S, vNro, vDV1, vDV2 : string;
    P, I, vSoma, Resto  : Integer;


    function CalcModulo11(var pNro: string): boolean;

    var j, Peso, x: Integer;

    begin
         result := true;
         x      := 0;
         Peso   := 2;

         for j := Length(pNro) downto 1 do
         begin
              x := x + StrToInt(Copy(pNro, j, 1)) * Peso;
              if Peso = 7 then
                Peso := 2
              else
                Inc(Peso);
         end;

         if x > 10 then
           x := x mod 11;

         x := 11 - x;
         if x = 10 then
         begin
              Result := false;
              x      := 0;
         end
         else
         if x = 11 then 
           x := 0;

         if not PRecalculaBanrisul then
            Result := true;

         pNro := IntToStr(x);
    end;

    
begin
     if PBanco = '041' then
     begin
          vNro := PNumero;
          vDV1 := FH.Modulo10(vNro);
          vDV2 := vNro + vDV1;
          while not CalcModulo11(vDV2) do
          begin
               vDV1 := IntToStr(StrToInt(vDV1) + 1);
               if vDV1 = '10' then
                  vDV1 := '0';
               vDV2 := vNro + vDV1;
          end;
          result := vNro + vDV1 + vDV2;
     end
     else
     if PBanco = '104' then
     begin
          S := ReverseString(PNumero);
          P := 2;
          vSoma := 0;
          for I := 1 to Length(S) do
          begin
            vSoma := vSoma + (P * StrToInt(S[I]));
            Inc(P);
            if P > 9 then
              P := 2;
          end;
          vSoma := 11 - (vSoma mod 11);
          if vSoma > 9 then
            vSoma := 0;

          Result := PNumero + IntToStr(vSoma);
     end
     else
     if PBanco = '748' then
     begin
          S := ReverseString(PNumero);
          P := 2;
          vSoma := 0;
          for I := 1 to Length(S) do
          begin
            vSoma := vSoma + (P * StrToInt(S[I]));
            Inc(P);
            if P > 9 then
              P := 2;
          end;
          vSoma := 11 - (vSoma mod 11);
          if vSoma > 9 then
            vSoma := 0;

          Result := PNumero + IntToStr(vSoma);
     end
     else
     if PBanco = '001' then
     begin
          S := ReverseString(PNumero);
          P := 2;
          vSoma := 0;
          for I := 1 to Length(S) do
          begin
            vSoma := vSoma + (P * StrToInt(S[I]));
            Inc(P);
            if P > 9 then
              P := 2;
          end;
          Resto := vSoma mod 11;

          if Resto < 10 then
             S := IntToStr(Resto)
          else
          if Resto = 10 then
            S := 'X'
          else
          if Resto = 0 then
            S := '0';

          Result := PNumero + S;
     end;
end;

{------------------------------------------------------------------------------}
function MontaNossoNumero(const PBanco, PNumero: string; const PAgenciaSicredi: string = ''): string;
var
    vNossoNumero : string;
begin
     vNossoNumero := FH.StrInteiro(PNumero);

     if PBanco = '041' then
     begin
          Result := FG.CalculaDV(PBanco, FH.PreencherEsq(vNossoNumero, '0', 8));

          system.Insert('-', Result, Length(Result) - 1);
     end
     else
     if PBanco = '104' then
     begin
          Result := FH.PreencherEsq(vNossoNumero, '0', 17);
          Result := Result + FH.Modulo11(vNossoNumero);

          system.Insert('-', Result, Length(Result));
     end
     else
     if PBanco = '748' then
     begin
          Result := FH.PreencherEsq(vNossoNumero, '0', 8);
          Result := Result + '-' + RightStr(FG.CalculaDV(PBanco, FH.LimpaPonto(PAgenciaSicredi) + Result), 1);

          system.Insert('/', Result, 3);
     end
     else
     if PBanco = '001' then
     begin
          Result := FG.CalculaDV(PBanco, FH.PreencherEsq(vNossoNumero, '0', 17));

          system.Insert('-', Result, Length(Result));
     end;
end;

{------------------------------------------------------------------------------}
function MontaNossoNumeroPortador(const PPortador, PNumero: string) : string;
var
   qr: TIBOQuery;
   vAgSicredi: string;
begin
     try
        FG.CriaIBOQuery(qr);
        qr.SQL.Text := ' select ban.* ' +
                       '  from bancadastro ban ' +
                       ' where ban.codigo = ' + QuotedStr(PPortador);
        qr.Open;

        vAgSicredi := qr.FieldByName('BCOAGE').AsString  + '.'
                    + qr.FieldByName('BCOAGED').AsString + '.'
                    + qr.FieldByName('CEDCOD').AsString;

        Result := MontaNossoNumero(qr.FieldByName('BCONRO').AsString,PNumero,vAgSicredi);

        qr.Close;

     finally
        FH.DestroiObj(qr);
     end;
end;

{------------------------------------------------------------------------------}
function GeraSICN010(var   aStringConexao: string;
                     const aProduto      ,
                           aNSU          ,
                           aCPFCNPJ      ,
                           aBanco        ,
                           aAgencia      ,
                           aConta        ,
                           aContaDv      ,
                           aCheque       ,
                           aChequeDv     ,
                           aChequeQtd    ,
                           aDDDFone      ,
                           aFone         : string) : Boolean;

const
//   cEntidade = 'CBA001';   // cdl salvador bahia
   cEntidade = 'CRS009'; // cdl caxias   RS

var
   v010Str : string;



      function fmtStr(S: string; ATam: Integer): string;
      begin
        Result := FH.PreencheDir(FH.LimpaChrEspecial(Trim(S)), ATam);
      end;


      function fmtInt(S: string; ATam: Integer): string; overload;
      begin
        Result := FH.StrZero(S, ATam);
      end;


      function fmtInt(N: Int64; ATam: Integer): string; overload;
      begin
        Result := FH.StrZero(IntToStr(N), ATam);
      end;


      function fmtFlt(V: Double; ATam, ADec: Integer): string;
      begin
          Result := FH.FmtFloatStr(V, ATam + ADec, ADec);
      end;


begin
    Result         := False;
    aStringConexao := '';

    if FG.LeConfigEstabeStr(Fg.CfgEstUsuSenhaSPC) = '' then
    begin
         FH.PrMsg_ER('Senha não informada na configuração do estabelecimento');
         Result   := False;
         Exit;
    end;
    if FG.LeConfigEstabeStr(Fg.CfgEstUsuLogonSPC) = '' then
    begin
         FH.PrMsg_ER('Logon não informado na configuração do estabelecimento');
         Result   := False;
         Exit;
    end;

    if Length(aCPFCNPJ) > 12 then
    begin
         if not FH.VerificaCNPJ(aCPFCNPJ) then
         begin
              Fh.PrMsg_ER('CNPJ inválido');
              Exit;
         end;
    end
    else
    begin
         if not FH.VerificaCPF(aCPFCNPJ) then
         begin
              Fh.PrMsg_ER('CPF inválido');
              Exit;
         end;
    end;

    v010Str := '';

    { Código da transação  }
    //Conteudo = SICN010
    v010Str := fmtStr('SICN010',7);

    { Versão  }
    //Conteudo = 02
    v010Str := v010Str + fmtStr('02',2);

    { Identificação do solicitante  }
    //Conteudo = livre para uso do solicitante (cliente)
    v010Str := v010Str + fmtStr('.PRESSIER.',10);

    { Origem da solicitação }
    //Conteudo = cep da localidade origem
    v010Str := v010Str + fmtInt(fh.LimpaString(FG.RetEstabeAtivoCep,['-']),8);

    { Código da entidade }
    //Conteudo = crs009 = cdl caxias
    v010Str := v010Str + fmtStr(cEntidade,6);

    { Logon do usuario }
    //Conteudo = solicitar entidade
    v010Str := v010Str + fmtStr(FG.LeConfigEstabeStr(Fg.CfgEstUsuLogonSPC),8);

    { Senha do usuario }
    //Conteudo = solicitar entidade
    v010Str := v010Str + fmtStr(FG.LeConfigEstabeStr(Fg.CfgEstUsuSenhaSPC),8);

    { Codigo do produto }
    //Conteudo = tabela de produtos da entidade
    v010Str := v010Str + fmtInt(aProduto,3);

    { NSU da consulta }
    //Conteudo = numero da consulta realizada
    v010Str := v010Str + fmtInt(aNSU,10);

    { Tipo de documento }
    //Conteudo = 1 cpf     2 cnpj
    if FH.VerificaCNPJ(aCPFCNPJ) then
    begin
         v010Str := v010Str + fmtInt('2',1);
         { Documento }
         //Conteudo = cnpj
         v010Str := v010Str + fmtInt(FH.StrInteiro(aCPFCNPJ),14);
    end
    else
    if FH.VerificaCPF(aCPFCNPJ) then
    begin
         v010Str := v010Str + fmtInt('1',1);
         { Documento }
         //Conteudo = cpf
         v010Str := v010Str + fmtInt(FH.StrInteiro(aCPFCNPJ),14);
    end;

    { Tipo do consultado }
    //Conteudo = c=comprador f-fiador l-locatario
    v010Str := v010Str + 'c'; // TAM=1

    { tipo da consulta em cheque }
    //Conteudo = 0=Sem cheque 1=digitado  2=cmc7
    v010Str := v010Str + fmtInt(IfThen(aChequeQtd > '0','1','0') ,1);

    { quantidade de cheques }
    //Conteudo = qtd de cheques a consultar
    v010Str := v010Str + fmtInt(aChequeQtd,2);

    ///===
    (* Dados do CHEQUE - DIGITADO *)

    { banco }
    v010Str := v010Str + fmtInt(aBanco,3);
    { agencia }
    v010Str := v010Str + fmtInt(aAgencia,4);
    { conta corrente }
    v010Str := v010Str + fmtInt(aConta,15);
    { digito conta ciorrente }
    v010Str := v010Str + fmtInt(aContaDv,1);
    { numero cheque inicial }
    v010Str := v010Str + fmtInt(aCheque,6);
    { digito cheque }
    v010Str := v010Str + fmtStr(aChequeDv,1);

    ///===
    (* Dados do CHEQUE - CMC7 *)

    { CMC7-1 }
    v010Str := v010Str + fmtInt('',8);
    { CMC7-2 }
    v010Str := v010Str + fmtInt('',10);
    { CMC7-3 }
    v010Str := v010Str + fmtInt('',12);

    ///===
    (* Dados do TELEFONE *)

    { DDD }
    v010Str := v010Str + fmtInt(aDDDFone,2);
    { Telefone }
    v010Str := v010Str + fmtInt(FH.StrInteiro(aFone),8);

    ///===
    (* RESERVADO *)
    v010Str := v010Str + fmtStr('',99);

    ///=== FIM DO ARQUIVO

    aStringConexao := v010Str;

    result         := True;
end;

{------------------------------------------------------------------------------}
function LeValorGroupFooter(GR: TcxGridDBTableView; const IDX: Integer; const Campo: string): Variant;
var  x : Integer;
begin
     x := GR.DataController.Summary.SummaryGroups.Summary.DefaultGroupSummaryItems.IndexOfItemLink(GR.GetColumnByFieldName(Campo));
     if VarIsNull(GR.DataController.Summary.GroupSummaryValues[IDX, x]) then
        Result := 0
     else
        Result := GR.DataController.Summary.GroupSummaryValues[IDX, x];
end;

procedure SetaValorGroupFooter(GR: TcxGridDBTableView; const IDX: Integer; const Campo: string; const Valor: Variant);
var  x : Integer;
begin
     x := GR.DataController.Summary.SummaryGroups.Summary.DefaultGroupSummaryItems.IndexOfItemLink(GR.GetColumnByFieldName(Campo));
     GR.DataController.Summary.GroupSummaryValues[IDX, x] := Valor;
end;

{------------------------------------------------------------------------------}
function LeValorFooter(GR: TcxGridDBTableView; DS: TcxDataSummary; const Campo: string): Variant;
var  x : Integer;
begin
     x := DS.FooterSummaryItems.IndexOfItemLink(GR.GetColumnByFieldName(Campo));
     if VarIsNull(TcxDataSummary(DS).FooterSummaryValues[x]) then
        Result := 0
     else
        Result := TcxDataSummary(DS).FooterSummaryValues[x];
end;

procedure SetaValorFooter(GR: TcxGridDBTableView; DS: TcxDataSummary; const Campo: string; const Valor: Variant);
var  x : Integer;
begin
     x := DS.FooterSummaryItems.IndexOfItemLink(GR.GetColumnByFieldName(Campo));
     TcxDataSummary(DS).FooterSummaryValues[x] := Valor;
end;

{------------------------------------------------------------------------------}
procedure AlteraColunasGridView(GR: TcxGridDBTableView; const Incremento: Integer);
var
     intFor: Integer;
begin
     for intFor := 0 to GR.ColumnCount-1 do
         GR.Columns[intFor].Width := GR.Columns[intFor].Width + Incremento;
end;

{------------------------------------------------------------------------------}
function VerVendedorAusente(const pCodigo: string): Boolean;
begin
     Result := False;
     if FG.FindKey(DMG.qrVendedor,'codigo',[pCodigo]) then
        if  FH.DataOk(DMG.qrVendedor.FieldByName('dtausenciaini').AsString)
        and FH.DataOk(DMG.qrVendedor.FieldByName('dtausenciafin').AsString) then
            Result := FH.DataNoIntervalo( FG.DataLocal,
                                          DMG.qrVendedor.FieldByName('dtausenciaini').AsDateTime ,
                                          DMG.qrVendedor.FieldByName('dtausenciafin').AsDateTime );
end;

{------------------------------------------------------------------------------}
procedure ApplyBestFitViewDetalhe(pDataController: TcxCustomDataController; pRecordIndex: Integer);
var
   ARecord    : TcxCustomGridRecord;
   ADetailView: TcxCustomGridView;
begin
     pDataController.FocusedRecordIndex := pRecordIndex;
     ADetailView := nil;
     ARecord     := TcxGridTableView(TcxGridDBDataController(pDataController).GridView).Controller.FocusedRecord;
     if ARecord is TcxGridMasterDataRow then
     begin
          ADetailView := TcxGridMasterDataRow(ARecord).ActiveDetailGridView;
          TcxGridTableView(ADetailView).ApplyBestFit();
     end;
end;

{------------------------------------------------------------------------------}
function RetSQLRAT(const pSelect, pOrderBy: Boolean): string;
begin
     Result := '';
     if pSelect then
        Result := Result + 'select mov.*,' +
                        #13'       (mov.vlconserto + mov.vldespesa) as total,' +
                        #13'       case mov.garantia ' +
                        #13'         when ''S'' THEN ''Sim'' '+
                        #13'         ELSE ''Não'' '+
                        #13'       end as des_garantia, '+
                        #13'       case mov.substituido ' +
                        #13'         WHEN ''S'' THEN ''Sim'' '+
                        #13'         ELSE ''Não'' '+
                        #13'       end as des_substituido, '+
                        #13'       case mov.situacao ' +
                        #13'         WHEN ''EA'' THEN ''EA-Em Avaliação'' '+
                        #13'         WHEN ''EC'' THEN ''EC-Em Conserto'' '+
                        #13'         WHEN ''CO'' THEN ''CO-Consertada'' '+
                        #13'         WHEN ''FI'' THEN ''FI-Finalizada'' '+
                        #13'       end as des_situacao '+
                        #13' from satmov mov';

     if pOrderBy then
        Result := Result + 'order by mov.nomefor';
end;

{------------------------------------------------------------------------------}
function MontaCam(C : TComponent; const Extensao: string = '.ini'): string;
begin
     Result := FG.SetaLocalIni + C.Owner.Name + C.Name + Extensao;
end;

{------------------------------------------------------------------------------}
procedure SetaEditPreco(CalcEdit: TJvCalcEdit);
begin
     CalcEdit.DisplayFormat := FG.RetMascaraPreco;
     CalcEdit.DecimalPlaces := FG.RetDecsPreco;
end;

{------------------------------------------------------------------------------}
function VerReferenciaEspecial(DS: TDataSet; const pFrete  : Boolean = True;
                                             const pSeguro : Boolean = True;
                                             const pDespesa: Boolean = True): Boolean;
begin
     Result := (pFrete   and AnsiSameText(DS.FieldByName('REFERENCIA').AsString,'FRETE'  ))
            or (pSeguro  and AnsiSameText(DS.FieldByName('REFERENCIA').AsString,'SEGURO' ))
            or (pDespesa and AnsiSameText(DS.FieldByName('REFERENCIA').AsString,'DESPESA'));
end;

{------------------------------------------------------------------------------}
function RetNFe_CodigoDesc(const pValor, pCampo: string): string;
var
     vDt : string;
begin
     Result := '';

     if pCampo = NFE_ModBC then
     begin
        if pValor  = NFE_ModBC_MrgVlrAgregado then
           Result := NFE_ModBC_MrgVlrAgregado + ' - ' + NFE_ModBC_MrgVlrAgregado_Des
        else
        if pValor  = NFE_ModBC_Pauta then
           Result := NFE_ModBC_Pauta + ' - ' + NFE_ModBC_Pauta_Des
        else
        if pValor  = NFE_ModBC_PrcTabelado then
           Result := NFE_ModBC_PrcTabelado + ' - ' + NFE_ModBC_PrcTabelado_Des
        else
        if pValor  = NFE_ModBC_VlrOperacao then
           Result := NFE_ModBC_VlrOperacao + ' - ' + NFE_ModBC_VlrOperacao_Des
     end
     else
     if pCampo = NFE_PIS_CST then
     begin
        if pValor  = NFE_PIS_CST_TribMonofasica then
           Result := NFE_PIS_CST_TribMonofasica + ' - ' + NFE_PIS_CST_TribMonofasica_Des
        else
        if pValor  = NFE_PIS_CST_TribAliqZero then
           Result := NFE_PIS_CST_TribAliqZero + ' - ' + NFE_PIS_CST_TribAliqZero_Des
        else
        if pValor  = NFE_PIS_CST_InsetoContrib then
           Result := NFE_PIS_CST_InsetoContrib + ' - ' + NFE_PIS_CST_InsetoContrib_Des
        else
        if pValor  = NFE_PIS_CST_SemIncidContrib then
           Result := NFE_PIS_CST_SemIncidContrib + ' - ' + NFE_PIS_CST_SemIncidContrib_Des
        else
        if pValor  = NFE_PIS_CST_SuspContrib then
           Result := NFE_PIS_CST_SuspContrib + ' - ' + NFE_PIS_CST_SuspContrib_Des;
     end
     else
     if pCampo = NFE_COFINS_CST then
     begin
        if pValor  = NFE_COFINS_CST_TribMonofasica then
           Result := NFE_COFINS_CST_TribMonofasica + ' - ' + NFE_COFINS_CST_TribMonofasica_Des
        else
        if pValor  = NFE_COFINS_CST_TribAliqZero then
           Result := NFE_COFINS_CST_TribAliqZero + ' - ' + NFE_COFINS_CST_TribAliqZero_Des
        else
        if pValor  = NFE_COFINS_CST_InsetoContrib then
           Result := NFE_COFINS_CST_InsetoContrib + ' - ' + NFE_COFINS_CST_InsetoContrib_Des
        else
        if pValor  = NFE_COFINS_CST_SemIncidContrib then
           Result := NFE_COFINS_CST_SemIncidContrib + ' - ' + NFE_COFINS_CST_SemIncidContrib_Des
        else
        if pValor  = NFE_COFINS_CST_SuspContrib then
           Result := NFE_COFINS_CST_SuspContrib + ' - ' + NFE_COFINS_CST_SuspContrib_Des;
     end
     else
     if pCampo = NFE_ICMS_CST then
     begin
        if pValor  = NFE_ICMS_CST_Tributado then
           Result := NFE_ICMS_CST_Tributado + ' - ' + NFE_ICMS_CST_Tributado_Des;
     end
     else
     if pCampo = NFE_ICMS_Orig then
     begin
        if pValor  = NFE_ICMS_Orig_Nacional then
           Result := NFE_ICMS_Orig_Nacional + ' - ' + NFE_ICMS_Orig_Nacional_Des
        else
        if pValor  = NFE_ICMS_Orig_EstrangImpDir then
           Result := NFE_ICMS_Orig_EstrangImpDir + ' - ' + NFE_ICMS_Orig_EstrangImpDir_Des
        else
        if pValor  = NFE_ICMS_Orig_EstrangMercadoInt then
           Result := NFE_ICMS_Orig_EstrangMercadoInt + ' - ' + NFE_ICMS_Orig_EstrangMercadoInt_Des
     end
     else
     if pCampo = NFE_tpEmis then
     begin
        if pValor  = NFE_tpEmis_Normal then
           Result := NFE_tpEmis_Normal + ' - ' + NFE_tpEmis_Normal_Des
        else
        if pValor  = NFE_tpEmis_ContFS then
           Result := NFE_tpEmis_ContFS + ' - ' + NFE_tpEmis_ContFS_Des
        else
        if pValor  = NFE_tpEmis_ContSCAN then
           Result := NFE_tpEmis_ContSCAN + ' - ' + NFE_tpEmis_ContSCAN_Des
        else
        if pValor  = NFE_tpEmis_ContDPEC then
           Result := NFE_tpEmis_ContDPEC + ' - ' + NFE_tpEmis_ContDPEC_Des
        else
        if pValor  = NFE_tpEmis_ContFSDA then
           Result := NFE_tpEmis_ContFSDA + ' - ' + NFE_tpEmis_ContFSDA_Des
     end
     else
     if pCampo = NFE_finNFe then
     begin
        if pValor  = NFE_finNfe_Normal then
           Result := NFE_tpEmis_Normal + ' - ' + NFE_finNFe_Normal_Des
        else
        if pValor  = NFE_finNFe_Complementar then
           Result := NFE_finNFe_Complementar + ' - ' + NFE_finNFe_Complementar_Des
        else
        if pValor  = NFE_finNFe_Ajuste then
           Result := NFE_finNFe_Ajuste + ' - ' + NFE_finNFe_Ajuste_Des
     end
     else
     if pCampo = NFE_tpNF then
     begin
        if pValor = NFE_tpNF_Entrada then
           Result := NFE_tpNF_Entrada + ' - ' + NFE_tpNF_Entrada_Des
        else
        if pValor = NFE_tpNF_Saida then
           Result := NFE_tpNF_Saida + ' - ' + NFE_tpNF_Saida_Des
     end
     else
     if pCampo = NFE_modFrete then
     begin
        if pValor = NFE_modFrete_Emitente then
           Result := NFE_modFrete_Emitente + ' - ' + NFE_modFrete_Emitente_Des
        else
        if pValor = NFE_modFrete_Dest then
           Result := NFE_modFrete_Dest + ' - ' + NFE_modFrete_Dest_Des;
     end
     else
     if pCampo = NFE_procEmi then
     begin
        if pValor = NFE_procEmi_ComAplicativo then
           Result := NFE_procEmi_ComAplicativo + ' - ' + NFE_procEmi_ComAplicativo_Des
        else
        if pValor = NFE_procEmi_Avulsa_Fisco then
           Result := NFE_procEmi_Avulsa_Fisco + ' - ' + NFE_procEmi_Avulsa_Fisco_Des
        else
        if pValor = NFE_procEmi_Avulsa_Contribuinte_Fisco then
           Result := NFE_procEmi_Avulsa_Contribuinte_Fisco + ' - ' + NFE_procEmi_Avulsa_Contribuinte_Fisco_Des
        else
        if pValor = NFE_procEmi_Contribuinte_Fisco then
           Result := NFE_procEmi_Contribuinte_Fisco + ' - ' + NFE_procEmi_Contribuinte_Fisco_Des
     end
     else
     if pCampo = NFE_enderEmit_Pais then
        Result := pValor
     else
     if pCampo = NFE_enderDest_Pais then
        Result := pValor
     else
     if pCampo = NFE_enderDest_Endereco then
        Result := pValor
     else
     if pCampo = NFE_enderEmit_Endereco then
        Result := pValor
     else
     if pCampo = NFE_enderEmit_Municipio then
        Result := pValor
     else
     if pCampo = NFE_enderDest_Municipio then
        Result := pValor
     else
     if pCampo = NFE_dest_CNPJ then
        Result := pValor
     else
     if pCampo = NFE_emit_CNPJ then
        Result := pValor
     else
     if pCampo = NFE_transp_CNPJCPF then
        Result := pValor
     else
     if pCampo = NFE_dEmi then
     begin
        vDt := Copy(pValor, 9,2);
        vDt := vDt + '/' + Copy(pValor, 6,2);
        vDt := vDt + '/' + Copy(pValor, 1,4);
        if vDt = '//' then
          Result := ''
        else
          Result := vDt;
     end
     else
     if pCampo = NFE_dVenc then
     begin
        vDt := Copy(pValor, 9,2);
        vDt := vDt + '/' + Copy(pValor, 6,2);
        vDt := vDt + '/' + Copy(pValor, 1,4);
        if vDt = '//' then
          Result := ''
        else
          Result := vDt;
     end;
end;

{------------------------------------------------------------------------------}
function LocalizaDescricao(const aTabela, aDescricao: string; const aCampoDescricao: string = 'descricao'): Boolean;
var
     queLoc : TIBOQuery;
     s      : string;
begin
     Result := True;
     if aDescricao = '' then
        Exit;
     Result := False;
     try
        FG.CriaIBOQuery(queLoc, 'select codigo from '+aTabela+
                                ' where upper('+aCampoDescricao+') = ' + QuotedStr(FH.SetaUpper(aDescricao)));
        queLoc.Open;
        Result := queLoc.IsEmpty;
        if not Result then
        begin
             s := FH.DsParaLinha(queLoc, 'codigo');
             Result := FH.PrMsg_CF(aDescricao + ' já está cadastrado para os códigos: '+s+#13#13+'Prosseguir?') = 1;
        end;
     finally
            FH.DestroiMultiObj([queLoc]);
     end;
end;


{------------------------------------------------------------------------------}
function EAD_GeraChave(var pChavePublica, pChavePrivada: string): boolean;
type
    Tgenkkey = function(ChavePublica: String; ChavePrivada: String): Integer; stdcall;
var
    vRet : Integer;
    H    : THandle;
    S    : string;
    F    : Tgenkkey;
begin
     try
        Result := False;
        H      := LoadLibrary(PChar(Trim('sign_bema.dll')));
        if H <= HINSTANCE_ERROR then
        begin
             FH.PrMsg_ER('A DLL "sign_bema" utilizada para gerar as chaves pública e privada não foi localizada.');
             exit;
        end;
        @F := GetProcAddress(H, 'genkkey');
        if @F = nil then
        begin
             FH.PrMsg_ER('A função "genkkey" da DLL "sign_bema" não foi localizada.');
             exit;
        end;
        SetLength(pChavePublica, 256);
        SetLength(pChavePrivada, 256);
        vRet   := F(pChavePublica, pChavePrivada);
        Result := vRet <> 0;
     finally
            FreeLibrary(H);
     end;
end;

{------------------------------------------------------------------------------}
function EAD_Assina(var pNomeArq, pChavePublica, pChavePrivada: string): Boolean;
type
    TgenerateEAD = function(NomeArq, ChavePublica, ChavePrivada, EAD: string; Sign: Integer): Integer; stdcall;
var
    vRet : Integer;
    H    : THandle;
    S    : string;
    F    : TgenerateEAD;
    vEAD : string;
begin
     try
        Result := False;
        H      := LoadLibrary(PChar(Trim('sign_bema.dll')));
        if H <= HINSTANCE_ERROR then
        begin
             FH.PrMsg_ER('A DLL "sign_bema" utilizada para gerar as chaves pública e privada não foi localizada.');
             exit;
        end;
        @F := GetProcAddress(H, 'generateEAD');
        if @F = nil then
        begin
             FH.PrMsg_ER('A função "generateEAD" da DLL "sign_bema" não foi localizada.');
             exit;
        end;
        SetLength(vEAD, 256);
        vRet   := F(pNomeArq, pChavePublica, pChavePrivada, vEAD, 1);
        Result := vRet <> 0;
     finally
            FreeLibrary(H);
     end;
end;

{------------------------------------------------------------------------------}
procedure CarregaFrxImagem(pReport: TfrxReport; const pPictureView: string; pDataSet: TDataSet; const pCampo, pChaveExtensao: string);
begin
     TfrxPictureView(pReport.FindObject(pPictureView)).Picture.Assign(nil);
     FH.LeImagemTab(TfrxPictureView(pReport.FindObject(pPictureView)).Picture,
                    pDataSet, pCampo, FG.LeConfigEstabeStr(pChaveExtensao, 'BMP'));
end;


{------------------------------------------------------------------------------}
function BuildFileList(const Path: string; const Attr: Integer; const List: TStrings): Boolean;
begin
     Result := JclFileUtils.BuildFileList(Path, Attr, List);
end;  


end.
