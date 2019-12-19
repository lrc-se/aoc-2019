import java.util.*;

public class Instruction {
  public long opcode;
  public List<ParameterMode> parameterModes;

  public Instruction(long code) {
    long modes = code / 100;
    this.opcode = code - modes * 100;
    this.parameterModes = new ArrayList<>();
    while (modes >= 1) {
      this.parameterModes.add(ParameterMode.values()[(int)(modes - modes / 10 * 10)]);
      modes /= 10;
    }
  }
}