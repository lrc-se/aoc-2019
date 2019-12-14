var AOC = (function(doc) {
  "use strict";

  var output;
  var isRunning = false;

  function getJSON(name, callback) {
    var xhr = new XMLHttpRequest();
    xhr.open("GET", name + ".json");
    xhr.onreadystatechange = function() {
      if(xhr.readyState == 4) {
        callback(JSON.parse(xhr.responseText));
      }
    };
    xhr.send();
  }

  function printLine(line) {
    var div = doc.createElement("div");
    if(typeof line != "undefined") {
      div.textContent = line;
    } else {
      div.innerHTML = "&nbsp;";
    }
    output.appendChild(div);
  }

  function clearOutput() {
    output.innerHTML = "";
  }

  function setState(state) {
    doc.body.className = state;
    if(state == "running") {
      isRunning = true;
    } else if(state == "halted") {
      isRunning = false;
    }
  }

  function init() {
    output = doc.getElementById("output");
    doc.getElementById("clear-output").addEventListener("click", clearOutput);
  }

  return {
    getJSON: getJSON,
    printLine: printLine,
    clearOutput: clearOutput,
    setState: setState,
    isRunning: function() {
      return isRunning;
    },
    init: init
  };
})(document);
