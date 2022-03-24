﻿//const hsl2rgb = require('./hsl2rgb.js');
//const pnglib = require('./pnglib');

console.log('from _ test');

const address = '0x47B40160f72C4321E08DE8B95E262e902c991cD3';

const randseed = new Array(4);

function seedrand(seed) {
	for (let i = 0; i < randseed.length; i++) {
		randseed[i] = 0;
	}
	for (let i = 0; i < seed.length; i++) {
		randseed[i % 4] = (randseed[i % 4] << 5) - randseed[i % 4] + seed.charCodeAt(i);
	}
}

function buildOpts(opts) {
	var newOpts = {};

	newOpts.seed = opts.seed || Math.floor((Math.random() * Math.pow(10, 16))).toString(16);

	seedrand(newOpts.seed);

	newOpts.size = opts.size || 8;
	newOpts.scale = opts.scale || 4;
	newOpts.color = opts.color || createColor();
	newOpts.bgcolor = opts.bgcolor || createColor();
	newOpts.spotcolor = opts.spotcolor || createColor();

	return newOpts;
}

function createImageData(size) {
	var width = size; // Only support square icons for now
	var height = size;

	var dataWidth = Math.ceil(width / 2);
	var mirrorWidth = width - dataWidth;

	var data = [];
	for (var y = 0; y < height; y++) {
		var row = [];
		for (var x = 0; x < dataWidth; x++) {
			// this makes foreground and background color to have a 43% (1/2.3) probability
			// spot color has 13% chance
			row[x] = Math.floor(rand() * 2.3);
		}
		var r = row.slice(0, mirrorWidth);
		r.reverse();
		row = row.concat(r);

		for (var i = 0; i < row.length; i++) {
			data.push(row[i]);
		}
	}

	return data;
}

function renderIcon(opts, canvas) {
	opts = buildOpts(opts || {});
	var imageData = createImageData(opts.size);
	var width = Math.sqrt(imageData.length);

	canvas.width = canvas.height = 100 * 1;

	var cc = canvas.getContext('2d');
	cc.fillStyle = opts.bgcolor;
	cc.fillRect(0, 0, canvas.width, canvas.height);
	cc.fillStyle = opts.color;

	for (var i = 0; i < imageData.length; i++) {

		// if data is 0, leave the background
		if (imageData[i]) {
			var row = Math.floor(i / width);
			var col = i % width;

			// if data is 2, choose spot color, if 1 choose foreground
			cc.fillStyle = (imageData[i] == 1) ? opts.color : opts.spotcolor;

			cc.fillRect(col * opts.scale, row * opts.scale, opts.scale, opts.scale);
		}
	}
	return canvas;
}

function makeBlockie(addr) {
	//return "1243" + addr;

	const hz = renderIcon({}, {});
	console.log("test: ", hz);
	
	return `url("data:image/png;base64,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")`;
}

const container = document.getElementById('icons');
//icon.style.backgroundImage = `url(${makeBlockie(address)})`;
container.style.backgroundImage = makeBlockie(address);
//`url("data:image/png;base64,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")`;

var icon = document.createElement('div');
icon.title = address;


var title = document.createElement('h5');
title.innerHTML = address;

var div = document.createElement('div');
icon.classList.add('icon');
div.appendChild(icon);
div.appendChild(title);
div.classList.add('icon-wrapper')

// Insert
container.appendChild(div);


console.log(" _ end _");