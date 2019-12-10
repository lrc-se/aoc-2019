import java.nio.file.*;
import java.util.*;

public class Puzzle {
  public static void main(String[] args) {
    runTests();

    try {
      String inputData = new String(Files.readAllBytes(Paths.get("input.txt")));
      List<Long> input = new ArrayList<>();
      for (String code : inputData.strip().split(",")) {
        input.add(Long.parseLong(code));
      }

      IntcodeRunner runner = new IntcodeRunner(input, IOMode.INTERNAL, IOMode.EXTERNAL, 4096);
      System.out.print("Puzzle 1 result: ");
      runner.getInputQueue().add(1L);
      runner.run();

      runner.getInputQueue().add(2L);
      System.out.print("Puzzle 2 result: ");
      long start = System.nanoTime();
      runner.run();
      long end = System.nanoTime();
      System.out.println("Puzzle 2 execution time: " + ((end - start) / 1000000) + "ms");
    } catch (Exception e) {
      e.printStackTrace();
      System.exit(1);
    }
  }

  public static void runTests() {
    IntcodeRunner runner = new IntcodeRunner(null, IOMode.EXTERNAL, IOMode.INTERNAL);

    try {
      runner.setProgram(Arrays.asList(109L, 1L, 204L, -1L, 1001L, 100L, 1L, 100L, 1008L, 100L, 16L, 101L, 1006L, 101L, 0L, 99L));
      runner.run();
      System.out.println(
          "Test 1: " + runner.getOutputList().equals(Arrays.asList(109L, 1L, 204L, -1L, 1001L, 100L, 1L, 100L, 1008L, 100L, 16L, 101L, 1006L, 101L, 0L, 99L)));

      runner.setProgram(Arrays.asList(1102L, 34915192L, 34915192L, 7L, 4L, 7L, 99L, 0L));
      runner.clearOutput();
      runner.run();
      System.out.println("Test 2: " + (runner.getOutputList().get(0) == 1219070632396864L));

      runner.setProgram(Arrays.asList(104L, 1125899906842624L, 99L));
      runner.clearOutput();
      runner.run();
      System.out.println("Test 3: " + (runner.getOutputList().get(0) == 1125899906842624L));
    } catch (Exception e) {
      e.printStackTrace();
      System.exit(1);
    }
  }
}