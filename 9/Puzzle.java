import java.util.*;

public class Puzzle {
  public static void main(String[] args) {
    runTests();
  }

  public static void runTests() {
    IntcodeRunner runner = new IntcodeRunner(null, IOMode.EXTERNAL, IOMode.INTERNAL);

    try {
      runner.program = Arrays.asList(1102L, 34915192L, 34915192L, 7L, 4L, 7L, 99L, 0L);
      runner.run();
      System.out.println(
          "Test 1: " + (runner.getOutputList().get(0) == 1219070632396864L));
    } catch (Exception e) {
      e.printStackTrace();
      System.exit(1);
  }
  }
}