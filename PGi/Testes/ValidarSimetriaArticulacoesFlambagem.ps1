$ErrorActionPreference = 'Stop'
[void][Reflection.Assembly]::LoadFrom((Join-Path $PSScriptRoot '..\bin\Release\PGi.exe'))
$p = [Runtime.Serialization.FormatterServices]::GetUninitializedObject([PG.TPorticoEspacial])
$p.nBarras = 2; $p.NLinhas = 11
$p.barras = New-Object 'PG.TBarraPortico[]' 3
$p.id = [int[]](0..12); $p.id[12] = 0
$p.glRestrito = New-Object bool[] 13; $p.glRestrito[12] = $true
function Campo($b,$nome,$valor) { [PG.TBarraPortico].GetField($nome).SetValue($b,$valor) }
for($n=1;$n -le 2;$n++) {
    $b = New-Object PG.TBarraPortico; $p.barras[$n] = $b
    foreach($e in @{L=2.0;A1=3.0;Iy1=4.0;Iz1=5.0;E1=200.0;G1=80.0;J1=2.0}.GetEnumerator()) { Campo $b $e.Key ([double]$e.Value) }
    $original=[Runtime.Serialization.FormatterServices]::GetUninitializedObject([PG.TBarraGenerica])
    $dados=New-Object PG.TDadosBarra
    [PG.TBarraGenerica].GetField('Dados').SetValue($original,$dados)
    $secao=[Runtime.Serialization.FormatterServices]::GetUninitializedObject([PG.TSecao])
    [PG.TSecao].GetField('propriedades').SetValue($secao,(New-Object PG.TPropriedades_Secao))
    $dados.secao=$secao
    Campo $b 'barraOriginal' $original
    $r = New-Object 'double[,]' 13,13
    for($i=1;$i -le 12;$i++) { $r[$i,$i]=1 }
    Campo $b 'MatrizRotacao' $r
    Campo $b 'GlGlobal' ([int[]](0..12))
    $e = New-Object PG.Esforcos_Barra(42); $e.Esforcos[1]=30; $e.Esforcos[7]=-30
    $lista = New-Object 'System.Collections.Generic.List[PG.Esforcos_Barra]'; $lista.Add($e)
    Campo $b 'casos_x_esforcos' $lista.PSObject.BaseObject
}
# Duas barras com quatro liberacoes, rotacao obliqua e molas de rigidezes distintas.
for($teste=0;$teste -lt 20;$teste++) {
    $angulo=0.137+0.073*$teste
    $c=[Math]::Cos($angulo); $s=[Math]::Sin($angulo)
    $rot=New-Object 'double[,]' 13,13
    for($bl=0;$bl -lt 4;$bl++) {
        $a=1+3*$bl; $b=$a+1; $d=$a+2
        $rot[$a,$a]=$c; $rot[$a,$b]=-$s
        $rot[$b,$a]=$s; $rot[$b,$b]=$c; $rot[$d,$d]=1
    }
    for($barra=1;$barra -le 2;$barra++) {
        Campo $p.barras[$barra] 'MatrizRotacao' $rot
        foreach($nome in @('articulacao_mz_ini','articulacao_my_ini','articulacao_mz_fin','articulacao_my_fin')) {
            Campo $p.barras[$barra] $nome $true
        }
        foreach($nome in @('KMz_Inicio','KMy_Inicio','KMz_Final','KMy_Final')) {
            $mola=0.0
            if($teste -gt 0) { $mola=[Math]::Pow(10,$teste-10)*($barra+0.23456789) }
            Campo $p.barras[$barra] $nome ([double]$mola)
        }
    }
    $f=New-Object PG.TPorticoEspacial_FlambagemLinear($p,2)
    $f.PrepararReferencia(42,$false); $f.MontarMatrizes()
    if($f.NumeroGrausLiberdade -ne 19) { throw 'Graus internos das articulacoes' }
    foreach($matriz in @($f.MatrizRigidez,$f.MatrizGeometrica)) {
        for($i=0;$i -lt 19;$i++) {
            for($j=0;$j -lt 19;$j++) {
                $v=[alglib]::sparseget($matriz,$i,$j)
                $t=[alglib]::sparseget($matriz,$j,$i)
                if([double]::IsNaN($v) -or [double]::IsInfinity($v) -or $v -ne $t) {
                    throw "Assimetria no teste $teste em [$i,$j]: $v / $t"
                }
            }
        }
    }
}
'PASS: simetria exata de K e Kg com liberacoes MY/MZ nas duas pontas, rotacoes obliquas e molas de zero a 1e9.'