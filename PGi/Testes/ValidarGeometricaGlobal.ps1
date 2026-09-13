$ErrorActionPreference = 'Stop'
[void][Reflection.Assembly]::LoadFrom((Join-Path $PSScriptRoot '..\bin\Release\PGi.exe'))
$p = [Runtime.Serialization.FormatterServices]::GetUninitializedObject([PG.TPorticoEspacial])
$p.nBarras = 2; $p.NLinhas = 11
$p.barras = New-Object 'PG.TBarraPortico[]' 3
$p.id = [int[]](0..12); $p.id[12] = 0
$p.glRestrito = New-Object bool[] 13; $p.glRestrito[12] = $true
function Campo($b,$nome,$valor) { [PG.TBarraPortico].GetField($nome).SetValue($b,$valor) }
for($n=1;$n -le 2;$n++) {
    $b = New-Object PG.TBarraPortico; $p.barras[$n] = $b
    foreach($e in @{L=2.0;A1=3.0;Iy1=4.0;Iz1=5.0;E1=200.0;G1=80.0;J1=2.0}.GetEnumerator()) { Campo $b $e.Key ([double]$e.Value) }
    $original=[Runtime.Serialization.FormatterServices]::GetUninitializedObject([PG.TBarraGenerica])
    $dados=New-Object PG.TDadosBarra
    [PG.TBarraGenerica].GetField('Dados').SetValue($original,$dados)
    $secao=[Runtime.Serialization.FormatterServices]::GetUninitializedObject([PG.TSecao])
    [PG.TSecao].GetField('propriedades').SetValue($secao,(New-Object PG.TPropriedades_Secao))
    $dados.secao=$secao
    Campo $b 'barraOriginal' $original
    $r = New-Object 'double[,]' 13,13
    for($i=1;$i -le 12;$i++) { $r[$i,$i]=1 }
    Campo $b 'MatrizRotacao' $r
    Campo $b 'GlGlobal' ([int[]](0..12))
    $e = New-Object PG.Esforcos_Barra(42); $e.Esforcos[1]=30; $e.Esforcos[7]=-30
    $lista = New-Object 'System.Collections.Generic.List[PG.Esforcos_Barra]'; $lista.Add($e)
    Campo $b 'casos_x_esforcos' $lista.PSObject.BaseObject
}
$f = New-Object PG.TPorticoEspacial_FlambagemLinear($p,2)
$f.PrepararReferencia(42,$false); $f.MontarMatrizGeometrica()
if([alglib]::sparsegetnrows($f.MatrizGeometrica) -ne 11) { throw 'Restricoes' }
for($i=0;$i -lt 11;$i++) {
    for($j=0;$j -lt 11;$j++) {
        $local=[PG.TBarraPortico].GetField('MatrizGeometrica').GetValue($p.barras[1])
        $esperado=2*$local.GetValue($i+1,$j+1)
        if([Math]::Abs([alglib]::sparseget($f.MatrizGeometrica,$i,$j)-$esperado) -gt 1e-10) { throw 'Superposicao/mapeamento' }
    }
}
$rigida=$f.MatrizRigidez
Campo $p.barras[1] 'articulacao_mz_ini' $true
Campo $p.barras[1] 'KMz_Inicio' ([double]100)
$f.MontarMatrizes()
if($f.NumeroGrausLiberdade -ne 12 -or $f.GrausInternosBarras.GetValue(1,0) -ne 11) { throw 'Grau interno' }
# EIy=800, L=2: K55=1600. Mola liga GL5 ao interno sem entrar em Kg.
if([alglib]::sparseget($f.MatrizRigidez,11,11) -ne 1700 -or
   [alglib]::sparseget($f.MatrizRigidez,4,11) -ne -100 -or
   [alglib]::sparseget($f.MatrizGeometrica,11,11) -ne -8 -or
   [alglib]::sparseget($f.MatrizGeometrica,4,11) -ne 0) { throw 'Mola ou matriz geometrica interna' }
# Condensacao independente de um grau: a rigidez nodal da mola e k*K55/(k+K55),
# somada a contribuicao da segunda barra rigidamente conectada.
$condensada=[alglib]::sparseget($f.MatrizRigidez,4,4)-10000.0/1700
if([Math]::Abs($condensada-(1600+100*1600.0/1700)) -gt 1e-10) { throw 'Equivalencia estatica' }
Campo $p.barras[1] 'KMz_Inicio' ([double]0)
$f.MontarMatrizes()
if([alglib]::sparseget($f.MatrizRigidez,4,11) -ne 0 -or
   [alglib]::sparseget($f.MatrizRigidez,11,11) -ne 1600) { throw 'Articulacao perfeita' }
