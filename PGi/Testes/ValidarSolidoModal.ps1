$ErrorActionPreference = 'Stop'
. (Join-Path $PSScriptRoot 'ValidarDesenhoModal.ps1')
$si = New-Object 'System.Collections.Generic.List[PG.vec3[]]'
$sf = New-Object 'System.Collections.Generic.List[PG.vec3[]]'
$pi = [PG.vec3[]]@((New-Object PG.vec3(0,1,1)),(New-Object PG.vec3(0,-1,1)),(New-Object PG.vec3(0,-1,-1)),(New-Object PG.vec3(0,1,-1)))
$pf = [PG.vec3[]]@((New-Object PG.vec3(10,1,1)),(New-Object PG.vec3(10,-1,1)),(New-Object PG.vec3(10,-1,-1)),(New-Object PG.vec3(10,1,-1)))
$si.Add($pi); $sf.Add($pf)
[PG.TBarraPortico].GetField('coordssecao_i').SetValue($barra,$si.PSObject.BaseObject)
[PG.TBarraPortico].GetField('coordssecao_f').SetValue($barra,$sf.PSObject.BaseObject)
function Solido($modo,$escala,$colorido) {
    $coords = New-Object 'System.Collections.Generic.List[float]'
    $selecao = New-Object 'System.Collections.Generic.List[PG.Triangulo]'
    $argsModal = New-Object object[] 7
    $argsModal[0]=$coords.PSObject.BaseObject
    $argsModal[1]=$selecao.PSObject.BaseObject
    $argsModal[2]=[double]$escala
    $argsModal[3]=$true; $argsModal[4]=$false
    $argsModal[5]=[int]$modo; $argsModal[6]=[bool]$colorido
    [void][PG.TBarraPortico].GetMethod('PreencheTriangulos_ModoVibracao').Invoke($barra,$argsModal)
    return @{Vertices=$coords.ToArray(); Triangulos=$selecao.Count}
}
$r=Solido 0 2 $false
if ($r.Triangulos -ne 8 -or $r.Vertices.Length -ne 264) { throw 'Faces/stride incorretos' }
if ($r.Vertices[0] -ne 12 -or $r.Vertices[1] -ne -7 -or $r.Vertices[2] -ne -5) { throw 'Translacao incorreta' }
for($i=0;$i -lt $r.Vertices.Length;$i+=11) {
    $norma=0.0
    for($j=3;$j -lt 6;$j++) { $norma+=$r.Vertices[$i+$j]*$r.Vertices[$i+$j] }
    if ([Math]::Abs($norma-1) -gt 1e-6) { throw 'Normal incorreta' }
}
$r=Solido 0 0 $false
if ($r.Vertices[0] -ne 10 -or $r.Vertices[1] -ne -1 -or $r.Vertices[2] -ne -1) { throw 'Acumulo entre quadros' }
$r=Solido 1 2 $true
if ($r.Vertices[0] -ne 6 -or $r.Vertices[6] -ne 1 -or $r.Vertices[8] -ne 0) { throw 'Modo/cor incorretos' }
$dm=[PG.TBarraPortico].GetProperty('DeslocamentosModais').GetValue($barra,$null)
$dm[10,0]=0.5
$r=Solido 0 2 $false
if ($r.Vertices[1] -ne -8 -or $r.Vertices[2] -ne -4) { throw 'Rotacao nodal incorreta' }
if ($pf[0].y -ne 1 -or $pf[0].z -ne 1) { throw 'Geometria original modificada' }
$r=Solido -1 2 $false
if ($r.Vertices.Length -ne 0) { throw 'Modo invalido desenhado' }
'PASS: faces, selecao, normais, translacoes, rotacoes, cores, modo e quadros sem acumulo.'
