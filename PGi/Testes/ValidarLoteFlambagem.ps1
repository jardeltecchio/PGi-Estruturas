$ErrorActionPreference='Stop'
. (Join-Path $PSScriptRoot 'ValidarGeometricaGlobal.ps1')
# O portico sintetico do teste base nao executa construtor.
$rf=New-Object 'System.Collections.Generic.List[PG.TPorticoEspacial+ResultadoFlambagemReferencia]'
[PG.TPorticoEspacial].GetField('<ResultadosFlambagem>k__BackingField',$flags).SetValue($p,$rf.PSObject.BaseObject)
$p.NLinhas=6
for($i=1;$i -le 12;$i++) { $p.glRestrito[$i]=$i -le 6; $p.id[$i]=[Math]::Max(0,$i-6) }
for($n=1;$n -le 2;$n++) {
    Campo $p.barras[$n] 'SomenteTracao' $false
    Campo $p.barras[$n] 'articulacao_mz_ini' $false
    Campo $p.barras[$n] 'temOffset' $false
    $rot=New-Object 'double[,]' 13,13
    for($i=1;$i -le 12;$i++) { $rot[$i,$i]=1 }
    Campo $p.barras[$n] 'MatrizRotacao' $rot
    foreach($nome in @('casos_x_esforcos','combinacoes_x_esforcos')) {
        $lista=[PG.TBarraPortico].GetField($nome).GetValue($p.barras[$n])
        $lista[0].Esforcos[1]=30; $lista[0].Esforcos[7]=-30
    }
}
$refs=New-Object 'System.Collections.Generic.List[PG.TPorticoEspacial+Deslocamentos_Portico]'
$ausente=New-Object 'PG.TPorticoEspacial+Deslocamentos_Portico'; $ausente.id=999
$valido=New-Object 'PG.TPorticoEspacial+Deslocamentos_Portico'; $valido.id=42
$refs.Add($ausente); $refs.Add($valido)
[PG.TPorticoEspacial].GetField('casos_x_deslocamentos',$flags).SetValue($p,$refs.PSObject.BaseObject)
$p.CalcularFlambagemLinear(1)
if($p.ResultadosFlambagem.Count -ne 3) { throw 'Lote incompleto' }
if($p.ResultadosFlambagem[0].Convergiu -or !$p.ResultadosFlambagem[0].Erro) { throw 'Falha nao registrada' }
if(!$p.ResultadosFlambagem[1].Convergiu -or !$p.ResultadosFlambagem[2].Convergiu) { throw 'Nao continuou apos falha' }
if($p.ResultadosFlambagem[1].EhCombinacao -or !$p.ResultadosFlambagem[2].EhCombinacao) { throw 'Tipos confundidos' }
if($p.ResultadosFlambagem[1].EquacoesNodais[1] -ne -1 -or $p.ResultadosFlambagem[1].EquacoesNodais[7] -ne 0) { throw 'Mapa nodal' }
$p.CalcularFlambagemLinear(1)
if($p.ResultadosFlambagem.Count -ne 3) { throw 'Acumulou resultados antigos' }
'PASS: casos e combinacoes, IDs, falha isolada, continuacao, resultados independentes e mapa nodal.'
