# Executar no Windows PowerShell após compilar Release.
$ErrorActionPreference = 'Stop'
$bin = Join-Path $PSScriptRoot '..\bin\Release'
[void][Reflection.Assembly]::LoadFrom((Join-Path $bin 'alglib318gpl_net2.dll'))
[void][Reflection.Assembly]::LoadFrom((Join-Path $bin 'PGi.exe'))
$flags = [Reflection.BindingFlags]'NonPublic,Instance'
function Definir($obj, $nome, $valor) {
    $obj.GetType().GetField('<'+$nome+'>k__BackingField', $flags).SetValue($obj, $valor.PSObject.BaseObject)
}
function Invocar($obj, $nome) {
    $metodo = $obj.GetType().GetMethod($nome, $flags)
    if ($nome -eq 'VerificarConvergencia') { $metodo.Invoke($obj, @($true)) }
    else { $metodo.Invoke($obj, $null) }
}
foreach ($escala in @(1.0, 1e-150, 1e150)) {
    $p = [Runtime.Serialization.FormatterServices]::GetUninitializedObject([PG.TPorticoEspacial])
    $p.NLinhas = 2
    $p.MatrizRigidez = [Runtime.Serialization.FormatterServices]::GetUninitializedObject([PG.TMatrizBanda])
    $k = New-Object alglib+sparsematrix
    $m = New-Object alglib+sparsematrix
    [alglib]::sparsecreate(2,2,[ref]$k)
    [alglib]::sparsecreate(2,2,[ref]$m)
    [alglib]::sparseset($k,0,0,8*$escala)
    [alglib]::sparseset($k,1,1,128*$escala)
    [alglib]::sparseset($m,0,0,2*$escala)
    [alglib]::sparseset($m,1,1,8*$escala)
    [alglib]::sparseconverttocrs($k)
    [alglib]::sparseconverttocrs($m)
    $p.MatrizRigidez.s = $k
    $t = New-Object PG.TPorticoEspacialModal($p,2)
    Definir $t 'MatrizMassa' $m
    # Solução analítica: lambda = 4 e 16; modos normalizados pela massa.
    $phi = New-Object 'double[,]' 2,2
    $phi[0,0] = 1/[Math]::Sqrt(2*$escala)
    $phi[1,1] = 1/[Math]::Sqrt(8*$escala)
    Definir $t 'ModosVibracao' $phi
    Definir $t 'Autovalores' ([double[]]@(4,16))
    Invocar $t 'VerificarConvergencia'
    if (!$t.Convergiu -or ($t.ResiduosRelativos | Where-Object { $_ -gt 1e-14 })) { throw 'Solução exata rejeitada' }

    # Um autovalor errado precisa reprovar somente o segundo modo.
    $t.Autovalores[1] = 17
    $rejeitado = $false
    try { Invocar $t 'VerificarConvergencia' } catch {
        if ($_.Exception.ToString() -notmatch 'modo 2') { throw }
        $rejeitado = $true
    }
    if (!$rejeitado -or $t.Convergiu -or !$t.ModosConvergidos[0] -or $t.ModosConvergidos[1]) { throw 'Não convergência não detectada' }
    if ([Math]::Abs($t.ResiduosRelativos[1] - 1.0/33) -gt 1e-14) { throw 'Resíduo relativo incorreto' }
    $t.Autovalores[1] = 16
    Invocar $t 'VerificarConvergencia'
    if (!$t.Convergiu) { throw 'Estado de convergência não atualizado' }
}
'PASS: solução exata, modo incorreto, resíduo analítico, escalas extremas e atualização de estado.'

# Cadeia de 40 GL: M = I, K tridiagonal (2, -1, -1).
# Autovalores exatos: 2 - 2*cos(j*pi/41). Exercita todo o solver modal.
$p = [Runtime.Serialization.FormatterServices]::GetUninitializedObject([PG.TPorticoEspacial])
$p.NLinhas = 40
$p.MatrizRigidez = [Runtime.Serialization.FormatterServices]::GetUninitializedObject([PG.TMatrizBanda])
$k = New-Object alglib+sparsematrix
$m = New-Object alglib+sparsematrix
[alglib]::sparsecreate(40,40,[ref]$k)
[alglib]::sparsecreate(40,40,[ref]$m)
for ($i=0; $i -lt 40; $i++) {
    [alglib]::sparseset($k,$i,$i,2)
    [alglib]::sparseset($m,$i,$i,1)
    if ($i -gt 0) {
        [alglib]::sparseset($k,$i,($i-1),-1)
        [alglib]::sparseset($k,($i-1),$i,-1)
    }
}
[alglib]::sparseconverttocrs($k)
[alglib]::sparseconverttocrs($m)
$p.MatrizRigidez.s = $k
$t = New-Object PG.TPorticoEspacialModal($p,3)
Definir $t 'MatrizMassa' $m
Invocar $t 'PrepararOperadorShiftInvert'
Invocar $t 'ConstruirBaseLanczos'
if (!$t.Convergiu) { throw 'Cadeia não convergiu' }
for ($j=0; $j -lt 3; $j++) {
    $exato = 2-2*[Math]::Cos(($j+1)*[Math]::PI/41)
    if ([Math]::Abs($t.Autovalores[$j]/$exato-1) -gt 1e-6) { throw 'Autovalor da cadeia incorreto' }
}
"PASS: cadeia 40 GL, base ampliada para $($t.BaseLanczos.Count); resíduos: $($t.ResiduosRelativos -join ', ')."

