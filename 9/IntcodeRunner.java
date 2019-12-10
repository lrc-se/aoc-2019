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
    new Operation(9, 1),
    new Operation(99, 0)
  );
  private static final int DEFAULT_MEMORY_SIZE = 1024;

  private int relativeBase = 0;
  private Queue<Long> inputQueue = new LinkedList<>();
  private List<Long> outputList = new ArrayList<>();
  private volatile boolean inputAvailable = false;
  private final Object inputAvailableLock = new Object();
  private IntcodeEventListener eventListener;

  public interface IntcodeEventListener extends EventListener {
    void outputAvailable(long value);
    void halted();
  }

  public List<Long> memory;
  public IOMode inputMode;
  public IOMode outputMode;


  public IntcodeRunner() {
    this(null);
  }

  public IntcodeRunner(int memorySize) {
    this(null, memorySize);
  }

    public IntcodeRunner(Collection<Long> program) {
    this(program, DEFAULT_MEMORY_SIZE);
  }

  public IntcodeRunner(Collection<Long> program, int memorySize) {
    this(program, IOMode.EXTERNAL, IOMode.EXTERNAL, memorySize);
  }

  public IntcodeRunner(Collection<Long> program, IOMode inputMode, IOMode outputMode) {
    setProgram(program);
    this.inputMode = inputMode;
    this.outputMode = outputMode;
  }

  public IntcodeRunner(Collection<Long> program, IOMode inputMode, IOMode outputMode, int memorySize) {
    setProgram(program, memorySize);
    this.inputMode = inputMode;
    this.outputMode = outputMode;
  }


  public void setProgram(Collection<Long> program) {
    setProgram(program, DEFAULT_MEMORY_SIZE);
  }

  public void setProgram(Collection<Long> program, int memorySize) {
    int programSize = (program != null ? program.size() : 0);
    memory = (programSize > 0 ? new ArrayList<>(program) : new ArrayList<>());
    memory.addAll(Collections.nCopies(memorySize - programSize, 0L));
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
    relativeBase = 0;
    while (pointer < memory.size()) {
      Instruction instruction = new Instruction(memory.get(pointer));

      Operation operation = OPERATIONS.stream().filter(o -> o.code == instruction.opcode).findFirst().orElse(null);
      if (operation == null) {
        throw new Exception("Unsupported opcode " + instruction.opcode + " at position " + pointer);
      }

      List<Parameter> parameters = new ArrayList<>();
      for (int i = 0; i < operation.parameterCount; i++) {
        parameters.add(
            new Parameter(memory.get(pointer + i + 1),
                (i < instruction.parameterModes.size() ? instruction.parameterModes.get(i) : ParameterMode.POSITION)));
      }

      switch (operation.code) {
      case 1:
        memory.set((int)parameters.get(2).value,
            getParameterValue(parameters.get(0)) + getParameterValue(parameters.get(1)));
        break;

      case 2:
        memory.set((int)parameters.get(2).value,
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
        memory.set((int)parameters.get(0).value, value);
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
        memory.set((int)parameters.get(2).value,
            (getParameterValue(parameters.get(0)) < getParameterValue(parameters.get(1)) ? 1L : 0L));
        break;

      case 8:
        memory.set((int)parameters.get(2).value,
            (getParameterValue(parameters.get(0)) == getParameterValue(parameters.get(1)) ? 1L : 0L));
        break;

      case 9:
        relativeBase += getParameterValue(parameters.get(0));
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
    long value;
    switch (parameter.mode) {
      case IMMEDIATE:
        value = parameter.value;
        break;
      case RELATIVE:
        value = memory.get(relativeBase + (int)parameter.value);
        break;
      case POSITION:
      default:
        value = memory.get((int)parameter.value);
        break;
    }
    return value;
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
