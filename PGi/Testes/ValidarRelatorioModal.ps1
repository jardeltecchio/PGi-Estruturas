$ErrorActionPreference = 'Stop'
. (Join-Path $PSScriptRoot 'ValidarConvergenciaModal.ps1')

# Dois GL livres em X (um por nó), com massa consistente acoplada.
# M = [2 1; 1 6], r = [1 1], massa de referência = 10.
# Modos M-ortogonais [1 0] e [-0.5 1]: participações exatas 45% e 55%.
$p = [Runtime.Serialization.FormatterServices]::GetUninitializedObject([PG.TPorticoEspacial])
$p.NLinhas = 2
$p.Ngl = 12
$p.id = [int[]]@(0,1,0,0,0,0,0,2,0,0,0,0,0)
$restritos = New-Object bool[] 13
for ($i=1; $i -le 12; $i++) { $restritos[$i] = $true }
$restritos[1] = $false; $restritos[7] = $false
$p.glRestrito = $restritos
$m = New-Object alglib+sparsematrix
[alglib]::sparsecreate(2,2,[ref]$m)
[alglib]::sparseset($m,0,0,2)
[alglib]::sparseset($m,0,1,1)
[alglib]::sparseset($m,1,0,1)
[alglib]::sparseset($m,1,1,6)
[alglib]::sparseconverttocrs($m)
$modal = New-Object PG.TPorticoEspacialModal($p,2)
Definir $modal 'MatrizMassa' $m
$phi = New-Object 'double[,]' 2,2
$phi[0,0] = 3 # Reescala do primeiro modo: resultado deve continuar 45%.
$phi[0,1] = -0.5; $phi[1,1] = 1
Definir $modal 'ModosVibracao' $phi
Invocar $modal 'CalcularParticipacaoModal'
$percentuais = $modal.PercentuaisMassaModal
$erroPrimeiro = $percentuais.GetValue(0,0) - 45
$erroSegundo = $percentuais.GetValue(1,0) - 55
if ([Math]::Abs($erroPrimeiro) -gt 1e-12 -or [Math]::Abs($erroSegundo) -gt 1e-12) {
    throw 'Massa modal efetiva incorreta'
}
for ($j=0; $j -lt 2; $j++) {
    if ($percentuais[$j,1] -ne 0 -or $percentuais[$j,2] -ne 0) { throw 'Direcao restringida possui participacao' }
}
Definir $p 'PercentuaisMassaModal' $percentuais
Definir $p 'FrequenciasAngulares' ([double[]]@(2,4))
Definir $p 'FrequenciasNaturais' ([double[]]@((1/[Math]::PI),(2/[Math]::PI)))
$relatorio = New-Object PG.TRelatoriosResultados
$texto = $relatorio.RelatorioAnaliseModal($p)
if ($texto -notmatch '45,0000' -or $texto -notmatch '55,0000' -or $texto -notmatch '100,0000') { throw 'Valores ausentes no TXT' }
if (($texto -split "`r?`n" | Where-Object { $_ -match '^\s+[12]\s+\|' }).Count -ne 2) { throw 'Quantidade incorreta de linhas' }
'PASS: massa acoplada, invariancia de escala, direcoes restringidas e relatorio em memoria.'
