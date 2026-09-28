param([switch]$Eliminacao)
$ErrorActionPreference='Stop'
$bin=Join-Path $PSScriptRoot '../bin/Release'
Add-Type 'public static class DependenciasMPC { public static void Registrar(string pasta) { System.AppDomain.CurrentDomain.AssemblyResolve += (s,e) => { string p=System.IO.Path.Combine(pasta,new System.Reflection.AssemblyName(e.Name).Name+".dll"); return System.IO.File.Exists(p) ? System.Reflection.Assembly.LoadFrom(p) : null; }; } }'
[DependenciasMPC]::Registrar([IO.Path]::GetFullPath($bin))
foreach($dll in @('alglib318gpl_net2.dll','OpenTK.dll','PGi.exe')) {
    [void][Reflection.Assembly]::LoadFrom((Join-Path $bin $dll))
}
$flags=[Reflection.BindingFlags]'Instance,Public,NonPublic'
Add-Type 'public static class CampoMPC { public static void Set(object o,string n,object v) { o.GetType().GetField(n,System.Reflection.BindingFlags.Instance|System.Reflection.BindingFlags.Public|System.Reflection.BindingFlags.NonPublic).SetValue(o,v); } }'
function Novo($tipo) { [Runtime.Serialization.FormatterServices]::GetUninitializedObject($tipo) }
function Campo($obj,$nome,$valor) {
    [CampoMPC]::Set($obj,$nome,$valor)
}
function Auto($obj,$nome,$valor) { Campo $obj ('<'+$nome+'>k__BackingField') $valor }
function Chamar($obj,$nome,[object[]]$argumentos=@()) {
    for($i=0;$i -lt $argumentos.Length;$i++) { if($null -ne $argumentos[$i]) { $argumentos[$i]=$argumentos[$i].PSObject.BaseObject } }
    try { $resultado=$obj.GetType().GetMethod($nome,$flags).Invoke($obj,$argumentos) }
    catch { throw ($nome + ': ' + $_.Exception.InnerException.ToString()) }
    if($resultado -is [Array]) { return ,$resultado }
    return $resultado
}
function Exigir($ok,$texto) { if(!$ok) { throw $texto } }
$p=Novo ([PG.TPorticoEspacial])
$p.nNos=3; $p.nBarras=0
$p.nos=New-Object 'PG.TNoPortico[]' 4
$coords=@(@(0,0,0),@(2,0,2),@(0,2,2))
$d=New-Object PG.TDiafragmaRigido
$contorno=New-Object 'System.Collections.Generic.List[PG.TPonto]'
Campo $d 'pontosContorno' $contorno
for($i=1;$i -le 3;$i++) {
    $xyz=$coords[$i-1]
    $no=New-Object PG.TNoPortico($xyz[0],$xyz[1],$xyz[2]); Campo $no 'Numero' ([int]$i)
    Campo $no 'GlGlobal' ([int[]]@(0,(6*$i-5),(6*$i-4),(6*$i-3),(6*$i-2),(6*$i-1),(6*$i)))
    $p.nos[$i]=$no
    $ponto=Novo ([PG.TPonto]); Campo $ponto 'x' ([double]$xyz[0]); Campo $ponto 'y' ([double]$xyz[1]); Campo $ponto 'z' ([double]$xyz[2])
    $contorno.Add($ponto)
}
$p.gerenciador=Novo ([PG.Gerenciador])
$p.gerenciador.ConfiguracoesPGi=Novo ([PG.TConfiguracoesPGi])
$p.gerenciador.ConfiguracoesPGi.CfgProjeto=Novo ([PG.SConfiguracoesProjeto])
$p.gerenciador.ConfiguracoesPGi.CfgProjeto.sistema=New-Object PG.SSistema($true)
Exigir ($p.gerenciador.ConfiguracoesPGi.CfgProjeto.sistema.MetodoMPC -eq [PG.MetodoVinculosMPC]::Eliminacao) 'Padrao deve ser eliminacao'
if(!$Eliminacao) { $p.gerenciador.ConfiguracoesPGi.CfgProjeto.sistema.MetodoMPC=[PG.MetodoVinculosMPC]::Penalidade }
$p.gerenciador.formDesenho=Novo ([PG.FPrincipal])
$p.gerenciador.formDesenho.Estrutura=Novo ([PG.TEstrutura])
$lista=New-Object 'System.Collections.Generic.List[PG.TDiafragmaRigido]'; $lista.Add($d)
$p.gerenciador.formDesenho.Estrutura.diafragmasRigidos=$lista
Campo $p 'bracosRigidos' (New-Object 'System.Collections.Generic.List[PG.TBracoRigido]')
Chamar $p 'PrepararPenalidadesMPC'
Exigir (($p.GetType().GetField('numeroGrausAuxiliaresMPC',$flags).GetValue($p)) -eq 3) 'Graus auxiliares'
$p.Ngl=21; $p.NLinhas=21; $p.id=[int[]](0..21); $p.glRestrito=New-Object bool[] 22; $p.LarguraBanda=21
Chamar $p 'PrepararEliminacaoMPC'
$k=$null; $m=$null; $g=$null
foreach($nome in @('k','m','g')) { $mat=$null; [alglib]::sparsecreate(21,21,[ref]$mat); Set-Variable $nome $mat }
$banda=New-Object double[] (22*22)
for($i=0;$i -lt 18;$i++) {
    $valor=10; if(($i%6) -ge 3){$valor=20}
    [alglib]::sparseset($k,$i,$i,$valor); $banda[($i+1)*21+1]=$valor
    [alglib]::sparseset($m,$i,$i,1); [alglib]::sparseset($g,$i,$i,-1)
}
Chamar $p 'AplicarPenalidadesMPC' @($k,$null)
Chamar $p 'AplicarPenalidadesMPC' @($null,$banda)
for($i=0;$i -lt 21;$i++) { for($j=$i;$j -lt 21;$j++) {
    $a=[alglib]::sparseget($k,$i,$j)
    Exigir ([Math]::Abs($a-$banda[($i+1)*21+$j-$i+1]) -lt 1e-5) 'Diferenca banda/esparsa'
    Exigir ([Math]::Abs($a-[alglib]::sparseget($k,$j,$i)) -lt 1e-5) 'Assimetria'
}}
foreach($mat in @($k,$m,$g)) { [alglib]::sparseconverttocrs($mat) }
$f=New-Object double[] 21; $f[0]=1
$u=$null; $rep=$null
$massaOriginal=$m
if($Eliminacao) {
    $k=Chamar $p 'ReduzirMatrizMPC' @($k)
    $m=Chamar $p 'ReduzirMatrizMPC' @($m)
    $g=Chamar $p 'ReduzirMatrizMPC' @($g)
    Exigir ([alglib]::sparsegetnrows($k) -eq 15) 'Eliminacao deve reduzir 21 para 15 equacoes'
    $p.MatrizRigidez=Novo ([PG.TMatrizBanda]); $p.MatrizRigidez.s=$k
    $cargas=New-Object double[] 22; $cargas[1]=1
    $argumentos=[object[]]@($cargas.PSObject.BaseObject,$null)
    [void]$p.GetType().GetMethod('ResolverEliminacaoMPC',$flags).Invoke($p,$argumentos)
    $u=$argumentos[1][1..21]
} else {
[alglib]::sparsespdsolvesks($k,$false,$f,[ref]$u,[ref]$rep)
Exigir ($rep.terminationtype -gt 0) 'Solucao estatica'
}
$restricoes=$p.GetType().GetField('restricoesMPC',$flags).GetValue($p)
foreach($c in $restricoes) {
    $residuo=0; foreach($t in $c.GetEnumerator()) { $residuo+=$t.Value*$u[$t.Key-1] }
    Exigir ([Math]::Abs($residuo) -lt 1e-7) 'Violacao do diafragma'
}
# Movimento normal e rotacoes proprias nao devem receber penalidade.
# Obter a normal diretamente das duas arestas transformadas evita convencoes visuais.
$normal=@(4.0,4.0,-4.0)
$teste=New-Object double[] 21
for($i=0;$i -lt 3;$i++) { $teste[$i]=$normal[$i] }
$teste=[double[]](Chamar $p 'ReduzirInfluenciaMPC' @(,$teste))
$ordem=[alglib]::sparsegetnrows($k)
$produto=New-Object double[] $ordem; $massaTeste=New-Object double[] $ordem
[alglib]::sparsemv($k,$teste,[ref]$produto); [alglib]::sparsemv($m,$teste,[ref]$massaTeste)
for($i=0;$i -lt $ordem;$i++) { Exigir ([Math]::Abs($produto[$i]-10*$massaTeste[$i]) -lt 1e-4) 'Restricao fora do plano' }
$p.MatrizRigidez=Novo ([PG.TMatrizBanda]); $p.MatrizRigidez.s=$k
foreach($subespaco in @($true,$false)) {
    $modal=New-Object PG.TPorticoEspacialModal($p,3)
    Auto $modal 'MatrizMassa' $m
    Campo $modal 'massaAntesMPC' $massaOriginal
    Chamar $modal 'VerificarMatrizesModais'
    Chamar $modal 'PrepararOperadorShiftInvert'
    if($subespaco) { Chamar $modal 'CalcularPorSubespaco' }
    else { Chamar $modal 'ConstruirBaseLanczos' }
    Exigir $modal.Convergiu 'Modal nao convergiu'
    Chamar $modal 'CalcularParticipacaoModal'
    foreach($percentual in $modal.PercentuaisMassaModal) { Exigir ($percentual -ge 0 -and $percentual -le 100.0001) 'Participacao modal' }
    $modos=Chamar $p 'ExpandirModosMPC' @(,$modal.ModosVibracao)
    foreach($valor in $modal.Autovalores) { Exigir ([Math]::Abs($valor-10) -lt 1e-4) 'Autovalor modal' }
    for($a=0;$a -lt 3;$a++) { for($b=0;$b -lt 3;$b++) {
        $produto=0
        for($i=0;$i -lt 18;$i++) { $produto+=$modos[$i,$a]*$modos[$i,$b] }
        $esperado=0; if($a -eq $b){$esperado=1}
        Exigir ([Math]::Abs($produto-$esperado) -lt 1e-4) 'Ortogonalidade modal'
    }}
}
$flambagem=New-Object PG.TPorticoEspacial_FlambagemLinear($p,3)
Auto $flambagem 'NumeroGrausLiberdade' ([int]$ordem)
Auto $flambagem 'MatrizRigidez' $k; Auto $flambagem 'MatrizGeometrica' $g
$flambagem.PrepararRigidez(); $flambagem.CalcularModosPorSubespaco()
Exigir $flambagem.Convergiu 'Flambagem nao convergiu'
foreach($valor in $flambagem.MultiplicadoresCriticos) { Exigir ([Math]::Abs($valor-10) -lt 1e-4) ('Multiplicador critico: '+$valor) }
$flambagem.CalcularModos()
Exigir $flambagem.Convergiu 'Lanczos de flambagem nao convergiu'
foreach($valor in $flambagem.MultiplicadoresCriticos) { Exigir ([Math]::Abs($valor-10) -lt 1e-4) 'Multiplicador Lanczos' }
if($Eliminacao) {
    # A flambagem acrescenta graus internos de articulacoes apos NLinhas.
    $extra=$null; [alglib]::sparsecreate(23,23,[ref]$extra)
    for($i=0;$i -lt 23;$i++) { [alglib]::sparseset($extra,$i,$i,($i+1)) }
    [alglib]::sparseset($extra,0,21,.3); [alglib]::sparseset($extra,21,0,.3)
    $reduzida=Chamar $p 'ReduzirMatrizMPC' @($extra)
    Exigir ([alglib]::sparsegetnrows($reduzida) -eq 17) 'Graus internos da flambagem'
    $q=New-Object 'double[,]' 17,1
    for($i=0;$i -lt 17;$i++) { $q[$i,0]=.1*($i+1) }
    $expandido=Chamar $p 'ExpandirModosMPC' @(,$q)
    Exigir ($expandido.GetLength(0) -eq 23 -and $expandido[21,0] -eq $q[15,0]) 'Recuperacao dos graus internos'
    $energiaOriginal=0; $energiaReduzida=0
    for($i=0;$i -lt 23;$i++) { for($j=0;$j -lt 23;$j++) { $energiaOriginal+=$expandido[$i,0]*[alglib]::sparseget($extra,$i,$j)*$expandido[$j,0] } }
    for($i=0;$i -lt 17;$i++) { for($j=0;$j -lt 17;$j++) { $energiaReduzida+=$q[$i,0]*[alglib]::sparseget($reduzida,$i,$j)*$q[$j,0] } }
    Exigir ([Math]::Abs($energiaOriginal-$energiaReduzida) -lt 1e-9) 'Conservacao da energia na transformacao'
}
# Braco rigido: mestre apoiado, escravo carregado. Testa os seis vinculos.
$lista.Clear()
$braco=New-Object PG.TBracoRigido
Campo $braco 'pIni' $contorno[0]; Campo $braco 'pFin' $contorno[1]
$bracos=New-Object 'System.Collections.Generic.List[PG.TBracoRigido]'; $bracos.Add($braco)
Campo $p 'bracosRigidos' $bracos
$p.nNos=2; $p.nBarras=1; $p.barras=New-Object 'PG.TBarraPortico[]' 2
$barra=New-Object PG.TBarraPortico
Campo $barra 'pIni' $p.nos[1]; Campo $barra 'pFin' $p.nos[2]; $p.barras[1]=$barra
Chamar $p 'PrepararPenalidadesMPC'
$restricoes=$p.GetType().GetField('restricoesMPC',$flags).GetValue($p)
Exigir ($restricoes.Count -eq 6) 'Seis restricoes do braco'
$p.Ngl=12; $p.NLinhas=6; $p.id=[int[]]@(0,7,8,9,10,11,12,1,2,3,4,5,6)
$p.glRestrito=New-Object bool[] 13
for($i=1;$i -le 6;$i++) { $p.glRestrito[$i]=$true }
$k=$null; [alglib]::sparsecreate(6,6,[ref]$k)
for($i=0;$i -lt 6;$i++) { [alglib]::sparseset($k,$i,$i,10) }
Chamar $p 'PrepararEliminacaoMPC'
Chamar $p 'AplicarPenalidadesMPC' @($k,$null)
if($Eliminacao) { Exigir ($null -eq (Chamar $p 'ReduzirMatrizMPC' @($k))) 'Braco totalmente apoiado deve eliminar todas as equacoes' }
else { for($i=0;$i -lt 6;$i++) { Exigir ([alglib]::sparseget($k,$i,$i) -eq 1000000010) 'Penalidade do braco apoiado' } }
# Sem apoios, uma rotacao em Z deve deslocar o escravo em Y e X.
$rigido=[double[]]@(0,0,0,0,0,1,2,2,0,0,0,1)
foreach($c in $restricoes) {
    $residuo=0; foreach($t in $c.GetEnumerator()) { $residuo+=$t.Value*$rigido[$t.Key-1] }
    Exigir ([Math]::Abs($residuo) -lt 1e-12) 'Cinematica do braco nos eixos do solver'
}
# Configuracao persistida e seletor sem sobrepor os grupos existentes.
$sistema=$p.gerenciador.ConfiguracoesPGi.CfgProjeto.sistema
$stream=New-Object IO.MemoryStream
$serializador=New-Object Runtime.Serialization.Formatters.Binary.BinaryFormatter
$serializador.Serialize($stream,$sistema); $stream.Position=0
$restaurado=$serializador.Deserialize($stream); $stream.Dispose()
Exigir ($restaurado.MetodoMPC -eq $sistema.MetodoMPC) 'Persistencia do metodo'
$form=New-Object PG.FConfiguraProjeto
try {
    $combo=$form.GetType().GetField('cbMetodoMPC',$flags).GetValue($form)
    Exigir ($combo.Items.Count -eq 2 -and $combo.SelectedIndex -eq 0) 'Seletor MPC'
    foreach($controle in $combo.Parent.Parent.Controls) {
        if(![object]::ReferenceEquals($controle,$combo.Parent)) {
            Exigir (!$controle.Bounds.IntersectsWith($combo.Parent.Bounds)) 'Sobreposicao no seletor MPC'
        }
    }
} finally { $form.Dispose() }
('PASS: metodo='+$sistema.MetodoMPC+'; estatica, modal, flambagem, recuperacao, configuracao e seletor.')
