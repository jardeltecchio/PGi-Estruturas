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
# Barras independentes biarticuladas, um elemento por barra. Translations fixas;
# somente as quatro rotacoes internas MY/MZ de cada barra permanecem livres.
# EIy=800, EIz=1000, L=2, N=-30: espectro por barra = 80,100,400,500.
# Os cinco menores multiplicadores das 80 barras identicas devem ser todos 80.
$p.nBarras=80; $p.NLinhas=0
$p.barras=New-Object 'PG.TBarraPortico[]' 81
for($gl=1;$gl -le 12;$gl++) { $p.glRestrito[$gl]=$true; $p.id[$gl]=0 }
$clone=[object].GetMethod('MemberwiseClone',[Reflection.BindingFlags]'Instance,NonPublic')
for($numero=1;$numero -le 80;$numero++) {
    $barra=$clone.Invoke($b,$null)
    foreach($nome in @('articulacao_mz_ini','articulacao_my_ini','articulacao_mz_fin','articulacao_my_fin')) { Campo $barra $nome $true }
    foreach($nome in @('KMz_Inicio','KMy_Inicio','KMz_Final','KMy_Final')) { Campo $barra $nome ([double]0) }
    $p.barras[$numero]=$barra
}
$f=New-Object PG.TPorticoEspacial_FlambagemLinear($p,5)
$f.PrepararReferencia(42,$false); $f.MontarMatrizes(); $f.PrepararRigidez()
$f.CalcularModos()
if(!$f.Convergiu -or $f.MultiplicadoresCriticos.Length -ne 5) { throw 'Nao convergiu' }
for($m=0;$m -lt 5;$m++) {
    if([Math]::Abs($f.MultiplicadoresCriticos[$m]-80) -gt 1e-8) { throw 'Perdeu multiplicidade dos menores modos' }
    if($f.ResiduosRelativos[$m] -gt 1e-10) { throw 'Residuo elevado' }
}
Write-Output ('PASS: 80 barras biarticuladas, 320 GL internos, cinco multiplicadores iguais a 80; vetores='+$f.BaseLanczos.Count)