$ErrorActionPreference = 'Stop'
. (Join-Path $PSScriptRoot 'ValidarConvergenciaModal.ps1')

# Cadeia de 2500 GL, cinco modos; matriz transformada por D preserva o espectro.
function CriarCadeia($expoente) {
    $p = [Runtime.Serialization.FormatterServices]::GetUninitializedObject([PG.TPorticoEspacial])
    $p.NLinhas = 2500
    $p.MatrizRigidez = [Runtime.Serialization.FormatterServices]::GetUninitializedObject([PG.TMatrizBanda])
    $k = New-Object alglib+sparsematrix
    $m = New-Object alglib+sparsematrix
    [alglib]::sparsecreate(2500,2500,[ref]$k)
    [alglib]::sparsecreate(2500,2500,[ref]$m)
    for ($i=0; $i -lt 2500; $i++) {
        $d = [Math]::Pow(10, $expoente*(2*$i/2499.0-1))
        [alglib]::sparseset($k,$i,$i,(2*$d*$d))
        [alglib]::sparseset($m,$i,$i,($d*$d))
        if ($i -gt 0) {
            $anterior = [Math]::Pow(10, $expoente*(2*($i-1)/2499.0-1))
            [alglib]::sparseset($k,$i,($i-1),(-$d*$anterior))
            [alglib]::sparseset($k,($i-1),$i,(-$d*$anterior))
        }
    }
    [alglib]::sparseconverttocrs($k)
    [alglib]::sparseconverttocrs($m)
    $p.MatrizRigidez.s = $k
    $modal = New-Object PG.TPorticoEspacialModal($p,5)
    Definir $modal 'MatrizMassa' $m
    return $modal
}
foreach ($expoente in @(0,3)) {
    $modal = CriarCadeia $expoente
    $sw = [Diagnostics.Stopwatch]::StartNew()
    Invocar $modal 'PrepararOperadorShiftInvert'
    Invocar $modal 'ConstruirBaseLanczos'
    $sw.Stop()
    if (!$modal.Convergiu) { throw 'Cadeia de 2500 GL nao convergiu' }
    for ($j=0; $j -lt 5; $j++) {
        $exato = 2-2*[Math]::Cos(($j+1)*[Math]::PI/2501)
        if ([Math]::Abs($modal.Autovalores[$j]/$exato-1) -gt 1e-5) { throw 'Erro nos autovalores de referencia' }
    }
    "PASS: 2500 GL, 5 modos, escala=$expoente; tempo=$($sw.Elapsed.TotalSeconds.ToString('F3')) s; bases=$($modal.VetoresPorTentativa -join ','); residuos=$($modal.ResiduosRelativos -join ',')"
}

# Base deliberadamente insuficiente: parar e manter os residuos, sem falso sucesso.
$modal = CriarCadeia 0
$modal.LimiteVetoresLanczos = 5
Invocar $modal 'PrepararOperadorShiftInvert'
$falhou = $false
try { Invocar $modal 'ConstruirBaseLanczos' } catch {
    if ($_.Exception.ToString() -notmatch 'limite de 5 vetores') { throw }
    $falhou = $true
}
if (!$falhou -or $modal.Convergiu -or $modal.VetoresPorTentativa[0] -ne 5) { throw 'Limite de vetores nao funcionou' }

# Limite minimo de tempo: exercitar deterministicamente a parada cooperativa.
$modal.LimiteTempoLanczos = [TimeSpan]::FromTicks(1)
$falhou = $false
try { Invocar $modal 'ConstruirBaseLanczos' } catch {
    if ($_.Exception.ToString() -notmatch 'limite de tempo') { throw }
    $falhou = $true
}
if (!$falhou -or $modal.Convergiu) { throw 'Limite de tempo nao funcionou' }
'PASS: limites de vetores e tempo interrompem sem declarar convergencia.'
