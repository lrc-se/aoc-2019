"use strict";

const fs = require("fs");
const network = require("./network");


let lastResumePacket = {};

function handlePacket(packet) {
  if(packet.address == 255) {
    console.log(`Packet sent to NAT: X=${packet.x}, Y=${packet.y}`);
    if(network.isIdle()) {
      console.log("Network idle: relaying packet to address 0");
      network.sendData(0, [packet.x, packet.y]);

      if(lastResumePacket.x === packet.x && lastResumePacket.y === packet.y) {
        console.log(`Repeated packet delivered to address 0: X=${packet.x}, Y=${packet.y}`);
        process.exit();
      }
      lastResumePacket = packet;
    } else {
      console.log("Network not idle: no action");
    }
  }
}


const input = fs.readFileSync("input.txt", "utf8").split(",").map(line => parseInt(line, 10));
for(let i = 0; i < 50; ++i) {
  network.addComputer(input);
}
network.on("packet", handlePacket);
network.boot();
