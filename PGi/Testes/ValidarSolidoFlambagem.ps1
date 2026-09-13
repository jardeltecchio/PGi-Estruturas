$ErrorActionPreference = 'Stop'
. (Join-Path $PSScriptRoot 'ValidarSolidoModal.ps1')

# IDs esparsos e modos distintos: o desenho deve usar a referencia solicitada.
$modosFlambagem = New-Object 'double[,]' 12,2
for ($gl = 1; $gl -le 12; $gl++) {
    for ($modo = 0; $modo -lt 2; $modo++) {
        $modosFlambagem[($gl-1),$modo] = $dm[$gl,$modo]
    }
}
$maximos = [double[]]@(4,2)
$prepararFlambagem = [PG.TBarraPortico].GetMethod('PrepararDesenhoFlambagem')
[void]$prepararFlambagem.Invoke($barra,[object[]]@($modosFlambagem.PSObject.BaseObject,$id,$restritos.PSObject.BaseObject,$maximos,17))
$modosFlambagem[6,0] = -3
[void]$prepararFlambagem.Invoke($barra,[object[]]@($modosFlambagem.PSObject.BaseObject,$id,$restritos.PSObject.BaseObject,$maximos,42))
$modosFlambagem[6,0] = 999
$maximos[0] = 999

function SolidoFlambagem($modo, $escala, $colorido, $idCombinacao) {
    $coords = New-Object 'System.Collections.Generic.List[float]'
    $selecao = [Activator]::CreateInstance([System.Collections.Generic.List`1].MakeGenericType([PG.TBarraPortico].Assembly.GetType('PG.Triangulo')))
    $argumentos = [object[]]@($coords.PSObject.BaseObject,$selecao.PSObject.BaseObject,
        [double]$escala,$true,$false,[int]$modo,[bool]$colorido,[int]$idCombinacao)
    [void][PG.TBarraPortico].GetMethod('PreencheTriangulos_Flambagem').Invoke($barra,$argumentos)
    return @{Vertices=$coords.ToArray(); Triangulos=$selecao.Count}
}

foreach ($modo in @(0,1)) {
    foreach ($escala in @(0,2,-2)) {
        foreach ($colorido in @($false,$true)) {
            $modal = Solido $modo $escala $colorido
            $flambagem = SolidoFlambagem $modo $escala $colorido 17
            if ($modal.Triangulos -ne $flambagem.Triangulos -or
                $modal.Vertices.Length -ne $flambagem.Vertices.Length) { throw 'Faces diferentes do modal' }
            for ($v = 0; $v -lt $modal.Vertices.Length; $v++) {
                if ($modal.Vertices[$v] -ne $flambagem.Vertices[$v]) { throw "Diferenca do modal no vertice $v" }
            }
        }
    }
}
$r = SolidoFlambagem 0 2 $false 42
if ($r.Vertices[0] -ne 4) { throw 'ID da combinacao ou copia dos modos incorretos' }
foreach ($par in @(@(-1,17),@(2,17),@(0,999))) {
    $r = SolidoFlambagem $par[0] 2 $true $par[1]
    if ($r.Vertices.Length -ne 0) { throw 'Modo ou combinacao inexistente desenhado' }
}
$dados = [PG.TBarraPortico].GetProperty('DeslocamentosFlambagem').GetValue($barra,$null)
if ($dados[17][1,0] -ne 0) { throw 'Apoio nao respeitado' }
'PASS: flambagem equivalente ao modal, IDs esparsos, multiplas combinacoes, copia dos dados e selecoes invalidas.'
