$ErrorActionPreference='Stop'
. (Join-Path $PSScriptRoot 'ValidarRecuperacaoFlambagem.ps1')
$f=New-Object PG.TPorticoEspacial_FlambagemLinear($p,2)
$ke=$null; $kg=$null
[alglib]::sparsecreate(40,40,[ref]$ke); [alglib]::sparsecreate(40,40,[ref]$kg)
for($i=0;$i -lt 40;$i++) {
    [alglib]::sparseset($ke,$i,$i,1)
    [alglib]::sparseset($kg,$i,$i,-($i+1))
}
[alglib]::sparseconverttocrs($ke); [alglib]::sparseconverttocrs($kg)
Definir 'NumeroGrausLiberdade' ([int]40)
Definir 'MatrizRigidez' $ke; Definir 'MatrizGeometrica' $kg
$f.PrepararRigidez(); $f.CalcularModos()
if(!$f.Convergiu -or $f.VetoresPorTentativa.Count -lt 2) { throw 'Nao ampliou a base' }
if([Math]::Abs($f.MultiplicadoresCriticos[0]-1.0/40) -gt 1e-6 -or
   [Math]::Abs($f.MultiplicadoresCriticos[1]-1.0/39) -gt 1e-6) { throw 'Multiplicadores conhecidos' }
foreach($r in $f.ResiduosRelativos) { if($r -gt 1e-4) { throw 'Tolerancia' } }
$q0=$f.BaseLanczos[0].Clone()
$f.ConstruirBaseLanczos(10)
for($i=0;$i -lt 40;$i++) { if($q0[$i] -ne $f.BaseLanczos[0][$i]) { throw 'Inicio inconsistente' } }
$f.LimiteVetoresLanczos=2
try { $f.CalcularModos(); throw 'Aceitou base insuficiente' } catch { if($_.Exception.Message -eq 'Aceitou base insuficiente') { throw } }
if($f.Convergiu -or $f.VetoresPorTentativa.Count -ne 1) { throw 'Falsa convergencia ou historico acumulado' }
$f.LimiteVetoresLanczos=0; $f.LimiteTempoLanczos=[TimeSpan]::FromTicks(1)
try { $f.CalcularModos(); throw 'Ignorou tempo' } catch { if($_.Exception.Message -eq 'Ignorou tempo') { throw } }
if($f.Convergiu -or !$f.RigidezFatorada) { throw 'Estado apos limite' }
'PASS: ampliacao adaptativa, multiplicadores conhecidos, tolerancia 1e-4 e limites de vetores/tempo.'
