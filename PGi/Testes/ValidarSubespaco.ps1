$ErrorActionPreference='Stop'
$bin=Join-Path $PSScriptRoot '..\bin\Release'
[void][Reflection.Assembly]::LoadFrom((Join-Path $bin 'alglib318gpl_net2.dll'))
[void][Reflection.Assembly]::LoadFrom((Join-Path $bin 'PGi.exe'))
$flags=[Reflection.BindingFlags]'Instance,NonPublic'
function Definir($obj,$nome,$valor) {
    $obj.GetType().GetField('<'+$nome+'>k__BackingField',$flags).SetValue($obj,$valor.PSObject.BaseObject)
}
function Invocar($obj,$nome) { [void]$obj.GetType().GetMethod($nome,$flags).Invoke($obj,$null) }
$p=[Runtime.Serialization.FormatterServices]::GetUninitializedObject([PG.TPorticoEspacial])
$p.NLinhas=40
$p.MatrizRigidez=[Runtime.Serialization.FormatterServices]::GetUninitializedObject([PG.TMatrizBanda])
$k=$null; $m=$null; $g=$null
[alglib]::sparsecreate(40,40,[ref]$k); [alglib]::sparsecreate(40,40,[ref]$m); [alglib]::sparsecreate(40,40,[ref]$g)
for($i=0;$i -lt 40;$i++) {
    $lambda=$i+1; if($i -eq 1) { $lambda=1 }
    [alglib]::sparseset($k,$i,$i,$lambda)
    [alglib]::sparseset($m,$i,$i,1)
    [alglib]::sparseset($g,$i,$i,-1)
}
foreach($mat in @($k,$m,$g)) { [alglib]::sparseconverttocrs($mat) }
$p.MatrizRigidez.s=$k
$modal=New-Object PG.TPorticoEspacialModal($p,3)
Definir $modal 'MatrizMassa' $m
Invocar $modal 'PrepararOperadorShiftInvert'
Invocar $modal 'CalcularPorSubespaco'
if(!$modal.Convergiu) { throw 'Modal nao convergiu' }
for($i=0;$i -lt 3;$i++) {
    $exato=@(1,1,3)[$i]
    if([Math]::Abs($modal.Autovalores[$i]-$exato) -gt 1e-5) { throw 'Autovalor modal' }
}
$ortho=0
for($i=0;$i -lt 40;$i++) { $ortho+=$modal.ModosVibracao.GetValue($i,0)*$modal.ModosVibracao.GetValue($i,1) }
if([Math]::Abs($ortho) -gt 1e-10) { throw 'Modos repetidos nao ortogonais' }
$f=New-Object PG.TPorticoEspacial_FlambagemLinear($p,3)
Definir $f 'NumeroGrausLiberdade' ([int]40)
Definir $f 'MatrizRigidez' $k; Definir $f 'MatrizGeometrica' $g
$f.PrepararRigidez(); $f.CalcularModosPorSubespaco()
if(!$f.Convergiu) { throw 'Flambagem nao convergiu' }
for($i=0;$i -lt 3;$i++) {
    if([Math]::Abs($f.MultiplicadoresCriticos[$i]-@(1,1,3)[$i]) -gt 1e-5) { throw 'Multiplicador' }
}
# Espectro indefinido com negativo dominante: nao perder os positivos desejados.
$g2=$null; [alglib]::sparsecreate(40,40,[ref]$g2)
for($i=0;$i -lt 40;$i++) { $v=-1; if($i -eq 39) { $v=4000 }; [alglib]::sparseset($g2,$i,$i,$v) }
[alglib]::sparseconverttocrs($g2)
Definir $f 'MatrizGeometrica' $g2
$f.LimiteVetoresLanczos=12
$f.PrepararRigidez(); $f.CalcularModosPorSubespaco()
if(!$f.Convergiu -or [Math]::Abs($f.MultiplicadoresCriticos[2]-3) -gt 1e-6) { throw 'Espectro indefinido' }
if($f.BaseLanczos.Count -gt 12) { throw 'Ignorou limite da base' }
foreach($residuo in $f.ResiduosRelativos) { if($residuo -gt 1e-4) { throw 'Aceitou residuo excessivo' } }
$f.LimiteTempoSubespaco=[TimeSpan]::FromTicks(1)
try { $f.CalcularModosPorSubespaco(); throw 'Ignorou tempo' } catch { if($_.Exception.Message -eq 'Ignorou tempo') { throw } }
if($f.Convergiu) { throw 'Estado apos falha' }
$f.LimiteTempoSubespaco=[TimeSpan]::FromMinutes(5); $f.LimiteIteracoesSubespaco=1
Definir $f 'MatrizGeometrica' $g; $f.PrepararRigidez()
try { $f.CalcularModosPorSubespaco(); throw 'Ignorou iteracoes' } catch { if($_.Exception.Message -eq 'Ignorou iteracoes') { throw } }
if($f.Convergiu) { throw 'Falsa convergencia' }
if([PG.TPorticoEspacialModal].GetMethod('Calcular').GetParameters().Length -ne 2) { throw 'Assinatura modal deve receber apenas numero de modos e lanczos' }
'PASS: subespaco modal/flambagem, autovalores repetidos, ortogonalidade, espectro indefinido, tempo, iteracoes e selecao.'
