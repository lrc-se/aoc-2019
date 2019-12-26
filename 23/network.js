"use strict";

const intcode = require("./intcode");


const computers = [];

function handleInput() {
  if(this.packetQueue.length) {
    const value = this.packetQueue[0].shift();
    setImmediate(() => {
      this.runner.enqueueInput(value);
    });
    if(!this.packetQueue[0].length) {
      this.packetQueue.shift();
    }
  } else {
    setImmediate(() => {
      this.runner.enqueueInput(-1);
    });
  }
}

function handleOutput(value) {
  this.outputList.push(value);
  if(this.outputList.length == 3) {
    const address = this.outputList[0];
    if(address < computers.length) {
      Network.sendData(address, this.outputList.slice(1));
    }
    Network.emit("packet", {
      address,
      x: this.outputList[1],
      y: this.outputList[2]
    });
    this.outputList = [];
  }
}


const Network = {
  addComputer(program) {
    const computer = {
      address: computers.length,
      packetQueue: [],
      outputList: [],
      runner: intcode.createRunner(program)
    };
    computers.push(computer);

    computer.runner.on("inputwaiting", handleInput.bind(computer));
    computer.runner.on("output", handleOutput.bind(computer));
    computer.runner.enqueueInput(computer.address);
    return computer;
  },

  boot() {
    for(let computer of computers) {
      computer.runner.run();
    }
  },

  sendData(address, data) {
    computers[address].packetQueue.push(data);
  }
};

Object.setPrototypeOf(Network, require("events").EventEmitter.prototype);


module.exports = Network;
