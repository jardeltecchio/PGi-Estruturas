$ErrorActionPreference='Stop'
. (Join-Path $PSScriptRoot 'ValidarOperadorFlambagem.ps1')
Definir 'NumeroGrausLiberdade' ([int]3)
Definir 'MatrizRigidez' (Matriz @(@(0,0,2),@(1,1,3),@(2,2,5)))
Definir 'MatrizGeometrica' (Matriz @(@(0,0,-2),@(1,1,6),@(2,2,-20)))
$f.PrepararRigidez()
$inicio=[double[]]@(1,2,3)
$f.ConstruirBaseLanczos(3,$inicio)
if($inicio[1] -ne 2 -or $f.BaseLanczos.Count -ne 3 -or $f.SubdiagonalLanczos.Length -ne 2) { throw 'Dimensoes ou mutacao' }
for($i=0;$i -lt 3;$i++) {
    for($j=0;$j -lt 3;$j++) {
        $qi=$f.BaseLanczos[$i]; $qj=$f.BaseLanczos[$j]
        $esperado=0; if($i -eq $j) { $esperado=1 }
        if([Math]::Abs($f.ProdutoInternoRigidez($qi,$qj)-$esperado) -gt 1e-12) { throw 'Ortogonalidade K' }
        $aqj=$f.AplicarOperadorFlambagem($qj)
        $t=0
        if($i -eq $j) { $t=$f.DiagonalLanczos[$i] }
        if([Math]::Abs($i-$j) -eq 1) { $t=$f.SubdiagonalLanczos[[Math]::Min($i,$j)] }
        if([Math]::Abs($f.ProdutoInternoRigidez($qi,$aqj)-$t) -gt 1e-12) { throw 'Projecao tridiagonal' }
    }
}
# Espectro conhecido do operador: 1,-2,4. Traco=3 e determinante=-8.
$d=$f.DiagonalLanczos; $s=$f.SubdiagonalLanczos
$det=$d[0]*($d[1]*$d[2]-$s[1]*$s[1])-$s[0]*$s[0]*$d[2]
if([Math]::Abs(($d[0]+$d[1]+$d[2])-3) -gt 1e-12 -or [Math]::Abs($det+8) -gt 1e-12) { throw 'Espectro reduzido' }
$f.ConstruirBaseLanczos(1,$inicio)
if($f.BaseLanczos.Count -ne 1 -or $f.SubdiagonalLanczos.Length -ne 0 -or $f.SubespacoLanczosEncerrado) { throw 'Limite de vetores' }
# Inicio em um autovetor: encerramento exato, sem divisao por zero.
$f.ConstruirBaseLanczos(3,[double[]]@(1,0,0))
if($f.BaseLanczos.Count -ne 1 -or !$f.SubespacoLanczosEncerrado) { throw 'Subespaco invariante' }
$f.ConstruirBaseLanczos(3)
if($f.BaseLanczos.Count -ne 3) { throw 'Inicio automatico' }
try { $f.ConstruirBaseLanczos(3,[double[]]@(0,0,0)); throw 'Aceitou vetor nulo' }
catch { if($_.Exception.Message -eq 'Aceitou vetor nulo') { throw } }
if($null -ne $f.BaseLanczos) { throw 'Base antiga apos falha' }
$f.ConstruirBaseLanczos(3,$inicio)
$f.PrepararRigidez()
if($null -ne $f.BaseLanczos) { throw 'Nao invalidou Lanczos' }
'PASS: base K-ortonormal, projecao tridiagonal, espectro conhecido, limite, encerramento e invalidacao.'
