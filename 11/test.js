var Test = (function(doc) {
  "use strict";

  var handlers = {
    output: function(value) {
      AOC.printLine(value);
    },

    inputwaiting: function() {
      AOC.setState("waiting");
    },

    halt: function() {
      AOC.setState("halted");
    }
  };

  function handleTestButtonClick(e) {
    if(AOC.isRunning()) {
      alert("Test already running!");
      return;
    }

    var test = e.target.getAttribute("data-test");
    if(test) {
      AOC.clearOutput();
      var input = e.target.getAttribute("data-input") || "";
      runTest(test, input.split(","), +e.target.getAttribute("data-delay") || 0);
    }
  }

  function runTest(test, input, delay) {
    AOC.setState("running");
    AOC.getJSON(test, function(program) {
      var runner = Intcode.createRunner(program);
      runner.onoutput = handlers.output;
      runner.oninputwaiting = handlers.inputwaiting;
      runner.onhalt = handlers.halt;
      if(Array.isArray(input) && input.length) {
        setTimeout(function() {
          input.forEach(function(value) {
            runner.enqueueInput(parseInt(value, 10));
          });
        }, delay);
      }
      setTimeout(function() {
        runner.run();
      }, 500);
    });
  }

  function init() {
    AOC.init();
    Array.prototype.forEach.call(doc.querySelectorAll(".run-test"), function(button) {
      button.addEventListener("click", handleTestButtonClick);
    });
  }

  return {
    init: init
  };
})(document);
