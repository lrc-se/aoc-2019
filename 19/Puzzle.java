import java.nio.file.*;
import java.util.*;

public class Puzzle {
  public static void main(String[] args) {
    try {
      String inputData = new String(Files.readAllBytes(Paths.get("input.txt")));
      List<Long> input = new ArrayList<>();
      for (String code : inputData.strip().split(",")) {
        input.add(Long.parseLong(code));
      }

      System.out.println("PUZZLE 1");
      System.out.println("========");
      IntcodeRunner runner = new IntcodeRunner(null, IOMode.INTERNAL, IOMode.INTERNAL);
      Queue<Long> inputQueue = runner.getInputQueue();
      List<Long> outputList = runner.getOutputList();

      int counter = 0;
      int numPoints = 0;
      for (long y = 0; y < 50; y++) {
        for (long x = 0; x < 50; x++) {
          runner.setProgram(input);
          inputQueue.add(x);
          inputQueue.add(y);
          runner.run();
          if (outputList.get(counter++) == 1) {
            System.out.print("#");
            numPoints++;
          } else {
            System.out.print(".");
          }
        }
        System.out.println();
      }

      System.out.println();
      System.out.println("Numer of affected points: " + numPoints);
    } catch (Exception e) {
      e.printStackTrace();
      System.exit(1);
    }
  }
}