# Mesma cadeia com mudança de coordenadas: M = D² e K = D*Kcadeia*D.
# Mantém os autovalores exatos, com massas variando por 12 ordens de grandeza.
[alglib]::sparsecreate(40,40,[ref]$k)
[alglib]::sparsecreate(40,40,[ref]$m)
for ($i=0; $i -lt 40; $i++) {
    $d = [Math]::Pow(10, -3+6*$i/39.0)
    [alglib]::sparseset($m,$i,$i,($d*$d))
    [alglib]::sparseset($k,$i,$i,(2*$d*$d))
    if ($i -gt 0) {
        $anterior = [Math]::Pow(10, -3+6*($i-1)/39.0)
        [alglib]::sparseset($k,$i,($i-1),(-$d*$anterior))
        [alglib]::sparseset($k,($i-1),$i,(-$d*$anterior))
    }
}
[alglib]::sparseconverttocrs($k)
[alglib]::sparseconverttocrs($m)
$p.MatrizRigidez.s = $k
$t = New-Object PG.TPorticoEspacialModal($p,3)
Definir $t 'MatrizMassa' $m
Invocar $t 'PrepararOperadorShiftInvert'
Invocar $t 'ConstruirBaseLanczos'
if (!$t.Convergiu) { throw 'Cadeia com massas distintas não convergiu' }
for ($j=0; $j -lt 3; $j++) {
    $exato = 2-2*[Math]::Cos(($j+1)*[Math]::PI/41)
    if ([Math]::Abs($t.Autovalores[$j]/$exato-1) -gt 1e-6) { throw 'Autovalor incorreto com massas distintas' }
}
"PASS: massas entre 1e-6 e 1e6; resíduos: $($t.ResiduosRelativos -join ', ')."

if ($t.AutovaloresReduzidos.Length -ne 3 -or $t.AutovetoresReduzidos.GetLength(1) -ne 3) {
    throw 'Solver reduzido calculou quantidade incorreta de pares'
}
if ($t.ResiduosPorTentativa.Count -ne $t.VetoresPorTentativa.Count -or $t.ResiduosPorTentativa.Count -lt 1) {
    throw 'Histórico de convergência incompleto'
}
for ($j=0; $j -lt 3; $j++) {
    $phi = New-Object double[] 40
    for ($i=0; $i -lt 40; $i++) { $phi[$i] = $t.ModosVibracao[$i,$j] }
    $mphi = New-Object double[] 40
    [alglib]::sparsemv($m,$phi,[ref]$mphi)
    for ($outro=0; $outro -lt 3; $outro++) {
        $produto = 0.0
        for ($i=0; $i -lt 40; $i++) { $produto += $t.ModosVibracao[$i,$outro]*$mphi[$i] }
        $esperado = 0.0
        if ($outro -eq $j) { $esperado = 1.0 }
        if ([Math]::Abs($produto-$esperado) -gt 1e-10) { throw 'Perda de ortogonalidade modal' }
    }
}
# Índices selecionados devem lidar também com autovalores repetidos.
$seletivo = New-Object PG.TPorticoEspacialModal($p,2)
Definir $seletivo 'DiagonalLanczos' ([double[]]@(1,3,3))
Definir $seletivo 'SubdiagonalLanczos' ([double[]]@(0,0))
Invocar $seletivo 'ResolverProblemaReduzido'
for ($j=0; $j -lt 2; $j++) {
    if ([Math]::Abs($seletivo.AutovaloresReduzidos[$j]-3) -gt 1e-12) { throw 'Seleção incorreta de autovalores repetidos' }
    for ($outro=0; $outro -lt 2; $outro++) {
        $produto = 0.0
        for ($i=0; $i -lt 3; $i++) { $produto += $seletivo.AutovetoresReduzidos[$i,$j]*$seletivo.AutovetoresReduzidos[$i,$outro] }
        $esperado = 0.0
        if ($outro -eq $j) { $esperado = 1.0 }
        if ([Math]::Abs($produto-$esperado) -gt 1e-12) { throw 'Autovetores repetidos não ortonormais' }
    }
}
'PASS: seleção parcial, autovalores repetidos, ortogonalidade na massa e histórico de tentativas.'
