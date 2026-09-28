$ErrorActionPreference = 'Stop'
$arquivo = Join-Path $PSScriptRoot ('../P' + [char]0x00f3 + 'rtico/TPorticoEspacial_MPC.cs')
$fonte = [IO.File]::ReadAllText($arquivo)
$trecho = [regex]::Match($fonte, '(?s)#region Identificacao geometrica dos diafragmas(.*?)#endregion').Groups[1].Value
if (!$trecho) { throw 'Identificacao de diafragmas nao encontrada.' }
$testes = @'
using System;
using System.Collections.Generic;
using System.Linq;
// Dados minimos para executar a geometria real sem inicializar a interface.
class TNoPortico {
    public double x,y,z;
    public TNoPortico(double x,double y,double z) { this.x=x; this.y=y; this.z=z; }
}
class TDiafragmaRigido { public int ID=7; public List<TNoPortico> pontosContorno; }
public class ValidacaoDiafragmas {
    static TNoPortico P(double x,double y,double z=0) { return new TNoPortico(x,y,z); }
    static TDiafragmaRigido D(params TNoPortico[] p) { return new TDiafragmaRigido {pontosContorno=p.ToList()}; }
    static void Exigir(bool ok) { if(!ok) throw new Exception("Falha na identificacao do diafragma"); }
    static void Invalido(TDiafragmaRigido d) {
        try { IdentificarDiafragma(d,new[]{(TNoPortico)null,P(0,0)},1); }
        catch(InvalidOperationException) { return; }
        throw new Exception("Contorno invalido aceito");
    }
    public static string Executar() {
        var d=D(P(0,0),P(4,0),P(4,2),P(0,2),P(0,0));
        var nos=new[]{(TNoPortico)null,P(0,0),P(4,0),P(4,2),P(0,2),P(1,1),P(2,0),P(5,1),P(1,1,.1),P(2,1)};
        var r=IdentificarDiafragma(d,nos,nos.Length-1);
        Exigir(!r.mestreAuxiliar && ReferenceEquals(r.mestre,nos[9]) && r.escravos.Count==6);
        Exigir(!r.escravos.Contains(nos[7]) && !r.escravos.Contains(nos[8]));
        r=IdentificarDiafragma(d,nos,8);
        Exigir(r.mestreAuxiliar && Math.Abs(r.mestre.x-2)<1e-12 && Math.Abs(r.mestre.y-1)<1e-12);
        d.pontosContorno.Reverse();
        r=IdentificarDiafragma(d,nos,8);
        Exigir(Math.Abs(r.mestre.x-2)<1e-12 && Math.Abs(r.mestre.y-1)<1e-12);
        // Plano inclinado z=x+2y, incluindo um no coplanar fora do poligono.
        d=D(P(10,20,50),P(14,20,54),P(14,22,58),P(10,22,54));
        nos=new[]{(TNoPortico)null,P(11,21,53),P(10,21,52),P(11,21,53.1),P(16,21,58)};
        r=IdentificarDiafragma(d,nos,4);
        Exigir(r.escravos.Count==2 && Math.Abs(r.mestre.x-12)<1e-12
            && Math.Abs(r.mestre.y-21)<1e-12 && Math.Abs(r.mestre.z-54)<1e-12);
        // Poligono concavo: o centroide nao e a media dos vertices.
        d=D(P(0,0),P(3,0),P(3,1),P(1,1),P(1,3),P(0,3));
        nos=new[]{(TNoPortico)null,P(.5,2),P(2,2),P(1,2)};
        r=IdentificarDiafragma(d,nos,3);
        Exigir(r.escravos.Count==2 && Math.Abs(r.mestre.x-1.1)<1e-12 && Math.Abs(r.mestre.y-1.1)<1e-12);
        // Primeiros tres vertices colineares e plano vertical.
        d=D(P(2,0,0),P(2,1,0),P(2,2,0),P(2,2,2),P(2,0,2));
        r=IdentificarDiafragma(d,new[]{(TNoPortico)null,P(2,.5,1)},1);
        Exigir(Math.Abs(r.mestre.x-2)<1e-12 && Math.Abs(r.mestre.y-1)<1e-12 && Math.Abs(r.mestre.z-1)<1e-12);
        Invalido(D(P(0,0),P(1,0),P(2,0)));
        Invalido(D(P(0,0),P(2,0),P(2,2),P(0,2,.1)));
        Invalido(D(P(0,0),P(2,2),P(0,2),P(2,0)));
        return "OK: contorno fechado/invertido, vertices, borda, interior, exclusao fora do plano/poligono, mestre existente/auxiliar, planos inclinado/vertical, concavidade e contornos invalidos.";
    }
'@
$opentk = [IO.Path]::GetFullPath((Join-Path $PSScriptRoot '../bin/Release/OpenTK.dll'))
Add-Type -Path $opentk
Add-Type -TypeDefinition ($testes + $trecho + '}') -ReferencedAssemblies @($opentk, 'System.Core')
[ValidacaoDiafragmas]::Executar()
