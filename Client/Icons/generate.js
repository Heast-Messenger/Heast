const fs = require("fs");

let template = fs.readFileSync("Icons.axaml.template").toString();
let codepoints = fs.readFileSync("Input/remixicon.css").toString();
let matches = codepoints.match(/^.*content.*$/gm);

let icons = []
matches.forEach(match => {
    let key = "Icon." + match.match(/(?<=\.ri-).*(?=:before)/)
        .toString()
        .replace(/\d+-/g, "")       // Remove icon-variant-number
        //  .replace(/-[^-]*$/g, "")    // Remove suffix (icon-fill-mode)
        .replace(/(^\w|-\w)/g, value => {
            return value.toUpperCase();
        });
    let codepoint = match.match(/(?<="\\).*(?=")/)
    let icon = "  ".repeat(2) + `<x:String x:Key="${key}">&#x${codepoint};</x:String>`
    icons.push(icon)
})

fs.cpSync(`Input/remixicon.ttf`, "../Assets/Fonts/Icons.ttf");
let output = template.replace("{{icons}}", icons.join("\n"));
fs.rmSync(`../Resources/Icons.axaml`, {force: true});
fs.writeFileSync(`../Resources/Icons.axaml`, output);