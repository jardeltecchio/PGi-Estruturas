$ErrorActionPreference='Stop'
[void][Reflection.Assembly]::LoadFrom((Join-Path $PSScriptRoot '..\bin\Release\alglib318gpl_net2.dll'))
[void][Reflection.Assembly]::LoadFrom((Join-Path $PSScriptRoot '..\bin\Release\PGi.exe'))
$p=[Runtime.Serialization.FormatterServices]::GetUninitializedObject([PG.TPorticoEspacial])
$f=New-Object PG.TPorticoEspacial_FlambagemLinear($p,1)
function Matriz($valores) {
    if($valores[0] -isnot [array]) { $valores = ,$valores }
    $s=$null
    [alglib]::sparsecreate(3,3,[ref]$s)
    foreach($item in $valores) { [alglib]::sparseset($s,[int]$item[0],[int]$item[1],[double]$item[2]) }
    [alglib]::sparseconverttocrs($s)
    return $s
}
function Definir($nome,$valor) { [PG.TPorticoEspacial_FlambagemLinear].GetProperty($nome).SetValue($f,$valor,$null) }
function Rejeitar {
    try { $f.PrepararRigidez(); throw 'Aceitou matriz invalida' }
    catch { if($_.Exception.Message -eq 'Aceitou matriz invalida') { throw } }
    if($f.RigidezFatorada) { throw 'Fatoracao antiga permaneceu valida' }
}
Definir 'NumeroGrausLiberdade' ([int]3)
$k=Matriz @(@(0,0,4),@(0,2,1),@(2,0,1),@(2,2,3))
$g=Matriz @(@(0,0,-1))
Definir 'MatrizRigidez' $k; Definir 'MatrizGeometrica' $g
$f.PrepararRigidez()
if($f.ExpandidoParaReduzido[1] -ne -1 -or $f.ReduzidoParaExpandido[1] -ne 2) { throw 'Mapa de recuperacao' }
$x=$f.ResolverComRigidez([double[]]@(6,7))
if([Math]::Abs($x[0]-1) -gt 1e-12 -or [Math]::Abs($x[1]-2) -gt 1e-12) { throw 'Solucao conhecida' }
$x=$f.ResolverComRigidez([double[]]@(9,4))
if([Math]::Abs($x[0]-(23.0/11)) -gt 1e-12) { throw 'Reuso da fatoracao' }
if($f.ResiduoValidacaoRigidez -gt 1e-10 -or [alglib]::sparseget($k,0,0) -ne 4) { throw 'Residuo ou alteracao da origem' }
# Uma equacao presente apenas em Kg nao pode ser eliminada.
Definir 'MatrizGeometrica' (Matriz @(@(1,1,-1)))
Rejeitar
Definir 'MatrizGeometrica' $g
Definir 'MatrizRigidez' (Matriz @(@(0,0,1),@(0,2,1),@(2,0,1),@(2,2,1)))
Rejeitar # mecanismo, apesar de diagonais positivas
Definir 'MatrizRigidez' (Matriz @(@(0,0,4),@(0,2,1),@(2,2,3)))
Rejeitar # assimetria
Definir 'MatrizRigidez' (Matriz @(@(0,0,4),@(1,1,1e-8),@(2,2,3)))
$f.PrepararRigidez()
if($f.ReduzidoParaExpandido.Length -ne 3) { throw 'Removeu rigidez pequena' }
try { $f.ResolverComRigidez([double[]]@(1,[double]::NaN,2)); throw 'Aceitou NaN' }
catch { if($_.Exception.Message -eq 'Aceitou NaN') { throw } }
try { $f.PrepararReferencia(99,$false) } catch { }
if($f.RigidezFatorada) { throw 'Referencia nova nao invalidou fatoracao' }
'PASS: mapas, Cholesky, solucao conhecida, reuso, residuo, mecanismo, assimetria, NaN e rigidez residual.'
