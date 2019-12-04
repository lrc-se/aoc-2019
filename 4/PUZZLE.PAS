{$R+}

program Puzzle;

uses crt;

const
  rangeStart = 125730;
  rangeEnd = 579381;

var
  tests: array[1..6] of string;
  i: longint;
  count: longint;
  curPwd: string;

function IsMatch(pwd: string): boolean;
var
  i: integer;
  lastDigit: integer;
  curDigit: integer;
  dummy: integer;
  hasDouble: boolean;
begin
  if Length(pwd) <> 6 then begin
     IsMatch := false;
     exit;
  end;

  lastDigit := -1;
  hasDouble := false;
  for i := 1 to 6 do begin
    Val(Copy(pwd, i, 1), curDigit, dummy);
    if curDigit < lastDigit then begin
      IsMatch := false;
      exit;
    end;
    if lastDigit = curDigit then
      hasDouble := true;

    lastDigit := curDigit;
  end;
  IsMatch := hasDouble;
end;

begin
  ClrScr;

  (* Tests *)
  tests[1] := '111111';
  tests[2] := '223450';
  tests[3] := '123789';
  tests[4] := '12345';
  tests[5] := '044679';
  tests[6] := '044357';
  WriteLn('TESTS');
  WriteLn('=====');
  for i := 1 to High(tests) do begin
    Write(tests[i] + ': ');
    if IsMatch(tests[i]) then
      WriteLn('MATCH')
    else
      WriteLn('NO MATCH');
  end;
  WriteLn;

  (* Puzzle 1 *)
  WriteLn('PUZZLE 1');
  WriteLn('========');
  count := 0;
  for i := rangeStart to rangeEnd do begin
    Str(i, curPwd);
    if IsMatch(curPwd) then
      count := count + 1;
  end;
  WriteLn('Possible passwords: ', count);

  repeat until KeyPressed;
end.