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

      runPuzzle1(input);
      System.out.println();
      runPuzzle2(input);
    } catch (Exception e) {
      e.printStackTrace();
      System.exit(1);
    }
  }

  private static void runPuzzle1(List<Long> input) throws Exception {
    System.out.println("PUZZLE 1");
    System.out.println("========");
    IntcodeRunner runner = new IntcodeRunner(null, IOMode.INTERNAL, IOMode.INTERNAL);
    Queue<Long> inputQueue = runner.getInputQueue();
    List<Long> outputList = runner.getOutputList();

    int counter = 0;
    int numPoints = 0;
    System.out.println("Scan from 0,0 to 49,49:");
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
  }

  private static void runPuzzle2(List<Long> input) throws Exception {
    System.out.println("PUZZLE 2");
    System.out.println("========");
    IntcodeRunner runner = new IntcodeRunner(null, IOMode.INTERNAL, IOMode.INTERNAL);
    Queue<Long> inputQueue = runner.getInputQueue();
    List<Long> outputList = runner.getOutputList();

    // find slopes of boundary lines with sufficient precision
    double upperSlope = 0;
    double lowerSlope = 0;
    long x = 10000;
    long y = 0;
    while (true) {
      runner.setProgram(input);
      inputQueue.add(x);
      inputQueue.add(y);
      runner.clearOutput();
      runner.run();

      long output = outputList.get(0);
      if (upperSlope == 0 && output == 1) {
        upperSlope = (double)y / x;
      } else if (upperSlope != 0 && output == 0) {
        lowerSlope = (double)y / x;
        break;
      }
      y++;
    }

    // calculate (some) corner coordinates of boundary box
    int upperRightX = (int)Math.round((99 + 99 * lowerSlope) / (lowerSlope - upperSlope));
    int upperRightY = (int)Math.round(upperSlope * upperRightX);
    int upperLeftX = upperRightX - 99;
    int upperLeftY = upperRightY;

    // print scan of extended boundary box for confirmation
    System.out.println("Scan from " + (upperLeftX - 1) + "," + (upperLeftY - 1) + " to " + (upperLeftX + 100) + "," + (upperLeftY + 100) + ":");
    for (y = upperLeftY - 1; y <= upperLeftY + 100; y++) {
      for (x = upperLeftX - 1; x <= upperLeftX + 100; x++) {
        runner.setProgram(input);
        inputQueue.add(x);
        inputQueue.add(y);
        runner.clearOutput();
        runner.run();

        if (outputList.get(0) == 1) {
          System.out.print("#");
        } else {
          System.out.print(".");
        }
      }
      System.out.println();
    }

    System.out.println();
    System.out.println("Coordinates of closest point: " + upperLeftX + "," + upperLeftY);
    System.out.println("Result: " + (upperLeftX * 10000 + upperLeftY));
  }
}