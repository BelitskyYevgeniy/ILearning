const hash = require("sha3");
const fs = require('fs');
for (var file of fs.readdirSync(process.cwd())) {
    let hex = hash.SHA3(256).update(fs.readFileSync(file)).digest('hex')
    console.log(file + '\t' + hex );
}