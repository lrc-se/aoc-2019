using System;
using System.Linq;
using System.Collections;
using System.Collections.Generic;

namespace AOC2019 
{
  public class IntcodeRunner
  {
    private static readonly IEnumerable<Operation> Operations = new List<Operation>
    {
      new Operation(1, 3),
      new Operation(2, 3),
      new Operation(3, 1),
      new Operation(4, 1),
      new Operation(99, 0)
    };

    public IList<int> Program { get; set; }


    public IntcodeRunner(IEnumerable<int> program = null)
    {
      Program = (program != null ? program.ToList() : new List<int>());
    }

    public void Run()
    {
      int pointer = 0;
      while (pointer < Program.Count())
      {
        var instruction = new Instruction(Program[pointer]);

        var operation = Operations.FirstOrDefault(o => o.Code == instruction.Opcode);
        if (operation == null)
        {
          throw new Exception("Unsupported opcode " + instruction.Opcode + " at position " + pointer);
        }
        var parameters = Program.Skip(pointer + 1).Take(operation.ParameterCount).Select((p, i) => new Parameter(
          Program[pointer + i + 1],
          instruction.ParameterModes.ElementAtOrDefault(i)
        )).ToList();

        switch (operation.Code)
        {
          case 1:
            Program[parameters[2].Value] = GetParameterValue(parameters[0]) + GetParameterValue(parameters[1]);
            break;
          case 2:
            Program[parameters[2].Value] = GetParameterValue(parameters[0]) * GetParameterValue(parameters[1]);
            break;
          case 3:
            Console.Write("Input: ");
            Program[parameters[0].Value] = Convert.ToInt32(Console.ReadLine());
            break;
          case 4:
            Console.WriteLine(GetParameterValue(parameters[0]));
            break;
          case 99:
            return;
        }
        pointer += operation.ParameterCount + 1;
      }
    }

    
    private int GetParameterValue(Parameter parameter)
    {
      return (parameter.Mode == ParameterMode.Position ? Program[parameter.Value] : parameter.Value);
    }
  }

  public class Operation
  {
    public int Code { get; private set; }
    public int ParameterCount { get; private set; }

    public Operation(int code, int parameterCount)
    {
      Code = code;
      ParameterCount = parameterCount;
    }
  }

  public class Instruction
  {
    public int Opcode { get; set; }
    public IList<ParameterMode> ParameterModes { get; set; }

    public Instruction(int code)
    {
      int modes = code / 100;
      Opcode = code - modes * 100;
      ParameterModes = new List<ParameterMode>();
      while (modes >= 1)
      {
        ParameterModes.Add((ParameterMode)(modes - modes / 10 * 10));
        modes /= 10;
      }
    }
  }

  public class Parameter
  {
    public int Value { get; set; }
    public ParameterMode Mode { get; set; }

    public Parameter(int value, ParameterMode mode)
    {
      Value = value;
      Mode = mode;
    }
  }

  public enum ParameterMode
  {
    Position = 0,
    Immediate = 1
  }
}
