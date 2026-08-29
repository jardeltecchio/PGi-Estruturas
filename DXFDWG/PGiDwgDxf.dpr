program PGiDwgDxf;

uses
  Forms,
  uDXFDWG in 'uDXFDWG.pas' {FDXFDWG};

{$R *.res}

begin
  Application.Initialize;
  Application.Title := 'PGi - Importar DWG/DXF';
  Application.CreateForm(TFDXFDWG, FDXFDWG);
  Application.Run;
end.
