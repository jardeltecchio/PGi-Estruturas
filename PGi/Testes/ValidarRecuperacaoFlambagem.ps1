$ErrorActionPreference='Stop'
. (Join-Path $PSScriptRoot 'ValidarReduzidoFlambagem.ps1')
$f=New-Object PG.TPorticoEspacial_FlambagemLinear($p,2)
Definir 'NumeroGrausLiberdade' ([int]3)
Definir 'MatrizRigidez' (Matriz @(@(0,0,2),@(2,2,5)))
Definir 'MatrizGeometrica' (Matriz @(@(0,0,-2),@(2,2,-20)))
$f.PrepararRigidez(); $f.ConstruirBaseLanczos(2,[double[]]@(1,2))
$f.ResolverProblemaReduzido(); $f.RecuperarModosEVerificarConvergencia(1e-10)
if(!$f.Convergiu) { throw 'Modos exatos nao convergiram' }
for($m=0;$m -lt 2;$m++) {
    if($f.ModosFlambagem.GetValue(1,$m) -ne 0) { throw 'Grau removido nao nulo' }
    $phi=[double[]]@($f.ModosReduzidos.GetValue(0,$m),$f.ModosReduzidos.GetValue(1,$m))
    if([Math]::Abs($f.ProdutoInternoRigidez($phi,$phi)-1) -gt 1e-12) { throw 'Normalizacao K' }
    if($f.ResiduosRelativos[$m] -gt 1e-12) { throw 'Residuo completo' }
}
if([Math]::Abs($f.ModosFlambagem.GetValue(2,0)-1/[Math]::Sqrt(5)) -gt 1e-12 -or
   [Math]::Abs($f.ModosFlambagem.GetValue(0,1)-1/[Math]::Sqrt(2)) -gt 1e-12) { throw 'Modo ou ordem incorretos' }
# Uma base truncada pode resolver T exatamente sem resolver o problema completo.
$f.ConstruirBaseLanczos(1,[double[]]@(1,2)); $f.ResolverProblemaReduzido()
$f.RecuperarModosEVerificarConvergencia(1e-10)
if($f.Convergiu -or $f.ModosConvergidos[0] -or $f.ResiduosRelativos[0] -lt 1e-3) { throw 'Falsa convergencia' }
# Um unico modo exato nao satisfaz a solicitacao de dois modos.
$f.ConstruirBaseLanczos(1,[double[]]@(1,0)); $f.ResolverProblemaReduzido()
$f.RecuperarModosEVerificarConvergencia()
if($f.Convergiu -or !$f.ModosConvergidos[0]) { throw 'Quantidade insuficiente' }
try { $f.RecuperarModosEVerificarConvergencia([double]::NaN); throw 'Aceitou tolerancia NaN' }
catch { if($_.Exception.Message -eq 'Aceitou tolerancia NaN') { throw } }
if($null -ne $f.ModosFlambagem -or $f.Convergiu) { throw 'Resultados antigos apos falha' }
$f.PrepararRigidez()
if($null -ne $f.ResiduosRelativos) { throw 'Nao invalidou recuperacao' }
'PASS: recuperacao expandida, modos conhecidos, normalizacao K, residuos e rejeicao de falsa convergencia.'
