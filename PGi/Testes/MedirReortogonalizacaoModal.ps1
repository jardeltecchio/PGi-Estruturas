# Windows PowerShell: mede o método real, sem interface, em matriz sintética.
$ErrorActionPreference = 'Stop'
$bin = Join-Path $PSScriptRoot '..\bin\Release'
[void][Reflection.Assembly]::LoadFrom((Join-Path $bin 'alglib318gpl_net2.dll'))
[void][Reflection.Assembly]::LoadFrom((Join-Path $bin 'PGi.exe'))
$p = [Runtime.Serialization.FormatterServices]::GetUninitializedObject([PG.TPorticoEspacial])
$p.NLinhas = 2500
$t = New-Object PG.TPorticoEspacialModal($p,5)
$m = New-Object alglib+sparsematrix
[alglib]::sparsecreate(2500,2500,[ref]$m)
# Matriz SPD em banda, com 21 diagonais.
for ($i=0; $i -lt 2500; $i++) {
    [alglib]::sparseset($m,$i,$i,21)
    for ($j=[Math]::Max(0,$i-10); $j -lt $i; $j++) {
        [alglib]::sparseset($m,$i,$j,-1)
        [alglib]::sparseset($m,$j,$i,-1)
    }
}
[alglib]::sparseconverttocrs($m)
$flags = [Reflection.BindingFlags]'NonPublic,Instance'
$t.GetType().GetField('<MatrizMassa>k__BackingField',$flags).SetValue($t,$m)
$metodo = $t.GetType().GetMethod('ReortogonalizarLanczos',$flags)
foreach ($quantidade in @(100,500,1000,2500)) {
    $base = New-Object 'System.Collections.Generic.List[double[]]'
    $massasBase = New-Object 'System.Collections.Generic.List[double[]]'
    for ($i=0; $i -lt $quantidade; $i++) {
        $q = New-Object double[] 2500
        $q[$i] = 1/[Math]::Sqrt(21)
        $base.Add($q)
        $mq = New-Object double[] 2500
        [alglib]::sparsemv($m,$q,[ref]$mq)
        $massasBase.Add($mq)
    }
    $tempos = @()
    for ($r=0; $r -lt 4; $r++) {
        $v = New-Object double[] 2500
        for ($i=0; $i -lt 2500; $i++) { $v[$i] = [Math]::Sin($i+1) }
        $argsMetodo = New-Object object[] 3
        $argsMetodo[0] = $v.PSObject.BaseObject
        $argsMetodo[1] = $base.PSObject.BaseObject
        $argsMetodo[2] = $massasBase.PSObject.BaseObject
        $cronometro = [Diagnostics.Stopwatch]::StartNew()
        [void]$metodo.Invoke($t,$argsMetodo)
        $cronometro.Stop()
        if ($r -gt 0) { $tempos += $cronometro.Elapsed.TotalMilliseconds }
    }
    '{0} vetores: {1:F1} ms por reortogonalizacao; 3 produtos M*v' -f $quantidade, (($tempos | Measure-Object -Average).Average)
}
