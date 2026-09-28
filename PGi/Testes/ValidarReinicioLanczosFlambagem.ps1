$ErrorActionPreference='Stop'
. (Join-Path $PSScriptRoot 'ValidarRigidezFlambagem.ps1')

function PrepararDiagonal([double[]]$mu, [int]$modos) {
    $script:f=New-Object PG.TPorticoEspacial_FlambagemLinear($p,$modos)
    $ke=$null; $kg=$null
    [alglib]::sparsecreate($mu.Length,$mu.Length,[ref]$ke)
    [alglib]::sparsecreate($mu.Length,$mu.Length,[ref]$kg)
    for($i=0;$i -lt $mu.Length;$i++) {
        $rigidez=$i+1.0
        [alglib]::sparseset($ke,$i,$i,$rigidez)
        [alglib]::sparseset($kg,$i,$i,-$rigidez*$mu[$i])
    }
    [alglib]::sparseconverttocrs($ke); [alglib]::sparseconverttocrs($kg)
    Definir 'NumeroGrausLiberdade' $mu.Length
    Definir 'MatrizRigidez' $ke; Definir 'MatrizGeometrica' $kg
    $f.PrepararRigidez()
}

# Todos os autovalores repetidos: cada recorrencia encerra com um vetor.
PrepararDiagonal @(4,4,4,4,4,4) 3
$f.CalcularModos()
if(!$f.Convergiu -or $f.BaseLanczos.Count -ne 3) { throw 'Nao recuperou multiplicidade' }
foreach($lambda in $f.MultiplicadoresCriticos) {
    if([Math]::Abs($lambda-0.25) -gt 1e-12) { throw 'Multiplicador repetido incorreto' }
}
for($i=0;$i -lt 3;$i++) {
    if($f.ResiduosRelativos[$i] -gt 1e-12) { throw 'Residuo dos modos repetidos' }
    for($j=0;$j -lt 3;$j++) {
        $esperado=0; if($i -eq $j) { $esperado=1 }
        if([Math]::Abs($f.ProdutoInternoRigidez($f.BaseLanczos[$i],$f.BaseLanczos[$j])-$esperado) -gt 1e-12) {
            throw 'Perda de ortogonalidade K no reinicio'
        }
    }
}

# Vetor inicial preso no autoespaco negativo: deve procurar direcoes positivas.
PrepararDiagonal @(-2,4,1) 2
$f.CalcularModos([double[]]@(1,0,0))
if(!$f.Convergiu -or [Math]::Abs($f.MultiplicadoresCriticos[0]-0.25) -gt 1e-12 -or
    [Math]::Abs($f.MultiplicadoresCriticos[1]-1) -gt 1e-12) { throw 'Reinicio no espectro indefinido' }

# Nao inventa modos positivos e respeita limite explicitamente configurado.
PrepararDiagonal @(-2,-2,-2) 2
try { $f.CalcularModos(); throw 'Aceitou espectro negativo' }
catch { if($_.Exception.Message -notmatch 'Espaco completo esgotado') { throw } }
if($f.Convergiu) { throw 'Falsa convergencia' }
PrepararDiagonal @(4,4,4,4) 3
$f.LimiteVetoresLanczos=2
try { $f.CalcularModos(); throw 'Ignorou limite' }
catch { if($_.Exception.Message -notmatch 'Limite de vetores atingido') { throw } }
if($f.Convergiu) { throw 'Aceitou quantidade insuficiente' }
'PASS: reinicio, multiplicidade, ortogonalidade K, espectro indefinido e diagnosticos de limites.'
