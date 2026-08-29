program prDXFDWG;

uses
  Forms,
  uDXFDWG in 'uDXFDWG.pas' {FDXFDWG};

{$R *.res}

begin
  Application.Initialize;
  Application.CreateForm(TFDXFDWG, FDXFDWG);
  Application.Run;
end.
