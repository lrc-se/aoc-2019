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

function IsMatch(pwd: string; noGroups: boolean): boolean;
var
  i: integer;
  lastDigit: integer;
  secondLastDigit: integer;
  curDigit: integer;
  doubledDigit: integer;
  dummy: integer;
  hasDouble: boolean;
begin
  if Length(pwd) <> 6 then begin
     IsMatch := false;
     exit;
  end;

  lastDigit := -1;
  secondLastDigit := -2;
  doubledDigit := -1;
  hasDouble := false;
  for i := 1 to 6 do begin
    Val(Copy(pwd, i, 1), curDigit, dummy);
    if curDigit < lastDigit then begin
      IsMatch := false;
      exit;
    end;
    if lastDigit = curDigit then begin
      if noGroups then begin
        if curDigit = doubledDigit then
          hasDouble := (secondLastDigit <> doubledDigit)
        else if not hasDouble then begin
          doubledDigit := curDigit;
          hasDouble := true;
        end
      end
      else begin
        doubledDigit := curDigit;
        hasDouble := true;
      end;
    end;

    secondLastDigit := lastDigit;
    lastDigit := curDigit;
  end;
  IsMatch := hasDouble;
end;

begin
  ClrScr;

  (* Tests for puzzle 1 *)
  tests[1] := '111111';
  tests[2] := '223450';
  tests[3] := '123789';
  tests[4] := '12345';
  tests[5] := '044679';
  tests[6] := '044357';
  WriteLn('TESTS 1');
  WriteLn('=======');
  for i := 1 to High(tests) do begin
    Write(tests[i] + ': ');
    if IsMatch(tests[i], false) then
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
    if IsMatch(curPwd, false) then
      count := count + 1;
  end;
  WriteLn('Possible passwords: ', count);
  WriteLn;

  (* Tests for puzzle 2 *)
  tests[1] := '112233';
  tests[2] := '123444';
  tests[3] := '111122';
  tests[4] := '223333';
  tests[5] := '122246';
  tests[6] := '678888';
  WriteLn('TESTS 2');
  WriteLn('=======');
  for i := 1 to High(tests) do begin
    Write(tests[i] + ': ');
    if IsMatch(tests[i], true) then
      WriteLn('MATCH')
    else
      WriteLn('NO MATCH');
  end;
  WriteLn;

  repeat until KeyPressed;
end.