$ErrorActionPreference='Stop'
. (Join-Path $PSScriptRoot 'ValidarLanczosFlambagem.ps1')
$f=New-Object PG.TPorticoEspacial_FlambagemLinear($p,3)
Definir 'NumeroGrausLiberdade' ([int]3)
Definir 'MatrizRigidez' (Matriz @(@(0,0,2),@(1,1,3),@(2,2,5)))
Definir 'MatrizGeometrica' (Matriz @(@(0,0,-2),@(1,1,6),@(2,2,-20)))
$f.PrepararRigidez(); $f.ConstruirBaseLanczos(3,[double[]]@(1,2,3))
$d=$f.DiagonalLanczos.Clone(); $s=$f.SubdiagonalLanczos.Clone()
$f.ResolverProblemaReduzido()
if($f.MultiplicadoresCriticos.Length -ne 2 -or $f.QuantidadePositivaSuficiente) { throw 'Quantidade de positivos' }
if([Math]::Abs($f.MultiplicadoresCriticos[0]-0.25) -gt 1e-12 -or
   [Math]::Abs($f.MultiplicadoresCriticos[1]-1) -gt 1e-12) { throw 'Ordem ou inversao' }
for($m=0;$m -lt 2;$m++) {
    $norma=0
    for($i=0;$i -lt 3;$i++) {
        $yi=$f.AutovetoresReduzidos.GetValue($i,$m)
        $ty=$d[$i]*$yi
        if($i -gt 0) { $ty += $s[$i-1]*$f.AutovetoresReduzidos.GetValue($i-1,$m) }
        if($i -lt 2) { $ty += $s[$i]*$f.AutovetoresReduzidos.GetValue($i+1,$m) }
        if([Math]::Abs($ty-$f.AutovaloresReduzidos[$m]*$yi) -gt 1e-12) { throw 'Residuo reduzido' }
        $norma += $yi*$yi
        if($d[$i] -ne $f.DiagonalLanczos[$i]) { throw 'Alterou T' }
    }
    if([Math]::Abs($norma-1) -gt 1e-12) { throw 'Normalizacao reduzida' }
}
$f.ConstruirBaseLanczos(1,[double[]]@(1,0,0))
if($null -ne $f.MultiplicadoresCriticos) { throw 'Resultados antigos' }
$f.ResolverProblemaReduzido()
if($f.MultiplicadoresCriticos.Length -ne 1 -or [Math]::Abs($f.MultiplicadoresCriticos[0]-1) -gt 1e-12) { throw 'Ordem um' }
# Um subespaco com mu negativo nao fornece multiplicador positivo.
$f.ConstruirBaseLanczos(1,[double[]]@(0,1,0)); $f.ResolverProblemaReduzido()
if($f.MultiplicadoresCriticos.Length -ne 0) { throw 'Aceitou mu negativo' }
Definir 'MatrizGeometrica' (Matriz @(@(0,0,0)))
$f.PrepararRigidez(); $f.ConstruirBaseLanczos(3); $f.ResolverProblemaReduzido()
if($f.MultiplicadoresCriticos.Length -ne 0) { throw 'Inverteu zero' }
'PASS: multiplicadores 0.25 e 1, filtragem de negativos/zero, residuos reduzidos, ordem um e invalidacao.'
