"use strict";

const fs = require("fs");
const network = require("./network");


function handlePacket(packet) {
  if(packet.address == 255) {
    console.log(`Packet sent to address 255: X=${packet.x}, Y=${packet.y}`);
    process.exit();
  }
}


const input = fs.readFileSync("input.txt", "utf8").split(",").map(line => parseInt(line, 10));
for(let i = 0; i < 50; ++i) {
  network.addComputer(input);
}
network.on("packet", handlePacket);
network.boot();
