$ErrorActionPreference = 'Stop'
. (Join-Path $PSScriptRoot 'ValidarSolidoFlambagem.ps1')

function LinhaModo($flambagem, $modo, $escala, $colorido, $combinacao = 17) {
    $coords = New-Object 'System.Collections.Generic.List[float]'
    $argumentos = [object[]]@($coords.PSObject.BaseObject,[double]$escala,
        [bool]$colorido,$true,[int]$modo)
    $nome = 'Preenche_Barra_ModoVibracao'
    if ($flambagem) {
        $nome = 'Preenche_Barra_Flambagem'
        $argumentos += [int]$combinacao
    }
    [void][PG.TBarraPortico].GetMethod($nome).Invoke($barra,$argumentos)
    return ,$coords.ToArray()
}

# Inclui o deslocamento do braco rigido produzido pela rotacao nodal.
$offset = [Runtime.Serialization.FormatterServices]::GetUninitializedObject([PG.TNoPortico])
[PG.TNoPortico].GetField('x').SetValue($offset,[double]10)
[PG.TNoPortico].GetField('y').SetValue($offset,[double]2)
[PG.TBarraPortico].GetField('pFin_offset').SetValue($barra,$offset)
foreach ($modo in @(0,1)) {
    foreach ($escala in @(0,2,-2)) {
        foreach ($colorido in @($false,$true)) {
            $modal = LinhaModo $false $modo $escala $colorido
            $flambagem = LinhaModo $true $modo $escala $colorido
            if ($modal.Length -ne 12 -or $flambagem.Length -ne 12) { throw 'Segmento incompleto' }
            for ($v = 0; $v -lt 12; $v++) {
                if ($modal[$v] -ne $flambagem[$v]) { throw "Diferenca do modal em $v" }
            }
        }
    }
}
$r = LinhaModo $true 0 2 $false 42
if ($r[6] -ne 4) { throw 'Combinacao incorreta' }
foreach ($par in @(@(-1,17),@(2,17),@(0,999))) {
    $r = LinhaModo $true $par[0] 2 $true $par[1]
    if ($r.Length -ne 0) { throw 'Selecao invalida desenhada' }
}
foreach ($escala in @([double]::NaN,[double]::PositiveInfinity)) {
    $r = LinhaModo $true 0 $escala $false
    if ($r.Length -ne 0) { throw 'Escala invalida desenhada' }
}
'PASS: barra de flambagem equivalente ao modal, offsets, cores, escalas e IDs de combinacoes.'
