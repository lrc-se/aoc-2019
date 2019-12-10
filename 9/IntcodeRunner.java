import java.beans.ExceptionListener;
import java.util.*;

public class IntcodeRunner {
  private static final List<Operation> OPERATIONS = List.of(
    new Operation(1, 3),
    new Operation(2, 3),
    new Operation(3, 1),
    new Operation(4, 1),
    new Operation(5, 2),
    new Operation(6, 2),
    new Operation(7, 3),
    new Operation(8, 3),
    new Operation(99, 0)
  );

  private Queue<Long> inputQueue = new LinkedList<>();
  private List<Long> outputList = new ArrayList<>();
  private volatile boolean inputAvailable = false;
  private final Object inputAvailableLock = new Object();
  private IntcodeEventListener eventListener;

  public interface IntcodeEventListener extends EventListener {
    void outputAvailable(long value);
    void halted();
  }

  public List<Long> program;
  public IOMode inputMode;
  public IOMode outputMode;


  public IntcodeRunner() {
    this(null);
  }

  public IntcodeRunner(Collection<Long> program) {
    this(program, IOMode.EXTERNAL, IOMode.EXTERNAL);
  }

  public IntcodeRunner(Collection<Long> program, IOMode inputMode, IOMode outputMode) {
    this.program = (program != null ? new ArrayList<>(program) : new ArrayList<>());
    this.inputMode = inputMode;
    this.outputMode = outputMode;
  }

  public Queue<Long> getInputQueue() {
    return (inputMode == IOMode.INTERNAL ? inputQueue : null);
  }

  public void setInputQueue(Queue<Long> input) throws Exception {
    if (inputMode == IOMode.INTERNAL) {
      inputQueue = input;
    } else {
      throw new Exception("Cannot set input data when not in internal input mode");
    }
  }

  public List<Long> getOutputList() {
    return (outputMode == IOMode.INTERNAL ? Collections.unmodifiableList(outputList) : null);
  }

  public void run() throws Exception {
    int pointer = 0;
    while (pointer < program.size()) {
      Instruction instruction = new Instruction(program.get(pointer));

      Operation operation = OPERATIONS.stream().filter(o -> o.code == instruction.opcode).findFirst().orElse(null);
      if (operation == null) {
        throw new Exception("Unsupported opcode " + instruction.opcode + " at position " + pointer);
      }

      List<Parameter> parameters = new ArrayList<>();
      for (int i = 0; i < operation.parameterCount; i++) {
        parameters.add(
            new Parameter(program.get(pointer + i + 1),
                (i < instruction.parameterModes.size() ? instruction.parameterModes.get(i) : ParameterMode.POSITION)));
      }

      switch (operation.code) {
      case 1:
        program.set((int)parameters.get(2).value,
            getParameterValue(parameters.get(0)) + getParameterValue(parameters.get(1)));
        break;

      case 2:
        program.set((int)parameters.get(2).value,
            getParameterValue(parameters.get(0)) * getParameterValue(parameters.get(1)));
        break;

      case 3: {
        long value;
        if (inputMode == IOMode.INTERNAL) {
          if (!inputQueue.isEmpty()) {
            value = inputQueue.remove();
          } else {
            throw new Exception("Empty input queue for input operation at position " + pointer);
          }
        } else if (inputMode == IOMode.EVENT) {
          if (inputQueue.isEmpty()) {
            synchronized (inputAvailableLock) {
              while (!inputAvailable) {
                try {
                  inputAvailableLock.wait();
                } catch (InterruptedException e) { }
              }
            }
          }
          value = inputQueue.remove();
          if (inputQueue.isEmpty()) {
            inputAvailable = false;
          }
        } else {
          System.out.print("Input: ");
          value = Integer.parseInt(System.console().readLine());
        }
        program.set((int)parameters.get(0).value, value);
        break;
      }

      case 4: {
        long value = getParameterValue(parameters.get(0));
        if (outputMode == IOMode.INTERNAL) {
          outputList.add(value);
        } else if (outputMode == IOMode.EVENT) {
          fireOutputEvent(value);
        } else {
          System.out.println(value);
        }
        break;
      }

      case 5:
        if (getParameterValue(parameters.get(0)) != 0) {
          pointer = (int)getParameterValue(parameters.get(1));
          continue;
        }
        break;

      case 6:
        if (getParameterValue(parameters.get(0)) == 0) {
          pointer = (int)getParameterValue(parameters.get(1));
          continue;
        }
        break;

      case 7:
        program.set((int)parameters.get(2).value,
            (getParameterValue(parameters.get(0)) < getParameterValue(parameters.get(1)) ? 1L : 0L));
        break;

      case 8:
        program.set((int)parameters.get(2).value,
            (getParameterValue(parameters.get(0)) == getParameterValue(parameters.get(1)) ? 1L : 0L));
        break;

      case 99:
        fireHaltEvent();
        return;
      }

      pointer += operation.parameterCount + 1;
    }

    fireHaltEvent();
  }

  public void runAsync(ExceptionListener exceptionListener) {
    new Thread(() -> {
      try {
        run();
      } catch (Exception e) {
        exceptionListener.exceptionThrown(e);
      }
    }).start();
  }

  public void receiveInput(long value) throws Exception {
    if (inputMode == IOMode.EVENT) {
      inputQueue.add(value);
      synchronized (inputAvailableLock) {
        inputAvailable = true;
        inputAvailableLock.notifyAll();
      }
    } else {
      throw new Exception("Cannot receive input when not in event input mode");
    }
  }

  public void clearOutput() throws Exception {
    if (outputMode == IOMode.INTERNAL) {
      outputList.clear();
    } else {
      throw new Exception("Cannot clear output data when not in internal output mode");
    }
  }

  public void setEventListener(IntcodeEventListener listener) {
    eventListener = listener;
  }


  private long getParameterValue(Parameter parameter) {
    return (parameter.mode == ParameterMode.POSITION ? program.get((int)parameter.value) : parameter.value);
  }

  private void fireOutputEvent(long value) {
    if (eventListener != null) {
      eventListener.outputAvailable(value);
    }
  }

  private void fireHaltEvent() {
    if (eventListener != null) {
      eventListener.halted();
    }
  }
}