# Rotacao local de 90 graus: a mola ry conecta agora a rotacao global x.
Campo $p.barras[1] 'KMz_Inicio' ([double]100)
$rot=New-Object 'double[,]' 13,13
for($bl=0;$bl -lt 4;$bl++) {
    $a=1+3*$bl; $c=$a+1; $d=$a+2
    $rot[$a,$c]=1; $rot[$c,$a]=-1; $rot[$d,$d]=1
}
Campo $p.barras[1] 'MatrizRotacao' $rot
$offset=New-Object 'double[,]' 13,13
for($i=1;$i -le 12;$i++) { $offset[$i,$i]=1 }
$offset[1,6]=0.25; $offset[7,12]=0.25
Campo $p.barras[1] 'MatrizOffset' $offset
Campo $p.barras[1] 'temOffset' $true
$sentinela=New-Object 'double[,]' 13,13; $sentinela[1,1]=12345
Campo $p.barras[1] 'MatrizLocal' $sentinela
$f.MontarMatrizes()
if([alglib]::sparseget($f.MatrizRigidez,3,11) -ne 100) { throw 'Rotacao da mola' }
# Kg axial=-15: offset 0.25 adiciona -15*0.25^2 ao GL6 (antes -8 por barra).
if([Math]::Abs([alglib]::sparseget($f.MatrizGeometrica,5,5)-(-16-15*0.25*0.25)) -gt 1e-10) { throw 'Offset geometrico' }
if(-not [object]::ReferenceEquals($sentinela,[PG.TBarraPortico].GetField('MatrizLocal').GetValue($p.barras[1]))) { throw 'Alterou matriz estatica' }
Campo $p.barras[1] 'SomenteTracao' $true
try { $f.MontarMatrizes(); throw 'Aceitou tirante' } catch { if($_.Exception.Message -eq 'Aceitou tirante') { throw } }
if($null -ne $f.MatrizRigidez -or $null -ne $f.MatrizGeometrica) { throw 'Matriz antiga apos falha' }
# Estados salvos por ID, com fator atual propositalmente diferente.
$flags=[Reflection.BindingFlags]'Instance,NonPublic'
$estados=New-Object 'System.Collections.Generic.List[PG.TPorticoEspacial+Deslocamentos_Portico]'
$estado=New-Object 'PG.TPorticoEspacial+Deslocamentos_Portico'
$estado.id=42; $estado.fatoresRigidezTensionOnly=[double[]]@(0,1e-8,1)
$estados.Add($estado)
[PG.TPorticoEspacial].GetField('casos_x_deslocamentos',$flags).SetValue($p,$estados.PSObject.BaseObject)
Campo $p.barras[1] 'FatorRigidezTensionOnly' ([double]1)
$f.PrepararReferencia(42,$false)
# Snapshot nao deve mudar se o armazenamento da estatica for alterado depois.
$estado.fatoresRigidezTensionOnly[1]=1
$f.MontarMatrizes()
if([Math]::Abs([alglib]::sparseget($f.MatrizRigidez,11,11)-(100+1600e-8)) -gt 1e-10) { throw 'Rigidez residual da referencia' }
if([alglib]::sparseget($f.MatrizGeometrica,11,11) -ne 0) { throw 'Tirante inativo com Kg' }
if([PG.TBarraPortico].GetField('FatorRigidezTensionOnly').GetValue($p.barras[1]) -ne 1) { throw 'Alterou estado estatico' }
$lista=[PG.TBarraPortico].GetField('casos_x_esforcos').GetValue($p.barras[1])
$lista[0].Esforcos[1]=-30; $lista[0].Esforcos[7]=30
Campo $p.barras[1] 'FatorRigidezTensionOnly' ([double]1e-8)
$f.PrepararReferencia(42,$false); $f.MontarMatrizes()
if([alglib]::sparseget($f.MatrizRigidez,11,11) -ne 1700 -or
   [alglib]::sparseget($f.MatrizGeometrica,11,11) -ne 8) { throw 'Tirante ativo da referencia' }
# Estado ativo com pequeno esforco negativo nao gera flambagem por compressao.
$lista[0].Esforcos[1]=1e-10; $lista[0].Esforcos[7]=-1e-10
$f.PrepararReferencia(42,$false); $f.MontarMatrizes()
if([alglib]::sparseget($f.MatrizGeometrica,11,11) -ne 0) { throw 'Compressao no tirante ativo' }
$estado.fatoresRigidezTensionOnly[1]=[double]::NaN
try { $f.PrepararReferencia(42,$false); throw 'Aceitou fator invalido' } catch { if($_.Exception.Message -eq 'Aceitou fator invalido') { throw } }
if($f.ReferenciaPreparada -or $null -ne $f.MatrizRigidez) { throw 'Estado antigo apos referencia invalida' }
# Mesmo ID em caso e combinacao deve consultar armazenamentos independentes.
$comb=New-Object 'System.Collections.Generic.List[PG.TPorticoEspacial+Deslocamentos_Portico]'
$estadoComb=New-Object 'PG.TPorticoEspacial+Deslocamentos_Portico'
$estadoComb.id=42; $estadoComb.fatoresRigidezTensionOnly=[double[]]@(0,1,1)
$comb.Add($estadoComb)
[PG.TPorticoEspacial].GetField('combinacoes_x_deslocamentos',$flags).SetValue($p,$comb.PSObject.BaseObject)
for($n=1;$n -le 2;$n++) {
    $res=New-Object PG.Esforcos_Barra(42); $res.Esforcos[1]=-30; $res.Esforcos[7]=30
    $lc=New-Object 'System.Collections.Generic.List[PG.Esforcos_Barra]'; $lc.Add($res)
    Campo $p.barras[$n] 'combinacoes_x_esforcos' $lc.PSObject.BaseObject
}
$f.PrepararReferencia(42,$true); $f.MontarMatrizes()
if([alglib]::sparseget($f.MatrizGeometrica,11,11) -ne 8) { throw 'Confundiu caso e combinacao' }
'PASS: tirantes ativos/inativos, snapshot por referencia, sinais, fator residual e preservacao da estatica.'
'PASS: montagem, restricoes, graus internos, mola, articulacao, equivalencia estatica e invalidacao.'
