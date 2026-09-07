$ErrorActionPreference = 'Stop'
$bin = Join-Path $PSScriptRoot '..\bin\Release'
[void][Reflection.Assembly]::LoadFrom((Join-Path $bin 'PGi.exe'))
$barra = New-Object PG.TBarraPortico
[PG.TBarraPortico].GetProperty('Visivel').SetValue($barra,$true,$null)
[PG.TBarraPortico].GetField('Rgb').SetValue($barra,[byte[]]@(255,255,255))
$noInicial = [Runtime.Serialization.FormatterServices]::GetUninitializedObject([PG.TNoPortico])
$noFinal = [Runtime.Serialization.FormatterServices]::GetUninitializedObject([PG.TNoPortico])
[PG.TNoPortico].GetField('x').SetValue($noFinal,[double]10)
[PG.TBarraPortico].GetField('pIni').SetValue($barra,$noInicial)
[PG.TBarraPortico].GetField('pFin').SetValue($barra,$noFinal)
[PG.TBarraPortico].GetField('GlGlobal').SetValue($barra,[int[]](0..12))
$id = [int[]](0..12)
$restritos = New-Object bool[] 13
$restritos[1] = $true
$modos = New-Object 'double[,]' 12,2
$modos[0,0] = 100 # Deve ser ignorado: GL restringido.
$modos[6,0] = 1
$modos[7,0] = 2
$modos[8,0] = 3
$modos[6,1] = -2
$preparar = [PG.TBarraPortico].GetMethod('PrepararDesenhoModal')
[void]$preparar.Invoke($barra,[object[]]@($modos.PSObject.BaseObject,$id,$restritos.PSObject.BaseObject,[double[]]@(4,2)))
# A copia da barra deve sobreviver a limpeza/alteracao dos dados do solver.
$modos[6,0] = 99
$metodo = [PG.TBarraPortico].GetMethod('Preenche_Barra_ModoVibracao')
function Desenhar($modo, $colorido) {
    $lista = New-Object 'System.Collections.Generic.List[float]'
    $argumentos = New-Object object[] 5
    $argumentos[0] = $lista.PSObject.BaseObject
    $argumentos[1] = [double]2
    $argumentos[2] = [bool]$colorido
    $argumentos[3] = $true
    $argumentos[4] = [int]$modo
    [void]$metodo.Invoke($barra,$argumentos)
    return ,$lista.ToArray()
}
$v = Desenhar 0 $false
if ($v.Length -ne 12 -or $v[0] -ne 0 -or $v[6] -ne 12 -or $v[7] -ne -6 -or $v[8] -ne -4) {
    throw 'Coordenadas, vinculos ou escala incorretos'
}
$v = Desenhar 1 $false
if ($v[6] -ne 6) { throw 'Selecao do modo incorreta' }
$v = Desenhar 0 $true
if ($v[3] -ne 0 -or $v[5] -ne 1 -or $v[9] -le 0) { throw 'Coloracao incorreta' }
$v = Desenhar -1 $false
if ($v.Length -ne 0) { throw 'Modo invalido desenhado' }
'PASS: coordenadas X/-Z/-Y, escala, vinculos, selecao, cores e persistencia dos vetores.'
