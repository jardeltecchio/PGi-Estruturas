$ErrorActionPreference='Stop'
. (Join-Path $PSScriptRoot 'ValidarRigidezFlambagem.ps1')
# Kg indefinida e um GL desconectado; K reduzida = [4,1;1,3].
Definir 'NumeroGrausLiberdade' ([int]3)
Definir 'MatrizRigidez' $k
Definir 'MatrizGeometrica' (Matriz @(@(0,0,-2),@(2,2,1)))
$f.PrepararRigidez()
$u=[double[]]@(1,2); $v=[double[]]@(3,-1)
$au=$f.AplicarOperadorFlambagem($u)
# -Kg*u=[2,-2]; K^-1*(-Kg*u)=[8/11,-10/11].
if([Math]::Abs($au[0]-8.0/11) -gt 1e-12 -or [Math]::Abs($au[1]+10.0/11) -gt 1e-12) { throw 'Operador ou sinal incorreto' }
if($u[0] -ne 1 -or $u[1] -ne 2) { throw 'Alterou vetor de entrada' }
$av=$f.AplicarOperadorFlambagem($v)
$esq=$f.ProdutoInternoRigidez($u,$av)
$dir=$f.ProdutoInternoRigidez($au,$v)
if([Math]::Abs($esq-$dir) -gt 1e-12 -or [Math]::Abs($esq-8) -gt 1e-12) { throw 'Autoadjuncao na metrica K' }
if([Math]::Abs($f.ProdutoInternoRigidez($u,$u)-20) -gt 1e-12) { throw 'Produto interno K' }
$zero=$f.AplicarOperadorFlambagem([double[]]@(0,0))
if($zero[0] -ne 0 -or $zero[1] -ne 0) { throw 'Operador nulo' }
foreach($invalido in @([double[]]@(1),[double[]]@(1,[double]::NaN))) {
    try { $f.AplicarOperadorFlambagem($invalido); throw 'Aceitou entrada invalida' }
    catch { if($_.Exception.Message -eq 'Aceitou entrada invalida') { throw } }
}
# Kg singular: operador deve aceitar autovalores nulos.
Definir 'MatrizGeometrica' $g
$f.PrepararRigidez()
$zero=$f.AplicarOperadorFlambagem([double[]]@(0,1))
if($zero[0] -ne 0 -or $zero[1] -ne 0) { throw 'Kg singular' }
try { $f.PrepararReferencia(99,$false) } catch { }
try { $f.AplicarOperadorFlambagem($u); throw 'Aceitou fatoracao invalidada' }
catch { if($_.Exception.Message -eq 'Aceitou fatoracao invalidada') { throw } }
'PASS: operador conhecido, Kg indefinida/singular, produto interno K, autoadjuncao e entradas invalidas.'
